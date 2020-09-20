using System;
using System.ComponentModel;

namespace OpenDentBusiness
{
    /// <summary>
    /// This is an enumeration of all the enumeration types that are used in the database.
    /// This is used in the reporting classes to make the data human-readable.
    /// May need to be updated with recent additions.
    /// </summary>
    public enum EnumType
	{
		YN,
		Relat,
		Month,
		ProcStat,
		DefCat,
		TreatmentArea,
		DentalSpecialty,
		ApptStatus,
		PatientStatus,
		PatientGender,
		PatientPosition,
		ScheduleType,
		LabCase,
		PlaceOfService,
		PaintType,
		SchedStatus,
		AutoCondition,
		ClaimProcStatus,
		CommItemType,
		ToolBarsAvail,
		ProblemStatus,
		EZTwainErrorCode,
		ScaleType,
		SortStrategy,
		ProcCodeListSort
	}

	/// <summary>
	/// 0=Unknown,1=Yes, or 2=No. UI can be tricky.
	/// Some success with a 3x1 listbox, multicolumn, see FormPatientEdit. Radiobuttons are also an option.
	/// You can also use another YN variable to store unknown, and then use a click event on the checkbox to change to Y or N.
	/// You can also use a three state checkbox if you translate properly between the enums.
	/// </summary>
	public enum YN
	{
		Unknown,
		Yes,
		No
	}

	/// <summary>
	/// Relationship to subscriber for insurance.
	/// </summary>
	public enum Relat
	{
		Self,
		Spouse,
		Child,
		Employee,
		HandicapDep,
		SignifOther,
		InjuredPlaintiff,
		LifePartner,
		Dependent
	}

	public enum Month
	{
		Jan = 1,
		Feb,
		Mar,
		Apr,
		May,
		Jun,
		Jul,
		Aug,
		Sep,
		Oct,
		Nov,
		Dec
	}

	/// <summary>
	/// Procedure Status.
	/// These statuses are transalted via class type "enumProcStat" (ex Lan.g("enumProcStat","..."))
	/// </summary>
	public enum ProcStat
	{
		///<summary>1- Treatment Plan.</summary>
		TP = 1,
		///<summary>2- Complete.</summary>
		C,
		///<summary>3- Existing Current Provider.</summary>
		EC,
		///<summary>4- Existing Other Provider.</summary>
		EO,
		///<summary>5- Referred Out.</summary>
		R,
		///<summary>6- Deleted.</summary>
		D,
		///<summary>7- Condition.</summary>
		Cn,
		///<summary>8- Treatment Plan inactive.</summary>
		TPi,
		//See ProcStatExt for pseudo statuses.
	}

	///<summary>The pseudo statuses inside this extended enum must always be mutually exclusive of the values inside the ProcStat enum.
	///These statuses are transalted via class type "enumProcStat" (ex Lan.g("enumProcStat",ProcStatExt.InProcess))</summary>
	public class ProcStatExt
	{
		///<summary>I - Stands for "Invalid".</summary>
		public const string Invalid = "I";
		///<summary>C/P - Stands for "Complete (In Process)".</summary>
		public const string InProcess = "C/P";
	}



	/// <summary>
	/// When the autorefresh message is sent to the other computers, this is the type.
	/// </summary>
	public enum InvalidType
	{
		None,

		/// <summary>
		/// 2 Deprecated.  Inefficient.
		/// All flags combined except Date and Tasks.
		/// </summary>
		AllLocal,

		/// <summary>
		/// 3 Not used with any other flags.
		/// Used to just indicate added tasks, but now it indicates any change at all except those where a popup is needed.
		/// If we also want a popup, then use TaskPopup.
		/// </summary>
		Task,

		ProcCodes,
		Prefs,

		/// <summary>
		/// 6 ApptViews, ApptViewItems, AppointmentRules, ProcApptColors.
		/// </summary>
		Views,

		AutoCodes,
		Carriers,
		ClearHouses,
		Computers,
		InsCats,

		/// <summary>
		/// 12- Also includes payperiods.
		/// </summary>
		Employees,

		Defs,

		/// <summary>
		/// 15. Templates and addresses, but not messages.
		/// </summary>
		Email,


		Letters,

		/// <summary>
		/// 18- Invalidates quick paste notes and cats.
		/// </summary>
		QuickPaste,

		/// <summary>
		/// 19- Userods, UserGroups, UserGroupAttaches, and GroupPermissions
		/// </summary>
		Security,

		/// <summary>
		/// 20 - Also includes program properties.
		/// </summary>
		Programs,

		/// <summary>
		/// 21- Also includes MountDefs
		/// </summary>
		ToolButsAndMounts,

		/// <summary>
		/// 22- Also includes clinics.
		/// </summary>
		Providers,

		/// <summary>
		/// 23- Also includes ClaimFormItems.
		/// </summary>
		ClaimForms,

		ZipCodes,
		LetterMerge,
		DentalSchools,
		Operatories,
		TaskPopup,
		Sites,
		Pharmacies,
		Sheets,
		RecallTypes,
		FeeScheds,
		DisplayFields,
		CustomFields,
		AccountingAutoPays,
		ProcButtons,

		/// <summary>
		/// 40.  Includes ICD9s.
		/// </summary>
		Diseases,

