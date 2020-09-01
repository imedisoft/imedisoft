using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
	public partial class FormAppointmentFieldDefinitions : FormBase
	{
		private bool appointmentFieldDefinitionsChanged = false;

		public FormAppointmentFieldDefinitions() => InitializeComponent();

		private void FormApptFieldDefs_Load(object sender, EventArgs e)
		{
			AppointmentFieldDefinitions.RefreshCache();

			foreach (var appointmenttFieldDefinition in AppointmentFieldDefinitions.All)
			{
				appointmentFieldDefinitionsListBox.Items.Add(appointmenttFieldDefinition);
			}
		}

		private void SetupButton_Click(object sender, EventArgs e)
		{
			using var formFieldDefLink = new FormFieldDefLink(FieldLocations.AppointmentEdit);

			formFieldDefLink.ShowDialog(this);
		}

		private void ApptFieldDefinitionsListBox_DoubleClick(object sender, EventArgs e)
		{
			if (appointmentFieldDefinitionsListBox.SelectedItem is AppointmentFieldDefinition appointmentFieldDefinition)
			{
				using var formAppointmentFieldDefinitionEdit = new FormAppointmentFieldDefinitionEdit(appointmentFieldDefinition);
				if (formAppointmentFieldDefinitionEdit.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}

				appointmentFieldDefinitionsChanged = true;
				appointmentFieldDefinitionsListBox.Invalidate(
					appointmentFieldDefinitionsListBox.GetItemRectangle(
						appointmentFieldDefinitionsListBox.SelectedIndex));
			}
		}

		private void ApptFieldDefinitionsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			deleteButton.Enabled = appointmentFieldDefinitionsListBox.SelectedItem != null;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var appointmentFieldDefinition = new AppointmentFieldDefinition();

			using var formAppointmentFieldDefinitionEdit = new FormAppointmentFieldDefinitionEdit(appointmentFieldDefinition);
			if (formAppointmentFieldDefinitionEdit.ShowDialog(this) != DialogResult.OK)
            {
				return;
            }

			appointmentFieldDefinitionsChanged = true;
			appointmentFieldDefinitionsListBox.Items.Add(appointmentFieldDefinition);
			appointmentFieldDefinitionsListBox.SelectedItem = appointmentFieldDefinition;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (appointmentFieldDefinitionsListBox.SelectedItem is AppointmentFieldDefinition appointmentFieldDefinition)
            {
				if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
                {
					return;
                }

                try
                {
					AppointmentFieldDefinitions.Delete(appointmentFieldDefinition);

					appointmentFieldDefinitionsChanged = true;
					appointmentFieldDefinitionsListBox.Items.Remove(appointmentFieldDefinition);
				}
				catch (Exception exception)
				{
					ShowError(exception.Message);
				}
			}
		}

		private void CloseButton_Click(object sender, EventArgs e) => Close();

		private void FormApptFieldDefs_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (appointmentFieldDefinitionsChanged)
            {
				CacheManager.RefreshGlobal(nameof(InvalidType.CustomFields));
            }
		}
    }
}
