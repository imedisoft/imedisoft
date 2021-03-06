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
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Imedisoft.Data
{
    public partial class Accounts
	{
		public static Account FromReader(MySqlDataReader dataReader)
		{
			return new Account
			{
				Id = (long)dataReader["id"],
				Description = (string)dataReader["description"],
				Type = ((string)dataReader["type"])[0],
				BankNumber = (string)dataReader["bank_number"],
				Inactive = (Convert.ToInt32(dataReader["inactive"]) == 1),
				Color = Color.FromArgb((int)dataReader["color"])
			};
		}

		/// <summary>
		/// Selects a single Account object from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static Account SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="Account"/> object with the specified key from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="Account"/> to select.</param>
		public static Account SelectOne(long id)
			=> SelectOne("SELECT * FROM `accounts` WHERE `id` = " + id);

		/// <summary>
		/// Selects multiple <see cref="Account"/> objects from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static IEnumerable<Account> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="Account"/> into the database.
		/// </summary>
		/// <param name="account">The <see cref="Account"/> to insert into the database.</param>
		private static long ExecuteInsert(Account account)
			=> account.Id = Database.ExecuteInsert(
				"INSERT INTO `accounts` " +
				"(`description`, `type`, `bank_number`, `inactive`, `color`) " +
				"VALUES (" +
					"@description, @type, @bank_number, @inactive, @color" +
				")");

		/// <summary>
		/// Updates the specified <see cref="Account"/> in the database.
		/// </summary>
		/// <param name="account">The <see cref="Account"/> to update.</param>
		private static void ExecuteUpdate(Account account)
			=> Database.ExecuteNonQuery(
				"UPDATE `accounts` SET " +
					"`description` = @description, " +
					"`type` = @type, " +
					"`bank_number` = @bank_number, " +
					"`inactive` = @inactive, " +
					"`color` = @color " +
				"WHERE `id` = @id",
					new MySqlParameter("id", account.Id),
					new MySqlParameter("description", account.Description ?? ""),
					new MySqlParameter("type", account.Type),
					new MySqlParameter("bank_number", account.BankNumber ?? ""),
					new MySqlParameter("inactive", (account.Inactive ? 1 : 0)),
					new MySqlParameter("color", account.Color.ToArgb()));

		/// <summary>
		/// Deletes a single <see cref="Account"/> object from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="Account"/> to delete.</param>
		private static void ExecuteDelete(long id)
			 => Database.ExecuteNonQuery("DELETE FROM `accounts` WHERE `id` = " + id);

		/// <summary>
		/// Deletes the specified <see cref="Account"/> object from the database.
		/// </summary>
		/// <param name="account">The <see cref="Account"/> to delete.</param>
		private static void ExecuteDelete(Account account)
			=> ExecuteDelete(account.Id);
	}
}
