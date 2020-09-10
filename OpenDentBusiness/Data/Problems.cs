using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class Problems
	{
		/// <summary>
		/// This returns a single disease, but a patient may have multiple instances of the same disease. 
		/// For example, they may have multiple pregnancy instances with the same DiseaseDefNum. 
		/// This will return a single instance of the disease, chosen at random by MySQL. 
		/// Would be better to use GetDiseasesForPatient below which returns a list of diseases with this DiseaseDefNum for the patient.
		/// </summary>
		public static Problem GetSpecificDiseaseForPatient(long patientId, long problemDefinitionId) 
			=> SelectOne("SELECT * FROM `problems` WHERE `patient_id` = " + patientId + " AND `problem_def_id` = " + problemDefinitionId);

		/// <summary>
		/// Gets a list of all problems for a given patient.
		/// </summary>
		public static IEnumerable<Problem> GetByPatient(long patientId, bool showActiveOnly = false)
		{
			var command = "SELECT * FROM `problems` WHERE `patient_id` = " + patientId;

			if (showActiveOnly)
			{
				command += " AND `status` = " + (int)ProblemStatus.Active;
			}

			return SelectMany(command);
		}

		/// <summary>
		/// Gets a list of all Diseases for a given patient. Show innactive returns all, otherwise only resolved and active problems.
		/// </summary>
		public static IEnumerable<Problem> Refresh(bool showInactive, long patientId)
		{
			var command = "SELECT * FROM `problems` WHERE `patient_id` = " + patientId;

			if (!showInactive)
			{
				command += " AND (`status` = " + (int)ProblemStatus.Active + " OR `status` = " + (int)ProblemStatus.Resolved + ")";
			}

			return SelectMany(command);
		}

		/// <summary>
		/// Gets a list of every disease for the patient that has the specified DiseaseDefNum. 
		/// Set showActiveOnly true to only show active Diseases based on status (i.e. it could have a stop date but still be active, or marked inactive with no stop date).
		/// </summary>
		public static IEnumerable<Problem> GetByPatient(long patientId, long problemDefinitionId, bool showActiveOnly = false)
		{
			var command = "SELECT * FROM `problems` WHERE patient_id = " + patientId + " AND `problem_def_id` = " + problemDefinitionId;

			if (showActiveOnly)
			{
				command += " AND `status` =" + (int)ProblemStatus.Active;
			}

			return SelectMany(command);
		}

		/// <summary>
		/// Returns a list of PatNums that have a disease from the PatNums that are passed in.
		/// </summary>
		public static IEnumerable<long> GetPatientsWithDisease(List<long> patientIds)
		{
			if (patientIds.Count == 0)
			{
				return new List<long>();
			}

			return Database.SelectMany(
				"SELECT DISTINCT `patient_id` FROM `problems` " +
				"WHERE `patient_id` IN (" + string.Join(", ", patientIds) + ") " +
				"AND `problems`.`problem_def_id` != " + Prefs.GetLong(PrefName.ProblemsIndicateNone),
					Database.ToScalar<long>);
		}

		public static Problem GetOne(long problemId) 
			=> SelectOne(problemId);

		public static void Update(Problem disease) 
			=> ExecuteUpdate(disease);

		public static long Insert(Problem disease) 
			=> ExecuteInsert(disease);

		public static void Delete(Problem disease) 
			=> ExecuteDelete(disease);

		public static List<long> GetChangedSinceDiseaseNums(DateTime changedSince, List<long> patientIdsEligibleForUpload)
		{
			var problemIds = new List<long>();

			if (patientIdsEligibleForUpload.Count > 0)
			{
				problemIds.AddRange(Database.SelectMany(
					"SELECT `id` FROM `problems` " +
					"WHERE `last_modified_date` > @date " +
					"AND `patient_id` IN (" + string.Join(", ", patientIdsEligibleForUpload) + ")",
						Database.ToScalar<long>,
						new MySqlParameter("date", changedSince)));
			}

			return problemIds;
		}

		public static List<Problem> GetMultDiseases(List<long> problemIds)
		{
			var problems = new List<Problem>();

			if (problemIds.Count > 0)
            {
				problems.AddRange(
					SelectMany(
						"SELECT * FROM `problems` WHERE `id` IN (" + string.Join(", ", problemIds) + ")"));
            }

			return problems;
		}

		public static void ResetTimeStamps(long patientId) 
			=> Database.ExecuteNonQuery(
				"UPDATE `problems` SET `last_modified_date` = CURRENT_TIMESTAMP " +
				"WHERE `patient_id` =" + patientId);

		public static void ResetTimeStamps(long patientId, ProblemStatus status) 
			=> Database.ExecuteNonQuery(
				"UPDATE `problems` SET `last_modified_date` = CURRENT_TIMESTAMP " +
				"WHERE `patient_id` =" + patientId + " AND `status` = " + (int)status);
	}
}
