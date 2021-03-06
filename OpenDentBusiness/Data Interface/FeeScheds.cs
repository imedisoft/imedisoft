﻿using CodeBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using OpenDentBusiness;
using System.IO;
using System.Windows.Forms;
using Imedisoft.Data;
using Imedisoft.Data.Models;

namespace OpenDentBusiness{
	///<summary></summary>
	public class FeeScheds{
		#region Get Methods
		///<summary>Gets the fee sched from the first insplan, the patient, or the provider in that order.  Uses procProvNum if>0, otherwise pat.PriProv.
		///Either returns a fee schedule (fk to definition.DefNum) or 0.</summary>
		public static long GetFeeSched(Patient pat,List<InsurancePlan> planList,List<PatPlan> patPlans,List<InsSub> subList,long procProvNum) {
			//No need to check RemotingRole; no call to db.
			//there's not really a good place to put this function, so it's here.
			long priPlanFeeSched=0;
			PatPlan patPlanPri = patPlans.FirstOrDefault(x => x.Ordinal==1);
			if(patPlanPri!=null) {
				InsurancePlan planCur=InsPlans.GetPlan(InsSubs.GetSub(patPlanPri.InsSubNum,subList).PlanNum,planList);
				if(planCur!=null) {
					priPlanFeeSched=planCur.FeeSched;
				}
			}
			return GetFeeSched(priPlanFeeSched,pat.FeeSched,procProvNum!=0?procProvNum:pat.PriProv);//use procProvNum, but if 0 then default to pat.PriProv
		}

		///<summary>A simpler version of the same function above.  The required numbers can be obtained in a fairly simple query.
		///Might return a 0 if the primary provider does not have a fee schedule set.</summary>
		public static long GetFeeSched(long priPlanFeeSched,long patFeeSched,long provNum) {
			//No need to check RemotingRole; no call to db.
			long provFeeSched=(Providers.FirstOrDefault(x => x.Id==provNum)??new Provider()).FeeScheduleId;//defaults to 0
			return new[] { priPlanFeeSched,patFeeSched,provFeeSched }.FirstOrDefault(x => x>0);//defaults to 0 if all fee scheds are 0
		}

