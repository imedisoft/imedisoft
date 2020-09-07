using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDental.UI;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormIcd10s : FormBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether the form is in selection mode.
		/// </summary>
		public bool IsSelectionMode { get; set; }

		/// <summary>
		/// Gets the selected ICD-10 code.
		/// </summary>
		public Icd10 SelectedIcd10 => icd10Grid.SelectedTag<Icd10>();

		public FormIcd10s()
		{
			InitializeComponent();
		}

		private void FormIcd10s_Load(object sender, EventArgs e)
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

		private void SearchButton_Click(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			icd10Grid.BeginUpdate();
			icd10Grid.ListGridColumns.Clear();
			icd10Grid.ListGridColumns.Add(new GridColumn("ICD-10", 100));
			icd10Grid.ListGridColumns.Add(new GridColumn(Translation.Common.Description, 500));
			icd10Grid.ListGridRows.Clear();

			foreach (var icd10 in Icd10s.GetByCodeOrDescription(searchTextBox.Text))
            {
				var gridRow = new GridRow();
				gridRow.Cells.Add(icd10.Code);
				gridRow.Cells.Add(icd10.Description);
				gridRow.Tag = icd10;

				icd10Grid.ListGridRows.Add(gridRow);
			}

			icd10Grid.EndUpdate();
		}

		private void Icd10Grid_CellDoubleClick(object sender, ODGridClickEventArgs e)
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

			if (SelectedIcd10 == null)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			DialogResult = DialogResult.OK;
		}
	}
}
