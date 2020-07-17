using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	public class ClaimValCodeLogs {
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


		public static double GetValAmountTotal(long claimNum, string valCode){
			
			//double total = 0;
			string command="SELECT SUM(ValAmount) FROM claimvalcodelog WHERE ClaimNum="+POut.Long(claimNum)+" AND ValCode='"+POut.String(valCode)+"'";
			return Database.ExecuteDouble(command);
			//DataTable table=Db.GetTable(command);
			//for(int i=0;i<table.Rows.Count;i++){
			//	total+=PIn.Double(table.Rows[i][4].ToString());
			//}
			//return total;
		}

		public static List<ClaimValCodeLog> GetForClaim(long claimNum) {
			
			string command="SELECT * FROM claimvalcodelog WHERE ClaimNum="+POut.Long(claimNum);
			return Crud.ClaimValCodeLogCrud.SelectMany(command);
		}

		public static void UpdateList(List<ClaimValCodeLog> vCodes) {
			
			for(int i=0;i<vCodes.Count;i++){
				ClaimValCodeLog vc = vCodes[i];
				if(vc.ClaimValCodeLogNum==0){
					Crud.ClaimValCodeLogCrud.Insert(vc);
				} 
				else {
					Crud.ClaimValCodeLogCrud.Update(vc);
				}
			}
		}
	}
} 