using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imedisoft.Data
{
    public partial class AccountingAutoPays
	{
		[CacheGroup(nameof(InvalidType.AccountingAutoPays))]
		private class AccountingAutoPayCache : ListCache<AccountingAutoPay>
		{
			protected override IEnumerable<AccountingAutoPay> Load()
				=> SelectMany("SELECT * FROM `accounting_auto_pays`");
		}

		private static readonly AccountingAutoPayCache cache = new AccountingAutoPayCache();

		public static List<AccountingAutoPay> GetDeepCopy() 
			=> cache.GetAll();

		public static AccountingAutoPay GetFirstOrDefault(Predicate<AccountingAutoPay> match) 
			=> cache.FirstOrDefault(match);

		public static int GetCount() 
			=> cache.Count();

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void Delete(AccountingAutoPay accountingAutoPay) => ExecuteDelete(accountingAutoPay);

		/// <summary>
		/// Converts the comma delimited list of AccountNums into full descriptions separated by carriage returns.
		/// </summary>
		public static string GetPickListDesc(AccountingAutoPay accountingAutoPay)
		{
			var items = accountingAutoPay.PickList.Split('.');

			var stringBuilder = new StringBuilder();

			foreach (var item in items)
			{
				if (long.TryParse(item, out var accountId))
				{
					stringBuilder.AppendLine(Accounts.GetDescription(accountId));
				}
			}

			return stringBuilder.ToString().Trim();
		}

		/// <summary>
		/// Converts the comma delimited list of AccountNums into an array of AccountNums.
		/// </summary>
		public static IEnumerable<long> GetPickListAccounts(AccountingAutoPay accountingAutoPay)
		{
			var items = accountingAutoPay.PickList.Split('.');

			foreach (var item in items)
            {
				if (long.TryParse(item, out var accountId))
                {
					yield return accountId;
                }
            }
		}

		/// <summary>
		/// Loops through the AList to find one with the specified payType (defNum). If none is found, then it returns null.
		/// </summary>
		public static AccountingAutoPay GetForPayType(long payType) 
			=> GetFirstOrDefault(x => x.PayType == payType);

		/// <summary>
		/// Saves the list of accountingAutoPays to the database. Deletes all existing ones first.
		/// </summary>
		public static void SaveList(List<AccountingAutoPay> accountingAutoPays)
		{
			Database.ExecuteNonQuery("DELETE FROM `accounting_auto_pays`");

			foreach (var accountingAutoPay in accountingAutoPays)
            {
				ExecuteInsert(accountingAutoPay);
            }
		}
	}
}
