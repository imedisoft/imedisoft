using OpenDental;
using OpenDentBusiness;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormFeeEdit : FormBase
	{
		private readonly Fee fee;
		private FeeSched feeSchedule;

		public FormFeeEdit(Fee fee)
		{
			InitializeComponent();

			this.fee = fee;
		}

		private void FormFeeEdit_Load(object sender, EventArgs e)
		{
			feeSchedule = FeeScheds.GetFirstOrDefault(x => x.FeeSchedNum == fee.FeeSched);
			if (!FeeL.CanEditFee(feeSchedule, fee.ProvNum, fee.ClinicNum))
			{
				DialogResult = DialogResult.Cancel;

				Close();

				return;
			}

			Location = new Point(Location.X - 190, Location.Y - 20);

			feeTextBox.Text = fee.Amount.ToString("F");
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var feeAmount = 0d;
			if (!string.IsNullOrEmpty(feeTextBox.Text) && !double.TryParse(feeTextBox.Text, out feeAmount))
            {
				ShowError("Please enter a valid amount.");

				return;
			}

			var dateModified = fee.SecDateTEdit;

			if (string.IsNullOrEmpty(feeTextBox.Text))
			{
				Fees.Delete(fee);
			}
			else
			{
				fee.Amount = feeAmount;

				Fees.Save(fee);
			}

			SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit, 0, 
				"Procedure: " + ProcedureCodes.GetStringProcCode(fee.CodeNum) + ", " + 
				"Fee: " + fee.Amount.ToString("c") + ", Fee Schedule: " + FeeScheds.GetDescription(fee.FeeSched) + ". " + 
				"Manual edit in Edit Fee window.", fee.CodeNum, DateTime.MinValue);

			SecurityLogs.MakeLogEntry(Permissions.LogFeeEdit, 0, "Fee Updated", fee.FeeNum, dateModified);

			DialogResult = DialogResult.OK;
		}
	}
}
