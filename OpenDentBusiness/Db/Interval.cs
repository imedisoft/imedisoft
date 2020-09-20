using System;
using System.Collections.Specialized;

namespace OpenDentBusiness
{
    /// <summary>
    /// Currently used in recall interval. Uses all four values together to establish an interval 
    /// between two dates, letting the user have total control.  Will later be used for such things as
    /// lab cases, appointment scheduling, etc.  Includes a way to combine all four values into one 
    /// number to be stored in the database (as an int32).  Each value has a max of 255, except years 
    /// has a max of 127.
    /// </summary>
    public struct Interval
	{
		public int Years;
		public int Months;
		public int Weeks;
		public int Days;

		public Interval(int combinedValue)
		{
			BitVector32 bitVector = new BitVector32(combinedValue);
			BitVector32.Section sectionDays = BitVector32.CreateSection(255);
			BitVector32.Section sectionWeeks = BitVector32.CreateSection(255, sectionDays);
			BitVector32.Section sectionMonths = BitVector32.CreateSection(255, sectionWeeks);
			BitVector32.Section sectionYears = BitVector32.CreateSection(255, sectionMonths);
			Days = bitVector[sectionDays];
			Weeks = bitVector[sectionWeeks];
			Months = bitVector[sectionMonths];
			Years = bitVector[sectionYears];
		}

		public Interval(int days, int weeks, int months, int years)
		{
			Days = days;
			Weeks = weeks;
			Months = months;
			Years = years;
		}

		public static bool operator ==(Interval a, Interval b) 
			=> a.Years == b.Years && a.Months == b.Months && a.Weeks == b.Weeks && a.Days == b.Days;

		public static bool operator !=(Interval a, Interval b)
			=> a.Years != b.Years || a.Months != b.Months || a.Weeks != b.Weeks || a.Days != b.Days;

		public override bool Equals(object o)
		{
			try
			{
				return this == (Interval)o;
			}
			catch
			{
				return false;
			}
		}

		public override int GetHashCode() => ToInt();

		public static DateTime operator +(DateTime date, Interval interval) 
			=> date.AddYears(interval.Years).AddMonths(interval.Months).AddDays(interval.Days + (interval.Weeks * 7));

		public int ToInt()
		{
			BitVector32 bitVector = new BitVector32(0);
			BitVector32.Section sectionDays = BitVector32.CreateSection(255);
			BitVector32.Section sectionWeeks = BitVector32.CreateSection(255, sectionDays);
			BitVector32.Section sectionMonths = BitVector32.CreateSection(255, sectionWeeks);
			BitVector32.Section sectionYears = BitVector32.CreateSection(255, sectionMonths);
			bitVector[sectionDays] = Days;
			bitVector[sectionWeeks] = Weeks;
			bitVector[sectionMonths] = Months;
			bitVector[sectionYears] = Years;
			return bitVector.Data;
		}

		public override string ToString()
		{
			string result = "";

			if (Years > 0)  result += Years.ToString() + "y";
			if (Months > 0) result += Months.ToString() + "m";
			if (Weeks > 0)  result += Weeks.ToString() + "w";
			if (Days > 0)   result += Days.ToString() + "d";
			
			return result;
		}
	}
}
