/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using CodeBase;
using DentalXChange.Dps.Pos;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.Rendering.Printing;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using PayConnectService = OpenDentBusiness.PayConnectService;
using OpenDentBusiness.WebTypes.Shared.XWeb;
using PdfSharp.Pdf;
using OpenDentBusiness.IO;
using Imedisoft.Forms;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental {
	///<summary></summary>
	public partial class FormPayment:ODForm {

		#region Fields - Public
		///<summary>The amount entered for the current payment.  Amount currently available for paying off charges.
		///If this value is zero, it will be set to the summation of the split amounts when OK is clicked.</summary>
		public decimal AmtTotal;
		///<summary>Set this value to a PaySplitNum if you want one of the splits highlighted when opening this form.</summary>
		public long InitialPaySplitNum;
		///<summary>Set to true if this payment is supposed to be an income transfer.</summary>
		public bool IsIncomeTransfer;
		///<summary></summary>
		public bool IsNew=false;
		///<summary>Procedures and payplan charges from account module we want to make splits for on this payment.</summary>
		public List<AccountEntry> ListEntriesPayFirst;
		///<summary>Set to a positive amount if there is an unearned amount for the patient and they want to use it.</summary>
		public double UnearnedAmt;
		#endregion

		#region Fields - Private
		private long[] _arrayDepositAcctNums;
		///<summary>A dictionary that stores the selected indices for the Outstanding Charges grid that are associated to a single PaySplit.</summary>
		private Dictionary<PaySplitHelper,List<int>> _dictGridChargesPaySplitIndices=new Dictionary<PaySplitHelper,List<int>>();
		///<summary>A dictionary that stores the selected indices for the Current Payment Splits grid that are associated to a single PaySplit.</summary>
		private Dictionary<PaySplitHelper,List<int>> _dictGridSplitsPaySplitIndices=new Dictionary<PaySplitHelper,List<int>>();
		///<summary>A dictionary that stores the selected indices for the Treat Plan grid that are associated to a single PaySplit.</summary>
		private Dictionary<PaySplitHelper,List<int>> _dictGridTreatPlanPaySplitIndices=new Dictionary<PaySplitHelper,List<int>>();
		///<summary>A dictionary or patients that we may need to reference to fill the grids to eliminate unnecessary calls to the DB.
		///Should contain all patients in the current family along with any patients of payment plans of which a member of this family is the guarantor.</summary>
		private Dictionary<long,Patient> _dictPatients;
		private bool _isCCDeclined;
		private bool _isInit;
		private bool _isPayConnectPortal;
		///<summary>Direct family members of the current patient.</summary>
		private readonly Family _famCur;
		///<summary></summary>
		private List<AccountEntry> _listAccountCharges;
		///<summary>Stored CreditCards for _patCur.</summary>
		private List<CreditCard> _listCreditCards;
		///<summary>When making income transfer splits, PaySplitAssociated objects are used.  It keeps track of split associations until we insert into
		///the database.  PaySplitOrig is the PaySplit the PaySplitLinked will "link to" with its FSplitNum.</summary>
		private List<PaySplits.PaySplitAssociated> _listPaySplitsAssociated;
		///<summary>List of paysplit that were deleted and need a securitylog entry.</summary>
		private List<PaySplit> _listPaySplitsForSecLog=new List<PaySplit>();
		///<summary>The original splits that existed when this window was opened.  Empty for new payments.</summary>
		private List<PaySplit> _listPaySplitsOld;
		private List<Definition> _listPaymentTypeDefs;
		///<summary>A current list of splits showing on the left grid.</summary>
		private List<PaySplit> _listSplitsCur=new List<PaySplit>();
		///<summary>Holds most all the data needed to load the form.</summary>
		private PaymentEdit.LoadData _loadData;
		private int _originalHeight;
		private Patient _patCur;
		private PayConnectService.creditCardRequest _payConnectRequest;
		private PayConnectResponseWeb _payConnectResponseWeb;
		private PaySimple.ApiResponse _paySimpleResponse;
		private Payment _paymentCur;
		private Payment _paymentOld;
		private System.Drawing.Printing.PrintDocument _pd2;
		private bool _preferCurrentPat;
		private bool _printReceipt;
		private bool _promptSignature;
		///<summary>This table gets created and filled once at the beginning.  After that, only the last column gets carefully updated.</summary>
		private DataTable _tableBalances;
		///<summary>Set to true when X-Charge or PayConnect makes a successful transaction, except for voids.</summary>
		private bool _wasCreditCardSuccessful;
		///<summary>Used to track position inside the MakeXChargeTransaction(), for troubleshooting purposes.</summary>
		private string _xChargeMilestone;
		///<summary>The local override path or normal path for X-Charge.</summary>
		private string _xPath;
		///<summary>Program X-Charge.</summary>
		private Program _xProg;
		///<summary>The XWebResponse that created this payment. Will only be set if the payment originated from XWeb..</summary>
		private XWebResponse _xWebResponse;
		#endregion

		///<summary>PatCur and FamCur are not for the PatCur of the payment.  They are for the patient and family from which this window was accessed.
		///Use listSelectedProcs to automatically attach payment to specific procedures.</summary>
		public FormPayment(Patient patCur,Family famCur,Payment paymentCur,bool preferCurrentPat) {
			InitializeComponent();// Required for Windows Form Designer support
			_patCur=patCur;
			_famCur=famCur;
			_paymentCur=paymentCur;
			_preferCurrentPat=preferCurrentPat;
			
			panelXcharge.ContextMenu=contextMenuXcharge;
			butPayConnect.ContextMenu=contextMenuPayConnect;
			butPaySimple.ContextMenu=contextMenuPaySimple;
			_paymentOld=paymentCur.Clone();
		}

		#region Properties - Public
		public string XchargeMilestone {
			get {
				return _xChargeMilestone;
			}
		}
		#endregion

		#region Properties - Private
		///<summary>Returns either the family or super family of the current patients 
		///depending on whether or not the "Show Charges for Superfamily" checkbox is checked.</summary>
		private Family _curFamOrSuperFam {
			get {
				if(checkShowSuperfamily.Checked) {
					return _loadData.SuperFam;
				}
				else {
					return _famCur;
				}
			}
		}

		///<summary>List of selected clinic nums to filter outstanding charges grid on.</summary>
		private List<long> _listFilteredClinics {
			get {
				//Get filtered clinics and if all is selected, this list will also contain all
				return comboClinicFilter.GetListSelected<Clinic>().Select(x => x.Id).ToList();

			}
		}

		///<summary>List of selected patnums to filter outstanding charges grid on.</summary>
		private List<long> _listFilteredPatNums {
			get {
				//Get filtered patients
				if(comboPatientFilter.ListSelectedIndices.Contains(0)) {//Contains "All"
					return comboPatientFilter.Items.Cast<ODBoxItem<Patient>>().Where(x => x.Tag!=null).Select(x => x.Tag.PatNum).ToList();
				}
				else {
					return comboPatientFilter.ListSelectedItems.Cast<ODBoxItem<Patient>>().Where(x => x.Tag!=null).Select(x => x.Tag.PatNum).ToList();
				}
			}
		}

		///<summary>List of user-inputed proc codes to filter outstanding charges grid on.</summary>
		private List<long> _listFilteredProcCodes {
			get {
				List<long> listFilteredProcCodes=new List<long>();
				//Proc codes
				List<string> listCodes=textFilterProcCodes.Text.Split(new char[] { ',' }).ToList();
				foreach(string code in listCodes) {
					long retrievedCode=ProcedureCodes.GetCodeNum(code.Trim());  //returns 0 if code not found
					if(retrievedCode!=0) {
						listFilteredProcCodes.Add(retrievedCode);
					}
				}
				return listFilteredProcCodes;
			}
		}

		///<summary>List of selected charge types to filter outstanding charges grid on: PaySplit, PayPlan Charge, Adjustment, Procedure.</summary>
		private List<string> _listFilteredType {
			get {
				List<string> listTypes=new List<string>();
				//Get filtered types
				if(comboTypeFilter.ListSelectedIndices.Contains(0)) {//'All' is selected
					for(int i=1;i<comboTypeFilter.Items.Count;i++) {//Starts at 1 to not include 'All'
						listTypes.Add(comboTypeFilter.Items[i].ToString());
					}
				}
				else {
					//Find all selected types
					for(int i=0;i<comboTypeFilter.ListSelectedItems.Count;i++) {
						listTypes.Add(comboTypeFilter.ListSelectedItems[i].ToString());
					}
				}
				return listTypes;
			}
		}

		///<summary>The list of patnums in _currentFamily, either the superfamily or the regular family depending on the superfam checkbox state.</summary>
		private List<long> _listPatNums {
			get {
				return _curFamOrSuperFam.ListPats.Select(x => x.PatNum).ToList();
			}
		}
		#endregion

		private void FormPayment_Load(object sender,System.EventArgs e) {
			_loadData=PaymentEdit.GetLoadData(_patCur,_paymentCur,IsNew,(IsIncomeTransfer || _paymentCur.PayType==0));
			_isPayConnectPortal=_paymentCur.PaymentSource.In(CreditCardSource.PayConnectPortal,CreditCardSource.PayConnectPortalLogin);
			if(_isPayConnectPortal) {
				groupXWeb.Text="PayConnect Portal";
			}
			if(PrefC.GetEnum<YN>(PreferenceName.PrePayAllowedForTpProcs)==YN.No) {
				tabProcCharges.TabPages.Remove(tabPageTreatPlan);
			}
			else {
				if(tabProcCharges.TabPages.Contains(tabPageTreatPlan)){
					tabProcCharges.TabPages.Remove(tabPageTreatPlan);
				}
				tabProcCharges.TabPages.Add(tabPageTreatPlan);
			}
			if(IsNew) {
				checkPayTypeNone.Enabled=true;
				if(!Security.IsAuthorized(Permissions.PaymentCreate,_paymentCur.PayDate)) {//to prevent backdating of payments, check for date when this form is loaded
					DialogResult=DialogResult.Cancel;
					return;
				}
				butDeletePayment.Enabled=false;
			}
			else {
				checkPayTypeNone.Enabled=false;
				checkRecurring.Checked=_paymentCur.IsRecurringCC;
				if(!Security.IsAuthorized(Permissions.PaymentEdit,_paymentCur.PayDate)) {
					butOK.Enabled=false;
					butDeletePayment.Enabled=false;
					butAddManual.Enabled=false;
					gridSplits.Enabled=false;
					butPay.Enabled=false;
					butCreatePartial.Enabled=false;
					butClear.Enabled=false;
					butDelete.Enabled=false;
					checkRecurring.Enabled=false;
					panelXcharge.Enabled=false;
					butPayConnect.Enabled=false;
					butPaySimple.Enabled=false;
					if(Security.IsAuthorized(Permissions.SplitCreatePastLockDate,true)) {
						//Since we are enabling the OK button, we need to make sure everything else is disabled (except for Add).
						butOK.Enabled=true;
						butAddManual.Enabled=true;
						comboClinic.Enabled=false;
						textDate.ReadOnly=true;
						textAmount.ReadOnly=true;
						butPrePay.Enabled=false;
						textCheckNum.ReadOnly=true;
						textBankBranch.ReadOnly=true;
						textNote.ReadOnly=true;
						checkPayTypeNone.Enabled=false;
						listPayType.Enabled=false;
						comboDepositAccount.Enabled=false;
						comboCreditCards.Enabled=false;
						checkProcessed.Enabled=false;
						gridSplits.Enabled=true;
					}
				}
			}
			if(PrefC.HasClinicsEnabled) {
				comboClinic.SelectedClinicNum=_paymentCur.ClinicNum;
			}
			else {//clinics not enabled
				comboClinicFilter.Visible=false;
				labelClinicFilter.Visible=false;
			}
			if(_paymentCur.ProcessStatus==ProcessStat.OfficeProcessed) {
				checkProcessed.Visible=false;//This checkbox will only show if the payment originated online.
			}
			else if(_paymentCur.ProcessStatus==ProcessStat.OnlineProcessed) {
				checkProcessed.Checked=true;
			}
			_listCreditCards=_loadData.ListCreditCards;
			FillCreditCards();
			_tableBalances=_loadData.TableBalances;
			//this works even if patient not in family
			textPaidBy.Text=_curFamOrSuperFam.GetNameInFamFL(_paymentCur.PatNum);
			textDateEntry.Text=_paymentCur.DateEntry.ToShortDateString();
			textDate.Text=_paymentCur.PayDate.ToShortDateString();
			textAmount.Text=_paymentCur.PayAmt.ToString("F");
			textCheckNum.Text=_paymentCur.CheckNum;
			textBankBranch.Text=_paymentCur.BankBranch;
			_listPaymentTypeDefs=Definitions.GetDefsForCategory(DefinitionCategory.PaymentTypes,true);
			for(int i=0;i<_listPaymentTypeDefs.Count;i++) {
				listPayType.Items.Add(_listPaymentTypeDefs[i].Name);
				if(IsNew && Preferences.GetBool(PreferenceName.PaymentsPromptForPayType)) {//skip auto selecting payment type if preference is enabled and payment is new
					continue;//user will be forced to selectan indexbefore closing or clicking ok
				}
				if(_listPaymentTypeDefs[i].Id==_paymentCur.PayType) {
					listPayType.SelectedIndex=i;
				}
			}
			textNote.Text=_paymentCur.PayNote;
			Deposit deposit=null;
			if(_paymentCur.DepositNum!=0) {
				deposit=Deposits.GetOne(_paymentCur.DepositNum);
			}
			if(deposit==null) {//If there was none or it got deleted, disable controls.
				labelDeposit.Visible=false;
				textDeposit.Visible=false;
			}
			else {
				textDeposit.Text=deposit.DateDeposit.ToShortDateString();
				textAmount.ReadOnly=true;
				textAmount.BackColor=SystemColors.Control;
				butPay.Enabled=false;
			}
			_listSplitsCur=_loadData.ListSplits;//Count might be 0
			_listPaySplitsOld=new List<PaySplit>();
			_listPaySplitsAssociated=new List<PaySplits.PaySplitAssociated>();
			foreach(PaySplit paySplit in _listSplitsCur) {
				_listPaySplitsOld.Add(paySplit.Copy());
			}
			if(_loadData.ListPaySplitAllocations.Count==0) {
				tabControlSplits.TabPages.Remove(tabPageAllocated);
			}
			FillListPaySplitAssociated();
			if(IsNew && UnearnedAmt.IsGreaterThanZero()) {
				_loadData.ListSplits=PaymentEdit.AllocateUnearned(_paymentCur.PayNum,UnearnedAmt,ListEntriesPayFirst,_famCur);
				_listSplitsCur=_loadData.ListSplits;
			}
			if(IsNew) {
				//Fill comboDepositAccount based on autopay for listPayType.SelectedIndex
				SetComboDepositAccounts();
				textDepositAccount.Visible=false;
			}
			else {
				//put a description in the textbox.  If the user clicks on the same or another item in listPayType,
				//then the textbox will go away, and be replaced by comboDepositAccount.
				labelDepositAccount.Visible=false;
				comboDepositAccount.Visible=false;
				Transaction trans=_loadData.Transaction;
				if(trans==null) {
					textDepositAccount.Visible=false;
				}
				else {
					//add only the description based on PaymentCur attached to transaction
					foreach(JournalEntry journalEntry in JournalEntries.GetForTrans(trans.Id)) {
						Account account=Accounts.GetAccount(journalEntry.AccountNum);
						//The account could be null if the AccountNum was never set correctly due to the automatic payment entry setup missing an income account from older versions.
						if(account!=null && account.Type==AccountType.Asset) {
							textDepositAccount.Text=journalEntry.DateDisplayed.ToShortDateString();
							if(journalEntry.DebitAmt>0) {
								textDepositAccount.Text+=" "+journalEntry.DebitAmt.ToString("c");
							}
							else {//negative
								textDepositAccount.Text+=" "+(-journalEntry.CreditAmt).ToString("c");
							}
							break;
						}
					}
				}
			}
			if(!string.IsNullOrEmpty(_paymentCur.Receipt)) {
				if(Preferences.GetBool(PreferenceName.AllowEmailCCReceipt)) {
					butEmailReceipt.Visible=true;
				}
				butPrintReceipt.Visible=true;
			}
			comboGroupBy.Items.Add("None");
			comboGroupBy.Items.Add("Provider");
			if(PrefC.HasClinicsEnabled) {
				comboGroupBy.Items.Add("Clinic and Provider");
			}
			comboGroupBy.SelectedIndex=0;
			if(IsIncomeTransfer || _paymentCur.PayType==0) {
				checkPayTypeNone.Checked=true;
			}
			if(_patCur.SuperFamily<=0) {
				checkShowSuperfamily.Visible=false;
			}
			else { 
				//Check the Super Family box if there are any splits from a member in the super family who is not in the immediate family.
				List<Patient> listSuperFamExclusive=_loadData.SuperFam.ListPats.Where(x => !_famCur.IsInFamily(x.PatNum)).ToList();
				if(!IsNew && (_listSplitsCur.Any(x => x.PatNum.In(listSuperFamExclusive.Select(y => y.PatNum))))) {
					checkShowSuperfamily.Checked=true;
				}
			}
			comboClinicFilter.IncludeAll=true;
			Init(doAutoSplit:true,doPayFirstAcctEntries:true);
			if(InitialPaySplitNum!=0) {
				gridSplits.SetSelected(false);
				PaySplit paySplitInit=_listSplitsCur.FirstOrDefault(x => x.SplitNum==InitialPaySplitNum);
				if(paySplitInit!=null) {
					SelectPaySplit(paySplitInit);
				}
				HighlightChargesForSplits();
			}
			CheckUIState();
			_originalHeight=Height;
			if(Preferences.GetBool(PreferenceName.PaymentWindowDefaultHideSplits)) {
				ToggleShowHideSplits();//set hidden
			}
			textCheckNum.Select();
			Plugins.HookAddCode(this,"FormPayment.Load_end",_paymentCur,IsNew);
		}

		///<summary>The shown event is for code that requires the Payment window to be visible already. E.g. showing MsgBoxCopyPaste.</summary>
		private void FormPayment_Shown(object sender,EventArgs e) {
			//jsalmon - The negative unallocated or unearned warning can come back once there is a preference for the office to opt out of receiving it.
			//if(IsNew && PaymentEdit.IsUnallocatedOrUnearnedNegative(_patCur.PatNum,_famCur,out string warningMessage)) {
			//	string displayText=Lans.g("PaymentEdit","The following pre-existing payment splits may be causing the Total of Outstanding Charges to"
			//		+" differ from the family balance:")+"\r\n"+warningMessage;
			//	MsgBoxCopyPaste msgBoxCopyPaste=new MsgBoxCopyPaste(displayText);
			//	msgBoxCopyPaste.Show();
			//}
		}

		#region Events - Click
		///<summary>Creates a paysplit for the user to edit manually.</summary>
		private void butAddManualSplit_Click(object sender,EventArgs e) {
			PaySplit paySplit=new PaySplit();
			paySplit.SplitAmt=0;
			paySplit.DatePay=_paymentCur.PayDate;
			paySplit.DateEntry=MiscData.GetNowDateTime();//just a nicety for the user.  Insert uses server time.
			paySplit.PayNum=_paymentCur.PayNum;
			paySplit.ProvNum=Patients.GetProvNum(_patCur);
			paySplit.ClinicNum=_paymentCur.ClinicNum;
			paySplit.IsNew=true;
			FormPaySplitEdit FormPSE=new FormPaySplitEdit(_curFamOrSuperFam);
      FormPSE.ListSplitsCur=_listSplitsCur;
			FormPSE.PaySplitCur=paySplit;
			FormPSE.IsNew=true;
			FormPSE.ListPaySplitAssociated=_listPaySplitsAssociated;
			if(FormPSE.ShowDialog()==DialogResult.OK) {
				if(!_dictPatients.ContainsKey(paySplit.PatNum)) {
					//add new patnum to _dictPatients
					Patient pat=Patients.GetLim(paySplit.PatNum);
					if(pat!=null) {
						_dictPatients[paySplit.PatNum]=pat;
					}
				}
				_listSplitsCur.Add(paySplit);
				Reinitialize();
			}
		}

		private void butCreatePartialSplit_Click(object sender,EventArgs e) {
			if(checkPayTypeNone.Checked) {//Button only visible if EnforceFully is enabled
				FormPaySplitManage FormPSM=new FormPaySplitManage(checkPayTypeNone.Checked);
				FormPSM.ListSplitsCur=_listSplitsCur;
				FormPSM.ListSplitsAssociated=_listPaySplitsAssociated;
				FormPSM.PayDate=PIn.Date(textDate.Text);
				FormPSM.PatCur=_patCur;
				FormPSM.PaymentAmt=0;
				FormPSM.PaymentCur=_paymentCur;
				FormPSM.FamCur=_famCur;
				if(FormPSM.ShowDialog()==DialogResult.OK) {
					Reinitialize();
				}
				return;
			}
			if(tabProcCharges.SelectedTab==tabPageCharges){
				CreatePartialSplitClickHelper(gridCharges);
			}
			else if(tabProcCharges.SelectedTab==tabPageTreatPlan){
				CreatePartialSplitClickHelper(gridTreatPlan);
			}
			Reinitialize();
		}

		private void butEmailReceipt_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.EmailSend)){
				return;
			}
			if(string.IsNullOrWhiteSpace(_paymentCur.Receipt)) {
				MessageBox.Show("There is no receipt to send for this payment.");
				return;
			}
			List<string> errors=new List<string>();
			if(!EmailAddresses.ExistsValidEmail()) {
				errors.Add("SMTP server name missing in e-mail setup.");
			}
			if(errors.Count>0) {
				MessageBox.Show(this,"The following errors need to be resolved before creating an email"+":\r\n"+string.Join("\r\n",errors));
				return;
			}
			string attachPath=EmailAttaches.GetAttachPath();
			Random rnd=new Random();

			string tempFile = Path.Combine(Path.GetTempPath(),
				DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.TimeOfDay.Ticks.ToString() + rnd.Next(1000).ToString() + ".pdf");

			PdfDocumentRenderer pdfRenderer=new PdfDocumentRenderer(true,PdfFontEmbedding.Always);
			pdfRenderer.Document=CreatePDFDoc(_paymentCur.Receipt);
			pdfRenderer.RenderDocument();
			pdfRenderer.PdfDocument.Save(tempFile);
			Storage.Copy(tempFile, Storage.CombinePaths(attachPath, Path.GetFileName(tempFile)));
			EmailMessage message=new EmailMessage();
			message.PatNum=_paymentCur.PatNum;
			message.ToAddress=_patCur.Email;
			EmailAddress address=EmailAddresses.GetByClinic(_patCur.ClinicNum);
			message.FromAddress=address.GetFrom();
			message.Subject="Receipt for payment received "+_paymentCur.PayDate.ToShortDateString();
			EmailAttach attachRcpt=new EmailAttach() {
				DisplayedFileName="Receipt.pdf",
				ActualFileName=Path.GetFileName(tempFile)
			};
			message.Attachments=new List<EmailAttach>() { attachRcpt };
			FormEmailMessageEdit FormE=new FormEmailMessageEdit(message,address);
			FormE.IsNew=true;
			FormE.ShowDialog();
		}

		private void butPay_Click(object sender,EventArgs e) {
			if(checkPayTypeNone.Checked) {//Income transfer mode.
				if(!gridSplits.Rows.IsNullOrEmpty()) {
					if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Performing a transfer will overwrite all Current Payment Splits.  Continue?")) {
						return;
					}
				}
				_listSplitsCur.Clear();//Ignore any splits the user has made / manipulated.
				PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(_famCur.GetPatNums(),_patCur.PatNum,_listSplitsCur,
						_paymentCur,ListEntriesPayFirst,isIncomeTxfr:true);
				_listAccountCharges=constructResults.ListAccountCharges;
				if(PaymentEdit.TryCreateIncomeTransfer(_listAccountCharges,DateTime.Now,out PaymentEdit.IncomeTransferData incomeTransferData)) {
					incomeTransferData.ListSplitsCur.ForEach(x => x.PayNum=_paymentCur.PayNum);
					_listSplitsCur=incomeTransferData.ListSplitsCur;
					_listPaySplitsAssociated=incomeTransferData.ListSplitsAssociated;
					Reinitialize(true);
				}
				else {
					MsgBoxCopyPaste msgBoxCopyPaste=new MsgBoxCopyPaste(incomeTransferData.SummaryText);
					msgBoxCopyPaste.Show();
				}
				return;
			}
			double payAmtOrig=PIn.Double(textAmount.Text);
			_paymentCur.PayAmt=payAmtOrig;//Just in case some other entity set the PayAmt field to a different value or the user manually changed it.
			if(!payAmtOrig.IsZero()) {
				double payAmtRemaining=(payAmtOrig-_listSplitsCur.Sum(x => x.SplitAmt));
				//When the remaining amount is in the negative let the user do whatever they want by treating the PayAmt as 0.
				//Otherwise, this will make it so that the MakePayment method will only suggest PaySplits up to the amount remaining on the payment.
				_paymentCur.PayAmt=Math.Max(0,payAmtRemaining);
			}
			tabControlSplits.SelectedIndex=0;
			List<List<AccountEntry>> listGridEntries=GetAccountEntriesFromGrid((tabProcCharges.SelectedTab==tabPageCharges) ? gridCharges: gridTreatPlan);
			//Remove groups of account entries that sum up to an AmountEnd of zero.  There is no reason to be making zero dollar PaySplits.
			//If the user is trying to make a transfer then there needs to be offsetting negative and positive splits.
			listGridEntries.RemoveAll(x => x.Sum(y => y.AmountEnd).IsZero());
			if(listGridEntries.Count==0) {
				return;//No need to display a message, no PaySplits showing up in the grid is enough for the user to know that nothing happened.
			}
			PaymentEdit.PayResults createdSplits=PaymentEdit.MakePayment(listGridEntries,_paymentCur,PIn.Decimal(textAmount.Text),
				_listAccountCharges);
			_listSplitsCur.AddRange(createdSplits.ListSplitsCur);
			_listAccountCharges=createdSplits.ListAccountCharges;
			_paymentCur=createdSplits.Payment;
			_paymentCur.PayAmt=payAmtOrig;//Reset it
			Reinitialize();
		}

		private void butPayConnect_Click(object sender,EventArgs e) {
			if(!CanAddNewCreditCard(Programs.GetCur(ProgramName.PayConnect),PayConnect.ProgramProperties.PayConnectPreventSavingNewCC)) {
				return;
			}
			if(comboCreditCards.SelectedIndex < _listCreditCards.Count) {
				CreditCard cc=_listCreditCards[comboCreditCards.SelectedIndex];
				if(cc!=null && cc.CCSource==CreditCardSource.PayConnectPortal) {
					MessageBox.Show("The selected credit card can only be used to void and return payments made with this card.  Use the Void and Return buttons in this window instead.");
					return;
				}
			}
			MakePayConnectTransaction();
		}

		private void butPaySimple_Click(object sender,MouseEventArgs e) {
			if(e.Button!=MouseButtons.Left) {
				return;
			}
			if(!CanAddNewCreditCard(Programs.GetCur(ProgramName.PaySimple),PaySimple.PropertyDescs.PaySimplePreventSavingNewCC)) {
				return;
			}
			MakePaySimpleTransaction();
		}

		private void butPrePay_Click(object sender,EventArgs e) {
			if(PIn.Double(textAmount.Text)==0) {
				MessageBox.Show("Amount cannot be zero.");
				return;
			}
			if(_listSplitsCur.Count>0) {
				if(!MsgBox.Show(MsgBoxButtons.YesNo,"This will replace all Payment Splits with one split for the total amount.  Continue?")) {
					return;
				}
			}
			_listSplitsCur.Clear();
			PaySplit split=new PaySplit();
			split.PatNum=_patCur.PatNum;
			split.PayNum=_paymentCur.PayNum;
			split.FSplitNum=0;
			split.SplitAmt=PIn.Double(textAmount.Text);
			split.DatePay=DateTime.Now;
			split.ClinicNum=_paymentCur.ClinicNum;
			split.UnearnedType=Preferences.GetLong(PreferenceName.PrepaymentUnearnedType);
			_listSplitsCur.Add(split);
			Reinitialize();
			Application.DoEvents();
			if(!SavePaymentToDb()) {
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butPrintReceipt_Click(object sender,EventArgs e) {
			PrintReceipt(_paymentCur.Receipt,"Receipt printed");
		}

		private void butReturn_Click(object sender,EventArgs e) {
			if(MsgBox.Show(MsgBoxButtons.YesNo,"Are you sure you want to return this transaction?")) {
				if(_xWebResponse!=null) {
					XWebReturn();
				}
				else if(_payConnectResponseWeb!=null) {
					PayConnectReturn();
				}
			}
		}

		private void butShowHide_Click(object sender,EventArgs e) {
			ToggleShowHideSplits();
		}

		private void butVoid_Click(object sender,EventArgs e) {
			if(MsgBox.Show(MsgBoxButtons.YesNo,"Are you sure you want to void this transaction?")) {
				if(_xWebResponse!=null) {
					XWebVoid();
				}
				else if(_payConnectResponseWeb!=null) {
					PayConnectVoid();
				}
			}
		}

		private void CheckIncludeExplicitCreditsOnly_Click(object sender,EventArgs e) {
			Reinitialize(doRefreshConstructData:true);
		}

		private void checkPayTypeNone_Click(object sender,EventArgs e) {
			Reinitialize(doRefreshConstructData:true);
		}

		private void checkShowAll_Clicked(object sender,EventArgs e) {
			Reinitialize(doSelectAllSplits:true);
		}

		///<summary>Constructs a list of AccountCharges and goes through and links those charges to credits.</summary>
		private void checkShowSuperfamily_Click(object sender,EventArgs e) {
			if(_patCur.SuperFamily==0) { //if no super family, just return.
				return;
			}
			Reinitialize(doRefreshConstructData:true);
		}

		///<summary>When a charge is selected this method highlights all paysplits associated with it.</summary>
		private void gridCharges_CellClick(object sender,ODGridClickEventArgs e) {
			CellClickHelper(gridCharges);
		}
		
		///<summary>When a paysplit is selected this method highlights all charges associated with it.</summary>
		private void gridSplits_CellClick(object sender,ODGridClickEventArgs e) {
			HighlightChargesForSplits();
		}

		///<summary>Allows editing of an individual double clicked paysplit entry.</summary>
		private void gridSplits_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PaySplit paySplitOld=(PaySplit)gridSplits.Rows[e.Row].Tag;
			PaySplit paySplit=paySplitOld.Copy();
			if(paySplit.DateEntry!=DateTime.MinValue && !Security.IsAuthorized(Permissions.PaymentEdit,paySplit.DatePay,false)) {
				return;
			}
			PaySplits.PaySplitAssociated splitAssociatedOld=_listPaySplitsAssociated.Find(x => x.PaySplitLinked==paySplitOld);
			FormPaySplitEdit FormPSE=new FormPaySplitEdit(_curFamOrSuperFam);
			FormPSE.ListSplitsCur=_listSplitsCur;
			FormPSE.PaySplitCur=paySplit;
			FormPSE.ListPaySplitAssociated=_listPaySplitsAssociated;
			if(FormPSE.ShowDialog()==DialogResult.OK) {//paySplit contains all the info we want.  
				//Delete paysplit from paysplit grid, credit the charge it's associated to.  
				//Paysplit may be re-associated with a different charge and we wouldn't know, so we need to do this.
				//Paysplits associated to payplancharge cannot be associated to a different payplancharge from this window.
				if(FormPSE.PaySplitCur==null){//remove old association from list.
					_listPaySplitsAssociated.Remove(splitAssociatedOld);
				}
				DeleteSelected(paySplit,doCreateSecLog:false);
				if(paySplit!=null && !_dictPatients.ContainsKey(paySplit.PatNum)) {
					//add new patnum to _dictPatients
					Patient pat=Patients.GetLim(paySplit.PatNum);
					if(pat!=null) {
						_dictPatients[paySplit.PatNum]=pat;
					}
				}
				if(FormPSE.PaySplitCur==null) {//Deleted the paysplit, just return here.
					return;
				}
				//A shallow copy of the list of splits was passed into the PaySplit edit window so it could have been manipulated within.
				//The user most likely changed something about the split which would cause paySplitOld to no longer be in the list.
				if(!_listSplitsCur.Contains(paySplitOld)) {
					//At this point we know that paySplit is not null (it is a shallow copy of FormPSE.PaySplitCur) and it was being manipulated by the user.
					//Add it to the list of splits and try to associate the 'new-ish' split to an AccountEntry if possible.
					//E.g. if the user attached the split to a procedure, adjustment, etc. it should then be associated to the corresponding account entry.
					_listSplitsCur.Add(paySplit);
					Reinitialize();
					//Try and select the PaySplit that was double clicked if it is still around.
					SelectPaySplit(paySplitOld);
					_paymentCur.PayAmt-=paySplit.SplitAmt;
				}
			}
			HighlightChargesForSplits();
		}

		///<summary>When a Treatment Plan Procedure is selected this method highlights all paysplits associated with it.</summary>
		private void gridTreatPlan_CellClick(object sender,ODGridClickEventArgs e) {
			CellClickHelper(gridTreatPlan);
		}

		private void listPayType_Click(object sender,EventArgs e) {
			if(Plugins.HookMethod(this,"FormPayment.listPayType_Click",listPayType.SelectedItem)) {
				return;
			}
			textDepositAccount.Visible=false;
			SetComboDepositAccounts();
		}

		private void menuPayConnect_Click(object sender,EventArgs e) {
			if(Security.IsAuthorized(Permissions.Setup)) {
				FormPayConnectSetup fpcs=new FormPayConnectSetup();
				fpcs.ShowDialog();
				CheckUIState();
			}
		}

		private void menuPaySimple_Click(object sender,EventArgs e) {
			if(Security.IsAuthorized(Permissions.Setup)) {
				FormPaySimpleSetup form=new FormPaySimpleSetup();
				form.ShowDialog();
				CheckUIState();
			}
		}

		private void menuXcharge_Click(object sender,EventArgs e) {
			if(Security.IsAuthorized(Permissions.Setup)) {
				FormXchargeSetup FormX=new FormXchargeSetup();
				FormX.ShowDialog();
				CheckUIState();
			}
		}

		private void panelXcharge_MouseClick(object sender,MouseEventArgs e) {
			if(e.Button != MouseButtons.Left) {
				return;
			}
			if(!CanAddNewCreditCard(Programs.GetCur(ProgramName.Xcharge),ProgramProperties.PropertyDescs.XCharge.XChargePreventSavingNewCC)) {
				return;
			}
			_xChargeMilestone="";
			try {
				MakeXChargeTransaction();
			}
			catch(Exception ex) {
				FriendlyException.Show("Error processing transaction.\r\n\r\nPlease contact support with the details of this error:"
					//The rest of the message is not translated on purpose because we here at HQ need to always be able to quickly read this part.
					+"\r\nLast valid milestone reached: "+_xChargeMilestone,ex);
			}
		}
		#endregion

		#region Events - Change
		private void checkPayTypeNone_CheckedChanged(object sender,EventArgs e) {
			//this fires before the click event.  The Checked property also reflects the new value.
			if(checkPayTypeNone.Checked) {
				checkShowAll.Checked=false;
				checkShowAll.Enabled=false;
				listPayType.Visible=false;
				butPay.Text="Transfer";
				if(PrefC.HasClinicsEnabled) {
					comboGroupBy.SelectedIndex=2;
				}
				else { 
					comboGroupBy.SelectedIndex=1;
				}
				comboGroupBy.Enabled=false;
				if(PrefC.GetInt(PreferenceName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
					butCreatePartial.Text="Proc Breakdown";
					butPay.Visible=false;
				}
				else {
					butCreatePartial.Visible=false;
				}
				checkIncludeExplicitCreditsOnly.Enabled=false;
				checkIncludeExplicitCreditsOnly.Checked=false;
				groupBoxFiltering.Enabled=false;
				gridCharges.AllowSelection=false;
			}
			else {
				checkShowAll.Enabled=true;
				listPayType.Visible=true;
				butPay.Text="Pay";
				comboGroupBy.Enabled=true;
				butCreatePartial.Visible=true;
				butCreatePartial.Text="Add Partials";
				butPay.Visible=true;
				checkIncludeExplicitCreditsOnly.Enabled=true;
				groupBoxFiltering.Enabled=true;
				gridCharges.AllowSelection=true;
			}
			CheckUIState();
			SetComboDepositAccounts();
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			//_listUserClinicNums contains all clinics the user has access to as well as ClinicNum 0 for 'none'
			_paymentCur.ClinicNum=comboClinic.SelectedClinicNum;
			if(_listSplitsCur.Count>0) {
				if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Change clinic for all splits?")) {
					return;
				}
				_listSplitsCur.ForEach(x => x.ClinicNum=_paymentCur.ClinicNum);
				Reinitialize();
			}
			CheckUIState();
		}

		private void comboGroupBy_SelectionChangeCommitted(object sender,EventArgs e) {
			//Go through and disable/enable filters depending on the group by state.
			if(comboGroupBy.SelectedIndex==1) {	//Group By Providers
				comboTypeFilter.ArraySelectedIndices=new int[] { 0 };	//Make sure "All" is selected.
				comboClinicFilter.IsAllSelected=true;
				comboTypeFilter.Enabled=false;
				comboClinicFilter.Enabled=false;
			}
			else if(comboGroupBy.SelectedIndex==2) {	//Group by providers and clinics
				comboTypeFilter.ArraySelectedIndices=new int[] { 0 };
				comboTypeFilter.Enabled=false;
				comboClinicFilter.Enabled=true;
			}
			else {		//Not grouping by anything
				comboTypeFilter.Enabled=true;
				comboClinicFilter.Enabled=true;
			}
			Reinitialize();
		}
		#endregion

		#region Methods - Private
		private void AddCreditCardsToCombo(List<CreditCard> listCreditCards,Func<CreditCard,bool> funcSelectCard=null) {
			comboCreditCards.Items.Clear();
			for(int i=0;i<listCreditCards.Count;i++) {
				string cardNum=listCreditCards[i].CCNumberMasked;
				if(Regex.IsMatch(cardNum,"^\\d{12}(\\d{0,7})")) { //Credit cards can have a minimum of 12 digits, maximum of 19
					int idxLast4Digits=(cardNum.Length-4);
					cardNum=(new string('X',12))+cardNum.Substring(idxLast4Digits);//replace the first 12 with 12 X's
				}
				if(listCreditCards[i].IsXWeb()) {
					cardNum+=" (XWeb)";
				}
				string tokensForCC=listCreditCards[i].GetTokenString();
				cardNum+=(string.IsNullOrEmpty(tokensForCC) ? "" : " "+tokensForCC);
				comboCreditCards.Items.Add(cardNum,listCreditCards[i]);
				if(funcSelectCard!=null && funcSelectCard(listCreditCards[i])) {
					comboCreditCards.SelectedIndex=i;
				}
			}
			comboCreditCards.Items.Add("New Card",new CreditCard()); //CreditCardNum=0
			if(comboCreditCards.SelectedIndex < 0) {
				comboCreditCards.SelectedIndex=comboCreditCards.Items.Count-1;
			}
		}

		private void AddIndexToDictForPaySplit(int index,List<AccountEntry> listAccountEntries,Dictionary<PaySplitHelper,List<int>> dictPaySplitIndices) {
			//Loop through all splits in the collection and make sure that each split is associated to this account entry index.
			foreach(AccountEntry accountEntry in listAccountEntries) {
				AddIndexToDictForPaySplit(index,accountEntry,dictPaySplitIndices);
			}
		}

		private void AddIndexToDictForPaySplit(int index,AccountEntry accountEntry,Dictionary<PaySplitHelper,List<int>> dictPaySplitIndices) {
			//Loop through all splits in the collection and make sure that each split is associated to this account entry index.
			foreach(PaySplit paySplit in accountEntry.SplitCollection) {
				PaySplitHelper paySplitHelper=new PaySplitHelper(paySplit);
				if(dictPaySplitIndices.TryGetValue(paySplitHelper,out List<int> listIndices)) {
					if(!listIndices.Contains(index)) {
						dictPaySplitIndices[paySplitHelper].Add(index);
					}
				}
				else {
					dictPaySplitIndices[paySplitHelper]=new List<int>() { index };
				}
			}
		}

		///<summary>Adds one split to _listPaySplits to work with.  Does not link the payment plan, that must be done outside this method.
		///Called when checkPayPlan click, or upon load if auto attaching to payplan, or upon OK click if no splits were created.</summary>
		private bool AddOneSplit(bool promptForPayPlan=false) {
			PaySplit paySplitCur=new PaySplit();
			paySplitCur.PatNum=_patCur.PatNum;
			paySplitCur.PayNum=_paymentCur.PayNum;
			paySplitCur.DatePay=_paymentCur.PayDate;//this may be updated upon closing
			if(PrefC.GetInt(PreferenceName.RigorousAccounting) < (int)RigorousAccounting.DontEnforce) {
				paySplitCur.ProvNum=0;
				paySplitCur.UnearnedType=Preferences.GetLong(PreferenceName.PrepaymentUnearnedType);//Use default unallocated type
			}
			else { 
				paySplitCur.ProvNum=Patients.GetProvNum(_patCur);
			}
			paySplitCur.ClinicNum=_paymentCur.ClinicNum;
			paySplitCur.SplitAmt=PIn.Double(textAmount.Text);
			if(promptForPayPlan && _loadData.ListValidPayPlans.Count > 0) {
				FormPayPlanSelect FormPPS=new FormPayPlanSelect(_loadData.ListValidPayPlans,true);
				FormPPS.ShowDialog();
				if(FormPPS.DialogResult!=DialogResult.OK) {
					return false;
				}
				paySplitCur.PayPlanNum=FormPPS.SelectedPayPlanNum;
			}
			_listSplitsCur.Add(paySplitCur);
			_paymentCur.PayAmt=PIn.Double(textAmount.Text);
			return true;
		}

		///<summary>Creates paysplits associated to the patient passed in for the current payment until the payAmt has been met.  
		///Returns the list of new paysplits that have been created.  PaymentAmt will attempt to move toward 0 as paysplits are created.</summary>
		private List<PaySplit> AutoSplitForPayment(DateTime date,PaymentEdit.LoadData loadData=null) {
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(_listPatNums,_patCur.PatNum,_listSplitsCur,_paymentCur
				,ListEntriesPayFirst,checkPayTypeNone.Checked,_preferCurrentPat,loadData);
			//Create Auto-splits for the current payment to any remaining non-zero charges FIFO by date.
			//At this point we have a list of procs, positive adjustments, and payplancharges that require payment if the Amount>0.   
			//Create and associate new paysplits to their respective charge items.
			PaymentEdit.AutoSplit autoSplit=PaymentEdit.AutoSplitForPayment(constructResults);
			_listAccountCharges=autoSplit.ListAccountCharges;
			_listSplitsCur=autoSplit.ListSplitsCur;
			_paymentCur.PayAmt=autoSplit.Payment.PayAmt;
			return autoSplit.ListAutoSplits;
		}

		///<summary>Returns true if the user can add a new credit card.</summary>
		private bool CanAddNewCreditCard(Program prog,string progPropertyDescription) {
			if(comboCreditCards.GetSelected<CreditCard>()==null) {
				MessageBox.Show("Invalid credit card selected.");
				return false;
			}
			bool hasPreventCcAdd=PIn.Bool(ProgramProperties.GetPropVal(prog.Id,progPropertyDescription,_paymentCur.ClinicNum));
			CreditCard ccSelected=comboCreditCards.GetSelected<CreditCard>();
			if(ccSelected==null) {
				return !hasPreventCcAdd;
			}
			bool hasToken=false;
			if(prog.Name==ProgramName.Xcharge.ToString() && !string.IsNullOrEmpty(ccSelected.XChargeToken)) {
				hasToken=true;
			}
			else if(prog.Name==ProgramName.PayConnect.ToString() && !string.IsNullOrEmpty(ccSelected.PayConnectToken)) {
				hasToken=true;
			}
			else if(prog.Name==ProgramName.PaySimple.ToString() && !string.IsNullOrEmpty(ccSelected.PaySimpleToken)) {
				hasToken=true;
			}
			if(hasPreventCcAdd && (ccSelected.CreditCardNum==0 || !hasToken)) {
				MessageBox.Show("Cannot add a new credit card.");
				return false;
			}
			return true;
		}

		///<summary>Takes an ODGrid and evaluates its single click events - proccessing selections based on paySplit type and entry type.
		///This method requires the passed in grids to have the same number of rows.</summary>
		private void CellClickHelper(ODGrid grid) {
			gridSplits.SetSelected(false);
			List<AccountEntry> listSelectedCharges=new List<AccountEntry>();
			if(comboGroupBy.SelectedIndex>0) {
				foreach(List<AccountEntry> listAccountEntries in grid.SelectedTags<List<AccountEntry>>()) {
					listSelectedCharges.AddRange(listAccountEntries);
				}
			}
			else {
				foreach(AccountEntry accountEntry in grid.SelectedTags<AccountEntry>()) {
					listSelectedCharges.Add(accountEntry);
				}
			}
			SelectPaySplitsForAccountEntries(listSelectedCharges);
			if(grid==gridCharges) {
				textChargeTotal.Text=listSelectedCharges.Sum(x => x.AmountEnd).ToString("f");
			}
		}

		///<summary>Mimics FormClaimPayEdit.CheckUIState().</summary>
		private void CheckUIState() {
			_xProg=Programs.GetCur(ProgramName.Xcharge);
			_xPath=Programs.GetProgramPath(_xProg);
			Program progPayConnect=Programs.GetCur(ProgramName.PayConnect);
			Program progPaySimple=Programs.GetCur(ProgramName.PaySimple);
			if(_xProg==null || progPayConnect==null || progPaySimple==null) {//Should not happen.
				panelXcharge.Visible=(_xProg!=null);
				butPayConnect.Visible=(progPayConnect!=null);
				butPaySimple.Visible=(progPaySimple!=null);
				return;
			}
			panelXcharge.Visible=false;
			butPayConnect.Visible=false;
			butPaySimple.Visible=false;
			if(checkPayTypeNone.Checked) {
				return;
			}
			if(!progPayConnect.Enabled && !_xProg.Enabled && !progPaySimple.Enabled) {//if none enabled
				//show all so user can pick
				panelXcharge.Visible=true;
				butPayConnect.Visible=true;
				butPaySimple.Visible=true;
				return;
			}
			//show if enabled.  User could have all enabled.
			if(progPayConnect.Enabled) {
				//if clinics are disabled, PayConnect is enabled if marked enabled
				if(!PrefC.HasClinicsEnabled) {
					butPayConnect.Visible=true;
				}
				else {//if clinics are enabled, PayConnect is enabled if the PaymentType is valid and the Username and Password are not blank
					string paymentType=ProgramProperties.GetPropVal(progPayConnect.Id,"PaymentType",_paymentCur.ClinicNum);
					if(!string.IsNullOrEmpty(ProgramProperties.GetPropVal(progPayConnect.Id,"Username",_paymentCur.ClinicNum))
						&& !string.IsNullOrEmpty(ProgramProperties.GetPropVal(progPayConnect.Id,"Password",_paymentCur.ClinicNum))
						&& _listPaymentTypeDefs.Any(x => x.Id.ToString()==paymentType))
					{
						butPayConnect.Visible=true;
					}
				}
			}
			//show if enabled.  User could have both enabled.
			if(_xProg.Enabled) {
				//if clinics are disabled, X-Charge is enabled if marked enabled
				if(!PrefC.HasClinicsEnabled) {
					panelXcharge.Visible=true;
				}
				else {//if clinics are enabled, X-Charge is enabled if the PaymentType is valid and the Username and Password are not blank
					string paymentType=ProgramProperties.GetPropVal(_xProg.Id,"PaymentType",_paymentCur.ClinicNum);
					if(!string.IsNullOrEmpty(ProgramProperties.GetPropVal(_xProg.Id,"Username",_paymentCur.ClinicNum))
						&& !string.IsNullOrEmpty(ProgramProperties.GetPropVal(_xProg.Id,"Password",_paymentCur.ClinicNum))
						&& _listPaymentTypeDefs.Any(x => x.Id.ToString()==paymentType))
					{
						panelXcharge.Visible=true;
					}
				}
			}
			if(progPaySimple.Enabled) {
				//if clinics are disabled, PaySimple is enabled if marked enabled
				if(!PrefC.HasClinicsEnabled) {
					butPaySimple.Visible=true;
				}
				else {//if clinics are enabled, PaySimple is enabled if the PaymentType is valid and the Username and Key are not blank
					string paymentType=ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.Id,PaySimple.PropertyDescs.PaySimplePayTypeCC,_paymentCur.ClinicNum);
					if(!string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.Id,PaySimple.PropertyDescs.PaySimpleApiUserName,_paymentCur.ClinicNum))
						&& !string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.Id,PaySimple.PropertyDescs.PaySimpleApiKey,_paymentCur.ClinicNum))
						&& _listPaymentTypeDefs.Any(x => x.Id.ToString()==paymentType)) {
						butPaySimple.Visible=true;
					}
				}
			}
			if(panelXcharge.Visible==false && butPayConnect.Visible==false && butPaySimple.Visible==false) {
				//This is an office with clinics and one of the payment processing bridges is enabled but this particular clinic doesn't have one set up.
				if(_xProg.Enabled) {
					panelXcharge.Visible=true;
				}
				if(progPayConnect.Enabled) {
					butPayConnect.Visible=true;
				}
				if(progPaySimple.Enabled) {
					butPaySimple.Visible=true;
				}
			}
		}

		///<summary>Creates a split similar to how CreateSplitsForPayment does it, but with selected rows of the grid.
		///If payAmt==0, attempt to pay charge in full.</summary>
		private void CreateSplit(AccountEntry charge,decimal payAmt,bool isManual=false) {
			PaymentEdit.PayResults createdSplit=PaymentEdit.CreatePaySplit(charge,payAmt,_paymentCur,PIn.Decimal(textAmount.Text),_listAccountCharges,
				isManual);
			_listSplitsCur.AddRange(createdSplit.ListSplitsCur);
			_listAccountCharges=createdSplit.ListAccountCharges;
			_paymentCur=createdSplit.Payment;
		}

		///<summary>A method which, for a given grid, allows the user to split a payment between procedures on it.</summary>
		private void CreatePartialSplitClickHelper(ODGrid grid) {
			if(comboGroupBy.SelectedIndex > 0) {
				foreach(List<AccountEntry> listSelectedEntries in grid.SelectedTags<List<AccountEntry>>()) {
					CreatPartialSplitForAccountEntries(listSelectedEntries.ToArray());
				}
			}
			else {
				foreach(AccountEntry selectedEntry in grid.SelectedTags<AccountEntry>()) {
					CreatPartialSplitForAccountEntries(selectedEntry);
				}
			}
		}

		///<summary></summary>
		private void CreatPartialSplitForAccountEntries(params AccountEntry[] arrayAccountEntries) {
			FormAmountEdit FormAE=new FormAmountEdit(GetCodesDescriptForEntries(10,arrayAccountEntries));
			//Suggest the maximum amount remaining for all of the account entries passed in.
			decimal amountEndTotal=arrayAccountEntries.Sum(x => x.AmountEnd);
			FormAE.Amount=amountEndTotal;
			FormAE.ShowDialog();
			if(FormAE.DialogResult==DialogResult.OK) {
				decimal amount=FormAE.Amount;
				
				//Warn the user if they chose to overpay the selected account entries which will put them into the negative.
				if (amountEndTotal < amount)
				{
					var result = MessageBox.Show(this,
						"One or more Outstanding Charges will be negative. Continue?", "Overpaid Charge Warning",
						MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					if (result == DialogResult.No)
					{
						return;
					}
				}

				foreach(AccountEntry entry in arrayAccountEntries) {
					if(amount.IsLessThanOrEqualToZero()) {
						return;
					}
					int splitCountOld=_listSplitsCur.Count;
					CreateSplit(entry,amount,true);
					if(_listSplitsCur.Count > 0 && splitCountOld!=_listSplitsCur.Count) {
						amount-=(decimal)_listSplitsCur.Last().SplitAmt;
					}
				}
			}
		}

		private MigraDoc.DocumentObjectModel.Document CreatePDFDoc(string receiptStr) {
			string[] receiptLines=receiptStr.Split(new string[] { "\r\n" },StringSplitOptions.None);
			MigraDoc.DocumentObjectModel.Document doc=new MigraDoc.DocumentObjectModel.Document();
			doc.DefaultPageSetup.PageWidth=Unit.FromInch(3.0);
			doc.DefaultPageSetup.PageHeight=Unit.FromInch(0.181*receiptLines.Length+0.56);//enough to print text plus 9/16 in. (0.56) extra space at bottom.
			doc.DefaultPageSetup.TopMargin=Unit.FromInch(0.25);
			doc.DefaultPageSetup.LeftMargin=Unit.FromInch(0.25);
			doc.DefaultPageSetup.RightMargin=Unit.FromInch(0.25);
			MigraDoc.DocumentObjectModel.Font bodyFontx=MigraDocHelper.CreateFont(8,false);
			bodyFontx.Name=FontFamily.GenericMonospace.Name;
			Section section=doc.AddSection();
			Paragraph par=section.AddParagraph();
			ParagraphFormat parformat=new ParagraphFormat();
			parformat.Alignment=ParagraphAlignment.Left;
			parformat.Font=bodyFontx;
			par.Format=parformat;
			par.AddFormattedText(receiptStr,bodyFontx);
			return doc;
		}

		///<summary>Deletes selected paysplits from the grid and attributes amounts back to where they originated from.
		///This will return a list of payment plan charges that were affected. This is so that splits can be correctly re-attributed to the payplancharge
		///when the user edits the paysplit. There should only ever be one payplancharge in that list, since the user can only edit one split at a time.</summary>
		private void DeleteSelected(PaySplit paySplitToBeAdded=null,bool doCreateSecLog=true) {
			bool suppressMessage=false;
			foreach(PaySplit paySplit in gridSplits.SelectedTags<PaySplit>()) {
				if(paySplit.SplitNum!=0 && _loadData.ListPaySplitAllocations.Any(x => x.FSplitNum==paySplit.SplitNum && x.PayNum!=paySplit.PayNum)) {
					MessageBox.Show("Splits in this payment have been allocated elsewhere.  Please delete those first.");
					return;
				}
				if(paySplit.DateEntry!=DateTime.MinValue && !Security.IsAuthorized(Permissions.PaymentEdit,paySplit.DatePay,suppressMessage)) {
					suppressMessage=true;
					continue;//Don't delete this paysplit
				}
				if(paySplitToBeAdded!=null) {
					//We need to do this in order to maintain object references on form closing.
					UpdateListAssociationsBeforeDelete(paySplit,paySplitToBeAdded);
				}
				else {
					//Remove pay split associated if Split is deleted and not replaced.
					_listPaySplitsAssociated.RemoveAll(x => x.PaySplitOrig==paySplit || x.PaySplitLinked==paySplit);
				}
				_loadData.ListSplits.Remove(paySplit);
				_listSplitsCur.Remove(paySplit);
				if(doCreateSecLog && paySplit.SplitNum!=0) { 
					_listPaySplitsForSecLog.Add(paySplit);
				}
			}
			Reinitialize();
		}

		///<summary>Returns true if the AccountEntry matches the currently selected filters.</summary>
		private bool DoShowAccountEntry(AccountEntry entryCharge) {
			if(!_listFilteredPatNums.Contains(entryCharge.PatNum)) {
				return false;
			}
			List<long> listProvNums=comboProviderFilter.GetSelectedProvNums();
			if(!listProvNums.Contains(entryCharge.ProvNum)) {
				return false;
			}
			if(PrefC.HasClinicsEnabled && comboGroupBy.SelectedIndex!=1 && !_listFilteredClinics.Contains(entryCharge.ClinicNum)) {
				return false;
			}
			//proc code filter
			if(_listFilteredProcCodes.Count>0
				&& (entryCharge.Tag.GetType()!=typeof(Procedure) || !_listFilteredProcCodes.Contains(((Procedure)entryCharge.Tag).CodeNum)))
			{
				return false;
			}
			//Charge Amount Filter
			if(amtMaxEnd.Value!=0 && entryCharge.AmountEnd > amtMaxEnd.Value) {
				return false;
			}
			//Charge Amount Filter
			if(amtMinEnd.Value!=0 && entryCharge.AmountEnd < amtMinEnd.Value) {
				return false;
			}
			//daterange filter
			if((entryCharge.Date.Date < datePickFrom.Value.Date) || (entryCharge.Date.Date > datePickTo.Value.Date)) { 
				return false;
			}
			//Type Filter
			if(!_listFilteredType.Contains(entryCharge.GetType().Name)) {
				return false;
			}
			return true;
		}

		private GridRow FillChargesHelper(List<AccountEntry> listEntriesForRow,bool includeClinic) {
			AccountEntry entryCharge=listEntriesForRow.First();
			GridRow row=new GridRow();
			row.Tag=listEntriesForRow;
			row.Cells.Add(Providers.GetAbbr(entryCharge.ProvNum));//Provider
			if(checkPayTypeNone.Checked) {
				if(!_dictPatients.TryGetValue(entryCharge.PatNum,out Patient pat)) {
					pat=Patients.GetLim(entryCharge.PatNum);
					_dictPatients[pat.PatNum]=pat;
				}
				row.Cells.Add(pat.GetNameLFnoPref());//patient
			}
			if(includeClinic) {
				row.Cells.Add(Clinics.GetAbbr(entryCharge.ClinicNum));
			}
			int procCodeLimit=(includeClinic ? 9 : 10);//this column is shorter when filtering by prov + clinic
			row.Cells.Add(GetCodesDescriptForEntries(procCodeLimit,listEntriesForRow.ToArray()));//ProcCodes
			row.Cells.Add(listEntriesForRow.Sum(x => x.AmountEnd).ToString("f"));//Amount End
			return row;
		}

		private void FillCreditCards() {
			AddCreditCardsToCombo(_listCreditCards);
			comboCreditCards.SelectedIndex=0;
			bool isXWebCardPresent=_listCreditCards.Any(x => x.IsXWeb());
			bool isPayConnectPortalCardPresent=_listCreditCards.Any(x => x.IsPayConnectPortal());
			_xWebResponse=_loadData.XWebResponse;
			_payConnectResponseWeb=_loadData.PayConnectResponseWeb;
			string _payConnectTransType="";
			if(_payConnectResponseWeb!=null && _payConnectResponseWeb.IsFromWebPortal) {
				_payConnectTransType=_payConnectResponseWeb.TransType.ToString();
			}
			groupXWeb.Visible=false;
			if(isXWebCardPresent || _xWebResponse!=null 
				|| _isPayConnectPortal || _payConnectResponseWeb!=null) 
			{
				groupXWeb.Visible=true;
			}
			//PayConnect will only let you void a payment in the first 25 minutes.
			if((_payConnectResponseWeb==null || _payConnectTransType==PayConnectService.transType.VOID.ToString() || DateTime.Now > _payConnectResponseWeb.DateTimeEntry.AddMinutes(25))
				&& (_xWebResponse==null || _xWebResponse.XTransactionType==XWebTransactionType.CreditVoidTransaction)) 
			{
				//Can't run an XWeb/PayConnect void unless this payment is attached to a non-void transaction.
				butVoid.Visible=false;
				groupXWeb.Height=55;
			}
			if(!isXWebCardPresent && _payConnectResponseWeb==null) {
				butReturn.Visible=false;
				butVoid.Location=butReturn.Location;
				groupXWeb.Height=55;
			}
		}

		///<summary>Fills the combo boxes with correct values.</summary>
		private void FillFilters() {
			//Fill min/max date/amt
			//If there are no account charges, the date time will be DateTime.MinimumDate
			//Get latest proc, or tomorrows date, whichever is later.
			//Ignore account entries with a Date set to MaxValue (TP PayPlanCharge account entries).
			List<DateTime> listAccountEntryDates=_listAccountCharges
				.Where(x => x.Date>=datePickFrom.MinDate && x.Date.Date!=DateTime.MaxValue.Date)
				.Select(x => x.Date)
				.ToList();
			if(listAccountEntryDates.IsNullOrEmpty()) {
				datePickFrom.Value=datePickFrom.Value;
				datePickTo.Value=DateTime.Today;
			}
			else {
				datePickFrom.Value=listAccountEntryDates.Min();
				datePickTo.Value=ODMathLib.Max(listAccountEntryDates.Max(),DateTime.Today);
			}
			amtMinEnd.Value=0;
			amtMaxEnd.Value=0; 
			//Fill Patient Combo
			comboPatientFilter.Items.Clear();
			comboPatientFilter.Items.Add(new ODBoxItem<Patient>("All"));
			//Fill the patient filter combo box with the known patients relating to the list of account charges.
			foreach(long patNum in _listAccountCharges.Select(x => x.PatNum).Distinct()) {
				if(_dictPatients.TryGetValue(patNum,out Patient pat)) {
					comboPatientFilter.Items.Add(new ODBoxItem<Patient>(pat.GetNameFirstOrPreferred(),pat));
				}
			}
			comboPatientFilter.SetSelected(0,true);
			//Fill Provider Combo
			comboProviderFilter.Items.Clear();
			comboProviderFilter.IncludeAll=true;
			comboProviderFilter.Items.AddProvNone();
			List<Provider> listProviders=_listAccountCharges.Select(x => Providers.FirstOrDefault(y => y.Id==x.ProvNum))
				.Where(x => x!=null)
				.DistinctBy(x => x.Id)
				.ToList();
			comboProviderFilter.Items.AddProvsAbbr(listProviders);
			comboProviderFilter.IsAllSelected=true;
			//Fill Clinics Combo
			if(PrefC.HasClinicsEnabled) {
				List<Clinic> listClinicsTemp=Clinics.GetAll(true);
				listClinicsTemp.Add(new Clinic() {Abbr="Unassigned"});
				comboClinicFilter.Items.Clear();
				foreach(Clinic clinic in _listAccountCharges.Select(x => listClinicsTemp.FirstOrDefault(y => y.Id==x.ClinicNum))
					.DistinctBy(x => x.Id))
				{
					comboClinicFilter.Items.Add(clinic.Abbr,clinic);
				}
				comboClinicFilter.IsAllSelected=true;
			}
			//Fill Type Combo
			comboTypeFilter.Items.Clear();
			comboTypeFilter.Items.Add("All");
			comboTypeFilter.Items.AddRange(_listAccountCharges.Select(x => x.GetType().Name).Distinct().ToList());
			comboTypeFilter.SetSelected(0,true);
		}

		private void FillGridAllocated() {
			if(_loadData.ListPaySplitAllocations.Count==0) {
				return;
			}
			gridAllocated.BeginUpdate();
			gridAllocated.Columns.Clear();
			GridColumn col=new GridColumn("Date",80);
			gridAllocated.Columns.Add(col);
			col=new GridColumn("Clinic",80);
			gridAllocated.Columns.Add(col);
			col=new GridColumn("Patient",140);
			gridAllocated.Columns.Add(col);
			col=new GridColumn("Amount",80,HorizontalAlignment.Right);
			gridAllocated.Columns.Add(col);
			col=new GridColumn("Unearned",50);
			gridAllocated.Columns.Add(col);
			gridAllocated.Rows.Clear();
			GridRow row;
			foreach(PaySplit paySplit in _loadData.ListPaySplitAllocations) {
				row=new GridRow();
				row.Cells.Add(paySplit.DatePay.ToShortDateString());
				row.Cells.Add(Clinics.GetAbbr(paySplit.ClinicNum));
				row.Cells.Add(_curFamOrSuperFam.GetNameInFamFL(paySplit.PatNum));
				row.Cells.Add(paySplit.SplitAmt.ToString("F"));
				row.Cells.Add(Definitions.GetName(DefinitionCategory.PaySplitUnearnedType,paySplit.UnearnedType));//handles 0 just fine
				gridAllocated.Rows.Add(row);
			}
			gridAllocated.EndUpdate();
		}

		///<summary></summary>
		private void FillGridCharges() {
			//Fill right-hand grid with all the charges, filtered based on checkbox and filters.
			gridCharges.BeginUpdate();
			gridCharges.Columns.Clear();
			_dictGridChargesPaySplitIndices.Clear();
			GridColumn col;
			decimal chargeTotal=0;
			List<AccountEntry> listOutstandingCharges=_listAccountCharges.FindAll(x => DoShowAccountEntry(x)
				&& (x.GetType()!=typeof(Procedure) || (x.GetType()==typeof(Procedure) && ((Procedure)x.Tag).ProcStatus==ProcStat.C)));
			#region Group By Provider
			if(comboGroupBy.SelectedIndex==1) {//Group by 'Provider'
				col=new GridColumn("Prov",checkPayTypeNone.Checked?70:110);
				gridCharges.Columns.Add(col);
				if(checkPayTypeNone.Checked) {
					col=new GridColumn("Patient",119);
					gridCharges.Columns.Add(col);
				}
				col=new GridColumn("Codes",50){ IsWidthDynamic=true,DynamicWeight=1 };
				gridCharges.Columns.Add(col);
				col=new GridColumn("Amt End",70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.Columns.Add(col);
				gridCharges.Rows.Clear();
				var dictProvPatEntries=listOutstandingCharges.Where(x => (checkShowAll.Checked || !x.AmountAvailable.IsZero()))
					.GroupBy(x => new { x.ProvNum,x.PatNum })
					.ToDictionary(x => x.Key,x => x.ToList());
				foreach(var key in dictProvPatEntries.Keys) {
					foreach(AccountEntry accountEntry in dictProvPatEntries[key]) {
						AddIndexToDictForPaySplit(gridCharges.Rows.Count,accountEntry,_dictGridChargesPaySplitIndices);
					}
					gridCharges.Rows.Add(FillChargesHelper(dictProvPatEntries[key],false));
					chargeTotal+=dictProvPatEntries[key].Sum(x => x.AmountEnd);
				}
			}
			#endregion
			#region Group By Clinic and Provider
			else if(comboGroupBy.SelectedIndex==2) {//Group by 'Clinic and Provider'
				col=new GridColumn("Prov",checkPayTypeNone.Checked?70:100);
				gridCharges.Columns.Add(col);
				if(checkPayTypeNone.Checked) {
					col=new GridColumn("Patient",100);
					gridCharges.Columns.Add(col);
				}
				col=new GridColumn("Clinic",60);
				gridCharges.Columns.Add(col);
				col=new GridColumn("Codes",50){ IsWidthDynamic=true };
				gridCharges.Columns.Add(col);
				col=new GridColumn("Amt End",70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.Columns.Add(col);
				gridCharges.Rows.Clear();
				var dictProvPatClinicEntries=listOutstandingCharges.Where(x => (checkShowAll.Checked || !x.AmountAvailable.IsZero()))
					.GroupBy(x => new { x.ProvNum,x.PatNum,x.ClinicNum })
					.ToDictionary(x => x.Key,x => x.ToList());
				foreach(var key in dictProvPatClinicEntries.Keys) {
					foreach(AccountEntry accountEntry in dictProvPatClinicEntries[key]) {
						AddIndexToDictForPaySplit(gridCharges.Rows.Count,accountEntry,_dictGridChargesPaySplitIndices);
					}
					gridCharges.Rows.Add(FillChargesHelper(dictProvPatClinicEntries[key],true));
					chargeTotal+=dictProvPatClinicEntries[key].Sum(x => x.AmountEnd);
				}
			}
			#endregion
			#region Group By None
			else { //Group by 'None'
				col=new GridColumn("Date",65,GridSortingStrategy.DateParse);
				gridCharges.Columns.Add(col);
				col=new GridColumn("Patient",92,GridSortingStrategy.StringCompare);
				gridCharges.Columns.Add(col);
				col=new GridColumn("Prov",40,GridSortingStrategy.StringCompare);
				gridCharges.Columns.Add(col);
				if(PrefC.HasClinicsEnabled) {//Clinics
					col=new GridColumn("Clinic",55,GridSortingStrategy.StringCompare);
					gridCharges.Columns.Add(col);
				}
				col=new GridColumn("Code",45,GridSortingStrategy.StringCompare);
				gridCharges.Columns.Add(col);
				col=new GridColumn("Tth",25,GridSortingStrategy.ToothNumberParse);
				gridCharges.Columns.Add(col);
				col=new GridColumn("Type",90,GridSortingStrategy.StringCompare);
				gridCharges.Columns.Add(col);
				col=new GridColumn("AmtOrig",55,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.Columns.Add(col);
				col=new GridColumn("AmtAvail",57,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.Columns.Add(col);
				col=new GridColumn("AmtEnd",55,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
				gridCharges.Columns.Add(col);
				gridCharges.Rows.Clear();
				GridRow row;
				foreach(AccountEntry entryCharge in listOutstandingCharges) {
					//Filter out those that are paid in full and from other payments if checkbox unchecked.
					if(!checkShowAll.Checked && entryCharge.AmountEnd.IsZero()) {
						bool doShowZeroCharge=false;
						foreach(PaySplit paySplit in gridSplits.GetTags<PaySplit>()) {
							if(entryCharge.SplitCollection.Contains(paySplit)) {
								//Charge is paid for by a split in this payment, display it.
								if(entryCharge.GetType()==typeof(Procedure) && paySplit.PayPlanNum!=0) {
									//Don't show the charge if it's a proc being paid by a payplan split.
									//From the user's perspective they're paying the "debits" not the procs.
								}
								else {
									doShowZeroCharge=true;
									break;
								}
							}
							else if(entryCharge.GetType()==typeof(FauxAccountEntry)
								&& paySplit.PayPlanNum==((FauxAccountEntry)entryCharge).PayPlanNum
								&& !entryCharge.AmountAvailable.IsZero())
							{
								doShowZeroCharge=true;
								break;
							}
						}
						if(!doShowZeroCharge) {
							continue;
						}
					}
					row=new GridRow();
					row.Tag=entryCharge;
					row.Cells.Add(entryCharge.Date.ToShortDateString());//Date
					if(!_dictPatients.TryGetValue(entryCharge.PatNum,out Patient patCur)) {
						patCur=Patients.GetLim(entryCharge.PatNum);
						_dictPatients[patCur.PatNum]=patCur;
					}
					string patName=patCur.GetNameLFnoPref();
					if(entryCharge.Tag.GetType()==typeof(FauxAccountEntry)
						&& ((FauxAccountEntry)entryCharge).Guarantor > 0
						&& ((FauxAccountEntry)entryCharge).Guarantor!=patCur.PatNum)
					{
						if(!_dictPatients.TryGetValue(((FauxAccountEntry)entryCharge).Guarantor,out Patient guarantor)) {
							guarantor=Patients.GetLim(((FauxAccountEntry)entryCharge).Guarantor);
							_dictPatients[guarantor.PatNum]=guarantor;
						}
						patName+="\r\n"+"Guar"+": "+guarantor.GetNameLFnoPref();
					}
					row.Cells.Add(patName);//Patient
					row.Cells.Add(Providers.GetAbbr(entryCharge.ProvNum));//Provider
					if(PrefC.HasClinicsEnabled) {//Clinics
						row.Cells.Add(Clinics.GetAbbr(entryCharge.ClinicNum));
					}
					string procCode="";
					string tth="";
					Procedure proc=null;
					if(entryCharge.Tag.GetType()==typeof(Procedure)) {
						proc=(Procedure)entryCharge.Tag;
						tth=proc.ToothNum=="" ? proc.Surf : Tooth.ToInternat(proc.ToothNum);
						procCode+=ProcedureCodes.GetStringProcCode(proc.CodeNum);
					}
					row.Cells.Add(procCode);//ProcCode
					row.Cells.Add(tth);
					if(entryCharge.GetType()==typeof(PaySplit)) {
						row.Cells.Add("Unallocated");
					}
					else if(entryCharge.GetType()==typeof(Procedure)) {
						//Get the proc and add its description if the row is a proc.
						row.Cells.Add("Proc"+": "+Procedures.GetDescription(proc));
					}
					else if(entryCharge.Tag.GetType()==typeof(FauxAccountEntry)) {
						string strFaux="PayPlanCharge";
						FauxAccountEntry fauxAccountEntry=(FauxAccountEntry)entryCharge;
						if(fauxAccountEntry.AccountEntryProc!=null
							&& fauxAccountEntry.AccountEntryProc.Tag!=null
							&& fauxAccountEntry.AccountEntryProc.GetType()==typeof(Procedure))
						{
							strFaux+="\r\n"+"Proc"+": "+Procedures.GetDescription((Procedure)fauxAccountEntry.AccountEntryProc.Tag);
						}
						else if(fauxAccountEntry.IsAdjustment) {
							strFaux+="\r\n"+"Adjustment";
						}
						else if(fauxAccountEntry.Interest.IsGreaterThanZero()) {
							strFaux+="\r\n"+"Interest";
						}
						else if(entryCharge.ProcNum==0 && entryCharge.AdjNum==0) {
							strFaux+="\r\n"+"Unattached";
						}
						row.Cells.Add(strFaux);
					}
					else {
						row.Cells.Add(entryCharge.GetType().Name);//Type
					}
					row.Cells.Add(entryCharge.AmountOriginal.ToString("f"));//Amount Original
					row.Cells.Add(entryCharge.AmountAvailable.ToString("f"));//Amount Start
					row.Cells.Add(entryCharge.AmountEnd.ToString("f"));//Amount End
					chargeTotal+=entryCharge.AmountEnd;
					//Associate every single split for every single account entry that matches this PayPlanChargeNum
					if(entryCharge.PayPlanChargeNum > 0) {
						AddIndexToDictForPaySplit(gridCharges.Rows.Count,
							listOutstandingCharges.FindAll(x => x.PayPlanChargeNum==entryCharge.PayPlanChargeNum),
							_dictGridChargesPaySplitIndices);
					}
					else {//Just add the index for the splits associated to the current entryCharge.
						AddIndexToDictForPaySplit(gridCharges.Rows.Count,entryCharge,_dictGridChargesPaySplitIndices);
					}
					gridCharges.Rows.Add(row);
				}
			}
			#endregion
			textChargeTotal.Text=chargeTotal.ToString("f");
			gridCharges.EndUpdate();
		}

		///<summary>Fills the Current Payment Splits grid and then invokes methods to refresh the charges, treat plan, and allocated grids.</summary>
		private void FillGridSplits() {
			//Fill left grid with paysplits created
			List<long> listMissingProcsNums=_listSplitsCur.Where(x => x.ProcNum!=0 && !_loadData.ListProcsForSplits.Any(y => y.ProcNum==x.ProcNum))
				.Select(x => x.ProcNum).ToList();
			_loadData.ListProcsForSplits.AddRange(Procedures.GetManyProc(listMissingProcsNums,false));
			gridSplits.BeginUpdate();
			gridSplits.Columns.Clear();
			_dictGridSplitsPaySplitIndices.Clear();
			GridColumn col;
			col=new GridColumn("Date",65,HorizontalAlignment.Center,GridSortingStrategy.DateParse);
			gridSplits.Columns.Add(col);
			col=new GridColumn("Prov",40, GridSortingStrategy.StringCompare);
			gridSplits.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) {//Clinics
				col=new GridColumn("Clinic",40, GridSortingStrategy.StringCompare);
				gridSplits.Columns.Add(col);
			}
			col=new GridColumn("Patient",100,GridSortingStrategy.StringCompare);
			gridSplits.Columns.Add(col);
			col=new GridColumn("Code",60, GridSortingStrategy.StringCompare);
			gridSplits.Columns.Add(col);
			col=new GridColumn("Type",100, GridSortingStrategy.StringCompare);
			gridSplits.Columns.Add(col);
			col=new GridColumn("Amount",55,HorizontalAlignment.Right, GridSortingStrategy.AmountParse);
			gridSplits.Columns.Add(col);
			gridSplits.Rows.Clear();
			GridRow row;
			decimal splitTotal=0;
			foreach(PaySplit splitCur in _listSplitsCur) {
				PaySplitHelper paySplitHelper=new PaySplitHelper(splitCur);
				int index=gridSplits.Rows.Count;
				splitTotal+=(decimal)splitCur.SplitAmt;
				row=new GridRow();
				row.Tag=splitCur;
				_dictGridSplitsPaySplitIndices[paySplitHelper]=new List<int>() { index };
				row.Cells.Add(splitCur.DatePay.ToShortDateString());//Date
				row.Cells.Add(Providers.GetAbbr(splitCur.ProvNum));//Prov
				if(PrefC.HasClinicsEnabled) {//Clinics
					if(splitCur.ClinicNum!=0) {
						row.Cells.Add(Clinics.GetAbbr(splitCur.ClinicNum));//Clinic
					}
					else {
						row.Cells.Add("");//Clinic
					}
				}
				Patient patCur;
				if(!_dictPatients.TryGetValue(splitCur.PatNum,out patCur)) {
					patCur=Patients.GetLim(splitCur.PatNum);
					_dictPatients[patCur.PatNum]=patCur;
				}
				string patName=patCur.LName + ", " + patCur.FName;
				row.Cells.Add(patName);//Patient
				Procedure proc=new Procedure();
				if(splitCur.ProcNum!=0) {
					proc=_loadData.ListProcsForSplits.FirstOrDefault(x => x.ProcNum==splitCur.ProcNum)??new Procedure();
				}
				row.Cells.Add(ProcedureCodes.GetStringProcCode(proc.CodeNum));//ProcCode
				bool isUnallocated=false;
				List<string> listTypeStrs=new List<string>();
				if(splitCur.PayPlanNum > 0) {
					listTypeStrs.Add("PayPlanCharge");
				}
				if(splitCur.ProcNum > 0) {//Procedure
					listTypeStrs.Add("Proc: "+Procedures.GetDescription(proc));
				}
				else if(splitCur.PayPlanChargeNum > 0) {//Might be payment plan interest when not a procedure.
					//Find the corresponding account entries for this PayPlanChargeNum and see if any have an interest amount that matches this amount.
					//Since we don't have an official way to designate PaySplits as 'interest' splits and since the user can't pick which PayPlanCharge,
					//our best bet is to try and guess which PaySplit is meant for the interest portion of the PayPlanCharge.
					List<FauxAccountEntry> listPayPlanChargeEntries=_listAccountCharges.Where(x => x.GetType()==typeof(FauxAccountEntry)
							&& x.PayPlanChargeNum==splitCur.PayPlanChargeNum)
						.Cast<FauxAccountEntry>()
						.ToList();
					//Principal and interest charges are split up into multiple account entry rows.
					//Always use the AmountAvailable value to equate to the SplitAmt just in case they paid for a portion of the interest in another payment.
					if(listPayPlanChargeEntries.Any(x => !x.Interest.IsZero() && x.AmountAvailable.IsEqual((decimal)splitCur.SplitAmt))) {
						listTypeStrs.Add("(interest)");
					}
				}
				if(splitCur.AdjNum > 0) {
					listTypeStrs.Add("Adjustment");
				}
				if(splitCur.UnearnedType > 0) {
					listTypeStrs.Add(Definitions.GetName(DefinitionCategory.PaySplitUnearnedType,splitCur.UnearnedType));
				}
				if(listTypeStrs.Count==0) {
					isUnallocated=true;
					listTypeStrs.Add("Unallocated");
				}
				row.Cells.Add(string.Join("\r\n",listTypeStrs));
				if(isUnallocated) {
					row.Cells.Last().ForeColor=System.Drawing.Color.Red;
				}
				if(!splitCur.PatNum.In(_famCur.GetPatNums())) {
					listTypeStrs.Add("(split to another family)");
				}
				row.Cells.Add(splitCur.SplitAmt.ToString("f"));//Amount
				gridSplits.Rows.Add(row);
			}
			textSplitTotal.Text=splitTotal.ToString("f");
			gridSplits.EndUpdate();
		}

		///<summary>Fills Treatment Plan Procedures grid.</summary>
		private void FillGridTreatPlan() {
			//Fill right-hand grid with all the TP procedures.
			gridTreatPlan.BeginUpdate();
			gridTreatPlan.Columns.Clear();
			_dictGridTreatPlanPaySplitIndices.Clear();
			GridColumn col;
			#region Group By None
			col=new GridColumn("Date",65,GridSortingStrategy.DateParse);
			gridTreatPlan.Columns.Add(col);
			col=new GridColumn("Patient",92,GridSortingStrategy.StringCompare);
			gridTreatPlan.Columns.Add(col);
			col=new GridColumn("Prov",40,GridSortingStrategy.StringCompare);
			gridTreatPlan.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) {//Clinics
				col=new GridColumn("Clinic",55,GridSortingStrategy.StringCompare);
				gridTreatPlan.Columns.Add(col);
			}
			col=new GridColumn("Code",45,GridSortingStrategy.StringCompare);
			gridTreatPlan.Columns.Add(col);
			col=new GridColumn("Tth",25,GridSortingStrategy.ToothNumberParse);
			gridTreatPlan.Columns.Add(col);
			col=new GridColumn("Type",90,GridSortingStrategy.StringCompare);
			gridTreatPlan.Columns.Add(col);
			col=new GridColumn("AmtOrig",55,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
			gridTreatPlan.Columns.Add(col);
			col=new GridColumn("AmtAvail",57,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
			gridTreatPlan.Columns.Add(col);
			col=new GridColumn("AmtEnd",55,HorizontalAlignment.Right,GridSortingStrategy.AmountParse);
			gridTreatPlan.Columns.Add(col);
			gridTreatPlan.Rows.Clear();
			GridRow row;
			foreach(AccountEntry entryCharge in _listAccountCharges) {
				if(entryCharge.GetType()!=typeof(Procedure) || ((Procedure)entryCharge.Tag).ProcStatus!=ProcStat.TP) {
					continue;
				}
				if(!DoShowAccountEntry(entryCharge)) {
					continue;
				}
				row=new GridRow();
				row.Tag=entryCharge;
				row.Cells.Add(entryCharge.Date.ToShortDateString());//Date
				if(!_dictPatients.TryGetValue(entryCharge.PatNum,out Patient patCur)) {
					patCur=Patients.GetLim(entryCharge.PatNum);
					_dictPatients[patCur.PatNum]=patCur;
				}
				string patName=patCur.GetNameLFnoPref();
				if(entryCharge.Tag.GetType()==typeof(FauxAccountEntry)) {
					if(!_dictPatients.TryGetValue(((FauxAccountEntry)entryCharge).Guarantor,out Patient guarantor)) {
						guarantor=Patients.GetLim(((FauxAccountEntry)entryCharge).Guarantor);
						_dictPatients[patCur.PatNum]=guarantor;
					}
					patName+="\r\n"+"Guar"+": "+guarantor.GetNameLFnoPref();
				}
				row.Cells.Add(patName);//Patient
				row.Cells.Add(Providers.GetAbbr(entryCharge.ProvNum));//Provider
				if(PrefC.HasClinicsEnabled) {//Clinics
					row.Cells.Add(Clinics.GetAbbr(entryCharge.ClinicNum));
				}
				string procCode="";
				string tth="";
				Procedure proc=null;
				if(entryCharge.Tag.GetType()==typeof(Procedure)) {
					proc=(Procedure)entryCharge.Tag;
					tth=proc.ToothNum=="" ? proc.Surf : Tooth.ToInternat(proc.ToothNum);
					procCode+=ProcedureCodes.GetStringProcCode(proc.CodeNum);
				}
				row.Cells.Add(procCode);//ProcCode
				row.Cells.Add(tth);//Tooth Number
				if(entryCharge.GetType()==typeof(PaySplit)) {
					row.Cells.Add("Unallocated");
				}
				else {
					row.Cells.Add(entryCharge.GetType().Name);//Type
				}
				if(entryCharge.GetType()==typeof(Procedure)) {
					//Get the proc and add its description if the row is a proc.
					row.Cells[row.Cells.Count-1].Text="Proc"+": "+Procedures.GetDescription(proc);
				}
				row.Cells.Add(entryCharge.AmountOriginal.ToString("f"));//Amount Original
				row.Cells.Add(entryCharge.AmountAvailable.ToString("f"));//Amount Start
				row.Cells.Add(entryCharge.AmountEnd.ToString("f"));//Amount End
				AddIndexToDictForPaySplit(gridTreatPlan.Rows.Count,entryCharge,_dictGridTreatPlanPaySplitIndices);
				gridTreatPlan.Rows.Add(row);
			}
			#endregion
			gridTreatPlan.EndUpdate();
		}

		///<summary>Populating _listPaySplitsAssociated with splits that are linked to each other.</summary>
		private void FillListPaySplitAssociated() {
			//All paysplits that have an FSplitNum of 0 are original paysplits.
			//Only loop through all allocated paysplits for this particular payment and find their corresponding original paysplit and make a PaySplitAssociated object.
			//We purposefully do not use _loadData.ListPaySplitAllocations because it contains paysplits that are not associated to this particular payment.
			List<PaySplit> listPaySplitAllocations=_listSplitsCur.FindAll(x => x.FSplitNum > 0);
			foreach(PaySplit paySplitAllocated in listPaySplitAllocations) {
				if(paySplitAllocated.FSplitNum==0) {
					continue;
				}
				//Find the corresponding original paysplit for the current allocated paysplit.
				//Look through all of the paysplits that are associated to this particular payment first (prefer _listSplitsCur due to the sync).
				PaySplit psOrig=_listSplitsCur.Find(x => x.SplitNum==paySplitAllocated.FSplitNum);
				if(psOrig==null) {
					//The original is not associated to this particular payment, look through all of the associated prepayment paysplits next.
					psOrig=_loadData.ListPrePaysForPayment.Find(x => x.SplitNum==paySplitAllocated.FSplitNum);
				}
				if(psOrig!=null) {
					_listPaySplitsAssociated.Add(new PaySplits.PaySplitAssociated(psOrig,paySplitAllocated));
				}
			}
		}
		
		///<summary>Called whenever any of the filtering objects are changed.  Rechecks filtering and refreshes the grid.</summary>
		private void FilterChangeCommitted(object sender, EventArgs e) {
			if(_isInit) {
				return;
			}
			FillGridCharges();
			FillGridTreatPlan();
		}

		///<summary>This method goes through the passed in grid and returns the selected account entries in a normalized list of list of account entries.
		///If there are no current selections in the grid passed in then all rows will be treated as selected.
		///This is considered normalized because the Outstanding Charges and Treatment Planned Procedures grids can have a single AccountEntry object
		///per row (not grouped) or a list of AccountEntry objects per row (when using the Group By combo box).</summary>
		private List<List<AccountEntry>> GetAccountEntriesFromGrid(ODGrid grid) {
			List<List<AccountEntry>> listSelectedCharges=new List<List<AccountEntry>>();
			bool hasSelectedIndices=(grid.SelectedIndices.Length > 0);
			if(!hasSelectedIndices) {
				grid.SetSelected(true);//Artificially select every row in the grid.
			}
			if(comboGroupBy.SelectedIndex > 0) {
				foreach(List<AccountEntry> listAccountEntries in grid.SelectedTags<List<AccountEntry>>()) {
					listSelectedCharges.Add(listAccountEntries);
				}
			}
			else {
				foreach(AccountEntry accountEntry in grid.SelectedTags<AccountEntry>()) {
					listSelectedCharges.Add(new List<AccountEntry>() { accountEntry });
				}
			}
			if(!hasSelectedIndices) {
				grid.SetSelected(false);//Deselect all of the rows to preserve old behavior.
			}
			return listSelectedCharges;
		}

		private string GetCodesDescriptForEntries(int procCodeLimit,params AccountEntry[] arrayAccountEntries) {
			List<string> listProcCodes=new List<string>();
			foreach(AccountEntry accountEntry in arrayAccountEntries.Where(x => x.GetType()==typeof(Procedure))) {
				if(listProcCodes.Count>=procCodeLimit) {
					listProcCodes.Add("(...)");
					break;
				}
				string procCode="";
				Procedure procedure=(Procedure)accountEntry.Tag;
				if(procedure.ProcStatus==ProcStat.TP) {
					procCode+="(TP)";//this needs to be handled differently. TP Procs need to be in their own provider grouping
				}
				procCode+=ProcedureCodes.GetStringProcCode(procedure.CodeNum);
				listProcCodes.Add(procCode);
			}
			return string.Join(", ",listProcCodes);
		}

		private string GetXChargeTransactionTypeCommands(int tranType,bool hasXToken,bool notRecurring,CreditCard CCard,string cashBack) {
			string tranText="";
			switch(tranType) {
				case 0:
					tranText+="/TRANSACTIONTYPE:PURCHASE /LOCKTRANTYPE /LOCKAMOUNT ";
					if(hasXToken && CCard!=null) {
						tranText+="/XCACCOUNTID:"+CCard.XChargeToken+" ";
						tranText+="/AUTOPROCESS ";
						tranText+="/GETXCACCOUNTIDSTATUS ";
					}
					if(notRecurring && CCard!=null) {
						tranText+="/ACCOUNT:"+CCard.CCNumberMasked+" ";
						tranText+="/AUTOPROCESS ";
					}
					break;
				case 1:
					tranText+="/TRANSACTIONTYPE:RETURN /LOCKTRANTYPE /LOCKAMOUNT ";
					if(hasXToken) {
						tranText+="/XCACCOUNTID:"+CCard.XChargeToken+" ";
						tranText+="/AUTOPROCESS ";
						tranText+="/GETXCACCOUNTIDSTATUS ";
					}
					if(notRecurring) {
						tranText+="/ACCOUNT:"+CCard.CCNumberMasked+" ";
						tranText+="/AUTOPROCESS ";
					}
					break;
				case 2:
					tranText+="/TRANSACTIONTYPE:DEBITPURCHASE /LOCKTRANTYPE /LOCKAMOUNT ";
					tranText+="/CASHBACK:"+cashBack+" ";
					break;
				case 3:
					tranText+="/TRANSACTIONTYPE:DEBITRETURN /LOCKTRANTYPE /LOCKAMOUNT ";
					break;
				case 4:
					tranText+="/TRANSACTIONTYPE:FORCE /LOCKTRANTYPE /LOCKAMOUNT ";
					break;
				case 5:
					tranText+="/TRANSACTIONTYPE:PREAUTH /LOCKTRANTYPE /LOCKAMOUNT ";
					if(hasXToken) {
						tranText+="/XCACCOUNTID:"+CCard.XChargeToken+" ";
						tranText+="/AUTOPROCESS ";
						tranText+="/GETXCACCOUNTIDSTATUS ";
					}
					if(notRecurring) {
						tranText+="/ACCOUNT:"+CCard.CCNumberMasked+" ";
						tranText+="/AUTOPROCESS ";
					}
					break;
				case 6:
					tranText+="/TRANSACTIONTYPE:ADJUSTMENT /LOCKTRANTYPE ";//excluding /LOCKAMOUNT, amount must be editable in X-Charge to make an adjustment
					string adjustTransactionID="";
					string[] noteSplit=Regex.Split(textNote.Text,"\r\n");
					foreach(string XCTrans in noteSplit) {
						if(XCTrans.StartsWith("XCTRANSACTIONID=")) {
							adjustTransactionID=XCTrans.Substring(16);
						}
					}
					if(adjustTransactionID!="") {
						tranText+="/XCTRANSACTIONID:"+adjustTransactionID+" ";
						tranText+="/AUTOPROCESS ";
					}
					break;
				case 7:
					tranText+="/TRANSACTIONTYPE:VOID /LOCKTRANTYPE /LOCKAMOUNT ";
					break;
			}
			if(_promptSignature) {
				tranText+="/PROMPTSIGNATURE:T /SAVESIGNATURE:T ";
			}
			else {
				tranText+="/PROMPTSIGNATURE:F ";
			}
			tranText+="/RECEIPTINRESULT ";//So that we can make a few changes to the receipt ourselves
			return tranText;
		}

		///<summary>Returns true if payconnect is enabled and completely setup.</summary>
		private bool HasPayConnect() {
			_listPaymentTypeDefs=_listPaymentTypeDefs??Definitions.GetDefsForCategory(DefinitionCategory.PaymentTypes,true);
			Program prog=Programs.GetCur(ProgramName.PayConnect);
			bool isSetupRequired=false;
			if(prog.Enabled) {
				//If clinics are disabled, _paymentCur.ClinicNum will be 0 and the Username and Password will be the 'Headquarters' or practice credentials
				string paymentType=ProgramProperties.GetPropVal(prog.Id,"PaymentType",_paymentCur.ClinicNum);
				if(string.IsNullOrEmpty(ProgramProperties.GetPropVal(prog.Id,"Username",_paymentCur.ClinicNum))
					|| string.IsNullOrEmpty(ProgramProperties.GetPropVal(prog.Id,"Password",_paymentCur.ClinicNum))
					|| !_listPaymentTypeDefs.Any(x => x.Id.ToString()==paymentType)) 
				{
					isSetupRequired=true;
				}
			}
			else {//Program link not enabled.  Launch a promo site.
				ODException.SwallowAnyException(() =>
					Process.Start("http://www.opendental.com/resources/redirects/redirectpayconnect.html")
				);
				return false;
			}
			if(isSetupRequired) {
				if(!Security.IsAuthorized(Permissions.Setup)) {
					return false;
				}
				FormPayConnectSetup FormPCS=new FormPayConnectSetup();
				FormPCS.ShowDialog();
				if(FormPCS.DialogResult!=DialogResult.OK) {
					return false;
				}
				//The user could have corrected the PayConnect bridge, recursively try again.
				return HasPayConnect();
			}
			return true;
		}

		///<summary>Returns true if PaySimple is enabled and completely setup.</summary>
		private bool HasPaySimple() {
			_listPaymentTypeDefs=_listPaymentTypeDefs??Definitions.GetDefsForCategory(DefinitionCategory.PaymentTypes,true);
			Program prog=Programs.GetCur(ProgramName.PaySimple);
			bool isSetupRequired=false;
			if(prog.Enabled) {
				//If clinics are disabled, _paymentCur.ClinicNum will be 0 and the Username and Key will be the 'Headquarters' or practice credentials
				string paymentType=ProgramProperties.GetPropValForClinicOrDefault(prog.Id,PaySimple.PropertyDescs.PaySimplePayTypeCC,_paymentCur.ClinicNum);
				if(string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(prog.Id,PaySimple.PropertyDescs.PaySimpleApiUserName,_paymentCur.ClinicNum))
					|| string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(prog.Id,PaySimple.PropertyDescs.PaySimpleApiKey,_paymentCur.ClinicNum))
					|| !_listPaymentTypeDefs.Any(x => x.Id.ToString()==paymentType)) 
				{
					isSetupRequired=true;
				}
			}
			else {//Program link not enabled.  Launch a promo website.
				ODException.SwallowAnyException(() =>
					Process.Start("http://www.opendental.com/resources/redirects/redirectpaysimple.html")
				);
				return false;
			}
			if(isSetupRequired) {
				if(!Security.IsAuthorized(Permissions.Setup)) {
					return false;
				}
				FormPaySimpleSetup form=new FormPaySimpleSetup();
				form.ShowDialog();
				if(form.DialogResult!=DialogResult.OK) {
					return false;
				}
				//The user could have corrected the PaySimple bridge, recursively try again.
				return HasPaySimple();
			}
			return true;
		}

		private bool HasXCharge() {
			_listPaymentTypeDefs=_listPaymentTypeDefs??Definitions.GetDefsForCategory(DefinitionCategory.PaymentTypes,true);
			if(_xProg==null) {
				MessageBox.Show("X-Charge entry is missing from the database.");//should never happen
				return false;
			}
			bool isSetupRequired=false;
			//if X-Charge is enabled, but the Username or Password are blank or the PaymentType is not a valid DefNum, setup is required
			if(_xProg.Enabled) {
				//X-Charge is enabled if the username and password are set and the PaymentType is a valid DefNum
				//If clinics are disabled, _paymentCur.ClinicNum will be 0 and the Username and Password will be the 'Headquarters' or practice credentials
				string paymentType=ProgramProperties.GetPropVal(_xProg.Id,"PaymentType",_paymentCur.ClinicNum);
				if(string.IsNullOrEmpty(ProgramProperties.GetPropVal(_xProg.Id,"Username",_paymentCur.ClinicNum))
					|| string.IsNullOrEmpty(ProgramProperties.GetPropVal(_xProg.Id,"Password",_paymentCur.ClinicNum))
					|| !_listPaymentTypeDefs.Any(x => x.Id.ToString()==paymentType))
				{
					isSetupRequired=true;
				}
			}
			else {//Program link not enabled.  Launch a promo site.
				ODException.SwallowAnyException(() =>
					Process.Start("http://www.opendental.com/resources/redirects/redirectopenedge.html")
				);
				return false;
			}
			//if X-Charge is enabled and the Username and Password is set and the PaymentType is a valid DefNum,
			//make sure the path (either local override or program path) is valid
			if(!isSetupRequired && !File.Exists(_xPath)) {
				MessageBox.Show("Path is not valid.");
				isSetupRequired=true;
			}
			//if setup is required and the user is authorized for setup, load the X-Charge setup form, but return false so the validation can happen again
			if(isSetupRequired && Security.IsAuthorized(Permissions.Setup)) {
				FormXchargeSetup FormX=new FormXchargeSetup();
				FormX.ShowDialog();
				CheckUIState();//user may have made a change in setup that affects the state of the UI, e.g. X-Charge is no longer enabled for this clinic
				return false;
			}
			return true;
		}

		///<summary>Highlights the charges that corresponds to the selected paysplit.</summary>
		private void HighlightChargesForSplits() {
			gridCharges.SetSelected(false);
			gridTreatPlan.SetSelected(false);
			List<PaySplit> listPaySplits=gridSplits.SelectedTags<PaySplit>();
			foreach(PaySplit paySplit in listPaySplits) {
				PaySplitHelper paySplitHelper=new PaySplitHelper(paySplit);
				if(gridCharges.AllowSelection && _dictGridChargesPaySplitIndices.TryGetValue(paySplitHelper,out List<int> listChargeIndices)) {
					foreach(int index in listChargeIndices) {
						gridCharges.SetSelected(index,true);
					}
				}
				if(_dictGridTreatPlanPaySplitIndices.TryGetValue(paySplitHelper,out List<int> listTreatPlanIndices)) {
					foreach(int index in listTreatPlanIndices) {
						gridTreatPlan.SetSelected(index,true);
					}
				}
			}
		}

		///<summary>Performs all of the Load functionality.</summary>
		private void Init(bool doAutoSplit=false,bool doSelectAllSplits=false,bool doPayFirstAcctEntries=false) {
			_isInit=true;
			AmtTotal=(decimal)_paymentCur.PayAmt;
			List<AccountEntry> listPayFirstAcctEntries=new List<AccountEntry>();
			if(doPayFirstAcctEntries && ListEntriesPayFirst!=null) {
				listPayFirstAcctEntries=ListEntriesPayFirst;
			}
			bool doShowExplicitCreditsOnly=checkIncludeExplicitCreditsOnly.Checked && checkIncludeExplicitCreditsOnly.Enabled;
			PaymentEdit.InitData initData=PaymentEdit.Init(_loadData,listPayFirstAcctEntries,_dictPatients,checkPayTypeNone.Checked,_preferCurrentPat,
				doAutoSplit,doShowExplicitCreditsOnly);
			_paymentCur.PayAmt=(double)AmtTotal;//Reset it.
			textSplitTotal.Text=initData.SplitTotal.ToString("f");
			_dictPatients=initData.DictPats;
			if(checkPayTypeNone.Checked) {
				if(PrefC.HasClinicsEnabled) {
					comboGroupBy.SelectedIndex=2;
				}
				else { 
					comboGroupBy.SelectedIndex=1;
				}
			}
			//Get data from constructing charges list, linking credits, and auto splitting.
			_listSplitsCur=initData.AutoSplitData.ListSplitsCur;
			_listAccountCharges=initData.AutoSplitData.ListAccountCharges;
			_paymentCur=initData.AutoSplitData.Payment;
			FillFilters();
			FillGridSplits();
			FillGridCharges();
			FillGridTreatPlan();
			FillGridAllocated();
			if(doSelectAllSplits) {
				gridSplits.SetSelected(true);
				HighlightChargesForSplits();
			}
			_isInit=false;
		}

		private void PayConnectReturn() {
			string refNum=_payConnectResponseWeb.RefNumber;
			if(refNum.IsNullOrEmpty()) {
				MessageBox.Show("Missing PayConnect Reference Number. This return cannot be processed.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			if(!PayConnectL.VoidOrRefundPayConnectPortalTransaction(_payConnectResponseWeb,_paymentCur,PayConnectService.transType.RETURN,refNum,(decimal)_paymentCur.PayAmt)) {
				Cursor=Cursors.Default;
				return;
			}
			Cursor=Cursors.Default;
			MessageBox.Show("Return successful.");
		}

		private void PayConnectVoid() {
			string refNum=_payConnectResponseWeb.RefNumber;
			if(refNum.IsNullOrEmpty()) {
				MessageBox.Show("Missing PayConnect Reference Number. This void cannot be processed.");
				butVoid.Enabled=false;
				return;
			}
			Cursor=Cursors.WaitCursor;
			if(!PayConnectL.VoidOrRefundPayConnectPortalTransaction(_payConnectResponseWeb,_paymentCur,PayConnectService.transType.VOID,refNum,(decimal)_paymentCur.PayAmt)) {
				Cursor=Cursors.Default;
				return;
			}
			Cursor=Cursors.Default;
			MessageBox.Show("Void successful.");
		}

		private void PrintReceipt(string receiptStr,string strAuditDescription) {//TODO: Implement ODprintout pattern - MigraDoc
			MigraDocPrintDocument printdoc=new MigraDocPrintDocument(new DocumentRenderer(CreatePDFDoc(receiptStr)));
			printdoc.Renderer.PrepareDocument();
#if DEBUG
			FormRpPrintPreview pView=new FormRpPrintPreview(printdoc);
			pView.ShowDialog();
#else
			if(PrinterL.SetPrinter(_pd2,PrintSituation.Receipt,_patCur.PatNum,strAuditDescription)) {
				printdoc.PrinterSettings=_pd2.PrinterSettings;
				try {
					printdoc.Print();
				}
				catch(Exception ex) {
					MessageBox.Show("Unable to print receipt"+". "+ex.Message);
				}
			}
#endif
		}

		private void Reinitialize(bool doRefreshConstructData=false,bool doSelectAllSplits=false) {
			List<long> listPatNumsFamily=_famCur.GetPatNums();
			if(_patCur.SuperFamily > 0 && checkShowSuperfamily.Checked) {
				listPatNumsFamily=listPatNumsFamily.Union(_loadData.SuperFam.GetPatNums()).ToList();
			}
			//Preserve all PaySplits that are not part of this current payment.
			_loadData.ListSplits.RemoveAll(x => x.PayNum==_paymentCur.PayNum);
			//Add back all PaySplits showing to the user.  Keep in mind that they may have deleted some splits or even added new ones (SplitNum=0).
			_loadData.ListSplits.AddRange(_listSplitsCur);
			if(doRefreshConstructData) {
				_loadData.ConstructChargesData=PaymentEdit.GetConstructChargesData(listPatNumsFamily,_patCur.PatNum,_listSplitsCur,_paymentCur.PayNum,
					checkPayTypeNone.Checked);
			}
			Init(doSelectAllSplits:doSelectAllSplits);
		}

		private bool SavePaymentToDb() {
			if(textDate.errorProvider1.GetError(textDate)!="" || textAmount.errorProvider1.GetError(textAmount)!="") {
				MessageBox.Show("Please fix data entry errors first.");
				return false;
			}
			if(PIn.Date(textDate.Text).Date > DateTime.Today.Date
					&& !Preferences.GetBool(PreferenceName.FutureTransDatesAllowed) && !Preferences.GetBool(PreferenceName.AccountAllowFutureDebits))
			{
				MessageBox.Show("Payment date cannot be in the future.");
				return false;
			}
			if(checkPayTypeNone.Checked) {
				if(PIn.Double(textAmount.Text)!=0) {
					MessageBox.Show("Amount must be zero for a transfer.");
					return false;
				}
			}
			else {
				double amt=PIn.Double(textAmount.Text);
				if(amt==0 && _listSplitsCur.Count==0) {
					MessageBox.Show("Please enter an amount or create payment splits.");
					return false;
				}
				if(amt!=0 && listPayType.SelectedIndex==-1) {
					MessageBox.Show("A payment type must be selected.");
					return false;
				}
			}
			if(PrefC.GetInt(PreferenceName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
				if(_listSplitsCur.Any(x => x.ProcNum==0 && x.UnearnedType==0 && x.AdjNum==0)) {//if no procs and no adjust and not an unearned type
					MessageBox.Show("A procedure, adjustment, or unearned type must be selected for each of the payment splits.");
					return false;
				}
			}
			double unearnedCur=_listSplitsCur.FindAll(x => x.UnearnedType>0).Sum(x => x.SplitAmt);
			if(unearnedCur.IsLessThan(0)
				&& !(unearnedCur+(double)PaySplits.GetUnearnedForFam(_famCur,payNumExcluded:_paymentCur.PayNum)).IsGreaterThanOrEqualToZero()
				&& !MsgBox.Show(MsgBoxButtons.YesNo,"You are attempting to create a negative unearned amount. Continue?"))
			{
				return false;
			}
			object[] parameters={ listPayType.SelectedItem,textNote.Text,_isCCDeclined,_paymentCur };
			Plugins.HookAddCode(this,"FormPayment.SavePaymentToDb_afterUnearnedCurCheck",parameters);
			textNote.Text=(string)parameters[1];
			_isCCDeclined=(bool)parameters[2];
			if(_isCCDeclined) {
				textAmount.Text=0.ToString("f");//So that a declined transaction does not affect account balance
				_listSplitsCur.ForEach(x => x.SplitAmt=0);
				textSplitTotal.Text=0.ToString("f");
			}
			if(IsNew) {
				//prevents backdating of initial payment
				if(!Security.IsAuthorized(Permissions.PaymentCreate,PIn.Date(textDate.Text))) {
					return false;
				}
			}
			else {
				//Editing an old entry will already be blocked if the date was too old, and user will not be able to click OK button
				//This catches it if user changed the date to be older. If user has SplitCreatePastLockDate permission and has not changed the date, then
				//it is okay to save the payment.
				if((!Security.IsAuthorized(Permissions.SplitCreatePastLockDate,true)
					|| _paymentOld.PayDate!=PIn.Date(textDate.Text))
					&& !Security.IsAuthorized(Permissions.PaymentEdit,PIn.Date(textDate.Text))) {
					return false;
				}
			}
			bool accountingSynchRequired=false;
			double accountingOldAmt=_paymentCur.PayAmt;
			long accountingNewAcct=-1;//the old acctNum will be retrieved inside the validation code.
			if(textDepositAccount.Visible) {//Not visable when IsNew or _loadData.Transaction is null or if listPayType is clicked.
				accountingNewAcct=-1;//indicates no change
			}
			else if(comboDepositAccount.Visible && comboDepositAccount.Items.Count>0 && comboDepositAccount.SelectedIndex!=-1) {
				//comboDepositAccount is set invisible when IsNew is false 
				//or if listPayType.SelectedIndex==-1 || checkPayTypeNone.Checked and IsNew and PrefName.PaymentClinicSetting is PayClinicSetting.PatientDefaultClinic
				//or if AccountingAutoPay can not be found based on listPayType.SelectedIndex.
				accountingNewAcct=_arrayDepositAcctNums[comboDepositAccount.SelectedIndex];
			}
			else {//neither textbox nor combo visible. Or something's wrong with combobox
				accountingNewAcct=0;
			}
			try {
				accountingSynchRequired=Payments.ValidateLinkedEntries(accountingOldAmt,PIn.Double(textAmount.Text),IsNew,
					_paymentCur.PayNum,accountingNewAcct);
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);//not able to alter, so must not allow user to continue.
				return false;
			}
			if(_paymentCur.ProcessStatus!=ProcessStat.OfficeProcessed) {
				if(checkProcessed.Checked) {
					_paymentCur.ProcessStatus=ProcessStat.OnlineProcessed;
				}
				else {
					_paymentCur.ProcessStatus=ProcessStat.OnlinePending;
				}
			}
			_paymentCur.PayAmt=PIn.Double(textAmount.Text);//handles blank
			_paymentCur.PayDate=PIn.Date(textDate.Text);
			#region Recurring charge logic
			//User chose to have a recurring payment so we need to know if the card has recurring setup and which month to apply the payment to.
			if(IsNew && checkRecurring.Checked && comboCreditCards.SelectedIndex!=_listCreditCards.Count) {
				//Check if a recurring charge is setup for the selected card.
				if(_listCreditCards[comboCreditCards.SelectedIndex].ChargeAmt==0 
					|| _listCreditCards[comboCreditCards.SelectedIndex].DateStart.Year < 1880) 
				{
					MessageBox.Show("The selected credit card has not been setup for recurring charges.");
					return false;
				}
				//Check if a stop date was set and if that date falls in on today or in the past.
				if(_listCreditCards[comboCreditCards.SelectedIndex].DateStop.Year > 1880
					&& _listCreditCards[comboCreditCards.SelectedIndex].DateStop<=DateTime.Now) 
				{
					MessageBox.Show("This card is no longer accepting recurring charges based on the stop date.");
					return false;
				}
				//Have the user decide what month to apply the recurring charge towards.
				FormCreditRecurringDateChoose formDateChoose=new FormCreditRecurringDateChoose(_listCreditCards[comboCreditCards.SelectedIndex],_patCur);
				formDateChoose.ShowDialog();
				if(formDateChoose.DialogResult!=DialogResult.OK) {
					MessageBox.Show("Uncheck the \"Apply to Recurring Charge\" box.");
					return false;
				}
				//This will change the PayDate to work better with the recurring charge automation.  User was notified in previous window.
				if(!Preferences.GetBool(PreferenceName.RecurringChargesUseTransDate)) {
					_paymentCur.PayDate=formDateChoose.PayDate;
				}
				_paymentCur.RecurringChargeDate=formDateChoose.PayDate;
			}
			else if(IsNew && checkRecurring.Checked && comboCreditCards.SelectedIndex==_listCreditCards.Count) {
				MessageBox.Show("Cannot apply a recurring charge to a new card.");
				return false;
			}
			#endregion
			_paymentCur.CheckNum=textCheckNum.Text;
			_paymentCur.BankBranch=textBankBranch.Text;
			_paymentCur.PayNote=textNote.Text;
			_paymentCur.IsRecurringCC=checkRecurring.Checked;
			if((PIn.Double(textAmount.Text)==0 && listPayType.SelectedIndex==-1) || checkPayTypeNone.Checked) {
				_paymentCur.PayType=0;
			}
			else {
				_paymentCur.PayType=_listPaymentTypeDefs[listPayType.SelectedIndex].Id;
			}
			if(_listSplitsCur.Count==0) {//Existing payment with no splits.
				if(!_isCCDeclined && PrefC.GetInt(PreferenceName.RigorousAccounting) < (int)RigorousAccounting.DontEnforce) {
					_listSplitsCur.AddRange(AutoSplitForPayment(_paymentCur.PayDate,_loadData));
					_paymentCur.PayAmt=PIn.Double(textAmount.Text);//AutoSplitForPayment reduces PayAmt - Set it back to what it should be.
				}
				else if(!_isCCDeclined
					&& Payments.AllocationRequired(_paymentCur.PayAmt,_paymentCur.PatNum)
					&& _curFamOrSuperFam.ListPats.Length>1 //Has other family members
					&& MsgBox.Show(MsgBoxButtons.YesNo,"Apply part of payment to other family members?"))
				{
					_listSplitsCur=Payments.Allocate(_paymentCur);//PayAmt needs to be set first
				}
				else {//Either no allocation required, or user does not want to allocate.  Just add one split.
					if(checkPayTypeNone.Checked) {//No splits created and it's an income transfer.  Delete payment? (it's not a useful payment)
						Payments.Delete(_paymentCur);
						return true;
					}
					else {
						if(!AddOneSplit(true)) {
							return false;
						}
					}
				}
				if(_listSplitsCur.Count==0) {//There's still no split.
					if(!AddOneSplit(true)) {
						return false;
					}
				}
			}
			else {//A new or existing payment with splits.
				if(_listSplitsCur.Count==1//if one split
					&& _listSplitsCur[0].PayPlanNum!=0//and split is on a payment plan
					&& PIn.Double(textAmount.Text) != _listSplitsCur[0].SplitAmt)//and amount doesn't match payment
				{
					_listSplitsCur[0].SplitAmt=PIn.Double(textAmount.Text);//make amounts match automatically
					textSplitTotal.Text=textAmount.Text;
				}
				if(_paymentCur.PayAmt!=PIn.Double(textSplitTotal.Text)) {
					MessageBox.Show("Split totals must equal payment amount.");
					//work on reallocation schemes here later
					return false;
				}
			}
			if(IsNew && !_isCCDeclined) {
				//Currently we do not want to modify historical data or credit transaction values. Moving forward zero dollar splits are not valid.
				_listSplitsCur.RemoveAll(x => x.SplitAmt.IsZero());
			}
			//At this point there better be a split in the list of Current Payment Splits.
			//There is no such thing as a payment with no payment splits.  If there is then the DBM 'PaymentMissingPaySplit' needs to be removed.
			if(_listSplitsCur.Count==0) {
				MessageBox.Show("Please create payment splits.");
				return false;
			}
			if(_listSplitsCur.Count>1) {
				_paymentCur.IsSplit=true;
			}
			else {
				_paymentCur.IsSplit=false;
			}
			try {
				Payments.Update(_paymentCur,true);
			}
			catch(ApplicationException ex) {//this catches bad dates.
				MessageBox.Show(ex.Message);
				return false;
			}
			//Set all DatePays the same.
			foreach(PaySplit paySplit in _listSplitsCur) {
				paySplit.DatePay=_paymentCur.PayDate;
			}
			bool hasChanged=PaySplits.Sync(_listSplitsCur,_listPaySplitsOld);
			foreach(PaySplit paySplitOld in _listPaySplitsForSecLog) {
				//Split was deleted. Add Securitylog Entry
				SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,paySplitOld.PatNum,PaySplits.GetSecurityLogMsgDelete(paySplitOld,_paymentCur),0,
					paySplitOld.SecDateTEdit);
			}
			foreach(PaySplits.PaySplitAssociated split in _listPaySplitsAssociated) {
				//Update the FSplitNum after inserts are made. 
				if(split.PaySplitLinked!=null && split.PaySplitOrig!=null) {
					PaySplits.UpdateFSplitNum(split.PaySplitOrig.SplitNum,split.PaySplitLinked.SplitNum);
				}
			}
			//Accounting synch is done here.  All validation was done further up
			//If user is trying to change the amount or linked account of an entry that was already copied and linked to accounting section
			if(accountingSynchRequired && !checkPayTypeNone.Checked) {
				Payments.AlterLinkedEntries(accountingOldAmt,_paymentCur.PayAmt,IsNew,_paymentCur.PayNum,accountingNewAcct,_paymentCur.PayDate,
					_curFamOrSuperFam.GetNameInFamFL(_paymentCur.PatNum));
			}
			if(IsNew) {
				SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,_paymentCur.PatNum,Payments.GetSecuritylogEntryText(_paymentCur,_paymentOld,IsNew,_listPaymentTypeDefs));
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,_paymentCur.PatNum,Payments.GetSecuritylogEntryText(_paymentCur,_paymentOld,IsNew,_listPaymentTypeDefs),
					0,_paymentOld.SecDateTEdit);
			}
			if(hasChanged) {
				string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,_listSplitsCur.Union(_listPaySplitsOld).ToList());
				if(!string.IsNullOrEmpty(strErrorMsg)) {
					MessageBox.Show(strErrorMsg);
				}
			}
			return true;
		}

		private string SecurityLogEntryHelper(string oldVal,string newVal,string textInLog) {
			if(oldVal!=newVal) {
				return "\r\n"+textInLog+" changed back to '"+oldVal+"' from '"+newVal+"'";
			}
			return "";
		}

		private void SelectPaySplit(PaySplit paySplit) {
			if(paySplit==null) {
				return;
			}
			PaySplitHelper paySplitHelper=new PaySplitHelper(paySplit);
			if(_dictGridSplitsPaySplitIndices.TryGetValue(paySplitHelper,out List<int> listSplitIndices)) {
				foreach(int index in listSplitIndices) {
					gridSplits.SetSelected(index,true);
				}
			}
		}

		private void SelectPaySplitsForAccountEntries(List<AccountEntry> listAccountEntries) {
			List<PaySplit> listPaySplits=new List<PaySplit>();
			//Select all splits associated to any PayPlanCharge because interest and principal are always treated as one entity.
			List<long> listPayPlanChargeNums=listAccountEntries.Select(x => x.PayPlanChargeNum).ToList();
			if(listPayPlanChargeNums.Count > 0) {
				List<AccountEntry> listPayPlanChargeEntries=_listAccountCharges.FindAll(x => x.PayPlanChargeNum.In(listPayPlanChargeNums));
				listAccountEntries.AddRange(listPayPlanChargeEntries.Except(listAccountEntries));
			}
			foreach(AccountEntry accountEntry in listAccountEntries) {
				foreach(PaySplit paySplit in accountEntry.SplitCollection) {
					listPaySplits.Add(paySplit);
				}
			}
			foreach(PaySplit paySplit in listPaySplits) {
				SelectPaySplit(paySplit);
			}
		}

		///<summary>Called from all 4 places where listPayType gets changed.</summary>
		private void SetComboDepositAccounts() {
			if(listPayType.SelectedIndex==-1 || checkPayTypeNone.Checked) {
				if(IsNew && (PayClinicSetting)PrefC.GetInt(PreferenceName.PaymentClinicSetting)==PayClinicSetting.PatientDefaultClinic) {
					labelDepositAccount.Visible=false;
					comboDepositAccount.Visible=false;
				}
				return;
			}
			AccountingAutoPay autoPay=AccountingAutoPays.GetForPayType(
				_listPaymentTypeDefs[listPayType.SelectedIndex].Id);
			if(autoPay==null) {
				labelDepositAccount.Visible=false;
				comboDepositAccount.Visible=false;
			}
			else {
				labelDepositAccount.Visible=true;
				comboDepositAccount.Visible=true;
				_arrayDepositAcctNums=AccountingAutoPays.GetPickListAccounts(autoPay).ToArray();
				comboDepositAccount.Items.Clear();
				comboDepositAccount.Items.AddRange(_arrayDepositAcctNums.Select(x => Accounts.GetDescription(x)).ToArray());
				if(comboDepositAccount.Items.Count>0) {
					comboDepositAccount.SelectedIndex=0;
				}
			}
		}

		private void ToggleShowHideSplits() {
			splitContainerCharges.Panel2Collapsed = !splitContainerCharges.Panel2Collapsed;
			if(splitContainerCharges.Panel2Collapsed) {
				butShowHide.Text="Show Splits";
				Height = splitContainerCharges.SplitterDistance+100;//Plus 100 to give room for the buttons
				this.butShowHide.Image = global::Imedisoft.Properties.Resources.arrowDownTriangle;
			}
			else {
				butShowHide.Text="Hide Splits";
				Height = _originalHeight;
				this.butShowHide.Image = global::Imedisoft.Properties.Resources.arrowUpTriangle;
			}
		}

		///<summary>Goes through _listPaySplitsAssociated and replaces the paySplitToBeDeleted with paySplitToBeAdded.</summary>
		private void UpdateListAssociationsBeforeDelete(PaySplit paySplitToBeDeleted,PaySplit paySplitToBeAdded) {
			foreach(PaySplits.PaySplitAssociated payAssociatedLink in _listPaySplitsAssociated.Where(x => x.PaySplitLinked==paySplitToBeDeleted)) {
				payAssociatedLink.PaySplitLinked=paySplitToBeAdded;
			}
			foreach(PaySplits.PaySplitAssociated payAssociatedOrig in _listPaySplitsAssociated.FindAll(x => x.PaySplitOrig==paySplitToBeDeleted)) {
				payAssociatedOrig.PaySplitOrig=paySplitToBeAdded;
			}
		}

		private void VoidPayConnectTransaction(string refNum,string amount) {
			PayConnectResponse response=null;
			string receiptStr="";
			Cursor=Cursors.WaitCursor;
			if(_payConnectRequest==null) {//The payment was made through the terminal.
				ODProgress.ShowAction(() => {
						PosRequest posRequest=PosRequest.CreateVoidByReference(refNum);
						PosResponse posResponse=DpsPos.ProcessCreditCard(posRequest);
						response=PayConnectTerminal.ToPayConnectResponse(posResponse);
						receiptStr=PayConnectTerminal.BuildReceiptString(posRequest,posResponse,null,0);
					},
					startingMessage:"Processing void on terminal.",
					actionException:ex => {
						this.Invoke(() => {
							Cursor=Cursors.Default;
							MessageBox.Show("Error voiding payment:"+" "+ex.Message);
						});
					});
			}
			else {//The payment was made through the web service.
				_payConnectRequest.TransType=PayConnectService.transType.VOID;
				_payConnectRequest.RefNumber=refNum;
				_payConnectRequest.Amount=PIn.Decimal(amount);
				PayConnectService.transResponse transResponse=PayConnect.ProcessCreditCard(_payConnectRequest,_paymentCur.ClinicNum,x => MessageBox.Show(x));
				response=new PayConnectResponse(transResponse,_payConnectRequest);
				receiptStr=PayConnect.BuildReceiptString(_payConnectRequest,transResponse,null,0);
			}
			Cursor=Cursors.Default;
			if(response==null || response.StatusCode!="0") {//error in transaction
				MessageBox.Show("This credit card payment has already been processed and will have to be voided manually through the web interface.");
				return;
			}
			else {//Record a new payment for the voided transaction
				Payment voidPayment=_paymentCur.Clone();
				voidPayment.PayAmt*=-1; //The negated amount of the original payment
				voidPayment.Receipt=receiptStr;
				voidPayment.PayNote="Transaction Type"+": "+Enum.GetName(typeof(PayConnectService.transType),PayConnectService.transType.VOID)
					+Environment.NewLine+"Status"+": "+response.Description+Environment.NewLine
					+"Amount"+": "+voidPayment.PayAmt+Environment.NewLine
					+"Auth Code"+": "+response.AuthCode+Environment.NewLine
					+"Ref Number"+": "+response.RefNumber;
				voidPayment.PaymentSource=CreditCardSource.PayConnect;
				voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
				voidPayment.PayNum=Payments.Insert(voidPayment);
				foreach(PaySplit splitCur in _listSplitsCur) {//Modify the paysplits for the original transaction to work for the void transaction
					PaySplit split=splitCur.Copy();
					split.SplitAmt*=-1;
					split.PayNum=voidPayment.PayNum;
					PaySplits.Insert(split);
				}
				SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,voidPayment.PatNum,
					Patients.GetLim(voidPayment.PatNum).GetNameLF()+", "+voidPayment.PayAmt.ToString("c"));
			}
		}

		private void VoidPaySimpleTransaction(string refNum,string originalReceipt) {
			PaySimple.ApiResponse response=null;
			string receiptStr="";
			Cursor=Cursors.WaitCursor;
			try {
				response=PaySimple.VoidPayment(refNum,_paymentCur.ClinicNum);
			}
			catch(PaySimpleException ex) {
				MessageBox.Show(ex.Message);
				if(ex.ErrorType==PaySimpleError.CustomerDoesNotExist && MsgBox.Show(MsgBoxButtons.OKCancel,
					"Delete the link to the customer id for this patient?")) 
				{
					PatientLinks.DeletePatNumTos(ex.CustomerId,PatientLinkType.PaySimple);
				}
				return;
			}
			catch(ODException wex) {
				MessageBox.Show(wex.Message);//This should have already been Lans.g if applicable.
				return;
			}
			catch(Exception ex) {
				MessageBox.Show("Error:"+" "+ex.Message);
				return;
			}
			string[] arrayReceiptFields=originalReceipt.Replace("\r\n","\n").Replace("\r","\n").Split(new string[] { "\n" },StringSplitOptions.RemoveEmptyEntries);
			string ccNum="";
			string expDateStr="";
			string nameOnCard="";
			foreach(string receiptField in arrayReceiptFields) {
				if(receiptField.StartsWith("Name")) {
					nameOnCard=receiptField.Substring(4).Replace(".","");
				}
				if(receiptField.StartsWith("Account")) {
					ccNum=receiptField.Substring(7).Replace(".","");
				}
				if(receiptField.StartsWith("Exp Date")) {
					expDateStr=receiptField.Substring(8).Replace(".","");
				}
			}
			//ACH payments do not have expDates. ACH payments can be voided as long as the void occurs before the batch is closed.
			int expMonth=-1;
			int expYear=-1;
			if(!string.IsNullOrEmpty(expDateStr) && expDateStr.Length > 2) {
				expMonth=PIn.Int(expDateStr.Substring(0,2));
				expYear=PIn.Int(expDateStr.Substring(2));
			}
			response.BuildReceiptString(ccNum,expMonth,expYear,nameOnCard,_paymentCur.ClinicNum);
			receiptStr=response.TransactionReceipt;
			Cursor=Cursors.Default;
			Payment voidPayment=_paymentCur.Clone();
			voidPayment.PayAmt*=-1; //The negated amount of the original payment
			voidPayment.Receipt=receiptStr;
			voidPayment.PayNote=response.ToNoteString();
			voidPayment.PaymentSource=CreditCardSource.PaySimple;
			voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
			voidPayment.PayNum=Payments.Insert(voidPayment);
			foreach(PaySplit splitCur in _listSplitsCur) {//Modify the paysplits for the original transaction to work for the void transaction
				PaySplit split=splitCur.Copy();
				split.SplitAmt*=-1;
				split.PayNum=voidPayment.PayNum;
				PaySplits.Insert(split);
			}
			SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,voidPayment.PatNum,
				Patients.GetLim(voidPayment.PatNum).GetNameLF()+", "+voidPayment.PayAmt.ToString("c"));
		}

		///<summary>Only used to void a transaction that has just been completed when the user hits Cancel. Uses the same Print Receipt settings as the 
		///original transaction.</summary>
		private void VoidXChargeTransaction(string transID,string amount,bool isDebit) {
			ProcessStartInfo info=new ProcessStartInfo(_xProg.Path);
			string resultfile=Storage.GetTempFileName(".txt");
			File.Delete(resultfile);//delete the old result file.
			info.Arguments="";
			if(isDebit) {
				info.Arguments+="/TRANSACTIONTYPE:DEBITRETURN /LOCKTRANTYPE ";
			}
			else {
				info.Arguments+="/TRANSACTIONTYPE:VOID /LOCKTRANTYPE ";
			}
			info.Arguments+="/XCTRANSACTIONID:"+transID+" /LOCKXCTRANSACTIONID ";
			info.Arguments+="/AMOUNT:"+amount+" /LOCKAMOUNT ";
			info.Arguments+="/RECEIPT:Pat"+_paymentCur.PatNum.ToString()+" ";//aka invoice#
			info.Arguments+="\"/CLERK:"+Security.CurrentUser.UserName+"\" /LOCKCLERK ";
			info.Arguments+="/RESULTFILE:\""+resultfile+"\" ";
			info.Arguments+="/USERID:"+ProgramProperties.GetPropVal(_xProg.Id,"Username",_paymentCur.ClinicNum)+" ";
			info.Arguments+="/PASSWORD:"+CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(_xProg.Id,"Password",_paymentCur.ClinicNum))+" ";
			info.Arguments+="/AUTOCLOSE ";
			info.Arguments+="/HIDEMAINWINDOW /SMALLWINDOW ";
			if(!isDebit) {
				info.Arguments+="/AUTOPROCESS ";
			}
			info.Arguments+="/PROMPTSIGNATURE:F ";
			info.Arguments+="/RECEIPTINRESULT ";
			Cursor=Cursors.WaitCursor;
			Process process=new Process();
			process.StartInfo=info;
			process.EnableRaisingEvents=true;
			process.Start();
			process.WaitForExit();
			Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
			Cursor=Cursors.Default;
			//Next, record the voided payment within Open Dental.  We use to delete the payment but Nathan wants us to negate voids with another payment.
			string resulttext="";
			string line="";
			bool showApprovedAmtNotice=false;
			double approvedAmt=0;
			string receipt="";
			Payment voidPayment=_paymentCur.Clone();
			voidPayment.PayAmt*=-1;//the negation of the original amount
			try {
				using(TextReader reader=new StreamReader(resultfile)) {
					line=reader.ReadLine();
					/*Example of successful void transaction:
						RESULT=SUCCESS
						TYPE=Void
						APPROVALCODE=000000
						SWIPED=F
						CLERK=Admin
						XCACCOUNTID=XAWpQPwLm7MXZ
						XCTRANSACTIONID=15042616
						ACCOUNT=XXXXXXXXXXXX6781
						EXPIRATION=1215
						ACCOUNTTYPE=VISA
						APPROVEDAMOUNT=11.00
					*/
					while(line!=null) {
						if(!line.StartsWith("RECEIPT=")) {//Don't include the receipt string in the PayNote
							if(resulttext!="") {
								resulttext+="\r\n";
							}
							resulttext+=line;
						}
						if(line.StartsWith("RESULT=")) {
							if(line!="RESULT=SUCCESS") {
								//Void was a failure and there might be a description as to why it failed. Continue to loop through line.
								while(line!=null) {
									line=reader.ReadLine();
									resulttext+="\r\n"+line;
								}
								break;
							}
						}
						if(line.StartsWith("APPROVEDAMOUNT=")) {
							approvedAmt=PIn.Double(line.Substring(15));
							if(approvedAmt != _paymentCur.PayAmt) {
								showApprovedAmtNotice=true;
							}
						}
						if(line.StartsWith("RECEIPT=") && line.Length>8) {
							receipt=PIn.String(line.Substring(8));
							receipt=receipt.Replace("\\n","\r\n");//The receipt from X-Charge escapes the newline characters
						}
						line=reader.ReadLine();
					}
				}
			}
			catch {
				MessageBox.Show("There was a problem voiding this transaction."+"\r\n"+"Please run the credit card report from inside "
					+"X-Charge to verify that the transaction was voided."+"\r\n"+"If the transaction was not voided, please create a new payment "
					+"to void the transaction.");
				return;
			}
			if(showApprovedAmtNotice) {
				MessageBox.Show("The amount of the original transaction"+": "+_paymentCur.PayAmt.ToString("C")+"\r\n"+"does not match "
					+"the approved amount returned"+": "+approvedAmt.ToString("C")+".\r\n"+"The amount will be changed to reflect the approved "
					+"amount charged.","Alert",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				voidPayment.PayAmt=approvedAmt;
			}
			if(textNote.Text!="") {
				textNote.Text+="\r\n";
			}
			voidPayment.PayNote=resulttext;
			voidPayment.Receipt=receipt;
			if(_printReceipt && receipt!="") {
				PrintReceipt(receipt,"X-Charge receipt printed");
			}
			voidPayment.PaymentSource=CreditCardSource.XServer;
			voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
			voidPayment.PayNum=Payments.Insert(voidPayment);
			foreach(PaySplit splitCur in _listSplitsCur) {//Modify the paysplits for the original transaction to work for the void transaction
				PaySplit split=splitCur.Copy();
				split.SplitAmt*=-1;
				split.PayNum=voidPayment.PayNum;
				PaySplits.Insert(split);
			}
			SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,voidPayment.PatNum,Patients.GetLim(voidPayment.PatNum).GetNameLF()+", "
				+voidPayment.PayAmt.ToString("c"));
		}

		private void XWebReturn() {
			CreditCard cc=null;
			List<CreditCard> creditCards=CreditCards.Refresh(_patCur.PatNum);
			if(comboCreditCards.SelectedIndex < creditCards.Count && comboCreditCards.SelectedIndex >-1) {
				cc=creditCards[comboCreditCards.SelectedIndex];
			}
			if(cc==null) {
				MessageBox.Show("Card no longer available. Return cannot be processed.");
				return;
			}
			if(!cc.IsXWeb()) {
				MessageBox.Show("Only cards that were created from XWeb can process an XWeb return.");
				return;
			}
			FormXWeb FormXW=new FormXWeb(_patCur.PatNum,cc,XWebTransactionType.CreditReturnTransaction,createPayment:false);
			FormXW.LockCardInfo=true;
			if(FormXW.ShowDialog()==DialogResult.OK) {
				if(FormXW.ResponseResult!=null) {
					textNote.Text=FormXW.ResponseResult.GetFormattedNote(false);
					textAmount.Text=(-FormXW.ResponseResult.Amount).ToString();//XWeb amounts are always positive even for returns and voids.
					_xWebResponse=FormXW.ResponseResult;
					_xWebResponse.PaymentNum=_paymentCur.PayNum;
					XWebResponses.Update(_xWebResponse);
					butVoid.Visible=true;
					groupXWeb.Height=85;
				}
				MessageBox.Show("Return successful.");
			}
		}

		private void XWebVoid() {
			//Uses DateTime.Today because a void is a new transaction that is created upon entering this function
			if(!Security.IsAuthorized(Permissions.PaymentCreate,DateTime.Today)) {
				return;
			}
			double amount=_xWebResponse.Amount;
			if(_xWebResponse.XTransactionType==XWebTransactionType.CreditReturnTransaction 
				|| _xWebResponse.XTransactionType==XWebTransactionType.DebitReturnTransaction) 
			{
				amount=-amount;//The amount in an xwebresponse is always stored as a positive number.
			}
			if(MessageBox.Show("Void the XWeb transaction of amount"+" "+amount.ToString("f")+" "+"attached to this payment?",
				"",MessageBoxButtons.YesNo)==DialogResult.No)
			{
				return;
			}
			try {
				Cursor=Cursors.WaitCursor;
				string payNote="Void XWeb payment made from within Open Dental";
				XWebs.VoidPayment(_patCur.PatNum,payNote,_xWebResponse.XWebResponseNum);
				Cursor=Cursors.Default;
				MessageBox.Show("Void successful. A new payment has been created for this void transaction.");
			}
			catch(ODException ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(ex.Message);
			}
		}
		#endregion

		#region Methods - Public
		///<summary>Launches the PayConnect transaction window.  Returns null upon failure, otherwise returns the transaction detail as a string.
		///If prepaidAmt is not zero, then will show the PayConnect window with the given prepaid amount and let the user enter card # and exp.
		///A patient is not required for prepaid cards.</summary>
		public string MakePayConnectTransaction(double prepaidAmt=0) {
			if(!HasPayConnect()) {
				return null;
			}
			if(prepaidAmt==0) {//Validation for regular credit cards (not prepaid cards).
				if(textAmount.Text=="") {
					MessageBox.Show("Please enter an amount first.");
					textAmount.Focus();
					return null;
				}
				if(_listSplitsCur.Count>0 && PIn.Double(textAmount.Text)!=PIn.Double(textSplitTotal.Text)
					&& (_listSplitsCur.Count!=1 || _listSplitsCur[0].PayPlanNum==0)) //Not one paysplit attached to payplan
				{
					MessageBox.Show( "Split totals must equal payment amount before running a credit card transaction.");
					return null;
				}
			}
			if(PIn.Date(textDate.Text).Date > DateTime.Today.Date
					&& !Preferences.GetBool(PreferenceName.FutureTransDatesAllowed) && !Preferences.GetBool(PreferenceName.AccountAllowFutureDebits))
			{
				MessageBox.Show("Payment date cannot be in the future.");
				return null;
			}
			CreditCard cc=null;
			List<CreditCard> creditCards=null;
			decimal amount=Math.Abs(PIn.Decimal(textAmount.Text));//PayConnect always wants a positive number even for voids and returns.
			if(prepaidAmt==0) {
				creditCards=CreditCards.Refresh(_patCur.PatNum);
				if(comboCreditCards.SelectedIndex < creditCards.Count) {
					cc=creditCards[comboCreditCards.SelectedIndex];
				}
			}
			else {//Prepaid card
				amount=(decimal)prepaidAmt;
			}
			FormPayConnect FormP=new FormPayConnect(_paymentCur.ClinicNum,_patCur,amount,cc);
			FormP.ShowDialog();
			if(prepaidAmt==0 && FormP.Response!=null) {//Regular credit cards (not prepaid cards).
				//If PayConnect response is not null, refresh comboCreditCards and select the index of the card used for this payment if the token was saved
				creditCards=CreditCards.Refresh(_patCur.PatNum);
				AddCreditCardsToCombo(creditCards,x => x.PayConnectToken==FormP.Response.PaymentToken
					&&x.PayConnectTokenExp.Year==FormP.Response.TokenExpiration.Year
					&&x.PayConnectTokenExp.Month==FormP.Response.TokenExpiration.Month);
				Program prog=Programs.GetCur(ProgramName.PayConnect);
				//still need to add functionality for accountingAutoPay
				string paytype=ProgramProperties.GetPropVal(prog.Id,"PaymentType",_paymentCur.ClinicNum);//paytype could be an empty string
				listPayType.SelectedIndex=Definitions.GetOrder(DefinitionCategory.PaymentTypes,PIn.Long(paytype));
				SetComboDepositAccounts();
			}
			string resultNote=null;
			if(FormP.Response!=null) {
				resultNote="Transaction Type"+": "+Enum.GetName(typeof(PayConnectService.transType),FormP.TranType)+Environment.NewLine+
					"Status"+": "+FormP.Response.Description+Environment.NewLine+
					"Amount"+": "+FormP.AmountCharged+Environment.NewLine+
					"Card Type"+": "+FormP.Response.CardType+Environment.NewLine+
					"Account"+": "+FormP.CardNumber.Right(4).PadLeft(FormP.CardNumber.Length,'X');
			}
			if(prepaidAmt!=0) {
				if(FormP.Response!=null && FormP.Response.StatusCode=="0") { //The transaction succeeded.
					return resultNote;
				}
				return null;
			}
			if(FormP.Response!=null) {
				if(FormP.Response.StatusCode=="0") { //The transaction succeeded.
					_isCCDeclined=false;
					resultNote+=Environment.NewLine
						+"Auth Code"+": "+FormP.Response.AuthCode+Environment.NewLine
						+"Ref Number"+": "+FormP.Response.RefNumber;
					if(FormP.TranType==PayConnectService.transType.RETURN) {
						textAmount.Text="-"+FormP.AmountCharged;
						_paymentCur.Receipt=FormP.ReceiptStr;
					}
					else if(FormP.TranType==PayConnectService.transType.AUTH) {
						textAmount.Text=FormP.AmountCharged;
					}
					else if(FormP.TranType==PayConnectService.transType.SALE) {
						textAmount.Text=FormP.AmountCharged;
						_paymentCur.Receipt=FormP.ReceiptStr;
					}
					if(FormP.TranType==PayConnectService.transType.VOID) {//Close FormPayment window now so the user will not have the option to hit Cancel
						if(IsNew) {
							if(!_wasCreditCardSuccessful) {
								textAmount.Text="-"+FormP.AmountCharged;
								textNote.Text+=((textNote.Text=="")?"":Environment.NewLine)+resultNote;
							}
							_paymentCur.Receipt=FormP.ReceiptStr;
							if(SavePaymentToDb()) {
								MessageBox.Show("Void successful.");
								DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
							}
							return resultNote;
						}
						if(!IsNew || _wasCreditCardSuccessful) {//Create a new negative payment if the void is being run from an existing payment
							if(_listSplitsCur.Count==0) {
								AddOneSplit();
								Reinitialize();
							}
							else if(_listSplitsCur.Count==1//if one split
								&& _listSplitsCur[0].PayPlanNum!=0//and split is on a payment plan
								&& _listSplitsCur[0].SplitAmt!=_paymentCur.PayAmt)//and amount doesn't match payment
							{
								_listSplitsCur[0].SplitAmt=_paymentCur.PayAmt;//make amounts match automatically
								textSplitTotal.Text=textAmount.Text;
							}
							_paymentCur.IsSplit=_listSplitsCur.Count>1;
							Payment voidPayment=_paymentCur.Clone();
							voidPayment.PayAmt*=-1;//the negation of the original amount
							voidPayment.PayNote=resultNote;
							voidPayment.Receipt=FormP.ReceiptStr;
							voidPayment.PaymentSource=CreditCardSource.PayConnect;
							voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
							voidPayment.PayNum=Payments.Insert(voidPayment);
							foreach(PaySplit splitCur in _listSplitsCur) {//Modify the paysplits for the original transaction to work for the void transaction
								PaySplit split=splitCur.Copy();
								split.SplitAmt*=-1;
								split.PayNum=voidPayment.PayNum;
								PaySplits.Insert(split);
							}
							string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,_listSplitsCur);
							if(!string.IsNullOrEmpty(strErrorMsg)) {
								MessageBox.Show(strErrorMsg);
							}
						}
						MessageBox.Show("Void successful.");
						DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
						return resultNote;
					}
					else {//Not Void
						_wasCreditCardSuccessful=true; //Will void the transaction if user cancels out of window.
					}
					_payConnectRequest=FormP.Request;
				}
				textNote.Text+=((textNote.Text=="")?"":Environment.NewLine)+resultNote;
				textNote.SelectionStart=textNote.TextLength;
				textNote.ScrollToCaret();//Scroll to the end of the text box to see the newest notes.
				_paymentCur.PayNote=textNote.Text;
				_paymentCur.PaymentSource=CreditCardSource.PayConnect;
				_paymentCur.ProcessStatus=ProcessStat.OfficeProcessed;
				Payments.Update(_paymentOld,true);
			}
			if(!string.IsNullOrEmpty(_paymentCur.Receipt)) {
				butPrintReceipt.Visible=true;
				if(Preferences.GetBool(PreferenceName.AllowEmailCCReceipt)) {
					butEmailReceipt.Visible=true;
				}
			}
			if(FormP.Response==null || FormP.Response.StatusCode!="0") { //The transaction failed.
				if(FormP.TranType==PayConnectService.transType.SALE || FormP.TranType==PayConnectService.transType.AUTH) {
					textAmount.Text=FormP.AmountCharged;//Preserve the amount so the user can try the payment again more easily.
				}
				_isCCDeclined=true;
				_wasCreditCardSuccessful=false;
				return null;
			}
			return resultNote;
		}

		///<summary>Launches the PaySimple transaction window.  Returns null upon failure, otherwise returns the transaction detail as a string.
		///If prepaidAmt is not zero, then will show the PaySimple window with the given prepaid amount and let the user enter card # and exp.
		///A patient is not required for prepaid cards.</summary>
		public string MakePaySimpleTransaction(double prepaidAmt=0,string carrierName="") {
			if(!HasPaySimple()) {
				return null;
			}
			CreditCard cc=null;
			List<CreditCard> creditCards=null;
			decimal amount=Math.Abs(PIn.Decimal(textAmount.Text));//PaySimple always wants a positive number even for voids and returns.
			if(prepaidAmt==0) {
				creditCards=CreditCards.Refresh(_patCur.PatNum);
				if(comboCreditCards.SelectedIndex < creditCards.Count) {
					cc=creditCards[comboCreditCards.SelectedIndex];
				}
			}
			else {//Prepaid card
				amount=(decimal)prepaidAmt;
			}
			FormPaySimple form=new FormPaySimple(_paymentCur.ClinicNum,_patCur,amount,cc,carrierName:carrierName);
			form.ShowDialog();
			Program prog=Programs.GetCur(ProgramName.PaySimple);
			if(prepaidAmt==0) {//Regular credit cards (not prepaid cards).
				//If PaySimple response is not null, refresh comboCreditCards and select the index of the card used for this payment if the token was saved
				creditCards=CreditCards.Refresh(_patCur.PatNum);
				string paySimpleToken=cc==null ? "" : cc.PaySimpleToken;
				if(form.ApiResponseOut!=null) {
					paySimpleToken=form.ApiResponseOut.PaySimpleToken;
				}
				AddCreditCardsToCombo(creditCards,x => x.PaySimpleToken==paySimpleToken && !string.IsNullOrEmpty(paySimpleToken));
				//still need to add functionality for accountingAutoPay
				//paytype could be an empty string
				string paytype=ProgramProperties.GetPropValForClinicOrDefault(prog.Id,PaySimple.PropertyDescs.PaySimplePayTypeCC,_paymentCur.ClinicNum);
				listPayType.SelectedIndex=Definitions.GetOrder(DefinitionCategory.PaymentTypes,PIn.Long(paytype));
				SetComboDepositAccounts();
			}
			if(prepaidAmt!=0) {
				if(form.ApiResponseOut!=null) { //The transaction succeeded.
					if(form.ApiResponseOut.CCSource==CreditCardSource.PaySimpleACH) {
						_paymentCur.PaymentStatus=PaymentStatus.PaySimpleAchPosted;
						_paymentCur.ExternalId=form.ApiResponseOut.RefNumber;
					}
					return form.ApiResponseOut.ToNoteString();
				}
				return null;
			}
			string resultNote=null;
			if(form.ApiResponseOut!=null) { //The transaction succeeded.
				_isCCDeclined=false;
				resultNote=form.ApiResponseOut.ToNoteString();
				_paymentCur.PaymentSource=form.ApiResponseOut.CCSource;
				if(form.ApiResponseOut.CCSource==CreditCardSource.PaySimpleACH) {
					string paytype=ProgramProperties.GetPropValForClinicOrDefault(prog.Id,PaySimple.PropertyDescs.PaySimplePayTypeACH,
						_paymentCur.ClinicNum);
					_paymentCur.PaymentStatus=PaymentStatus.PaySimpleAchPosted;
					_paymentCur.ExternalId=form.ApiResponseOut.RefNumber;
					int defOrder=Definitions.GetOrder(DefinitionCategory.PaymentTypes,PIn.Long(paytype));
					//paytype could be an empty string, so then leave listPayType as it was.
					if(defOrder>=-1) {
						listPayType.SelectedIndex=defOrder;
					}
				}
				if(form.ApiResponseOut.TransType==PaySimple.TransType.RETURN) {
					textAmount.Text="-"+form.ApiResponseOut.Amount.ToString("F");
					_paymentCur.Receipt=form.ApiResponseOut.TransactionReceipt;
				}
				else if(form.ApiResponseOut.TransType==PaySimple.TransType.AUTH) {
					textAmount.Text=form.ApiResponseOut.Amount.ToString("F");
				}
				else if(form.ApiResponseOut.TransType==PaySimple.TransType.SALE) {
					textAmount.Text=form.ApiResponseOut.Amount.ToString("F");
					_paymentCur.Receipt=form.ApiResponseOut.TransactionReceipt;
				}
				if(form.ApiResponseOut.TransType==PaySimple.TransType.VOID) {//Close FormPayment window now so the user will not have the option to hit Cancel
					if(IsNew) {
						if(!_wasCreditCardSuccessful) {
							textAmount.Text="-"+form.ApiResponseOut.Amount.ToString("F");
							textNote.Text+=((textNote.Text=="") ? "" : Environment.NewLine)+resultNote;
						}
						_paymentCur.Receipt=form.ApiResponseOut.TransactionReceipt;
						if(SavePaymentToDb()) {
							MessageBox.Show("Void successful.");
							DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
						}
						return resultNote;
					}
					if(!IsNew || _wasCreditCardSuccessful) {//Create a new negative payment if the void is being run from an existing payment
						if(_listSplitsCur.Count==0) {
							AddOneSplit();
							Reinitialize();
						}
						else if(_listSplitsCur.Count==1//if one split
							&& _listSplitsCur[0].PayPlanNum!=0//and split is on a payment plan
							&& _listSplitsCur[0].SplitAmt!=_paymentCur.PayAmt)//and amount doesn't match payment
						{
							_listSplitsCur[0].SplitAmt=_paymentCur.PayAmt;//make amounts match automatically
							textSplitTotal.Text=textAmount.Text;
						}
						_paymentCur.IsSplit=_listSplitsCur.Count>1;
						Payment voidPayment=_paymentCur.Clone();
						voidPayment.PayAmt*=-1;//the negation of the original amount
						voidPayment.PayNote=resultNote;
						voidPayment.Receipt=form.ApiResponseOut.TransactionReceipt;
						voidPayment.PaymentSource=CreditCardSource.PaySimple;
						voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
						voidPayment.PayNum=Payments.Insert(voidPayment);
						foreach(PaySplit splitCur in _listSplitsCur) {//Modify the paysplits for the original transaction to work for the void transaction
							PaySplit split=splitCur.Copy();
							split.SplitAmt*=-1;
							split.PayNum=voidPayment.PayNum;
							PaySplits.Insert(split);
						}
						string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,_listSplitsCur);
						if(!string.IsNullOrEmpty(strErrorMsg)) {
							MessageBox.Show(strErrorMsg);
						}
					}
					MessageBox.Show("Void successful.");
					DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
					return resultNote;
				}
				else {//Not Void
					_wasCreditCardSuccessful=true; //Will void the transaction if user cancels out of window.
				}
				_paySimpleResponse=form.ApiResponseOut;
			}
			if(!string.IsNullOrWhiteSpace(resultNote)) {
				textNote.Text+=((textNote.Text=="") ? "" : Environment.NewLine)+resultNote;
			}
			textNote.SelectionStart=textNote.TextLength;
			textNote.ScrollToCaret();//Scroll to the end of the text box to see the newest notes.
			_paymentCur.PayNote=textNote.Text;
			if(_paymentCur.PaymentSource==CreditCardSource.None) {
				_paymentCur.PaymentSource=CreditCardSource.PaySimple;
			}
			_paymentCur.ProcessStatus=ProcessStat.OfficeProcessed;
			Payments.Update(_paymentCur,true);
			if(!string.IsNullOrEmpty(_paymentCur.Receipt)) {
				butPrintReceipt.Visible=true;
				if(Preferences.GetBool(PreferenceName.AllowEmailCCReceipt)) {
					butEmailReceipt.Visible=true;
				}
			}
			if(form.ApiResponseOut==null || form.ApiResponseOut.Status.ToLower()=="failed") { //The transaction failed.
				//PaySimple checks the transaction type here and sets the amount the user chose to the textAmount textbox. 
				//We don't have that information here so do nothing.
				_isCCDeclined=true;
				_wasCreditCardSuccessful=false;
				return null;
			}
			return resultNote;
		}

		///<summary>Launches the XCharge transaction window and then executes whatever type of transaction was selected for the current payment.
		///This is to help troubleshooting. Returns null upon failure, otherwise returns the transaction detail as a string.
		///If prepaidAmt is not zero, then will show the xcharge window with the given prepaid amount and let the user enter card # and exp.
		///A patient is not required for prepaid cards.</summary>
		public string MakeXChargeTransaction(double prepaidAmt=0) {
			//Need to refresh this list locally in case we are coming from another form
			_listPaymentTypeDefs=_listPaymentTypeDefs??Definitions.GetDefsForCategory(DefinitionCategory.PaymentTypes,true);
			_xChargeMilestone="Validation";
			CreditCard cc=null;
			List<CreditCard> creditCards=null;
			if(prepaidAmt!=0) {
				CheckUIState();//To ensure that _xProg is set and _xPath is set.  Normally this would happen when loading.  Needed for HasXCharge().
			}
			if(!HasXCharge()) {//Will show setup window if xcharge is not enabled or not completely setup yet.
				return null;
			}
			if(prepaidAmt==0) {//Validation for regular credit cards (not prepaid cards).
				if(textAmount.Text=="" || PIn.Double(textAmount.Text)==0) {
					MessageBox.Show("Please enter an amount first.");
					textAmount.Focus();
					return null;
				}
				creditCards=CreditCards.Refresh(_patCur.PatNum);
				if(comboCreditCards.SelectedIndex < creditCards.Count && comboCreditCards.SelectedIndex >-1) {
					cc=creditCards[comboCreditCards.SelectedIndex];
				}
				if(cc!=null && cc.IsXWeb()) {
					MessageBox.Show("Cards saved through XWeb cannot be used with the XCharge client program.");
					return null;
				}
				if(_listSplitsCur.Count>0 && PIn.Double(textAmount.Text)!=PIn.Double(textSplitTotal.Text)
					&& (_listSplitsCur.Count!=1 || _listSplitsCur[0].PayPlanNum==0)) //Not one paysplit attached to payplan
				{
					MessageBox.Show("Split totals must equal payment amount before running a credit card transaction.");
					return null;
				}
			}
			if(PIn.Date(textDate.Text).Date > DateTime.Today.Date
					&& !Preferences.GetBool(PreferenceName.FutureTransDatesAllowed) && !Preferences.GetBool(PreferenceName.AccountAllowFutureDebits))
			{
				MessageBox.Show("Payment date cannot be in the future.");
				return null;
			}
			_xChargeMilestone="XResult File";
			string resultfile=Storage.GetTempFileName(".txt");
			try {
				File.Delete(resultfile);//delete the old result file.
			}
			catch {
				MessageBox.Show("Could not delete XResult.txt file.  It may be in use by another program, flagged as read-only, or you might not have "
					+"sufficient permissions.");
				return null;
			}
			_xChargeMilestone="Properties";
			bool needToken=false;
			bool newCard=false;
			bool hasXToken=false;
			bool notRecurring=false;
			if(prepaidAmt==0) {
				//These UI changes only need to happen for regular credit cards when the payment window is displayed.
				string xPayTypeNum=ProgramProperties.GetPropVal(_xProg.Id,"PaymentType",_paymentCur.ClinicNum);
				//still need to add functionality for accountingAutoPay
				listPayType.SelectedIndex=Definitions.GetOrder(DefinitionCategory.PaymentTypes,PIn.Long(xPayTypeNum));
				SetComboDepositAccounts();
			}
			/*XCharge.exe [/TRANSACTIONTYPE:type] [/AMOUNT:amount] [/ACCOUNT:account] [/EXP:exp]
				[“/TRACK:track”] [/ZIP:zip] [/ADDRESS:address] [/RECEIPT:receipt] [/CLERK:clerk]
				[/APPROVALCODE:approval] [/AUTOPROCESS] [/AUTOCLOSE] [/STAYONTOP] [/MID]
				[/RESULTFILE:”C:\Program Files\X-Charge\LocalTran\XCResult.txt”*/
			ProcessStartInfo info=new ProcessStartInfo(_xPath);
			Patient pat=null;
			if(prepaidAmt==0) {
				pat=Patients.GetPat(_paymentCur.PatNum);
				if(pat==null) {
					MessageBox.Show("Invalid patient associated to this payment.");
					return null;
				}
			}
			info.Arguments="";
			double amt=PIn.Double(textAmount.Text);
			if(prepaidAmt != 0) {
				amt=prepaidAmt;
			}
			if(amt<0) {//X-Charge always wants a positive number, even for returns.
				amt*=-1;
			}
			info.Arguments+="/AMOUNT:"+amt.ToString("F2")+" ";
			_xChargeMilestone="Get Selected Credit Card";
			FormXchargeTrans FormXT=null;
			int tranType=0;//Default to 0 "Purchase" for prepaid cards.
			string cashBack=null;
			if(prepaidAmt==0) {//All regular cards (not prepaid)
				_xChargeMilestone="Transaction Window Launch";
				//Show window to lock in the transaction type.
				FormXT=new FormXchargeTrans();
				FormXT.PrintReceipt=PIn.Bool(ProgramProperties.GetPropVal(_xProg.Id,"PrintReceipt",_paymentCur.ClinicNum));
				FormXT.PromptSignature=PIn.Bool(ProgramProperties.GetPropVal(_xProg.Id,"PromptSignature",_paymentCur.ClinicNum));
				FormXT.ClinicNum=_paymentCur.ClinicNum;
				FormXT.ShowDialog();
				if(FormXT.DialogResult!=DialogResult.OK) {
					return null;
				}
				_xChargeMilestone="Transaction Window Digest";
				_paymentCur.PaymentSource=CreditCardSource.XServer;
				_paymentCur.ProcessStatus=ProcessStat.OfficeProcessed;
				tranType=FormXT.TransactionType;
				decimal cashAmt=FormXT.CashBackAmount;
				cashBack=cashAmt.ToString("F2");
				_promptSignature=FormXT.PromptSignature;
				_printReceipt=FormXT.PrintReceipt;
			}
			_xChargeMilestone="Check Duplicate Cards";
			if(cc!=null && !string.IsNullOrEmpty(cc.XChargeToken)) {//Have CC on file with an XChargeToken
				hasXToken=true;
				if(CreditCards.GetXChargeTokenCount(cc.XChargeToken,false)!=1) {
					MessageBox.Show("This card shares a token with another card. Delete it from the Credit Card Manage window and re-add it.");
					return null;
				}
				/*       ***** An example of how recurring charges work***** 
				C:\Program Files\X-Charge\XCharge.exe /TRANSACTIONTYPE:Purchase /LOCKTRANTYPE
				/AMOUNT:10.00 /LOCKAMOUNT /XCACCOUNTID:XAW0JWtx5kjG8 /RECEIPT:RC001
				/LOCKRECEIPT /CLERK:Clerk /LOCKCLERK /RESULTFILE:C:\ResultFile.txt /USERID:system
				/PASSWORD:system /STAYONTOP /AUTOPROCESS /AUTOCLOSE /HIDEMAINWINDOW
				/RECURRING /SMALLWINDOW /NORESULTDIALOG
				*/
			}
			else if(cc!=null) {//Have CC on file, no XChargeToken so not a recurring charge, and might need a token.
				notRecurring=true;
				if(!Preferences.GetBool(PreferenceName.StoreCCnumbers)) {//Use token only if user has has pref unchecked in module setup (allow store credit card nums).
					needToken=true;//Will create a token from result file so credit card info isn't saved in our db.
				}
			}
			else {//CC is null, add card option was selected in credit card drop down, no other possibility.
				newCard=true;
			}
			_xChargeMilestone="Arguments Fill Card Info";
			info.Arguments+=GetXChargeTransactionTypeCommands(tranType,hasXToken,notRecurring,cc,cashBack);
			if(prepaidAmt!=0) {
				//Zip and address are optional fields and for prepaid cards this information is probably not provided to the user.
			}
			else if(newCard) {
				info.Arguments+="\"/ZIP:"+pat.Zip+"\" ";
				info.Arguments+="\"/ADDRESS:"+pat.Address+"\" ";
			}
			else {
				if(cc.CCExpiration!=null && cc.CCExpiration.Year>2005) {
					info.Arguments+="/EXP:"+cc.CCExpiration.ToString("MMyy")+" ";
				}
				if(!string.IsNullOrEmpty(cc.Zip)) {
					info.Arguments+="\"/ZIP:"+cc.Zip+"\" ";
				}
				else {
					info.Arguments+="\"/ZIP:"+pat.Zip+"\" ";
				}
				if(!string.IsNullOrEmpty(cc.Address)) {
					info.Arguments+="\"/ADDRESS:"+cc.Address+"\" ";
				}
				else {
					info.Arguments+="\"/ADDRESS:"+pat.Address+"\" ";
				}
				if(hasXToken) {//Special parameter for tokens.
					info.Arguments+="/RECURRING ";
				}
			}
			_xChargeMilestone="Arguments Fill X-Charge Settings";
			if(prepaidAmt==0) {
				info.Arguments+="/RECEIPT:Pat"+_paymentCur.PatNum.ToString()+" ";//aka invoice#
			}
			else {
				info.Arguments+="/RECEIPT:PREPAID ";//aka invoice#
			}
			info.Arguments+="\"/CLERK:"+Security.CurrentUser.UserName+"\" /LOCKCLERK ";
			info.Arguments+="/RESULTFILE:\""+resultfile+"\" ";
			info.Arguments+="/USERID:"+ProgramProperties.GetPropVal(_xProg.Id,"Username",_paymentCur.ClinicNum)+" ";
			info.Arguments+="/PASSWORD:"+CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(_xProg.Id,"Password",_paymentCur.ClinicNum))+" ";
			info.Arguments+="/PARTIALAPPROVALSUPPORT:T ";
			info.Arguments+="/AUTOCLOSE ";
			info.Arguments+="/HIDEMAINWINDOW ";
			info.Arguments+="/SMALLWINDOW ";
			info.Arguments+="/GETXCACCOUNTID ";
			info.Arguments+="/NORESULTDIALOG ";
			_xChargeMilestone="X-Charge Launch";
			Cursor=Cursors.WaitCursor;
			Process process=new Process();
			process.StartInfo=info;
			process.EnableRaisingEvents=true;
			process.Start();
			process.WaitForExit();
			_xChargeMilestone="X-Charge Complete";
			Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
			Cursor=Cursors.Default;
			string resulttext="";
			string line="";
			bool showApprovedAmtNotice=false;
			bool xAdjust=false;
			bool xReturn=false;
			bool xVoid=false;
			double approvedAmt=0;
			double additionalFunds=0;
			string xChargeToken="";
			string accountMasked="";
			string expiration="";
			string signatureResult="";
			string receipt="";
			bool isDigitallySigned=false;
			bool updateCard=false;
			string newAccount="";
			long creditCardNum;
			DateTime newExpiration=new DateTime();
			_xChargeMilestone="Digest XResult";
			try {
				using(TextReader reader=new StreamReader(resultfile)) {
					line=reader.ReadLine();
					/*Example of successful transaction:
						RESULT=SUCCESS
						TYPE=Purchase
						APPROVALCODE=000064
						ACCOUNT=XXXXXXXXXXXX6781
						ACCOUNTTYPE=VISA*
						AMOUNT=1.77
						AVSRESULT=Y
						CVRESULT=M
					*/
					while(line!=null) {
						if(!line.StartsWith("RECEIPT=")) {//Don't include the receipt string in the PayNote
							if(resulttext!="") {
								resulttext+="\r\n";
							}
							resulttext+=line;
						}
						if(line.StartsWith("RESULT=")) {
							if(line!="RESULT=SUCCESS") {
								//Charge was a failure and there might be a description as to why it failed. Continue to loop through line.
								while(line!=null) {
									line=reader.ReadLine();
									if(line!=null && !line.StartsWith("RECEIPT=")) {//Don't include the receipt string in the PayNote
										resulttext+="\r\n"+line;
									}
								}
								needToken=false;//Don't update CCard due to failure
								newCard=false;//Don't insert CCard due to failure
								_isCCDeclined=true;
								break;
							}
							if(tranType==1) {
								xReturn=true;
							}
							if(tranType==6) {
								xAdjust=true;
							}
							if(tranType==7) {
								xVoid=true;
							}
							_isCCDeclined=false;
						}
						if(line.StartsWith("APPROVEDAMOUNT=")) {
							approvedAmt=PIn.Double(line.Substring(15));
							if(approvedAmt != amt) {
								showApprovedAmtNotice=true;
							}
						}
						if(line.StartsWith("XCACCOUNTID=")) {
							xChargeToken=PIn.String(line.Substring(12));
						}
						if(line.StartsWith("ACCOUNT=")) {
							accountMasked=PIn.String(line.Substring(8));
						}
						if(line.StartsWith("EXPIRATION=")) {
							expiration=PIn.String(line.Substring(11));
						}
						if(line.StartsWith("ADDITIONALFUNDSREQUIRED=")) {
							additionalFunds=PIn.Double(line.Substring(24));
						}
						if(line.StartsWith("SIGNATURE=") && line.Length>10) {
							signatureResult=PIn.String(line.Substring(10));
							//A successful digitally signed signature will say SIGNATURE=C:\Users\Folder\Where\The\Signature\Is\Stored.bmp
							if(signatureResult!="NOT SUPPORTED" && signatureResult!="FAILED") {
								isDigitallySigned=true;
							}
						}
						if(line.StartsWith("RECEIPT=")) {
							receipt=PIn.String(line.Replace("RECEIPT=","").Replace("\\n","\n"));//The receipt from X-Charge escapes the newline characters
							if(isDigitallySigned) {
								//Replace X____________________________ with 'Electronically signed'
								receipt.Split('\n').ToList().FindAll(x => x.StartsWith("X___")).ForEach(x => x="Electronically signed");
							}
							receipt=receipt.Replace("\r","").Replace("\n","\r\n");//remove any existing \r's before replacing \n's with \r\n's
						}
						if(line=="XCACCOUNTIDUPDATED=T") {//Decline minimizer updated the account information since the last time this card was charged
							updateCard=true;
						}
						if(line.StartsWith("ACCOUNT=")) {
							newAccount=line.Substring("ACCOUNT=".Length);
						}
						if(line.StartsWith("EXPIRATION=")) {
							string expStr=line.Substring("EXPIRATION=".Length);//Expiration should be MMYY
							newExpiration=new DateTime(PIn.Int("20"+expStr.Substring(2)),PIn.Int(expStr.Substring(0,2)),1);//First day of the month
						}
						line=reader.ReadLine();
					}
					if(needToken && !string.IsNullOrEmpty(xChargeToken) && prepaidAmt==0) {//never save token for prepaid cards
						_xChargeMilestone="Update Token";
						DateTime expDate=new DateTime(PIn.Int("20"+expiration.Right(2)),PIn.Int(expiration.Left(2)),1);
						//If the stored CC used for this X-Charge payment has a PayConnect token, and X-Charge returns a different masked number or exp date, we
						//will clear out the PayConnect token since this CC no longer refers to the same card that was used to generate the PayConnect token.
						if(!string.IsNullOrEmpty(cc.PayConnectToken) //there is a PayConnect token for this saved CC
							&& Regex.IsMatch(cc.CCNumberMasked,@"X+[0-9]{4}") //the saved CC has a masked number with the pattern XXXXXXXXXXXX1234
							&& (cc.CCNumberMasked.Right(4)!=accountMasked.Right(4) //and either the last four digits don't match what X-Charge returned
									|| cc.CCExpiration.Year!=expDate.Year //or the exp year doesn't match that returned by X-Charge
									|| cc.CCExpiration.Month!=expDate.Month)) //or the exp month doesn't match that returned by X-Charge
						{
							cc.PayConnectToken="";
							cc.PayConnectTokenExp=DateTime.MinValue;
						}
						//Only way this code can be hit is if they have set up a credit card and it does not have a token.
						//So we'll use the created token from result file and assign it to the coresponding account.
						//Also will delete the credit card number and replace it with secure masked number.
						cc.XChargeToken=xChargeToken;
						cc.CCNumberMasked=accountMasked;
						cc.CCExpiration=expDate;
						cc.Procedures=Preferences.GetString(PreferenceName.DefaultCCProcs);
						cc.CCSource=CreditCardSource.XServer;
						CreditCards.Update(cc);
					}
					if(newCard && prepaidAmt==0) {//never save card information to the patient account for prepaid cards
						if(!string.IsNullOrEmpty(xChargeToken) && FormXT.SaveToken) {
							_xChargeMilestone="Create New Credit Card Entry";
							cc=new CreditCard();
							List<CreditCard> itemOrderCount=CreditCards.Refresh(_patCur.PatNum);
							cc.ItemOrder=itemOrderCount.Count;
							cc.PatientId=_patCur.PatNum;
							cc.CCExpiration=new DateTime(Convert.ToInt32("20"+expiration.Substring(2,2)),Convert.ToInt32(expiration.Substring(0,2)),1);
							cc.XChargeToken=xChargeToken;
							cc.CCNumberMasked=accountMasked;
							cc.Procedures=Preferences.GetString(PreferenceName.DefaultCCProcs);
							cc.CCSource=CreditCardSource.XServer;
							cc.ClinicNum=_paymentCur.ClinicNum;
							creditCardNum=CreditCards.Insert(cc);
						}
						else if(string.IsNullOrEmpty(xChargeToken)) {//Shouldn't happen again but leaving just in case.
							MessageBox.Show("X-Charge didn't return a token so credit card information couldn't be saved.");
						}
					}
					if(updateCard && newAccount!="" && newExpiration.Year>1880 && prepaidAmt==0) {//Never save credit card info to patient for prepaid cards.
						if(textNote.Text!="") {
							textNote.Text+="\r\n";
						}
						if(cc.CCNumberMasked != newAccount) {
							textNote.Text+="Account number changed from"+" "+cc.CCNumberMasked+" "
								+"to"+" "+newAccount;
						}
						if(cc.CCExpiration != newExpiration) {
							textNote.Text+="Expiration changed from"+" "+cc.CCExpiration.ToString("MMyy")+" "
								+"to"+" "+newExpiration.ToString("MMyy");
						}
						cc.CCNumberMasked=newAccount;
						cc.CCExpiration=newExpiration;
						CreditCards.Update(cc);
					}
				}
			}
			catch {
				MessageBox.Show("There was a problem charging the card.  Please run the credit card report from inside X-Charge to verify that "
					+"the card was not actually charged."+"\r\n"+"If the card was charged, you need to make sure that the payment amount matches."
					+"\r\n"+"If the card was not charged, please try again.");
				return null;
			}
			_xChargeMilestone="Check Approved Amount";
			if(showApprovedAmtNotice && !xVoid && !xAdjust && !xReturn) {
				MessageBox.Show("The amount you typed in"+": "+amt.ToString("C")+"\r\n"+"does not match the approved amount returned"
					+": "+approvedAmt.ToString("C")+".\r\n"+"The amount will be changed to reflect the approved amount charged.","Alert",
					MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				textAmount.Text=approvedAmt.ToString("F");
			}
			if(xAdjust) {
				_xChargeMilestone="Check Adjust";
				MessageBox.Show("The amount will be changed to the X-Charge approved amount"+": "+approvedAmt.ToString("C"));
				textNote.Text="";
				textAmount.Text=approvedAmt.ToString("F");
			}
			else if(xReturn) {
				_xChargeMilestone="Check Return";
				textAmount.Text="-"+approvedAmt.ToString("F");
			}
			else if(xVoid) {//For prepaid cards, tranType is set to 0 "Purchase", therefore xVoid will be false.
				_xChargeMilestone="Check Void";
				if(IsNew) {
					if(!_wasCreditCardSuccessful) {
						textAmount.Text="-"+approvedAmt.ToString("F");
						textNote.Text+=resulttext;
					}
					_paymentCur.Receipt=receipt;
					if(_printReceipt && receipt!="") {
						PrintReceipt(receipt,"X-Charge receipt printed");
						_printReceipt=false;
					}
					if(SavePaymentToDb()) {
						DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
					}
					return resulttext;
				}
				_xChargeMilestone="Create Negative Payment";
				if(!IsNew || _wasCreditCardSuccessful) {//Create a new negative payment if the void is being run from an existing payment
					if(_listSplitsCur.Count==0) {
						AddOneSplit();
						Reinitialize();
					}
					else if(_listSplitsCur.Count==1//if one split
						&& _listSplitsCur[0].PayPlanNum!=0//and split is on a payment plan
						&& _listSplitsCur[0].SplitAmt!=_paymentCur.PayAmt)//and amount doesn't match payment
					{
						_listSplitsCur[0].SplitAmt=_paymentCur.PayAmt;//make amounts match automatically
						textSplitTotal.Text=textAmount.Text;
					}
					_paymentCur.IsSplit=_listSplitsCur.Count>1;
					Payment voidPayment=_paymentCur.Clone();
					voidPayment.PayAmt*=-1;//the negation of the original amount
					voidPayment.PayNote=resulttext;
					voidPayment.Receipt=receipt;
					if(_printReceipt && receipt!="") {
						PrintReceipt(receipt,"X-Charge receipt printed");
					}
					voidPayment.PaymentSource=CreditCardSource.XServer;
					voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
					voidPayment.PayNum=Payments.Insert(voidPayment);
					foreach(PaySplit splitCur in _listSplitsCur) {//Modify the paysplits for the original transaction to work for the void transaction
						PaySplit split=splitCur.Copy();
						split.SplitAmt*=-1;
						split.PayNum=voidPayment.PayNum;
						PaySplits.Insert(split);
					}
					string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,_listSplitsCur);
					if(!string.IsNullOrEmpty(strErrorMsg)) {
						MessageBox.Show(strErrorMsg);
					}
				}
				DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
				return resulttext;
			}
			_xChargeMilestone="Check Additional Funds";
			_wasCreditCardSuccessful=!_isCCDeclined;//If the transaction is not a void transaction, we will void this transaction if the user hits Cancel
			if(additionalFunds>0) {
				MessageBox.Show("Additional funds required"+": "+additionalFunds.ToString("C"));
			}
			if(textNote.Text!="") {
				textNote.Text+="\r\n";
			}
			textNote.Text+=resulttext;
			_xChargeMilestone="Receipt";
			_paymentCur.Receipt=receipt;
			if(!string.IsNullOrEmpty(receipt)) {
				butPrintReceipt.Visible=true;
				if(Preferences.GetBool(PreferenceName.AllowEmailCCReceipt)) {
					butEmailReceipt.Visible=true;
				}
				if(_printReceipt && prepaidAmt==0) {
					PrintReceipt(receipt,"X-Charge receipt printed");
				}
			}
			_xChargeMilestone="Reselect Credit Card in Combo";
			if(cc!=null && !string.IsNullOrEmpty(cc.XChargeToken) && cc.CCExpiration!=null) {
				//Refresh comboCreditCards and select the index of the card used for this payment if the token was saved
				creditCards=CreditCards.Refresh(_patCur.PatNum);
				AddCreditCardsToCombo(creditCards,x => x.XChargeToken==cc.XChargeToken
					&& x.CCExpiration.Year==cc.CCExpiration.Year
					&& x.CCExpiration.Month==cc.CCExpiration.Month);
			}
			if(_isCCDeclined) {
				return null;
			}
			return resulttext;
		}
		#endregion

		///<summary>Deletes all payment splits in the grid.</summary>
		private void butDeleteAllSplits_Click(object sender,EventArgs e) {
			gridSplits.SetSelected(true);
			DeleteSelected();
		}

		private void butDeletePayment_Click(object sender,System.EventArgs e) {
			if(textDeposit.Visible) {//this will get checked again by the middle layer
				MessageBox.Show("This payment is attached to a deposit.  Not allowed to delete.");
				return;
			}
			if(PaySplits.GetSplitsForPrepay(_listSplitsCur).Count>0) {
				MessageBox.Show("This prepayment has been allocated.  Not allowed to delete.");
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"This will delete the entire payment and all splits.")) {
				return;
			}
			//If payment is attached to a transaction which is more than 48 hours old, then not allowed to delete.
			//This is hard coded.  User would have to delete or detach from within transaction rather than here.
			Transaction trans=Transactions.GetAttachedToPayment(_paymentCur.PayNum);
			if(trans != null) {
				if(trans.DateAdded < MiscData.GetNowDateTime().AddDays(-2)) {
					MessageBox.Show("Not allowed to delete.  This payment is already attached to an accounting transaction.  You will need to detach it from "
						+"within the accounting section of the program.");
					return;
				}
				if(Transactions.IsReconciled(trans)) {
					MessageBox.Show("Not allowed to delete.  This payment is attached to an accounting transaction that has been reconciled.  You will need "
						+"to detach it from within the accounting section of the program.");
					return;
				}
				try {
					Transactions.Delete(trans);
				}
				catch(ApplicationException ex) {
					MessageBox.Show(ex.Message);
					return;
				}
			}
			try {
				Payments.Delete(_paymentCur);
			}
			catch(ApplicationException ex) {//error if attached to deposit slip
				MessageBox.Show(ex.Message);
				return;
			}
			if(!IsNew) {
				SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,_paymentCur.PatNum,"Delete for: "+Patients.GetLim(_paymentCur.PatNum).GetNameLF()+", "
					+_paymentOld.PayAmt.ToString("c")+", with payment type '"+Payments.GetPaymentTypeDesc(_paymentOld,_listPaymentTypeDefs)+"'",
					0,_paymentOld.SecDateTEdit);
			}
			DialogResult=DialogResult.OK;
		}

		private void butDeleteSplits_Click(object sender,EventArgs e) {
			DeleteSelected();
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(!SavePaymentToDb()) {
				return;
			}
			DialogResult=DialogResult.OK;
			Plugins.HookAddCode(this,"FormPayment.butOK_Click_end",_paymentCur,_listSplitsCur);
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormPayment_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult==DialogResult.OK) {
				return;
			}
			//make additional logging so user knows changes they had just made to any paysplits were rolled back.
			//individual audit trails for splits
			foreach(PaySplit split in _listSplitsCur) {
				PaySplit oldSplit=_listPaySplitsOld.FirstOrDefault(x => x.SplitNum==split.SplitNum);
				string secLogText="Payment changes canceled:";
				string changesMade="";
				if(oldSplit==null) {//null when splits are new
					secLogText="New paysplit canceled.";
					SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,split.PatNum,secLogText,0);
					continue;
				}
				changesMade+=SecurityLogEntryHelper(Providers.GetAbbr(oldSplit.ProvNum),Providers.GetAbbr(split.ProvNum),"Provider");
				changesMade+=SecurityLogEntryHelper(Clinics.GetAbbr(oldSplit.ClinicNum),Clinics.GetAbbr(split.ClinicNum),"Clinic");
				changesMade+=SecurityLogEntryHelper(oldSplit.SplitAmt.ToString("F"),split.SplitAmt.ToString("F"),"Amount");
				changesMade+=SecurityLogEntryHelper(oldSplit.PatNum.ToString(),split.PatNum.ToString(),"Patient number");
				if(changesMade!="") {
					SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,split.PatNum,secLogText+changesMade,0,oldSplit.SecDateTEdit);
				}
			}
			if(!IsNew && !_wasCreditCardSuccessful) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!_wasCreditCardSuccessful) {//new payment that was not a credit card payment that has already been processed
				try {
					Payments.Delete(_paymentCur);
				}
				catch(ApplicationException ex) {
					MsgBox.Show(ex.Message);
					e.Cancel=true;//they must either OK the payment and complete, or go back and detach the deposit that was just made. 
					return;
				}
				DialogResult=DialogResult.Cancel;
				return;
			}
			//Successful CC payment
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"This will void the transaction that has just been completed. Are you sure you want to continue?")) {
				e.Cancel=true;//Stop the form from closing
				return;
			}
			DateTime payDateCur=PIn.Date(textDate.Text);
			if(payDateCur==null || payDateCur==DateTime.MinValue) {
				MessageBox.Show("Invalid Payment Date");
				e.Cancel=true;//Stop the form from closing
				return;
			}
			if(payDateCur.Date > DateTime.Today.Date && !Preferences.GetBool(PreferenceName.AccountAllowFutureDebits) && !Preferences.GetBool(PreferenceName.FutureTransDatesAllowed)) {
				MessageBox.Show("Payment Date must not be a future date.");
				e.Cancel=true;//Stop the form from closing
				return;
			}
			//Save the credit card transaction as a new payment
			_paymentCur.PayAmt=PIn.Double(textAmount.Text);//handles blank
			_paymentCur.PayDate=payDateCur;
			_paymentCur.CheckNum=textCheckNum.Text;
			_paymentCur.BankBranch=textBankBranch.Text;
			_paymentCur.IsRecurringCC=false;
			_paymentCur.PayNote=textNote.Text;
			if(checkPayTypeNone.Checked) {
				_paymentCur.PayType=0;
			}
			else {
				_paymentCur.PayType=_listPaymentTypeDefs[listPayType.SelectedIndex].Id;
			}
			if(_listSplitsCur.Count==0) {
				AddOneSplit();
				//FillMain();
			}
			else if(_listSplitsCur.Count==1//if one split
				&& _listSplitsCur[0].PayPlanNum!=0//and split is on a payment plan
				&& _listSplitsCur[0].SplitAmt!=_paymentCur.PayAmt)//and amount doesn't match payment
			{
				_listSplitsCur[0].SplitAmt=_paymentCur.PayAmt;//make amounts match automatically
				textSplitTotal.Text=textAmount.Text;
			}
			if(_paymentCur.PayAmt!=PIn.Double(textSplitTotal.Text)) {
				MessageBox.Show("Split totals must equal payment amount.");
				DialogResult=DialogResult.None;
				return;
			}
			if(_listSplitsCur.Count>1) {
				_paymentCur.IsSplit=true;
			}
			else {
				_paymentCur.IsSplit=false;
			}
			try {
				Payments.Update(_paymentCur,true);
			}
			catch(ApplicationException ex) {//this catches bad dates.
				MessageBox.Show(ex.Message);
				e.Cancel=true;
				return;
			}
			//Set all DatePays the same.
			foreach(PaySplit paySplit in _listSplitsCur) {
				paySplit.DatePay=_paymentCur.PayDate;
			}
			bool hasChanged=PaySplits.Sync(_listSplitsCur,_listPaySplitsOld);
			if(IsNew) {
				SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,_paymentCur.PatNum,Payments.GetSecuritylogEntryText(_paymentCur,_paymentOld,IsNew,
					_listPaymentTypeDefs));
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,_paymentCur.PatNum,Payments.GetSecuritylogEntryText(_paymentCur,_paymentOld,IsNew,
					_listPaymentTypeDefs),0,_paymentOld.SecDateTEdit);
			}
			if(hasChanged) {
				string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,_listSplitsCur.Union(_listPaySplitsOld).ToList());
				if(!string.IsNullOrEmpty(strErrorMsg)) {
					MessageBox.Show(strErrorMsg);
				}
			}
			string refNum="";
			string amount="";
			string transactionID="";
			string paySimplePaymentId="";
			bool isDebit=false;
			string[] arrayNoteFields=textNote.Text.Replace("\r\n","\n").Replace("\r","\n").Split(new string[] { "\n" },StringSplitOptions.RemoveEmptyEntries);
			foreach(string noteField in arrayNoteFields) {
				if(noteField.StartsWith("Amount: ")) {
					amount=noteField.Substring(8);
				}
				if(noteField.StartsWith("Ref Number: ")) {
					refNum=noteField.Substring(12);
				}
				if(noteField.StartsWith("XCTRANSACTIONID=")) {
					transactionID=noteField.Substring(16);
				}
				if(noteField.StartsWith("APPROVEDAMOUNT=")) {
					amount=noteField.Substring(15);
				}
				if(noteField.StartsWith("TYPE=") && noteField.Substring(5)=="Debit Purchase") {
					isDebit=true;
				}
				if(noteField.StartsWith("PaySimple Transaction Number")) {
					paySimplePaymentId=noteField.Split(':')[1].Trim();//Better than substring 28, because we do not know how long the translation will be.
				}
			}
			if(refNum!="") {//Void the PayConnect transaction if there is one
				VoidPayConnectTransaction(refNum,amount);
			}
			else if(transactionID!="" && HasXCharge()) {//Void the X-Charge transaction if there is one
				VoidXChargeTransaction(transactionID,amount,isDebit);
			}
			else if(!string.IsNullOrWhiteSpace(paySimplePaymentId)) {
				string originalReceipt=_paymentCur.Receipt;
				if(_paySimpleResponse!=null) {
					originalReceipt=_paySimpleResponse.TransactionReceipt;
				}
				VoidPaySimpleTransaction(paySimplePaymentId,originalReceipt);
			}
			else {
				MessageBox.Show("Unable to void transaction");
			}
			DialogResult=DialogResult.Cancel;
		}

		///<summary>Helper struct to accommodate equating PaySplit objects to one another.
		///This window shows PaySplits in one grid and AccountEntries in another grid and selections in one grid need to affect the other.
		///Matching is done by looking for and selecting any related entries to the PaySplit in question.
		///This helper struct is required because editing old payments won't have equating PaySplit objects but will have equating SplitNums
		///and new PaySplits won't have equating SplitNums but will have equating TagOD objects.  Both need to be supported at the same time.
		///E.g. if a PaySplit is selected, loop through the account entries in the other grid looking for a match in the SplitCollection.</summary>
		private struct PaySplitHelper {
			///<summary>The primary key of PaySplitCur.  Can be 0 when the PaySplit is new (user just added it and hasn't been inserted yet).</summary>
			public long SplitNumCur;
			///<summary>The GUID from the TagOD on the PaySplit object that this PaySplitHelper represents.</summary>
			public string SplitGUID;

			public PaySplitHelper(PaySplit paySplitCur) {
				SplitNumCur=paySplitCur.SplitNum;
				SplitGUID=(string)paySplitCur.TagOD;
			}

			public override bool Equals(object obj) {
				if(!(obj is PaySplitHelper)) {
					return false;
				}
				if(SplitNumCur > 0) {
					return SplitNumCur.Equals(((PaySplitHelper)obj).SplitNumCur);
				}
				return SplitGUID.Equals(((PaySplitHelper)obj).SplitGUID);
			}

			public override int GetHashCode() {
				if(SplitNumCur > 0) {
					return SplitNumCur.GetHashCode();
				}
				return SplitGUID.GetHashCode();
			}
		}

	}
}
