using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAppointmentFieldDefinitionEdit : FormBase
	{
		private readonly AppointmentFieldDefinition appointmentFieldDefinition;
		
		public FormAppointmentFieldDefinitionEdit(AppointmentFieldDefinition appointmentFieldDefinition)
		{
			InitializeComponent();

			this.appointmentFieldDefinition = appointmentFieldDefinition;
		}

		private void FormApptFieldDefEdit_Load(object sender, EventArgs e)
		{
			nameTextBox.Text = appointmentFieldDefinition.Name;

			foreach (var apptFieldType in AppointmentFieldType.Values)
            {
				typeComboBox.Items.Add(apptFieldType);
				if (apptFieldType.Value == appointmentFieldDefinition.Type)
                {
					typeComboBox.SelectedItem = apptFieldType;
                }
            }

			pickListTextBox.Text = appointmentFieldDefinition.PickList;
			pickListTextBox.Visible = warningLabel.Visible 
				= appointmentFieldDefinition.Type == AppointmentFieldType.PickList;
		}

		private void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (typeComboBox.SelectedItem is DataItem<int> dataItem)
            {
				pickListTextBox.Visible = warningLabel.Visible
					= dataItem.Value == AppointmentFieldType.PickList;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var name = nameTextBox.Text.Trim();
			if (name.Equals(appointmentFieldDefinition.Name))
			{
				if (AppointmentFieldDefinitions.GetExists(x => x.Name == nameTextBox.Text))
				{
					ShowError(Translation.Common.FieldNameAlreadyInUse);

					return;
				}
			}

			if (!(typeComboBox.SelectedItem is DataItem<int> dataItem))
            {
				ShowError(Translation.Common.PleaseSelectType);

				return;
            }

			var appointmentFieldType = dataItem.Value;
			if (appointmentFieldType == AppointmentFieldType.PickList)
            {
				if (string.IsNullOrEmpty(pickListTextBox.Text))
				{
					ShowError(Translation.Common.ListCannotBeBlank);

					return;
				}
			}

			appointmentFieldDefinition.Name = name;
			appointmentFieldDefinition.Type = appointmentFieldType;
			appointmentFieldDefinition.PickList = pickListTextBox.Text;

			AppointmentFieldDefinitions.Save(appointmentFieldDefinition);

			DialogResult = DialogResult.OK;
		}
	}
}
