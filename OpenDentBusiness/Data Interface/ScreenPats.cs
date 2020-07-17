using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	///<summary></summary>
	public class ScreenPats {
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
		public static long Insert(ScreenPat screenPat) {
			
			return Crud.ScreenPatCrud.Insert(screenPat);
		}

		/// <summary></summary>
		public static List<ScreenPat> GetForScreenGroup(long screenGroupNum) {
			
			string command="SELECT * FROM screenpat WHERE ScreenGroupNum ="+POut.Long(screenGroupNum);
			return Crud.ScreenPatCrud.SelectMany(command);
		}

		///<summary>Inserts, updates, or deletes rows to reflect changes between listScreenPats and stale listScreenPatsOld.</summary>
		public static bool Sync(List<ScreenPat> listScreenPats,List<ScreenPat> listScreenPatsOld) {
			
			return Crud.ScreenPatCrud.Sync(listScreenPats,listScreenPatsOld);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<ScreenPat> Refresh(long patNum){
			
			string command="SELECT * FROM screenpat WHERE PatNum = "+POut.Long(patNum);
			return Crud.ScreenPatCrud.SelectMany(command);
		}

		///<summary>Gets one ScreenPat from the db.</summary>
		public static ScreenPat GetOne(long screenPatNum){
			
			return Crud.ScreenPatCrud.SelectOne(screenPatNum);
		}


		///<summary></summary>
		public static void Update(ScreenPat screenPat){
			
			Crud.ScreenPatCrud.Update(screenPat);
		}

		///<summary></summary>
		public static void Delete(long screenPatNum) {
			
			string command= "DELETE FROM screenpat WHERE ScreenPatNum = "+POut.Long(screenPatNum);
			Db.ExecuteNonQuery(command);
		}
		*/




	}
}