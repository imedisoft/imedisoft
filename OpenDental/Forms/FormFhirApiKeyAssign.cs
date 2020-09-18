using Imedisoft.Bridges.Fhir;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormFhirApiKeyAssign : FormBase
	{
		public FormFhirApiKeyAssign()
		{
			InitializeComponent();
		}

		private async void AcceptButton_Click(object sender, EventArgs e)
		{
			var key = keyTextBox.Text.Trim();
			if (key.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterApiKey);

				return;
			}

			Cursor = Cursors.WaitCursor;

            try
            {
				await FhirClient.Assign(key);

				Cursor = Cursors.Default;
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

			DialogResult = DialogResult.OK;
		}
	}
}
