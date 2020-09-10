using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using System.IO;
using CodeBase;
using DataConnectionBase;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Net;
using Imedisoft.Data;

namespace OpenDental {
	public partial class FormReportSetup:ODForm {
		private bool changed;
		///<summary>Either the currently logged in user or the user of a group selected in the Security window.</summary>
		private long _userGroupNum;
		private bool _isPermissionMode;
		public bool HasReportPerms; 

		public FormReportSetup(long userGroupNum,bool isPermissionMode) {
			InitializeComponent();
			
			_userGroupNum=userGroupNum;
			_isPermissionMode=isPermissionMode;
		}

		private void FormReportSetup_Load(object sender,EventArgs e) {
			if(!PrefC.HasClinicsEnabled) {
				checkReportPIClinic.Visible=false;
				checkReportPIClinicInfo.Visible=false;
			}
			FillComboReportWriteoff();
			comboReportWriteoff.SelectedIndex=PrefC.GetInt(PreferenceName.ReportsPPOwriteoffDefaultToProcDate);
			checkProviderPayrollAllowToday.Checked=Preferences.GetBool(PreferenceName.ProviderPayrollAllowToday);
			checkNetProdDetailUseSnapshotToday.Checked=Preferences.GetBool(PreferenceName.NetProdDetailUseSnapshotToday);
			checkReportsShowPatNum.Checked=Preferences.GetBool(PreferenceName.ReportsShowPatNum);
			checkReportProdWO.Checked=Preferences.GetBool(PreferenceName.ReportPandIschedProdSubtractsWO);
			checkReportPIClinicInfo.Checked=Preferences.GetBool(PreferenceName.ReportPandIhasClinicInfo);
			checkReportPIClinic.Checked=Preferences.GetBool(PreferenceName.ReportPandIhasClinicBreakdown);
			checkReportPrintWrapColumns.Checked=Preferences.GetBool(PreferenceName.ReportsWrapColumns);
			checkReportsShowHistory.Checked=Preferences.GetBool(PreferenceName.ReportsShowHistory);
			checkReportsIncompleteProcsNoNotes.Checked=Preferences.GetBool(PreferenceName.ReportsIncompleteProcsNoNotes);
			checkReportsIncompleteProcsUnsigned.Checked=Preferences.GetBool(PreferenceName.ReportsIncompleteProcsUnsigned);
			checkBenefitAssumeGeneral.Checked=Preferences.GetBool(PreferenceName.TreatFinderProcsAllGeneral);
			checkOutstandingRpDateTab.Checked=Preferences.GetBool(PreferenceName.OutstandingInsReportDateFilterTab);
			checkReportDisplayUnearnedTP.Checked=Preferences.GetBool(PreferenceName.ReportsDoShowHiddenTPPrepayments);

				FillReportServer();
			
			userControlReportSetup.InitializeOnStartup(true,_userGroupNum,_isPermissionMode);
			if(_isPermissionMode) {
				tabControl1.SelectedIndex=1;
			}
		}

		private void FillReportServer() {
			checkUseReportServer.Checked=Preferences.GetString(PreferenceName.ReportingServerCompName)!="" || Preferences.GetString(PreferenceName.ReportingServerURI)!="";
			radioReportServerDirect.Checked=Preferences.GetString(PreferenceName.ReportingServerURI)=="";
			radioReportServerMiddleTier.Checked=Preferences.GetString(PreferenceName.ReportingServerURI)!="";
			comboServerName.Text=Preferences.GetString(PreferenceName.ReportingServerCompName);
			comboDatabase.Text=Preferences.GetString(PreferenceName.ReportingServerDbName);
			textMysqlUser.Text=Preferences.GetString(PreferenceName.ReportingServerMySqlUser);
			string decryptedPass;
			//CDT.Class1.Decrypt(Prefs.GetString(PrefName.ReportingServerMySqlPassHash),out decryptedPass);
			textMysqlPass.Text= Preferences.GetString(PreferenceName.ReportingServerMySqlPassHash);
			textMysqlPass.PasswordChar='*';
			textMiddleTierURI.Text=Preferences.GetString(PreferenceName.ReportingServerURI);
			FillComboComputers();
			FillComboDatabases();
			SetReportServerUIEnabled();
		}

		private void FillComboComputers() {
			comboServerName.Items.Clear();
			comboServerName.Items.AddRange(GetComputerNames());
		}

		private void FillComboDatabases() {
			comboDatabase.Items.Clear();
			comboDatabase.Items.AddRange(GetDatabases());
		}

		private void FillComboReportWriteoff() {
			comboReportWriteoff.Items.Clear();
			comboReportWriteoff.Items.AddEnums<PPOWriteoffDateCalc>();
		}

