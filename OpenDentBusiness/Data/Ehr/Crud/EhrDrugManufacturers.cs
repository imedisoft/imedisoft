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
    public partial class EhrDrugManufacturers
	{
		public static EhrDrugManufacturer FromReader(MySqlDataReader dataReader)
		{
			return new EhrDrugManufacturer
			{
				Id = (long)dataReader["id"],
				Name = (string)dataReader["name"],
				Code = (string)dataReader["abbr"]
			};
		}

		/// <summary>
		/// Selects a single EhrDrugManufacturer object from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static EhrDrugManufacturer SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="EhrDrugManufacturer"/> object with the specified key from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="EhrDrugManufacturer"/> to select.</param>
		public static EhrDrugManufacturer SelectOne(long id)
			=> SelectOne("SELECT * FROM `ehr_drug_manufacturers` WHERE `id` = " + id);

		/// <summary>
		/// Selects multiple <see cref="EhrDrugManufacturer"/> objects from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static IEnumerable<EhrDrugManufacturer> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="EhrDrugManufacturer"/> into the database.
		/// </summary>
		/// <param name="ehrDrugManufacturer">The <see cref="EhrDrugManufacturer"/> to insert into the database.</param>
		private static long ExecuteInsert(EhrDrugManufacturer ehrDrugManufacturer)
			=> ehrDrugManufacturer.Id = Database.ExecuteInsert(
				"INSERT INTO `ehr_drug_manufacturers` " +
				"(`name`, `abbr`) " +
				"VALUES (" +
					"@name, @abbr" +
				")",
					new MySqlParameter("name", ehrDrugManufacturer.Name ?? ""),
					new MySqlParameter("abbr", ehrDrugManufacturer.Code ?? ""));

		/// <summary>
		/// Updates the specified <see cref="EhrDrugManufacturer"/> in the database.
		/// </summary>
		/// <param name="ehrDrugManufacturer">The <see cref="EhrDrugManufacturer"/> to update.</param>
		private static void ExecuteUpdate(EhrDrugManufacturer ehrDrugManufacturer)
			=> Database.ExecuteNonQuery(
				"UPDATE `ehr_drug_manufacturers` SET " +
					"`name` = @name, " +
					"`abbr` = @abbr " +
				"WHERE `id` = @id",
					new MySqlParameter("id", ehrDrugManufacturer.Id),
					new MySqlParameter("name", ehrDrugManufacturer.Name ?? ""),
					new MySqlParameter("abbr", ehrDrugManufacturer.Code ?? ""));

		/// <summary>
		/// Deletes a single <see cref="EhrDrugManufacturer"/> object from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="EhrDrugManufacturer"/> to delete.</param>
		private static void ExecuteDelete(long id)
			 => Database.ExecuteNonQuery("DELETE FROM `ehr_drug_manufacturers` WHERE `id` = " + id);

		/// <summary>
		/// Deletes the specified <see cref="EhrDrugManufacturer"/> object from the database.
		/// </summary>
		/// <param name="ehrDrugManufacturer">The <see cref="EhrDrugManufacturer"/> to delete.</param>
		private static void ExecuteDelete(EhrDrugManufacturer ehrDrugManufacturer)
			=> ExecuteDelete(ehrDrugManufacturer.Id);
	}
}
