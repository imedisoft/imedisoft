﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestsCore;
using OpenDentBusiness;
using System.Reflection;
using CodeBase;

namespace UnitTests.PaymentEdit_Tests {
	[TestClass]
	public class PaymentEditTests:TestBase {
		#region Prepayment Test
		private List<PaySplit> _listFamPrePaySplits;
		private List<PaySplit> _listPosPrePay;
		private List<PaySplit> _listNegPrePay;
		#endregion

		///<summary>This method will execute only once, just before any tests in this class run.</summary>
		[ClassInitialize]
		public static void SetupClass(TestContext context) {

		}

		///<summary>This method will execute just before each test in this class.</summary>
		[TestInitialize]
		public void SetupTest() {
			PrefT.UpdateBool(PrefName.AllowPrepayProvider,true);
		}

		#region AllocateUnearned Tests

		[TestMethod]
		public void PaymentEdit_AllocateUnearned_NoUnearned() {
			//This scenario shouldn't be possible within the UI but the following functionality is desired if it somehow happens.
			//All excess unearned should apply to the 0 / 'None' provider if there is no actual unearned for specific providers to take from.
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum=ProviderT.CreateProvider(suffix);
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc=ProcedureT.CreateProcedure(pat,"NU001",ProcStat.C,"",100,provNum:provNum);
			List<AccountEntry> listAccountEntries=ConstructListCharges(listProcs:new List<Procedure>(){ proc });
			//Act like the user wants to apply an arbitrary amount of unearned towards proc even though there isn't any unearned ATM.
			List<PaySplit> listPaySplits=PaymentEdit.AllocateUnearned(0,50,listAccountEntries);
			Assert.AreEqual(2,listPaySplits.Count);
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==-50
				&& x.ProvNum==0//Excess unearned that doesn't actually exist should always come from the 0 / 'None' provider.
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==50
				&& x.ProvNum==provNum
				&& x.ProcNum==proc.ProcNum
				&& x.UnearnedType==0));
		}

		[TestMethod]
		public void PaymentEdit_AllocateUnearned_OtherProvider() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc=ProcedureT.CreateProcedure(pat,"OP001",ProcStat.C,"",100,provNum:provNum1);
			//Fill unearned with some money for provNum2.
			PaymentT.MakePayment(pat.PatNum,65,provNum:provNum2,unearnedType:unearnedType);
			List<AccountEntry> listAccountEntries=ConstructListCharges(listProcs:new List<Procedure>(){ proc });
			//Try to allocate a random amount from the unearned for provNum2 to provNum1 / proc
			List<PaySplit> listPaySplits=PaymentEdit.AllocateUnearned(0,50,listAccountEntries);
			Assert.AreEqual(2,listPaySplits.Count);
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==-50
				&& x.ProvNum==provNum2
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==50
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc.ProcNum
				&& x.UnearnedType==0));
		}

		[TestMethod]
		public void PaymentEdit_AllocateUnearned_OtherProviders() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			long provNum3=ProviderT.CreateProvider($"{suffix}-3");
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc=ProcedureT.CreateProcedure(pat,"OP002",ProcStat.C,"",100,provNum:provNum1);
			//Fill unearned with some money for provNum2 and provNum3.
			PaymentT.MakePayment(pat.PatNum,45,provNum:provNum2,unearnedType:unearnedType);
			PaymentT.MakePayment(pat.PatNum,55,provNum:provNum3,unearnedType:unearnedType);
			List<AccountEntry> listAccountEntries=ConstructListCharges(listProcs:new List<Procedure>(){ proc });
			//Try to allocate the full $100 amount from unearned for provNum2 and provNum3 which should go to provNum1 / proc
			List<PaySplit> listPaySplits=PaymentEdit.AllocateUnearned(0,100,listAccountEntries);
			Assert.AreEqual(4,listPaySplits.Count);
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==-45
				&& x.ProvNum==provNum2
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==45
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc.ProcNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==-55
				&& x.ProvNum==provNum3
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==55
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc.ProcNum
				&& x.UnearnedType==0));
		}

		[TestMethod]
		public void PaymentEdit_AllocateUnearned_NoneAndRealProvider() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc=ProcedureT.CreateProcedure(pat,"NARP1",ProcStat.C,"",100,provNum:provNum1);
			//Fill unearned with some money for provNum2 and the None provider.
			PaymentT.MakePayment(pat.PatNum,45,provNum:provNum2,unearnedType:unearnedType);
			PaymentT.MakePayment(pat.PatNum,55,provNum:0,unearnedType:unearnedType);//The None provider somehow had unearned!
			List<AccountEntry> listAccountEntries=ConstructListCharges(listProcs:new List<Procedure>(){ proc });
			//Try to allocate the full $100 amount from unearned for provNum2 and the None provider which should go to provNum1 / proc
			List<PaySplit> listPaySplits=PaymentEdit.AllocateUnearned(0,100,listAccountEntries);
			Assert.AreEqual(4,listPaySplits.Count);
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==-45
				&& x.ProvNum==provNum2
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==45
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc.ProcNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==-55
				&& x.ProvNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==55
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc.ProcNum
				&& x.UnearnedType==0));
		}

		[TestMethod]
		public void PaymentEdit_AllocateUnearned_MultipleProcsOneProvider() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"MPOP1",ProcStat.C,"",45,provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"MPOP2",ProcStat.C,"",55,provNum:provNum1);
			//Fill unearned with some money for provNum.
			PaymentT.MakePayment(pat.PatNum,100,provNum:provNum1,unearnedType: unearnedType);
			List<AccountEntry> listAccountEntries=ConstructListCharges(listProcs:new List<Procedure>(){ proc1,proc2 });
			//Try to allocate the full $100 amount from unearned which should get split between the two procedures for provNum1.
			List<PaySplit> listPaySplits=PaymentEdit.AllocateUnearned(0,100,listAccountEntries);
			Assert.AreEqual(4,listPaySplits.Count);
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==-45
				&& x.ProvNum==provNum1
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==45
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==-55
				&& x.ProvNum==provNum1
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==55
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc2.ProcNum
				&& x.UnearnedType==0));
		}

		[TestMethod]
		public void PaymentEdit_AllocateUnearned_MultipleProcsMultipleProviders() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			long provNum3=ProviderT.CreateProvider($"{suffix}-3");
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"MPOP1",ProcStat.C,"",45,provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"MPOP2",ProcStat.C,"",55,provNum:provNum1);
			//Fill unearned with some money for provNum2 and provNum3.
			PaymentT.MakePayment(pat.PatNum,80,provNum:provNum2,unearnedType:unearnedType);
			PaymentT.MakePayment(pat.PatNum,20,provNum:provNum3,unearnedType:unearnedType);
			List<AccountEntry> listAccountEntries=ConstructListCharges(listProcs:new List<Procedure>(){ proc1,proc2 });
			//Try to allocate the full $100 amount from unearned which should get split between the two procedures for provNum1.
			List<PaySplit> listPaySplits=PaymentEdit.AllocateUnearned(0,100,listAccountEntries);
			Assert.AreEqual(6,listPaySplits.Count);
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==-20
				&& x.ProvNum==provNum3
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==20
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==-25
				&& x.ProvNum==provNum2
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==25
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==-55
				&& x.ProvNum==provNum2
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==55
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc2.ProcNum
				&& x.UnearnedType==0));
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AllocateUnearned_NoOverpay() {
			/*****************************************************
				Patient    pat1  (guarantor)
				Provider   provNum1
				Payment    prepayment1  pat1  provNum1  Today-5D   $100
				Procedure  proc1        pat1  provNum1  Today-3D   $50
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Patient pat1=PatientT.CreatePatient($"{suffix}-1");
			//Make a prepayment for one hundred dollars to unearned that the user will allocate later.
			Payment prepayment1=PaymentT.MakePayment(pat1.PatNum,100,payDate:DateTime.Today.AddDays(-5),provNum:provNum1,unearnedType:unearnedType);
			//Complete one procedure worth half of what is currently in the unearned bucket.
			Procedure proc1=ProcedureT.CreateProcedure(pat1,"PTP01",ProcStat.C,"",50,procDate:DateTime.Today.AddDays(-3),provNum:provNum1);
			//Act like the user selected proc1 within the Account module and clicked the Allocate Unearned menu option under the Payment button.
			List<AccountEntry> listAccountEntries=ConstructListCharges(listProcs:new List<Procedure>(){ proc1 });
			//Act like the user wants to apply an arbitrary amount of unearned towards proc even though there isn't any unearned ATM.
			List<PaySplit> listPaySplits=PaymentEdit.AllocateUnearned(0,50,listAccountEntries);
			Assert.AreEqual(2,listPaySplits.Count);
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==-50
				&& x.ProvNum==provNum1
				&& x.ProcNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,listPaySplits.Count(x => x.SplitAmt==50
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.UnearnedType==0));
			//Insert the splits into the database to mimic the user saving the payment as suggested.
			Payment paymentAllocate=PaymentT.MakePaymentNoSplits(pat1.PatNum,0,doInsert:false);
			Payments.Insert(paymentAllocate,listPaySplits);
			//Act like the user selected proc1 within the Account module and clicked the Allocate Unearned menu option under the Payment button AGAIN.
			//This is where the bug was because we would suggest two PaySplits to the user that were identical to the ones above.
			//There should be no PaySplits suggested to the user at this point because there is no outstanding production (regardless of proc selection).
			listAccountEntries=ConstructListCharges(listProcs:new List<Procedure>(){ proc1 });
			//Act like the user wants to allocate more unearned towards proc1 but it is already paid off so no splits should be suggested.
			listPaySplits=PaymentEdit.AllocateUnearned(0,50,listAccountEntries);
			Assert.AreEqual(0,listPaySplits.Count);
		}

		#endregion

		#region AutoSplitForPayment Tests

		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_ImplicitlyLinkCredits_NeverPayTPProc() {
			PrefT.UpdateYN(PrefName.PrePayAllowedForTpProcs,YN.Yes);
			long provNumDoc=ProviderT.CreateProvider("DOC");
			long provNumHyg=ProviderT.CreateProvider("HYG");
			Patient pat1=PatientT.CreatePatient(lName:"Jones",fName:"Jane",priProvNum:provNumDoc,clinicNum:1,secProvNum:provNumHyg);
			//Patient2 is a family member of Patient1, who is Gaurantor
			Patient pat2=PatientT.CreatePatient(lName:"Jones",fName:"Jack",priProvNum:provNumDoc,clinicNum:1,secProvNum:provNumHyg,guarantor:pat1.PatNum);
			//Patient 1, has 3 C Procs and 1 TP proc
			Procedure pat1Proc1=ProcedureT.CreateProcedure(pat1,"D0150",ProcStat.C,"",139.10,DateTime.Today.AddDays(-5),0,0,provNumDoc);
			Procedure pat1Proc2=ProcedureT.CreateProcedure(pat1,"D0210",ProcStat.C,"",483.00,DateTime.Today.AddDays(-5),0,0,provNumDoc);
			Procedure pat1Proc3=ProcedureT.CreateProcedure(pat1,"D1110",ProcStat.C,"",174.80,DateTime.Today.AddDays(-5),0,0,provNumHyg);
			Procedure pat1Proc4=ProcedureT.CreateProcedure(pat1,"D1351",ProcStat.TP,"16",120.70,DateTime.Today,0,provNumDoc);
			//Patient2,  has 3 C Procs
			Procedure pat2Proc1=ProcedureT.CreateProcedure(pat2,"D0150",ProcStat.C,"",139.10,DateTime.Today.AddDays(-2),0,0,provNumDoc);
			Procedure pat2Proc2=ProcedureT.CreateProcedure(pat2,"D0210",ProcStat.C,"",483.00,DateTime.Today.AddDays(-2),0,0,provNumDoc);
			Procedure pat2Proc3=ProcedureT.CreateProcedure(pat2,"D1110",ProcStat.C,"",174.80,DateTime.Today.AddDays(-2),0,0,provNumHyg);
			//Patient2 has insurance
			Carrier carrier=CarrierT.CreateCarrier("ABC");
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub insSub=InsSubT.CreateInsSub(pat2.PatNum,plan.PlanNum);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat2,carrier.CarrierName);
			//Patient2 has partially paid their procs with insurance claim, paid "As Total"
			ClaimProcT.AddInsPaidAsTotal(pat2.PatNum,plan.PlanNum,provNumDoc,398.45,insSub.InsSubNum,0,0);
			//Patient 2 pays of the rest of their balance while allocating to all but 1 proc
			Payment pat2attachedPayment1=PaymentT.MakePayment(pat2.PatNum,139.10,DateTime.Now,0,provNumDoc,pat2Proc1.ProcNum,0,1);
			Payment pat2attachedPayment2=PaymentT.MakePayment(pat2.PatNum,259.35,DateTime.Now,0,provNumDoc,pat2Proc2.ProcNum,0,1);
			//Patient 2 now has a zero balance
			Assert.AreEqual(0,pat2.EstBalance);
			//Patient 1 pays of the rest of their balance with an unallocated lump-sum payment
			Payment pat1unattachedPayment=PaymentT.MakePayment(pat1.PatNum,796.90,DateTime.Today.AddDays(-1),0,provNumDoc,0,1);
			//Patient 1 now has a zero balance
			Assert.AreEqual(0,pat1.EstBalance);
			Assert.AreEqual(0,pat1.BalTotal);//Ensure the family has a zero balance at this point
			//Create a zero charge payment to process the rest, use Guarantor
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat1.PatNum,0,DateTime.Now,true,0,1);
			//Create paysplits for Family
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat1,transferPayment,true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat1.PatNum, pat2.PatNum},pat1.PatNum
				,PaySplits.GetForPayment(transferPayment.PayNum),transferPayment.PayNum,false);
			PaymentEdit.ConstructResults chargeResult=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat1.PatNum, pat2.PatNum},pat1.PatNum
				,chargeData.ListPaySplits,transferPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplitForPayment(chargeResult);
			List<AccountEntry> listTPProcs=chargeResult.ListAccountCharges.FindAll(//Create a list of all TP Procs in ListAccountCharges
				x => x.GetType()==typeof(Procedure) 
				&& ((Procedure)x.Tag).ProcStatus==ProcStat.TP
			);
			List<AccountEntry> listCProcs=chargeResult.ListAccountCharges.FindAll(//Create a list of all C Procs in ListAccountCharges
				x => x.GetType()==typeof(Procedure) 
				&& ((Procedure)x.Tag).ProcStatus==ProcStat.C
			);
			Assert.IsTrue(listTPProcs.Sum(x =>x.AmountEnd)!=0);//All TP PRocs have not been paid (so AmountEnd should not be 0) THIS LINE FAILS WITHOUT FIX
			Assert.IsTrue(listCProcs.Sum(x => x.AmountEnd)==0);//All C Procs are paid (so AmountEnd is 0)
		}

		///<summary>Make sure auto splits go to each procedure for proper amounts.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_SplitForPaymentLessThanTotalofProcs() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",40);
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,50);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,true,false);
			PaymentEdit.ConstructResults chargeResult=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,loadData.ConstructChargesData.ListPaySplits,pay,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplit=PaymentEdit.AutoSplitForPayment(chargeResult);
			Assert.AreEqual(2,autoSplit.ListAutoSplits.Count);
			Assert.AreEqual(1,autoSplit.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(40)));
			Assert.AreEqual(1,autoSplit.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(10)));
			Assert.AreEqual(2,autoSplit.ListAutoSplits.Count(x => x.UnearnedType==0));
		}

		///<summary>Make sure there are no negative auto splits created for an overpaid procedure.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_NoNegativeAutoSplits() {
			long provNumA=ProviderT.CreateProvider("provA");
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",70);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0150",ProcStat.C,"",20);
			//make an overpayment for one of the procedures so it spills over.
			DateTime payDate=DateTime.Today;
			Payment pay=PaymentT.MakePayment(pat.PatNum,71,payDate,procNum:proc1.ProcNum);//pre-existing payment
			//attempt to make another payment. Auto splits should not suggest a negative split.
			Payment newPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,2,payDate,isNew:true,
				payType:Defs.GetDefsForCategory(DefCat.PaymentTypes,true)[0].DefNum);//current payment we're trying to make
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,newPayment,true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long> {pat.PatNum },pat.PatNum,
				PaySplits.GetForPayment(pay.PayNum),pay.PayNum,false);
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,chargeData.ListPaySplits,newPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplits=PaymentEdit.AutoSplitForPayment(constructResults);
			Assert.AreEqual(0,autoSplits.ListAutoSplits.FindAll(x => x.SplitAmt<0).Count);//assert no negative auto splits were made.
			Assert.AreEqual(0,autoSplits.ListSplitsCur.FindAll(x => x.SplitAmt<0).Count);//auto splits not catching everything
			Assert.AreEqual(0,autoSplits.ListAutoSplits.Count(x => x.UnearnedType!=0));
		}

		///<summary>Make sure auto splits go to payment plan charges for proper amounts and aren't marked as unearned.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_NoExtraZeroSplitsForPayPlanCharges() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",75);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0150",ProcStat.C,"",75);
			Procedure proc3=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",75);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,75,DateTime.Today.AddMonths(-4),0,new List<Procedure>() {proc1,proc2,proc3 });
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,100,DateTime.Today,true,1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat.PatNum},pat.PatNum
				,PaySplits.GetForPayment(pay.PayNum),pay.PayNum,false);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() {pat.PatNum},pat.PatNum,chargeData.ListPaySplits
				,pay,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplits=PaymentEdit.AutoSplitForPayment(results);
			//only two auto splits should exist. 1 covering the first whole payplancharge,and a second partial.
			Assert.AreEqual(2,autoSplits.ListAutoSplits.Count);
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(75)));
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(25)));
			Assert.AreEqual(2,autoSplits.ListAutoSplits.Count(x => x.UnearnedType==0));
		}

		///<summary>Make sure procedures are implicitly paid by unattached payment, and that a later procedure is paid by auto splits.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayments_ProceduresAndUnattachedPayments() {
			/*****************************************************
				Create Patient:  pat
				Create proc1:              Today-2  prov?  pat   $55
				Create proc2:              Today-2  prov?  pat   $65
				Create proc3:              Today-2  prov?  pat   $75
				Create unattachedPayment:  Today-1  prov?  pat   $195
					^Unallocated
				Create newProc:            Today    prov?  pat   $100
				Create newPayment:         Today    prov?  pat   $100
					^No splits (not a valid payment).  Designed for AutoSplitForPayment logic.
			******************************************************/
			//Note, timing matters for this test. If all procedures are for today, the asserts will not be true.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			//make past procedures and pay them off with an unattached payment. Ok for prov, but do not link the procs.
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",55,DateTime.Today.AddDays(-2));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0150",ProcStat.C,"",65,DateTime.Today.AddDays(-2));
			Procedure proc3=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",75,DateTime.Today.AddDays(-2));
			Payment unattachedPayment=PaymentT.MakePayment(pat.PatNum,195,DateTime.Today.AddDays(-1));//no other fields because unattached.
			//make new procedure
			Procedure newProc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,DateTime.Today);
			Payment newPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,100);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat.PatNum},pat.PatNum
				,PaySplits.GetForPayment(newPayment.PayNum),newPayment.PayNum,false);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() {pat.PatNum},pat.PatNum,chargeData.ListPaySplits
				,newPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplits=PaymentEdit.AutoSplitForPayment(results);
			/*****************************************************
			AccountEntry:  Today  prov?  pat
				^AmountOriginal = $295
				^AmountEnd      = $0  [$295 (procs) - $195 (implicit unattachedPayment) - $100 (auto split newPayment)]
			******************************************************/
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count);
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count(x => x.SplitAmt==100 && x.UnearnedType==0));
			Assert.AreEqual(4,autoSplits.ListAccountCharges.Count);
			Assert.AreEqual(1,autoSplits.ListAccountCharges.Count(x => x.AmountOriginal==55 && x.AmountEnd==0 && x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,autoSplits.ListAccountCharges.Count(x => x.AmountOriginal==65 && x.AmountEnd==0 && x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,autoSplits.ListAccountCharges.Count(x => x.AmountOriginal==75 && x.AmountEnd==0 && x.ProcNum==proc3.ProcNum));
			Assert.AreEqual(1,autoSplits.ListAccountCharges.Count(x => x.AmountOriginal==100 && x.AmountEnd==0 && x.ProcNum==newProc.ProcNum));
		}

		///<summary>Make sure auto split will be for procedure even when payment plan payment covers more than payment plan charge (but less than proc total)</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_NoUnallocatedSplitsWhenPayPlanPaymentCoversMoreThanOneChargeInAPayment() {
			//Full split amount was being added to both charge's split collections during Explicit Linking
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("Prov"+MethodBase.GetCurrentMethod().Name);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0150",ProcStat.C,"",100,DateTime.Today.AddMonths(-2),provNum:provNum);
			//Payment Plan For The Procedure
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,25,DateTime.Today.AddMonths(-1),provNum,new List<Procedure> {proc});
			//Initial payment - important that it covers more than the first charge
			Payment firstPayment=PaymentT.MakePayment(pat.PatNum,31,DateTime.Today.AddDays(-1),payplan.PayPlanNum,provNum,proc.ProcNum,1);
			//Go to make another payment - 19 should be the current amount remaining on the payment plan. (25+25) - 31 = 19 
			Payment curPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,19,DateTime.Today,true,payType:1);//1 because not income txfr.
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,curPayment,true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat.PatNum },pat.PatNum,
				loadData.ConstructChargesData.ListPaySplits,curPayment.PayNum,false);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,chargeData.ListPaySplits,
				curPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplits=PaymentEdit.AutoSplitForPayment(results);
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count);
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(19)));
			Assert.AreEqual(1,autoSplits.ListAutoSplits.Count(x => x.UnearnedType==0));//make sure it's not unallocated.
		}

		///<summary>If there is an adjustment that needs paying, autosplit logic should attach the paysplit to the adjustment. (treat it like a proc)</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PaymentAutoSplitToAdjustment() {
			//Make an unattached adjustment for 50 (a charge)
			//Make a new payment for 50
			//Perform a normal payment - There should be one split for 50 that has the adjustment's AdjNum
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("ProvA");
			Adjustment adjust1=AdjustmentT.MakeAdjustment(pat.PatNum,50,DateTime.Today,provNum:provNum);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,50);
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,new PaymentEdit.LoadData());
			Assert.AreEqual(1,results.ListAutoSplits.Count);
			Assert.AreEqual(results.ListAutoSplits[0].AdjNum,adjust1.AdjNum);
		}

		///<summary>Auto split logic should not use later payments to allocate to the same adjustment that's been allocated to already.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_AutoSplitDoesntReuseAllocatedAdjustment() {
			//Make an unattached adjustment for 50 (a charge)
			//Make a payment for 50 to that adjustment
			//Create a procedure that comes after the adjustment for 25
			//Make a new payment for 50 and autosplit it.
			//The auto payment should have 2 splits - One for the procedure and one to unallocated, each for 25.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("ProvA");
			Adjustment adjust1=AdjustmentT.MakeAdjustment(pat.PatNum,50,DateTime.Today.AddDays(-3),provNum:provNum);
			Payment payOld=PaymentT.MakePaymentNoSplits(pat.PatNum,50,DateTime.Today.AddDays(-3));
			PaySplitT.CreateSplit(adjust1.ClinicNum,pat.PatNum,payOld.PayNum,0,DateTime.Today.AddDays(-3),0,provNum,50,0,adjust1.AdjNum);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",25);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,50,DateTime.Today);
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,new PaymentEdit.LoadData());
			Assert.AreEqual(2,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAutoSplits.FindAll(x => x.PatNum==proc.PatNum && x.ProvNum==proc.ProvNum && x.ClinicNum==proc.ClinicNum && x.ProcNum==proc.ProcNum && x.AdjNum==0 && x.UnearnedType==0 && x.SplitAmt==25).Count);
			Assert.AreEqual(1,results.ListAutoSplits.FindAll(x => x.PatNum==pat.PatNum && x.ProvNum==0 && x.ClinicNum==pat.ClinicNum && x.ProcNum==0 && x.AdjNum==0 && x.UnearnedType>0 && x.SplitAmt==25).Count);
		}

		///<summary>Adjustments that overpay procedures should be used to implicitly pay off anything.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_OverpaidProceduresUsedImplicitly() {
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum
				Create proc1:     Today     provNum  pat   $50
				Create adjust1:   Today-3D  provNum  pat  -$70
					^Attached to proc
				Create proc2:     Today     prov?    pat   $20
				Create payment:   Today-3D  prov?    pat   $50
					^Invalid payment (no pay splits) designed for autosplit logic.
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("ProvA");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",50,provNum:provNum);
			Adjustment adjust1=AdjustmentT.MakeAdjustment(pat.PatNum,-70,DateTime.Today.AddDays(-3),provNum:provNum,procNum:proc1.ProcNum);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",20);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,50,DateTime.Today.AddDays(-3));
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,new PaymentEdit.LoadData());
			/*****************************************************
			AccountEntry:  Today  provNum  pat   $50
				^Represents proc1
			AccountEntry:  Today  provNum  pat  -$70
				^Represents adjust1 that is attached to proc1
			AccountEntry:  Today  prov?    pat   $20
				^Represents proc2
			******************************************************/
			Assert.AreEqual(3,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum
				&& x.AmountOriginal==50
				&& x.AmountEnd==0//Implicit linking will have applied the $20 that was overpaid to this procedure to the other procedure.
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum
				&& x.AmountOriginal==-70
				&& x.AmountEnd==0//Directly applied to the associated procedure.
				&& x.Date==DateTime.Today.AddDays(-3)
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum!=provNum
				&& x.AmountOriginal==20
				&& x.AmountEnd==0//Implicit linking logic will count this procedure as "payment" for the other procedure.
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAutoSplits.Count);
			//One split for 50 to unearned because the patient doesn't technically owe anything.  An income transfer needs to be ran instead.
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==50
				&& x.ProcNum==0
				&& x.UnearnedType>0));
		}

		///<summary>Adjustments that overpay procedures should be used to implicitly pay off anything.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_OverpaidProceduresNotUsedExplicitLinkingOnly() {
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum
				Create proc1:     Today     provNum  pat   $50
				Create adjust1:   Today-3D  provNum  pat  -$70
					^Attached to proc
				Create proc2:     Today     prov?    pat   $20
				Create payment:   Today-3D  prov?    pat   $50
					^Invalid payment (no pay splits) designed for autosplit logic.
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("ProvA");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",50,provNum:provNum);
			Adjustment adjust1=AdjustmentT.MakeAdjustment(pat.PatNum,-70,DateTime.Today.AddDays(-3),provNum:provNum,procNum:proc1.ProcNum);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",20);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,50,DateTime.Today.AddDays(-3));
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false,new PaymentEdit.LoadData());
			/*****************************************************
			AccountEntry:  Today  provNum  pat   $50
				^Represents proc1
			AccountEntry:  Today  provNum  pat  -$70
				^Represents adjust1 that is attached to proc1
			AccountEntry:  Today  prov?    pat   $20
				^Represents proc2
			******************************************************/
			Assert.AreEqual(3,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum
				&& x.AmountOriginal==50
				&& x.AmountEnd==-20//Implicit linking did not run so the $20 that was overpaid should remain.
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum
				&& x.AmountOriginal==-70
				&& x.AmountEnd==0//Directly applied to the associated procedure.
				&& x.Date==DateTime.Today.AddDays(-3)
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum!=provNum
				&& x.AmountOriginal==20
				&& x.AmountEnd==0//Auto-split logic will suggest paying this procedure.
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(2,results.ListAutoSplits.Count);
			//One split for 20 to proc2 because an income transfer needs to be made to correct this account that is in an invalid state.
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==20
				&& x.ProcNum==proc2.ProcNum
				&& x.UnearnedType==0));
			//One split for 30 to unearned for the income transfer system to deal with (the patient doesn't technically owe any money).
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==30
				&& x.ProcNum==0
				&& x.UnearnedType>0));
		}

		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_TransferredClaimsAreNotCountedTwice() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			List<long> listFamilyPatNums=Patients.GetFamily(pat.PatNum).ListPats.Select(x => x.PatNum).ToList();
			long provNum=ProviderT.CreateProvider("LS");
			Carrier carrier=CarrierT.CreateCarrier("BestCarrier");
			InsuranceInfo ins=InsuranceT.AddInsurance(pat,carrier.CarrierName);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,DateTime.Today,provNum:provNum);
			ins.AddBenefit(BenefitT.CreatePercentForProc(ins.PriPatPlan.PatPlanNum,proc.CodeNum,50));
			Claim claim=ClaimT.CreateClaim(new List<Procedure>{proc},ins);
			ins.ListAllClaimProcs=ClaimProcs.Refresh(ins.Pat.PatNum);
			ClaimT.ReceiveClaim(claim,ins.ListAllClaimProcs);
			//create new as total claim proc payment.
			ClaimProcT.AddInsPaidAsTotal(pat.PatNum,ins.PriInsPlan.PlanNum,provNum,65,ins.PriInsSub.InsSubNum,0,0,claim.ClaimNum);
			ins.ListAllClaimProcs=ClaimProcs.Refresh(ins.Pat.PatNum);
			Procedures.ComputeEstimatesForAll(pat.PatNum,ins.ListAllClaimProcs,ins.ListAllProcs,ins.ListInsPlans,ins.ListPatPlans,ins.ListBenefits,pat.Age,
				ins.ListInsSubs);
			List<ClaimProc> listValid=ClaimProcs.TransferClaimsAsTotalToProcedures(listFamilyPatNums).ListInsertedClaimProcs;
			Payment patPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,35);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>()
				,patPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplitData=PaymentEdit.AutoSplitForPayment(results);
			Assert.AreEqual(1,autoSplitData.ListAutoSplits.Count);
			Assert.AreEqual(1,autoSplitData.ListAutoSplits.FindAll(x => x.SplitAmt==35 && x.UnearnedType==0 && x.ProcNum==proc.ProcNum).Count);
		}

		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_TpProcsInPaymentWindowWhenDisplayCorrectlyAccordingToPref() {
			PrefT.UpdateInt(PrefName.PrePayAllowedForTpProcs,(int)YN.Yes);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov");
			Procedure treatPlanProc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.TP,"",50,provNum:provNum);
			Payment currentPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,50,isNew:true);
			PaymentEdit.ConstructResults constructData=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>{pat.PatNum},pat.PatNum
				,new List<PaySplit>(),currentPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplitData=PaymentEdit.AutoSplitForPayment(constructData);
			Assert.AreEqual(1,autoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(0,autoSplitData.ListAutoSplits.FindAll(x => x.ProcNum!=0).Count);//auto split should be for reg. unallocated, not the tp.
			//now do the same with the pref turned off. Account charge should not show in the list.
			PrefT.UpdateInt(PrefName.PrePayAllowedForTpProcs,(int)YN.No);
			constructData=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>{pat.PatNum},pat.PatNum,new List<PaySplit>(),currentPayment,
				new List<AccountEntry>());
			autoSplitData=PaymentEdit.AutoSplitForPayment(constructData);
			Assert.AreEqual(0,autoSplitData.ListAccountCharges.Count);
		}

		///<summary>Make sure that Amount begin, Amount start, and Amount end are correct after clicking and unclicking checkExplicitCreditsOnly.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_CheckExplicitCreditsOnly() {
			/*****************************************************
				Create Patient:  pat
				Create proc:      Today    prov?  pat   $100
				Create payment1:  Today-1  prov?  pat   $50
					^PaySplit for prov? and pat
				Create payment2:  Today    prov?  pat   $10
					^No splits (not a valid payment).  Designed for AutoSplitForPayment logic.
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",100);
			Payment payment1=PaymentT.MakePayment(pat.PatNum,50,DateTime.Now.AddDays(-1));
			Payment payment2=PaymentT.MakePaymentNoSplits(pat.PatNum,10);
			PaymentEdit.ConstructResults chargeResult=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum},pat.PatNum,
				new List<PaySplit>(),payment2,new List<AccountEntry>());//chargeResults for making autosplit
			PaymentEdit.AutoSplit autoSplit=PaymentEdit.AutoSplitForPayment(chargeResult);//Create autosplit for payment2
			autoSplit.ListSplitsCur.AddRange(autoSplit.ListAutoSplits);//Add auto splits to current splits like in PaymentEdit.Init()
			//Check explicit credits only box
			chargeResult.ListAccountCharges=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> { pat.PatNum },pat.PatNum,
				autoSplit.ListSplitsCur,autoSplit.Payment,new List<AccountEntry>(),doIncludeExplicitCreditsOnly: true).ListAccountCharges;
			/*****************************************************
			AccountEntry:  Today  prov?  pat
				^AmountOriginal = $100
				^AmountEnd      = $90  [$100 (proc) - $0 (no implicit) - $10 (auto split payment2)]
			******************************************************/
			Assert.AreEqual(100,chargeResult.ListAccountCharges[0].AmountOriginal);
			Assert.AreEqual(90,chargeResult.ListAccountCharges[0].AmountEnd);
			//Uncheck explicit credits only box
			chargeResult.ListAccountCharges=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> { pat.PatNum },pat.PatNum,
				autoSplit.ListSplitsCur,autoSplit.Payment,new List<AccountEntry>(),doIncludeExplicitCreditsOnly: false).ListAccountCharges;
			/*****************************************************
			AccountEntry:  Today  prov?  pat
				^AmountOriginal = $100
				^AmountEnd      = $40  [$100 (proc) - $50 (implicit payment1) - $10 (auto split payment2)]
			******************************************************/
			Assert.AreEqual(100,chargeResult.ListAccountCharges[0].AmountOriginal);
			Assert.AreEqual(40,chargeResult.ListAccountCharges[0].AmountEnd);
		}

		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_HiddenPaysplitsAreNotUsedInImplicitLinking() {
			/*****************************************************
				Create Patient:  pat
				Create procedure:       Today    prov?  pat   $100
				Create hiddenPayment:   Today-1  prov?  pat   $50
					^Attached to Unearned
				Create currentPayment:  Today    prov?  pat   $25
					^No splits (not a valid payment).  Designed for AutoSplitForPayment logic.
			******************************************************/
			Def def=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"hiddenType","x");
			PrefT.UpdateInt(PrefName.PrePayAllowedForTpProcs,(int)YN.Yes);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			//Make an unattached hidden payment that will be reserved for some treatment planned procedure down the line.
			Payment hiddenPayment=PaymentT.MakePayment(pat.PatNum,50,DateTime.Today.AddDays(-1),unearnedType:def.DefNum);
			//Make a completed procedure that we do not want linked to the payment we just made. 
			Procedure procedure=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"4",100);
			Payment currentPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,25,DateTime.Today);
			PaymentEdit.ConstructResults constructData=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>{pat.PatNum},pat.PatNum
				,new List<PaySplit>(),currentPayment,new List<AccountEntry>());
			PaymentEdit.AutoSplit autoSplitData=PaymentEdit.AutoSplitForPayment(constructData);
			/*****************************************************
			AccountEntry:  Today  prov?  pat
				^AmountOriginal = $100
				^AmountEnd      = $75  [$100 (proc) - $0 (no implicit) - $25 (auto split payment2)]
			******************************************************/
			Assert.AreEqual(1,autoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(1,autoSplitData.ListAutoSplits.Count);
			Assert.AreEqual(1,autoSplitData.ListAccountCharges.FindAll(x => x.AmountOriginal==100 && x.AmountEnd==75).Count);
			Assert.AreEqual(1,autoSplitData.ListAutoSplits.FindAll(x => x.SplitAmt==25 && x.UnearnedType==0).Count);
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanTest1() {
			/*****************************************************
				Proc1	Prov1	1000
				PP1		Prov2	500
					PPC1	Prov2	1000	Attached to Proc1
					PPC2	Prov2	-500	Unallocated
					PPD1	Prov2	500		Unallocated
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"PPTXFR1",ProcStat.C,"",1000,provNum:provNum1);
			PayPlan payPlan1=PayPlanT.CreatePayPlanNoCharges(pat.PatNum,500,DateTime.Today,provNum:provNum2);
			PayPlanCharge ppc1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				procNum:proc1.ProcNum,chargeType:PayPlanChargeType.Credit);
			PayPlanCharge ppc2=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,-500,provNum:provNum2,
				chargeType:PayPlanChargeType.Credit);
			PayPlanCharge ppd1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,500,provNum:provNum2,
				chargeType:PayPlanChargeType.Debit);
			//Update the 'Tx Completed Amt' so that the Account module knows how much money the patient actually owes.
			PayPlans.UpdateTreatmentCompletedAmt(new List<PayPlan>() { payPlan1 });
			//The patient owes $500 right now because ppc1 attached the entire amount for proc1 $1000 to payPlan1.
			//Then the office decided to randomly remove -$500 from payPlan1.  Thus, the patient only owes $500.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,5000,DateTime.Today);//Make a payment for more than what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $1000
				^Represents proc1
			FauxAccountEntry:  Today  provNum2  pat   $500
				^Represents ppd1
			******************************************************/
			Assert.AreEqual(2,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==1000
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1//Note that this ISN'T the provider associated to the payment plan but is the provider on the procedure.
				&& x.AmountOriginal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.AdjNum==0
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanChargeNum==ppd1.PayPlanChargeNum));
			Assert.AreEqual(2,results.ListAutoSplits.Count);
			//The patient owes provNum1 for proc1 even though provNum2 is associated to the payment plan (that technically has value right now).
			//The new payment system will detect this (hence the FauxAccountEntry should have provNum1 set) and suggest the payment be applied correctly.
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum1//Auto-split system should suggest paying the provider that deserves the money (no need for income transfer).
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0
				&& x.PayPlanChargeNum==ppd1.PayPlanChargeNum));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==4500
				&& x.ProvNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic to prove that no transfer is necessary.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanTest2() {
			/*****************************************************
				Proc1	Prov1	1000
				Proc2	Prov2	500
				PP1		Prov2	500
					PPC1	Prov2	1000	Attached to Proc1
					PPC2	Prov2	-500	Unallocated
					PPD1	Prov2	500		Unallocated
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"PPTXFR2",ProcStat.C,"",1000,provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"PPTXFR2",ProcStat.C,"",500,provNum:provNum2);
			PayPlan payPlan1=PayPlanT.CreatePayPlanNoCharges(pat.PatNum,500,DateTime.Today,provNum:provNum2);
			PayPlanCharge ppc1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				procNum:proc1.ProcNum,chargeType:PayPlanChargeType.Credit);
			PayPlanCharge ppc2=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,-500,provNum:provNum2,
				chargeType:PayPlanChargeType.Credit);
			PayPlanCharge ppd1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,500,provNum:provNum2,
				chargeType:PayPlanChargeType.Debit);
			//Update the 'Tx Completed Amt' so that the Account module knows how much money the patient actually owes.
			PayPlans.UpdateTreatmentCompletedAmt(new List<PayPlan>() { payPlan1 });
			//The patient owes $1000 right now because ppc1 attached the entire amount for proc1 $1000 to payPlan1.
			//Then the office decided to randomly remove -$500 from payPlan1.  Then another procedure was completed for $500.
			//Thus the patient owes $1000 due to the outstanding $500 on payPlan1 and $500 for proc2.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,5000,DateTime.Today);//Make a payment for more than what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $1000
				^Represents proc1
			AccountEntry:      Today  provNum2  pat   $500
				^Represents proc2
			FauxAccountEntry:  Today  provNum1  pat   $500
				^Represents ppd1 and proc1
			******************************************************/
			Assert.AreEqual(3,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==1000
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& x.AmountOriginal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1//Note that this ISN'T the provider associated to the payment plan but is the provider on the procedure.
				&& x.AmountOriginal==500
				&& x.AmountEnd==0
				&& x.Date==ppd1.ChargeDate
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanChargeNum==ppd1.PayPlanChargeNum));
			Assert.AreEqual(3,results.ListAutoSplits.Count);
			//The patient owes provNum1 for proc1 even though provNum2 is associated to the payment plan (that technically has value right now).
			//The new payment system will detect this (hence the FauxAccountEntry should have provNum1 set) and suggest the payment be applied correctly.
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum1//Auto-split system should suggest paying the provider that deserves the money (no need for income transfer).
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			//The patient also owes provNum2 for proc2.
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum2
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==4000
				&& x.ProvNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic to prove that no transfer is necessary.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanTest3() {
			/*****************************************************
				Proc1	Prov1	500
				Proc2	Prov2	1000
				PP1		Prov2	1500
					PPC1	Prov2	1000	Attached to Proc1
					PPC2	Prov2	-500	Unallocated
					PPC3	Prov2	1000	Attached to Proc2
					PPD1	Prov2	1500	Unallocated
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"PPTXFR3",ProcStat.C,"",500,provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"PPTXFR3",ProcStat.C,"",1000,provNum:provNum2);
			PayPlan payPlan1=PayPlanT.CreatePayPlanNoCharges(pat.PatNum,1500,DateTime.Today,provNum:provNum2);
			PayPlanCharge ppc1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				procNum:proc1.ProcNum,chargeType:PayPlanChargeType.Credit);
			PayPlanCharge ppc2=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,-500,provNum:provNum2,
				chargeType:PayPlanChargeType.Credit);
			PayPlanCharge ppc3=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				procNum:proc2.ProcNum,chargeType:PayPlanChargeType.Credit);
			PayPlanCharge ppd1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1500,provNum:provNum2,
				chargeType:PayPlanChargeType.Debit);
			//Update the 'Tx Completed Amt' so that the Account module knows how much money the patient actually owes.
			PayPlans.UpdateTreatmentCompletedAmt(new List<PayPlan>() { payPlan1 });
			//The patient owes $1500 right now because:
			//ppc1 attached more than the entire amount of proc1 to payPlan1 => (proc1 $500, ppc1 $1000)
			//ppc2 randomly removed -$500 from payPlan1 (probably to offset ppc1 overcharging)
			//ppc3 attaches the entire amount of proc2 => (proc2 $1000)
			//Thus the patient owes $1500 which just happens to be exactly what the procedures were originally worth.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,5000,DateTime.Today);//Make a payment for more than what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $500
				^Represents proc1
			AccountEntry:      Today  provNum2  pat   $1000
				^Represents proc2
			FauxAccountEntry:  Today  provNum1  pat   $1500
				^Represents ppd1 and proc1
			FauxAccountEntry:  Today  provNum2  pat   $1500
				^Represents ppd1 and proc2
			******************************************************/
			Assert.AreEqual(4,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& x.AmountOriginal==1000
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			//That's right, the one debit will get allocated to several credits. This will happen often in patient payment plans.
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1//Note that this ISN'T the provider associated to the payment plan but is the provider on the procedure.
				&& ((FauxAccountEntry)x).Principal==1000//Note that this is MORE than what proc1 is worth.
				&& x.AmountEnd==0
				&& x.Date==ppd1.ChargeDate
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanChargeNum==ppd1.PayPlanChargeNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& ((FauxAccountEntry)x).Principal==500
				&& x.AmountEnd==0
				&& x.Date==ppd1.ChargeDate
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanChargeNum==ppd1.PayPlanChargeNum));
			Assert.AreEqual(3,results.ListAutoSplits.Count);
			//The auto-split system is going to honor the strange overallocation of proc1 in regards to ppc1 (proc1 $500, ppc1 $1000).
			//One might think that the system should have suggested paying only $500 to proc1 but in "payment plan land" proc1 is worth $1000.
			//This is because the unattached ppc2 of -$500 gets associated to ppc3 which matches the pat/prov/clinic combination for provNum2!
			//Even though payPlan1 started off with $1000 for provNum2, the unattached charge of -$500 will remove from provNum2 and voila, $500.
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==1000
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum2
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==3500
				&& x.ProvNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic to prove that no transfer is necessary.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanTest4() {
			/*****************************************************
				Proc1	Prov1	1000
				PP1		Prov2	500
					PPC1	Prov2	1000	Tx Credit Attached to Proc1
					PPD1	Prov2	500	Create Schedule Debit (manually edited from $1000 to $500)
					PPD2	Prov2	-500	Adj Debit
					PPC2	Prov2	-500	Adj Credit (offset)
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"PPTXFR4",ProcStat.C,"",1000,provNum:provNum1);
			PayPlan payPlan1=PayPlanT.CreatePayPlanNoCharges(pat.PatNum,500,DateTime.Today,provNum:provNum2);
			PayPlanCharge ppc1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				procNum:proc1.ProcNum,chargeType:PayPlanChargeType.Credit,doInsert:false);
			PayPlanCharge ppd1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,500,provNum:provNum2,
				chargeType:PayPlanChargeType.Debit,doInsert:false);
			List<PayPlanCharge> listPaySplits=PayPlanEdit.CreatePayPlanAdjustments(-500,new List<PayPlanCharge>() { ppc1,ppd1 },0);
			//For whatever reason, the Payment Plan window will create an offsetting Credit for the adjustment charge.
			listPaySplits.Add(PayPlanChargeT.CreateNegativeCreditForAdj(pat.PatNum,payPlan1.PayPlanNum,-500,provNum:provNum2));
			PayPlanCharges.InsertMany(listPaySplits);
			//Update the 'Tx Completed Amt' so that the Account module knows how much money the patient actually owes.
			PayPlans.UpdateTreatmentCompletedAmt(new List<PayPlan>() { payPlan1 });
			//Now we come to the predicament of figuring out how much the patient owes.
			//Technically the patient owes $500.  The strange part is that the patient owes $500 to proc1.
			//This is because the entirety of proc1 was attached to payPlan1 (even though payPlay1 was only for $500 total in the end).
			//So $1000 of production was removed from proc1 and applied to payPlan1.
			//Then payPlan1 evaporates $500 of the overall payment plan worth by manually editing ppd1 to $500, for whatever reason.
			//Then the user makes a payment plan adjustment which just so happens to be for the entire payment plan amount, -$500.
			//This pushes $500 of value back to proc1 but payPlan1 was only worth $500 (manually set) and then adjusted to $0,
			//so the patient owes nothing to the payment plan and will only owe $500 to proc1 for the rest of their lives.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,5000,DateTime.Today);//Make a payment for more than what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $1000
				^Represents proc1
			FauxAccountEntry:  Today  provNum2  pat   $500
				^Represents ppd1 and proc1
			FauxAccountEntry:  Today  provNum2  pat  -$500
				^Represents ppd2 and adj
			******************************************************/
			Assert.AreEqual(3,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==1000
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& ((FauxAccountEntry)x).Principal==500
				&& x.AmountEnd==0
				&& x.Date==ppd1.ChargeDate
				&& x.ProcNum==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& ((FauxAccountEntry)x).IsAdjustment
				&& ((FauxAccountEntry)x).ChargeType==PayPlanChargeType.Debit
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& ((FauxAccountEntry)x).Principal==-500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0));
			Assert.AreEqual(2,results.ListAutoSplits.Count);
			//The payment plan did shenanigans and the patient owes $500 to proc1 due to the adjustments.
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==4500
				&& x.ProvNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.PayPlanChargeNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic to prove that no transfer is necessary.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			//An income transfer cannot be made because payPlan1 has a 'Total Tx Amt' that does not match the 'Total Amount'.
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanTest5() {
			/*****************************************************
				Proc1	Prov1	1000
				Proc2	Prov2	500
				PP1		Prov2	500
					PPC1	Prov2	1000	Tx Credit Attached to Proc1
					PPD1	Prov2	1000	Create Schedule Debit
					PPD2	Prov2	-500	Adj Debit
					PPC2	Prov2	-500	Adj Credit (offset)
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"PPTXFR5",ProcStat.C,"",1000,provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"PPTXFR5",ProcStat.C,"",500,provNum:provNum2);
			PayPlan payPlan1=PayPlanT.CreatePayPlanNoCharges(pat.PatNum,500,DateTime.Today,provNum:provNum2);
			PayPlanCharge ppc1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				procNum:proc1.ProcNum,chargeType:PayPlanChargeType.Credit,doInsert:false);
			PayPlanCharge ppd1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				chargeType:PayPlanChargeType.Debit,doInsert:false);
			List<PayPlanCharge> listPaySplits=PayPlanEdit.CreatePayPlanAdjustments(-500,new List<PayPlanCharge>() { ppc1,ppd1 },0);
			//For whatever reason, the Payment Plan window will create an offsetting Credit for the adjustment charge.
			listPaySplits.Add(PayPlanChargeT.CreateNegativeCreditForAdj(pat.PatNum,payPlan1.PayPlanNum,-500,provNum:provNum2));
			PayPlanCharges.InsertMany(listPaySplits);
			//Update the 'Tx Completed Amt' so that the Account module knows how much money the patient actually owes.
			PayPlans.UpdateTreatmentCompletedAmt(new List<PayPlan>() { payPlan1 });
			//Now we come to the predicament of figuring out how much the patient owes.
			//Technically the patient only owes $1500, of which $500 is for proc1, $500 is for payPlan1/proc1, and $500 is for proc2.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,5000,DateTime.Today);//Make a payment for what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $1000
				^Represents proc1
			AccountEntry:      Today  provNum2  pat   $500
				^Represents proc2
			FauxAccountEntry:  Today  provNum2  pat   $1000
				^Represents ppd1 and proc1
			FauxAccountEntry:  Today  provNum2  pat  -$500
				^Represents ppd1 and adj
			******************************************************/
			Assert.AreEqual(4,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==1000
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& x.AmountOriginal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1//Note that this ISN'T the provider associated to the payment plan but is the provider on the procedure.
				&& ((FauxAccountEntry)x).Principal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& ((FauxAccountEntry)x).IsAdjustment
				&& ((FauxAccountEntry)x).ChargeType==PayPlanChargeType.Debit
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& ((FauxAccountEntry)x).Principal==-500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0));
			//The patient owes $500 to proc1/provNum1/payPlan1, $500 to proc1/provNum1, and $500 proc2/provNum2.
			Assert.AreEqual(4,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum2
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==3500
				&& x.ProvNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanTest6() {
			/*****************************************************
				Proc1	Prov1	500
				Proc2	Prov2	1000
				PP1		Prov2	1500
					PPC1	Prov2	1000	Attached to Proc1
					PPC2	Prov2	-500	Unallocated
					PPC3	Prov2	1000	Attached to Proc2
					PPD1	Prov2	1500	Unallocated
					PPD2	Prov2	-500	Unallocated
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"PPTXFR6",ProcStat.C,"",500,provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"PPTXFR6",ProcStat.C,"",1000,provNum:provNum2);
			PayPlan payPlan1=PayPlanT.CreatePayPlanNoCharges(pat.PatNum,1500,DateTime.Today,provNum:provNum2);
			PayPlanCharge ppc1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				procNum:proc1.ProcNum,chargeType:PayPlanChargeType.Credit,doInsert:false);
			PayPlanCharge ppc3=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				procNum:proc2.ProcNum,chargeType:PayPlanChargeType.Credit,doInsert:false);
			PayPlanCharge ppd1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1500,provNum:provNum2,
				chargeType:PayPlanChargeType.Debit,doInsert:false);
			List<PayPlanCharge> listPaySplits=PayPlanEdit.CreatePayPlanAdjustments(-500,new List<PayPlanCharge>() { ppc1,ppc3,ppd1 },0);
			//For whatever reason, the Payment Plan window will create an offsetting Credit for the adjustment charge.
			listPaySplits.Add(PayPlanChargeT.CreateNegativeCreditForAdj(pat.PatNum,payPlan1.PayPlanNum,-500,provNum:provNum2));
			PayPlanCharges.InsertMany(listPaySplits);
			//Update the 'Tx Completed Amt' so that the Account module knows how much money the patient actually owes.
			PayPlans.UpdateTreatmentCompletedAmt(new List<PayPlan>() { payPlan1 });
			//This is a crazy scenario.  The payment plan should technically be worth $2000 and an income transfer is not allowed because it is $1500.
			//The user has chosen to make proc1 worth $1000 (incorrect) and proc2 also worth $1000 (total of $2K value) via the payment plan.
			//They then notice the error of their ways but instead of correcting the credit attached to proc1 they just adjust the entire plan -$500.
			//This is terrible because it simply brings the entire value of the payment plan down (notice it is for a different provider than proc1!).
			//The construct and link charges system will remove $500 from a random credit (preferring ones that are attached to procedures).
			//Therefore, payPlan1/proc1 will only be worth $500, payPlan1/proc1 will remain at $1000 but now the plan is worth a total of $1500.
			//The adjustment will get applied to proc1 which will inflate its value to $500 (oh wow, $500 directly to the proc and $500 on payPlan).
			//Therefore, the user owes $500 to proc1/provNum1, $500 to proc1/provNum1/payPlan1, and $500 to proc2/provNum2/payPlan1.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,5000,DateTime.Today);//Make a payment for more than what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $500
				^Represents proc1
			AccountEntry:      Today  provNum2  pat   $1000
				^Represents proc2
			FauxAccountEntry:  Today  provNum2  pat   $500
				^Represents ppd1 and proc1
			FauxAccountEntry:  Today  provNum2  pat   $500
				^Represents ppd1 and proc2
			FauxAccountEntry:  Today  provNum2  pat  -$500
				^Represents ppd2 and adj
			******************************************************/
			Assert.AreEqual(5,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& x.AmountOriginal==1000
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1//Note that this ISN'T the provider associated to the payment plan but is the provider on the procedure.
				&& ((FauxAccountEntry)x).Principal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& ((FauxAccountEntry)x).Principal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& ((FauxAccountEntry)x).IsAdjustment
				&& ((FauxAccountEntry)x).ChargeType==PayPlanChargeType.Debit
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& x.AmountOriginal==-500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0));
			//The patient owes $500 to proc1/provNum1, $500 proc1/provNum1/payPlan1, and $500 to proc2/provNum2/payPlan1.
			Assert.AreEqual(4,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum2
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==3500
				&& x.ProvNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			//An income transfer cannot be made because payPlan1 has a 'Total Tx Amt' that does not match the 'Total Amount'.
			//The auto-split logic doesn't care why provNum1 was magically paid $1000 just for being associated to payPlan1.
			//The income transfer system will think that provNum1 deserves the $1000 due to the tx credit on payPlan1 saying so.
			//Therefore, nothing would need to happen if an income transfer was made.
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanTest7() {
			/*****************************************************
				Proc1	Prov1	1000
				PP1		Prov2	500
					PPC1	Prov2	1000	Attached to Proc1
					PPC2	Prov2	-500	Unallocated
					PPD1	Prov2	500	Unallocated
					PPD2	Prov2	-500	Unallocated
				Adj		Prov2	-500
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"PPTXFR7",ProcStat.C,"",1000,provNum:provNum1);
			PayPlan payPlan1=PayPlanT.CreatePayPlanNoCharges(pat.PatNum,500,DateTime.Today,provNum:provNum2);
			PayPlanCharge ppc1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				procNum:proc1.ProcNum,chargeType:PayPlanChargeType.Credit,doInsert:false);
			PayPlanCharge ppd1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,500,provNum:provNum2,
				chargeType:PayPlanChargeType.Debit,doInsert:false);
			List<PayPlanCharge> listPaySplits=PayPlanEdit.CreatePayPlanAdjustments(-500,new List<PayPlanCharge>() { ppc1,ppd1 },0);
			listPaySplits.Add(PayPlanChargeT.CreateNegativeCreditForAdj(pat.PatNum,payPlan1.PayPlanNum,-500,provNum:provNum2));
			PayPlanCharges.InsertMany(listPaySplits);
			//Update the 'Tx Completed Amt' so that the Account module knows how much money the patient actually owes.
			PayPlans.UpdateTreatmentCompletedAmt(new List<PayPlan>() { payPlan1 });
			//Now make the adjustment to the account.
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,-500,provNum:provNum2);
			//The patient owes $0 because their debits do not equate to their credits on the payment plan.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,5000,DateTime.Today);//Make a payment for what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $1000
				^Represents proc1
			FauxAccountEntry:  Today  provNum2  pat   $1000
				^Represents ppd1 and proc1
			FauxAccountEntry:  Today  provNum2  pat   $1000
				^Represents ppd2 and adj
			AccountEntry:      Today  provNum2  pat  -$500
				^Represents adj1
			******************************************************/
			Assert.AreEqual(4,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==1000
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2//Note that this is the provider associated to the payment plan and not proc1. Credits and debits don't equate.
				&& ((FauxAccountEntry)x).Principal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& ((FauxAccountEntry)x).IsAdjustment
				&& ((FauxAccountEntry)x).ChargeType==PayPlanChargeType.Debit
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& ((FauxAccountEntry)x).Principal==-500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& x.AmountOriginal==-500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0));
			//The patient owes $0 so everything will go to unearned.
			Assert.AreEqual(1,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==5000
				&& x.ProvNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			//An income transfer cannot be made because payPlan1 has a 'Total Tx Amt' that does not match the 'Total Amount'.
			//There is no positive production anywhere on the account, the income transfer system will have nothing to do anyway.
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanTest8() {
			/*****************************************************
				Proc1	Prov1	1000
				Proc2	Prov2	500
				PP1		Prov2	500
					PPC1	Prov2	1000	Attached to Proc1
					PPC2	Prov2	-500	Unallocated
					PPD1	Prov2	1000	Unallocated
					PPD2	Prov2	-500	Unallocated
				Adj		Prov2	-500
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"PPTXFR8",ProcStat.C,"",1000,provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"PPTXFR8",ProcStat.C,"",500,provNum:provNum2);
			PayPlan payPlan1=PayPlanT.CreatePayPlanNoCharges(pat.PatNum,500,DateTime.Today,provNum:provNum2);
			PayPlanCharge ppc1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				procNum:proc1.ProcNum,chargeType:PayPlanChargeType.Credit,doInsert:false);
			PayPlanCharge ppd1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				chargeType:PayPlanChargeType.Debit,doInsert:false);
			List<PayPlanCharge> listPaySplits=PayPlanEdit.CreatePayPlanAdjustments(-500,new List<PayPlanCharge>() { ppc1,ppd1 },0);
			//For whatever reason, the Payment Plan window will create an offsetting Credit for the adjustment charge.
			listPaySplits.Add(PayPlanChargeT.CreateNegativeCreditForAdj(pat.PatNum,payPlan1.PayPlanNum,-500,provNum:provNum2));
			PayPlanCharges.InsertMany(listPaySplits);
			//Update the 'Tx Completed Amt' so that the Account module knows how much money the patient actually owes.
			PayPlans.UpdateTreatmentCompletedAmt(new List<PayPlan>() { payPlan1 });
			//Now make the adjustment to the account.
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,-500,provNum:provNum2);
			//The payment plan has an outstanding value of $500.
			//The entirety of proc1 was put onto payPlan1 but then the payment plan was adjusted by -$500 which gave value back to proc1.
			//Now proc1 is split between the regular account and payPlan1.
			//The procedure for provNum2 is still owed $500 but that value evaporates due to the -$500 adjustment on the actual account.
			//The only way that provNum2 will get the money they deserve is if an income transfer is performed.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,5000,DateTime.Today);//Make a payment for more than what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $1000
				^Represents proc1
			AccountEntry:      Today  provNum2  pat   $500
				^Represents proc2
			FauxAccountEntry:  Today  provNum2  pat   $1000
				^Represents ppd1 and proc1
			FauxAccountEntry:  Today  provNum2  pat  -$500
				^Represents ppd2 and adj
			AccountEntry:      Today  provNum2  pat  -$500
				^Represents adj1
			******************************************************/
			Assert.AreEqual(5,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==1000
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& x.AmountOriginal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1//Note that this ISN'T the provider associated to the payment plan but is the provider on the procedure.
				&& ((FauxAccountEntry)x).Principal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& ((FauxAccountEntry)x).IsAdjustment
				&& ((FauxAccountEntry)x).ChargeType==PayPlanChargeType.Debit
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& ((FauxAccountEntry)x).Principal==-500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& x.AmountOriginal==-500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0));
			//The patient owes $1000, $500 towards provNum1/proc1/payPlan1 and $500 directly towards provNum1/proc1.
			Assert.AreEqual(3,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==4000
				&& x.ProvNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			//The auto-split system didn't pay what proc2 was actually owed (due to implicit linking).
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.SplitAmt==-500
				&& x.ProvNum==provNum2
				&& x.AdjNum==adj1.AdjNum
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum2
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum2
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.SplitAmt==-500
				&& x.ProvNum==provNum2
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic.
			transferResults.ListSplitsCur.ForEach(x => PaySplits.Insert(x));
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanTest9() {
			/*****************************************************
				Proc1	Prov1	500
				Proc2	Prov2	1000
				PP1		Prov2	1500
					PPC1	Prov2	1000	Attached to Proc1
					PPC2	Prov2	-500	Unallocated
					PPC3	Prov2	1000	Attached to Proc2
					PPD1	Prov2	1500	Unallocated
					PPD2	Prov2	-500	Unallocated
				Adj		Prov2	-500
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"PPTXFR9",ProcStat.C,"",500,provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"PPTXFR9",ProcStat.C,"",1000,provNum:provNum2);
			PayPlan payPlan1=PayPlanT.CreatePayPlanNoCharges(pat.PatNum,1500,DateTime.Today,provNum:provNum2);
			PayPlanCharge ppc1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				procNum:proc1.ProcNum,chargeType:PayPlanChargeType.Credit,doInsert:false);
			PayPlanCharge ppc3=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1000,provNum:provNum2,
				procNum:proc2.ProcNum,chargeType:PayPlanChargeType.Credit,doInsert:false);
			PayPlanCharge ppd1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1500,provNum:provNum2,
				chargeType:PayPlanChargeType.Debit,doInsert:false);
			List<PayPlanCharge> listPaySplits=PayPlanEdit.CreatePayPlanAdjustments(-500,new List<PayPlanCharge>() { ppc1,ppc3,ppd1 },0);
			//For whatever reason, the Payment Plan window will create an offsetting Credit for the adjustment charge.
			listPaySplits.Add(PayPlanChargeT.CreateNegativeCreditForAdj(pat.PatNum,payPlan1.PayPlanNum,-500,provNum: provNum2));
			PayPlanCharges.InsertMany(listPaySplits);
			//Update the 'Tx Completed Amt' so that the Account module knows how much money the patient actually owes.
			PayPlans.UpdateTreatmentCompletedAmt(new List<PayPlan>() { payPlan1 });
			//Now make the adjustment to the account.
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,-500,provNum:provNum2);
			//The patient owes $1000 to the payment plan.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,5000,DateTime.Today);//Make a payment for more than what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $500
				^Represents proc1
			AccountEntry:      Today  provNum2  pat   $1000
				^Represents proc2
			FauxAccountEntry:  Today  provNum2  pat   $1000
				^Represents ppd1 and proc1
			FauxAccountEntry:  Today  provNum2  pat   $1000
				^Represents ppd1 and proc2
			FauxAccountEntry:  Today  provNum2  pat  -$500
				^Represents ppd2 and adj
			AccountEntry:      Today  provNum2  pat  -$500
				^Represents adj1
			******************************************************/
			Assert.AreEqual(6,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& x.AmountOriginal==1000
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1//Note that this ISN'T the provider associated to the payment plan but is the provider on the procedure.
				&& ((FauxAccountEntry)x).Principal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& ((FauxAccountEntry)x).Principal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& ((FauxAccountEntry)x).IsAdjustment
				&& ((FauxAccountEntry)x).ChargeType==PayPlanChargeType.Debit
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& ((FauxAccountEntry)x).Principal==-500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& x.AmountOriginal==-500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0));
			//The user created a payment plan and made it worth $1500 yet they adjusted it by -$500 thus making it only worth $1000 total.
			//This -$500 adjustment will come out of the payment plan and get directly applied to proc1 (or another random credit associated to a proc).
			//At this point, proc1 is worth $500, proc1/payPlan1 is worth $500, and proc2/payPlan1 is worth $500
			//But then the office makes an adjustment on the account for -$500 to provNum2.
			//Due to implicit linking, the -$500 adjustment gets applied to proc1 and what is left is proc1/payPlan1 and proc2/payPlan1.
			//What a terrible chain of events...
			Assert.AreEqual(3,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum2
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==4000
				&& x.ProvNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			//An income transfer cannot be made because payPlan1 has a 'Total Tx Amt' that does not match the 'Total Amount'.
		}

		///<summary>The auto-split system should not suggest making a PaySplit that is attached to both a procedure and an adjustment.</summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_SplitsAttachedToProcedureOrAdjustment() {
			//The PaySplits that the auto-split system suggests should not be attached to both a procedure and an adjustment.
			//Make a procedure for provNum1 and an adjustment for provNum2 that is attached to the procedure.
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"TASTA",ProcStat.C,"",100,procDate:DateTime.Today,provNum:provNum1);
			Adjustment adjust1=AdjustmentT.MakeAdjustment(pat.PatNum,10,DateTime.Today,procNum:proc1.ProcNum,provNum:provNum2);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,200,DateTime.Today);//Make a payment for what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			Assert.AreEqual(3,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==100
				&& x.ProvNum==provNum1
				&& x.AdjNum==0
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==10
				&& x.ProvNum==provNum2
				&& x.AdjNum==adjust1.AdjNum
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==90
				&& x.ProvNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
		}

		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanWithAPR() {
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum
				Create proc:     Today-2Y  provNum  pat   $1568
				Create payplan:  Today-2Y  provNum  pat   $1568
					^Associated to proc, APR=10, payment amount=$269.01, total amount=$1614.04 (including interest)
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum=ProviderT.CreateProvider(suffix);
			DateTime dateTimePP=DateTime.Today.AddYears(-2);
			Procedure proc=ProcedureT.CreateProcedure(pat,"TPPAPR",ProcStat.C,"",1568,dateTimePP,provNum:provNum);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,269.01,dateTimePP,provNum:provNum,
				listProcs:new List<Procedure>(){ proc },APR:10);
			List<PayPlanCharge> listPayPlanCharges=PayPlanCharges.GetDueForPayPlan(payplan,pat.PatNum);
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Now,true,0,1);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,2000,DateTime.Today);//Make a payment for what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			/*****************************************************
			PaySplit:  Today  provNum  pat   $255.94
				^Payment for month #1 principal
			PaySplit:  Today  provNum  pat   $13.07
				^Payment for month #1 interest
			PaySplit:  Today  provNum  pat   $258.08
				^Payment for month #2 principal
			PaySplit:  Today  provNum  pat   $10.93
				^Payment for month #2 interest
			PaySplit:  Today  provNum  pat   $260.23
				^Payment for month #3 principal
			PaySplit:  Today  provNum  pat   $8.78
				^Payment for month #3 interest
			PaySplit:  Today  provNum  pat   $262.40
				^Payment for month #4 principal
			PaySplit:  Today  provNum  pat   $6.61
				^Payment for month #4 interest
			PaySplit:  Today  provNum  pat   $264.58
				^Payment for month #5 principal
			PaySplit:  Today  provNum  pat   $4.43
				^Payment for month #5 interest
			PaySplit:  Today  provNum  pat   $266.77
				^Payment for month #6 principal
			PaySplit:  Today  provNum  pat   $2.22
				^Payment for month #6 interest
			PaySplit:  Today     0     pat   $385.96
				^Prepayment because the patient overpaid.
			******************************************************/
			Assert.AreEqual(13,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==255.94
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==13.07
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.PayPlanChargeNum==listPayPlanCharges.First(y => y.Interest==13.07).PayPlanChargeNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==258.08
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==10.93
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.PayPlanChargeNum==listPayPlanCharges.First(y => y.Interest==10.93).PayPlanChargeNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==260.23
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==8.78
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.PayPlanChargeNum==listPayPlanCharges.First(y => y.Interest==8.78).PayPlanChargeNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==262.40
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==6.61
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.PayPlanChargeNum==listPayPlanCharges.First(y => y.Interest==6.61).PayPlanChargeNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==264.58
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==4.43
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.PayPlanChargeNum==listPayPlanCharges.First(y => y.Interest==4.43).PayPlanChargeNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==266.77
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==2.22
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.PayPlanChargeNum==listPayPlanCharges.First(y => y.Interest==2.22).PayPlanChargeNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(385.96)
				&& x.ProvNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic to prove that no transfer is necessary.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanProcCreditWithAdj() {
			/*****************************************************
				Proc1	provNum1	1000
				PP1		provNum1	1250
					PPC1	provNum1	1250	Attached to Proc1
				Adj1	provNum1	250
					^attached to Proc1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"TPPPCWA",ProcStat.C,"",1000,provNum:provNum1);
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,250,procNum:proc1.ProcNum,provNum:provNum1);
			PayPlan payPlan1=PayPlanT.CreatePayPlanNoCharges(pat.PatNum,1250,DateTime.Today,provNum:provNum1);
			//Make a manual PayPlanCharge credit which will be for the sum of proc1 + adj1 (we suggest this to users via the UI).
			PayPlanCharge ppc1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1250,provNum:provNum1,
				chargeType:PayPlanChargeType.Credit,procNum:proc1.ProcNum);
			//Make only $500 of the payment plan due right now.
			PayPlanCharge ppd1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,500,provNum:provNum1,
				chargeType:PayPlanChargeType.Debit);
			//Make some other debits that don't matter because they will be in the future.
			//The only thing that matters is the grand total of all debits must equate to $1250 so that we can execute the income transfer at the end.
			PayPlanCharge ppd2=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today.AddMonths(1),500,provNum:provNum1,
				chargeType:PayPlanChargeType.Debit);
			PayPlanCharge ppd3=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today.AddMonths(2),250,provNum:provNum1,
				chargeType:PayPlanChargeType.Debit);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,2000,DateTime.Today);//Make a payment for more than what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $1000
				^Represents proc1
			FauxAccountEntry:  Today     provNum1  pat   $500
				^Represents ppd1 and proc1
			FauxAccountEntry:  Today+1M  provNum1  pat   $500
				^Represents ppd2 and proc1
			FauxAccountEntry:  Today+2M  provNum1  pat   $250
				^Represents ppd3 and proc1
			AccountEntry:      Today     provNum1  pat   $250
				^Represents adj1
			******************************************************/
			Assert.AreEqual(5,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==1000
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==500
				&& x.AmountEnd==0
				&& x.Date==ppd1.ChargeDate
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanChargeNum==ppd1.PayPlanChargeNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==500
				&& x.AmountEnd==0
				&& x.Date==ppd2.ChargeDate
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanChargeNum==ppd2.PayPlanChargeNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==250
				&& x.AmountEnd==0
				&& x.Date==ppd3.ChargeDate
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanChargeNum==ppd3.PayPlanChargeNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==250
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(2,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum1
				&& x.AdjNum==0
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.PayPlanChargeNum==ppd1.PayPlanChargeNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==1500
				&& x.ProvNum==0
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//The patient technically owes $500 because the office correctly created a manual adjustment that matches the pat/prov/clinic of proc1.
			//Insert the suggested PaySplit(s) and then perform the income transfer logic to prove that no transfer is necessary.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanProcCreditWithAdjDiffProv() {
			/*****************************************************
				Proc1	provNum1	1000
				PP1		provNum1	1250
					PPC1	provNum1	1250	Attached to Proc1
				Adj1	provNum2	250
					^attached to Proc1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"TPPPCWA",ProcStat.C,"",1000,provNum:provNum1);
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,250,procNum:proc1.ProcNum,provNum:provNum2);
			PayPlan payPlan1=PayPlanT.CreatePayPlanNoCharges(pat.PatNum,1250,DateTime.Today,provNum:provNum1);
			//Make a manual PayPlanCharge credit which will be for the sum of proc1 + adj1 (we suggest this to users via the UI).
			PayPlanCharge ppc1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1250,provNum:provNum1,
				chargeType:PayPlanChargeType.Credit,procNum:proc1.ProcNum);
			//Make only $500 of the payment plan due right now.
			PayPlanCharge ppd1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,500,provNum:provNum1,
				chargeType:PayPlanChargeType.Debit);
			//Make some other debits that don't matter because they will be in the future.
			//The only thing that matters is the grand total of all debits must equate to $1250 so that we can execute the income transfer at the end.
			PayPlanCharge ppd2=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today.AddMonths(1),500,provNum:provNum1,
				chargeType:PayPlanChargeType.Debit);
			PayPlanCharge ppd3=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today.AddMonths(2),250,provNum:provNum1,
				chargeType:PayPlanChargeType.Debit);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,2000,DateTime.Today);//Make a payment for more than what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $1000
				^Represents proc1
			FauxAccountEntry:     Today     provNum1  pat   $500
				^Represents ppd1 and proc1
			FauxAccountEntry:     Today+1M  provNum1  pat   $500
				^Represents ppd2 and proc1
			FauxAccountEntry:     Today+2M  provNum1  pat   $250
				^Represents ppd3 and proc1
			AccountEntry:         Today     provNum2  pat   $250
				^Represents adj1
			******************************************************/
			Assert.AreEqual(5,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==1000
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==500
				&& x.AmountEnd==0
				&& x.Date==ppd1.ChargeDate
				&& x.AdjNum==0
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanChargeNum==ppd1.PayPlanChargeNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==500
				&& x.AmountEnd==0
				&& x.Date==ppd2.ChargeDate
				&& x.AdjNum==0
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanChargeNum==ppd2.PayPlanChargeNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==250
				&& x.AmountEnd==0
				&& x.Date==ppd3.ChargeDate
				&& x.AdjNum==0
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanChargeNum==ppd3.PayPlanChargeNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& x.AmountOriginal==250
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			//The patient technically owes $750 because the office messed up on the manual adjustment that they created.
			//$500 to provNum1 payPlan1 and $250 to provNum2 adj1.
			Assert.AreEqual(3,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==500
				&& x.ProvNum==provNum1
				&& x.AdjNum==0
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.PayPlanChargeNum==ppd1.PayPlanChargeNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==250
				&& x.ProvNum==provNum2
				&& x.AdjNum==adj1.AdjNum
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==1250
				&& x.ProvNum==0
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic to prove that no transfer is necessary.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanPreviousPaymentWithAdjDynamic() {
			/*****************************************************
				proc1   Today  provNum1   $139.10
				proc2   Today  provNum1   $483.00
				proc3   Today  provNum2   $174.80
				adj1    Today  provNum1   $8.35
					^Attached to proc1
				adj2    Today  provNum1   $3.10
				adj3    Today  provNum1   $50.00
				pay1    Today  provNum1   $41.35
					^Attached to 5 of the 6 entries above, splits are as follows:
				split1  Today  provNum1   $7.45
					^Attached to proc1
				split2  Today  provNum1   $3.00
					^Attached to proc2
				split3  Today  provNum2   $4.80
					^Attached to proc3
				split4  Today  provNum1   $1.10
					^Attached to adj2
				split5  Today  provNum1   $25.00
					^Attached to adj3
				payPlan1  Today  provNum1   $817.00
					^Attached to proc1, proc2, proc3, adj2, adj3
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"TPPPPWAD1",ProcStat.C,"",139.10,provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"TPPPPWAD2",ProcStat.C,"",483.00,provNum:provNum1);
			Procedure proc3=ProcedureT.CreateProcedure(pat,"TPPPPWAD3",ProcStat.C,"",174.80,provNum:provNum2);
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,8.35,procNum:proc1.ProcNum,provNum:provNum1);
			Adjustment adj2=AdjustmentT.MakeAdjustment(pat.PatNum,3.10,provNum:provNum1);
			Adjustment adj3=AdjustmentT.MakeAdjustment(pat.PatNum,50.00,provNum:provNum1);
			Payment pay1=PaymentT.MakePaymentNoSplits(pat.PatNum,41.35,isNew:true);
			PaySplit split1=PaySplitT.CreateOne(pat.PatNum,7.45,pay1.PayNum,provNum1,procNum:proc1.ProcNum);
			PaySplit split2=PaySplitT.CreateOne(pat.PatNum,3.00,pay1.PayNum,provNum1,procNum:proc2.ProcNum);
			PaySplit split3=PaySplitT.CreateOne(pat.PatNum,4.80,pay1.PayNum,provNum2,procNum:proc3.ProcNum);
			PaySplit split4=PaySplitT.CreateOne(pat.PatNum,1.10,pay1.PayNum,provNum1,adjNum:adj2.AdjNum);
			PaySplit split5=PaySplitT.CreateOne(pat.PatNum,25.00,pay1.PayNum,provNum1,adjNum:adj3.AdjNum);
			PayPlan payPlan1=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,pat.Guarantor,DateTime.Today,0,0,204.25,
				new List<Procedure>(){ proc1,proc2,proc3 },
				new List<Adjustment>(){ adj2,adj3 },
				frequency:PayPlanFrequency.Monthly,provNum:provNum1);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,204.25,DateTime.Today);//Make a payment for what is due right now.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $139.10
				^Represents proc1
			AccountEntry:      Today  provNum1  pat   $483.00
				^Represents proc2
			AccountEntry:      Today  provNum2  pat   $174.80
				^Represents proc3
			AccountEntry:      Today  provNum1  pat   $8.35
				^Represents adj1
			AccountEntry:      Today  provNum1  pat   $3.10
				^Represents adj2
			AccountEntry:      Today  provNum1  pat   $50.00
				^Represents adj3
			FauxAccountEntry:  Today  provNum1  pat   $140.00
				^PayPlanCharge debit for proc1
			FauxAccountEntry:  Today  provNum1  pat   $64.25
				^PayPlanCharge debit for proc2
			******************************************************/
			Assert.AreEqual(8,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==(decimal)139.10
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==(decimal)483.00
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& x.AmountOriginal==(decimal)174.80
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc3.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==(decimal)8.35
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==(decimal)3.10
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==(decimal)50.00
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& ((FauxAccountEntry)x).Principal==140
				&& x.AmountEnd==0
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& ((FauxAccountEntry)x).Principal==(decimal)64.25
				&& x.AmountEnd==0
				&& x.ProcNum==proc2.ProcNum));
			//The patient technically owes $204.25 because that is the monthly payment amount on the payment plan.
			//Since dynamic payment plans explicitly pay off procedures there will be two splits instead of a singular split for the entire $204.25
			//This is because proc1 can only handle $140 and the remaining $64.25 will go to proc2 totalling to $204.25
			Assert.AreEqual(2,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==140
				&& x.ProvNum==provNum1
				&& x.AdjNum==0
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==64.25
				&& x.ProvNum==provNum1
				&& x.AdjNum==0
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic to prove that no transfer is necessary.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanWithPayNotAllowedForTP() {
			/*****************************************************
				Proc1	Prov1	250	Completed 1 year ago
				Proc2 Prov1 500	Treatment Planned 1 year ago
				PP1		Prov1	750
					PPC1	Prov1	Today-1Y		250
						^Attached to Proc1
					PPC2	Prov1	12/31/9999	500
						^Attached to Proc2
					PPD1	Prov1	Today-1Y		250
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			PrefT.UpdateYN(PrefName.PrePayAllowedForTpProcs,YN.No);
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"PPWTP",ProcStat.C,"",250,procDate:DateTime.Today.AddYears(-1),provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"PPWTP",ProcStat.TP,"",500,procDate:DateTime.Today.AddYears(-1),provNum:provNum1);
			DateTime payPlanDate=DateTime.Today.AddYears(-1);
			PayPlan payPlan1=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,250,payPlanDate,provNum:provNum1,
				new List<Procedure>(){ proc1,proc2 });
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,2000,DateTime.Today);//Make a payment for more than what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			Assert.AreEqual(4,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==250
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddYears(-1)
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==250
				&& x.AmountEnd==0
				&& x.Date==payPlanDate
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==250
				&& x.AmountEnd==0
				&& x.Date==payPlanDate.AddMonths(1)
				&& x.ProcNum==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==250
				&& x.AmountEnd==0
				&& x.Date==payPlanDate.AddMonths(2)
				&& x.ProcNum==0));
			Assert.AreEqual(4,results.ListAutoSplits.Count);
			//Treatment planned procedures should not be suggested to be paid (at this time).
			//The full $250 should be suggested for proc1/provNum1/payPlan1
			//Then there should be two more $250 splits suggested for the payment plan charges that would be attached to the TP procedure but aren't.
			//Finally, the rest goes to unearned.
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==250
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(2,results.ListAutoSplits.Count(x => x.SplitAmt==250
				&& x.ProvNum==provNum1
				&& x.ProcNum==0
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==1250
				&& x.ProvNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic to prove that no transfer is necessary.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PayPlanWithPayAllowedForTP() {
			/*****************************************************
				Proc1	Prov1	250	Completed 1 year ago
				Proc2 Prov1 500	Treatment Planned 1 year ago
				PP1		Prov1	750
					PPC1	Prov1	Today-1Y		250
						^Attached to Proc1
					PPC2	Prov1	12/31/9999	500
						^Attached to Proc2
					PPD1	Prov1	Today-1Y		250
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			PrefT.UpdateYN(PrefName.PrePayAllowedForTpProcs,YN.Yes);
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"PPWTP",ProcStat.C,"",250,procDate:DateTime.Today.AddYears(-1),provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"PPWTP",ProcStat.TP,"",500,procDate:DateTime.Today.AddYears(-1),provNum:provNum1);
			DateTime payPlanDate=DateTime.Today.AddYears(-1);
			PayPlan payPlan1=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,250,payPlanDate,provNum:provNum1,
				new List<Procedure>(){ proc1,proc2 });
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,2000,DateTime.Today);//Make a payment for more than what the patient owes.
			PaymentEdit.AutoSplit results=PaymentEdit.AutoSplitForPayment(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false,null);
			Assert.AreEqual(5,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==250
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddYears(-1)
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==500
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddYears(-1)
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& ((FauxAccountEntry)x).Principal==250
				&& x.AmountEnd==0
				&& x.Date==payPlanDate
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(4,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& ((FauxAccountEntry)x).Principal==250
				&& x.AmountEnd==0
				&& x.Date==payPlanDate.AddMonths(1)
				&& x.ProcNum==0));
			Assert.AreEqual(4,results.ListAutoSplits.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& ((FauxAccountEntry)x).Principal==250
				&& x.AmountEnd==0
				&& x.Date==payPlanDate.AddMonths(2)
				&& x.ProcNum==0));
			Assert.AreEqual(4,results.ListAutoSplits.Count);
			//Treatment planned procedures should not be suggested to be paid (at this time).
			//The full $250 should be suggested for proc1/provNum1/payPlan1
			//Then there should be two more $250 splits suggested for the payment plan charges that would be attached to the TP procedure but aren't.
			//Finally, the rest goes to unearned.
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==250
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(2,results.ListAutoSplits.Count(x => x.SplitAmt==250
				&& x.ProvNum==provNum1
				&& x.ProcNum==0
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAutoSplits.Count(x => x.SplitAmt==1250
				&& x.ProvNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Insert the suggested PaySplit(s) and then perform the income transfer logic to prove that no transfer is necessary.
			results.ListAutoSplits.ForEach(x => PaySplits.Insert(x));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_PreferThisPatient() {
			/*****************************************************
				Patient  pat1  (guarantor)
				Patient  pat2  (family member)
				Provider  provNum1
				Procedure  proc1  pat1  provNum1  Today-5D   $250
				Procedure  proc2  pat2  provNum1  Today-2D   $100
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Patient pat1=PatientT.CreatePatient($"{suffix}-1");
			Patient pat2=PatientT.CreatePatient($"{suffix}-2");
			Procedure proc1=ProcedureT.CreateProcedure(pat1,"PTP01",ProcStat.C,"",250,procDate:DateTime.Today.AddDays(-5),provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat2,"PTP02",ProcStat.C,"",100,procDate:DateTime.Today.AddDays(-2),provNum:provNum1);
			//Make a payment for less than either procedure.
			Payment paymentFIFO=PaymentT.MakePaymentNoSplits(pat2.PatNum,50,DateTime.Today);
			//Act like we have pat2 selected (patCurNum will be set to pat2.PatNum).
			//The auto-split logic should suggest paying proc1 first due to FIFO order when isPatPrefer==false.
			PaymentEdit.AutoSplit autoSplitFIFO=PaymentEdit.AutoSplitForPayment(new List<long>() { pat1.PatNum,pat2.PatNum },pat2.PatNum,new List<PaySplit>(),
				paymentFIFO,new List<AccountEntry>(),false,false,null);
			Assert.AreEqual(1,autoSplitFIFO.ListAutoSplits.Count);
			Assert.AreEqual(1,autoSplitFIFO.ListAutoSplits.Count(x => x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc1.ProcNum
				&& x.SplitAmt==50
				&& x.UnearnedType==0));
			//Make a payment for less than either procedure.
			Payment paymentPrefer=PaymentT.MakePaymentNoSplits(pat2.PatNum,50,DateTime.Today);
			//Continue to have pat2 selected (patCurNum set to pat2.PatNum) but set isPatPrefer==true.
			//The auto-split logic should suggest paying proc2 first due to it being associated to pat2 even though proc1 is first via FIFO order.
			PaymentEdit.AutoSplit autoSplitPrefer=PaymentEdit.AutoSplitForPayment(new List<long>() { pat1.PatNum,pat2.PatNum },pat2.PatNum,new List<PaySplit>(),
				paymentPrefer,new List<AccountEntry>(),false,true,null);
			Assert.AreEqual(1,autoSplitPrefer.ListAutoSplits.Count);
			Assert.AreEqual(1,autoSplitPrefer.ListAutoSplits.Count(x => x.PatNum==pat2.PatNum
				&& x.ProvNum==provNum1
				&& x.ProcNum==proc2.ProcNum
				&& x.SplitAmt==50
				&& x.UnearnedType==0));
		}

		#endregion

		#region Init Tests

		///<summary>Make sure auto splits that are created are in the correct number and order (earliest proc paid first).</summary>
		[TestMethod]
		public void PaymentEdit_Init_CorrectlyOrderedAutoSplits() {//Legacy_TestFortyOne
			string suffix="41";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",40,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Now.AddDays(-3));
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,150);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payment,true,false);
			PaymentEdit.InitData init=PaymentEdit.Init(loadData);
			//Auto Splits will be in opposite order from least recent to most recent.
			Assert.AreEqual(3,init.AutoSplitData.ListAutoSplits.Count);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[0].SplitAmt!=60 || init.AutoSplitData.ListAutoSplits[0].ProcNum!=procedure3.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[1].SplitAmt!=40 || init.AutoSplitData.ListAutoSplits[1].ProcNum!=procedure2.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[2].SplitAmt!=50 || init.AutoSplitData.ListAutoSplits[2].ProcNum!=procedure1.ProcNum);
		}

		///<summary>Make sure auto splits are created in correct number and order with an existing payment already present.</summary>
		[TestMethod]
		public void PaymentEdit_Init_CorrectlyOrderedAutoSplitsWithExistingPayment() {//Legacy_TestFortyTwo
			string suffix="42";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",60,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",80,DateTime.Now.AddDays(-3));
			Payment payment1=PaymentT.MakePayment(patNum,110,DateTime.Now.AddDays(-2));
			Payment payment2=PaymentT.MakePaymentNoSplits(patNum,80,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payment2,true,false);
			PaymentEdit.InitData init=PaymentEdit.Init(loadData);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain three paysplits, one for procedure1 for 40, and one for procedure2 for 30,
			//and an unallocated split for 10 with the remainder on the payment (40+30+10=80).
			Assert.AreEqual(3,init.AutoSplitData.ListAutoSplits.Count);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[0].SplitAmt!=30 || init.AutoSplitData.ListAutoSplits[0].ProcNum!=procedure2.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[1].SplitAmt!=40 || init.AutoSplitData.ListAutoSplits[1].ProcNum!=procedure1.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[2].SplitAmt!=10 || init.AutoSplitData.ListAutoSplits[2].ProcNum!=0);
		}

		///<summary>Make sure if existing procedures are overpaid with an unallocated payment that an additional payment doesn't autosplit to the procs.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AutoSplitOverAllocation() {//Legacy_TestFortyThree
			string suffix="43";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",60,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",80,DateTime.Now.AddDays(-3));
			Payment payment1=PaymentT.MakePayment(patNum,200,DateTime.Now.AddDays(-2));
			Payment payment2=PaymentT.MakePaymentNoSplits(patNum,50,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payment2,true,false);
			PaymentEdit.InitData init=PaymentEdit.Init(loadData);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain one paysplit worth 50 and not attached to any procedures.
			Assert.AreEqual(1,init.AutoSplitData.ListAutoSplits.Count);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[0].SplitAmt!=50 || init.AutoSplitData.ListAutoSplits[0].ProcNum!=0);
		}

		///<summary>Make sure if a payment is created fo rnegative amount that it makes no auto splits.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AutoSplitForPaymentNegativePaymentAmount() {//Legacy_TestFortyFour
			string suffix="44";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40,DateTime.Now.AddDays(-1));
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,-50,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payment,true,false);
			PaymentEdit.InitData init=PaymentEdit.Init(loadData);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain no paysplits since it doesn't make sense to create negative payments when there are outstanding charges.
			Assert.AreEqual(0,init.AutoSplitData.ListAutoSplits.Count);
		}

		///<summary>Make sure auto splits take into account unallocated adjustment, an overpayment on a procedure, 
		///and are attributed correctly to the remaining procedure with an unallocated split for the rest.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AutoSplitWithAdjustmentAndExistingPayment() {//Legacy_TestFortyFive
			/*****************************************************
				Create Patient:   pat
				Create Provider:  provNum
				Create procedure1:   Today-1D  provNum  pat   $40
				Create procedure2:   Today-2D  provNum  pat   $60
				Create procedure3:   Today-3D  provNum  pat   $80
				Create adjustment1:  Today-2D  provNum  pat  -$40
				Create payment1:     Today-2D  provNum  pat   $100
					^Attached to procedure3
				Create payment2:     Today     provNum  pat   $50
					^Invalid payment (no splits) designed for auto split logic.
			******************************************************/
			string suffix="45";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			long provNum=ProviderT.CreateProvider(suffix);
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40,DateTime.Today.AddDays(-1),provNum:provNum);
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",60,DateTime.Today.AddDays(-2),provNum:provNum);
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",80,DateTime.Today.AddDays(-3),provNum:provNum);
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(patNum,-40,procDate:DateTime.Today.AddDays(-2));
			Payment payment1=PaymentT.MakePayment(patNum,100,DateTime.Now.AddDays(-2),provNum:provNum,procNum:procedure3.ProcNum);
			Payment payment2=PaymentT.MakePaymentNoSplits(patNum,50,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payment2,true,false);
			PaymentEdit.InitData init=PaymentEdit.Init(loadData);
			/*****************************************************
			AccountEntry:  Today-1D  provNum  pat   $40
				^Represents procedure1
			AccountEntry:  Today-2D  provNum  pat   $60
				^Represents procedure2
			AccountEntry:  Today-3D  provNum  pat   $80
				^Represents procedure3
			AccountEntry:  Today-2D  provNum  pat  -$40
				^Represents ajustment1
			//PaymentEdit.Init() was invoked with isIncomeTxfr set to false which causes the paysplit for $100 to not show as an account entry.
			******************************************************/
			Assert.AreEqual(4,init.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(1,init.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(Procedure)
				&& x.Date==DateTime.Today.AddDays(-1)
				&& x.ProcNum==procedure1.ProcNum
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.AmountOriginal==40
				&& x.AmountEnd==0));//This procedure would be $40 had it not been for auto-split logic.
			Assert.AreEqual(1,init.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(Procedure)
				&& x.Date==DateTime.Today.AddDays(-2)
				&& x.ProcNum==procedure2.ProcNum
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.AmountOriginal==60
				&& x.AmountEnd==0));//This procedure would be $20 had it not been for implicit linking.
			Assert.AreEqual(1,init.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(Procedure)
				&& x.Date==DateTime.Today.AddDays(-3)
				&& x.ProcNum==procedure3.ProcNum
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.AmountOriginal==80
				&& x.AmountEnd==0));//This procedure would be -$20 had it not been for implicit linking.
			Assert.AreEqual(1,init.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(Adjustment)
				&& x.Date==DateTime.Today//Notice that this is not set to the adjustment's ProcDate but instead when the adjustment itself was created.
				&& x.ProcNum==0
				&& x.ProvNum!=provNum
				&& x.AdjNum==adjustment1.AdjNum
				&& x.AmountOriginal==-40
				&& x.AmountEnd==0));//This adjustment would be -$40 had it not been for implicit linking.
			//ListSplitsCur should contain two paysplits, $40 attached to the D1110 and another for the remainder of $10, not attached to any procedure.
			Assert.AreEqual(2,init.AutoSplitData.ListAutoSplits.Count);
			Assert.AreEqual(1,init.AutoSplitData.ListAutoSplits.Count(x => x.ProcNum==procedure1.ProcNum
				&& x.ProvNum==provNum
				&& x.AdjNum==0
				&& x.SplitAmt==40));
			Assert.AreEqual(1,init.AutoSplitData.ListAutoSplits.Count(x => x.ProcNum==0
				&& x.ProvNum==0
				&& x.AdjNum==0
				&& x.SplitAmt==10
				&& x.UnearnedType==Prefs.GetLong(PrefName.PrepaymentUnearnedType)));
		}

		///<summary>Make sure if there is a negative procedure and a negative payment amount that a new payment goes fully to unallocated.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AutoSplitForPaymentNegativePaymentAmountNegProcedure() {//Legacy_TestFortySix
			string suffix="46";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",-40,DateTime.Now.AddDays(-1));
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,-50,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payment,true,false);
			PaymentEdit.InitData init=PaymentEdit.Init(loadData);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain one paysplit for the amount passed in that is unallocated.
			Assert.AreEqual(1,init.AutoSplitData.ListAutoSplits.Count);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[0].SplitAmt!=-50 || init.AutoSplitData.ListAutoSplits[0].ProcNum!=0);
		}

		///<summary>Make sure auto split suggestions go to the correct patients, for the correct amounts.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AutoSplitProcedureGuarantor() {//Legacy_TestFortySeven
			string suffix="47";
			Patient pat=PatientT.CreatePatient(suffix);
			Patient patOld=PatientT.CreatePatient(suffix+"fam");
			Patient pat2=patOld.Copy();
			long patNum=pat.PatNum;
			pat2.Guarantor=patNum;
			Patients.Update(pat2,patOld);
			long patNum2=pat2.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat2,"D0120",ProcStat.C,"",40,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Now.AddDays(-3));
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,150,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payment,true,false);
			PaymentEdit.InitData init=PaymentEdit.Init(loadData);
			//Auto Splits will be in opposite order from least recent to most recent.
			Assert.AreEqual(3,init.AutoSplitData.ListAutoSplits.Count);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[0].SplitAmt!=60 || init.AutoSplitData.ListAutoSplits[0].ProcNum!=procedure3.ProcNum 
				|| init.AutoSplitData.ListAutoSplits[0].PatNum!=patNum);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[1].SplitAmt!=40 || init.AutoSplitData.ListAutoSplits[1].ProcNum!=procedure2.ProcNum 
				|| init.AutoSplitData.ListAutoSplits[1].PatNum!=patNum2);
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[2].SplitAmt!=50 || init.AutoSplitData.ListAutoSplits[2].ProcNum!=procedure1.ProcNum 
				|| init.AutoSplitData.ListAutoSplits[2].PatNum!=patNum);
		}

		///<summary>Make sure auto split suggestions take into account claim payments on procedures.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AutoSplitWithClaimPayments() {//Legacy_TestFortyEight
			string suffix="48";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			InsPlan insPlan=InsPlanT.CreateInsPlan(CarrierT.CreateCarrier(suffix).CarrierNum);
			InsSub insSub=InsSubT.CreateInsSub(patNum,insPlan.PlanNum);
			PatPlan patPlan=PatPlanT.CreatePatPlan(1,patNum,insSub.InsSubNum);
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",40,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Now.AddDays(-3));
			ClaimProcT.AddInsPaid(patNum,insPlan.PlanNum,procedure1.ProcNum,20,insSub.InsSubNum,0,0);
			ClaimProcT.AddInsPaid(patNum,insPlan.PlanNum,procedure2.ProcNum,5,insSub.InsSubNum,5,0);
			ClaimProcT.AddInsPaid(patNum,insPlan.PlanNum,procedure3.ProcNum,20,insSub.InsSubNum,0,10);
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,150,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payment,true,false);
			PaymentEdit.InitData init=PaymentEdit.Init(loadData);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain four splits, 30, 35, and 30, then one unallocated for the remainder of the payment 55.
			Assert.AreEqual(4,init.AutoSplitData.ListAutoSplits.Count);
			//First auto split not not 30, not not procedure 3, and not not patNum
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[0].SplitAmt!=30 
				|| init.AutoSplitData.ListAutoSplits[0].ProcNum!=procedure3.ProcNum
				|| init.AutoSplitData.ListAutoSplits[0].PatNum!=patNum);
			//Second auto split not not 35, not not procedure 2, and not not patNum
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[1].SplitAmt!=35 
				|| init.AutoSplitData.ListAutoSplits[1].ProcNum!=procedure2.ProcNum
				|| init.AutoSplitData.ListAutoSplits[1].PatNum!=patNum);
			//Third auto split not not 30, not not procedure 1, and not not patNum
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[2].SplitAmt!=30 
				|| init.AutoSplitData.ListAutoSplits[2].ProcNum!=procedure1.ProcNum
				|| init.AutoSplitData.ListAutoSplits[2].PatNum!=patNum);
			//Fourth auto split not not 55, and not not procNum of 0
			Assert.IsFalse(init.AutoSplitData.ListAutoSplits[3].SplitAmt!=55
				|| init.AutoSplitData.ListAutoSplits[3].ProcNum!=0);
		}

		///<summary>Make sure that with a positive adjustment that the first procedure is 
		///worth 55 at the end of auto splitting (was originally 75, and payment is for 20)</summary>
		[TestMethod]
		public void PaymentEdit_Init_FIFOWithPosAdjustment() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov1");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",75,DateTime.Today.AddMonths(-1),provNum:provNum);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",135,DateTime.Today.AddMonths(-1).AddDays(1),provNum:provNum);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,20,DateTime.Today.AddDays(-15),provNum:provNum);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,20);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payCur,true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData);
			//Verify the logic pays starts to pay off the first procedure
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(Procedure) && x.AmountEnd==55));
		}

		///<summary>Make sure that a new payment goes to a procedure instead of a payplan due to the payplan being paid.</summary>
		[TestMethod]
		public void PaymentEdit_Init_PayPlansWithAttachedCreditsAndUnattachedProcedureOnPaySplits() {
			//Test when implicit linking was taking payment plan splits into account twice.
			//The following scenario was causing a procedure to start with $25 upon init of the payment window when it should be $75, the full amount.
			/*****************************************************
				Create Patient:  pat
				Create proc1:              Today     prov?  pat   $50
				Create payplan:            Today-1M  prov?  pat   $50
					^Associated to proc1
				Create paymentForPayPlan:  Today     prov?  pat   $50
					^Attached to payment plan (but not proc)
				Create proc2:              Today     prov?  pat   $75
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",50);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,50,DateTime.Today.AddMonths(-1),listProcs:new List<Procedure>() { proc1 });
			Payment paymentForPayPlan=PaymentT.MakePayment(pat.PatNum,50,payDate:DateTime.Today,payPlanNum:payplan.PayPlanNum);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0150",ProcStat.C,"",75);
			Payment paymentForNewProc=PaymentT.MakePaymentNoSplits(pat.PatNum,75,payDate:DateTime.Today,isNew:true,payType:1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,paymentForNewProc,true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData);
			//Verify the starting amount of the new procedure is correct and an autosplit exists for only the procedure, no unallocated.
			/*****************************************************
			AccountEntry:  Today  prov?  pat   $50
				^Represents proc1
			AccountEntry:  Today  prov?  pat   $75
				^Represents proc2
			AccountEntry:  Today  prov?  pat   $50
				^Faux entry designed for proc1 / payplan combo
			******************************************************/
			Assert.AreEqual(3,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==50
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==50
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddMonths(-1)//Note, this is the date of the ppcharge not the proc.
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==75
				&& x.AmountEnd==0//This is $0 because $75 from pay will be "auto split".
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,initData.AutoSplitData.ListAutoSplits.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAutoSplits.Count(x => x.PatNum==pat.PatNum
				&& x.ProcNum==proc2.ProcNum
				&& x.ProvNum==proc2.ProvNum
				&& x.SplitAmt==75
				&& x.PayPlanNum==0));
		}

		///<summary>Make sure if an adjustment is attached to a procedure that it affects the procedure's amount.</summary>
		[TestMethod]
		public void PaymentEdit_Init_AttachedAdjustment() {
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum
				Create proc:         Today-1M+1D  provNum  pat   $135
				Create adjustment:   Today-15D    provNum  pat   $20
					^Attached to proc
				Create payment:      Today        provNum  pat   $20
					^Invalid payment (no splits) designed for auto-split logic.
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum=ProviderT.CreateProvider(suffix);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",135,DateTime.Today.AddMonths(-1).AddDays(1),provNum:provNum);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,20,DateTime.Today.AddDays(-15),provNum:provNum,procNum:proc.ProcNum);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,20);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payCur,true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData);
			/*****************************************************
			AccountEntry:  Today-1M+1D  provNum  pat   $135
				^Represents proc
			AccountEntry:  Today-15D    provNum  pat   $20
				^Represents adjustment associated to proc
			******************************************************/
			Assert.AreEqual(2,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==135
				//The procedure should get inflated by the positive adjustment: $135 + $20 = $155
				//Then the auto-split logic should suggest paying the procedure: $155 - $20 = $135
				&& x.AmountEnd==135
				&& x.Date==DateTime.Today.AddMonths(-1).AddDays(1)
				&& x.ProcNum==proc.ProcNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==20
				&& x.AmountEnd==0//The positive $20 got applied directly to the procedure.
				&& x.Date==DateTime.Today.AddDays(-15)
				&& x.ProcNum==proc.ProcNum));
			//Verify there is only one charge (the procedure's charge + the adjustment for the amount original)
			Assert.AreEqual(1,initData.AutoSplitData.ListAutoSplits.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAutoSplits.Count(x => x.SplitAmt==20
				&& x.AdjNum==0
				&& x.ProcNum==proc.ProcNum
				&& x.PatNum==proc.PatNum
				&& x.ProvNum==proc.ProvNum
				&& x.ClinicNum==proc.ClinicNum));
		}

		///<summary>Make sure that adjustments attached to a procedure affects its amount and unallocated splits reduce the procedure's end amount to 0.</summary>
		[TestMethod]
		public void PaymentEdit_Init_UnattachedPaymentsAndAttachedAdjustments() {
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum
				Create proc:         Today-1M+1D  provNum  pat   $135
				Create adjustment:   Today-15D    provNum  pat   $20
					^Attached to proc
				Create payment1:      Today-1D    prov?    pat   $35
					^Unallocated
				Create payment2:      Today-1D    prov?    pat   $20
					^Unallocated
				Create paycur:        Today       prov?    pat   $100
					^Invalid payment (no splits) designed for auto-split logic.
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov1");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",135,DateTime.Today.AddMonths(-1).AddDays(1),provNum:provNum);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,20,DateTime.Today.AddDays(-15),provNum:provNum,procNum:proc.ProcNum);
			Payment existingPayment1=PaymentT.MakePayment(pat.PatNum,35,DateTime.Today.AddDays(-1));//no prov or proc because it's unattached.
			Payment existingPayment2=PaymentT.MakePayment(pat.PatNum,20,DateTime.Today.AddDays(-1));
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,100);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payCur,true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData);
			/*****************************************************
			AccountEntry:  Today-1M+1D  provNum  pat   $135
				^Represents proc
			AccountEntry:  Today-15D    provNum  pat   $20
				^Represents adjustment associated to proc
			******************************************************/
			Assert.AreEqual(2,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==135
				//The procedure should get inflated by the positive adjustment: $135 + $20 = $155
				//Then the procedure should defalte by the two payments (implicitly): $155 - ($35 + $20) = $100
				//Finally, the auto-split logic should suggest paying the procedure: $100 - $100 = $0
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddMonths(-1).AddDays(1)
				&& x.ProcNum==proc.ProcNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==20
				&& x.AmountEnd==0//The positive $20 got applied directly to the procedure.
				&& x.Date==DateTime.Today.AddDays(-15)
				&& x.ProcNum==proc.ProcNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAutoSplits.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAutoSplits.Count(x => x.SplitAmt==100
				&& x.AdjNum==0
				&& x.ProcNum==proc.ProcNum
				&& x.PatNum==proc.PatNum
				&& x.ProvNum==proc.ProvNum
				&& x.ClinicNum==proc.ClinicNum));
		}

		///<summary>Make sure that if a procedure isn't paid explicitly that income transfer mode displays all things separately (procedures and paysplit).</summary>
		[TestMethod]
		public void PaymentEdit_Init_IncomeTransferableWhenNotExplicitlyLinkedProcs() {
			/*****************************************************
				Create Patient:   pat
				Create Provider1:  provNum1
				Create Provider2:  provNum2
				Create Procedure1:  Today-1M  prov1  pat   $100
				Create Procedure2:  Today-1M  prov2  pat   $50
				Create Payment1:    Today-1D  prov1  pat   $150
					^PaySplit for prov1 and pat
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum1=ProviderT.CreateProvider("prov1");
			long provNum2=ProviderT.CreateProvider("prov2");
			Procedure procByProv1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",100,DateTime.Today.AddMonths(-1),provNum:provNum1);
			Procedure procByProv2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",50,DateTime.Today.AddMonths(-1),provNum:provNum2);
			Payment payAllForOneProv=PaymentT.MakePayment(pat.PatNum,150,DateTime.Today.AddDays(-1),provNum:provNum1,payType:1);//make entire payment to prov1
			//make an income transfer and see if it catches the over and underallocations
			Payment incomeTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0,isNew:true);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,incomeTransfer,true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData,isIncomeTxfr:true);
			/*****************************************************
			AccountEntry:  Today-1M  prov1  pat   $100
				^Represents Procedure1
			AccountEntry:  Today-1M  prov2  pat   $50
				^Represents Procedure2
			AccountEntry:  Today-1D  prov1  pat  -$150
				^Represents Payment1
			******************************************************/
			//Assert that the appropriate amounts are still considered transferable because they are not explicitly linked.
			Assert.AreEqual(3,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.Tag.GetType()==typeof(Procedure)
				&& x.ProvNum==provNum1
				&& x.ProcNum==procByProv1.ProcNum
				&& x.AmountOriginal==100
				&& x.AmountEnd==100).Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.Tag.GetType()==typeof(Procedure)
				&& x.ProvNum==provNum2
				&& x.ProcNum==procByProv2.ProcNum
				&& x.AmountOriginal==50
				&& x.AmountEnd==50).Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.Tag.GetType()==typeof(PaySplit)
				&& x.ProvNum==provNum1
				&& x.ProcNum==0
				&& x.AmountOriginal==-150
				&& x.AmountEnd==-150).Count);
		}

		///<summary>Make sure in income transfer mode that if nothing is explicitly attached, all charges/credits show in the list.</summary>
		[TestMethod]
		public void PaymentEdit_Init_IncomeTransferableWhenNotExplicitlyLinkedAdjs() {
			/*****************************************************
				Create Patient:   pat
				Create Provider1:  provNum1
				Create Provider2:  provNum2
				Create Adjustment1:  Today-1M  prov1  pat   $100
				Create Adjustment2:  Today-1M  prov2  pat   $150
				Create Payment1:     Today-1D  prov1  pat   $250
					^PaySplit for prov1 and pat
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum1=ProviderT.CreateProvider("prov1");
			long provNum2=ProviderT.CreateProvider("prov2");
			Adjustment adjForProv1=AdjustmentT.MakeAdjustment(pat.PatNum,100,DateTime.Today.AddMonths(-1),provNum:provNum1);
			Adjustment adjForProv2=AdjustmentT.MakeAdjustment(pat.PatNum,150,DateTime.Today.AddMonths(-1),provNum:provNum2);
			Payment payAllForOneProv=PaymentT.MakePayment(pat.PatNum,250,DateTime.Today.AddDays(-1),provNum:provNum1,payType:1);//make entire payment to prov1
			//make an income transfer and see if it catches the over and underallocations
			Payment incomeTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0,isNew:true);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,incomeTransfer,true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData,isIncomeTxfr:true);
			/*****************************************************
			AccountEntry:  Today-1M  prov1  pat   $100
				^Represents Adjustment1
			AccountEntry:  Today-1M  prov2  pat   $150
				^Represents Adjustment2
			AccountEntry:  Today-1D  prov1  pat  -$250
				^Represents paysplit
			******************************************************/
			//Assert that the appropriate amounts are still considered transferable because they are not explicitly linked.
			Assert.AreEqual(3,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.Tag.GetType()==typeof(Adjustment)
				&& x.ProvNum==provNum1
				&& x.AdjNum==adjForProv1.AdjNum
				&& x.AmountOriginal==100
				&& x.AmountEnd==100).Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.Tag.GetType()==typeof(Adjustment)
				&& x.ProvNum==provNum2
				&& x.AdjNum==adjForProv2.AdjNum
				&& x.AmountOriginal==150
				&& x.AmountEnd==150).Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.Tag.GetType()==typeof(PaySplit)
				&& x.ProvNum==provNum1
				&& x.AdjNum==0
				&& x.AmountOriginal==-250
				&& x.AmountEnd==-250).Count);
		}

		///<summary>Make sure that payment logic takes into account base units.</summary>
		[TestMethod]
		public void PaymentEdit_Init_ProcedureWithBaseUnits() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure procWithBaseUnits=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",100,baseUnits:1);//1 proc fee is $100, so total should be $200.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,200);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payCur,true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData,new List<AccountEntry> { new AccountEntry(procWithBaseUnits) });
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.AmountOriginal==200 && x.AmountEnd==0).Count);
		}

		///<summary>Make sure that if a payment plan has a payment plan adjustment that the procedure owes the correct amount.</summary>
		[TestMethod]
		public void PaymentEdit_Init_ShowCorrectAmountNeedingPaymentOnChargeWhenPaymentPlanAdjustmentsExistForThePayPlanCharge() {
			/*****************************************************
				Create Patient:  pat
				Create proc1:    Today-3M  prov?  pat   $100
				Create payplan:  Today-3M  prov0  pat   $100
					^PayPlanCharge debit for proc1
				Create PayPlanCharge: Today-3M  prov0  pat  -$60
					^PayPlanCharge debit adjustment
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",100,DateTime.Today.AddMonths(-3));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,100,DateTime.Today.AddMonths(-3),0,new List<Procedure> { proc1 });
			List<PayPlanCharge> listChargesAndCredits=PayPlanCharges.GetForPayPlan(payplan.PayPlanNum);
			listChargesAndCredits=PayPlanEdit.CreatePayPlanAdjustments(-60,listChargesAndCredits,0);//create a $60 adjustment for the $100 charge.
			PayPlans.Update(payplan);
			PayPlanCharges.Sync(listChargesAndCredits,payplan.PayPlanNum);
			Payment paymentCur=PaymentT.MakePaymentNoSplits(pat.PatNum,40,DateTime.Today,true);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,paymentCur,true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData);
			/*****************************************************
			AccountEntry:  Today-3M  prov?  pat   $100
				^Represents proc1
			AccountEntry:  Today-3M  prov0  pat   $100
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today-3M  prov0  pat   $100
				^Represents a payplan charge debit
			AccountEntry:  Today-3M  prov0  pat  -$60
				^Faux entry that represents a payplan charge debit adjustment
			******************************************************/
			Assert.AreEqual(3,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.Date==DateTime.Today.AddMonths(-3)
				&& x.AmountOriginal==100
				&& x.AmountEnd==0
				&& x.AdjNum==0
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.Date==DateTime.Today.AddMonths(-3)
				&& ((FauxAccountEntry)x).Principal==100
				//Implicit linking (mismatch in prov) should apply the adjustment to this faux account entry: $100 - $60 = $40
				//Then the auto split logic should apply the new $40 payment: $40 - $40 = $0
				&& x.AmountEnd==0
				&& x.AdjNum==0
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.Date==DateTime.Today.AddMonths(-3)
				&& ((FauxAccountEntry)x).IsAdjustment
				&& x.AmountOriginal==-60
				&& x.AmountEnd==0//Should get implicitly linked to the payment plan it is adjusting.
				&& x.AdjNum==0//Not attached to a legit adjustment, just an adjustment for the payment plan in general.
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
			//The auto-split logic should have suggested that the payment plan procedure get paid.
			Assert.AreEqual(1,initData.AutoSplitData.ListAutoSplits.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAutoSplits.Count(x => x.DatePay==DateTime.Today
				&& x.PatNum==proc1.PatNum
				&& x.PayNum==paymentCur.PayNum
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.ProcNum==proc1.ProcNum
				&& x.ProvNum==proc1.ProvNum
				&& x.ClinicNum==proc1.ClinicNum
				&& x.SplitAmt==40
				&& x.UnearnedType==0));
		}

		///<summary>Make sure that two procedures on a payment plan, with payments attached to the plan, that there are two charges requiring payment still.</summary>
		[TestMethod]
		public void PaymentEdit_Init_ChargesWithAttachedPayPlanCreditsWithPreviousPaymentsImplicit() {
			/*****************************************************
				Create Patient:  pat
				Create proc1:    Today-4M  prov?  pat   $135
				Create proc2:    Today-4M  prov?  pat   $60
				Create payplan:  Today-3M  prov?  pat   $195
					^PayPlanCharge Credit for proc1
					^PayPlanCharge Credit for proc2
					^PayPlanCharge Debits for 195 / 30 = 6.5 (rounded up for 7 total debits)
				Create payment:  Today-2M  prov?  pat   $30
					^Attached to proc1 and payplan
				Create payment:  Today-1M  prov?  pat   $30
					^Attached to proc1 and payplan
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-4));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-4));
			DateTime datePayPlanStart=DateTime.Today.AddMonths(-3);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,datePayPlanStart,0,new List<Procedure>() {proc1,proc2});
			//Procedures's amount start should now be 0 from being attached. Make initial payments.
			PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-2),payplan.PayPlanNum,procNum:proc1.ProcNum);
			PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-1),payplan.PayPlanNum,procNum:proc1.ProcNum);
			//2 pay plan charges should have been removed from being paid. Make a new payment. 
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,30,DateTime.Today,isNew:true,payType:1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData);
			/*****************************************************
			AccountEntry:  Today-4M  prov?  pat   $135
				^Represents proc1
			AccountEntry:  Today-4M  prov?  pat   $60
				^Represents proc2
			AccountEntry:  Today-4M  prov0  pat   $30
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today-3M  prov0  pat   $30
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today-2M  prov0  pat   $30
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today-1M  prov0  pat   $30
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today     prov0  pat   $15
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today     prov0  pat   $15
				^Faux entry designed for proc2 / payplan combo
			AccountEntry:  Today+1M  prov0  pat   $30
				^Faux entry designed for proc2 / payplan combo
			AccountEntry:  Today+2M  prov0  pat   $15
				^Faux entry designed for proc2 / payplan combo
			******************************************************/
			Assert.AreEqual(10,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==135
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddMonths(-4)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==60
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddMonths(-4)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==0));
			//There are only be 4 faux entries that are due (<=Today) for the payment plan at this time ($30 x 4(months) = $120).
			//The two $30 payments should be implicitly applied (different providers) ($120 - $30 - $30 = $60)
			//Then the $30 payment that is about to be made will also remove from this faux entry ($60 - $30 = $30).
			//Therefore, only $30 of outstanding value should be left on faux entries.
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(1)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(2)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==30
				&& x.Date==datePayPlanStart.AddMonths(3)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			//Nothing past this point will have value because it hasn't come due yet (in the future).
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==15
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(4)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==15
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(4)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(5)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==15
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(6)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
		}

		///<summary>Make sure that two procedures on a payment plan, with payments attached to the plan, that there are two charges requiring payment still.</summary>
		[TestMethod]
		public void PaymentEdit_Init_ChargesWithAttachedPayPlanCreditsWithPreviousPaymentsExplicit() {
			/*****************************************************
				Create Patient:  pat
				Create proc1:    Today-4M  prov?  pat   $135
				Create proc2:    Today-4M  prov?  pat   $60
				Create payplan:  Today-3M  prov?  pat   $195
					^PayPlanCharge Credit for proc1
					^PayPlanCharge Credit for proc2
					^PayPlanCharge Debits for 195 / 30 = 6.5 (rounded up for 7 total debits)
				Create payment:  Today-2M  prov?  pat   $30
					^Attached to proc1 and payplan
				Create payment:  Today-1M  prov?  pat   $30
					^Attached to proc1 and payplan
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-4));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-4));
			DateTime datePayPlanStart=DateTime.Today.AddMonths(-3);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,datePayPlanStart,0,new List<Procedure>() {proc1,proc2});
			//Procedures's amount start should now be 0 from being attached. Make initial payments.
			PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-2),payplan.PayPlanNum,procNum:proc1.ProcNum);
			PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-1),payplan.PayPlanNum,procNum:proc1.ProcNum);
			//2 pay plan charges should have been removed from being paid. Make a new payment. 
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,30,DateTime.Today,isNew:true,payType:1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData,doIncludeExplicitCreditsOnly:true);
			/*****************************************************
			AccountEntry:  Today-4M  prov?  pat   $135
				^Represents proc1
			AccountEntry:  Today-4M  prov?  pat   $60
				^Represents proc2
			AccountEntry:  Today-4M  prov0  pat   $30
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today-3M  prov0  pat   $30
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today-2M  prov0  pat   $30
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today-1M  prov0  pat   $30
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today     prov0  pat   $15
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today     prov0  pat   $15
				^Faux entry designed for proc2 / payplan combo
			AccountEntry:  Today+1M  prov0  pat   $30
				^Faux entry designed for proc2 / payplan combo
			AccountEntry:  Today+2M  prov0  pat   $15
				^Faux entry designed for proc2 / payplan combo
			******************************************************/
			Assert.AreEqual(10,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==135
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddMonths(-4)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==60
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddMonths(-4)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==0));
			//There are only be 4 faux entries that are due (<=Today) for the payment plan at this time ($30 x 4(months) = $120).
			//The two $30 payments should NOT be explicitly applied to any faux entries (different providers).
			//However, the $30 payment that is about to be made for the new payment will be the only thing to remove value from a faux entry.
			//Therefore, only $90 of outstanding value should be left on faux entries  ($120 - $30 = $90).
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==30
				&& x.Date==datePayPlanStart.AddMonths(1)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==30
				&& x.Date==datePayPlanStart.AddMonths(2)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==30
				&& x.Date==datePayPlanStart.AddMonths(3)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			//Nothing past this point will have value because it hasn't come due yet (in the future).
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==15
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(4)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==15
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(4)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(5)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==15
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(6)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
		}

		///<summary>Make sure that when a payment plan is closed out but partially paid that procedures still calculate as owing correct amount</summary>
		[TestMethod]
		public void PaymentEdit_Init_ShowCorrectAmountNeedingPaymentWhenPaymentPlanV2IsClosedAndPartiallyPaid() {
			//Explicitly Link Credits is the method that will contain the method being tested, but test will call Init to run through the whole gambit.
			/*****************************************************
				Create Patient:  pat
				Create proc:     Today-4M  provNum  pat   $100
				Create payplan:  Today-3M  provNum  pat   $100
					^PayPlanCharge debits for proc $100 / $30 = 3.33 (4 debits, 3 x $30 and 1 x $10)
				Create payment:  Today-2M  provNum  pat   $30
					^Attached to proc and payplan.
			******************************************************/
			PrefT.UpdateInt(PrefName.PayPlansVersion,(int)PayPlanVersions.AgeCreditsAndDebits);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum=ProviderT.CreateProvider(suffix);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",100,procDate:DateTime.Today.AddMonths(-4),provNum:provNum);
			DateTime datePayPlanStart=DateTime.Today.AddMonths(-3);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,datePayPlanStart,provNum:provNum,
				listProcs:new List<Procedure>() { proc });
			PaymentT.MakePayment(pat.PatNum,30,payDate:DateTime.Today.AddMonths(-2),payPlanNum:payplan.PayPlanNum,provNum:provNum,procNum:proc.ProcNum);
			//jsalmon - There is technically no reason to close out the payment plan for today because nothing is due in the future...
			//Leaving the "close out" code here just because it was here previously and doesn't really hurt anything, but also doesn't do anything...
			List<PayPlanCharge> listCharges=PayPlanCharges.GetForPayPlan(payplan.PayPlanNum);
			listCharges.Add(PayPlanEdit.CloseOutPatPayPlan(listCharges,payplan,DateTime.Today));
			listCharges.RemoveAll(x => x.ChargeDate > DateTime.Today);
			payplan.IsClosed=true;
			PayPlans.Update(payplan);
			PayPlanCharges.Sync(listCharges,payplan.PayPlanNum);
			//Create a $60 payment that should get auto split within Init().
			Payment currentPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,60,DateTime.Today,true,1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,currentPayment,true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData);
			/*****************************************************
			AccountEntry:  Today-4M  provNum  pat   $100
				^Represents proc
			AccountEntry:  Today-3M  provNum  pat   $30
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today-2M  provNum  pat   $30
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today-1M  provNum  pat   $30
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today     provNum  pat   $10
				^Faux entry designed for proc1 / payplan combo
			******************************************************/
			Assert.AreEqual(5,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.Date==DateTime.Today.AddMonths(-4)
				&& x.AmountOriginal==100
				&& x.AmountEnd==0
				&& x.AdjNum==0
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart
				&& x.AdjNum==0
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(2)
				&& x.AdjNum==0
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(2)
				&& x.AdjNum==0
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			//The $30 payment should get applied to this faux account entry: $100 - $30 = $70
			//Then the $60 payment that is being made should get auto split:  $70 - $60 = $10
			//So there will still be $10 left due on this PayPlanCharge because FIFO will have suggested paying the older charges.
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==10
				&& x.AmountEnd==10
				&& x.Date==datePayPlanStart.AddMonths(3)
				&& x.AdjNum==0
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(2,initData.AutoSplitData.ListAutoSplits.Count);
			Assert.AreEqual(2,initData.AutoSplitData.ListAutoSplits.Count(x => x.PatNum==proc.PatNum
				&& x.ProvNum==proc.ProvNum
				&& x.ProcNum==proc.ProcNum
				&& x.SplitAmt==30
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.UnearnedType==0));
		}

		///<summary>If a payment plan is created with a guarantor outside the payment plan, and payments made by guarantor, don't show the payplan's procedures nor guarantor's payment as valid income transfer sources/destinations.</summary>
		[TestMethod]
		public void PaymentEdit_Init_PayPlanAndGuarantorPaymentsOutsideFamily() {
			/*****************************************************
				Create Patient:  pat1
				Create Patient:  pat2
				Create Provider: provNum1
				Create proc1:    Today-3M  provNum1  pat1   $100
				Create proc2:    Today-3M  provNum1  pat2   $92
				Create payplan:  Today-3M  provNum1  pat1   $100
					^Associated to proc1 but pat2 is set as guarantor
				Create payment:  Today-2M  provNum1  pat2   $100
					^Attached to payment plan (but not proc)
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			Patient pat2=PatientT.CreatePatient(suffix+"2");
			long provNum1=ProviderT.CreateProvider(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat1,"D1120",ProcStat.C,"",100,DateTime.Today.AddMonths(-3),provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat2,"D0220",ProcStat.C,"",92,DateTime.Today.AddMonths(-3),provNum:provNum1);
			DateTime datePayPlan=DateTime.Today.AddMonths(-3);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat1.PatNum,100,datePayPlan,provNum1,new List<Procedure> { proc1 },guarantorNum:pat2.PatNum);
			Payment payment=PaymentT.MakePayment(pat2.PatNum,100,DateTime.Today.AddMonths(-2),payplan.PayPlanNum,provNum1);//make a payment for the plan
			List<PayPlanCharge> listCharges=PayPlanCharges.GetForPayPlan(payplan.PayPlanNum);
			//When performing an income transfer, we should see only one charge.
			//The charge for the guarantor that's unpaid for 92, even though the guarantor paid on the payplan.
			Payment paymentNoSplits=PaymentT.MakePaymentNoSplits(pat2.PatNum,0,isNew:true);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat2,paymentNoSplits,true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData);
			/*****************************************************
			AccountEntry:  Today-3M  provNum1  pat1   $100
				^Represents proc1
			AccountEntry:  Today-3M  provNum1  pat2   $92
				^Represents proc2
			AccountEntry:  Today-3M  provNum1  pat1   $100
				^Faux entry designed for proc1 / payplan combo
			******************************************************/
			Assert.AreEqual(3,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat1.PatNum
				&& x.AmountOriginal==100
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddMonths(-3)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat2.PatNum
				&& x.AmountOriginal==92
				&& x.AmountEnd==92
				&& x.Date==DateTime.Today.AddMonths(-3)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat1.PatNum
				&& x.AmountOriginal==100
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddMonths(-3)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
		}

		[TestMethod]
		public void PaymentEdit_Init_PayPlanCreateAttachedSplitsForMultipleAttachedCredits() {
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum
				Create proc1:    Today-3M  provNum  pat   $100
				Create proc2:    Today-3M  provNum  pat   $100
				Create payplan:  Today-3M  provNum  pat   $100
					^Associated to proc1 and proc2
				Create payment:  Today-2M  provNum  pat   $100
					^Attached to payment plan (but not proc)
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum=ProviderT.CreateProvider(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",100,DateTime.Today.AddMonths(-3),provNum:provNum);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,DateTime.Today.AddMonths(-3),provNum:provNum);
			DateTime datePayPlanStart=DateTime.Today.AddMonths(-3);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,100,datePayPlanStart,provNum,new List<Procedure> { proc1,proc2 });
			Payment payment=PaymentT.MakePayment(pat.PatNum,100,DateTime.Today.AddMonths(-2),payplan.PayPlanNum,provNum:provNum);
			Payment newPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,100,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,newPayment,true,false);
			PaymentEdit.InitData init=PaymentEdit.Init(loadData);
			/*****************************************************
			AccountEntry:  Today-3M  provNum  pat   $100
				^Represents proc1
			AccountEntry:  Today-3M  provNum  pat   $100
				^Represents proc2
			AccountEntry:  Today-3M  provNum  pat   $100
				^Faux entry designed for proc1 / payplan combo
			AccountEntry:  Today-2M  provNum  pat   $100
				^Faux entry designed for proc2 / payplan combo
			******************************************************/
			Assert.AreEqual(4,init.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(1,init.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==100
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddMonths(-3)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,init.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==100
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddMonths(-3)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,init.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==100
				&& ((FauxAccountEntry)x).Principal==100
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,init.AutoSplitData.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==100
				&& x.AmountEnd==0//The auto split logic should apply the $100 payment to this account entry.
				&& x.Date==datePayPlanStart.AddMonths(1)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,init.AutoSplitData.ListAutoSplits.Count);
			Assert.AreEqual(1,init.AutoSplitData.ListAutoSplits.Count(x => x.PatNum==pat.PatNum
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.ProvNum==proc2.ProvNum
				&& x.SplitAmt==100
				&& x.UnearnedType==0));
		}

		#endregion

		#region MakePayment Tests

		///<summary>Make sure that if there is a positive adjustment (and only that) that auto split logic will make a split to it.</summary>
		[TestMethod]
		public void PaymentEdit_MakePayment_Adjustment() {
			//equivalent of being in the payment window and then hitting the 'pay' button to move a charge over from outstanding to current list of splits.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,100,DateTime.Today.AddDays(-1));
			Payment payment=PaymentT.MakePaymentNoSplits(pat.PatNum,100,DateTime.Today,true,1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payment,true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long> {pat.PatNum },pat.PatNum,
				PaySplits.GetForPayment(payment.PayNum),payment.PayNum,false);
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,chargeData.ListPaySplits,payment,new List<AccountEntry>());
			List<List<AccountEntry>> listListAE=new List<List<AccountEntry>>();
			listListAE.Add(constructResults.ListAccountCharges);
			PaymentEdit.PayResults results=PaymentEdit.MakePayment(listListAE,payment,100,constructResults.ListAccountCharges);
			Assert.AreEqual(0,results.ListAccountCharges.FindAll(x => x.AmountEnd>0).Count);//no more splits should need to be paid off.
			Assert.AreEqual(1,results.ListSplitsCur.FindAll(x => x.SplitAmt==100).Count);//Adjustment should now be paid in full. 
		}

		#endregion

		#region AllocateUnearned Tests

		///<summary>Make sure that if there is a 100 prepayment and a procedure for 50, that allocating unearned allocates properly and unearned amount total
		///is correct.</summary>
		[TestMethod]
		public void PaymentEdit_AllocateUnearned_LinkToOriginalPrepayment() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			//create prepayment of $100
			long provNum=ProviderT.CreateProvider("SG");
			Clinic clinic1=ClinicT.CreateClinic("Clinic1");
			Family fam=Patients.GetFamily(pat.PatNum);
			//create original prepayment.
			PaySplit prePay=PaySplitT.CreatePrepayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum,clinic1.ClinicNum);
			//complete a procedure
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,provNum:provNum);
			//Setup to run the PaymentEdit.AllocateUnearned
			List<PaySplit> listPaySplits=new List<PaySplit>();
			List<PaySplit> listFamPrePaySplits=PaySplits.GetPrepayForFam(fam);
			//Unearned amount should be $100.
			double unearnedAmt=(double)PaySplits.GetUnearnedForFam(fam,listFamPrePaySplits);
			Assert.AreEqual(100,unearnedAmt);
			//Create the payment we will use to allocate some of the $100 prepayment.
			Payment pay=PaymentT.MakePaymentForPrepayment(pat,clinic1);
			//Run the AllocateUnearned method. This a list of paysplitAssociated.
			//The ref list of paysplits should also have the new paysplits that are associated to the original prepayment.
			List<PaySplits.PaySplitAssociated> listPaySplitAssociated=PaymentEdit.AllocateUnearned(new List<AccountEntry> { new AccountEntry(proc1) },ref listPaySplits,pay,unearnedAmt,fam);
			//Insert the paysplits and link the prepayment paysplits. This is similar to what is done when a user creates a payment from FormPayment.cs.
			PaySplits.InsertManyWithAssociated(pay.PayNum,listPaySplits,listPaySplitAssociated);
			//The ref list of paysplits should have the correct allocated prepayment amount. 
			Assert.AreEqual(-50,listPaySplits.Where(x => x.UnearnedType!=0).Sum(x => x.SplitAmt));
			//Create new procedure
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",100,provNum:provNum);
			//Now do what we just did again for a new procedure. The unallocated amount should be $50.
			listFamPrePaySplits=PaySplits.GetPrepayForFam(fam);
			unearnedAmt=(double)PaySplits.GetUnearnedForFam(fam,listFamPrePaySplits);
			Assert.AreEqual(50,unearnedAmt);
			List<PaySplit> listPaySplitsUnearned=new List<PaySplit>();
			pay=PaymentT.MakePaymentForPrepayment(pat,clinic1);
			List<PaySplits.PaySplitAssociated> retVal=PaymentEdit.AllocateUnearned(new List<AccountEntry> { new AccountEntry(proc2) },ref listPaySplitsUnearned,pay,unearnedAmt,fam);
			Assert.AreEqual(2,retVal.Count);
			Assert.AreEqual(-50,listPaySplitsUnearned.Where(x => x.UnearnedType!=0).Sum(x => x.SplitAmt));
		}

		///<summary>Make sure that when a prepayment is allocated incorrectly (not linked) that it will allocate the correct partial amount later.</summary>
		[TestMethod]
		public void PaymentEdit_AllocateUnearned_NoLinkToOriginalPrepayment() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			//create prepayment of $100
			long provNum=ProviderT.CreateProvider("SG");
			Clinic clinic1=ClinicT.CreateClinic("Clinic1");
			//create original prepayment.
			PaySplit prePay=PaySplitT.CreatePrepayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum,clinic1.ClinicNum);
			//complete a procedure
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,provNum:provNum);
			//Manually allocate prepayment without linking to the original prepayment.
			//We want to do it manually so we don't link this to the orginal prepayment correctly.
			//Not linking correctly will test that the AllocateUnearned method is implicitly linking prepayments correctly.
			PaySplitT.CreatePaySplitsForPrepayment(proc1,50,prov: provNum,clinic: clinic1);
			//Create new procedure
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",100,provNum:provNum);
			//test the PaymentEdit.AllocateUnearned() method.
			Family fam=Patients.GetFamily(pat.PatNum);
			List<PaySplit> listFamPrePaySplits=PaySplits.GetPrepayForFam(fam);
			//Should be $100
			double unearnedAmt=(double)PaySplits.GetUnearnedForFam(fam,listFamPrePaySplits);
			Assert.AreEqual(100,unearnedAmt);
			List<PaySplit> listPaySplitsUnearned=new List<PaySplit>();
			Payment pay=PaymentT.MakePaymentForPrepayment(pat,clinic1);
			List<PaySplits.PaySplitAssociated> retVal=PaymentEdit.AllocateUnearned(new List<AccountEntry> { new AccountEntry(proc2) },ref listPaySplitsUnearned,pay,unearnedAmt,fam);
			Assert.AreEqual(2,retVal.Count);
			//After running the AllocateUnearned, we should implicitly link the incorrect prepayment made when we call CreatePaySplitsForPrepayment above.
			Assert.AreEqual(-50,listPaySplitsUnearned.Where(x => x.UnearnedType!=0).Sum(x => x.SplitAmt));
		}

		///<summary>Make sure that when a we are allocating prepayments, that we do not allocate TP prepayments, and do not allocate hidden prepayments. </summary>
		[TestMethod]
		public void PaymentEdit_AllocateUnearned_NoAllocationForHiddenAndTpPrepayments() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("LS");
			Clinic clinic1=ClinicT.CreateClinic("Clinic1");
			//create tp prepayment of $100
			Procedure tpProc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.TP,"",100,provNum:provNum);
			PaySplit prePay=PaySplitT.CreateTpPrepayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum,clinic1.ClinicNum,tpProc.ProcNum);
			//make a hidden payment split that is attached to nothing
			PaySplit hiddenSplit=PaySplitT.CreateTpPrepayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum,clinic1.ClinicNum,isHidden:true);
			//complete a different procedure
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",200,provNum:provNum);
			//Allocate the unearned
			Family fam=Patients.GetFamily(pat.PatNum);
			List<PaySplit> listPaySplitsUnearned=new List<PaySplit>();
			Payment pay=PaymentT.MakePaymentForPrepayment(pat,clinic1);
			//unearned amount is only 100 because only 100 will show on the account. Unearned doesn't take into account hidden unearned. 
			List<PaySplits.PaySplitAssociated> retVal=PaymentEdit.AllocateUnearned(new List<AccountEntry> {new AccountEntry(proc1)}
				,ref listPaySplitsUnearned,pay,100,fam);
			//nothing should get allocated, because there was no valid unearned to allocate. 
			Assert.AreEqual(0,retVal.Count);
		}

		#endregion

		#region ImplicitlyLinkPrepaymentsHelper Tests

		///<summary>Various tests for allocating unearned under different circumstances (linked and unlinked splits, matching or mismatching pat/prov/clinic)</summary>
		[TestMethod]
		public void PaymentEdit_ImplicitlyLinkPrepaymentsHelper() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Patient patFam=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Patient patFamOld=patFam.Copy();
			patFam.Guarantor=pat.PatNum;
			Patients.Update(patFam,patFamOld);
			//create prepayment of $100
			long provNum=ProviderT.CreateProvider("SG");
			Clinic clinic1=ClinicT.CreateClinic("Clinic1");
			//create original prepayment.
			PaySplit prePay=PaySplitT.CreatePrepayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum,clinic1.ClinicNum);
			//complete a procedure
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,provNum:provNum);
			//Manually allocate prepayment without linking to the original prepayment.
			List<PaySplit> listPaySplits=PaySplitT.CreatePaySplitsForPrepayment(proc1,50,prov:provNum,clinic:clinic1);
			ResetPrepayments(pat);
			long nonMatch=100000;
			//test the PaymentEdit.AllocateUnearned() method.
			double unearnedAmt=(double)PaySplits.GetUnearnedForFam(Patients.GetFamily(pat.PatNum),_listFamPrePaySplits);
			//Logic check PatNum - match, ProvNum - match, ClinicNum - match
			double retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - match, ClinicNum - zero
			ResetPrepayments(pat);
			//update the clinicnum to 0
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,provNum,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//previous Test one should be $100
			ResetPrepayments(pat);
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(100,retVal);
			//Logic check PatNum - match, ProvNum - match, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,provNum,100000,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch: true,isProvNumMatch: true,isClinicNonZeroNonMatch: true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,0,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch: true,isProvNumZero:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,0,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumZero:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - non zero & non match 
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,0,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumZero:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - match 
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,nonMatch,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,nonMatch,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,nonMatch,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,provNum,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,provNum,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,provNum,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - zero, ClinicNum - match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,0,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - zero, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,0,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic checkPatNum - other family members, ProvNum - zero, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,0,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
			//Old test from above
			ResetPrepayments(pat);
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(100,retVal);
			//Logic checkPatNum - other family members, ProvNum - non zero & non match, ClinicNum - match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,nonMatch,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - non zero & non match, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,nonMatch,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - non zero & non match, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,nonMatch,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
		}

		#endregion

		#region ExplicitlyLinkCredits Tests

		/// <summary>Make sure that discount plan amounts are considered for treatment planned procs and, if there is no discount plan amount, 
		/// it uses the existing fee on the procedure</summary>
		[TestMethod]
		public void PaymentEdit_ExplicityLinkCredits_DiscountPlanAmountsForTreatmentPlannedProcedures() {
			/*****************************************************
				Create Patient:  pat1
				Create Provider: provNum
				Create DiscountPlan:  dp
				Create proc1:    Today  provNum  pat   $100
				Create proc2:    Today  provNum  pat   $100
				Create Fee:  $50
					^Attached to dp and proc1
				Create payment:  Today  provNum  pat   $10
					^No splits (not a valid payment).  Designed for AutoSplitForPayment logic.
			******************************************************/
			PrefT.UpdateYN(PrefName.PrePayAllowedForTpProcs,YN.Yes);
			string suffix=MethodBase.GetCurrentMethod().Name;
			decimal discountAmount=50;
			decimal startingProcFee=100;
			long discSched=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Discount");
			DiscountPlan dp=DiscountPlanT.CreateDiscountPlan("Discount", feeSchedNum:discSched);
			long provNum=ProviderT.CreateProvider(suffix);
			Patient pat1=PatientT.CreatePatient(suffix,discountPlanNum:dp.DiscountPlanNum,priProvNum:provNum);
			Procedure proc1=ProcedureT.CreateProcedure(pat1,"D0120",ProcStat.TP,"0",(double)startingProcFee,provNum:provNum);
			Procedure proc2=ProcedureT.CreateProcedure(pat1,"D0150",ProcStat.TP,"0",(double)startingProcFee,provNum:provNum);
			FeeT.CreateFee(discSched,proc1.CodeNum,(double)discountAmount);
			Payment payment=PaymentT.MakePayment(pat1.PatNum,10,DateTime.Now);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat1,payment,true,false);
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> { pat1.PatNum },pat1.PatNum
				,loadData.ConstructChargesData.ListPaySplits,payment,new List<AccountEntry>(),isIncomeTxfr:false);
			/*****************************************************
			AccountEntry:  Today  provNum  pat   $50
				^Attached to proc1 and had discount plan applied to AmountEnd
			AccountEntry:  Today  provNum  pat   $100
				^Attached to proc2
			******************************************************/
			Assert.AreEqual(2,constructResults.ListAccountCharges.Count);
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.AmountOriginal==startingProcFee 
				&& x.AmountEnd==discountAmount
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.AmountOriginal==startingProcFee 
				&& x.AmountEnd==startingProcFee
				&& x.ProcNum==proc2.ProcNum));
		}

		#endregion

		#region ConstructAndLinkChargeCredits Tests

		///<summary>Make sure if there are claims paid by total that they are implicitly used by a valid transfer (neg split -> pos split).</summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_NoClaimProcsInIncomeTransfer() {
			//Make procedure for Provider A worth 50.
			//Make claimproc paid by total for Provider B for 50.
			//Perform an income transfer - It should display the claimproc as a source of income and the procedure as a destination.
			//Create the income transfer (manually create a -50 for Prov B, and create a +50 on procedure for ProvA)
			//Once complete, perform an income transfer again - There should be no targets for transfer since the procedure is paid properly and claimproc is counteracted by transfer.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provA=ProviderT.CreateProvider("ProvA");
			long provB=ProviderT.CreateProvider("ProvB");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",50,provNum:provA);
			Carrier carrier=CarrierT.CreateCarrier("ABC");
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub insSub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			ClaimProcT.AddInsPaidAsTotal(pat.PatNum,plan.PlanNum,provB,50,insSub.InsSubNum,0,0);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			//Make sure that the logic creates two charges - One for the procedure (original, start, and end are 100) and one for the claimproc paid by total (original, start, and end are -50).
			Assert.AreEqual(0,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(ClaimProc)).Count);
		}

		///<summary>Make sure that if there are two procs and a payplan made with no procs attached that has a start date 4 months ago, 
		///the payment logic returns 6 owed charges, two of which are the procs and 4 of which are payplan charges.</summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanChargesWithUnattachedCredits() {
			/*****************************************************
				Create Patient:  pat
				Create proc1:    Today-4M  prov?  pat   $135
				Create proc2:    Today-4M  prov?  pat   $60
				Create payplan:  Today-3M  prov?  pat   $195
					^PayPlanCharge Credit for proc0 for the lump sum
					^PayPlanCharge Debits for 195 / 30 = 6.5 (rounded up for 7 total debits)
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-4));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-4));
			DateTime datePayPlanStart=DateTime.Today.AddMonths(-3);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,datePayPlanStart,0,totalAmt:195);
			//Go to make a payment for the charges due
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,true,false);
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,loadData.ConstructChargesData.ListPaySplits,pay,new List<AccountEntry>());
			/*****************************************************
			AccountEntry:  Today-4M  prov?  pat   $135
				^Represents proc1
			AccountEntry:  Today-4M  prov?  pat   $60
				^Represents proc2
			AccountEntry:  Today-3M  prov0  pat   $30
			AccountEntry:  Today-2M  prov0  pat   $30
			AccountEntry:  Today-1M  prov0  pat   $30
			AccountEntry:  Today     prov0  pat   $30
			AccountEntry:  Today+1M  prov0  pat   $30
			AccountEntry:  Today+2M  prov0  pat   $30
			AccountEntry:  Today+3M  prov0  pat   $15
			******************************************************/
			Assert.AreEqual(9,constructResults.ListAccountCharges.Count);
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==135
				&& x.AmountEnd==135
				&& x.Date==DateTime.Today.AddMonths(-4)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==60
				&& x.AmountEnd==60
				&& x.Date==DateTime.Today.AddMonths(-4)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==0
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==30
				&& x.Date==datePayPlanStart
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==0
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==30
				&& x.Date==datePayPlanStart.AddMonths(1)
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==0
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==30
				&& x.Date==datePayPlanStart.AddMonths(2)
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==0
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==30
				&& x.Date==datePayPlanStart.AddMonths(3)
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==0
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(4)
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==0
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(5)
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==0
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==15
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(6)
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
		}

		///<summary>Make sure that if there are two procs both attached to a payplan that has a start date of 4 months ago, that the payment logic
		///returns 4 owed charges, all four of which are payplan charges.</summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanChargesWithAttachedCredits() {
			/*****************************************************
				Create Patient:  pat
				Create proc1:    Today-4M  prov?  pat   $135
				Create proc2:    Today-4M  prov?  pat   $60
				Create payplan:  Today-3M  prov?  pat   $195
					^PayPlanCharge Credit for proc1
					^PayPlanCharge Credit for proc2
					^PayPlanCharge Debits for 195 / 30 = 6.5 (rounded up for 7 total debits)
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-4));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-4));
			DateTime datePayPlanStart=DateTime.Today.AddMonths(-3);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,datePayPlanStart,0,new List<Procedure>() {proc1,proc2});
			//Go to make a payment for the charges that are due
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,60,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,true,false);
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,loadData.ConstructChargesData.ListPaySplits,pay,new List<AccountEntry>());
			/*****************************************************
			AccountEntry:  Today-4M  prov?  pat   $135
				^Represents proc1
			AccountEntry:  Today-4M  prov?  pat   $60
				^Represents proc2
			AccountEntry:  Today-3M  prov?  pat   $30
				^Faux entry debit for proc1 / payplan combo
			AccountEntry:  Today-2M  prov?  pat   $30
				^Faux entry debit for proc1 / payplan combo
			AccountEntry:  Today-1M  prov?  pat   $30
				^Faux entry debit for proc1 / payplan combo
			AccountEntry:  Today     prov?  pat   $30
				^Faux entry debit for proc1 / payplan combo
			AccountEntry:  Today+1M  prov?  pat   $15
				^Faux entry debit for proc1 / payplan combo
			AccountEntry:  Today+1M  prov?  pat   $15
				^Faux entry debit for proc2 / payplan combo
			AccountEntry:  Today+2M  prov?  pat   $30
				^Faux entry debit for proc2 / payplan combo
			AccountEntry:  Today+3M  prov?  pat   $15
				^Faux entry debit for proc2 / payplan combo
			******************************************************/
			Assert.AreEqual(10,constructResults.ListAccountCharges.Count);
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==135
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddMonths(-4)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.ProvNum==pat.PriProv
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==60
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today.AddMonths(-4)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==proc1.ProvNum
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==30
				&& x.Date==datePayPlanStart
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==proc1.ProvNum
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==30
				&& x.Date==datePayPlanStart.AddMonths(1)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==proc1.ProvNum
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==30
				&& x.Date==datePayPlanStart.AddMonths(2)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==proc1.ProvNum
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==30
				&& x.Date==datePayPlanStart.AddMonths(3)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==proc1.ProvNum
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==15
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(4)
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==proc2.ProvNum
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==15
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(4)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==proc2.ProvNum
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==30
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(5)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.ProvNum==proc2.ProvNum
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==15
				&& x.AmountEnd==0
				&& x.Date==datePayPlanStart.AddMonths(6)
				&& x.ProcNum==proc2.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanWithAdjustments() {
			//There will be a completed procedure for $165 with a payment plan for the entire amount, all on the same day.
			//Later, add two adjustments from within the payment plan; one for $10 and then one for $20.
			//Do NOT choose to "Also make line item in Account Module" which simply removes value from the payment plan (now worth $135).
			//This leaves $30 due on the account and $135 due for the payment plan that is associated to the aforementioned procedure.
			//The other tests that are like this will associate the $10 payment plan adjustment to the procedure but we cannot do that in this scenario.
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum
				Create proc:     Today  provNum  pat   $165
				Create payplan:  Today  provNum  pat   $165
					^Associated to proc
				Create adj1:     Today  provNum  pat  -$10
					^Associated to payment plan
				Create adj2:     Today  provNum  pat  -$20
					^Associated to payment plan
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum=ProviderT.CreateProvider(suffix);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",165,DateTime.Today,provNum:provNum);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,165,DateTime.Today,listProcs:new List<Procedure>(){ proc });
			List<PayPlanCharge> listChargesAndCredits=PayPlanEdit.CreatePayPlanAdjustments(-10,PayPlanCharges.GetForPayPlan(payplan.PayPlanNum),0);
			listChargesAndCredits.Add(PayPlanChargeT.CreateNegativeCreditForAdj(pat.PatNum,payplan.PayPlanNum,-10));
			listChargesAndCredits=PayPlanEdit.CreatePayPlanAdjustments(-20,listChargesAndCredits,0);
			listChargesAndCredits.Add(PayPlanChargeT.CreateNegativeCreditForAdj(pat.PatNum,payplan.PayPlanNum,-20));
			PayPlanCharges.Sync(listChargesAndCredits,payplan.PayPlanNum);
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Now,true,0,1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,transferPayment,true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat.PatNum},pat.PatNum
				,PaySplits.GetForPayment(transferPayment.PayNum),transferPayment.PayNum,false);
			PaymentEdit.ConstructResults chargeResult=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum},pat.PatNum
				,chargeData.ListPaySplits,transferPayment,new List<AccountEntry>(),loadData:loadData);
			/*****************************************************
			AccountEntry:  Today  provNum  pat   $165
				^Represents proc.
			AccountEntry:  Today  provNum  pat   $165
				^Faux entry designed for proc / payplan combo.
			AccountEntry:  Today  provNum  pat  -$10
				^Faux entry representing an unattached adjustment debit.
			AccountEntry:  Today  provNum  pat  -$20
				^Faux entry representing an unattached adjustment debit.
			******************************************************/
			Assert.AreEqual(4,chargeResult.ListAccountCharges.Count);
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==165
				&& x.AmountEnd==30//The Tx Credit is for the full amount of this procedure, but the payment plan adjustemnt gives it value back.
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==165
				//The payment plan has $30 of adjustments attached to it which should cause this faux procedure to lose value.
				//There are no other faux procedures on the payment plan which is why this one will lose value (FIFO style).
				&& x.AmountEnd==135
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==-20
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==-10
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum
				&& x.UnearnedType==unearnedType));
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanWithAdjustmentsOnBoth() {
			//There will be a completed procedure for $165 with a payment plan for the entire amount, all on the same day.
			//Later, add two adjustments from within the payment plan; one for $10 and then one for $20.
			//Choose to "Also make line item in Account Module" which will remove value from both the payment plan and account (aka procedure).
			//The $10 adjustment in the account will be attached to the procedure but the $20 adjustment will not be attached to anything.
			//This should cause the procedure to owe $0 but the payment plan should be worth $135.
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum
				Create proc:         Today  provNum  pat   $165
				Create payplan:      Today  provNum  pat   $165
					^Associated to proc
				Create adj1:         Today  provNum  pat  -$10
					^Attached to proc
				Create payplanAdj1:  Today  provNum  pat  -$10
					^Associated to payment plan
				Create adj2:         Today  provNum  pat  -$20
					^Unattached
				Create payplanAdj2:  Today  provNum  pat  -$20
					^Associated to payment plan
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum=ProviderT.CreateProvider(suffix);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",165,DateTime.Today,provNum:provNum);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,165,DateTime.Today,listProcs:new List<Procedure>(){ proc });
			List<PayPlanCharge> listChargesAndCredits=PayPlanEdit.CreatePayPlanAdjustments(-10,PayPlanCharges.GetForPayPlan(payplan.PayPlanNum),0);
			listChargesAndCredits.Add(PayPlanChargeT.CreateNegativeCreditForAdj(pat.PatNum,payplan.PayPlanNum,-10));
			listChargesAndCredits=PayPlanEdit.CreatePayPlanAdjustments(-20,listChargesAndCredits,0);
			listChargesAndCredits.Add(PayPlanChargeT.CreateNegativeCreditForAdj(pat.PatNum,payplan.PayPlanNum,-20));
			PayPlanCharges.Sync(listChargesAndCredits,payplan.PayPlanNum);
			//Now make the two adjustments to the account (one attached to the procedure and one unattached).
			//This is actually adjusting the value of what the patient owes (aka, $135 is technically owed instead of $165).
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,-10,procNum: proc.ProcNum,provNum: proc.ProvNum);//attached adjustment
			Adjustment adj2=AdjustmentT.MakeAdjustment(pat.PatNum,-20,provNum: provNum);//unattached adjustment
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Now,true,0,1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,transferPayment,true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat.PatNum},pat.PatNum
				,PaySplits.GetForPayment(transferPayment.PayNum),transferPayment.PayNum,false);
			PaymentEdit.ConstructResults chargeResult=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum},pat.PatNum
				,chargeData.ListPaySplits,transferPayment,new List<AccountEntry>(),loadData:loadData);
			/*****************************************************
			AccountEntry:  Today  provNum  pat   $165
				^Represents proc.
			AccountEntry:  Today  provNum  pat   $10
				^Represents adj1 that is attached to proc.
			AccountEntry:  Today  provNum  pat   $20
				^Represents adj2 that is unattached.
			AccountEntry:  Today  provNum  pat   $165
				^Faux entry designed for proc / payplan combo.
			AccountEntry:  Today  provNum  pat  -$10
				^Faux entry representing an unattached adjustment debit.
			AccountEntry:  Today  provNum  pat  -$20
				^Faux entry representing an unattached adjustment debit.
			******************************************************/
			Assert.AreEqual(6,chargeResult.ListAccountCharges.Count);
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==165
				&& x.AmountEnd==0//The Tx Credit is for the full amount of this procedure, so it should never have value.
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==-10
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc.ProcNum
				&& x.AdjNum==adj1.AdjNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==-20
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0
				&& x.AdjNum==adj2.AdjNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==165
				//There are technically four faux account entries to represent the two adjustments associated to the payment plan.
				//All of the faux adjustment account entries change the AmountEnd of the faux credit account entries FIFO style.
				//$165 + -$20 + -$10 = $135 => the actual value of the entire payment plan as a whole
				&& x.AmountEnd==135
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==-20
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum
				&& ((FauxAccountEntry)x).ChargeType==PayPlanChargeType.Debit
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==-10
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum
				&& ((FauxAccountEntry)x).ChargeType==PayPlanChargeType.Debit
				&& x.UnearnedType==unearnedType));
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanWithAdjustmentsDynamic() {
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum
				Create proc:     Today  provNum  pat   $165
				Create adj1:     Today  provNum  pat  -$10
					^Attached to proc
				Create adj2:     Today  provNum  pat  -$20
				Create payplan:  Today  provNum  pat   $135
					^Associated to proc and adj2
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum=ProviderT.CreateProvider(suffix);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",165,DateTime.Today,provNum:provNum);
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,-10,procNum: proc.ProcNum,provNum: proc.ProvNum);//attached adjustment
			Adjustment adj2=AdjustmentT.MakeAdjustment(pat.PatNum,-20,provNum: provNum);//unattached adjustment
			//Create a dynamic payment plan for the entire value of the procedure (at this point is $165 + -$10(adj) + -$20 = $135).
			PayPlan dynamicPayPlan=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,pat.PatNum,DateTime.Today,0,0,135,
				new List<Procedure>(){ proc },new List<Adjustment>() { adj2 },provNum:provNum);
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today,true,0,1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,transferPayment,true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat.PatNum},pat.PatNum
				,PaySplits.GetForPayment(transferPayment.PayNum),transferPayment.PayNum,false);
			PaymentEdit.ConstructResults chargeResult=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum},pat.PatNum
				,chargeData.ListPaySplits,transferPayment,new List<AccountEntry>(),loadData:loadData);
			/*****************************************************
			AccountEntry:  Today  provNum  pat   $165
				^Represents proc
			AccountEntry:  Today  provNum  pat   $155
				^Faux entry designed for proc / payplan combo
			AccountEntry:  Today  provNum  pat   $135
				^PayPlanCharge debit.
			AccountEntry:  Today  provNum  pat  -$20
				^Faux entry representing an unattached adjustment debit.
			AccountEntry:  Today  provNum  pat  -$10
				^Represents adj1 that is attached to proc.
			AccountEntry:  Today  provNum  pat  -$20
				^Represents adj2 that is an unattached adjustment.
			******************************************************/
			Assert.AreEqual(4,chargeResult.ListAccountCharges.Count);
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==165
				&& x.AmountEnd==0//Procedure is attached to a payment plan so it has no value.
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==-10
				&& x.AmountEnd==0//Directly applied to the procedure.
				&& x.Date==DateTime.Today
				&& x.AdjNum==adj1.AdjNum
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==-20
				&& x.AmountEnd==0//Directly applied to the payment plan.
				&& x.Date==DateTime.Today
				&& x.AdjNum==adj2.AdjNum
				&& x.ProcNum==0
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				//Dynamic payment plans do not store links to adjustments that are directly linked to the procedure.
				//Thus the procedure is technically only worth $155 and so the dynamic payment plan starts off at $155.
				//However, there is an 'unattached' adjustment for the entire payment plan so only $135 can be paid.
				&& ((FauxAccountEntry)x).Principal==135
				&& x.AmountEnd==135
				&& x.Date.Date==DateTime.Today
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==dynamicPayPlan.PayPlanNum));
			//Negative adjustments will never have value because the system will never create a negative debit entry for dynamic pay plans.
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanWithAftermarketAdjustmentDynamic() {
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum
				Create proc:     Today  provNum  pat   $165
				Create adj1:     Today  provNum  pat  -$10
					^Attached to proc
				Create adj2:     Today  provNum  pat  -$20
				Create payplan:  Today  provNum  pat   $155
					^Associated to proc
				Create payPlanLink: 
					^Manually link adj2 to payplan (act like the user came in later and did this after initial creation).
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum=ProviderT.CreateProvider(suffix);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",165,DateTime.Today,provNum:provNum);
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,-10,procNum: proc.ProcNum,provNum: proc.ProvNum);//attached adjustment
			Adjustment adj2=AdjustmentT.MakeAdjustment(pat.PatNum,-20,provNum: provNum);//unattached adjustment
			//Create a dynamic payment plan for the entire value of the procedure (at this point is $165 + -$10(adj) = $155).
			PayPlan dynamicPayPlan=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,pat.PatNum,DateTime.Today,0,0,155,
				new List<Procedure>(){ proc },new List<Adjustment>(),frequency:PayPlanFrequency.Monthly,provNum:provNum);
			//The user forgot to add the second unattached adjustment to the plan.  Manually add the second adjustment.
			PayPlanLink payPlanLink=PayPlanLinkT.CreatePayPlanLink(dynamicPayPlan,adj2.AdjNum,PayPlanLinkType.Adjustment);
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0,isNew:true);
			//Create paysplits for Family
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,transferPayment,true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat.PatNum},pat.PatNum
				,PaySplits.GetForPayment(transferPayment.PayNum),transferPayment.PayNum,false);
			PaymentEdit.ConstructResults chargeResult=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum},pat.PatNum
				,chargeData.ListPaySplits,transferPayment,new List<AccountEntry>(),loadData:loadData);
			/*****************************************************
			AccountEntry:  Today  provNum  pat   $165
				^Represents proc
			AccountEntry:  Today  provNum  pat   $135
				^Faux entry designed for proc / payplan combo
			AccountEntry:  Today  provNum  pat  -$20
				^Faux entry representing an unattached adjustment debit.
			AccountEntry:  Today  provNum  pat  -$10
				^Represents adj1 that is attached to proc.
			******************************************************/
			Assert.AreEqual(4,chargeResult.ListAccountCharges.Count);
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==165
				&& x.AmountEnd==0//Procedure is attached to a payment plan so it has no value.
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==-10
				&& x.AmountEnd==0//Directly applied to the procedure.
				&& x.Date==DateTime.Today
				&& x.AdjNum==adj1.AdjNum
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==-20
				&& x.AmountEnd==0//Implicit linking killed the value?
				&& x.Date==DateTime.Today
				&& x.AdjNum==adj2.AdjNum
				&& x.ProcNum==0
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				//Dynamic payment plans do not store links to adjustments that are directly linked to the procedure.
				//Thus the procedure is technically only worth $155 and so the dynamic payment plan starts off at $155.
				&& x.AmountOriginal==155
				&& x.AmountEnd==155
				&& x.Date.Date==DateTime.Today
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==dynamicPayPlan.PayPlanNum));
			//Negative adjustments will never have value because the system will never create a negative debit entry for dynamic pay plans.
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanWithAPR() {
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum
				Create proc:     Today-2Y  provNum  pat   $1568
				Create payplan:  Today-2Y  provNum  pat   $1568
					^Associated to proc, APR=10, payment amount=$269.01, total amount=$1614.04 (including interest)
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum=ProviderT.CreateProvider(suffix);
			DateTime dateTimePP=DateTime.Today.AddYears(-2);
			Procedure proc=ProcedureT.CreateProcedure(pat,"TPPAPR",ProcStat.C,"",1568,dateTimePP,provNum:provNum);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,269.01,dateTimePP,provNum:provNum,
				listProcs:new List<Procedure>(){ proc },APR:10);
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Now,true,0,1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,transferPayment,true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long>() {pat.PatNum},pat.PatNum
				,PaySplits.GetForPayment(transferPayment.PayNum),transferPayment.PayNum,false);
			PaymentEdit.ConstructResults chargeResult=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum},pat.PatNum
				,chargeData.ListPaySplits,transferPayment,new List<AccountEntry>(),loadData:loadData);
			/*****************************************************
			AccountEntry:  Today-2Y  provNum  pat   $1568
				^Represents proc.
			AccountEntry:  Today-2Y  provNum  pat   $269.01
				^Principal = $255.94
				^Interest  = $13.07
			AccountEntry:  Today-2Y+1M  provNum  pat   $269.01
				^Principal = $258.08
				^Interest  = $10.93
			AccountEntry:  Today-2Y+2M  provNum  pat   $269.01
				^Principal = $260.23
				^Interest  = $8.78
			AccountEntry:  Today-2Y+3M  provNum  pat   $269.01
				^Principal = $262.4
				^Interest  = $6.61
			AccountEntry:  Today-2Y+4M  provNum  pat   $269.01
				^Principal = $264.58
				^Interest  = $4.43
			AccountEntry:  Today-2Y+5M  provNum  pat   $268.99
				^Principal = $266.77
				^Interest  = $2.22
			******************************************************/
			Assert.AreEqual(13,chargeResult.ListAccountCharges.Count);
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.AmountOriginal==1568
				&& x.AmountEnd==0//The Tx Credit is for the full amount of this procedure, so it should never have value.
				&& x.Date==dateTimePP
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==0));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==(decimal)255.94
				&& x.AmountEnd==(decimal)255.94
				&& x.Date==dateTimePP
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Interest==(decimal)13.07
				&& x.AmountEnd==(decimal)13.07
				&& x.Date==dateTimePP
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==(decimal)258.08
				&& x.AmountEnd==(decimal)258.08
				&& x.Date==dateTimePP.AddMonths(1)
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Interest==(decimal)10.93
				&& x.AmountEnd==(decimal)10.93
				&& x.Date==dateTimePP.AddMonths(1)
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==(decimal)260.23
				&& x.AmountEnd==(decimal)260.23
				&& x.Date==dateTimePP.AddMonths(2)
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Interest==(decimal)8.78
				&& x.AmountEnd==(decimal)8.78
				&& x.Date==dateTimePP.AddMonths(2)
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==(decimal)262.40
				&& x.AmountEnd==(decimal)262.40
				&& x.Date==dateTimePP.AddMonths(3)
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Interest==(decimal)6.61
				&& x.AmountEnd==(decimal)6.61
				&& x.Date==dateTimePP.AddMonths(3)
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==(decimal)264.58
				&& x.AmountEnd==(decimal)264.58
				&& x.Date==dateTimePP.AddMonths(4)
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Interest==(decimal)4.43
				&& x.AmountEnd==(decimal)4.43
				&& x.Date==dateTimePP.AddMonths(4)
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Principal==(decimal)266.77
				&& x.AmountEnd==(decimal)266.77
				&& x.Date==dateTimePP.AddMonths(5)
				&& x.ProcNum==proc.ProcNum
				&& x.PayPlanNum==payplan.PayPlanNum));
			Assert.AreEqual(1,chargeResult.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& ((FauxAccountEntry)x).Interest==(decimal)2.22
				&& x.AmountEnd==(decimal)2.22
				&& x.Date==dateTimePP.AddMonths(5)
				&& x.ProcNum==0
				&& x.PayPlanNum==payplan.PayPlanNum));
		}

		///<summary>Let's say a procedure was completed and then someone made a partial payment on it but to the wrong provider.
		///This unit test will execute an income transfer to move that partial payment to the correct provider and then will perform the typical
		///linking logic for the payment edit window to make sure the AmountEnd for the procedure is correct (considers pos and neg splits).</summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PreviousTransfersConsideredImplicit() {
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum1
				Create Provider: provNum2
				Create proc1:    Today  provNum1  pat   $100
				Create pay1:     Today  provNum2  pat   $10
					^Attached to proc1 but notice that the provider is wrong.
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"T2385",ProcStat.C,"",100,DateTime.Today,provNum:provNum1);
			Payment pay1=PaymentT.MakePayment(pat.PatNum,10,payDate:DateTime.Today,provNum:provNum2,procNum:proc1.ProcNum);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			//Perform an income transfer to get the $10 applied to the correct provider
			/*****************************************************
			AccountEntry:  Today  provNum2  pat  -$10
				^Attached to proc1
			AccountEntry:  Today  provNum2  pat   $10
				^Unearned
			AccountEntry:  Today  provNum2  pat  -$10
				^Unearned
			AccountEntry:  Today  provNum1  pat   $10
				^Attached to proc1
			******************************************************/
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==proc1.ProcNum
				&& x.SplitAmt==-10
				&& x.UnearnedType==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==0
				&& x.SplitAmt==10
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==0
				&& x.SplitAmt==-10
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==proc1.ProcNum
				&& x.SplitAmt==10
				&& x.UnearnedType==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
			//Now we need to make sure that the AmountEnd field is correct the next time the user loads the payment edit window.
			Payment pay2=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay2,true,false);
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> { pat.PatNum },pat.PatNum
				,loadData.ConstructChargesData.ListPaySplits,pay2,new List<AccountEntry>());
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count);
			Assert.AreEqual(1,constructResults.ListAccountCharges.Count(x => x.PatNum==pat.PatNum
				&& x.ProcNum==proc1.ProcNum
				&& x.ProvNum==provNum1
				&& x.AmountAvailable==90
				&& x.AmountEnd==90
				&& x.AmountOriginal==100
				&& x.AmountPaid==10));
		}

		///<summary>In income transfer mode,claimprocs shouldn't show  and the paysplit should.</summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_NegSplitsImplicitlyUsedByPosSources() {
			//Make a payment for Provider A for -50 (a charge)
			//Make an unattached claimproc for Provider A for 50
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provA=ProviderT.CreateProvider("ProvA");
			Carrier carrier=CarrierT.CreateCarrier("ABC");
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub insSub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			ClaimProcT.AddInsPaidAsTotal(pat.PatNum,plan.PlanNum,provA,50,insSub.InsSubNum,0,0);
			PaySplit wrongSplit=PaySplitT.CreateSplit(0,pat.PatNum,0,0,DateTime.Today,0,provA,-50,0);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			Assert.AreEqual(0,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(ClaimProc)).Count);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.AmountEnd==50).Count);//PaySplits are opposite signed as account charges. 
		}

		///<summary>Transferring income to an adjustment should no longer show that adjustment as having owed money.</summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_IncomeTransferToAdjustment() {
			//Create charge adjustment for 50
			//Create payment for 50 that's unallocated
			//Create an income transfer that takes 50 from the unallocated and allocates it to the adjustment.
			//Create new procedure for 25 and backdate it to before the adjustment
			//Create new payment and perform income transfer logic.  
			//There should display only one charge owing any money - The backdated procedure for 25.  (It is no longer implicitly paid due to adjustment link)
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("ProvA");
			Adjustment adjust1=AdjustmentT.MakeAdjustment(pat.PatNum,50,DateTime.Today,provNum:provNum);
			Payment payOld=PaymentT.MakePaymentNoSplits(pat.PatNum,50,DateTime.Today);
			PaySplit prepaySplit=PaySplitT.CreateSplit(0,pat.PatNum,payOld.PayNum,0,DateTime.Today,0,0,50,20);//Some arbitrary unearned type number
			Payment payXfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today);
			PaySplit negSplit=PaySplitT.CreateSplit(prepaySplit.ClinicNum,prepaySplit.PatNum,payXfer.PayNum,0,DateTime.Today,0,prepaySplit.ProvNum,-50,prepaySplit.UnearnedType,0);//negative split taking 50 from prepay split
			PaySplit posSplit=PaySplitT.CreateSplit(adjust1.ClinicNum,adjust1.PatNum,payXfer.PayNum,0,DateTime.Today,0,adjust1.ProvNum,50,0,adjust1.AdjNum);//positive split allocating 50 to adjustment
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",25,DateTime.Today.AddDays(-3));//pre-date procedure for before adjust.  In the old way, we'd implicitly use the income on this instead of adjustment.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(Procedure) && x.AmountEnd==25).Count);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(Adjustment) && x.AmountEnd==0).Count);
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_ExplicitlyLinkCredits_TransferFromWrongProviderSplit() {
			//To simulate a scenario where a procedure has been paid to the wrong provider and making an income transfer to correct it.
			//Problem was that after making the transfer, the original split and the offsetting split would still show in the window.
			/*****************************************************
				Create Provider: provNumA
				Create Provider: provNumB
				Create Patient: pat
				Create proc:  Today  provNumA  pat   $65
				Create pay:   Today  provNumB  pat   $65
					^Attached to proc
				Manual txfr:  Today            pat   $0
					^splitA     Today  provNumB  pat  -$65
						^Attached to proc
					^splitB     Today  provNumA  pat   $65
						^Attached to proc
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNumA=ProviderT.CreateProvider("ProvA");
			long provNumB=ProviderT.CreateProvider("ProvB");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",65,provNum:provNumA);
			Payment originalPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,65);
			PaySplit originalPaySplit=PaySplitT.CreateSplit(0,pat.PatNum,originalPayment.PayNum,0,proc.ProcDate,proc.ProcNum,provNumB,65,0);
			Payment transfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			//create income transfer manually
			PaySplit negOffsetSplit=PaySplitT.CreateSplit(0,pat.PatNum,transfer.PayNum,0,DateTime.Today,proc.ProcNum,provNumB,-65,0,0);
			PaySplit posAllocation=PaySplitT.CreateSplit(0,pat.PatNum,transfer.PayNum,0,DateTime.Today,proc.ProcNum,provNumA,65,0,0);
			//Pretend like user is going to make another transfer so we can verify nothing would show up in the window
			Payment fakeTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>()
				,fakeTransfer,new List<AccountEntry>(),true);
			Assert.AreEqual(4,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.ProvNum==provNumA
				&& x.PatNum==pat.PatNum
				&& x.AmountEnd==0
				&& x.AmountOriginal==65
				&& x.ProcNum==proc.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.ProvNum==provNumA
				&& x.PatNum==pat.PatNum
				&& x.AmountEnd==0
				&& x.AmountOriginal==-65
				&& x.ProcNum==proc.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.ProvNum==provNumB
				&& x.PatNum==pat.PatNum
				&& x.AmountEnd==65
				&& x.AmountOriginal==65
				&& x.ProcNum==proc.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.ProvNum==provNumB
				&& x.PatNum==pat.PatNum
				&& x.AmountEnd==-65
				&& x.AmountOriginal==-65
				&& x.ProcNum==proc.ProcNum));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary>Test checking and unchecking the Income Transfer Manager Window's Include Hidden Payment Splits check box.
		///Checking and unchecking the box will show or hide hidden pay splits in the Income Sources pane.</summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_CheckIncludeHiddenSplitsAndTransfersWithHidden() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			Def newUnearned=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"HiddenPrepay","x");
			Payment paymentHiddenUnearned=PaymentT.MakePayment(pat.PatNum,20,DateTime.Today,unearnedType:newUnearned.DefNum);
			Payment paymentUnearned=PaymentT.MakePayment(pat.PatNum,10,DateTime.Today);
			long provNum=ProviderT.CreateProvider("prov");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",100,provNum:provNum);
			Payment paymentTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData returnedData=new PaymentEdit.IncomeTransferData();
			//Get list of account charges without hidden pay splits.
			List<AccountEntry> listAccountCharges=PaymentEdit.ConstructAndLinkChargeCredits(fam.ListPats.Select(x => x.PatNum).ToList(),pat.PatNum,
				new List<PaySplit>(),paymentTransfer,new List<AccountEntry>(),true,false,doShowHiddenSplits:false).ListAccountCharges;
			Assert.AreEqual(0,listAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && ((PaySplit)x.Tag).UnearnedType==newUnearned.DefNum).Count);
			//Run income transfer logic and make sure hidden splits do not transfer
			returnedData=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,returnedData.ListSplitsCur.FindAll(x => x.UnearnedType==newUnearned.DefNum).Count);
			//Get list of account charges with hidden pay splits to simulate box being checked.
			listAccountCharges=PaymentEdit.ConstructAndLinkChargeCredits(fam.ListPats.Select(x => x.PatNum).ToList(),pat.PatNum,
				new List<PaySplit>(),paymentTransfer,new List<AccountEntry>(),true,false,doShowHiddenSplits:true).ListAccountCharges;
			Assert.AreEqual(1,listAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && ((PaySplit)x.Tag).UnearnedType==newUnearned.DefNum).Count);
			//Run the income transfer logic and make sure hidden splits can be transferred
			returnedData=PaymentT.BalanceAndIncomeTransfer(pat.PatNum,doIncludeHidden:true);
			Assert.AreEqual(1,returnedData.ListSplitsCur.FindAll(x => x.UnearnedType==newUnearned.DefNum).Count);
			//Uncheck box. Change it back and verify we get the same results as first run through. 
			listAccountCharges=PaymentEdit.ConstructAndLinkChargeCredits(fam.ListPats.Select(x => x.PatNum).ToList(),pat.PatNum,
				new List<PaySplit>(),paymentTransfer,new List<AccountEntry>(),true,false,doShowHiddenSplits: false).ListAccountCharges;
			Assert.AreEqual(0,listAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit) && ((PaySplit)x.Tag).UnearnedType==newUnearned.DefNum).Count);
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_UnallocatedSplitsGetTransferredAndReducedValue() {
			//Bug would make the correct transfer splits but would then not evaluate the amount correctly going into the window
			//making it seem like things still needed to be paid, which is false.
			/*****************************************************
				Create Provider: provNum
				Create Patient: pat
				Create Payment1:  Today-1  provNum  pat   $70
				Create Payment2:  Today-1  provNum  pat  -$70
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("LS");
			PaymentT.MakePayment(pat.PatNum,70,DateTime.Today.AddDays(-1),0,provNum,0,1,0,0);//positive unallocated payment
			PaymentT.MakePayment(pat.PatNum,-70,DateTime.Today.AddDays(-1),0,provNum,0,1,0,0);//negative unallocated payment
			Payment transfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults explicitLinkResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>(){pat.PatNum},pat.PatNum
				,new List<PaySplit>(),transfer,new List<AccountEntry>(),true);
			Assert.AreEqual(2,explicitLinkResults.ListAccountCharges.Count);
			Assert.AreEqual(1,explicitLinkResults.ListAccountCharges.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.AmountEnd==70
				&& x.AmountOriginal==70));
			Assert.AreEqual(1,explicitLinkResults.ListAccountCharges.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.AmountEnd==-70
				&& x.AmountOriginal==-70));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_UnallocatedTransferAndOverpaidProcedureGetLinkedCorrectly() {
			//Buggy behavior: If an unallocated split transfer happened, and there was an overpaid procedure that needed to be transferred,
			//the explicit linking logic would not correctly reduce the value on the unallocated paysplit (that had been transferred and has no value)
			//so it would be available to be transferred incorrectly.
			/*****************************************************
				Create Provider: provNum
				Create Patient: pat
				Create Payment1:    Today-2  prov0    pat   $70
				Create Payment2:    Today-1  prov0    pat  -$70
				Create proc:        Today    provNum  pat   $70
				Create Payment1:    Today    provNum  pat   $105
					^Attached to proc
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("LS");
			PaymentT.MakePayment(pat.PatNum,70,DateTime.Today.AddDays(-2));//positive unallocated payment
			PaymentT.MakePayment(pat.PatNum,-70,DateTime.Today.AddDays(-1));//negative unallocated payment
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",70,DateTime.Today,provNum:provNum);
			PaymentT.MakePayment(pat.PatNum,105,DateTime.Today,provNum:provNum,procNum:proc.ProcNum);
			Payment transferTwo=PaymentT.MakePaymentNoSplits(pat.PatNum,0,payType:1,isNew:true);
			PaymentEdit.ConstructResults linkResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>(){pat.PatNum},pat.PatNum,new List<PaySplit>()
				,transferTwo,new List<AccountEntry>(),true);
			Assert.AreEqual(3,linkResults.ListAccountCharges.FindAll(x => x.AmountEnd!=0).Count);
			Assert.AreEqual(1,linkResults.ListAccountCharges.FindAll(x => x.AmountEnd==0).Count);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			/*****************************************************
			Paysplit1:  Today  provNum  pat  -$35
				^Attached to proc
			Paysplit2:  Today  provNum  pat   $35
				^Unearned
			******************************************************/
			Assert.AreEqual(2,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-35
				&& x.ProcNum==proc.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==35
				&& x.ProcNum==0
				&& x.UnearnedType==Prefs.GetLong(PrefName.PrepaymentUnearnedType)));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_AdjustmentsAreNotCountedTwiceInTheIncomeTransferManger() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			Def unearnedType=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"unearned1");
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,unearnedType:unearnedType.DefNum);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",150,provNum:provNum);
			AdjustmentT.MakeAdjustment(pat.PatNum,-50,procNum:proc.ProcNum,provNum:proc.ProvNum);
			Payment transfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0,payType:1,isNew:true);
			PaymentEdit.ConstructResults linkResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>(){pat.PatNum},pat.PatNum,new List<PaySplit>()
				,transfer,new List<AccountEntry>(),true);
			Assert.AreEqual(2,linkResults.ListAccountCharges.FindAll(x => x.AmountEnd!=0).Count);
			//adjustment should appear in the list of items needing to be transferred.
			Assert.AreEqual(0,linkResults.ListAccountCharges.First(x => x.GetType()==typeof(Adjustment)).AmountEnd);//adjustment should be used up
			//procedure should have it's value without considering the adjustment
			Assert.AreEqual(100,linkResults.ListAccountCharges.First(x => x.GetType()==typeof(Procedure)).AmountEnd);//adjustment should have applied to proc
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PaySplitsWithMissingLinksStillBalanceCorrectly() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			Def unearnedType=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"unearned"+MethodBase.GetCurrentMethod().Name);
			Payment unearnedPay=PaymentT.MakePayment(pat.PatNum,150,DateTime.Today,unearnedType:unearnedType.DefNum,provNum:provNum);
			long unearnedSplitNum=PaySplits.GetForPayment(unearnedPay.PayNum).First().SplitNum;
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum);
			//Allocate unearned to the procedure, but with the negative split unlinked to simulate workaround given for a previous bug. 
			Payment allocatedUnearnedPayment=PaymentT.MakePayment(pat.PatNum,0,DateTime.Today);
			PaySplitT.CreateSplit(0,pat.PatNum,allocatedUnearnedPayment.PayNum,0,DateTime.MinValue,0,provNum,-100,unearnedType.DefNum);
			PaySplitT.CreateSplit(0,pat.PatNum,allocatedUnearnedPayment.PayNum,0,proc.ProcDate,proc.ProcNum,provNum,100,0);
			//Run income transfer logic - should end up with a family balance of $50 in credit
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults linkResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>(){pat.PatNum},pat.PatNum,new List<PaySplit>()
				,transferPayment,new List<AccountEntry>(),true);
			Assert.AreEqual(-50,linkResults.ListAccountCharges.Sum(x => x.AmountEnd));//charges should sum to -50 (credit)
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_RefundsBalanceToZero() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			Def unearnedType=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"unearned"+MethodBase.GetCurrentMethod().Name);
			Payment payment=PaymentT.MakePayment(pat.PatNum,30,DateTime.Today,unearnedType:unearnedType.DefNum);
			Payment refund=PaymentT.MakePayment(pat.PatNum,-30,DateTime.Today,unearnedType:unearnedType.DefNum);
			List<PaySplit> listPaymentSplits=PaySplits.GetForPayment(payment.PayNum);
			List<PaySplit> listRefundSplits=PaySplits.GetForPayment(refund.PayNum);
			PaySplits.Update(listRefundSplits.First());
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults linkResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>(){pat.PatNum},pat.PatNum,new List<PaySplit>()
				,transferPayment,new List<AccountEntry>(),true);
			Assert.AreEqual(0,linkResults.ListAccountCharges.Sum(x => x.AmountEnd));//splits should counteract each other.
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary>Assert that an adjustment can be explicitly offset by a paysplit when both are associated to the same procedure.</summary>
		[TestMethod]
		public void PaymentEdit_ConstructLinkChargeCredits_AdjustmentsLinkToPaySplitsThroughProcedures() {
			/*****************************************************
				Create Patient: pat1
				Create Provider: provNum1
				Create Provider: provNum2
				Create Procedure:  Today  provNum1  Pat1  $60
				Create Adjustment: Today  provNum2  Pat1  $10
					^Attached to Procedure
				Create Payment:    Today  provNum2  Pat1  $10
					^Attached to Procedure
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			ProcedureCode procedureCode=ProcedureCodeT.CreateProcCode("T1234");
			Procedure procedure=ProcedureT.CreateProcedure(pat1,procedureCode.ProcCode,ProcStat.C,"",60,provNum:provNum1);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat1.PatNum,10,procNum:procedure.ProcNum,provNum:provNum2);
			Payment payment=PaymentT.MakePayment(pat1.PatNum,10,DateTime.Today,provNum:provNum2,procNum:procedure.ProcNum);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat1.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat1.PatNum },pat1.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			//The AmountEnd on the adjustment and the payment should be 0 because they should explicitly link to each other via the procedure.
			Assert.AreEqual(3,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.AmountEnd==60
				&& x.AmountOriginal==60));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.AmountEnd==0
				&& x.AmountOriginal==10));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(PaySplit)
				&& x.AmountEnd==0
				&& x.AmountOriginal==-10));
			//No transfers should be made because there is no income to transfer to the production.
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary>Adjustments can be explicitly offset by a paysplit when both are associated to the same procedure.
		///However, the procedure should always be preferred over the adjustment (regardless of date).</summary>
		[TestMethod]
		public void PaymentEdit_ConstructLinkChargeCredits_AdjustmentsLinkToPaySplitsThroughProceduresLastResort() {
			/*****************************************************
				Create Patient: pat1
				Create Provider: provNum1
				Create Procedure:  Today     provNum1  Pat1  $40
				Create Adjustment: Today-5D  provNum1  Pat1 -$8
					^Attached to Procedure
				Create Payment:    Today     provNum1  Pat1  $32
					^Attached to Procedure
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			ProcedureCode procedureCode=ProcedureCodeT.CreateProcCode("T1234");
			Procedure procedure=ProcedureT.CreateProcedure(pat1,procedureCode.ProcCode,ProcStat.C,"",40,provNum:provNum1);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat1.PatNum,-8,adjDate:DateTime.Today.AddDays(-5),procNum:procedure.ProcNum,provNum:provNum1);
			Payment payment=PaymentT.MakePayment(pat1.PatNum,32,DateTime.Today,provNum:provNum1,procNum:procedure.ProcNum);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat1.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat1.PatNum },pat1.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			//The AmountEnd on the adjustment and the procedure should be 0 because the payment should explicitly link to the procedure first.
			//If the adjustment was linked to the payment first then the adjustment would end up having a value of ($32.00) for implicit linking to handle.
			Assert.AreEqual(3,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.AmountEnd==0
				&& x.AmountOriginal==40));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.AmountEnd==0
				&& x.AmountOriginal==-8));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(PaySplit)
				&& x.AmountEnd==0
				&& x.AmountOriginal==-32));
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_DynamicPayPlanDebitsLinkedToProcs() {
			//Dynamic payment plans will link PayPlanCharge debits to PayPlanCharge credit entities (like procedures) via the FKey / LinkType columns.
			//The auto-split and income transfer system should honor these links since the dynamic payment plan system went to the trouble of setting them.
			//This does not change much about the transfer system but helps the 'overpaid payment plan' report break overpayments down to a proc level.
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"TDLP1",ProcStat.C,"",58,DateTime.Today,provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"TDLP1",ProcStat.C,"",64,DateTime.Today,provNum:provNum1);
			PayPlan payPlan1=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,pat.Guarantor,DateTime.Today,35,0,35,
				new List<Procedure>(){ proc2,proc1 },new List<Adjustment>(),frequency:PayPlanFrequency.Monthly,provNum:provNum1);
			//There should be several PayPlanCharge entries and some should be linked to the procedures via FKey.
			List<PayPlanCharge> listPayPlanCharges=PayPlanCharges.GetForPayPlan(payPlan1.PayPlanNum);
			Assert.AreEqual(3,listPayPlanCharges.Count);
			Assert.AreEqual(1,listPayPlanCharges.Count(x => x.ChargeDate==DateTime.Today
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.Principal==35
				&& x.FKey==proc2.ProcNum
				&& x.LinkType==PayPlanLinkType.Procedure));
			Assert.AreEqual(1,listPayPlanCharges.Count(x => x.ChargeDate==DateTime.Today
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.Principal==29
				&& x.FKey==proc2.ProcNum
				&& x.LinkType==PayPlanLinkType.Procedure));
			Assert.AreEqual(1,listPayPlanCharges.Count(x => x.ChargeDate==DateTime.Today
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.Principal==6
				&& x.FKey==proc1.ProcNum
				&& x.LinkType==PayPlanLinkType.Procedure));
			//ConstructAndLinkChargeCredits should honor the procedures set within the FKey column of the PayPlanCharge entries.
			//All FauxAccountEntries should be exact replicas of the PayPlanCharges above.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			Assert.AreEqual(5,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==0
				&& x.ProcNum==proc1.ProcNum
				&& x.AmountOriginal==58
				&& x.AmountEnd==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==0
				&& x.ProcNum==proc2.ProcNum
				&& x.AmountOriginal==64
				&& x.AmountEnd==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.ProcNum==proc2.ProcNum
				&& ((FauxAccountEntry)x).Principal==35
				&& x.AmountEnd==35));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.ProcNum==proc2.ProcNum
				&& ((FauxAccountEntry)x).Principal==29
				&& x.AmountEnd==29));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.ProcNum==proc1.ProcNum
				&& ((FauxAccountEntry)x).Principal==6
				&& x.AmountEnd==6));
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_DynamicPayPlanDebitsLinkedToAdjs() {
			//Dynamic payment plans will link PayPlanCharge debits to PayPlanCharge credit entities (like adjustments) via the FKey / LinkType columns.
			//The auto-split and income transfer system should honor these links since the dynamic payment plan system went to the trouble of setting them.
			//This does not change much about the transfer system but may be used by some entity in the future (like reports).
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Def defPosAdjType=DefT.CreateDefinition(DefCat.AdjTypes,suffix,"+");
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider(suffix);
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,50,adjType:defPosAdjType.DefNum);//Make a positive adjustment.
			PayPlan payPlan1=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,pat.Guarantor,DateTime.Today,0,0,50,
				new List<Procedure>(),new List<Adjustment>() { adj1 },frequency:PayPlanFrequency.Monthly,provNum:provNum1);
			//There should be a PayPlanCharge entry linked to the adjustment via FKey.
			List<PayPlanCharge> listPayPlanCharges=PayPlanCharges.GetForPayPlan(payPlan1.PayPlanNum);
			Assert.AreEqual(1,listPayPlanCharges.Count);
			Assert.AreEqual(1,listPayPlanCharges.Count(x => x.ChargeDate==DateTime.Today
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.Principal==50
				&& x.FKey==adj1.AdjNum
				&& x.LinkType==PayPlanLinkType.Adjustment));
			//ConstructAndLinkChargeCredits should honor the AdjNum set within the FKey column of the PayPlanCharge entries.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			Assert.AreEqual(2,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==0
				&& x.ProcNum==0
				&& x.AdjNum==adj1.AdjNum
				&& x.AmountOriginal==50
				&& x.AmountEnd==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.ProcNum==0
				&& x.AdjNum==adj1.AdjNum
				&& x.AmountOriginal==50
				&& x.AmountEnd==50));
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanProcCreditWithAdj() {
			/*****************************************************
				Proc1	provNum1	1000
				PP1		provNum1	1250
					PPC1	provNum1	1250	Attached to Proc1
				Adj1	provNum1	250
					^attached to Proc1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"TPPPCWA",ProcStat.C,"",1000,provNum:provNum1);
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,250,procNum:proc1.ProcNum,provNum:provNum1);
			PayPlan payPlan1=PayPlanT.CreatePayPlanNoCharges(pat.PatNum,1250,DateTime.Today,provNum:provNum1);
			//Make a manual PayPlanCharge credit which will be for the sum of proc1 + adj1 (we suggest this to users via the UI).
			PayPlanCharge ppc1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1250,provNum:provNum1,
				chargeType:PayPlanChargeType.Credit,procNum:proc1.ProcNum);
			PayPlanCharge ppd1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1250,provNum:provNum1,
				chargeType:PayPlanChargeType.Debit);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $1000
				^Represents proc1
			PayPlanCharge:     Today  provNum1  pat   $1250
				^Represents ppd1 and proc1
			AccountEntry:      Today  provNum1  pat   $250
				^Represents adj1
			******************************************************/
			Assert.AreEqual(3,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==1000
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==250
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.AdjNum==0
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& ((FauxAccountEntry)x).Principal==1250
				&& x.AmountEnd==1250
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanProcCreditWithAdjDiffProv() {
			/*****************************************************
				Proc1	provNum1	1000
				PP1		provNum1	1250
					PPC1	provNum1	1250	Attached to Proc1
				Adj1	provNum2	250
					^attached to Proc1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Patient pat=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"TPPPCWA",ProcStat.C,"",1000,provNum:provNum1);
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,250,procNum:proc1.ProcNum,provNum:provNum2);
			PayPlan payPlan1=PayPlanT.CreatePayPlanNoCharges(pat.PatNum,1250,DateTime.Today,provNum:provNum1);
			//Make a manual PayPlanCharge credit which will be for the sum of proc1 + adj1 (we suggest this to users via the UI).
			PayPlanCharge ppc1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1250,provNum:provNum1,
				chargeType:PayPlanChargeType.Credit,procNum:proc1.ProcNum);
			PayPlanCharge ppd1=PayPlanChargeT.CreateOne(payPlan1.PayPlanNum,pat.Guarantor,pat.PatNum,DateTime.Today,1250,provNum:provNum1,
				chargeType:PayPlanChargeType.Debit);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),false,false);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $1000
				^Represents proc1
			FauxAccountEntry:  Today  provNum1  pat   $1250
				^Represents ppd1 and proc1
			AccountEntry:      Today  provNum2  pat   $250
				^Represents adj1
			******************************************************/
			Assert.AreEqual(3,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==1000
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum1
				&& ((FauxAccountEntry)x).Principal==1250//The PayPlanCharge credit was set to 1250 even though this is technically more than the value of the proc.
				&& x.AmountEnd==1250//This amount will reach 1250 because there will be debits that are due (regardless of anything else).
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.ProvNum==provNum2
				&& x.AmountOriginal==250
				&& x.AmountEnd==250//This adjustment is for a different pat/prov/clinic combo so it cannot be 'morphed' into the procedure's value.
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			//The patient technically owes $1500 because the office messed up on the manual adjustment that they created.
			//$1250 to provNum1 payPlan1 and $250 to provNum2 adj1.
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanPayFirstEntries() {
			/*****************************************************
				patient:  pat1
				provider: provNum1
				proc1     Today  pat1  provNum1   $200
				proc2     Today  pat1  provNum1   $75
				proc3     Today  pat1  provNum1   $75
				payPlan1  Today  pat1  provNum1   $150
					^ppc proc2
					^ppc proc3
					^ppd (entire plan due today)
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Patient pat1=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat1,"PPPFE",ProcStat.C,"",200,provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat1,"PPPFE",ProcStat.C,"",75,provNum:provNum1);
			Procedure proc3=ProcedureT.CreateProcedure(pat1,"PPPFE",ProcStat.C,"",75,provNum:provNum1);
			PayPlan payPlan1=PayPlanT.CreatePayPlanWithCredits(pat1.PatNum,150,DateTime.Today,provNum:provNum1,
				listProcs:new List<Procedure>(){ proc2,proc3 });
			List<PayPlanCharge> listPayPlanCharges=PayPlanCharges.GetDueForPayPlan(payPlan1,pat1.PatNum);
			//Find the debit for the payment plan and act like the user selected it within the account module and wants to pay it first.
			List<AccountEntry> listPayFirstEntries=listPayPlanCharges.Where(x => x.ChargeType==PayPlanChargeType.Debit).Select(x => new AccountEntry(x)).ToList();
			Payment payCur=PaymentT.MakePaymentNoSplits(pat1.PatNum,200);//Make a payment for all of the payment plan and some of proc1.
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat1.PatNum },pat1.PatNum,new List<PaySplit>(),
				payCur,listPayFirstEntries);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $200
				^Represents proc1
			AccountEntry:      Today  provNum1  pat   $75
				^Represents proc2
			AccountEntry:      Today  provNum1  pat   $75
				^Represents proc3
			FauxAccountEntry:  Today  provNum1  pat   $75
				^Represents ppd1 and proc2
			FauxAccountEntry:  Today  provNum1  pat   $75
				^Represents ppd1 and proc3
			******************************************************/
			Assert.AreEqual(5,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==200
				&& x.AmountEnd==200
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==75
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==75
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc3.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& ((FauxAccountEntry)x).Principal==75
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& ((FauxAccountEntry)x).Principal==75
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc3.ProcNum));
			//The implicit linking should have preferred the "pay these first" account entries passed in (payment plan related entries).
			Assert.AreEqual(2,results.ListSplitsCur.Count);
			Assert.AreEqual(1,results.ListSplitsCur.Count(x => x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& x.SplitAmt==75
				&& x.AdjNum==0
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListSplitsCur.Count(x => x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& x.SplitAmt==75
				&& x.AdjNum==0
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.ProcNum==proc3.ProcNum));
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_DynamicPayPlanPayFirstEntries() {
			/*****************************************************
				patient:  pat1
				provider: provNum1
				proc1     Today  pat1  provNum1   $200
				proc2     Today  pat1  provNum1   $75
				proc3     Today  pat1  provNum1   $75
				payPlan1  Today  pat1  provNum1   $150
					^ppc proc2
					^ppc proc3
					^ppd (entire plan due today)
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Patient pat1=PatientT.CreatePatient(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat1,"PPPFE",ProcStat.C,"",200,provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat1,"PPPFE",ProcStat.C,"",75,provNum:provNum1);
			Procedure proc3=ProcedureT.CreateProcedure(pat1,"PPPFE",ProcStat.C,"",75,provNum:provNum1);
			PayPlan payPlan1=PayPlanT.CreateDynamicPaymentPlan(pat1.PatNum,pat1.Guarantor,DateTime.Today,0,0,150,
				listProcs:new List<Procedure>(){ proc2,proc3 },new List<Adjustment>(),provNum:provNum1);
			List<PayPlanCharge> listPayPlanCharges=PayPlanCharges.GetDueForPayPlan(payPlan1,pat1.PatNum);
			//Find the debit for the payment plan and act like the user selected it within the account module and wants to pay it first.
			List<AccountEntry> listPayFirstEntries=listPayPlanCharges.Where(x => x.ChargeType==PayPlanChargeType.Debit).Select(x => new AccountEntry(x)).ToList();
			Payment payCur=PaymentT.MakePaymentNoSplits(pat1.PatNum,200);//Make a payment for all of the payment plan and some of proc1.
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat1.PatNum },pat1.PatNum,new List<PaySplit>(),
				payCur,listPayFirstEntries);
			/*****************************************************
			AccountEntry:      Today  provNum1  pat   $200
				^Represents proc1
			AccountEntry:      Today  provNum1  pat   $75
				^Represents proc2
			AccountEntry:      Today  provNum1  pat   $75
				^Represents proc3
			FauxAccountEntry:  Today  provNum1  pat   $75
				^Represents ppd1 and proc2
			FauxAccountEntry:  Today  provNum1  pat   $75
				^Represents ppd1 and proc3
			******************************************************/
			Assert.AreEqual(5,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==200
				&& x.AmountEnd==200
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc1.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==75
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& x.AmountOriginal==75
				&& x.AmountEnd==0
				&& x.Date==DateTime.Today
				&& x.ProcNum==proc3.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& ((FauxAccountEntry)x).Principal==75
				&& x.AmountEnd==0
				&& x.Date.Date==DateTime.Today
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& ((FauxAccountEntry)x).Principal==75
				&& x.AmountEnd==0
				&& x.Date.Date==DateTime.Today
				&& x.ProcNum==proc3.ProcNum));
			//The implicit linking should have preferred the "pay these first" account entries passed in (payment plan related entries).
			Assert.AreEqual(2,results.ListSplitsCur.Count);
			Assert.AreEqual(1,results.ListSplitsCur.Count(x => x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& x.SplitAmt==75
				&& x.AdjNum==0
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.ProcNum==proc2.ProcNum));
			Assert.AreEqual(1,results.ListSplitsCur.Count(x => x.PatNum==pat1.PatNum
				&& x.ProvNum==provNum1
				&& x.SplitAmt==75
				&& x.AdjNum==0
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.ProcNum==proc3.ProcNum));
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_DynamicPayPlanDebitAdjsHaveValue() {
			//FauxAccountEntries will get created for adjustments attached to dynamic payment plans.
			//The AmountEnd from the corresponding PayPlanCharge (debit) entry needs to flow into the FauxAccountEntry (credit).
			//Procedures already have this functionality and the UI is already prepared for this to happen.
			//Without this value moving in this way, FauxAccountEntries that are associated to procedures could get more value than they should.
			string suffix=MethodBase.GetCurrentMethod().Name;
			Def defPosAdjType=DefT.CreateDefinition(DefCat.AdjTypes,suffix,"+");
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider(suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"",ProcStat.C,"",60,DateTime.Today,provNum:provNum1);
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,20,adjType:defPosAdjType.DefNum);//Make a positive adjustment.
			PayPlan payPlan1=PayPlanT.CreateDynamicPaymentPlan(pat.PatNum,pat.Guarantor,DateTime.Today,0,0,80,
				new List<Procedure>() { proc1 },new List<Adjustment>() { adj1 },frequency:PayPlanFrequency.Monthly,provNum:provNum1);
			List<PayPlanCharge> listPayPlanCharges=PayPlanCharges.GetForPayPlan(payPlan1.PayPlanNum);
			Assert.AreEqual(2,listPayPlanCharges.Count);
			Assert.AreEqual(1,listPayPlanCharges.Count(x => x.ChargeDate==DateTime.Today
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.Principal==60
				&& x.FKey==proc1.ProcNum
				&& x.LinkType==PayPlanLinkType.Procedure));
			Assert.AreEqual(1,listPayPlanCharges.Count(x => x.ChargeDate==DateTime.Today
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.Principal==20
				&& x.FKey==adj1.AdjNum
				&& x.LinkType==PayPlanLinkType.Adjustment));
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			Assert.AreEqual(4,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==0
				&& x.ProcNum==proc1.ProcNum
				&& x.AdjNum==0
				&& x.AmountOriginal==60
				&& x.AmountEnd==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==0
				&& x.ProcNum==0
				&& x.AdjNum==adj1.AdjNum
				&& x.AmountOriginal==20
				&& x.AmountEnd==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.ProcNum==proc1.ProcNum
				&& x.AdjNum==0
				&& x.AmountOriginal==60
				&& x.AmountEnd==60));//This unit test would typically fail here stating that the value was $80 (adjustment value applied to it).
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.PatNum==pat.PatNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.ProcNum==0
				&& x.AdjNum==adj1.AdjNum
				&& x.AmountOriginal==20
				&& x.AmountEnd==20));
		}

		///<summary>Overpaid (negative AmountEnd) procedures should not be implicitly linked to.</summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_DoNotImplicitlyLinkToOverpaidProcs() {
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum1
				Create proc1:    Today  provNum1  pat   $60
				Create proc2:    Today  provNum1  pat   $20
				Create adj1:     Today  provNum1  pat  -$30
					^Attached to proc1
				Create adj2:     Today  provNum1  pat  -$10
					^Attached to proc2
				Create pay1:     Today  provNum1  pat   $80
					^paySplit1:   Today  provNum1  pat   $60
						^^Attached to proc1
					^paySplit2:   Today  provNum1  pat   $10
						^^Attached to proc2
					^paySplit3:   Today  provNum1  pat   $10
						^^Attached to unearned
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"DNILTOP1",ProcStat.C,"",60,DateTime.Today,provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"DNILTOP2",ProcStat.C,"",20,DateTime.Today,provNum:provNum1);
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,-30,procNum:proc1.ProcNum,provNum:provNum1);
			Adjustment adj2=AdjustmentT.MakeAdjustment(pat.PatNum,-10,procNum:proc2.ProcNum,provNum:provNum1);
			Payment pay1=PaymentT.MakePaymentNoSplits(pat.PatNum,10,payDate:DateTime.Today,doInsert:false);
			PaySplit paySplit1=PaySplitT.CreateOne(pat.PatNum,60,pay1.PayNum,provNum1,procNum:proc1.ProcNum);
			PaySplit paySplit2=PaySplitT.CreateOne(pat.PatNum,10,pay1.PayNum,provNum1,procNum:proc2.ProcNum);
			PaySplit paySplit3=PaySplitT.CreateOne(pat.PatNum,10,pay1.PayNum,provNum1,unearnedType:unearnedType);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			//proc1 should be in the negative due to how the entities are linked above. Implicit linking should do nothing for proc1.
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>());
			Assert.AreEqual(4,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.AdjNum==0
				&& x.AmountOriginal==60
				&& x.AmountEnd==-30//This is where the bug was, the $10 split to unearned was implicitly linking to this and setting it to $0 (very wrong).
				&& x.ProvNum==provNum1
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==proc1.ProcNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure)
				&& x.AdjNum==0
				&& x.AmountOriginal==20
				&& x.AmountEnd==0
				&& x.ProvNum==provNum1
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==proc2.ProcNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.AdjNum==adj1.AdjNum
				&& x.AmountOriginal==-30
				&& x.AmountEnd==0
				&& x.ProvNum==provNum1
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==proc1.ProcNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.AdjNum==adj2.AdjNum
				&& x.AmountOriginal==-10
				&& x.AmountEnd==0
				&& x.ProvNum==provNum1
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==proc2.ProcNum
				&& x.UnearnedType==0));
			Assert.AreEqual(0,results.ListSplitsCur.Count);
		}

		///<summary>Negative adjustments should implicitly link to negative paysplits.</summary>
		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_ImplicitlyLinkNegativeAdjustments() {
			/*****************************************************
				Create Patient:  pat
				Create Provider: provNum1
				Create adj1:  Today  provNum1  pat  -$60
				Create pay1:  Today  provNum1  pat   $60
					^Unearned
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Adjustment adj1=AdjustmentT.MakeAdjustment(pat.PatNum,-60,provNum:provNum1);
			Payment pay1=PaymentT.MakePayment(pat.PatNum,-60,payDate:DateTime.Today,unearnedType:unearnedType);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			//Implicit linking should tie the negative adjustment and the negative paysplit on pay1 together thus making the adj account entry worth $0.
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>());
			Assert.AreEqual(1,results.ListAccountCharges.Count);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment)
				&& x.AdjNum==adj1.AdjNum
				&& x.AmountOriginal==-60
				&& x.AmountEnd==0
				&& x.ProvNum==provNum1
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(0,results.ListSplitsCur.Count);
		}

		#endregion

		#region CreateTransferForTpProcs Tests

		#endregion

		#region BalanceAndIncomeTransfer Tests

		#region Payment Plans

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_PayPlanPaidWrongClinic() {
			/*****************************************************
				Create Provider: prov1
				Create Patient:  pat1
				Create Clinic:   clinic1
				Create paymentPlan1:    1/1/2019  prov1  pat1    HQ      $100
				Create payPlanCharge1:  1/1/2019  prov1  pat1    HQ      $100
				Create payment1:        1/2/2019  prov1  pat1  clinic1   $25
					^Attached to paymentplan1
			******************************************************/
			PrefT.UpdateBool(PrefName.EasyNoClinics,false);//Not-no clinics
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Clinic clinic1=ClinicT.CreateClinic($"{suffix}-1");
			PayPlan paymentPlan1=PayPlanT.CreatePayPlan(pat1.PatNum,100,100,new DateTime(2019,1,1),provNum1);
			PayPlanCharge payPlanCharge1=PayPlanChargeT.CreateOne(paymentPlan1.PayPlanNum,pat1.PatNum,pat1.PatNum,new DateTime(2019,1,1),100,
				provNum:provNum1,chargeType:PayPlanChargeType.Credit);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,25,payDate:new DateTime(2019,1,2),provNum:provNum1,payPlanNum:paymentPlan1.PayPlanNum,
				clinicNum:clinic1.ClinicNum);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  Today  prov1  pat1  clinic1  -$25
				^Attached to paymentPlan1
			Paysplit2:  Today  prov1  pat1  clinic1   $25
				^Attached to Unearned
			Paysplit3:  Today  prov1  pat1  clinic1  -$25
				^Attached to Unearned
			Paysplit4:  Today  prov1  pat1    HQ      $25
				^Attached to paymentPlan1
			******************************************************/
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-25
				&& x.PayPlanNum==paymentPlan1.PayPlanNum
				&& x.ClinicNum==clinic1.ClinicNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==25
				&& x.PayPlanNum==0
				&& x.ClinicNum==clinic1.ClinicNum
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-25
				&& x.PayPlanNum==0
				&& x.ClinicNum==clinic1.ClinicNum
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==25
				&& x.PayPlanNum==paymentPlan1.PayPlanNum
				&& x.ClinicNum==0
				&& x.UnearnedType==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_PayPlanNoProcOverpaid() {
			/*****************************************************
				Create Provider: prov1
				Create Patient:  pat1
				Create payPlan1:    Today  prov1  pat1   $100
				Create payment1:    Today  prov1  pat1   $250
					^Attached to paymentplan1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			PayPlan paymentPlan1=PayPlanT.CreatePayPlanWithCredits(pat1.PatNum,100,DateTime.Today,provNum:provNum1,totalAmt:100);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,250,payDate:DateTime.Today,provNum:provNum1,payPlanNum:paymentPlan1.PayPlanNum);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  Today  prov1  pat1  -$150
				^Attached to paymentPlan1
			Paysplit2:  Today  prov1  pat1   $150
				^Attached to Unearned
			******************************************************/
			Assert.AreEqual(2,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-150
				&& x.PayPlanNum==paymentPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==150
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_PayPlanNoProcWrongProv() {
			/*****************************************************
				Create Provider: prov1
				Create Provider: prov2
				Create Patient:  pat1
				Create payPlan1:    Today  prov2  pat1   $100
				Create payment1:    Today  prov1  pat1   $250
					^Attached to paymentplan1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			PayPlan paymentPlan1=PayPlanT.CreatePayPlanWithCredits(pat1.PatNum,100,DateTime.Today,provNum:provNum2,totalAmt:100);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,250,payDate:DateTime.Today,provNum:provNum1,payPlanNum:paymentPlan1.PayPlanNum);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  Today  prov1  pat1  -$250
				^Attached to paymentPlan1
			Paysplit2:  Today  prov1  pat1   $250
				^Attached to Unearned
			Paysplit3:  Today  prov1  pat1  -$100
				^Attached to Unearned
			Paysplit4:  Today  prov2  pat1   $100
				^Attached to paymentPlan1
			******************************************************/
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-250
				&& x.PayPlanNum==paymentPlan1.PayPlanNum
				&& x.UnearnedType==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==250
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-100
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==100
				&& x.PayPlanNum==paymentPlan1.PayPlanNum
				&& x.UnearnedType==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_PayPlanMultipleProcsWrongPayment() {
			/*****************************************************
				Create Provider: provNum1
				Create Provider: provNum2
				Create Patient:  pat1
				Create Patient:  pat2
				Create procedure1:      Today-5M  provNum1  pat1   $30
				Create procedure2:      Today-5M  provNum1  pat1   $10
				Create payPlan1:        Today-4M  provNum1  pat1   $100
				Create payPlanCharge1:  Today-4M  provNum1  pat1   $30
					^Attached to procedure1 and payPlan1
				Create payPlanCharge2:  Today-4M  provNum1  pat1   $10
					^Attached to procedure2 and payPlan1
				Create payPlanCharge3:  Today-4M  provNum1  pat1   $60
					^Attached to nothing (just meant to have enough credits on the payplan to equate to the total amount).
				Create payment1:        Today     provNum2  pat2   $40
					^Attached to payPlan1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat1=PatientT.CreatePatient($"{suffix}-1");
			Patient pat2=PatientT.CreatePatient($"{suffix}-2");
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"T8793",ProcStat.C,"",30,procDate:DateTime.Today.AddMonths(-5),provNum:provNum1);
			Procedure procedure2=ProcedureT.CreateProcedure(pat1,"T8794",ProcStat.C,"",10,procDate:DateTime.Today.AddMonths(-5),provNum:provNum1);
			DateTime datePayPlan=DateTime.Today.AddMonths(-4);
			PayPlan payPlan1=PayPlanT.CreatePayPlanWithCredits(pat1.PatNum,10,datePayPlan,provNum:provNum1,
				listProcs:new List<Procedure>(){ procedure1,procedure2 },totalAmt:100);
			//Make a payment to the wrong patient and provider but attach it to the payment plan.
			Payment payment1=PaymentT.MakePayment(pat2.PatNum,40,payDate:DateTime.Today,provNum:provNum2,payPlanNum:payPlan1.PayPlanNum);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  Today  prov2  pat2  -$40
				^Attached to payPlan1
			Paysplit2:  Today  prov2  pat2   $40
				^Attached to Unearned
			Paysplit3:  Today  prov2  pat2  -$10
				^Attached to Unearned
			Paysplit4:  Today  prov1  pat1   $10
				^Attached to payPlan1 and procedure1
			Paysplit3:  Today  prov2  pat2  -$10
				^Attached to Unearned
			Paysplit4:  Today  prov1  pat1   $10
				^Attached to payPlan1 and procedure1
			Paysplit3:  Today  prov2  pat2  -$10
				^Attached to Unearned
			Paysplit4:  Today  prov1  pat1   $10
				^Attached to payPlan1 and procedure1
			Paysplit5:  Today  prov2  pat2  -$10
				^Attached to Unearned
			Paysplit6:  Today  prov1  pat1   $10
				^Attached to payPlan1 and procedure2
			******************************************************/
			Assert.AreEqual(10,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat2.PatNum
				&& x.SplitAmt==-40
				&& x.ProcNum==0
				&& x.PayPlanNum==payPlan1.PayPlanNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat2.PatNum
				&& x.SplitAmt==40
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat2.PatNum
				&& x.SplitAmt==-10
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(3,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==10
				&& x.ProcNum==procedure1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==10
				&& x.ProcNum==procedure2.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_PayPlanWithUnearnedOut() {
			//Payment plans with extra unearned money attached should be able to transfer that money out of the pay plan.
			/*****************************************************
				Create Provider: provNum1
				Create Patient:  pat1
				Create payPlan1:        Today     provNum1  pat1   $100
				Create procedure1:      Today     provNum1  pat1   $100
				Create procedure2:      Today     provNum1  pat1   $60
				Create payPlanCharge1:  Today     provNum1  pat1   $100
					^Credit - Attached to procedure1 and payPlan1
				Create payPlanCharge1:  Today+1M  provNum1  pat1   $100
					^Debit - Not due for another month!  This plan has no value yet!
				Create payment1:        Today     provNum1  pat1   $10
					^Attached to payPlan1 (this is like a manual prepayment)
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient($"{suffix}-1");
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"T8793",ProcStat.C,"",100,procDate:DateTime.Today,provNum:provNum1);
			Procedure procedure2=ProcedureT.CreateProcedure(pat1,"T8794",ProcStat.C,"",60,procDate:DateTime.Today,provNum:provNum1);
			PayPlan payPlan1=PayPlanT.CreatePayPlanWithCredits(pat1.PatNum,100,DateTime.Today.AddMonths(1),provNum:provNum1,
				listProcs:new List<Procedure>(){ procedure1 });
			//Make a prepayment and attach it to the payment plan.  The payment plan does not have any value yet so this is money that should be for proc2.
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,10,payDate:DateTime.Today,payPlanNum:payPlan1.PayPlanNum,provNum:provNum1,
				unearnedType:unearnedType);
			//Make an income transfer to get the prepayment out of the payment plan and apply it towards proc2.
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-10
				&& x.ProcNum==0
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==unearnedType));//This is only set because the original payment was to unearned.
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==10
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-10
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==10
				&& x.ProcNum==procedure2.ProcNum
				&& x.PayPlanNum==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_PayPlanWithUnearnedIn() {
			//Prepayments should be able to transfer into pay plans.
			/*****************************************************
				Create Provider: provNum1
				Create Patient:  pat1
				Create payPlan1:        Today     provNum1  pat1   $100
				Create procedure1:      Today     provNum1  pat1   $100
				Create payPlanCharge1:  Today     provNum1  pat1   $100
					^Credit - Attached to procedure1 and payPlan1
				Create payPlanCharge1:  Today     provNum1  pat1   $10
					^Debit - Due right now.
				Create payPlanCharge2:  Today+1M  provNum1  pat1   $90
					^Debit - Not due for another month.
				Create payment1:        Today     provNum0  pat1   $10
					^Prepayment that should be available for transfer.  Notice that there is no provider set.
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient($"{suffix}-1");
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"T8793",ProcStat.C,"",100,procDate:DateTime.Today,provNum:provNum1);
			PayPlan payPlan1=PayPlanT.CreatePayPlanWithCredits(pat1.PatNum,100,DateTime.Today.AddMonths(1),provNum:provNum1,
				listProcs:new List<Procedure>(){ procedure1 },downPayment:10);
			//Make a prepayment that is not associated to any provider.
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,10,payDate:DateTime.Today,provNum:0,unearnedType:unearnedType);
			//Make an income transfer to get the prepayment into the payment plan.
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(2,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==0
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-10
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==10
				&& x.ProcNum==procedure1.ProcNum
				&& x.PayPlanNum==payPlan1.PayPlanNum
				&& x.UnearnedType==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_PayPlansWithOneProc() {
			//Have multiple patient payment plans that both have PayPlanCharge credits for the same procedure.
			/*****************************************************
				Create Provider: provNum1
				Create Patient:  pat1
				Create procedure1:      Today     provNum1  pat1   $75
				Create payPlan1:        Today     provNum1  pat1   $50
				Create payPlan2:        Today     provNum1  pat1   $25
				Create payPlanCharge1:  Today     provNum1  pat1   $50
					^Credit - Attached to procedure1 and payPlan1
				Create payPlanCharge2:  Today     provNum1  pat1   $25
					^Credit - Attached to procedure1 and payPlan2
				Create payPlanCharge3:  Today     provNum1  pat1   $50
					^Debit - Attached to payPlan1, everything is due right now
				Create payPlanCharge4:  Today     provNum1  pat1   $25
					^Debit - Attached to payPlan2, everything is due right now
				Create payment1:        Today     provNum1  pat1   $100  -  notice that this is too much money for the proc / payplans.
				Create paySplit1:       Today     provNum1  pat1   $100
					^Attached to payPlan1
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient($"{suffix}-1");
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"PPWOP",ProcStat.C,"",75,procDate:DateTime.Today,provNum:provNum1);
			//Create a payment plan that doesn't cover the entire procedure amount.  There will be a second one created for that.
			PayPlan payPlan1=PayPlanT.CreatePayPlanWithCredits(pat1.PatNum,50,DateTime.Today,provNum:provNum1,
				listProcs:new List<Procedure>(){ procedure1 },totalAmt:50);
			//Create a second payment plan attached to the same procedure as the first payment plan (not possible with dynamic payment plans).
			PayPlan payPlan2=PayPlanT.CreatePayPlanWithCredits(pat1.PatNum,25,DateTime.Today,provNum:provNum1,
				listProcs:new List<Procedure>(){ procedure1 },totalAmt:25);
			//Both payment plans are due (the full $75).  Make a payment that is for more than what the patient owes, attached to the first payment plan.
			//The income transfer system should be able to pick up on this, move money to unearned and then apply it to the next payment plan.
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,100,payDate:DateTime.Today,payPlanNum:payPlan1.PayPlanNum,provNum:provNum1);
			//Make an income transfer to get the excess payment out of the first payment plan and into the second one with leftovers into unearned.
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-50
				&& x.ProcNum==0
				&& x.PayPlanNum==payPlan1.PayPlanNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==50
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType > 0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-25
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType > 0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==25
				&& x.ProcNum==procedure1.ProcNum
				&& x.PayPlanNum==payPlan2.PayPlanNum));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		#endregion

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_TransferPreDatesProcedure() {
			//Create procedure for date X, create payment (unallocated) for date X.  
			//Create transfer to transfer from unallocated to procedure, but on date X-Y, where Y is some time.  
			//There should be no charges for transfer.
			/*****************************************************
				Create Provider: provNum1
				Create Patient: pat
				Create Procedure1:  Today-1M  provNum1  pat   $100
				Create Payment1:    Today-1M  provNum1  pat   $100
					^Unearned
				Manual Txfr:
					^negSplit:        Today-2M  provNum1  pat  -$100
						^^Unallocated
					^posSplit:        Today-2M  provNum1  pat   $100
						^^Attached to Procedure1
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum1=ProviderT.CreateProvider("prov1");
			Procedure procByProv1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",100,DateTime.Today.AddMonths(-1),provNum:provNum1);
			//Create an income transfer to transfer from the total payment to the proc, but make it for prior to proc/payment date
			PaySplit prePay=PaySplitT.CreatePrepayment(pat.PatNum,100,DateTime.Today.AddMonths(-1),provNum1,0);
			Payment incomeTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0,isNew:true,payDate:DateTime.Today.AddMonths(-2));
			PaySplit negSplit=PaySplitT.CreateSplit(0,pat.PatNum,incomeTransfer.PayNum,0,DateTime.Today.AddMonths(-2),0,provNum1,-100,0);
			PaySplitT.CreateSplit(0,pat.PatNum,incomeTransfer.PayNum,0,DateTime.Today.AddMonths(-2),procByProv1.ProcNum,procByProv1.ProvNum,100,0);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			/*****************************************************
			Paysplit1:  Today  provNum1  pat  -$100
				^Unearned
			Paysplit2:  Today  provNum1  pat   $100
				^Unallocated
			******************************************************/
			Assert.AreEqual(2,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==0
				&& x.SplitAmt==-100
				&& x.UnearnedType==prePay.UnearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==0
				&& x.SplitAmt==100
				&& x.UnearnedType==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary>Make sure if there are unattached adjustments that they are implicitly used by a valid previous transfer (neg split -> pos split).</summary>
		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_ExplicitlyLinkAdjustmentTransfers() {
			/*****************************************************
				Create Provider: provA
				Create Provider: provB
				Create Patient: pat
				Create proc:    Today  provA  pat   $50
				Create adjust:  Today  provB  pat  -$50
				Create payCur:  Today         pat   $0
					^Manual PaySplits:
						^^negSplit:  Today  provB  pat  -$50
							^^^Attached to unearned
						^^posSplit:  Today  provA  pat   $50
							^^^Attached to proc
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provA=ProviderT.CreateProvider("ProvA");
			long provB=ProviderT.CreateProvider("ProvB");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",50,provNum:provA);
			Adjustment adjust=AdjustmentT.MakeAdjustment(pat.PatNum,-50,proc.ProcDate,provNum:provB);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Procedure) 
				&& x.AmountOriginal==50
				&& x.AmountEnd==50));
			Assert.AreEqual(1,results.ListAccountCharges.Count(x => x.GetType()==typeof(Adjustment) 
				&& x.AmountOriginal==-50
				&& x.AmountEnd==-50));
			//Create income transfer manually (cuz test)
			PaySplit negSplit=PaySplitT.CreateSplit(proc.ClinicNum,pat.PatNum,payCur.PayNum,0,proc.ProcDate,0,provB,-50,unearnedType);
			PaySplit posSplit=PaySplitT.CreateSplit(proc.ClinicNum,pat.PatNum,payCur.PayNum,0,proc.ProcDate,proc.ProcNum,provA,50,0);
			//make an income transfer using the manager
			PaymentEdit.IncomeTransferData txfr=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			/*****************************************************
			Paysplit1:  1/4/2019  provB  pat  -$50
				^Attached to adjustment
			Paysplit2:  1/4/2019  provB  pat  $50
				^Unearned
			******************************************************/
			Assert.AreEqual(2,txfr.ListSplitsCur.Count);
			Assert.AreEqual(1,txfr.ListSplitsCur.Count(x => x.ProvNum==provB
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-50
				&& x.AdjNum==adjust.AdjNum));
			Assert.AreEqual(1,txfr.ListSplitsCur.Count(x => x.ProvNum==provB
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==50
				&& x.UnearnedType==unearnedType));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,txfr.ListSplitsCur);
			txfr=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,txfr.ListSplitsCur.Count);
		}

		///<summary>Make sure if a procedure has been paid by an incorrect provider and a negative unallocated split was made to counteract that payment that
		///the procedure shows as owing money still.</summary>
		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_UnlinkedIncomeXferShowsProcOwingMoney() {
			/*****************************************************
				Create Provider: provNumA
				Create Provider: provNumB
				Create Patient: pat
				Create proc:           Today  provNumA  pat   $50
				Manual txfr:           Today            pat   $0
					^wrongSplit          Today  provNumB  pat  -$50
						^^Attached to proc
					^incorrectXferSplit  Today  provNumB  pat   $50
			******************************************************/
			//Make a procedure for Provider A
			//Make a payment on that procedure (in full) for Provider B
			//"Correct" the mistake by creating a negative split for Provider B, but unattached
			//When making another payment it should show the procedure as owing money for Provider A (instead of owing none)
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNumA=ProviderT.CreateProvider("ProvA");
			long provNumB=ProviderT.CreateProvider("ProvB");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",50,provNum:provNumA);
			PaySplit wrongSplit=PaySplitT.CreateSplit(proc.ClinicNum,pat.PatNum,0,0,proc.ProcDate,proc.ProcNum,provNumB,-50,0);
			PaySplit incorrectXferSplit=PaySplitT.CreateSplit(proc.ClinicNum,pat.PatNum,0,0,proc.ProcDate,0,provNumB,50,0);
			//In income xfer mode it'll show that there is a proc owing money.
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.ConstructResults results=PaymentEdit.ConstructAndLinkChargeCredits(new List<long>() { pat.PatNum },pat.PatNum,new List<PaySplit>(),
				payCur,new List<AccountEntry>(),true,false);
			//Make sure that the logic creates 3 charges - One for the procedure (original, start, and end are 50) 
			//Splits should get explicitly linked correctly for the wrong provider which will equate provB's balance to 0. 
			//The procedure will still think it has been paid, there is not a way to know if the user intended that or not so no action needs to be taken.
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(Procedure) 
				&& x.AmountOriginal==50
				&& x.AmountEnd==50).Count);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit)
				&& x.AmountOriginal==50 
				&& x.AmountEnd==50).Count);
			Assert.AreEqual(1,results.ListAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit)
				&& x.AmountOriginal==50 
				&& x.AmountEnd==50).Count);
			//The user needs to either unattach the paysplit on the procedure, attach the negative split on the procedure, or delete both splits.
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			/*****************************************************
			Paysplit1:  Today  provNumB  pat   $50
				^Attached to proc
			Paysplit2:  Today  provNumB  pat  -$50
				^Unallocated
			******************************************************/
			Assert.AreEqual(2,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNumB
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==50
				&& x.ProcNum==proc.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNumB
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-50
				&& x.ProcNum==0
				&& x.UnearnedType==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_MoveOverpaidProdToUnearnedLeaveOverpaidUnallocated() {
			/*****************************************************
				Create provider:  provNum
				Create patient:  pat
				Create procPaid:         Today  provNum  pat   $100
				Create overpaymentProc:  Today  provNum  pat   $200
					^Attached to procPaid
				Create adjPaid:          Today  provNum  pat   $100
				Create overpaymentAdj:   Today  provNum  pat   $200
					^Attached to adjPaid
				Create unallocatedPayment:Today  provNum  pat   $100
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("UnearnedTxfr");
			//create procedure that gets overpaid by 100
			Procedure procPaid=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum);
			Payment overpaymentProc=PaymentT.MakePayment(pat.PatNum,200,DateTime.Today,procNum:procPaid.ProcNum,provNum:provNum);
			//create adjustment that gets overpaid by 100
			Adjustment adjPaid=AdjustmentT.MakeAdjustment(pat.PatNum,100,provNum:provNum
				,adjType:Defs.GetDefsForCategory(DefCat.AdjTypes).FirstOrDefault(x => x.ItemValue=="+").DefNum);
			Payment overpaymentAdj=PaymentT.MakePayment(pat.PatNum,200,DateTime.Today,provNum:provNum,adjNum:adjPaid.AdjNum);
			//create unattached split for $100 that should get moved to unearned. 
			Payment unallocatedPayment=PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,provNum:provNum);
			//make transfer. We're expecting a total of $200 to get moved to unearned by the end.
			//the $100 payment to unallocated will simply remain because there is no production to link it to in order to transfer it.
			PaymentEdit.IncomeTransferData transfer=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			/*****************************************************
			Paysplit1:  1/4/2019  provNum  pat  -$100
				^Attached to procPaid
			Paysplit2:  1/4/2019  provNum  pat  $100
				^Unearned
			Paysplit3:  1/4/2019  provNum  pat  -$100
				^Attached to adjPaid
			Paysplit4:  1/4/2019  provNum  pat  $100
				^Unearned
			Paysplit5:  1/4/2019  provNum  pat  -$100
				^Unallocated
			Paysplit6:  1/4/2019  provNum  pat  $100
				^Unearned
			******************************************************/
			Assert.AreEqual(6,transfer.ListSplitsCur.Count);
			Assert.AreEqual(1,transfer.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-100
				&& x.ProcNum==procPaid.ProcNum));
			Assert.AreEqual(1,transfer.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-100
				&& x.AdjNum==adjPaid.AdjNum));
			Assert.AreEqual(1,transfer.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-100
				&& x.ProcNum==0
				&& x.AdjNum==0
				&& x.PayPlanNum==0));
			Assert.AreEqual(3,transfer.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==100
				&& x.ProcNum==0
				&& x.AdjNum==0
				&& x.UnearnedType==unearnedType));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transfer.ListSplitsCur);
			transfer=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transfer.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_MoveOverpaidProdToUnearnedLeaveOverpaidUnearned() {
			/*****************************************************
				Create provider:  provNum
				Create patient:  pat
				Create procPaid:         Today  provNum  pat   $100
				Create overpaymentProc:  Today  provNum  pat   $200
					^Attached to procPaid
				Create adjPaid:          Today  provNum  pat   $100
				Create overpaymentAdj:   Today  provNum  pat   $200
					^Attached to adjPaid
				Create unearnedPayment:Today  provNum  pat   $100
					^Unearned
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("UnearnedTxfr");
			//create procedure that gets overpaid by 100
			Procedure procPaid=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum);
			Payment overpaymentProc=PaymentT.MakePayment(pat.PatNum,200,DateTime.Today,procNum:procPaid.ProcNum,provNum:provNum);
			//create adjustment that gets overpaid by 100
			Adjustment adjPaid=AdjustmentT.MakeAdjustment(pat.PatNum,100,provNum:provNum
				,adjType:Defs.GetDefsForCategory(DefCat.AdjTypes).FirstOrDefault(x => x.ItemValue=="+").DefNum);
			Payment overpaymentAdj=PaymentT.MakePayment(pat.PatNum,200,DateTime.Today,provNum:provNum,adjNum:adjPaid.AdjNum);
			//create unattached split for $100 that should get moved to unearned. 
			Payment unearnedPayment=PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,provNum:provNum,
				unearnedType:unearnedType);
			//make transfer. We're expecting a total of $200 to get moved to unearned by the end.
			//the $100 payment to unearned will simply remain because there is no production to link it to in order to transfer it.
			PaymentEdit.IncomeTransferData transfer=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			/*****************************************************
			Paysplit1:  1/4/2019  provNum  pat  -$100
				^Attached to procPaid
			Paysplit2:  1/4/2019  provNum  pat  $100
				^Unearned
			Paysplit3:  1/4/2019  provNum  pat  -$100
				^Attached to adjPaid
			Paysplit4:  1/4/2019  provNum  pat  $100
				^Unearned
			******************************************************/
			Assert.AreEqual(4,transfer.ListSplitsCur.Count);
			Assert.AreEqual(1,transfer.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-100
				&& x.ProcNum==procPaid.ProcNum));
			Assert.AreEqual(1,transfer.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-100
				&& x.AdjNum==adjPaid.AdjNum));
			Assert.AreEqual(2,transfer.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==100
				&& x.ProcNum==0
				&& x.AdjNum==0
				&& x.UnearnedType==unearnedType));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transfer.ListSplitsCur);
			transfer=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transfer.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_AllowTransfersForRigorousAccounting() {
			//To test scenario when office previously had rigorous accouting turned off, made some bad splits, and now have it turned on.
			//Transfers should still be allowed to fix the previous bad splits and get them going to a place they should be. 
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("Rig");
			//create an unattached payments
			Payment unattachedPayment=PaymentT.MakePayment(pat.PatNum,300,DateTime.Today,provNum:provNum);
			//create procedure for same amount
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",300,provNum:provNum);
			//Turn on Rigorous Accounting
			PrefT.UpdateInt(PrefName.RigorousAccounting,(int)RigorousAccounting.EnforceFully);
			//attempt to transfer the payment so it ends up going to the procedure
			PaymentEdit.IncomeTransferData transfer=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.IsFalse(transfer.HasInvalidSplits);//we just created the scenario where we expect splits to be transferred to unearned with a provider.
			Assert.AreEqual(2,transfer.ListSplitsCur.Count);//2 splits should be created that moves the money to unearned and then 2 that allocate
			Assert.AreEqual(1,transfer.ListSplitsCur.FindAll(x => x.SplitAmt.IsGreaterThan(0) && x.UnearnedType==0 && x.ProcNum==proc.ProcNum).Count);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transfer.ListSplitsCur);
			transfer=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transfer.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_NegativeProceduresDoNotGetUsedAsIncomeSource() {
			//Users have been allowed to make negative procedures. We need to make sure we don't use that money to fund something else. 
			/*****************************************************
				Create Provider:  provNum - unused...
				Create Patient:  pat
				Create procNeg:  Today-2  Prov?  pat  -$100
				Create procPos:  Today-1  Prov?  pat   $50
			******************************************************/
			//^Note that Prov? = PracticeDefaultProv due to enheriting the primary provider for the patient... Not sure if this is intended but W/E.
			long practiceDefaultProvNum=Prefs.GetLong(PrefName.PracticeDefaultProv);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("NoNegProcs");
			//create negative completed procedure
			Procedure procNeg=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",-100,DateTime.Today.AddDays(-2));
			//create regular positive procedure for less than the absolute value of the negative
			Procedure procPos=ProcedureT.CreateProcedure(pat,"D0210",ProcStat.C,"",50,DateTime.Today.AddDays(-1));
			PaymentEdit.IncomeTransferData transfer=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			/*****************************************************
			Paysplit1:  1/4/2019  Prov?  pat  -$100
				^Attached to procNeg
			Paysplit2:  1/4/2019  Prov?  pat  $100
				^Unearned
			Paysplit3:  1/4/2019  Prov?  pat  -$50
				^Unearned
			Paysplit4:  1/4/2019  Prov?  pat  $50
				^Attached to procPos
			******************************************************/
			Assert.AreEqual(4,transfer.ListSplitsCur.Count);
			Assert.AreEqual(1,transfer.ListSplitsCur.Count(x => x.ProvNum==practiceDefaultProvNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-100
				&& x.ProcNum==procNeg.ProcNum));
			Assert.AreEqual(1,transfer.ListSplitsCur.Count(x => x.ProvNum==practiceDefaultProvNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==100
				&& x.ProcNum==0));
			Assert.AreEqual(1,transfer.ListSplitsCur.Count(x => x.ProvNum==practiceDefaultProvNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-50
				&& x.ProcNum==0));
			Assert.AreEqual(1,transfer.ListSplitsCur.Count(x => x.ProvNum==practiceDefaultProvNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==50
				&& x.ProcNum==procPos.ProcNum));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transfer.ListSplitsCur);
			transfer=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transfer.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_TransferKeepsUnearnedTypeFromSplit() {
			//default unearned type was being used during transfer even though the original prepayment did not use the default.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			//create pre-payment that does not use the default, so create a new prepayment type as well to use.
			Def newUnearned=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"HiddenPrepay","x");
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,unearnedType:newUnearned.DefNum);
			//make procedure to transfer to
			long provNum=ProviderT.CreateProvider("TransferKeepUnearned");
			ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum);
			//Make transfer
			PaymentEdit.IncomeTransferData transfer=PaymentT.BalanceAndIncomeTransfer(pat.PatNum,doIncludeHidden:true);
			Assert.AreEqual(2,transfer.ListSplitsCur.Count);
			Assert.AreEqual(newUnearned.DefNum,transfer.ListSplitsCur.FirstOrDefault(x => x.SplitAmt<0).UnearnedType);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_PreBalanceAccountForUnattachedSplits() {
			//bug here was transferring more money than available to unearned because it was double counting splits when a manual transfer was entered.
			//note this scenario is specifically for when prepayments are allowed to providers, when they are not, the count will only be 6 (non zero)
			/*****************************************************
				Create Provider: provNumA
				Create Provider: provNumB
				Create Patient: pat
				Create proc:  Today  provNumA  pat   $100
				Create pay:   Today  provNumB  pat   $100
					^Attached to proc
				Manual txfr:  Today            pat   $0
					^splitA     Today  provNumB  pat  -$100
					^splitB     Today  provNumA  pat   $100
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNumA=ProviderT.CreateProvider("LSA");
			long provNumB=ProviderT.CreateProvider("LSB");
			//make a procedure for the patient
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNumA);
			//make a payment for the incorrect provider for this procedure.
			Payment pay=PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,provNum:provNumB,procNum:proc.ProcNum);
			//make a manual transfer (unattached) that attempts to fix the incorrect provider being paid. 
			Payment payShell=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today);
			PaySplit splitA=PaySplitT.CreateSplit(0,pat.PatNum,payShell.PayNum,0,DateTime.Today,0,provNumB,-100,0);
			PaySplit splitB=PaySplitT.CreateSplit(0,pat.PatNum,payShell.PayNum,0,DateTime.Today,0,provNumA,100,0);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			/*****************************************************
			Paysplit1:  Today  provNumB  pat  -$100
				^Attached to proc
			Paysplit2:  Today  provNumB  pat  $100
				^Unearned
			Paysplit3:  Today  provNumA  pat  -$100
				^Unallocated
			Paysplit4:  Today  provNumA  pat  $100
				^Attached to proc
			Paysplit5:  Today  provNumB  pat  -$100
				^Unearned
			Paysplit6:  Today  provNumB  pat  $100
				^Unallocated
			******************************************************/
			Assert.AreEqual(6,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNumB
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-100
				&& x.ProcNum==proc.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNumB
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==100
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNumA
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-100
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNumA
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==100
				&& x.ProcNum==proc.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNumB
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-100
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNumB
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==100
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.UnearnedType==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_BalanceAndUseCreatedUnearned() {
			//bug here was transferring more money that available to unearned because it was double counting splits when a manual transfer was entered.
			/*****************************************************
				Create Provider: provNum
				Create Patient: pat
				Create proc:  Today  provNum  pat  $100
				Create pay:   Today  provNum  pat  $100
					^Unallocated
			******************************************************/
			PrefT.UpdateInt(PrefName.RigorousAccounting,(int)RigorousAccounting.AutoSplitOnly);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("LSA");
			//make a procedure for the patient
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum);
			//make an unallocated payment.
			Payment pay=PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,provNum:provNum);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			/*****************************************************
			Paysplit1:  Today  provNum  pat  -$100
				^Unearned
			Paysplit2:  Today  provNum  pat   $100
				^Attached to Procedure1
			******************************************************/
			Assert.AreEqual(2,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-100
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==100
				&& x.ProcNum==proc.ProcNum));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_DoNotTransferToUnearnedWhenAdjNumIsPresent() {
			//This is seemingly more of an edge case scenario but we need to check for it since it is present is some databases.
			//There was a point in time when adjustments did not have providers. We need to make sure those adjustments do not get transferred to unearned.
			PrefT.UpdateBool(PrefName.AllowPrepayProvider,false);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNumA=ProviderT.CreateProvider("LS");
			AdjustmentT.MakeAdjustment(pat.PatNum,100);
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today);
			PaymentEdit.IncomeTransferData results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,results.ListSplitsCur.FindAll(x => x.AdjNum!=0 && x.UnearnedType!=0).Count);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,results.ListSplitsCur);
			results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,results.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_DoNotMakeTransfersForBalancedUnallocated() {
			//There are transfers that are okay to make negative unearned (unallocated transfer logic) but that is a specific case. The regular transfer 
			//logic should not leave the negative unearned on an account in this specific circumstance (procedure, positive unearned, negative unearned).
			//The unearned should balance out first, and then the income transfer will know it has no splits to create.
			/*****************************************************
				Create Provider: provNum
				Create Patient: pat
				Create proc:   Today  provNum  pat   $100
				Create pay1:   Today-1  provNum  pat   $100
					^Unallocated
				Create pay2:   Today-1  provNum  pat  -$100
					^Unallocated
			******************************************************/
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("LS");
			ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum,procDate:DateTime.Today);//procedure
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum:provNum);//unallocated positive
			PaymentT.MakePayment(pat.PatNum,-100,DateTime.Today.AddDays(-1),provNum:provNum);//unallocated negative
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			//We should get a total of 0 splits.
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_DoNotMakeTransfersForBalancedUnearned() {
			//There are transfers that are okay to make negative unearned (unallocated transfer logic) but that is a specific case. The regular transfer 
			//logic should not leave the negative unearned on an account in this specific circumstance (procedure, positive unearned, negative unearned).
			//The unearned should balance out first, and then the income transfer will know it has no splits to create.
			/*****************************************************
				Create Provider: provNum
				Create Patient: pat
				Create proc:   Today  provNum  pat   $100
				Create pay1:   Today-1  provNum  pat   $100
					^Unearned
				Create pay2:   Today-1  provNum  pat  -$100
					^Unearned
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("LS");
			ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum,procDate:DateTime.Today);//procedure
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum:provNum,unearnedType:unearnedType);
			PaymentT.MakePayment(pat.PatNum,-100,DateTime.Today.AddDays(-1),provNum:provNum,unearnedType:unearnedType);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			//We should get a total of 0 splits.
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_MakeTransfersToBalanceUnallocated() {
			//There are transfers that are okay to make negative unearned (unallocated transfer logic) but that is a specific case. The regular transfer 
			//logic should not leave the negative unearned on an account in this specific circumstance (procedure, positive unearned, negative unearned).
			//The unearned should balance out first, and then the income transfer will know it has no splits to create.
			/*****************************************************
				Create Provider: provNum
				Create Patient: pat
				Create proc:   Today  provNum  pat   $100
				Create pay1:   Today-1  provNum  pat   $100
					^Unearned
				Create pay2:   Today-1  provNum  pat  -$100
					^Unallocated
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("LS");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum,procDate:DateTime.Today);//procedure
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum:provNum,unearnedType:unearnedType);
			PaymentT.MakePayment(pat.PatNum,-100,DateTime.Today.AddDays(-1),provNum:provNum);//unallocated negative
			PaymentEdit.IncomeTransferData results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(2,results.ListSplitsCur.Count);
			Assert.AreEqual(1,results.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==0
				&& x.SplitAmt==-100
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,results.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==0
				&& x.SplitAmt==100
				&& x.UnearnedType==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,results.ListSplitsCur);
			results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,results.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_MakeTransfersToBalanceUnearned() {
			//There are transfers that are okay to make negative unearned (unallocated transfer logic) but that is a specific case. The regular transfer 
			//logic should not leave the negative unearned on an account in this specific circumstance (procedure, positive unearned, negative unearned).
			//The unearned should balance out first, and then the income transfer will know it has no splits to create.
			/*****************************************************
				Create Provider: provNum
				Create Patient: pat
				Create proc:   Today  provNum  pat   $100
				Create pay1:   Today  provNum  pat   $100
					^Unallocated
				Create pay2:   Today  provNum  pat  -$100
					^Unearned
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("LS");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum,procDate:DateTime.Today);//procedure
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum:provNum);
			PaymentT.MakePayment(pat.PatNum,-100,DateTime.Today.AddDays(-1),provNum:provNum,unearnedType:unearnedType);
			PaymentEdit.IncomeTransferData results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(2,results.ListSplitsCur.Count);
			Assert.AreEqual(1,results.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==0
				&& x.SplitAmt==100
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,results.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==0
				&& x.SplitAmt==-100
				&& x.UnearnedType==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,results.ListSplitsCur);
			results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,results.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_NoTransferNegativeUnearnedPositiveProcedure() {
			/*****************************************************
				Create Provider: provNum
				Create Patient: pat
				Create proc:   Today  provNum  pat   $100
				Create pay2:   Today  provNum  pat  -$100
					^Unearned
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum=ProviderT.CreateProvider(suffix);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",100,provNum:provNum,procDate:DateTime.Today);
			PaymentT.MakePayment(pat.PatNum,-100,DateTime.Today,provNum: provNum,unearnedType: unearnedType);
			PaymentEdit.IncomeTransferData results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,results.ListSplitsCur.Count);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,results.ListSplitsCur);
			results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,results.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_TransferNegativeUnearnedNegativeProcedure() {
			/*****************************************************
				Create Provider: provNum
				Create Patient: pat
				Create proc:   Today  provNum  pat  -$100
				Create pay2:   Today  provNum  pat  -$100
					^Unearned
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum=ProviderT.CreateProvider(suffix);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",-100,provNum:provNum,procDate:DateTime.Today);
			PaymentT.MakePayment(pat.PatNum,-100,DateTime.Today,provNum:provNum,unearnedType:unearnedType);
			PaymentEdit.IncomeTransferData results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(4,results.ListSplitsCur.Count);
			Assert.AreEqual(1,results.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==proc.ProcNum
				&& x.SplitAmt==-100
				&& x.UnearnedType==0));
			Assert.AreEqual(1,results.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==0
				&& x.SplitAmt==-100
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(2,results.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==0
				&& x.SplitAmt==100
				&& x.UnearnedType==unearnedType));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,results.ListSplitsCur);
			results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,results.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_NoTransferNegativeUnearnedPositiveAdjustment() {
			/*****************************************************
				Create Provider: provNum
				Create Patient: pat
				Create adj:    Today  provNum  pat   $100
				Create pay2:   Today  provNum  pat  -$100
					^Unearned
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum=ProviderT.CreateProvider(suffix);
			Adjustment adj=AdjustmentT.MakeAdjustment(pat.PatNum,100,provNum:provNum);
			PaymentT.MakePayment(pat.PatNum,-100,DateTime.Today,provNum: provNum,unearnedType: unearnedType);
			PaymentEdit.IncomeTransferData results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,results.ListSplitsCur.Count);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,results.ListSplitsCur);
			results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,results.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_TransferNegativeUnearnedNegativeAdjustment() {
			/*****************************************************
				Create Provider: provNum
				Create Patient: pat
				Create adj:    Today  provNum  pat  -$100
				Create pay2:   Today  provNum  pat  -$100
					^Unearned
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum=ProviderT.CreateProvider(suffix);
			Adjustment adj=AdjustmentT.MakeAdjustment(pat.PatNum,-100,provNum:provNum);
			PaymentT.MakePayment(pat.PatNum,-100,DateTime.Today,provNum: provNum,unearnedType: unearnedType);
			PaymentEdit.IncomeTransferData results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(2,results.ListSplitsCur.Count);
			Assert.AreEqual(1,results.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.ProcNum==0
				&& x.SplitAmt==100
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,results.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.AdjNum==adj.AdjNum
				&& x.SplitAmt==-100
				&& x.UnearnedType==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,results.ListSplitsCur);
			results=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,results.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_UnallocatedToProduction() {
			//Bug was that the unearned created from the income transfer manager would not be correctly evaluated and thus would not 
			//think that the income had any value. 
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("LS");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",70,provNum:provNum,procDate:DateTime.Today.AddDays(-2));
			PaymentT.MakePayment(pat.PatNum,70,DateTime.Today.AddDays(-1),0,provNum,0,1,0,0);//unallocated payment
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			//after both logics have run, the unearned that was created should have been applied to the procedure.
			Assert.AreEqual(1,transferResults.ListSplitsCur.FindAll(x => x.UnearnedType==0 && x.SplitAmt.IsLessThan(0)).Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.FindAll(x => x.ProcNum==proc.ProcNum && x.SplitAmt.IsEqual(70)).Count);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_NoNegativeUnearnedFromUnattachedAdjustment() {
			//Unattached adjustments could sometimes make the account go into negative unearned
			/*****************************************************
				Create Provider:  provNum
				Create Patient:  pat
				Create Procedure1:  Today-5  provNum  pat   $200
				Create Adjustment1: Today-4  provNum  pat  -$100
				Create Payment1:    Today-3  provNum  pat   $100
				manualTransfer:     Today-2           pat   $0
					^PaySplit1:       Today-2  provNum  pat   $200
						^^Attached to Procedure1
					^PaySplit1:       Today-2  provNum  pat  -$200
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("LS");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",200,provNum:provNum,procDate:DateTime.Today.AddDays(-5));
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(pat.PatNum,-100,DateTime.Today.AddDays(-4),provNum:provNum);
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today.AddDays(-3),provNum:provNum,payType:1);
			//manual transfer
			Payment manualTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0,DateTime.Today.AddDays(-2));
			PaySplitT.CreateSplit(0,pat.PatNum,manualTransfer.PayNum,0,DateTime.Today.AddDays(-2),proc.ProcNum,provNum,200,0);
			PaySplitT.CreateSplit(0,pat.PatNum,manualTransfer.PayNum,0,DateTime.Today.AddDays(-2),0,provNum,-200,0);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			//There should be a transfer from unallocated to the negative adjustment.
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			/*****************************************************
			Paysplit1:  Today  provNum  pat  -$100
				^Attached to adjustment1
			Paysplit2:  Today  provNum  pat   $100
				^Unearned
			Paysplit3:  Today  provNum  pat  -$100
				^Unearned
			Paysplit4:  Today  provNum  pat   $100
				^Unallocated
			******************************************************/
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-100
				&& x.AdjNum==adjustment1.AdjNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==100
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==-100
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum
				&& x.PatNum==pat.PatNum
				&& x.SplitAmt==100
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.UnearnedType==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_OverpaidProcedureDoesNotResultInMulitpleTransfers() {
			//If this test fails in the future, it may be due to the order of splits while explicitly linking.
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Family fam=Patients.GetFamily(pat.PatNum);
			long provNum=ProviderT.CreateProvider("LS");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",65,DateTime.Today,provNum:provNum);
			PaymentT.MakePayment(pat.PatNum,100,DateTime.Today,provNum:provNum,procNum:proc.ProcNum,payType:1);
			Payment transferPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			PaymentEdit.IncomeTransferData transfer=PaymentT.BalanceAndIncomeTransfer(pat.PatNum,fam,transferPayment);
			Assert.AreEqual(2,transfer.ListSplitsCur.Count);
			//should be a transfer to move the overpayment to unearned.
			Assert.AreEqual(1,transfer.ListSplitsCur.FindAll(x => x.ProcNum!=0 && x.SplitAmt==-35).Count);
			Assert.AreEqual(1,transfer.ListSplitsCur.FindAll(x => x.UnearnedType!=0 && x.SplitAmt==35).Count);
			//insert and attempt to make another transfer.
			PaySplits.InsertMany(0,transfer.ListSplitsCur);
			Payment secondTransfer=PaymentT.MakePaymentNoSplits(pat.PatNum,0);
			transfer.ListSplitsCur.Clear();
			transfer=PaymentT.BalanceAndIncomeTransfer(pat.PatNum,fam,secondTransfer);
			Assert.AreEqual(0,transfer.ListSplitsCur.Count);
		}

		///<summary>When a claim has an as total payment with no procedures attached, we transfer the insurance payment to a patient payment. Previously
		///we allowed the transfer to create an unearned payment with a provider when AllowPrepayProvider was false. This test makes sure that after
		///this, we can still transfer the payment correctly.</summary>
		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_AfterClaimPayAsTotalTransferWithNoProcedures() {
			/*****************************************************
				Create Patient: pat
				Create Provider: provNum
				Create Procedure: Today-2  provNum  pat   $200
				Create ClaimProc1:  Today  provNum  pat   $200
					^As Total
				Create ClaimProc2:  Today  provNum  pat  -$200
					^As Total
				Create Payment1:    Today  provNum  pat   $200
					^Attached to Unearned
			******************************************************/
			PrefT.UpdateBool(PrefName.AllowPrepayProvider,false);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			Def unearnedType=DefT.CreateDefinition(DefCat.PaySplitUnearnedType,"unearned"+suffix);
			long provNum=ProviderT.CreateProvider("LS");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",200,provNum:provNum,procDate:DateTime.Today.AddDays(-2));
			InsuranceInfo ins=InsuranceT.AddInsurance(pat,"BestCarrier");
			Claim claim=new Claim();
			claim.PatNum=pat.PatNum;
			claim.ClaimStatus="R";
			claim.ProvBill=provNum;
			claim.ProvTreat=provNum;
			Claims.Insert(claim);
			//create new as total claim proc payment.
			ClaimProcT.AddInsPaidAsTotal(pat.PatNum,ins.PriInsPlan.PlanNum,provNum,200,ins.PriInsSub.InsSubNum,0,0,claim.ClaimNum);
			//Act as if we created a transfer (back when we allowed the unearned payment to have a provider)
			Payment transfer1=PaymentT.MakePayment(pat.PatNum,200,provNum:provNum,unearnedType:unearnedType.DefNum);
			ClaimProcT.CreateClaimProc(pat.PatNum,0,ins.PriInsPlan.PlanNum,ins.PriInsSub.InsSubNum,cps:ClaimProcStatus.Supplemental,insPayAmt:-200,
				isTransfer:true,claimNum:claim.ClaimNum);
			//Now create another transfer
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			/*****************************************************
			Paysplit1:  Today  provNum  pat  -$200
				^Attached to Procedure1
			Paysplit2:  Today  provNum  pat   $200
				^Unearned
			******************************************************/
			//Should have a negative split that matched the provider and unearned type along with a positive split applied to the procedure
			Assert.IsFalse(transferResults.HasInvalidSplits);
			Assert.AreEqual(2,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.FindAll(x => x.UnearnedType==unearnedType.DefNum && x.ProvNum==provNum 
				&& x.SplitAmt.IsEqual(-200)).Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.FindAll(x => x.ProcNum==proc.ProcNum && x.ProvNum==provNum
				&& x.SplitAmt.IsEqual(200)).Count);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_RobPeterToPayPaul() {
			/*****************************************************
				Create Provider: Prov1
				Create Provider: Prov2
				Create Patient: Pat1
				Create Procedure1:  1/1/2019  Prov1  Pat1  $500
				Create Adjustment1: 1/1/2019  Prov2  Pat1  $50
					^Attached to Procedure1
				Create Payment1:  1/2/2019  Prov2  Pat1  $500
					^Attached to Procedure1
				Create Adjustment2:  1/3/2019  Prov2  Pat1  -$50
					^Attached to Procedure1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",500,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(pat1.PatNum,50,adjDate:new DateTime(2019,1,1),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum2);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,500,payDate:new DateTime(2019,1,2),provNum:provNum2,procNum:procedure1.ProcNum);
			Adjustment adjustment2=AdjustmentT.MakeAdjustment(pat1.PatNum,-50,adjDate:new DateTime(2019,1,3),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum2);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  1/4/2019  Prov2  Pat1  -$500
				^Attached to Procedure1
			Paysplit2:  1/4/2019  Prov2  Pat1  $500
				^Unearned
			Paysplit3:  1/4/2019  Prov2  Pat1  -$500
				^Unearned
			Paysplit4:  1/4/2019  Prov1  Pat1  $500
				^Attached to Procedure1
			******************************************************/
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-500
				&& x.ProcNum==procedure1.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==500
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-500
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==500
				&& x.ProcNum==procedure1.ProcNum));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_PaulPaidForPaul() {
			/*****************************************************
				Create Provider: Prov1
				Create Provider: Prov2
				Create Patient: Pat1
				Create Procedure1:  1/1/2019  Prov1  Pat1  $500
				Create Adjustment1: 1/1/2019  Prov2  Pat1  $50
					^Attached to Procedure1
				Create Payment1:  1/2/2019  Prov2  Pat1  $500
					^Attached to Procedure1
				Create Adjustment2:  1/3/2019  Prov2  Pat1  -$50
					^Attached to Procedure1
				Create Payment2:  Today  Prov2  Pat1  -$500
					^Attached to Procedure1
			After Explicit Linking:
				Create Procedure1:  1/1/2019  Prov1  Pat1  $500
				Create Adjustment1: 1/1/2019  Prov2  Pat1  $50
					^Attached to Procedure1
				Create Payment1:  1/2/2019  Prov2  Pat1  $0
					^Attached to Procedure1
				Create Adjustment2:  1/3/2019  Prov2  Pat1  -$50
					^Attached to Procedure1
				Create Payment2:  Today  Prov2  Pat1  $0
					^Attached to Procedure1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",500,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(pat1.PatNum,50,adjDate:new DateTime(2019,1,1),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum2);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,500,payDate:new DateTime(2019,1,2),provNum:provNum2,procNum:procedure1.ProcNum);
			Adjustment adjustment2=AdjustmentT.MakeAdjustment(pat1.PatNum,-50,adjDate:new DateTime(2019,1,3),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum2);
			Payment payment2=PaymentT.MakePayment(pat1.PatNum,-500,provNum:provNum2,procNum:procedure1.ProcNum);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			No PaySplits.
			******************************************************/
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_ManuallyRobPeterToPayPaul() {
			/*****************************************************
				Create Provider: Prov1
				Create Provider: Prov2
				Create Patient: Pat1
				Create Procedure1:  1/1/2019  Prov1  Pat1  $500
				Create Adjustment1: 1/1/2019  Prov2  Pat1  $50
					^Attached to Procedure1
				Create Payment1:  1/2/2019  Prov2  Pat1  $500
					^Attached to Procedure1
				Create Adjustment2:  1/3/2019  Prov2  Pat1  -$50
					^Attached to Procedure1
				Create Payment2:  Today  Prov2  Pat1  -$500
					^Attached to Procedure1
				Create Payment3:  Today  Prov2  Pat1  $500
					^Unearned
			After Explicit Linking:
				Create Procedure1:  1/1/2019  Prov1  Pat1  $500
				Create Adjustment1: 1/1/2019  Prov2  Pat1  $50
					^Attached to Procedure1
				Create Payment1:  1/2/2019  Prov2  Pat1  $0
					^Attached to Procedure1
				Create Adjustment2:  1/3/2019  Prov2  Pat1  -$50
					^Attached to Procedure1
				Create Payment2:  Today  Prov2  Pat1  $0
					^Attached to Procedure1
				Create Payment3:  Today  Prov2  Pat1  -$500
					^Unearned
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",500,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(pat1.PatNum,50,adjDate:new DateTime(2019,1,1),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum2);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,500,payDate:new DateTime(2019,1,2),provNum:provNum2,procNum:procedure1.ProcNum);
			Adjustment adjustment2=AdjustmentT.MakeAdjustment(pat1.PatNum,-50,adjDate:new DateTime(2019,1,3),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum2);
			Payment payment2=PaymentT.MakePayment(pat1.PatNum,-500,provNum:provNum2,procNum:procedure1.ProcNum);
			Payment payment3=PaymentT.MakePayment(pat1.PatNum,500,provNum:provNum2,unearnedType:unearnedType);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit3:  1/4/2019  Prov2  Pat1  -$500
				^Unearned
			Paysplit4:  1/4/2019  Prov1  Pat1  $500
				^Attached to Procedure1
			******************************************************/
			Assert.AreEqual(2,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-500
				&& x.ProcNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==500
				&& x.ProcNum==procedure1.ProcNum));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_ManualAdjustmentCorrection() {
			/*****************************************************
				Create Provider: Prov1
				Create Provider: Prov2
				Create Patient: Pat1
				Create Procedure1:  1/1/2019  Prov1  Pat1  $500
				Create Adjustment1:  1/1/2019  Prov2  Pat1  $50
					^Attached to Procedure1
				Create Payment1:  1/2/2019  Prov1  Pat1  $500
					^Attached to Procedure1
				Create Adjustment2:  1/3/2019  Prov2  Pat1  -$50
					^Attached to Procedure1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",500,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(pat1.PatNum,50,adjDate:new DateTime(2019,1,1),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum2);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,500,payDate:new DateTime(2019,1,2),provNum:provNum1,procNum:procedure1.ProcNum);
			Adjustment adjustment2=AdjustmentT.MakeAdjustment(pat1.PatNum,-50,adjDate:new DateTime(2019,1,3),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum2);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Nothing to Transfer
			******************************************************/
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_MoveOverpaidAdjustmentsToUnearned() {
			/*****************************************************
				Create Provider: Prov1
				Create Provider: Prov2
				Create Patient: Pat1
				Create Procedure1:  1/1/2019  Prov1  Pat1  $500
				Create Adjustment1:  1/1/2019  Prov1  Pat1  $-50
					^Attached to Procedure1
				Create Payment1:  1/2/2019  Prov1  Pat1  $500
					^Attached to Procedure1
				Create Adjustment2:  1/3/2019  Prov2  Pat1  -$50
					^Attached to Procedure1
				After Explicit Linking:
					Procedure 1:  1/1/2019  Prov1  Pat1  $-50
					Adjustment1:  1/1/2019  Prov1  Pat1  $0
						^Attached to Procedure1
					Payment1:  1/2/2019  Prov1  Pat1  $0
						^Attached to Procedure1
					Adjustment2:  1/3/2019  Prov2  Pat1  -$50
						^Attached to Procedure1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",500,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(pat1.PatNum,-50,adjDate:new DateTime(2019,1,1),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum1);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,500,payDate:new DateTime(2019,1,2),provNum:provNum1,procNum:procedure1.ProcNum);
			Adjustment adjustment2=AdjustmentT.MakeAdjustment(pat1.PatNum,-50,adjDate:new DateTime(2019,1,3),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum2);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  1/4/2019  Prov1  Pat1  -$50
				^Attached to Procedure1
			Paysplit2:  1/4/2019  Prov1  Pat1  $50
				^Unearned
			Paysplit3:  1/4/2019  Prov2  Pat1  -$50
				^Attached to Procedure1
			Paysplit4:  1/4/2019  Prov2  Pat1  $50
				^Unearned
			******************************************************/
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-50
				&& x.ProcNum==procedure1.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==50
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-50
				&& x.ProcNum==procedure1.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==50
				&& x.ProcNum==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_NegativeAdjustmentToUnearned() {
			/*****************************************************
				Create Provider: Prov1
				Create Patient: Pat1
				Create Adjustment1:  Today  Prov1  Pat1  $-50
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(pat1.PatNum,-50,adjDate:DateTime.Today,provNum:provNum1);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  1/4/2019  Prov1  Pat1  -$50
				^Attached to adjustment1
			Paysplit2:  1/4/2019  Prov1  Pat1  $50
				^Unearned
			******************************************************/
			Assert.AreEqual(2,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-50
				&& x.AdjNum==adjustment1.AdjNum
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==50
				&& x.ProcNum==0
				&& x.UnearnedType==Prefs.GetLong(PrefName.PrepaymentUnearnedType)));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_OverpaidAdjustmentPrepayProviderOff() {
			/*****************************************************
				Create Provider: Prov1
				Create Provider: Prov2
				Create Patient: Pat1
				Create Procedure1:  1/1/2019  Prov1  Pat1  $500
				Create Adjustment1:  1/1/2019  Prov1  Pat1  $-550
					^Attached to Procedure1
				Create Payment1:  1/2/2019  Prov1  Pat1  $500
					^Attached to Procedure1
				Create Adjustment2:  1/3/2019  Prov2  Pat1  $50
					^Attached to Procedure1
			******************************************************/
			PrefT.UpdateBool(PrefName.AllowPrepayProvider,false);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",500,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(pat1.PatNum,-550,adjDate:new DateTime(2019,1,1),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum1);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,500,payDate:new DateTime(2019,1,2),provNum:provNum1,procNum:procedure1.ProcNum);
			Adjustment adjustment2=AdjustmentT.MakeAdjustment(pat1.PatNum,50,adjDate:new DateTime(2019,1,3),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum2);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  1/4/2019  Prov1  Pat1  -$550
				^Attached to Procedure1
			Paysplit2:  1/4/2019  Prov0  Pat1  $550
				^Unearned
			Paysplit3:  1/4/2019  Prov0  Pat1  -$50
				^Unearned
			Paysplit4:  1/4/2019  Prov2  Pat1  $50
				^Attached to Adjustment2
			******************************************************/
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-550
				&& x.ProcNum==procedure1.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==0
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==550
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==0
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-50
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==50
				&& x.AdjNum==adjustment2.AdjNum
				&& x.ProcNum==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_OverpaidAdjustmentPrepayProvicerOn() {
			/*****************************************************
				Create Provider: Prov1
				Create Provider: Prov2
				Create Patient: Pat1
				Create Procedure1:  1/1/2019  Prov1  Pat1  $500
				Create Adjustment1:  1/1/2019  Prov1  Pat1  $-550
					^Attached to Procedure1
				Create Payment1:  1/2/2019  Prov1  Pat1  $500
					^Attached to Procedure1
				Create Adjustment2:  1/3/2019  Prov2  Pat1  $50
					^Attached to Procedure1
				After Explicit Linking:
					Procedure 1:  1/1/2019  Prov1  Pat1  $-550
					Adjustment1:  1/1/2019  Prov1  Pat1  $0
						^Attached to Procedure1
					Payment1:  1/2/2019  Prov1  Pat1  $0
						^Attached to Procedure1
					Adjustment2:  1/3/2019  Prov2  Pat1  $50
						^Attached to Procedure1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",500,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(pat1.PatNum,-550,adjDate:new DateTime(2019,1,1),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum1);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,500,payDate:new DateTime(2019,1,2),provNum:provNum1,procNum:procedure1.ProcNum);
			Adjustment adjustment2=AdjustmentT.MakeAdjustment(pat1.PatNum,50,adjDate:new DateTime(2019,1,3),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum2);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
				Paysplit1:  1/4/2019  Prov1  Pat1  -$550
					^Attached to Procedure1
				Paysplit2:  1/4/2019  Prov1  Pat1  $550
					^Unearned
				Paysplit3:  1/4/2019  Prov1  Pat1  -$50
					^Unearned
				Paysplit4:  1/4/2019  Prov2  Pat1  $50
					^Attached to Adjustment2
				******************************************************/
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-550
				&& x.ProcNum==procedure1.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==550
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-50
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==50
				&& x.ProcNum==0
				&& x.AdjNum==adjustment2.AdjNum));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_OverpaidWrongProvider() {
			/*****************************************************
				Create Provider: Prov1
				Create Provider: Prov2
				Create Patient: Pat1
				Create Procedure1:  1/1/2019  Prov1  Pat1  $500
				Create Adjustment1:  1/1/2019  Prov1  Pat1  $-250
					^Attached to Procedure1
				Create Payment1:  1/2/2019  Prov2  Pat1  $300
					^Attached to Procedure1
				Create Adjustment2:  1/3/2019  Prov2  Pat1  $50
					^Attached to Procedure1
				After Explicit Linking:
					Procedure 1:  1/1/2019  Prov1  Pat1  $250
					Adjustment1:  1/1/2019  Prov1  Pat1  $0
						^Attached to Procedure1
					Payment1:  1/2/2019  Prov2  Pat1  $0
						^Attached to Procedure1
					Adjustment2:  1/3/2019  Prov2  Pat1  -$250
						^Attached to Procedure1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",500,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(pat1.PatNum,-250,adjDate:new DateTime(2019,1,1),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum1);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,300,payDate:new DateTime(2019,1,2),provNum:provNum2,procNum:procedure1.ProcNum);
			Adjustment adjustment2=AdjustmentT.MakeAdjustment(pat1.PatNum,50,adjDate:new DateTime(2019,1,3),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum2);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  1/4/2019  Prov2  Pat1  -$250
				^Attached to Procedure1
			Paysplit2:  1/4/2019  Prov2  Pat1  $250
				^Unearned
			Paysplit3:  1/4/2019  Prov2  Pat1  -$250
				^Unearned
			Paysplit4:  1/4/2019  Prov1  Pat1  $250
				^Attached to Procedure1
			******************************************************/
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-250
				&& x.ProcNum==procedure1.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==250
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-250
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==250
				&& x.ProcNum==procedure1.ProcNum));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_MultipleOverpaidProcedures() {
			/*****************************************************
				Create Provider: Prov1
				Create Provider: Prov2
				Create Patient: Pat1
				Create Procedure1:  1/1/2019  Prov1  Pat1  $500
				Create Procedure2:  1/1/2019  Prov2  Pat1  $500
				Create Adjustment1:  1/1/2019  Prov1  Pat1  $-250
					^Attached to Procedure1
				Create Payment1:  1/2/2019  Prov1  Pat1  $300
					^Attached to Procedure1
				Create Adjustment2:  1/3/2019  Prov2  Pat1  $50
					^Attached to Procedure2
				Create Payment2:  1/4/2019  Prov2  Pat1  $500
					^Attached to Procedure2
				After Explicit Linking:
					Procedure 1:  1/1/2019  Prov1  Pat1  $-50
					Procedure 2:  1/1/2019  Prov2  Pat1  $50
					Adjustment1:  1/1/2019  Prov1  Pat1  $0
						^Attached to Procedure1
					Payment1:  1/2/2019  Prov1  Pat1  $0
						^Attached to Procedure1
					Adjustment2:  1/1/2019  Prov2  Pat1  $0
						^Attached to Procedure2
					Payment2:  1/2/2019  Prov2  Pat1  $0
						^Attached to Procedure2
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",500,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Procedure procedure2=ProcedureT.CreateProcedure(pat1,"I0002",ProcStat.C,"",500,procDate:new DateTime(2019,1,1),provNum:provNum2);
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(pat1.PatNum,-250,adjDate:new DateTime(2019,1,1),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum1);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,300,payDate:new DateTime(2019,1,2),provNum:provNum1,procNum:procedure1.ProcNum);
			Adjustment adjustment2=AdjustmentT.MakeAdjustment(pat1.PatNum,50,adjDate:new DateTime(2019,1,3),procDate:procedure2.ProcDate
				,procNum:procedure2.ProcNum,provNum:provNum2);
			Payment payment2=PaymentT.MakePayment(pat1.PatNum,500,payDate:new DateTime(2019,1,4),provNum:provNum2,procNum:procedure2.ProcNum);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  1/5/2019  Prov1  Pat1  -$50
				^Attached to Procedure1
			Paysplit2:  1/5/2019  Prov1  Pat1  $50
				^Unearned
			Paysplit3:  1/5/2019  Prov1  Pat1  -$50
				^Unearned
			Paysplit4:  1/5/2019  Prov2  Pat1  $50
				^Attached to Procedure2
			******************************************************/
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-50
				&& x.ProcNum==procedure1.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==50
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-50
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==50
				&& x.ProcNum==procedure2.ProcNum));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_CareForOthersAfterYourself() {
			/*****************************************************
				Create Provider: Prov1
				Create Provider: Prov2
				Create Patient: Pat1
				Create Procedure1:  1/1/2019  Prov1  Pat1  $500
				Create Procedure2:  1/1/2019  Prov2  Pat1  $500
				Create Payment1:  1/2/2019  Prov1  Pat1  $300
					^Attached to Procedure1
				Create Adjustment1:  1/3/2019  Prov2  Pat1  $50
					^Attached to Procedure2
				Create Payment2:  1/4/2019  Prov1  Pat1  $500
					^Attached to Procedure2
				After Explicit Linking:
					Procedure 1:  1/1/2019  Prov1  Pat1  $200
					Procedure 2:  1/1/2019  Prov2  Pat1  $550
					Payment1:  1/2/2019  Prov1  Pat1  $0
						^Attached to Procedure1
					Adjustment2:  1/1/2019  Prov2  Pat1  $0
						^Attached to Procedure2
					Payment2:  1/2/2019  Prov1  Pat1  $-500
						^Attached to Procedure2
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",500,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Procedure procedure2=ProcedureT.CreateProcedure(pat1,"I0002",ProcStat.C,"",500,procDate:new DateTime(2019,1,1),provNum:provNum2);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,300,payDate:new DateTime(2019,1,2),provNum:provNum1,procNum:procedure1.ProcNum);
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(pat1.PatNum,50,adjDate:new DateTime(2019,1,3),procDate:procedure2.ProcDate
				,procNum:procedure2.ProcNum,provNum:provNum2);
			Payment payment2=PaymentT.MakePayment(pat1.PatNum,500,payDate:new DateTime(2019,1,4),provNum:provNum1,procNum:procedure2.ProcNum);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  1/5/2019  Prov1  Pat1  -$500
				^Attached to Procedure2
			Paysplit2:  1/5/2019  Prov1  Pat1  $500
				^Unearned
			Paysplit3:  1/5/2019  Prov1  Pat1  -$200
				^Unearned
			Paysplit4:  1/5/2019  Prov1  Pat1  $200
				^Attached to Procedure1
			Paysplit5:  1/5/2019  Prov1  Pat1  -$300
				^Unearned
			Paysplit6:  1/5/2019  Prov2  Pat1  $300
				^Attached to Procedure2
			******************************************************/
			Assert.AreEqual(6,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-500
				&& x.ProcNum==procedure2.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==500
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-200
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==200
				&& x.ProcNum==procedure1.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-300
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==300
				&& x.ProcNum==procedure2.ProcNum));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_MakeItRain() {
			/*****************************************************
				Create Provider: Prov1
				Create Provider: Prov2
				Create Provider: Prov3
				Create Provider: Prov4
				Create Patient: Pat1
				Create Procedure1:  1/1/2019  Prov1  Pat1  $-50
				Create Procedure2:  1/1/2019  Prov2  Pat1  $100
				Create Procedure3:  1/1/2019  Prov3  Pat1  $50
				Create Procedure4:  1/1/2019  Prov4  Pat1  $600
				Create Payment1:  1/2/2019  Prov0  Pat1  $500
					^Unearned
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			long provNum3=ProviderT.CreateProvider($"{suffix}-3");
			long provNum4=ProviderT.CreateProvider($"{suffix}-4");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",-50,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Procedure procedure2=ProcedureT.CreateProcedure(pat1,"I0002",ProcStat.C,"",100,procDate:new DateTime(2019,1,1),provNum:provNum2);
			Procedure procedure3=ProcedureT.CreateProcedure(pat1,"I0003",ProcStat.C,"",50,procDate:new DateTime(2019,1,1),provNum:provNum3);
			Procedure procedure4=ProcedureT.CreateProcedure(pat1,"I0004",ProcStat.C,"",600,procDate:new DateTime(2019,1,1),provNum:provNum4);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,500,payDate:new DateTime(2019,1,2),
				unearnedType:Prefs.GetLong(PrefName.PrepaymentUnearnedType));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  1/5/2019  Prov1  Pat1  -$50
				^Attached to Procedure1
			Paysplit2:  1/5/2019  Prov1  Pat1  $50
				^Unearned
			Paysplit3:  1/5/2019  Prov0  Pat1  -$100
				^Unearned
			Paysplit4:  1/5/2019  Prov2  Pat1  $100
				^Attached to Procedure2
			Paysplit5:  1/5/2019  Prov0  Pat1  -$50
				^Unearned
			Paysplit6:  1/5/2019  Prov3  Pat1  $50
				^Attached to Procedure3
			Paysplit7:  1/5/2019  Prov0  Pat1  -$350
				^Unearned
			Paysplit8:  1/5/2019  Prov4  Pat1  $350
				^Attached to Procedure4
			Paysplit9:  1/5/2019  Prov1  Pat1  -$50
				^Unearned
			Paysplit10:  1/5/2019  Prov4  Pat1  $50
				^Attached to Procedure4
			******************************************************/
			Assert.AreEqual(10,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-50
				&& x.ProcNum==procedure1.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==50
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==0
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-100
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==100
				&& x.ProcNum==procedure2.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==0
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-50
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum3
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==50
				&& x.ProcNum==procedure3.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==0
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-350
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum4
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==350
				&& x.ProcNum==procedure4.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-50
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum4
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==50
				&& x.ProcNum==procedure4.ProcNum));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_MakeItRainLess() {
			/*****************************************************
				Create Provider: Prov1
				Create Provider: Prov2
				Create Provider: Prov3
				Create Provider: Prov4
				Create Patient: Pat1
				Create Procedure1:  1/1/2019  Prov1  Pat1  $50
				Create Procedure2:  1/1/2019  Prov2  Pat1  $-300
				Create Procedure3:  1/1/2019  Prov1  Pat1  $100
				Create Procedure4:  1/1/2019  Prov2  Pat1  $200
				Create Payment1:    1/2/2019  Prov0  Pat1  $500
					^Unearned
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",50,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Procedure procedure2=ProcedureT.CreateProcedure(pat1,"I0002",ProcStat.C,"",-300,procDate:new DateTime(2019,1,1),provNum:provNum2);
			Procedure procedure3=ProcedureT.CreateProcedure(pat1,"I0003",ProcStat.C,"",100,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Procedure procedure4=ProcedureT.CreateProcedure(pat1,"I0004",ProcStat.C,"",200,procDate:new DateTime(2019,1,1),provNum:provNum2);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,500,payDate:new DateTime(2019,1,2),
				unearnedType:Prefs.GetLong(PrefName.PrepaymentUnearnedType));
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  1/5/2019  Prov2  Pat1  -$300
				^Attached to Procedure2
			Paysplit2:  1/5/2019  Prov2  Pat1  $300
				^Unearned
			Paysplit3:  1/5/2019  Prov0  Pat1  -$50
				^Unearned
			Paysplit4:  1/5/2019  Prov1  Pat1  $50
				^Attached to Procedure1
			Paysplit5:  1/5/2019  Prov0  Pat1  -$100
				^Unearned
			Paysplit6:  1/5/2019  Prov1  Pat1  $100
				^Attached to Procedure3
			Paysplit7:  1/5/2019  Prov2  Pat1  -$200
				^Unearned
			Paysplit8:  1/5/2019  Prov2  Pat1  $200
				^Attached to Procedure4
			******************************************************/
			Assert.AreEqual(8,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-300
				&& x.ProcNum==procedure2.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==300
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==0
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-50
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==50
				&& x.ProcNum==procedure1.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==0
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-100
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==100
				&& x.ProcNum==procedure3.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-200
				&& x.ProcNum==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==200
				&& x.ProcNum==procedure4.ProcNum));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_MoveUnearnedToUnallocated() {
			/*****************************************************
				Create Provider: Prov1
				Create Provider: Prov2
				Create Patient: Pat1
				Create Payment1:    1/1/2019  Prov1  Pat1  -$500
					^Unallocated
				Create Procedure1:  1/1/2019  Prov2  Pat1  -$300
				Create Procedure2:  1/1/2019  Prov1  Pat1   $400
				Create Payment2:    1/1/2019  Prov2  Pat1   $600
					^Attached to Procedure2
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,-500,payDate:new DateTime(2019,1,1),provNum:provNum1);
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",-300,procDate:new DateTime(2019,1,1),provNum:provNum2);
			Procedure procedure2=ProcedureT.CreateProcedure(pat1,"I0002",ProcStat.C,"",400,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Payment payment2=PaymentT.MakePayment(pat1.PatNum,600,payDate:new DateTime(2019,1,1),provNum:provNum2,procNum:procedure2.ProcNum);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  1/5/2019  Prov2  Pat1  -$300
				^Attached to Procedure1
			Paysplit2:  1/5/2019  Prov2  Pat1   $300
				^Unearned
			Paysplit3:  1/5/2019  Prov2  Pat1  -$600
				^Attached to Procedure2
			Paysplit4:  1/5/2019  Prov2  Pat1   $600
				^Unearned
			Paysplit5:  1/5/2019  Prov2  Pat1  -$300
				^Unearned
			Paysplit6:  1/5/2019  Prov1  Pat1   $300
				^Attached to Procedure2
			Paysplit7:  1/5/2019  Prov2  Pat1  -$100
				^Unearned
			Paysplit8:  1/5/2019  Prov1  Pat1   $100
				^Attached to Procedure2
			Paysplit8:  1/5/2019  Prov2  Pat1  -$500
				^Unearned
			Paysplit10:  1/5/2019  Prov1  Pat1  $500
				^Unallocated
			******************************************************/
			Assert.AreEqual(10,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-300
				&& x.ProcNum==procedure1.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==300
				&& x.ProcNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-600
				&& x.ProcNum==procedure2.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==600
				&& x.ProcNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-300
				&& x.ProcNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==300
				&& x.ProcNum==procedure2.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-100
				&& x.ProcNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==100
				&& x.ProcNum==procedure2.ProcNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum2
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-500
				&& x.ProcNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==500
				&& x.ProcNum==0
				&& x.UnearnedType==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary></summary>
		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_OldClinicAdjustmentClinicNowOff() {
			/*****************************************************
				Create clinic: clinic
				Create provider: provNum
				Create patient: pat
				Create procedure:   Today  provNum  pat  clinic   $45
				Create adjustment:  Today  provNum  pat  clinic   $15
				Create paySplit1:   Today  provNum  pat    HQ     $15
					^Attached to adjustment
				Create paySplit2:   Today  provNum  pat    HQ    -$15
					^Procedure
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			PrefT.UpdateBool(PrefName.EasyNoClinics,false);//Not-no clinics
			Clinic clinic=ClinicT.CreateClinic(suffix);
			Patient pat=PatientT.CreatePatient(suffix,clinicNum:clinic.ClinicNum);
			long provNum=ProviderT.CreateProvider(suffix);
			Procedure procedure=ProcedureT.CreateProcedure(pat,"I0817",ProcStat.C,"",45,provNum:provNum);//ClinicNum gets set to pat.ClinicNum
			Def defAdjType=DefT.CreateDefinition(DefCat.AdjTypes,suffix,itemValue:"+",isHidden:true);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,15,provNum:provNum,adjType:defAdjType.DefNum,clinicNum:clinic.ClinicNum);
			Payment payment1=PaymentT.MakePaymentNoSplits(pat.PatNum,0,clinicNum:clinic.ClinicNum);
			PaySplit paySplit1=PaySplitT.CreateSplit(0,pat.PatNum,payment1.PayNum,0,DateTime.Today,0,provNum,15,0,adjustment.AdjNum);
			PaySplit paySplit2=PaySplitT.CreateSplit(0,pat.PatNum,payment1.PayNum,0,DateTime.Today,procedure.ProcNum,provNum,-15,0,0);
			PrefT.UpdateBool(PrefName.EasyNoClinics,true);//No clinics
			//The user needs to either unattach the paysplit on the procedure, attach the negative split on the procedure, or delete both splits.
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			/*****************************************************
			Paysplit1:  Today  provNum  pat  clinic   $15
				^Attached to procedure
			Paysplit2:  Today  provNum  pat    HQ    -$15
				^Attached to adjustment
			Paysplit3:  Today  provNum  pat    HQ     $15
				^Attached to procedure
			Paysplit4:  Today  provNum  pat    HQ    -$15
				^Unearned
			******************************************************/
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			for(int i = 0;i<5;i++) {
				//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
				PaySplits.InsertMany(0,transferResults.ListSplitsCur);
				transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			}
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_OverallocatedAdjustment() {
			/*****************************************************
				Create Provider: provNum1
				Create Patient: pat1
				Create Procedure1:  1/1/2019  provNum1  pat1   $100
				Create Adjustment1: 1/1/2019  provNum1  pat1   $25
					^Attached to Procedure1
				Create Payment1:    1/2/2019  provNum1  pat1   $25
					^Attached to Adjustment1
			******************************************************/
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat1=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Procedure procedure1=ProcedureT.CreateProcedure(pat1,"I0001",ProcStat.C,"",100,procDate:new DateTime(2019,1,1),provNum:provNum1);
			Adjustment adjustment1=AdjustmentT.MakeAdjustment(pat1.PatNum,25,adjDate:new DateTime(2019,1,1),procDate:procedure1.ProcDate
				,procNum:procedure1.ProcNum,provNum:provNum1);
			Payment payment1=PaymentT.MakePayment(pat1.PatNum,25,payDate:new DateTime(2019,1,2),provNum:provNum1,adjNum:adjustment1.AdjNum);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			/*****************************************************
			Paysplit1:  Today  provNum1  pat1  -$25
				^Attached to Adjustment1
			Paysplit2:  Today  provNum1  pat1   $25
				^Unearned
			Paysplit3:  Today  provNum1  pat1  -$25
				^Unearned
			Paysplit4:  Today  provNum1  pat1   $25
				^Attached to Procedure1
			******************************************************/
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-25
				&& x.AdjNum==adjustment1.AdjNum));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==25
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==-25
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.ProvNum==provNum1
				&& x.PatNum==pat1.PatNum
				&& x.SplitAmt==25
				&& x.ProcNum==procedure1.ProcNum));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		///<summary>The income transfer system should not suggest making a PaySplit that is attached to both a procedure and an adjustment.</summary>
		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_SplitsAttachedToProcedureOrAdjustment() {
			//The PaySplits that the income transfer system suggests should not be attached to both a procedure and an adjustment.
			//Make a procedure for provNum1 and an adjustment for provNum2 that is attached to the procedure.
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"TASTA",ProcStat.C,"",100,procDate:DateTime.Today,provNum:provNum1);
			Adjustment adjust1=AdjustmentT.MakeAdjustment(pat.PatNum,10,DateTime.Today,procNum:proc1.ProcNum,provNum:provNum2);
			//Make a payment to unearned for provNum0 (doesn't exist) so that an income transfer is required for each account entry.
			Payment payment1=PaymentT.MakePayment(pat.PatNum,200,provNum:0,unearnedType:unearnedType);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(4,transferResults.ListSplitsCur.Count);
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.SplitAmt==-100
				&& x.ProvNum==0
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.SplitAmt==100
				&& x.ProvNum==provNum1
				&& x.AdjNum==0
				&& x.ProcNum==proc1.ProcNum
				&& x.PayPlanNum==0
				&& x.UnearnedType==0));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.SplitAmt==-10
				&& x.ProvNum==0
				&& x.AdjNum==0
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.UnearnedType==unearnedType));
			Assert.AreEqual(1,transferResults.ListSplitsCur.Count(x => x.SplitAmt==10
				&& x.ProvNum==provNum2
				&& x.ProcNum==0
				&& x.AdjNum==adjust1.AdjNum
				&& x.PayPlanNum==0
				&& x.UnearnedType==0));
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertMany(0,transferResults.ListSplitsCur);
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_UnearnedWithNoProvider() {
			//Sorry for the complicated setup.  It's an actual scenario that took me a long time to narrow down to this scope...
			/*****************************************************
				Create Provider: prov1
				Create Patient: pat1
				Create ProcedureCode: procCode1
				Create ProcedureCode: procCode2
				Create ProcedureCode: procCode3
				Create proc1:   1/7/2018  procCode1  prov1  pat1  $5
				Create proc2:   1/7/2018  procCode2  prov1  pat1  $20.78
				Create proc3:   1/7/2018  procCode3  prov1  pat1  $99    (deleted)
				Create proc4:   2/7/2018  procCode1  prov1  pat1  $5
				Create proc5:   2/7/2018  procCode2  prov1  pat1  $26
				Create proc6:   2/7/2018  procCode3  prov1  pat1  $99
				Create proc7:   3/7/2018  procCode1  prov1  pat1  $5
				Create proc8:   3/7/2018  procCode2  prov1  pat1  $24.78
				Create proc9:   3/7/2018  procCode3  prov1  pat1  $99    (deleted)
				Create proc10:  4/7/2018  procCode1  prov1  pat1  $5
				Create proc11:  4/7/2018  procCode2  prov1  pat1  $31.80
				Create proc12:  4/7/2018  procCode3  prov1  pat1  $99    (deleted)
				Create payment1:  2/05/2018  prov1  pat1  $99
					^split1:  prov1  pat1   $17.06  --  attached to proc2
					^split2:  prov1  pat1   $81.94  --  unearned 
				Create payment2:  2/07/2018  prov1  pat1  $0
					^split3:  prov0  pat1  -$81.94  --  unearned  !!NOTICE PROV0!!
					^split4:  prov1  pat1   $81.94  --  attached to proc6
				Create payment3:  2/21/2018  prov1  pat1  $107.34
					^split5:  prov1  pat1   $48.06  --  unallocated
					^split6:  prov1  pat1   $59.28  --  unearned
				Create payment4:  4/18/2018  prov1  pat1  $7.30
					^split7:  prov1  pat1   $7.30   --  unallocated
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat1=PatientT.CreatePatient(suffix);
			ProcedureCode procCode1=ProcedureCodeT.CreateProcCode("UWNP1");
			ProcedureCode procCode2=ProcedureCodeT.CreateProcCode("UWNP2");
			ProcedureCode procCode3=ProcedureCodeT.CreateProcCode("UWNP3");
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			Procedure proc1=ProcedureT.CreateProcedure(pat1,procCode1.ProcCode,ProcStat.C,"",5,procDate:new DateTime(2018,1,7),provNum:provNum1);
			Procedure proc2=ProcedureT.CreateProcedure(pat1,procCode2.ProcCode,ProcStat.C,"",20.78,procDate:new DateTime(2018,1,7),provNum:provNum1);
			Procedure proc3=ProcedureT.CreateProcedure(pat1,procCode3.ProcCode,ProcStat.D,"",99,procDate:new DateTime(2018,1,7),provNum:provNum1);
			Procedure proc4=ProcedureT.CreateProcedure(pat1,procCode1.ProcCode,ProcStat.C,"",5,procDate:new DateTime(2018,2,7),provNum:provNum1);
			Procedure proc5=ProcedureT.CreateProcedure(pat1,procCode2.ProcCode,ProcStat.C,"",26,procDate:new DateTime(2018,2,7),provNum:provNum1);
			Procedure proc6=ProcedureT.CreateProcedure(pat1,procCode3.ProcCode,ProcStat.C,"",99,procDate:new DateTime(2018,2,7),provNum:provNum1);
			Procedure proc7=ProcedureT.CreateProcedure(pat1,procCode1.ProcCode,ProcStat.C,"",5,procDate:new DateTime(2018,3,7),provNum:provNum1);
			Procedure proc8=ProcedureT.CreateProcedure(pat1,procCode2.ProcCode,ProcStat.C,"",24.78,procDate:new DateTime(2018,3,7),provNum:provNum1);
			Procedure proc9=ProcedureT.CreateProcedure(pat1,procCode3.ProcCode,ProcStat.D,"",99,procDate:new DateTime(2018,3,7),provNum:provNum1);
			Procedure proc10=ProcedureT.CreateProcedure(pat1,procCode1.ProcCode,ProcStat.C,"",5,procDate:new DateTime(2018,4,7),provNum:provNum1);
			Procedure proc11=ProcedureT.CreateProcedure(pat1,procCode2.ProcCode,ProcStat.C,"",31.80,procDate:new DateTime(2018,4,7),provNum:provNum1);
			Procedure proc12=ProcedureT.CreateProcedure(pat1,procCode3.ProcCode,ProcStat.D,"",99,procDate:new DateTime(2018,4,7),provNum:provNum1);
			Payment payment1=PaymentT.MakePaymentNoSplits(pat1.PatNum,99,payDate:new DateTime(2018,2,5));
			PaySplit split1=PaySplitT.CreateOne(pat1.PatNum,17.06,payment1.PayNum,provNum1,procNum:proc2.ProcNum);
			PaySplit split2=PaySplitT.CreateOne(pat1.PatNum,81.94,payment1.PayNum,provNum1,unearnedType:unearnedType);
			Payment payment2=PaymentT.MakePaymentNoSplits(pat1.PatNum,0,payDate:new DateTime(2018,2,7));
			PaySplit split3=PaySplitT.CreateOne(pat1.PatNum,-81.94,payment1.PayNum,0,unearnedType:unearnedType);//NOTICE PROV0
			PaySplit split4=PaySplitT.CreateOne(pat1.PatNum,81.94,payment1.PayNum,provNum1,procNum:proc6.ProcNum);
			Payment payment3=PaymentT.MakePaymentNoSplits(pat1.PatNum,107.34,payDate:new DateTime(2018,2,21));
			PaySplit split5=PaySplitT.CreateOne(pat1.PatNum,48.06,payment1.PayNum,provNum1);//unallocated
			PaySplit split6=PaySplitT.CreateOne(pat1.PatNum,59.28,payment1.PayNum,provNum1,unearnedType:unearnedType);
			Payment payment4=PaymentT.MakePaymentNoSplits(pat1.PatNum,7.30,payDate:new DateTime(2018,4,18));
			PaySplit split7=PaySplitT.CreateOne(pat1.PatNum,7.30,payment1.PayNum,provNum1);//unallocated
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			double totalNonUnearned=transferResults.ListSplitsCur.Where(x => x.UnearnedType==0).Sum(x => x.SplitAmt);
			double totalUnearned=transferResults.ListSplitsCur.Where(x => x.UnearnedType > 0).Sum(x => x.SplitAmt);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertManyWithAssociated(0,transferResults.ListSplitsCur,transferResults.ListSplitsAssociated);
			decimal unearnedForFam=PaySplits.GetSumUnearnedForFam(Patients.GetFamily(pat1.PatNum));
			//Uneared should never be negative after an income transfer has been saved to the database.
			Assert.IsTrue(unearnedForFam.IsGreaterThanOrEqualToZero(),$"Unearned should not be negative after an income transfer: {unearnedForFam:f}");
			//Run another income transfer and nothing should be suggested.
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat1.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_UnallocatedWithNoProviderSimplified() {
			/*****************************************************
				Create Provider: prov1
				Create Provider: prov2
				Create Patient: pat1
				Create proc1:     Today  prov1  pat1  $60
				Create payment1:  Today  prov1  pat1  $50
					^Unallocated
				Create payment2:  Today  prov1  pat1  $0
					^split3:  prov2  pat1  -$50  --  unearned  !!NOTICE this is taking from a provider that doesn't have any unearned to take from!!
					^split4:  prov1  pat1   $50  --  attached to proc1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"UWNPS1",ProcStat.C,"",60,provNum:provNum1);
			Payment payment1=PaymentT.MakePayment(pat.PatNum,50,provNum:provNum1);//Unallocated
			Payment payment2=PaymentT.MakePaymentNoSplits(pat.PatNum,0,payDate:new DateTime(2018,2,7));
			PaySplit split1=PaySplitT.CreateOne(pat.PatNum,-50,payment2.PayNum,provNum2,unearnedType:unearnedType);//Notice provNum2!
			PaySplit split2=PaySplitT.CreateOne(pat.PatNum,50,payment2.PayNum,provNum1,procNum:proc1.ProcNum);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertManyWithAssociated(0,transferResults.ListSplitsCur,transferResults.ListSplitsAssociated);
			decimal unearnedForFam=PaySplits.GetSumUnearnedForFam(Patients.GetFamily(pat.PatNum));
			//Uneared should never be negative after an income transfer has been saved to the database.
			Assert.IsTrue(unearnedForFam.IsGreaterThanOrEqualToZero(),$"Unearned should not be negative after an income transfer: {unearnedForFam:f}");
			//Run another income transfer and nothing should be suggested.
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_BalanceAndIncomeTransfer_UnearnedWithNoProviderSimplified() {
			/*****************************************************
				Create Provider: prov1
				Create Provider: prov2
				Create Patient: pat1
				Create proc1:     Today  prov1  pat1  $60
				Create payment1:  Today  prov1  pat1  $50
					^Unearned
				Create payment2:  Today  prov1  pat1  $0
					^split3:  prov2  pat1  -$50  --  unearned  !!NOTICE this is taking from a provider that doesn't have any unearned to take from!!
					^split4:  prov1  pat1   $50  --  attached to proc1
			******************************************************/
			string suffix=MethodBase.GetCurrentMethod().Name;
			long unearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			Patient pat=PatientT.CreatePatient(suffix);
			long provNum1=ProviderT.CreateProvider($"{suffix}-1");
			long provNum2=ProviderT.CreateProvider($"{suffix}-2");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"UWNPS1",ProcStat.C,"",60,provNum:provNum1);
			Payment payment1=PaymentT.MakePayment(pat.PatNum,50,provNum:provNum1,unearnedType:unearnedType);
			Payment payment2=PaymentT.MakePaymentNoSplits(pat.PatNum,0,payDate:new DateTime(2018,2,7));
			PaySplit split1=PaySplitT.CreateOne(pat.PatNum,-50,payment2.PayNum,provNum2,unearnedType:unearnedType);//Notice provNum2!
			PaySplit split2=PaySplitT.CreateOne(pat.PatNum,50,payment2.PayNum,provNum1,procNum:proc1.ProcNum);
			PaymentEdit.IncomeTransferData transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			//Make the income transfer official by inserting the splits into the database and then run the transfer again.  No splits should be made!
			PaySplits.InsertManyWithAssociated(0,transferResults.ListSplitsCur,transferResults.ListSplitsAssociated);
			decimal unearnedForFam=PaySplits.GetSumUnearnedForFam(Patients.GetFamily(pat.PatNum));
			//Uneared should never be negative after an income transfer has been saved to the database.
			Assert.IsTrue(unearnedForFam.IsGreaterThanOrEqualToZero(),$"Unearned should not be negative after an income transfer: {unearnedForFam:f}");
			//Run another income transfer and nothing should be suggested.
			transferResults=PaymentT.BalanceAndIncomeTransfer(pat.PatNum);
			Assert.AreEqual(0,transferResults.ListSplitsCur.Count);
		}

		#endregion

		#region Helper Methods

		private void UpdatePaySplitHelper(PaySplit ps,long patNum,long provNum,long clinicNum,long payNum) {
			if(payNum!=0 && payNum==ps.PayNum) {
				ps.PatNum=patNum;
				ps.ProvNum=provNum;
				ps.ClinicNum=clinicNum;
				PaySplits.Update(ps);
			}
		}

		private void ResetPrepayments(Patient pat) {
			Family fam=Patients.GetFamily(pat.PatNum);
			_listFamPrePaySplits=PaySplits.GetPrepayForFam(fam);
			_listPosPrePay=_listFamPrePaySplits.FindAll(x => x.SplitAmt.IsGreaterThan(0));
			_listNegPrePay=_listFamPrePaySplits.FindAll(x => x.SplitAmt.IsLessThan(0));
		}

		#endregion

		#region Methods - Helpers

		private List<AccountEntry> ConstructListCharges(List<Procedure> listProcs=null,List<Adjustment> listAdjustments=null,
			List<PaySplit> listPaySplits=null,List<PayAsTotal> listInsPayAsTotal=null,List<PayPlanCharge> listPayPlanCharges=null,
			List<PayPlanLink> listPayPlanLinks=null,bool isIncomeTxfr=true,bool doShowHiddenSplits=false,List<ClaimProc> listClaimProcs=null,
			PaymentEdit.LoadData loadData=null)
		{
			return PaymentEdit.ConstructListCharges(listProcs??new List<Procedure>(),
				listAdjustments??new List<Adjustment>(),
				listPaySplits??new List<PaySplit>(),
				listInsPayAsTotal??new List<PayAsTotal>(),
				listPayPlanCharges??new List<PayPlanCharge>(),
				listPayPlanLinks??new List<PayPlanLink>(),
				isIncomeTxfr,
				doShowHiddenSplits,
				listClaimProcs??new List<ClaimProc>(),
				loadData);
		}

		#endregion

	}
}
