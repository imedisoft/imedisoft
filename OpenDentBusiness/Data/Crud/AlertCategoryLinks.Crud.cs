//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: v4.0.30319
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Imedisoft.Data;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenDentBusiness
{
	public partial class AlertCategoryLinks
	{
		public static AlertCategoryLink FromReader(MySqlDataReader dataReader)
		{
			return new AlertCategoryLink
			{
				Id = (long)dataReader["id"],
				AlertCategoryId = (long)dataReader["alert_category_id"],
				AlertType = (AlertType)Convert.ToInt32(dataReader["alert_type"])
			};
		}

		/// <summary>
		/// Selects a single AlertCategoryLink object from the database using the specified SQL command.
		/// </summary>
		public static AlertCategoryLink SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="AlertCategoryLink"/> object with the specified key from the database.
		/// </summary>
		public static AlertCategoryLink SelectOne(Int64 id)
			=> SelectOne("SELECT * FROM `alertcategorylink` WHERE `id` = " + id);

		/// <summary>
		/// Selects multiple <see cref="AlertCategoryLink"/> objects from the database using the specified SQL command.
		/// </summary>
		public static IEnumerable<AlertCategoryLink> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="AlertCategoryLink"/> into the database.
		/// </summary>
		public static long Insert(AlertCategoryLink alertCategoryLink)
			=> alertCategoryLink.Id = Database.ExecuteInsert(
				"INSERT INTO `alertcategorylink` " +
				"(`alert_category_id`, `alert_type`) " +
				"VALUES (" +
					"@alert_category_id, @alert_type" +
				")");

		/// <summary>
		/// Updates the specified <see cref="AlertCategoryLink"/> in the database.
		/// </summary>
		public static void Update(AlertCategoryLink alertCategoryLink)
			=> Database.ExecuteNonQuery(
				"UPDATE `alertcategorylink` SET " +
					"`alert_category_id` = @alert_category_id, " +
					"`alert_type` = @alert_type " +
				"WHERE `id` = @id",
					new MySqlParameter("id", alertCategoryLink.Id),
					new MySqlParameter("alert_category_id", alertCategoryLink.AlertCategoryId),
					new MySqlParameter("alert_type", (int)alertCategoryLink.AlertType));

		/// <summary>
		/// Updates the specified <see cref="AlertCategoryLink"/> in the database.
		/// </summary>
		public static bool Update(AlertCategoryLink alertcategorylinkNew, AlertCategoryLink alertcategorylinkOld)
		{
			var updates = new List<string>();
			var parameters = new List<MySqlParameter>();

			if (alertcategorylinkNew.AlertCategoryId != alertcategorylinkOld.AlertCategoryId)
			{
				updates.Add("`alert_category_id` = @alert_category_id");
				parameters.Add(new MySqlParameter("alert_category_id", alertcategorylinkNew.AlertCategoryId));
			}

			if (alertcategorylinkNew.AlertType != alertcategorylinkOld.AlertType)
			{
				updates.Add("`alert_type` = @alert_type");
				parameters.Add(new MySqlParameter("alert_type", (int)alertcategorylinkNew.AlertType));
			}

			if (updates.Count == 0) return false;

			parameters.Add(new MySqlParameter("id", alertcategorylinkNew.Id));

			Database.ExecuteNonQuery("UPDATE `alertcategorylink` " +
				"SET " + string.Join(", ", updates) + " " +
				"WHERE `id` = @id",
					parameters.ToArray());

			return true;
		}

		/// <summary>
		/// Deletes a single <see cref="AlertCategoryLink"/> object from the database.
		/// </summary>
		public static void Delete(Int64 id)
			 => Database.ExecuteNonQuery("DELETE FROM `alertcategorylink` WHERE `id` = " + id);

		/// <summary>
		/// Deletes the specified <see cref="AlertCategoryLink"/> object from the database.
		/// </summary>
		public static void Delete(AlertCategoryLink alertCategoryLink)
			=> Delete(alertCategoryLink.Id);
	}
}
