using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using DataConnectionBase;
using Imedisoft.Data;
using MySql.Data.MySqlClient;

namespace OpenDentBusiness
{
	public partial class Ucums
	{
		public static IEnumerable<Ucum> GetAll() 
			=> SelectMany("SELECT * FROM `ucums` ORDER BY `code`");

		public static long GetCodeCount() 
			=> Database.ExecuteLong("SELECT COUNT(*) FROM `ucums`");

		/// <summary>
		/// Returns a list of just the codes for use in update or insert logic.
		/// </summary>
		public static IEnumerable<string> GetAllCodes() 
			=> Database.SelectMany("SELECT `code` FROM `ucums`", Database.ToScalar<string>);

		public static Ucum GetByCode(string code)
		{
			// because when we search for UnumCode 'a' for 'year [time]' used for age we sometimes get 'A' for 'Ampere [electric current]'
			// since MySQL is case insensitive, so we compare the binary values of 'a' and 'A' which are 0x61 and 0x41 in Hex respectively.
			return SelectOne("SELECT * FROM `ucums` WHERE CAST(`code` AS BINARY) = CAST('" + POut.String(code) + "' AS BINARY)");
		}

		public static IEnumerable<Ucum> GetBySearchText(string searchText)
		{
			var tokens = searchText.Split(' ');
			if (tokens.Length > 0)
            {
				var command = "SELECT * FROM `ucums` WHERE " +
					string.Join(" OR ",
						tokens.Select(t => "(`code` LIKE @code OR `description` LIKE @code)"));

				return SelectMany(command, 
					new MySqlParameter("code", '%' + searchText + '%'));
            }

			return SelectMany("SELECT * FROM `ucums`");
		}
	}
}
