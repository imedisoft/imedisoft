﻿using OpenDentBusiness;
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

				if (user.IsInUserGroup(userGroup.Id))
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

			if (user.IsNew) unlockButton.Visible = false;
		}

		private void RefreshUserTree() 
			=> securityTreeUser.FillForUserGroup(
				userGroupsListBox.SelectedItems.OfType<UserGroup>()
					.Select(userGroup => userGroup.Id)
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

				user.PasswordHash = formCentralUserPasswordEdit.PasswordHash;
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

			user.FailedLoginDateTime = DateTime.MinValue;
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

			user.IsHidden = checkIsHidden.Checked;
			user.UserName = username;
			user.EmployeeId = 0;
			user.ProviderId = 0;
			user.ClinicId = 0;
			user.ClinicIsRestricted = false;

			if (user.Id == Security.CurrentUser.Id)
			{
				Security.CurrentUser.UserName = username;
			}

			try
			{
				if (user.IsNew)
				{
					long userId = Userods.Insert(user, 
						userGroupsListBox.SelectedItems.OfType<UserGroup>().Select(x => x.Id).ToList(),
						true);
				}
				else
				{
					Userods.Update(user, 
						userGroupsListBox.SelectedItems.OfType<UserGroup>().Select(x => x.Id).ToList());
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
