using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;

namespace Imedisoft.Data
{
    public class DataConnection : IDisposable
	{
		private const int ER_GET_ERRNO = 1030;
		private const int ER_OPEN_AS_READONLY = 1036;
		private const int ER_CON_COUNT_ERROR = 1040;
		private const int ER_BAD_HOST_ERROR = 1042;
		private const int ER_DBACCESS_DENIED_ERROR = 1044;
		private const int ER_ACCESS_DENIED_ERROR = 1045;
        private const int ER_NET_PACKET_TOO_LARGE = 1153;

		private readonly MySqlConnection connection;
		private readonly MySqlCommand command;
		private bool disposed = false;

		/// <summary>
		///		<para>
		///			The connection string to use when connecting to the database.
		///		</para>
		/// </summary>
		private static string connectionString = "";

		/// <summary>
		///		<para>
		///			The connection string to use when connecting to the database.
		///		</para>
		///		<para>
		///			This value will override <see cref="connectionString"/> if not null.
		///		</para>
		/// </summary>
		[ThreadStatic]
		private static string connectionStringLocal;

		/// <summary>
		///		<para>
		///			The number of seconds to automatically retry connection to the database when the 
		///			connection has been lost.
		///		</para>
		///		<para>
		///			Defaults to 0 seconds.	
		///		</para>
		/// </summary>
		private static int connectionRetryTimeoutSeconds;

		/// <summary>
		///		<para>
		///			The number of seconds that the thread will automatically retry connection to the database 
		///			when the connection has been lost.
		///		</para>
		///		<para>
		///			Must be set intentionally from every thread that wants to wait for a connection to be 
		///			re-established.
		///		</para>
		///		<para>
		///			This value will override <see cref="connectionRetryTimeoutSeconds"/> if not null.
		///		</para>
		/// </summary>
		[ThreadStatic]
		private static int? connectionRetryTimeoutSecondsLocal;

		/// <summary>
		/// Gets the name of the current database server.
		/// </summary>
		public static string ServerName =>
			new MySqlConnectionStringBuilder(ConnectionString).Server;

		/// <summary>
		/// Gets the name of the current database.
		/// </summary>
		public static string DatabaseName =>
			new MySqlConnectionStringBuilder(ConnectionString).Database;

		/// <summary>
		/// Gets the username for the current database.
		/// </summary>
		public static string User =>
			new MySqlConnectionStringBuilder(ConnectionString).UserID;

		/// <summary>
		/// Gets the password for the current database.
		/// </summary>
		public static string Password =>
			new MySqlConnectionStringBuilder(ConnectionString).Password;

		/// <summary>
		/// Gets or sets the command timeout (in seconds).
		/// </summary>
		public static int CommandTimeout
        {
			get => (int)new MySqlConnectionStringBuilder(ConnectionString).DefaultCommandTimeout;
            set
            {
                var connectionStringBuilder = new MySqlConnectionStringBuilder(ConnectionString)
                {
                    DefaultCommandTimeout = (uint)value
                };

                connectionStringLocal = connectionStringBuilder.ToString();
            }
		}

		/// <summary>
		/// Gets the primary key of the last inserted row.
		/// </summary>
		public long LastInsertId { get; private set; }

		/// <summary>
		/// Gets current connection string.
		/// </summary>
		public static string ConnectionString 
			=> string.IsNullOrEmpty(connectionStringLocal) ? connectionString : connectionStringLocal;
		
		/// <summary>
		/// The number of seconds that the thread will automatically retry connecting to the database 
		/// when the connection has been lost.
		/// </summary>
		public static int ConnectionRetryTimeoutSeconds
        {
            get => connectionRetryTimeoutSecondsLocal ?? connectionRetryTimeoutSeconds;
            set => connectionRetryTimeoutSecondsLocal = connectionRetryTimeoutSeconds = value;
		}

		/// <summary>
		///		<para>
		///			Initializes a new instance of the <see cref="DataConnection"/> class.
		///		</para>
		/// </summary>
		public DataConnection() : this(ConnectionString)
		{
		}

		/// <summary>
		///		<para>
		///			Initializes a new instance of the <see cref="DataConnection"/> class.
		///		</para>
		/// </summary>
		/// <param name="server"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="database"></param>
		/// <param name="port"></param>
		public DataConnection(string server, string username, string password, string database, int port = 3306) :
			this(CreateConnectionString(server, username, password, database, port))
        {
        }

