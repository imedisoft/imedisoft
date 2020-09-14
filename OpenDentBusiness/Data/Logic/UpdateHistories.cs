using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class UpdateHistories
	{
		public static IEnumerable<UpdateHistory> GetAll() 
			=> SelectMany("SELECT * FROM `update_histories` ORDER BY `installed_on`");

		public static UpdateHistory GetLastUpdateHistory() 
			=> SelectOne("SELECT * FROM `update_histories` ORDER BY `installed_on` DESC LIMIT 1");

		public static UpdateHistory GetForVersion(string version) 
			=> SelectOne("SELECT * FROM `update_histories` WHERE `version` = @version",
				new MySqlParameter("version", version ?? ""));

		public static DateTime GetDateForVersion(Version version)
		{
			foreach (var update in GetAll())
			{
				if (Version.TryParse(update.Version, out var result) && result >= version)
                {
					return update.InstalledOn;
                }
			}

			return DateTime.MinValue;
		}

		public static long Insert(UpdateHistory updateHistory) 
			=> ExecuteInsert(updateHistory);
	}
}
