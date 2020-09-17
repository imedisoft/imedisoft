using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class AlertCategoryLinks
	{
		[CacheGroup(nameof(InvalidType.AlertCategoryLinks))]
		private class AlertCategoryLinkCache : ListCache<AlertCategoryLink>
		{
			protected override IEnumerable<AlertCategoryLink> Load()
				=> SelectMany("SELECT * FROM `alert_category_links`");
		}

		private static readonly AlertCategoryLinkCache cache = new AlertCategoryLinkCache();

		public static List<AlertCategoryLink> Find(Predicate<AlertCategoryLink> predicate) 
			=> cache.Find(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static AlertCategoryLink GetById(long alertCategoryLinkId) 
			=> SelectOne(alertCategoryLinkId);

		public static IEnumerable<AlertCategoryLink> GetByAlertCategory(long alertCategoryId) 
			=> SelectMany(
				"SELECT * FROM `alert_category_links` WHERE `alert_category_id` = " + alertCategoryId);

		public static void Insert(AlertCategoryLink alertCategoryLink)
			=> ExecuteInsert(alertCategoryLink);

		/// <summary>
		/// Sets the alert types for the specified alert category.
		/// </summary>
		/// <param name="alertCategory"></param>
		/// <param name="alertTypes"></param>
		public static void Assign(AlertCategory alertCategory, IEnumerable<string> alertTypes)
        {
			if (alertCategory.Id == 0) return;

			Database.ExecuteNonQuery("DELETE FROM `alert_category_links` WHERE `alert_category_id` = " + alertCategory.Id);

			var alertTypesList = alertTypes.ToList();
			if (alertTypesList.Count == 0)
            {
				return;
            }

			foreach (var alertType in alertTypesList)
            {
				ExecuteInsert(new AlertCategoryLink
				{
					AlertCategoryId = alertCategory.Id,
					Type = alertType
				});
            }
        }
	}
}
