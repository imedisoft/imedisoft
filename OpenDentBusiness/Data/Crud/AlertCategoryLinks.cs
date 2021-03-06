//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: v4.0.30319
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class AlertCategoryLinks
	{
		public static AlertCategoryLink FromReader(MySqlDataReader dataReader)
		{
			return new AlertCategoryLink
			{
				Id = (long)dataReader["id"],
				AlertCategoryId = (long)dataReader["alert_category_id"],
				Type = (string)dataReader["type"]
			};
		}

		/// <summary>
		/// Selects a single AlertCategoryLink object from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static AlertCategoryLink SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="AlertCategoryLink"/> object with the specified key from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="AlertCategoryLink"/> to select.</param>
		public static AlertCategoryLink SelectOne(long id)
			=> SelectOne("SELECT * FROM `alert_category_links` WHERE `id` = " + id);

		/// <summary>
		/// Selects multiple <see cref="AlertCategoryLink"/> objects from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static IEnumerable<AlertCategoryLink> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="AlertCategoryLink"/> into the database.
		/// </summary>
		/// <param name="alertCategoryLink">The <see cref="AlertCategoryLink"/> to insert into the database.</param>
		private static long ExecuteInsert(AlertCategoryLink alertCategoryLink)
			=> alertCategoryLink.Id = Database.ExecuteInsert(
				"INSERT INTO `alert_category_links` " +
				"(`alert_category_id`, `type`) " +
				"VALUES (" +
					"@alert_category_id, @type" +
				")",
					new MySqlParameter("alert_category_id", alertCategoryLink.AlertCategoryId),
					new MySqlParameter("type", alertCategoryLink.Type ?? ""));

		/// <summary>
		/// Updates the specified <see cref="AlertCategoryLink"/> in the database.
		/// </summary>
		/// <param name="alertCategoryLink">The <see cref="AlertCategoryLink"/> to update.</param>
		private static void ExecuteUpdate(AlertCategoryLink alertCategoryLink)
			=> Database.ExecuteNonQuery(
				"UPDATE `alert_category_links` SET " +
					"`alert_category_id` = @alert_category_id, " +
					"`type` = @type " +
				"WHERE `id` = @id",
					new MySqlParameter("id", alertCategoryLink.Id),
					new MySqlParameter("alert_category_id", alertCategoryLink.AlertCategoryId),
					new MySqlParameter("type", alertCategoryLink.Type ?? ""));

		/// <summary>
		/// Deletes a single <see cref="AlertCategoryLink"/> object from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="AlertCategoryLink"/> to delete.</param>
		private static void ExecuteDelete(long id)
			 => Database.ExecuteNonQuery("DELETE FROM `alert_category_links` WHERE `id` = " + id);

		/// <summary>
		/// Deletes the specified <see cref="AlertCategoryLink"/> object from the database.
		/// </summary>
		/// <param name="alertCategoryLink">The <see cref="AlertCategoryLink"/> to delete.</param>
		private static void ExecuteDelete(AlertCategoryLink alertCategoryLink)
			=> ExecuteDelete(alertCategoryLink.Id);
	}
}
