using CodeBase;
using System;

namespace Imedisoft.Forms
{
    public partial class FormLicense : FormBase
	{
		public FormLicense() 
			=> InitializeComponent();

		private void FormLicense_Load(object sender, EventArgs e)
		{
			licenseListBox.Items.Add(new ODBoxItem<string>("OpenDental", Properties.Resources.OpenDentalLicense));
			licenseListBox.Items.Add(new ODBoxItem<string>("AForge", Properties.Resources.AForge));
			licenseListBox.Items.Add(new ODBoxItem<string>("Bouncy Castle", Properties.Resources.BouncyCastle));
			licenseListBox.Items.Add(new ODBoxItem<string>("BSD", Properties.Resources.Bsd));
			licenseListBox.Items.Add(new ODBoxItem<string>("CDT", Properties.Resources.CDT_Content_End_User_License1));
			licenseListBox.Items.Add(new ODBoxItem<string>("Dropbox", Properties.Resources.Dropbox_Api));
			licenseListBox.Items.Add(new ODBoxItem<string>("GPL", Properties.Resources.GPL));
			licenseListBox.Items.Add(new ODBoxItem<string>("Drifty", Properties.Resources.Ionic));
			licenseListBox.Items.Add(new ODBoxItem<string>("Mentalis", Properties.Resources.Mentalis));
			licenseListBox.Items.Add(new ODBoxItem<string>("Microsoft", Properties.Resources.Microsoft));
			licenseListBox.Items.Add(new ODBoxItem<string>("MigraDoc", Properties.Resources.MigraDoc));
			licenseListBox.Items.Add(new ODBoxItem<string>("NDde", Properties.Resources.NDde));
			licenseListBox.Items.Add(new ODBoxItem<string>("Newton Soft", Properties.Resources.NewtonSoft_Json));
			licenseListBox.Items.Add(new ODBoxItem<string>("PDFSharp", Properties.Resources.PdfSharp));
			licenseListBox.Items.Add(new ODBoxItem<string>("SharpDX", Properties.Resources.SharpDX));
			licenseListBox.Items.Add(new ODBoxItem<string>("SSHNet", Properties.Resources.SshNet));
			licenseListBox.Items.Add(new ODBoxItem<string>("Stdole", Properties.Resources.stdole));
			licenseListBox.Items.Add(new ODBoxItem<string>("Tamir", Properties.Resources.Tamir));
			licenseListBox.Items.Add(new ODBoxItem<string>("Tao_Freeglut", Properties.Resources.Tao_Freeglut));
			licenseListBox.Items.Add(new ODBoxItem<string>("Tao_OpenGL", Properties.Resources.Tao_OpenGL));
			licenseListBox.Items.Add(new ODBoxItem<string>("Twain Group", Properties.Resources.Twain));
			licenseListBox.Items.Add(new ODBoxItem<string>("Zxing", Properties.Resources.Zxing));
			licenseListBox.SelectedIndex = 0;
		}

		private void LicenseListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			licenseTextBox.Text = licenseListBox.GetSelected<string>();
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
