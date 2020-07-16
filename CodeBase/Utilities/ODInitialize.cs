using System;
using System.Net;
using System.Reflection;

namespace CodeBase
{
    public class ODInitialize
	{
		/// <summary>
		/// Indicates that the program is running unit tests.
		/// Should only be set to true from TestBase.Initialize().
		/// Useful for methods that should behave differently in unit tests, such as 
		/// FriendlyException.Show().
		/// </summary>
		public static bool IsRunningInUnitTest;

		/// <summary>
		/// Indicates that Initialize has been invoked at least once and has successfully executed.
		/// </summary>
		public static bool HasInitialized;

		/// <summary>
		/// This method is called from all Open Dental programs or projects.
		/// This method can throw. There is a good chance you should not let the user continue if 
		/// the method throws as it can cause the program to behave in unexpecting ways.
		/// </summary>
		public static void Initialize()
		{
			// The default SecurityProtocol is "Ssl3|Tls".  We must add Tls12 in order to support Tls1.2 web reference handshakes, 
			// without breaking any web references using Ssl3 or Tls. This is necessary for XWeb payments.
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

			HasInitialized = true;
		}
	}
}
