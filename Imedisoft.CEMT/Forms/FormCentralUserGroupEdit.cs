using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralUserGroupEdit : FormBase
	{
		private readonly UserGroup userGroup;

		public FormCentralUserGroupEdit(UserGroup userGroup)
		{
			InitializeComponent();

			this.userGroup = userGroup;
		}

		private void FormCentralUserGroupEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = userGroup.Description;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (userGroup.IsNew)
			{
				DialogResult = DialogResult.Cancel;

				return;
			}

			if (Confirm("Are you sure you want to delete this user group?") == DialogResult.Yes)
			{
				try
				{
					UserGroups.Delete(userGroup);

					DialogResult = DialogResult.OK;
				}
				catch (Exception exception)
				{
					ShowError(exception.Message);
				}
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
			{
				ShowError("Please enter a description.");
				return;
			}

			userGroup.Description = description;
			try
			{
				if (userGroup.IsNew)
				{
					long userGroupId = UserGroups.Insert(userGroup);

					userGroup.CentralUserGroupId = userGroupId;

					UserGroups.Update(userGroup);
				}
				else
				{
					UserGroups.Update(userGroup);
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
