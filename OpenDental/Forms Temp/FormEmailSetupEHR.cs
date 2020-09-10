using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEmailSetupEHR : ODForm
	{
		public FormEmailSetupEHR()
		{
			InitializeComponent();

		}

		private void FormEmailSetupEHR_Load(object sender, EventArgs e)
		{
			textPOPserver.Text = Preferences.GetString(PreferenceName.EHREmailPOPserver);
			textUsername.Text = Preferences.GetString(PreferenceName.EHREmailFromAddress);
			textPassword.Text = Preferences.GetString(PreferenceName.EHREmailPassword);
			textPort.Text = Preferences.GetString(PreferenceName.EHREmailPort);
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			Preferences.Set(PreferenceName.EHREmailPOPserver, textPOPserver.Text);
			Preferences.Set(PreferenceName.EHREmailFromAddress, textUsername.Text);
			Preferences.Set(PreferenceName.EHREmailPassword, textPassword.Text);

			try
			{
				Preferences.Set(PreferenceName.EHREmailPort, PIn.Long(textPort.Text));
			}
			catch
			{
				MessageBox.Show("invalid port number.");
			}

			DataValid.SetInvalid(InvalidType.Prefs);
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
