using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class EhrDrugManufacturers
	{
		[CacheGroup(nameof(InvalidType.Vaccines))]
		private class EhrDrugManufacturerCache : ListCache<EhrDrugManufacturer>
		{
			protected override IEnumerable<EhrDrugManufacturer> Load() 
				=> SelectMany("SELECT * FROM `ehr_drug_manufacturers` ORDER BY `abbr`");
		}

		private static readonly EhrDrugManufacturerCache cache = new EhrDrugManufacturerCache();

		public static bool Any(Predicate<EhrDrugManufacturer> predicate) 
			=> cache.Any(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static List<EhrDrugManufacturer> GetAll()
			=> cache.GetAll();

		public static EhrDrugManufacturer GetById(long drugManufacturerId) 
			=> SelectOne(drugManufacturerId);

		public static void Save(EhrDrugManufacturer drugManufacturer)
		{
			if (drugManufacturer.Id == 0) ExecuteInsert(drugManufacturer);
            else
            {
				ExecuteUpdate(drugManufacturer);
			}
		}

		public static void Delete(EhrDrugManufacturer drugManufacturer)
		{
			if (Database.ExecuteLong("SELECT COUNT(*) FROM `ehr_vaccines` WHERE `ehr_drug_manufacturer_id` =" + drugManufacturer.Id) != 0)
			{
				throw new Exception(Translation.Ehr.UnableToDeleteDrugManufacturerIsInUse);
			}

			ExecuteDelete(drugManufacturer);
		}
	}
}
