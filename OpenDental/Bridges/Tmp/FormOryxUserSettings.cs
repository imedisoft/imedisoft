using System;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	/// <summary>Used for user-specific settings that are unique to the Oryx bridge.</summary>
	public partial class FormOryxUserSettings:ODForm {
		///<summary>User pref holding the user's Oryx username.</summary>
		private UserOdPref _userNamePref;
		///<summary>User pref holding the user's Oryx password.</summary>
		private UserOdPref _passwordPref;
		///<summary>Oryx program bridge.</summary>
		private Program _progOryx;

		public FormOryxUserSettings() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormUserSetting_Load(object sender,EventArgs e) {
			_progOryx=Programs.GetCur(ProgramName.Oryx);
			_userNamePref=UserOdPrefs.GetByUserFkeyAndFkeyType(Security.CurrentUser.Id,_progOryx.Id,UserOdFkeyType.ProgramUserName)
				.FirstOrDefault();
			_passwordPref=UserOdPrefs.GetByUserFkeyAndFkeyType(Security.CurrentUser.Id,_progOryx.Id,UserOdFkeyType.ProgramPassword)
				.FirstOrDefault();
			if(_userNamePref!=null) {
				textUsername.Text=_userNamePref.ValueString;
			}
			if(_passwordPref!=null) {
				textPassword.Text= _passwordPref.ValueString;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			_userNamePref=_userNamePref??new UserOdPref {
				Fkey=_progOryx.Id,
				FkeyType=UserOdFkeyType.ProgramUserName,
				UserNum=Security.CurrentUser.Id,
			};
			_passwordPref=_passwordPref??new UserOdPref {
				Fkey=_progOryx.Id,
				FkeyType=UserOdFkeyType.ProgramPassword,
				UserNum=Security.CurrentUser.Id,
			};
			_userNamePref.ValueString=textUsername.Text;
			_passwordPref.ValueString = textPassword.Text;
			UserOdPrefs.Upsert(_userNamePref);
			UserOdPrefs.Upsert(_passwordPref);
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}

	}
}