using DataConnectionBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OpenDentalGraph.Cache
{
    public class DashboardCacheBrokenProcedure : DashboardCacheWithQuery<BrokenProc>
	{
		protected override string GetCommand(DashboardFilter filter)
		{
			string where = "WHERE ProcStatus=" + (int)ProcStat.C + " ";
			if (filter.UseDateFilter)
			{
				where = "AND DATE(ProcDate) BETWEEN " + SOut.Date(filter.DateFrom) + " AND " + SOut.Date(filter.DateTo) + " ";
			}

			if (filter.UseProvFilter)
			{
				where += "AND ProvNum=" + SOut.Long(filter.ProvNum) + " ";
			}

			return
				"SELECT ProcDate,ProvNum,ClinicNum,COUNT(ProcNum) ProcCount, SUM(ProcFee) ProcFee,ProcCode " +
				"FROM procedurelog " +
				"INNER JOIN procedurecode ON procedurecode.CodeNum=procedurelog.CodeNum " +
				"AND procedurecode.ProcCode IN('D9986','D9987') " + where + 
				"GROUP BY ProcDate,ProvNum,ClinicNum,ProcCode ";
		}

		protected override BrokenProc GetInstanceFromDataRow(DataRow x)
		{
			return new BrokenProc()
			{
				ProvNum = SIn.Long(x["ProvNum"].ToString()),
				ClinicNum = SIn.Long(x["ClinicNum"].ToString()),
				DateStamp = SIn.Date(x["ProcDate"].ToString()),
				Count = SIn.Long(x["ProcCount"].ToString()),
				Val = SIn.Double(x["ProcFee"].ToString()),
				ProcCode = SIn.String(x["ProcCode"].ToString())
			};
		}
	}

	public class BrokenProc : GraphQuantityOverTime.GraphDataPointClinic
	{
		public string ProcCode;
	}
}
