using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using OpenDentBusiness;
using System.IO;
using CodeBase;

namespace OpenDental {
	public partial class FormBillingDefaults:ODForm {
		private List<Ebill> _listEbills;
		///<summary>Stale deep copy of _listEbills to use with sync.</summary>
		private List<Ebill> _listEbillsOld;
		///<summary>The eBill corresponding to the currently selected clinic if clinics are enabled.</summary>
		private Ebill _eBillCur;
		///<summary>The eBill corresponding to the default credentials.</summary>
		private Ebill _eBillDefault;
		///<summary>Corresponds to the output path/url of the 5 items in listElectBilling</summary>
		private string[] arrayOutputPaths=new string[5];
		public bool IsUserPassOnly;

		public FormBillingDefaults() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormBillingDefaults_Load(object sender,EventArgs e) {
			textDays.Text=PrefC.GetLong(PrefName.BillingDefaultsLastDays).ToString();
			checkIntermingled.Checked=PrefC.GetBool(PrefName.BillingDefaultsIntermingle);
			checkSinglePatient.Checked=PrefC.GetBool(PrefName.BillingDefaultsSinglePatient);
			textNote.Text=PrefC.GetString(PrefName.BillingDefaultsNote);
			checkCreatePDF.Checked=PrefC.GetBool(PrefName.BillingElectCreatePDF);
			checkBoxBillShowTransSinceZero.Checked=PrefC.GetBool(PrefName.BillingShowTransSinceBalZero);
			listElectBilling.SelectedIndex=0;
			int billingUseElectronicIdx=PrefC.GetInt(PrefName.BillingUseElectronic);
			if(billingUseElectronicIdx==1) {
				listElectBilling.SelectedIndex=1;
				checkCreatePDF.Enabled=true;
				labelBlankForDefault.Visible=true;
			}
			if(billingUseElectronicIdx==2) {
				listElectBilling.SelectedIndex=2;
			}
			if(billingUseElectronicIdx==3) {
				checkCreatePDF.Enabled=true;
				listElectBilling.SelectedIndex=3;
			}
			if(billingUseElectronicIdx==4) {
				listElectBilling.SelectedIndex=4;
			}
			arrayOutputPaths[0]="";//Will never be used, but is helpful to keep the indexes of arrayOutputPaths aligned with the options listed in listElectBilling.
			arrayOutputPaths[1]=PrefC.GetString(PrefName.BillingElectStmtUploadURL);
			arrayOutputPaths[2]=PrefC.GetString(PrefName.BillingElectStmtOutputPathPos);
			arrayOutputPaths[3]=PrefC.GetString(PrefName.BillingElectStmtOutputPathClaimX);
			arrayOutputPaths[4]=PrefC.GetString(PrefName.BillingElectStmtOutputPathEds);
			textStatementURL.Text=arrayOutputPaths[billingUseElectronicIdx];
			textVendorId.Text=PrefC.GetString(PrefName.BillingElectVendorId);
			textVendorPMScode.Text=PrefC.GetString(PrefName.BillingElectVendorPMSCode);
			string cc=PrefC.GetString(PrefName.BillingElectCreditCardChoices);
			if(cc.Contains("MC")) {
				checkMC.Checked=true;
			}
			if(cc.Contains("V")) {
				checkV.Checked=true;
			}
			if(cc.Contains("D")) {
				checkD.Checked=true;
			}
			if(cc.Contains("A")) {
				checkAmEx.Checked=true;
			}
			textBillingEmailSubject.Text=PrefC.GetString(PrefName.BillingEmailSubject);
			textBillingEmailBody.Text=PrefC.GetString(PrefName.BillingEmailBodyText);
			textInvoiceNote.Text=PrefC.GetString(PrefName.BillingDefaultsInvoiceNote);
			_listEbills=Ebills.GetDeepCopy();
			_listEbillsOld=_listEbills.Select(x => x.Copy()).ToList();
			//Find the default Ebill
			for(int i=0;i<_listEbills.Count;i++) {
				if(_listEbills[i].ClinicNum==0) {
					_eBillDefault=_listEbills[i];
				}
			}
			if(_eBillDefault==null) {
				MessageBox.Show("The default ebill entry is missing. Run "+nameof(DatabaseMaintenances.EbillMissingDefaultEntry)
					+" in the Database Maintenance Tool before continuing.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			_eBillCur=_eBillDefault;
			//Set the textboxes to default values.
			textClientAcctNumber.Text=_eBillDefault.ClientAcctNumber;
			textUserName.Text=_eBillDefault.ElectUserName;
			textPassword.Text=_eBillDefault.ElectPassword;
			string[] arrayEbillAddressEnums=Enum.GetNames(typeof(EbillAddress));
			for(int i=0;i<arrayEbillAddressEnums.Length;i++) {
				comboPracticeAddr.Items.Add(arrayEbillAddressEnums[i]);
				comboRemitAddr.Items.Add(arrayEbillAddressEnums[i]);
				//If clinics are off don't add the Clinic specific EbillAddress enums
				if(!PrefC.HasClinicsEnabled && i==2) {
					break;
				}
			}
			if(PrefC.HasClinicsEnabled) {
				//Bold clinic specific fields.
				groupBoxBilling.Text=Lan.g(this,"Electronic Billing - Bolded fields are clinic specific");
				labelAcctNum.Font=new Font(labelAcctNum.Font,FontStyle.Bold);
				labelUserName.Font=new Font(labelUserName.Font,FontStyle.Bold);
				labelPassword.Font=new Font(labelPassword.Font,FontStyle.Bold);
				labelPracticeAddr.Font=new Font(labelPracticeAddr.Font,FontStyle.Bold);
				labelRemitAddr.Font=new Font(labelRemitAddr.Font,FontStyle.Bold);
				comboClinic.SelectedClinicNum=Clinics.ClinicNum;
				Ebill eBill=null;
				if(Clinics.ClinicNum==0) {//Use the default Ebill if OD has Headquarters selected or if clinics are disabled.
					eBill=_eBillDefault;
				}
				else {
					eBill=_listEbills.FirstOrDefault(x => x.ClinicNum==comboClinic.SelectedClinicNum);//Can be null.
				}
				//_eBillCur will be the default Ebill, the clinic's Ebill, or null if there are no existing ebills for OD's selected clinic.
				_eBillCur=eBill;
			}
			listModesToText.Items.Clear();
			foreach(StatementMode stateMode in Enum.GetValues(typeof(StatementMode))) {
				listModesToText.Items.Add(new ODBoxItem<StatementMode>(Lan.g("enumStatementMode",stateMode.GetDescription()),stateMode));
			}
			foreach(string modeIdx in PrefC.GetString(PrefName.BillingDefaultsModesToText)
				.Split(new string[] { "," },StringSplitOptions.RemoveEmptyEntries)) 
			{ 
				listModesToText.SetSelected(PIn.Int(modeIdx),true);
			}
			textSmsTemplate.Text=PrefC.GetString(PrefName.BillingDefaultsSmsTemplate);
			//Load _eBillCur's fields into the UI.
			LoadEbill(_eBillCur);
		}

		///<summary>eBill can be null, creates Ebill if needed.</summary>
		private void LoadEbill(Ebill eBill) {
			if(eBill==null) {//Matching Ebill entry not found.  Make a new entry with default values.
				eBill=new Ebill();
				eBill.ClinicNum=comboClinic.SelectedClinicNum;
				eBill.ClientAcctNumber="";
				eBill.ElectUserName="";
				eBill.ElectPassword="";
				eBill.PracticeAddress=EbillAddress.PracticePhysical;
				eBill.RemitAddress=EbillAddress.PracticeBilling;
				_listEbills.Add(eBill);
			}
			textClientAcctNumber.Text=_eBillDefault.ClientAcctNumber;
			if(eBill.ClientAcctNumber!="") {//If the Ebill field is blank use default value.
				textClientAcctNumber.Text=eBill.ClientAcctNumber;
			}
			textUserName.Text=_eBillDefault.ElectUserName;
			if(eBill.ElectUserName!="") {//If the Ebill field is blank use default value.
				textUserName.Text=eBill.ElectUserName;
			}
			textPassword.Text=_eBillDefault.ElectPassword;
			if(eBill.ElectPassword!="") {//If the Ebill field is blank use default value.
				textPassword.Text=eBill.ElectPassword;
			}
			//If clinics are disabled and the eBill had a clinic specific enum, set it to default value.  May happen if clinics were previously enabled.
			if(PrefC.HasClinicsEnabled) {
				comboPracticeAddr.SelectedIndex=(int)eBill.PracticeAddress;
				comboRemitAddr.SelectedIndex=(int)eBill.RemitAddress;
			}
			else {//No clinics
				if(eBill.PracticeAddress==EbillAddress.ClinicPhysical) {
					comboPracticeAddr.SelectedIndex=0;//PracticePhysical
				}
				else if(eBill.PracticeAddress==EbillAddress.ClinicBilling) {
					comboPracticeAddr.SelectedIndex=1;//PracticeBilling
				}
				else if(eBill.PracticeAddress==EbillAddress.ClinicPayTo) {
					comboPracticeAddr.SelectedIndex=2;//PracticePayTo
				}
				else {
					comboPracticeAddr.SelectedIndex=(int)eBill.PracticeAddress;
				}
				if(eBill.RemitAddress==EbillAddress.ClinicPhysical) {
					comboRemitAddr.SelectedIndex=0;//PracticePhysical
				}
				else if(eBill.RemitAddress==EbillAddress.ClinicBilling) {
					comboRemitAddr.SelectedIndex=1;//PracticeBilling
				}
				else if(eBill.RemitAddress==EbillAddress.ClinicPayTo) {
					comboRemitAddr.SelectedIndex=2;//PracticePayTo
				}
				else {
					comboRemitAddr.SelectedIndex=(int)eBill.RemitAddress;
				}
			}
			_eBillCur=eBill;
			if(IsUserPassOnly) {
				Controls.OfType<Control>().ToList().ForEach(x => x.Enabled=false);
				groupBoxBilling.Enabled=true;
				butOK.Enabled=true;
				butCancel.Enabled=true;
				this.Text=Lan.g(this,"Billing Defaults")+" - {"+Lan.g(this,"Limited")+"}";
			}
		}

		///<summary>Saves the current Ebill information from the UI into the cache.</summary>
		private void SaveEbill(Ebill eBill) {
			if(eBill.ClinicNum==0) {//If the ebill being edited is for the defaults use what's in the text
				eBill.ClientAcctNumber=textClientAcctNumber.Text;
				eBill.ElectUserName=textUserName.Text;
				eBill.ElectPassword=textPassword.Text;
			}			
			else {//If the ebill isn't the default
				if(textClientAcctNumber.Text!="" && textClientAcctNumber.Text!=_eBillDefault.ClientAcctNumber) {
					eBill.ClientAcctNumber=textClientAcctNumber.Text;
				}
				else {//Text was blank or the same as the default, blank it.
					eBill.ClientAcctNumber="";
				}
				if(textUserName.Text!="" && textUserName.Text!=_eBillDefault.ElectUserName) {
					eBill.ElectUserName=textUserName.Text;
				}
				else {//Text was blank or the same as the default, blank it.
					eBill.ElectUserName="";
				}
				if(textPassword.Text!="" && textPassword.Text!=_eBillDefault.ElectPassword) {
					eBill.ElectPassword=textPassword.Text;
				}
				else {//Text was blank or the same as the default, blank it.
					eBill.ElectPassword="";
				}
			}
			eBill.PracticeAddress=(EbillAddress)comboPracticeAddr.SelectedIndex;
			eBill.RemitAddress=(EbillAddress)comboRemitAddr.SelectedIndex;
		}

		private void textStatementURL_KeyUp(object sender,KeyEventArgs e) {
			arrayOutputPaths[listElectBilling.SelectedIndex]=textStatementURL.Text;
		}
		
		private void listElectBilling_SelectedIndexChanged(object sender,EventArgs e) {
			//If Dental X Change is selected, enable its textboxes and combo.
			if(listElectBilling.SelectedIndex==1) {
				comboRemitAddr.Enabled=true;
				textUserName.ReadOnly=false;
				textPassword.ReadOnly=false;
				textClientAcctNumber.ReadOnly=false;
				textVendorId.ReadOnly=false;
				textVendorPMScode.ReadOnly=false;
				labelBlankForDefault.Visible=true;
				labelStatementURL.Text=Lan.g(this,"URL Override");
			}
			else {
				//If Dental X Change is not selected, disable changing information for fields the selected format won't use.
				comboRemitAddr.Enabled=false;
				textUserName.ReadOnly=true;
				textPassword.ReadOnly=true;
				textClientAcctNumber.ReadOnly=true;
				textVendorId.ReadOnly=true;
				textVendorPMScode.ReadOnly=true;
				labelBlankForDefault.Visible=false;
				labelStatementURL.Text=Lan.g(this,"Output Path");
			}
			textStatementURL.Text=arrayOutputPaths[listElectBilling.SelectedIndex];
			if(listElectBilling.SelectedIndex==1 || listElectBilling.SelectedIndex==3) {
				checkCreatePDF.Enabled=true;
			}
			else {
				checkCreatePDF.Enabled=false;
			}
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			SaveEbill(_eBillCur);
			Ebill eBill=null;
			if((!Security.CurUser.ClinicIsRestricted || Clinics.ClinicNum==0) && comboClinic.IsUnassignedSelected) {
				eBill=_eBillDefault;
			}
			else {//Otherwise locate the Ebill from the cache.
				for(int i=0;i<_listEbills.Count;i++) {
					if(_listEbills[i].ClinicNum==comboClinic.SelectedClinicNum) {//Check for existing Ebill entry
						eBill=_listEbills[i];
						break;
					}
				}
			}
			LoadEbill(eBill);//Could be null if user switches to a clinic which has not Ebill entry yet.
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDays.errorProvider1.GetError(textDays)!=""){
				MessageBox.Show("Please fix data entry errors first.");
				return;
			}
			if(listElectBilling.SelectedIndex.In(2,3,4) && !Directory.Exists(textStatementURL.Text)){
				MessageBox.Show("Please choose a valid Output Path.");
				return;
			}
			if(checkSinglePatient.Checked && checkIntermingled.Checked) {
				MessageBox.Show("Cannot select both 'Intermingle family members' and 'Single patient only' as defaults.");
				return;
			}
			string cc="";
			if(checkMC.Checked) {
				cc="MC";
			}
			if(checkV.Checked) {
				if(cc!="") {
					cc+=",";
				}
				cc+="V";
			}
			if(checkD.Checked) {
				if(cc!="") {
					cc+=",";
				}
				cc+="D";
			}
			if(checkAmEx.Checked) {
				if(cc!="") {
					cc+=",";
				}
				cc+="A";
			}
			string billingUseElectronic=listElectBilling.SelectedIndex.ToString();
			SaveEbill(_eBillCur);
			if(listElectBilling.SelectedIndex==1 && string.IsNullOrEmpty(textStatementURL.Text)) {
				textStatementURL.Text=@"https://claimconnect.dentalxchange.com/dci/upload.svl";//default value from before 16.2.19
			}
			string modesToText=string.Join(",",listModesToText.GetListSelected<StatementMode>().Select(x => POut.Int((int)x)));
			if(Prefs.UpdateLong(PrefName.BillingDefaultsLastDays,PIn.Long(textDays.Text))
				| Prefs.UpdateBool(PrefName.BillingDefaultsIntermingle,checkIntermingled.Checked)
				| Prefs.UpdateString(PrefName.BillingDefaultsNote,textNote.Text)
				| Prefs.UpdateString(PrefName.BillingUseElectronic,billingUseElectronic)
				| Prefs.UpdateString(PrefName.BillingEmailSubject,textBillingEmailSubject.Text)
				| Prefs.UpdateString(PrefName.BillingEmailBodyText,textBillingEmailBody.Text)
				| Prefs.UpdateString(PrefName.BillingElectVendorId,textVendorId.Text)
				| Prefs.UpdateString(PrefName.BillingElectVendorPMSCode,textVendorPMScode.Text)
				| Prefs.UpdateString(PrefName.BillingElectCreditCardChoices,cc)
				| Prefs.UpdateString(PrefName.BillingDefaultsInvoiceNote,textInvoiceNote.Text)
				| Prefs.UpdateBool(PrefName.BillingElectCreatePDF,checkCreatePDF.Checked)
				| (listElectBilling.SelectedIndex==1 && Prefs.UpdateString(PrefName.BillingElectStmtUploadURL,textStatementURL.Text))
				| (listElectBilling.SelectedIndex==2 && Prefs.UpdateString(PrefName.BillingElectStmtOutputPathPos,textStatementURL.Text))
				| (listElectBilling.SelectedIndex==3 && Prefs.UpdateString(PrefName.BillingElectStmtOutputPathClaimX,textStatementURL.Text))
				| (listElectBilling.SelectedIndex==4 && Prefs.UpdateString(PrefName.BillingElectStmtOutputPathEds,textStatementURL.Text))
				| Prefs.UpdateBool(PrefName.BillingDefaultsSinglePatient,checkSinglePatient.Checked)
				| Prefs.UpdateString(PrefName.BillingDefaultsModesToText,modesToText)
				| Prefs.UpdateString(PrefName.BillingDefaultsSmsTemplate,textSmsTemplate.Text)
				| Prefs.UpdateBool(PrefName.BillingShowTransSinceBalZero,checkBoxBillShowTransSinceZero.Checked))
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			if(Ebills.Sync(_listEbills,_listEbillsOld)) {//Includes the default Ebill
				DataValid.SetInvalid(InvalidType.Ebills);//Also updates cache.
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	
	}
}