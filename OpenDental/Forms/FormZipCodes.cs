using Imedisoft.Data.Cache;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormZipCodes : FormBase
	{
		private bool hasChanges;

		public FormZipCodes() 
			=> InitializeComponent();

		private void FormZipCodes_Load(object sender, EventArgs e) 
			=> FillGrid();

		private void FillGrid()
		{
			ZipCodes.RefreshCache();

			zipCodesGrid.BeginUpdate();
			zipCodesGrid.Columns.Clear();
			zipCodesGrid.Columns.Add(new GridColumn("Zip", 75));
			zipCodesGrid.Columns.Add(new GridColumn("City", 270));
			zipCodesGrid.Columns.Add(new GridColumn("State", 50));
			zipCodesGrid.Columns.Add(new GridColumn("Frequent", 80));
			zipCodesGrid.Rows.Clear();

			foreach (var zipCode in ZipCodes.GetDeepCopy())
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(zipCode.ZipCodeDigits);
				gridRow.Cells.Add(zipCode.City);
				gridRow.Cells.Add(zipCode.State);
				gridRow.Cells.Add((zipCode.IsFrequent ? "X" : ""));
				gridRow.Tag = zipCode;

				zipCodesGrid.Rows.Add(gridRow);
			}

			zipCodesGrid.EndUpdate();
		}

		private void ZipCodesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var zipCode = zipCodesGrid.SelectedTag<ZipCode>();
			if (zipCode == null)
			{
				return;
			}

			using var formZipCodeEdit = new FormZipCodeEdit(zipCode);

			if (formZipCodeEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			FillGrid();
		}

		private void ZipCodesGrid_SelectionCommitted(object sender, EventArgs e)
		{
			deleteButton.Enabled = zipCodesGrid.SelectedRows.Count > 0;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var zipCode = zipCodesGrid.SelectedTag<ZipCode>();
			if (zipCode == null)
			{
				MessageBox.Show(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
            {
				return;
            }

			hasChanges = true;

			ZipCodes.Delete(zipCode);

			FillGrid();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var zipCode = new ZipCode();

			using var formZipCodeEdit = new FormZipCodeEdit(zipCode);

			if (formZipCodeEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			FillGrid();
		}

		private void CloseButton_Click(object sender, EventArgs e) 
			=> Close();

		private void FormZipCodes_Closing(object sender, CancelEventArgs e)
		{
			if (hasChanges)
			{
				CacheManager.RefreshGlobal(nameof(InvalidType.ZipCodes));
			}
		}
    }
}
