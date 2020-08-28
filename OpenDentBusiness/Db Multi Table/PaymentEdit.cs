using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using CodeBase;

namespace OpenDentBusiness {
	public class PaymentEdit {

		#region FormPayment
		///<summary>Gets most all the data needed to load FormPayment.</summary>
		public static LoadData GetLoadData(Patient patCur,Payment paymentCur,bool isNew,bool isIncomeTxfr) {
			LoadData data=new LoadData();
			data.PatCur=patCur;
			data.Fam=Patients.GetFamily(patCur.PatNum);
			data.SuperFam=new Family(Patients.GetBySuperFamily(patCur.SuperFamily));
			data.ListCreditCards=CreditCards.Refresh(patCur.PatNum);
			data.XWebResponse=XWebResponses.GetOneByPaymentNum(paymentCur.PayNum);
			data.PayConnectResponseWeb=PayConnectResponseWebs.GetOneByPayNum(paymentCur.PayNum);
			data.PaymentCur=paymentCur;
			data.TableBalances=Patients.GetPaymentStartingBalances(patCur.Guarantor,paymentCur.PayNum);
			data.TableBalances.TableName="TableBalances";
			data.ListSplits=PaySplits.GetForPayment(paymentCur.PayNum);
			data.ListPaySplitAllocations=PaySplits.GetSplitsForPrepay(data.ListSplits);
			if(!isNew) {
				data.Transaction=Transactions.GetAttachedToPayment(paymentCur.PayNum);
			}
			data.ListValidPayPlans=PayPlans.GetValidPlansNoIns(patCur.PatNum);
			List<long> listFamilyPatNums=data.Fam.GetPatNums();
			if(patCur.SuperFamily > 0) {
				//Add all of the super family members to listFamilyPatNums if there are any splits for super family members outside of the direct family.
				if(data.ListSplits.Any(x => !x.PatNum.In(listFamilyPatNums) && x.PatNum.In(data.SuperFam.GetPatNums()))) {
					listFamilyPatNums.AddRange(data.SuperFam.ListPats.Select(x => x.PatNum));
				}
			}
			//if there were no payment plans found where this patient is the guarantor, find payment plans in the family.
			if(data.ListValidPayPlans.Count == 0) {
				//Do not include insurance payment plans.
				data.ListValidPayPlans=PayPlans.GetForPats(listFamilyPatNums,patCur.PatNum).FindAll(x => !x.IsClosed && x.PlanNum==0);
			}
			data.ListAssociatedPatients=Patients.GetAssociatedPatients(patCur.PatNum);
			data.ListPrePaysForPayment=PaySplits.GetSplitsLinked(data.ListSplits);
			data.ListProcsForSplits=Procedures.GetManyProc(data.ListSplits.Select(x => x.ProcNum).ToList(),false);
			data.ConstructChargesData=GetConstructChargesData(listFamilyPatNums,patCur.PatNum,data.ListSplits,paymentCur.PayNum,isIncomeTxfr);
			List<Procedure> listTpProcs=data.ConstructChargesData.ListProcs.Where(x => x.ProcStatus==ProcStat.TP).ToList();
			LoadDiscountPlanInfo(ref data,listTpProcs);
			return data;
		}

		///<summary>Performs explicit linking, imiplicit linking, and auto split logic depending on the parameters provided.</summary>
		public static InitData Init(LoadData loadData,List<AccountEntry> listPayFirstAcctEntries=null,Dictionary<long,Patient> dictPatients=null,
			bool isIncomeTxfr=false,bool isPatPrefer=false,bool doAutoSplit=true,bool doIncludeExplicitCreditsOnly=false)
		{
			//No remoting role check; no call to db
			InitData initData=new InitData();
			//get patients who have this patient's guarantor as their payplan's guarantor
			List<Patient> listPatients=loadData.ListAssociatedPatients;
			listPatients.AddRange(loadData.Fam.ListPats);
			if(loadData.SuperFam.ListPats!=null) {
				listPatients.AddRange(loadData.SuperFam.ListPats);
			}
			List<long> listPatNums=listPatients.Select(x => x.PatNum).ToList();
			//Add patients with paysplits on this payment
			List<long> listUnknownPatNums=loadData.ListSplits.Select(x => x.PatNum)
				.Where(x => !x.In(listPatNums))
				.Distinct()
				.ToList();
			listPatients.AddRange(Patients.GetLimForPats(listUnknownPatNums));
			if(dictPatients==null) {
				initData.DictPats=listPatients.GroupBy(x => x.PatNum).ToDictionary(x => x.Key,x => x.First());
			}
			else {
				//Preserve any patients already present in the dictionary.
				initData.DictPats=dictPatients;
				//But overwrite or add to the dictionary for any patients that it might not already know about.
				foreach(Patient patient in listPatients) {
					initData.DictPats[patient.PatNum]=patient;
				}
			}
			initData.SplitTotal=(decimal)loadData.ListSplits.Sum(x => x.SplitAmt);
			loadData.PaymentCur.PayAmt=Math.Round(loadData.PaymentCur.PayAmt-(double)initData.SplitTotal,3);
			if(listPayFirstAcctEntries==null) {
				listPayFirstAcctEntries=new List<AccountEntry>();
			}
			initData.AutoSplitData=AutoSplitForPayment(listPatNums,loadData.PatCur.PatNum,loadData.ListSplits,loadData.PaymentCur,listPayFirstAcctEntries,
				isIncomeTxfr,isPatPrefer,loadData,doAutoSplit,doIncludeExplicitCreditsOnly);
			loadData.ListSplits.AddRange(initData.AutoSplitData.ListAutoSplits);
			initData.AutoSplitData.ListSplitsCur=loadData.ListSplits.Union(initData.AutoSplitData.ListAutoSplits).ToList();
			return initData;
		}
		#endregion

		#region constructing and linking charges and credits
		///<summary>Gets the data needed to construct a list of charges on FormPayment.</summary>
		public static ConstructChargesData GetConstructChargesData(List<long> listPatNums,long patNum,List<PaySplit> listSplitsCur,long payCurNum
			,bool isIncomeTransfer) 
		{
			ConstructChargesData data=new ConstructChargesData();
			data.ListPaySplits=PaySplits.GetForPats(listPatNums);//Might contain payplan payments.
			data.ListProcs=Procedures.GetCompleteForPats(listPatNums);//will also contain TP procs if pref is set to ON
			if(Prefs.GetBool(PrefName.PrePayAllowedForTpProcs) && !isIncomeTransfer) {
				data.ListProcs.AddRange(Procedures.GetTpForPats(listPatNums));
			}
			if(isIncomeTransfer) {
				//do not show pre-payment splits that are attached to TP procedures, they should not be transferred since they are already reserved money
				//only TP unearned should have an unearned type and a procNum. 
				data.ListPaySplits.RemoveAll(x => x.UnearnedType!=0 && x.ProcNum!=0 && !x.ProcNum.In(data.ListProcs.Select(y => y.ProcNum)));
			}
			else {
				//sum ins pay as totals by claims to include the value for the ones that have already been transferred and taken care of. 
				data.ListInsPayAsTotal=ClaimProcs.GetOutstandingClaimPayByTotal(listPatNums);
			}
			data.ListPayPlans=PayPlans.GetForPats(listPatNums,patNum);//Used to figure out how much we need to pay off procs with, also contains ins payplans
			if(data.ListPayPlans.Count>0) {
				List<long> listPayPlanNums=data.ListPayPlans.Select(x => x.PayPlanNum).ToList();
				//get list where payplan guar is not in the fam)
				data.ListPayPlanSplits=PaySplits.GetForPayPlans(listPayPlanNums);
				data.ListPayPlanCharges=PayPlanCharges.GetForPayPlans(listPayPlanNums,listPatNums);
				data.ListPayPlanLinks=PayPlanLinks.GetForPayPlans(listPayPlanNums);
				data.ListPaySplits.AddRange(data.ListPayPlanSplits.FindAll(x => !data.ListPaySplits.Any(y => y.SplitNum==x.SplitNum)));
			}
			//Look for splits that are in the database yet have been deleted from the pay splits grid.
			for(int i=data.ListPaySplits.Count-1;i>=0;i--) {
				bool isFound=listSplitsCur.Any(x => x.SplitNum==data.ListPaySplits[i].SplitNum);
				if(!isFound && data.ListPaySplits[i].PayNum==payCurNum) {
					//If we have a split that's not found in the passed-in list of splits for the payment
					//and the split we got from the DB is on this payment, remove it because the user must have deleted the split from the payment window.
					//The payment window won't update the DB with the change until it's closed.
					data.ListPaySplits.RemoveAt(i);
				}
			}
			//In the rare case that a Payment Plan has a Guarantor outside of the Patient's family, we want to make sure to collect more 
			//data for Adjustments and ClaimProcs so that debits and credits balance properly.
			if(data.ListPayPlans.Any(x => x.Guarantor!=x.PatNum && !x.PatNum.In(listPatNums))) {
				data.ListAdjustments=Adjustments.GetForProcs(data.ListProcs.Select(x => x.ProcNum).ToList());
				//still does not contain PayAsTotals
				data.ListClaimProcsFiltered=ClaimProcs.GetForProcs(data.ListProcs.FindAll(x => x.ProcNum!=0).Select(x => x.ProcNum).ToList());
			}
			else {//Otherwise get the smaller dataset as it will suffice for our needs.
				data.ListAdjustments=Adjustments.GetAdjustForPats(listPatNums);
				data.ListClaimProcsFiltered=ClaimProcs.Refresh(listPatNums).Where(x => x.ProcNum!=0).ToList();//does not contain PayAsTotals
			}
			return data;
		}

		///<summary>Gets the charges and links credits for the patient passed in only.</summary>
		public static ConstructResults ConstructAndLinkChargeCredits(long patNum,bool isIncomeTxfr=false) {
			//No remoting role check; no call to db
			return ConstructAndLinkChargeCredits(new List<long>() { patNum },patNum,new List<PaySplit>(),new Payment(),new List<AccountEntry>(),
				isIncomeTxfr:isIncomeTxfr);
		}

		/// <summary>Helper method that does the entire original auto split for payment. Gets the charges, and links the credits.</summary>
		public static ConstructResults ConstructAndLinkChargeCredits(List<long> listPatNums,long patCurNum,List<PaySplit> listSplitsCur,Payment payCur
			,List<AccountEntry> listPayFirstAcctEntries,bool isIncomeTxfr=false,bool isPreferCurPat=false,LoadData loadData=null
			,bool doIncludeExplicitCreditsOnly=false,bool doShowHiddenSplits=false)
		{
			//No remoting role check; no call to db
			ConstructResults retVal=new ConstructResults();
			retVal.Payment=payCur;
			#region Get data
			//Get the lists of items we'll be using to calculate with.
			ConstructChargesData constructChargesData=loadData?.ConstructChargesData
				??GetConstructChargesData(listPatNums,patCurNum,listSplitsCur,retVal.Payment.PayNum,isIncomeTxfr);
			#endregion
			#region Construct List of Charges
			retVal.ListAccountCharges=ConstructListCharges(constructChargesData.ListProcs,
				constructChargesData.ListAdjustments,
				constructChargesData.ListPaySplits,
				constructChargesData.ListInsPayAsTotal,
				constructChargesData.ListPayPlanCharges,
				constructChargesData.ListPayPlanLinks,
				isIncomeTxfr,
				doShowHiddenSplits,
				constructChargesData.ListClaimProcsFiltered,
				loadData);
			retVal.ListAccountCharges.Sort(AccountEntrySort);
			#endregion
			#region Explicitly Link Credits
			//When executing an income transfer from within the payment window listSplitsCur can be filled with new splits.
			//These splits need to be present in the list of outstanding charges so that their value can be considered within the pat/prov/clinic bucket.
			//This is because the Outstanding Charges grid is forced to be grouped by pat/prov/clinic when in transfer mode.
			//The AmountEnd column for the grouping could show incorrectly without these Account Entries after a transfer has been made.
			if(isIncomeTxfr) {
				retVal.ListAccountCharges.AddRange(listSplitsCur.Where(x => x.SplitNum==0).Select(x => new AccountEntry(x)));
			}
			//Make deep copies of the current splits that are attached to the payment because the SplitAmt field will be manipulated below.
			List<PaySplit> listSplitsCurrent=listSplitsCur.Where(x => x.SplitNum==0).Select(y => y.Copy()).ToList();
			List<PaySplit> listSplitsHistoric=constructChargesData.ListPaySplits.Select(x => x.Copy()).ToList();
			List<PaySplit> listSplitsCurrentAndHistoric=listSplitsCurrent;
			listSplitsCurrentAndHistoric.AddRange(listSplitsHistoric);
			//This ordering is necessary so parents come before their children when explicitly linking credits.
			listSplitsCurrentAndHistoric=listSplitsCurrentAndHistoric.OrderBy(x => x.SplitNum > 0)
				.ThenBy(x => x.DatePay)
				.ThenBy(x => x.FSplitNum)
				.ToList();
			retVal.ListAccountCharges=ExplicitlyLinkCredits(retVal,
				listSplitsCurrentAndHistoric);
			#endregion
			#region Implicitly Link Credits
			if(!isIncomeTxfr && !doIncludeExplicitCreditsOnly) {//If this payment is an income transfer, do NOT use unallocated income to pay off charges.
				PayResults implicitResult=ImplicitlyLinkCredits(listSplitsHistoric,
					constructChargesData.ListInsPayAsTotal,
					retVal.ListAccountCharges,
					listSplitsCur,
					listPayFirstAcctEntries,
					retVal.Payment,
					patCurNum,
					isPreferCurPat);
				retVal.ListAccountCharges=implicitResult.ListAccountCharges;
				retVal.ListSplitsCur=implicitResult.ListSplitsCur;
			}
			#endregion
			#region Set AmountAvailable
			//Set the AmountAvailable field on each account entry to the sum of all PaySplits that are associated to other payments.
			foreach(AccountEntry accountEntry in retVal.ListAccountCharges.Where(x => x.PayPlanChargeNum==0)) {
				double amtUsed=accountEntry.SplitCollection.Where(x => x.SplitNum > 0 && x.PayNum!=payCur.PayNum && x!=accountEntry.Tag)
					.Sum(x => x.SplitAmt);
				accountEntry.AmountAvailable=accountEntry.AmountOriginal-(decimal)amtUsed;
			}
			//Payment plan account entries are handled differently because they can have multiple account entries that represent one payment plan charge.
			Dictionary<long,List<AccountEntry>> dictPayPlanChargeEntries=retVal.ListAccountCharges.Where(x => x.PayPlanChargeNum > 0)
				.GroupBy(x => x.PayPlanChargeNum)
				.ToDictionary(x => x.Key,x => x.ToList());
			foreach(long payPlanChargeNum in dictPayPlanChargeEntries.Keys) {
				List<PaySplit> listPaySplits=new List<PaySplit>();
				//Get all unique PaySplits that are associated to this charge.
				foreach(AccountEntry accountEntry in dictPayPlanChargeEntries[payPlanChargeNum]) {
					foreach(PaySplit paySplit in accountEntry.SplitCollection.Where(x => x.SplitNum > 0 && x.PayNum!=payCur.PayNum && x!=accountEntry.Tag)) {
						if(!listPaySplits.Any(x => x.SplitNum==paySplit.SplitNum)) {
							listPaySplits.Add(paySplit);
						}
					}
				}
				double amtToAllocate=listPaySplits.Sum(x => x.SplitAmt);
				//Only apply as much as possible to each charge (might be a tiny amount of interest followed by a large principal amount).
				foreach(AccountEntry accountEntry in dictPayPlanChargeEntries[payPlanChargeNum]) {
					double amtUsed=Math.Min(amtToAllocate,(double)accountEntry.AmountOriginal);
					accountEntry.AmountAvailable=accountEntry.AmountOriginal-(decimal)amtUsed;
					amtToAllocate-=amtUsed;
				}
			}
			#endregion
			return retVal;
		}

		public static List<AccountEntry> ConstructListCharges(List<Procedure> listProcs,List<Adjustment> listAdjustments,List<PaySplit> listPaySplits,
			List<PayAsTotal> listInsPayAsTotal,List<PayPlanCharge> listPayPlanCharges,List<PayPlanLink> listPayPlanLinks,bool isIncomeTxfr,
			bool doShowHiddenSplits,List<ClaimProc> listClaimProcs,LoadData loadData=null)
		{
			//No remoting role check; no call to db
			List<AccountEntry> listCharges=new List<AccountEntry>();
			#region Procedures
			listCharges.AddRange(listProcs.Select(x => new AccountEntry(x)));
			if(loadData==null) {
				List<Procedure> listTpProcs=listCharges.Where(x => x.Tag is Procedure && ((Procedure)x.Tag).ProcStatus==ProcStat.TP)
					.Select(x => (Procedure)x.Tag).ToList();
				LoadDiscountPlanInfo(ref loadData,listTpProcs);
			}
			foreach(AccountEntry accountEntryProc in listCharges) {
				Procedure proc=(Procedure)accountEntryProc.Tag;
				#region Set AmountEnd
				//Set the AmountEnd field for each procedure account entry to the patient portion.
				if(proc.ProcStatus==ProcStat.TP
					&& loadData.DictPatToDPFeeScheds!=null
					&& loadData.DictPatToDPFeeScheds.TryGetValue(proc.PatNum,out long dpFeeSchedNum)
					&& dpFeeSchedNum!=0) 
				{
					//If the patient has a discount plan and this is a TP proc, use the discount plan fee
					decimal procFee=(decimal)Fees.GetAmount(proc.CodeNum,dpFeeSchedNum,proc.ClinicNum,proc.ProvNum,loadData.ListFeesForDiscountPlans);
					if(procFee.IsEqual(-1)) { //Use the UCR fee if there is no provided fee in the Discount Plan
						procFee=GetPatPortion(proc,listClaimProcs);
					}
					//Insurance does not need to be considered because a patient cannot have a discount plan and insurance at the same time
					accountEntryProc.AmountEnd=procFee;
				}
				else {
					accountEntryProc.AmountEnd=GetPatPortion(proc,listClaimProcs);
				}
				#endregion
			}
			#endregion
			#region Adjustments
			listCharges.AddRange(listAdjustments.Select(x => new AccountEntry(x)));
			#endregion
			#region Payment Plans
			if(!listPayPlanCharges.IsNullOrEmpty() || !listPayPlanLinks.IsNullOrEmpty()) {
				listCharges.AddRange(GetFauxEntriesForPayPlans(listPayPlanCharges,listPayPlanLinks,listCharges));
			}
			#endregion
			#region Unearned
			if(isIncomeTxfr) {
				List<long> listHiddenUnearnedTypes=PaySplits.GetHiddenUnearnedDefNums();
				for(int i=listPaySplits.Count-1;i>=0;i--) {
					//If we are hiding hidden splits and current split doesn't have hidden type OR we are not hiding hidden splits, add split to listCharges
					if(doShowHiddenSplits || !listHiddenUnearnedTypes.Contains(listPaySplits[i].UnearnedType)) {
						//In Income Transfer mode, add all paysplits to the buckets.  
						//The income transfers made previously should balance out any adjustments/inspaytotals that were transferred previously.
						listCharges.Add(new AccountEntry(listPaySplits[i]));
					}
				}
				foreach(PayAsTotal totalPmt in listInsPayAsTotal) {//Ins pay totals need to be added to the sum total for income transfers
					listCharges.Add(new AccountEntry(totalPmt));
				}
			}
			#endregion
			return listCharges;
		}

		///<summary>Helper method that will eventually invoke ClaimProcs.GetPatPortion().
		///This method will completely ignore the list of ClaimProcs passed in if all related to the proc have NotBillIns set.</summary>
		private static decimal GetPatPortion(Procedure proc,List<ClaimProc> listClaimProcs) {
			//No remoting role check; no call to db
			//There is an extremely rare scenario where a completed procedure can be flagged as "Do Not Bill to Ins" but will still have ins estimates.
			//Act like there are no ClaimProcs for the procedure in question when all ClaimProcs are flagged as NoBillIns.
			if(listClaimProcs.Where(x => x.ProcNum==proc.ProcNum).All(x => x.NoBillIns)) {
				return ClaimProcs.GetPatPortion(proc,new List<ClaimProc>());
			}
			else {//One or more ClaimProcs are flagged to be billed to insurance, take their value into consideration.
				return ClaimProcs.GetPatPortion(proc,listClaimProcs);
			}
		}

