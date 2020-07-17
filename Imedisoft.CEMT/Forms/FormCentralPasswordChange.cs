using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralPasswordChange : FormBase
	{
		public FormCentralPasswordChange() => InitializeComponent();

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (accessCodeTextBox.Text != "I'm admin")
			{
				ShowError("Access code is incorrect.");
				return;
			}

			Prefs.UpdateString(PrefName.CentralManagerPassHash, Authentication.HashPasswordMD5(newPasswordTextBox.Text));
			Prefs.RefreshCache();

			DialogResult = DialogResult.OK;
		}
    }
}
