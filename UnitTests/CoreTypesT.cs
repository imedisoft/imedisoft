﻿using DataConnectionBase;
using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Data;
using System.Text;

namespace UnitTests
{
    public class CoreTypesT
	{
		public static string CreateTempTable(string serverAddr, string port, string userName, string password)
		{
			string retVal = "";
			UnitTestsCore.DatabaseTools.SetDbConnection(TestBase.UnitTestDbName, serverAddr, port, userName, password);
			string command;

			command = "DROP TABLE IF EXISTS tempcore";
			Database.ExecuteNonQuery(command);
			command = @"CREATE TABLE tempcore (
					TempCoreNum bigint NOT NULL,
					TimeOfDayTest time NOT NULL DEFAULT '00:00:00',
					TimeStampTest timestamp,
					DateTest date NOT NULL DEFAULT '0001-01-01',
					DateTimeTest datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
					TimeSpanTest time NOT NULL DEFAULT '00:00:00',
					CurrencyTest double NOT NULL,
					BoolTest tinyint NOT NULL,
					TextTinyTest varchar(255) NOT NULL, 
					TextSmallTest text NOT NULL,
					TextMediumTest text NOT NULL,
					TextLargeTest mediumtext NOT NULL,
					VarCharTest varchar(255) NOT NULL
					) DEFAULT CHARSET=utf8";
			Database.ExecuteNonQuery(command);


			command = "DROP TABLE IF EXISTS tempgroupconcat";
			Database.ExecuteNonQuery(command);
			command = @"CREATE TABLE tempgroupconcat (
					Names varchar(255)
					) DEFAULT CHARSET=utf8";
			Database.ExecuteNonQuery(command);


