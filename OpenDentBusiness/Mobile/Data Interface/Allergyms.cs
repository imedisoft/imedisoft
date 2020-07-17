using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness.Mobile
{
	public class Allergyms
	{
		#region Only used for webserver for mobile.
		///<summary>Gets all Allergym for a single patient </summary>
		public static List<Allergym> GetAllergyms(long customerNum, long patNum)
		{
			string command =
					"SELECT * from allergym "
					+ "WHERE CustomerNum = " + POut.Long(customerNum)
					+ " AND PatNum = " + POut.Long(patNum);
			return Crud.AllergymCrud.SelectMany(command);
		}

		public static DataTable GetAllergymDetails(long customerNum, long patNum)
		{
			string command =
				"SELECT  allergydefm.Description, allergym.Reaction from allergym  LEFT JOIN allergydefm on allergym.AllergyDefNum=allergydefm.AllergyDefNum "
				+ "WHERE allergym.CustomerNum = " + POut.Long(customerNum)
					+ " AND allergym.PatNum = " + POut.Long(patNum)
					+ " AND allergym.StatusIsActive = " + POut.Bool(true) // get only active allergies
					+ " AND allergydefm.CustomerNum = " + POut.Long(customerNum);
			return Database.ExecuteDataTable(command);
		}

		#endregion

		#region Used only on OD
		///<summary>The values returned are sent to the webserver.</summary>
		public static List<long> GetChangedSinceAllergyNums(DateTime changedSince)
		{
			return Allergies.GetChangedSinceAllergyNums(changedSince);
		}

		///<summary>The values returned are sent to the webserver.</summary>
		public static List<Allergym> GetMultAllergyms(List<long> allergyNums)
		{
			List<Allergy> allergyList = Allergies.GetMultAllergies(allergyNums);
			List<Allergym> allergymList = ConvertListToM(allergyList);
			return allergymList;
		}

		///<summary>First use GetChangedSince.  Then, use this to convert the list a list of 'm' objects.</summary>
		public static List<Allergym> ConvertListToM(List<Allergy> list)
		{
			List<Allergym> retVal = new List<Allergym>();
			for (int i = 0; i < list.Count; i++)
			{
				retVal.Add(Crud.AllergymCrud.ConvertToM(list[i]));
			}
			return retVal;
		}
		#endregion

		#region Used only on the Mobile webservice server for  synching.
		///<summary>Only run on server for mobile.  Takes the list of changes from the dental office and makes updates to those items in the mobile server db.  Also, make sure to run DeletedObjects.DeleteForMobile().</summary>
		public static void UpdateFromChangeList(List<Allergym> list, long customerNum)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list[i].CustomerNum = customerNum;
				Allergym allergym = Crud.AllergymCrud.SelectOne(customerNum, list[i].AllergyNum);
				if (allergym == null)
				{//not in db
					Crud.AllergymCrud.Insert(list[i], true);
				}
				else
				{
					Crud.AllergymCrud.Update(list[i]);
				}
			}
		}

		///<summary>used in tandem with Full synch</summary>
		public static void DeleteAll(long customerNum)
		{
			string command = "DELETE FROM allergym WHERE CustomerNum = " + POut.Long(customerNum); ;
			Database.ExecuteNonQuery(command);
		}

		///<summary>Delete all allergies of a particular patient</summary>
		public static void Delete(long customerNum, long PatNum)
		{
			string command = "DELETE FROM allergym WHERE CustomerNum = " + POut.Long(customerNum) + " AND PatNum = " + POut.Long(PatNum);
			Database.ExecuteNonQuery(command);
		}
		#endregion
	}
}