		/// <summary>
		///		<para>
		///			Initializes a new instance of the <see cref="DataConnection"/> class.
		///		</para>
		/// </summary>
		/// <param name="connectionString">The connection string to use for this connection.</param>
		public DataConnection(string connectionString)
		{
			connectionStringLocal = connectionString;

			connection = new MySqlConnection(connectionString);
			command = connection.CreateCommand();
		}

		/// <summary>
		/// Executes the specified command and returns the first result.
		/// </summary>
		/// <typeparam name="T">The record type.</typeparam>
		/// <param name="commandText">The SQL command to execute.</param>
		/// <param name="recordBuilder">Action that converts database records into instances of type <typeparamref name="T"/>.</param>
		/// <param name="parameters">Optional command parameters.</param>
		/// <returns>The first result.</returns>
		public T SelectOne<T>(string commandText, DataRecordBuilder<T> recordBuilder, params MySqlParameter[] parameters)
		{
			// TODO: Add support for monitoring queries run by this method...

			if (recordBuilder == null) return default;

			command.CommandText = commandText;
			command.Parameters.Clear();
			command.Parameters.AddRange(parameters);

			try
			{
				connection.Open();

				using (var dataReader = command.ExecuteReader())
				{
					if (dataReader.Read())
					{
						return recordBuilder(dataReader);
					}
				}
			}
            catch (MySqlException exception)
            {
				if (!IsErrorHandled(exception))
				{
					throw;
				}
			}
            finally
            {
				connection.Close();
            }

			return default;
		}

		/// <summary>
		/// Executes the specified command and returns all results.
		/// </summary>
		/// <typeparam name="T">The record type.</typeparam>
		/// <param name="commandText">The SQL command to execute.</param>
		/// <param name="recordBuilder">Action that converts database records into instances of type <typeparamref name="T"/>.</param>
		/// <param name="parameters">Optional command parameters.</param>
		/// <returns>All results.</returns>
		public IEnumerable<T> SelectMany<T>(string commandText, DataRecordBuilder<T> recordBuilder, params MySqlParameter[] parameters)
		{
			// TODO: Add support for monitoring queries run by this method...

			if (recordBuilder == null) yield break;

			command.CommandText = commandText;
			command.Parameters.Clear();
			command.Parameters.AddRange(parameters);

			MySqlDataReader dataReader;
			try
			{
				connection.Open();

				dataReader = command.ExecuteReader();
			}
			catch (MySqlException exception)
			{
				connection.Close();

				if (!IsErrorHandled(exception))
                {
					throw;
                }

				yield break;
			}

			while (dataReader.Read())
			{
				yield return recordBuilder(dataReader);
			}

			connection.Close();
		}

		/// <summary>
		///		<para>
		///			Creates a connection string for the specified parameters.
		///		</para>
		/// </summary>
		/// <param name="server"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="database"></param>
		/// <param name="port"></param>
		/// <returns>A connection string.</returns>
		public static string CreateConnectionString(string server, string username, string password, string database, int port = 3306)
		{
			var connectionStringBuilder = new MySqlConnectionStringBuilder
			{
				Server = server,
				UserID = username,
				Password = password,
				Database = database,
				Port = (uint)port,
				SslMode = MySqlSslMode.None,
				CharacterSet = "utf8",
				TreatTinyAsBoolean = false,
				AllowUserVariables = true,
				ConvertZeroDateTime = true,
				DefaultCommandTimeout = 3600
			};

			return connectionStringBuilder.ToString();
		}

		public void SetDb(string server, string username, string password, string database, int port = 3306, bool skipValidation = false)
			=> SetDb(CreateConnectionString(server, username, password, database, port), skipValidation);

		public void SetDb(string connectionString, bool skipValidation = false)
		{
			TestConnection(connectionString, skipValidation);

            DataConnection.connectionString = connectionStringLocal = connectionString;
		}

		public void SetDbLocal(string server, string username, string password, string database, int port = 3306, bool skipValidation = false, bool skipTestConnection = false)
			=> SetDbLocal(CreateConnectionString(server, username, password, database, port), skipValidation, skipTestConnection);

		public void SetDbLocal(string connectionString, bool skipValidation = false, bool skipTestConnection = false)
		{
			if (!skipTestConnection)
			{
				TestConnection(connectionString, skipValidation);
			}

			connectionStringLocal = connectionString;
		}

		/// <summary>
		/// Attempts to open a connection with a database using the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string to test.</param>
		/// <param name="skipValidation"></param>
		private static void TestConnection(string connectionString, bool skipValidation)
		{
			var connection = new MySqlConnection(connectionString);

			connection.Open();

			if (!skipValidation)
			{
				var command = connection.CreateCommand();

				command.CommandText = "UPDATE `preference` SET `ValueString` = '0' WHERE `ValueString` = '0'";

				RunAction(() => command.ExecuteNonQuery(), connection, command);
			}

			connection.Close();
		}

