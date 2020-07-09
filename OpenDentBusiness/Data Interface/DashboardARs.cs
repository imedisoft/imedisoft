using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class DashboardARs{
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		///<summary>Gets all rows gt= dateFrom.</summary>
		public static List<DashboardAR> Refresh(DateTime dateFrom){
			
			string command="SELECT * FROM dashboardar WHERE DateCalc >= "+POut.Date(dateFrom);
			return ReportsComplex.RunFuncOnReportServer(() => Crud.DashboardARCrud.SelectMany(command));
		}

		///<summary></summary>
		public static long Insert(DashboardAR dashboardAR){
			
			return Crud.DashboardARCrud.Insert(dashboardAR);
		}

		///<summary>Dashboardar is safe to truncate because it gets refilled as needed and there are no FKeys to any other table.</summary>
		public static void Truncate() {
			
			string command = "TRUNCATE dashboardar";
			Db.NonQ(command);
			if(!string.IsNullOrEmpty(PrefC.ReportingServer.Server)) { //only attempt to insert into the reporting server if the reporting server is set up.
				ReportsComplex.RunFuncOnReportServer(() => Db.NonQ(command));
			}
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary>Gets one DashboardAR from the db.</summary>
		public static DashboardAR GetOne(long dashboardARNum){
			
			return Crud.DashboardARCrud.SelectOne(dashboardARNum);
		}

		///<summary></summary>
		public static void Update(DashboardAR dashboardAR){
			
			Crud.DashboardARCrud.Update(dashboardAR);
		}

		///<summary></summary>
		public static void Delete(long dashboardARNum) {
			
			string command= "DELETE FROM dashboardar WHERE DashboardARNum = "+POut.Long(dashboardARNum);
			Db.NonQ(command);
		}
		*/



	}
}