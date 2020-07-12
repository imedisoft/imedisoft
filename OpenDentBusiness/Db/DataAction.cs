using CodeBase;
using DataConnectionBase;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenDentBusiness
{
    /// <summary>
	/// Perform actions in different database contexts.
	/// </summary>
    public class DataAction
	{
		/// <summary>
		/// Filled via FillDictHqCentralConnections().
		/// </summary>
		private static readonly ConcurrentDictionary<ConnectionNames, CentralConnection> _dictHqCentralConnections =
			new ConcurrentDictionary<ConnectionNames, CentralConnection>();

		/// <summary>
		/// Perform the given action in the context of the dental office db.
		/// </summary>
		public static void RunPractice(Action a) 
			=> Run(a, ConnectionNames.DentalOffice);

		/// <summary>
		/// Perform the given function in the context of the dental office db.
		/// </summary>
		public static T GetPractice<T>(Func<T> fn) 
			=> GetT(fn, ConnectionNames.DentalOffice);

		/// <summary>
		/// Perform the given action in the context of the given connectionName db.
		/// </summary>
		public static void Run(Action a, ConnectionNames connectionName) 
			=> GetT(new Func<object>(() => { a(); return null; }), connectionName);

		/// <summary>
		/// Perform the given function in the context of the given connectionName db and return a T.
		/// </summary>
		public static T GetT<T>(Func<T> fn, ConnectionNames connectionName)
		{
			T result = default;

			ExecuteThread(new ODThread((o) =>
			{
				using (var dataConnection = new DataConnection())
				{
					ConnectionStore.SetDbT(connectionName, dataConnection);

					result = fn();
				}
			}));

			return result;
		}

		/// <summary>
		/// Adds an exception handler to the thread passed in, starts the thread, and then waits until the thread has finished executing.
		/// This is just a helper method that will throw any exception that occurs within the thread on the parent (usually main) thread.
		/// Throws exceptions.
		/// </summary>
		private static void ExecuteThread(ODThread thread)
		{
			Exception threadException = null;

			thread.AddExceptionHandler((e) => { threadException = e; });
			thread.Start(true);

			// This is intended to be a blocking call so give the action as long as it needs to complete.
			thread.Join(Timeout.Infinite);

			if (threadException != null)
			{ 
				// We are back on the main thread so it is safe to throw.
				throw new Exception(threadException.Message, threadException);
			}
		}
	}
}