		Languages,
		AutoNotes,
		ElectIDs,
		Employers,
		ProviderIdents,
		ShutDownNow,
		InsFilingCodes,
		ReplicationServers,
		Automation,
		TimeCardRules,

		/// <summary>
		/// 52. Includes DrugManufacturers and DrugUnits.
		/// </summary>
		Vaccines,

		/// <summary>
		/// 53. Includes all 4 HL7Def tables.
		/// </summary>
		HL7Defs,

		DictCustoms,

		/// <summary>
		/// 55. Caches the wiki master page and the wikiListHeaderWidths
		/// </summary>
		Wiki,

		/// <summary>
		/// 56. SourceOfPayment
		/// </summary>
		Sops,

		/// <summary>
		/// 57. In-Memory table used for hard-coded codes and CQMs
		/// </summary>
		EhrCodes,

		/// <summary>
		/// 58. Used to override appointment color.  Might be used for other appointment attributes in the future.
		/// </summary>
		AppointmentTypes,

		/// <summary>
		/// 59. Caches the medication list to stop from over-refreshing and causing slowness.
		/// </summary>
		Medications,

		ProviderErxs,

		/// <summary>
		/// 64. Caches the StateAbbrs used for helping prefill state fields and for state validations.
		/// </summary>
		StateAbbrs,

		RequiredFields,
		Ebills,
		UserClinics,

		/// <summary>
		/// 68. Replaces the deprecated "Date" invalid type for more granularity on invalid signals.
		/// </summary>
		Appointment,

		OrthoChartTabs,

		/// <summary>
		/// 70. A user either acknowledged or added to the messaging buttons system.
		/// </summary>
		SigMessages,

		/// <summary>
		/// 72. THIS IS NOT CACHED. But is used to make server run the alert logic in OpenDentalService.
		/// </summary>
		AlertItems,

		/// <summary>
		/// 74. Used to refresh the active kiosk grid in FormTerminalManager and loaded patient with list of forms in FormTerminal.
		/// </summary>
		Kiosk,

		ClinicPrefs,

		/// <summary>
		/// 76. Not addresses or templates, but inbox and sent messages.
		/// </summary>
		EmailMessages,

		SmsBlockPhones,
		AlertCategories,
		AlertCategoryLinks,

		/// <summary>
		/// 81. Used in updating menu item in report menu.
		/// </summary>
		UnfinalizedPayMenuUpdate,

		/// <summary>
		/// 82. Used for validating clinics for eRx.
		/// </summary>
		ClinicErxs,

		DisplayReports,
		UserQueries,

		/// <summary>
		/// 85. Schedules are not cached, but alerts other workstations if the schedules were changed
		/// </summary>
		Schedules,

		SmsPhones,

		/// <summary>
		/// 90. Used for tracking refreshes on tabs 'for [User]', 'New for [User]', 'Main', 'Reminders'.
		/// </summary>
		TaskList,

		/// <summary>
		/// 91. Used for tracking refreshes on tab 'Open Tasks'.
		/// </summary>
		TaskAuthor,

		/// <summary>
		/// 92. Used for tracking refreshes on tab 'Patient Tasks'.
		/// </summary>
		TaskPatient,

		/// <summary>
		/// 93. Used for refreshing the Referral cache.
		/// </summary>
		Referral,

		/// <summary>
		/// 94. Used for refreshing "In Process" pseudo procedure statuses.
		/// </summary>
		ProcMultiVisits,

		/// <summary>
		/// 95. Used for refreshing the ProviderClinicLink cache.
		/// </summary>
		ProviderClinicLink,

		/// <summary>
		/// 96. Used for refreshing the KioskManager with eClipboard information.
		/// </summary>
		EClipboard,

		/// <summary>
		/// 97. Used for refreshing the TP module for a specific patient.
		/// </summary>
		TPModule,
	}

	///<summary>Appointment status.</summary>
	public enum ApptStatus
	{
		///<summary>0- No appointment should ever have this status.</summary>
		None,
		///<summary>1- Shows as a regularly scheduled appointment.</summary>
		Scheduled,
		///<summary>2- Shows greyed out.</summary>
		Complete,
		///<summary>3- Only shows on unscheduled list.</summary>
		UnschedList,
		///<summary>4- Deprecated in 17.4.1. Use Appointment.Priority instead. </summary>
		ASAP,
		///<summary>5- Shows with a big X on it.</summary>
		Broken,
		///<summary>6- Planned appointment.  Only shows in Chart module. User not allowed to change this status, and it does not display as one of the options.</summary>
		Planned,
		///<summary>7- Patient "post-it" note on the schedule. Shows light yellow. Shows on day scheduled just like appt, as well as in prog notes, etc.</summary>
		PtNote,
		///<summary>8- Patient "post-it" note completed</summary>
		PtNoteCompleted
	}

	///<summary>Known as administrativeGender (HL7 OID of 2.16.840.1.113883.5.1) Male=M, Female=F, Unknown=Undifferentiated=UN.</summary>
	public enum PatientGender
	{//known as administrativeGender HL7 OID of 2.16.840.1.113883.5.1
		///<summary>0</summary>
		Male,
		///<summary>1</summary>
		Female,
		///<summary>2- Required by HIPAA for privacy.  Required by ehr to track missing entries. EHR/HL7 known as undifferentiated (UN).</summary>
		Unknown
	}

	public enum PatientPosition
	{
		Single,
		Married,
		Child,
		Widowed,
		Divorced
	}