		private void SetReportServerUIEnabled() {
			if(!checkUseReportServer.Checked) {
				radioReportServerDirect.Enabled=false;
				radioReportServerMiddleTier.Enabled=false;
				groupConnectionSettings.Enabled=false;
				groupMiddleTier.Enabled=false;
			}
			else {
				radioReportServerDirect.Enabled=true;
				radioReportServerMiddleTier.Enabled=true;
				if(radioReportServerDirect.Checked) {
					groupConnectionSettings.Enabled=true;
					groupMiddleTier.Enabled=false;
				}
				else {
					groupConnectionSettings.Enabled=false;
					groupMiddleTier.Enabled=true;
				}
			}
		}

		///<summary>Gets a list of all computer names on the network (this is not easy)</summary>
		private string[] GetComputerNames() {
			if(Environment.OSVersion.Platform==PlatformID.Unix) {
				return new string[0];
			}
			try {
				File.Delete(ODFileUtils.CombinePaths(Application.StartupPath,"tempCompNames.txt"));
				ArrayList retList = new ArrayList();
				//string myAdd=Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();//obsolete
				string myAdd = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
				ProcessStartInfo psi = new ProcessStartInfo();
				psi.FileName=@"C:\WINDOWS\system32\cmd.exe";//Path for the cmd prompt
				psi.Arguments="/c net view > tempCompNames.txt";//Arguments for the command prompt
				//"/c" tells it to run the following command which is "net view > tempCompNames.txt"
				//"net view" lists all the computers on the network
				//" > tempCompNames.txt" tells dos to put the results in a file called tempCompNames.txt
				psi.WindowStyle=ProcessWindowStyle.Hidden;//Hide the window
				Process.Start(psi);
				StreamReader sr = null;
				string filename = ODFileUtils.CombinePaths(Application.StartupPath,"tempCompNames.txt");
				Thread.Sleep(200);//sleep for 1/5 second
				if(!File.Exists(filename)) {
					return new string[0];
				}
				try {
					sr=new StreamReader(filename);
				}
				catch(Exception) {
				}
				while(!sr.ReadLine().StartsWith("--")) {
					//The line just before the data looks like: --------------------------
				}
				string line = "";
				retList.Add("localhost");
				while(true) {
					line=sr.ReadLine();
					if(line.StartsWith("The"))//cycle until we reach,"The command completed successfully."
					break;
					line=line.Split(char.Parse(" "))[0];// Split the line after the first space
																	// Normally, in the file it lists it like this
																	// \\MyComputer                 My Computer's Description
																	// Take off the slashes, "\\MyComputer" to "MyComputer"
					retList.Add(line.Substring(2,line.Length-2));
				}
				sr.Close();
				File.Delete(ODFileUtils.CombinePaths(Application.StartupPath,"tempCompNames.txt"));
				string[] retArray = new string[retList.Count];
				retList.CopyTo(retArray);
				return retArray;
			}
			catch(Exception) {//it will always fail if not WinXP
				return new string[0];
			}
		}

		///<summary></summary>
		private string[] GetDatabases()
		{
			if (comboServerName.Text == "")
			{
				return new string[0];
			}
			try
			{
				DataConnection dcon;
				//use the one table that we know exists
				if (textMysqlUser.Text == "")
				{
					dcon = new DataConnection(comboServerName.Text, "root", textMysqlPass.Text, "mysql");
				}
				else
				{
					dcon = new DataConnection(comboServerName.Text, textMysqlUser.Text, textMysqlPass.Text, "mysql");
				}
				string command = "SHOW DATABASES";
				//if this next step fails, table will simply have 0 rows
				DataTable table = dcon.ExecuteDataTable(command, false);
				string[] dbNames = new string[table.Rows.Count];
				for (int i = 0; i < table.Rows.Count; i++)
				{
					dbNames[i] = table.Rows[i][0].ToString();
				}
				return dbNames;
			}
			catch (Exception)
			{
				return new string[0];
			}
		}

		private void checkReportingServer_CheckChanged(object sender,EventArgs e) {
			SetReportServerUIEnabled();
		}

		private void comboDatabase_DropDown(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			FillComboDatabases();
			Cursor=Cursors.Default;
		}

		private void tabControl1_SelectedIndexChanged(object sender,EventArgs e) {
			if(tabControl1.SelectedIndex==0) {
				userControlReportSetup.Parent=tabDisplaySettings;
				userControlReportSetup.InitializeOnStartup(false,_userGroupNum,false);//This will change usergroups when they change tabs.  NOT what we want......
			}
			else if(tabControl1.SelectedIndex==1) {
				if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
					tabControl1.SelectedIndex=0;
					return;
				}
				userControlReportSetup.Parent=tabReportPermissions;
				userControlReportSetup.InitializeOnStartup(false,_userGroupNum,true);
			}
		}

