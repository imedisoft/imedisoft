using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralPasswordCheck : FormBase
	{
		public FormCentralPasswordCheck() => InitializeComponent();

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			string passwordHash = PrefC.GetString(PrefName.CentralManagerPassHash);
			string passwordSalt = PrefC.GetString(PrefName.CentralManagerPassSalt);

			bool result = Authentication.CheckPassword(passwordTextBox.Text, passwordSalt, passwordHash);

			if (!result)
			{
				ShowError("The specified password is invalid.");
				return;
			}

			DialogResult = DialogResult.OK;
		}
	}
}
