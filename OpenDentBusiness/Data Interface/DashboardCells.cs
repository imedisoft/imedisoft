using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary>Insert, Update, Delete are all managed by DashboardLayouts. The 2 classes are tightly coupled and should not be modified separately.</summary>
	public class DashboardCells{
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

		///<summary></summary>
		public static List<DashboardCell> GetAll() {
			
			string command="SELECT * FROM dashboardcell";
			return Crud.DashboardCellCrud.SelectMany(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static long Insert(DashboardCell dashboardCell) {
			
			return Crud.DashboardCellCrud.Insert(dashboardCell);
		}

		///<summary></summary>
		public static void Delete(long dashboardCellNum) {
			
			Crud.DashboardCellCrud.Delete(dashboardCellNum);
		}

		///<summary>Gets one DashboardCell from the db.</summary>
		public static DashboardCell GetOne(long dashboardCellNum){
			
			return Crud.DashboardCellCrud.SelectOne(dashboardCellNum);
		}


		///<summary></summary>
		public static void Update(DashboardCell dashboardCell){
			
			Crud.DashboardCellCrud.Update(dashboardCell);
		}

		

		

		
		*/



	}
}