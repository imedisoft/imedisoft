using CodeBase;
using DataConnectionBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Reflection;
using Microsoft.VisualBasic.Devices;
using System.Management;
using System.Linq;

namespace OpenDentBusiness
{

	///<summary>Miscellaneous database functions.</summary>
	public class MiscData
	{

		///<summary>Gets the current date/Time direcly from the server.  Mostly used to prevent uesr from altering the workstation date to bypass security.</summary>
		public static DateTime GetNowDateTime()
		{
			string command = "SELECT NOW()";
			if (DataConnection.DBtype == DatabaseType.Oracle)
			{
				command = "SELECT CURRENT_TIMESTAMP FROM DUAL";
			}
			DataTable table = Db.GetTable(command);
			return PIn.DateT(table.Rows[0][0].ToString());
		}

		///<summary>Gets the current date/Time with milliseconds directly from server.  In Mysql we must query the server until the second rolls over, which may take up to one second.  Used to confirm synchronization in time for EHR.</summary>
		public static DateTime GetNowDateTimeWithMilli()
		{
			string command;
			string dbtime;
			if (DataConnection.DBtype == DatabaseType.MySql)
			{
				command = "SELECT NOW()"; //Only up to 1 second precision pre-Mysql 5.6.4.  Does not round milliseconds.
				dbtime = Db.GetScalar(command);
				int secondInit = PIn.DateT(dbtime).Second;
				int secondCur;
				//Continue querying server for current time until second changes (milliseconds will be close to 0)
				do
				{
					dbtime = Db.GetScalar(command);
					secondCur = PIn.DateT(dbtime).Second;
				}
				while (secondInit == secondCur);
			}
			else
			{
				command = "SELECT CURRENT_TIMESTAMP(3) FROM DUAL"; //Timestamp with milliseconds
				dbtime = Db.GetScalar(command);
			}
			return PIn.DateT(dbtime);
		}

		///<summary>Used in MakeABackup to ensure a unique backup database name.</summary>
		private static bool Contains(string[] arrayToSearch, string valueToTest)
		{
			//No need to check RemotingRole; no call to db.
			string compare;
			for (int i = 0; i < arrayToSearch.Length; i++)
			{
				compare = arrayToSearch[i];
				if (arrayToSearch[i] == valueToTest)
				{
					return true;
				}
			}
			return false;
		}

