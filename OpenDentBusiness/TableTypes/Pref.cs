using CodeBase;
using Imedisoft.Data.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace OpenDentBusiness
{
	/// <summary>
	/// Used by pref "AppointmentSearchBehavior".
	/// </summary>
	public enum SearchBehaviorCriteria
	{
		/// <summary>
		/// Based only on provider availability from the schedule.
		/// </summary>
		[Description("Provider Time")]
		ProviderTime,
		
		/// <summary>
		/// Based on provider schedule availability as well as the availabilty of their operatory (dynamic or directly assigned).
		/// </summary>
		[Description("Provider Time Operatory")]
		ProviderTimeOperatory,
	}

	/// <summary>
	/// Used by pref "AccountingSoftware"
	/// </summary>
	public enum AccountingSoftware
	{
		OpenDental,
		QuickBooks
	}

	public enum RigorousAccounting
	{
		/// <summary>
		/// Auto-splitting payments and enforcing paysplit validity is enforced.
		/// </summary>
		[Description("Enforce Fully")]
		EnforceFully,

		/// <summary>
		/// Auto-splitting payments is done, paysplit validity isn't enforced.
		/// </summary>
		[Description("Auto-Split Only")]
		AutoSplitOnly,

		/// <summary>
		/// Neither auto-splitting nor paysplit validity is enforced.
		/// </summary>
		[Description("Don't Enforce")]
		DontEnforce
	}

	public enum RigorousAdjustments
	{
		/// <summary>
		/// Automatically link adjustments and procedures, adjustment linking enforced.
		/// </summary>
		[Description("Enforce Fully")]
		EnforceFully,

		/// <summary>
		/// Adjustment links are made automatically, but it can be edited.
		/// </summary>
		[Description("Link Only")]
		LinkOnly,

		/// <summary>
		/// Adjustment links aren't made, nor are they enforced.
		/// </summary>
		[Description("Don't Enforce")]
		DontEnforce
	}

	///<summary>
	///Used by pref "WebSchedProviderRule". Determines how Web Sched will decide on what provider time slots to show patients.
	///</summary>
	public enum WebSchedProviderRules
	{
		/// <summary>
		/// Dynamically picks the first available provider based on the time slot picked by the patient.
		/// </summary>
		FirstAvailable,

		/// <summary>
		/// Only shows time slots that are available via the patient's primary provider.
		/// </summary>
		PrimaryProvider,

		/// <summary>
		/// Only shows time slots that are available via the patient's secondary provider.
		/// </summary>
		SecondaryProvider,

		/// <summary>
		/// Only shows time slots that are available via the patient's last seen hygienist.
		/// </summary>
		LastSeenHygienist
	}

	public enum PPOWriteoffDateCalc
	{
		/// <summary>
		/// Use the insurance payment date when dating writeoff estimates and adjustments in reports.
		/// </summary>
		[Description("Insurance Pay Date")]
		InsPayDate,
		
		/// <summary>
		/// Use the date of the procedure when dating writeoff estimates and adjustments in reports.
		/// </summary>
		[Description("Procedure Date")]
		ProcDate,

		/// <summary>
		/// Uses initial claim date for writeoff estimates and insurance payment date for writeoff adjustments in reports.
		/// </summary>
		[Description("Initial Claim Date/Ins Pay Date")]
		ClaimPayDate
	}

	/// <summary>
	/// Different options for electronic statements.
	/// Descriptions taken from the FormBillingDefaults.
	/// </summary>
	public enum BillingUseElectronicEnum
	{
		[Description("No electronic billing")]
		None,

		[Description("Dental X Change")]
		EHG,

		[Description("Output to file")]
		POS,

		[Description("ClaimX / ExtraDent")]
		ClaimX,

		[Description("EDS")]
		EDS,
	}
}
