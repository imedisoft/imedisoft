using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using Microsoft.VisualBasic.Devices;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;

namespace OpenDentBusiness
{
    public class MiscData
	{
		/// <summary>
		/// Gets the current date/Time direcly from the server.
		/// Mostly used to prevent uesr from altering the workstation date to bypass security.
		/// </summary>
		public static DateTime GetNowDateTime() => Database.ExecuteDateTime("SELECT NOW()") ?? DateTime.MinValue;

		/// <summary>
		/// Gets the current date/Time with milliseconds directly from server.
		/// In Mysql we must query the server until the second rolls over, which may take up to one second.
		/// Used to confirm synchronization in time for EHR.
		/// </summary>
		public static DateTime GetNowDateTimeWithMilli()
		{
			// Only up to 1 second precision pre-Mysql 5.6.4. Does not round milliseconds.
			int secondInit = GetNowDateTime().Second;
			int secondCur;

			//Continue querying server for current time until second changes (milliseconds will be close to 0)
			DateTime time;
			do
			{
				time = GetNowDateTime();
				secondCur = time.Second;
			}
			while (secondInit == secondCur);

			return time;
		}

		/// <summary>
		/// Backs up the database to the same directory as the original just in case the user did not have sense enough to do a backup first.
		/// </summary>
		public static void MakeABackup()
		{
			// TODO: Implement me...

			////This function should always make the backup on the server itself, and since no directories are
			////referred to (all handled with MySQL), this function will always be referred to the server from
			////client machines.

			////UpdateStreamLinePassword is purposefully named poorly and used in an odd fashion to sort of obfuscate it from our users.
			////GetStringNoCache() will return blank if pref does not exist.
			//if (Prefs.GetStringNoCache(PrefName.UpdateStreamLinePassword) == "abracadabra")
			//{
			//	return;
			//}
			//string currentServerName = DataConnection.ServerName;
			//bool useSameServer = string.IsNullOrWhiteSpace(serverName) || currentServerName.Equals(serverName, StringComparison.CurrentCultureIgnoreCase);
			//if (!string.IsNullOrWhiteSpace(serverName) && currentServerName == "localhost" && serverName.ToLower() != "localhost")
			//{ //there could be a mismatch but technically the same server
			//	useSameServer = serverName.Equals(Environment.MachineName, StringComparison.CurrentCultureIgnoreCase);
			//}
			//if (serverName.ToLower() == "localhost" && currentServerName != "localhost")
			//{ //there could be a mismatch but technically the same server
			//	useSameServer = currentServerName.Equals(Environment.MachineName, StringComparison.CurrentCultureIgnoreCase);
			//}
			////only used in two places: upgrading version, and upgrading mysql version.
			////Both places check first to make sure user is using mysql.
			////we have to be careful to throw an exception if the backup is failing.
			//using DataConnection dcon = new DataConnection();
			////if they provided a different server where they want their backup to be, we need a separate connection for that
			//using DataConnection dconBackupServer = useSameServer ? new DataConnection() : new DataConnection(connectionString);
			////Check that the backup server does not already contain this database
			//string command = "SELECT database()";
			//DataTable table = dcon.GetTable(command);
			//string oldDb = PIn.String(table.Rows[0][0].ToString());
			//string newDb = oldDb + "backup_" + DateTime.Today.ToString("MM_dd_yyyy");
			//command = "SHOW DATABASES";
			//table = dconBackupServer.GetTable(command);
			//string[] databases = new string[table.Rows.Count];
			//for (int i = 0; i < table.Rows.Count; i++)
			//{
			//	databases[i] = table.Rows[i][0].ToString();
			//}
			//int uniqueID = 1;
			//string originalNewDb = newDb;
			//while (Contains(databases, newDb))
			//{//if the new database name already exists find a unique one
			//	newDb = originalNewDb + "_" + uniqueID++.ToString();
			//}
			//command = "CREATE DATABASE `" + newDb + "` CHARACTER SET utf8";
			//dconBackupServer.ExecuteNonQuery(command); //create the backup db on the backup server
			//								//But get the tables from the current, not the backup server
			//command = "SHOW FULL TABLES WHERE Table_type='BASE TABLE'";//Tables, not views.  Does not work in MySQL 4.1, however we test for MySQL version >= 5.0 in PrefL.
			//table = dcon.GetTable(command);
			////Set the connection to the new database now that it has been created
			//DataConnection.CommandTimeout = 43200;//12 hours, because backup commands may take longer to run.
			//try
			//{
			//	using DataConnection dconBackupServerNoTimout = useSameServer ? new DataConnection(newDb) : new DataConnection(connectionString);
			//	foreach (DataRow row in table.Rows)
			//	{
			//		string tableName = row[0].ToString();
			//		//First create the table on the new db
			//		MiscDataEvent.Fire(ODEventType.MiscData, $"Backing up table: {tableName}");
			//		//also works with views. Added backticks around table name for unusual characters.
			//		command = $"SHOW CREATE TABLE `{oldDb}`.`{tableName}`";
			//		DataTable dtCreate = dcon.GetTable(command);
			//		command = PIn.ByteArray(dtCreate.Rows[0][1]);
			//		//The backup database tables will be MyISAM because it is significantly faster at doing bulk inserts.
			//		command = command.Replace("ENGINE=InnoDB", "ENGINE=MyISAM");
			//		dconBackupServerNoTimout.ExecuteNonQuery(command);
			//		//Then copy the data into the new table
			//		if (useSameServer)
			//		{
			//			//If on the same server we can select into directly, which is faster
			//			command = $"INSERT INTO `{newDb}`.`{tableName}` SELECT * FROM `{oldDb}`.`{tableName}`";//Added backticks around table name for unusual characters.
			//			dconBackupServerNoTimout.ExecuteNonQuery(command);
			//		}
			//		else
			//		{
			//			long count = PIn.Long(dcon.GetCount($"SELECT COUNT(*) FROM `{oldDb}`.`{tableName}`"));
			//			int limit = 10000;
			//			if (tableName == "documentmisc")
			//			{ //This table can have really large rows so just to be safe, handle the backup one row at a time
			//				limit = 1;
			//			}
			//			int offset = 0;
			//			while (count > offset)
			//			{
			//				DataTable dtOld = dcon.GetTable($" SELECT * FROM `{oldDb}`.`{tableName}` LIMIT {limit} OFFSET {offset}");
			//				offset += dtOld.Rows.Count;
			//				dconBackupServerNoTimout.BulkCopy(dtOld, tableName);
			//			}
			//		}
			//	}
			//	//Verify that the old database and the new backup have the same number of rows
			//	if (doVerify)
			//	{
			//		List<string> listTablesFailed = new List<string>();
			//		foreach (DataRow dbTable in table.Rows)
			//		{
			//			string tableName = dbTable[0].ToString();
			//			MiscDataEvent.Fire(ODEventType.MiscData, $"Verifying backup: {tableName}");
			//			int ctOld = PIn.Int(dcon.GetCount($"SELECT COUNT(*) FROM `{oldDb}`.`{tableName}`"));
			//			int ctNew = PIn.Int(dconBackupServerNoTimout.GetCount($"SELECT COUNT(*) FROM `{newDb}`.`{tableName}`"));
			//			if (ctOld != ctNew)
			//			{
			//				listTablesFailed.Add(tableName);
			//			}
			//		}
			//		if (listTablesFailed.Count > 0)
			//		{
			//			throw new Exception($@"Failed to create database backup because the following tables contained a different number of rows than expected: 
			//				{string.Join(", ", listTablesFailed)}.");
			//		}
			//	}
			//}
			//finally
			//{
			//	DataConnection.CommandTimeout = 3600;//Set back to default of 1 hour.
			//}
		}

