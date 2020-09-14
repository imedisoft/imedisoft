using System;

namespace Imedisoft.Data
{
    /// <summary>
    /// Because this enum is stored in the database as strings rather than as numbers, we can do the order alphabetically.  
    /// Deprecated preferences will start with "Deprecated" in the summary section.
    /// Preferences that are missing in general will start with "Missing in general" in the summary section.
    /// </summary>
    public static class PreferenceName
	{
		public const string AccountingCashIncomeAccount = "AccountingCashIncomeAccount";

		/// <summary>
		/// The default cash payment type used to determine the CashSumTotal for deposit slips.
		/// </summary>
		public const string AccountingCashPaymentType = "AccountingCashPaymentType";
		public const string AccountingDepositAccounts = "AccountingDepositAccounts";
		public const string AccountingIncomeAccount = "AccountingIncomeAccount";
		public const string AccountingLockDate = "AccountingLockDate";

		/// <summary>
		/// Enum:AccountingSoftware 0=None, 1=Open Dental, 2=QuickBooks
		/// </summary>
		public const string AccountingSoftware = "AccountingSoftware";

		/// <summary>
		/// Boolean, false by default to preserve old functionality.
		/// Allows users to make future dated patient payments when turned on.
		/// </summary>
		public const string AccountAllowFutureDebits = "AccountAllowFutureDebits";

		/// <summary>
		/// Defaulted to off, determines whether completed payment plans are visible in the account module.
		/// </summary>
		public const string AccountShowCompletedPaymentPlans = "AccountShowCompletedPaymentPlans";
		public const string AccountShowPaymentNums = "AccountShowPaymentNums";

		/// <summary>
		/// Show questionnaire button in account module toolbar. 
		/// Feature is obsolete and can only be accessed by users that had this preference turned on prior to its removal from the Show Features window.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")] 
		public const string AccountShowQuestionnaire = "AccountShowQuestionnaire";

		[Obsolete("This preference has been deprecated. Do not use.", true)]
		public const string AccountShowTrojanExpressCollect = "AccountShowTrojanExpressCollect";

		public const string ADAComplianceDateTime = "ADAComplianceDateTime";
		public const string ADAdescriptionsReset = "ADAdescriptionsReset";

		/// <summary>
		/// Boolean, true by default.
		/// When set to true and a new family member is added, the new patient's email will be autofilled with the guarantor's email.
		/// </summary>
		public const string AddFamilyInheritsEmail = "AddFamilyInheritsEmail";

		/// <summary>
		/// Deprecated in version 17.4.40.
		/// When set to true, the user will not be able to save a new adjustment without first attaching a procedure to it.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")] 
		public const string AdjustmentsForceAttachToProc = "AdjustmentsForceAttachToProc";

		/// <summary>
		/// Enum:ADPCompanyCode Used to generate the export file from FormTimeCardManage. Set in FormTimeCardSetup.
		/// </summary>
		public const string ADPCompanyCode = "ADPCompanyCode";

		///<summary>Stored as DateTime, but cleared when aging finishes.  The DateTime will be used as a flag to signal other connections that aging
		///calculations have started and prevents another connection from running simultaneously.  In order to run aging, this will have to be cleared,
		///either by the connection that set the flag when aging finishes, or by the user overriding the lock and manually clearing this pref.</summary>
		public const string AgingBeginDateTime = "AgingBeginDateTime";
		public const string AgingCalculatedMonthlyInsteadOfDaily = "AgingCalculatedMonthlyInsteadOfDaily";

		/// <summary>
		/// If true, aging will use the intermediate table famaging for calculating aging.
		/// </summary>
		public const string AgingIsEnterprise = "AgingIsEnterprise";

		[Obsolete("This preference has been deprecated. Do not use. Use AgingProcLifo instead.", true)]
		public const string AgingNegativeAdjsByAdjDate = "AgingNegativeAdjsByAdjDate";

		/// <summary>
		/// YN_DEFAULT_FALSE, 0-unknown,1-yes,2-no.
		/// For job 14902 - "Aging of AR change:  LIFO negative adjustments to (within) attached procedure and positive adjustments."
		/// </summary>
		public const string AgingProcLifo = "AgingProcLifo";
		
		/// <summary>
		/// This pref is hidden, so no UI to enable this feature.  
		/// If this is true, there will be a checkbox in the aging report window to age patient payments to payment plans.
		/// Aging patient payments to payment plans will only work if the completed amounts on the payment plans are 0.
		/// Otherwise the payments and the completed amounts will essentially double the amounts of the payment plans in the aging calculation.
		/// This is only for a specific customer, so no UI, defaults to false, only able to enable this via query.
		/// </summary>
		public const string AgingReportShowAgePatPayplanPayments = "AgingReportShowAgePatPayplanPayments";
		
		/// <summary>
		/// Stored as DateTime, but only the time is used. 
		/// This is the time of day during which aging will be calculated by the aging service.
		/// Aging will run during a one hour block of time starting with the time set. 
		/// If AgingBeginDateTime is not blank, aging will not be calculated.
		/// If AgingCalculatedMonthlyInsteadOfDaily is true, aging will not be calculated. 
		/// This will be blank if disabled.
		/// </summary>
		public const string AgingServiceTimeDue = "AgingServiceTimeDue";
		
		/// <summary>
		/// How often to check for new alerts in Open Dental. Defaults to 180 (3 minutes).
		/// </summary>
		public const string AlertCheckFrequencySeconds = "AlertCheckFrequencySeconds";
		
		/// <summary>
		/// After this many minutes of inactivity, alerts will stop processing. Defaults to the same value as SignalInactiveMinutes.
		/// </summary>
		public const string AlertInactiveMinutes = "AlertInactiveMinutes";
		
		/// <summary>
		/// FK to allergydef.AllergyDefNum
		/// </summary>
		public const string AllergiesIndicateNone = "AllergiesIndicateNone";
		
		/// <summary>
		/// Boolean defaults to true.  If true, allows a user to email CC receipt otherwise not allowed.
		/// </summary>
		public const string AllowEmailCCReceipt = "AllowEmailCCReceipt";
		public const string AllowedFeeSchedsAutomate = "AllowedFeeSchedsAutomate";
		
		/// <summary>
		/// Boolean defauts to false.  If true, users can enter insurance payments that are for a future date.
		/// </summary>
		public const string AllowFutureInsPayments = "AllowFutureInsPayments";
		
		/// <summary>
		/// Boolean, false by default to preserve old functionality. 
		/// Allows users to attach providers to prepayments while EnforceFully is on.
		/// </summary>
		public const string AllowPrepayProvider = "AllowPrepayProvider";

		/// <summary>
		/// Bool. Allows adjustments from FormClaimEdit. 0 by default.
		/// </summary>
		public const string AllowProcAdjFromClaim = "AllowProcAdjFromClaim";
		public const string AllowSettingProcsComplete = "AllowSettingProcsComplete";

		/// <summary>
		/// DefNum for the payment type to be used for payments created through our Extended (FHIR) API.
		/// </summary>
		public const string ApiPaymentType = "ApiPaymentType";
		public const string AppointmentBubblesDisabled = "AppointmentBubblesDisabled";
		public const string AppointmentBubblesNoteLength = "AppointmentBubblesNoteLength";


		///<summary>Reset calendar to today on clinic select.</summary>
		public const string AppointmentClinicTimeReset = "AppointmentClinicTimeReset";
		///<summary>Enum:SearchBehaviorCriteria 0=ProviderTime, 1=ProviderTimeOperatory</summary>
		public const string AppointmentSearchBehavior = "AppointmentSearchBehavior";
		public const string AppointmentTimeArrivedTrigger = "AppointmentTimeArrivedTrigger";
		public const string AppointmentTimeDismissedTrigger = "AppointmentTimeDismissedTrigger";
		///<summary>The number of minutes that the appointment schedule is broken up into.  E.g. "10" represents 10 minute increments.</summary>
		public const string AppointmentTimeIncrement = "AppointmentTimeIncrement";
		///<summary>Set to true if appointment times are locked by default.</summary>
		public const string AppointmentTimeIsLocked = "AppointmentTimeIsLocked";
		///<summary>Used to set the color of the time indicator line in the appt module.  Stored as an int.</summary>
		public const string AppointmentTimeLineColor = "AppointmentTimeLineColor";
		public const string AppointmentTimeSeatedTrigger = "AppointmentTimeSeatedTrigger";
		///<summary>Controls whether or not creating new appointments prompt to select an appointment type.</summary>
		public const string AppointmentTypeShowPrompt = "AppointmentTypeShowPrompt";
		///<summary>Controls whether or not a warning will be displayed when selecting an appointment type would detach procedures from an appointment..</summary>
		public const string AppointmentTypeShowWarning = "AppointmentTypeShowWarning";
		///<summary>Integer in minutes.  Defaults to 30.  The defualt length of an appointment that is created without attaching any procedures.</summary>
		public const string AppointmentWithoutProcsDefaultLength = "AppointmentWithoutProcsDefaultLength";
		///<summary>Boolean defauts to true.  If true, users can set appointments without procedures complete.</summary>
		public const string ApptAllowEmptyComplete = "ApptAllowEmptyComplete";
		///<summary>Boolean defauts to false.  If true, users can set future appointments complete.</summary>
		public const string ApptAllowFutureComplete = "ApptAllowFutureComplete";
		///<summary>Int. Defaults to 4.  Number of days out to automatically refresh appointment module. -1 will get all appointments.</summary>
		public const string ApptAutoRefreshRange = "ApptAutoRefreshRange";
		public const string ApptBubbleDelay = "ApptBubbleDelay";
		///<summary>True if the office has actived Arrivals in the UI.</summary>
		public const string ApptArrivalAutoEnabled = "ApptArrivalAutoEnabled";
		///<summary>True if the office has actived eConfirmations in the UI.</summary>
		public const string ApptConfirmAutoEnabled = "ApptConfirmAutoEnabled";
		///<summary>True if HQ has confirmed that this office is signed up for eConfirmations.</summary>
		public const string ApptConfirmAutoSignedUp = "ApptConfirmAutoSignedUp";
		///<summary>Bool; Only if using clinics, when true causes automation to skip appointments not assigned to a clinic.</summary>
		public const string ApptConfirmEnableForClinicZero = "ApptConfirmEnableForClinicZero";
		///<summary>Comma delimited list of FK to definition.DefNum. Every appointment with a confirmed status that is in this list will be excluded from 
		///sending Arrival Response SMS and being marked Arrived.</summary>
		public const string ApptConfirmExcludeArrivalResponse = "ApptConfirmExcludeArrivalResponse";
		///<summary>Comma delimited list of FK to definition.DefNum. Every appointment with a confirmed status that is in this list will be excluded from 
		///sending Arrival SMS.</summary>
		public const string ApptConfirmExcludeArrivalSend = "ApptConfirmExcludeArrivalSend";
		///<summary>Comma delimited list of FK to definition.DefNum. Every appointment with a confirmed status that is in this list will be excluded from EConfirmation RSVP updates.
		///Prevents overwriting manual Confirmation status.</summary>
		public const string ApptConfirmExcludeEConfirm = "ApptConfirmExcludeEConfirm";
		///<summary>Comma delimited list of FK to definition.DefNum. Every appointment with a confirmed status that is in this list will be excluded from EReminders.</summary>
		public const string ApptConfirmExcludeERemind = "ApptConfirmExcludeERemind";
		///<summary>Comma delimited list of FK to definition.DefNum. Every appointment with a confirmed status that is in this list will be excluded from sending an EConfirmation.
		///Prevents overwriting manual Confirmation status.</summary>
		public const string ApptConfirmExcludeESend = "ApptConfirmExcludeESend";
		///<summary>Comma delimited list of FK to definition.DefNum. Every appointment with a confirmed status that is in this list will be excluded from EThankYous.</summary>
		public const string ApptConfirmExcludeEThankYou = "ApptConfirmExcludeEThankYou";
		///<summary>Boolean. True by default. If true, clicking the confirm URL will bring up the Confirmation Portal where the patient can select a
		///confirmation option. If false, clicking the confirm URL in the message will confirm the appt (1-click confirmation).</summary>
		public const string ApptEConfirm2ClickConfirmation = "ApptEConfirm2ClickConfirmation";
		///<summary>FK to definition.DefNum.  If using automated confirmations, appointment set to this status when confirmation is sent.</summary>
		public const string ApptEConfirmStatusSent = "ApptEConfirmStatusSent";
		///<summary>FK to definition.DefNum.  If using automated confirmations, appointment set to this status when confirmation is confirmed.</summary>
		public const string ApptEConfirmStatusAccepted = "ApptEConfirmStatusAccepted";
		///<summary>FK to definition.DefNum.  If using automated confirmations, Anything that is not "Accepted" or "Sent".</summary>
		public const string ApptEConfirmStatusDeclined = "ApptEConfirmStatusDeclined";
		///<summary>FK to definition.DefNum.  If using automated confirmations, when failed by HQ for some reason.</summary>
		public const string ApptEConfirmStatusSendFailed = "ApptEConfirmStatusSendFailed";
		public const string ApptExclamationShowForUnsentIns = "ApptExclamationShowForUnsentIns";
		///<summary>Float. Default 8. Valid between 1 and 40.</summary>
		public const string ApptFontSize = "ApptFontSize";
		///<summary>Boolean defaults to 0.  If true, adds the adjustment total to the net production in appointment module.</summary>
		public const string ApptModuleAdjustmentsInProd = "ApptModuleAdjustmentsInProd";
		///<summary>Boolean defaults to 0, when true appt module will default to week view</summary>
		public const string ApptModuleDefaultToWeek = "ApptModuleDefaultToWeek";
		///<summary>Bool; 0 by default. When false, calculates net and gross production by provider bars in each appointment view.
		///When true, calulates net and gross production by appointments in the apppointment view. </summary>
		public const string ApptModuleProductionUsesOps = "ApptModuleProductionUsesOps";
		///<summary>Boolean defaults to 1 if there is relevant ortho chart info, when true appt menu will have an ortho chart item.</summary>
		public const string ApptModuleShowOrthoChartItem = "ApptModuleShowOrthoChartItem";
		///<summary>Keeps the waiting room indicator times current.  Initially 1.</summary>
		public const string ApptModuleRefreshesEveryMinute = "ApptModuleRefreshesEveryMinute";


		[Obsolete("This preference has been deprecated. Do not use.", true)]
		public const string ApptModuleUses2019Overhaul = "ApptModuleUses2019Overhaul";

		/// <summary>
		/// Boolean.
		/// False by default.
		/// If true, prevents from making changes (breaking, deleting, sending to unscheduled, changing status) to completed appointments.
		/// The completed appointment MUST have completed procedures attached. 
		/// If false, does nothing.
		/// </summary>
		public const string ApptPreventChangesToCompleted = "ApptPreventChangesToCompleted";

		public const string ApptPrintColumnsPerPage = "ApptPrintColumnsPerPage";
		public const string ApptPrintFontSize = "ApptPrintFontSize";

		/// <summary>
		/// Stored as DateTime. 
		/// Currently the date portion is not used but might be used in future versions.
		/// </summary>
		public const string ApptPrintTimeStart = "ApptPrintTimeStart";

		/// <summary>
		/// Stored as DateTime. 
		/// Currently the date portion is not used but might be used in future versions.
		/// </summary>
		public const string ApptPrintTimeStop = "ApptPrintTimeStop";

		/// <summary>
		/// Int. 
		/// Width of prov bar on individual appts, not the bars on left of screen. 
		/// Was historically 8. 
		/// In version 19.3, default was changed to 11 to allow text in provbar. Valid between 0 and 20.
		/// </summary>
		public const string ApptProvbarWidth = "ApptProvbarWidth";

		/// <summary>
		/// True if automated appointment reminders are enabled for the entire DB.
		/// See ApptReminderRules for setup details.
		/// Permissions are still checked here at HQ so manually overriding this value will only make the program behave annoyingly, but won't break anything.
		/// </summary>
		public const string ApptRemindAutoEnabled = "ApptRemindAutoEnabled";

		/// <summary>
		/// DEPRECATED.  See ApptReminderRule table instead.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string ApptReminderDayInterval = "ApptReminderDayInterval";

		/// <summary>
		/// DEPRECATED.  See ApptReminderRule table instead.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string ApptReminderDayMessage = "ApptReminderDayMessage";

		/// <summary>
		/// DEPRECATED.  See ApptReminderRule table instead.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string ApptReminderEmailMessage = "ApptReminderEmailMessage";

		/// <summary>
		/// DEPRECATED.  See ApptReminderRule table instead.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string ApptReminderHourInterval = "ApptReminderHourInterval";

		/// <summary>
		/// DEPRECATED.  See ApptReminderRule table instead.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string ApptReminderHourMessage = "ApptReminderHourMessage";

		/// <summary>
		/// DEPRECATED.  See ApptReminderRule table instead.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string ApptReminderSendAll = "ApptReminderSendAll";

		/// <summary>
		/// DEPRECATED.  See ApptReminderRule table instead.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string ApptReminderSendOrder = "ApptReminderSendOrder";

		/// <summary>
		/// Enum:ApptSchedEnforceSpecialty Allow by default. 
		/// 0=Allow, 1=Warn, 2=Block. Determines behavior when an appointment is scheduled in an operatory assigned to a 
		/// clinic for a patient with a specialty that does not exist in the list of that clinic's specialties.
		/// </summary>
		public const string ApptSchedEnforceSpecialty = "ApptSchedEnforceSpecialty";

		/// <summary>
		/// 	<para>
		///			Indicates whether appointments are allowed to overlap. Defaults to allow 
		///			overlap because blocking it is annoying.
		///		</para>
		///		<para>
		///			Type: <b>bool</b>, Default: <b>true</b>
		///		</para>
		/// </summary>
		public const string ApptsAllowOverlap = "ApptsAllowOverlap";

		/// <summary>
		/// Bool; True by default. When true, new appointments require at least one procedure to be attached.
		/// </summary>
		public const string ApptsRequireProc = "ApptsRequireProc";

		/// <summary>
		/// DEPRECATED. Use InsChecksFrequency instead.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string ApptsCheckFrequency = "ApptsCheckFrequency";

		///<summary>Bool; False by default.  When true, the secondary provider used when scheduling an appointment will use the Operatory's secondary provider no matter what.</summary>
		public const string ApptSecondaryProviderConsiderOpOnly = "ApptSecondaryProviderConsiderOpOnly";
		///<summary>True if automated appointment thank yous are enabled for the entire DB. See ApptReminderRules for setup details.
		///Permissions are still checked here at HQ so manually overriding this value will only make the program behave annoyingly, but won't break anything.</summary>
		public const string ApptThankYouAutoEnabled = "ApptThankYouAutoEnabled";
		///<summary>Used as the value of the SUMMARY field in a .ics file for Appointment Thank You [AddToCalendar] tag.</summary>
		public const string ApptThankYouCalendarTitle = "ApptThankYouCalendarTitle";
		///<summary>Date, MinDate by default.  The Date that was set within the "Archive entries on or before:" field within the Archive tab of the 
		///Backup window when the archive process was last ran successfully.</summary>
		public const string ArchiveDate = "ArchiveDate";

		/// <summary>
		/// In FormBackup the remove old data tab, if true make a backup when the user runs remove old data.
		/// </summary>
		public const string ArchiveDoBackupFirst = "ArchiveDoBackupFirst";

		/// <summary>
		/// String of random alpha-numeric characters that represents a synced key between an OD database and its specific archive database.
		/// </summary>
		public const string ArchiveKey = "ArchiveKey";

		/// <summary>
		/// Obfuscated password for the database user that will be used when directly connecting to the archive server. Not actually the hash.
		/// </summary>
		public const string ArchivePassHash = "ArchivePassHash";
		
		/// <summary>
		/// The name of the server where the archive database should be located.
		/// </summary>
		public const string ArchiveServerName = "ArchiveServerName";

		/// <summary>
		/// DEPRECATED. 
		/// Archiving with Middle Tier connection never fully implemented. 
		/// Here was the original intent behind the preference:
		/// URI to a Middle Tier web service that is connected to the database where archives should be made.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string ArchiveServerURI = "ArchiveServerURI";
		
