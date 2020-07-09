using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EntryLogs{
		#region Get Methods
		
		///<summary>Gets one EntryLog from the db.</summary>
		public static EntryLog GetOne(long entryLogNum){
			
			return Crud.EntryLogCrud.SelectOne(entryLogNum);
		}
		#endregion
		#region Modification Methods
			#region Insert
		///<summary></summary>
		public static long Insert(EntryLog entryLog){
			
			return Crud.EntryLogCrud.Insert(entryLog);
		}

		public static void InsertMany(List<EntryLog> listEntry) {
			
			Crud.EntryLogCrud.InsertMany(listEntry);
		}
			#endregion
			#region Update
			#endregion
			#region Delete
			#endregion
		#endregion
		#region Misc Methods
		

		
		#endregion

	}
}