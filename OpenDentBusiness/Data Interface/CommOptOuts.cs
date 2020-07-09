using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class CommOptOuts{

		///<summary></summary>
		public static List<CommOptOut> Refresh(long patNum) {
			
			string command="SELECT * FROM commoptout WHERE PatNum = "+POut.Long(patNum);
			return Crud.CommOptOutCrud.SelectMany(command);
		}

		public static List<CommOptOut> GetForPats(List<long> listPatNums,CommOptOutType optOutType) {
			if(listPatNums.Count==0) {
				return new List<CommOptOut>();
			}
			
			string command="SELECT * FROM commoptout WHERE PatNum IN("+string.Join(",",listPatNums.Select(x => POut.Long(x)))+") "
				+"AND CommType="+POut.Int((int)optOutType);
			return Crud.CommOptOutCrud.SelectMany(command);
		}


		///<summary></summary>
		public static void InsertMany(List<CommOptOut> listCommOptOuts) {
			if(listCommOptOuts.Count==0) {
				return;
			}
			
			Crud.CommOptOutCrud.InsertMany(listCommOptOuts);
		}

		public static void DeleteMany(List<CommOptOut> listCommOptOuts) {
			if(listCommOptOuts.Count==0) {
				return;
			}
			
			string command="DELETE FROM commoptout WHERE CommOptOutNum IN("+string.Join(",",
				listCommOptOuts.Select(x => POut.Long(x.CommOptOutNum)))+")";
			Db.NonQ(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		#region Get Methods
		
		///<summary>Gets one CommOptOut from the db.</summary>
		public static CommOptOut GetOne(long communicationOptOutNum){
			
			return Crud.CommOptOutCrud.SelectOne(communicationOptOutNum);
		}
		#endregion
		#region Modification Methods
			#region Insert
			#endregion
			#region Update
		///<summary></summary>
		public static void Update(CommOptOut communicationOptOut){
			
			Crud.CommOptOutCrud.Update(communicationOptOut);
		}
			#endregion
			#region Delete
		///<summary></summary>
			#endregion
		#endregion
		#region Misc Methods
		

		
		#endregion
		*/



	}
}