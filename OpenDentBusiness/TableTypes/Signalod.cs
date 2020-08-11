using Imedisoft.Data.Annotations;
using System;
using System.Collections;
using System.Security.RightsManagement;

namespace OpenDentBusiness
{
	/// <summary>
	/// Names of frequently used signals.
	/// </summary>
	public static class SignalName
    {
		public const string Invalidate = "invalidate";
		public const string InvalidateDate = "invalidate_date";
		public const string Shutdown = "shutdown";
		public const string Schedules = "schedules";
		public const string Appointment = "appointment";
		public const string Message = "message";


		public const string RefreshPatient = "refresh_patient";

		public const string UnfinalizedPayMenuUpdate = "unfinalized_pay_menu_update";

	}

	[Table("signals")]
	public class Signalod
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The name of the signal.
		/// </summary>
		public string Name;

		/// <summary>
		/// The date and time on which the signal was created.
		/// </summary>
		public DateTime Date;



		public string Param1;
		
		public long? Param2;

		public DateTime? Param3;
	}

	///<summary>Do not combine with SignalType, they must be seperate. Stored as string, safe to reorder enum values.</summary>
	public enum KeyType
	{
		Undefined = 0,
		FeeSched,
		Job,
		Operatory,
		Provider,
		SigMessage,
		
		/// <summary>
		/// Special KeyType that does not use a FK but instead will set FKey to a count of unread messages.
		/// Used along side the SmsTextMsgReceivedUnreadCount InvalidType.
		/// </summary>
		SmsMsgUnreadCount,

		Task,

		/// <summary>
		/// Used to identify which signals a form can ignore.
		/// If the FKey==Process.GetCurrentProcess().Id then this process sent it so ignore it.
		/// Used in FormTerminal, FormTerminalManager, and FormSheetFillEdit (for forms being filled at a kiosk).
		/// </summary>
		ProcessId,

		/// <summary>
		/// Used to notify the phone tracking server to kick all users out of a conference room.
		/// </summary>
		ConfKick,

		PatNum,
	}
}
