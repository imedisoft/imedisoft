using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class Icd9s
	{
		[CacheGroup(nameof(InvalidType.Diseases))]
		private class Icd9Cache : ListCache<Icd9>
		{
			protected override IEnumerable<Icd9> Load()
				=> SelectMany("SELECT * FROM `icd9`");
        }

		private static readonly Icd9Cache cache = new Icd9Cache();

		public static Icd9 FirstOrDefault(Predicate<Icd9> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static IEnumerable<Icd9> GetAll() 
			=> SelectMany("SELECT * FROM `icd9`");

		public static Icd9 GetByCode(string code) 
			=> FirstOrDefault(x => x.Code == code);

		public static IEnumerable<Icd9> GetByCodeOrDescription(string searchText)
			=> SelectMany(
				"SELECT * FROM `icd9` WHERE `code` LIKE @criteria OR `description` LIKE @criteria",
					new MySqlParameter("criteria", '%' + searchText + '%'));

		public static long GetCodeCount() 
			=> Database.ExecuteLong("SELECT COUNT(*) FROM `icd9`");

		public static string GetCodeAndDescription(string code)
		{
			if (string.IsNullOrEmpty(code))
			{
				return "";
			}

			var icd9 = FirstOrDefault(x => x.Code == code);
			if (icd9 == null)
			{
				return "";
			}

			return icd9.Code + '-' + icd9.Description;
		}

		public static bool CodeExists(string code)
		{
			if (string.IsNullOrEmpty(code)) return false;

			var count = Database.ExecuteLong(
				"SELECT COUNT(*) FROM `icd9` WHERE `code` = @code", 
					new MySqlParameter("code", code));

			return count > 0;
		}

		public static void Insert(Icd9 icd9) 
			=> ExecuteInsert(icd9);

		public static void Update(Icd9 icd9) 
			=> ExecuteUpdate(icd9);

		/// <summary>
		/// Deletes the specified ICD-9 code from the database.
		/// </summary>
		/// <param name="code">The ICD-9 code.</param>
		/// <exception cref="Exception">If the ICD-9 code is in use.</exception>
		public static void Delete(string code)
		{
			if (string.IsNullOrEmpty(code)) return;

			string command =
				"SELECT `make_patient_id`(pat.`PatNum`, FName, LName) " +
				"FROM `patient` pat, `problems` p, `problem_definitions` pd " +
				"WHERE pat.`PatNum` = p.`patient_id` " +
				"AND p.`problem_def_id` = pd.`id` " +
				"AND pd.`code_icd9` = @code " +
				"GROUP BY pat.`PatNum`";

			var patients = Database.SelectMany(command, Database.ToScalar<string>, new MySqlParameter("code", code)).ToList();
			if (patients.Count > 0)
			{
				var message = "Not allowed to delete. Already in use by " + patients.Count.ToString() + " patients, including";

				for (int i = 0; i < patients.Count; i++)
				{
					if (i > 5)
					{
						break;
					}

					message += "\r\n" + patients[i];
				}

				throw new Exception(message);
			}

			ExecuteDelete(code);
		}

		/// <summary>
		/// Returns true if any of the procedures has a ICD-9 code.
		/// </summary>
		public static bool HasIcd9Codes(List<Procedure> procedures)
		{
			var codes = new List<string>();

			var icd9Procedures = procedures.FindAll(x => x.IcdVersion == 9);

			codes.AddRange(icd9Procedures.Where(x => !string.IsNullOrEmpty(x.DiagnosticCode)).Select(x => x.DiagnosticCode));
			codes.AddRange(icd9Procedures.Where(x => !string.IsNullOrEmpty(x.DiagnosticCode2)).Select(x => x.DiagnosticCode2));
			codes.AddRange(icd9Procedures.Where(x => !string.IsNullOrEmpty(x.DiagnosticCode3)).Select(x => x.DiagnosticCode3));
			codes.AddRange(icd9Procedures.Where(x => !string.IsNullOrEmpty(x.DiagnosticCode4)).Select(x => x.DiagnosticCode4));

			return codes.Count != 0;
		}
	}
}
