using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormRxSelect : FormBase
	{
		private readonly Patient patient;

        /// <summary>
		/// This is set for any medical orders that are selected.
		/// </summary>
        public long MedicalOrderId { get; private set; }

		public FormRxSelect(Patient patient)
		{
			InitializeComponent();

			this.patient = patient;
		}

		private void FormRxSelect_Load(object sender, EventArgs e)
		{
			if (Prefs.GetBool(PrefName.ShowFeatureEhr))
			{
				// We cannot allow blank prescription when using EHR, because each prescription created in this window must have an RxCui.
				// If we allowed blank, we would not know where to pull the RxCui from.
				blankButton.Visible = false;

				instructionsLabel.Text = Translation.Rx.PleaseSelectPrescriptionFromTheList;
			}

			SearchButton_Click(this, EventArgs.Empty);
		}

		private void FillGrid()
		{
			IEnumerable<RxDef> rxDefs = RxDefs.Refresh();

			if (drugTextBox.Text != "")
			{
				var searchTerms = drugTextBox.Text.Split(' ');

				foreach (var searchTerm in searchTerms)
				{
					rxDefs = rxDefs.Where(
						rxDef => rxDef.Drug.ToLower().Contains(searchTerm.ToLower()));
				}
			}

			if (dispTextBox.Text != "")
			{
				var searchTerms = dispTextBox.Text.Split(' ');

				foreach (var searchTerm in searchTerms)
				{
					rxDefs = rxDefs.Where(
						rxDef => rxDef.Disp.ToLower().Contains(searchTerm.ToLower()));
				}
			}

			if (checkControlledOnly.Checked)
			{
				rxDefs = rxDefs.Where(rxDef => rxDef.IsControlled);
			}

			rxDefsGrid.BeginUpdate();
			rxDefsGrid.ListGridColumns.Clear();
			rxDefsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Drug, 140));
			rxDefsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Controlled, 70, HorizontalAlignment.Center));
			rxDefsGrid.ListGridColumns.Add(new GridColumn(Translation.Rx.SIG, 250));
			rxDefsGrid.ListGridColumns.Add(new GridColumn(Translation.Rx.DispenseAbbr, 70));
			rxDefsGrid.ListGridColumns.Add(new GridColumn(Translation.Rx.Refills, 70));
			rxDefsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Notes, 300));
			rxDefsGrid.ListGridRows.Clear();

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

				rxDefsGrid.ListGridRows.Add(gridRow);
			}

			rxDefsGrid.EndUpdate();
		}

		private void RxDefsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e) 
			=> RxSelected();

		private void SearchButton_Click(object sender, EventArgs e) 
			=> FillGrid();

        private void RxSelected()
		{
			var rxDef = rxDefsGrid.SelectedTag<RxDef>();
			if (rxDef == null)
            {
				return;
            }

			if (Prefs.GetBool(PrefName.ShowFeatureEhr) && rxDef.RxCui == 0)
			{
				string error = Translation.Rx.SelectedPrescriptionIsMissingRxNorm;

				if (!Security.IsAuthorized(Permissions.RxEdit, true))
				{
					ShowError(error);
				}
				else if (Confirm(error + "\r\n\r\n" + Translation.Rx.ConfirmEditRxNormInRxTemplate))
				{
					using var formRxDefEdit = new FormRxDefEdit(rxDef);

					formRxDefEdit.ShowDialog(this);
				}
			}

			if (!RxAlertL.DisplayAlerts(patient.PatNum, rxDef.Id)) return;

            var rxPat = new RxPat
            {
                RxDate = DateTime.Today,
                PatNum = patient.PatNum,
                ClinicNum = patient.ClinicNum,
                Drug = rxDef.Drug,
                IsControlled = rxDef.IsControlled,
                Sig = rxDef.Sig,
                Disp = rxDef.Disp,
                Refills = rxDef.Refills,
                SendStatus = Prefs.GetBool(PrefName.RxSendNewToQueue) ? RxSendStatus.InElectQueue : RxSendStatus.Unsent,
                PatientInstruction = rxDef.PatientInstruction
            };

			// Notes not copied: we don't want these kinds of notes cluttering things

			if (Prefs.GetBool(PrefName.RxHasProc) && (Clinics.ClinicId == 0 || Clinics.GetById(Clinics.ClinicId).HasProcedureOnRx))
			{
				rxPat.IsProcRequired = rxDef.IsProcRequired;
			}

			using var formRxEdit = new FormRxEdit(patient, rxPat);

            if (formRxEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			bool isProvOrder = Security.CurrentUser.ProviderId != 0;

			MedicalOrderId = MedicationPats.InsertOrUpdateMedOrderForRx(rxPat, rxDef.RxCui, isProvOrder);//RxDefCur.RxCui can be 0.

            EhrMeasureEvents.Insert(new EhrMeasureEvent
			{
				DateTEvent = DateTime.Now,
				EventType = EhrMeasureEventType.CPOE_MedOrdered,
				PatNum = patient.PatNum,
				MoreInfo = "",
				FKey = MedicalOrderId
			});

			DialogResult = DialogResult.OK;
		}

		private void BlankButton_Click(object sender, EventArgs e)
		{
            var rxPat = new RxPat
            {
                RxDate = DateTime.Today,
                PatNum = patient.PatNum,
                ClinicNum = patient.ClinicNum,
                SendStatus = Prefs.GetBool(PrefName.RxSendNewToQueue) ? RxSendStatus.InElectQueue : RxSendStatus.Unsent
            };

			using var formRxEdit = new FormRxEdit(patient, rxPat);

            if (formRxEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			DialogResult = DialogResult.OK;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (rxDefsGrid.GetSelectedIndex() == -1)
			{
				ShowError(
					Prefs.GetBool(PrefName.ShowFeatureEhr) ? 
						Translation.Rx.PleaseSelectRxFirst : 
						Translation.Rx.PleaseSelectRxFirstOrClickBlank);

				return;
			}

			RxSelected();
		}
	}
}
