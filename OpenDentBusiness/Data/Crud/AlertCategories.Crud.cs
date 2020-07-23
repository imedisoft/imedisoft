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
using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenDentBusiness
{
	public partial class AlertCategories
	{
		public static AlertCategory FromReader(MySqlDataReader dataReader)
		{
			return new AlertCategory
			{
				Id = (long)dataReader["id"],
				IsHqCategory = (Convert.ToInt32(dataReader["is_hq_category"]) == 1),
				InternalName = (string)dataReader["internal_name"],
				Description = (string)dataReader["description"]
			};
		}

		/// <summary>
		/// Selects a single AlertCategory object from the database using the specified SQL command.
		/// </summary>
		public static AlertCategory SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="AlertCategory"/> object with the specified key from the database.
		/// </summary>
		public static AlertCategory SelectOne(Int64 id)
			=> SelectOne("SELECT * FROM `alertcategory` WHERE `id` = " + id);

		/// <summary>
		/// Selects multiple <see cref="AlertCategory"/> objects from the database using the specified SQL command.
		/// </summary>
		public static IEnumerable<AlertCategory> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="AlertCategory"/> into the database.
		/// </summary>
		public static long Insert(AlertCategory alertCategory)
			=> alertCategory.Id = Database.ExecuteInsert(
				"INSERT INTO `alertcategory` " +
				"(`is_hq_category`, `internal_name`, `description`) " +
				"VALUES (" +
					"@is_hq_category, @internal_name, @description" +
				")");

		/// <summary>
		/// Updates the specified <see cref="AlertCategory"/> in the database.
		/// </summary>
		public static void Update(AlertCategory alertCategory)
			=> Database.ExecuteNonQuery(
				"UPDATE `alertcategory` SET " +
					"`is_hq_category` = @is_hq_category, " +
					"`internal_name` = @internal_name, " +
					"`description` = @description " +
				"WHERE `id` = @id",
					new MySqlParameter("id", alertCategory.Id),
					new MySqlParameter("is_hq_category", (alertCategory.IsHqCategory ? 1 : 0)),
					new MySqlParameter("internal_name", alertCategory.InternalName ?? ""),
					new MySqlParameter("description", alertCategory.Description ?? ""));

		/// <summary>
		/// Updates the specified <see cref="AlertCategory"/> in the database.
		/// </summary>
		public static bool Update(AlertCategory alertcategoryNew, AlertCategory alertcategoryOld)
		{
			var updates = new List<string>();
			var parameters = new List<MySqlParameter>();

			if (alertcategoryNew.IsHqCategory != alertcategoryOld.IsHqCategory)
			{
				updates.Add("`is_hq_category` = @is_hq_category");
				parameters.Add(new MySqlParameter("is_hq_category", (alertcategoryNew.IsHqCategory ? 1 : 0)));
			}

			if (alertcategoryNew.InternalName != alertcategoryOld.InternalName)
			{
				updates.Add("`internal_name` = @internal_name");
				parameters.Add(new MySqlParameter("internal_name", alertcategoryNew.InternalName ?? ""));
			}

			if (alertcategoryNew.Description != alertcategoryOld.Description)
			{
				updates.Add("`description` = @description");
				parameters.Add(new MySqlParameter("description", alertcategoryNew.Description ?? ""));
			}

			if (updates.Count == 0) return false;

			parameters.Add(new MySqlParameter("id", alertcategoryNew.Id));

			Database.ExecuteNonQuery("UPDATE `alertcategory` " +
				"SET " + string.Join(", ", updates) + " " +
				"WHERE `id` = @id",
					parameters.ToArray());

			return true;
		}

		/// <summary>
		/// Deletes a single <see cref="AlertCategory"/> object from the database.
		/// </summary>
		public static void Delete(Int64 id)
			 => Database.ExecuteNonQuery("DELETE FROM `alertcategory` WHERE `id` = " + id);

		/// <summary>
		/// Deletes the specified <see cref="AlertCategory"/> object from the database.
		/// </summary>
		public static void Delete(AlertCategory alertCategory)
			=> Delete(alertCategory.Id);
	}
}
