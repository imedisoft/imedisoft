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
		public Userod User { get; private set; }

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
			if (PrefC.GetBool(PrefName.UserNameManualEntry))
			{
				usersListBox.Visible = false;
				userTextBox.Visible = true;
				userTextBox.Focus();
			}
			else
			{
				showCentralUsersCheckBox.Visible = Userods.HasUsersForCEMTNoCache();
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

			var userNames = Userods.GetUserNamesNoCache(showCentralUsersCheckBox.Checked);

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

            if (PrefC.GetBool(PrefName.UserNameManualEntry))
			{
				userName = userTextBox.Text.Trim();
				if (userName.Length == 0)
                {
					ShowError("You have to enter your username.");

					return;
                }
			}
			else
			{
				userName = usersListBox.SelectedItem?.ToString();
				if (string.IsNullOrEmpty(userName))
                {
					ShowError("You have to select a user.");

					return;
                }
			}

			var password = passwordTextBox.Text;

			try
			{
				User = Userods.CheckUserAndPassword(userName, password);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			if (!isTemporary)
			{
				Security.CurrentUser = User;

				if (PrefC.GetBool(PrefName.PasswordsMustBeStrong) && PrefC.GetBool(PrefName.PasswordsWeakChangeToStrong))
				{
					if (Userods.IsPasswordStrong(password) != "")
					{
						ShowInfo("You must change your password to a strong password due to the current Security settings.");

						if (!SecurityL.ChangePassword(true))
						{
							return;
						}

						RefreshSecurityCache = true;
					}
				}
				
				SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, "User: " + Security.CurrentUser.UserName + " has logged on.");
			}

			DialogResult = DialogResult.OK;
		}
	}
}
