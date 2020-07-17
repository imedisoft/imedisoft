using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenDentBusiness
{
    public class Reports
	{
		/// <summary>
		/// Gets a table of data using low permissions.
		/// </summary>
		public static DataTable GetTable(string command)
		{
			return Database.ExecuteDataTable(command);
		}
	}
}
