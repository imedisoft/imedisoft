using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OpenDentBusiness
{
    [Table("alert_items")]
	public class AlertItem
	{
		[PrimaryKey]
		public long Id;

		// [ForeignKey(typeof(Clinic), nameof(Clinic.Id))]
		public long? ClinicId;

		[ForeignKey(typeof(User), nameof(User.Id))]
		public long? UserId;

		/// <summary>
		/// A description of the alert item. Displayed in the alerts menu.
		/// </summary>
		public string Description;

		/// <summary>
		/// Like description, but more specific. When set use ActionType.ShowItemValue to show this variable within a MsgBoxCopyPaste window.
		/// </summary>
		public string Details;

		/// <summary>
		/// Enum:AlertType Identifies what type of alert this row is.
		/// </summary>
		public string Type;

		/// <summary>
		/// Enum:SeverityType The severity will help determine what color this alert should be in the main menu.
		/// </summary>
		public AlertSeverityType Severity;

		/// <summary>
		/// Enum:ActionType Bitwise flag that represents what actions are available for this alert.
		/// </summary>
		public AlertAction Actions;

		/// <summary>
		/// The form to open when the user clicks "Open Form".
		/// </summary>
		public FormType FormToOpen;

		/// <summary>
		/// A FK to a table associated with the AlertType. 
		/// 0 indicates not in use.
		/// </summary>
		public long? ObjectId;






		private static Dictionary<AlertAction, int> _dictActionTypeOrder = new Dictionary<AlertAction, int>
		{
			{ AlertAction.OpenForm, 1 },
			{ AlertAction.ShowItemValue, 2 },
			{ AlertAction.MarkAsRead, 3 },
			{ AlertAction.Delete, 4 },
			{ AlertAction.None, 5 },
		};

		public AlertItem()
		{
		}

		public AlertItem Copy()
		{
			return (AlertItem)MemberwiseClone();
		}

		public override bool Equals(object obj)
		{
			AlertItem alert = obj as AlertItem;
			if (alert == null)
			{
				return false;
			}
			return Id == alert.Id
				&& ClinicId == alert.ClinicId
				&& Description == alert.Description
				&& Type == alert.Type
				&& Severity == alert.Severity
				&& Actions == alert.Actions;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static int CompareActionType(AlertAction x, AlertAction y)
		{
			return _dictActionTypeOrder[x].CompareTo(_dictActionTypeOrder[y]);
		}
	}

	

	/// <summary>
	/// Identifies the urgency of a alert.
	/// </summary>
	public enum AlertSeverityType
	{
		Normal,
		Low,
		Medium,
		High
	}

	/// <summary>
	/// Identifies the action(s) that can be taken on a alert.
	/// </summary>
	[Flags]
	public enum AlertAction
	{
		None = 0,
		MarkAsRead = 1,
		OpenForm = 2,
		Delete = 4,
		ShowItemValue = 8
	}

	///<summary>Add this.</summary>
	public enum FormType
	{
		///<summary>0 - No form.</summary>
		None,
		///<summary>1 - FormEServicesWebSchedRecall.</summary>
		[Description("eServices Web Sched Recall")]
		FormEServicesWebSchedRecall,
		///<summary>2 - FormPendingPayments.</summary>
		[Description("Pending Payments")]
		FormPendingPayments,
		///<summary>3 - FormRadOrderList.</summary>
		[Description("Radiology Orders")]
		FormRadOrderList,
		///<summary>4 - FormEServicesSetup.</summary>
		[Description("eServices Signup Portal")]
		FormEServicesSignupPortal,
		///<summary>5 - FormEServicesSetup. FKey will be the AptNum of the appointment to open.</summary>
		[Description("Appointment")]
		FormApptEdit,
		///<summary>6 - FormEServicesSetup Web Sched New Pat.</summary>
		[Description("eServices Web Sched New Pat")]
		FormEServicesWebSchedNewPat,
		///<summary>7 - FormWebSchedAppts.</summary>
		[Description("Web Sched Appointments")]
		FormWebSchedAppts,
		///<summary>8 - FormPatientEdit. FKey will be PatNum.</summary>
		[Description("Edit Patient Information")]
		FormPatientEdit,
		///<summary>9 - FormEServicesSetup eConnector Service.</summary>
		[Description("eServices eConnector Service")]
		FormEServicesEConnector,
		///<summary>10 - FormDoseSpotAssignUserId.</summary>
		[Description("DoseSpot Assign User ID")]
		FormDoseSpotAssignUserId,
		///<summary>11 - FormDoseSpotAssignClinicId.</summary>
		[Description("DoseSpot Assign Clinic ID")]
		FormDoseSpotAssignClinicId,
		///<summary>12 - FormWebMailMessageEdit</summary>
		[Description("WebMail Inbox")]
		FormEmailInbox,
		///<summary>13 - FormEmailAddresses</summary>
		[Description("Email Addresses Setup")]
		FormEmailAddresses,
	}
}
