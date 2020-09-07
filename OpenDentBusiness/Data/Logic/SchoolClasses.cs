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

		public static List<SchoolClass> GetDeepCopy() 
			=> cache.GetAll();

		public static SchoolClass GetFirstOrDefault(Predicate<SchoolClass> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void Update(SchoolClass schoolClass) 
			=> ExecuteUpdate(schoolClass);

		public static long Insert(SchoolClass schoolClass) 
			=> ExecuteInsert(schoolClass);

		public static void InsertOrUpdate(SchoolClass schoolClass)
		{
			if (schoolClass.Id == 0) Insert(schoolClass);
            else
            {
				Update(schoolClass);
            }
		}

		public static void Delete(long schoolClassId)
		{
			var count = Database.ExecuteLong("SELECT COUNT(*) FROM `providers` WHERE `school_class_id` = " + schoolClassId);
			if (count > 0)
			{
				throw new Exception("Class already in use by providers.");
			}

			count = Database.ExecuteLong("SELECT COUNT(*) FROM `school_course_requirements` WHERE `school_class_id` = " + schoolClassId);
			if (count > 0)
			{
				throw new Exception("Class already in use by 'course requirements' table.");
			}

			ExecuteDelete(schoolClassId);
		}

		public static string GetDescription(long SchoolClassNum)
		{
			var schoolClass = GetFirstOrDefault(x => x.Id == SchoolClassNum);
			if (schoolClass == null)
            {
				return "";
            }

			return GetDescription(schoolClass);
		}

		public static string GetDescription(SchoolClass schoolClass)
		{
			return schoolClass.Year + "-" + schoolClass.Description;
		}
	}
}
