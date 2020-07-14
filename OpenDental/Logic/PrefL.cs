using CodeBase;
using DataConnectionBase;
using Ionic.Zip;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace OpenDental
{
    public class PrefL
	{
		/// <summary>
		/// Called from <see cref="FormOpenDental.PrefsStartup"/>. 
		/// Compares the installed version to the version on the server being connected to.
		/// </summary>
		/// <param name="currentForm">The form where the method is being called from.</param>
		/// <param name="isSilent">Whether this is a silent update. A silent update will have no UI elements appear.</param>
		/// <param name="model">May be null. The model for the choose database window. Stores all information entered within the window.</param>
		public static bool CheckProgramVersion(bool isSilent)
		{
			if (isSilent)
			{
				FormOpenDental.ExitCode = 399; // Classic View is not supported with Silent Update

				return false;
			}

			return CheckProgramVersionClassic();
		}


		///<summary>If AtoZ.manifest was wrong, or if user is not using AtoZ, then just download again.  Will use dir selected by user.  If an appropriate download is not available, it will fail and inform user.</summary>
		private static void DownloadAndRunSetup(Version storedVersion, Version currentVersion)
		{
			string patchName = "Setup.exe";
			string updateUri = PrefC.GetString(PrefName.UpdateWebsitePath);
			string updateCode = PrefC.GetString(PrefName.UpdateCode);
			string updateInfoMajor = "";
			string updateInfoMinor = "";
			if (!ShouldDownloadUpdate(updateUri, updateCode, out updateInfoMajor, out updateInfoMinor))
			{
				return;
			}

			var result = ODMessageBox.Show(
				"Setup file will now be downloaded.\r\nWorkstation version will be updated from " + currentVersion.ToString(3) + " to " + storedVersion.ToString(3), "Imedisoft",
				MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

			if (result != DialogResult.OK)
			{
				return;
			}

			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.SelectedPath = OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath();
			dlg.Description = "Setup.exe will be downloaded to the folder you select below";
			if (dlg.ShowDialog() != DialogResult.OK)
			{
				return;//app will exit
			}
			string tempFile = ODFileUtils.CombinePaths(dlg.SelectedPath, patchName);
			//ODFileUtils.CombinePaths(GetTempFolderPath(),patchName);
			DownloadInstallPatchFromURI(updateUri + updateCode + "/" + patchName,//Source URI
				tempFile, true, false, null);//Local destination file.
			if (File.Exists(tempFile))
			{//If user canceld in DownloadInstallPatchFromURI file will not exist.
				File.Delete(tempFile);//Cleanup install file.
			}
		}

		///<summary>Returns true if the download at the specified remoteUri with the given registration code should be downloaded and installed as an update, and false is returned otherwise. Also, information about the decision making process is stored in the updateInfoMajor and updateInfoMinor strings, but only holds significance to a human user.</summary>
		public static bool ShouldDownloadUpdate(string remoteUri, string updateCode, out string updateInfoMajor, out string updateInfoMinor)
		{
			updateInfoMajor = "";
			updateInfoMinor = "";
			bool shouldDownload = false;
			string fileName = "Manifest.txt";
			WebClient myWebClient = new WebClient();
			string myStringWebResource = remoteUri + updateCode + "/" + fileName;
			Version versionNewBuild = null;
			string strNewVersion = "";
			string newBuild = "";
			bool buildIsAlpha = false;
			bool buildIsBeta = false;
			bool versionIsAlpha = false;
			bool versionIsBeta = false;
			try
			{
				using (StreamReader sr = new StreamReader(myWebClient.OpenRead(myStringWebResource)))
				{
					newBuild = sr.ReadLine();//must be be 3 or 4 components (revision is optional)
					strNewVersion = sr.ReadLine();//returns null if no second line
				}
				if (newBuild.EndsWith("a"))
				{
					buildIsAlpha = true;
					newBuild = newBuild.Replace("a", "");
				}
				if (newBuild.EndsWith("b"))
				{
					buildIsBeta = true;
					newBuild = newBuild.Replace("b", "");
				}
				versionNewBuild = new Version(newBuild);
				if (versionNewBuild.Revision == -1)
				{
					versionNewBuild = new Version(versionNewBuild.Major, versionNewBuild.Minor, versionNewBuild.Build, 0);
				}
				if (strNewVersion != null && strNewVersion.EndsWith("a"))
				{
					versionIsAlpha = true;
					strNewVersion = strNewVersion.Replace("a", "");
				}
				if (strNewVersion != null && strNewVersion.EndsWith("b"))
				{
					versionIsBeta = true;
					strNewVersion = strNewVersion.Replace("b", "");
				}
			}
			catch
			{
				updateInfoMajor += Lan.G("FormUpdate", "Registration number not valid, or internet connection failed.  ");
				return false;
			}
			if (versionNewBuild == new Version(Application.ProductVersion))
			{
				updateInfoMajor += Lan.G("FormUpdate", "You are using the most current build of this version.  ");
			}
			else
			{
				//this also allows users to install previous versions.
				updateInfoMajor += Lan.G("FormUpdate", "A new build of this version is available for download:  ")
					+ versionNewBuild.ToString();
				if (buildIsAlpha)
				{
					updateInfoMajor += Lan.G("FormUpdate", "(alpha)  ");
				}
				if (buildIsBeta)
				{
					updateInfoMajor += Lan.G("FormUpdate", "(beta)  ");
				}
				shouldDownload = true;
			}
			//Whether or not build is current, we want to inform user about the next minor version
			if (strNewVersion != null)
			{//we don't really care what it is.
				updateInfoMinor += Lan.G("FormUpdate", "A newer version is also available.  ");
				if (versionIsAlpha)
				{
					updateInfoMinor += Lan.G("FormUpdate", "It is alpha (experimental), so it has bugs and " +
						"you will need to update it frequently.  ");
				}
				if (versionIsBeta)
				{
					updateInfoMinor += Lan.G("FormUpdate", "It is beta (test), so it has some bugs and " +
						"you will need to update it frequently.  ");
				}
				updateInfoMinor += Lan.G("FormUpdate", "Contact us for a new Registration number if you wish to use it.  ");
			}
			return shouldDownload;
		}

		/// <summary>destinationPath includes filename (Setup.exe).  destinationPath2 will create a second copy at the specified path/filename, or it will be skipped if null or empty.</summary>
		public static void DownloadInstallPatchFromURI(string downloadUri, string destinationPath, bool runSetupAfterDownload, bool showShutdownWindow, string destinationPath2)
		{
			string[] dblist = PrefC.GetString(PrefName.UpdateMultipleDatabases).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
			bool isShutdownWindowNeeded = showShutdownWindow;
			while (isShutdownWindowNeeded)
			{
				//Even if updating multiple databases, extra shutdown signals are not needed.
				FormShutdown FormSD = new FormShutdown();
				FormSD.IsUpdate = true;
				FormSD.ShowDialog();
				if (FormSD.DialogResult == DialogResult.OK)
				{
					//turn off signal reception for 5 seconds so this workstation will not shut down.
					Signalods.SignalLastRefreshed = MiscData.GetNowDateTime().AddSeconds(5);
					Signalod sig = new Signalod();
					sig.IType = InvalidType.ShutDownNow;
					Signalods.Insert(sig);
					Computers.ClearAllHeartBeats(Environment.MachineName);//always assume success
					isShutdownWindowNeeded = false;
					//SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Shutdown all workstations.");//can't do this because sometimes no user.
				}
				else if (FormSD.DialogResult == DialogResult.Cancel)
				{//Cancel
					if (MsgBox.Show("FormUpdate", MsgBoxButtons.YesNo, "Are you sure you want to cancel the update?"))
					{
						return;
					}
					continue;
				}
				//no other workstation will be able to start up until this value is reset.
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, Environment.MachineName);
			}
			MiscData.LockWorkstationsForDbs(dblist);//lock workstations for other db's.
			try
			{
				File.Delete(destinationPath);
			}
			catch (Exception ex)
			{
				FriendlyException.Show(Lan.G("FormUpdate", "Error deleting file:") + "\r\n" + ex.Message, ex);
				MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");
				return;
			}
			WebRequest wr = WebRequest.Create(downloadUri);
			WebResponse webResp = null;
			try
			{
				webResp = wr.GetResponse();
			}
			catch (Exception ex)
			{
				CodeBase.MsgBoxCopyPaste msgbox = new MsgBoxCopyPaste(ex.Message + "\r\nUri: " + downloadUri);
				msgbox.ShowDialog();
				MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");
				return;
			}
			int fileSize = (int)webResp.ContentLength / 1024;
			FormProgress FormP = new FormProgress();
			//start the thread that will perform the download
			ThreadStart downloadDelegate = delegate { DownloadInstallPatchWorker(downloadUri, destinationPath, webResp.ContentLength, ref FormP); };
			Thread workerThread = new Thread(downloadDelegate);
			workerThread.Start();
			//display the progress dialog to the user:
			FormP.MaxVal = (double)fileSize / 1024;
			FormP.NumberMultiplication = 100;
			FormP.DisplayText = "?currentVal MB of ?maxVal MB copied";
			FormP.NumberFormat = "F";
			FormP.ShowDialog();
			if (FormP.DialogResult == DialogResult.Cancel)
			{
				workerThread.Abort();
				MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");
				return;
			}
			//copy to second destination directory

			if (destinationPath2 != null && destinationPath2 != "")
			{
				if (File.Exists(destinationPath2))
				{
					try
					{
						File.Delete(destinationPath2);
					}
					catch (Exception ex)
					{
						FriendlyException.Show(Lan.G("FormUpdate", "Error deleting file:") + "\r\n" + ex.Message, ex);
						MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
						Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");
						return;
					}
				}
				File.Copy(destinationPath, destinationPath2);
			}


			//copy the Setup.exe to the AtoZ folders for the other db's.
			List<string> atozNameList = MiscData.GetAtoZforDb(dblist);
			for (int i = 0; i < atozNameList.Count; i++)
			{
				if (destinationPath == Path.Combine(atozNameList[i], "Setup.exe"))
				{//if they are sharing an AtoZ folder.
					continue;
				}
				if (Directory.Exists(atozNameList[i]))
				{
					File.Copy(destinationPath,//copy the Setup.exe that was just downloaded to this AtoZ folder
						Path.Combine(atozNameList[i], "Setup.exe"),//to the other atozFolder
						true);//overwrite
				}
			}
			if (!runSetupAfterDownload)
			{
				return;
			}
			string msg = Lan.G("FormUpdate", "Download succeeded.  Setup program will now begin.  When done, restart the program on this computer, then on the other computers.");
			if (dblist.Length > 0)
			{
				msg = "Download succeeded.  Setup file probably copied to other AtoZ folders as well.  Setup program will now begin.  When done, restart the program for each database on this computer, then on the other computers.";
			}
			if (MessageBox.Show(msg, "", MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				//Clicking cancel gives the user a chance to avoid running the setup program,
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");//unlock workstations, since nothing was actually done.
				return;
			}
			#region Stop OpenDent Services
			//If the update has been initiated from the designated update server then try and stop all "OpenDent..." services.
			//They will be automatically restarted once Open Dental has successfully upgraded.
			if (PrefC.GetString(PrefName.WebServiceServerName) != "" && ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.WebServiceServerName)))
			{
				Action actionCloseStopServicesProgress = ODProgress.Show(ODEventType.MiscData, typeof(MiscDataEvent), "Stopping services...");
				List<ServiceController> listOpenDentServices = ServicesHelper.GetAllOpenDentServices();
				//Newer versions of Windows have heightened security measures for managing services.
				//We get lots of calls where users do not have the correct permissions to start and stop Open Dental services.
				//Open Dental services are not important enough to warrent "Admin" rights to manage so we want to allow "Everyone" to start and stop them.
				ServicesHelper.SetSecurityDescriptorToAllowEveryoneToManageServices(listOpenDentServices);
				//Loop through all Open Dental services and stop them if they have not stopped or are not pending a stop so that their binaries can be updated.
				string servicesNotStopped = ServicesHelper.StopServices(listOpenDentServices);
				actionCloseStopServicesProgress?.Invoke();
				//Notify the user to go manually stop the services that could not automatically stop.
				if (!string.IsNullOrEmpty(servicesNotStopped))
				{
					MsgBoxCopyPaste msgBCP = new MsgBoxCopyPaste(Lan.G("FormUpdate", "The following services could not be stopped.  You need to manually stop them before continuing.")
					+ "\r\n" + servicesNotStopped);
					msgBCP.ShowDialog();
				}
			}
			#endregion
			try
			{
				Process.Start(destinationPath);
				FormOpenDental.S_ProcessKillCommand();
			}
			catch
			{
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");//unlock workstations, since nothing was actually done.
				MsgBox.Show("Could not launch setup");
			}
		}

		///<summary>This is the function that the worker thread uses to actually perform the download.
		///Can also call this method in the ordinary way if the file to be transferred is short.</summary>
		private static void DownloadInstallPatchWorker(string downloadUri, string destinationPath, long contentLength, ref FormProgress progressIndicator)
		{
			using (WebClient webClient = new WebClient())
			using (Stream streamRead = webClient.OpenRead(downloadUri))
			using (FileStream fileStream = new FileStream(destinationPath, FileMode.Create))
			{
				int bytesRead;
				long position = 0;
				byte[] buffer = new byte[10 * 1024];
				try
				{
					while ((bytesRead = streamRead.Read(buffer, 0, buffer.Length)) > 0)
					{
						position += bytesRead;
						if (position != contentLength)
						{
							progressIndicator.CurrentVal = ((double)position / 1024) / 1024;
						}
						fileStream.Write(buffer, 0, bytesRead);
					}
				}
				catch (Exception ex)
				{
					//Set the error message so that the user can call in and complain and we can get more information about what went wrong.
					//This error message will NOT show if the user hit the Cancel button and a random exception happened (because the window will have closed).
					progressIndicator.ErrorMessage = ex.Message;
				}
			}
			//If the file was successfully downloaded, set the progress indicator to maximum so that it closes the progress window.
			//Otherwise leave the window open so that the error message can be displayed to the user in red text.
			if (string.IsNullOrEmpty(progressIndicator.ErrorMessage))
			{
				progressIndicator.CurrentVal = (double)contentLength / 1024;
			}
			else
			{//There was an unexpected error.
				try
				{
					File.Delete(destinationPath);//Try to clean up after ourselves.
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// This ONLY runs when first opening the program. 
		/// Gets run early in the sequence. Returns false if the program should exit.
		/// </summary>
		public static bool CheckMySqlVersion(bool isSilent = false)
		{
			bool hasBackup = false;
			string thisVersion = MiscData.GetMySqlVersion();
			Version versionMySQL = new Version(thisVersion);
			if (versionMySQL < new Version(5, 0))
			{
				FormOpenDental.ExitCode = 110; // MySQL version lower than 5.0
				if (!isSilent)
				{
					MessageBox.Show(
						"Your version of MySQL won't work with this program: " + thisVersion + ". " +
						"You should upgrade to MySQL 5.0 using the installer on our website.");
				}
				return false;
			}

			if (!PrefC.ContainsKey("MySqlVersion"))
			{//db has not yet been updated to store this pref
			 //We're going to skip this.  We will recommend that people first upgrade OD, then MySQL, so this won't be an issue.
			}
			else
			{//Using a version that stores the MySQL version as a preference.
			 //There was an old bug where the MySQLVersion preference could be stored as 5,5 instead of 5.5 due to converting the version into a float.
			 //Replace any commas with periods before checking if the preference is going to change.
			 //This is simply an attempt to avoid making unnecessary backups for users with a corrupt version (e.g. 5,5).
				if (PrefC.GetString(PrefName.MySqlVersion).Contains(","))
				{
					Prefs.UpdateString(PrefName.MySqlVersion, PrefC.GetString(PrefName.MySqlVersion).Replace(",", "."));
				}
				//Now check to see if the MySQL version has been updated.  If it has, make an automatic backup, repair, and optimize all tables.
				if (Prefs.UpdateString(PrefName.MySqlVersion, (thisVersion)))
				{
#if !DEBUG
					if(!isSilent) {
						if(!MsgBox.Show("Prefs",MsgBoxButtons.OKCancel,"Tables will now be backed up, optimized, and repaired.  This will take a minute or two.  Continue?")) {
							FormOpenDental.ExitCode=0;
							return false;
						}
					}
					if(!Shared.BackupRepairAndOptimize(isSilent,BackupLocation.ConvertScript,false)) {
						FormOpenDental.ExitCode=101;//Database Backup failed
						return false;
					}
					hasBackup=true;
#endif
				}
			}


			//ClassConvertDatabase CCD=new ClassConvertDatabase();
			if (!hasBackup)
			{//A backup could have been made if the tables were optimized and repaired above.
				if (!Shared.MakeABackup(isSilent, BackupLocation.ConvertScript, false))
				{
					FormOpenDental.ExitCode = 101;//Database Backup failed
					return false;//but this should never happen
				}
			}

			if (!isSilent)
			{
				MsgBox.Show("Backup performed");
			}

			return true;
		}

		/// <summary>
		/// This runs when first opening the program. 
		/// If MySql is not at 5.5 or higher, it reminds the user, but does not force them to upgrade.
		/// </summary>
		public static void MySqlVersion55Remind()
		{
			string thisVersion = MiscData.GetMySqlVersion();
			Version versionMySQL = new Version(thisVersion);

			if (versionMySQL < new Version(5, 5) && !Programs.IsEnabled(ProgramName.eClinicalWorks))
			{//Do not show msg if MySQL version is 5.5 or greater or eCW is enabled
				MsgBox.Show(
					"You should upgrade to MySQL 5.5 using the installer posted on our website. " +
					"It's not urgent, but until you upgrade, you are likely to get a few errors each day which will require restarting the MySQL service.");
			}
		}

		private static bool CheckProgramVersionClassic()
		{
			Version storedVersion = new Version(PrefC.GetString(PrefName.ProgramVersion));
			Version currentVersion = new Version(Application.ProductVersion);
			string database = MiscData.GetCurrentDatabase();
			if (storedVersion < currentVersion)
			{
				Prefs.UpdateString(PrefName.ProgramVersion, currentVersion.ToString());
				UpdateHistory updateHistory = new UpdateHistory(currentVersion.ToString());
				UpdateHistories.Insert(updateHistory);
				Cache.Refresh(InvalidType.Prefs);
			}
			if (storedVersion > currentVersion)
			{
				//if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
				//	string setupBinPath=ODFileUtils.CombinePaths(OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath(),"Setup.exe");
				//	if(File.Exists(setupBinPath)) {
				//		if(MessageBox.Show("You are attempting to run version "+currentVersion.ToString(3)+",\r\n"
				//			+"But the database "+database+"\r\n"
				//			+"is already using version "+storedVersion.ToString(3)+".\r\n"
				//			+"A newer version must have already been installed on at least one computer.\r\n"  
				//			+"The setup program stored in your A to Z folder will now be launched.\r\n"
				//			+"Or, if you hit Cancel, then you will have the option to download again."
				//			,"",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
				//			if(MessageBox.Show("Download again?","",MessageBoxButtons.OKCancel)
				//				==DialogResult.OK) {
				//				FormUpdate FormU=new FormUpdate();
				//				FormU.ShowDialog();
				//			}
				//			Application.Exit();
				//			return false;
				//		}
				//		try {
				//			Process.Start(setupBinPath);
				//		}
				//		catch {
				//			MessageBox.Show("Could not launch Setup.exe");
				//		}
				//	}
				//	else if(MessageBox.Show("A newer version has been installed on at least one computer,"+
				//			"but Setup.exe could not be found in any of the following paths: "+
				//			OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath()+".  Download again?","",MessageBoxButtons.OKCancel)==DialogResult.OK) {
				//		FormUpdate FormU=new FormUpdate();
				//		FormU.ShowDialog();
				//	}
				//}
				//else {//Not using image path.
				//	//perform program update automatically.
				//	string patchName="Setup.exe";
				//	string updateUri=PrefC.GetString(PrefName.UpdateWebsitePath);
				//	string updateCode=PrefC.GetString(PrefName.UpdateCode);
				//	string updateInfoMajor="";
				//	string updateInfoMinor="";
				//	if(ShouldDownloadUpdate(updateUri,updateCode,out updateInfoMajor,out updateInfoMinor)) {
				//		if(MessageBox.Show(updateInfoMajor+Lan.G("Prefs","Perform program update now?"),"",
				//			MessageBoxButtons.YesNo)==DialogResult.Yes) {
				//			string tempFile=ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),patchName);//Resort to a more common temp file name.
				//			DownloadInstallPatchFromURI(updateUri+updateCode+"/"+patchName,//Source URI
				//				tempFile,true,true,null);//Local destination file.
				//			if(File.Exists(tempFile)) {//If user canceld in DownloadInstallPatchFromURI file will not exist.
				//				File.Delete(tempFile);//Cleanup install file.
				//			}
				//		}
				//	}
				//}
				Application.Exit();//always exits, whether launch of setup worked or not
				return false;
			}
			return true;
		}

		///<summary>Checks to see any OpenDentalCustListener services are currently installed.
		///If present, each CustListener service will be uninstalled.
		///After successfully removing all CustListener services, one eConnector service will be installed.
		///Returns true if the CustListener service was successfully upgraded to the eConnector service.</summary>
		///<param name="isSilent">Set to false to show meaningful error messages, otherwise fails silently.</param>
		///<param name="isListening">Will get set to true if the customer was previously using the CustListener service.</param>
		///<returns>True if only one CustListener services present and was successfully uninstalled along with the eConnector service getting installed.
		///False if more than one CustListener service is present or the eConnector service could not install.</returns>
		public static bool UpgradeOrInstallEConnector(bool isSilent, out bool isListening)
		{
			isListening = false;
			try
			{
				//Check to see if CustListener service is installed and needs to be uninstalled.
				List<ServiceController> listCustListenerServices = ServicesHelper.GetServicesByExe("OpenDentalCustListener.exe");
				if (listCustListenerServices.Count > 0)
				{
					isListening = true;
				}
				if (listCustListenerServices.Count == 1)
				{//Only uninstall the listener service if there is exactly one found.  This is just a nicety.
					ServicesHelper.Uninstall(listCustListenerServices[0]);
				}
				List<ServiceController> listEConnectorServices = ServicesHelper.GetServicesByExe("OpenDentalEConnector.exe");
				if (listEConnectorServices.Count > 0)
				{
					return true;//An eConnector service is already installed.
				}
				string eConnectorExePath = ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(), "OpenDentalEConnector", "OpenDentalEConnector.exe");
				FileInfo eConnectorExeFI = new FileInfo(eConnectorExePath);
				if (!ServicesHelper.Install("OpenDentalEConnector", eConnectorExeFI))
				{
					if (!isSilent)
					{
						throw new ApplicationException(Lans.g("ServicesHelper", "Unable to install the OpenDentalEConnector service."));
					}
					return false;
				}
				//Create a new OpenDentalWebConfig.xml file for the eConnector if one is not already present.
				if (!CreateConfigForEConnector(isListening))
				{
					if (!isSilent)
					{
						throw new ApplicationException(Lans.g("ServicesHelper", "The config file for the OpenDentalEConnector service could not be created."));
					}
					return false;
				}
				//Now that the service has finally installed we need to try and start it.
				listEConnectorServices = ServicesHelper.GetServicesByExe("OpenDentalEConnector.exe");
				if (listEConnectorServices.Count < 1)
				{
					if (!isSilent)
					{
						throw new ApplicationException(Lans.g("ServicesHelper", "OpenDentalEConnector service could not be found in order to automatically start it."));
					}
					return false;
				}
				string eConnectorStartingErrors = ServicesHelper.StartServices(listEConnectorServices);
				if (!string.IsNullOrEmpty(eConnectorStartingErrors))
				{
					if (!isSilent)
					{
						throw new ApplicationException(Lans.g("ServicesHelper", "Unable to start the following eConnector services:") + "\r\n" + eConnectorStartingErrors);
					}
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				if (!isSilent)
				{
					MessageBox.Show(Lans.g("ServicesHelper", "Failed upgrading to the eConnector service:") + "\r\n" + ex.Message
						+ "\r\n\r\n" + Lans.g("ServicesHelper", "Try running as Administrator."));
				}
				return false;
			}
		}

		///<summary>Tries to install the OpenDentalService if needed.  Returns false if failed.
		///Set isSilent to false to show meaningful error messages, otherwise fails silently.</summary>
		public static bool TryInstallOpenDentalService(bool isSilent)
		{
			try
			{
				List<ServiceController> listOpenDentalServices = ServicesHelper.GetServicesByExe("OpenDentalService.exe");
				if (listOpenDentalServices.Count > 0)
				{
					return true;//An Open Dental Service is already installed.
				}
				string odServiceFilePath = ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(), "OpenDentalService", "OpenDentalService.exe");
				FileInfo odServiceExeFI = new FileInfo(odServiceFilePath);
				if (!ServicesHelper.Install("OpenDentalService", odServiceExeFI))
				{
					AlertItems.CreateGenericAlert(Lans.g("ServicesHelper", "Open Dental Service Error"), Lans.g("ServicesHelper", "Failed to install OpenDentalService, try running as admin."));
					return false;
				}
				//Create a new OpenDentalServiceConfig.xml file for Open Dental Service if one is not already present.
				if (!CreateConfigForOpenDentalService())
				{
					AlertItems.CreateGenericAlert(Lans.g("ServicesHelper", "Open Dental Service Error"), Lans.g("ServicesHelper", "Failed to create OpenDentalServiceConfig.xml file."));
					return false;
				}
				//Now that the service has finally installed we need to try and start it.
				listOpenDentalServices = ServicesHelper.GetServicesByExe("OpenDentalService.exe");
				if (listOpenDentalServices.Count < 1)
				{
					AlertItems.CreateGenericAlert(Lans.g("ServicesHelper", "Open Dental Service Error"), Lans.g("ServicesHelper", "OpenDental Service could not be found."));
					return false;
				}
				string openDentalServiceStartingErrors = ServicesHelper.StartServices(listOpenDentalServices);
				if (!string.IsNullOrEmpty(openDentalServiceStartingErrors))
				{
					AlertItems.CreateGenericAlert(Lans.g("ServicesHelper", "Open Dental Service Error"), Lans.g("ServicesHelper", "The following service(s) could not start:") + " " + openDentalServiceStartingErrors);
					return false;
				}
				return true;
			}
			catch (Exception e)
			{
				AlertItems.CreateGenericAlert(Lans.g("ServicesHelper", "Open Dental Service Error"), Lans.g("ServicesHelper", "Unknown exception:") + " " + e.Message);
				return false;
			}
		}

		///<summary>Creates a default OpenDentalWebConfig.xml file for the eConnector if one is not already present.
		///Uses the current connection settings in DataConnection.  This method does NOT work if called via middle tier.
		///Users should not be installing the eConnector via the middle tier.</summary>
		private static bool CreateConfigForEConnector(bool isListening)
		{
			throw new NotImplementedException();
			//string eConnectorConfigPath = ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(), "OpenDentalEConnector", "OpenDentalWebConfig.xml");
			//string custListenerConfigPath = ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(), "OpenDentalCustListener", "OpenDentalWebConfig.xml");
			////Check to see if there is already a config file present.
			//if (File.Exists(eConnectorConfigPath))
			//{
			//	return true;//Nothing to do.
			//}
			////At this point we know that the eConnector does not have a config file present.
			////Check to see if the user is currently using the CustListener service.
			//if (isListening)
			//{
			//	//Try and grab a copy of the CustListener service config file first.
			//	if (File.Exists(custListenerConfigPath))
			//	{
			//		try
			//		{
			//			File.Copy(custListenerConfigPath, "", false);
			//			//If we got to this point the copy was successful and now the eConnector has a valid config file.
			//			return true;
			//		}
			//		catch (Exception)
			//		{
			//			//The copy didn't work for some reason.  Simply try to create a new file in the eConnector directory.
			//		}
			//	}
			//}
			//string mySqlPassHash;
			//CDT.Class1.Encrypt(DataConnection.GetMysqlPass(), out mySqlPassHash);
			//return ServicesHelper.CreateServiceConfigFile(eConnectorConfigPath
			//	, DataConnection.GetServerName()
			//	, DataConnection.GetDatabaseName()
			//	, DataConnection.GetMysqlUser()
			//	, DataConnection.GetMysqlPass()
			//	, mySqlPassHash
			//	, DataConnection.GetMysqlUserLow()
			//	, DataConnection.GetMysqlPassLow());
		}

		///<summary>Creates a default OpenDentalServiceConfig.xml file for Open Dental Service if one is not already present.
		///Uses the current connection settings in DataConnection.  This method does NOT work if called via middle tier.
		///Users should not be installing Open Dental Service via the middle tier.</summary>
		public static bool CreateConfigForOpenDentalService()
		{
			throw new NotImplementedException();

			//string odServiceConfigPath = ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(), "OpenDentalService", "OpenDentalServiceConfig.xml");
			////Check to see if there is already a config file present.
			//if (File.Exists(odServiceConfigPath))
			//{
			//	return true;//Nothing to do.
			//}
			////At this point we know that Open Dental Service does not have a config file present.
			//string mySqlPassHash;
			//CDT.Class1.Encrypt(DataConnection.GetMysqlPass(), out mySqlPassHash);
			//return ServicesHelper.CreateServiceConfigFile(odServiceConfigPath
			//	, DataConnection.GetServerName()
			//	, DataConnection.GetDatabaseName()
			//	, DataConnection.GetMysqlUser()
			//	, DataConnection.GetMysqlPass()
			//	, mySqlPassHash
			//	, DataConnection.GetMysqlUserLow()
			//	, DataConnection.GetMysqlPassLow());
		}


		///<summary>Performs both upgrades and downgrades by recopying update files from DB to temp folder, then from temp folder to the path specified.
		///Returns whether the whole process from downloading the files to copying them was successful.</summary>
		///<param name="storedVersion">A version object that represents the ProgramVersion preference in the database.</param>
		///<param name="currentVersion">A version object that represents the currently running version (Application.ProductVersion).</param>
		///<param name="destDir">The directory that the server files will be copied to.</param>
		///<param name="doKillServices">Indicates whether the file copier should kill all services before copying the update files.</param>
		///<param name="useLocalUpdateFileCopier">If set, this will use the update file copier in the local installation directory rather than the 
		///one downloaded from the server.</param>
		///<param name="openCopiedFiles">Tells the file copier to open the copied files after completion.</param>
		private static bool UpdateClientFromServerUpdateFiles(Version storedVersion, Version currentVersion, string destDir, bool doKillServices,
			bool useLocalUpdateFileCopier, bool openCopiedFiles)
		{
			string folderUpdate = ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(), "UpdateFiles");
			if (!DownloadUpdateFilesFromDatabase(folderUpdate))
			{
				return false;//if something failed while downloading.
			}
			//look at the manifest to see if it's the version we need
			string manifestVersion = "";
			ODException.SwallowAnyException(() =>
			{
				manifestVersion = File.ReadAllText(ODFileUtils.CombinePaths(folderUpdate, "Manifest.txt"));
			});
			if (manifestVersion != storedVersion.ToString(3))
			{//manifest version is wrong
				string manpath = ODFileUtils.CombinePaths(folderUpdate, "Manifest.txt");
				string message = Lan.G("Prefs", "The expected version information was not found in this file:") + " " + manpath + ".  "
					+ Lan.G("Prefs", "There is probably a permission issue on that folder which should be fixed.\r\n\r\n"
					+ "The suggested solution is to return to the computer where the update was just run.  Go to Help | "
					+ "Update | Setup, and click the Recopy button.");
				//If they were copying the files to a dynamic mode folder, do not install the exe. Give them the troubleshooting message with no option.
				if (destDir != Application.StartupPath)
				{
					MessageBox.Show(message);
					Environment.Exit(0);
					return false;
				}
				else
				{
					//No point trying the Setup.exe because that's probably wrong too.
					//Just go straight to downloading and running the Setup.exe.
					if (MessageBox.Show(message + "\r\n\r\n" + Lan.G("Prefs", "If, instead, you click OK in this window, then a fresh Setup file will be "
						+ "downloaded and run."), "", MessageBoxButtons.OKCancel) != DialogResult.OK)//they don't want to download again.
					{
						FormOpenDental.ExitCode = 312;//Stored version is higher that client version after an update was successful.
						Environment.Exit(FormOpenDental.ExitCode);
						return false;
					}
					DownloadAndRunSetup(storedVersion, currentVersion);
					Environment.Exit(0);
					return false;
				}
			}
			//Manifest version matches. Show window if they are updating their main installation of Open Dental.
			if (destDir == Application.StartupPath)
			{//if this is copying the files to the installation folder
				if (MessageBox.Show(Lan.G("Prefs", "Files will now be copied.") + "\r\n"
					+ Lan.G("Prefs", "Workstation version will be updated from ") + currentVersion.ToString(3)
					+ Lan.G("Prefs", " to ") + storedVersion.ToString(3),
					"", MessageBoxButtons.OKCancel)
					!= DialogResult.OK)//they don't want to update for some reason.
				{
					Environment.Exit(0);
					return false;
				}
			}
			return OpenFileCopier(folderUpdate, destDir, doKillServices, useLocalUpdateFileCopier, openCopiedFiles);
		}

		///<summary>Downloads the update files from the database and places them in the given folder. Returns false if anything went wrong.</summary>
		///<param name="tempFolderUpdate">The temporary folder used to store the update files before being copied.</param>
		private static bool DownloadUpdateFilesFromDatabase(string tempFolderUpdate)
		{
			if (Directory.Exists(tempFolderUpdate))
			{
				try
				{
					Directory.Delete(tempFolderUpdate, true);
				}
				catch (Exception ex)
				{
					FriendlyException.Show(Lan.G("Prefs", "Unable to delete update files from local temp folder. Try closing and reopening the program."), ex);
					FormOpenDental.ExitCode = 301;//UpdateFiles folder cannot be deleted
					Environment.Exit(FormOpenDental.ExitCode);
					return false;
				}
			}
			StringBuilder strBuilder = new StringBuilder();
			DocumentMisc docUpdateFilesPart = null;
			int count = 1;
			string fileName = count.ToString().PadLeft(4, '0');
			while ((docUpdateFilesPart = DocumentMiscs.GetByTypeAndFileName(fileName, DocumentMiscType.UpdateFilesSegment)) != null)
			{
				strBuilder.Append(docUpdateFilesPart.RawBase64);
				count++;
				fileName = count.ToString().PadLeft(4, '0');
			}
			ODException.SwallowAnyException(() =>
			{
				//strBuilder.ToString() has a tendency to fail when the string contains roughly 170MB of data.
				//If that becomes a typical size for our Update Files folder we should consider not storing the data as Base64.
				byte[] rawBytes = Convert.FromBase64String(strBuilder.ToString());
				using (ZipFile unzipped = ZipFile.Read(rawBytes))
				{
					unzipped.ExtractAll(tempFolderUpdate);
				}
			});//fail silently
			return true;
		}

		///<summary>Sets up the executable file and opens the UpdateFileCopier with the correct command line arguments passed in. Returns whether
		///the file copier was successfully started.</summary>
		///<param name="folderUpdate">Where the update files are stored.</param>
		///<param name="destDir">Where the update files will be copied to.</param>
		///<param name="doKillServices">Will tell the file copier whether to kill all Open Dental services or not.</param>
		///<param name="useLocalUpdateFileCopier">Will use the update file copier in the local installation directory rather than the one downloaded from
		///the server.</param>
		///<param name="openCopiedFiles">Tells the file copier to open the copied files after completion.</param>
		public static bool OpenFileCopier(string folderUpdate, string destDir, bool doKillServices, bool useLocalUpdateFileCopier, bool openCopiedFiles)
		{
			string tempDir = PrefC.GetTempFolderPath();
			//copy UpdateFileCopier.exe to the temp directory
			//In the case of using dynamic mode, because we have modified the update file copier when we released this feature, we need to be 
			//guarenteed we are using the correct version. We know that the version in our installation directory has the updates we need as otherwise
			//they would never be able to reach these lines of code.
			string updateFileCopierLocation = "";
			if (useLocalUpdateFileCopier)
			{
				if (Application.StartupPath.Contains("DynamicMode"))
				{
					//If they are within a different dynamic mode folder, the installation directory will be two directories up.
					updateFileCopierLocation = Directory.GetParent(Directory.GetParent(Application.StartupPath).FullName).FullName;
				}
				else
				{
					//Otherwise, this is the installation directory.
					updateFileCopierLocation = Application.StartupPath;
				}
			}
			else
			{//Otherwise use the update file copier from the server.
				updateFileCopierLocation = folderUpdate;
			}
			try
			{
				File.Copy(ODFileUtils.CombinePaths(updateFileCopierLocation, "UpdateFileCopier.exe"),//source
					ODFileUtils.CombinePaths(tempDir, "UpdateFileCopier.exe"),//dest
					true);//overwrite
			}
			catch (Exception ex)
			{
				FriendlyException.Show(Lans.g("Prefs", "Unable to copy ") + "UpdateFileCopier.exe " + Lans.g("Prefs", "from ") + updateFileCopierLocation + ".", ex);
				return false;
			}
			//wait a moment to make sure the file was copied
			Thread.Sleep(500);
			//launch UpdateFileCopier to copy all files to here.
			int processId = Process.GetCurrentProcess().Id;
			string startFileName = ODFileUtils.CombinePaths(tempDir, "UpdateFileCopier.exe");
			string arguments = "\"" + folderUpdate + "\""//pass the source directory to the file copier.
				+ " " + processId.ToString()//and the processId of Open Dental.
				+ " \"" + destDir + "\""//and the destination directory
				+ " " + doKillServices.ToString()//and whether to kill all processes or not
				+ " " + openCopiedFiles.ToString();//and whether to open the copied files or not.
			try
			{
				Process proc = new Process();
				proc.StartInfo.FileName = startFileName;
				proc.StartInfo.Arguments = arguments;
				proc.Start();
				proc.WaitForExit();//Waits for the file copier to be complete.
			}
			catch (Exception ex)
			{
				FriendlyException.Show(Lan.G("Prefs", "Unable to start the update file copier. Try closing and reopening the program."), ex);
				FormOpenDental.ExitCode = 305;//Unable to start the UpdateFileCopier.exe process.
				Environment.Exit(FormOpenDental.ExitCode);
				return false;
			}
			return true;
		}

		/// <summary>Check for a developer only license</summary>
		public static bool IsRegKeyForTesting()
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			StringBuilder strbuild = new StringBuilder();
			using (XmlWriter writer = XmlWriter.Create(strbuild, settings))
			{
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
				writer.WriteEndElement();
			}
			try
			{
				string response = CustomerUpdatesProxy.GetWebServiceInstance().RequestIsDevKey(strbuild.ToString());
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(response);
				XmlNode node = doc.SelectSingleNode("//IsDevKey");
				return PIn.Bool(node.InnerText);
			}
			catch
			{
				//They don't have an external internet connection.
				return false;
			}
		}
	}
}
