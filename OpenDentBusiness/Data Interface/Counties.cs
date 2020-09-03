using Imedisoft.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
	public class Counties
	{
		/// <summary>
		/// Gets county names similar to the one provided.
		/// </summary>
		public static County[] Refresh(string name) 
			=> Crud.CountyCrud.SelectMany("SELECT * from county WHERE CountyName LIKE '" + POut.String(name) + "%' ORDER BY CountyName").ToArray();

		public static List<County> GetAll() 
			=> Crud.CountyCrud.SelectMany("SELECT * from `counties` ORDER BY `name`");

		/// <summary>
		/// Gets an array of strings containing all the counties in alphabetical order. 
		/// Used for the screening interface which must be simpler than the usual interface.
		/// </summary>
		public static string[] GetListNames()
		{
			DataTable table = Database.ExecuteDataTable("SELECT CountyName from county ORDER BY CountyName");

			string[] ListNames = new string[table.Rows.Count];
			for (int i = 0; i < ListNames.Length; i++)
			{
				ListNames[i] = PIn.String(table.Rows[i]["CountyName"].ToString());
			}

			return ListNames;
		}

		public static long Insert(County county) 
			=> Crud.CountyCrud.Insert(county);

		public static void Update(County county) 
			=> Database.ExecuteNonQuery(
				"UPDATE `counties` SET `name` ='" + POut.String(county.Name) + "', `code` ='" + POut.String(county.Code) + "' " +
				"WHERE `id` = " + county.Id);

		public static void Save(County county)
		{
			if (county.Id == 0) Insert(county);
            else
            {
				Update(county);
            }
		}

		public static void Delete(County county) 
			=> Database.ExecuteNonQuery(
				"DELETE FROM `counties` WHERE `id` = " + county.Id);

		/// <summary>
		/// Use before DeleteCur to determine if this County name is in use. 
		/// Returns a formatted string that can be used to quickly display the names of all patients using the Countyname.
		/// </summary>
		public static string UsedBy(string countyName)
		{
			DataTable table = Database.ExecuteDataTable("SELECT LName,FName FROM patient WHERE County = '" + POut.String(countyName) + "'");
			if (table.Rows.Count == 0)
			{
				return "";
			}

			string retVal = "";
			for (int i = 0; i < table.Rows.Count; i++)
			{
				retVal += PIn.String(table.Rows[i][0].ToString()) + ", "
					+ PIn.String(table.Rows[i][1].ToString());
				if (i < table.Rows.Count - 1)
				{//if not the last row
					retVal += "\r";
				}
			}

			return retVal;
		}

		/// <summary>
		/// Checks whether a county with the specified <paramref name="name"/> exists in the database.
		/// </summary>
		/// <param name="name">The county name.</param>
		/// <param name="excludeId">The ID of the county to ignore.</param>
		/// <returns>True if the county exists; otherwise, false.</returns>
		public static bool Exists(string name, long excludeId = 0)
		{
			var count = Database.ExecuteLong(
				"SELECT COUNT(`id`) FROM `counties` WHERE `name` = @name AND `id` != " + excludeId,
					new MySqlParameter("name", name));

			return count > 0;
		}
	}
}