	/// <summary>
	/// For schedule timeblocks.
	/// </summary>
	public enum ScheduleType
	{
		Practice,
		Provider,
		Blockout,
		Employee,

		/// <summary>
		/// A slot of time that an ASAP appointment can be moved up to.
		/// </summary>
		WebSchedASAP,
	}

	/// <summary>
	/// For actions taken on blockouts (cut,copy,paste, etc.)
	/// </summary>
	public enum BlockoutAction
	{
		Cut,
		Copy,
		Paste,
		Delete,
		Create,
		Edit,
		Clear
	}

	/// <summary>
	/// Default sort method of the Procedure Code list.
	/// </summary>
	public enum ProcCodeListSort
	{
		Category,
		ProcCode
	}

	/// <summary>
	/// Used in the other appointments window to keep track of the result when closing.
	/// </summary>
	public enum OtherResult
	{
		Cancel,
		CreateNew,
		GoTo,
		CopyToPinBoard,
		NewToPinBoard,

		/// <summary>
		/// Currently only used when scheduling a recall.
		/// Puts it on the pinboard, and then launches a search, jumping to a new date in the process.
		/// </summary>
		PinboardAndSearch
	}

	//public enum PaintType
	//{
	//	Extraction,
	//	FillingSolid,
	//	FillingOutline,
	//	RCT,
	//	Post,
	//	CrownSolid,
	//	CrownOutline,
	//	CrownHatch,
	//	Implant,
	//	Sealant,
	//	PonticSolid,
	//	PonticOutline,
	//	PonticHatch,
	//	RetainerSolid,
	//	RetainerOutline,
	//	RetainerHatch
	//}

	/// <summary>Schedule status.</summary>
	public enum SchedStatus
	{
		Open,
		Closed,
		Holiday
	}



	///<summary>Claimproc Status.  The status must generally be the same as the claim, although it is sometimes not strictly enforced.</summary>
	public enum ClaimProcStatus
	{
		///<summary>0: For claims that have been created or sent, but have not been received.</summary>
		NotReceived,
		///<summary>1: For claims that have been received.</summary>
		Received,
		///<summary>2: For preauthorizations.</summary>
		Preauth,
		///<summary>3: The only place that this status is used is to make adjustments to benefits from the coverage window.  It is never attached to a claim.</summary>
		Adjustment,
		///<summary>4:This differs from Received only slightly.  It's for additional payments on procedures already received.  Most fields are blank.</summary>
		Supplemental,
		///<summary>5: CapClaim is used when you want to send a claim to a capitation insurance company.  These are similar to Supplemental in that there will always be a duplicate claimproc for a procedure. The first claimproc tracks the copay and writeoff, has a status of CapComplete, and is never attached to a claim. The second claimproc has status of CapClaim.</summary>
		CapClaim,
		///<summary>6: Estimates have replaced the fields that were in the procedure table.  Once a procedure is complete, the claimprocstatus will still be Estimate.  An Estimate can be attached to a claim and status gets changed to NotReceived.</summary>
		Estimate,
		///<summary>7: For capitation procedures that are complete.  This replaces the old procedurelog.CapCoPay field. This stores the copay and writeoff amounts.  The copay is only there for reference, while it is the writeoff that actually affects the balance. Never attached to a claim. If procedure is TP, then status will be CapEstimate.  Only set to CapComplete if procedure is Complete.</summary>
		CapComplete,
		///<summary>8: For capitation procedures that are still estimates rather than complete.  When procedure is completed, this can be changed to CapComplete, but never to anything else.</summary>
		CapEstimate,
		///<summary>9: For InsHist procedures.</summary>
		InsHist
	}

	public enum ToolBarsAvail
	{
		AccountModule,
		ApptModule,
		ChartModule,
		ImagesModule,
		FamilyModule,
		TreatmentPlanModule,
		ClaimsSend,

		/// <summary>
		/// Shows in the toolbar at the top that is common to all modules.
		/// </summary>
		MainToolbar,

		/// <summary>
		/// Shows in the main menu Reports submenu.
		/// </summary>
		ReportsMenu,
	}

	public enum TimeClockStatus
	{
		[Description("Home")]
		Home,

		[Description("Lunch")]
		Lunch,

		[Description("Break")]
		Break
	}



	/// <summary>
	/// Deprecated, use patientrace table instead. 
	/// Temporarily used for converting old patient races to patientrace entries and screening. 
	/// Race and ethnicity for patient. Used by public health. 
	/// The problem is that everyone seems to want different choices. 
	/// If we give these choices their own table, then we also need to include mapping functions. 
	/// These are currently used in ArizonaReports, HL7 w ECW, and EHR. 
	/// Foreign users would like their own mappings.
	/// </summary>
	public enum PatientRaceOld
	{
		Unknown,
		Multiracial,
		HispanicLatino,
		AfricanAmerican,
		White,
		HawaiiOrPacIsland,
		AmericanIndian,
		Asian,
		Other,
		Aboriginal,

		/// <summary>Required by EHR.</summary>
		BlackHispanic
	}

	/// <summary>
	/// Grade level used in public health.
	/// </summary>
	public enum PatientGrade
	{
		Unknown,
		First,
		Second,
		Third,
		Fourth,
		Fifth,
		Sixth,
		Seventh,
		Eighth,
		Ninth,
		Tenth,
		Eleventh,
		Twelfth,
		PrenatalWIC,
		PreK,
		Kindergarten,
		Other
	}

