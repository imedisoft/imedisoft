//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: v4.0.30319
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
	public partial class AlertItems
	{
		public static AlertItem FromReader(MySqlDataReader dataReader)
		{
			return new AlertItem
			{
				Id = (long)dataReader["id"],
				ClinicId = dataReader["clinic_id"] as long?,
				UserId = dataReader["user_id"] as long?,
				Description = (string)dataReader["description"],
				Details = (string)dataReader["details"],
				Type = (string)dataReader["type"],
				Severity = (AlertSeverityType)Convert.ToInt32(dataReader["severity"]),
				Actions = (AlertAction)Convert.ToInt32(dataReader["actions"]),
				FormToOpen = (FormType)Convert.ToInt32(dataReader["form_to_open"]),
				ObjectId = dataReader["object_id"] as long?
			};
		}

		/// <summary>
		/// Selects a single AlertItem object from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static AlertItem SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="AlertItem"/> object with the specified key from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="AlertItem"/> to select.</param>
		public static AlertItem SelectOne(long id)
			=> SelectOne("SELECT * FROM `alert_items` WHERE `id` = " + id);

		/// <summary>
		/// Selects multiple <see cref="AlertItem"/> objects from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static IEnumerable<AlertItem> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="AlertItem"/> into the database.
		/// </summary>
		/// <param name="alertItem">The <see cref="AlertItem"/> to insert into the database.</param>
		private static long ExecuteInsert(AlertItem alertItem)
			=> alertItem.Id = Database.ExecuteInsert(
				"INSERT INTO `alert_items` " +
				"(`clinic_id`, `user_id`, `description`, `details`, `type`, `severity`, `actions`, `form_to_open`, `object_id`) " +
				"VALUES (" +
					"@clinic_id, @user_id, @description, @details, @type, @severity, @actions, @form_to_open, @object_id" +
				")",
					new MySqlParameter("clinic_id", (alertItem.ClinicId.HasValue ? (object)alertItem.ClinicId.Value : DBNull.Value)),
					new MySqlParameter("user_id", (alertItem.UserId.HasValue ? (object)alertItem.UserId.Value : DBNull.Value)),
					new MySqlParameter("description", alertItem.Description ?? ""),
					new MySqlParameter("details", alertItem.Details ?? ""),
					new MySqlParameter("type", alertItem.Type ?? ""),
					new MySqlParameter("severity", (int)alertItem.Severity),
					new MySqlParameter("actions", (int)alertItem.Actions),
					new MySqlParameter("form_to_open", (int)alertItem.FormToOpen),
					new MySqlParameter("object_id", (alertItem.ObjectId.HasValue ? (object)alertItem.ObjectId.Value : DBNull.Value)));

		/// <summary>
		/// Updates the specified <see cref="AlertItem"/> in the database.
		/// </summary>
		/// <param name="alertItem">The <see cref="AlertItem"/> to update.</param>
		private static void ExecuteUpdate(AlertItem alertItem)
			=> Database.ExecuteNonQuery(
				"UPDATE `alert_items` SET " +
					"`clinic_id` = @clinic_id, " +
					"`user_id` = @user_id, " +
					"`description` = @description, " +
					"`details` = @details, " +
					"`type` = @type, " +
					"`severity` = @severity, " +
					"`actions` = @actions, " +
					"`form_to_open` = @form_to_open, " +
					"`object_id` = @object_id " +
				"WHERE `id` = @id",
					new MySqlParameter("id", alertItem.Id),
					new MySqlParameter("clinic_id", (alertItem.ClinicId.HasValue ? (object)alertItem.ClinicId.Value : DBNull.Value)),
					new MySqlParameter("user_id", (alertItem.UserId.HasValue ? (object)alertItem.UserId.Value : DBNull.Value)),
					new MySqlParameter("description", alertItem.Description ?? ""),
					new MySqlParameter("details", alertItem.Details ?? ""),
					new MySqlParameter("type", alertItem.Type ?? ""),
					new MySqlParameter("severity", (int)alertItem.Severity),
					new MySqlParameter("actions", (int)alertItem.Actions),
					new MySqlParameter("form_to_open", (int)alertItem.FormToOpen),
					new MySqlParameter("object_id", (alertItem.ObjectId.HasValue ? (object)alertItem.ObjectId.Value : DBNull.Value)));

		/// <summary>
		/// Deletes a single <see cref="AlertItem"/> object from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="AlertItem"/> to delete.</param>
		private static void ExecuteDelete(long id)
			 => Database.ExecuteNonQuery("DELETE FROM `alert_items` WHERE `id` = " + id);

		/// <summary>
		/// Deletes the specified <see cref="AlertItem"/> object from the database.
		/// </summary>
		/// <param name="alertItem">The <see cref="AlertItem"/> to delete.</param>
		private static void ExecuteDelete(AlertItem alertItem)
			=> ExecuteDelete(alertItem.Id);
	}
}