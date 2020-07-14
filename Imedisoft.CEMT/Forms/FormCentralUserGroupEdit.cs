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

			try
			{
				UserGroups.Delete(userGroup);

				DialogResult = DialogResult.OK;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
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
					long userGroupNum = UserGroups.Insert(userGroup);

					userGroup.UserGroupNumCEMT = userGroupNum;

					UserGroups.Update(userGroup); // Doing this so we don't have to make another version of Insert
				}
				else
				{
					UserGroups.Update(userGroup);
				}
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
				return;
			}

			Cache.Refresh(InvalidType.Security);

			DialogResult = DialogResult.OK;
		}
	}
}
