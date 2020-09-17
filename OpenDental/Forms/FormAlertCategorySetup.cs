using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAlertCategorySetup : FormBase
	{
		public FormAlertCategorySetup()
		{
			InitializeComponent();
		}

		private void FormAlertCategorySetup_Load(object sender, EventArgs e)
		{
			FillGrids();
		}

		private void FillGrids()
		{
			var alertCategories = AlertCategories.GetAll().OrderBy(x => x.InternalName);
			var alertCategoriesInternal = alertCategories.Where(x => x.IsHqCategory);
			var alertCategoriesCustom = alertCategories.Where(x => !x.IsHqCategory);

			FillGrid(internalGrid, alertCategoriesInternal);
			FillGrid(customGrid, alertCategoriesCustom);
		}

		private void FillGrid(ODGrid grid, IEnumerable<AlertCategory> alertCategories)
		{
			grid.BeginUpdate();
			grid.Columns.Clear();
			grid.Columns.Add(new GridColumn(Translation.Common.Description, 100));
			grid.Rows.Clear();

			foreach (var alertCategory in alertCategories)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(alertCategory.Description);
				gridRow.Tag = alertCategory;

				grid.Rows.Add(gridRow);
			}

			grid.EndUpdate();
		}

		private void Edit(AlertCategory alertCategory)
		{
			if (alertCategory == null)
			{
				return;
			}

			using var formAlertCategoryEdit = new FormAlertCategoryEdit(alertCategory);

			if (formAlertCategoryEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrids();
		}

		private void InternalGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			Edit(internalGrid.SelectedTag<AlertCategory>());
		}

		private void CopyButton_Click(object sender, EventArgs e)
		{
			var alertCategory = internalGrid.SelectedTag<AlertCategory>();
			if (alertCategory == null)
			{
				ShowError("Please select an internal alert category first.");

				return;
			}

			CopyAlertCategory(alertCategory);
		}

		private void CustomGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void CustomGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = duplicateButton.Enabled 
				= customGrid.SelectedRows.Count > 0;
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			Edit(customGrid.SelectedTag<AlertCategory>());
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var alertCategory = customGrid.SelectedTag<AlertCategory>();
			if (alertCategory == null)
            {
				return;
            }

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
            {
				return;
            }

			AlertCategories.Delete(alertCategory);

			CacheManager.RefreshGlobal(nameof(InvalidType.AlertCategories));

			FillGrids();
		}

		private void DuplicateButton_Click(object sender, EventArgs e)
		{
			var alertCategory = customGrid.SelectedTag<AlertCategory>();
			if (alertCategory == null)
			{
				ShowError("Please select a custom alert category first.");

				return;
			}

			CopyAlertCategory(alertCategory);
		}

		private void CopyAlertCategory(AlertCategory alertCategory)
		{
			var alertCategoryLinks = AlertCategoryLinks.GetByAlertCategory(alertCategory.Id);

			var newAlertCategory = new AlertCategory
			{
				Description = alertCategory.Description + " " + Translation.Common.TagCopy
			};

			AlertCategories.Insert(newAlertCategory);
			AlertCategoryLinks.Assign(newAlertCategory, alertCategoryLinks.Select(x => x.Type));
			
			CacheManager.RefreshGlobal(nameof(InvalidType.AlertCategories));

			FillGrids();
		}
    }
}
