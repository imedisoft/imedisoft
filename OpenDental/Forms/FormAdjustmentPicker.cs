using Imedisoft.Data;
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
    public partial class FormAdjustmentPicker : FormBase
	{
        private readonly List<Adjustment> adjustments;
		private readonly bool isUnattachedMode;

		/// <summary>
		/// Gets the selected adjustment.
		/// </summary>
		public Adjustment SelectedAdjustment => adjustmentsGrid.SelectedTag<Adjustment>();

		public FormAdjustmentPicker(long patientId, bool isUnattachedMode = false, List<Adjustment> adjustments = null)
		{
			InitializeComponent();

			this.isUnattachedMode = isUnattachedMode;
			this.adjustments = adjustments ?? Adjustments.Refresh(patientId).ToList();
		}

		private void FormAdjustmentPicker_Load(object sender, EventArgs e)
		{
			if (isUnattachedMode)
			{
				unattachedCheckBox.Checked = true;
				unattachedCheckBox.Enabled = false;
			}

			FillGrid();
		}

		private void FillGrid()
		{
			IEnumerable<Adjustment> adjustments = this.adjustments;
			if (unattachedCheckBox.Checked)
			{
				adjustments = adjustments.Where(adjustment => adjustment.ProcedureId == 0);
			}

			adjustmentsGrid.BeginUpdate();
			adjustmentsGrid.Columns.Clear();
			adjustmentsGrid.Columns.Add(new GridColumn(Translation.Common.Date, 90));
			adjustmentsGrid.Columns.Add(new GridColumn(Translation.Common.PatientId, 100));
			adjustmentsGrid.Columns.Add(new GridColumn(Translation.Common.Type, 120));
			adjustmentsGrid.Columns.Add(new GridColumn(Translation.Common.Amount, 70));
			adjustmentsGrid.Columns.Add(new GridColumn(Translation.Common.HasProc, 0, HorizontalAlignment.Center));
			adjustmentsGrid.Rows.Clear();

			foreach (var adjustment in adjustments)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(adjustment.AdjustDate.ToShortDateString());
				gridRow.Cells.Add(adjustment.PatientId.ToString());
				gridRow.Cells.Add(Definitions.GetName(DefinitionCategory.AdjTypes, adjustment.Type));
				gridRow.Cells.Add(adjustment.AdjustAmount.ToString("F"));
				gridRow.Cells.Add(adjustment.ProcedureId != 0 ? "X" : "");
				gridRow.Tag = adjustment;

				adjustmentsGrid.Rows.Add(gridRow);
			}

			adjustmentsGrid.EndUpdate();
		}

		private void UnattachedCheckBox_Click(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void AdjustmentsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			AcceptButton_Click(this, EventArgs.Empty);
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (SelectedAdjustment == null)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			DialogResult = DialogResult.OK;
		}
	}
}
