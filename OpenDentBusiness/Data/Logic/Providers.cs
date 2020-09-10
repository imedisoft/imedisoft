using CodeBase;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class Providers
	{
		/// <summary>
		/// Checks to see if the providers passed in have term dates that occur after the date passed in.
		/// Returns a list of the ProvNums that have invalid term dates. Otherwise; empty list.
		/// </summary>
		public static List<long> GetInvalidProvsByTermDate(List<long> providerIds, DateTime dateCompare)
		{
			return 
				GetWhere(x => providerIds.Any(y => y == x.Id) && x.TerminationDate.HasValue && x.TerminationDate < dateCompare.Date).Select(x => x.Id).ToList();
		}



		#region Misc Methods

		///<summary>Checks the appointment's provider and hygienist's term dates to see if an appointment should be scheduled or marked complete.
		///Returns an empty string if the appointment does not violate the Term Date for the provider or hygienist.
		///A non-empty return value should be displayed to the user in a message box (already translated).
		///isSetComplete simply modifies the message. Use this when checking if an appointment should be set complete.</summary>
		public static string CheckApptProvidersTermDates(Appointment apt, bool isSetComplete = false)
		{
			string message = "";
			List<long> listProvNums = new List<long> { apt.ProvNum, apt.ProvHyg };
			List<long> listInvalidProvNums = Providers.GetInvalidProvsByTermDate(listProvNums, apt.AptDateTime);
			if (listInvalidProvNums.Count == 0)
			{
				return message;
			}
			if (listInvalidProvNums.Contains(apt.ProvNum))
			{
				message += "provider";
			}
			if (listInvalidProvNums.Contains(apt.ProvHyg))
			{
				if (message != "")
				{
					message += " and ";
				}
				message += "hygienist";
			}

			if (listInvalidProvNums.Contains(apt.ProvNum) && listInvalidProvNums.Contains(apt.ProvHyg))
			{
				message = "The " + message + " selected for this appointment have Term Dates prior to the selected day and time. "
					+ "Please select another " + message + (isSetComplete ? " to set the appointment complete." : ".");
			}
			else
			{
				message = "The " + message + " selected for this appointment has a Term Date prior to the selected day and time. "
					+ "Please select another " + message + (isSetComplete ? " to set the appointment complete." : ".");
			}

			return message;
		}

		#endregion


		[CacheGroup(nameof(InvalidType.Providers))]
		private class ProviderCache : ListCache<Provider>
		{
			protected override IEnumerable<Provider> Load() 
				=> SelectMany("SELECT * FROM `providers` ORDER BY `sort_order`");
        }

		private static readonly ProviderCache cache = new ProviderCache();

		public static List<Provider> GetDeepCopy(bool isShort = false)
		{
			return cache.GetAll();
		}

		public static bool GetExists(Predicate<Provider> match, bool isShort = false)
		{
			return cache.Any(match);
		}

		public static Provider GetFirst(bool isShort = false)
		{
			return cache.FirstOrDefault();
		}

		public static Provider GetFirst(Predicate<Provider> predicate, bool isShort = false)
		{
			return cache.FirstOrDefault(predicate);
		}

		public static Provider GetFirstOrDefault(Predicate<Provider> predicate, bool isShort = false)
		{
			return cache.FirstOrDefault(predicate);
		}

		public static Provider GetLastOrDefault(Predicate<Provider> predicate, bool isShort = false)
		{
			return cache.LastOrDefault(predicate);
		}

		public static List<Provider> GetWhere(Predicate<Provider> match, bool isShort = false)
		{
			return cache.Find(match);
		}

		public static void RefreshCache() 
			=> cache.Refresh();

		public static IEnumerable<Provider> GetAll() 
			=> SelectMany("SELECT * FROM `providers`");

		public static void Update(Provider provider) 
			=> ExecuteUpdate(provider);

		public static long Insert(Provider provider) 
			=> ExecuteInsert(provider);

		/// <summary>
		/// This checks for the maximum number of provnum in the database and then returns the one directly after.
		/// Not guaranteed to be a unique primary key.
		/// </summary>
		public static long GetNextAvailableProvNum() 
			=> Database.ExecuteLong("SELECT MAX(`id`) FROM `providers`") + 1;

		public static void MoveDownBelow(Provider provider) 
			=> Database.ExecuteNonQuery(
				"UPDATE `providers` SET `sort_order` = `sort_order` + 1 " +
				"WHERE `provider_id` != " + provider.Id+ " AND `sort_order` >= " + provider.SortOrder);

		public static void Delete(Provider provider) 
			=> ExecuteDelete(provider);



		///<summary>Gets table for the FormProviderSetup window.  Always orders by ItemOrder.</summary>
		public static DataTable RefreshStandard(bool canShowPatCount)
		{

			//Max function used because some providers may have multiple user names.
			string command = "SELECT Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,MAX(UserName) UserName,ProvStatus,IsHiddenReport ";
			if (canShowPatCount)
			{
				command += ",PatCountPri,PatCountSec ";
			}
			command += "FROM provider "
			+ "LEFT JOIN userod ON userod.ProvNum=provider.ProvNum ";//there can be multiple userods attached to one provider
			if (canShowPatCount)
			{
				command += "LEFT JOIN (SELECT PriProv, COUNT(*) PatCountPri FROM patient "
					+ "WHERE patient.PatStatus!=" + POut.Int((int)PatientStatus.Deleted) + " AND patient.PatStatus!=" + POut.Int((int)PatientStatus.Deceased) + " "
					+ "GROUP BY PriProv) patPri ON provider.ProvNum=patPri.PriProv  ";
				command += "LEFT JOIN (SELECT SecProv,COUNT(*) PatCountSec FROM patient "
					+ "WHERE patient.PatStatus!=" + POut.Int((int)PatientStatus.Deleted) + " AND patient.PatStatus!=" + POut.Int((int)PatientStatus.Deceased) + " "
					+ "GROUP BY SecProv) patSec ON provider.ProvNum=patSec.SecProv ";
			}
			command += "GROUP BY Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,ProvStatus,IsHiddenReport ";
			if (canShowPatCount)
			{
				command += ",PatCountPri,PatCountSec ";
			}
			command += "ORDER BY ItemOrder";
			return Database.ExecuteDataTable(command);
		}

		///<summary>Gets table for main provider edit list when in dental school mode.  Always orders alphabetically, but there will be lots of filters to get the list shorter.  Must be very fast because refreshes while typing.  selectAll will trump selectInstructors and always return all providers.</summary>
		public static DataTable RefreshForDentalSchool(long schoolClassNum, string lastName, string firstName, string provNum, bool selectInstructors, bool selectAll)
		{

			string command = "SELECT Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,GradYear,IsInstructor,Descript,"
				+ "MAX(UserName) UserName,"//Max function used for Oracle compatability (some providers may have multiple user names).
				+ "PatCountPri,PatCountSec,ProvStatus,IsHiddenReport "
				+ "FROM provider LEFT JOIN schoolclass ON provider.SchoolClassNum=schoolclass.SchoolClassNum "
				+ "LEFT JOIN userod ON userod.ProvNum=provider.ProvNum "//there can be multiple userods attached to one provider
				+ "LEFT JOIN (SELECT PriProv, COUNT(*) PatCountPri FROM patient "
					+ "WHERE patient.PatStatus!=" + POut.Int((int)PatientStatus.Deleted) + " AND patient.PatStatus!=" + POut.Int((int)PatientStatus.Deceased) + " "
					+ "GROUP BY PriProv) pat ON provider.ProvNum=pat.PriProv ";
			command += "LEFT JOIN (SELECT SecProv,COUNT(*) PatCountSec FROM patient "
				+ "WHERE patient.PatStatus!=" + POut.Int((int)PatientStatus.Deleted) + " AND patient.PatStatus!=" + POut.Int((int)PatientStatus.Deceased) + " "
				+ "GROUP BY SecProv) patSec ON provider.ProvNum=patSec.SecProv ";
			command += "WHERE TRUE ";//This is here so that we can prevent nested if-statements
			if (schoolClassNum > 0)
			{
				command += "AND provider.SchoolClassNum=" + POut.Long(schoolClassNum) + " ";
			}
			if (lastName != "")
			{
				command += "AND provider.LName LIKE '%" + POut.String(lastName) + "%' ";
			}
			if (firstName != "")
			{
				command += "AND provider.FName LIKE '%" + POut.String(firstName) + "%' ";
			}
			if (provNum != "")
			{
				command += "AND provider.ProvNum LIKE '%" + POut.String(provNum) + "%' ";
			}
			if (!selectAll)
			{
				command += "AND provider.IsInstructor=" + POut.Bool(selectInstructors) + " ";
				if (!selectInstructors)
				{
					command += "AND provider.SchoolClassNum!=0 ";
				}
			}
			command += "GROUP BY Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,GradYear,IsInstructor,Descript,PatCountPri,PatCountSec "
				+ "ORDER BY LName,FName";
			return Database.ExecuteDataTable(command);
		}

		public static List<Provider> GetInstructors() 
			=> GetWhere(x => x.IsInstructor);

		public static IEnumerable<Provider> GetChangedSince(DateTime changedSince) 
			=> SelectMany("SELECT * FROM `providers` WHERE `last_modified_date` > @date",
				new MySqlParameter("date", changedSince));


		public static string GetAbbr(long providerId, bool includeHidden = false)
		{
			var provider = cache.FirstOrDefault(x => x.Id == providerId);
			if (provider == null)
			{
				return "";
			}

			if (includeHidden)
			{
				return provider.GetAbbr();
			}

			return provider.Abbr;
		}


		public static string GetLName(long provNum, List<Provider> listProvs = null)
		{
			Provider provider;
			if (listProvs == null)
			{//Use the cache.
				provider = GetLastOrDefault(x => x.Id == provNum);
			}
			else
			{//Use the custom list passed in.
				provider = listProvs.LastOrDefault(x => x.Id == provNum);
			}
			return (provider == null ? "" : provider.LastName);
		}

		///<summary>First Last, Suffix</summary>
		public static string GetFormalName(long provNum)
		{
			//No need to check RemotingRole; no call to db.
			Provider provider = GetLastOrDefault(x => x.Id == provNum);
			string retStr = "";
			if (provider != null)
			{
				retStr = provider.FirstName + " " + provider.LastName;
				if (provider.Suffix != "")
				{
					retStr += ", " + provider.Suffix;
				}
			}
			return retStr;
		}

		/// <summary>
		/// Abbr - LName, FName (hidden).  For dental schools -- ProvNum - LName, FName (hidden).
		/// </summary>
		public static string GetLongDesc(long providerId) 
			=> cache.FirstOrDefault(x => x.Id == providerId)?.GetLongDesc() ?? "";

		public static Color GetColor(long providerId) 
			=> cache.FirstOrDefault(x => x.Id == providerId)?.Color ?? Color.White;

		public static Color GetOutlineColor(long providerId) 
			=> cache.FirstOrDefault(x => x.Id == providerId)?.ColorOutline ?? Color.Black;

		public static Provider GetById(long? providerId)
        {
			if (providerId == null)
            {
				return null;
            }

			return cache.FirstOrDefault(x => x.Id == providerId);
		}

		public static bool GetIsSec(long providerId) 
			=> GetById(providerId)?.IsSecondary ?? false;

		///<summary>Gets all providers that have the matching prov nums from ListLong.  Returns an empty list if no matches.</summary>
		public static List<Provider> GetProvsByProvNums(List<long> listProvNums, bool isShort = false)
		{
			//No need to check RemotingRole; no call to db.
			return GetWhere(x => x.Id.In(listProvNums), isShort);
		}

		/// <summary>
		/// Gets a list of providers from ListLong. 
		/// If none found or if either LName or FName are an empty string, returns an empty list. 
		/// There may be more than on provider with the same FName and LName so we will return a list of all such providers. 
		/// Usually only one will exist with the FName and LName provided so list returned will have count 0 or 1 normally. 
		/// Name match is not case sensitive.
		/// </summary>
		public static List<Provider> GetProvsByFLName(string lName, string fName)
		{
			if (string.IsNullOrWhiteSpace(lName) || string.IsNullOrWhiteSpace(fName))
			{
				return new List<Provider>();
			}

			//GetListLong already returns a copy of the prov from the cache, no need to .Copy
			return GetWhere(x => x.LastName.ToLower() == lName.ToLower() && x.FirstName.ToLower() == fName.ToLower());
		}

		///<summary>Gets a list of providers from ListLong with either the NPI provided or a blank NPI and the Medicaid ID provided.
		///medicaidId can be blank.  If the npi param is blank, or there are no matching provs, returns an empty list.
		///Shouldn't be two separate functions or we would have to compare the results of the two lists.</summary>
		public static List<Provider> GetProvsByNpiOrMedicaidId(string npi, string medicaidId)
		{
			//No need to check RemotingRole; no call to db.
			List<Provider> retval = new List<Provider>();
			if (npi == "")
			{
				return retval;
			}
			List<Provider> listProvs = Providers.GetDeepCopy();
			for (int i = 0; i < listProvs.Count; i++)
			{
				//if the prov has a NPI set and it's a match, add this prov to the list
				if (listProvs[i].NationalProviderID != "")
				{
					if (listProvs[i].NationalProviderID.Trim().ToLower() == npi.Trim().ToLower())
					{
						retval.Add(listProvs[i].Copy());
					}
				}
				else
				{//if the NPI is blank and the Medicaid ID is set and it's a match, add this prov to the list
					if (listProvs[i].MedicaidID != ""
						&& listProvs[i].MedicaidID.Trim().ToLower() == medicaidId.Trim().ToLower())
					{
						retval.Add(listProvs[i].Copy());
					}
				}
			}
			return retval;
		}

		/// <summary>
		/// Gets all providers associated to users that have a clinic set to the clinic passed in. 
		/// Passing in 0 will get a list of providers not assigned to any clinic or to any users.
		/// </summary>
		public static List<Provider> GetProvsByClinic(long clinicNum)
		{
			//No need to check RemotingRole; no call to db.
			List<Provider> listProvsWithClinics = new List<Provider>();
			List<Userod> listUsersShort = Userods.GetAll();
			for (int i = 0; i < listUsersShort.Count; i++)
			{
				if (!listUsersShort[i].ProviderId.HasValue) continue;

				Provider prov = GetById(listUsersShort[i].ProviderId.Value);
				if (prov == null)
				{
					continue;
				}
				List<UserClinic> listUserClinics = UserClinics.GetForUser(listUsersShort[i].Id);
				//If filtering by a specific clinic, make sure the clinic matches the clinic passed in.
				//If the user is associated to multiple clinics we check to make sure one of them isn't the clinic in question.
				if (clinicNum > 0 && !listUserClinics.Exists(x => x.ClinicId == clinicNum))
				{
					continue;
				}
				if (listUsersShort[i].ClinicId > 0)
				{//User is associated to a clinic, add the provider to the list of provs with clinics.
					listProvsWithClinics.Add(prov);
				}
			}
			if (clinicNum == 0)
			{//Return the list of providers without clinics.
			 //We need to find all providers not associated to a clinic (via userod) and also include all providers not even associated to a user.
			 //Since listProvsWithClinics is comprised of all providers associated to a clinic, simply loop through the provider cache and remove providers present in listProvsWithClinics.
				List<Provider> listProvsUnassigned = Providers.GetDeepCopy(true);
				for (int i = listProvsUnassigned.Count - 1; i >= 0; i--)
				{
					for (int j = 0; j < listProvsWithClinics.Count; j++)
					{
						if (listProvsWithClinics[j].Id == listProvsUnassigned[i].Id)
						{
							listProvsUnassigned.RemoveAt(i);
							break;
						}
					}
				}
				return listProvsUnassigned;
			}
			else
			{
				return listProvsWithClinics;
			}
		}

		/// <summary>
		/// Gets all providers from the database.  Doesn't use the cache.
		/// </summary>
		public static IEnumerable<Provider> GetProvsNoCache()
		{
			return SelectMany("SELECT * FROM providers");
		}

		///<summary>Gets a provider from the List.  If EcwID is not found, then it returns null.</summary>
		public static Provider GetProvByEcwID(string eID)
		{
			//No need to check RemotingRole; no call to db.
			if (eID == "")
			{
				return null;
			}
			Provider provider = GetFirstOrDefault(x => x.EcwID == eID);
			if (provider != null)
			{
				return provider;
			}
			//If using eCW, a provider might have been added from the business layer.
			//The UI layer won't know about the addition.
			//So we need to refresh if we can't initially find the prov.
			RefreshCache();
			return GetFirstOrDefault(x => x.EcwID == eID);
		}

		/// <summary>
		/// Takes a provNum. Normally returns that provNum. 
		/// If in Orion mode, returns the user's ProvNum, if that user is a primary provider. 
		/// Otherwise, in Orion Mode, returns 0.
		/// </summary>
		public static long GetOrionProvNum(long providerId)
		{
			if (Programs.UsingOrion)
			{
				var user = Security.CurrentUser;

				if (user != null && user.ProviderId.HasValue)
				{
					var provider = GetById(user.ProviderId.Value);
					if (provider != null)
					{
						if (!provider.IsSecondary)
						{
							return user.ProviderId.Value;
						}
					}
				}

				return 0;
			}

			return providerId;
		}

		/// <summary>
		/// Within the regular list of visible providers. 
		/// Will return -1 if the specified provider is not in the list.
		/// </summary>
		public static int GetIndex(long providerId)
		{
			return -1;
		}

		public static List<Userod> GetAttachedUsers(long providerId) // TODO: Move to Userods...
			=> Userods.SelectMany(
				"SELECT * FROM `users` WHERE u.`provider_id` = " + providerId).ToList();

		/// <summary>
		/// Returns the billing provnum to use based on practice/clinic settings.
		/// Takes the treating provider provnum and clinicnum.
		/// If clinics are enabled and clinicnum is passed in, the clinic's billing provider will be used.
		/// Otherwise will use pactice defaults.
		/// It will return a valid provNum unless the supplied treatProv was invalid.
		/// </summary>
		public static long GetBillingProviderId(long treatmentProviderId, long clinicId)
		{
			var clinic = Clinics.GetById(clinicId);
			if (null == clinic)
			{
				return 0;
			}

			switch (clinic.InsBillingProviderType)
			{
				case 'D':
					return Preferences.GetLong(PreferenceName.InsBillingProv);

				case 'T':
					return treatmentProviderId;

				case 'S':
					return clinic.InsBillingProviderId ?? 0;
			}

			return 0;
		}

		///<summary>Returns list of providers that are either not restricted to a clinic, or are restricted to the ClinicNum provided. Excludes hidden provs.  Passing ClinicNum=0 returns all unrestricted providers. Ordered by provider.ItemOrder.</summary>
		public static List<Provider> GetProvsForClinic(long clinicNum)
		{
			//No need to check RemotingRole; no call to db.
			if (!PrefC.HasClinicsEnabled)
			{
				return Providers.GetDeepCopy(true);//if clinics not enabled, return all visible providers.
			}
			//The GetWhere uses a "UserClinicNum>-1" in its selection to behave as a "Where true" to retrieve everything from the cache 
			Dictionary<long, List<long>> dictUserClinicsReference = UserClinics.GetWhere(x => x.Id > -1).GroupBy(x => x.UserId).ToDictionary(x => x.Key, x => x.Select(y => y.ClinicId).ToList());
			Dictionary<long, List<long>> dictUserClinics = Userods.GetAll()
				.ToDictionary(x => x.Id, x => dictUserClinicsReference.ContainsKey(x.Id) ? dictUserClinicsReference[x.Id] : new List<long>());
			Dictionary<long?, List<long>> dictProvUsers = Userods.GetWhere(x => x.ProviderId > 0).GroupBy(x => x.ProviderId)
				.ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToList());
			HashSet<long> hashSetProvsRestrictedOtherClinic = new HashSet<long>(ProviderClinicLinks.GetProvsRestrictedToOtherClinics(clinicNum));
			return Providers.GetWhere(x =>
				(!dictProvUsers.ContainsKey(x.Id) //provider not associated to any users.
				|| dictProvUsers[x.Id].Any(y => dictUserClinics[y].Count == 0) //provider associated with user not restricted to any clinics
				|| dictProvUsers[x.Id].Any(y => dictUserClinics[y].Contains(clinicNum))) //provider associated to user restricted to clinic at hand
				&& !hashSetProvsRestrictedOtherClinic.Contains(x.Id)
				, true).OrderBy(x => x.SortOrder).ToList();
		}

		///<Summary>Used once in the Provider Select window to warn user of duplicate Abbrs.</Summary>
		public static string GetDuplicateAbbrs()
		{
			string command = "SELECT Abbr FROM provider WHERE ProvStatus!=" + (int)ProviderStatus.Deleted;
			List<string> listDuplicates = Database.GetListString(command).GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
			return string.Join(",", listDuplicates);//returns empty string when listDuplicates is empty
		}

		/// <summary>
		/// Returns the default practice provider.
		/// Returns null if there is no default practice provider set.
		/// </summary>
		public static Provider GetDefaultProvider()
			=> GetDefaultProvider(Clinics.ClinicId ?? 0);

		/// <summary>
		/// Returns the default provider for the clinic if it exists, else returns the default practice provider.
		/// Pass 0 to get practice default.
		/// Can return null if no clinic or practice default provider found.
		/// </summary>
		public static Provider GetDefaultProvider(long clinicId)
		{
			var clinic = Clinics.GetById(clinicId);

			if (clinic != null && clinic.DefaultProviderId.HasValue)
			{
				var provider = GetById(clinic.DefaultProviderId.Value);

				if (provider != null)
				{
					return provider;
				}
			}

			return GetById(Preferences.GetLong(PreferenceName.PracticeDefaultProv));
		}

		public static DataTable GetDefaultPracticeProvider() 
			=> Database.ExecuteDataTable(
				"SELECT `first_name`, `last_name`, `suffix`, `state_license` " +
				"FROM `providers` WHERE `id` = " + Preferences.GetLong(PreferenceName.PracticeDefaultProv));

		/// <summary>
		/// We should merge these results with GetDefaultPracticeProvider(), but that would require
		/// restructuring indexes in different places in the code and this is faster to do as we 
		/// are just moving the queries down in to the business layer for now.
		/// </summary>
		public static DataTable GetDefaultPracticeProvider2() 
			=> Database.ExecuteDataTable(
				"SELECT `first_name`, `last_name`, `speciality` " +
				"FROM `providers` WHERE `id` = " + Preferences.GetLong(PreferenceName.PracticeDefaultProv));

		///<summary>We should merge these results with GetDefaultPracticeProvider(), but
		///that would require restructuring indexes in different places in the code and this is
		///faster to do as we are just moving the queries down in to the business layer for now.</summary>
		public static DataTable GetDefaultPracticeProvider3()
		{

			string command = @"SELECT NationalProvID " +
				"FROM provider WHERE provnum=" +
				POut.Long(Preferences.GetLong(PreferenceName.PracticeDefaultProv));
			return Database.ExecuteDataTable(command);
		}

		public static DataTable GetPrimaryProviders(long patientId)
		{
			return Database.ExecuteDataTable(
				"SELECT `first_name`, `last_name` FROM `providers` " +
				"WHERE `id` IN (SELECT priprov FROM patient WHERE patnum = " + patientId + ")");
		}

		///<summary>Returns the patient's last seen hygienist.  Returns null if no hygienist has been seen.</summary>
		public static Provider GetLastSeenHygienistForPat(long patNum)
		{

			//Look at all completed appointments and get the most recent secondary provider on it.
			string command = @"SELECT appointment.ProvHyg
				FROM appointment
				WHERE appointment.PatNum=" + POut.Long(patNum) + @"
				AND appointment.ProvHyg!=0
				AND appointment.AptStatus=" + POut.Int((int)ApptStatus.Complete) + @"
				ORDER BY AptDateTime DESC";
			List<long> listPatHygNums = Database.GetListLong(command);
			//Now that we have all hygienists for this patient.  Lets find the last non-hidden hygienist and return that one.
			List<Provider> listProviders = Providers.GetDeepCopy(true);
			List<long> listProvNums = listProviders.Select(x => x.Id).Distinct().ToList();
			long lastHygNum = listPatHygNums.FirstOrDefault(x => listProvNums.Contains(x));
			return listProviders.FirstOrDefault(x => x.Id == lastHygNum);
		}

		///<summary>Gets a list of providers based for the patient passed in based on the WebSchedProviderRule preference.</summary>
		public static List<Provider> GetProvidersForWebSched(long patNum, long clinicNum)
		{
			//No need to check RemotingRole; no call to db.
			List<Provider> listProviders = Providers.GetDeepCopy(true);
			WebSchedProviderRules providerRule = PIn.Enum<WebSchedProviderRules>(
					ClinicPrefs.GetString(clinicNum, PreferenceName.WebSchedProviderRule) ?? Preferences.GetString(PreferenceName.WebSchedProviderRule));
			switch (providerRule)
			{
				case WebSchedProviderRules.PrimaryProvider:
					Patient patPri = Patients.GetPat(patNum);
					Provider patPriProv = listProviders.Find(x => x.Id == patPri.PriProv);
					if (patPriProv == null)
					{
						throw new Exception("Invalid primary provider set for patient.");
					}
					return new List<Provider>() { patPriProv };
				case WebSchedProviderRules.SecondaryProvider:
					Patient patSec = Patients.GetPat(patNum);
					Provider patSecProv = listProviders.Find(x => x.Id == patSec.SecProv);
					if (patSecProv == null)
					{
						throw new Exception("No secondary provider set for patient.");
					}
					return new List<Provider>() { patSecProv };
				case WebSchedProviderRules.LastSeenHygienist:
					Provider lastHygProvider = GetLastSeenHygienistForPat(patNum);
					if (lastHygProvider == null)
					{
						throw new Exception("No last seen hygienist found for patient.");
					}
					return new List<Provider>() { lastHygProvider };
				case WebSchedProviderRules.FirstAvailable:
				default:
					return listProviders;
			}
		}

		///<summary>Gets a list of providers that are allowed to have new patient appointments scheduled for them.</summary>
		public static List<Provider> GetProvidersForWebSchedNewPatAppt()
		{
			//No need to check RemotingRole; no call to db.
			//Currently all providers are allowed to be considered for new patient appointments.
			//This follows the "WebSchedProviderRules.FirstAvailable" logic for recall Web Sched appointments which is what Nathan agreed upon.
			//This method is here so that we have a central location to go and get these types of providers in case we change this in the future.
			return Providers.GetWhere(x => !x.IsNotPerson, true);//Make sure that we only return not is not persons.
		}

		public static IEnumerable<long> GetChangedSinceProvNums(DateTime changedSince) 
			=> Database.SelectMany("SELECT `id` FROM `providers` WHERE `last_modified_date` > @date", Database.ToScalar<long>,
				new MySqlParameter("date", changedSince));

		public static List<Provider> GetMultProviders(IEnumerable<long> providerIds)
		{
			var providers = new List<Provider>();

			if (providerIds == null)
            {
				return providers;
			}

			var providerIdsList = providerIds.ToList();
			if (providerIdsList.Count == 0)
            {
				return providers;
			}

			providers.AddRange(SelectMany(
				"SELECT * FROM `providers` WHERE `id` IN (" + string.Join(", ", providerIdsList) + ")"));

			return providers;
		}

		/// <summary>Currently only used for Dental Schools and will only return Providers.ListShort if Dental Schools is not active.  Otherwise this will return a filtered provider list.</summary>
		public static List<Provider> GetFilteredProviderList(long provNum, string lName, string fName, long classNum)
		{
			//No need to check RemotingRole; no call to db.
			List<Provider> listProvs = Providers.GetDeepCopy(true);
			if (Preferences.GetBool(PreferenceName.EasyHideDentalSchools))
			{//This is here to save doing the logic below for users who have no way to filter the provider picker list.
				return listProvs;
			}
			for (int i = listProvs.Count - 1; i >= 0; i--)
			{
				if (provNum != 0 && !listProvs[i].Id.ToString().Contains(provNum.ToString()))
				{
					listProvs.Remove(listProvs[i]);
					continue;
				}
				if (!String.IsNullOrWhiteSpace(lName) && !listProvs[i].LastName.Contains(lName))
				{
					listProvs.Remove(listProvs[i]);
					continue;
				}
				if (!String.IsNullOrWhiteSpace(fName) && !listProvs[i].FirstName.Contains(fName))
				{
					listProvs.Remove(listProvs[i]);
					continue;
				}
				if (classNum != 0 && classNum != listProvs[i].SchoolClassId)
				{
					listProvs.Remove(listProvs[i]);
					continue;
				}
			}
			return listProvs;
		}

		///<summary>Returns a dictionary, with the key being ProvNum and the value being the production goal amount.</summary>
		public static Dictionary<long, decimal> GetProductionGoalForProviders(List<long> listProvNums, List<long> listOpNums, DateTime start, DateTime end)
		{
			//No need to check RemotingRole; no call to db.
			Dictionary<long, double> dictProvSchedHrs = Schedules.GetHoursSchedForProvsInRange(listProvNums, listOpNums, start, end);
			Dictionary<long, decimal> retVal = new Dictionary<long, decimal>();
			foreach (KeyValuePair<long, double> kvp in dictProvSchedHrs)
			{
				Provider prov = GetById(kvp.Key);
				if (prov != null)
				{
					retVal[prov.Id] = (decimal)(kvp.Value * prov.HourlyProductionGoal);
				}
			}
			return retVal;
		}

		///<summary>Removes a provider from the future schedule.  Currently called after a provider is hidden.</summary>
		public static void RemoveProvFromFutureSchedule(long provNum)
		{
			//No need to check RemotingRole; no call to db.
			if (provNum < 1)
			{//Invalid provNum, nothing to do.
				return;
			}
			List<long> provNums = new List<long>();
			provNums.Add(provNum);
			RemoveProvsFromFutureSchedule(provNums);
		}

		///<summary>Removes the providers from the future schedule.  Currently called from DBM to clean up hidden providers still on the schedule.</summary>
		public static void RemoveProvsFromFutureSchedule(List<long> provNums)
		{

			string provs = "";
			for (int i = 0; i < provNums.Count; i++)
			{
				if (provNums[i] < 1)
				{//Invalid provNum, nothing to do.
					continue;
				}
				if (i > 0)
				{
					provs += ",";
				}
				provs += provNums[i].ToString();
			}
			if (provs == "")
			{//No valid provNums were passed in.  Simply return.
				return;
			}
			string command = "SELECT ScheduleNum FROM schedule WHERE ProvNum IN (" + provs + ") AND SchedDate > " + DbHelper.Now();
			DataTable table = Database.ExecuteDataTable(command);
			List<string> listScheduleNums = new List<string>();//Used for deleting scheduleops below
			for (int i = 0; i < table.Rows.Count; i++)
			{
				//Add entry to deletedobjects table if it is a provider schedule type
				DeletedObjects.SetDeleted(DeletedObjectType.ScheduleProv, PIn.Long(table.Rows[i]["ScheduleNum"].ToString()));
				listScheduleNums.Add(table.Rows[i]["ScheduleNum"].ToString());
			}
			if (listScheduleNums.Count != 0)
			{
				command = "DELETE FROM scheduleop WHERE ScheduleNum IN(" + POut.String(String.Join(",", listScheduleNums)) + ")";
				Database.ExecuteNonQuery(command);
			}
			command = "DELETE FROM schedule WHERE ProvNum IN (" + provs + ") AND SchedDate > " + DbHelper.Now();
			Database.ExecuteNonQuery(command);
		}

		public static bool IsAttachedToUser(long provNum)
		{

			string command = "SELECT COUNT(*) FROM userod,provider "
					+ "WHERE userod.ProvNum=provider.ProvNum "
					+ "AND provider.provNum=" + POut.Long(provNum);
			int count = PIn.Int(Database.ExecuteString(command));
			if (count > 0)
			{
				return true;
			}
			return false;
		}

		///<summary>Used to check if a specialty is in use when user is trying to hide it.</summary>
		public static bool IsSpecialtyInUse(long defNum)
		{

			string command = "SELECT COUNT(*) FROM provider WHERE Specialty=" + POut.Long(defNum);
			if (Database.ExecuteString(command) == "0")
			{
				return false;
			}
			return true;
		}

		///<summary>Used to get a list of providers that are scheduled for today.  
		///Pass in specific clinicNum for providers scheduled in specific clinic, clinicNum of -1 for all clinics</summary>
		public static List<Provider> GetProvsScheduledToday(long clinicNum = -1)
		{

			List<Schedule> listSchedulesForDate = Schedules.GetAllForDateAndType(DateTime.Today, ScheduleType.Provider);
			if (PrefC.HasClinicsEnabled && clinicNum >= 0)
			{
				listSchedulesForDate.FindAll(x => x.ClinicNum == clinicNum);
			}
			List<long> listProvNums = listSchedulesForDate.Select(x => x.ProvNum).ToList();
			return Providers.GetMultProviders(listProvNums);
		}

		///<summary>Provider merge tool.  Returns the number of rows changed when the tool is used.</summary>
		public static long Merge(long provNumFrom, long provNumInto)
		{

			string[] provNumForeignKeys = new string[] { //add any new FKs to this list.
				"adjustment.ProvNum",
				"anestheticrecord.ProvNum",
				"appointment.ProvNum",
				"appointment.ProvHyg",
				"apptviewitem.ProvNum",
				"claim.ProvTreat",
				"claim.ProvBill",
				"claim.ReferringProv",
				"claim.ProvOrderOverride",
				"claimproc.ProvNum",
				"clinic.DefaultProv",
				"clinic.InsBillingProv",
				"dispsupply.ProvNum",
				"ehrnotperformed.ProvNum",
				"emailmessage.ProvNumWebMail",
				"encounter.ProvNum",
				"equipment.ProvNumCheckedOut",
				"erxlog.ProvNum",
				"evaluation.InstructNum",
				"evaluation.StudentNum",
				"fee.ProvNum",
				"intervention.ProvNum",
		"labcase.ProvNum",
				"medicalorder.ProvNum",
				"medicationpat.ProvNum",
				"operatory.ProvDentist",
				"operatory.ProvHygienist",
				"patient.PriProv",
				"patient.SecProv",
				"payplancharge.ProvNum",
		"paysplit.ProvNum",
				"perioexam.ProvNum",
				"proccodenote.ProvNum",
				"procedurecode.ProvNumDefault",
		"procedurelog.ProvNum",
				"procedurelog.ProvOrderOverride",
				"provider.ProvNumBillingOverride",
				//"providerclinic.ProvNum",
				"providerident.ProvNum",
				"refattach.ProvNum",
				"reqstudent.ProvNum",
				"reqstudent.InstructorNum",
				"rxpat.ProvNum",
				"schedule.ProvNum",
				"userod.ProvNum",
				"vaccinepat.ProvNumAdminister",
				"vaccinepat.ProvNumOrdering"
			};
			string command = "";
			long retVal = 0;
			for (int i = 0; i < provNumForeignKeys.Length; i++)
			{ //actually change all of the FKs in the above tables.
				string[] tableAndKeyName = provNumForeignKeys[i].Split(new char[] { '.' });
				command = "UPDATE " + tableAndKeyName[0]
					+ " SET " + tableAndKeyName[1] + "=" + POut.Long(provNumInto)
					+ " WHERE " + tableAndKeyName[1] + "=" + POut.Long(provNumFrom);
				retVal += Database.ExecuteNonQuery(command);
			}
			//Merge any providerclinic rows associated to the FROM provider where the INTO provider does not have a row for said clinic.
			List<ProviderClinic> listProviderClinicsAll = ProviderClinics.GetByProvNums(new List<long>() { provNumFrom, provNumInto });
			List<ProviderClinic> listProviderClinicsFrom = listProviderClinicsAll.FindAll(x => x.ProviderId == provNumFrom);
			List<ProviderClinic> listProviderClinicsInto = listProviderClinicsAll.FindAll(x => x.ProviderId == provNumInto);
			List<long> listProviderClinicNums = listProviderClinicsFrom.Where(x => !x.ClinicId.In(listProviderClinicsInto.Select(y => y.ClinicId)))
				.Select(x => x.Id)
				.ToList();
			if (!listProviderClinicNums.IsNullOrEmpty())
			{
				command = $@"UPDATE providerclinic SET ProvNum = {POut.Long(provNumInto)}
					WHERE ProviderClinicNum IN({string.Join(",", listProviderClinicNums.Select(x => POut.Long(x)))})";
				Database.ExecuteNonQuery(command);
			}
			command = "UPDATE provider SET IsHidden=1 WHERE ProvNum=" + POut.Long(provNumFrom);
			Database.ExecuteNonQuery(command);
			command = "UPDATE provider SET ProvStatus=" + POut.Int((int)ProviderStatus.Deleted) + " WHERE ProvNum=" + POut.Long(provNumFrom);
			Database.ExecuteNonQuery(command);
			return retVal;
		}

		public static long CountPats(long provNum)
		{

			string command = "SELECT COUNT(DISTINCT patient.PatNum) FROM patient WHERE (patient.PriProv=" + POut.Long(provNum)
				+ " OR patient.SecProv=" + POut.Long(provNum) + ")"
				+ " AND patient.PatStatus=0";
			string retVal = Database.ExecuteString(command);
			return PIn.Long(retVal);
		}

		public static long CountClaims(long provNum)
		{

			string command = "SELECT COUNT(DISTINCT claim.ClaimNum) FROM claim WHERE claim.ProvBill=" + POut.Long(provNum)
				+ " OR claim.ProvTreat=" + POut.Long(provNum);
			string retVal = Database.ExecuteString(command);
			return PIn.Long(retVal);
		}

		///<summary>Gets the 0 provider for reports that display payments.</summary>
		public static Provider GetUnearnedProv()
		{
			return new Provider()
			{
				Id = 0,
				Abbr = "No Provider"
			};
		}

		///<summary>Only for reports. Includes all providers where IsHiddenReport = 0 and ProvStatus != Deleted.</summary>
		public static List<Provider> GetListReports()
		{
			return GetWhere(x => !x.IsHiddenReport && x.Status != ProviderStatus.Deleted);
		}
	}
}