		///<summary>Gets the fee schedule from the primary MEDICAL insurance plan, 
		///the first insurance plan, the patient, or the provider in that order.</summary>
		public static long GetMedFeeSched(Patient pat,List<InsurancePlan> planList,List<PatPlan> patPlans,List<InsSub> subList,long procProvNum) {
			//No need to check RemotingRole; no call to db.
			long retVal = 0;
			if(PatPlans.GetInsSubNum(patPlans,1) != 0){
				//Pick the medinsplan with the ordinal closest to zero
				int planOrdinal=10; //This is a hack, but I doubt anyone would have more than 10 plans
				bool hasMedIns=false; //Keep track of whether we found a medical insurance plan, if not use dental insurance fee schedule.
				InsSub subCur;
				foreach(PatPlan patplan in patPlans){
					subCur=InsSubs.GetSub(patplan.InsSubNum,subList);
					if(patplan.Ordinal<planOrdinal && InsPlans.GetPlan(subCur.PlanNum,planList).IsMedical) {
						planOrdinal=patplan.Ordinal;
						hasMedIns=true;
					}
				}
				if(!hasMedIns) { //If this patient doesn't have medical insurance (under ordinal 10)
					return GetFeeSched(pat,planList,patPlans,subList,procProvNum);  //Use dental insurance fee schedule
				}
				subCur=InsSubs.GetSub(PatPlans.GetInsSubNum(patPlans,planOrdinal),subList);
				InsurancePlan PlanCur=InsPlans.GetPlan(subCur.PlanNum, planList);
				if (PlanCur==null){
					retVal=0;
				} 
				else {
					retVal=PlanCur.FeeSched;
				}
			}
			if (retVal==0){
				if (pat.FeeSched!=0){
					retVal=pat.FeeSched;
				} 
				else {
					if (pat.PriProv==0){
						retVal=Providers.GetFirst(true).FeeScheduleId;
					} 
					else {
						Provider providerFirst=Providers.GetFirst();//Used in order to preserve old behavior...  If this fails, then old code would have failed.
						Provider provider=Providers.FirstOrDefault(x => x.Id==pat.PriProv)??providerFirst;
						retVal=provider.FeeScheduleId;
					}
				}
			}
			return retVal;
		}
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
		///<summary>Copies one fee schedule to one or more fee schedules.  fromClinicNum, fromProvNum, and toProvNum can be zero.  Set listClinicNumsTo to copy to multiple clinic overrides.  If this list is null or empty, clinicNum 0 will be used.</summary>
		public static void CopyFeeSchedule(FeeSchedule fromFeeSched,long fromClinicNum,long fromProvNum,FeeSchedule toFeeSched,List<long> listClinicNumsTo,long toProvNum){
			
			if(listClinicNumsTo==null) {
				listClinicNumsTo=new List<long>();
			}
			if(listClinicNumsTo.Count==0) {
				listClinicNumsTo.Add(0);
			}			
			//Store a local copy of the fees from the old FeeSched
			List<Fee> listFeeLocalCopy=Fees.GetListExact(toFeeSched.Id,listClinicNumsTo,toProvNum);
			//Delete all fees that exactly match setting in "To" combo selections.
			foreach(long clinicNum in listClinicNumsTo){
				Fees.DeleteFees(toFeeSched.Id,clinicNum,toProvNum);
			}
			//Copy:
			List<Fee> listNewFees=Fees.GetListExact(fromFeeSched.Id,fromClinicNum,fromProvNum);
			int blockValue=0;
			int blockMax=(listNewFees.Count * listClinicNumsTo.Count);
			object locker=new object();
			List<Action> listActions=new List<Action>();
			foreach(long clinicNumTo in listClinicNumsTo) {
				listActions.Add(() => {
					foreach(Fee fee in listNewFees) {
						bool isReplacementFee=false;
						Fee newFee=fee.Copy();
						newFee.FeeNum=0;
						newFee.ProvNum=toProvNum;
						newFee.ClinicNum=clinicNumTo;
						newFee.FeeScheduleId=toFeeSched.Id;
						Fees.Insert(newFee);
						//Check to see if this replaced an old fee with the same fee details
						Fee oldFee=listFeeLocalCopy.Where(x => x.ProvNum==newFee.ProvNum)
							.Where(x => x.ClinicNum==newFee.ClinicNum)
							.Where(x => x.CodeNum==newFee.CodeNum)
							.Where(x => x.FeeScheduleId==newFee.FeeScheduleId)
							.FirstOrDefault();
						if(oldFee!=null)	{ 
							isReplacementFee=true;
						}
						ProcedureCode procCode=ProcedureCodes.GetById(fee.CodeNum);
						string securityLogText="Fee Schedule \""+fromFeeSched.Description+"\" copied to Fee Schedule \""+toFeeSched.Description+"\", ";
						if(clinicNumTo!=0){
							securityLogText+="To Clinic \""+Clinics.GetDescription(clinicNumTo)+"\", ";
						}
						securityLogText+="Proc Code \""+procCode.Code+"\", Fee \""+fee.Amount+"\", ";
						if(isReplacementFee) { 
							securityLogText+="Replacing Previous Fee \""+oldFee.Amount+"\"";
						}
						SecurityLogs.MakeLogEntry(Permissions.FeeSchedEdit,0,securityLogText);
						FeeSchedEvent.Fire(EventCategory.FeeSched,
							new ProgressBarHelper("Copying fees, please wait"+"...",blockValue:blockValue,blockMax:blockMax,
							progressStyle:ProgBarStyle.Continuous));
						lock(locker) {
							blockValue++;
						}
					}
				});
			}
			//Research and testing will determine whether we can run this on multiple threads.
			ODThread.RunParallel(listActions,TimeSpan.FromMinutes(30),numThreads:1);
		}

