using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrProviderKeyEdit : FormBase
	{
		private readonly EhrProviderKey ehrProviderKey;

		public FormEhrProviderKeyEdit(EhrProviderKey ehrProviderKey)
		{
			InitializeComponent();

			this.ehrProviderKey = ehrProviderKey;
		}

		private void FormEhrProviderKeyEdit_Load(object sender, EventArgs e)
		{
			lastNameTextBox.Text = ehrProviderKey.LastName;
			firstNameTextBox.Text = ehrProviderKey.FirstName;
			yearTextBox.Text = ehrProviderKey.Year.ToString();
			keyTextBox.Text = ehrProviderKey.Key;
		}

		private void YearTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) || !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(yearTextBox.Text) || !int.TryParse(yearTextBox.Text, out var year))
            {
				ShowError(Translation.Common.PleaseEnterValidYear);

				return;
			}

			if (!FormEHR.ProvKeyIsValid(lastNameTextBox.Text, firstNameTextBox.Text, year, keyTextBox.Text))
			{
				ShowError(Translation.Ehr.InvalidProviderKey);

				return;
			}

			ehrProviderKey.LastName = lastNameTextBox.Text;
			ehrProviderKey.FirstName = firstNameTextBox.Text;
			ehrProviderKey.Year = year;
			ehrProviderKey.Key = keyTextBox.Text;

			EhrProviderKeys.Save(ehrProviderKey);

			DialogResult = DialogResult.OK;
		}
	}
}