		///<summary>The user name for the database user that will be used when directly connecting to the archive server.</summary>
		public const string ArchiveUserName = "ArchiveUserName";
		///<summary>Default billing types selected when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerBillingTypes = "ArManagerBillingTypes";
		///<summary>Default state for the exclude if bad address (no zipcode) checkbox when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerExcludeBadAddresses = "ArManagerExcludeBadAddresses";
		///<summary>Default state for the exclude if unsent procs checkbox when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerExcludeIfUnsentProcs = "ArManagerExcludeIfUnsentProcs";
		///<summary>Default state for the exclude if pending ins checkbox when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerExcludeInsPending = "ArManagerExcludeInsPending";
		///<summary>Default transaction types selected when loading the Sent Tab of the Accounts Receivable Manager - Sent tab.</summary>
		public const string ArManagerLastTransTypes = "ArManagerLastTransTypes";
		///<summary>Default account age when loading the Sent Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerSentAgeOfAccount = "ArManagerSentAgeOfAccount";
		///<summary>Default number of days since the last payment when loading the Sent Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerSentDaysSinceLastPay = "ArManagerSentDaysSinceLastPay";
		///<summary>Default minimum balances when loading the Sent Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerSentMinBal = "ArManagerSentMinBal";
		///<summary>Default account age when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerUnsentAgeOfAccount = "ArManagerUnsentAgeOfAccount";
		///<summary>Default number of days since the last payment when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerUnsentDaysSinceLastPay = "ArManagerUnsentDaysSinceLastPay";
		///<summary>Default minimum balances when loading the Unsent Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerUnsentMinBal = "ArManagerUnsentMinBal";
		///<summary>Default state for the exclude if bad address (no zipcode) checkbox when loading the Excluded Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerExcludedExcludeBadAddresses = "ArManagerExcludedExcludeBadAddresses";
		///<summary>Default state for the exclude if unsent procs checkbox when loading the Excluded Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerExcludedExcludeIfUnsentProcs = "ArManagerExcludedExcludeIfUnsentProcs";
		///<summary>Default state for the exclude if pending ins checkbox when loading the Excluded Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerExcludedExcludeInsPending = "ArManagerExcludedExcludeInsPending";
		///<summary>Default number of days since the last payment when loading the Excluded Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerExcludedDaysSinceLastPay = "ArManagerExcludedDaysSinceLastPay";
		///<summary>Default minimum balances when loading the Excluded Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerExcludedMinBal = "ArManagerExcludedMinBal";
		///<summary>Default account age when loading the Excluded Tab of the Accounts Receivable Manager.</summary>
		public const string ArManagerExcludedAgeOfAccount = "ArManagerExcludedAgeOfAccount";
		///<summary>The template that is used when manually texting patients on the ASAP list.</summary>
		public const string ASAPTextTemplate = "ASAPTextTemplate";

		/// <summary>
		/// Deprecated, but must remain here to avoid breaking updates.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string AtoZfolderNotRequired = "AtoZfolderNotRequired";

		/// <summary>
		/// Enum - Enumerations.DataStorageType.
		/// Normally 1 (AtoZ). 
		/// This used to be called AtoZfolderNotRequired, but that name was confusing.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string AtoZfolderUsed = "AtoZfolderUsed";

		/// <summary>
		/// The number of audit trail entries that are displayed in the grid.
		/// </summary>
		public const string AuditTrailEntriesDisplayed = "AuditTrailEntriesDisplayed";
		
		/// <summary>
		/// No UI for this pref. The number of clinics to run in parallel for AutoComm (eConfirms, eReminders, WebSchedRecall, etc.). 
		/// If set to 0, then the number of threads to use will be the number of cores on the machine. Defaults to 0.
		/// </summary>
		public const string AutoCommNumClinicsParallel = "AutoCommNumClinicsParallel";
		
		/// <summary>
		/// Used to determine the runtime of the threads that do automatic communication in the listener.
		/// Stored as a DateTime.
		/// </summary>
		public const string AutomaticCommunicationTimeStart = "AutomaticCommunicationTimeStart";
		
		/// <summary>
		/// Used to determine the runtime of the threads that do automatic communication in the listener.
		/// Stored as a DateTime.
		/// </summary>
		public const string AutomaticCommunicationTimeEnd = "AutomaticCommunicationTimeEnd";
		
		/// <summary>
		/// Boolean.
		/// Defaults to same value as ShowFeatureEhr.
		/// Used to determine whether automatic summary of care webmails are sent.
		/// </summary>
		public const string AutomaticSummaryOfCareWebmail = "AutomaticSummaryOfCareWebmail";
		public const string AutoResetTPEntryStatus = "AutoResetTPEntryStatus";
		
		/// <summary>
		/// Enum - AutoSplitPreference. Deprecated. 
		/// Defaults to Adjustments (1). 
		/// Used to choose order to apply unattached credits to adjustments in account module.
		/// </summary>
		public const string AutoSplitLogic = "AutoSplitLogic";

		public const string BackupExcludeImageFolder = "BackupExcludeImageFolder";
		public const string BackupFromPath = "BackupFromPath";
		public const string BackupReminderLastDateRun = "BackupReminderLastDateRun";
		public const string BackupRestoreAtoZToPath = "BackupRestoreAtoZToPath";
		public const string BackupRestoreFromPath = "BackupRestoreFromPath";
		public const string BackupRestoreToPath = "BackupRestoreToPath";
		public const string BackupToPath = "BackupToPath";
		public const string BadDebtAdjustmentTypes = "BadDebtAdjustmentTypes";
		public const string BalancesDontSubtractIns = "BalancesDontSubtractIns";
		public const string BankAddress = "BankAddress";
		public const string BankRouting = "BankRouting";
		public const string BillingAgeOfAccount = "BillingAgeOfAccount";
		public const string BillingChargeAdjustmentType = "BillingChargeAdjustmentType";
		public const string BillingChargeAmount = "BillingChargeAmount";
		public const string BillingChargeLastRun = "BillingChargeLastRun";

		/// <summary>
		/// Value is a string, either Billing or Finance.
		/// </summary>
		public const string BillingChargeOrFinanceIsDefault = "BillingChargeOrFinanceIsDefault";
		public const string BillingDefaultsInvoiceNote = "BillingDefaultsInvoiceNote";
		public const string BillingDefaultsIntermingle = "BillingDefaultsIntermingle";
		public const string BillingDefaultsLastDays = "BillingDefaultsLastDays";

		/// <summary>
		/// The statement modes that will also receive a text message. 
		/// Stored as a comma-separated list of integers where each item is the integer value of the StatementMode enum.
		/// </summary>
		public const string BillingDefaultsModesToText = "BillingDefaultsModesToText";

		public const string BillingDefaultsNote = "BillingDefaultsNote";

		/// <summary>
		/// Boolean, false by default. 
		/// Indicates if billing statements default to single patients(true) or guarantors(false).
		/// </summary>
		public const string BillingDefaultsSinglePatient = "BillingDefaultsSinglePatient";

		///<summary>The template used for SMS text notifications for statements.</summary>
		public const string BillingDefaultsSmsTemplate = "BillingDefaultsSmsTemplate";
		///<summary>Value is an integer, identifying the max number of statements that can be sent per batch.  Default of 0, which indicates no limit.
		///This preference is used for both printed statements and electronic ones.  It was decided to not rename the pref.</summary>
		public const string BillingElectBatchMax = "BillingElectBatchMax";
		///<summary>Deprecated.  Use ebill.ClientAcctNumber instead.</summary>
		public const string BillingElectClientAcctNumber = "BillingElectClientAcctNumber";
		///<summary>Boolean, true by default.  Indicates if electronic billing should generate a PDF document.</summary>
		public const string BillingElectCreatePDF = "BillingElectCreatePDF";
		public const string BillingElectCreditCardChoices = "BillingElectCreditCardChoices";
		///<summary>Deprecated.  Use ebill.ElectPassword instead.</summary>
		public const string BillingElectPassword = "BillingElectPassword";
		///<summary>No UI, can only be manually enabled by a programmer.  Only used for debugging electronic statements, because it will bloat the OpenDentImages folder.  Originally created to help with the "missing brackets bug" for EHG billing.</summary>
		public const string BillingElectSaveHistory = "BillingElectSaveHistory";
		///<summary>Output path for ClaimX EStatments.</summary>
		public const string BillingElectStmtOutputPathClaimX = "BillingElectStmtOutputPathClaimX";
		///<summary>Output path for EDS EStatments.</summary>
		public const string BillingElectStmtOutputPathEds = "BillingElectStmtOutputPathEds";
		///<summary>Output path for POS EStatments.</summary>
		public const string BillingElectStmtOutputPathPos = "BillingElectStmtOutputPathPos";
		///<summary>URL that EStatments are uploaded to for Dental X Change. Previously hardcoded in version 16.2.18 and below.</summary>
		public const string BillingElectStmtUploadURL = "BillingElectStmtUploadURL";
		///<summary>Deprecated.  Use ebill.ElectUserName instead.</summary>
		public const string BillingElectUserName = "BillingElectUserName";
		public const string BillingElectVendorId = "BillingElectVendorId";
		public const string BillingElectVendorPMSCode = "BillingElectVendorPMSCode";
		public const string BillingEmailBodyText = "BillingEmailBodyText";
		public const string BillingEmailSubject = "BillingEmailSubject";
		public const string BillingExcludeBadAddresses = "BillingExcludeBadAddresses";
		public const string BillingExcludeIfUnsentProcs = "BillingExcludeIfUnsentProcs";
		public const string BillingExcludeInactive = "BillingExcludeInactive";
		public const string BillingExcludeInsPending = "BillingExcludeInsPending";
		public const string BillingExcludeLessThan = "BillingExcludeLessThan";
		public const string BillingExcludeNegative = "BillingExcludeNegative";
		public const string BillingIgnoreInPerson = "BillingIgnoreInPerson";
		public const string BillingIncludeChanged = "BillingIncludeChanged";
		///<summary>Used with repeat charges to apply repeat charges to patient accounts on billing cycle date.</summary>
		public const string BillingUseBillingCycleDay = "BillingUseBillingCycleDay";
		public const string BillingSelectBillingTypes = "BillingSelectBillingTypes";
		///<summary>Boolean.  Defaults to true.  Determines if the billing window shows progress when sending statements.</summary>
		public const string BillingShowSendProgress = "BillingShowSendProgress";
		///<summary>Boolean. Allows option to show activity on statements since the last $0 balance on the account (or family).</summary>
		public const string BillingShowTransSinceBalZero = "BillingShowTransSinceBalZero";
		///<summary>0=no,1=EHG,2=POS(xml file),3=ClaimX(xml file),4=EDS(xml file)</summary>
		public const string BillingUseElectronic = "BillingUseElectronic";
		public const string BirthdayPostcardMsg = "BirthdayPostcardMsg";
		///<summary>FK to definition.DefNum.  The adjustment type that will be used on the adjustment that is automatically created when an appointment is broken.</summary>
		public const string BrokenAppointmentAdjustmentType = "BrokenAppointmentAdjustmentType";
		///<summary>Enumeration of type "BrokenApptProcedure".  Missed by default when D9986 is present.  This preference determines how broken appointments are handeld.</summary>
		public const string BrokenApptProcedure = "BrokenApptProcedure";
		///<summary>Deprecated.  Boolean.  0 by default.  When true, makes a commlog, otherwise makes an adjustment.</summary>
		public const string BrokenApptCommLogNotAdjustment = "BrokenApptCommLogNotAdjustment";
		///<summary>Boolean.  0 by default.  When true, makes a commlog when an appointment is broken.</summary>
		public const string BrokenApptCommLog = "BrokenApptCommLog";
		///<summary>Boolean.  0 by default.  When true, makes an adjustment when an appointment is broken.</summary>
		public const string BrokenApptAdjustment = "BrokenApptAdjustment";
		///<summary>Boolean. 0 by default. Require user to break scheduled appt before moving to a new day, the pinboard, the unscheduled list or deleting.</summary>
		public const string BrokenApptRequiredOnMove = "BrokenApptRequiredOnMove";
		///<summary>Boolean.  True by default.  When true, Canadian PPO insurance plans create estimates for labs (default behavior for category percentage plans).</summary>
		public const string CanadaCreatePpoLabEst = "CanadaCreatePpoLabEst";
		///<summary>For Ontario Dental Association fee schedules.</summary>
		public const string CanadaODAMemberNumber = "CanadaODAMemberNumber";
		///<summary>For Ontario Dental Association fee schedules.</summary>
		public const string CanadaODAMemberPass = "CanadaODAMemberPass";
		///<summary>Boolean.  0 by default.  If enabled, only CEMT can edit certain security settings.  Currently only used for global lock date.</summary>
		public const string CentralManagerSecurityLock = "CentralManagerSecurityLock";
		///<summary>This is the hash of the password that is needed to open the Central Manager tool.</summary>
		public const string CentralManagerPassHash = "CentralManagerPassHash";
		///<summary>This is the salt that is prepended to the password when hashing.  It provides an extra layer of security.</summary>
		public const string CentralManagerPassSalt = "CentralManagerPassSalt";
		///<summary>Blank by default.  Contains a key for the CEMT.  Each CEMT database contains a unique sync code.  Syncing from the CEMT will skip any databases without the correct sync code.</summary>
		public const string CentralManagerSyncCode = "CentralManagerSyncCode";



		///<summary>Connections initiated from within CEMT will use dynamic mode, allowing connection to db of mismatched version.</summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string CentralManagerUseDynamicMode = "CentralManagerUseDynamicMode";

		///<summary>Connections initiated from within CEMT will automatically log on the CEMT user.</summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string CentralManagerIsAutoLogon = "CentralManagerIsAutoLogon";

		///<summary>Deprecated.</summary>
		public const string ChartQuickAddHideAmalgam = "ChartQuickAddHideAmalgam";
		///<summary>Deprecated. If set to true (1), then after adding a proc, a row will be added to datatable instead of rebuilding entire datatable by making queries to the database.
		///This preference was never fully implemented and should not be used.  We may revisit some day.</summary>
		public const string ChartAddProcNoRefreshGrid = "ChartAddProcNoRefreshGrid";
		///<summary>Preference to warn users when they have a nonpatient selected.</summary>
		public const string ChartNonPatientWarn = "ChartNonPatientWarn";
		public const string ClaimAttachExportPath = "ClaimAttachExportPath";
		///<summary>Default value of "[PatNum]/".  Allows customization of ClaimIdentifier prefix format.</summary>
		public const string ClaimIdPrefix = "ClaimIdPrefix";
		public const string ClaimFormTreatDentSaysSigOnFile = "ClaimFormTreatDentSaysSigOnFile";
		///<summary>When true, the default ordering provider on medical eclaim procedures will be set to the procedure treating provider.</summary>
		public const string ClaimMedProvTreatmentAsOrdering = "ClaimMedProvTreatmentAsOrdering";
		public const string ClaimMedTypeIsInstWhenInsPlanIsMedical = "ClaimMedTypeIsInstWhenInsPlanIsMedical";
		///<summary>Boolean, 0 by default.  When true, only Batch Insurance in Manage Module can be used to finalize payments.</summary>
		public const string ClaimPaymentBatchOnly = "ClaimPaymentBatchOnly";
		///<summary>Int.  Valid values are >=0.  Use -1 to disable.  Used to be a date.  Now represents a rolling date, thus the name is a bit off.
		///We decided to keep the name the same instead of deprecating and creating a new pref, because user never sees the pref name and to avoid bloat.
		///Number of days (default 1) to subtract from the current date when deciding which NO PAYMENT claims and
		///$0 claimprocs to consider when using the This Claim Only button from the Edit Claim window or when creating a batch payment from Manage module.
		///Used for filtering Outstanding Claims from FormClaimPayBatch list, and when finalizing from Edit Claim window.</summary>
		public const string ClaimPaymentNoShowZeroDate = "ClaimPaymentNoShowZeroDate";
		///<summary>When true, procedurecode overrides will send the override's description to insurance instead of the original procedurecode's description.</summary>
		public const string ClaimPrintProcChartedDesc = "ClaimPrintProcChartedDesc";
		///<summary>Enum:ClaimProcCreditsGreaterThanProcFee.  Allow by default.  0=Allow, 1=Warn, 2=Block.  This preference either allows, warns or blocks the user from 
		///entering an insurance payment on the Enter Payment screen if (for a procedure) the sum of the Ins Pay + Writeoff + any attached adjustments + and attached 
		///patient payments > Procedure Fee </summary>
		public const string ClaimProcAllowCreditsGreaterThanProcFee = "ClaimProcAllowCreditsGreaterThanProcFee";
		///<summary>Boolean,  0 by default.  When true, allows claimprocs to be created for backdated completed procedures.</summary>
		public const string ClaimProcsAllowedToBackdate = "ClaimProcsAllowedToBackdate";
		///<summary>For the Procedures Not Billed to Insurance report.  If true, when creating new claims from the report window, will group procedures
		///by clinic and site.  If false, will block user from creating claims if the selected procedures for a specific patient have different
		///clinis or different sites.  Default value is true to encourage automation.</summary>
		public const string ClaimProcsNotBilledToInsAutoGroup = "ClaimProcsNotBilledToInsAutoGroup";
		///<summary>Blank by default.  Computer name to receive reports from automatically.</summary>
		public const string ClaimReportComputerName = "ClaimReportComputerName";
		///<summary>Boolean, 0 by default. When true, Open Dental Service will receive claim reports instead of the specified computer spawning a thread
		///in FormOpenDental.</summary>
		public const string ClaimReportReceivedByService = "ClaimReportReceivedByService";
		///<summary>Report receive interval. In minutes. 30 by default. If the ClaimReportReceiveLastDateTime preference is set, then this value will
		///be 0.</summary>
		public const string ClaimReportReceiveInterval = "ClaimReportReceiveInterval";
		///<summary>Stores last time the reports were ran.</summary>
		public const string ClaimReportReceiveLastDateTime = "ClaimReportReceiveLastDateTime";
		///<summary>Time to retrieve claim reports. Stored as a DateTime even though we only care about the time. If theClaimReportReceiveInterval 
		///preference is set, then this value will be an empty string.</summary>
		public const string ClaimReportReceiveTime = "ClaimReportReceiveTime";
		///<summary>Boolean.  0 by default.  If enabled, the Send Claims window will automatically validate e-claims upon loading the window.
		///Validating all claims on load was old behavior that was significantly slowing down the loading of the send claims window.
		///Several offices complained that we took away the validation until they attempt sending the claim.</summary>
		public const string ClaimsSendWindowValidatesOnLoad = "ClaimsSendWindowValidatesOnLoad";
		///<summary>Boolean.  0 by default.  If enabled, snapshots of claimprocs are created when claims are created.</summary>
		public const string ClaimSnapshotEnabled = "ClaimSnapshotEnabled";
		///<summary>DateTime where the time is the only useful part. 
		///Stores the time of day that the OpenDentalService should create a claimsnapshot.</summary>
		public const string ClaimSnapshotRunTime = "ClaimSnapshotRunTime";
		///<summary>Enumeration of type "ClaimSnapshotTrigger".  ClaimCreate by default.  This preference determines how ClaimSnapshots get created. Stored as the enumeration.ToString().</summary>
		public const string ClaimSnapshotTriggerType = "ClaimSnapshotTriggerType";
		///<summary>When set to true, adding a claim tracking status to a claim requires an error.</summary>
		public const string ClaimTrackingRequiresError = "ClaimTrackingRequiresError";
		///<summary>Bool determines if 'None' will show as an option in the custom tracking form. Defaults to false,'None' will show as an option.</summary>
		public const string ClaimTrackingStatusExcludesNone = "ClaimTrackingStatusExcludesNone";
		///<summary>Enumeration of type "ClaimZeroDollarProcBehavior". Defaults to 0 (Allow).  Determines if $0 procedures can be attached to claims.</summary>
		public const string ClaimZeroDollarProcBehavior = "ClaimZeroDollarProcBehavior";
		public const string ClaimsValidateACN = "ClaimsValidateACN";
		public const string ClearinghouseDefaultDent = "ClearinghouseDefaultDent";
		///<summary>FK to clearinghouse.ClearingHouseNum.  Allows a different clearinghouse to be used for checking eligibility.
		///Defaults to the current dental (or medical) clearinghouse which preserves old behavior.</summary>
		public const string ClearinghouseDefaultEligibility = "ClearinghouseDefaultEligibility";
		public const string ClearinghouseDefaultMed = "ClearinghouseDefaultMed";
		///<summary>Boolean.  0 by default.  If enabled, new patients can be added with an usassigned clinic.</summary>
		public const string ClinicAllowPatientsAtHeadquarters = "ClinicAllowPatientsAtHeadquarters";

