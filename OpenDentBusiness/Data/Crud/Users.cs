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
	public partial class Userods
	{
		public static Userod FromReader(MySqlDataReader dataReader)
		{
			return new Userod
			{
				Id = (long)dataReader["id"],
				UserName = (string)dataReader["user_name"],
				PasswordHash = (string)dataReader["password_hash"],
				PasswordIsStrong = (Convert.ToInt32(dataReader["password_is_strong"]) == 1),
				EmployeeId = dataReader["employee_id"] as long?,
				ClinicId = dataReader["clinic_id"] as long?,
				ProviderId = dataReader["provider_id"] as long?,
				IsHidden = (Convert.ToInt32(dataReader["is_hidden"]) == 1),
				InboxTaskListId = dataReader["inbox_task_list_id"] as long?,
				RootTaskListId = dataReader["root_task_list_id"] as long?,
				DefaultHidePopups = (Convert.ToInt32(dataReader["default_hide_popups"]) == 1),
				ClinicIsRestricted = (Convert.ToInt32(dataReader["clinic_is_restricted"]) == 1),
				InboxHidePopups = (Convert.ToInt32(dataReader["inbox_hide_popups"]) == 1),
				UserIdCEMT = dataReader["cemt_user_id"] as long?,
				FailedLoginDate = dataReader["failed_login_date"] as DateTime?,
				FailedAttempts = (int)dataReader["failed_attempts"],
				DomainUser = (string)dataReader["domain_user"],
				IsPasswordResetRequired = (Convert.ToInt32(dataReader["is_password_reset_required"]) == 1),
				MobileWebPin = (string)dataReader["mobile_web_pin"],
				MobileWebPinFailedAttempts = (int)dataReader["mobile_web_pin_failed_attempts"],
				LastLoginDate = (DateTime)dataReader["last_login_date"]
			};
		}

		/// <summary>
		/// Selects a single Userod object from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static Userod SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="Userod"/> object with the specified key from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="Userod"/> to select.</param>
		public static Userod SelectOne(long id)
			=> SelectOne("SELECT * FROM `users` WHERE `id` = " + id);

		/// <summary>
		/// Selects multiple <see cref="Userod"/> objects from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static IEnumerable<Userod> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="Userod"/> into the database.
		/// </summary>
		/// <param name="userod">The <see cref="Userod"/> to insert into the database.</param>
		private static long ExecuteInsert(Userod userod)
			=> userod.Id = Database.ExecuteInsert(
				"INSERT INTO `users` " +
				"(`user_name`, `password_hash`, `password_is_strong`, `employee_id`, `clinic_id`, `provider_id`, `is_hidden`, `inbox_task_list_id`, `root_task_list_id`, `default_hide_popups`, `clinic_is_restricted`, `inbox_hide_popups`, `cemt_user_id`, `failed_login_date`, `failed_attempts`, `domain_user`, `is_password_reset_required`, `mobile_web_pin`, `mobile_web_pin_failed_attempts`, `last_login_date`) " +
				"VALUES (" +
					"@user_name, @password_hash, @password_is_strong, @employee_id, @clinic_id, @provider_id, @is_hidden, @inbox_task_list_id, @root_task_list_id, @default_hide_popups, @clinic_is_restricted, @inbox_hide_popups, @cemt_user_id, @failed_login_date, @failed_attempts, @domain_user, @is_password_reset_required, @mobile_web_pin, @mobile_web_pin_failed_attempts, @last_login_date" +
				")",
					new MySqlParameter("user_name", userod.UserName ?? ""),
					new MySqlParameter("password_hash", userod.PasswordHash ?? ""),
					new MySqlParameter("password_is_strong", (userod.PasswordIsStrong ? 1 : 0)),
					new MySqlParameter("employee_id", (userod.EmployeeId.HasValue ? (object)userod.EmployeeId.Value : DBNull.Value)),
					new MySqlParameter("clinic_id", (userod.ClinicId.HasValue ? (object)userod.ClinicId.Value : DBNull.Value)),
					new MySqlParameter("provider_id", (userod.ProviderId.HasValue ? (object)userod.ProviderId.Value : DBNull.Value)),
					new MySqlParameter("is_hidden", (userod.IsHidden ? 1 : 0)),
					new MySqlParameter("inbox_task_list_id", (userod.InboxTaskListId.HasValue ? (object)userod.InboxTaskListId.Value : DBNull.Value)),
					new MySqlParameter("root_task_list_id", (userod.RootTaskListId.HasValue ? (object)userod.RootTaskListId.Value : DBNull.Value)),
					new MySqlParameter("default_hide_popups", (userod.DefaultHidePopups ? 1 : 0)),
					new MySqlParameter("clinic_is_restricted", (userod.ClinicIsRestricted ? 1 : 0)),
					new MySqlParameter("inbox_hide_popups", (userod.InboxHidePopups ? 1 : 0)),
					new MySqlParameter("cemt_user_id", (userod.UserIdCEMT.HasValue ? (object)userod.UserIdCEMT.Value : DBNull.Value)),
					new MySqlParameter("failed_login_date", (userod.FailedLoginDate.HasValue ? (object)userod.FailedLoginDate.Value : DBNull.Value)),
					new MySqlParameter("failed_attempts", userod.FailedAttempts),
					new MySqlParameter("domain_user", userod.DomainUser ?? ""),
					new MySqlParameter("is_password_reset_required", (userod.IsPasswordResetRequired ? 1 : 0)),
					new MySqlParameter("mobile_web_pin", userod.MobileWebPin ?? ""),
					new MySqlParameter("mobile_web_pin_failed_attempts", userod.MobileWebPinFailedAttempts),
					new MySqlParameter("last_login_date", userod.LastLoginDate));

		/// <summary>
		/// Updates the specified <see cref="Userod"/> in the database.
		/// </summary>
		/// <param name="userod">The <see cref="Userod"/> to update.</param>
		private static void ExecuteUpdate(Userod userod)
			=> Database.ExecuteNonQuery(
				"UPDATE `users` SET " +
					"`user_name` = @user_name, " +
					"`password_hash` = @password_hash, " +
					"`password_is_strong` = @password_is_strong, " +
					"`employee_id` = @employee_id, " +
					"`clinic_id` = @clinic_id, " +
					"`provider_id` = @provider_id, " +
					"`is_hidden` = @is_hidden, " +
					"`inbox_task_list_id` = @inbox_task_list_id, " +
					"`root_task_list_id` = @root_task_list_id, " +
					"`default_hide_popups` = @default_hide_popups, " +
					"`clinic_is_restricted` = @clinic_is_restricted, " +
					"`inbox_hide_popups` = @inbox_hide_popups, " +
					"`cemt_user_id` = @cemt_user_id, " +
					"`failed_login_date` = @failed_login_date, " +
					"`failed_attempts` = @failed_attempts, " +
					"`domain_user` = @domain_user, " +
					"`is_password_reset_required` = @is_password_reset_required, " +
					"`mobile_web_pin` = @mobile_web_pin, " +
					"`mobile_web_pin_failed_attempts` = @mobile_web_pin_failed_attempts, " +
					"`last_login_date` = @last_login_date " +
				"WHERE `id` = @id",
					new MySqlParameter("id", userod.Id),
					new MySqlParameter("user_name", userod.UserName ?? ""),
					new MySqlParameter("password_hash", userod.PasswordHash ?? ""),
					new MySqlParameter("password_is_strong", (userod.PasswordIsStrong ? 1 : 0)),
					new MySqlParameter("employee_id", (userod.EmployeeId.HasValue ? (object)userod.EmployeeId.Value : DBNull.Value)),
					new MySqlParameter("clinic_id", (userod.ClinicId.HasValue ? (object)userod.ClinicId.Value : DBNull.Value)),
					new MySqlParameter("provider_id", (userod.ProviderId.HasValue ? (object)userod.ProviderId.Value : DBNull.Value)),
					new MySqlParameter("is_hidden", (userod.IsHidden ? 1 : 0)),
					new MySqlParameter("inbox_task_list_id", (userod.InboxTaskListId.HasValue ? (object)userod.InboxTaskListId.Value : DBNull.Value)),
					new MySqlParameter("root_task_list_id", (userod.RootTaskListId.HasValue ? (object)userod.RootTaskListId.Value : DBNull.Value)),
					new MySqlParameter("default_hide_popups", (userod.DefaultHidePopups ? 1 : 0)),
					new MySqlParameter("clinic_is_restricted", (userod.ClinicIsRestricted ? 1 : 0)),
					new MySqlParameter("inbox_hide_popups", (userod.InboxHidePopups ? 1 : 0)),
					new MySqlParameter("cemt_user_id", (userod.UserIdCEMT.HasValue ? (object)userod.UserIdCEMT.Value : DBNull.Value)),
					new MySqlParameter("failed_login_date", (userod.FailedLoginDate.HasValue ? (object)userod.FailedLoginDate.Value : DBNull.Value)),
					new MySqlParameter("failed_attempts", userod.FailedAttempts),
					new MySqlParameter("domain_user", userod.DomainUser ?? ""),
					new MySqlParameter("is_password_reset_required", (userod.IsPasswordResetRequired ? 1 : 0)),
					new MySqlParameter("mobile_web_pin", userod.MobileWebPin ?? ""),
					new MySqlParameter("mobile_web_pin_failed_attempts", userod.MobileWebPinFailedAttempts),
					new MySqlParameter("last_login_date", userod.LastLoginDate));

		/// <summary>
		/// Deletes a single <see cref="Userod"/> object from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="Userod"/> to delete.</param>
		private static void ExecuteDelete(long id)
			 => Database.ExecuteNonQuery("DELETE FROM `users` WHERE `id` = " + id);

		/// <summary>
		/// Deletes the specified <see cref="Userod"/> object from the database.
		/// </summary>
		/// <param name="userod">The <see cref="Userod"/> to delete.</param>
		private static void ExecuteDelete(Userod userod)
			=> ExecuteDelete(userod.Id);
	}
}
