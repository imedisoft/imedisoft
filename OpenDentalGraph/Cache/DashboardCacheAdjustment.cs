using DataConnectionBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;

namespace OpenDentalGraph.Cache
{
    public class DashboardCacheAdjustment : DashboardCacheWithQuery<Adjustment>
	{
		protected override string GetCommand(DashboardFilter filter)
		{
			var whereClauses = new List<string>();
			if (filter.UseDateFilter)
			{
				whereClauses.Add("AdjDate BETWEEN " + SOut.Date(filter.DateFrom) + " AND " + SOut.Date(filter.DateTo) + " ");
			}

			if (filter.UseProvFilter)
			{
				whereClauses.Add("ProvNum=" + SOut.Long(filter.ProvNum) + " ");
			}

			string where = "";
			if (whereClauses.Count > 0)
			{
				where = "WHERE " + string.Join("AND ", whereClauses);
			}

			return
				"SELECT AdjDate,ProvNum,SUM(AdjAmt) AdjTotal, ClinicNum " +
				"FROM adjustment " + where + 
				"GROUP BY AdjDate,ProvNum,ClinicNum " +
				"HAVING AdjTotal<>0 " +
				"ORDER BY AdjDate,ProvNum ";
		}

		protected override Adjustment GetInstanceFromDataRow(DataRow x)
		{
			return new Adjustment()
			{
				ProvNum = SIn.Long(x["ProvNum"].ToString()),
				DateStamp = SIn.Date(x["AdjDate"].ToString()),
				Val = SIn.Double(x["AdjTotal"].ToString()),
				Count = 0, // Count procedures, not adjustments.			
				ClinicNum = SIn.Long(x["ClinicNum"].ToString()),
			};
		}
	}

	public class Adjustment : GraphQuantityOverTime.GraphDataPointClinic
	{
	}
}
