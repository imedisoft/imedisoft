using CodeBase;
using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;

namespace OpenDentBusiness
{
    public class ReportsComplex
	{
		/// <summary>
		/// Gets a table of data using normal permissions.
		/// </summary>
		public static DataTable GetTable(string command)
		{
			return Database.ExecuteDataTable(command);
		}

		/// <summary>
		/// Wrapper method to call the passed-in func in a seperate thread connected to the reporting server.
		/// This method should only be used for SELECT, with the exception DashboardAR. Using this for create/update/delete may cause duplicates.
		/// The return type of this function is whatever the return type of the method you passed in is.
		/// Throws an exception if anything went wrong executing func within the thread.
		/// </summary>
		/// <param name="doRunOnReportServer">If this false, the func will run against the currently connected server.</param>
		public static T RunFuncOnReportServer<T>(Func<T> func, bool doRunOnReportServer = true)
		{
			return func();
		}
	}
}
