using CodeBase;
using DataConnectionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness.WebServices
{
    public class OpenDentalServerMockIIS : IOpenDentalServer
	{
		/// <summary>
		/// Optionally pass in database connection settings to override using the parent thread database context.
		/// </summary>
		public OpenDentalServerMockIIS(
			string server = "", string db = "", string user = "", string password = "", string userLow = "", string passLow = "", DatabaseType dbType = DatabaseType.MySql)
		{
		}

		public string ProcessRequest(string dtoString) => "";
	}
}