	/// <summary>
	/// For public health.  Unknown, NoProblems, NeedsCarE, or Urgent.
	/// </summary>
	public enum TreatmentUrgency
	{

		Unknown,
		NoProblems,
		NeedsCare,
		Urgent
	}

	/// <summary>
	/// The type of image for images module.
	/// </summary>
	public enum ImageType
	{
		/// <summary>
		/// Includes scanned documents and screenshots.
		/// </summary>
		Document,

		Radiograph,
		Photo,

		/// <summary>
		/// For instance a Word document or a spreadsheet. Not an image.
		/// </summary>
		File
	}

	/// <summary>
	/// Used by QuickPasteCat to determine which category to default to when opening.
	/// </summary>
	public enum QuickPasteType
	{
		/// <summary>
		/// None should never be used. 
		/// It is simply used as a "default" when adding a new control. 
		/// Searching for usage of "None" is an easy way to find spots where our pattern was not followed correctly.
		/// </summary>
		None,

		Procedure,
		Appointment,
		CommLog,
		Adjustment,
		Claim,
		Email,
		InsPlan,
		Letter,
		MedicalSummary,
		ServiceNotes,
		MedicalHistory,
		MedicationEdit,
		MedicationPat,
		PatAddressNote,
		Payment,
		PayPlan,
		Query,
		Referral,
		Rx,
		FinancialNotes,
		ChartTreatment,
		MedicalUrgent,
		Statement,
		Recall,
		Popup,
		TxtMsg,
		Task,
		Schedule,
		TreatPlan,
		ClaimCustomTrack,
		AutoNote,
		JobManager,

		/// <summary>
		/// Only to be used if the ReadOnly property is set to true.
		/// </summary>
		ReadOnly,

		Lab,
		Equipment,
		FilePath,
		ContactInfo,
		Office,
		ProgramLink,
		EmployeeStatus,
		WebChat,
	}

	/// <summary>
	/// For every type of electronic claim format that Open Dental can create, there will be an item in this enumeration. 
	/// All e-claim formats are hard coded due to complexity.
	/// </summary>
	public enum ElectronicClaimFormat
	{
		/// <summary>
		/// Not in database, but used in various places in program.
		/// </summary>
		None,

		/// <summary>
		/// The American standard through 12/31/11.
		/// </summary>
		x837D_4010,

		/// <summary>
		/// Proprietary format for Renaissance.
		/// </summary>
		Renaissance,

		/// <summary>
		/// CDAnet format version 4.
		/// </summary>
		Canadian,

		/// <summary>
		/// CSV file adaptable for use in Netherlands.
		/// </summary>
		Dutch,

		/// <summary>
		/// The American standard starting on 1/1/12.
		/// </summary>
		x837D_5010_dental,

		/// <summary>
		/// Either professional or medical.  The distiction is stored at the claim level.
		/// </summary>
		x837_5010_med_inst,

		/// <summary>
		/// A specific Canadian carrier located in Quebec which has their own format.
		/// </summary>
		Ramq,
	}

	/// <summary>
	/// Used when submitting e-claims to some carriers who require extra provider identifiers. 
	/// Usage varies by company. Only used as needed. 
	/// SiteNumber is the only one that is still used on 5010s. 
	/// The other 3 have been deprecated and replaced by NPI.
	/// </summary>
	public enum ProviderSupplementalID
	{
		BlueCross,
		BlueShield,
		SiteNumber,
		CommercialNumber
	}

	/// <summary>
	/// Each clearinghouse can have a hard-coded comm bridge which handles all the communications of transfering the claim files to the clearinghouse/carrier. 
	/// Does not just include X12, but can include any format at all.
	/// </summary>
	public enum EclaimsCommBridge
	{
		/// <summary>
		/// No comm bridge will be activated. The claim files will be created to the specified path, but they will not be uploaded.
		/// </summary>
		None,

		WebMD,
		BCBSGA,
		Renaissance,
		ClaimConnect,
		RECS,
		Inmediata,
		AOS,
		PostnTrack,

		/// <summary>
		/// Canadian clearinghouse.
		/// </summary>
		ITRANS,

		Tesia,
		MercuryDE,
		ClaimX,
		DentiCal,
		EmdeonMedical,

		/// <summary>
		/// Canadian clearinghouse.
		/// </summary>
		Claimstream,

		/// <summary>
		/// UK clearinghouse.
		/// </summary>
		NHS,

		EDS,
		Ramq,
		EdsMedical,
	}

	/// <summary>
	/// Used as the enumeration of FieldValueType.ForeignKey.
	/// Basically, this allows lists to be included in the parameter list.
	/// The lists are those common short lists that are used so frequently.
	/// The user can only select one from the list, and the primary key of that item will be used as the parameter.
	/// </summary>
	public enum ReportFKType
	{
		None,

		/// <summary>
		/// The schoolclass table in the database. Used for dental schools.
		/// </summary>
		SchoolClass,

		/// <summary>
		/// The schoolcourse table in the database. Used for dental schools.
		/// </summary>
		SchoolCourse
	}

	/// <summary>
	/// The type of signal being sent.
	/// </summary>
	public enum SignalType
	{
		/// <summary>
		/// Includes text messages.
		/// </summary>
		Button,

		Invalid
	}

