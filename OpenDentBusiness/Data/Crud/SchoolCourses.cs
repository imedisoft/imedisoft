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
	public partial class SchoolCourses
	{
		public static SchoolCourse FromReader(MySqlDataReader dataReader)
		{
			return new SchoolCourse
			{
				Id = (long)dataReader["id"],
				CourseID = (string)dataReader["course_id"],
				Descript = (string)dataReader["descript"]
			};
		}

		/// <summary>
		/// Selects a single SchoolCourse object from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static SchoolCourse SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="SchoolCourse"/> object with the specified key from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="SchoolCourse"/> to select.</param>
		public static SchoolCourse SelectOne(long id)
			=> SelectOne("SELECT * FROM `school_courses` WHERE `id` = " + id);

		/// <summary>
		/// Selects multiple <see cref="SchoolCourse"/> objects from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static IEnumerable<SchoolCourse> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="SchoolCourse"/> into the database.
		/// </summary>
		/// <param name="schoolCourse">The <see cref="SchoolCourse"/> to insert into the database.</param>
		private static long ExecuteInsert(SchoolCourse schoolCourse)
			=> schoolCourse.Id = Database.ExecuteInsert(
				"INSERT INTO `school_courses` " +
				"(`course_id`, `descript`) " +
				"VALUES (" +
					"@course_id, @descript" +
				")",
					new MySqlParameter("course_id", schoolCourse.CourseID ?? ""),
					new MySqlParameter("descript", schoolCourse.Descript ?? ""));

		/// <summary>
		/// Updates the specified <see cref="SchoolCourse"/> in the database.
		/// </summary>
		/// <param name="schoolCourse">The <see cref="SchoolCourse"/> to update.</param>
		private static void ExecuteUpdate(SchoolCourse schoolCourse)
			=> Database.ExecuteNonQuery(
				"UPDATE `school_courses` SET " +
					"`course_id` = @course_id, " +
					"`descript` = @descript " +
				"WHERE `id` = @id",
					new MySqlParameter("id", schoolCourse.Id),
					new MySqlParameter("course_id", schoolCourse.CourseID ?? ""),
					new MySqlParameter("descript", schoolCourse.Descript ?? ""));

		/// <summary>
		/// Deletes a single <see cref="SchoolCourse"/> object from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="SchoolCourse"/> to delete.</param>
		private static void ExecuteDelete(long id)
			 => Database.ExecuteNonQuery("DELETE FROM `school_courses` WHERE `id` = " + id);

		/// <summary>
		/// Deletes the specified <see cref="SchoolCourse"/> object from the database.
		/// </summary>
		/// <param name="schoolCourse">The <see cref="SchoolCourse"/> to delete.</param>
		private static void ExecuteDelete(SchoolCourse schoolCourse)
			=> ExecuteDelete(schoolCourse.Id);
	}
}