		private static List<FauxAccountEntry> GetFauxEntriesForPayPlans(List<PayPlanCharge> listPayPlanCharges,List<PayPlanLink> listPayPlanLinks,
			List<AccountEntry> listCharges)
		{
			List<FauxAccountEntry> listPayPlanAccountEntries=new List<FauxAccountEntry>();
			List<PayPlanCharge> listPayPlanChargeCredits=listPayPlanCharges.FindAll(x => x.ChargeType==PayPlanChargeType.Credit);
			List<PayPlanCharge> listPayPlanChargeDebits=listPayPlanCharges.FindAll(x => x.ChargeType==PayPlanChargeType.Debit);
			List<PayPlanProductionEntry> listPayPlanProductionEntries=PayPlanProductionEntry.GetProductionForLinks(listPayPlanLinks);
			#region Patient Payment Plan Credits
			//Create faux account entries for all credits associated to a payment plan.
			foreach(PayPlanCharge payPlanChargeCredit in listPayPlanChargeCredits) {
				FauxAccountEntry fauxAccountEntry=new FauxAccountEntry(payPlanChargeCredit,true);
				if(!fauxAccountEntry.IsAdjustment) {
					//We don't technically know how much this faux procedure account entry is worth until we consider the debits that are due.  That is later.
					fauxAccountEntry.AmountEnd=0;
					//Prefer the patient, provider, and clinic combo from the procedure if present.
					if(payPlanChargeCredit.ProcNum > 0) {
						AccountEntry accountEntryProc=listCharges.FirstOrDefault(x => x.ProcNum > 0 && x.ProcNum==payPlanChargeCredit.ProcNum);
						if(accountEntryProc==null) {
							continue;//Do NOT add this FauxAccountEntry to the list of payment plan account entries because the associated proc was not found.
						}
						fauxAccountEntry.AccountEntryProc=accountEntryProc;
						fauxAccountEntry.PatNum=accountEntryProc.PatNum;
						fauxAccountEntry.ProvNum=accountEntryProc.ProvNum;
						fauxAccountEntry.ClinicNum=accountEntryProc.ClinicNum;
						//Take as much value away from the procedure as possible if principal is positive because a payplan might only cover part of a procedure.
						//Only do this for positive credits because negative procedure credits should not give value back to the procedure.
						if(fauxAccountEntry.Principal.IsGreaterThanZero()) {
							accountEntryProc.AmountEnd-=Math.Min(accountEntryProc.AmountEnd,fauxAccountEntry.Principal);
						}
					}
				}
				listPayPlanAccountEntries.Add(fauxAccountEntry);
			}
			#endregion
			#region Dynamic Payment Plan Credits
			//Create faux account entries for PayPlanProductionEntry procedures and adjustments (dynamic payment plan credits).
			foreach(PayPlanProductionEntry payPlanProdEntry in listPayPlanProductionEntries
				.FindAll(x => x.LinkType.In(PayPlanLinkType.Procedure,PayPlanLinkType.Adjustment)))
			{
				FauxAccountEntry fauxCreditEntry=new FauxAccountEntry(payPlanProdEntry);
				if(fauxCreditEntry.IsAdjustment) {
					AccountEntry accountEntryAdj=listCharges.FirstOrDefault(x => x.GetType()==typeof(Adjustment) && x.AdjNum > 0 && x.AdjNum==fauxCreditEntry.AdjNum);
					if(accountEntryAdj==null) {
						continue;
					}
					//Dynamic payment plans don't create multiple PayPlanCharge entries for adjustments (like the old payment plan system does).
					//There will be a single PayPlanLink entry that is attached to a specific adjustment.
					//The strange part is that the PayPlanCharge entries that are created for this dynamic payment plan have already factored in adjustments.
					//So take value away from the adjustment if AmountEnd is positive because a payplan might only cover part of an adjustment.
					if(fauxCreditEntry.AmountEnd.IsGreaterThanZero()) {
						accountEntryAdj.AmountEnd-=Math.Min(accountEntryAdj.AmountEnd,fauxCreditEntry.AmountEnd);
					}
					else if(fauxCreditEntry.AmountEnd.IsLessThanZero()) {
						accountEntryAdj.AmountEnd-=Math.Max(accountEntryAdj.AmountEnd,fauxCreditEntry.AmountEnd);
					}
				}
				else {//Procedure
					AccountEntry accountEntryProc=listCharges.FirstOrDefault(x => x.ProcNum > 0 && x.ProcNum==fauxCreditEntry.ProcNum);
					if(accountEntryProc==null) {
						continue;//Do NOT add this FauxAccountEntry to the list of payment plan account entries because the associated proc was not found.
					}
					fauxCreditEntry.AccountEntryProc=accountEntryProc;
					//Take as much value away from the procedure as possible if AmountEnd is positive because a payplan might only cover part of a procedure.
					//Only do this for positive credits because negative procedure credits should not give value back to the procedure.
					if(fauxCreditEntry.AmountEnd.IsGreaterThanZero()) {
						accountEntryProc.AmountEnd-=Math.Min(accountEntryProc.AmountEnd,fauxCreditEntry.AmountEnd);
					}
				}
				listPayPlanAccountEntries.Add(fauxCreditEntry);
			}
			#endregion
			#region All Payment Plan Debits
			foreach(PayPlanCharge payPlanDebit in listPayPlanChargeDebits) {
				if(!payPlanDebit.Principal.IsZero()) {
					listPayPlanAccountEntries.Add(new FauxAccountEntry(payPlanDebit,true));
				}
				if(!payPlanDebit.Interest.IsZero()) {
					listPayPlanAccountEntries.Add(new FauxAccountEntry(payPlanDebit,false));
				}
			}
			#endregion
			#region Manipulate Procedure AmountEnd
			//Now that all of the account entries have been created, manipulate the AmountEnd in preparation for explicit and implicit linking.
			//We need to figure out how much of the payment plan is actually due right now (debits due) and give that value to the faux entries.
			Dictionary<long,List<FauxAccountEntry>> dictPayPlanEntries=listPayPlanAccountEntries
				.GroupBy(x => x.PayPlanNum)
				.ToDictionary(x => x.Key,x => x.ToList());
			List<FauxAccountEntry> listAllocatedDebits=new List<FauxAccountEntry>();
			foreach(long payPlanNum in dictPayPlanEntries.Keys) {
				List<FauxAccountEntry> listDebits=dictPayPlanEntries[payPlanNum].FindAll(x => x.ChargeType==PayPlanChargeType.Debit);
				//Dynamic payment plans are strange in the sense that they utilize the PayPlanCharge FKey column to directly link DEBITS to procedures.
				//There is a report that breaks down 'overpaid payment plans' to a procedure level to help the user know exactly which procedures are wrong.
				//Therefore, loop through each faux credit entry one at a time and apply any matching due debits (via FKey) first. Starting with adjustments.
				foreach(FauxAccountEntry creditEntry in dictPayPlanEntries[payPlanNum].Where(x => x.AdjNum > 0)) {
					List<FauxAccountEntry> listAdjDebits=listDebits.FindAll(x => x.IsAdjustment && x.AdjNum==creditEntry.AdjNum);
					listAllocatedDebits.AddRange(AllocatePayPlanDebitsToCredit(creditEntry,listAdjDebits));
				}
				foreach(FauxAccountEntry fauxEntry in dictPayPlanEntries[payPlanNum].Where(x => x.ProcNum > 0)) {
					List<FauxAccountEntry> listProcDebits=listDebits.FindAll(x => !x.IsAdjustment && x.ProcNum==fauxEntry.ProcNum);
					listAllocatedDebits.AddRange(AllocatePayPlanDebitsToCredit(fauxEntry,listProcDebits));
				}
				//Same goes for adjustments.
				//Now that the dynamic payment plan credits have been handled to the best of our ability, blindly apply any leftover debits to credits.
				listAllocatedDebits.AddRange(AllocatePayPlanDebitsToCredits(dictPayPlanEntries[payPlanNum],listDebits));
			}
			#endregion
			//Only return debit faux account entries because the credits were only used to figure out where value was distributed.
			//Patients should never pay on credits and should only pay on debits (negative credits aren't supported ATM).
			listPayPlanAccountEntries.RemoveAll(x => x.ChargeType==PayPlanChargeType.Credit);
			//Remove any debits that have no value because they have been allocated to credits.
			listPayPlanAccountEntries.RemoveAll(x => x.ChargeType==PayPlanChargeType.Debit 
				&& x.AmountEnd.IsZero()
				&& x.PayPlanChargeNum.In(listAllocatedDebits.Select(y => y.PayPlanChargeNum)));
			//Add every distributed debit to the return value.
			listPayPlanAccountEntries.AddRange(listAllocatedDebits);
			//Remove all value from debits due in the future so that calling entities don't think these charges are due right now.
			listPayPlanAccountEntries.FindAll(x => x.Date > DateTime.Today).ForEach(x => x.AmountEnd=0);
			return listPayPlanAccountEntries;
		}

		private static List<FauxAccountEntry> AllocatePayPlanDebitsToCredit(FauxAccountEntry fauxCredit,List<FauxAccountEntry> listDebits) {
			return AllocatePayPlanDebitsToCredits(new List<FauxAccountEntry>() { fauxCredit },listDebits);
		}

		private static List<FauxAccountEntry> AllocatePayPlanDebitsToCredits(List<FauxAccountEntry> listFauxEntries,List<FauxAccountEntry> listDebits) {
			List<FauxAccountEntry> listAllocatedDebits=new List<FauxAccountEntry>();
			#region Adjustments (for non-dynamic payment plans)
			//Adjustments for patient payment plans will have a negative debit AND a negative credit.
			//These adjustments need to offset each other and push value back to the credits that are attached to procedures (if any).
			foreach(FauxAccountEntry fauxAdjCredit in listFauxEntries.FindAll(x => x.IsAdjustment && x.ChargeType==PayPlanChargeType.Credit && !x.IsDynamic)) {
				decimal amtToOffset=fauxAdjCredit.AmountEnd;//Used when offsetting adjustments.
				decimal amtToRemove=fauxAdjCredit.AmountEnd;//Used when removing value from due debits.
				decimal amtToAllocate=fauxAdjCredit.AmountEnd;//Used when allocating value back to procedure credits.
				#region Offset Adjustments
				//There should always be a credit adjustment entry with a corresponding debit adjustment for patient payment plans.
				//Since we are about to 'adjust' the entire value of the payment plan (remove value from due debits) we can offset the adjustment entries.
				foreach(FauxAccountEntry fauxAdjDebit in listFauxEntries.FindAll(x => x.IsAdjustment && x.ChargeType==PayPlanChargeType.Debit)) {
					if(amtToOffset.IsGreaterThanOrEqualToZero()) {
						break;
					}
					if(fauxAdjDebit.AmountEnd.IsGreaterThanOrEqualToZero()) {
						continue;
					}
					decimal amt=Math.Min(Math.Abs(amtToOffset),Math.Abs(fauxAdjDebit.AmountEnd));
					fauxAdjCredit.AmountEnd+=amt;
					fauxAdjDebit.AmountEnd+=amt;
					amtToOffset+=amt;
				}
				#endregion
				#region Remove Due Debits Value
				//This is where the real magic of payment plan adjustments takes place.
				//Removing value from debits that are due is how the payment plan is worth less money overall.
				//Patient payment plan debits are never explicitly linked to anything so it doesn't matter which debits we take from.
				List<FauxAccountEntry> listPosDebits=listDebits.FindAll(x => x.AmountEnd.IsGreaterThanZero() 
					&& x.ChargeType==PayPlanChargeType.Debit
					&& !x.Principal.IsZero());
				foreach(FauxAccountEntry positiveDebit in listPosDebits) {
					if(amtToRemove.IsGreaterThanOrEqualToZero()) {
						break;
					}
					decimal amtDebitRemaining=positiveDebit.AmountEnd;
					if(amtDebitRemaining.IsLessThanOrEqualToZero()) {
						continue;
					}
					decimal amt=Math.Min(Math.Abs(amtToRemove),amtDebitRemaining);
					positiveDebit.AmountEnd-=amt;
					amtToRemove+=amt;
				}
				#endregion
				#region Allocate to Procedures
				//This is where the ludicrous magic of payment plan adjustments takes place.
				//Feed the adjustment value back into entities that have FauxAccountEntry procedure credits attached.
				//If there are no faux credits attached to procedures then this value won't go anywhere and is just 'written off'.
				//We need to give value BACK to the procedure credits just in case there was an invalid pat/prov/clinic attached (for income transfers).
				foreach(FauxAccountEntry fauxProcEntry in listFauxEntries.FindAll(x => !x.IsAdjustment && x.AccountEntryProc!=null)) {
					if(amtToAllocate.IsGreaterThanOrEqualToZero()) {
						break;
					}
					decimal amtProcCanTake=fauxProcEntry.AccountEntryProc.AmountOriginal-fauxProcEntry.AccountEntryProc.AmountEnd;
					if(amtProcCanTake.IsLessThanOrEqualToZero()) {
						continue;
					}
					decimal amtToGiveBack=Math.Min(Math.Abs(amtToAllocate),amtProcCanTake);
					fauxProcEntry.AccountEntryProc.AmountEnd+=amtToGiveBack;
					//The overall value of this faux entry needs to be adjusted just in case there are other procedures / credits on the payment plan.
					//E.g. if this procedure was overcharged to begin with, it shouldn't give value back to the procedure AND continue to be worth full value.
					fauxProcEntry.PrincipalAdjusted-=amtToGiveBack;
					amtToAllocate+=amtToGiveBack;
				}
				#endregion
			}
			#endregion
			#region Adjustments (dynamic payment plans)
			//Loop through each faux credit adjustment and apply as many adjustment debits as possible.
			//That's right, dynamic payment plans explicitly dictate if a debit is designed for a procedure or an adjustment.
			foreach(FauxAccountEntry fauxAdjCredit in listFauxEntries.FindAll(x => x.IsAdjustment && x.IsDynamic)) {
				//It is safe to use AmountEnd because Principal was the only thing used to populate it when this faux entry was created (no interest).
				List<FauxAccountEntry> listPosDebits=listDebits.FindAll(x => x.AmountEnd.IsGreaterThanZero()
					&& x.ChargeType==PayPlanChargeType.Debit
					&& x.IsAdjustment
					&& !x.Principal.IsZero());
				foreach(FauxAccountEntry positiveDebit in listPosDebits) {
					if(fauxAdjCredit.AmountEnd.IsLessThanOrEqualToZero()) {
						break;
					}
					decimal amtDebitRemaining=positiveDebit.AmountEnd;
					if(amtDebitRemaining.IsLessThanOrEqualToZero()) {
						continue;
					}
					decimal amtToAllocate=Math.Min(fauxAdjCredit.AmountEnd,amtDebitRemaining);
					positiveDebit.AmountEnd-=amtToAllocate;
					fauxAdjCredit.AmountEnd-=amtToAllocate;
					listAllocatedDebits.Add(GetAllocatedDebit(amtToAllocate,fauxAdjCredit,positiveDebit));
				}
			}
			#endregion
			#region Credits (non-adjustments)
			//Loop through each faux credit and apply as many debits as possible.
			List<FauxAccountEntry> listFauxNonAdjCredits=listFauxEntries.FindAll(x => !x.IsAdjustment);
			//Never allow FauxAccountEntries associated to TP procedures to get value.
			listFauxNonAdjCredits.RemoveAll(x => x.AccountEntryProc!=null && ((Procedure)x.AccountEntryProc.Tag).ProcStatus==ProcStat.TP);
			foreach(FauxAccountEntry fauxNonAdjCredit in listFauxNonAdjCredits) {
				//Use PrincipalAdjusted instead of AmountEnd (which could include interest) or Principal (has not been adjusted).
				decimal amtCreditRemaining=fauxNonAdjCredit.PrincipalAdjusted;
				List<FauxAccountEntry> listPosDebits=listDebits.FindAll(x => x.AmountEnd.IsGreaterThanZero()
					&& x.ChargeType==PayPlanChargeType.Debit
					&& !x.Principal.IsZero());
				foreach(FauxAccountEntry positiveDebit in listPosDebits) {
					if(amtCreditRemaining.IsLessThanOrEqualToZero()) {
						break;
					}
					decimal amtDebitRemaining=positiveDebit.AmountEnd;
					if(amtDebitRemaining.IsLessThanOrEqualToZero()) {
						continue;
					}
					decimal amtToAllocate=Math.Min(amtCreditRemaining,amtDebitRemaining);
					positiveDebit.AmountEnd-=amtToAllocate;
					amtCreditRemaining-=amtToAllocate;
					listAllocatedDebits.Add(GetAllocatedDebit(amtToAllocate,fauxNonAdjCredit,positiveDebit));
				}
			}
			#endregion
			return listAllocatedDebits;
		}

		private static FauxAccountEntry GetAllocatedDebit(decimal amtToAllocate,FauxAccountEntry fauxCredit,FauxAccountEntry fauxDebit) {
			//Create a new faux account entry from the debit but only for the amount that to allocate.
			FauxAccountEntry allocatedDebit=fauxDebit.Copy();
			allocatedDebit.AccountEntryProc=fauxCredit.AccountEntryProc;
			allocatedDebit.AmountEnd=amtToAllocate;
			allocatedDebit.Principal=amtToAllocate;
			allocatedDebit.PrincipalAdjusted=amtToAllocate;
			//Change specific information on this allocated debit to match the faux credit passed in.
			allocatedDebit.PatNum=fauxCredit.PatNum;
			allocatedDebit.ProvNum=fauxCredit.ProvNum;
			allocatedDebit.ClinicNum=fauxCredit.ClinicNum;
			allocatedDebit.AdjNum=fauxCredit.AdjNum;
			allocatedDebit.ProcNum=fauxCredit.ProcNum;
			allocatedDebit.Guarantor=fauxCredit.Guarantor;
			allocatedDebit.IsAdjustment=fauxCredit.IsAdjustment;
			return allocatedDebit;
		}

