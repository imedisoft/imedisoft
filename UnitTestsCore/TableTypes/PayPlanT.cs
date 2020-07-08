using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDentBusiness;
using CodeBase;
using System.Globalization;

namespace UnitTestsCore {
	public class PayPlanT {
		public static PayPlan CreatePayPlan(long patNum,double totalAmt,double payAmt,DateTime datePayStart,long provNum,bool doInsert=true,
			List<PayPlanCharge> listCharges=null,long guarantorNum=0) 
		{
			if(guarantorNum==0) {
				guarantorNum=patNum;
			}
			PayPlan payPlan=new PayPlan();
			payPlan.Guarantor=guarantorNum;
			payPlan.PatNum=patNum;
			payPlan.PayAmt=totalAmt;
			payPlan.PayPlanDate=datePayStart;
			payPlan.PayAmt=totalAmt;
			if(doInsert) {
				PayPlans.Insert(payPlan);
			}
			if(payAmt > 0) {
				PayPlanCharge charge=new PayPlanCharge();
				charge.PayPlanNum=payPlan.PayPlanNum;
				charge.PatNum=patNum;
				charge.ChargeDate=datePayStart;
				charge.Principal=totalAmt;
				charge.ChargeType=PayPlanChargeType.Credit;
				double sumCharges=0;
				int countPayments=0;
				while(sumCharges < totalAmt) {
					charge=new PayPlanCharge();
					charge.ChargeDate=datePayStart.AddMonths(countPayments);
					charge.PatNum=patNum;
					charge.Guarantor=patNum;
					charge.PayPlanNum=payPlan.PayPlanNum;
					charge.Principal=Math.Min(payAmt,totalAmt-sumCharges);
					charge.ProvNum=provNum;
					sumCharges+=charge.Principal;
					charge.ChargeType=PayPlanChargeType.Debit;
					if(doInsert) {
						PayPlanCharges.Insert(charge);
					}
					listCharges?.Add(charge);
					countPayments++;
				}
			}
			return payPlan;
		}

		public static PayPlan CreatePayPlanNoCharges(long patNum,double totalAmt,DateTime payPlanDate,long guarantorNum=0,long provNum=0) {
			return CreatePayPlan(patNum,totalAmt,0,payPlanDate,provNum,guarantorNum:guarantorNum);
		}

		/// <summary>Creates a payplan and payplan charges with credits. Credit amount generated based off the total amount of the procedures in the list.
		/// If credits are not attached,list of procedures must be null and a total amount must be specified.</summary>
		public static PayPlan CreatePayPlanWithCredits(long patNum,double payAmt,DateTime datePayStart,long provNum=0,List<Procedure> listProcs=null
			,double totalAmt=0,long guarantorNum=0,long clinicNum=0,double APR=0,double downPayment=0)
		{
			double totalAmount;
			guarantorNum=guarantorNum==0?patNum:guarantorNum;//if it's 0, default to the patNum. 
			if(listProcs!=null) {
				double totalProcFees=listProcs.Sum(x => x.ProcFee);
				if(totalAmt > 0) {
					totalAmount=Math.Min(totalAmt,totalProcFees);
				}
				else {
					totalAmount=totalProcFees;
				}
			}
			else {
				totalAmount=totalAmt;
			}
			PayPlan payPlan=CreatePayPlanNoCharges(patNum,totalAmount,datePayStart,guarantorNum);//create charges later depending on if attached to procs or not.
			double totalRemaining=totalAmount;
			if(listProcs!=null) {
				foreach(Procedure proc in listProcs) {
					if(totalRemaining.IsLessThanOrEqualToZero()) {
						break;
					}
					PayPlanCharge credit=new PayPlanCharge();
					credit.PayPlanNum=payPlan.PayPlanNum;
					credit.PatNum=patNum;
					credit.ProcNum=proc.ProcNum;
					credit.ProvNum=proc.ProvNum;
					credit.Guarantor=patNum;//credits should always appear on the patient of the payment plan.
					credit.ChargeDate=datePayStart;
					credit.ClinicNum=clinicNum;
					credit.Principal=Math.Min(totalRemaining,proc.ProcFee);
					credit.ChargeType=PayPlanChargeType.Credit;
					PayPlanCharges.Insert(credit);//attach the credit for the proc amount. 
					totalRemaining-=credit.Principal;
				}
			}
			if(totalRemaining.IsGreaterThanZero()) {
				PayPlanCharge credit=new PayPlanCharge();
				credit.PayPlanNum=payPlan.PayPlanNum;
				credit.PatNum=patNum;
				credit.ChargeDate=datePayStart;
				credit.ProvNum=provNum;
				credit.ClinicNum=clinicNum;
				credit.Guarantor=patNum;//credits should always appear on the patient of the payment plan.
				credit.Principal=totalRemaining;
				credit.ChargeType=PayPlanChargeType.Credit;
				PayPlanCharges.Insert(credit);//attach the credit for the total amount.
			}
			//make debit charges for the payment plan
			CreateDebitsForPlan(payPlan,provNum:provNum,clinicNum:clinicNum,principalAmount:totalAmount,periodPayment:(decimal)payAmt,
				downPayment:downPayment,dateFirstPayment:datePayStart,dateInterestStart:datePayStart,APR:APR);
			return payPlan;
		}

