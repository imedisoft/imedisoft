using Imedisoft.Data.Cache;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public partial class AlertCategories
	{
		[CacheGroup(nameof(InvalidType.AlertCategories))]
		private class AlertCategoryCache : ListCache<AlertCategory>
		{
			protected override IEnumerable<AlertCategory> Load()
				=> SelectMany("SELECT * FROM alertcategory");
		}

		private static readonly AlertCategoryCache cache = new AlertCategoryCache();

		public static List<AlertCategory> GetAll()
			=> cache.GetAll();

		public static void RefreshCache()
			=> cache.Refresh();
	}
}
