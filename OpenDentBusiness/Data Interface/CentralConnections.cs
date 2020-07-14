using CodeBase;
using DataConnectionBase;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace OpenDentBusiness
{
    public class CentralConnections
	{
		/// <summary>
		/// Gets all the central connections from the database ordered by ItemOrder.
		/// </summary>
		public static List<CentralConnection> GetConnections() 
			=> Crud.CentralConnectionCrud.SelectMany(
				"SELECT * FROM centralconnection ORDER BY ItemOrder");

		/// <summary>
		/// Gets all the central connections from the database for one group, ordered by ItemOrder.
		/// </summary>
		public static List<CentralConnection> GetForGroup(long connectionGroupNum) 
			=> Crud.CentralConnectionCrud.SelectMany(
				"SELECT centralconnection.* FROM conngroupattach " +
				"LEFT JOIN centralconnection ON conngroupattach.CentralConnectionNum=centralconnection.CentralConnectionNum " +
				"WHERE conngroupattach.ConnectionGroupNum=" + SOut.Long(connectionGroupNum) + " " +
				"ORDER BY ItemOrder");

		/// <summary>
		/// Gets all the central connections from the database that are not in the specificed group, ordered by ItemOrder.
		/// </summary>
		public static List<CentralConnection> GetNotForGroup(long connectionGroupNum) 
			=> Crud.CentralConnectionCrud.SelectMany(
				"SELECT centralconnection.* FROM centralconnection " +
				"LEFT JOIN conngroupattach ON conngroupattach.CentralConnectionNum=centralconnection.CentralConnectionNum AND conngroupattach.ConnectionGroupNum = " + SOut.Long(connectionGroupNum) + " " +
				"WHERE conngroupattach.ConnectionGroupNum IS NULL " +
				"ORDER BY ItemOrder");

		public static long Insert(CentralConnection centralConnection) 
			=> Crud.CentralConnectionCrud.Insert(centralConnection);
		
		public static void Update(CentralConnection centralConnection) 
			=> Crud.CentralConnectionCrud.Update(centralConnection);

		/// <summary>
		/// Updates Status of the provided CentralConnection
		/// </summary>
		public static void UpdateStatus(CentralConnection centralConnection) => Db.NonQ(
			"UPDATE centralconnection " +
			"SET ConnectionStatus='" + SOut.String(centralConnection.ConnectionStatus) + "' " +
			"WHERE CentralConnectionNum=" + SOut.Long(centralConnection.CentralConnectionNum));
		
		public static void Delete(long centralConnectionNum) => Db.NonQ(
			"DELETE FROM centralconnection " +
			"WHERE CentralConnectionNum = " + SOut.Long(centralConnectionNum));

		public static IEnumerable<string> EnumerateDatabases(CentralConnection centralConnection)
		{
			if (string.IsNullOrEmpty(centralConnection.ServerName))
			{
				yield break;
			}

			DataTable dataTable;
			try
			{
				var dataConnection = centralConnection.MySqlUser == "" ?
					new DataConnection(centralConnection.ServerName, "mysql", "root", centralConnection.MySqlPassword) :
					new DataConnection(centralConnection.ServerName, "mysql", centralConnection.MySqlUser, centralConnection.MySqlPassword);

				dataTable = dataConnection.GetTable("SHOW DATABASES", false);
			}
			catch
			{
				yield break;
			}

			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				yield return dataTable.Rows[i][0].ToString();
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
		/// Filters _listConns to only include connections that are associated to the selected connection group.
		/// </summary>
		public static List<CentralConnection> FilterConnections(List<CentralConnection> connections, string filterText, ConnectionGroup connectionGroup)
		{
			var result = connections;
			if (connectionGroup != null)
			{
				var connectionGroupAttaches = ConnGroupAttaches.GetForGroup(connectionGroup.ConnectionGroupNum);

				result = result.FindAll(
					x => connectionGroupAttaches.Exists(
						y => y.CentralConnectionNum == x.CentralConnectionNum));
			}

			// Find all central connections that meet the filterText criteria
			result = result.FindAll(
				x => x.DatabaseName.ToLower().Contains(filterText.ToLower()) || 
				     x.ServerName.ToLower().Contains(filterText.ToLower()));

			return result;
		}

		/// <summary>
		/// Encrypts signature text and returns a base 64 string so that it can go directly into the database.
		/// </summary>
		[Obsolete]
		public static string Encrypt(string str, byte[] key) => str;

		[Obsolete]
		public static string Decrypt(string str, byte[] key) => str;

		/// <summary>
		/// Supply a CentralConnection and this method will go through the logic to put together the connection string.
		/// </summary>
		public static string GetConnectionString(CentralConnection conn)
		{
			string connString = "";

			if (conn.DatabaseName != "")
			{
				connString = conn.ServerName;
				connString += ", " + conn.DatabaseName;
			}

			return connString;
		}
	}
}
