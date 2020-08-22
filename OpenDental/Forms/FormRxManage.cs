using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormRxManage : FormBase
	{
		private readonly Patient patient;
		private List<RxPat> rxPats;

		public FormRxManage(Patient patient)
		{
			InitializeComponent();

			this.patient = patient;
		}

		private void FormRxManage_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			rxPatGrid.BeginUpdate();
			rxPatGrid.ListGridColumns.Clear();
			rxPatGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Date, 70));
			rxPatGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Drug, 140));
			rxPatGrid.ListGridColumns.Add(new GridColumn(Translation.Rx.SIG, 70) { IsWidthDynamic = true });
			rxPatGrid.ListGridColumns.Add(new GridColumn(Translation.Rx.DispenseAbbr, 70));
			rxPatGrid.ListGridColumns.Add(new GridColumn(Translation.Rx.Refills, 70));
			rxPatGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Provider, 70));
			rxPatGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Notes, 70) { IsWidthDynamic = true });
			rxPatGrid.ListGridColumns.Add(new GridColumn(Translation.Common.MissingInfo, 70) { IsWidthDynamic = true });
			rxPatGrid.ListGridRows.Clear();

			rxPats = RxPats.GetAllForPat(patient.PatNum);
			rxPats.Sort((rx1, rx2) =>
			{
				if (rx1.RxDate != rx2.RxDate)
				{
					return rx2.RxDate.CompareTo(rx1.RxDate);
				}

				return rx2.Id.CompareTo(rx1.Id);
			});

			foreach (var rxPat in rxPats)
			{
				var gridRow = new GridRow();

				gridRow.Cells.Add(rxPat.RxDate.ToShortDateString());
				gridRow.Cells.Add(rxPat.Drug);
				gridRow.Cells.Add(rxPat.Sig);
				gridRow.Cells.Add(rxPat.Disp);
				gridRow.Cells.Add(rxPat.Refills);
				gridRow.Cells.Add(Providers.GetAbbr(rxPat.ProvNum));
				gridRow.Cells.Add(rxPat.Notes);
				gridRow.Cells.Add(OpenDental.SheetPrinting.ValidateRxForSheet(rxPat));
				gridRow.Tag = rxPat;

				rxPatGrid.ListGridRows.Add(gridRow);
			}
			rxPatGrid.EndUpdate();
		}

		private void RxPatGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var rxPat = rxPatGrid.SelectedTag<RxPat>();
			if (rxPat == null)
            {
				return;
            }

			if (rxPatGrid.GetSelectedIndex() == -1)
			{
				return;
			}

			using var formRxEdit = new FormRxEdit(patient, rxPat);

			if (formRxEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void PrintSelectedButton_Click(object sender, EventArgs e)
		{
			var rxPats = rxPatGrid.SelectedTags<RxPat>();
			if (rxPats.Count == 0)
			{
				ShowError(Translation.Rx.AtleastOnePrescriptionMustBeSeleted);

				return;
			}

			if (PrinterSettings.InstalledPrinters.Count == 0)
			{
				ShowError(Translation.Common.NoPrintersInstalled);

				return;
			}

			if (rxPats.Count == 1)
			{
				// old way of printing one rx
				// This logic is an exact copy of FormRxEdit.butPrint_Click()'s logic.  If this is updated, that method needs to be updated as well.

				var sheetDef = SheetDefs.GetSheetsDefault(SheetTypeEnum.Rx, Clinics.ClinicNum);
				var sheet = SheetUtil.CreateSheet(sheetDef, patient.PatNum);

				SheetParameter.SetParameter(sheet, "RxNum", rxPats[0].Id);
				SheetFiller.FillFields(sheet);
				SheetUtil.CalculateHeights(sheet);

				OpenDental.SheetPrinting.PrintRx(sheet, rxPats[0]);
			}
			else
			{ 
				// multiple rx selected
				// Print batch list of rx

				OpenDental.SheetPrinting.PrintMultiRx(rxPats);
			}
		}

		private void NewButton_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.RxCreate))
			{
				return;
			}

			using var formRxSelect = new FormRxSelect(patient);

			if (formRxSelect.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			SecurityLogs.MakeLogEntry(Permissions.RxCreate, patient.PatNum, Translation.Rx.CreatedPrescription);

			FillGrid();
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