		public static string GetCurrentDatabase() 
			=> Database.ExecuteString("SELECT database()");

		public static string GetMySqlVersion()
		{
			var parts = Database.ExecuteString("SELECT @@version").Split('.');

			if (parts.Length >= 2)
            {
				if (int.TryParse(parts[0], out var major) && 
					int.TryParse(parts[1], out var minor))
                {
					return major + "." + minor;
                }
            }

			return "0.0";
		}

		public static string GetMySqlServer()
		{
			var hostName = DataConnection.ServerName;
			if (!string.IsNullOrEmpty(hostName))
            {
				var p = hostName.IndexOf(':');
				if (p != -1)
                {
					hostName = hostName.Substring(p + 1);
                }
            }

			try
			{
				var hostEntry = Dns.GetHostEntry(hostName);
				if (hostEntry != null)
				{
					hostName = hostEntry.HostName;
				}
			}
			catch
			{
			}

			return hostName;
		}

		public static int GetMaxAllowedPacket() 
			=> Database.ExecuteInt("SELECT @@max_allowed_packet");

		public static void SetSqlMode()
		{
			var sqlMode = Database.ExecuteString("SELECT @@sql_mode").ToUpper();

			if (string.IsNullOrEmpty(sqlMode) || !sqlMode.Contains("NO_AUTO_CREATE_USER"))
			{
				if (string.IsNullOrEmpty(sqlMode)) sqlMode = "NO_AUTO_CREATE_USER";
                else
                {
					sqlMode += ",NO_AUTO_CREATE_USER";
                }

				Database.ExecuteNonQuery("SET GLOBAL sql_mode = @sql_mode", new MySqlParameter("sql_mode", sqlMode));
			}
		}
	}
}
