using Imedisoft.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace OpenDentBusiness
{
    /// <summary>
    /// Should only be used for running user queries.
    /// 
    /// A list of current MySqlConnections is kept so that we can cancel queries running on specific server threads in that list if need be.
    /// </summary>
    public class DataConnectionCancelable
	{
		/// <summary>
		/// A static dictionary of connections via their ServerThread IDs.
		/// </summary>
		private static readonly Dictionary<int, MySqlConnection> _dictCons = new Dictionary<int, MySqlConnection>();

		/// <summary>
		/// The lock object that is used to lock the dictionary of MySqlConnections.
		/// 
		/// Only used when adding and removing from the dict.
		/// </summary>
		private static readonly object _lockObj = new object();

		/// <summary>
		/// Turns "pooling" off, then opens the current database connection, adds that connection to the dictionary of MySqlConnections, then returns the unique ServerThread.  
		/// The returned ServerThread can then be used later in order to stop the query in the middle of executing.
		/// A non-pooled connection will NOT attempt to re-use connections to the DB that already exist but are idle, rather it will create a brand new connection that no other connection can use.
		/// This is so that user queries can be safely cancelled if needed.
		/// Required as a first step for user queries (and ONLY user queries).
		/// </summary>
		public static int GetServerThread()
		{
			string connectionString;

            connectionString = DataConnection.ConnectionString;
            
			var con = new MySqlConnection(connectionString + ";pooling=false");

            con.Open();

			int serverThread = con.ServerThread;

			// If the dictionary already contains the ServerThread key, then something went wrong. Just stop and throw.
			if (_dictCons.ContainsKey(serverThread))
			{
				con.Close();

				throw new ApplicationException(
					"Critical error in GetServerThread: A duplicate connection was found via the server thread ID.");
			}

			lock (_lockObj)
			{
				_dictCons[serverThread] = con;
			}

			return serverThread;
		}

		/// <summary>
		/// Currently only for user queries.
		/// The connection must already be opened before calling this method.
		/// Fills and returns a DataTable from the database.
		/// Use isRunningOnReportServer to indicate if the connection is made directly to the Report Server.
		/// Throws an exception if a connection could not be found via the passed in server thread.
		/// </summary>
		public static DataTable GetTableConAlreadyOpen(int serverThread, string command, bool isSqlValidated, bool isRunningOnReportServer = false)
		{
			throw new NotImplementedException();
   //         // If the dictionary does not contain the ServerThread key, then something went wrong. Just stop and throw.
   //         if (!_dictCons.TryGetValue(serverThread, out MySqlConnection con))
   //         {
   //             throw new ApplicationException(
			//		"Critical error in GetTableConAlreadyOpen: A connection could not be found via the given server thread ID.");
   //         }

   //         // Throws Exception if Sql is not allowed, which is handled by the ExceptionThreadHandler and output in a MsgBox
   //         if (!isSqlValidated && !Db.IsSqlAllowed(command))
			//{
			//	throw new ApplicationException(
			//		"Error: Command is either not safe or user does not have permission.");
			//}

			//// At this point, we know that _dictCons contains the current connection's ServerThread ID.
			//DataTable table = new DataTable();
			//MySqlDataAdapter da = new MySqlDataAdapter(new MySqlCommand(command, con));
			//try
			//{
			//	Db.LastCommand = command;

			//	QueryMonitor.RunMonitoredQuery(() =>
			//	{
			//		Db.Execute(() =>
			//		{
			//			da.Fill(table);

			//		});


			//	}, da.SelectCommand);
			//}
			//finally
			//{
			//	con.Close(); // If the query was stopped or has finished executing, this will close the connection that it was executing on.
			//	lock (_lockObj)
			//	{
			//		_dictCons.Remove(serverThread);
			//	}
			//}

			//return table;
		}

		/// <summary>
		/// Currently only for user queries.
		/// Tries to cancel the connection that corresponds to the passed in server thread. 
		/// Does not close the connection as that is taken care of in GetTableConAlreadyOpen() in a finally statement.
		/// Optionally throws an exception if a connection could not be found via the passed in server thread.
		/// </summary>
		public static void CancelQuery(int serverThread, bool hasExceptions = true)
		{
			if (!_dictCons.ContainsKey(serverThread))
			{
				// This could happen if the user clicked 'Cancel' and by the time it got here, the query finished executing on a different thread.
				// Since this race condition could happen more frequently within the middle tier environment, we just won't do anything about it.

				return; // Should be harmless so don't throw a critical exception here.
			}

			// At this point we want to stop the query currently running on the MySQL connection that corresponds to the server thread passed in.
			try
			{
				Database.ExecuteNonQuery("KILL QUERY " + serverThread);
			}
			catch (MySqlException ex)
			{
				if (ex.Number == 1094)
				{
					return; // Suppress errors about the thread not existing -- that means the query finished executing already.
				}
				
				if (hasExceptions) throw;
			}
			catch
			{
				if (hasExceptions) throw;
			}
		}
	}
}
