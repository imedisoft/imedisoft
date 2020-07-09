using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ErxLogs{
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
		public static long Insert(ErxLog erxLog) {
			
			return Crud.ErxLogCrud.Insert(erxLog);
		}

		///<summary>Returns the latest ErxLog entry for the specified patient and before the specified dateTimeMax. Can return null.
		///Called from Chart when fetching prescriptions from NewCrop to determine the provider on incoming prescriptions.</summary>
		public static ErxLog GetLatestForPat(long patNum,DateTime dateTimeMax) {
			
			string command=DbHelper.LimitOrderBy("SELECT * FROM erxlog WHERE PatNum="+POut.Long(patNum)+" AND DateTStamp<"+POut.DateT(dateTimeMax)+" ORDER BY DateTStamp DESC",1);
			List <ErxLog> listErxLog=Crud.ErxLogCrud.SelectMany(command);
			if(listErxLog.Count==0) {
				return null;
			}
			return listErxLog[0];
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<ErxLog> Refresh(long patNum){
			
			string command="SELECT * FROM erxlog WHERE PatNum = "+POut.Long(patNum);
			return Crud.ErxLogCrud.SelectMany(command);
		}

		///<summary>Gets one ErxLog from the db.</summary>
		public static ErxLog GetOne(long erxLogNum){
			
			return Crud.ErxLogCrud.SelectOne(erxLogNum);
		}

		///<summary></summary>
		public static void Update(ErxLog erxLog){
			
			Crud.ErxLogCrud.Update(erxLog);
		}

		///<summary></summary>
		public static void Delete(long erxLogNum) {
			
			string command= "DELETE FROM erxlog WHERE ErxLogNum = "+POut.Long(erxLogNum);
			Db.NonQ(command);
		}
		*/



	}
}