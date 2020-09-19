using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class AutoCodeConditions
	{
		[CacheGroup(nameof(InvalidType.AutoCodes))]
		private class AutoCodeCondCache : ListCache<AutoCodeCondition>
		{
			protected override IEnumerable<AutoCodeCondition> Load()
				=> SelectMany("SELECT * FROM `auto_code_conditions` ORDER BY `type`");
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

        public static void Set(long autoCodeItemId, IEnumerable<AutoCodeConditionType> conditions)
        {
            var conditionTypes = conditions.Select(x => (int)x).ToList();

            if (conditionTypes.Count == 0)
            {
                Database.ExecuteNonQuery(
                    "DELETE FROM `auto_code_conditions` " +
                    "WHERE `auto_code_item_id` = " + autoCodeItemId);
            }
            else
            {
                Database.ExecuteNonQuery(
                    "DELETE FROM `auto_code_conditions` " +
                    "WHERE `auto_code_item_id` = " + autoCodeItemId + " " +
                    "AND `type` NOT IN (" + string.Join(", ", conditionTypes) + ")");

                foreach (var condition in conditionTypes)
                {
                    Database.ExecuteNonQuery(
                        "INSERT IGNORE INTO `auto_code_conditions` (`auto_code_item_id`, `type`) " +
                        "VALUES (" + autoCodeItemId + ", " + condition + ")");
                }
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

        public static string GetDescription(AutoCodeConditionType conditionType)
        {
            return conditionType switch
            {
                AutoCodeConditionType.Anterior => "Anterior",
                AutoCodeConditionType.Posterior => "Posterior",
                AutoCodeConditionType.Premolar => "Premolar",
                AutoCodeConditionType.Molar => "Molar",
                AutoCodeConditionType.One_Surf => "1 Surface",
                AutoCodeConditionType.Two_Surf => "2 Surfaces",
                AutoCodeConditionType.Three_Surf => "3 Surfaces",
                AutoCodeConditionType.Four_Surf => "4 Surfaces",
                AutoCodeConditionType.Five_Surf => "5 Surfaces",
                AutoCodeConditionType.First => "First",
                AutoCodeConditionType.EachAdditional => "Each Additional",
                AutoCodeConditionType.Maxillary => "Maxillary",
                AutoCodeConditionType.Mandibular => "Mandibular",
                AutoCodeConditionType.Primary => "Primary",
                AutoCodeConditionType.Permanent => "Permanent",
                AutoCodeConditionType.Pontic => "Pontic",
                AutoCodeConditionType.Retainer => "Retainer",
                AutoCodeConditionType.AgeOver18 => "Age Over 18",
                _ => conditionType.ToString()
            };
        }
	}
}
