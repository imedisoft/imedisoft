using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class RxDefs {
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
		public static RxDef[] Refresh() {
			
			string command="SELECT * FROM rxdef ORDER BY Drug";
			return Crud.RxDefCrud.SelectMany(command).ToArray();
		}

		public static RxDef GetOne(long rxDefNum) {
			
			return Crud.RxDefCrud.SelectOne(rxDefNum);
		}

		///<summary></summary>
		public static void Update(RxDef def) {
			
			Crud.RxDefCrud.Update(def);
		}

		///<summary></summary>
		public static long Insert(RxDef def) {
			
			return Crud.RxDefCrud.Insert(def);
		}

		///<summary></summary>
		public static List<RxDef> TableToList(DataTable table) {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			return Crud.RxDefCrud.TableToList(table);
		}

		///<summary>Also deletes all RxAlerts that were attached.</summary>
		public static void Delete(RxDef def) {
			
			string command="DELETE FROM rxalert WHERE RxDefNum="+POut.Long(def.Id);
			Database.ExecuteNonQuery(command);
			command= "DELETE FROM rxdef WHERE RxDefNum = "+POut.Long(def.Id);
			Database.ExecuteNonQuery(command);
		}
	
	
	
	}

	

	


}













