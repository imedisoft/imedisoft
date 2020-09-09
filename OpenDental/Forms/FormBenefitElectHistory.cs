using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Forms
{
    public partial class FormBenefitElectHistory : FormBase
	{
		private readonly long insPlanId;
		private readonly long patPlanId;
		private readonly long insSubscriberId;
		private readonly long subPatientId;
		private readonly long carrierId;

		public List<Benefit> Benefits { get; set; }
		
		public FormBenefitElectHistory(long insPlanId, long patPlanId, long insSubscriberId, long subPatientId, long carrierId)
		{
			InitializeComponent();

			this.insPlanId = insPlanId;
			this.patPlanId = patPlanId;
			this.insSubscriberId = insSubscriberId;
			this.subPatientId = subPatientId;
			this.carrierId = carrierId;
		}

		private void FormBenefitElectHistory_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			var transactions = Etranss.GetList270ForPlan(insPlanId, insSubscriberId);

			IEnumerable<long> GetPatientIds()
            {
				foreach (var transaction in transactions)
                {
					if (transaction.PatNum == 0) continue;

					yield return transaction.PatNum;
                }

				yield return subPatientId;
            }

			var patients = Patients.GetMultPats(GetPatientIds().Distinct());

			etransGrid.BeginUpdate();
			etransGrid.Columns.Clear();
			etransGrid.Columns.Add(new GridColumn(Translation.Common.Date, 100));
			etransGrid.Columns.Add(new GridColumn(Translation.Common.Patient, 100));
			etransGrid.Columns.Add(new GridColumn(Translation.Common.Response, 100));
			etransGrid.Rows.Clear();

			foreach (var transaction in transactions)
			{
				// All old 270s do not have a patient ID set, so they were subscriber request.
				string patientName = Patients.GetOnePat(patients, transaction.PatNum == 0 ? subPatientId : transaction.PatNum).GetNameLFnoPref();

				var gridRow = new GridRow();
				gridRow.Cells.Add(transaction.DateTimeTrans.ToShortDateString());
				gridRow.Cells.Add(patientName);
				gridRow.Cells.Add(transaction.Note);
				gridRow.Tag = transaction;

				etransGrid.Rows.Add(gridRow);
			}

			etransGrid.EndUpdate();
		}

		private void EtransGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var etrans = etransGrid.SelectedTag<Etrans>();
			if (etrans == null)
            {
				return;
            }

			if (etrans.Etype == EtransType.Eligibility_CA)
			{
                using var formEtransEdit = new FormEtransEdit
                {
                    EtransCur = etrans
                };

                formEtransEdit.ShowDialog(this);
			}
			else
			{
				var errors = X271.ValidateSettings();
				if (!string.IsNullOrEmpty(errors))
				{
					ShowError(errors);

					return;
				}

				bool isDependent = etrans.PatNum != 0 && subPatientId != etrans.PatNum; // Old rows will be 0, but when 0 then request was for subscriber.

				var carrier = Carriers.GetCarrier(carrierId);

                using var formEtrans270Edit = new FormEtrans270Edit(patPlanId, insPlanId, insSubscriberId, isDependent, subPatientId, carrier.IsCoinsuranceInverted)
                {
                    EtransCur = etrans,
                    benList = Benefits
                };

                formEtrans270Edit.ShowDialog(this);
			}

			FillGrid();
		}
	}
}
