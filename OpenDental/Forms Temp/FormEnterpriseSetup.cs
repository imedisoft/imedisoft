using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEnterpriseSetup:ODForm {
		private int _claimReportReceiveInterval;
		private YN _usePhonenumTable=YN.Unknown;

		public FormEnterpriseSetup() {
			InitializeComponent();
			
		}

		private void FormEnterpriseSetup_Load(object sender,EventArgs e) {
			textPatSelectMinChars.errorProvider1.SetIconPadding(textPatSelectMinChars,-1);
			textPatSelectPauseMs.errorProvider1.SetIconPadding(textPatSelectPauseMs,-1);
			FillStandardPrefs();
			try {
				FillHiddenPrefs();
			}
			catch {
				//Suppress unhandled exceptions from hidden preferences, since they are read only.
			}
		}

		/// <summary>Sets UI for preferences that we know for sure will exist.</summary>
		private void FillStandardPrefs() {
			#region Account Tab
			checkAgingMonthly.Checked=Preferences.GetBool(PreferenceName.AgingCalculatedMonthlyInsteadOfDaily);
			foreach(PayClinicSetting prompt in Enum.GetValues(typeof(PayClinicSetting))) {
				comboPaymentClinicSetting.Items.Add(prompt.GetDescription());
			}
			comboPaymentClinicSetting.SelectedIndex=PrefC.GetInt(PreferenceName.PaymentClinicSetting);
			checkPaymentsPromptForPayType.Checked=Preferences.GetBool(PreferenceName.PaymentsPromptForPayType);
			checkBillShowTransSinceZero.Checked=Preferences.GetBool(PreferenceName.BillingShowTransSinceBalZero);
			textClaimIdentifier.Text=Preferences.GetString(PreferenceName.ClaimIdPrefix);
			checkReceiveReportsService.Checked=Preferences.GetBool(PreferenceName.ClaimReportReceivedByService);
			_claimReportReceiveInterval=PrefC.GetInt(PreferenceName.ClaimReportReceiveInterval);
			if(_claimReportReceiveInterval==0) {
				radioTime.Checked=true;
				DateTime fullDateTime=PrefC.GetDate(PreferenceName.ClaimReportReceiveTime);
				textReportCheckTime.Text=fullDateTime.ToShortTimeString();
			}
			else {
				textReportCheckInterval.Text=POut.Int(_claimReportReceiveInterval);
				radioInterval.Checked=true;
			}
			List<RigorousAccounting> listEnums=Enum.GetValues(typeof(RigorousAccounting)).OfType<RigorousAccounting>().ToList();
			for(int i=0;i<listEnums.Count;i++) {
				comboRigorousAccounting.Items.Add(listEnums[i].GetDescription());
			}
			comboRigorousAccounting.SelectedIndex=PrefC.GetInt(PreferenceName.RigorousAccounting);
			List<RigorousAdjustments> listAdjEnums=Enum.GetValues(typeof(RigorousAdjustments)).OfType<RigorousAdjustments>().ToList();
			for(int i=0;i<listAdjEnums.Count;i++) {
				comboRigorousAdjustments.Items.Add(listAdjEnums[i].GetDescription());
			}
			comboRigorousAdjustments.SelectedIndex=PrefC.GetInt(PreferenceName.RigorousAdjustments);
			checkHidePaysplits.Checked=Preferences.GetBool(PreferenceName.PaymentWindowDefaultHideSplits);
			foreach(PayPlanVersions version in Enum.GetValues(typeof(PayPlanVersions))) {
				comboPayPlansVersion.Items.Add(version.GetDescription());
			}
			comboPayPlansVersion.SelectedIndex=PrefC.GetInt(PreferenceName.PayPlansVersion)-1;
			textBillingElectBatchMax.Text=PrefC.GetInt(PreferenceName.BillingElectBatchMax).ToString();
			checkBillingShowProgress.Checked=Preferences.GetBool(PreferenceName.BillingShowSendProgress);
			#endregion Account Tab
			#region Advanced Tab
			checkPasswordsMustBeStrong.Checked=Preferences.GetBool(PreferenceName.PasswordsMustBeStrong);
			checkPasswordsStrongIncludeSpecial.Checked=Preferences.GetBool(PreferenceName.PasswordsStrongIncludeSpecial);
			checkPasswordForceWeakToStrong.Checked=Preferences.GetBool(PreferenceName.PasswordsWeakChangeToStrong);
			checkLockIncludesAdmin.Checked=Preferences.GetBool(PreferenceName.SecurityLockIncludesAdmin);
			textLogOffAfterMinutes.Text=PrefC.GetInt(PreferenceName.SecurityLogOffAfterMinutes).ToString();
			checkUserNameManualEntry.Checked=Preferences.GetBool(PreferenceName.UserNameManualEntry);
			textDateLock.Text=PrefC.GetDate(PreferenceName.SecurityLockDate).ToShortDateString();
			textDaysLock.Text=PrefC.GetInt(PreferenceName.SecurityLockDays).ToString();
			long signalInactive=Preferences.GetLong(PreferenceName.SignalInactiveMinutes);
			textInactiveSignal.Text=(signalInactive==0 ? "" : signalInactive.ToString());
			long sigInterval=Preferences.GetLong(PreferenceName.ProcessSigsIntervalInSecs);
			textSigInterval.Text=(sigInterval==0 ? "" : sigInterval.ToString());
			string patSearchMinChars=Preferences.GetString(PreferenceName.PatientSelectSearchMinChars);
			textPatSelectMinChars.Text=Math.Min(10,Math.Max(1,PIn.Int(patSearchMinChars,false))).ToString();//enforce minimum 1 maximum 10
			string patSearchPauseMs=Preferences.GetString(PreferenceName.PatientSelectSearchPauseMs);
			textPatSelectPauseMs.Text=Math.Min(10000,Math.Max(1,PIn.Int(patSearchPauseMs,false))).ToString();//enforce minimum 1 maximum 10000
			checkPatientSelectFilterRestrictedClinics.Checked=Preferences.GetBool(PreferenceName.PatientSelectFilterRestrictedClinics);
			YN searchEmptyParams=PIn.Enum<YN>(PrefC.GetInt(PreferenceName.PatientSelectSearchWithEmptyParams));
			if(searchEmptyParams!=YN.Unknown) {
				checkPatSearchEmptyParams.CheckState=CheckState.Unchecked;
				checkPatSearchEmptyParams.Checked=searchEmptyParams==YN.Yes;
			}
			_usePhonenumTable=PrefC.GetEnum<YN>(PreferenceName.PatientPhoneUsePhonenumberTable);
			if(_usePhonenumTable!=YN.Unknown) {
				checkUsePhoneNumTable.CheckState=CheckState.Unchecked;
				checkUsePhoneNumTable.Checked=_usePhonenumTable==YN.Yes;
			}
			#endregion Advanced Tab
			#region Appts Tab
			checkApptsRequireProcs.Checked=Preferences.GetBool(PreferenceName.ApptsRequireProc);
			checkUseOpHygProv.Checked=Preferences.GetBool(PreferenceName.ApptSecondaryProviderConsiderOpOnly);
			checkEnterpriseApptList.Checked=Preferences.GetBool(PreferenceName.EnterpriseApptList);
			checkEnableNoneView.Checked=Preferences.GetBool(PreferenceName.EnterpriseNoneApptViewDefaultDisabled);
			#endregion Appts Tab
			#region Family Tab
			checkSuperFam.Checked=Preferences.GetBool(PreferenceName.ShowFeatureSuperfamilies);
			checkPatClone.Checked=Preferences.GetBool(PreferenceName.ShowFeaturePatientClone);
			checkShowFeeSchedGroups.Checked=Preferences.GetBool(PreferenceName.ShowFeeSchedGroups);
			checkSuperFamCloneCreate.Checked=Preferences.GetBool(PreferenceName.CloneCreateSuperFamily);
			//users should only see the snapshot trigger and service runtime if they have it set to something other than ClaimCreate.
			//if a user wants to be able to change claimsnapshot settings, the following MySQL statement should be run:
			//UPDATE preference SET ValueString = 'Service'	 WHERE PrefName = 'ClaimSnapshotTriggerType'
			if(PIn.Enum<ClaimSnapshotTrigger>(Preferences.GetString(PreferenceName.ClaimSnapshotTriggerType),true)==ClaimSnapshotTrigger.ClaimCreate) {
				groupClaimSnapshot.Visible=false;
			}
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				comboClaimSnapshotTrigger.Items.Add(trigger.GetDescription());
			}
			comboClaimSnapshotTrigger.SelectedIndex=(int)PIn.Enum<ClaimSnapshotTrigger>(Preferences.GetString(PreferenceName.ClaimSnapshotTriggerType),true);
			textClaimSnapshotRunTime.Text=PrefC.GetDate(PreferenceName.ClaimSnapshotRunTime).ToShortTimeString();
			#endregion Family Tab
			#region Reports Tab
			checkUseReportServer.Checked=(Preferences.GetString(PreferenceName.ReportingServerCompName)!="" || Preferences.GetString(PreferenceName.ReportingServerURI)!="");
			textServerName.Text=Preferences.GetString(PreferenceName.ReportingServerCompName);
			comboDatabase.Text=Preferences.GetString(PreferenceName.ReportingServerDbName);
			textMysqlUser.Text=Preferences.GetString(PreferenceName.ReportingServerMySqlUser);
			textMysqlPass.Text= Preferences.GetString(PreferenceName.ReportingServerMySqlPassHash);
			textMiddleTierURI.Text=Preferences.GetString(PreferenceName.ReportingServerURI);
			FillComboDatabases();
			SetReportServerUIEnabled();
			#endregion Reports Tab
		}

		///<summary>Load values from database for hidden preferences if they exist.  If a pref doesn't exist then the corresponding UI is hidden.</summary>
		private void FillHiddenPrefs() {
			FillOptionalPrefBool(checkAgingEnterprise,PreferenceName.AgingIsEnterprise);
			FillOptionalPrefBool(checkAgingShowPayplanPayments,PreferenceName.AgingReportShowAgePatPayplanPayments);
			FillOptionalPrefBool(checkClaimSnapshotEnabled,PreferenceName.ClaimSnapshotEnabled);
			FillOptionalPrefBool(checkDBMDisableOptimize,PreferenceName.DatabaseMaintenanceDisableOptimize);
			FillOptionalPrefBool(checkDBMSkipCheckTable,PreferenceName.DatabaseMaintenanceSkipCheckTable);
			validDateAgingServiceTimeDue.Text=PrefC.GetDate(PreferenceName.AgingServiceTimeDue).ToShortTimeString();
			checkEnableClinics.Checked=PrefC.HasClinicsEnabled;
			string updateStreamline=GetHiddenPrefString(PreferenceName.UpdateStreamLinePassword);
			if(updateStreamline!=null) {
				checkUpdateStreamlinePassword.Checked=(updateStreamline=="abracadabra");
			}
			else {
				checkUpdateStreamlinePassword.Visible=false;
			}
			string updateLargeTables=GetHiddenPrefString(PreferenceName.UpdateAlterLargeTablesDirectly);
			if(updateLargeTables!=null) {
				checkUpdateAlterLargeTablesDirectly.Checked=updateLargeTables=="1";
			}
			else {
				checkUpdateAlterLargeTablesDirectly.Visible=false;
			}
		}

		///<summary>Returns the ValueString of a pref or null if that pref is not found in the database.</summary>
		private string GetHiddenPrefString(string pref)
		{
			return Preferences.GetString(pref);
		}

		///<summary>Helper method for setting UI for boolean preferences.  Some of the preferences calling this may not exist in the database.</summary>
		private void FillOptionalPrefBool(CheckBox checkPref, string pref) {
			string valueString=GetHiddenPrefString(pref);
			if(valueString==null) {
				checkPref.Visible=false;
				return;
			}
			checkPref.Checked=PIn.Bool(valueString);
		}

		#region Report helper functions

		private void SetReportServerUIEnabled() {
			if(checkUseReportServer.Checked) {
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
			else {
				radioReportServerDirect.Enabled=false;
				radioReportServerMiddleTier.Enabled=false;
				groupConnectionSettings.Enabled=false;
				groupMiddleTier.Enabled=false;
			}
		}

		private void FillComboDatabases() {
			comboDatabase.Items.Clear();
			comboDatabase.Items.AddRange(GetDatabases());
		}

		///<summary>Taken from FormReportSetup.</summary>
		private string[] GetDatabases() {
			if(textServerName.Text=="") {
				return new string[0];
			}
			try {
				DataConnection dcon;
				//use the one table that we know exists
				if(textMysqlUser.Text=="") {
					dcon=new DataConnection(textServerName.Text,"root",textMysqlPass.Text, "mysql");
				}
				else {
					dcon=new DataConnection(textServerName.Text,textMysqlUser.Text,textMysqlPass.Text, "mysql");
				}
				string command="SHOW DATABASES";
				//if this next step fails, table will simply have 0 rows
				DataTable table=dcon.ExecuteDataTable(command,false);
				string[] dbNames=new string[table.Rows.Count];
				for(int i=0;i<table.Rows.Count;i++) {
					dbNames[i]=table.Rows[i][0].ToString();
				}
				return dbNames;
			}
			catch(Exception) {
				return new string[0];
			}
		}

		#endregion
		#region Update preference helpers

		private void UpdatePreferenceChanges() {
			bool hasChanges=false;
			if(Preferences.Set(PreferenceName.AgingCalculatedMonthlyInsteadOfDaily,checkAgingMonthly.Checked)
				| Preferences.Set(PreferenceName.ApptSecondaryProviderConsiderOpOnly,checkUseOpHygProv.Checked)
				| Preferences.Set(PreferenceName.ApptsRequireProc,checkApptsRequireProcs.Checked)
				| Preferences.Set(PreferenceName.BillingShowSendProgress,checkBillingShowProgress.Checked)
				| Preferences.Set(PreferenceName.BillingShowTransSinceBalZero,checkBillShowTransSinceZero.Checked)
				| Preferences.Set(PreferenceName.ClaimReportReceivedByService,checkReceiveReportsService.Checked)
				| Preferences.Set(PreferenceName.CloneCreateSuperFamily,checkSuperFamCloneCreate.Checked)
				| Preferences.Set(PreferenceName.EnterpriseApptList,checkEnterpriseApptList.Checked)
				| Preferences.Set(PreferenceName.EnterpriseNoneApptViewDefaultDisabled,checkEnableNoneView.Checked)
				| Preferences.Set(PreferenceName.PasswordsMustBeStrong,checkPasswordsMustBeStrong.Checked)
				| Preferences.Set(PreferenceName.PasswordsStrongIncludeSpecial,checkPasswordsStrongIncludeSpecial.Checked)
				| Preferences.Set(PreferenceName.PasswordsWeakChangeToStrong,checkPasswordForceWeakToStrong.Checked)
				| Preferences.Set(PreferenceName.PaymentWindowDefaultHideSplits,checkHidePaysplits.Checked)
				| Preferences.Set(PreferenceName.PaymentsPromptForPayType,checkPaymentsPromptForPayType.Checked)
				| Preferences.Set(PreferenceName.SecurityLockIncludesAdmin,checkLockIncludesAdmin.Checked)
				| Preferences.Set(PreferenceName.ShowFeaturePatientClone,checkPatClone.Checked)
				| Preferences.Set(PreferenceName.ShowFeatureSuperfamilies,checkSuperFam.Checked)
				| Preferences.Set(PreferenceName.ShowFeeSchedGroups,checkShowFeeSchedGroups.Checked)
				| Preferences.Set(PreferenceName.UserNameManualEntry,checkUserNameManualEntry.Checked)
				| Preferences.Set(PreferenceName.BillingElectBatchMax,PIn.Int(textBillingElectBatchMax.Text))
				| Preferences.Set(PreferenceName.ClaimIdPrefix,textClaimIdentifier.Text)
				| Preferences.Set(PreferenceName.ClaimReportReceiveInterval,PIn.Int(textReportCheckInterval.Text))
				| Preferences.Set(PreferenceName.ClaimReportReceiveTime,PIn.Date(textReportCheckTime.Text))
				| Preferences.Set(PreferenceName.ProcessSigsIntervalInSecs,PIn.Long(textSigInterval.Text))
				//SecurityLockDate and SecurityLockDays are handled in FormSecurityLock
				//| Prefs.UpdateString(PrefName.SecurityLockDate,POut.Date(PIn.Date(textDateLock.Text),false))
				//| Prefs.UpdateInt(PrefName.SecurityLockDays,PIn.Int(textDaysLock.Text))
				| Preferences.Set(PreferenceName.SecurityLogOffAfterMinutes,PIn.Int(textLogOffAfterMinutes.Text))
				| Preferences.Set(PreferenceName.SignalInactiveMinutes,PIn.Long(textInactiveSignal.Text))
				| Preferences.Set(PreferenceName.PayPlansVersion,comboPayPlansVersion.SelectedIndex+1)
				| Preferences.Set(PreferenceName.PaymentClinicSetting,comboPaymentClinicSetting.SelectedIndex)
				| Preferences.Set(PreferenceName.PatientSelectSearchMinChars,PIn.Int(textPatSelectMinChars.Text))
				| Preferences.Set(PreferenceName.PatientSelectSearchPauseMs,PIn.Int(textPatSelectPauseMs.Text))
				| Preferences.Set(PreferenceName.PatientSelectFilterRestrictedClinics,checkPatientSelectFilterRestrictedClinics.Checked)
			)
			{
				hasChanges=true;
			}
			if(checkPatSearchEmptyParams.CheckState!=CheckState.Indeterminate) {
				hasChanges|=Preferences.Set(PreferenceName.PatientSelectSearchWithEmptyParams,checkPatSearchEmptyParams.Checked);
			}
			if(checkUsePhoneNumTable.CheckState!=CheckState.Indeterminate) {
				hasChanges|=Preferences.Set(PreferenceName.PatientPhoneUsePhonenumberTable,checkUsePhoneNumTable.Checked);
			}
			int prefRigorousAccounting=PrefC.GetInt(PreferenceName.RigorousAccounting);
			//Copied logging for RigorousAccounting and RigorousAdjustments from FormModuleSetup.
			if(Preferences.Set(PreferenceName.RigorousAccounting,comboRigorousAccounting.SelectedIndex)) {
				hasChanges=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rigorous accounting changed from "+
					((RigorousAccounting)prefRigorousAccounting).GetDescription()+" to "
					+((RigorousAccounting)comboRigorousAccounting.SelectedIndex).GetDescription()+".");
			}
			int prefRigorousAdjustments=PrefC.GetInt(PreferenceName.RigorousAdjustments);
			if(Preferences.Set(PreferenceName.RigorousAdjustments,comboRigorousAdjustments.SelectedIndex)) {
				hasChanges=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rigorous adjustments changed from "+
					((RigorousAdjustments)prefRigorousAdjustments).GetDescription()+" to "
					+((RigorousAdjustments)comboRigorousAdjustments.SelectedIndex).GetDescription()+".");
			}
			hasChanges|=UpdateReportingServer();
			hasChanges|=UpdateClaimSnapshotRuntime();
			hasChanges|=UpdateClaimSnapshotTrigger();
			if(hasChanges) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

		///<summary>Copied from FormReportSetup.</summary>
		private bool UpdateReportingServer() {
			bool changed=false;
			if(!checkUseReportServer.Checked) {
				if(Preferences.Set(PreferenceName.ReportingServerCompName,"")
					| Preferences.Set(PreferenceName.ReportingServerDbName,"")
					| Preferences.Set(PreferenceName.ReportingServerMySqlUser,"")
					| Preferences.Set(PreferenceName.ReportingServerMySqlPassHash,"")
					| Preferences.Set(PreferenceName.ReportingServerURI,"")
				)
				{
					changed=true;
				}
			}
			else {
				if(radioReportServerDirect.Checked) {
					if(Preferences.Set(PreferenceName.ReportingServerCompName,textServerName.Text)
						| Preferences.Set(PreferenceName.ReportingServerDbName,comboDatabase.Text)
						| Preferences.Set(PreferenceName.ReportingServerMySqlUser,textMysqlUser.Text)
						| Preferences.Set(PreferenceName.ReportingServerMySqlPassHash, textMysqlPass.Text)
						| Preferences.Set(PreferenceName.ReportingServerURI,"")
					)
					{
						changed=true;
					}
				}
				else {
					if(Preferences.Set(PreferenceName.ReportingServerCompName,"")
						|Preferences.Set(PreferenceName.ReportingServerDbName,"")
						|Preferences.Set(PreferenceName.ReportingServerMySqlUser,"")
						|Preferences.Set(PreferenceName.ReportingServerMySqlPassHash,"")
						|Preferences.Set(PreferenceName.ReportingServerURI,textMiddleTierURI.Text)
					)
					{
						changed=true;
					}
				}
			}
			return changed;
		}

		private bool UpdateClaimSnapshotRuntime() {
			DateTime claimSnapshotRunTime=DateTime.MinValue;
			DateTime.TryParse(textClaimSnapshotRunTime.Text,out claimSnapshotRunTime);//This already gets checked in the validate method.
			claimSnapshotRunTime=new DateTime(1881,01,01,claimSnapshotRunTime.Hour,claimSnapshotRunTime.Minute,claimSnapshotRunTime.Second);
			return Preferences.Set(PreferenceName.ClaimSnapshotRunTime,claimSnapshotRunTime);
		}

		private bool UpdateClaimSnapshotTrigger() {
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				if(trigger.GetDescription()==comboClaimSnapshotTrigger.Text) {
					return Preferences.Set(PreferenceName.ClaimSnapshotTriggerType,trigger.ToString());
				}
			}
			return false;
		}

		///<summary>Checks preferences that take user entry for errors, returns true if all entries are valid.</summary>
		private bool ValidateEntries() {
			string errorMsg="";
			//SecurityLogOffAfterMinutes
			if(textLogOffAfterMinutes.Text!="") {
				try {
					int logOffMinutes = Int32.Parse(textLogOffAfterMinutes.Text);
					if(logOffMinutes<0) {//Automatic log off must be a positive numerical value.
						throw new Exception();
					}
				}
				catch {
					errorMsg+="Log off after minutes is invalid. Must be a positive number.\r\n";
				}
			}
			//ClaimReportReceiveInterval
			int reportCheckIntervalMinuteCount=0;
			reportCheckIntervalMinuteCount=PIn.Int(textReportCheckInterval.Text,false);
			if(textReportCheckInterval.Enabled && (reportCheckIntervalMinuteCount<5 || reportCheckIntervalMinuteCount>60)) {
				errorMsg+="Report check interval must be between 5 and 60 inclusive.\r\n";
			}
			//ClaimReportReceiveTime
			if(radioTime.Checked && (textReportCheckTime.Text=="" || !textReportCheckTime.IsValid)) {
				errorMsg+="Please enter a time to receive reports.";
			}
			//ClaimSnapshotRuntime
			if(!DateTime.TryParse(textClaimSnapshotRunTime.Text,out DateTime claimSnapshotRunTime)) {
				errorMsg+="Service Snapshot Run Time must be a valid time value.\r\n";
			}
			//ProcessSigsIntervalInSecs
			if(!textSigInterval.IsValid) {
				errorMsg+="Signal interval must be a valid number or blank.\r\n";
			}
			//SignalInactiveMinutes
			if(!textInactiveSignal.IsValid) {
				errorMsg+="Disable signal interval must be a valid number or blank.\r\n";
			}
			//BillingElectBatchMax
			if(!textBillingElectBatchMax.IsValid) {
				errorMsg+="The maximum number of statements per batch must be a valid number or blank.\r\n";
			}
			//PatientSelectSearchMinChars
			if(!textPatSelectMinChars.IsValid) {
				errorMsg+="The patient select number of characters before filling the grid must be a valid number.\r\n";
			}
			//PatientSelectSearchPauseMs
			if(!textPatSelectPauseMs.IsValid) {
				errorMsg+="The patient select number of milliseconds to wait before filling the grid must be a valid number.\r\n";
			}
			if(errorMsg!="") {
				MessageBox.Show("Please fix the following errors:\r\n"+errorMsg);
				return false;
			}
			if(_usePhonenumTable!=YN.Yes && checkUsePhoneNumTable.CheckState==CheckState.Checked) {//use CheckState since it can be indeterminate
				string msgText="When enabling the use of the phonenumber table a one-time sync of patient phone numbers needs to take place.  This could "+
					"take a couple minutes.  Continue?";
				if(!MsgBox.Show(MsgBoxButtons.OKCancel,msgText)) {
					return false;
				}
				if(!SyncPhoneNums()) {
					return false;
				}
				MessageBox.Show("Done");
			}
			return true;

		}

		private bool SyncPhoneNums() {
			string syncError=null;
			Cursor=Cursors.WaitCursor;
			ODProgress.ShowAction(
				() => PhoneNumbers.SyncAllPats(),
				startingMessage: "Syncing all patient phone numbers to the phonenumber table"+"...",
				actionException: ex => syncError="The patient phone number sync failed with the message"+":\r\n"+ex.Message+"\r\n"+"Please try again."
			);
			Cursor=Cursors.Default;
			if(!string.IsNullOrEmpty(syncError)) {
				MessageBox.Show(this,syncError);
				return false;
			}
			_usePhonenumTable=YN.Yes;//so it won't sync again if you clicked the button
			return true;
		}

		#endregion

		private void checkUseReportServer_CheckedChanged(object sender,EventArgs e) {
			SetReportServerUIEnabled();
		}

		private void RadioReportServerDirect_CheckedChanged(object sender,EventArgs e) {
			SetReportServerUIEnabled();
		}

		private void ComboDatabase_DropDown(object sender,EventArgs e) {
			FillComboDatabases();
		}

		private void radioInterval_CheckedChanged(object sender,EventArgs e) {
			//Copied from FormClearingHouses
			if(radioInterval.Checked) {
				labelReportheckUnits.Enabled=true;
				textReportCheckInterval.Enabled=true;
				textReportCheckTime.Text="";
				textReportCheckTime.Enabled=false;
				textReportCheckTime.ClearError();
			}
			else {
				labelReportheckUnits.Enabled=false;
				textReportCheckInterval.Text="";
				textReportCheckInterval.Enabled=false;
				textReportCheckTime.Enabled=true;
			}
		}

		private void butReplacements_Click(object sender,EventArgs e) {
			//Copied from FormModuleSetup.
			FormMessageReplacements form=new FormMessageReplacements(MessageReplaceType.Patient);
			form.IsSelectionMode=true;
			form.ShowDialog();
			if(form.DialogResult!=DialogResult.OK) {
				return;
			}
			textClaimIdentifier.Focus();
			int cursorIndex=textClaimIdentifier.SelectionStart;
			textClaimIdentifier.Text=textClaimIdentifier.Text.Insert(cursorIndex,form.Replacement);
			textClaimIdentifier.SelectionStart=cursorIndex+form.Replacement.Length;
		}

		private void butChange_Click(object sender,EventArgs e) {
			//Copied from FormGlobalSecurity.
			FormSecurityLock FormS=new FormSecurityLock();
			FormS.ShowDialog();//prefs are set invalid within that form if needed.
			if(PrefC.GetInt(PreferenceName.SecurityLockDays)>0) {
				textDaysLock.Text=PrefC.GetInt(PreferenceName.SecurityLockDays).ToString();
			}
			else {
				textDaysLock.Text="";
			}
			if(PrefC.GetDate(PreferenceName.SecurityLockDate).Year>1880) {
				textDateLock.Text=PrefC.GetDate(PreferenceName.SecurityLockDate).ToShortDateString();
			}
			else {
				textDateLock.Text="";
			}
			checkLockIncludesAdmin.Checked=Preferences.GetBool(PreferenceName.SecurityLockIncludesAdmin);
		}

		private void butSyncPhNums_Click(object sender,EventArgs e) {
			if(SyncPhoneNums()) {
				MessageBox.Show("Done");
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!ValidateEntries()) {
				return;
			}
			UpdatePreferenceChanges();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}