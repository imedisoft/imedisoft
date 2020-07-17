using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace OpenDentBusiness {
	///<summary></summary>
	public class RxAlerts {
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


		///<summary>Gets a list of all RxAlerts for one RxDef.</summary>
		public static List<RxAlert> Refresh(long rxDefNum) {
			
			string command="SELECT * FROM rxalert WHERE RxDefNum="+POut.Long(rxDefNum);
			return Crud.RxAlertCrud.SelectMany(command);
		}

		///<summary></summary>
		public static List<RxAlert> TableToList(DataTable table) {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			return Crud.RxAlertCrud.TableToList(table);
		}

		///<summary></summary>
		public static void Update(RxAlert alert) {
			
			Crud.RxAlertCrud.Update(alert);
		}

		///<summary></summary>
		public static long Insert(RxAlert alert) {
			
			return Crud.RxAlertCrud.Insert(alert);
		}

		///<summary></summary>
		public static void Delete(RxAlert alert) {
			
			string command="DELETE FROM rxalert WHERE RxAlertNum ="+POut.Long(alert.RxAlertNum);
			Database.ExecuteNonQuery(command);
		}

	
	

		
		
		
	}

		



		
	

	

	


}










