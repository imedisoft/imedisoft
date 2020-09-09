using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDental.UI;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormIcd9s : FormBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether the form is in selection mode.
		/// </summary>
		public bool IsSelectionMode { get; set; }

		/// <summary>
		/// Gets the selected ICD-9 code.
		/// </summary>
		public Icd9 SelectedIcd9 => icd9Grid.SelectedTag<Icd9>();

		public FormIcd9s()
		{
			InitializeComponent();
		}

		private void FormIcd9s_Load(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				cancelButton.Text = "Cancel";
			}
			else
			{
				acceptButton.Visible = false;
			}
		}

		private void FillGrid()
		{
			icd9Grid.BeginUpdate();
			icd9Grid.Columns.Clear();
			icd9Grid.Columns.Add(new GridColumn("ICD-9", 100));
			icd9Grid.Columns.Add(new GridColumn(Translation.Common.Description, 500));
			icd9Grid.Rows.Clear();

			foreach (var icd10 in Icd9s.GetByCodeOrDescription(searchTextBox.Text))
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(icd10.Code);
				gridRow.Cells.Add(icd10.Description);
				gridRow.Tag = icd10;

				icd9Grid.Rows.Add(gridRow);
			}

			icd9Grid.EndUpdate();
		}

		private void SearchButton_Click(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void Icd9Grid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			AcceptButton_Click(this, EventArgs.Empty);
		}

		private void ImportButton_Click(object sender, EventArgs e)
		{
			using var formCodeSystemsImport = new FormCodeSystemsImport();
			if (formCodeSystemsImport.ShowDialog(this) != DialogResult.OK)
            {
				return;
            }
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!IsSelectionMode) return;

			if (SelectedIcd9 == null)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			DialogResult = DialogResult.OK;
		}
	}
}
