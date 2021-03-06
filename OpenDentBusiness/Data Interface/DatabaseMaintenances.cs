﻿using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDentBusiness
{
	public class DatabaseMaintenances
	{
		public static List<DatabaseMaintenance> GetAll()
		{
			string command = "SELECT * FROM databasemaintenance";
			return Crud.DatabaseMaintenanceCrud.SelectMany(command);
		}

		///<summary>Compares all DBM methods in the database to the entire list of methods passed in.</summary>
		public static void InsertMissingDBMs(List<string> listDbmMethods)
		{
			List<string> listDbmMethodNames = GetAll().Select(x => x.MethodName).ToList();
			foreach (string methodName in listDbmMethods)
			{
				if (listDbmMethodNames.Contains(methodName))
				{
					continue;
				}
				DatabaseMaintenance dbm = new DatabaseMaintenance();
				dbm.MethodName = methodName;
				Crud.DatabaseMaintenanceCrud.Insert(dbm);
			}
		}

		public static void Update(DatabaseMaintenance dbm)
		{
			Crud.DatabaseMaintenanceCrud.Update(dbm);
		}

		///<summary>Moves a DBM from the 'Checks' to the 'Old' tab by updating DatabaseMaintenance.IsOld=true</summary>
		public static void MoveToOld(string methodName)
		{
			string command = "UPDATE databasemaintenance SET IsOld=1 "
				+ "WHERE MethodName='" + POut.String(methodName) + "'";
			Database.ExecuteNonQuery(command);
		}

		///<summary>Updates the DateLastRun column to NOW for any DBM method that matches the method name passed in.</summary>
		public static void UpdateDateLastRun(string methodName)
		{
			string command = "UPDATE databasemaintenance SET DateLastRun=" + DbHelper.Now() + " "
				+ "WHERE MethodName='" + POut.String(methodName) + "'";
			Database.ExecuteNonQuery(command);
		}

		///<summary>Adds the logText to a centralized log file for the current day if the current data storage type is LocalAtoZ.
		///Throws exceptions to be displayed to the user.</summary>
		public static void SaveLogToFile(string logText)
		{
			string machineName = "~INVALID~";
			ODException.SwallowAnyException(() => { machineName = Environment.MachineName; });
			StringBuilder strB = new StringBuilder();
			strB.Append(DateTime.Now.ToString());
			strB.Append(" - Computer Name: " + machineName);
			strB.Append('-', 45);
			strB.AppendLine();//New line.
			strB.Append(logText);
			strB.AppendLine("Done");
			string path = ODFileUtils.CombinePaths(FileAtoZ.GetPreferredAtoZpath(), "DBMLogs");
			try
			{
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);//Create DBM Logs folder if it does not exist.
				}
				File.AppendAllText(ODFileUtils.CombinePaths(path, DateTime.Now.ToShortDateString().Replace("/", "_") + ".txt"), strB.ToString());//One file per date
			}
			catch (SecurityException se)
			{
				throw new ODException("Log not saved to DBM Logs folder because user does not have permission to access that file.", se);
			}
			catch (UnauthorizedAccessException uae)
			{
				throw new ODException("Log not saved to DBM Logs folder because user does not have permission to access that file.", uae);
			}
			//Throw all other types of exceptions like usual.
		}

		#region List of Tables and Columns for null check---------------------------------------------------------------------------------------------------
		///<summary>List of tables and columns to remove null characters from.
		///Loop through this list two items at a time because it is designed to have a table first which is then followed by a relative column.</summary>
		private static List<string> _listTableAndColumns = new List<string>() {
				//Table					//Column
				"adjustment",   "AdjNote",
				"appointment",  "Note",
				"commlog",      "Note",
				"definition",   "ItemName",
				"diseasedef",   "DiseaseName",
				"patient",      "Address",
				"patient",      "Address2",
				"patient",      "AddrNote",
				"patient",      "MedUrgNote",
				"patient",      "WirelessPhone",
				"patientnote",  "FamFinancial",
				"patientnote",  "Medical",
				"patientnote",  "MedicalComp",
				"patientnote",  "Service",
				"patientnote",  "Treatment",
				"payment",      "PayNote",
				"popup",        "Description",
				"procnote",     "Note",
				"securitylog",  "LogText",
			};
		#endregion List of Tables and Columns for null check------------------------------------------------------------------------------------------------

		#region Methods That Affect All or Many Tables------------------------------------------------------------------------------------------------------

		///<summary></summary>
		[DbmMethodAttr]
		public static string MySQLServerOptionsValidate(bool verbose, DbmMode modeCur)
		{
			string command = "SHOW GLOBAL VARIABLES LIKE 'sql_mode'";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count < 1 || table.Columns.Count < 2)
			{//user may not have permission to access global variables?
				return
					"Unable to access the MySQL server variable 'sql_mode', probably due to permissions. " +
					"The sql_mode must be blank or NO_AUTO_CREATE_USER.\r\n";
			}
			string sqlmode = table.Rows[0][1].ToString();
			string sqlmodeDisplay = (string.IsNullOrWhiteSpace(sqlmode) ? "blank" : sqlmode);//translated 'blank' for display
			if (string.IsNullOrWhiteSpace(sqlmode) || sqlmode.ToUpper() == "NO_AUTO_CREATE_USER")
			{
				if (!verbose)
				{
					//Nothing broken, not verbose, show ""
					return "";
				}
				else
				{
					//Nothing is broken, verbose on, show current sql_mode.
					return "The MySQL server variable 'sql_mode' is currently set to " + sqlmodeDisplay + ".\r\n";
				}
			}
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					log += "The MySQL server variable 'sql_mode' must be blank or NO_AUTO_CREATE_USER and is currently set to " + sqlmodeDisplay + ".\r\n";
					break;
				case DbmMode.Fix:
					try
					{
						command = "SET GLOBAL sql_mode=''";
						Database.ExecuteNonQuery(command);
						command = "SET SESSION sql_mode=''";
						Database.ExecuteNonQuery(command);
						log += "The MySQL server variable 'sql_mode' has been changed from " + sqlmodeDisplay + " to blank" + ".\r\n";
					}
					catch
					{
						log +=
							"Unable to set the MySQL server variable 'sql_mode', probably due to permissions. " +
							"The sql_mode must be blank or NO_AUTO_CREATE_USER and is currently set to " + sqlmodeDisplay + ".\r\n";
					}
					break;
			}//end switch
			return log;
		}

		/// <summary>
		/// Returns a Tuple with Item1=log string and Item2=whether the table checks were successful.
		/// </summary>
		public static (string, bool) MySQLTables(bool verbose)
		{
			string log = "";
			bool success = true;
			if (Preferences.GetBool(PreferenceName.DatabaseMaintenanceSkipCheckTable))
			{
				return ("", success);
			}
			string command = "DROP TABLE IF EXISTS `signal`";//Signal is keyword for MySQL 5.5.  Was renamed to signalod so drop if exists.
			Database.ExecuteNonQuery(command);
			command = "SHOW FULL TABLES WHERE Table_type='BASE TABLE'";//Tables, not views.  Does not work in MySQL 4.1, however we test for MySQL version >= 5.0 in PrefL.
			DataTable table = Database.ExecuteDataTable(command);
			string[] tableNames = new string[table.Rows.Count];
			int lastRow;
			bool existsCorruptFiles = false;
			bool existsUnvalidatedTables = false;

			for (int i = 0; i < table.Rows.Count; i++)
			{
				tableNames[i] = table.Rows[i][0].ToString();
			}

			string checkingTable = "Checking table";
			for (int i = 0; i < tableNames.Length; i++)
			{
				//Alert the thread we are running this on that we are checking this table.
				DatabaseMaintEvent.Fire(EventCategory.DatabaseMaint, checkingTable + ": " + tableNames[i]);
				command = "CHECK TABLE `" + tableNames[i] + "`";
				try
				{
					table = Database.ExecuteDataTable(command);
					lastRow = table.Rows.Count - 1;
					string status = PIn.ByteArray(table.Rows[lastRow][3]);
					if (status != "OK")
					{
						log += "Corrupt file found for table " + tableNames[i] + "\r\n";
						existsCorruptFiles = true;
					}
				}
				catch (Exception ex)
				{
					log += "Unable to validate table " + tableNames[i] + "\r\n" + ex.Message + "\r\n";
					existsUnvalidatedTables = true;
				}
			}

			if (existsUnvalidatedTables)
			{
				success = false; // no other checks should be done until we can successfully get past this.
				log += "Tables found that could not be validated.\r\nDone.";
			}

			if (existsCorruptFiles)
			{
				success = false; // no other checks should be done until we can successfully get past this.
				log += "Corrupted database files found, please call support immediately or see manual for more details.\r\nDone.";
			}

			if (!existsUnvalidatedTables && !existsCorruptFiles)
			{
				if (verbose)
				{
					log += "Tables validated successfully. No corrupted tables.\r\n";
				}
			}

			return (log, success);
		}

		///<summary>If using MySQL, tries to repair and then optimize each table.
		///Developers must make a backup prior to calling this method because repairs have a tendency to delete data.
		///Currently called whenever MySQL is upgraded and when users click Optimize in database maintenance.</summary>
		public static string RepairAndOptimize(bool isLogged = false)
		{
			StringBuilder retVal = new StringBuilder();
			DataTable tableLog = null;
			string command = "SHOW FULL TABLES WHERE Table_type='BASE TABLE';";//Tables, not views.  Does not work in MySQL 4.1, however we test for MySQL version >= 5.0 in PrefL.
			DataTable table = Database.ExecuteDataTable(command);
			string[] tableNames = new string[table.Rows.Count];
			for (int i = 0; i < table.Rows.Count; i++)
			{
				tableNames[i] = table.Rows[i][0].ToString();
			}
			for (int i = 0; i < tableNames.Length; i++)
			{
				//Alert anyone that cares that we are optimizing this table.
				MiscDataEvent.Fire(EventCategory.MiscData, "Optimizing table: " + tableNames[i]);
				string optimizeResult = OptimizeTable(tableNames[i], isLogged);
				if (isLogged)
				{
					retVal.AppendLine(optimizeResult);
				}
			}
			for (int i = 0; i < tableNames.Length; i++)
			{
				//Alert anyone that cares that we are repairing this table.
				MiscDataEvent.Fire(EventCategory.MiscData, "Repairing table: " + tableNames[i]);
				command = "REPAIR TABLE `" + tableNames[i] + "`";
				if (!isLogged)
				{
					Database.ExecuteNonQuery(command);
				}
				else
				{
					tableLog = Database.ExecuteDataTable(command);
					for (int r = 0; r < tableLog.Rows.Count; r++)
					{//usually only 1 row, unless something abnormal is found.
						retVal.AppendLine(tableLog.Rows[r]["Table"].ToString().PadRight(50, ' ')
							+ " | " + tableLog.Rows[r]["Op"].ToString()
							+ " | " + tableLog.Rows[r]["Msg_type"].ToString()
							+ " | " + tableLog.Rows[r]["Msg_text"].ToString());
					}
				}
			}
			return retVal.ToString();
		}

		///<summary>Optimizes the table passed in.  Set hasResult to true to return a string representation of the query results.
		///Does not attempt the optimize if random PKs is turned on, or if the table is of storage engine InnoDB (unless canOptimizeInnodb is true).
		///See wiki page [[Database Storage Engine Comparison: InnoDB vs MyISAM]] for reasons why.
		///As documented online, https://dev.mysql.com/doc/refman/5.6/en/optimize-table.html, MySQL 5.6 supports the optimize
		///command for MyISAM, InnoDB and Archive tables only.</summary>
		public static string OptimizeTable(string tableName, bool hasResult = false, bool canOptimizeInnodb = false)
		{
			string retVal = "";
			string command;
			if (!canOptimizeInnodb)
			{
				//Check to see if the table has its storage engine set to InnoDB.
				command = "SELECT ENGINE FROM information_schema.TABLES "
					+ "WHERE TABLE_SCHEMA='" + POut.String(DataConnection.DatabaseName) + "' "
					+ "AND TABLE_NAME='" + POut.String(tableName) + "' ";
				string storageEngine = Database.ExecuteString(command);
				if (storageEngine.ToLower() == "innodb")
				{
					retVal = tableName + " skipped due to using the InnoDB storage engine.";
				}
			}
			//Only run OPTIMIZE if random PKs are not used and the table is not using the InnoDB storage engine.
			if (retVal == "")
			{
				DataConnection.CommandTimeout = 43200;//12 hours, because conversion commands may take longer to run.
				command = "OPTIMIZE TABLE `" + tableName + "`";//Ticks used in case user has added custom tables with unusual characters.
				try
				{
					DataTable tableLog = Database.ExecuteDataTable(command);
					if (hasResult)
					{
						//Loop through any rows returned and return the results.  Often times will only be one row unless there was a problem with optimizing.
						foreach (DataRow row in tableLog.Rows)
						{
							retVal += 
								row["Table"].ToString().PadRight(50, ' ') + " | " + 
								row["Op"].ToString() + " | " + 
								row["Msg_type"].ToString() + " | " + 
								row["Msg_text"].ToString();
						}
					}
				}
				finally
				{
					DataConnection.CommandTimeout = 3600;//Set back to default of 1 hour.
				}
			}
			return retVal;
		}

		///<summary>also checks patient.AddrNote</summary>
		[DbmMethodAttr]
		public static string SpecialCharactersInNotes(bool verbose, DbmMode modeCur)
		{
			string log = "";
			//this will run for fix or check, but will only fix if the special char button is used 
			//Fix code is in a dedicated button "Spec Char"
			string command = "SELECT * FROM appointment WHERE (ProcDescript REGEXP '[^[:alnum:]^[:space:]^[:punct:]]+') OR (Note REGEXP '[^[:alnum:]^[:space:]^[:punct:]]+')";
			List<Appointment> apts = Crud.AppointmentCrud.SelectMany(command);
			List<char> specialCharsFound = new List<char>();
			int specialCharCount = 0;
			int intC;
			foreach (Appointment apt in apts)
			{
				foreach (char c in apt.Note)
				{
					intC = c;
					if ((intC < 126 && intC > 31)//31 - 126 are all safe.
						|| intC == 9     //"Horizontal Tabulation"
						|| intC == 10    //Line Feed
						|| intC == 13)
					{ //carriage return
						continue;
					}
					specialCharCount++;
					if (specialCharsFound.Contains(c))
					{
						continue;
					}
					specialCharsFound.Add(c);
				}
				foreach (char c in apt.ProcDescript)
				{//search every character in ProcDescript
					intC = c;
					if ((intC < 126 && intC > 31)//31 - 126 are all safe.
						|| intC == 9     //"Horizontal Tabulation"
						|| intC == 10    //Line Feed
						|| intC == 13)
					{ //carriage return
						continue;
					}
					specialCharCount++;
					if (specialCharsFound.Contains(c))
					{
						continue;
					}
					specialCharsFound.Add(c);
				}
			}
			command = "SELECT * FROM patient WHERE AddrNote REGEXP '[^[:alnum:]^[:space:]]+'";
			List<Patient> pats = Crud.PatientCrud.SelectMany(command);

			foreach (Patient pat in pats)
			{
				foreach (char c in pat.AddrNote)
				{
					intC = c;
					if ((intC < 126 && intC > 31)//31 - 126 are all safe.
						|| intC == 9      //"Horizontal Tabulation"
						|| intC == 10     //Line Feed
						|| intC == 13)
					{  //carriage return
						continue;
					}
					specialCharCount++;
					if (specialCharsFound.Contains(c))
					{
						continue;
					}
					specialCharsFound.Add(c);
				}
			}
			foreach (char c in specialCharsFound)
			{
				log += c.ToString() + " doesn't work.\r\n";
			}
			for (int i = 0; i < _listTableAndColumns.Count; i += 2)
			{
				string tableName = _listTableAndColumns[i];
				string columnName = _listTableAndColumns[i + 1];
				command = "SELECT COUNT(*) FROM " + tableName + " WHERE " + columnName + " LIKE '%" + POut.String("\0") + "%'";
				specialCharCount += PIn.Int(Database.ExecuteString(command));
			}
			if (specialCharCount != 0 || verbose)
			{
				log += specialCharCount.ToString() + " " + "total special characters found. The Spec Char tool will remove these characters.\r\n";
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string NotesWithTooMuchWhiteSpace(bool verbose, DbmMode modeCur)
		{
			string command;
			string logBreakdown = "";
			long countTotal = 0;
			string tooManyT = string.Join("", Enumerable.Repeat("\t", 30));//Can't think of any good reason to have more than 30 tabs in a row
			string tooManySP = string.Join("", Enumerable.Repeat(@" ", 300));//Spaces are very easy to draw so only remove ridiculous amounts of them.
			string tooManyRN = string.Join("", Enumerable.Repeat("\r\n", 30));// \r\n, \r\n, \r\n... as fast as you can!
			string tooManyN = string.Join("", Enumerable.Repeat("\n", 30));// Sometimes we have had newlines encoded as \n
																		   //Create an array of custom objects to easily see which tables, columns, and primary keys will be considered in this method.
			var listTablesAndColumns = new[] {
				new { tableName="appointment",columnName="Note",key="AptNum" },
				new { tableName="commlog",columnName="Note",key="CommlogNum" },
				new { tableName="procnote",columnName="Note",key="ProcNoteNum" },
				new { tableName="patient",columnName="FamFinUrgNote",key="PatNum" },
				new { tableName="patfield",columnName="FieldValue",key="PatFieldNum" }
			};
			//Loop through the custom object array looking for rows in the database that have too many tabs, newlines, or trailing spaces.
			for (int i = 0; i < listTablesAndColumns.Length; i++)
			{
				string tableName = listTablesAndColumns[i].tableName;
				string colName = listTablesAndColumns[i].columnName;
				string priKey = listTablesAndColumns[i].key;
				#region Tabs
				switch (modeCur)
				{
					case DbmMode.Breakdown:
					case DbmMode.Check:
						command = "SELECT COUNT(*) FROM " + POut.String(tableName) + " WHERE " + POut.String(colName) + " LIKE '%" + POut.String(tooManyT) + "%'";
						int countLocal = PIn.Int(Database.ExecuteString(command));
						countTotal += countLocal;
						if (modeCur == DbmMode.Breakdown)
						{
							logBreakdown += POut.String(tableName) + "." + POut.String(colName) + " " + "rows with too many tabs found:"
								+ " " + countLocal + "\r\n";
						}
						break;
					case DbmMode.Fix:
						command = "SELECT " + priKey + "," + colName + " FROM " + POut.String(tableName) + " WHERE " + POut.String(colName) + " LIKE '%" + POut.String(tooManyT) + "%'";
						DataTable resultTable = Database.ExecuteDataTable(command);
						if (resultTable.Rows.Count > 0 || verbose)
						{
							for (int j = 0; j < resultTable.Rows.Count; j++)
							{
								long id = PIn.Long(resultTable.Rows[j][priKey].ToString());
								string oldNote = PIn.String(resultTable.Rows[j][colName].ToString());
								string newNote = Regex.Replace(oldNote, POut.String(tooManyT) + "[\t]*", "");
								command = "UPDATE " + POut.String(tableName) + " SET " + POut.String(colName) + "='" + POut.String(newNote) + "' WHERE " + POut.String(priKey) + "=" + POut.Long(id);
								Database.ExecuteNonQuery(command);
								countTotal++;
							}
						}
						break;
				}
				#endregion
				#region Newlines
				switch (modeCur)
				{
					case DbmMode.Breakdown:
					case DbmMode.Check:
						command = "SELECT COUNT(*) FROM " + POut.String(tableName) + " "
							+ "WHERE " + POut.String(colName) + " LIKE '%" + POut.String(tooManyRN) + "%' "
							+ "OR " + POut.String(colName) + " LIKE '%" + POut.String(tooManyN) + "%'";
						int countLocal = PIn.Int(Database.ExecuteString(command));
						countTotal += countLocal;
						if (modeCur == DbmMode.Breakdown)
						{
							logBreakdown += POut.String(tableName) + "." + POut.String(colName) + " " + "rows with too many newlines found:"
								+ " " + countLocal + "\r\n";
						}
						break;
					case DbmMode.Fix:
						command = "SELECT " + priKey + "," + colName + " FROM " + POut.String(tableName) + " "
							+ "WHERE " + POut.String(colName) + " LIKE '%" + POut.String(tooManyRN) + "%' "
							+ "OR " + POut.String(colName) + " LIKE '%" + POut.String(tooManyN) + "%'";
						DataTable resultTable = Database.ExecuteDataTable(command);
						if (resultTable.Rows.Count > 0 || verbose)
						{
							for (int j = 0; j < resultTable.Rows.Count; j++)
							{
								long id = PIn.Long(resultTable.Rows[j][priKey].ToString());
								string oldNote = PIn.String(resultTable.Rows[j][colName].ToString());
								string newNote = Regex.Replace(oldNote, POut.String(tooManyRN) + "[\r\n]*", "\r\n");
								newNote = Regex.Replace(newNote, POut.String(tooManyN) + "[\n]*", "\r\n");
								command = "UPDATE " + POut.String(tableName) + " SET " + POut.String(colName) + "='" + POut.String(newNote) + "' WHERE " + POut.String(priKey) + "=" + POut.Long(id);
								Database.ExecuteNonQuery(command);
								countTotal++;
							}
						}
						break;
				}
				#endregion
				#region Trailing Spaces
				switch (modeCur)
				{
					case DbmMode.Breakdown:
					case DbmMode.Check:
						command = @"SELECT COUNT(*) FROM " + POut.String(tableName) + " "
							+ @"WHERE " + POut.String(colName) + " LIKE '%" + POut.String(tooManySP) + "%' ";//This is Sparta!
						int countLocal = PIn.Int(Database.ExecuteString(command));
						countTotal += countLocal;
						if (modeCur == DbmMode.Breakdown)
						{
							logBreakdown += POut.String(tableName) + "." + POut.String(colName) + " " + "rows with too many trailing white spaces found:"
								+ " " + countLocal + "\r\n";
						}
						break;
					case DbmMode.Fix:
						command = "SELECT " + priKey + "," + colName + " FROM " + POut.String(tableName) + " "
							+ "WHERE " + POut.String(colName) + " LIKE '%" + POut.String(tooManySP) + "%' ";
						DataTable resultTable = Database.ExecuteDataTable(command);
						if (resultTable.Rows.Count > 0 || verbose)
						{
							for (int j = 0; j < resultTable.Rows.Count; j++)
							{
								long id = PIn.Long(resultTable.Rows[j][priKey].ToString());
								string oldNote = PIn.String(resultTable.Rows[j][colName].ToString());
								string newNote = Regex.Replace(oldNote, POut.String(tooManySP) + "[ ]*", "");
								command = "UPDATE " + POut.String(tableName) + " SET " + POut.String(colName) + "='" + POut.String(newNote) + "' WHERE " + POut.String(priKey) + "=" + POut.Long(id);
								Database.ExecuteNonQuery(command);
								countTotal++;
							}
						}
						break;
				}
				#endregion
			}
			string log = "";
			if (countTotal > 0 || verbose)
			{
				switch (modeCur)
				{
					case DbmMode.Breakdown:
						log = logBreakdown;
						break;
					case DbmMode.Check:
						log = "Notes with too much white space found: " + countTotal.ToString() + "\r\n";
						log += "   Double click to see a break down.\r\n";
						break;
					case DbmMode.Fix:
						log = "Notes with too much white space fixed: " + countTotal.ToString() + "\r\n";
						break;
				}
			}
			return log;
		}

		[DbmMethodAttr]
		public static string TablesWithClinicNumNegative(bool verbose, DbmMode modeCur)
		{
			List<string> listTablesToCheck = new List<string> {
				//"alertitem",//Not including this because we intentionally set ClinicNum to -1 to indicate the alert's for all clinics.
				"appointment",
				"claim",
				"claimproc",
				"histappointment",
				"patient",
				"procedurelog",
				"smstomobile",
			};
			string command = "SELECT CountInvalid,TableName FROM (\r\n" + string.Join("\r\nUNION ALL\r\n", listTablesToCheck.Select(x =>
						 $"SELECT COUNT(*) CountInvalid,'{x}' TableName FROM {x} WHERE ClinicNum < 0")) + "\r\n" +
				") invalid WHERE CountInvalid > 0";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Total rows found with invalid ClinicNums: " + table.Select().Sum(x => x.GetLong("CountInvalid"))
							+ "\r\n  " + string.Join("\r\n  ", table.Select().Select(x => x.GetString("TableName") + ": " + x.GetString("CountInvalid")));
					}
					break;
				case DbmMode.Fix:
					if (table.Rows.Count > 0)
					{
						command = string.Join(";\r\n", listTablesToCheck.Select(x => $"UPDATE {x} SET ClinicNum=0 WHERE ClinicNum < 0"));
						Database.ExecuteNonQuery(command);
					}
					int numberFixed = table.Rows.Count;
					if (numberFixed != 0 || verbose)
					{
						log += "Total rows fixed with invalid ClinicNums: " + table.Select().Sum(x => x.GetLong("CountInvalid"))
							+ "\r\n  " + string.Join("\r\n  ", table.Select().Select(x => x.GetString("TableName") + ": " + x.GetString("CountInvalid")));
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string TransactionsWithFutureDates(bool verbose, DbmMode modeCur)
		{
			string log = "";
			bool isFutureTransAllowed = Preferences.GetBool(PreferenceName.FutureTransDatesAllowed);
			bool isFuturePaymentsAllowed = Preferences.GetBool(PreferenceName.AccountAllowFutureDebits);
			if (isFutureTransAllowed)
			{//future dates are allowed so this DBM doesn't apply.
				return log;
			}
			string command = Ledgers.GetTransQueryString(DateTime.Today, "");
			DataTable table = Database.ExecuteDataTable(command);
			DataTable flaggedTransactions = table.Clone();
			foreach (DataRow row in table.Rows)
			{
				if (PIn.String(row["TranType"].ToString()) != "PPCharge" && PIn.String(row["TranType"].ToString()) != "PPCComplete"
					&& PIn.Date(row["TranDate"].ToString()) > DateTime.Today.Date)
				{//transaction is date for the future
				 //if either future dated payments or future transactions are allowed, don't count transactions dealing with payments.
					if (PIn.String(row["TranType"].ToString()) == "PatPay" && isFuturePaymentsAllowed)
					{
						continue; //the are allowing future payments so skip this row. 
					}
					flaggedTransactions.Rows.Add(row.ItemArray);
				}
			}
			log += "Future dated transactions found: " + flaggedTransactions.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (flaggedTransactions.Rows.Count > 0 || verbose)
					{
						log += "\r\nManual fix needed. Double click to see a break down.";
					}
					break;
				case DbmMode.Breakdown:
					if (flaggedTransactions.Rows.Count > 0)
					{
						log += ", including" + ":\r\n";
						foreach (DataRow transaction in flaggedTransactions.Rows)
						{
							string tranType = "";
							switch (PIn.String(transaction["TranType"].ToString()))
							{
								case "Adj":
									tranType = "Adjustment";
									break;
								case "Claimproc":
									tranType = "Claim procedure";
									break;
								case "PatPay":
									tranType = "Patient payment";//paysplit?
									break;
								case "Proc":
									tranType = "Procedure";
									break;
							}
							log += "\r\n   " + tranType + " " + "found for patient #" + PIn.Long(transaction["PatNum"].ToString()) + " "
								+ "dated" + " " + PIn.Date(transaction["TranDate"].ToString()).ToShortDateString() + " "
								+ "amounting to" + " " + PIn.Double(transaction["TranAmount"].ToString()).ToString("c");
						}
						log += "\r\nGo to patient accounts to find and manually correct future dates.";
					}
					break;
			}
			return log;
		}

		#endregion Methods That Affect All or Many Tables---------------------------------------------------------------------------------------------------
		#region Methods That Apply to Specific Tables-------------------------------------------------------------------------------------------------------

		#region Appointment-----------------------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr(HasBreakDown = true)]
		public static string AppointmentCompleteWithTpAttached(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT DISTINCT appointment.PatNum, " + DbHelper.Concat("LName", "\", \"", "FName") + " AS PatName, AptDateTime "
				+ "FROM appointment "
				+ "INNER JOIN patient ON patient.PatNum=appointment.PatNum "
				+ "INNER JOIN procedurelog ON procedurelog.AptNum=appointment.AptNum "
				+ "WHERE AptStatus=" + POut.Int((int)ApptStatus.Complete) + " "
				+ "AND procedurelog.ProcStatus=" + POut.Int((int)ProcStat.TP) + " "
				+ "ORDER BY PatName";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			//There is something to report OR the user has verbose mode on.
			string log = "Completed appointments with treatment planned procedures attached: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   Manual fix needed.  Double click to see a break down.\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", including" + ":\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "   " + table.Rows[i]["PatNum"].ToString()
								+ "-" + table.Rows[i]["PatName"].ToString()
								+ "  Appt Date:" + PIn.Date(table.Rows[i]["AptDateTime"].ToString()).ToShortDateString();
							log += "\r\n";
						}
						log += "   They need to be fixed manually.\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string AppointmentsEndingOnDifferentDay(bool verbose, DbmMode modeCur)
		{
			//This pulls appointments that extend past the end of the day. CHAR_LENGTH(Pattern)-1 as if the appointment goes to midnight, it does
			//not need to be fixed.
			string command = "SELECT AptNum FROM appointment WHERE DATE(AptDateTime) != DATE(AptDateTime + INTERVAL (CHAR_LENGTH(Pattern)-1)*5 MINUTE)";
			List<long> listAptNums = Database.GetListLong(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (listAptNums.Count > 0 || verbose)
					{
						log += "Appointments found that span over multiple days: " + listAptNums.Count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					if (listAptNums.Count > 0)
					{
						//We're going to truncate the appointment to end at midnight. We will grab the substring of the pattern by calculating the
						//number of 5 minute increments between the aptDateTime and midnight of the next day
						command = "UPDATE appointment SET Pattern = SUBSTRING(Pattern,1,TIMESTAMPDIFF(MINUTE,AptDateTime,DATE(AptDateTime) "
							+ "+ INTERVAL 1 DAY)/5) WHERE AptNum IN(" + string.Join(",", listAptNums) + ")";
						Database.ExecuteNonQuery(command);
					}
					int numberFixed = listAptNums.Count;
					if (numberFixed != 0 || verbose)
					{
						log += "Appointments shortened: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string AppointmentsNoPattern(bool verbose, DbmMode modeCur)
		{
			string command = @"SELECT AptNum FROM appointment WHERE Pattern=''";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Appointments found with zero length: " + table.Rows.Count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					if (table.Rows.Count > 0)
					{
						//detach all procedures
						List<Procedure> listProceduresModified;
						List<DbmLog> listDbmLogs = new List<DbmLog>();
						string methodName = MethodBase.GetCurrentMethod().Name;
						string where = "WHERE P.AptNum=A.AptNum AND A.Pattern=''";
						command = "SELECT P.* FROM procedurelog P,appointment A " + where;
						listProceduresModified = Crud.ProcedureCrud.SelectMany(command);
						command = "UPDATE procedurelog P, appointment A SET P.AptNum=0 " + where;
						Database.ExecuteNonQuery(command);
						//Add changes to listDbmLogs
						listProceduresModified.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ProcNum, DbmLogFKeyType.Procedure,
							DbmLogActionType.Update, methodName, "Updated AptNum from " + x.AptNum + " to 0 from AppointmentsNoPattern")));
						where = "WHERE P.PlannedAptNum=A.AptNum AND A.Pattern=''";
						command = "SELECT P.* FROM procedurelog P,appointment A " + where;
						listProceduresModified = Crud.ProcedureCrud.SelectMany(command);
						command = "UPDATE procedurelog P, appointment A SET P.PlannedAptNum=0 " + where;
						Database.ExecuteNonQuery(command);
						//Add changes to listDbmLogs
						listProceduresModified.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ProcNum, DbmLogFKeyType.Procedure,
							DbmLogActionType.Update, methodName, "Updated PlannedAptNum from " + x.PlannedAptNum + " to 0 from AppointmentsNoPattern")));
						command = "SELECT appointment.AptNum FROM appointment WHERE Pattern=''";
						DataTable tableAptNums = Database.ExecuteDataTable(command);
						List<long> listAptNums = new List<long>();
						for (int i = 0; i < tableAptNums.Rows.Count; i++)
						{
							listAptNums.Add(PIn.Long(tableAptNums.Rows[i]["AptNum"].ToString()));
						}
						if (listAptNums.Count > 0)
						{
							//List<Permissions> listPerms = GroupPermissions.GetPermsFromCrudAuditPerm(CrudTableAttribute.GetCrudAuditPermForClass(typeof(Appointment)));
							//List<SecurityLog> listSecurityLogs = SecurityLogs.GetFromFKeysAndType(listAptNums, listPerms);
							Appointments.ClearFkey(listAptNums);//Zero securitylog FKey column for rows to be deleted.
																//Add securitylog changes to listDbmLogs
							//listSecurityLogs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.SecurityLogNum, DbmLogFKeyType.Securitylog,
							//	DbmLogActionType.Delete, methodName, "Updated FKey from " + x.FKey + " to 0.")));
							listAptNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Appointment,
								DbmLogActionType.Delete, methodName, "Deleted appointment with no pattern")));
							command = "SELECT * FROM appointment WHERE AptNum IN(" + String.Join(",", listAptNums) + ")";
							List<Appointment> listApts = Crud.AppointmentCrud.SelectMany(command);
							foreach (Appointment apt in listApts)
							{
								HistAppointment hist = HistAppointments.CreateHistoryEntry(apt, HistAppointmentAction.Deleted);
								if (hist == null)
								{
									continue;
								}
								listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, hist.HistApptNum, DbmLogFKeyType.HistAppointment, DbmLogActionType.Insert,
									methodName, "Inserted hist appointment."));
							}
						}
						command = "DELETE FROM appointment WHERE Pattern=''";
						Database.ExecuteNonQuery(command);
						DbmLogs.InsertMany(listDbmLogs);
					}
					int numberFixed = table.Rows.Count;
					if (numberFixed != 0 || verbose)
					{
						log += "Appointments deleted with zero length: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string AppointmentsNoDateOrProcs(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM appointment "
						+ "WHERE AptStatus=1 "//scheduled 
						+ "AND " + DbHelper.Year("AptDateTime") + "<1880 "//scheduled but no date 
						+ "AND NOT EXISTS(SELECT * FROM procedurelog WHERE procedurelog.AptNum=appointment.AptNum)";//and no procs
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound != 0 || verbose)
					{
						log += "Appointments found with no date and no procs: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "SELECT appointment.AptNum FROM appointment "
						+ "WHERE AptStatus=" + POut.Int((int)ApptStatus.Scheduled) + " "
						+ "AND " + DbHelper.Year("AptDateTime") + "<1880 "//scheduled but no date 
						+ "AND NOT EXISTS(SELECT * FROM procedurelog WHERE procedurelog.AptNum=appointment.AptNum)";//and no procs
					DataTable tableAptNums = Database.ExecuteDataTable(command);
					List<long> listAptNums = new List<long>();
					for (int i = 0; i < tableAptNums.Rows.Count; i++)
					{
						listAptNums.Add(PIn.Long(tableAptNums.Rows[i]["AptNum"].ToString()));
					}
					if (listAptNums.Count > 0)
					{
						//List<Permissions> listPerms = GroupPermissions.GetPermsFromCrudAuditPerm(CrudTableAttribute.GetCrudAuditPermForClass(typeof(Appointment)));
						//List<SecurityLog> listSecurityLogs = SecurityLogs.GetFromFKeysAndType(listAptNums, listPerms);
						Appointments.ClearFkey(listAptNums);//Zero securitylog FKey column for rows to be deleted.
															//Add changes to listDbmLogs
						listAptNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Appointment,
								DbmLogActionType.Delete, methodName, "Deleted appointment from AppointmentsNoDateOrProcs.")));
						//listSecurityLogs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.SecurityLogNum, DbmLogFKeyType.Securitylog,
						//		 DbmLogActionType.Update, methodName, "Cleared securitylog FKey column.")));
						command = "SELECT * FROM appointment WHERE AptNum IN(" + String.Join(",", listAptNums) + ")";
						List<Appointment> listApts = Crud.AppointmentCrud.SelectMany(command);
						foreach (Appointment apt in listApts)
						{
							HistAppointment hist = HistAppointments.CreateHistoryEntry(apt, HistAppointmentAction.Deleted);
							if (hist == null)
							{
								continue;
							}
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, hist.HistApptNum, DbmLogFKeyType.HistAppointment, DbmLogActionType.Insert,
								methodName, "Inserted hist appointment."));
						}
					}
					command = "DELETE FROM appointment "
						+ "WHERE AptStatus=" + POut.Int((int)ApptStatus.Scheduled) + " "
						+ "AND " + DbHelper.Year("AptDateTime") + "<1880 "//scheduled but no date 
						+ "AND NOT EXISTS(SELECT * FROM procedurelog WHERE procedurelog.AptNum=appointment.AptNum)";//and no procs
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed != 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Appointments deleted due to no date and no procs: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string AppointmentsNoPatients(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT Count(*) FROM appointment WHERE PatNum NOT IN (SELECT PatNum FROM patient)";
			int count = PIn.Int(Database.ExecuteString(command));
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (count > 0 || verbose)
					{
						log += "Appointments found with invalid patients: " + count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					//Fix is safe because we are not deleting data, we are just attaching abandoned appointments to a dummy patient.
					long patientsAdded = 0;
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					if (count != 0)
					{
						command = "SELECT PatNum FROM appointment WHERE PatNum NOT IN (SELECT PatNum FROM patient)";
						List<long> patNums = Database.GetListLong(command).Distinct().ToList();
						if (patNums.Contains(0))
						{//appointments with no patient.
							Patient tempPat = new Patient()
							{
								FName = "MISSING",
								LName = "PATIENT",
								AddrNote = "DBM created this patient and assigned patientless appointments to it on " + DateTime.Now.ToShortDateString(),
								Birthdate = DateTime.MinValue,
								BillingType = Preferences.GetLong(PreferenceName.PracticeDefaultBillType),
								PatStatus = PatientStatus.Inactive,
								PriProv = Preferences.GetLong(PreferenceName.PracticeDefaultProv)
							};
							//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
							tempPat.SecUserNumEntry = Security.CurrentUser.Id;
							Patients.Insert(tempPat, false);
							SecurityLogs.MakeLogEntry(Permissions.PatientCreate, tempPat.PatNum, "Recreated from DBM fix for AppointmentsNoPatients.", SecurityLogSource.DBM);
							Patient oldPat = tempPat.Copy();
							tempPat.Guarantor = tempPat.PatNum;
							Patients.Update(tempPat, oldPat);//update guarantor
															 //Add new patient to listDbmLogs
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, tempPat.PatNum, DbmLogFKeyType.Patient, DbmLogActionType.Insert,
								methodName, "Inserted patient from AppointmentsNoPatients."));
							command = "SELECT * FROM appointment WHERE PatNum=0";
							//add the updated appointments to listDbmLogs
							List<Appointment> listAppts = Crud.AppointmentCrud.SelectMany(command);
							command = "UPDATE appointment SET PatNum=" + POut.Long(tempPat.PatNum) + " WHERE PatNum=0";
							Database.ExecuteNonQuery(command);
							listAppts.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.AptNum, DbmLogFKeyType.Appointment, DbmLogActionType.Update,
								methodName, "Updated the PatNum from 0 to " + tempPat.PatNum + " from AppointmentsNoPatients.")));
							patientsAdded++;
							patNums.Remove(0);
						}
						foreach (long patnum in patNums)
						{//appointments with missing patient
							Patients.Insert(new Patient()
							{
								PatNum = patnum,
								Guarantor = patnum,
								FName = "MISSING",
								LName = "PATIENT",
								AddrNote = "DBM re-created this patient because an appointment existed for the patient on " + DateTime.Now.ToShortDateString(),
								Birthdate = DateTime.MinValue,
								BillingType = Preferences.GetLong(PreferenceName.PracticeDefaultBillType),
								PatStatus = PatientStatus.Inactive,
								PriProv = Preferences.GetLong(PreferenceName.PracticeDefaultProv),
								//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
								SecUserNumEntry = Security.CurrentUser.Id
							}, true);
							//Add new patients to listDbmLogs
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, patnum, DbmLogFKeyType.Patient, DbmLogActionType.Insert, methodName,
								"Inserted patient from AppointmentsNoPatients."));
							SecurityLogs.MakeLogEntry(Permissions.PatientCreate, patnum, "Recreated from DBM fix for AppointmentsNoPatients.", SecurityLogSource.DBM);
							patientsAdded++;
						}
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					if (count > 0 || verbose)
					{
						log += "Appointments fixed with invalid patients: " + count.ToString() + "\r\n";
						log += "Missing patients added: " + patientsAdded.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string AppointmentPlannedNoPlannedApt(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM appointment WHERE AptStatus=6 AND AptNum NOT IN (SELECT AptNum FROM plannedappt)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound != 0 || verbose)
					{
						log += "Appointments with status set to planned without Planned Appointment: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "SELECT * FROM appointment WHERE AptStatus=6 AND AptNum NOT IN (SELECT AptNum FROM plannedappt)";
					DataTable appts = Database.ExecuteDataTable(command);
					if (appts.Rows.Count > 0 || verbose)
					{
						PlannedAppt plannedAppt;
						List<DbmLog> listDbmLogs = new List<DbmLog>();
						string methodName = MethodBase.GetCurrentMethod().Name;
						for (int i = 0; i < appts.Rows.Count; i++)
						{
                            plannedAppt = new PlannedAppt
                            {
                                PatNum = PIn.Long(appts.Rows[i]["PatNum"].ToString()),
                                AptNum = PIn.Long(appts.Rows[i]["AptNum"].ToString()),
                                ItemOrder = 1
                            };
                            PlannedAppts.Insert(plannedAppt);
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, plannedAppt.PlannedApptNum, DbmLogFKeyType.PlannedAppt,
								DbmLogActionType.Insert, methodName, "Created a new planned appointment from AppointmentPlannedNoPlannedApt."));
						}
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Planned Appointments created for Appointments with status set to planned and no Planned Appointment: " + appts.Rows.Count + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string AppointmentScheduledWithCompletedProcs(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT DISTINCT appointment.PatNum, " + DbHelper.Concat("LName", "\', \'", "FName") + " AS PatName, appointment.AptDateTime "
				+ "FROM appointment "
				+ "INNER JOIN patient ON patient.PatNum=appointment.PatNum "
				+ "INNER JOIN procedurelog ON procedurelog.AptNum=appointment.AptNum "
				+ "WHERE AptStatus = " + POut.Int((int)ApptStatus.Scheduled) + " "
				+ "AND procedurelog.ProcStatus=" + POut.Int((int)ProcStat.C) + " "
				+ "ORDER BY PatName";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			//There is something to report OR the user has verbose mode on.
			string log = "Scheduled appointments with completed procedures attached: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   Manual fix needed.  Double click to see a break down.\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", including:\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "   " + table.Rows[i]["PatNum"].ToString()
								+ "-" + table.Rows[i]["PatName"].ToString()
								+ "  Appt Date:" + PIn.Date(table.Rows[i]["AptDateTime"].ToString()).ToShortDateString();
							log += "\r\n";
						}
						log += "   They need to be fixed manually.\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Appointment--------------------------------------------------------------------------------------------------------------------------
		#region AuditTrail, AutoCode, Automation--------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string AutoCodesDeleteWithNoItems(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = @"SELECT COUNT(*) FROM autocode WHERE NOT EXISTS(
						SELECT * FROM autocodeitem WHERE autocodeitem.AutoCodeNum=autocode.AutoCodeNum)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound != 0 || verbose)
					{
						log += "Autocodes found with no items: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = @"SELECT * FROM autocode WHERE NOT EXISTS(SELECT * FROM autocodeitem WHERE autocodeitem.AutoCodeNum=autocode.AutoCodeNum)";
					List<AutoCode> listAutoCodes = AutoCodes.SelectMany(command).ToList();
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = @"DELETE FROM autocode WHERE NOT EXISTS(
						SELECT * FROM autocodeitem WHERE autocodeitem.AutoCodeNum=autocode.AutoCodeNum)";
					long numberFixed = Database.ExecuteNonQuery(command);
					listAutoCodes.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.Id, DbmLogFKeyType.AutoCode,
						DbmLogActionType.Delete, methodName, "Deleted AutoCode:" + x.Description + " from AutoCodesDeleteWithNoItems")));
					if (numberFixed > 0)
					{
						Signalods.SetInvalid(InvalidType.AutoCodes);
					}
					if (numberFixed != 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Autocodes deleted due to no items: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string AutomationTriggersWithNoSheetDefs(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = @"SELECT COUNT(*) FROM automation WHERE automation.SheetDefNum!=0 AND NOT EXISTS(
					SELECT SheetDefNum FROM sheetdef WHERE automation.SheetDefNum=sheetdef.SheetDefNum)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound != 0 || verbose)
					{
						log += "Automation triggers found with no sheet defs: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmlogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = @"SELECT * FROM automation WHERE automation.SheetDefNum!=0 AND NOT EXISTS(
					SELECT SheetDefNum FROM sheetdef WHERE automation.SheetDefNum=sheetdef.SheetDefNum)";
					List<Automation> listAutomation = Automations.SelectMany(command).ToList();
					command = @"DELETE FROM automation WHERE automation.SheetDefNum!=0 AND NOT EXISTS(
					SELECT SheetDefNum FROM sheetdef WHERE automation.SheetDefNum=sheetdef.SheetDefNum)";
					long numberFixed = Database.ExecuteNonQuery(command);
					//Add to listDbmlogs
					listAutomation.ForEach(x => listDbmlogs.Add(new DbmLog(Security.CurrentUser.Id, x.Id, DbmLogFKeyType.Automation,
						DbmLogActionType.Delete, methodName, "Deleted automation from AutomationTriggersWithNoSheetDefs.")));
					if (numberFixed != 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmlogs);
						log += "Automation triggers deleted due to no sheet defs: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion AuditTrail, AutoCode, Automation-----------------------------------------------------------------------------------------------------
		#region Benefit, BillingType--------------------------------------------------------------------------------------------------------------------

		///<summary>Remove duplicates where all benefit columns match except for BenefitNum.</summary>
		[DbmMethodAttr]
		public static string BenefitsWithExactDuplicatesForInsPlan(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			DataTable table;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(DISTINCT ben2.BenefitNum) DuplicateCount "
						+ "FROM benefit ben "
						+ "INNER JOIN benefit ben2 ON ben.PlanNum=ben2.PlanNum "
						+ "AND ben.PatPlanNum=ben2.PatPlanNum "
						+ "AND ben.CovCatNum=ben2.CovCatNum "
						+ "AND ben.BenefitType=ben2.BenefitType "
						+ "AND ben.Percent=ben2.Percent "
						+ "AND ben.MonetaryAmt=ben2.MonetaryAmt "
						+ "AND ben.TimePeriod=ben2.TimePeriod "
						+ "AND ben.QuantityQualifier=ben2.QuantityQualifier "
						+ "AND ben.Quantity=ben2.Quantity "
						+ "AND ben.CodeNum=ben2.CodeNum "
						+ "AND ben.CoverageLevel=ben2.CoverageLevel "
						+ "AND ben.BenefitNum<ben2.BenefitNum";  //This ensures that the benefit with the lowest primary key in the match will not be counted.
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Duplicate benefits found" + ": " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "SELECT DISTINCT ben2.BenefitNum "
						+ "FROM benefit ben "
						+ "INNER JOIN benefit ben2 ON ben.PlanNum=ben2.PlanNum "
						+ "AND ben.PatPlanNum=ben2.PatPlanNum "
						+ "AND ben.CovCatNum=ben2.CovCatNum "
						+ "AND ben.BenefitType=ben2.BenefitType "
						+ "AND ben.Percent=ben2.Percent "
						+ "AND ben.MonetaryAmt=ben2.MonetaryAmt "
						+ "AND ben.TimePeriod=ben2.TimePeriod "
						+ "AND ben.QuantityQualifier=ben2.QuantityQualifier "
						+ "AND ben.Quantity=ben2.Quantity "
						+ "AND ben.CodeNum=ben2.CodeNum "
						+ "AND ben.CoverageLevel=ben2.CoverageLevel "
						+ "AND ben.BenefitNum<ben2.BenefitNum";  //This ensures that the benefit with the lowest primary key in the match will not be deleted.
					table = Database.ExecuteDataTable(command);
					List<long> listBenefitNums = new List<long>();
					if (table.Rows.Count > 0 || verbose)
					{
						for (int i = 0; i < table.Rows.Count; i++)
						{
							listBenefitNums.Add(PIn.Long(table.Rows[i]["BenefitNum"].ToString()));
						}
						long numFixed = 0;
						if (listBenefitNums.Count > 0)
						{
							command = "DELETE FROM benefit WHERE BenefitNum IN (" + string.Join(",", listBenefitNums) + ")";
							numFixed = PIn.Long(Database.ExecuteNonQuery(command).ToString());
						}
						log += "Duplicate benefits deleted" + ": " + numFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Identify duplicates where all benefit columns match except for BenefitNum, Percent, and MonetaryAmt.</summary>
		[DbmMethodAttr(HasBreakDown = true)]
		public static string BenefitsWithPartialDuplicatesForInsPlan(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT DISTINCT employer.EmpName,carrier.CarrierName,carrier.Phone,carrier.Address,carrier.City,carrier.State,carrier.Zip, "
				+ "insplan.GroupNum,insplan.GroupName, carrier.NoSendElect,carrier.ElectID, "
				+ "(SELECT COUNT(DISTINCT Subscriber) FROM inssub WHERE insplan.PlanNum=inssub.PlanNum) subscribers, insplan.PlanNum "
				+ "FROM benefit ben "
				+ "INNER JOIN benefit ben2 ON ben.PlanNum=ben2.PlanNum "
					+ "AND ben.PatPlanNum=ben2.PatPlanNum "
					+ "AND ben.CovCatNum=ben2.CovCatNum "
					+ "AND ben.BenefitType=ben2.BenefitType "
					+ "AND (ben.Percent!=ben2.Percent OR ben.MonetaryAmt!=ben2.MonetaryAmt) "  //Only benefits with Percent or MonetaryAmts that don't match.
					+ "AND ben.TimePeriod=ben2.TimePeriod "
					+ "AND ben.QuantityQualifier=ben2.QuantityQualifier "
					+ "AND ben.Quantity=ben2.Quantity "
					+ "AND ben.CodeNum=ben2.CodeNum "
					+ "AND ben.CoverageLevel=ben2.CoverageLevel "
					+ "AND ben.BenefitNum<ben2.BenefitNum "
				+ "INNER JOIN insplan ON insplan.PlanNum=ben.PlanNum "
				+ "LEFT JOIN carrier ON carrier.CarrierNum=insplan.CarrierNum "
				+ "LEFT JOIN employer ON employer.EmployerNum=insplan.EmployerNum";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			//There is something to report OR the user has verbose mode on.
			string log = "Insurance plans with partial duplicate benefits found: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   Manual fix needed.  Double click to see a break down.\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", including:\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							//Show the same columns as the Insurance Plans list.  We don't have an easy identifier for insurance plans, and we do not want to 
							//  give a patient example since there is a good chance that in fixing the benefits the user will just split that plan off and will 
							//  not solve the issue.
							log += "   Employer: " + table.Rows[i]["EmpName"].ToString();
							log += ",  Carrier: " + table.Rows[i]["CarrierName"].ToString();
							log += ",  Phone: " + table.Rows[i]["Phone"].ToString();
							log += ",  Address: " + table.Rows[i]["Address"].ToString();
							log += ",  City: " + table.Rows[i]["City"].ToString();
							log += ",  ST: " + table.Rows[i]["State"].ToString();
							log += ",  Zip: " + table.Rows[i]["Zip"].ToString();
							log += ",  Group#: " + table.Rows[i]["GroupNum"].ToString();
							log += ",  GroupName: " + table.Rows[i]["GroupName"].ToString();
							log += ",  NoE: ";
							if (table.Rows[i]["NoSendElect"].ToString() == "1")
							{
								log += "X";
							}
							else
							{
								log += " ";
							}
							log += ",  ElectID: " + table.Rows[i]["ElectID"].ToString();
							log += ",  Subs: " + table.Rows[i]["subscribers"].ToString();
							log += "\r\n";
						}
						log += "   They need to be fixed manually.\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string BillingTypesInvalid(bool verbose, DbmMode modeCur)
		{
			long billingType = Preferences.GetLong(PreferenceName.PracticeDefaultBillType);
			var command = "SELECT COUNT(*) FROM definition WHERE Category=4 AND definition.DefNum=" + billingType;
			int prefExists = PIn.Int(Database.ExecuteString(command));
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (prefExists != 1)
					{
						log += "No default billing type set." + "\r\n";
					}
					else if (verbose)
					{
						log += "Default practice billing type verified." + "\r\n";
					}
					//Check for any patients with invalid billingtype.
					command = "SELECT COUNT(*) FROM patient WHERE NOT EXISTS(SELECT * FROM definition WHERE Category=4 AND patient.BillingType=definition.DefNum)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound != 0 || verbose)
					{
						log += "Patients with invalid billing type: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					//Fix for default billingtype not being set.
					if (prefExists != 1)
					{//invalid billing type
						command = "SELECT DefNum FROM definition WHERE Category=4 AND IsHidden=0 ORDER BY ItemOrder";
						DataTable table = Database.ExecuteDataTable(command);
						if (table.Rows.Count == 0)
						{//if all billing types are hidden
							command = "SELECT DefNum FROM definition WHERE Category=4 ORDER BY ItemOrder";
							table = Database.ExecuteDataTable(command);
						}

						Preferences.Set(PreferenceName.PracticeDefaultBillType, table.Rows[0][0].ToString());

						log += "Default billing type preference was set due to being invalid." + "\r\n";
						Preferences.RefreshCache();//for the next line.
					}
					//Fix for patients with invalid billingtype.
					command = "UPDATE patient SET patient.BillingType=" + POut.Long(Preferences.GetLong(PreferenceName.PracticeDefaultBillType));
					command += " WHERE NOT EXISTS(SELECT * FROM definition WHERE Category=4 AND patient.BillingType=definition.DefNum)";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed != 0 || verbose)
					{
						log += "Patients billing type set to default due to being invalid: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Benefit, BillingType-----------------------------------------------------------------------------------------------------------------
		#region Carrier, Claim--------------------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr(IsCanada = true)]
		public static string CanadaCarriersCdaMissingInfo(bool verbose, DbmMode modeCur)
		{
			if (!CultureInfo.CurrentCulture.Name.EndsWith("CA"))
			{//Canadian. en-CA or fr-CA
				return "Skipped. Local computer region must be set to Canada to run.";
			}
			string command = "SELECT CanadianNetworkNum FROM canadiannetwork WHERE Abbrev='TELUS B' LIMIT 1";
			long canadianNetworkNumTelusB = PIn.Long(Database.ExecuteString(command));
			command = "SELECT CanadianNetworkNum FROM canadiannetwork WHERE Abbrev='CSI' LIMIT 1";
			//CSI is now known as "instream"
			long canadianNetworkNumCSI = PIn.Long(Database.ExecuteString(command));
			command = "SELECT CanadianNetworkNum FROM canadiannetwork WHERE Abbrev='CDCS' LIMIT 1";
			long canadianNetworkNumCDCS = PIn.Long(Database.ExecuteString(command));
			command = "SELECT CanadianNetworkNum FROM canadiannetwork WHERE Abbrev='TELUS A' LIMIT 1";
			long canadianNetworkNumTelusA = PIn.Long(Database.ExecuteString(command));
			command = "SELECT CanadianNetworkNum FROM canadiannetwork WHERE Abbrev='MBC' LIMIT 1";
			long canadianNetworkNumMBC = PIn.Long(Database.ExecuteString(command));
			command = "SELECT CanadianNetworkNum FROM canadiannetwork WHERE Abbrev='ABC' LIMIT 1";
			long canadianNetworkNumABC = PIn.Long(Database.ExecuteString(command));
			CanSupTransTypes claimTypes = CanSupTransTypes.ClaimAckEmbedded_11e | CanSupTransTypes.ClaimEobEmbedded_21e;//Claim 01, claim ack 11, and claim eob 21 are implied.
			CanSupTransTypes reversalTypes = CanSupTransTypes.ClaimReversal_02 | CanSupTransTypes.ClaimReversalResponse_12;
			CanSupTransTypes predeterminationTypes = CanSupTransTypes.PredeterminationAck_13 | CanSupTransTypes.PredeterminationAckEmbedded_13e | CanSupTransTypes.PredeterminationMultiPage_03 | CanSupTransTypes.PredeterminationSinglePage_03;
			CanSupTransTypes rotTypes = CanSupTransTypes.RequestForOutstandingTrans_04;
			CanSupTransTypes cobTypes = CanSupTransTypes.CobClaimTransaction_07;
			CanSupTransTypes eligibilityTypes = CanSupTransTypes.EligibilityTransaction_08 | CanSupTransTypes.EligibilityResponse_18;
			CanSupTransTypes rprTypes = CanSupTransTypes.RequestForPaymentReconciliation_06;
			//Column order: ElectID,CanadianEncryptionMethod,CDAnetVersion,CanadianSupportedTypes,CanadianNetworkNum
			object[] carrierInfo = new object[] {
				//accerta
				"311140",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumCSI,
				//adsc
				"000105",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes|eligibilityTypes,canadianNetworkNumCSI,
				//aga
				"610226",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//appq
				"628112",1,"02",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//alberta blue cross. Usually sent through ClaimStream instead of ITRANS.
				"000090",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes,canadianNetworkNumABC,
				//assumption life
				"610191",1,"04",claimTypes,canadianNetworkNumTelusB,
				//autoben
				"628151",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//benecaid health benefit solutions
				"610708",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//benefits trust
				"610146",1,"02",claimTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//beneplan - Old carrier that is no longer listed one iTrans supported list.
				"400008",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//boilermakers' national benefit plan - Old carrier that is no longer listed one iTrans supported list.
				"000116",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//canadian benefit providers
				"610202",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//capitale
				"600502",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes,canadianNetworkNumTelusB,
				//carpenters and allied workers local
				"000117",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//cdcs
				"610129",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumCDCS,
				//claimsecure
				"610099",1,"04",claimTypes|eligibilityTypes,canadianNetworkNumTelusB,
				//co-operators
				"606258",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//Commision de la construction du Quebec
				"000036",1,"02",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//coughlin & associates
				"610105",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes,canadianNetworkNumTelusB,
				//cowan wright beauchamps
				"610153",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//desjardins financial security
				"000051",1,"04",claimTypes|reversalTypes|rotTypes,canadianNetworkNumTelusB,
				//empire life insurance company
				"000033",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//equitable life
				"000029",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//esorse corporation
				"610650",1,"04",claimTypes|reversalTypes|predeterminationTypes|rprTypes|cobTypes,canadianNetworkNumTelusB,
				//fas administrators
				"610614",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//GMS Insurance Inc. (GMS) (ESC)
				"610218",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumCSI,
				//great west life assurance company
				"000011",1,"04",claimTypes|cobTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//green sheild canada
				"000102",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//group medical services
				"610217",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//groupe premier medical
				"610266",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//grouphealth benefit solutions
				"000125",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//groupsource - Old carrier that is no longer listed one iTrans supported list.
				"605064",1,"04",claimTypes|reversalTypes|eligibilityTypes,canadianNetworkNumCSI,
				//Humania Assurance Inc (formerly La Survivance) (ESC)
				"000080",1,"04",claimTypes,canadianNetworkNumTelusB,
				//industrial alliance
				"000060",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusA,
				//industrial alliance pacific insurance and financial
				"000024",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusA,
				//johnson inc.
				"627265",1,"04",claimTypes,canadianNetworkNumTelusB,
				//johnston group
				"627223",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumCSI,
				//lee-power & associates
				"627585",1,"02",claimTypes,canadianNetworkNumTelusA,
				//local 1030 health benefity plan
				"000118",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//manion wilkins
				"610158",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//manitoba blue cross
				"000094",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes,canadianNetworkNumMBC,
				//manitoba cleft palate program
				"000114",1,"04",claimTypes|predeterminationTypes|rotTypes,canadianNetworkNumCSI,
				//manitoba health
				"000113",1,"04",claimTypes|rotTypes,canadianNetworkNumCSI,
				//telus adjudicare
				"000034",1,"04",claimTypes|cobTypes|reversalTypes|predeterminationTypes|eligibilityTypes,canadianNetworkNumTelusB,
				//manulife financial
				"610059",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//maritime life assurance company
				"311113",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//maritime pro
				"610070",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//mdm
				"601052",1,"02",claimTypes|reversalTypes|predeterminationTypes|eligibilityTypes,canadianNetworkNumTelusB,
				//medavie blue cross
				"610047",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//nexgenrx
				"610634",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//Non-Insured Health Benefits (NIHB) Program (ESC)
				"610124",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//nova scotia community services
				"000109",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes|eligibilityTypes,canadianNetworkNumCSI,
				//nova scotia medical services insurance
				"000108",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes|eligibilityTypes,canadianNetworkNumCSI,
				//nunatsiavut government department of health
				"610172",1,"04",claimTypes|reversalTypes,canadianNetworkNumCSI,
				//ontario ironworkers
				"000123",1,"04",claimTypes|predeterminationTypes|cobTypes,canadianNetworkNumCSI,
				//pacific blue cross
				"000064",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumCSI,
				//pbas
				"610256",1,"04",claimTypes|predeterminationTypes|rotTypes,canadianNetworkNumCSI,
				//quickcard
				"000103",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes|eligibilityTypes,canadianNetworkNumCSI,
				//rbc insurance
				"000124",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes,canadianNetworkNumTelusB,
				//rwam insurance
				"610616",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//saskatchewan blue cross
				"000096",1,"04",claimTypes,canadianNetworkNumTelusB,
				//Segic (BATCH) benefits
				"610360",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//ses benefits
				"610196",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//sheet metal workers local 30 benefit plan - Old carrier that is no longer listed one iTrans supported list.
				"000119",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//ssq societe d'assurance-vie inc.
				"000079",1,"04",claimTypes,canadianNetworkNumTelusB,
				//standard life assurance company
				"000020",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//sun life of canada
				"000016",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes,canadianNetworkNumTelusB,
				//syndicat des fonctionnaires municipaux mtl
				"610677",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//TELUS HS Assure test carrier (V4)
				"000010",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//the building union of canada health beneift plan
				"000120",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//U-L Mutual (ESC)
				"610643",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//u.a. local 46 dental plan
				"000115",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//u.a. local 787 health trust fund dental plan - Old carrier that is no longer listed one iTrans supported list.
				"000110",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//wawanesa
				"311109",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
			};
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = 0;
					for (int i = 0; i < carrierInfo.Length; i += 5)
					{
						command = "SELECT COUNT(*) " +
							"FROM carrier " +
							"WHERE IsCDA<>0 AND ElectID='" + POut.String((string)carrierInfo[i]) + "' AND " +
							"(CanadianEncryptionMethod<>" + POut.Int((int)carrierInfo[i + 1]) + " OR " +
							"CDAnetVersion<>'" + POut.String((string)carrierInfo[i + 2]) + "' OR " +
							"CanadianSupportedTypes<>" + POut.Int((int)carrierInfo[i + 3]) + " OR " +
							"CanadianNetworkNum<>" + POut.Long((long)carrierInfo[i + 4]) + ")";
						numFound += PIn.Int(Database.ExecuteString(command));
					}
					if (numFound != 0 || verbose)
					{
						log += "CDANet carriers with incorrect network, encryption method or version, based on carrier identification number: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					long numberFixed = 0;
					List<DbmLog> listDbmlogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					for (int i = 0; i < carrierInfo.Length; i += 5)
					{
						command = "SELECT * " +
							"FROM carrier " +
							"WHERE IsCDA<>0 AND ElectID='" + POut.String((string)carrierInfo[i]) + "' AND " +
							"(CanadianEncryptionMethod<>" + POut.Int((int)carrierInfo[i + 1]) + " OR " +
							"CDAnetVersion<>'" + POut.String((string)carrierInfo[i + 2]) + "' OR " +
							"CanadianSupportedTypes<>" + POut.Int((int)carrierInfo[i + 3]) + " OR " +
							"CanadianNetworkNum<>" + POut.Long((long)carrierInfo[i + 4]) + ")";
						List<Carrier> listCarriers = Crud.CarrierCrud.SelectMany(command);
						command = "UPDATE carrier SET " +
							"CanadianEncryptionMethod=" + POut.Int((int)carrierInfo[i + 1]) + "," +
							"CDAnetVersion='" + POut.String((string)carrierInfo[i + 2]) + "'," +
							"CanadianSupportedTypes=" + POut.Int((int)carrierInfo[i + 3]) + "," +
							"CanadianNetworkNum=" + POut.Long((long)carrierInfo[i + 4]) + " " +
							"WHERE IsCDA<>0 AND ElectID='" + POut.String((string)carrierInfo[i]) + "' AND " +
							"(CanadianEncryptionMethod<>" + POut.Int((int)carrierInfo[i + 1]) + " OR " +
							"CDAnetVersion<>'" + POut.String((string)carrierInfo[i + 2]) + "' OR " +
							"CanadianSupportedTypes<>" + POut.Int((int)carrierInfo[i + 3]) + " OR " +
							"CanadianNetworkNum<>" + POut.Long((long)carrierInfo[i + 4]) + ")";
						long numUpdated = Database.ExecuteNonQuery(command);
						numberFixed += numUpdated;
						//add changes to dbmLogs
						if (numUpdated > 0)
						{
							listCarriers.ForEach(x => listDbmlogs.Add(new DbmLog(Security.CurrentUser.Id, x.Id, DbmLogFKeyType.Carrier,
								DbmLogActionType.Update, methodName, "Updated Carrier info for carrier: " + x.Name)));
						}
					}
					if (numberFixed != 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmlogs);
						log += "CDANet carriers fixed based on carrier identification number: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimDeleteWithNoClaimProcs(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = @"SELECT COUNT(*) FROM claim WHERE NOT EXISTS(
						SELECT * FROM claimproc WHERE claim.ClaimNum=claimproc.ClaimNum)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound != 0 || verbose)
					{
						log += "Claims found with no procedures" + ": " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "SELECT claim.ClaimNum FROM claim WHERE NOT EXISTS( "
						+ "SELECT * FROM claimproc WHERE claim.ClaimNum=claimproc.ClaimNum)";
					DataTable tableClaimNums = Database.ExecuteDataTable(command);
					List<long> listClaimNums = new List<long>();
					for (int i = 0; i < tableClaimNums.Rows.Count; i++)
					{
						listClaimNums.Add(PIn.Long(tableClaimNums.Rows[i]["ClaimNum"].ToString()));
					}
					if (listClaimNums.Count > 0)
					{
						//List<Permissions> listPerms = GroupPermissions.GetPermsFromCrudAuditPerm(CrudTableAttribute.GetCrudAuditPermForClass(typeof(Claim)));
						//List<SecurityLog> listSecurityLogs = SecurityLogs.GetFromFKeysAndType(listClaimNums, listPerms);
						Claims.ClearFkey(listClaimNums);//Zero securitylog FKey column for rows to be deleted.
														//Insert changes to DbmLogs
						//listSecurityLogs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.SecurityLogNum, DbmLogFKeyType.Securitylog,
						//	DbmLogActionType.Update, methodName, "Updated FKey from " + x.FKey + " to 0 from ClaimDeleteWithNoClaimProcs.")));
					}
					//Orphaned claims do not show in the account module (tested) so we need to delete them because no other way.
					command = @"DELETE FROM claim WHERE NOT EXISTS(
						SELECT * FROM claimproc WHERE claim.ClaimNum=claimproc.ClaimNum)";
					long numberFixed = Database.ExecuteNonQuery(command);
					listClaimNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Claim, DbmLogActionType.Delete,
						methodName, "Deleted claim from ClaimDeleteWithNoClaimProcs")));
					if (numberFixed != 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Claims deleted due to no procedures" + ": " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClaimWithInvalidInsSubNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				#region Check
				case DbmMode.Check:
					//Claim.PlanNum is 0 and inssub.plannum is 0 or does not exist.
					command = @"SELECT COUNT(*) FROM claim 
						LEFT JOIN inssub ON claim.InsSubNum=inssub.InsSubNum
						WHERE claim.PlanNum=0 
						AND ( inssub.PlanNum=0 OR inssub.PlanNum IS NULL )";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound != 0 || verbose)
					{
						log += "Claims with invalid InsSubNum" + ": " + numFound + "\r\n";
						log += "   Double click to see a break down.\r\n";
					}
					//situation where PlanNum and InsSubNum are both invalid and not zero is handled in InsSubNumMismatchPlanNum
					break;
				#endregion
				#region Breakdown
				case DbmMode.Breakdown:
					command = @"SELECT claim.ClaimNum, claim.PatNum, claim.DateService FROM claim 
						LEFT JOIN inssub ON claim.InsSubNum=inssub.InsSubNum 
						WHERE claim.PlanNum=0 
						AND ( inssub.PlanNum=0 OR inssub.PlanNum IS NULL )";
					//Turn the query results into a dictionary in order to make it easier to create a breakdown for the user.
					var dictPatClaims = Database.ExecuteDataTable(command).Select().Select(x => new
					{
						ClaimNum = PIn.Long(x["ClaimNum"].ToString()),
						PatNum = PIn.Long(x["PatNum"].ToString()),
						DateService = PIn.Date(x["DateService"].ToString())
					})
						.GroupBy(x => x.PatNum)
						.ToDictionary(x => x.Key, x => x.ToList());
					if (dictPatClaims.Count > 0 || verbose)
					{
						//Get minimal patient information in order to display the name of the patient to the user.
						List<Patient> listPatLims = Patients.GetLimForPats(dictPatClaims.Keys.ToList());
						log += "Claims with invalid InsSubNum" + ": " + dictPatClaims.Values.Sum(x => x.Count) + "\r\n";
						log += "Patients Affected" + ": " + dictPatClaims.Count + "\r\n\r\n";
						StringBuilder strBuilder = new StringBuilder();
						foreach (Patient patLim in listPatLims)
						{//Only show information for patients found in the database.
						 //No translations needed, all "words" will exactly match schema column names.
							strBuilder.AppendLine(patLim.GetNameLF() + " (PatNum:" + patLim.PatNum + ")");
							dictPatClaims[patLim.PatNum].ForEach(x =>
								strBuilder.AppendLine($"    ClaimNum:{x.ClaimNum} DateService:{x.DateService.ToShortDateString()}")
							);
							strBuilder.AppendLine("");//Add a newline between each patient for a nice visible separation.
						}
						log += strBuilder.ToString();//Does nothing if strBuilder is empty.
					}
					break;
				#endregion
				#region Fix
				case DbmMode.Fix:
					command = @"SELECT claim.ClaimNum, claim.PatNum FROM claim 
						LEFT JOIN inssub ON claim.InsSubNum=inssub.InsSubNum 
						WHERE claim.PlanNum=0 
						AND ( inssub.PlanNum=0 OR inssub.PlanNum IS NULL )";
					DataTable table = Database.ExecuteDataTable(command);
					long numberFixed = table.Rows.Count;
					InsurancePlan plan = null;
					InsSub sub = null;
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					if (numberFixed > 0)
					{
						log += "Reenter insurance information for patients associated to UNKNOWN CARRIER." + "\r\n";
					}
					long unknownCarrierNum = Carriers.GetByNameAndPhone("UNKNOWN CARRIER", "", true).Id;
					for (int i = 0; i < numberFixed; i++)
					{
                        plan = new InsurancePlan
                        {
                            IsHidden = true,
                            CarrierId = unknownCarrierNum,
                            //Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
                            SecUserNumEntry = Security.CurrentUser.Id
                        };//Create a dummy plan and carrier to attach claims and claim procs to.
                        InsPlans.Insert(plan);
						long claimNum = PIn.Long(table.Rows[i]["ClaimNum"].ToString());
                        sub = new InsSub
                        {
                            PlanNum = plan.Id,
                            Subscriber = PIn.Long(table.Rows[i]["PatNum"].ToString()),
                            SubscriberID = "unknown",
                            //Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
                            SecUserNumEntry = Security.CurrentUser.Id
                        };//Create inssubs and attach claim and procs to both plan and inssub.
                        InsSubs.Insert(sub);
						listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, sub.InsSubNum, DbmLogFKeyType.InsSub, DbmLogActionType.Insert,
							methodName, "Added new InsSub from ClaimWithInvalidInsSubNum."));
						List<Claim> listClaims = Crud.ClaimCrud.SelectMany("SELECT * FROM claim WHERE ClaimNum=" + claimNum);
						command = "UPDATE claim SET PlanNum=" + plan.Id + ",InsSubNum=" + sub.InsSubNum + " WHERE ClaimNum=" + claimNum;
						Database.ExecuteNonQuery(command);
						listClaims.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimNum, DbmLogFKeyType.Claim, DbmLogActionType.Update,
							methodName, "Updated PlanNum from " + x.PlanNum + " to " + plan.Id
							+ " and InsSubNum from " + x.InsSubNum + " to " + sub.InsSubNum + " from ClaimWithInvalidInsSubNum")));
						List<ClaimProc> listClaimProc = Crud.ClaimProcCrud.SelectMany("SELECT * FROM claimproc WHERE ClaimNum=" + claimNum);
						command = "UPDATE claimproc SET PlanNum=" + plan.Id + ",InsSubNum=" + sub.InsSubNum + " WHERE ClaimNum=" + claimNum;
						Database.ExecuteNonQuery(command);
						listClaimProc.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimProcNum, DbmLogFKeyType.ClaimProc,
							DbmLogActionType.Update, methodName, "Updated PlanNum from " + x.PlanNum + " to " + plan.Id
							+ " and InsSubNum from " + x.InsSubNum + " to " + sub.InsSubNum + " from ClaimWithInvalidInsSubNum")));
					}
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Claims with invalid InsSubNum fixed" + ": " + numberFixed.ToString() + "\r\n";
					}
					break;
					#endregion
			}
			return log;
		}

		///<summary>Also fixes situations where PatNum=0</summary>
		[DbmMethodAttr]
		public static string ClaimWithInvalidPatNum(bool verbose, DbmMode modeCur)
		{
			string command = @"SELECT claim.ClaimNum, procedurelog.PatNum patNumCorrect
				FROM claim, claimproc, procedurelog
				WHERE claim.PatNum NOT IN (SELECT PatNum FROM patient)
				AND claim.ClaimNum=claimproc.ClaimNum
				AND claimproc.ProcNum=procedurelog.ProcNum
				AND procedurelog.PatNum!=0
				GROUP BY claim.ClaimNum, procedurelog.PatNum";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Claims found with invalid patients attached: " + table.Rows.Count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					for (int i = 0; i < table.Rows.Count; i++)
					{
						command = "SELECT * FROM claim WHERE ClaimNum=" + POut.Long(PIn.Long(table.Rows[i]["ClaimNum"].ToString()));
						List<Claim> listClaims = Crud.ClaimCrud.SelectMany(command);
						command = "UPDATE claim SET PatNum='" + POut.Long(PIn.Long(table.Rows[i]["patNumCorrect"].ToString())) + "' "
							+ "WHERE ClaimNum=" + POut.Long(PIn.Long(table.Rows[i]["ClaimNum"].ToString()));
						Database.ExecuteNonQuery(command);
						listClaims.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimNum, DbmLogFKeyType.Claim, DbmLogActionType.Update,
							methodName, "Updated PatNum from " + x.PatNum + " to " + table.Rows[i]["patNumCorrect"].ToString())));
					}
					int numberFixed = table.Rows.Count;
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Claim with invalid PatNums fixed: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimWithInvalidProvTreat(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT * FROM claim WHERE ProvTreat > 0 AND ProvTreat NOT IN (SELECT ProvNum FROM provider);";
			List<Claim> listClaims = Crud.ClaimCrud.SelectMany(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listClaims.Count;
					if (numFound > 0 || verbose)
					{
						log += "Claims with invalid ProvTreat found: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "UPDATE claim SET ProvTreat=" + POut.Long(Preferences.GetLong(PreferenceName.PracticeDefaultProv)) +
							" WHERE ProvTreat > 0 AND ProvTreat NOT IN (SELECT ProvNum FROM provider);";
					long numFixed = Database.ExecuteNonQuery(command);
					listClaims.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimNum, DbmLogFKeyType.Claim, DbmLogActionType.Update,
						methodName, "Updated ProvTreat from " + x.ProvTreat + " to " + POut.Long(Preferences.GetLong(PreferenceName.PracticeDefaultProv)))));
					if (numFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Claims with invalid ProvTreat fixed: " + numFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimWriteoffSum(bool verbose, DbmMode modeCur)
		{
			//Sums for each claim---------------------------------------------------------------------
			string command = @"SELECT claim.ClaimNum,SUM(claimproc.WriteOff) sumwo,claim.WriteOff
				FROM claim,claimproc
				WHERE claim.ClaimNum=claimproc.ClaimNum
				GROUP BY claim.ClaimNum,claim.WriteOff
				HAVING sumwo-claim.WriteOff > .01
				OR sumwo-claim.WriteOff < -.01";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Claim writeoff sums found incorrect: " + table.Rows.Count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					for (int i = 0; i < table.Rows.Count; i++)
					{
						command = "SELECT * FROM claim WHERE ClaimNum=" + table.Rows[i]["ClaimNum"].ToString();
						List<Claim> listClaims = Crud.ClaimCrud.SelectMany(command);
						command = "UPDATE claim SET WriteOff='" + POut.Double(PIn.Double(table.Rows[i]["sumwo"].ToString())) + "' "
							+ "WHERE ClaimNum=" + table.Rows[i]["ClaimNum"].ToString();
						Database.ExecuteNonQuery(command);
						listClaims.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimNum, DbmLogFKeyType.Claim, DbmLogActionType.Update,
							methodName, "Updated the WriteOff from " + table.Rows[i]["WriteOff"].ToString()
							+ " to " + table.Rows[i]["sumwo"].ToString() + " from ClaimWriteoffSum")));
					}
					int numberFixed = table.Rows.Count;
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Claim writeoff sums fixed: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Carrier, Claim-----------------------------------------------------------------------------------------------------------------------
		#region ClaimPayment----------------------------------------------------------------------------------------------------------------------------

		///<summary>Finds claimpayments where the CheckAmt!=SUM(InsPayAmt) for the claimprocs attached to the claimpayment.  Manual fix.</summary>
		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClaimPaymentCheckAmt(bool verbose, DbmMode modeCur)
		{
			//because of the way this is grouped, it will just get one of many patients for each
			string command = @"SELECT claimproc.ClaimPaymentNum,ROUND(SUM(InsPayAmt),2) _sumpay,ROUND(CheckAmt,2) _checkamt,claimproc.PatNum
					FROM claimpayment,claimproc
					WHERE claimpayment.ClaimPaymentNum=claimproc.ClaimPaymentNum
					GROUP BY claimproc.ClaimPaymentNum,CheckAmt
					HAVING _sumpay!=_checkamt";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			//There is something to report OR the user has verbose mode on.
			string log = "Claim payment sums found incorrect: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{//Does not make any DB changes, see breakdown for manual fix.
						log += "\r\n   " + "Manual fix needed.  Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						List<DbmLog> listDbmLogs = new List<DbmLog>();
						string methodName = MethodBase.GetCurrentMethod().Name;
						log += ", " + "including" + ":\r\n";
						//Changing the claim payment sums automatically is dangerous so give the user enough information to investigate themselves.
						for (int i = 0; i < table.Rows.Count; i++)
						{
							Patient pat = Patients.GetPat(PIn.Long(table.Rows[i]["PatNum"].ToString()));
							command = "SELECT CheckDate,CheckAmt,IsPartial FROM claimpayment WHERE ClaimPaymentNum=" + table.Rows[i]["ClaimPaymentNum"].ToString();
							DataTable claimPayTable = Database.ExecuteDataTable(command);
							if (pat == null)
							{
                                //insert pat
                                Patient dummyPatient = new Patient
                                {
                                    PatNum = PIn.Long(table.Rows[i]["PatNum"].ToString())
                                };
                                dummyPatient.Guarantor = dummyPatient.PatNum;
								dummyPatient.FName = "MISSING";
								dummyPatient.LName = "PATIENT";
								dummyPatient.AddrNote = "This patient was inserted due to claimprocs with invalid PatNum on " + DateTime.Now.ToShortDateString() + " while doing database maintenance.";
								dummyPatient.BillingType = Preferences.GetLong(PreferenceName.PracticeDefaultBillType);
								dummyPatient.PatStatus = PatientStatus.Archived;
								dummyPatient.PriProv = Preferences.GetLong(PreferenceName.PracticeDefaultProv);
								//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
								dummyPatient.SecUserNumEntry = Security.CurrentUser.Id;
								long dummyPatNum = Patients.Insert(dummyPatient, true);
								listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, dummyPatNum, DbmLogFKeyType.Patient, DbmLogActionType.Insert,
									methodName, "Inserted new patient from ClaimPaymentCheckAmt."));
								SecurityLogs.MakeLogEntry(Permissions.PatientCreate, dummyPatNum, "Recreated from DBM fix for ClaimPaymentCheckAmt.", SecurityLogSource.DBM);
								pat = Patients.GetPat(dummyPatient.PatNum);
							}
							log += "   Patient: #" + table.Rows[i]["PatNum"].ToString() + ":" + pat.GetNameFirstOrPrefL()
									+ " Date: " + PIn.Date(claimPayTable.Rows[0]["CheckDate"].ToString()).ToShortDateString()
									+ " Amount: " + PIn.Double(claimPayTable.Rows[0]["CheckAmt"].ToString()).ToString("F");
							if (!PIn.Bool(claimPayTable.Rows[0]["IsPartial"].ToString()))
							{
								command = "SELECT * from claimpayment WHERE ClaimPaymentNum=" + PIn.Long(table.Rows[i]["ClaimPaymentNum"].ToString()).ToString();
								List<ClaimPayment> listClaimPayment = Crud.ClaimPaymentCrud.SelectMany(command);
								command = "UPDATE claimpayment SET IsPartial=1 WHERE ClaimPaymentNum=" + PIn.Long(table.Rows[i]["ClaimPaymentNum"].ToString()).ToString();
								Database.ExecuteNonQuery(command);
								listClaimPayment.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimPaymentNum, DbmLogFKeyType.ClaimPayment,
									DbmLogActionType.Update, methodName, "Updated ClaimPaymentNum from " + x.IsPartial + " to True from ClaimPaymentCheckAmt.")));
								log += " (row has been unlocked and marked as partial)";
							}
							log += "\r\n";
						}
						if (listDbmLogs.Count > 0)
						{
							Crud.DbmLogCrud.InsertMany(listDbmLogs);
						}
						log += "   They need to be fixed manually." + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimPaymentDetachMissingDeposit(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT * FROM claimpayment "
				+ "WHERE DepositNum != 0 "
				+ "AND NOT EXISTS(SELECT * FROM deposit WHERE deposit.DepositNum=claimpayment.DepositNum)";
			List<ClaimPayment> listClaimPayments = Crud.ClaimPaymentCrud.SelectMany(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listClaimPayments.Count;
					if (numFound > 0 || verbose)
					{
						log += "Claim payments attached to deposits that no longer exist: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "UPDATE claimpayment SET DepositNum=0 "
						+ "WHERE DepositNum != 0 "
						+ "AND NOT EXISTS(SELECT * FROM deposit WHERE deposit.DepositNum=claimpayment.DepositNum)";
					long numberFixed = Database.ExecuteNonQuery(command);
					listClaimPayments.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimPaymentNum, DbmLogFKeyType.ClaimPayment,
						DbmLogActionType.Update, methodName, "Updated the DepositNum from " + x.DepositNum + " to 0 .")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Claim payments detached from deposits that no longer exist: "
						+ numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClaimPaymentsNotPartialWithNoClaimProcs(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT claimpayment.CheckDate, definition.ItemName, claimpayment.CheckAmt, " +
				"claimpayment.CarrierName, clinic.Description, claimpayment.Note " +
				"FROM claimpayment " +
				"INNER JOIN definition ON definition.DefNum=claimpayment.PayType " +
				"LEFT JOIN clinic ON clinic.ClinicNum=claimpayment.ClinicNum " +
				"WHERE claimpayment.IsPartial=0 " +
				"AND NOT EXISTS( " +
					"SELECT ClaimProcNum " +
					"FROM claimproc " +
					"WHERE claimproc.ClaimPaymentNum=claimpayment.ClaimPaymentNum " +
				") ";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			//There is something to report OR the user has verbose mode on.   
			string log = "Insurance payments with no claims attached that are not marked as partial" + ": " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   " + "Manual fix needed.  Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", " + "including" + ":\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "   " + "Date" + ": " + PIn.Date(table.Rows[i]["CheckDate"].ToString()).ToShortDateString();
							log += ", " + "Type" + ": " + PIn.String(table.Rows[i]["ItemName"].ToString());
							log += ", " + "Amount" + ": " + PIn.Double(table.Rows[i]["CheckAmt"].ToString()).ToString("c");
							//Partial will always be blank
							log += ", " + "Carrier" + ": " + PIn.String(table.Rows[i]["CarrierName"].ToString());
							log += ", " + "Clinic" + ": " + PIn.String(table.Rows[i]["Description"].ToString());
							log += ", " + "Note" + ": ";
							if (PIn.String(table.Rows[i]["Note"].ToString()).Length > 15)
							{
								log += PIn.String(table.Rows[i]["Note"].ToString()).Substring(0, 15) + "...";
							}
							else
							{
								log += PIn.String(table.Rows[i]["Note"].ToString());
							}
							log += "\r\n";
						}
						log += "   " + "They need to be fixed manually." + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion ClaimPayment-------------------------------------------------------------------------------------------------------------------------
		#region ClaimProc-------------------------------------------------------------------------------------------------------------------------------
		/// <summary>Shows patients that have claim payments attached to patient payment plans.</summary>
		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClaimProcAttachedToPatientPaymentPlans(bool verbose, DbmMode modeCur)
		{
			DataTable table = GetClaimProcsAttachedToPatientPaymentPlans();
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "ClaimProcs attached to insurance payment plans" + ": " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n" + "Manual fix needed. Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", " + "including" + ":\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "\r\n   " + "Patient #" + table.Rows[i]["PatNum"].ToString() + " "
								+ "has a payment amount for" + " " + PIn.Double(table.Rows[i]["InsPayAmt"].ToString()).ToString("c") + " "
								+ "on date" + " " + PIn.Date(table.Rows[i]["DateCP"].ToString()).ToShortDateString() + " "
								+ "attached to patient payment plan #" + table.Rows[i]["PayPlanNum"];
						}
						log += "\r\n" + "Run 'Pay Plan Payments' in the Tools tab to fix these payments.";
					}
					break;
			}
			return log;
		}

		///<summary>Deletes claimprocs that are attached to group notes.</summary>
		[DbmMethodAttr]
		public static string ClaimProcEstimateAttachedToGroupNote(bool verbose, DbmMode modeCur)
		{
			//It is impossible to attach a group note to a claim, because group notes always have status EC, but status C is required to attach to a claim, or status TP for a preauth.
			//Since the group note cannot be attached to a claim, it also cannot be attached to a claim payment.
			//Claimproc estimates attached to group notes cannot be viewed anywhere in the UI.
			string command = "SELECT claimproc.ClaimProcNum "
				+ "FROM claimproc "
				+ "INNER JOIN procedurelog ON claimproc.ProcNum=procedurelog.ProcNum "
				+ "INNER JOIN procedurecode ON procedurecode.CodeNum=procedurelog.CodeNum AND procedurecode.ProcCode='~GRP~' "
				+ "WHERE claimproc.Status=" + POut.Int((int)ClaimProcStatus.Estimate) + " AND claimproc.ClaimNum=0 AND claimproc.ClaimPaymentNum=0";//Ensures that the claimproc has no relevant information attached to it.
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Estimates attached to group notes" + ": " + table.Rows.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					if (table.Rows.Count > 0)
					{
						string inCommand = string.Join(",", table.Select().Select(x => PIn.Long(x["ClaimProcNum"].ToString())));
						command = "DELETE FROM claimproc WHERE claimproc.ClaimProcNum IN (" + inCommand + ")";
						Database.ExecuteNonQuery(command);
						table.Select().Select(x => PIn.Long(x["ClaimProcNum"].ToString())).ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id,
							x, DbmLogFKeyType.ClaimProc, DbmLogActionType.Delete, methodName, "Deleted claimproc.")));
					}
					if (table.Rows.Count > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Estimates attached to group notes deleted" + ": " + table.Rows.Count + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDateNotMatchCapComplete(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT * FROM claimproc WHERE Status=7 AND DateCP != ProcDate";
			List<ClaimProc> listClaimProcs = Crud.ClaimProcCrud.SelectMany(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listClaimProcs.Count;
					if (numFound > 0 || verbose)
					{
						log += "Capitation procs with mismatched dates found: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					//js ok
					command = "UPDATE claimproc SET DateCP=ProcDate WHERE Status=7 AND DateCP != ProcDate";
					long numberFixed = Database.ExecuteNonQuery(command);
					listClaimProcs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimProcNum, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Update, methodName, "Updated the DateCp from " + x.DateCP + " to " + x.ProcDate + ".")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Capitation procs with mismatched dates fixed: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClaimProcDateNotMatchPayment(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			DataTable table;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT claimproc.ClaimProcNum,claimpayment.CheckDate FROM claimproc,claimpayment "
						+ "WHERE claimproc.ClaimPaymentNum=claimpayment.ClaimPaymentNum "
						+ "AND claimproc.DateCP!=claimpayment.CheckDate";
					table = Database.ExecuteDataTable(command);
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Claim payments with mismatched dates found:" + " " + table.Rows.Count.ToString() + "\r\n";
						log += "   " + "Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown: //Splits off below, no DB changes.
				case DbmMode.Fix:
					//This is a very strict relationship that has been enforced rigorously for many years.
					//If there is an error, it is a fairly new error.  All errors must be repaired.
					//It won't change amounts of history, just dates.  The changes will typically be only a few days or weeks.
					//Various reports assume this enforcement and the reports will malfunction if this is not fixed.
					//Let's list out each change.  Patient name, procedure desc, date of service, old dateCP, new dateCP (check date).
					command = "SELECT patient.LName,patient.FName,patient.MiddleI,claimproc.CodeSent,claim.DateService,claimproc.DateCP,claimpayment.CheckDate,claimproc.ClaimProcNum "
						+ "FROM claimproc,patient,claim,claimpayment "
						+ "WHERE claimproc.PatNum=patient.PatNum "
						+ "AND claimproc.ClaimNum=claim.ClaimNum "
						+ "AND claimproc.ClaimPaymentNum=claimpayment.ClaimPaymentNum "
						+ "AND claimproc.DateCP!=claimpayment.CheckDate";
					table = Database.ExecuteDataTable(command);
					string patientName;
					string codeSent;
					DateTime dateService;
					DateTime oldDateCP;
					DateTime newDateCP;
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					List<long> listClaimProcNums = null;
					long claimProcNum;
					StringBuilder strBuild = new StringBuilder();
					if (table.Rows.Count > 0 || verbose)
					{
						strBuild.AppendLine("Claim payments with mismatched dates (Patient Name, Code Sent, Date of Service, Old Date, New Date):");
					}
					for (int i = 0; i < table.Rows.Count; i++)
					{
						patientName = table.Rows[i]["LName"].ToString() + ", " + table.Rows[i]["FName"].ToString() + " " + table.Rows[i]["MiddleI"].ToString();
						patientName = patientName.Trim();//Looks better when middle initial is not present.//Doesn't work though
						codeSent = table.Rows[i]["CodeSent"].ToString();
						dateService = PIn.Date(table.Rows[i]["DateService"].ToString());
						oldDateCP = PIn.Date(table.Rows[i]["DateCP"].ToString());
						newDateCP = PIn.Date(table.Rows[i]["CheckDate"].ToString());
						claimProcNum = PIn.Long(table.Rows[i]["ClaimProcNum"].ToString());
						command = "SELECT ClaimProcNum FROM claimproc WHERE ClaimProcNum=" + claimProcNum.ToString();
						listClaimProcNums = Database.GetListLong(command);
						if (modeCur == DbmMode.Fix)
						{
							command = "UPDATE claimproc SET DateCP=" + POut.Date(newDateCP)
								+ " WHERE ClaimProcNum=" + claimProcNum.ToString();
							Database.ExecuteNonQuery(command);
							listClaimProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.ClaimProc,
								DbmLogActionType.Update, methodName, "Updated DateCP from " + oldDateCP + " to " + newDateCP + ".")));
						}
						else
						{//Breakdown
							strBuild.AppendLine(patientName + ", " + codeSent + ", " + dateService.ToShortDateString() + ", " + oldDateCP.ToShortDateString()
								+ ", " + newDateCP.ToShortDateString());
						}
					}
					int numberFixed = table.Rows.Count;
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						if (modeCur == DbmMode.Fix)
						{
							log = "Claim payments with mismatched dates fixed:" + " " + numberFixed.ToString() + "\r\n";
						}
						else
						{//Breakdown
							strBuild.AppendLine("Claim payments with mismatched dates found:" + " " + numberFixed.ToString());
							log = strBuild.ToString();
						}
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDeleteDuplicateEstimateForSameInsPlan(bool verbose, DbmMode modeCur)
		{
			string command;

			//Get all the claimproc estimates that already have a claimproc marked as received from the same InsPlan.
			command = "SELECT cp.ClaimProcNum FROM claimproc cp USE KEY(PRIMARY)"
			+ " INNER JOIN claimproc cp2 ON cp2.PatNum=cp.PatNum"
			+ " AND cp2.PlanNum=cp.PlanNum"    //The same insurance plan
			+ " AND cp2.InsSubNum=cp.InsSubNum"//for the same subscriber
			+ " AND cp2.ProcNum=cp.ProcNum"    //for the same procedure.
			+ " AND cp2.Status=" + POut.Int((int)ClaimProcStatus.Received)
			+ " WHERE cp.Status=" + POut.Int((int)ClaimProcStatus.Estimate)
			+ " AND cp.ClaimNum=0";//Make sure the estimate is not already attached to a claim somehow.


			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Duplicate ClaimProc estimates for the same InsPlan found: " + table.Rows.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					if (table.Rows.Count > 0)
					{
						string inCommand = string.Join(",", table.Select().Select(x => PIn.Long(x["ClaimProcNum"].ToString())));
						command = "SELECT ClaimProcNum FROM claimproc WHERE ClaimProcNum IN (" + inCommand + ")";
						List<long> listClaimProcNums = Database.GetListLong(command);
						command = "DELETE FROM claimproc WHERE ClaimProcNum IN (" + inCommand + ")";
						Database.ExecuteNonQuery(command);
						listClaimProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.ClaimProc,
							DbmLogActionType.Delete, methodName, "Deleted ClaimProc from ClaimProcDeleteDuplicateEstimateForSameInsPlan.")));
					}
					if (table.Rows.Count > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Duplicate ClaimProc estimates for the same InsPlan deleted: " + table.Rows.Count + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDeleteInvalidAdjustments(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT ClaimProcNum FROM claimproc WHERE claimproc.ClaimNum=0 "
				+ "AND NOT EXISTS(SELECT PlanNum FROM insplan WHERE insplan.PlanNum=claimproc.PlanNum) "
				+ "AND claimproc.Status=" + POut.Int((int)ClaimProcStatus.Adjustment);
			List<long> listClaimProcNums = Database.GetListLong(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listClaimProcNums.Count;
					if (numFound > 0 || verbose)
					{
						log += "Claimproc adjustments found with invalid PlanNum:" + " " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "DELETE FROM claimproc WHERE claimproc.ClaimNum=0 "
						+ "AND NOT EXISTS(SELECT PlanNum FROM insplan WHERE insplan.PlanNum=claimproc.PlanNum) "
						+ "AND claimproc.Status=" + POut.Int((int)ClaimProcStatus.Adjustment);
					long numberFixed = Database.ExecuteNonQuery(command);
					listClaimProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Delete, methodName, "Deleted ClaimProc from ClaimProcDeleteInvalidAdjustments.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Claimproc adjustments deleted due to invalid PlanNum:" + " " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasPatNum = true)]
		public static string ClaimProcDeleteWithInvalidClaimNum(bool verbose, DbmMode modeCur, long patNum = 0)
		{
			string log = "";
			string patWhere = (patNum > 0 ? "AND claimproc.PatNum=" + POut.Long(patNum) + " " : "");
			string command = "SELECT ClaimProcNum FROM claimproc WHERE claimproc.ClaimNum!=0 "
						+ patWhere
						+ "AND NOT EXISTS(SELECT * FROM claim WHERE claim.ClaimNum=claimproc.ClaimNum) "
						+ "AND claimproc.InsPayAmt=0 AND claimproc.WriteOff=0";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = Database.GetListLong(command).Count;
					if (numFound > 0 || verbose)
					{
						log += "Claimprocs found with invalid ClaimNum: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					List<long> listClaimProcNums = Database.GetListLong(command);
					command = "DELETE FROM claimproc WHERE claimproc.ClaimNum!=0 "
						+ patWhere
						+ "AND NOT EXISTS(SELECT * FROM claim WHERE claim.ClaimNum=claimproc.ClaimNum) "
						+ "AND claimproc.InsPayAmt=0 AND claimproc.WriteOff=0";
					long numberFixed = Database.ExecuteNonQuery(command);
					listClaimProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Delete, methodName, "Deleted ClaimProc from ClaimProcDeleteWithInvalidClaimNum.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Claimprocs deleted due to invalid ClaimNum: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDeleteMismatchPatNum(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT ClaimProcNum FROM claimproc "
				+ "INNER JOIN procedurelog ON procedurelog.ProcNum=claimproc.ProcNum "
				+ "WHERE claimproc.ProcNum>0 "
				+ "AND claimproc.PatNum!=procedurelog.PatNum "
				+ "AND claimproc.InsPayAmt=0 "
				+ "AND(claimproc.WriteOff=0 "
				+ "OR(claimproc.Status=" + (int)ClaimProcStatus.CapEstimate + " "
				+ "AND claimproc.WriteOff=procedurelog.ProcFee AND procedurelog.ProcStatus IN(" + (int)ProcStat.TP + "," + (int)ProcStat.TPi + ")))";
			List<long> listClaimProcNums = Database.GetListLong(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listClaimProcNums.Count;
					if (numFound > 0 || verbose)
					{
						log += "Claimprocs found with PatNum that doesn't match the procedure PatNum:" + " " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					if (listClaimProcNums.Count > 0)
					{
						List<DbmLog> listDbmLogs = new List<DbmLog>();
						string methodName = MethodBase.GetCurrentMethod().Name;
						command = "DELETE FROM claimproc WHERE ClaimProcNum IN(" + string.Join(",", listClaimProcNums) + ")";
						long numberFixed = Database.ExecuteNonQuery(command);
						listClaimProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.ClaimProc,
							DbmLogActionType.Delete, methodName, "Deleted ClaimProc from ClaimProcDeleteMismatchPatNum.")));
						if (numberFixed > 0 || verbose)
						{
							log += "Claimprocs deleted due to PatNum not matching the procedure PatNum:" + " "
								+ numberFixed.ToString() + "\r\n";
							Crud.DbmLogCrud.InsertMany(listDbmLogs);
						}
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDeleteEstimateWithInvalidProcNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT ClaimProcNum FROM claimproc WHERE ProcNum>0 "
				+ "AND Status=" + POut.Int((int)ClaimProcStatus.Estimate) + " "
				+ "AND NOT EXISTS(SELECT * FROM procedurelog "
				+ "WHERE claimproc.ProcNum=procedurelog.ProcNum)";
			List<long> listClaimProcNums = Database.GetListLong(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listClaimProcNums.Count;
					if (numFound > 0 || verbose)
					{
						log += "Estimates found for procedures that no longer exist: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					//These seem to pop up quite regularly due to the program forgetting to delete them
					command = "DELETE FROM claimproc WHERE ProcNum>0 "
						+ "AND Status=" + POut.Int((int)ClaimProcStatus.Estimate) + " "
						+ "AND NOT EXISTS(SELECT * FROM procedurelog "
						+ "WHERE claimproc.ProcNum=procedurelog.ProcNum)";
					long numberFixed = Database.ExecuteNonQuery(command);
					listClaimProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Delete, methodName, "Deleted ClaimProc from ClaimProcDeleteEstimateWithInvalidProcNum.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Estimates deleted for procedures that no longer exist: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDeleteCapEstimateWithProcComplete(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT ClaimProcNum FROM claimproc WHERE ProcNum>0 "
				+ "AND Status=" + POut.Int((int)ClaimProcStatus.CapEstimate) + " "
				+ "AND EXISTS("
					+ "SELECT * FROM procedurelog "
					+ "WHERE claimproc.ProcNum=procedurelog.ProcNum "
					+ "AND procedurelog.ProcStatus=" + POut.Int((int)ProcStat.C)
				+ ")";
			List<long> listClaimProcNums = Database.GetListLong(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listClaimProcNums.Count;
					if (numFound > 0 || verbose)
					{
						log += "Capitation estimates found for completed procedures: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "DELETE FROM claimproc WHERE ProcNum>0 "
						+ "AND Status=" + POut.Int((int)ClaimProcStatus.CapEstimate) + " "
						+ "AND EXISTS("
							+ "SELECT * FROM procedurelog "
							+ "WHERE claimproc.ProcNum=procedurelog.ProcNum "
							+ "AND procedurelog.ProcStatus=" + POut.Int((int)ProcStat.C)
						+ ")";
					long numberFixed = Database.ExecuteNonQuery(command);
					listClaimProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Delete, methodName, "Deleted ClaimProc from ClaimProcDeleteCapEstimateWithProcComplete.")));
					if (numberFixed > 0 || verbose)
					{
						log += "Capitation estimates deleted for completed procedures: " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcEstNoBillIns(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT * FROM claimproc WHERE NoBillIns=1 AND InsPayEst !=0";
			List<ClaimProc> listClaimProcs = Crud.ClaimProcCrud.SelectMany(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listClaimProcs.Count;
					if (numFound > 0 || verbose)
					{
						log += "Claimprocs found with non-zero estimates marked NoBillIns: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					//This is just estimate info, regardless of the claimproc status, so totally safe.
					command = "UPDATE claimproc SET InsPayEst=0 WHERE NoBillIns=1 AND InsPayEst !=0";
					long numberFixed = Database.ExecuteNonQuery(command);
					listClaimProcs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimProcNum, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Update, methodName, "Updated InsPayEst from " + x.InsPayEst + " to 0 from ClaimProcEstNoBillIns.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Claimproc estimates set to zero because marked NoBillIns: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcEstWithInsPaidAmt(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = @"SELECT * FROM claimproc WHERE InsPayAmt > 0 AND ClaimNum=0 AND Status=6";
			List<ClaimProc> listClaimProcs = Crud.ClaimProcCrud.SelectMany(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listClaimProcs.Count;
					if (numFound > 0 || verbose)
					{
						log += "ClaimProc estimates with InsPaidAmt > 0 found: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					//The InsPayAmt is already being ignored due to the status of the claimproc.  So changing its value is harmless.
					command = @"UPDATE claimproc SET InsPayAmt=0 WHERE InsPayAmt > 0 AND ClaimNum=0 AND Status=6";
					long numberFixed = Database.ExecuteNonQuery(command);
					listClaimProcs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimProcNum, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Update, methodName, "Updated InsPayAmt from " + x.InsPayEst + " to 0 from ClaimProcEstWithInsPaidAmt.")));
					if (numberFixed > 0 || verbose)
					{
						log += "ClaimProc estimates with InsPaidAmt > 0 fixed: " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcPatNumMissing(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT ClaimProcNum FROM claimproc WHERE PatNum=0 AND InsPayAmt=0 AND WriteOff=0";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = Database.GetListLong(command).Count;
					if (numFound > 0 || verbose)
					{
						log += "ClaimProcs with missing patnums found: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					List<long> listClaimProcNums = Database.GetListLong(command);
					command = "DELETE FROM claimproc WHERE PatNum=0 AND InsPayAmt=0 AND WriteOff=0";
					long numberFixed = Database.ExecuteNonQuery(command);
					listClaimProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Delete, methodName, "Deleted ClaimProc from ClaimProcPatNumMissing.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "ClaimProcs with missing patnums fixed: " + numberFixed + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcProvNumMissing(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT ClaimProcNum FROM claimproc WHERE ProvNum=0 AND Status=" + POut.Int((int)ClaimProcStatus.Estimate);
			List<long> listClaimProcNums = Database.GetListLong(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listClaimProcNums.Count;
					if (numFound > 0 || verbose)
					{
						log += "ClaimProcs with missing provnums found: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					//If estimate, set to default prov (doesn't affect finances)
					command = "UPDATE claimproc SET ProvNum=" + Preferences.GetString(PreferenceName.PracticeDefaultProv) + " WHERE ProvNum=0 AND Status=" + POut.Int((int)ClaimProcStatus.Estimate);
					long numberFixed = Database.ExecuteNonQuery(command);
					listClaimProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Update, methodName, "Updated ProvNum from 0 to " + Preferences.GetString(PreferenceName.PracticeDefaultProv) + ".")));
					if (numberFixed > 0 || verbose)
					{
						log += "ClaimProcs with missing provnums fixed: " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					//create a dummy provider (using helper function in Providers.cs)
					//change provnum to the dummy prov (something like Providers.GetDummy())
					//Provider dummy=new Provider();
					//dummy.Abbr="Dummy";
					//dummy.FName="Dummy";
					//dummy.LName="Provider";
					//Will get to this soon.
					//01-17-2011 No fix yet. This has not caused issues except for notifying users.
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClaimProcPreauthNotMatchClaim(bool verbose, DbmMode modeCur)
		{
			string command = @"SELECT claim.PatNum,claim.DateService,claimproc.ProcDate,claimproc.CodeSent,claimproc.FeeBilled 
				FROM claimproc,claim 
				WHERE claimproc.ClaimNum=claim.ClaimNum
				AND claim.ClaimType='PreAuth'
				AND claimproc.Status!=2";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "ClaimProcs for preauths with status not preauth" + ": " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   " + "Manual fix needed.  Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", " + "including" + ":\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							Patient pat = Patients.GetPat(PIn.Long(table.Rows[i]["PatNum"].ToString()));
							log += "   Patient: #" + pat.PatNum.ToString() + ":" + pat.GetNameFirstOrPrefL()
								+ " ClaimDate: " + PIn.Date(table.Rows[i]["DateService"].ToString()).ToShortDateString()
								+ " ProcDate: " + PIn.Date(table.Rows[i]["ProcDate"].ToString()).ToShortDateString()
								+ " Code: " + table.Rows[i]["CodeSent"].ToString() + "\r\n";
						}
						log += "   They need to be fixed manually." + "\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>We are only checking mismatched statuses if claim is marked as received.</summary>
		[DbmMethodAttr(HasBreakDown = true, HasPatNum = true)]
		public static string ClaimProcStatusNotMatchClaim(bool verbose, DbmMode modeCur, long patNum = 0)
		{
			string patWhere = (patNum > 0 ? "AND claimproc.PatNum=" + POut.Long(patNum) + " " : "");
			string command = @"SELECT claim.PatNum,claim.DateService,claimproc.ProcDate,claimproc.CodeSent,claimproc.FeeBilled
					FROM claimproc,claim
					WHERE claimproc.ClaimNum=claim.ClaimNum
					AND claim.ClaimStatus='R'
					AND claimproc.Status=" + POut.Int((int)ClaimProcStatus.NotReceived) + " "
					+ patWhere;
			//If a claim is re-sent after being received, the claimprocs Status will be Received but the claim will be Sent, which is to be expected, so we
			//no longer want to flag them as being a DBM issue.  They will show on the unreceived claims report and the user can go manually change the
			//claim status to received if it really is a mistake caused by a user manually changing the claim or claimproc statuses.
			//+"OR (claim.ClaimStatus!='R' AND claimproc.Status="+POut.Int((int)ClaimProcStatus.Received)+"))";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "ClaimProcs with status not matching claim found: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   " + "Manual fix needed.  Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", " + "including" + ":\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							Patient pat = Patients.GetPat(PIn.Long(table.Rows[i]["PatNum"].ToString()));
							log += "   Patient: #" + pat.PatNum.ToString() + ":" + pat.GetNameFirstOrPrefL()
								+ " ClaimDate: " + PIn.Date(table.Rows[i]["DateService"].ToString()).ToShortDateString()
								+ " ProcDate: " + PIn.Date(table.Rows[i]["ProcDate"].ToString()).ToShortDateString()
								+ " Code: " + table.Rows[i]["CodeSent"].ToString()
								+ " FeeBilled: " + PIn.Double(table.Rows[i]["FeeBilled"].ToString()).ToString("F") + "\r\n";
						}
						log += "   They need to be fixed manually." + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcTotalPaymentWithInvalidDate(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT claimproc.ClaimProcNum,claimproc.ProcDate,claim.DateService FROM claimproc,claim"
				+ " WHERE claimproc.ProcNum=0"//Total payments
				+ " AND claimproc.ProcDate < " + POut.Date(new DateTime(1880, 1, 1))//which have invalid dates
				+ " AND claimproc.ClaimNum=claim.ClaimNum"
				+ " AND claim.DateService > " + POut.Date(new DateTime(1880, 1, 1));//but have valid date of service on the claim
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Total claim payments with invalid date found" + ": " + table.Rows.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					if (table.Rows.Count > 0)
					{
						command = "UPDATE claimproc,claim SET claimproc.ProcDate=claim.DateService"//Resets date for total payments to DateService
							+ " WHERE claimproc.ProcNum=0"//Total payments
							+ " AND claimproc.ProcDate < " + POut.Date(new DateTime(1880, 1, 1))//which have invalid dates
							+ " AND claimproc.ClaimNum=claim.ClaimNum"
							+ " AND claim.DateService > " + POut.Date(new DateTime(1880, 1, 1));//but have valid date of service on the claim
						Database.ExecuteNonQuery(command);
						table.Select().ToList().ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, PIn.Long(x["ClaimProcNum"].ToString()),
							DbmLogFKeyType.ClaimProc, DbmLogActionType.Update, methodName, "Updated ProcDate from " + x["ProcDate"].ToString()
							+ " to " + x["DateService"].ToString() + " from ClaimProcTotalPaymentWithInvalidDate.")));
					}
					int numberFixed = table.Rows.Count;
					if (numberFixed > 0 || verbose)
					{
						log += "Total claim payments with invalid date fixed" + ": " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcUpdateStatusWithInvalidClaim(bool verbose, DbmMode modeCur)
		{
			string command = @$"SELECT ClaimProcNum 
				FROM claimproc 
				WHERE Status={POut.Int((int)ClaimProcStatus.NotReceived)}
				AND ClaimNum=0
				AND InsPayAmt=0 
				AND WriteOff=0";
			List<long> listClaimProcNums = Database.GetListLong(command);
			string log = "";
			if (listClaimProcNums.Count == 0 && !verbose)
			{
				return log;
			}
			switch (modeCur)
			{
				case DbmMode.Check:
					log += "ClaimProcs with status" +
						" '" + ClaimProcStatus.NotReceived.GetDescription() + "' " +
						"found where no claim is attached:" + " " + POut.Long(listClaimProcNums.Count);
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					if (listClaimProcNums.Count > 0)
					{
						command = $"UPDATE claimproc SET Status={POut.Enum(ClaimProcStatus.Estimate)} " +
							$"WHERE ClaimProcNum IN(" + string.Join(",", listClaimProcNums) + ")";
						Database.ExecuteNonQuery(command);
						listClaimProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.ClaimProc,
							DbmLogActionType.Update, methodName, $"Updated status of ClaimProc from '{ClaimProcStatus.NotReceived.GetDescription()}' to " +
							$"'{ClaimProcStatus.Estimate.GetDescription()}' where no claim was attached.")));
					}
					if (listClaimProcNums.Count > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "ClaimProcs with invalid claims set to" + " " + ClaimProcStatus.Estimate.GetDescription() +
							": " + POut.Long(listClaimProcNums.Count);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true, HasPatNum = true)]
		public static string ClaimProcWithInvalidClaimNum(bool verbose, DbmMode modeCur, long patNum = 0)
		{
			string log = "";
			string command;
			string patWhere = (patNum > 0 ? "AND claimproc.PatNum=" + POut.Long(patNum) + " " : "");
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM claimproc "
						+ "WHERE claimproc.ClaimNum!=0 "
						+ patWhere
						+ "AND NOT EXISTS(SELECT * FROM claim WHERE claim.ClaimNum=claimproc.ClaimNum) "
						+ "AND (claimproc.InsPayAmt!=0 OR claimproc.WriteOff!=0)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Claimprocs found with invalid ClaimNum:" + " " + numFound + "\r\n";
						log += "   " + "Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:  //No DB changes made, fix splits off below.
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					//We can't touch those claimprocs because it would mess up the accounting.
					//Create dummy claims for all claimprocs with invalid ClaimNums if those claimprocs have amounts entered in the InsPayAmt or Writeoff 
					//columns, otherwise you could not delete the procedure or create a new claim.
					command = "SELECT * FROM claimproc "
						+ "WHERE claimproc.ClaimNum!=0 "
						+ patWhere
						+ "AND NOT EXISTS(SELECT * FROM claim WHERE claim.ClaimNum=claimproc.ClaimNum) "
						+ "AND (claimproc.InsPayAmt!=0 OR claimproc.WriteOff!=0) "
						+ "GROUP BY claimproc.ClaimNum";
					DataTable table = Database.ExecuteDataTable(command);
					List<ClaimProc> cpList = Crud.ClaimProcCrud.TableToList(table);
					Claim claim;
					if (modeCur == DbmMode.Fix)
					{
						for (int i = 0; i < cpList.Count; i++)
						{
                            claim = new Claim
                            {
                                ClaimNum = cpList[i].ClaimNum,
                                PatNum = cpList[i].PatNum,
                                ClinicNum = cpList[i].ClinicNum
                            };
                            if (cpList[i].Status == ClaimProcStatus.Received)
							{
								claim.ClaimStatus = "R";//Status received because we know it's been paid on and the claimproc status is received
							}
							else
							{
								claim.ClaimStatus = "W";
							}
							claim.PlanNum = cpList[i].PlanNum;
							claim.InsSubNum = cpList[i].InsSubNum;
							claim.ProvTreat = cpList[i].ProvNum;
							//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
							claim.SecUserNumEntry = Security.CurrentUser.Id;
							Crud.ClaimCrud.Insert(claim, true);//Allows us to use a primary key that was "used".
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, claim.ClaimNum, DbmLogFKeyType.Claim, DbmLogActionType.Insert,
								methodName, "Added new claim from ClaimProcWithInvalidClaimNum."));
						}
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						if (cpList.Count > 0 || verbose)
						{
							log += "Claimprocs with invalid ClaimNums fixed:" + " " + cpList.Count + "\r\n";
						}
					}
					if (modeCur == DbmMode.Breakdown && cpList.Count > 0)
					{
						StringBuilder strBuild = new StringBuilder();
						strBuild.AppendLine("Claims will be created due to claimprocs with invalid ClaimNums for patients:");
						//Get limited data for all patients that will have a new claim created.
						List<Patient> listPatients = Patients.GetLimForPats(cpList.Select(x => x.PatNum).ToList());
						foreach (Patient pat in listPatients)
						{
							strBuild.AppendLine($"{pat.PatNum} - {pat.GetNameFL()}");
						}
						log += strBuild.ToString();
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasPatNum = true)]
		public static string ClaimProcWithInvalidClaimPaymentNum(bool verbose, DbmMode modeCur, long patNum = 0)
		{
			string log = "";
			string patWhere = (patNum > 0 ? "AND claimproc.PatNum=" + POut.Long(patNum) + " " : "");
			string command = @"SELECT * FROM claimproc WHERE claimpaymentnum !=0 "
				+ patWhere
				+ "AND NOT EXISTS(SELECT * FROM claimpayment WHERE claimpayment.ClaimPaymentNum=claimproc.ClaimPaymentNum)";
			List<ClaimProc> listClaimProcs = Crud.ClaimProcCrud.SelectMany(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listClaimProcs.Count;
					if (numFound > 0 || verbose)
					{
						log += "ClaimProcs with with invalid ClaimPaymentNumber found: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					//slightly dangerous.  User will have to create ins check again.  But does not alter financials.
					command = @"UPDATE claimproc SET ClaimPaymentNum=0 WHERE claimpaymentnum !=0 "
						+ patWhere
						+ "AND NOT EXISTS(SELECT * FROM claimpayment WHERE claimpayment.ClaimPaymentNum=claimproc.ClaimPaymentNum)";
					long numberFixed = Database.ExecuteNonQuery(command);
					listClaimProcs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimProcNum, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Update, methodName, "Updated ClaimPaymentNum from " + x.ClaimPaymentNum + " to 0.")));
					if (numberFixed > 0 || verbose)
					{
						log += "ClaimProcs with with invalid ClaimPaymentNumber fixed: " + numberFixed.ToString() + "\r\n";
						//Tell user what items to create ins checks for?
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcWithInvalidPayPlanNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = @"SELECT * FROM claimproc WHERE PayPlanNum>0 AND PayPlanNum NOT IN(SELECT PayPlanNum FROM payplan)";
			List<ClaimProc> listClaimProcs = Crud.ClaimProcCrud.SelectMany(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listClaimProcs.Count;
					if (numFound > 0 || verbose)
					{
						log += "ClaimProcs with with invalid PayPlanNum found" + ": " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					//safe, if the user wants to attach the claimprocs to a payplan for tracking the ins payments they would just need to attach to a valid payplan
					command = @"UPDATE claimproc SET PayPlanNum=0 WHERE PayPlanNum>0 AND PayPlanNum NOT IN(SELECT PayPlanNum FROM payplan)";
					long numFixed = Database.ExecuteNonQuery(command);
					listClaimProcs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimProcNum, DbmLogFKeyType.ClaimProc,
						 DbmLogActionType.Update, methodName, "Updated PayPlanNum from " + x.PayPlanNum + " to 0.")));
					if (numFixed > 0 || verbose)
					{
						log += "ClaimProcs with with invalid PayPlanNum fixed" + ": " + numFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcWithInvalidProvNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT * FROM claimproc WHERE ProvNum > 0 AND ProvNum NOT IN (SELECT ProvNum FROM provider)";
			List<ClaimProc> listClaimProcs = Crud.ClaimProcCrud.SelectMany(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listClaimProcs.Count;
					if (numFound > 0 || verbose)
					{
						log += "Claimprocs with invalid ProvNum found" + ": " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "UPDATE claimproc SET ProvNum=" + POut.Long(Preferences.GetLong(PreferenceName.PracticeDefaultProv)) +
							" WHERE ProvNum > 0 AND ProvNum NOT IN (SELECT ProvNum FROM provider)";
					long numFixed = Database.ExecuteNonQuery(command);
					listClaimProcs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimProcNum, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Update, methodName,
						"Updated ProvNum from " + x.ProvNum + " to " + POut.String(Preferences.GetLong(PreferenceName.PracticeDefaultProv).ToString()))));
					if (numFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Claimprocs with invalid ProvNum fixed" + ": " + numFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcWithInvalidSubNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT ClaimProcNum FROM claimproc WHERE claimproc.InsSubNum > 0 AND claimproc.Status=" + POut.Int((int)ClaimProcStatus.Estimate)
				+ " AND claimproc.InsSubNum NOT IN (SELECT inssub.InsSubNum FROM inssub)";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = Database.GetListLong(command).Count;
					if (numFound > 0 || verbose)
					{
						log += "Claimprocs with invalid InsSubNum found" + ": " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					List<long> listClaimProcNums = Database.GetListLong(command);
					command = "DELETE FROM claimproc WHERE claimproc.InsSubNum > 0 AND claimproc.Status=" + POut.Int((int)ClaimProcStatus.Estimate)
						+ " AND claimproc.InsSubNum NOT IN (SELECT inssub.InsSubNum FROM inssub)";
					long numFixed = Database.ExecuteNonQuery(command);
					listClaimProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Delete, methodName, "Deleted ClaimProc from ClaimProcWithInvalidSubNum.")));
					if (numFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Claimprocs with invalid InsSubNum fixed" + ": " + numFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClaimProcWriteOffNegative(bool verbose, DbmMode modeCur)
		{
			string command = @"SELECT patient.LName,patient.FName,patient.MiddleI,claimproc.CodeSent,procedurelog.ProcFee,procedurelog.ProcDate,claimproc.WriteOff
					FROM claimproc 
					LEFT JOIN patient ON claimproc.PatNum=patient.PatNum
					LEFT JOIN procedurelog ON claimproc.ProcNum=procedurelog.ProcNum 
					WHERE claimproc.WriteOff<0
					AND claimproc.IsTransfer=0";//The income transfer tool creates claimprocs with negative writeoffs. These are valid.
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "Negative writeoffs found: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   " + "Manual fix needed.  Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						string patientName;
						string codeSent;
						decimal writeOff;
						decimal procFee;
						DateTime procDate;
						log += "List of patients with procedures that have negative writeoffs:\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							patientName = table.Rows[i]["LName"].ToString() + ", " + table.Rows[i]["FName"].ToString() + " " + table.Rows[i]["MiddleI"].ToString();
							codeSent = table.Rows[i]["CodeSent"].ToString();
							procDate = PIn.Date(table.Rows[i]["ProcDate"].ToString());
							writeOff = PIn.Decimal(table.Rows[i]["WriteOff"].ToString());
							procFee = PIn.Decimal(table.Rows[i]["ProcFee"].ToString());
							log += patientName + " " + codeSent + " fee:" + procFee.ToString("c") + " date:" + procDate.ToShortDateString() + " writeoff:" + writeOff.ToString("c") + "\r\n";
						}
						log += "Go to the patients listed above and manually correct the writeoffs.\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(IsCanada = true)]
		public static string CanadaClaimProcForWrongPatient(bool verbose, DbmMode modeCur)
		{
			if (!CultureInfo.CurrentCulture.Name.EndsWith("CA"))
			{
				return "Skipped. Local computer region must be set to Canada to run.";
			}
			//Look at the comments for claimProc.DateEntry, if not set then the claimProc has no financial meaning yet.
			string command = @"SELECT claimproc.*
				FROM claimproc 
				INNER JOIN claim ON claim.ClaimNum=claimproc.ClaimNum
				WHERE (claimproc.PatNum!=claim.PatNum)";
			List<ClaimProc> listClaimProcs = Crud.ClaimProcCrud.SelectMany(command);
			if (listClaimProcs.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (listClaimProcs.Count > 0 || verbose)
					{
						log += "Claimprocs associated to wrong patient found" + ": " + listClaimProcs.Count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					foreach (ClaimProc claimProc in listClaimProcs)
					{
						ClaimProcs.Delete(claimProc);
						listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, claimProc.ClaimProcNum, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Delete, methodName, "Deleted ClaimProc from CanadaClaimProcForWrongPatient."));
					}
					Crud.DbmLogCrud.InsertMany(listDbmLogs);
					log += "Claimprocs associated to wrong patient fixed" + ":" + listClaimProcs.Count;
					break;
			}
			return log;
		}

		#endregion ClaimProc----------------------------------------------------------------------------------------------------------------------------
		#region Clearinghouse---------------------------------------------------------------------------------------------------------------------------
		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClearinghouseInvalidFormat(bool verbose, DbmMode modeCur)
		{
			int formatEnumCount = Enum.GetNames(typeof(ElectronicClaimFormat)).Length - 1;
			string command = "SELECT clearinghouse.Description,COALESCE(clinic.Abbr,'Unassigned') Abbr "
				+ "FROM clearinghouse "
				+ "LEFT JOIN clinic on clinic.ClinicNum=clearinghouse.ClinicNum "
				+ "WHERE EFormat>" + formatEnumCount;
			DataTable table = Database.ExecuteDataTable(command);
			int numFound = table.Rows.Count;
			if (numFound == 0 && !verbose)
			{
				return "";
			}
			string log = "Clearinghouses with invalid Format found:" + " " + numFound;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (numFound > 0)
					{
						log += "\r\n   " + "Manual fix needed.  Double click to see a break down.";
					}
					break;
				case DbmMode.Breakdown:
					if (numFound > 0)
					{
						log += ", " + "including" + ":\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "	" + table.Rows[i]["Description"].ToString();
							if (PrefC.HasClinicsEnabled)
							{
								log += " " + "for Clinic" + ": " + table.Rows[i]["Abbr"].ToString();
							}
							log += "\r\n";
						}
						log += "They need to be fixed manually.";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClearinghouseInvalidCommBridge(bool verbose, DbmMode modeCur)
		{
			int commBridgeEnumCount = Enum.GetNames(typeof(EclaimsCommBridge)).Length - 1;
			string command = "SELECT clearinghouse.Description,COALESCE(clinic.Abbr,'Unassigned') Abbr "
				+ "FROM clearinghouse "
				+ "LEFT JOIN clinic on clinic.ClinicNum=clearinghouse.ClinicNum "
				+ "WHERE CommBridge>" + commBridgeEnumCount;
			DataTable table = Database.ExecuteDataTable(command);
			int numFound = table.Rows.Count;
			if (numFound == 0 && !verbose)
			{
				return "";
			}
			string log = "Clearinghouses with invalid CommBridge found:" + " " + numFound;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (numFound > 0)
					{
						log += "\r\n   " + "Manual fix needed.  Double click to see a break down.";
					}
					break;
				case DbmMode.Breakdown:
					if (numFound > 0)
					{
						log += ", " + "including" + ":\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "	" + table.Rows[i]["Description"].ToString();
							if (PrefC.HasClinicsEnabled)
							{
								log += " " + "for Clinic" + ": " + table.Rows[i]["Abbr"].ToString();
							}
							log += "\r\n";
						}
						log += "They need to be fixed manually.";
					}
					break;
			}
			return log;
		}
		#endregion
		#region ClockEvent, ComputerPref, Deposit, Disease, Document--------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string ClockEventInFuture(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = @"SELECT COUNT(*) FROM clockevent WHERE TimeDisplayed1 > " + DbHelper.Now() + "+INTERVAL 15 MINUTE";
					int numFound = PIn.Int(Database.ExecuteString(command));
					command = @"SELECT COUNT(*) FROM clockevent WHERE TimeDisplayed2 > " + DbHelper.Now() + "+INTERVAL 15 MINUTE";
					numFound += PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Time card entries invalid" + ": " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = @"UPDATE clockevent SET TimeDisplayed1=" + DbHelper.Now() + " WHERE TimeDisplayed1 > " + DbHelper.Now() + "+INTERVAL 15 MINUTE";
					long numberFixed = Database.ExecuteNonQuery(command);
					command = @"UPDATE clockevent SET TimeDisplayed2=" + DbHelper.Now() + " WHERE TimeDisplayed2 > " + DbHelper.Now() + "+INTERVAL 15 MINUTE";
					numberFixed += Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Future timecard entry times fixed" + ": " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ComputerPrefDuplicates(bool verbose, DbmMode modeCur)
		{
			//Min may not be the oldest when using random primary keys, but we have to pick one.
			string command = "SELECT MIN(ComputerPrefNum) ComputerPrefNum, ComputerName "
				+ "FROM computerpref "
				+ "GROUP BY ComputerName "
				+ "HAVING COUNT(*)>1 ";
			DataTable tableDupComputerPrefs = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (tableDupComputerPrefs.Rows.Count > 0 || verbose)
					{
						log += "ComputerPref duplicate computer name entries found:" + " " + tableDupComputerPrefs.Rows.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					long numberFixed = 0;
					if (tableDupComputerPrefs.Rows.Count > 0)
					{
						command = "DELETE FROM computerpref WHERE ComputerPrefNum NOT IN ("
							+ string.Join(",", tableDupComputerPrefs.Select().Select(x => POut.Long(PIn.Long(x["ComputerPrefNum"].ToString())))) + ") "
							+ "AND ComputerName IN ("
							+ string.Join(",", tableDupComputerPrefs.Select().Select(x => $"'{POut.String(PIn.String(x["ComputerName"].ToString()))}'")) + ")";
						numberFixed = Database.ExecuteNonQuery(command);
					}
					if (numberFixed > 0 || verbose)
					{
						log += "ComputerPref duplicate computer name entries deleted:" + " " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Finds deposits where there are attached payments and the deposit amount does not equal the sum of the attached payment amounts.</summary>
		[DbmMethodAttr(HasBreakDown = true)]
		public static string DepositsWithIncorrectAmount(bool verbose, DbmMode modeCur)
		{
			//only deposits with payments attached (INNER JOIN) where sum of payment amounts don't match the deposit amount
			//deposits with positive amount but no payments attached (LEFT JOIN...) is handled in a separate DBM above
			string command = @"SELECT deposit.DepositNum,deposit.Amount,deposit.DateDeposit,SUM(payments.amt) _sum
				FROM deposit
				INNER JOIN (
					SELECT payment.DepositNum,payment.PayAmt amt
					FROM payment
					UNION ALL
					SELECT claimpayment.DepositNum,claimpayment.CheckAmt amt
					FROM claimpayment
				) payments ON payments.DepositNum=deposit.DepositNum
				GROUP BY deposit.DepositNum
				HAVING ROUND(_sum,2) != ROUND(deposit.Amount,2)
				ORDER BY deposit.DateDeposit,deposit.DepositNum";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			//There is something to report OR the user has verbose mode on.
			string log = "Deposit sums found incorrect: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   " + "Manual fix needed.  Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", " + "including" + ":\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							DateTime date = PIn.Date(table.Rows[i]["DateDeposit"].ToString());
							Double oldval = PIn.Double(table.Rows[i]["Amount"].ToString());
							Double newval = PIn.Double(table.Rows[i]["_sum"].ToString());
							log += "   " + "Deposit Date: " + date.ToShortDateString()
								+ ", " + "Current Sum: " + oldval.ToString("c")
								+ ", " + "Expected Sum:" + newval.ToString("c") + "\r\n";
						}
						log += "   They need to be fixed manually." + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string DiseaseWithInvalidDiseaseDef(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = @"SELECT DiseaseNum,DiseaseDefNum FROM disease WHERE DiseaseDefNum NOT IN(SELECT DiseaseDefNum FROM diseasedef)";
			DataTable table = Database.ExecuteDataTable(command);
			int numFound = table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
					if (numFound > 0 || verbose)
					{
						log += "Problems with invalid references found" + ": " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					if (numFound > 0 || verbose)
					{
						log += "Problems with invalid references found" + ": " + numFound + "\r\n";
					}
					if (numFound > 0)
					{
						//Check to see if there is already a diseasedef called UNKNOWN PROBLEM.
						command = "SELECT DiseaseDefNum FROM diseasedef WHERE DiseaseName='UNKNOWN PROBLEM'";
						long diseaseDefNum = Database.ExecuteLong(command);
						if (diseaseDefNum == 0)
						{
                            //Create a new DiseaseDef called UNKNOWN PROBLEM.
                            ProblemDefinition diseaseDef = new ProblemDefinition
                            {
                                Description = "UNKNOWN PROBLEM",
                                IsHidden = false
                            };
                            diseaseDefNum = ProblemDefinitions.Insert(diseaseDef);
						}
						//Update the disease table.
						command = "UPDATE disease SET DiseaseDefNum=" + POut.Long(diseaseDefNum) + " WHERE DiseaseNum IN("
							+ string.Join(",", table.Select().Select(x => PIn.Long(x["DiseaseNum"].ToString()))) + ")";
						Database.ExecuteNonQuery(command);
						log += "All invalid references have been attached to the problem called" + " UNKNOWN PROBLEM.\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string DocumentWithInvalidDate(bool verbose, DbmMode modeCur)
		{
			//Gets a list of documents with dates that are invalid (0001-01-01). The list should be blank. If not, then the document's date will be set to 001-01-02 which will allow deletion.
			string command = "SELECT COUNT(*) FROM document WHERE DateCreated=" + POut.Date(new DateTime(1, 1, 1));
			int numFound = PIn.Int(Database.ExecuteString(command));
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (numFound > 0 || verbose)
					{
						log += "Documents with invalid dates found" + ": " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					if (numFound > 0)
					{
						command = "UPDATE document SET DateCreated=" + POut.Date(new DateTime(1, 1, 2)) + " WHERE DateCreated=" + POut.Date(new DateTime(1, 1, 1));
						Database.ExecuteNonQuery(command);
					}
					if (numFound > 0 || verbose)
					{
						log += "Documents with invalid dates fixed" + ": " + numFound.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string DocumentWithNoCategory(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT DocNum FROM document WHERE DocCategory=0";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Images with no category found: " + table.Rows.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<Definition> listDefs = Definitions.GetDefsForCategory(DefinitionCategory.ImageCats, true);
					for (int i = 0; i < table.Rows.Count; i++)
					{
						command = "UPDATE document SET DocCategory=" + POut.Long(listDefs[0].Id)
							+ " WHERE DocNum=" + table.Rows[i][0].ToString();
						Database.ExecuteNonQuery(command);
					}
					int numberFixed = table.Rows.Count;
					if (numberFixed > 0 || verbose)
					{
						log += "Images with no category fixed: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion ClockEvent, Deposit, Disease, Document-----------------------------------------------------------------------------------------------
		#region Ebill, EClipboard, EduResource, EmailAttach, Etrans-------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string EbillDuplicates(bool verbose, DbmMode modeCur)
		{
			//Min may not be the oldest when using random primary keys, but we have to pick one.  In most cases they're identical anyway.
			string command = "SELECT MIN(EbillNum) EbillNum,COUNT(*) Count "
				+ "FROM ebill "
				+ "GROUP BY ClinicNum ";
			DataTable tableEbills = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = tableEbills.Select().Select(x => PIn.Int(x["Count"].ToString()) - 1).Sum();//count duplicates=Sum(# per group-1)
					if (numFound > 0 || verbose)
					{
						log += "Ebill duplicate clinic entries found: "
							+ numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					long numberFixed = 0;
					if (tableEbills.Rows.Count > 0)
					{
						command = "DELETE FROM ebill WHERE EbillNum NOT IN ("
							+ string.Join(",", tableEbills.Select().Select(x => PIn.Long(x["EbillNum"].ToString()))) + ")";
						numberFixed = Database.ExecuteNonQuery(command);
					}
					if (numberFixed > 0 || verbose)
					{
						log += "Ebill duplicate clinic entries deleted: "
							+ numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Inserts an ebill for ClinicNum 0 if it does not exist.</summary>
		[DbmMethodAttr]
		public static string EbillMissingDefaultEntry(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT COUNT(*) FROM ebill WHERE ClinicNum=0";
			int numFound = PIn.Int(Database.ExecuteString(command));
			switch (modeCur)
			{
				case DbmMode.Check:
					if (numFound == 0)
					{
						log += "Missing default ebill entry.";
					}
					break;
				case DbmMode.Fix:
					if (numFound == 0)
					{
						long ebillNum = OpenDentBusiness.Crud.EbillCrud.Insert(new Ebill()
						{
							ClinicNum = 0,
							ClientAcctNumber = "",
							ElectUserName = "",
							ElectPassword = "",
							PracticeAddress = EbillAddress.PracticePhysical,
							RemitAddress = EbillAddress.PracticeBilling
						});
						if (ebillNum > 0)
						{
							log += "Default ebill entry inserted.";
						}
					}
					break;
			}
			return log;
		}

		///<summary>Deletes orphaned sheetdefs from eClipboard.</summary>
		[DbmMethodAttr]
		public static string EClipboardOrphanedSheetDefRow(bool verbose, DbmMode modeCur)
		{
			//MySQL error without nested select
			string command = $@"
				SELECT EClipboardSheetDefNum 
					FROM eclipboardsheetdef 
					LEFT JOIN sheetdef 
						ON sheetdef.SheetDefNum=eclipboardsheetdef.SheetDefNum 
					WHERE sheetdef.SheetDefNum IS NULL";
			List<long> listSheetNums = Database.GetListLong(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (listSheetNums.Count > 0 || verbose)
					{
						log += "Invalid eClipboard sheets found" + ": " + listSheetNums.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					if (listSheetNums.Count > 0)
					{
						command = $@"
							DELETE 
							FROM eclipboardsheetdef
							WHERE EClipboardSheetDefNum IN({string.Join(",", listSheetNums.Select(POut.Long))})";
						Database.ExecuteNonQuery(command);
					}
					if (listSheetNums.Count > 0 || verbose)
					{
						log += "Invalid eClipbaord sheets removed" + ": " + listSheetNums.Count + "\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>This could be enhanced to validate all foreign keys on the eduresource.</summary>
		[DbmMethodAttr]
		public static string EduResourceInvalidDiseaseDefNum(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT EduResourceNum FROM eduresource WHERE DiseaseDefNum != 0 AND DiseaseDefNum NOT IN (SELECT DiseaseDefNum FROM diseasedef)";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "EHR Educational Resources with invalid problem found: " + table.Rows.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "SELECT DiseaseDefNum FROM diseasedef WHERE ItemOrder=(SELECT MIN(ItemOrder) FROM diseasedef WHERE IsHidden=0)";
					long defNum = Database.ExecuteLong(command);
					for (int i = 0; i < table.Rows.Count; i++)
					{
						command = "UPDATE eduresource SET DiseaseDefNum='" + defNum + "' WHERE EduResourceNum='" + table.Rows[i][0].ToString() + "'";
						Database.ExecuteNonQuery(command);
					}
					int numberFixed = table.Rows.Count;
					if (numberFixed > 0 || verbose)
					{
						log += "EHR Educational Resources with invalid problem fixed: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string EmailAttachWithTemplateNumAndMessageNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM emailattach WHERE emailattach.EmailTemplateNum!=0 AND emailattach.EmailMessageNum!=0";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Email attachments attached to both an email and a template found" + ": " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "UPDATE emailattach SET EmailTemplateNum=0 WHERE emailattach.EmailTemplateNum!=0 AND emailattach.EmailMessageNum!=0";
					long numFixed = Database.ExecuteNonQuery(command);
					if (numFixed > 0 || verbose)
					{
						log += "Email attachments attached to both an email and a template fixed" + ": " + numFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string Etrans835AttachWithInvalidClaimNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			List<long> listIssueEtrans835Attaches = Database.GetListLong(
				@"SELECT etrans835attach.Etrans835AttachNum
				FROM etrans835attach
				LEFT JOIN claim ON claim.ClaimNum=etrans835attach.ClaimNum
				WHERE etrans835attach.ClaimNum!=0
				AND claim.ClaimNum IS NULL"
			);
			switch (modeCur)
			{
				case DbmMode.Check:
					if (listIssueEtrans835Attaches.Count > 0 || verbose)
					{
						log = "Etrans835Attaches with an invalid claimNum" + ": " + listIssueEtrans835Attaches.Count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					Etrans835Attaches.DeleteMany(listIssueEtrans835Attaches);
					if (listIssueEtrans835Attaches.Count > 0 || verbose)
					{
						log = "Etrans835Attaches with an invalid claimNum deleted" + ": " + listIssueEtrans835Attaches.Count.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Ebill, EduResource, EmailAttach, Etrans------------------------------------------------------------------------------------------------
		#region Fee, FeeSchedule, GroupNote, GroupPermission----------------------------------------------------------------------------------------------

		[DbmMethodAttr(HasBreakDown = true)]
		public static string FeeDeleteDuplicates(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT FeeNum,FeeSched,CodeNum,Amount,ClinicNum,ProvNum FROM fee "
				+ "GROUP BY FeeSched,CodeNum,ClinicNum,ProvNum HAVING COUNT(CodeNum)>1";
			DataTable table = Database.ExecuteDataTable(command);
			int count = table.Rows.Count;
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (count > 0 || verbose)
					{
						log += "Procedure codes with duplicate fee entries:" + " " + count + "\r\n";
						log += "   " + "Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					StringBuilder strBuild = new StringBuilder();
					strBuild.AppendLine("The following procedure codes have duplicate fee entries."
						+ "  Verify that the following amounts are correct:");
					foreach (DataRow row in table.Rows)
					{
						strBuild.AppendLine("Fee Schedule: " + FeeScheds.GetDescription(PIn.Long(row["FeeSched"].ToString()))//No call to db.
							+ " - Code: " + ProcedureCodes.GetStringProcCode(PIn.Long(row["CodeNum"].ToString()))//No call to db.
							+ " - Amount: " + PIn.Double(row["Amount"].ToString()).ToString("n"));
					}
					if (count > 0 || verbose)
					{
						log += strBuild.ToString();
					}
					break;
				case DbmMode.Fix:
					long numberFixed = 0;
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					for (int i = 0; i < count; i++)
					{
						//At least one fee needs to stay.  Each row in table is a random fee, so we'll just keep that one and delete the rest.
						command = "SELECT FeeNum FROM fee WHERE FeeSched=" + table.Rows[i]["FeeSched"].ToString()
							+ " AND CodeNum=" + table.Rows[i]["CodeNum"].ToString()
							+ " AND ClinicNum=" + table.Rows[i]["ClinicNum"].ToString()
							+ " AND ProvNum=" + table.Rows[i]["ProvNum"].ToString()
							+ " AND FeeNum!=" + table.Rows[i]["FeeNum"].ToString();//This is the random fee we will keep.
						List<long> listFeeNums = Database.GetListLong(command);
						Fees.DeleteMany(listFeeNums);
						listFeeNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Fee,
							DbmLogActionType.Delete, methodName, "Deleted fee from FeeDeleteDuplicates.")));
						numberFixed += listFeeNums.Count;
					}
					if (numberFixed > 0 || verbose)
					{
						log += "Duplicate fees deleted" + ": " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string FeeDeleteForInvalidProc(bool verbose, DbmMode modeCur)
		{
			string command = @"SELECT FeeNum,FeeSched,fee.CodeNum AS 'CodeNum' FROM fee 
									LEFT JOIN procedurecode ON fee.CodeNum=procedurecode.CodeNum 
									WHERE procedurecode.CodeNum IS NULL";
			DataTable table = Database.ExecuteDataTable(command);
			int count = table.Rows.Count;
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (count > 0 || verbose)
					{
						log += "Procedure codes with invalid procedure codes: " + count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					long numberFixed = 0;
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					for (int i = 0; i < count; i++)
					{
						command = "SELECT FeeNum FROM fee WHERE FeeNum=" + table.Rows[i]["FeeNum"].ToString();
						List<long> listFeeNums = Database.GetListLong(command);
						Fees.DeleteMany(listFeeNums);
						listFeeNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Fee,
							DbmLogActionType.Delete, methodName, "Deleted fee from FeeDeleteForInvalidProc.")));
						numberFixed += listFeeNums.Count;
					}
					if (numberFixed > 0 || verbose)
					{
						log += "Fees with invalid procedure codes deleted" + ": " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string GroupNoteWithInvalidAptNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT * FROM procedurelog "
				+ "INNER JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
				+ "WHERE procedurelog.AptNum!=0 AND procedurecode.ProcCode='~GRP~'";
			List<Procedure> listProcedures = Crud.ProcedureCrud.SelectMany(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listProcedures.Count;
					if (numFound > 0 || verbose)
					{
						log += "Group notes attached to appointments: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "UPDATE procedurelog SET AptNum=0 "
						+ "WHERE AptNum!=0 AND CodeNum IN(SELECT CodeNum FROM procedurecode WHERE procedurecode.ProcCode='~GRP~')";
					long numfixed = Database.ExecuteNonQuery(command);
					listProcedures.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ProcNum, DbmLogFKeyType.Procedure,
						DbmLogActionType.Update, methodName, "Updated AptNum from " + x.AptNum + " to 0 from GroupNoteWithInvalidAptNum.")));
					if (numfixed > 0 || verbose)
					{
						log += "Group notes attached to appointments fixed: " + numfixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string GroupNoteWithInvalidProcStatus(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT * FROM procedurelog "
				+ "INNER JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
				+ "WHERE procedurelog.ProcStatus NOT IN(" + POut.Int((int)ProcStat.D) + "," + POut.Int((int)ProcStat.EC) + ") "
				+ "AND procedurecode.ProcCode='~GRP~'";
			List<Procedure> listProcedures = Crud.ProcedureCrud.SelectMany(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listProcedures.Count;
					if (numFound > 0 || verbose)
					{
						log += "Group notes with invalid status: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "UPDATE procedurelog SET ProcStatus=" + POut.Long((int)ProcStat.EC) + " "
						+ "WHERE ProcStatus NOT IN(" + POut.Int((int)ProcStat.D) + "," + POut.Int((int)ProcStat.EC) + ") "
						+ "AND CodeNum IN(SELECT CodeNum FROM procedurecode WHERE procedurecode.ProcCode='~GRP~')";
					long numfixed = Database.ExecuteNonQuery(command);
					listProcedures.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ProcNum, DbmLogFKeyType.Procedure,
						DbmLogActionType.Update, methodName, "Updated ProcStatus from " + x.ProcStatus + " to " + POut.String(ProcStat.EC.ToString())
						+ " from GroupNoteWithInvalidProcStatus.")));
					if (numfixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Group notes statuses fixed: " + numfixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string FeeScheduleHiddenWithPatient(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(DISTINCT(FeeSchedNum)) FROM patient "
						+ "INNER JOIN feesched ON patient.FeeSched=feesched.FeeSchedNum "
						+ "WHERE feesched.IsHidden=1";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Hidden Fee Schedules associated to patients: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "SELECT FeeSchedNum FROM feesched WHERE IsHidden=1 AND FeeSchedNum IN (SELECT FeeSched FROM patient)";
					List<long> listFeeSchedNums = Database.GetListLong(command);
					command = "UPDATE feesched SET IsHidden=0 "
						+ "WHERE IsHidden=1 AND FeeSchedNum IN (SELECT FeeSched FROM patient)";
					long numfixed = Database.ExecuteNonQuery(command);
					listFeeSchedNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.FeeSched,
						DbmLogActionType.Update, methodName, "Updated IsHidden from 1 to 0 from FeeScheduleHiddenWithPatient.")));
					if (numfixed > 0 || verbose)
					{
						log += "Hidden Fee Schedules associated to patients unhidden: " + numfixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string GroupPermissionInvalidNewerDays(bool verbose, DbmMode modeCur)
		{
			string log = "";
            string command;
            switch (modeCur)
			{
				case DbmMode.Check:
					command = $"SELECT COUNT(GroupPermNum) FROM grouppermission WHERE NewerDays<0 OR NewerDays>{GroupPermissions.NewerDaysMax}";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Group permissions with invalid NewerDays found: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
                    command = $@"UPDATE grouppermission 
						SET NewerDays=(CASE WHEN NewerDays<0 THEN 0 ELSE {GroupPermissions.NewerDaysMax} END) 
						WHERE NewerDays<0 OR NewerDays>{GroupPermissions.NewerDaysMax}";
					long numfixed = Database.ExecuteNonQuery(command);
					if (numfixed > 0 || verbose)
					{
						log += "Group permissions with invalid NewerDays fixed: " + numfixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Fee, FeeSchedule, GroupNote----------------------------------------------------------------------------------------------------------
		#region Icd9
		[DbmMethodAttr]
		public static string Icd9InvalidCodes(bool verbose, DbmMode modeCur)
		{
			List<long> listIcd9Nums = Database.GetListLong("SELECT ICD9Num FROM icd9 WHERE ICD9Code=''");
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (listIcd9Nums.Count > 0 || verbose)
					{
						log += "Invalid ICD9 codes found" + ": " + listIcd9Nums.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					if (listIcd9Nums.Count > 0)
					{
						Database.ExecuteNonQuery("DELETE FROM icd9 WHERE ICD9Num IN(" + String.Join(",", listIcd9Nums) + ")");
					}
					int numberFixed = listIcd9Nums.Count;
					if (numberFixed > 0 || verbose)
					{
						log += "Invalid ICD9 codes removed" + ": " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}
		#endregion
		#region InsPayPlan, InsPlan, InsSub-------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string InsPayPlanWithPatientPayments(bool verbose, DbmMode modeCur)
		{
			//Gets a list of payplans of type insurance that have patient payments attached to them and no insurance payments attached
			string command = @"SELECT payplan.PayPlanNum 
								FROM payplan
								INNER JOIN paysplit ON paysplit.PayPlanNum=payplan.PayPlanNum
								LEFT JOIN claimproc ON claimproc.PayPlanNum=payplan.PayPlanNum
									AND claimproc.Status IN("
										+ POut.Int((int)ClaimProcStatus.Received) + ","
										+ POut.Int((int)ClaimProcStatus.Supplemental) + ","
										+ POut.Int((int)ClaimProcStatus.CapClaim) + ") " +
							@"WHERE payplan.PlanNum!=0
								AND claimproc.ClaimProcNum IS NULL
								GROUP BY payplan.PayPlanNum";
			List<long> listPayPlanNums = Database.GetListLong(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (listPayPlanNums.Count > 0 || verbose)
					{
						log += "Ins payment plans with patient payments attached" + ": " + listPayPlanNums.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					if (listPayPlanNums.Count > 0)
					{
						//Change the insurance payment plan to a patient payment plan so that the payments will be visible within the payment plan
						command = "UPDATE payplan SET PlanNum=0 WHERE PayPlanNum IN(" + String.Join(",", listPayPlanNums) + ")";
						Database.ExecuteNonQuery(command);
						listPayPlanNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.PayPlan,
							DbmLogActionType.Update, methodName, "Updated PlanNum to 0 from InsPayPlanWithPatientPayments.")));
					}
					int numberFixed = listPayPlanNums.Count;
					if (numberFixed > 0 || verbose)
					{
						log += "Ins payment plans with patient payments attached fixed: " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string InsPlanInvalidCarrier(bool verbose, DbmMode modeCur)
		{
			//Gets a list of insurance plans that do not have a carrier attached. The list should be blank. If not, then you need to go to the plan listed and add a carrier. Missing carriers will cause the send claims function to give an error.
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT PlanNum FROM insplan WHERE CarrierNum NOT IN (SELECT CarrierNum FROM carrier)";
					DataTable table = Database.ExecuteDataTable(command);
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Ins plans with carrier missing found: " + table.Rows.Count + "\r\n";
					}
					break;

				case DbmMode.Breakdown:
					command = "SELECT PlanNum, CarrierNum FROM insplan WHERE CarrierNum NOT IN (SELECT CarrierNum FROM carrier)";
					var dictCarrierPlans = Database.ExecuteDataTable(command).Select().Select(x => new
					{
						CarrierNum = PIn.Long(x["CarrierNum"].ToString()),
						PlanNum = PIn.Long(x["PlanNum"].ToString())
					}).GroupBy(x => x.CarrierNum).ToDictionary(x => x.Key, x => x.ToList());
					if (dictCarrierPlans.Count > 0 || verbose)
					{
						log += "Invalid Carriers Referenced: " + dictCarrierPlans.Count + "\r\n";
						log += "Ins Plans Affected: " + dictCarrierPlans.Values.Sum(x => x.Count) + "\r\n\r\n";
						foreach (var kvp in dictCarrierPlans)
						{
							log += "   CarrierNum: " + kvp.Key + "\r\n";
							kvp.Value.ForEach(x => log += string.Format("    PlanNum:{0}\r\n", x.PlanNum));
							log += "\r\n";
						}
					}
					break;

				case DbmMode.Fix:
					command = "SELECT PlanNum,CarrierNum FROM insplan WHERE CarrierNum NOT IN (SELECT CarrierNum FROM carrier)";
					table = Database.ExecuteDataTable(command);
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					if (table.Rows.Count > 0)
					{
						long carrierNum0 = 0;//The new CarrierNum for any plans that have 0 for their CarrierNum.
						List<long> listCarrierNums = table.Select().Select(x => PIn.Long(x["CarrierNum"].ToString())).Distinct().ToList();
						foreach (long carrierNum in listCarrierNums)
						{
							if (carrierNum <= 0 && carrierNum0 != 0)
							{
								continue;//We'll only insert one carrier for all carrier nums equal or less than 0.
							}
							Carrier carrier = new Carrier
							{
								Name = "UNKNOWN CARRIER " + Math.Max(carrierNum, 0),
								Id = carrierNum,
							};
							Carriers.Insert(carrier, useExistingPriKey: carrierNum > 0);
							if (carrierNum <= 0)
							{
								carrierNum0 = carrier.Id;
							}
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, carrierNum, DbmLogFKeyType.Carrier, DbmLogActionType.Insert, methodName,
								$"Created new carrier '{carrier.Name}' from InsPlanInvalidCarrier."));
						}
						if (carrierNum0 != 0)
						{//If we had any plans with CarrierNum of 0
							command = "UPDATE insplan SET CarrierNum=" + POut.Long(carrierNum0)//set this new carrier for the insplans
								+ " WHERE CarrierNum<=0";
							Database.ExecuteNonQuery(command);
							table.Select().Where(x => PIn.Long(x["CarrierNum"].ToString()) <= 0)
								.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, PIn.Long(x["PlanNum"].ToString()),
									DbmLogFKeyType.InsPlan, DbmLogActionType.Update, methodName, "Updated CarrierNum from " + x["CarrierNum"].ToString() + " to " + carrierNum0
									+ " from InsPlanInvalidCarrier.")));
						}
					}
					int numberFixed = table.Rows.Count;
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Ins plans with carrier missing fixed: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true, HasPatNum = true)]
		public static string InsPlanInvalidNum(bool verbose, DbmMode modeCur, long patNum = 0)
		{
			string log = "";
			bool isPatSpecific = false;
			List<long> listPatPlanNums = new List<long>();
			List<long> listInsSubNums = new List<long>();
			List<long> listPlanNums = new List<long>();
			if (patNum > 0)
			{
				isPatSpecific = true;
				//Benefits need to check against PlanNums and PatPlanNums
				List<PatPlan> listPatPlans = PatPlans.Refresh(patNum);
				List<InsSub> listInsSubs = InsSubs.GetListForSubscriber(patNum);
				if (listPatPlans.Count < 1)
				{
					if (listInsSubs.Count < 1)
					{
						return "No insurance plans on file for patient.  Run the full DBM in order to fix any problems.";
					}
					listInsSubNums = listInsSubs.Select(x => x.InsSubNum).ToList();
					listPlanNums = listInsSubs.Select(x => x.PlanNum).ToList();
				}
				else
				{//PatPlans in the database
					listPatPlanNums = listPatPlans.Select(x => x.PatPlanNum).ToList();
					//Patients could have orphaned ins subs in the database, make sure to include those as well.
					listInsSubNums = listPatPlans.Select(x => x.InsSubNum)
						.Union(listInsSubs.Select(x => x.InsSubNum))
						.Distinct().ToList();
					listPlanNums = InsSubs.GetMany(listInsSubNums).Select(x => x.PlanNum).ToList();
				}
				if (listInsSubNums.Count < 1)
				{
					return "This patient has insurance plans that cannot be fixed on a patient specific level.\r\n"
						+ "PatPlans: " + listPatPlanNums.Count + "  InsSubs: " + listInsSubNums.Count + "  PlanNums: " + listPlanNums.Count
						+ "Run the full DBM in order to fix these problems.";
				}
			}
			switch (modeCur)
			{
				case DbmMode.Check:
					#region CHECK
					string command = "SELECT COUNT(*) FROM appointment "
						+ "WHERE appointment.InsPlan1 != 0 "
						+ (isPatSpecific ? "AND appointment.PatNum=" + POut.Long(patNum) + " " : "")
						+ "AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=appointment.InsPlan1)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Invalid appointment InsPlan1 values: " + numFound + "\r\n";
					}
					command = "SELECT COUNT(*) FROM appointment "
						+ "WHERE appointment.InsPlan2 != 0 "
						+ (isPatSpecific ? "AND appointment.PatNum=" + POut.Long(patNum) + " " : "")
						+ "AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=appointment.InsPlan2)";
					numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Invalid appointment InsPlan2 values: " + numFound + "\r\n";
					}
					command = "SELECT COUNT(*) FROM benefit "
						+ "WHERE PlanNum !=0 ";
					if (isPatSpecific)
					{
						if (listPlanNums.Count > 0)
						{
							command += "AND PlanNum IN (" + string.Join(",", listPlanNums) + ") ";
						}
						else
						{
							command += "AND FALSE ";
						}
					}
					command += "AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=benefit.PlanNum)";
					numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Invalid benefit PlanNums: " + numFound + "\r\n";
					}
					command = "SELECT COUNT(*) FROM inssub "
						+ "WHERE " + (isPatSpecific ? "InsSubNum IN(" + string.Join(",", listInsSubNums) + ") AND " : "")
						+ "NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=inssub.PlanNum)";
					numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Invalid inssub PlanNums: " + numFound + "\r\n";
					}
					#endregion CHECK
					break;
				case DbmMode.Breakdown:
					#region BREAKDOWN
					command = "SELECT PatNum, AptNum, InsPlan1 FROM appointment "
						+ "WHERE appointment.InsPlan1 != 0 "
						+ (isPatSpecific ? "AND appointment.PatNum=" + POut.Long(patNum) + " " : "")
						+ "AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=appointment.InsPlan1)";
					var dictBadAppts1 = Database.ExecuteDataTable(command).Select().Select(x => new
					{
						PatNum = PIn.Long(x["PatNum"].ToString()),
						AptNum = PIn.Long(x["AptNum"].ToString()),
						InsPlan1 = PIn.Long(x["InsPlan1"].ToString())
					})
					.GroupBy(x => x.PatNum).ToDictionary(x => x.Key, x => x.ToList());
					command = "SELECT PatNum, AptNum, InsPlan2 FROM appointment "
						+ "WHERE appointment.InsPlan2 != 0 "
						+ (isPatSpecific ? "AND appointment.PatNum=" + POut.Long(patNum) + " " : "")
						+ "AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=appointment.InsPlan2)";
					var dictBadAppts2 = Database.ExecuteDataTable(command).Select().Select(x => new
					{
						PatNum = PIn.Long(x["PatNum"].ToString()),
						AptNum = PIn.Long(x["AptNum"].ToString()),
						InsPlan2 = PIn.Long(x["InsPlan2"].ToString())
					})
					.GroupBy(x => x.PatNum).ToDictionary(x => x.Key, x => x.ToList());
					command = "SELECT BenefitNum, PlanNum FROM benefit "
						+ "WHERE PlanNum !=0 ";
					if (isPatSpecific)
					{
						if (listPlanNums.Count > 0)
						{
							command += "AND PlanNum IN (" + string.Join(",", listPlanNums) + ") ";
						}
						else
						{
							command += "AND FALSE ";
						}
					}
					command += "AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=benefit.PlanNum)";
					var dictBadBenefitsByPlan = Database.ExecuteDataTable(command).Select().Select(x => new
					{
						//PatNum=PIn.Long(x["PatNum"].ToString()), //PatNum not available.
						PlanNum = PIn.Long(x["PlanNum"].ToString()),
						BenefitNum = PIn.Long(x["BenefitNum"].ToString())
					})
					.GroupBy(x => x.PlanNum).ToDictionary(x => x.Key, x => x.ToList());
					command = "SELECT Subscriber, InsSubNum, PlanNum FROM inssub "
						+ "WHERE " + (isPatSpecific ? "InsSubNum IN(" + string.Join(",", listInsSubNums) + ") AND " : "")
						+ "NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=inssub.PlanNum)";
					var dictBadInsSubs = Database.ExecuteDataTable(command).Select().Select(x => new
					{
						Subscriber = PIn.Long(x["Subscriber"].ToString()),
						InsSubNum = PIn.Long(x["InsSubNum"].ToString()),
						PlanNum = PIn.Long(x["PlanNum"].ToString())
					})
					.GroupBy(x => x.Subscriber).ToDictionary(x => x.Key, x => x.ToList());
					List<long> distinctPatNums = dictBadAppts1.Keys
						.Union(dictBadAppts2.Keys)
						.Union(dictBadInsSubs.Keys)
						.ToList();
					List<Patient> listPatLims = Patients.GetLimForPats(distinctPatNums);
					numFound = dictBadAppts1.Values.Sum(x => x.Count) +
						dictBadAppts2.Values.Sum(x => x.Count) +
						dictBadInsSubs.Values.Sum(x => x.Count) +
						dictBadBenefitsByPlan.Values.Sum(x => x.Count);
					if (numFound > 0 || verbose)
					{
						log += "Invalid PlanNum references found : " + numFound + "\r\n";
						log += "Patients Affected : " + distinctPatNums.Count + (dictBadBenefitsByPlan.Count > 0 ? "+" : "") + "\r\n";
						foreach (Patient patLim in listPatLims)
						{
							string lineItemDBM = "   Patient with invalid PlanNums:" + patLim.GetNameLF() + " (PatNum:" + patLim.PatNum + ")" + "\r\n";
							//No additional translation needed. All "words" exactly match Schema column names.
							if (dictBadAppts1.ContainsKey(patLim.PatNum))
							{
								dictBadAppts1[patLim.PatNum].ForEach(x => lineItemDBM += string.Format("    AptNum:{0} InsPlan1:{1}\r\n", x.AptNum, x.InsPlan1));
							}
							if (dictBadAppts2.ContainsKey(patLim.PatNum))
							{
								dictBadAppts2[patLim.PatNum].ForEach(x => lineItemDBM += string.Format("    AptNum:{0} InsPlan2:{1}\r\n", x.AptNum, x.InsPlan2));
							}
							if (dictBadInsSubs.ContainsKey(patLim.PatNum))
							{
								dictBadInsSubs[patLim.PatNum].ForEach(x => lineItemDBM += string.Format("    InsSubNum:{0} PlanNum:{1}\r\n", x.InsSubNum, x.PlanNum));
							}
							lineItemDBM += "\r\n";
							log += lineItemDBM;
						}
						foreach (var kvp in dictBadBenefitsByPlan)
						{
							string lineItemDBM = "   Invalid plan with attached benefits: PlanNum:" + kvp.Key + "\r\n";
							//No additional translation needed. All "words" exactly match Schema column names.
							lineItemDBM += "    BenefitNums:" + string.Join(", ", kvp.Value.Select(x => x.BenefitNum)) + "\r\n\r\n";
							log += lineItemDBM;
						}
					}
					#endregion BREAKDOWN
					break;
				case DbmMode.Fix:
					#region FIX
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					string where = "";
					//One option will sometimes be to create a dummy plan to attach these things to, be we have not had to implement that yet.  
					//We need databases with actual problems to test these fixes against.
					//appointment.InsPlan1-----------------------------------------------------------------------------------------------
					where = "WHERE InsPlan1 != 0 "
						+ (isPatSpecific ? "AND appointment.PatNum=" + POut.Long(patNum) + " " : "")
						+ "AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=appointment.InsPlan1)";
					command = "SELECT * FROM appointment " + where;
					List<Appointment> listAppointments = Crud.AppointmentCrud.SelectMany(command);
					command = "UPDATE appointment SET InsPlan1=0 " + where;
					long numFixed = Database.ExecuteNonQuery(command);
					listAppointments.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.AptNum, DbmLogFKeyType.Appointment,
						DbmLogActionType.Update, methodName, "Updated InsPlan1 from " + x.InsPlan1 + " to 0 from InsPlanInvalidNum.")));
					if (numFixed > 0 || verbose)
					{
						log += "Invalid appointment InsPlan1 values fixed: " + numFixed + "\r\n";
					}
					//appointment.InsPlan2-----------------------------------------------------------------------------------------------
					where = "WHERE InsPlan2 != 0 "
						+ (isPatSpecific ? "AND appointment.PatNum=" + POut.Long(patNum) + " " : "")
						+ "AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=appointment.InsPlan2)";
					command = "SELECT * FROM appointment " + where;
					listAppointments = Crud.AppointmentCrud.SelectMany(command);
					command = "UPDATE appointment SET InsPlan2=0 " + where;
					numFixed = Database.ExecuteNonQuery(command);
					listAppointments.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.AptNum, DbmLogFKeyType.Appointment,
						DbmLogActionType.Update, methodName, "Updated InsPlan2 from " + x.InsPlan2 + " to 0 from InsPlanInvalidNum.")));
					if (numFixed > 0 || verbose)
					{
						log += "Invalid appointment InsPlan2 values fixed: " + numFixed + "\r\n";
					}
					//benefit.PlanNum----------------------------------------------------------------------------------------------------
					where = "WHERE PlanNum !=0 ";
					if (isPatSpecific)
					{
						if (listPlanNums.Count > 0)
						{
							where += "AND PlanNum IN (" + string.Join(",", listPlanNums) + ") ";
						}
						else
						{
							where += "AND FALSE ";
						}
					}
					where += "AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=benefit.PlanNum)";
					command = "SELECT * FROM benefit " + where;
					List<Benefit> listBenefits = Crud.BenefitCrud.SelectMany(command);
					command = "DELETE FROM benefit " + where;
					numFixed = Database.ExecuteNonQuery(command);
					listBenefits.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.BenefitNum, DbmLogFKeyType.Benefit,
						DbmLogActionType.Delete, methodName, "Deleted benefit from InsPlanInvalidNum.")));
					if (numFixed > 0 || verbose)
					{
						log += "Invalid benefit PlanNums fixed: " + numFixed + "\r\n";
					}
					//inssub.PlanNum------------------------------------------------------------------------------------------------------
					numFixed = 0;
					//1: PlanNum=0
					List<SecurityLog> listSecurityLogs = new List<SecurityLog>();
					List<InsSub> listInsSubs = new List<InsSub>();
					command = "SELECT InsSubNum FROM inssub WHERE PlanNum=0 " + (isPatSpecific ? "AND InsSubNum IN(" + string.Join(",", listInsSubNums) + ")" : "");
					DataTable table = Database.ExecuteDataTable(command);
					for (int i = 0; i < table.Rows.Count; i++)
					{
						long insSubNum = PIn.Long(table.Rows[i]["InsSubNum"].ToString());
						command = "SELECT COUNT(*) FROM claim WHERE InsSubNum=" + POut.Long(insSubNum);
						int countUsed = PIn.Int(Database.ExecuteString(command));
						command = "SELECT COUNT(*) FROM claimproc WHERE InsSubNum=" + POut.Long(insSubNum) + " "
							+ "AND (ClaimNum<>0 OR (Status<>6 AND Status<>3))";//attached to a claim or (not an estimate or adjustment)
						countUsed += PIn.Int(Database.ExecuteString(command));
						command = "SELECT COUNT(*) FROM etrans WHERE InsSubNum=" + POut.Long(insSubNum);
						countUsed += PIn.Int(Database.ExecuteString(command));
						//command="SELECT COUNT(*) FROM patplan WHERE InsSubNum="+POut.Long(insSubNum);
						//countUsed+=PIn.Int(Db.GetCount(command));
						command = "SELECT COUNT(*) FROM payplan WHERE InsSubNum=" + POut.Long(insSubNum);
						countUsed += PIn.Int(Database.ExecuteString(command));
						if (countUsed == 0)
						{
							where = "WHERE InsSubNum=" + POut.Long(insSubNum) + " AND ClaimNum=0 AND (Status=6 OR Status=3)";
							command = "SELECT * FROM claimproc " + where;
							List<ClaimProc> listClaimProc = Crud.ClaimProcCrud.SelectMany(command);
							command = "DELETE FROM claimproc " + where;//ok to delete because no claim and just an estimate or adjustment
							Database.ExecuteNonQuery(command);
							listClaimProc.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimProcNum, DbmLogFKeyType.ClaimProc,
								DbmLogActionType.Delete, methodName, "Deleted claimproc from InsPlanInvalidNum.")));
							//List<Permissions> listPerms = GroupPermissions.GetPermsFromCrudAuditPerm(CrudTableAttribute.GetCrudAuditPermForClass(typeof(InsSub)));
							//listSecurityLogs = SecurityLogs.GetFromFKeysAndType(new List<long> { insSubNum }, listPerms);
							InsSubs.ClearFkey(insSubNum);
							//listSecurityLogs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.SecurityLogNum, DbmLogFKeyType.Securitylog,
							//	DbmLogActionType.Update, methodName, "Updated securitylog by setting FKey to 0 from InsPlanInvalidNum.")));
							command = "SELECT * FROM inssub WHERE InsSubNum=" + POut.Long(insSubNum);
							listInsSubs = Crud.InsSubCrud.SelectMany(command);
							command = "DELETE FROM inssub WHERE InsSubNum=" + POut.Long(insSubNum);
							Database.ExecuteNonQuery(command);
							listInsSubs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.InsSubNum, DbmLogFKeyType.InsSub,
								DbmLogActionType.Delete, methodName, "Deleted inssub from InsPlanInvalidNum.")));
							command = "SELECT * FROM patplan WHERE InsSubNum=" + POut.Long(insSubNum);
							List<PatPlan> listPatPlans = Crud.PatPlanCrud.SelectMany(command);
							command = "DELETE FROM patplan WHERE InsSubNum=" + POut.Long(insSubNum);//It's very safe to "drop coverage" for a patient.
							Database.ExecuteNonQuery(command);
							listPatPlans.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.PatPlanNum, DbmLogFKeyType.PatPlan,
								DbmLogActionType.Delete, methodName, "Deleted patplan from InsPlanInvalidNum.")));
							numFixed++;
							continue;
						}
					}
					//2: PlanNum invalid
					command = "SELECT InsSubNum,PlanNum FROM inssub "
						+ "WHERE " + (isPatSpecific ? "InsSubNum IN(" + string.Join(",", listInsSubNums) + ") AND " : "")
						+ "NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=inssub.PlanNum)";
					table = Database.ExecuteDataTable(command);
					for (int i = 0; i < table.Rows.Count; i++)
					{
						long planNum = PIn.Long(table.Rows[i]["PlanNum"].ToString());
						long insSubNum = PIn.Long(table.Rows[i]["InsSubNum"].ToString());
						command = "SELECT COUNT(*) FROM claim WHERE InsSubNum=" + POut.Long(insSubNum);
						int countUsed = PIn.Int(Database.ExecuteString(command));
						command = "SELECT COUNT(*) FROM claimproc WHERE InsSubNum=" + POut.Long(insSubNum) + " AND Status NOT IN (6,3)";//Estimate,Adjustment
						countUsed += PIn.Int(Database.ExecuteString(command));
						command = "SELECT COUNT(*) FROM etrans WHERE InsSubNum=" + POut.Long(insSubNum);
						countUsed += PIn.Int(Database.ExecuteString(command));
						command = "SELECT COUNT(*) FROM patplan WHERE InsSubNum=" + POut.Long(insSubNum);
						countUsed += PIn.Int(Database.ExecuteString(command));
						command = "SELECT COUNT(*) FROM payplan WHERE InsSubNum=" + POut.Long(insSubNum);
						countUsed += PIn.Int(Database.ExecuteString(command));
						//planNum
						command = "SELECT COUNT(*) FROM benefit WHERE PlanNum=" + POut.Long(planNum);
						countUsed += PIn.Int(Database.ExecuteString(command));
						command = "SELECT COUNT(*) FROM claim WHERE PlanNum=" + POut.Long(planNum);
						countUsed += PIn.Int(Database.ExecuteString(command));
						command = "SELECT COUNT(*) FROM claimproc WHERE PlanNum=" + POut.Long(planNum) + " AND Status NOT IN (6,3)";//Estimate,Adjustment
						countUsed += PIn.Int(Database.ExecuteString(command));
						if (countUsed == 0)
						{//There are no other pointers to this invalid plannum or this inssub, delete this inssub
							//List<Permissions> listPerms = GroupPermissions.GetPermsFromCrudAuditPerm(CrudTableAttribute.GetCrudAuditPermForClass(typeof(InsSub)));
							//listSecurityLogs = SecurityLogs.GetFromFKeysAndType(new List<long> { insSubNum }, listPerms);
							InsSubs.ClearFkey(insSubNum);
							//listSecurityLogs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.SecurityLogNum, DbmLogFKeyType.Securitylog,
							//	DbmLogActionType.Update, methodName, "Updated securitylog by setting FKey to 0 from InsPlanInvalidNum.")));
							command = "SELECT * FROM inssub WHERE InsSubNum=" + POut.Long(insSubNum);
							listInsSubs = Crud.InsSubCrud.SelectMany(command);
							command = "DELETE FROM inssub WHERE InsSubNum=" + POut.Long(insSubNum);
							Database.ExecuteNonQuery(command);
							command = "DELETE FROM claimproc WHERE PlanNum=" + POut.Long(planNum) + " AND Status IN (6,3)";//Estimate,Adjustment
							Database.ExecuteNonQuery(command);
							command = "DELETE FROM claimproc WHERE InsSubNum=" + POut.Long(insSubNum) + " AND Status IN (6,3)";//Estimate,Adjustment
							Database.ExecuteNonQuery(command);
							listInsSubs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.InsSubNum, DbmLogFKeyType.InsSub,
								DbmLogActionType.Delete, methodName, "Deleted inssub from InsPlanInvalidNum.")));
							numFixed++;
							continue;
						}
						else
						{//There are objects referencing this inssub or this insplan.  Insert a dummy plan linked to a dummy carrier with CarrierName=Unknown
                            InsurancePlan insplan = new InsurancePlan
                            {
                                IsHidden = true,
                                CarrierId = Carriers.GetByNameAndPhone("UNKNOWN CARRIER", "", true).Id,
                                //Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
                                SecUserNumEntry = Security.CurrentUser.Id
                            };
                            long insPlanNum = InsPlans.Insert(insplan);
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, insPlanNum, DbmLogFKeyType.InsPlan, DbmLogActionType.Insert,
								methodName, "Inserted a new insplan from InsPlanInvalidNum"));
							command = "SELECT * FROM inssub WHERE InsSubNum=" + POut.Long(insSubNum);
							listInsSubs = Crud.InsSubCrud.SelectMany(command);
							command = "UPDATE inssub SET PlanNum=" + POut.Long(insPlanNum) + " WHERE InsSubNum=" + POut.Long(insSubNum);
							Database.ExecuteNonQuery(command);
							listInsSubs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.InsSubNum, DbmLogFKeyType.InsSub,
								DbmLogActionType.Update, methodName, "Updated PlanNum from " + x.PlanNum + " to " + insPlanNum + " InsPlanInvalidNum.")));
							numFixed++;
							continue;
						}
					}
					if (numFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Invalid inssub PlanNums fixed: " + numFixed + "\r\n";
					}
					#endregion FIX
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string InsPlanNoClaimForm(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM insplan WHERE ClaimFormNum=0";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Insplan claimforms missing: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "SELECT * FROM insplan WHERE ClaimFormNum=0";
					List<InsurancePlan> listInsPlans = Crud.InsPlanCrud.SelectMany(command);
					command = "UPDATE insplan SET ClaimFormNum=" + POut.Long(Preferences.GetLong(PreferenceName.DefaultClaimForm))
						+ " WHERE ClaimFormNum=0";
					long numberFixed = Database.ExecuteNonQuery(command);
					listInsPlans.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.Id, DbmLogFKeyType.InsPlan,
						DbmLogActionType.Update, methodName, "Updated ClaimFormNum from 0 to " + Preferences.GetLong(PreferenceName.DefaultClaimForm))));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Insplan claimforms set if missing: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string InsSubInvalidSubscriber(bool verbose, DbmMode modeCur)
		{
			string command = @"SELECT inssub.Subscriber,COALESCE(patient.PatStatus,-1) PatStatus FROM inssub 
				LEFT JOIN patient ON patient.PatNum=inssub.Subscriber
				WHERE (patient.PatNum IS NULL OR patient.PatStatus=" + POut.Int((int)PatientStatus.Deleted) + @")
				AND inssub.Subscriber != 0 
				GROUP BY inssub.Subscriber";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = table.Rows.Count;
					if (numFound > 0 || verbose)
					{
						log += "InsSub subscribers missing: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					long priProv = Preferences.GetLong(PreferenceName.PracticeDefaultProv);
					long billType = Preferences.GetLong(PreferenceName.PracticeDefaultBillType);
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					for (int i = 0; i < table.Rows.Count; i++)
					{
						if (PIn.Int(table.Rows[i]["PatStatus"].ToString()) == (int)PatientStatus.Deleted)
						{//The patient exists in the db but is deleted.
						 //Change the patient to Archived.
							Patient pat = Patients.GetPat(PIn.Long(table.Rows[i]["Subscriber"].ToString()));
							Patient patOld = pat.Copy();
							pat.PatStatus = PatientStatus.Archived;
							Patients.Update(pat, patOld);
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, pat.PatNum, DbmLogFKeyType.Patient, DbmLogActionType.Update,
								methodName, "Updated PatStatus from" + patOld.PatStatus + " to " + PatientStatus.Archived + "."));
							SecurityLogs.MakeLogEntry(Permissions.PatientEdit, pat.PatNum,
								"Patient status changed from 'Deleted' to 'Archived' from DBM fix for InsSubInvalidSubscriber.", SecurityLogSource.DBM);
						}
						else
						{//The patient does not exist in the db at all.
                         //Create dummy patients using the FKs that the Subscriber column is expecting.
                            Patient pat = new Patient
                            {
                                PatNum = PIn.Long(table.Rows[i]["Subscriber"].ToString()),
                                LName = "UNKNOWN",
                                FName = "Unknown"
                            };
                            pat.Guarantor = pat.PatNum;
							pat.PriProv = priProv;
							pat.BillingType = billType;
							//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
							pat.SecUserNumEntry = Security.CurrentUser.Id;
							Patients.Insert(pat, true);
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, pat.PatNum, DbmLogFKeyType.Patient, DbmLogActionType.Insert,
								methodName, "Inserted patient from InsSubInvalidSubscriber."));
							SecurityLogs.MakeLogEntry(Permissions.PatientCreate, pat.PatNum, "Recreated from DBM fix for InsSubInvalidSubscriber.", SecurityLogSource.DBM);
						}
					}
					int numberFixed = table.Rows.Count;
					if (numberFixed > 0 || verbose)
					{
						log += "InsSub subscribers fixed: " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		///<summary>Checks for situations where there are valid InsSubNums, but mismatched PlanNums.</summary>
		[DbmMethodAttr(HasBreakDown = true, HasPatNum = true)]
		public static string InsSubNumMismatchPlanNum(bool verbose, DbmMode modeCur, long patNumSpecific = 0)
		{
			string log = "";
			//Not going to validate the following tables because they do not have an InsSubNum column: appointmentx2, benefit.
			//This DBM assumes that the inssub table is correct because that's what we're comparing against.
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					#region CHECK
					int numFound = 0;
					bool hasBreakDown = false;
					//Can't do the following because no inssubnum: appointmentx2, benefit.
					//Can't do inssub because that's what we're comparing against.  That's the one that's assumed to be correct.
					//claim.PlanNum -----------------------------------------------------------------------------------------------------
					command = "SELECT COUNT(*) FROM claim "
						+ "WHERE PlanNum NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum) "
						+ PatientAndClauseHelper(patNumSpecific, "claim");
					numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Mismatched claim InsSubNum/PlanNum values: " + numFound + "\r\n";
						if (numFound > 0)
						{
							hasBreakDown = true;
						}
					}
					//claim.PlanNum2---------------------------------------------------------------------------------------------------
					command = "SELECT COUNT(*) FROM claim "
						+ "WHERE PlanNum2 != 0 "
						+ "AND PlanNum2 NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum2)"
						+ PatientAndClauseHelper(patNumSpecific, "claim");
					numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Mismatched claim InsSubNum2/PlanNum2 values: " + numFound + "\r\n";
						if (numFound > 0)
						{
							hasBreakDown = true;
						}
					}
					//claimproc---------------------------------------------------------------------------------------------------
					command = "SELECT COUNT(*) FROM claimproc "
						+ "WHERE PlanNum NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claimproc.InsSubNum)"
						+ PatientAndClauseHelper(patNumSpecific, "claimproc");
					numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Mismatched claimproc InsSubNum/PlanNum values: " + numFound + "\r\n";
						if (numFound > 0)
						{
							hasBreakDown = true;
						}
					}
					//etrans---------------------------------------------------------------------------------------------------
					command = "SELECT COUNT(*) FROM etrans "
						+ "WHERE PlanNum!=0 AND PlanNum NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=etrans.InsSubNum)"
						+ PatientAndClauseHelper(patNumSpecific, "etrans");
					numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Mismatched etrans InsSubNum/PlanNum values: " + numFound + "\r\n";
						if (numFound > 0)
						{
							hasBreakDown = true;
						}
					}
					//payplan---------------------------------------------------------------------------------------------------
					command = "SELECT COUNT(*) FROM payplan "
						+ "WHERE EXISTS (SELECT PlanNum FROM inssub WHERE inssub.InsSubNum=payplan.InsSubNum AND inssub.PlanNum!=payplan.PlanNum)"
						+ PatientAndClauseHelper(patNumSpecific, "payplan");
					numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Mismatched payplan InsSubNum/PlanNum values: " + numFound + "\r\n";
						if (numFound > 0)
						{
							hasBreakDown = true;
						}
					}
					if (hasBreakDown)
					{
						log += "   Run Fix or double click to see a break down.";
					}
					break;
				#endregion CHECK
				case DbmMode.Breakdown:
					#region BREAKDOWN
					//In this BREAKDOWN, when user double clicks on this DBM query and show what needs to be fixed/will attempted to be fixed when running Fix.
					//claim.PlanNum -----------------------------------------------------------------------------------------------------
					command = "SELECT claim.PatNum,claim.PlanNum,claim.ClaimNum,claim.DateService FROM claim "
						+ "WHERE PlanNum NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum) "
						+ PatientAndClauseHelper(patNumSpecific, "claim");
					var dictBadClaims1 = Database.ExecuteDataTable(command).Select().Select(x => new
					{
						PatNum = PIn.Long(x["PatNum"].ToString()),
						PlanNum = PIn.Long(x["PlanNum"].ToString()),
						ClaimNum = PIn.Long(x["ClaimNum"].ToString()),
						DateService = PIn.Date(x["DateService"].ToString())
					}).GroupBy(x => x.PatNum)
					.ToDictionary(x => x.Key, x => x.ToList());
					//claim.PlanNum2---------------------------------------------------------------------------------------------------
					command = "SELECT claim.PatNum,claim.PlanNum2,claim.ClaimNum,claim.DateService FROM claim "
						+ "WHERE PlanNum2 != 0 "
						+ "AND PlanNum2 NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum2)"
						+ PatientAndClauseHelper(patNumSpecific, "claim");
					var dictBadClaims2 = Database.ExecuteDataTable(command).Select().Select(x => new
					{
						PatNum = PIn.Long(x["PatNum"].ToString()),
						PlanNum2 = PIn.Long(x["PlanNum2"].ToString()),
						ClaimNum = PIn.Long(x["ClaimNum"].ToString()),
						DateService = PIn.Date(x["DateService"].ToString())
					}).GroupBy(x => x.PatNum)
					.ToDictionary(x => x.Key, x => x.ToList());
					//claimproc---------------------------------------------------------------------------------------------------
					command = "SELECT claimproc.PatNum,claimproc.ClaimProcNum,claimproc.InsSubNum,claimproc.ProcNum,claimproc.ClaimNum FROM claimproc "
						+ "WHERE PlanNum NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claimproc.InsSubNum)"
						+ PatientAndClauseHelper(patNumSpecific, "claimproc");
					var dictBadClaimProcs = Database.ExecuteDataTable(command).Select().Select(x => new
					{
						PatNum = PIn.Long(x["PatNum"].ToString()),
						ClaimProcNum = PIn.Long(x["ClaimProcNum"].ToString()),
						InsSubNum = PIn.Long(x["InsSubNum"].ToString()),
						ProcNum = PIn.Long(x["ProcNum"].ToString()),
						ClaimNum = PIn.Long(x["ClaimNum"].ToString())
					}).GroupBy(x => x.PatNum)
					.ToDictionary(x => x.Key, x => x.ToList());
					//etrans---------------------------------------------------------------------------------------------------
					command = "SELECT etrans.PatNum,etrans.PlanNum,etrans.EtransNum,etrans.DateTimeTrans FROM etrans "
						+ "WHERE PlanNum!=0 AND PlanNum NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=etrans.InsSubNum)"
						+ PatientAndClauseHelper(patNumSpecific, "etrans");
					var dictBadEtrans = Database.ExecuteDataTable(command).Select().Select(x => new
					{
						PatNum = PIn.Long(x["PatNum"].ToString()),
						PlanNum = PIn.Long(x["PlanNum"].ToString()),
						EtransNum = PIn.Long(x["EtransNum"].ToString()),
						DateTimeTrans = PIn.Date(x["DateTimeTrans"].ToString())
					}).GroupBy(x => x.PatNum)
					.ToDictionary(x => x.Key, x => x.ToList());
					//payplan---------------------------------------------------------------------------------------------------
					command = "SELECT payplan.PatNum,payplan.PlanNum,payplan.PayPlanNum FROM payplan "
						+ "WHERE EXISTS (SELECT PlanNum FROM inssub WHERE inssub.InsSubNum=payplan.InsSubNum AND inssub.PlanNum!=payplan.PlanNum)"
						+ PatientAndClauseHelper(patNumSpecific, "payplan");
					var dictBadPayPlans = Database.ExecuteDataTable(command).Select().Select(x => new
					{
						PatNum = PIn.Long(x["PatNum"].ToString()),
						PlanNum = PIn.Long(x["PlanNum"].ToString()),
						PayPlanNum = PIn.Long(x["PayPlanNum"].ToString())
					}).GroupBy(x => x.PatNum)
					.ToDictionary(x => x.Key, x => x.ToList());
					List<long> distinctPatNums = dictBadClaims1.Keys
						.Union(dictBadClaims2.Keys)
						.Union(dictBadClaimProcs.Keys)
						.Union(dictBadEtrans.Keys)
						.Union(dictBadPayPlans.Keys)
						.ToList();
					List<Patient> listPatLims = Patients.GetLimForPats(distinctPatNums);
					numFound = dictBadClaims1.Values.Sum(x => x.Count) +
						dictBadClaims2.Values.Sum(x => x.Count) +
						dictBadClaimProcs.Values.Sum(x => x.Count) +
						dictBadEtrans.Values.Sum(x => x.Count) +
						dictBadPayPlans.Values.Sum(x => x.Count);
					if (numFound > 0 || verbose)
					{
						log += "Mismatched InsSubNum/PlanNum values: " + numFound + "\r\n";
						log += "Patients affected: " + distinctPatNums.Count + "\r\n";
						foreach (Patient patLim in listPatLims)
						{
							string lineItemDBM = "   Patient with associated invalid PlanNums:" + patLim.GetNameLF() + " (PatNum:" + patLim.PatNum + ")" + "\r\n";
							//No additional translation needed. All "words" exactly match Schema column names.
							if (dictBadClaims1.ContainsKey(patLim.PatNum))
							{
								dictBadClaims1[patLim.PatNum].ForEach(x => lineItemDBM += string.Format("    ClaimNum:{0} PlanNum:{1} DateService:{2}\r\n", x.ClaimNum, x.PlanNum, x.DateService.ToShortDateString()));
							}
							if (dictBadClaims2.ContainsKey(patLim.PatNum))
							{
								dictBadClaims2[patLim.PatNum].ForEach(x => lineItemDBM += string.Format("    ClaimNum:{0} PlanNum2:{1} DateService:{2}\r\n", x.ClaimNum, x.PlanNum2, x.DateService.ToShortDateString()));
							}
							if (dictBadClaimProcs.ContainsKey(patLim.PatNum))
							{
								dictBadClaimProcs[patLim.PatNum].ForEach(x => lineItemDBM += string.Format("    ClaimProcNum:{0} InsSubNum:{1} ClaimNum:{2} ProcNum:{3}\r\n", x.ClaimProcNum, x.InsSubNum, x.ClaimNum, x.ProcNum));
							}
							if (dictBadEtrans.ContainsKey(patLim.PatNum))
							{
								dictBadEtrans[patLim.PatNum].ForEach(x => lineItemDBM += string.Format("    ETransNum:{0} PlanNum:{1} DateTimeTrans:{2}\r\n", x.EtransNum, x.PlanNum, x.DateTimeTrans.ToString()));
							}
							if (dictBadPayPlans.ContainsKey(patLim.PatNum))
							{
								dictBadPayPlans[patLim.PatNum].ForEach(x => lineItemDBM += string.Format("    PayPlanNum:{0} PlanNum:{1}\r\n", x.PayPlanNum, x.PlanNum));
							}
							lineItemDBM += "\r\n";
							log += lineItemDBM;
						}
					}
					break;
				#endregion BREAKDOWN
				case DbmMode.Fix:
					#region FIX
					long numFixed = 0;
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					string where = "";
					#region Claim PlanNum
					#region claim.PlanNum (1/4) Mismatch
					where = "WHERE PlanNum != (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum)"
						+ PatientAndClauseHelper(patNumSpecific, "claim");
					command = "SELECT * FROM claim " + where;
					List<Claim> listClaims = Crud.ClaimCrud.SelectMany(command);
					command = "UPDATE claim SET PlanNum=(SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum) " + where;
					numFixed = Database.ExecuteNonQuery(command);
					listClaims.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimNum, DbmLogFKeyType.Claim,
						DbmLogActionType.Update, methodName, "Updated PlanNum from InsSubNumMismatchPlanNum.")));
					if (numFixed > 0 || verbose)
					{
						log += "Mismatched claim InsSubNum/PlanNum fixed: " + numFixed.ToString() + "\r\n";
					}
					#endregion
					numFixed = 0;
					#region claim.PlanNum (2/4) PlanNum zero, invalid InsSubNum
					//Will leave orphaned claimprocs. No finanicals to check.
					command = "SELECT claim.ClaimNum FROM claim WHERE PlanNum=0 AND ClaimStatus IN ('PreAuth','W','U','H') "
						+ "AND NOT EXISTS(SELECT * FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum)"
						+ PatientAndClauseHelper(patNumSpecific, "claim");
					DataTable tableClaimNums = Database.ExecuteDataTable(command);
					List<long> listClaimNums = new List<long>();
					for (int i = 0; i < tableClaimNums.Rows.Count; i++)
					{
						listClaimNums.Add(PIn.Long(tableClaimNums.Rows[i]["ClaimNum"].ToString()));
					}
					if (listClaimNums.Count > 0)
					{
						//List<Permissions> listPerms = GroupPermissions.GetPermsFromCrudAuditPerm(CrudTableAttribute.GetCrudAuditPermForClass(typeof(Claim)));
						//List<SecurityLog> listSecurityLogs = SecurityLogs.GetFromFKeysAndType(listClaimNums, listPerms);
						Claims.ClearFkey(listClaimNums);//Zero securitylog FKey column for rows to be deleted.
						//listSecurityLogs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.SecurityLogNum, DbmLogFKeyType.Securitylog,
						//	DbmLogActionType.Update, methodName, "Set FKey to 0 from InsSubNumMismatchPlanNum.")));
					}
					where = "WHERE PlanNum=0 AND ClaimStatus IN('PreAuth','W','U','H') AND NOT EXISTS(SELECT * FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum)"
						+ PatientAndClauseHelper(patNumSpecific, "claim");
					command = "SELECT ClaimNum FROM claim " + where;
					listClaimNums = Database.GetListLong(command);
					command = "DELETE FROM claim " + where;
					numFixed = Database.ExecuteNonQuery(command);
					listClaimNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Claim,
						DbmLogActionType.Delete, methodName, "Claim deleted with invalid InsSubNum and PlanNum=0 .")));
					if (numFixed > 0 || verbose)
					{
						log += "Claims deleted with invalid InsSubNum and PlanNum=0: " + numFixed.ToString() + "\r\n";
					}
					#endregion
					numFixed = 0;
					#region claim.PlanNum (3/4) PlanNum invalid, and claim.InsSubNum invalid
					command = "SELECT claim.PatNum,claim.PlanNum,claim.InsSubNum FROM claim "
						+ "WHERE PlanNum NOT IN (SELECT insplan.PlanNum FROM insplan) "
						+ "AND InsSubNum NOT IN (SELECT inssub.InsSubNum FROM inssub) "
						+ PatientAndClauseHelper(patNumSpecific, "claim");
					DataTable table = Database.ExecuteDataTable(command);
					if (table.Rows.Count > 0)
					{
						log += "List of patients who will need insurance information reentered:\r\n";
					}
					for (int i = 0; i < table.Rows.Count; i++)
					{//Create simple InsPlans and InsSubs for each claim to replace the missing ones.
					 //make sure a plan does not exist from a previous insert in this loop
						command = "SELECT COUNT(*) FROM insplan WHERE PlanNum=" + table.Rows[i]["PlanNum"].ToString();
						if (Database.ExecuteString(command) == "0")
						{
                            InsurancePlan plan = new InsurancePlan
                            {
                                Id = PIn.Long(table.Rows[i]["PlanNum"].ToString()),//reuse the existing FK
                                IsHidden = true,
                                CarrierId = Carriers.GetByNameAndPhone("UNKNOWN CARRIER", "", true).Id,
                                //Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
                                SecUserNumEntry = Security.CurrentUser.Id
                            };
                            InsPlans.Insert(plan, true);
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, plan.Id, DbmLogFKeyType.InsPlan, DbmLogActionType.Insert,
								methodName, "Inserted new insplan from InsSubNumMismatchPlanNum."));
						}
						long patNum = PIn.Long(table.Rows[i]["PatNum"].ToString());
						//make sure an inssub does not exist from a previous insert in this loop
						command = "SELECT COUNT(*) FROM inssub WHERE InsSubNum=" + table.Rows[i]["InsSubNum"].ToString();
						if (Database.ExecuteString(command) == "0")
						{
                            InsSub sub = new InsSub
                            {
                                InsSubNum = PIn.Long(table.Rows[i]["InsSubNum"].ToString()),//reuse the existing FK
                                PlanNum = PIn.Long(table.Rows[i]["PlanNum"].ToString()),
                                Subscriber = patNum,//if this sub was created on a previous loop, this may be some other patient.
                                SubscriberID = "unknown",
                                //Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
                                SecUserNumEntry = Security.CurrentUser.Id
                            };
                            InsSubs.Insert(sub, true);
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, sub.InsSubNum, DbmLogFKeyType.InsSub, DbmLogActionType.Insert,
								methodName, "Inserted new inssub from InsSubNumMismatchPlanNum."));
						}
						Patient pat = Patients.GetLim(patNum);
						log += "PatNum: " + pat.PatNum + " - " + Patients.GetNameFL(pat.LName, pat.FName, pat.Preferred, pat.MiddleI) + "\r\n";
					}
					numFixed = table.Rows.Count;
					if (numFixed > 0 || verbose)
					{
						log += "Claims with invalid PlanNums and invalid InsSubNums fixed: " + numFixed.ToString() + "\r\n";
					}
					#endregion
					numFixed = 0;
					#region claim.PlanNum (4/4) PlanNum valid, but claim.InsSubNum invalid
					command = "SELECT PatNum,PlanNum,InsSubNum FROM claim "
						+ "WHERE PlanNum IN (SELECT insplan.PlanNum FROM insplan) "
						+ "AND InsSubNum NOT IN (SELECT inssub.InsSubNum FROM inssub) GROUP BY InsSubNum"
						+ PatientAndClauseHelper(patNumSpecific, "claim");
					table = Database.ExecuteDataTable(command);
					//Create a dummy inssub and link it to the valid plan.
					for (int i = 0; i < table.Rows.Count; i++)
					{
                        InsSub sub = new InsSub
                        {
                            InsSubNum = PIn.Long(table.Rows[i]["InsSubNum"].ToString()),
                            PlanNum = PIn.Long(table.Rows[i]["PlanNum"].ToString()),
                            Subscriber = PIn.Long(table.Rows[i]["PatNum"].ToString()),
                            SubscriberID = "unknown",
                            //Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
                            SecUserNumEntry = Security.CurrentUser.Id
                        };
                        InsSubs.Insert(sub, true);
						listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, sub.InsSubNum, DbmLogFKeyType.InsSub, DbmLogActionType.Insert,
							methodName, "Inserted new inssub from InsSubNumMismatchPlanNum."));
					}
					numFixed = table.Rows.Count;
					if (numFixed > 0 || verbose)
					{
						log += "Claims with invalid InsSubNums and invalid PlanNums fixed: " + numFixed.ToString() + "\r\n";
					}
					#endregion
					#endregion
					numFixed = 0;
					#region Claim PlanNum2
					//claim.PlanNum2---------------------------------------------------------------------------------------------------
					where = "WHERE PlanNum2 != 0 AND PlanNum2 NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum2)"
						+ PatientAndClauseHelper(patNumSpecific, "claim");
					command = "SELECT * FROM claim " + where;
					listClaims = Crud.ClaimCrud.SelectMany(command);
					command = "UPDATE claim SET PlanNum2=(SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum2) " + where;
					//if InsSubNum2 was completely invalid, then PlanNum2 gets set to zero here.
					numFixed = Database.ExecuteNonQuery(command);
					listClaims.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimNum, DbmLogFKeyType.Claim,
						DbmLogActionType.Update, methodName, "Updated PlanNum2 from InsSubNumMismatchPlanNum.")));
					if (numFixed > 0 || verbose)
					{
						log += "Mismatched claim InsSubNum2/PlanNum2 fixed: " + numFixed.ToString() + "\r\n";
					}
					#endregion
					numFixed = 0;
					#region ClaimProc
					//claimproc (1/2) If planNum is valid but InsSubNum does not exist, then add a dummy inssub----------------------------------------
					command = "SELECT PatNum,PlanNum,InsSubNum FROM claimproc "
						+ "WHERE PlanNum IN (SELECT insplan.PlanNum FROM insplan) "
						+ PatientAndClauseHelper(patNumSpecific, "claimproc")
						+ "AND InsSubNum NOT IN (SELECT inssub.InsSubNum FROM inssub) GROUP BY InsSubNum";
					table = Database.ExecuteDataTable(command);
					//Create a dummy inssub and link it to the valid plan.
					for (int i = 0; i < table.Rows.Count; i++)
					{
                        InsSub sub = new InsSub
                        {
                            InsSubNum = PIn.Long(table.Rows[i]["InsSubNum"].ToString()),
                            PlanNum = PIn.Long(table.Rows[i]["PlanNum"].ToString()),
                            Subscriber = PIn.Long(table.Rows[i]["PatNum"].ToString()),
                            SubscriberID = "unknown",
                            //Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
                            SecUserNumEntry = Security.CurrentUser.Id
                        };
                        InsSubs.Insert(sub, true);
						listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, sub.InsSubNum, DbmLogFKeyType.InsSub, DbmLogActionType.Insert,
							methodName, "Inserted new inssub from InsSubNumMismatchPlanNum."));
					}
					numFixed = table.Rows.Count;
					if (numFixed > 0 || verbose)
					{
						log += "Claims with valid PlanNums and invalid InsSubNums fixed: " + numFixed.ToString() + "\r\n";
					}
					numFixed = 0;
					//claimproc (2/2) Mismatch, but InsSubNum is valid
					where = "WHERE PlanNum != (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claimproc.InsSubNum)"
						+ PatientAndClauseHelper(patNumSpecific, "claimproc");
					command = "SELECT * FROM claimproc " + where;
					List<ClaimProc> listClaimProcs = Crud.ClaimProcCrud.SelectMany(command);
					command = "UPDATE claimproc SET PlanNum=(SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claimproc.InsSubNum) " + where;
					numFixed = Database.ExecuteNonQuery(command);
					listClaimProcs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimProcNum, DbmLogFKeyType.ClaimProc,
						DbmLogActionType.Update, methodName, "Updated PlanNum from InsSubNumMismatchPlanNum.")));
					if (numFixed > 0 || verbose)
					{
						log += "Mismatched claimproc InsSubNum/PlanNum fixed: " + numFixed.ToString() + "\r\n";
					}
					numFixed = 0;
					//claimproc.PlanNum zero, invalid InsSubNum--------------------------------------------------------------------------------
					where = "WHERE PlanNum=0 AND NOT EXISTS(SELECT * FROM inssub WHERE inssub.InsSubNum=claimproc.InsSubNum)"
						+ " AND InsPayAmt=0 AND WriteOff=0"//Make sure this deletion will not affect financials.
						+ " AND Status IN (6,2)"//OK to delete because no claim and just an estimate (6) or preauth (2) claimproc
						+ PatientAndClauseHelper(patNumSpecific, "claimproc");
					command = "SELECT * FROM claimproc " + where;
					listClaimProcs = Crud.ClaimProcCrud.SelectMany(command);
					command = "DELETE FROM claimproc " + where;
					numFixed = Database.ExecuteNonQuery(command);
					listClaimProcs.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.ClaimProcNum, DbmLogFKeyType.ClaimProc,
						 DbmLogActionType.Delete, methodName, "Deleted claimproc from InsSubNumMismatchPlanNum.")));
					if (numFixed > 0 || verbose)
					{
						log += "Claimprocs deleted with invalid InsSubNum and PlanNum=0: " + numFixed.ToString() + "\r\n";
					}
					#endregion
					numFixed = 0;
					#region Etrans
					//etrans---------------------------------------------------------------------------------------------------
					where = "WHERE PlanNum!=0 AND PlanNum NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=etrans.InsSubNum)"
						+ PatientAndClauseHelper(patNumSpecific, "etrans");
					command = "SELECT * FROM etrans " + where;
					List<Etrans> listEtrans = Crud.EtransCrud.SelectMany(command);
					command = "UPDATE etrans SET PlanNum=(SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=etrans.InsSubNum) " + where;
					numFixed = Database.ExecuteNonQuery(command);
					listEtrans.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.EtransNum, DbmLogFKeyType.Etrans,
						DbmLogActionType.Update, methodName, "Updated PlanNum from InsSubNumMismatchPlanNum.")));
					if (numFixed > 0 || verbose)
					{
						log += "Mismatched etrans InsSubNum/PlanNum fixed: " + numFixed.ToString() + "\r\n";
					}
					#endregion
					numFixed = 0;
					#region PayPlan
					//payplan--------------------------------------------------------------------------------------------------
					where = "WHERE EXISTS (SELECT PlanNum FROM inssub WHERE inssub.InsSubNum=payplan.InsSubNum AND inssub.PlanNum!=payplan.PlanNum)"
						+ PatientAndClauseHelper(patNumSpecific, "payplan");
					command = "SELECT * FROM payplan " + where;
					List<PayPlan> listPayPlans = Crud.PayPlanCrud.SelectMany(command);
					command = "UPDATE payplan SET PlanNum=(SELECT PlanNum FROM inssub WHERE inssub.InsSubNum=payplan.InsSubNum) " + where;
					numFixed = Database.ExecuteNonQuery(command);
					listPayPlans.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.PayPlanNum, DbmLogFKeyType.PayPlan,
						DbmLogActionType.Update, methodName, "Updated PlanNum from InsSubNumMismatchPlanNum.")));
					if (numFixed > 0 || verbose)
					{
						log += "Mismatched payplan InsSubNum/PlanNum fixed: " + numFixed.ToString() + "\r\n";
					}
					#endregion
					Crud.DbmLogCrud.InsertMany(listDbmLogs);
					#endregion FIX
					break;
			}
			return log;
		}

		#endregion InsPayPlan, InsPlan, InsSub----------------------------------------------------------------------------------------------------------
		#region JournalEntry, LabCase, Laboratory-------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string LaboratoryWithInvalidSlip(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM laboratory WHERE Slip NOT IN(SELECT SheetDefNum FROM sheetdef) AND Slip != 0";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Laboratories found with invalid lab slips: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "UPDATE laboratory SET Slip=0 WHERE Slip NOT IN(SELECT SheetDefNum FROM sheetdef) AND Slip != 0";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Laboratories fixed with invalid lab slips: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion JournalEntry, LabCase, Laboratory----------------------------------------------------------------------------------------------------
		#region MedicationPat, Medication, MessageButton------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string MedicationPatDeleteWithInvalidMedNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM medicationpat WHERE "//We no longer delete medicationpats where MedicationNum is 0 because we allow MedicationNums to be 0 for use in MU2.
						+ "(medicationpat.MedicationNum<>0 AND NOT EXISTS(SELECT * FROM medication WHERE medication.MedicationNum=medicationpat.MedicationNum))";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Medications found where no defition exists for them: " + numFound + "\r\n";
					}
					break;

				case DbmMode.Fix:
					command = "DELETE FROM medicationpat WHERE "//We no longer delete medicationpats where MedicationNum is 0 because we allow MedicationNums to be 0 for use in MU2.
						+ "(medicationpat.MedicationNum<>0 AND NOT EXISTS(SELECT * FROM medication WHERE medication.MedicationNum=medicationpat.MedicationNum))";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Medications deleted because no definition exists for them: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string MessageButtonDuplicateButtonIndex(bool verbose, DbmMode modeCur)
		{
			string queryStr = "SELECT COUNT(*) NumFound,SigButDefNum,ButtonIndex,ComputerName FROM sigbutdef GROUP BY ComputerName,ButtonIndex HAVING COUNT(*) > 1";
			DataTable table = Database.ExecuteDataTable(queryStr);
			int numFound = table.Select().Sum(x => PIn.Int(x["NumFound"].ToString()) - 1);//sum the duplicates; one in each group is valid.
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					if (numFound > 0 || verbose)
					{
						log += "Messaging buttons found with invalid button orders: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					do
					{
						//Loop through the messaging buttons and increment the duplicate button index by the max plus one.
						for (int i = 0; i < table.Rows.Count; i++)
						{
							command = "SELECT MAX(ButtonIndex) FROM sigbutdef WHERE ComputerName='" + table.Rows[i]["ComputerName"].ToString() + "'";
							int newIndex = Database.ExecuteInt(command) + 1;
							command = "UPDATE sigbutdef SET ButtonIndex=" + newIndex.ToString() + " WHERE SigButDefNum=" + table.Rows[i]["SigButDefNum"].ToString();
							Database.ExecuteNonQuery(command);
						}
						//It's possible we need to loop through several more times depending on how many items shared the same button index. 
						table = Database.ExecuteDataTable(queryStr);
					} while (table.Rows.Count > 0);
					if (numFound > 0 || verbose)
					{
						log += "Messaging buttons with invalid button orders fixed: " + numFound.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion MedicationPat, Medication, MessageButton---------------------------------------------------------------------------------------------
		#region Operatory, OrthoChart, PatField---------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string OperatoryInvalidReference(bool verbose, DbmMode modeCur)
		{
			//Get distinct operatory nums that have been orphaned from appointment, scheduleop, and apptviewitems.  
			//We use a UNION instead of UNION ALL because we want MySQL or Oracle to group duplicate OpNums together.
			string command = @"SELECT appointment.Op AS OpNum FROM appointment WHERE appointment.Op!=0 AND NOT EXISTS(SELECT * FROM operatory WHERE operatory.OperatoryNum=appointment.Op)
									UNION 
									SELECT scheduleop.OperatoryNum AS OpNum FROM scheduleop WHERE scheduleop.OperatoryNum!=0 AND NOT EXISTS(SELECT * FROM operatory WHERE operatory.OperatoryNum=scheduleop.OperatoryNum) 
									UNION 
									SELECT apptviewitem.OpNum AS OpNum FROM apptviewitem WHERE apptviewitem.OpNum!=0 AND NOT EXISTS(SELECT * FROM operatory WHERE operatory.OperatoryNum=apptviewitem.OpNum)";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Operatory references that are invalid: " + table.Rows.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					for (int i = 0; i < table.Rows.Count; i++)
					{
						long opNum = PIn.Long(table.Rows[i]["OpNum"].ToString());
						if (opNum != 0)
						{
                            Operatory op = new Operatory
                            {
                                Id = opNum,
                                OpName = "UNKNOWN-" + opNum,
                                Abbrev = "UNKN"
                            };
                            Crud.OperatoryCrud.Insert(op, true);
						}
					}
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Operatories created from an invalid operatory reference: " + table.Rows.Count + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string OrthoChartDeleteDuplicates(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = @"SELECT SUM(duplicates.CountDup) 
					FROM (
						SELECT COUNT(*)-1 CountDup 
						FROM orthochart 
						GROUP BY PatNum,DateService,BINARY FieldName,BINARY FieldValue 
						HAVING COUNT(*) > 1
					) duplicates";
			long numFound = PIn.Long(Database.ExecuteString(command));
			switch (modeCur)
			{
				case DbmMode.Check:
					if (numFound > 0 || verbose)
					{
						log += numFound.ToString() + " duplicate cell entries found.\r\n";
					}
					break;
				case DbmMode.Fix:
					if (numFound > 0)
					{
						//Holds the unique row that we will be keeping. All other rows like this one will be deleted (not copied to the renamed table) below.
						//The group by clause must use the keyword BINARY because the ortho chart within Open Dental is case sensitive.
						command = @"RENAME TABLE orthochart TO orthochartbak;
										CREATE TABLE orthochart LIKE orthochartbak;
										INSERT INTO orthochart
										SELECT orthochartbak.* FROM orthochartbak
										JOIN (
											SELECT MAX(OrthoChartNum) maxNum
											FROM orthochartbak
											GROUP BY PatNum,DateService,BINARY FieldName,BINARY FieldValue
											ORDER BY NULL
										) o2
										WHERE orthochartbak.OrthoChartNum=o2.maxNum";
						Database.ExecuteNonQuery(command);
						command = "DROP TABLE IF EXISTS orthochartbak";
						Database.ExecuteNonQuery(command);
						log += "All exact duplicate entries have been removed ";
					}
					//check to see if there are duplicate date entries,fieldnames which aren't supposed to occur. This means there is conflict one needs to be chosen.
					//If a user calls in due to the following message, an engineer should run the following query and help the user address each chart.
					//If the duplicate count is unreasonable (20+) then we should make a job for creating a tool for the user to use in order to "combine" them.
					//The manual fix would be something along the lines of manually going each ortho chart, removing duplicate values by opening and closing
					//the ortho chart several times until the field that had duplicates loads with no data.  At that point all duplicates should have been
					//removed and the new / correct value can be entered by the user.
					command = "SELECT * FROM orthochart GROUP BY PatNum,DateService,BINARY FieldName HAVING COUNT(*) > 1";
					DataTable table = Database.ExecuteDataTable(command);
					if (table.Rows.Count > 0)
					{
						log += 
							"Potential duplicate entries could not be deleted for all ortho charts. " +
							table.Rows.Count + " possible duplicates remaining. " +
							"Please call support and escalate to an engineer to confirm and remove entries.";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string OrthoChartFieldsWithoutValues(bool verbose, DbmMode modeCur)
		{
			string queryStr = "SELECT COUNT(*) FROM orthochart WHERE FieldValue=''";
			int numFound = PIn.Int(Database.ExecuteString(queryStr));
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (numFound > 0 || verbose)
					{
						log += "Ortho chart fields without values found" + ": " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					string command = "DELETE FROM orthochart WHERE FieldValue=''";
					Database.ExecuteNonQuery(command);
					if (numFound > 0 || verbose)
					{
						log += "Ortho chart fields without values fixed" + ": " + numFound.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PatFieldsDeleteDuplicates(bool verbose, DbmMode modeCur)
		{
			//This code is only needed for older db's. New DB's created after 12.2.30 and 12.3.2 shouldn't need this.
			string command = @"DROP TABLE IF EXISTS tempduplicatepatfields";
			Database.ExecuteNonQuery(command);
			string tableName = "tempduplicatepatfields" + MiscUtils.CreateRandomAlphaNumericString(8);//max size for a table name in oracle is 30 chars.
																									  //This query run very fast on a db with no corruption.
			command = @"CREATE TABLE " + tableName + @"
								SELECT *
								FROM patfield
								GROUP BY PatNum,FieldName
								HAVING COUNT(*)>1";
			Database.ExecuteNonQuery(command);
			command = @"SELECT patient.PatNum,LName,FName
								FROM patient 
								INNER JOIN " + tableName + @" t ON t.PatNum=patient.PatNum
								GROUP BY patient.PatNum";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Patients with duplicate field entries found:" + " " + table.Rows.Count + ".\r\n";
						log += "   " + "Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0 || verbose)
					{
						StringBuilder strBuild = new StringBuilder();
						strBuild.AppendLine("The following patients had corrupt Patient Fields.  "
							+ "Please verify the Patient Fields of these patients:");
						foreach (DataRow row in table.Rows)
						{
							strBuild.AppendLine("#" + row["PatNum"].ToString() + " " + row["LName"] + ", " + row["FName"]);
						}
						strBuild.AppendLine("Patients with duplicate field entries found:" + " " + table.Rows.Count);
						log += strBuild.ToString();
					}
					break;
				case DbmMode.Fix:
					if (table.Rows.Count > 0)
					{
						//Without this index the delete process takes too long.
						command = "ALTER TABLE " + tableName + " ADD INDEX(PatNum)";
						Database.ExecuteNonQuery(command);
						command = "ALTER TABLE " + tableName + " ADD INDEX(FieldName)";
						Database.ExecuteNonQuery(command);
						command = "DELETE FROM patfield WHERE ((PatNum, FieldName) IN (SELECT PatNum, FieldName FROM " + tableName + "))";
						Database.ExecuteNonQuery(command);
						command = "INSERT INTO patfield SELECT * FROM " + tableName;
						Database.ExecuteNonQuery(command);
					}
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Patients with duplicate field entries removed:" + " " + table.Rows.Count + "\r\n";
					}
					break;
			}
			command = @"DROP TABLE IF EXISTS " + tableName;
			Database.ExecuteNonQuery(command);
			return log;
		}

		#endregion Operatory, OrthoChart, PatField------------------------------------------------------------------------------------------------------
		#region Patient---------------------------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string PatientBadGuarantor(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT p.PatNum,p.Guarantor FROM patient p LEFT JOIN patient p2 ON p.Guarantor=p2.PatNum WHERE p2.PatNum IS NULL";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Patients with invalid Guarantors found: " + table.Rows.Count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					for (int i = 0; i < table.Rows.Count; i++)
					{
						long patNum = PIn.Long(table.Rows[i]["PatNum"].ToString());
						long guarantor = PIn.Long(table.Rows[i]["Guarantor"].ToString());
						command = "UPDATE patient SET Guarantor=PatNum WHERE PatNum=" + patNum;
						Database.ExecuteNonQuery(command);
						listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, patNum, DbmLogFKeyType.Patient, DbmLogActionType.Update, methodName,
							"Updated Guarantor from " + guarantor + " to " + patNum + " from PatientBadGuarantor."));
					}
					int numberFixed = table.Rows.Count;
					if (numberFixed > 0 || verbose)
					{
						log += "Patients with invalid Guarantors fixed: " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatientBadGuarantorWithAnotherGuarantor(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT p.PatNum,p2.Guarantor FROM patient p LEFT JOIN patient p2 ON p.Guarantor=p2.PatNum WHERE p2.PatNum!=p2.Guarantor";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Patients with a guarantor who has another guarantor found: " + table.Rows.Count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					for (int i = 0; i < table.Rows.Count; i++)
					{
						long patNum = PIn.Long(table.Rows[i]["PatNum"].ToString());
						long guarantor = PIn.Long(table.Rows[i]["Guarantor"].ToString());
						command = "UPDATE patient SET Guarantor=" + guarantor + " WHERE PatNum=" + patNum;
						Database.ExecuteNonQuery(command);
						listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, patNum, DbmLogFKeyType.Patient, DbmLogActionType.Update, methodName,
							"Updated Guarantor to " + guarantor + " from PatientBadGuarantorWithAnotherGuarantor."));
					}
					int numberFixed = table.Rows.Count;
					if (numberFixed > 0 || verbose)
					{
						log += "Patients with a guarantor who has another guarantor fixed: " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatientDeletedWithClinicNumSet(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM patient WHERE ClinicNum!=0 AND PatStatus=" + POut.Int((int)PatientStatus.Deleted);
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Deleted patients with a clinic still set: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					string where = "WHERE ClinicNum!=0 AND PatStatus=" + POut.Int((int)PatientStatus.Deleted);
					command = "SELECT * FROM patient " + where;
					List<Patient> listPatients = Crud.PatientCrud.SelectMany(command);
					command = "UPDATE patient SET ClinicNum=0 " + where;
					long numberFixed = Database.ExecuteNonQuery(command);
					listPatients.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.PatNum, DbmLogFKeyType.Patient,
						DbmLogActionType.Update, methodName, "Updated ClinicNum from " + x.ClinicNum + " to 0.")));
					if (numberFixed > 0 || verbose)
					{
						log += "Deleted patients with clinics cleared: " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatientInvalidGradeLevel(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM patient WHERE GradeLevel < 0";//Any negative number is considered invalid.
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Patients with invalid GradeLevel set: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "SELECT * FROM patient WHERE GradeLevel < 0";
					List<Patient> listPatients = Crud.PatientCrud.SelectMany(command);
					//Set all invalid Grade Levels to Unknown.
					command = "UPDATE patient SET GradeLevel=" + POut.Int((int)PatientGrade.Unknown) + " WHERE GradeLevel < 0";
					long numFixed = Database.ExecuteNonQuery(command);
					listPatients.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.PatNum, DbmLogFKeyType.Patient,
						DbmLogActionType.Update, methodName, "Updated GradeLevel from " + x.GradeLevel + " to " + POut.Int((int)PatientGrade.Unknown) + ".")));
					if (numFixed > 0 || verbose)
					{
						log += "Patients with invalid GradeLevel fixed: " + numFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		///<summary>Finds Patients in a SuperFamily where the SuperHead is no longer in the SuperFamily.  This occurs when the SuperHead is moved out of 
		///the SuperFamily but the remaining SuperFamily members do not have their SuperFamily field updated.  Since we cannot reliably choose a member 
		///of the remaining SuperFamily as the new SuperHead, we use the new Guarantor of the previous SuperHead as the new SuperHead, or in the event 
		///the old SuperHead has been moved to a new SuperFamily we use the SuperHead of that SuperFamily, effectively merging the SuperFamily into this 
		///new Family/SuperFamily where the previous SuperHead now resides.</summary>
		[DbmMethodAttr]
		public static string PatientInvalidSuperFamilyHead(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT p1.PatNum, p2.PatNum AS OldHead, IF(p2.SuperFamily=0,p2.Guarantor,p2.SuperFamily) AS NewHead "
				+ "FROM patient p1 "
				+ "INNER JOIN patient p2 on p1.SuperFamily=p2.PatNum "//Bring on patient SuperFamily head
				+ "AND p2.PatNum!=p2.SuperFamily";//Limit down to patients associated to invalid super family heads.
			DataTable tablePatientsWithInvalidSuperHead = Database.ExecuteDataTable(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = tablePatientsWithInvalidSuperHead.Rows.Count;
					if (numFound > 0 || verbose)
					{
						log += "Patients in a SuperFamily with invalid super head: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					List<string> listInvalidSuperFamilyHeadPatNums = new List<string>();
					long countFixed = 0;
					long countValidPatientsStartingNewSuperFamily = 0;
					string methodName = MethodBase.GetCurrentMethod().Name;
					foreach (DataRow row in tablePatientsWithInvalidSuperHead.Rows)
					{
						string patNum = POut.String(row["PatNum"].ToString());
						string oldSuperFamilyHeadNum = POut.String(row["OldHead"].ToString());
						string newSuperFamilyHeadNum = POut.String(row["NewHead"].ToString());
						if (!listInvalidSuperFamilyHeadPatNums.Contains(oldSuperFamilyHeadNum))
						{//Only run UPDATE once per distinct invalid super head.
							listInvalidSuperFamilyHeadPatNums.Add(oldSuperFamilyHeadNum);
							//Get all patients who are currently in a valid Family (not SuperFamily), but will have their SuperFamily changed in order for the 
							//patients with an invalid SuperFamily head to be moved to a new valid SuperFamily. Only applies when the oldSuperFamilyHead is in a
							//Family which is not already part of a SuperFamily.
							command = "SELECT patient.PatNum, patient.SuperFamily FROM patient "
								+ "WHERE (patient.PatNum=" + newSuperFamilyHeadNum + " AND " + "patient.SuperFamily!=" + newSuperFamilyHeadNum + ") "//New super head.
								+ "OR (patient.Guarantor=" + newSuperFamilyHeadNum + " AND " + "patient.SuperFamily!=" + newSuperFamilyHeadNum + ")";//Dependents.
							DataTable tableValidPatientsMovingToNewSuperFamily = Database.ExecuteDataTable(command);
							//Three groups of patients need to be updated.
							//1. Update all the patients who had an invalid SuperFamily head because they were invalid.
							//2. Update the new superfamily head to be in the SuperFamily because otherwise we would be reproducing the invalid SuperFamily head 
							//scenario with a different patient.
							//3. Update the dependents of the new SuperFamily head to be in the SuperFamily as well, because in the UI adding any family member to a 
							//SuperFamily will also bring along all other family members into the SuperFamily.
							command = "UPDATE patient SET patient.SuperFamily=" + newSuperFamilyHeadNum + " "
								+ "WHERE patient.SuperFamily=" + oldSuperFamilyHeadNum + " "
								+ "OR (patient.PatNum=" + newSuperFamilyHeadNum + " AND " + "patient.SuperFamily!=" + newSuperFamilyHeadNum + ") "
								+ "OR (patient.Guarantor=" + newSuperFamilyHeadNum + " AND " + "patient.SuperFamily!=" + newSuperFamilyHeadNum + ")";
							//(All patients updated)-(prev valid patients)=patients fixed
							countFixed += Database.ExecuteNonQuery(command) - tableValidPatientsMovingToNewSuperFamily.Rows.Count;
							countValidPatientsStartingNewSuperFamily += tableValidPatientsMovingToNewSuperFamily.Rows.Count;
							//Since we are changing the SuperFamily on previously valid patients, we need to log them as well.
							foreach (DataRow rowValidPat in tableValidPatientsMovingToNewSuperFamily.Rows)
							{
								long validPatNum = PIn.Long(rowValidPat["PatNum"].ToString());
								long validSuperFamilyNum = PIn.Long(rowValidPat["SuperFamily"].ToString());
								listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, validPatNum, DbmLogFKeyType.Patient,//Log for each patient.
									DbmLogActionType.Update, methodName, "Updated SuperFamily from " + validSuperFamilyNum + " to " + newSuperFamilyHeadNum + "."));
							}
						}
						listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, PIn.Long(patNum), DbmLogFKeyType.Patient,//Log for each patient.
							DbmLogActionType.Update, methodName, "Updated SuperFamily from " + oldSuperFamilyHeadNum + " to " + newSuperFamilyHeadNum + "."));
					}
					if (countFixed > 0 || verbose)
					{
						log += 
							"Patients in a SuperFamily with invalid super head fixed: " + countFixed.ToString() + "\r\n" +
							"Previously valid Patients incorporated into a new SuperFamily: " + countValidPatientsStartingNewSuperFamily.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					//This is the first implementation of a Dbm that should be moved to the Old tab once it has been run once.  The thought is that we have 
					//added a bug fix that prevents this scenario from occurring again, so it is unreasonable to include the Dbm in the normal list of 'Checks'
					//anymore, where it would run every time and eat up processing power and time.
					MoveToOld(methodName);
					log += "DatabaseMaintenance method moved to Old tab" + ": " + methodName;
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PatientsNoClinicSet(bool verbose, DbmMode modeCur)
		{
			if (!PrefC.HasClinicsEnabled)
			{
				return "";
			}
			//Get patients not assigned to a clinic:
			string command = @"SELECT PatNum,LName,FName FROM patient WHERE ClinicNum=0 AND PatStatus!=" + POut.Int((int)PatientStatus.Deleted);
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "Patients with no Clinic assigned: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   Manual fix needed.  Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", including" + ":\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							//Start a new line and indent every three patients for printing purposes.
							if (i % 3 == 0)
							{
								log += "\r\n   ";
							}
							log += table.Rows[i]["PatNum"].ToString() + "-"
							+ table.Rows[i]["LName"].ToString() + ", "
							+ table.Rows[i]["FName"].ToString() + "; ";
						}
						log += "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PatientPriProvHidden(bool verbose, DbmMode modeCur)
		{
			string command = @"
				SELECT provider.ProvNum,provider.Abbr
				FROM provider
				INNER JOIN patient ON patient.PriProv=provider.ProvNum
				WHERE provider.IsHidden=1
				GROUP BY provider.ProvNum";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "Hidden providers with patients: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   Manual fix needed.  Double click to see a break down.\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", including:\r\n";
						if (table.Rows.Count > 0 || verbose)
						{
							DataTable patTable;
							for (int i = 0; i < table.Rows.Count; i++)
							{
								log += "     " + table.Rows[i]["Abbr"].ToString() + ": ";
								command = @"SELECT PatNum,LName,FName FROM patient WHERE PriProv=(SELECT ProvNum FROM provider WHERE ProvNum="
									+ table.Rows[i]["ProvNum"].ToString() + " AND IsHidden=1) LIMIT 10";
								patTable = Database.ExecuteDataTable(command);
								for (int j = 0; j < patTable.Rows.Count; j++)
								{
									if (j > 0)
									{
										log += ", ";
									}
									log += patTable.Rows[j]["PatNum"].ToString() + "-" + patTable.Rows[j]["FName"].ToString() + " " + patTable.Rows[j]["LName"].ToString();
								}
								log += "\r\n";
							}
							log += "   Go to Lists | Providers to move all patients from the hidden provider to another provider.\r\n";
						}
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatientPriProvMissing(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = @"SELECT COUNT(*) FROM patient WHERE PriProv=0";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Patient pri provs not set: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "SELECT PatNum FROM patient WHERE PriProv=0";
					List<long> listPatNums = Database.GetListLong(command);
					//previous versions of the program just dealt gracefully with missing provnum.
					//From now on, we can assum priprov is not missing, making coding easier.
					command = @"UPDATE patient SET PriProv=" + Preferences.GetString(PreferenceName.PracticeDefaultProv) + " WHERE PriProv=0";
					long numberFixed = Database.ExecuteNonQuery(command);
					listPatNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Patient, DbmLogActionType.Update,
						methodName, "Updated PriProv from 0 to " + Preferences.GetString(PreferenceName.PracticeDefaultProv))));
					if (numberFixed > 0 || verbose)
					{
						log += "Patient pri provs fixed: " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PatientUnDeleteWithBalance(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT PatNum FROM patient "
				+ "WHERE PatStatus=4 "
				+ "AND (Bal_0_30 !=0 "
					+ "OR Bal_31_60 !=0 "
					+ "OR Bal_61_90 !=0 "
					+ "OR BalOver90 !=0 "
					+ "OR InsEst !=0 "
					+ "OR BalTotal !=0)";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Patients found who are marked deleted with non-zero balances: " + table.Rows.Count + "\r\n";
						log += "   Double click to see a break down.\r\n";
					}
					break;
				case DbmMode.Breakdown:  //No DB changes made, see breakdown if below.
				case DbmMode.Fix:
					Patient old;
					List<Patient> listPatients = Patients.GetMultPats(table.Select().Select(x => PIn.Long(x["PatNum"].ToString())).ToList()).ToList();
					if (table.Rows.Count > 0 || verbose)
					{
						if (modeCur == DbmMode.Fix)
						{
							List<DbmLog> listDbmLogs = new List<DbmLog>();
							string methodName = MethodBase.GetCurrentMethod().Name;
							foreach (Patient pat in listPatients)
							{
								old = pat.Copy();
								pat.LName += "DELETED";
								pat.PatStatus = PatientStatus.Archived;
								Patients.Update(pat, old);
								listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, pat.PatNum, DbmLogFKeyType.Patient, DbmLogActionType.Update,
									methodName, "Updated the PatStatus from Deleted to Archived from PatientUnDeleteWithBalance"));
							}
							Crud.DbmLogCrud.InsertMany(listDbmLogs);
							log += "Patients with non-zero balances that have been undeleted: " + listPatients.Count;
						}
						else
						{//Breakdown
							log += 
								"The following patients are marked as Deleted but have a balance. " +
								"They will have 'DELETED' appended to their last name and their status will be changed to Archive. " +
								"This will allow the account to be accessed so that it can be manually cleared and then deleted again." +
								"\r\n\r\n";

							log += string.Join("\r\n", listPatients.Select(x => $"#{x.PatNum} - {x.GetNameFL()}"));
						}
					}
					break;
			}
			return log;
		}

		#endregion Patient------------------------------------------------------------------------------------------------------------------------------
		#region PatPlan, Payment------------------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string PatPlanDeleteWithInvalidInsSubNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM patplan WHERE InsSubNum NOT IN (SELECT InsSubNum FROM inssub)";
					string countStr = Database.ExecuteString(command);
					if (countStr != "0" || verbose)
					{
						log += "Pat plans found with invalid InsSubNums: " + countStr + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "SELECT PatPlanNum FROM patplan WHERE InsSubNum NOT IN (SELECT InsSubNum FROM inssub)";
					List<long> listPatPlanNums = Database.GetListLong(command);
					command = "DELETE FROM patplan WHERE InsSubNum NOT IN (SELECT InsSubNum FROM inssub)";
					long numberFixed = Database.ExecuteNonQuery(command);
					listPatPlanNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.PatPlan,
						DbmLogActionType.Delete, methodName, "Deleted patplan from PatPlanDeleteWithInvalidInsSubNum.")));
					if (numberFixed > 0 || verbose)
					{
						log += "Pat plans with invalid InsSubNums deleted: " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatPlanDeleteWithInvalidPatNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM patplan WHERE PatNum NOT IN (SELECT PatNum FROM patient)";
					string countStr = Database.ExecuteString(command);
					if (countStr != "0" || verbose)
					{
						log += "Pat plans found with invalid PatNums: " + countStr + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "SELECT PatPlanNum FROM patplan WHERE PatNum NOT IN (SELECT PatNum FROM patient)";
					List<long> listPatPlanNums = Database.GetListLong(command);
					command = "DELETE FROM patplan WHERE PatNum NOT IN (SELECT PatNum FROM patient)";
					long numberFixed = Database.ExecuteNonQuery(command);
					listPatPlanNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.PatPlan,
						DbmLogActionType.Delete, methodName, "Deleted patplan from PatPlanDeleteWithInvalidPatNum.")));
					if (numberFixed > 0 || verbose)
					{
						log += "Pat plans with invalid PatNums deleted: " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PatPlanOrdinalDuplicates(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT patient.PatNum,patient.LName,patient.FName,COUNT(*) "
				+ "FROM patplan "
				+ "INNER JOIN patient ON patient.PatNum=patplan.PatNum "
				+ "GROUP BY patplan.PatNum,patplan.Ordinal "
				+ "HAVING COUNT(*)>1";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "PatPlan duplicate ordinals: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   " + "Manual fix needed.  Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", " + "including" + ":\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "   #" + table.Rows[i]["PatNum"].ToString() + " - " + PIn.String(table.Rows[i]["FName"].ToString()) + " " + PIn.String(table.Rows[i]["LName"].ToString()) + "\r\n";
						}
						log += "   They need to be fixed manually." + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatPlanOrdinalZeroToOne(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT PatPlanNum,PatNum FROM patplan WHERE Ordinal=0";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "PatPlan ordinals currently zero: " + table.Rows.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					int numberFixed = 0;
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					for (int i = 0; i < table.Rows.Count; i++)
					{
						PatPlan patPlan = PatPlans.GetPatPlan(PIn.Long(table.Rows[i][1].ToString()), 0);
						if (patPlan != null)
						{//Unlikely but possible if plan gets deleted by a user during this check.
							PatPlans.SetOrdinal(patPlan.PatPlanNum, 1);
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, patPlan.PatPlanNum, DbmLogFKeyType.PatPlan,
								DbmLogActionType.Update, methodName, "PatPlan ordinal changed from 0 to 1 from PatPlanOrdinalZeroToOne."));
							numberFixed++;
						}
					}
					if (numberFixed > 0 || verbose)
					{
						log += "PatPlan ordinals changed from 0 to 1: " + numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatPlanOrdinalTwoToOne(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT PatPlanNum,PatNum FROM patplan patplan1 WHERE Ordinal=2 AND NOT EXISTS("
				+ "SELECT * FROM patplan patplan2 WHERE patplan1.PatNum=patplan2.PatNum AND patplan2.Ordinal=1)";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "PatPlans for secondary found where no primary ins: " + table.Rows.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					int numberFixed = 0;
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					for (int i = 0; i < table.Rows.Count; i++)
					{
						PatPlan patPlan = PatPlans.GetPatPlan(PIn.Long(table.Rows[i][1].ToString()), 2);
						if (patPlan != null)
						{//Unlikely but possible if plan gets deleted by a user during this check.
							PatPlans.SetOrdinal(patPlan.PatPlanNum, 1);
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, patPlan.PatPlanNum, DbmLogFKeyType.PatPlan,
								DbmLogActionType.Update, methodName, "PatPlan ordinal changed from 2 to 1 from PatPlanOrdinalTwoToOne."));
							numberFixed++;
						}
					}
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "PatPlan ordinals changed from 2 to 1 if no primary ins: " + numberFixed + "\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Shows payments that have a PaymentAmt that doesn't match the sum of all associated PaySplits.  
		///Payments with no PaySplits are dealt with in PaymentMissingPaySplit()</summary>
		[DbmMethodAttr(HasBreakDown = true)]
		public static string PaymentAmtNotMatchPaySplitTotal(bool verbose, DbmMode modeCur)
		{
			//Note that this just returns info for a (seemingly) random patient that has a paysplit for the payment.
			//This is because the payment only shows in the ledger for the patient with the paysplit, not the patient on the payment.
			string command = "SELECT patient.PatNum, patient.LName, patient.FName, payment.PayDate "
				+ "FROM payment "
				+ "INNER JOIN ( "
					+ "SELECT paysplit.PayNum, SUM(paysplit.SplitAmt) totSplitAmt, MIN(paysplit.PatNum) PatNum "
					+ "FROM paysplit "
					+ "GROUP BY paysplit.PayNum "
				+ ") pstotals ON pstotals.PayNum=payment.PayNum "
				+ "INNER JOIN patient ON patient.PatNum=pstotals.PatNum "
				+ "WHERE payment.PayAmt!=0 "
				+ "AND ROUND(payment.PayAmt,2)!=ROUND(pstotals.totSplitAmt,2) "
				+ "ORDER BY patient.LName, patient.FName, payment.PayDate";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "Payments with amounts that do not match the total split(s) amounts" + ": " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   " + "Manual fix needed.  Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", " + "including" + ":\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "   " + table.Rows[i]["PatNum"].ToString();
							log += "  " + Patients.GetNameLF(table.Rows[i]["LName"].ToString(), table.Rows[i]["FName"].ToString(), "", "");
							log += "  " + PIn.Date(table.Rows[i]["PayDate"].ToString()).ToShortDateString();
							log += "\r\n";
						}
						log += "   " + "They need to be fixed manually." + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PaymentDetachMissingDeposit(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM payment "
						+ "WHERE DepositNum != 0 "
						+ "AND NOT EXISTS(SELECT * FROM deposit WHERE deposit.DepositNum=payment.DepositNum)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Payments attached to deposits that no longer exist: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					string where = "WHERE DepositNum != 0 AND NOT EXISTS(SELECT * FROM deposit WHERE deposit.DepositNum=payment.DepositNum)";
					command = "SELECT * FROM payment " + where;
					List<Payment> listPayments = Crud.PaymentCrud.SelectMany(command);
					command = "UPDATE payment SET DepositNum=0 " + where;
					long numberFixed = Database.ExecuteNonQuery(command);
					listPayments.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x.PayNum, DbmLogFKeyType.Payment,
						DbmLogActionType.Update, methodName, "Updated DepositNum from " + x.DepositNum + " to 0.")));
					if (numberFixed > 0 || verbose)
					{
						log += "Payments detached from deposits that no longer exist: "
						+ numberFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PaymentMissingPaySplit(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM payment "
						+ "WHERE PayNum NOT IN (SELECT PayNum FROM paysplit) "//Payments with no split that are
						+ "AND ((DepositNum=0) "                              //not attached to a deposit
						+ "OR (DepositNum!=0 AND PayAmt=0))";                 //or attached to a deposit with no amount.
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Payments with no attached paysplit: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					string where = "WHERE PayNum NOT IN (SELECT PayNum FROM paysplit) "//Payments with no split that are
						+ "AND ((DepositNum=0) "                              //not attached to a deposit
						+ "OR (DepositNum!=0 AND PayAmt=0))";                 //or attached to a deposit with no amount.
					command = "SELECT PayNum FROM payment " + where;
					List<long> listPayNums = Database.GetListLong(command);
					command = "DELETE FROM payment " + where;
					long numberFixed = Database.ExecuteNonQuery(command);
					listPayNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Payment,
						DbmLogActionType.Delete, methodName, "Deleted payment from PaymentMissingPaySplit.")));
					if (numberFixed > 0 || verbose)
					{
						log += "Payments with no attached paysplit fixed: " + numberFixed + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		#endregion PatPlan, Payment---------------------------------------------------------------------------------------------------------------------
		#region PayPlanCharge, PayPlan, PaySplit--------------------------------------------------------------------------------------------------------

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PayPlanChargeProvNum(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT pat.PatNum AS 'PatNum',pat.LName AS 'PatLName',pat.FName AS 'PatFName',guar.PatNum AS 'GuarNum',guar.LName AS 'GuarLName',guar.FName AS 'GuarFName',payplan.PayPlanDate "
				+ "FROM payplancharge "
				+ "LEFT JOIN payplan ON payplancharge.PayPlanNum=payplan.PayPlanNum "
				+ "LEFT JOIN patient pat ON payplan.PatNum=pat.PatNum "
				+ "LEFT JOIN patient guar ON payplan.Guarantor=guar.PatNum "
				+ "WHERE payplancharge.ProvNum=0 "
				+ "GROUP BY payplancharge.PayPlanNum";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "Pay plans with charges that have providers missing: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   " + "Manual fix needed.  Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", including:\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "   " + "Pay Plan Date" + ": " + PIn.Date(table.Rows[i]["PayPlanDate"].ToString()).ToShortDateString() + "\r\n"
							+ "      " + "Guarantor" + ": #" + table.Rows[i]["PatNum"] + " - " + table.Rows[i]["PatFName"] + " " + table.Rows[i]["PatLName"] + "\r\n"
							+ "      " + "For Patient" + ": #" + table.Rows[i]["GuarNum"] + " - " + table.Rows[i]["GuarFName"] + " " + table.Rows[i]["GuarLName"] + "\r\n";
						}
						log += "   They need to be fixed manually.\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PayPlanChargeWithInvalidPayPlanNum(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT COUNT(DISTINCT PayPlanNum) FROM (SELECT PayPlanNum FROM payplancharge WHERE PayPlanNum NOT IN(SELECT PayPlanNum FROM payplan) "
				+ "UNION SELECT PayPlanNum FROM creditcard WHERE PayPlanNum>0 AND PayPlanNum NOT IN(SELECT PayPlanNum FROM payplan)) A";
			string count = Database.ExecuteString(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (count != "0" || verbose)
					{
						log += "PayPlan charges or credit cards with an invalid PayPlanNum found: " + count;
					}
					break;

				case DbmMode.Fix:
					if (count != "0" || verbose)
					{
						List<DbmLog> listDbmLogs = new List<DbmLog>();
						string methodName = MethodBase.GetCurrentMethod().Name;
						string where = "WHERE PayPlanNum NOT IN(SELECT PayPlanNum FROM payplan)";
						//Delete the payment plan charges and update credit cards that point to an invalid payment plan. Claimprocs and paysplits with an invalid
						//PayPlanNum are taken care of in other DBM methods.
						command = "SELECT PayPlanChargeNum FROM payplancharge " + where;
						List<long> listPrikeys = Database.GetListLong(command);
						command = "DELETE FROM payplancharge " + where;
						Database.ExecuteNonQuery(command);
						listPrikeys.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.PayPlanCharge,
							DbmLogActionType.Delete, methodName, "Deleted paysplancharge from PayPlanChargeWithInvalidPayPlanNum.")));
						command = "SELECT CreditCardNum FROM creditcard " + where;
						listPrikeys = Database.GetListLong(command);
						command = "UPDATE creditcard SET PayPlanNum=0 " + where;
						Database.ExecuteNonQuery(command);
						listPrikeys.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.CreditCard,
							DbmLogActionType.Update, methodName, "Set creditcard.PayPlanNum to 0 from PayPlanChargeWithInvalidPayPlanNum.")));
						log += "PayPlan charges or credit cards with an invalid PayPlanNum fixed: " + count;
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PayPlanSetGuarantorToPatForIns(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT COUNT(*) FROM payplan WHERE PlanNum>0 AND Guarantor != PatNum";
			int numFound = PIn.Int(Database.ExecuteString(command));
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (numFound > 0 || verbose)
					{
						log += "PayPlan Guarantors not equal to PatNum where used for insurance tracking: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					//Too dangerous to do anything at all.  Just have a very descriptive explanation in the check.
					//For now, tell the user that a fix is under development.
					if (numFound > 0 || verbose)
					{
						log += "PayPlan Guarantors not equal to PatNum where used for insurance tracking: " + numFound + "\r\n";
						log += "   A safe fix is under development." + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PaySplitAttachedToDeletedProc(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT CONCAT(pat.LName,' ',pat.FName) AS 'PatientName', payment.PatNum, payment.PayDate, payment.PayAmt, "
				+ "paysplit.DatePay, procedurecode.AbbrDesc, CONCAT(splitpat.FName,' ',splitpat.LName) AS 'SplitPatientName', paysplit.SplitAmt "
				+ "FROM paysplit "
				+ "INNER JOIN payment ON payment.PayNum=paysplit.PayNum "
				+ "INNER JOIN procedurelog ON paysplit.ProcNum=procedurelog.ProcNum AND procedurelog.ProcStatus=" + POut.Int((int)ProcStat.D) + " "
				+ "INNER JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
				+ "INNER JOIN patient pat ON pat.PatNum=payment.PatNum "
				+ "INNER JOIN patient splitpat ON splitpat.PatNum=paysplit.PatNum "
				+ "ORDER BY pat.LName, pat.FName, payment.PayDate, paysplit.DatePay, procedurecode.AbbrDesc";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "Paysplits attached to deleted procedures: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   Manual fix needed.  Double click to see a break down.\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", including:\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "   Payment: #" + table.Rows[i]["PatNum"].ToString();
							log += " " + table.Rows[i]["PatientName"].ToString();
							log += " " + PIn.Date(table.Rows[i]["PayDate"].ToString()).ToShortDateString();
							log += " " + PIn.Double(table.Rows[i]["PayAmt"].ToString()).ToString("c");
							log += "\r\n      Split: " + PIn.Date(table.Rows[i]["DatePay"].ToString()).ToShortDateString();
							log += " " + table.Rows[i]["SplitPatientName"].ToString();
							log += " " + table.Rows[i]["AbbrDesc"].ToString();
							log += " " + PIn.Double(table.Rows[i]["SplitAmt"].ToString()).ToString("c");
							log += "\r\n";
						}
						log += "   They need to be fixed manually.\r\n";
					}
					break;
			}
			return log;
		}

		/// <summary>Shows patients that have paysplits attached to insurance payment plans.</summary>
		[DbmMethodAttr(HasBreakDown = true)]
		public static string PaySplitAttachedToInsurancePaymentPlan(bool verbose, DbmMode modeCur)
		{
			DataTable table = GetPaySplitsAttachedToInsurancePaymentPlan();
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "Paysplits attached to insurance payment plans" + ": " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n" + "Manual fix needed. Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", " + "including" + ":\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "\r\n   " + "Patient #" + " " + table.Rows[i]["PatNum"].ToString() + " "
								+ "Amount" + ": " + PIn.Double(table.Rows[i]["SplitAmt"].ToString()).ToString("c") + " "
								+ "Date" + ": " + PIn.Date(table.Rows[i]["DatePay"].ToString()).ToShortDateString() + " "
								+ "Insurance payment plan #" + table.Rows[i]["PayPlanNum"];
						}
						log += "\r\nRun 'Pay Plan Payments' in the Tools tab to fix these payments.";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = false)]
		public static string PaySplitAttachedToItself(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT * FROM paysplit WHERE FSplitNum=SplitNum";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					log += "Paysplits attached to themselves: " + table.Rows.Count + "\r\n";
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					List<long> listPaySplitNums = table.Select().Select(x => PIn.Long(x["SplitNum"].ToString())).ToList();
					command = "UPDATE paysplit SET paysplit.FSplitNum=0 WHERE paysplit.FSplitNum=paysplit.SplitNum";
					long numFixed = Database.ExecuteNonQuery(command);
					listPaySplitNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.PaySplit,
						DbmLogActionType.Update, methodName, "Updated FSplitNum to 0 from PaySplitAttachedToItself.")));
					if (numFixed > 0 || verbose)
					{
						log += "Paysplits with invalid FSplitNums fixed: " + numFixed + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PaySplitTransfersWithNoUnearnedType(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string methodName = MethodBase.GetCurrentMethod().Name;
			//If the parent is an unearned split (has a prepayment type) then update the unearned type on the current split to match the unearned type of the parent.
			string command = @"SELECT pschild.SplitNum,psparent.UnearnedType FROM paysplit pschild
				INNER JOIN paysplit psparent ON pschild.FSplitNum=psparent.SplitNum
				WHERE pschild.SplitAmt < 0 
				AND pschild.FSplitNum!=0 
				AND pschild.UnearnedType=0
				AND pschild.AdjNum=0
				AND pschild.ProcNum=0
				AND pschild.PayPlanNum=0
				AND psparent.UnearnedType!=0
				AND psparent.UnearnedType!=pschild.UnearnedType";
			DataTable tableSplitsWithoutUnearnedType = Database.ExecuteDataTable(command);
			int count = tableSplitsWithoutUnearnedType.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
					if (count > 0 || verbose)
					{
						log += "Paysplit transfers with no UnearnedType" + ": " + count;
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					foreach (DataRow row in tableSplitsWithoutUnearnedType.Rows)
					{
						long splitNum = PIn.Long(row["SplitNum"].ToString());
						long unearnedType = PIn.Long(row["UnearnedType"].ToString());
						listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, splitNum, DbmLogFKeyType.PaySplit
							, DbmLogActionType.Update, methodName, $"Updated unearned type from 0 to {unearnedType}"));
						command = $@"UPDATE paysplit 
							SET UnearnedType = {POut.Long(unearnedType)} 
							WHERE SplitNum = {POut.Long(splitNum)}";
						Database.ExecuteNonQuery(command);
					}
					DbmLogs.InsertMany(listDbmLogs);
					if (count > 0 || verbose)
					{
						log += "Paysplit transfers with no UnearnedType fixed" + ": " + count;
					}
					MoveToOld(methodName);
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasPatNum = true)]
		public static string PaySplitWithInvalidPayNum(bool verbose, DbmMode modeCur, long patNum = 0)
		{
			string log = "";
			string command;
			string patWhere = (patNum > 0 ? ("paysplit.PatNum=" + POut.Long(patNum) + " AND ") : "");
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM paysplit WHERE "
						+ patWhere
						+ "NOT EXISTS(SELECT * FROM payment WHERE paysplit.PayNum=payment.PayNum)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Paysplits found with invalid PayNum: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = @"SELECT *,SUM(SplitAmt) SplitAmt_ 
						FROM paysplit WHERE "
						+ patWhere
						+ "NOT EXISTS(SELECT * FROM payment WHERE paysplit.PayNum=payment.PayNum) "
						+ "AND PayNum!=0 "
						+ "GROUP BY PayNum";
					DataTable table = Database.ExecuteDataTable(command);
					int rowsFixed = 0;
					if (table.Rows.Count > 0)
					{
						List<Definition> listDefs = Definitions.GetDefsForCategory(DefinitionCategory.PaymentTypes, true);
						for (int i = 0; i < table.Rows.Count; i++)
						{
                            //There's only one place in the program where this is called from.  Date is today, so no need to validate the date.
                            Payment payment = new Payment
                            {
                                PayType = listDefs[0].Id,
                                DateEntry = PIn.Date(table.Rows[i]["DateEntry"].ToString()),
                                PatNum = PIn.Long(table.Rows[i]["PatNum"].ToString()),
                                PayDate = PIn.Date(table.Rows[i]["DatePay"].ToString()),
                                PayAmt = PIn.Double(table.Rows[i]["SplitAmt_"].ToString()),
                                PayNote = "Dummy payment. Original payment entry missing from the database.",
                                PayNum = PIn.Long(table.Rows[i]["PayNum"].ToString()),
                                //Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
                                SecUserNumEntry = Security.CurrentUser.Id,
                                PaymentSource = CreditCardSource.None,
                                ProcessStatus = ProcessStat.OfficeProcessed
                            };
                            Payments.Insert(payment, true);
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, payment.PayNum, DbmLogFKeyType.Payment,
								DbmLogActionType.Insert, methodName, "Inserted payment from PaySplitWithInvalidPayNum."));
						}
						rowsFixed += table.Rows.Count;
					}
					//Handling paysplits that have a pay num of 0 separately because we want to create one payment per patient per day
					command = @"SELECT *,SUM(SplitAmt) SplitAmt_ 
						FROM paysplit WHERE "
						+ patWhere
						+ "NOT EXISTS(SELECT * FROM payment WHERE paysplit.PayNum=payment.PayNum) "
						+ "AND PayNum=0 "
						+ "GROUP BY PatNum,DatePay";
					table = Database.ExecuteDataTable(command);
					string where = "";
					if (table.Rows.Count > 0)
					{
						List<Definition> listDefs = Definitions.GetDefsForCategory(DefinitionCategory.PaymentTypes, true);
						for (int i = 0; i < table.Rows.Count; i++)
						{
                            Payment payment = new Payment
                            {
                                PayType = listDefs[0].Id,
                                DateEntry = PIn.Date(table.Rows[i]["DateEntry"].ToString()),
                                PatNum = PIn.Long(table.Rows[i]["PatNum"].ToString()),
                                PayDate = PIn.Date(table.Rows[i]["DatePay"].ToString()),
                                PayAmt = PIn.Double(table.Rows[i]["SplitAmt_"].ToString()),
                                PayNote = "Dummy payment. Original payment entry number was 0.",
                                PayNum = PIn.Long(table.Rows[i]["PayNum"].ToString()),
                                //Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
                                SecUserNumEntry = Security.CurrentUser.Id,
                                PaymentSource = CreditCardSource.None,
                                ProcessStatus = ProcessStat.OfficeProcessed
                            };

                            Payments.Insert(payment);
							listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, payment.PayNum, DbmLogFKeyType.Payment,
								DbmLogActionType.Insert, methodName, "Inserted payment from PaySplitWithInvalidPayNum."));
							where = " WHERE PayNum=0 AND PatNum=" + POut.Long(payment.PatNum) + " AND DatePay=" + POut.Date(payment.PayDate);
							command = "SELECT SplitNum FROM paysplit" + where;
							List<long> listPaySplitNums = Database.GetListLong(command);
							command = "UPDATE paysplit SET PayNum=" + POut.Long(payment.PayNum) + where;
							Database.ExecuteNonQuery(command);
							listPaySplitNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Payment,
								DbmLogActionType.Update, methodName, "Updated PayNum from 0 to " + payment.PayNum + ".")));
						}
						rowsFixed += table.Rows.Count;
					}
					if (rowsFixed > 0 || verbose)
					{
						log += "Paysplits found with invalid PayNum fixed: " + rowsFixed.ToString() + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PaySplitWithInvalidPayPlanNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM paysplit WHERE paysplit.PayPlanNum!=0 AND paysplit.PayPlanNum NOT IN(SELECT payplan.PayPlanNum FROM payplan)";
					int numFound = Database.ExecuteInt(command);
					if (numFound > 0 || verbose)
					{
						log += "Paysplits found with invalid PayPlanNum: " + numFound + "\r\n";
					}
					break;

				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					string where = "WHERE paysplit.PayPlanNum!=0 AND paysplit.PayPlanNum NOT IN(SELECT payplan.PayPlanNum FROM payplan)";
					command = "SELECT SplitNum FROM paysplit " + where;
					List<long> listPaySplitNums = Database.GetListLong(command);
					command = "UPDATE paysplit SET paysplit.PayPlanNum=0 " + where;
					long numFixed = Database.ExecuteNonQuery(command);
					listPaySplitNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.PaySplit,
						DbmLogActionType.Update, methodName, "Updated PayPlanNum to 0 from PaySplitWithInvalidPayPlanNum.")));
					if (numFixed > 0 || verbose)
					{
						log += "Paysplits with invalid PayPlanNums fixed: " + numFixed + "\r\n";
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PaySplitWithInvalidPrePayNum(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT ps1.* FROM paysplit ps1 LEFT JOIN paysplit ps2 ON ps1.FSplitNum=ps2.SplitNum WHERE ps1.FSplitNum!=0 AND ps2.SplitNum IS NULL";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "Paysplits attached to deleted prepayments: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   Manual fix needed.  Double click to see a break down.\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", including:\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "   PatNum: #" + table.Rows[i]["PatNum"].ToString();
							log += " " + PIn.Date(table.Rows[i]["DatePay"].ToString()).ToShortDateString();
							log += " " + PIn.Double(table.Rows[i]["SplitAmt"].ToString()).ToString("c");
							log += "\r\n";
						}
						log += "   They need to be fixed manually.\r\n";
					}
					break;
			}
			return log;
		}

		#endregion PayPlanCharge, PayPlan, PaySplit-----------------------------------------------------------------------------------------------------
		#region PerioMeasure, PlannedAppt, Preference---------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string PerioMeasureWithInvalidIntTooth(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = @"SELECT COUNT(*) FROM periomeasure WHERE IntTooth > 32 OR IntTooth < 1";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "PerioMeasures found with invalid tooth number: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = @"DELETE FROM periomeasure WHERE IntTooth > 32 OR IntTooth < 1";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "PerioMeasures deleted due to invalid tooth number: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PlannedApptsWithInvalidAptNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = @"SELECT COUNT(*) FROM plannedappt WHERE AptNum=0";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Planned appointments found with invalid AptNum" + ": " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "SELECT PlannedApptNum FROM plannedappt WHERE AptNum=0";
					List<long> listPlannedAppts = Database.GetListLong(command);
					command = @"DELETE FROM plannedappt WHERE AptNum=0";
					long numberFixed = Database.ExecuteNonQuery(command);
					listPlannedAppts.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.PlannedAppt,
						DbmLogActionType.Delete, methodName, "Deleted plannedappt from PlannedApptsWithInvalidAptNum.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Planned appointments deleted due to invalid AptNum" + ": " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PreferenceAllergiesIndicateNone(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM allergydef where AllergyDefNum=" + POut.Long(Preferences.GetLong(PreferenceName.AllergiesIndicateNone));
					if (PIn.Int(Database.ExecuteString(command)) == 0 && Preferences.GetString(PreferenceName.AllergiesIndicateNone) != "")
					{
						log += "Preference \"AllergyIndicatesNone\" is an invalid value." + "\r\n";
					}
					else if (verbose)
					{
						log += "Preference \"AllergyIndicatesNone\" checked." + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "SELECT COUNT(*) FROM allergydef where AllergyDefNum=" + POut.Long(Preferences.GetLong(PreferenceName.AllergiesIndicateNone));
					if (PIn.Int(Database.ExecuteString(command)) == 0 && Preferences.GetString(PreferenceName.AllergiesIndicateNone) != "")
					{
						Preferences.Set(PreferenceName.AllergiesIndicateNone, "");
						Signalods.SetInvalid(InvalidType.Prefs);
						log += "Preference \"AllergyIndicatesNone\" set to blank due to an invalid value." + "\r\n";
					}
					else if (verbose)
					{
						log += "Preference \"AllergyIndicatesNone\" checked." + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PreferenceDateDepositsStarted(bool verbose, DbmMode modeCur)
		{
			DateTime date = PrefC.GetDate(PreferenceName.DateDepositsStarted);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (date < DateTime.Now.AddMonths(-1))
					{
						log += "Deposit start date needs to be reset.\r\n";
					}
					else if (verbose)
					{
						log += "Deposit start date checked.\r\n";
					}
					break;
				case DbmMode.Fix:
					//If the program locks up when trying to create a deposit slip, it's because someone removed the start date from the deposit edit window. Run this query to get back in.
					if (date < DateTime.Now.AddMonths(-1))
					{

						Preferences.Set(PreferenceName.DateDepositsStarted, DateTime.UtcNow.Date.AddDays(-21));

						Signalods.SetInvalid(InvalidType.Prefs);
						log += "Deposit start date reset.\r\n";
					}
					else if (verbose)
					{
						log += "Deposit start date checked.\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PreferenceInsBillingProv(bool verbose, DbmMode modeCur)
		{
			if (modeCur == DbmMode.Breakdown)
			{
				return "";
			}
			string log = "";
			long insBillingProvNum = Preferences.GetLong(PreferenceName.InsBillingProv);
			Provider prov = Providers.GetById(insBillingProvNum);
			if (insBillingProvNum == 0 || prov != null)
			{//0 means the program will use the default practice provider.
				if (verbose)
				{
					log += "Default insurance billing provider verified." + "\r\n";
				}
			}
			else
			{
				log += "Invalid default insurance billing provider set." + "\r\n";
				if (modeCur != DbmMode.Check)
				{
					Preferences.Set(PreferenceName.InsBillingProv, 0);//Set it to zero so it can default to the practice provider.
					log += "  " + "Fixed." + "\r\n";
				}
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PreferenceMedicationsIndicateNone(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM medication where MedicationNum=" + POut.Long(Preferences.GetLong(PreferenceName.MedicationsIndicateNone));
					if (PIn.Int(Database.ExecuteString(command)) == 0 && Preferences.GetString(PreferenceName.MedicationsIndicateNone) != "")
					{
						log += "Preference \"MedicationsIndicateNone\" is an invalid value." + "\r\n";
					}
					else if (verbose)
					{
						log += "Preference \"MedicationsIndicateNone\" checked." + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "SELECT COUNT(*) FROM medication where MedicationNum=" + POut.Long(Preferences.GetLong(PreferenceName.MedicationsIndicateNone));
					if (PIn.Int(Database.ExecuteString(command)) == 0 && Preferences.GetString(PreferenceName.MedicationsIndicateNone) != "")
					{
						Preferences.Set(PreferenceName.MedicationsIndicateNone, "");
						Signalods.SetInvalid(InvalidType.Prefs);
						log += "Preference \"MedicationsIndicateNone\" set to blank due to an invalid value." + "\r\n";
					}
					else if (verbose)
					{
						log += "Preference \"MedicationsIndicateNone\" checked." + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PreferenceProblemsIndicateNone(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM diseasedef where DiseaseDefNum=" + POut.Long(Preferences.GetLong(PreferenceName.ProblemsIndicateNone));
					if (PIn.Int(Database.ExecuteString(command)) == 0 && Preferences.GetString(PreferenceName.ProblemsIndicateNone) != "")
					{
						log += "Preference \"ProblemsIndicateNone\" is an invalid value.\r\n";
					}
					else if (verbose)
					{
						log += "Preference \"ProblemsIndicateNone\" checked.\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "SELECT COUNT(*) FROM diseasedef where DiseaseDefNum=" + POut.Long(Preferences.GetLong(PreferenceName.ProblemsIndicateNone));
					if (PIn.Int(Database.ExecuteString(command)) == 0 && Preferences.GetString(PreferenceName.ProblemsIndicateNone) != "")
					{
						Preferences.Set(PreferenceName.ProblemsIndicateNone, "");
						Signalods.SetInvalid(InvalidType.Prefs);
						log += "Preference \"ProblemsIndicateNone\" set to blank due to an invalid value.\r\n";
					}
					else if (verbose)
					{
						log += "Preference \"ProblemsIndicateNone\" checked.\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PreferenceTimeCardOvertimeFirstDayOfWeek(bool verbose, DbmMode modeCur)
		{
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (PrefC.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek) < 0 || PrefC.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek) > 6)
					{
						log += "Preference \"TimeCardOvertimeFirstDayOfWeek\" is an invalid value." + "\r\n";
					}
					else if (verbose)
					{
						log += "Preference \"TimeCardOvertimeFirstDayOfWeek\" checked." + "\r\n";
					}
					break;
				case DbmMode.Fix:
					if (PrefC.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek) < 0 || PrefC.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek) > 6)
					{
						Preferences.Set(PreferenceName.TimeCardOvertimeFirstDayOfWeek, 0);//0==Sunday
						Signalods.SetInvalid(InvalidType.Prefs);
						log += "Preference \"TimeCardOvertimeFirstDayOfWeek\" set to Sunday due to an invalid value." + "\r\n";
					}
					else if (verbose)
					{
						log += "Preference \"TimeCardOvertimeFirstDayOfWeek\" checked." + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PreferencePracticeProv(bool verbose, DbmMode modeCur)
		{
			if (modeCur == DbmMode.Breakdown) return "";

			string log = "";

			var defaultProv = Preferences.GetLong(PreferenceName.PracticeDefaultProv);

			if (defaultProv > 0)
			{
				if (verbose)
				{
					log += "Default practice provider verified.\r\n";
				}
			}
			else
			{
				log += "No default provider set.\r\n";
				if (modeCur != DbmMode.Check)
				{
					defaultProv = Database.ExecuteLong("SELECT provnum FROM provider WHERE IsHidden=0 ORDER BY itemorder LIMIT 1");

					Preferences.Set(PreferenceName.PracticeDefaultProv, defaultProv);

					log += "  Fixed.\r\n";
				}
			}

			return log;
		}

		#endregion PerioMeasure, PlannedAppt, Preference------------------------------------------------------------------------------------------------
		#region ProcButton, ProcedureCode---------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string ProcButtonItemsDeleteWithInvalidAutoCode(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = @"SELECT COUNT(*) FROM procbuttonitem WHERE CodeNum=0 AND NOT EXISTS(
						SELECT * FROM autocode WHERE autocode.AutoCodeNum=procbuttonitem.AutoCodeNum)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "ProcButtonItems found with invalid autocode: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = @"DELETE FROM procbuttonitem WHERE CodeNum=0 AND NOT EXISTS(
						SELECT * FROM autocode WHERE autocode.AutoCodeNum=procbuttonitem.AutoCodeNum)";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0)
					{
						Signalods.SetInvalid(InvalidType.ProcButtons);
					}
					if (numberFixed > 0 || verbose)
					{
						log += "ProcButtonItems deleted due to invalid autocode: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurecodeCategoryNotSet(bool verbose, DbmMode modeCur)
		{
			List<Definition> listProcCodeCats = Definitions.GetDefsForCategory(DefinitionCategory.ProcCodeCats, true);
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM procedurecode WHERE procedurecode.ProcCat=0";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						if (listProcCodeCats.Count == 0)
						{
							log += "Procedure codes with no categories found but cannot be fixed because there are no visible proc code categories.\r\n";
							return log;
						}
						log += "Procedure codes with no category found: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					if (listProcCodeCats.Count == 0)
					{
						log += "Procedure codes with no categories cannot be fixed because there are no visible proc code categories.\r\n";
						return log;
					}
					command = "UPDATE procedurecode SET procedurecode.ProcCat=" + POut.Long(listProcCodeCats[0].Id) + " WHERE procedurecode.ProcCat=0";
					long numberfixed = Database.ExecuteNonQuery(command);
					if (numberfixed > 0)
					{
						Signalods.SetInvalid(InvalidType.ProcCodes);
					}
					if (numberfixed > 0 || verbose)
					{
						log += "Procedure codes with no category fixed: " + numberfixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Some customers have duplicate procedure codes existing within their database due to past conversions that allowed these 
		/// duplicate codes to exist.  Each proc code does have a different primary key, however in some cases the same procedure code is used and listed
		/// multiple times causing errors/UE issues.  This method will correct those duplicates by identifying them and if found, will add
		/// a hiphen to the duplicate proc code with a numerical count for each duplicate. Example U0001, U0001-1, U0001-2, etc.///</summary>
		[DbmMethodAttr]
		public static string ProcedurecodeFixDuplicateProcedureCodes(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = @"SELECT ProcCode FROM procedurecode GROUP BY ProcCode HAVING COUNT(*) > 1";
			List<string> listProcCodes = Database.GetListString(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listProcCodes.Count;
					if (numFound > 0 || verbose)
					{
						log += "Procedures found using the same Proc Code: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					if (listProcCodes.Count > 0)
					{
						List<DbmLog> listDbmLogs = new List<DbmLog>();
						string methodName = MethodBase.GetCurrentMethod().Name;
						int numFixed = 0;
						if (listProcCodes.Count > 0)
						{
							//Get the PK and ProcCode for each duplicate so that we can make the ProcCodes unique.
							command = @"SELECT CodeNum,ProcCode FROM procedurecode WHERE ProcCode IN ('" + String.Join("','", listProcCodes) + "') ORDER BY CodeNum";
							DataTable table = Database.ExecuteDataTable(command);
							//Group up the duplicates by ProcCode.
							Dictionary<string, List<DataRow>> dictDupeProcCodes = table.Select().GroupBy(x => x["ProcCode"].ToString()).ToDictionary(x => x.Key, x => x.ToList());
							//Go through each ProcCode group and make the ProcCode string unique for each entry (except the first one).
							foreach (KeyValuePair<string, List<DataRow>> kvp in dictDupeProcCodes)
							{
								for (int i = 0; i < kvp.Value.Count; i++)
								{
									if (i == 0)
									{
										continue;//Arbitrarilly leave the first proc code alone.
									}
									long codeNum = PIn.Long(kvp.Value[i]["CodeNum"].ToString());
									string procCodeOriginal = kvp.Key;
									string procCodeFix = kvp.Key + "-" + i;
									command = @"UPDATE procedurecode SET ProcCode='" + POut.String(procCodeFix) + "' WHERE CodeNum=" + POut.Long(codeNum);
									Database.ExecuteNonQuery(command);
									listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, codeNum, DbmLogFKeyType.ProcedureCode,
										DbmLogActionType.Update, methodName, $"Duplicate procedure code found, ProcCode Changed from '"
											+ POut.String(procCodeOriginal) + "' to '" + POut.String(procCodeFix) + "'"));
									numFixed++;
								}
							}
						}
						if (numFixed > 0 || verbose)
						{
							Crud.DbmLogCrud.InsertMany(listDbmLogs);
							log += "Procedures fixed that had a duplicate Proc Code: " + numFixed.ToString() + "\r\n";
						}
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurecodeInvalidProvNum(bool verbose, DbmMode modeCur)
		{
			string command = @"SELECT procedurecode.CodeNum FROM procedurecode 
				LEFT JOIN provider ON procedurecode.ProvNumDefault=provider.ProvNum 
				WHERE provider.ProvNum IS NULL 
				AND procedurecode.ProvNumDefault!=0";
			List<long> listProcNums = Database.GetListLong(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (listProcNums.Count > 0 || verbose)
					{
						log += "Procedure codes with invalid Default Provider found: " + listProcNums.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					if (listProcNums.Count > 0)
					{
						command = "UPDATE procedurecode SET procedurecode.ProvNumDefault=0 WHERE procedurecode.CodeNum IN (" + String.Join(",", listProcNums) + ")";
						Database.ExecuteNonQuery(command);
					}
					if (listProcNums.Count > 0 || verbose)
					{
						log += "Procedure codes with invalid Default Provider fixed: " + listProcNums.Count + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion ProcButton, ProcedureCode------------------------------------------------------------------------------------------------------------
		#region ProcedureLog----------------------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string ProcedurelogAttachedToApptWithProcStatusDeleted(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT ProcNum FROM procedurelog "
						+ "WHERE ProcStatus=6 AND (AptNum!=0 OR PlannedAptNum!=0)";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = Database.GetListLong(command).Count;
					if (numFound > 0 || verbose)
					{
						log += "Deleted procedures still attached to appointments: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					List<long> listProcNums = Database.GetListLong(command);
					command = "UPDATE procedurelog SET AptNum=0,PlannedAptNum=0 "
						+ "WHERE ProcStatus=6 "
						+ "AND (AptNum!=0 OR PlannedAptNum!=0)";
					long numberFixed = Database.ExecuteNonQuery(command);
					listProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Procedure,
						 DbmLogActionType.Update, methodName, "Appointment detached.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Deleted procedures detached from appointments: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogAttachedToWrongAppts(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT procedurelog.ProcNum FROM appointment,procedurelog "
							+ "WHERE procedurelog.AptNum=appointment.AptNum AND procedurelog.PatNum != appointment.PatNum";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = Database.GetListLong(command).Count;
					if (numFound > 0 || verbose)
					{
						log += "Procedures attached to appointments with incorrect patient: " + numFound + "\r\n";
					}
					break;

				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					List<long> listProcNums = Database.GetListLong(command);
					command = "UPDATE appointment,procedurelog SET procedurelog.AptNum=0 "
						+ "WHERE procedurelog.AptNum=appointment.AptNum AND procedurelog.PatNum != appointment.PatNum";
					long numberFixed = Database.ExecuteNonQuery(command);
					listProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Procedure,
						 DbmLogActionType.Update, methodName, "Appointment detached from ProcedurelogAttachedToWrongAppts.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Procedures detached from appointments: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogAttachedToWrongApptDate(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = @"SELECT procedurelog.ProcNum FROM procedurelog,appointment
							WHERE procedurelog.AptNum=appointment.AptNum
							AND DATE(procedurelog.ProcDate) != DATE(appointment.AptDateTime)
							AND procedurelog.ProcStatus=2";//only detach completed procs 
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = Database.GetListLong(command).Count;
					if (numFound > 0 || verbose)
					{
						log += "Procedures which are attached to appointments with mismatched dates: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					List<long> listProcNums = Database.GetListLong(command);
					command = @"UPDATE procedurelog,appointment
						SET procedurelog.AptNum=0
						WHERE procedurelog.AptNum=appointment.AptNum
						AND DATE(procedurelog.ProcDate) != DATE(appointment.AptDateTime)
						AND procedurelog.ProcStatus=2";//only detach completed procs 
					long numberFixed = Database.ExecuteNonQuery(command);
					listProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Procedure,
						 DbmLogActionType.Update, methodName, "Appointment detached from ProcedurelogAttachedToWrongApptDate.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Procedures detached from appointments due to mismatched dates: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogBaseUnitsZero(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					//zero--------------------------------------------------------------------------------------
					command = @"SELECT COUNT(*) FROM procedurelog 
						WHERE baseunits != (SELECT procedurecode.BaseUnits FROM procedurecode WHERE procedurecode.CodeNum=procedurelog.CodeNum)
						AND baseunits=0";
					//we do not want to change this automatically.  Do not fix these!
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Procedure BaseUnits are zero and are not matching procedurecode BaseUnits: " + numFound + "\r\n";
					}
					//not zero----------------------------------------------------------------------------------
					command = @"SELECT COUNT(*)
						FROM procedurelog
						WHERE BaseUnits!=0
						AND (SELECT procedurecode.BaseUnits FROM procedurecode WHERE procedurecode.CodeNum=procedurelog.CodeNum)=0";
					//very safe to change them back to zero.
					numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Procedure BaseUnits not zero, but procedurecode BaseUnits are zero: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = @"SELECT ProcNum FROM procedurelog
						WHERE BaseUnits!=0 
						AND (SELECT procedurecode.BaseUnits FROM procedurecode WHERE procedurecode.CodeNum=procedurelog.CodeNum)=0";
					List<long> listProcNums = Database.GetListLong(command);
					//first situation: don't fix.
					//second situation:
					//Writing the query this way allows it to work with Oracle.
					command = @"UPDATE procedurelog
						SET BaseUnits=0
						WHERE BaseUnits!=0 
						AND (SELECT procedurecode.BaseUnits FROM procedurecode WHERE procedurecode.CodeNum=procedurelog.CodeNum)=0";
					long numberFixed = Database.ExecuteNonQuery(command);
					listProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Procedure,
						 DbmLogActionType.Update, methodName, "Procedure BaseUnit set to zero from ProcedurelogBaseUnitsZero.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Procedure BaseUnits set to zero because procedurecode BaseUnits are zero: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogCodeNumInvalid(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = @"SELECT ProcNum FROM procedurelog WHERE NOT EXISTS(SELECT * FROM procedurecode WHERE procedurecode.CodeNum=procedurelog.CodeNum)";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = Database.GetListLong(command).Count;
					if (numFound > 0 || verbose)
					{
						log += "Procedures found with invalid CodeNum: " + numFound + "\r\n";
					}
					break;

				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					List<long> listProcNums = Database.GetListLong(command);
					long badCodeNum = 0;
					if (!ProcedureCodes.IsValidCode("~BAD~"))
					{
                        ProcedureCode badCode = new ProcedureCode
                        {
                            Code = "~BAD~",
                            Description = "Invalid procedure",
                            ShortDescription = "Invalid procedure",
                            ProcedureCategory = Definitions.GetByExactNameNeverZero(DefinitionCategory.ProcCodeCats, "Never Used")
                        };
                        ProcedureCodes.Insert(badCode);
						badCodeNum = badCode.Id;
					}
					else
					{
						badCodeNum = ProcedureCodes.GetCodeNum("~BAD~");
					}
					command = "UPDATE procedurelog SET CodeNum=" + POut.Long(badCodeNum) + " WHERE NOT EXISTS (SELECT * FROM procedurecode WHERE procedurecode.CodeNum=procedurelog.CodeNum)";
					long numberFixed = Database.ExecuteNonQuery(command);
					listProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Procedure,
						 DbmLogActionType.Update, methodName, "Procedure fixed with invalid CodeNum from ProcedurelogCodeNumInvalid.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Procedures fixed with invalid CodeNum: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>There was a bug introduced that created an invalid tooth range when the user selected teeth from both mandibular and maxillary teeth.
		///The bug was fixed in v19.2.23 with job #16655 but databases were left in an invalid state.  This method will correct almost all situations.
		///In addition, we identified an issue where old procedures would store toothrange information as abbrieviations such as LA (Lower Arch),
		///UA (Upper Arch), and FM (Full Mouth) these will also be addressed by this fix.  We can correct this issue because we always store tooth ranges 
		///in US nomenclature (predictable numbers).</summary>
		[DbmMethodAttr(HasPatNum = true)]
		public static string ProcedurelogFixInvalidToothranges(bool verbose, DbmMode modeCur, long patNum = 0)
		{
			string patWhere = (patNum > 0 ? " AND PatNum=" + POut.Long(patNum) : "");
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM procedurelog WHERE ToothRange REGEXP '[A-Z]{2}|[0-9]{3}'" + patWhere;//2 letters or 3 numbers
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Procedures found with invalid ToothRange: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "SELECT PatNum,ProcNum,ToothRange FROM procedurelog WHERE ToothRange REGEXP '[A-Z]{2}|[0-9]{3}'" + patWhere;
					DataTable table = Database.ExecuteDataTable(command);
					int numFixed = 0;
					if (table.Rows.Count > 0)
					{
						foreach (DataRow row in table.Rows)
						{
							long procNum = PIn.Long(row["ProcNum"].ToString());
							string toothRange = PIn.String(row["ToothRange"].ToString());
							string[] arrayTeeth = toothRange.Split(',');
							string toothRangeUpdate = "";
							#region Separate mandibular and maxillary
							for (int i = 0; i < arrayTeeth.Length; i++)
							{
								if (arrayTeeth[i].Length == 4)
								{
									toothRangeUpdate += arrayTeeth[i].Substring(0, 2) + "," + arrayTeeth[i].Substring(2, 2) + ",";
								}
								else if (arrayTeeth[i].Length == 3)
								{
									toothRangeUpdate += arrayTeeth[i].Substring(0, 1) + "," + arrayTeeth[i].Substring(1, 2) + ",";//because numbers are going lower to higher
								}
								else if (arrayTeeth[i].Length == 2 && Char.IsLetter(arrayTeeth[i], 0) && Char.IsLetter(arrayTeeth[i], 1))
								{
									toothRangeUpdate += arrayTeeth[i].Substring(0, 1) + "," + arrayTeeth[i].Substring(1, 1) + ",";
								}
								else
								{
									toothRangeUpdate += arrayTeeth[i] + ",";
								}
							}
							toothRangeUpdate = toothRangeUpdate.TrimEnd(',');
							#endregion
							#region Known Invalid ToothRange Values
							//The following values were found in some live databases.  These are the correct way to translate the values into valid ToothRanges.
							switch (toothRange.ToUpper())
							{
								case "FM": //Full Mouth stored as FM and should be updated to the correct toothrange
									toothRangeUpdate = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32";
									break;
								case "LA"://Lower Arch
									toothRangeUpdate = "17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32";
									break;
								case "UA"://Upper Arch
									toothRangeUpdate = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16";
									break;
								case "UR"://Upper Right
									toothRangeUpdate = "1,2,3,4,5,6,7,8";
									break;
								case "UL"://Upper Left
									toothRangeUpdate = "9,10,11,12,13,14,15,16";
									break;
								case "LL"://Lower Left
									toothRangeUpdate = "17,18,19,20,21,22,23,24";
									break;
								case "LR"://Lower Left
									toothRangeUpdate = "25,26,27,28,29,30,31,32";
									break;
								default:
									//Do nothing.
									break;
							}
							#endregion
							if (toothRangeUpdate != toothRange)
							{
								command = $"UPDATE procedurelog SET ToothRange='{POut.String(toothRangeUpdate)}' "
									+ "WHERE ProcNum=" + POut.Long(procNum) + patWhere;
								Database.ExecuteNonQuery(command);
								numFixed++;
								listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, procNum, DbmLogFKeyType.Procedure,
									DbmLogActionType.Update, methodName, $"Invalid ToothRange of '{POut.String(toothRange)}' changed to '{POut.String(toothRangeUpdate)}'"));
							}
						}
					}
					if (numFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Procedures fixed with invalid ToothRange: " + numFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true, IsCanada = true)]
		public static string ProcedurelogLabAttachedToDeletedProc(bool verbose, DbmMode modeCur)
		{
			if (!CultureInfo.CurrentCulture.Name.EndsWith("CA"))
			{//Canadian. en-CA or fr-CA
				return "Skipped. Local computer region must be set to Canada to run.";
			}

			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM procedurelog "
						+ "WHERE ProcStatus=2 AND ProcNumLab IN(SELECT ProcNum FROM procedurelog WHERE ProcStatus=6)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Completed procedure labs attached to deleted procedures: " + numFound;
						log += "\r\n   Double click to see a break down.\r\n";
					}
					break;
				case DbmMode.Breakdown:  //No db changes made, see if statement below.
				case DbmMode.Fix:
					command = "SELECT patient.PatNum,patient.FName,patient.LName,procedurelog.ProcNum FROM procedurelog "
						+ "LEFT JOIN patient ON procedurelog.PatNum=patient.PatNum "
						+ "WHERE ProcStatus=" + POut.Int((int)ProcStat.C) + " "
						+ "AND ProcNumLab IN(SELECT ProcNum FROM procedurelog WHERE ProcStatus=" + POut.Int((int)ProcStat.D) + ") "
						+ "GROUP BY patient.PatNum ";
					DataTable table = Database.ExecuteDataTable(command);
					if (table.Rows.Count > 0 || verbose)
					{
						if (modeCur == DbmMode.Fix)
						{
							command = "UPDATE procedurelog plab,procedurelog p "
								+ "SET plab.ProcNumLab=0 "
								+ "WHERE plab.ProcStatus=" + POut.Int((int)ProcStat.C) + " AND plab.ProcNumLab=p.ProcNum AND p.ProcStatus=" + POut.Int((int)ProcStat.D);
							Database.ExecuteNonQuery(command);
							List<DbmLog> listDbmLogs = new List<DbmLog>();
							string methodName = MethodBase.GetCurrentMethod().Name;
							table.Select().ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, PIn.Long(x["ProcNum"].ToString()),
								DbmLogFKeyType.Procedure, DbmLogActionType.Update, methodName, "Lab procedure detached from ProcedurelogLabAttachedToDeletedProc.")));
							Crud.DbmLogCrud.InsertMany(listDbmLogs);

							log += "Patients with completed lab procedures detached from deleted procedures: " + table.Rows.Count;
						}
						if (modeCur == DbmMode.Breakdown)
						{
							log += 
								"Patients with completed lab procedures that will be detached from deleted procedures: " + table.Rows.Count + 
								", including:\r\n";

							log += string.Join("\r\n", table.Select().Select(x => "#" + x["PatNum"].ToString() + ":" + x["FName"].ToString() + " " + x["LName"].ToString()));
						}
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ProcedurelogMultipleClaimProcForInsSub(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT patient.PatNum,patient.LName,patient.FName,procedurelog.ProcDate,procedurecode.ProcCode "
				+ "FROM claimproc "
				+ "INNER JOIN procedurelog ON procedurelog.ProcNum=claimproc.ProcNum "
				+ "INNER JOIN procedurecode ON procedurecode.CodeNum=procedurelog.CodeNum "
				+ "INNER JOIN patient ON patient.PatNum=claimproc.PatNum "
				+ "WHERE (claimproc.Status=" + POut.Int((int)ClaimProcStatus.NotReceived) + " "
				+ "OR claimproc.Status=" + POut.Int((int)ClaimProcStatus.Received) + " "
				+ "OR claimproc.Status=" + POut.Int((int)ClaimProcStatus.Estimate) + ") "
				+ "AND procedurelog.ProcStatus!=" + POut.Int((int)ProcStat.D) + " " //exclude deleted procedures
				+ "GROUP BY claimproc.ProcNum, claimproc.InsSubNum, claimproc.PlanNum "
					+ ", patient.PatNum, patient.LName, patient.FName, procedurelog.ProcDate, procedurecode.ProcCode "//For Oracle.
				+ "HAVING COUNT(*)>1 "
				+ "ORDER BY patient.LName, patient.FName, procedurelog.ProcDate, procedurecode.ProcCode";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}

			string log = "Procedures with multiple claimprocs for the same insurance plan: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   Manual fix needed.  Double click to see a break down.\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", including:\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "   " + table.Rows[i]["PatNum"].ToString() + "-" + table.Rows[i]["LName"].ToString() + ", " + table.Rows[i]["FName"].ToString()
								+ "  Procedure Date: " + PIn.Date(table.Rows[i]["ProcDate"].ToString()).ToShortDateString() + "  " + table.Rows[i]["ProcCode"];
							log += "\r\n";
						}
						log += "   They need to be fixed manually.\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogProvNumMissing(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT COUNT(*) FROM procedurelog WHERE ProvNum=0";
			int numFound = PIn.Int(Database.ExecuteString(command));
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (numFound > 0 || verbose)
					{
						log += "Procedures with missing provnums found: " + numFound + "\r\n";
						log += "   A safe fix is under development." + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogToothNums(bool verbose, DbmMode modeCur)
		{
			if (modeCur == DbmMode.Breakdown)
			{
				return "";
			}
			string log = "";
			//The logic for checking whether a tooth is invalid was obtained from Tooth.IsValidDB().
			string command = "SELECT ProcNum,ToothNum,PatNum FROM procedurelog "
				+ "WHERE ToothNum!='' "
				+ "AND ToothNum NOT REGEXP '^[A-T]S?$' "//supernumerary
				+ "AND (ToothNum NOT REGEXP '^[1-9][0-9]?$' "//matches 1 or 2 digits, leading 0 not allowed
				+ "OR (CAST(ToothNum AS UNSIGNED)>32 AND CAST(ToothNum AS UNSIGNED)<51) "
				+ "OR CAST(ToothNum AS UNSIGNED)>82) ";
			DataTable table = Database.ExecuteDataTable(command);
			Patient Lim = null;
			string toothNum;
			int numberFixed = 0;
			List<long> listProcNums = new List<long>();
			List<DbmLog> listDbmLogs = new List<DbmLog>();
			string methodName = MethodBase.GetCurrentMethod().Name;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				toothNum = table.Rows[i][1].ToString();
				if (verbose)
				{
					Lim = Patients.GetLim(Convert.ToInt32(table.Rows[i][2].ToString()));
				}
				if (string.CompareOrdinal(toothNum, "a") >= 0 && string.CompareOrdinal(toothNum, "t") <= 0)
				{
					if (modeCur != DbmMode.Check)
					{
						command = "UPDATE procedurelog SET ToothNum='" + toothNum.ToUpper() + "' WHERE ProcNum=" + table.Rows[i][0].ToString();
						Database.ExecuteNonQuery(command);
					}
					if (verbose)
					{
						log += Lim.GetNameLF() + " " + toothNum + " - " + toothNum.ToUpper() + "\r\n";
					}
					numberFixed++;
				}
				else
				{
					if (modeCur != DbmMode.Check)
					{
						command = "UPDATE procedurelog SET ToothNum='1' WHERE ProcNum=" + table.Rows[i][0].ToString();
						Database.ExecuteNonQuery(command);
					}
					if (verbose)
					{
						log += Lim.GetNameLF() + " " + toothNum + " - 1\r\n";
					}
					numberFixed++;
				}
			}
			table.Select().ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, PIn.Long(x["ProcNum"].ToString()), DbmLogFKeyType.Procedure,
					 DbmLogActionType.Update, methodName, "Fixed invalid tooth number from ProcedurelogToothNums.")));
			if (numberFixed != 0 || verbose)
			{
				Crud.DbmLogCrud.InsertMany(listDbmLogs);
				log += 
					"Check for invalid tooth numbers complete. Records checked: " + Database.ExecuteString("SELECT COUNT(*) FROM procedurelog") + ". " +
					"Records invalid: " + numberFixed.ToString() + "\r\n";
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ProcedurelogTpAttachedToClaim(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT procedurelog.ProcNum,claim.ClaimNum,claim.DateService,patient.PatNum,patient.LName,patient.FName,procedurecode.ProcCode "
				+ "FROM procedurelog,claim,claimproc,patient,procedurecode "
				+ "WHERE procedurelog.ProcNum=claimproc.ProcNum "
				+ "AND claim.ClaimNum=claimproc.ClaimNum "
				+ "AND claim.PatNum=patient.PatNum "
				+ "AND procedurelog.CodeNum=procedurecode.CodeNum "
				+ "AND procedurelog.ProcStatus!=" + POut.Long((int)ProcStat.C) + " "//procedure not complete
				+ "AND (claim.ClaimStatus='W' OR claim.ClaimStatus='S' OR claim.ClaimStatus='R') "//waiting, sent, or received
				+ "AND (claim.ClaimType='P' OR claim.ClaimType='S' OR claim.ClaimType='Other')";//pri, sec, or other.  Eliminates preauths.
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "Procedures attached to claims with status of TP: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   " + "Manual fix needed.  Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", including:\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "Patient"
									+ " " + table.Rows[i]["FName"].ToString()
									+ " " + table.Rows[i]["LName"].ToString()
									+ " #" + table.Rows[i]["PatNum"].ToString()
									+ ", for claim service date " + PIn.Date(table.Rows[i]["DateService"].ToString()).ToShortDateString()
									+ ", procedure code " + table.Rows[i]["ProcCode"].ToString() + "\r\n";
						}
						log += "   They need to be fixed manually.\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true, IsCanada = true)]
		public static string ProcedurelogNotComplAttachedToComplLabCanada(bool verbose, DbmMode modeCur)
		{
			if (!CultureInfo.CurrentCulture.Name.EndsWith("CA"))
			{//Canadian. en-CA or fr-CA
				return "Skipped. Local computer region must be set to Canada to run.";
			}
			string command = "SELECT pc.ProcCode ProcCode,pclab.ProcCode ProcCodeLab,proc.PatNum,proc.ProcDate "
				+ "FROM procedurelog proc "
				+ "INNER JOIN procedurecode pc ON pc.CodeNum=proc.CodeNum "
				+ "INNER JOIN procedurelog lab ON proc.ProcNum=lab.ProcNumLab AND lab.ProcStatus=" + POut.Long((int)ProcStat.C) + " "
				+ "INNER JOIN procedurecode pclab ON pclab.CodeNum=lab.CodeNum "
				+ "WHERE proc.ProcStatus!=" + POut.Long((int)ProcStat.C);
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "Completed lab fees with treatment planned procedures attached: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   Manual fix needed.  Double click to see a break down.\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", including:\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "Completed lab fee" + " " + table.Rows[i]["ProcCodeLab"].ToString() + " "
									+ "is attached to non-complete procedure" + " " + table.Rows[i]["ProcCode"].ToString() + " "
									+ "on date" + " " + PIn.Date(table.Rows[i]["ProcDate"].ToString()).ToShortDateString() + ". "
									+ "PatNum: " + table.Rows[i]["PatNum"].ToString() + "\r\n";
						}
						log += "   Fix manually from within the Chart module.r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogUnitQtyZero(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT ProcNum FROM procedurelog WHERE UnitQty=0";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = Database.GetListLong(command).Count;
					if (numFound > 0 || verbose)
					{
						log += "Procedures with UnitQty=0 found: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					List<long> listProcNums = Database.GetListLong(command);
					command = @"UPDATE procedurelog        
						SET UnitQty=1
						WHERE UnitQty=0";
					long numberFixed = Database.ExecuteNonQuery(command);
					listProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Procedure,
						 DbmLogActionType.Update, methodName, "Procedure changed from UnitQty=0 to UnitQty=1.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Procedures changed from UnitQty=0 to UnitQty=1: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogWithInvalidProvNum(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT ProcNum FROM procedurelog WHERE ProvNum > 0 AND ProvNum NOT IN (SELECT ProvNum FROM provider)";
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = Database.GetListLong(command).Count;
					if (numFound > 0 || verbose)
					{
						log += "Procedures with invalid ProvNum found: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					List<long> listProcNums = Database.GetListLong(command);
					command = "UPDATE procedurelog SET ProvNum=" + POut.Long(Preferences.GetLong(PreferenceName.PracticeDefaultProv)) +
							" WHERE ProvNum > 0 AND ProvNum NOT IN (SELECT ProvNum FROM provider)";
					long numFixed = Database.ExecuteNonQuery(command);
					listProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Procedure,
						 DbmLogActionType.Update, methodName, "Updated invalid provider from ProcedurelogWithInvalidProvNum.")));
					if (numFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Procedures with invalid ProvNum fixed: " + numFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogWithInvalidAptNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT ProcNum "
						+ "FROM procedurelog "
						+ "WHERE (AptNum NOT IN(SELECT AptNum FROM appointment) AND AptNum!=0) "
						+ "OR (PlannedAptNum NOT IN(SELECT AptNum FROM appointment) AND PlannedAptNum!=0)";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = Database.GetListLong(command).Count;
					if (numFound > 0 || verbose)
					{
						log += "Procedures attached to invalid appointments: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					List<long> listProcNums = Database.GetListLong(command);
					command = "UPDATE procedurelog SET AptNum=0 "
						+ "WHERE AptNum NOT IN(SELECT AptNum FROM appointment) AND AptNum!=0";
					long numberFixed = Database.ExecuteNonQuery(command);
					command = "UPDATE procedurelog SET PlannedAptNum=0 "
						+ "WHERE PlannedAptNum NOT IN(SELECT AptNum FROM appointment) AND PlannedAptNum!=0";
					numberFixed += Database.ExecuteNonQuery(command);
					listProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Procedure,
						 DbmLogActionType.Update, methodName, "Set AptNum to 0 from ProcedurelogWithInvalidAptNum.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Procedures with invalid appointments fixed: " + numberFixed.ToString() + "\r\n";//Do we care enough that this number could be inflated if a procedure had both an invalid AptNum AND PlannedNum?
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogWithInvalidClinicNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = "SELECT ProcNum "
						+ "FROM procedurelog "
						+ "WHERE ClinicNum NOT IN(SELECT ClinicNum FROM clinic) AND ClinicNum!=0 ";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = Database.GetListLong(command).Count;
					if (numFound > 0 || verbose)
					{
						log += "Procedures attached to invalid clinics: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					List<long> listProcNums = Database.GetListLong(command);
					command = "UPDATE procedurelog SET ClinicNum=0 "
						+ "WHERE ClinicNum NOT IN(SELECT ClinicNum FROM clinic) and ClinicNum!=0 ";
					long numberFixed = Database.ExecuteNonQuery(command);
					listProcNums.ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, x, DbmLogFKeyType.Procedure,
							 DbmLogActionType.Update, methodName, "Fixed invalid clinicnum from ProcedurelogWithInvalidClinicNum.")));
					if (numberFixed > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Procedures with invalid clinics fixed" + ": " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}
		#endregion ProcedureLog-------------------------------------------------------------------------------------------------------------------------
		#region ProgramProperty, Provider, QuickPasteNote-----------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string ProgramPropertiesDuplicateLocalPath(bool verbose, DbmMode modeCur)
		{
			//Min may not be the oldest when using random primary keys, but we have to pick one.  In most all cases theyre identical anyway.
			string command = "SELECT MIN(ProgramPropertyNum) ProgramPropertyNum,COUNT(*) CountDup "
					+ "FROM programproperty "
					+ "WHERE PropertyDesc='' "//Blank for workstation overrides of program path.
					+ "GROUP BY ProgramNum,ComputerName,ClinicNum";
			DataTable tableProgProps = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = tableProgProps.Select().Select(x => PIn.Int(x["CountDup"].ToString()) - 1).Sum();
					if (numFound > 0 || verbose)
					{
						log += "Duplicate local path program properties entries found: "
							+ numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					string validProgramPropertyNums = string.Join(",", tableProgProps.Select().Select(x => PIn.Long(x["ProgramPropertyNum"].ToString())));
					long numberFixed = 0;
					if (!validProgramPropertyNums.IsNullOrEmpty())
					{
						command = "DELETE FROM programproperty WHERE PropertyDesc='' "
							+ "AND ProgramPropertyNum NOT IN "
							+ "(" + validProgramPropertyNums + ")";
						numberFixed = Database.ExecuteNonQuery(command);
					}
					if (numberFixed > 0 || verbose)
					{
						log += "Duplicate local path program properties entries found: "
							+ numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProgramPropertiesDuplicatesForHQ(bool verbose, DbmMode modeCur)
		{
			string progNumStr = POut.Long(Programs.GetProgramNum(ProgramName.Xcharge)) + "," + POut.Long(Programs.GetProgramNum(ProgramName.PayConnect));
			//Min may not be the oldest when using random primary keys, but we have to pick one.  In most all cases theyre identical anyway.
			string command = "SELECT MIN(ProgramPropertyNum) ProgramPropertyNum,COUNT(*) Count "
					+ "FROM programproperty "
					+ "WHERE ClinicNum=0 "
					+ "AND ProgramNum IN (" + progNumStr + ") "
					+ "GROUP BY ProgramNum,PropertyDesc";
			DataTable tableProgProps = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = tableProgProps.Select().Select(x => PIn.Int(x["Count"].ToString()) - 1).Sum();
					if (numFound > 0 || verbose)
					{
						log += "X-Charge and/or PayConnect duplicate program property entries found: "
							+ numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "DELETE FROM programproperty WHERE ClinicNum=0 AND ProgramNum IN (" + progNumStr + ") "
						+ "AND ProgramPropertyNum NOT IN (" + string.Join(",", tableProgProps.Select().Select(x => PIn.Long(x["ProgramPropertyNum"].ToString()))) + ")";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "X-Charge and/or PayConnect duplicate program property entries deleted: "
							+ numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		//[DbmMethodAttr]
		//public static string ProgramPropertiesMissingForClinic(bool verbose,DbmMode modeCur) {
		//	//X-Charge and PayConnect are currently the only program links that use ClinicNum.
		//	string progNumStr=POut.Long(Programs.GetProgramNum(ProgramName.Xcharge))+","+POut.Long(Programs.GetProgramNum(ProgramName.PayConnect));
		//	string command="SELECT DISTINCT pphq.*,clinic.ClinicNum missingClinicNum "//Distinct in case there are duplicate prog props with a ClinicNum 0.
		//		+"FROM programproperty pphq "
		//		+"INNER JOIN clinic ON TRUE "
		//		+"LEFT JOIN programproperty ppcl ON ppcl.ProgramNum=pphq.ProgramNum "
		//		+"AND ppcl.PropertyDesc=pphq.PropertyDesc "
		//			+"AND ppcl.ClinicNum=clinic.ClinicNum "
		//		+"WHERE pphq.ProgramNum IN ("+progNumStr+") "
		//		+"AND pphq.ClinicNum=0 "
		//		+"AND pphq.PropertyDesc!='' "
		//		+"AND ppcl.ClinicNum IS NULL ";
		//	DataTable tableProgProps=Database.ExecuteDataTable(command);
		//	string log="";
		//	switch(modeCur) {
		//		case DbmMode.Check:
		//			int numFound=tableProgProps.Rows.Count;
		//			if(numFound>0 || verbose) {
		//				log+="X-Charge and/or PayConnect missing program property entries found: "
		//					+numFound+"\r\n";
		//			}
		//			break;
		//		case DbmMode.Fix:
		//			List<ProgramProperty> listProgProps=Crud.ProgramPropertyCrud.TableToList(tableProgProps);
		//			for(int i = 0;i<listProgProps.Count;i++) {
		//				listProgProps[i].ClinicId=PIn.Long(tableProgProps.Rows[i]["missingClinicNum"].ToString());
		//				ProgramProperties.Insert(listProgProps[i]);
		//			}
		//			long numberFixed=tableProgProps.Rows.Count;
		//			if(numberFixed>0 || verbose) {
		//				log+="X-Charge and/or PayConnect missing program property entries inserted: "
		//					+numberFixed.ToString()+"\r\n";
		//			}
		//			break;
		//	}
		//	return log;
		//}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ProviderHiddenWithClaimPayments(bool verbose, DbmMode modeCur)
		{
			string command = @"SELECT MAX(claimproc.ProcDate) ProcDate,provider.ProvNum
				FROM claimproc,provider
				WHERE claimproc.ProvNum=provider.ProvNum
				AND provider.IsHidden=1
				AND claimproc.InsPayAmt>0
				GROUP BY provider.ProvNum";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "Hidden providers with claim payments: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count > 0)
					{
						log += "\r\n   Manual fix needed.  Double click to see a break down.\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						Provider prov;
						log += ", including:\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							prov = Providers.GetById(PIn.Long(table.Rows[i]["ProvNum"].ToString()));
							log += prov.Abbr + " has claim payments entered as recently as "
								+ PIn.Date(table.Rows[i]["ProcDate"].ToString()).ToShortDateString() + "\r\n";
						}
						log += "   This data will be missing on income reports.\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProviderWithInvalidFeeSched(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = @"SELECT COUNT(*) FROM provider WHERE FeeSched NOT IN (SELECT FeeSchedNum FROM feesched)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Providers found with invalid FeeSched: " + numFound + "\r\n";
					}
					break;

				case DbmMode.Fix:
					command = @"UPDATE provider SET FeeSched=" + POut.Long(FeeScheds.GetFirst(true).Id) + " "
						+ "WHERE FeeSched NOT IN (SELECT FeeSchedNum FROM feesched)";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Providers whose FeeSched has been changed: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string QuickPasteNoteWithInvalidCatNum(bool verbose, DbmMode modeCur)
		{
			string command = @"SELECT COUNT(*) FROM quickpastenote WHERE QuickPasteCatNum=0";
			int numFound = PIn.Int(Database.ExecuteString(command));
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (numFound > 0 || verbose)
					{
						log += "Quick Paste Notes with an invalid category num: " + numFound + "\r\n";
					}
					break;

				case DbmMode.Fix:
					long numberFixed = 0;
					if (numFound > 0)
					{
                        QuickPasteCat quickPasteCatNew = new QuickPasteCat
                        {
                            Description = "DBM GENERATED"
                        };
                        QuickPasteCats.Insert(quickPasteCatNew);
						command = @"UPDATE quickpastenote SET QuickPasteCatNum=" + POut.Long(quickPasteCatNew.QuickPasteCatNum) + " WHERE QuickPasteCatNum=0";
						numberFixed = Database.ExecuteNonQuery(command);
					}
					if (numberFixed > 0 || verbose)
					{
						log += "Quick Paste Notes with an invalid category num fixed: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion ProgramProperty, Provider, QuickPasteNote--------------------------------------------------------------------------------------------
		#region Recall, RecallTrigger, RefAttach, RxAlert---------------------------------------------------------------------------------------

		[DbmMethodAttr(HasBreakDown = true)]
		public static string RecallDuplicatesWarn(bool verbose, DbmMode modeCur)
		{
			if (RecallTypes.PerioType < 1 || RecallTypes.ProphyType < 1)
			{
				return "Warning!  Recall types not set up properly.  There must be at least one of each type: perio and prophy." + "\r\n";
			}
			string command = "SELECT FName,LName,COUNT(*) countDups FROM patient LEFT JOIN recall ON recall.PatNum=patient.PatNum "
				+ "AND (recall.RecallTypeNum=" + POut.Long(RecallTypes.PerioType) + " "
				+ "OR recall.RecallTypeNum=" + POut.Long(RecallTypes.ProphyType) + ") "
				+ "GROUP BY FName,LName,patient.PatNum HAVING countDups>1";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "Number of patients with duplicate recalls: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   " + "Manual fix needed.  Double click to see a break down." + "\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", " + "including" + ":\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							if (i % 3 == 0)
							{
								log += "\r\n   ";
							}
							log += table.Rows[i]["FName"].ToString() + " " + table.Rows[i]["LName"].ToString() + "; ";
						}
						log += "   They need to be fixed manually." + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string RecallsWithInvalidRecallType(bool verbose, DbmMode modeCur)
		{
			string command = @"SELECT recall.RecallTypeNum 
				FROM recall 
				LEFT JOIN recalltype ON recalltype.RecallTypeNum=recall.RecallTypeNum 
				WHERE recalltype.RecallTypeNum IS NULL";
			List<long> listRecallTypeNums = Database.GetListLong(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listRecallTypeNums.Count;
					if (numFound > 0 || verbose)
					{
						log += "Recalls found with invalid recall types: " + numFound + "\r\n";
					}
					break;

				case DbmMode.Fix:
					//Inserting temporary recall types so that the recalls are no longer orphaned
					long numberFixed = listRecallTypeNums.Count;
					listRecallTypeNums = listRecallTypeNums.Distinct().ToList();
					for (int i = 0; i < listRecallTypeNums.Count; i++)
					{
						command = "INSERT INTO recalltype (RecallTypeNum,Description,DefaultInterval,TimePattern,Procedures) VALUES ("
							+ POut.Long(listRecallTypeNums[i]) + ",'Temporary Recall " + POut.Int(i + 1) + "',0,'','')";
						Database.ExecuteNonQuery(command);
					}
					long numberFixedTypes = listRecallTypeNums.Count;
					if (numberFixedTypes > 0)
					{
						Signalods.SetInvalid(InvalidType.RecallTypes);
					}
					if (numberFixed > 0 || verbose)
					{
						log += 
							"Recalls fixed with invalid recall types: " + numberFixed + ". " +
							"Temporary recall types added:" + " " + numberFixedTypes + ". " +
							"Go to Setup | Appointments | Recall Types to either rename them or remove all recalls from these recall types.\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string RecallTriggerDeleteBadCodeNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM recalltrigger WHERE NOT EXISTS (SELECT * FROM procedurecode WHERE procedurecode.CodeNum=recalltrigger.CodeNum)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Recall triggers found with bad codenum: " + numFound + "\r\n";
					}
					break;

				case DbmMode.Fix:
					command = @"DELETE FROM recalltrigger
						WHERE NOT EXISTS (SELECT * FROM procedurecode WHERE procedurecode.CodeNum=recalltrigger.CodeNum)";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0)
					{
						Signalods.SetInvalid(InvalidType.RecallTypes);
					}
					if (numberFixed > 0 || verbose)
					{
						log += "Recall triggers deleted due to bad codenum: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string RefAttachDeleteWithInvalidReferral(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM refattach WHERE ReferralNum NOT IN (SELECT ReferralNum FROM referral)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Ref attachments found with invalid referrals: " + numFound + "\r\n";
					}
					break;

				case DbmMode.Fix:
					command = "DELETE FROM refattach WHERE ReferralNum NOT IN (SELECT ReferralNum FROM referral)";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Ref attachments with invalid referrals deleted: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Finds patients that have a more than 1 from referral with the same order.</summary>
		[DbmMethodAttr]
		public static string RefAttachesWithDuplicateOrder(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = @"SELECT DISTINCT PatNum 
						FROM refattach 
						GROUP BY PatNum,ItemOrder
						HAVING COUNT(*) > 1";
					int numFound = Database.GetListLong(command).Count;
					if (numFound > 0 || verbose)
					{
						log += "Patients found with multiple referral attachments of the same order: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = @"SELECT refattach.*
						FROM (
							SELECT DISTINCT PatNum
							FROM refattach
							GROUP BY PatNum,ItemOrder
							HAVING COUNT(*) > 1
						) multattach
						INNER JOIN refattach ON refattach.PatNum=multattach.PatNum";
					List<RefAttach> listAttaches = Crud.RefAttachCrud.SelectMany(command);
					foreach (List<RefAttach> listAttachesForPat in listAttaches.GroupBy(x => x.PatNum).Select(x => x.ToList()))
					{
						//Change the order of all ref attaches on the patient so that none have the same ItemOrder.
						int itemOrder = 1;
						foreach (RefAttach attach in listAttachesForPat.OrderBy(x => x.ItemOrder).ThenBy(x => x.RefDate).ThenBy(x => x.RefAttachNum))
						{
							RefAttach attachOld = attach.Copy();
							attach.ItemOrder = itemOrder++;
							RefAttaches.Update(attach, attachOld);
						}
					}
					long numberFixed = listAttaches.Select(x => x.PatNum).Distinct().Count();
					if (numberFixed > 0 || verbose)
					{
						log += "Patients fixed with multiple referral attachments of the same order: " + numberFixed + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string RxAlertBadAllergyDefNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM rxalert WHERE rxalert.AllergyDefNum!=0 AND NOT EXISTS (SELECT * FROM allergydef WHERE allergydef.AllergyDefNum=rxalert.AllergyDefNum)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Rx alerts with bad allergy definitions: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					//command=@"SELECT * FROM rxalert WHERE NOT EXISTS (SELECT * FROM allergydef WHERE allergydef.AllergyDefNum=rxalert.AllergyDefNum)";
					//table=Db.GetTable(command);
					command = "UPDATE rxalert SET AllergyDefNum=0 WHERE rxalert.AllergyDefNum!=0 AND NOT EXISTS (SELECT * FROM allergydef WHERE allergydef.AllergyDefNum=rxalert.AllergyDefNum)";
					long rowsChanged = Database.ExecuteNonQuery(command);
					if (rowsChanged > 0 || verbose)
					{
						log += "Rx alerts with bad allergy definitions cleared: " + rowsChanged.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Recall, RecallTrigger, RefAttach, RxAlert------------------------------------------------------------------------------------
		#region ScheduleOp, Schedule, SecurityLog, Sheet------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string ScheduleOpsInvalidScheduleNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM scheduleop WHERE NOT EXISTS(SELECT * FROM schedule WHERE scheduleop.ScheduleNum=schedule.ScheduleNum)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Scheduleops with invalid ScheduleNums found: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "DELETE FROM scheduleop WHERE NOT EXISTS(SELECT * FROM schedule WHERE scheduleop.ScheduleNum=schedule.ScheduleNum)";
					long numFixed = Database.ExecuteNonQuery(command);
					if (numFixed > 0 || verbose)
					{
						log += "Scheduleops with invalid ScheduleNums deleted: " + numFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string SchedulesBlockoutStopBeforeStart(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM schedule WHERE StopTime<StartTime";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Schedules and blockouts having stop time before start time: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "DELETE FROM schedule WHERE StopTime<StartTime";
					long numFixed = Database.ExecuteNonQuery(command);
					if (numFixed > 0 || verbose)
					{
						log += "Schedules and blockouts having stop time before start time fixed: " + numFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string SchedulesDeleteHiddenProviders(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM provider WHERE IsHidden=1 AND ProvNum IN (SELECT ProvNum FROM schedule WHERE SchedDate > " + DbHelper.Now() + " GROUP BY ProvNum)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Hidden providers found on future schedules: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "SELECT ProvNum FROM provider WHERE IsHidden=1 AND ProvNum IN (SELECT ProvNum FROM schedule WHERE SchedDate > " + DbHelper.Now() + " GROUP BY ProvNum)";
					DataTable table = Database.ExecuteDataTable(command);
					List<long> provNums = new List<long>();
					for (int i = 0; i < table.Rows.Count; i++)
					{
						provNums.Add(PIn.Long(table.Rows[i]["ProvNum"].ToString()));
					}
					Providers.RemoveProvsFromFutureSchedule(provNums);//Deletes future schedules for providers.
					if (provNums.Count > 0 || verbose)
					{
						log += "Hidden providers found on future schedules fixed: " + provNums.Count.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string SchedulesDeleteShort(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command = @"SELECT schedule.ScheduleNum
				FROM schedule
				WHERE schedule.Status=" + POut.Int((int)SchedStatus.Open)/*closed and holiday statuses do not use starttime and stoptime*/+ @"
				AND TIMEDIFF(schedule.StopTime,schedule.StartTime)<'00:05:00'
				AND (schedule.Note=''"/*we don't want to remove provider notes, employee notes, or pratice notes.*/+ @"
				OR schedule.SchedType=" + POut.Int((int)ScheduleType.WebSchedASAP) + ")";
			List<long> listSchedulesToDelete = Database.GetListLong(command);
			switch (modeCur)
			{
				case DbmMode.Check:
					int numFound = listSchedulesToDelete.Count;
					if (numFound > 0 || verbose)
					{
						log += "Schedule blocks invalid: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					int numberFixed = listSchedulesToDelete.Count;
					if (listSchedulesToDelete.Count > 0)
					{
						command = "DELETE FROM schedule WHERE ScheduleNum IN(" + string.Join(",", listSchedulesToDelete) + ")";
						Database.ExecuteNonQuery(command);
					}
					if (numberFixed > 0 || verbose)
					{
						log += "Schedule blocks fixed: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string SchedulesDeleteProvClosed(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM schedule WHERE SchedType=1 AND Status=1";//type=prov,status=closed
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Schedules found which are causing printing issues: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "DELETE FROM schedule WHERE SchedType=1 AND Status=1";//type=prov,status=closed
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Schedules deleted that were causing printing issues: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string SheetDepositSlips(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT SheetNum FROM sheet WHERE SheetType=" + POut.Int((int)SheetTypeEnum.DepositSlip);
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Deposit slip sheets: " + table.Rows.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					if (table.Rows.Count > 0)
					{
						for (int i = 0; i < table.Rows.Count; i++)
						{
							long sheetNum = PIn.Long(table.Rows[i]["SheetNum"].ToString());
							command = "DELETE FROM sheetfield WHERE SheetNum=" + POut.Long(sheetNum);
							Database.ExecuteNonQuery(command);
							command = "DELETE FROM sheet WHERE SheetNum=" + POut.Long(sheetNum);
							Database.ExecuteNonQuery(command);
						}
					}
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Deposit slip sheets deleted: " + table.Rows.Count + "\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>At one point, we had an issue on our Web Forms server that possibly caused offices to import 1000's of blank Web Forms.</summary>
		[DbmMethodAttr]
		public static string SheetsWithNoSheetFields(bool verbose, DbmMode modeCur)
		{
			string command = @"SELECT sheet.SheetNum 
				FROM sheet 
				LEFT JOIN sheetfield ON sheetfield.SheetNum=sheet.SheetNum
				WHERE sheet.IsWebForm=1
				AND sheetfield.SheetNum IS NULL";
			List<long> listSheetNums = Database.GetListLong(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (listSheetNums.Count > 0 || verbose)
					{
						log += "Blank Web Forms sheets found: " + listSheetNums.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					if (listSheetNums.Count > 0)
					{
						command = "DELETE FROM sheet WHERE SheetNum IN(" + string.Join(",", listSheetNums.Select(POut.Long)) + ")";
						Database.ExecuteNonQuery(command);
					}
					if (listSheetNums.Count > 0 || verbose)
					{
						log += "Blank Web Forms sheets deleted: " + listSheetNums.Count + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion ScheduleOp, Schedule, SecurityLog, Sheet---------------------------------------------------------------------------------------------
		#region Signal, SigMessage, Statement, SummaryOfCare--------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string SignalInFuture(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = @"SELECT COUNT(*) FROM signalod WHERE SigDateTime > NOW()";
					long numFound = PIn.Long(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Signalod entries with future time: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = @"DELETE FROM signalod WHERE SigDateTime > NOW()";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Signalod entries with future times deleted: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string SigMessageInFuture(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = @"SELECT COUNT(*) FROM sigmessage WHERE MessageDateTime > NOW() OR AckDateTime > NOW()";
					long numFound = PIn.Long(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Sigmessage entries with future time: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = @"DELETE FROM sigmessage WHERE MessageDateTime > NOW() OR AckDateTime > NOW()";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Sigmessage entries with future times deleted: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string StatementDateRangeMax(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM statement WHERE DateRangeTo='9999-12-31'";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Statement DateRangeTo max found: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "UPDATE statement SET DateRangeTo='2200-01-01' WHERE DateRangeTo='9999-12-31'";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Statement DateRangeTo max fixed: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string StatementsWithInvalidDocNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM statement WHERE DocNum>0 AND DocNum NOT IN (SELECT DocNum FROM document)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Statements with invalid DocNum found" + ": " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "UPDATE statement SET DocNum=0 WHERE DocNum>0 AND DocNum NOT IN (SELECT DocNum FROM document)";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Statements with invalid DocNum fixed" + ": " + numberFixed + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string SummaryOfCaresWithoutReferralsAttached(bool verbose, DbmMode modeCur)
		{
			//if (modeCur == DbmMode.Breakdown)
			//{
			//	return "";
			//}
			//string log = "";
			//string command = "SELECT * FROM refattach WHERE RefAttachNum NOT IN ("
			//		+ "SELECT FKey FROM ehrmeasureevent WHERE EventType=" + POut.Int((int)EhrMeasureEventType.SummaryOfCareProvidedToDr) + " "
			//		+ "OR EventType=" + POut.Int((int)EhrMeasureEventType.SummaryOfCareProvidedToDrElectronic) + ") "
			//	+ "AND RefType=" + POut.Int((int)ReferralType.RefTo) + " "
			//	+ "AND IsTransitionOfCare=1 ";
			////We want to fix as many measure events as we can even if they aren't good enough to count towards the actual measure. 
			////+"AND ProvNum!=0";//E.g. we will link measure events to refattaches even if the ref attach has no provider.  This way, they only have to fix the ref attach in order for their measures to show.
			//List<RefAttach> refAttaches = Crud.RefAttachCrud.SelectMany(command);
			//command = "SELECT * FROM ehrmeasureevent "
			//	+ "WHERE FKey NOT IN (SELECT RefAttachNum FROM refattach WHERE RefType=" + POut.Int((int)ReferralType.RefTo) + " AND IsTransitionOfCare=1) "
			//	+ "AND EventType=" + POut.Int((int)EhrMeasureEventType.SummaryOfCareProvidedToDr) + " "
			//	+ "OR EventType=" + POut.Int((int)EhrMeasureEventType.SummaryOfCareProvidedToDrElectronic) + " "
			//	+ "ORDER BY DateTEvent";
			//List<EhrMeasureEvent> measureEvents = Crud.EhrMeasureEventCrud.SelectMany(command);
			//int numberFixed = 0;
			//for (int i = 0; i < refAttaches.Count; i++)
			//{
			//	for (int j = 0; j < measureEvents.Count; j++)
			//	{
			//		if (refAttaches[i].PatNum == measureEvents[j].PatientId
			//				&& measureEvents[j].ObjectId == 0
			//				&& measureEvents[j].Date >= refAttaches[i].RefDate.AddDays(-3)
			//				&& measureEvents[j].Date <= refAttaches[i].RefDate.AddDays(1))
			//		{
			//			if (modeCur != DbmMode.Check)
			//			{
			//				measureEvents[j].ObjectId = refAttaches[i].RefAttachNum;
			//				EhrMeasureEvents.Update(measureEvents[j]);
			//			}
			//			measureEvents.RemoveAt(j);
			//			numberFixed++;
			//			break;
			//		}
			//	}
			//}
			//if (modeCur == DbmMode.Check && (numberFixed > 0 || verbose))
			//{
			//	log += "Summary of cares with no referrals attached: " + numberFixed.ToString() + "\r\n";
			//}
			//else if (modeCur != DbmMode.Check && (numberFixed > 0 || verbose))
			//{
			//	log += "Summary of cares that had a referral attached: " + numberFixed.ToString() + "\r\n";
			//}
			//return log;


			return "";
		}

		#endregion Signal, SigMessage, Statement, SummaryOfCare-----------------------------------------------------------------------------------------
		#region Task, TaskList, TimeCardRule, TreatPlan-------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string TaskListsWithCircularParentChild(bool verbose, DbmMode modeCur)
		{
			//In order to figure out a cyclical chain of task lists we need to:
			//1. Get all TaskLists
			//2. For each TaskList whose Parent is 0...
			//a. Find all TaskLists in that "family".  We know that in order to have a cyclical relationship NONE of the TaskLists in the cycle can have Parent of 0
			//b. Remove those TaskLists from the "bad list"
			//c. When we run out of TaskLists with Parent of 0, the TaskLists left in the list are those that are part of a TaskList cycle
			//Get a list of all TaskLists
			string command = "SELECT * FROM tasklist";
			List<TaskList> listAllTaskLists = TaskLists.GetAll().ToList();
			List<TaskList> listTrunkTaskLists = listAllTaskLists.FindAll(x => x.ParentId == 0);//Find first TaskList with Parent of 0
			listAllTaskLists.RemoveAll(x => x.ParentId == 0);
			Action<long> RemoveAncestors = null;
			//Delegate method to recursively traverse the tree of a TaskList and remove all child TaskLists.
			RemoveAncestors = new Action<long>(taskListNum =>
			{
				List<TaskList> listChildren = listAllTaskLists.FindAll(x => x.ParentId == taskListNum);
				foreach (TaskList childList in listChildren)
				{
					RemoveAncestors.Invoke(childList.Id);
					listAllTaskLists.Remove(childList);
				}
			});
			foreach (TaskList taskListParent in listTrunkTaskLists)
			{
				RemoveAncestors.Invoke(taskListParent.Id);
			}
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (listAllTaskLists.Count > 0 || verbose)
					{
						log += "Task Lists with circular parent-child relationship: " + listAllTaskLists.Count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					long taskListNum = 0;
					if (listAllTaskLists.Count > 0)
					{
						command = "INSERT INTO tasklist (Descript,Parent,DateTL,IsRepeating,DateType,FromNum,ObjectType,DateTimeEntry) VALUES('FIX TASKLISTS',0,'0001-01-01',0,0,0,0,CURDATE())";
						taskListNum = Database.ExecuteInsert(command);
					}
					foreach (TaskList taskList in listAllTaskLists)
					{
						//We will set each TaskList's parent to be 0 so the user can again access them via the Main tab and put them wherever they want.
						command = "UPDATE tasklist SET Parent=" + POut.Long(taskListNum) + " WHERE TaskListNum=" + taskList.Id.ToString();
						Database.ExecuteNonQuery(command);
					}
					if (listAllTaskLists.Count > 0 || verbose)
					{
						log += "Task Lists with circular parent-child relationship corrected: " + listAllTaskLists.Count.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string TasksCompletedWithInvalidFinishDateTime(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT task.TaskNum,IFNULL(MAX(tasknote.DateTimeNote),task.DateTimeEntry) AS DateTimeNoteMax "
				+ "FROM task "
				+ "LEFT JOIN tasknote ON task.TaskNum=tasknote.TaskNum "
				+ "WHERE task.TaskStatus=" + POut.Int((int)TaskStatus.Done) + " "
				+ "AND task.DateTimeFinished=" + POut.DateT(new DateTime(1, 1, 1)) + " "
				+ "GROUP BY task.TaskNum";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Tasks completed with invalid Finished Date/Time: " + table.Rows.Count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					foreach (DataRow row in table.Rows)
					{
						//Update the DateTimeFinished with either the max note DateTime or the time of the tasks DateTimeEntry.
						//We cannot use the raw string in the DataTable because C# has auto-formatted the row into a DateTime row.
						//Therefore we have to convert the string into a DateTime object and then send it back out in the format that MySQL expects.
						DateTime dateTimeNoteMax = PIn.Date(row["DateTimeNoteMax"].ToString());
						command = "UPDATE task SET DateTimeFinished=" + POut.DateT(dateTimeNoteMax) + " "
							+ "WHERE TaskNum=" + row["TaskNum"].ToString();
						Database.ExecuteNonQuery(command);
					}
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Tasks completed with invalid Finished Date/Times corrected: " + table.Rows.Count.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string TaskSubscriptionsInvalid(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM tasksubscription "
						+ "WHERE NOT EXISTS(SELECT * FROM tasklist WHERE tasksubscription.TaskListNum=tasklist.TaskListNum)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Task subscriptions invalid: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "DELETE FROM tasksubscription "
						+ "WHERE NOT EXISTS(SELECT * FROM tasklist WHERE tasksubscription.TaskListNum=tasklist.TaskListNum)";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Task subscriptions deleted: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string TaskUnreadsWithoutTasksAttached(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM taskunread "
						+ "WHERE taskunread.TaskNum NOT IN(SELECT TaskNum FROM task)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Unread task notifications for deleted tasks: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "DELETE FROM taskunread "
						+ "WHERE taskunread.TaskNum NOT IN(SELECT TaskNum FROM task)";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Unread task notifications for deleted tasks removed: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string TimeCardRuleEmployeeNumInvalid(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM timecardrule "
						+ "WHERE timecardrule.EmployeeNum!=0 " //0 is all employees, so it is a 'valid' employee number
						+ "AND timecardrule.EmployeeNum NOT IN(SELECT employee.EmployeeNum FROM employee)";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Time card rules found with invalid employee number: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "UPDATE timecardrule "
						+ "SET timecardrule.EmployeeNum=0 "
						+ "WHERE timecardrule.EmployeeNum!=0 " //don't set to 0 if already 0
						+ "AND timecardrule.EmployeeNum NOT IN(SELECT employee.EmployeeNum FROM employee)";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Time card rules applied to All Employees due to invalid employee number: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string TreatPlansInvalid(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT treatplan.PatNum FROM procedurelog	"//procs for 1 pat attached to a treatplan for another
					+ "INNER JOIN treatplanattach ON treatplanattach.ProcNum=procedurelog.ProcNum "
					+ "INNER JOIN treatplan ON treatplan.TreatPlanNum=treatplanattach.TreatPlanNum AND procedurelog.PatNum!=treatplan.PatNum "
					+ "UNION "//more than 1 active treatment plan
					+ "SELECT PatNum FROM treatplan WHERE TPStatus=1 GROUP BY PatNum HAVING COUNT(DISTINCT TreatPlanNum)>1";
			List<long> listPatNumsForAudit = Database.GetListLong(command).Distinct().ToList();
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (listPatNumsForAudit.Count > 0 || verbose)
					{
						log += "Patients found with one or more invalid treatment plans: " + listPatNumsForAudit.Count + "\r\n";
					}
					break;
				case DbmMode.Fix:
					listPatNumsForAudit.ForEach(x => TreatPlans.AuditPlans(x
						, (Patients.GetPat(x).DiscountPlanNum == 0 ? TreatPlanType.Insurance : TreatPlanType.Discount)));
					TreatPlanAttaches.DeleteOrphaned();
					if (listPatNumsForAudit.Count > 0 || verbose)
					{
						log += "Patients with one or more invalid treatment plans fixed: " + listPatNumsForAudit.Count + "\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Finds treatplanattaches with the same treatplannum and procnum.</summary>
		[DbmMethodAttr]
		public static string TreatPlanAttachDuplicateProc(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT treatplanattach.TreatPlanNum, MIN(treatplanattach.TreatPlanAttachNum) AS OriginalTPANum, "
			+ " ProcNum, "
			+ " COUNT(ProcNum) NumDupes "
			+ " FROM treatplanattach "
			+ " GROUP BY treatplanattach.treatplannum, treatplanattach.ProcNum "
			+ " HAVING NumDupes > 1 ";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "TreatPlanAttaches with duplicate ProcNums and TreatPlanNums found: " + table.Rows.Count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					for (int i = 0; i < table.Rows.Count; i++)
					{
						command = "DELETE FROM treatplanattach WHERE treatplanattach.TreatPlanNum=" + table.Rows[i]["TreatPlanNum"]
							+ " AND treatplanattach.ProcNum=" + table.Rows[i]["ProcNum"]
							+ " AND treatplanattach.TreatPlanAttachNum != " + table.Rows[i]["OriginalTPANum"];
						Database.ExecuteNonQuery(command);
					}
					if (table.Rows.Count > 0 || verbose)
					{
						log += "TreatPlanAttaches with duplicate ProcNums and TreatPlanNums deleted: " + table.Rows.Count.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Finds proctps that have been orphaned and creates dummy treatment plans for DateTime.MinValue so that the orphaned proctps can be viewed.</summary>
		[DbmMethodAttr]
		public static string TreatPlanOrphanedProcTps(bool verbose, DbmMode modeCur)
		{
			string command = @"SELECT proctp.PatNum,proctp.TreatPlanNum 
				FROM proctp
				LEFT JOIN treatplan ON treatplan.TreatPlanNum = proctp.TreatPlanNum 
				WHERE treatplan.TreatPlanNum IS NULL 
				GROUP BY proctp.TreatPlanNum";
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Treatment Plans with orphaned proctps: " + table.Rows.Count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					for (int i = 0; i < table.Rows.Count; i++)
					{
						TreatPlan tp = new TreatPlan()
						{
							DateTP = DateTime.MinValue,
							Heading = "MISSING TREATMENT PLAN",
							Note = "This treatment plan was created by Database Maintenence because of orphaned proctps.",
							PatNum = PIn.Long(table.Rows[i]["PatNum"].ToString()),
							SecUserNumEntry = Security.CurrentUser.Id,
							TreatPlanNum = PIn.Long(table.Rows[i]["TreatPlanNum"].ToString()),
							TPStatus = TreatPlanStatus.Saved
						};
						Crud.TreatPlanCrud.Insert(tp, true);
					}
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Treatment Plans with orphaned proctps fixed: " + table.Rows.Count.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Task, TaskList, TimeCardRule, TreatPlan----------------------------------------------------------------------------------------------
		#region UnscheduledAppt, Userod-----------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string UnscheduledApptsWithInvalidOpNum(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT AptNum FROM appointment WHERE Op != 0 AND AptStatus=3";//UnschedList
			DataTable table = Database.ExecuteDataTable(command);
			string log = "";
			switch (modeCur)
			{
				case DbmMode.Check:
					if (table.Rows.Count > 0 || verbose)
					{
						log += "Unscheduled appointments with invalid Op nums: " + table.Rows.Count.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					List<DbmLog> listDbmLogs = new List<DbmLog>();
					string methodName = MethodBase.GetCurrentMethod().Name;
					command = "UPDATE appointment SET Op=0 WHERE AptStatus=3";//UnschedList
					Database.ExecuteNonQuery(command);
					table.Select().ForEach(x => listDbmLogs.Add(new DbmLog(Security.CurrentUser.Id, PIn.Long(x["AptNum"].ToString()), DbmLogFKeyType.Appointment,
						DbmLogActionType.Update, methodName, "Fixed invalid OpNum from UnscheduledApptsWithInvalidOpNum.")));
					if (table.Rows.Count > 0 || verbose)
					{
						Crud.DbmLogCrud.InsertMany(listDbmLogs);
						log += "Unscheduled appointments with invalid Op nums corrected: " + table.Rows.Count.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		/// <summary>Only one user of a given UserName may be unhidden at a time. Warn the user and instruct them to hide extras.</summary>
		[DbmMethodAttr(HasBreakDown = true)]
		public static string UserodDuplicateUser(bool verbose, DbmMode modeCur)
		{
			string command = "SELECT UserName FROM userod WHERE IsHidden=0 GROUP BY UserName HAVING Count(*)>1;";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0 && !verbose)
			{
				return "";
			}
			string log = "Users with duplicates: " + table.Rows.Count;
			switch (modeCur)
			{
				case DbmMode.Check:
				case DbmMode.Fix:
					if (table.Rows.Count != 0)
					{
						log += "\r\n   Manual fix needed.  Double click to see a break down.\r\n";
					}
					break;
				case DbmMode.Breakdown:
					if (table.Rows.Count > 0)
					{
						log += ", including:\r\n";
						for (int i = 0; i < table.Rows.Count; i++)
						{
							log += "User - " + table.Rows[i]["UserName"].ToString() + "\r\n";
						}
						log += "   They need to be fixed manually. Please go to Setup | Security and hide all but one of each unique user.\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string UserodInvalidClinicNum(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT Count(*) FROM userod WHERE ClinicNum<>0 AND ClinicNum NOT IN (SELECT ClinicNum FROM clinic)";
					long numFound = PIn.Long(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Users found with invalid ClinicNum: " + numFound + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "UPDATE userod SET ClinicNum=0 WHERE ClinicNum<>0 AND ClinicNum NOT IN (SELECT ClinicNum FROM clinic)";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Users fixed with invalid ClinicNum: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		/// <summary>userod is restricted to ClinicNum 0 - All.  Restricted to All clinics doesn't make sense.  This will set the ClinicIsRestricted bool to false if ClinicNum=0.</summary>
		[DbmMethodAttr]
		public static string UserodInvalidRestrictedClinic(bool verbose, DbmMode modeCur)
		{
			string log = "";
			string command;
			switch (modeCur)
			{
				case DbmMode.Check:
					command = "SELECT COUNT(*) FROM userod WHERE ClinicNum=0 AND ClinicIsRestricted=1";
					int numFound = PIn.Int(Database.ExecuteString(command));
					if (numFound > 0 || verbose)
					{
						log += "Users found restricted to an invalid clinic: " + numFound.ToString() + "\r\n";
					}
					break;
				case DbmMode.Fix:
					command = "UPDATE userod SET ClinicIsRestricted=0 WHERE ClinicNum=0 AND ClinicIsRestricted=1";
					long numberFixed = Database.ExecuteNonQuery(command);
					if (numberFixed > 0 || verbose)
					{
						log += "Users fixed with restriction to an invalid clinic: " + numberFixed.ToString() + "\r\n";
					}
					break;
			}
			return log;
		}

		#endregion UnscheduledAppt, Userod--------------------------------------------------------------------------------------------------------------

		#endregion Methods That Apply to Specific Tables----------------------------------------------------------------------------------------------------
		#region Tool Button and Helper Methods--------------------------------------------------------------------------------------------------------------

		public static List<string> GetDatabaseNames()
		{
			List<string> retVal = new List<string>();
			string command = "SHOW DATABASES";
			//if this next step fails, table will simply have 0 rows
			DataTable table = Database.ExecuteDataTable(command);
			for (int i = 0; i < table.Rows.Count; i++)
			{
				retVal.Add(table.Rows[i][0].ToString());
			}
			return retVal;
		}

		///<summary>Will return empty string if no problems.</summary>
		public static string GetDuplicateClaimProcs()
		{
			string retVal = "";
			string command = @"SELECT LName,FName,patient.PatNum,ClaimNum,FeeBilled,Status,ProcNum,ProcDate,ClaimProcNum,InsPayAmt,LineNumber, COUNT(*) cnt
FROM claimproc
LEFT JOIN patient ON patient.PatNum=claimproc.PatNum
WHERE ClaimNum > 0
AND ProcNum>0
AND Status!=4/*exclude supplemental*/
GROUP BY LName,FName,patient.PatNum,ClaimNum,FeeBilled,Status,ProcNum,ProcDate,ClaimProcNum,InsPayAmt,LineNumber 
HAVING cnt>1";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0)
			{
				return "";
			}
			retVal += "Duplicate claim payments found:\r\n";
			DateTime date;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				if (i > 0)
				{//check for duplicate rows.  We only want to report each claim once.
					if (table.Rows[i]["ClaimNum"].ToString() == table.Rows[i - 1]["ClaimNum"].ToString())
					{
						continue;
					}
				}
				date = PIn.Date(table.Rows[i]["ProcDate"].ToString());
				retVal += table.Rows[i]["LName"].ToString() + ", "
					+ table.Rows[i]["FName"].ToString() + " "
					+ "(" + table.Rows[i]["PatNum"].ToString() + "), "
					+ date.ToShortDateString() + "\r\n";
			}
			retVal += "\r\n";
			return retVal;
		}

		///<summary>Will return empty string if no problems.</summary>
		public static string GetDuplicateSupplementalPayments()
		{
			string retVal = "";
			string command = @"SELECT LName,FName,patient.PatNum,ClaimNum,FeeBilled,Status,ProcNum,ProcDate,ClaimProcNum,InsPayAmt,LineNumber, COUNT(*) cnt
FROM claimproc
LEFT JOIN patient ON patient.PatNum=claimproc.PatNum
WHERE ClaimNum > 0
AND ProcNum>0
AND Status=4/*only supplemental*/
GROUP BY LName,FName,patient.PatNum,ClaimNum,FeeBilled,Status,ProcNum,ProcDate,ClaimProcNum,InsPayAmt,LineNumber
HAVING cnt>1";
			DataTable table = Database.ExecuteDataTable(command);
			if (table.Rows.Count == 0)
			{
				return "";
			}
			retVal += "Duplicate supplemental payments found (may be false positives):\r\n";
			DateTime date;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				if (i > 0)
				{
					if (table.Rows[i]["ClaimNum"].ToString() == table.Rows[i - 1]["ClaimNum"].ToString())
					{
						continue;
					}
				}
				date = PIn.Date(table.Rows[i]["ProcDate"].ToString());
				retVal += table.Rows[i]["LName"].ToString() + ", "
					+ table.Rows[i]["FName"].ToString() + " "
					+ "(" + table.Rows[i]["PatNum"].ToString() + "), "
					+ date.ToShortDateString() + "\r\n";
			}
			retVal += "\r\n";
			return retVal;
		}

		///<summary></summary>
		public static string GetMissingClaimProcs(string olddb)
		{
			string retVal = "";
			string command = "SELECT LName,FName,patient.PatNum,ClaimNum,FeeBilled,Status,ProcNum,ProcDate,ClaimProcNum,InsPayAmt,LineNumber "
				+ "FROM " + olddb + ".claimproc "
				+ "LEFT JOIN " + olddb + ".patient ON " + olddb + ".patient.PatNum=" + olddb + ".claimproc.PatNum "
				+ "WHERE NOT EXISTS(SELECT * FROM claimproc WHERE claimproc.ClaimProcNum=" + olddb + ".claimproc.ClaimProcNum) "
				+ "AND ClaimNum > 0 AND ProcNum>0";
			DataTable table = Database.ExecuteDataTable(command);
			double insPayAmt;
			double feeBilled;
			int count = 0;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				insPayAmt = PIn.Double(table.Rows[i]["InsPayAmt"].ToString());
				feeBilled = PIn.Double(table.Rows[i]["FeeBilled"].ToString());
				command = "SELECT COUNT(*) FROM " + olddb + ".claimproc "
					+ "WHERE ClaimNum= " + table.Rows[i]["ClaimNum"].ToString() + " "
					+ "AND ProcNum= " + table.Rows[i]["ProcNum"].ToString() + " "
					+ "AND Status= " + table.Rows[i]["Status"].ToString() + " "
					+ "AND InsPayAmt= '" + POut.Double(insPayAmt) + "' "
					+ "AND FeeBilled= '" + POut.Double(feeBilled) + "' "
					+ "AND LineNumber= " + table.Rows[i]["LineNumber"].ToString();
				string result = Database.ExecuteString(command);
				if (result != "1")
				{//only include in result if there are duplicates in old db.
					count++;
				}
			}
			command = "SELECT ClaimPaymentNum "
				+ "FROM " + olddb + ".claimpayment "
				+ "WHERE NOT EXISTS(SELECT * FROM claimpayment WHERE claimpayment.ClaimPaymentNum=" + olddb + ".claimpayment.ClaimPaymentNum) ";
			DataTable table2 = Database.ExecuteDataTable(command);
			if (count == 0 && table2.Rows.Count == 0)
			{
				return "";
			}
			retVal += "Missing claim payments found: " + count.ToString() + "\r\n";
			retVal += "Missing claim checks found (probably false positives): " + table2.Rows.Count.ToString() + "\r\n";
			return retVal;
		}

		///<summary>Removes unsupported unicode characters from appointment.ProcDescript, appointment.Note, and patient.AddrNote.
		///Also removes mysql null character ("\0" or CHAR(0)) from several columns from several tables.
		///These null characters were causing the middle tier deserialization to fail as they are not UTF-16 supported characters.
		///They are, however, allowed in UTF-8.</summary>
		public static void FixSpecialCharacters()
		{
			string command = "SELECT * FROM appointment WHERE (ProcDescript REGEXP '[^[:alnum:]^[:space:]^[:punct:]]+') OR (Note REGEXP '[^[:alnum:]^[:space:]^[:punct:]]+')";
			List<Appointment> apts = Crud.AppointmentCrud.SelectMany(command);
			List<char> specialCharsFound = new List<char>();
			int specialCharCount = 0;

            int intC;
            if (apts.Count != 0)
			{
				foreach (Appointment apt in apts)
				{
					foreach (char c in apt.Note)
					{
						intC = c;
						if ((intC < 126 && intC > 31)//31 - 126 are all safe.
							|| intC == 9     //"Horizontal Tabulation"
							|| intC == 10    //Line Feed
							|| intC == 13)
						{ //carriage return
							continue;
						}
						specialCharCount++;
						if (specialCharsFound.Contains(c))
						{
							continue;
						}
						specialCharsFound.Add(c);
					}

					foreach (char c in apt.ProcDescript)
					{//search every character in ProcDescript
						intC = c;
						if ((intC < 126 && intC > 31)//31 - 126 are all safe.
							|| intC == 9     //"Horizontal Tabulation"
							|| intC == 10    //Line Feed
							|| intC == 13)
						{ //carriage return
							continue;
						}
						specialCharCount++;
						if (specialCharsFound.Contains(c))
						{
							continue;
						}
						specialCharsFound.Add(c);
					}
				}

				foreach (char c in specialCharsFound)
				{
					command = "UPDATE appointment SET Note=REPLACE(Note,'" + POut.String(c.ToString()) + "',''), ProcDescript=REPLACE(ProcDescript,'" + POut.String(c.ToString()) + "','')";
					Database.ExecuteNonQuery(command);
				}
			}

			command = "SELECT * FROM patient WHERE AddrNote REGEXP '[^[:alnum:]^[:space:]]+'";
			List<Patient> pats = Crud.PatientCrud.SelectMany(command);
			specialCharsFound = new List<char>();
			specialCharCount = 0;

			if (pats.Count > 0)
			{
				foreach (Patient pat in pats)
				{
					foreach (char c in pat.AddrNote)
					{
						intC = c;
						if ((intC < 126 && intC > 31)//31 - 126 are all safe.
							|| intC == 9      //"Horizontal Tabulation"
							|| intC == 10     //Line Feed
							|| intC == 13)
						{  //carriage return
							continue;
						}

						specialCharCount++;
						if (specialCharsFound.Contains(c))
						{
							continue;
						}
						specialCharsFound.Add(c);
					}
				}

				foreach (char c in specialCharsFound)
				{
					command = "UPDATE patient SET AddrNote=REPLACE(AddrNote,'" + POut.String(c.ToString()) + "','')";
					Database.ExecuteNonQuery(command);
				}
			}

			for (int i = 0; i < _listTableAndColumns.Count; i += 2)
			{
				string tableName = _listTableAndColumns[i];
				string columnName = _listTableAndColumns[i + 1];
				command = "UPDATE " + tableName + " "
					+ "SET " + columnName + "=REPLACE(" + columnName + ",CHAR(0),'') "
					+ "WHERE " + columnName + " LIKE '%" + POut.String("\0") + "%'";
				Database.ExecuteNonQuery(command);
			}

			return;
		}

		///<summary>Replaces null strings with empty strings and returns the number of rows changed.</summary>
		public static long MySqlRemoveNullStrings()
		{
			string command = @"SELECT table_name,column_name 
				FROM information_schema.columns 
				WHERE table_schema=(SELECT DATABASE()) 
				AND (data_type='char' 
					OR data_type='longtext' 
					OR data_type='mediumtext' 
					OR data_type='text' 
					OR data_type='varchar') 
				AND is_nullable='YES'";
			DataTable table = Database.ExecuteDataTable(command);
			long changeCount = 0;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				command = "UPDATE `" + table.Rows[i]["table_name"].ToString() + "` "
					+ "SET `" + table.Rows[i]["column_name"].ToString()
					+ "`='' WHERE `" + table.Rows[i]["column_name"].ToString() + "` IS NULL";
				changeCount += Database.ExecuteNonQuery(command);
			}
			return changeCount;
		}

		///<summary>Makes a backup of the database, clears out etransmessagetext entries over a year old, and then runs optimize on just the etransmessagetext table.  Customers were calling in with the complaint that their etransmessagetext table is too big so we added this tool.</summary>
		public static void ClearOldEtransMessageText()
		{
			//Unlink etrans records from their etransmessagetext records if older than 1 year.
			//We want to keep the 835's around, because they are financial documents which the user may want to reference from the claim edit window later.
			string command = "UPDATE etrans "
				+ "SET EtransMessageTextNum=0 "
				+ "WHERE DATE(DateTimeTrans)<ADDDATE(CURDATE(),INTERVAL -1 YEAR) AND Etype!=" + POut.Long((int)EtransType.ERA_835);
			Database.ExecuteNonQuery(command);
			//Create a temporary table to hold all of the EtransMessageTextNum foreign keys which are sill in use within etrans.  The temporary table speeds up the next query.
			string tableName = "tempetransnomessage" + MiscUtils.CreateRandomAlphaNumericString(8);//max size for a table name in oracle is 30 chars.
			command = "DROP TABLE IF EXISTS " + tableName + "; "
				+ "CREATE TABLE " + tableName + " "
				+ "SELECT DISTINCT EtransMessageTextNum FROM etrans WHERE EtransMessageTextNum!=0; "
				+ "ALTER TABLE " + tableName + " ADD INDEX (EtransMessageTextNum);";
			Database.ExecuteNonQuery(command);
			//Delete unlinked etransmessagetext entries.  Remember, multiple etrans records might point to a single etransmessagetext record.  Therefore, we must keep a particular etransmessagetext record if at least one etrans record needs it.
			command = "DELETE FROM etransmessagetext "
				+ "WHERE EtransMessageTextNum NOT IN (SELECT EtransMessageTextNum FROM " + tableName + ");";
			Database.ExecuteNonQuery(command);
			//Remove the temporary table which is no longer needed.
			command = "DROP TABLE " + tableName + ";";
			Database.ExecuteNonQuery(command);
			//To reclaim that space on the disk you have to do an Optimize.
			//The reasons listed at [[Database Storage Engine Comparison: InnoDB vs MyISAM]] do not apply to these two tables.
			//We just did a massive delete and therefore optimzing a table which is not an "insert only table".
			//Optimizing etrans and etransmessagetext helped a large customer improve speeds by 100x.  They are using innodb tables.
			OptimizeTable("etransmessagetext", canOptimizeInnodb: true);
			OptimizeTable("etrans", canOptimizeInnodb: true);
		}

		///<summary>Used to estimate the time that CreateMissingActiveTPs will take to run.</summary>
		public static List<Procedure> GetProcsNoActiveTp()
		{
			//pats with TP'd procs and no active treatplan OR pats with TPi'd procs that are attached to a sched or planned appt and no active treatplan
			string command = "SELECT * FROM procedurelog WHERE (ProcStatus=" + (int)ProcStat.TP + " "//TP proc exists
				+ "OR (ProcStatus=" + (int)ProcStat.TPi + " AND (AptNum>0 OR PlannedAptNum>0))) "//TPi proc exists that is attached to a sched or planned appt
				+ "AND PatNum NOT IN(SELECT PatNum FROM treatplan WHERE TPStatus=" + (int)TreatPlanStatus.Active + ")";//no active treatplan
			return Crud.ProcedureCrud.SelectMany(command);
		}

		public static string CreateMissingActiveTPs(List<Procedure> listTpTpiProcs)
		{
			if (listTpTpiProcs.Count == 0)
			{//should never happen, won't get called if the list is empty, but just in case
				return "";
			}
			listTpTpiProcs = listTpTpiProcs.OrderBy(x => x.PatNum).ToList();//code below relies of patients being grouped.
																			//listTpTpiProcs.Sort((x,y) => { return x.PatNum.CompareTo(y.PatNum); });//possibly more efficient
			TreatPlan activePlan = null;
			long patNumCur = 0;
			//listProcsNoTp is ordered by PatNum, so each time we find a new PatNum we will create a new active plan and attach procs to it
			//until we find the next PatNum
			foreach (Procedure procCur in listTpTpiProcs)
			{
				if (procCur.PatNum != patNumCur)
				{//new patient, create active plan
					activePlan = new TreatPlan
					{//create active plan, all patients in listPatNumsNoTp do not have an active plan
						Heading = "Active Treatment Plan",
						Note = Preferences.GetString(PreferenceName.TreatmentPlanNote),
						TPStatus = TreatPlanStatus.Active,
						PatNum = procCur.PatNum,
						//UserNumPresenter=userNum,
						//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
						SecUserNumEntry = Security.CurrentUser.Id,
						TPType = (Patients.GetPat(procCur.PatNum).DiscountPlanNum == 0 ? TreatPlanType.Insurance : TreatPlanType.Discount)
					};
					activePlan.TreatPlanNum = TreatPlans.Insert(activePlan);
					patNumCur = procCur.PatNum;
				}
				TreatPlanAttaches.Insert(new TreatPlanAttach { ProcNum = procCur.ProcNum, TreatPlanNum = activePlan.TreatPlanNum, Priority = procCur.Priority });
			}
			return "Patients with active treatment plans created: " + listTpTpiProcs.Select(x => x.PatNum).Distinct().ToList().Count;
		}

		///<summary>This method is designed to help save hard drive space due to the RawEmailIn column containing Base64 attachments. This method
		///should be run on a separate thread and this thread should be passed in to update the progress window.</summary>
		public static string CleanUpRawEmails()
		{
			//Get all clear text emailmessages that can have their RawEmailIn columns safely manipulated from the inbox.
			//These emails are safe to remove attachments from the RawEmailIn because they have already been digested and attchments extracted.
			string command = "SELECT EmailMessageNum FROM emailmessage "
				+ "WHERE RawEmailIn!='' "
				+ "AND SentOrReceived IN (" + POut.Int((int)EmailSentOrReceived.Received) + "," + POut.Int((int)EmailSentOrReceived.Read) + ")";
			//POut.Int((int)EmailSentOrReceived.ReceivedDirect)+","+POut.Int((int)EmailSentOrReceived.ReadDirect)
			//We might need to include encrypted emails in the future if the email table is large due to encrypted emails.
			//Currently not including encrypted emails because the computer running this tool would need the private key to decrypt the message and
			//we would need to take an extra step at the end (after cleaning up attachments) to re-encrypt the modified email message. 
			//The current customers complaining only have bloat with clear text emails so that is where we are going to start with the clean up tool.
			DatabaseMaintEvent.Fire(EventCategory.DatabaseMaint, "Getting email messages from the database...");
			DataTable tableEmailMessageNums = Database.ExecuteDataTable(command);
			if (tableEmailMessageNums.Rows.Count == 0)
			{
				return "There are no email messages that need to be cleaned up.";
			}
			List<EmailAddress> listEmailAddresses = EmailAddresses.GetAll().ToList();//Do not use the cache because the cache doesn't contain all email addresses.
			int noChangeCount = 0;
			int errorCount = 0;
			int cleanedCount = 0;
			int index = 1;
			//Call the processing email logic for each email which will clear out the RawEmailIn column if the email is successfully digested.
			foreach (DataRow row in tableEmailMessageNums.Rows)
			{
				DatabaseMaintEvent.Fire(EventCategory.DatabaseMaint, "Processing email message"
					+ "  " + index.ToString() + " / " + tableEmailMessageNums.Rows.Count.ToString());
				index++;
				EmailMessage emailMessage = EmailMessages.GetOne(PIn.Long(row["EmailMessageNum"].ToString()));
				EmailMessage oldEmailMessage = emailMessage.Copy();
				//Try and find the corresponding email address for this email.
				EmailAddress emailAddress = listEmailAddresses.FirstOrDefault(x => x.SmtpUsername.ToLower() == emailMessage.RecipientAddress.ToLower());
				if (emailAddress == null)
				{
					errorCount++;
					continue;
				}
				try
				{
					EmailMessage emailMessageNew = EmailMessages.ProcessRawEmailMessageIn(emailMessage.RawEmailIn, emailMessage.EmailMessageNum
						, emailAddress, false, oldEmailMessage.SentOrReceived);
					if (Crud.EmailMessageCrud.UpdateComparison(emailMessageNew, oldEmailMessage))
					{
						cleanedCount++;
					}
					else
					{//No changes.
						noChangeCount++;
					}
				}
				catch (Exception)
				{
					//Nothing to do, don't worry about it.
					errorCount++;
				}
			}
			if (tableEmailMessageNums.Rows.Count != noChangeCount)
			{//Using MySQL and something actually changed.
				DatabaseMaintEvent.Fire(EventCategory.DatabaseMaint, "Optimizing the email message table...");
				OptimizeTable("emailmessage");
			}
			string strResults = "Done.  No clean up required.";
			if (cleanedCount > 0 || errorCount > 0)
			{
				strResults = "Total email messages considered: " + tableEmailMessageNums.Rows.Count.ToString() + "\r\n"
					+ "Email messages successfully cleaned up: " + cleanedCount.ToString() + "\r\n"
					+ "Email messages that did not nead to be cleaned up: " + noChangeCount.ToString() + "\r\n"
					+ "Email messages that failed to be cleaned up: " + errorCount.ToString();
			}
			return strResults;
		}

		///<summary>This method will move any email attachment files from the root EmailAttachments directory that start with 'In_' or 'Out_'
		///and will move them into their corresponding In or Out sub directories.</summary>
		public static string CleanUpAttachmentsRootDirectiory()
		{
			StringBuilder strBuildResult = new StringBuilder();
			try
			{
				char separator = Path.DirectorySeparatorChar;
				string attachPath = EmailAttaches.GetAttachPath();
				List<string> listFileNames = FileAtoZ.GetFilesInDirectory(attachPath);
				strBuildResult.AppendLine($"Total files in folder '{attachPath}': {listFileNames.Count}");
				List<string> listInFileNames = listFileNames.FindAll(x => x.Split(separator).Last().StartsWith("In_"));
				strBuildResult.AppendLine($"Total files starting with 'In_': {listInFileNames.Count}");
				List<string> listOutFileNames = listFileNames.FindAll(x => x.Split(separator).Last().StartsWith("Out_"));
				strBuildResult.AppendLine($"Total files starting with 'Out_': {listOutFileNames.Count}");
				if (!FileAtoZ.DirectoryExists(FileAtoZ.CombinePaths(attachPath, "In")))
				{
					FileAtoZ.CreateDirectory(FileAtoZ.CombinePaths(attachPath, "In"));
				}
				if (!FileAtoZ.DirectoryExists(FileAtoZ.CombinePaths(attachPath, "Out")))
				{
					FileAtoZ.CreateDirectory(FileAtoZ.CombinePaths(attachPath, "Out"));
				}
				int countMoved = 0;
				int countErrors = 0;
				foreach (string fileName in listInFileNames)
				{
					try
					{
						string fileNameNew = fileName.Replace($"{separator}In_", $"{separator}In{separator}");
						FileAtoZ.Move(fileName, fileNameNew);
						countMoved++;
					}
					catch (Exception ex)
					{
						strBuildResult.AppendLine($"  Error moving {fileName}: {ex.Message}");
						countErrors++;
					}
				}
				foreach (string fileName in listOutFileNames)
				{
					try
					{
						string fileNameNew = fileName.Replace($"{separator}Out_", $"{separator}Out{separator}");
						FileAtoZ.Move(fileName, fileNameNew);
						countMoved++;
					}
					catch (Exception ex)
					{
						strBuildResult.AppendLine($"  Error moving {fileName}: {ex.Message}");
						countErrors++;
					}
				}
				strBuildResult.AppendLine($"Total files successfully moved: {countMoved}");
				if (countErrors > 0)
				{
					strBuildResult.AppendLine($"Total errors: {countErrors}");
					strBuildResult.AppendLine($"    Please fix the above errors and try again or call support.");
				}
			}
			catch (Exception ex)
			{
				strBuildResult.Append($"There was an error cleaning up email attachments:\r\n{ex.Message}");
			}
			return strBuildResult.ToString().Trim();
		}

		///<summary>Similar to InsPlans.ComputeEstimatesForPatNums(...)</summary>
		public static void RecalcEstimates(List<Procedure> listProcs)
		{
			List<long> listPatNums = listProcs.Select(x => x.PatNum).Distinct().ToList();
			//No need to check RemotingRole; no call to db.
			long patNum = 0;
			for (int i = 0; i < listPatNums.Count; i++)
			{
				patNum = listPatNums[i];
				Family fam = Patients.GetFamily(patNum);
				Patient pat = fam.GetPatient(patNum);
				//Only grab the procedures that have not been completed yet.
				List<Procedure> listNonCompletedProcs = listProcs.FindAll(x => x.PatNum == patNum);
				List<ClaimProc> listClaimProcs = ClaimProcs.GetForProcs(listNonCompletedProcs.Select(x => x.ProcNum).ToList());
				//Only use the claim procs associated to the non-completed procedures.
				List<ClaimProc> listNonCompletedClaimProcs = listClaimProcs.FindAll(x => listNonCompletedProcs.Exists(y => y.ProcNum == x.ProcNum));
				List<InsSub> listSubs = InsSubs.RefreshForFam(fam);
				List<InsurancePlan> listPlans = InsPlans.RefreshForSubList(listSubs);
				List<PatPlan> listPatPlans = PatPlans.Refresh(patNum);
				List<Benefit> listBenefits = Benefits.Refresh(listPatPlans, listSubs);
				Procedures.ComputeEstimatesForAll(patNum, listNonCompletedClaimProcs, listNonCompletedProcs, listPlans, listPatPlans, listBenefits, pat.Age, listSubs, null, true);
				Patients.SetHasIns(patNum);
			}
		}

		///<summary>Detaches all patient payments attached to insurance payment plans and all insurance payments attached to patient payment plans.
		///Returns a description of the changes that were made so that the user can go make manual changes if necessary.</summary>
		public static string DetachInvalidPaymentPlanPayments()
		{
			string resultsMsg = "";
			DataTable table = GetPaySplitsAttachedToInsurancePaymentPlan();
			if (table.Rows.Count > 0)
			{
				string command = "UPDATE paysplit SET PayPlanNum=0 WHERE SplitNum IN("
					+ string.Join(",", table.Select().Select(x => x["SplitNum"].ToString())) + ")";
				Database.ExecuteNonQuery(command);

				resultsMsg +=
					"The following patient payments were detached from insurance payment plans. " +
					"It is recommended you verify these accounts are correct.";

				for (int i = 0; i < table.Rows.Count; i++)
				{
					resultsMsg += "\r\n   Patient # " + table.Rows[i]["PatNum"].ToString() + " "
						+ "had a payment amount for " + PIn.Double(table.Rows[i]["SplitAmt"].ToString()).ToString("c") + " "
						+ "on date " + PIn.Date(table.Rows[i]["DatePay"].ToString()).ToShortDateString() + " "
						+ "attached to insurance payment plan #" + table.Rows[i]["PayPlanNum"];
				}
			}
			table = GetClaimProcsAttachedToPatientPaymentPlans();
			if (table.Rows.Count > 0)
			{
				string command = "UPDATE claimproc SET PayPlanNum=0 WHERE ClaimProcNum IN("
					+ string.Join(",", table.Select().Select(x => x["ClaimProcNum"].ToString())) + ")";
				Database.ExecuteNonQuery(command);
				if (resultsMsg != "")
				{
					resultsMsg += "\r\n\r\n";
				}
				resultsMsg +=
					"The following insurance payments were detached from patient payment plans. " +
					"It is recommended you verify these accounts are correct.";
				for (int i = 0; i < table.Rows.Count; i++)
				{
					resultsMsg += "\r\n   Patient #" + table.Rows[i]["PatNum"].ToString() + " "
						+ "had a payment amount for " + PIn.Double(table.Rows[i]["InsPayAmt"].ToString()).ToString("c") + " "
						+ "on date " + PIn.Date(table.Rows[i]["DateCP"].ToString()).ToShortDateString() + " "
						+ "attached to patient payment plan #" + table.Rows[i]["PayPlanNum"];
				}
			}
			if (resultsMsg == "")
			{
				resultsMsg += "No payments found that needed to be detached from payment plans.";
			}
			return resultsMsg;
		}

		///<summary>Gets the DataTable that contains paysplits attached to insurance payment plans.
		///Table will contain the following columns; SplitNum, PatNum, SplitAmt, DatePay, PayPlanNum</summary>
		private static DataTable GetPaySplitsAttachedToInsurancePaymentPlan()
		{
			//Need to check remoting role before calling; private method
			string command = "SELECT paysplit.SplitNum,paysplit.PatNum,paysplit.SplitAmt,paysplit.DatePay,paysplit.PayPlanNum FROM paysplit "
				+ "INNER JOIN payplan ON payplan.PayPlanNum=paysplit.PayPlanNum "
				+ "WHERE paysplit.PayPlanNum!=0 "
				+ "AND payplan.PlanNum!=0 ";//insurance payment plan
			return Database.ExecuteDataTable(command);
		}

		///<summary>Gets claim procs that are attached to patient payment plans.
		///Table will contain the following columns; ClaimProcNum, PatNum, InsPayAmt, DateCP, PayPlanNum</summary>
		private static DataTable GetClaimProcsAttachedToPatientPaymentPlans()
		{
			//Need to check remoting role before calling; private method
			string command = "SELECT claimproc.ClaimProcNum,claimproc.PatNum,claimproc.InsPayAmt,claimproc.DateCP,claimproc.PayPlanNum FROM claimproc "
				+ "INNER JOIN payplan ON payplan.PayPlanNum=claimproc.PayPlanNum "
				+ "WHERE claimproc.PayPlanNum!=0 "
				+ "AND payplan.PlanNum=0 ";//standard payment plan
			return Database.ExecuteDataTable(command);
		}

		///<summary>Given a patnum and the name of a table this helper builds the MySQL AND clause string used for our patient specific DBMs. 
		///Currently only works if the column name on the table is "PatNum" and will return an empty string if the PatNum is less than 1.</summary>
		private static string PatientAndClauseHelper(long patNum, string tableName)
		{
			//Not running patient specific DBM or a table wasn't specified.
			if (patNum < 1 || string.IsNullOrWhiteSpace(tableName))
			{
				return "";
			}
			return $" AND {tableName}.PatNum={POut.Long(patNum)} ";
		}

		public static DataTable GetRedundantIndexesTable()
		{
			string dbName = MiscData.GetCurrentDatabase();
			string command = $@"SELECT table1.TABLE_NAME,
				REPLACE(
					CASE WHEN table1.COLS=table2.COLS AND table1.NON_UNIQUE=table2.NON_UNIQUE
						THEN
							CASE WHEN INSTR(REPLACE(table2.INDEX_NAME,'`',''),REPLACE(table1.INDEX_NAME,'`',''))=1
								THEN table2.INDEX_NAME
							WHEN INSTR(REPLACE(table1.INDEX_NAME,'`',''),REPLACE(table2.INDEX_NAME,'`',''))=1
								THEN table1.INDEX_NAME
							ELSE GREATEST(table1.INDEX_NAME,table2.INDEX_NAME)
							END
					WHEN LENGTH(table1.COLS)-LENGTH(REPLACE(table1.COLS,',',''))>LENGTH(table2.COLS)-LENGTH(REPLACE(table2.COLS,',',''))
						THEN table2.INDEX_NAME
					ELSE table1.INDEX_NAME
					END
				,'`','') INDEX_NAME,
				REPLACE(
					CASE WHEN table1.COLS=table2.COLS AND table1.NON_UNIQUE=table2.NON_UNIQUE
						THEN
							CASE WHEN INSTR(REPLACE(table2.INDEX_NAME,'`',''),REPLACE(table1.INDEX_NAME,'`',''))=1
								THEN CONCAT(table2.COLS,IFNULL(CONCAT('(',table2.SUB_PART,')'),''))
							WHEN INSTR(REPLACE(table1.INDEX_NAME,'`',''),REPLACE(table2.INDEX_NAME,'`',''))=1
								THEN CONCAT(table1.COLS,IFNULL(CONCAT('(',table1.SUB_PART,')'),''))
							ELSE
								CASE WHEN table1.INDEX_NAME>table2.INDEX_NAME
									THEN CONCAT(table1.COLS,IFNULL(CONCAT('(',table1.SUB_PART,')'),''))
								ELSE CONCAT(table2.COLS,IFNULL(CONCAT('(',table2.SUB_PART,')'),''))
								END
							END
					WHEN LENGTH(table1.COLS)-LENGTH(REPLACE(table1.COLS,',',''))>LENGTH(table2.COLS)-LENGTH(REPLACE(table2.COLS,',',''))
						THEN CONCAT(table2.COLS,IFNULL(CONCAT('(',table2.SUB_PART,')'),''))
					ELSE CONCAT(table1.COLS,IFNULL(CONCAT('(',table1.SUB_PART,')'),''))
					END
				,'`','') INDEX_COLS,
				REPLACE(
					GROUP_CONCAT(
						DISTINCT CASE WHEN table1.COLS=table2.COLS AND table1.NON_UNIQUE=table2.NON_UNIQUE
							THEN
								CASE WHEN INSTR(REPLACE(table2.INDEX_NAME,'`',''),REPLACE(table1.INDEX_NAME,'`',''))=1
									THEN CONCAT(table1.INDEX_NAME,' (',table1.COLS,')')
								WHEN INSTR(REPLACE(table1.INDEX_NAME,'`',''),REPLACE(table2.INDEX_NAME,'`',''))=1
									THEN CONCAT(table2.INDEX_NAME,' (',table2.COLS,')')
								ELSE
									CASE WHEN table1.INDEX_NAME<table2.INDEX_NAME
										THEN CONCAT(table1.INDEX_NAME,' (',table1.COLS,')')
									ELSE CONCAT(table2.INDEX_NAME,' (',table2.COLS,')')
									END
								END
						WHEN LENGTH(table1.COLS)-LENGTH(REPLACE(table1.COLS,',',''))>LENGTH(table2.COLS)-LENGTH(REPLACE(table2.COLS,',',''))
							THEN CONCAT(table1.INDEX_NAME,' (',table1.COLS,')')
						ELSE CONCAT(table2.INDEX_NAME,' (',table2.COLS,')')
						END
						SEPARATOR '\r\n'
					)
				,'`','') REDUNDANT_OF,
				CASE WHEN table1.COLS=table2.COLS AND table1.NON_UNIQUE=table2.NON_UNIQUE
					THEN
						CASE WHEN INSTR(REPLACE(table2.INDEX_NAME,'`',''),REPLACE(table1.INDEX_NAME,'`',''))=1
							THEN table2.ENGINE
						WHEN INSTR(REPLACE(table1.INDEX_NAME,'`',''),REPLACE(table2.INDEX_NAME,'`',''))=1
							THEN table1.ENGINE
						ELSE
							CASE WHEN table1.INDEX_NAME>table2.INDEX_NAME
								THEN table1.ENGINE
							ELSE table2.ENGINE
							END
						END
				WHEN LENGTH(table1.COLS)-LENGTH(REPLACE(table1.COLS,',',''))>LENGTH(table2.COLS)-LENGTH(REPLACE(table2.COLS,',',''))
					THEN table2.ENGINE
				ELSE table1.ENGINE
				END `ENGINE`
				FROM (
					SELECT s.TABLE_NAME,CONCAT('`',s.INDEX_NAME,'`') AS INDEX_NAME,s.INDEX_TYPE,s.NON_UNIQUE,s.SUB_PART,t.ENGINE,
					GROUP_CONCAT(CONCAT('`',s.COLUMN_NAME,'`') ORDER BY IF(s.INDEX_TYPE='BTREE',s.SEQ_IN_INDEX,0),s.COLUMN_NAME) COLS
					FROM information_schema.STATISTICS s
					INNER JOIN information_schema.TABLES t ON t.TABLE_SCHEMA=s.TABLE_SCHEMA
						AND t.TABLE_NAME=s.TABLE_NAME
					WHERE s.TABLE_SCHEMA='{POut.String(dbName)}'
					GROUP BY s.TABLE_NAME,s.INDEX_NAME,s.INDEX_TYPE,s.NON_UNIQUE
				) table1
				INNER JOIN (
					SELECT s.TABLE_NAME,CONCAT('`',s.INDEX_NAME,'`') AS INDEX_NAME,s.INDEX_TYPE,s.NON_UNIQUE,s.SUB_PART,t.ENGINE,
					GROUP_CONCAT(CONCAT('`',s.COLUMN_NAME,'`') ORDER BY IF(s.INDEX_TYPE='BTREE',s.SEQ_IN_INDEX,0),s.COLUMN_NAME) COLS
					FROM information_schema.STATISTICS s
					INNER JOIN information_schema.TABLES t ON t.TABLE_SCHEMA=s.TABLE_SCHEMA
						AND t.TABLE_NAME=s.TABLE_NAME
					WHERE s.TABLE_SCHEMA='{POut.String(dbName)}'
					GROUP BY s.TABLE_NAME,s.INDEX_NAME,s.INDEX_TYPE,s.NON_UNIQUE
				) AS table2
				WHERE table2.TABLE_NAME=table1.TABLE_NAME
				AND table2.INDEX_NAME!=table1.INDEX_NAME
				AND table2.INDEX_TYPE=table1.INDEX_TYPE
				AND (
					(
						table2.COLS=table1.COLS
						AND (
							table1.NON_UNIQUE
							OR table1.NON_UNIQUE=table2.NON_UNIQUE
						)
					)
					OR (
						table1.INDEX_TYPE='BTREE'
						AND INSTR(table2.COLS,table1.COLS)=1
						AND table1.NON_UNIQUE
					)
				)
				GROUP BY table1.TABLE_NAME,INDEX_NAME";
			return Database.ExecuteDataTable(command);
		}

		public static string DropRedundantIndexes(List<DataRow> listRows)
		{
			bool hasInnoDbFilePerTable = false;
			using (DataTable table = Database.ExecuteDataTable("SHOW GLOBAL VARIABLES LIKE 'INNODB_FILE_PER_TABLE'"))
			{
				if (table.Rows.Count > 0 && table.Columns.Count > 1)
				{
					hasInnoDbFilePerTable = PIn.Bool(table.Rows[0][1].ToString());
				}
			}
			StringBuilder sbLog = new StringBuilder();
			string dbName = MiscData.GetCurrentDatabase();
			DataConnection.CommandTimeout = 43200;//12 hours, just in case
			listRows.GroupBy(x => PIn.String(x["TABLE_NAME"].ToString()))
				.ToDictionary(x => x.Key, x => x.ToList())
				.Where(x => x.Value.Count > 0)
				.ForEach(x =>
				{
					string fullTableName = $@"`{POut.String(dbName)}`.`{POut.String(x.Key)}`";
					sbLog.AppendLine($@"ALTER TABLE {fullTableName} {string.Join(", ",
						x.Value.Select(y => $@"ADD INDEX {POut.String(y["INDEX_NAME"].ToString())} ({POut.String(y["INDEX_COLS"].ToString())})"))};");
					string command = $@"ALTER TABLE {fullTableName} {string.Join(", ", x.Value.Select(y => $@"DROP INDEX {POut.String(y["INDEX_NAME"].ToString())}"))};";
					//The ENGINE column should be the same for all rows in the list, since it's the table's storage engine. Using .Any just in case.
					bool doOptimize = x.Value.Any(y => PIn.String(y["ENGINE"].ToString()).ToLower() == "innodb") && hasInnoDbFilePerTable;
					if (doOptimize)
					{
						//For InnoDb tables with innodb_file_per_table set, optimize table to reclaim hard drive space and reduce .ibd file size
						command += $@"
							OPTIMIZE TABLE {fullTableName};";
					}
					DatabaseMaintEvent.Fire(EventCategory.DatabaseMaint, "Dropping redundant indexes "
						+ (doOptimize ? "and optimizing " : "") + "table" + " "
						+ fullTableName.Replace("`", "") + ".");
					Database.ExecuteNonQuery(command);
				});
			DataConnection.CommandTimeout = 3600;//set back to 1 hour default
			return sbLog.ToString();
		}

		#endregion Tool Button and Helper Methods-----------------------------------------------------------------------------------------------------------

		///<summary>Uses reflection to get all database maintenance methods that are specifically flagged for DBM.
		///When clinicNum is set to a medical clinic, all methods that match "tooth" will not be returned.</summary>
		public static List<MethodInfo> GetMethodsForDisplay(long clinicNum = 0, bool hasOnlyPatNumMethods = false)
		{
			//No need to check RemotingRole; no call to db.
			List<MethodInfo> listDbmMethodsGrid = new List<MethodInfo>();
			//Workstations region settings.
			string country = CultureInfo.CurrentCulture.Name;
			//Grab all methods from the DatabaseMaintenance class to dynamically fill the grid.
			MethodInfo[] arrayDbmMethodsAll = (typeof(DatabaseMaintenances)).GetMethods();
			//Sort the methods by name so that they are easier for users to find desired methods to run.
			Array.Sort(arrayDbmMethodsAll, new MethodInfoComparer());
			bool isMedicalClinic = Clinics.IsMedicalClinic(clinicNum);
			foreach (MethodInfo meth in arrayDbmMethodsAll)
			{
				DbmMethodAttr dbmAttribute = (DbmMethodAttr)Attribute.GetCustomAttribute(meth, typeof(DbmMethodAttr));
				if (dbmAttribute == null)
				{
					continue;//This is not a valid DBM method.
				}
				if (!country.EndsWith("CA") && dbmAttribute.IsCanada)
				{//Skip over Canada dbm's if not in Canada.
					continue;
				}
				if (isMedicalClinic && Regex.IsMatch(meth.Name, "tooth", RegexOptions.IgnoreCase))
				{
					continue;//This is not a DBM for medical users.
				}
				if (hasOnlyPatNumMethods && !dbmAttribute.HasPatNum)
				{
					continue;//This is not a patient specific DBM method.
				}
				//This is a valid DBM method and should be added to the list of methods to display to the user.
				listDbmMethodsGrid.Add(meth);
			}
			return listDbmMethodsGrid;
		}

		///<summary>Returns true if the method passed in supports break down.</summary>
		public static bool MethodHasBreakDown(MethodInfo method)
		{
			//No need to check RemotingRole; no call to db.
			return method.GetCustomAttributes(typeof(DbmMethodAttr), true).OfType<DbmMethodAttr>().All(x => x.HasBreakDown);
		}
	}
}
