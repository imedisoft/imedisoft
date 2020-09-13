using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrProviderKeys : FormBase
	{
		public FormEhrProviderKeys()
		{
			InitializeComponent();
		}

		private void FormEhrProviderKeys_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			ehrProviderKeysGrid.BeginUpdate();
			ehrProviderKeysGrid.Columns.Clear();
			ehrProviderKeysGrid.Columns.Add(new GridColumn(Translation.Common.LastName, 80));
			ehrProviderKeysGrid.Columns.Add(new GridColumn(Translation.Common.FirstName, 80));
			ehrProviderKeysGrid.Columns.Add(new GridColumn(Translation.Common.Year, 30));
			ehrProviderKeysGrid.Columns.Add(new GridColumn(Translation.Common.Key, 100));
			ehrProviderKeysGrid.Rows.Clear();

			foreach (var ehrProviderKey in EhrProviderKeys.GetAll())
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(ehrProviderKey.LastName);
				gridRow.Cells.Add(ehrProviderKey.FirstName);
				gridRow.Cells.Add(ehrProviderKey.Year.ToString());
				gridRow.Cells.Add(ehrProviderKey.Key);
				gridRow.Tag = ehrProviderKey;

				ehrProviderKeysGrid.Rows.Add(gridRow);
			}

			ehrProviderKeysGrid.EndUpdate();
		}

		private void EhrProviderKeysGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void ehrProviderKeysGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = ehrProviderKeysGrid.SelectedRows.Count > 0;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var ehrProviderKey = new EhrProviderKey();

			using var formEhrProviderKeyEdit = new FormEhrProviderKeyEdit(ehrProviderKey);
			if (formEhrProviderKeyEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var ehrProviderKey = ehrProviderKeysGrid.SelectedTag<EhrProviderKey>();
			if (ehrProviderKey == null)
			{
				return;
			}

			using var formEhrProviderKeyEdit = new FormEhrProviderKeyEdit(ehrProviderKey);
			if (formEhrProviderKeyEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var ehrProviderKey = ehrProviderKeysGrid.SelectedTag<EhrProviderKey>();
			if (ehrProviderKey == null)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
			{
				return;
			}

			EhrProviderKeys.Delete(ehrProviderKey.Id);

			FillGrid();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}
    }
}
