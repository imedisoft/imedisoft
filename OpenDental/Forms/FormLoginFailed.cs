using System;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormLoginFailed:ODForm {
		private string _errorMsg;

		///<summary></summary>
		public FormLoginFailed(string errorMessage) {
			InitializeComponent();
			_errorMsg=errorMessage;
		}

		private void FormLoginFailed_Load(object sender,EventArgs e) {
			labelErrMsg.Text=_errorMsg;
			textUser.Text=Security.CurUser.UserName;//CurUser verified to not be null in FormOpenDental before loading this form
			textPassword.Focus();
		}

		private void butLogin_Click(object sender,EventArgs e) {
			Userod userEntered;
			string password;
			try {
				bool useEcwAlgorithm=Programs.UsingEcwTightOrFullMode();
				//ecw requires hash, but non-ecw requires actual password
				password=textPassword.Text;
				string username=textUser.Text;
				#if DEBUG
				if(username=="") {
					username="Admin";
					password="pass";
				}
				#endif
				//Set the PasswordTyped property prior to checking the credentials for Middle Tier.
				Security.PasswordTyped=password;
				userEntered=Userods.CheckUserAndPassword(username,password,useEcwAlgorithm);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			//successful login.
			Security.CurUser=userEntered;
			Security.IsUserLoggedIn=true;
			UserOdPrefL.SetThemeForUserIfNeeded();
			if(PrefC.GetBool(PrefName.PasswordsMustBeStrong)
				&& PrefC.GetBool(PrefName.PasswordsWeakChangeToStrong)
				&& Userods.IsPasswordStrong(textPassword.Text)!="") //Password is not strong
			{
				MessageBox.Show("You must change your password to a strong password due to the current Security settings.");
				if(!SecurityL.ChangePassword(true)) {//Failed password update.
					return;
				}
			}
			SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurUser.Id+" has logged on.");
			DialogResult=DialogResult.OK;
		}

		private void butExit_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}