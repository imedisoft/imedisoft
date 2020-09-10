using Imedisoft.Data;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using OpenDentBusiness.Crud;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Imedisoft.Data
{
	public partial class AllergyDefs
	{
		///<summary>Gets one AllergyDef from the db.</summary>
		public static AllergyDef GetOne(long allergyDefNum)
		{
			return SelectOne(allergyDefNum);
		}

		///<summary>Gets one AllergyDef matching the specified allergyDefNum from the list passed in. If none found will search the db for a matching allergydef. Returns null if not found in the db.</summary>
		public static AllergyDef GetOne(long allergyDefNum, List<AllergyDef> listAllergyDef)
		{
			for (int i = 0; i < listAllergyDef.Count; i++)
			{
				if (allergyDefNum == listAllergyDef[i].Id)
				{
					return listAllergyDef[i];//Gets the allergydef matching the allergy so we can use it to populate the grid
				}
			}
			return GetOne(allergyDefNum);
		}

		public static AllergyDef GetByDescription(string allergyDescription) 
			=> SelectOne("SELECT * FROM `allergy_defs` WHERE `description` = @description", 
				new MySqlParameter("description", allergyDescription));


		public static long Insert(AllergyDef allergyDef)
		{
			return ExecuteInsert(allergyDef);
		}

		public static void Update(AllergyDef allergyDef)
		{
			ExecuteUpdate(allergyDef);
		}

		public static void Save(AllergyDef allergyDef)
        {
			if (allergyDef.Id == 0) ExecuteInsert(allergyDef);
            else
            {
				ExecuteUpdate(allergyDef);
            }
		}

		public static void Delete(long allergyDefNum)
		{
			ExecuteDelete(allergyDefNum);
		}

		public static IEnumerable<AllergyDef> GetAll(bool includeHidden = false)
		{
			return includeHidden ?
				SelectMany("SELECT * FROM `allergy_defs` ORDER BY `description`") :
				SelectMany("SELECT * FROM `allergy_defs` WHERE `is_hidden` = 0 ORDER BY `description`");
		}

		public static bool DefIsInUse(long allergyDefId)
		{
			var command = "SELECT COUNT(*) FROM `allergies` WHERE `allergy_def_id` = " + allergyDefId;
			if (Database.ExecuteLong(command) > 0)
			{
				return true;
			}

			command = "SELECT COUNT(*) FROM rxalert WHERE AllergyDefNum=" + allergyDefId;
			if (Database.ExecuteLong(command) > 0)
			{
				return true;
			}

			if (allergyDefId == Preferences.GetLong(PreferenceName.AllergiesIndicateNone))
			{
				return true;
			}

			return false;
		}

		public static List<long> GetChangedSinceAllergyDefNums(DateTime changedSince)
		{
			string command = "SELECT AllergyDefNum FROM allergydef WHERE DateTStamp > " + POut.DateT(changedSince);
			DataTable dt = Database.ExecuteDataTable(command);
			List<long> allergyDefNums = new List<long>(dt.Rows.Count);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				allergyDefNums.Add(PIn.Long(dt.Rows[i]["AllergyDefNum"].ToString()));
			}
			return allergyDefNums;
		}

		/// <summary>
		/// Used along with GetChangedSinceAllergyDefNums
		/// </summary>
		public static List<AllergyDef> GetMultAllergyDefs(List<long> allergyDefIds)
		{
			string strAllergyDefNums = "";

			if (allergyDefIds.Count > 0)
			{
				for (int i = 0; i < allergyDefIds.Count; i++)
				{
					if (i > 0)
					{
						strAllergyDefNums += "OR ";
					}
					strAllergyDefNums += "AllergyDefNum='" + allergyDefIds[i].ToString() + "' ";
				}
				string command = "SELECT * FROM allergydef WHERE " + strAllergyDefNums;

				return SelectMany(command).ToList();
			}

			return new List<AllergyDef>();
		}

		public static IEnumerable<AllergyDef> GetAllergyDefs(long patientId, bool includeInactive)
		{
			var command = 
				"SELECT ad.* FROM `allergy_defs` ad " +
				"INNER JOIN `allergies` a ON a.`allergy_def_id` = ad.`id` " +
				"WHERE a.`patient_id` = " + patientId;
			
			if (!includeInactive)
			{
				command += " AND a.`status_is_active` !=0";
			}

			return SelectMany(command);
		}

		public static string GetDescription(long allergyDefId)
		{
			if (allergyDefId == 0)
			{
				return "";
			}

			return SelectOne(allergyDefId)?.Description ?? "";
		}

		public static AllergyDef GetAllergyDefFromCode(string codeValue)
		{
			if (codeValue == "")
			{
				return null;
			}
			string command = "SELECT * FROM `allergy_defs` WHERE SnomedAllergyTo=" + POut.String(codeValue);
			return SelectOne(command);
		}

		///<summary>Returns the AllergyDef with the corresponding Medication. Returns null if medicationNum is 0.</summary>
		public static AllergyDef GetAllergyDefFromMedication(long medicationId)
		{
			if (medicationId == 0)
			{
				return null;
			}

			return SelectOne("SELECT * FROM `allergy_defs` WHERE `medication_id` = " + medicationId);
		}

		/// <summary>
		/// Returns the AllergyDef set to SnomedType 2 (DrugAllergy) or SnomedType 3 (DrugIntolerance) that is attached to a medication with this rxnorm. 
		/// Returns null if rxnorm is 0 or no allergydef for this rxnorm exists. 
		/// Used by HL7 service for inserting drug allergies for patients.
		/// </summary>
		public static AllergyDef GetAllergyDefFromRxnorm(long rxnorm) // TODO: rxnorm -> string
		{
			if (rxnorm == 0)
			{
				return null;
			}

			return SelectOne(
				"SELECT ad.* FROM `alergy_defs` ad " +
				"INNER JOIN `medications` m ON ad.`medication_id` = m.`id` " +
				"AND m.`rx_cui` = " + rxnorm + " " +
				"WHERE ad.`snomed_code` IN ('" + SnomedAllergyCode.DrugAllergy + "', '" + SnomedAllergyCode.DrugIntolerance + "')");
		}
	}
}
