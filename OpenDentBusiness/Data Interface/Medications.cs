using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Imedisoft.Data
{
    public partial class Medications
	{
		[CacheGroup(nameof(InvalidType.Medications))]
		private class MedicationCache : ListCache<Medication>
		{
			protected override IEnumerable<Medication> Load() 
				=> SelectMany("SELECT * FROM `medications` ORDER BY `name`");
        }

		private static readonly MedicationCache cache = new MedicationCache();

		public static Medication GetById(long medicationId) 
			=> cache.FirstOrDefault(medication => medication.Id == medicationId);

		public static List<Medication> GetWhere(Predicate<Medication> predicate) 
			=> cache.Find(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void FillCacheFromTable(DataTable table)
		{
			// TODO: cache.FillCacheFromTable(table);
		}

		public static IEnumerable<Medication> GetListFromDb() 
			=> SelectMany("SELECT * FROM `medications` ORDER BY `name`");

		/// <summary>
		/// Returns medications that contain the passed in string.
		/// Blank for all.
		/// </summary>
		public static List<Medication> GetList(string str = "")
		{
			return GetWhere(x => str == "" || x.Name.ToUpper().Contains(str.ToUpper()));
		}

		public static void Update(Medication medication) 
			=> ExecuteUpdate(medication);

		public static long Insert(Medication medication) 
			=> ExecuteInsert(medication);

		public static void Delete(Medication medication)
		{
			string message = IsInUse(medication);
			if (!string.IsNullOrEmpty(message))
			{
				throw new Exception(message);
			}

			ExecuteDelete(medication);
		}

		/// <summary>
		/// Returns a string if medication is in use in medicationpat, allergydef, eduresources, or preference.MedicationsIndicateNone.
		/// The string will explain where the medication is in use.
		/// </summary>
		public static string IsInUse(Medication med)
		{
			string[] brands;
			if (med.Id == med.GenericId)
			{
				brands = GetBrands(med.Id);
			}
			else
			{
				brands = new string[0];
			}

			if (brands.Length > 0)
			{
				return "You can not delete a medication that has brand names attached.";
			}

			string command = "SELECT COUNT(*) FROM medicationpat WHERE MedicationNum=" + med.Id;
			if (Database.ExecuteLong(command) != 0)
			{
				return "Not allowed to delete medication because it is in use by a patient";
			}

			command = "SELECT COUNT(*) FROM allergydef WHERE MedicationNum=" + med.Id;
			if (Database.ExecuteLong(command) != 0)
			{
				return "Not allowed to delete medication because it is in use by an allergy";
			}

			command = "SELECT COUNT(*) FROM eduresource WHERE MedicationNum=" + med.Id;
			if (Database.ExecuteLong(command) != 0)
			{
				return "Not allowed to delete medication because it is in use by an education resource";
			}

			command = "SELECT COUNT(*) FROM rxalert WHERE MedicationNum=" + med.Id;
			if (Database.ExecuteLong(command) != 0)
			{
				return "Not allowed to delete medication because it is in use by an Rx alert";
			}

			// If any more tables are added here in the future, then also update GetAllInUseMedicationNums() to include the new table.
			if (Preferences.GetLong(PreferenceName.MedicationsIndicateNone) == med.Id)
			{
				return "Not allowed to delete medication because it is in use by a medication";
			}

			return "";
		}

		public static List<long> GetAllInUseMedicationNums()
		{

			//If any more tables are added here in the future, then also update IsInUse() to include the new table.
			string command = 
				"SELECT MedicationNum FROM medicationpat WHERE MedicationNum!=0 " +
				"UNION SELECT `medication_id` FROM `allergy_defs` WHERE `medication_id` IS NOT NULL " +
				"UNION SELECT MedicationNum FROM eduresource WHERE MedicationNum!=0 " +
				"GROUP BY MedicationNum";
			List<long> listMedicationNums = Database.GetListLong(command);
			if (Preferences.GetLong(PreferenceName.MedicationsIndicateNone) != 0)
			{
				listMedicationNums.Add(Preferences.GetLong(PreferenceName.MedicationsIndicateNone));
			}
			return listMedicationNums;
		}

		///<summary>Returns an array of all patient names who are using this medication.</summary>
		public static string[] GetPatNamesForMed(long medicationNum)
		{

			string command =
				"SELECT CONCAT(CONCAT(CONCAT(CONCAT(LName,', '),FName),' '),Preferred) FROM medicationpat,patient "
				+ "WHERE medicationpat.PatNum=patient.PatNum "
				+ "AND medicationpat.MedicationNum=" + POut.Long(medicationNum);
			DataTable table = Database.ExecuteDataTable(command);
			string[] retVal = new string[table.Rows.Count];
			for (int i = 0; i < table.Rows.Count; i++)
			{
				retVal[i] = PIn.String(table.Rows[i][0].ToString());
			}
			return retVal;
		}

		///<summary>Returns a list of all brands dependend on this generic. Only gets run if this is a generic.</summary>
		public static string[] GetBrands(long medicationNum)
		{

			string command =
				"SELECT MedName FROM medication "
				+ "WHERE GenericNum=" + medicationNum.ToString()
				+ " AND MedicationNum !=" + medicationNum.ToString();//except this med
			DataTable table = Database.ExecuteDataTable(command);
			string[] retVal = new string[table.Rows.Count];
			for (int i = 0; i < table.Rows.Count; i++)
			{
				retVal[i] = PIn.String(table.Rows[i][0].ToString());
			}
			return retVal;
		}

		/// <summary>Deprecated. Use GetMedication instead.</summary>
		public static Medication GetByIdNoCache(long medicationId) 
			=> SelectOne("SELECT * FROM `medications` WHERE `id` = " + medicationId);

		/// <summary>
		/// Returns first medication with matching MedName, if not found returns null.
		/// </summary>
		public static Medication GetByNameNoCache(string medicationName)
		{
			var medications = SelectMany("SELECT * FROM `medications` WHERE `name` = '" + POut.String(medicationName) + "' ORDER BY `id`").ToList();

			if (medications.Count > 0)
			{
				return medications[0];
			}

			return null;
		}

		/// <summary>
		/// Gets the generic medication for the specified medication Num. Returns null if not found.
		/// </summary>
		public static Medication GetGeneric(long medicationId)
		{
			var medication = GetById(medicationId);

			if (medication != null && medication.GenericId.HasValue)
			{
				medication = GetById(medication.GenericId.Value);
			}

			return medication;
		}

		public static string GetDescription(long medicationId)
		{
			var medication = GetById(medicationId);
			if (medication == null)
            {
				return "";
            }

			if (medication.GenericId.HasValue)
            {
				var generic = GetById(medication.GenericId.Value);
				if (generic != null)
                {
					return medication.Name + " (" + generic.Name + ")";
                }
            }

			return medication.Name;
		}

		/// <summary>
		/// Gets the medication name. Copied from GetDescription.
		/// </summary>
		public static string GetNameOnly(long medicationId)
		{
			return GetById(medicationId)?.Name ?? "";
		}

		/// <summary>
		/// Gets the generic medication name, given it's generic Num.
		/// </summary>
		public static string GetGenericName(long genericId)
		{
			return GetById(genericId)?.Name ?? "";
		}

		/// <summary>
		/// Gets the generic medication name, given it's generic Num. 
		/// Will search through the passed in list before resorting to cache.
		/// </summary>
		public static string GetGenericName(long genericNum, Hashtable hlist)
		{
			if (!hlist.ContainsKey(genericNum))
			{
				//Medication not found.  Refresh the cache and check again.
				return GetGenericName(genericNum);
			}

			return ((Medication)hlist[genericNum]).Name;
		}

		public static List<long> GetChangedSinceMedicationNums(DateTime changedSince)
		{

			string command = "SELECT MedicationNum FROM medication WHERE DateTStamp > " + POut.DateT(changedSince);
			DataTable dt = Database.ExecuteDataTable(command);
			List<long> medicationNums = new List<long>(dt.Rows.Count);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				medicationNums.Add(PIn.Long(dt.Rows[i]["MedicationNum"].ToString()));
			}
			return medicationNums;
		}

		public static IEnumerable<Medication> GetMultMedications(IEnumerable<long> medicationIds)
		{
			var medicationIdsList = medicationIds.ToList();
			if (medicationIdsList.Count == 0)
            {
				return new List<Medication>();
            }

			return SelectMany(
				"SELECT * FROM `medications` " +
				"WHERE `id` IN (" + string.Join(", ", medicationIdsList) + ")");
		}

		///<summary>Deprecated.  Use MedicationPat.Refresh() instead.  Returns medication list for a specific patient.</summary>
		public static IEnumerable<Medication> GetByPatientNoCache(long patientId)
		{
			return SelectMany(
				"SELECT m.* " +
				"FROM `medications` m, `medicationpat` mp " +
				"WHERE m.`id` = mp.`MedicationNum` " +
				"AND mp.`PatNum` = " + patientId);
		}

		public static List<Medication> GetByRxCui(string rxCui)
			=> cache.Find(x => x.RxCui == rxCui).OrderBy(x => x.Id).ToList();

		public static Medication GetByRxCuiNoCache(string rxCui) 
			=> SelectOne("SELECT * FROM `medications` WHERE `rx_cui` = @rx_cui ORDER BY `id`",
				new MySqlParameter("rx_cui", rxCui ?? ""));



		public static bool AreMedicationsEqual(Medication medication, Medication medicationOld)
		{
			if ((medicationOld == null || medication == null)
				|| medicationOld.Id != medication.Id
				|| medicationOld.Name != medication.Name
				|| medicationOld.GenericId != medication.GenericId
				|| medicationOld.Notes != medication.Notes
				|| medicationOld.RxCui != medication.RxCui)
			{
				return false;
			}
			return true;
		}

		///<summary>Returns the number of patients associated with the passed-in medicationNum.</summary>
		public static long CountPats(long medNum) 
			=> Database.ExecuteLong(
				"SELECT COUNT(DISTINCT medicationpat.PatNum) FROM medicationpat WHERE MedicationNum=" + medNum);

		///<summary>Medication merge tool.  Returns the number of rows changed.  Deletes the medication associated with medNumInto.</summary>
		public static long Merge(long medNumFrom, long medNumInto)
		{

			string[] medNumForeignKeys = new string[] { //add any new FKs to this list.
				"allergydef.MedicationNum",
				"eduresource.MedicationNum",
				"medication.GenericNum",
				"medicationpat.MedicationNum",
				"rxalert.MedicationNum"
			};
			string command = "";
			long rowsChanged = 0;
			for (int i = 0; i < medNumForeignKeys.Length; i++)
			{ //actually change all of the FKs in the above tables.
				string[] tableAndKeyName = medNumForeignKeys[i].Split(new char[] { '.' });
				command = "UPDATE " + tableAndKeyName[0]
					+ " SET " + tableAndKeyName[1] + "=" + POut.Long(medNumInto)
					+ " WHERE " + tableAndKeyName[1] + "=" + POut.Long(medNumFrom);
				rowsChanged += Database.ExecuteNonQuery(command);
			}
			command = "SELECT medication.RxCui FROM medication WHERE MedicationNum=" + medNumInto; //update medicationpat's RxNorms to match medication.
			string rxNorm = Database.ExecuteString(command);
			command = "UPDATE medicationpat SET RxCui=" + rxNorm + " WHERE MedicationNum=" + medNumInto;
			Database.ExecuteNonQuery(command);
			command = "SELECT * FROM ehrtrigger WHERE MedicationNumList LIKE '% " + POut.Long(medNumFrom) + " %'";
			List<EhrTrigger> ListEhrTrigger = OpenDentBusiness.Crud.EhrTriggerCrud.SelectMany(command); //get all ehr triggers with matching mednum in mednumlist
			for (int i = 0; i < ListEhrTrigger.Count; i++)
			{//for each trigger...
				string[] arrayMedNums = ListEhrTrigger[i].MedicationNumList.Split(new char[] { ' ' }); //get an array of their medicationNums.
				bool containsMedNumInto = arrayMedNums.Any(x => x == POut.Long(medNumInto));
				string strMedNumList = "";
				foreach (string medNumStr in arrayMedNums)
				{ //for each mednum in the MedicationList for the current trigger.
					string medNumCur = medNumStr.Trim();
					if (medNumCur == "")
					{   //because we use spaces as a buffer before and after mednums, this prevents empty spaces from being considered.
						continue;
					}
					if (containsMedNumInto)
					{ //if the list already contains medNumInto, 
						if (medNumCur == POut.Long(medNumFrom))
						{
							continue;   //skip medNumFrom (remove it from the list)
						}
						else
						{
							strMedNumList += " " + medNumCur + " ";
						}
					}
					else
					{ //if the list doesn't contain medNumInto
						if (medNumCur == POut.Long(medNumFrom))
						{
							strMedNumList += " " + medNumInto + " "; //replace medNumFrom with medNumInto
						}
						else
						{
							strMedNumList += " " + medNumCur + " ";
						}
					}
				}//end for each mednum
				ListEhrTrigger[i].MedicationNumList = strMedNumList;
				EhrTriggers.Update(ListEhrTrigger[i]); //update the ehrtrigger list.
			}//end for each trigger
			ExecuteDelete(medNumFrom); //finally, delete the mednum.
			return rowsChanged;
		}
	}
}
