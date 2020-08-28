using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAbout : FormBase
    {
        public FormAbout() => InitializeComponent();

        private void FormAbout_Load(object sender, EventArgs e)
        {
            versionLabel.Text = "Version: " + Application.ProductVersion;

            var updateHistory = UpdateHistories.GetForVersion(Application.ProductVersion);
            if (updateHistory != null)
            {
                versionLabel.Text += "  Since: " + updateHistory.InstalledOn.ToShortDateString();
            }

            copyrightLabel.Text = Prefs.GetString(PrefName.SoftwareName) + " Copyright 2003-" + DateTime.Now.ToString("yyyy") + ", Jordan Sparks, D.M.D.";

            var serviceInfo = Computers.GetServiceInfo();
            serviceHostnameLabel.Text = serviceInfo.hostname;
            serviceNameLabel.Text = serviceInfo.name;
            serviceVersionLabel.Text = serviceInfo.version;
            serviceCommentLabel.Text = serviceInfo.comment;
            machineNameLabel.Text = Environment.MachineName.ToUpper();
        }

        private void LicenseButton_Click(object sender, EventArgs e)
        {
            using var formLicense = new FormLicense();

            formLicense.ShowDialog(this);
        }

        private void butClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
