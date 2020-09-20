using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrQuarterlyKeyEdit : FormBase
	{
		private readonly EhrQuarterlyKey ehrQuarterlyKey;

		public FormEhrQuarterlyKeyEdit(EhrQuarterlyKey ehrQuarterlyKey)
		{
			InitializeComponent();

			this.ehrQuarterlyKey = ehrQuarterlyKey;
		}

		private void FormEhrQuarterlyKeyEdit_Load(object sender, EventArgs e)
		{
			if (ehrQuarterlyKey.Year > 0)
			{
				yearTextBox.Text = ehrQuarterlyKey.Year.ToString();
			}

			if (ehrQuarterlyKey.Quarter > 0)
			{
				quarterTextBox.Text = ehrQuarterlyKey.Quarter.ToString();
			}

			keyTextBox.Text = ehrQuarterlyKey.Key;
		}

		private void YearTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void YearTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (string.IsNullOrEmpty(yearTextBox.Text))
			{
				return;
			}

			if (!int.TryParse(yearTextBox.Text, out var year) || year < 1900 || year > 2999)
			{
				e.Cancel = true;

				return;
			}
		}

		private void QuarterTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && e.KeyChar != '1' && e.KeyChar != '2' && e.KeyChar != '3' && e.KeyChar != '4')
			{
				e.Handled = true;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!Preferences.GetBool(PreferenceName.ShowFeatureEhr))
			{
				ShowError("You must go to Setup, Show Features, and activate EHR before entering keys.");

				return;
			}

			if (string.IsNullOrEmpty(yearTextBox.Text) || !int.TryParse(yearTextBox.Text, out var year) || year < 1900 || year > 2999)
			{
				ShowError("Please enter a valid year.");

				return;
			}

			if (string.IsNullOrEmpty(quarterTextBox.Text) || !int.TryParse(quarterTextBox.Text, out var quarter) || year < 0 || year > 4)
			{
				ShowError("Please enter a valid quarter.");

				return;
			}

			ehrQuarterlyKey.Year = year;
			ehrQuarterlyKey.Quarter = quarter;
			ehrQuarterlyKey.Key = keyTextBox.Text;
			ehrQuarterlyKey.PracticeName = Preferences.GetString(PreferenceName.PracticeTitle);

			EhrQuarterlyKeys.Save(ehrQuarterlyKey);

			DialogResult = DialogResult.OK;
		}
    }
}
