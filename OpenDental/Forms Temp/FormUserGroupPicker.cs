using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormUserGroupPicker : FormBase
	{
		/// <summary>
		/// Gets the selected user group.
		/// </summary>
		public UserGroup UserGroup => userGroupsListBox.SelectedItem as UserGroup;

		public bool IsAdminMode { get; set; }

		public FormUserGroupPicker()
		{
			InitializeComponent();
		}

		private void FormUserGroupPicker_Load(object sender, EventArgs e)
		{
			FillList();
		}

		private void FillList()
		{
			UserGroups.RefreshCache();

			userGroupsListBox.Items.Clear();

			foreach (var userGroup in UserGroups.Find(x => IsAdminMode || !GroupPermissions.HasPermission(x.Id, Permissions.SecurityAdmin)))
			{
				userGroupsListBox.Items.Add(userGroup);
			}
		}

		private void UserGroupsListBox_DoubleClick(object sender, EventArgs e)
		{
			if (UserGroup == null)
			{
				return;
			}

			DialogResult = DialogResult.OK;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (UserGroup == null)
			{
				ShowError("Please select a group.");

				return;
			}

			DialogResult = DialogResult.OK;
		}
	}
}