		///<summary>String, "Workstation"(default), "User", "None". See FormMisc. Determines how recently viewed clinics should be tracked.</summary>
		public const string ClinicTrackLast = "ClinicTrackLast";
		///<summary>Boolean.  1 by default.  If enabled, displays 'Break' and 'Lunch' buttons in Manage Module, if disabled, changes 'Lunch' button text to
		///'Break' and disables/hides true 'Break' button.  Effectively, enabling this preference means on-the-clock-breaks are allowed, and 
		///disabling the preference means on-the-clock-breaks are not allowed.</summary>
		public const string ClockEventAllowBreak = "ClockEventAllowBreak";
		///<summary>Boolean. 0 by default. When set to true, all new clones will be put into their own family (guarantor as themselves)
		///and then a super family will be created if one does not exist or the new clone will be associated to the master clone's super family.
		///When set to false, all new clones blindly inherit their master's guarantor and super family settings.</summary>
		public const string CloneCreateSuperFamily = "CloneCreateSuperFamily";
		///<summary>YN enum. Defaults to Unknown. Indicates whether the office needs to reset their Cloud password. For non-Cloud offices, this will
		///be Unknown. Once they switch to Cloud, they will be prompted until they change the password, and this will be set to No.</summary>
		public const string CloudPasswordNeedsReset = "CloudPasswordNeedsReset";
		///<summary>0=standard, 1=alternate icons on ModuleBar and a few on Main Toolbar.  This no longer affects any colors.</summary>
		public const string ColorTheme = "ColorTheme";
		///<summary>Boolean.  False by default.  When true, causes CommLogs to auto-save on a timer.</summary>
		public const string CommLogAutoSave = "CommLogAutoSave";
		public const string ConfirmEmailMessage = "ConfirmEmailMessage";
		public const string ConfirmEmailSubject = "ConfirmEmailSubject";
		public const string ConfirmPostcardMessage = "ConfirmPostcardMessage";
		///<summary>FK to definition.DefNum.  Initially 0.</summary>
		public const string ConfirmStatusEmailed = "ConfirmStatusEmailed";
		///<summary>FK to definition.DefNum.</summary>
		public const string ConfirmStatusTextMessaged = "ConfirmStatusTextMessaged";
		///<summary>The message that goes out to patients when doing a batch confirmation.</summary>
		public const string ConfirmTextMessage = "ConfirmTextMessage";
		///<summary>Selected connection group within the CEMT.</summary>
		public const string ConnGroupCEMT = "ConnGroupCEMT";
		///<summary>Missing in general.  HQ Only.  JSON serialized dictionary of the default connection settings for several databases.
		///Key = ConnectionNames enum, Value = CentralConnection object.  These default connection settings can be overridden by SiteLinks.</summary>
		public const string ConnectionSettingsHQ = "ConnectionSettingsHQ";
		///<summary></summary>
		public const string CoPay_FeeSchedule_BlankLikeZero = "CoPay_FeeSchedule_BlankLikeZero";
		///<summary>Boolean.  Typically set to true when an update is in progress and will be set to false when finished.  Otherwise true means that the database is in a corrupt state.</summary>
		public const string CorruptedDatabase = "CorruptedDatabase";
		///<summary>This is the default encounter code used for automatically generating encounters when specific actions are performed in Open Dental.  The code is displayed/set in FormEhrSettings.  We will set it and give the user a list of 9 suggested codes to use such that the encounters generated will cause the pateint to be considered part of the initial patient population in the 9 clinical quality measures tracked by OD.  CQMDefaultEncounterCodeSystem will identify the code system this code is from and the code value will be a FK to that code system.</summary>
		public const string CQMDefaultEncounterCodeValue = "CQMDefaultEncounterCodeValue";
		public const string CQMDefaultEncounterCodeSystem = "CQMDefaultEncounterCodeSystem";
		public const string CropDelta = "CropDelta";
		///<summary>Used by OD HQ.  Not added to db convert script.  Allowable timeout for Negotiator to establish a connection with Listener. Different than SocketTimeoutMS and TransmissionTimeoutMS.  Specifies the allowable timeout for Patient Portal Negotiator to establish a connection with Listener.  Negotiator will only wait this long to get an acknowledgement that the Listener is available for a transmission before timing out.  Initially 10000</summary>
		public const string CustListenerConnectionRequestTimeoutMS = "CustListenerConnectionRequestTimeoutMS";
		///<summary>Used by OD HQ.  Not added to db convert script.  Will be passed to OpenDentalEConnector when service initialized.  Specifies the time (in minutes) between each time that the listener service will upload it's current heartbeat to HQ.  Initially 360</summary>
		public const string CustListenerHeartbeatFrequencyMinutes = "CustListenerHeartbeatFrequencyMinutes";
		///<summary>Used by OpenDentalEConnector.  String specifies which port the OpenDentalWebService should look for on the customer's server in order to create a socket connection.  Initially 25255</summary>
		public const string CustListenerPort = "CustListenerPort";
		///<summary>Used by OD HQ.  Not added to db convert script.  Will be passed to OpenDentalEConnector when service initialized.  Specifies the read/write socket timeout.  Initially 3000</summary>
		public const string CustListenerSocketTimeoutMS = "CustListenerSocketTimeoutMS";
		///<summary>Used by OD HQ.  Not added to db convert script.  Specifies the entire wait time alloted for a transmission initiated by the patient portal.  Negotiator will only wait this long for a valid response back from Listener before timing out.  Initially 30000</summary>		
		public const string CustListenerTransmissionTimeoutMS = "CustListenerTransmissionTimeoutMS";
		///<summary>Used by OD HQ. The name of the customers database that is considered the "source of truth".</summary>
		public const string CustomersHQDatabase = "CustomersHQDatabase";
		///<summary>Used by OD HQ. The MySQL password hash for the HQ customers database.</summary>
		public const string CustomersHQMySqlPassHash = "CustomersHQMySqlPassHash";
		///<summary>Used by OD HQ. The MySQL user for the HQ customers database.</summary>
		public const string CustomersHQMySqlUser = "CustomersHQMySqlUser";
		///<summary>Used by OD HQ. The name of the customers server that is considered the "source of truth".</summary>
		public const string CustomersHQServer = "CustomersHQServer";
		public const string CustomizedForPracticeWeb = "CustomizedForPracticeWeb";
		public const string DatabaseConvertedForMySql41 = "DatabaseConvertedForMySql41";
		///<summary>bool. Set to false by default. If true, the optimize database maintenance tool will be disabled.</summary>
		public const string DatabaseMaintenanceDisableOptimize = "DatabaseMaintenanceDisableOptimize";
		///<summary>bool. Set to false by default. If true, database maintenance will skip table checks.</summary>
		public const string DatabaseMaintenanceSkipCheckTable = "DatabaseMaintenanceSkipCheckTable";
		public const string DataBaseVersion = "DataBaseVersion";
		public const string DateDepositsStarted = "DateDepositsStarted";
		public const string DateLastAging = "DateLastAging";
		public const string DefaultCCProcs = "DefaultCCProcs";
		public const string DefaultClaimForm = "DefaultClaimForm";
		public const string DefaultProcedurePlaceService = "DefaultProcedurePlaceService";
		///<summary>Long. 0 by default. Used to assign a user group to a new user that is added by a user who does not have the SecurityAdmin user 
		///permission.</summary>
		public const string DefaultUserGroup = "DefaultUserGroup";
		///<summary>Bool.  Default true 1. Applies to all computers.</summary>
		public const string DirectX11ToothChartUseIfAvail = "DirectX11ToothChartUseIfAvail";
		///<summary>Boolean. Set to true by default. When true, patient fields that do not have a matching patient field def will display at the bottom
		///of the patient fields with gray text.</summary>
		public const string DisplayRenamedPatFields = "DisplayRenamedPatFields";
		///<summary>Boolean.  Set to 1 to indicate that this database holds customers instead of patients.  Used by OD HQ.  Used for showing extra phone numbers, showing some extra buttons for tools that only we use, behavior of checkboxes in repeating charge window, etc.  But phone panel visibility is based on DockPhonePanelShow.</summary>
		[Obsolete]
		public const string DistributorKey = "DistributorKey";
		///<summary>If this is true, then PrefC.IsODHQ will return true.</summary>
		[Obsolete]
		public const string DockPhonePanelShow = "DockPhonePanelShow";
		///<summary>The AtoZ folder path.</summary>
		public const string DocPath = "DocPath";
		///<summary>There is no UI for user to change this.  Used by OD HQ. Determines if Task refreshes only update locally.  True is local only, false is every workstation.</summary>
		public const string DoLimitTaskSignals = "DoLimitTaskSignals";
		/// <summary>Boolean. Determine whether or not to allow users to bypass OpenDentalLogin using ActiveDirectory. Default is false.</summary>
		public const string DomainLoginEnabled = "DomainLoginEnabled";
		///<summary>Specifies the path to use when ActiveDirectory/Domain Logins are enabled.</summary>
		public const string DomainLoginPath = "DomainLoginPath";
		///<summary>The ObjectGuid of the Active Directory domain object.</summary>
		public const string DomainObjectGuid = "DomainObjectGuid";
		///<summary>The date this customer last checked with HQ to determine which provider have access to eRx.</summary>
		public const string DoseSpotDateLastAccessCheck = "DoseSpotDateLastAccessCheck";
		///<summary>The ICD Diagnosis Code version primarily used by the practice.  Value of '9' for ICD-9, and '10' for ICD-10.</summary>
		public const string DxIcdVersion = "DxIcdVersion";
		///<summary>The last known date and time that the dynamic payplan service ran.</summary>
		public const string DynamicPayPlanLastDateTime = "DynamicPayPlanLastDateTime";
		///<summary>Defaults to 9AM.  The time the user has specified that they would like the service to run on each day.</summary>
		public const string DynamicPayPlanRunTime = "DynamicPayPlanRunTime";
		///<summary>The date and time that the service started running today. Will be blank if not currently running.</summary>
		public const string DynamicPayPlanStartDateTime = "DynamicPayPlanStartDateTime";
		public const string EasyBasicModules = "EasyBasicModules";

		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string EasyHideAdvancedIns = "EasyHideAdvancedIns";

		public const string EasyHideCapitation = "EasyHideCapitation";
		public const string EasyHideClinical = "EasyHideClinical";
		public const string EasyHideDentalSchools = "EasyHideDentalSchools";
		public const string EasyHideHospitals = "EasyHideHospitals";
		public const string EasyHideInsurance = "EasyHideInsurance";
		public const string EasyHideMedicaid = "EasyHideMedicaid";
		public const string EasyHidePrinters = "EasyHidePrinters";
		public const string EasyHidePublicHealth = "EasyHidePublicHealth";
		public const string EasyHideRepeatCharges = "EasyHideRepeatCharges";
		public const string EasyNoClinics = "EasyNoClinics";
		public const string EclaimsSeparateTreatProv = "EclaimsSeparateTreatProv";
		///<summary>Boolean, true by default. Basically enables eClipboard/self-check in. If this is true users can self-check in using the offices
		///set up devices.</summary>
		public const string EClipboardAllowSelfCheckIn = "EClipboardAllowSelfCheckIn";
		///<summary>Boolean, false by default. When true, allows patients on tablets to take a picture of themselves on check-in.</summary>
		public const string EClipboardAllowSelfPortraitOnCheckIn = "EClipboardAllowSelfPortraitOnCheckIn";
		///<summary>Boolean, true by default. If this is true, when the patient status is changed to the waiting room status, we will check if there
		///are any forms defined in the office eClipboard settings that the patient is due to fill out and attach those forms to the patient.</summary>
		public const string EClipboardCreateMissingFormsOnCheckIn = "EClipboardCreateMissingFormsOnCheckIn";
		///<summary>Plain text, this is the message we want to show users after they have used the self check-in feature.</summary>
		public const string EClipboardMessageComplete = "EClipboardMessageComplete";
		
		/// <summary>
		/// Boolean, false by default. 
		/// If this is true, when the front desk manually changes patient status to the waiting room status, this will cause the kiosk manager to automatically pop-up so that the office staff can decide if they want to send the patient to kiosk or eClipboard mode.
		/// </summary>
		public const string EClipboardPopupKioskOnCheckIn = "EClipboardPopupKioskOnCheckIn";
		
		/// <summary>
		/// Boolean, true by default. 
		/// If this is true and the office is using self check-in, if the patient has any "kiosk" forms on their account, we will display those forms on their device for them to fill out.
		/// </summary>
		public const string EClipboardPresentAvailableFormsOnCheckIn = "EClipboardPresentAvailableFormsOnCheckIn";
		
		/// <summary>Boolean. Note that this is actually ONLY a clinic pref, as it is meaningless for the default clinic and practices without
		///clinics. Indicates whether or not we should remove a clinics other EClipboard related clinic preferences.</summary>
		public const string EClipboardUseDefaults = "EClipboardUseDefaults";
		
		/// <summary>
		/// The ClinicNums that are signed up for eClipboard. 
		/// Checked and set periodically by the eConnector. 
		/// In a comma-delimited list. An empty string indicates no clinics are signed up.
		/// </summary>
		public const string EClipboardClinicsSignedUp = "EClipboardClinicsSignedUp";
		
		/// <summary>
		/// Boolean, false by default. 
		/// Will be set to true when the update server successfully upgrades the CustListener service to the eConnector service. 
		/// This only needs to happen once. This will automatically happen after upgrading past v15.4.
		/// If automatically upgrading the CustListener service to the eConnector service fails, they can click Install in eService Setup.
		/// NEVER programmatically set this preference back to false.
		/// </summary>
		public const string EConnectorEnabled = "EConnectorEnabled";
		
		/// <summary>
		/// long, Indicates, in seconds, how frequently the eConnector will update its Sms Messages cache and insert an SmsNotifications signal.
		/// </summary>
		public const string EConnectorSmsNotificationFrequency = "EConnectorSmsNotificationFrequency";
		
		/// <summary>
		/// A JSON string of disparate pieces of information regarding the eConnector.
		/// </summary>
		public const string EConnectorStatistics = "EConnectorStatistics";
		public const string EHREmailFromAddress = "EHREmailFromAddress";
		public const string EHREmailPassword = "EHREmailPassword";
		public const string EHREmailPOPserver = "EHREmailPOPserver";
		public const string EHREmailPort = "EHREmailPort";
		public const string EhrRxAlertHighSeverity = "EhrRxAlertHighSeverity";
		
		/// <summary>
		/// Boolean, false by default.
		/// When this is set and using eRx it will utilize the currently selected clinic instead of the patient's default clinic.
		/// </summary>
		public const string ElectronicRxClinicUseSelected = "ElectronicRxClinicUseSelected";
		
		/// <summary>
		/// This pref is hidden, so no practical way for user to turn this on. 
		/// Only used for ehr testing.
		/// </summary>
		public const string EHREmailToAddress = "EHREmailToAddress";
		
		/// <summary>
		/// Date when user upgraded to 13.1.14 and started using NewCrop Guids on Rxs.
		/// </summary>
		public const string ElectronicRxDateStartedUsing131 = "ElectronicRxDateStartedUsing131";
		
		///<summary>No UI for this pref. Number of failed consecutive email send attempts from the eConnector before an alert is shown.</summary>
		public const string EmailAlertMaxConsecutiveFails = "EmailAlertMaxConsecutiveFails";
		/// <summary>FK to EmailAddress.EmailAddressNum.  It is not required that a default be set.</summary>
		public const string EmailDefaultAddressNum = "EmailDefaultAddressNum";
		///<summary>Bool which indicates, where applicable, all emails being sent should have the EmailDisclaimerTemplate appended to the end of the EmailBody.</summary>
		public const string EmailDisclaimerIsOn = "EmailDisclaimerIsOn";
		///<summary>String provides template for any email correspondence methods participating in Email Disclaimer. Must include [PostalAddress] tag.</summary>
		public const string EmailDisclaimerTemplate = "EmailDisclaimerTemplate";
		///<summary>Deprecated.  Not used anywhere. Email messages are retreived from Open Dental Service.</summary>
		[Obsolete("This preference has been deprecated. Do not use.", true)]
		public const string EmailInboxComputerName = "EmailInboxComputerName";
		///<summary>Time interval in minutes describing how often to automatically check the email inbox for new messages. Default is 5 minutes.</summary>
		public const string EmailInboxCheckInterval = "EmailInboxCheckInterval";
		///<summary>String that contains the HTML shell for emails. </summary>
		public const string EmailMasterTemplate = "EmailMasterTemplate";
		///<summary>FK to EmailAddress.EmailAddressNum.  Used for webmail notifications (Patient Portal).</summary>
		public const string EmailNotifyAddressNum = "EmailNotifyAddressNum";
		/// <summary>Deprecated. Use emailaddress.EmailPassword instead.</summary>
		public const string EmailPassword = "EmailPassword";
		/// <summary>Deprecated. Use emailaddress.ServerPort instead.</summary>
		public const string EmailPort = "EmailPort";

		/// <summary>Deprecated. Use emailaddress.SenderAddress instead.</summary>
		public const string EmailSenderAddress = "EmailSenderAddress";

		/// <summary>Deprecated. Use emailaddress.SMTPserver instead.</summary>
		public const string EmailSMTPserver = "EmailSMTPserver";

		/// <summary>Deprecated. Use emailaddress.EmailUsername instead.</summary>
		public const string EmailUsername = "EmailUsername";

		/// <summary>Deprecated. Use emailaddress.UseSSL instead.</summary>
		public const string EmailUseSSL = "EmailUseSSL";

		/// <summary>
		/// Boolean. 0 means false and means it is not an EHR Emergency, and emergency access to the family module is not granted.
		/// </summary>
		public const string EhrEmergencyNow = "EhrEmergencyNow";

		/// <summary>
		/// There is no UI for this. 
		/// It's only used by OD HQ.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string EhrProvKeyGeneratorPath = "EhrProvKeyGeneratorPath";

		public const string EnableAnesthMod = "EnableAnesthMod";

		/// <summary>
		/// Warns the user if the Medicaid ID is not the proper number of digits for that state.
		/// </summary>
		public const string EnforceMedicaidIDLength = "EnforceMedicaidIDLength";

		/// <summary>
		/// Applies to these 7 windows: Recall, Confirmation, Planned Appt Tracker, Unsched, ASAP, Ins Verification, Text Messaging. 
		/// If this preference is turned on , then the "All Clinics" option is hidden, preventing user from clicking on that option and overloading the server.
		/// </summary>
		public const string EnterpriseApptList = "EnterpriseApptList";
		
		/// <summary>
		/// Boolean, false by default. When true the Appointment 'None' View will not be selected by default when there is at least one existing 
		/// Appointment view. False preserves current behavior - ie: Appointment 'None' View displays on first load of a clinic for a brand new user in a clinic.
		/// </summary>
		public const string EnterpriseNoneApptViewDefaultDisabled = "EnterpriseNoneApptViewDefaultDisabled";
		public const string EraAllowTotalPayments = "EraAllowTotalPayments";
		
		/// <summary>
		/// Boolean, false by default.
		/// When true then there will be 1 page per each claim paid for an ERA header and ERA claim paid on printouts.
		/// </summary>
		public const string EraPrintOneClaimPerPage = "EraPrintOneClaimPerPage";
		
		/// <summary>
		/// Boolean, true by default.
		/// When true, the ERA 'Verify and Enter Payment' window will post WriteOffs for procedures covered by category percentage or medicaid/flat copay insurance plans.
		/// When false, WriteOffs will not be posted for these insurance plan types.
		/// </summary>
		public const string EraIncludeWOPercCoPay = "EraIncludeWOPercCoPay";
		
		/// <summary>
		/// Boolean, false by default. 
		/// When true FormEtrans835 allows the user to filter by Control ID and shows the value in a grid column.
		/// Otherwise Control ID UI is not visable and can not be used.
		/// </summary>
		public const string EraShowControlIdFilter = "EraShowControlIdFilter";
		public const string ExportPath = "ExportPath";

		/// <summary>
		/// Allows guarantor access to all family health information in the patient portal.
		/// Default is 1.
		/// </summary>
		public const string FamPhiAccess = "FamPhiAccess";

		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string FeesUseCache = "FeesUseCache";

