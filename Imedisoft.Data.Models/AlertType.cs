using System.Collections.Generic;

namespace Imedisoft.Data.Models
{
    public static class AlertType
	{
		/// <summary>
		/// Generic. Informational, has no action associated with it.
		/// </summary>
		public const string Generic = "";

		/// <summary>
		/// Opens the 'Online Payments Window' when clicked.
		/// </summary>
		public const string OnlinePaymentsPending = "online_payments_pending";

		/// <summary>
		/// Opens the 'Radiology Order List' window when clicked.
		/// </summary>
		public const string RadiologyProcedures = "radiology_procedures";

		/// <summary>
		/// A patient has clicked "Request Callback" on an e-Confirmation.
		/// </summary>
		public const string CallbackRequested = "callback_requested";

		/// <summary>
		/// Alerts related to new ASAP appointments via web sched.
		/// </summary>
		public const string WebSchedASAPApptCreated = "web_sched_asap_appt_created";

		/// <summary>
		/// Alerts related to the Web Sched New Pat eService.
		/// </summary>
		public const string WebSchedNewPat = "web_sched_new_pat";

		/// <summary>
		/// Alerts related to Web Sched New Patient Appointments.
		/// </summary>
		public const string WebSchedNewPatApptCreated = "web_sched_new_pat_appt_created";

		/// <summary>
		/// An appointment has been created via Web Sched Recall.
		/// </summary>
		public const string WebSchedRecallApptCreated = "web_sched_recall_appt_created";

		/// <summary>
		/// A number is not able to receive text messages.
		/// </summary>
		public const string NumberBarredFromTexting = "number_barred_from_texting";

		/// <summary>
		/// The number of MySQL connections to the server has exceeded half the allowed number of connections.
		/// </summary>
		public const string MaxConnectionsMonitor = "max_connections_monitor";

		/// <summary>
		/// Multiple computers are running eConnector services. There should only ever be one.
		/// </summary>
		public const string MultipleEConnectors = "multiple_econnectors";

		/// <summary>
		/// The eConnector is in a critical state and not currently turned on. There should only ever be one.
		/// </summary>
		public const string EConnectorDown = "econnector_down";

		/// <summary>
		/// The eConnector has an error that is not critical but is worth looking into. There should only ever be one.
		/// </summary>
		public const string EConnectorError = "econnector_error";

		/// <summary>
		/// Triggered when the consecutive count of failed emails for clinic reaches greater than 
		/// the value set in EmailAlertMaxConsecutiveFails preference.
		/// </summary>
		public const string EconnectorEmailTooManySendFails = "econnector_email_too_many_send_fails";

		/// <summary>
		/// Alerts related to DoseSpot provider registration.
		/// </summary>
		public const string DoseSpotProviderRegistered = "dose_spot_provider_registered";

		/// <summary>
		/// Alerts related to DoseSpot clinic registration.
		/// </summary>
		public const string DoseSpotClinicRegistered = "dose_spot_clinic_registered";

		/// <summary>
		/// Alerts related to turning clinics on or off for eServices.
		/// </summary>
		public const string ClinicsChanged = "clinics_changed";

		/// <summary>
		/// Alerts related to turning clinics on or off for eServices. Internal, not displayed to 
		/// the customer. Will be processed by the eConnector and then deleted.
		/// </summary>
		public const string ClinicsChangedInternal = "clinics_changed_internal";

		/// <summary>
		/// Multiple computers are running OpenDentalServices. There should only ever be one.
		/// </summary>
		public const string MultipleOpenDentalServices = "multiple_services";

		/// <summary>
		/// OpenDentalService is down.
		/// </summary>
		public const string OpenDentalServiceDown = "service_down";

		/// <summary>
		/// Triggered when a new WebMail is recieved from the patient portal.
		/// </summary>
		public const string WebMailRecieved = "webmail_received";

		/// <summary>
		/// Alert the user for things like not making a local supplemental backup within the last month.
		/// </summary>
		public const string SupplementalBackups = "supplemental_backups";

		public static IEnumerable<DataItem<string>> GetValues()
		{
			yield return new DataItem<string>(Generic, "Generic");
			yield return new DataItem<string>(OnlinePaymentsPending, "Online Payments Pending");
			yield return new DataItem<string>(RadiologyProcedures, "Radiology Orders");
			yield return new DataItem<string>(CallbackRequested, "Patient Requests Callback");
			yield return new DataItem<string>(WebSchedNewPat, "Web Sched New Patient");
			yield return new DataItem<string>(WebSchedNewPatApptCreated, "Web Sched New Patient Appointment Created");
			yield return new DataItem<string>(NumberBarredFromTexting, "Number Barred From Texting");
			yield return new DataItem<string>(MaxConnectionsMonitor, "MySQL Maximum Connection Issues");
			yield return new DataItem<string>(WebSchedASAPApptCreated, "Web Sched ASAP Appointment Created");
			yield return new DataItem<string>(MultipleEConnectors, "Multiple eConnectors");
			yield return new DataItem<string>(EConnectorDown, "eConnection Down");
			yield return new DataItem<string>(EConnectorError, "eConnection Error");
			yield return new DataItem<string>(DoseSpotProviderRegistered, "DoseSpot Provider Registered");
			yield return new DataItem<string>(DoseSpotClinicRegistered, "DoseSpot Clinic Registered");
			yield return new DataItem<string>(WebSchedRecallApptCreated, "Web Sched Recall Appointment Created");
			yield return new DataItem<string>(ClinicsChanged, "Clinic Feature Changed");
			yield return new DataItem<string>(ClinicsChangedInternal, "Clinic Feature Changed (internal)");
			yield return new DataItem<string>(MultipleOpenDentalServices, "Multiple OpenDentalServices");
			yield return new DataItem<string>(OpenDentalServiceDown, "OpenDentalService Down");
			yield return new DataItem<string>(WebMailRecieved, "New WebMail");
			yield return new DataItem<string>(EconnectorEmailTooManySendFails, "eConnector Email Send Failures");
			yield return new DataItem<string>(SupplementalBackups, "Supplemental Backups");
		}
	}
}
