using Imedisoft.Data;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Reflection;

namespace UnitTests
{
    ///<summary>Contains the queries, scripts, and tools to clear the database of data from previous unitTest runs.</summary>
    class DatabaseTools {
		public static string FreshFromDump(string serverAddr,string port,string userName,string password) {
			Security.CurrentUser=Security.CurrentUser??new Userod();

				string command="DROP DATABASE IF EXISTS "+TestBase.UnitTestDbName;
				try {
					Database.ExecuteNonQuery(command);
				}
				catch {
					throw new Exception("Database could not be dropped.  Please remove any remaining text files and try again.");
				}
				command="CREATE DATABASE "+TestBase.UnitTestDbName;
				Database.ExecuteNonQuery(command);
				UnitTestsCore.DatabaseTools.SetDbConnection(TestBase.UnitTestDbName,serverAddr,port,userName,password);
				command=Properties.Resources.dump;
				Database.ExecuteNonQuery(command);
				string toVersion=Assembly.GetAssembly(typeof(OpenDental.PrefL)).GetName().Version.ToString();
				ProcedureCodes.TcodesClear();
				// TODO: FormProcCodes.ImportProcCodes("",CDT.Class1.GetADAcodes(),"");//IF THIS LINE CRASHES:
				//Go to Solution, Configuration Manager.  Exclude UnitTest project from build.
				AutoCodes.SetToDefault();
				ProcButtons.SetToDefault();
				ProcedureCodes.ResetApptProcsQuickAdd();
				//RefreshCache (might be missing a few)  Or, it might make more sense to do this as an entirely separate method when running.
				ProcedureCodes.RefreshCache();
				command="UPDATE userod SET Password='qhd+xdy/iMpe3xcjbBmB6A==' WHERE UserNum=1";//sets Password to 'pass' for middle tier testing.
				Database.ExecuteNonQuery(command);
				AddCdcrecCodes();

		return "Fresh database loaded from sql dump.\r\n";
		}

		///<summary>Manually adds the few CDCREC codes necessary for the HL7 unit tests.</summary>
		private static void AddCdcrecCodes() {
			string command="SELECT COUNT(*) FROM cdcrec";
			if(Database.ExecuteString(command)=="0") {
				Cdcrecs.Insert(new Cdcrec() {
					CdcrecCode="2106-3",
					HeirarchicalCode="R5",
					Description="WHITE"
				});
				Cdcrecs.Insert(new Cdcrec() {
					CdcrecCode="2135-2",
					HeirarchicalCode="E1",
					Description="HISPANIC OR LATINO"
				});
			}
		}
				
	}
}
