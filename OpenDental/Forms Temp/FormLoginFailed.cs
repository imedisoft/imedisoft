using Imedisoft.Data;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormLoginFailed : FormBase
	{
		private readonly string errorMessage;

		public FormLoginFailed(string errorMessage)
		{
			InitializeComponent();

			this.errorMessage = errorMessage;
		}

		private void FormLoginFailed_Load(object sender, EventArgs e)
		{
			errorLabel.Text = errorMessage;
			userTextBox.Text = Security.CurrentUser?.UserName;

			passwordTextBox.Focus();
		}

		private void LoginButton_Click(object sender, EventArgs e)
		{
			try
			{
				Security.CurrentUser = Users.CheckUserAndPassword(userTextBox.Text, passwordTextBox.Text);
			}
			catch (Exception exception)
			{
                ShowError(exception.Message);

				return;
			}

			if (Preferences.GetBool(PreferenceName.PasswordsMustBeStrong) && 
				Preferences.GetBool(PreferenceName.PasswordsWeakChangeToStrong))
			{
				if (!Users.IsPasswordStrong(passwordTextBox.Text))
				{
					ShowInfo(Translation.Common.YouMustChangePasswordToStrongPasswordDueToSecuritySettings);

					if (!SecurityL.ChangePassword(true))
					{
						return;
					}
				}
			}

			SecurityLogs.Write(Permissions.UserLogOnOff, "User '" + Security.CurrentUser.Id + "' has logged on.");

			DialogResult = DialogResult.OK;
		}
	}
}