		/// <summary>
		/// Executes the specified command and returns the results as a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="commandText">The command to execute.</param>
		/// <param name="autoRetry">A value indicating whether to auto retry when the command fails.</param>
		/// <param name="parameters">Optional command parameters.</param>
		/// <returns>The results.</returns>
		public DataTable ExecuteDataTable(string commandText, bool autoRetry = true, params MySqlParameter[] parameters)
		{
#if DEBUG
			Debug.WriteLine(commandText);
#endif

			var dataTable = new DataTable();

			command.CommandText = commandText;
			command.Parameters.Clear();
			command.Parameters.AddRange(parameters);

			try
			{
				connection.Open();

				RunAction(() =>
				{
					using (var dataAdapter = new MySqlDataAdapter(command))
                    {
						dataAdapter.Fill(dataTable);
                    }
				});
			}
			catch (MySqlException exception)
			{
				if (autoRetry && IsErrorHandled(exception))
				{
					connection.Close();

					return ExecuteDataTable(commandText, true);
				}

				throw;
			}
            finally
            {
				connection.Close();
			}

			return dataTable;
		}

        /// <summary>
        /// Executes the specified command query.
        /// </summary>
        /// <param name="commandText">The command to execute.</param>
        /// <param name="parameters">Optional command parameters.</param>
        /// <returns>The number of rows affected.</returns>
		public long ExecuteNonQuery(string commandText, bool autoRetry = false, params MySqlParameter[] parameters)
		{
#if DEBUG
			Debug.WriteLine(commandText);
#endif

			long rowsAffected = 0;

			command.CommandText = commandText;
			command.Parameters.Clear();
			command.Parameters.AddRange(parameters);

			try
			{
				connection.Open();

				RunAction(new Action(() => rowsAffected = command.ExecuteNonQuery()));

				LastInsertId = command.LastInsertedId;
			}
			catch (MySqlException ex)
			{
				connection.Close();

				if (ex.Number == ER_NET_PACKET_TOO_LARGE)
				{
					throw new ApplicationException("Please add the following to your my.ini file: max_allowed_packet=40000000");
				}

				if (autoRetry && IsErrorHandled(ex))
				{
					return ExecuteNonQuery(commandText, autoRetry, parameters);
				}

				throw;
			}
            finally
            {
				connection.Close();
			}

			return rowsAffected;
		}

		/// <summary>
		/// Get one value.
		/// </summary>
		public object ExecuteScalar(string commandText, bool autoRetry = true, params MySqlParameter[] parameters)
		{
#if DEBUG
			Debug.WriteLine(commandText);
#endif

			object scalar = null;

			command.CommandText = commandText;
			command.Parameters.Clear();
			command.Parameters.AddRange(parameters);

			try
			{
				connection.Open();

                RunAction(new Action(() => scalar = command.ExecuteScalar()));
			}
			catch (MySqlException exception)
			{
				connection.Close();

				if (autoRetry && IsErrorHandled(exception))
				{
					return ExecuteScalar(commandText, autoRetry);
				}

				throw;
			}
            finally
            {
				connection.Close();
			}

			return scalar;
		}

		/// <summary>
		/// Run an action using the MySqlConnection and close the MySqlConnection if the action throws an exception.
		/// Re-throws the exception.
		/// This is strictly used to close the orphaned connection.
		/// </summary>
		private void RunAction(Action action) 
			=> RunAction(action, connection, command);

		/// <summary>
		/// Run an action using the given connection and close the connection if the action throws an exception. Re-throws the exception.
		/// This is strictly used to close the orphaned connection and to handle special exceptions.
		/// Do not call this directly, instead use RunMySqlAction() or RunOracleAction().
		/// </summary>
		private static void RunAction(Action action, MySqlConnection connection, MySqlCommand command)
		{
			try
			{
				QueryMonitor.RunMonitoredQuery(action, command);
			}
			catch (MySqlException exception)
			{
				connection.Close();

				// This occurs when the servers storage is too full. 
				// Instead of saying, "got error 28 from storage engine", we will catch the error and give them a better message to avoid them from calling us.
				if (exception.Number == ER_GET_ERRNO && exception.Message.ToLower() == "got error 28 from storage engine")
				{
					throw new Exception("The server's storage is full. Free space to avoid this error.", exception);
				}

				// If the exception was triggered because MySQL marked a table as read-only, just retry it once...
				if (exception.Number == ER_OPEN_AS_READONLY)
				{
					RetryQuery(action, connection);
					return;
				}

				if ((exception.Message.ToLower().Contains("fatal error") || exception.Message.ToLower().Contains("transport connection")) && CommandCanBeRetried(command))
				{
					RetryQuery(action, connection);
					return;
				}

				throw;
			}
            finally
            {
				connection.Close();
            }
		}

