using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using System.Collections.Generic;
using System.DirectoryServices;
using CodeBase;
using Imedisoft.Forms;
using Imedisoft.Data;
using Imedisoft.Data.Models;

namespace OpenDental{
	///<summary></summary>
	public partial class FormUserEdit : ODForm {
		///<summary></summary>
		public bool IsNew;
		///<summary></summary>
		public Userod UserCur;
		private List<AlertSub> _listUserAlertTypesOld;
		private List<UserGroup> _listUserGroups;
		private List<Clinic> _listClinics;
		///<summary>The password that was entered in FormUserPassword.</summary>
		private string _passwordTyped;
		///<summary>The alert categories that are available to be selected. Some alert types will not be displayed if this is not OD HQ.</summary>
		private List<AlertCategory> _listAlertCategories;
		///<summary>The UserOdPref for DoseSpot User ID.</summary>
		//private UserOdPref _doseSpotUserPrefDefault;
		private List<Employee> _listEmployees;
		private List<Provider> _listProviders;
		private bool _isFromAddUser;
		//private List<UserOdPref> _listDoseSpotUserPrefOld;
		//private List<UserOdPref> _listDoseSpotUserPrefNew;
		private bool _isFillingList;
		//private UserOdPref _logOffAfterMinutes;
		private int _logOffAfterMinutesInitialValue;

		///<summary></summary>
		public FormUserEdit(Userod userCur,bool isFromAddUser=false) {
			InitializeComponent();
			
			UserCur=userCur.Copy();
			_isFromAddUser=isFromAddUser;
		}

