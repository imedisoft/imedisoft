namespace ODR
{
    public class Aggregate
	{
		private static decimal runningSum;
		private static string groupByVal;

		///<summary>Used to test the sign on debits and credits for the five different account types.  Pass in a number in string format.  Like "2", for example.</summary>
		public static bool AccountDebitIsPos(string accountType)
		{
			switch (accountType)
			{
				case "0"://asset
				case "4"://expense
					return true;

				case "1"://liability
				case "2"://equity //because liabilities and equity are treated the same
				case "3"://revenue
					return false;
			}
			return true;
		}

		public static string RunningSumForAccounts(object groupBy, object debitAmt, object creditAmt, object acctType)
		{
			if (debitAmt == null || creditAmt == null)
			{
				return 0.ToString("N");
			}
			try
			{
				//Cannot read debitAmt and creditAmt as decimals because it makes the general ledger detail report fail.  Simply cast as decimals when doing mathematical operations.
				double debit = (double)debitAmt;//PIn.PDouble(debitAmt);
				double credit = (double)creditAmt;//PIn.PDouble(creditAmt)
				if (groupByVal == null || groupBy.ToString() != groupByVal)
				{//if new or changed group
					runningSum = 0;
				}

				groupByVal = groupBy.ToString();
				if (AccountDebitIsPos(acctType.ToString()))
				{
					runningSum += (decimal)debit - (decimal)credit;
				}
				else
				{
					runningSum += (decimal)credit - (decimal)debit;
				}
				return runningSum.ToString("N");
			}
			catch
			{
				return 0.ToString("N");
			}
		}
	}
}
