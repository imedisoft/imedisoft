using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    /// <summary>
    /// Used for user-specific settings that are unique to the Oryx bridge.
    /// </summary>
    public partial class FormOryxUserSettings : ODForm
	{
		private readonly Program program;

		public FormOryxUserSettings()
		{
			InitializeComponent();

			program = Programs.GetCur(ProgramName.Oryx);
		}

		private void FormUserSetting_Load(object sender, EventArgs e)
		{
			textUsername.Text = UserPreference.GetString(UserPreferenceName.ProgramUserName, program.Id);
			textPassword.Text = UserPreference.GetString(UserPreferenceName.ProgramPassword, program.Id);
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			UserPreference.Set(UserPreferenceName.ProgramUserName, textUsername.Text, program.Id);
			UserPreference.Set(UserPreferenceName.ProgramPassword, textPassword.Text, program.Id);

			DialogResult = DialogResult.OK;

			Close();
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;

			Close();
		}
	}
}