		public const string FinanceChargeAdjustmentType = "FinanceChargeAdjustmentType";
		public const string FinanceChargeAPR = "FinanceChargeAPR";
		public const string FinanceChargeAtLeast = "FinanceChargeAtLeast";
		public const string FinanceChargeLastRun = "FinanceChargeLastRun";
		public const string FinanceChargeOnlyIfOver = "FinanceChargeOnlyIfOver";

		/// <summary>
		/// Boolean. When true, blank fees in a Fixed Benefit type schedule will be treated as a $0 insurance coverage (patient owes the full amount of the PPO fee).
		/// When false, blank fees in a Fixed Benefit type schedule will not be treated as $0 insurance coverage(insurance is estimated at the full amount of the PPO fee).
		/// </summary>
		public const string FixedBenefitBlankLikeZero = "FixedBenefitBlankLikeZero";

		/// <summary>
		/// Double defaults to 0. Prevents clicks from happening in a window. 
		/// Currently used in ApptEdit to prevent procedures from being attached or detached for delay amount in tenths of a second.
		/// Will always be stored in en-US format.
		/// </summary>
		public const string FormClickDelay = "FormClickDelay";

		public const string FuchsListSelectionColor = "FuchsListSelectionColor";

		/// <summary>
		/// These were changes added by Dr. Fuchs many years ago, approx 2004-2008.
		/// Nobody but him uses this flag, so we'll be gradually deprecating places where this is used in order to tidy the code.
		/// </summary>
		public const string FuchsOptionsOn = "FuchsOptionsOn";

		/// <summary>
		/// Bool defaults to 0. When false, future dates transactions are not allowed.
		/// </summary>
		public const string FutureTransDatesAllowed = "FutureTransDatesAllowed";
		public const string GenericEClaimsForm = "GenericEClaimsForm";

		/// <summary>
		/// Used to store the ClinicNum of the last clinic the global update writeoff tool completed before being paused or interrupted.
		/// Only stored if an unrestricted user runs the update for all clinics. 
		/// When starting the tool again for an unrestricted user and all clinics the tool will pick up on the next clinic (ordered by primary key).
		/// </summary>
		public const string GlobalUpdateWriteOffLastClinicCompleted = "GlobalUpdateWriteOffLastClinicCompleted";

		/// <summary>
		/// A comma delimited list of computer names. If a computer name is in the list then it has verbose logging on.
		/// </summary>
		public const string HasVerboseLogging = "HasVerboseLogging";

		/// <summary>
		/// Encrypted.  Has no UI to change it, but status can be viewed in FormSupportStatus.
		/// Used to validate whether customer is on support and should have access to Help, Sparks3D, Imaging, etc.
		/// See the OpenDentalHelp project.
		/// </summary>
		public const string HelpKey = "HelpKey";
		public const string HL7FolderOut = "HL7FolderOut";
		public const string HL7FolderIn = "HL7FolderIn";

		///<summary>Deprecated.  Use SiteLink.EmployeeNum instead.  Used by HQ. Projected onto wall displayed on top of FormMapHQ</summary>
		public const string HQTriageCoordinator = "HQTriageCoordinator";
		///<summary>procedurelog.DiagnosticCode will be set to this for new procedures and complete procedures if this field was blank when set complete.
		///This can be an ICD-9 or an ICD-10.  In future versions, could be another an ICD-11, ICD-12, etc.</summary>
		public const string ICD9DefaultForNewProcs = "ICD9DefaultForNewProcs";
		///<summary>3-state prefernce used in the image module, state definitions are:
		///0 = Expand the document tree each time the Images module is visited.
		///1 = Document tree collapses when patient changes.
		///2 = Document tree folders persistent expand/collapse per user.</summary>
		public const string ImagesModuleTreeIsCollapsed = "ImagesModuleTreeIsCollapsed";
		
		/// <summary>
		/// Use the old Images module, prior to the 2020 overhaul. 
		/// Required for Suni capture and Apteryx XVWeb bridge. False by default. 
		/// The only interface for changing this is by enabling/disabling Suni and XVWeb bridges.
		/// </summary>
		public const string ImagesModuleUsesOld2020 = "ImagesModuleUsesOld2020";
		public const string ImageWindowingMax = "ImageWindowingMax";
		public const string ImageWindowingMin = "ImageWindowingMin";

		/// <summary>
		/// Boolean.  False by default. 
		/// When enabled a fix is enabled within ODTextBox (RichTextBox) for foreign users that use 
		/// a different language input methodology that requires the composition of symbols in order to display their language correctly.
		/// E.g. the Korean symbol '역' (dur) will not display correctly inside ODTextBoxes without this set to true.
		/// </summary>
		public const string ImeCompositionCompatibility = "ImeCompositionCompatibility";
		
		/// <summary>
		/// Boolean.  False by default.
		/// When true, when importing 834s, all patient plans that are not in the 834 will be replaced for the patient.
		/// </summary>
		public const string Ins834DropExistingPatPlans = "Ins834DropExistingPatPlans";
		public const string Ins834ImportPath = "Ins834ImportPath";
		public const string Ins834IsPatientCreate = "Ins834IsPatientCreate";

		///<summary>Comma delimited list of procedure codes that represent bitewing codes.  Defaults to D codes for all users.</summary>
		public const string InsBenBWCodes = "InsBenBWCodes";
		///<summary>Comma delimited list of procedure codes that represent exam codes.  Defaults to D codes for all users.</summary>
		public const string InsBenExamCodes = "InsBenExamCodes";
		///<summary>Comma delimited list of procedure codes that represent pano codes.  Defaults to D codes for all users.</summary>
		public const string InsBenPanoCodes = "InsBenPanoCodes";
		///<summary>Comma delimited list of procedure codes that represent cancer screening codes.  Defaults to D codes for all users.</summary>
		public const string InsBenCancerScreeningCodes = "InsBenCancerScreeningCodes";
		///<summary>Comma delimited list of procedure codes that represent prophy codes.  Defaults to D codes for all users.</summary>
		public const string InsBenProphyCodes = "InsBenProphyCodes";
		///<summary>Comma delimited list of procedure codes that represent flouride codes.  Defaults to D codes for all users.</summary>
		public const string InsBenFlourideCodes = "InsBenFlourideCodes";
		///<summary>Comma delimited list of procedure codes that represent sealant codes.  Defaults to D codes for all users.</summary>
		public const string InsBenSealantCodes = "InsBenSealantCodes";
		///<summary>Comma delimited list of procedure codes that represent crown codes.  Defaults to D codes for all users.</summary>
		public const string InsBenCrownCodes = "InsBenCrownCodes";
		///<summary>Comma delimited list of procedure codes that represent SRP codes.  Defaults to D codes for all users.</summary>
		public const string InsBenSRPCodes = "InsBenSRPCodes";
		///<summary>Comma delimited list of procedure codes that represent full debridement codes.  Defaults to D codes for all users.</summary>
		public const string InsBenFullDebridementCodes = "InsBenFullDebridementCodes";
		///<summary>Comma delimited list of procedure codes that represent perio maint codes.  Defaults to D codes for all users.</summary>
		public const string InsBenPerioMaintCodes = "InsBenPerioMaintCodes";
		///<summary>Comma delimited list of procedure codes that represent dentures codes.  Defaults to D codes for all users.</summary>
		public const string InsBenDenturesCodes = "InsBenDenturesCodes";
		///<summary>Comma delimited list of procedure codes that represent implant codes.  Defaults to D codes for all users.</summary>
		public const string InsBenImplantCodes = "InsBenImplantCodes";
		///<summary>0=Default practice provider, -1=Treating Provider. Otherwise, FK to provider.ProvNum.</summary>
		public const string InsBillingProv = "InsBillingProv";
		public const string InsDefaultCobRule = "InsDefaultCobRule";
		public const string InsDefaultPPOpercent = "InsDefaultPPOpercent";
		public const string InsDefaultShowUCRonClaims = "InsDefaultShowUCRonClaims";
		public const string InsDefaultAssignBen = "InsDefaultAssignBen";
		///<summary>Boolean.  False by default.  When true, insurance estimates will be recalculated on ClaimProcs even when marked as Received.  When 
		///false, ClaimProcs marked as Received will not recalculate insurance estimates.</summary>
		public const string InsEstRecalcReceived = "InsEstRecalcReceived";
		///<summary>Comma delimited list of procedure codes that represent bitewing codes used for insurance prior dates of service.  
		///Defaults to InsBenBWCodes codes.</summary>
		//[Description("Bitewing Ins Hist Codes")]
		public const string InsHistBWCodes = "InsHistBWCodes";
		///<summary>Comma delimited list of procedure codes that represent Debridement codes used for insurance prior dates of service.
		///Defaults to InsBenFullDebridementCodes codes.</summary>
		//[Description("Debridement Ins Hist Codes")]
		public const string InsHistDebridementCodes = "InsHistDebridementCodes";
		///<summary>Comma delimited list of procedure codes that represent exam codes used for insurance prior dates of service.
		///Defaults to InsBenExamCodes codes.</summary>
		//[Description("Exam Ins Hist Codes")]
		public const string InsHistExamCodes = "InsHistExamCodes";
		///<summary>Comma delimited list of procedure codes that represent pano codes used for insurance prior dates of service.
		///Defaults to InsBenPanoCodes codes.</summary>
		//[Description("FMX/Pano Ins Hist Codes")]
		public const string InsHistPanoCodes = "InsHistPanoCodes";
		///<summary>Comma delimited list of procedure codes that represent perio maintenance codes used for insurance prior dates of service.
		///Defaults to InsBenPerioMaintCodes codes.</summary>
		//[Description("Perio Maint Ins Hist Codes")]
		public const string InsHistPerioMaintCodes = "InsHistPerioMaintCodes";
		///<summary>Comma delimited list of procedure codes that represent perio LL codes used for insurance prior dates of service.
		///Defaults to InsBenSRPCodes codes</summary>
		//[Description("Perio Scaling LL Ins Hist Codes")]
		public const string InsHistPerioLLCodes = "InsHistPerioLLCodes";
		///<summary>Comma delimited list of procedure codes that represent perio LL codes used for insurance prior dates of service.
		///Defaults to InsBenSRPCodes codes.</summary>
		//[Description("Perio Scaling LR Ins Hist Codes")]
		public const string InsHistPerioLRCodes = "InsHistPerioLRCodes";
		///<summary>Comma delimited list of procedure codes that represent perio UL codes used for insurance prior dates of service.
		///Defaults to InsBenSRPCodes codes.</summary>
		//[Description("Perio Scaling UL Ins Hist Codes")]
		public const string InsHistPerioULCodes = "InsHistPerioULCodes";
		///<summary>Comma delimited list of procedure codes that represent perio UR codes used for insurance prior dates of service.
		///Defaults to InsBenSRPCodes codes.</summary>
		//[Description("Perio Scaling UR Ins Hist Codes")]
		public const string InsHistPerioURCodes = "InsHistPerioURCodes";
		///<summary>Comma delimited list of procedure codes that represent prophy codes used for insurance prior dates of service.
		///Defaults to InsBenProphyCodes codes.</summary>
		//[Description("Prophy Ins Hist Codes")]
		public const string InsHistProphyCodes = "InsHistProphyCodes";
		///<summary>Boolean. True by default. When enabled, disallow writeoffs amount greater than procedure fee.</summary>
		public const string InsPayNoWriteoffMoreThanProc = "InsPayNoWriteoffMoreThanProc";
		///<summary>0=unknown, user did not make a selection.  1=Yes, 2=No.</summary>
		public const string InsPlanConverstion_7_5_17_AutoMergeYN = "InsPlanConverstion_7_5_17_AutoMergeYN";
		///<summary>Boolean.  False by default.  When true, insurance plan with exclusions will always use UCR fee.</summary>
		public const string InsPlanUseUcrFeeForExclusions = "InsPlanUseUcrFeeForExclusions";
		///<summary>Boolean.  False by default.  When true, insurance plans with exclusions are marked as Do Not Bill Ins.</summary>
		public const string InsPlanExclusionsMarkDoNotBillIns = "InsPlanExclusionsMarkDoNotBillIns";
		///<summary>Boolean.  False by default.  When enabled, procedure fees will always use the UCR fee.</summary>
		public const string InsPpoAlwaysUseUcrFee = "InsPpoAlwaysUseUcrFee";
		///<summary>0 by default.  If false, secondary PPO writeoffs will always be zero (normal).  At least one customer wants to see secondary writeoffs.</summary>
		public const string InsPPOsecWriteoffs = "InsPPOsecWriteoffs";
		///<summary>Boolean, false by default.  When true, treatment plan module and appointment scheduling checks for frequency conflicts.</summary>
		public const string InsChecksFrequency = "InsChecksFrequency";
		public const string InsurancePlansShared = "InsurancePlansShared";
		///<summary>7 by default.  Number of days before displaying insurances that need verified.</summary>
		public const string InsVerifyAppointmentScheduledDays = "InsVerifyAppointmentScheduledDays";
		///<summary>90 by default. Number of days before requiring insurance plans to be verified.</summary>
		public const string InsVerifyBenefitEligibilityDays = "InsVerifyBenefitEligibilityDays";
		///<summary>1 by default.  Number of days that a past appointment will show in the "Past Due" insurance verification grid.</summary>
		public const string InsVerifyDaysFromPastDueAppt = "InsVerifyDaysFromPastDueAppt";
		///<summary>Boolean, false by default.  When true, defaults a filter to the current user instead of All when opening the InsVerifyList.</summary>
		public const string InsVerifyDefaultToCurrentUser = "InsVerifyDefaultToCurrentUser";
		///<summary>Boolean, false by default.  When true, excludes patient clones from the Insurance Verification List.</summary>
		public const string InsVerifyExcludePatientClones = "InsVerifyExcludePatientClones";
		///<summary>Boolean, false by default.  When true, excludes patient plans associated to insurance plans that are marked "Do Not Verify" from the Insurance Verification List.</summary>
		public const string InsVerifyExcludePatVerify = "InsVerifyExcludePatVerify";
		///<summary>Boolean, false by default.  When true, if an appointment is after the benefit renewal month for the insurance plan, make that plan be reverified and postdate the insverify.DateLastVerified.</summary>
		public const string InsVerifyFutureDateBenefitYear = "InsVerifyFutureDateBenefitYear";
		///<summary>30 by default.  Number of days before requiring patient plans to be verified.</summary>
		public const string InsVerifyPatientEnrollmentDays = "InsVerifyPatientEnrollmentDays";
		///<summary>Writeoff description displayed in the Account Module and on statements.  If blank, the default is "Writeoff".
		///We are using "Writeoff" since "PPO Discount" was only used for a brief time in 15.3 while it was Beta and no customer requested it</summary>
		public const string InsWriteoffDescript = "InsWriteoffDescript";
		public const string IntermingleFamilyDefault = "IntermingleFamilyDefault";
		///<summary>Missing in general.  JSON serialized dictionary of K=IntrospectionEntity, V=string that contains testing values meant for overriding release mode values.
		///Should only be present in testing databases. The presence of this preference means the program will be put into "Testing Mode" which is specifically designed for
		///pointing 3rd party bridges and web references to "development" or "testing" URLs.
		///The value of each entity can be treated as "Tag" and does not have to be confined to being a simple string (e.g. can be a serialized object stored as a string).
		///Never directly ask the cache for this preference.  Instead, use the Introspection class within the Misc folder of the OpenDentBusiness project in order to access it.</summary>
		public const string IntrospectionItems = "IntrospectionItems";
		///<summary>Preference to show writeoffs in the StatementInvoicePayment grid.</summary>
		public const string InvoicePaymentsGridShowNetProd = "InvoicePaymentsGridShowNetProd";
		///<summary>True if there is a row in the ehrprovkey table.  The OpenDentalService will check this preference and if it is false it will not query
		///the procedurelog table for scheduled non-CPOE radiology procs.  When the first row is inserted into the ehrprovkey table, or if there is an
		///existing row when the db is updated, this will be set to true.  Otherwise false.  Users can manually turn this pref on or off.</summary>
		public const string IsAlertRadiologyProcsEnabled = "IsAlertRadiologyProcsEnabled";
		///<summary>Enum.  Flags ItransNCpl.ItransUpdateFields: identifies what carrier fields to update when impotring carriers for ITRANS 2.0.</summary>
		public const string ItransImportFields = "ItransImportFields";
		public const string JobManagerDefaultEmail = "JobManagerDefaultEmail";
		public const string JobManagerDefaultBillingMsg = "JobManagerDefaultBillingMsg";
		public const string LabelPatientDefaultSheetDefNum = "LabelPatientDefaultSheetDefNum";
		///<summary>Used to determine how many windows are displayed throughout the program, translation, charting, and other features. Version 15.4.1</summary>
		public const string LanguageAndRegion = "LanguageAndRegion";
		///<summary>Initially set to Declined to Specify.  Indicates which language from the LanguagesUsedByPatients preference is the language that indicates the patient declined to specify.  Text must exactly match a language in the list of available languages.  Can be blank if the user deletes the language from the list of available languages.</summary>
		public const string LanguagesIndicateNone = "LanguagesIndicateNone";
		///<summary>Comma-delimited list of two-letter language names and custom language names.  The custom language names are the full string name and are not necessarily supported by Microsoft.</summary>
		public const string LanguagesUsedByPatients = "LanguagesUsedByPatients";
		public const string LetterMergePath = "LetterMergePath";

		///<summary>Boolean. Only used to override server time in the following places: Time Cards.</summary>
		public const string LocalTimeOverridesServerTime = "LocalTimeOverridesServerTime";
		public const string MainWindowTitle = "MainWindowTitle";

		///<summary>Enum. Flags MassEmailStatus. Defaults to None. Used for the clinicpref table. When activated and enabled, this practice or 
		///clinic is enabled to send mass emails.</summary>
		public const string MassEmailStatus = "MassEmailStatus";

		///<summary>String. The Guid for a clinic's mass email account. Plain text.</summary>
		public const string MassEmailGuid = "MassEmailGuid";

		///<summary>String. The secret for a clinic's mass email account. Equivalent to an API key or password. Plain text.</summary>
		public const string MassEmailSecret = "MassEmailSecret";

		///<summary>0=Meaningful Use Stage 1, 1=Meaningful Use Stage 2, 2=Meaningful Use Modified Stage 2.  Global, affects all providers.  Changes the MU grid that is seen for individual patients and for summary reports.</summary>
		public const string MeaningfulUseTwo = "MeaningfulUseTwo";

		///<summary>Number of days after medication order start date until stop date.  Used when automatically inserting a medication order when creating
		///a new Rx.  Default value is 7 days.  If set to 0 days, the automatic stop date will not be entered.</summary>
		public const string MedDefaultStopDays = "MedDefaultStopDays";

		///<summary>New procs will use the fee amount tied to the medical code instead of the ADA code.</summary>
		public const string MedicalFeeUsedForNewProcs = "MedicalFeeUsedForNewProcs";

		///<summary>FK to medication.MedicationNum</summary>
		public const string MedicationsIndicateNone = "MedicationsIndicateNone";

		///<summary>If MedLabReconcileDone=="0", a one time reconciliation of the MedLab HL7 messages is needed. The reconcile will reprocess the original
		///HL7 messages for any MedLabs with PatNum=0 in order to create the embedded PDF files from the base64 text in the ZEF segments. The old method
		///of waiting to extract these files until the message is manually attached to a patient was very slow using the middle tier. The new method is to
		///create the PDF files and save them in the image folder in a subdirectory called "MedLabEmbeddedFiles" if a pat is not located from the details
		///in the PID segment of the message. Attaching the MedLabs to a patient is now just a matter of moving the files to the patient's image folder.
		///All files will now be extracted and stored, either in a pat's folder or in the "MedLabEmbeddedFiles" folder, by the HL7 service.</summary>
		public const string MedLabReconcileDone = "MedLabReconcileDone";


		/// <summary>
		/// Deprecated. Always false.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string MiddleTierCacheFees = "MiddleTierCacheFees";

		public const string MobileAutoUnlockCode = "MobileAutoUnlockCode";

		public const string MobileSyncDateTimeLastRun = "MobileSyncDateTimeLastRun";

		///<summary>Used one time after the conversion to 7.9 for initial synch of the provider table.</summary>
		public const string MobileSynchNewTables79Done = "MobileSynchNewTables79Done";
		
