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
		private List<AlertSub> oldAlertSubscriptions;
		private bool _isFillingList;

		public FormCentralUserEdit(Userod user)
		{
			InitializeComponent();

			this.user = user.Copy();
		}

		private void FormCentralUserEdit_Load(object sender, EventArgs e)
		{
			checkIsHidden.Checked = user.IsHidden;
			usernameTextBox.Text = user.UserName;

			_isFillingList = true;

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
			{//never allowed to delete last group, so this won't fail
				userGroupsListBox.SelectedIndex = 0;
			}

			_isFillingList = false;
			securityTreeUser.FillTreePermissionsInitial();
			RefreshUserTree();

			if (user.PasswordHash == "")
			{
				passwordButton.Text = "Create Password";
			}

			oldAlertSubscriptions = AlertSubs.GetAllForUser(Security.CurUser.UserNum);
			listAlertSubMulti.Items.Clear();
			string[] arrayAlertTypes = Enum.GetNames(typeof(AlertType));
			for (int i = 0; i < arrayAlertTypes.Length; i++)
			{
				listAlertSubMulti.Items.Add(arrayAlertTypes[i]);
				listAlertSubMulti.SetSelected(i, oldAlertSubscriptions.Exists(x => x.Type == (AlertType)i));
			}
			if (user.IsNew)
			{
				unlockButton.Visible = false;
			}
		}

		private void RefreshUserTree()
		{
			securityTreeUser.FillForUserGroup(
				userGroupsListBox.SelectedItems.OfType<UserGroup>()
					.Select(userGroup => userGroup.UserGroupNum)
					.ToList());
		}

		private void UserGroupsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_isFillingList)
			{
				return;
			}

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
			var result = MessageBox.Show(this, 
				"Users can become locked when invalid credentials have been entered several times in a row.\r\n" +
				"Unlock this user so that more log in attempts can be made?", "CEMT", 
				MessageBoxButtons.YesNo, 
				MessageBoxIcon.Question);

			if (result == DialogResult.No)
            {
				return;
            }

			user.DateTFail = DateTime.MinValue;
			user.FailedAttempts = 0;

			try
			{
				Userods.Update(user);

				MessageBox.Show(this, 
					"User has been unlocked.", "CEMT", 
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);
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

			foreach (int index in listAlertSubMulti.SelectedIndices)
			{
                var alertSub = new AlertSub
                {
                    ClinicNum = 0,
                    UserNum = Security.CurUser.UserNum,
                    Type = (AlertType)index
                };
                alertSubscriptions.Add(alertSub);
			}

			AlertSubs.Sync(alertSubscriptions, oldAlertSubscriptions);

			user.IsHidden = checkIsHidden.Checked;
			user.UserName = username;
			user.EmployeeNum = 0;
			user.ProvNum = 0;
			user.ClinicNum = 0;
			user.ClinicIsRestricted = false;

			if (user.UserNum == Security.CurUser.UserNum)
			{
				Security.CurUser.UserName = username;
				//They changed their logged in user's information.  Update for when they sync then attempt to connect to remote DB.
			}

			try
			{
				if (user.IsNew)
				{
					long userNum = Userods.Insert(user, 
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

        private void checkIsHidden_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
