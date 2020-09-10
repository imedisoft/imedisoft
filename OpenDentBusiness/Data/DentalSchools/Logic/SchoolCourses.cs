using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class SchoolCourses
	{
		[CacheGroup(nameof(InvalidType.DentalSchools))]
		private class SchoolCourseCache : ListCache<SchoolCourse>
		{
			protected override IEnumerable<SchoolCourse> Load() 
				=> SelectMany("SELECT * FROM `school_courses` ORDER BY `course_id`");
        }

		private static readonly SchoolCourseCache cache = new SchoolCourseCache();

		public static List<SchoolCourse> GetAll() 
			=> cache.GetAll();

		public static SchoolCourse GetById(long schoolCourseId)
			=> cache.FirstOrDefault(schoolCourse => schoolCourse.Id == schoolCourseId);

		public static SchoolCourse FirstOrDefault(Predicate<SchoolCourse> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void Save(SchoolCourse schoolCourse)
		{
			if (schoolCourse.Id == 0) ExecuteInsert(schoolCourse);
			else
			{
				ExecuteUpdate(schoolCourse);
			}
		}

		public static void Delete(SchoolCourse schoolCourse)
		{
			var count = Database.ExecuteLong("SELECT COUNT(*) FROM `school_course_requirements` WHERE `school_course_id` = " + schoolCourse.Id);
			if (count > 0)
			{
				throw new Exception(Translation.DentalSchools.CourseInUse);
			}

			count = Database.ExecuteLong("SELECT COUNT(*) FROM `student_results` WHERE `school_course_id` = " + schoolCourse.Id);
			if (count > 0)
			{
				throw new Exception(Translation.DentalSchools.CourseInUse);
			}

			ExecuteDelete(schoolCourse.Id);

			cache.Remove(schoolCourse);
		}

		public static string GetDescription(long schoolCourseId) 
			=> GetDescription(FirstOrDefault(schoolCourse => schoolCourse.Id == schoolCourseId));

		public static string GetDescription(SchoolCourse schoolCourse) 
			=> schoolCourse?.ToString() ?? "";
	}
}
