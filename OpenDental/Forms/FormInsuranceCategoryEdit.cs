using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormInsuranceCategoryEdit : FormBase
	{
		private readonly CovCat covCat;

		public FormInsuranceCategoryEdit(CovCat covCat)
		{
			InitializeComponent();

			this.covCat = covCat;
		}

		private void FormInsuranceCategoryEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = covCat.Description;
			if (covCat.DefaultPercent != -1)
			{
				percentTextBox.Text = covCat.DefaultPercent.ToString();
			}

			hiddenCheckBox.Checked = covCat.IsHidden;

			for (int i = 0; i < Enum.GetNames(typeof(EbenefitCategory)).Length; i++)
			{
				categoryComboBox.Items.Add(Enum.GetNames(typeof(EbenefitCategory))[i]);
				if (Enum.GetNames(typeof(EbenefitCategory))[i] == covCat.EbenefitCat.ToString())
				{
					categoryComboBox.SelectedIndex = i;
				}
			}
		}

		private void PercentTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void PercentTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (string.IsNullOrEmpty(percentTextBox.Text)) return;

			if (int.TryParse(percentTextBox.Text, out var percent))
			{
				percentTextBox.Text = Math.Max(Math.Min(percent, 100), 0).ToString();
			}
			else
			{
				e.Cancel = true;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			int percent = -1;
			if (!string.IsNullOrEmpty(percentTextBox.Text))
			{
				if (!int.TryParse(percentTextBox.Text, out percent))
				{
					ShowError(Translation.Common.PleaseEnterValidPercentage);

					return;
				}

				percent = Math.Max(Math.Min(percent, 100), 0);
			}

			covCat.Description = descriptionTextBox.Text;
			covCat.DefaultPercent = percent;
			covCat.IsHidden = hiddenCheckBox.Checked;
			covCat.EbenefitCat = (EbenefitCategory)categoryComboBox.SelectedIndex;

			CovCats.Save(covCat);

			DialogResult = DialogResult.OK;
		}
    }
}
