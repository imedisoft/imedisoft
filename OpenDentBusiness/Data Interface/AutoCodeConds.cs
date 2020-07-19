using Imedisoft.Data;
using Imedisoft.Data.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
	public class AutoCodeConds
	{
		[CacheGroup(nameof(InvalidType.AutoCodes))]
		private class AutoCodeCondCache : ListCache<AutoCodeCond>
		{
			protected override IEnumerable<AutoCodeCond> Load()
				=> Crud.AutoCodeCondCrud.SelectMany("SELECT * from autocodecond ORDER BY Cond");
		}

		private static readonly AutoCodeCondCache cache = new AutoCodeCondCache();

		public static List<AutoCodeCond> GetDeepCopy()
			=> cache.GetAll();

		public static List<AutoCodeCond> GetWhere(Predicate<AutoCodeCond> match)
			=> cache.Find(match);

		public static void RefreshCache()
			=> cache.Refresh();

		public static long Insert(AutoCodeCond Cur)
			=> Crud.AutoCodeCondCrud.Insert(Cur);

		public static void Update(AutoCodeCond Cur)
			=> Crud.AutoCodeCondCrud.Update(Cur);

		public static void Delete(AutoCodeCond Cur)
			=> Database.ExecuteNonQuery("DELETE from autocodecond WHERE autocodecondnum = " + Cur.AutoCodeCondNum);

		public static void DeleteForItemNum(long itemNum)
			=> Database.ExecuteNonQuery("DELETE from autocodecond WHERE autocodeitemnum = " + itemNum);

		public static List<AutoCodeCond> GetListForItem(long autoCodeItemNum)
			=> GetWhere(x => x.AutoCodeItemNum == autoCodeItemNum);

		public static bool IsSurf(AutoCondition myAutoCondition)
		{
			switch (myAutoCondition)
			{
				case AutoCondition.One_Surf:
				case AutoCondition.Two_Surf:
				case AutoCondition.Three_Surf:
				case AutoCondition.Four_Surf:
				case AutoCondition.Five_Surf:
					return true;
				default:
					return false;
			}
		}

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

				case AutoCondition.One_Surf:
					return surf.Length == 1;
				case AutoCondition.Two_Surf:
					return surf.Length == 2;
				case AutoCondition.Three_Surf:
					return surf.Length == 3;
				case AutoCondition.Four_Surf:
					return surf.Length == 4;
				case AutoCondition.Five_Surf:
					return surf.Length == 5;

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
