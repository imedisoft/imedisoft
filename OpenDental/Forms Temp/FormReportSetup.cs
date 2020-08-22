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
			comboReportWriteoff.SelectedIndex=PrefC.GetInt(PrefName.ReportsPPOwriteoffDefaultToProcDate);
			checkProviderPayrollAllowToday.Checked=Prefs.GetBool(PrefName.ProviderPayrollAllowToday);
			checkNetProdDetailUseSnapshotToday.Checked=Prefs.GetBool(PrefName.NetProdDetailUseSnapshotToday);
			checkReportsShowPatNum.Checked=Prefs.GetBool(PrefName.ReportsShowPatNum);
			checkReportProdWO.Checked=Prefs.GetBool(PrefName.ReportPandIschedProdSubtractsWO);
			checkReportPIClinicInfo.Checked=Prefs.GetBool(PrefName.ReportPandIhasClinicInfo);
			checkReportPIClinic.Checked=Prefs.GetBool(PrefName.ReportPandIhasClinicBreakdown);
			checkReportPrintWrapColumns.Checked=Prefs.GetBool(PrefName.ReportsWrapColumns);
			checkReportsShowHistory.Checked=Prefs.GetBool(PrefName.ReportsShowHistory);
			checkReportsIncompleteProcsNoNotes.Checked=Prefs.GetBool(PrefName.ReportsIncompleteProcsNoNotes);
			checkReportsIncompleteProcsUnsigned.Checked=Prefs.GetBool(PrefName.ReportsIncompleteProcsUnsigned);
			checkBenefitAssumeGeneral.Checked=Prefs.GetBool(PrefName.TreatFinderProcsAllGeneral);
			checkOutstandingRpDateTab.Checked=Prefs.GetBool(PrefName.OutstandingInsReportDateFilterTab);
			checkReportDisplayUnearnedTP.Checked=Prefs.GetBool(PrefName.ReportsDoShowHiddenTPPrepayments);

				FillReportServer();
			
			userControlReportSetup.InitializeOnStartup(true,_userGroupNum,_isPermissionMode);
			if(_isPermissionMode) {
				tabControl1.SelectedIndex=1;
			}
		}

		private void FillReportServer() {
			checkUseReportServer.Checked=Prefs.GetString(PrefName.ReportingServerCompName)!="" || Prefs.GetString(PrefName.ReportingServerURI)!="";
			radioReportServerDirect.Checked=Prefs.GetString(PrefName.ReportingServerURI)=="";
			radioReportServerMiddleTier.Checked=Prefs.GetString(PrefName.ReportingServerURI)!="";
			comboServerName.Text=Prefs.GetString(PrefName.ReportingServerCompName);
			comboDatabase.Text=Prefs.GetString(PrefName.ReportingServerDbName);
			textMysqlUser.Text=Prefs.GetString(PrefName.ReportingServerMySqlUser);
			string decryptedPass;
			//CDT.Class1.Decrypt(Prefs.GetString(PrefName.ReportingServerMySqlPassHash),out decryptedPass);
			textMysqlPass.Text= Prefs.GetString(PrefName.ReportingServerMySqlPassHash);
			textMysqlPass.PasswordChar='*';
			textMiddleTierURI.Text=Prefs.GetString(PrefName.ReportingServerURI);
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
				if(Prefs.Set(PrefName.ReportingServerCompName,"")
						| Prefs.Set(PrefName.ReportingServerDbName,"")
						| Prefs.Set(PrefName.ReportingServerMySqlUser,"")
						| Prefs.Set(PrefName.ReportingServerMySqlPassHash,"")
						| Prefs.Set(PrefName.ReportingServerURI,"")) 
					{
					changed=true;
				}
			}
			else {
				if(radioReportServerDirect.Checked) {
					if(Prefs.Set(PrefName.ReportingServerCompName,comboServerName.Text)
							| Prefs.Set(PrefName.ReportingServerDbName,comboDatabase.Text)
							| Prefs.Set(PrefName.ReportingServerMySqlUser,textMysqlUser.Text)
							| Prefs.Set(PrefName.ReportingServerMySqlPassHash, textMysqlPass.Text)
							| Prefs.Set(PrefName.ReportingServerURI,"")
					) {
					changed=true;
					}
				}
				else {
					if(Prefs.Set(PrefName.ReportingServerCompName,"")
							|Prefs.Set(PrefName.ReportingServerDbName,"")
							|Prefs.Set(PrefName.ReportingServerMySqlUser,"")
							|Prefs.Set(PrefName.ReportingServerMySqlPassHash,"")
							|Prefs.Set(PrefName.ReportingServerURI,textMiddleTierURI.Text)
					) {
					changed=true;
					}
				}
			}
			return changed;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(Prefs.Set(PrefName.ReportsPPOwriteoffDefaultToProcDate,comboReportWriteoff.SelectedIndex)
				| Prefs.Set(PrefName.ReportsShowPatNum,checkReportsShowPatNum.Checked)
				| Prefs.Set(PrefName.ReportPandIschedProdSubtractsWO,checkReportProdWO.Checked)
				| Prefs.Set(PrefName.ReportPandIhasClinicInfo,checkReportPIClinicInfo.Checked)
				| Prefs.Set(PrefName.ReportPandIhasClinicBreakdown,checkReportPIClinic.Checked)
				| Prefs.Set(PrefName.ProviderPayrollAllowToday,checkProviderPayrollAllowToday.Checked)
				| Prefs.Set(PrefName.NetProdDetailUseSnapshotToday,checkNetProdDetailUseSnapshotToday.Checked)
				| Prefs.Set(PrefName.ReportsWrapColumns,checkReportPrintWrapColumns.Checked)
				| Prefs.Set(PrefName.ReportsIncompleteProcsNoNotes,checkReportsIncompleteProcsNoNotes.Checked)
				| Prefs.Set(PrefName.ReportsIncompleteProcsUnsigned,checkReportsIncompleteProcsUnsigned.Checked)
				| Prefs.Set(PrefName.TreatFinderProcsAllGeneral,checkBenefitAssumeGeneral.Checked)
				| Prefs.Set(PrefName.ReportsShowHistory,checkReportsShowHistory.Checked)
				| Prefs.Set(PrefName.OutstandingInsReportDateFilterTab,checkOutstandingRpDateTab.Checked)
				| Prefs.Set(PrefName.ReportsDoShowHiddenTPPrepayments,checkReportDisplayUnearnedTP.Checked)
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