			retVal += "Temp tables created.\r\n";
			//retVal+="Temp tables cannot yet be created.\r\n";
			return retVal;
		}

		/// <summary></summary>
		public static string RunAll()
		{
			string retVal = "";
            DataTable table;
			TimeSpan timespan;
			TimeSpan timespan2;
			string varchar1;
			string varchar2;
			//timespan(timeOfDay)----------------------------------------------------------------------------------------------
			timespan = new TimeSpan(1, 2, 3);//1hr,2min,3sec
                                             //Things that we might later add to this series of tests:
                                             //Special column types such as timestamp
                                             //Computer set to other region, affecting string parsing of types such dates and decimals
                                             //Test types without casting back and forth to strings.
                                             //Retrieval using a variety of techniques, such as getting a table, scalar, and reading a row.
                                             //Blobs
            string command = "INSERT INTO tempcore (TimeOfDayTest) VALUES (" + SOut.Time(timespan) + ")";
            Database.ExecuteNonQuery(command);
			command = "SELECT TimeOfDayTest FROM tempcore";
			table = Database.ExecuteDataTable(command);
			timespan2 = PIn.Time(table.Rows[0]["TimeOfDayTest"].ToString());
			if (timespan != timespan2)
			{
				throw new Exception();
			}
			command = "DELETE FROM tempcore";
			Database.ExecuteNonQuery(command);
			retVal += "TimeSpan (time of day): Passed.\r\n";
			//timespan, negative------------------------------------------------------------------------------------
			timespan = new TimeSpan(0, -36, 0);//This particular timespan value was found to fail in mysql with the old connector.
											   //Don't know what's so special about this one value.  There are probably other values failing as well, but it doesn't matter.
											   //Oracle does not seem to like negative values.
			command = "INSERT INTO tempcore (TimeSpanTest) VALUES ('" + POut.TSpan(timespan) + "')";
			Database.ExecuteNonQuery(command);
			command = "SELECT TimeSpanTest FROM tempcore";
			table = Database.ExecuteDataTable(command);
			string tempVal = table.Rows[0]["TimeSpanTest"].ToString();
			timespan2 = SIn.Time(tempVal);
			if (timespan != timespan2)
			{
				throw new Exception();
			}
			command = "DELETE FROM tempcore";
			Database.ExecuteNonQuery(command);
			retVal += "TimeSpan, negative: Passed.\r\n";
			//timespan, over 24 hours-----------------------------------------------------------------------------
			timespan = new TimeSpan(432, 5, 17);
			command = "INSERT INTO tempcore (TimeSpanTest) VALUES ('" + POut.TSpan(timespan) + "')";
			Database.ExecuteNonQuery(command);
			command = "SELECT TimeSpanTest FROM tempcore";
			table = Database.ExecuteDataTable(command);
			timespan2 = SIn.Time(table.Rows[0]["TimeSpanTest"].ToString());
			if (timespan != timespan2)
			{
				throw new Exception();
			}
			command = "DELETE FROM tempcore";
			Database.ExecuteNonQuery(command);
			retVal += "TimeSpan, large: Passed.\r\n";
			//date----------------------------------------------------------------------------------------------
			DateTime date1;
			DateTime date2;
			date1 = new DateTime(2003, 5, 23);
			command = "INSERT INTO tempcore (DateTest) VALUES (" + POut.Date(date1) + ")";
			Database.ExecuteNonQuery(command);
			command = "SELECT DateTest FROM tempcore";
			table = Database.ExecuteDataTable(command);
			date2 = SIn.Date(table.Rows[0]["DateTest"].ToString());
			if (date1 != date2)
			{
				throw new Exception();
			}
			command = "DELETE FROM tempcore";
			Database.ExecuteNonQuery(command);
			retVal += "Date: Passed.\r\n";
			//datetime------------------------------------------------------------------------------------------
			DateTime datet1;
			DateTime datet2;
			datet1 = new DateTime(2003, 5, 23, 10, 18, 0);
			command = "INSERT INTO tempcore (DateTimeTest) VALUES (" + POut.DateT(datet1) + ")";
			Database.ExecuteNonQuery(command);
			command = "SELECT DateTimeTest FROM tempcore";
			table = Database.ExecuteDataTable(command);
			datet2 = SIn.Date(table.Rows[0]["DateTimeTest"].ToString());
			if (datet1 != datet2)
			{
				throw new Exception();
			}
			command = "DELETE FROM tempcore";
			Database.ExecuteNonQuery(command);
			retVal += "Date/Time: Passed.\r\n";
			//currency------------------------------------------------------------------------------------------
			double double1;
			double double2;
			double1 = 12.34d;
			command = "INSERT INTO tempcore (CurrencyTest) VALUES (" + POut.Double(double1) + ")";
			Database.ExecuteNonQuery(command);
			command = "SELECT CurrencyTest FROM tempcore";
			table = Database.ExecuteDataTable(command);
			double2 = SIn.Double(table.Rows[0]["CurrencyTest"].ToString());
			if (double1 != double2)
			{
				throw new Exception();
			}
			command = "DELETE FROM tempcore";
			Database.ExecuteNonQuery(command);
			retVal += "Currency: Passed.\r\n";
			//group_concat------------------------------------------------------------------------------------
			command = "INSERT INTO tempgroupconcat VALUES ('name1')";
			Database.ExecuteNonQuery(command);
			command = "INSERT INTO tempgroupconcat VALUES ('name2')";
			Database.ExecuteNonQuery(command);
			command = "SELECT " + DbHelper.GroupConcat("Names") + " allnames FROM tempgroupconcat";
			table = Database.ExecuteDataTable(command);
			string allnames = SIn.ByteArray(table.Rows[0]["allnames"].ToString());
			//if(DataConnection.DBtype==DatabaseType.Oracle) {
			//	allnames=allnames.TrimEnd(',');//known issue.  Should already be fixed:
			//Use RTRIM(REPLACE(REPLACE(XMLAgg(XMLElement("x",column_name)),'<x>'),'</x>',','))
			//}
			if (allnames != "name1,name2")
			{
				throw new Exception();
			}
			command = "DELETE FROM tempgroupconcat";
			Database.ExecuteNonQuery(command);
			retVal += "Group_concat: Passed.\r\n";
			//bool,pos------------------------------------------------------------------------------------
			bool bool1;
			bool bool2;
			bool1 = true;
			command = "INSERT INTO tempcore (BoolTest) VALUES (" + POut.Bool(bool1) + ")";
			Database.ExecuteNonQuery(command);
			command = "SELECT BoolTest FROM tempcore";
			table = Database.ExecuteDataTable(command);
			bool2 = SIn.Bool(table.Rows[0]["BoolTest"].ToString());
			if (bool1 != bool2)
			{
				throw new Exception();
			}
			command = "DELETE FROM tempcore";
			Database.ExecuteNonQuery(command);
			retVal += "Bool, true: Passed.\r\n";
			//bool,neg------------------------------------------------------------------------------------
			bool1 = false;
			command = "INSERT INTO tempcore (BoolTest) VALUES (" + POut.Bool(bool1) + ")";
			Database.ExecuteNonQuery(command);
			command = "SELECT BoolTest FROM tempcore";
			table = Database.ExecuteDataTable(command);
			bool2 = SIn.Bool(table.Rows[0]["BoolTest"].ToString());
			if (bool1 != bool2)
			{
				throw new Exception();
			}
			command = "DELETE FROM tempcore";
			Database.ExecuteNonQuery(command);
			retVal += "Bool, false: Passed.\r\n";
			//varchar255 Nonstandard Characters-----------------------------------------------------------
			varchar1 = @"'!@#$%^&*()-+[{]}\`~,<.>/?'"";:=_" + "\r\n\t";
            command = "INSERT INTO tempcore (TextTinyTest) VALUES ('" + SOut.String(varchar1) + "')";
			Database.ExecuteNonQuery(command);
			command = "SELECT TextTinyTest FROM tempcore";
			table = Database.ExecuteDataTable(command);
			varchar2 = SIn.String(table.Rows[0]["TextTinyTest"].ToString());
			if (varchar1 != varchar2)
			{
				throw new Exception();
			}
			command = "DELETE FROM tempcore";
			Database.ExecuteNonQuery(command);
			retVal += "VarChar(255): Passed.\r\n";
			//VARCHAR2(4000)------------------------------------------------------------------------------
			varchar1 = CreateRandomAlphaNumericString(4000); //Tested 4001 and it was too large as expected.
			command = "INSERT INTO tempcore (TextSmallTest) VALUES ('" + SOut.String(varchar1) + "')";
			Database.ExecuteNonQuery(command);
			command = "SELECT TextSmallTest FROM tempcore";
			table = Database.ExecuteDataTable(command);
			varchar2 = SIn.String(table.Rows[0]["TextSmallTest"].ToString());
			if (varchar1 != varchar2)
			{
				throw new Exception();
			}
			command = "DELETE FROM tempcore";
			Database.ExecuteNonQuery(command);
			retVal += "VarChar2(4000): Passed.\r\n";
			////clob:-----------------------------------------------------------------------------------------
			////tested up to 20MB in oracle.  (50MB however was failing: Chunk size error)
			////mysql mediumtext maxes out at about 16MB.
			//string clobstring1 = CreateRandomAlphaNumericString(10485760); //10MB should be larger than anything we store.
   //         OdSqlParameter param = new OdSqlParameter("param1", OdDbType.Text, clobstring1);
			//command = "INSERT INTO tempcore (TextLargeTest) VALUES (" + DbHelper.ParamChar + "param1)";
			//Database.ExecuteNonQuery(command, param);
			//command = "SELECT TextLargeTest FROM tempcore";
			//table = Database.GetTable(command);
   //         string clobstring2 = SIn.String(table.Rows[0]["TextLargeTest"].ToString());
   //         if (clobstring1 != clobstring2)
			//{
			//	throw new Exception();
			//}
			//command = "DELETE FROM tempcore";
			//Database.ExecuteNonQuery(command);
			//retVal += "Clob, Alpha-Numeric 10MB: Passed.\r\n";
			////clob:non-standard----------------------------------------------------------------------------------
			//clobstring1 = CreateRandomNonStandardString(8000000); //8MB is the max because many chars takes 2 bytes, and mysql maxes out at 16MB
   //         param = new OdSqlParameter("param1", OdDbType.Text, clobstring1);
			//command = "INSERT INTO tempcore (TextLargeTest) VALUES (" + DbHelper.ParamChar + "param1)";
			//Database.ExecuteNonQuery(command, param);
			//command = "SELECT TextLargeTest FROM tempcore";
			//table = Database.GetTable(command);
			//clobstring2 = SIn.String(table.Rows[0]["TextLargeTest"].ToString());
			//if (clobstring1 != clobstring2)
			//{
			//	throw new Exception();
			//}
			//command = "DELETE FROM tempcore";
			//Database.ExecuteNonQuery(command);
			//retVal += "Clob, Symbols and Chinese: Passed.\r\n";
			////clob:Rick Roller----------------------------------------------------------------------------------
			//clobstring1 = RickRoller(10485760); //10MB should be larger than anything we store.
   //         param = new OdSqlParameter("param1", OdDbType.Text, clobstring1);
			//command = "INSERT INTO tempcore (TextLargeTest) VALUES (" + DbHelper.ParamChar + "param1)";
			//Database.ExecuteNonQuery(command, param);
			//command = "SELECT TextLargeTest FROM tempcore";
			//table = Database.GetTable(command);
			//clobstring2 = SIn.String(table.Rows[0]["TextLargeTest"].ToString());
			//if (clobstring1 != clobstring2)
			//{
			//	throw new Exception();
			//}
			//command = "DELETE FROM tempcore";
			//Database.ExecuteNonQuery(command);
			//retVal += "Clob, Rick Roller: Passed.\r\n";
			//SHOW CREATE TABLE -----------------------------------------------------------------------
			//This command is needed in order to perform a backup.

			command = "SHOW CREATE TABLE account";
			table = Database.ExecuteDataTable(command);
			string createResult = SIn.ByteArray(table.Rows[0][1]);
			if (!createResult.StartsWith("CREATE TABLE"))
			{
				throw new Exception();
			}
			retVal += "SHOW CREATE TABLE: Passed.\r\n";

			//Single Command Split-------------------------------------------------------------------------
			varchar1 = "';\"";
			varchar2 = ";'';;;;\"\"\"\"'asdfsadsdaf'";
			command = "INSERT INTO tempcore (TextTinyTest,TextSmallTest) VALUES ('" + SOut.String(varchar1) + "','" + SOut.String(varchar2) + "');";
			Database.ExecuteNonQuery(command);//Test the split function.
			command = "SELECT TextTinyTest,TextSmallTest FROM tempcore";
			table = Database.ExecuteDataTable(command);
			if (SIn.String(table.Rows[0]["TextTinyTest"].ToString()) != varchar1 || SIn.String(table.Rows[0]["TextSmallTest"].ToString()) != varchar2)
			{
				throw new ApplicationException();
			}
			command = "DELETE FROM tempcore";
			Database.ExecuteNonQuery(command);
			retVal += "Single Command Split: Passed.";
			//Run multiple non-queries in one transaction--------------------------------------------------
			varchar1 = "A";
			varchar2 = "B";
			command = "INSERT INTO tempcore (TextTinyTest) VALUES ('" + POut.String(varchar1) + "'); DELETE FROM tempcore; INSERT INTO tempcore (TextTinyTest) VALUES ('" + POut.String(varchar2) + "')";
			Database.ExecuteNonQuery(command);
			command = "SELECT TextTinyTest FROM tempcore";
			table = Database.ExecuteDataTable(command);
			if (SIn.String(table.Rows[0][0].ToString()) != varchar2)
			{
				throw new ApplicationException();
			}
			command = "DELETE FROM tempcore";
			Database.ExecuteNonQuery(command);
			retVal += "Multi-Non-Queries: Passed.\r\n";
			//Cleanup---------------------------------------------------------------------------------------

			command = "DROP TABLE IF EXISTS tempcore";
			Database.ExecuteNonQuery(command);
			command = "DROP TABLE IF EXISTS tempgroupconcat";
			Database.ExecuteNonQuery(command);

			retVal += "CoreTypes test done.\r\n";
			return retVal;
		}

		public static string CreateRandomAlphaNumericString(int length)
		{
			StringBuilder result = new StringBuilder(length);
			Random rand = new Random();
			string randChrs = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			for (int i = 0; i < length; i++)
			{
				result.Append(randChrs[rand.Next(0, randChrs.Length - 1)]);
			}
			return result.ToString();
		}

		public static string CreateRandomNonStandardString(int length)
		{
			StringBuilder result = new StringBuilder(length);
			Random rand = new Random();
			string randChrs = "'!@#$%^&*()-+[{]}\\`~,<.>/?'\";:=_是像电子和质子这样的亚原子粒子之间的产生排斥力和吸引";
			for (int i = 0; i < length; i++)
			{
				result.Append(randChrs[rand.Next(0, randChrs.Length - 1)]);
			}
			return result.ToString();
		}

		public static string RickRoller(int length)
		{
			StringBuilder result = new StringBuilder(length);
            _ = new Random();
            string randChrs = "I just couldn't take it anymore.  Kept getting the d--- song stuck in my head.";
			for (int i = 0; i < length; i++)
			{
				result.Append(randChrs[i % randChrs.Length]);
			}
			return result.ToString();
		}
	}
}
