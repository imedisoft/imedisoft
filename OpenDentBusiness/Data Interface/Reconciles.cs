using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;
using Imedisoft.Data;

namespace OpenDentBusiness{
	///<summary>The two lists get refreshed the first time they are needed rather than at startup.</summary>
	public class Reconciles {
	
		///<summary></summary>
		public static Reconcile[] GetList(long accountNum) {
			
			string command="SELECT * FROM reconcile WHERE AccountNum="+POut.Long(accountNum)
				+" ORDER BY DateReconcile";
			return Crud.ReconcileCrud.SelectMany(command).ToArray();
		}

		///<summary>Gets one reconcile directly from the database.  Program will crash if reconcile not found.</summary>
		public static Reconcile GetOne(long reconcileNum) {
			
			string command="SELECT * FROM reconcile WHERE ReconcileNum="+POut.Long(reconcileNum);
			return Crud.ReconcileCrud.SelectOne(command);
		}

		///<summary></summary>
		public static long Insert(Reconcile reconcile) {
			
			return Crud.ReconcileCrud.Insert(reconcile);
		}

		///<summary></summary>
		public static void Update(Reconcile reconcile) {
			
			Crud.ReconcileCrud.Update(reconcile);
		}

		///<summary>Throws exception if Reconcile is in use.</summary>
		public static void Delete(Reconcile reconcile) {
			
			//check to see if any journal entries are attached to this Reconcile
			string command="SELECT COUNT(*) FROM journalentry WHERE ReconcileNum="+POut.Long(reconcile.ReconcileNum);
			if(Database.ExecuteString(command)!="0"){
				throw new ApplicationException("Not allowed to delete a Reconcile with existing journal entries.");
			}
			command="DELETE FROM reconcile WHERE ReconcileNum = "+POut.Long(reconcile.ReconcileNum);
			Database.ExecuteNonQuery(command);
		}

	
	

		

	}

	
}




