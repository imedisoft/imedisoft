using CodeBase;
using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness {
	///<summary></summary>
	public class RepeatCharges {
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

		///<summary>Gets a list of all RepeatCharges for a given patient.  Supply 0 to get a list for all patients.</summary>
		public static RepeatCharge[] Refresh(long patNum) {
			
			string command="SELECT * FROM repeatcharge";
			if(patNum!=0) {
				command+=" WHERE PatNum = "+POut.Long(patNum);
			}
			command+=" ORDER BY DateStart";
			return Crud.RepeatChargeCrud.SelectMany(command).ToArray();
		}	

		///<summary></summary>
		public static void Update(RepeatCharge charge){
			
			Crud.RepeatChargeCrud.Update(charge);
		}

		public static void UpdateChargeAmt(long repeatChargeNum,double chargeAmt) {
			
			string command="UPDATE repeatcharge SET ChargeAmt="+POut.Double(chargeAmt)+" "
				+"WHERE RepeatChargeNum="+POut.Long(repeatChargeNum);
			Database.ExecuteNonQuery(command);
		}

		///<summary></summary>
		public static long Insert(RepeatCharge charge) {
			
			return Crud.RepeatChargeCrud.Insert(charge);
		}

		///<summary>Called from FormRepeatCharge.</summary>
		public static void Delete(RepeatCharge charge){
			
			string command="DELETE FROM repeatcharge WHERE RepeatChargeNum ="+POut.Long(charge.RepeatChargeNum);
			Database.ExecuteNonQuery(command);
		}

		///<summary>For internal use only.  Returns all eRx repeating charges for all customers.</summary>
		public static List<RepeatCharge> GetForErx() {
			
			//Does not need to be Oracle compatible because this is an internal tool only.
			string command="SELECT * FROM repeatcharge WHERE ProcCode REGEXP '^Z[0-9]{3,}$'";
			return Crud.RepeatChargeCrud.SelectMany(command);
		}

		///<summary>Get the list of all RepeatCharge rows. DO NOT REMOVE! Used by OD WebApps solution.</summary>
		// ReSharper disable once UnusedMember.Global
		public static List<RepeatCharge> GetAll() {
			//No need to check RemotingRole; no call to db.
			return Refresh(0).ToList();			
		}

		///<summary>Gets all repeat charges for a family.  A family is currently defined as all accounts that share the same guarantor or are 
		///associated to the same super family.  This is used by the Reseller Portal to get all repeat charges linked to the given reseller.
		///Optionally pass in a super family in order to broaden the family tree by also including accounts in the same super family.</summary>
		public static List<RepeatCharge> GetByGuarantorOrSuperFamily(long guarantor,long superFamily=0) {
			
			string command="SELECT rc.* FROM repeatcharge rc "
				+"INNER JOIN patient p ON p.PatNum=rc.PatNum "
				+"WHERE p.Guarantor="+POut.Long(guarantor)+" ";
			if(superFamily > 0) {
				command+="OR p.SuperFamily="+POut.Long(superFamily);
			}
			return Crud.RepeatChargeCrud.SelectMany(command);
		}

		///<summary>Returns true if there are any active repeating charges on the patient's account, false if there are not.</summary>
		public static bool ActiveRepeatChargeExists(long patNum) {
			
			//Counts the number of repeat charges that a patient has with a valid start date in the past and no stop date or a stop date in the future
			string command="SELECT COUNT(*) FROM repeatcharge "
				+"WHERE PatNum="+POut.Long(patNum)+" AND DateStart BETWEEN '1880-01-01' AND "+DbHelper.Curdate()+" "
				+"AND (DateStop='0001-01-01' OR DateStop>="+DbHelper.Curdate()+")";
			if(Database.ExecuteString(command)=="0") {
				return false;
			}
			return true;
		}

		///<summary>Returns true if the dates passed in from the corresponding repeat charge are active as of DateTime.Today.
		///Mimics the logic within ActiveRepeatChargeExists() in the sense that dateStart is a valid date and is before or on DateTime.Today 
		/// and that dateStop is either an invalid date (has yet to be set) OR is a valid date that is in the future.</summary>
		public static bool IsRepeatChargeActive(DateTime dateStart,DateTime dateStop) {
			if(dateStart.Year > 1880 && dateStart.Date<=DateTime.Today
				&& (dateStop.Year < 1880 || dateStop.Date>DateTime.Today)) 
			{
				return true;
			}
			return false;
		}
		
		/// <summary>Runs repeating charges for the date passed in, usually today. Can't use 'out' variables because this runs over Middle Tier.
		/// When doComputeAging=true, aging calculations will run for the families that had a repeating charge procedure added to the account.</summary>
		public static RepeatChargeResult RunRepeatingCharges(DateTime dateRun,bool doComputeAging=true) {
			
			RepeatChargeResult result=new RepeatChargeResult();
			Prefs.Set(PrefName.RepeatingChargesBeginDateTime,dateRun);
			try {
				List<RepeatCharge> listRepeatingCharges=RepeatCharges.Refresh(0).ToList();
				//Must contain all procedures that affect the date range, safe to contain too many, bad to contain too few.
				List<Procedure> listExistingProcs=Procedures.GetCompletedForDateRange(dateRun.AddMonths(-3),dateRun.AddDays(1),
					listRepeatingCharges.Select(x => x.ProcCode).Distinct().Select(x => ProcedureCodes.GetProcCode(x).CodeNum).ToList());
				DateTime startedUsingFKs=UpdateHistories.GetDateForVersion(new Version("16.1.0.0"));//We started using FKs from procs to repeat charges in 16.1.
				List<long> listAddedPatNums=new List<long>();
				OrthoCaseProcLinkingData orthoCaseProcLinkingData=new OrthoCaseProcLinkingData(listRepeatingCharges.Select(x => x.PatNum).ToList());
				foreach(RepeatCharge repeatCharge in listRepeatingCharges) {
					if(!repeatCharge.IsEnabled || (repeatCharge.DateStop.Year > 1880 && repeatCharge.DateStop.AddMonths(3) < dateRun)) {
						continue;//This repeating charge is too old to possibly create a new charge. Not precise but greatly reduces calls to DB.
										 //We will filter by more stringently on the DateStop later on.
					}
					Patient pat=null;
					List<DateTime> listBillingDates;//This list will have 1 or 2 dates where a repeating charge might be added
					if(Prefs.GetBool(PrefName.BillingUseBillingCycleDay)) {
						pat=Patients.GetPat(repeatCharge.PatNum);
						listBillingDates=GetBillingDatesHelper(repeatCharge.DateStart,repeatCharge.DateStop,dateRun,pat.BillingCycleDay);
					}
					else {
						listBillingDates=GetBillingDatesHelper(repeatCharge.DateStart,repeatCharge.DateStop,dateRun);
					}
					long codeNum=ProcedureCodes.GetCodeNum(repeatCharge.ProcCode);
					//Remove billing dates if there is a procedure from this repeat charge in that month and year
					for(int i=listBillingDates.Count-1;i>=0;i--) {//iterate backwards to remove elements
						DateTime billingDate=listBillingDates[i];
						for(int j=listExistingProcs.Count-1;j>=0;j--) {//iterate backwards to remove elements
							Procedure proc=listExistingProcs[j];
							if((proc.RepeatChargeNum==repeatCharge.RepeatChargeNum //Check the procedure's FK first
								&& IsRepeatDateHelper(repeatCharge,billingDate,proc.ProcDate,pat))
								//Use the old logic without matching FKs only if the procedure was added before updating to 16.1
								//Match patnum, codenum, fee, year, and month (IsRepeatDateHelper uses special logic to determine correct month)
								|| ((proc.ProcDate<startedUsingFKs || startedUsingFKs.Year<1880)
								&& proc.PatNum==repeatCharge.PatNum
								&& proc.CodeNum==codeNum
								&& IsRepeatDateHelper(repeatCharge,billingDate,proc.ProcDate,pat)
								&& proc.ProcFee.IsEqual(repeatCharge.ChargeAmt))) 
							{
								//This is a match to an existing procedure.
								listBillingDates.RemoveAt(i);//Removing so that a procedure will not get added on this date.
								listExistingProcs.RemoveAt(j);//Removing so that another repeat charge of the same code, date, and amount will be added.
								break;//Go to the next billing date
							}
						}
					}
					//If any billing dates have not been filtered out, add a repeating charge on those dates
					foreach(DateTime billingDate in listBillingDates) {
						Procedure procAdded=AddProcForRepeatCharge(repeatCharge,billingDate,dateRun,orthoCaseProcLinkingData);
						if(procAdded.ProcNum==0) { //error we actually don't want to add this procedure
							continue;
						}
						List<Claim> listClaimsAdded=new List<Claim>();
						if(repeatCharge.CreatesClaim && !ProcedureCodes.GetProcCode(repeatCharge.ProcCode).NoBillIns) {
							listClaimsAdded=AddClaimsHelper(repeatCharge,procAdded,orthoCaseProcLinkingData);
						}
						AllocateUnearned(repeatCharge,procAdded,billingDate);
						result.ProceduresAddedCount++;
						result.ClaimsAddedCount+=listClaimsAdded.Count;
						if(!listAddedPatNums.Contains(procAdded.PatNum)) {
							listAddedPatNums.Add(procAdded.PatNum);
						}
					}
				}
				if(doComputeAging && listAddedPatNums.Count>0) {
					List<long> listGuarantors=Patients.GetGuarantorsForPatNums(listAddedPatNums);
					DateTime dateTAgingBeganPref=DateTime.MinValue;
					DateTime dtNow=MiscData.GetNowDateTime();
					DateTime asOfDate=dateRun;
					if(Prefs.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {
						asOfDate=PrefC.GetDate(PrefName.DateLastAging);
					}
					bool isFamaging=(Prefs.GetBool(PrefName.AgingIsEnterprise) && listGuarantors.Count>1);//will only use the famaging table if more than 1 guar
					if(isFamaging) {//if this will utilize the famaging table we need to check and set the pref to block others from starting aging
						Prefs.RefreshCache();
						dateTAgingBeganPref=PrefC.GetDate(PrefName.AgingBeginDateTime);
						if(dateTAgingBeganPref>DateTime.MinValue) {//pref has been set by another process, don't run aging and notify user
							result.ErrorMsg+=Lans.g("RepeatCharges","Aging failed to run for patients who had repeat charges added to their account. This is due to "
								+"the currently running aging calculations which began on")+" "+dateTAgingBeganPref.ToString()+".  "+Lans.g("RepeatCharges","If you "
								+"believe the current aging process has finished, a user with SecurityAdmin permission can manually clear the date and time by going " 
								+"to Setup | Miscellaneous and pressing the 'Clear' button.  You will need to run aging manually once the current aging process has "
								+"finished or date and time is cleared.");
						}
						else {
							Prefs.Set(PrefName.AgingBeginDateTime,POut.DateT(dtNow,false));//get lock on pref to block others
							Signalods.SetInvalid(InvalidType.Prefs);//signal a cache refresh so other computers will have the updated pref as quickly as possible
							try {
								Ledgers.ComputeAging(listGuarantors,asOfDate);
							}
							finally {
								Prefs.Set(PrefName.AgingBeginDateTime,"");//clear lock on pref whether aging was successful or not
								Signalods.SetInvalid(InvalidType.Prefs);
							}
						}
					}
					else {//not enterprise aging or only 1 guar so not using the famaging table, just run aging as usual
						Ledgers.ComputeAging(listGuarantors,asOfDate);
					}
				}
			}
			catch(Exception ex) {
				result.ErrorMsg+=MiscUtils.GetExceptionText(ex);
			}
			finally {
				Prefs.Set(PrefName.RepeatingChargesBeginDateTime,"");
				//Even if failure, we want to update so OpenDentalService doesn't launch Repeating Charges again today.
				Prefs.Set(PrefName.RepeatingChargesLastDateTime,dateRun);
			}
			return result;
		}

		///<summary>Do not call this until after determining if the repeate charge might generate a claim.  This function checks current insurance and 
		///may not add claims if no insurance is found.</summary>
		private static List<Claim> AddClaimsHelper(RepeatCharge repeateCharge,Procedure proc,OrthoCaseProcLinkingData orthoCaseProcLinkingData) { 
			//No remoting role check; no call to db
			List<PatPlan> patPlanList=PatPlans.Refresh(repeateCharge.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(Patients.GetFamily(repeateCharge.PatNum));
			List<InsPlan> insPlanList=InsPlans.RefreshForSubList(subList);
			List<Benefit> benefitList=Benefits.Refresh(patPlanList,subList);
			List<Claim> retVal=new List<Claim>();
			Claim claimCur;
			Patient pat=Patients.GetPat(proc.PatNum);
			if(patPlanList.Count==0) {//no current insurance, do not create a claim
				return retVal;
			}
			//create the claimprocs
			Procedures.ComputeEstimates(proc,proc.PatNum,new List<ClaimProc>(),true,insPlanList,patPlanList,benefitList,pat.Age,subList
				,orthoCaseProcLinkingData.NewOrUpdatedProcLink,orthoCaseProcLinkingData.ActiveOrthoCase,orthoCaseProcLinkingData.OrthoSchedule
				,orthoCaseProcLinkingData.ListProcLinksForCase);
			//get claimprocs for this proc, may be more than one
			List<ClaimProc> claimProcList=ClaimProcs.GetForProc(ClaimProcs.Refresh(proc.PatNum),proc.ProcNum);
			string claimType="P";
			if(patPlanList.Count==1 && PatPlans.GetOrdinal(PriSecMed.Medical,patPlanList,insPlanList,subList)>0) {//if there's exactly one medical plan
				claimType="Med";
			}
			claimCur=Claims.CreateClaimForRepeatCharge(claimType,patPlanList,insPlanList,claimProcList,proc,subList,pat);
			claimProcList=ClaimProcs.Refresh(proc.PatNum);
			if(claimCur.ClaimNum==0) {
				return retVal;
			}
			retVal.Add(claimCur);
			Claims.CalculateAndUpdate(new List<Procedure> { proc },insPlanList,claimCur,patPlanList,benefitList,pat,subList);
			if(PatPlans.GetOrdinal(PriSecMed.Secondary,patPlanList,insPlanList,subList)>0 //if there exists a secondary plan
				 && !CultureInfo.CurrentCulture.Name.EndsWith("CA")) //and not canada (don't create secondary claim for canada)
			{
				claimCur=Claims.CreateClaimForRepeatCharge("S",patPlanList,insPlanList,claimProcList,proc,subList,pat);
				if(claimCur.ClaimNum==0) {
					return retVal;
				}
				retVal.Add(claimCur);
				ClaimProcs.Refresh(proc.PatNum);
				claimCur.ClaimStatus="H";
				Claims.CalculateAndUpdate(new List<Procedure> { proc },insPlanList,claimCur,patPlanList,benefitList,pat,subList);
			}
			return retVal;
		}

		///<summary>Returns 1 or 2 dates to be billed given the date range. Only filtering based on date range has been performed.</summary>
		private static List<DateTime> GetBillingDatesHelper(DateTime dateStart,DateTime dateStop,DateTime dateRun,int billingCycleDay=0) {
			//No remoting role check; no call to db
			List<DateTime> retVal=new List<DateTime>();
			if(!Prefs.GetBool(PrefName.BillingUseBillingCycleDay)) {
				billingCycleDay=dateStart.Day;
			}
			//Add dates on the first of each of the last three months
			retVal.Add(new DateTime(dateRun.AddMonths(-0).Year,dateRun.AddMonths(-0).Month,1));//current month -0
			retVal.Add(new DateTime(dateRun.AddMonths(-1).Year,dateRun.AddMonths(-1).Month,1));//current month -1
			retVal.Add(new DateTime(dateRun.AddMonths(-2).Year,dateRun.AddMonths(-2).Month,1));//current month -2
			//This loop fixes day of month, taking into account billing day past the end of the month.
			for(int i=0;i<retVal.Count;i++) {
				int billingDay=Math.Min(retVal[i].AddMonths(1).AddDays(-1).Day, billingCycleDay);
				retVal[i]=new DateTime(retVal[i].Year,retVal[i].Month,billingDay);//This re-adds the billing date with the proper day of month.
			}
			//Remove billing dates that are calulated before repeat charge started.
			retVal.RemoveAll(x => x < dateStart);
			//Remove billing dates older than one month and 20 days ago.
			retVal.RemoveAll(x => x < dateRun.AddMonths(-1).AddDays(-20));
			//Remove any dates after today
			retVal.RemoveAll(x => x > dateRun);
			//Remove billing dates past the end of the dateStop
			int monthAdd=0;
			//To account for a partial month, add a charge after the repeat charge stop date in certain circumstances (for each of these scenarios, the 
			//billingCycleDay will be 11):
			//--Scenario #1: The start day is before the stop day which is before the billing day. Ex: Start: 12/08, Stop 12/09
			//--Scenario #2: The start day is after the billing day which is after the stop day. Ex: Start: 11/25 Stop 12/01
			//--Scenario #3: The start day is before the stop day but before the billing day. Ex: Start: 11/25, Stop 11/27
			//--Scenario #4: The start day is the same as the stop day but after the billing day. Ex: Start: 10/13, Stop 11/13
			//--Scenario #5: The start day is the same as the stop day but before the billing day. Ex: Start: 11/10, Stop 12/10
			//Each of these repeat charges will post a charge on 12/11 even though it is after the stop date.
			if(Prefs.GetBool(PrefName.BillingUseBillingCycleDay)) {
				if(dateStart.Day<billingCycleDay) {
					if((dateStop.Day < billingCycleDay && dateStart.Day < dateStop.Day)//Scenario #1
						|| dateStart.Day==dateStop.Day)//Scenario #5
					{
						monthAdd=1;
					}
				}
				else if(dateStart.Day>billingCycleDay) {
					if(dateStart.Day <= dateStop.Day//Scenario #3 and #4
						|| dateStop.Day < billingCycleDay)//Scenario #2
					{
						monthAdd=1;
					}
				}
			}
			if(dateStop.Year>1880) {
				retVal.RemoveAll(x => x > dateStop.AddMonths(monthAdd));
			}
			retVal.Sort();//Order by oldest first
			return retVal;
		}

		///<summary>Inserts a procedure for the repeat charge. Possibly will allocate prepayments to the procedure.</summary>
		public static Procedure AddProcForRepeatCharge(RepeatCharge repeatCharge,DateTime billingDate,DateTime dateNow
			,OrthoCaseProcLinkingData orthoCaseProcLinkingData)
		{
			//No remoting role check; no call to db
			Procedure procedure=new Procedure();
			ProcedureCode procCode=ProcedureCodes.GetProcCode(repeatCharge.ProcCode);
			Patient pat=Patients.GetPat(repeatCharge.PatNum);
			procedure.CodeNum=procCode.CodeNum;
			procedure.ClinicNum=pat.ClinicNum;
			procedure.DateEntryC=dateNow;
			procedure.PatNum=repeatCharge.PatNum;
			procedure.ProcDate=billingDate;
			procedure.DateTP=billingDate;
			procedure.ProcFee=repeatCharge.ChargeAmt;
			procedure.ProcStatus=ProcStat.C;
			if(procCode.ProvNumDefault==0) {
				procedure.ProvNum=pat.PriProv;
			}
			else {
				procedure.ProvNum=procCode.ProvNumDefault;
			}
			procedure.MedicalCode=ProcedureCodes.GetProcCode(procedure.CodeNum).MedicalCode;
			procedure.BaseUnits=ProcedureCodes.GetProcCode(procedure.CodeNum).BaseUnits;
			procedure.DiagnosticCode=Prefs.GetString(PrefName.ICD9DefaultForNewProcs);
			procedure.RepeatChargeNum=repeatCharge.RepeatChargeNum;
			procedure.PlaceService=(PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService);//Default Proc Place of Service for the Practice is used.  
			//Check if the repeating charge has been flagged to copy it's note into the billing note of the procedure.
			if(repeatCharge.CopyNoteToProc) {
				procedure.BillingNote=repeatCharge.Note;
				if(repeatCharge.ErxAccountId!="") {
					procedure.BillingNote=
						"NPI="+repeatCharge.Npi+"  "+"ErxAccountId="+repeatCharge.ErxAccountId;
					if(!string.IsNullOrEmpty(repeatCharge.ProviderName)) {//Provider name would be empty if older and no longer updated from eRx.
						procedure.BillingNote+="\r\nProviderName="+repeatCharge.ProviderName;
					}
					if(!string.IsNullOrEmpty(repeatCharge.Note)) {
						procedure.BillingNote+="\r\n"+repeatCharge.Note;
					}
				}
			}
			if(!Prefs.GetBool(PrefName.EasyHidePublicHealth)) {
				procedure.SiteNum=pat.SiteNum;
			}
			try {
				//No recall synch needed because dental offices don't use this feature
				Procedures.Insert(procedure,isRepeatCharge:true,skipDiscountPlanAdjustment:orthoCaseProcLinkingData.CanProcLinkToOrthoCase(procedure));
				OrthoProcLinks.TryLinkProcForActiveOrthoCaseAndUpdate(orthoCaseProcLinkingData,procedure);
			}
			catch{
			}
			return procedure;
		}

		///<summary>If there are unearned paysplits and the repeat charge is set to allocate unearned, creates a payments and allocates unearned 
		///paysplits to the new payment.</summary>
		public static void AllocateUnearned(RepeatCharge repeatCharge,Procedure procedure,DateTime billingDate) {
			//No remoting role check; no call to db
			if(!repeatCharge.UsePrepay) {
				return;
			}			
			//Using Prepayments for this Procedure
			//NOTE: ProvNum=0 on these splits, so I'm pretty sure they aren't allocated to anything.
			List<ClaimProc> listClaimProcs=ClaimProcs.GetForProcs(new List<long> {procedure.ProcNum});
			List<PaySplit> prePaySplits=PaySplits.GetPrepayForFam(Patients.GetFamily(repeatCharge.PatNum),doExcludeTpPrepay:true);
			if(!string.IsNullOrEmpty(repeatCharge.UnearnedTypes)) {
				//This repeat charge is limited to certain unearned types. If repeatCharge.UnearnedTypes is empty, it is for all unearned types.
				List<long> listDefNumsUnearnedTypeCur=repeatCharge.UnearnedTypes.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries)
					.Select(x => PIn.Long(x,false)).ToList();
				prePaySplits=prePaySplits.Where(x => x.UnearnedType.In(listDefNumsUnearnedTypeCur)).ToList();
			}
			if(prePaySplits.Count==0) {
				//The family does not have an unallocated unearned pay splits that match the repeat charge's unearned types.
				return;
			}
			List<PaySplit> paySplitsForPrePaySplits=PaySplits.GetSplitsForPrepay(prePaySplits);
			Payment payCur=new Payment();
			payCur.ClinicNum=procedure.ClinicNum;
			payCur.DateEntry=billingDate;
			payCur.IsSplit=true;
			payCur.PatNum=repeatCharge.PatNum;
			payCur.PayDate=billingDate;
			payCur.PayType=0;//Income transfer (will always be income transfer)
			payCur.PayAmt=0;//Income transfer payment
			decimal payAmt=0;
			string noteText="";
			decimal procFee=ClaimProcs.GetPatPortion(procedure,listClaimProcs);
			List<PaySplit> listInsertPaySplits=new List<PaySplit>();
			List<PaySplits.PaySplitAssociated> listAssociatedSplits=new List<PaySplits.PaySplitAssociated>();
			foreach(PaySplit prePaySplit in prePaySplits) {
				prePaySplit.SplitAmt+=paySplitsForPrePaySplits.Where(x => x.FSplitNum==prePaySplit.SplitNum).Sum(y => y.SplitAmt);//Reduce prepay split amount.
				PaySplit split=new PaySplit();
				PaySplit splitNeg=new PaySplit();
				if((decimal)prePaySplit.SplitAmt > procFee-payAmt) {
					//Split amount is more than the remainder of the procfee requires, use partial from split
					split.SplitAmt=(double)(procFee-payAmt);
					splitNeg.SplitAmt=0-((double)(procFee-payAmt));
					payAmt=procFee;
				}
				else {
					//Split amount is less than or equal to the remainder of the procfee
					split.SplitAmt=prePaySplit.SplitAmt;
					splitNeg.SplitAmt=0-prePaySplit.SplitAmt;
					payAmt+=(decimal)prePaySplit.SplitAmt;
				}
				if(split.SplitAmt==0) {
					continue;//Don't make splits for 0 amount.
				}
				//Negative split, attached to prepay's prov and clinic, but not proc
				splitNeg.DateEntry=billingDate;
				splitNeg.DatePay=billingDate;
				splitNeg.PatNum=procedure.PatNum;
				listAssociatedSplits.Add(new PaySplits.PaySplitAssociated(prePaySplit,splitNeg));
				splitNeg.ProvNum=prePaySplit.ProvNum;
				splitNeg.ClinicNum=prePaySplit.ClinicNum;
				splitNeg.UnearnedType=prePaySplit.UnearnedType;
				listInsertPaySplits.Add(splitNeg);
				//Positive split, attached to proc and for proc's prov and clinic
				split.DateEntry=billingDate;
				split.DatePay=billingDate;
				split.PatNum=procedure.PatNum;
				split.ProcNum=procedure.ProcNum;
				split.ProvNum=procedure.ProvNum;
				split.ClinicNum=procedure.ClinicNum;
				listAssociatedSplits.Add(new PaySplits.PaySplitAssociated(splitNeg,split));
				if(noteText!="") {
					noteText+=", ";
				}
				noteText+=split.SplitAmt.ToString("c");
				listInsertPaySplits.Add(split);
				if(payAmt>=procFee) {
					//Break out of loop
					break;
				}
			}
			payCur.PayNote="Allocated "+noteText+" prepayments to repeating charge.";
			Payments.Insert(payCur,listInsertPaySplits,listAssociatedSplits);
		}

		///<summary>Returns true if the existing procedure was for the possibleBillingDate.</summary>
		private static bool IsRepeatDateHelper(RepeatCharge repeatCharge,DateTime possibleBillingDate,DateTime existingProcedureDate,Patient pat) {
			//No remoting role check; no call to db
			if(Prefs.GetBool(PrefName.BillingUseBillingCycleDay)) {
				pat=pat??Patients.GetPat(repeatCharge.PatNum);
				if(pat.BillingCycleDay!=existingProcedureDate.Day
					&& possibleBillingDate.AddMonths(-1).Month==existingProcedureDate.Month 
					&& possibleBillingDate.AddMonths(-1).Year==existingProcedureDate.Year
					&& existingProcedureDate>=repeatCharge.DateStart) 
				{
					DateSpan dateDiff=new DateSpan(possibleBillingDate,existingProcedureDate);
					if(dateDiff.MonthsDiff>0) {
						//return false if the last charge has been longer than a month. 
						return false;
					}
					//This is needed in case the patient's billing day changed after procedures had been added for a repeat charge.
					return true;
				}
				//Only match month and year to be equal
				return (possibleBillingDate.Month==existingProcedureDate.Month && possibleBillingDate.Year==existingProcedureDate.Year);
			}
			if(possibleBillingDate.Month!=existingProcedureDate.Month || possibleBillingDate.Year!=existingProcedureDate.Year) {
				return false;
			}
			//Iterate through dates using new logic that takes repeatCharge.DateStart.AddMonths(n) to calculate dates
			DateTime possibleDateNew=repeatCharge.DateStart;
			int dateNewMonths=0;
			//Iterate through dates using old logic that starts with repeatCharge.DateStart and adds one month at a time to calculate dates
			DateTime possibleDateOld=repeatCharge.DateStart;
			do {
				if(existingProcedureDate==possibleDateNew || existingProcedureDate==possibleDateOld) {
					return true;
				}
				dateNewMonths++;
				possibleDateNew=repeatCharge.DateStart.AddMonths(dateNewMonths);
				possibleDateOld=possibleDateOld.AddMonths(1);
			}
			while(possibleDateNew<=existingProcedureDate);
			return false;
		}
	}

	public class RepeatChargeResult {
		public int ProceduresAddedCount=0;
		public int ClaimsAddedCount=0;
		///<summary>Used to return an error message, e.g. enterprise aging blocked due to currently running calculations, so this message tells the user
		///to run aging afterward.</summary>
		public string ErrorMsg="";
	}
}










