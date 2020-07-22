using Imedisoft.Data;
using OpenDentBusiness;

namespace UnitTestsCore
{
    /// <summary>
    /// Contains the queries, scripts, and tools to clear the database of data from previous unitTest runs.
    /// </summary>
    public class DatabaseTools
	{
		/// <summary>
		/// This is analogous to FormChooseDatabase.TryToConnect.
		/// Empty string is allowed.
		/// </summary>
		public static bool SetDbConnection(string dbName)
		{
			return SetDbConnection(dbName, "localhost", "", "root", "");
		}

		/// <summary>
		/// This function allows connecting to a specific server.
		/// </summary>
		public static bool SetDbConnection(string dbName, string serverAddr, string port, string userName, string password)
		{
			try
			{
				new DataConnection().SetDb(serverAddr, userName, password, dbName);

				return true;

			}
			catch
			{
				return false;
			}
		}

		public static string ClearDb()
		{
			string command = @"
				DELETE FROM alertitem;
				DELETE FROM appointment;
				DELETE FROM apptreminderrule;
				DELETE FROM asapcomm;
				DELETE FROM carrier;
				DELETE FROM claim;
				DELETE FROM claimproc;
				DELETE FROM clinic;
				DELETE FROM clockevent;
				DELETE FROM confirmationrequest;
				DELETE FROM creditcard;
				DELETE FROM eclipboardsheetdef;
				DELETE FROM emailmessage;
				DELETE FROM employee;
				DELETE FROM fee;
				DELETE FROM feesched WHERE FeeSchedNum != 53; /*because this is the default fee schedule for providers*/
				DELETE FROM hl7def;
				DELETE FROM hl7msg;
				DELETE FROM insplan;
				DELETE FROM mobileappdevice;
				DELETE FROM operatory;
				DELETE FROM patient;
				DELETE FROM patientportalinvite;
				DELETE FROM patientrace;
				DELETE FROM patplan;
				DELETE FROM payment;
				DELETE FROM paysplit;
				DELETE FROM payperiod;
				DELETE FROM payplan;
				DELETE FROM payplancharge;
				DELETE FROM pharmacy;
				DELETE FROM procedurelog;
				DELETE FROM provider WHERE ProvNum>2;
				DELETE FROM recall;
				DELETE FROM schedule;
				DELETE FROM sheet;
				DELETE FROM sheetdef;
				DELETE FROM sheetfield;
				DELETE FROM sheetfielddef;
				DELETE FROM smsphone;
				DELETE FROM smstomobile;
				DELETE FROM smsfrommobile;
				DELETE FROM timeadjust;
				DELETE FROM timecardrule;
				DELETE FROM userweb;";

			Database.ExecuteNonQuery(command);
			Providers.RefreshCache();
			FeeScheds.RefreshCache();
			PharmacyT.CreatePharmacies();

			return "Database cleared of old data.\r\n";
		}
	}
}
