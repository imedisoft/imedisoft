using Imedisoft.Data.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public class AlertCategories
	{
		[CacheGroup(nameof(InvalidType.AlertCategories))]
		private class AlertCategoryCache : ListCache<AlertCategory>
		{
			protected override IEnumerable<AlertCategory> Load()
				=> Crud.AlertCategoryCrud.SelectMany("SELECT * FROM alertcategory");
		}

		private static readonly AlertCategoryCache cache = new AlertCategoryCache();

		public static List<AlertCategory> GetDeepCopy()
			=> cache.GetAll();

		public static void RefreshCache()
			=> cache.Refresh();

		public static AlertCategory GetOne(long alertCategoryNum)
		{
			return Crud.AlertCategoryCrud.SelectOne(alertCategoryNum);
		}

		public static long Insert(AlertCategory alertCategory)
		{
			return Crud.AlertCategoryCrud.Insert(alertCategory);
		}

		public static void Update(AlertCategory alertCategory)
		{
			Crud.AlertCategoryCrud.Update(alertCategory);
		}

		public static void Delete(long alertCategoryNum)
		{
			Crud.AlertCategoryCrud.Delete(alertCategoryNum);
		}
	}
}
