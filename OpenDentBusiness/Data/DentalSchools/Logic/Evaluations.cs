using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class Evaluations
	{
		public class Summary
		{
			public long EvaluationId;
			public string EvaluationTitle;
			public DateTime EvaluationDate;
			public long StudentId;
			public long InstructorId;
			public string FirstName;
			public string LastName;
			public string CourseID;
			public string GradingScale;
			public string Grade;
		}

		public class StudentInfo
		{
			public long ProviderId;
			public string FirstName;
			public string LastName;
		}

		public static Evaluation GetById(long evaluationId) 
			=> SelectOne(evaluationId);

		private static Summary SummaryFromReader(MySqlDataReader dataReader)
        {
			return new Summary
			{
				EvaluationId = (long)dataReader["id"],
				EvaluationTitle = (string)dataReader["title"],
				EvaluationDate = (DateTime)dataReader["evaluation_date"],
				StudentId = (long)dataReader["student_id"],
				InstructorId = (long)dataReader["instructor_id"],
				FirstName = (string)dataReader["first_name"],
				LastName = (string)dataReader["last_name"],
				CourseID = (string)dataReader["course_id"],
				GradingScale = (string)dataReader["grading_scale"],
				Grade = (string)dataReader["grade"]
			};
        }

		public static IEnumerable<Summary> GetSummaries(DateTime startDate, DateTime endDate, string lastName, string firstName, long? studentId, long? schoolCourseId, long? instructorId)
		{
			var criteria = new List<string>();
			var criteriaParams = new List<MySqlParameter>();

			if (!string.IsNullOrWhiteSpace(lastName))
			{
				criteria.Add("stu.`last_name` LIKE @last_name");
				criteriaParams.Add(new MySqlParameter("last_name", '%' + lastName + '%'));
			}

			if (!string.IsNullOrWhiteSpace(firstName))
			{
				criteria.Add("stu.`first_name` LIKE @first_name");
				criteriaParams.Add(new MySqlParameter("first_name", '%' + firstName + '%'));
			}

			if (studentId.HasValue)
			{
				criteria.Add("ev.`student_id` = " + studentId.Value);
			}

			if (schoolCourseId.HasValue)
			{
				criteria.Add("sc.`school_course_id` = " + schoolCourseId.Value);
			}

			if (instructorId != 0)
			{
				criteria.Add("ev.`instructor_id` = " + instructorId.Value);
			}

			if (endDate < startDate)
            {
				(startDate, endDate) = (endDate, startDate);
            }

			criteria.Add("(ev.`evaluation_date` BETWEEN @start_date AND @end_date)");
			criteriaParams.Add(new MySqlParameter("start_date", startDate));
			criteriaParams.Add(new MySqlParameter("end_date", endDate));

			var command =
				"SELECT " +
					"ev.`id`, ev.`title`, ev.`evaluation_date`, ev.`student_id`, ev.`instructor_id`, " +
					"stu.`first_name`, stu.`last_name`, " +
					"sc.`course_id`, " +
					"gs.`description` AS `grading_scale`, ev.`overall_grade_showing` AS `grade` " +
				"FROM `evaluations` ev " +
				"INNER JOIN `providers` ins ON ins.`id` = ev.`instrutor_id` " +
				"INNER JOIN `providers` stu ON stu.`id` = ev.`student_id` " +
				"INNER JOIN `school_courses` sc ON sc.`id` = ev.`school_course_id` " +
				"INNER JOIN `grading_scales` gs ON gs.`id` = ev.`grading_scale_id` " +
				"WHERE " + string.Join(" AND ", criteria) + " " +
				"ORDER BY `evaluation_date`, `last_name`";

			return Database.SelectMany(command, SummaryFromReader, criteriaParams.ToArray());
		}

		private static StudentInfo StudentInfoFromReader(MySqlDataReader dataReader)
        {
			return new StudentInfo
			{
				ProviderId = (long)dataReader["student_id"],
				FirstName = (string)dataReader["last_name"],
				LastName = (string)dataReader["first_name"]
			};
        }

		public static IEnumerable<StudentInfo> GetStudents(IEnumerable<long> schoolCourseIds, IEnumerable<long> instructorIds)
		{
			var criteria = new List<string>();

			if (schoolCourseIds != null)
			{
				var schoolCourseIdsList = schoolCourseIds.ToList();
				if (schoolCourseIdsList.Count > 0)
				{
					criteria.Add("school_courses.id IN (" + string.Join(", ", schoolCourseIdsList) + ")");
				}
			}

			if (instructorIds != null)
			{
				var instructorIdsList = instructorIds.ToList();
				if (instructorIdsList.Count != 0)
				{
					criteria.Add("ins.ProvNum IN (" + string.Join(", ", instructorIdsList) + ")");
				}
			}

			var command =
				"SELECT DISTINCT e.`student_id`, stu.`last_name`, stu.`first_name` " +
				"FROM `evaluations` e " +
				"INNER JOIN `providers` ins ON ins.`id` = e.`instructor_id` " +
				"INNER JOIN `providers` stu ON stu.`id` = e.`student_id` " +
				"INNER JOIN `school_courses` sc ON sc.`id` = e.`school_course_id`";

			if (criteria.Count > 0)
            {
				command += " WHERE " + string.Join(" AND ", criteria);
            }

			command += " ORDER BY `last_name`, `first_name`";

			return Database.SelectMany(command, StudentInfoFromReader);
		}

		public static void Save(Evaluation evaluation)
        {
			if (evaluation.Id == 0) ExecuteInsert(evaluation);
            else
            {
				ExecuteUpdate(evaluation);
            }
        }

		public static void Delete(Evaluation evaluation) 
			=> ExecuteDelete(evaluation);
	}
}
