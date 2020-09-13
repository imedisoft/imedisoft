using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class Counties
	{
		public static IEnumerable<County> GetAll()
			=> SelectMany(
				"SELECT * FROM `counties` ORDER BY `name`");

		public static IEnumerable<County> Refresh(string name) 
			=> SelectMany(
				"SELECT * from counties WHERE `name` LIKE @name ORDER BY `name`",
					new MySqlParameter("name", (name ?? "") + '%'));

		public static IEnumerable<string> GetListNames() 
			=> Database.SelectMany(
				"SELECT `name` FROM `counties` ORDER BY `name`", Database.ToScalar<string>);

		public static long Insert(County county) 
			=> ExecuteInsert(county);

		public static void Update(County county)
			=> ExecuteUpdate(county);

		public static void Save(County county)
		{
			if (county.Id == 0) Insert(county);
            else
            {
				Update(county);
            }
		}

		public static void Delete(County county)
		{
			string usedBy = UsedBy(county.Name);

			if (usedBy != "")
			{
				throw new Exception("Cannot delete County because it is already in use by the following patients: \r" + usedBy);
			}

			ExecuteDelete(county);
		}

		public static string UsedBy(string name) 
			=> string.Join("\r\n",
				Database.SelectMany(
					"SELECT CONCAT(LName, ', ', FName) FROM patient WHERE County = @name", Database.ToScalar<string>,
						new MySqlParameter("name", name ?? "")));

		/// <summary>
		/// Checks whether a county with the specified <paramref name="name"/> exists in the database.
		/// </summary>
		/// <param name="name">The county name.</param>
		/// <param name="excludeId">The ID of the county to ignore.</param>
		/// <returns>True if the county exists; otherwise, false.</returns>
		public static bool Exists(string name, long excludeId = 0)
		{
			var count = Database.ExecuteLong(
				"SELECT COUNT(`id`) FROM `counties` WHERE `name` = @name AND `id` != " + excludeId,
					new MySqlParameter("name", name));

			return count > 0;
		}
	}
}
