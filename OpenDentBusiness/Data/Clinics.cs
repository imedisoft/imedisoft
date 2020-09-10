using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.X12.Codes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OpenDentBusiness
{
    /// <summary>
    /// Use ListLong or ListShort to get a cached list of clinics that you can then filter upon.
    /// </summary>
    public partial class Clinics
	{
		/// <summary>
		/// Returns a list of clinics that are associated to any clones of the master patient passed in (patNumFrom).
		/// This method will include the clinic for the master patient passed in.
		/// </summary>
		public static IEnumerable<Clinic> GetClinicsForClones(long fromPatientId)
		{
			// Always include the master patient's clinic.
			var patientCloneIds = new List<long>() { fromPatientId };

			// Get all clones (PatNumTos) that are associated to the master patient passed in (patNumFrom).
			patientCloneIds.AddRange(PatientLinks.GetPatNumsLinkedFrom(fromPatientId, PatientLinkType.Clone));

			// Get the clinics associated to all of the clones for this patient.
			return SelectMany(
				"SELECT * FROM `clinics` " +
				"INNER JOIN patient ON `clinics`.`id` = patient.ClinicNum " +
				"WHERE patient.PatNum IN (" + string.Join(",", patientCloneIds) + ") " +
				"GROUP BY `clinics`.`id`");
		}

		/// <summary>
		/// Returns a dictionary such that the key is a clinicNum and the value is a count of patients whith a matching patient.ClinicNum.
		/// Excludes all patients with PatStatus of Deleted, Archived, Deceased, or NonPatient unless IsAllStatuses is set to true.
		/// </summary>
		public static Dictionary<long, int> GetClinicalPatientCount(bool IsAllStatuses = false)
		{
			string command = "SELECT ClinicNum, COUNT(*) AS Count FROM patient ";

			if (!IsAllStatuses)
			{
				command += 
					"WHERE PatStatus NOT IN (" + 
						(int)PatientStatus.Deleted + "," + 
						(int)PatientStatus.Archived + "," + 
						(int)PatientStatus.Deceased + "," + 
						(int)PatientStatus.NonPatient + ") ";
			}

			command += "GROUP BY ClinicNum";

			return Database.ExecuteDataTable(command).Select().ToDictionary(
				x => PIn.Long(x["ClinicNum"].ToString()), 
				x => PIn.Int(x["Count"].ToString()));
		}

		/// <summary>
		/// Gets a list of Clinics for a given pharmacy with the specified ID.
		/// </summary>
		/// <param name="pharmacyId">The primary key of the pharmacy.</param>
		public static List<Clinic> GetClinicsForPharmacy(long pharmacyId)
		{
			var dictionary = GetDictClinicsForPharmacy(pharmacyId);

            if (!dictionary.TryGetValue(pharmacyId, out var clinics))
            {
                clinics = new List<Clinic>();
            }

            return clinics;
		}

		/// <summary>
		/// Gets a SerializableDictionary of Lists of Clinics for given pharmacyNums.
		/// </summary>
		/// <param name="pharmacyIds">The primary keys of the pharmacies.</param>
		public static Dictionary<long, List<Clinic>> GetDictClinicsForPharmacy(params long[] pharmacyIds)
		{
			var clinicsByPharmacy = new Dictionary<long, List<Clinic>>();
			if (pharmacyIds.Length == 0)
			{
				return clinicsByPharmacy;
			}

			string command = 
				"SELECT pharmclinic.PharmacyNum, `clinics`.* " +
				"FROM `clinics` " +
				"INNER JOIN pharmclinic ON pharmclinic.ClinicNum = `clinics`.`id` " +
				"WHERE pharmclinic.PharmacyNum IN (" + string.Join(", ", pharmacyIds) + ") " +
				"ORDER BY `clinics`.`abbr`";

			Database.ExecuteReader(command, dataReader =>
			{
				var pharmacyId = (long)dataReader[0];

				if (!clinicsByPharmacy.TryGetValue(pharmacyId, out var clinics))
                {
					clinics = new List<Clinic>();
					clinicsByPharmacy.Add(pharmacyId, clinics);
                }

				clinics.Add(FromReader(dataReader));
			});

			return clinicsByPharmacy;
		}

		/// <summary>
		/// The ID of the active clinic.
		/// </summary>
		private static long? activeClinicId = null;

		[CacheGroup(nameof(InvalidType.Providers))]
		private class ClinicCache : ListCache<Clinic>
		{
			protected override IEnumerable<Clinic> Load() 
				=> SelectMany("SELECT * FROM `clinics` ORDER BY `abbr`");
        }

		private static readonly ClinicCache cache = new ClinicCache();

		public static List<Clinic> GetAll(bool includeHidden)
			=> includeHidden ? 
				cache.GetAll() : 
				cache.Find(x => !x.IsHidden).ToList();

		public static int Count(bool includeHidden)
			=> includeHidden ? 
				cache.Count() : 
				cache.Count(x => !x.IsHidden);

		public static Clinic First(bool includeHidden) 
			=> includeHidden ?
				cache.GetAll().First() :
				cache.Find(x => !x.IsHidden).First();

		public static Clinic FirstOrDefault(Predicate<Clinic> predicate, bool includeHidden) 
			=> includeHidden ?
				cache.FirstOrDefault(predicate) :
				cache.Find(x => !x.IsHidden).FirstOrDefault(x => predicate(x));

		public static List<Clinic> Where(Predicate<Clinic> predicate)
			=> cache.Find(predicate).ToList();

		public static void RefreshCache() 
			=> cache.Refresh();

		public static long Insert(Clinic clinic) 
			=> ExecuteInsert(clinic);

		/// <summary>
		/// Gets or sets the ID of the active clinic.
		/// </summary>
		public static long? ClinicId
		{
			get => activeClinicId;
			set
			{
				if (activeClinicId == value)
				{
					return;
				}

				activeClinicId = value;
				if (Security.CurrentUser == null)
				{
					return;
				}

				if (Preferences.GetString(PreferenceName.ClinicTrackLast) == "User")
				{
					UserPreference.Set(UserPreferenceName.ClinicLast, value ?? 0);
				}
			}
		}

		/// <summary>
		/// Gets the active clinic.
		/// </summary>
		public static Clinic Active 
			=> ClinicId.HasValue ? GetById(ClinicId.Value) : null;

		///<summary>Sets Clinics.ClinicNum. Used when logging on to determines what clinic to start with based on user and workstation preferences.</summary>
		public static void LoadActiveClinicForUser()
		{
			activeClinicId = 0;//aka headquarters clinic when clinics are enabled.
			if (Security.CurrentUser == null)
			{
				return;
			}

			var clinics = GetByUser(Security.CurrentUser);
			switch (Preferences.GetString(PreferenceName.ClinicTrackLast))
			{
				case "Workstation":
					if (Security.CurrentUser.ClinicIsRestricted && Security.CurrentUser.ClinicId != ComputerPrefs.LocalComputer.ClinicNum)
					{//The user is restricted and it's not the clinic this computer has by default
					 //User's default clinic isn't the LocalComputer's clinic, see if they have access to the Localcomputer's clinic, if so, use it.
						Clinic clinic = clinics.Find(x => x.Id == ComputerPrefs.LocalComputer.ClinicNum);
						if (clinic != null)
						{
							activeClinicId = clinic.Id;
						}
						else
						{
							activeClinicId = Security.CurrentUser.ClinicId;//Use the user's default clinic if they don't have access to LocalComputer's clinic.
						}
					}
					else
					{//The user is not restricted, just use the clinic in the ComputerPref table.
						activeClinicId = ComputerPrefs.LocalComputer.ClinicNum;
					}
					return;//Error

				case "User":
					long clinicId = UserPreference.GetLong(UserPreferenceName.ClinicLast);

					if (clinicId == 0 && Security.CurrentUser.ClinicId.HasValue)
					{
						clinicId = Security.CurrentUser.ClinicId.Value;

						UserPreference.Set(UserPreferenceName.ClinicLast, clinicId);
					}

					if (clinics.Any(x => x.Id == clinicId)) activeClinicId = clinicId;
					return;

				case "None":
				default:
					if (clinics.Any(x => x.Id == Security.CurrentUser.ClinicId))
					{
						activeClinicId = Security.CurrentUser.ClinicId;
					}
					break;
			}
		}

		/// <summary>
		/// Called when the user is logging of or is closing the program.
		/// </summary>
		public static void LogOff()
		{
			switch (Preferences.GetString(PreferenceName.ClinicTrackLast))
			{
				case "Workstation":
					ComputerPrefs.LocalComputer.ClinicNum = ClinicId ?? 0;
					ComputerPrefs.Update(ComputerPrefs.LocalComputer);
					break;

				case "User":
				case "None":

				default:
					break;
			}

			UserPreference.Set(UserPreferenceName.ClinicLast, ClinicId ?? 0);

			activeClinicId = 0;
		}

		public static void Update(Clinic clinic) 
			=> ExecuteUpdate(clinic);

		/// <summary>
		///		<para>
		///			Helper method used when determining whether a clinic is in use.
		///		</para>
		///		<para>
		///			The specified SQL command should return the 'names' of the objects that are 
		///			using the clinic. If the command returns no results the clinic is not 
		///			considered in use.
		///		</para>
		///		<para>
		///			If the clinic is in use, <paramref name="summary"/> will hold a summary of
		///			the objects that are using the clinic. If not in use, 
		///			<paramref name="summary"/> will contain an empty string.
		///		</para>
		/// </summary>
		/// <param name="command">The SQL command to execute.</param>
		/// <param name="summary">A summary of the objects that are using the clinic.</param>
		/// <returns>True if the clinic is in use; otherwise, false.</returns>
		private static bool IsInUse(string command, out string summary)
		{
			summary = "";

			var results = Database.SelectMany(command, Database.ToScalar<string>).ToList();
			if (results.Count > 0)
			{
				for (int i = 0; i < results.Count; i++)
				{
					if (i == 15)
					{
						summary += $"\r\n" + string.Format(Imedisoft.Translation.Common.AndXOthers, results.Count - 1); ;
					}

					summary += "\r\n" + results[i];
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Executes the specified command to determine whether a clinic is in use. Throws an
		/// exception if the clinic is in use.
		/// </summary>
		/// <param name="command">The SQL command to execute.</param>
		/// <param name="error">The error message to include in the exception when the clinic is in use.</param>
		private static void EnsureNotInUse(string command, string error)
        {
			if (IsInUse(command, out var summary))
			{
				throw new Exception(error + "\r\n" + summary);
			}
		}

		/// <summary>
		///	Deletes the specified <paramref name="clinic"/> from the database. Throws an exception
		///	if the clinic is in use and cannot be deleted.
		/// </summary>
		/// <exception cref="Exception">When the clinic is in use.</exception>
		public static void Delete(Clinic clinic)
		{
			EnsureNotInUse(
				"SELECT CONCAT(LName, ', ', FName) FROM patient WHERE ClinicNum =" + clinic.Id,
                Imedisoft.Translation.Common.CannotDeleteClinicInUseByPatients);

			EnsureNotInUse(
				"SELECT CONCAT(patient.LName, ', ', patient.FName) " +
				"FROM patient,payment " +
				"WHERE payment.ClinicNum =" + clinic.Id + " AND patient.PatNum=payment.PatNum",
                Imedisoft.Translation.Common.CannotDeleteClinicInUseByPayments);

			EnsureNotInUse(
				"SELECT CONCAT(patient.LName, ', ', patient.FName) " +
				"FROM patient,claimproc,claimpayment " +
				"WHERE claimpayment.ClinicNum =" + clinic.Id + " " +
				"AND patient.PatNum=claimproc.PatNum " +
				"AND claimproc.ClaimPaymentNum=claimpayment.ClaimPaymentNum " +
				"GROUP BY patient.LName,patient.FName,claimpayment.ClaimPaymentNum",
				Imedisoft.Translation.Common.CannotDeleteClinicInUseByClaimPayments);

			EnsureNotInUse(
				"SELECT CONCAT(patient.LName, ', ', patient.FName) " +
				"FROM patient,appointment " +
				"WHERE appointment.ClinicNum =" + clinic.Id + " " +
				"AND patient.PatNum=appointment.PatNum",
				Imedisoft.Translation.Common.CannotDeleteClinicInUseByAppointments);

			EnsureNotInUse(
				"SELECT OpName FROM operatory WHERE ClinicNum =" + clinic.Id,
				Imedisoft.Translation.Common.CannotDeleteClinicInUseByOperatories);

			EnsureNotInUse(
				"SELECT UserName FROM userod WHERE ClinicNum =" + clinic.Id,
				Imedisoft.Translation.Common.CannotDeleteClinicInUseByUsers);

			EnsureNotInUse(
				"SELECT userod.UserName FROM userclinic " +
				"INNER JOIN userod ON userclinic.UserNum=userod.UserNum " +
				"WHERE userclinic.ClinicNum=" + clinic.Id,
				Imedisoft.Translation.Common.CannotDeleteClinicInUseByClinicRestrictedUsers);

			//reassign procedure.ClinicNum=0 if the procs are status D.
			var procedureIds = Database.GetListLong("SELECT ProcNum FROM procedurelog WHERE ProcStatus=" + (int)ProcStat.D + " AND ClinicNum=" + clinic.Id);
			if (procedureIds.Count > 0)
			{
				Database.ExecuteNonQuery(
					"UPDATE procedurelog SET ClinicNum=0 WHERE ProcNum IN (" + string.Join(",", procedureIds) + ")");
			}

			EnsureNotInUse(
				"SELECT CONCAT(patient.LName, ', ', patient.FName) FROM patient,procedurelog " +
				"WHERE procedurelog.ClinicNum =" + clinic.Id + " " +
				"AND patient.PatNum=procedurelog.PatNum",
				Imedisoft.Translation.Common.CannotDeleteClinicInUseByProcedures);

			// Clinic is not being used, OK to delete.

			// Delete clinic specific program properties.
			Database.ExecuteNonQuery("DELETE FROM programproperty WHERE ClinicNum=" + clinic.Id + " AND ClinicNum!=0");

			ExecuteDelete(clinic);
		}

		/// <summary>
		/// Returns a list of clinicNums with the specified regions' DefNums.
		/// </summary>
		public static List<long> GetListByRegion(List<long> regionDefIds) 
			=> Where(x => !x.IsHidden && x.Region.HasValue && regionDefIds.Contains(x.Region.Value)).Select(x => x.Id).Distinct().ToList();

		/// <summary>
		/// Gets the clinic with the specified ID.
		/// </summary>
		/// <param name="clinicId">The ID of the clinic.</param>
		/// <returns>The clinic with the specified ID.</returns>
		public static Clinic GetById(long clinicId) 
			=> FirstOrDefault(x => x.Id == clinicId, false);

		/// <summary>
		/// Gets the clinic with the specified ID. Retrieves the clinic from the database.
		/// </summary>
		/// <param name="clinicId">The ID of the clinic.</param>
		/// <returns>The clinic with the specified ID.</returns>
		[Obsolete("Use SelectOne() instead.")]
		public static Clinic GetByIdNoCache(long clinicId) 
			=> SelectOne(clinicId);

		/// <summary>
		/// Pulls from cache.  Can contain a null clinic if not found. Includes hidden clinics.
		/// </summary>
		public static List<Clinic> GetClinics(List<long> clinicIds) 
			=> clinicIds.Select(clinicId => GetById(clinicId)).ToList();

		///<summary>Returns the patient's clinic based on the recall passed in.
		///If the patient is no longer associated to a clinic, 
		///  returns the clinic associated to the appointment (scheduled or completed) with the largest date.
		///Returns null if the patient doesn't have a clinic or if the clinics feature is not activate.</summary>
		public static Clinic GetClinicForRecall(long recallNum)
		{
			if (!PrefC.HasClinicsEnabled)
			{
				return null;
			}

			string command = "SELECT patient.ClinicNum FROM patient "
				+ "INNER JOIN recall ON patient.PatNum=recall.PatNum "
				+ "WHERE recall.RecallNum=" + POut.Long(recallNum) + " "
				+ DbHelper.LimitAnd(1);
			long patientClinicNum = Database.ExecuteLong(command);
			if (patientClinicNum > 0)
			{
				return FirstOrDefault(x => x.Id == patientClinicNum, true);
			}

			//Patient does not have an assigned clinic.  Grab the clinic from a scheduled or completed appointment with the largest date.
			command = @"SELECT appointment.ClinicNum,appointment.AptDateTime 
				FROM appointment
				INNER JOIN recall ON appointment.PatNum=recall.PatNum AND recall.RecallNum=" + POut.Long(recallNum) + @"
				WHERE appointment.AptStatus IN (" + POut.Int((int)ApptStatus.Scheduled) + "," + POut.Int((int)ApptStatus.Complete) + ")" + @"
				ORDER BY AptDateTime DESC";
			command = DbHelper.LimitOrderBy(command, 1);
			long appointmentClinicNum = Database.ExecuteLong(command);
			if (appointmentClinicNum > 0)
			{
				return FirstOrDefault(x => x.Id == appointmentClinicNum, true);
			}

			return null;
		}

		/// <summary>
		/// Gets a list of all clinics. Doesn't use the cache. Includes hidden clinics.
		/// </summary>
		public static IEnumerable<Clinic> GetClinicsNoCache() 
			=> SelectMany("SELECT * FROM `clinics`");

		public static string GetDescription(long clinicId, List<Clinic> clinics = null)
		{
			var clinic = clinics?.FirstOrDefault(x => x.Id == clinicId) ??
				FirstOrDefault(x => x.Id == clinicId, true);

			return clinic?.Description ?? "";
		}

		public static string GetAbbr(long clinicId, List<Clinic> clinics = null)
		{
			var clinic = clinics?.FirstOrDefault(x => x.Id == clinicId) ??
				FirstOrDefault(x => x.Id == clinicId, true);

			return clinic?.Abbr ?? "";
		}

		/// <summary>
		/// Gets the place of service for the clinic with the specified ID.
		/// </summary>
		/// <param name="clinicId">The ID of the clinic.</param>
		/// <returns>The place of the service for the specified clinic.</returns>
		public static string GetPlaceService(long clinicId)
		{
			var clinic = FirstOrDefault(x => x.Id == clinicId, true);

			return clinic?.DefaultPlaceOfService ?? 
				Preferences.GetString(PreferenceName.DefaultProcedurePlaceService, PlaceOfService.Office);
		}

		/// <summary>
		/// Used by HL7 when parsing incoming messages.  
		/// Returns the ClinicNum of the clinic with Description matching exactly (not case sensitive) the description provided.  
		/// Returns 0 if no clinic is found with this exact description.  
		/// If there is more than one clinic with the same description, this will look for non-hidden ones first, or return the first one in the list.
		/// </summary>
		public static Clinic GetByDescription(string description)
		{
			var clinic = cache.FirstOrDefault(
				x => !x.IsHidden && x.Description.Equals(description, StringComparison.InvariantCultureIgnoreCase));
			
			if (clinic == null)
            {
				clinic = cache.FirstOrDefault(
					x => x.Description.Equals(description, StringComparison.InvariantCultureIgnoreCase));
			}

			return clinic;
		}

		/// <summary>
		/// Gets all the clinics the specified <paramref name="user"/> has permission to access.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns>All clinics the user has permission to access.</returns>
		public static List<Clinic> GetByUser(Userod user)
		{
			var clinics = GetAll(false);

			if (user.ClinicIsRestricted && user.ClinicId != 0)
			{
				var userClinics = UserClinics.GetForUser(user.Id).Select(x => x.ClinicId).ToList();

				return clinics.FindAll(x => userClinics.Contains(x.Id)).ToList();
			}

			return clinics;
		}

		public static List<Clinic> GetByCurrentUser()
			=> GetByUser(Security.CurrentUser);

		/// <summary>
		/// Determines whether the provider with the specified ID is the default provider for any clinic (includes hidden clinics).
		/// </summary>
		public static bool IsDefaultClinicProvider(long providerId) 
			=> FirstOrDefault(x => x.DefaultProviderId == providerId, true) != null;

		/// <summary>
		/// This method returns true if the given provider is set as the default ins billing provider for any clinic. Includes hidden Clinics.
		/// </summary>
		public static bool IsInsBillingProvider(long providerId) 
			=> FirstOrDefault(x => x.InsBillingProviderId == providerId, true) != null;

		/// <summary>
		/// Gets the default clinic for texting. Returns null if no clinic is set as default.
		/// </summary>
		public static Clinic GetDefaultForTexting() 
			=> FirstOrDefault(x => x.Id == Preferences.GetLong(PreferenceName.TextingDefaultClinicNum), true);

		public static bool IsTextingEnabled(long clinicId)
		{
			if (clinicId == 0) clinicId = Preferences.GetLong(PreferenceName.TextingDefaultClinicNum);

			var clinic = GetById(clinicId);
			if (clinic == null)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Provide the currently selected clinic num (FormOpenDental.ClinicNum).
		/// If clinics are not enabled, this will return true if the pref PracticeIsMedicalOnly is true.
		/// If clinics are enabled, this will return true if either the headquarters 'clinic' is selected
		/// (FormOpenDental.ClinicNum=0) and the pref PracticeIsMedicalOnly is true OR if the currently selected clinic's IsMedicalOnly flag is true.
		/// Otherwise returns false.
		/// </summary>
		public static bool IsMedicalClinic(long? clinicId)
        {
			if (!clinicId.HasValue)
            {
				return false;
            }

			return GetById(clinicId.Value)?.IsMedicalOnly ?? false;
		}

		/// <summary>
		/// Returns a clinic object with ClinicNum=0, and values filled using practice level preferences. 
		/// Caution: do not attempt to save the clinic back to the DB. This should be used for read only purposes.
		/// </summary>
		public static Clinic GetPracticeAsClinicZero(string clinicName = null)
		{
			if (clinicName == null)
			{
				clinicName = Preferences.GetString(PreferenceName.PracticeTitle);
			}

			return new Clinic
			{
				Id = 0,
				Abbr = clinicName,
				Description = clinicName,
				AddressLine1 = Preferences.GetString(PreferenceName.PracticeAddress),
				AddressLine2 = Preferences.GetString(PreferenceName.PracticeAddress2),
				City = Preferences.GetString(PreferenceName.PracticeCity),
				State = Preferences.GetString(PreferenceName.PracticeST),
				Zip = Preferences.GetString(PreferenceName.PracticeZip),
				BillingAddressLine1 = Preferences.GetString(PreferenceName.PracticeBillingAddress),
				BillingAddressLine2 = Preferences.GetString(PreferenceName.PracticeBillingAddress2),
				BillingCity = Preferences.GetString(PreferenceName.PracticeBillingCity),
				BillingState = Preferences.GetString(PreferenceName.PracticeBillingST),
				BillingZip = Preferences.GetString(PreferenceName.PracticeBillingZip),
				PayToAddressLine1 = Preferences.GetString(PreferenceName.PracticePayToAddress),
				PayToAddressLine2 = Preferences.GetString(PreferenceName.PracticePayToAddress2),
				PayToCity = Preferences.GetString(PreferenceName.PracticePayToCity),
				PayToState = Preferences.GetString(PreferenceName.PracticePayToST),
				PayToZip = Preferences.GetString(PreferenceName.PracticePayToZip),
				Phone = Preferences.GetString(PreferenceName.PracticePhone),
				BankNumber = Preferences.GetString(PreferenceName.PracticeBankNumber),
				DefaultPlaceOfService = Preferences.GetString(PreferenceName.DefaultProcedurePlaceService, PlaceOfService.Office),
				InsBillingProviderId = Preferences.GetLong(PreferenceName.InsBillingProv),
				Fax = Preferences.GetString(PreferenceName.PracticeFax),
				EmailAddressId = Preferences.GetLong(PreferenceName.EmailDefaultAddressNum),
				DefaultProviderId = Preferences.GetLong(PreferenceName.PracticeDefaultProv),
				IsMedicalOnly = Preferences.GetBool(PreferenceName.PracticeIsMedicalOnly)
			};
		}

		///<summary>Replaces all clinic fields in the given message with the supplied clinic's information.  Returns the resulting string.
		///Will use clinic information when available, otherwise defaults to practice info.
		///Replaces: [OfficePhone], [OfficeFax], [OfficeName], [OfficeAddress], and possibly [EmailDisclaimer]. </summary>
		public static string ReplaceOffice(string message, Clinic clinic, bool isHtmlEmail = false, bool doReplaceDisclaimer = false)
		{
			StringBuilder template = new StringBuilder(message);
			ReplaceOffice(template, clinic, isHtmlEmail, doReplaceDisclaimer);
			return template.ToString();
		}

		///<summary>Replaces all clinic fields in the given message with the supplied clinic's information.  Returns the resulting string.
		///Will use clinic information when available, otherwise defaults to practice info.
		///Replaces: [OfficePhone], [OfficeFax], [OfficeName], [OfficeAddress], and possibly [EmailDisclaimer]. </summary>
		public static void ReplaceOffice(StringBuilder template, Clinic clinic, bool isHtmlEmail = false, bool doReplaceDisclaimer = false)
		{
			string officePhone = Preferences.GetString(PreferenceName.PracticePhone);
			string officeFax = Preferences.GetString(PreferenceName.PracticeFax);
			string officeName = Preferences.GetString(PreferenceName.PracticeTitle);
			string officeAddr = Patients.GetAddressFull(
				Preferences.GetString(PreferenceName.PracticeAddress),
				Preferences.GetString(PreferenceName.PracticeAddress2),
				Preferences.GetString(PreferenceName.PracticeCity),
				Preferences.GetString(PreferenceName.PracticeST),
				Preferences.GetString(PreferenceName.PracticeZip));
			if (clinic != null && !String.IsNullOrEmpty(clinic.Phone))
			{
				officePhone = clinic.Phone;
			}
			if (clinic != null && !String.IsNullOrEmpty(clinic.Fax))
			{
				officeFax = clinic.Fax;
			}
			if (clinic != null && !String.IsNullOrEmpty(clinic.Description))
			{
				officeName = clinic.Description;
			}
			if (clinic != null && !String.IsNullOrEmpty(clinic.AddressLine1))
			{
				officeAddr = Patients.GetAddressFull(clinic.AddressLine1, clinic.AddressLine2, clinic.City, clinic.State, clinic.Zip);
			}
			if (CultureInfo.CurrentCulture.Name == "en-US" && officePhone.Length == 10)
			{
				officePhone = "(" + officePhone.Substring(0, 3) + ")" + officePhone.Substring(3, 3) + "-" + officePhone.Substring(6);
			}
			if (CultureInfo.CurrentCulture.Name == "en-US" && officeFax.Length == 10)
			{
				officeFax = "(" + officeFax.Substring(0, 3) + ")" + officeFax.Substring(3, 3) + "-" + officeFax.Substring(6);
			}
			ReplaceTags.ReplaceOneTag(template, "[OfficePhone]", officePhone, isHtmlEmail);
			ReplaceTags.ReplaceOneTag(template, "[OfficeFax]", officeFax, isHtmlEmail);
			ReplaceTags.ReplaceOneTag(template, "[OfficeName]", officeName, isHtmlEmail);
			ReplaceTags.ReplaceOneTag(template, "[OfficeAddress]", officeAddr, isHtmlEmail);
			if (doReplaceDisclaimer)
			{
				ReplaceTags.ReplaceOneTag(template, "[EmailDisclaimer]", OpenDentBusiness.EmailMessages.GetEmailDisclaimer(clinic?.Id ?? 0), isHtmlEmail);
			}
		}

		public static string GetOfficeName(Clinic clinic)
		{
			string officeName = clinic?.Description;
			if (string.IsNullOrEmpty(officeName))
			{
				officeName = Preferences.GetString(PreferenceName.PracticeTitle);
			}
			return officeName;
		}

		public static string GetOfficePhone(Clinic clinic)
		{
			string officePhone = clinic?.Phone;
			if (string.IsNullOrEmpty(officePhone))
			{
				officePhone = Preferences.GetString(PreferenceName.PracticePhone);
			}
			return officePhone;
		}
	}
}
