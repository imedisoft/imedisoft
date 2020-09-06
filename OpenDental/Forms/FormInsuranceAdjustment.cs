using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormInsuranceAdjustment : FormBase
	{
		private readonly ClaimProc claimProc;

		public FormInsuranceAdjustment(ClaimProc claimProc)
		{
			InitializeComponent();

			this.claimProc = claimProc;
		}

		private void FormInsuranceAdjustment_Load(object sender, EventArgs e)
		{
			dateTextBox.Text = claimProc.ProcDate.ToShortDateString();
			insuranceUsedTextBox.Text = claimProc.InsPayAmt.ToString("F");
			deductibleUsedTextBox.Text = claimProc.DedApplied.ToString("F");
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (claimProc.ClaimProcNum == 0)
			{
				DialogResult = DialogResult.Cancel;

				return;
			}

			if (!Confirm(Translation.Common.ConfirmDelete))
			{
				return;
			}

			ClaimProcs.Delete(claimProc);

			InsEditPatLogs.MakeLogEntry(null, claimProc, InsEditPatLogType.Adjustment);

			DialogResult = DialogResult.OK;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!DateTime.TryParse(dateTextBox.Text, out var date))
            {
				ShowError(Translation.Common.PleaseEnterValidDate);

				return;
            }

			if (!double.TryParse(insuranceUsedTextBox.Text, out var insuranceUsed) ||
				!double.TryParse(deductibleUsedTextBox.Text, out var deductibleUsed))
            {
				ShowError(Translation.Common.PleaseEnterValidAmount);

				return;
            }

			claimProc.ProcDate = date;
			claimProc.InsPayAmt = insuranceUsed;
			claimProc.DedApplied = deductibleUsed;

			ClaimProcs.Save(claimProc);

			DialogResult = DialogResult.OK;
		}
	}
}
