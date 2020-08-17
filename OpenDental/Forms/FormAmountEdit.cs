using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAmountEdit : FormBase
	{
		public decimal Amount
		{
            get
            {
				if (decimal.TryParse(amountTextBox.Text, out var result))
                {
					return result;
                }

				return default;
            }
            set
            {
				amountTextBox.Text = value.ToString("f");
			}
		}

		public FormAmountEdit(string text)
		{
			InitializeComponent();

			textLabel.Text = text;
		}

		private void FormAmountEdit_Load(object sender, EventArgs e)
		{
			amountTextBox.SelectionStart = 0;
			amountTextBox.SelectionLength = amountTextBox.Text.Length;
		}

		private void AmountTextBox_Validating(object sender, CancelEventArgs e)
		{
			if (!decimal.TryParse(amountTextBox.Text, out var result))
			{
				e.Cancel = true;
			}
			else
			{
				amountTextBox.Text = result.ToString("f");
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
    }
}
