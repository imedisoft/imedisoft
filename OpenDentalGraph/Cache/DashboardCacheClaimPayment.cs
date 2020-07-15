using DataConnectionBase;
using OpenDentBusiness;
using System;
using System.Data;

namespace OpenDentalGraph.Cache
{
    public class DashboardCacheClaimPayment : DashboardCacheWithQuery<ClaimPayment>
	{
		protected override string GetCommand(DashboardFilter filter)
		{
			string where = "";
			if (filter.UseDateFilter)
			{
				where = "DateCP BETWEEN " + SOut.Date(filter.DateFrom) + " AND " + SOut.Date(filter.DateTo) + " AND ";
			}

			if (filter.UseProvFilter)
			{
				where += "ProvNum=" + SOut.Long(filter.ProvNum) + " AND ";
			}

			return
				"SELECT ProvNum,DateCP,SUM(InsPayAmt) AS GrossIncome,ClinicNum " +
				"FROM claimproc WHERE " + where + "ClaimPaymentNum<>0 AND InsPayAmt<>0 " +
				"GROUP BY ProvNum,DateCP,ClinicNum ";
		}

		protected override ClaimPayment GetInstanceFromDataRow(DataRow x)
		{
			return new ClaimPayment()
			{
				ProvNum = SIn.Long(x["ProvNum"].ToString()),
				DateStamp = SIn.Date(x["DateCP"].ToString()),
				Val = SIn.Double(x["GrossIncome"].ToString()),
				Count = 0, // Nothing to count for income.
				ClinicNum = SIn.Long(x["ClinicNum"].ToString()),
			};
		}
	}

	public class ClaimPayment : GraphQuantityOverTime.GraphDataPointClinic
	{
	}
}
