using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralUserPasswordEdit : FormBase
	{
		//public string HashedResult;

		private readonly bool isCreate;
		private readonly bool isSecurityWindow;

		public PasswordContainer LoginDetails;

		public FormCentralUserPasswordEdit(string userName, bool isCreate, bool isSecurityWindow)
		{
			InitializeComponent();

			this.isCreate = isCreate;
			this.isSecurityWindow = isSecurityWindow;

			usernameTextBox.Text = userName;
		}

		private void FormCentralUserPasswordEdit_Load(object sender, EventArgs e)
		{
			if (isCreate) Text = "Create Password";

			if (isSecurityWindow)
			{
				currentLabel.Visible = false;
				currentTextBox.Visible = false;
			}
		}

		private void ShowCheckBox_CheckedChanged(object sender, EventArgs e)
			=> newTextBox.UseSystemPasswordChar = !showCheckBox.Checked;

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var user = Userods.GetUserByName(usernameTextBox.Text, false);

			if (!isCreate && !isSecurityWindow)
			{
				string currentPassword = "";
				if (user != null)
				{
					currentPassword = user.PasswordHash;
				}

				// If user's current password is blank we dont care what they put for the old one.
				if (currentPassword != "" && !Authentication.CheckPassword(user, currentTextBox.Text))
				{
					MessageBox.Show(this, "Current password incorrect.");
					return;
				}
			}

			var newPassword = newTextBox.Text;
			if (string.IsNullOrEmpty(newPassword))
			{
				MessageBox.Show(this, "Passwords cannot be blank.");
				return;
			}
			else
			{
				LoginDetails = Authentication.GenerateLoginDetailsSHA512(newTextBox.Text);
				if (user?.UserName == Security.CurUser.UserName || isSecurityWindow)
				{
					Security.PasswordTyped = newTextBox.Text;
					//They're updating the password for the logged in user.  Update CurUser for when they sync then attempt to log into remote DB.
				}
			}

			DialogResult = DialogResult.OK;
		}
    }
}
