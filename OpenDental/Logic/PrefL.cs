using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

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

		/// <summary>
		/// Returns true if the download at the specified remoteUri with the given registration code should be downloaded and installed as an update, and false is returned otherwise.
		/// Also, information about the decision making process is stored in the updateInfoMajor and updateInfoMinor strings, but only holds significance to a human user.
		/// </summary>
		public static bool ShouldDownloadUpdate(string remoteUri, string updateCode, out string updateInfoMajor, out string updateInfoMinor)
		{
			updateInfoMajor = "";
			updateInfoMinor = "";
			bool shouldDownload = false;
			string fileName = "Manifest.txt";
			WebClient myWebClient = new WebClient();
			string myStringWebResource = remoteUri + updateCode + "/" + fileName;
            string strNewVersion = "";
            string newBuild = "";
			bool buildIsAlpha = false;
			bool buildIsBeta = false;
			bool versionIsAlpha = false;
			bool versionIsBeta = false;
            Version versionNewBuild;
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
                updateInfoMajor += "Registration number not valid, or internet connection failed.  ";
                return false;
            }

            if (versionNewBuild == new Version(Application.ProductVersion))
			{
				updateInfoMajor += "You are using the most current build of this version.  ";
			}
			else
			{
				//this also allows users to install previous versions.
				updateInfoMajor += "A new build of this version is available for download:  " + versionNewBuild.ToString();
				if (buildIsAlpha)
				{
					updateInfoMajor += "(alpha)  ";
				}
				if (buildIsBeta)
				{
					updateInfoMajor += "(beta)  ";
				}
				shouldDownload = true;
			}
			//Whether or not build is current, we want to inform user about the next minor version
			if (strNewVersion != null)
			{//we don't really care what it is.
				updateInfoMinor += "A newer version is also available.  ";
				if (versionIsAlpha)
				{
					updateInfoMinor += "It is alpha (experimental), so it has bugs and " +
						"you will need to update it frequently.  ";
				}
				if (versionIsBeta)
				{
					updateInfoMinor += "It is beta (test), so it has some bugs and " +
						"you will need to update it frequently.  ";
				}
				updateInfoMinor += "Contact us for a new Registration number if you wish to use it.  ";
			}
			return shouldDownload;
		}

		/// <summary>
		/// destinationPath includes filename (Setup.exe). 
		/// destinationPath2 will create a second copy at the specified path/filename, or it will be skipped if null or empty.
		/// </summary>
		public static void DownloadInstallPatchFromURI(string downloadUri, string destinationPath, bool runSetupAfterDownload, bool showShutdownWindow, string destinationPath2)
		{
			string[] databases = Prefs.GetString(PrefName.UpdateMultipleDatabases).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

			bool isShutdownWindowNeeded = showShutdownWindow;
			while (isShutdownWindowNeeded)
			{
				// Even if updating multiple databases, extra shutdown signals are not needed.
				using (var formShutdown = new FormShutdown())
				{
					formShutdown.IsUpdate = true;

					if (formShutdown.ShowDialog() == DialogResult.OK)
					{
						// Turn off signal reception for 5 seconds so this workstation will not shut down.
						Signalods.SignalLastRefreshed = MiscData.GetNowDateTime().AddSeconds(5);
						Signalods.Send(SignalName.Shutdown);

						Computers.ClearAllHeartBeats(Environment.MachineName);//always assume success
						isShutdownWindowNeeded = false;
						//SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Shutdown all workstations.");//can't do this because sometimes no user.
					}
					else 
					{
						if (MsgBox.Show("FormUpdate", MsgBoxButtons.YesNo, "Are you sure you want to cancel the update?"))
						{
							return;
						}

						continue;
					}
				}

				// No other workstation will be able to start up until this value is reset.
				Prefs.Set(PrefName.UpdateInProgressOnComputerName, Environment.MachineName);
			}

			Prefs.Set(PrefName.UpdateInProgressOnComputerName, Environment.MachineName);

			try
			{
				File.Delete(destinationPath);
			}
			catch (Exception ex)
			{
				FriendlyException.Show("Error deleting file:\r\n" + ex.Message, ex);

				Prefs.Set(PrefName.UpdateInProgressOnComputerName, "");

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

				Prefs.Set(PrefName.UpdateInProgressOnComputerName, "");

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

				Prefs.Set(PrefName.UpdateInProgressOnComputerName, "");

				return;
			}

			// copy to second destination directory

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
						FriendlyException.Show("Error deleting file:\r\n" + ex.Message, ex);

						Prefs.Set(PrefName.UpdateInProgressOnComputerName, "");

						return;
					}
				}

				File.Copy(destinationPath, destinationPath2);
			}

			if (!runSetupAfterDownload)
			{
				return;
			}

			string msg = 
				"Download succeeded. " +
				"Setup program will now begin. " +
				"When done, restart the program on this computer, then on the other computers.";

			if (databases.Length > 0)
			{
				msg = 
					"Download succeeded. " +
					"Setup file probably copied to other AtoZ folders as well. " +
					"Setup program will now begin. " +
					"When done, restart the program for each database on this computer, then on the other computers.";
			}

			if (CodeBase.ODMessageBox.Show(msg, "", MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				//Clicking cancel gives the user a chance to avoid running the setup program,
				Prefs.Set(PrefName.UpdateInProgressOnComputerName, "");//unlock workstations, since nothing was actually done.
				return;
			}

			try
			{
				Process.Start(destinationPath);
				FormOpenDental.S_ProcessKillCommand();
			}
			catch
			{
				Prefs.Set(PrefName.UpdateInProgressOnComputerName, "");//unlock workstations, since nothing was actually done.
				MsgBox.Show("Could not launch setup");
			}
		}

		/// <summary>
		/// This is the function that the worker thread uses to actually perform the download.
		/// Can also call this method in the ordinary way if the file to be transferred is short.
		/// </summary>
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


			// Using a version that stores the MySQL version as a preference.
			// There was an old bug where the MySQLVersion preference could be stored as 5,5 instead of 5.5 due to converting the version into a float.
			// Replace any commas with periods before checking if the preference is going to change.
			// This is simply an attempt to avoid making unnecessary backups for users with a corrupt version (e.g. 5,5).
			if (Prefs.GetString(PrefName.MySqlVersion).Contains(","))
			{
				Prefs.Set(PrefName.MySqlVersion, Prefs.GetString(PrefName.MySqlVersion).Replace(",", "."));
			}

			//Now check to see if the MySQL version has been updated. If it has, make an automatic backup, repair, and optimize all tables.
			if (Prefs.Set(PrefName.MySqlVersion, thisVersion))
			{
#if !DEBUG
				if (!isSilent)
				{
					if (!MsgBox.Show("Prefs", MsgBoxButtons.OKCancel, "Tables will now be backed up, optimized, and repaired.  This will take a minute or two.  Continue?"))
					{
						FormOpenDental.ExitCode = 0;
						return false;
					}
				}

				if (!Shared.BackupRepairAndOptimize(isSilent, BackupLocation.ConvertScript, false))
				{
					FormOpenDental.ExitCode = 101;//Database Backup failed
					return false;
				}

				hasBackup = true;
#endif
			}

			// ClassConvertDatabase CCD=new ClassConvertDatabase();
			if (!hasBackup)
			{
				// A backup could have been made if the tables were optimized and repaired above.
				if (!Shared.MakeABackup(isSilent, BackupLocation.ConvertScript, false))
				{
					FormOpenDental.ExitCode = 101; // Database Backup failed
					return false; // but this should never happen
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
			Version storedVersion = new Version(Prefs.GetString(PrefName.ProgramVersion));
			Version currentVersion = new Version(Application.ProductVersion);

			string database = MiscData.GetCurrentDatabase();
			if (storedVersion < currentVersion)
			{
				Prefs.Set(PrefName.ProgramVersion, currentVersion.ToString());
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
				//	string updateUri=Prefs.GetString(PrefName.UpdateWebsitePath);
				//	string updateCode=Prefs.GetString(PrefName.UpdateCode);
				//	string updateInfoMajor="";
				//	string updateInfoMinor="";
				//	if(ShouldDownloadUpdate(updateUri,updateCode,out updateInfoMajor,out updateInfoMinor)) {
				//		if(MessageBox.Show(updateInfoMajor+"Perform program update now?","",
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
	}
}
