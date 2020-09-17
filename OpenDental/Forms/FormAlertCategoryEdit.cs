using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAlertCategoryEdit : FormBase
	{
		private readonly AlertCategory alertCategory;

		public FormAlertCategoryEdit(AlertCategory category)
		{
			InitializeComponent();

			alertCategory = category;
		}

		private void FormAlertCategoryEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = alertCategory.Description;

			var alertCategoryLinks = AlertCategoryLinks.GetByAlertCategory(alertCategory.Id).ToList();

			foreach (var alertType in AlertType.GetValues())
            {
				var index = alertTypesCheckedListBox.Items.Add(alertType);

				if (alertCategoryLinks.Any(x => x.Type == alertType.Value))
                {
					alertTypesCheckedListBox.SetItemChecked(index, true);
                }
            }

			if (alertCategory.IsHqCategory)
			{
				descriptionTextBox.Enabled = false;
				alertTypesCheckedListBox.ItemCheck += AlertTypesCheckedListBox_ItemCheck;
				alertTypesCheckedListBox.CheckOnClick = false;
				acceptButton.Enabled = false;
			}
		}

		private void AlertTypesCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			e.NewValue = e.CurrentValue;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (alertCategory.IsHqCategory) return;

			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
            {
				ShowError(Translation.Common.PleaseEnterDescription);

				return;
            }

			alertCategory.Description = description;

			AlertCategories.Update(alertCategory);

			AlertCategoryLinks.Assign(alertCategory,
				alertTypesCheckedListBox.CheckedItems.OfType<DataItem<string>>().Select(x => x.Value));

			CacheManager.RefreshGlobal(nameof(InvalidType.AlertCategories));

			DialogResult = DialogResult.OK;
		}
    }
}
