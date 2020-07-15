using DataConnectionBase;
using OpenDentBusiness;
using System;
using System.Data;

namespace OpenDentalGraph.Cache
{
    public class DashboardCacheCompletedProc : DashboardCacheWithQuery<CompletedProc>
	{
		protected override string GetCommand(DashboardFilter filter)
		{
			string where = "WHERE procedurelog.ProcStatus=" + SOut.Int((int)ProcStat.C) + " ";
			if (filter.UseDateFilter)
			{
				where += "AND procedurelog.ProcDate BETWEEN " + SOut.Date(filter.DateFrom) + " AND " + SOut.Date(filter.DateTo) + " ";
			}

			if (filter.UseProvFilter)
			{
				where += "AND ProvNum=" + SOut.Long(filter.ProvNum) + " ";
			}

			return
				"SELECT procedurelog.ProcDate,procedurelog.ProvNum,procedurelog.ClinicNum, " +
				"SUM(procedurelog.ProcFee*(procedurelog.UnitQty+procedurelog.BaseUnits)) AS GrossProd, " +
				"COUNT(procedurelog.ProcNum) AS ProcCount " +
				"FROM procedurelog " + where + 
				"GROUP BY procedurelog.ProcDate,procedurelog.ProvNum,procedurelog.ClinicNum ";
		}

		protected override CompletedProc GetInstanceFromDataRow(DataRow x)
		{
			return new CompletedProc()
			{
				ProvNum = SIn.Long(x["ProvNum"].ToString()),
				DateStamp = SIn.Date(x["ProcDate"].ToString()),
				Val = SIn.Double(x["GrossProd"].ToString()),
				Count = SIn.Long(x["ProcCount"].ToString()),
				ClinicNum = SIn.Long(x["ClinicNum"].ToString()),
			};
		}
	}

	public class CompletedProc : GraphQuantityOverTime.GraphDataPointClinic { }
}
