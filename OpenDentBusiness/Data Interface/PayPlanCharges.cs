using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;
using CodeBase;
using Imedisoft.Data;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PayPlanCharges {
		#region Get Methods
		///<summary></summary>
		public static List<PayPlanCharge> GetForDownPayment(PayPlanTerms terms,Family family,List<PayPlanLink> listPayPlanLinks,PayPlan payplan) {
			//No remoting role check; no call to db
			//Create a temporary variable to keep track of the original PeriodPayment.
			decimal periodPaymentTemp=terms.PeriodPayment;
			double aprTemp=terms.APR;
			//Set the PeriodPayment to the current DownPayment so that the full amount of the down payment gets generated.
			//E.g. there are several procedures attached to the payment plan and the down payment only covers one and a half (partial proc).
			terms.PeriodPayment=(decimal)terms.DownPayment;
			terms.APR=0;//downpayments should pay on principal only
			List<PayPlanCharge> listDownPayments=PayPlanEdit.GetListExpectedCharges(new List<PayPlanCharge>(),terms,family,listPayPlanLinks,payplan,true
				,true,new List<PaySplit>());
			listDownPayments.ForEach(x => {
				x.Note="Down Payment";
				x.ChargeDate=DateTime.Today.Date;
				x.Interest=0;
			});
			//Put the PeriodPayment back to the way it was upon entry.
			terms.PeriodPayment=periodPaymentTemp;
			terms.APR=aprTemp;
			return listDownPayments;
		}

		///<summary>Gets all payplancharges for a specific payment plan.</summary>
		public static List<PayPlanCharge> GetForPayPlan(long payPlanNum) {
			
			string command=
				"SELECT * FROM payplancharge "
				+"WHERE PayPlanNum="+POut.Long(payPlanNum)
				+" ORDER BY ChargeDate";
			return Crud.PayPlanChargeCrud.SelectMany(command);
		}

		///<summary>Returns a list of payplancharges associated to the passed in payplannums.  Will return a blank list if none.</summary>
		public static List<PayPlanCharge> GetForPayPlans(List<long> listPayPlanNums) {
			
			if(listPayPlanNums==null || listPayPlanNums.Count<1) {
				return new List<PayPlanCharge>();
			}
			string command=
				"SELECT * FROM payplancharge "
				+"WHERE PayPlanNum IN ("+POut.String(String.Join(",",listPayPlanNums)) +") "
				+"ORDER BY ChargeDate";
			return Crud.PayPlanChargeCrud.SelectMany(command);
		}

		///<summary>Gets all payplan charges for the payplans passed in where the specified patient is the Guarantor.  Based on today's date.  
		///Will return both credits and debits.  Does not return insurance payment plan charges.</summary>
		public static List<PayPlanCharge> GetDueForPayPlan(PayPlan payPlan,long patNum) {
						
			return GetDueForPayPlans(new List<PayPlan> { payPlan },patNum);
		}

		///<summary>Gets all payplan charges for the payplans passed in where the specified patient is the Guarantor.  Based on today's date.  
		///Will return both credits and debits.  Does not return insurance payment plan charges.</summary>
		public static List<PayPlanCharge> GetDueForPayPlans(List<PayPlan> listPayPlans,long patNum) {
			
			if(listPayPlans.Count < 1) {
				return new List<PayPlanCharge>();
			}
			string command="SELECT payplancharge.* FROM payplan "
				+"INNER JOIN payplancharge ON payplancharge.PayPlanNum = payplan.PayPlanNum "
					+"AND payplancharge.ChargeDate <= "+DbHelper.Curdate()+" "
				+"WHERE payplan.Guarantor="+POut.Long(patNum)+" "
				+"AND payplan.PayPlanNum IN("+String.Join(", ",listPayPlans.Select(x => x.PayPlanNum).ToList())+") "
				+"AND payplan.PlanNum = 0 "; //do not return insurance payment plan charges.
			return Crud.PayPlanChargeCrud.SelectMany(command);
		}

		///<summary>Gets all payplan charges for the payplans passed in where the any of the patients in the list are the Guarantor or the patient on the
		///payment plan.  Will return both credits and debits.  Does not return insurance payment plan charges.</summary>
		public static List<PayPlanCharge> GetForPayPlans(List<long> listPayPlans,List<long> listPatNums) {
			if(listPayPlans.IsNullOrEmpty() || listPatNums.IsNullOrEmpty()) {
				return new List<PayPlanCharge>();
			}
			
			string command="SELECT payplancharge.* FROM payplan "
				+"INNER JOIN payplancharge ON payplancharge.PayPlanNum = payplan.PayPlanNum "
				+"WHERE (payplan.PatNum IN("+string.Join(",",listPatNums)+") OR payplan.Guarantor IN("+string.Join(",",listPatNums)+")) "
				+"AND payplan.PayPlanNum IN("+string.Join(", ",listPayPlans)+") "
				+"AND payplan.PlanNum = 0 "; //do not return insurance payment plan charges.
			return Crud.PayPlanChargeCrud.SelectMany(command);
		}

		///<summary></summary>
		public static List<PayPlanCharge> GetChargesForPayPlanChargeType(long payPlanNum, PayPlanChargeType chargeType) {
			
			string command="SELECT * FROM payplancharge "
				+"WHERE PayPlanNum="+POut.Long(payPlanNum)+" "
				+"AND ChargeType="+POut.Int((int)chargeType);
			return Crud.PayPlanChargeCrud.SelectMany(command);
		}

		///<summary>Gets all charges of the credit type that don't have a procnum of 0 and belong to any of the pats in listPatNums</summary>
		public static List<PayPlanCharge> GetAllProcCreditsForPats(List<long> listPatNums) {
			if(listPatNums.Count==0) {
				return new List<PayPlanCharge>();
			}
			
			string command=$"SELECT * FROM payplancharge WHERE payplancharge.ChargeType={POut.Int((int)PayPlanChargeType.Credit)} " +
				$"AND payplancharge.ProcNum!=0 AND payplancharge.PatNum IN ({string.Join(",",listPatNums)})";
			return Crud.PayPlanChargeCrud.SelectMany(command);
		}

		///<summary>Takes a procNum and returns a list of all payment plan charges associated to the procedure.
		///Returns an empty list if there are none.</summary>
		public static List<PayPlanCharge> GetFromProc(long procNum) {
			
			string command=$"SELECT * FROM payplancharge WHERE payplancharge.ProcNum={POut.Long(procNum)} OR (payplancharge.LinkType=" +
				$"{POut.Int((int)PayPlanLinkType.Procedure)} AND payplancharge.FKey={POut.Long(procNum)})";
			return Crud.PayPlanChargeCrud.SelectMany(command);
		}

		///<summary>Gets a list of all payment plan charges of type Credit associated to the procedures for patient payment plans.</summary>
		public static List<PayPlanCharge> GetPatientPayPlanCreditsForProcs(List<long> listProcNums) {
			if(listProcNums.Count==0) {
				return new List<PayPlanCharge>();
			}
			
			string command=$"SELECT * FROM payplancharge WHERE payplancharge.ProcNum IN({string.Join(",",listProcNums.Select(x => POut.Long(x)))})" +
				$" AND payplancharge.ChargeType={POut.Int((int)PayPlanChargeType.Credit)}";
			return Crud.PayPlanChargeCrud.SelectMany(command);
		}

		///<summary></summary>
		public static PayPlanCharge GetOne(long payPlanChargeNum) {
			
			string command=
				"SELECT * FROM payplancharge "
				+"WHERE PayPlanChargeNum="+POut.Long(payPlanChargeNum);
			return Crud.PayPlanChargeCrud.SelectOne(command);
		}

		///<summary>Gets a list of charges for the passed in fkey and link type (i.e. adjustment, procedure...)</summary>
		public static List<PayPlanCharge> GetForLinkTypeAndFKeys(PayPlanLinkType linkType,params long[] arrayFKeys) {
			if(arrayFKeys.IsNullOrEmpty()) {
				return new List<PayPlanCharge>();
			}
			
			string command=$"SELECT * FROM payplancharge "+
				$"WHERE payplancharge.FKey IN({string.Join(",",arrayFKeys.Select(x => POut.Long(x)))}) "+
				$"AND payplancharge.LinkType={POut.Int((int)linkType)}";
			return Crud.PayPlanChargeCrud.SelectMany(command);
		}
		#endregion

		#region Modification Methods
		
		#region Insert
		///<summary></summary>
		public static long Insert(PayPlanCharge charge) {
			
			return Crud.PayPlanChargeCrud.Insert(charge);
		}
		
		///<summary></summary>
		public static void InsertMany(List<PayPlanCharge> listPayPlanCharges) {
			if(listPayPlanCharges.IsNullOrEmpty()) {
				return;
			}
			
			Crud.PayPlanChargeCrud.InsertMany(listPayPlanCharges);
		}
		#endregion

		#region Update
		///<summary>Takes a procNum and updates all of the dates of the payment plan charge credits associated to it.
		///If a completed procedure is passed in, it will update all of the payment plan charges associated to it to the ProcDate. 
		///If a non-complete procedure is passed in, it will update the charges associated to MaxValue.
		///Does nothing if there are no charges attached to the passed-in procedure.</summary>
		public static void UpdateAttachedPayPlanCharges(Procedure proc) {
			
			List<PayPlanCharge> listCharges=GetFromProc(proc.ProcNum);
			foreach(PayPlanCharge chargeCur in listCharges) {
				chargeCur.ChargeDate=DateTime.MaxValue;
				if(proc.ProcStatus==ProcStat.C) {
					chargeCur.ChargeDate=proc.ProcDate;
				}
				Update(chargeCur); //one update statement for each payplancharge.
			}
			List<PayPlan> listPayPlans=PayPlans.GetAllForCharges(listCharges);
			PayPlans.UpdateTreatmentCompletedAmt(listPayPlans);
		}

		///<summary></summary>
		public static void Update(PayPlanCharge charge){
			
			Crud.PayPlanChargeCrud.Update(charge);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Must always pass in payPlanNum.</summary>
		public static void Sync(List<PayPlanCharge> listPayPlanCharges,long payPlanNum) {
			
			List<PayPlanCharge> listDB=PayPlanCharges.GetForPayPlan(payPlanNum);
			Crud.PayPlanChargeCrud.Sync(listPayPlanCharges,listDB);
		}
		#endregion

		#region Delete
		///<summary>Will delete all PayPlanCharges associated to the passed-in procNum from the database.  Does nothing if the procNum = 0.</summary>
		public static void DeleteForProc(long procNum) {
			
			if(procNum==0) {
				return;
			}
			List<PayPlan> listPayPlans=PayPlans.GetAllForCharges(GetFromProc(procNum));
			string command="DELETE FROM payplancharge WHERE ProcNum="+POut.Long(procNum);
			Database.ExecuteNonQuery(command);
			PayPlans.UpdateTreatmentCompletedAmt(listPayPlans);
		}

		///<summary></summary>
		public static void Delete(PayPlanCharge charge){
			
			string command= "DELETE from payplancharge WHERE PayPlanChargeNum = '"
				+POut.Long(charge.PayPlanChargeNum)+"'";
 			Database.ExecuteNonQuery(command);
		}	

		public static void DeleteMany(List<long> listCharges) {
			if(listCharges.IsNullOrEmpty()) {
				return;
			}
			
			string command=$"DELETE from payplancharge WHERE PayPlanChargeNum IN ({string.Join(",",listCharges.Select(x => POut.Long(x)))})";
			Database.ExecuteNonQuery(command);
		}
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		
	}
}