		///<summary>Backs up the database to the same directory as the original just in case the user did not have sense enough to do a backup first.
		///Does not work for Oracle, due to some MySQL specific commands inside.</summary>
		public static void MakeABackup(string serverName = "", string user = "", string pass = "", bool doVerify = false)
		{
			//This function should always make the backup on the server itself, and since no directories are
			//referred to (all handled with MySQL), this function will always be referred to the server from
			//client machines.

			//UpdateStreamLinePassword is purposefully named poorly and used in an odd fashion to sort of obfuscate it from our users.
			//GetStringNoCache() will return blank if pref does not exist.
			if (PrefC.GetStringNoCache(PrefName.UpdateStreamLinePassword) == "abracadabra")
			{
				return;
			}
			string currentServerName = DataConnection.GetServerName().ToLower();
			bool useSameServer = string.IsNullOrWhiteSpace(serverName) || currentServerName.Equals(serverName, StringComparison.CurrentCultureIgnoreCase);
			if (!string.IsNullOrWhiteSpace(serverName) && currentServerName == "localhost" && serverName.ToLower() != "localhost")
			{ //there could be a mismatch but technically the same server
				useSameServer = serverName.Equals(Environment.MachineName, StringComparison.CurrentCultureIgnoreCase);
			}
			if (serverName.ToLower() == "localhost" && currentServerName != "localhost")
			{ //there could be a mismatch but technically the same server
				useSameServer = currentServerName.Equals(Environment.MachineName, StringComparison.CurrentCultureIgnoreCase);
			}
			//only used in two places: upgrading version, and upgrading mysql version.
			//Both places check first to make sure user is using mysql.
			//we have to be careful to throw an exception if the backup is failing.
			using DataConnection dcon = new DataConnection();
			//if they provided a different server where they want their backup to be, we need a separate connection for that
			using DataConnection dconBackupServer = useSameServer ? new DataConnection() : new DataConnection(serverName, "", user, pass, DatabaseType.MySql);
			//Check that the backup server does not already contain this database
			string command = "SELECT database()";
			DataTable table = dcon.GetTable(command);
			string oldDb = PIn.String(table.Rows[0][0].ToString());
			string newDb = oldDb + "backup_" + DateTime.Today.ToString("MM_dd_yyyy");
			command = "SHOW DATABASES";
			table = dconBackupServer.GetTable(command);
			string[] databases = new string[table.Rows.Count];
			for (int i = 0; i < table.Rows.Count; i++)
			{
				databases[i] = table.Rows[i][0].ToString();
			}
			int uniqueID = 1;
			string originalNewDb = newDb;
			while (Contains(databases, newDb))
			{//if the new database name already exists find a unique one
				newDb = originalNewDb + "_" + uniqueID++.ToString();
			}
			command = "CREATE DATABASE `" + newDb + "` CHARACTER SET utf8";
			dconBackupServer.NonQ(command); //create the backup db on the backup server
											//But get the tables from the current, not the backup server
			command = "SHOW FULL TABLES WHERE Table_type='BASE TABLE'";//Tables, not views.  Does not work in MySQL 4.1, however we test for MySQL version >= 5.0 in PrefL.
			table = dcon.GetTable(command);
			//Set the connection to the new database now that it has been created
			DataConnection.CommandTimeout = 43200;//12 hours, because backup commands may take longer to run.
			try
			{
				using DataConnection dconBackupServerNoTimout = useSameServer ? new DataConnection(newDb) : new DataConnection(serverName, newDb, user, pass, DatabaseType.MySql);
				foreach (DataRow row in table.Rows)
				{
					string tableName = row[0].ToString();
					//First create the table on the new db
					MiscDataEvent.Fire(ODEventType.MiscData, $"Backing up table: {tableName}");
					//also works with views. Added backticks around table name for unusual characters.
					command = $"SHOW CREATE TABLE `{oldDb}`.`{tableName}`";
					DataTable dtCreate = dcon.GetTable(command);
					command = PIn.ByteArray(dtCreate.Rows[0][1]);
					//The backup database tables will be MyISAM because it is significantly faster at doing bulk inserts.
					command = command.Replace("ENGINE=InnoDB", "ENGINE=MyISAM");
					dconBackupServerNoTimout.NonQ(command);
					//Then copy the data into the new table
					if (useSameServer)
					{
						//If on the same server we can select into directly, which is faster
						command = $"INSERT INTO `{newDb}`.`{tableName}` SELECT * FROM `{oldDb}`.`{tableName}`";//Added backticks around table name for unusual characters.
						dconBackupServerNoTimout.NonQ(command);
					}
					else
					{
						long count = PIn.Long(dcon.GetCount($"SELECT COUNT(*) FROM `{oldDb}`.`{tableName}`"));
						int limit = 10000;
						if (tableName == "documentmisc")
						{ //This table can have really large rows so just to be safe, handle the backup one row at a time
							limit = 1;
						}
						int offset = 0;
						while (count > offset)
						{
							DataTable dtOld = dcon.GetTable($" SELECT * FROM `{oldDb}`.`{tableName}` LIMIT {limit} OFFSET {offset}");
							offset += dtOld.Rows.Count;
							dconBackupServerNoTimout.BulkCopy(dtOld, tableName);
						}
					}
				}
				//Verify that the old database and the new backup have the same number of rows
				if (doVerify)
				{
					List<string> listTablesFailed = new List<string>();
					foreach (DataRow dbTable in table.Rows)
					{
						string tableName = dbTable[0].ToString();
						MiscDataEvent.Fire(ODEventType.MiscData, $"Verifying backup: {tableName}");
						int ctOld = PIn.Int(dcon.GetCount($"SELECT COUNT(*) FROM `{oldDb}`.`{tableName}`"));
						int ctNew = PIn.Int(dconBackupServerNoTimout.GetCount($"SELECT COUNT(*) FROM `{newDb}`.`{tableName}`"));
						if (ctOld != ctNew)
						{
							listTablesFailed.Add(tableName);
						}
					}
					if (listTablesFailed.Count > 0)
					{
						throw new Exception($@"Failed to create database backup because the following tables contained a different number of rows than expected: 
							{string.Join(", ", listTablesFailed)}.");
					}
				}
			}
			finally
			{
				DataConnection.CommandTimeout = 3600;//Set back to default of 1 hour.
			}
		}

		public static string GetCurrentDatabase()
		{
			string command = "SELECT database()";
			DataTable table = Db.GetTable(command);
			return PIn.String(table.Rows[0][0].ToString());
		}

		/// <summary>
		/// Returns the name of the archive database based on the current connection settings.
		/// E.g. if the current database that is connected is called 'opendental182' then the archive name will be 'opendental182_archive'
		/// </summary>
		public static string GetArchiveDatabaseName()
		{
			//No need to check RemotingRole; no call to db.
			return GetCurrentDatabase() + "_archive";
		}

		/// <summary>
		/// Returns the major and minor version of MySQL for the current connection. 
		/// Returns a version of 0.0 if the MySQL version cannot be determined.
		/// </summary>
		public static string GetMySqlVersion()
		{
			string command = "SELECT @@version";
			DataTable table = Db.GetTable(command);
			string version = PIn.String(table.Rows[0][0].ToString());
			string[] arrayVersion = version.Split('.');
			try
			{
				return int.Parse(arrayVersion[0]) + "." + int.Parse(arrayVersion[1]);
			}
			catch
			{
			}
			return "0.0";
		}

		/// <summary>
		/// Gets the human readable host name of the database server, even when using the middle-tier. 
		/// This will return an empty string if Dns lookup fails.
		/// </summary>
		public static string GetODServer()
		{
			//string command="SELECT @@hostname";//This command fails in MySQL 5.0.22 (the version of MySQL 5.0 we used to use), because the hostname variable was added in MySQL 5.0.38.
			//string rawHostName=DataConnection.GetServerName();//This could be a human readable name, or it might be "localhost" or "127.0.0.1" or another IP address.
			//return Dns.GetHostEntry(rawHostName).HostName;//Return the human readable name (full domain name) corresponding to the rawHostName.
			//Had to strip off the port, caused Dns.GetHostEntry to fail and is not needed to get the hostname
			string rawHostName = DataConnection.GetServerName();
			if (rawHostName != null)
			{//rawHostName will be null if the user used a custom ConnectionString when they chose their database.
				rawHostName = rawHostName.Split(':')[0];//This could be a human readable name, or it might be "localhost" or "127.0.0.1" or another IP address.
			}
			string retval = "";
			try
			{
				retval = Dns.GetHostEntry(rawHostName).HostName;//Return the human readable name (full domain name) corresponding to the rawHostName.
			}
			catch (Exception ex)
			{
				ex.DoNothing();
			}
			return retval;
		}

		///<summary>Returns the current value in the GLOBAL max_allowed_packet variable.
		///max_allowed_packet is stored as an integer in multiples of 1,024 with a min value of 1,024 and a max value of 1,073,741,824.</summary>
		public static int GetMaxAllowedPacket()
		{
			int maxAllowedPacket = 0;
			//The SHOW command is used because it was able to run with a user that had no permissions whatsoever.
			string command = "SHOW GLOBAL VARIABLES WHERE Variable_name='max_allowed_packet'";
			DataTable table = Db.GetTable(command);
			if (table.Rows.Count > 0)
			{
				maxAllowedPacket = PIn.Int(table.Rows[0]["Value"].ToString());
			}
			return maxAllowedPacket;
		}

		///<summary>Sets the global MySQL variable max_allowed_packet to the passed in size (in bytes).
		///Returns the results of GetMaxAllowedPacket() after running the SET GLOBAL command.</summary>
		public static int SetMaxAllowedPacket(int sizeBytes)
		{
			//As of MySQL 5.0.84 the session level max_allowed_packet variable is read only so we only need to change the global.
			string command = "SET GLOBAL max_allowed_packet=" + POut.Int(sizeBytes);
			Db.NonQ(command);
			return GetMaxAllowedPacket();
		}

		///<summary>Returns a collection of unique AtoZ folders for the array of dbnames passed in.  It will not include the current AtoZ folder for this database, even if shared by another db.  This is used for the feature that updates multiple databases simultaneously.</summary>
		public static List<string> GetAtoZforDb(string[] dbNames)
		{
			List<string> retval = new List<string>();
			DataConnection dcon = null;
			string atozName;
			string atozThisDb = PrefC.GetString(PrefName.DocPath);
			for (int i = 0; i < dbNames.Length; i++)
			{
				try
				{
					dcon = new DataConnection(dbNames[i]);
					string command = "SELECT ValueString FROM preference WHERE PrefName='DocPath'";
					atozName = dcon.GetScalar(command);
					if (retval.Contains(atozName))
					{
						continue;
					}
					if (atozName == atozThisDb)
					{
						continue;
					}
					retval.Add(atozName);
				}
				catch
				{
					//don't add it to the list
				}
			}
			return retval;
		}

		public static void LockWorkstationsForDbs(string[] dbNames)
		{
			DataConnection dcon = null;
			for (int i = 0; i < dbNames.Length; i++)
			{
				try
				{
					dcon = new DataConnection(dbNames[i]);
					string command = "UPDATE preference SET ValueString ='" + POut.String(Environment.MachineName)
						+ "' WHERE PrefName='UpdateInProgressOnComputerName'";
					dcon.NonQ(command);
				}
				catch { }
			}
		}

		public static void UnlockWorkstationsForDbs(string[] dbNames)
		{
			DataConnection dcon = null;
			for (int i = 0; i < dbNames.Length; i++)
			{
				try
				{
					dcon = new DataConnection(dbNames[i]);
					string command = "UPDATE preference SET ValueString =''"
						+ " WHERE PrefName='UpdateInProgressOnComputerName'";
					dcon.NonQ(command);
				}
				catch { }
			}
		}

		public static void SetSqlMode()
		{
			try
			{
				if (PrefC.IsCloudMode)
				{
					return;
				}
			}
			catch (Exception ex)
			{
				ex.DoNothing();//This method might get called before the DatabaseMode preference is added.
			}

			//The SHOW command is used because it was able to run with a user that had no permissions whatsoever.
			string command = "SHOW GLOBAL VARIABLES WHERE Variable_name='sql_mode'";
			DataTable table = Db.GetTable(command);
			//We want to run the SET GLOBAL command when no rows were returned (above query failed) or if the sql_mode is not blank or NO_AUTO_CREATE_USER
			//(set to something that could cause errors).
			if (table.Rows.Count < 1 || (table.Rows[0]["Value"].ToString() != "" && table.Rows[0]["Value"].ToString().ToUpper() != "NO_AUTO_CREATE_USER"))
			{
				command = "SET GLOBAL sql_mode=''";//in case user did not use our my.ini file.  http://www.opendental.com/manual/mysqlservervariables.html
				Db.NonQ(command);
			}
		}
	}
}
