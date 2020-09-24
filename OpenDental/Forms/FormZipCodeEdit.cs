using OpenDentBusiness;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormZipCodeEdit : FormBase
	{
		private readonly ZipCode zipCode;

		public FormZipCodeEdit(ZipCode zipCode)
		{
			InitializeComponent();

			this.zipCode = zipCode;
		}

		private void FormZipCodeEdit_Load(object sender, EventArgs e)
		{
			if (CultureInfo.CurrentCulture.Name.EndsWith("CA")) // Canadian. en-CA or fr-CA
			{
				Text = "Postal Code";
				labelZipCode.Text = "Postal Code";
				labelState.Text = "Province";
			}

			zipTextBox.Text = zipCode.Digits;
			cityTextBox.Text = zipCode.City;
			stateTextBox.Text = zipCode.State;
			frequentCheckBox.Checked = zipCode.IsFrequent;
		}

		private void CityTextBox_TextChanged(object sender, EventArgs e)
		{
			if (cityTextBox.Text.Length == 1)
			{
				cityTextBox.Text = cityTextBox.Text.ToUpper();
				cityTextBox.SelectionStart = 1;
			}
		}

		private void StateTextBox_TextChanged(object sender, EventArgs e)
		{
			// if USA or Canada, capitalize first 2 letters
			if (CultureInfo.CurrentCulture.Name == "en-US"  || CultureInfo.CurrentCulture.Name.EndsWith("CA"))
			{
				if (stateTextBox.Text.Length == 1 || stateTextBox.Text.Length == 2)
				{
					stateTextBox.Text = stateTextBox.Text.ToUpper();
					stateTextBox.SelectionStart = 2;
				}
			}
			else
			{
				if (stateTextBox.Text.Length == 1)
				{
					stateTextBox.Text = stateTextBox.Text.ToUpper();
					stateTextBox.SelectionStart = 1;
				}
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(zipTextBox.Text) || 
				string.IsNullOrWhiteSpace(cityTextBox.Text) || 
				string.IsNullOrWhiteSpace(stateTextBox.Text))
			{
				ShowError("City, State, or Zip cannot be left blank.");

				return;
			}

			zipCode.City = cityTextBox.Text;
			zipCode.State = stateTextBox.Text;
			zipCode.Digits = zipTextBox.Text;
			zipCode.IsFrequent = frequentCheckBox.Checked;

			ZipCodes.Save(zipCode);

			DialogResult = DialogResult.OK;
		}
	}
}
