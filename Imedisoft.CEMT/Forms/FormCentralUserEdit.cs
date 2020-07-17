using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralUserEdit : FormBase
	{
		private readonly Userod user;
		private List<AlertSub> alertSubscriptions;

		public FormCentralUserEdit(Userod user)
		{
			InitializeComponent();

			this.user = user.Copy();
		}

		private void FormCentralUserEdit_Load(object sender, EventArgs e)
		{
			checkIsHidden.Checked = user.IsHidden;
			usernameTextBox.Text = user.UserName;

			securityTreeUser.FillTreePermissionsInitial();

			var userGroups = UserGroups.GetDeepCopy();
			foreach (var userGroup in userGroups)
            {
				var index = userGroupsListBox.Items.Add(userGroup);

				if (user.IsInUserGroup(userGroup.UserGroupNum))
                {
					userGroupsListBox.SetSelected(index, true);
                }
            }

			if (userGroupsListBox.SelectedIndex == -1)
			{
				userGroupsListBox.SelectedIndex = 0;
			}
			
			if (user.PasswordHash == "")
			{
				passwordButton.Text = "Create Password";
			}

			alertSubscriptions = AlertSubs.GetAllForUser(Security.CurUser.UserNum);
			alertSubscriptionsListBox.Items.Clear();

			var alertTypes = Enum.GetNames(typeof(AlertType));
			for (int i = 0; i < alertTypes.Length; i++)
			{
				alertSubscriptionsListBox.Items.Add(alertTypes[i]);
				alertSubscriptionsListBox.SetSelected(i, alertSubscriptions.Exists(x => x.Type == (AlertType)i));
			}

			if (user.IsNew) unlockButton.Visible = false;
		}

		private void RefreshUserTree() 
			=> securityTreeUser.FillForUserGroup(
				userGroupsListBox.SelectedItems.OfType<UserGroup>()
					.Select(userGroup => userGroup.UserGroupNum)
					.ToList());

		private void UserGroupsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			RefreshUserTree();
		}

		private void PasswordButton_Click(object sender, EventArgs e)
		{
			bool isCreate = false;
			if (string.IsNullOrEmpty(user.PasswordHash))
			{
				isCreate = true;
			}

			using (var formCentralUserPasswordEdit = new FormCentralUserPasswordEdit(user.UserName, isCreate, true))
			{
				if (formCentralUserPasswordEdit.ShowDialog() == DialogResult.Cancel)
				{
					return;
				}

				user.LoginDetails = formCentralUserPasswordEdit.LoginDetails;
			}

			passwordButton.Text = 
				string.IsNullOrEmpty(user.PasswordHash) ? 
					"Create Password" : 
					"Change Password";
		}

		private void UnlockButton_Click(object sender, EventArgs e)
		{
			var result = Confirm(
				"Users can become locked when invalid credentials have been entered several times in a row.\r\n" +
				"Unlock this user so that more log in attempts can be made?");

			if (result == DialogResult.No)
            {
				return;
            }

			user.DateTFail = DateTime.MinValue;
			user.FailedAttempts = 0;

			try
			{
				Userods.Update(user);

				ShowInfo("User has been unlocked.");
			}
			catch
			{
				ShowError("There was a problem unlocking this user. Please call support or wait the allotted lock time.");
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var username = usernameTextBox.Text.Trim();
			if (username.Length == 0)
			{
				ShowError("Please enter a username.");
				return;
			}

			if (userGroupsListBox.SelectedItems.Count == 0)
			{
				ShowError("Every user must be associated to at least one User Group.");
				return;
			}

			var alertSubscriptions = new List<AlertSub>();

			foreach (int index in alertSubscriptionsListBox.SelectedIndices)
			{
                var alertSubscription = new AlertSub
                {
                    ClinicNum = 0,
                    UserNum = Security.CurUser.UserNum,
                    Type = (AlertType)index
                };
                alertSubscriptions.Add(alertSubscription);
			}

            AlertSubs.Sync(alertSubscriptions, this.alertSubscriptions);

			user.IsHidden = checkIsHidden.Checked;
			user.UserName = username;
			user.EmployeeNum = 0;
			user.ProvNum = 0;
			user.ClinicNum = 0;
			user.ClinicIsRestricted = false;

			if (user.UserNum == Security.CurUser.UserNum)
			{
				Security.CurUser.UserName = username;
			}

			try
			{
				if (user.IsNew)
				{
					long userId = Userods.Insert(user, 
						userGroupsListBox.SelectedItems.OfType<UserGroup>().Select(x => x.UserGroupNum).ToList(),
						true);
				}
				else
				{
					Userods.Update(user, 
						userGroupsListBox.SelectedItems.OfType<UserGroup>().Select(x => x.UserGroupNum).ToList());
				}
			}
			catch (Exception exception)
			{
                ShowError(exception.Message);
				return;
			}

			Cache.Refresh(InvalidType.Security);

			DialogResult = DialogResult.OK;
		}
    }
}
