using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class AlertCategories
	{
		[CacheGroup(nameof(InvalidType.AlertCategories))]
		private class AlertCategoryCache : ListCache<AlertCategory>
		{
			protected override IEnumerable<AlertCategory> Load()
				=> SelectMany("SELECT * FROM `alert_categories`");
		}

		private static readonly AlertCategoryCache cache = new AlertCategoryCache();

		public static List<AlertCategory> GetAll()
			=> cache.GetAll();

		public static void RefreshCache()
			=> cache.Refresh();

		public static long Insert(AlertCategory alertCategory)
			=> ExecuteInsert(alertCategory);

		public static void Update(AlertCategory alertCategory) 
			=> ExecuteUpdate(alertCategory);

		public static void Delete(AlertCategory alertCategory) 
			=> ExecuteDelete(alertCategory);
	}
}
