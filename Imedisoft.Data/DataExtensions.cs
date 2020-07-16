using DataConnectionBase;
using System;
using System.Data;

namespace Imedisoft.Data
{
    public static class DataExtensions
	{
		/// <summary>
		/// Simpler way to get a long from a DataRow.
		/// </summary>
		public static long GetLong(this DataRow row, string columnName) 
			=> SIn.Long(row[columnName].ToString());

		/// <summary>
		/// Simpler way to get a string from a DataRow.
		/// </summary>
		public static string GetString(this DataRow row, string columnName) 
			=> SIn.String(row[columnName].ToString());

		/// <summary>
		/// Simpler way to get a Date (without time) from a DataRow.
		/// </summary>
		public static DateTime GetDate(this DataRow row, string columnName) 
			=> SIn.Date(row[columnName].ToString());
	}
}
