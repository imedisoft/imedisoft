using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using CodeBase;
using DataConnectionBase;
using OpenDental.Bridges;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormBackup : ODForm {		
		//private bool usesInternalImages;
		///<summary>This message will only get filled when a backup attempt has failed.  It will hold the message text that we want to show to the user giving them more information about the failure.</summary>
		private string _errorMessage;

		///<summary></summary>
		public FormBackup()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		private void FormBackup_Load(object sender, System.EventArgs e) {
			#region Backup Tab
			//usesInternalImages=(Prefs.GetString(PrefName.ImageStore)=="OpenDental.Imaging.SqlStore");
			checkExcludeImages.Checked=Prefs.GetBool(PrefName.BackupExcludeImageFolder);
			checkArchiveDoBackupFirst.Checked=Prefs.GetBool(PrefName.ArchiveDoBackupFirst);
			textBackupFromPath.Text=Prefs.GetString(PrefName.BackupFromPath);
			textBackupToPath.Text=Prefs.GetString(PrefName.BackupToPath);
			textBackupRestoreFromPath.Text=Prefs.GetString(PrefName.BackupRestoreFromPath);
			textBackupRestoreToPath.Text=Prefs.GetString(PrefName.BackupRestoreToPath);
			textBackupRestoreAtoZToPath.Text=Prefs.GetString(PrefName.BackupRestoreAtoZToPath);
			textBackupRestoreAtoZToPath.Enabled=ShouldUseAtoZFolder();
			butBrowseRestoreAtoZTo.Enabled=ShouldUseAtoZFolder();
			if(ProgramProperties.IsAdvertisingDisabled(ProgramName.CentralDataStorage)) {
				groupManagedBackups.Visible=false;
			}
			#endregion
			#region Archive Tab
			textArchivePass.Text= Prefs.GetString(PrefName.ArchivePassHash);
			textArchivePass.PasswordChar=(textArchivePass.Text=="" ? default(char) : '*');
			textArchiveServerName.Text=Prefs.GetString(PrefName.ArchiveServerName);
			textArchiveUser.Text=Prefs.GetString(PrefName.ArchiveUserName);
			//If pref is set, use it.  Otherwise, 3 years ago.
			dateTimeArchive.Value=PrefC.GetDate(PrefName.ArchiveDate)==DateTime.MinValue?DateTime.Today.AddYears(-3):PrefC.GetDate(PrefName.ArchiveDate);
			ToggleBackupSettings();
		#endregion
			#region Supplemental Tab
			checkSupplementalBackupEnabled.Checked=Prefs.GetBool(PrefName.SupplementalBackupEnabled);
			if(PrefC.GetDate(PrefName.SupplementalBackupDateLastComplete).Year > 1880) {
				textSupplementalBackupDateLastComplete.Text=PrefC.GetDate(PrefName.SupplementalBackupDateLastComplete).ToString();
			}
			textSupplementalBackupCopyNetworkPath.Text=Prefs.GetString(PrefName.SupplementalBackupNetworkPath);
			#endregion Supplemental Tab
		}

		#region Backup Tab

		private bool IsBackupTabValid() {
			//test for trailing slashes
			if(textBackupFromPath.Text!="" && !textBackupFromPath.Text.EndsWith(""+Path.DirectorySeparatorChar)){
				MessageBox.Show("Paths must end with "+Path.DirectorySeparatorChar+".");
				return false;
			}
			if(textBackupToPath.Text!="" && !textBackupToPath.Text.EndsWith(""+Path.DirectorySeparatorChar)){
				MessageBox.Show("Paths must end with "+Path.DirectorySeparatorChar+".");
				return false;
			}
			if(textBackupRestoreFromPath.Text!="" && !textBackupRestoreFromPath.Text.EndsWith(""+Path.DirectorySeparatorChar)) {
				MessageBox.Show("Paths must end with "+Path.DirectorySeparatorChar+".");
				return false;
			}
			if(textBackupRestoreToPath.Text!="" && !textBackupRestoreToPath.Text.EndsWith(""+Path.DirectorySeparatorChar)) {
				MessageBox.Show("Paths must end with "+Path.DirectorySeparatorChar+".");
				return false;
			}
			if(textBackupRestoreAtoZToPath.Text!="" && !textBackupRestoreAtoZToPath.Text.EndsWith(""+Path.DirectorySeparatorChar)) {
				MessageBox.Show("Paths must end with "+Path.DirectorySeparatorChar+".");
				return false;
			}
			return true;
		}

		private bool SaveTabPrefs() {
			bool hasChanged=false;
			hasChanged |= Prefs.Set(PrefName.BackupExcludeImageFolder,checkExcludeImages.Checked);
			hasChanged |= Prefs.Set(PrefName.ArchiveDoBackupFirst,checkArchiveDoBackupFirst.Checked);
			hasChanged |= Prefs.Set(PrefName.BackupFromPath,textBackupFromPath.Text);
			hasChanged |= Prefs.Set(PrefName.BackupToPath,textBackupToPath.Text);
			hasChanged |= Prefs.Set(PrefName.BackupRestoreFromPath,textBackupRestoreFromPath.Text);
			hasChanged |= Prefs.Set(PrefName.BackupRestoreToPath,textBackupRestoreToPath.Text);
			hasChanged |= Prefs.Set(PrefName.BackupRestoreAtoZToPath,textBackupRestoreAtoZToPath.Text);
			hasChanged |= Prefs.Set(PrefName.ArchiveServerName,textArchiveServerName.Text);
			hasChanged |= Prefs.Set(PrefName.ArchiveUserName,textArchiveUser.Text);
			hasChanged |= Prefs.Set(PrefName.ArchivePassHash, textArchivePass.Text);
			return hasChanged;
		}

		private bool ShouldUseAtoZFolder() {
			return (!checkExcludeImages.Checked);
		}

		private void butBrowseFrom_Click(object sender, System.EventArgs e) {
			FolderBrowserDialog browserDlg=new FolderBrowserDialog();
			browserDlg.SelectedPath=textBackupFromPath.Text;
			if(browserDlg.ShowDialog()==DialogResult.Cancel){
				return;
			}
			textBackupFromPath.Text=ODFileUtils.CombinePaths(browserDlg.SelectedPath,"");//Add trail slash.
		}

		private void butBrowseTo_Click(object sender, System.EventArgs e) {
			FolderBrowserDialog browserDlg=new FolderBrowserDialog();
			browserDlg.SelectedPath=textBackupToPath.Text;
			if(browserDlg.ShowDialog()==DialogResult.Cancel){
				return;
			}
			textBackupToPath.Text=ODFileUtils.CombinePaths(browserDlg.SelectedPath,"");//Add trail slash.
		}

		private void butBrowseRestoreFrom_Click(object sender, System.EventArgs e) {
			FolderBrowserDialog browserDlg=new FolderBrowserDialog();
			browserDlg.SelectedPath=textBackupRestoreFromPath.Text;
			if(browserDlg.ShowDialog()==DialogResult.Cancel){
				return;
			}
			textBackupRestoreFromPath.Text=ODFileUtils.CombinePaths(browserDlg.SelectedPath,"");//Add trail slash.
		}

		private void butBrowseRestoreTo_Click(object sender, System.EventArgs e) {
			FolderBrowserDialog browserDlg=new FolderBrowserDialog();
			browserDlg.SelectedPath=textBackupRestoreToPath.Text;
			if(browserDlg.ShowDialog()==DialogResult.Cancel){
				return;
			}
			textBackupRestoreToPath.Text=ODFileUtils.CombinePaths(browserDlg.SelectedPath,"");//Add trail slash.
		}

		private void butBrowseRestoreAtoZTo_Click(object sender, System.EventArgs e) {
			FolderBrowserDialog browserDlg=new FolderBrowserDialog();
			browserDlg.SelectedPath=textBackupRestoreAtoZToPath.Text;
			if(browserDlg.ShowDialog()==DialogResult.Cancel){
				return;
			}
			textBackupRestoreAtoZToPath.Text=ODFileUtils.CombinePaths(browserDlg.SelectedPath,"");//Add trail slash.
		}

		private void butBackup_Click(object sender, System.EventArgs e) {
			if(!IsBackupTabValid()) {
				return;
			}
			//Ensure that the backup from and backup to paths are different. This is to prevent the live database
			//from becoming corrupt.
			if(this.textBackupFromPath.Text.Trim().ToLower()==this.textBackupToPath.Text.Trim().ToLower()) {
				MessageBox.Show("The backup from path and backup to path must be different.");
				return;
			}
			//test saving defaults
			if(textBackupFromPath.Text!=Prefs.GetString(PrefName.BackupFromPath)
				|| textBackupToPath.Text!=Prefs.GetString(PrefName.BackupToPath)
				|| textBackupRestoreFromPath.Text!=Prefs.GetString(PrefName.BackupRestoreFromPath)
				|| textBackupRestoreToPath.Text!=Prefs.GetString(PrefName.BackupRestoreToPath)
				|| textBackupRestoreAtoZToPath.Text!=Prefs.GetString(PrefName.BackupRestoreAtoZToPath)) 
			{
				if(MsgBox.Show(MsgBoxButtons.YesNo,"Set as default?") && SaveTabPrefs()) {
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			string dbName=MiscData.GetCurrentDatabase();
			if(InnoDb.HasInnoDbTables(dbName)) {
				//Database has innodb tables. Backup tool does not work on dbs with InnoDb tables. 
				MessageBox.Show("InnoDb tables detected. Backup tool cannot run with InnoDb tables.");
				return;
			}
			if(!Directory.Exists(ODFileUtils.CombinePaths(textBackupFromPath.Text,dbName))){// C:\mysql\data\opendental
				MessageBox.Show("Backup FROM path is invalid.");
				return;
			}
			if(!Directory.Exists(textBackupToPath.Text)){// D:\
				MessageBox.Show("Backup TO path is invalid.");
				return;
			}
			_errorMessage="";
			FormP=new FormProgress();
			FormP.MaxVal=100;//We will be setting maxVal from worker thread.  (double)fileSize/1024;
			FormP.NumberMultiplication=100;
			FormP.DisplayText="";//We will set the text from the worker thread.
			FormP.NumberFormat="N1";
			//start the thread that will perform the database copy
			Thread workerThread=new Thread(new ThreadStart(InstanceMethodBackup));
			workerThread.Start();
			//display the progress dialog to the user:
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.Cancel){
				workerThread.Abort();
				return;
			}
			if(_errorMessage=="") {
				SecurityLogs.MakeLogEntry(Permissions.Backup,0,Lan.G(this,"Database backup created at ")+textBackupToPath.Text);
				MessageBox.Show(Lan.G(this,"Backup complete."));
			}
			else {//Backup failed for some reason.
				MessageBox.Show(_errorMessage);
			}
			Close();
		}

		///<summary>This is the function that the worker thread uses to perform the backup.</summary>
		private void InstanceMethodBackup(){
			curVal=0;
			Invoke(new PassProgressDelegate(PassProgressToDialog),new object [] { curVal,
				Lan.G(this,"Preparing to copy database"),//this happens very fast and probably won't be noticed.
				100,"" });//max of 100 keeps dlg from closing
			string dbName=MiscData.GetCurrentDatabase();
			ulong driveFreeSpace=0;
			double dbSize=GetFileSizes(textBackupFromPath.Text+dbName)/1024;
			//Attempt to get the free disk space on the drive or share of the destination folder.
			//If the free space cannot be determined the backup will be attempted anyway (old behavior).
			if(ODFileUtils.GetDiskFreeSpace(textBackupToPath.Text,out driveFreeSpace)) {
				if((ulong)dbSize*1024*1024>=driveFreeSpace) {//dbSize is in megabytes, cast to ulong to compare. It will never be negative so this is safe.
					Invoke(new ErrorMessageDelegate(SetErrorMessage),new object[] { Lan.G(this,"Not enough free disk space available on the destination drive to backup the database.") });
					//We now want to automatically close FormProgress.  This is done by clearing out the variables.
					Invoke(new PassProgressDelegate(PassProgressToDialog),new object[] { 0,"",0,"" });
					return;
				}
			}
			try{
				string dbtopath=ODFileUtils.CombinePaths(textBackupToPath.Text,dbName);
				if(Directory.Exists(dbtopath)){// D:\opendental
					int loopCount=1;
					while(Directory.Exists(dbtopath+"backup_"+loopCount)){
						loopCount++;
					}
				  Directory.Move(dbtopath,dbtopath+"backup_"+loopCount);
				}
				string fromPath=ODFileUtils.CombinePaths(textBackupFromPath.Text,dbName);
				string toPath=textBackupToPath.Text;
				DirectoryInfo dirInfo=new DirectoryInfo(fromPath);//does not check to see if dir exists
				Directory.CreateDirectory(ODFileUtils.CombinePaths(toPath,dirInfo.Name));
				FileInfo[] files=dirInfo.GetFiles();
				curVal=0;//curVal gets increased
				for(int i=0;i<files.Length;i++){
					string fromFile=files[i].FullName;
					string toFile=ODFileUtils.CombinePaths(new string[] { toPath,dirInfo.Name,files[i].Name });
					if(File.Exists(toFile)) {
						if(files[i].LastWriteTime!=File.GetLastWriteTime(toFile)) {//if modification dates don't match
							FileAttributes fa=File.GetAttributes(toFile);
							bool isReadOnly=((fa&FileAttributes.ReadOnly)==FileAttributes.ReadOnly);
							if(isReadOnly) {
								//If the destination file exists and is marked as read only, then we must mark it as a
								//normal read/write file before it may be overwritten.
								File.SetAttributes(toFile,FileAttributes.Normal);//Remove read only from the destination file.
							}
							File.Copy(fromFile,toFile,true);
						}
					} else {//file doesn't exist, so just copy
						File.Copy(fromFile,toFile);
					}
					curVal+=(double)files[i].Length/(double)1024/(double)1024;
					if(curVal<dbSize){//this avoids setting progress bar to max, which would close the dialog.
						Invoke(new PassProgressDelegate(PassProgressToDialog),new object [] { curVal,
							Lan.G(this,"Database: ?currentVal MB of ?maxVal MB copied"),
							dbSize,""});
					}
				}
			}
			catch{//for instance, if abort.
				//If the user aborted, FormP will return DialogResult.Cancel which will not cause this error text to be displayed to the user.  See butBackup_Click for more info.
				Invoke(new ErrorMessageDelegate(SetErrorMessage),new object[] { Lan.G(this,"Backup failed.") });
				//We now want to automatically close FormProgress.  This is done by clearing out the variables.
				Invoke(new PassProgressDelegate(PassProgressToDialog),new object[] { 0,"",0,"" });
				return;
			}
			//A to Z folder------------------------------------------------------------------------------------
			try {
				if(ShouldUseAtoZFolder()) {
					string atozFull=ODFileUtils.RemoveTrailingSeparators(OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath());
					string atozDir=atozFull.Substring(atozFull.LastIndexOf(Path.DirectorySeparatorChar)+1);//OpenDentalData
					Invoke(new PassProgressDelegate(PassProgressToDialog),new object[] { 0,
					Lan.G(this,"Calculating size of files in A to Z folder."),
					100,"" });//max of 100 keeps dlg from closing
					long atozSize=GetFileSizes(ODFileUtils.CombinePaths(atozFull,""),
						ODFileUtils.CombinePaths(new string[] { textBackupToPath.Text,atozDir,"" }))/1024; 
					driveFreeSpace=0;
					//Attempt to get the free disk space on the drive or share of the destination folder.
					//If the free space cannot be determined the backup will be attempted anyway (old behavior).
					if(ODFileUtils.GetDiskFreeSpace(textBackupToPath.Text,out driveFreeSpace)) {
						if((ulong)(atozSize*1024*1024)>=driveFreeSpace) {//atozSize is in megabytes, cast to ulong in order to compare.  It will never be negative so it's safe.
							//Not enough free space to perform the backup.
							throw new ApplicationException(Lan.G(this,"Backing up A to Z images folder failed.  Not enough free disk space available on the destination drive.")
								+"\r\n"+Lan.G(this,"AtoZ folder size:")+" "+atozSize*1024*1024+"B\r\n"
								+Lan.G(this,"Destination available space:")+" "+driveFreeSpace+"B");
						}
					}
					if(!Directory.Exists(ODFileUtils.CombinePaths(textBackupToPath.Text,atozDir))) {// D:\OpenDentalData
						Directory.CreateDirectory(ODFileUtils.CombinePaths(textBackupToPath.Text,atozDir));// D:\OpenDentalData
					}
					curVal=0;
					CopyDirectoryIncremental(ODFileUtils.CombinePaths(atozFull,""),// C:\OpenDentalData\
						ODFileUtils.CombinePaths(new string[] { textBackupToPath.Text,atozDir,"" }),// D:\OpenDentalData\
						atozSize);
				}
			}
			catch(ApplicationException ex) {
				Invoke(new ErrorMessageDelegate(SetErrorMessage),new object[] { ex.Message }); 
			}
			catch {
				Invoke(new ErrorMessageDelegate(SetErrorMessage),new object[] { Lan.G(this,"Backing up A to Z images folder failed.  User might not have enough permissions or a file might be in use.") });
			}
			//force dialog to close even if no files copied or calculation was slightly off.
			Invoke(new PassProgressDelegate(PassProgressToDialog),new object[] { 0,"",0,"" });
		}

		///<summary>This is the function that the worker thread uses to restore the A-Z folder.</summary>
		private void InstanceMethodRestore(){
			curVal=0;
			string atozFull=textBackupRestoreAtoZToPath.Text;// C:\OpenDentalData\
			//remove the trailing \
			atozFull=atozFull.Substring(0,atozFull.Length-1);// C:\OpenDentalData
			string atozDir=atozFull.Substring(atozFull.LastIndexOf(Path.DirectorySeparatorChar)+1);// OpenDentalData
			Invoke(new PassProgressDelegate(PassProgressToDialog),new object [] { 0,
				Lan.G(this,"Database restored.\r\nCalculating size of files in A to Z folder."),
				100,"" });//max of 100 keeps dlg from closing
			long atozSize=GetFileSizes(ODFileUtils.CombinePaths(new string[] {textBackupRestoreFromPath.Text,atozDir,""}),
				ODFileUtils.CombinePaths(atozFull,""))/1024;// C:\OpenDentalData\
			if(!Directory.Exists(atozFull)){// C:\OpenDentalData\
				Directory.CreateDirectory(atozFull);// C:\OpenDentalData\
			}
			curVal=0;
			CopyDirectoryIncremental(ODFileUtils.CombinePaths(new string[] {textBackupRestoreFromPath.Text,atozDir,""}),
				ODFileUtils.CombinePaths(atozFull,""),// C:\OpenDentalData\
				atozSize);
			//force dlg to close even if no files copied or calculation was slightly off.
			Invoke(new PassProgressDelegate(PassProgressToDialog),new object[] { 0,"",0,"" });
		}

		///<summary>This function gets invoked from the worker threads.</summary>
		private void SetErrorMessage(string errorMessage) {
			_errorMessage=errorMessage;
		}

		///<summary>This function gets invoked from the worker threads.</summary>
		private void PassProgressToDialog(double currentVal,string displayText,double maxVal,string errorMessage){
			FormP.CurrentVal=currentVal;
			FormP.DisplayText=displayText;
			FormP.MaxVal=maxVal;
			FormP.ErrorMessage=errorMessage;
		}

		///<summary>Counts the total KB of all files that will need to be copied from one directory to another.  Recursive.  Only includes missing files, not changed files.  Used to display the progress bar.  Supplied paths must end in \. toPath might not exist.</summary>
		private long GetFileSizes(string fromPath,string toPath){
			long retVal=0;
			DirectoryInfo dirInfo=new DirectoryInfo(fromPath);
			DirectoryInfo[] dirs=dirInfo.GetDirectories();
			for(int i=0;i<dirs.Length;i++){
				retVal+=GetFileSizes(ODFileUtils.CombinePaths(dirs[i].FullName,""),
					ODFileUtils.CombinePaths(new string[] {toPath,dirs[i].Name,""}));
			}
			FileInfo[] files=dirInfo.GetFiles();//of fromPath
			for(int i=0;i<files.Length;i++){
				if(!File.Exists(ODFileUtils.CombinePaths(toPath,files[i].Name))){
					retVal+=(long)(files[i].Length/1024);
				}
			}
			return retVal;
		}

		///<summary>Counts the total KB of all files in the given directory.  Not recursive since it's just used for db files.  Used to display the progress bar.</summary>
		private long GetFileSizes(string fromPath) {
			long retVal=0;
			DirectoryInfo dirInfo=new DirectoryInfo(fromPath);
			FileInfo[] files=dirInfo.GetFiles();
			for(int i=0;i<files.Length;i++){
				retVal+=(long)(files[i].Length/1024);
			}
			return retVal;
		}

		///<summary>A recursive fuction that copies any new or changed files or folders from one directory to another.  An exception will be thrown if either directory does not already exist.  fromPath is the fully qualified path of the directory to copy.  toPath is the fully qualified path of the destination directory.  Both paths must include a trailing \.  The max size should be calculated ahead of time.  It's passed in for use in progress bar.</summary>
		private void CopyDirectoryIncremental(string fromPath,string toPath,double maxSize){
			if(!Directory.Exists(fromPath)){
				throw new Exception(fromPath+" does not exist.");
			}
			if(!Directory.Exists(toPath)){
				throw new Exception(toPath+" does not exist.");
			}
			DirectoryInfo dirInfo=new DirectoryInfo(fromPath);
			DirectoryInfo[] dirs=dirInfo.GetDirectories();
			for(int i=0;i<dirs.Length;i++){
				string destPath=ODFileUtils.CombinePaths(toPath,dirs[i].Name);
				if(!Directory.Exists(destPath)){
					Directory.CreateDirectory(destPath);
				}
				CopyDirectoryIncremental(ODFileUtils.CombinePaths(dirs[i].FullName,""),
					ODFileUtils.CombinePaths(destPath,""),maxSize);
			}
			FileInfo[] files=dirInfo.GetFiles();//of fromPath
			for(int i=0;i<files.Length;i++){
				string fromFile=files[i].FullName;
				string toFile=ODFileUtils.CombinePaths(toPath,files[i].Name);
				if(File.Exists(toFile)){
					if(files[i].LastWriteTime!=File.GetLastWriteTime(toFile)){//if modification dates don't match
						FileAttributes fa=File.GetAttributes(toFile);
						bool isReadOnly=((fa&FileAttributes.ReadOnly)==FileAttributes.ReadOnly);
						if(isReadOnly){
							//If the destination file exists and is marked as read only, then we must mark it as a
							//normal read/write file before it may be overwritten.
							File.SetAttributes(toFile,FileAttributes.Normal);//Remove read only from the destination file.
						}
						File.Copy(fromFile,toFile,true);
					}
				}
				else{//file doesn't exist, so just copy
					File.Copy(fromFile,toFile);
				}
				curVal+=(double)files[i].Length/1048576.0; //Number of megabytes.
				if(curVal<maxSize) {//this avoids setting progress bar to max, which would close the dialog.
					Invoke(new PassProgressDelegate(PassProgressToDialog),new object[] { curVal,
							Lan.G(this,"A to Z folder: ?currentVal MB of ?maxVal MB copied"),
							maxSize,""});
				}
			}
		}

		private void butRestore_Click(object sender, System.EventArgs e) {			
			if(textBackupRestoreFromPath.Text!="" && !textBackupRestoreFromPath.Text.EndsWith(""+Path.DirectorySeparatorChar)){
				MessageBox.Show(Lan.G(this,"Paths must end with ")+Path.DirectorySeparatorChar+".");
				return;
			}
			if(textBackupRestoreToPath.Text!="" && !textBackupRestoreToPath.Text.EndsWith(""+Path.DirectorySeparatorChar)){
				MessageBox.Show(Lan.G(this,"Paths must end with ")+Path.DirectorySeparatorChar+".");
				return;
			}
			if(ShouldUseAtoZFolder()) {
				if(textBackupRestoreAtoZToPath.Text!="" && !textBackupRestoreAtoZToPath.Text.EndsWith(""+Path.DirectorySeparatorChar)){
					MessageBox.Show(Lan.G(this,"Paths must end with ")+Path.DirectorySeparatorChar+".");
					return;
				}
			}
			if(Environment.OSVersion.Platform!=PlatformID.Unix){
				//dmg This check will not work on Linux, because mapped drives exist as regular (mounted) paths. Perhaps there
				//is another way to check for this on Linux.
				if(textBackupRestoreToPath.Text!="" && textBackupRestoreToPath.Text.StartsWith(""+Path.DirectorySeparatorChar)){
					MessageBox.Show("The restore database TO folder must be on this computer.");
					return;
				}
			}
			//pointless to save defaults
			string dbName=MiscData.GetCurrentDatabase();
			if(InnoDb.HasInnoDbTables(dbName)) {
				//Database has innodb tables. Restore tool does not work on dbs with InnoDb tables. 
				MessageBox.Show("InnoDb tables detected. Restore tool cannot run with InnoDb tables.");
				return;
			}
			if(!Directory.Exists(ODFileUtils.CombinePaths(textBackupRestoreFromPath.Text,dbName))){// D:\opendental
				MessageBox.Show(Lan.G(this,"Restore FROM path is invalid.  Unable to find folder named ")+dbName);
				return;
			}
			if(!Directory.Exists(ODFileUtils.CombinePaths(textBackupRestoreToPath.Text,dbName))) {// C:\mysql\data\opendental
				MessageBox.Show(Lan.G(this,"Restore TO path is invalid.  Unable to find folder named ")+dbName);
				return;
			}
			if(ShouldUseAtoZFolder()) {
				if(!Directory.Exists(textBackupRestoreAtoZToPath.Text)) {// C:\OpenDentalData\
					MessageBox.Show("Restore A-Z images TO path is invalid.");
					return;
				}
				string atozFull=textBackupRestoreAtoZToPath.Text;// C:\OpenDentalData\
				//remove the trailing \
				atozFull=atozFull.Substring(0,atozFull.Length-1);// C:\OpenDentalData
				string atozDir=atozFull.Substring(atozFull.LastIndexOf(Path.DirectorySeparatorChar)+1);// OpenDentalData
				if(!Directory.Exists(ODFileUtils.CombinePaths(textBackupRestoreFromPath.Text,atozDir))){// D:\OpenDentalData
					MessageBox.Show("Restore A-Z images FROM path is invalid.");
					return;
				}
			}
			string fromPath=ODFileUtils.CombinePaths(new string[] {textBackupRestoreFromPath.Text,dbName,""});// D:\opendental\
			DirectoryInfo dirInfo=new DirectoryInfo(fromPath);//does not check to see if dir exists
			if(MessageBox.Show(Lan.G(this,"Restore from backup created on")+"\r\n"
				+dirInfo.LastWriteTime.ToString("dddd")+"  "+dirInfo.LastWriteTime.ToString()
				,"",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)==DialogResult.Cancel) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			//stop the service--------------------------------------------------------------------------------------
			//ServiceController sc=new ServiceController("MySQL");
			//if(!ServicesHelper.Stop(sc)) {
			//	MessageBox.Show("Unable to stop MySQL service.");
			//	Cursor=Cursors.Default;
			//	return;
			//}
			//rename the current database---------------------------------------------------------------------------
			//Get a name for the new directory
			string newDb=dbName+"backup_"+DateTime.Today.ToString("MM_dd_yyyy");
			if(Directory.Exists(ODFileUtils.CombinePaths(textBackupRestoreToPath.Text,newDb))){//if the new database name already exists
				//find a unique one
				int uniqueID=1;
				string originalNewDb=newDb;
				do{
					newDb=originalNewDb+"_"+uniqueID.ToString();
					uniqueID++;
				}
				while(Directory.Exists(ODFileUtils.CombinePaths(textBackupRestoreToPath.Text,newDb)));
			}
			//move the current db (rename)
			Directory.Move(ODFileUtils.CombinePaths(textBackupRestoreToPath.Text,dbName)
				,ODFileUtils.CombinePaths(textBackupRestoreToPath.Text,newDb));
			//Restore----------------------------------------------------------------------------------------------
			string toPath=textBackupRestoreToPath.Text;// C:\mysql\data\
			Directory.CreateDirectory(ODFileUtils.CombinePaths(toPath,dirInfo.Name));
			FileInfo[] files=dirInfo.GetFiles();
			curVal=0;//curVal gets increased
			for(int i=0;i<files.Length;i++){
				File.Copy(files[i].FullName,ODFileUtils.CombinePaths(new string[] {toPath,dirInfo.Name,files[i].Name}));
			}

			Cursor=Cursors.Default;
			//restore A-Z folder, and give user a chance to cancel it.
			if(ShouldUseAtoZFolder()) {
				FormP=new FormProgress();
				FormP.MaxVal=100;//We will be setting maxVal from worker thread.  (double)fileSize/1024;
				FormP.NumberMultiplication=100;
				FormP.DisplayText="";//We will set the text from the worker thread.
				FormP.NumberFormat="N1";
				//start the thread that will perform the database copy
				Thread workerThread=new Thread(new ThreadStart(InstanceMethodRestore));
				workerThread.Start();
				//display the progress dialog to the user:
				FormP.ShowDialog();
				if(FormP.DialogResult==DialogResult.Cancel){
					workerThread.Abort();
					return;
				}
			}
			Version programVersionDb=new Version(Prefs.GetStringNoCache(PrefName.ProgramVersion));
			Version programVersionCur=new Version(Application.ProductVersion);
			if(programVersionDb!=programVersionCur) {
				MessageBox.Show("The restored database version is different than the version installed and requires a restart.  The program will now close.");
				FormOpenDental.S_ProcessKillCommand();
				return;
			}
			else {
				DataValid.SetInvalid(Cache.GetAllCachedInvalidTypes().ToArray());
			}
			MessageBox.Show("Done");
			Close();
			return;
		}

		private void butSave_Click(object sender, System.EventArgs e) {
			if(!IsBackupTabValid()) {
				return;
			}
			if(SaveTabPrefs()) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void checkExcludeImages_Click(object sender,EventArgs e) {
			textBackupRestoreAtoZToPath.Enabled=ShouldUseAtoZFolder();
			butBrowseRestoreAtoZTo.Enabled=ShouldUseAtoZFolder();
		}

		private void pictureCDS_Click(object sender,EventArgs e) {
			CDS.ShowPage();
		}

		#endregion

		#region Archive Tab

		private void ToggleBackupSettings() {
			groupBoxBackupConnection.GetAllControls().ForEach(x => x.Enabled=checkArchiveDoBackupFirst.Checked);
		}

		private void checkMakeBackup_CheckedChanged(object sender,EventArgs e) {
			ToggleBackupSettings();
		}
		
		private void butArchive_Click(object sender,EventArgs e) {
			#region Validation
			if(checkArchiveDoBackupFirst.Checked) { //We only need to validate the backup settings if the user wants to make a backup first
				if(!MsgBox.Show(MsgBoxButtons.YesNo,"To make a backup of the database, ensure no other machines are currently using OpenDental. Proceed?")) {
					return;
				}
				//Validation
				if(string.IsNullOrWhiteSpace(textArchiveServerName.Text)) {
					MessageBox.Show("Please specify a Server Name.");
					return;
				}
				if(string.IsNullOrWhiteSpace(textArchiveUser.Text)) {
					MessageBox.Show("Please enter a User.");
					return;
				}
				if(string.IsNullOrWhiteSpace(Prefs.GetString(PrefName.ArchiveKey))) {//If archive key isn't set, generate a new one.
					string archiveKey=MiscUtils.CreateRandomAlphaNumericString(10);
					Prefs.Set(PrefName.ArchiveKey,archiveKey);
				}
			}
			#endregion
			//Create an ODProgress
			ODProgress.ShowAction(() => {
				//Make a backup if needed
				if(checkArchiveDoBackupFirst.Checked) {
					try { 
						MiscData.MakeABackup();
					}
					catch(Exception ex) {
						FriendlyException.Show("An error occurred backing up the old database. Old data was not removed from the database. "+
							"Ensure no other machines are currently using OpenDental and try again.",ex);
						return;
					}
				}
				//Delete the unnecessary data
				SecurityLogs.DeleteBeforeDateInclusive(dateTimeArchive.Value);
				SecurityLogs.MakeLogEntry(Permissions.Backup,0,$"SecurityLog and SecurityLogHashes on/before {dateTimeArchive.Value} deleted.");
			},
			eventType:typeof(MiscDataEvent),
			odEventType:EventCategory.MiscData);
		}

		private void butSaveArchive_Click(object sender,EventArgs e) {
			if(SaveTabPrefs()) {
				DataValid.SetInvalid(InvalidType.Prefs);
				MessageBox.Show("Saved");
			}

			Prefs.Set(PrefName.ArchiveDate, dateTimeArchive.Value);
        }

		#endregion

		#region Supplemental Tab

		private void TabControl1Tab_Selected(object sender,TabControlEventArgs e) {
			if(e.TabPage==tabPageSupplementalBackups) {
				if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
					tabControl1.SelectedTab=tabPageBackup;
					return;
				}
			}
		}

		private void ButSupplementalBrowse_Click(object sender,EventArgs e) {
			if(folderBrowserSupplementalCopyNetworkPath.ShowDialog()==DialogResult.OK) {
				textSupplementalBackupCopyNetworkPath.Text=folderBrowserSupplementalCopyNetworkPath.SelectedPath;
			}
		}

		private void ButSupplementalSaveDefaults_Click(object sender,EventArgs e) {
			if(!string.IsNullOrEmpty(textSupplementalBackupCopyNetworkPath.Text) && !Directory.Exists(textSupplementalBackupCopyNetworkPath.Text)) {
				MessageBox.Show("Invalid or inaccessible "+labelSupplementalBackupCopyNetworkPath.Text+".");//This label text will rarely change.
				return;
			}
			if(Prefs.Set(PrefName.SupplementalBackupEnabled,checkSupplementalBackupEnabled.Checked)) {
				try {
					//Inform HQ when the supplemental backups are enabled/disabled and which security admin performed the change.
					PayloadItem pliStatus=new PayloadItem(
						(int)(checkSupplementalBackupEnabled.Checked?SupplementalBackupStatuses.Enabled:SupplementalBackupStatuses.Disabled),
						"SupplementalBackupStatus");
					PayloadItem pliAdminUserName=new PayloadItem(Security.CurrentUser.UserName,"AdminUserName");
					string officeData=PayloadHelper.CreatePayload(new List<PayloadItem>() { pliStatus,pliAdminUserName },eServiceCode.SupplementalBackup);
					WebServiceMainHQProxy.GetWebServiceMainHQInstance().SetSupplementalBackupStatus(officeData);
				}
				catch {
					//Internet probably is unavailble right now.
				}
				SecurityLogs.MakeLogEntry(Permissions.SupplementalBackup,0,
					"Supplemental backup has been "+(checkSupplementalBackupEnabled.Checked?"Enabled":"Disabled")+".");
			}
			if(Prefs.Set(PrefName.SupplementalBackupNetworkPath,textSupplementalBackupCopyNetworkPath.Text)) {
				SecurityLogs.MakeLogEntry(Permissions.SupplementalBackup,0,
					labelSupplementalBackupCopyNetworkPath.Text+" changed to '"+textSupplementalBackupCopyNetworkPath.Text+"'.");
			}
			MessageBox.Show("Saved");
		}

		#endregion Supplemental Tab

	}

	///<summary>Backing up can fail at two points, when backing up the database or the A to Z images.  This delegate lets the backup thread manipulate a local variable so that we can let the user know at what point the backup failed.</summary>
	public delegate void ErrorMessageDelegate(string errorMessage);
}





















