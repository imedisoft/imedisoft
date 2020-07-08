using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness
{
	public static class DataExtensions
	{
		/// <summary>Simpler way to get a long from a DataRow.</summary>
		public static long GetLong(this DataRow row, string columnName)
		{
			return DataConnectionBase.SIn.Long(row[columnName].ToString());
		}

		/// <summary>Simpler way to get a string from a DataRow.</summary>
		public static string GetString(this DataRow row, string columnName)
		{
			return DataConnectionBase.SIn.String(row[columnName].ToString());
		}

		/// <summary>Simpler way to get a Date (without time) from a DataRow.</summary>
		public static DateTime GetDate(this DataRow row, string columnName)
		{
			return DataConnectionBase.SIn.Date(row[columnName].ToString());
		}
	}
}