		private void FormUserEdit_Load(object sender, EventArgs e) {
			var _logOffAfterMinutes=UserPreference.GetInt(UserCur.Id,UserPreferenceName.LogOffTimerOverride);
			_logOffAfterMinutesInitialValue=_logOffAfterMinutes;
			textLogOffAfterMinutes.Text=_logOffAfterMinutesInitialValue.ToString();
			checkIsHidden.Checked=UserCur.IsHidden;
			if(UserCur.Id!=0) {
				textUserNum.Text=UserCur.Id.ToString();
			}
			textUserName.Text=UserCur.UserName;
			textDomainUser.Text=UserCur.DomainUser;
			if(!Prefs.GetBool(PrefName.DomainLoginEnabled)) {
				labelDomainUser.Visible=false;
				textDomainUser.Visible=false;
				butPickDomainUser.Visible=false;
			}
			checkRequireReset.Checked=UserCur.IsPasswordResetRequired;
			_listUserGroups=UserGroups.GetList();
			_isFillingList=true;
			for(int i=0;i<_listUserGroups.Count;i++){
				listUserGroup.Items.Add(new ODBoxItem<UserGroup>(_listUserGroups[i].Description,_listUserGroups[i]));
				if(!_isFromAddUser && UserCur.IsInUserGroup(_listUserGroups[i].Id)) {
					listUserGroup.SetSelected(i,true);
				}
				if(_isFromAddUser && _listUserGroups[i].Id==Prefs.GetLong(PrefName.DefaultUserGroup)) {
					listUserGroup.SetSelected(i,true);
				}
			}
			if(listUserGroup.SelectedIndex==-1){//never allowed to delete last group, so this won't fail
				listUserGroup.SelectedIndex=0;
			}
			_isFillingList=false;
			securityTreeUser.FillTreePermissionsInitial();
			RefreshUserTree();
			listEmployee.Items.Clear();
			listEmployee.Items.Add("none");
			listEmployee.SelectedIndex=0;
			_listEmployees=Employees.GetAll(true);
			for(int i=0;i<_listEmployees.Count;i++){
				listEmployee.Items.Add(Employees.GetNameFL(_listEmployees[i]));
				if(UserCur.EmployeeId==_listEmployees[i].Id) {
					listEmployee.SelectedIndex=i+1;
				}
			}
			listProv.Items.Clear();
			listProv.Items.Add("none");
			listProv.SelectedIndex=0;
			_listProviders=Providers.GetDeepCopy(true);
			for(int i=0;i<_listProviders.Count;i++) {
				listProv.Items.Add(_listProviders[i].GetLongDesc());
				if(UserCur.ProviderId==_listProviders[i].Id) {
					listProv.SelectedIndex=i+1;
				}
			}
			_listClinics=Clinics.GetAll(false);
			_listUserAlertTypesOld=AlertSubs.GetAllForUser(UserCur.Id);
			List<long> listSubscribedClinics;
			bool isAllClinicsSubscribed=false;
			if(_listUserAlertTypesOld.Select(x => x.ClinicId).Contains(-1)) {//User subscribed to all clinics
				isAllClinicsSubscribed=true;
				listSubscribedClinics=_listClinics.Select(x => x.Id).Distinct().ToList();
			}
			else {
				listSubscribedClinics=_listUserAlertTypesOld.Select(x => x.ClinicId.Value).Distinct().ToList();
			}
			List<long> listAlertCatNums=_listUserAlertTypesOld.Select(x => x.AlertCategoryId).Distinct().ToList();
			listAlertSubMulti.Items.Clear();
			_listAlertCategories=AlertCategories.GetAll();
			List<long> listUserAlertCatNums=_listUserAlertTypesOld.Select(x => x.AlertCategoryId).ToList();
			foreach(AlertCategory cat in _listAlertCategories) {
				int index=listAlertSubMulti.Items.Add(cat.Description);
				listAlertSubMulti.SetSelected(index,listUserAlertCatNums.Contains(cat.Id));
			}
			if(!PrefC.HasClinicsEnabled) {
				tabClinics.Enabled=false;//Disables all controls in the clinics tab.  Tab is still selectable.
				listAlertSubsClinicsMulti.Visible=false;
				labelAlertClinic.Visible=false;
			}
			else {
				listClinic.Items.Clear();
				listClinic.Items.Add("All");
				listAlertSubsClinicsMulti.Items.Add("All");
				listAlertSubsClinicsMulti.Items.Add("Headquarters");
				if(UserCur.ClinicId==0) {//Unrestricted
					listClinic.SetSelected(0,true);
					checkClinicIsRestricted.Enabled=false;//We don't really need this checkbox any more but it's probably better for users to keep it....
				}
				if(isAllClinicsSubscribed) {//They are subscribed to all clinics
					listAlertSubsClinicsMulti.SetSelected(0,true);
				}
				else if(listSubscribedClinics.Contains(0)) {//They are subscribed to Headquarters
					listAlertSubsClinicsMulti.SetSelected(1,true);
				}
				List<UserClinic> listUserClinics=UserClinics.GetForUser(UserCur.Id);
				for(int i=0;i<_listClinics.Count;i++) {
					listClinic.Items.Add(_listClinics[i].Abbr);
					listClinicMulti.Items.Add(_listClinics[i].Abbr);
					listAlertSubsClinicsMulti.Items.Add(_listClinics[i].Abbr);
					if(UserCur.ClinicId==_listClinics[i].Id) {
						listClinic.SetSelected(i+1,true);
					}
					if(UserCur.ClinicId!=0 && listUserClinics.Exists(x => x.ClinicId==_listClinics[i].Id)) {
						listClinicMulti.SetSelected(i,true);//No "All" option, don't select i+1
					}
					if(!isAllClinicsSubscribed && _listUserAlertTypesOld.Exists(x => x.ClinicId==_listClinics[i].Id)) {
						listAlertSubsClinicsMulti.SetSelected(i+2,true);//All+HQ
					}
				}
				checkClinicIsRestricted.Checked=UserCur.ClinicIsRestricted;
			}
			if(string.IsNullOrEmpty(UserCur.PasswordHash)){
				butPassword.Text="Create Password";
			}

			if(IsNew) {
				butUnlock.Visible=false;
			}
			// TODO:
			//_listDoseSpotUserPrefOld=UserOdPrefs.GetByUserAndFkeyAndFkeyType(UserCur.Id,
			//	Programs.GetCur(ProgramName.eRx).Id,UserOdFkeyType.Program,
			//	Clinics.GetForUserod(Security.CurrentUser,true).Select(x => x.ClinicNum)
			//	.Union(new List<long>() { 0 })//Always include 0 clinic, this is the default, NOT a headquarters only value.
			//	.Distinct()
			//	.ToList());
			//_listDoseSpotUserPrefNew=_listDoseSpotUserPrefOld.Select(x => x.Clone()).ToList();
			//_doseSpotUserPrefDefault=_listDoseSpotUserPrefNew.Find(x => x.ClinicNum==0);
			//if(_doseSpotUserPrefDefault==null) {
			//	_doseSpotUserPrefDefault=DoseSpot.GetDoseSpotUserIdFromPref(UserCur.Id,0);
			//	_listDoseSpotUserPrefNew.Add(_doseSpotUserPrefDefault);
			//}
			//textDoseSpotUserID.Text=_doseSpotUserPrefDefault.ValueString;
			//if(_isFromAddUser && !Security.IsAuthorized(Permissions.SecurityAdmin,true)) {
			//	butPassword.Visible=false;
			//	checkRequireReset.Checked=true;
			//	checkRequireReset.Enabled=false;
			//	butUnlock.Visible=false;
			//}
			//if(!PrefC.HasClinicsEnabled) {
			//	butDoseSpotAdditional.Visible=false;
			//}
		}

