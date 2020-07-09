using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	///<summary></summary>
	public class DbmLogs {
		#region Get Methods
		///<summary>Gets one DbmLog from the db.</summary>
		public static DbmLog GetOne(long dbmLogNum) {
			
			return Crud.DbmLogCrud.SelectOne(dbmLogNum);
		}
		#endregion
		#region Modification Methods
		#region Insert
		///<summary></summary>
		public static long Insert(DbmLog dbmLog) {
			
			return Crud.DbmLogCrud.Insert(dbmLog);
		}

		public static void InsertMany(List<DbmLog> listDbmLogs) {
			
			Crud.DbmLogCrud.InsertMany(listDbmLogs);
		}
		#endregion
		#region Update
		///<summary></summary>
		public static void Update(DbmLog dbmLog) {
			
			Crud.DbmLogCrud.Update(dbmLog);
		}
		#endregion
		#region Delete
		///<summary></summary>
		public static void Delete(long dbmLogNum) {
			
			Crud.DbmLogCrud.Delete(dbmLogNum);
		}
		#endregion
		#endregion
		#region Misc Methods
		#endregion
	}
}