		///<summary>Returns a list of AccountEntries with manipulated amounts due to entities that are explicitly linked to them.
		///An explicit link is a match between an entity itself (procedure, adjustment, etc) along with matching patient, provider, and clinic.</summary>
		private static List<AccountEntry> ExplicitlyLinkCredits(ConstructResults contructResults,List<PaySplit> listSplitsCurrentAndHistoric) {
			//No remoting role check; no call to db and private method
			List<AccountEntry> listExplicitAccountCharges=contructResults.ListAccountCharges
				.FindAll(x => x.GetType().In(typeof(Procedure),typeof(FauxAccountEntry),typeof(Adjustment)));
			//Create a dictionary that can easily find a corresponding AccountEntry for a specific PaySplit.
			//Old logic did not consider the fact that the same PaySplit could be in multiple AccountEntries so maybe that scenario isn't possible.
			Dictionary<string,AccountEntry> dictPaySplitAccountEntries=new Dictionary<string,AccountEntry>();
			foreach(AccountEntry splitEntry in contructResults.ListAccountCharges.FindAll(x => x.GetType()==typeof(PaySplit))) {
				foreach(PaySplit paySplit in splitEntry.SplitCollection) {
					dictPaySplitAccountEntries[(string)paySplit.TagOD]=splitEntry;
				}
			}
			#region Adjustments (payment plans / procedures)
			Dictionary<long,List<AccountEntry>> dictProcNumEntries=listExplicitAccountCharges.GroupBy(x => x.ProcNum)
				.ToDictionary(x => x.Key,x => x.ToList());
			#region Payment Plans
			foreach(long procNum in dictProcNumEntries.Keys) {
				//Adjustments can be associated to procedures that are associated to payment plans.
				//Value should be removed from positive adjustments that have already been considered within the principal of payment plan credits.
				if(dictProcNumEntries[procNum].Count(x => x.GetType()==typeof(FauxAccountEntry))==0
					|| dictProcNumEntries[procNum].Count(x => x.GetType()==typeof(Adjustment))==0
					|| dictProcNumEntries[procNum].Count(x => x.GetType()==typeof(Procedure))==0)
				{
					continue;//No payment plan entries, adjustments, or procedures.  Nothing to do in this loop which requires all three be present.
				}
				//Find the procedure that all of these entries are associated to in order to get access to the ProcFee.
				AccountEntry procAccountEntry=dictProcNumEntries[procNum].First(x => x.GetType()==typeof(Procedure));
				//Sum the principal for the faux entries and subtract the ProcFee from that value.
				//Any amount remaining can be directly removed from the adjustment so it's not double counted.
				decimal principalTotal=dictProcNumEntries[procNum].Where(x => x.GetType()==typeof(FauxAccountEntry))
					.Cast<FauxAccountEntry>()
					.Sum(x => x.Principal);
				decimal amountRemaining=(principalTotal-procAccountEntry.AmountOriginal);
				//Find all of the adjustments that are directly associated to this pat/prov/clinic combo.
				List<AccountEntry> listAdjAccountEntries=dictProcNumEntries[procNum].FindAll(x => x.GetType()==typeof(Adjustment)
					&& x.PatNum==procAccountEntry.PatNum
					&& x.ProvNum==procAccountEntry.ProvNum
					&& x.ClinicNum==procAccountEntry.ClinicNum);
				foreach(AccountEntry adjAccountEntry in listAdjAccountEntries) {
					if(amountRemaining.IsLessThanOrEqualToZero()) {
						break;
					}
					decimal amountToRemove=Math.Min(adjAccountEntry.AmountEnd,amountRemaining);
					adjAccountEntry.AmountEnd-=amountToRemove;
					amountRemaining-=amountToRemove;
				}
			}
			#endregion
			#region Procedures
			foreach(AccountEntry procAdjEntry in listExplicitAccountCharges.FindAll(x => x.GetType()==typeof(Adjustment) && x.ProcNum > 0)) {
				//There should be one account entry for the procedure.  Directly manipulate the AmountEnd for matching procedure.
				//Always remove the entire amount of the negative adjustment (even if the procedure is sent into the negative).
				//This is so that we do not accidentally implicitly pay off anything associated to the same pat/prov/clinic later (if implicit linking).
				AccountEntry procEntry=listExplicitAccountCharges.FirstOrDefault(x => x.GetType()==typeof(Procedure)
					&& x.ProcNum==procAdjEntry.ProcNum
					&& x.PatNum==procAdjEntry.PatNum
					&& x.ProvNum==procAdjEntry.ProvNum
					&& x.ClinicNum==procAdjEntry.ClinicNum);
				if(procEntry==null) {
					continue;
				}
				procEntry.AmountEnd+=procAdjEntry.AmountEnd;
				procAdjEntry.AmountEnd=0;
			}
			#endregion
			#endregion
			//Group up all current and historical splits by Pat/Prov/Clinic for explicit linking.
			var dictPatProvClinicSplits=listSplitsCurrentAndHistoric.Where(x => !x.SplitAmt.IsZero())
				.GroupBy(x => new { x.PatNum,x.ProvNum,x.ClinicNum })
				.ToDictionary(x => x.Key,x => x.ToList());
			foreach(var kvpPatProvClinicSplits in dictPatProvClinicSplits) {
				List<PaySplit> listPatProvClinicSplits=kvpPatProvClinicSplits.Value;
				//Get a subset of account entries that can be explicitly linked to these splits.
				List<AccountEntry> listPatProvClinicAccountCharges=listExplicitAccountCharges.FindAll(x => x.PatNum==kvpPatProvClinicSplits.Key.PatNum
					&& x.ProvNum==kvpPatProvClinicSplits.Key.ProvNum
					&& x.ClinicNum==kvpPatProvClinicSplits.Key.ClinicNum);
				//Prefer explicit links to procedures, faux account entries, and then adjustments.
				//This is because splits can be vicariously attached to adjustments via the procedure but the split should prefer the procedure first.
				List<AccountEntry> listProcEntries=listPatProvClinicAccountCharges.FindAll(x => x.GetType()==typeof(Procedure));
				List<AccountEntry> listPayPlanEntries=listPatProvClinicAccountCharges.FindAll(x => x.GetType()==typeof(FauxAccountEntry));
				List<AccountEntry> listAdjEntries=listPatProvClinicAccountCharges.FindAll(x => x.GetType()==typeof(Adjustment));
				//NOTE: Any explicitly linked paysplit needs to be used on what it's attached to in its entirety (even if it's overpaid).
				#region Procedures
				foreach(AccountEntry procEntry in listProcEntries) {
					foreach(PaySplit procSplit in listPatProvClinicSplits.FindAll(x => x.ProcNum==procEntry.ProcNum && x.PayPlanNum==0)) {
						decimal splitAmt=(decimal)procSplit.SplitAmt;//Overpayment on procedures is handled later
						procEntry.AmountEnd-=splitAmt;
						procEntry.SplitCollection.Add(procSplit.Copy());//take copy so we can get amtPaid without overwriting.
						procSplit.SplitAmt-=(double)splitAmt;
						if(dictPaySplitAccountEntries.TryGetValue((string)procSplit.TagOD,out AccountEntry splitEntry)) {
							splitEntry.AmountEnd+=splitAmt;
						}
					}
				}
				#endregion
				#region FauxAccountEntry
				//Explicitly attach all PayPlanCharge splits first. Then the payment plan splits will be attached to anything that is left over.
				foreach(AccountEntry payPlanChargeEntry in listPayPlanEntries) {
					List<PaySplit> listPayPlanChargeSplits=listPatProvClinicSplits.FindAll(x => x.SplitAmt.IsGreaterThanZero()
						&& x.PayPlanNum==payPlanChargeEntry.PayPlanNum
						&& x.PayPlanChargeNum==payPlanChargeEntry.PayPlanChargeNum);
					foreach(PaySplit payPlanChargeSplit in listPayPlanChargeSplits) {
						decimal splitAmt=Math.Min((decimal)payPlanChargeSplit.SplitAmt,payPlanChargeEntry.AmountEnd);
						payPlanChargeEntry.AmountEnd-=splitAmt;
						payPlanChargeEntry.SplitCollection.Add(payPlanChargeSplit.Copy());//take copy so we can get amtPaid without overwriting.
						payPlanChargeSplit.SplitAmt-=(double)splitAmt;
						if(dictPaySplitAccountEntries.TryGetValue((string)payPlanChargeSplit.TagOD,out AccountEntry splitEntry)) {
							splitEntry.AmountEnd+=splitAmt;
						}
					}
				}
				//Do the same thing over but this time do it on a payment plan level (old splits won't always be explicitly linked to a PayPlanCharge).
				foreach(AccountEntry payPlanChargeEntry in listPayPlanEntries.Where(x => x.AmountEnd.IsGreaterThanZero())) {
					List<PaySplit> listPayPlanSplits=listPatProvClinicSplits.FindAll(x => x.SplitAmt.IsGreaterThanZero()
						&& x.PayPlanNum==payPlanChargeEntry.PayPlanNum);
					foreach(PaySplit payPlanSplit in listPayPlanSplits) {
						decimal splitAmt=Math.Min((decimal)payPlanSplit.SplitAmt,payPlanChargeEntry.AmountEnd);
						if(splitAmt.IsZero()) {
							break;
						}
						payPlanChargeEntry.AmountEnd-=splitAmt;
						payPlanChargeEntry.SplitCollection.Add(payPlanSplit.Copy());//take copy so we can get amtPaid without overwriting.
						payPlanSplit.SplitAmt-=(double)splitAmt;
						if(dictPaySplitAccountEntries.TryGetValue((string)payPlanSplit.TagOD,out AccountEntry splitEntry)) {
							splitEntry.AmountEnd+=splitAmt;
						}
					}
				}
				#endregion
				#region Adjustment
				foreach(AccountEntry adjEntry in listAdjEntries) {
					List<PaySplit> listAdjSplits=listPatProvClinicSplits.FindAll(x => !x.SplitAmt.IsZero()
						&& (x.AdjNum==adjEntry.AdjNum || (x.ProcNum!=0 && x.ProcNum==((Adjustment)adjEntry.Tag).ProcNum))
						&& x.PayPlanNum==0);
					foreach(PaySplit adjSplit in listAdjSplits) {
						decimal splitAmt=(decimal)adjSplit.SplitAmt;//Overpayment on procedures is handled later
						adjEntry.AmountEnd-=splitAmt;
						adjEntry.SplitCollection.Add(adjSplit.Copy());//take copy so we can get amtPaid without overwriting.
						adjSplit.SplitAmt-=(double)splitAmt;
						if(dictPaySplitAccountEntries.TryGetValue((string)adjSplit.TagOD,out AccountEntry splitEntry)) {
							splitEntry.AmountEnd+=splitAmt;
						}
					}
				}
				#endregion
			}
			return ExplicitlyLinkUnearnedTogether(contructResults.ListAccountCharges);
		}

		///<summary>This method applies positive and negative unearned to each other so the sum is correct before making any transfers.
		///This method assumes that explicit linking has been done before it is called so the unallocated splits no longer have any value (if any)
		///Without this method when a procedure, postive unearned, and negative unearned are on an account the postive would be applied to the procedure,
		///leaving a negative on the account. We want the positive and negative to cancel each other out before any transfers are made.</summary>
		private static List<AccountEntry> ExplicitlyLinkUnearnedTogether(List<AccountEntry> listAccountCharges) {
			//No remoting role check; no call to db and private method
			List<AccountEntry> listUnearned=listAccountCharges.FindAll(x => x.IsUnearned);
			//Prefer to link unearned that is attached to the same payment plan together first.
			Dictionary<long,List<AccountEntry>> dictPayPlanEntries=listUnearned.GroupBy(x => x.PayPlanNum).ToDictionary(x => x.Key,x => x.ToList());
			foreach(long payPlanNum in dictPayPlanEntries.Keys) {
				List<AccountEntry> listPositiveUnearnedPP=dictPayPlanEntries[payPlanNum].FindAll(x => x.AmountEnd.IsGreaterThanZero());
				List<AccountEntry> listNegativeUnearnedPP=dictPayPlanEntries[payPlanNum].FindAll(x => x.AmountEnd.IsLessThanZero());
				ExplicitlyLinkPositiveNegativeEntries(ref listPositiveUnearnedPP,ref listNegativeUnearnedPP);
			}
			//After both regular and payment plan unearned splits have been considered separately, lump them all together.
			List<AccountEntry> listPositiveUnearned=listUnearned.FindAll(x => x.AmountEnd.IsGreaterThanZero());
			List<AccountEntry> listNegativeUnearned=listUnearned.FindAll(x => x.AmountEnd.IsLessThanZero());
			ExplicitlyLinkPositiveNegativeEntries(ref listPositiveUnearned,ref listNegativeUnearned);
			return listAccountCharges;
		}

		private static void ExplicitlyLinkPositiveNegativeEntries(ref List<AccountEntry> listPositiveEntries,ref List<AccountEntry> listNegativeEntries) {
			//No remoting role check; no call to db and private method
			foreach(AccountEntry positiveEntry in listPositiveEntries) {
				foreach(AccountEntry negativeEntry in listNegativeEntries) {
					if(positiveEntry.AmountEnd.IsLessThanOrEqualToZero()) {
						continue;//no more money to apply.
					}
					if(positiveEntry.ProvNum!=negativeEntry.ProvNum
						|| positiveEntry.PatNum!=negativeEntry.PatNum
						|| positiveEntry.ClinicNum!=negativeEntry.ClinicNum
						|| positiveEntry.UnearnedType!=negativeEntry.UnearnedType)
					{
						continue;
					}
					decimal amount=Math.Min(Math.Abs(positiveEntry.AmountEnd),Math.Abs(negativeEntry.AmountEnd));
					positiveEntry.AmountEnd-=amount;
					negativeEntry.AmountEnd+=amount;
				}
			}
		}

		///<summary>Attempts to implicitly link old unattached payments to past production that has not explicitly has payments attached to them.
		///This will give the patient a better idea on what they need to pay off next.
		///It is important to invoke ExplicitlyLinkCredits() prior to this method.</summary>
		///<param name="listPaySplits">All payment splits for the family.</param>
		///<param name="listInsPayAsTotal">All claimprocs paid as total for the family, might contain ins payplan payments.
		///Adds claimprocs for the completed procedures if in income xfer mode.</param>
		///<param name="listAccountCharges">All account entries generated by ConstructListCharges() and ExplicitlyLinkCredits().
		///Can include account charges for the family.</param>
		///<param name="listSplitsCur">All splits associated to payCur.  Empty list for a new payment.</param>
		///<param name="listPayFirstAcctEntries">All account entries that payCur should be linked to first.
		///If payCur.PayAmt is greater than the sum of these account entries then any leftover amount will be implicitly linked to other entries.</param>
		///<param name="payCur">The payment that is wanting to be linked to other account entries.  Could have entites linked already.</param>
		///<param name="patNum">The PatNum of the currently selected patient.</param>
		///<param name="isPatPrefer">Set to true if account entries for patNum should be prioritized before other entries.</param>
		///<returns>A helper class that represents the implicit credits that this method made.</returns>
		private static PayResults ImplicitlyLinkCredits(List<PaySplit> listPaySplits,List<PayAsTotal> listInsPayAsTotal,
			List<AccountEntry> listAccountCharges,List<PaySplit> listSplitsCur,List<AccountEntry> listPayFirstAcctEntries,Payment payCur,long patNum,
			bool isPatPrefer)
		{
			//No remoting role check; no call to db and private method
			if(isPatPrefer) {
				//Shove all account entries associated to patNum passed in to the bottom of the list so that they are implicitly linked to last.
				listAccountCharges=listAccountCharges.OrderByDescending(x => x.PatNum!=patNum).ThenBy(x => x.Date).ToList();
			}
			if(!listPayFirstAcctEntries.IsNullOrEmpty()) {//User has specific procs/payplancharges/adjustments selected prior to entering the Payment window.  
				//They wish these be paid by this payment specifically.
				//To accomplish this, we need to auto-split to the selected procedures prior to implicit linking.
				foreach(AccountEntry entry in listPayFirstAcctEntries) {
					if(payCur.PayAmt<=0) {
						break;//Will be empty
					}
					if(entry.GetType()==typeof(PayPlanCharge)) {
						//Handle payment plan splits in a special way. Continue to the next entry after we add to the list. 
						listSplitsCur.AddRange(CreatePaySplitsForPayPlanCharge(payCur,(PayPlanCharge)entry.Tag,listAccountCharges));
						continue;
					}
					AccountEntry charge=listAccountCharges.Find(x => x.PriKey==entry.PriKey);
					if(charge==null) {
						continue;//likely only for the event of selecting payplan line items when the plan is closed and on version 2. 
					}
					PaySplit split=new PaySplit();
					if(charge.GetType()==typeof(Procedure)) {
						split.ProcNum=charge.PriKey;
					}
					else if(charge.GetType()==typeof(Adjustment) && ((Adjustment)charge.Tag).ProcNum==0) {
						//should already be verified to have no procedure and positive amount
						split.AdjNum=charge.PriKey;
					}
					if(PrefC.HasClinicsEnabled) {//Clinics
						split.ClinicNum=charge.ClinicNum;
					}
					double amt=Math.Min((double)charge.AmountEnd,payCur.PayAmt);
					payCur.PayAmt=Math.Round(payCur.PayAmt-amt,3);
					split.SplitAmt=amt;
					charge.AmountEnd-=(decimal)amt;
					split.DatePay=DateTime.Today;
					split.PatNum=charge.PatNum;
					split.ProvNum=charge.ProvNum;
					split.PayNum=payCur.PayNum;
					split.PayPlanNum=charge.PayPlanNum;
					split.PayPlanChargeNum=charge.PayPlanChargeNum;
					charge.SplitCollection.Add(split);
					listSplitsCur.Add(split);
				}
			}
			//Make a deep copy of all splits because the SplitAmt will get directly manipulated within implicit linking processing.
			List<PaySplit> listSplitsCopied=listPaySplits.Select(x => x.Copy()).ToList();
			//Create a list of account entries that ignore TP procs as they should never be implicitly paid.
			List<AccountEntry> listImplicitCharges=new List<AccountEntry>(listAccountCharges);
			//Never auto split to treatment planned entries
			listImplicitCharges.RemoveAll(x => x.GetType()==typeof(Procedure) && ((Procedure)x.Tag).ProcStatus==ProcStat.TP);
			//Patient payment plans can have credits attached to treatment planned procedures, ignore those as well.
			listImplicitCharges.RemoveAll(x => x.GetType()==typeof(FauxAccountEntry)
				&& ((FauxAccountEntry)x.Tag).AccountEntryProc!=null
				&& ((FauxAccountEntry)x.Tag).AccountEntryProc.GetType()==typeof(Procedure)
				&& ((Procedure)((FauxAccountEntry)x.Tag).AccountEntryProc.Tag).ProcStatus==ProcStat.TP);
			foreach(AccountBalancingLayers layer in Enum.GetValues(typeof(AccountBalancingLayers))) {
				if(layer==AccountBalancingLayers.Unearned) {
					continue;
				}
				List<ImplicitLinkBucket> listBuckets=CreateImplicitLinkBucketsForLayer(layer,listInsPayAsTotal,listSplitsCopied,listImplicitCharges);
				foreach(ImplicitLinkBucket bucket in listBuckets) {
					ProcessImplicitLinkBucket(bucket);
				}
			}
			//At this point implicit linking has been performed as accurately as possible (in regards to the bucket system).
			//Now is the point in the process where ANY negative account entry can be applied towards ANY positive entry FIFO style.
			//E.g. Some chain of events could have taken place to cause a procedure to get overpaid (AmountEnd of -$20).
			//This negative production needs to be applied to another piece of production that has a positive value.
			//Ergo, it can be applied towards another procedure with an AmountEnd of $40 (just as long as it is positive).
			BalanceAccountEntries(ref listImplicitCharges);
			if(isPatPrefer) {
				//The list of account entries were sorted at the beginning of this method to make charges associated to the patNum passed in 'unpaid'.
				//Shove all account entries associated to patNum to the top of the list so that calling methods prefer these entries first.
				listAccountCharges=listAccountCharges.OrderBy(x => x.PatNum!=patNum).ThenBy(x => x.Date).ToList();
			}
			PayResults implicitCredits=new PayResults();
			implicitCredits.ListAccountCharges=listAccountCharges;
			implicitCredits.ListSplitsCur=listSplitsCur;
			implicitCredits.Payment=payCur;
			return implicitCredits;
		}

		private static List<PaySplit> CreatePaySplitsForPayPlanCharge(Payment payCur,PayPlanCharge payPlanCharge,List<AccountEntry> listAccountEntries) {
			List<PaySplit> listSplitsToAdd=new List<PaySplit>();
			//Find all of the account entries that are associated to the PayPlanCharge.
			//There can be multiple; one for principal and one for interest.
			List<FauxAccountEntry> listFauxAccountEntries=listAccountEntries.Where(x => x.GetType()==typeof(FauxAccountEntry)
					&& x.AmountEnd.IsGreaterThanZero()
					&& x.PayPlanChargeNum==payPlanCharge.PayPlanChargeNum)
				.Cast<FauxAccountEntry>()
				.OrderBy(x => x.Interest.IsGreaterThanZero())//Pay interest first.
				.ToList();
			foreach(FauxAccountEntry fauxAccountEntry in listFauxAccountEntries) {
				if(payCur.PayAmt.IsLessThanOrEqualToZero()) {
					break;
				}
				double amountToSplit=Math.Min((double)fauxAccountEntry.AmountEnd,payCur.PayAmt);
				PaySplit paySplit=new PaySplit();
				paySplit.AdjNum=fauxAccountEntry.AdjNum;
				paySplit.DatePay=DateTime.Today;
				//the split should always go to the payplancharge's guarantor.
				paySplit.PatNum=payPlanCharge.Guarantor;
				paySplit.ProcNum=fauxAccountEntry.ProcNum;
				paySplit.ProvNum=fauxAccountEntry.ProvNum;
				if(PrefC.HasClinicsEnabled) {//Clinics
					paySplit.ClinicNum=fauxAccountEntry.ClinicNum;
				}
				paySplit.PayPlanNum=fauxAccountEntry.PayPlanNum;
				paySplit.PayPlanChargeNum=fauxAccountEntry.PayPlanChargeNum;
				paySplit.PayNum=payCur.PayNum;
				paySplit.SplitAmt=amountToSplit;
				fauxAccountEntry.AmountEnd-=(decimal)amountToSplit;
				fauxAccountEntry.SplitCollection.Add(paySplit);
				listSplitsToAdd.Add(paySplit);
				payCur.PayAmt-=amountToSplit;
			}
			return listSplitsToAdd;
		}