		///<summary>Replaces ImportCanadaFeeSchedule.  Imports a canadian fee schedule. Called only in FormFeeSchedTools, located here to allow unit testing. 
		///Fires FeeSchedEvents for a progress bar.</summary>
		public static List<Fee> ImportCanadaFeeSchedule2(FeeSchedule feeSched,string feeData,long clinicNum,long provNum,out int numImported,out int numSkipped) {
			//No need to check RemotingRole; no call to db.
			string[] feeLines=feeData.Split('\n');
			numImported=0;
			numSkipped=0;
			List<Fee> listFees=Fees.GetListExact(feeSched.Id,clinicNum,provNum);
			List<Fee> listFeesImported=new List<Fee>(listFees);
			for(int i=0;i<feeLines.Length;i++) {
				string[] fields=feeLines[i].Split('\t');
				if(fields.Length>1) {// && fields[1]!=""){//we no longer skip blank fees
					string procCode=fields[0];
					if(ProcedureCodes.IsValidCode(procCode)) { 
						long codeNum = ProcedureCodes.GetCodeNum(procCode);
						Fee fee=Fees.GetFee(codeNum,feeSched.Id,clinicNum,provNum,listFees);//gets best match
						if(fields[1]=="") {//an empty entry will delete an existing fee, but not insert a blank override
							if(fee==null){//nothing to do
								
							}
							else{
								//doesn't matter if the existing fee is an override or not.
								Fees.Delete(fee);
								listFeesImported.Remove(fee);
							}
						}
						else {//value found in text file
							if(fee==null){//no current fee
								fee=new Fee();
								fee.Amount=PIn.Double(fields[1],doUseEnUSFormat: true);//The fees are always in the format "1.00" so we need to parse accordingly.
								fee.FeeScheduleId=feeSched.Id;
								fee.CodeNum=codeNum;
								fee.ClinicNum=clinicNum;
								fee.ProvNum=provNum;
								Fees.Insert(fee);
								listFeesImported.Add(fee);
							}
							else{
								fee.Amount=PIn.Double(fields[1],doUseEnUSFormat: true);
								Fees.Update(fee);
							}
						}
						numImported++;
					}
					else {
						numSkipped++;
					}
					FeeSchedEvent.Fire(EventCategory.FeeSched,
					new ProgressBarHelper("Processing fees, please wait"+"...","",(numImported+numSkipped),feeLines.Length,
					ProgBarStyle.Continuous));
				}
			}
			return listFeesImported;
		}

		///<summary>Exports a fee schedule.  Called only in FormFeeSchedTools. Fires FeeSchedEvents for a progress bar.</summary>
		public static void ExportFeeSchedule(long feeSchedNum,long clinicNum,long provNum,string fileName) {
			//No need to check RemotingRole; no call to db.
			//CreateText will overwrite any content if the file already exists.
			using(StreamWriter sr=File.CreateText(fileName)) {
				//Get every single procedure code from the cache which will already be ordered by ProcCat and then ProcCode.
				//Even if the code does not have a fee, include it in the export because that will trigger a 'deletion' when importing over other schedules.
				int rowNum=0;
				List<ProcedureCode> listProcCodes=ProcedureCodes.GetListDeep();
				List<Fee> listFees=Fees.GetListForScheds(feeSchedNum,clinicNum,provNum);//gets best matches
				foreach(ProcedureCode procCode in listProcCodes) {
					//Get the best matching fee (not exact match) for the current selections. 
					Fee fee=Fees.GetFee(procCode.Id,feeSchedNum,clinicNum,provNum,listFees);
					sr.Write(procCode.Code+"\t");
					if(fee!=null && fee.Amount!=-1) {
						sr.Write(fee.Amount.ToString("n"));
					}
					sr.Write("\t");
					sr.Write(procCode.ShortDescription+"\t");
					sr.WriteLine(procCode.Description);
					double percent=((rowNum*1.0)/listProcCodes.Count*100);
					FeeSchedEvent.Fire(EventCategory.FeeSched,new ProgressBarHelper(
						"Exporting fees, please wait...",percent.ToString(),blockValue:(int)percent,progressStyle:ProgBarStyle.Continuous));
					rowNum++;
				}
			}
		}

		///<summary>Used for moving feesched items to a new location within the feesched list.</summary>
		public static void RepositionFeeSched(FeeSchedule feeSched,int newItemOrder) {
			
			string command;
			//change specific row in question.
			command="UPDATE feesched SET ItemOrder="+POut.Int(newItemOrder)+" WHERE FeeSchedNum="+POut.Long(feeSched.Id);
			Database.ExecuteNonQuery(command);
			//decrement items below old pos to close the gap, except the one we're moving
			command="UPDATE feesched SET ItemOrder=ItemOrder-1 WHERE ItemOrder >"+POut.Int(feeSched.SortOrder)
				+" AND FeeSchedNum !="+POut.Long(feeSched.Id);
			Database.ExecuteNonQuery(command);
			//increment items (move down) at or below new pos, except the one we're moving
			command="UPDATE feesched SET ItemOrder=ItemOrder+1 WHERE ItemOrder >= "+POut.Int(newItemOrder)
			 +" AND FeeSchedNum !="+POut.Long(feeSched.Id);
			Database.ExecuteNonQuery(command);
		}

