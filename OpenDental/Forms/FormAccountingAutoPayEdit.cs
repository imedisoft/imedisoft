using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAccountingAutoPayEdit : FormBase
	{
		private readonly AccountingAutoPay accountingAutoPay;
		private readonly List<long> accountIds = new List<long>();
		private List<Definition> paymentTypes;

		public FormAccountingAutoPayEdit(AccountingAutoPay accountingAutoPay)
		{
			InitializeComponent();

			this.accountingAutoPay = accountingAutoPay;
		}

		private void FormAccountingAutoPayEdit_Load(object sender, EventArgs e)
		{
			paymentTypes = Definitions.GetDefsForCategory(DefinitionCategory.PaymentTypes, true);
			foreach (var paymentType in paymentTypes)
			{
				payTypeComboBox.Items.Add(paymentType);
				if (paymentType.Id == accountingAutoPay.PayType)
				{
					payTypeComboBox.SelectedItem = paymentType;
				}
			}

			accountingAutoPay.PickList ??= "";

			string[] tokens = accountingAutoPay.PickList.Split(',');

			foreach (var token in tokens)
			{
				if (long.TryParse(token, out var accountId) && accountId > 0)
                {
					accountIds.Add(accountId);
				}
			}

			FillList();
		}

		private void FillList()
		{
			accountsListBox.Items.Clear();

			foreach (var accountId in accountIds)
			{
				accountsListBox.Items.Add(Accounts.GetDescription(accountId));
			}
		}

		private void AccountsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			removeButton.Enabled = accountsListBox.SelectedItem != null;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			using var formAccountPick = new FormAccountPick();

			if (formAccountPick.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			accountIds.Add(formAccountPick.SelectedAccount.Id);

			FillList();
		}

		private void RemoveButton_Click(object sender, EventArgs e)
		{
			if (accountsListBox.SelectedIndex == -1)
			{
				ShowError("Please select an item first.");

				return;
			}

			accountIds.RemoveAt(accountsListBox.SelectedIndex);

			FillList();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (accountingAutoPay.Id == 0)
			{
				DialogResult = DialogResult.Cancel;

				return;
			}

			AccountingAutoPays.Delete(accountingAutoPay);

			accountingAutoPay.Id = 0;

			DialogResult = DialogResult.OK;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!(payTypeComboBox.SelectedItem is Definition payType))
			{
				ShowError("Please select a pay type first.");

				return;
			}

			if (accountIds.Count == 0)
			{
				ShowError("Please add at least one account to the pick list first.");

				return;
			}

			accountingAutoPay.PayType = payType.Id;
			accountingAutoPay.PickList = string.Join(",", accountIds);

			DialogResult = DialogResult.OK;
		}
    }
}
