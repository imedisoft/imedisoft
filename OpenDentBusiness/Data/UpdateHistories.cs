using CodeBase;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public partial class UpdateHistories
	{
		/// <summary>
		/// All updatehistory entries ordered by DateTimeUpdated.
		/// </summary>
		public static IEnumerable<UpdateHistory> GetAll() 
			=> SelectMany("SELECT * FROM `update_histories` ORDER BY `installed_on`");

		/// <summary>
		/// Get the most recently inserted updatehistory entry. Ordered by DateTimeUpdated.
		/// </summary>
		public static UpdateHistory GetLastUpdateHistory() 
			=> SelectOne("SELECT * FROM `update_histories` ORDER BY `installed_on` DESC LIMIT 1");

		/// <summary>
		/// Returns the latest version information.
		/// </summary>
		public static UpdateHistory GetForVersion(string version) 
			=> SelectOne("SELECT * FROM `update_histories` WHERE `version` = @version",
				new MySqlParameter("version", version ?? ""));

		/// <summary>
		/// Returns the earliest datetime that a version was reached. If that version has not been reached, returns the MinDate.
		/// </summary>
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
	}
}