		///<summary>Used one time after the conversion to 11.2 for re-synch of the patient records because a)2 columns BalTotal and InsEst have been added to the patientm table. b) the table documentm has been added</summary>
		public const string MobileSynchNewTables112Done = "MobileSynchNewTables112Done";
		
		///<summary>Used one time after the conversion to 12.1 for the recallm table being added and for upload of the practice Title.</summary>
		public const string MobileSynchNewTables121Done = "MobileSynchNewTables121Done";
		public const string MobileSyncIntervalMinutes = "MobileSyncIntervalMinutes";
		public const string MobileSyncServerURL = "MobileSyncServerURL";
		public const string MobileSyncWorkstationName = "MobileSyncWorkstationName";
		public const string MobileExcludeApptsBeforeDate = "MobileExcludeApptsBeforeDate";
		public const string MobileUserName = "MobileUserName";
		public const string MobileWebClinicsSignedUp = "MobileWebClinicsSignedUp";


		///<summary>The major and minor version of the current MySQL connection.  Gets updated on startup when a new version is detected.</summary>
		public const string MySqlVersion = "MySqlVersion";
		///<summary>True by default.  Will use the claimsnapshot table for calculating production in the Net Production Detail report if the date range is today's date only.</summary>
		public const string NetProdDetailUseSnapshotToday = "NetProdDetailUseSnapshotToday";
		///<summary>There is no UI for user to change this.  Format, if OD customer, is PatNum-(RandomString)(CheckSum).  Example: 1234-W6c43.  Format for resellers is up to them.</summary>
		public const string NewCropAccountId = "NewCropAccountId";
		///<summary>The date this customer last checked with HQ to determine which provider have access to eRx.</summary>
		public const string NewCropDateLastAccessCheck = "NewCropDateLastAccessCheck";
		///<summary>True for customers who were using NewCrop before version 15.4.  True if NewCropAccountId was not blank when upgraded.</summary>
		public const string NewCropIsLegacy = "NewCropIsLegacy";
		///<summary>Controls which NewCrop database to use.  If false, then the customer uses the First Data Bank (FDB) database, otherwise the 
		///customer uses the LexiData database.  Connecting to LexiData saves NewCrop some money on the new accounts.  Additionally, the RxNorms which
		///come back from the prescription refresh in the Chart are more complete for the LexiData database than for the FDB database.</summary>
		public const string NewCropIsLexiData = "NewCropIsLexiData";
		///<summary>There is no UI for user to change this. For distributors, this is part of the credentials.  OD credentials are not stored here, but are hard-coded.</summary>
		public const string NewCropName = "NewCropName";
		///<summary>There is no UI for user to change this.  For distributors, this is part of the credentials.
		///OD credentials are not stored here, but are hard-coded.</summary>
		public const string NewCropPartnerName = "NewCropPartnerName";
		///<summary>There is no UI for user to change this.  For distributors, this is part of the credentials.
		///OD credentials are not stored here, but are hard-coded.</summary>
		public const string NewCropPassword = "NewCropPassword";
		///<summary>URL of the time server to use for EHR time synchronization.  Only used for EHR.  Example nist-time-server.eoni.com</summary>
		public const string NistTimeServerUrl = "NistTimeServerUrl";

		/// <summary>
		///		<para>
		///			A value indicating whether notes can only be signed by providers.
		///		</para>
		///		<para>
		///			Type: <b>bool</b> - Default: <b>false</b>
		///		</para>
		/// </summary>
		public const string NotesProviderSignatureOnly = "NotesProviderSignatureOnly";



		///<summary>Duration the eConnector will maintain in memory cache of SmsToMobile and SmsFromMobile messages before purging older messages.
		///Stored as hours.</summary>
		public const string ODMobileCacheDurationHours = "ODMobileCacheDurationHours";
		///<summary>Missing in general: When clicking the "?" button with this preference present, the name of the Form (in the code) is placed in the user's clipboard.
		///This helps the documentation team create a document of new forms and their associated manual page.</summary>
		public const string OpenDentalHelpCaptureFormName = "OpenDentalHelpCaptureFormName";
		///<summary>Records the name of the computer running OpenDentalService.</summary>
		public const string OpenDentalServiceComputerName = "OpenDentalServiceComputerName";
		///<summary>Records a heartbeat for OpenDentalService. If the heartbeat is older than 6 minutes, an alert will notify the user.</summary>
		public const string OpenDentalServiceHeartbeat = "OpenDentalServiceHeartbeat";
		public const string OpenDentalVendor = "OpenDentalVendor";
		public const string OracleInsertId = "OracleInsertId";
		///<summary>User-defined automatic ortho claim procedure.  D8670.auto by default. Can be overridden at the insplan level.</summary>
		public const string OrthoAutoProcCodeNum = "OrthoAutoProcCodeNum";
		///<summary>User-defined comma separated string of procedure codes that can be attached to ortho cases as banding procedures</summary>
		public const string OrthoBandingCodes = "OrthoBandingCodes";
		///<summary>When turned on, ortho case information is shown in the ortho chart.</summary>
		public const string OrthoCaseInfoInOrthoChart = "OrthoCaseInfoInOrthoChart";
		///<summary>Determines whether claims with ortho procedures on them will automatically be marked as Ortho claims.</summary>
		public const string OrthoClaimMarkAsOrtho = "OrthoClaimMarkAsOrtho";
		///<summary>When true, ortho claims' "OrthoDate" will be automatically set to the patient's first ortho procedure when created.</summary>
		public const string OrthoClaimUseDatePlacement = "OrthoClaimUseDatePlacement";
		///<summary>User-defined comma separated string of procedure codes that can be attached to ortho cases as debond procedures</summary>
		public const string OrthoDebondCodes = "OrthoDebondCodes";
		///<summary>Byte, 24 by default.  The default number of months ortho treatments last.  Overridden by patientnote.OrthoMonthsTreat.</summary>
		public const string OrthoDefaultMonthsTreat = "OrthoDefaultMonthsTreat";
		///<summary>Defines whether ortho chart and ortho features should show.</summary>
		public const string OrthoEnabled = "OrthoEnabled";
		///<summary>When turned on, prompts the user to move any completed ortho procedures' fees' to be moved to the D8080 procedurecode.</summary>
		public const string OrthoInsPayConsolidated = "OrthoInsPayConsolidated";
		///<summary>Comma delimited list of procedure code CodeNum's.  These procedures are used as flags in order to determine the Patients' DatePlacement.
		///DatePlacement is the ProcDate of the first completed procedure that is associated to any of the procedure codes in this list.</summary>
		public const string OrthoPlacementProcsList = "OrthoPlacementProcsList";
		///<summary>User-defined comma separated string of procedure codes that can be attached to ortho cases as visit procedures</summary>
		public const string OrthoVisitCodes = "OrthoVisitCodes";
		///<summary>Enum:RpOutstandingIns.DateFilterTab. Defaults to DaysOld. Determines which date filter tab to default load in Outstanding Insurance 
		///Report.</summary>
		public const string OutstandingInsReportDateFilterTab = "OutstandingInsReportDateFilterTab";
		public const string PasswordsMustBeStrong = "PasswordsMustBeStrong";
		///<summary>Boolean.  False by default.  When true strong passwords require a special character (Non letter or digit).</summary>
		public const string PasswordsStrongIncludeSpecial = "PasswordsStrongIncludeSpecial";
		///<summary>Boolean.  False by default.  When true and PasswordsMustBeStrong is also true users without strong passwords will be prompted to change their password at next login.</summary>
		public const string PasswordsWeakChangeToStrong = "PasswordsWeakChangeToStrong";
		public const string PatientAllSuperFamilySync = "PatientAllSuperFamilySync";
		///<summary>The way that dates should be formatted when communicating with patients. Defaults to "d" which is equivalent to .ToShortDateString().
		///User editable.  Whatever value is in this preference is intended to be passed to DateTime.ToString().
		///Used in eReminders, eConfirms, manual confirmations, ASAP list texting, and other places.</summary>
		public const string PatientCommunicationDateFormat = "PatientCommunicationDateFormat";
		public const string PatientFormsShowConsent = "PatientFormsShowConsent";
		///<summary>Bool, false by default. Global pref. When true, the current patient and module will be maintained when switching OD users.</summary>
		public const string PatientMaintainedOnUserChange = "PatientMaintainedOnUserChange";
		///<summary>Defaults to 0 - Unknown using the YN enum, which will behave the same as if it's set to 2 - No using the YN enum.  If set to 1 - Yes
		///using the YN enum, the patient table phone number values will be synced to the phonenumber table, and stored in the PhoneNumberDigits column
		///with non-digit chars stripped out.  The select patient query will use the phonenumber table when searching for a pat by phone number.</summary>
		public const string PatientPhoneUsePhonenumberTable = "PatientPhoneUsePhonenumberTable";
		///<summary>Boolean.  Defaults to false. Used for the clinicpref table, in addition to the preference table. When true, this practice or 
		///clinic is enabled to send Patient Portal Invites.</summary>
		public const string PatientPortalInviteEnabled = "PatientPortalInviteEnabled";
		///<summary>Boolean.  Defaults to true. Only used for the clinicpref table, not the preference table. When true, this clinic will use the 
		///Patient Portal Invite rules for ClinicNum 0.</summary>
		public const string PatientPortalInviteUseDefaults = "PatientPortalInviteUseDefaults";
		///<summary>Free-form 'Body' text of the notification sent by this practice when a new secure EmailMessage is sent to patient.</summary>
		public const string PatientPortalNotifyBody = "PatientPortalNotifyBody";
		///<summary>Free-form 'Subject' text of the notification sent by this practice when a new secure EmailMessage is sent to patient.</summary>
		public const string PatientPortalNotifySubject = "PatientPortalNotifySubject";
		///<summary>Boolean.  Defaults to false.  True if the office is signed up for patient portal. Currently only set in AutoCommPatientPortalInvites.</summary>
		public const string PatientPortalSignedUp = "PatientPortalSignedUp";
		public const string PatientPortalURL = "PatientPortalURL";
		///<summary>Boolean. Defaults to false. When false: User can see patients not in their list of restricted clinics when they select clinics="All"
		///in FromPatientSelect.cs. When true: Clinics="All" list in FormPatientSelect.cs will only show patients who have had an appointment at 
		///user's unrestricted clinics or are assigned to one of the user's list of unrestricted clinics.</summary>
		public const string PatientSelectFilterRestrictedClinics = "PatientSelectFilterRestrictedClinics";
		///<summary>1 by default.  The minimum allowed value is 1, maximum allowed is 10.  The minimum number of characters entered into a textbox in the
		///patient select window before triggering a search and fill grid.  With every textbox text changed event, compares the textbox text length to
		///this pref and if the length is less than this pref a timer is started with interval equal to PatientSelectSearchPauseMs.  If the length is >=
		///this pref, the search and fill grid is triggered without waiting.</summary>
		public const string PatientSelectSearchMinChars = "PatientSelectSearchMinChars";
		///<summary>1 by default.  The minimum allowed value is 1, maximum allowed is 10,000.  The number of milliseconds to wait after the last character 
		///is entered into a textbox in the patient select window before triggering a search and fill grid.  Only waits this long if the textbox text 
		///length is &lt; PatientSelectSearchMinChars.</summary>
		public const string PatientSelectSearchPauseMs = "PatientSelectSearchPauseMs";
		///<summary>Represents a bool with a third state for 'unset'.  Use Yes, No, Unknown enum.  If No, don't automatically
		///search and fill the grid of the select patient window when all of the textboxes are blank.  If a patient was initially set on load and the user
		///clears the search fields, the previous search results will remain in the grid.  The grid will be refilled when data is entered.</summary>
		public const string PatientSelectSearchWithEmptyParams = "PatientSelectSearchWithEmptyParams";
		///<summary>Boolean. True by default. When true, automatically 'shows inactive patients' in the patient selection window.</summary>
		public const string PatientSelectShowInactive = "PatientSelectShowInactive";
		public const string PatientSelectUseFNameForPreferred = "PatientSelectUseFNameForPreferred";
		///<summary>Boolean. This is the default for new computers, otherwise it uses the computerpref PatSelectSearchMode.</summary>
		public const string PatientSelectUsesSearchButton = "PatientSelectUsesSearchButton";
		///<summary>Boolean. False by default. When true, mask patient date of birth in ChartModule, FamilyModule, PatientSelect, PatientEdit</summary>
		public const string PatientDOBMasked = "PatientDOBMasked";

		/// <summary>
		///		<para>
		///			A value indicating whether a patient social security number must be masked.
		///		</para>
		///		<para>
		///			Type: <b>bool</b>, Default: <b>true</b>
		///		</para>
		///	</summary>
		public const string PatientSSNMasked = "PatientSSNMasked";


		///<summary>Boolean. False by default.  When true and assigning new primary insurance, the associated patients billing type will be inherited from insPlan.BillingType</summary>
		public const string PatInitBillingTypeFromPriInsPlan = "PatInitBillingTypeFromPriInsPlan";
		///<summary>Deprecated. PaySplitManager enum. 1 by default. 0=DoNotUse, 1=Prompt, 2=Force</summary>
		public const string PaymentsPromptForAutoSplit = "PaymentsPromptForAutoSplit";
		///<summary>0 by default.1=Prompt users to select payment type when creating new Payments.</summary>
		public const string PaymentsPromptForPayType = "PaymentsPromptForPayType";
		///<summary>Bool, false by default.  When true, limit income transfers to the amount of associated patient payments.</summary>
		public const string PaymentsTransferPatientIncomeOnly = "PaymentsTransferPatientIncomeOnly";
		///<summary>PayClinicSetting enum. 0 by default. 0=SelectedClinic, 1=PatientDefaultClinic, 2=SelectedExceptHQ</summary>
		public const string PaymentClinicSetting = "PaymentClinicSetting";
		///<summary>When true, the payment window does not show paysplits by default.</summary>
		public const string PaymentWindowDefaultHideSplits = "PaymentWindowDefaultHideSplits";
		///<summary>Int.  Represents PayPeriodInterval enum (Weekly, Bi-Weekly, Monthly). </summary>
		public const string PayPeriodIntervalSetting = "PayPeriodIntervalSetting";
		///<summary>Int.  If set, represents the number of days after the pay period the pay day is.</summary>
		public const string PayPeriodPayAfterNumberOfDays = "PayPeriodPayAfterNumberOfDays";
		///<summary>Boolean.  True by default.  If true, pay days will fall before weekends.  If false, pay days will fall after weekends.</summary>
		public const string PayPeriodPayDateBeforeWeekend = "PayPeriodPayDateBeforeWeekend";
		///<summary>Boolean.  True by default.  Pay Day cannot fall on weekend if true.</summary>
		public const string PayPeriodPayDateExcludesWeekends = "PayPeriodPayDateExcludesWeekends";
		///<summary>Int. If set to 0, it's disabled, but any other number represents a day of the week. 1:Sunday, 2:Monday etc...</summary>
		public const string PayPeriodPayDay = "PayPeriodPayDay";
		/// <summary>Long. Stores the defnum of the neg adjustment type chosen to use for pay plan adjustments default. </summary>
		public const string PayPlanAdjType = "PayPlanAdjType";
		/// <summary>bool. Set to false by default. If true, the "Due Now" column will be hidden from pay plans grid in acct module.</summary>
		public const string PayPlanHideDueNow = "PayPlanHideDueNow";
		public const string PayPlansBillInAdvanceDays = "PayPlansBillInAdvanceDays";
		///<summary>Boolean.  False by default.  If true, payment plan window will exclude past activity in the amortization grid by default.</summary>
		public const string PayPlansExcludePastActivity = "PayPlansExcludePastActivity";
		public const string PayPlansUseSheets = "PayPlansUseSheets";
		///<summary>The Payment Plan version that the customer is using. Derives from PayPlanVersions enum.
		///Valid values are 1, 2, 3 or 4. 1 is legacy(Do Not Age) 2 is the default(Age Credits and Debits).</summary>
		public const string PayPlansVersion = "PayPlansVersion";
		public const string PerioColorCAL = "PerioColorCAL";
		public const string PerioColorFurcations = "PerioColorFurcations";
		public const string PerioColorFurcationsRed = "PerioColorFurcationsRed";
		public const string PerioColorGM = "PerioColorGM";
		public const string PerioColorMGJ = "PerioColorMGJ";
		public const string PerioColorProbing = "PerioColorProbing";
		public const string PerioColorProbingRed = "PerioColorProbingRed";
		public const string PerioRedCAL = "PerioRedCAL";
		public const string PerioRedFurc = "PerioRedFurc";
		public const string PerioRedGing = "PerioRedGing";
		public const string PerioRedMGJ = "PerioRedMGJ";
		public const string PerioRedMob = "PerioRedMob";
		public const string PerioRedProb = "PerioRedProb";
		///<summary>Enabled by default.  When a new perio exam is created, will always mark all missing teeth as skipped.</summary>
		public const string PerioSkipMissingTeeth = "PerioSkipMissingTeeth";
		///<summary>Enabled by default.  When a tooth with an implant procedure completed will not be skipped on perio exams</summary>
		public const string PerioTreatImplantsAsNotMissing = "PerioTreatImplantsAsNotMissing";
		///<summary>Can be any int.  Defaults to 0.</summary>
		public const string PlannedApptDaysFuture = "PlannedApptDaysFuture";
		///<summary>Can be any int.  Defaults to 365.</summary>
		public const string PlannedApptDaysPast = "PlannedApptDaysPast";
		public const string PlannedApptTreatedAsRegularAppt = "PlannedApptTreatedAsRegularAppt";
		public const string PracticeAddress = "PracticeAddress";
		public const string PracticeAddress2 = "PracticeAddress2";
		public const string PracticeBankNumber = "PracticeBankNumber";
		public const string PracticeBillingAddress = "PracticeBillingAddress";
		public const string PracticeBillingAddress2 = "PracticeBillingAddress2";
		public const string PracticeBillingCity = "PracticeBillingCity";
		public const string PracticeBillingST = "PracticeBillingST";
		public const string PracticeBillingZip = "PracticeBillingZip";
		public const string PracticeCity = "PracticeCity";
		public const string PracticeDefaultBillType = "PracticeDefaultBillType";
		public const string PracticeDefaultProv = "PracticeDefaultProv";
		///<summary>In USA and Canada, enforced to be exactly 10 digits or blank.</summary>
		public const string PracticeFax = "PracticeFax";
		///<summary>This preference is used to hide/change certain OD features, like hiding the tooth chart and changing 'dentist' to 'provider'.</summary>
		public const string PracticeIsMedicalOnly = "PracticeIsMedicalOnly";
		public const string PracticePayToAddress = "PracticePayToAddress";
		public const string PracticePayToAddress2 = "PracticePayToAddress2";
		public const string PracticePayToCity = "PracticePayToCity";
		public const string PracticePayToST = "PracticePayToST";
		public const string PracticePayToZip = "PracticePayToZip";
		
		/// <summary>
		/// In USA and Canada, enforced to be exactly 10 digits or blank.
		/// </summary>
		public const string PracticePhone = "PracticePhone";
		public const string PracticeST = "PracticeST";
		public const string PracticeTitle = "PracticeTitle";
		public const string PracticeZip = "PracticeZip";
		
		/// <summary>
		/// Boolean. 
		/// False by default.
		/// If true, checks "Preferred only" in FormReferralSelect.
		/// </summary>
		public const string ShowPreferedReferrals = "ShowPreferedReferrals";
		
		/// <summary>
		/// Enum:EmailType 0=Regular 1=Html 2=RawHtml. 
		/// Used to determine format for email for patient portal web mail messages.
		/// </summary>
		public const string PortalWebEmailTemplateType = "PortalWebEmailTemplateType";
		
		/// <summary>
		/// This is the default pregnancy code used for diagnosing pregnancy from FormVitalSignEdit2014 and is displayed/set in FormEhrSettings.
		/// When the check box for BMI and BP not taken due to pregnancy Dx is selected, this code value will be inserted into the diseasedef table in the column identified by the PregnancyDefaultCodeSystem (i.e. diseasedef.SnomedCode, diseasedef.ICD9Code).
		/// It will then be a FK in the diseasedef table to the associated code system table.
		/// </summary>
		public const string PregnancyDefaultCodeValue = "PregnancyDefaultCodeValue";

		public const string PregnancyDefaultCodeSystem = "PregnancyDefaultCodeSystem";

