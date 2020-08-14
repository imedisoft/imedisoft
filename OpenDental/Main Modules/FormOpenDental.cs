/*=============================================================================================================
Open Dental is a dental practice management program.
Copyright 2003-2020  Jordan Sparks, DMD.  http://www.opendental.com

This program is free software; you can redistribute it and/or modify it under the terms of the
GNU Db Public License as published by the Free Software Foundation; either version 2 of the License,
or (at your option) any later version.

This program is distributed in the hope that it will be useful, but without any warranty. See the GNU Db Public License
for more details, available at http://www.opensource.org/licenses/gpl-license.php

Any changes to this program must follow the guidelines of the GPL license if a modified version is to be
redistributed.
===============================================================================================================*/
using CodeBase;
using Imedisoft.Data;
using Imedisoft.Forms;
using Microsoft.Win32;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace OpenDental
{
    public partial class FormOpenDental : ODForm
	{
		#region Fields
		///<summary>This is the singleton instance of the FormOpenDental. This allows us to have S_ methods that are public static and can be called from anywhere in the program to update FormOpenDental.</summary>
		private static FormOpenDental _formOpenDentalS;
		///<summary>When user logs out, this keeps track of where they were for when they log back in.</summary>
		private EnumModuleType LastModule;
		private Bitmap bitmapIcon;
		///<summary>A list of button definitions for this computer.  These button defs display in the lightSignalGrid1 control.</summary>
		private SigButDef[] SigButDefList;
		private bool IsMouseDownOnSplitter;
		private Point PointSplitterOriginalLocation;
		private Point PointOriginalMouse;
		///<summary>This list will only contain events for this computer where the users clicked to disable a popup for a specified period of time.  So it won't typically have many items in it.</summary>
		private List<PopupEvent> PopupEventList;
		///<summary>Command line args passed in when program starts.</summary>
		public string[] CommandLineArgs;
		///<summary>True if there is already a different instance of OD running.  This prevents attempting to start the listener.</summary>
		public bool IsSecondInstance;
		private FormTerminalManager formTerminalManager;
		private Form FormRecentlyOpenForLogoff;
		private FormLogOn FormLogOn_;
		///<summary>When auto log off is in use, we don't want to log off user if they are in the FormLogOn window.  Mostly a problem when using web service because CurUser is not null.</summary>
		private bool IsFormLogOnLastActive;
		private FormCreditRecurringCharges formCreditRecurringCharges;
		private long _previousPatNum;
		private DateTime _datePopupDelay;
		///<summary>A secondary cache only used to determine if preferences related to the redrawing of the Chart module have been changed.</summary>
		private Dictionary<string, object> dictChartPrefsCache = new Dictionary<string, object>();
		///<summary>A secondary cache only used to determine if preferences related to the redrawing of the non-modal task list have been changed.</summary>
		private Dictionary<string, object> dictTaskListPrefsCache = new Dictionary<string, object>();
		///<summary>This is used to determine how Open Dental closed.  If this is set to anything but 0 then some kind of error occurred and Open Dental was forced to close.  Currently only used when updating Open Dental silently.</summary>
		public static int ExitCode = 0;
		///<summary>A specific reference to the "Text" button.  This special reference helps us preserve the notification text on the button after setup is modified.</summary>
		private ODToolBarButton _butText;
		///<summary>A specific reference to the "Task" button. This special reference helps us refresh the notification text on the button after the user changes.</summary>
		private ODToolBarButton _butTask;
		private MenuItem menuItemMoveSubscribers;
		/// <summary>Command line can pass in show=... "Popup", "Popups", "ApptsForPatient", or "SearchPatient".  Stored here as lowercase.</summary>
		private string _StrCmdLineShow = "";
		private FormSmsTextMessaging _formSmsTextMessaging;
		private FormQuery _formUserQuery;
		private OpenDentalGraph.FormDashboardEditTab _formDashboardEditTab;
		///<summary>HQ only. Multiple phone maps can be opened at the same time. This keeps a list of all that are open so we can modify their contents.</summary>
		private static Dictionary<long, Dictionary<long, DateTime>> _dicBlockedAutomations;
		///<summary>Tracks the reminder tasks for the currently logged in user.  Is null until the first signal refresh.  Includes new and viewed tasks.</summary>
		private List<Task> _listReminderTasks = null;
		///<summary>Gets initialized or refreshed when searching for archived task lists to exclude from reminders</summary>
		private Dictionary<long, TaskList> _dictAllTaskLists;
		///<summary>Tracks reminder tasks that were not allowed to popup because we had too many FormTaskEdit windows open already.</summary>
		private List<Task> _listReminderTasksOverLimit = null;
		///<summary>Tracks the normal (non-reminder) tasks for the currently logged in user.  Is null until the first signal refresh.</summary>
		private List<long> _listNormalTaskNums = null;
		///<summary>Tracks the UserNum of the user for which the _listReminderTaskNums and _listOtherTaskNums belong to so we can compensate for different users logging off/on.</summary>
		private long _tasksUserNum = 0;
		///<summary>Task Popups use this upper limit of open FormTaskEdit instances to determine if a task should popup.  More than 115 open FormTaskEdit has been observed to crash the program.  See task #1481164.</summary>
		private static int _popupPressureReliefLimit = 20;//20 is chosen arbitrarily.  We could implement a preference for this, with a max of 115.
		///<summary>The date the appointment module reminders tab was last refreshed.</summary>
		private DateTime _dateReminderRefresh = DateTime.MinValue;
		///<summary>HQ only. Keep track of the last time the office down was checked. Too taxing on the server to perform every 1.6 seconds with the rest of the HQ thread metrics. Will be refreshed on ProcessSigsIntervalInSecs interval.</summary>
		private DateTime _hqOfficeDownLastRefreshed = DateTime.MinValue;
		///<summary>List of AlerReads for the current User.</summary>
		List<AlertRead> _listAlertReads = new List<AlertRead>();
		///<summary>List of AlertItems for the current user and clinic.</summary>
		List<AlertItem> _listAlertItems = new List<AlertItem>();
		private FormXWebTransactions FormXWT;
		private static bool _isTreatPlanSortByTooth;
		///<summary>In most cases, CurPatNum should be used instead of _CurPatNum.</summary>
		private static long _curPatNum;
		private FormLoginFailed _formLoginFailed = null;
		///<summary>We will send a maximum of 1 exception to HQ that occurs when processing signals.</summary>
		private Exception _signalsTickException;
		///<summary>This will be set to true if signal processing has been paused due to inactivity or if the login window is showing when the signal timer ticks.  When signal processing resumes the current Application.ProductVersion will be compared to the db value of PrefName.ProgramVersion and if these two versions don't match, the user will get a message box informing them that OD will have to shutdown and the user will have to relaunch to correct the version mismatch.  We will also check the UpdateInProgressOnComputerName and CorruptedDatabase prefs.</summary>
		private bool _hasSignalProcessingPaused = false;
		///<summary>This is the location of the splitter at 96dpi. That way, it can be reliably and consistently redrawn, regardless of the current dpi.  Either X or Y will be ignored.  For example, if it's docked to the bottom, then only Y will be used.</summary>
		private Point panelSplitterLocation96dpi;
		#endregion Fields

		#region Properties
		///<summary>PatNum for currently loaded patient.</summary>
		[Browsable(false)]
		public static long CurPatNum
		{
			get
			{
				return _curPatNum;
			}
			set
			{
				if (value == _curPatNum)
				{
					return;
				}
				_curPatNum = value;
				PatientChangedEvent.Fire(EventCategory.Patient, value);
			}
		}

		///<summary>Dictionary of AutomationNums mapped to a dictionary of patNums and dateTimes. The dateTime is the time that the given automation for a specific patient should be blocked until. Dictionary removes any entries whos blocked until dateTime is greater than DateTime.Now before returning.  Currently only used when triggered Automation.AutoAction == AutomationAction.PopUp</summary>
		[Browsable(false)]
		public static Dictionary<long, Dictionary<long, DateTime>> DicBlockedAutomations
		{
			get
			{
				if (_dicBlockedAutomations == null)
				{
					_dicBlockedAutomations = new Dictionary<long, Dictionary<long, DateTime>>();
					return _dicBlockedAutomations;
				}
				List<long> listAutoNums = _dicBlockedAutomations.Keys.ToList();
				List<long> listPatNums;
				foreach (long automationNum in listAutoNums)
				{//Key is an AutomationNum
					listPatNums = _dicBlockedAutomations[automationNum].Keys.ToList();
					foreach (long patNum in listPatNums)
					{//Key is a patNum for current AutomationNum key.
						if (_dicBlockedAutomations[automationNum][patNum] > DateTime.Now)
						{//Disable time has not expired yet.
							continue;
						}
						_dicBlockedAutomations[automationNum].Remove(patNum);//Remove automation for current user since block time has expired.
																			 //Since we removed an entry from the lower level dictionary we need to check if there are still entries in the top level dictionary. 
					}
					if (_dicBlockedAutomations[automationNum].Count() == 0)
					{//Top level dictionary no longer contains entries for current automationNum.
						_dicBlockedAutomations.Remove(automationNum);
					}
				}
				return _dicBlockedAutomations;
			}
		}

		///<summary>Inherits value from PrefName.TreatPlanSortByTooth on startup.  The user can change this value without changing the pref from the treatplan module.</summary>
		[Browsable(false)]
		public static bool IsTreatPlanSortByTooth
		{
			get
			{
				return _isTreatPlanSortByTooth;
			}
			set
			{
				_isTreatPlanSortByTooth = value;
				PrefC.IsTreatPlanSortByTooth = value;
			}
		}

		///<summary>List of tab titles for the TabProc control. Used to get accurate preview in sheet layout design view. Returns a list of one item called "Tab" if something goes wrong.</summary>
		[Browsable(false)]
		public static List<string> S_Contr_TabProcPageTitles
		{
			get
			{
				return _formOpenDentalS.ContrChart2.ListTabProcPageTitles;
			}
		}
		#endregion Properties

		#region Constructor

		public FormOpenDental(string[] cla)
		{
			_formOpenDentalS = this;
			Logger.LogInfo("Initializing Open Dental...");
			CommandLineArgs = cla;


			FormSplash formSplash = new FormSplash();
			if (CommandLineArgs.Length == 0)
			{
				formSplash.Show();
			}
			InitializeComponent();

			SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);

			//toolbar		
			ToolBarMain = new ODToolBar();
			ToolBarMain.Location = new Point(51, 0);
			ToolBarMain.Size = new Size(931, 25);
			ToolBarMain.Dock = DockStyle.Top;
			ToolBarMain.ImageList = imageListMain;
			ToolBarMain.ButtonClick += new ODToolBarButtonClickEventHandler(ToolBarMain_ButtonClick);
			this.Controls.Add(ToolBarMain);

			//outlook bar
			moduleBar = new ModuleBar();
			moduleBar.Location = new Point(0, 0);
			moduleBar.Size = new Size(51, 626);
			moduleBar.Dock = DockStyle.Left;
			moduleBar.ButtonClicked += new ButtonClickedEventHandler(myOutlookBar_ButtonClicked);
			this.Controls.Add(moduleBar);

			//MAIN MODULE CONTROLS
			//contrApptJ
			ContrAppt2 = new ContrAppt() { Visible = false };
			ContrAppt2.Dock = DockStyle.Fill;
			splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrAppt2);

			//contrFamily
			ContrFamily2 = new ContrFamily() { Visible = false };
			ContrFamily2.Dock = DockStyle.Fill;
			ContrFamily2.Dock = DockStyle.Fill;
			splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrFamily2);

			//contrFamilyEcw
			ContrFamily2Ecw = new ContrFamilyEcw() { Visible = false };
			ContrFamily2.Dock = DockStyle.Fill;
			splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrFamily2Ecw);

			//contrAccount
			ContrAccount2 = new ContrAccount() { Visible = false };
			ContrAccount2.Dock = DockStyle.Fill;
			splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrAccount2);

			//contrTreat
			ContrTreat2 = new ContrTreat() { Visible = false };
			ContrTreat2.Dock = DockStyle.Fill;
			splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrTreat2);

			//contrChart
			ContrChart2 = new ContrChart() { Visible = false };
			ContrChart2.Dock = DockStyle.Fill;
			splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrChart2);

			//contrImages
			//Moved down to Load because it needs a pref to decide which one to load.
			//contrManage
			ContrManage2 = new ContrStaff() { Visible = false };
			ContrManage2.Dock = DockStyle.Fill;
			splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrManage2);


			userControlPatientDashboard = new UserControlDashboard();
			userControlPatientDashboard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
			userControlPatientDashboard.Size = new Size(splitContainerNoFlickerDashboard.Panel2.Width, splitContainerNoFlickerDashboard.Panel2.Height);
			splitContainerNoFlickerDashboard.Panel2.Controls.Add(userControlPatientDashboard);
			
			
			userControlTasks1 = new UserControlTasks() { Visible = false };
			this.Controls.Add(userControlTasks1);


			panelSplitter.ContextMenu = menuSplitter;
			menuItemDockBottom.Checked = true;

			Logger.LogInfo("Open Dental initialization complete.");

			formSplash.Close();
		}
        #endregion Constructor

        public override string HelpSubject 
		{
			get
			{
				switch (moduleBar.SelectedModule)
				{
					case EnumModuleType.Appointments:
						return nameof(ContrAppt);
					case EnumModuleType.Family:
						return nameof(ContrFamily);
					case EnumModuleType.Account:
						return nameof(ContrAccount);
					case EnumModuleType.TreatPlan:
						return nameof(ContrTreat);
					case EnumModuleType.Chart:
						return nameof(ContrChart);
					case EnumModuleType.Images:
						return nameof(ContrImages);
					case EnumModuleType.Manage:
						return nameof(ContrStaff);
					default:
						return "";
				}
			}
		}

		private void FormOpenDental_Load(object sender, EventArgs e)
		{
			//In order for the "Automatically show the touch keyboard in windowed apps when there's no keyboard attached to your device" Windows setting
			//to work we have to invoke the following line.  Surrounded in a try catch because the user can simply put the OS into tablet mode.
			//Affects WPF RichTextBoxes accross the entire program.
			ODException.SwallowAnyException(() =>
			{
				System.Windows.Automation.AutomationElement.FromHandle(this.Handle); // Just invoking this method wakes up something deep within Windows...
			});

			//Flag the userod cache as NOT allowed to cache any items for security purposes
            TopMost = true;
			Application.DoEvents();
			TopMost = false;
			Activate();
			//This will be increased to 4 hours below but only after the convert script has succeeded.
			DataConnection.ConnectionRetryTimeoutSeconds = (int)TimeSpan.FromMinutes(1).TotalSeconds;
			//Have the auto retry timeout monitor throw an exception after the timeout specified above.
			//If left false, then the application would fall into an infinite wait and we can't afford to have that happen at this point.
			//This will get set to false down below after we register for the DataConnectionLost event which will display the Data Connection Lost window.
			allNeutral();
			string odUser = "";
			string odPassHash = "";
			string webServiceUri = "";
			string odPassword = "";
			string serverName = "";
			string databaseName = "";
			string mySqlUser = "";
			string mySqlPassword = "";
			string mySqlPassHash = "";
			bool useDynamicMode = false;
			if (CommandLineArgs.Length != 0)
			{
				for (int i = 0; i < CommandLineArgs.Length; i++)
				{
					if (CommandLineArgs[i].StartsWith("UserName=") && CommandLineArgs[i].Length > 9)
					{
						odUser = CommandLineArgs[i].Substring(9).Trim('"');
					}
					if (CommandLineArgs[i].StartsWith("PassHash=") && CommandLineArgs[i].Length > 9)
					{
						odPassHash = CommandLineArgs[i].Substring(9).Trim('"');
					}
					if (CommandLineArgs[i].StartsWith("WebServiceUri=") && CommandLineArgs[i].Length > 14)
					{
						webServiceUri = CommandLineArgs[i].Substring(14).Trim('"');
					}
					if (CommandLineArgs[i].StartsWith("OdPassword=") && CommandLineArgs[i].Length > 11)
					{
						odPassword = CommandLineArgs[i].Substring(11).Trim('"');
					}
					if (CommandLineArgs[i].StartsWith("ServerName=") && CommandLineArgs[i].Length > 11)
					{
						serverName = CommandLineArgs[i].Substring(11).Trim('"');
					}
					if (CommandLineArgs[i].StartsWith("DatabaseName=") && CommandLineArgs[i].Length > 13)
					{
						databaseName = CommandLineArgs[i].Substring(13).Trim('"');
					}
					if (CommandLineArgs[i].StartsWith("MySqlUser=") && CommandLineArgs[i].Length > 10)
					{
						mySqlUser = CommandLineArgs[i].Substring(10).Trim('"');
					}
					if (CommandLineArgs[i].StartsWith("MySqlPassword=") && CommandLineArgs[i].Length > 14)
					{
						mySqlPassword = CommandLineArgs[i].Substring(14).Trim('"');
					}
					if (CommandLineArgs[i].StartsWith("MySqlPassHash=") && CommandLineArgs[i].Length > 14)
					{
						mySqlPassHash = CommandLineArgs[i].Substring(14).Trim('"');
					}
					if (CommandLineArgs[i].StartsWith("DynamicMode="))
					{
						useDynamicMode = CommandLineArgs[i].ToLower().Contains("true");
					}
				}
			}

			FormSplash formSplash = new FormSplash();
			FormChooseDatabase formChooseDatabase = new FormChooseDatabase(false);


			CentralConnections.GetChooseDatabaseConnectionSettings(out var connectionString, out var autoConnect);

			bool hasDatabaseConnection = false;
			if (autoConnect)
			{
				try
				{
					CentralConnections.TryToConnect(connectionString);

					hasDatabaseConnection = true;
				}
				catch
				{
				}
			}

			if (!hasDatabaseConnection)
			{
				if (formChooseDatabase.ShowDialog(this) == DialogResult.Cancel)
				{
					Environment.Exit(ExitCode);

					return;
				}

				hasDatabaseConnection = true;
			}


			Cursor = Cursors.WaitCursor;

			#region Theme
			//this section can be fired twice because it's in a loop, but it can't be fired from different threads
			ModuleBar.ActionIconChange = new Action(() =>
			{
					//ODColorTheme.AddOnThemeChanged(() => {
					moduleBar.RefreshButtons();
				LayoutToolBar();
			});
			#endregion Theme

			Plugins.LoadAllPlugins(this);

			formSplash.Show(this);

			if (!PrefsStartup())
			{
				//In Release, refreshes the Pref cache if conversion successful.
				Cursor = Cursors.Default;
				formSplash.Close();

				if (ExitCode == 0)
				{
					//PrefsStartup failed and ExitCode is still 0 which means an unexpected error must have occurred.
					//Set the exit code to 999 which will represent an Unknown Error
					ExitCode = 999;
				}

				Environment.Exit(ExitCode);

				return;
			}

			Logger.MinLogLevel = PrefC.IsVerboseLoggingSession() ? LogLevel.Verbose : Logger.MinLogLevel;
			if (Programs.UsingEcwTightOrFullMode())
			{
				formSplash.Close();
			}

			//Setting the time that we want to wait when the database connection has been lost.
			//We don't want a LostConnection event to fire when updating because of Silent Updating which would fail due to window pop-ups from this event.
			//When the event is triggered a "connection lost" window will display allowing the user to attempt reconnecting to the database
			//and then resume what they were doing.  The purpose of this is to prevent UE's from happening with poor connections or temporary outages.
			DataConnection.ConnectionRetryTimeoutSeconds = (int)TimeSpan.FromHours(4).TotalSeconds;
			DataConnectionEvent.Fired += DataConnection_ConnectionLost;//Hook up the connection lost event. Nothing prior to this point will have LostConnection events fired.

			CredentialsFailedAfterLoginEvent.Fired += DataConnection_CredentialsFailedAfterLogin;
			RefreshLocalData(InvalidType.Prefs);//should only refresh preferences so that SignalLastClearedDate preference can be used in ClearOldSignals()
			Signalods.ClearOldSignals();
			//We no longer do this shotgun approach because it can slow the loading time.
			//RefreshLocalData(InvalidType.AllLocal);
			List<InvalidType> invalidTypes = new List<InvalidType>();
			//invalidTypes.Add(InvalidType.Prefs);//Preferences were refreshed above.  The only preference which might be stale is SignalLastClearedDate, but it is not used anywhere after calling ClearOldSignals() above.
			invalidTypes.Add(InvalidType.Defs);
			invalidTypes.Add(InvalidType.Providers);//obviously heavily used
			invalidTypes.Add(InvalidType.Programs);//already done above, but needs to be done explicitly to trigger the PostCleanup 
			invalidTypes.Add(InvalidType.ToolButsAndMounts);//so program buttons will show in all the toolbars
			if (Programs.UsingEcwTightMode())
			{
				lightSignalGrid1.Visible = false;
			}
			else
			{
				invalidTypes.Add(InvalidType.SigMessages);//so when mouse moves over light buttons, it won't crash
			}
			RefreshLocalData(invalidTypes.ToArray());
			FillSignalButtons();
			ContrManage2.InitializeOnStartup();//so that when a signal is received, it can handle it.
											   //Images module.  The other modules are in constructor because they don't need the pref.
			if (Prefs.GetBool(PrefName.ImagesModuleUsesOld2020, false))
			{
				ContrImages2 = new ContrImages() { Visible = false };
				ContrImages2.Dock = DockStyle.Fill;
				splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrImages2);
			}
			else
			{
				ContrImagesJ2 = new ContrImagesJ() { Visible = false };
				ContrImagesJ2.Dock = DockStyle.Fill;
				splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrImagesJ2);
			}
			//Lan.Refresh();//automatically skips if current culture is en-US
			//LanguageForeigns.Refresh(CultureInfo.CurrentCulture);//automatically skips if current culture is en-US			
			moduleBar.RefreshButtons();
			if (CultureInfo.CurrentCulture.Name == "en-US")
			{
				menuItemTranslation.Visible = false;
			}
			if (!File.Exists("Help.chm"))
			{
				menuItemHelpWindows.Visible = false;
			}
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{//Create A to Z unsupported on Unix for now.
				menuItemCreateAtoZFolders.Visible = false;
			}
			if (!Prefs.GetBool(PrefName.ProcLockingIsAllowed))
			{
				menuItemProcLockTool.Visible = false;
			}
			//Query Monitor does not capture queries from a Middle Tier client, only show Query Monitor menu item when directly connected to the database.
			menuItemQueryMonitor.Visible = true;
			if (Security.IsAuthorized(Permissions.ProcCodeEdit, true) && !Prefs.GetBool(PrefName.ADAdescriptionsReset))
			{
				ProcedureCodes.ResetADAdescriptions();
				Prefs.Set(PrefName.ADAdescriptionsReset, true);
			}
			//Spawn a thread so that attempting to start services on this computer does not hinder the loading time of Open Dental.
			//This is placed before login on pupose so it will run even when the user does not login properly.

			formSplash.Close();
			LogOnOpenDentalUser(odUser, odPassword);

			//If clinics are enabled, we will set the public ClinicNum variable
			//If the user is restricted to a clinic(s), and the computerpref clinic is not one of the user's restricted clinics, the user's clinic will be selected
			//If the user is not restricted, or if the user is restricted but has access to the computerpref clinic, the computerpref clinic will be selected
			//The ClinicNum will determine which view is loaded, either from the computerpref table or from the userodapptview table
			if (PrefC.HasClinicsEnabled && Security.CurrentUser != null)
			{//If block must be run before StartCacheFillForFees() so correct clinic filtration occurs.
				Clinics.LoadClinicNumForUser();
				RefreshMenuClinics();
			}
			BeginODDashboardStarterThread();
			FillSignalButtons();

			string prefImagePath = OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath();
			if (prefImagePath == null || !Directory.Exists(prefImagePath))
			{//AtoZ folder not found
			 //Cache.Refresh(InvalidType.Security);
				FormPath FormP = new FormPath(true);
				FormP.ShowDialog();
				if (FormP.DialogResult != DialogResult.OK)
				{
					MessageBox.Show("Invalid A to Z path.  Closing program.");
					Application.Exit();
				}
			}

			IsTreatPlanSortByTooth = Prefs.GetBool(PrefName.TreatPlanSortByTooth); //not a great place for this, but we don't have a better alternative.
			if (userControlTasks1.Visible)
			{
				userControlTasks1.InitializeOnStartup();
			}
			moduleBar.SelectedIndex = Security.GetModule(0);//for eCW, this fails silently.
			if (Programs.UsingEcwTightOrFullMode()
				|| (HL7Defs.IsExistingHL7Enabled() && !HL7Defs.GetOneDeepEnabled().ShowAppts))
			{
				moduleBar.SelectedModule = EnumModuleType.Chart;
				LayoutControls();
			}
			if (Programs.UsingOrion)
			{
				moduleBar.SelectedModule = EnumModuleType.Family;
			}

			moduleBar.Invalidate();
			LayoutToolBar();
			RefreshMenuReports();
			Cursor = Cursors.Default;
			if (moduleBar.SelectedModule == EnumModuleType.None)
			{
				MessageBox.Show("You do not have permission to use any modules.");
			}

			Bridges.Trojan.StartupCheck();
			FormUAppoint.StartThreadIfEnabled();
			Bridges.ICat.StartFileWatcher();
			Bridges.TigerView.StartFileWatcher();


			// Show a backup reminder once a month...
			if (Prefs.GetDateTime(PrefName.BackupReminderLastDateRun).AddMonths(1) < DateTime.UtcNow)
            {
				using var formBackupReminder = new FormBackupReminder();
				if (formBackupReminder.ShowDialog(this) == DialogResult.OK)
				{
					Prefs.Set(PrefName.BackupReminderLastDateRun, DateTimeOD.Today);
				}
				else
				{
					Application.Exit();

					return;
				}
			}






			FillPatientButton(null);
			ProcessCommandLine(CommandLineArgs);
			ODException.SwallowAnyException(() =>
			{
				Computers.UpdateHeartBeat(Environment.MachineName, true);
			});
			Text = PatientL.GetMainTitle(Patients.GetPat(CurPatNum), Clinics.ClinicNum);
			Security.DateTimeLastActivity = DateTime.Now;
			//Certificate stores for emails need to be created on all computers since any of the computers are able to potentially send encrypted email.
			//If this fails, prrobably a permission issue creating the stores. Nothing we can do except explain in the manual.
			ODException.SwallowAnyException(() =>
			{
				EmailMessages.CreateCertificateStoresIfNeeded();
			});
			Patient pat = Patients.GetPat(CurPatNum);
			if (pat != null && (_StrCmdLineShow == "popup" || _StrCmdLineShow == "popups") && moduleBar.SelectedModule != EnumModuleType.None)
			{
				FormPopupsForFam FormP = new FormPopupsForFam(PopupEventList);
				FormP.PatCur = pat;
				FormP.ShowDialog();
			}
			bool isApptModuleSelected = false;
			if (moduleBar.SelectedModule == EnumModuleType.Appointments)
			{
				isApptModuleSelected = true;
			}
			if (CurPatNum != 0 && _StrCmdLineShow == "apptsforpatient" && isApptModuleSelected)
			{
				ContrAppt2.DisplayOtherDlg(false);
			}
			if (_StrCmdLineShow == "searchpatient")
			{
				FormPatientSelect formPatientSelect = new FormPatientSelect();
				formPatientSelect.ShowDialog();
				if (formPatientSelect.DialogResult == DialogResult.OK)
				{
					CurPatNum = formPatientSelect.SelectedPatientId;
					pat = Patients.GetPat(CurPatNum);
					if (ContrChart2.Visible)
					{
						ContrChart2.ModuleSelectedErx(CurPatNum);
					}
					else
					{
						RefreshCurrentModule();
					}
					FillPatientButton(pat);
				}
			}

			menuItemAccount.MenuItems.Clear();

			if (Prefs.GetString(PrefName.LanguageAndRegion) != CultureInfo.CurrentCulture.Name && !ComputerPrefs.LocalComputer.NoShowLanguage)
			{
				if (MsgBox.Show(MsgBoxButtons.YesNo, "Warning, having mismatched language setting between the workstation and server may cause the program "
					+ "to behave in unexpected ways. Would you like to view the setup window?"))
				{
					FormLanguageAndRegion FormLAR = new FormLanguageAndRegion();
					FormLAR.ShowDialog();
				}
			}
			if (CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits != 2 //We want our users to have their currency decimal setting set to 2.
				&& !ComputerPrefs.LocalComputer.NoShowDecimal)
			{
				FormDecimalSettings FormDS = new FormDecimalSettings();
				FormDS.ShowDialog();
			}
			//Choose a default DirectX format when no DirectX format has been specified and running in DirectX tooth chart mode.
			if (ComputerPrefs.LocalComputer.GraphicsSimple == DrawingMode.DirectX && ComputerPrefs.LocalComputer.DirectXFormat == "")
			{
				try
				{
					ComputerPrefs.LocalComputer.DirectXFormat = FormGraphics.GetPreferredDirectXFormat(this);
					if (ComputerPrefs.LocalComputer.DirectXFormat == "invalid")
					{
						//No valid local DirectX format could be found.
						ComputerPrefs.LocalComputer.GraphicsSimple = DrawingMode.Simple2D;
					}
					ComputerPrefs.Update(ComputerPrefs.LocalComputer);
					//Reinitialize the tooth chart because the graphics mode was probably changed which should change the tooth chart appearence.
					ContrChart2.InitializeOnStartup();
				}
				catch (Exception)
				{
					//The tooth chart will default to Simple2D mode if the above code fails for any reason.  This will at least get the user into the program.
				}
			}
			//Only show enterprise setup if it is enabled
			menuItemEnterprise.Visible = Prefs.GetBool(PrefName.ShowFeatureEnterprise);
			menuItemReactivation.Visible = Prefs.GetBool(PrefName.ShowFeatureReactivations);
			ComputerPrefs.UpdateLocalComputerOS();
			WikiPages.NavPageDelegate = S_WikiLoadPage;
			//We are about to start signal processing for the first time so set the initial refresh timestamp.
			Signalods.SignalLastRefreshed = MiscData.GetNowDateTime();
			Signalods.ApptSignalLastRefreshed = Signalods.SignalLastRefreshed;
			SetTimersAndThreads(true);//Safe to start timers since this method call is on the main thread.
			if (Programs.GetCur(ProgramName.BencoPracticeManagement).Enabled)
			{
				menuItemRemoteSupport.Visible = false;
			}

			menuItemCloudManagement.Visible = false;

			if (CommandLineArgs.Contains("--runLoadSimulation"))
			{
				StartLoadSimulation();
			}
			Plugins.HookAddCode(this, "FormOpenDental.Load_end");
		}

		private bool SetAdvertising(ProgramName progName, XmlDocument doc)
		{
			ProgramProperty property = ProgramProperties.GetForProgram(Programs.GetCur(progName).Id).FirstOrDefault(x => x.Name == "Disable Advertising HQ");
			ProgramProperty propOld = null;
			XmlNode node = doc.SelectSingleNode("//" + progName.ToString());
			if (node == null)
			{
				return false;
			}
			if (property == null)
			{
				property = new ProgramProperty();
				property.Name = "Disable Advertising HQ";
				property.ProgramId = Programs.GetCur(progName).Id;
			}
			else
			{
				propOld = property.Copy();
			}
			//"true" from HQ == 0 for the property value.
			//"false" from HQ == 1 for the property value.
			//This is because the boolean from HQ is whether or not to show the advertisement, whereas in OD the boolean is whether or not to hide the advertisement
			bool isDisabledByHQ = !(node.InnerText.ToLower() == "true");
			property.Value = POut.Bool(isDisabledByHQ);
			if (propOld == null)
			{
				ProgramProperties.Insert(property);
				return true;
			}
			else
			{
				return ProgramProperties.Update(property, propOld);
			}
		}

		/// <summary>
		/// Returns false if it can't complete a conversion, find datapath, or validate registration key.
		/// A silent update will have no UI elements appear. model stores all the info used within the choose database window.
		/// Stores all information entered within the window.
		/// </summary>
		private bool PrefsStartup(bool isSilentUpdate = false)
		{
			try
			{
				Cache.Refresh(InvalidType.Prefs);
			}
			catch (Exception ex)
			{
				if (isSilentUpdate)
				{
					ExitCode = 100;//Database could not be accessed for cache refresh
					Environment.Exit(ExitCode);
					return false;
				}
				MessageBox.Show(ex.Message);
				return false;//shuts program down.
			}

			if (!PrefL.CheckMySqlVersion(isSilentUpdate))
			{
				return false;
			}

			try
			{
				MiscData.SetSqlMode();
			}
			catch
			{
				if (isSilentUpdate)
				{
					ExitCode = 111;//Global SQL mode could not be set
					Environment.Exit(ExitCode);
					return false;
				}
				MessageBox.Show("Unable to set global sql mode.  User probably does not have enough permission.");
				return false;
			}

			string updateComputerName = Prefs.GetString(PrefName.UpdateInProgressOnComputerName);
			if (updateComputerName != "" && Environment.MachineName.ToUpper() != updateComputerName.ToUpper())
			{
				if (isSilentUpdate)
				{
					ExitCode = 120;//Computer trying to access DB during update
					Environment.Exit(ExitCode);
					return false;
				}
				FormUpdateInProgress formUIP = new FormUpdateInProgress(updateComputerName);
				DialogResult result = formUIP.ShowDialog();
				if (result != DialogResult.OK)
				{
					return false;//Either the user canceled out of the window or clicked the override button which 
				}
			}

			if (!isSilentUpdate) PrefL.MySqlVersion55Remind();

			if (!PrefL.CheckProgramVersion(isSilentUpdate))
			{
				return false;
			}



											//menuItemMergeDatabases.Visible=Prefs.GetBool(PrefName.RandomPrimaryKeys");
			return true;
		}

		///<summary>Refreshes certain rarely used data from database.  Must supply the types of data to refresh as flags.  Also performs a few other tasks that must be done when local data is changed.</summary>
		private void RefreshLocalData(params InvalidType[] arrayITypes)
		{
			if (arrayITypes == null || arrayITypes.Length == 0)
			{
				return;//Just in case.
			}
			Cache.Refresh(arrayITypes);
			RefreshLocalDataPostCleanup(arrayITypes);
		}

		///<summary>Performs a few tasks that must be done when local data is changed.</summary>
		private void RefreshLocalDataPostCleanup(params InvalidType[] arrayITypes)
		{//This is where the flickering and reset of windows happens
			bool isAll = arrayITypes.Contains(InvalidType.AllLocal);
			#region IvalidType.Prefs
			if (arrayITypes.Contains(InvalidType.Prefs) || isAll)
			{
				if (Prefs.GetBool(PrefName.EasyHidePublicHealth))
				{
					menuItemSchools.Visible = false;
					menuItemCounties.Visible = false;
					menuItemScreening.Visible = false;
				}
				else
				{
					menuItemSchools.Visible = true;
					menuItemCounties.Visible = true;
					menuItemScreening.Visible = true;
				}
				if (Prefs.GetBool(PrefName.EasyNoClinics))
				{
					menuItemClinics.Visible = false;
					menuClinics.Visible = false;
				}
				else
				{
					menuItemClinics.Visible = true;
					menuClinics.Visible = true;
				}
				//See other solution @3401 for past commented out code.
				moduleBar.RefreshButtons();
				if (Prefs.GetBool(PrefName.EasyHideDentalSchools))
				{
					menuItemSchoolClass.Visible = false;
					menuItemSchoolCourses.Visible = false;
					menuItemDentalSchools.Visible = false;
					menuItemRequirementsNeeded.Visible = false;
					menuItemReqStudents.Visible = false;
					menuItemEvaluations.Visible = false;
				}
				else
				{
					menuItemSchoolClass.Visible = true;
					menuItemSchoolCourses.Visible = true;
					menuItemRequirementsNeeded.Visible = true;
					menuItemReqStudents.Visible = true;
				}
				if (Prefs.GetBool(PrefName.EasyHideRepeatCharges))
				{
					menuItemRepeatingCharges.Visible = false;
				}
				else
				{
					menuItemRepeatingCharges.Visible = true;
				}
				if (PrefC.HasOnlinePaymentEnabled(out ProgramName progNameForPayments))
				{
					menuItemPendingPayments.Visible = true;
					menuItemXWebTrans.Visible = true;
				}
				else
				{
					menuItemPendingPayments.Visible = false;
					menuItemXWebTrans.Visible = false;
				}

				menuItemCustomerManage.Visible = false;
				menuItemNewCropBilling.Visible = false;

				menuFeeSchedGroups.Visible = Prefs.GetBool(PrefName.ShowFeeSchedGroups);
				CheckCustomReports();
				if (NeedsRedraw("ChartModule"))
				{
					ContrChart2.InitializeLocalData();
				}
				if (NeedsRedraw("TaskLists"))
				{
					if (Prefs.GetBool(PrefName.TaskListAlwaysShowsAtBottom))
					{//Refreshing task list here may not be the best course of action.
					 //separate if statement to prevent database call if not showing task list at bottom to begin with
					 //ComputerPref computerPref = ComputerPrefs.GetForLocalComputer();
						if (ComputerPrefs.LocalComputer.TaskKeepListHidden)
						{
							userControlTasks1.Visible = false;
						}
						else if (this.WindowState != FormWindowState.Minimized)
						{//task list show and window is not minimized.
							userControlTasks1.Visible = true;
							userControlTasks1.InitializeOnStartup();
							if (ComputerPrefs.LocalComputer.TaskDock == 0)
							{//bottom
								menuItemDockBottom.Checked = true;
								menuItemDockRight.Checked = false;
								panelSplitter.Cursor = Cursors.HSplit;
								panelSplitter.Height = 7;
								int splitterNewY = Dpi.Scale(this, 540);
								if (ComputerPrefs.LocalComputer.TaskY != 0)
								{
									splitterNewY = ComputerPrefs.LocalComputer.TaskY;
									if (splitterNewY < 300)
									{
										splitterNewY = 300;//keeps it from going too high
									}
									if (splitterNewY > ClientSize.Height - 50)
									{
										splitterNewY = ClientSize.Height - panelSplitter.Height - 50;//keeps it from going off the bottom edge
									}
								}
								panelSplitterLocation96dpi = new Point(0, Dpi.Unscale(this, splitterNewY));
								panelSplitter.Location = new Point(moduleBar.Width, Dpi.Scale(this, panelSplitterLocation96dpi.Y));
							}
							else
							{//right
								menuItemDockRight.Checked = true;
								menuItemDockBottom.Checked = false;
								panelSplitter.Cursor = Cursors.VSplit;
								panelSplitter.Width = 7;
								int splitterNewX = Dpi.Scale(this, 900);
								if (ComputerPrefs.LocalComputer.TaskX != 0)
								{
									splitterNewX = ComputerPrefs.LocalComputer.TaskX;
									if (splitterNewX < 300)
									{
										splitterNewX = 300;//keeps it from going too far to the left
									}
									if (splitterNewX > ClientSize.Width - 60)
									{
										splitterNewX = ClientSize.Width - panelSplitter.Width - 60;//keeps it from going off the right edge
									}
								}
								panelSplitterLocation96dpi = new Point(Dpi.Unscale(this, splitterNewX), 0);
								panelSplitter.Location = new Point(Dpi.Scale(this, panelSplitterLocation96dpi.X), ToolBarMain.Height);
							}
						}
					}
					else
					{
						userControlTasks1.Visible = false;
					}
				}
				LayoutControls();
			}
			else if (arrayITypes.Contains(InvalidType.Sheets) && userControlPatientDashboard.IsInitialized)
			{
				LayoutControls();//The current dashboard may have changed.
				userControlPatientDashboard.RefreshDashboard();
				ResizeDashboard();
				RefreshMenuDashboards();
			}
			else if (arrayITypes.Contains(InvalidType.Security) || isAll)
			{
				RefreshMenuDashboards();
			}
			#endregion
			#region InvalidType.Signals
			if (arrayITypes.Contains(InvalidType.SigMessages) || isAll)
			{
				FillSignalButtons();
			}
			#endregion
			#region InvalidType.Programs
			if (arrayITypes.Contains(InvalidType.Programs) || isAll)
			{
				if (Programs.GetCur(ProgramName.PT).Enabled)
				{
					Bridges.PaperlessTechnology.InitializeFileWatcher();
				}
			}
			#endregion
			#region InvalidType.Programs OR InvalidType.Prefs
			if (arrayITypes.Contains(InvalidType.Programs) || arrayITypes.Contains(InvalidType.Prefs) || isAll)
			{
				if (Prefs.GetBool(PrefName.EasyBasicModules))
				{
					moduleBar.SetVisible(EnumModuleType.TreatPlan, false);
					moduleBar.SetVisible(EnumModuleType.Images, false);
					moduleBar.SetVisible(EnumModuleType.Manage, false);
				}
				else
				{
					moduleBar.SetVisible(EnumModuleType.TreatPlan, true);
					moduleBar.SetVisible(EnumModuleType.Images, true);
					moduleBar.SetVisible(EnumModuleType.Manage, true);
				}
				if (Programs.UsingEcwTightOrFullMode())
				{//has nothing to do with HL7
					if (ProgramProperties.GetPropVal(ProgramName.eClinicalWorks, "ShowImagesModule") == "1")
					{
						moduleBar.SetVisible(EnumModuleType.Images, true);
					}
					else
					{
						moduleBar.SetVisible(EnumModuleType.Images, false);
					}
				}
				if (Programs.UsingEcwTightMode())
				{//has nothing to do with HL7
					moduleBar.SetVisible(EnumModuleType.Manage, false);
				}
				if (Programs.UsingEcwTightOrFullMode())
				{//old eCW interfaces
					if (Programs.UsingEcwTightMode())
					{
						moduleBar.SetVisible(EnumModuleType.Appointments, false);
						moduleBar.SetVisible(EnumModuleType.Account, false);
					}
					else if (Programs.UsingEcwFullMode())
					{
						//We might create a special Appt module for eCW full users so they can access Recall.
						moduleBar.SetVisible(EnumModuleType.Appointments, false);
					}
				}
				else if (HL7Defs.IsExistingHL7Enabled())
				{//There may be a def enabled as well as the old program link enabled. In this case, do not look at the def for whether or not to show the appt and account modules, instead go by the eCW interface enabled.
					HL7Def hl7Def = HL7Defs.GetOneDeepEnabled();
					moduleBar.SetVisible(EnumModuleType.Appointments, hl7Def.ShowAppts);
					moduleBar.SetVisible(EnumModuleType.Account, hl7Def.ShowAccount);
				}
				else
				{//no def and not using eCW tight or full program link
					moduleBar.SetVisible(EnumModuleType.Appointments, true);
					moduleBar.SetVisible(EnumModuleType.Account, true);
				}
				if (Programs.UsingOrion)
				{
					moduleBar.SetVisible(EnumModuleType.Appointments, false);
					moduleBar.SetVisible(EnumModuleType.Account, false);
					moduleBar.SetVisible(EnumModuleType.TreatPlan, false);
				}
				moduleBar.Invalidate();
			}
			#endregion
			#region InvalidType.ToolButsAndMounts
			if (arrayITypes.Contains(InvalidType.ToolButsAndMounts) || isAll)
			{
				ContrAccount2.LayoutToolBar();
				ContrAppt2.LayoutToolBar();
				if (ContrChart2.Visible)
				{
					//When the invalidated (running DBM) if we just layout the tool bar the buttons would be enabled, need to consider if no patient is selected.
					//The following line calls LayoutToolBar() and then does the toolbar enable/disable logic.
					ContrChart2.RefreshModuleScreen(false);//false because module is already selected.
				}
				else
				{
					ContrChart2.LayoutToolBar();
				}
				if (Prefs.GetBool(PrefName.ImagesModuleUsesOld2020, false))
				{
					if (ContrImages2 != null)
					{//can be null on startup
						ContrImages2.LayoutToolBar();
					}
				}
				else
				{
					if (ContrImagesJ2 != null)
					{
						ContrImagesJ2.LayoutToolBars();
					}
				}
				ContrFamily2.LayoutToolBar();
				LayoutToolBar();//Ensures the main toolbar refreshes with the rest.
			}
			#endregion
			#region InvalidType.Views
			if (arrayITypes.Contains(InvalidType.Views) || isAll)
			{
				ContrAppt2.FillViews();
			}
			#endregion
			//TODO: If there are still issues with TP refreshing, include TP prefs in needsRedraw()
			ContrTreat2.InitializeLocalData();//easier to leave this here for now than to split it.
			dictChartPrefsCache.Clear();
			dictTaskListPrefsCache.Clear();
			//Chart Drawing Prefs
			dictChartPrefsCache.Add(PrefName.UseInternationalToothNumbers.ToString(), PrefC.GetInt(PrefName.UseInternationalToothNumbers));
			dictChartPrefsCache.Add("GraphicsUseHardware", ComputerPrefs.LocalComputer.GraphicsUseHardware);
			dictChartPrefsCache.Add("PreferredPixelFormatNum", ComputerPrefs.LocalComputer.PreferredPixelFormatNum);
			dictChartPrefsCache.Add("GraphicsSimple", ComputerPrefs.LocalComputer.GraphicsSimple);
			dictChartPrefsCache.Add(PrefName.ShowFeatureEhr.ToString(), Prefs.GetBool(PrefName.ShowFeatureEhr));
			dictChartPrefsCache.Add("DirectXFormat", ComputerPrefs.LocalComputer.DirectXFormat);
			//Task list drawing prefs
			dictTaskListPrefsCache.Add("TaskDock", ComputerPrefs.LocalComputer.TaskDock);
			dictTaskListPrefsCache.Add("TaskY", ComputerPrefs.LocalComputer.TaskY);
			dictTaskListPrefsCache.Add("TaskX", ComputerPrefs.LocalComputer.TaskX);
			dictTaskListPrefsCache.Add(PrefName.TaskListAlwaysShowsAtBottom.ToString(), Prefs.GetBool(PrefName.TaskListAlwaysShowsAtBottom));
			dictTaskListPrefsCache.Add(PrefName.TasksUseRepeating.ToString(), Prefs.GetBool(PrefName.TasksUseRepeating));
			dictTaskListPrefsCache.Add(PrefName.TasksNewTrackedByUser.ToString(), Prefs.GetBool(PrefName.TasksNewTrackedByUser));
			dictTaskListPrefsCache.Add(PrefName.TasksShowOpenTickets.ToString(), Prefs.GetBool(PrefName.TasksShowOpenTickets));
			dictTaskListPrefsCache.Add("TaskKeepListHidden", ComputerPrefs.LocalComputer.TaskKeepListHidden);
			if (Security.IsAuthorized(Permissions.UserQueryAdmin, true))
			{
				menuItemReportsUserQuery.Text = "User Query";
			}
			else
			{
				menuItemReportsUserQuery.Text = "Released User Queries";
			}
		}

		///<summary>Compares preferences related to sections of the program that require redraws and returns true if a redraw is necessary, false otherwise.  If anything goes wrong with checking the status of any preference this method will return true.</summary>
		private bool NeedsRedraw(string section)
		{
			try
			{
				switch (section)
				{
					case "ChartModule":
						if (dictChartPrefsCache.Count == 0
							|| PrefC.GetInt(PrefName.UseInternationalToothNumbers) != (int)dictChartPrefsCache["UseInternationalToothNumbers"]
							|| ComputerPrefs.LocalComputer.GraphicsUseHardware != (bool)dictChartPrefsCache["GraphicsUseHardware"]
							|| ComputerPrefs.LocalComputer.PreferredPixelFormatNum != (int)dictChartPrefsCache["PreferredPixelFormatNum"]
							|| ComputerPrefs.LocalComputer.GraphicsSimple != (DrawingMode)dictChartPrefsCache["GraphicsSimple"]
							|| Prefs.GetBool(PrefName.ShowFeatureEhr) != (bool)dictChartPrefsCache["ShowFeatureEhr"]
							|| ComputerPrefs.LocalComputer.DirectXFormat != (string)dictChartPrefsCache["DirectXFormat"])
						{
							return true;
						}
						break;
					case "TaskLists":
						if (dictTaskListPrefsCache.Count == 0
							|| ComputerPrefs.LocalComputer.TaskDock != (int)dictTaskListPrefsCache["TaskDock"] //Checking for task list redrawing
							|| ComputerPrefs.LocalComputer.TaskY != (int)dictTaskListPrefsCache["TaskY"]
							|| ComputerPrefs.LocalComputer.TaskX != (int)dictTaskListPrefsCache["TaskX"]
							|| Prefs.GetBool(PrefName.TaskListAlwaysShowsAtBottom) != (bool)dictTaskListPrefsCache["TaskListAlwaysShowsAtBottom"]
							|| Prefs.GetBool(PrefName.TasksUseRepeating) != (bool)dictTaskListPrefsCache["TasksUseRepeating"]
							|| Prefs.GetBool(PrefName.TasksNewTrackedByUser) != (bool)dictTaskListPrefsCache["TasksNewTrackedByUser"]
							|| Prefs.GetBool(PrefName.TasksShowOpenTickets) != (bool)dictTaskListPrefsCache["TasksShowOpenTickets"]
							|| ComputerPrefs.LocalComputer.TaskKeepListHidden != (bool)dictTaskListPrefsCache["TaskKeepListHidden"])
						{
							return true;
						}
						break;
						//case "TreatmentPlan":
						//	//If needed implement this section
						//	break;
				}//end switch
				return false;
			}
			catch
			{
				return true;//Should never happen.  Would most likely be caused by invalid preferences within the database.
			}
		}

		///<summary>Sets up the custom reports list in the main menu when certain requirements are met, or disables the custom reports menu item when those same conditions are not met. This function is called during initialization, and on the event that the A to Z folder usage has changed.</summary>
		private void CheckCustomReports()
		{
			menuItemCustomReports.MenuItems.Clear();
			//Try to load custom reports, but only if using the A to Z folders.

			try
			{
				string imagePath = OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath();
				string reportFolderName = Prefs.GetString(PrefName.ReportFolderName);
				string reportDir = ODFileUtils.CombinePaths(imagePath, reportFolderName);
				if (Directory.Exists(reportDir))
				{
					DirectoryInfo infoDir = new DirectoryInfo(reportDir);
					FileInfo[] filesRdl = infoDir.GetFiles("*.rdl");
					for (int i = 0; i < filesRdl.Length; i++)
					{
						string itemName = Path.GetFileNameWithoutExtension(filesRdl[i].Name);
						menuItemCustomReports.MenuItems.Add(itemName, new System.EventHandler(this.menuItemRDLReport_Click));
					}
				}
			}
			catch
			{
				MessageBox.Show("Failed to retrieve custom reports.");
			}

			if (menuItemCustomReports.MenuItems.Count == 0)
			{
				menuItemCustomReports.Visible = false;
			}
			else
			{
				menuItemCustomReports.Visible = true;
			}
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		private void LayoutToolBar()
		{
			ToolBarMain.Buttons.Clear();
			if (ModuleBar.IsAlternateIcons)
			{
				//if(ODColorTheme.HasFlatIcons) {
				ToolBarMain.ImageList = imageListFlat;
			}
			else
			{
				ToolBarMain.ImageList = imageListMain;
			}
			ODToolBarButton button;
			button = new ODToolBarButton("Select Patient", 0, "", "Patient");
			button.Style = ODToolBarButtonStyle.DropDownButton;
			button.DropDownMenu = menuPatient;
			ToolBarMain.Buttons.Add(button);
			if (!Programs.UsingEcwTightMode())
			{//eCW tight only gets Patient Select and Popups toolbar buttons
				button = new ODToolBarButton("Commlog", 1, "New Commlog Entry", "Commlog");
				ToolBarMain.Buttons.Add(button);
				button = new ODToolBarButton("E-mail", 2, "Send E-mail", "Email");
				button.Style = ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu = menuEmail;
				ToolBarMain.Buttons.Add(button);
				button = new ODToolBarButton("WebMail", 2, "Secure WebMail", "WebMail");
				button.Enabled = true;//Always enabled.  If the patient does not have an email address, then the user will be blocked from the FormWebMailMessageEdit window.
				ToolBarMain.Buttons.Add(button);
				if (_butText == null)
				{//If laying out again (after modifying setup), we keep the button to preserve the current notification text.
					_butText = new ODToolBarButton("Text", 5, "Send Text Message", "Text");
					_butText.Style = ODToolBarButtonStyle.DropDownButton;
					_butText.DropDownMenu = menuText;
					_butText.Enabled = Programs.IsEnabled(ProgramName.CallFire) || SmsPhones.IsIntegratedTextingEnabled();
					//The Notification text has not been set since startup.  We need an accurate starting count.
					if (SmsPhones.IsIntegratedTextingEnabled())
					{
						//Init.  Will query for sms notification signal, or insert one if not found (eConnector hasn't updated this signal since we last cleared
						//old signals).
						SetSmsNotificationText();
					}
				}
				ToolBarMain.Buttons.Add(_butText);
				button = new ODToolBarButton("Letter", -1, "Quick Letter", "Letter");
				button.Style = ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu = menuLetter;
				ToolBarMain.Buttons.Add(button);
				button = new ODToolBarButton("Forms", -1, "", "Form");
				//button.Style=ODToolBarButtonStyle.DropDownButton;
				//button.DropDownMenu=menuForm;
				ToolBarMain.Buttons.Add(button);
				if (_butTask == null)
				{
					_butTask = new ODToolBarButton("Tasks", 3, "Open Tasks", "Tasklist");
					_butTask.Style = ODToolBarButtonStyle.DropDownButton;
					_butTask.DropDownMenu = menuTask;
				}
				ToolBarMain.Buttons.Add(_butTask);
				button = new ODToolBarButton("Label", 4, "Print Label", "Label");
				button.Style = ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu = menuLabel;
				ToolBarMain.Buttons.Add(button);
			}
			ToolBarMain.Buttons.Add(new ODToolBarButton("Popups", -1, "Edit popups for this patient", "Popups"));
			ProgramL.LoadToolbar(ToolBarMain, ToolBarsAvail.MainToolbar);
			Plugins.HookAddCode(this, "FormOpenDental.LayoutToolBar_end");
			ToolBarMain.Invalidate();
		}

		///<summary>Starts a thread that repeatedly gets a random patient and selects each module. Goes until the program is closed.</summary>
		private void StartLoadSimulation()
		{
			ODThread threadLoad = new ODThread(o =>
			{
				void selectModule(int moduleBarIdx)
				{
					this.Invoke(() =>
					{
						moduleBar.SelectedIndex = moduleBarIdx;
						SetModuleSelected(true);
						myOutlookBar_ButtonClicked(moduleBar, new ButtonClicked_EventArgs(null, false));
					});
					Thread.Sleep(1000);
				}
				while (true)
				{
					Patient pat = Patients.GetRandomPatient();
					this.Invoke(() =>
					{
						CurPatNum = pat.PatNum;
						Text = PatientL.GetMainTitle(pat, Clinics.ClinicNum);
					});
					for (int i = 0; i <= 6; i++)
					{
						selectModule(i);
					}
				}
			});
			threadLoad.Name = "LoadSimulation";
			threadLoad.Start();
		}

		private void menuPatient_Popup(object sender, EventArgs e)
		{
			Family fam = null;
			if (CurPatNum != 0)
			{
				fam = Patients.GetFamily(CurPatNum);
			}
			//Always refresh the patient menu to reflect any patient status changes.
			PatientL.AddFamilyToMenu(menuPatient, new EventHandler(menuPatient_Click), CurPatNum, fam);
		}

		private void ToolBarMain_ButtonClick(object sender, ODToolBarButtonClickEventArgs e)
		{
			if (e.Button.Tag.GetType() == typeof(string))
			{
				//standard predefined button
				switch (e.Button.Tag.ToString())
				{
					case "Patient":
						toolButPatient_Click();
						break;
					case "Commlog":
						toolButCommlog_Click();
						break;
					case "Email":
						toolButEmail_Click();
						break;
					case "WebMail":
						toolButWebMail_Click();
						break;
					case "Text":
						toolButTxtMsg_Click(CurPatNum);
						break;
					case "Letter":
						toolButLetter_Click();
						break;
					case "Form":
						toolButForm_Click();
						break;
					case "Tasklist":
						toolButTasks_Click();
						break;
					case "Label":
						toolButLabel_Click();
						break;
					case "Popups":
						toolButPopups_Click();
						break;
				}
			}
			else if (e.Button.Tag.GetType() == typeof(Program))
			{
				ProgramL.Execute(((Program)e.Button.Tag).Id, Patients.GetPat(CurPatNum));
			}
		}

		private void toolButPatient_Click()
		{
			bool isClipMatch = false;
			try
			{
				if (Clipboard.ContainsText())
				{
					string txtClip = "";
					txtClip = Clipboard.GetText();
					if (Regex.IsMatch(txtClip, @"^PatNum:\d+$"))
					{//very restrictive specific match for "PatNum:##"
						long patNum = PIn.Long(txtClip.Substring(7));
						Patient patient = Patients.GetLim(patNum);
						if (patient.PatNum != 0)
						{
							Clipboard.Clear();//so if they click it again, the can select a patient
							CurPatNum = patNum;
							isClipMatch = true;
						}
					}
				}
			}
			catch
			{
				//do nothing
			}
			if (!isClipMatch)
			{
				FormPatientSelect formPatientSelect = new FormPatientSelect();
				formPatientSelect.ShowDialog();
				if (formPatientSelect.DialogResult != DialogResult.OK)
				{
					return;
				}
				CurPatNum = formPatientSelect.SelectedPatientId;
			}
			Patient pat = Patients.GetPat(CurPatNum);
			if (ContrChart2.Visible)
			{
				userControlTasks1.RefreshPatTicketsIfNeeded();//This is a special case.  Normally it's called in RefreshCurrentModule()
				ContrChart2.ModuleSelectedErx(CurPatNum);
			}
			else
			{
				RefreshCurrentModule();
			}
			FillPatientButton(pat);
			Plugins.HookAddCode(this, "FormOpenDental.OnPatient_Click_end"); //historical name 
		}

		private void menuPatient_Click(object sender, System.EventArgs e)
		{
			Family fam = Patients.GetFamily(CurPatNum);
			CurPatNum = PatientL.ButtonSelect(menuPatient, sender, fam);
			//new family now
			Patient pat = Patients.GetPat(CurPatNum);
			RefreshCurrentModule();
			FillPatientButton(pat);
		}

		///<summary>If the call to this is followed by ModuleSelected or GotoModule, set isRefreshCurModule=false to prevent the module from being refreshed twice.  If the current module is ContrAppt and the call to this is preceded by a call to RefreshModuleDataPatient, set isApptRefreshDataPat=false so the query to get the patient does not run twice.</summary>
		public static void S_Contr_PatientSelected(Patient pat, bool isRefreshCurModule, bool isApptRefreshDataPat = true, bool hasForcedRefresh = false)
		{
			_formOpenDentalS.Contr_PatientSelected(pat, isRefreshCurModule, isApptRefreshDataPat, hasForcedRefresh);
		}

		///<summary>Happens when any of the modules changes the current patient or when this main form changes the patient.  The calling module should refresh itself.  The current patNum is stored here in the parent form so that when switching modules, the parent form knows which patient to call up for that module.</summary>
		private void Contr_PatientSelected(Patient pat, bool isRefreshCurModule, bool isApptRefreshDataPat, bool hasForcedRefresh)
		{
			CurPatNum = pat.PatNum;
			if (isRefreshCurModule)
			{
				RefreshCurrentModule(hasForcedRefresh, isApptRefreshDataPat);
			}
			userControlTasks1.RefreshPatTicketsIfNeeded();
			FillPatientButton(pat);
		}

		///<Summary>Serves four functions.  1. Sends the new patient to the dropdown menu for select patient.  2. Changes which toolbar buttons are enabled.  3. Sets main form text.  4. Displays any popup.</Summary>
		private void FillPatientButton(Patient pat)
		{
			if (pat == null)
			{
				pat = new Patient();
			}
			Text = PatientL.GetMainTitle(pat, Clinics.ClinicNum);
			bool patChanged = PatientL.AddPatientToMenu(pat.GetNameLF(), pat.PatNum);
			if (patChanged)
			{
				if (AutomationL.Trigger(AutomationTrigger.OpenPatient, null, pat.PatNum))
				{//if a trigger happened
					if (ContrAppt2.Visible)
					{
						ContrAppt2.MouseUpForced();
					}
				}
			}
			if (ToolBarMain.Buttons == null || ToolBarMain.Buttons.Count < 2)
			{//on startup.  js Not sure why it's checking count.
				return;
			}
			if (CurPatNum == 0)
			{//Only on startup, I think.
				if (!Programs.UsingEcwTightMode())
				{//eCW tight only gets Patient Select and Popups toolbar buttons
				 //We need a drafts folder the user can view saved emails in before we allow the user to save email without a patient selected.
					ToolBarMain.Buttons["Email"].Enabled = false;
					ToolBarMain.Buttons["WebMail"].Enabled = false;
					ToolBarMain.Buttons["Commlog"].Enabled = false;
					ToolBarMain.Buttons["Letter"].Enabled = false;
					ToolBarMain.Buttons["Form"].Enabled = false;
					ToolBarMain.Buttons["Tasklist"].Enabled = true;
					ToolBarMain.Buttons["Label"].Enabled = false;
				}
				ToolBarMain.Buttons["Popups"].Enabled = false;
			}
			else
			{
				if (!Programs.UsingEcwTightMode())
				{//eCW tight only gets Patient Select and Popups toolbar buttons
					ToolBarMain.Buttons["Commlog"].Enabled = true;
					ToolBarMain.Buttons["Email"].Enabled = true;
					if (_butText != null)
					{
						_butText.Enabled = Programs.IsEnabled(ProgramName.CallFire) || SmsPhones.IsIntegratedTextingEnabled();
					}
					ToolBarMain.Buttons["WebMail"].Enabled = true;
					ToolBarMain.Buttons["Letter"].Enabled = true;
					ToolBarMain.Buttons["Form"].Enabled = true;
					ToolBarMain.Buttons["Tasklist"].Enabled = true;
					ToolBarMain.Buttons["Label"].Enabled = true;
				}
				ToolBarMain.Buttons["Popups"].Enabled = true;
			}
			ToolBarMain.Invalidate();
			if (PopupEventList == null)
			{
				PopupEventList = new List<PopupEvent>();
			}
			if (Plugins.HookMethod(this, "FormOpenDental.FillPatientButton_popups", pat, PopupEventList, patChanged))
			{
				return;
			}
			if (!patChanged)
			{
				return;
			}
			if (ContrChart2.Visible)
			{
				TryNonPatientPopup();
			}
			//New patient selected.  Everything below here is for popups.
			//First, remove all expired popups from the event list.
			for (int i = PopupEventList.Count - 1; i >= 0; i--)
			{//go backwards
				if (PopupEventList[i].DisableUntil < DateTime.Now)
				{//expired
					PopupEventList.RemoveAt(i);
				}
			}
			//Now, loop through all popups for the patient.
			List<Popup> popList = Popups.GetForPatient(pat);//get all possible 
			for (int i = 0; i < popList.Count; i++)
			{
				//skip any popups that are disabled because they are on the event list
				bool popupIsDisabled = false;
				for (int e = 0; e < PopupEventList.Count; e++)
				{
					if (popList[i].PopupNum == PopupEventList[e].PopupNum)
					{
						popupIsDisabled = true;
						break;
					}
				}
				if (popupIsDisabled)
				{
					continue;
				}
				//This popup is not disabled, so show it.
				//A future improvement would be to assemble all the popups that are to be shown and then show them all in one large window.
				//But for now, they will show in sequence.
				if (ContrAppt2.Visible)
				{
					ContrAppt2.MouseUpForced();
				}
				FormPopupDisplay FormP = new FormPopupDisplay();
				FormP.PopupCur = popList[i];
				FormP.ShowDialog();
				if (FormP.MinutesDisabled > 0)
				{
					PopupEvent popevent = new PopupEvent();
					popevent.PopupNum = popList[i].PopupNum;
					popevent.DisableUntil = DateTime.Now + TimeSpan.FromMinutes(FormP.MinutesDisabled);
					popevent.LastViewed = DateTime.Now;
					PopupEventList.Add(popevent);
					PopupEventList.Sort();
				}
			}
		}

		private void toolButEmail_Click()
		{
			if (CurPatNum == 0)
			{
				MessageBox.Show("Please select a patient to send an email.");
				return;
			}
			if (!Security.IsAuthorized(Permissions.EmailSend))
			{
				return;
			}
			EmailMessage message = new EmailMessage();
			message.PatNum = CurPatNum;
			Patient pat = Patients.GetPat(CurPatNum);
			message.ToAddress = pat.Email;
			EmailAddress selectedAddress = EmailAddresses.GetNewEmailDefault(Security.CurrentUser.Id, pat.ClinicNum);
			message.FromAddress = selectedAddress.GetFrom();
			FormEmailMessageEdit FormE = new FormEmailMessageEdit(message, selectedAddress);
			FormE.IsNew = true;
			FormE.ShowDialog();
			if (FormE.DialogResult == DialogResult.OK)
			{
				RefreshCurrentModule();
			}
		}

		private void menuEmail_Popup(object sender, EventArgs e)
		{
			menuEmail.MenuItems.Clear();
			MenuItem menuItem;
			menuItem = new MenuItem("Referrals:");
			menuItem.Tag = null;
			menuEmail.MenuItems.Add(menuItem);
			List<RefAttach> refAttaches = RefAttaches.Refresh(CurPatNum);
			string referralDescript = DisplayFields.GetForCategory(DisplayFieldCategory.PatientInformation)
				.FirstOrDefault(x => x.InternalName == "Referrals")?.Description;
			if (string.IsNullOrWhiteSpace(referralDescript))
			{//either not displaying the Referral field or no description entered, default to 'Referral'
				referralDescript = "Referral";
			}
			Referral refer;
			string str;
			for (int i = 0; i < refAttaches.Count; i++)
			{
				if (!Referrals.TryGetReferral(refAttaches[i].ReferralNum, out refer))
				{
					continue;
				}
				if (refAttaches[i].RefType == ReferralType.RefFrom)
				{
					str = "From";
				}
				else if (refAttaches[i].RefType == ReferralType.RefTo)
				{
					str = "To";
				}
				else
				{
					str = referralDescript;
				}
				str += " " + Referrals.GetNameFL(refer.ReferralNum) + " <";
				if (refer.EMail == "")
				{
					str += "no email";
				}
				else
				{
					str += refer.EMail;
				}
				str += ">";
				menuItem = new MenuItem(str, menuEmail_Click);
				menuItem.Tag = refer;
				menuEmail.MenuItems.Add(menuItem);
			}
		}

		private void toolButWebMail_Click()
		{
			if (!Security.IsAuthorized(Permissions.WebMailSend))
			{
				return;
			}
			FormWebMailMessageEdit FormWMME = new FormWebMailMessageEdit(CurPatNum);
			FormWMME.ShowDialog();
		}

		private void menuEmail_Click(object sender, System.EventArgs e)
		{
			if (((MenuItem)sender).Tag == null)
			{
				return;
			}
			LabelSingle label = new LabelSingle();
			if (((MenuItem)sender).Tag.GetType() == typeof(Referral))
			{
				Referral refer = (Referral)((MenuItem)sender).Tag;
				if (refer.EMail == "")
				{
					return;
					//MessageBox.Show("");
				}
				EmailMessage message = new EmailMessage();
				message.PatNum = CurPatNum;
				Patient pat = Patients.GetPat(CurPatNum);
				message.ToAddress = refer.EMail;//pat.Email;
				EmailAddress address = EmailAddresses.GetByClinic(pat.ClinicNum);
				message.FromAddress = address.GetFrom();
				message.Subject = "RE: " + pat.GetNameFL();
				FormEmailMessageEdit FormE = new FormEmailMessageEdit(message, address);
				FormE.IsNew = true;
				FormE.ShowDialog();
				if (FormE.DialogResult == DialogResult.OK)
				{
					RefreshCurrentModule();
				}
			}
		}

		private void toolButCommlog_Click()
		{
			if (Plugins.HookMethod(this, "FormOpenDental.OnCommlog_Click", CurPatNum))
			{
				return;
			}
			FormCommItem FormCI = new FormCommItem(GetNewCommlog());
			if (FormCI.ShowDialog() == DialogResult.OK)
			{
				RefreshCurrentModule();
			}
		}

		private void menuItemCommlogPersistent_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.CommlogPersistent))
			{
				return;
			}
			FormCommItem FormCI = Application.OpenForms.OfType<FormCommItem>().FirstOrDefault(x => !x.IsDisposed);
			if (FormCI == null)
			{
				FormCI = new FormCommItem(GetNewCommlog(), isPersistent: true);
			}
			if (FormCI.WindowState == FormWindowState.Minimized)
			{
				FormCI.WindowState = FormWindowState.Normal;
			}
			FormCI.Show();
			FormCI.BringToFront();
		}

		///<summary>This is a helper method to get a new commlog object for the commlog tool bar buttons.</summary>
		private Commlog GetNewCommlog()
		{
			Commlog commlog = new Commlog();
			commlog.PatNum = CurPatNum;
			commlog.CommDateTime = DateTime.Now;
			commlog.CommType = Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
			commlog.Mode_ = CommItemMode.Phone;
			commlog.SentOrReceived = CommSentOrReceived.Received;
			commlog.UserNum = Security.CurrentUser.Id;
			commlog.IsNew = true;
			return commlog;
		}

		private void toolButLetter_Click()
		{
			FormSheetPicker FormS = new FormSheetPicker();
			FormS.SheetType = SheetTypeEnum.PatientLetter;
			FormS.ShowDialog();
			if (FormS.DialogResult != DialogResult.OK)
			{
				return;
			}
			SheetDef sheetDef = FormS.SelectedSheetDefs[0];
			Sheet sheet = SheetUtil.CreateSheet(sheetDef, CurPatNum);
			SheetParameter.SetParameter(sheet, "PatNum", CurPatNum);
			//SheetParameter.SetParameter(sheet,"ReferralNum",referral.ReferralNum);
			SheetFiller.FillFields(sheet);
			SheetUtil.CalculateHeights(sheet);
			FormSheetFillEdit.ShowForm(sheet, FormSheetFillEdit_FormClosing);
			//Patient pat=Patients.GetPat(CurPatNum);
			//FormLetters FormL=new FormLetters(pat);
			//FormL.ShowDialog();
		}

		private void menuLetter_Popup(object sender, EventArgs e)
		{
			menuLetter.MenuItems.Clear();
			MenuItem menuItem;
			menuItem = new MenuItem("Merge", menuLetter_Click);
			menuItem.Tag = "Merge";
			menuLetter.MenuItems.Add(menuItem);
			//menuItem=new MenuItem("Stationery",menuLetter_Click);
			//menuItem.Tag="Stationery";
			//menuLetter.MenuItems.Add(menuItem);
			menuLetter.MenuItems.Add("-");
			//Referrals---------------------------------------------------------------------------------------
			menuItem = new MenuItem("Referrals:");
			menuItem.Tag = null;
			menuLetter.MenuItems.Add(menuItem);
			string referralDescript = DisplayFields.GetForCategory(DisplayFieldCategory.PatientInformation)
				.FirstOrDefault(x => x.InternalName == "Referrals")?.Description;
			if (string.IsNullOrWhiteSpace(referralDescript))
			{//either not displaying the Referral field or no description entered, default to 'Referral'
				referralDescript = "Referral";
			}
			List<RefAttach> refAttaches = RefAttaches.Refresh(CurPatNum);
			Referral refer;
			string str;
			for (int i = 0; i < refAttaches.Count; i++)
			{
				if (!Referrals.TryGetReferral(refAttaches[i].ReferralNum, out refer))
				{
					continue;
				}
				if (refAttaches[i].RefType == ReferralType.RefFrom)
				{
					str = "From";
				}
				else if (refAttaches[i].RefType == ReferralType.RefTo)
				{
					str = "To";
				}
				else
				{
					str = referralDescript;
				}
				str += " " + Referrals.GetNameFL(refer.ReferralNum);
				menuItem = new MenuItem(str, menuLetter_Click);
				menuItem.Tag = refer;
				menuLetter.MenuItems.Add(menuItem);
			}
		}

		private void menuLetter_Click(object sender, System.EventArgs e)
		{
			if (((MenuItem)sender).Tag == null)
			{
				return;
			}
			Patient pat = Patients.GetPat(CurPatNum);
			if (((MenuItem)sender).Tag.GetType() == typeof(string))
			{
				if (((MenuItem)sender).Tag.ToString() == "Merge")
				{
					FormLetterMerges FormL = new FormLetterMerges(pat);
					FormL.ShowDialog();
				}
				//if(((MenuItem)sender).Tag.ToString()=="Stationery") {
				//	FormCommunications.PrintStationery(pat);
				//}
			}
			if (((MenuItem)sender).Tag.GetType() == typeof(Referral))
			{
				Referral refer = (Referral)((MenuItem)sender).Tag;
				FormSheetPicker FormS = new FormSheetPicker();
				FormS.SheetType = SheetTypeEnum.ReferralLetter;
				FormS.ShowDialog();
				if (FormS.DialogResult != DialogResult.OK)
				{
					return;
				}
				SheetDef sheetDef = FormS.SelectedSheetDefs[0];
				Sheet sheet = SheetUtil.CreateSheet(sheetDef, CurPatNum);
				SheetParameter.SetParameter(sheet, "PatNum", CurPatNum);
				SheetParameter.SetParameter(sheet, "ReferralNum", refer.ReferralNum);
				//Don't fill these params if the sheet doesn't use them.
				if (sheetDef.SheetFieldDefs.Any(x =>
					 (x.FieldType == SheetFieldType.Grid && x.FieldName == "ReferralLetterProceduresCompleted")
					 || (x.FieldType == SheetFieldType.Special && x.FieldName == "toothChart")))
				{
					List<Procedure> listProcs = Procedures.GetCompletedForDateRange(sheet.DateTimeSheet, sheet.DateTimeSheet
						, listPatNums: new List<long>() { CurPatNum }
						, includeNote: true
						, includeGroupNote: true);
					if (sheetDef.SheetFieldDefs.Any(x => x.FieldType == SheetFieldType.Grid && x.FieldName == "ReferralLetterProceduresCompleted"))
					{
						SheetParameter.SetParameter(sheet, "CompletedProcs", listProcs);
					}
					if (sheetDef.SheetFieldDefs.Any(x => x.FieldType == SheetFieldType.Special && x.FieldName == "toothChart"))
					{
						SheetParameter.SetParameter(sheet, "toothChartImg", SheetPrinting.GetToothChartHelper(CurPatNum, false, listProceduresFilteredOverride: listProcs));
					}
				}
				SheetFiller.FillFields(sheet);
				SheetUtil.CalculateHeights(sheet);
				FormSheetFillEdit.ShowForm(sheet, FormSheetFillEdit_FormClosing);
				//FormLetters FormL=new FormLetters(pat);
				//FormL.ReferralCur=refer;
				//FormL.ShowDialog();
			}
		}

		/// <summary>Event handler for closing FormSheetFillEdit when it is non-modal.</summary>
		private void FormSheetFillEdit_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (((FormSheetFillEdit)sender).DialogResult == DialogResult.OK || ((FormSheetFillEdit)sender).DidChangeSheet)
			{
				RefreshCurrentModule();
			}
		}

		private void toolButForm_Click()
		{
			FormPatientForms formP = new FormPatientForms();
			formP.PatNum = CurPatNum;
			formP.ShowDialog();
			//always refresh, especially to get the titlebar right after an import.
			Patient pat = Patients.GetPat(CurPatNum);
			RefreshCurrentModule(docNum: formP.DocNum);
			FillPatientButton(pat);
		}

		private void toolButTasks_Click()
		{
			using FormTaskListSelect formTaskListSelect = new FormTaskListSelect();

			formTaskListSelect.Location = new Point(50, 50);
			formTaskListSelect.Text = "Add Task - " + formTaskListSelect.Text;
			if (formTaskListSelect.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			Task task = new Task();
			task.TaskListId = -1;//don't show it in any list yet.
			Tasks.Insert(task);

			Task taskOld = task.Copy();
			task.PatientId = CurPatNum;
			task.TaskListId = formTaskListSelect.SelectedList.Id;
			task.UserId = Security.CurrentUser.Id;

			var formTaskEdit = new FormTaskEdit(task);
			formTaskEdit.Show();
		}

		private void menuTask_Popup(object sender, EventArgs e)
		{
			menuItemTaskNewForUser.Text = "for" + " " + Security.CurrentUser.UserName;
			menuItemTaskReminders.Text = "Reminders";
			int reminderTaskNewCount = GetNewReminderTaskCount();
			if (reminderTaskNewCount > 0)
			{
				menuItemTaskReminders.Text += " (" + reminderTaskNewCount + ")";
			}
			int otherTaskCount = (_listNormalTaskNums != null) ? _listNormalTaskNums.Count : 0;
			if (otherTaskCount > 0)
			{
				menuItemTaskNewForUser.Text += " (" + otherTaskCount + ")";
			}
		}

		private void RefreshTasksNotification()
		{
			if (_butTask == null)
			{
				return;
			}
			// TODO: Logger.LogToPath("", LogPath.Signals, LogPhase.Start);
			int otherTaskCount = (_listNormalTaskNums != null) ? _listNormalTaskNums.Count : 0;
			int totalTaskCount = GetNewReminderTaskCount() + otherTaskCount;
			string notificationText = "";
			if (totalTaskCount > 0)
			{
				notificationText = Math.Min(totalTaskCount, 99).ToString();
			}
			if (notificationText != _butTask.NotificationText)
			{
				_butTask.NotificationText = notificationText;
				ToolBarMain.Invalidate(_butTask.Bounds);//Cause the notification text on the Task button to update as soon as possible.
			}
			// TODO: Logger.LogToPath("", LogPath.Signals, LogPhase.End);
		}

		private int GetNewReminderTaskCount()
		{
			if (_listReminderTasks == null)
			{
				return 0;
			}
			//Mimics how checkNew is set in FormTaskEdit.
			if (Prefs.GetBool(PrefName.TasksNewTrackedByUser))
			{//Per definition of task.IsUnread.
				return _listReminderTasks.FindAll(x => x.IsUnread && x.DateStart <= DateTime.Now).Count;
			}
			return _listReminderTasks.FindAll(x => x.Status == TaskStatus.New && x.DateStart <= DateTime.Now).Count;
		}

		private void menuItemTaskNewForUser_Click(object sender, EventArgs e)
		{
			ContrManage2.LaunchTaskWindow();//Set the tab to the "for [User]" tab.
		}

		private void menuItemTaskReminders_Click(object sender, EventArgs e)
		{
			ContrManage2.LaunchTaskWindow();//Set the tab to the "Reminders" tab
		}

		private delegate void ToolBarMainClick(long patNum);

		private void toolButLabel_Click()
		{
			//The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
			//when it comes from a toolbar click.
			//https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
			ToolBarMainClick toolClick = LabelSingle.PrintPat;
			this.BeginInvoke(toolClick, CurPatNum);
		}

		private void menuLabel_Popup(object sender, EventArgs e)
		{
			menuLabel.MenuItems.Clear();
			MenuItem menuItem;
			List<SheetDef> LabelList = SheetDefs.GetCustomForType(SheetTypeEnum.LabelPatient);
			if (LabelList.Count == 0)
			{
				menuItem = new MenuItem("LName, FName, Address", menuLabel_Click);
				menuItem.Tag = "PatientLFAddress";
				menuLabel.MenuItems.Add(menuItem);
				menuItem = new MenuItem("Name, ChartNumber", menuLabel_Click);
				menuItem.Tag = "PatientLFChartNumber";
				menuLabel.MenuItems.Add(menuItem);
				menuItem = new MenuItem("Name, PatNum", menuLabel_Click);
				menuItem.Tag = "PatientLFPatNum";
				menuLabel.MenuItems.Add(menuItem);
				menuItem = new MenuItem("Radiograph", menuLabel_Click);
				menuItem.Tag = "PatRadiograph";
				menuLabel.MenuItems.Add(menuItem);
			}
			else
			{
				for (int i = 0; i < LabelList.Count; i++)
				{
					menuItem = new MenuItem(LabelList[i].Description, menuLabel_Click);
					menuItem.Tag = LabelList[i];
					menuLabel.MenuItems.Add(menuItem);
				}
			}
			menuLabel.MenuItems.Add("-");
			//Carriers---------------------------------------------------------------------------------------
			Family fam = Patients.GetFamily(CurPatNum);
			//Received multiple bug submissions where CurPatNum==0, even though this toolbar button should not be enabled when no patient is selected.
			if (fam.ListPats != null && fam.ListPats.Length > 0)
			{
				List<PatPlan> PatPlanList = PatPlans.Refresh(CurPatNum);
				List<InsSub> subList = InsSubs.RefreshForFam(fam);
				List<InsPlan> PlanList = InsPlans.RefreshForSubList(subList);
				Carrier carrier;
				InsPlan plan;
				InsSub sub;
				for (int i = 0; i < PatPlanList.Count; i++)
				{
					sub = InsSubs.GetSub(PatPlanList[i].InsSubNum, subList);
					plan = InsPlans.GetPlan(sub.PlanNum, PlanList);
					carrier = Carriers.GetCarrier(plan.CarrierNum);
					menuItem = new MenuItem(carrier.CarrierName, menuLabel_Click);
					menuItem.Tag = carrier;
					menuLabel.MenuItems.Add(menuItem);
				}
				menuLabel.MenuItems.Add("-");
			}
			//Referrals---------------------------------------------------------------------------------------
			menuItem = new MenuItem("Referrals:");
			menuItem.Tag = null;
			menuLabel.MenuItems.Add(menuItem);
			string referralDescript = DisplayFields.GetForCategory(DisplayFieldCategory.PatientInformation)
				.FirstOrDefault(x => x.InternalName == "Referrals")?.Description;
			if (string.IsNullOrWhiteSpace(referralDescript))
			{//either not displaying the Referral field or no description entered, default to 'Referral'
				referralDescript = "Referral";
			}
			List<RefAttach> refAttaches = RefAttaches.Refresh(CurPatNum);
			Referral refer;
			string str;
			for (int i = 0; i < refAttaches.Count; i++)
			{
				if (!Referrals.TryGetReferral(refAttaches[i].ReferralNum, out refer))
				{
					continue;
				}
				if (refAttaches[i].RefType == ReferralType.RefFrom)
				{
					str = "From";
				}
				else if (refAttaches[i].RefType == ReferralType.RefTo)
				{
					str = "To";
				}
				else
				{
					str = referralDescript;
				}
				str += " " + Referrals.GetNameFL(refer.ReferralNum);
				menuItem = new MenuItem(str, menuLabel_Click);
				menuItem.Tag = refer;
				menuLabel.MenuItems.Add(menuItem);
			}
		}

		private void menuLabel_Click(object sender, System.EventArgs e)
		{
			if (((MenuItem)sender).Tag == null)
			{
				return;
			}
			//LabelSingle label=new LabelSingle();
			if (((MenuItem)sender).Tag.GetType() == typeof(string))
			{
				if (((MenuItem)sender).Tag.ToString() == "PatientLFAddress")
				{
					LabelSingle.PrintPatientLFAddress(CurPatNum);
				}
				if (((MenuItem)sender).Tag.ToString() == "PatientLFChartNumber")
				{
					LabelSingle.PrintPatientLFChartNumber(CurPatNum);
				}
				if (((MenuItem)sender).Tag.ToString() == "PatientLFPatNum")
				{
					LabelSingle.PrintPatientLFPatNum(CurPatNum);
				}
				if (((MenuItem)sender).Tag.ToString() == "PatRadiograph")
				{
					LabelSingle.PrintPatRadiograph(CurPatNum);
				}
			}
			else if (((MenuItem)sender).Tag.GetType() == typeof(SheetDef))
			{
				LabelSingle.PrintCustomPatient(CurPatNum, (SheetDef)((MenuItem)sender).Tag);
			}
			else if (((MenuItem)sender).Tag.GetType() == typeof(Carrier))
			{
				Carrier carrier = (Carrier)((MenuItem)sender).Tag;
				LabelSingle.PrintCarrier(carrier.CarrierNum);
			}
			else if (((MenuItem)sender).Tag.GetType() == typeof(Referral))
			{
				Referral refer = (Referral)((MenuItem)sender).Tag;
				LabelSingle.PrintReferral(refer.ReferralNum);
			}
		}

		private void toolButPopups_Click()
		{
			FormPopupsForFam FormPFF = new FormPopupsForFam(PopupEventList);
			FormPFF.PatCur = Patients.GetPat(CurPatNum);
			FormPFF.ShowDialog();
		}

		#region SMS Text Messaging

		///<summary>Returns true if the message was sent successfully.</summary>
		public static bool S_OnTxtMsg_Click(long patNum, string startingText = "")
		{
			return _formOpenDentalS.toolButTxtMsg_Click(patNum, startingText);
		}

		///<summary>Called from the text message button and the right click context menu for an appointment. Returns true if the message was sent
		///successfully.</summary>
		private bool toolButTxtMsg_Click(long patNum, string startingText = "")
		{
			if (patNum == 0)
			{
				FormTxtMsgEdit FormTxtME = new FormTxtMsgEdit();
				FormTxtME.Message = startingText;
				FormTxtME.PatNum = 0;
				FormTxtME.ShowDialog();
				if (FormTxtME.DialogResult == DialogResult.OK)
				{
					RefreshCurrentModule();
					return true;
				}
				return false;
			}
			Patient pat = Patients.GetPat(patNum);
			bool updateTextYN = false;
			if (pat.TxtMsgOk == YN.No)
			{
				if (MsgBox.Show(MsgBoxButtons.YesNo, "This patient is marked to not receive text messages. "
					+ "Would you like to mark this patient as okay to receive text messages?"))
				{
					updateTextYN = true;
				}
				else
				{
					return false;
				}
			}
			if (pat.TxtMsgOk == YN.Unknown && Prefs.GetBool(PrefName.TextMsgOkStatusTreatAsNo))
			{
				if (MsgBox.Show(MsgBoxButtons.YesNo, "This patient might not want to receive text messages. "
					+ "Would you like to mark this patient as okay to receive text messages?"))
				{
					updateTextYN = true;
				}
				else
				{
					return false;
				}
			}
			if (updateTextYN)
			{
				Patient patOld = pat.Copy();
				pat.TxtMsgOk = YN.Yes;
				Patients.Update(pat, patOld);
			}
			FormTxtMsgEdit FormTME = new FormTxtMsgEdit();
			FormTME.Message = startingText;
			FormTME.PatNum = patNum;
			FormTME.WirelessPhone = pat.WirelessPhone;
			FormTME.TxtMsgOk = pat.TxtMsgOk;
			FormTME.ShowDialog();
			if (FormTME.DialogResult == DialogResult.OK)
			{
				RefreshCurrentModule();
				return true;
			}
			return false;
		}

		private void menuItemTextMessagesReceived_Click(object sender, EventArgs e)
		{
			ShowFormTextMessagingModeless(false, true);
		}

		private void menuItemTextMessagesSent_Click(object sender, EventArgs e)
		{
			ShowFormTextMessagingModeless(true, false);
		}

		private void menuItemTextMessagesAll_Click(object sender, EventArgs e)
		{
			ShowFormTextMessagingModeless(true, true);
		}

		private void ShowFormTextMessagingModeless(bool isSent, bool isReceived)
		{
			if (_formSmsTextMessaging == null || _formSmsTextMessaging.IsDisposed)
			{
				_formSmsTextMessaging = new FormSmsTextMessaging(isSent, isReceived, (x) => { SetSmsNotificationText(increment: x); });
				_formSmsTextMessaging.FormClosed += new FormClosedEventHandler((o, e) => { _formSmsTextMessaging = null; });
			}
			_formSmsTextMessaging.Show();
			_formSmsTextMessaging.BringToFront();
		}

		///<summary>Sets the SMS "Text" button's Notification Text based on structured data parsed from signalSmsCount.MsgValue.  This Signalod will have
		///been inserted into the db by the eConnector.  If signalSmsCount is not passed in, attempts to find the most recent Signalod of type 
		///SmsTextMsgReceivedUnreadCount, using it to update the notification text, or if not found, either creates and inserts the Signalod (this occurs
		///on startup if the Signalod table does not have an entry for this signal type) or uses the currently displayed value of the Sms notification 
		///Text and the 'increment' value to update the locally displayed notification count (occurs when this method is called between signal intervals
		///and the eConnector has not updated the SmsTextmsgReceivedUnreadCount signal in the last signal interval).
		///</summary>
		///<param name="signalSmsCount">Signalod, inserted by the eConnector, containing a list of clinicnums and the count of unread SmsFromMobiles for 
		///each clinic.</param>
		///<param name="doUseSignalInterval">Defaults to true.  Indicates, in the event that signalSmsCount is null, if the query to find the most 
		///recent SmsTextMsgReceivedUnreadCount type Signalod should be run for the interval since signals were last processed, or if the entire table
		///should be considered.</param>
		///<param name="increment">Defaults to 0.  Increments the value displayed in the Sms notification "Text" button for unread SmsFromMobiles, but
		///only if a Signalod of type SmsTextMsgReceivedUnreadCount was not found.  This can occur if signalSmsCount is null, doUseSignalInteral is true
		///and the signal was not found in the the last signal interval (meaning the eConnector has not updated the SmsNotification count recently).
		///</param>
		private void SetSmsNotificationText(Signalod signalSmsCount = null, bool doUseSignalInterval = true, int increment = 0)
		{
			//if (_butText == null)
			//{
			//	return;//This button does not exist in eCW tight integration mode.
			//}
			//try
			//{
			//	if (!_butText.Enabled)
			//	{
			//		return;//This button is disabled when neither of the Text Messaging bridges have been enabled.
			//	}
			//	List<SmsFromMobiles.SmsNotification> listNotifications = null;
			//	if (signalSmsCount == null)
			//	{
			//		//If we are here because the user changed clinics, then get the absolute most recent sms notification signal.
			//		//Otherwise, use DateTime since last signal refresh.
			//		DateTime signalStartTime = doUseSignalInterval ? Signalods.SignalLastRefreshed : DateTime.MinValue;
			//		//Get the most recent SmsTextMsgReceivedUnreadCount. Should only be one, but just in case, order desc.
			//		signalSmsCount = Signalods.RefreshTimed(signalStartTime, new List<InvalidType>() { InvalidType.SmsTextMsgReceivedUnreadCount })
			//			.OrderByDescending(x => x.Date)
			//			.FirstOrDefault();
			//		if (signalSmsCount == null && signalStartTime == DateTime.MinValue)
			//		{
			//			//No SmsTextMsgReceivedUnreadCount signal in db.  This means the eConnector has not updated the sms notification signal in quite some 
			//			//time.  Do the eConnector's job; 
			//			//listNotifications = Signalods.UpsertSmsNotification();
			//		}
			//	}
			//	if (signalSmsCount != null)
			//	{//Either the signal was passed in, or we found it when we queried.
			//		listNotifications = SmsFromMobiles.SmsNotification.GetListFromJson(signalSmsCount.Value);//Extract notifications from signal.
			//		if (listNotifications == null)
			//		{
			//			return;//Something went wrong deserializing the signal.  Leave the stale notification count until eConnector updates the signal.
			//		}
			//	}
			//	int smsUnreadCount = 0;
			//	if (listNotifications == null)
			//	{
			//		//listNotifications might still be null if signalSmsCount was not passed in, signal processing had already started, and we didn't find the
			//		//sms notification signal in the last signal interval.  We will assume the signal is stale.  We know the count has changed (based on some 
			//		//action) if 'increment' is non-zero, so increment according to our known changes.
			//		smsUnreadCount = PIn.Int(_butText.NotificationText) + increment;
			//	}
			//	else if (!PrefC.HasClinicsEnabled || Clinics.ClinicNum == 0)
			//	{
			//		//No clinics or HQ clinic is active so sum them all.
			//		smsUnreadCount = listNotifications.Sum(x => x.Count);
			//	}
			//	else
			//	{
			//		//Only count the active clinic.
			//		smsUnreadCount = listNotifications.Where(x => x.ClinicNum == Clinics.ClinicNum).Sum(x => x.Count);
			//	}
			//	//Default to empty so we show nothing if there aren't any notifications.
			//	string smsNotificationText = "";
			//	if (smsUnreadCount > 99)
			//	{ //We only have room in the UI for a 2-digit number.
			//		smsNotificationText = "99";
			//	}
			//	else if (smsUnreadCount > 0)
			//	{ //We have a "real" number so show it.
			//		smsNotificationText = smsUnreadCount.ToString();
			//	}
			//	if (_butText.NotificationText == smsNotificationText)
			//	{ //Prevent the toolbar from being invalidated unnecessarily.
			//		return;
			//	}
			//	_butText.NotificationText = smsNotificationText;
			//	if (menuItemTextMessagesReceived.Text.Contains("("))
			//	{//Remove the old count from the menu item.
			//		menuItemTextMessagesReceived.Text = menuItemTextMessagesReceived.Text.Substring(0, menuItemTextMessagesReceived.Text.IndexOf("(") - 1);
			//	}
			//	if (smsNotificationText != "")
			//	{
			//		menuItemTextMessagesReceived.Text += " (" + smsNotificationText + ")";
			//	}
			//	Plugins.HookAddCode(this, "FormOpenDental.SetSmsNotificationText_end", _butText, menuItemTextMessagesReceived, increment);
			//}
			//finally
			//{ //Always redraw the toolbar item.
			//	ToolBarMain.Invalidate(_butText.Bounds);//To cause the Text button to redraw.			
			//}
		}

		#endregion SMS Text Messaging

		private void RefreshMenuClinics()
		{
			menuClinics.MenuItems.Clear();
			List<Clinic> listClinics = Clinics.GetForUserod(Security.CurrentUser);
			if (listClinics.Count < 30)
			{ //This number of clinics will fit in a 990x735 form.
				MenuItem menuItem;
				if (!Security.CurrentUser.ClinicIsRestricted)
				{
					menuItem = new MenuItem("Headquarters", menuClinic_Click);
					menuItem.Tag = new Clinic();//Having a ClinicNum of 0 will make OD act like 'Headquarters'.  This allows the user to see unassigned appt views, all operatories, etc.
					if (Clinics.ClinicNum == 0)
					{
						menuItem.Checked = true;
					}
					menuClinics.MenuItems.Add(menuItem);
					menuClinics.MenuItems.Add("-");//Separator
				}
				for (int i = 0; i < listClinics.Count; i++)
				{
					menuItem = new MenuItem(listClinics[i].Abbr, menuClinic_Click);
					menuItem.Tag = listClinics[i];
					if (Clinics.ClinicNum == listClinics[i].ClinicNum)
					{
						menuItem.Checked = true;
					}
					menuClinics.MenuItems.Add(menuItem);
				}
			}
			else
			{//too many clinics to put in a menu drop down
				menuClinics.Click -= menuClick_OpenPickList;
				menuClinics.Click += menuClick_OpenPickList;
			}
			RefreshLocalData(InvalidType.Views);//fills apptviews, sets the view, and then calls ContrAppt.ModuleSelected
			if (!ContrAppt2.Visible)
			{
				RefreshCurrentModule();//calls ModuleSelected of the current module, don't do this if ContrAppt2 is visible since it was just done above
			}
			moduleBar.RefreshButtons();
		}

		private void menuClick_OpenPickList(object sender, EventArgs e)
		{
			FormClinics FormC = new FormClinics();
			FormC.IsSelectionMode = true;
			if (!Security.CurrentUser.ClinicIsRestricted)
			{
				FormC.IncludeHQInList = true;
			}
			FormC.ShowDialog();
			if (FormC.DialogResult != DialogResult.OK)
			{
				return;
			}
			if (FormC.SelectedClinicNum == 0)
			{//'Headquarters' was selected.
				RefreshCurrentClinic(new Clinic());
				return;
			}
			Clinic clinicCur = Clinics.GetFirstOrDefault(x => x.ClinicNum == FormC.SelectedClinicNum);
			if (clinicCur != null)
			{ //Should never be null because the clinic should always be in the list
				RefreshCurrentClinic(clinicCur);
			}
			CheckAlerts();
		}

		///<summary>This is will set Clinics.ClinicNum and refresh the current module.</summary>
		private void menuClinic_Click(object sender, System.EventArgs e)
		{
			if (sender.GetType() != typeof(MenuItem) && ((MenuItem)sender).Tag != null)
			{
				return;
			}
			Clinic clinicCur = (Clinic)((MenuItem)sender).Tag;
			RefreshCurrentClinic(clinicCur);
		}

		///<summary>This is used to set Clinics.ClinicNum and refreshes the current module.</summary>
		private void RefreshCurrentClinic(Clinic clinicCur)
		{
			bool isChangingClinic = (Clinics.ClinicNum != clinicCur.ClinicNum);
			Clinics.ClinicNum = clinicCur.ClinicNum;
			Text = PatientL.GetMainTitle(Patients.GetPat(CurPatNum), Clinics.ClinicNum);
			SetSmsNotificationText(doUseSignalInterval: !isChangingClinic);
			if (Prefs.GetBool(PrefName.AppointmentClinicTimeReset))
			{
				ContrAppt2.ModuleSelected(DateTimeOD.Today);
				//this actually refreshes the module, which is possibly different behavior than before the overhaul.
				//Alternatively, we might check to see of that module is selected first.
			}
			RefreshMenuClinics();
			if (isChangingClinic)
			{
				_listNormalTaskNums = null;//Will cause task preprocessing to run again.
				_listReminderTasks = null;//Will cause task preprocessing to run again.

				UserControlTasks.RefreshTasksForAllInstances(null);//Refresh tasks so any filter changes are applied immediately.
																   //In the future this may need to be enhanced to also consider refreshing other clinic specific features
				LayoutToolBar();
				FillPatientButton(Patients.GetPat(CurPatNum));//Need to do this also for disabling of buttons when no pat is selected.
			}
		}

		private void FormOpenDental_Resize(object sender, EventArgs e)
		{
			LayoutControls();
			if (Plugins.PluginsAreLoaded)
			{
				Plugins.HookAddCode(this, "FormOpenDental.FormOpenDental_Resize_end");
			}
		}

		private void FormOpenDental_ResizeEnd(object sender, EventArgs e)
		{
			LayoutControls();//for dpi change when dragging
		}

		private void FormOpenDental_SizeChanged(object sender, EventArgs e)
		{

		}

		///<summary>This used to be called much more frequently when it was an actual layout event.</summary>
		private void LayoutControls()
		{
			//Debug.WriteLine("layout");
			if (this.WindowState == FormWindowState.Minimized)
			{
				return;
			}
			if (Width < 200)
			{
				Width = 200;
			}
			int dpi = this.DeviceDpi;
			Point position = new Point(moduleBar.Width, ToolBarMain.Height);
			int width = this.ClientSize.Width - position.X;
			int height = this.ClientSize.Height - position.Y;
			if (userControlTasks1.Visible)
			{
				if (menuItemDockBottom.Checked)
				{
					if (panelSplitter.Height > 8)
					{//docking needs to be changed from right to bottom
						panelSplitter.Height = 7;
						panelSplitterLocation96dpi = new Point(0, 540);
					}
					panelSplitter.Location = new Point(position.X, Dpi.Scale(this, panelSplitterLocation96dpi.Y));
					panelSplitter.Width = width;
					panelSplitter.Visible = true;

					//phoneSmall.Visible=false;
					//phonePanel.Visible=false;
					//butBigPhones.Visible=false;
					//labelMsg.Visible=false;
					userControlTasks1.Location = new Point(position.X, panelSplitter.Bottom);
					userControlTasks1.Width = width;

					userControlTasks1.Height = this.ClientSize.Height - userControlTasks1.Top;
					height = ClientSize.Height - panelSplitter.Height - userControlTasks1.Height - ToolBarMain.Height;
				}
				else
				{//docked Right
					if (panelSplitter.Width > 8)
					{//docking needs to be changed
						panelSplitter.Width = 7;
						panelSplitterLocation96dpi = new Point(900, 0);
					}
					panelSplitter.Location = new Point(Dpi.Scale(this, panelSplitterLocation96dpi.X), position.Y);
					panelSplitter.Height = height;
					panelSplitter.Visible = true;
					userControlTasks1.Location = new Point(panelSplitter.Right, position.Y);
					userControlTasks1.Height = height;
					userControlTasks1.Width = this.ClientSize.Width - userControlTasks1.Left;
					width = ClientSize.Width - panelSplitter.Width - userControlTasks1.Width - position.X;
				}
				panelSplitter.BringToFront();
				panelSplitter.Invalidate();
				userControlTasks1.Refresh();//draw even if not resizing
			}
			else
			{
				panelSplitter.Visible = false;
			}

			splitContainerNoFlickerDashboard.Location = position;
			splitContainerNoFlickerDashboard.Height = height;
			splitContainerNoFlickerDashboard.Width = width;
			if (userControlPatientDashboard.IsInitialized && userControlPatientDashboard.ListOpenWidgets.Count > 0)
			{
				if (splitContainerNoFlickerDashboard.Panel2Collapsed)
				{
					splitContainerNoFlickerDashboard.Panel2Collapsed = false;//Make the Patient Dashboard visible.
					ResizeDashboard();
				}
			}
			else
			{
				splitContainerNoFlickerDashboard.Panel2Collapsed = true;
			}

			FillSignalButtons(null);//Refresh using cache only, do not run query, because this is fired a lot when resizing window or docted task control.
		}

		///<summary>Sets the splitter distance and Patient Dashboard height.</summary>
		private void ResizeDashboard()
		{
			int width = userControlPatientDashboard.WidgetWidth;
			splitContainerNoFlickerDashboard.SplitterDistance = splitContainerNoFlickerDashboard.Width - splitContainerNoFlickerDashboard.SplitterWidth
				- width;
			userControlPatientDashboard.Size = new Size(width, splitContainerNoFlickerDashboard.Height);
		}

		private void splitContainerNoFlickerDashboard_SplitterMoved(object sender, SplitterEventArgs e)
		{
			if (userControlPatientDashboard == null || splitContainerNoFlickerDashboard.Panel2Collapsed)
			{
				return;
			}
			if (ContrAppt2.Visible)
			{
				ContrAppt2.ModuleSelected(CurPatNum);
			}
		}

		private void panelSplitter_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			IsMouseDownOnSplitter = true;
			PointSplitterOriginalLocation = panelSplitter.Location;
			PointOriginalMouse = new Point(panelSplitter.Left + e.X, panelSplitter.Top + e.Y);
		}

		private void panelSplitter_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (!IsMouseDownOnSplitter)
			{
				return;
			}
			if (menuItemDockBottom.Checked)
			{
				int splitterNewY = PointSplitterOriginalLocation.Y + (panelSplitter.Top + e.Y) - PointOriginalMouse.Y;
				if (splitterNewY < 300)
				{
					splitterNewY = 300;//keeps it from going too high
				}
				if (splitterNewY > ClientSize.Height - 50)
				{
					splitterNewY = ClientSize.Height - panelSplitter.Height - 50;//keeps it from going off the bottom edge
				}
				//panelSplitter.Top=splitterNewY;
				panelSplitterLocation96dpi.Y = Dpi.Unscale(this, splitterNewY);
			}
			else
			{//docked right
				int splitterNewX = PointSplitterOriginalLocation.X + (panelSplitter.Left + e.X) - PointOriginalMouse.X;
				if (splitterNewX < 300)
				{
					splitterNewX = 300;//keeps it from going too far to the left
				}
				if (splitterNewX > ClientSize.Width - 60)
				{
					splitterNewX = ClientSize.Width - panelSplitter.Width - 60;//keeps it from going off the right edge
				}
				//panelSplitter.Left=splitterNewX;
				panelSplitterLocation96dpi.X = Dpi.Unscale(this, splitterNewX);
			}
			LayoutControls();
		}

		private void panelSplitter_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			IsMouseDownOnSplitter = false;
			TaskDockSavePos();
		}

		private void menuItemDockBottom_Click(object sender, EventArgs e)
		{
			menuItemDockBottom.Checked = true;
			menuItemDockRight.Checked = false;
			panelSplitter.Cursor = Cursors.HSplit;
			TaskDockSavePos();
			LayoutControls();
		}

		private void menuItemDockRight_Click(object sender, EventArgs e)
		{
			if (IsDashboardVisible)
			{
				MsgBox.Show("Tasks cannot be docked to the right when Dashboards are in use.");
				return;
			}
			menuItemDockBottom.Checked = false;
			menuItemDockRight.Checked = true;
			//included now with layoutcontrols
			panelSplitter.Cursor = Cursors.VSplit;
			TaskDockSavePos();
			LayoutControls();
		}

		///<summary>Every time user changes doc position, it will save automatically.</summary>
		private void TaskDockSavePos()
		{
			//ComputerPref computerPref = ComputerPrefs.GetForLocalComputer();
			if (menuItemDockBottom.Checked)
			{
				ComputerPrefs.LocalComputer.TaskY = panelSplitterLocation96dpi.Y; //panelSplitter.Top;
				ComputerPrefs.LocalComputer.TaskDock = 0;
			}
			else
			{
				ComputerPrefs.LocalComputer.TaskX = panelSplitterLocation96dpi.X;  //panelSplitter.Left;
				ComputerPrefs.LocalComputer.TaskDock = 1;
			}
			ComputerPrefs.Update(ComputerPrefs.LocalComputer);
		}

		public static void S_DataValid_BecomeInvalid(OpenDental.ValidEventArgs e)
		{
			_formOpenDentalS?.DataValid_BecameInvalid(e);//Can be null if called from other projects like CEMT
		}

		///<summary>This is called when any local data becomes outdated.  It's purpose is to tell the other computers to update certain local data.</summary>
		private void DataValid_BecameInvalid(OpenDental.ValidEventArgs e)
		{
			string suffix = "Refreshing Caches: ";
			ODEvent.Fire(EventCategory.Cache, suffix);
			if (e.OnlyLocal)
			{//Currently used after doing a restore from FormBackup so that the local cache is forcefully updated.
				ODEvent.Fire(EventCategory.Cache, suffix + "PrefsStartup");
				if (!PrefsStartup())
				{//??
					return;
				}
				ODEvent.Fire(EventCategory.Cache, suffix + "AllLocal");
				RefreshLocalData(InvalidType.AllLocal);//does local computer only
				return;
			}
			if (!e.ITypes.Contains(InvalidType.Appointment) //local refresh for dates is handled within ContrAppt, not here
				&& !e.ITypes.Contains(InvalidType.Task)//Tasks are not "cached" data.
				&& !e.ITypes.Contains(InvalidType.TaskPopup))
			{
				RefreshLocalData(e.ITypes);//does local computer
			}
			if (e.ITypes.Contains(InvalidType.Task) || e.ITypes.Contains(InvalidType.TaskPopup))
			{
				Plugins.HookAddCode(this, "FormOpenDental.DataValid_BecameInvalid_taskInvalidTypes");
				if (ContrChart2?.Visible ?? false)
				{
					ODEvent.Fire(EventCategory.Cache, suffix + "Chart Module");
					ContrChart2.ModuleSelected(CurPatNum);
				}
				return;//All task signals should already be sent. Sending more Task signals here would cause unnecessary refreshes.
			}

			ODEvent.Fire(EventCategory.Cache, suffix + "Inserting Signals");
			foreach (var invalidType in e.ITypes)
			{
				Signalods.Send(SignalName.Invalidate, invalidType.ToString());
			}
		}

		///<summary>Referenced at least 40 times indirectly.</summary>
		public static void S_GotoModule_ModuleSelected(ModuleEventArgs e)
		{
			_formOpenDentalS.GotoModule_ModuleSelected(e);
		}

		///<summary>This is a way that any form within Open Dental can ask the main form to refresh whatever module is currently selected.</summary>
		public static void S_RefreshCurrentModule(bool hasForceRefresh = false, bool isApptRefreshDataPat = true, bool isClinicRefresh = true)
		{
			_formOpenDentalS.RefreshCurrentModule(hasForceRefresh, isApptRefreshDataPat, isClinicRefresh);
		}

		private void GotoModule_ModuleSelected(ModuleEventArgs e)
		{
			//patient can also be set separately ahead of time instead of doing it this way:
			if (e.PatNum != 0)
			{
				if (e.PatNum != CurPatNum)
				{ //Currently selected patient changed.
					CurPatNum = e.PatNum;
					//Going to Chart Module, to specifically handle the SendToMeCreateTask_Click in FormVoiceMails to make sure Patient tab refreshes.
				}
				Patient pat = Patients.GetPat(CurPatNum);
				FillPatientButton(pat);
			}
			UnselectActive();
			allNeutral();
			if (e.ClaimNum > 0)
			{
				moduleBar.SelectedModule = e.ModuleType;
				ContrAccount2.Visible = true;
				this.ActiveControl = this.ContrAccount2;
				ContrAccount2.ModuleSelected(CurPatNum, e.ClaimNum);
			}
			else if (e.ListPinApptNums.Count != 0)
			{
				moduleBar.SelectedModule = e.ModuleType;
				ContrAppt2.Visible = true;
				this.ActiveControl = this.ContrAppt2;
				ContrAppt2.ModuleSelectedWithPinboard(CurPatNum, e.ListPinApptNums, e.DateSelected);
			}
			else if (e.SelectedAptNum != 0)
			{
				moduleBar.SelectedModule = e.ModuleType;
				ContrAppt2.Visible = true;
				this.ActiveControl = this.ContrAppt2;
				ContrAppt2.ModuleSelectedGoToAppt(e.SelectedAptNum, e.DateSelected);
			}
			else if (e.DocNum > 0)
			{
				if (Prefs.GetBool(PrefName.ImagesModuleUsesOld2020, false))
				{
					moduleBar.SelectedModule = e.ModuleType;
					ContrImages2.Visible = true;
					this.ActiveControl = this.ContrImages2;
					ContrImages2.ModuleSelected(CurPatNum, e.DocNum);
				}
				else
				{
					moduleBar.SelectedModule = e.ModuleType;
					ContrImagesJ2.Visible = true;
					this.ActiveControl = this.ContrImagesJ2;
					ContrImagesJ2.ModuleSelected(CurPatNum, e.DocNum);
				}
			}
			else if (e.ModuleType != EnumModuleType.None)
			{
				moduleBar.SelectedModule = e.ModuleType;
				SetModuleSelected();
			}
			moduleBar.Invalidate();
		}

		///<summary>Manipulates the current lightSignalGrid1 control based on the SigMessages passed in. Pass in a null list in order to simply refresh the lightSignalGrid1 control in its current state (no database call).</summary>
		private void FillSignalButtons(List<SigMessage> listSigMessages)
		{
			if (!DoFillSignalButtons())
			{
				return;
			}
			if (SigButDefList == null)
			{
				SigButDefList = SigButDefs.GetByComputer(SystemInformation.ComputerName);
			}
			int maxButton = SigButDefList.Select(x => x.ButtonIndex).DefaultIfEmpty(-1).Max() + 1;
			int lightGridHeightOld = lightSignalGrid1.Height;
			int lightGridHeightNew = Math.Min(maxButton * 25 + 1, this.ClientRectangle.Height - lightSignalGrid1.Location.Y);
			if (lightGridHeightOld != lightGridHeightNew)
			{
				lightSignalGrid1.Visible = false;//"erases" light signal grid that has been drawn on FormOpenDental
				lightSignalGrid1.Height = lightGridHeightNew;
				lightSignalGrid1.Visible = true;//re-draws light signal grid to the correct size.
			}
			if (listSigMessages == null)
			{
				return;//No new SigMessages to process.
			}
			SigButDef butDef;
			int row;
			Color color;
			bool hadErrorPainting = false;
			foreach (SigMessage sigMessage in listSigMessages)
			{
				if (sigMessage.AckDateTime.Year > 1880)
				{//process ack
					int buttonIndex = lightSignalGrid1.ProcessAck(sigMessage.SigMessageNum);
					if (buttonIndex != -1)
					{
						butDef = SigButDefs.GetByIndex(buttonIndex, SigButDefList);
						if (butDef != null)
						{
							try
							{
								PaintOnIcon(butDef.SynchIcon, Color.White);
							}
							catch
							{
								hadErrorPainting = true;
							}
						}
					}
				}
				else
				{//process normal message
					row = 0;
					color = Color.White;
					List<SigElementDef> listSigElementDefs = SigElementDefs.GetDefsForSigMessage(sigMessage);
					foreach (SigElementDef sigElementDef in listSigElementDefs)
					{
						if (sigElementDef.LightRow != 0)
						{
							row = sigElementDef.LightRow;
						}
						if (sigElementDef.LightColor.ToArgb() != Color.White.ToArgb())
						{
							color = sigElementDef.LightColor;
						}
					}
					if (row != 0 && color != Color.White)
					{
						lightSignalGrid1.SetButtonActive(row - 1, color, sigMessage);
						butDef = SigButDefs.GetByIndex(row - 1, SigButDefList);
						if (butDef != null)
						{
							try
							{
								PaintOnIcon(butDef.SynchIcon, color);
							}
							catch
							{
								hadErrorPainting = true;
							}
						}
					}
				}
			}
			if (hadErrorPainting)
			{
				MessageBox.Show("Error painting on program icon.  Probably too many non-ack'd messages.");
			}
		}

		///<summary>Refreshes the entire lightSignalGrid1 control to the current state according to the database. This is typically used when the program is first starting up or when a signal is processed for a change to the SigButDef cache.</summary>
		private void FillSignalButtons()
		{
			if (!DoFillSignalButtons())
			{
				return;
			}
			SigButDefList = SigButDefs.GetByComputer(SystemInformation.ComputerName);
			lightSignalGrid1.SetButtons(SigButDefList);
			lightSignalGrid1.Visible = (SigButDefList.Length > 0);
			FillSignalButtons(SigMessages.RefreshCurrentButState());//Get the current SigMessages from the database.
		}

		private bool DoFillSignalButtons()
		{
			if (!Security.IsUserLoggedIn)
			{
				return false;
			}
			if (!lightSignalGrid1.Visible && Programs.UsingEcwTightOrFullMode())
			{//for faster eCW loading
				return false;
			}
			return true;
		}

		///<summary>Pass in the cellNum as 1-based.</summary>
		private void PaintOnIcon(int cellNum, Color color)
		{
			Graphics g;
			if (bitmapIcon == null)
			{
				bitmapIcon = new Bitmap(16, 16);
				g = Graphics.FromImage(bitmapIcon);
				g.FillRectangle(new SolidBrush(Color.White), 0, 0, 15, 15);
				//horizontal
				g.DrawLine(Pens.Black, 0, 0, 15, 0);
				g.DrawLine(Pens.Black, 0, 5, 15, 5);
				g.DrawLine(Pens.Black, 0, 10, 15, 10);
				g.DrawLine(Pens.Black, 0, 15, 15, 15);
				//vertical
				g.DrawLine(Pens.Black, 0, 0, 0, 15);
				g.DrawLine(Pens.Black, 5, 0, 5, 15);
				g.DrawLine(Pens.Black, 10, 0, 10, 15);
				g.DrawLine(Pens.Black, 15, 0, 15, 15);
				g.Dispose();
			}
			if (cellNum == 0)
			{
				return;
			}
			g = Graphics.FromImage(bitmapIcon);
			int x = 0;
			int y = 0;
			switch (cellNum)
			{
				case 1: x = 1; y = 1; break;
				case 2: x = 6; y = 1; break;
				case 3: x = 11; y = 1; break;
				case 4: x = 1; y = 6; break;
				case 5: x = 6; y = 6; break;
				case 6: x = 11; y = 6; break;
				case 7: x = 1; y = 11; break;
				case 8: x = 6; y = 11; break;
				case 9: x = 11; y = 11; break;
			}
			g.FillRectangle(new SolidBrush(color), x, y, 4, 4);
			IntPtr intPtr = bitmapIcon.GetHicon();
			Icon icon = Icon.FromHandle(intPtr);
			Icon = (Icon)icon.Clone();
			DestroyIcon(intPtr);
			icon.Dispose();
			g.Dispose();
		}

		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		extern static bool DestroyIcon(IntPtr handle);

		private void lightSignalGrid1_ButtonClick(object sender, OpenDental.UI.ODLightSignalGridClickEventArgs e)
		{
			if (e.ActiveSignal != null)
			{//user trying to ack an existing light signal
			 //Acknowledge all sigmessages in the database which correspond with the button that was just clicked.
			 //Only acknowledge sigmessages which have a MessageDateTime prior to the last time we processed signals in the singal timer.
			 //This is so that we don't accidentally acknowledge any sigmessages that we are currently unaware of.
				SigMessages.AckButton(e.ButtonIndex + 1, Signalods.SignalLastRefreshed);
				//Immediately update the signal button instead of waiting on our instance to process its own signals.
				e.ActiveSignal.AckDateTime = DateTime.Now;
				FillSignalButtons(new List<SigMessage>() { e.ActiveSignal });//Does not run query.
				return;
			}
			if (e.ButtonDef == null || (e.ButtonDef.SigElementDefNumUser == 0 && e.ButtonDef.SigElementDefNumExtra == 0 && e.ButtonDef.SigElementDefNumMsg == 0))
			{
				return;//There is no signal to send.
			}
			//user trying to send a signal
			SigMessage sigMessage = new SigMessage();
			sigMessage.SigElementDefNumUser = e.ButtonDef.SigElementDefNumUser;
			sigMessage.SigElementDefNumExtra = e.ButtonDef.SigElementDefNumExtra;
			sigMessage.SigElementDefNumMsg = e.ButtonDef.SigElementDefNumMsg;
			SigElementDef sigElementDefUser = SigElementDefs.GetElementDef(e.ButtonDef.SigElementDefNumUser);
			if (sigElementDefUser != null)
			{
				sigMessage.ToUser = sigElementDefUser.SigText;
			}
			SigMessages.Insert(sigMessage);
			FillSignalButtons(new List<SigMessage>() { sigMessage });//Does not run query.
																	 //Let the other computers in the office know to refresh this specific light.

			Signalods.Send(SignalName.Message, null, sigMessage.SigMessageNum);
		}

		private void timerTimeIndic_Tick(object sender, System.EventArgs e)
		{
			//every minute:
			if (WindowState != FormWindowState.Minimized && ContrAppt2.Visible)
			{
				ContrAppt2.TickRefresh();
			}
		}

		///<summary>Usually set at 4 to 6 second intervals.</summary>
		private void timerSignals_Tick(object sender, System.EventArgs e)
		{
			try
			{
				SignalsTick();
			}
			catch (Exception ex)
			{
				SignalsTickExceptionHandler(ex);
			}
		}

		///<summary>Processes signals.</summary>
		private void SignalsTick()
		{
			try
			{
				// TODO: Logger.LogToPath("", LogPath.Signals, LogPhase.Start);
				//This checks if any forms are open that make us want to continue processing signals even if inactive. Currently only FormTerminal.
				if (Application.OpenForms.OfType<FormTerminal>().Count() == 0)
				{
					DateTime dtInactive = Security.DateTimeLastActivity + TimeSpan.FromMinutes((double)PrefC.GetInt(PrefName.SignalInactiveMinutes));
					if ((double)PrefC.GetInt(PrefName.SignalInactiveMinutes) != 0 && DateTime.Now > dtInactive)
					{
						_hasSignalProcessingPaused = true;
						return;
					}
				}
				if (Security.CurrentUser == null)
				{
					//User must be at the log in screen, so no need to process signals. We will need to look for shutdown signals since the last refreshed time when the user attempts to log in.
					_hasSignalProcessingPaused = true;
					return;
				}
				//if signal processing paused due to inactivity or due to Security.CurUser being null (i.e. login screen visible) and we are now going to 
				//process signals again, we will shutdown OD if:
				//1. there is a mismatch between the current software version and the program version stored in the db (ProgramVersion pref)
				//2. the UpdateInProgressOnComputerName pref is set (regardless of whether or not the computer name matches this machine name)
				//3. the CorruptedDatabase flag is set
				if (_hasSignalProcessingPaused)
				{
					string errorMsg;
					if (!IsDbConnectionSafe(out errorMsg))
					{//Running version verses ProgramVersion preference can be different in debug.
						timerSignals.Stop();
						MessageBox.Show(errorMsg);
						ProcessKillCommand();
						return;
					}
					_hasSignalProcessingPaused = false;
				}
			}
			catch
			{
				//Currently do nothing.
			}

			#region Task Preprocessing
			if (_tasksUserNum != Security.CurrentUser.Id //The user has changed since the last signal tick was run (when logoff then logon),
				|| _listReminderTasks == null || _listNormalTaskNums == null)//or first time processing signals since the program started.
			{
				// TODO: Logger.LogToPath("CurUser change", LogPath.Signals, LogPhase.Start);
				_tasksUserNum = Security.CurrentUser.Id;
				List<Task> listRefreshedTasks = Tasks.GetNewTasksThisUser(Security.CurrentUser.Id).ToList();//Get all tasks pertaining to current user.
				_listNormalTaskNums = new List<long>();
				_listReminderTasks = new List<Task>();
				_listReminderTasksOverLimit = new List<Task>();
				//List<UserOdPref> listBlockedTaskLists = UserOdPrefs.GetByUserAndFkeyType(Security.CurrentUser.Id, UserOdFkeyType.TaskListBlock);
				if (_dictAllTaskLists == null || listRefreshedTasks.Exists(x => !_dictAllTaskLists.ContainsKey(x.TaskListId)))
				{//Refresh dict if needed.
					_dictAllTaskLists = TaskLists.GetAll().ToDictionary(x => x.Id);
				}
				foreach (Task taskForUser in listRefreshedTasks)
				{//Construct the initial task meta data for the current user's tasks.
				 //If task's taskList is in dictionary and it's archived or has an archived ancestor, ignore it.
					if (_dictAllTaskLists.ContainsKey(taskForUser.TaskListId)
						&& (_dictAllTaskLists[taskForUser.TaskListId].Status == TaskListStatus.Archived
						|| TaskLists.IsAncestorTaskListArchived(ref _dictAllTaskLists, _dictAllTaskLists[taskForUser.TaskListId])))
					{
						continue;
					}
					bool isTrackedByUser = Prefs.GetBool(PrefName.TasksNewTrackedByUser);
					if (String.IsNullOrEmpty(taskForUser.ReminderGroupId))
					{//A normal task.
					 //Mimics how checkNew is set in FormTaskEdit.
						if ((isTrackedByUser && taskForUser.IsUnread) || (!isTrackedByUser && taskForUser.Status == TaskStatus.New))
						{//See def of task.IsUnread
							_listNormalTaskNums.Add(taskForUser.Id);
						}
					}
					else if (!Prefs.GetBool(PrefName.TasksUseRepeating))
					{//A reminder task (new or viewed).  Reminders not allowed if repeating tasks enabled.
						_listReminderTasks.Add(taskForUser);
						if (taskForUser.DateStart <= DateTime.Now)
						{//Do not show reminder popups for future reminders which are not due yet.
						 //Mimics how checkNew is set in FormTaskEdit.
							if ((isTrackedByUser && taskForUser.IsUnread) || (!isTrackedByUser && taskForUser.Status == TaskStatus.New))
							{//See def of task.IsUnread
							 //NOTE: POPUPS ONLY HAPPEN IF THEY ARE MARKED AS NEW. (Also, they will continue to pop up as long as they are marked "new")
								// TODO: TaskPopupHelper(taskForUser, listBlockedTaskLists);
							}
						}
					}
				}
				//Refresh the appt module to show the current list of reminders, even if the appt module not visible.  This refresh is fast.
				//The user will load the appt module eventually and these refreshes are the only updates the appointment module receives for reminders.
				ContrAppt2.RefreshReminders(_listReminderTasks);
				_dateReminderRefresh = DateTimeOD.Today;
				// TODO: Logger.LogToPath("CurUser change", LogPath.Signals, LogPhase.End);
			}
			//Check to see if a reminder task became due between the last signal interval and the current signal interval.
			else if (_listReminderTasks.FindAll(x => x.DateStart <= DateTime.Now
				 && x.DateStart >= DateTime.Now.AddSeconds(-PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs))).Count > 0)
			{
				List<Task> listDueReminderTasks = _listReminderTasks.FindAll(x => x.DateStart <= DateTime.Now
					  && x.DateStart >= DateTime.Now.AddSeconds(-PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)));
				// TODO: Logger.LogToPath("Reminder task due", LogPath.Signals, LogPhase.Start);
				//List<Signalod> listSignals = new List<Signalod>();
				//foreach (Task task in listDueReminderTasks)
				//{
				//	Signalod sig = new Signalod();
				//	sig.InvalidType = InvalidType.TaskList;
				//	sig.InvalidForeignKey = task.TaskListId;
				//	sig.Name = KeyType.Undefined;
				//	listSignals.Add(sig);
				//}
				//UserControlTasks.RefreshTasksForAllInstances(listSignals);

				// TODO:
				//List<UserOdPref> listBlockedTaskLists = UserOdPrefs.GetByUserAndFkeyType(Security.CurrentUser.Id, UserOdFkeyType.TaskListBlock);
				//foreach (Task reminderTask in listDueReminderTasks)
				//{
				//	TaskPopupHelper(reminderTask, listBlockedTaskLists);
				//}
				// TODO: Logger.LogToPath("Reminder task due", LogPath.Signals, LogPhase.End);
			}
			else if (_listReminderTasksOverLimit.Count > 0)
			{//Try to display any due reminders that previously exceeded our limit of FormTaskEdit to show.

				// TODO:
				//List<UserOdPref> listBlockedTaskLists = UserOdPrefs.GetByUserAndFkeyType(Security.CurrentUser.Id, UserOdFkeyType.TaskListBlock);
				//for (int i = _listReminderTasksOverLimit.Count - 1; i >= 0; i--)
				//{//TaskPopupHelper
				//	TaskPopupHelper(_listReminderTasksOverLimit[i], listBlockedTaskLists);
				//}
			}
			else if (_dateReminderRefresh.Date < DateTimeOD.Today)
			{
				// TODO: Logger.LogToPath("Daily reminder refresh is due", LogPath.Signals, LogPhase.Unspecified);
				//Refresh the appt module to show the current list of reminders, even if the appt module not visible.  This refresh is fast.
				//The user will load the appt module eventually and these refreshes are the only updates the appointment module receives for reminders.
				ContrAppt2.RefreshReminders(_listReminderTasks);
				_dateReminderRefresh = DateTimeOD.Today;
			}
			RefreshTasksNotification();
			#endregion Task Preprocessing

			//Signal Processing
			timerSignals.Stop();
			ODForm.SignalsTick(
				() => this.Invoke(OnShutdown),
				(listODForms, listSignals) =>
				{
					//Broadcast to all subscribed signal processors.
					this.Invoke(() =>
					{
						listODForms.ForEach(x =>
						{
							ODException.SwallowAnyException(() => x.ProcessSignals(listSignals));
						});
					});
				},
				() =>
				{
					this.Invoke(timerSignals.Start);
				}
			);
			//Be careful about doing anything that takes a long amount of computation time after the SignalsTick.
			//The UI will appear invalid for the time it takes any methods to process.
			//Post Signal Processing
			//STOP! 
			//If you are trying to do something in FormOpenDental that uses a signal, you should use FormOpenDental.OnProcessSignals() instead.
			//This Function is only for processing things at regular intervals IF IT DOES NOT USE SIGNALS.
			// TODO: Logger.LogToPath("", LogPath.Signals, LogPhase.End);
		}

		///<summary>Called when _hasSignalProcessingPaused is true and we are about to start processing signals again.  We may have missed a shutdown workstations signal, so this method will check the version, the update in progress pref, and the corrupt db pref.  Returns false if the OD
		///instance should be restarted.  The errorMsg out variable will be set to the error message for the first failed check.</summary>
		private bool IsDbConnectionSafe(out string errorMsg)
		{
			errorMsg = "";
			Prefs.RefreshCache();//this is a db call, but will only happen once when an inactive workstation is re-activated
								 //The logic below mimics parts of PrefL.CheckProgramVersion().
			Version storedVersion = new Version(Prefs.GetString(PrefName.ProgramVersion));
			Version currentVersion = new Version(Application.ProductVersion);
			if (storedVersion != currentVersion)
			{
				errorMsg = "You are attempting to run version" + " " + currentVersion.ToString(3) + ", "
					+ "but the database is using version" + " " + storedVersion.ToString(3) + ".\r\n\r\n"
					+ "You will have to restart" + " " + Prefs.GetString(PrefName.SoftwareName) + " " + "to correct the version mismatch.";
				return false;
			}
			string updateComputerName = Prefs.GetString(PrefName.UpdateInProgressOnComputerName);
			if (!string.IsNullOrEmpty(updateComputerName))
			{
				errorMsg = "An update is in progress on workstation" + ": '" + updateComputerName + "'.\r\n\r\n"
					+ "You will have to restart" + " " + Prefs.GetString(PrefName.SoftwareName) + " " + "once the update has finished.";
				return false;
			}
			return true;
		}

		///<summary>Catches an exception from signal processing and sends the first one to HQ.</summary>
		private void SignalsTickExceptionHandler(Exception ex)
		{
			//If an exception happens during processing signals, we will not close the program because the user is not trying to do anything. We will
			//send the first exception to HQ.
			if (_signalsTickException == null)
			{
				_signalsTickException = new Exception("SignalsTick exception.", ex);
				ODException.SwallowAnyException(() =>
				{
					//BugSubmissions.SubmitException(_signalsTickException,patNumCur: CurPatNum,moduleName: GetSelectedModuleName());
				});
			}
		}

		///<summary>Adds the alert items to the alert menu item.</summary>
		private void AddAlertsToMenu()
		{
			//At this point _listAlertItems and _listAlertReads should be user, clinic and subscription filtered.
			//If the counts match this means they have read all AlertItems. 
			//This will result in the 'Alerts' menu item to not be colored.
			int alertCount = _listAlertItems.Count - _listAlertReads.Count;
			if (alertCount > 99)
			{
				menuItemAlerts.Text = "Alerts" + " (99)";
			}
			else
			{
				menuItemAlerts.Text = "Alerts" + " (" + alertCount + ")";
			}
			List<MenuItem> listMenuItem = menuItemAlerts.MenuItems.Cast<MenuItem>().ToList();
			bool doRedrawMenu = false;
			foreach (MenuItem menuItem in listMenuItem)
			{
				if (menuItem == menuItemAlerts || menuItem == menuItemNoAlerts)
				{//Never want to remove these MenuItems.
					continue;
				}
				if (_listAlertItems.Any(x => x.AlertItemNum == ((AlertItem)menuItem.Tag).AlertItemNum))
				{
					continue;//A menu item already exists for this alert. May update the description later.
				}
				menuItemAlerts.MenuItems.Remove(menuItem);//New MenuItem needed for new AlertItem.
				doRedrawMenu = true;
			}
			List<ActionType> listActionTypes = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();
			listActionTypes.Sort(AlertItem.CompareActionType);
			//Loop through the _listAlertItems to either update or create our MenuItems.
			foreach (AlertItem alertItemCur in _listAlertItems)
			{
				string alertItemKey = alertItemCur.Type.ToString();
				string alertDescriptNew = AlertMenuItemHelper(alertItemCur) + alertItemCur.Description;
				MenuItem menuItem = listMenuItem.Where(x => x != menuItemAlerts && x != menuItemNoAlerts)
					.FirstOrDefault(x => alertItemCur.AlertItemNum == ((AlertItem)x.Tag).AlertItemNum);
				if (menuItem != null)
				{//Menu already has an item for this alert, so update text if needed.
					if (menuItem.Text != alertDescriptNew)
					{
						menuItem.Text = alertDescriptNew;
						doRedrawMenu = true;
					}
					continue;
				}
				//A List of sub menuitems based off of the available actions for the current AlertItem.
				List<MenuItem> listSubMenuItems = new List<MenuItem>();
				foreach (ActionType actionTypeCur in listActionTypes)
				{
					if (actionTypeCur == ActionType.None || //This should never be shown to the user. Simply a default ActionType.
						!alertItemCur.Actions.HasFlag(actionTypeCur))//Current AlertItem does not have this ActionType associated with it.
					{
						continue;
					}
					MenuItem menuItemSub = new MenuItem(AlertSubMenuItemHelper(actionTypeCur, alertItemCur));
					menuItemSub.Name = actionTypeCur.ToString();//Used in menuItemAlerts_Click(...) 
					menuItemSub.Tag = alertItemCur;//Used in menuItemAlerts_Click(...) .
					menuItemSub.Click += this.menuItemAlerts_Click;
					listSubMenuItems.Add(menuItemSub);
				}
				MenuItem itemCur = new MenuItem(alertDescriptNew, items: listSubMenuItems.ToArray());
				itemCur.Name = alertItemKey;//Used to find existing menuitems.
				itemCur.Tag = alertItemCur;//Used in menuItemAlerts_DrawItem(...) .
				itemCur.OwnerDraw = true;
				itemCur.DrawItem += this.menuItemAlerts_DrawItem;
				itemCur.MeasureItem += this.menuItemAlerts_MeasureItem;
				menuItemAlerts.MenuItems.Add(itemCur);
				doRedrawMenu = true;
			}
			menuItemAlerts.MenuItems[0].Visible = !(menuItemAlerts.MenuItems.Count > 1);//1 for 'No Alerts' MenuItem which is always there.
			if (doRedrawMenu)
			{
				InvalidateAlertsMenuItem();//Forces menuItemAlerts_DrawItem(...) logic to run again.
			}

			// TODO: Logger.LogToPath("CheckAlerts", LogPath.Signals, LogPhase.End);
		}

		///<summary>Helper function to translate the title for the given alertItem.</summary>
		private string AlertMenuItemHelper(AlertItem alertItem)
		{
			string value = "";
			switch (alertItem.Type)
			{
				case AlertType.Generic:
				case AlertType.ClinicsChangedInternal:
					break;
				case AlertType.OnlinePaymentsPending:
					value += "Pending Online Payments" + ": ";
					break;
				case AlertType.RadiologyProcedures:
					value += "Radiology Orders" + ": ";
					break;
				case AlertType.CallbackRequested:
					value += "Patient would like a callback regarding this appointment" + ": ";
					break;
				case AlertType.WebSchedNewPat:
					value += "eServices" + ": ";
					break;
				case AlertType.WebSchedNewPatApptCreated:
					value += "New Web Sched New Patient Appointment" + ": ";
					break;
				case AlertType.MaxConnectionsMonitor:
					value += "MySQL Max Connections" + ": ";
					break;
				case AlertType.WebSchedASAPApptCreated:
					value += "New Web Sched ASAP Appointment" + ": ";
					break;
				case AlertType.WebSchedRecallApptCreated:
					value += "New Web Sched Recall Appointment" + ": ";
					break;
				case AlertType.WebMailRecieved:
					value += "Unread Web Mails" + ": ";
					break;
				case AlertType.EconnectorEmailTooManySendFails:
				case AlertType.NumberBarredFromTexting:
				case AlertType.MultipleEConnectors:
				case AlertType.EConnectorDown:
				case AlertType.EConnectorError:
				case AlertType.DoseSpotProviderRegistered:
				case AlertType.DoseSpotClinicRegistered:
				case AlertType.ClinicsChanged:
				default:
					value += alertItem.Type.GetDescription() + ": ";
					break;
			}
			return value;
		}

		///<summary>Helper function to translate the title for the given alerttype and alertItem.</summary>
		private string AlertSubMenuItemHelper(ActionType actionType, AlertItem parentAlertItem)
		{
			string value = "";
			switch (actionType)
			{
				case ActionType.None://This should never happen.
					value += "None";
					break;
				case ActionType.MarkAsRead:
					value += "Mark As Read";
					break;
				case ActionType.OpenForm:
					value += "Open " + parentAlertItem.FormToOpen.GetDescription();
					break;
				case ActionType.Delete:
					value += "Delete Alert";
					break;
				case ActionType.ShowItemValue:
					value += "View Details";
					break;
			}
			return value;
		}

		///<summary>Takes one task and determines if it should popup for the current user.  Displays task popup if needed.</summary>
		private void TaskPopupHelper(Task taskPopup, /*List<UserOdPref> listBlockedTaskLists, */List<TaskNote> listNotesForTask = null)
		{
			try
			{
				//Check if application is in kiosk mode. If so, no popups should happen. 
				if (Application.OpenForms.OfType<FormTerminal>().Count() > 0)
				{
					string msg = "Kiosk mode enabled, popup blocked for TaskNum:";
					// TODO: Logger.LogToPath("", LogPath.Signals, LogPhase.Start, msg + " " + POut.Long(taskPopup.TaskNum));
					return;
				}
				// TODO: Logger.LogToPath("", LogPath.Signals, LogPhase.Start, "TaskNum: " + taskPopup.TaskNum.ToString());
				if (taskPopup.DateStart > DateTime.Now && taskPopup.ReminderType != TaskReminderType.None)
				{
					return;//Don't pop up future dated reminder tasks
				}
				//Don't pop up reminders if we reach our upper limit of open FormTaskEdit windows to avoid overwhelming users with popups.
				//Add the task to another list that temporarily holds the reminder task until it is allowed to popup.
				if (taskPopup.ReminderType != TaskReminderType.None)
				{//Is a reminder task.
					if (Application.OpenForms.OfType<FormTaskEdit>().ToList().Count >= _popupPressureReliefLimit)
					{//Open Task Edit windows over display limit.
						if (!_listReminderTasksOverLimit.Exists(x => x.Id == taskPopup.Id))
						{
							_listReminderTasksOverLimit.Add(taskPopup);//Add to list to be shown later to prevent too many windows from being open at same time.
						}
						return;//We are over the display limit for now.   Will try again later after user closes some Task Edit windows.
					}
					_listReminderTasksOverLimit.RemoveAll(x => x.Id == taskPopup.Id);//Remove from list if present.
				}
				//Even though this is triggered to popup, if this is my own task, then do not popup.
				List<TaskNote> notesForThisTask = (listNotesForTask ?? TaskNotes.GetForTask(taskPopup.Id)).OrderBy(x => x.DateModified).ToList();
				if (taskPopup.ReminderType == TaskReminderType.None)
				{//We care about notes and task sender only if it's not a reminder.
					if (notesForThisTask.Count == 0)
					{//'sender' is the usernum on the task and it's not a reminder
						if (taskPopup.UserId == Security.CurrentUser.Id)
						{
							return;
						}
					}
					else
					{//'sender' is the user on the last added note
						if (notesForThisTask[notesForThisTask.Count - 1].UserId == Security.CurrentUser.Id)
						{
							return;
						}
					}
				}
				List<TaskList> listUserTaskListSubsTrunk = TaskLists.RefreshUserTrunk(Security.CurrentUser.Id).ToList();//Get the list of directly subscribed tasklists.
				List<long> listUserTaskListSubNums = listUserTaskListSubsTrunk.Select(x => x.Id).ToList();
				bool isUserSubscribed = listUserTaskListSubNums.Contains(taskPopup.TaskListId);//First check if user is directly subscribed.
				if (!isUserSubscribed)
				{
					isUserSubscribed = listUserTaskListSubsTrunk.Any(x => TaskLists.IsAncestor(x.Id, taskPopup.TaskListId));//Check ancestors for subscription.
				}
				if (isUserSubscribed)
				{//User is subscribed to this TaskList, or one of its ancestors.
					//if (!listBlockedTaskLists.Any(x => x.Fkey == taskPopup.TaskListNum && PIn.Bool(x.ValueString)))
					//{//Subscribed and Unblocked, Show it!
					//	SoundPlayer soundplay = new SoundPlayer(Imedisoft.Properties.Resources.notify);
					//	soundplay.Play();
					//	FormTaskEdit FormT = new FormTaskEdit(taskPopup);
					//	FormT.IsPopup = true;
					//	if (taskPopup.ReminderType != TaskReminderType.NoReminder)
					//	{//If a reminder task, make an audit trail entry
					//		Tasks.TaskEditCreateLog(Permissions.TaskReminderPopup, $"Reminder task {taskPopup.TaskNum} shown to user", taskPopup);
					//	}
					//	FormT.Show();//non-modal
					//}
				}
			}
			finally
			{
				// TODO: Logger.LogToPath("", LogPath.Signals, LogPhase.End, "TaskNum: " + taskPopup.TaskNum.ToString());
			}
		}

		///<summary>MenuItem does not have an invalidate or refresh so we quickly disable and enable the menu item so that the OwnerDraw methods get called.</summary>
		private void InvalidateAlertsMenuItem()
		{
			menuItemAlerts.Enabled = false;
			menuItemAlerts.Enabled = true;
			foreach (MenuItem menuItem in menuItemAlerts.MenuItems)
			{
				menuItem.Enabled = false;
				menuItem.Enabled = true;
			}
		}

		///<summary>Called when a shutdown signal is found.</summary>
		private void OnShutdown()
		{
			if (timerSignals.Tag?.ToString() == "shutdown")
			{
				//We have already responded to the shutdown signal.
				return;
			}
			timerSignals.Enabled = false;//quit receiving signals.
			timerSignals.Tag = "shutdown";
			string msg = "";
			if (Process.GetCurrentProcess().ProcessName == "OpenDental")
			{
				msg += "All copies of Open Dental ";
			}
			else
			{
				msg += Process.GetCurrentProcess().ProcessName + " ";
			}
			msg += "will shut down in 15 seconds.  Quickly click OK on any open windows with unsaved data.";
			MsgBoxCopyPaste msgbox = new MsgBoxCopyPaste(msg);
			msgbox.Size = new Size(300, 300);
			msgbox.TopMost = true;
			msgbox.Show();
			BeginShutdownThread();
			return;
		}

		///<summary>This only contains UI signal processing. See Signalods.SignalsTick() for cache updates.</summary>
		public override void OnProcessSignals(List<Signalod> listSignals)
		{
			if (listSignals.Exists(x => x.Name == SignalName.Invalidate && x.Param1 == nameof(InvalidType.Programs)))
			{
				RefreshMenuReports();
			}

			if (listSignals.Exists(x => x.Name == SignalName.Invalidate && x.Param1 == nameof(InvalidType.Prefs)))
			{
				PrefC.InvalidateVerboseLogging();
			}

			//#region SMS Notifications
			//// TODO: Logger.LogToPath("SMS Notifications", LogPath.Signals, LogPhase.Start);
			//Signalod signalSmsCount = listSignals.OrderByDescending(x => x.Date)
			//	.FirstOrDefault(x => x.InvalidType == InvalidType.SmsTextMsgReceivedUnreadCount && x.Name == KeyType.SmsMsgUnreadCount);
			//if (signalSmsCount != null)
			//{
			//	//Provide the pre-existing value here. This will act as a flag indicating that we should not resend the signal.  This would cause infinite signal loop.
			//	SetSmsNotificationText(signalSmsCount);
			//}
			//// TODO: Logger.LogToPath("SMS Notifications", LogPath.Signals, LogPhase.End);
			//#endregion SMS Notifications


			//#region Tasks

			//List<Signalod> listSignalTasks = listSignals.FindAll(x => 
			//	x.InvalidType == InvalidType.Task || 
			//	x.InvalidType == InvalidType.TaskPopup ||
			//	x.InvalidType == InvalidType.TaskList || 
			//	x.InvalidType == InvalidType.TaskAuthor || 
			//	x.InvalidType == InvalidType.TaskPatient);

			//List<long> listEditedTaskNums = listSignalTasks.FindAll(x => x.Name == KeyType.Task).Select(x => x.InvalidForeignKey).ToList();

			//BeginTasksThread(listSignalTasks, listEditedTaskNums);

			//#endregion Tasks



			#region Appointment Module
			if (ContrAppt2.Visible)
			{
				List<long> listOpNumsVisible = ContrAppt2.GetListOpsVisible().Select(x => x.OperatoryNum).ToList();
				List<long> listProvNumsVisible = ContrAppt2.GetListProvsVisible().Select(x => x.ProvNum).ToList();
				bool isRefreshAppts = Signalods.IsApptRefreshNeeded(ContrAppt2.GetDateSelected().Date, listSignals, listOpNumsVisible, listProvNumsVisible);
				bool isRefreshScheds = Signalods.IsSchedRefreshNeeded(ContrAppt2.GetDateSelected().Date, listSignals, listOpNumsVisible, listProvNumsVisible);
				bool isRefreshPanelButtons = Signalods.IsContrApptButtonRefreshNeeded(listSignals);
				if (isRefreshAppts || isRefreshScheds)
				{
					// TODO: Logger.LogToPath("RefreshPeriod", LogPath.Signals, LogPhase.Start);
					ContrAppt2.RefreshPeriod(isRefreshAppointments: isRefreshAppts, isRefreshSchedules: isRefreshScheds);
					// TODO: Logger.LogToPath("RefreshPeriod", LogPath.Signals, LogPhase.End);
				}
				if (isRefreshPanelButtons)
				{
					ContrAppt2.RefreshModuleScreenButtonsRight();
				}
			}


			var signalTP = listSignals.FirstOrDefault(x => x.Name == SignalName.RefreshPatient && x.Param2.HasValue);

			if (ContrTreat2.Visible && signalTP != null && signalTP.Param2 == ContrTreat2.PatCur.PatNum)
			{
				RefreshCurrentModule();
			}


			#endregion Appointment Module
			#region Unfinalize Pay Menu Update

			UpdateUnfinalizedPayCount(listSignals.FindAll(x => x.Name == SignalName.UnfinalizedPayMenuUpdate));


			#endregion Unfinalize Pay Menu Update
			#region eClipboard/Kiosk
			//if (listSignals.Exists(x => x.InvalidType == InvalidType.EClipboard))
			//{
			//	EClipboardEvent.Fire(EventCategory.eClipboard);
			//}
			#endregion
			#region Refresh
			InvalidType[] arrInvalidTypes = Signalods.GetInvalidTypes(listSignals).ToArray();
			if (arrInvalidTypes.Length > 0)
			{
				RefreshLocalDataPostCleanup(arrInvalidTypes);
			}
			#endregion Refresh
			//Sig Messages must be the last code region to run in the process signals method because it changes the application icon.




			#region Sig Messages (In the manual as "Internal Messages")

			var sigMessageIds = listSignals
				.Where(
					signal => signal.Name == SignalName.Message && signal.Param2.HasValue)
				.Select(
					signal => signal.Param2.Value)
				.ToList();

			if (sigMessageIds.Count > 0)
            {
				var sigMessages = SigMessages.GetSigMessages(sigMessageIds);

				ContrManage2.LogMsgs(sigMessages);

				FillSignalButtons(sigMessages);

				BeginPlaySoundsThread(sigMessages);
            }

			#endregion Sig Messages

			Plugins.HookAddCode(this, "FormOpenDental.ProcessSignals_end", listSignals);
		}

		///<summary>Will invoke a refresh of tasks on the only instance of FormOpenDental. listRefreshedTaskNotes and listBlockedTaskLists are only used 
		///for Popup tasks, only used if listRefreshedTasks includes at least one popup task.</summary>
		public static void S_HandleRefreshedTasks(List<Signalod> listSignalTasks, List<long> listEditedTaskNums, List<Task> listRefreshedTasks,
			List<TaskNote> listRefreshedTaskNotes/*, List<UserOdPref> listBlockedTaskLists*/)
		{
			_formOpenDentalS.HandleRefreshedTasks(listSignalTasks, listEditedTaskNums, listRefreshedTasks, listRefreshedTaskNotes);
		}

		///<summary>Refreshes tasks and pops up as necessary. Invoked from thread callback in OnProcessSignals(). listRefreshedTaskNotes and 
		///listBlockedTaskLists are only used for Popup tasks, only used if listRefreshedTasks includes at least one popup task.</summary>
		private void HandleRefreshedTasks(List<Signalod> listSignalTasks, List<long> listEditedTaskNums, List<Task> listRefreshedTasks,
			List<TaskNote> listRefreshedTaskNotes/*, List<UserOdPref> listBlockedTaskLists*/)
		{
			bool hasChangedReminders = UpdateTaskMetaData(listEditedTaskNums, listRefreshedTasks);
			RefreshTasksNotification();
			RefreshOpenTasksOrPopupNewTasks(listSignalTasks, listRefreshedTasks, listRefreshedTaskNotes);
			//Refresh the appt module if reminders have changed, even if the appt module not visible.
			//The user will load the appt module eventually and these refreshes are the only updates the appointment module receives for reminders.
			if (hasChangedReminders)
			{
				ContrAppt2.RefreshReminders(_listReminderTasks);
				_dateReminderRefresh = DateTimeOD.Today;
			}
		}

		///<summary>Updates the class-wide meta data used for updating the task notification UI elements.
		///Returns true if a reminder task has changed.  Otherwise; false.</summary>
		private bool UpdateTaskMetaData(List<long> listEditedTaskNums, List<Task> listRefreshedTasks)
		{
			//Check to make sure there are edited task nums passed in and that the meta data lists have been initialized by the signal processor.
			if (listEditedTaskNums == null || _listReminderTasks == null || _listNormalTaskNums == null)
			{
				return false;//Nothing to do.
			}
			bool hasChangedReminders = false;
			for (int i = 0; i < listEditedTaskNums.Count; i++)
			{//Update the task meta data for the current user based on the query results.
				long editedTaskNum = listEditedTaskNums[i];//The tasknum mentioned in the signal.
				Task taskForUser = listRefreshedTasks?.FirstOrDefault(x => x.Id == editedTaskNum);
				Task taskNewForUser = null;
				if (taskForUser != null)
				{
					bool isTrackedByUser = Prefs.GetBool(PrefName.TasksNewTrackedByUser);
					//Mimics how checkNew is set in FormTaskEdit.
					if (((isTrackedByUser && taskForUser.IsUnread) || (!isTrackedByUser && taskForUser.Status == TaskStatus.New))//See def of task.IsUnread
																																		 //Reminders not due yet are excluded from Tasks.RefreshUserNew().
						&& (string.IsNullOrEmpty(taskForUser.ReminderGroupId) || taskForUser.DateStart <= DateTime.Now))
					{
						taskNewForUser = taskForUser;
					}
				}
				Task taskReminderOld = _listReminderTasks.FirstOrDefault(x => x.Id == editedTaskNum);
				if (taskReminderOld != null)
				{//The task is a reminder which is relevant to the current user.
					hasChangedReminders = true;
					_listReminderTasks.RemoveAll(x => x.Id == editedTaskNum);//Remove the old copy of the task.
					if (taskForUser != null)
					{//The updated reminder task is relevant to the current user.
						_listReminderTasks.Add(taskForUser);//Add the updated reminder task into the list (replacing the old reminder task).
					}
				}
				else if (_listNormalTaskNums.Contains(editedTaskNum))
				{//The task is a normal task which is relevant to the current user.
					if (taskNewForUser == null)
					{//But now the task is no longer relevant to the user.
						_listNormalTaskNums.Remove(editedTaskNum);
					}
				}
				else
				{//The edited tasknum is not currently in our meta data.
					if (taskNewForUser != null && String.IsNullOrEmpty(taskNewForUser.ReminderGroupId))
					{//A new normal task has now become relevant.
						_listNormalTaskNums.Add(editedTaskNum);
					}
					else if (taskForUser != null && !String.IsNullOrEmpty(taskForUser.ReminderGroupId))
					{//A reminder task has become relevant (new or viewed)
						hasChangedReminders = true;
						_listReminderTasks.Add(taskForUser);
					}
				}//else
			}//for
			return hasChangedReminders;
		}

		private void RefreshOpenTasksOrPopupNewTasks(List<Signalod> signals, List<Task> listRefreshedTasks, List<TaskNote> listRefreshedTaskNotes/*, List<UserOdPref> listBlockedTaskLists*/)
		{
			if (signals == null) return;




			List<long> listSignalTasksNums = signals.Where(x => x.Param2.HasValue).Select(x => x.Param2.Value).ToList();

			List<long> listTaskNumsOpen = new List<long>();

			for (int i = 0; i < Application.OpenForms.Count; i++)
			{
				Form form = Application.OpenForms[i];
				if (!(form is FormTaskEdit formTaskEdit))
				{
					continue;
				}

				if (listSignalTasksNums.Contains(formTaskEdit.TaskId))
				{
					formTaskEdit.OnTaskEdited();

					listTaskNumsOpen.Add(formTaskEdit.TaskId);
				}
			}


			//var popupTaskIds = signals
			//	.Where(
			//		signal => signal.Name == "task_popup" && signal.Param2.HasValue)
			//	.Select(
			//		signal => signal.Param2.Value);

			//foreach (var taskId in popupTaskIds)
   //         {
			//	if (listTaskNumsOpen.Contains(taskId)) continue;

			//	TaskPopupHelper(taskId);
   //         }


			//List<Task> tasksPopup = new List<Task>();
			//if (listRefreshedTasks != null)
			//{
			//	for (int i = 0; i < listRefreshedTasks.Count; i++)
			//	{//Locate any popup tasks in the returned list of tasks.
			//	 //Verify the current task is a popup task.
			//		if (!signals.Exists(x => 
			//			x.Name == KeyType.Task && x.InvalidType == InvalidType.TaskPopup && x.InvalidForeignKey == listRefreshedTasks[i].Id)
			//			|| listTaskNumsOpen.Contains(listRefreshedTasks[i].Id))
			//		{
			//			continue;//Not a popup task or is already open.
			//		}

			//		tasksPopup.Add(listRefreshedTasks[i]);
			//	}
			//}


			//for (int i = 0; i < tasksPopup.Count; i++)
			//{
			//	//Reminders sent to a subscribed tasklist will pop up prior to the reminder date/time.
			//	TaskPopupHelper(tasksPopup[i], /*listBlockedTaskLists, */listRefreshedTaskNotes?.FindAll(x => x.TaskId == tasksPopup[i].Id));
			//}

			//if (signals.Count > 0 || tasksPopup.Count > 0)
			//{
			//	UserControlTasks.RefreshTasksForAllInstances(signals);
			//}
		}

		public void ProcessKillCommand()
		{
			//It is crucial that every form be forcefully closed so that they do not stay connected to a database that has been updated to a more recent version.
			CloseOpenForms(true);
			Application.Exit();//This will call FormOpenDental's closing event which will clean up all threads that are currently running.
		}

		public static void S_ProcessKillCommand()
		{
			_formOpenDentalS.ProcessKillCommand();
		}

		private void myOutlookBar_ButtonClicked(object sender, ButtonClicked_EventArgs e)
		{
			switch (moduleBar.SelectedModule)
			{
				case EnumModuleType.Appointments:
					if (!Security.IsAuthorized(Permissions.AppointmentsModule))
					{
						e.Cancel = true;
						return;
					}
					break;
				case EnumModuleType.Family:
					if (Prefs.GetBool(PrefName.EhrEmergencyNow))
					{//if red emergency button is on
						if (Security.IsAuthorized(Permissions.EhrEmergencyAccess, true))
						{
							break;//No need to check other permissions.
						}
					}
					//Whether or not they were authorized by the special situation above,
					//they can get into the Family module with the ordinary permissions.
					if (!Security.IsAuthorized(Permissions.FamilyModule))
					{
						e.Cancel = true;
						return;
					}
					break;
				case EnumModuleType.Account:
					if (!Security.IsAuthorized(Permissions.AccountModule))
					{
						e.Cancel = true;
						return;
					}
					break;
				case EnumModuleType.TreatPlan:
					if (!Security.IsAuthorized(Permissions.TPModule))
					{
						e.Cancel = true;
						return;
					}
					break;
				case EnumModuleType.Chart:
					if (!Security.IsAuthorized(Permissions.ChartModule))
					{
						e.Cancel = true;
						return;
					}
					break;
				case EnumModuleType.Images:
					if (!Security.IsAuthorized(Permissions.ImagesModule))
					{
						e.Cancel = true;
						return;
					}
					break;
				case EnumModuleType.Manage:
					if (!Security.IsAuthorized(Permissions.ManageModule))
					{
						e.Cancel = true;
						return;
					}
					break;
			}
			UnselectActive();
			allNeutral();
			SetModuleSelected(true);
		}

		///<summary>Returns the translated name of the currently selected module for logging bug submissions.</summary>
		public string GetSelectedModuleName()
		{
			try
			{
				return moduleBar.SelectedModule.ToString();//.Buttons[moduleBar.SelectedIndex].Caption;
			}
			catch
			{
				return "";
			}
		}

		///<summary>Sets the currently selected module based on the selectedIndex of the outlook bar. If selectedIndex is -1, which might happen if user does not have permission to any module, then this does nothing.</summary>
		private void SetModuleSelected()
		{
			SetModuleSelected(false);
		}

		///<summary>Sets the currently selected module based on the selectedIndex of the outlook bar. If selectedIndex is -1, which might happen if user does not have permission to any module, then this does nothing. The menuBarClicked variable should be set to true when a module button is clicked, and should be false when called for refresh purposes.</summary>
		private void SetModuleSelected(bool menuBarClicked)
		{
			switch (moduleBar.SelectedModule)
			{
				case EnumModuleType.Appointments:
					ContrAppt2.InitializeOnStartup();
					ContrAppt2.Visible = true;
					this.ActiveControl = this.ContrAppt2;
					ContrAppt2.ModuleSelected(CurPatNum);
					break;
				case EnumModuleType.Family:
					if (HL7Defs.IsExistingHL7Enabled())
					{
						HL7Def def = HL7Defs.GetOneDeepEnabled();
						if (def.ShowDemographics == HL7ShowDemographics.Hide)
						{
							ContrFamily2Ecw.Visible = true;
							this.ActiveControl = this.ContrFamily2Ecw;
							ContrFamily2Ecw.ModuleSelected(CurPatNum);
						}
						else
						{
							ContrFamily2.InitializeOnStartup();
							ContrFamily2.Visible = true;
							this.ActiveControl = this.ContrFamily2;
							ContrFamily2.ModuleSelected(CurPatNum);
						}
					}
					else
					{
						if (Programs.UsingEcwTightMode())
						{
							ContrFamily2Ecw.Visible = true;
							this.ActiveControl = this.ContrFamily2Ecw;
							ContrFamily2Ecw.ModuleSelected(CurPatNum);
						}
						else
						{
							ContrFamily2.InitializeOnStartup();
							ContrFamily2.Visible = true;
							this.ActiveControl = this.ContrFamily2;
							ContrFamily2.ModuleSelected(CurPatNum);
						}
					}
					break;
				case EnumModuleType.Account:
					ContrAccount2.InitializeOnStartup();
					ContrAccount2.Visible = true;
					this.ActiveControl = this.ContrAccount2;
					ContrAccount2.ModuleSelected(CurPatNum);
					break;
				case EnumModuleType.TreatPlan:
					ContrTreat2.InitializeOnStartup();
					ContrTreat2.Visible = true;
					this.ActiveControl = this.ContrTreat2;
					if (menuBarClicked)
					{
						ContrTreat2.ModuleSelected(CurPatNum, true);//Set default date to true when button is clicked.
					}
					else
					{
						ContrTreat2.ModuleSelected(CurPatNum);
					}
					break;
				case EnumModuleType.Chart:
					ContrChart2.InitializeOnStartup();
					ContrChart2.Visible = true;
					this.ActiveControl = this.ContrChart2;
					if (menuBarClicked)
					{
						ContrChart2.ModuleSelectedErx(CurPatNum);
					}
					else
					{
						ContrChart2.ModuleSelected(CurPatNum, true);
					}
					TryNonPatientPopup();
					break;
				case EnumModuleType.Images:
					if (Prefs.GetBool(PrefName.ImagesModuleUsesOld2020, false))
					{
						ContrImages2.InitializeOnStartup();
						ContrImages2.Visible = true;
						this.ActiveControl = this.ContrImages2;
						ContrImages2.ModuleSelected(CurPatNum);
					}
					else
					{
						ContrImagesJ2.InitializeOnStartup();
						ContrImagesJ2.Visible = true;
						this.ActiveControl = this.ContrImagesJ2;
						ContrImagesJ2.ModuleSelected(CurPatNum);
					}
					break;
				case EnumModuleType.Manage:
					//ContrManage2.InitializeOnStartup();//This gets done earlier.
					ContrManage2.Visible = true;
					this.ActiveControl = this.ContrManage2;
					ContrManage2.ModuleSelected(CurPatNum);
					break;
			}
		}

		private void allNeutral()
		{
			ContrAppt2.Visible = false;
			ContrFamily2.Visible = false;
			ContrFamily2Ecw.Visible = false;
			ContrAccount2.Visible = false;
			ContrTreat2.Visible = false;
			ContrChart2.Visible = false;
			if (Prefs.GetBool(PrefName.ImagesModuleUsesOld2020, false))
			{
				ContrImages2.Visible = false;
			}
			else
			{
				if (ContrImagesJ2 != null)
				{//can be null on startup
					ContrImagesJ2.Visible = false;
				}
			}
			ContrManage2.Visible = false;
		}

		private void UnselectActive(bool isLoggingOff = false)
		{
			if (ContrAppt2.Visible)
			{
				ContrAppt2.ModuleUnselected();
			}
			if (ContrFamily2.Visible)
			{
				ContrFamily2.ModuleUnselected();
			}
			if (ContrFamily2Ecw.Visible)
			{
				//ContrFamily2Ecw.ModuleUnselected();
			}
			if (ContrAccount2.Visible)
			{
				ContrAccount2.ModuleUnselected();
			}
			if (ContrTreat2.Visible)
			{
				ContrTreat2.ModuleUnselected();
			}
			if (ContrChart2.Visible)
			{
				ContrChart2.ModuleUnselected(isLoggingOff);
			}
			if (Prefs.GetBool(PrefName.ImagesModuleUsesOld2020, false))
			{
				if (ContrImages2.Visible)
				{
					ContrImages2.ModuleUnselected();
				}
			}
			else
			{
				if (ContrImagesJ2.Visible)
				{
					ContrImagesJ2.ModuleUnselected();
				}
			}
		}

		///<Summary>This also passes CurPatNum down to the currently selected module (except the Manage module).  If calling from ContrAppt and RefreshModuleDataPatient was called before calling this method, set isApptRefreshDataPat=false so the get pat query isn't run twice.</Summary>
		private void RefreshCurrentModule(bool hasForceRefresh = false, bool isApptRefreshDataPat = true, bool isClinicRefresh = true, long docNum = 0)
		{
			if (ContrAppt2.Visible)
			{
				if (hasForceRefresh)
				{
					ContrAppt2.ModuleSelected(CurPatNum);
				}
				else
				{
					if (isApptRefreshDataPat)
					{//don't usually skip data refresh, only if CurPatNum was set just prior to calling this method
						ContrAppt2.RefreshModuleDataPatient(CurPatNum);
					}
					ContrAppt2.RefreshModuleScreenButtonsRight();
				}
			}
			if (ContrFamily2.Visible)
			{
				ContrFamily2.ModuleSelected(CurPatNum);
			}
			if (ContrFamily2Ecw.Visible)
			{
				ContrFamily2Ecw.ModuleSelected(CurPatNum);
			}
			if (ContrAccount2.Visible)
			{
				ContrAccount2.ModuleSelected(CurPatNum);
			}
			if (ContrTreat2.Visible)
			{
				ContrTreat2.ModuleSelected(CurPatNum);
			}
			if (ContrChart2.Visible)
			{
				ContrChart2.ModuleSelected(CurPatNum, isClinicRefresh);
			}
			if (Prefs.GetBool(PrefName.ImagesModuleUsesOld2020, false))
			{
				if (ContrImages2.Visible)
				{
					ContrImages2.ModuleSelected(CurPatNum, docNum);
				}
			}
			else
			{
				if (ContrImagesJ2.Visible)
				{
					ContrImagesJ2.ModuleSelected(CurPatNum, docNum);
				}
			}
			if (ContrManage2.Visible)
			{
				ContrManage2.ModuleSelected(CurPatNum);
			}
			userControlTasks1.RefreshPatTicketsIfNeeded();
		}

		/// <summary>sends function key presses to the appointment module and chart module</summary>
		private void FormOpenDental_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			//This suppresses the base windows functionality for giving focus to the main menu on F10. See Job 8289
			if (e.KeyCode == Keys.F10)
			{
				e.SuppressKeyPress = true;
			}
			if (ContrAppt2.Visible && e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F12)
			{
				ContrAppt2.FunctionKeyPress(e.KeyCode);
				return;
			}
			if (ContrChart2.Visible && e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F12)
			{
				ContrChart2.FunctionKeyPressContrChart(e.KeyCode);
				return;
			}
			//Ctrl-Alt-R is supposed to show referral window, but it doesn't work on some computers.
			//so we're also going to use Ctrl-X to show the referral window.
			if (CurPatNum != 0
				&& (e.Modifiers == (Keys.Alt | Keys.Control) && e.KeyCode == Keys.R)
					|| (e.Modifiers == Keys.Control && e.KeyCode == Keys.X))
			{
				FormReferralsPatient FormRE = new FormReferralsPatient();
				FormRE.PatNum = CurPatNum;
				FormRE.ShowDialog();
			}
			Plugins.HookAddCode(this, "FormOpenDental_KeyDown_end", e);
		}

		///<summary>This method stops all (local) timers and displays a connection lost window that will let users attempt to reconnect.
		///At any time during the lifespan of the application connection to the database can be lost for unknown reasons.
		///When anything spawned by FormOpenDental (main thread) tries to connect to the database and fails, this event will get fired.</summary>
		private void DataConnection_ConnectionLost(DataConnectionEventArgs e)
		{
			if (InvokeRequired)
			{
				this.BeginInvoke(() => DataConnection_ConnectionLost(e));
				return;
			}
			if (e == null || e.EventType != EventCategory.DataConnection || e.IsConnectionRestored)
			{
				return;
			}
			BeginDataConnectionLostThread(e);
		}

		///<summary>This method stops all (local) timers and displays a bad credentials window that will let users attempt to login again.  This is to
		///handle the situation where a user is logged into multiple computers via middle tier and changes their password on 1 connection.  The other
		///connection(s) would attempt to access the database using the old password (for signal refreshes etc) and lock the user's account for too many
		///failed attempts.  FormLoginFailed will not allow a different user to login, only the current user or exit the program.</summary>
		private void DataConnection_CredentialsFailedAfterLogin(ODEventArgs e)
		{
			if (InvokeRequired)
			{
				this.BeginInvoke(() => DataConnection_CredentialsFailedAfterLogin(e));
				return;
			}
			if (e != null && e.EventType != EventCategory.ServiceCredentials)
			{
				return;
			}
			if (Security.CurrentUser == null)
			{
				Environment.Exit(0);//shouldn't be possible, would have to have a user logged in to get here, but just in case, exit the program
			}
			if (_formLoginFailed != null && !_formLoginFailed.IsDisposed)
			{//_formLoginFailed already displayed, wait for _formLoginFailed to close
				return;
			}
			try
			{
				SetTimersAndThreads(false);//Safe to stop timers since this method was invoked on the main thread if required.
				Security.CurrentUser = null;
				string errorMsg = (string)e.Tag;
				_formLoginFailed = new FormLoginFailed(errorMsg);
				_formLoginFailed.ShowDialog();
				if (_formLoginFailed.DialogResult == DialogResult.Cancel)
				{
					Environment.Exit(0);
				}
				SetTimersAndThreads(true);//Safe to start timers since this method was invoked on the main thread if required.
				Security.DateTimeLastActivity = DateTime.Now;
			}
			catch
			{
				throw;
			}
			finally
			{
				_formLoginFailed = null;
			}
		}

		//public static void S_TaskGoTo(TaskObjectType taskOT, long keyNum)
		//{
		//	_formOpenDentalS.TaskGoTo(taskOT, keyNum);
		//}

		//private void TaskGoTo(TaskObjectType taskOT, long keyNum)
		//{
		//	if (taskOT == TaskObjectType.None || keyNum == 0)
		//	{
		//		return;
		//	}
		//	if (taskOT == TaskObjectType.Patient)
		//	{
		//		CurPatNum = keyNum;
		//		Patient pat = Patients.GetPat(CurPatNum);
		//		RefreshCurrentModule();
		//		FillPatientButton(pat);
		//	}
		//	if (taskOT == TaskObjectType.Appointment)
		//	{
		//		Appointment apt = Appointments.GetOneApt(keyNum);
		//		if (apt == null)
		//		{
		//			MessageBox.Show("Appointment has been deleted, so it's not available.");
		//			return;
		//		}
		//		DateTime dateSelected = DateTime.MinValue;
		//		if (apt.AptStatus == ApptStatus.Planned || apt.AptStatus == ApptStatus.UnschedList)
		//		{
		//			//I did not add feature to put planned or unsched apt on pinboard.
		//			MessageBox.Show("Cannot navigate to appointment.  Use the Other Appointments button.");
		//			//return;
		//		}
		//		else
		//		{
		//			dateSelected = apt.AptDateTime;
		//		}
		//		CurPatNum = apt.PatNum;//OnPatientSelected(apt.PatNum);
		//		FillPatientButton(Patients.GetPat(CurPatNum));
		//		GotoModule.GotoAppointment(dateSelected, apt.AptNum);
		//	}
		//}

		#region MenuEvents
		private void menuItemLogOff_Click(object sender, System.EventArgs e)
		{
			NullUserCheck("menuItemLogOff_Click");
			if (!AreYouSurePrompt(Security.CurrentUser.Id, "Are you sure you would like to log off?"))
			{
				return;
			}
			LogOffNow(false);
		}

		/// <summary>
		/// First checks if users have a message prompt turned off for logging off/closing the program. If they don't, then a message is passed in that
		/// corresponds with intent (ie logging off vs closing the program)	/// </summary>
		/// <param name="userNum">Used to check if user has "Close/Log off" message preference turned off under File->User Settings</param>
		/// <param name="message">Used for passing in message that lets User know what is being done</param>
		private bool AreYouSurePrompt(long userNum, string message)
		{
			if (UserPreference.GetBool(UserPreferenceName.SuppressLogOffMessage) == false)
			{
				using var inputBox = new InputBox(message, "Do not show me this message again.", true, new Point(0, 40));

				if (inputBox.ShowDialog(this) == DialogResult.Cancel)
				{
					return false;
				}

				UserPreference.Set(UserPreferenceName.SuppressLogOffMessage, inputBox.checkBoxResult.Checked);
			}

			return true;
		}


		#endregion MenuEvents

		#region File

		//File
		private void menuItemPassword_Click(object sender, EventArgs e)
		{
			SecurityL.ChangePassword(false);
		}

		private void menuItemUserEmailAddress_Click(object sender, EventArgs e)
		{
			EmailAddress emailAddressCur = EmailAddresses.GetForUser(Security.CurrentUser.Id);
			FormEmailAddressEdit formEAE;
			if (emailAddressCur == null)
			{
				formEAE = new FormEmailAddressEdit(Security.CurrentUser.Id);
			}
			else
			{
				formEAE = new FormEmailAddressEdit(emailAddressCur);
			}
			formEAE.ShowDialog();
		}

		private void menuItemUserSettings_Click(object sender, EventArgs e)
		{
			FormUserSetting FormUS = new OpenDental.FormUserSetting();
			FormUS.ShowDialog();
		}

		private void menuItemPrinter_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormPrinterSetup FormPS = new FormPrinterSetup();
			FormPS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Printers");
		}

		private void menuItemGraphics_Click(object sender, EventArgs e)
		{
			//if(ToothChartRelay.IsSparks3DPresent){
			//	MessageBox.Show("You are using the new 3D tooth chart (Sparks3D.dll), so the Graphics setup window is not needed.");
			//	return;
			//}
			if (!Security.IsAuthorized(Permissions.GraphicsEdit))
			{
				return;
			}
			Cursor = Cursors.WaitCursor;
			FormGraphics fg = new FormGraphics();
			fg.ShowDialog();
			Cursor = Cursors.Default;
			if (fg.DialogResult == DialogResult.OK)
			{
				ContrChart2.InitializeLocalData();
				RefreshCurrentModule();
			}
		}

		private void menuItemConfig_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.ChooseDatabase))
			{
				return;
			}

			SecurityLogs.MakeLogEntry(Permissions.ChooseDatabase, 0, "");//make the entry before switching databases.

			using (var formChooseDatabase = new FormChooseDatabase(true))
			{
				if (formChooseDatabase.ShowDialog() != DialogResult.OK)
				{
					return;
				}
			}

			CurPatNum = 0;
			if (!PrefsStartup())
			{
				return;
			}

			RefreshLocalData(InvalidType.AllLocal);

			UnselectActive();//Deselect the currently Visible module.
			allNeutral();//Set all modules invisible.
						 //The following 2 methods mimic RefreshCurrentModule()
			SetModuleSelected(true);//Reselect the previously selected module, UI is reset to same state as when program starts.
			userControlTasks1.RefreshPatTicketsIfNeeded();
			FillPatientButton(null);
		}

		private void menuItemExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		#endregion File

		#region Setup

		//FormBackupJobsSelect FormBJS=new FormBackupJobsSelect();
		//FormBJS.ShowDialog();	

		//Setup
		private void menuItemApptFieldDefs_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormApptFieldDefs FormA = new FormApptFieldDefs();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Appointment Field Defs");
		}

		private void menuItemApptRules_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormApptRules FormA = new FormApptRules();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Appointment Rules");
		}

		private void menuItemApptTypes_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormApptTypes FormA = new FormApptTypes();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Appointment Types");
		}

		private void menuItemApptViews_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormApptViews FormAV = new FormApptViews();
			FormAV.ShowDialog();
			RefreshCurrentModule(true);
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Appointment Views");
		}

		private void menuItemAlertCategories_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.SecurityAdmin))
			{
				return;
			}
			FormAlertCategorySetup FormACS = new FormAlertCategorySetup();
			FormACS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin, 0, "Alert Categories");
		}

		private void menuItemAutoCodes_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormAutoCode FormAC = new FormAutoCode();
			FormAC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Auto Codes");
		}

		private void menuItemAutomation_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormAutomation FormA = new FormAutomation();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Automation");
		}

		private void menuItemAutoNotes_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.AutoNoteQuickNoteEdit))
			{
				return;
			}
			FormAutoNotes FormA = new FormAutoNotes();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.AutoNoteQuickNoteEdit, 0, "Auto Notes Setup");
		}

		private void menuItemClaimForms_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormClaimForms FormCF = new FormClaimForms();
			FormCF.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Claim Forms");
		}

		private void menuItemClearinghouses_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormClearinghouses FormC = new FormClearinghouses();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Clearinghouses");
		}

		private void menuItemCloudManagement_Click(object sender, EventArgs e)
		{
			FormChangeCloudPassword formChangeCloudPassword = new FormChangeCloudPassword();
			formChangeCloudPassword.ShowDialog();
		}

		private void menuItemDiscountPlans_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormDiscountPlans FormDP = new FormDiscountPlans();
			FormDP.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Discount Plans");
		}

		private void menuItemComputers_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormComputers FormC = new FormComputers();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Computers");
		}

		private void menuItemDataPath_Click(object sender, System.EventArgs e)
		{
			//security is handled from within the form.
			FormPath FormP = new FormPath();
			FormP.ShowDialog();
			CheckCustomReports();
			this.RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Data Path");
		}

		private void menuItemDefaultCCProcs_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormDefaultCCProcs FormD = new FormDefaultCCProcs();
			FormD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Default CC Procedures");
		}

		private void menuItemDefinitions_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormDefinitions FormD = new FormDefinitions(DefCat.AccountColors);//just the first cat.
			FormD.ShowDialog();
			RefreshCurrentModule(true);
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Definitions");
		}

		private void menuItemDentalSchools_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormDentalSchoolSetup FormDSS = new FormDentalSchoolSetup();
			FormDSS.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Dental Schools");
		}

		private void menuItemDisplayFields_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormDisplayFieldCategories FormD = new FormDisplayFieldCategories();
			FormD.ShowDialog();
			RefreshCurrentModule(true);
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Display Fields");
		}

		private void menuItemEnterprise_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormEnterpriseSetup FormES = new FormEnterpriseSetup();
			if (FormES.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			if (PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs) == 0)
			{
				timerSignals.Enabled = false;
				_hasSignalProcessingPaused = true;
			}
			else
			{
				timerSignals.Interval = PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs) * 1000;
				timerSignals.Enabled = true;
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Enterprise");
		}

		private void menuItemEmail_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormEmailAddresses FormEA = new FormEmailAddresses();
			FormEA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Email");
		}

		private void menuItemEHR_Click(object sender, EventArgs e)
		{
			//if(!Security.IsAuthorized(Permissions.Setup)) {
			//  return;
			//}
			FormEhrSetup FormE = new FormEhrSetup();
			FormE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "EHR");
		}

		private void menuItemFeeScheds_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.FeeSchedEdit))
			{
				return;
			}
			FormFeeScheds FormF = new FormFeeScheds(false);
			FormF.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.FeeSchedEdit, 0, "Fee Schedules");
		}

		private void menuFeeSchedGroups_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.FeeSchedEdit))
			{
				return;
			}
			//Users that are clinic restricted are not allowed to setup Fee Schedule Groups.
			if (Security.CurrentUser.ClinicIsRestricted)
			{
				MessageBox.Show("You are restricted from accessing certain clinics.  Only user without clinic restrictions can edit Fee Schedule Groups.");
				return;
			}
			FormFeeSchedGroups FormF = new FormFeeSchedGroups();
			FormF.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.FeeSchedEdit, 0, "Fee Schedule Groups");
		}

		private void menuItemFHIR_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			Cursor = Cursors.WaitCursor;
			FormFHIRSetup FormFS = new FormFHIRSetup();
			FormFS.ShowDialog();
			Cursor = Cursors.Default;
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "FHIR");
		}

		private void menuItemHL7_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormHL7Defs FormH = new FormHL7Defs();
			FormH.CurPatNum = CurPatNum;
			FormH.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "HL7");
		}

		private void menuItemScanning_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormImagingSetup FormI = new FormImagingSetup();
			FormI.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Imaging");
		}

		private void menuItemInsCats_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormInsCatsSetup FormE = new FormInsCatsSetup();
			FormE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Insurance Categories");
		}

		private void menuItemInsFilingCodes_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormInsFilingCodes FormF = new FormInsFilingCodes();
			FormF.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Insurance Filing Codes");
		}

		private void menuItemLaboratories_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			if (Plugins.HookMethod(this, "FormOpenDental.menuItemLaboratories_Click"))
			{
				return;
			}
			FormLaboratories FormL = new FormLaboratories();
			FormL.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Laboratories");
		}

		private void menuItemLetters_Click(object sender, EventArgs e)
		{
			FormLetters FormL = new FormLetters();
			FormL.ShowDialog();
		}

		private void menuItemMessaging_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormMessagingSetup FormM = new FormMessagingSetup();
			FormM.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Messaging");
		}

		private void menuItemMessagingButs_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormMessagingButSetup FormM = new FormMessagingButSetup();
			FormM.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Messaging");
		}

		private void menuItemMisc_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormMisc FormM = new FormMisc();
			if (FormM.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			if (PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs) == 0)
			{
				timerSignals.Enabled = false;
				_hasSignalProcessingPaused = true;
			}
			else
			{
				timerSignals.Interval = PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs) * 1000;
				timerSignals.Enabled = true;
			}

			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Misc");
		}

		private void menuItemModules_Click(object sender, EventArgs e)
		{
			LaunchModuleSetupWithTab(0);//Default to Appts tab.
		}

		private void menuItemMounts_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormMountDefs formMountDefs = new FormMountDefs();
			formMountDefs.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Mounts");
		}

		private void menuItemSensors_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			try
			{
				Assembly assemblyImaging = Assembly.Load("OpenDentalImaging");
				Type typeFormSensors = assemblyImaging.GetType("OpenDentalImaging.FormSensors");
				Form formSensors = (Form)(Activator.CreateInstance(typeFormSensors));
				formSensors.ShowDialog();
			}
			catch (Exception ex)
			{
				MsgBox.Show(ex.Message);
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Sensors");
		}

		private void menuItemOrtho_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormOrthoSetup FormOS = new FormOrthoSetup();
			FormOS.ShowDialog();
		}

		private void menuItemPreferencesAppts_Click(object sender, EventArgs e)
		{
			LaunchModuleSetupWithTab(0);
		}

		private void menuItemPreferencesFamily_Click(object sender, EventArgs e)
		{
			LaunchModuleSetupWithTab(1);
		}

		private void menuItemPreferencesAccount_Click(object sender, EventArgs e)
		{
			LaunchModuleSetupWithTab(2);
		}

		private void menuItemPreferencesTreatPlan_Click(object sender, EventArgs e)
		{
			LaunchModuleSetupWithTab(3);
		}

		private void menuItemPreferencesChart_Click(object sender, EventArgs e)
		{
			LaunchModuleSetupWithTab(4);
		}

		private void menuItemPreferencesImaging_Click(object sender, EventArgs e)
		{
			LaunchModuleSetupWithTab(5);
		}

		private void menuItemPreferencesManage_Click(object sender, EventArgs e)
		{
			LaunchModuleSetupWithTab(6);
		}

		///<summary>Checks setup permission, launches the module setup window with the specified tab and then makes an audit entry.
		///This is simply a helper method because every preferences menu item will do the exact same code.</summary>
		private void LaunchModuleSetupWithTab(int selectedTab)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormModuleSetup FormM = new FormModuleSetup(selectedTab);
			if (FormM.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			FillPatientButton(Patients.GetPat(CurPatNum));
			RefreshCurrentModule(true);
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Modules");
		}

		private void menuItemOperatories_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormOperatories FormO = new FormOperatories();
			FormO.ContrApptRef = ContrAppt2;
			FormO.ShowDialog();
			if (FormO.ListConflictingAppts.Count > 0)
			{
				FormApptConflicts FormAC = new FormApptConflicts(FormO.ListConflictingAppts);
				FormAC.Show();
				FormAC.BringToFront();
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Operatories");
		}

		private void menuItemPatFieldDefs_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormPatFieldDefs FormP = new FormPatFieldDefs();
			FormP.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Patient Field Defs");
		}

		private void menuItemPayerIDs_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormElectIDs FormE = new FormElectIDs();
			FormE.IsSelectMode = false;
			FormE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Payer IDs");
		}

		private void menuItemPractice_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormPractice FormPr = new FormPractice();
			FormPr.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Practice Info");
			if (FormPr.DialogResult != DialogResult.OK)
			{
				return;
			}
			moduleBar.RefreshButtons();
			RefreshCurrentModule();
		}

		private void menuItemProblems_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormDiseaseDefs FormD = new FormDiseaseDefs();
			FormD.ShowDialog();
			//RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Disease Defs");
		}

		private void menuItemProcedureButtons_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormProcButtons FormPB = new FormProcButtons();
			FormPB.Owner = this;
			FormPB.ShowDialog();
			SetModuleSelected();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Procedure Buttons");
		}

		private void menuItemLinks_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormProgramLinks FormPL = new FormProgramLinks();
			FormPL.ShowDialog();
			ContrChart2.InitializeLocalData();//for eCW
			RefreshMenuReports();
			if (CurPatNum > 0)
			{
				Patient pat = Patients.GetPat(CurPatNum);
				FillPatientButton(pat);
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Program Links");
		}

		/*
		private void menuItem_ProviderAllocatorSetup_Click(object sender,EventArgs e) {
			// Check Permissions
			if(!Security.IsAuthorized(Permissions.Setup)) {
				// Failed security prompts message box. Consider adding overload to not show message.
				//MessageBox.Show("Not Authorized to Run Setup for Provider Allocation Tool");
				return;
			}
			Reporting.Allocators.MyAllocator1.FormInstallAllocator_Provider fap = new OpenDental.Reporting.Allocators.MyAllocator1.FormInstallAllocator_Provider();
			fap.ShowDialog();
		}*/

		private void menuItemAsapList_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormAsapSetup FormAS = new FormAsapSetup();
			FormAS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "ASAP List Setup");
		}

		private void menuItemConfirmations_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormConfirmationSetup FormCS = new FormConfirmationSetup();
			FormCS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Confirmation Setup");
		}

		private void menuItemInsVerify_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormInsVerificationSetup FormIV = new FormInsVerificationSetup();
			FormIV.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Insurance Verification");
		}

		private void menuItemQuestions_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormQuestionDefs FormQ = new FormQuestionDefs();
			FormQ.ShowDialog();
			//RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Questionnaire");
		}

		private void menuItemRecall_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormRecallSetup FormRS = new FormRecallSetup();
			FormRS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Recall");
		}

		private void menuItemRecallTypes_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormRecallTypes FormRT = new FormRecallTypes();
			FormRT.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Recall Types");
		}

		private void menuItemReactivation_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormReactivationSetup FormRS = new FormReactivationSetup();
			FormRS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Reactivation");
		}

		private void menuItemReports_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormReportSetup FormRS = new FormReportSetup(0, false);
			FormRS.ShowDialog();
		}

		private void menuItemRequiredFields_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormRequiredFields FormRF = new FormRequiredFields();
			FormRF.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Required Fields");
		}

		private void menuItemRequirementsNeeded_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormReqNeededs FormR = new FormReqNeededs();
			FormR.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Requirements Needed");
		}

		private void menuItemSched_Click(object sender, EventArgs e)
		{
			//anyone should be able to view. Security must be inside schedule window.
			//if(!Security.IsAuthorized(Permissions.Schedules)) {
			//	return;
			//}
			FormSchedule FormS = new FormSchedule();
			FormS.ShowDialog();
			//SecurityLogs.MakeLogEntry(Permissions.Schedules,0,"");
		}

		private void MenuItemScheduledProcesses_Click(object sender, EventArgs e)
		{
			FormScheduledProcesses formScheduledProcesses = new FormScheduledProcesses();
			formScheduledProcesses.ShowDialog();
		}

		/*private void menuItemBlockoutDefault_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Blockouts)) {
				return;
			}
			FormSchedDefault FormSD=new FormSchedDefault(ScheduleType.Blockout);
			FormSD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Default");
		}*/

		public static void S_MenuItemSecurity_Click(object sender, EventArgs e)
		{
			_formOpenDentalS.menuItemSecuritySettings_Click(sender, e);
		}

		private void menuItemSecuritySettings_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.SecurityAdmin))
			{
				return;
			}
			FormSecurity FormS = new FormSecurity();
			FormS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin, 0, "Security Window");
			if (!PrefC.HasClinicsEnabled)
			{//clinics not enabled, refresh current module and return
				RefreshCurrentModule();
				return;
			}
			//clinics is enabled
			long clinicNumOld = Clinics.ClinicNum;
			if (Security.CurrentUser.ClinicIsRestricted)
			{
				Clinics.ClinicNum = Security.CurrentUser.ClinicId;
			}
			Text = PatientL.GetMainTitle(Patients.GetPat(CurPatNum), Clinics.ClinicNum);
			SetSmsNotificationText(doUseSignalInterval: (clinicNumOld == Clinics.ClinicNum));//Clinic selection changed, update sms notifications.
			RefreshMenuClinics();//this calls ModuleSelected, so no need to call RefreshCurrentModule
			RefreshMenuDashboards();
		}

		private void menuItemAddUser_Click(object sender, EventArgs e)
		{
			bool isAuthorizedAddNewUser = Security.IsAuthorized(Permissions.AddNewUser, true);
			bool isAuthorizedSecurityAdmin = Security.IsAuthorized(Permissions.SecurityAdmin, true);
			if (!(isAuthorizedAddNewUser || isAuthorizedSecurityAdmin))
			{
				MessageBox.Show("Not authorized to add a new user.");
				return;
			}
			if (Prefs.GetLong(PrefName.DefaultUserGroup) == 0)
			{
				if (isAuthorizedSecurityAdmin)
				{
					//Prompt to go to form.
					var result = MessageBox.Show(this,
						"Default user group is not set. Would you like to set the default user group now?",
						"Default user group",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question);

					if (result == DialogResult.Yes)
					{
						FormGlobalSecurity FormGS = new FormGlobalSecurity();
						FormGS.ShowDialog();//No refresh needed; Signals sent from this form.
					}
				}
				else
				{
					//Using verbage similar to that found in the manual for describing how to navigate to a window in the program.
					MessageBox.Show(this,
						"Default user group is not set. A user with the SecurityAdmin permission must set a default user group. " +
						"To view the default user group, in the Main Menu, click Setup, Security, Security Settings, Global Security Settings.",
						"Default user group",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
				}
				return;
			}
			FormUserEdit FormUE = new FormUserEdit(new Userod(), true);
			FormUE.IsNew = true;
			FormUE.ShowDialog();
		}

		private void menuItemSheets_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormSheetDefs FormSD = new FormSheetDefs();
			FormSD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Sheets");
		}

		//This shows as "Show Features"
		private void menuItemEasy_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormShowFeatures FormE = new FormShowFeatures();
			FormE.ShowDialog();
			ContrAccount2.LayoutToolBar();//for repeating charges
			RefreshCurrentModule(true);
			//Show enterprise setup if it was enabled
			menuItemEnterprise.Visible = Prefs.GetBool(PrefName.ShowFeatureEnterprise);
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Show Features");
		}

		private void menuItemSpellCheck_Click(object sender, EventArgs e)
		{
			FormSpellCheck FormD = new FormSpellCheck();
			FormD.ShowDialog();
		}

		private void menuItemTimeCards_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormTimeCardSetup FormTCS = new FormTimeCardSetup();
			FormTCS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Time Card Setup");
		}

		private void menuItemTask_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormTaskPreferences formTaskSetup = new FormTaskPreferences();
			if (formTaskSetup.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			if (userControlTasks1.Visible)
			{
				userControlTasks1.InitializeOnStartup();
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Task");
		}

		private void menuItemQuickPasteNotes_Click(object sender, EventArgs e)
		{
			FormQuickPaste formQP = new FormQuickPaste(true);
			formQP.QuickType = QuickPasteType.None;
			formQP.ShowDialog();
		}

		private void menuItemWebForm_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormWebFormSetup formWFS = new FormWebFormSetup();
			formWFS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Web Forms Setup");
		}

		#endregion

		#region Lists

		//Lists
		private void menuItemProcCodes_Click(object sender, System.EventArgs e)
		{
			//security handled within form
			using (FormProcCodes FormP = new FormProcCodes(true))
			{
				FormP.ShowDialog();
			}
		}

		private void menuItemAllergies_Click(object sender, EventArgs e)
		{
			new FormAllergySetup().ShowDialog();
		}

		private void menuItemClinics_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormClinics FormC = new FormClinics();
			FormC.IncludeHQInList = true;
			FormC.IsMultiSelect = true;
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Clinics");
			//this menu item is only visible if the clinics show feature is enabled (!EasyNoClinics)
			if (Clinics.GetDesc(Clinics.ClinicNum) == "")
			{//will be empty string if ClinicNum is not valid, in case they deleted the clinic
				Clinics.ClinicNum = Security.CurrentUser.ClinicId;
				SetSmsNotificationText(doUseSignalInterval: true);//Update sms notification text.
				Text = PatientL.GetMainTitle(Patients.GetPat(CurPatNum), Clinics.ClinicNum);
			}
			RefreshMenuClinics();
			//reset the main title bar in case the user changes the clinic description for the selected clinic
			Patient pat = Patients.GetPat(CurPatNum);
			Text = PatientL.GetMainTitle(pat, Clinics.ClinicNum);
			//reset the tip text in case the user changes the clinic description
		}

		private void menuItemContacts_Click(object sender, System.EventArgs e)
		{
			FormContacts FormC = new FormContacts();
			FormC.ShowDialog();
		}

		private void menuItemCounties_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormCounties FormC = new FormCounties();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Counties");
		}

		private void menuItemSchoolClass_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormSchoolClasses FormS = new FormSchoolClasses();
			FormS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Dental School Classes");
		}

		private void menuItemSchoolCourses_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormSchoolCourses FormS = new FormSchoolCourses();
			FormS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Dental School Courses");
		}

		private void menuItemEmployees_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormEmployeeSelect FormEmp = new FormEmployeeSelect();
			FormEmp.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Employees");
		}

		private void menuItemEmployers_Click(object sender, System.EventArgs e)
		{
			FormEmployers FormE = new FormEmployers();
			FormE.ShowDialog();
		}

		private void menuItemInstructors_Click(object sender, System.EventArgs e)
		{
			/*if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormInstructors FormI=new FormInstructors();
			FormI.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental School Instructors");*/
		}

		private void menuItemCarriers_Click(object sender, System.EventArgs e)
		{
			FormCarriers FormC = new FormCarriers();
			FormC.ShowDialog();
			RefreshCurrentModule();
		}

		private void menuItemInsPlans_Click(object sender, System.EventArgs e)
		{
			FormInsPlans FormIP = new FormInsPlans();
			FormIP.ShowDialog();
			RefreshCurrentModule(true);
		}

		private void menuItemLabCases_Click(object sender, EventArgs e)
		{
			FormLabCases FormL = new FormLabCases();
			FormL.ShowDialog();
			if (FormL.GoToAptNum != 0)
			{
				Appointment apt = Appointments.GetOneApt(FormL.GoToAptNum);
				Patient pat = Patients.GetPat(apt.PatNum);
				S_Contr_PatientSelected(pat, false);
				//OnPatientSelected(pat.PatNum,pat.GetNameLF(),pat.Email!="",pat.ChartNumber);
				GotoModule.GotoAppointment(apt.AptDateTime, apt.AptNum);
			}
		}

		private void menuItemMedications_Click(object sender, System.EventArgs e)
		{
			FormMedications FormM = new FormMedications();
			FormM.ShowDialog();
		}

		private void menuItemPharmacies_Click(object sender, EventArgs e)
		{
			FormPharmacies FormP = new FormPharmacies();
			FormP.ShowDialog();
		}

		private void menuItemProviders_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Providers, true) && !Security.IsAuthorized(Permissions.AdminDentalStudents, true))
			{
				MessageBox.Show("Not authorized for" + "\r\n"
					+ GroupPermissions.GetDesc(Permissions.Providers) + " " + "or" + " " + GroupPermissions.GetDesc(Permissions.AdminDentalStudents));
				return;
			}
			FormProviderSetup FormPS = new FormProviderSetup();
			FormPS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Providers");
		}

		private void menuItemPrescriptions_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormRxSetup FormRxSetup2 = new FormRxSetup();
			FormRxSetup2.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Rx");
		}

		private void menuItemReferrals_Click(object sender, System.EventArgs e)
		{
			FormReferralSelect FormRS = new FormReferralSelect();
			FormRS.ShowDialog();
		}

		private void menuItemSites_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormSites FormS = new FormSites();
			FormS.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Sites");
		}

		private void menuItemStateAbbrs_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormStateAbbrs formSA = new FormStateAbbrs();
			formSA.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "StateAbbrs");
		}

		private void menuItemZipCodes_Click(object sender, System.EventArgs e)
		{
			//if(!Security.IsAuthorized(Permissions.Setup)){
			//	return;
			//}
			FormZipCodes FormZ = new FormZipCodes();
			FormZ.ShowDialog();
			//SecurityLogs.MakeLogEntry(Permissions.Setup,"Zip Codes");
		}

		#endregion

		#region Reports

		private void menuItemReportsStandard_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Reports))
			{
				return;
			}
			FormReportsMore FormR = new FormReportsMore();
			FormR.DateSelected = ContrAppt2.GetDateSelected();
			FormR.ShowDialog();
			NonModalReportSelectionHelper(FormR.RpNonModalSelection);
		}

		private void menuItemReportsGraphic_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.GraphicalReports))
			{
				return;
			}
			if (_formDashboardEditTab != null)
			{
				_formDashboardEditTab.BringToFront();
				return;
			}
			//on extremely large dbs, the ctor can take a few seconds to load, so show the wait cursor.
			Cursor = Cursors.WaitCursor;
			//Check if the user has permission to view all providers in production and income reports
			bool hasAllProvsPermission = Security.IsAuthorized(Permissions.ReportProdIncAllProviders, true);
			if (!hasAllProvsPermission && Security.CurrentUser.ProviderId == 0)
			{
				if (!MsgBox.Show(MsgBoxButtons.OKCancel, "The current user must be a provider or have the 'All Providers' permission to view provider reports. Continue?"))
				{
					return;
				}
			}
			_formDashboardEditTab = new OpenDentalGraph.FormDashboardEditTab(Security.CurrentUser.ProviderId, !Security.IsAuthorized(Permissions.ReportProdIncAllProviders, true)) { IsEditMode = false };
			_formDashboardEditTab.FormClosed += new FormClosedEventHandler((object senderF, FormClosedEventArgs eF) => { _formDashboardEditTab = null; });
			Cursor = Cursors.Default;
			_formDashboardEditTab.Show();
		}

		private void menuItemReportsUserQuery_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.UserQuery))
			{
				return;
			}

			if (Security.IsAuthorized(Permissions.UserQueryAdmin, true))
			{
				SecurityLogs.MakeLogEntry(Permissions.UserQuery, 0,"User query form accessed.");
				if (_formUserQuery == null || _formUserQuery.IsDisposed)
				{
					_formUserQuery = new FormQuery(null);
					_formUserQuery.FormClosed += (s, e) => { _formUserQuery = null; };
					_formUserQuery.Show();
				}

				if (_formUserQuery.WindowState == FormWindowState.Minimized)
				{
					_formUserQuery.WindowState = FormWindowState.Normal;
				}

				_formUserQuery.BringToFront();
			}
			else
			{
				using var formQueryFavorites = new FormQueryFavorites();

				if (formQueryFavorites.ShowDialog(this) == DialogResult.OK)
				{
					ExecuteQueryFavorite(formQueryFavorites.UserQueryCur);
				}
			}
		}

		private void menuItemReportsUnfinalizedPay_Click(object sender, EventArgs e)
		{
			if (!GroupPermissions.HasReportPermission(DisplayReports.ReportNames.UnfinalizedInsPay, Security.CurrentUser))
			{
                ODMessageBox.Show("You do not have permission to run this report.");
				return;
			}

			using var formRpUnfinalizedInsPay = new FormRpUnfinalizedInsPay();

			formRpUnfinalizedInsPay.ShowDialog(this);
		}

		private void UpdateUnfinalizedPayCount(IEnumerable<Signalod> signals)
		{
			var signal = signals.OrderByDescending(x => x.Date).FirstOrDefault();

			if (signal == null || !signal.Param2.HasValue)
			{
				menuItemReportsUnfinalizedPay.Text = "Unfinalized Payments";

				return;
			}

			menuItemReportsUnfinalizedPay.Text = "Unfinalized Payments" + ": " + signal.Param2.Value;
		}

		private void RefreshMenuReports()
		{
			//Find the index of the last separator which separates the static menu items from the dynamic menu items.
			int separatorIndex = -1;
			for (int i = 0; i < menuItemReportsHeader.MenuItems.Count; i++)
			{
				if (menuItemReportsHeader.MenuItems[i].Text == "-")
				{
					separatorIndex = i;
				}
			}
			//Remove dynamic items and separator.  Leave hard coded items.
			if (separatorIndex != -1)
			{
				for (int i = menuItemReportsHeader.MenuItems.Count - 1; i >= separatorIndex; i--)
				{
					menuItemReportsHeader.MenuItems.RemoveAt(i);
				}
			}
			List<ToolButItem> listToolButItems = ToolButItems.GetForToolBar(ToolBarsAvail.ReportsMenu);
			if (listToolButItems.Count == 0)
			{
				return;//Return early to avoid adding a useless separator in the menu.
			}
			//Add separator, then dynamic items to the bottom of the menu.
			menuItemReportsHeader.MenuItems.Add("-");//Separator			
			listToolButItems.Sort(ToolButItem.Compare);//Alphabetical order
			foreach (ToolButItem toolButItemCur in listToolButItems)
			{
				MenuItem menuItem = new MenuItem(toolButItemCur.ButtonText, menuReportLink_Click);
				menuItem.Tag = toolButItemCur;
				menuItemReportsHeader.MenuItems.Add(menuItem);
			}
		}

		private void menuReportLink_Click(object sender, System.EventArgs e)
		{
			MenuItem menuItem = (MenuItem)sender;
			ToolButItem toolButItemCur = ((ToolButItem)menuItem.Tag);
			ProgramL.Execute(toolButItemCur.ProgramNum, Patients.GetPat(CurPatNum));
		}

		#endregion

		#region CustomReports

		//Custom Reports
		private void menuItemRDLReport_Click(object sender, System.EventArgs e)
		{
			//This point in the code is only reached if the A to Z folders are enabled, thus
			//the image path should exist.
			FormReportCustom FormR = new FormReportCustom();
			FormR.SourceFilePath =
				ODFileUtils.CombinePaths(OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath(), Prefs.GetString(PrefName.ReportFolderName), ((MenuItem)sender).Text + ".rdl");
			FormR.ShowDialog();
		}

		#endregion

		#region Tools

		//Tools
		private void menuItemPrintScreen_Click(object sender, System.EventArgs e)
		{
			FormPrntScrn FormPS = new FormPrntScrn();
			FormPS.ShowDialog();
		}

		#region MiscTools
		private void menuItemDuplicateBlockouts_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormBlockoutDuplicatesFix form = new FormBlockoutDuplicatesFix();
			Cursor = Cursors.WaitCursor;
			form.ShowDialog();
			Cursor = Cursors.Default;
			//Security log entries are made from within the form.
		}

		private void menuItemCreateAtoZFolders_Click(object sender, EventArgs e)
		{
		}

		private void menuItemDatabaseMaintenancePat_Click(object sender, EventArgs e)
		{
			//Purposefully not checking permissions.  All users need the ability to call patient specific DBMs ATM.
			FormDatabaseMaintenancePat FormDMP = new FormDatabaseMaintenancePat(CurPatNum);
			FormDMP.ShowDialog();
		}

		private void menuItemMergeDPs_Click(object sender, EventArgs e)
		{
			FormDiscountPlanMerge FormDPM = new FormDiscountPlanMerge();
			FormDPM.ShowDialog();
		}

		private void menuItemMergeImageCat_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormImageCatMerge FormImageCat = new FormImageCatMerge();
			FormImageCat.ShowDialog();
			//Security log entries are made from within the form.
		}

		private void menuItemMergeMedications_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.MedicationMerge))
			{
				return;
			}
			FormMedicationMerge FormMM = new FormMedicationMerge();
			FormMM.ShowDialog();
			//Securitylog entries are handled within the form.
		}

		private void menuItemMergePatients_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.PatientMerge))
			{
				return;
			}
			FormPatientMerge fpm = new FormPatientMerge();
			fpm.ShowDialog();
			//Security log entries are made from within the form.
		}

		private void menuItemMergeReferrals_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.ReferralMerge))
			{
				return;
			}
			FormReferralMerge FormRM = new FormReferralMerge();
			FormRM.ShowDialog();
			//Security log entries are made from within the form.
		}

		private void menuItemMergeProviders_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.ProviderMerge))
			{
				return;
			}
			FormProviderMerge FormPM = new FormProviderMerge();
			FormPM.ShowDialog();
			//Security log entries are made from within the form.
		}

		private void menuItemMoveSubscribers_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.InsPlanChangeSubsc))
			{
				return;
			}
			FormSubscriberMove formSM = new FormSubscriberMove();
			formSM.ShowDialog();
			//Security log entries are made from within the form.
		}

		private void menuPatientStatusSetter_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.SecurityAdmin))
			{
				return;
			}
			FormPatientStatusTool formPST = new FormPatientStatusTool();
			formPST.ShowDialog();
			//Security log entries are made from within the form.
		}

		private void menuItemProcLockTool_Click(object sender, EventArgs e)
		{
			FormProcLockTool FormT = new FormProcLockTool();
			FormT.ShowDialog();
			//security entries made inside the form
			//SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Proc Lock Tool");
		}

		private void menuItemSetupWizard_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormSetupWizard FormSW = new FormSetupWizard();
			FormSW.ShowDialog();
		}

		private void menuItemServiceManager_Click(object sender, EventArgs e)
		{
			// OBSOLETE...
		}

		private void menuItemShutdown_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}

			using var formShutdown = new FormShutdown();
			if (formShutdown.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			// Turn off signal reception for 5 seconds so this workstation will not shut down.
			Signalods.SignalLastRefreshed = DateTime.UtcNow.AddSeconds(5);

			Signalods.Send(SignalName.Shutdown);

			Computers.ClearAllHeartBeats(Environment.MachineName);

			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Shutdown all workstations.");
		}

		private void menuTelephone_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}

			using var formTelephone = new FormTelephone();

			formTelephone.ShowDialog(this);
		}

		private void menuItemTestLatency_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}

			using var formTestLatency = new FormTestLatency();

			formTestLatency.ShowDialog(this);
		}

		private void menuItemXChargeReconcile_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Accounting))
			{
				return;
			}
			FormXChargeReconcile FormXCR = new FormXChargeReconcile();
			FormXCR.ShowDialog();
		}
		#endregion MiscTools

		private void menuItemAging_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormAging FormAge = new FormAging();
			FormAge.ShowDialog();
		}

		private void menuItemAuditTrail_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.AuditTrail))
			{
				return;
			}
			FormAudit FormA = new FormAudit();
			FormA.CurPatNum = CurPatNum;
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.AuditTrail, 0, "Audit Trail");
		}

		private void menuItemFinanceCharge_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormFinanceCharges FormFC = new FormFinanceCharges();
			FormFC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Run Finance Charges");
		}

		private void menuItemCCRecurring_Click(object sender, EventArgs e)
		{
			if (formCreditRecurringCharges == null || formCreditRecurringCharges.IsDisposed)
			{
				formCreditRecurringCharges = new FormCreditRecurringCharges();
			}
			Cursor = Cursors.WaitCursor;
			formCreditRecurringCharges.Show();
			Cursor = Cursors.Default;
			if (formCreditRecurringCharges.WindowState == FormWindowState.Minimized)
			{
				formCreditRecurringCharges.WindowState = FormWindowState.Normal;
			}
			formCreditRecurringCharges.BringToFront();
		}

		private void menuItemCustomerManage_Click(object sender, EventArgs e)
		{
			FormCustomerManagement FormC = new FormCustomerManagement();
			FormC.ShowDialog();
			if (FormC.SelectedPatNum != 0)
			{
				CurPatNum = FormC.SelectedPatNum;
				Patient pat = Patients.GetPat(CurPatNum);
				RefreshCurrentModule();
				FillPatientButton(pat);
			}
		}

		private void menuItemDatabaseMaintenance_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormDatabaseMaintenance FormDM = new FormDatabaseMaintenance();
			FormDM.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Database Maintenance");
		}

		private void menuItemDispensary_Click(object sender, System.EventArgs e)
		{
			FormDispensary FormD = new FormDispensary();
			FormD.ShowDialog();
		}

		private void menuItemEvaluations_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.AdminDentalEvaluations, true) && (Security.CurrentUser.ProviderId == 0 || Providers.GetProv(Security.CurrentUser.ProviderId).SchoolClassNum != 0))
			{
				MessageBox.Show("Only Instructors may view or edit evaluations.");
				return;
			}
			FormEvaluations FormE = new FormEvaluations();
			FormE.ShowDialog();
		}

		private void menuItemTerminal_Click(object sender, EventArgs e)
		{
			if (Prefs.GetLong(PrefName.ProcessSigsIntervalInSecs) == 0)
			{
				MessageBox.Show("Cannot open terminal unless process signal interval is set. To set it, go to Setup > Miscellaneous.");
				return;
			}
			FormTerminal FormT = new FormTerminal();
			FormT.ShowDialog();
			Application.Exit();//always close after coming out of terminal mode as a safety precaution.*/
		}

		private void menuItemTerminalManager_Click(object sender, EventArgs e)
		{
			if (formTerminalManager == null || formTerminalManager.IsDisposed)
			{
				formTerminalManager = new FormTerminalManager(setupMode: true);
			}
			formTerminalManager.Show();
			formTerminalManager.BringToFront();
		}

		private void menuItemTranslation_Click(object sender, System.EventArgs e)
		{
		}

		private void menuItemMobileSetup_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormEServicesMobileSynch formESMobileSynch = new FormEServicesMobileSynch();
			formESMobileSynch.ShowDialog();
			ShowEServicesSetup();
		}

		private void menuItemNewCropBilling_Click(object sender, EventArgs e)
		{
			FormNewCropBilling FormN = new FormNewCropBilling();
			FormN.ShowDialog();
		}

		private void menuItemPendingPayments_Click(object sender, EventArgs e)
		{
			FormPendingPayments FormPP = new FormPendingPayments();
			FormPP.Show();//Non-modal so the user can view the patient's account
		}

		private void menuItemRepeatingCharges_Click(object sender, System.EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.RepeatChargeTool))
			{
				return;
			}
			FormRepeatChargesUpdate FormR = new FormRepeatChargesUpdate();
			FormR.ShowDialog();
		}

		private void menuItemResellers_Click(object sender, EventArgs e)
		{
			FormResellers FormR = new FormResellers();
			FormR.ShowDialog();
		}

		private void menuItemScreening_Click(object sender, System.EventArgs e)
		{
			FormScreenGroups FormS = new FormScreenGroups();
			FormS.ShowDialog();
		}

		private void menuItemReqStudents_Click(object sender, EventArgs e)
		{
			Provider prov = Providers.GetProv(Security.CurrentUser.ProviderId);
			if (prov == null)
			{
				MessageBox.Show("The current user is not attached to a provider. Attach the user to a provider to gain access to this feature.");
				return;
			}
			if (!prov.IsInstructor)
			{//if a student is logged in
			 //the student always has permission to view their own requirements
				FormReqStudentOne FormO = new FormReqStudentOne();
				FormO.ProvNum = prov.ProvNum;
				FormO.ShowDialog();
				return;
			}
			if (prov.IsInstructor)
			{
				FormReqStudentsMany FormM = new FormReqStudentsMany();
				FormM.ShowDialog();
			}
		}

		private void menuItemWebForms_Click(object sender, EventArgs e)
		{
			FormWebForms FormWF = new FormWebForms();
			FormWF.Show();
		}

		private void menuItemWiki_Click(object sender, EventArgs e)
		{
			if (Plugins.HookMethod(this, "FormOpenDental.menuItemWiki_Click"))
			{
				return;
			}
			//We want to allow as many wiki pages open as possible to maximize efficiency in the office.
			new FormWiki().Show();
		}

		private void menuItemXWebTrans_Click(object sender, EventArgs e)
		{
			if (FormXWT == null || FormXWT.IsDisposed)
			{
				FormXWT = new FormXWebTransactions();
				FormXWT.FormClosed += new FormClosedEventHandler((o, e1) => { FormXWT = null; });
				FormXWT.Show();
			}
			if (FormXWT.WindowState == FormWindowState.Minimized)
			{
				FormXWT.WindowState = FormWindowState.Normal;
			}
			FormXWT.BringToFront();
		}

		public static void S_WikiLoadPage(string pageTitle)
		{
			if (!Prefs.GetBool(PrefName.WikiCreatePageFromLink) && !WikiPages.CheckPageNamesExist(new List<string> { pageTitle })[0])
			{
				MsgBox.Show("Wiki page does not exist.");
				return;
			}
			FormWiki FormW = new FormWiki();
			FormW.Show();
			FormW.LoadWikiPagePublic(pageTitle);//This has to be after the form has loaded
		}

		private void menuItemAutoClosePayPlans_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			if (!MsgBox.Show(MsgBoxButtons.OKCancel, "Running this tool will automatically mark all payment plans that have"
				+ " been paid off and have no future charges as closed.  Do you want to continue?"))
			{
				return;
			}
			long plansClosed = PayPlans.AutoClose(); //returns # of payplans closed.
			string msgText;
			if (plansClosed > 0)
			{
				msgText = "Success." + "  " + plansClosed + " " + "plan(s) closed.";
			}
			else
			{
				msgText = "There were no plans to close.";
			}
			MessageBox.Show(msgText);
		}

		private void menuItemOrthoAuto_Click(object sender, EventArgs e)
		{
			FormOrthoAutoClaims FormOAC = new FormOrthoAutoClaims();
			FormOAC.ShowDialog();
		}

		private void menuItemMassEmails_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.EmailSend))
			{
				return;
			}
			FormMassEmail formSendMassEmail = new FormMassEmail();
			formSendMassEmail.ShowDialog();
		}
		#endregion

		#region Clinics
		//menuClinics is a dynamic menu that is maintained within RefreshMenuClinics()
		#endregion

		#region Dashboard
		private void RefreshMenuDashboards()
		{
			List<SheetDef> listDashboards = SheetDefs.GetWhere(x => x.SheetType == SheetTypeEnum.PatientDashboardWidget
				  && Security.IsAuthorized(Permissions.DashboardWidget, x.SheetDefNum, true), true);
			bool isAuthorizedForSetup = Security.IsAuthorized(Permissions.Setup, true);
			this.InvokeIfRequired(() =>
			{
				menuItemPatDashboards.MenuItems.Clear();
				if (listDashboards.Count > 28)
				{//This number of items+line+Setup will fit in a 990x735 form.
					menuItemPatDashboards.Click -= OpenDashboardSelect;//Make sure we only subscribe once.
					menuItemPatDashboards.Click += OpenDashboardSelect;
					return;
				}
				List<long> listOpenDashboardsSheetDefNums = userControlPatientDashboard.ListOpenWidgets.Select(x => x.SheetDefWidget.SheetDefNum).ToList();
				MenuItem menuItem = new MenuItem("Dashboard Setup", OpenDashboardSetup);
				if (!isAuthorizedForSetup)
				{
					menuItem.Enabled = false;
				}
				menuItemPatDashboards.MenuItems.Add(menuItem);
				if (listDashboards.Count > 0)
				{
					menuItemPatDashboards.MenuItems.Add(new MenuItem("-"));
				}
				foreach (SheetDef dashboardDef in listDashboards)
				{
					menuItem = new MenuItem(dashboardDef.Description, DashboardMenuClick);
					menuItem.Tag = dashboardDef;
					if (dashboardDef.SheetDefNum.In(listOpenDashboardsSheetDefNums))
					{//Currently open Dashboard.
						menuItem.Checked = true;
					}
					menuItemPatDashboards.MenuItems.Add(menuItem);
				}
			});
		}

		private void OpenDashboardSelect(object sender, EventArgs e)
		{
			FormDashboardWidgets formDashboards = new FormDashboardWidgets();//Open the LaunchDashboard window.
			if (formDashboards.ShowDialog() == DialogResult.OK && formDashboards.SheetDefDashboardWidget != null)
			{
				TryLaunchPatientDashboard(formDashboards.SheetDefDashboardWidget);
			}
			RefreshMenuDashboards();
		}

		private void OpenDashboardSetup(object sender, System.EventArgs e)
		{
			FormDashboardWidgetSetup formDS = new FormDashboardWidgetSetup();
			formDS.ShowDialog();
			RefreshMenuDashboards();
		}

		///<summary>Opens a UserControlDashboardWidget, closing the previously selected UserControlDashboardWidget if one is already open.  If the user
		///clicked on the menu item corresponding to the currently open Patient Dashboard, this means "Close".</summary>
		private void DashboardMenuClick(object sender, System.EventArgs e)
		{
			if (sender.GetType() != typeof(MenuItem) || ((MenuItem)sender).Tag == null || ((MenuItem)sender).Tag.GetType() != typeof(SheetDef))
			{
				return;
			}
			SheetDef widgetNew = (SheetDef)((MenuItem)sender).Tag;
			TryLaunchPatientDashboard(widgetNew);//Open the newly selected Patient Dashboard.
		}

		///<summary>Opens a UserControlDashboardWidget.  The user's permissions should be validated prior to calling this method.</summary>
		private bool TryLaunchPatientDashboard(SheetDef sheetDefWidget)
		{
			// TODO:
			//Action actionOpenNewDashboard;
			//if (userControlPatientDashboard.IsInitialized)
			//{
			//	if (userControlPatientDashboard.ListOpenWidgets.Any(x => x.Name == POut.Long(sheetDefWidget.SheetDefNum)))
			//	{
			//		//Clicked on the currently open Patient Dashboard.  This means "Close the Patient Dashboard".
			//		userControlPatientDashboard.CloseDashboard(false);//Causes userodpref to be deleted.
			//		return false;
			//	}
			//	//Changing which Patient Dashboard is being shown.  First add the new one, then close the old, and update user pref.
			//	//This order of operations helps avoid unnecessary UI flicker and slowness because we don't actually close the entire Dashboard control.
			//	actionOpenNewDashboard = new Action(() =>
			//	{
			//		UserOdPref userPrefDashboard = UserOdPrefs.GetByUserAndFkeyType(Security.CurrentUser.Id, UserOdFkeyType.Dashboard).FirstOrDefault();
			//		List<UserControlDashboardWidget> listOpenWidgets = userControlPatientDashboard.ListOpenWidgets;
			//		userControlPatientDashboard.AddWidget(sheetDefWidget);
			//		foreach (UserControlDashboardWidget widget in listOpenWidgets)
			//		{
			//			widget.CloseWidget();
			//		}
			//		ResizeDashboard();//Resize the splitter/Patient Dashboard container appropriately.
			//		userPrefDashboard.Fkey = sheetDefWidget.SheetDefNum;
			//		UserOdPrefs.Update(userPrefDashboard);
			//		RefreshMenuDashboards();
			//	});
			//}
			//else
			//{
			//	actionOpenNewDashboard = new Action(() =>
			//	{
			//		UserOdPref userPrefDashboard = new UserOdPref()
			//		{//If Patient Dashboard was not open, so we need a new user pref for the current user.
			//			UserNum = Security.CurrentUser.Id,
			//			Fkey = sheetDefWidget.SheetDefNum,
			//			FkeyType = UserOdFkeyType.Dashboard,
			//			ClinicNum = Clinics.ClinicNum
			//		};
			//		if (Security.CurrentUser.Id != 0)
			//		{//If the userNum is 0 for the following command it will delete all Patient Dashboard UserOdPrefs!
			//		 //if any Patient Dashboard UserOdPrefs already exists for this user, remove them. This could happen due to a previous concurrency bug.
			//			UserOdPrefs.DeleteForValueString(Security.CurrentUser.Id, UserOdFkeyType.Dashboard, "");
			//		}
			//		userPrefDashboard.UserOdPrefNum = UserOdPrefs.Insert(userPrefDashboard);//Pre-insert for PK.
			//		try
			//		{
			//			InitDashboards(Security.CurrentUser.Id, userPrefDashboard);
			//		}
			//		catch (NotImplementedException niex)
			//		{
			//			MessageBox.Show(this, "Error loading Patient Dashboard:\r\n" + niex.Message + "\r\nCorrect errors in Dashboard Setup.");
			//		}
			//		catch (Exception ex)
			//		{
			//			throw new Exception("Unexpected error loading Patient Dashboard: " + ex.Message, ex);//So we get bug submission.
			//		}
			//	});
			//}
			//ODProgress.ShowAction(actionOpenNewDashboard, "Starting Patient Dashboard");
			//return userControlPatientDashboard.IsInitialized;
			return true;
		}

		///<summary>Determines if there is a user preference for which Dashboard to open on startup, and launches it if the user has permissions to 
		///launch the dashboard.</summary>
		private void InitDashboards(long userNum/*, UserOdPref userPrefDashboard = null*/)
		{
			//bool isOpenedManually = (userPrefDashboard != null);
			//userPrefDashboard = userPrefDashboard ?? UserOdPrefs.GetByUserAndFkeyType(userNum, UserOdFkeyType.Dashboard).FirstOrDefault();
			//if (userPrefDashboard == null)
			//{
			//	return;//User didn't have the dashboard open the last time logged out.
			//}
			//if (userControlTasks1.Visible && ComputerPrefs.LocalComputer.TaskDock == 1)
			//{//Tasks are docked right
			//	this.InvokeIfRequired(() =>
			//	{
			//		MessageBox.Show("Dashboards are disabled when Tasks are docked to the right.");
			//		if (Security.CurrentUser.Id != 0)
			//		{//If the userNum is 0 for the following command it will delete all Patient Dashboard UserOdPrefs!
			//		 //Stop the Patient Dashboard from attempting to open on next login.
			//			UserOdPrefs.DeleteForValueString(Security.CurrentUser.Id, UserOdFkeyType.Dashboard, "");
			//		}
			//	});
			//	return;
			//}
			//SheetDef sheetDefDashboard = GetUserDashboard(userPrefDashboard);
			//if (sheetDefDashboard == null)
			//{//Couldn't find the SheetDef, no sense trying to initialize the Patient Dashboard.
			//	if (isOpenedManually)
			//	{//Only prompt if user attempted to open a Patient Dashboard from the menu.
			//		this.InvokeIfRequired(() =>
			//		{
			//			MessageBox.Show("Patient Dashboard could not be found.");
			//		});
			//	}
			//	return;
			//}
			////Pass in SheetDef describing Dashboard layout.
			//userControlPatientDashboard.Initialize(sheetDefDashboard, () => { this.InvokeIfRequired(() => LayoutControls()); }
			//	, () =>
			//	{
			//		RefreshMenuDashboards();
			//		if (ContrAppt2.Visible)
			//		{//Ensure appointment view redraws.
			//			ContrAppt2.ModuleSelected(CurPatNum);
			//		}
			//	}
			//);
			//RefreshMenuDashboards();
		}

		///<summary>Gets the current user's Patient Dashboard SheetDef.  Returns null if the SheetDef linked via userPrefDashboard does not exist.
		///If the Patient Dashboard SheetDef linked via userPrefDashboard no longer exists, userPrefDashboard is deleted.</summary>
		private static SheetDef GetUserDashboard(/*UserOdPref userPrefDashboard*/)
		{
			//if (userPrefDashboard == null)
			//{
			//	return null;
			//}
			//long sheetDefDashboardNum = userPrefDashboard.Fkey;
			//SheetDef sheetDefDashboard = SheetDefs.GetFirstOrDefault(x => x.SheetDefNum == sheetDefDashboardNum);
			//if (sheetDefDashboard == null)
			//{
			//}
			//else if (sheetDefDashboard.SheetType == SheetTypeEnum.PatientDashboard)
			//{
			//	//Previously, users could open multiple Patient Dashboard SheetDefs as UserControlDashboardWidgets within the UserControlDashboard container.
			//	//These UserControlDashboardWidgets could be arranged by dragging/dropping them around the container, and this layout was saved on a user by
			//	//user basis.
			//	//In E13631, we only allow one UserControlDashboardWidget to be open at a time, and in E13629 we removed the drag/drop functionality.  This
			//	//eliminates the need for a separate SheetDef to save the user's Patient Dashboard layout.  However, we want to as seemlessly as possible
			//	//transition users away from the multiple Patient Dashboard functionality, ideally without any interruption in user experience.
			//	//If the user had a saved layout, we will pick the first (hopefully only) UserControlDashboardWidget/SheetDef and use it as the last opened
			//	//Patient Dashboard, simultaneously removing the user specific layout SheetDef and linking via userPrefDashboard to the first 
			//	//UserControlDashboard that was previoulsy open. This will allow us to remove the obsolete layout SheetDef, as well as save the selected
			//	//Patient Dashboard for the user.
			//	SheetDefs.GetFieldsAndParameters(sheetDefDashboard);
			//	//FieldValue corresponds to the Patient Dashboard widget SheetDef.SheetDefNum
			//	long firstWidgetSheetDefNum = PIn.Long(sheetDefDashboard.SheetFieldDefs.FirstOrDefault().FieldValue);
			//	SheetDefs.DeleteObject(sheetDefDashboard.SheetDefNum);//Delete the layout SheetDef.
			//	sheetDefDashboard = SheetDefs.GetFirstOrDefault(x => x.SheetDefNum == firstWidgetSheetDefNum);
			//	userPrefDashboard.Fkey = firstWidgetSheetDefNum;//May not exist.  Will get cleaned up later.
			//	UserOdPrefs.Update(userPrefDashboard);
			//}
			//return sheetDefDashboard;
			return default;
		}

		///<summary>Determines if the Dashboard is currently visible.</summary>
		public static bool IsDashboardVisible
		{
			get
			{
				return (!_formOpenDentalS.splitContainerNoFlickerDashboard.Panel2Collapsed && _formOpenDentalS.userControlPatientDashboard.IsInitialized);
			}
		}
		#endregion

		#region eServices

		private void ShowEServicesSetup()
		{
			if (_butText != null)
			{ //User may just have signed up for texting.
				_butText.Enabled = Programs.IsEnabled(ProgramName.CallFire) || SmsPhones.IsIntegratedTextingEnabled();
			}
		}
		#endregion

		#region Alerts
		///<summary>Helper function to determin backgroud color of an AlertItem.</summary>
		private Color AlertBackgroudColorHelper(SeverityType type)
		{
			switch (type)
			{
				default:
				case SeverityType.Normal:
					return SystemColors.Control;
				case SeverityType.Low:
					return Color.LightGoldenrodYellow;
				case SeverityType.Medium:
					return Color.DarkOrange;
				case SeverityType.High:
					return Color.OrangeRed;
			}
		}

		///<summary>Helper function to determin text color of an AlertItem.</summary>
		private Color AlertTextColorHelper(SeverityType type)
		{
			switch (type)
			{
				default:
					return Color.White;
				case SeverityType.Low:
				case SeverityType.Normal:
					return Color.Black;
			}
		}

		private void menuItemAlerts_Popup(object sender, EventArgs e)
		{
			CheckAlerts();
		}

		///<summary></summary>
		private void menuItem_DrawItem(object sender, DrawItemEventArgs e)
		{
			//the MainMenu kept getting messed up vertical alignment when changing Dpi, so this is a quick way to take control of the drawing.
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Center;
			stringFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
			Rectangle rectOutline = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
			if ((e.State & DrawItemState.HotLight) == DrawItemState.HotLight || (e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				using (SolidBrush brushBackground = new SolidBrush(Color.FromArgb(224, 240, 255)))
				{//light blue
					e.Graphics.FillRectangle(brushBackground, e.Bounds);
				}
				using (Pen pen = new Pen(Color.FromArgb(0, 173, 254)))
				{//med blue line
					e.Graphics.DrawRectangle(pen, rectOutline);
				}
			}
			else
			{
				using (SolidBrush brushBackground = new SolidBrush(this.BackColor))
				{
					e.Graphics.FillRectangle(brushBackground, e.Bounds);
				}
			}
			e.Graphics.DrawString(((MenuItem)sender).Text, this.Font, Brushes.Black, e.Bounds, stringFormat);
			stringFormat?.Dispose();
		}

		///<summary>Handles the drawing and coloring for the Alerts menu and its sub items.</summary>
		private void menuItemAlerts_DrawItem(object sender, DrawItemEventArgs e)
		{
			MenuItem menuItem = (MenuItem)sender;
			AlertItem alertItem = ((AlertItem)menuItem.Tag);//Can be Null
															//The TagOD on the alert is the list of AlertItemNums for all alerts that are its duplicate.
			List<long> listThisAlertItemNums = (List<long>)alertItem?.TagOD ?? new List<long>();
			Color colorText = SystemColors.MenuText;
			Color backGroundColor = SystemColors.Control;
			if (menuItem == menuItemAlerts)
			{
				if (_listAlertItems != null && _listAlertReads != null)
				{
					List<long> listAlertItemNums = _listAlertItems.Select(x => x.AlertItemNum).ToList();//All alert nums for current alertItems.
					List<long> listAlertReadItemNums = _listAlertReads.Select(x => x.AlertItemId).ToList();//All alert nums for read alertItems.
					if (!menuItemNoAlerts.Visible && //menuItemNoAlerts is only Visible when there are no AlertItems to show.
							!listAlertItemNums.All(x => listAlertReadItemNums.Contains(x)))
					{
						//Max SeverityType for all unread AlertItems.
						SeverityType maxSeverity = _listAlertItems.FindAll(x => !listAlertReadItemNums.Contains(x.AlertItemNum)).Select(x => x.Severity).Max();
						backGroundColor = AlertBackgroudColorHelper(maxSeverity);
						colorText = AlertTextColorHelper(maxSeverity);
					}
					else
					{//Either there are no AlertItems to show or they all have an AlertRead row.
						colorText = SystemColors.MenuText;
					}
				}
			}
			else if (menuItem == menuItemNoAlerts)
			{
				//Keep this menuItem colors as system defaults.
			}
			else
			{//This is an alert menuItem.
				if (!_listAlertReads.Exists(x => x.AlertItemId == alertItem.AlertItemNum))
				{//User has not acknowleged alert yet.
					backGroundColor = AlertBackgroudColorHelper(alertItem.Severity);
					colorText = AlertTextColorHelper(alertItem.Severity);
				}
				else
				{//User has an AlertRead row for this AlertItem.
					colorText = SystemColors.MenuText;
				}
			}
			//if(!menuItem.Enabled || e.State==(DrawItemState.NoAccelerator | DrawItemState.Inactive)) {
			//	colorText=SystemColors.ControlDark;
			//}
			//Check if selected or hovering over.
			if (e.State == (DrawItemState.NoAccelerator | DrawItemState.Selected)
				|| e.State == (DrawItemState.NoAccelerator | DrawItemState.HotLight))
			{
				if (backGroundColor == Color.OrangeRed || backGroundColor == Color.DarkOrange)
				{
					colorText = Color.Yellow;
				}
				else if (backGroundColor == Color.LightGoldenrodYellow)
				{
					colorText = Color.OrangeRed;
				}
				else if (menuItem == menuItemAlerts)
				{
					//we could make this look the same as the others main menu items, but it would take about 30 minutes,
					//including handling outline colors for different situations, etc.  This is good enough.
					backGroundColor = Color.FromArgb(145, 201, 247);//med blue  //SystemColors.Highlight;
																	//colorText=SystemColors.HighlightText;
				}
				else
				{//sub menu items
					backGroundColor = Color.FromArgb(145, 201, 247);//med blue
				}
			}
			using (SolidBrush brushBackground = new SolidBrush(backGroundColor))
			using (Font font = new Font("Microsoft Sans Serif", Dpi.Scale(this, 8.25f)))
			using (SolidBrush brushText = new SolidBrush(colorText))
			{
				//Get the text that is displaying from the menu item compenent.
				//Create a string format to center the text to mimic the other menu items.
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Center;
				if (menuItem != menuItemAlerts)
				{
					stringFormat.Alignment = StringAlignment.Near;
				}
				stringFormat.LineAlignment = StringAlignment.Center;
				//stringFormat.HotkeyPrefix=System.Drawing.Text.HotkeyPrefix.Show;
				Rectangle rectBack;
				Rectangle rectText;
				if (menuItem == menuItemAlerts)
				{
					rectBack = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
					//Other options include making our own menu control or drawing our own menu items.
					rectText = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
				}
				else
				{
					rectBack = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 30, e.Bounds.Height);//Sub menu items need some extra width.
					rectText = new Rectangle(e.Bounds.X + 15, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);//Mimic the spacing of other menu items.
				}
				e.Graphics.FillRectangle(brushBackground, rectBack);
				e.Graphics.DrawString(menuItem.Text, font, brushText, rectText, stringFormat);
				stringFormat.Dispose();
			}
		}

		private void menuItem_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			MenuItem menuItem = (MenuItem)sender;
			Size sizeString;
			using (Font font = new Font("Microsoft Sans Serif", Dpi.Scale(this, 8.25f)))
			{
				sizeString = TextRenderer.MeasureText(menuItem.Text, font);
			}
			//this also measures the hotkey prefix (&), but that's ok.
			e.ItemWidth = sizeString.Width;
			e.ItemHeight = sizeString.Height;
		}

		private void menuItemAlerts_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			//Measure the text showing.
			MenuItem menuItem = (MenuItem)sender;
			Size sizeString;
			using (Font font = new Font("Microsoft Sans Serif", Dpi.Scale(this, 8.25f)))
			{
				sizeString = TextRenderer.MeasureText(menuItem.Text, font);
			}
			e.ItemWidth = sizeString.Width;
			if (menuItem != menuItemAlerts)
			{
				e.ItemWidth = sizeString.Width + 15;//Due to logic in menuItemAlerts_DrawItem(...).
			}
			e.ItemHeight = sizeString.Height + 5;//Pad the bottom
		}

		private void menuItemAlerts_Click(object sender, EventArgs e)
		{
			MenuItem menuItem = (MenuItem)sender;
			AlertItem alertItem = (AlertItem)menuItem.Tag;
			//The TagOD on the alert is the list of AlertItemNums for all alerts that are its duplicate.
			List<long> listAlertItemNums = (List<long>)alertItem.TagOD;
			if (menuItem.Name == ActionType.MarkAsRead.ToString())
			{
				AlertReadsHelper(listAlertItemNums);
				CheckAlerts();
				return;
			}
			if (menuItem.Name == ActionType.Delete.ToString())
			{
				if (!MsgBox.Show(MsgBoxButtons.OKCancel, "This will delete the alert for all users. Are you sure you want to delete it?"))
				{
					return;
				}
				AlertItems.Delete(listAlertItemNums);
				CheckAlerts();
				return;
			}
			if (menuItem.Name == ActionType.OpenForm.ToString())
			{
				AlertReadsHelper(listAlertItemNums);
				switch (alertItem.FormToOpen)
				{
					case FormType.FormPendingPayments:
						FormPendingPayments FormPP = new FormPendingPayments();
						FormPP.Show();//Non-modal so the user can view the patient's account
						FormPP.FormClosed += this.alertFormClosingHelper;
						break;
					case FormType.FormEServicesWebSchedRecall:
						FormEServicesWebSched formESWebSched = new FormEServicesWebSched();
						formESWebSched.ShowDialog();
						ShowEServicesSetup();
						break;
					case FormType.FormRadOrderList:
						List<FormRadOrderList> listFormROLs = Application.OpenForms.OfType<FormRadOrderList>().ToList();
						if (listFormROLs.Count > 0)
						{
							listFormROLs[0].RefreshRadOrdersForUser(Security.CurrentUser);
							listFormROLs[0].BringToFront();
						}
						else
						{
							FormRadOrderList FormROL = new FormRadOrderList(Security.CurrentUser);
							FormROL.Show();
							FormROL.FormClosed += this.alertFormClosingHelper;
						}
						break;
					case FormType.FormEServicesSignupPortal:
						FormEServicesSignup formESSignup = new FormEServicesSignup();
						formESSignup.ShowDialog();
						ShowEServicesSetup();
						break;
					case FormType.FormEServicesWebSchedNewPat:
						FormEServicesWebSched formESWebSchedNewPat = new FormEServicesWebSched(setTab: FormEServicesWebSched.WebSchedTab.NewPat);
						formESWebSchedNewPat.ShowDialog();
						ShowEServicesSetup();
						break;
					case FormType.FormEServicesEConnector:
						FormEServicesEConnector formESEConnector = new FormEServicesEConnector();
						formESEConnector.ShowDialog();
						ShowEServicesSetup();
						break;
					case FormType.FormApptEdit:
						Appointment appt = Appointments.GetOneApt(alertItem.FKey);
						Patient pat = Patients.GetPat(appt.PatNum);
						S_Contr_PatientSelected(pat, false);
						FormApptEdit FormAE = new FormApptEdit(appt.AptNum);
						FormAE.ShowDialog();
						break;
					case FormType.FormWebSchedAppts:
						FormWebSchedAppts FormWebSchedAppts = new FormWebSchedAppts(alertItem.Type == AlertType.WebSchedNewPatApptCreated,
							alertItem.Type == AlertType.WebSchedRecallApptCreated, alertItem.Type == AlertType.WebSchedASAPApptCreated);
						FormWebSchedAppts.Show();
						break;
					case FormType.FormPatientEdit:
						pat = Patients.GetPat(alertItem.FKey);
						Family fam = Patients.GetFamily(pat.PatNum);
						S_Contr_PatientSelected(pat, false);
						FormPatientEdit FormPE = new FormPatientEdit(pat, fam);
						FormPE.ShowDialog();
						break;
					case FormType.FormDoseSpotAssignUserId:
						if (!Security.IsAuthorized(Permissions.SecurityAdmin))
						{
							break;
						}
						FormDoseSpotAssignUserId FormAU = new FormDoseSpotAssignUserId(alertItem.FKey);
						FormAU.ShowDialog();
						break;
					case FormType.FormDoseSpotAssignClinicId:
						if (!Security.IsAuthorized(Permissions.SecurityAdmin))
						{
							break;
						}
						FormDoseSpotAssignClinicId FormACI = new FormDoseSpotAssignClinicId(alertItem.FKey);
						FormACI.ShowDialog();
						break;
					case FormType.FormEmailInbox:
						//Will open the email inbox form and set the current inbox to "WebMail".
						FormEmailInbox FormEI = new FormEmailInbox("WebMail");
						FormEI.FormClosed += this.alertFormClosingHelper;
						FormEI.Show();
						break;
					case FormType.FormEmailAddresses:
						//Will open the email addresses window that is usually opened from email inbox setup.
						FormEmailAddresses formEA = new FormEmailAddresses();
						formEA.FormClosed += this.alertFormClosingHelper;
						formEA.ShowDialog();
						break;
				}
			}
			if (menuItem.Name == ActionType.ShowItemValue.ToString())
			{
				AlertReadsHelper(listAlertItemNums);
				MsgBoxCopyPaste msgBCP = new MsgBoxCopyPaste($"{alertItem.Description}\r\n\r\n{alertItem.ItemValue}");
				msgBCP.Show();
			}
		}

		///<summary>This is used to force the alert logic to run on the server in OpenDentalService.
		///OpenDentalService Alerts logic will re run on signal update interval time.
		///This could be enhanced eventually only invalidate when something from the form changed.</summary>
		private void alertFormClosingHelper(object sender, FormClosedEventArgs e)
		{
			DataValid.SetInvalid(InvalidType.AlertItems);//THIS IS NOT CACHED. But is used to make server run the alert logic in OpenDentalService.
		}

		///<summary>Refreshes AlertReads for current user and creates a new one if one does not exist for given alertItem.</summary>
		private void AlertReadsHelper(List<long> listAlertItemNums)
		{
			listAlertItemNums.RemoveAll(x => _listAlertReads.Exists(y => y.AlertItemId == x));//Remove all the ones the user has already read.
			listAlertItemNums.ForEach(x => AlertReads.Insert(new AlertRead(x, Security.CurrentUser.Id)));
		}
		#endregion Alerts

		#region Standard and Query reports
		private void menuItemReportsHeader_Popup(object sender, EventArgs e)
		{
			menuItemReportsStandard.MenuItems.Clear();
			menuItemReportsUserQuery.MenuItems.Clear();
			if (Security.CurrentUser == null)
			{
				return;
			}
			#region Standard
			List<DisplayReport> listDisplayReports = DisplayReports.GetSubMenuReports();
			if (listDisplayReports.Count > 0)
			{
				List<long> listReportPermissionFkeys = GroupPermissions.GetPermsForReports()
					.Where(x => Security.CurrentUser.IsInUserGroup(x.UserGroupNum))
					.Select(x => x.FKey)
					.ToList();
				listDisplayReports.RemoveAll(x => !listReportPermissionFkeys.Contains(x.DisplayReportNum));//Remove reports user does not have permission for
				menuItemReportsStandard.MenuItems.Add("Standard Reports", menuItemReportsStandard_Click);
				menuItemReportsStandard.MenuItems.Add("-");//Horizontal line.
				listDisplayReports.ForEach(x =>
				{
					MenuItem menuItem = new MenuItem(x.Description, StandardReport_ClickEvent);
					menuItem.Tag = x;
					menuItemReportsStandard.MenuItems.Add(menuItem);
				});
			}
			#endregion
			#region UserQueries
			List<UserQuery> listReleasedQuries = UserQueries.GetDeepCopy(true);
			if (listReleasedQuries.Count > 0)
			{
				menuItemReportsUserQuery.MenuItems.Add("User Query", menuItemReportsUserQuery_Click);
				menuItemReportsUserQuery.MenuItems.Add("-");//Horizontal line.
				listReleasedQuries.ForEach(x =>
				{
					MenuItem menuItem = new MenuItem(x.Description, UserQuery_ClickEvent);
					menuItem.Tag = x;
					menuItemReportsUserQuery.MenuItems.Add(menuItem);
				});
			}
			#endregion
		}

		private void StandardReport_ClickEvent(object sender, EventArgs e)
		{
			DisplayReport displayReport = (DisplayReport)((MenuItem)sender).Tag;
			//Permission already validated.
			ReportNonModalSelection selection = FormReportsMore.OpenReportHelper(displayReport, ContrAppt2.GetDateSelected(), doValidatePerm: false);
			NonModalReportSelectionHelper(selection);
		}

		private void NonModalReportSelectionHelper(ReportNonModalSelection selection)
		{
			switch (selection)
			{
				case ReportNonModalSelection.TreatmentFinder:
					FormRpTreatmentFinder FormT = new FormRpTreatmentFinder();
					FormT.Show();
					break;
				case ReportNonModalSelection.OutstandingIns:
					FormRpOutstandingIns FormOI = new FormRpOutstandingIns();
					FormOI.Show();
					break;
				case ReportNonModalSelection.UnfinalizedInsPay:
					FormRpUnfinalizedInsPay FormU = new FormRpUnfinalizedInsPay();
					FormU.Show();
					break;
				case ReportNonModalSelection.UnsentClaim:
					FormRpClaimNotSent FormCNS = new FormRpClaimNotSent();
					FormCNS.Show();
					break;
				case ReportNonModalSelection.WebSchedAppointments:
					FormWebSchedAppts formWSA = new FormWebSchedAppts(true, true, true);
					formWSA.Show();
					break;
				case ReportNonModalSelection.CustomAging:
					FormRpCustomAging FormCAO = new FormRpCustomAging();
					FormCAO.Show();
					break;
				case ReportNonModalSelection.IncompleteProcNotes:
					FormRpProcNote FormPN = new FormRpProcNote();
					FormPN.Show();
					break;
				case ReportNonModalSelection.ProcNotBilledIns:
					FormRpProcNotBilledIns FormProc = new FormRpProcNotBilledIns();
					//Both FormRpProcNotBilledIns and FormClaimsSend are non-modal.
					//If both forms are open try and update FormClaimsSend to reflect any newly created claims.
					FormProc.OnPostClaimCreation += () => ContrManage2.TryRefreshFormClaimSend();
					FormProc.FormClosed += (s, ea) => { ODEvent.Fired -= formProcNotBilled_GoToChanged; };
					ODEvent.Fired += formProcNotBilled_GoToChanged;
					FormProc.Show();//FormProcSend has a GoTo option and is shown as a non-modal window.
					FormProc.BringToFront();
					break;
				case ReportNonModalSelection.ODProcsOverpaid:
					FormRpProcOverpaid FormPO = new FormRpProcOverpaid();
					FormPO.Show();
					break;
				case ReportNonModalSelection.DPPOvercharged:
					FormRpDPPOvercharged FormDPPOvercharged = new FormRpDPPOvercharged();
					FormDPPOvercharged.Show();
					break;
				case ReportNonModalSelection.None:
				default:
					//Do nothing.
					break;
			}
		}

		private void formProcNotBilled_GoToChanged(ODEventArgs e)
		{
			if (e.EventType != EventCategory.FormProcNotBilled_GoTo)
			{
				return;
			}
			Patient pat = Patients.GetPat((long)e.Tag);
			FormOpenDental.S_Contr_PatientSelected(pat, false);
			GotoModule.GotoClaim((long)e.Tag);
		}

		private void UserQuery_ClickEvent(object sender, EventArgs e)
		{
			UserQuery userQuery = (UserQuery)((MenuItem)sender).Tag;
			ExecuteQueryFavorite(userQuery);
		}

		private void ExecuteQueryFavorite(UserQuery userQuery)
		{
			SecurityLogs.MakeLogEntry(Permissions.UserQuery, 0, "User query form accessed.");
			//ReportSimpleGrid report=new ReportSimpleGrid();
			if (userQuery.IsPromptSetup && UserQueries.ParseSetStatements(userQuery.QueryText).Count > 0)
			{
				//if the user is not a query admin, they will not have the ability to edit 
				//the query before it is run, so show them the SET statement edit window.
				FormQueryParser FormQP = new FormQueryParser(userQuery);
				FormQP.ShowDialog();
				if (FormQP.DialogResult != DialogResult.OK)
				{
					//report.Query=userQuery.QueryText;
					return;
				}
			}
			if (_formUserQuery != null)
			{
				_formUserQuery.textQuery.Text = userQuery.QueryText;
				_formUserQuery.textTitle.Text = userQuery.FileName;
				_formUserQuery.SubmitQueryThreaded();
				_formUserQuery.BringToFront();
				return;
			}
			_formUserQuery = new FormQuery(null, true);
			_formUserQuery.FormClosed += new FormClosedEventHandler((object senderF, FormClosedEventArgs eF) => { _formUserQuery = null; });
			_formUserQuery.textQuery.Text = userQuery.QueryText;
			_formUserQuery.textTitle.Text = userQuery.FileName;
			_formUserQuery.Show();
		}

		#endregion

		#region Help

		//Help
		private void menuItemRemote_Click(object sender, System.EventArgs e)
		{
			string site = "http://www.opendental.com/contact.html";
			if (Programs.GetCur(ProgramName.BencoPracticeManagement).Enabled)
			{
				site = "https://support.benco.com/";
			}
			try
			{
				Process.Start(site);
			}
			catch (Exception)
			{
				MessageBox.Show("Could not find" + " " + site + "\r\n"
					+ "Please set up a default web browser.");
			}
			/*
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"A remote connection will now be attempted. Do NOT continue unless you are already on the phone with us.  Do you want to continue?"))
			{
				return;
			}
			try{
				Process.Start("remoteclient.exe");//Network streaming remote client or any other similar client
			}
			catch{
				MessageBox.Show("Could not find file.");
			}*/
		}

		private void menuItemHelpWindows_Click(object sender, System.EventArgs e)
		{
			try
			{
				Process.Start("Help.chm");
			}
			catch
			{
				MessageBox.Show("Could not find file.");
			}
		}

		private void menuItemHelpContents_Click(object sender, System.EventArgs e)
		{
			try
			{
				Process.Start("https://www.opendental.com/manual/manual.html");
			}
			catch
			{
				MessageBox.Show("Could not find file.");
			}
		}

		private void menuItemHelpIndex_Click(object sender, System.EventArgs e)
		{
			try
			{
				Process.Start("https://www.opendental.com/site/searchsite.html");
			}
			catch
			{
				MessageBox.Show("Could not find file.");
			}
		}

		private void menuItemWebinar_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start("https://opendental.com/webinars/webinars.html");
			}
			catch
			{
				MessageBox.Show("Could not open page.");
			}
		}

		private void menuItemRemoteSupport_Click(object sender, EventArgs e)
		{
			//Check the installation directory for the GoToAssist corporate exe.
			string fileGTA = CodeBase.ODFileUtils.CombinePaths(Application.StartupPath, "GoToAssist_Corporate_Customer_ver11_9.exe");
			try
			{
				if (!File.Exists(fileGTA))
				{
					throw new ApplicationException();//No message because a different message shows below.
				}
				//GTA exe is available, so load it up
				Process.Start(fileGTA);
			}
			catch
			{
				MessageBox.Show("Could not find file.  Please use Online Support instead.");
			}
		}

		private void MenuItemQueryMonitor_Click(object sender, EventArgs e)
		{
			var thread = new Thread(() => new FormQueryMonitor().ShowDialog());

			thread.SetApartmentState(ApartmentState.STA);
			thread.Name = "QueryMonitorThread";
			thread.Start();
		}

		private void menuItemRequestFeatures_Click(object sender, EventArgs e)
		{
			FormFeatureRequest FormF = new FormFeatureRequest();
			FormF.Show();
		}

		private void MenuItemSupportStatus_Click(object sender, EventArgs e)
		{
			FormSupportStatus formSS = new FormSupportStatus();
			formSS.Show();
		}

		private void menuItemUpdate_Click(object sender, System.EventArgs e)
		{
			//If A to Z folders are disabled, this menu option is unavailable, since
			//updates are handled more automatically.
			FormUpdate FormU = new FormUpdate();
			FormU.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Update Version");
		}

		private void menuItemAbout_Click(object sender, System.EventArgs e)
		{
			FormAbout FormA = new FormAbout();
			FormA.ShowDialog();
		}
		#endregion

		#region Startup methods
		///<summary></summary>
		private void ProcessCommandLine(string[] args)
		{
			//if(!Programs.UsingEcwTight() && args.Length==0){
			if (!Programs.UsingEcwTightOrFullMode() && args.Length == 0)
			{//May have to modify to accept from other sw.
				SetModuleSelected();
				return;
			}
			/*string descript="";
			for(int i=0;i<args.Length;i++) {
				if(i>0) {
					descript+="\r\n";
				}
				descript+=args[i];
			}
			MessageBox.Show(descript);*/
			/*
			PatNum (the integer primary key)
			ChartNumber (alphanumeric)
			SSN (exactly nine digits. If required, we can gracefully handle dashes, but that is not yet implemented)
			UserName
			Password*/
			long patNum = 0;
			string chartNumber = "";
			string ssn = "";
			string userName = "";
			string passHash = "";
			string aptNum = "";
			string ecwConfigPath = "";
			long userId = 0;
			string jSessionId = "";
			string jSessionIdSSO = "";
			string lbSessionId = "";
			Dictionary<string, EnumModuleType> dictModules = new Dictionary<string, EnumModuleType>();
			dictModules.Add("appt", EnumModuleType.Appointments);
			dictModules.Add("family", EnumModuleType.Family);
			dictModules.Add("account", EnumModuleType.Account);
			dictModules.Add("txplan", EnumModuleType.TreatPlan);
			dictModules.Add("treatplan", EnumModuleType.TreatPlan);
			dictModules.Add("chart", EnumModuleType.Chart);
			dictModules.Add("images", EnumModuleType.Images);
			dictModules.Add("manage", EnumModuleType.Manage);
			EnumModuleType startingModule = EnumModuleType.None;
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith("PatNum=") && args[i].Length > 7)
				{
					string patNumStr = args[i].Substring(7).Trim('"');
					try
					{
						patNum = Convert.ToInt64(patNumStr);
					}
					catch { }
				}
				if (args[i].StartsWith("ChartNumber=") && args[i].Length > 12)
				{
					chartNumber = args[i].Substring(12).Trim('"');
				}
				if (args[i].StartsWith("SSN=") && args[i].Length > 4)
				{
					ssn = args[i].Substring(4).Trim('"');
				}
				if (args[i].StartsWith("UserName=") && args[i].Length > 9)
				{
					userName = args[i].Substring(9).Trim('"');
				}
				if (args[i].StartsWith("PassHash=") && args[i].Length > 9)
				{
					passHash = args[i].Substring(9).Trim('"');
				}
				if (args[i].StartsWith("AptNum=") && args[i].Length > 7)
				{
					aptNum = args[i].Substring(7).Trim('"');
				}
				if (args[i].StartsWith("EcwConfigPath=") && args[i].Length > 14)
				{
					ecwConfigPath = args[i].Substring(14).Trim('"');
				}
				if (args[i].StartsWith("UserId=") && args[i].Length > 7)
				{
					string userIdStr = args[i].Substring(7).Trim('"');
					try
					{
						userId = Convert.ToInt64(userIdStr);
					}
					catch { }
				}
				if (args[i].StartsWith("JSESSIONID=") && args[i].Length > 11)
				{
					jSessionId = args[i].Substring(11).Trim('"');
				}
				if (args[i].StartsWith("JSESSIONIDSSO=") && args[i].Length > 14)
				{
					jSessionIdSSO = args[i].Substring(14).Trim('"');
				}
				if (args[i].StartsWith("LBSESSIOINID=") && args[i].Length > 12)
				{
					lbSessionId = args[i].Substring(12).Trim('"');
				}
				if (args[i].ToLower().StartsWith("module=") && args[i].Length > 7)
				{
					string moduleName = args[i].Substring(7).Trim('"').ToLower();
					if (dictModules.ContainsKey(moduleName))
					{
						startingModule = dictModules[moduleName];
					}
				}
				if (args[i].ToLower().StartsWith("show=") && args[i].Length > 5)
				{
					_StrCmdLineShow = args[i].Substring(5).Trim('"').ToLower();
				}
			}
			if (ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.eClinicalWorks), "IsLBSessionIdExcluded") == "1" //if check box in Program Links is checked
				&& lbSessionId == "" //if lbSessionId not previously set
				&& args.Length > 0 //there is at least one argument passed in
				&& !args[args.Length - 1].StartsWith("LBSESSIONID="))//if there is an argument that is the last argument that is not called "LBSESSIONID", then use that argument, including the "name=" part
			{
				//An example of this is command line includes LBSESSIONID= icookie=ECWAPP3ECFH. The space makes icookie a separate parameter. We want to set lbSessionId="icookie=ECWAPP3ECFH". 
				//We are not guaranteed that the parameter is always going to be named icookie, in fact it will be different on each load balancer depending on the setup of the LB.  
				//Therefore, we cannot look for parameter name, but Aislinn from eCW guaranteed that it would be the last parameter every time during our (Cameron and Aislinn's) conversation on 3/5/2014.
				//jsalmon - This is very much a hack but the customer is very large and needs this change ASAP.  Nathan has suggested that we create a ticket with eCW to complain about this and make them fix it.
				lbSessionId = args[args.Length - 1].Trim('"');
			}
			#region eCW bridge
			Bridges.ECW.AptNum = PIn.Long(aptNum);
			Bridges.ECW.EcwConfigPath = ecwConfigPath;
			Bridges.ECW.UserId = userId;
			Bridges.ECW.JSessionId = jSessionId;
			Bridges.ECW.JSessionIdSSO = jSessionIdSSO;
			Bridges.ECW.LBSessionId = lbSessionId;
			#endregion
			#region UserName and PassHash
			//Only consider username and password here when not in Middle Tier mode.
			//If credentials were passed in the command line arguments for Middle Tier, they were already considered in the Choose Database window.

			//Users are allowed to use eCW tight integration without command line.  They can manually launch Open Dental.
			//We always want to trigger login window for eCW tight, even if no username was passed in.
			if ((Programs.UsingEcwTightOrFullMode() && Security.CurrentUser == null)
				//Or if a username was passed in and it's different from the current user
				|| (userName != "" && (Security.CurrentUser == null || Security.CurrentUser.UserName != userName)))
			{
				//Use the username and passhash that was passed in to determine which user to log in
				//log out------------------------------------
				LastModule = moduleBar.SelectedModule;
				moduleBar.SelectedModule = EnumModuleType.None;
				moduleBar.Invalidate();
				UnselectActive();
				allNeutral();
				Userod user = Userods.GetUserByName(userName, true);
				if (user == null)
				{
					ShowLogOn();
					user = Security.CurrentUser.Copy();
				}
				//Can't use Userods.CheckPassword, because we only have the hashed password.
				if (passHash != user.PasswordHash || !Programs.UsingEcwTightOrFullMode())
				{//password not accepted or not using eCW
				 //So present logon screen
					ShowLogOn();
				}
				else
				{//password accepted and using eCW tight.
				 //this part usually happens in the logon window
					Security.CurrentUser = user.Copy();
					SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, "User:" + " " + Security.CurrentUser.UserName + " " + "has logged on via command line.");
				}
				moduleBar.SelectedIndex = Security.GetModule(moduleBar.IndexOf(LastModule));
				moduleBar.Invalidate();
				SetModuleSelected();
				Patient pat = Patients.GetPat(CurPatNum);//pat could be null
				Text = PatientL.GetMainTitle(pat, Clinics.ClinicNum);//handles pat==null by not displaying pat name in title bar
				if (userControlTasks1.Visible)
				{
					userControlTasks1.InitializeOnStartup();
				}
				if (moduleBar.SelectedModule == EnumModuleType.None)
				{
					MessageBox.Show("You do not have permission to use any modules.");
				}
			}

			#endregion
			#region Module
			if (startingModule != EnumModuleType.None && moduleBar.IndexOf(startingModule) == Security.GetModule(moduleBar.IndexOf(startingModule)))
			{
				UnselectActive();
				allNeutral();//Sets all controls to false.  Needed to set the new module as selected.
				moduleBar.SelectedModule = startingModule;
				moduleBar.Invalidate();
			}
			SetModuleSelected();
			#endregion
			#region PatNum
			if (patNum != 0)
			{
				Patient pat = Patients.GetPat(patNum);
				if (pat == null)
				{
					CurPatNum = 0;
					RefreshCurrentModule();
					FillPatientButton(null);
				}
				else
				{
					CurPatNum = patNum;
					RefreshCurrentModule();
					FillPatientButton(pat);
				}
			}
			#endregion
			#region ChartNumber
			else if (chartNumber != "")
			{
				Patient pat = Patients.GetPatByChartNumber(chartNumber);
				if (pat == null)
				{
					//todo: decide action
					CurPatNum = 0;
					RefreshCurrentModule();
					FillPatientButton(null);
				}
				else
				{
					CurPatNum = pat.PatNum;
					RefreshCurrentModule();
					FillPatientButton(pat);
				}
			}
			#endregion
			#region SSN
			else if (ssn != "")
			{
				Patient pat = Patients.GetPatBySSN(ssn);
				if (pat == null)
				{
					//todo: decide action
					CurPatNum = 0;
					RefreshCurrentModule();
					FillPatientButton(null);
				}
				else
				{
					CurPatNum = pat.PatNum;
					RefreshCurrentModule();
					FillPatientButton(pat);
				}
			}
			#endregion
			else
			{
				FillPatientButton(null);
			}
		}

		#endregion Startup methods

		private void TryNonPatientPopup()
		{
			if (CurPatNum != 0 && _previousPatNum != CurPatNum)
			{
				_datePopupDelay = DateTime.Now;
				_previousPatNum = CurPatNum;
			}
			if (!Prefs.GetBool(PrefName.ChartNonPatientWarn))
			{
				return;
			}
			Patient patCur = Patients.GetPat(CurPatNum);
			if (patCur != null
						&& patCur.PatStatus.ToString() == "NonPatient"
						&& _datePopupDelay <= DateTime.Now)
			{
				MessageBox.Show("A patient with the status NonPatient is currently selected.");
				_datePopupDelay = DateTime.Now.AddMinutes(5);
			}
		}

		#region LogOn
		///<summary>Logs on a user using the passed in credentials or Active Directory or the good old-fashioned log on window.</summary>
		private void LogOnOpenDentalUser(string odUser, string odPassword)
		{
			//CurUser will be set if using web service because login from ChooseDatabase window.
			if (Security.CurrentUser != null)
			{
				CheckForPasswordReset();
				SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, "User:" + " " + Security.CurrentUser.UserName + " " + "has logged on.");
				return;
			}

			#region Command Line Args
			//Both a username and password was passed in via command line arguments.
			if (odUser != "" && odPassword != "")
			{
				try
				{
					bool isEcwTightOrFullMode = Programs.UsingEcwTightOrFullMode();
					Security.CurrentUser = Userods.CheckUserAndPassword(odUser, odPassword, isEcwTightOrFullMode);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					Application.Exit();
					return;
				}
				SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, "User:" + " " + Security.CurrentUser.UserName + " " + "has logged on via command line.");
			}
			#endregion
			#region Good Old-fashioned Log On
			if (Security.CurrentUser == null)
			{//Security.CurUser could be set if valid command line arguments were passed in.
				#region Admin User No Password
				if (!Userods.HasSecurityAdminUserNoCache())
				{
					MessageBox.Show("There are no users with the SecurityAdmin permission.  Call support.");
					Application.Exit();
					return;
				}
				long userNumFirstAdminNoPass = Userods.GetFirstSecurityAdminUserNumNoPasswordNoCache();
				if (userNumFirstAdminNoPass > 0)
				{
					Security.CurrentUser = Userods.GetUserNoCache(userNumFirstAdminNoPass);
					CheckForPasswordReset();

					SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, "User:" + " " + Security.CurrentUser.UserName + " " + "has logged on.");
				}
				#endregion
				#region Domain Login
				else if (Prefs.GetBool(PrefName.DomainLoginEnabled) && !string.IsNullOrWhiteSpace(Prefs.GetString(PrefName.DomainLoginPath)))
				{
					string loginPath = Prefs.GetString(PrefName.DomainLoginPath);
					try
					{
						DirectoryEntry loginEntry = new DirectoryEntry(loginPath);
						string distinguishedName = loginEntry.Properties["distinguishedName"].Value.ToString();
						string domainGuid = loginEntry.Guid.ToString();
						string domainGuidPref = Prefs.GetString(PrefName.DomainObjectGuid);
						if (domainGuidPref.IsNullOrEmpty())
						{
							//Domain login was setup before we started recording the domain's ObjectGuid. We will save it now for future use.
							Prefs.Set(PrefName.DomainObjectGuid, domainGuid);
							domainGuidPref = domainGuid;
						}
						//All LDAP servers must expose a special entry, called the root DSE. This gets the current user's domain path.
						DirectoryEntry rootDSE = new DirectoryEntry("LDAP://RootDSE");
						string defaultNamingContext = rootDSE.Properties["defaultNamingContext"].Value.ToString();
						if (//If the domain of the current user doesn't match the provided LDAP Path
							!distinguishedName.ToLower().Contains(defaultNamingContext.ToLower()) ||
							//Or the domain's ObjectGuid does not match what's in the database
							domainGuid != domainGuidPref)
						{
							ShowLogOn();
							return;
						}
						SerializableDictionary<long, string> dictDomainUserNumsAndNames = Userods.GetUsersByDomainUserNameNoCache(Environment.UserName);
						if (dictDomainUserNumsAndNames.Count == 0)
						{ //Log on normally if no user linked the current domain user
							ShowLogOn();
						}
						else if (dictDomainUserNumsAndNames.Count > 1)
						{ //Select a user if multiple users linked to the current domain user
							InputBox box = new InputBox("Select an Open Dental user to log in with:", dictDomainUserNumsAndNames.Select(x => x.Value).ToList());
							box.ShowDialog();
							if (box.DialogResult == DialogResult.OK)
							{
								Security.CurrentUser = Userods.GetUserNoCache(dictDomainUserNumsAndNames.Keys.ElementAt(box.SelectedIndex));
								CheckForPasswordReset();

								SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, "User:" + " " + Security.CurrentUser.UserName + " "
									+ "has logged on automatically via ActiveDirectory.");
							}
							else
							{
								ShowLogOn();
							}
						}
						else
						{ //log on automatically if only one user is linked to current domain user
							Security.CurrentUser = Userods.GetUserNoCache(dictDomainUserNumsAndNames.Keys.First());
							CheckForPasswordReset();

							SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, "User:" + " " + Security.CurrentUser.UserName + " "
									+ "has logged on automatically via ActiveDirectory.");
						}
					}
					catch
					{
						ShowLogOn();
						return;
					}
				}
				#endregion
				#region Manual LogOn Window
				else
				{
					ShowLogOn();
				}
				#endregion
			}
			#endregion
		}

		///<summary>Show the log on window.</summary>
		private void ShowLogOn()
		{
            FormLogOn_ = new FormLogOn();
			FormLogOn_.ShowDialog(this);
			if (FormLogOn_.DialogResult != DialogResult.OK)
			{
				//Using FormLogOn_.DialogResult==DailogResult.CANCEL previously resulted in a null user/UE.
				Cursor = Cursors.Default;
				Application.Exit();
			}
			CheckForPasswordReset();

            if (FormLogOn_.RefreshSecurityCache)
			{//Refresh the cache if we need to since cache allowed was just set to true
				DataValid.SetInvalid(InvalidType.Security);
			}
		}

		/// <summary>
		/// Checks to see if the currently logged-in user needs to reset their password.
		/// If they do, then this method will force the user to reset the password otherwise the program will exit.
		/// </summary>
		private void CheckForPasswordReset()
		{
			if (Security.CurrentUser == null) return;

			try
			{
				if (Security.CurrentUser.IsPasswordResetRequired)
				{
                    using var formUserPassword = new FormUserPassword(Security.CurrentUser.UserName, true);

                    if (formUserPassword.ShowDialog() == DialogResult.Cancel) // Shouldn't be possible
                    {
                        Cursor = Cursors.Default;

                        Application.Exit();
                    }

                    bool isPasswordStrong = formUserPassword.PasswordIsStrong;
                    try
                    {
                        Security.CurrentUser.IsPasswordResetRequired = false;

                        Userods.Update(Security.CurrentUser);
                        Userods.UpdatePassword(Security.CurrentUser, formUserPassword.PasswordHash, isPasswordStrong);

                        Security.CurrentUser = Userods.GetUserNoCache(Security.CurrentUser.Id);//UpdatePassword() changes multiple fields.  Refresh from db.
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
			}
			finally
			{
				//Record last login time always, regardless of if we are changing password or not.
				//This catches domain logins, middle-tier, regular login, and logging in with passwords disabled.
				try
				{
					Security.CurrentUser.DateTLastLogin = DateTime.Now;
					Userods.Update(Security.CurrentUser);//Unfortunately there is no update(new,old) for Userods yet due to comlexity.
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			DataValid.SetInvalid(InvalidType.Security);
		}
		#endregion LogOn

		#region Logoff

		///<summary>Returns a list of forms that are currently open excluding FormOpenDental and FormLogOn.
		///This method is typically called in order to close any open forms sans the aforementioned forms.
		///Therefore, the list returned is ordered with the intent that the calling method will close children first and then parents last.</summary>
		private List<Form> GetOpenForms()
		{
			if (this.InvokeRequired)
			{
				return (List<Form>)this.Invoke(new Func<List<Form>>(() => GetOpenForms()));
			}
			List<Form> listOpenForms = new List<Form>();
			for (int f = Application.OpenForms.Count - 1; f >= 0; f--)
			{//Loop backwards assuming children are added later in the collection.
				Form openForm = Application.OpenForms[f];
				if (openForm == this)
				{// main form
					continue;
				}
				if (openForm.Name == "FormLogOn")
				{
					continue;
				}
				listOpenForms.Add(Application.OpenForms[f]);
			}
			return listOpenForms;
		}

		///<summary>Enumerates open forms and saves work for those forms which have a save handler.  Some forms are closed as part of saving work.</summary>
		private bool SaveWork(bool isForceClose)
		{
			if (this.InvokeRequired)
			{
				return (bool)this.Invoke(new Func<bool>(() => SaveWork(isForceClose)));
			}
			List<Form> listOpenForms = GetOpenForms();
			foreach (Form openForm in listOpenForms)
			{
				//If force closing, we HAVE to forcefully close everything related to Open Dental, regardless of plugins.  Otherwise, give plugins a chance to stop the log off event.
				if (!isForceClose)
				{
					//This hook was moved into this method so that the form closing loop could be shared.
					//It is correctly named and was not updated to say "FormOpenDental.CloseOpenForms" on purpose for backwards compatibility.
					if (Plugins.HookMethod(this, "FormOpenDental.LogOffNow_loopingforms", openForm))
					{
						continue;//if some criteria are met in the hook, don't close a certain form
					}
				}
				if (openForm.Name == "FormWikiEdit")
				{
					if (!isForceClose)
					{
						if (!MsgBox.Show(MsgBoxButtons.OKCancel, "You are currently editing a wiki page and it will be saved as a draft.  Continue?"))
						{
							return false;//This form needs to stay open and the close operation should be aborted.
						}
					}
				}
			}
			GeneralProgramEvent.Fire(EventCategory.Shutdown, isForceClose);
			foreach (Form formToClose in listOpenForms)
			{
				if (formToClose.Name == "FormWikiEdit")
				{
					WikiSaveEvent.Fire(EventCategory.WikiSave);
				}
				if (formToClose.Name == "FormCommItem")
				{
					CommItemSaveEvent.Fire(EventCategory.CommItemSave, "ShutdownAllWorkstations");
				}
				if (formToClose.Name == "FormEmailMessageEdit")
				{
					EmailSaveEvent.Fire(EventCategory.EmailSave);
				}
			}
			return true;
		}

		///<summary>Do not call this function inside of an invoke, or else the form closing events will not return from ShowDialog() calls in time.
		///Closes all open forms except FormOpenDental.  Set isForceClose to true if you want to close all forms asynchronously.  Set 
		///forceCloseTimeoutMS when isForceClose is set to true to specify a timeout value for forms that take too long to close, e.g. a form hanging in 
		///a FormClosing event on a MessageBox.  If the timeout value is reached, the program will exit.  E.g. FormWikiEdit will ask users on closing if 
		///they are sure they want to discard unsaved work.  Returns false if there is an open form that requests attention, thus needs to stop the 
		///closing of the forms.</summary>
		private bool CloseOpenForms(bool isForceClose, int forceCloseTimeoutMS = 15000)
		{
			if (!SaveWork(isForceClose))
			{
				return false;
			}
			List<Form> listCloseForms = GetOpenForms();
			#region Close forms and quit threads.  Some form closing events rely on the closing events of parent forms.
			while (listCloseForms.Count > 0)
			{
				Form formToClose = listCloseForms[0];
				bool hasShown = false;
				while (!hasShown)
				{
					hasShown = true;//In case not inherited ODFormAbs.
					if (formToClose is ODForm)
					{
						hasShown = ((ODForm)formToClose).HasShown;
					}
					else if (formToClose.GetType().GetProperty("HasShown") != null)
					{
						//Is a Form and has property HasShown => Assume is an ODFormAbs.  Ex FormHelpBrowser is not an ODForm.
						hasShown = (bool)formToClose.GetType().GetProperty("HasShown").GetValue(formToClose);
					}
					//Window handle has not been created yet.  Calling formToClose.Invoke() will throw an InvalidOperationException.
					//GetOpenForms() will return a subset of forms which are in the Application.OpenForms array.
					//A form can only be placed into the Application.OpenForms array if Show() or ShowDialog() is called on it.
					//The fact that Show() or ShowDialog() was called on the form, means that the form constructor ran completely before showing,
					//therefore nothing in the constructor could cause the loop to lock up here.					
					//When Show() or ShowDialog() is called, the form will fire the Load() event followed by the Shown() event.
					//Therefore, a form returned by GetOpenForms() will fire the Load() event of the form, then the Shown() event.
					//If the form gets stuck in the Load() event due to an infinite or very long waiting period (ex Lan.F() lost connection to database),
					//then the main thread will lock up and CloseOpenForms() thread will not fire and we will never get here anyway,
					//unless CloseOpenForms() is called from a thread, in which case sleeping will not affect the main loop.
					//Finally, since we are here and the form will eventually fire the Shown() event, we can count on HasShown quickly becoming true.
					if (!hasShown)
					{
						Thread.Sleep(100);//Check 10 times per second.
					}
				}
				if (isForceClose)
				{
					ODThread threadCloseForm = new ODThread((o) =>
					{
						if (!IsDisposedOrClosed(formToClose))
						{
							formToClose.Invoke(formToClose.Close);
						}
					});
					threadCloseForm.Name = "ForceCloseForm";
					bool hasError = false;
					threadCloseForm.AddExceptionHandler((ex) =>
					{
						hasError = true;
						ODException.SwallowAnyException(() =>
						{
							//A FormClosing() or FormClosed() event caused an exception.  Try to submit the exception so that we are made aware.
							//BugSubmissions.SubmitException(new ODException(
							//		"Form failed to close when force closing.\r\n"
							//		+"FormName: "+formToClose.Name+"\r\n"
							//		+"FormType: "+formToClose.GetType().FullName+"\r\n"
							//		+"FormIsODForm: "+((formToClose is ODForm)?"Yes":"No")+"\r\n"
							//		+"FormIsDiposed: "+(ODForm.IsDisposedOrClosed(formToClose)?"Yes":"No")+"\r\n"
							//		,"",ex),
							//	threadCloseForm.Name);
						});
					});
					threadCloseForm.Start();
					threadCloseForm.Join(1000);//Give the form a limited amount of time to close, and continue if not responsive.
					this.Invoke(Application.DoEvents);//Run on main thread so that ShowDialog() for the form will continue in the parent context immediately.
					if (hasError || !IsDisposedOrClosed(formToClose))
					{
						formToClose.Invoke(formToClose.Dispose);//If failed to close, kill window so that the ShowDialog() call can continue in parent context.
					}
					//In case the form we just closed created new popup forms inside the FormClosing or FormClosed event,
					//we need to check for newly created forms and add them to the queue of forms to close.
					//Any new forms will be closed next, so that child forms are closed as soon as possible before closing any parent forms.
					List<Form> listNewForms = GetOpenForms();
					listCloseForms.ForEach(x => listNewForms.Remove(x));
					foreach (Form brandNewForm in listNewForms)
					{
						listCloseForms.Insert(0, brandNewForm);
					}
				}
				else
				{//User manually chose to logoff/shutdown.  Gracefully close each window.
				 //If the window which showed the messagebox popup causes the form to stay open, then stop the log off event, because the user chose to.
					formToClose.InvokeIfRequired(() => formToClose.Close());//Attempt to close the form, even if created in another thread (ex FormHelpBrowser).
																			//Run Applicaiton.DoEvents() to allow the FormClosing/FormClosed events to fire in the form before checking if they have closed below.
					Application.DoEvents();//Required due to invoking.  Otherwise FormClosing/FormClosed will not fire until after we exit CloseOpenForms.
					if (!IsDisposedOrClosed(formToClose))
					{
						//E.g. The wiki edit window will ask users if they want to lose their work or continue working.  This will get hit if they chose to continue working.
						return false;//This form needs to stay open and stop all other forms from being closed.
					}
				}
				listCloseForms.Remove(formToClose);
			}
			#endregion
			return true;//All open forms have been closed at this point.
		}

		private void LogOffNow()
		{
			bool isForceClose = PrefC.LogOffTimer > 0;
			LogOffNow(isForceClose);
		}

		public void LogOffNow(bool isForced)
		{
			if (!CloseOpenForms(isForced))
			{
				return;//A form is still open.  Do not continue to log the user off.
			}
			FinishLogOff(isForced);
		}

		private void FinishLogOff(bool isForced)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(() => { FinishLogOff(isForced); });
				return;
			}
			Plugins.HookAddCode(this, "FormOpenDental.FinishLogOff_start", isForced);//perform logoff 
			NullUserCheck("FinishLogOff");
			LastModule = moduleBar.SelectedModule;
			moduleBar.SelectedModule = EnumModuleType.None;
			moduleBar.Invalidate();
			UnselectActive(true);
			allNeutral();
			ContrChart2.UserLogOffCommited();//Ensures that we refresh view when user logs back on or a new user logs on.
			if (userControlTasks1.Visible)
			{
				userControlTasks1.ClearLogOff();
			}
			if (isForced)
			{
				SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, "User: " + Security.CurrentUser.UserName + " has auto logged off.");
			}
			else
			{
				SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, "User: " + Security.CurrentUser.UserName + " has logged off.");
			}
			Clinics.LogOff();
			Userod oldUser = Security.CurrentUser;
			Security.CurrentUser = null;
			_listReminderTasks = null;
			_listNormalTaskNums = null;
			ContrAppt2.RefreshReminders(new List<Task>());
			RefreshTasksNotification();
			Text = PatientL.GetMainTitle(null, 0);
			SetTimersAndThreads(false);//Safe to stop timers since this method was invoked on the main thread if required.
			userControlPatientDashboard.CloseDashboard(true);
			ShowLogOn();

			//If a different user logs on and they have clinics enabled, and they don't want data to persist, then clear the patient drop down history
			//since the current user may not have permission to access patients from the same clinic(s) as the old user
			if (oldUser.Id == Security.CurrentUser.Id || !PrefC.HasClinicsEnabled)
			{
				//continue //we do not need to blank out patient or menu
			}
			else if (!Prefs.GetBool(PrefName.PatientMaintainedOnUserChange))
			{//not the same user and clinics are enabled. 
				CurPatNum = 0;
				PatientL.RemoveAllFromMenu(menuPatient);
			}
			else
			{
				//Preserve data if new user is allowed to access the same clinics
				List<Clinic> listClinicsForUser = Clinics.GetAllForUserod(Security.CurrentUser);
				List<Patient> listMenuPatsLim = PatientL.GetPatientsLimFromMenu();
				foreach (Patient patLim in listMenuPatsLim)
				{
					if (!patLim.ClinicNum.In(listClinicsForUser.Select(x => x.ClinicNum)))
					{
						//user is not allowed to access the clinic this patient is located in, remove.
						PatientL.RemoveFromMenu(patLim.PatNum);
						if (CurPatNum == patLim.PatNum)
						{
							CurPatNum = 0;
						}
					}
				}
			}
			moduleBar.SelectedIndex = Security.GetModule(moduleBar.IndexOf(LastModule));
			moduleBar.Invalidate();
			if (PrefC.HasClinicsEnabled)
			{
				Clinics.LoadClinicNumForUser();
				RefreshMenuClinics();
			}
			SetModuleSelected();
			Patient pat = Patients.GetPat(CurPatNum);//pat could be null
			Text = PatientL.GetMainTitle(pat, Clinics.ClinicNum);//handles pat==null by not displaying pat name in title bar
			FillPatientButton(pat);
			if (userControlTasks1.Visible)
			{
				userControlTasks1.InitializeOnStartup();
			}
			BeginODDashboardStarterThread();
			SetTimersAndThreads(true);//Safe to start timers since this method was invoked on the main thread if required.
									  //User logged back in so log on form is no longer the active window.
			IsFormLogOnLastActive = false;
			Security.DateTimeLastActivity = DateTime.Now;
			if (moduleBar.SelectedModule == EnumModuleType.None)
			{
				MessageBox.Show("You do not have permission to use any modules.");
			}
		}

		///<summary>Call this method in places where Security.CurUser should not be null in order to
		///notify HQ with additional information when a null Security.CurUser is detected.  Does nothing if CurUser is not null.</summary>
		private void NullUserCheck(string methodName)
		{
			if (Security.CurrentUser != null)
			{
				return;
			}
			StringBuilder strBld = new StringBuilder("OpenForms:");
			foreach (Form form in Application.OpenForms)
			{
				strBld.Append($"\r\n  {(form == null ? "Unknown" : form.Name)}");
			}
			//ODException.SwallowAnyException(() => BugSubmissions.SubmitException(new ODException("Null user detected during log off.\r\n"
			//	+$"Method: {methodName}\r\n"
			//	+$"ActiveForm.Name: {(ODForm.ActiveForm==null ? "Unknown" : ODForm.ActiveForm.Name)}\r\n"
			//	+strBld.ToString()))
			//);
		}
		#endregion Logoff

		private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
		{
			if (e.Reason != SessionSwitchReason.SessionLock)
			{
				return;
			}
			//CurUser will be null if Open Dental is already in a 'logged off' state.  Check Security.IsUserLoggedIn as well because Middle Tier does not 
			//set CurUser to null when logging off.
			//Also catches the case where Open Dental has NEVER connected to a database yet and checking PrefC would throw an exception (no db conn).
			if (Security.CurrentUser == null || !Security.IsUserLoggedIn)
			{
				return;
			}
			if (!Prefs.GetBool(PrefName.SecurityLogOffWithWindows))
			{
				return;
			}
			LogOffNow(true);
		}

		private void FormOpenDental_Deactivate(object sender, EventArgs e)
		{
			//There is a chance that the user has gone to a non-modal form (e.g. task) and can change the patient from that form.
			//We need to save the Treatment Note in the chart module because the "on leave" event might not get fired for the text box.
			if (ContrChart2.TreatmentNoteChanged)
			{
				ContrChart2.UpdateTreatmentNote();
			}
			if (ContrAccount2.UrgFinNoteChanged)
			{
				ContrAccount2.UpdateUrgFinNote();
			}
			if (ContrAccount2.FinNoteChanged)
			{
				ContrAccount2.UpdateFinNote();
			}
			if (ContrTreat2.HasNoteChanged)
			{
				ContrTreat2.UpdateTPNoteIfNeeded();
			}
		}

		private void FormOpenDental_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing && Security.CurrentUser != null && Security.IsUserLoggedIn)
			{
				if (!AreYouSurePrompt(Security.CurrentUser.Id, "Are you sure you would like to close?"))
				{
					e.Cancel = true;
					return;
				}
			}

			try
			{
				FormOpenDentalClosing(sender, e);
			}
			catch
			{
				try
				{
					//Allow the program to close quietly, but send us at HQ a bug report so we can look into the problem.
					//BugSubmissions.SubmitException(ex,patNumCur:CurPatNum);
				}
				catch
				{
				}
			}
		}

		private void FormOpenDentalClosing(object sender, FormClosingEventArgs e)
		{
			//ExitCode will only be set if trying to silently update.  
			//If we start using ExitCode for anything other than silently updating, this can be moved towards the bottom of this closing.
			//If moved to the bottom, all of the clean up code that this closing event does needs to be considered in regards to updating silently from a CEMT computer.
			if (ExitCode != 0)
			{
				Environment.Exit(ExitCode);
			}
			bool hadMultipleFormsOpen = (Application.OpenForms.Count > 1);
			//CloseOpenForms should have already been called with isForceClose=true if we are force closing Open Dental
			//In that scenario, calling CloseOpenForms with isForceClose=false should not leave the program open.
			//However, if Open Dental is closing from any other means, we want to give all forms the opportunity to stop closing.
			//Example, if you have FormWikiEdit open, it will attempt to save it as a draft unless the user wants to back out.
			if (!CloseOpenForms(false))
			{
				e.Cancel = true;
				return;
			}
			if (hadMultipleFormsOpen)
			{
				//If this form is closing because someone called Application.Exit, then the call above to CloseOpenForms would cause an exception later in 
				//Application.Exit because CloseOpenForms altered a collection inside a foreach loop inside of Application.Exit. We still want to exit, but
				//we need to start afresh in order to not cause an exception.
				e.Cancel = true;
				this.BeginInvoke(() => Application.Exit());
				return;
			}
			//Put any meaningful code below this point. The hadMultipleFormsOpen check above can lead to this method being invoked twice.
			//The first invoke only exists to invoke the second invoke.
			try
			{
				Programs.ScrubExportedPatientData();//Required for EHR module d.7.
			}
			catch
			{
				//Can happen if cancel is clicked in Choose Database window.
			}
			try
			{
				Computers.ClearHeartBeat(Environment.MachineName);
			}
			catch { }

			FormUAppoint.AbortThread();

			ODThread.QuitSyncThreadsByGroupName(0, "");

			if (Security.CurrentUser != null)
			{
				try
				{
					SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, "User: " + Security.CurrentUser.UserName + " has logged off.");
					Clinics.LogOff();
				}
				catch
				{
				}
			}
			//Per https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.systemevents.sessionswitch?view=netframework-4.7.2 we need to unsubscribe 
			//from the SessionSwitch event "Because this is a static event, you must detach your event handlers when your application is disposed, or 
			//memory leaks will result."
			SystemEvents.SessionSwitch -= new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
			//if(Prefs.GetBool(PrefName.DistributorKey)) {//for OD HQ
			//  for(int f=Application.OpenForms.Count-1;f>=0;f--) {
			//    if(Application.OpenForms[f]==this) {// main form
			//      continue;
			//    }
			//    Application.OpenForms[f].Close();
			//  }
			//}
			string tempPath = "";
			string[] arrayFileNames;
			List<string> listDirectories;
			try
			{

				// TODO: Cleanup temp files...

				tempPath = Storage.GetTempPath();
				arrayFileNames = Directory.GetFiles(tempPath, "*.*", SearchOption.AllDirectories);//All files in the current directory plus all files in all subdirectories.
				listDirectories = new List<string>(Directory.GetDirectories(tempPath, "*", SearchOption.AllDirectories));//All subdirectories.
			}
			catch
			{
				//We will only reach here if we error out of getting the temp folder path
				//If we can't get the path, then none of the stuff below matters
				Plugins.HookAddCode(null, "FormOpenDental.FormClosing_end");
				return;
			}
			for (int i = 0; i < arrayFileNames.Length; i++)
			{
				try
				{
					//All files related to updates need to stay.  They do not contain PHI information and will not harm anything if left around.
					if (arrayFileNames[i].Contains("UpdateFileCopier.exe"))
					{
						continue;//Skip any files related to updates.
					}
					//When an update is in progress, the binaries will be stored in a subfolder called UpdateFiles within the temp directory.
					if (arrayFileNames[i].Contains("UpdateFiles"))
					{
						continue;//Skip any files related to updates.
					}
					//The UpdateFileCopier will create temporary backups of source and destination setup files so that it can revert if copying fails.
					if (arrayFileNames[i].Contains("updatefilecopier"))
					{
						continue;//Skip any files related to updates.
					}
					File.Delete(arrayFileNames[i]);
				}
				catch
				{
					//Do nothing because the file could have been in use or there were not sufficient permissions.
					//This file will most likely get deleted next time a temp file is created.
				}
			}
			listDirectories.Sort();//We need to sort so that we know for certain which directories are parent directories of other directories.
			for (int i = listDirectories.Count - 1; i >= 0; i--)
			{//Easier than recursion.  Since the list is ordered ascending, then going backwards means we delete subdirectories before their parent directories.
				try
				{
					//When an update is in progress, the binaries will be stored in a subfolder called UpdateFiles within the temp directory.
					if (listDirectories[i].Contains("UpdateFiles"))
					{
						continue;//Skip any files related to updates.
					}
					//The UpdateFileCopier will create temporary backups of source and destination setup files so that it can revert if copying fails.
					if (listDirectories[i].Contains("updatefilecopier"))
					{
						continue;//Skip any files related to updates.
					}
					Directory.Delete(listDirectories[i]);
				}
				catch
				{
					//Do nothing because the folder could have been in use or there were not sufficient permissions.
					//This folder will most likely get deleted next time Open Dental closes.
				}
			}
			Plugins.HookAddCode(null, "FormOpenDental.FormClosing_end");
		}

		private void FormOpenDental_FormClosed(object sender, FormClosedEventArgs e)
		{
			//Cleanup all resources related to the program which have their Dispose methods properly defined.
			//This helps ensure that the chart module and its tooth chart wrapper are properly disposed of in particular.
			//This step is necessary so that graphics memory does not fill up.
			Dispose();
			//"=====================================================
			//https://msdn.microsoft.com/en-us/library/system.environment.exit%28v=vs.110%29.aspx
			//Environment.Exit Method:
			//Terminates this process and gives the underlying operating system the specified exit code.
			//For the exitCode parameter, use a non-zero number to indicate an error. In your application, you can define your own error codes in an
			//enumeration, and return the appropriate error code based on the scenario. For example, return a value of 1 to indicate that the required file
			//is not present and a value of 2 to indicate that the file is in the wrong format. For a list of exit codes used by the Windows operating
			//system, see System Error Codes in the Windows documentation.
			//Calling the Exit method differs from using your programming language's return statement in the following ways:
			//*Exit always terminates an application. Using the return statement may terminate an application only if it is used in the application entry
			//	point, such as in the Main method.
			//*Exit terminates an application immediately, even if other threads are running. If the return statement is called in the application entry
			//	point, it causes an application to terminate only after all foreground threads have terminated.
			//*Exit requires the caller to have permission to call unmanaged code. The return statement does not.
			//*If Exit is called from a try or finally block, the code in any catch block does not execute. If the return statement is used, the code in the
			//catch block does execute.
			//====================================================="
			//Call Environment.Exit() to kill all threads which we forgot to close.  Also sends exit code 0 to the command line to indicate success.
			//If a thread needs to be gracefully quit, then it is up to the designing engineer to Join() to that thread before we get to this point.
			//We considered trying to get a list of active threads and logging debug information for those threads, but there is no way
			//to get the list of managed threads from the system.  It is our responsibility to keep track of our own managed threads.  There is a way
			//to get the list of unmanaged system threads for our application using Process.GetCurrentProcess().Threads, but that does not help us enough.
			//See http://stackoverflow.com/questions/466799/how-can-i-enumerate-all-managed-threads-in-c.  To keep track of a managed thread, use ODThread.
			//Environment.Exit requires permission for unmanaged code, which we have explicitly specified in the solution already.
			Environment.Exit(0);//Guaranteed to kill any threads which are still running.
		}

		private void menuItemEServices_Click(object sender, EventArgs e)
		{
			FormEServicesSetup formESSetup = new FormEServicesSetup();
			formESSetup.ShowDialog();
		}
    }

	public class PopupEvent : IComparable
	{
		public long PopupNum;

		/// <summary>
		/// Disable this popup until this time.
		/// </summary>
		public DateTime DisableUntil;

		/// <summary>
		/// The last time that this popup popped up.
		/// </summary>
		public DateTime LastViewed;

		public int CompareTo(object obj)
		{
			PopupEvent pop = (PopupEvent)obj;
			return DisableUntil.CompareTo(pop.DisableUntil);
		}

		public override string ToString()
		{
			return PopupNum.ToString() + ", " + DisableUntil.ToString();
		}
	}

	/// <summary>
	/// This is a global class because it must run at the application level in order to catch application level system input events. 
	/// WM_KEYDOWN (0x0100) message details: https://msdn.microsoft.com/en-us/library/windows/desktop/ms646280(v=vs.85).aspx. 
	/// WM_MOUSEMOVE (0x0200) message details: https://msdn.microsoft.com/en-us/library/windows/desktop/ms645616(v=vs.85).aspx. 
	/// </summary>
	public class ODGlobalUserActiveHandler : IMessageFilter
	{
		const int WM_KEYDOWN = 0x0100;
		const int WM_MOUSEMOVE = 0x0200;

		/// <summary>
		/// Compare position of mouse at the time of the message to the previously stored mouse position to correctly identify a mouse movement.
		/// In testing, a mouse will sometimes fire a series of multiple MouseMove events with the same position, possibly due to wireless mouse chatter.
		/// Comparing to previous position allows us to only update the last activity timer when the mouse actually changes position.
		/// </summary>
		private Point previousMousePos;

		/// <summary>
		/// Returning false guarantees that the message will continue to the next filter control. 
		/// Therefore this method inspects the messages, but the messages are not consumed.
		/// </summary>
		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == WM_KEYDOWN)
			{
				Security.DateTimeLastActivity = DateTime.Now;
			}
			else if (m.Msg == WM_MOUSEMOVE && previousMousePos != Cursor.Position) // WM_MOUSEMOVE
			{
				previousMousePos = Cursor.Position;
				Security.DateTimeLastActivity = DateTime.Now;
			}

			return false;
		}
	}
}
