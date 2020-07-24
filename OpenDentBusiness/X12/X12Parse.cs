using System;

namespace OpenDentBusiness
{
    public class X12Parse
	{
		/// <summary>
		/// Converts a YYYYMMDD format date string into a <see cref="DateTime"/> object.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static DateTime ToDate(string value)
		{
			if (value.Length < 8) return DateTime.MinValue;

			if (int.TryParse(value.Substring(0, 4), out var year) && year >= 1880 &&
				int.TryParse(value.Substring(4, 2), out var month) &&
				int.TryParse(value.Substring(6, 2), out var day))
			{
				return new DateTime(year, month, day);
			}

			return DateTime.MinValue;
		}

		public static string UrlDecode(string str) => str
			.Replace("%3A", ":")
			.Replace("%26", "&")
			.Replace("%2F", "/")
			.Replace("%3D", "=")
			.Replace("%3F", "?");
	}
}
