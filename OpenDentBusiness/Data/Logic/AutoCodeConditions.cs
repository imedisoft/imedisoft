using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class AutoCodeConditions
	{
		[CacheGroup(nameof(InvalidType.AutoCodes))]
		private class AutoCodeCondCache : ListCache<AutoCodeCondition>
		{
			protected override IEnumerable<AutoCodeCondition> Load()
				=> SelectMany("SELECT * FROM `auto_code_conditions` ORDER BY `cond`");
		}

		private static readonly AutoCodeCondCache cache = new AutoCodeCondCache();

		public static void RefreshCache()
			=> cache.Refresh();

		public static List<AutoCodeCondition> GetAll()
			=> cache.GetAll();

		public static List<AutoCodeCondition> Find(Predicate<AutoCodeCondition> predicate)
			=> cache.Find(predicate);

		public static List<AutoCodeCondition> GetByAutoCodeItem(long autoCodeItemId)
			=> Find(x => x.AutoCodeItemId == autoCodeItemId);

		public static void DeleteForAutoCodeItemId(long autoCodeItemId)
			=> Database.ExecuteNonQuery("DELETE FROM `auto_code_conditions` WHERE auto_code_item_id = " + autoCodeItemId);

		public static void Save(AutoCodeCondition autoCodeCondition)
        {
			if (autoCodeCondition.Id == 0) ExecuteInsert(autoCodeCondition);
            else
            {
				ExecuteUpdate(autoCodeCondition);
            }
        }

		public static bool ConditionIsMet(AutoCodeConditionType conditionType, string tooth, string surfaces, bool isAdditional, bool willBeMissing, int age)
		{
            return conditionType switch
            {
                AutoCodeConditionType.Anterior => Tooth.IsAnterior(tooth),
                AutoCodeConditionType.Posterior => Tooth.IsPosterior(tooth),
                AutoCodeConditionType.Premolar => Tooth.IsPreMolar(tooth),
                AutoCodeConditionType.Molar => Tooth.IsMolar(tooth),
                AutoCodeConditionType.One_Surf => surfaces.Length == 1,
                AutoCodeConditionType.Two_Surf => surfaces.Length == 2,
                AutoCodeConditionType.Three_Surf => surfaces.Length == 3,
                AutoCodeConditionType.Four_Surf => surfaces.Length == 4,
                AutoCodeConditionType.Five_Surf => surfaces.Length == 5,
                AutoCodeConditionType.First => !isAdditional,
                AutoCodeConditionType.EachAdditional => isAdditional,
                AutoCodeConditionType.Maxillary => Tooth.IsMaxillary(tooth),
                AutoCodeConditionType.Mandibular => !Tooth.IsMaxillary(tooth),
                AutoCodeConditionType.Primary => Tooth.IsPrimary(tooth),
                AutoCodeConditionType.Permanent => !Tooth.IsPrimary(tooth),
                AutoCodeConditionType.Pontic => willBeMissing,
                AutoCodeConditionType.Retainer => !willBeMissing,
                AutoCodeConditionType.AgeOver18 => age > 18,
                _ => false,
            };
        }
	}
}
