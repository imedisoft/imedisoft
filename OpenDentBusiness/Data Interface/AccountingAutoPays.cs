using Imedisoft.Data;
using Imedisoft.Data.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    public class AccountingAutoPays
	{
		#region Cache Pattern

		[CacheGroup(nameof(InvalidType.AccountingAutoPays))]
		private class AccountingAutoPayCache : ListCache<AccountingAutoPay>
		{
			protected override IEnumerable<AccountingAutoPay> Load()
				=> Crud.AccountingAutoPayCrud.SelectMany("SELECT * FROM accountingautopay");
		}

		private static readonly AccountingAutoPayCache cache = new AccountingAutoPayCache();

		public static List<AccountingAutoPay> GetDeepCopy()
		{
			return cache.GetAll();
		}

		public static AccountingAutoPay GetFirstOrDefault(Predicate<AccountingAutoPay> match)
		{
			return cache.FirstOrDefault(match);
		}

		public static int GetCount()
		{
			return cache.Count();
		}

		public static void RefreshCache()
		{
			cache.Refresh();
		}

		#endregion Cache Pattern

		///<summary></summary>
		public static long Insert(AccountingAutoPay pay)
		{
			return Crud.AccountingAutoPayCrud.Insert(pay);
		}

		///<summary>Converts the comma delimited list of AccountNums into full descriptions separated by carriage returns.</summary>
		public static string GetPickListDesc(AccountingAutoPay pay)
		{
			//No need to check RemotingRole; no call to db.
			string[] numArray = pay.PickList.Split(new char[] { ',' });
			string retVal = "";
			for (int i = 0; i < numArray.Length; i++)
			{
				if (numArray[i] == "")
				{
					continue;
				}
				if (retVal != "")
				{
					retVal += "\r\n";
				}
				retVal += Accounts.GetDescript(PIn.Long(numArray[i]));
			}
			return retVal;
		}

		///<summary>Converts the comma delimited list of AccountNums into an array of AccountNums.</summary>
		public static long[] GetPickListAccounts(AccountingAutoPay pay)
		{
			//No need to check RemotingRole; no call to db.
			string[] numArray = pay.PickList.Split(new char[] { ',' });
			ArrayList AL = new ArrayList();
			for (int i = 0; i < numArray.Length; i++)
			{
				if (numArray[i] == "")
				{
					continue;
				}
				AL.Add(PIn.Long(numArray[i]));
			}
			long[] retVal = new long[AL.Count];
			AL.CopyTo(retVal);
			return retVal;
		}

		///<summary>Loops through the AList to find one with the specified payType (defNum).  If none is found, then it returns null.</summary>
		public static AccountingAutoPay GetForPayType(long payType)
		{
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.PayType == payType);
		}

		///<summary>Saves the list of accountingAutoPays to the database.  Deletes all existing ones first.</summary>
		public static void SaveList(List<AccountingAutoPay> list)
		{
			string command = "DELETE FROM accountingautopay";
			Database.ExecuteNonQuery(command);
			for (int i = 0; i < list.Count; i++)
			{
				Insert(list[i]);
			}
		}
	}
}
