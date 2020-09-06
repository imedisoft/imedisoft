using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormApptFieldPickEdit : FormBase
	{
		private readonly ApptField apptField;

		public FormApptFieldPickEdit(ApptField apptField)
		{
			InitializeComponent();

			this.apptField = apptField;
		}

		private void FormApptFieldPickEdit_Load(object sender, EventArgs e)
		{
			nameLabel.Text = apptField.FieldName;

			var pickList = AppointmentFieldDefinitions.GetPickListByFieldName(apptField.FieldName);
			var pickListValues = pickList.Split('\n');

			foreach (string value in pickListValues)
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					continue;
				}

				valuesListBox.Items.Add(value);
			}

			valuesListBox.SelectedItem = apptField.FieldValue;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (valuesListBox.SelectedIndex == -1)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			apptField.FieldValue = valuesListBox.SelectedItem.ToString();
			if (string.IsNullOrEmpty(apptField.FieldValue))
			{
				if (apptField.ApptFieldNum == 0)
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
