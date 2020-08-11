using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormBackupReminder : FormBase
	{
		public FormBackupReminder() => InitializeComponent();

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!a1CheckBox.Checked && 
				!a2CheckBox.Checked && 
				!a3CheckBox.Checked && 
				!a4CheckBox.Checked)
			{
				ShowError(
					"You are not allowed to continue using this program unless you are making daily backups.");

				return;
			}

			if (!b1CheckBox.Checked && 
				!b2CheckBox.Checked)
			{
				ShowError(
					"You are not allowed to continue using this program unless you have proof that your backups are good.");

				return;
			}

			if (!c1CheckBox.Checked && 
				!c2CheckBox.Checked)
			{
				ShowError(
					"You are not allowed to continue using this program unless you have a long-term strategy.");

				return;
			}

			DialogResult = DialogResult.OK;
		}

		private void FormBackupReminder_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult != DialogResult.OK)
			{
				if (Prompt("Program will now close.", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
					e.Cancel = true;
                }
			}
		}
	}
}
