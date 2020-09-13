using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormLoincs : FormBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether the form is in selection mode.
		/// </summary>
		public bool IsSelectionMode { get; set; }

		/// <summary>
		/// Gets the selected LOINC code.
		/// </summary>
		public Loinc SelectedLoinc => loincsGrid.SelectedTag<Loinc>();

		public FormLoincs()
		{
			InitializeComponent();
		}

		private void FormLoincs_Load(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				cancelButton.Text = Translation.Common.CancelWithMnemonic;
			}
			else
			{
				acceptButton.Visible = false;
			}
		}

		private void FillGrid()
		{
			loincsGrid.BeginUpdate();
			loincsGrid.Columns.Clear();
			loincsGrid.Columns.Add(new GridColumn("Loinc Code", 80));
			loincsGrid.Columns.Add(new GridColumn("Status", 80));
			loincsGrid.Columns.Add(new GridColumn("Long Name", 500));
			loincsGrid.Columns.Add(new GridColumn("UCUM Units", 100));
			loincsGrid.Columns.Add(new GridColumn("Order or Observation", 100));
			loincsGrid.Rows.Clear();

			foreach (var loinc in Loincs.GetBySearchString(searchTextBox.Text))
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(loinc.Code);
				gridRow.Cells.Add(loinc.Status);
				gridRow.Cells.Add(loinc.LongCommonName);
				gridRow.Cells.Add(loinc.UnitsUCUM);
				gridRow.Cells.Add(loinc.OrderObs);
				gridRow.Tag = loinc;

				loincsGrid.Rows.Add(gridRow);
			}

			loincsGrid.EndUpdate();
		}

		private void SearchButton_Click(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void LoincsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (IsSelectionMode)
			{
				AcceptButton_Click(this, EventArgs.Empty);
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!IsSelectionMode) return;

			if (SelectedLoinc == null)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			DialogResult = DialogResult.OK;
		}
	}
}
