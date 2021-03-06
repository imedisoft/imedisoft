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
	public partial class Icd10s
	{
		public static Icd10 FromReader(MySqlDataReader dataReader)
		{
			return new Icd10
			{
				Code = (string)dataReader["code"],
				Description = (string)dataReader["description"],
				IsCode = (Convert.ToInt32(dataReader["is_code"]) == 1)
			};
		}

		/// <summary>
		/// Selects a single Icd10 object from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static Icd10 SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="Icd10"/> object with the specified key from the database.
		/// </summary>
		/// <param name="code">The primary key of the <see cref="Icd10"/> to select.</param>
		public static Icd10 SelectOne(string code)
			=> SelectOne("SELECT * FROM `icd10` WHERE `code` = @code",
				new MySqlParameter("code", code ?? ""));

		/// <summary>
		/// Selects multiple <see cref="Icd10"/> objects from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static IEnumerable<Icd10> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="Icd10"/> into the database.
		/// </summary>
		/// <param name="icd10">The <see cref="Icd10"/> to insert into the database.</param>
		private static void ExecuteInsert(Icd10 icd10)
			=> Database.ExecuteNonQuery(
				"INSERT INTO `icd10` " +
				"(`description`, `is_code`) " +
				"VALUES (" +
					"@description, @is_code" +
				")",
					new MySqlParameter("description", icd10.Description ?? ""),
					new MySqlParameter("is_code", (icd10.IsCode ? 1 : 0)));

		/// <summary>
		/// Updates the specified <see cref="Icd10"/> in the database.
		/// </summary>
		/// <param name="icd10">The <see cref="Icd10"/> to update.</param>
		private static void ExecuteUpdate(Icd10 icd10)
			=> Database.ExecuteNonQuery(
				"UPDATE `icd10` SET " +
					"`description` = @description, " +
					"`is_code` = @is_code " +
				"WHERE `code` = @code",
					new MySqlParameter("code", icd10.Code ?? ""),
					new MySqlParameter("description", icd10.Description ?? ""),
					new MySqlParameter("is_code", (icd10.IsCode ? 1 : 0)));

		/// <summary>
		/// Deletes a single <see cref="Icd10"/> object from the database.
		/// </summary>
		/// <param name="code">The primary key of the <see cref="Icd10"/> to delete.</param>
		private static void ExecuteDelete(string code)
			 => Database.ExecuteNonQuery("DELETE FROM `icd10` WHERE `code` = @code",
					new MySqlParameter("code", code ?? ""));

		/// <summary>
		/// Deletes the specified <see cref="Icd10"/> object from the database.
		/// </summary>
		/// <param name="icd10">The <see cref="Icd10"/> to delete.</param>
		private static void ExecuteDelete(Icd10 icd10)
			=> ExecuteDelete(icd10.Code);
	}
}