		///<summary>Refreshes the security tree in the "Users" tab.</summary>
		private void RefreshUserTree() {
			securityTreeUser.FillForUserGroup(listUserGroup.SelectedItems.OfType<ODBoxItem<UserGroup>>().Select(x => x.Tag.Id).ToList());
		}

		private void listUserGroup_SelectedIndexChanged(object sender,EventArgs e) {
			if(_isFillingList) {
				return;
			}
			RefreshUserTree();
		}

		private void butPickDomainUser_Click(object sender,EventArgs e) {
			//DirectoryEntry does recognize an empty string as a valid LDAP entry and will just return all logins from all available domains
			//But all logins should be on the same domain, so this field is required
			if(string.IsNullOrWhiteSpace(Prefs.GetString(PrefName.DomainLoginPath))) {
				MessageBox.Show("DomainLoginPath is missing in security settings. DomainLoginPath is required before assigning domain logins to user accounts.");
				return;
			}
			//Try to access the specified DomainLoginPath
			try {
				DirectoryEntry.Exists(Prefs.GetString(PrefName.DomainLoginPath));
			}
			catch(Exception ex) {
				MessageBox.Show("An error occurred while attempting to access the provided DomainLoginPath:"+" "+ex.Message);
				return;
			}
			FormDomainUserPick FormDU=new FormDomainUserPick();
			FormDU.ShowDialog();
			if(FormDU.DialogResult==DialogResult.OK && FormDU.SelectedDomainName!=null) { //only check for null, as empty string should clear the field
				UserCur.DomainUser=FormDU.SelectedDomainName;
				textDomainUser.Text=UserCur.DomainUser;
			}
		}

		private void listClinic_MouseClick(object sender,MouseEventArgs e) {
			int idx=listClinic.IndexFromPoint(e.Location);
			if(idx==-1){
				return;
			}
			if(idx==0){//all
				checkClinicIsRestricted.Checked=false;
				checkClinicIsRestricted.Enabled=false;
			}
			else{
				checkClinicIsRestricted.Enabled=true;
			}
		}

		private void butPassword_Click(object sender, System.EventArgs e) {
			bool isCreate=string.IsNullOrEmpty(UserCur.PasswordHash);
			FormUserPassword FormU=new FormUserPassword(UserCur.UserName, isCreate: isCreate, inSecurityWindow: true);
			FormU.ShowDialog();
			if(FormU.DialogResult==DialogResult.Cancel){
				return;
			}
			UserCur.PasswordHash=FormU.PasswordHash;
			UserCur.PasswordIsStrong=FormU.PasswordIsStrong;
			if(string.IsNullOrEmpty(UserCur.PasswordHash)) {
				butPassword.Text="Create Password";
			}
			else{
				butPassword.Text="Change Password";
			}
		}