		/// <summary>
		///		<para>
		///			Indicates whether prepayments are allowed on TP procedures.
		///		</para>
		///		<para>
		///			Type: <b>bool</b>, Default: <b>false</b>
		///		</para>
		/// </summary>
		public const string PrePayAllowedForTpProcs = "PrePayAllowedForTpProcs";

		///<summary>FK to definition.DefNum for PaySplitUnearnedType DefinitionCategory.(29)</summary>
		public const string PrepaymentUnearnedType = "PrepaymentUnearnedType";

		/// <summary>
		/// Prints statements alphabetically by patients last name then first name within the billing window. 
		/// If false, it will print each clinic together and then within each clinic, it will be alphabetized.
		/// </summary>
		public const string PrintStatementsAlphabetically = "PrintStatementsAlphabetically";

		/// <summary>
		/// In Patient Edit and Add Family windows, the Primary Provider defaults to 'Select Provider' instead of the practice provider.
		/// </summary>
		public const string PriProvDefaultToSelectProv = "PriProvDefaultToSelectProv";

		/// <summary>FK to diseasedef.DiseaseDefNum</summary>
		public const string ProblemsIndicateNone = "ProblemsIndicateNone";

		/// <summary>Deprecated. Also spelled wrong.</summary>
		public const string ProblemListIsAlpabetical = "ProblemListIsAlpabetical";

		/// <summary>Determines default sort order of Proc Codes list when accessed from Lists -> Procedure Codes.  Enum:ProcCodeListSort, 0 by default.</summary>
		public const string ProcCodeListSortOrder = "ProcCodeListSortOrder";

		/// <summary>In FormProcCodes, this is the default for the ShowHidden checkbox.</summary>
		public const string ProcCodeListShowHidden = "ProcCodeListShowHidden";

		/// <summary>Users must use suggested auto codes for a procedure.</summary>
		public const string ProcEditRequireAutoCodes = "ProcEditRequireAutoCodes";

		///<summary>Determines if and how we want to update a procedures ProcFee when changing providers.  
		///0 - No prompt, don't change fee (default), 
		///1 - No prompt, always change fee, 
		///2 - Prompt - When patient portion changes, 
		///3 - Prompt - Always
		///</summary>
		public const string ProcFeeUpdatePrompt = "ProcFeeUpdatePrompt";
		public const string ProcLockingIsAllowed = "ProcLockingIsAllowed";
		///<summary>Bool.  Defaults to false.  Custom feature that a customer paid for to merge the current and last procedure note together.
		///The merging of the current and last procedure note will only happen when a concurrency issue has been identified.</summary>
		public const string ProcNoteConcurrencyMerge = "ProcNoteConcurrencyMerge";
		///<summary>True by default.  Allows for substituting AutoNote text for [[text]] segments in a procedure's default note.</summary>
		public const string ProcPromptForAutoNote = "ProcPromptForAutoNote";
		///<summary>If this is on, the claimproc's provider will inherit the provider on the procedure.
		///If this is off AND the claimproc is attached to a claim, then the claimproc's provider and the procedure's provider are saved separately.
		///Most users will keep this on so their providers stay in sync.  
		///Pref created for customers who manually change claimproc Prov so they can have income attributed for specific prov for financial reasons.</summary>
		public const string ProcProvChangesClaimProcWithClaim = "ProcProvChangesClaimProcWithClaim";
		///<summary>Frequency at which signals are processed. Also used by HQ to determine triage label refresh frequency.</summary>		
		public const string ProcessSigsIntervalInSecs = "ProcessSigsIntervalInSecs";
		public const string ProcGroupNoteDoesAggregate = "ProcGroupNoteDoesAggregate";
		///<summary>DateTime.  Next date that the advertising programming properties will automatically check.</summary>		
		public const string ProgramAdditionalFeatures = "ProgramAdditionalFeatures";
		///<summary>Deprecated. Use updatehistory table instead.  Stored the DateTime of when the ProgramVersion preference last changed.</summary>		
		public const string ProgramVersionLastUpdated = "ProgramVersionLastUpdated";
		public const string ProgramVersion = "ProgramVersion";
		///<summary>Boolean, true by default.  When true, checks for "Unsent" or "Hold for Primary" secondary claims and automates the claim's status.</summary>
		public const string PromptForSecondaryClaim = "PromptForSecondaryClaim";
		public const string ProviderIncomeTransferShows = "ProviderIncomeTransferShows";
		///<summary>Bool.  Defaults to true.  When true, allow the Provider Payroll report to select Today's date in the date range.</summary>		
		public const string ProviderPayrollAllowToday = "ProviderPayrollAllowToday";
		///<summary>Was never used.  Was supposed to indicate FK to sheet.Sheet_DEF_Num, so not even named correctly. Must be an exam sheet. Only makes sense if PublicHealthScreeningUsePat is true.</summary>
		public const string PublicHealthScreeningSheet = "PublicHealthScreeningSheet";
		///<summary>Was never used.  Always 0.  Boolean. Work for attaching to patients stopped 11/30/2012, there is currently no access to change the value of this preference.    When in this mode, screenings will be attached to actual PatNums rather than just freeform text names.</summary>
		public const string PublicHealthScreeningUsePat = "PublicHealthScreeningUsePat";
		///<summary>Comma-delimited list of strings.  Each entry represents a QuickBooks "class" which is used to separate deposits (typically by clinics).
		///Empty by default.</summary>
		public const string QuickBooksClassRefs = "QuickBooksClassRefs";
		///<summary>Boolean, off by default.  Some users have clinics enabled but do not want QuickBooks to itemize their accounts.
		///Class Refs are a way for QuickBooks to itemize if set up correctly.</summary>
		public const string QuickBooksClassRefsEnabled = "QuickBooksClassRefsEnabled";
		public const string QuickBooksCompanyFile = "QuickBooksCompanyFile";
		///<summary>Comma-delimited list of strings.  Each entry represents a QuickBooks deposit account.</summary>
		public const string QuickBooksDepositAccounts = "QuickBooksDepositAccounts";
		///<summary>Comma-delimited list of strings.  Each entry represents a QuickBooks income account.</summary>
		public const string QuickBooksIncomeAccount = "QuickBooksIncomeAccount";
		///<summary>Date when user upgraded to or past 15.4.1 and started using ADA procedures to count CPOE radiology orders for EHR.</summary>
		public const string RadiologyDateStartedUsing154 = "RadiologyDateStartedUsing154";
		/// <summary>Boolean.  True if random primary keys have been turned on. There is no interface to change this preference because as of 17.2, all users of
		/// replication must use primary key offset instead of random primary keys.
		///Causes all CRUD classes to look for an unused random PK before inserting instead of leaving it up to auto incrementing.</summary>
		// TODO: [Obsolete]
		public const string RandomPrimaryKeys = "RandomPrimaryKeys";
		///<summary>Integer. Indicated by number of days between contact attempts.</summary>
		public const string ReactivationContactInterval = "ReactivationContactInterval";
		///<summary> long. -1=infinite, 0=zero; if stored as -1, displays as "".</summary>
		public const string ReactivationCountContactMax = "ReactivationCountContactMax";
		///<summary>Defaults to 1095.  -1 indicates min for all dates</summary>
		public const string ReactivationDaysPast = "ReactivationDaysPast";
		public const string ReactivationEmailFamMsg = "ReactivationEmailFamMsg";
		public const string ReactivationEmailMessage = "ReactivationEmailMessage";
		public const string ReactivationEmailSubject = "ReactivationEmailSubject";
		///<summary>Boolean. False by default.</summary>
		public const string ReactivationGroupByFamily = "ReactivationGroupByFamily";
		public const string ReactivationPostcardFamMsg = "ReactivationPostcardFamMsg";
		public const string ReactivationPostcardMessage = "ReactivationPostcardMessage";
		///<summary>Integer.  3 by default.</summary>
		public const string ReactivationPostcardsPerSheet = "ReactivationPostcardsPerSheet";
		///<summary>FK to definition.DefNum. Uses the existing RecallUnschedStatus DefinitionCategory.</summary>
		public const string ReactivationStatusEmailed = "ReactivationStatusEmailed";
		///<summary>FK to definition.DefNum. Uses the existing RecallUnschedStatus DefinitionCategory.</summary>
		public const string ReactivationStatusEmailedTexted = "ReactivationStatusEmailedTexted";
		///<summary>FK to definition.DefNum. Uses the existing RecallUnschedStatus DefinitionCategory.</summary>
		public const string ReactivationStatusMailed = "ReactivationStatusMailed";
		///<summary>FK to definition.DefNum. Uses the existing RecallUnschedStatus DefinitionCategory.</summary>
		public const string ReactivationStatusTexted = "ReactivationStatusTexted";
		public const string RecallAdjustDown = "RecallAdjustDown";
		public const string RecallAdjustRight = "RecallAdjustRight";
		///<summary>Defaults to 12 for new customers.  The number in this field is considered adult.  Only used when automatically adding procedures to a new recall appointment.</summary>
		public const string RecallAgeAdult = "RecallAgeAdult";
		public const string RecallCardsShowReturnAdd = "RecallCardsShowReturnAdd";
		///<summary>-1 indicates min for all dates</summary>
		public const string RecallDaysFuture = "RecallDaysFuture";
		///<summary>-1 indicates min for all dates</summary>
		public const string RecallDaysPast = "RecallDaysPast";
		public const string RecallEmailFamMsg = "RecallEmailFamMsg";
		public const string RecallEmailFamMsg2 = "RecallEmailFamMsg2";
		public const string RecallEmailFamMsg3 = "RecallEmailFamMsg3";
		public const string RecallEmailMessage = "RecallEmailMessage";
		public const string RecallEmailMessage2 = "RecallEmailMessage2";
		public const string RecallEmailMessage3 = "RecallEmailMessage3";
		public const string RecallEmailSubject = "RecallEmailSubject";
		public const string RecallEmailSubject2 = "RecallEmailSubject2";
		public const string RecallEmailSubject3 = "RecallEmailSubject3";
		public const string RecallExcludeIfAnyFutureAppt = "RecallExcludeIfAnyFutureAppt";
		///<summary>Boolean.</summary>
		public const string RecallGroupByFamily = "RecallGroupByFamily";
		///<summary> long. -1=infinite, 0=zero; if stored as -1, displays as "".</summary>
		public const string RecallMaxNumberReminders = "RecallMaxNumberReminders";
		public const string RecallPostcardFamMsg = "RecallPostcardFamMsg";
		public const string RecallPostcardFamMsg2 = "RecallPostcardFamMsg2";
		public const string RecallPostcardFamMsg3 = "RecallPostcardFamMsg3";
		public const string RecallPostcardMessage = "RecallPostcardMessage";
		public const string RecallPostcardMessage2 = "RecallPostcardMessage2";
		public const string RecallPostcardMessage3 = "RecallPostcardMessage3";
		public const string RecallPostcardsPerSheet = "RecallPostcardsPerSheet";
		public const string RecallShowIfDaysFirstReminder = "RecallShowIfDaysFirstReminder";
		public const string RecallShowIfDaysSecondReminder = "RecallShowIfDaysSecondReminder";
		public const string RecallStatusEmailed = "RecallStatusEmailed";
		public const string RecallStatusEmailedTexted = "RecallStatusEmailedTexted";
		public const string RecallStatusMailed = "RecallStatusMailed";
		public const string RecallStatusTexted = "RecallStatusTexted";
		///<summary>Used if younger than 12 on the recall date.</summary>
		public const string RecallTypeSpecialChildProphy = "RecallTypeSpecialChildProphy";
		public const string RecallTypeSpecialPerio = "RecallTypeSpecialPerio";
		public const string RecallTypeSpecialProphy = "RecallTypeSpecialProphy";
		///<summary>Comma-delimited list. FK to recalltype.RecallTypeNum.</summary>
		public const string RecallTypesShowingInList = "RecallTypesShowingInList";
		///<summary>If false, then it will only use email in the recall list if email is the preferred recall method.</summary>
		public const string RecallUseEmailIfHasEmailAddress = "RecallUseEmailIfHasEmailAddress";
		///<summary>Boolean, true by default. Allows recurring charges even when the Patient balance is 0 or less on the account.</summary>
		public const string RecurringChargesAllowedWhenNoPatBal = "RecurringChargesAllowedWhenNoPatBal";
		///<summary>The last time automated recurring charges was run.</summary>
		public const string RecurringChargesAutomatedLastDateTime = "RecurringChargesAutomatedLastDateTime";
		///<summary>Bool, false by default. If true, then OpenDentalService will run recurring charges on a set schedule.</summary>
		public const string RecurringChargesAutomatedEnabled = "RecurringChargesAutomatedEnabled";
		///<summary>Stored as a DateTime, but only the time portion is used. This time will be when recurring charges are automatically run.</summary>
		public const string RecurringChargesAutomatedTime = "RecurringChargesAutomatedTime";
		///<summary>Stored as DateTime, but cleared when recurring charges tool finishes.  The DateTime will be used as a flag to signal other connections
		///that recurring charges have started and prevents OpenDentalService from running repeating charges.</summary>
		public const string RecurringChargesBeginDateTime = "RecurringChargesBeginDateTime";
		///<summary>Stored as a long, defaults to 0. Default recurring payment charge type. When the value is 0, it will use the program property payment
		///type. Does not apply to ACH payments.</summary>
		public const string RecurringChargesPayTypeCC = "RecurringChargesPayTypeCC";
		///<summary>Bool, 0 by default.  When true, recurring charges will use the primary provider of the patient when creating paysplits.
		///When false, the provider that the family is most in debt to will be used.</summary>
		public const string RecurringChargesUsePriProv = "RecurringChargesUsePriProv";
		///<summary>Bool, 0 by default.  When true, uses the transaction date for the recurring charge payment date.
		///When false, the recurring charge date will be used as the recurring charge payment date.</summary>
		public const string RecurringChargesUseTransDate = "RecurringChargesUseTransDate";
		// TODO: [Obsolete]
		public const string RegistrationKey = "RegistrationKey";
		// TODO: [Obsolete]
		public const string RegistrationKeyIsDisabled = "RegistrationKeyIsDisabled";
		public const string RegistrationNumberClaim = "RegistrationNumberClaim";
		public const string RenaissanceLastBatchNumber = "RenaissanceLastBatchNumber";
		///<summary>Bool, false by default.  When true, the repeating charges tool will run automatically on a daily basis.</summary>
		public const string RepeatingChargesAutomated = "RepeatingChargesAutomated";
		///<summary>Time, 08:00 am by default.  Defines the time of day that the repeating charges tool will run if set to run automatically.</summary>
		public const string RepeatingChargesAutomatedTime = "RepeatingChargesAutomatedTime";
		///<summary>Stored as DateTime, but cleared when repeating charges tool finishes.  The DateTime will be used as a flag to signal other connections
		///that repeating charges have started and prevents another connection from running simultaneously. In order to run repeating charges, 
		///this will have to be cleared, either by the connection that set the flag when repeating charges finishes or by an update query.</summary>
		public const string RepeatingChargesBeginDateTime = "RepeatingChargesBeginDateTime";
		///<summary>DateTime, the last date and time repeating charges was run.</summary>
		public const string RepeatingChargesLastDateTime = "RepeatingChargesLastDateTime";
		///<summary>Bool, true by default.  When true, the repeating charges tool will run aging after posting charges.</summary>
		public const string RepeatingChargesRunAging = "RepeatingChargesRunAging";
		///<summary>Bool, 0 by default. When on, the user will be prompted to overwrite existing blockouts when making a new blockout that overlaps the existing ones.</summary>
		public const string ReplaceExistingBlockout = "ReplaceExistingBlockout";
		/// <summary>
		/// If replication has failed, this indicates the server_id. 
		/// No computer will be able to connect to this single server until this flag is cleared.
		/// </summary>
		[Obsolete("Support for replication is being removed. Replication should be handled by the database, not by us.")]
		public const string ReplicationFailureAtServer_id = "ReplicationFailureAtServer_id";
		/// <summary>
		/// The PK of the replication server that is flagged as the "report server". 
		/// If using replication, "create table" or "drop table" commands can only be executed within the user query 
		/// window when connected to this server.
		/// </summary>
		[Obsolete("Support for replication is being removed. Replication should be handled by the database, not by us.")]
		public const string ReplicationUserQueryServer = "ReplicationUserQueryServer";
		public const string ReportFolderName = "ReportFolderName";
		///<summary>When using a distinct reporting server, stores the server name.</summary>
		public const string ReportingServerCompName = "ReportingServerCompName";
		///<summary>When using a distinct reporting server, stores the database name.</summary>
		public const string ReportingServerDbName = "ReportingServerDbName";
		///<summary>When using a distinct reporting server, stores the mysql username.</summary>
		public const string ReportingServerMySqlUser = "ReportingServerMySqlUser";
		///<summary>When using a distinct reporting server, stores the hashed mysql password.</summary>
		public const string ReportingServerMySqlPassHash = "ReportingServerMySqlPassHash";
		///<summary>When using a distinct reporting server over middle tier, stores the uri.</summary>
		public const string ReportingServerURI = "ReportingServerURI";
		///<summary>Boolean, on by default.</summary>
		public const string ReportPandIhasClinicBreakdown = "ReportPandIhasClinicBreakdown";
		///<summary>Boolean, off by default.</summary>
		public const string ReportPandIhasClinicInfo = "ReportPandIhasClinicInfo";
		public const string ReportPandIschedProdSubtractsWO = "ReportPandIschedProdSubtractsWO";
		///<summary>Boolean, false by default. Used to allow a user to display hidden treatment planned prepayments in the payment report.</summary>
		public const string ReportsDoShowHiddenTPPrepayments = "ReportsDoShowHiddenTPPrepayments";
		///<summary>Bool.  False by defualt, used to filter incomplete procedures by having no note in the Incomplete Procedures Report.</summary>
		public const string ReportsIncompleteProcsNoNotes = "ReportsIncompleteProcsNoNotes";
		///<summary>Bool.  False by defualt, used to filter incomplete procedures by having a note that is unsigned in the Incomplete Procedures Report.</summary>
		public const string ReportsIncompleteProcsUnsigned = "ReportsIncompleteProcsUnsigned";
		/// <summary>Tri-state enumeration. 0 by default.  Determines which date is used when calculating certain reports.
		/// 0=Insurance payment date. 1=Procedure Date. 2=Claim Date/Payment Date.</summary>
		public const string ReportsPPOwriteoffDefaultToProcDate = "ReportsPPOwriteoffDefaultToProcDate";
		///<summary>Bool.  False by defualt, used to wrap columns when printing a custom report.</summary>
		public const string ReportsWrapColumns = "ReportsWrapColumns";
		///<summary>Bool.  False by defualt, used to determine whether the reports progress bar will show a history or not.</summary>
		public const string ReportsShowHistory = "ReportsShowHistory";
		public const string ReportsShowPatNum = "ReportsShowPatNum";
		public const string RequiredFieldColor = "RequiredFieldColor";
		///<summary>Tri-state enumeration. 1 by default. 0=Fully Enforced. 1=Auto-split but don't enforce rigorous accounting. 2=Don't auto-split and don't enforce.</summary>
		public const string RigorousAccounting = "RigorousAccounting";
		///<summary>Tri-state enumeration. 1 by default. 0=Fully Enforced. 1=Auto-link but don't enforce. 2=Don't auto-link and don't enforce.</summary>
		public const string RigorousAdjustments = "RigorousAdjustments";
		///<summary>Defaults to false.  When true, will require procedure code to be attached to controlled prescriptions.</summary>
		public const string RxHasProc = "RxHasProc";
		public const string RxSendNewToQueue = "RxSendNewToQueue";
		///<summary>FK to definition.DefNum.  Represents default adjustment types for sales tax adjustments.</summary>
		public const string SalesTaxAdjustmentType = "SalesTaxAdjustmentType";
		public const string SalesTaxPercentage = "SalesTaxPercentage";
		///<summary>Boolean. 1 by default.  Allows users to choose if they want to save a copy of any attachments they send to DXC.</summary>
		public const string SaveDXCAttachments = "SaveDXCAttachments";
		public const string ScannerCompression = "ScannerCompression";
		public const string ScannerResolution = "ScannerResolution";
		public const string ScannerSuppressDialog = "ScannerSuppressDialog";
		///<summary>Set to 1 by default. Selects all providers/employees when loading schedules.</summary>
		public const string ScheduleProvEmpSelectAll = "ScheduleProvEmpSelectAll";
		public const string ScheduleProvUnassigned = "ScheduleProvUnassigned";
		///<summary>Boolean. Off by default so that users will have to opt into utilizing the screening with sheets feature.
		///Screening with sheets is extremely customized for Dental3 (D3)</summary>
		public const string ScreeningsUseSheets = "ScreeningsUseSheets";