		///<summary>Groups up the account entries passed in into buckets based on the layer passed in.</summary>
		private static List<ImplicitLinkBucket> CreateImplicitLinkBucketsForLayer(AccountBalancingLayers layer,List<PayAsTotal> listInsPayAsTotal,
			List<PaySplit> listPaySplits,List<AccountEntry> listAccountEntries)
		{
			//No remoting role check; private method
			switch(layer) {
				case AccountBalancingLayers.ProvPatClinic:
					List<ImplicitLinkBucket> listProvPatClinicBuckets=listAccountEntries.GroupBy(x => new { x.ProvNum,x.PatNum,x.ClinicNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new ImplicitLinkBucket(x.Value))
						.ToList();
					foreach(ImplicitLinkBucket bucketProvPatClinic in listProvPatClinicBuckets) {
						long patNum=bucketProvPatClinic.ListAccountEntries.First().PatNum;
						long provNum=bucketProvPatClinic.ListAccountEntries.First().ProvNum;
						long clinicNum=bucketProvPatClinic.ListAccountEntries.First().ClinicNum;
						bucketProvPatClinic.ListInsPayAsTotal=listInsPayAsTotal.FindAll(x => x.PatNum==patNum && x.ProvNum==provNum && x.ClinicNum==clinicNum);
						bucketProvPatClinic.ListPaySplits=listPaySplits.FindAll(x => x.PatNum==patNum && x.ProvNum==provNum && x.ClinicNum==clinicNum);
					}
					return listProvPatClinicBuckets;
				case AccountBalancingLayers.ProvPat:
					List<ImplicitLinkBucket> listProvPatBuckets=listAccountEntries.GroupBy(x => new { x.ProvNum,x.PatNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new ImplicitLinkBucket(x.Value))
						.ToList();
					foreach(ImplicitLinkBucket bucketProvPat in listProvPatBuckets) {
						long patNum=bucketProvPat.ListAccountEntries.First().PatNum;
						long provNum=bucketProvPat.ListAccountEntries.First().ProvNum;
						bucketProvPat.ListInsPayAsTotal=listInsPayAsTotal.FindAll(x => x.PatNum==patNum && x.ProvNum==provNum);
						bucketProvPat.ListPaySplits=listPaySplits.FindAll(x => x.PatNum==patNum && x.ProvNum==provNum);
					}
					return listProvPatBuckets;
				case AccountBalancingLayers.ProvClinic:
					List<ImplicitLinkBucket> listProvClinicBuckets=listAccountEntries.GroupBy(x => new { x.ProvNum,x.ClinicNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new ImplicitLinkBucket(x.Value))
						.ToList();
					foreach(ImplicitLinkBucket bucketProvClinic in listProvClinicBuckets) {
						long provNum=bucketProvClinic.ListAccountEntries.First().ProvNum;
						long clinicNum=bucketProvClinic.ListAccountEntries.First().ClinicNum;
						bucketProvClinic.ListInsPayAsTotal=listInsPayAsTotal.FindAll(x => x.ProvNum==provNum && x.ClinicNum==clinicNum);
						bucketProvClinic.ListPaySplits=listPaySplits.FindAll(x => x.ProvNum==provNum && x.ClinicNum==clinicNum);
					}
					return listProvClinicBuckets;
				case AccountBalancingLayers.PatClinic:
					List<ImplicitLinkBucket> listPatClinicBuckets=listAccountEntries.GroupBy(x => new { x.PatNum,x.ClinicNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new ImplicitLinkBucket(x.Value))
						.ToList();
					foreach(ImplicitLinkBucket bucketPatClinic in listPatClinicBuckets) {
						long patNum=bucketPatClinic.ListAccountEntries.First().PatNum;
						long clinicNum=bucketPatClinic.ListAccountEntries.First().ClinicNum;
						bucketPatClinic.ListInsPayAsTotal=listInsPayAsTotal.FindAll(x => x.PatNum==patNum && x.ClinicNum==clinicNum);
						bucketPatClinic.ListPaySplits=listPaySplits.FindAll(x => x.PatNum==patNum && x.ClinicNum==clinicNum);
					}
					return listPatClinicBuckets;
				case AccountBalancingLayers.Prov:
					List<ImplicitLinkBucket> listProvBuckets=listAccountEntries.GroupBy(x => new { x.ProvNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new ImplicitLinkBucket(x.Value))
						.ToList();
					foreach(ImplicitLinkBucket bucketProv in listProvBuckets) {
						long provNum=bucketProv.ListAccountEntries.First().ProvNum;
						bucketProv.ListInsPayAsTotal=listInsPayAsTotal.FindAll(x => x.ProvNum==provNum);
						bucketProv.ListPaySplits=listPaySplits.FindAll(x => x.ProvNum==provNum);
					}
					return listProvBuckets;
				case AccountBalancingLayers.Pat:
					List<ImplicitLinkBucket> listPatBuckets=listAccountEntries.GroupBy(x => new { x.PatNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new ImplicitLinkBucket(x.Value))
						.ToList();
					foreach(ImplicitLinkBucket bucketPat in listPatBuckets) {
						long patNum=bucketPat.ListAccountEntries.First().PatNum;
						bucketPat.ListInsPayAsTotal=listInsPayAsTotal.FindAll(x => x.PatNum==patNum);
						bucketPat.ListPaySplits=listPaySplits.FindAll(x => x.PatNum==patNum);
					}
					return listPatBuckets;
				case AccountBalancingLayers.Clinic:
					List<ImplicitLinkBucket> listClinicBuckets=listAccountEntries.GroupBy(x => new { x.ClinicNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new ImplicitLinkBucket(x.Value))
						.ToList();
					foreach(ImplicitLinkBucket bucketClinic in listClinicBuckets) {
						long clinicNum=bucketClinic.ListAccountEntries.First().ClinicNum;
						bucketClinic.ListInsPayAsTotal=listInsPayAsTotal.FindAll(x => x.ClinicNum==clinicNum);
						bucketClinic.ListPaySplits=listPaySplits.FindAll(x => x.ClinicNum==clinicNum);
					}
					return listClinicBuckets;
				case AccountBalancingLayers.Nothing:
					//Create a single bucket to hold all entities:
					ImplicitLinkBucket bucket=new ImplicitLinkBucket(listAccountEntries);
					bucket.ListInsPayAsTotal=listInsPayAsTotal;
					bucket.ListPaySplits=listPaySplits;
					return new List<ImplicitLinkBucket>() {
						bucket
					};
				case AccountBalancingLayers.Unearned:
				default:
					throw new ODException($"Income transfer buckets cannot be created for unsupported layer: {layer}");
			}
		}

		///<summary></summary>
		public static void ProcessImplicitLinkBucket(ImplicitLinkBucket bucket) {
			//No remoting role check; no call to db
			#region PayAsTotal
			foreach(PayAsTotal payAsTotal in bucket.ListInsPayAsTotal) {//Use claim payments by total to pay off procedures for that specific patient.
				if(payAsTotal.SummedInsPayAmt==0 && payAsTotal.SummedWriteOff==0) {
					continue;
				}
				foreach(AccountEntry accountEntry in bucket.ListAccountEntries) {
					if(payAsTotal.SummedInsPayAmt==0 && payAsTotal.SummedWriteOff==0) {
						break;
					}
					if(accountEntry.AmountEnd==0) {
						continue;
					}
					if(accountEntry.GetType().In(typeof(PayPlanCharge),typeof(FauxAccountEntry))) {
						continue;
					}
					double amt=Math.Min((double)accountEntry.AmountEnd,payAsTotal.SummedInsPayAmt);
					accountEntry.AmountEnd-=(decimal)amt;
					payAsTotal.SummedInsPayAmt-=amt;
					double amtWriteOff=Math.Min((double)accountEntry.AmountEnd,payAsTotal.SummedWriteOff);
					accountEntry.AmountEnd-=(decimal)amtWriteOff;
					payAsTotal.SummedWriteOff-=amtWriteOff;
				}
			}
			#endregion
			#region PaySplits
			List<long> listHiddenUnearnedDefNums=Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType)
				.FindAll(x => !string.IsNullOrEmpty(x.ItemValue))//If ItemValue is not blank, it means "do not show on account"
				.Select(x => x.DefNum).ToList();
			List<PaySplit> listLinkableSplits=bucket.ListPaySplits.FindAll(x => !x.UnearnedType.In(listHiddenUnearnedDefNums));
			List<PaySplit> listLinkablePosSplits=listLinkableSplits.FindAll(x => x.SplitAmt.IsGreaterThanZero());
			List<PaySplit> listLinkableNegSplits=listLinkableSplits.FindAll(x => x.SplitAmt.IsLessThanZero());
			#region Payment Plans
			foreach(PaySplit split in listLinkablePosSplits.FindAll(x => x.PayPlanNum > 0)) {
				foreach(AccountEntry accountEntry in bucket.ListAccountEntries.FindAll(x => x.PayPlanNum==split.PayPlanNum)) {
					if(split.SplitAmt.IsZero()) {
						break;//Split's amount has been used by previous charges.
					}
					if(accountEntry.AmountEnd.IsZero()) {
						continue;
					}
					if(accountEntry.GetType()==typeof(Procedure) && ((Procedure)accountEntry.Tag).ProcStatus==ProcStat.TP) {
						continue;//we do not implicitly link to TP procedures
					}
					double amt=Math.Min((double)accountEntry.AmountEnd,split.SplitAmt);
					accountEntry.AmountEnd-=(decimal)amt;
					accountEntry.SplitCollection.Add(split.Copy());
					split.SplitAmt-=amt;
				}
			}
			#endregion
			#region Non-Payment Plans
			//Loop through any negative pay splits and offset their value with any other positive pay split within this bucket.
			foreach(PaySplit positiveSplit in listLinkablePosSplits.FindAll(x => x.PayPlanNum==0)) {
				if(positiveSplit.SplitAmt.IsLessThanOrEqualToZero()) {
					continue;
				}
				foreach(PaySplit negativeSplit in listLinkableNegSplits.FindAll(x => x.PayPlanNum==0)) {
					if(positiveSplit.SplitAmt.IsLessThanOrEqualToZero()) {
						break;
					}
					if(negativeSplit.SplitAmt.IsGreaterThanOrEqualToZero()) {
						continue;
					}
					double amountTxfr=Math.Min(Math.Abs(positiveSplit.SplitAmt),Math.Abs(negativeSplit.SplitAmt));
					positiveSplit.SplitAmt-=amountTxfr;
					negativeSplit.SplitAmt+=amountTxfr;
				}
			}
			//Distribute any splits that still have a positive SplitAmt remaining (after negating the negative splits above).
			foreach(PaySplit split in listLinkablePosSplits.FindAll(x => x.PayPlanNum==0)) {
				if(split.SplitAmt.IsLessThanOrEqualToZero()) {
					continue;
				}
				foreach(AccountEntry accountEntry in bucket.ListAccountEntries.FindAll(x => x.PayPlanNum==0)) {
					if(split.SplitAmt.IsLessThanOrEqualToZero()) {
						break;//Split's amount has been used by previous charges.
					}
					if(accountEntry.AmountEnd.IsLessThanOrEqualToZero()) {
						continue;
					}
					if(accountEntry.GetType()==typeof(Procedure) && ((Procedure)accountEntry.Tag).ProcStatus==ProcStat.TP) {
						continue;//we do not implicitly link to TP procedures
					}
					double amt=Math.Min((double)accountEntry.AmountEnd,split.SplitAmt);
					accountEntry.AmountEnd-=(decimal)amt;
					accountEntry.SplitCollection.Add(split.Copy());
					split.SplitAmt-=amt;
				}
			}
			//Negative non-procedure adjustments can implicitly link to negative paysplits.
			//Negative non-procedure adjustments are basically bookkeeping errors, courtesy discounts, or some sort of donation to the patient.
			//Negative paysplits are money going from the doctor/office back to the patient for similar reasons (usually done to correct errors).
			//It is completely acceptable to have these donations/corrections offset each other.
			List<AccountEntry> listNegNonPayPlanAdjEntries=bucket.ListAccountEntries.FindAll(x => x.GetType()==typeof(Adjustment)
				&& x.ProcNum==0
				&& x.PayPlanNum==0
				&& x.AmountEnd.IsLessThanZero());
			foreach(PaySplit split in listLinkableNegSplits.FindAll(x => x.PayPlanNum==0)) {
				if(split.SplitAmt.IsGreaterThanOrEqualToZero()) {
					continue;
				}
				foreach(AccountEntry accountEntry in listNegNonPayPlanAdjEntries) {
					if(split.SplitAmt.IsGreaterThanOrEqualToZero()) {
						break;//Split amount has been used by previous charges.
					}
					if(accountEntry.AmountEnd.IsGreaterThanOrEqualToZero()) {
						continue;
					}
					double amt=Math.Max((double)accountEntry.AmountEnd,split.SplitAmt);
					accountEntry.AmountEnd-=(decimal)amt;
					accountEntry.SplitCollection.Add(split.Copy());
					split.SplitAmt-=amt;
				}
			}
			#endregion
			#endregion
			#region Adjustments
			//Negative non-procedure adjustments need to remove value from positive procedures as accurately as possible.
			List<AccountEntry> listNegAdjEntries=bucket.ListAccountEntries.FindAll(x => x.GetType()==typeof(Adjustment) 
				&& x.ProcNum==0
				&& x.AmountEnd.IsLessThanZero());
			List<AccountEntry> listPosProcEntries=bucket.ListAccountEntries.FindAll(x => x.GetType()==typeof(Procedure)
				&& x.AmountEnd.IsGreaterThanZero());
			List<AccountEntry> listAdjProcEntries=new List<AccountEntry>();
			listAdjProcEntries.AddRange(listNegAdjEntries);
			listAdjProcEntries.AddRange(listPosProcEntries);
			BalanceAccountEntries(ref listAdjProcEntries);
			#region Payment Plan Adjustments
			//Negative non-procedure faux account entries (pay plan adjustments) need to remove value from positive faux account entries FIFO style.
			//Only consider ones that are not associated to an unearned type.  Those faux entries are designed for the transfer system, not linking system.
			List<AccountEntry> listNegAdjFauxEntries=bucket.ListAccountEntries.FindAll(x => x.GetType()==typeof(FauxAccountEntry)
				&& ((FauxAccountEntry)x.Tag).IsAdjustment
				&& x.AmountEnd.IsLessThanZero()
				&& !x.IsUnearned);
			List<AccountEntry> listPosFauxEntries=bucket.ListAccountEntries.FindAll(x => x.GetType()==typeof(FauxAccountEntry)
				&& x.AmountEnd.IsGreaterThanZero()
				&& !x.IsUnearned);
			List<AccountEntry> listNegAdjPosFauxEntries=new List<AccountEntry>();
			listNegAdjPosFauxEntries.AddRange(listNegAdjFauxEntries);
			listNegAdjPosFauxEntries.AddRange(listPosFauxEntries);
			BalanceAccountEntries(ref listNegAdjPosFauxEntries);
			#endregion
			#endregion
		}

		/// <summary>Attaches information into the LoadData.DictPatToDPFeeSchedule and LoadData.ListFeesForDiscountPlan variables. Requires
		/// a list of TP procedures to get fees for.</summary>
		private static void LoadDiscountPlanInfo(ref LoadData loadData,List<Procedure> listTpProcs) {
			//No remoting role check; private method and uses ref parameter.
			if(loadData==null) {
				loadData=new LoadData();
			}
			loadData.DictPatToDPFeeScheds=DiscountPlans.GetFeeSchedNumsByPatNums(listTpProcs.Select(x => x.PatNum).ToList());
			loadData.ListFeesForDiscountPlans=Fees.GetByFeeSchedNumsClinicNums(loadData.DictPatToDPFeeScheds.Select(x => x.Value).Distinct()
					.Where(x => x!=0).ToList(),listTpProcs.Select(x => x.ClinicNum).Union(new List<long> { 0 }).ToList())
				.Select(x => (Fee)x).ToList();
		}
		#endregion

		#region AllocateUnearned
		///<summary>This function takes a list of a patient's prepayments and first explicitly links prepayment then implicitly links them to the existing prepayments.</summary>
		public static List<PaySplits.PaySplitAssociated> AllocateUnearned(List<AccountEntry> listAccountEntries,ref List<PaySplit> listPaySplit,Payment payCur
			,double unearnedAmt,Family fam) 
		{
			//No remoting role check; Method that uses ref parameters.
			List<PaySplits.PaySplitAssociated> retVal=new List<PaySplits.PaySplitAssociated>();
			if(unearnedAmt.IsLessThan(0) || listAccountEntries==null) {
				return retVal;
			}
			//don't allocate prepayments that are attached to TP procedures or that are flagged as hidden on account. 
			List<PaySplit> listFamPrePaySplits=PaySplits.GetPrepayForFam(fam,doExcludeTpPrepay:true);
			//Manipulate the original prepayments SplitAmt by subtracting counteracting SplitAmt.
			foreach(PaySplit prePaySplit in listFamPrePaySplits.FindAll(x => x.SplitAmt.IsGreaterThan(0))) {
				List<PaySplit> listSplitsForPrePay=PaySplits.GetSplitsForPrepay(new List<PaySplit>() { prePaySplit });//Find all splits for the pre-payment.
				foreach(PaySplit splitForPrePay in listSplitsForPrePay) {
					prePaySplit.SplitAmt+=splitForPrePay.SplitAmt;//Sum the amount of the pre-payment that's used. (balancing splits are negative usually)
				}
			}
			//We will try our best to automatically link any prepayments that don't have an explicit counteracting paysplit.
			//After all prepayments have been "linked", manipulate unearnedAmt accordingly.
			ImplicitlyLinkPrepayments(ref listFamPrePaySplits,ref unearnedAmt);
			//Get all claimprocs and adjustments for the proc;
			List<Procedure> listProcs=PaymentEdit.GetProcsForAccountEntries(listAccountEntries);
			List<ClaimProc> listClaimProcs=ClaimProcs.GetForProcs(listProcs.Select(x => x.ProcNum).Distinct().ToList());
			List<Adjustment> listAdjusts=Adjustments.GetForProcs(listProcs.Select(x => x.ProcNum).Distinct().ToList());
			foreach(Procedure proc in listProcs) {//For each proc see how much of the Unearned we use per proc
				if(unearnedAmt<=0) {
					break;
				}
				//Calculate the amount remaining on the procedure so we can know how much of the remaining pre-payment amount we can use.
				proc.ProcFee-=PaySplits.GetPaySplitsFromProc(proc.ProcNum).Sum(x => x.SplitAmt);//Figure out how much is due on this proc.
				double patPortion=(double)ClaimProcs.GetPatPortion(proc,listClaimProcs,listAdjusts);
				foreach(PaySplit prePaySplit in listFamPrePaySplits) {
					//First we need to decide how much of each pre-payment split we can use per proc.
					if(patPortion<=0) {//Proc has been paid for, go to next proc
						break;
					}
					if(prePaySplit.SplitAmt<=0) {//Split has been used, go to next split
						continue;
					}
					if(patPortion<=0) {
						break;
					}
					decimal splitTotal=0;
					if(splitTotal<(decimal)prePaySplit.SplitAmt) {//If the sum indicates there's pre-payment amount left over, let's use it.
						double amtToUse=0;
						if(prePaySplit.SplitAmt<patPortion) {
							amtToUse=prePaySplit.SplitAmt;
						}
						else {
							amtToUse=patPortion;
						}
						unearnedAmt-=amtToUse;//Reflect the new unearned amount available for future proc use.
						PaySplit splitNeg=new PaySplit();
						splitNeg.PatNum=prePaySplit.PatNum;
						splitNeg.PayNum=payCur.PayNum;
						splitNeg.FSplitNum=prePaySplit.SplitNum;
						splitNeg.ClinicNum=prePaySplit.ClinicNum;
						splitNeg.ProvNum=prePaySplit.ProvNum;
						splitNeg.SplitAmt=0-amtToUse;
						splitNeg.UnearnedType=prePaySplit.UnearnedType;
						splitNeg.DatePay=DateTime.Now;
						listPaySplit.Add(splitNeg);
						//Make a different paysplit attached to proc and prov they want to use it for.
						PaySplit splitPos=new PaySplit();
						splitPos.PatNum=proc.PatNum;//Use procedure's pat to allocate to that patient's account even when viewing entire family.
						splitPos.PayNum=payCur.PayNum;
						splitPos.FSplitNum=0;//The association will be done on form closing.
						splitPos.ProvNum=proc.ProvNum;
						splitPos.ClinicNum=proc.ClinicNum;
						splitPos.SplitAmt=amtToUse;
						splitPos.DatePay=DateTime.Now;
						splitPos.ProcNum=proc.ProcNum;
						listPaySplit.Add(splitPos);
						//link negSplit to posSplit. 
						retVal.Add(new PaySplits.PaySplitAssociated(splitNeg,splitPos));
						//link original prepayment to neg split.
						PaySplit paySplitPrePayOrig=PaySplits.GetOne(prePaySplit.SplitNum);
						retVal.Add(new PaySplits.PaySplitAssociated(paySplitPrePayOrig,splitNeg));
						prePaySplit.SplitAmt-=amtToUse;
						patPortion-=amtToUse;
					}
				}
			}
			return retVal;
		}

		///<summary>This function takes a list of a patient's prepayments and implicitly links them to unlinked paysplits.  Lists are passed in by reference
		///because the lists are used multiple times.</summary>
		private static void ImplicitlyLinkPrepayments(ref List<PaySplit> listFamPrePaySplits, ref double unearnedAmt) {
			//No remoting role check; private method that uses ref parameters.
			List<PaySplit> listPosPrePay=listFamPrePaySplits.FindAll(x => x.SplitAmt.IsGreaterThan(0));
			List<PaySplit> listNegPrePay=listFamPrePaySplits.FindAll(x => x.SplitAmt.IsLessThan(0));
			//Logic check PatNum - match, ProvNum - match, ClinicNum - match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			//Logic check PatNum - match, ProvNum - match, ClinicNum - zero
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumZero:true);
			//Logic check PatNum - match, ProvNum - match, ClinicNum - non zero & non match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNonZeroNonMatch:true);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumZero:true,isClinicNumMatch:true);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - zero
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumZero:true,isClinicNumZero:true);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - non zero & non match 
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumZero:true,isClinicNonZeroNonMatch:true);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - match 
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNumMatch:true);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - zero
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNumZero:true);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - non zero & non match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNonZeroNonMatch:true);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - zero
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNumZero:true);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - non zero & non match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNonZeroNonMatch:true);
			//Logic check PatNum - other family members, ProvNum - zero, ClinicNum - match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNumMatch:true);
			//Logic check PatNum - other family members, ProvNum - zero, ClinicNum - zero
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNumZero:false);
			//Logic checkPatNum - other family members, ProvNum - zero, ClinicNum - non zero & non match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNonZeroNonMatch:true);
			//Logic checkPatNum - other family members, ProvNum - non zero & non match, ClinicNum - match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNumMatch:true);
			//Logic check PatNum - other family members, ProvNum - non zero & non match, ClinicNum - zero
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNumZero:true);
			//Logic check PatNum - other family members, ProvNum - non zero & non match, ClinicNum - non zero & non match
			unearnedAmt=ImplicitlyLinkPrepaymentsHelper(listPosPrePay,listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNonZeroNonMatch:true);
		}

		/// <summary> Helpler method to allocate unearned implicitly. Returns amount remaining to be allocated after implicitly linking.</summary>
		public static double ImplicitlyLinkPrepaymentsHelper(List<PaySplit> listPosPrePay,List<PaySplit> listNegPrePay,double unearnedAmt,bool isPatMatch=false
			,bool isProvNumMatch=false,bool isClinicNumMatch=false,bool isFamMatch=false,bool isProvNumZero=false,bool isClinicNumZero=false
			,bool isProvNonZeroNonMatch=false,bool isClinicNonZeroNonMatch=false) 
		{
			//No remoting role check; no call to db.
			if(unearnedAmt.IsLessThan(0) || unearnedAmt.IsZero()){
				return 0;
			}
			//Manipulate the amounts (never letting them go into the negative) and then use whatever is left to represent the unearned amount.
			foreach(PaySplit posSplit in listPosPrePay) {
				if(posSplit.SplitAmt.IsEqual(0)) {
					continue;
				}
				//Find all negative paysplits with the filters
				List<PaySplit> filteredNegSplit=listNegPrePay
					.Where(x => !isPatMatch               || x.PatNum==posSplit.PatNum)
					.Where(x => !isFamMatch               || x.PatNum!=posSplit.PatNum)
					.Where(x => !isProvNumMatch           || x.ProvNum==posSplit.ProvNum)
					.Where(x => !isProvNumZero            || x.ProvNum==0)
					.Where(x => !isProvNonZeroNonMatch    || (x.ProvNum!=0 && x.ProvNum!=posSplit.ProvNum))
					.Where(x => !isClinicNumMatch         || x.ClinicNum==posSplit.ClinicNum)
					.Where(x => !isClinicNumZero          || x.ClinicNum==0)
					.Where(x => !isClinicNonZeroNonMatch  || (x.ClinicNum!=0 && x.ClinicNum!=posSplit.ClinicNum))
					.ToList();
				foreach(PaySplit negSplit in filteredNegSplit) {
					if(negSplit.SplitAmt.IsEqual(0)) {
						continue;
					}
					//Deduct split amount from the positive split for the amount of the negative split,or the postive split depending on which is smaller according to the absolute value. 
					double amt=Math.Min(posSplit.SplitAmt,Math.Abs(negSplit.SplitAmt));
					negSplit.SplitAmt+=amt;
					posSplit.SplitAmt-=amt;
				}
			}
			return listPosPrePay.Sum(x => x.SplitAmt);
		}
		#endregion

		#region Misc
		///<summary>Distributes available unearned money to the account entries up to the amount passed in.</summary>
		public static List<PaySplit> AllocateUnearned(long payNum,double amountUnearned,List<AccountEntry> listAccountEntries,Family fam=null) {
			//No need to check RemotingRole; no call to db.
			if(amountUnearned.IsLessThanOrEqualToZero() || listAccountEntries.IsNullOrEmpty()) {
				return new List<PaySplit>();
			}
			if(fam==null) {
				fam=Patients.GetFamily(listAccountEntries.First().PatNum);
			}
			List<PaySplit> listPaySplits=new List<PaySplit>();
			double amountRemaining=amountUnearned;
			//Perform explicit linking on the entire account and get the actual account entries that make up the current unearned bucket.
			ConstructResults constructResults=ConstructAndLinkChargeCredits(fam.GetPatNums(),fam.Guarantor.PatNum,new List<PaySplit>(),new Payment(),
				new List<AccountEntry>(),isIncomeTxfr:true);
			//The account entries passed in may not have had explicit linking executed on them so find the same entries from our results.
			List<AccountEntry> listAllocateEntries=new List<AccountEntry>();
			for(int i=0;i<listAccountEntries.Count;i++) {
				AccountEntry allocateEntry=constructResults.ListAccountCharges.FirstOrDefault(x => x.IsSameTag(listAccountEntries[i]));
				if(allocateEntry!=null) {
					listAllocateEntries.Add(allocateEntry);
				}
			}
			//Suggest splits that explicitly take from the unearned bucket first. Any value left over will get transferred from the 0 provider later.
			foreach(AccountEntry unearnedEntry in constructResults.ListAccountCharges.FindAll(x => x.IsUnearned && x.AmountEnd.IsLessThanOrEqualToZero())) {
				if(amountRemaining.IsLessThanOrEqualToZero()) {
					break;
				}
				foreach(AccountEntry accountEntry in listAllocateEntries.FindAll(x => x.AmountEnd.IsGreaterThanZero())) {
					if(amountRemaining.IsLessThanOrEqualToZero() || unearnedEntry.AmountEnd.IsGreaterThanOrEqualToZero()) {
						break;
					}
					double amountToAllocate=Math.Min((double)Math.Abs(unearnedEntry.AmountEnd),(double)accountEntry.AmountEnd);
					amountToAllocate=Math.Min(amountToAllocate,amountRemaining);
					//Make a split that will offset a legitimate unearned account entry.
					listPaySplits.Add(new PaySplit() {
						AdjNum=unearnedEntry.AdjNum,
						ClinicNum=unearnedEntry.ClinicNum,
						DatePay=DateTimeOD.Today,
						PatNum=unearnedEntry.PatNum,
						PayPlanNum=unearnedEntry.PayPlanNum,
						PayNum=payNum,
						ProcNum=unearnedEntry.ProcNum,
						ProvNum=unearnedEntry.ProvNum,
						SplitAmt=0-amountToAllocate,
						UnearnedType=unearnedEntry.UnearnedType,
					});
					//Blindly apply the amount that was taken from unearned to one of the account entries that the user selected.
					listPaySplits.Add(new PaySplit() {
						AdjNum=accountEntry.AdjNum,
						ClinicNum=accountEntry.ClinicNum,
						DatePay=DateTimeOD.Today,
						PatNum=accountEntry.PatNum,
						PayPlanNum=accountEntry.PayPlanNum,
						PayNum=payNum,
						ProcNum=accountEntry.ProcNum,
						ProvNum=accountEntry.ProvNum,
						SplitAmt=amountToAllocate,
						UnearnedType=0,
					});
					unearnedEntry.AmountEnd+=(decimal)amountToAllocate;
					accountEntry.AmountEnd-=(decimal)amountToAllocate;
					amountRemaining-=amountToAllocate;
				}
			}
			//Get the default unearned types and make as many splits as necessary in order to move amountRemaining from the 0 provider.
			long unearnedTypePrepayment=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			long unearnedTypeTP=Prefs.GetLong(PrefName.TpUnearnedType);
			foreach(AccountEntry accountEntry in listAllocateEntries) {
				if(amountRemaining.IsLessThanOrEqualToZero()) {
					break;
				}
				if(accountEntry.AmountEnd.IsLessThanOrEqualToZero()) {
					continue;
				}
				long unearnedType=unearnedTypePrepayment;
				if(accountEntry.GetType()==typeof(Procedure) && ((Procedure)accountEntry.Tag).ProcStatus==ProcStat.TP) {
					unearnedType=unearnedTypeTP;
				}
				double amountToAllocate=Math.Min(amountRemaining,(double)accountEntry.AmountEnd);
				//Always take from the default unearned payment type and the 0 / 'None' provider. The income transfer system will correct this later.
				//They simply want to see a singlular negative entry (or as few as possible) and an offsetting positive to wherever they chose.
				listPaySplits.Add(new PaySplit() {
					AdjNum=0,
					ClinicNum=0,
					DatePay=DateTimeOD.Today,
					PatNum=accountEntry.PatNum,
					PayPlanNum=0,
					PayNum=payNum,
					ProcNum=0,
					ProvNum=0,
					SplitAmt=0-amountToAllocate,
					UnearnedType=unearnedType,
				});
				//Blindly apply the amount that was taken from unearned to one of the account entries that the user selected.
				listPaySplits.Add(new PaySplit() {
					AdjNum=accountEntry.AdjNum,
					ClinicNum=accountEntry.ClinicNum,
					DatePay=DateTimeOD.Today,
					PatNum=accountEntry.PatNum,
					PayPlanNum=accountEntry.PayPlanNum,
					PayNum=payNum,
					ProcNum=accountEntry.ProcNum,
					ProvNum=accountEntry.ProvNum,
					SplitAmt=amountToAllocate,
					UnearnedType=0,
				});
				amountRemaining-=amountToAllocate;
				accountEntry.AmountEnd-=(decimal)amountToAllocate;
			}
			return listPaySplits;
		}

		///<summary>Makes a payment from a passed in list of charges.</summary>
		public static PayResults MakePayment(List<List<AccountEntry>> listSelectedCharges,Payment payCur,decimal textAmount,
			List<AccountEntry> listAllCharges)
		{
			//No remoting role check; no call to db.
			PayResults splitData=null;
			List<PaySplit> listPaySplits=new List<PaySplit>();
			bool isPayAmtZeroUponEntering=payCur.PayAmt.IsZero();
			foreach(List<AccountEntry> listCharges in listSelectedCharges) {
				if(!isPayAmtZeroUponEntering && payCur.PayAmt.IsZero()) {
					break;
				}
				foreach(AccountEntry charge in listCharges.FindAll(x => !x.AmountEnd.IsZero())) {
					decimal splitAmt=(isPayAmtZeroUponEntering ? charge.AmountEnd : (decimal)payCur.PayAmt);
					if(!isPayAmtZeroUponEntering && splitAmt.IsLessThanOrEqualToZero()) {
						break;
					}
					splitData=CreatePaySplit(charge,splitAmt,payCur,textAmount,listAllCharges);
					listPaySplits.AddRange(splitData.ListSplitsCur);
					listAllCharges=splitData.ListAccountCharges;
					payCur=splitData.Payment;
				}
			}
			if(splitData==null) {
				splitData=new PayResults { ListAccountCharges=listAllCharges,ListSplitsCur=listPaySplits,Payment=payCur };
			}
			else {
				splitData.ListSplitsCur=listPaySplits;
			}
			return splitData;
		}

		public static PayResults CreatePaySplit(AccountEntry charge,decimal payAmt,Payment payCur,decimal textAmount,List<AccountEntry> listCharges,
			bool isManual=false) 
		{
			//No remoting role check; no call to db.
			PayResults createdSplit=new PayResults();
			createdSplit.ListSplitsCur=new List<PaySplit>();
			createdSplit.ListAccountCharges=listCharges;
			createdSplit.Payment=payCur;
			PaySplit split=new PaySplit();
			split.DatePay=DateTime.Today;
			split.PayNum=payCur.PayNum;
			split.PatNum=charge.PatNum;
			split.ProvNum=charge.ProvNum;
			split.PayPlanChargeNum=charge.PayPlanChargeNum;
			split.PayPlanNum=charge.PayPlanNum;
			split.ClinicNum=charge.ClinicNum;
			split.ProcNum=charge.ProcNum;
			split.AdjNum=charge.AdjNum;
			split.UnearnedType=charge.UnearnedType;
			//PaySplits for TP procedures should always set the UnearnedType to the TpUnearnedType preference.
			if(charge.GetType()==typeof(Procedure) && ((Procedure)charge.Tag).ProcStatus==ProcStat.TP) {
				split.UnearnedType=Prefs.GetLong(PrefName.TpUnearnedType);
			}
			if(!isManual && (Math.Abs(charge.AmountEnd)<Math.Abs(payAmt) || textAmount==0)) {
				//Not a manual charge and user wants to make a split for the full charge amount.
				split.SplitAmt=(double)charge.AmountEnd;
				charge.AmountEnd=0;
			}
			else {//Either a manual charge or a partial payment.
				split.SplitAmt=(double)payAmt;
				charge.AmountEnd-=payAmt;
			}
			payCur.PayAmt-=split.SplitAmt;
			charge.SplitCollection.Add(split);
			createdSplit.ListSplitsCur.Add(split);
			createdSplit.Payment=payCur;
			return createdSplit;
		}

		///<summary>Checks if the amtEntered will result in the AccountEntry.Tag procedure to be overpaid. Returns false if AccountEntry.Tag is not a procedure.</summary>
		public static bool IsProcOverPaid(decimal amtEntered,AccountEntry accountEntry) {
			if(accountEntry.GetType()!=typeof(Procedure)) {
				return false;//AccountEntry is not a procedure. Return false.
			}
			//Only look for explicitly linked paysplits when calculating amount remaining. 
			decimal amtOverpay=accountEntry.AmountOriginal-(decimal)accountEntry.SplitCollection.Sum(x=>x.SplitAmt)-amtEntered;
			return amtOverpay.IsLessThanZero();
		}

		///<summary>Returns true if there are any positive PaySplit account entries that are associated to unallocated or unearned 
		///and then sets warningMessage to a translated message that is ready to display to the user. Will not return true if unallocate and unearned
		///offset each other.  E.g. -$30 in unearned and $30 in unallocated will offset which leaves no reason to warn the user.
		///Otherwise, returns false. Optionally pass in a list of account entries if an income transfer has already been performed.</summary>
		public static bool IsUnallocatedOrUnearnedNegative(long patNum,Family family,out string warningMessage,
			List<AccountEntry> listAccountEntries=null)
		{
			//No remoting role check; no call to db and out parameter
			if(listAccountEntries==null) {
				ConstructResults constructResults=ConstructAndLinkChargeCredits(family.GetPatNums(),patNum,new List<PaySplit>(),new Payment(),
					new List<AccountEntry>(),isIncomeTxfr:true);
				listAccountEntries=constructResults.ListAccountCharges;
			}
			warningMessage="";
			List<AccountEntry> listUnassociatedSplits=listAccountEntries.FindAll(x => x.IsUnallocated || x.IsUnearned);
			Dictionary<long,List<AccountEntry>> dictProvSplits=listUnassociatedSplits.GroupBy(x => x.ProvNum).ToDictionary(x => x.Key,x => x.ToList());
			//Never warn users about the 0 / 'None' provider because the Allocate Unearned tool explicitly suggests making that provider negative.
			if(dictProvSplits.ContainsKey(0)) {
				dictProvSplits.Remove(0);
			}
			if(!dictProvSplits.Any(x => x.Value.Sum(y => y.AmountEnd).IsGreaterThanZero())) {
				return false;
			}
			//At this point we know that there is a provider that has a negative balance within Unallocated or Unearned.
			Dictionary<long,Patient> dictPatients=family.ListPats.GroupBy(x => x.PatNum).ToDictionary(x => x.Key,x => x.First());
			//Warn the user about all of the providers that have negative unearned and tell them to go manually fix them.
			StringBuilder strBuildWarning=new StringBuilder();
			string strUnallocated="Unallocated";
			foreach(long provNum in dictProvSplits.Keys) {
				if(dictProvSplits[provNum].Sum(y => y.AmountEnd).IsLessThanOrEqualToZero()) {
					continue;
				}
				strBuildWarning.AppendLine(provNum==0 ? "'None' Provider" : Providers.GetLongDesc(provNum));
				List<AccountEntry> listPosSplitEntries=dictProvSplits[provNum].FindAll(x => x.AmountEnd.IsGreaterThanZero())
					.OrderBy(x => x.PatNum)
					.ThenBy(x => x.Date)
					.ToList();
				foreach(AccountEntry posSplitEntry in listPosSplitEntries) {
					PaySplit paySplit=(PaySplit)posSplitEntry.Tag;
					if(!dictPatients.TryGetValue(paySplit.PatNum,out Patient patient)) {
						patient=Patients.GetLim(paySplit.PatNum);
						dictPatients[paySplit.PatNum]=patient;
					}
					strBuildWarning.AppendLine($"  {patient.GetNameLF()}"
						+$"  {paySplit.DatePay.ToShortDateString()}"
						+$"  {(paySplit.UnearnedType > 0 ? Defs.GetName(DefCat.PaySplitUnearnedType,paySplit.UnearnedType) : strUnallocated)}"
						+$"  {paySplit.SplitAmt:C}");
				}
			}
			strBuildWarning.AppendLine("To correct the balance for the family, the above payment splits need to be manually reallocated.");
			strBuildWarning.Append("See the manual for additional information.");
			warningMessage=strBuildWarning.ToString();
			return true;
		}

		///<summary>Finds and updates matching charge's amount end and adds the current paySplit to the split collection for the charge.
		///Updates the underlying data structures when a manual split is created or edited.</summary>
		public static void UpdateForManualSplit(PaySplit paySplit,List<AccountEntry> listAccountCharges,long prePaymentNum=0,bool isAllCharges=false) {
			//No remoting role check; no call to db
			//Find the charge row for this new split.
			//Locate a charge to apply the credit to, if a reasonable match exists.
			for(int i=0;i<listAccountCharges.Count;i++) {
				AccountEntry charge=listAccountCharges[i];
				if(charge.AmountEnd==0 && !isAllCharges) {
					continue;
				}
				bool isMatchFound=false;
				//New Split is for this proc (but not attached to a payplan)
				if(charge.GetType()==typeof(Procedure) && charge.PriKey==paySplit.ProcNum && paySplit.PayPlanNum==0) {
					isMatchFound=true;
				}
				else if(charge.GetType()==typeof(Adjustment) //New split is for this adjust
						&& paySplit.ProcNum==0 && paySplit.PayPlanNum==0 //Both being 0 is the only way we can tell it's for an Adj
						&& paySplit.DatePay==charge.Date
						&& paySplit.ProvNum==charge.ProvNum
						&& paySplit.PatNum==charge.PatNum
						&& paySplit.ClinicNum==charge.ClinicNum)
				{
					isMatchFound=true;
				}
				else if(charge.GetType()==typeof(PayPlanCharge) && charge.PriKey==paySplit.PayPlanChargeNum) {//split is for this payplancharge
					isMatchFound=true;
				}
				else if(charge.GetType()==typeof(PaySplit) && (charge.PriKey==prePaymentNum || charge.PriKey==paySplit.FSplitNum)) {//prepayment
					isMatchFound=true;
				}
				if(isMatchFound) {
					charge.AmountEnd=charge.AmountEnd-(decimal)paySplit.SplitAmt;
					charge.SplitCollection.Add(paySplit.Copy());
				}
				//If none of these, it's unattached to the best of our knowledge.
			}
		}
		#endregion

		#region AutoSplit
		/// <summary>Leave loadData blank for doRefreshData to be true and get a new copy of the objects.</summary>
		public static AutoSplit AutoSplitForPayment(List<long> listPatNums,long patCurNum,List<PaySplit> listSplitsCur,Payment payCur,
			List<AccountEntry> listPayFirstAcctEntries,bool isIncomeTxfr,bool isPatPrefer,LoadData loadData,bool doAutoSplit=true,
			bool doIncludeExplicitCreditsOnly=false)
		{
			ConstructResults constructResults=ConstructAndLinkChargeCredits(listPatNums,patCurNum,listSplitsCur,payCur
				,listPayFirstAcctEntries,isIncomeTxfr,isPatPrefer,loadData,doIncludeExplicitCreditsOnly);
			AutoSplit autoSplit=AutoSplitForPayment(constructResults,doAutoSplit);
			return autoSplit;
		}

		public static AutoSplit AutoSplitForPayment(ConstructResults constructResults,bool doAutoSplit=true) {
			AutoSplit autoSplitData=new AutoSplit();
			autoSplitData.ListAccountCharges=constructResults.ListAccountCharges;
			autoSplitData.ListSplitsCur=constructResults.ListSplitsCur;
			autoSplitData.Payment=constructResults.Payment;
			//Create Auto-splits for the current payment to any remaining non-zero charges FIFO by date.
			if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.DontEnforce) {
				return autoSplitData;
			}
			if(!doAutoSplit) {
				return autoSplitData;
			}
			//Get a subset of the account charges that can have value auto split to them.
			List<AccountEntry> listAutoSplitAccountEntries=autoSplitData.ListAccountCharges.FindAll(x => x.AmountEnd.IsGreaterThanZero());
			//Never auto split to treatment planned entries
			listAutoSplitAccountEntries.RemoveAll(x => x.GetType()==typeof(Procedure) && ((Procedure)x.Tag).ProcStatus==ProcStat.TP);
			//Patient payment plans can have credits attached to treatment planned procedures, ignore those as well.
			listAutoSplitAccountEntries.RemoveAll(x => x.GetType()==typeof(FauxAccountEntry) 
				&& ((FauxAccountEntry)x.Tag).AccountEntryProc!=null
				&& ((FauxAccountEntry)x.Tag).AccountEntryProc.GetType()==typeof(Procedure)
				&& ((Procedure)((FauxAccountEntry)x.Tag).AccountEntryProc.Tag).ProcStatus==ProcStat.TP);
			//Create a variable to keep track of the money that can be allocated for this payment.
			double amtToAllocate=autoSplitData.Payment.PayAmt;
			//Create as many auto splits as possible for account entries with positive AmountEnd values.
			foreach(AccountEntry charge in listAutoSplitAccountEntries) {
				if(amtToAllocate.IsZero()) {
					break;//No more value to allocate.
				}
				if(amtToAllocate.IsLessThanZero()) {
					return autoSplitData;//Negative payments should not make any auto splits so return here.
				}
				if(charge.AmountEnd.IsLessThanOrEqualToZero()) {
					continue;
				}
				double splitAmt=Math.Min(amtToAllocate,(double)charge.AmountEnd);
				//Make a new split that will apply as much value as possible from the account entry.
				PaySplit split=new PaySplit();
				split.IsNew=true;
				split.DatePay=autoSplitData.Payment.PayDate;
				split.PatNum=charge.PatNum;
				split.ProvNum=charge.ProvNum;
				split.ClinicNum=charge.ClinicNum;
				//A split should never be attached to both a procedure and an adjustment at the same time.
				//Therefore, the split being created for an adjustment account entry should ignore any attached procedure so that the adjustment is paid.
				split.ProcNum=(charge.GetType()==typeof(Adjustment) ? 0 : charge.ProcNum);
				split.PayPlanChargeNum=charge.PayPlanChargeNum;
				split.PayPlanNum=charge.PayPlanNum;
				split.AdjNum=charge.AdjNum;
				split.PayNum=autoSplitData.Payment.PayNum;
				split.UnearnedType=charge.UnearnedType;
				split.SplitAmt=splitAmt;
				//Remove the value from the account entry
				charge.AmountEnd-=(decimal)splitAmt;
				amtToAllocate-=splitAmt;
				charge.SplitCollection.Add(split);
				autoSplitData.ListAutoSplits.Add(split);
			}
			//Create an unearned split if there is any remaining money to allocate.
			if(!amtToAllocate.IsZero()) {
				PaySplit split=new PaySplit();
				split.SplitAmt=amtToAllocate;
				amtToAllocate=0;
				split.DatePay=autoSplitData.Payment.PayDate;
				split.PatNum=autoSplitData.Payment.PatNum;
				split.ProvNum=0;
				split.UnearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
				if(PrefC.HasClinicsEnabled) {
					split.ClinicNum=autoSplitData.Payment.ClinicNum;
				}
				split.PayNum=autoSplitData.Payment.PayNum;
				autoSplitData.ListAutoSplits.Add(split);
			}
			autoSplitData.Payment.PayAmt=amtToAllocate;
			return autoSplitData;
		}
		#endregion

		#region AccountEntry
		///<summary>Sorts similar to account module (AccountModules.cs, AccountLineComparer).Groups procedures together, then Adjustments, then everything else.</summary>
		private static int AccountEntrySort(AccountEntry x,AccountEntry y) {
			if(x.Date==y.Date) {
				if(x.GetType()==typeof(Procedure) && y.GetType()!=typeof(Procedure)) {
					return -1;
				}
				if(x.GetType()!=typeof(Procedure) && y.GetType()==typeof(Procedure)) {
					return 1;
				}
				if(x.GetType()==typeof(Adjustment) && y.GetType()!=typeof(Adjustment)) {
					return -1;
				}
				if(x.GetType()!=typeof(Adjustment) && y.GetType()==typeof(Adjustment)) {
					return 1;
				}
				if(x.GetType()==typeof(FauxAccountEntry) && y.GetType()==typeof(FauxAccountEntry)) {
					//PayPlanCharge entries should prefer to show interest charges above principal (we suggest paying interest first when auto-splitting).
					if(((FauxAccountEntry)x).Interest.IsGreaterThanZero() && ((FauxAccountEntry)y).Interest.IsZero()) {
						return -1;
					}
					if(((FauxAccountEntry)x).Interest.IsZero() && ((FauxAccountEntry)y).Interest.IsGreaterThanZero()) {
						return 1;
					}
					//If both have interest set then order predictably every time.
					if(((FauxAccountEntry)x).Interest.IsGreaterThanZero() && ((FauxAccountEntry)y).Interest.IsGreaterThanZero()) {
						return ((FauxAccountEntry)x).Interest.CompareTo(((FauxAccountEntry)y).Interest);
					}
					//Simply sort by principal due if no interest is being used.
					return ((FauxAccountEntry)x).Principal.CompareTo(((FauxAccountEntry)y).Principal);
				}
			}
			return x.Date.CompareTo(y.Date);
		}

		public static List<Procedure> GetProcsForAccountEntries(List<AccountEntry> listAccountEntry) {
			List<Procedure> listProcs=new List<Procedure>();
			List<AccountEntry> listAccountEntryProcs=listAccountEntry.FindAll(x => x.GetType()==typeof(Procedure));
			foreach(AccountEntry entry in listAccountEntryProcs) {
				listProcs.Add((Procedure)entry.Tag);
			}
			return listProcs;
		}

		public static List<AccountEntry> CreateAccountEntries(List<Procedure> listProcs) {
			//No remoting role check; no call to db
			List<AccountEntry> listAccountEntries=new List<AccountEntry>();
			foreach(Procedure proc in listProcs) {
				listAccountEntries.Add(new AccountEntry(proc));
			}
			return listAccountEntries;
		}
		#endregion

		#region Income Transfer

		///<summary>Throws exceptions.</summary>
		public static void TransferClaimsPayAsTotal(long patNum,List<long> listFamPatNums,string logText) {
			ClaimProcs.FixClaimsNoProcedures(listFamPatNums);
			if(!ProcedureCodes.GetContainsKey("ZZZFIX")) {
				Cache.Refresh(InvalidType.ProcCodes);//Refresh local cache only because middle tier has already inserted the signal.
			}
			ClaimTransferResult claimTransferResult=ClaimProcs.TransferClaimsAsTotalToProcedures(listFamPatNums);
			if(claimTransferResult!=null && claimTransferResult.ListInsertedClaimProcs.Count > 0) {//valid and items were created
				SecurityLogs.MakeLogEntry(Permissions.ClaimProcReceivedEdit,patNum,logText);
			}
		}

		///<summary></summary>
		public static bool TryCreateIncomeTransfer(List<AccountEntry> listAccountEntries,DateTime datePay,out IncomeTransferData incomeTransferData,
			long guarNum=0,List<PayPlan> listPayPlans=null)
		{
			//No remoting role check; no call to db
			incomeTransferData=new IncomeTransferData();
			if(listAccountEntries.IsNullOrEmpty()) {
				return true;
			}
			//Do not allow transfers if there is a payment plan associated to the family that is for an amount that does not equal the Tx amount.
			//E.g. A "Total Tx Amt" not equal to the "Total Amount" means the user is using a patient payment plan and didn't attach Tx Credits.
			//This is a requirement for the transfer system because it needs to know what to take value from and what to give it to (pat/prov/clinic).
			//Side note, dynamic payment plans can always be used in the imcome transfer manager.
			if(listPayPlans==null) {
				Family famCur=Patients.GetFamily(listAccountEntries.First().PatNum);
				listPayPlans=PayPlans.GetForPats(famCur.GetPatNums(),famCur.Guarantor.Guarantor).FindAll(x => !x.IsDynamic);
			}
			if(!listPayPlans.IsNullOrEmpty()) {
				//PayPlanCharge Credits are not made when the PaymentPlanVersion is set to NoCharges.
				//For now, do not allow income transfers to be made when the version is set to NoCharges because we don't know what has value to transfer.
				if(PrefC.GetEnum<PayPlanVersions>(PrefName.PayPlansVersion)==PayPlanVersions.NoCharges) {
					incomeTransferData.AppendLine("Transfers cannot be made while 'Pay Plan charge logic' is set to "+
						$"'{PayPlanVersions.NoCharges.GetDescription()}'.");
					return false;
				}
				List<long> listInvalidPayPlanNums=new List<long>();
				Dictionary<long,List<PayPlanCharge>> dictPayPlanCharges=PayPlanCharges.GetForPayPlans(listPayPlans.Select(x => x.PayPlanNum).ToList())
					.GroupBy(x => x.PayPlanNum)
					.ToDictionary(x => x.Key,x => x.ToList());
				foreach(long payPlanNum in dictPayPlanCharges.Keys) {
					//The total Principal of all credits must equate to the total Principal of all debits.
					double txTotalAmt=PayPlans.GetTxTotalAmt(dictPayPlanCharges[payPlanNum]);//credits
					double totalCost=PayPlans.GetTotalPrinc(payPlanNum,dictPayPlanCharges[payPlanNum]);//debits
					if(!txTotalAmt.IsEqual(totalCost)) {
						listInvalidPayPlanNums.Add(payPlanNum);
					}
				}
				if(listInvalidPayPlanNums.Count > 0) {
					List<PayPlan> listInvalidPayPlans=listPayPlans.FindAll(x => x.PayPlanNum.In(listInvalidPayPlanNums));
					List<long> listInvalidPatNums=listInvalidPayPlans.Select(x => x.PatNum).ToList();
					listInvalidPatNums.AddRange(listInvalidPayPlans.Select(x => x.Guarantor));
					Dictionary<long,Patient> dictPatients=Patients.GetLimForPats(listInvalidPatNums.Distinct().ToList()).ToDictionary(x => x.PatNum);
					//Notify the user that they cannot perform income transfers until they correct/delete payment plans that do not use Tx Credits correctly.
					StringBuilder stringBuilder=new StringBuilder();
					stringBuilder.AppendLine("Transfers cannot be made for this family at this time.");
					stringBuilder.AppendLine("The following payment plans have a 'Total Tx Amt' that does not match the 'Total Amount':");
					foreach(PayPlan payPlan in listInvalidPayPlans) {
						string ppType;
						if(payPlan.IsDynamic) {
							ppType="DPP";
						}
						else if(payPlan.PlanNum==0) {
							ppType="PP";
						}
						else {
							ppType="Ins";
						}
						string planCategory="None";
						if(payPlan.PlanCategory > 0) {
							planCategory=Defs.GetDef(DefCat.PayPlanCategories,payPlan.PlanCategory).ItemName;
						}
						double principal=PayPlans.GetTotalPrinc(payPlan.PayPlanNum,dictPayPlanCharges[payPlan.PayPlanNum]);
						stringBuilder.AppendLine($"Date: {payPlan.PayPlanDate.ToShortDateString()}");
						stringBuilder.AppendLine($"  Guarantor: {dictPatients[payPlan.Guarantor].GetNameLF()}");
						stringBuilder.AppendLine($"  Patient: {dictPatients[payPlan.PatNum].GetNameLF()}");
						stringBuilder.AppendLine($"  Type: {ppType}");
						stringBuilder.AppendLine($"  Category: {planCategory}");
						stringBuilder.AppendLine($"  Principal: {principal.ToString("C")}");
					}
					incomeTransferData.AppendLine(stringBuilder.ToString());
					return false;
				}
			}
			listAccountEntries.Sort(AccountEntrySort);
			#region Preprocess Unearned/Unallocated
			//Users can choose to make strange decisions like taking from a provider's unearned bucket to pay off a procedure for a different provider
			//even when the original provider has no unearned to take from (see unit tests associated to this commit).
			//These providers that were wrongly taken from need to be credited back so that the income transfer system can correctly balance the account.
			//Without this preprocessing, accounts could end up with a negative unearned bucket (rare, but easy to duplicate).
			List<AccountEntry> listUnearnedUnallocated=listAccountEntries.FindAll(x => x.IsUnearned || x.IsUnallocated);
			//Go through each bucket for all unearned and unallocated account entries to balance them out prior to looking at production.
			foreach(AccountBalancingLayers layer in Enum.GetValues(typeof(AccountBalancingLayers))) {
				if(layer==AccountBalancingLayers.Unearned) {
					continue;//Unearned is special and gets handled after this loop.
				}
				incomeTransferData.MergeIncomeTransferData(TransferForLayer(layer,guarNum,datePay,ref listUnearnedUnallocated));
			}
			listAccountEntries.AddRange(listUnearnedUnallocated.FindAll(y => !y.In(listAccountEntries)));
			#endregion
			#region Payment Plans
			//Find entries with PayPlanNums and perform income transfers for each individual payment plan.
			//Payment plan entries should prefer to transfer within themselves and any excess money (overpaid plan) should move to unearned.
			Dictionary<long,List<AccountEntry>> dictPayPlanEntries=listAccountEntries
				.Where(x => x!=null && x.Tag!=null && x.PayPlanNum > 0)
				.GroupBy(x => x.PayPlanNum)
				.ToDictionary(x => x.Key,x => x.ToList());
			List<AccountEntry> listUnearnedEntries=listAccountEntries.FindAll(x => x.GetType()==typeof(PaySplit)
				&& x.PayPlanNum==0
				&& x.IsUnearned
				&& x.AmountEnd.IsLessThanZero());
			List<AccountEntry> listOverpaidUnearnedEntries=new List<AccountEntry>();
			foreach(long payPlanNum in dictPayPlanEntries.Keys) {
				List<AccountEntry> listPayPlanEntries=dictPayPlanEntries[payPlanNum];
				//Always consider transferring unearned that is outside of the payment plan into the payment plan.
				listPayPlanEntries.AddRange(listUnearnedEntries);
				//Loop through all of the income transfer layers except the Unearned layer which will be handled manually afterwards.
				foreach(AccountBalancingLayers layer in Enum.GetValues(typeof(AccountBalancingLayers))) {
					if(layer==AccountBalancingLayers.Unearned) {
						continue;//Unearned is special and gets handled after this loop.
					}
					incomeTransferData.MergeIncomeTransferData(TransferForLayer(layer,guarNum,datePay,ref listPayPlanEntries));
				}
				//There is a posibility that this payment plan was overpaid.  The overpayment will have been transferred to unearned.
				//This money is now available to go towards other payment plans (if present).
				listOverpaidUnearnedEntries.AddRange(listPayPlanEntries.Where(x => x.GetType()==typeof(PaySplit)
					&& x.PayPlanNum==0
					&& x.IsUnearned
					&& x.AmountEnd.IsLessThanZero()
					&& !x.In(listUnearnedEntries)));
			}
			//Go through the list of payment plans again now that all overpayments have been detected. 
			//This allows overpayments from payment plans to flow into other payment plans.
			listUnearnedEntries.AddRange(listOverpaidUnearnedEntries);
			foreach(long payPlanNum in dictPayPlanEntries.Keys) {
				List<AccountEntry> listPayPlanEntries=dictPayPlanEntries[payPlanNum];
				//Always consider transferring unearned that is outside of the payment plan into the payment plan.
				listPayPlanEntries.AddRange(listUnearnedEntries);
				//Loop through all of the income transfer layers except the Unearned layer which will be handled manually afterwards.
				foreach(AccountBalancingLayers layer in Enum.GetValues(typeof(AccountBalancingLayers))) {
					if(layer==AccountBalancingLayers.Unearned) {
						continue;//Unearned is special and gets handled after this loop.
					}
					incomeTransferData.MergeIncomeTransferData(TransferForLayer(layer,guarNum,datePay,ref listPayPlanEntries));
				}
			}
			#endregion
			//Get all non-payment plan account entries along with any leftover unearned payment plan PaySplits that still have value (can be transferred).
			List<AccountEntry> listAccountEntriesNoPP=listAccountEntries.FindAll(x => x.PayPlanNum==0
				|| (x.GetType()==typeof(PaySplit) && x.AmountEnd.IsLessThanZero()));
			//Loop through all of the income transfer layers except the Unearned layer which will be handled manually afterwards.
			foreach(AccountBalancingLayers layer in Enum.GetValues(typeof(AccountBalancingLayers))) {
				if(layer==AccountBalancingLayers.Unearned) {
					continue;//Unearned is special and gets handled after this loop.
				}
				incomeTransferData.MergeIncomeTransferData(TransferForLayer(layer,guarNum,datePay,ref listAccountEntriesNoPP));
			}
			#region Unearned
			incomeTransferData.AppendLine($"Processing for layer: {AccountBalancingLayers.Unearned.ToString()}...");
			//Transfer all remaining excess production to unearned (but keep the same pat/prov/clinic, unless they're on rigorous accounting)
			//Only consider procedures and adjustments because payment plan entries should not be allowed to go into the negative.
			//The scenarios where that would be possible should have been blocked or transferred to unearned above.
			List<AccountEntry> listNegativeProduction=listAccountEntries.FindAll(x => x.GetType().In(typeof(Procedure),typeof(Adjustment))
				&& x.AmountEnd.IsLessThanZero());
			foreach(AccountEntry negativeProduction in listNegativeProduction) {
				if(negativeProduction.GetType()==typeof(Procedure) && ((Procedure)negativeProduction.Tag).ProcFee.IsLessThanZero()) {
					continue;//do not use negative procedures as a souce of income. 
				}
				incomeTransferData.AppendLine($"  Moving excess production for {negativeProduction.Description} to unearned.");
				negativeProduction.SplitCollection.Add(CreateUnearnedTransfer(negativeProduction,ref incomeTransferData,datePay)[0]);
				negativeProduction.AmountEnd+=Math.Abs(negativeProduction.AmountEnd);
			}
			//Transfer all remaining income to unearned (but keep the same pat/prov/clinic, unless they're on rigorous accounting)
			List<AccountEntry> listRemainingIncome=listAccountEntries.FindAll(x => x.GetType()==typeof(PaySplit) 
				&& !x.IsUnearned
				&& !x.AmountEnd.IsZero());
			#region Remaining Adjustments
			List<AccountEntry> listRemainingAdjIncome=listRemainingIncome.FindAll(x => ((PaySplit)x.Tag).AdjNum > 0);
			foreach(IncomeTransferBucket buckets in CreateTransferBucketsForLayer(AccountBalancingLayers.ProvPatClinic,listRemainingAdjIncome)) {
				foreach(var adjGroup in buckets.ListAccountEntries.GroupBy(x => x.AdjNum).ToDictionary(x => x.Key,x => x.ToList())) {
					decimal amountEndSum=adjGroup.Value.Sum(x => x.AmountEnd);
					if(amountEndSum.IsZero()) {
						continue;
					}
					AccountEntry entryFirst=adjGroup.Value.First();
					incomeTransferData.AppendLine($"  Moving excess income for AdjNum #{entryFirst.AdjNum} to unearned.");
					CreateUnearnedTransfer(amountEndSum,entryFirst.PatNum,entryFirst.ProvNum,entryFirst.ClinicNum,ref incomeTransferData,
						adjNum:entryFirst.AdjNum,payPlanNum:entryFirst.PayPlanNum,datePay:datePay);
					adjGroup.Value.ForEach(x => x.AmountEnd=0);
				}
			}
			#endregion
			#region Remaining Procedures
			List<AccountEntry> listRemainingProcIncome=listRemainingIncome.FindAll(x => ((PaySplit)x.Tag).ProcNum > 0);
			foreach(IncomeTransferBucket buckets in CreateTransferBucketsForLayer(AccountBalancingLayers.ProvPatClinic,listRemainingProcIncome)) {
				foreach(var procGroup in buckets.ListAccountEntries.GroupBy(x => x.ProcNum).ToDictionary(x => x.Key,x => x.ToList())) {
					decimal amountEndSum=procGroup.Value.Sum(x => x.AmountEnd);
					if(amountEndSum.IsZero()) {
						continue;
					}
					AccountEntry entryFirst=procGroup.Value.First();
					incomeTransferData.AppendLine($"  Moving excess income for ProcNum #{entryFirst.ProcNum} to unearned.");
					CreateUnearnedTransfer(amountEndSum,entryFirst.PatNum,entryFirst.ProvNum,entryFirst.ClinicNum,ref incomeTransferData,
						procNum:entryFirst.ProcNum,payPlanNum:entryFirst.PayPlanNum,datePay:datePay);
					procGroup.Value.ForEach(x => x.AmountEnd=0);
				}
			}
			#endregion
			#region Remaining Unallocated
			List<AccountEntry> listRemainingUnallocatedIncome=listAccountEntries.FindAll(x => x.IsUnallocated);
			foreach(IncomeTransferBucket buckets in CreateTransferBucketsForLayer(AccountBalancingLayers.ProvPatClinic,listRemainingUnallocatedIncome)) {
				decimal amountEndSum=buckets.ListAccountEntries.Sum(x => x.AmountEnd);
				if(amountEndSum.IsZero()) {
					continue;
				}
				AccountEntry entryFirst=buckets.ListAccountEntries.First();
				incomeTransferData.AppendLine($"  Moving excess unallocated income for PatNum #{entryFirst.PatNum}" +
					$", ProvNum #{entryFirst.ProvNum}, ClinicNum #{entryFirst.ClinicNum} to unearned.");
				CreateUnearnedTransfer(amountEndSum,entryFirst.PatNum,entryFirst.ProvNum,entryFirst.ClinicNum,ref incomeTransferData,
					payPlanNum:entryFirst.PayPlanNum,datePay:datePay);
				buckets.ListAccountEntries.ForEach(x => x.AmountEnd=0);
			}
			#endregion
			#endregion
			return true;
		}

		///<summary>Creates income transfer buckets out of the account entries passed in and then processes the buckets for the layer.
		///Preprocessing will be performed for the ProvPatClinic layer only.  See PreprocessProvPatClinicBuckets() for details.</summary>
		private static IncomeTransferData TransferForLayer(AccountBalancingLayers layer,long guarNum,DateTime datePay,
			ref List<AccountEntry> listAccountEntries)
		{
			//No remoting role check; private method
			IncomeTransferData incomeTransferData=new IncomeTransferData();
			List<IncomeTransferBucket> listBuckets=CreateTransferBucketsForLayer(layer,listAccountEntries);
			//Preprocess the production explicitly linked to procedures within each bucket on layer ProvPatClinic.
			if(layer==AccountBalancingLayers.ProvPatClinic) {
				PreprocessProvPatClinicBuckets(ref listBuckets,ref listAccountEntries,ref incomeTransferData,datePay);
			}
			incomeTransferData.AppendLine($"Processing buckets for layer: {layer.ToString()}...");
			//Process each bucket and make any necessary income transfers for this layer.
			//Create 'account entries' for any transfers that were created so that subsequent layers know about the transfers from previous layers.
			foreach(IncomeTransferBucket bucket in listBuckets) {
				incomeTransferData.MergeIncomeTransferData(TransferLoopHelper(bucket,guarNum,datePay));
			}
			return incomeTransferData;
		}

		///<summary>Groups up the account entries passed in into buckets based on the layer passed in.</summary>
		private static List<IncomeTransferBucket> CreateTransferBucketsForLayer(AccountBalancingLayers layer,List<AccountEntry> listAccountEntries) {
			//No remoting role check; private method
			switch(layer) {
				case AccountBalancingLayers.ProvPatClinic:
					return listAccountEntries.GroupBy(x => new { x.ProvNum,x.PatNum,x.ClinicNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new IncomeTransferBucket(x.Value))
						.ToList();
				case AccountBalancingLayers.ProvPat:
					return listAccountEntries.GroupBy(x => new { x.ProvNum,x.PatNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new IncomeTransferBucket(x.Value))
						.ToList();
				case AccountBalancingLayers.ProvClinic:
					return listAccountEntries.GroupBy(x => new { x.ProvNum,x.ClinicNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new IncomeTransferBucket(x.Value))
						.ToList();
				case AccountBalancingLayers.PatClinic:
					return listAccountEntries.GroupBy(x => new { x.PatNum,x.ClinicNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new IncomeTransferBucket(x.Value))
						.ToList();
				case AccountBalancingLayers.Prov:
					return listAccountEntries.GroupBy(x => new { x.ProvNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new IncomeTransferBucket(x.Value))
						.ToList();
				case AccountBalancingLayers.Pat:
					return listAccountEntries.GroupBy(x => new { x.PatNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new IncomeTransferBucket(x.Value))
						.ToList();
				case AccountBalancingLayers.Clinic:
					return listAccountEntries.GroupBy(x => new { x.ClinicNum })
						.ToDictionary(x => x.Key,x => x.ToList())
						.Select(x => new IncomeTransferBucket(x.Value))
						.ToList();
				case AccountBalancingLayers.Nothing:
					return new List<IncomeTransferBucket>() {
						new IncomeTransferBucket(listAccountEntries)
					};
				case AccountBalancingLayers.Unearned:
				default:
					throw new ODException($"Income transfer buckets cannot be created for unsupported layer: {layer}");
			}
		}

		///<summary>Breaks up the individual buckets passed in into sub-buckets that are grouped by procedures.
		///Loops through each sub-bucket and removes as much production as possible (allocating AmountEnd).
		///Creates a transfer if there is a net negative after all production has been allocated.
		///There shouldn't be such a thing as negative production and if there is then it should be transferred to unearned for later transfers.
		///Also, balances any unallocated / unearned income that is not explicitly linked to a procedure for all buckets passed in.
		///This last step is so that we don't get stuck in infinite loops transferring income between unallocated and unearned.</summary>
		private static void PreprocessProvPatClinicBuckets(ref List<IncomeTransferBucket> listBuckets,ref List<AccountEntry> listAccountEntries,
			ref IncomeTransferData incomeTransferData,DateTime datePay)
		{
			//No remoting role check; private method
			incomeTransferData.AppendLine($"Preprocessing buckets for ProvPatClinic...");
			foreach(IncomeTransferBucket bucket in listBuckets) {
				List<AccountEntry> listUnallocatedEntries=new List<AccountEntry>();
				List<AccountEntry> listUnearnedEntries=new List<AccountEntry>();
				List<AccountEntry> listNonPayPlanEntries=bucket.ListAccountEntries.FindAll(x => x.PayPlanNum==0 && x.GetType()!=typeof(FauxAccountEntry));
				List<AccountEntry> listPayPlanEntries=bucket.ListAccountEntries.FindAll(x => x.PayPlanNum > 0);
				foreach(var procGroup in listNonPayPlanEntries.GroupBy(x => x.ProcNum).ToDictionary(x => x.Key,x => x.ToList())) {
					List<AccountEntry> listBucketEntries=procGroup.Value;
					if(procGroup.Key==0) {//Account entries are not associated to any procedures.
						listUnallocatedEntries=listBucketEntries.FindAll(x => x.IsUnallocated);
						listUnearnedEntries=listBucketEntries.FindAll(x => x.IsUnearned);
						continue;
					}
					decimal total=listBucketEntries.Sum(x => x.AmountEnd);
					if(total.IsZero()) {
						listBucketEntries.ForEach(x => x.AmountEnd=0);
						continue;
					}
					else if(total.IsGreaterThanZero()) {
						continue;
					}
					long clinicNum=listBucketEntries.First().ClinicNum;
					long patNum=listBucketEntries.First().PatNum;
					long procNum=procGroup.Key;
					long provNum=listBucketEntries.First().ProvNum;
					//This procedure specific bucket does not balance out.
					//Try to balance the procedure specific bucket FIFO style (strictly consider negative and positive amounts).
					string results=BalanceAccountEntries(ref listBucketEntries);
					if(!string.IsNullOrWhiteSpace(results)) {
						incomeTransferData.AppendLine($"  Procedure #{procNum} PatNum #{patNum}" +
							$", ProvNum #{provNum} ({Providers.GetAbbr(provNum)})" +
							$"{((clinicNum > 0) ? $", ClinicNum #{clinicNum} ({Clinics.GetAbbr(clinicNum)})" : "")}" +
							$" is over allocated");
						incomeTransferData.AppendLine(results);
					}
					decimal offsetAmt=listBucketEntries.Sum(x => x.AmountEnd);
					//Any negative balance left over shall get transferred to unearned so that it can be balanced later.
					//Create AccountEntries out of the PaySplits that were just created and add them to listAccountEntries for allocation.
					List<PaySplit> listPaySplits=CreateUnearnedTransfer(offsetAmt,patNum,provNum,clinicNum,ref incomeTransferData,procNum:procNum,
						datePay:datePay);
					AccountEntry accountEntryOffset=new AccountEntry(listPaySplits[0]);
					AccountEntry accountEntryUnearned=new AccountEntry(listPaySplits[1]);
					//The PaySplits that were just created will technically offset any production or income that left this bucket in the negative.
					//Zero out the AmountEnd field for every negative account entry so that they are not transferred.
					listBucketEntries.FindAll(x => x.AmountEnd.IsLessThanZero()).ForEach(x => x.AmountEnd=0);
					//Also, the offsetting PaySplit needs to have no value (untransferrable, a.k.a. AmountEnd set to zero).
					accountEntryOffset.AmountEnd=0;
					listAccountEntries.AddRange(new List<AccountEntry>() {
						accountEntryOffset,
						accountEntryUnearned,
					});
					listUnearnedEntries.Add(accountEntryUnearned);
				}
				foreach(var adjGroup in listNonPayPlanEntries.GroupBy(x => x.AdjNum).ToDictionary(x => x.Key,x => x.ToList())) {
					if(adjGroup.Key==0) {
						continue;//Unearned and unallocated have already been considered within the procedure grouping loop.
					}
					List<AccountEntry> listBucketEntries=adjGroup.Value;
					decimal total=listBucketEntries.Sum(x => x.AmountEnd);
					if(total.IsZero()) {
						listBucketEntries.ForEach(x => x.AmountEnd=0);
						continue;
					}
					else if(total.IsGreaterThanZero()) {
						continue;
					}
					long adjNum=adjGroup.Key;
					long clinicNum=listBucketEntries.First().ClinicNum;
					long patNum=listBucketEntries.First().PatNum;
					long provNum=listBucketEntries.First().ProvNum;
					//This adjustment specific bucket does not balance out.
					//Try to balance the adjustment specific bucket FIFO style (strictly consider negative and positive amounts).
					string results=BalanceAccountEntries(ref listBucketEntries);
					if(!string.IsNullOrWhiteSpace(results)) {
						incomeTransferData.AppendLine($"  Adjustment #{adjNum} PatNum #{patNum}" +
							$", ProvNum #{provNum} ({Providers.GetAbbr(provNum)})" +
							$"{((clinicNum > 0) ? $", ClinicNum #{clinicNum} ({Clinics.GetAbbr(clinicNum)})" : "")}" +
							$" is over allocated");
						incomeTransferData.AppendLine(results);
					}
					decimal offsetAmt=listBucketEntries.Sum(x => x.AmountEnd);
					//Any negative balance left over shall get transferred to unearned so that it can be balanced later.
					//Create an AccountEntry out of the unearned PaySplit that was just created and add it to listPendingAccountEntries for allocation.
					List<PaySplit> listPaySplits=CreateUnearnedTransfer(offsetAmt,patNum,provNum,clinicNum,ref incomeTransferData,adjNum:adjNum,
						datePay:datePay);
					AccountEntry accountEntryOffset=new AccountEntry(listPaySplits[0]);
					AccountEntry accountEntryUnearned=new AccountEntry(listPaySplits[1]);
					//The PaySplits that were just created will technically offset any production or income that left this bucket in the negative.
					//Zero out the AmountEnd field for every negative account entry so that they are not transferred.
					listBucketEntries.FindAll(x => x.AmountEnd.IsLessThanZero()).ForEach(x => x.AmountEnd=0);
					//Also, the offsetting PaySplit needs to have no value (untransferrable, a.k.a. AmountEnd set to zero).
					accountEntryOffset.AmountEnd=0;
					listAccountEntries.AddRange(new List<AccountEntry>() {
						accountEntryOffset,
						accountEntryUnearned,
					});
					listUnearnedEntries.Add(accountEntryUnearned);
				}
				foreach(var payPlanGroup in listPayPlanEntries.GroupBy(x => x.PayPlanNum).ToDictionary(x => x.Key,x => x.ToList())) {
					List<AccountEntry> listBucketEntries=payPlanGroup.Value;
					listBucketEntries.AddRange(listPayPlanEntries.FindAll(x => !x.In(listBucketEntries)));
					//Skip adjustments because they directly manipulate the value of production and should not be transferred to unearned at this point.
					listBucketEntries.RemoveAll(x => x.GetType()==typeof(FauxAccountEntry) && ((FauxAccountEntry)x).IsAdjustment);
					if(payPlanGroup.Key==0) {//Account entries are not associated to any payment plans.
						continue;
					}
					decimal total=listBucketEntries.Sum(x => x.AmountEnd);
					if(total.IsZero()) {
						listBucketEntries.ForEach(x => x.AmountEnd=0);
						continue;
					}
					else if(total.IsGreaterThanZero()) {
						continue;
					}
					long clinicNum=listBucketEntries.First().ClinicNum;
					long patNum=listBucketEntries.First().PatNum;
					long payPlanNum=payPlanGroup.Key;
					long provNum=listBucketEntries.First().ProvNum;
					//This payment plan specific bucket does not balance out.
					//Try to balance the payment plan specific bucket FIFO style (strictly consider negative and positive amounts).
					string results=BalanceAccountEntries(ref listBucketEntries);
					if(!string.IsNullOrWhiteSpace(results)) {
						incomeTransferData.AppendLine($"  Payment Plan #{payPlanNum} PatNum #{patNum}" +
							$", ProvNum #{provNum} ({Providers.GetAbbr(provNum)})" +
							$"{((clinicNum > 0) ? $", ClinicNum #{clinicNum} ({Clinics.GetAbbr(clinicNum)})" : "")}");
						incomeTransferData.AppendLine(results);
					}
					//Payment plans are unique in that they can have unearned payments attached that might need to be moved out of the payment plan.
					//Preserve the UnearnedType in order to not create an infinite loop of transferring the same unearned out of a payment plan over and over.
					Dictionary<long,List<AccountEntry>> dictUnearnedEntries=listBucketEntries.GroupBy(x => x.UnearnedType).ToDictionary(x => x.Key,x => x.ToList());
					foreach(long unearnedType in dictUnearnedEntries.Keys) {
						decimal offsetAmt=dictUnearnedEntries[unearnedType].Sum(x => x.AmountEnd);
						List<PaySplit> listPaySplits=CreateUnearnedTransfer(offsetAmt,patNum,provNum,clinicNum,ref incomeTransferData,unearnedType:unearnedType,
							payPlanNum:payPlanNum,datePay:datePay);
						AccountEntry accountEntryOffset=new AccountEntry(listPaySplits[0]);
						AccountEntry accountEntryUnearned=new AccountEntry(listPaySplits[1]);
						//The PaySplits that were just created will technically offset any production or income that left this bucket in the negative.
						//Zero out the AmountEnd field for every negative account entry so that they are not transferred.
						dictUnearnedEntries[unearnedType].FindAll(x => x.AmountEnd.IsLessThanZero()).ForEach(x => x.AmountEnd=0);
						//Also, the offsetting PaySplit needs to have no value (untransferrable, a.k.a. AmountEnd set to zero).
						accountEntryOffset.AmountEnd=0;
						listAccountEntries.AddRange(new List<AccountEntry>() {
							accountEntryOffset,
							accountEntryUnearned,
						});
						listUnearnedEntries.Add(accountEntryUnearned);
					}
				}
				//Balance both unallocated and unearned lists (manipulate their AmountEnd to balance themselves out FIFO style).
				if(listUnallocatedEntries.Count > 1) {
					string results=BalanceAccountEntries(ref listUnallocatedEntries);
					if(!string.IsNullOrWhiteSpace(results)) {
						incomeTransferData.AppendLine($"  Balancing {listUnallocatedEntries.Count} unallocated PaySplits not associated to any procedures...");
						incomeTransferData.AppendLine(results);
						incomeTransferData.AppendLine($"  Done");
					}
				}
				if(listUnearnedEntries.Count > 1) {
					string results=BalanceAccountEntries(ref listUnearnedEntries);
					if(!string.IsNullOrWhiteSpace(results)) {
						incomeTransferData.AppendLine($"  Balancing {listUnearnedEntries.Count} unearned PaySplits not associated to any procedures...");
						incomeTransferData.AppendLine(results);
						incomeTransferData.AppendLine($"  Done");
					}
				}
			}
		}

		///<summary>Loops through all of the positive entries in the bucket and creates as many transfers as necessary from the negative entries
		///until the entire bucket has been balanced as much as possible.  Preprocessing should be done prior to calling this method.</summary>
		private static IncomeTransferData TransferLoopHelper(IncomeTransferBucket bucket,long guarNum,DateTime datePay) {
			//No remoting role check; private method
			IncomeTransferData transferData=new IncomeTransferData();
			foreach(AccountEntry posCharge in bucket.ListPositiveEntries) {
				if(posCharge.AmountEnd.IsLessThanOrEqualToZero()) {
					continue;
				}
				bool hasTransfer=false;
				foreach(AccountEntry negCharge in bucket.ListNegativeEntries) {
					if(posCharge.AmountEnd.IsLessThanOrEqualToZero()) {
						break;
					}
					if(negCharge.AmountEnd.IsGreaterThanOrEqualToZero()) {
						continue;
					}
					//Check if both positive and negative charges are misallocated PaySplits and skip them if they are.
					//This is to prevent transferring between misallocated income which would just create more misallocated income.
					if(posCharge.IsPaySplitAttachedToProd && negCharge.IsPaySplitAttachedToProd) {
						continue;
					}
					if(!hasTransfer) {
						transferData.AppendLine($"  Balancing {posCharge.Description}");
						hasTransfer=true;
					}
					transferData.AppendLine($"    Moving excess to {negCharge.Description}");
					transferData.MergeIncomeTransferData(CreateTransferHelper(posCharge,negCharge,guarNum,datePay));
				}
				if(hasTransfer) {
					transferData.AppendLine($"  Done - {posCharge.Description}");
				}
			}
			return transferData;
		}

		///<summary>Creates and links paysplits with micro-allocations based on the charges passed in.  
		///Constructs the paysplits with all necessary information depending on the type of the charges.
		///Returns true if attempting to create invalid splits and rigorous accounting is enabled.</summary>
		private static IncomeTransferData CreateTransferHelper(AccountEntry posCharge,AccountEntry negCharge,long guarNum,DateTime datePay) {
			//No remoting role check; private method
			IncomeTransferData transferSplits=new IncomeTransferData();
			if(negCharge.GetType()==typeof(Procedure) && ((Procedure)negCharge.Tag).ProcFee.IsLessThanZero()) {
				transferSplits.AppendLine($"  Negative procedure cannot be used as source of income:\r\n      {negCharge.Description}");
				return transferSplits;//do not use negative procedures as sources of income. 
			}
			decimal amt=Math.Min(Math.Abs(posCharge.AmountEnd),Math.Abs(negCharge.AmountEnd));
			if(amt.IsEqual(0)) {
				return transferSplits;//there is no income to transfer
			}
			#region Positive Split
			PaySplit posSplit=new PaySplit();
			posSplit.DatePay=datePay;
			posSplit.ClinicNum=posCharge.ClinicNum;
			posSplit.FSplitNum=0;//Can't set FSplitNum just yet, the neg split isn't inserted so there is no FK yet.
			posSplit.PatNum=posCharge.PatNum;
			posSplit.PayPlanNum=posCharge.PayPlanNum;
			posSplit.AdjNum=posCharge.AdjNum;
			//A split should never be attached to both a procedure and an adjustment at the same time.
			//Therefore, the split being created for an adjustment account entry should ignore any attached procedure so that the adjustment is paid.
			posSplit.ProcNum=(posCharge.GetType()==typeof(Adjustment) ? 0 : posCharge.ProcNum);
			posSplit.ProvNum=posCharge.ProvNum;
			posSplit.SplitAmt=(double)amt;
			posSplit.UnearnedType=posCharge.UnearnedType;
			#endregion
			#region Negative Split
			PaySplit negSplit=new PaySplit();
			negSplit.DatePay=datePay;
			negSplit.ClinicNum=negCharge.ClinicNum;
			negSplit.FSplitNum=0;
			negSplit.PatNum=negCharge.PatNum;
			negSplit.PayPlanNum=negCharge.PayPlanNum;
			negSplit.AdjNum=negCharge.AdjNum;
			//A split should never be attached to both a procedure and an adjustment at the same time.
			//Therefore, the split being created for an adjustment account entry should ignore any attached procedure so that the adjustment is paid.
			negSplit.ProcNum=(negCharge.GetType()==typeof(Adjustment) ? 0 : negCharge.ProcNum);
			//Money may be coming from an overpaid procedure instead of paysplit.
			if(negCharge.GetType()==typeof(PaySplit)) {
				negSplit.FSplitNum=negCharge.PriKey;
				if(negCharge.PriKey==0) {
					//money is coming from a split that we created earlier, and hasn't been inserted into the database yet. Make a split association for it.
					transferSplits.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated((PaySplit)negCharge.Tag,negSplit));
				}
			}
			negSplit.ProvNum=negCharge.ProvNum;
			negSplit.SplitAmt=0-(double)amt;
			negSplit.UnearnedType=negCharge.UnearnedType;
			#endregion
			if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
				//split being created needs to have a procedure with a provider, or an adjustment with a provider 
				//or unearned type (optionally w/ provider) BUT if unearned, cannot have a procedure or adjustment. 
				if((Math.Sign(posSplit.ProcNum)!=Math.Sign(posSplit.ProvNum) && Math.Sign(posSplit.AdjNum)!=Math.Sign(posSplit.ProvNum))
					|| (posSplit.UnearnedType==0 && posSplit.ProvNum==0))//Allow the negative unearned split to have a provider to fix bad scenarios
				{
					transferSplits.HasInvalidSplits=true;
					transferSplits.AppendLine($"  Due to Rigorous Accounting, invalid transactions have been cancelled for Guarantor: #{guarNum}"+
						$"\r\n    Manual fix required.");
					return transferSplits;
				}
			}
			if(!Prefs.GetBool(PrefName.AllowPrepayProvider)) {//this pref used to only be available for enforce fully, but now is for all types.
				if(posSplit.UnearnedType!=0 && posSplit.ProvNum!=0) {//Allow the negative unearned split to have a provider to fix bad scenarios
					transferSplits.HasInvalidSplits=true;
					return transferSplits;
				}
			}
			transferSplits.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated(negSplit,posSplit));
			transferSplits.ListSplitsCur.AddRange(new List<PaySplit>() { posSplit, negSplit });
			negCharge.SplitCollection.Add(negSplit);
			posCharge.SplitCollection.Add(posSplit);
			posCharge.AmountEnd-=amt;
			negCharge.AmountEnd+=amt;
			transferSplits.AppendLine($"      ^PaySplit created to move {amt.ToString("c")} " +
				$"from {posCharge.TagTypeName} [AmtEnd: {posCharge.AmountEnd.ToString("c")}]");
			transferSplits.AppendLine($"      ^PaySplit created to move {amt.ToString("c")} " +
				$"to {negCharge.TagTypeName} [AmtEnd: {negCharge.AmountEnd.ToString("c")}]");
			return transferSplits;
		}

		private static List<PaySplit> CreateUnearnedTransfer(AccountEntry accountEntry,ref IncomeTransferData incomeTransferData,DateTime datePay) {
			//No remoting role check; private method
			long procNum=0;
			long adjNum=0;
			long fSplitNum=0;
			long unearnedType=0;
			if(accountEntry.GetType()==typeof(PaySplit)) {
				procNum=((PaySplit)accountEntry.Tag).ProcNum;
				unearnedType=((PaySplit)accountEntry.Tag).UnearnedType;
				fSplitNum=accountEntry.PriKey;
			}
			else if(accountEntry.GetType()==typeof(Procedure)) {
				procNum=((Procedure)accountEntry.Tag).ProcNum;
				fSplitNum=0;
			}
			else if(accountEntry.GetType()==typeof(Adjustment)) {
				adjNum=((Adjustment)accountEntry.Tag).AdjNum;
				fSplitNum=0;
			}
			return CreateUnearnedTransfer(accountEntry.AmountEnd,accountEntry.PatNum,accountEntry.ProvNum,accountEntry.ClinicNum,ref incomeTransferData,
				procNum:procNum,adjNum:adjNum,fSplitNum:fSplitNum,unearnedType:unearnedType,payPlanNum:accountEntry.PayPlanNum,datePay:datePay);
		}

		private static List<PaySplit> CreateUnearnedTransfer(decimal splitAmount,long patNum,long provNum,long clinicNum,
			ref IncomeTransferData incomeTransferData,long procNum=0,long adjNum=0,long fSplitNum=0,long unearnedType=0,long payPlanNum=0,
			DateTime datePay=default)
		{
			//No remoting role check; private method
			long offsetUnearnedType=0;
			//Payment plans are the only entities that need the ability to transfer unearned to unearned.
			if(payPlanNum > 0) {
				offsetUnearnedType=unearnedType;
			}
			if(datePay.Year < 1880) {
				datePay=DateTimeOD.Today;
			}
			PaySplit offsetSplit=new PaySplit() {
				AdjNum=adjNum,
				ClinicNum=clinicNum,
				DatePay=datePay,
				FSplitNum=fSplitNum,
				PatNum=patNum,
				PayPlanNum=payPlanNum,
				ProcNum=procNum,
				ProvNum=provNum,
				SplitAmt=(double)splitAmount,
				UnearnedType=offsetUnearnedType,
			};
			PaySplit unearnedSplit=new PaySplit() {
				AdjNum=0,
				ClinicNum=clinicNum,
				DatePay=datePay,
				FSplitNum=0,
				PatNum=patNum,
				PayPlanNum=0,
				ProcNum=0,
				ProvNum=Prefs.GetBool(PrefName.AllowPrepayProvider) ? provNum : 0,
				SplitAmt=0-(double)splitAmount,
				UnearnedType=(unearnedType==0? Prefs.GetLong(PrefName.PrepaymentUnearnedType) : unearnedType),
			};
			List<PaySplit> listPaySplits=new List<PaySplit>() { offsetSplit,unearnedSplit };
			incomeTransferData.ListSplitsCur.AddRange(listPaySplits);
			incomeTransferData.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated(offsetSplit,unearnedSplit));
			incomeTransferData.AppendLine($"    ^PaySplit created to move {offsetSplit.SplitAmt.ToString("c")} from {GetPaySplitTypeDesc(offsetSplit)}");
			incomeTransferData.AppendLine($"    ^PaySplit created to move {unearnedSplit.SplitAmt.ToString("c")} to {GetPaySplitTypeDesc(unearnedSplit)}");
			return listPaySplits;
		}

		///<summary>A helper method that will move as much AmountEnd as possible from the negative entries and apply them to the positive ones.
		///Returns true if account entries changed.  Otherwise; false.</summary>
		private static string BalanceAccountEntries(ref List<AccountEntry> listAccountEntries) {
			//No remoting role check; private method
			StringBuilder strBuilderSummary=new StringBuilder();
			foreach(AccountEntry positiveEntry in listAccountEntries.FindAll(x => x.AmountEnd.IsGreaterThanZero())) {
				if(positiveEntry.AmountEnd.IsLessThanOrEqualToZero()) {
					continue;
				}
				foreach(AccountEntry negativeEntry in listAccountEntries.FindAll(x => x.AmountEnd.IsLessThanZero())) {
					if(positiveEntry.AmountEnd.IsLessThanOrEqualToZero()) {
						break;
					}
					if(negativeEntry.AmountEnd.IsGreaterThanOrEqualToZero()) {
						continue;
					}
					decimal amountTxfr=Math.Min(Math.Abs(positiveEntry.AmountEnd),Math.Abs(negativeEntry.AmountEnd));
					positiveEntry.AmountEnd-=amountTxfr;
					negativeEntry.AmountEnd+=amountTxfr;
					strBuilderSummary.AppendLine($"    Removed {amountTxfr.ToString("c")} from {positiveEntry.Description}");
					strBuilderSummary.AppendLine($"    Added {amountTxfr.ToString("c")} to {negativeEntry.Description}");
				}
			}
			return strBuilderSummary.ToString().TrimEnd();
		}

		///<summary>Method to encapsulate the creation of a new payment that is specifically meant to store payment information for the unallocated
		///payment transfer.</summary>
		public static long CreateAndInsertUnallocatedPayment(Patient patCur) {
			//No remoting role check; no call to db
			//user clicked ok and has permisson to save splits to the database. 
			Payment unallocatedTransferPayment=new Payment();
			unallocatedTransferPayment.PatNum=patCur.PatNum;
			unallocatedTransferPayment.PayDate=DateTime.Today;
			unallocatedTransferPayment.ClinicNum=0;
			if(PrefC.HasClinicsEnabled) {//if clinics aren't enabled default to 0
				unallocatedTransferPayment.ClinicNum=Clinics.ClinicId;
				if((PayClinicSetting)PrefC.GetInt(PrefName.PaymentClinicSetting)==PayClinicSetting.PatientDefaultClinic) {
					unallocatedTransferPayment.ClinicNum=patCur.ClinicNum;
				}
				else if((PayClinicSetting)PrefC.GetInt(PrefName.PaymentClinicSetting)==PayClinicSetting.SelectedExceptHQ) {
					unallocatedTransferPayment.ClinicNum=(Clinics.ClinicId==0 ? patCur.ClinicNum : Clinics.ClinicId);
				}
			}
			unallocatedTransferPayment.DateEntry=DateTime.Today;
			unallocatedTransferPayment.PaymentSource=CreditCardSource.None;
			unallocatedTransferPayment.PayAmt=0;
			unallocatedTransferPayment.PayType=0;
			long payNum=Payments.Insert(unallocatedTransferPayment);
			return payNum;
		}

		///<summary>A helper method to get a description for the type of pay split that was passed in.  E.g. returns "Adjustment" if AdjNum > 0</summary>
		private static string GetPaySplitTypeDesc(PaySplit paySplit) {
			string offsetTypeName="Unallocated";
			if(paySplit.UnearnedType > 0) {
				offsetTypeName="Unearned";
			}
			else if(paySplit.ProcNum > 0) {
				offsetTypeName="Procedure";
			}
			else if(paySplit.AdjNum > 0) {
				offsetTypeName="Adjustment";
			}
			else if(paySplit.PayPlanChargeNum > 0) {
				offsetTypeName="PayPlanCharge";
			}
			else if(paySplit.PayPlanNum > 0) {
				offsetTypeName="PayPlan";
			}
			return offsetTypeName;
		}

		#endregion

		#region Data Classes
		///<summary>The data needed to load FormPayment.</summary>
		public class LoadData {
			public Patient PatCur;
			public Family Fam;
			public Family SuperFam;
			public Payment PaymentCur;
			public List<CreditCard> ListCreditCards;
			public XWebResponse XWebResponse;
			public PayConnectResponseWeb PayConnectResponseWeb;
			public DataTable TableBalances;
			///<summary>List of splits associated to this payment</summary>
			public List<PaySplit> ListSplits;
			public List<PaySplit> ListPaySplitAllocations;
			public Transaction Transaction;
			public List<PayPlan> ListValidPayPlans;
			public List<Patient> ListAssociatedPatients;
			public List<PaySplit> ListPrePaysForPayment;
			public ConstructChargesData ConstructChargesData;
			public List<Procedure> ListProcsForSplits;
			public SerializableDictionary<long,long> DictPatToDPFeeScheds;
			public List<Fee> ListFeesForDiscountPlans;
		}

		///<summary>The data needed to construct a list of charges for FormPayment.</summary>
		[Serializable]
		public class ConstructChargesData {
			///<summary>List from the db, completed for pat. Not list of pre-selected procs from acct. Contains TP procs if pref is set to ON.</summary>
			public List<Procedure> ListProcs=new List<Procedure>();
			public List<Adjustment> ListAdjustments=new List<Adjustment>();
			///<summary>Current list of all splits from database</summary>
			public List<PaySplit> ListPaySplits=new List<PaySplit>();
			///<summary>Stores the summed outstanding ins pay as totals (amounts and write offs) for the list of patnums</summary>
			public List<PayAsTotal> ListInsPayAsTotal=new List<PayAsTotal>();
			public List<PayPlan> ListPayPlans=new List<PayPlan>();
			public List<PaySplit> ListPayPlanSplits=new List<PaySplit>();
			///<summary>List of all pay plan charges (including future charges).  Does not include insurance pay plan charges.</summary>
			public List<PayPlanCharge> ListPayPlanCharges=new List<PayPlanCharge>();
			///<summary>Stores the list of claimprocs (not ins pay as totals) for the list of pat nums</summary>
			public List<ClaimProc> ListClaimProcsFiltered=new List<ClaimProc>();
			///<summary>List of all pay plan links for the pay plans.</summary>
			public List<PayPlanLink> ListPayPlanLinks=new List<PayPlanLink>();
		}

		///<summary>Data retrieved upon initialization. AutpSplit stores data retireved from going through list of charges, linking,and autosplitting.</summary>
		[Serializable]
		public class InitData {
			public AutoSplit AutoSplitData;
			public Dictionary<long,Patient> DictPats=new Dictionary<long,Patient>();
			public decimal SplitTotal;
		}

		/// <summary>Data resulting after making a payment.</summary>
		[Serializable]
		public class PayResults {
			public List<PaySplit> ListSplitsCur=new List<PaySplit>();
			public Payment Payment;
			public List<AccountEntry> ListAccountCharges=new List<AccountEntry>();
		}

		/// <summary>Data results after constructing list of charges and linking credits to them.</summary>
		[Serializable]
		public class ConstructResults {
			public List<PaySplit> ListSplitsCur=new List<PaySplit>();
			public Payment Payment;
			public List<AccountEntry> ListAccountCharges=new List<AccountEntry>();
		}
		
		/// <summary>Data after autosplitting. ListAutoSplits is separate from ListSplitsCur./// </summary>
		[Serializable]
		public class AutoSplit {
			public List<PaySplit> ListAutoSplits=new List<PaySplit>();
			public Payment Payment;
			public List<AccountEntry> ListAccountCharges=new List<AccountEntry>();
			public List<PaySplit> ListSplitsCur=new List<PaySplit>();
		}

		///<summary>Keeps the list of splits that are being transferred, and a bool telling if one or more that was attempted are invalid.</summary>
		[Serializable]
		public class IncomeTransferData {
			public List<PaySplit> ListSplitsCur=new List<PaySplit>();
			public List<PaySplits.PaySplitAssociated> ListSplitsAssociated=new List<PaySplits.PaySplitAssociated>();
			public bool HasInvalidSplits=false;
			public bool HasInvalidProcWithPayPlan=false;
			private StringBuilder _stringBuilderSummary=new StringBuilder();

			///<summary>This is a detailed log of what happened during the transfer process.  This text is designed to help explain the transfer.</summary>
			public string SummaryText {
				get {
					return _stringBuilderSummary.ToString().TrimEnd();
				}
			}

			///<summary>Appends text along with a new line at the end of it to Summary.  Does nothing if the text passed in is null or empty.</summary>
			public void AppendLine(string text) {
				if(string.IsNullOrEmpty(text)) {
					return;
				}
				_stringBuilderSummary.AppendLine(text);
			}

			///<summary>Merges given data list into this objects assocaited list.</summary>
			public void MergeIncomeTransferData(IncomeTransferData data) {
				this.ListSplitsAssociated.AddRange(data.ListSplitsAssociated);
				this.ListSplitsCur.AddRange(data.ListSplitsCur);
				this.HasInvalidSplits|=data.HasInvalidSplits;
				this.HasInvalidProcWithPayPlan|=data.HasInvalidProcWithPayPlan;
				AppendLine(data.SummaryText);
			}

			///<summary>Creates the negative and positive splits from a given parentSplit and a payment's payNum. When isTransferToUnearned is true, these
			///splits will transfer to the user's unearned type based on preference values. If transferAmtOverride is given a value, it will be used for the
			///split amounts in place of the parentSplit.SplitAmt.</summary>
			public static IncomeTransferData CreateTransfer(PaySplit parentSplit,long payNum,bool isTransferToUnearned=false
				,double transferAmtOverride=0) 
			{
				IncomeTransferData transferReturning=new IncomeTransferData();
				PaySplit offsetSplit=new PaySplit();
				offsetSplit.DatePay=DateTime.Today;
				offsetSplit.PatNum=parentSplit.PatNum;
				offsetSplit.PayNum=payNum;
				offsetSplit.ProvNum=parentSplit.ProvNum;
				offsetSplit.ClinicNum=parentSplit.ClinicNum;
				offsetSplit.UnearnedType=parentSplit.UnearnedType;
				offsetSplit.ProcNum=parentSplit.ProcNum;
				offsetSplit.AdjNum=parentSplit.AdjNum;
				offsetSplit.SplitAmt=(transferAmtOverride==0?parentSplit.SplitAmt:transferAmtOverride)*-1;
				offsetSplit.FSplitNum=parentSplit.SplitNum;
				PaySplit allocationSplit=new PaySplit();
				allocationSplit.DatePay=DateTime.Today;
				allocationSplit.PatNum=parentSplit.PatNum;
				allocationSplit.PayNum=payNum;
				allocationSplit.ProvNum=parentSplit.ProvNum;
				allocationSplit.ClinicNum=parentSplit.ClinicNum;
				allocationSplit.ProcNum=parentSplit.ProcNum;
				allocationSplit.AdjNum=parentSplit.AdjNum;
				allocationSplit.SplitAmt=transferAmtOverride==0?parentSplit.SplitAmt:transferAmtOverride;
				allocationSplit.UnearnedType=parentSplit.UnearnedType;
				allocationSplit.FSplitNum=0;//should be offsetSplit's splitNum but has not been inserted into DB yet
				if(isTransferToUnearned) {
					allocationSplit.UnearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
				}
				transferReturning.ListSplitsCur.AddRange(new List<PaySplit>{offsetSplit,allocationSplit });
				transferReturning.ListSplitsAssociated.Add(new PaySplits.PaySplitAssociated(offsetSplit,allocationSplit));
				return transferReturning;
			} 
		}

		///<summary>Helper class for organizing account entries when processing income transfer layers.</summary>
		public class IncomeTransferBucket {
			///<summary>All account entries for the current bucket.</summary>
			public List<AccountEntry> ListAccountEntries=new List<AccountEntry>();

			///<summary>All account entries that have an AmountEnd greater than zero.</summary>
			public List<AccountEntry> ListPositiveEntries {
				get {
					return ListAccountEntries.FindAll(x => x.AmountEnd.IsGreaterThanZero());
				}
			}

			///<summary>All account entries that have an AmountEnd less than zero.</summary>
			public List<AccountEntry> ListNegativeEntries {
				get {
					return ListAccountEntries.FindAll(x => x.AmountEnd.IsLessThanZero());
				}
			}

			public IncomeTransferBucket(List<AccountEntry> listAccountEntries) {
				ListAccountEntries=listAccountEntries;
			}
		}

		///<summary>Helper class for organizing entities involved with implicitly linking account entries.</summary>
		public class ImplicitLinkBucket {
			///<summary>All account entries for the current bucket.</summary>
			public List<AccountEntry> ListAccountEntries=new List<AccountEntry>();
			///<summary>All PayAsTotals for the current bucket.</summary>
			public List<PayAsTotal> ListInsPayAsTotal=new List<PayAsTotal>();
			///<summary>All PaySplits for the current bucket.</summary>
			public List<PaySplit> ListPaySplits=new List<PaySplit>();

			public ImplicitLinkBucket(List<AccountEntry> listAccountEntries) {
				ListAccountEntries=listAccountEntries;
			}
		}

		///<summary>Represents ways of grouping up account entities.  Each layer represents how account entries should be grouped for balancing.
		///It is critical that each layer be considered in the EXACT order that this enumeration is constructed when balancing an account.</summary>
		private enum AccountBalancingLayers {
			///<summary>0 - Creates buckets grouped by provider, patient, and clinic for balancing.  Arguably the most important layer.
			///This is the only layer that is explicit enough to transfer money to unearned.
			///Transferring money to unearned should happen prior to processing the income transfer buckets that this layer creates.</summary>
			ProvPatClinic,
			///<summary>1 - Creates buckets grouped by provider and patient for balancing.</summary>
			ProvPat,
			///<summary>2 - Creates buckets grouped by provider and clinic for balancing.</summary>
			ProvClinic,
			///<summary>3 - Creates buckets grouped by patient and clinic for balancing.</summary>
			PatClinic,
			///<summary>4 - Creates buckets grouped by provider for balancing.</summary>
			Prov,
			///<summary>5 - Creates buckets grouped by patient for balancing.</summary>
			Pat,
			///<summary>6 - Creates buckets grouped by clinic for balancing.</summary>
			Clinic,
			///<summary>7 - Creates buckets grouped by nothing for balancing.</summary>
			Nothing,
			///<summary>8 - Does not create any bucket for balancing.  This is another special layer and it should always be considered last.
			///It will move any leftover money that the previous layers could not address and will move it over to unearned for balancing.</summary>
			Unearned,
		}

		#endregion
	}
}