		private void butUnlock_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Users can become locked when invalid credentials have been entered several times in a row.\r\n"
				+"Unlock this user so that more log in attempts can be made?"))
			{
				return;
			}
			UserCur.FailedLoginDateTime=DateTime.MinValue;
			UserCur.FailedAttempts=0;
			try {
				Userods.Update(UserCur);
				MessageBox.Show("User has been unlocked.");
			}
			catch(Exception) {
				MessageBox.Show("There was a problem unlocking this user.  Please call support or wait the allotted lock time.");
			}
		}

		private void butDoseSpotAdditional_Click(object sender,EventArgs e) {
			//_doseSpotUserPrefDefault.ValueString=textDoseSpotUserID.Text;
			//FormUserPrefAdditional FormUP=new FormUserPrefAdditional(_listDoseSpotUserPrefNew,UserCur);
			//FormUP.ShowDialog();
			//if(FormUP.DialogResult==DialogResult.OK) {
			//	_listDoseSpotUserPrefNew=FormUP.ListUserPrefOut;
			//	_doseSpotUserPrefDefault=_listDoseSpotUserPrefNew.Find(x => x.ClinicNum==0);
			//	textDoseSpotUserID.Text=_doseSpotUserPrefDefault.ValueString;
			//}
		}

		private bool IsValidLogOffMinutes() {
			if(!(textLogOffAfterMinutes.Text=="") && (!int.TryParse(textLogOffAfterMinutes.Text,out int minutes) || minutes<0)) {
				MessageBox.Show("Invalid 'Automatic logoff time in minutes'.\r\n" +
					"Must be blank, 0, or a positive integer.");
				return false;
			}
			return true;
		}

		private void SaveLogOffPreferences()
		{
			var logOffTimer = textLogOffAfterMinutes.Text.Trim();
			if (logOffTimer.Length == 0 || !int.TryParse(logOffTimer, out var logOffTimerOverride))
			{
				UserPreference.Delete(UserPreferenceName.LogOffTimerOverride);
			}
			else if (logOffTimerOverride != _logOffAfterMinutesInitialValue)
			{
				UserPreference.Set(UserPreferenceName.LogOffTimerOverride, logOffTimerOverride);

				if (!Prefs.GetBool(PrefName.SecurityLogOffAllowUserOverride))
				{
					ODMessageBox.Show("User logoff overrides will not take effect until the Global Security setting \"Allow user override for automatic logoff\" is checked");
				}
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textUserName.Text==""){
				MessageBox.Show("Please enter a username.");
				return;
			}
			if(!_isFromAddUser && IsNew && Prefs.GetBool(PrefName.PasswordsMustBeStrong) && string.IsNullOrWhiteSpace(_passwordTyped)) {
				MessageBox.Show("Password may not be blank when the strong password feature is turned on.");
				return;
			}
			if(PrefC.HasClinicsEnabled && listClinic.SelectedIndex==-1) {
				MessageBox.Show("This user does not have a User Default Clinic set.  Please choose one to continue.");
				return;
			}
			if(listUserGroup.SelectedIndices.Count == 0) {
				MessageBox.Show("Users must have at least one user group associated. Please select a user group to continue.");
				return;
			}
			if(_isFromAddUser && !Security.IsAuthorized(Permissions.SecurityAdmin,true)
				&& (listUserGroup.SelectedItems.Count!=1 || listUserGroup.GetSelected<UserGroup>().Id!=Prefs.GetLong(PrefName.DefaultUserGroup)))
			{
				MessageBox.Show("This user must be assigned to the default user group.");
				for(int i=0;i<listUserGroup.Items.Count;i++) {
					if(((ODBoxItem<UserGroup>)listUserGroup.Items[i]).Tag.Id==Prefs.GetLong(PrefName.DefaultUserGroup)) {
						listUserGroup.SetSelected(i,true);
					}
					else {
						listUserGroup.SetSelected(i,false);
					}
				}
				return;
			}
			if(!IsValidLogOffMinutes()) {
				return;
			}
			List<UserClinic> listUserClinics=new List<UserClinic>();
			if(PrefC.HasClinicsEnabled && checkClinicIsRestricted.Checked) {//They want to restrict the user to certain clinics or clinics are enabled.  
				for(int i=0;i<listClinicMulti.SelectedIndices.Count;i++) {
					listUserClinics.Add(new UserClinic(_listClinics[listClinicMulti.SelectedIndices[i]].Id,UserCur.Id));
				}
				//If they set the user up with a default clinic and it's not in the restricted list, return.
				if(!listUserClinics.Exists(x => x.ClinicId==_listClinics[listClinic.SelectedIndex-1].Id)) {
					MessageBox.Show("User cannot have a default clinic that they are not restricted to.");
					return;
				}
			}
			if(!PrefC.HasClinicsEnabled || listClinic.SelectedIndex==0) {
				UserCur.ClinicId=0;
			}
			else {
				UserCur.ClinicId=_listClinics[listClinic.SelectedIndex-1].Id;
			}
			UserCur.ClinicIsRestricted=checkClinicIsRestricted.Checked;//This is kept in sync with their choice of "All".
			UserCur.IsHidden=checkIsHidden.Checked;
			UserCur.IsPasswordResetRequired=checkRequireReset.Checked;
			UserCur.UserName=textUserName.Text;
			if(listEmployee.SelectedIndex==0){
				UserCur.EmployeeId=0;
			}
			else{
				UserCur.EmployeeId=_listEmployees[listEmployee.SelectedIndex-1].Id;
			}
			if(listProv.SelectedIndex==0) {
				Provider prov=Providers.GetById(UserCur.ProviderId);
				if(prov!=null) {
					prov.IsInstructor=false;//If there are more than 1 users associated to this provider, they will no longer be an instructor.
					Providers.Update(prov);	
				}
				UserCur.ProviderId=0;
			}
			else {
				Provider prov=Providers.GetById(UserCur.ProviderId);
				if(prov!=null) {
					if(prov.Id!=_listProviders[listProv.SelectedIndex-1].Id) {
						prov.IsInstructor=false;//If there are more than 1 users associated to this provider, they will no longer be an instructor.
					}
					Providers.Update(prov);
				}
				UserCur.ProviderId=_listProviders[listProv.SelectedIndex-1].Id;
			}
			try{
				if(IsNew){
					Userods.Insert(UserCur,listUserGroup.SelectedItems.OfType<ODBoxItem<UserGroup>>().Select(x => x.Tag.Id).ToList());
					//Set the userodprefs to the new user's UserNum that was just retreived from the database.
					//_listDoseSpotUserPrefNew.ForEach(x => x.UserNum=UserCur.Id);
					listUserClinics.ForEach(x => x.UserId=UserCur.Id);//Set the user clinic's UserNum to the one we just inserted.
					SecurityLogs.MakeLogEntry(Permissions.AddNewUser,0,"New user '"+UserCur.UserName+"' added");
				}
				else{
					List<UserGroup> listNewUserGroups=listUserGroup.SelectedItems.OfType<ODBoxItem<UserGroup>>().Select(x => x.Tag).ToList();
					List<UserGroup> listOldUserGroups=UserCur.GetGroups();
					Userods.Update(UserCur,listNewUserGroups.Select(x => x.Id).ToList());
					//if this is the current user, update the user, credentials, etc.
					if(UserCur.Id==Security.CurrentUser.Id) {
						Security.CurrentUser=UserCur.Copy();
					}
					//Log changes to the User's UserGroups.
					Func<List<UserGroup>,List<UserGroup>,List<UserGroup>> funcGetMissing=(listCur,listCompare) => {
						List<UserGroup> retVal=new List<UserGroup>();
						foreach(UserGroup group in listCur) {
							if(listCompare.Exists(x => x.Id==group.Id)) {
								continue;
							}
							retVal.Add(group);
						}
						return retVal;
					};
					List<UserGroup> listRemovedGroups=funcGetMissing(listOldUserGroups,listNewUserGroups);
					List<UserGroup> listAddedGroups=funcGetMissing(listNewUserGroups,listOldUserGroups);
					if(listRemovedGroups.Count>0) {//Only log if there are items in the list
						SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin,0,"User "+UserCur.UserName+
							" removed from User group(s): "+string.Join(", ",listRemovedGroups.Select(x => x.Description).ToArray())+" by: "+Security.CurrentUser.UserName);
					}
					if(listAddedGroups.Count>0) {//Only log if there are items in the list.
						SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin,0,"User "+UserCur.UserName+
							" added to User group(s): "+string.Join(", ",listAddedGroups.Select(x => x.Description).ToArray())+" by: "+Security.CurrentUser.UserName);
					}
				}
				if(UserClinics.Sync(listUserClinics,UserCur.Id)) {//Either syncs new list, or clears old list if no longer restricted.
					DataValid.SetInvalid(InvalidType.UserClinics);
				}
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
				return;
			}
			//DoseSpot User ID Insert/Update/Delete
			// TODO: if(_doseSpotUserPrefDefault.ValueString!=textDoseSpotUserID.Text) {
			//	if(string.IsNullOrWhiteSpace(textDoseSpotUserID.Text)) {
			//		UserOdPrefs.Delete(_doseSpotUserPrefDefault.UserNum,_doseSpotUserPrefDefault.Fkey,UserOdFkeyType.Program);
			//	}
			//	else {
			//		_doseSpotUserPrefDefault.ValueString=textDoseSpotUserID.Text.Trim();
			//		UserOdPrefs.Upsert(_doseSpotUserPrefDefault);
			//	}
			//}
			DataValid.SetInvalid(InvalidType.Security);
			//List of AlertTypes that are selected.
			List<long> listUserAlertCats=new List<long>();
			foreach(int index in listAlertSubMulti.SelectedIndices) {
				listUserAlertCats.Add(_listAlertCategories[index].Id);
			}
			List<long> listClinics=new List<long>();
			foreach(int index in listAlertSubsClinicsMulti.SelectedIndices) {
				if(index==0) {//All
					listClinics.Add(-1);//Add All
					break;
				}
				if(index==1) {//HQ
					listClinics.Add(0);
					continue;
				}
				Clinic clinic=_listClinics[index-2];//Subtract 2 for 'All' and 'HQ'
				listClinics.Add(clinic.Id);
			}
			List<AlertSub> _listUserAlertTypesNew=_listUserAlertTypesOld.Select(x => x.Copy()).ToList();
			//Remove AlertTypes that have been deselected through either deslecting the type or clinic.
			_listUserAlertTypesNew.RemoveAll(x => !listUserAlertCats.Contains(x.AlertCategoryId));
			if(PrefC.HasClinicsEnabled) {
				_listUserAlertTypesNew.RemoveAll(x => !listClinics.Contains(x.ClinicId.Value));
			}
			foreach(long alertCatNum in listUserAlertCats) {
				if(!PrefC.HasClinicsEnabled) {
					if(!_listUserAlertTypesOld.Exists(x => x.AlertCategoryId==alertCatNum)) {//Was not subscribed to type.
						_listUserAlertTypesNew.Add(new AlertSub(UserCur.Id,0,alertCatNum));
					}
				}
				else {//Clinics enabled.
					foreach(long clinicNumCur in listClinics) {
						if(!_listUserAlertTypesOld.Exists(x => x.ClinicId==clinicNumCur && x.AlertCategoryId==alertCatNum)) {//Was not subscribed to type.
							_listUserAlertTypesNew.Add(new AlertSub(UserCur.Id,clinicNumCur,alertCatNum));
							continue;
						}
					}
				}
			}
			SaveLogOffPreferences();
			// TODO: AlertSubs.Sync(_listUserAlertTypesNew,_listUserAlertTypesOld);
			// TODO: UserOdPrefs.Sync(_listDoseSpotUserPrefNew,_listDoseSpotUserPrefOld);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}