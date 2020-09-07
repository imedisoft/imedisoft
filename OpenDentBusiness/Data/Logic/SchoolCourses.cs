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

		public static List<SchoolCourse> GetDeepCopy() 
			=> cache.GetAll();

		public static SchoolCourse GetFirstOrDefault(Predicate<SchoolCourse> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void Update(SchoolCourse schoolCourse) 
			=> ExecuteUpdate(schoolCourse);

		public static long Insert(SchoolCourse schoolCourse) 
			=> ExecuteInsert(schoolCourse);

		public static void InsertOrUpdate(SchoolCourse schoolCourse)
		{
			if (schoolCourse.Id == 0) Insert(schoolCourse);
			else
			{
				Update(schoolCourse);
			}
		}

		public static void Delete(long schoolCourseId)
		{
			var count = Database.ExecuteLong("SELECT COUNT(*) FROM `school_course_requirements` WHERE `school_course_id` = " + schoolCourseId);
			if (count > 0)
			{
				throw new Exception("Course already in use by 'course requirements' table.");
			}

			count = Database.ExecuteLong("SELECT COUNT(*) FROM reqstudent WHERE SchoolCourseNum = " + schoolCourseId);
			if (count > 0)
			{
				throw new Exception("Course already in use by 'student requirements' table.");
			}

			ExecuteDelete(schoolCourseId);
		}

		public static string GetDescript(long schoolCourseId)
		{
			var schoolCourse = GetFirstOrDefault(x => x.Id == schoolCourseId);
			if (schoolCourse == null)
            {
				return "";
            }

			return GetDescription(schoolCourse);
		}

		public static string GetDescription(SchoolCourse schoolCourse)
		{
			return schoolCourse.CourseID + " " + schoolCourse.Descript;
		}

		public static string GetCourseID(long schoolCourseId)
		{
			var schoolCourse = GetFirstOrDefault(x => x.Id == schoolCourseId);
			if (schoolCourse == null)
            {
				return "";
            }

			return schoolCourse.CourseID;
		}
	}
}
