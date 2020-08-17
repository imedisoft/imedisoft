using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormApptBreakRequired : FormBase
	{
		public ProcedureCode SelectedProcedureCode { get; private set; }

		public FormApptBreakRequired() => InitializeComponent();

		private void MissedButton_Click(object sender, EventArgs e)
		{
			SelectedProcedureCode = ProcedureCodes.GetProcCode("D9986");

			DialogResult = DialogResult.OK;
		}

		private void CancelledButton_Click(object sender, EventArgs e)
		{
			SelectedProcedureCode = ProcedureCodes.GetProcCode("D9987");

			DialogResult = DialogResult.OK;
		}
	}
}