namespace Imedisoft.Data.Models
{
    /// <summary>
    /// Known entities that create security logs.
    /// </summary>
    public enum SecurityLogSource
	{
		/// <summary>
		/// Open Dental and unknown entities.
		/// </summary>
		None,

		/// <summary>
		/// GWT Web Sched application Recall version.
		/// </summary>
		WebSched,

		/// <summary>
		/// X12 834 Insurance Plan Import from the Manage Module.
		/// </summary>
		InsPlanImport834,

		/// <summary>
		/// HL7 is an automated process which the user may not be aware of.
		/// </summary>
		HL7,

		/// <summary>
		/// Database maintenance. This process creates patients which are known to be missing, but 
		/// the user may not be aware that the fix involves patient recreation.
		/// </summary>
		DBM,

		/// <summary>
		/// FHIR is an automated process which the user may not be aware of.
		/// </summary>
		FHIR,

		PatientPortal,

		/// <summary>
		/// GWT Web Sched application New Patient Appointment version
		/// </summary>
		WebSchedNewPatAppt,

		/// <summary>
		/// Automated eConfirmation and eReminders
		/// </summary>
		AutoConfirmations,

		/// <summary>
		/// Messages created for debugging and diagnostic purposes. For example, to diagnose an 
		/// unhandled exception or unexpected behavior that is otherwise too hard to diagnose.
		/// </summary>
		Diagnostic,

		/// <summary>
		/// Mobile Web application.
		/// </summary>
		MobileWeb,

		/// <summary>
		/// When retrieving reports in the background of FormOpenDental
		/// </summary>
		CanadaEobAutoImport,

		/// <summary>
		/// Web Sched application for moving ASAP appointments.
		/// </summary>
		WebSchedASAP,

		OpenDentalService,
		BroadcastMonitor,

		/// <summary>
		/// Automatic log off from main form. Used to track when auto log off needs to kill the 
		/// program to force close open forms which are blocked or slow to respond.
		/// </summary>
		AutoLogOff,

		MobileApp,
		TextMessaging,
		CareCredit,
	}
}
