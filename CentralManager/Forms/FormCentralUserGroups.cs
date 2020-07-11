using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralUserGroups : FormBase
	{
		public FormCentralUserGroups() => InitializeComponent();

		private void FormCentralUserGroups_Load(object sender, EventArgs e)
		{
			UserGroups.RefreshCache();

			userGroupsListBox.Items.Clear();

			foreach (var userGroup in UserGroups.GetDeepCopy())
			{
				userGroupsListBox.Items.Add(userGroup);
			}
		}

		private void UserGroupsListBox_DoubleClick(object sender, EventArgs e)
		{
			if (userGroupsListBox.SelectedIndex == -1)
			{
				return;
			}

			if (userGroupsListBox.SelectedItem is UserGroup userGroup)
			{
				using (var formCentralUserGroupEdit = new FormCentralUserGroupEdit(userGroup))
				{
					if (formCentralUserGroupEdit.ShowDialog() == DialogResult.Cancel)
					{
						return;
					}
				}

				userGroupsListBox.Invalidate(
					userGroupsListBox.GetItemRectangle(
						userGroupsListBox.SelectedIndex));
			}
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
            var userGroup = new UserGroup
            {
                IsNew = true
            };

            using (var formCentralUserGroupEdit = new FormCentralUserGroupEdit(userGroup))
            {
				if (formCentralUserGroupEdit.ShowDialog() == DialogResult.Cancel)
				{
					return;
				}
			}

			userGroupsListBox.Items.Add(userGroup);
			userGroupsListBox.SelectedItem = userGroup;
		}
	}
}
