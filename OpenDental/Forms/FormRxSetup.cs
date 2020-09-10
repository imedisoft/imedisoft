using Imedisoft.Data;
using Imedisoft.Data.Cache;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormRxSetup : FormBase
	{
		public FormRxSetup() => InitializeComponent();

		private void FormRxSetup_Load(object sender, EventArgs e)
		{
			procCodeRequiredCheckBox.Checked = Preferences.GetBool(PreferenceName.RxHasProc);

			FillGrid();
		}

		private void FillGrid()
		{
			var rxDefs = RxDefs.Refresh();

			rxDefsGrid.BeginUpdate();
			rxDefsGrid.Columns.Clear();
			rxDefsGrid.Columns.Add(new GridColumn(Translation.Common.Drug, 140));
			rxDefsGrid.Columns.Add(new GridColumn(Translation.Common.Controlled, 70, HorizontalAlignment.Center));
			rxDefsGrid.Columns.Add(new GridColumn(Translation.Rx.SIG, 320));
			rxDefsGrid.Columns.Add(new GridColumn(Translation.Rx.DispenseAbbr, 70));
			rxDefsGrid.Columns.Add(new GridColumn(Translation.Rx.Refills, 70));
			rxDefsGrid.Columns.Add(new GridColumn(Translation.Common.Notes, 300));
			rxDefsGrid.Rows.Clear();

			foreach (var rxDef in rxDefs)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(rxDef.Drug);
				gridRow.Cells.Add(rxDef.IsControlled ? "X" : "");
				gridRow.Cells.Add(rxDef.Sig);
				gridRow.Cells.Add(rxDef.Disp);
				gridRow.Cells.Add(rxDef.Refills);
				gridRow.Cells.Add(rxDef.Notes);
				gridRow.Tag = rxDef;

				rxDefsGrid.Rows.Add(gridRow);
			}

			rxDefsGrid.EndUpdate();
		}

		private void ProcCodeRequiredCheckBox_Click(object sender, EventArgs e)
		{
			if (Preferences.Set(PreferenceName.RxHasProc, procCodeRequiredCheckBox.Checked))
			{
				CacheManager.RefreshGlobal(nameof(InvalidType.Prefs));
			}
		}

		private void RxDefsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var rxDef = rxDefsGrid.SelectedTag<RxDef>();
			if (rxDef==null)
            {
				return;
            }

			using var formRxDefEdit = new FormRxDefEdit(rxDef);

			formRxDefEdit.ShowDialog(this);

			FillGrid();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var newRexDef = new RxDef();

			using var formRxDefEdit = new FormRxDefEdit(newRexDef);

            if (formRxDefEdit.ShowDialog(this) == DialogResult.OK)
            {
				FillGrid();
			}
		}

		private void DuplicateButton_Click(object sender, EventArgs e)
		{
			var rxDef = rxDefsGrid.SelectedTag<RxDef>();
			if (rxDef == null)
			{
				return;
			}

			var newRxDef = rxDef.Copy();
			newRxDef.Id = 0;

			using var formRxDefEdit = new FormRxDefEdit(newRxDef);

			if (formRxDefEdit.ShowDialog(this) == DialogResult.OK)
			{
				FillGrid();
			}
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var rxDef = rxDefsGrid.SelectedTag<RxDef>();
			if (rxDef == null)
			{
				return;
			}

			if (Confirm(Translation.Rx.ConfirmDeleteRxTemplate))
			{
				RxDefs.Delete(rxDef);

				FillGrid();
			}
		}

		private void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}
