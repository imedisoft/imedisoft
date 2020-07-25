using CodeBase;
using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ProcTPs {
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update

		///<summary>Sets the priority for the procedures passed in that are associated to the designated treatment plan.</summary>
		public static void SetPriorityForTreatPlanProcs(long priority,long treatPlanNum,List<long> listProcNums) {
			if(listProcNums.IsNullOrEmpty()) {
				return;
			}
			
			Database.ExecuteNonQuery($@"UPDATE proctp SET Priority = {POut.Long(priority)}
				WHERE TreatPlanNum = {POut.Long(treatPlanNum)}
				AND ProcNumOrig IN({string.Join(",",listProcNums.Select(x => POut.Long(x)))})");
		}

		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		///<summary>Gets all ProcTPs for a given Patient ordered by ItemOrder.</summary>
		public static ProcTP[] Refresh(long patNum) {
			
			string command="SELECT * FROM proctp "
				+"WHERE PatNum="+POut.Long(patNum)
				+" ORDER BY ItemOrder";
			return Crud.ProcTPCrud.SelectMany(command).ToArray();
		}

		///<summary>Ordered by ItemOrder.</summary>
		public static List<ProcTP> RefreshForTP(long tpNum) {
			
			string command="SELECT * FROM proctp "
				+"WHERE TreatPlanNum="+POut.Long(tpNum)
				+" ORDER BY ItemOrder";
			DataTable table=Database.ExecuteDataTable(command);
			return Crud.ProcTPCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Update(ProcTP proc){
			
			Crud.ProcTPCrud.Update(proc);
		}

		///<summary></summary>
		public static long Insert(ProcTP proc) {
			
			//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
			proc.SecUserNumEntry=Security.CurrentUser.Id;
			return Crud.ProcTPCrud.Insert(proc);
		}

		///<summary></summary>
		public static void InsertOrUpdate(ProcTP proc, bool isNew){
			//No need to check RemotingRole; no call to db.
			if(isNew){
				Insert(proc);
			}
			else{
				Update(proc);
			}
		}

		///<summary>There are no dependencies.</summary>
		public static void Delete(ProcTP proc){
			
			string command= "DELETE from proctp WHERE ProcTPNum = '"+POut.Long(proc.ProcTPNum)+"'";
 			Database.ExecuteNonQuery(command);
		}

		///<summary>Gets a list for just one tp.  Used in TP module.  Supply a list of all ProcTPs for pt.</summary>
		public static ProcTP[] GetListForTP(long treatPlanNum,ProcTP[] listAll) {
			//No need to check RemotingRole; no call to db.
			ArrayList AL=new ArrayList();
			for(int i=0;i<listAll.Length;i++){
				if(listAll[i].TreatPlanNum!=treatPlanNum){
					continue;
				}
				AL.Add(listAll[i]);
			}
			ProcTP[] retVal=new ProcTP[AL.Count];
			AL.CopyTo(retVal);
			return retVal;
		}

		///<summary>No dependencies to worry about.</summary>
		public static void DeleteForTP(long treatPlanNum) {
			
			string command="DELETE FROM proctp "
				+"WHERE TreatPlanNum="+POut.Long(treatPlanNum);
			Database.ExecuteNonQuery(command);
		}

		public static List<ProcTP> GetForProcs(List<long> listProcNums) {
			
			if(listProcNums.Count==0) {
				return new List<ProcTP>();
			}
			string command = "SELECT * FROM proctp "
				+"WHERE proctp.ProcNumOrig IN ("+ string.Join(",",listProcNums) +")";
			return Crud.ProcTPCrud.SelectMany(command);
		}

		///<summary>Returns only three columns from all ProcTPs -- TreatPlanNum, PatNum, and ProcNumOrig.</summary>
		public static List<ProcTP> GetAllLim() {
			
			string command = "SELECT TreatPlanNum,PatNum,ProcNumOrig FROM proctp";
			DataTable table = Database.ExecuteDataTable(command);
			List<ProcTP> listProcTpsLim = new List<ProcTP>();
			foreach(DataRow row in table.Rows) {
				ProcTP procTp = new ProcTP();
				procTp.TreatPlanNum=PIn.Long(row["TreatPlanNum"].ToString());
				procTp.PatNum=PIn.Long(row["PatNum"].ToString());
				procTp.ProcNumOrig=PIn.Long(row["ProcNumOrig"].ToString());
				listProcTpsLim.Add(procTp);
			}
			return listProcTpsLim;
		}


	}

	

	


}




















