using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    public class InnoDb
	{
		/// <summary>
		/// Returns the default storage engine.
		/// </summary>
		public static string GetDefaultEngine() 
			=> Database.ExecuteScalar("SELECT @@default_storage_engine").ToString();

		public static bool IsInnodbAvail()
		{
			try
			{
				return Database.ExecuteScalar("SELECT @@have_innodb").ToString() == "YES";
			}
			catch // MySQL 5.6 and higher
			{
				var dataTable = Database.ExecuteDataTable("SHOW ENGINES");
				foreach (DataRow row in dataTable.Rows)
				{
					if (row["Engine"].ToString().ToLower() == "innodb" && 
						row["Support"].ToString().ToLower().In("yes", "default"))
					{
						return true;
					}
				}
				return false;
			}
		}

		/// <summary>
		/// Returns the number of MyISAM tables and the number of InnoDB tables in the current database.
		/// </summary>
		public static string GetEngineCount()
		{
			string command = @"SELECT SUM(CASE WHEN information_schema.tables.engine='MyISAM' THEN 1 ELSE 0 END) AS 'myisam',
				SUM(CASE WHEN information_schema.tables.engine='InnoDB' THEN 1 ELSE 0 END) AS 'innodb'
				FROM information_schema.tables
				WHERE table_schema=(SELECT DATABASE())";

			DataTable results = Database.ExecuteDataTable(command);
			string retval = "Number of MyISAM tables: ";
			retval += results.Rows[0]["myisam"].ToString() + "\r\n";
			retval += "Number of InnoDB tables: ";
			retval += results.Rows[0]["innodb"].ToString() + "\r\n";
			return retval;
		}

		/// <summary>
		/// Gets the names of tables in InnoDB format, comma delimited (excluding the 'phone' table).
		/// Returns empty string if none.
		/// </summary>
		public static string GetInnodbTableNames()
		{
			string command = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.tables "
				+ "WHERE TABLE_SCHEMA='" + SOut.String(DataConnection.DatabaseName) + "' "
				+ "AND TABLE_NAME!='phone' "//this table is used internally at OD HQ, and is always innodb.
				+ "AND ENGINE NOT LIKE 'MyISAM'";
			DataTable table = Database.ExecuteDataTable(command);
			string tableNames = "";
			for (int i = 0; i < table.Rows.Count; i++)
			{
				if (tableNames != "")
				{
					tableNames += ",";
				}
				tableNames += PIn.String(table.Rows[i][0].ToString());
			}
			return tableNames;
		}

		/// <summary>
		/// Returns true if the database has at least one table in InnoDB format.
		/// Default db is DataConnection.GetDatabaseName().
		/// </summary>
		public static bool HasInnoDbTables(string dbName = "") 
			=> Database.ExecuteDataTable(
				"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.tables " +
				"WHERE TABLE_SCHEMA='" + SOut.String(string.IsNullOrEmpty(dbName) ? DataConnection.DatabaseName : dbName) + "' " +
				"AND ENGINE NOT LIKE 'MyISAM'").Rows.Count > 1;

		/// <summary>
		/// The only allowed parameters are "InnoDB" or "MyISAM".
		/// Converts tables to toEngine type and returns the number of tables converted.
		/// </summary>
		public static int ConvertTables(string fromEngine, string toEngine)
		{
			int numtables = 0;
			string command = "SELECT DATABASE()";
			string database = Database.ExecuteString(command);
			command = @"SELECT table_name
				FROM information_schema.tables
				WHERE table_schema='" + POut.String(database) + "' AND information_schema.tables.engine='" + fromEngine + "'";
			DataTable results = Database.ExecuteDataTable(command);
			command = "";
			if (results.Rows.Count == 0)
			{
				return numtables;
			}
			for (int i = 0; i < results.Rows.Count; i++)
			{
				command += "ALTER TABLE `" + database + "`.`" + results.Rows[i]["table_name"].ToString() + "` ENGINE='" + toEngine + "'; ";
				numtables++;
			}
			Database.ExecuteNonQuery(command);
			return numtables;
		}
	}
}
