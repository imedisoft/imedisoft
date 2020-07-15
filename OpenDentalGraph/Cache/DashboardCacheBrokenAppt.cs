using DataConnectionBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OpenDentalGraph.Cache
{
    public class DashboardCacheBrokenAppt : DashboardCacheWithQuery<BrokenAppt>
	{
		protected override string GetCommand(DashboardFilter filter)
		{
			string where = "WHERE AptStatus=" + (int)ApptStatus.Broken + " ";
			if (filter.UseDateFilter)
			{
				where += "AND DATE(AptDateTime) BETWEEN " + SOut.Date(filter.DateFrom) + " AND " + SOut.Date(filter.DateTo) + " ";
			}

			if (filter.UseProvFilter)
			{
				where += "AND ProvNum=" + SOut.Long(filter.ProvNum) + " ";
			}

			return
				"SELECT DATE(AptDateTime) ApptDate,ProvNum,ClinicNum,COUNT(AptNum) ApptCount " +
				"FROM appointment " + where + "GROUP BY ApptDate,ProvNum,ClinicNum ";
		}

		protected override BrokenAppt GetInstanceFromDataRow(DataRow x)
		{
			return new BrokenAppt()
			{
				ProvNum = SIn.Long(x["ProvNum"].ToString()),
				ClinicNum = SIn.Long(x["ClinicNum"].ToString()),
				DateStamp = SIn.Date(x["ApptDate"].ToString()),
				Count = SIn.Long(x["ApptCount"].ToString()),
				Val = 0, // Appointments do not have their own value.
			};
		}
	}

	public class BrokenAppt : GraphQuantityOverTime.GraphDataPointClinic
	{
	}
}
