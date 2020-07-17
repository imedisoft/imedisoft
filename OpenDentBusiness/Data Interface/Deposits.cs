using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using DataConnectionBase;
using Imedisoft.Data;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Deposits {
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


		///<summary>Gets all Deposits, ordered by DateDeposit, DepositNum.  </summary>
		public static Deposit[] Refresh() {
			
			string command="SELECT * FROM deposit "
				+"ORDER BY DateDeposit";
			return Crud.DepositCrud.SelectMany(command).ToArray();
		}

		///<summary>Gets all Deposits, as well as the clinic(s) associated to the deposit.  The listClinicNums cannot be null.  If listClinicNums is empty, then will return the deposits for all clinics.</summary>
		public static Deposit[] GetForClinics(List<long> listClinicNums,bool isUnattached) {
			
			string command=
			"SELECT deposit.*,"
			+"(CASE COUNT(DISTINCT COALESCE(clinic.ClinicNum,0)) WHEN 1 THEN COALESCE(clinic.Abbr,'(None)') ELSE CONCAT('(',COUNT(DISTINCT COALESCE(clinic.ClinicNum,0)),')') END) AS ClinicAbbr "
			+"FROM deposit "
			+"INNER JOIN ( "
				+"SELECT DISTINCT DepositNum,ClinicNum "
				+"FROM payment "
				+"WHERE DepositNum!=0 ";
				if(listClinicNums.Count!=0) {
					command+="AND ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
				}
				command+="UNION "//This will remove duplicates if any exist between the two tables being unioned.
				+"SELECT DISTINCT DepositNum,ClinicNum "
				+"FROM claimpayment "
				+"INNER JOIN definition ON claimpayment.PayType=definition.DefNum AND definition.ItemValue='' "
				+"WHERE DepositNum!=0 ";
				if(listClinicNums.Count!=0) {
					command+="AND ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
				}
				command+=") PayInfo ON PayInfo.DepositNum=deposit.DepositNum "
			+"LEFT JOIN clinic ON clinic.ClinicNum=PayInfo.ClinicNum ";//LEFT JOIN since there may be some ClinicNum=0.
			if(isUnattached) {
				command+="WHERE NOT EXISTS(SELECT * FROM transaction WHERE deposit.DepositNum=transaction.DepositNum) ";
			}
			command+="GROUP BY deposit.DepositNum ";
			command+="ORDER BY deposit.DateDeposit";
			DataTable table=Database.ExecuteDataTable(command);
			List <Deposit> listDeposits=Crud.DepositCrud.TableToList(table);
			for(int i=0;i<listDeposits.Count;i++) {
				listDeposits[i].ClinicAbbr=PIn.String(table.Rows[i]["ClinicAbbr"].ToString());
			}
			return listDeposits.ToArray();
		}

		///<summary>Gets only Deposits which are not attached to transactions.</summary>
		public static Deposit[] GetUnattached() {
			
			string command="SELECT * FROM deposit "
				+"WHERE NOT EXISTS(SELECT * FROM transaction WHERE deposit.DepositNum=transaction.DepositNum) "
				+"ORDER BY DateDeposit";
			return Crud.DepositCrud.SelectMany(command).ToArray();
		}

		///<summary>Gets a single deposit directly from the database.</summary>
		public static Deposit GetOne(long depositNum) {
			
			return Crud.DepositCrud.SelectOne(depositNum);
		}

		///<summary></summary>
		public static void Update(Deposit dep){
			
			Crud.DepositCrud.Update(dep);
		}

		///<summary></summary>
		public static void Update(Deposit dep, Deposit depOld) {
			
			Crud.DepositCrud.Update(dep,depOld);
		}

		///<summary></summary>
		public static long Insert(Deposit dep) {
			
			return Crud.DepositCrud.Insert(dep);
		}

		///<summary>Returns without making any changes if dep.DepositNum==0.  Also handles detaching all payments and claimpayments.  Throws exception if
		///deposit is attached as a source document to a transaction.  The program should have detached the deposit from the transaction ahead of time, so
		///I would never expect the program to throw this exception unless there was a bug.</summary>
		public static void Delete(Deposit dep){
			
			if(dep.DepositNum==0) {
				return;
			}
			//check dependencies
			string command="SELECT COUNT(*) FROM transaction WHERE DepositNum ="+POut.Long(dep.DepositNum);
			if(PIn.Long(Database.ExecuteString(command))>0) {
				throw new ApplicationException(Lans.g("Deposits","Cannot delete deposit because it is attached to a transaction."));
			}
			//ready to delete
			command="UPDATE payment SET DepositNum=0 WHERE DepositNum="+POut.Long(dep.DepositNum);
			Database.ExecuteNonQuery(command);
			command="UPDATE claimpayment SET DepositNum=0 WHERE DepositNum="+POut.Long(dep.DepositNum);
			Database.ExecuteNonQuery(command);
			Crud.DepositCrud.Delete(dep.DepositNum);
		}

		///<summary>Detach specific payments and claimpayments from passed in deposit.</summary>
		public static void DetachFromDeposit(long depositNum,List<long> listPayNums,List<long> listClaimPaymentNums) {
			
			string command="";
			if(listPayNums.Count>0) {
				command="UPDATE payment SET DepositNum=0 WHERE DepositNum="+POut.Long(depositNum)
					+" AND PayNum IN("+string.Join(",",listPayNums)+")";
				Database.ExecuteNonQuery(command);
			}
			if(listClaimPaymentNums.Count>0) {
				command="UPDATE claimpayment SET DepositNum=0 WHERE DepositNum="+POut.Long(depositNum)
					+" AND ClaimPaymentNum IN("+string.Join(",",listClaimPaymentNums)+")";
				Database.ExecuteNonQuery(command);
			}
		}


	



	
	}

	

	


}




















