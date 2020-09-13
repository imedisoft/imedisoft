using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class Accounts
	{
		private class AccountCache : ListCache<Account>
		{
			protected override IEnumerable<Account> Load()
				=> SelectMany("SELECT * FROM `accounts` ORDER BY `type`, `description`");
		}

		private static readonly AccountCache cache = new AccountCache();

		public static void RefreshCache() 
			=> cache.Refresh();

		public static List<Account> All 
			=> cache.GetAll();

		public static Account GetFirstOrDefault(Predicate<Account> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static long Insert(Account account) 
			=> ExecuteInsert(account);

		/// <summary>
		/// Also updates existing journal entry splits linked to this account that have not been locked.
		/// </summary>
		public static void Update2(Account account)
		{
			ExecuteUpdate(account);

			var oldDescription = SelectOne(account.Id)?.Description;
			if (oldDescription == account.Description)
			{
				return;
			}

			// The account was renamed, so update journalentry.Splits.
			string command = @"SELECT je2.*,account.Description
					FROM journalentry 
					INNER JOIN journalentry je2 ON je2.TransactionNum=journalentry.TransactionNum
					INNER JOIN account ON account.AccountNum=je2.AccountNum
					WHERE journalentry.AccountNum=" + account.Id + @"
					AND journalentry.DateDisplayed > " + POut.Date(PrefC.GetDate(PreferenceName.AccountingLockDate)) + @"
					ORDER BY je2.TransactionNum";

			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0)
			{
				return;
			}

			List<JournalEntry> listJournalEntries = OpenDentBusiness.Crud.JournalEntryCrud.TableToList(table);
			//Construct a dictionary that has the description for each JournalEntryNum.
			Dictionary<long, string> dictJournalEntryDescriptions = table.Rows.Cast<DataRow>()
				.GroupBy(x => PIn.Long(x["JournalEntryNum"].ToString()))
				.ToDictionary(x => x.Key, x => PIn.String(x.First()["Description"].ToString()));

			// Now we will loop through all the journal entries and find the other journal entries that are attached to the same transaction and update
			// those splits.
			List<int> listIndexesForTrans = new List<int>();
			long curTransactionNum = listJournalEntries[0].TransactionNum;
			for (int i = 0; i < listJournalEntries.Count; i++)
			{
				if (listJournalEntries[i].TransactionNum == curTransactionNum)
				{
					listIndexesForTrans.Add(i);
					continue;
				}
				UpdateJournalEntrySplits(listJournalEntries, listIndexesForTrans, dictJournalEntryDescriptions, account);
				curTransactionNum = listJournalEntries[i].TransactionNum;
				listIndexesForTrans.Clear();
				listIndexesForTrans.Add(i);
			}

			UpdateJournalEntrySplits(listJournalEntries, listIndexesForTrans, dictJournalEntryDescriptions, account);
		}

		/// <summary>
		/// Updates the splits on the journal entries whose indexes are passed in.
		/// </summary>
		/// <param name="journalEntries">All journal entries for a particular account.</param>
		/// <param name="listIndexesForTrans">The index of the journal entries in listJournalEntries. These are the ones that will be updated.</param>
		/// <param name="journalEntryDescriptions">A dictionary where the key is the JournalEntryNum and the value is the journal entry's account description.</param>
		/// <param name="acct">The account that whose description is being updates.</param>
		private static void UpdateJournalEntrySplits(List<JournalEntry> journalEntries, List<int> listIndexesForTrans,
			Dictionary<long, string> journalEntryDescriptions, Account acct)
		{
			foreach (int index in listIndexesForTrans.Where(x => journalEntries[x].AccountNum != acct.Id))
			{
				JournalEntry journalEntry = journalEntries[index];
				if (listIndexesForTrans.Count <= 2)
				{
					//When a transaction only has two splits, the Splits column will simply be the name of the account of the other split.
					journalEntry.Splits = acct.Description;
				}
				else
				{
					//When a transaction has three or more splits, the Splits column will be the names of the account and the amount of the other splits.
					//Ex.: 
					//Patient Fee Income 85.00
					//Supplies 110.00
					journalEntry.Splits = string.Join("\r\n", listIndexesForTrans
						.Where(x => journalEntries[x].Id != journalEntry.Id)
						.Select(x => journalEntryDescriptions[journalEntries[x].Id] + " " +
						(journalEntries[x].DebitAmt > 0 ?
							journalEntries[x].DebitAmt.ToString("n") :
							journalEntries[x].CreditAmt.ToString("n"))));
				}
				JournalEntries.Update(journalEntry);
			}
		}

		/// <summary>
		///		<para>
		///			Gets the description of the account with the specified ID.
		///		</para>
		/// </summary>
		/// <param name="accountId">The ID of the account.</param>
		/// <returns>A description of the account; or a empty string if the account doesn't exist.</returns>
		public static string GetDescription(long accountId) => GetAccount(accountId)?.Description ?? "";

		/// <summary>
		///		<para>
		///			Gets the account with the specified ID.
		///		</para>
		/// </summary>
		/// <param name="accountId">The ID of the account.</param>
		public static Account GetAccount(long accountId) 
			=> cache.FirstOrDefault(account => account.Id == accountId);

		/// <summary>
		///		<para>
		///			Attempts to delete the specified <paramref name="account"/>. Throws an 
		///			exception if the account is in use and cannot be deleted.
		///		</para>
		/// </summary>
		/// <param name="account">The account to delete.</param>
		/// <exception cref="Exception">If the account is in use.</exception>
		public static void Delete(Account account)
		{
			// Check to see if account has any journal entries
			if (Database.ExecuteLong("SELECT COUNT(*) FROM `journal_entries` WHERE `account_id` = " + account.Id) != 0)
			{
				throw new Exception(
					Translation.Accounting.NotAllowedToDeleteAccountWithJournalEntries);
			}

			string[] depositAccountIds = Preferences.GetString(PreferenceName.AccountingDepositAccounts, "").Split(new char[] { ',' });
			for (int i = 0; i < depositAccountIds.Length; i++)
			{
				if (depositAccountIds[i] == account.Id.ToString())
				{
					throw new Exception(Translation.Accounting.AccountInUseInSetup);
				}
			}

			var incomingAccount = Preferences.GetString(PreferenceName.AccountingIncomeAccount);
			if (incomingAccount == account.Id.ToString())
			{
				throw new Exception(Translation.Accounting.AccountInUseInSetup);
			}

			var cashIncomeAccount = Preferences.GetString(PreferenceName.AccountingCashIncomeAccount);
			if (cashIncomeAccount == account.Id.ToString())
			{
				throw new Exception(Translation.Accounting.AccountInUseInSetup);
			}

			var autoPays = AccountingAutoPays.GetDeepCopy();
			for (int i = 0; i < autoPays.Count; i++)
			{
				depositAccountIds = autoPays[i].PickList.Split(new char[] { ',' });
				for (int s = 0; s < depositAccountIds.Length; s++)
				{
					if (depositAccountIds[s] == account.Id.ToString())
					{
						throw new Exception(Translation.Accounting.AccountInUseInSetup);
					}
				}
			}

			ExecuteDelete(account);
		}

		/// <summary>
		/// Used to test the sign on debits and credits for the five different account types
		/// </summary>
		public static bool DebitIsPos(char accountType)
		{
			switch (accountType)
			{
				case AccountType.Asset:
				case AccountType.Expense:
					return true;

				case AccountType.Liability:
				case AccountType.Equity: // Because liabilities and equity are treated the same.
				case AccountType.Income:
					return false;
			}

			return true;
		}

		/// <summary>
		/// Gets the balance of an account directly from the database.
		/// </summary>
		public static double GetBalance(long accountId, char accountType)
		{
			string command = "SELECT SUM(DebitAmt), SUM(CreditAmt) FROM `journal_entries` WHERE `account_id` = " + accountId + " GROUP BY `account_id`";
			DataTable table = Database.ExecuteDataTable(command);
			
			double debit = 0;
			double credit = 0;
			if (table.Rows.Count > 0)
			{
				debit = PIn.Double(table.Rows[0][0].ToString());
				credit = PIn.Double(table.Rows[0][1].ToString());
			}

			return DebitIsPos(accountType) ? 
				debit - credit : 
				credit - debit;
		}

		/// <summary>
		/// Checks the loaded prefs to see if user has setup deposit linking.  Returns true if so.
		/// </summary>
		public static bool DepositsLinked()
		{
			if (PrefC.GetInt(PreferenceName.AccountingSoftware) == (int)AccountingSoftware.QuickBooks)
			{
				if (Preferences.GetString(PreferenceName.QuickBooksDepositAccounts) == "")
				{
					return false;
				}
				if (Preferences.GetString(PreferenceName.QuickBooksIncomeAccount) == "")
				{
					return false;
				}
			}
			else
			{
				if (Preferences.GetString(PreferenceName.AccountingDepositAccounts) == "")
				{
					return false;
				}
				if (Preferences.GetLong(PreferenceName.AccountingIncomeAccount) == 0)
				{
					return false;
				}
			}

			return true;
		}

		///<summary>Checks the loaded prefs and accountingAutoPays to see if user has setup auto pay linking.  Returns true if so.</summary>
		public static bool PaymentsLinked()
		{
			if (AccountingAutoPays.GetCount() == 0)
			{
				return false;
			}

			if (Preferences.GetLong(PreferenceName.AccountingCashIncomeAccount) == 0)
			{
				return false;
			}
			//might add a few more checks later.
			return true;
		}

		public static long[] GetDepositAccounts()
		{
			var depositAccountIds = new List<long>();
			var depositAccounts = Preferences.GetString(PreferenceName.AccountingDepositAccounts, "").Split(',');

			foreach (var depositAccount in depositAccounts)
            {
				if (long.TryParse(depositAccount.Trim(), out var depositAccountId))
                {
					depositAccountIds.Add(depositAccountId);

				}
            }

			return depositAccountIds.ToArray();
		}

		///<summary></summary>
		public static List<string> GetDepositAccountsQB()
		{
			//No need to check RemotingRole; no call to db.
			string depStr = Preferences.GetString(PreferenceName.QuickBooksDepositAccounts);
			string[] depStrArray = depStr.Split(new char[] { ',' });
			List<string> retVal = new List<string>();
			for (int i = 0; i < depStrArray.Length; i++)
			{
				if (depStrArray[i] == "")
				{
					continue;
				}
				retVal.Add(depStrArray[i]);
			}
			return retVal;
		}

		///<summary></summary>
		public static List<string> GetIncomeAccountsQB()
		{
			//No need to check RemotingRole; no call to db.
			string incomeStr = Preferences.GetString(PreferenceName.QuickBooksIncomeAccount);
			string[] incomeStrArray = incomeStr.Split(new char[] { ',' });
			List<string> retVal = new List<string>();
			for (int i = 0; i < incomeStrArray.Length; i++)
			{
				if (incomeStrArray[i] == "")
				{
					continue;
				}
				retVal.Add(incomeStrArray[i]);
			}
			return retVal;
		}

		/// <summary>
		/// Gets the full list to display in the Chart of Accounts, including balances.
		/// </summary>
		public static List<AccountSummary> GetSummaries(DateTime asOfDate, bool showInactive)
		{
			var accountSummaries = new List<AccountSummary>();

			static AccountSummary FromReader(MySqlDataReader dataReader)
			{
				var accountSummary = new AccountSummary
				{
					AccountId = (long)dataReader["account_id"],
					Description = (string)dataReader["description"],
					BankNumber = (string)dataReader["bank_number"],
					Inactive = (bool)dataReader["inactive"],
					Color = Color.FromArgb((int)dataReader["color"]),
					Type = (char)dataReader["type"]
				};

				var amtDebit = (double)dataReader["debit_amt"];
				var amtCredit = (double)dataReader["credit_amt"];

				accountSummary.Balance =
					DebitIsPos(accountSummary.Type) ?
						amtDebit - amtCredit :
						amtCredit - amtDebit;

				return accountSummary;
			}

			//
			// Asset and Liability accounts...
			//

			var command =
				"SELECT a.`id`, a.`description`, a.`type`, a.`bank_number`, a.`color`, SUM(je.`debit`) AS `debit`, SUM(je.`credit`) AS `credit` " +
				"FROM `accounts` a " +
				"LEFT JOIN `journal_entries` je ON je.`account_id` = a.`id` " +
				"WHERE je.`date_displayed` <= @date AND (a.`type` = 'A' OR a.type = 'L') ";

			if (!showInactive) command += "AND a.`inactive` = 0 ";

			command += 
				"GROUP BY a.`account_id`, a.`type`, a.`description`, a.`bank_number`, a.`inactive`, a.`color` " +
				"ORDER BY a.`type`, a.`description`";

			accountSummaries.AddRange(
				Database.SelectMany(
					command, FromReader,
						new MySqlParameter("date", asOfDate)));

			//
			// Retained earnings (auto) account...
			//

			command =
				"SELECT SUM(je.`credit`) - SUM(je.`debit`) " +
				"FROM `accounts` a " +
				"LEFT JOIN `journal_entries` je ON je.`account_id` = a.`id` " +
				"WHERE je.`date_displayed` <= @date AND (a.`type` = 'I' OR a.`type` = 'E')";

			var balance = 
				Database.SelectOne(command, Database.ToScalar<double>,
					new MySqlParameter("date", asOfDate));

			accountSummaries.Add(new AccountSummary
			{
				AccountId = 0,
				Description = Translation.Accounting.RetainedEarningsAuto,
				BankNumber = "",
				Balance = balance,
				Inactive = false,
				Color = Color.White,
				Type = AccountType.Equity
			});

			//
			// Income and Expense accounts...
			//

			command =
				"SELECT a.`id`, a.`description`, a.`type`, a.`bank_number`, a.`color`, SUM(je.`debit`) AS `debit`, SUM(je.`credit`) AS `credit` " +
				"FROM `accounts` a " +
				"LEFT JOIN `journal_entries` je ON je.`account_id` = a.`id` " +
				"WHERE je.`date_displayed` <= @date AND je.`date_displayed` >= @first_of_year " +
				"AND (a.`type` = 'I' OR a.type = 'E') ";

			if (!showInactive) command += "AND a.`inactive` = 0 ";

			command += 
				"GROUP BY a.`account_id`, a.`type`, a.`description`, a.`bank_number`, a.`inactive`, a.`color` " +
				"ORDER BY a.`type`, a.`description`";

			accountSummaries.AddRange(
				Database.SelectMany(
					command, FromReader,
						new MySqlParameter("date", asOfDate),
						new MySqlParameter("first_of_year", new DateTime(asOfDate.Year, 1, 1))));

			return accountSummaries;
		}

		/// <summary>
		/// Gets the full GeneralLedger list.
		/// </summary>
		public static DataTable GetGeneralLedger(DateTime dateStart, DateTime dateEnd)
		{
			string queryString = @"SELECT DATE(" + POut.Date(new DateTime(dateStart.Year - 1, 12, 31)) + @") DateDisplayed,
				'' Memo,
				'' Splits,
				'' CheckNumber,
				startingbals.SumTotal DebitAmt,
				0 CreditAmt,
				'' Balance,
				startingbals.Description,
				startingbals.AcctType,
				startingbals.AccountNum
				FROM (
					SELECT account.AccountNum,
					account.Description,
					account.AcctType,
					ROUND(SUM(journalentry.DebitAmt-journalentry.CreditAmt),2) SumTotal
					FROM account
					INNER JOIN journalentry ON journalentry.AccountNum=account.AccountNum
					AND journalentry.DateDisplayed < " + POut.Date(dateStart) + @" 
					AND account.AcctType IN (0,1,2)/*assets,liablities,equity*/
					GROUP BY account.AccountNum
				) startingbals

				UNION ALL
	
				SELECT journalentry.DateDisplayed,
				journalentry.Memo,
				journalentry.Splits,
				journalentry.CheckNumber,
				journalentry.DebitAmt, 
				journalentry.CreditAmt,
				'' Balance,
				account.Description,
				account.AcctType,
				account.AccountNum 
				FROM account
				LEFT JOIN journalentry ON account.AccountNum=journalentry.AccountNum 
					AND journalentry.DateDisplayed >= " + POut.Date(dateStart) + @" 
					AND journalentry.DateDisplayed <= " + POut.Date(dateEnd) + @" 
				WHERE account.AcctType IN(0,1,2)
				
				UNION ALL 
				
				SELECT journalentry.DateDisplayed, 
				journalentry.Memo, 
				journalentry.Splits, 
				journalentry.CheckNumber,
				journalentry.DebitAmt, 
				journalentry.CreditAmt, 
				'' Balance,
				account.Description, 
				account.AcctType,
				account.AccountNum 
				FROM account 
				LEFT JOIN journalentry ON account.AccountNum=journalentry.AccountNum 
					AND journalentry.DateDisplayed >= " + POut.Date(dateStart) + @"  
					AND journalentry.DateDisplayed <= " + POut.Date(dateEnd) + @"  
				WHERE account.AcctType IN(3,4)
				
				ORDER BY AcctType, Description, DateDisplayed;";
			return Database.ExecuteDataTable(queryString);
		}

		/// <summary>
		/// Gets the full list to display in the Chart of Accounts, including balances.
		/// </summary>
		public static DataTable GetAssetTable(DateTime asOfDate) 
			=> GetAccountTotalByType(asOfDate, AccountType.Asset);

		/// <summary>
		/// Gets the full list to display in the Chart of Accounts, including balances.
		/// </summary>
		public static DataTable GetLiabilityTable(DateTime asOfDate) 
			=> GetAccountTotalByType(asOfDate, AccountType.Liability);

		/// <summary>
		/// Gets the full list to display in the Chart of Accounts, including balances.
		/// </summary>
		public static DataTable GetEquityTable(DateTime asOfDate) 
			=> GetAccountTotalByType(asOfDate, AccountType.Equity);

		public static DataTable GetAccountTotalByType(DateTime asOfDate, char accountType)
		{
            string sumTotalStr;
            if (accountType == AccountType.Asset)
			{
				sumTotalStr = "SUM(ROUND(DebitAmt,3)-ROUND(CreditAmt,3))";
			}
			else
			{//Liability or equity
				sumTotalStr = "SUM(ROUND(CreditAmt,3)-ROUND(DebitAmt,3))";
			}

			string command = "SELECT Description, " + sumTotalStr + " SumTotal, AcctType "
				+ "FROM account, journalentry "
				+ "WHERE account.AccountNum=journalentry.AccountNum AND DateDisplayed <= " + POut.Date(asOfDate) + " AND AcctType=" + POut.Int((int)accountType) + " "
				+ "GROUP BY account.AccountNum "
				+ "ORDER BY Description, DateDisplayed ";

			return Database.ExecuteDataTable(command);
		}

		public static DataTable GetAccountTotalByType(DateTime dateStart, DateTime dateEnd, char accountType)
		{
			string sumTotalStr = "";
			if (accountType == AccountType.Expense)
			{
				sumTotalStr = "SUM(ROUND(DebitAmt,3)-ROUND(CreditAmt,3))";
			}
			else
			{//Income instead of expense
				sumTotalStr = "SUM(ROUND(CreditAmt,3)-ROUND(DebitAmt,3))";
			}
			string command = "SELECT Description, " + sumTotalStr + " SumTotal, AcctType "
				+ "FROM account, journalentry "
				+ "WHERE account.AccountNum=journalentry.AccountNum AND DateDisplayed >= " + POut.Date(dateStart) + " "
				+ "AND DateDisplayed <= " + POut.Date(dateEnd) + " "
				+ "AND AcctType=" + accountType + " "
				+ "GROUP BY account.AccountNum "
				+ "ORDER BY Description, DateDisplayed ";
			return Database.ExecuteDataTable(command);
		}

		///<Summary>Gets sum of all income-expenses for all previous years. asOfDate could be any date</Summary>
		public static double RetainedEarningsAuto(DateTime asOfDate)
		{
			DateTime firstOfYear = new DateTime(asOfDate.Year, 1, 1);
			string command = "SELECT SUM(ROUND(CreditAmt,3)), SUM(ROUND(DebitAmt,3)), AcctType "
			+ "FROM journalentry,account "
			+ "WHERE journalentry.AccountNum=account.AccountNum "
			+ "AND DateDisplayed < " + POut.Date(firstOfYear)
			+ " GROUP BY AcctType";
			DataTable table = Database.ExecuteDataTable(command);
			double retVal = 0;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				if (table.Rows[i][2].ToString() == "3"//income
					|| table.Rows[i][2].ToString() == "4")//expense
				{
					retVal += PIn.Double(table.Rows[i][0].ToString());//add credit
					retVal -= PIn.Double(table.Rows[i][1].ToString());//subtract debit
																	  //if it's an expense, we are subtracting (income-expense), but the signs cancel.
				}
			}
			return retVal;
		}

		///<Summary>asOfDate is typically 12/31/...  </Summary>
		public static double NetIncomeThisYear(DateTime asOfDate)
		{
			DateTime firstOfYear = new DateTime(asOfDate.Year, 1, 1);
			string command = "SELECT SUM(ROUND(CreditAmt,3)), SUM(ROUND(DebitAmt,3)), AcctType "
			+ "FROM journalentry,account "
			+ "WHERE journalentry.AccountNum=account.AccountNum "
			+ "AND DateDisplayed >= " + POut.Date(firstOfYear)
			+ " AND DateDisplayed <= " + POut.Date(asOfDate)
			+ " GROUP BY AcctType";
			DataTable table = Database.ExecuteDataTable(command);
			double retVal = 0;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				if (table.Rows[i][2].ToString() == "3"//income
					|| table.Rows[i][2].ToString() == "4")//expense
				{
					retVal += PIn.Double(table.Rows[i][0].ToString());//add credit
					retVal -= PIn.Double(table.Rows[i][1].ToString());//subtract debit
																	  //if it's an expense, we are subtracting (income-expense), but the signs cancel.
				}
			}
			return retVal;
		}
	}
}
