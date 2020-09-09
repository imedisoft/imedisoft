using Imedisoft.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAlertCategorySetup : FormBase
	{
		private List<AlertCategory> alertCategoriesInternal = new List<AlertCategory>();
		private List<AlertCategory> alertCategoriesCustom = new List<AlertCategory>();

		public FormAlertCategorySetup()
		{
			InitializeComponent();
		}

		private void FormAlertCategorySetup_Load(object sender, EventArgs e)
		{
			FillGrids();
		}

		private void FillGrids(long selectedIneranlKey = 0, long selectedCustomKey = 0)
		{
			alertCategoriesCustom.Clear();
			alertCategoriesInternal.Clear();

			AlertCategories.GetAll().ForEach(x =>
			{
				if (x.IsHqCategory)
				{
					alertCategoriesInternal.Add(x);
				}
				else
				{
					alertCategoriesCustom.Add(x);
				}
			});


			alertCategoriesInternal.OrderBy(x => x.InternalName);
			alertCategoriesCustom.OrderBy(x => x.InternalName);

			FillInternalGrid(selectedIneranlKey);
			FillCustomGrid(selectedCustomKey);
		}

		private void FillInternalGrid(long selectedIneranlKey)
		{
			internalGrid.BeginUpdate();
			internalGrid.Columns.Clear();
			internalGrid.Columns.Add(new GridColumn("Description", 100));
			internalGrid.Rows.Clear();

			for (int i = 0; i < alertCategoriesInternal.Count; i++)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(alertCategoriesInternal[i].Description);
				gridRow.Tag = alertCategoriesInternal[i].Id;
				internalGrid.Rows.Add(gridRow);

				int index = internalGrid.Rows.Count - 1;
				if (selectedIneranlKey == alertCategoriesInternal[i].Id)
				{
					customGrid.SetSelected(index, true);
				}
			}

			internalGrid.EndUpdate();
		}

		private void FillCustomGrid(long selectedCustomKey)
		{
			customGrid.BeginUpdate();
			customGrid.Columns.Clear();
			customGrid.Columns.Add(new GridColumn("Description", 100));
			customGrid.Rows.Clear();

			int index = 0;
			for (int i = 0; i < alertCategoriesCustom.Count; i++)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(alertCategoriesCustom[i].Description);
				gridRow.Tag = alertCategoriesCustom[i].Id;

				customGrid.Rows.Add(gridRow);

				index = customGrid.Rows.Count - 1;
				if (selectedCustomKey != alertCategoriesCustom[i].Id)
				{
					index = 0;
				}
			}

			if (index != 0)
			{
				customGrid.SetSelected(index, true);
			}

			customGrid.EndUpdate();
		}

		private void InternalGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (!(internalGrid.Rows[e.Row].Tag is AlertCategory alertCategory))
			{
				return;
			}

			using var formAlertCategoryEdit = new FormAlertCategoryEdit(alertCategory);

			if (formAlertCategoryEdit.ShowDialog(this) == DialogResult.OK)
			{
				FillGrids();
			}
		}

		private void CustomGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (!(internalGrid.Rows[e.Row].Tag is AlertCategory alertCategory))
			{
				return;
			}

			using var formAlertCategoryEdit = new FormAlertCategoryEdit(alertCategory);

			if (formAlertCategoryEdit.ShowDialog(this) == DialogResult.OK)
			{
				FillGrids();
			}
		}

		private void CopyButton_Click(object sender, EventArgs e)
		{
			if (internalGrid.GetSelectedIndex() == -1)
			{
				ShowError("Please select an internal alert category from the list first.");

				return;
			}

			InsertCopyAlertCategory(alertCategoriesInternal[internalGrid.GetSelectedIndex()].Copy());
		}

		private void DuplicateButton_Click(object sender, EventArgs e)
		{
			if (customGrid.GetSelectedIndex() == -1)
			{
				ShowError("Please select a custom alert category from the list first.");

				return;
			}

			InsertCopyAlertCategory(alertCategoriesCustom[customGrid.GetSelectedIndex()].Copy());
		}

		private void InsertCopyAlertCategory(AlertCategory alertCategory)
		{
			alertCategory.IsHqCategory = false;
			alertCategory.Description += "(Copy)";

			//alertCat.AlertCategoryNum reflects the original pre-copied PK. After Insert this will be a new PK for the new row.
			List<AlertCategoryLink> listAlertCategoryType = AlertCategoryLinks.GetForCategory(alertCategory.Id);
			alertCategory.Id = AlertCategories.Insert(alertCategory);
			//At this point alertCat has a new PK, so we need to update and insert our new copied alertCategoryLinks
			listAlertCategoryType.ForEach(x =>
			{
				x.AlertCategoryId = alertCategory.Id;
				AlertCategoryLinks.Insert(x);
			});
			DataValid.SetInvalid(InvalidType.AlertCategories, InvalidType.AlertCategoryLinks);
			FillGrids();
		}
	}
}
