using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class Icd10s
	{
		public static IEnumerable<Icd10> GetAll() 
			=> SelectMany("SELECT * FROM `icd10`");

		public static IEnumerable<Icd10> GetByCodes(List<string> codes)
		{
			if (codes == null || codes.Count == 0)
			{
				return new List<Icd10>();
			}

			return SelectMany(
				"SELECT * FROM `icd10` " +
				"WHERE `code` IN (" + string.Join(", ", codes.Select(code => MySqlHelper.EscapeString(code))) + ")");
		}

		public static Icd10 GetByCode(string code)
			=> SelectOne(code);

		public static IEnumerable<Icd10> GetByCodeOrDescription(string searchText)
		{
			if (string.IsNullOrEmpty(searchText)) return GetAll();

			var criteria = searchText.Split(' ')
				.Select(
					term => MySqlHelper.EscapeString(term))
				.Select(
					term => "(`code` LIKE '%" + term + "%' OR `description` LIKE '%" + term + "%')");

			return SelectMany("SELECT * FROM `icd10` WHERE " + string.Join(" AND ", criteria));
		}

		public static long GetCodeCount() 
			=> Database.ExecuteLong("SELECT COUNT(*) FROM `icd10` WHERE `is_code` != 0");

		public static string GetCodeAndDescription(string code)
		{
			if (string.IsNullOrEmpty(code))
			{
				return "";
			}

			var icd10 = GetByCode(code);
			if (icd10 == null)
			{
				return "";
			}

			return icd10.Code + '-' + icd10.Description;
		}

		public static bool CodeExists(string code)
		{
			if (string.IsNullOrEmpty(code)) return false;

			var count = Database.ExecuteLong(
				"SELECT COUNT(*) FROM `icd10` WHERE `code` = @code",
					new MySqlParameter("code", code));

			return count > 0;
		}

		public static void Insert(Icd10 icd10) 
			=> ExecuteInsert(icd10);

		public static void Update(Icd10 icd10) 
			=> ExecuteUpdate(icd10);
	}
}
