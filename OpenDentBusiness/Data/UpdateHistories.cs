using CodeBase;
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
			=> SelectMany("SELECT * FROM updatehistory ORDER BY DateTimeUpdated");

		/// <summary>
		/// Get the most recently inserted updatehistory entry. Ordered by DateTimeUpdated.
		/// </summary>
		public static UpdateHistory GetLastUpdateHistory() 
			=> SelectOne("SELECT * FROM updatehistory ORDER BY DateTimeUpdated DESC LIMIT 1");

		/// <summary>
		/// Gets the most recently inserted updatehistory entries. Ordered by DateTimeUpdated.
		/// </summary>
		public static IEnumerable<UpdateHistory> GetPreviousUpdateHistories(int count) 
			=> SelectMany("SELECT * FROM updatehistory ORDER BY DateTimeUpdated DESC LIMIT " + count);

		/// <summary>
		/// Returns the latest version information.
		/// </summary>
		public static UpdateHistory GetForVersion(string version) 
			=> SelectOne("SELECT * FROM updatehistory WHERE ProgramVersion='" + POut.String(version.ToString()) + "'");

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
