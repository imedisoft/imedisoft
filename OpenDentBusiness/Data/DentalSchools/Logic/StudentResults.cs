using Imedisoft.Data;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class StudentResults
	{
		public class StudentSummary
		{
			public long StudentProviderId;
			public long ReqTotal;
			public long ReqCompleted;
			public string FirstName;
			public string LastName;
		}

		public class StudentResultSummary
		{
			public long StudentResultId;
			public string Requirement;
			public string Appointment;
			public string Course;
			public string Patient;
			public bool Completed;
		}

		public static IEnumerable<StudentResult> GetByAppt(long apptId) 
			=> SelectMany(
				"SELECT * FROM `student_results` " +
				"WHERE `appt_id` = " + apptId + " " +
				"ORDER BY `provider_id`, `description`");

		public static StudentResult GetById(long studentResultId) 
			=> SelectOne(studentResultId);

		public static List<Provider> GetStudents(long schoolClassId) // TODO: This should be part of the SchoolClasses interface...
			=> Providers.GetWhere(x => x.SchoolClassId == schoolClassId, true);

		public static void Save(StudentResult studentResult)
        {
			if (studentResult.Id == 0) ExecuteInsert(studentResult);
            else
            {
				ExecuteUpdate(studentResult);
			}
        }

		public static void Delete(long studentResultId)
		{
			var studentResult = GetById(studentResultId);
			if (studentResult == null)
            {
				return;
            }

			if (SchoolCourseRequirements.GetById(studentResult.SchoolCourseRequirementId) == null)
			{
				throw new Exception("Cannot delete result. Delete the requirement instead.");
			}

			ExecuteDelete(studentResultId);
		}

		private static StudentSummary StudentSummaryFromReader(MySqlDataReader dataReader)
		{
			return new StudentSummary
			{
				StudentProviderId = (long)dataReader["provider_id"],
				ReqTotal = (long)dataReader["req_total"],
				ReqCompleted = (long)dataReader["req_completed"],
				FirstName = (string)dataReader["first_name"],
				LastName = (string)dataReader["last_name"]
			};
		}

		private static StudentResultSummary StudentResultSummaryFromReader(MySqlDataReader dataReader)
        {
			var apptDescription = "";
			var apptDate = dataReader["appt_date"] as DateTime?;
			if (apptDate.HasValue)
            {
				apptDescription =
					apptDate.Value.ToShortDateString() + " " +
					apptDate.Value.ToShortTimeString() + " " +
					dataReader["appt_description"];
            }

			return new StudentResultSummary
			{
				StudentResultId = (long)dataReader["student_result_id"],
				Requirement = (string)dataReader["requirement"],
				Course = (string)dataReader["course"],
				Patient = (string)dataReader["patient"],
				Appointment = apptDescription.Trim(),
				Completed = Convert.ToInt32(dataReader["completed"]) == 1
			};
        }

		public static IEnumerable<StudentResultSummary> GetSummaryForStudentResults(long studentProviderId)
			=> Database.SelectMany("CALL `get_student_result_summary`(" + studentProviderId + ")",
				StudentResultSummaryFromReader);

		public static IEnumerable<StudentSummary> GetSummaryForStudents(long schoolClassId, long schoolCourseId) 
			=> Database.SelectMany("CALL `get_students_summary`(" + schoolClassId + ", " + schoolCourseId + ")", 
				StudentSummaryFromReader);



		///<summary>All fields for all reqs will have already been set.  All except for reqstudent.ReqStudentNum if new.  Now, they just have to be persisted to the database.</summary>
		public static void SynchApt(List<StudentResult> listReqsAttached, List<StudentResult> listReqsRemoved, long aptNum)
		{
			string command;
			//first, delete all that were removed from this appt
			if (listReqsRemoved.Count(x => x.Id != 0) > 0)
			{
				command = "DELETE FROM reqstudent WHERE ReqStudentNum IN(" + string.Join(",", listReqsRemoved.Where(x => x.Id != 0)
					.Select(x => x.Id)) + ")";
				Database.ExecuteNonQuery(command);
			}
			//second, detach all from this appt
			command = "UPDATE reqstudent SET AptNum=0 WHERE AptNum=" + aptNum;
			Database.ExecuteNonQuery(command);
			if (listReqsAttached.Count == 0)
			{
				return;
			}
			for (int i = 0; i < listReqsAttached.Count; i++)
			{
				if (listReqsAttached[i].Id == 0)
				{
					ExecuteInsert(listReqsAttached[i]);
				}
				else
				{
					ExecuteUpdate(listReqsAttached[i]);
				}
			}
		}

		public static bool IsInUseBy(long schoolCourseRequirementId, out string studentNames)
		{
			string command =
				"SELECT CONCAT(`last_name`, ', ', `first_name`) FROM `providers` p, `student_requirements` sr " +
				"WHERE p.`id` = sr.`provider_id` " +
				"AND sr.`school_course_requirement_id` = " + schoolCourseRequirementId + " " +
				"AND sr.`completion_date` IS NOT NULL";

			studentNames = "";

			var results = "";
			Database.ExecuteReader(command, dataReader =>
			{
				while (dataReader.Read())
				{
					results += (string)dataReader[0] + "\r\n";
				}
			});

			if (string.IsNullOrEmpty(results))
            {
				return false;
            }

			studentNames = results;
			return true;
		}
	}
}
