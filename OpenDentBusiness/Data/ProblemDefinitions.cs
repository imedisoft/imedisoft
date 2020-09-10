using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class ProblemDefinitions
	{
		[CacheGroup(nameof(InvalidType.Diseases))]
		private class ProblemDefinitionCache : ListCache<ProblemDefinition>
		{
			protected override IEnumerable<ProblemDefinition> Load()
				=> SelectMany("SELECT * FROM `problem_defs`");
        }

		private static readonly ProblemDefinitionCache cache = new ProblemDefinitionCache();

		public static List<ProblemDefinition> GetWhere(Predicate<ProblemDefinition> predicate) 
			=> cache.Find(predicate);

		public static List<ProblemDefinition> GetAll(bool includeHidden = true)
			=> includeHidden ? cache.GetAll() : 
				cache.Find(problemDefinition => !problemDefinition.IsHidden);

		public static ProblemDefinition GetItem(long problemDefinitionId)
			=> cache.FirstOrDefault(x => x.Id == problemDefinitionId);

		public static ProblemDefinition GetByDescription(string description) 
			=> cache.FirstOrDefault(x => x.Description == description && !x.IsHidden);

		public static ProblemDefinition FirstOrDefault(Predicate<ProblemDefinition> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void Update(ProblemDefinition def) 
			=> ExecuteUpdate(def);

		public static long Insert(ProblemDefinition def) 
			=> ExecuteInsert(def);

		public static void Save(ProblemDefinition diseaseDef)
        {
			if (diseaseDef.Id == 0)
            {
				ExecuteInsert(diseaseDef);

				SecurityLogs.Write(Permissions.ProblemEdit, diseaseDef.Description + " added.");
			}
            else
            {
				ExecuteUpdate(diseaseDef);

				SecurityLogs.Write(Permissions.ProblemEdit, diseaseDef.Description + " updated.");
			}
        }
		
		public static void Delete(ProblemDefinition diseaseDef)
        {
			EnsureNotInUse(diseaseDef.Id);

			ExecuteDelete(diseaseDef);

			SecurityLogs.Write(Permissions.ProblemEdit, diseaseDef.Description + " deleted.");

			diseaseDef.Id = 0;
		}

		private static void EnsureNotInUse(long diseaseDefId)
        {
			if (IsInUse(diseaseDefId))
            {
				throw new Exception("This problem definition is currently in use and cannot be deleted.");
			}
        }

		private static bool IsInUse(long problemDefinitionId)
        {
			if (Preferences.GetLong(PreferenceName.ProblemsIndicateNone) == problemDefinitionId)
				return true;

			var commands = new string[]
			{
				"SELECT COUNT(*) FROM `problems` WHERE `problem_def_id` = " + problemDefinitionId,
				"SELECT COUNT(*) FROM eduresource WHERE DiseaseDefNum = " + problemDefinitionId,
				"SELECT COUNT(*) FROM familyhealth WHERE DiseaseDefNum = " + problemDefinitionId
			};

			foreach (var command in commands)
			{
				var count = Database.ExecuteLong(command);
				if (count > 0)
				{
					return true;
				}
			}

			return false;
        }

		/// <summary>
		/// Returns the name of the disease, whether hidden or not.
		/// </summary>
		public static string GetName(long problemDefintionId) 
			=> cache.FirstOrDefault(x => x.Id == problemDefintionId)?.Description ?? "";

		/// <summary>
		/// Returns the name of the disease based on SNOMEDCode, then if no match tries ICD9Code, then if no match returns empty string. Used in EHR Patient Lists.
		/// </summary>
		/// <param name="code">A SNOMED or ICD-9 code.</param>
		public static string GetNameByCode(string code)
		{
            var problemDefinition = 
				cache.FirstOrDefault(x => x.CodeSnomed == code) ?? 
				cache.FirstOrDefault(x => x.CodeIcd9 == code);

			return problemDefinition?.Description ?? "";
		}

		/// <summary>
		/// Returns the DiseaseDefNum based on SNOMEDCode, then if no match tries ICD9Code, then if no match tries ICD10Code, then if no match returns 0. 
		/// Used in EHR Patient Lists and when automatically inserting pregnancy Dx from FormVitalsignEdit2014. 
		/// Will match hidden diseases.
		/// </summary>
		public static long? GetNumFromCode(string code)
		{
			var problemDefinition =
				cache.FirstOrDefault(x => x.CodeSnomed == code) ??
				cache.FirstOrDefault(x => x.CodeIcd9 == code) ??
				cache.FirstOrDefault(x => x.CodeIcd10 == code);

			return problemDefinition?.Id;
		}

		/// <summary>
		/// Returns the DiseaseDefNum based on SNOMEDCode. 
		/// If no match or if SnomedCode is an empty string returns null. 
		/// Only matches SNOMEDCode, not ICD9 or ICD10.
		/// </summary>
		public static long? GetNumFromSnomed(string code)
		{
			if (string.IsNullOrEmpty(code)) return null;

            var problemDefinition = cache.FirstOrDefault(x => x.CodeSnomed == code);

			return problemDefinition?.Id;
		}

		/// <summary>
		/// Returns the diseaseDefNum that exactly matches the specified string. 
		/// Used in import functions when you only have the name to work with. 
		/// Can return 0 if no match. 
		/// Does not match hidden diseases.
		/// </summary>
		public static long? GetNumFromName(string description) 
			=> GetNumFromName(description, false);

		/// <summary>
		/// Returns the diseaseDefNum that exactly matches the specified string. Will return 0 if no match.
		/// Set matchHidden to true to match hidden diseasedefs as well.
		/// </summary>
		public static long? GetNumFromName(string description, bool matchHidden)
		{
            var problemDefinition = cache.FirstOrDefault(x => x.Description == description);
			if (!matchHidden && problemDefinition.IsHidden)
            {
				return null;
            }

			return problemDefinition?.Id;
		}

		public static List<long> GetChangedSinceDiseaseDefNums(DateTime changedSince) 
			=> Database.SelectMany(
				"SELECT * FROM `problem_defs` WHERE `last_modified_date` > @date", 
				Database.ToScalar<long>,
					new MySqlParameter("date", changedSince)).ToList();

		/// <summary>
		/// Used along with GetChangedSinceDiseaseDefNums
		/// </summary>
		public static List<ProblemDefinition> GetMultDiseaseDefs(List<long> problemDefinitionIds)
		{
			var problemDefinitions = new List<ProblemDefinition>();

			if (problemDefinitionIds.Count > 0)
            {
				problemDefinitions.AddRange(
					SelectMany(
						"SELECT * FROM `problem_defs` " +
						"WHERE `id` IN (" + string.Join(", ", problemDefinitionIds) + ")"));
            }

			return problemDefinitions;
		}

		public static bool ContainsSnomed(string snomedCode, long excludeProblemDefinitionId) 
			=> cache.Any(x => x.CodeSnomed == snomedCode && x.Id != excludeProblemDefinitionId);

		public static bool ContainsIcd9(string icd9Code, long excludeProblemDefinitionId) 
			=> cache.Any(x => x.CodeIcd9 == icd9Code && x.Id != excludeProblemDefinitionId);

		public static bool ContainsIcd10(string icd10Code, long excludeProblemDefinitionId) 
			=> cache.Any(x => x.CodeIcd10 == icd10Code && x.Id != excludeProblemDefinitionId);

		/// <summary>
		/// Get all problem definitions that have a pregnancy code that applies to the three CQM 
		/// measures with pregnancy as an exclusion condition.
		/// </summary>
		public static List<ProblemDefinition> GetAllPregnancyProblemDefinitions()
		{
			var pregnancyCodesForCQMs = 
				EhrCodes.GetCodesExistingInAllSets(
					new List<string> { 
						"2.16.840.1.113883.3.600.1.1623",
						"2.16.840.1.113883.3.526.3.378" 
					});
			
			var results = new List<ProblemDefinition>();

			foreach (var problemDefinition in GetAll())
			{
				if (pregnancyCodesForCQMs.ContainsKey(problemDefinition.CodeIcd9) && 
					pregnancyCodesForCQMs[problemDefinition.CodeIcd9] == "ICD9CM")
				{
					results.Add(problemDefinition);
				}
				else if (pregnancyCodesForCQMs.ContainsKey(problemDefinition.CodeIcd10) && 
					pregnancyCodesForCQMs[problemDefinition.CodeIcd10] == "ICD10CM")
				{
					results.Add(problemDefinition);
				}
				else if (pregnancyCodesForCQMs.ContainsKey(problemDefinition.CodeSnomed) && 
					pregnancyCodesForCQMs[problemDefinition.CodeSnomed] == "SNOMEDCT")
				{
					results.Add(problemDefinition);
				}
			}

			return results;
		}
	}
}
