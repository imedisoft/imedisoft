using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenDentalGraph.Cache
{
    public class DashboardCacheClinic : DashboardCacheBase<Clinic>
	{
		private Dictionary<long, string> clinicNames = new Dictionary<long, string>();

		protected override List<Clinic> GetCache(DashboardFilter filter)
		{
			List<Clinic> list = Clinics.GetAll(false);
			clinicNames = list.ToDictionary(x => x.Id, x => string.IsNullOrEmpty(x.Description) ? x.Id.ToString() : x.Description);
			return list;
		}

		public string GetClinicName(long clinicNum)
		{
			string clinicName;

			if (!clinicNames.TryGetValue(clinicNum, out clinicName))
			{
				if (clinicNum == 0)
				{
					return "Unassigned";
				}
				return clinicNum.ToString();
			}

			return clinicName;
		}
	}
}