		/// <summary>
		/// The ID of the default user group for instructors. Set only for dental schools in dental school setup.
		/// </summary>
		public const string SecurityGroupForInstructors = "SecurityGroupForInstructors";

		/// <summary>
		/// The ID of the default user group for students. Set only for dental schools in dental school setup.
		/// </summary>
		public const string SecurityGroupForStudents = "SecurityGroupForStudents";



		public const string SecurityLockDate = "SecurityLockDate";
		///<summary>Set to 0 to always grant permission. 1 means only today.</summary>
		public const string SecurityLockDays = "SecurityLockDays";
		public const string SecurityLockIncludesAdmin = "SecurityLockIncludesAdmin";
		///<summary>Set to 0 to disable auto logoff.</summary>
		public const string SecurityLogOffAfterMinutes = "SecurityLogOffAfterMinutes";
		///<summary>Boolean.  False by default.  Allows users to set their own automatic logoff times.</summary>
		public const string SecurityLogOffAllowUserOverride = "SecurityLogOffAllowUserOverride";
		public const string SecurityLogOffWithWindows = "SecurityLogOffWithWindows";
		///<summary>Indicates if emails should be sent from a different process when sending through the eConnector. Stored using the YN enum. 0 is
		///unknown, 1 is yes, and 2 is no.</summary>
		public const string SendEmailsInDiffProcess = "SendEmailsInDiffProcess";
		///<summary>Bool.  True by default.  When enabled and user is on support and on the most recent stable or on any beta version a BugSubmissions will be reported to HQ.</summary>
		public const string SendUnhandledExceptionsToHQ = "SendUnhandledExceptionsToHQ";
		///<summary>HQ only. If true, then nobody should connect to the serviceshq database.</summary>
		public const string ServicesHqDoNotConnect = "ServicesHqDoNotConnect";
		///<summary>HQ only. Database name for the serviceshq database.</summary>
		public const string ServicesHqDatabase = "ServicesHqDatabase";
		///<summary>HQ only. Server name for the serviceshq database.</summary>
		public const string ServicesHqServer = "ServicesHqServer";
		///<summary>HQ only. MySQL user for the serviceshq database.</summary>
		public const string ServicesHqMySqlUser = "ServicesHqMySqlUser";
		///<summary>HQ only. Obfuscated password for the serviceshq database.</summary>
		public const string ServicesHqMySqpPasswordObf = "ServicesHqMySqpPasswordObf";
		///<summary>FK to sheetdef.SheetDefNum.  The default chart module layout sheetdef.</summary>
		public const string SheetsDefaultChartModule = "SheetsDefaultChartModule";
		///<summary>The DefNum for the default sheet def to use for Consent sheets</summary>
		public const string SheetsDefaultConsent = "SheetsDefaultConsent";
		///<summary>The DefNum for the default sheet def to use for DepositSlip sheets</summary>
		public const string SheetsDefaultDepositSlip = "SheetsDefaultDepositSlip";
		///<summary>The DefNum for the default sheet def to use for ExamSheet sheets</summary>
		public const string SheetsDefaultExamSheet = "SheetsDefaultExamSheet";
		///<summary>The DefNum for the default sheet def to use for LapSlip sheets</summary>
		public const string SheetsDefaultLabSlip = "SheetsDefaultLabSlip";
		///<summary>The DefNum for the default sheet def to use for LabelAppointment sheets</summary>
		public const string SheetsDefaultLabelAppointment = "SheetsDefaultLabelAppointment";
		///<summary>The DefNum for the default sheet def to use for LabelCarrier sheets</summary>
		public const string SheetsDefaultLabelCarrier = "SheetsDefaultLabelCarrier";
		///<summary>The DefNum for the default sheet def to use for LabelPatient sheets</summary>
		public const string SheetsDefaultLabelPatient = "SheetsDefaultLabelPatient";
		///<summary>The DefNum for the default sheet def to use for LabelReferral sheets</summary>
		public const string SheetsDefaultLabelReferral = "SheetsDefaultLabelReferral";
		///<summary>The DefNum for the default sheet def to use for MedicalHistory sheets</summary>
		public const string SheetsDefaultMedicalHistory = "SheetsDefaultMedicalHistory";
		///<summary>The DefNum for the default sheet def to use for MedLabResults sheets</summary>
		public const string SheetsDefaultMedLabResults = "SheetsDefaultMedLabResults";
		///<summary>The DefNum for the default sheet def to use for PatientForm sheets</summary>
		public const string SheetsDefaultPatientForm = "SheetsDefaultPatientForm";
		///<summary>The DefNum for the default sheet def to use for PatientLetter sheets</summary>
		public const string SheetsDefaultPatientLetter = "SheetsDefaultPatientLetter";
		///<summary>The DefNum for the default sheet def to use for PaymentPlan sheets</summary>
		public const string SheetsDefaultPaymentPlan = "SheetsDefaultPaymentPlan";
		///<summary>The DefNum for the default sheet def to use for ReferralLetter sheets</summary>
		public const string SheetsDefaultReferralLetter = "SheetsDefaultReferralLetter";
		///<summary>The DefNum for the default sheet def to use for ReferralSlip sheets</summary>
		public const string SheetsDefaultReferralSlip = "SheetsDefaultReferralSlip";
		///<summary>The DefNum for the default sheet def to use for RoutingSlip sheets</summary>
		public const string SheetsDefaultRoutingSlip = "SheetsDefaultRoutingSlip";
		///<summary>The DefNum for the default sheet def to use for Rx sheets</summary>
		public const string SheetsDefaultRx = "SheetsDefaultRx";
		///<summary>The DefNum for the default sheet def to use for RxInstruction sheets</summary>
		public const string SheetsDefaultRxInstruction = "SheetsDefaultRxInstruction";
		///<summary>The DefNum for the default sheet def to use for RxMulti sheets</summary>
		public const string SheetsDefaultRxMulti = "SheetsDefaultRxMulti";
		///<summary>The DefNum for the default sheet def to use for Screening sheets</summary>
		public const string SheetsDefaultScreening = "SheetsDefaultScreening";
		///<summary>The DefNum for the default sheet def to use for Statement sheets</summary>
		public const string SheetsDefaultStatement = "SheetsDefaultStatement";
		///<summary>The DefNum for the default sheet def to use for TreatmentPlan sheets</summary>
		public const string SheetsDefaultTreatmentPlan = "SheetsDefaultTreatmentPlan";
		/// <summary>Set to 0 by default.  ClinicPref, indicates which ApptReminderTypes should use Short Codes.</summary>
		public const string ShortCodeApptReminderTypes = "ShortCodeApptReminderTypes";
		///<summary>ClinicPref.  Individual clinics can specify a Clinic Title to use in the Short Code Opt In Reply, substitutes [YourDentist]
		///"You'll now receive appointment messages from [YourDentist] Reply HELP for Help, Reply STOP to cancel. Msg and data rates may apply."</summary>
		public const string ShortCodeOptInClinicTitle = "ShortCodeOptInClinicTitle";
		///<summary>Set to true by default.  Allows the office to turn off the automated Short Code script window when completing an appointment.
		///</summary>
		public const string ShortCodeOptInOnApptComplete = "ShortCodeOptInOnApptComplete";
		///<summary>Set by HQ.  Script occurs when office turns off the Short Code opt in prompt for a clinic/practice.  Informs the user that by turning
		///the prompt off, they agree to still have the opt in conversation with the patient and patients with Unknown status will be set to Yes.
		///</summary>
		public const string ShortCodeOptInOnApptCompleteOffScript = "ShortCodeOptInOnApptCompleteOffScript";
		///<summary>A script for the dentist to read to a patient for opting in to receive appointment reminders via Short Code.</summary>
		public const string ShortCodeOptInScript = "ShortCodeOptInScript";

		/// <summary>
		/// A script for the dentist to read to a patient who has previously opted out of receiving apptointment reminders via Short Code.
		/// </summary>
		public const string ShortCodeOptedOutScript = "ShortCodeOptedOutScript";

		public const string ShowAccountFamilyCommEntries = "ShowAccountFamilyCommEntries";

		/// <summary>
		/// Set to 1 by default.
		/// Prompts user to allocate unearned income after creating a claim.
		/// </summary>
		public const string ShowAllocateUnearnedPaymentPrompt = "ShowAllocateUnearnedPaymentPrompt";

		/// <summary>
		/// Set to 0 by default. 
		/// Preference that controls if the auto deposit group box shows or not in FormClaimPayEdit.cs
		/// </summary>
		public const string ShowAutoDeposit = "ShowAutoDeposit";

		public const string ShowFeatureEhr = "ShowFeatureEhr";
		
		/// <summary>
		/// Set to 0 by default.
		/// Controls if the Enterprise Setup Window will be available.
		/// </summary>
		public const string ShowFeatureEnterprise = "ShowFeatureEnterprise";

		/// <summary>
		/// Set to 1 by default.
		/// Shows a button in Edit Patient Information that lets users launch Google Maps.
		/// </summary>
		public const string ShowFeatureGoogleMaps = "ShowFeatureGoogleMaps";

		public const string ShowFeatureMedicalInsurance = "ShowFeatureMedicalInsurance";

		/// <summary>
		/// Set to 1 to enable the Synch Clone button in the Family module which allows users to create and synch clones of patients.
		/// </summary>
		public const string ShowFeaturePatientClone = "ShowFeaturePatientClone";

		/// <summary>
		/// Set to 1 to enable the Reactivations tab in the Recall list.
		/// </summary>
		public const string ShowFeatureReactivations = "ShowFeatureReactivations";

		public const string ShowFeatureSuperfamilies = "ShowFeatureSuperfamilies";
		
		///<summary>Set to 0 by default.  For enterprise users.  When enabled users can setup Fee Schedule Groups so that whenever a Fee Schedule in a clinic is edited 
		///it will automatically update that fee schedule for the other clinics in that group.</summary>
		public const string ShowFeeSchedGroups = "ShowFeeSchedGroups";
		///<summary>0=None, 1=PatNum, 2=ChartNumber, 3=Birthdate</summary>
		public const string ShowIDinTitleBar = "ShowIDinTitleBar";
		///<summary>Boolean.  True by default. If true then the user might be prompted to create a planned appointment when leaving the Chart Module.</summary>
		public const string ShowPlannedAppointmentPrompt = "ShowPlannedAppointmentPrompt";
		public const string ShowProgressNotesInsteadofCommLog = "ShowProgressNotesInsteadofCommLog";
		///<summary>Deprecated.  Was used to hide the provider payroll report before users had the ability to remove it from the production and income listbox.</summary>
		public const string ShowProviderPayrollReport = "ShowProviderPayrollReport";
		public const string ShowUrgFinNoteInProgressNotes = "ShowUrgFinNoteInProgressNotes";
		///<summary>If enabled, allow Providers to digitally sign procedures and proc notes.</summary>
		public const string SignatureAllowDigital = "SignatureAllowDigital";
		///<summary>Used to stop signals after a period of inactivity.  A value of 0 disables this feature.  Default value of 0 to maintain backward compatibility</summary>
		public const string SignalInactiveMinutes = "SignalInactiveMinutes";
		///<summary>Only used on startup.  The date in which stale signalods were removed.</summary>
		public const string SignalLastClearedDate = "SignalLastClearedDate";
		///<summary>Blank if not signed. Date signed. For practice level contract, if using clinics see Clinic.SmsContractDate. Record of signing also kept at HQ.</summary>
		public const string SmsContractDate = "SmsContractDate";
		///<summary>(Deprecated) Blank if not signed. Name signed. For practice level contract, if using clinics see Clinic.SmsContractName. Record of signing also kept at HQ.</summary>
		public const string SmsContractName = "SmsContractName";
		///<summary>Always stored in US dollars. This is the desired limit for SMS outbound messages per month.</summary>
		public const string SmsMonthlyLimit = "SmsMonthlyLimit";

		/// <summary>Name of this Software.  Defaults to 'Open Dental Software'.</summary>
		public const string SoftwareName = "SoftwareName";


		public const string SolidBlockouts = "SolidBlockouts";
		public const string SpellCheckIsEnabled = "SpellCheckIsEnabled";
		public const string StatementAccountsUseChartNumber = "StatementAccountsUseChartNumber";
		public const string StatementsCalcDueDate = "StatementsCalcDueDate";
		///<summary>Deprecated. Not used anywhere. This preference was only used with non-sheet statements.</summary>
		[Obsolete("This preference has been deprecated. Do not use.", true)]
		public const string StatementShowCreditCard = "StatementShowCreditCard";
		///<summary>Show payment notes.</summary>
		public const string StatementShowNotes = "StatementShowNotes";
		public const string StatementShowAdjNotes = "StatementShowAdjNotes";
		public const string StatementShowProcBreakdown = "StatementShowProcBreakdown";
		public const string StatementShowReturnAddress = "StatementShowReturnAddress";
		///<summary>Deprecated.  Not used anywhere.</summary>
		public const string StatementSummaryShowInsInfo = "StatementSummaryShowInsInfo";
		///<summary>Deprecated.  Not used anywhere. All statements now use sheets.</summary>
		[Obsolete("This preference has been deprecated. Do not use.", true)]
		public const string StatementsUseSheets = "StatementsUseSheets";
		///<summary>Used by OD HQ. Indicates whether WebCamOD applications should be sending their images to the server or not.</summary>
		public const string StopWebCamSnapshot = "StopWebCamSnapshot";
		///<summary>Deprecated. We no longer allow storing of credit card numbers.</summary>
		public const string StoreCCnumbers = "StoreCCnumbers";
		public const string StoreCCtokens = "StoreCCtokens";
		public const string SubscriberAllowChangeAlways = "SubscriberAllowChangeAlways";
		public const string SuperFamSortStrategy = "SuperFamSortStrategy";
		public const string SuperFamNewPatAddIns = "SuperFamNewPatAddIns";
		///<summary>Defaults to true, unless BackupReminderLastDateRun is disabled (more than a decade into the future).
		///When true, supplemental backups will be executed from the eConnector and copied to HQ as a last resort recovery solution.</summary>
		public const string SupplementalBackupEnabled = "SupplementalBackupEnabled";
		///<summary>Defaults to MinVal (0001-01-01 00:00:00).  Last date and time supplemental backup was successful.</summary>
		public const string SupplementalBackupDateLastComplete = "SupplementalBackupDateLastComplete";


		/// <summary>
		/// Blank by default.
		/// Designed to be set to a network path (UNC) to another computer on the network.
		/// A local copy of the supplemental backup file will be placed here as a secondary measure for last resort recovery.
		/// A copy of the supplemental backup will be placed here before uploading to HQ.
		/// This way customers with databases larger than 1GB can make use of our backup system.
		/// </summary>
		public const string SupplementalBackupNetworkPath = "SupplementalBackupNetworkPath";


		public const string TaskAncestorsAllSetInVersion55 = "TaskAncestorsAllSetInVersion55";
		public const string TaskListAlwaysShowsAtBottom = "TaskListAlwaysShowsAtBottom";

		/// <summary>
		/// Deprecated. 
		/// Not used anywhere. 
		/// Previously used for the popup window to show how many new tasks for cur user after login.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string TasksCheckOnStartup = "TasksCheckOnStartup";

		/// <summary>
		/// Determines how Tasks will be filtered from root node positions in UserControlTasks, i.e. when viewing a tab without having selected a TaskList.
		/// </summary>
		public const string TasksGlobalFilterType = "TasksGlobalFilterType";

		/// <summary>
		/// If true use task.Status to determine if task is new. Otherwise use task.IsUnread.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string TasksNewTrackedByUser = "TasksNewTrackedByUser";

		public const string TasksShowOpenTickets = "TasksShowOpenTickets";

		/// <summary>
		/// Boolean. 
		/// 0 by default. 
		/// Sets appointment task lists to use special logic to sort by AptDateTime.
		/// </summary>
		public const string TaskSortApptDateTime = "TaskSortApptDateTime";

		/// <summary>
		/// Boolean. 
		/// Defaults to false to hide repeating tasks feature if no repeating tasks are in use when updating to 16.3.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string TasksUseRepeating = "TasksUseRepeating";

		///<summary>Keeps track of date of one-time cleanup of temp files.  Prevents continued annoying cleanups after the first month.</summary>
		public const string TempFolderDateFirstCleaned = "TempFolderDateFirstCleaned";
		public const string TerminalClosePassword = "TerminalClosePassword";
		///<summary>If true, treat Yes-No-Unknown status of Unknown as if it were a No.</summary>
		public const string TextMsgOkStatusTreatAsNo = "TextMsgOkStatusTreatAsNo";
		public const string TextingDefaultClinicNum = "TextingDefaultClinicNum";
		///<summary>Boolean. True if users are allowed to set their own themes.</summary>
		public const string ThemeSetByUser = "ThemeSetByUser";
		public const string TimeCardADPExportIncludesName = "TimeCardADPExportIncludesName";
		///<summary>0=Sun,1=Mon...6=Sat</summary>
		public const string TimeCardOvertimeFirstDayOfWeek = "TimeCardOvertimeFirstDayOfWeek";
		public const string TimecardSecurityEnabled = "TimecardSecurityEnabled";
		///<summary>Boolean.  0 by default.  When enabled, FormTimeCard and FormTimeCardMange display H:mm:ss instead of HH:mm</summary>
		public const string TimeCardShowSeconds = "TimeCardShowSeconds";
		public const string TimeCardsMakesAdjustmentsForOverBreaks = "TimeCardsMakesAdjustmentsForOverBreaks";
		///<summary>bool</summary>
		public const string TimeCardsUseDecimalInsteadOfColon = "TimeCardsUseDecimalInsteadOfColon";
		///<summary>Users can not edit their own time card for past pay periods.</summary>
		public const string TimecardUsersCantEditPastPayPeriods = "TimecardUsersCantEditPastPayPeriods";
		public const string TimecardUsersDontEditOwnCard = "TimecardUsersDontEditOwnCard";
		///<summary>Boolean, false by default. When enabled, main title of FormOpenDental uses clinic abbr instead of description</summary>
		public const string TitleBarClinicUseAbbr = "TitleBarClinicUseAbbr";
		public const string TitleBarShowSite = "TitleBarShowSite";
		/// <summary>Boolean. 0 by default.  When enabled, Main title bar and Account module patient select grid will display the Patient's 
		/// specialty.</summary>
		public const string TitleBarShowSpecialty = "TitleBarShowSpecialty";
		///<summary>Deprecated.  Not used anywhere.</summary>
		public const string ToothChartMoveMenuToRight = "ToothChartMoveMenuToRight";
		///<summary>Prepayments for TP procedures are non-refundable. When appointment is broken that has one of these payments, payment will go to the 
		///broken appointment procedure instead of remaining on the TP procedures.</summary>
		public const string TpPrePayIsNonRefundable = "TpPrePayIsNonRefundable";
		///<summary>FK to definition.DefNum. The default unearned type that will be used for prepayments attached to tp procedures.</summary>
		public const string TpUnearnedType = "TpUnearnedType";
		///<summary>The date and time when the OpenDentalService last sent update messages for account debits and credits.</summary>
		public const string TransworldDateTimeLastUpdated = "TransworldDateTimeLastUpdated";
		
		/// <summary>
		/// Determines how often account activity is sent to Transworld.
		/// Default is once per day at the time of day set in the
		/// TransworldServiceTimeDue pref.  User can adjust this to be more or less frequent.
		/// </summary>
		public const string TransworldServiceSendFrequency = "TransworldServiceSendFrequency";
		
		/// <summary>
		/// The time of day for the OpenDentalService to update Transoworld (TSI) with all payments
		/// and other debits and credits for families where the guarantor has been sent to TSI for 
		/// collection.
		/// </summary>
		public const string TransworldServiceTimeDue = "TransworldServiceTimeDue";

		/// <summary>
		/// FK to definition.DefNum.
		/// Billing type the OpenDentalService will change guarantors to once an account is paid in full.
		/// </summary>
		public const string TransworldPaidInFullBillingType = "TransworldPaidInFullBillingType";
		
