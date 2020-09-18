using Imedisoft.Bridges.Fhir;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormFhirApiKeyEdit : FormBase
	{
		private FhirApiKey fhirApiKey;
		private bool hasChanges;

		public FormFhirApiKeyEdit(FhirApiKey fhirApiKey)
		{
			InitializeComponent();

			this.fhirApiKey = fhirApiKey;
		}

		private void FormFhirApiKeyEdit_Load(object sender, EventArgs e)
		{
			FillForm();
		}

		private void FillForm()
		{
			keyTextBox.Text = fhirApiKey.Key;
			statusTextBox.Text = FhirClient.GetDescription(fhirApiKey.Status);
			nameTextBox.Text = fhirApiKey.DeveloperName;
			emailTextBox.Text = fhirApiKey.DeveloperEmail;
			phoneTextBox.Text = fhirApiKey.DeveloperPhone;

			if (fhirApiKey.DisabledOn.HasValue)
			{
				dateDisabledTextBox.Text = fhirApiKey.DisabledOn.Value.ToShortDateString();
			}

			disableButton.Enabled = fhirApiKey.Enabled || fhirApiKey.IsDisabledByCustomer;

			if (fhirApiKey.Enabled)
			{
				disableButton.Image = Properties.Resources.IconToggleOff;
				disableButton.Text = Translation.Common.DisableWithMnemonic;
			}
			else
			{
				disableButton.Image = Properties.Resources.IconToggleOn;
				disableButton.Text = Translation.Common.EnableWithMnemonic;
			}
		}

		private async void DisableButton_Click(object sender, EventArgs e)
		{
            try
            {
				disableButton.Enabled = false;

				Cursor = Cursors.WaitCursor;

				var updatedFhirApiKey = await FhirClient.Toggle(fhirApiKey.Key);

				fhirApiKey = updatedFhirApiKey;

				disableButton.Enabled = true;

				FillForm();

				Cursor = Cursors.Default;

				hasChanges = true;
			}
			catch (Exception exception)
            {
				if (Visible)
                {
					Cursor = Cursors.Default;

					ShowError(exception.Message);
				}

				return;
            }
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			DialogResult = hasChanges ? DialogResult.OK : DialogResult.Cancel;

			Close();
		}
	}
}
