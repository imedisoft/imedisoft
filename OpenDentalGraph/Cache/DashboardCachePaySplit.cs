using DataConnectionBase;
using OpenDentBusiness;
using System;
using System.Data;

namespace OpenDentalGraph.Cache
{
    public class DashboardCachePaySplit : DashboardCacheWithQuery<PaySplit>
	{
		protected override string GetCommand(DashboardFilter filter)
		{
			string where = "";
			if (filter.UseDateFilter)
			{
				where = "DatePay BETWEEN " + SOut.Date(filter.DateFrom) + " AND " + SOut.Date(filter.DateTo) + " AND ";
			}

			if (filter.UseProvFilter)
			{
				where += "ProvNum=" + SOut.Long(filter.ProvNum) + " AND ";
			}

			return
				"SELECT ProvNum,DatePay,SUM(SplitAmt) AS GrossSplit,ClinicNum " +
				"FROM paysplit WHERE " + where + "IsDiscount=0 " +
				"GROUP BY ProvNum,DatePay,ClinicNum ";
		}

		protected override PaySplit GetInstanceFromDataRow(DataRow x)
		{
			return new PaySplit()
			{
				ProvNum = SIn.Long(x["ProvNum"].ToString()),
				DateStamp = SIn.Date(x["DatePay"].ToString()),
				Val = SIn.Double(x["GrossSplit"].ToString()),
				Count = 0, // Counting paysplits is not useful
				ClinicNum = SIn.Long(x["ClinicNum"].ToString()),
			};
		}
	}

	public class PaySplit : GraphQuantityOverTime.GraphDataPointClinic
	{
	}
}
