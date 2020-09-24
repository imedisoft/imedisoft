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
    public partial class EhrDrugUnits
	{
		public static EhrDrugUnit FromReader(MySqlDataReader dataReader)
		{
			return new EhrDrugUnit
			{
				Code = (string)dataReader["code"],
				Description = (string)dataReader["description"]
			};
		}

		/// <summary>
		/// Selects a single EhrDrugUnit object from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static EhrDrugUnit SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="EhrDrugUnit"/> object with the specified key from the database.
		/// </summary>
		/// <param name="code">The primary key of the <see cref="EhrDrugUnit"/> to select.</param>
		public static EhrDrugUnit SelectOne(string code)
			=> SelectOne("SELECT * FROM `ehr_drug_units` WHERE `code` = @code",
				new MySqlParameter("code", code ?? ""));

		/// <summary>
		/// Selects multiple <see cref="EhrDrugUnit"/> objects from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static IEnumerable<EhrDrugUnit> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="EhrDrugUnit"/> into the database.
		/// </summary>
		/// <param name="ehrDrugUnit">The <see cref="EhrDrugUnit"/> to insert into the database.</param>
		private static void ExecuteInsert(EhrDrugUnit ehrDrugUnit)
			=> Database.ExecuteNonQuery(
				"INSERT INTO `ehr_drug_units` " +
				"(`description`) " +
				"VALUES (" +
					"@description" +
				")",
					new MySqlParameter("description", ehrDrugUnit.Description ?? ""));

		/// <summary>
		/// Updates the specified <see cref="EhrDrugUnit"/> in the database.
		/// </summary>
		/// <param name="ehrDrugUnit">The <see cref="EhrDrugUnit"/> to update.</param>
		private static void ExecuteUpdate(EhrDrugUnit ehrDrugUnit)
			=> Database.ExecuteNonQuery(
				"UPDATE `ehr_drug_units` SET " +
					"`description` = @description " +
				"WHERE `code` = @code",
					new MySqlParameter("code", ehrDrugUnit.Code ?? ""),
					new MySqlParameter("description", ehrDrugUnit.Description ?? ""));

		/// <summary>
		/// Deletes a single <see cref="EhrDrugUnit"/> object from the database.
		/// </summary>
		/// <param name="code">The primary key of the <see cref="EhrDrugUnit"/> to delete.</param>
		private static void ExecuteDelete(string code)
			 => Database.ExecuteNonQuery("DELETE FROM `ehr_drug_units` WHERE `code` = @code",
					new MySqlParameter("code", code ?? ""));

		/// <summary>
		/// Deletes the specified <see cref="EhrDrugUnit"/> object from the database.
		/// </summary>
		/// <param name="ehrDrugUnit">The <see cref="EhrDrugUnit"/> to delete.</param>
		private static void ExecuteDelete(EhrDrugUnit ehrDrugUnit)
			=> ExecuteDelete(ehrDrugUnit.Code);
	}
}