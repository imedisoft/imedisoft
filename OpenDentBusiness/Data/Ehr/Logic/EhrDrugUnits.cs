using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class EhrDrugUnits
	{
        [CacheGroup(nameof(InvalidType.Vaccines))]
        private class EhrDrugUnitCache : ListCache<EhrDrugUnit>
        {
			protected override IEnumerable<EhrDrugUnit> Load() 
				=> SelectMany("SELECT * FROM `ehr_drug_units` ORDER BY `code`");
        }

        private static readonly EhrDrugUnitCache cache = new EhrDrugUnitCache();

		public static void RefreshCache() 
			=> cache.Refresh();

		public static List<EhrDrugUnit> GetAll()
			=> cache.GetAll();

		public static EhrDrugUnit GetByCode(string drugUnitCode) 
			=> SelectOne(drugUnitCode);

		public static void Save(EhrDrugUnit drugUnit) 
			=> Database.ExecuteNonQuery(
				"INSERT INTO `ehr_drug_units` (`code`, `description`) " +
				"VALUES (@code, @description) " +
				"ON DUPLICATE KEY UPDATE `description` = @description",
					new MySqlParameter("code", drugUnit.Code ?? ""),
					new MySqlParameter("description", drugUnit.Description ?? ""));

		public static void Delete(EhrDrugUnit drugUnit)
		{
			var count = Database.ExecuteLong(
				"SELECT COUNT(*) FROM `ehr_patient_vaccines` WHERE `drug_unit_code` = @code", 
					new MySqlParameter("@code", drugUnit.Code ?? ""));

			if (count != 0)
			{
				throw new Exception(Translation.Ehr.UnableToDeleteDrugUnitIsInUse);
			}

			ExecuteDelete(drugUnit);
		}
	}
}
