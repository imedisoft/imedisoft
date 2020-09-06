using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormApptFieldEdit : FormBase
	{
		private readonly ApptField apptField;

		public FormApptFieldEdit(ApptField apptField)
		{
			InitializeComponent();

			this.apptField = apptField;
		}

		private void FormApptFieldEdit_Load(object sender, EventArgs e)
		{
			nameLabel.Text = apptField.FieldName;
			valueTextBox.Text = apptField.FieldValue;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			apptField.FieldValue = valueTextBox.Text;

			if (string.IsNullOrEmpty(apptField.FieldValue))
			{
				if (apptField.IsNew)
				{
					DialogResult = DialogResult.Cancel;

					return;
				}

				ApptFields.DeleteFieldForAppt(apptField.FieldName, apptField.AptNum);
			}
			else
			{
				ApptFields.Upsert(apptField);
			}

			DialogResult = DialogResult.OK;
		}
	}
}