		/// <summary>
		/// Returns a value indicating whether the command can be safely retried.
		/// </summary>
		private static bool CommandCanBeRetried(MySqlCommand command) => 
			command.CommandText.ToLower().StartsWith("select") || 
			command.CommandText.ToLower().StartsWith("insert into securitylog");

		/// <summary>
		/// Handles certain types of MySQL errors. 
		/// The application may pause here to wait for the error to be resolved.
		/// Returns true if the calling method should retry the db action that just failed.  E.g. recursively invoke GetTable()
		/// Returns false if the exception passed in is not specifically handled and the program should crash.
		/// </summary>
		private bool IsErrorHandled(MySqlException exception)
		{
			if (IsConnectionLost(exception))
			{
				return StartConnectionErrorRetry(connection.ConnectionString, 
					DataConnectionEventType.ConnectionLost);
			}

			if (IsTooManyConnections(exception))
			{
				return StartConnectionErrorRetry(connection.ConnectionString, 
					DataConnectionEventType.TooManyConnections);
			}

			return false;
		}

		/// <summary>
		/// Fires an event to launch the LostConnection window and freezes the calling thread until
		/// connection has been restored or the timeout has been reached. This is only a blocking 
		/// call when ConnectionRetrySeconds is greater than 0. Immediately returns if 
		/// ConnectionRetrySeconds is 0. Returns true if the calling method should retry the db 
		/// action that just failed. E.g. recursively invoke GetTable()
		/// Returns false if the calling method should instead bubble up the original exception.
		/// </summary>
		private bool StartConnectionErrorRetry(string connectionString, DataConnectionEventType errorType)
		{
			if (ConnectionRetryTimeoutSeconds == 0) return false;

			DataConnectionEvent.Fire(
				new DataConnectionEventArgs(errorType, false, connectionString));

			var connectionRestored = false;

			void RetryConnection()
            {
				var start = DateTime.Now;

				while (true)
                {
					if (connectionRestored)
					{
						return;
					}

					var elapsed = DateTime.Now - start;
					if (elapsed.TotalSeconds > ConnectionRetryTimeoutSeconds)
                    {
						return;
                    }

                    try
                    {
						TestConnection(connectionString, false);

						connectionRestored = true;

						return;
                    }
                    catch { }

					Thread.Sleep(500);
				}
            }

            var retryThread = new Thread(RetryConnection)
            {
                Name = "DataConnectionAutoRetryThread"
            };

            retryThread.Start();
			retryThread.Join();

			if (connectionRestored)
            {
				DataConnectionEvent.Fire(
					new DataConnectionEventArgs(DataConnectionEventType.ConnectionRestored, true, connectionString));
			}

			return connectionRestored;
		}

		/// <summary>
		/// Checks whether the specified exception indicates a <b>ER_BAD_HOST_ERROR</b> (1042), 
		/// <b>ER_DBACCESS_DENIED_ERROR</b> (1044) or <b>ER_ACCESS_DENIED_ERRO</b> (1045) error.
		/// </summary>
		private static bool IsConnectionLost(MySqlException ex) 
			=> ex.Number == ER_BAD_HOST_ERROR || ex.Number == ER_DBACCESS_DENIED_ERROR || ex.Number == ER_ACCESS_DENIED_ERROR;

		/// <summary>
		/// Checks whether the specified exception indicates a <b>ER_CON_COUNT_ERROR</b> (1040) error.
		/// </summary>
		private static bool IsTooManyConnections(MySqlException exception) 
			=> exception.Number == ER_CON_COUNT_ERROR;

		/// <summary>
		/// This method will retry a query once.
		/// This should only be used in special cases as generally if a query causes an exception, there is a good reason for doing so.
		/// </summary>
		/// <param name="action">The given action that caused the exception. Will be ran again.</param>
		/// <param name="connection">The connection used to run the query.</param>
		private static void RetryQuery(Action action, MySqlConnection connection)
		{
			connection.Open();

			action();
		}

		/// <summary>
		/// All disposable entities should be disposed here.
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (disposed) return;

			if (disposing)
			{
				connection?.Close();
				connection?.Dispose();
				command?.Dispose();
			}

			disposed = true;
		}

		public void Dispose() => Dispose(true);
	}
}
