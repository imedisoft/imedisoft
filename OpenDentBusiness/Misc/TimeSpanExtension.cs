using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness
{
    public static class TimeSpanExtension
	{
		/// <summary>
		/// -H:mm.  If zero, then returns empty string.
		/// Hours can be greater than 24.
		/// </summary>
		public static string ToStringHmm(this TimeSpan timeSpan)
		{
			if (timeSpan == TimeSpan.Zero) return "";

			string result = "";
			if (timeSpan < TimeSpan.Zero)
			{
				result += "-";
				timeSpan = timeSpan.Duration();
			}

			return result + ((timeSpan.Days * 24) + timeSpan.Hours).ToString() + ":" + timeSpan.Minutes.ToString().PadLeft(2, '0');
		}

		/// <summary>
		/// -H:mm:ss.
		/// If zero, then returns empty string.
		/// </summary>
		public static string ToStringHmmss(this TimeSpan timeSpan)
		{
			if (timeSpan == TimeSpan.Zero) return "";

			string result = "";
			if (timeSpan < TimeSpan.Zero)
			{
				result += "-";
				timeSpan = timeSpan.Duration();
			}

			return result + ((timeSpan.Days * 24) + timeSpan.Hours).ToString().PadLeft(2, '0') + ":" + timeSpan.Minutes.ToString().PadLeft(2, '0');
		}

		public static string ToString(this TimeSpan timeSpan, string format) 
			=> (DateTime.Today + timeSpan).ToString(format);

		public static string ToShortTimeString(this TimeSpan timeSpan) 
			=> (DateTime.Today + timeSpan).ToShortTimeString();
	}
}
