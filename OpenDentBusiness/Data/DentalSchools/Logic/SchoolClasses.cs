using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class SchoolClasses
	{
		[CacheGroup(nameof(InvalidType.DentalSchools))]
        private class SchoolClassCache : ListCache<SchoolClass>
        {
			protected override IEnumerable<SchoolClass> Load() 
				=> SelectMany("SELECT * FROM `school_classes` ORDER BY `year`, `description`");
        }

        private static readonly SchoolClassCache cache = new SchoolClassCache();

		public static List<SchoolClass> GetAll() 
			=> cache.GetAll();

		public static SchoolClass FirstOrDefault(Predicate<SchoolClass> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void Save(SchoolClass schoolClass)
		{
			if (schoolClass.Id == 0) ExecuteInsert(schoolClass);
            else
            {
				ExecuteUpdate(schoolClass);
            }
		}

		public static void Delete(SchoolClass schoolClass)
		{
			var count = Database.ExecuteLong("SELECT COUNT(*) FROM `providers` WHERE `school_class_id` = " + schoolClass.Id);
			if (count > 0)
			{
				throw new Exception(Translation.DentalSchools.ClassInUse);
			}

			count = Database.ExecuteLong("SELECT COUNT(*) FROM `school_course_requirements` WHERE `school_class_id` = " + schoolClass.Id);
			if (count > 0)
			{
				throw new Exception(Translation.DentalSchools.ClassInUse);
			}

			ExecuteDelete(schoolClass);

			cache.Remove(schoolClass);
		}

		public static string GetDescription(long schoolClassId) 
			=> GetDescription(FirstOrDefault(schoolClass => schoolClass.Id == schoolClassId));

		public static string GetDescription(SchoolClass schoolClass) 
			=> schoolClass?.ToString() ?? "";
	}
}
