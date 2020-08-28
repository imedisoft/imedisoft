using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.Bridges;
using OpenDental.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAccountPick : FormBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether the form should display accounts from QuickBooks.
		/// </summary>
		public bool IsQuickBooks { get; set; }

		/// <summary>
		/// Gets the selected account.
		/// </summary>
		public Account SelectedAccount { get; private set; }

		/// <summary>
		/// Gets the names of the selected QuickBooks accounts.
		/// </summary>
		public List<string> SelectedQuickBookAccounts { get; } = new List<string>();

		public FormAccountPick()
		{
			InitializeComponent();
		}

		private void FormAccountPick_Load(object sender, EventArgs e)
		{
			if (IsQuickBooks)
			{
				inactiveCheckBox.Visible = false;

				FillGridQuickBooks();

				accountsGrid.SelectionMode = GridSelectionMode.MultiExtended;
			}
			else
			{
				FillGrid();
			}
		}

		private void FillGrid()
		{
			accountsGrid.BeginUpdate();
			accountsGrid.ListGridColumns.Clear();
			accountsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Type, 70));
			accountsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Description, 170));
			accountsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Balance, 65, HorizontalAlignment.Right));
			accountsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.BankNumber, 100));
			accountsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Inactive, 70));
			accountsGrid.ListGridRows.Clear();

			var accounts = Accounts.All;
			for (int i = 0; i < accounts.Count; ++i)
			{
				var account = accounts[i];
				if (!inactiveCheckBox.Checked && account.Inactive)
				{
					continue;
				}

				var gridRow = new GridRow();
				gridRow.Cells.Add(account.Type.ToString());
				gridRow.Cells.Add(account.Description);

				if (account.Type == AccountType.Asset)
				{
					gridRow.Cells.Add(Accounts.GetBalance(account.Id, account.Type).ToString("n"));
				}
				else
				{
					gridRow.Cells.Add("");
				}

				gridRow.Cells.Add(account.BankNumber);
				gridRow.Cells.Add(account.Inactive ? "X" : "");

				if (i < accounts.Count - 1 && account.Type != accounts[i + 1].Type)
				{
					gridRow.LowerBorderColor = Color.Black;
				}

				gridRow.BackColor = account.Color;
				gridRow.Tag = account;
				

				accountsGrid.ListGridRows.Add(gridRow);
			}

			accountsGrid.EndUpdate();
		}

		private void FillGridQuickBooks()
		{
			Cursor.Current = Cursors.WaitCursor;

			var accounts = new List<string>();
			try
			{
				accounts = QuickBooks.GetListOfAccounts();
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);
			}

			Cursor.Current = Cursors.Default;
			
			accountsGrid.BeginUpdate();
			accountsGrid.ListGridColumns.Clear();
			accountsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Description, 200));
			accountsGrid.ListGridRows.Clear();

			if (accounts != null)
			{
				foreach (var account in accounts)
				{
					var gridRow = new GridRow();
					gridRow.Cells.Add(account);
					gridRow.Tag = account;

					accountsGrid.ListGridRows.Add(gridRow);
				}
			}

			accountsGrid.EndUpdate();
		}

		private void AccountsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			AcceptButton_Click(this, EventArgs.Empty);
		}

		private void InactiveCheckBox_Click(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (accountsGrid.GetSelectedIndex() == -1)
			{
				ShowError(Translation.Common.PleaseSelectAccountFirst);

				return;
			}

			if (IsQuickBooks)
			{
				SelectedQuickBookAccounts.Clear();

				for (int i = 0; i < accountsGrid.SelectedIndices.Length; i++)
				{
					SelectedQuickBookAccounts.Add((string)accountsGrid.ListGridRows[accountsGrid.SelectedIndices[i]].Tag);
				}
			}
			else
			{
				SelectedAccount = (Account)accountsGrid.ListGridRows[accountsGrid.GetSelectedIndex()].Tag;
			}

			DialogResult = DialogResult.OK;
		}
	}
}
