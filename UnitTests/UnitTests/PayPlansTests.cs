using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests.PayPlans_Tests {
	[TestClass]
	public class PayPlansTests:TestBase {

		///<summary>This method will execute only once, just before any tests in this class run.</summary>
		[ClassInitialize]
		public static void SetupClass(TestContext testContext) {
		}

		///<summary>This method will execute just before each test in this class.</summary>
		[TestInitialize]
		public void SetupTest() {
		}

		///<summary>This method will execute after each test in this class.</summary>
		[TestCleanup]
		public void TearDownTest() {
		}

		///<summary>This method will execute only once, just after all tests in this class have run.
		///However, this method is not guaranteed to execute before starting another TestMethod from another TestClass.</summary>
		[ClassCleanup]
		public static void TearDownClass() {
		}

		///<summary>Ensures that running GetOverPaidPayPlans correctly finds the overpaid PayPlan and returns it.</summary>
		[TestMethod]
		public void PayPlans_GetOverpaidPayPlans_CorrectlyReturnsOverPaidPayPlan() {
			//Setup
			long provNum=ProviderT.CreateProvider("LS");
			Patient pat=PatientT.CreatePatient(fName:"Austin",lName:"Patient",priProvNum:provNum);
			Carrier carrier=CarrierT.CreateCarrier("Blue Cross");
			InsPlan insplan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat,carrier.CarrierName);
			InsSubT.CreateInsSub(pat.PatNum,insplan.PlanNum,insInfo.PriInsSub.SubscriberID);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",4100,DateTime.Today.AddMonths(-1),provNum:provNum);
			Benefit benefit=BenefitT.CreatePercentForProc(insInfo.PriInsPlan.PlanNum,proc.CodeNum,50);
			insInfo.AddBenefit(benefit);
			insInfo.ListAllProcs=Procedures.Refresh(pat.PatNum);
			//Make a dynamic payment plan where the entire amount of the procedure is due right now (today).
			PayPlan dynamicPayPlan=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,pat.Guarantor,DateTime.Today,0,0,2050,
				insInfo.ListAllProcs,new List<Adjustment>{ });
			List<PayPlanCharge> listPayPlanCharges=PayPlanCharges.GetForPayPlan(dynamicPayPlan.PayPlanNum);
			Assert.AreEqual(1,listPayPlanCharges.Count);
			//Patient pays their portion.
			PaymentT.MakePayment(pat.PatNum,2050,payDate: DateTime.Now,procNum: proc.ProcNum,payPlanNum: dynamicPayPlan.PayPlanNum,
				payPlanChargeNum: listPayPlanCharges[0].PayPlanChargeNum);
			insInfo.ListAllClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			insInfo.ListAllProcs=Procedures.Refresh(pat.PatNum);
			//Insurance pays their portion.
			Claim claim=ClaimT.CreateClaim("P",insInfo.ListPatPlans,insInfo.ListInsPlans,insInfo.ListAllClaimProcs,insInfo.ListAllProcs,pat,
				new List<Procedure>{proc},insInfo.ListBenefits,insInfo.ListInsSubs);
			ClaimT.ReceiveClaim(claim,insInfo.ListAllClaimProcs,doAddPayAmount:true);
			//Payment plan should not be overpaid.
			List<PayPlan> listOverPaidDPP=PayPlans.GetOverChargedPayPlans(new List<long>{dynamicPayPlan.PayPlanNum});
			Assert.AreEqual(0,listOverPaidDPP.Count);
		}

		///<summary>Ensures that running GetOverPaidPayPlans correctly discerns when a PayPlan is not overpaid then fails to return it.</summary>
		[TestMethod]
		public void PayPlans_GetOverpaidPayPlans_DoesNotReturnPropperlyPaidPayPlan() {
			//Setup
			long provNum=ProviderT.CreateProvider("LS");
			Patient pat=PatientT.CreatePatient(fName:"Austin",lName:"Patient",priProvNum:provNum);
			Carrier carrier=CarrierT.CreateCarrier("Blue Cross");
			InsPlan insplan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat,carrier.CarrierName);
			InsSubT.CreateInsSub(pat.PatNum,insplan.PlanNum,insInfo.PriInsSub.SubscriberID);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",4100,DateTime.Today.AddMonths(-1),provNum:provNum);
			Benefit benefit=BenefitT.CreatePercentForProc(insInfo.PriInsPlan.PlanNum,proc.CodeNum,50);
			insInfo.AddBenefit(benefit);
			insInfo.ListAllProcs=Procedures.Refresh(pat.PatNum);
			//Make a dynamic payment plan where the entire amount of the procedure is due right now (today).
			PayPlan dynamicPayPlan=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,pat.Guarantor,DateTime.Today,0,0,2050,
				insInfo.ListAllProcs,new List<Adjustment>{ });
			List<PayPlanCharge> listPayPlanCharges=PayPlanCharges.GetForPayPlan(dynamicPayPlan.PayPlanNum);
			Assert.AreEqual(1,listPayPlanCharges.Count);
			//Patient pays their portion.
			PaymentT.MakePayment(pat.PatNum,2050,payDate: DateTime.Now,procNum: proc.ProcNum,payPlanNum: dynamicPayPlan.PayPlanNum,
				payPlanChargeNum: listPayPlanCharges[0].PayPlanChargeNum);
			insInfo.ListAllClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			insInfo.ListAllProcs=Procedures.Refresh(pat.PatNum);
			//Insurance pays their portion.
			Claim claim=ClaimT.CreateClaim("P",insInfo.ListPatPlans,insInfo.ListInsPlans,insInfo.ListAllClaimProcs,insInfo.ListAllProcs,pat,
				new List<Procedure>{proc},insInfo.ListBenefits,insInfo.ListInsSubs);
			ClaimT.ReceiveClaim(claim,insInfo.ListAllClaimProcs,doAddPayAmount:true);
			//And then insurance overpays their portion.
			ClaimProcT.AddInsPaid(pat.PatNum,insplan.PlanNum,proc.ProcNum,50,insInfo.PriInsSub.InsSubNum,0,0);
			//Payment plan should be overpaid.
			List<PayPlan> listOverPaidDPP=PayPlans.GetOverChargedPayPlans(new List<long>{dynamicPayPlan.PayPlanNum});
			Assert.AreEqual(1,listOverPaidDPP.Count);
		}

		///<summary>Ensures that running GetOverPaidPayPlans correctly discerns when a PayPlan is not overpaid even when there is an adjustment on the procedure.</summary>
		[TestMethod]
		public void PayPlans_GetOverpaidPayPlans_ConsidersAdjOnPayPlan() {
			//Setup
			long provNum=ProviderT.CreateProvider("LS");
			Patient pat=PatientT.CreatePatient(fName:"Austin",lName:"Patient",priProvNum:provNum);
			Family fam=Patients.GetFamily(pat.PatNum);
			Carrier carrier=CarrierT.CreateCarrier("Blue Cross");
			InsPlan insplan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat,carrier.CarrierName);
			InsSubT.CreateInsSub(pat.PatNum,insplan.PlanNum,insInfo.PriInsSub.SubscriberID);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",4100,DateTime.Today.AddMonths(-1),provNum:provNum);
			AdjustmentT.MakeAdjustment(pat.PatNum,950,procNum:proc.ProcNum);
			Benefit benefit=BenefitT.CreatePercentForProc(insInfo.PriInsPlan.PlanNum,proc.CodeNum,50);
			insInfo.AddBenefit(benefit);
			insInfo.ListAllProcs=Procedures.Refresh(pat.PatNum);
			PayPlan dynamicPayPlan=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,0,DateTime.Today.AddMonths(-1),0,0,280,insInfo.ListAllProcs,new List<Adjustment>{ });
			//Patient pays their portion
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,2050,DateTime.Now,true,0,1);
			insInfo.ListAllClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			insInfo.ListAllProcs=Procedures.Refresh(pat.PatNum);
			//Insurance pays their portion, but pays more than anticipated
			Claim claim=ClaimT.CreateClaim("P",insInfo.ListPatPlans,insInfo.ListInsPlans,insInfo.ListAllClaimProcs,insInfo.ListAllProcs,pat,
				new List<Procedure>{proc},insInfo.ListBenefits,insInfo.ListInsSubs);
			ClaimT.ReceiveClaim(claim,insInfo.ListAllClaimProcs);
			ClaimProcT.AddInsPaid(pat.PatNum,insplan.PlanNum,proc.ProcNum,2060,insInfo.PriInsSub.InsSubNum,0,0);
			//Process PayPLan Charges for amounts paid
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,transferPayment,true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat.PatNum},pat.PatNum
				,PaySplits.GetForPayment(transferPayment.PayNum),transferPayment.PayNum,false);
			PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum},pat.PatNum
				,chargeData.ListPaySplits,transferPayment,new List<AccountEntry>(),loadData:loadData);
			List<PayPlanCharge> listChargesDb=PayPlanCharges.GetForPayPlan(dynamicPayPlan.PayPlanNum);
			List<PayPlanLink> listEntries=PayPlanLinks.GetForPayPlans(new List<long>{dynamicPayPlan.PayPlanNum});
			PayPlanTerms terms=PayPlanT.GetTerms(dynamicPayPlan,listEntries);
			PayPlanEdit.GetListExpectedCharges(listChargesDb,terms,fam,listEntries,dynamicPayPlan,true);
			List<PayPlan> listOverPaidDPP=PayPlans.GetOverChargedPayPlans(new List<long>{dynamicPayPlan.PayPlanNum});
			Assert.AreEqual(0,listOverPaidDPP.Count);
		}

		///<summary>A bug was causing pay plans to be flagged as overcharged when more than half of a linked production entry was paid.</summary>
		[TestMethod]
		public void PayPlans_GetOverpaidPayPlans_DoNotReturnPayPlanWhenMajorityOfPrincipalIsPaid() {
			//Setup
			long provNum=ProviderT.CreateProvider("LS");
			Patient pat=PatientT.CreatePatient(fName:"Austin",lName:"Patient",priProvNum:provNum);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",4100,DateTime.Today.AddMonths(-1),provNum:provNum);
			//Make a dynamic payment plan where over half of the production is due right now.
			PayPlan dynamicPayPlan=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,pat.Guarantor,DateTime.Today,0,0,2060,
				new List<Procedure>{proc},new List<Adjustment>{ });
			List<PayPlanCharge> listPayPlanCharges=PayPlanCharges.GetForPayPlan(dynamicPayPlan.PayPlanNum);
			Assert.AreEqual(1,listPayPlanCharges.Count);
			//Patient pays full amount that is due
			PaymentT.MakePayment(pat.PatNum,2060,payDate: DateTime.Now,procNum: proc.ProcNum,payPlanNum: dynamicPayPlan.PayPlanNum,
				payPlanChargeNum: listPayPlanCharges[0].PayPlanChargeNum);
			//Payment plan should not be overpaid.
			List<PayPlan> listOverPaidDPP=PayPlans.GetOverChargedPayPlans(new List<long>{dynamicPayPlan.PayPlanNum});
			Assert.AreEqual(0,listOverPaidDPP.Count);
		}

		///<summary>A bug was causing pay plans to be flagged as overcharged when the pay splits applied to a production entry were greater
		///than the amount charged for that production entry. Pay plans should only be flagged as overcharged when the total for all debits exceeds
		///the total principal for all production entries.</summary>
		[TestMethod]
		public void PayPlans_GetOverpaidPayPlans_DoNotReturnPayPlanWhenSingleProductionEntryIsOvercharged() {
			//Setup
			long provNum=ProviderT.CreateProvider("LS");
			Patient pat=PatientT.CreatePatient(fName:"Austin",lName:"Patient",priProvNum:provNum);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0150",ProcStat.C,"",64,DateTime.Today.AddMonths(-1),provNum:provNum);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0274",ProcStat.C,"",58,DateTime.Today.AddMonths(-1),provNum:provNum);
			//Make a dynamic payment plan where over half of the production is due right now.
			PayPlan dynamicPayPlan=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,pat.Guarantor,DateTime.Today,35,0,35,
				new List<Procedure>{proc1,proc2},new List<Adjustment>{ },PayPlanFrequency.Monthly);
			List<PayPlanCharge> listPayPlanCharges=PayPlanCharges.GetForPayPlan(dynamicPayPlan.PayPlanNum);
			Assert.AreEqual(3,listPayPlanCharges.Count);
			PayPlanCharge proc2PayPlanCharge=listPayPlanCharges.FirstOrDefault(x => x.FKey==proc2.ProcNum);
			PayPlanCharge proc1PayPlanCharge=listPayPlanCharges.FirstOrDefault(x => x.FKey==proc1.ProcNum);
			//Patient pays full amount that is due
			PaymentT.MakePayment(pat.PatNum,58,payDate:DateTime.Now,procNum:proc2.ProcNum,payPlanNum:dynamicPayPlan.PayPlanNum,
				payPlanChargeNum:proc2PayPlanCharge.PayPlanChargeNum);
			PaymentT.MakePayment(pat.PatNum,12,payDate: DateTime.Now,procNum: proc1.ProcNum,payPlanNum: dynamicPayPlan.PayPlanNum,
				payPlanChargeNum: proc1PayPlanCharge.PayPlanChargeNum);
			//Payment plan should not be overpaid.
			List<PayPlan> listOverPaidDPP=PayPlans.GetOverChargedPayPlans(new List<long>{dynamicPayPlan.PayPlanNum});
			Assert.AreEqual(0,listOverPaidDPP.Count);
		}

		[TestMethod]
		public void PayPlanEdit_GetProductionForLinks_PayPlanLinksConsiderPatientPayments() {
			//set up payment plan
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			//create the produciton that will be attached to the payment plan
			List<Procedure> listProcs=new List<Procedure>();
			List<Adjustment> listAdjs=new List<Adjustment>();
			listProcs.Add(ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",50,DateTime.Today));
			PaymentT.MakePayment(pat.PatNum,22,procNum:listProcs.First().ProcNum);
			PayPlan dynamicPayPlan=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,pat.PatNum,DateTime.Today.AddMonths(1),5,12,25,listProcs,listAdjs
				,PayPlanFrequency.Monthly,dateInterestStart:DateTime.Today.AddMonths(2));
			List<PayPlanLink> listPayPlanLinks=PayPlanLinks.GetForPayPlans(new List<long>{dynamicPayPlan.PayPlanNum});
			List<PayPlanProductionEntry> listEntries=PayPlanProductionEntry.GetProductionForLinks(listPayPlanLinks);
			Assert.AreEqual(28,listEntries.Sum(x => x.AmountOriginal));//amount attached to the payplan should only attach amount needing to still be paid
		}

	}
}
