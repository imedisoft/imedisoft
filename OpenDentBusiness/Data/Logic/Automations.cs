using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class Automations
	{
		[CacheGroup(nameof(InvalidType.Automation))]
		private class AutomationCache : ListCache<Automation>
		{
            protected override IEnumerable<Automation> Load() 
				=> SelectMany("SELECT * FROM `automations`");
		}

		private static readonly AutomationCache cache = new AutomationCache();

		public static List<Automation> GetAll() 
			=> cache.GetAll();

		public static Automation GetFirstOrDefault(Predicate<Automation> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void Save(Automation automation)
        {
			if (automation.Id == 0) ExecuteInsert(automation);
            else
            {
				ExecuteUpdate(automation);
            }
        }

		public static void Delete(Automation automation) 
			=> ExecuteDelete(automation);
	}
}
