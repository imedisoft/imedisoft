using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAccountEdit : FormBase
	{
		private readonly Account account;
		private List<(char code, string description)> accountTypes;

		public FormAccountEdit(Account account)
		{
			InitializeComponent();

			this.account = account;
		}

		private void FormAccountEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = account.Description;
			bankNumberTextBox.Text = account.BankNumber;
			inactiveCheckBox.Checked = account.Inactive;
			colorButton.BackColor = account.Color;

			accountTypes = AccountType.All.ToList();

			foreach (var accountType in accountTypes)
            {
				typeListBox.Items.Add(accountType.description);
				if (account.Type == accountType.code)
                {
					typeListBox.SelectedItem = accountType.ToString();
                }
            }
		}

		private void ColorButton_Click(object sender, EventArgs e)
		{
            using var colorDialog = new ColorDialog
            {
                Color = colorButton.BackColor
            };

            if (colorDialog.ShowDialog(this) == DialogResult.OK)
			{
				colorButton.BackColor = colorDialog.Color;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterDescription);

				return;
			}

			if (typeListBox.SelectedIndex == -1)
            {
				ShowError(Translation.Common.PleaseSelectType);

				return;
            }

			if (account.Description != description)
			{
				if (!Confirm(Translation.Accounting.ConfirmUpdateSplitsForAllTransactionsAttachedToAccount))
				{
					return;
				}
			}

			account.Description = description;
			account.Type = accountTypes[typeListBox.SelectedIndex].code;
			account.BankNumber = bankNumberTextBox.Text;
			account.Inactive = inactiveCheckBox.Checked;
			account.Color = colorButton.BackColor;

			if (account.Id == 0)
			{
				Accounts.Insert(account);
			}
			else
			{
				Accounts.Update2(account);
			}

			DialogResult = DialogResult.OK;
		}
	}
}
