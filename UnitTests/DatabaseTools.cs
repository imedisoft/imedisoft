using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OpenDental;
using OpenDentBusiness;
using System.Windows.Forms;
using CodeBase;

namespace UnitTests {
	///<summary>Contains the queries, scripts, and tools to clear the database of data from previous unitTest runs.</summary>
	class DatabaseTools {
		public static string FreshFromDump(string serverAddr,string port,string userName,string password) {
			Security.CurUser=Security.CurUser??new Userod();

				string command="DROP DATABASE IF EXISTS "+TestBase.UnitTestDbName;
				try {
					DataCore.NonQ(command);
				}
				catch {
					throw new Exception("Database could not be dropped.  Please remove any remaining text files and try again.");
				}
				command="CREATE DATABASE "+TestBase.UnitTestDbName;
				DataCore.NonQ(command);
				UnitTestsCore.DatabaseTools.SetDbConnection(TestBase.UnitTestDbName,serverAddr,port,userName,password);
				command=Properties.Resources.dump;
				DataCore.NonQ(command);
				string toVersion=Assembly.GetAssembly(typeof(OpenDental.PrefL)).GetName().Version.ToString();
				ProcedureCodes.TcodesClear();
				FormProcCodes.ImportProcCodes("",CDT.Class1.GetADAcodes(),"");//IF THIS LINE CRASHES:
				//Go to Solution, Configuration Manager.  Exclude UnitTest project from build.
				AutoCodes.SetToDefault();
				ProcButtons.SetToDefault();
				ProcedureCodes.ResetApptProcsQuickAdd();
				//RefreshCache (might be missing a few)  Or, it might make more sense to do this as an entirely separate method when running.
				ProcedureCodes.RefreshCache();
				command="UPDATE userod SET Password='qhd+xdy/iMpe3xcjbBmB6A==' WHERE UserNum=1";//sets Password to 'pass' for middle tier testing.
				DataCore.NonQ(command);
				AddCdcrecCodes();

		return "Fresh database loaded from sql dump.\r\n";
		}

		///<summary>Manually adds the few CDCREC codes necessary for the HL7 unit tests.</summary>
		private static void AddCdcrecCodes() {
			string command="SELECT COUNT(*) FROM cdcrec";
			if(DataCore.GetScalar(command)=="0") {
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
