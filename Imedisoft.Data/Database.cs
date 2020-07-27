using CodeBase;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Imedisoft.Data
{
    /// <summary>
    /// Used to send queries. The methods are internal since it is not acceptable for the UI to be sending queries.
    /// </summary>
    public class Database
	{
		[ThreadStatic]
		private static string lastCommand;

		/// <summary>
		/// Gets the last SQL command that was executed on the current thread.
		/// </summary>
		public static string LastCommand => lastCommand ?? "[COMMAND NOT SET]";

		/// <summary>
		/// Gets a value indicating whether a connection to the database has been established.
		/// </summary>
		public static bool HasDatabaseConnection => 
			!string.IsNullOrEmpty(DataConnection.ConnectionString) && 
			!string.IsNullOrEmpty(DataConnection.ServerName);

		/// <summary>
		///		<para>
		///			Converts the value of the first column into the given type.
		///		</para>
		///		<para>
		///			Created primarily for usage with <see cref="SelectOne{T}(string, DataRecordBuilder{T}, MySqlParameter[])"/>
		///			and <see cref="SelectMany{T}(string, DataRecordBuilder{T}, MySqlParameter[])"/>.
		///		</para>
		/// </summary>
		/// <typeparam name="T">The type to convert to.</typeparam>
		/// <returns>The converted value.</returns>
		public static T ToScalar<T>(MySqlDataReader dataReader)
			=> (T)Convert.ChangeType(dataReader[0], typeof(T));

		/// <summary>
		/// Checks whether the specified command is allowed and if the user has the correct
		/// permission(s) to run the command.
		/// </summary>
		public static bool IsSqlAllowed(string commandText, bool suppressMessage = false) 
			=> throw new NotImplementedException();

		/// <summary>
		/// Gets the current database name.
		/// </summary>
		public static string CurrentDatabase => ExecuteString("SELECT database()");

		/// <summary>
		/// Executes the specified command and returns the first result.
		/// </summary>
		/// <typeparam name="T">The record type.</typeparam>
		/// <param name="commandText">The SQL command to execute.</param>
		/// <param name="recordBuilder">
		///		Action that converts database records into instances of type <typeparamref name="T"/>.
		///	</param>
		/// <param name="parameters">Optional command parameters.</param>
		/// <returns>The first result.</returns>
		public static T SelectOne<T>(string commandText, DataRecordBuilder<T> recordBuilder, params MySqlParameter[] parameters)
		{
			lastCommand = commandText;

			if (recordBuilder == null) return default;

			using (var dataConnection = new DataConnection())
			{
				return dataConnection.SelectOne(commandText, recordBuilder, parameters);
			}
		}

		/// <summary>
		/// Executes the specified command and returns all results.
		/// </summary>
		/// <typeparam name="T">The record type.</typeparam>
		/// <param name="commandText">The SQL command to execute.</param>
		/// <param name="recordBuilder">Action that converts database records into instances of type <typeparamref name="T"/>.</param>
		/// <param name="parameters">Optional command parameters.</param>
		/// <returns>All results.</returns>
		public static IEnumerable<T> SelectMany<T>(string commandText, DataRecordBuilder<T> recordBuilder, params MySqlParameter[] parameters)
		{
			lastCommand = commandText;

			if (recordBuilder == null) return default;

			using (var dataConnection = new DataConnection())
			{
				return dataConnection.SelectMany(commandText, recordBuilder, parameters);
			}
		}

		/// <summary>
		/// Executes the specified command and returns the results as a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="commandText">The command to execute.</param>
		/// <param name="parameters">Optional command parameters.</param>
		/// <returns>The results.</returns>
		public static DataTable ExecuteDataTable(string commandText, params MySqlParameter[] parameters)
		{
			lastCommand = commandText;

			return Execute(dataConnection => dataConnection.ExecuteDataTable(commandText, true, parameters));
		}


		public static List<long> GetListLong(string commandText) 
            => SelectMany(commandText, dataReader => Convert.ToInt64(dataReader[0])).ToList();

        public static List<string> GetListString(string commandText) 
            => SelectMany(commandText, dataReader => dataReader[0].ToString()).ToList();


        /// <summary>
		/// Executes the specified command query.
		/// </summary>
		/// <param name="commandText">The command to execute.</param>
		/// <param name="parameters">Optional command parameters.</param>
		/// <returns>The number of rows affected.</returns>
		public static long ExecuteNonQuery(string commandText, params MySqlParameter[] parameters)
		{
			lastCommand = commandText;

			return Execute(dataConnection => dataConnection.ExecuteNonQuery(commandText, true, parameters));
		}

		/// <summary>
		/// Executes a insert query and returns the ID of the newly inserted row.
		/// </summary>
        /// <param name="commandText">The command to execute.</param>
        /// <param name="parameters">Optional command parameters.</param>
		/// <returns>The ID assigned to the inserted row.</returns>
		public static long ExecuteInsert(string commandText, params MySqlParameter[] parameters)
        {
            lastCommand = commandText;

            return Execute(dataConnection =>
            {
                dataConnection.ExecuteNonQuery(commandText, true, parameters);

                return dataConnection.LastInsertId;
            });
        }

		/// <summary>
		/// Executes the specified command query and returns the result as a double.
		/// </summary>
		/// <param name="commandText">The command to execute.</param>
		/// <param name="parameters">Optional command parameters.</param>
		/// <returns>The query result.</returns>
		public static double ExecuteDouble(string commandText, params MySqlParameter[] parameters)
			=> Convert.ToDouble(ExecuteScalar(commandText, parameters));

		/// <summary>
		/// Executes the specified command query and returns the result as a date and time.
		/// </summary>
		/// <param name="commandText">The command to execute.</param>
		/// <param name="parameters">Optional command parameters.</param>
		/// <returns>The query result.</returns>
		public static DateTime? ExecuteDateTime(string commandText, params MySqlParameter[] parameters) 
			=> ExecuteScalar(commandText, parameters) as DateTime?;

		/// <summary>
		/// Executes the specified command query and returns the result as a 64-bit integer.
		/// </summary>
		/// <param name="commandText">The command to execute.</param>
		/// <param name="parameters">Optional command parameters.</param>
		/// <returns>The query result.</returns>
		public static long ExecuteLong(string commandText, params MySqlParameter[] parameters)
		{
			var result = ExecuteScalar(commandText, parameters);
			if (result == null)
            {
				return 0;
            }

			return Convert.ToInt64(result);
		}

		/// <summary>
		/// Executes the specified command query and returns the result as a 32-bit integer.
		/// </summary>
		/// <param name="commandText">The command to execute.</param>
		/// <param name="parameters">Optional command parameters.</param>
		/// <returns>The query result.</returns>
		public static int ExecuteInt(string commandText, params MySqlParameter[] parameters)
			=> Convert.ToInt32(ExecuteLong(commandText, parameters));

		/// <summary>
		/// Executes the specified command query and returns the result as a string.
		/// </summary>
		/// <param name="commandText">The command to execute.</param>
		/// <param name="parameters">Optional command parameters.</param>
		/// <returns>The query result.</returns>
		public static string ExecuteString(string commandText, params MySqlParameter[] parameters) 
			=> ExecuteScalar(commandText, parameters)?.ToString() ?? "";

		/// <summary>
		/// Executes the specified command query and returns the first result.
		/// </summary>
		/// <param name="commandText">The command to execute.</param>
		/// <param name="parameters">Optional command parameters.</param>
		/// <returns>The query result.</returns>
		public static object ExecuteScalar(string commandText, params MySqlParameter[] parameters)
		{
			lastCommand = commandText;

			var result = Execute(dataConnection => dataConnection.ExecuteScalar(commandText, true, parameters));

			return result == DBNull.Value ? null : result;
		}

		/// <summary>
		/// Initializes a new data connection and executes the specified function.
		/// </summary>
		/// <typeparam name="T">The type of the result returned by the given function.</typeparam>
		/// <param name="func">The function to execute.</param>
		/// <returns>The result of the function.</returns>
		public static T Execute<T>(Func<DataConnection, T> func)
		{
			if (func == null) return default;

			try
			{
				using (var dataConnection = new DataConnection())
				{
					return func(dataConnection);
				}
			}
			catch (Exception exception)
			{
				if (exception.Message.ToLower().Contains("fatal error"))
				{
					throw new ODException("Query Execution Error", LastCommand, exception);
				}

				throw;
			}
		}
	}
}
