using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;
using CodeBase;
using Imedisoft.Forms;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental {
	///<summary></summary>
	public partial class FormAdjust : ODForm {
		///<summary></summary>
		public bool IsNew;
		private Patient _patCur;
		private Adjustment _adjustmentCur;
		///<summary>When true, the OK click will not let the user leave the window unless the check amount is 0.</summary>
		private bool _checkZeroAmount;
		///<summary>All positive adjustment defs.</summary>
		private List<Definition> _listAdjPosCats;
		///<summary>All negative adjustment defs.</summary>
		private List<Definition> _listAdjNegCats;
		///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers. Also includes a dummy clinic at index 0 for "none"</summary>
		//private List<Provider> _listProviders;
		private decimal _adjRemAmt;
		private bool _isTsiAdj;
		private bool _isEditAnyway;
		private List<PaySplit> _listSplitsForAdjustment;

		///<summary></summary>
		public FormAdjust(Patient patCur,Adjustment adjustmentCur,bool isTsiAdj=false){
			InitializeComponent();
			_patCur=patCur;
			_adjustmentCur=adjustmentCur;
			_isTsiAdj=isTsiAdj;
			
		}

		private void FormAdjust_Load(object sender, System.EventArgs e) {
			if(IsNew){
				if(!Security.IsAuthorized(Permissions.AdjustmentCreate,true)) {//Date not checked here.  Message will show later.
					if(!Security.IsAuthorized(Permissions.AdjustmentEditZero,true)) {//Let user create an adjustment of zero if they have this perm.
						MessageBox.Show("Not authorized for"+"\r\n"+GroupPermissions.GetDesc(Permissions.AdjustmentCreate));
						DialogResult=DialogResult.Cancel;
						return;
					}
					//Make sure amount is 0 after OK click.
					_checkZeroAmount=true;
				}
			}
			else{
				if(!Security.IsAuthorized(Permissions.AdjustmentEdit,_adjustmentCur.AdjustDate)){
					butOK.Enabled=false;
					butDelete.Enabled=false;
					//User can't edit but has edit zero amount perm.  Allow delete only if date is today.
					if(Security.IsAuthorized(Permissions.AdjustmentEditZero,true) 
						&& _adjustmentCur.AdjustAmount==0
						&& _adjustmentCur.AddedDate.Date==MiscData.GetNowDateTime().Date) 
					{
						butDelete.Enabled=true;
					}
				}
				bool isAttachedToPayPlan=PayPlanLinks.GetForFKeyAndLinkType(_adjustmentCur.Id,PayPlanLinkType.Adjustment).Count>0;
				_listSplitsForAdjustment=PaySplits.GetForAdjustments(new List<long>() {_adjustmentCur.Id});
				if(_listSplitsForAdjustment.Count>0 || isAttachedToPayPlan) {
					butAttachProc.Enabled=false;
					butDetachProc.Enabled=false;
					labelProcDisabled.Visible=true;
				}
				//Do not let the user change the adjustment type if the current adjustment is a "discount plan" adjustment type.
				if(Definitions.GetValue(DefinitionCategory.AdjTypes,_adjustmentCur.Type)=="dp") {
					labelAdditions.Text="Discount Plan"+": "+Definitions.GetName(DefinitionCategory.AdjTypes,_adjustmentCur.Type);
					labelSubtractions.Visible=false;
					listTypePos.Visible=false;
					listTypeNeg.Visible=false;
				}
			}
			textDateEntry.Text=_adjustmentCur.AddedDate.ToShortDateString();
			textAdjDate.Text=_adjustmentCur.AdjustDate.ToShortDateString();
			textProcDate.Text=_adjustmentCur.ProcedureDate.ToShortDateString();
			if(Definitions.GetValue(DefinitionCategory.AdjTypes,_adjustmentCur.Type)=="+"){//pos
				textAmount.Text=_adjustmentCur.AdjustAmount.ToString("F");
			}
			else if(Definitions.GetValue(DefinitionCategory.AdjTypes,_adjustmentCur.Type)=="-"){//neg
				textAmount.Text=(-_adjustmentCur.AdjustAmount).ToString("F");//shows without the neg sign
			}
			else if(Definitions.GetValue(DefinitionCategory.AdjTypes,_adjustmentCur.Type)=="dp") {//Discount Plan (neg)
				textAmount.Text=(-_adjustmentCur.AdjustAmount).ToString("F");//shows without the neg sign
			}
			comboClinic.SelectedClinicNum=_adjustmentCur.ClinicId;
			comboProv.SetSelectedProvNum(_adjustmentCur.ProviderId);
			FillComboProv();
			if(_adjustmentCur.ProcedureId!=0 && PrefC.GetInt(PreferenceName.RigorousAdjustments)==(int)RigorousAdjustments.EnforceFully) {
				comboProv.Enabled=false;
				butPickProv.Enabled=false;
				comboClinic.Enabled=false;
				if(Security.IsAuthorized(Permissions.Setup,true)) {
					labelEditAnyway.Visible=true;
					butEditAnyway.Visible=true;
				}
			}
			//prevents FillProcedure from being called too many times.  Event handlers hooked back up after the lists are filled.
			listTypeNeg.SelectedIndexChanged-=listTypeNeg_SelectedIndexChanged;
			listTypePos.SelectedIndexChanged-=listTypePos_SelectedIndexChanged;
			List<Definition> adjCat = Definitions.GetDefsForCategory(DefinitionCategory.AdjTypes,true);
			//Positive adjustment types
			_listAdjPosCats=adjCat.FindAll(x => x.Value=="+");
			_listAdjPosCats.ForEach(x => listTypePos.Items.Add(x.Name));
			listTypePos.SelectedIndex=_listAdjPosCats.FindIndex(x => x.Id==_adjustmentCur.Type);//can be -1
			//Negative adjustment types
			_listAdjNegCats=adjCat.FindAll(x => x.Value=="-");
			_listAdjNegCats.ForEach(x => listTypeNeg.Items.Add(x.Name));
			listTypeNeg.SelectedIndex=_listAdjNegCats.FindIndex(x => x.Id==_adjustmentCur.Type);//can be -1
			listTypeNeg.SelectedIndexChanged+=listTypeNeg_SelectedIndexChanged;
			listTypePos.SelectedIndexChanged+=listTypePos_SelectedIndexChanged;
			FillProcedure();
			textNote.Text=_adjustmentCur.Note;
		}

		private void listTypePos_SelectedIndexChanged(object sender,System.EventArgs e) {
			if(listTypePos.SelectedIndex>-1) {
				listTypeNeg.SelectedIndex=-1;
				FillProcedure();
			}
		}

		private void listTypeNeg_SelectedIndexChanged(object sender,System.EventArgs e) {
			if(listTypeNeg.SelectedIndex>-1) {
				listTypePos.SelectedIndex=-1;
				FillProcedure();
			}
		}

		private void textAmount_Validating(object sender,CancelEventArgs e) {
			FillProcedure();
		}

		private void butPickProv_Click(object sender,EventArgs e) {
			FormProviderPick FormPP = new FormProviderPick(comboProv.Items.GetAll<Provider>());
			FormPP.SelectedProviderId=comboProv.GetSelectedProvNum();
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			comboProv.SetSelectedProvNum(FormPP.SelectedProviderId);
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			
		}

		private void ComboClinic_SelectionChangeCommitted(object sender, EventArgs e){
			FillComboProv();
		}

		///<summary>Fills combo provider based on which clinic is selected and attempts to preserve provider selection if any.</summary>
		private void FillComboProv() {
			long provNum=comboProv.GetSelectedProvNum();
			comboProv.Items.Clear();
			comboProv.Items.AddProvsAbbr(Providers.GetProvsForClinic(comboClinic.SelectedClinicNum));
			comboProv.SetSelectedProvNum(provNum);
		}

		private void FillProcedure(){
			if(_adjustmentCur.ProcedureId==0) {
				textProcDate2.Text="";
				textProcProv.Text="";
				textProcTooth.Text="";
				textProcDescription.Text="";
				textProcFee.Text="";
				textProcWriteoff.Text="";
				textProcInsPaid.Text="";
				textProcInsEst.Text="";
				textProcAdj.Text="";
				textProcPatPaid.Text="";
				textProcAdjCur.Text="";
				labelProcRemain.Text="";
				_adjRemAmt=0;
				return;
			}
			Procedure procCur=Procedures.GetOneProc(_adjustmentCur.ProcedureId??0,false);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(procCur.PatNum);
			List<Adjustment> listAdjustments=Adjustments.Refresh(procCur.PatNum)
				.Where(x => x.ProcedureId==procCur.ProcNum && x.Id!=_adjustmentCur.Id).ToList();
			textProcDate.Text=procCur.ProcDate.ToShortDateString();
			textProcDate2.Text=procCur.ProcDate.ToShortDateString();
			textProcProv.Text=Providers.GetAbbr(procCur.ProvNum);
			textProcTooth.Text=Tooth.ToInternat(procCur.ToothNum);
			textProcDescription.Text=ProcedureCodes.GetById(procCur.CodeNum).Description;
			double procWO=-ClaimProcs.ProcWriteoff(listClaimProcs,procCur.ProcNum);
			double procInsPaid=-ClaimProcs.ProcInsPay(listClaimProcs,procCur.ProcNum);
			double procInsEst=-ClaimProcs.ProcEstNotReceived(listClaimProcs,procCur.ProcNum);
			double procAdj=listAdjustments.Sum(x => x.AdjustAmount);
			double procPatPaid=-PaySplits.GetTotForProc(procCur);
			textProcFee.Text=procCur.ProcFeeTotal.ToString("F");
			textProcWriteoff.Text=procWO==0?"":procWO.ToString("F");
			textProcInsPaid.Text=procInsPaid==0?"":procInsPaid.ToString("F");
			textProcInsEst.Text=procInsEst==0?"":procInsEst.ToString("F");
			textProcAdj.Text=procAdj==0?"":procAdj.ToString("F");
			textProcPatPaid.Text=procPatPaid==0?"":procPatPaid.ToString("F");
			//Intelligently sum the values above based on statuses instead of blindly adding all of the values together.
			//The remaining amount is typically called the "patient portion" so utilze the centralized method that gets the patient portion.
			decimal patPort=ClaimProcs.GetPatPortion(procCur,listClaimProcs,listAdjustments);
			double procAdjCur=0;
			if(textAmount.errorProvider1.GetError(textAmount)==""){
				if(listTypePos.SelectedIndex>-1){//pos
					procAdjCur=PIn.Double(textAmount.Text);
				}
				else if(listTypeNeg.SelectedIndex>-1 || Definitions.GetValue(DefinitionCategory.AdjTypes,_adjustmentCur.Type)=="dp"){//neg or discount plan
					procAdjCur=-PIn.Double(textAmount.Text);
				}
			}
			textProcAdjCur.Text=procAdjCur==0?"":procAdjCur.ToString("F");
			//Add the current adjustment amount to the patient portion which will give the newly calculated remaining amount.
			_adjRemAmt=(decimal)procAdjCur+(decimal)procPatPaid+patPort;
			labelProcRemain.Text=_adjRemAmt.ToString("c");
		}

		private void butAttachProc_Click(object sender, System.EventArgs e) {
			FormProcSelect formPS=new FormProcSelect(_adjustmentCur.PatientId,false);
			formPS.ShowDialog();
			if(formPS.DialogResult!=DialogResult.OK){
				return;
			}
			if(OrthoProcLinks.IsProcLinked(formPS.ListSelectedProcs[0].ProcNum)) {
				MessageBox.Show("Adjustments cannot be attached to a procedure that is linked to an ortho case.");
				return;
			}
			if(PrefC.GetInt(PreferenceName.RigorousAdjustments)<2) {//Enforce Linking
				//_selectedProvNum=FormPS.ListSelectedProcs[0].ProvNum;
				comboClinic.SelectedClinicNum=formPS.ListSelectedProcs[0].ClinicNum;
				comboProv.SetSelectedProvNum(formPS.ListSelectedProcs[0].ProvNum);
				if(PrefC.GetInt(PreferenceName.RigorousAdjustments)==(int)RigorousAdjustments.EnforceFully && !_isEditAnyway) {
					if(Security.IsAuthorized(Permissions.Setup,true)) {
						labelEditAnyway.Visible=true;
						butEditAnyway.Visible=true;
					}
					comboProv.Enabled=false;//Don't allow changing if enforce fully
					butPickProv.Enabled=false;
					comboClinic.Enabled=false;//this is a separate issue from the internal edit blocking for comboClinic with no permission for a clinic
				}
			}
			_adjustmentCur.ProcedureId=formPS.ListSelectedProcs[0].ProcNum;
			FillProcedure();
			textProcDate.Text=formPS.ListSelectedProcs[0].ProcDate.ToShortDateString();
		}

		private void butDetachProc_Click(object sender, System.EventArgs e) {
			comboProv.Enabled=true;
			butPickProv.Enabled=true;
			comboClinic.Enabled=true;
			labelEditAnyway.Visible=false;
			butEditAnyway.Visible=false;
			_adjustmentCur.ProcedureId=0;
			FillProcedure();
		}

		private void butEditAnyway_Click(object sender,EventArgs e) {
			_isEditAnyway=true;
			comboClinic.Enabled=true;
			comboProv.Enabled=true;
			butPickProv.Enabled=true;
			labelEditAnyway.Visible=false;
			butEditAnyway.Visible=false;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			bool isDiscountPlanAdj=(Definitions.GetValue(DefinitionCategory.AdjTypes,_adjustmentCur.Type)=="dp");
			if( textAdjDate.errorProvider1.GetError(textAdjDate)!=""
				|| textProcDate.errorProvider1.GetError(textProcDate)!=""
				|| textAmount.errorProvider1.GetError(textAmount)!="")
			{
				MessageBox.Show("Please fix data entry errors first.");
				return;
			}
			if(PIn.Date(textAdjDate.Text).Date > DateTime.Today.Date && !Preferences.GetBool(PreferenceName.FutureTransDatesAllowed)) {
				MessageBox.Show("Adjustment date can not be in the future.");
				return;
			}
			if(textAmount.Text==""){
				MessageBox.Show("Please enter an amount.");	
				return;
			}
			if(!isDiscountPlanAdj && listTypeNeg.SelectedIndex==-1 && listTypePos.SelectedIndex==-1){
				MessageBox.Show("Please select a type first.");
				return;
			}
			if(PrefC.GetInt(PreferenceName.RigorousAdjustments)==0 && _adjustmentCur.ProcedureId==0) {
				MessageBox.Show("You must attach a procedure to the adjustment.");
				return;
			}

			if (_adjRemAmt < 0) {
				var result = MessageBox.Show(this,
					"Remaining amount is negative. Continue?", "Overpaid Procedure Warning", 
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question);

				if (result == DialogResult.No)
				{
					return;
				}
			}

			bool changeAdjSplit=false;
			List<PaySplit> listPaySplitsForAdjust=new List<PaySplit>();
			if(IsNew){
				//prevents backdating of initial adjustment
				if(!Security.IsAuthorized(Permissions.AdjustmentCreate,PIn.Date(textAdjDate.Text),true)){//Give message later.
					if(!_checkZeroAmount) {//Let user create as long as Amount is zero and has edit zero permissions.  This was checked on load.
						MessageBox.Show("Not authorized for"+"\r\n"+GroupPermissions.GetDesc(Permissions.AdjustmentCreate));
						return;
					}
				}
			}
			else{
				//Editing an old entry will already be blocked if the date was too old, and user will not be able to click OK button
				//This catches it if user changed the date to be older.
				if(!Security.IsAuthorized(Permissions.AdjustmentEdit,PIn.Date(textAdjDate.Text))){
					return;
				}
				if(_adjustmentCur.ProviderId!=comboProv.GetSelectedProvNum()) {
					listPaySplitsForAdjust=PaySplits.GetForAdjustments(new List<long>() {_adjustmentCur.Id});
					foreach(PaySplit paySplit in listPaySplitsForAdjust) {
						if(!Security.IsAuthorized(Permissions.PaymentEdit,Payments.GetPayment(paySplit.PayNum).PayDate)) {
							return;
						}
						if(comboProv.GetSelectedProvNum()!=paySplit.ProvNum && PrefC.GetInt(PreferenceName.RigorousAccounting)==(int)RigorousAdjustments.EnforceFully) {
							changeAdjSplit=true;
							break;
						}
					}
					if(changeAdjSplit
						&& !MsgBox.Show(MsgBoxButtons.OKCancel,"The provider for the associated payment splits will be changed to match the provider on the "
						+"adjustment.")) 
					{
						return;
					}
				}
			}
			//DateEntry not allowed to change
			DateTime datePreviousChange=_adjustmentCur.LastModifiedDate;
			_adjustmentCur.AdjustDate=PIn.Date(textAdjDate.Text);
			_adjustmentCur.ProcedureDate=PIn.Date(textProcDate.Text);
			_adjustmentCur.ProviderId=comboProv.GetSelectedProvNum();
			_adjustmentCur.ClinicId=comboClinic.SelectedClinicNum;
			if(listTypePos.SelectedIndex!=-1) {
				_adjustmentCur.Type=_listAdjPosCats[listTypePos.SelectedIndex].Id;
				_adjustmentCur.AdjustAmount=PIn.Double(textAmount.Text);
			}
			if(listTypeNeg.SelectedIndex!=-1) {
				_adjustmentCur.Type=_listAdjNegCats[listTypeNeg.SelectedIndex].Id;
				_adjustmentCur.AdjustAmount=-PIn.Double(textAmount.Text);
			}
			if(isDiscountPlanAdj) {
				//AdjustmentCur.AdjType is already set to a "discount plan" adj type.
				_adjustmentCur.AdjustAmount=-PIn.Double(textAmount.Text);
			}
			if(_checkZeroAmount && _adjustmentCur.AdjustAmount!=0) {
				MessageBox.Show("Amount has to be 0.00 due to security permission.");
				return;
			}
			_adjustmentCur.Note=textNote.Text;
			try{
				if(IsNew) {
					Adjustments.Insert(_adjustmentCur);
					SecurityLogs.MakeLogEntry(Permissions.AdjustmentCreate,_adjustmentCur.PatientId,
						_patCur.GetNameLF()+", "
						+_adjustmentCur.AdjustAmount.ToString("c"));
					TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(_adjustmentCur,_isTsiAdj);
				}
				else {
					Adjustments.Update(_adjustmentCur);
					SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,_adjustmentCur.PatientId,_patCur.GetNameLF()+", "+_adjustmentCur.AdjustAmount.ToString("c"),0
						,datePreviousChange);
				}
			}
			catch(Exception ex){//even though it doesn't currently throw any exceptions
				MessageBox.Show(ex.Message);
				return;
			}
			if(changeAdjSplit) {
				PaySplits.UpdateProvForAdjust(_adjustmentCur,listPaySplitsForAdjust);
			}
			DialogResult=DialogResult.OK;
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(IsNew){
				DialogResult=DialogResult.Cancel;
			}
			else{
				if(_listSplitsForAdjustment.Count>0) {
					MessageBox.Show("Cannot delete adjustment while a payment split is attached.");
					return;
				}
				bool isAttachedToPayPlan=PayPlanLinks.GetForFKeyAndLinkType(_adjustmentCur.Id,PayPlanLinkType.Adjustment).Count>0;
				if(isAttachedToPayPlan) {
					MessageBox.Show("Cannot delete adjustment that is attached to a dynamic payment plan");
					return;
				}
				if(_listSplitsForAdjustment.Count>0 
					&& !MsgBox.Show(MsgBoxButtons.YesNo,"There are payment splits associated to this adjustment.  Do you want to continue deleting?")) 
				{//There are splits for this adjustment
					return;
				}
				SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,_adjustmentCur.PatientId
					,"Delete for patient: "+_patCur.GetNameLF()+", "+_adjustmentCur.AdjustAmount.ToString("c"),0,_adjustmentCur.LastModifiedDate);
				Adjustments.Delete(_adjustmentCur);
				DialogResult=DialogResult.OK;
			}
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		
	}

	///<summary></summary>
	public struct AdjustmentItem{
		///<summary></summary>
		public string ItemText;
		///<summary></summary>
		public int ItemIndex;
	}

}
