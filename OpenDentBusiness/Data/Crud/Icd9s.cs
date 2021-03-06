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
using System.Collections;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class Icd9s
	{
		public static Icd9 FromReader(MySqlDataReader dataReader)
		{
			return new Icd9
			{
				Code = (string)dataReader["code"],
				Description = (string)dataReader["description"],
				LastModifiedDate = (DateTime)dataReader["last_modified_date"]
			};
		}

		/// <summary>
		/// Selects a single Icd9 object from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static Icd9 SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="Icd9"/> object with the specified key from the database.
		/// </summary>
		/// <param name="code">The primary key of the <see cref="Icd9"/> to select.</param>
		public static Icd9 SelectOne(string code)
			=> SelectOne("SELECT * FROM `icd9` WHERE `code` = @code",
				new MySqlParameter("code", code ?? ""));

		/// <summary>
		/// Selects multiple <see cref="Icd9"/> objects from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static IEnumerable<Icd9> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="Icd9"/> into the database.
		/// </summary>
		/// <param name="icd9">The <see cref="Icd9"/> to insert into the database.</param>
		private static void ExecuteInsert(Icd9 icd9)
			=> Database.ExecuteNonQuery(
				"INSERT INTO `icd9` " +
				"(`description`) " +
				"VALUES (" +
					"@description" +
				")",
					new MySqlParameter("description", icd9.Description ?? ""),
					new MySqlParameter("last_modified_date", icd9.LastModifiedDate));

		/// <summary>
		/// Updates the specified <see cref="Icd9"/> in the database.
		/// </summary>
		/// <param name="icd9">The <see cref="Icd9"/> to update.</param>
		private static void ExecuteUpdate(Icd9 icd9)
			=> Database.ExecuteNonQuery(
				"UPDATE `icd9` SET " +
					"`description` = @description " +
				"WHERE `code` = @code",
					new MySqlParameter("code", icd9.Code ?? ""),
					new MySqlParameter("description", icd9.Description ?? ""),
					new MySqlParameter("last_modified_date", icd9.LastModifiedDate));

		/// <summary>
		/// Deletes a single <see cref="Icd9"/> object from the database.
		/// </summary>
		/// <param name="code">The primary key of the <see cref="Icd9"/> to delete.</param>
		private static void ExecuteDelete(string code)
			 => Database.ExecuteNonQuery("DELETE FROM `icd9` WHERE `code` = @code",
					new MySqlParameter("code", code ?? ""));

		/// <summary>
		/// Deletes the specified <see cref="Icd9"/> object from the database.
		/// </summary>
		/// <param name="icd9">The <see cref="Icd9"/> to delete.</param>
		private static void ExecuteDelete(Icd9 icd9)
			=> ExecuteDelete(icd9.Code);
	}
}