	///<summary>Used in the benefit table.  Corresponds to X12 EB01.</summary>
	public enum InsBenefitType
	{
		///<summary>0- Not usually used.  Would only be used if you are just indicating that the patient is covered, but without any specifics.</summary>
		ActiveCoverage,
		///<summary>1- Used for percentages to indicate portion that insurance will cover.  When interpreting electronic benefit information, this is the opposite percentage, the percentage that the patient will pay after deductible.</summary>
		CoInsurance,
		///<summary>2- The deductible amount.  Might be two entries if, for instance, deductible is waived on preventive.</summary>
		Deductible,
		///<summary>3- A dollar amount.</summary>
		CoPayment,
		///<summary>4- Services that are simply not covered at all.</summary>
		Exclusions,
		///<summary>5- Covers a variety of limitations, including Max, frequency, fee reductions, etc.</summary>
		Limitations,
		///<summary>6- Sets a period of time after the effective date where a benefit will not be used.</summary>
		WaitingPeriod
	}

	///<summary>Used in the benefit table.  Corresponds to X12 EB06.</summary>
	public enum BenefitTimePeriod
	{
		///<summary>0- A timeperiod is frequenly not needed.  For example, percentages.</summary>
		None,
		///<summary>1- The renewal month is not Jan.  In this case, we need to know the effective date so that we know which month the benefits start over in.</summary>
		ServiceYear,
		///<summary>2- Renewal month is Jan.</summary>
		CalendarYear,
		///<summary>3- Usually used for ortho max.</summary>
		Lifetime,
		///<summary>4- Wouldn't be used alone.  Years would again be specified in the quantity field along with a number.</summary>
		Years,
		///<summary>5- # in last 12 months.  Does not care about when benefit year begins. Looks at previous 12 months.</summary>
		NumberInLast12Months,
	}

	///<summary>Used in the benefit table in conjunction with an integer quantity.</summary>
	public enum BenefitQuantity
	{
		///<summary>0- This is used a lot. Most benefits do not need any sort of quantity.</summary>
		None,
		///<summary>1- For example, two exams per year</summary>
		NumberOfServices,
		///<summary>2- For example, 18 when flouride only covered to 18 y.o.</summary>
		AgeLimit,
		///<summary>3- For example, copay per 1 visit.</summary>
		Visits,
		///<summary>4- For example, pano every 5 years.</summary>
		Years,
		///<summary>5- For example, BWs every 6 months.</summary>
		Months
	}

	///<summary>Used in the benefit table.</summary>
	public enum BenefitCoverageLevel
	{
		///<summary>0- Since this is a situational X12 field, we can also have none.  Typical for percentages and copayments.</summary>
		None,
		///<summary>1- The default for deductibles and maximums.</summary>
		Individual,
		///<summary>2- For example, family deductible or family maximum.</summary>
		Family

	}

	/// <summary>
	/// The X12 benefit categories. 
	/// Used to link the user-defined CovCats to the corresponding X12 category.
	/// </summary>
	public enum EbenefitCategory
	{
		///<summary>0- Default.  Applies to all codes.</summary>
		None,
		///<summary>1- X12: 30 and 35. All ADA codes except ortho.  D0000-D7999 and D9000-D9999</summary>
		General,
		///<summary>2- X12: 23. ADA D0000-D0999.  This includes DiagnosticXray.</summary>
		Diagnostic,
		///<summary>3- X12: 24. ADA D4000</summary>
		Periodontics,
		///<summary>4- X12: 25. ADA D2000-D2699, and D2800-D2999.</summary>
		Restorative,
		///<summary>5- X12: 26. ADA D3000</summary>
		Endodontics,
		///<summary>6- X12: 27. ADA D5900-D5999</summary>
		MaxillofacialProsth,
		///<summary>7- X12: 36. Exclusive subcategory of restorative.  D2700-D2799</summary>
		Crowns,
		///<summary>8- X12: 37. ADA range?</summary>
		Accident,
		///<summary>9- X12: 38. ADA D8000-D8999</summary>
		Orthodontics,
		///<summary>10- X12: 39. ADA D5000-D5899 (removable), and D6200-D6899 (fixed)</summary>
		Prosthodontics,
		///<summary>11- X12: 40. ADA D7000</summary>
		OralSurgery,
		///<summary>12- X12: 41. ADA D1000</summary>
		RoutinePreventive,
		///<summary>13- X12: 4. ADA D0200-D0399.  So this is like an optional category which is otherwise considered to be diagnosic.</summary>
		DiagnosticXRay,
		///<summary>14- X12: 28. ADA D9000-D9999</summary>
		Adjunctive
	}

	//public enum ToothPaintingType
	//{
	//	None,
	//	Extraction,
	//	Implant,
	//	RCT,
	//	PostBU,
	//	FillingDark,
	//	FillingLight,
	//	CrownDark,
	//	CrownLight,
	//	BridgeDark,
	//	BridgeLight,
	//	DentureDark,
	//	DentureLight,
	//	Sealant,
	//	Veneer,
	//	Watch
	//}

	public enum ToothInitialType
	{
		Missing,

		/// <summary>
		/// Also hides the number. 
		/// This is now also allowed for primary teeth.
		/// </summary>
		Hidden,