		private static void CreateDebitsForPlan(PayPlan payPlan,long provNum=0,long clinicNum=0,double principalAmount=0,decimal periodPayment=0,
			double downPayment=0,DateTime dateFirstPayment=default,DateTime dateInterestStart=default,double APR=0,
			PayPlanFrequency payPlanFrequency=PayPlanFrequency.Monthly,PaymentSchedule paySchedule=PaymentSchedule.Monthly)
		{
			PayPlanTerms terms=new PayPlanTerms() {
				APR=APR,
				DateAgreement=DateTime.Now,
				DateFirstPayment=dateFirstPayment,
				DateInterestStart=dateInterestStart,
				DownPayment=downPayment,
				Frequency=payPlanFrequency,
				PayCount=0,
				PaySchedule=paySchedule,
				PeriodPayment=periodPayment,
				PrincipalAmount=principalAmount,
				RoundDec=2,
			};
			Family family=Patients.GetFamily(payPlan.PatNum);
			List<PayPlanCharge> listPayPlanCharges=new List<PayPlanCharge>();
			PayPlanEdit.CreateScheduleCharges(terms,payPlan,family,provNum,clinicNum,listPayPlanCharges);
			PayPlanCharges.InsertMany(listPayPlanCharges);
		}

		///<summary>Total of the adjustments made to the payment plan that have not come due yet. </summary>
		public static double GetTotalNegFutureAdjs(List<PayPlanCharge> listAllCharges) {
				return listAllCharges.FindAll(x => x.ChargeType==PayPlanChargeType.Debit && x.Principal.IsLessThan(0) 
					&& x.ChargeDate > DateTime.Today).Sum(x => x.Principal);
		}

		#region Dynamic Payment Plans
		public static PayPlan CreateDynamicPaymentPlan(long patNum,long guarantorNum,DateTime date,double downPaymentAmt,int APR,double payAmt,
			List<Procedure> listProcs,List<Adjustment> listAdjustments,PayPlanFrequency frequency = PayPlanFrequency.Weekly,
			DateTime dateInterestStart = default,long provNum = 0,int payCount = 0)
		{
			PayPlan payPlan=CreatePayPlanNoCharges(patNum,0,date,guarantorNum,provNum:provNum);
			//create the production links for the payment plan.
			List<PayPlanLink> listPayPlanLinks=new List<PayPlanLink>();
			foreach(Procedure proc in listProcs) {
				listPayPlanLinks.Add(PayPlanLinkT.CreatePayPlanLink(payPlan,proc.ProcNum,PayPlanLinkType.Procedure));
			}
			//Only make payment plan link entries for adjustments that are NOT attached to procedures.
			foreach(Adjustment adj in listAdjustments.FindAll(x => x.ProcNum==0)) {
				listPayPlanLinks.Add(PayPlanLinkT.CreatePayPlanLink(payPlan,adj.AdjNum,PayPlanLinkType.Adjustment));
			}
			payPlan.IsDynamic=true;
			payPlan.DatePayPlanStart=date;
			payPlan.DateInterestStart=dateInterestStart;
			payPlan.DownPayment=downPaymentAmt;
			payPlan.APR=APR;
			payPlan.IsLocked=APR==0 ? false : true;
			payPlan.Guarantor=guarantorNum;
			payPlan.PayAmt=payAmt;
			payPlan.ChargeFrequency=frequency;
			payPlan.NumberOfPayments=0;//This should never be set. We only store the payment amount for dynamic payplans.
			PayPlanTerms terms=PayPlanEdit.GetPayPlanTerms(payPlan,listPayPlanLinks);
			terms.PayCount=payCount;
			//now that terms are set, we need to potentially calculate the periodpayment amount since we only store that and not the payCount
			if(terms.PayCount!=0) {
				terms.PeriodPayment=PayPlanEdit.CalculatePeriodPayment(terms.APR,terms.Frequency,terms.PeriodPayment,terms.PayCount,terms.RoundDec
					,terms.PrincipalAmount,terms.DownPayment);
				terms.PayCount=0;
				payPlan.PayAmt=(double)terms.PeriodPayment;
			}
			PayPlans.Update(payPlan);
			Family fam=Patients.GetFamily(patNum);
			List<PayPlanCharge> listCharges=new List<PayPlanCharge>();
			if(!payPlan.DownPayment.IsZero()) {
				listCharges.AddRange(PayPlanCharges.GetForDownPayment(terms,fam,listPayPlanLinks,payPlan));
			}
			//immediately call the code to run the "service" on this payment plan in case they created a plan who's first charge is today. 
			listCharges.AddRange(PayPlanEdit.GetListExpectedCharges(new List<PayPlanCharge>(),terms,fam,listPayPlanLinks,payPlan,true
				,listExpectedChargesDownPayment: listCharges)
				.FindAll(x => x.ChargeDate <= DateTime.Today));
			if(listCharges.Count > 0) {
				PayPlanCharges.InsertMany(listCharges);
			}
			return payPlan;
		}

		public static PayPlanTerms GetTerms(PayPlan payplan,List<PayPlanLink> listLinksForPayPlan=null,double principalAmt=0) {
			PayPlanTerms terms=new PayPlanTerms();
			terms.DownPayment=payplan.DownPayment;
			terms.APR=payplan.APR;
			terms.DateInterestStart=payplan.DateInterestStart;
			terms.DateAgreement=payplan.PayPlanDate;
			terms.DateFirstPayment=payplan.DatePayPlanStart;
			terms.Frequency=payplan.ChargeFrequency;
			terms.PaySchedule=payplan.PaySchedule;
			terms.PeriodPayment=(decimal)payplan.PayAmt;
			terms.PayCount=payplan.NumberOfPayments;
			if(listLinksForPayPlan!=null) {
				terms.PrincipalAmount=(double)PayPlanProductionEntry.GetProductionForLinks(listLinksForPayPlan)
					.Sum(x => x.AmountOverride==0 ? x.AmountOriginal : x.AmountOverride);
			}
			else {
				terms.PrincipalAmount=principalAmt;
			}
			terms.RoundDec=CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits;
			return terms;
		}
		#endregion
	}

}