		private bool UpdateReportingServer() {
			bool changed=false;
			if(!checkUseReportServer.Checked) {
				if(Preferences.Set(PreferenceName.ReportingServerCompName,"")
						| Preferences.Set(PreferenceName.ReportingServerDbName,"")
						| Preferences.Set(PreferenceName.ReportingServerMySqlUser,"")
						| Preferences.Set(PreferenceName.ReportingServerMySqlPassHash,"")
						| Preferences.Set(PreferenceName.ReportingServerURI,"")) 
					{
					changed=true;
				}
			}
			else {
				if(radioReportServerDirect.Checked) {
					if(Preferences.Set(PreferenceName.ReportingServerCompName,comboServerName.Text)
							| Preferences.Set(PreferenceName.ReportingServerDbName,comboDatabase.Text)
							| Preferences.Set(PreferenceName.ReportingServerMySqlUser,textMysqlUser.Text)
							| Preferences.Set(PreferenceName.ReportingServerMySqlPassHash, textMysqlPass.Text)
							| Preferences.Set(PreferenceName.ReportingServerURI,"")
					) {
					changed=true;
					}
				}
				else {
					if(Preferences.Set(PreferenceName.ReportingServerCompName,"")
							|Preferences.Set(PreferenceName.ReportingServerDbName,"")
							|Preferences.Set(PreferenceName.ReportingServerMySqlUser,"")
							|Preferences.Set(PreferenceName.ReportingServerMySqlPassHash,"")
							|Preferences.Set(PreferenceName.ReportingServerURI,textMiddleTierURI.Text)
					) {
					changed=true;
					}
				}
			}
			return changed;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(Preferences.Set(PreferenceName.ReportsPPOwriteoffDefaultToProcDate,comboReportWriteoff.SelectedIndex)
				| Preferences.Set(PreferenceName.ReportsShowPatNum,checkReportsShowPatNum.Checked)
				| Preferences.Set(PreferenceName.ReportPandIschedProdSubtractsWO,checkReportProdWO.Checked)
				| Preferences.Set(PreferenceName.ReportPandIhasClinicInfo,checkReportPIClinicInfo.Checked)
				| Preferences.Set(PreferenceName.ReportPandIhasClinicBreakdown,checkReportPIClinic.Checked)
				| Preferences.Set(PreferenceName.ProviderPayrollAllowToday,checkProviderPayrollAllowToday.Checked)
				| Preferences.Set(PreferenceName.NetProdDetailUseSnapshotToday,checkNetProdDetailUseSnapshotToday.Checked)
				| Preferences.Set(PreferenceName.ReportsWrapColumns,checkReportPrintWrapColumns.Checked)
				| Preferences.Set(PreferenceName.ReportsIncompleteProcsNoNotes,checkReportsIncompleteProcsNoNotes.Checked)
				| Preferences.Set(PreferenceName.ReportsIncompleteProcsUnsigned,checkReportsIncompleteProcsUnsigned.Checked)
				| Preferences.Set(PreferenceName.TreatFinderProcsAllGeneral,checkBenefitAssumeGeneral.Checked)
				| Preferences.Set(PreferenceName.ReportsShowHistory,checkReportsShowHistory.Checked)
				| Preferences.Set(PreferenceName.OutstandingInsReportDateFilterTab,checkOutstandingRpDateTab.Checked)
				| Preferences.Set(PreferenceName.ReportsDoShowHiddenTPPrepayments,checkReportDisplayUnearnedTP.Checked)
				) 
			{
				changed=true;
			}
			if(changed) {
				DataValid.SetInvalid(InvalidType.Prefs);
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Report settings have been changed.");
			}
			if(Security.IsAuthorized(Permissions.SecurityAdmin,true)) {
				if(GroupPermissions.Sync(userControlReportSetup.ListGroupPermissionsForReports,userControlReportSetup.ListGroupPermissionsOld)) {
					SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin,0,"Report permissions have been changed.");
				}
				if(userControlReportSetup.ListGroupPermissionsForReports.Exists(x => x.UserGroupId==_userGroupNum)) {
					HasReportPerms=true;
				}
				DataValid.SetInvalid(InvalidType.Security);
			}
			if(DisplayReports.Sync(userControlReportSetup.ListDisplayReportAll)) {
				DataValid.SetInvalid(InvalidType.DisplayReports);
			}
			DialogResult=DialogResult.OK;
		}

    private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}