using OpenDentBusiness;
using System;
using System.Linq;
using System.Windows.Forms;
using CodeBase;

namespace OpenDental {
	/// <summary>
	/// This form (per Nathan) should be used for any future features that could be categorized as a user setting. The intent of this class was to
	/// create a place for specific user settings.
	/// </summary>
	public partial class FormUserSetting:ODForm {
		private UserOdPref _suppressLogOffMessage;
		private UserOdPref _userODPrefTheme=null;

		public FormUserSetting() {
			InitializeComponent();
			Lan.F(this);
		}
		private void FormUserSetting_Load(object sender,EventArgs e) {
			//Logoff After Minutes
			UserOdPref logOffAfterMinutes=UserOdPrefs.GetByUserAndFkeyType(Security.CurrentUser.Id,UserOdFkeyType.LogOffTimerOverride).FirstOrDefault();
			textLogOffAfterMinutes.Text=(logOffAfterMinutes==null) ? "" : logOffAfterMinutes.ValueString;
			//Suppress Logoff Message
			_suppressLogOffMessage=UserOdPrefs.GetByUserAndFkeyType(Security.CurrentUser.Id,UserOdFkeyType.SuppressLogOffMessage).FirstOrDefault();
			if(_suppressLogOffMessage!=null) {//Does exist in the database
				checkSuppressMessage.Checked=true;
			}
			//Theme Combo
			FillThemeCombo();
		}

		private void FillThemeCombo() {
			_userODPrefTheme=UserOdPrefs.GetByUserAndFkeyType(Security.CurrentUser.Id,UserOdFkeyType.UserTheme).FirstOrDefault();
			if(_userODPrefTheme!=null) {//user has chosen a theme before. Display their currently chosen theme.
				checkAlternateIcons.Checked=PIn.Bool(_userODPrefTheme.Fkey.ToString());
			}
			else {//user has not chosen a theme before. Show them the current default.
				checkAlternateIcons.Checked=PrefC.GetBool(PrefName.ColorTheme);
			}
		}

		private void SavePreferences() {
			#region Suppress Logoff Message
			if(checkSuppressMessage.Checked && _suppressLogOffMessage==null) {
				UserOdPrefs.Insert(new UserOdPref() {
					UserNum=Security.CurrentUser.Id,
					FkeyType=UserOdFkeyType.SuppressLogOffMessage
				});
			}
			else if(!checkSuppressMessage.Checked && _suppressLogOffMessage!=null) {
				UserOdPrefs.Delete(_suppressLogOffMessage.UserOdPrefNum);
			}
			#endregion
			#region Theme Change
			if(_userODPrefTheme==null) {
				_userODPrefTheme=new UserOdPref() {UserNum=Security.CurrentUser.Id,FkeyType=UserOdFkeyType.UserTheme};
			}
			if(checkAlternateIcons.Checked){
				_userODPrefTheme.Fkey=1;
			}
			else{
				_userODPrefTheme.Fkey=0;
			}
			UserOdPrefs.Upsert(_userODPrefTheme);
			if(PrefC.GetBool(PrefName.ThemeSetByUser)) {
				UserOdPrefL.SetThemeForUserIfNeeded();
			}
			else {
				//No need to return, just showing a warning so they know why the theme will not change.
				MsgBox.Show("Theme will not take effect until the miscellaneous preference has been set for users can set their own theme.");
			}
			#endregion
		}

		private void butOK_Click(object sender,EventArgs e) {
			SavePreferences();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}