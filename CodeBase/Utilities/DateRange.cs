using System;

namespace CodeBase
{
	public class DateRange
	{
		/// <summary>
		/// The beginning of the date range.
		/// </summary>
		public DateTime Start;

		/// <summary>
		/// The end of the date range.
		/// </summary>
		public DateTime End;

		public DateRange()
		{
		}

		public DateRange(DateTime startDateTime, DateTime dateRangeEnd)
		{
			Start = startDateTime;
			End = dateRangeEnd;
		}

		public bool IsInRange(DateTime dateTime) 
			=> dateTime.Between(Start, End);
	}
}
