using Imedisoft.Data;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrQuarterlyKeys : FormBase
	{
		public FormEhrQuarterlyKeys()
		{
			InitializeComponent();
		}

		private void FormEhrQuarterlyKeys_Load(object sender, EventArgs e)
		{
			practiceTitleTextBox.Text = Preferences.GetString(PreferenceName.PracticeTitle);

			FillGrid();
		}

		private void FillGrid()
		{
			ehrQuarterlyKeysGrid.BeginUpdate();
			ehrQuarterlyKeysGrid.Columns.Clear();
			ehrQuarterlyKeysGrid.Columns.Add(new GridColumn(Translation.Common.Year, 50));
			ehrQuarterlyKeysGrid.Columns.Add(new GridColumn(Translation.Common.Quarter, 50));
			ehrQuarterlyKeysGrid.Columns.Add(new GridColumn(Translation.Common.Key, 100));
			ehrQuarterlyKeysGrid.Rows.Clear();

			foreach (var ehrQuarterlyKey in EhrQuarterlyKeys.Refresh(0))
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(ehrQuarterlyKey.Year.ToString());
				gridRow.Cells.Add(ehrQuarterlyKey.Quarter.ToString());
				gridRow.Cells.Add(ehrQuarterlyKey.Key);
				gridRow.Tag = ehrQuarterlyKey;

				ehrQuarterlyKeysGrid.Rows.Add(gridRow);
			}

			ehrQuarterlyKeysGrid.EndUpdate();
		}

		private void EhrQuarterlyKeysGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void EhrQuarterlyKeysGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled
				= ehrQuarterlyKeysGrid.SelectedTag<EhrQuarterlyKey>() != null;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var ehrQuarterlyKey = new EhrQuarterlyKey();

			using var formEhrQuarterlyKeyEdit = new FormEhrQuarterlyKeyEdit(ehrQuarterlyKey);
			if (formEhrQuarterlyKeyEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var ehrQuarterlyKey = ehrQuarterlyKeysGrid.SelectedTag<EhrQuarterlyKey>();
			if (ehrQuarterlyKey == null)
			{
				return;
			}

			using var formEhrQuarterlyKeyEdit = new FormEhrQuarterlyKeyEdit(ehrQuarterlyKey);
			if (formEhrQuarterlyKeyEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var ehrQuarterlyKey = ehrQuarterlyKeysGrid.SelectedTag<EhrQuarterlyKey>();
			if (ehrQuarterlyKey == null)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
			{
				return;
			}

			EhrQuarterlyKeys.Delete(ehrQuarterlyKey);

			FillGrid();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}
    }
}
