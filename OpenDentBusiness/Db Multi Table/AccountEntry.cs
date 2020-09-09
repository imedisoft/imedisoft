﻿using CodeBase;
using Imedisoft.Data;
using Newtonsoft.Json;
using OpenDentBusiness.WebServiceMainHQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	///<summary>Separate class for keeping track of accounting transactions (procedures, payplancharges, and adjustments).
	///This should be used when you need to get a list of all transactions for a patient and sort them, eg in FormProcSelect.</summary>
	[Serializable]
	public class AccountEntry {
		[XmlIgnore]
		public object Tag;
		public DateTime Date;
		public long PriKey;
		public long PatNum;
		public long ProvNum;
		public long ClinicNum;
		public long ProcNum;
		public long AdjNum;
		public long PayPlanChargeNum;
		public long PayPlanNum;
		public long UnearnedType;
		///<summary>The original amount of the entity that this account entry represents.  Not designed to change after initial set.</summary>
		public decimal AmountOriginal;
		///<summary>The amount available for allocation or payment.  This amount is not meant for the user to change.
		///Allows the user to manipulate AmountEnd as desired and then set AmountEnd back to this value if they wish to undo or cancel.</summary>
		public decimal AmountAvailable;
		///<summary>The amount used to indicate to the user what the account entry will end up at if they perform a certain action.
		///E.g. This amount field is used to show what the entry will be at after an income transfer, or after a specific payment is made, etc.
		///This amount changes often.</summary>
		public decimal AmountEnd;
		public SplitCollection SplitCollection=new SplitCollection();

		///<summary>Returns a brief description of the current AccountEntry.  E.g:
		///"Adjustment #718410 on 01/22/2020 PatNum #1389, ProvNum #15441 (DOC1), ClinicNum #97653 (Roch) [AmtOrig: $50.00 AmountEnd: $5.55]"</summary>
		[XmlIgnore,JsonIgnore]
		public string Description {
			get {
				string description=$"{TagTypeName} #{PriKey}";
				if(GetType()==typeof(PaySplit)) {
					PaySplit paySplit=(PaySplit)Tag;
					if(paySplit.ProcNum > 0) {
						description+=$" (ProcNum #{paySplit.ProcNum})";
					}
					else if(paySplit.AdjNum > 0) {
						description+=$" (AdjNum #{paySplit.AdjNum})";
					}
					else if(paySplit.PayPlanNum > 0) {
						description+=$" (PayPlanNum #{paySplit.PayPlanNum})";
					}
					else if(paySplit.UnearnedType > 0) {
						description+=$" (UnearnedType #{paySplit.UnearnedType})";
					}
					else {
						description+=$" (unallocated)";
					}
				}
				description+=$", PatNum #{PatNum}";
				if(ProvNum > 0) {//Unearned and unallocated will sometimes have ProvNum of 0 so don't show anything.
					description+=$", ProvNum #{ProvNum} ({Providers.GetAbbr(ProvNum)})";
				}
				if(ClinicNum > 0) {
					description+=$", ClinicNum #{ClinicNum} ({Clinics.GetAbbr(ClinicNum)})";
				}
				description+=$" on {Date.ToShortDateString()} [AmtOrig: {AmountOriginal.ToString("c")} AmtEnd: {AmountEnd.ToString("c")}]";
				return description;
			}
		}

		///<summary>Returns the Name property of the type for Tag.  Returns '[NULL]' if tag has yet to be set.</summary>
		[XmlIgnore, JsonIgnore]
		public string TagTypeName {
			get {
				return GetType()?.Name??"[NULL]";
			}
		}

		///<summary>Returns true if the Tag is of type PaySplit and is allocated to a procedure or an adjustment.  Otherwise; false.
		///Note: Does not consider payment plans which is technically production.</summary>
		[XmlIgnore,JsonIgnore]
		public bool IsPaySplitAttachedToProd {
			get {
				Type type=GetType();
				if(type==null || type!=typeof(PaySplit)) {
					return false;
				}
				PaySplit paySplit=(PaySplit)Tag;
				return (paySplit.ProcNum > 0 || paySplit.AdjNum > 0);
			}
		}

		///<summary>Returns true if the Tag is of type PaySplit and is not allocated to a procedure, adjustment, unearned, or payment plan.
		///Otherwise; false</summary>
		[XmlIgnore, JsonIgnore]
		public bool IsUnallocated {
			get {
				Type type=GetType();
				if(type==null || type!=typeof(PaySplit)) {
					return false;
				}
				PaySplit paySplit=(PaySplit)Tag;
				return (paySplit.ProcNum==0
					&& paySplit.AdjNum==0
					&& paySplit.PayPlanNum==0
					&& paySplit.PayPlanChargeNum==0
					&& paySplit.UnearnedType==0);
			}
		}

		///<summary>Returns true if the Tag is of type PaySplit is allocated to unearned.  Otherwise; false</summary>
		[XmlIgnore, JsonIgnore]
		public bool IsUnearned {
			get {
				Type type=GetType();
				if(type==null || type!=typeof(PaySplit)) {
					return false;
				}
				return UnearnedType > 0;
			}
		}

		///<summary>The sum of all splits currently associated to this charge.</summary>
		[XmlIgnore, JsonIgnore]
		public double AmountPaid {
			get {
				return SplitCollection.Sum(x => x.SplitAmt);
			}
		}

		///<summary>Overridden GetType() which will return the type of the Tag field.  Returns null if the Tag field is currently null.</summary>
		public new Type GetType() {
			return Tag?.GetType()??null;
		}

		///<summary></summary>
		public AccountEntry() {
		}

		///<summary>Only for claimprocs with status NotReceived, Received, Supplemental, CapClaim.</summary>
		public AccountEntry(ClaimProc claimProc) {
			Tag=claimProc;
			Date=claimProc.DateCP;
			PriKey=claimProc.ClaimProcNum;
			if(claimProc.Status==ClaimProcStatus.NotReceived) {
				AmountOriginal=0-(decimal)(claimProc.InsPayEst+claimProc.WriteOff);
			}
			else {//Received, Supplemental, CapClaim
				AmountOriginal=0-(decimal)(claimProc.InsPayAmt+claimProc.WriteOff);
			}
			AmountAvailable=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=claimProc.ProvNum;
			ClinicNum=claimProc.ClinicNum;
			PatNum=claimProc.PatNum;
			PayPlanNum=claimProc.PayPlanNum;
			ProcNum=GetProcNumFromTag();
			AdjNum=GetAdjNumFromTag();
		}

		public AccountEntry(PayPlanCharge payPlanCharge) {
			Tag=payPlanCharge;
			Date=payPlanCharge.ChargeDate;
			PriKey=payPlanCharge.PayPlanChargeNum;
			AmountOriginal=(decimal)payPlanCharge.Principal+(decimal)payPlanCharge.Interest;
			AmountAvailable=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=payPlanCharge.ProvNum;
			ClinicNum=payPlanCharge.ClinicNum;
			PatNum=payPlanCharge.PatNum;
			PayPlanChargeNum=payPlanCharge.PayPlanChargeNum;
			PayPlanNum=payPlanCharge.PayPlanNum;
			ProcNum=GetProcNumFromTag();
			AdjNum=GetAdjNumFromTag();
		}

		///<summary>Turns negative adjustments positive.</summary>
		public AccountEntry(Adjustment adjustment) {
			Tag=adjustment;
			Date=adjustment.AdjustDate;
			PriKey=adjustment.Id;
			AmountOriginal=(decimal)adjustment.AdjustAmount;
			AmountAvailable=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=adjustment.ProviderId;
			ClinicNum=adjustment.ClinicId;
			PatNum=adjustment.PatientId;
			ProcNum=GetProcNumFromTag();
			AdjNum=GetAdjNumFromTag();
		}

		public AccountEntry(Procedure proc) {
			Tag=proc;
			Date=proc.ProcDate;
			PriKey=proc.ProcNum;
			AmountOriginal=(decimal)proc.ProcFeeTotal;
			AmountAvailable=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=proc.ProvNum;
			ClinicNum=proc.ClinicNum;
			PatNum=proc.PatNum;
			ProcNum=GetProcNumFromTag();
			AdjNum=GetAdjNumFromTag();
		}

		public AccountEntry(ProcExtended procE) {
			Tag=procE;
			Date=procE.Proc.ProcDate;
			PriKey=procE.Proc.ProcNum;
			AmountOriginal=(decimal)procE.AmountOriginal;
			AmountAvailable=AmountOriginal;
			AmountEnd=(decimal)procE.AmountEnd;
			ProvNum=procE.Proc.ProvNum;
			ClinicNum=procE.Proc.ClinicNum;
			PatNum=procE.Proc.PatNum;
			ProcNum=GetProcNumFromTag();
			AdjNum=GetAdjNumFromTag();
		}

		public AccountEntry(PaySplit paySplit) {
			Tag=paySplit;
			Date=paySplit.DatePay;
			PriKey=paySplit.SplitNum;
			AmountOriginal=0-(decimal)paySplit.SplitAmt;
			AmountAvailable=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=paySplit.ProvNum;
			SplitCollection.Add(paySplit);
			ClinicNum=paySplit.ClinicNum;
			PatNum=paySplit.PatNum;
			PayPlanChargeNum=paySplit.PayPlanChargeNum;
			PayPlanNum=paySplit.PayPlanNum;
			ProcNum=GetProcNumFromTag();
			AdjNum=GetAdjNumFromTag();
			UnearnedType=paySplit.UnearnedType;
		}

		///<summary>Similar to a claimproc, payAsTotal stores the total InsPayAmt and WriteOffAmt for a group of Pat/Prov/Clinic's on a single claim 
		///representing all of the claimprocs for that group. There is no primary key here because this is a placeholder for multiple objects.</summary>
		public AccountEntry(PayAsTotal payAsTotal) {
			Tag=payAsTotal;
			Date=payAsTotal.DateEntry;
			PriKey=0;//This is not a database object, no primary keys are available 
			AmountOriginal=0-(decimal)(payAsTotal.SummedInsPayAmt+payAsTotal.SummedWriteOff);
			AmountAvailable=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=payAsTotal.ProvNum;
			ClinicNum=payAsTotal.ClinicNum;
			PatNum=payAsTotal.PatNum;
			ProcNum=GetProcNumFromTag();
			AdjNum=GetAdjNumFromTag();
		}

		///<summary></summary>
		public AccountEntry Copy() {
			AccountEntry accountEntry=(AccountEntry)this.MemberwiseClone();
			accountEntry.SplitCollection=new SplitCollection();
			//Make a deep copy of each split within SplitCollection because it is technically an ICollection and MemberwiseClone does not handle that.
			foreach(PaySplit paySplit in SplitCollection) {
				accountEntry.SplitCollection.Add(paySplit.Copy());
			}
			return accountEntry;
		}

		///<summary>Simple sort that sorts based on date.</summary>
		private static int AccountEntrySort(AccountEntry x,AccountEntry y) {
			return x.Date.CompareTo(y.Date);
		}

		///<summary>Returns true if the account entry passed in was created from the same Tag object (procedure, adjustment, split, etc). This method equates the type and primary key of the Tag object and nothing else. This means that no amount fields are equated.</summary>
		public bool IsSameTag(AccountEntry accountEntry) {
			if(GetType()==null
				|| accountEntry.GetType()==null
				|| GetType()!=accountEntry.GetType()
				|| PriKey==0
				|| accountEntry.PriKey==0
				|| PriKey!=accountEntry.PriKey)
			{
				return false;
			}
			return true;
		}

		///<summary>Gets all charges for the current patient. Returns a list of AccountEntries.</summary>
		public static List<AccountEntry> GetAccountCharges(List<PayPlanCharge> listPayPlanCharges,List<Adjustment> listAdjustments,List<Procedure> listProcs) {
			List<AccountEntry> listCharges=new List<AccountEntry>();
			for(int i=0;i<listPayPlanCharges.Count;i++) {
					if(listPayPlanCharges[i].ChargeType==PayPlanChargeType.Debit) {
						listCharges.Add(new AccountEntry(listPayPlanCharges[i]));
					}
				}
			for(int i=0;i<listAdjustments.Count;i++) {
				if(listAdjustments[i].AdjustAmount>0 && listAdjustments[i].ProcedureId==0) {
					listCharges.Add(new AccountEntry(listAdjustments[i]));
				}
			}
			for(int i=0;i<listProcs.Count;i++) {
				listCharges.Add(new AccountEntry(listProcs[i]));
			}
			listCharges.Sort(AccountEntrySort);
			return listCharges;
		}

		///<summary>Returns the ProcNum based on the current Tag object.
		///If Tag is null or not associated to a valid AccountEntry tag type, 0 is returned.</summary>
		private long GetProcNumFromTag() {
			switch(TagTypeName) {
				case nameof(Adjustment):
					return (this.Tag as Adjustment).ProcedureId ?? 0;
				case nameof(ClaimProc):
					return (this.Tag as ClaimProc).ProcNum;
				case nameof(PayPlanCharge):
					PayPlanCharge payPlanCharge=(this.Tag as PayPlanCharge);
					long procNum=payPlanCharge.ProcNum;
					if(payPlanCharge.LinkType==PayPlanLinkType.Procedure && payPlanCharge.FKey > 0) {
						procNum=payPlanCharge.FKey;
					}
					return procNum;
				case nameof(PaySplit):
					return (this.Tag as PaySplit).ProcNum;
				case nameof(Procedure):
					return (this.Tag as Procedure).ProcNum;
				default:
					return 0;
			}
		}

		///<summary>Returns the AdjNum based on the current Tag object.
		///If Tag is null or not associated to a valid AccountEntry tag type, 0 is returned.</summary>
		private long GetAdjNumFromTag() {
			switch(TagTypeName) {
				case nameof(Adjustment):
					return (this.Tag as Adjustment).Id;
				case nameof(PaySplit):
					return (this.Tag as PaySplit).AdjNum;
				case nameof(ClaimProc):
				case nameof(PayPlanCharge):
					PayPlanCharge payPlanCharge=(this.Tag as PayPlanCharge);
					long adjNum=0;
					if(payPlanCharge.LinkType==PayPlanLinkType.Adjustment && payPlanCharge.FKey > 0) {
						adjNum=payPlanCharge.FKey;
					}
					return adjNum;
				case nameof(Procedure):
				default:
					return 0;
			}
		}

	}

	///<summary>Helper class that is used to identify a faux AccountEntry.
	///These are entities that are used to help know who and what certain credits and debits on payment plans are associated to.
	///E.g. There will be faux entries created to represent payment plan credits (e.g. procedures).
	///There will be faux entries created to represent negative payment plan credits (e.g. adjustments).</summary>
	public class FauxAccountEntry : AccountEntry {
		public long Guarantor;
		public PayPlanChargeType ChargeType;
		///<summary>True when the payplan charge is a credit or debit adjustment OR when the payplan production entry is link type adjustment.</summary>
		public bool IsAdjustment;
		///<summary>True when the faux entry was created via a PayPlanProductionEntry object (dynamic payment plan entity).</summary>
		public bool IsDynamic;
		///<summary>Never changes.  Sometimes a payment plan will have adjustments attached which need to remove value from PayPlanCharges.
		///This field simply keeps track of the Principal of the original object that this faux account entry was created from.</summary>
		public decimal Principal;
		///<summary>The value for this faux account entry (not including interest).  This value can change due to payment plan adjustments.
		///Use PrincipalOrig if you need to know what the Principal value was on the original object.</summary>
		public decimal PrincipalAdjusted;
		public decimal Interest;
		///<summary>Will be set to the corresponding procedure Account Entry if this is a patient payment plan credit attached to a proc.
		///Null if the proc couldn't be found or if this is an adjustment faux entry or if this is for a non-patient payment plan.</summary>
		public AccountEntry AccountEntryProc;

		public FauxAccountEntry(PayPlanCharge payPlanCharge,bool isPrincipal) {
			if(isPrincipal) {
				this.AmountOriginal=(decimal)payPlanCharge.Principal;
				this.Principal=(decimal)payPlanCharge.Principal;
				this.PrincipalAdjusted=Principal;
			}
			else {
				this.AmountOriginal=(decimal)payPlanCharge.Interest;
				this.Interest=(decimal)payPlanCharge.Interest;
			}
			this.AmountEnd=this.AmountOriginal;
			this.ChargeType=payPlanCharge.ChargeType;
			this.ClinicNum=payPlanCharge.ClinicNum;
			this.Date=payPlanCharge.ChargeDate;
			this.Guarantor=payPlanCharge.Guarantor;
			this.PatNum=payPlanCharge.PatNum;
			this.PayPlanChargeNum=(payPlanCharge.ChargeType==PayPlanChargeType.Credit ? 0 : payPlanCharge.PayPlanChargeNum);
			this.PayPlanNum=payPlanCharge.PayPlanNum;
			this.PriKey=payPlanCharge.PayPlanChargeNum;
			this.ProcNum=payPlanCharge.ProcNum;
			this.ProvNum=payPlanCharge.ProvNum;
			this.IsAdjustment=(payPlanCharge.IsCreditAdjustment || payPlanCharge.IsDebitAdjustment);
			if(this.IsAdjustment) {
				this.AdjNum=payPlanCharge.FKey;
				this.UnearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);
			}
			this.Tag=this;
		}

		public FauxAccountEntry(PayPlanProductionEntry payPlanProdEntry) {
			this.IsDynamic=true;
			this.AdjNum=payPlanProdEntry.GetAdjNum();
			this.AmountOriginal=(payPlanProdEntry.AmountOverride.IsZero() ? payPlanProdEntry.AmountOriginal : payPlanProdEntry.AmountOverride);
			this.AmountEnd=this.AmountOriginal;
			this.ClinicNum=payPlanProdEntry.ClinicNum;
			//Dynamic payment plans create faux account entry debits from PayPlanCharge objects so always set ChargeType to Credit.
			this.ChargeType=PayPlanChargeType.Credit;
			this.Date=payPlanProdEntry.CreditDate;
			this.Guarantor=0;
			this.Interest=0;
			this.PatNum=payPlanProdEntry.PatNum;
			this.PayPlanNum=payPlanProdEntry.LinkedCredit.PayPlanNum;
			this.PriKey=payPlanProdEntry.PriKey;
			this.Principal=this.AmountOriginal;
			this.PrincipalAdjusted=Principal;
			this.ProcNum=payPlanProdEntry.GetProcNum();
			this.ProvNum=payPlanProdEntry.ProvNum;
			this.IsAdjustment=(payPlanProdEntry.LinkType==PayPlanLinkType.Adjustment);
			this.Tag=this;
		}

		public new FauxAccountEntry Copy() {
			FauxAccountEntry accountEntry=(FauxAccountEntry)this.MemberwiseClone();
			accountEntry.SplitCollection=new SplitCollection();
			//Make a deep copy of each split within SplitCollection because it is technically an ICollection and MemberwiseClone does not handle that.
			foreach(PaySplit paySplit in SplitCollection) {
				accountEntry.SplitCollection.Add(paySplit.Copy());
			}
			return accountEntry;
		}
	}
}
