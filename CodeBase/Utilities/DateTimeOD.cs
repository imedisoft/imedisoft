using System;

namespace CodeBase
{
    public class DateTimeOD
	{
		/// <summary>
		/// We are switching to using this method instead of DateTime.Today.
		/// You can track actual Year/Month/Date differences by creating an instance of this class and passing in the two dates to compare.
		/// The values will be stored in YearsDiff, MonthsDiff, and DaysDiff.
		/// </summary> 
		public static DateTime Today 
			=> new DateTime(
				DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 
				0, 0, 0, DateTimeKind.Unspecified);

		/// <summary>
		/// Returns the most recent valid date possible based on the year and month passed in.
		/// E.g. y:2017,m:4,d:31 is passed in (an invalid date) which will return a date of "04/30/2017" which is the most recent 'valid' date.
		/// Throws an exception if the year is not between 1 and 9999, and if the month is not between 1 and 12.
		/// </summary>
		public static DateTime GetMostRecentValidDate(int year, int month, int day)
		{
			int maxDay = DateTime.DaysInMonth(year, month);

			return new DateTime(year, month, Math.Min(day, maxDay));
		}

		/// <summary>
		/// Finds the greatest date which is considered to be the given number of months in the past.  Does not consider the time, just the date.
		/// Ex: March 31st minus 1 month is Feb. 28th or 29th, but the result we want is March 1st.
		/// </summary>
		public static DateTime CalculateForEndOfMonthOffset(DateTime date, int numMonthsInPast)
		{
			DateTime dateCalc = date.AddMonths(0 - numMonthsInPast);
			while (dateCalc.AddMonths(numMonthsInPast) < date)
			{
				dateCalc = dateCalc.AddDays(1);
			}
			return dateCalc;
		}
	}
}
