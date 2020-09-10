using Imedisoft.Data.Models;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class SchoolCourseRequirements
	{
		public static IEnumerable<SchoolCourseRequirement> GetByCourseAndClass(long schoolClassId, long schoolCourseId)
			=> SelectMany(
				"SELECT * FROM `school_course_requirements` " +
				"WHERE `school_class_id` = " + schoolClassId + " AND `school_course_id` = " + schoolCourseId + " " +
				"ORDER BY `description`");

		public static IEnumerable<SchoolCourseRequirement> GetAll()
			=> SelectMany("SELECT * FROM `school_course_requirements` ORDER BY `description`");

		public static SchoolCourseRequirement GetById(long schoolCourseRequirementId) 
			=> SelectOne(schoolCourseRequirementId);

		public static void Save(SchoolCourseRequirement schoolCourseRequirement)
		{
			if (schoolCourseRequirement.Id == 0) ExecuteInsert(schoolCourseRequirement);
			else
			{
				ExecuteUpdate(schoolCourseRequirement);
			}
		}

		public static void Delete(long schoolCourseRequirementId) 
			=> ExecuteDelete(schoolCourseRequirementId);
	}
}
