using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormUserPassword : FormBase
	{
		private readonly string userName;
		private readonly bool isPasswordReset;
		private readonly bool isCreate;
		private readonly bool inSecurityWindow;

		/// <summary>
		/// Gets a value indicating whether the entered password is strong.
		/// </summary>
		public bool PasswordIsStrong { get; private set; }

		/// <summary>
		/// Gets the hash of the entered password.
		/// </summary>
		public string PasswordHash { get; private set; }

		public FormUserPassword(string userName, bool isPasswordReset = false, bool isCreate = false, bool inSecurityWindow = false)
		{
			InitializeComponent();

			this.userName = userName;
			this.isPasswordReset = isPasswordReset;
			this.isCreate = isCreate;
			this.inSecurityWindow = inSecurityWindow;
		}

		private void FormUserPassword_Load(object sender, EventArgs e)
		{
			userTextBox.Text = userName;

			if (inSecurityWindow)
			{
				passwordLabel.Visible = false;
				passwordTextBox.Visible = false;
			}

			if (isCreate || isPasswordReset)
			{
				passwordLabel.Text = newPasswordLabel.Text;
				newPasswordLabel.Text = Translation.Common.ReEnterPassword;
				cancelButton.Visible = false;
				acceptButton.Location = cancelButton.Location;

				ControlBox = false;
			}
		}

		private void ShowCheckBox_Click(object sender, EventArgs e)
		{
			newPasswordTextBox.UseSystemPasswordChar = !showCheckBox.Checked;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (isPasswordReset)
			{
				if (string.IsNullOrEmpty(newPasswordTextBox.Text))
                {
					ShowError(Translation.Common.PasswordCannotBeEmpty);

					return;
                }

				if (newPasswordTextBox.Text != passwordTextBox.Text)
				{
					ShowError(Translation.Common.PasswordsDoNotMatch);

					return;
				}
			}
			else if (!inSecurityWindow && !Password.Verify(passwordTextBox.Text, Security.CurrentUser.PasswordHash))
			{
				ShowError(Translation.Common.CurrentPasswordIncorrect);

				return;
			}

			bool isPasswordStrong = false;
			if (Preferences.GetBool(PreferenceName.PasswordsMustBeStrong))
			{
				try
				{
					Users.EnsurePasswordStrong(newPasswordTextBox.Text);

					isPasswordStrong = true;
				}
				catch (Exception exception)
				{
					ShowError(exception.Message);

					return;
				}
			}

			PasswordIsStrong = isPasswordStrong;
			PasswordHash = Password.Hash(newPasswordTextBox.Text);

			DialogResult = DialogResult.OK;
		}
	}
}
