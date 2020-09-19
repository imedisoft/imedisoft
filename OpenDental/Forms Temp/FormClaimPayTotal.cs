using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using CodeBase;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental {
	///<summary>Summary description for FormClaimPayTotal.</summary>
	public partial class FormClaimPayTotal : ODForm {
		///<summary></summary>
		public ClaimProc[] ClaimProcsToEdit;
		private List<Procedure> ProcList;
		private Patient PatCur;
		private Family FamCur;
		private List<InsurancePlan> PlanList;
		private List<PatPlan> PatPlanList;
		private List<InsSub> SubList;
		private List<Definition> _listClaimPaymentTrackingDefs;
		private List<ClaimProc> _listClaimProcsOld;
		///<summary>True if the user has permission to edit WriteOffs based on the minimum proc.DateEntryC of the procedures to which the claimprocs
		///in the ClaimProcsToEdit array are attached.</summary>
		private bool _isWriteOffEditable;

		///<summary></summary>
		public FormClaimPayTotal(Patient patCur,Family famCur,List <InsurancePlan> planList,List<PatPlan> patPlanList,List<InsSub> subList){
			InitializeComponent();// Required for Windows Form Designer support
			FamCur=famCur;
			PatCur=patCur;
			PlanList=planList;
			SubList=subList;
			PatPlanList=patPlanList;
			
		}

		private void FormClaimPayTotal_Load(object sender, System.EventArgs e) {
			_listClaimProcsOld=new List<ClaimProc>();
			foreach(ClaimProc cp in ClaimProcsToEdit) {
				_listClaimProcsOld.Add(cp.Copy());
			}
			ProcList=Procedures.Refresh(PatCur.PatNum);
			_isWriteOffEditable=Security.IsAuthorized(Permissions.InsWriteOffEdit,
				ProcList.FindAll(x => ClaimProcsToEdit.Any(y => y.ProcNum==x.ProcNum)).Select(x => x.DateEntryC).Min());
			butWriteOff.Enabled=_isWriteOffEditable;
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				textLabFees.Visible=false;
				textDedApplied.Location=textLabFees.Location;
				textInsPayAllowed.Location=new Point(textDedApplied.Right-1,textInsPayAllowed.Location.Y);
				textInsPayAmt.Location=new Point(textInsPayAllowed.Right-1,textInsPayAllowed.Location.Y);
				textWriteOff.Location=new Point(textInsPayAmt.Right-1,textInsPayAllowed.Location.Y);
			}
			_listClaimPaymentTrackingDefs=Definitions.GetByCategory(DefinitionCategory.ClaimPaymentTracking);
			FillGrid();
		}

		private void FormClaimPayTotal_Shown(object sender,EventArgs e) {
			InsurancePlan plan=InsPlans.GetPlan(ClaimProcsToEdit[0].PlanNum,PlanList);
			if(plan.AllowedFeeSched!=0){//allowed fee sched
				gridMain.SetSelected(new Point(gridMain.Columns.GetIndex("Allowed"),0));//Allowed, first row.
			}
			else{
				gridMain.SetSelected(new Point(gridMain.Columns.GetIndex("Ins Pay"),0));//InsPay, first row.
			}
		}

		private void FillGrid(){
			//Changes made in this window do not get saved until after this window closes.
			//But if you double click on a row, then you will end up saving.  That shouldn't hurt anything, but could be improved.
			//also calculates totals for this "payment"
			//the payment itself is imaginary and is simply the sum of the claimprocs on this form
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			List<string> listDefDescripts=new List<string>();
			listDefDescripts.Add("None");
			for(int i=0;i<_listClaimPaymentTrackingDefs.Count;i++){
				listDefDescripts.Add(_listClaimPaymentTrackingDefs[i].Name);
			}
			GridColumn col=new GridColumn("Date",66);
			gridMain.Columns.Add(col);
			col=new GridColumn("Prov",50);
			gridMain.Columns.Add(col);
			if(Clinics.IsMedicalClinic(Clinics.ClinicId)) {
				col=new GridColumn("Code",75);
				gridMain.Columns.Add(col);
			}
			else {
				col=new GridColumn("Code",50);
				gridMain.Columns.Add(col);
				col=new GridColumn("Tth",25);
				gridMain.Columns.Add(col);
			}
			col=new GridColumn("Description",130);
			gridMain.Columns.Add(col);
			col=new GridColumn("Fee",62,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new GridColumn("Billed to Ins",75,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				col=new GridColumn("Labs",62,HorizontalAlignment.Right);
				gridMain.Columns.Add(col);
			}
			col=new GridColumn("Deduct",62,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			col=new GridColumn("Allowed",62,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			col=new GridColumn("Ins Pay",62,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			col=new GridColumn("Writeoff",62,HorizontalAlignment.Right,_isWriteOffEditable);
			gridMain.Columns.Add(col);
			col=new GridColumn("Status",50,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new GridColumn("Pmt",62,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new GridColumn("Pay Tracking",90){ 
				ListDisplayStrings=listDefDescripts, DropDownWidth=160 };
			gridMain.Columns.Add(col);
			col=new GridColumn("Remarks",130,true){ IsWidthDynamic=true };
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			Procedure ProcCur;
			for(int i=0;i<ClaimProcsToEdit.Length;i++){
				row=new GridRow();
				if(ClaimProcsToEdit[i].ProcNum==0) {//Total payment
					//We want to always show the "Payment Date" instead of the procedure date for total payments because they are not associated to procedures.
					row.Cells.Add(ClaimProcsToEdit[i].DateCP.ToShortDateString());
				}
				else {
					row.Cells.Add(ClaimProcsToEdit[i].ProcDate.ToShortDateString());
				}
				row.Cells.Add(Providers.GetAbbr(ClaimProcsToEdit[i].ProvNum));
				string procFee="";
				if(ClaimProcsToEdit[i].ProcNum==0) {
					row.Cells.Add("");
					if(!Clinics.IsMedicalClinic(Clinics.ClinicId)) {
						row.Cells.Add("");
					}
					row.Cells.Add("Total Payment");
				}
				else {
					ProcCur=Procedures.GetProcFromList(ProcList,ClaimProcsToEdit[i].ProcNum);//will return a new procedure if none found.
					procFee=ProcCur.ProcFeeTotal.ToString("F");
					ProcedureCode procCode=ProcedureCodes.GetById(ProcCur.CodeNum);
					row.Cells.Add(procCode.Code);
					if(!Clinics.IsMedicalClinic(Clinics.ClinicId)) {
						row.Cells.Add(ProcCur.ToothNum=="" ? Tooth.SurfTidyFromDbToDisplay(ProcCur.Surf,ProcCur.ToothNum) : Tooth.ToInternat(ProcCur.ToothNum));
					}
					string descript=procCode.Description;
					if(procCode.IsCanadianLab) {
						descript="^ ^ "+descript;
					}
					row.Cells.Add(descript);
				}
				row.Cells.Add(procFee);
				row.Cells.Add(ClaimProcsToEdit[i].FeeBilled.ToString("F"));
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					decimal labFeesForProc=0;
					List<Procedure> labFeeProcs=Procedures.GetCanadianLabFees(ClaimProcsToEdit[i].ProcNum,ProcList);
					for(int j=0;j<labFeeProcs.Count;j++) {
						labFeesForProc+=(decimal)labFeeProcs[j].ProcFee;
					}
					row.Cells.Add(labFeesForProc.ToString("F"));
				}
				row.Cells.Add(ClaimProcsToEdit[i].DedApplied.ToString("F"));
				if(ClaimProcsToEdit[i].AllowedOverride==-1){
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(ClaimProcsToEdit[i].AllowedOverride.ToString("F"));
				}
				row.Cells.Add(ClaimProcsToEdit[i].InsPayAmt.ToString("F"));
				row.Cells.Add(ClaimProcsToEdit[i].WriteOff.ToString("F"));
				switch(ClaimProcsToEdit[i].Status){
					case ClaimProcStatus.Received:
						row.Cells.Add("Recd");
						break;
					case ClaimProcStatus.NotReceived:
						row.Cells.Add("");
						break;
					//adjustment would never show here
					case ClaimProcStatus.Preauth:
						row.Cells.Add("PreA");
						break;
					case ClaimProcStatus.Supplemental:
						row.Cells.Add("Supp");
						break;
					case ClaimProcStatus.CapClaim:
						row.Cells.Add("Cap");
						break;
					//Estimate would never show here
					//Cap would never show here
				}
				if(ClaimProcsToEdit[i].ClaimPaymentNum>0){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				bool isDefPresent=false;
				for(int j=0;j<_listClaimPaymentTrackingDefs.Count;j++) {
					if(ClaimProcsToEdit[i].ClaimPaymentTracking==_listClaimPaymentTrackingDefs[j].Id) {
						row.Cells.Add(_listClaimPaymentTrackingDefs[j].Name);
						row.Cells[row.Cells.Count-1].ComboSelectedIndex=j+1;
						isDefPresent=true;
						break;
					}
				}
				if(!isDefPresent) { //The ClaimPaymentTracking definition has been hidden or ClaimPaymentTracking==0					
					row.Cells.Add("");
					row.Cells[row.Cells.Count-1].ComboSelectedIndex=0;
				}
				row.Cells.Add(ClaimProcsToEdit[i].Remarks);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			FillTotals();
		}

		private void gridMain_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			if(!SaveGridChanges()) {
				return;
			}
			List<ClaimProcHist> histList=null;
			List<ClaimProcHist> loopList=null;
			FormClaimProc FormCP=new FormClaimProc(ClaimProcsToEdit[e.Row],null,FamCur,PatCur,PlanList,histList,ref loopList,PatPlanList,false,SubList);
			FormCP.IsInClaim=true;
			//no need to worry about permissions here
			FormCP.ShowDialog();
			if(FormCP.DialogResult!=DialogResult.OK){
				return;
			}
			FillGrid();
			FillTotals();
		}

		private void gridMain_CellTextChanged(object sender,EventArgs e) {
			FillTotals();
		}

		///<Summary>Fails silently if text is in invalid format.</Summary>
		private void FillTotals(){
			double claimFee=0;
			double labFees=0;
			double dedApplied=0;
			double insPayAmtAllowed=0;
			double insPayAmt=0;
			double writeOff=0;
			//double amt;
			for(int i=0;i<gridMain.Rows.Count;i++){
				claimFee+=ClaimProcsToEdit[i].FeeBilled;//5
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					labFees+=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex("Labs")].Text);
				}
				dedApplied+=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex("Deduct")].Text);
				insPayAmtAllowed+=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex("Allowed")].Text);
				insPayAmt+=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex("Ins Pay")].Text);
				writeOff+=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex("Writeoff")].Text);
			}
			textClaimFee.Text=claimFee.ToString("F");
			textLabFees.Text=labFees.ToString("F");
			textDedApplied.Text=dedApplied.ToString("F");
			textInsPayAllowed.Text=insPayAmtAllowed.ToString("F");
			textInsPayAmt.Text=insPayAmt.ToString("F");
			textWriteOff.Text=writeOff.ToString("F");
		}

		private bool SaveGridChanges(){
			//validate all grid cells
			double dbl=0;
			int deductIdx=gridMain.Columns.GetIndex("Deduct");
			int allowedIdx=gridMain.Columns.GetIndex("Allowed");
			int insPayIdx=gridMain.Columns.GetIndex("Ins Pay");
			int writeoffIdx=gridMain.Columns.GetIndex("Writeoff");
			int statusIdx=gridMain.Columns.GetIndex("Status");
			for(int i=0;i<gridMain.Rows.Count;i++){
				try{
					//Check for invalid numbers being entered.
					if(gridMain.Rows[i].Cells[deductIdx].Text != "") {
						dbl=Convert.ToDouble(gridMain.Rows[i].Cells[deductIdx].Text);
					}
					if(gridMain.Rows[i].Cells[allowedIdx].Text != "") {
						dbl=Convert.ToDouble(gridMain.Rows[i].Cells[allowedIdx].Text);
					}
					if(gridMain.Rows[i].Cells[insPayIdx].Text != "") {
						dbl=Convert.ToDouble(gridMain.Rows[i].Cells[insPayIdx].Text);
					}
					if(gridMain.Rows[i].Cells[writeoffIdx].Text != "") {
						dbl=Convert.ToDouble(gridMain.Rows[i].Cells[writeoffIdx].Text);
						if(dbl<0 && gridMain.Rows[i].Cells[statusIdx].Text!="Supp") {
							MessageBox.Show("Only supplemental payments can have a negative writeoff.");
							return false;
						}
						double claimWriteOffTotal=ClaimProcs.GetClaimWriteOffTotal(ClaimProcsToEdit[0].ClaimNum,ClaimProcsToEdit[i].ProcNum,ClaimProcsToEdit.ToList());
						if(claimWriteOffTotal+dbl<0) {
							MessageBox.Show("The current writeoff value for supplemental payment "+(i+1).ToString()+" will cause the procedure's total writeoff to be negative.  Please change it to be at least "+(dbl-(claimWriteOffTotal+dbl)).ToString()+" to continue.");
							return false;
						}
					}
				}
				catch{
					MessageBox.Show("Invalid number.  It needs to be in 0.00 form.");
					return false;
				}
			}
			if(IsWriteOffGreaterThanProcFee()) {
				return false;
			}
			if(!isClaimProcGreaterThanProcFee()) {
				return false;
			}
			for(int i=0;i<ClaimProcsToEdit.Length;i++) {
				ClaimProcsToEdit[i].DedApplied=PIn.Double(gridMain.Rows[i].Cells[deductIdx].Text);
				if(gridMain.Rows[i].Cells[allowedIdx].Text=="") {
					ClaimProcsToEdit[i].AllowedOverride=-1;
				}
				else {
					ClaimProcsToEdit[i].AllowedOverride=PIn.Double(gridMain.Rows[i].Cells[allowedIdx].Text);
				}
				ClaimProcsToEdit[i].InsPayAmt=PIn.Double(gridMain.Rows[i].Cells[insPayIdx].Text);
				ClaimProcsToEdit[i].WriteOff=PIn.Double(gridMain.Rows[i].Cells[writeoffIdx].Text);
				int idx=gridMain.Rows[i].Cells[gridMain.Columns.GetIndex("Pay Tracking")].ComboSelectedIndex;
				ClaimProcsToEdit[i].ClaimPaymentTracking=idx==0 ? 0 : _listClaimPaymentTrackingDefs[idx-1].Id;
				ClaimProcsToEdit[i].Remarks=gridMain.Rows[i].Cells[gridMain.Columns.GetIndex("Remarks")].Text;
			}
			return true;
		}

		///<summary>Checks the ValidDouble fields below the grid and makes sure they are within their min/max values.  Returns true if valid.</summary>
		private bool ValidateTotals() {
			if(textLabFees.IsValid && textClaimFee.IsValid && textDedApplied.IsValid && textInsPayAllowed.IsValid && textInsPayAmt.IsValid && textWriteOff.IsValid) {
				return true;
			}
			return false;
		}

		/// <summary>Returns true if ClaimProcAllowCreditsGreaterThanProcFee preference allows the user to add credits greater than the proc fee. Otherwise returns false </summary>
		private bool isClaimProcGreaterThanProcFee() {
			ClaimProcCreditsGreaterThanProcFee creditsGreaterPref=(ClaimProcCreditsGreaterThanProcFee)PrefC.GetInt(PreferenceName.ClaimProcAllowCreditsGreaterThanProcFee);
			if(creditsGreaterPref==ClaimProcCreditsGreaterThanProcFee.Allow) {
				return true;
			}
			List<Procedure> listProcs=Procedures.GetManyProc(ClaimProcsToEdit.Select(x=>x.ProcNum).ToList(),false);
			List<ClaimProc> listClaimProcsForPat=ClaimProcs.Refresh(PatCur.PatNum);
			List<PaySplit> listPaySplitForSelectedCP= PaySplits.GetPaySplitsFromProcs(ClaimProcsToEdit.Select(x=>x.ProcNum).ToList());
			List<Adjustment> listAdjForSelectedCP=Adjustments.GetForProcs(ClaimProcsToEdit.Select(x=>x.ProcNum).ToList());
			bool isCreditGreater=false;
			List<string> listProcDescripts=new List<string>();
			for(int i=0;i<ClaimProcsToEdit.Length;i++) {
				ClaimProc claimProcCur=ClaimProcsToEdit[i];
				int insPayIdx=gridMain.Columns.GetIndex("Ins Pay");
				int writeoffIdx=gridMain.Columns.GetIndex("Writeoff");
				int feeAcctIdx=gridMain.Columns.GetIndex("Fee");
				decimal insPayAmt=(decimal)ClaimProcs.ProcInsPay(listClaimProcsForPat.FindAll(x => x.ClaimProcNum!=claimProcCur.ClaimProcNum),claimProcCur.ProcNum)
					+PIn.Decimal(gridMain.Rows[i].Cells[insPayIdx].Text);
				decimal writeOff=(decimal)ClaimProcs.ProcWriteoff(listClaimProcsForPat.FindAll(x => x.ClaimProcNum!=claimProcCur.ClaimProcNum),claimProcCur.ProcNum)
					+PIn.Decimal(gridMain.Rows[i].Cells[writeoffIdx].Text);
				decimal feeAcct=PIn.Decimal(gridMain.Rows[i].Cells[feeAcctIdx].Text);
				decimal adj=listAdjForSelectedCP.Where(x=>x.ProcedureId==claimProcCur.ProcNum).Select(x=>(decimal)x.AdjustAmount).Sum();
				decimal patPayAmt=listPaySplitForSelectedCP.Where(x=>x.ProcNum==claimProcCur.ProcNum).Select(x=>(decimal)x.SplitAmt).Sum();
				//Any changes to this calculation should also consider FormClaimProc.IsClaimProcGreaterThanProcFee().
				decimal creditRem=feeAcct-patPayAmt-insPayAmt-writeOff+adj;
				isCreditGreater|=(creditRem.IsLessThanZero());
				if(creditRem.IsLessThanZero()) {
					Procedure proc=listProcs.FirstOrDefault(x=>x.ProcNum==claimProcCur.ProcNum);
					listProcDescripts.Add((proc==null ? "" : ProcedureCodes.GetById(proc.CodeNum).Code)
						+"\t"+"Fee"+": "+feeAcct.ToString("F")
						+"\t"+"Credits"+": "+(Math.Abs(-patPayAmt-insPayAmt-writeOff+adj)).ToString("F")
						+"\t"+"Remaining"+": ("+Math.Abs(creditRem).ToString("F")+")");
				}
			}
			if(!isCreditGreater) {
				return true;
			}
			if(creditsGreaterPref==ClaimProcCreditsGreaterThanProcFee.Block) {
				MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste("Remaining amount is negative for the following procedures"+":\r\n"
					+string.Join("\r\n",listProcDescripts)+"\r\n"+"Not allowed to continue.");
				msgBox.Text="Overpaid Procedure Warning";
				msgBox.ShowDialog();
				return false;
			}
			if(creditsGreaterPref==ClaimProcCreditsGreaterThanProcFee.Warn) {
				return MessageBox.Show("Remaining amount is negative for the following procedures"+":\r\n"
					+string.Join("\r\n",listProcDescripts.Take(10))+"\r\n"+(listProcDescripts.Count>10?"...\r\n":"")+"Continue?"
					,"Overpaid Procedure Warning",MessageBoxButtons.OKCancel)==DialogResult.OK;
			} 
			return true;//should never get to this line, only possible if another enum value is added to allow, warn, and block
		}

		///<summary>Returns true if InsPayNoWriteoffMoreThanProc preference is turned on and the sum of write off amount is greater than the proc fee.
		///Otherwise returns false </summary>
		private bool IsWriteOffGreaterThanProcFee() {
			if(!Preferences.GetBool(PreferenceName.InsPayNoWriteoffMoreThanProc)) {
				return false;//InsPayNoWriteoffMoreThanProc preference is off. No need to check.
			}
			List<ClaimProc> listClaimProcsForPat=ClaimProcs.Refresh(PatCur.PatNum);
			List<Adjustment> listAdjustmentsForPat=Adjustments.GetForProcs(ClaimProcsToEdit.Select(x => x.ProcNum).Where(x => x!=0).ToList());
			bool isWriteoffGreater=false;
			List<string> listProcDescripts=new List<string>();
			for(int i = 0;i<ClaimProcsToEdit.Length;i++) {
				ClaimProc claimProcCur=ClaimProcsToEdit[i];
				//Fetch all adjustments for the given procedure.
				List<Adjustment> listClaimProcAdjustments=listAdjustmentsForPat.Where(x => x.ProcedureId==claimProcCur.ProcNum).ToList();
				int writeoffIdx=gridMain.Columns.GetIndex("Writeoff");
				int feeAcctIdx=gridMain.Columns.GetIndex("Fee");
				decimal writeOff=(decimal)ClaimProcs.ProcWriteoff(listClaimProcsForPat.FindAll(x => x.ClaimProcNum!=claimProcCur.ClaimProcNum),claimProcCur.ProcNum)
					+PIn.Decimal(gridMain.Rows[i].Cells[writeoffIdx].Text);
				decimal feeAcct=PIn.Decimal(gridMain.Rows[i].Cells[feeAcctIdx].Text);
				decimal adjAcct=listClaimProcAdjustments.Sum(x => (decimal)x.AdjustAmount);
				//Any changes to this calculation should also consider FormClaimProc.IsWriteOffGreaterThanProc().
				decimal writeoffRem=feeAcct-writeOff+adjAcct;
				isWriteoffGreater|=(writeoffRem.IsLessThanZero() && writeOff.IsGreaterThanZero());
				if(writeoffRem.IsLessThanZero() && writeOff.IsGreaterThanZero()) {
					Procedure proc=Procedures.GetProcFromList(ProcList,claimProcCur.ProcNum);//will return a new procedure if none found.
					listProcDescripts.Add((proc==null ? "" : ProcedureCodes.GetById(proc.CodeNum).Code)
						+"\t"+"Fee"+": "+feeAcct.ToString("F")
						+"\t"+"Adjustments"+": "+adjAcct.ToString("F")
						+"\t"+"Write-off"+": "+(Math.Abs(-writeOff)).ToString("F")
						+"\t"+"Remaining"+": ("+Math.Abs(writeoffRem).ToString("F")+")");
				}
			}
			if(isWriteoffGreater) {
				MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste("Write-off amount is greater than the adjusted procedure fee for the following "
					+"procedure(s)"+":\r\n"+string.Join("\r\n",listProcDescripts)+"\r\n"+"Not allowed to continue.");
				msgBox.Text="Excessive Write-off";
				msgBox.ShowDialog();
				return true;
			}
			return false;
		}

		private void butDeductible_Click(object sender, System.EventArgs e) {
			if(gridMain.SelectedCell.X==-1){
				MessageBox.Show("Please select one procedure.  Then click this button to assign the deductible to that procedure.");
				return;
			}
			if(!SaveGridChanges()) {
				return;
			}
			Double dedAmt=0;
			//remove the existing deductible from each procedure and move it to dedAmt.
			for(int i=0;i<ClaimProcsToEdit.Length;i++){
				if(ClaimProcsToEdit[i].DedApplied > 0){
					dedAmt+=ClaimProcsToEdit[i].DedApplied;
					ClaimProcsToEdit[i].InsPayEst+=ClaimProcsToEdit[i].DedApplied;//dedAmt might be more
					ClaimProcsToEdit[i].InsPayAmt+=ClaimProcsToEdit[i].DedApplied;
					ClaimProcsToEdit[i].DedApplied=0;
				}
			}
			if(dedAmt==0){
				MessageBox.Show("There does not seem to be a deductible to apply.  You can still apply a deductible manually by double clicking on a procedure.");
				return;
			}
			//then move dedAmt to the selected proc
			ClaimProcsToEdit[gridMain.SelectedCell.Y].DedApplied=dedAmt;
			ClaimProcsToEdit[gridMain.SelectedCell.Y].InsPayEst-=dedAmt;
			ClaimProcsToEdit[gridMain.SelectedCell.Y].InsPayAmt-=dedAmt;
			FillGrid();
		}

		public void butWriteOff_Click(object sender, System.EventArgs e) {
			DialogResult dresWriteoff=DialogResult.Cancel;
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				dresWriteoff=MessageBox.Show(
					 "Write off unpaid amounts on labs and procedures?"+"\r\n"
					+"Choose Yes to write off unpaid amounts on both labs and procedures."+"\r\n"
					+"Choose No to write off unpaid amounts on procedures only.","",MessageBoxButtons.YesNoCancel);
				if(dresWriteoff!=DialogResult.Yes && dresWriteoff!=DialogResult.No) {//Cancel
					return;
				}
			}
			else {//United States
				if(MessageBox.Show("Write off unpaid amount on each procedure?","",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
					return;
				}
			}
			if(!SaveGridChanges()) {
				return;
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && dresWriteoff==DialogResult.Yes) {//Canadian. en-CA or fr-CA
				Claim claim=Claims.GetClaim(ClaimProcsToEdit[0].ClaimNum);//There should be at least one, since a claim can only be created with one or more procedures.
				ClaimProc cpTotalLabs=new ClaimProc();
				cpTotalLabs.ClaimNum=claim.ClaimNum;
				cpTotalLabs.PatNum=claim.PatNum;
				cpTotalLabs.ProvNum=claim.ProvTreat;
				cpTotalLabs.Status=ClaimProcStatus.Received;
				cpTotalLabs.PlanNum=claim.PlanNum;
				cpTotalLabs.InsSubNum=claim.InsSubNum;
				cpTotalLabs.DateCP=DateTimeOD.Today;
				cpTotalLabs.ProcDate=claim.DateService;
				cpTotalLabs.DateEntry=DateTime.Now;
				cpTotalLabs.ClinicNum=claim.ClinicNum;
				cpTotalLabs.WriteOff=0;
				cpTotalLabs.InsPayAmt=0;
				for(int i=0;i<ClaimProcsToEdit.Length;i++) {
					ClaimProc claimProc=ClaimProcsToEdit[i];
					double procLabInsPaid=0;
					if(claimProc.InsPayAmt>claimProc.FeeBilled) {
						procLabInsPaid=claimProc.InsPayAmt-claimProc.FeeBilled;//The amount of exceess greater than the fee billed.
						claimProc.InsPayAmt=claimProc.FeeBilled;
					}
					List<Procedure> listProcLabs=Procedures.GetCanadianLabFees(claimProc.ProcNum);//0, 1 or 2 lab fees per procedure
					double procLabTotal=0;
					for(int j=0;j<listProcLabs.Count;j++) {
						procLabTotal+=listProcLabs[j].ProcFee;
					}
					if(procLabInsPaid>procLabTotal) {//Could happen if the user enters a payment amount greater than the fee billed and lab fees added together.
						procLabInsPaid=procLabTotal;
					}
					cpTotalLabs.InsPayAmt+=procLabInsPaid;
					cpTotalLabs.WriteOff+=procLabTotal-procLabInsPaid;
				}
				if(cpTotalLabs.InsPayAmt>0 || cpTotalLabs.WriteOff>0) {//These amounts will both be zero if there are no lab fees on any of the procedures.  These amounts should never be negative.
					ClaimProcs.Insert(cpTotalLabs);
				}
			}
			//fix later: does not take into account other payments.
			double unpaidAmt=0;
			List<Procedure> ProcList=Procedures.Refresh(PatCur.PatNum);
			for(int i=0;i<ClaimProcsToEdit.Length;i++){
				//ClaimProcsToEdit guaranteed to only contain claimprocs for procedures before this form loads, payments are not in the list
				unpaidAmt=Procedures.GetProcFromList(ProcList,ClaimProcsToEdit[i].ProcNum).ProcFee
					//((Procedure)Procedures.HList[ClaimProcsToEdit[i].ProcNum]).ProcFee
					-ClaimProcsToEdit[i].DedApplied
					-ClaimProcsToEdit[i].InsPayAmt;
				if(unpaidAmt > 0){
					ClaimProcsToEdit[i].WriteOff=unpaidAmt;
				}
			}
			FillGrid();
		}

		private void SaveAllowedFees(){
			//if no allowed fees entered, then nothing to do 
			bool allowedFeesEntered=false;
			for(int i=0;i<gridMain.Rows.Count;i++){
				if(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex("Allowed")].Text!=""){
					allowedFeesEntered=true;
					break;
				}
			}
			if(!allowedFeesEntered){
				return;
			}
			//if no allowed fee schedule, then nothing to do
			InsurancePlan plan=InsPlans.GetPlan(ClaimProcsToEdit[0].PlanNum,PlanList);
			if(plan.AllowedFeeSched==0){//no allowed fee sched
				//plan.PlanType!="p" && //not ppo, and 
				return;
			}
			//ask user if they want to save the fees
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Save the allowed amounts to the allowed fee schedule?")){
				return;
			}
			//select the feeSchedule
			long feeSched=-1;
			//if(plan.PlanType=="p"){//ppo
			//	feeSched=plan.FeeSched;
			//}
			//else if(plan.AllowedFeeSched!=0){//an allowed fee schedule exists
			feeSched=plan.AllowedFeeSched;
			//}
			if(FeeScheds.GetIsHidden(feeSched)){
				MessageBox.Show("Allowed fee schedule is hidden, so no changes can be made.");
				return;
			}
			Fee FeeCur=null;
			long codeNum;
			List<Procedure> ProcList=Procedures.Refresh(PatCur.PatNum);
			Procedure proc;
			List<long> invalidFeeSchedNums = new List<long>();
			for(int i=0;i<ClaimProcsToEdit.Length;i++){
				proc=Procedures.GetProcFromList(ProcList,ClaimProcsToEdit[i].ProcNum);
				codeNum=proc.CodeNum;
				//ProcNum not found or 0 for payments
				if(codeNum==0){
					continue;
				}
				if(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex("Allowed")].Text.Trim()==""//Nothing is entered in allowed 
					&& _listClaimProcsOld[i].AllowedOverride==-1) //And there was not originally a value in the allowed column
				{
					continue;
				}
				DateTime datePrevious=DateTime.MinValue;
				FeeCur=Fees.GetFee(codeNum,feeSched,proc.ClinicNum,proc.ProvNum);
				if(FeeCur==null) {
					FeeSchedule feeSchedObj=FeeScheds.GetFirst(x => x.Id==feeSched);
					FeeCur=new Fee();
					FeeCur.FeeScheduleId=feeSched;
					FeeCur.CodeNum=codeNum;
					FeeCur.ClinicNum=(feeSchedObj.IsGlobal) ? 0 : proc.ClinicNum;
					FeeCur.ProvNum=(feeSchedObj.IsGlobal) ? 0 : proc.ProvNum;
					FeeCur.Amount=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex("Allowed")].Text);
					Fees.Insert(FeeCur);
				}
				else{
					datePrevious=FeeCur.SecDateTEdit;
					FeeCur.Amount=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex("Allowed")].Text);
					Fees.Update(FeeCur);
				}
				SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,"Procedure"+": "+ProcedureCodes.GetStringProcCode(FeeCur.CodeNum)
					+", "+"Fee"+": "+FeeCur.Amount.ToString("c")+", "+"Fee Schedule"+" "+FeeScheds.GetDescription(FeeCur.FeeScheduleId)
					+". "+"Automatic change to allowed fee in Enter Payment window.  Confirmed by user.",FeeCur.CodeNum,DateTime.MinValue);
				SecurityLogs.MakeLogEntry(Permissions.LogFeeEdit,0,"Fee Updated",FeeCur.FeeNum,datePrevious);
				invalidFeeSchedNums.Add(FeeCur.FeeScheduleId);
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(!SaveGridChanges()) {
				return;
			}
			if(!ValidateTotals()) {
				MessageBox.Show("One or more column totals exceed the maximum allowed value, please fix data entry errors.");
				return;
			}
			SaveAllowedFees();
			if(Preferences.GetBool(PreferenceName.ClaimSnapshotEnabled)) {
				Claim claimCur=Claims.GetClaim(_listClaimProcsOld[0].ClaimNum);
				if(claimCur.ClaimType!="PreAuth") {
					ClaimSnapshots.CreateClaimSnapshot(_listClaimProcsOld,ClaimSnapshotTrigger.InsPayment,claimCur.ClaimType);
				}
			}
			ClaimTrackings.InsertClaimProcReceived(ClaimProcsToEdit[0].ClaimNum,Security.CurrentUser.Id);
			//Make audit trail entries if writeoff or inspayamt's were changed. The claimprocs are updated outside of this form,
			//but all of the information we need for audit trail logging lives inside the form so we do it here.
			MakeAuditTrailEntries();
			DialogResult=DialogResult.OK;
		}

		private void MakeAuditTrailEntries() {
			foreach(ClaimProc claimProc in ClaimProcsToEdit) {
				string strProcCode=ProcedureCodes.GetStringProcCode(Procedures.GetOneProc(claimProc.ProcNum,false).CodeNum);
				ClaimProc oldClaimProc=_listClaimProcsOld.FirstOrDefault(x => x.ProcNum==claimProc.ProcNum);
				if(oldClaimProc!=null) {//Shouldn't be null, but if somehow it is, do not log changes since we do not know what it changed from.
					double procOldWriteoffAmt=oldClaimProc.WriteOff;
					double procOldInsAmt=oldClaimProc.InsPayAmt;
					if(claimProc.WriteOff!=procOldWriteoffAmt) {
						SecurityLogs.MakeLogEntry(Permissions.InsWriteOffEdit,claimProc.PatNum,$"Writeoff amount for procedure {strProcCode}, " +
							$"changed from ${procOldWriteoffAmt.ToString("C")} to ${claimProc.WriteOff.ToString("C")}");
					}
					if(claimProc.InsPayAmt!=procOldInsAmt) {
						SecurityLogs.MakeLogEntry(Permissions.InsPayEdit,claimProc.PatNum,$"Insurance payment amount for procedure {strProcCode}, " +
							$"changed from ${procOldInsAmt.ToString("C")} to ${claimProc.InsPayAmt.ToString("C")}");
					}
				}
			}
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormClaimPayTotal_Activated(object sender,EventArgs e) {

		}

	
	}
}







