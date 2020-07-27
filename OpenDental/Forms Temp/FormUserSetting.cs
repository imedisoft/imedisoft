using Imedisoft.Forms;
using OpenDentBusiness;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    /// <summary>
    /// This form (per Nathan) should be used for any future features that could be categorized as 
	/// a user setting. The intent of this class was to create a place for specific user settings.
    /// </summary>
    public partial class FormUserSetting : FormBase
	{
		public FormUserSetting()
		{
			InitializeComponent();
		}

		private void FormUserSetting_Load(object sender, EventArgs e)
		{
			var logOffTimerOverride = UserPreference.GetInt(UserPreferenceName.LogOffTimerOverride);

			autoLogoffTextBox.Text = logOffTimerOverride == 0 ? "" : logOffTimerOverride.ToString();
			supressMessageCheckBox.Checked = UserPreference.GetBool(UserPreferenceName.SuppressLogOffMessage);
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			UserPreference.Set(UserPreferenceName.SuppressLogOffMessage, supressMessageCheckBox.Checked);

			DialogResult = DialogResult.OK;
		}
	}
}