		///<summary>Used for sorting feesched based on FeeSchedType followed by Description.</summary>
		public static void SortFeeSched() {
			
			string command=@"UPDATE feesched,(SELECT @neworder:=-1) a,
				(SELECT FeeSchedNum,(@neworder := @neworder+1) AS NewOrderCol
				FROM feesched
				ORDER BY FeeSchedType,Description) AS feesched2
				SET feesched.ItemOrder=feesched2.NewOrderCol
				WHERE feesched.FeeSchedNum=feesched2.FeeSchedNum";
			Database.ExecuteNonQuery(command);
		}

		///<summary>Used for checking to make sure that the feesched ItemOrder column is in sequential order.</summary>
		public static void CorrectFeeSchedOrder() {
			
			string command=@"UPDATE feesched,(SELECT @neworder:=-1) a,
				(SELECT FeeSchedNum,(@neworder := @neworder+1) AS NewOrderCol
				FROM feesched
				ORDER BY ItemOrder) AS feesched2
				SET feesched.ItemOrder=feesched2.NewOrderCol
				WHERE feesched.FeeSchedNum=feesched2.FeeSchedNum";
			Database.ExecuteNonQuery(command);
		}

		///<summary>Updates writeoff estimated for claimprocs for the passed in clinics. Called only in FormFeeSchedTools, located here to allow unit
		///testing. Requires an ODProgressExtended to display UI updates.  If clinics are enabled and the user is not clinic restricted and chooses to run
		///for all clinics, set doUpdatePrevClinicPref to true so that the ClinicNums will be stored in the preference table as they are finished to allow
		///for pausing/resuming the process.</summary>
		public static long GlobalUpdateWriteoffs(List<long> listWriteoffClinicNums,ODProgressExtended progress,bool doUpdatePrevClinicPref=false) {
			//No need to check RemotingRole; no call to db.
			long totalWriteoffsUpdated=0;
			List<Fee> listFeesHQ=Fees.GetByClinicNum(0);//All HQ fees
			Dictionary<long,List<Procedure>> dictPatProcs;
			List<FamProc> listFamProcs;
			Dictionary<long,List<ClaimProc>> dictClaimProcs;
			List<Fee> listFeesHQandClinic;
			Lookup<FeeKey2,Fee> lookupFeesByCodeAndSched;
			List<InsSub> listInsSubs;
			List<InsurancePlan> listInsPlans;
			List<PatPlan> listPatPlans;
			List<Benefit> listBenefits;
			List<Action> listActions;
			//Get all objects needed to check if procedures are linked to an orthocase here to avoid querying in loops.
			List<OrthoProcLink> listOrthoProcLinksAll=new List<OrthoProcLink>();
			Dictionary<long,OrthoProcLink> dictOrthoProcLinksAll=new Dictionary<long,OrthoProcLink>();
			Dictionary<long,OrthoCase> dictOrthoCases=new Dictionary<long,OrthoCase>();
			Dictionary<long,OrthoSchedule> dictOrthoSchedules=new Dictionary<long,OrthoSchedule>();
			OrthoCases.GetDataForAllProcLinks(ref listOrthoProcLinksAll,ref dictOrthoProcLinksAll,ref dictOrthoCases,ref dictOrthoSchedules);
			OrthoProcLink orthoProcLink=null;
			OrthoCase orthoCase=null;
			OrthoSchedule orthoSchedule=null;
			List<OrthoProcLink> listOrthoProcLinksForOrthoCase=null;
			foreach(long clinicNumCur in listWriteoffClinicNums) {
				progress.Fire(EventCategory.FeeSched,new ProgressBarHelper(Clinics.GetAbbr(clinicNumCur),"0%",0,100,ProgBarStyle.Blocks,"WriteoffProgress"));
				long rowCurIndex = 0; //reset for each clinic.
				object lockObj=new object();//used to lock rowCurIndex so the threads will correctly increment the count
				progress.Fire(EventCategory.FeeSched,new ProgressBarHelper("Getting list to update writeoffs...",
						progressBarEventType: ProgBarEventType.TextMsg));
				listFeesHQandClinic=listFeesHQ;
				if(PrefC.HasClinicsEnabled && clinicNumCur>0) {//listFeesHQ is already the fees for ClinicNum 0, only add to list if > 0
					listFeesHQandClinic.AddRange(Fees.GetByClinicNum(clinicNumCur));//could be empty for some clinics that don't use overrides
				}
				lookupFeesByCodeAndSched=(Lookup<FeeKey2,Fee>)listFeesHQandClinic.ToLookup(x => new FeeKey2(x.CodeNum,x.FeeScheduleId));
				List<Procedure> listProcsTp;
				if(PrefC.HasClinicsEnabled) {
					listProcsTp=Procedures.GetAllTp(clinicNumCur);
				}
				else {//clinics not enabled
					listProcsTp=Procedures.GetAllTp();
				}
				dictPatProcs=listProcsTp
					.GroupBy(x => x.PatNum)
					.ToDictionary(x => x.Key,x => Procedures.SortListByTreatPlanPriority(x.ToList()).ToList());
				#region Has Paused or Cancelled
				while(progress.IsPaused) {
					progress.AllowResume();
					if(progress.IsCanceled) {
						break;
					}
				}
				if(progress.IsCanceled) {
					break;
				}
				#endregion Has Paused or Cancelled
				if(dictPatProcs.Count==0) {
					continue;
				}
				int procCount=dictPatProcs.Sum(x => x.Value.Count);
				listFamProcs=Patients.GetFamilies(dictPatProcs.Keys.ToList()).Where(x => x.Guarantor!=null)
					.Select(x => new FamProc {
						GuarNum=x.Guarantor.PatNum,
						ListPatProcs=x.ListPats.Select(y => new PatProc {
							PatNum=y.PatNum,
							Age=y.Age,
							ListProcs=dictPatProcs.TryGetValue(y.PatNum,out List<Procedure> listProcsCurr)?listProcsCurr:new List<Procedure>()
						}).ToList()
					}).ToList();
				listPatPlans=PatPlans.GetPatPlansForPats(dictPatProcs.Keys.ToList());
				listInsSubs=InsSubs.GetListInsSubs(dictPatProcs.Keys.ToList());
				List<long> listInsSubNums=listInsSubs.Select(x => x.InsSubNum).ToList();
				listInsSubs.AddRange(InsSubs.GetMany(listPatPlans.Select(x => x.InsSubNum).Distinct().Where(x => !listInsSubNums.Contains(x)).ToList()));
				listInsSubs=listInsSubs.DistinctBy(x => x.InsSubNum).ToList();
				listInsPlans=InsPlans.RefreshForSubList(listInsSubs);
				listBenefits=Benefits.GetAllForPatPlans(listPatPlans,listInsSubs);
				#region Has Paused or Cancelled
				while(progress.IsPaused) {
					progress.AllowResume();
					if(progress.IsCanceled) {
						break;
					}
				}
				if(progress.IsCanceled) {
					break;
				}
				#endregion Has Paused or Cancelled
				//dictionary of key=PatNum, value=list of claimprocs, i.e. a dictionary linking each PatNum to a list of claimprocs for the given procs
				dictClaimProcs=ClaimProcs.GetForProcs(dictPatProcs.SelectMany(x => x.Value.Select(y => y.ProcNum)).ToList(),useDataReader:true)
					.GroupBy(x => x.PatNum)
					.ToDictionary(x => x.Key,x => x.ToList());
				#region Has Paused or Cancelled
				while(progress.IsPaused) {
					progress.AllowResume();
					if(progress.IsCanceled) {
						break;
					}
				}
				if(progress.IsCanceled) {
					break;
				}
				#endregion Has Paused or Cancelled
				progress.Fire(EventCategory.FeeSched,new ProgressBarHelper("Updating writeoff estimates for patients...",
						progressBarEventType: ProgBarEventType.TextMsg));
				listActions=listFamProcs.Select(x => new Action(() => {
					#region Has Cancelled
					if(progress.IsCanceled) {
						return;
					}
					#endregion Has Cancelled
					List<long> listPatNums=x.ListPatProcs.Select(y => y.PatNum).ToList();
					List<long> listInsSubNumsPatPlanCur=listPatPlans.Where(y => y.PatNum.In(listPatNums)).Select(y => y.InsSubNum).ToList();
					List<InsSub> listInsSubsCur=listInsSubs.FindAll(y => listPatNums.Contains(y.Subscriber) || y.InsSubNum.In(listInsSubNumsPatPlanCur));
					List<long> listInsSubPlanNumsCur=listInsSubsCur.Select(y => y.PlanNum).ToList();
					List<InsurancePlan> listInsPlansCur=listInsPlans.FindAll(y => listInsSubPlanNumsCur.Contains(y.Id));
					List<SubstitutionLink> listSubstitutionLinks=SubstitutionLinks.GetAllForPlans(listInsPlansCur);
					List<PatPlan> listPatPlansCur;
					List<Benefit> listBenefitsCur;
					foreach(PatProc patProc in x.ListPatProcs) {//foreach patient in the family
						if(patProc.ListProcs.IsNullOrEmpty()) {
							continue;
						}
						listPatPlansCur=listPatPlans.FindAll(y => y.PatNum==patProc.PatNum);
						List<long> listInsPlanNumsCur=listInsPlansCur.Select(y => y.Id).ToList();
						List<long> listPatPlanNumsCur=listPatPlansCur.Select(y => y.PatPlanNum).ToList();
						listBenefitsCur=listBenefits
							.FindAll(y => listInsPlanNumsCur.Contains(y.PlanNum) || listPatPlanNumsCur.Contains(y.PatPlanNum));
						listBenefitsCur.Sort(Benefits.SortBenefits);
						if(!dictClaimProcs.TryGetValue(patProc.PatNum,out List<ClaimProc> listClaimProcsCur)) {
							listClaimProcsCur=new List<ClaimProc>();
						}
						foreach(Procedure procCur in patProc.ListProcs) {//foreach proc for this patient
							OrthoCases.FillOrthoCaseObjectsForProc(procCur.ProcNum,ref orthoProcLink,ref orthoCase,ref orthoSchedule
								,ref listOrthoProcLinksForOrthoCase,dictOrthoProcLinksAll,dictOrthoCases,dictOrthoSchedules,listOrthoProcLinksAll);
							Procedures.ComputeEstimates(procCur,patProc.PatNum,ref listClaimProcsCur,false,listInsPlansCur,listPatPlansCur,listBenefitsCur,
								null,null,true,patProc.Age,listInsSubsCur,listSubstLinks:listSubstitutionLinks,lookupFees:lookupFeesByCodeAndSched,
								orthoProcLink:orthoProcLink,orthoCase:orthoCase,orthoSchedule:orthoSchedule,listOrthoProcLinksForOrthoCase:listOrthoProcLinksForOrthoCase);
							double percentage=0;
							lock(lockObj) {
								percentage=Math.Ceiling(((double)(++rowCurIndex)/procCount)*100);
							}
							progress.Fire(EventCategory.FeeSched,
								new ProgressBarHelper(Clinics.GetAbbr(clinicNumCur),(int)percentage+"%",(int)percentage,100,ProgBarStyle.Blocks,"WriteoffProgress"));
						}
					}
				})).ToList();
				ODThread.RunParallel(listActions,TimeSpan.FromHours(3),
					exceptionHandler:new ODThread.ExceptionDelegate((ex) => {
						//Notify the user what went wrong via the text box.
						progress.Fire(EventCategory.FeeSched,new ProgressBarHelper("Error updating writeoffs: "+ex.Message,
							progressBarEventType:ProgBarEventType.TextMsg));
					})
				);
				if(listWriteoffClinicNums.Count>1) {//only show if more than one clinic
					progress.Fire(EventCategory.FeeSched,
						new ProgressBarHelper(rowCurIndex+" "+"procedures processed from"+" "+Clinics.GetAbbr(clinicNumCur),
							progressBarEventType:ProgBarEventType.TextMsg));
				}
				totalWriteoffsUpdated+=rowCurIndex;
				if(doUpdatePrevClinicPref && rowCurIndex==procCount) {
					//if storing previously completed clinic and we actually completed this clinic's procs, update the pref
					if(listWriteoffClinicNums.Last()==clinicNumCur) {
						//if this is the last clinic in the list, clear the last clinic pref so the next time it will run for all clinics
						Preferences.Set(PreferenceName.GlobalUpdateWriteOffLastClinicCompleted,"");
					}
					else {
						Preferences.Set(PreferenceName.GlobalUpdateWriteOffLastClinicCompleted,POut.Long(clinicNumCur));
					}
					Signalods.SetInvalid(InvalidType.Prefs);
				}
				#region Has Cancelled
				if(progress.IsCanceled) {
					break;
				}
				#endregion Has Cancelled
			}
			progress.OnProgressDone();
			progress.Fire(EventCategory.FeeSched,new ProgressBarHelper("Writeoffs updated. "+totalWriteoffsUpdated+" procedures processed.\r\nDone.",
				progressBarEventType: ProgBarEventType.TextMsg));
			return totalWriteoffsUpdated;
		}

