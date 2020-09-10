using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using Imedisoft.UI;
using OpenDentBusiness;
using OpenDentBusiness.IO;

namespace OpenDental {
	public partial class FormCreditCardEdit:ODForm {
		private Patient PatCur;
		private List<PayPlan> PayPlanList;
		private CreditCard _creditCardOld;
		public CreditCard CreditCardCur;
		///<summary>True if X-Charge is enabled.  Recurring charge section will show if using X-Charge.</summary>
		private bool _isXChargeEnabled;
		///<summary>True if PayConnect is enabled.  Recurring charge section will show if using PayConnect.</summary>
		private bool _isPayConnectEnabled;
		///<summary>True if PaySimple is enabled.  Recurring charge section will show if using PaySimple.</summary>
		private bool _isPaySimpleEnabled;

		public FormCreditCardEdit(Patient pat) {
			InitializeComponent();
			
			PatCur=pat;
			_isXChargeEnabled=Programs.IsEnabled(ProgramName.Xcharge);
			_isPayConnectEnabled=Programs.IsEnabled(ProgramName.PayConnect);
			_isPaySimpleEnabled=Programs.IsEnabled(ProgramName.PaySimple);
		}

		private void FormCreditCardEdit_Load(object sender, EventArgs e)
		{
			_creditCardOld = CreditCardCur.Clone();
			FillFrequencyCombos();
			FillData();
			FillPayTypeCombo();
			checkExcludeProcSync.Checked = CreditCardCur.ExcludeProcSync;
			if ((_isXChargeEnabled || _isPayConnectEnabled || _isPaySimpleEnabled)
				&& (!CreditCardCur.IsXWeb() && !CreditCardCur.IsPayConnectPortal()))
			{//Get recurring payment plan information if using X-Charge or PayConnect and the card is not from XWeb or PayConnectPortal.
				PayPlanList = PayPlans.GetValidPlansNoIns(PatCur.PatNum);
				List<PayPlanCharge> chargeList = PayPlanCharges.GetForPayPlans(PayPlanList.Select(x => x.PayPlanNum).ToList());
				comboPaymentPlans.Items.Add("None");
				comboPaymentPlans.SelectedIndex = 0;
				for (int i = 0; i < PayPlanList.Count; i++)
				{
					comboPaymentPlans.Items.Add(PayPlans.GetTotalPrinc(PayPlanList[i].PayPlanNum, chargeList).ToString("F")
					+ "  " + Patients.GetPat(PayPlanList[i].PatNum).GetNameFL());
					if (PayPlanList[i].PayPlanNum == CreditCardCur.PayPlanNum)
					{
						comboPaymentPlans.SelectedIndex = i + 1;
					}
				}

				this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height - 144);

				UpdateFrequencyText();
				EnableFrequencyControls();
			}
			else
			{//This will hide the recurring section and change the window size.
				groupRecurringCharges.Visible = false;
				groupChargeFrequency.Visible = false;
				this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height - 486);
			}
			if (_isPaySimpleEnabled && !CreditCardCur.IsNew)
			{
				labelAcctType.Visible = true;
				textAccountType.Visible = true;
			}
			checkChrgWithNoBal.Checked = CreditCardCur.CanChargeWhenNoBal;
			//Only visible if preference is on.
			checkChrgWithNoBal.Visible = Preferences.GetBool(PreferenceName.RecurringChargesAllowedWhenNoPatBal);
			Plugins.HookAddCode(this, "FormCreditCardEdit.Load_end", PatCur);
		}

		private void FillFrequencyCombos() {
			foreach(DayOfWeekFrequency frequency in Enum.GetValues(typeof(DayOfWeekFrequency))) {
				comboFrequency.Items.Add(frequency.GetDescription());
			}
			foreach(DayOfWeek day in Enum.GetValues(typeof(DayOfWeek))) {
				comboDays.Items.Add(day.GetDescription());
			}
			//Set Defaults
			comboFrequency.SelectedIndex=(int)DayOfWeekFrequency.Every;
			comboDays.SelectedIndex=(int)DayOfWeek.Monday;
		}

		private void FillData() {
			if(!CreditCardCur.IsNew) {
				string ccNum=CreditCardCur.CCNumberMasked;
				if(Regex.IsMatch(ccNum,"^\\d{12}(\\d{0,7})")) {	//Credit cards can have a minimum of 12 digits, maximum of 19
					int idxLast4Digits=(ccNum.Length-4);
					ccNum=(new string('X',12))+ccNum.Substring(idxLast4Digits);//replace the first 12 with 12 X's
				}
				textCardNumber.Text=ccNum;
				textAddress.Text=CreditCardCur.Address;
				if(CreditCardCur.CCExpiration.Year>1800) {
					textExpDate.Text=CreditCardCur.CCExpiration.ToString("MMyy");
				}
				textZip.Text=CreditCardCur.Zip;
				if(_isXChargeEnabled || _isPayConnectEnabled || _isPaySimpleEnabled) {//Only fill information if using X-Charge, PayConnect, or PaySimple.
					if(CreditCardCur.ChargeAmt>0) {
						textChargeAmt.Text=CreditCardCur.ChargeAmt.ToString("F");
					}
					if(CreditCardCur.DateStart.Year>1880) {
						textDateStart.Text=CreditCardCur.DateStart.ToShortDateString();
					}
					if(CreditCardCur.DateStop.Year>1880) {
						textDateStop.Text=CreditCardCur.DateStop.ToShortDateString();
					}
					textNote.Text=CreditCardCur.Note;
					if(CreditCardCur.ChargeFrequency!="") {//No charge frequency set
						if(CreditCards.GetFrequencyType(CreditCardCur.ChargeFrequency)==ChargeFrequencyType.FixedDayOfMonth) {
							//No need to change check as it is FixedDayOfMonth default
							textDayOfMonth.Text=CreditCards.GetDaysOfMonthForChargeFrequency(CreditCardCur.ChargeFrequency);
						}
						else {//FixedDayOfWeek
							radioWeekDay.Checked=true;
							comboFrequency.SelectedIndex=(int)CreditCards.GetDayOfWeekFrequency(CreditCardCur.ChargeFrequency);
							comboDays.SelectedIndex=(int)CreditCards.GetDayOfWeek(CreditCardCur.ChargeFrequency);
						}
					}
				}
				if(_isPaySimpleEnabled) {
					textAccountType.Text=(CreditCardCur.CCSource==CreditCardSource.PaySimpleACH ? "ACH" : "Credit Card");
				}
			}
		}

		private void FillProcs() {
			listProcs.Items.Clear();
			if(String.IsNullOrEmpty(CreditCardCur.Procedures)) {
				return;
			}
			string[] arrayProcCodes=CreditCardCur.Procedures.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);
			for(int i=0;i<arrayProcCodes.Length;i++) {
				listProcs.Items.Add(arrayProcCodes[i]+"- "+ProcedureCodes.GetLaymanTerm(ProcedureCodes.GetProcCode(arrayProcCodes[i]).CodeNum));
			}
		}

		private void FillPayTypeCombo() {
			comboPaymentType.Items.AddDefNone("Use Default");
			comboPaymentType.Items.AddDefs(Definitions.GetDefsForCategory(DefinitionCategory.PaymentTypes,true));
			comboPaymentType.SetSelectedDefNum(CreditCardCur.PaymentType);
		}

		private bool VerifyData() {
			if(textCardNumber.Text.Trim().Length<5) {
				MessageBox.Show("Invalid Card Number.");
				return false;
			}
			try {
				if(Regex.IsMatch(textExpDate.Text,@"^\d\d[/\- ]\d\d$")) {//08/07 or 08-07 or 08 07
					CreditCardCur.CCExpiration=new DateTime(Convert.ToInt32("20"+textExpDate.Text.Substring(3,2)),Convert.ToInt32(textExpDate.Text.Substring(0,2)),1);
				}
				else if(Regex.IsMatch(textExpDate.Text,@"^\d{4}$")) {//0807
					CreditCardCur.CCExpiration=new DateTime(Convert.ToInt32("20"+textExpDate.Text.Substring(2,2)),Convert.ToInt32(textExpDate.Text.Substring(0,2)),1);
				}
				else if(CreditCardCur.CCSource!=CreditCardSource.PaySimpleACH) {
					MessageBox.Show("Expiration format invalid.");
					return false;
				}
			}
			catch {
				MessageBox.Show("Expiration format invalid.");
				return false;
			}
			if(textDateStop.Text.Trim()!="" && PIn.Date(textDateStart.Text)>PIn.Date(textDateStop.Text)) {
				if(!MsgBox.Show(MsgBoxButtons.OKCancel,"The recurring charge start date is after the stop date.  Continue?")) {
					return false;
				}
			}
			if(_isXChargeEnabled || _isPayConnectEnabled || _isPaySimpleEnabled) {//Only validate recurring setup if using X-Charge, PayConnect, or PaySimple.
				if(textDateStart.errorProvider1.GetError(textDateStart)!=""
					|| textDateStop.errorProvider1.GetError(textDateStop)!=""
					|| textChargeAmt.errorProvider1.GetError(textChargeAmt)!="")
				{
					MessageBox.Show("Please fix data entry errors first.");
					return false;
				}
				if((textChargeAmt.Text=="" && comboPaymentPlans.SelectedIndex>0)
					|| (textChargeAmt.Text=="" && textDateStart.Text.Trim()!=""))
				{
					MessageBox.Show("You need a charge amount for recurring charges.");
					return false;
				}
				if(textChargeAmt.Text!="" && textDateStart.Text.Trim()=="") {
					MessageBox.Show("You need a start date for recurring charges.");
					return false;
				}
				if(radioDayOfMonth.Checked && textDayOfMonth.Text=="" && textDateStart.Text!="") {
					MessageBox.Show("You must select at least one date for recurring charges when Fixed day(s) of month is checked.");
					return false;
				}
			}
			return true;
		}

		private void textDayOfMonth_TextChanged(object sender,EventArgs e) {
			UpdateFrequencyText();
		}

		private void comboFrequency_SelectionChangeCommitted(object sender,EventArgs e) {
			UpdateFrequencyText();
		}

		private void comboDays_SelectionChangeCommitted(object sender,EventArgs e) {
			UpdateFrequencyText();
		}

		private void radioDayOfMonth_CheckedChanged(object sender,EventArgs e) {
			UpdateFrequencyText();
			EnableFrequencyControls();
		}

		private void EnableFrequencyControls() {
			comboDays.Enabled=radioWeekDay.Checked;
			comboFrequency.Enabled=radioWeekDay.Checked;
			textDayOfMonth.Enabled=radioDayOfMonth.Checked;
			butAddDay.Enabled=radioDayOfMonth.Checked;
			butClearDayOfMonth.Enabled=radioDayOfMonth.Checked;
		}

		private void UpdateFrequencyText() {
			if(radioDayOfMonth.Checked) {
				string daysOfMonth=textDayOfMonth.Text;
				if(daysOfMonth=="") {
					labelFrequencyInWords.Text="";
					return;
				}
				List<string> days=daysOfMonth.Split(',').ToList();
				string daysFormatted="";
				for(int i=0;i<days.Count;i++) {
					if(days.Count > 2 && i > 0) {
						daysFormatted+=",";
					}
					if(days.Count >= 2 && i==days.Count-1) {
						daysFormatted+=" "+"and";
					}
					daysFormatted+=" "+days[i].Trim()+MiscUtils.GetOrdinalIndicator(days[i].Trim());
				}
				labelFrequencyInWords.Text=daysFormatted+" "+"day of the month";
			}
			else {//radioWeekDay
				string frequency=comboFrequency.GetItemText((comboFrequency.Items[comboFrequency.SelectedIndex]));
				string dayOfWeek=comboDays.GetItemText((comboDays.Items[comboDays.SelectedIndex]));
				labelFrequencyInWords.Text=frequency+" "+dayOfWeek+" "+"of the month";
			}
		}

		private void butAddDay_Click(object sender,EventArgs e) {
			List<string> listDaysOfMonth=new List<string>();
			for(int i=1;i<=31;i++) {
				listDaysOfMonth.Add(i.ToString());
			}
			InputBox input=new InputBox(new List<InputBoxParam> { new InputBoxParam {
				ParamType=InputBoxType.ComboSelect,
				LabelText="Day of month",
				ListSelections=listDaysOfMonth,
				ParamSize=new Size(75,21),
				HorizontalAlign=HorizontalAlignment.Center,
			}});
			input.Text="Select Day";
			input.ShowDialog();
			if(input.DialogResult!=DialogResult.OK) {
				return;
			}
			int selectedDay=input.SelectedIndex+1;
			List<int> currentDays=textDayOfMonth.Text.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries)
				.Select(x => PIn.Int(x.Trim())).ToList();
			if(currentDays.Contains(selectedDay)) {
				MessageBox.Show("The selected date has already been added.");
				return;
			}
			currentDays.Add(selectedDay);
			currentDays.Sort();
			textDayOfMonth.Text=string.Join(", ",currentDays);
		}

		private void butClearDayOfMonth_Click(object sender,EventArgs e) {
			textDayOfMonth.Text="";
		}

		private string GetFormattedChargeFrequency() {
			if(textDateStart.Text=="" && CreditCardCur.ChargeFrequency=="") {
				return "";
			}
			if(radioDayOfMonth.Checked) {
				return ((int)ChargeFrequencyType.FixedDayOfMonth).ToString()
					+"|"+textDayOfMonth.Text;
			}
			else {//radioWeekDay
				return ((int)ChargeFrequencyType.FixedWeekDay).ToString()
					+"|"+comboFrequency.SelectedIndex+"|"+comboDays.SelectedIndex;//comboFrequency see enum DayOfWeekFrequency
			}
		}

		private void butClear_Click(object sender,EventArgs e) {
			//Only clear text boxes for recurring charges group.
			textChargeAmt.Text="";
			textDateStart.Text="";
			textDateStop.Text="";
			textNote.Text="";
		}

		private void butToday_Click(object sender,EventArgs e) {
			if(textDayOfMonth.Text=="" && radioDayOfMonth.Checked) {
				textDayOfMonth.Text=DateTime.Today.Day.ToString();
			}
			textDateStart.Text=DateTime.Today.ToShortDateString();
		}

		private void textDateStart_Leave(object sender,EventArgs e) {
			if(radioWeekDay.Checked) {
				return;
			}


				DateTime dateStart=PIn.Date(textDateStart.Text);
				if(dateStart.Year < 1880 || textDayOfMonth.Text!="") {//if invalid date or if they already have something in the day of the month text
					return;
				}
				textDayOfMonth.Text=dateStart.Date.Day.ToString();
			
		}

		private void butAddProc_Click(object sender,EventArgs e) {
			FormProcCodes FormP=new FormProcCodes();
			FormP.IsSelectionMode=true;
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return;
			}
			string procCode=ProcedureCodes.GetStringProcCode(FormP.SelectedCodeNum);
			List<string> procsOnCards=CreditCardCur.Procedures.Split(new string[] { "," },StringSplitOptions.RemoveEmptyEntries).ToList();
			//If the procedure is already attached to this card, return without adding the procedure again
			if(procsOnCards.Exists(x => x==procCode)) {
				return;
			}
			//Warn if attached to a different active card for this patient
			if(CreditCards.ProcLinkedToCard(CreditCardCur.PatNum,procCode,CreditCardCur.CreditCardNum)) {
				if(!MsgBox.Show(MsgBoxButtons.YesNo,"This procedure is already linked with another credit card on this patient's "
					+"account. Adding the procedure to this card will result in the patient being charged twice for this procedure. Add this procedure?")) {
					return;
				}
			}
			if(CreditCardCur.Procedures!="") {
				CreditCardCur.Procedures+=",";
			}
			CreditCardCur.Procedures+=procCode;
			FillProcs();
		}

		private void butRemoveProc_Click(object sender,EventArgs e) {
			if(listProcs.SelectedIndex==-1) {
				MessageBox.Show("Please select a procedure first.");
				return;
			}
			List<string> strList=new List<string>(CreditCardCur.Procedures.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries));
			strList.RemoveAt(listProcs.SelectedIndex);
			CreditCardCur.Procedures=string.Join(",",strList);
			FillProcs();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(CreditCardCur.IsNew) {
				DialogResult=DialogResult.Cancel;
			}
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Are you sure you want to delete this credit card?")) {
				return;
			}
			#region X-Charge
			//Delete the archived X-Charge token 
			if(_isXChargeEnabled && CreditCardCur.XChargeToken!="") { 
				if(CreditCardCur.IsXWeb()) {
					OpenDentBusiness.WebTypes.Shared.XWeb.XWebs.DeleteCreditCard(PatCur.PatNum,CreditCardCur.CreditCardNum);//Also deletes cc from db
				}
				else {
					DeleteXChargeAlias();
				}				
			}
			#endregion
			if(!DeletePaySimpleToken()) {
				return;
			}
			CreditCards.Delete(CreditCardCur.CreditCardNum);
			List<CreditCard> creditCards=CreditCards.Refresh(PatCur.PatNum);
			for(int i=0;i<creditCards.Count;i++) {
				creditCards[i].ItemOrder=creditCards.Count-(i+1);
				CreditCards.Update(creditCards[i]);//Resets ItemOrder.
			}
			DialogResult=DialogResult.OK;
		}
		
		private void DeleteXChargeAlias() {
			Program prog=Programs.GetCur(ProgramName.Xcharge);
			string path=Programs.GetProgramPath(prog);
			if(prog==null) {
				MessageBox.Show("X-Charge entry is missing from the database.");//should never happen
				return;
			}
			if(!prog.Enabled) {
				if(Security.IsAuthorized(Permissions.Setup)) {
					FormXchargeSetup FormX=new FormXchargeSetup();
					FormX.ShowDialog();
				}
				return;
			}
			if(!File.Exists(path)) {
				MessageBox.Show("Path is not valid.");
				if(Security.IsAuthorized(Permissions.Setup)) {
					FormXchargeSetup FormX=new FormXchargeSetup();
					FormX.ShowDialog();
				}
				return;
			}
			ProcessStartInfo info=new ProcessStartInfo(path);
			string resultfile= Storage.GetTempFileName(".txt");
			try {
				File.Delete(resultfile);//delete the old result file.
			}
			catch {
				MessageBox.Show("Could not delete XResult.txt file.  It may be in use by another program, flagged as read-only, or you might not have "
					+"sufficient permissions.");
				return;
			}
			string xUsername=ProgramProperties.GetPropVal(prog.Id,"Username",Clinics.Active.Id);
			string xPassword=CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(prog.Id,"Password",Clinics.Active.Id));
			info.Arguments+="/TRANSACTIONTYPE:ARCHIVEVAULTDELETE ";
			info.Arguments+="/XCACCOUNTID:"+CreditCardCur.XChargeToken+" ";
			info.Arguments+="/RESULTFILE:\""+resultfile+"\" ";
			info.Arguments+="/USERID:"+xUsername+" ";
			info.Arguments+="/PASSWORD:"+xPassword+" ";
			info.Arguments+="/AUTOPROCESS ";
			info.Arguments+="/AUTOCLOSE ";
			Cursor=Cursors.WaitCursor;
			Process process=new Process();
			process.StartInfo=info;
			process.EnableRaisingEvents=true;
			process.Start();
			while(!process.HasExited) {
				Application.DoEvents();
			}
			Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
			Cursor=Cursors.Default;
			string line="";
			try {
				using(TextReader reader = new StreamReader(resultfile)) {
					line=reader.ReadLine();
					while(line!=null) {
						if(line=="RESULT=SUCCESS") {
							break;
						}
						if(line.StartsWith("DESCRIPTION") && !line.Contains("Alias does not exist")) {//If token doesn't exist in X-Charge, still delete from OD
							MessageBox.Show("There was a problem deleting this card within X-Charge.  Please try again.");
							return;//Don't delete the card from OD
						}
						line=reader.ReadLine();
					}
				}
			}
			catch {
				MessageBox.Show("Could not read XResult.txt file.  It may be in use by another program, flagged as read-only, or you might not have "
					+"sufficient permissions.");
				return;
			}
		}

		///<summary>Deletes the PaySimple token if there is one. Returns false if deleting the token failed.</summary>
		private bool DeletePaySimpleToken() {
			if(CreditCardCur.PaySimpleToken!="") {
				Cursor=Cursors.WaitCursor;
				try {
					if(CreditCardCur.CCSource==CreditCardSource.PaySimpleACH) {
						PaySimple.DeleteACHAccount(CreditCardCur);
					}
					else if(CreditCardCur.CCSource==CreditCardSource.PaySimple) {//Credit card
						PaySimple.DeleteCreditCard(CreditCardCur);
					}
				}
				catch(PaySimpleException ex) {
					MessageBox.Show(ex.Message);
					if(ex.ErrorType==PaySimpleError.CustomerDoesNotExist && MsgBox.Show(MsgBoxButtons.OKCancel,
						"Delete the link to the customer id for this patient?")) 
					{
						PatientLinks.DeletePatNumTos(ex.CustomerId,PatientLinkType.PaySimple);
					}
					return false;
				}
				catch(Exception ex) {
					if(MessageBox.Show("Error when deleting from PaySimple:"+"\r\n"+ex.Message+"\r\n\r\n"
						+"Do you still want to delete the card from "+Preferences.GetString(PreferenceName.SoftwareName)+"?",
						"",MessageBoxButtons.YesNo)==DialogResult.No) 
					{
						return false;
					}
				}
				finally {
					Cursor=Cursors.Default;
				}
			}
			return true;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!VerifyData()) {
				return;
			}
			CreditCardCur.ExcludeProcSync=checkExcludeProcSync.Checked;
			CreditCardCur.Address=textAddress.Text;
			CreditCardCur.CCNumberMasked=textCardNumber.Text;
			CreditCardCur.PatNum=PatCur.PatNum;
			CreditCardCur.Zip=textZip.Text;
			CreditCardCur.PaymentType=comboPaymentType.GetSelectedDefNum();
			//Create an Audit Trail whenever CanChargeWhenNoBal changes
			if(checkChrgWithNoBal.Checked!=CreditCardCur.CanChargeWhenNoBal) {
				SecurityLogs.MakeLogEntry(Permissions.AccountModule,PatCur.PatNum,"Credit Card "+CreditCardCur.CCNumberMasked+" set to "
					+(CreditCardCur.CanChargeWhenNoBal?"":"not ")+"run charges even when no patient balance is present.");
			}
			CreditCardCur.CanChargeWhenNoBal=checkChrgWithNoBal.Checked;
			if(_isXChargeEnabled || _isPayConnectEnabled || _isPaySimpleEnabled) {//Only update recurring if using X-Charge, PayConnect,or PaySimple.
				CreditCardCur.ChargeAmt=PIn.Double(textChargeAmt.Text);
				CreditCardCur.DateStart=PIn.Date(textDateStart.Text);
				CreditCardCur.DateStop=PIn.Date(textDateStop.Text);
				CreditCardCur.Note=textNote.Text;
				if(comboPaymentPlans.SelectedIndex>0) {
					CreditCardCur.PayPlanNum=PayPlanList[comboPaymentPlans.SelectedIndex-1].PayPlanNum;
				}
				else {
					CreditCardCur.PayPlanNum=0;//Allows users to change from a recurring payplan charge to a normal one.
				}
				CreditCardCur.ChargeFrequency=GetFormattedChargeFrequency();
			}
			if(CreditCardCur.IsNew) {
				List<CreditCard> itemOrderCount=CreditCards.Refresh(PatCur.PatNum);
				CreditCardCur.ItemOrder=itemOrderCount.Count;
				CreditCardCur.CCSource=CreditCardSource.None;
				CreditCardCur.ClinicNum=Clinics.Active.Id;
				CreditCards.Insert(CreditCardCur);
			}
			else {
				#region X-Charge
				//Special logic for had a token and changed number or expiration date X-Charge
				if(_isXChargeEnabled && CreditCardCur.XChargeToken!="" &&
					(_creditCardOld.CCNumberMasked!=CreditCardCur.CCNumberMasked || _creditCardOld.CCExpiration!=CreditCardCur.CCExpiration)) 
				{ 
					Program prog=Programs.GetCur(ProgramName.Xcharge);
					string path=Programs.GetProgramPath(prog);
					if(prog==null){
						MessageBox.Show("X-Charge entry is missing from the database.");//should never happen
						return;
					}
					if(!prog.Enabled){
						if(Security.IsAuthorized(Permissions.Setup)){
							FormXchargeSetup FormX=new FormXchargeSetup();
							FormX.ShowDialog();
						}
						return;
					}
					if(!File.Exists(path)){
						MessageBox.Show("Path is not valid.");
						if(Security.IsAuthorized(Permissions.Setup)){
							FormXchargeSetup FormX=new FormXchargeSetup();
							FormX.ShowDialog();
						}
						return;
					}
					//Either update the exp date or update credit card number by deleting archive so new token can be created next time it's used.
					ProcessStartInfo info=new ProcessStartInfo(path);
					string resultfile= Storage.GetTempFileName(".txt");
					try {
						File.Delete(resultfile);//delete the old result file.
					}
					catch {
						MessageBox.Show("Could not delete XResult.txt file.  It may be in use by another program, flagged as read-only, or you might not have sufficient permissions.");
						return;
					}
					string xUsername=ProgramProperties.GetPropVal(prog.Id,"Username",Clinics.Active.Id);
					string xPassword=CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(prog.Id,"Password",Clinics.Active.Id));
					//We can only change exp date for X-Charge via ARCHIVEAULTUPDATE.
					info.Arguments+="/TRANSACTIONTYPE:ARCHIVEVAULTUPDATE ";
					info.Arguments+="/XCACCOUNTID:"+CreditCardCur.XChargeToken+" ";
					if(CreditCardCur.CCExpiration!=null && CreditCardCur.CCExpiration.Year>2005) {
						info.Arguments+="/EXP:"+CreditCardCur.CCExpiration.ToString("MMyy")+" ";
					}
					info.Arguments+="/RESULTFILE:\""+resultfile+"\" ";
					info.Arguments+="/USERID:"+xUsername+" ";
					info.Arguments+="/PASSWORD:"+xPassword+" ";
					info.Arguments+="/AUTOPROCESS ";
					info.Arguments+="/AUTOCLOSE ";
					Cursor=Cursors.WaitCursor;
					Process process=new Process();
					process.StartInfo=info;
					process.EnableRaisingEvents=true;
					process.Start();
					while(!process.HasExited) {
						Application.DoEvents();
					}
					Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
					Cursor=Cursors.Default;
					string resulttext="";
					string line="";
					try {
						using(TextReader reader=new StreamReader(resultfile)) {
							line=reader.ReadLine();
							while(line!=null) {
								if(resulttext!="") {
									resulttext+="\r\n";
								}
								resulttext+=line;
								if(line.StartsWith("RESULT=")) {
									if(line!="RESULT=SUCCESS") {
										CreditCardCur=CreditCards.GetOne(CreditCardCur.CreditCardNum);
										FillData();
										return;
									}
								}
								line=reader.ReadLine();
							}
						}
					}
					catch {
						MessageBox.Show("There was a problem creating or editing this card with X-Charge.  Please try again.");
						return;
					}
				}//End of special token logic
				#endregion
				#region PayConnect
				//Special logic for had a token and changed expiration date PayConnect
				//We have to compare the year and month of the expiration instead of just comparing expirations because the X-Charge logic stores the
				//expiration day of the month as the 1st in the db, but it makes more sense to set the expriation day of month to the last day in that month.
				//Since we only want to invalidate the PayConnect token if the expiration month or year is different, we will ignore any difference in day.
				if(_isPayConnectEnabled && CreditCardCur.PayConnectToken!=""
					&& (_creditCardOld.CCExpiration.Year!=CreditCardCur.CCExpiration.Year
						|| _creditCardOld.CCExpiration.Month!=CreditCardCur.CCExpiration.Month))
				{
					//if the expiration is changed, the token is no longer valid, so clear the token and token expiration so a new one can be
					//generated the next time a payment is processed using this card.
					CreditCardCur.PayConnectToken="";
					CreditCardCur.PayConnectTokenExp=DateTime.MinValue;
					//To match PaySimple, this should be enhanced to validate the cc number and get a new token.
				}
				#endregion
				#region PaySimple
				//Special logic for had a token and changed number or expiration date PaySimple
				//We have to compare the year and month of the expiration instead of just comparing expirations because the X-Charge logic stores the
				//expiration day of the month as the 1st in the db, but it makes more sense to set the expriation day of month to the last day in that month.
				//Since we only want to invalidate the PayConnect token if the expiration month or year is different, we will ignore any difference in day.
				if(_isPaySimpleEnabled && CreditCardCur.PaySimpleToken!=""
					&& (_creditCardOld.Zip!=CreditCardCur.Zip
						|| _creditCardOld.CCExpiration.Year!=CreditCardCur.CCExpiration.Year
						|| _creditCardOld.CCExpiration.Month!=CreditCardCur.CCExpiration.Month))
				{
					//TODO: Open form to have user enter the CC number.  Then make API call to update cc instead of wiping out token.
					//If the billing zip or the expiration changes, the token is invalid and they need to get a new one.
					CreditCardCur.PaySimpleToken="";
				}
				#endregion
				CreditCards.Update(CreditCardCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}