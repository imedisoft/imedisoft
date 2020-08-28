using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAccounting : FormBase
	{
		public FormAccounting()
		{
			InitializeComponent();
		}

		private void FormAccounting_Load(object sender, EventArgs e)
		{
			dateTextBox.Text = DateTime.Today.ToShortDateString();

			FillGrid();
		}

		private void SetupMenuItem_Click(object sender, EventArgs e)
		{
			using var formAccountingSetup = new FormAccountingSetup();

			formAccountingSetup.ShowDialog(this);
		}

		private void LockMenuItem_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.SecurityAdmin))
			{
				return;
			}

			using var formAccountingLock = new FormAccountingLock();
			if (formAccountingLock.ShowDialog(this) == DialogResult.OK)
			{
				SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin, 0, "Accounting Lock Changed");
			}
		}

		private void GeneralLedgerMenuItem_Click(object sender, EventArgs e)
		{
			using var formRpAccountingGenLedg = new FormRpAccountingGenLedg();

			formRpAccountingGenLedg.ShowDialog(this);
		}

		private void BalanceSheetMenuItem_Click(object sender, EventArgs e)
		{
			using var formRpAccountingBalanceSheet = new FormRpAccountingBalanceSheet();

			formRpAccountingBalanceSheet.ShowDialog(this);
		}

		private void ProfitLossMenuItem_Click(object sender, EventArgs e)
		{
			using var formRpAccountingProfitLoss = new FormRpAccountingProfitLoss();

			formRpAccountingProfitLoss.ShowDialog(this);
		}

		private void FillGrid(long? selectedAccountId = null)
		{
			var selectedAccountIndex = -1;

			Accounts.RefreshCache();

			accountsGrid.BeginUpdate();
			accountsGrid.ListGridColumns.Clear();
			accountsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Type, 70));
			accountsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Description, 170));
			accountsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Balance, 80, HorizontalAlignment.Right));
			accountsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.BankNumber, 100));
			accountsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Inactive, 70, HorizontalAlignment.Center));
			accountsGrid.ListGridRows.Clear();

			var date = DateTime.Today;
			if (!string.IsNullOrEmpty(dateTextBox.Text) && DateTime.TryParse(dateTextBox.Text, out var result))
            {
				date = result;
            }

			var accounts = Accounts.GetSummaries(date, inactiveCheckBox.Checked);
			for (int i = 0; i < accounts.Count; i++)
			{
				var account = accounts[i];

				var gridRow = new GridRow();
				gridRow.Cells.Add(AccountType.GetDescription(account.Type));
				gridRow.Cells.Add(account.Description);
				gridRow.Cells.Add(account.Balance.ToString("N2"));
				gridRow.Cells.Add(account.BankNumber);
				gridRow.Cells.Add(account.Inactive ? "X" : "");

				gridRow.BackColor = account.Color;
				if (i < accounts.Count - 1 && account.Type != accounts[i + 1].Type)
				{
					gridRow.LowerBorderColor = Color.Black;
				}

				if (selectedAccountId == account.AccountId)
                {
					selectedAccountIndex = i;
                }

				accountsGrid.ListGridRows.Add(gridRow);
			}

			accountsGrid.EndUpdate();

			if (selectedAccountIndex != -1) 
				accountsGrid.SetSelected(selectedAccountIndex, true);
		}

		private void InactiveCheckBox_Click(object sender, EventArgs e)
			=> RefreshButton_Click(this, EventArgs.Empty);

		private void RefreshButton_Click(object sender, EventArgs e)
			=> FillGrid();

		private void TodayButton_Click(object sender, EventArgs e)
		{
			dateTextBox.Text = DateTime.Today.ToShortDateString();

			RefreshButton_Click(this, EventArgs.Empty);
		}

		private void AccountsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var accountSummary = accountsGrid.SelectedTag<AccountSummary>();
			if (accountSummary == null)
            {
				return;
            }

			if (accountSummary.AccountId == 0)
			{
				ShowError(Translation.Accounting.AccountIsGeneratedAutomatically);
				
				return;
			}

			var date = DateTime.Today;
			if (!string.IsNullOrEmpty(dateTextBox.Text))
            {
				if (DateTime.TryParse(dateTextBox.Text, out var result))
                {
					date = result;
                }
                else
                {
					ShowError(Translation.Common.PleaseEnterValidDate);

					return;
                }
            }

			var account = Accounts.GetAccount(accountSummary.AccountId);

            using var formJournal = new FormJournal(account)
            {
                InitialAsOfDate = date
            };

            formJournal.ShowDialog(this);

			FillGrid(accountSummary.AccountId);
		}

		private void AccountsGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = 
				accountsGrid.SelectedGridRows.Count > 0;
		}

		private void AddButton_Click(object sender, EventArgs e)
        {
			using var formAccountEdit = new FormAccountEdit(
				new Account
				{
					Type = AccountType.Asset,
					Color = Color.White
				});

			formAccountEdit.ShowDialog(this);

			FillGrid();
		}

        private void EditButton_Click(object sender, EventArgs e)
        {
			var accountSummary = accountsGrid.SelectedTag<AccountSummary>();
			if (accountSummary == null)
			{
				ShowError(Translation.Common.PleaseSelectAccountFirst);

				return;
			}

			if (accountSummary.AccountId == 0)
			{
				ShowError(Translation.Accounting.AccountIsGeneratedAutomaticallyAndCannotEdit);

				return;
			}

			var account = Accounts.GetAccount(accountSummary.AccountId);

			using var formAccountEdit = new FormAccountEdit(account);
			if (formAccountEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid(accountSummary.AccountId);
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var accountSummary = accountsGrid.SelectedTag<AccountSummary>();
			if (accountSummary == null)
			{
				ShowError(Translation.Common.PleaseSelectAccountFirst);

				return;
			}

			if (accountSummary.AccountId == 0)
			{
				ShowError(Translation.Accounting.AccountIsGeneratedAutomaticallyAndCannotDelete);

				return;
			}

			var account = Accounts.GetAccount(accountSummary.AccountId);

			if (!Confirm(Translation.Accounting.ConfirmDeleteAccount))
            {
				return;
            }

			try
			{
				Accounts.Delete(account);
			}
			catch (ApplicationException exception)
			{
				ShowError(exception.Message);

				return;
			}

			FillGrid();
		}

		private void ExportButton_Click(object sender, EventArgs e)
        {
			accountsGrid.Export(accountsGrid.Title);
		}

        private void CancelButton_Click(object sender, EventArgs e)
        {
			Close();
        }
    }
}
