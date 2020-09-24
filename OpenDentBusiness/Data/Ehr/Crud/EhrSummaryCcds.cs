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

namespace Imedisoft.Data
{
    public partial class EhrSummaryCcds
	{
		public static EhrSummaryCcd FromReader(MySqlDataReader dataReader)
		{
			return new EhrSummaryCcd
			{
				Id = (long)dataReader["id"],
				PatientId = (long)dataReader["patient_id"],
				EmailAttachmentId = (long)dataReader["email_attachment_id"],
				Date = dataReader["date"] as DateTime?,
				Content = (string)dataReader["content"]
			};
		}

		/// <summary>
		/// Selects a single EhrSummaryCcd object from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static EhrSummaryCcd SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="EhrSummaryCcd"/> object with the specified key from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="EhrSummaryCcd"/> to select.</param>
		public static EhrSummaryCcd SelectOne(long id)
			=> SelectOne("SELECT * FROM `ehr_summary_ccds` WHERE `id` = " + id);

		/// <summary>
		/// Selects multiple <see cref="EhrSummaryCcd"/> objects from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static IEnumerable<EhrSummaryCcd> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="EhrSummaryCcd"/> into the database.
		/// </summary>
		/// <param name="ehrSummaryCcd">The <see cref="EhrSummaryCcd"/> to insert into the database.</param>
		private static long ExecuteInsert(EhrSummaryCcd ehrSummaryCcd)
			=> ehrSummaryCcd.Id = Database.ExecuteInsert(
				"INSERT INTO `ehr_summary_ccds` " +
				"(`patient_id`, `email_attachment_id`, `date`, `content`) " +
				"VALUES (" +
					"@patient_id, @email_attachment_id, @date, @content" +
				")",
					new MySqlParameter("patient_id", ehrSummaryCcd.PatientId),
					new MySqlParameter("email_attachment_id", ehrSummaryCcd.EmailAttachmentId),
					new MySqlParameter("date", (ehrSummaryCcd.Date.HasValue ? (object)ehrSummaryCcd.Date.Value : DBNull.Value)),
					new MySqlParameter("content", ehrSummaryCcd.Content ?? ""));

		/// <summary>
		/// Updates the specified <see cref="EhrSummaryCcd"/> in the database.
		/// </summary>
		/// <param name="ehrSummaryCcd">The <see cref="EhrSummaryCcd"/> to update.</param>
		private static void ExecuteUpdate(EhrSummaryCcd ehrSummaryCcd)
			=> Database.ExecuteNonQuery(
				"UPDATE `ehr_summary_ccds` SET " +
					"`patient_id` = @patient_id, " +
					"`email_attachment_id` = @email_attachment_id, " +
					"`date` = @date, " +
					"`content` = @content " +
				"WHERE `id` = @id",
					new MySqlParameter("id", ehrSummaryCcd.Id),
					new MySqlParameter("patient_id", ehrSummaryCcd.PatientId),
					new MySqlParameter("email_attachment_id", ehrSummaryCcd.EmailAttachmentId),
					new MySqlParameter("date", (ehrSummaryCcd.Date.HasValue ? (object)ehrSummaryCcd.Date.Value : DBNull.Value)),
					new MySqlParameter("content", ehrSummaryCcd.Content ?? ""));

		/// <summary>
		/// Deletes a single <see cref="EhrSummaryCcd"/> object from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="EhrSummaryCcd"/> to delete.</param>
		private static void ExecuteDelete(long id)
			 => Database.ExecuteNonQuery("DELETE FROM `ehr_summary_ccds` WHERE `id` = " + id);

		/// <summary>
		/// Deletes the specified <see cref="EhrSummaryCcd"/> object from the database.
		/// </summary>
		/// <param name="ehrSummaryCcd">The <see cref="EhrSummaryCcd"/> to delete.</param>
		private static void ExecuteDelete(EhrSummaryCcd ehrSummaryCcd)
			=> ExecuteDelete(ehrSummaryCcd.Id);
	}
}
