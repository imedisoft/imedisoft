using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public class ApptFieldDefs
	{
		private class ApptFieldDefCache : CacheListAbs<ApptFieldDef>
		{
			protected override List<ApptFieldDef> GetCacheFromDb() 
				=> Crud.ApptFieldDefCrud.SelectMany("SELECT * FROM apptfielddef ORDER BY FieldName");

			protected override List<ApptFieldDef> TableToList(DataTable table) 
				=> Crud.ApptFieldDefCrud.TableToList(table);

			protected override ApptFieldDef Copy(ApptFieldDef apptFieldDef) 
				=> apptFieldDef.Clone();

			protected override DataTable ListToTable(List<ApptFieldDef> listApptFieldDefs) 
				=> Crud.ApptFieldDefCrud.ListToTable(listApptFieldDefs, "ApptFieldDef");

			protected override void FillCacheIfNeeded() 
				=> ApptFieldDefs.GetTableFromCache(false);
		}

		private static readonly ApptFieldDefCache cache = new ApptFieldDefCache();

		public static bool GetExists(Predicate<ApptFieldDef> match, bool isShort = false) 
			=> cache.GetExists(match, isShort);

		public static List<ApptFieldDef> GetDeepCopy(bool isShort = false) 
			=> cache.GetDeepCopy(isShort);

		public static ApptFieldDef GetFirstOrDefault(Func<ApptFieldDef, bool> match, bool isShort = false) 
			=> cache.GetFirstOrDefault(match, isShort);

		public static DataTable RefreshCache() 
			=> GetTableFromCache(true);

		public static DataTable GetTableFromCache(bool doRefreshCache) 
			=> cache.GetTableFromCache(doRefreshCache);

		/// <summary>
		/// Must supply the old field name so that the apptFields attached to appointments can be updated.
		/// Will throw exception if new FieldName is already in use.
		/// </summary>
		public static void Update(ApptFieldDef apptFieldDef, string oldFieldName)
		{
			string command = 
				"SELECT COUNT(*) FROM apptfielddef " +
				"WHERE FieldName='" + POut.String(apptFieldDef.FieldName) + "' " +
				"AND ApptFieldDefNum != " + apptFieldDef.ApptFieldDefNum;

			if (Database.ExecuteLong(command) != 0)
			{
				throw new ApplicationException("Field name already in use.");
			}

			Crud.ApptFieldDefCrud.Update(apptFieldDef);

			Database.ExecuteNonQuery(
				"UPDATE apptfield SET FieldName='" + POut.String(apptFieldDef.FieldName) + "' " +
				"WHERE FieldName='" + POut.String(oldFieldName) + "'");
		}

		/// <summary>
		/// Surround with try/catch in case field name already in use.
		/// </summary>
		public static long Insert(ApptFieldDef apptFieldDef)
		{
			string command = "SELECT COUNT(*) FROM apptfielddef WHERE FieldName='" + POut.String(apptFieldDef.FieldName) + "'";
			if (Database.ExecuteString(command) != "0")
			{
				throw new ApplicationException("Field name already in use.");
			}

			return Crud.ApptFieldDefCrud.Insert(apptFieldDef);
		}

		/// <summary>
		/// Surround with try/catch, because it will throw an exception if any appointment is using this def.
		/// </summary>
		public static void Delete(ApptFieldDef apptFieldDef)
		{
			string command = "SELECT LName,FName,AptDateTime "
				+ "FROM patient,apptfield,appointment WHERE "
				+ "patient.PatNum=appointment.PatNum "
				+ "AND appointment.AptNum=apptfield.AptNum "
				+ "AND FieldName='" + POut.String(apptFieldDef.FieldName) + "'";

			DataTable table = Database.ExecuteDataTable(command);

			DateTime aptDateTime;
			if (table.Rows.Count > 0)
			{
				string s = "Not allowed to delete. Already in use by " + table.Rows.Count.ToString() + " appointments, including\r\n";

				for (int i = 0; i < table.Rows.Count; i++)
				{
					if (i > 5)
					{
						break;
					}

					aptDateTime = PIn.Date(table.Rows[i]["AptDateTime"].ToString());

					s += table.Rows[i]["LName"].ToString() + ", " + table.Rows[i]["FName"].ToString() + POut.DateT(aptDateTime, false) + "\r\n";
				}

				throw new ApplicationException(s);
			}

			Database.ExecuteNonQuery(
				"DELETE FROM apptfielddef WHERE ApptFieldDefNum =" + apptFieldDef.ApptFieldDefNum);
		}

		public static string GetFieldName(long apptFieldDefNum) 
			=> GetFirstOrDefault(x => x.ApptFieldDefNum == apptFieldDefNum)?.FieldName ?? "";

		/// <summary>
		/// GetPickListByFieldName returns the pick list identified by the field name passed as a parameter.
		/// </summary>
		public static string GetPickListByFieldName(string FieldName) 
			=> GetFirstOrDefault(x => x.FieldName == FieldName)?.PickList ?? "";

		/// <summary>
		/// Returns true if there are any duplicate field names in the entire apptfielddef table.
		/// </summary>
		public static bool HasDuplicateFieldNames() 
			=> Database.ExecuteLong(
				"SELECT COUNT(*) FROM apptfielddef GROUP BY FieldName HAVING COUNT(FieldName) > 1") > 0;

		/// <summary>
		/// Returns the ApptFieldDef for the specified field name. Returns null if an ApptFieldDef does not exist for that field name.
		/// </summary>
		public static ApptFieldDef GetFieldDefByFieldName(string fieldName) 
			=> GetFirstOrDefault(x => x.FieldName == fieldName);
	}
}
