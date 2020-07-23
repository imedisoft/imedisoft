using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness
{
    public enum AutomationAction
	{
		PrintPatientLetter,
		CreateCommlog,

		/// <summary>
		/// If a referral does not exist for this patient, then notify user instead.
		/// </summary>
		PrintReferralLetter,

		ShowExamSheet,
		PopUp,
		SetApptASAP,
		ShowConsentForm,
		SetApptType,

		/// <summary>
		/// Similar to PopUp, but will only show once per WS per 10 minutes.
		/// </summary>
		PopUpThenDisable10Min,

		/// <summary>
		/// When triggered, automatically restricts patient from being scheduled. See also PatRestriction.cs
		/// </summary>
		PatRestrictApptSchedTrue,

		/// <summary>
		/// When triggered, automatically removes patient from scheduling restriction. See also PatRestriction.cs
		/// </summary>
		PatRestrictApptSchedFalse,

		/// <summary>
		/// When triggered, it will automatically print a copy of the Patient Rx Instructions
		/// </summary>
		PrintRxInstruction,

		/// <summary>
		/// When triggered, automatically set a patient's status to the status type in the PatStatus column. Delete should never be used.
		/// </summary>
		[Description("Change Pat Status")]
		ChangePatStatus,
	}
}
