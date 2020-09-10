using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEhrQuarterlyKeyEdit : ODForm
	{
		public EhrQuarterlyKey KeyCur;

		public FormEhrQuarterlyKeyEdit()
		{
			InitializeComponent();
		}

		private void FormEhrQuarterlyKeyEdit_Load(object sender, EventArgs e)
		{
			if (KeyCur.YearValue > 0)
			{
				textYear.Text = KeyCur.YearValue.ToString();
			}

			if (KeyCur.QuarterValue > 0)
			{
				textQuarter.Text = KeyCur.QuarterValue.ToString();
			}

			textKey.Text = KeyCur.KeyValue;
		}

		private void butDelete_Click(object sender, EventArgs e)
		{
			if (KeyCur.IsNew)
			{
				DialogResult = DialogResult.Cancel;
				return;
			}

			if (!MsgBox.Show(MsgBoxButtons.OKCancel, "Delete?"))
			{
				return;
			}

			EhrQuarterlyKeys.Delete(KeyCur.EhrQuarterlyKeyNum);

			DialogResult = DialogResult.OK;
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			if (!Preferences.GetBool(PreferenceName.ShowFeatureEhr))
			{
				MessageBox.Show("You must go to Setup, Show Features, and activate EHR before entering keys.");
				return;
			}

			if (textYear.Text == "")
			{
				MessageBox.Show("Please enter a year.");
				return;
			}

			if (textQuarter.Text == "")
			{
				MessageBox.Show("Please enter a quarter.");
				return;
			}

			if (textYear.errorProvider1.GetError(textYear) != "" || textQuarter.errorProvider1.GetError(textQuarter) != "")
			{
				MessageBox.Show("Please fix errors first.");
				return;
			}

			if (!FormEHR.QuarterlyKeyIsValid(textYear.Text, textQuarter.Text, Preferences.GetString(PreferenceName.PracticeTitle), textKey.Text))
			{
				MessageBox.Show("Invalid quarterly key");
				return;
			}

			KeyCur.YearValue = PIn.Int(textYear.Text);
			KeyCur.QuarterValue = PIn.Int(textQuarter.Text);
			KeyCur.KeyValue = textKey.Text;
			KeyCur.PracticeName = Preferences.GetString(PreferenceName.PracticeTitle);

			if (KeyCur.IsNew)
			{
				EhrQuarterlyKeys.Insert(KeyCur);
			}
			else
			{
				EhrQuarterlyKeys.Update(KeyCur);
			}

			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
