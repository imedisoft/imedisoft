namespace OpenDentBusiness
{
    /// <summary>
    /// Storage class that holds all the names of commonly used user preferences.
    /// </summary>
    public static class UserPreferenceName
    {
		public const string Definition = "definition";

		/// <summary>
		/// Stores the ID of the last selected clinic.
		/// </summary>
		public const string ClinicLast = "last_selected_clinic";

		/// <summary>
		/// Wiki home pages use ValueString to store the name of the wiki page instead of Fkey due to how FormWiki loads pages.
		/// </summary>
		public const string WikiHomePage = "wiki_home_page";

		/// <summary>
		/// ValueString will be a comma delimited list of DefNums for the last expanded categories for the user.  When FormAutoNotes loads, these categories will be expanded again.
		/// </summary>
		public const string AutoNoteExpandedCats = "auto_note_expanded_cats";

		/// <summary>
		/// A value indicating whether tasks are collapsed by the default or not.
		/// </summary>
		public const string TaskCollapse = "task_collapse";

		/// <summary>
		/// When FormCommItem is in Persistent mode, clear the note text box after the user creates a commlog.
		/// </summary>
		public const string CommlogPersistClearNote = "commlog_persist_clear_note";

		/// <summary>
		/// When FormCommItem is in Persistent mode, clear the End text box after the user creates a commlog.
		/// </summary>
		public const string CommlogPersistClearEndDate = "commlog_persist_clear_end_date";

		/// <summary>
		/// When FormCommItem is in Persistent mode, update the Date / Time text box with NOW() whenver the patient changes.
		/// </summary>
		public const string CommlogPersistUpdateDateTimeWithNewPatient = "commlog_persist_update_date_time_with_new_patient";

		/// <summary>
		/// Whether or not to display just the currently selected exam in the Perio Chart.
		/// </summary>
		public const string PerioCurrentExamOnly = "perio_current_exam_only";

		/// <summary>
		/// Text message grouping preference. 0 - None; 1 - By Patient;
		/// </summary>
		public const string SmsGroupBy = "sms_group_by";

		/// <summary>
		/// Stores a TaskListNum that the corresponding user wants to block all pop ups from.
		/// </summary>
		public const string TaskListBlock = "task_list_block";

		/// <summary>
		/// Stores user specific values for programs.  Currently only used in DoseSpot for the DoseSpot User ID.
		/// </summary>
		public const string Program = "program";

		public const string SuppressLogOffMessage = "suppress_logoff_messsage";

		/// <summary>
		/// Sets the default state of the Account Module "Show Proc Breakdowns" checkbox.
		/// </summary>
		public const string AcctProcBreakdown = "acct_proc_breakdown";

		/// <summary>
		/// Stores user specific username for programs.
		/// </summary>
		public const string ProgramUserName = "program_user_name";

		/// <summary>
		/// Stores user specific password for programs.
		/// </summary>
		public const string ProgramPassword = "program_password";

		/// <summary>
		/// Stores user specific dashboard to open on load. FKey points to the SheetDef.SheetDefNum that the user last had open.
		/// </summary>
		public const string Dashboard = "dashboard";

		/// <summary>
		/// Stores the Dynamic Chart Layout SheetDef.SheetDefNum selected by a user.
		/// </summary>
		public const string DynamicChartLayout = "dynamic_chart_layout";

		/// <summary>
		/// Whether or not to set the perio auto advance to custom.
		/// </summary>
		public const string PerioAutoAdvanceCustom = "perio_auto_advance_custom";

		/// <summary>
		/// Stores the value (in minutes) of when the user should be auto logged off.
		/// </summary>
		public const string LogOffTimerOverride = "logoff_timer_override";

		/// <summary>
		/// A value indicating whether to check the "Show received" checkbox when loading the supply order history.
		/// </summary>
		public const string ReceivedSupplyOrders = "received_supply_orders";
	}
}
