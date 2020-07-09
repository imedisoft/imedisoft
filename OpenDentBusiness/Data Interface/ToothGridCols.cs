using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ToothGridCols{
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
		public static List<ToothGridCol> Refresh(long patNum){
			
			string command="SELECT * FROM toothgridcol WHERE PatNum = "+POut.Long(patNum);
			return Crud.ToothGridColCrud.SelectMany(command);
		}

		///<summary>Gets one ToothGridCol from the db.</summary>
		public static ToothGridCol GetOne(long toothGridColNum){
			
			return Crud.ToothGridColCrud.SelectOne(toothGridColNum);
		}

		///<summary></summary>
		public static long Insert(ToothGridCol toothGridCol){
			
			return Crud.ToothGridColCrud.Insert(toothGridCol);
		}

		///<summary></summary>
		public static void Update(ToothGridCol toothGridCol){
			
			Crud.ToothGridColCrud.Update(toothGridCol);
		}

		///<summary></summary>
		public static void Delete(long toothGridColNum) {
			
			string command= "DELETE FROM toothgridcol WHERE ToothGridColNum = "+POut.Long(toothGridColNum);
			Db.NonQ(command);
		}
		*/



	}
}