		/// <summary>
		/// Only used with 1-32. "sets" this tooth as a primary tooth. 
		/// The result is that the primary tooth shows in addition to the perm, and that the letter shows in addition to the number.
		/// It also does a Shift0 -12 and some other handy movements.
		/// Even if this is set to true, there can be a separate entry for a missing primary tooth; this would be almost equivalent to not even setting the tooth as primary, but would also allow user to select the letter.
		/// </summary>
		Primary,

		ShiftM,
		ShiftO,
		ShiftB,
		Rotate,
		TipM,
		TipB,

		/// <summary>
		/// One segment of a drawing.
		/// </summary>
		Drawing
	}

	/// <summary>
	/// Indicates at what point the patient is in the sequence.
	/// </summary>
	public enum TerminalStatusEnum
	{
		Standby,
		PatientInfo,
		Medical,

		/// <summary>
		/// Only the patient info tab will be visible. 
		/// This is just to let patient up date their address and phone number.
		/// </summary>
		UpdateOnly
	}

	public enum QuestionType
	{
		FreeformText,
		YesNoUnknown
	}

	public enum SignalElementType
	{
		///<summary>0-To and From lists.  Not tied in any way to the users that are part of security.</summary>
		User,
		///<summary>Typically used to insert "family" before "phone" signals.</summary>
		Extra,
		///<summary>Elements of this type show in the last column and trigger the message to be sent.</summary>
		Message
	}

	public enum InsFilingCodeOldOld
	{
		Commercial_Insurance,
		SelfPay,
		OtherNonFed,
		PPO,
		POS,
		EPO,
		Indemnity,
		HMO_MedicareRisk,
		DMO,
		BCBS,
		Champus,
		Disability,
		FEP,
		HMO,
		LiabilityMedical,
		MedicarePartB,
		Medicaid,
		ManagedCare_NonHMO,
		OtherFederalProgram,
		SelfAdministered,
		Veterans,
		WorkersComp,
		MutuallyDefined
	}

	public enum ContactMethod
	{
		None,
		DoNotCall,
		HmPhone,
		WkPhone,
		WirelessPh,
		Email,
		SeeNotes,
		Mail,
		TextMessage
	}

	public enum ReferralToStatus
	{
		None,
		Declined,
		Scheduled,
		Consulted,
		InTreatment,
		Complete
	}

	public enum StatementMode
	{
		Mail,
		InPerson,
		Email,
		Electronic
	}

	public enum DeletedObjectType
	{
		///<summary>0</summary>
		Appointment,
		///<summary>1 - A schedule object.  Only provider schedules are tracked for deletion.</summary>
		ScheduleProv,
		///<summary>2 - When a recall row is deleted, this records the PatNum for which it was deleted.</summary>
		RecallPatNum,
	}

	public enum SmokingSnoMed
	{
		///<summary>0 - UnknownIfEver</summary>
		_266927001,
		///<summary>1 - SmokerUnknownCurrent</summary>
		_77176002,
		///<summary>2 - NeverSmoked</summary>
		_266919005,
		///<summary>3 - FormerSmoker</summary>
		_8517006,
		///<summary>4 - CurrentSomeDay</summary>
		_428041000124106,
		///<summary>5 - CurrentEveryDay</summary>
		_449868002,
		///<summary>6 - LightSmoker</summary>
		_428061000124105,
		///<summary>7 - HeavySmoker</summary>
		_428071000124103
	}

