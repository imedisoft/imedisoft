using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Imedisoft.Data
{
    public partial class EvaluationDefs
	{
		public class Summary
		{
			public long EvaluationDefId;
			public string EvaluationTitle;
			public string CourseID;
		}

		private static Summary SummaryFromReader(MySqlDataReader dataReader)
        {
			return new Summary
			{
				EvaluationDefId = (long)dataReader["id"],
				EvaluationTitle = (string)dataReader["title"],
				CourseID = (string)dataReader["course_id"]
			};
        }

		public static IEnumerable<Summary> GetSummaryForCourse(long? schoolCourseId)
		{
			var command = 
				"SELECT ed.`id`, ed.`title`, sc.`course_id` " +
				"FROM `evaluation_defs` ed " +
				"INNER JOIN `school_courses` sc ON sc.`id` = ed.`school_course_id`";

			if (schoolCourseId.HasValue)
			{
				command += " WHERE sc.`id` = " + schoolCourseId.Value;
			}

			command += " ORDER BY `course_id`, ed.`title`";

			return Database.SelectMany(command, SummaryFromReader);
		}

		public static EvaluationDef GetById(long evaluationDefId) 
			=> SelectOne(evaluationDefId);

		public static void Save(EvaluationDef evaluationDef)
        {
			if (evaluationDef.Id == 0) ExecuteInsert(evaluationDef);
            else
            {
				ExecuteUpdate(evaluationDef);
            }
        }

		public static void Delete(EvaluationDef evaluationDef) 
			=> ExecuteDelete(evaluationDef);
    }
}
