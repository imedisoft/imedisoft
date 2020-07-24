using System;

namespace OpenDentBusiness
{
    public class ReportsComplex : Reports
	{
		/// <summary>
		/// Wrapper method to call the passed-in func in a seperate thread connected to the reporting server.
		/// This method should only be used for SELECT, with the exception DashboardAR. Using this for create/update/delete may cause duplicates.
		/// The return type of this function is whatever the return type of the method you passed in is.
		/// Throws an exception if anything went wrong executing func within the thread.
		/// </summary>
		/// <param name="doRunOnReportServer">If this false, the func will run against the currently connected server.</param>
		public static T RunFuncOnReportServer<T>(Func<T> func, bool doRunOnReportServer = true) => func();
	}
}