	///<summary>EZTwain Error Codes. 43 EZTEC_NO_PDF is because of a missing DLL.</summary>
	public enum EZTwainErrorCode
	{
		///<summary>0</summary>
		EZTEC_NONE,
		///<summary>1</summary>
		EZTEC_START_TRIPLET_ERRS,
		///<summary>3</summary>
		EZTEC_CAP_GET,
		///<summary>3</summary>
		EZTEC_CAP_SET,
		///<summary>4</summary>
		EZTEC_DSM_FAILURE,
		///<summary>5</summary>
		EZTEC_DS_FAILURE,
		///<summary>6</summary>
		EZTEC_XFER_FAILURE,
		///<summary>7</summary>
		EZTEC_END_TRIPLET_ERRS,
		///<summary>8</summary>
		EZTEC_OPEN_DSM,
		///<summary>9</summary>
		EZTEC_OPEN_DEFAULT_DS,
		///<summary>10</summary>
		EZTEC_NOT_STATE_4,
		///<summary>11</summary>
		EZTEC_NULL_HCON,
		///<summary>12</summary>
		EZTEC_BAD_HCON,
		///<summary>13</summary>
		EZTEC_BAD_CONTYPE,
		///<summary>14</summary>
		EZTEC_BAD_ITEMTYPE,
		///<summary>15</summary>
		EZTEC_CAP_GET_EMPTY,
		///<summary>16</summary>
		EZTEC_CAP_SET_EMPTY,
		///<summary>17</summary>
		EZTEC_INVALID_HWND,
		///<summary>18</summary>
		EZTEC_PROXY_WINDOW,
		///<summary>19</summary>
		EZTEC_USER_CANCEL,
		///<summary>20</summary>
		EZTEC_RESOLUTION,
		///<summary>21</summary>
		EZTEC_LICENSE,
		///<summary>22</summary>
		EZTEC_JPEG_DLL,
		///<summary>23</summary>
		EZTEC_SOURCE_EXCEPTION,
		///<summary>24</summary>
		EZTEC_LOAD_DSM,
		///<summary>25</summary>
		EZTEC_NO_SUCH_DS,
		///<summary>26</summary>
		EZTEC_OPEN_DS,
		///<summary>27</summary>
		EZTEC_ENABLE_FAILED,
		///<summary>28</summary>
		EZTEC_BAD_MEMXFER,
		///<summary>29</summary>
		EZTEC_JPEG_GRAY_OR_RGB,
		///<summary>30</summary>
		EZTEC_JPEG_BAD_Q,
		///<summary>31</summary>
		EZTEC_BAD_DIB,
		///<summary>32</summary>
		EZTEC_BAD_FILENAME,
		///<summary>33</summary>
		EZTEC_FILE_NOT_FOUND,
		///<summary>34</summary>
		EZTEC_FILE_ACCESS,
		///<summary>35</summary>
		EZTEC_MEMORY,
		///<summary>36</summary>
		EZTEC_JPEG_ERR,
		///<summary>37</summary>
		EZTEC_JPEG_ERR_REPORTED,
		///<summary>38</summary>
		EZTEC_0_PAGES,
		///<summary>39</summary>
		EZTEC_UNK_WRITE_FF,
		///<summary>40</summary>
		EZTEC_NO_TIFF,
		///<summary>41</summary>
		EZTEC_TIFF_ERR,
		///<summary>42</summary>
		EZTEC_PDF_WRITE_ERR,
		///<summary>43</summary>
		EZTEC_NO_PDF,
		///<summary>44</summary>
		EZTEC_GIFCON,
		///<summary>45</summary>
		EZTEC_FILE_READ_ERR,
		///<summary>46</summary>
		EZTEC_BAD_REGION,
		///<summary>47</summary>
		EZTEC_FILE_WRITE,
		///<summary>48</summary>
		EZTEC_NO_DS_OPEN,
		///<summary>49</summary>
		EZTEC_DCXCON,
		///<summary>50</summary>
		EZTEC_NO_BARCODE,
		///<summary>51</summary>
		EZTEC_UNK_READ_FF,
		///<summary>52</summary>
		EZTEC_DIB_FORMAT,
		///<summary>53</summary>
		EZTEC_PRINT_ERR,
		///<summary>54</summary>
		EZTEC_NO_DCX,
		///<summary>55</summary>
		EZTEC_APP_BAD_CON,
		///<summary>56</summary>
		EZTEC_LIC_KEY,
		///<summary>57</summary>
		EZTEC_INVALID_PARAM,
		///<summary>58</summary>
		EZTEC_INTERNAL,
		///<summary>59</summary>
		EZTEC_LOAD_DLL,
		///<summary>60</summary>
		EZTEC_CURL,
		///<summary>61</summary>
		EZTEC_MULTIPAGE_OPEN,
		///<summary>62</summary>
		EZTEC_BAD_SHUTDOWN,
		///<summary>63</summary>
		EZTEC_DLL_VERSION,
		///<summary>64</summary>
		EZTEC_OCR_ERR,
		///<summary>65</summary>
		EZTEC_ONLY_TO_PDF,
		///<summary>66</summary>
		EZTEC_APP_TITLE,
		///<summary>67</summary>
		EZTEC_PATH_CREATE,
		///<summary>68</summary>
		EZTEC_LATE_LIC,
		///<summary>69</summary>
		EZTEC_PDF_PASSWORD,
		///<summary>70</summary>
		EZTEC_PDF_UNSUPPORTED,
		///<summary>71</summary>
		EZTEC_PDF_BAFFLED,
		///<summary>72</summary>
		EZTEC_PDF_INVALID,
		///<summary>73</summary>
		EZTEC_PDF_COMPRESSION,
		///<summary>74</summary>
		EZTEC_NOT_ENOUGH_PAGES,
		///<summary>75</summary>
		EZTEC_DIB_ARRAY_OVERFLOW,
		///<summary>76</summary>
		EZTEC_DEVICE_PAPERJAM,
		///<summary>77</summary>
		EZTEC_DEVICE_DOUBLEFEED,
		///<summary>78</summary>
		EZTEC_DEVICE_COMM,
		///<summary>79</summary>
		EZTEC_DEVICE_INTERLOCK,
		///<summary>80</summary>
		EZTEC_BAD_DOC,
		///<summary>81</summary>
		EZTEC_OTHER_DS_OPEN
	}

	///<summary>Only applies to 15.4 and following.  This defines what and how the eConnector is running for a customer. Do not re-order and do not rename any values.</summary>
	public enum ListenerServiceType
	{
		///<summary>0.  Default for people who had been using the listener prior to the 15.3 proxy listener.</summary>
		ListenerService,
		///<summary>1.  Opt-in required to use the proxy service.</summary>
		ListenerServiceProxy,
		///<summary>2.  Customer is off by HQ's choice. This can only be undone by HQ.</summary>
		DisabledByHQ,
		///<summary>3.  Customer listener is off and awaiting customer consent.</summary>
		NoListener
	}

	/// <summary>
	/// Defines a user-friendly way of describing sorting strategies. 
	/// Intended for user selection for sorting grids. 
	/// Can easily be added to.
	/// </summary>
	public enum SortStrategy
	{
		[Description("Name Asc")]
		NameAsc,

		[Description("Name Desc")]
		NameDesc,

		[Description("PatNum Asc")]
		PatNumAsc,

		[Description("PatNum Desc")]
		PatNumDesc
	}

