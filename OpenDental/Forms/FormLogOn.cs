using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormLogOn : FormBase
	{
		private readonly bool isTemporary;

		/// <summary>
		/// Gets the user that has logged on.
		/// </summary>
		public User User { get; private set; }

        /// <summary>
		/// Will be true when the calling method needs to refresh the security cache themselves due to changes.
		/// </summary>
        public bool RefreshSecurityCache { get; private set; } = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="FormLogOn"/> class.
		/// </summary>
		/// <param name="isTemporary">
		///		<para>
		///			Set to true if this is a temporary log on. User in scenario's where we 
		///			temporarily need to switch to another user.
		///		</para>
		///		<para>
		///			When performing a temporary login the <see cref="Security.CurrentUser"/> is not
		///			updated, instead the logged in user should be accessed through the 
		///			<see cref="User"/> property of this form.
		///		</para>
		/// </param>
		public FormLogOn(bool isTemporary = false)
		{
			InitializeComponent();

			this.isTemporary = isTemporary;
		}

		private void FormLogOn_Load(object sender, EventArgs e)
		{
			if (Preferences.GetBool(PreferenceName.UserNameManualEntry))
			{
				usersListBox.Visible = false;
				userTextBox.Visible = true;
				userTextBox.Focus();
			}
			else
			{
				showCentralUsersCheckBox.Visible = Users.HasUsersForCemtNoCache();
			}

			FillListBox();

			Focus();
		}

		private void UsersListBox_MouseUp(object sender, MouseEventArgs e)
		{
			passwordTextBox.Focus();
		}

		private void FillListBox()
		{
			usersListBox.BeginUpdate();
			usersListBox.Items.Clear();

			var currentUserName = Security.CurrentUser?.UserName;

			var userNames = Users.GetUserNamesNoCache(showCentralUsersCheckBox.Checked);

			foreach (string userName in userNames)
			{
				usersListBox.Items.Add(userName);
				if (userName.Equals(currentUserName, StringComparison.CurrentCultureIgnoreCase))
                {
					usersListBox.SelectedItem = userName;
                }
			}

			if (usersListBox.SelectedItem == null && 
				usersListBox.Items.Count > 0)
			{
				usersListBox.SelectedIndex = 0;
			}

			usersListBox.EndUpdate();
		}

		private void ShowCentralUsersCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			FillListBox();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
            string userName;

            if (Preferences.GetBool(PreferenceName.UserNameManualEntry))
			{
				userName = userTextBox.Text.Trim();
				if (userName.Length == 0)
                {
					ShowError(Translation.Common.YouHaveToEnterYourUsername);

					return;
                }
			}
			else
			{
				userName = usersListBox.SelectedItem?.ToString();
				if (string.IsNullOrEmpty(userName))
                {
					ShowError(Translation.Common.PleaseSelectUser);

					return;
                }
			}

			var password = passwordTextBox.Text;

			try
			{
				User = Users.CheckUserAndPassword(userName, password);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			if (!isTemporary)
			{
				Security.CurrentUser = User;

				if (Preferences.GetBool(PreferenceName.PasswordsMustBeStrong) && Preferences.GetBool(PreferenceName.PasswordsWeakChangeToStrong))
				{
					if (!Users.IsPasswordStrong(password))
					{
						ShowInfo(Translation.Common.YouMustChangePasswordToStrongPasswordDueToSecuritySettings);

						if (!SecurityL.ChangePassword(true))
						{
							return;
						}

						RefreshSecurityCache = true;
					}
				}
				
				SecurityLogs.Write(Permissions.UserLogOnOff, 
					string.Format(Translation.SecurityLog.UserHasLoggedOn, Security.CurrentUser.UserName));
			}

			DialogResult = DialogResult.OK;
		}
	}
}
