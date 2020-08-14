using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormCommItemUserPrefs : ODForm
	{
		///<summary>Helper variable that gets set to Security.CurUser.UserNum on load.</summary>
		private long _userNumCur;

		public FormCommItemUserPrefs()
		{
			InitializeComponent();
			
		}

		private void FormCommItemUserPrefs_Load(object sender, EventArgs e)
		{
			if (Security.CurrentUser == null || Security.CurrentUser.Id < 1)
			{
				MessageBox.Show("Invalid user currently logged in.  No user preferences can be saved.");
				DialogResult = DialogResult.Abort;
				return;
			}
			_userNumCur = Security.CurrentUser.Id;

			//Add the user name of the user currently logged in to the title of this window much like we do for FormOpenDental.
			Text += " {" + Security.CurrentUser.UserName + "}";

			checkCommlogPersistClearNote.Checked = UserPreference.GetBool(UserPreferenceName.CommlogPersistClearNote, true);
			checkCommlogPersistClearEndDate.Checked = UserPreference.GetBool(UserPreferenceName.CommlogPersistClearEndDate, true);
			checkCommlogPersistUpdateDateTimeWithNewPatient.Checked = UserPreference.GetBool(UserPreferenceName.CommlogPersistUpdateDateTimeWithNewPatient, true);
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			UserPreference.Set(UserPreferenceName.CommlogPersistClearNote, checkCommlogPersistClearNote.Checked);
			UserPreference.Set(UserPreferenceName.CommlogPersistClearEndDate, checkCommlogPersistClearEndDate.Checked);
			UserPreference.Set(UserPreferenceName.CommlogPersistUpdateDateTimeWithNewPatient, checkCommlogPersistUpdateDateTimeWithNewPatient.Checked);

			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
