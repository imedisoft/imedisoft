using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public class Allergies
	{ 
		public static List<Allergy> Refresh(long patNum) 
			=> Crud.AllergyCrud.SelectMany("SELECT * FROM allergy WHERE PatNum = " + patNum).ToList();
		
		public static Allergy GetOne(long allergyNum) 
			=> Crud.AllergyCrud.SelectOne(allergyNum);

		public static long Insert(Allergy allergy) 
			=> Crud.AllergyCrud.Insert(allergy);
		
		public static void Update(Allergy allergy) 
			=> Crud.AllergyCrud.Update(allergy);

		public static void Delete(long allergyNum) 
			=> Database.ExecuteNonQuery("DELETE FROM allergy WHERE AllergyNum = " + allergyNum);

		/// <summary>
		/// Gets all allergies for patient whether active or not.
		/// </summary>
		public static List<Allergy> GetAll(long patNum, bool showInactive)
		{
			string command = "SELECT * FROM allergy WHERE PatNum = " + patNum;
			if (!showInactive)
			{
				command += " AND StatusIsActive<>0";
			}

			return Crud.AllergyCrud.SelectMany(command).ToList();
		}

		public static List<long> GetChangedSinceAllergyNums(DateTime changedSince)
		{
			string command = "SELECT AllergyNum FROM allergy WHERE DateTStamp > " + POut.DateT(changedSince);
			DataTable dt = Database.ExecuteDataTable(command);
			List<long> allergynums = new List<long>(dt.Rows.Count);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				allergynums.Add(PIn.Long(dt.Rows[i]["AllergyNum"].ToString()));
			}
			return allergynums;
		}

		/// <summary>
		/// Used along with GetChangedSinceAllergyNums
		/// </summary>
		public static List<Allergy> GetMultAllergies(List<long> allergyNums)
		{
			string strAllergyNums = "";

			if (allergyNums.Count > 0)
			{
				for (int i = 0; i < allergyNums.Count; i++)
				{
					if (i > 0)
					{
						strAllergyNums += "OR ";
					}
					strAllergyNums += "AllergyNum='" + allergyNums[i].ToString() + "' ";
				}
				string command = "SELECT * FROM allergy WHERE " + strAllergyNums;

				return Crud.AllergyCrud.SelectMany(command).ToList();
			}

			return new List<Allergy>();
		}

		///<summary>Returns an array of all patient names who are using this allergy.</summary>
		public static string[] GetPatNamesForAllergy(long allergyDefNum)
		{
			string command = "SELECT CONCAT(CONCAT(CONCAT(CONCAT(LName,', '),FName),' '),Preferred) FROM allergy,patient "
				+ "WHERE allergy.PatNum=patient.PatNum "
				+ "AND allergy.AllergyDefNum=" + POut.Long(allergyDefNum);
			DataTable table = Database.ExecuteDataTable(command);
			string[] retVal = new string[table.Rows.Count];
			for (int i = 0; i < table.Rows.Count; i++)
			{
				retVal[i] = PIn.String(table.Rows[i][0].ToString());
			}
			return retVal;
		}

		///<summary>Returns a list of PatNums that have an allergy from the PatNums that are passed in.</summary>
		public static List<long> GetPatientsWithAllergy(List<long> listPatNums)
		{
			if (listPatNums.Count == 0)
			{
				return new List<long>();
			}
			string command = "SELECT DISTINCT PatNum FROM allergy WHERE PatNum IN (" + string.Join(",", listPatNums) + ") "
				+ "AND allergy.AllergyDefNum != " + POut.Long(PrefC.GetLong(PrefName.AllergiesIndicateNone));
			return Database.GetListLong(command);
		}

		///<summary>Changes the value of the DateTStamp column to the current time stamp for all allergies of a patient</summary>
		public static void ResetTimeStamps(long patNum)
		{
			string command = "UPDATE allergy SET DateTStamp = CURRENT_TIMESTAMP WHERE PatNum =" + POut.Long(patNum);
			Database.ExecuteNonQuery(command);
		}

		///<summary>Changes the value of the DateTStamp column to the current time stamp for all allergies of a patient that are the status specified</summary>
		public static void ResetTimeStamps(long patNum, bool onlyActive)
		{
			string command = "UPDATE allergy SET DateTStamp = CURRENT_TIMESTAMP WHERE PatNum =" + POut.Long(patNum);
			if (onlyActive)
			{
				command += " AND StatusIsActive = " + POut.Bool(onlyActive);
			}
			Database.ExecuteNonQuery(command);
		}
	}
}
