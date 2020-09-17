using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using OpenDentBusiness.Eclaims;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace Imedisoft.Data
{
	public partial class ProcedureCodes
	{
		[CacheGroup(nameof(InvalidType.ProcCodes))]
        private class ProcedureCodeCache : DictionaryCache<string, ProcedureCode>
        {
			protected override string GetKey(ProcedureCode item)
				=> item.Code;

			protected override IEnumerable<ProcedureCode> Load() 
				=> SelectMany("SELECT * FROM `proedure_codes` ORDER BY `procedure_category`, `code`");
        }

        private static readonly ProcedureCodeCache cache = new ProcedureCodeCache();

		public static List<ProcedureCode> GetListDeep() 
			=> cache.GetAll();

		public static int GetCount(bool isShort = false) 
			=> cache.Count();

		public static ProcedureCode GetOne(string procedureCode) 
			=> cache.Find(procedureCode);

		public static ProcedureCode GetFirstOrDefault(Predicate<ProcedureCode> match, bool isShort = false) 
			=> cache.FirstOrDefault(match);

		public static ProcedureCode GetFirstOrDefaultFromList(Predicate<ProcedureCode> match, bool isShort = false) 
			=> cache.FirstOrDefault(match);

		public static List<ProcedureCode> GetWhere(Predicate<ProcedureCode> match, bool isShort = false) 
			=> cache.Find(match);

		public static List<ProcedureCode> GetWhereFromList(Predicate<ProcedureCode> match, bool isShort = false) 
			=> cache.Find(match);

		public static bool GetContainsKey(string procCode) 
			=> cache.Contains(procCode);

		public static List<ProcedureCode> GetProcCodeStartsWith(string procedureCodeStart) 
			=> cache.Find(x => x.Code.StartsWith(procedureCodeStart));

		public static void RefreshCache()
			=> cache.Refresh();

		#region Additional Lists

		///<summary>Returns a list of CodeNums for specific BW procedure codes.
		///There are several places in the program that need a BW group.  E.g. when computing limitations.</summary>
		public static List<long> ListBWCodeNums
		{
			get
			{
				return GetCodeNumsForPref(PreferenceName.InsBenBWCodes);
			}
		}

		///<summary>Returns a list of CodeNums for specific Exam procedure codes.
		///There are several places in the program that need a Exam group.  E.g. when computing limitations.</summary>
		public static List<long> ListExamCodeNums
		{
			get
			{
				return GetCodeNumsForPref(PreferenceName.InsBenExamCodes);
			}
		}

		///<summary>Returns a list of CodeNums for specific PanoFMX procedure codes.
		///There are several places in the program that need a PanoFMX group.  E.g. when computing limitations.</summary>
		public static List<long> ListPanoFMXCodeNums
		{
			get
			{
				return GetCodeNumsForPref(PreferenceName.InsBenPanoCodes);
			}
		}

		///<summary>Returns a list of CodeNums for specific BW procedure codes.
		///There are several places in the program that need a BW group.  E.g. when computing limitations.</summary>
		public static List<long> ListCancerScreeningCodeNums
		{
			get
			{
				return GetCodeNumsForPref(PreferenceName.InsBenCancerScreeningCodes);
			}
		}

		///<summary>Returns a list of CodeNums for specific Exam procedure codes.
		///There are several places in the program that need a Exam group.  E.g. when computing limitations.</summary>
		public static List<long> ListProphyCodeNums
		{
			get
			{
				return GetCodeNumsForPref(PreferenceName.InsBenProphyCodes);
			}
		}

		///<summary>Returns a list of CodeNums for specific PanoFMX procedure codes.
		///There are several places in the program that need a PanoFMX group.  E.g. when computing limitations.</summary>
		public static List<long> ListFlourideCodeNums
		{
			get
			{
				return GetCodeNumsForPref(PreferenceName.InsBenFlourideCodes);
			}
		}

		///<summary>Returns a list of CodeNums for specific BW procedure codes.
		///There are several places in the program that need a BW group.  E.g. when computing limitations.</summary>
		public static List<long> ListSealantCodeNums
		{
			get
			{
				return GetCodeNumsForPref(PreferenceName.InsBenSealantCodes);
			}
		}

		///<summary>Returns a list of CodeNums for specific Exam procedure codes.
		///There are several places in the program that need a Exam group.  E.g. when computing limitations.</summary>
		public static List<long> ListCrownCodeNums
		{
			get
			{
				return GetCodeNumsForPref(PreferenceName.InsBenCrownCodes);
			}
		}

		///<summary>Returns a list of CodeNums for specific PanoFMX procedure codes.
		///There are several places in the program that need a PanoFMX group.  E.g. when computing limitations.</summary>
		public static List<long> ListSRPCodeNums
		{
			get
			{
				return GetCodeNumsForPref(PreferenceName.InsBenSRPCodes);
			}
		}

		///<summary>Returns a list of CodeNums for specific BW procedure codes.
		///There are several places in the program that need a BW group.  E.g. when computing limitations.</summary>
		public static List<long> ListFullDebridementCodeNums
		{
			get
			{
				return GetCodeNumsForPref(PreferenceName.InsBenFullDebridementCodes);
			}
		}

		///<summary>Returns a list of CodeNums for specific Exam procedure codes.
		///There are several places in the program that need a Exam group.  E.g. when computing limitations.</summary>
		public static List<long> ListPerioMaintCodeNums
		{
			get
			{
				return GetCodeNumsForPref(PreferenceName.InsBenPerioMaintCodes);
			}
		}

		///<summary>Returns a list of CodeNums for specific PanoFMX procedure codes.
		///There are several places in the program that need a PanoFMX group.  E.g. when computing limitations.</summary>
		public static List<long> ListDenturesCodeNums
		{
			get
			{
				return GetCodeNumsForPref(PreferenceName.InsBenDenturesCodes);
			}
		}

		///<summary>Returns a list of CodeNums for specific BW procedure codes.
		///There are several places in the program that need a BW group.  E.g. when computing limitations.</summary>
		public static List<long> ListImplantCodeNums
		{
			get
			{
				return GetCodeNumsForPref(PreferenceName.InsBenImplantCodes);
			}
		}

		#region InsHist Preference Procedures

		///<summary>Returns the first procedure code for the InsHist preference passed in.</summary>
		public static ProcedureCode GetByInsHistPref(string prefName)
		{
			return GetProcCode(GetCodeNumsForPref(prefName).FirstOrDefault());
		}

		#endregion InsHist Preference Procedures

		#endregion Additional Lists



		public const string GroupProcCode = "~GRP~";

		private static bool IsCanada => CultureInfo.CurrentCulture.Name.EndsWith("CA"); // Canadian. en-CA or fr-CA

		public static string BitewingCode
		{
			get
			{
				if (IsCanada) 
				{
					return "02144"; // 4BW The same code for Quebec as well as the rest of Canada.
				}

				return "D0274";
			}
		}

		public static string PanoCode
		{
			get
			{
				if (IsCanada)
				{
					if (Canadian.IsQuebec()) // Quebec
					{
						return "02600";
					}
					else // The rest of Canada use the same procedure codes.
					{
						return "02601";
					}
				}

				return "D0330";
			}
		}

		public static string CancerScreeningCode
		{
			get
			{
				if (IsCanada)
				{
					return "01401";//Just a diagnostic code that doesn't look like it's very common.
				}

				return "D0431";
			}
		}

		public static string ProphyCode
		{
			get
			{
				if (IsCanada)
				{
					return "11101";
				}

				return "D1110";
			}
		}

		public static string FlourideCode
		{
			get
			{
				if (IsCanada)
				{
					return "12101";
				}

				return "D1206";
			}
		}

		public static string SealantCode
		{
			get
			{
				if (IsCanada)
				{
					return "13401";
				}

				return "D1351";
			}
		}

		public static string CrownCode
		{
			get
			{
				if (IsCanada)
				{
					return "27201";
				}

				return "D2740";
			}
		}

		public static string SRPCode
		{
			get
			{
				if (IsCanada)
				{
					return "43421";
				}

				return "D4341";
			}
		}

		public static string FullDebridementCode
		{
			get
			{
				if (IsCanada)
				{
					return "42121";
				}

				return "D4355";
			}
		}

		public static string PerioMaintCode
		{
			get
			{
				if (IsCanada)
				{
					return "49101";
				}

				return "D4910";
			}
		}

		public static string DenturesCode
		{
			get
			{
				if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
				{
					return "51711";
				}
				return "D5110";
			}
		}

		public static string ImplantCode
		{
			get
			{
				if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
				{
					return "79921";
				}
				return "D6010";
			}
		}

		///<summary>Hard-coded dictionary of eService codes and their corresponding ProcCode within the database at HQ.</summary>
		private static readonly Dictionary<eServiceCode, string> _dictEServiceProcCodes = new Dictionary<eServiceCode, string>() {
			{ eServiceCode.Bundle,"042" },
			{ eServiceCode.ConfirmationOwn,"045" },
			{ eServiceCode.ConfirmationRequest,"040" },
			{ eServiceCode.EClipboard,"047" },
			{ eServiceCode.MobileWeb,"027" },
			{ eServiceCode.PatientPortal,"033" },
			{ eServiceCode.ResellerSoftwareOnly,"043" },
			{ eServiceCode.SoftwareOnly,"030" },
			{ eServiceCode.IntegratedTexting,"038" },
			{ eServiceCode.IntegratedTextingOwn,"046" },
			{ eServiceCode.IntegratedTextingUsage,"039" },
			{ eServiceCode.WebForms,"036" },
			{ eServiceCode.WebSched,"037" },
			{ eServiceCode.WebSchedNewPatAppt,"041" },
			{ eServiceCode.WebSchedASAP,"044" },
			{ eServiceCode.EmailMassUsage,"050" },
			{ eServiceCode.EmailSecureUsage,"051" },
			{ eServiceCode.EmailSecureAccess,"052" },
		};

		public static string GetProcCodeForEService(eServiceCode eService) 
			=> _dictEServiceProcCodes[eService];
		
		public static eServiceCode GetEServiceForProcCode(string procedureCode)
		{
			foreach (eServiceCode eService in _dictEServiceProcCodes.Keys)
			{
				if (_dictEServiceProcCodes[eService] == procedureCode)
				{
					return eService;
				}
			}

			throw new Exception("No corresponding eService found for the ProcCode provided.");
		}

		public static List<string> GetAllEServiceProcCodes() 
			=> _dictEServiceProcCodes.Values.ToList();

		public static IEnumerable<ProcedureCode> GetChangedSince(DateTime changedSince) 
			=> SelectMany("SELECT * FROM `procedure_codes` WHERE `last_modified_date` > @date",
				new MySqlParameter("date", changedSince));

		public static long Insert(ProcedureCode procedureCode) 
			=> ExecuteInsert(procedureCode);

		public static void InsertMany(List<ProcedureCode> procedureCodes)
		{
			if (procedureCodes.IsNullOrEmpty())
			{
				return;
			}

			foreach (var procedureCode in procedureCodes)
            {
				ExecuteInsert(procedureCode);
            }
		}

		public static void Update(ProcedureCode procedureCode) 
			=> ExecuteUpdate(procedureCode);

		public static void Update(ProcedureCode procCode, ProcedureCode procCodeOld)
		{

			//return Crud.ProcedureCodeCrud.Update(procCode, procCodeOld);
		}

		///<summary>Counts all procedure codes, including hidden codes.</summary>
		public static long GetCodeCount()
		{

			string command = "SELECT COUNT(*) FROM procedurecode";
			return PIn.Long(Database.ExecuteString(command));
		}

		///<summary>Returns the ProcedureCode for the supplied procCode such as such as D####.
		///If no ProcedureCode is found, returns a new ProcedureCode.</summary>
		public static ProcedureCode GetProcCode(string myCode)
		{
			ProcedureCode procedureCode = new ProcedureCode();

			if (IsValidCode(myCode))
			{
				procedureCode = cache.Find(myCode);
			}

			return procedureCode;
		}

		/// <summary>
		/// Returns a list of ProcedureCodes for the supplied procCodes such as such as D####. 
		/// Returns an empty list if no matches.
		/// </summary>
		public static List<ProcedureCode> GetProcCodes(List<string> listCodes)
		{
			if (listCodes == null || listCodes.Count == 0)
			{
				return new List<ProcedureCode>();
			}

			return cache.Find(x => listCodes.Contains(x.Code));
		}

		///<summary>The new way of getting a procCode. Uses the primary key instead of string code. Returns new instance if not found.
		///Pass in a list of all codes to save from locking the cache if you are going to call this method repeatedly.</summary>
		public static ProcedureCode GetProcCode(long procedureCodeId, List<ProcedureCode> procedureCodes = null)
		{
			if (procedureCodeId == 0)
			{
				return new ProcedureCode();
			}

			if (procedureCodes == null)
			{
				return cache.FirstOrDefault(x => x.Id == procedureCodeId) 
					?? new ProcedureCode();
			}

			return procedureCodes.FirstOrDefault(x => x.Id == procedureCodeId) 
				?? new ProcedureCode();
		}

		public static ProcedureCode GetProcCodeFromDb(long procedureCodeId)
		{
			ProcedureCode retval = SelectOne(procedureCodeId);
			if (retval == null)
			{
				return new ProcedureCode();
			}
			return retval;
		}

		///<summary>Supply the human readable proc code such as D####. If not found, returns 0.</summary>
		public static long GetCodeNum(string code)
		{
			if (IsValidCode(code))
			{
				return cache.Find(code).Id;
			}

			return 0;
		}

		///<summary>Gets a list of ProcedureCode for a given treatment area,including codes in hidden categories if isHiddenIncluded=true.</summary>
		public static List<ProcedureCode> GetProcCodesByTreatmentArea(bool isHiddenIncluded, params ProcedureTreatmentArea[] treatmentAreas)
		{
			return cache.Find(x => x.TreatmentArea.In<ProcedureTreatmentArea>(treatmentAreas)
				&& (isHiddenIncluded || !Definitions.GetHidden(DefinitionCategory.ProcCodeCats, x.ProcedureCategory)));
		}

		///<summary>If a substitute exists for the given proc code, then it will give the CodeNum of that code.
		///Otherwise, it will return the codeNum for the given procCode.</summary>
		public static long GetSubstituteCodeNum(string code, string tooth, long planNum, List<SubstitutionLink> listSubLinks = null)
		{
			long procedureCodeId = 0;

			if (string.IsNullOrEmpty(code))
			{
				return procedureCodeId;
			}

			ODException.SwallowAnyException(() =>
			{
				ProcedureCode procedureCode = cache.Find(code);
				procedureCodeId = procedureCode.Id;
				listSubLinks = listSubLinks ?? SubstitutionLinks.GetAllForPlans(new long[] { planNum });
				//Check procedure code level substitution if no insurance substitution override.
				SubstitutionLink subLink = listSubLinks.FirstOrDefault(x => x.CodeNum == procedureCode.Id && x.PlanNum == planNum);
				if ((subLink == null || string.IsNullOrEmpty(subLink.SubstitutionCode) || !ProcedureCodes.IsValidCode(subLink.SubstitutionCode))
					&& !string.IsNullOrEmpty(procedureCode.SubstitutionCode) && ProcedureCodes.IsValidCode(procedureCode.SubstitutionCode))
				{
					//Swallow any following exceptions because the old code would first check and make sure the key was in the dictionary.
					ODException.SwallowAnyException(() =>
					{
						if (procedureCode.SubstitutionCondition == SubstitutionCondition.Always)
						{
							procedureCodeId = cache.Find(procedureCode.SubstitutionCode).Id;
						}
						else if (procedureCode.SubstitutionCondition == SubstitutionCondition.Molar && Tooth.IsMolar(tooth))
						{
							procedureCodeId = cache.Find(procedureCode.SubstitutionCode).Id;
						}
						else if (procedureCode.SubstitutionCondition == SubstitutionCondition.SecondMolar && Tooth.IsSecondMolar(tooth))
						{
							procedureCodeId = cache.Find(procedureCode.SubstitutionCode).Id;
						}
						else if (procedureCode.SubstitutionCondition == SubstitutionCondition.Posterior && Tooth.IsPosterior(tooth))
						{
							procedureCodeId = cache.Find(procedureCode.SubstitutionCode).Id;
						}
					});
				}
				//Check insplan substituationlink(override) for the procedure.
				if (subLink != null && !string.IsNullOrEmpty(subLink.SubstitutionCode) && ProcedureCodes.IsValidCode(subLink.SubstitutionCode))
				{
					ODException.SwallowAnyException(() =>
					{
						if (subLink.SubstOnlyIf == SubstitutionCondition.Always)
						{
							procedureCodeId = cache.Find(subLink.SubstitutionCode).Id;
						}
						else if (subLink.SubstOnlyIf == SubstitutionCondition.Molar && Tooth.IsMolar(tooth))
						{
							procedureCodeId = cache.Find(subLink.SubstitutionCode).Id;
						}
						else if (subLink.SubstOnlyIf == SubstitutionCondition.SecondMolar && Tooth.IsSecondMolar(tooth))
						{
							procedureCodeId = cache.Find(subLink.SubstitutionCode).Id;
						}
						else if (subLink.SubstOnlyIf == SubstitutionCondition.Posterior && Tooth.IsPosterior(tooth))
						{
							procedureCodeId = cache.Find(subLink.SubstitutionCode).Id;
						}
					});
				}
			});
			return procedureCodeId;
		}

		///<summary>Gets the proc codes as a comma separated list from the preference and finds the corresponding code nums.</summary>
		public static List<long> GetCodeNumsForPref(string pref)
		{
			List<string> listCodes = Preferences.GetString(pref).Split(',').Select(x => x.Trim()).ToList();
			return GetWhereFromList(x => x.Code.In(listCodes)).Select(x => x.Id).ToList();
		}

		///<summary>Gets the CodeNums for the passed in InsHist preference.</summary>
		public static List<long> GetCodeNumsForInsHistPref(string pref)
		{
			List<long> retVal = GetCodeNumsForPref(pref);
			switch (pref)
			{
				case PreferenceName.InsHistBWCodes:
					retVal.AddRange(ProcedureCodes.ListBWCodeNums);
					break;
				case PreferenceName.InsHistExamCodes:
					retVal.AddRange(ProcedureCodes.ListExamCodeNums);
					break;
				case PreferenceName.InsHistPanoCodes:
					retVal.AddRange(ProcedureCodes.ListPanoFMXCodeNums);
					break;
				case PreferenceName.InsHistPerioLLCodes:
				case PreferenceName.InsHistPerioLRCodes:
				case PreferenceName.InsHistPerioULCodes:
				case PreferenceName.InsHistPerioURCodes:
					retVal.AddRange(ProcedureCodes.ListSRPCodeNums);
					break;
				case PreferenceName.InsHistPerioMaintCodes:
					retVal.AddRange(ProcedureCodes.ListPerioMaintCodeNums);
					break;
				case PreferenceName.InsHistProphyCodes:
					retVal.AddRange(ProcedureCodes.ListProphyCodeNums);
					break;
				default:
					//InsHistDebridementCodes
					break;
			}
			return retVal;
		}

		///<summary>Returns true if this code is marked as BypassIfZero and if this procFee is zero.</summary>
		public static bool CanBypassLockDate(long codeNum, double procFee)
		{
			bool isBypassGlobalLock = false;
			ProcedureCode procCode = GetFirstOrDefaultFromList(x => x.Id == codeNum);
			if (procCode != null && procCode.BypassGlobalLock == BypassLockStatus.BypassIfZero && procFee.IsZero())
			{
				isBypassGlobalLock = true;
			}
			return isBypassGlobalLock;
		}

		///<summary>Returns true if any code is marked as BypassIfZero.</summary>
		public static bool DoAnyBypassLockDate()
		{
			ProcedureCode procedureCode = GetFirstOrDefaultFromList(x => x.BypassGlobalLock == BypassLockStatus.BypassIfZero);
			return (procedureCode != null);
		}

		///<summary>Pass in an optional list of procedure codes in order to use it instead of the cache.</summary>
		public static string GetStringProcCode(long codeNum, List<ProcedureCode> listProcedureCodes = null, bool doThrowIfMissing = true)
		{
			//No need to check RemotingRole; no call to db.
			if (codeNum == 0)
			{
				return "";
				//throw new ApplicationException("CodeNum cannot be zero.");
			}
			ProcedureCode procedureCode;
			if (listProcedureCodes == null)
			{
				procedureCode = GetFirstOrDefaultFromList(x => x.Id == codeNum);
			}
			else
			{
				procedureCode = listProcedureCodes.FirstOrDefault(x => x.Id == codeNum);
			}
			if (procedureCode != null)
			{
				return procedureCode.Code;
			}
			if (doThrowIfMissing)
			{
				throw new ApplicationException("Missing codenum");
			}
			return "";
		}

		///<summary></summary>
		public static bool IsValidCode(string myCode)
		{
			//No need to check RemotingRole; no call to db.
			if (string.IsNullOrEmpty(myCode))
			{
				return false;
			}
			return cache.Contains(myCode);
		}

		/// <summary>
		/// Grouped by Category.
		/// Used only in FormRpProcCodes.
		/// </summary>
		public static ProcedureCode[] GetProcList(Dictionary<string, List<Definition>> definitions = null)
		{
			List<ProcedureCode> retVal = new List<ProcedureCode>();

			List<Definition> array;
			if (definitions == null)
			{
				array = Definitions.GetByCategory(DefinitionCategory.ProcCodeCats);
			}
			else
			{
				array = definitions[DefinitionCategory.ProcCodeCats];
			}

			List<ProcedureCode> listProcedureCodes = GetListDeep();
			for (int j = 0; j < definitions[DefinitionCategory.ProcCodeCats].Count; j++)
			{
				for (int k = 0; k < listProcedureCodes.Count; k++)
				{
					if (definitions[DefinitionCategory.ProcCodeCats][j].Id == listProcedureCodes[k].ProcedureCategory)
					{
						retVal.Add(listProcedureCodes[k].Copy());
					}
				}
			}

			return retVal.ToArray();
		}

		///<summary>Gets a list of procedure codes directly from the database.  If categories.length==0, then we will get for all categories.  Categories are defnums.  FeeScheds are, for now, defnums.</summary>
		public static DataTable GetProcTable(string abbr, string desc, string code, List<long> categories, long feeSchedNum,
			long feeSched2Num, long feeSched3Num)
		{

			string whereCat;
			if (categories.Count == 0)
			{
				whereCat = "1";
			}
			else
			{
				whereCat = "(";
				for (int i = 0; i < categories.Count; i++)
				{
					if (i > 0)
					{
						whereCat += " OR ";
					}
					whereCat += "ProcCat=" + POut.Long(categories[i]);
				}
				whereCat += ")";
			}
			FeeSchedule feeSched = FeeScheds.GetFirstOrDefault(x => x.Id == feeSchedNum);
			FeeSchedule feeSched2 = FeeScheds.GetFirstOrDefault(x => x.Id == feeSched2Num);
			FeeSchedule feeSched3 = FeeScheds.GetFirstOrDefault(x => x.Id == feeSched3Num);
			//Query changed to be compatible with both MySQL and Oracle (not tested).
			string command = "SELECT ProcCat,Descript,AbbrDesc,procedurecode.ProcCode,";
			if (feeSched == null)
			{
				command += "-1 FeeAmt1,";
			}
			else
			{
				command += "CASE ";
				if (!feeSched.IsGlobal && Clinics.ClinicId != 0)
				{//Use local clinic fee if there's one present
					command += "WHEN (feeclinic1.Amount IS NOT NULL) THEN feeclinic1.Amount ";
				}
				command += "WHEN (feehq1.Amount IS NOT NULL) THEN feehq1.Amount ELSE -1 END FeeAmt1,";
			}
			if (feeSched2 == null)
			{
				command += "-1 FeeAmt2,";
			}
			else
			{
				command += "CASE ";
				if (!feeSched2.IsGlobal && Clinics.ClinicId != 0)
				{//Use local clinic fee if there's one present
					command += "WHEN (feeclinic2.Amount IS NOT NULL) THEN feeclinic2.Amount ";
				}
				command += "WHEN (feehq2.Amount IS NOT NULL) THEN feehq2.Amount ELSE -1 END FeeAmt2,";
			}
			if (feeSched3 == null)
			{
				command += "-1 FeeAmt3,";
			}
			else
			{
				command += "CASE ";
				if (!feeSched3.IsGlobal && Clinics.ClinicId != 0)
				{//Use local clinic fee if there's one present
					command += "WHEN (feeclinic3.Amount IS NOT NULL) THEN feeclinic3.Amount ";
				}
				command += "WHEN (feehq3.Amount IS NOT NULL) THEN feehq3.Amount ELSE -1 END FeeAmt3,";
			}
			command += "procedurecode.CodeNum ";
			if (feeSched != null && !feeSched.IsGlobal && Clinics.ClinicId != 0)
			{
				command += ",CASE WHEN (feeclinic1.Amount IS NOT NULL) THEN 1 ELSE 0 END IsClinic1 ";
			}
			if (feeSched2 != null && !feeSched2.IsGlobal && Clinics.ClinicId != 0)
			{
				command += ",CASE WHEN (feeclinic2.Amount IS NOT NULL) THEN 1 ELSE 0 END IsClinic2 ";
			}
			if (feeSched3 != null && !feeSched3.IsGlobal && Clinics.ClinicId != 0)
			{
				command += ",CASE WHEN (feeclinic3.Amount IS NOT NULL) THEN 1 ELSE 0 END IsClinic3 ";
			}
			command += "FROM procedurecode ";
			if (feeSched != null)
			{
				if (!feeSched.IsGlobal && Clinics.ClinicId.HasValue)
				{//Get local clinic fee if there's one present
					command += "LEFT JOIN fee feeclinic1 ON feeclinic1.CodeNum=procedurecode.CodeNum AND feeclinic1.FeeSched=" + POut.Long(feeSched.Id)
						+ " AND feeclinic1.ClinicNum=" + POut.Long(Clinics.ClinicId.Value) + " ";
				}
				//Get the hq clinic fee if there's one present
				command += "LEFT JOIN fee feehq1 ON feehq1.CodeNum=procedurecode.CodeNum AND feehq1.FeeSched=" + POut.Long(feeSched.Id)
					+ " AND feehq1.ClinicNum=0 ";
			}
			if (feeSched2 != null)
			{
				if (!feeSched2.IsGlobal && Clinics.ClinicId.HasValue)
				{//Get local clinic fee if there's one present
					command += "LEFT JOIN fee feeclinic2 ON feeclinic2.CodeNum=procedurecode.CodeNum AND feeclinic2.FeeSched=" + POut.Long(feeSched2.Id)
						+ " AND feeclinic2.ClinicNum=" + POut.Long(Clinics.ClinicId.Value) + " ";
				}
				//Get the hq clinic fee if there's one present
				command += "LEFT JOIN fee feehq2 ON feehq2.CodeNum=procedurecode.CodeNum AND feehq2.FeeSched=" + POut.Long(feeSched2.Id)
					+ " AND feehq2.ClinicNum=0 ";
			}
			if (feeSched3 != null)
			{
				if (!feeSched3.IsGlobal && Clinics.ClinicId.HasValue)
				{//Get local clinic fee if there's one present
					command += "LEFT JOIN fee feeclinic3 ON feeclinic3.CodeNum=procedurecode.CodeNum AND feeclinic3.FeeSched=" + POut.Long(feeSched3.Id)
						+ " AND feeClinic3.ClinicNum=" + POut.Long(Clinics.ClinicId.Value) + " ";
				}
				//Get the hq clinic fee if there's one present
				command += "LEFT JOIN fee feehq3 ON feehq3.CodeNum=procedurecode.CodeNum AND feehq3.FeeSched=" + POut.Long(feeSched3.Id)
					+ " AND feehq3.ClinicNum=0 ";
			}
			command += "LEFT JOIN definition ON definition.DefNum=procedurecode.ProcCat "
			+ "WHERE " + whereCat + " "
			+ "AND Descript LIKE '%" + POut.String(desc) + "%' "
			+ "AND AbbrDesc LIKE '%" + POut.String(abbr) + "%' "
			+ "AND procedurecode.ProcCode LIKE '%" + POut.String(code) + "%' "
			+ "ORDER BY definition.ItemOrder,procedurecode.ProcCode";
			return Database.ExecuteDataTable(command);
		}

		///<summary>Returns the LaymanTerm for the supplied codeNum, or the description if none present.</summary>
		public static string GetLaymanTerm(long codeNum)
		{
			//No need to check RemotingRole; no call to db.
			string laymanTerm = "";
			ProcedureCode procedureCode = GetFirstOrDefaultFromList(x => x.Id == codeNum);
			if (procedureCode != null)
			{
				laymanTerm = (procedureCode.LaymanTerm != "" ? procedureCode.LaymanTerm : procedureCode.Description);
			}
			return laymanTerm;
		}

		///<summary>Used to check whether codes starting with T exist and are in a visible category.  If so, it moves them to the Obsolete category.  If the T code has never been used, then it deletes it.</summary>
		public static void TcodesClear()
		{

			//first delete any unused T codes
			string command = @"SELECT CodeNum,ProcCode FROM procedurecode
				WHERE CodeNum NOT IN(SELECT CodeNum FROM procedurelog)
				AND CodeNum NOT IN(SELECT CodeNum FROM autocodeitem)
				AND CodeNum NOT IN(SELECT CodeNum FROM procbuttonitem)
				AND CodeNum NOT IN(SELECT CodeNum FROM recalltrigger)
				AND CodeNum NOT IN(SELECT CodeNum FROM benefit)
				AND ProcCode NOT IN(SELECT CodeValue FROM encounter WHERE CodeSystem='CDT')
				AND ProcCode LIKE 'T%'";
			DataTable table = Database.ExecuteDataTable(command);
			List<long> listCodeNums = new List<long>();
			List<string> listRecallCodes = RecallTypes.GetDeepCopy()
				.SelectMany(x => x.Procedures.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				.ToList();
			for (int i = 0; i < table.Rows.Count; i++)
			{
				if (!listRecallCodes.Contains(PIn.String(table.Rows[i]["ProcCode"].ToString())))
				{//The ProcCode is not attached to a recall type.
					listCodeNums.Add(PIn.Long(table.Rows[i]["CodeNum"].ToString()));
				}
			}
			if (listCodeNums.Count > 0)
			{
				ProcedureCodes.ClearFkey(listCodeNums);//Zero securitylog FKey column for rows to be deleted.
				command = "SELECT FeeNum FROM fee WHERE CodeNum IN(" + String.Join(",", listCodeNums) + ")";
				List<long> listFeeNums = Database.GetListLong(command);
				Fees.DeleteMany(listFeeNums);
				command = "DELETE FROM proccodenote WHERE CodeNum IN(" + String.Join(",", listCodeNums) + ")";
				Database.ExecuteNonQuery(command);
				command = "DELETE FROM procedurecode WHERE CodeNum IN(" + String.Join(",", listCodeNums) + ")";
				Database.ExecuteNonQuery(command);
			}
			//then, move any other T codes to obsolete category
			command = @"SELECT DISTINCT ProcCat FROM procedurecode,definition 
				WHERE procedurecode.ProcCode LIKE 'T%'
				AND definition.IsHidden=0
				AND procedurecode.ProcCat=definition.DefNum";
			table = Database.ExecuteDataTable(command);
			long catNum = Definitions.GetByExactName(DefinitionCategory.ProcCodeCats, "Obsolete");//check to make sure an Obsolete category exists.
			Definition def;
			if (catNum != 0)
			{//if a category exists with that name
				def = Definitions.GetDef(DefinitionCategory.ProcCodeCats, catNum);
				if (!def.IsHidden)
				{
					def.IsHidden = true;
					Definitions.Update(def);
					Definitions.RefreshCache();
				}
			}
			if (catNum == 0)
			{
				List<Definition> listDefs = Definitions.GetByCategory(DefinitionCategory.ProcCodeCats);
				def = new Definition();
				def.Category = DefinitionCategory.ProcCodeCats;
				def.Name = "Obsolete";
				def.SortOrder = listDefs.Count;
				def.IsHidden = true;
				Definitions.Insert(def);
				Definitions.RefreshCache();
				catNum = def.Id;
			}
			for (int i = 0; i < table.Rows.Count; i++)
			{
				command = "UPDATE procedurecode SET ProcCat=" + POut.Long(catNum)
					+ " WHERE ProcCat=" + table.Rows[i][0].ToString()
					+ " AND procedurecode.ProcCode LIKE 'T%'";
				Database.ExecuteNonQuery(command);
			}
			//finally, set Never Used category to be hidden.  This isn't really part of clearing Tcodes, but is required
			//because many customers won't have that category hidden
			catNum = Definitions.GetByExactName(DefinitionCategory.ProcCodeCats, "Never Used");
			if (catNum != 0)
			{//if a category exists with that name
				def = Definitions.GetDef(DefinitionCategory.ProcCodeCats, catNum);
				if (!def.IsHidden)
				{
					def.IsHidden = true;
					Definitions.Update(def);
					Definitions.RefreshCache();
				}
			}
		}

		public static void ResetApptProcsQuickAdd()
		{

			string command = "DELETE FROM definition WHERE Category=3";
			Database.ExecuteNonQuery(command);
			string[] array = new string[] {
				"CompEx-4BW-Pano-Pro-Flo","D0150,D0274,D0330,D1110,D1208",
				"CompEx-2BW-Pano-ChPro-Flo","D0150,D0272,D0330,D1120,D1208",
				"PerEx-4BW-Pro-Flo","D0120,D0274,D1110,D1208",
				"LimEx-PA","D0140,D0220",
				"PerEx-4BW-Pro-Flo","D0120,D0274,D1110,D1208",
				"PerEx-2BW-ChildPro-Flo","D0120,D0272,D1120,D1208",
				"Comp Exam","D0150",
				"Per Exam","D0120",
				"Lim Exam","D0140",
				"1 PA","D0220",
				"2BW","D0272",
				"4BW","D0274",
				"Pano","D0330",
				"Pro Adult","D1110",
				"Fluor","D1208",
				"Pro Child","D1120",
				"PostOp","N4101",
				"DentAdj","N4102",
				"Consult","D9310"
			};
			Definition def;
			string[] codelist;
			bool allvalid;
			int itemorder = 0;
			for (int i = 0; i < array.Length; i += 2)
			{
				//first, test all procedures for valid
				codelist = array[i + 1].Split(',');
				allvalid = true;
				for (int c = 0; c < codelist.Length; c++)
				{
					if (!ProcedureCodes.IsValidCode(codelist[c]))
					{
						allvalid = false;
					}
				}
				if (!allvalid)
				{
					continue;
				}
				def = new Definition();
				def.Category = DefinitionCategory.ApptProcsQuickAdd;
				def.SortOrder = itemorder;
				def.Name = array[i];
				def.Value = array[i + 1];
				Definitions.Insert(def);
				itemorder++;
			}
		}

		///<summary>Resets the descriptions for all ADA codes to the official wording.  Required by the license.</summary>
		public static int ResetADAdescriptions()
		{
			//No need to check RemotingRole; no call to db.
			// return ResetADAdescriptions(CDT.Class1.GetADAcodes());

			// TODO: Find out what CDT.Class1.GetADACodes() does and implement it here....

			return 0;
		}

		///<summary>Resets the descriptions for all ADA codes to the official wording.  Required by the license.</summary>
		public static int ResetADAdescriptions(List<ProcedureCode> codeList)
		{
			//No need to check RemotingRole; no call to db.
			ProcedureCode code;
			int count = 0;
			for (int i = 0; i < codeList.Count; i++)
			{
				if (!ProcedureCodes.IsValidCode(codeList[i].Code))
				{//If this code is not in this database
					continue;
				}
				code = ProcedureCodes.GetProcCode(codeList[i].Code);
				if (code.Description == codeList[i].Description)
				{
					continue;
				}
				string oldDescript = code.Description;
				DateTime datePrevious = code.LastModifiedDate;
				code.Description = codeList[i].Description;
				ProcedureCodes.Update(code);
				SecurityLogs.MakeLogEntry(Permissions.ProcCodeEdit, 0, "Code " + code.Code + " changed from '" + oldDescript + "' to '" + code.Description + "' by D-Codes Tool."
					, code.Id, datePrevious);
				count++;
			}
			return count;
			//don't forget to refresh procedurecodes.
		}

		/// <summary>
		/// Returns true if the ADA missed appointment procedure code is in the database to identify how to track missed appointments.
		/// </summary>
		public static bool HasMissedCode() => cache.Contains("D9986");

		/// <summary>
		/// Returns true if the ADA cancelled appointment procedure code is in the database to identify how to track cancelled appointments.
		/// </summary>
		public static bool HasCancelledCode() => cache.Contains("D9987");

		/// <summary>
		/// Gets a list of procedureCodes from the cache using a comma-delimited list of ProcCodes.
		/// Returns a new list is the passed in string is empty.
		/// </summary>
		public static List<ProcedureCode> GetFromCommaDelimitedList(string codeStr)
		{
			List<ProcedureCode> retVal = new List<ProcedureCode>();
			if (string.IsNullOrEmpty(codeStr))
			{
				return retVal;
			}

			string[] arrayProcCodes = codeStr.Split(new char[] { ',' });
			foreach (string code in arrayProcCodes)
			{
				retVal.Add(GetProcCode(code));
			}

			return retVal;
		}

		public static IEnumerable<ProcedureCode> GetAllCodes()
		{
			return SelectMany("SELECT * FROM `procedure_codes` ORDER BY `code`");
		}

		public static IEnumerable<ProcedureCode> GetAllCodesOrderedCatCode()
		{
			return SelectMany("SELECT * FROM `procedure_codes` ORDER BY `procedure_category`, `code`");
		}

		public static void ClearFkey(long procedureCodeId)
		{
		}

		public static void ClearFkey(List<long> procedureCodeIds)
		{
		}

		public static List<ProcedureCode> GetCodesForCodeNums(List<long> procedureCodeIds)
		{
			return cache.Find(x => procedureCodeIds.Contains(x.Id));
		}

		///<summary>Gets all ortho procedure code nums from the OrthoPlacementProcsList preference if any are present.
		///Otherwise gets all procedure codes that start with D8</summary>
		public static List<long> GetOrthoBandingCodeNums()
		{
			//No need to check RemotingRole; no call to db.
			string strListOrthoNums = Preferences.GetString(PreferenceName.OrthoPlacementProcsList);
			List<long> listCodeNums = new List<long>();
			if (strListOrthoNums != "")
			{
				return strListOrthoNums.Split(new char[] { ',' }).ToList().Select(x => PIn.Long(x)).ToList();
			}
			else
			{
				return GetWhereFromList(x => x.Code.ToUpper().StartsWith("D8")).Select(x => x.Id).ToList();
			}
		}

		/* js These are not currently in use.  This probably needs to be consolidated with code from other places.  ProcsColored and InsSpans comes to mind.
		///<summary>Returns true if any of the codes in the list fall within the code range.</summary>
		public static bool IsCodeInRange(List<string> myCodes,string range) {
			for(int i=0;i<myCodes.Count;i++) {
				if(IsCodeInRange(myCodes[i],range)) {
					return true;
				}
			}
			return false;
		}

		///<summary>Returns true if myCode is within the code range.  Ex: myCode="D####", range="D####-D####"</summary>
		public static bool IsCodeInRange(string myCode,string range) {
			string code1="";
			string code2="";
			if(range.Contains("-")) {
				string[] codeSplit=range.Split('-');
				code1=codeSplit[0].Trim();
				code2=codeSplit[1].Trim();
			}
			else{
				code1=range.Trim();
				code2=range.Trim();
			}
			if(myCode.CompareTo(code1)<0 || myCode.CompareTo(code2)>0) {
				return false;
			}
			return true;
		}*/



	}
}
