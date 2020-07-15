using DataConnectionBase;
using OpenDentBusiness;
using System;
using System.Data;

namespace OpenDentalGraph.Cache
{
    public class DashboardCacheNewPatient : DashboardCacheWithQuery<NewPatient>
	{
		protected override string GetCommand(DashboardFilter filter)
		{
			string where = "ProcStatus=" + SOut.Int((int)ProcStat.C);
			if (filter.UseProvFilter)
			{
				where += " AND ProvNum=" + SOut.Long(filter.ProvNum);
			}

			return
				"SELECT PatNum, MIN(ProcDate) FirstProc, ClinicNum, ProvNum " +
				"FROM procedurelog USE INDEX(indexPNPSCN) " +
				"INNER JOIN procedurecode ON procedurecode.CodeNum = procedurelog.CodeNum " +
				"AND procedurecode.ProcCode NOT IN ('D9986','D9987') " +
				"WHERE " + where + " GROUP BY PatNum";
		}

		protected override NewPatient GetInstanceFromDataRow(DataRow x)
		{
			return new NewPatient()
			{
				DateStamp = SIn.Date(x["FirstProc"].ToString()),
				Count = 1, // Each row counts as 1.
				Val = 0, // There are no fees
				SeriesName = "All",
				ProvNum = SIn.Long(x["ProvNum"].ToString()),
				ClinicNum = SIn.Long(x["ClinicNum"].ToString()),
			};
		}

		protected override bool AllowQueryDateFilter()
		{
			return false;
		}
	}

	public class NewPatient : GraphQuantityOverTime.GraphDataPointClinic
	{
	}
}
