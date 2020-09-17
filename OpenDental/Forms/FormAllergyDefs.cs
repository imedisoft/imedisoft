using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAllergyDefs : FormBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether the form is in selection mode.
		/// </summary>
		public bool IsSelectionMode { get; set; }

		/// <summary>
		/// Gets the selected allergy definition.
		/// </summary>
		public AllergyDef SelectedAllergyDef => allergiesGrid.SelectedTag<AllergyDef>();

		public FormAllergyDefs()
		{
			InitializeComponent();
		}

		private void FormAllergyDefs_Load(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				acceptButton.Visible = true;
				cancelButton.Text = Translation.Common.CancelWithMnemonic;
			}

			FillGrid();
		}

		private void FillGrid()
		{
			allergiesGrid.BeginUpdate();
			allergiesGrid.Columns.Clear();
			allergiesGrid.Columns.Add(new GridColumn(Translation.Common.Definition, 160));
			allergiesGrid.Columns.Add(new GridColumn(Translation.Common.Hidden, 60));
			allergiesGrid.Rows.Clear();

			foreach (var allergyDef in AllergyDefs.GetAll(showHiddenCheckBox.Checked))
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(allergyDef.Description);
				gridRow.Cells.Add(allergyDef.IsHidden ? "X" : "");
				gridRow.Tag = allergyDef;

				allergiesGrid.Rows.Add(gridRow);
			}

			allergiesGrid.EndUpdate();
		}

		private void ShowHiddenCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void AllergiesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (IsSelectionMode)
			{
				AcceptButton_Click(this, EventArgs.Empty);

				return;
			}

			EditButton_Click(this, EventArgs.Empty);
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var allergyDef = new AllergyDef();

			using var formAllergyDefEdit = new FormAllergyDefEdit(allergyDef);
			if (formAllergyDefEdit.ShowDialog() != DialogResult.OK)
            {
				return;
            }

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var allergyDef = SelectedAllergyDef;
			if (allergyDef == null)
			{
				return;
			}

			using var formAllergyDefEdit = new FormAllergyDefEdit(allergyDef);
			if (formAllergyDefEdit.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var allergyDef = SelectedAllergyDef;
			if (allergyDef == null)
			{
				return;
			}

			if (AllergyDefs.IsInUse(allergyDef.Id))
			{
				ShowError("Cannot delete allergies in use.");

				return;
			}

			AllergyDefs.Delete(allergyDef.Id);

			FillGrid();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (SelectedAllergyDef == null)
			{
				MessageBox.Show(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			DialogResult = DialogResult.OK;
		}
    }
}
