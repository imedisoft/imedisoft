using Imedisoft.Data;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormFeeSchedPickAuthOntario : ODForm
	{
		public string ODAMemberNumber
		{
			get
			{
				return textODAMemberNumber.Text;
			}
		}

		public string ODAMemberPassword
		{
			get
			{
				return textODAMemberPassword.Text;
			}
		}

		public FormFeeSchedPickAuthOntario()
		{
			InitializeComponent();
		}

		private void FormFeeSchedPickAuthOntario_Load(object sender, EventArgs e)
		{
			textODAMemberNumber.Text = Preferences.GetString(PreferenceName.CanadaODAMemberNumber);
			textODAMemberPassword.Text = Preferences.GetString(PreferenceName.CanadaODAMemberPass);
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			if (textODAMemberNumber.Text == "")
			{
				MessageBox.Show("ODA Member Number cannot be blank.");
				return;
			}

			if (textODAMemberPassword.Text == "")
			{
				MessageBox.Show("ODA Member Password cannot be blank.");
				return;
			}

			Preferences.Set(PreferenceName.CanadaODAMemberNumber, textODAMemberNumber.Text);
			Preferences.Set(PreferenceName.CanadaODAMemberPass, textODAMemberPassword.Text);

			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
