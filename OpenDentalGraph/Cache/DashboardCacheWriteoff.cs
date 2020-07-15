using DataConnectionBase;
using OpenDentBusiness;
using System;
using System.Data;

namespace OpenDentalGraph.Cache
{
    /// <summary>
    /// We use the same cache to track regular writeoffs and capitation writeoffs for efficiency.
    /// </summary>
    public class DashboardCacheWriteoff : DashboardCacheWithQuery<Writeoff>
	{
		protected override string GetCommand(DashboardFilter filter)
		{
			string where = "WHERE TRUE ";
			if (filter.UseDateFilter)
			{
				where += "AND ProcDate BETWEEN " + SOut.Date(filter.DateFrom) + " AND " + SOut.Date(filter.DateTo) + " ";
			}
			if (filter.UseProvFilter)
			{
				where += "AND ProvNum=" + SOut.Long(filter.ProvNum) + " ";
			}

			return
				"SELECT ProcDate,ProvNum,SUM(WriteOff) AS WriteOffs, IF(claimproc.Status=" + (int)ClaimProcStatus.CapComplete + ",'1','0') AS IsCap, ClinicNum " +
				"FROM claimproc " + where + "AND claimproc.Status IN (" + 
				SOut.Int((int)ClaimProcStatus.Received) + "," +
				SOut.Int((int)ClaimProcStatus.Supplemental) + "," +
				SOut.Int((int)ClaimProcStatus.NotReceived) + "," +
				SOut.Int((int)ClaimProcStatus.CapComplete) + ") " +
				"GROUP BY ProcDate,ProvNum,(claimproc.Status=" + (int)ClaimProcStatus.CapComplete + "),ClinicNum " +
				"HAVING WriteOffs<>0 ";
		}

		protected override Writeoff GetInstanceFromDataRow(DataRow x)
		{
			return new Writeoff()
			{
				ProvNum = SIn.Long(x["ProvNum"].ToString()),
				DateStamp = SIn.Date(x["ProcDate"].ToString()),
				Val = -SIn.Double(x["WriteOffs"].ToString()),
				Count = 0, // Count procedures, not writeoffs.
				ClinicNum = SIn.Long(x["ClinicNum"].ToString()),
				IsCap = SIn.Bool(x.Field<string>("IsCap")),
			};
		}
	}

	public class Writeoff : GraphQuantityOverTime.GraphDataPointClinic
	{
		public bool IsCap;
	}
}
