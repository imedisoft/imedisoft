using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using Imedisoft.Forms;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace OpenDental
{
    public partial class FormOpenDental : ODForm
	{
		/// <summary>
		/// Add your thread instance to this list if you only want this thread to only be started once.
		/// </summary>
		private readonly List<ODThread> _listOdThreadsRunOnce = new List<ODThread>();

		private ODThread _odThreadDataConnectionLost;

		/// <summary>
		/// Starts or stops all local timers and threads that should be started and stopped.
		/// Only starts signal timer if interval preference is set to non-zero value.
		/// The Windows Forms timer is designed for use in a single-threaded environment which requires this method called from the main UI thread or marshal / invoke the call onto another thread.
		/// </summary>
		private void SetTimersAndThreads(bool doStart)
		{
			SetTimers(doStart); // The calling method is in charge of invoking or not invoking based on being inside main thread or not.
			SetThreads(doStart); // The calling method is in charge of invoking or not invoking based on being inside main thread or not.
		}

		/// <summary>
		/// Starts or stops the local timers owned by FormOpenDental.
		/// Only starts signal timer if interval preference is set to non-zero value.
		/// The Windows Forms timer is designed for use in a single-threaded environment which requires this method called from the main UI thread  or marshal / invoke the call onto another thread.
		/// </summary>
		private void SetTimers(bool doStart)
		{
			if (doStart)
			{
				if (PrefC.GetInt(PreferenceName.ProcessSigsIntervalInSecs) == 0)
				{
					_hasSignalProcessingPaused = true;
				}
				else
				{
					timerSignals.Interval = PrefC.GetInt(PreferenceName.ProcessSigsIntervalInSecs) * 1000;
					timerSignals.Start();
				}
				timerTimeIndic.Start();
			}
			else
			{
				timerSignals.Stop();
				timerTimeIndic.Stop();
				_hasSignalProcessingPaused = true;
			}
		}

		/// <summary>
		/// Either starts all possible threads owned by FormOpenDental or stops a select few threads which are safe to stop.
		/// Some threads are not designed to be stopped once they've started.  E.g. heartbeat, data connection lost, etc.
		/// </summary>
		private void SetThreads(bool doStart)
		{
			if (doStart)
			{
				BeginClaimReportThread();
				BeginCanadianItransCarrierThread();
				BeginEServiceMonitorThread();
				BeginLogOffThread();
				BeginODServiceMonitorThread();
				BeginWebSyncThread();
				BeginComputerHeartbeatThread();
				BeginPodiumThread();
				BeginEnableFeaturesThread();
				BeginEhrCodeListThread();
				BeginTimeSyncThread();
				CheckAlerts(doRunOnThread: true);
			}
			else
			{
				Enum.GetValues(typeof(FormODThreadNames)).Cast<FormODThreadNames>().ForEach(threadName =>
				{
					switch (threadName)
					{
						// Do not kill these.
						case FormODThreadNames.EhrCodeList:
						case FormODThreadNames.EnableAdditionalFeatures:
						case FormODThreadNames.ODServiceStarter:
						case FormODThreadNames.ComputerHeartbeat:
						case FormODThreadNames.DataConnectionLost:
							break;

						// Kill these.
						case FormODThreadNames.CanadianItransCarrier:
						case FormODThreadNames.ClaimReport:
						case FormODThreadNames.EServiceMonitoring:
						case FormODThreadNames.LogOff:
						case FormODThreadNames.ODServiceMonitor:
						case FormODThreadNames.Podium:
						case FormODThreadNames.UpdateFormText:
						case FormODThreadNames.WebSync:
						case FormODThreadNames.TimeSync:
						case FormODThreadNames.CheckAlerts:
						default:
							ODThread.QuitAsyncThreadsByGroupName(threadName.GetDescription());
							break;
					}
				});
			}
		}

		///<summary>Checks to see if there is a thread running with the passed in group name. Will return true if there is or if a thread that is only set to run once has already ran. 
		///Will return false if no thread is running.</summary>
		private bool IsThreadAlreadyRunning(FormODThreadNames threadName)
		{
			if (_listOdThreadsRunOnce.Any(x => x.GroupName == threadName.GetDescription()))
			{
				return true;
			}
			List<ODThread> listThreads = ODThread.GetThreadsByGroupName(threadName.GetDescription()).ToList();
			return !listThreads.IsNullOrEmpty();
		}

		#region CanadianItransCarrierThread

		private void BeginCanadianItransCarrierThread()
		{
			if (IsThreadAlreadyRunning(FormODThreadNames.CanadianItransCarrier))
			{
				return;
			}
			if (!CultureInfo.CurrentCulture.Name.EndsWith("CA"))
			{//Canada
				return;
			}
			ODThread odThread = new ODThread((int)TimeSpan.FromHours(1).TotalMilliseconds, (o) =>
			{
				ItransNCpl.TryCarrierUpdate();
			});
			odThread.AddExceptionHandler((e) => { });
			odThread.GroupName = FormODThreadNames.CanadianItransCarrier.GetDescription();
			odThread.Name = FormODThreadNames.CanadianItransCarrier.GetDescription();
			odThread.Start();
		}

		#endregion
		#region CheckAlertsThread

		///<summary>May begin a thread that checks for alerts and update the main alerts tool bar menu.
		///Pass false to doRunOnThread if you want to run alerts on the main thread.</summary>
		private void CheckAlerts(bool doRunOnThread = false)
		{
			if (doRunOnThread && IsThreadAlreadyRunning(FormODThreadNames.CheckAlerts))
			{
				return;
			}
			ODThread.WorkerDelegate getAlerts = new ODThread.WorkerDelegate((o) =>
			{
				DateTime dtInactive = Security.DateTimeLastActivity.AddMinutes(PrefC.GetInt(PreferenceName.AlertInactiveMinutes));
				if (PrefC.GetInt(PreferenceName.AlertInactiveMinutes) != 0 && DateTime.Now > dtInactive)
				{
					return;//user has been inactive for a while, so stop checking alerts.
				}
				long clinicNumCur = Clinics.Active.Id;
				long userNumCur = Security.CurrentUser.Id;
				// TODO: Logger.LogToPath("",LogPath.Signals,LogPhase.Start);
				List<List<AlertItem>> listUniqueAlerts = AlertItems.GetUniqueAlerts(userNumCur, clinicNumCur);
				//We will set the alert's tag to all the items in its list so that all can be marked read/deleted later.
				//listUniqueAlerts.ForEach(x => x.First().TagOD = x.Select(y => y.Id).ToList());
				List<AlertItem> listAlertItems = listUniqueAlerts.Select(x => x.First())
					.Where(x => x.Type != AlertType.ClinicsChangedInternal).ToList();//These alerts are not supposed to be displayed to the end user.
																					 //Update listUserAlertTypes to only those with active AlertItems.
				List<string> listUserAlertLinks = listAlertItems.Select(x => x.Type).ToList();
				var listAlertItemReads = AlertReads.RefreshForAlertNums(userNumCur, listAlertItems.Select(x => x.Id).ToList());
				this.InvokeIfRequired(() =>
				{
					//Assigning this inside Invoke so that we don't have to lock _listAlertItems and _listAlertReads.
					_listAlertItems = listAlertItems;
					_listAlertReads = listAlertItemReads.ToList();
					AddAlertsToMenu();
				});
			});
			if (!doRunOnThread)
			{
				getAlerts(null);
				return;
			}
			int checkAlertsIntervalMS = (int)TimeSpan.FromSeconds(PrefC.GetInt(PreferenceName.AlertCheckFrequencySeconds)).TotalMilliseconds;
			if (checkAlertsIntervalMS == 0)
			{
				//Office has disabled alert checking. We won't periodically check alerts, but we will do it when the user does something alert related.
				return;
			}
			ODThread odThread = new ODThread(checkAlertsIntervalMS, getAlerts);
			odThread.AddExceptionHandler((ex) => { });
			odThread.GroupName = FormODThreadNames.CheckAlerts.GetDescription();
			odThread.Name = FormODThreadNames.CheckAlerts.GetDescription();
			odThread.Start(true);
		}

		#endregion
		#region ClaimReportThread

		///<summary>If the local computer is the computer where claim reports are retrieved then this thread runs in the background and will retrieve
		///and import reports for the default clearinghouse or for clearinghouses where both the Payors field is not empty plus the Eformat matches the
		///region the user is in.  If an error is returned from the importation, this thread will silently fail.</summary>
		private void BeginClaimReportThread()
		{
			if (IsThreadAlreadyRunning(FormODThreadNames.ClaimReport))
			{
				return;
			}
			if (Preferences.GetBool(PreferenceName.ClaimReportReceivedByService))
			{
				return;
			}
			int claimReportRetrieveIntervalMS = (int)TimeSpan.FromMinutes(PrefC.GetInt(PreferenceName.ClaimReportReceiveInterval)).TotalMilliseconds;
			ODThread odThread = new ODThread(claimReportRetrieveIntervalMS, (o) =>
			{
				string claimReportComputer = Preferences.GetString(PreferenceName.ClaimReportComputerName);
				if (claimReportComputer == "" || claimReportComputer != Dns.GetHostName())
				{
					return;
				}
				Clearinghouses.RetrieveReportsAutomatic(false);//only run for the selected clinic, if clinics are enabled
			});
			odThread.AddExceptionHandler(ex => { });
			odThread.GroupName = FormODThreadNames.ClaimReport.GetDescription();
			odThread.Name = FormODThreadNames.ClaimReport.GetDescription();
			odThread.Start();
		}

		#endregion
		#region ComputerHeartbeatThread

		private void BeginComputerHeartbeatThread()
		{
			if (IsThreadAlreadyRunning(FormODThreadNames.ComputerHeartbeat))
			{
				return;
			}
			ODThread threadCompHeartbeat = new ODThread(180000, o =>
			{//Every three minutes
				ODException.SwallowAnyException(() =>
				{
					Computers.UpdateHeartBeat(Environment.MachineName);
				});
			});
			threadCompHeartbeat.AddExceptionHandler((e) => { });
			threadCompHeartbeat.GroupName = FormODThreadNames.ComputerHeartbeat.GetDescription();
			threadCompHeartbeat.Name = FormODThreadNames.ComputerHeartbeat.GetDescription();
			threadCompHeartbeat.Start();
		}

		#endregion
		#region DataConnectionLostThread

		private void BeginDataConnectionLostThread(DataConnectionEventArgs e)
		{
			if (_odThreadDataConnectionLost != null)
			{
				return;
			}
			_odThreadDataConnectionLost = new ODThread((o) =>
			{
				//Stop all appropriate threads and open the Connection Lost window.
				//It is not safe to stop timers at this point because we would need to invoke back over to the main thread which is waiting in a Join().
				SetThreads(false);//Only stop threads because the main thread is locked waiting for this thread to finish, which means ticks cannot fire.
				string errorMessage = (string)e.Tag;
				Func<bool> funcTestConnection = () =>
				{
					using (DataConnection dconn = new DataConnection())
					{
						try
						{
							dconn.SetDb(DataConnection.ConnectionString);
							//Tell everyone that the data connection has been found.
							DataConnectionEvent.Fire(new DataConnectionEventArgs(DataConnectionEventType.ConnectionRestored, true, e.ConnectionString));
						}
						catch
						{
							return false;//Data connection is still lost so do not close the Connection Lost window.
						}
					}
					return true;//Data connection has been found so close the Connection Lost window.
				};
				FormConnectionLost FormCL = new FormConnectionLost(funcTestConnection, EventCategory.DataConnection, errorMessage);
				if (FormCL.ShowDialog() == DialogResult.Cancel)
				{
					//This is problematic because it causes DirectX to cause a UE but there doesn't seem to be a better way to close without using the database.
					ExitCode = 106;//Connection to specified database has failed
					Environment.Exit(ExitCode);
					return;
				}
			});
			//Add exception handling just in case MySQL is unreachable at any point in the lifetime of this session.
			_odThreadDataConnectionLost.AddExceptionHandler((ex) => { });
			_odThreadDataConnectionLost.AddExitHandler((ex) =>
			{
				_odThreadDataConnectionLost = null;
				//Restart our threads no matter what happened.  If we're killing the program this won't matter anyway.
				SetThreads(true);//Start the threads because they were the only ones stopped, the timers were locked up via a Join() on the main thread.
			});
			_odThreadDataConnectionLost.GroupName = FormODThreadNames.DataConnectionLost.GetDescription();
			_odThreadDataConnectionLost.Name = FormODThreadNames.DataConnectionLost.GetDescription();
			_odThreadDataConnectionLost.Start();
		}

		#endregion
		#region EhrCodeListThread

		///<summary>This begins a thread that loads the EHR.dll in the background.</summary>
		private void BeginEhrCodeListThread()
		{
			if (IsThreadAlreadyRunning(FormODThreadNames.EhrCodeList))
			{
				return;
			}
			//For EHR users we want to load up the EHR code list from the obfuscated dll in a background thread because it takes roughly 11 seconds to load up.
			if (!Preferences.GetBool(PreferenceName.ShowFeatureEhr))
			{
				return;
			}
			ODThread odThread = new ODThread(o =>
			{
				//In regards to throwing, this should never happen.  It would most likely be due to a corrupt dll issue but I don't want to stop the 
				//start up sequence. Users could theoretically use Open Dental for an entire day and never hit the code that utilizes the EhrCodes class.
				//Therefore, we do not want to cause any issues and the worst case scenario is the users has to put up with the 11 second delay (old behavior).
				ODException.SwallowAnyException(() =>
				{
					EhrCodes.UpdateList();
				});
			});
			odThread.AddExceptionHandler((e) => { });
			odThread.GroupName = FormODThreadNames.EhrCodeList.GetDescription();
			odThread.Name = FormODThreadNames.EhrCodeList.GetDescription();
			odThread.Start();
			_listOdThreadsRunOnce.Add(odThread);
		}

		#endregion
		#region EnableFeaturesThread

		private void BeginEnableFeaturesThread()
		{
			if (IsThreadAlreadyRunning(FormODThreadNames.EnableAdditionalFeatures))
			{
				return;
			}
			ODThread odThread = new ODThread((o) => { EnableFeaturesWorker(); });
			odThread.AddExceptionHandler((ex) => { });
			odThread.GroupName = FormODThreadNames.EnableAdditionalFeatures.GetDescription();
			odThread.Name = FormODThreadNames.EnableAdditionalFeatures.GetDescription();
			odThread.Start(true);
			_listOdThreadsRunOnce.Add(odThread);
		}

		private void EnableFeaturesWorker()
		{
			var featureDate = Preferences.GetDateTimeOrNull(PreferenceName.ProgramAdditionalFeatures);
			if (!featureDate.HasValue || featureDate > DateTime.UtcNow)
            {
				return;
            }




			DateTime dateOriginal = MiscData.GetNowDateTime().AddMinutes(-30);

			Preferences.Set(PreferenceName.ProgramAdditionalFeatures, dateOriginal.AddDays(1));

			Signalods.SetInvalid(InvalidType.Prefs);
			string response = WebServiceMainHQProxy.GetWebServiceMainHQInstance()
				.EnableAdditionalFeatures(PayloadHelper.CreatePayload("", eServiceCode.Undefined));
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(response);
			XmlNode node;
			bool refreshNeeded = false;
			//Update all "Disable Advertising HQ" program links based on what HQ provided.
			refreshNeeded |= SetAdvertising(ProgramName.CentralDataStorage, doc);
			refreshNeeded |= SetAdvertising(ProgramName.DentalTekSmartOfficePhone, doc);
			refreshNeeded |= SetAdvertising(ProgramName.Podium, doc);
			refreshNeeded |= SetAdvertising(ProgramName.RapidCall, doc);
			refreshNeeded |= SetAdvertising(ProgramName.Transworld, doc);
			refreshNeeded |= SetAdvertising(ProgramName.DentalIntel, doc);
			refreshNeeded |= SetAdvertising(ProgramName.PracticeByNumbers, doc);
			refreshNeeded |= SetAdvertising(ProgramName.DXCPatientCreditScore, doc);
			refreshNeeded |= SetAdvertising(ProgramName.Oryx, doc);
			if (refreshNeeded)
			{
				Signalods.SetInvalid(InvalidType.Programs);
			}
			node = doc.SelectSingleNode("//NextIntervalDays");
			if (node != null)
			{
				long days = 7;//default value;
				long.TryParse(node.InnerText, out days);

				Preferences.Set(PreferenceName.ProgramAdditionalFeatures, dateOriginal.AddDays(days));
			}
		}

		#endregion
		#region EServiceMonitorThread

		///<summary>Starts the eService monitoring thread that will run once a minute.  Only runs if the user currently logged in has the eServices permission.</summary>
		private void BeginEServiceMonitorThread()
		{
			if (IsThreadAlreadyRunning(FormODThreadNames.EServiceMonitoring))
			{
				return;
			}
			//If the user currently logged in has permission to view eService settings, turn on the listener monitor.
			if (Security.CurrentUser == null || !Security.IsAuthorized(Permissions.EServicesSetup, true))
			{
				return;//Do not start the listener service monitor for users without permission.
			}
			//Process any Error signals that happened due to an update:
			EServiceSignals.ProcessErrorSignalsAroundTime(PrefC.GetDate(PreferenceName.ProgramVersionLastUpdated));
			//Create a separate thread that will run every 60 seconds to monitor eService signals.
			ODThread odThread = new ODThread(60000, EServiceMonitorWorker);
			//Currently we don't want to do anything if the eService signal processing fails.  Simply try again in a minute.  
			//Most likely cause for exceptions will be database IO when computers are just sitting around not doing anything.
			//Implementing this delegate allows us to NOT litter ProcessEServiceSignals() with try catches.  
			odThread.AddExceptionHandler((e) => { });
			odThread.GroupName = FormODThreadNames.EServiceMonitoring.GetDescription();
			odThread.Name = FormODThreadNames.EServiceMonitoring.GetDescription();
			odThread.Start();
		}

		///<summary>Worker method for eServiceMonitorThread.  Call BeginEServiceMonitorThread() to start monitoring eService signals instead of calling 
		///this method directly. This thread's only job is to check to see if the eConnector's current status is critical and if it is critical, 
		///create a High severity alert.</summary>
		private void EServiceMonitorWorker(ODThread odThread)
		{
			//The listener service will have a local heartbeat every 5 minutes so it's overkill to check every time timerSignals_Tick fires.
			//Only check the Listener Service status once a minute.
			//The downside to doing this is that the menu item will stay red up to one minute when a user wants to stop monitoring the service.
			eServiceSignalSeverity listenerStatus = EServiceSignals.GetListenerServiceStatus();
			if (listenerStatus == eServiceSignalSeverity.None)
			{
				//This office has never had a valid listener service running and does not have more than 5 patients set up to use the listener service.
				//Quit the thread so that this computer does not waste its time sending queries to the server every minute.
				odThread.QuitAsync();
				return;
			}
			if (listenerStatus != eServiceSignalSeverity.Critical)
			{ //Not a critical event so no need to continue.
				return;
			}
			if (AlertItems.RefreshForType(AlertType.EConnectorDown).Count() > 0)
			{ //Alert already exists to no need to continue.
				return;
			}
			//Create an alert.
			AlertItems.Insert(new AlertItem
			{
				//Do not allow delete. The only way for this alert to be deleted is for the eConnector to insert a heartbeat, which will in-turn delete this alert.
				Actions = AlertAction.MarkAsRead | AlertAction.OpenForm,
				Description = "eConnector needs to be restarted",
				Severity = AlertSeverityType.High,
				Type = AlertType.EConnectorDown,
				//Show for all clinics.
				ClinicId = -1,
				FormToOpen = FormType.FormEServicesEConnector,
			});
			//We just inserted an alert so update the alert menu.
			CheckAlerts();
		}

		#endregion
		#region LogOffThread

		///<summary>Begins the thread that checks for a forced log off.</summary>
		private void BeginLogOffThread()
		{
			if (IsThreadAlreadyRunning(FormODThreadNames.LogOff))
			{
				return;
			}
			ODThread odThread = new ODThread((int)TimeSpan.FromSeconds(15).TotalMilliseconds, (o) => { LogOffWorker(); });
			//Do not add an exception handler for the log off thread.  If it fails for any unhandled reason then the program should crash.
			odThread.GroupName = FormODThreadNames.LogOff.GetDescription();
			odThread.Name = FormODThreadNames.LogOff.GetDescription();
			odThread.Start();
		}

		///<summary>Thread set to run every 15 seconds. This interval must be longer than the interval of the timer in FormLogoffWarning (10s), 
		///or it will go into a loop.</summary>
		private void LogOffWorker()
		{
			int logOffTimerMins = PrefC.LogOffTimer;
			if (logOffTimerMins == 0)
			{
				return;
			}
			if (this.InvokeRequired)
			{
				//Invoke here as the following uses Application wide variables and accesses UI elements when logging off.
				this.Invoke(() => LogOffWorker());
				return;
			}
			for (int f = Application.OpenForms.Count - 1; f >= 0; f--)
			{//This checks if any forms are open that make us not want to automatically log off. Currently only FormTerminal is checked for.
				Form openForm;
				try
				{
					openForm = Application.OpenForms[f];
				}
				catch
				{
					continue;
				}
				if (openForm.Name == "FormTerminal")
				{
					return;
				}
				//If anything is in progress we should halt the autologoff. After the window finishes, this will get hit after a maximum of 15 seconds and perform the auto-logoff.
				if (openForm.Name == "FormProgress")
				{
					return;
				}
			}
			//Warning.  When debugging this, the ActiveForm will be impossible to determine by setting breakpoints.
			//string activeFormText=Form.ActiveForm.Text;
			//If a breakpoint is set below here, ActiveForm will erroneously show as null.
			Form currentForm = Form.ActiveForm;
			if (currentForm == null)
			{//some other program has focus
				FormRecentlyOpenForLogoff = null;
				//Do not alter IsFormLogOnLastActive because it could still be active in background.
			}
			else if (currentForm == this)
			{//main form active
				FormRecentlyOpenForLogoff = null;
				//User must have logged back in so IsFormLogOnLastActive should be false.
				IsFormLogOnLastActive = false;
			}
			else
			{//Some Open Dental dialog is active.
				if (currentForm == FormRecentlyOpenForLogoff)
				{
					//The same form is active as last time, so don't add events again.
					//The active form will now be constantly resetting the dateTimeLastActivity.
				}
				else
				{//this is the first time this form has been encountered, so attach events and don't do anything else
					FormRecentlyOpenForLogoff = currentForm;
					Security.DateTimeLastActivity = DateTime.Now;
					//Flag FormLogOn as the active form so that OD doesn't continue trying to log the user off when using the web service.
					if (currentForm.GetType() == typeof(FormLogOn))
					{
						IsFormLogOnLastActive = true;
					}
					else
					{
						IsFormLogOnLastActive = false;
					}
					return;
				}
			}
			DateTime dtDeadline = Security.DateTimeLastActivity + TimeSpan.FromMinutes(logOffTimerMins);
			//Debug.WriteLine("Now:"+DateTime.Now.ToLongTimeString()+", Deadline:"+dtDeadline.ToLongTimeString());
			if (DateTime.Now < dtDeadline)
			{
				return;
			}
			if (Security.CurrentUser == null)
			{//nobody logged on
				return;
			}
			//The above check works unless using web service.  With web service, CurUser is not set to null when FormLogOn is shown.
			if (IsFormLogOnLastActive)
			{//Don't try to log off a user that is already logged off.
				return;
			}
			FormLogoffWarning formW = new FormLogoffWarning();
			formW.ShowDialog();
			if (formW.DialogResult != DialogResult.OK)
			{
				Security.DateTimeLastActivity = DateTime.Now;
				return;//user hit cancel, so don't log off
			}
			//User could be working outside of OD and the Log On window will never become "active" so we set it here for a fail safe.
			IsFormLogOnLastActive = true;
			//WE are inside of an Invoke call here and we are on the main UI thread.
			//Launch the LogOffNow() inside a new thread so that we can leave the UI thread to allow other invoke calls to execute inside LogOffNow().
			//Invoke calls which are nested are delayed until the parent invoke finishes.
			//We need form Close() to cause ShowDialog() to return immediately instead of after leaving LogOffNow().
			ODThread thread = new ODThread((o) =>
			{
				ODException.SwallowAnyException(() =>
				{
					LogOffNow(true);
				});
			});
			thread.Start();
		}

		#endregion
		#region ODServiceMonitorThread

		///<summary>Begins a thread that monitor's the Open Dental Service heartbeat and alerts the user if the service is not running.</summary>
		private void BeginODServiceMonitorThread()
		{
			if (IsThreadAlreadyRunning(FormODThreadNames.ODServiceMonitor))
			{
				return;
			}
			ODThread threadOpenDentalServiceCheck = new ODThread((int)TimeSpan.FromMinutes(10).TotalMilliseconds,
				(o) => { AlertItems.CheckODServiceHeartbeat(); });
			threadOpenDentalServiceCheck.AddExceptionHandler(ex => { });
			threadOpenDentalServiceCheck.GroupName = FormODThreadNames.ODServiceMonitor.GetDescription();
			threadOpenDentalServiceCheck.Name = FormODThreadNames.ODServiceMonitor.GetDescription();
			threadOpenDentalServiceCheck.Start();
		}

		#endregion
		#region ODServiceStarterThread

		///<summary>Spawns a thread that attempts to start the Patient Dashboard.</summary>
		private void BeginODDashboardStarterThread()
		{
			if (IsThreadAlreadyRunning(FormODThreadNames.Dashboard))
			{
				return;
			}
			ODThread odThread = new ODThread((o) =>
			{
				RefreshMenuDashboards();
				if (Security.CurrentUser != null)
				{
					InitDashboards(Security.CurrentUser.Id);
				}
			});
			//If the thread that attempts to start Open Dental dashboard fails for any reason, silently fail.
			odThread.AddExceptionHandler(ex =>
			{
				//if (Security.CurrentUser != null && Security.CurrentUser.Id != 0)
				//{


				//	//Defensive to ensure all Patient Dashboard userprefs are not deleted.
				//	UserOdPrefs.DeleteForValueString(Security.CurrentUser.Id, UserOdFkeyType.Dashboard, string.Empty);//All Dashboard userodprefs for this user.
				//}
			});
			odThread.GroupName = FormODThreadNames.Dashboard.GetDescription();
			odThread.Name = FormODThreadNames.Dashboard.GetDescription();
			odThread.Start(true);
		}

		#endregion
		#region PlaySoundsThread

		///<summary>Begins a thread that will play sounds based on the given signals.</summary>
		private void BeginPlaySoundsThread(List<SigMessage> listSigMessages)
		{
			//Do not check if the thread is already running. If there are more sounds to play, play them. 
			ODThread odThread = new ODThread((o) => PlaySoundsWorker(listSigMessages));
			odThread.AddExceptionHandler((e) => { });
			odThread.GroupName = FormODThreadNames.PlaySounds.GetDescription();
			odThread.Name = FormODThreadNames.PlaySounds.GetDescription();
			odThread.Start();
		}

		private void PlaySoundsWorker(List<SigMessage> listSigMessages)
		{
			byte[] rawData;
			MemoryStream stream = null;
			SoundPlayer simpleSound = null;
			try
			{
				//loop through each signal
				foreach (SigMessage sigMessage in listSigMessages)
				{
					if (sigMessage.AckDateTime.Year > 1880)
					{
						continue;//don't play any sounds for acks.
					}
					//play all the sounds.
					List<SigElementDef> listSigElementDefs = SigElementDefs.GetDefsForSigMessage(sigMessage);
					foreach (SigElementDef sigElement in listSigElementDefs)
					{
						if (sigElement.Sound == "")
						{
							continue;
						}
						ODException.SwallowAnyException(() =>
						{
							rawData = Convert.FromBase64String(sigElement.Sound);
							stream = new MemoryStream(rawData);
							simpleSound = new SoundPlayer(stream);
							simpleSound.PlaySync();//sound will finish playing before thread continues
						});
					}
					Thread.Sleep(1000);//pause 1 second between signals.
				}
			}
			finally
			{
				if (stream != null)
				{
					stream.Dispose();
				}
				if (simpleSound != null)
				{
					simpleSound.Dispose();
				}
			}
		}

		#endregion
		#region PodiumThread

		///<summary>If the local computer is the computer where Podium invitations are sent, then this thread runs in the background and checks for 
		///appointments that started 10-40 minutes ago (depending on in the patient is a new patient) at 10 minute intervals.  No preferences.
		///In the future, some sort of identification should be made to tell if this thread is running on any computer.</summary>
		private void BeginPodiumThread()
		{
			if (IsThreadAlreadyRunning(FormODThreadNames.Podium))
			{
				return;
			}
			ODThread odThread = new ODThread(Podium.PodiumThreadIntervalMS, ((ODThread o) => { Podium.ThreadPodiumSendInvitations(false); }));
			odThread.AddExceptionHandler((ex) =>
			{
				// TODO: Logger.WriteException(ex,Podium.LOG_DIRECTORY_PODIUM);
			});
			odThread.GroupName = FormODThreadNames.Podium.GetDescription();
			odThread.Name = FormODThreadNames.Podium.GetDescription();
			odThread.Start();
		}

		#endregion

		#region ShutdownThread

		///<summary>Begins a thread that shutsdown Open Dental.</summary>
		private void BeginShutdownThread()
		{
			//If this thread is already running, do not start another one.
			if (IsThreadAlreadyRunning(FormODThreadNames.Shutdown))
			{
				return;
			}
			ODThread odThread = new ODThread((o) =>
			{
				Thread.Sleep(15000);//15 seconds
				CloseOpenForms(true);
				this.Invoke(Application.Exit);
			});
			//Do not add an exception handler for the shutdown thread.  If it fails for any unhandled reason then the program should crash.
			odThread.GroupName = FormODThreadNames.Shutdown.GetDescription();
			odThread.Name = FormODThreadNames.Shutdown.GetDescription();
			odThread.Start();
		}

		#endregion
		#region TasksThread

		///<summary>Begins a thread that handles all tasks from signals. Only call within signal processing.</summary>
		private void BeginTasksThread(List<Signalod> listSignalTasks, List<long> listEditedTaskNums)
		{
			//Do not call IsThreadAlreadyRunning(FormODThreadNames.Tasks) here.
			//Allow this thread to re-enter in the rare case that a subsequent run is required while the first run is still busy.
			//We don't want to miss the specific input that belongs to the second run.
			//SamO and Luke made this decision as it retains previous behavior.			
			ODThread threadTasks = new ODThread(new ODThread.WorkerDelegate((o) =>
			{
				List<TaskNote> listRefreshedTaskNotes = null;
				//List<UserOdPref> listBlockedTaskLists = null;
				//JM: Bug fix, but we do not know what would cause Security.CurUser to be null. Worst case task wont show till next signal tick.
				long userNumCur = Security.CurrentUser?.Id ?? 0;
				List<OpenDentBusiness.Task> listRefreshedTasks = Tasks.GetNewTasksThisUser(userNumCur, listEditedTaskNums).ToList();

				// TODO: Fix me...

				//if (listRefreshedTasks.Count > 0)
				//{
				//	listRefreshedTaskNotes = TaskNotes.GetForTasks(listRefreshedTasks.Select(x => x.TaskNum).ToList());
				//	listBlockedTaskLists = UserOdPrefs.GetByUserAndFkeyType(userNumCur, UserOdFkeyType.TaskListBlock);
				//}

				//this.Invoke(() 
				//	=> HandleRefreshedTasks(listSignalTasks, listEditedTaskNums, listRefreshedTasks, listRefreshedTaskNotes, listBlockedTaskLists));
			}));
			threadTasks.AddExceptionHandler((e) => { });
			threadTasks.GroupName = FormODThreadNames.Tasks.GetDescription();
			threadTasks.Name = FormODThreadNames.Tasks.GetDescription();
			threadTasks.Start();
		}

		#endregion
		#region TimeSyncThread

		///<summary>This begins the time sync thread. If OpenDental is running on the same machine as the mysql server, then a thread is runs in the 
		///background to update the local machine's time using NTPv4 from the NIST time server set in the NistTimeServerUrl pref.</summary>
		private void BeginTimeSyncThread()
		{
			if (IsThreadAlreadyRunning(FormODThreadNames.TimeSync))
			{
				return;
			}
			if (!(ODEnvironment.IsRunningOnDbServer(MiscData.GetMySqlServer()) && Preferences.GetBool(PreferenceName.ShowFeatureEhr)))
			{
				return;
			}
			//OpenDental has EHR enabled and is running on the same machine as the mysql server it is connected to.
			ODThread odThread = new ODThread((int)TimeSpan.FromHours(4).TotalMilliseconds, TimeSyncWorker);
			odThread.AddExceptionHandler((e) => { });
			odThread.GroupName = FormODThreadNames.TimeSync.GetDescription();
			odThread.Name = FormODThreadNames.TimeSync.GetDescription();
			odThread.Start();
		}

		///<summary>Worker thread for the time sync thread. Every 6 hours gets the time from an NTPv4 server and sets the local time to that.</summary>
		private void TimeSyncWorker(ODThread o)
		{
			NTPv4 ntp = new NTPv4();
			double nistOffset = double.MaxValue;
			ODException.SwallowAnyException(() =>
			{//Invalid NIST Server URL if fails
				nistOffset = ntp.getTime(Preferences.GetString(PreferenceName.NistTimeServerUrl));
			});
			if (nistOffset != double.MaxValue)
			{
				//Did not timeout, or have invalid NIST server URL
				//Sets local machine time. May error if unable to set machine.
				ODException.SwallowAnyException(() =>
				{
					WindowsTime.SetTime(DateTime.Now.AddMilliseconds(nistOffset));
				});
			}
		}

		#endregion
		#region WebSyncThread

		///<summary>Begins the thread that checks for mobile sync. This will sync parts of a users database to HQ if certain preferences are set.</summary>
		private void BeginWebSyncThread()
		{
			//if (IsThreadAlreadyRunning(FormODThreadNames.WebSync))
			//{
			//	return;
			//}
			//string interval = Prefs.GetString(PrefName.MobileSyncIntervalMinutes);
			//if (interval == "" || interval == "0")
			//{//not a paid customer or chooses not to synch
			//	return;
			//}
			//if (System.Environment.MachineName.ToUpper() != Prefs.GetString(PrefName.MobileSyncWorkstationName).ToUpper())
			//{
			//	//Since GetStringSilent returns "" before OD is connected to db, this gracefully loops out
			//	return;
			//}
			//if (PrefC.GetDate(PrefName.MobileExcludeApptsBeforeDate).Year < 1880)
			//{
			//	//full synch never run
			//	return;
			//}
			//ODThread odThread = new ODThread((int)TimeSpan.FromSeconds(30).TotalMilliseconds, (o) =>
			//{
			//	ODException.SwallowAnyException(() =>
			//	{
			//		FormEServicesMobileSynch.SynchFromMain(false);
			//	});
			//});
			//odThread.AddExceptionHandler((e) => { });
			//odThread.GroupName = FormODThreadNames.WebSync.GetDescription();
			//odThread.Name = FormODThreadNames.WebSync.GetDescription();
			//odThread.Start();
		}

		#endregion


		/// <summary>
		/// A list of the names and group names for all threads spawned from FormOpenDental.
		/// </summary>
		private enum FormODThreadNames
		{
			[Obsolete]
			CacheFillForFees,
			CanadianItransCarrier,
			CheckAlerts,
			ClaimReport,
			ComputerHeartbeat,
			DataConnectionLost,
			EhrCodeList,
			EnableAdditionalFeatures,
			EServiceMonitoring,
			LogOff,
			ODServiceMonitor,
			ODServiceStarter,
			PhoneConference,
			PlaySounds,
			Podium,
			Shutdown,
			Tasks,
			TimeSync,
			UpdateFormText,
			WebSync,
			Dashboard
		}
	}
}
