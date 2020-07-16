using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public static class WindowsTime
{
	[DllImport("kernel32.dll", SetLastError = true)]
	private extern static uint SetLocalTime(ref SYSTEMTIME lpSystemTime);

	[StructLayout(LayoutKind.Sequential)]
	private struct SYSTEMTIME
	{
		public ushort wYear;
		public ushort wMonth;
		public ushort wDayOfWeek;
		public ushort wDay;
		public ushort wHour;
		public ushort wMinute;
		public ushort wSecond;
		public ushort wMilliseconds;
	}

	/// <summary>
	/// Set the Windows system time.
	/// </summary>
	public static void SetTime(DateTime newTime)
	{
		SYSTEMTIME systime = new SYSTEMTIME
		{
			wYear = (ushort)newTime.Year,
			wMonth = (ushort)newTime.Month,
			wDayOfWeek = (ushort)newTime.DayOfWeek,
			wDay = (ushort)newTime.Day,
			wHour = (ushort)newTime.Hour,
			wMinute = (ushort)newTime.Minute,
			wSecond = (ushort)newTime.Second,
			wMilliseconds = (ushort)newTime.Millisecond
		};
		SetLocalTime(ref systime);

		EventLog.WriteEntry("OpenDental", "System date and time set to:  " + newTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + ".", EventLogEntryType.Information);
	}
}
