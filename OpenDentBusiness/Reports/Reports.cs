using Imedisoft.Data;
using System.Data;

namespace OpenDentBusiness
{
    public class Reports
	{
		/// <summary>
		/// Gets a table of data from the database.
		/// </summary>
		public static DataTable GetTable(string command) 
			=> Database.ExecuteDataTable(command);
	}
}
