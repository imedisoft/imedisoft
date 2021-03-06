using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormRxAlertEdit : FormBase
	{
		private readonly RxAlert rxAlert;
		private readonly RxDef rxDef;

		public FormRxAlertEdit(RxAlert rxAlert, RxDef rxDef)
		{
			InitializeComponent();

			this.rxAlert = rxAlert;
			this.rxDef = rxDef;
		}

		private void FormRxAlertEdit_Load(object sender, EventArgs e)
		{
			if (rxAlert.DiseaseDefId > 0)
			{
				nameLabel.Text = Translation.Rx.IfPatientAlreadyHasThisProblem;
				nameTextBox.Text = ProblemDefinitions.GetName(rxAlert.DiseaseDefId);
			}

			if (rxAlert.AllergyDefId > 0)
			{
				nameLabel.Text = Translation.Rx.IfPatientAlreadyHasThisAllergy;
				nameTextBox.Text = AllergyDefs.GetById(rxAlert.AllergyDefId).Description;
			}

			if (rxAlert.MedicationId > 0)
			{
				nameLabel.Text = Translation.Rx.IfPatientIsAlreadyTakingThisMedication;
				nameTextBox.Text = Medications.GetByIdNoCache(rxAlert.MedicationId).Name;
			}

			drugTextBox.Text = rxDef.Drug;
			messageTextBox.Text = rxAlert.NotificationMsg;
			highSignificanceCheckBox.Checked = rxAlert.IsHighSignificance;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			rxAlert.NotificationMsg = messageTextBox.Text;
			rxAlert.IsHighSignificance = highSignificanceCheckBox.Checked;

			DialogResult = DialogResult.OK;
		}
	}
}
