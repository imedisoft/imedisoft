using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ToothGridCells{
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

		

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<ToothGridCell> Refresh(long patNum){
			
			string command="SELECT * FROM toothgridcell WHERE PatNum = "+POut.Long(patNum);
			return Crud.ToothGridCellCrud.SelectMany(command);
		}

		///<summary>Gets one ToothGridCell from the db.</summary>
		public static ToothGridCell GetOne(long toothGridCellNum){
			
			return Crud.ToothGridCellCrud.SelectOne(toothGridCellNum);
		}

		///<summary></summary>
		public static long Insert(ToothGridCell toothGridCell){
			
			return Crud.ToothGridCellCrud.Insert(toothGridCell);
		}

		///<summary></summary>
		public static void Update(ToothGridCell toothGridCell){
			
			Crud.ToothGridCellCrud.Update(toothGridCell);
		}

		///<summary></summary>
		public static void Delete(long toothGridCellNum) {
			
			string command= "DELETE FROM toothgridcell WHERE ToothGridCellNum = "+POut.Long(toothGridCellNum);
			Db.NonQ(command);
		}
		*/



	}
}