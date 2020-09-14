using Imedisoft.Data;
using Imedisoft.Data.Cache;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormUpdateSetup : FormBase
    {
        public FormUpdateSetup()
        {
            InitializeComponent();
        }

        private void FormUpdateSetup_Load(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.SecurityAdmin, true))
            {
                registrationKeyButton.Enabled = false;

                acceptButton.Enabled = false;
            }

            updateServerTextBox.Text = Preferences.GetString(PreferenceName.UpdateServerAddress);
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            var hasChanges =
                Preferences.Set(PreferenceName.UpdateServerAddress, updateServerTextBox.Text);
            
            if (hasChanges)
            {
                CacheManager.RefreshGlobal(nameof(InvalidType.Prefs));
            }

            DialogResult = DialogResult.OK;
        }

        private void FormUpdateSetup_FormClosing(object sender, FormClosingEventArgs e)
        {
            SecurityLogs.Write(DialogResult == DialogResult.OK ? Permissions.SecurityAdmin : Permissions.Setup, 
                Translation.SecurityLog.AccessedWindowUpdateSetup);
        }
    }
}
