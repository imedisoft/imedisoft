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
	public partial class GroupPermissions
	{
		public static GroupPermission FromReader(MySqlDataReader dataReader)
		{
			return new GroupPermission
			{
				Id = (long)dataReader["id"],
				UserGroupId = (long)dataReader["user_group_id"],
				NewerDate = dataReader["newer_date"] as DateTime?,
				NewerDays = (int)dataReader["newer_days"],
				Permission = (Permissions)Convert.ToInt32(dataReader["permission"]),
				ObjectId = dataReader["object_id"] as long?
			};
		}

		/// <summary>
		/// Selects a single GroupPermission object from the database using the specified SQL command.
		/// </summary>
		public static GroupPermission SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="GroupPermission"/> object with the specified key from the database.
		/// </summary>
		public static GroupPermission SelectOne(Int64 id)
			=> SelectOne("SELECT * FROM `group_permissions` WHERE `id` = " + id);

		/// <summary>
		/// Selects multiple <see cref="GroupPermission"/> objects from the database using the specified SQL command.
		/// </summary>
		public static IEnumerable<GroupPermission> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="GroupPermission"/> into the database.
		/// </summary>
		private static long InsertInternal(GroupPermission groupPermission)
			=> groupPermission.Id = Database.ExecuteInsert(
				"INSERT INTO `group_permissions` " +
				"(`user_group_id`, `newer_date`, `newer_days`, `permission`, `object_id`) " +
				"VALUES (" +
					"@user_group_id, @newer_date, @newer_days, @permission, @object_id" +
				")");

		/// <summary>
		/// Updates the specified <see cref="GroupPermission"/> in the database.
		/// </summary>
		private static void UpdateInternal(GroupPermission groupPermission)
			=> Database.ExecuteNonQuery(
				"UPDATE `group_permissions` SET " +
					"`user_group_id` = @user_group_id, " +
					"`newer_date` = @newer_date, " +
					"`newer_days` = @newer_days, " +
					"`permission` = @permission, " +
					"`object_id` = @object_id " +
				"WHERE `id` = @id",
					new MySqlParameter("id", groupPermission.Id),
					new MySqlParameter("user_group_id", groupPermission.UserGroupId),
					new MySqlParameter("newer_date", (groupPermission.NewerDate.HasValue ? (object)groupPermission.NewerDate.Value : DBNull.Value)),
					new MySqlParameter("newer_days", groupPermission.NewerDays),
					new MySqlParameter("permission", (int)groupPermission.Permission),
					new MySqlParameter("object_id", (groupPermission.ObjectId.HasValue ? (object)groupPermission.ObjectId.Value : DBNull.Value)));

		/// <summary>
		/// Updates the specified <see cref="GroupPermission"/> in the database.
		/// </summary>
		public static bool Update(GroupPermission groupPermissionNew, GroupPermission groupPermissionOld)
		{
			var updates = new List<string>();
			var parameters = new List<MySqlParameter>();

			if (groupPermissionNew.UserGroupId != groupPermissionOld.UserGroupId)
			{
				updates.Add("`user_group_id` = @user_group_id");
				parameters.Add(new MySqlParameter("user_group_id", groupPermissionNew.UserGroupId));
			}

			if (groupPermissionNew.NewerDate != groupPermissionOld.NewerDate)
			{
				updates.Add("`newer_date` = @newer_date");
				parameters.Add(new MySqlParameter("newer_date", (groupPermissionNew.NewerDate.HasValue ? (object)groupPermissionNew.NewerDate.Value : DBNull.Value)));
			}

			if (groupPermissionNew.NewerDays != groupPermissionOld.NewerDays)
			{
				updates.Add("`newer_days` = @newer_days");
				parameters.Add(new MySqlParameter("newer_days", groupPermissionNew.NewerDays));
			}

			if (groupPermissionNew.Permission != groupPermissionOld.Permission)
			{
				updates.Add("`permission` = @permission");
				parameters.Add(new MySqlParameter("permission", (int)groupPermissionNew.Permission));
			}

			if (groupPermissionNew.ObjectId != groupPermissionOld.ObjectId)
			{
				updates.Add("`object_id` = @object_id");
				parameters.Add(new MySqlParameter("object_id", (groupPermissionNew.ObjectId.HasValue ? (object)groupPermissionNew.ObjectId.Value : DBNull.Value)));
			}

			if (updates.Count == 0) return false;

			parameters.Add(new MySqlParameter("id", groupPermissionNew.Id));

			Database.ExecuteNonQuery("UPDATE `group_permissions` " +
				"SET " + string.Join(", ", updates) + " " +
				"WHERE `id` = @id",
					parameters.ToArray());

			return true;
		}

		/// <summary>
		/// Deletes a single <see cref="GroupPermission"/> object from the database.
		/// </summary>
		public static void Delete(Int64 id)
			 => Database.ExecuteNonQuery("DELETE FROM `group_permissions` WHERE `id` = " + id);

		/// <summary>
		/// Deletes the specified <see cref="GroupPermission"/> object from the database.
		/// </summary>
		public static void Delete(GroupPermission groupPermission)
			=> Delete(groupPermission.Id);
	}
}
