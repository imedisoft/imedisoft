using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoCodes : FormBase
	{
		private bool hasChanges;

		public FormAutoCodes() 
			=> InitializeComponent();

		private void FormAutoCodes_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			AutoCodes.RefreshCache();

			autoCodesGrid.BeginUpdate();
			autoCodesGrid.Columns.Clear();
			autoCodesGrid.Columns.Add(new GridColumn(Translation.Common.Description, 200));
			autoCodesGrid.Rows.Clear();

			foreach (var autoCode in AutoCodes.GetAll())
			{
				autoCodesGrid.Rows.Add(new GridRow(autoCode.Description)
				{
					Tag = autoCode
				});
			}

			autoCodesGrid.EndUpdate();
		}

		private void AutoCodesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void AutoCodesGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = autoCodesGrid.Rows.Count > 0;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var autoCode = new AutoCode();

			using var formAutoCodeEdit = new FormAutoCodeEdit(autoCode);
			if (formAutoCodeEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var autoCode = autoCodesGrid.SelectedTag<AutoCode>();
			if (autoCode == null)
			{
				return;
			}

			using var formAutoCodeEdit = new FormAutoCodeEdit(autoCode);
			if (formAutoCodeEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var autoCode = autoCodesGrid.SelectedTag<AutoCode>();
			if (autoCode == null)
			{
                return;
            }

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
            {
				return;
            }

            try
			{
				AutoCodes.Delete(autoCode);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			hasChanges = true;

			FillGrid();
		}

		private void CancelButton_Click(object sender, EventArgs e)
        {
			Close();
		}

		private void FormAutoCode_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (hasChanges)
			{
				CacheManager.RefreshGlobal(nameof(InvalidType.AutoCodes));
			}
		}
    }
}
