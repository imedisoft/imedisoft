using Imedisoft.Data;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormRxNorms : FormBase
	{
		/// <summary>
		/// When this window is used for selecting an RxNorm (medication.RxCui), then use must click OK, None, or double click in grid. 
		/// In those cases, this field will have a value. 
		/// If None was clicked, it will be null.
		/// </summary>
		public RxNorm SelectedRxNorm
		{
            get
            {
				if (rxNormsGrid.SelectedIndices.Length > 0 &&
					rxNormsGrid.SelectedGridRows[0].Tag is RxNorm rxNorm)
                {
					return rxNorm;
                }

				return null;
            }
		}

		public List<RxNorm> ListSelectedRxNorms 
			=> rxNormsGrid.SelectedTags<RxNorm>();

		public bool IsSelectionMode { get; set; }

		public bool IsMultiSelectMode { get; set; }

		public string InitSearchCodeOrDescript { get; set; }


		public FormRxNorms()
		{
			InitializeComponent();
		}

		private void FormRxNorms_Load(object sender, EventArgs e)
		{
			if (!IsSelectionMode && !IsMultiSelectMode)
			{
				noneButton.Visible = false;
				acceptButton.Visible = false;
				cancelButton.Text = Translation.Common.Close;
			}

			if (IsMultiSelectMode)
			{
				rxNormsGrid.SelectionMode = GridSelectionMode.MultiExtended;
			}

			ignoreCheckBox.Checked = true;
			if (!string.IsNullOrWhiteSpace(InitSearchCodeOrDescript))
			{
				codeTextBox.Text = InitSearchCodeOrDescript;
				if (InitSearchCodeOrDescript != Regex.Replace(InitSearchCodeOrDescript, "[0-9]", ""))
				{
					ignoreCheckBox.Checked = false;
				}

				FillGrid(true); // Try exact match first.
				if (rxNormsGrid.ListGridRows.Count == 0)
				{
					// If no exact matches, then show similar matches.
					FillGrid(false);
				}
			}
		}

		private void FormRxNorms_Shown(object sender, EventArgs e)
		{
			if (RxNorms.IsRxNormTableSmall())
			{
				if (Confirm(Translation.Rx.IncompleteRxNormsConfirmDownloadRxNormCodes))
				{
					using var formCodeSystemsImport = new FormCodeSystemsImport(CodeSystemName.RXNORM);

					formCodeSystemsImport.ShowDialog(this);
				}
			}
		}

		private void SearchSimilarButton_Click(object sender, EventArgs e) 
			=> FillGrid(false);

		private void SearchExactButton_Click(object sender, EventArgs e) 
			=> FillGrid(true);

		private void ClearButton_Click(object sender, EventArgs e) 
			=> codeTextBox.Text = "";

		private void FillGrid(bool exactMatch)
		{
			Cursor = Cursors.WaitCursor;

			var rxNorms = RxNorms.GetListByCodeOrDesc(codeTextBox.Text, exactMatch, ignoreCheckBox.Checked);
			var rxCuisMedication = Medications.GetWhere(x => x.RxCui != "").Select(x => x.RxCui.ToString()).Distinct().ToList();
			var rxCuisPatientMedication = MedicationPats.GetForRxCuis(rxNorms.Select(x => x.RxCui).ToList()).Select(x => x.RxCui.ToString()).ToList();

			rxNormsGrid.BeginUpdate();
			rxNormsGrid.ListGridColumns.Clear();
			rxNormsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Code, 80));
			rxNormsGrid.ListGridColumns.Add(new GridColumn("InMedList", 60, HorizontalAlignment.Center));
			rxNormsGrid.ListGridColumns.Add(new GridColumn("MedCount", 60, HorizontalAlignment.Center));
			rxNormsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Description, 80) { IsWidthDynamic = true });
			rxNormsGrid.ListGridRows.Clear();

			foreach (var rxNorm in rxNorms)
			{
				var gridRow = new GridRow();

				gridRow.Cells.Add(rxNorm.RxCui);
				gridRow.Cells.Add(rxCuisMedication.Exists(x => x == rxNorm.RxCui) ? "X" : "");
				gridRow.Cells.Add(rxCuisPatientMedication.FindAll(x => x == rxNorm.RxCui).Count.ToString());
				gridRow.Cells.Add(rxNorm.Description);
				gridRow.Tag = rxNorm;

				rxNormsGrid.ListGridRows.Add(gridRow);
			}

			rxNormsGrid.EndUpdate();
			rxNormsGrid.ScrollValue = 0;

			Cursor = Cursors.Default;
		}

		private void RxNormsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (IsSelectionMode)
			{
				AcceptButton_Click(this, EventArgs.Empty);
			}
		}

        private void NoneButton_Click(object sender, EventArgs e)
		{
			rxNormsGrid.SetSelected(false);

			DialogResult = DialogResult.OK;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (rxNormsGrid.GetSelectedIndex() < 0)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			DialogResult = DialogResult.OK;
		}
	}
}
