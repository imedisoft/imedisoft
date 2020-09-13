using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormUserGroupEdit : FormBase
	{
		private readonly UserGroup userGroup;

		public FormUserGroupEdit(UserGroup userGroup)
		{
			InitializeComponent();

			this.userGroup = userGroup;
		}

		private void FormUserGroupEdit_Load(object sender, EventArgs e)
		{
			textDescription.Text = userGroup.Description;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (userGroup.Id == 0)
			{
				DialogResult = DialogResult.Cancel;

				return;
			}

			if (Preferences.GetLong(PreferenceName.DefaultUserGroup) == userGroup.Id)
			{
				ShowError("Cannot delete user group that is set as the default user group.");

				return;
			}

			try
			{
				UserGroups.Delete(userGroup);

				CacheManager.RefreshGlobal(nameof(InvalidType.Security));

				DialogResult = DialogResult.OK;
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var description = textDescription.Text.Trim();
			if (description.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterDescription);

				return;
			}

			userGroup.Description = description;

			UserGroups.Save(userGroup);

			CacheManager.RefreshGlobal(nameof(InvalidType.Security));

			DialogResult = DialogResult.OK;
		}
	}
}