		/// <summary>
		/// Boolean, true by default. 
		/// When enabled, all procedures considered in the treatment finder report will count towards general benefits.
		/// </summary>
		public const string TreatFinderProcsAllGeneral = "TreatFinderProcsAllGeneral";

		public const string TreatmentPlanNote = "TreatmentPlanNote";
		public const string TreatPlanDiscountAdjustmentType = "TreatPlanDiscountAdjustmentType";
		
		/// <summary>
		/// Set to 0 to clear out previous discounts.
		/// </summary>
		public const string TreatPlanDiscountPercent = "TreatPlanDiscountPercent";

		public const string TreatPlanItemized = "TreatPlanItemized";
		public const string TreatPlanPriorityForDeclined = "TreatPlanPriorityForDeclined";
		
		/// <summary>
		/// True by default. Prompt user with name suggestion when saving a treatment plan.
		/// </summary>
		public const string TreatPlanPromptSave = "TreatPlanPromptSave";
		
		/// <summary>
		/// When a TP is signed a PDF will be generated and saved. If disabled, TPs will be redrawn with current data (pre 15.4 behavior).
		/// </summary>
		public const string TreatPlanSaveSignedToPdf = "TreatPlanSaveSignedToPdf";
		public const string TreatPlanShowCompleted = "TreatPlanShowCompleted";

		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string TreatPlanShowGraphics = "TreatPlanShowGraphics";

		public const string TreatPlanShowIns = "TreatPlanShowIns";

		/// <summary>
		/// This preference merely defines what FormOpenDental.IsTreatPlanSortByTooth is on startup.
		/// When true, procedures in the treatment plan module sort by priority, date, toothnum, surface, then PK. 
		/// When false, does not sort by toothnum or surface. True by default to preserve old behavior.
		/// </summary>
		public const string TreatPlanSortByTooth = "TreatPlanSortByTooth";

		/// <summary>
		/// Deprecated. 
		/// All new TPs use sheets as of 17.1. 
		/// Old Printing for classic sheets is handled by ContrTreat.cs bool DoPrintUsingSheets.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.")]
		public const string TreatPlanUseSheets = "TreatPlanUseSheets";



		/// <summary>
		/// DEPRECATED.  Moved to the Trojan Express Collect program link properties.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.", true)]
		public const string TrojanExpressCollectBillingType = "TrojanExpressCollectBillingType";
		/// <summary>
		/// DEPRECATED.  Moved to the Trojan Express Collect program link properties.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.", true)]
		public const string TrojanExpressCollectPassword = "TrojanExpressCollectPassword";
		/// <summary>
		/// DEPRECATED.  Moved to the Trojan Express Collect program link properties.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.", true)]
		public const string TrojanExpressCollectPath = "TrojanExpressCollectPath";
		/// <summary>
		/// DEPRECATED.  Moved to the Trojan Express Collect program link properties.
		/// </summary>
		[Obsolete("This preference has been deprecated. Do not use.", true)]
		public const string TrojanExpressCollectPreviousFileNumber = "TrojanExpressCollectPreviousFileNumber";

		/// <summary>
		/// Can be any int.
		/// Defaults to 0.
		/// </summary>
		public const string UnschedDaysFuture = "UnschedDaysFuture";

		/// <summary>
		/// Can be any int.
		/// Defaults to 365.
		/// </summary>
		public const string UnschedDaysPast = "UnschedDaysPast";

		/// <summary>
		/// Bool, true by default, prevents recall appointments from being sent to the Unscheduled List.
		/// </summary>
		public const string UnscheduledListNoRecalls = "UnscheduledListNoRecalls";

		/// <summary>
		/// Hidden preference, no UI to enable this and is missing in most databases. 
		/// Use GetStringNoCache() to get the value of this preference.
		/// If this preference exists and is set to 1, altering large tables will not use the large table helper method of creating a copy of the table structure, altering the empty copy, and then inserting all of the rows from the original table. 
		/// Instead, the alter table method will be run directly on the large table.
		/// </summary>
		public const string UpdateAlterLargeTablesDirectly = "UpdateAlterLargeTablesDirectly";




		public const string UpdateInProgressOnComputerName = "UpdateInProgressOnComputerName";
		public const string UpdateServerAddress = "UpdateServerAddress";

		[Obsolete] public const string UpdateWebProxyAddress = "UpdateWebProxyAddress";
		[Obsolete] public const string UpdateWebProxyPassword = "UpdateWebProxyPassword";
		[Obsolete] public const string UpdateWebProxyUserName = "UpdateWebProxyUserName";
		[Obsolete] public const string UpdateWebsitePath = "UpdateWebsitePath";
		[Obsolete] public const string UpdateWindowShowsClassicView = "UpdateWindowShowsClassicView";

		/// <summary>
		/// Hidden preference, no UI to enable this feature, but is present in all databases. 
		/// Boolean.
		/// Set to true to make file selection windows use the flag ".ShowHelp=true;".
		/// Added as attemped fix to stop lockups when importing images.
		/// </summary>
		public const string UseAlternateOpenFileDialogWindow = "UseAlternateOpenFileDialogWindow";
		
		public const string UseBillingAddressOnClaims = "UseBillingAddressOnClaims";
		
		/// <summary>
		/// Enum:ToothNumberingNomenclature 0=Universal(American), 1=FDI, 2=Haderup, 3=Palmer
		/// </summary>
		public const string UseInternationalToothNumbers = "UseInternationalToothNumbers";
		
		/// <summary>
		/// Boolean.  0 by default.  
		/// When enabled, users must enter their user name manually at the log on window.
		/// </summary>
		public const string UserNameManualEntry = "UserNameManualEntry";
		
		/// <summary>
		/// Boolean. 0 by default. 
		/// When enabled, chart module procedures that are complete will use the provider's color as row's background color
		/// </summary>
		public const string UseProviderColorsInChart = "UseProviderColorsInChart";



		public const string WaitingRoomAlertColor = "WaitingRoomAlertColor";
		///<summary>0 to disable.  When enabled, sets rows to alert color based on wait time.</summary>
		public const string WaitingRoomAlertTime = "WaitingRoomAlertTime";
		///<summary>Boolean.  0 by default.  When enabled, the waiting room will filter itself by the selected appointment view.  0, normal filtering, will show all patients waiting for the entire practice (or entire clinic when using clinics).</summary>
		public const string WaitingRoomFilterByView = "WaitingRoomFilterByView";
		
		///<summary>Boolean.  1 by default.  Determines whether or not the checkbox in FormWebFormSetup is checked by default.</summary>
		public const string WebFormsAutoFillNameAndBirthdate = "WebFormsAutoFillNameAndBirthdate";
		///<summary>Only used for sheet synch.  See Mobile... for URL for mobile synch.</summary>
		public const string WebHostSynchServerURL = "WebHostSynchServerURL";
		///<summary>The template that will be used for Web Sched automation when a reminder for multiple recalls is sent to the same phone number.
		///</summary>
		public const string WebSchedAggregatedTextMessage = "WebSchedAggregatedTextMessage";
		///<summary>The template that will be used for Web Sched automation when a reminder for multiple recalls is sent to the same email.</summary>
		public const string WebSchedAggregatedEmailBody = "WebSchedAggregatedEmailBody";
		///<summary>The template that will be used for Web Sched automation when a reminder for multiple recalls is sent to the same email.</summary>
		public const string WebSchedAggregatedEmailSubject = "WebSchedAggregatedEmailSubject";
		///<summary>The subject line used for Web Sched ASAP emails.</summary>
		public const string WebSchedAsapEmailSubj = "WebSchedAsapEmailSubj";
		///<summary>The template used for Web Sched ASAP email bodies.</summary>
		public const string WebSchedAsapEmailTemplate = "WebSchedAsapEmailTemplate";
		///<summary>Enum:EmailType 0=Regular 1=Html 2=RawHtml. Used to determine format for email for web sched asap messages</summary>
		public const string WebSchedAsapEmailTemplateType = "WebSchedAsapEmailTemplateType";
		///<summary>Boolean. 0 by default. True when Web Sched ASAP service is enabled.
		///The eConnector keeps this preference current with OD HQ, calling our web service to verify status.</summary>
		public const string WebSchedAsapEnabled = "WebSchedAsapEnabled";
		///<summary>The maximum number of texts allowed to be sent to a patient in a day. Blank means no limit.</summary>
		public const string WebSchedAsapTextLimit = "WebSchedAsapTextLimit";
		///<summary>The template used for Web Sched ASAP texts.</summary>
		public const string WebSchedAsapTextTemplate = "WebSchedAsapTextTemplate";
		///<summary>Stored as an int value from the WebSchedAutomaticSend enum.</summary>
		public const string WebSchedAutomaticSendSetting = "WebSchedAutomaticSendSetting";
		///<summary>Stored as an int value from the WebSchedAutomaticSendText enum.</summary>
		public const string WebSchedAutomaticSendTextSetting = "WebSchedAutomaticSendTextSetting";
		public const string WebSchedMessage = "WebSchedMessage";
		public const string WebSchedMessageText = "WebSchedMessageText";
		public const string WebSchedMessage2 = "WebSchedMessage2";
		public const string WebSchedMessageText2 = "WebSchedMessageText2";
		public const string WebSchedMessage3 = "WebSchedMessage3";
		public const string WebSchedMessageText3 = "WebSchedMessageText3";
		///<summary>The number of text messages sent automatically per batch. Currently one batch runs every 10 minutes.</summary>
		public const string WebSchedTextsPerBatch = "WebSchedTextsPerBatch";
		///<summary>Determines whether or not the birthdate of the Web Sched New Patient gets validated for being 18 years or older.</summary>
		public const string WebSchedNewPatAllowChildren = "WebSchedNewPatAllowChildren";
		///<summary>Boolean, false by default. If true, patients will be able to select their provider for Web Sched New Pat.</summary>
		public const string WebSchedNewPatAllowProvSelection = "WebSchedNewPatAllowProvSelection";
		///<summary>Int, 0 indicates we do not do anything beyond appointment rules in Web Sched New Pat to block double-booking.
		///1 indicates we do not allow double booking at all.  This preference can be enhanced in the future to be an enum.</summary>
		public const string WebSchedNewPatApptDoubleBooking = "WebSchedNewPatApptDoubleBooking";
		///<summary>Deprecated as of 17.1, use signup portal.  Boolean.  0 by default.  True when the New Patient Appointment version of Web Sched is enabled.
		///Loosely keeps track of service status, calling our web service to verify active service is still required.</summary>
		public const string WebSchedNewPatApptEnabled = "WebSchedNewPatApptEnabled";
		///<summary>Boolean.  Defaults to true.  Determines whether or not the phone number field is forced to be formatted (currently only US format XXX-XXX-XXXX)</summary>
		public const string WebSchedNewPatApptForcePhoneFormatting = "WebSchedNewPatApptForcePhoneFormatting";
		///<summary>Comma delimited list.  Empty by default.  Stores the defnums to blockouts to ignore in Web Sched Recall web app.</summary>
		public const string WebSchedNewPatApptIgnoreBlockoutTypes = "WebSchedNewPatApptIgnoreBlockoutTypes";
		///<summary>String.  Is not empty by default.  Stores the message that will show up on the Web Sched New Pat web application.</summary>
		public const string WebSchedNewPatApptMessage = "WebSchedNewPatApptMessage";
		///<summary>Deprecated in v18.1.1 - Utilize appointment types instead.
		///Comma delimited list of procedures that should be put onto the new patient appointment.</summary>
		public const string WebSchedNewPatApptProcs = "WebSchedNewPatApptProcs";
		///<summary>Deprecated in v18.1.1 - Utilize appointment types instead.
		///The time pattern that will be used to determine the length of the new patient appointment.
		///This time pattern is stored as /'s and X's that each represent an amount of time dictated by the current AppointmentTimeIncrement pref.
		///This functionality matches the recall system, not the appointment system (which always stores /'s and X's as 5 mins).</summary>
		public const string WebSchedNewPatApptTimePattern = "WebSchedNewPatApptTimePattern";
		///<summary>Integer.  Represents the number of days into the future we will go before searching for available time slots.
		///Empty will start looking for available time slots today.</summary>
		public const string WebSchedNewPatApptSearchAfterDays = "WebSchedNewPatApptSearchAfterDays";
		///<summary>DefNum for the ApptConfirm status type that will automatically be assigned to Web Sched new patient appointments.</summary>
		public const string WebSchedNewPatConfirmStatus = "WebSchedNewPatConfirmStatus";
		///<summary>Require new patient to respond to 2-step verification email before creating new appointment.</summary>
		public const string WebSchedNewPatDoAuthEmail = "WebSchedNewPatDoAuthEmail";
		///<summary>Require new patient to respond to 2-step verification text message before creating new appointment.</summary>
		public const string WebSchedNewPatDoAuthText = "WebSchedNewPatDoAuthText";
		///<summary>Boolean, default is true.  Used to verify important user information, such as if they are a new patient and how old they are.</summary>
		public const string WebSchedNewPatVerifyInfo = "WebSchedNewPatVerifyInfo";
		/// <summary>String, the webforms url that should be launched after a new patient signs up using web sched.</summary>
		public const string WebSchedNewPatWebFormsURL = "WebSchedNewPatWebFormsURL";
		///<summary>Enum: WebSchedProviderRules 0=FirstAvailable, 1=PrimaryProvider, 2=SecondaryProvider, 3=LastSeenHygienist</summary>
		public const string WebSchedProviderRule = "WebSchedProviderRule";
		///<summary>Int, 0 indicates we do not do anything beyond appointment rules in Web Sched Recall to block double-booking.
		///1 indicates we do not allow double booking at all.  This preference can be enhanced in the future to be an enum.</summary>
		public const string WebSchedRecallDoubleBooking = "WebSchedRecallDoubleBooking";
		///<summary>Boolean, true by default. If true, patients will be able to select their provider for Web Sched Recall.</summary>
		public const string WebSchedRecallAllowProvSelection = "WebSchedRecallAllowProvSelection";
		///<summary>Integer.  Represents the number of days into the future we will go before searching for available time slots.
		///Empty will start looking for available time slots today.</summary>
		public const string WebSchedRecallApptSearchAfterDays = "WebSchedRecallApptSearchAfterDays";
		///<summary>DefNum for the ApptConfirm status type that will automatically be assigned to Web Sched Recall appointments.</summary>
		public const string WebSchedRecallConfirmStatus = "WebSchedRecallConfirmStatus";
		///<summary>Enum:EmailType 0=Regular 1=Html 2=RawHtml. Used to determine format for email for web sched recall messages</summary>
		public const string WebSchedRecallEmailTemplateType = "WebSchedRecallEmailTemplateType";
		///<summary>Enum:EmailType 0=Regular 1=Html 2=RawHtml. Used to determine format for email for web sched recall messages</summary>
		public const string WebSchedRecallEmailTemplateType2 = "WebSchedRecallEmailTemplateType2";
		///<summary>Enum:EmailType 0=Regular 1=Html 2=RawHtml. Used to determine format for email for web sched recall messages</summary>
		public const string WebSchedRecallEmailTemplateType3 = "WebSchedRecallEmailTemplateType3";
		///<summary>Enum:EmailType 0=Regular 1=Html 2=RawHtml. Used to determine format for email for web sched recall messages</summary>
		public const string WebSchedRecallEmailTemplateTypeAgg = "WebSchedRecallEmailTemplateTypeAgg";
		///<summary>Comma delimited list.  Empty by default.  Stores the defnums to blockouts to ignore in Web Sched Recall web app.</summary>
		public const string WebSchedRecallIgnoreBlockoutTypes = "WebSchedRecallIgnoreBlockoutTypes";
		///<summary>In seconds, how often the eConnector thread runs that sends Web Sched notifications. This preference can be updated from OD HQ.
		///</summary>
		public const string WebSchedSendThreadFrequency = "WebSchedSendThreadFrequency";
		///<summary>In seconds, how often the eConnector thread runs that sends Web Sched ASAP messages.</summary>
		public const string WebSchedSendASAPThreadFrequency = "WebSchedSendASAPThreadFrequency";
		///<summary>Boolean. 0 by default. True when Web Sched service is enabled.
		///The eConnector keeps this preference current with OD HQ, calling our web service to verify active service is still required.</summary>
		public const string WebSchedService = "WebSchedService";
		public const string WebSchedSubject = "WebSchedSubject";
		public const string WebSchedSubject2 = "WebSchedSubject2";
		public const string WebSchedSubject3 = "WebSchedSubject3";
		///<summary>String. The e-mail template used for Web Sched verifications to ASAP patients.</summary>
		public const string WebSchedVerifyASAPEmailBody = "WebSchedVerifyASAPEmailBody";
		///<summary>String. The e-mail subject used for Web Sched verifications to ASAP patients.</summary>
		public const string WebSchedVerifyASAPEmailSubj = "WebSchedVerifyASAPEmailSubj";
		///<summary>Enum:EmailType 0=Regular 1=Html 2=RawHtml. Used to determine format for email for web sched notify asap messages</summary>
		public const string WebSchedVerifyAsapEmailTemplateType = "WebSchedVerifyAsapEmailTemplateType";
		///<summary>String. The text template used for Web Sched verifications to ASAP patients.</summary>
		public const string WebSchedVerifyASAPText = "WebSchedVerifyASAPText";
		///<summary>Enum. The communication type for Web Sched verifications to ASAP patients. 0: None, 1: Text, 2: E-mail, 3: Text and E-mail</summary>
		public const string WebSchedVerifyASAPType = "WebSchedVerifyASAPType";
		///<summary>String. The e-mail template used for Web Sched verifications to new patients.</summary>
		public const string WebSchedVerifyNewPatEmailBody = "WebSchedVerifyNewPatEmailBody";
		///<summary>String. The e-mail subject used for Web Sched verifications to new patients.</summary>
		public const string WebSchedVerifyNewPatEmailSubj = "WebSchedVerifyNewPatEmailSubj";
		///<summary>Enum:EmailType 0=Regular 1=Html 2=RawHtml. Used to determine format for email for web sched notify new pat messages</summary>
		public const string WebSchedVerifyNewPatEmailTemplateType = "WebSchedVerifyNewPatEmailTemplateType";
		///<summary>String. The text template used for Web Sched verifications to new patients.</summary>
		public const string WebSchedVerifyNewPatText = "WebSchedVerifyNewPatText";
		///<summary>Enum. The communication type for Web Sched verifications to new patients. 0: None, 1: Text, 2: E-mail, 3: Text and E-mail</summary>
		public const string WebSchedVerifyNewPatType = "WebSchedVerifyNewPatType";
		///<summary>String. The e-mail template used for Web Sched verifications to recalls.</summary>
		public const string WebSchedVerifyRecallEmailBody = "WebSchedVerifyRecallEmailBody";
		///<summary>String. The e-mail subject used for Web Sched verifications to recalls.</summary>
		public const string WebSchedVerifyRecallEmailSubj = "WebSchedVerifyRecallEmailSubj";
		///<summary>Enum:EmailType 0=Regular 1=Html 2=RawHtml. Used to determine format for email for web sched notify recall messages</summary>
		public const string WebSchedVerifyRecallEmailTemplateType = "WebSchedVerifyRecallEmailTemplateType";
		///<summary>String. The text template used for Web Sched verifications to recalls.</summary>
		public const string WebSchedVerifyRecallText = "WebSchedVerifyRecallText";
		///<summary>Enum. The communication type for Web Sched verifications to recalls. 0: None, 1: Text, 2: E-mail, 3: Text and E-mail</summary>
		public const string WebSchedVerifyRecallType = "WebSchedVerifyRecallType";



		[Obsolete] public const string WebServiceServerName = "WebServiceServerName";


		///<summary>If enabled, allows users to right click on ODTextboxes or ODGrids to populate the context menu with any detected wiki links.</summary>
		public const string WikiDetectLinks = "WikiDetectLinks";
		///<summary>If enabled, allows users to create new wiki pages when following links from textboxes and grids. (Disable to prevent proliferation of misspelled wiki pages.)</summary>
		public const string WikiCreatePageFromLink = "WikiCreatePageFromLink";
		public const string WordProcessorPath = "WordProcessorPath";
		public const string XRayExposureLevel = "XRayExposureLevel";
	}
}
