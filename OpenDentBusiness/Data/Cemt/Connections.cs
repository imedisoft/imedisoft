using Imedisoft.Data.Models.Cemt;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Imedisoft.Data.Cemt
{
    public partial class Connections
	{
		// TODO: Move some methods to a more appropriate class...

		/// <summary>
		/// Gets all connections.
		/// </summary>
		public static IEnumerable<Connection> GetAll() 
			=> SelectMany("SELECT * FROM `cemt_connections` ORDER BY `sort_order`");

        /// <summary>
        /// Gets all connections in the group with the specified ID.
        /// </summary>
        public static IEnumerable<Connection> GetAllInGroup(long connectionGroupId)
            => SelectMany("CALL `cemt_get_connections_in_group`(" + connectionGroupId + ")");

		/// <summary>
		/// Gets all connections not in the group with the specified ID.
		/// </summary>
		public static IEnumerable<Connection> GetAllNotInGroup(long connectionGroupId) 
			=> SelectMany("CALL `cemt_get_connections_not_in_group`(" + connectionGroupId + ");");

		public static long Insert(Connection connection) 
			=> ExecuteInsert(connection);
		
		public static void Update(Connection connection) 
			=> ExecuteUpdate(connection);

		public static void Delete(long connectionId)
			=> ExecuteDelete(connectionId);

		/// <summary>
		/// Saves the status of the specified <paramref name="connection"/> to the database.
		/// </summary>
		/// <param name="connection">The connection to update.</param>
		public static void UpdateStatus(Connection connection) 
			=> Database.ExecuteNonQuery(
				"UPDATE `cemt_connections` " +
				"SET `connection_status` = @connection_status " +
				"WHERE `id` =" + connection.Id,
					new MySqlParameter("connection_status", connection.ConnectionStatus ?? ""));

		public static IEnumerable<string> EnumerateDatabases(Connection connection)
		{
			if (string.IsNullOrEmpty(connection.DatabaseServer))
			{
				yield break;
			}

			List<string> results = null;
			try
			{
				using var dataConnection = connection.DatabaseUser == "" ?
					new DataConnection(connection.DatabaseServer, "mysql", connection.DatabasePassword, connection.DatabaseName) :
					new DataConnection(connection.DatabaseServer, connection.DatabaseUser, connection.DatabasePassword, connection.DatabaseName);

				results = dataConnection.SelectMany("SHOW DATABASES", Database.ToScalar<string>).ToList();
			}
            catch
            {
            }

			if (null != results)
            {
				foreach (var databaseName in results)
                {
					yield return databaseName;
                }
            }
		}

		public static void TryToConnect(string connectionString)
        {
			DataConnection dcon = new DataConnection();
			dcon.SetDb(connectionString);
		}

		/// <summary>
		/// Throws an exception to display to the user if anything goes wrong.
		/// </summary>
		public static void TryToConnect(string server, string username, string password, string database, bool autoConnect = false, bool saveConnectionSettings = true)
		{
			var connectionString = DataConnection.CreateConnectionString(server, username, password, database);

			TryToConnect(connectionString);

			if (saveConnectionSettings)
			{
				TrySaveConnectionSettings(connectionString, autoConnect);
			}
		}

		class DatabaseInfoDto
		{
			[JsonProperty(PropertyName = "connectionString")]
			public string ConnectionString { get; set; }

			[JsonProperty(PropertyName = "autoConnect")]
			public bool AutoConnect { get; set; }
		}

		/// <summary>
		/// Returns true if the connection settings were successfully saved to the FreeDentalConfig file. Otherwise, false.
		/// </summary>
		public static bool TrySaveConnectionSettings(string connectionString, bool autoConnect)
		{
			try
			{
				// Get the application data path, if it doesn't exist yet, create it.
				string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Imedisoft");
				try
				{
					if (!Directory.Exists(appDataPath))
					{
						Directory.CreateDirectory(appDataPath);
					}
				}
				catch
				{
				}

				var databaseInfo = new DatabaseInfoDto
				{
					ConnectionString = connectionString,
					AutoConnect = autoConnect
				};


				var json = JsonConvert.SerializeObject(databaseInfo);

				// Save the configuration to the config file.
				string configPath = Path.Combine(appDataPath, "database.json");
				if (!File.Exists(configPath))
				{
					File.Delete(configPath);
				}

				File.WriteAllText(configPath, json);
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Gets all of the connection setting information from the database.json
		/// </summary>
		public static void GetChooseDatabaseConnectionSettings(out string connectionString, out bool autoConnect)
		{
			connectionString = "";
			autoConnect = false;

			// Get the application data path, if it doesn't exist yet, create it.
			string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Imedisoft");
            try
            {
				if (!Directory.Exists(appDataPath))
				{
					Directory.CreateDirectory(appDataPath);
				}
			}
            catch
            {
            }

			// If the configuration file does not exists, we don't do anything, asume the default settings...
			string configPath = Path.Combine(appDataPath, "database.json");
			if (!File.Exists(configPath))
            {
				return;
            }

            try
            {
				var json = File.ReadAllText(configPath);
				if (!string.IsNullOrEmpty(json))
                {
					var databaseInfo = JsonConvert.DeserializeObject<DatabaseInfoDto>(json);

					connectionString = databaseInfo.ConnectionString;
					autoConnect = databaseInfo.AutoConnect;
				}
            }
            catch
            {
            }
		}

        /// <summary>
        /// Supply a CentralConnection and this method will go through the logic to put together the connection string.
        /// </summary>
        public static string GetConnectionString(Connection connection)
		{
			string connString = "";

			if (connection.DatabaseName != "")
			{
				connString = connection.DatabaseServer;
				connString += ", " + connection.DatabaseName;
			}

			return connString;
		}
	}
}
