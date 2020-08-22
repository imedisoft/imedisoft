using CodeBase;
using Imedisoft.Forms;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAlertCategoryEdit : FormBase
	{
		private readonly AlertCategory alertCategory;


		private List<AlertType> listShownAlertTypes;
		private List<AlertCategoryLink> listOldAlertCategoryLinks;//New list generated on OK click.


		public FormAlertCategoryEdit(AlertCategory category)
		{
			InitializeComponent();

			alertCategory = category;
		}

		private void FormAlertCategoryEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = alertCategory.Description;

			listShownAlertTypes = Enum.GetValues(typeof(AlertType)).OfType<AlertType>().ToList();

			if (alertCategory.IsHqCategory)
			{
				descriptionTextBox.Enabled = false;
				deleteButton.Enabled = false;
				acceptButton.Enabled = false;
			}

			listOldAlertCategoryLinks = AlertCategoryLinks.GetForCategory(alertCategory.Id);
			InitAlertTypeSelections();
		}

		private void InitAlertTypeSelections()
		{
			alertTypesListBox.Items.Clear();

			List<AlertType> listCategoryAlertTypes = listOldAlertCategoryLinks.Select(x => x.AlertType).ToList();

			foreach (AlertType type in listShownAlertTypes)
			{
				int index = alertTypesListBox.Items.Add(type.GetDescription());

				alertTypesListBox.SetSelected(index, listCategoryAlertTypes.Contains(type));
			}
		}

		private void AlertTypesListBox_MouseClick(object sender, MouseEventArgs e)
		{
			if (alertCategory.IsHqCategory)
			{
				InitAlertTypeSelections();

				ShowError("You can only edit custom alert categories.");
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			alertCategory.Description = descriptionTextBox.Text;

			List<AlertType> listSelectedTypes = alertTypesListBox.SelectedIndices
				.OfType<int>()
				.Select(x => (AlertType)listShownAlertTypes[x])
				.ToList();

			List<AlertCategoryLink> listNewAlertCategoryType = listOldAlertCategoryLinks.Select(x => x.Copy()).ToList();

			listNewAlertCategoryType.RemoveAll(x => !listSelectedTypes.Contains(x.AlertType));//Remove unselected AlertTypes
			foreach (AlertType type in listSelectedTypes)
			{
				if (!listOldAlertCategoryLinks.Exists(x => x.AlertType == type))
				{//Add newly selected AlertTypes.
					listNewAlertCategoryType.Add(new AlertCategoryLink(alertCategory.Id, type));
				}
			}

			AlertCategoryLinks.Sync(listNewAlertCategoryType, listOldAlertCategoryLinks);
			AlertCategories.Update(alertCategory);

			DataValid.SetInvalid(InvalidType.AlertCategoryLinks, InvalidType.AlertCategories);

			DialogResult = DialogResult.OK;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (Prompt("Are you sure you would like to delete this?") == DialogResult.Yes)
			{
				return;
			}

			AlertCategoryLinks.DeleteForCategory(alertCategory.Id);
			AlertCategories.Delete(alertCategory.Id);

			DataValid.SetInvalid(InvalidType.AlertCategories, InvalidType.AlertCategories);

			DialogResult = DialogResult.OK;
		}
	}
}