	/// <summary>
	/// The Enumeration value for which Claim Snapshot Trigger that will be stored.
	/// </summary>
	public enum ClaimSnapshotTrigger
	{
		[Description("Claim Created")]
		ClaimCreate,

		[Description("Service - Specific Time")]
		Service,

		[Description("Insurance Payment Received")]
		InsPayment
	}

	public enum OAuthApplicationNames
	{
		Dropbox,
		Google
	}

	/// <summary>
	/// Only used by the Signup Portal
	/// </summary>
	public enum SignupPortalPermission
	{
		///<summary>This user is denied access to the signup portal.</summary>
		Denied,
		///<summary>The user is only able to sign up any clinic for any eService</summary>
		FullPermission,
		///<summary>The user is only able to see what clinics are signed up for eServices</summary>
		ReadOnly,
		///<summary>The user is only able to sign up their restricted clinic for any eService</summary>
		ClinicOnly,
		///<summary>The user is only able to see what eServices their restricted clinic is signed up for.</summary>
		ClinicOnlyReadOnly,
		///<summary>The user is viewing the Signup Portal from HQ.</summary>
		FromHQ,
		///<summary>Request is coming from the reseller portal.</summary>
		ResellerPortal,
		///<summary>The user is only able to make changes to the SMS Monthly Warning Amount.</summary>
		LimitedSmsOnly,
	}

	public enum ClaimProcCreditsGreaterThanProcFee
	{
		Allow,
		Warn,
		Block,
	}

	/// <summary>
	/// Determines behavior when an appointment is scheduled in an operatory assigned to a clinic
	/// for a patient with a specialty that does not exist in the list of that clinic's specialties.
	/// </summary>
	public enum ApptSchedEnforceSpecialty
	{
		[Description("Don't Enforce")]
		DontEnforce,

		[Description("Warn")]
		Warn,

		[Description("Block")]
		Block,
	}

	/// <summary>
	/// String values that are stored for blockout defintions.
	/// </summary>
	public enum BlockoutType
	{
		/// <summary>
		/// Do not schedule an appointment over this blockout.
		/// </summary>
		[Description("NS")]
		NoSchedule,

		/// <summary>
		/// Do not allow this blockout to be cut or copied and do not allow another blockout to be pasted on this blockout.
		/// </summary>
		[Description("DC")]
		DontCopy,
	}

	/// <summary>
	/// Scheduling priority used by Appointments.
	/// </summary>
	public enum ApptPriority
	{
		/// <summary>Default priority</summary>
		Normal,

		/// <summary>Used to identify items for the ASAP list</summary>
		ASAP
	}

	/// <summary>
	/// Scheduling priority used by Recalls.
	/// </summary>
	public enum RecallPriority
	{
		/// <summary>Default priority</summary>
		Normal,

		/// <summary>Used to identify items for the ASAP list</summary>
		ASAP
	}

	public enum ProcessingMethod
	{
		/// <summary>
		/// PayConnect will use the web service to process payments.
		/// </summary>
		WebService,

		/// <summary>
		/// PayConnect will use the terminal to process payments.
		/// </summary>
		Terminal,
	}

	public enum WebSchedVerifyType
	{
		None,
		Text,
		Email,
		TextAndEmail
	}

	///<summary>Used by the OpenDentalService to determine how often a thread should perform a task.  Example:  For Transworld, account activity is sent
	///to the SFTP server at a user specified frequency. Default once per day.  The user can specify a repeat in Days, Hours, or Minutes.</summary>
	public enum FrequencyUnit
	{
		///<summary>0 - Default frequency is repeating once per day (1 Days).</summary>
		Days,
		///<summary>1</summary>
		Hours,
		///<summary>2</summary>
		Minutes
	}

	///<summary>Used to determine how to handle creating claims with $0 procedures.</summary>
	public enum ClaimZeroDollarProcBehavior
	{
		///<summary>0.  Default value for the ClaimZeroDollarProcBehavior preference.  Allows all procedures to be attached to a claim</summary>
		Allow,
		///<summary>1.  Prompts the user to confirm attaching the $0 procedures to claims.</summary>
		Warn,
		///<summary>2.  Always block users from creating a claim for $0 procedures.</summary>
		Block,
	}

	/// <summary>
	/// Differentiate between transaction types.
	/// </summary>
	public enum AccountEntryType
	{
		/// <summary>adjustment.AdjNum.  Can be a positive (Debit) or negative (Credit) adjustment to the amount owed.</summary>
		Adjustment = 0,

		/// <summary>claimproc.ClaimProcNum. For ins payments and/or writeoffs entered.</summary>
		ClaimPayment,

		/// <summary>paysplit.SplitNum.  Patient payment on an account.</summary>
		Payment,

		/// <summary>procedurelog.ProcNum.  Positive (debit, increases the amount owed).</summary>
		Procedure,

		/// <summary>claim</summary>
		Claim
	}

	[Flags]
	public enum MassEmailStatus
	{
		[Description("NotActivated")]
		NotActivated = 0,

		/// <summary>
		/// The absence of this flag prevents Enabled flag from having any effect.
		/// </summary>
		[Description("Activated")]
		Activated = 1,

		/// <summary>
		/// The absense of this flag is equivalent to Disabled.
		/// </summary>
		[Description("Enabled")]
		Enabled = 2,
	}
}