		#endregion

		#region CachePattern

		private class FeeSchedCache : CacheListAbs<FeeSchedule> {
			protected override List<FeeSchedule> GetCacheFromDb() {
				string command="SELECT * FROM feesched ORDER BY ItemOrder";
				return Crud.FeeSchedCrud.SelectMany(command);
			}
			protected override List<FeeSchedule> TableToList(DataTable table) {
				return Crud.FeeSchedCrud.TableToList(table);
			}
			protected override FeeSchedule Copy(FeeSchedule feeSched) {
				return feeSched.Copy();
			}
			protected override DataTable ListToTable(List<FeeSchedule> listFeeScheds) {
				return Crud.FeeSchedCrud.ListToTable(listFeeScheds,"FeeSched");
			}
			protected override void FillCacheIfNeeded() {
				FeeScheds.GetTableFromCache(false);
			}
			protected override bool IsInListShort(FeeSchedule feeSched) {
				return !feeSched.IsHidden;
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static FeeSchedCache _feeSchedCache=new FeeSchedCache();

		public static int GetCount(bool isShort=false) {
			return _feeSchedCache.GetCount(isShort);
		}

		public static List<FeeSchedule> GetDeepCopy(bool isShort=false) {
			return _feeSchedCache.GetDeepCopy(isShort);
		}

		public static FeeSchedule GetFirst(bool isShort=true) {
			return _feeSchedCache.GetFirst(isShort);
		}

		public static FeeSchedule GetFirst(Func<FeeSchedule,bool> match,bool isShort=true) {
			return _feeSchedCache.GetFirst(match,isShort);
		}

		public static FeeSchedule GetFirstOrDefault(Func<FeeSchedule,bool> match,bool isShort=false) {
			return _feeSchedCache.GetFirstOrDefault(match,isShort);
		}

		public static List<FeeSchedule> GetWhere(Predicate<FeeSchedule> match,bool isShort=false) {
			return _feeSchedCache.GetWhere(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_feeSchedCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			
			return _feeSchedCache.GetTableFromCache(doRefreshCache);
		}

		#endregion Cache Pattern

		///<summary></summary>
		public static long Insert(FeeSchedule feeSched) {
			
			//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
			feeSched.AddedBy=Security.CurrentUser.Id;
			return Crud.FeeSchedCrud.Insert(feeSched);
		}

		///<summary></summary>
		public static void Update(FeeSchedule feeSched) {
			
			Crud.FeeSchedCrud.Update(feeSched);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.</summary>
		public static bool Sync(List<FeeSchedule> listNew,List<FeeSchedule> listOld) {
			
			//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
			return Crud.FeeSchedCrud.Sync(listNew,listOld,Security.CurrentUser.Id);
		}

		///<summary>Returns the description of the fee schedule.  Appends (hidden) if the fee schedule has been hidden.</summary>
		public static string GetDescription(long feeSchedNum) {
			//No need to check RemotingRole; no call to db.
			string feeSchedDesc="";
			FeeSchedule feeSched=GetFirstOrDefault(x => x.Id==feeSchedNum);
			if(feeSched!=null) {
				feeSchedDesc=feeSched.Description+(feeSched.IsHidden ? " ("+"hidden"+")" : "");
			}
			return feeSchedDesc;
		}

		///<summary>Returns whether the FeeSched is hidden.  Defaults to true if not found.</summary>
		public static bool GetIsHidden(long feeSchedNum) {
			//No need to check RemotingRole; no call to db.
			FeeSchedule feeSched=GetFirstOrDefault(x => x.Id==feeSchedNum);
			return (feeSched==null ? true : feeSched.IsHidden);
		}

		///<summary>Returns whether the FeeSched has IsGlobal set to true.  Defaults to false if not found.</summary>
		public static bool IsGlobal(long feeSchedNum) {
			//No need to check RemotingRole; no call to db.
			FeeSchedule feeSched=GetFirstOrDefault(x => x.Id==feeSchedNum);
			return (feeSched==null ? false : feeSched.IsGlobal);
		}

		///<summary>Will return null if exact name not found.</summary>
		public static FeeSchedule GetByExactName(string description){
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.Description==description);
		}

		///<summary>Will return null if exact name not found.</summary>
		public static FeeSchedule GetByExactName(string description,FeeScheduleType feeSchedType){
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.Type==feeSchedType && x.Description==description);
		}

		///<summary>Used to find FeeScheds of a certain type from within a given list.</summary>
		public static List<FeeSchedule> GetListForType(FeeScheduleType feeSchedType,bool includeHidden,List<FeeSchedule> listFeeScheds=null) {
			//No need to check RemotingRole; no call to db.
			listFeeScheds=listFeeScheds??GetDeepCopy();
			List<FeeSchedule> retVal=new List<FeeSchedule>();
			for(int i=0;i<listFeeScheds.Count;i++) {
				if(!includeHidden && listFeeScheds[i].IsHidden){
					continue;
				}
				if(listFeeScheds[i].Type==feeSchedType){
					retVal.Add(listFeeScheds[i]);
				}
			}
			return retVal;
		}

		///<summary>Deletes FeeScheds that are hidden and not attached to any insurance plans.  Returns the number of deleted fee scheds.</summary>
		public static long CleanupAllowedScheds() {
			
			long result;
			//Detach allowed FeeSchedules from any hidden InsPlans.
			string command="UPDATE insplan "
				+"SET AllowedFeeSched=0 "
				+"WHERE IsHidden=1";
			Database.ExecuteNonQuery(command);
			//Delete unattached FeeSchedules.
			command="DELETE FROM feesched "
				+"WHERE FeeSchedNum NOT IN (SELECT AllowedFeeSched FROM insplan) "
				+"AND FeeSchedType="+POut.Int((int)FeeScheduleType.OutNetwork);
			result=Database.ExecuteNonQuery(command);
			//Delete all orphaned fees.
			command="SELECT FeeNum FROM fee "
				+"WHERE FeeSched NOT IN (SELECT FeeSchedNum FROM feesched)";
			List<long> listFeeNums=Database.GetListLong(command);
			Fees.DeleteMany(listFeeNums);
			return result;
		}

		///<summary>Hides FeeScheds that are not hidden and not in use by anything. Returns the number of fee scheds that were hidden.</summary>
		public static long HideUnusedScheds() {
			
			ODEvent.Fire(EventCategory.HideUnusedFeeSchedules,"Finding unused fee schedules...");
			string command=@"SELECT feesched.FeeSchedNum 
				FROM feesched
				LEFT JOIN provider ON provider.FeeSched=feesched.FeeSchedNum
				LEFT JOIN patient ON patient.FeeSched=feesched.FeeSchedNum
				LEFT JOIN insplan ON insplan.FeeSched=feesched.FeeSchedNum
					OR insplan.AllowedFeeSched=feesched.FeeSchedNum
					OR insplan.CopayFeeSched=feesched.FeeSchedNum
				LEFT JOIN discountplan ON discountplan.FeeSchedNum=feesched.FeeSchedNum
				WHERE COALESCE(provider.FeeSched,patient.FeeSched,insplan.FeeSched,discountplan.FeeSchedNum) IS NULL
				AND feesched.IsHidden=0";
			List<long> listFeeScheds=Database.GetListLong(command);
			if(listFeeScheds.Count==0) {
				return 0;
			}
			ODEvent.Fire(EventCategory.HideUnusedFeeSchedules,"Hiding unused fee schedules...");
			command="UPDATE feesched SET IsHidden=1 WHERE FeeSchedNum IN("+string.Join(",",listFeeScheds.Select(x => POut.Long(x)))+")";
			long rowsChanged=Database.ExecuteNonQuery(command);
			return rowsChanged;
		}

		private class FamProc {
			public long GuarNum;
			public List<PatProc> ListPatProcs=new List<PatProc>();
		}

		private class PatProc {
			public long PatNum;
			public int Age;
			public List<Procedure> ListProcs=new List<Procedure>();
		}
	}
}