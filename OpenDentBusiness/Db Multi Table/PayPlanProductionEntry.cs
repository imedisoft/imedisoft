﻿using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	///<summary>Helper class for keeping track of information on payment plans, linked production </summary>
	public class PayPlanProductionEntry {
		public PayPlanLink LinkedCredit;
		public decimal AmountOriginal;
		public decimal AmountOverride;
		///<summary>Amount that still needs to be made into payplan charges (debited).</summary>
		public decimal AmountRemaining;
		[XmlIgnore]
		public object ProductionTag;
		//The following fields are accessors to fields within the ProductionTag object.
		public DateTime ProductionDate;
		///<summary>Contains the primary key for the production entry (ProcNum,AdjNum...etc)</summary>
		public long PriKey;
		public long ProvNum;
		public long ClinicNum;
		public long PatNum;
		///<summary>credit.SecDateTEntry.  Will be MinValue if this credit is new (not inserted into the db yet).</summary>
		public DateTime CreditDate;
		///<summary>A short description of the production entry.  Currently supports procedures and adjustments.
		///Procedures description will be the proc code followed by the layman term or full description.
		///Adjustments description will be 'Adjustment' followed by the textual representation of the adjustment type.</summary>
		public string Description;
		public PayPlanLinkType LinkType;

		///<summary>Construct a payplanproductionentry item for a procedure. Calculates pat portion.</summary>
		public PayPlanProductionEntry(Procedure proc,PayPlanLink credit,List<ClaimProc> listClaimProcs,List<Adjustment> listAdjustments
			,List<PaySplit> listPaySplits) 
		{
			ProductionTag=proc;
			LinkedCredit=credit;
			ProductionDate=proc.ProcDate;
			PriKey=proc.ProcNum;
			ProvNum=proc.ProvNum;
			ClinicNum=proc.ClinicNum;
			PatNum=proc.PatNum;
			decimal patPortion=ClaimProcs.GetPatPortion(proc,listClaimProcs,listAdjustments);
			//Get the amount that was paid to the procedure prior to the procedure being attached to a payment plan.
			decimal patPaid=(decimal)listPaySplits.FindAll(x => x.ProcNum==proc.ProcNum && x.PayPlanNum==0 && x.PayPlanChargeNum==0).Sum(x => x.SplitAmt);
			AmountOriginal=patPortion-patPaid;
			AmountOverride=(decimal)credit.AmountOverride;
			AmountRemaining=(AmountOverride==0)?AmountOriginal:AmountOverride;
			CreditDate=credit.SecDateTEntry;
			Description=$"{ProcedureCodes.GetStringProcCode(proc.CodeNum)} - {ProcedureCodes.GetLaymanTerm(proc.CodeNum)}";
			LinkType=PayPlanLinkType.Procedure;
		}

		///<summary>Construct a payplanproductionentry for an UNATTACHED adjustment (attached adjustments get treated as procedures).</summary>
		public PayPlanProductionEntry(Adjustment adj,PayPlanLink credit,List<PaySplit> listPaySplits) {
			ProductionTag=adj;
			LinkedCredit=credit;
			ProductionDate=adj.AdjustDate;
			PriKey=adj.Id;
			ProvNum=adj.ProviderId;
			ClinicNum=adj.ClinicId;
			PatNum=adj.PatientId;
			//Get the amount that was paid to the adjustment prior to the adjustment being attached to a payment plan.
			decimal patPaid=(decimal)listPaySplits.FindAll(x => x.AdjNum==adj.Id && x.PayPlanNum==0 && x.PayPlanChargeNum==0).Sum(x => x.SplitAmt);
			AmountOriginal=(decimal)adj.AdjustAmount-patPaid;
			AmountOverride=(decimal)credit.AmountOverride;
			AmountRemaining=(AmountOverride==0)?AmountOriginal:AmountOverride;//Gets set when calculating
			CreditDate=credit.SecDateTEntry;
			Description=$"Adjustment - {Definitions.GetName(DefinitionCategory.AdjTypes,adj.Type)}";
			LinkType=PayPlanLinkType.Adjustment;
		}

		public decimal GetAmountAttached() {
			if(!this.AmountOverride.IsEqual(0)) {
				return this.AmountOverride;
			}
			return this.AmountOriginal;
		}

		///<summary>Used as a short way to grab the procNum from an adjustment attached to a procedure. Returns 0 if no procedure is attached.</summary>
		public long GetProcNum() {
			if(this.LinkType==PayPlanLinkType.Procedure) {
				return this.PriKey;//just as safeguard in case is called with this link type
			}
			if(this.LinkType==PayPlanLinkType.Adjustment) {
				return ((Adjustment)this.ProductionTag).ProcedureId ?? 0;
			}
			return 0;
		}

		///<summary>Used as a short way to grab the AdjNum from an adjustment. Returns 0 if not an adjustment.</summary>
		public long GetAdjNum() {
			if(this.LinkType==PayPlanLinkType.Adjustment) {
				return ((Adjustment)this.ProductionTag).Id;
			}
			return 0;
		}

		public static List<PayPlanProductionEntry> GetWithAmountRemaining(List<PayPlanLink> listPayPlanLinks,List<PayPlanCharge> listChargesInDB) {
			//calculate remaining amounts for attached production
			List<PayPlanProductionEntry> listCreditsAndProduction=GetProductionForLinks(listPayPlanLinks);//will need to account for newly added
			foreach(PayPlanProductionEntry entry in listCreditsAndProduction) { 
			//find amount remaining for each credit/production object. This will be our basis for caculating estimated remaining charges. 
				if(entry.AmountRemaining.IsEqual(0)) {
					continue;
				}
				List<PayPlanCharge> listChargesInDbForEntry=listChargesInDB.FindAll(x => x.LinkType==entry.LinkType && x.FKey==entry.PriKey);
				foreach(PayPlanCharge chargeForEntry in listChargesInDbForEntry) {
					entry.AmountRemaining-=Math.Min((decimal)chargeForEntry.Principal,entry.AmountRemaining);
					if(entry.AmountRemaining.IsEqual(0)) {
						break;
					}
				}
			}
			listCreditsAndProduction.RemoveAll(x => x.AmountRemaining.IsEqual(0));//only keep the ones we still need to make charges for. 
			return listCreditsAndProduction.OrderBy(x => x.CreditDate).ToList();//It is important to consumers that the result be ordered by CreditDate.
		}

		public static List<PayPlanProductionEntry> GetProductionForLinks(List<PayPlanLink> listCredits) {
			//No remoting role check; no call to db
			List<long> listProcNums=listCredits.FindAll(x => x.LinkType==PayPlanLinkType.Procedure).Select(x => x.FKey).ToList();
			List<long> listAdjNumsForCredits=listCredits.FindAll(x => x.LinkType==PayPlanLinkType.Adjustment).Select(x => x.FKey).ToList();
			List<PayPlanProductionEntry> listPayPlanProductionEntries=new List<PayPlanProductionEntry>(); 
			List<Procedure> listProcedures=Procedures.GetManyProc(listProcNums,false);
			List<Adjustment> listCreditAdjustments=Adjustments.GetMany(listAdjNumsForCredits);
			List<Adjustment> listProcAdjustments=Adjustments.GetForProcs(listProcNums);
			List<ClaimProc> listClaimProcs=ClaimProcs.GetForProcs(listProcNums);//used for calculating patient porition
			List<PaySplit> listAdjPaySplits=PaySplits.GetForAdjustments(listAdjNumsForCredits);
			List<PaySplit> listProcPaySplits=PaySplits.GetPaySplitsFromProcs(listProcNums);
			foreach(PayPlanLink credit in listCredits){
				if(credit.LinkType==PayPlanLinkType.Procedure) {
					Procedure proc=listProcedures.FirstOrDefault(x => x.ProcNum==credit.FKey);
					if(proc!=null) {
						listPayPlanProductionEntries.Add(new PayPlanProductionEntry(proc,credit,listClaimProcs,listProcAdjustments,listProcPaySplits));
					}
				}
				else if(credit.LinkType==PayPlanLinkType.Adjustment) {
					Adjustment adj=listCreditAdjustments.FirstOrDefault(x => x.Id==credit.FKey);
					if(adj!=null) {
						listPayPlanProductionEntries.Add(new PayPlanProductionEntry(adj,credit,listAdjPaySplits));
					}
				}
			}
			return listPayPlanProductionEntries;
		}
	}
}
