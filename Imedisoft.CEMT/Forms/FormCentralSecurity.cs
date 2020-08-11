using CentralManager;
using DataConnectionBase;
using OpenDental;
using OpenDentBusiness;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralSecurity : FormBase
	{
		public FormCentralSecurity()
		{
			InitializeComponent();
		}

		private void FormCentralSecurity_Load(object sender, EventArgs e)
		{
			syncCodeTextBox.Text = Prefs.GetString(PrefName.CentralManagerSyncCode);
			securityLockCheckBox.Checked = Prefs.GetBool(PrefName.CentralManagerSecurityLock);
			adminCheckBox.Checked = Prefs.GetBool(PrefName.SecurityLockIncludesAdmin);

			if (PrefC.GetDate(PrefName.SecurityLockDate).Year > 1880)
			{
				dateTextBox.Text = PrefC.GetDate(PrefName.SecurityLockDate).ToShortDateString();
			}

			if (PrefC.GetInt(PrefName.SecurityLockDays) > 0)
			{
				daysTextBox.Text = PrefC.GetInt(PrefName.SecurityLockDays).ToString();
			}
		}

		private void DateTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			daysTextBox.Text = "";
		}

		private void DateTextBox_Validating(object sender, CancelEventArgs e)
		{
			var value = dateTextBox.Text.Trim();

			if (value.Length > 0)
			{
				if (!DateTime.TryParse(value, out var dateTime))
				{
					dateTextBox.Text = "";
				}
				else
				{
					dateTextBox.Text = dateTime.ToShortDateString();
				}
			}
		}

		private void DaysTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			dateTextBox.Text = "";
		}

		private void DaysTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void DaysTextBox_Validating(object sender, CancelEventArgs e)
		{
			var value = daysTextBox.Text.Trim();

			if (value.Length > 0 && int.TryParse(value, out _))
			{
				return;
			}

			daysTextBox.Text = "";
		}

		private void PushBothButton_Click(object sender, EventArgs e)
		{
			if (!SavePreferences()) return;

			using (var formCentralConnections = new FormCentralConnections())
			{
				formCentralConnections.IsSelectionMode = true;

				if (formCentralConnections.ShowDialog() != DialogResult.OK)
				{
					return;
				}

				using (CodeBase.MsgBoxCopyPaste msgBox = new CodeBase.MsgBoxCopyPaste(CentralSyncHelper.PushBoth(formCentralConnections.SelectedConnections)))
				{
					msgBox.ShowDialog();
				}
			}
		}

		private void PushUsersButton_Click(object sender, EventArgs e)
		{
			using (var formCentralConnections = new FormCentralConnections())
			{
				formCentralConnections.IsSelectionMode = true;

				if (formCentralConnections.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}

				using (CodeBase.MsgBoxCopyPaste msgBox = new CodeBase.MsgBoxCopyPaste(CentralSyncHelper.PushUsers(formCentralConnections.SelectedConnections)))
				{
					msgBox.ShowDialog(this);
				}
			}
		}

		private void PushLocksButton_Click(object sender, EventArgs e)
		{
			using (var formCentralConnections = new FormCentralConnections())
			{
				formCentralConnections.IsSelectionMode = true;

				if (formCentralConnections.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}

				using (CodeBase.MsgBoxCopyPaste msgBox = new CodeBase.MsgBoxCopyPaste(CentralSyncHelper.PushLocks(formCentralConnections.SelectedConnections)))
				{
					msgBox.ShowDialog(this);
				}
			}
		}

		private void SecurityEditor_AddUserClick(object sender, SecurityEventArgs e)
		{
            var user = new Userod
            {
                IsNew = true
            };

            using (var formCentralUserEdit = new FormCentralUserEdit(user))
			{
				if (formCentralUserEdit.ShowDialog() == DialogResult.OK)
				{
					securityEditor.FillGridUsers();
					securityEditor.RefreshUserTabGroups();
				}
			}
		}

		private void SecurityEditor_EditUserClick(object sender, SecurityEventArgs e)
		{
			using (var formCentralUserEdit = new FormCentralUserEdit(e.User))
			{
				if (formCentralUserEdit.ShowDialog(this) == DialogResult.OK)
				{
					securityEditor.FillGridUsers();
					securityEditor.RefreshUserTabGroups();
				}
			}
		}

		private void SecurityEditor_AddUserGroupClick(object sender, SecurityEventArgs e)
		{
            var userGroup = new UserGroup
            {
                IsNew = true
            };

            using (var formCentralUserGroupEdit = new FormCentralUserGroupEdit(userGroup))
			{
				if (formCentralUserGroupEdit.ShowDialog(this) == DialogResult.OK)
				{
					securityEditor.FillListUserGroupTabUserGroups();
					securityEditor.SelectedUserGroup = userGroup;
				}
			}
		}

		private void SecurityEditor_EditUserGroupClick(object sender, SecurityEventArgs e)
		{
			using (var formCentralUserGroupEdit = new FormCentralUserGroupEdit(e.Group))
			{
				if (formCentralUserGroupEdit.ShowDialog(this) == DialogResult.OK)
				{
					securityEditor.FillListUserGroupTabUserGroups();
				}
			}
		}

		private DialogResult SecurityEditor_ReportPermissionChecked(object sender, SecurityEventArgs e)
		{
			var groupPermission = e.Perm;

			using (var formCentralReportSetup = new FormCentralReportSetup(groupPermission.UserGroupNum, true))
			{
				if (formCentralReportSetup.ShowDialog(this) == DialogResult.Cancel)
				{
					return formCentralReportSetup.DialogResult;
				}

				if (!formCentralReportSetup.HasReportPermissions)
				{
					return formCentralReportSetup.DialogResult;
				}

				try
				{
					GroupPermissions.Insert(groupPermission);
				}
				catch (Exception exception)
				{
					ShowError(exception.Message);

					return DialogResult.Cancel;
				}

				return formCentralReportSetup.DialogResult;
			}
		}

		private DialogResult SecurityEditor_GroupPermissionChecked(object sender, SecurityEventArgs e)
		{
			using (var formCentralGroupPermEdit = new FormCentralGroupPermEdit(e.Perm))
			{
				return formCentralGroupPermEdit.ShowDialog(this);
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (SavePreferences())
			{
				DialogResult = DialogResult.OK;
			}
		}

		private bool SavePreferences()
        {
			var date = DateTime.MinValue;
			if (!string.IsNullOrEmpty(dateTextBox.Text))
			{
				if (!DateTime.TryParse(dateTextBox.Text, out date))
				{
					ShowError("Please enter a valid date.");
					return false;
				}
			}

			int days = 0;
			if (!string.IsNullOrEmpty(daysTextBox.Text))
            {
				if (!int.TryParse(daysTextBox.Text, out days))
                {
					ShowError("Please enter a valid number of days.");

					return false;
                }

				if (days > GroupPermissions.NewerDaysMax)
				{
					ShowError($"Days must be less than {GroupPermissions.NewerDaysMax}.");

					return false;
				}
			}

			Prefs.Set(PrefName.SecurityLockDate, SOut.Date(date, false));
			Prefs.Set(PrefName.SecurityLockDays, days);
			Prefs.Set(PrefName.SecurityLockIncludesAdmin, adminCheckBox.Checked);
			Prefs.Set(PrefName.CentralManagerSecurityLock, securityLockCheckBox.Checked);

			return true;
		}
    }
}
