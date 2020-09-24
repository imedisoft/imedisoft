using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class EhrVaccines
	{
        [CacheGroup(nameof(InvalidType.Vaccines))]
        private class EhrVaccineCache : ListCache<EhrVaccine>
        {
			protected override IEnumerable<EhrVaccine> Load() 
				=> SelectMany("SELECT * FROM `ehr_vaccines` ORDER BY `cvx_code`");
        }

        private static readonly EhrVaccineCache cache = new EhrVaccineCache();

		public static bool Any(Predicate<EhrVaccine> predicate) 
			=> cache.Any(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static List<EhrVaccine> GetAll()
			=> cache.GetAll();

		public static EhrVaccine GetById(long vaccineId) 
			=> SelectOne(vaccineId);

		public static void Save(EhrVaccine ehrVaccine)
		{
			if (ehrVaccine.Id == 0) ExecuteInsert(ehrVaccine);
            else
            {
				ExecuteUpdate(ehrVaccine);
			}
		}

		public static void Delete(EhrVaccine ehrVaccine)
		{
			if (Database.ExecuteLong("SELECT COUNT(*) FROM `ehr_patient_vaccines` WHERE `ehr_vaccine_id` = " + ehrVaccine.Id) != 0)
			{
				throw new Exception(Translation.Ehr.UnableToDeleteVaccineIsInUse);
			}

			ExecuteDelete(ehrVaccine);
		}
	}
}
