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
	public partial class AlertSubs
	{
		public static AlertSub FromReader(MySqlDataReader dataReader)
		{
			return new AlertSub
			{
				Id = (long)dataReader["id"],
				UserId = (long)dataReader["user_id"],
				ClinicId = dataReader["clinic_id"] as long?,
				AlertCategoryId = (long)dataReader["alert_category_id"]
			};
		}

		/// <summary>
		/// Selects a single AlertSub object from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static AlertSub SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="AlertSub"/> object with the specified key from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="AlertSub"/> to select.</param>
		public static AlertSub SelectOne(long id)
			=> SelectOne("SELECT * FROM `alert_subs` WHERE `id` = " + id);

		/// <summary>
		/// Selects multiple <see cref="AlertSub"/> objects from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static IEnumerable<AlertSub> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="AlertSub"/> into the database.
		/// </summary>
		/// <param name="alertSub">The <see cref="AlertSub"/> to insert into the database.</param>
		private static long ExecuteInsert(AlertSub alertSub)
			=> alertSub.Id = Database.ExecuteInsert(
				"INSERT INTO `alert_subs` " +
				"(`user_id`, `clinic_id`, `alert_category_id`) " +
				"VALUES (" +
					"@user_id, @clinic_id, @alert_category_id" +
				")");

		/// <summary>
		/// Updates the specified <see cref="AlertSub"/> in the database.
		/// </summary>
		/// <param name="alertSub">The <see cref="AlertSub"/> to update.</param>
		private static void ExecuteUpdate(AlertSub alertSub)
			=> Database.ExecuteNonQuery(
				"UPDATE `alert_subs` SET " +
					"`user_id` = @user_id, " +
					"`clinic_id` = @clinic_id, " +
					"`alert_category_id` = @alert_category_id " +
				"WHERE `id` = @id",
					new MySqlParameter("id", alertSub.Id),
					new MySqlParameter("user_id", alertSub.UserId),
					new MySqlParameter("clinic_id", (alertSub.ClinicId.HasValue ? (object)alertSub.ClinicId.Value : DBNull.Value)),
					new MySqlParameter("alert_category_id", alertSub.AlertCategoryId));

		/// <summary>
		/// Deletes a single <see cref="AlertSub"/> object from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="AlertSub"/> to delete.</param>
		private static void ExecuteDelete(long id)
			 => Database.ExecuteNonQuery("DELETE FROM `alert_subs` WHERE `id` = " + id);

		/// <summary>
		/// Deletes the specified <see cref="AlertSub"/> object from the database.
		/// </summary>
		/// <param name="alertSub">The <see cref="AlertSub"/> to delete.</param>
		private static void ExecuteDelete(AlertSub alertSub)
			=> ExecuteDelete(alertSub.Id);
	}
}
