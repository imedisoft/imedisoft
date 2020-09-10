using Imedisoft.Data;
using Imedisoft.Data.Cache;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public partial class AutoCodeConds
	{
		[CacheGroup(nameof(InvalidType.AutoCodes))]
		private class AutoCodeCondCache : ListCache<AutoCodeCond>
		{
			protected override IEnumerable<AutoCodeCond> Load()
				=> SelectMany("SELECT * FROM `auto_code_conditions` ORDER BY `cond`");
		}

		private static readonly AutoCodeCondCache cache = new AutoCodeCondCache();

		public static void RefreshCache()
			=> cache.Refresh();

		public static List<AutoCodeCond> GetAll()
			=> cache.GetAll();

		public static List<AutoCodeCond> GetWhere(Predicate<AutoCodeCond> predicate)
			=> cache.Find(predicate);

		public static List<AutoCodeCond> GetByAutoCodeItem(long autoCodeItemId)
			=> GetWhere(x => x.AutoCodeItemId == autoCodeItemId);

		public static void DeleteForAutoCodeItemId(long autoCodeItemId)
			=> Database.ExecuteNonQuery("DELETE FROM auto_code_conditions WHERE auto_code_item_id = " + autoCodeItemId);

		public static bool ConditionIsMet(AutoCondition myAutoCondition, string toothNum, string surf, bool isAdditional, bool willBeMissing, int age)
		{
			switch (myAutoCondition)
			{
				case AutoCondition.Anterior:
					return Tooth.IsAnterior(toothNum);

				case AutoCondition.Posterior:
					return Tooth.IsPosterior(toothNum);

				case AutoCondition.Premolar:
					return Tooth.IsPreMolar(toothNum);

				case AutoCondition.Molar:
					return Tooth.IsMolar(toothNum);

				case AutoCondition.One_Surf: return surf.Length == 1;
				case AutoCondition.Two_Surf: return surf.Length == 2;
				case AutoCondition.Three_Surf: return surf.Length == 3;
				case AutoCondition.Four_Surf: return surf.Length == 4;
				case AutoCondition.Five_Surf: return surf.Length == 5;

				case AutoCondition.First:
					return !isAdditional;

				case AutoCondition.EachAdditional:
					return isAdditional;

				case AutoCondition.Maxillary:
					return Tooth.IsMaxillary(toothNum);

				case AutoCondition.Mandibular:
					return !Tooth.IsMaxillary(toothNum);

				case AutoCondition.Primary:
					return Tooth.IsPrimary(toothNum);

				case AutoCondition.Permanent:
					return !Tooth.IsPrimary(toothNum);

				case AutoCondition.Pontic:
					return willBeMissing;

				case AutoCondition.Retainer:
					return !willBeMissing;

                case AutoCondition.AgeOver18:
					return age > 18;
			}

			return false;
		}
	}
}
