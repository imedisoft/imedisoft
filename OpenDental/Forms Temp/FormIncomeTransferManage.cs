using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;
using System.Text;
using Imedisoft.Data;

namespace OpenDental {
	public partial class FormIncomeTransferManage:ODForm {
		private Family _famCur;
		private Patient _patCur;
		private PaymentEdit.ConstructResults _constructResults;

		public FormIncomeTransferManage(Family famCur,Patient patCur) {
			_famCur=famCur;
			_patCur=patCur;
			InitializeComponent();
			
		}

		private void FormIncomeTransferManage_Load(object sender,EventArgs e) {
			//Intentionally check by the payment create permission even though it is claim supplementals (InsPayCreate).
			if(Security.IsAuthorized(Permissions.PaymentCreate,DateTime.Today,true)) {
				try {
					PaymentEdit.TransferClaimsPayAsTotal(_patCur.PatNum,_famCur.GetPatNums(),"Automatic transfer of claims pay as total from income transfer.");
				}
				catch(ApplicationException ex) {
					FriendlyException.Show(ex.Message,ex);
					return;
				}
			}
			RefreshWindow();
		}

		///<summary>Refreshes all of the data from the database and updates the UI accordingly.</summary>
		private void RefreshWindow() {
			FillTransfers();
			FillGridCharges();
			//IsIncomeTransferNeeded must be called after FillGridCharges has been invoked so that _constructResults is not null.
			butTransfer.Enabled=IsIncomeTransferNeeded();
		}

		///<summary></summary>
		private void FillTransfers() {
			string translationName=gridTransfers.TranslationName;
			gridTransfers.BeginUpdate();
			gridTransfers.Columns.Clear();
			gridTransfers.Columns.Add(new GridColumn("Date",65,HorizontalAlignment.Center));
			if(PrefC.HasClinicsEnabled) {//Clinics
				gridTransfers.Columns.Add(new GridColumn("Clinic",80){ IsWidthDynamic=true });
			}
			gridTransfers.Columns.Add(new GridColumn("Paid By",80){ IsWidthDynamic=true });
			gridTransfers.Rows.Clear();
			List<Payment> transfers=Payments.GetTransfers(_famCur.GetPatNums().ToArray());
			foreach(Payment transfer in transfers.OrderBy(x => x.PayDate)) {
				GridRow row=new GridRow();
				row.Cells.Add(transfer.PayDate.ToShortDateString());
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetAbbr(transfer.ClinicNum));
				}
				row.Cells.Add(_famCur.GetNameInFamFL(transfer.PatNum));
				row.Tag=transfer;
				gridTransfers.Rows.Add(row);
			}
			gridTransfers.EndUpdate();
			gridTransfers.ScrollToEnd();
		}

		///<summary></summary>
		private void FillGridCharges() {
			gridImbalances.BeginUpdate();
			gridImbalances.Columns.Clear();
			gridImbalances.Columns.Add(new GridColumn("Prov",80){ IsWidthDynamic=true });
			gridImbalances.Columns.Add(new GridColumn("Patient",80){ IsWidthDynamic=true });
			if(PrefC.HasClinicsEnabled) {
				gridImbalances.Columns.Add(new GridColumn("Clinic",80){ IsWidthDynamic=true });
			}
			gridImbalances.Columns.Add(new GridColumn("Charges",80,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridImbalances.Columns.Add(new GridColumn("Credits",80,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridImbalances.Columns.Add(new GridColumn("Balance",80,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridImbalances.Rows.Clear();
			_constructResults=PaymentEdit.ConstructAndLinkChargeCredits(_famCur.GetPatNums(),_patCur.PatNum,
				new List<PaySplit>(),new Payment(),new List<AccountEntry>(),isIncomeTxfr:true);
			//Get information for any patient that is not currently present within the family.
			List<long> listPatNums=_constructResults.ListAccountCharges.Select(x => x.PatNum).Distinct().ToList();
			Dictionary<long,Patient> dictPatients=Patients.GetLimForPats(listPatNums.FindAll(x => !x.In(_famCur.GetPatNums())))
				.GroupBy(x => x.PatNum)
				.ToDictionary(x => x.Key,x => x.First());
			//Add the family memebers to the dictionary of patients.
			foreach(Patient patient in _famCur.ListPats) {
				dictPatients[patient.PatNum]=patient;
			}
			//Group up account entries by Pat/Prov/Clinic and skip over any "buckets" that have the AmtEnd sum up to 0.
			var dictPatProvClinicBuckets=_constructResults.ListAccountCharges
				.GroupBy(x => new { x.PatNum,x.ProvNum,x.ClinicNum })
				.ToDictionary(x => x.Key,x => new PaymentEdit.IncomeTransferBucket(x.ToList()));
			foreach(var kvp in dictPatProvClinicBuckets) {
				//Do not display any buckets that have their AmtEnd sum to zero.
				if(kvp.Value.ListAccountEntries.Sum(x => x.AmountEnd).IsZero()) {
					continue;
				}
				AccountEntry accountEntryFirst=kvp.Value.ListAccountEntries.First();
				GridRow row=new GridRow();
				row.Cells.Add(Providers.GetAbbr(accountEntryFirst.ProvNum,includeHidden:true));
				row.Cells.Add(dictPatients[accountEntryFirst.PatNum].GetNameLFnoPref());//Will throw if patient was not in the database.  Db corruption?
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetAbbr(accountEntryFirst.ClinicNum));
				}
				row.Cells.Add(kvp.Value.ListPositiveEntries.Sum(x => x.AmountEnd).ToString("c"));
				row.Cells.Add(kvp.Value.ListNegativeEntries.Sum(x => x.AmountEnd).ToString("c"));
				row.Cells.Add(kvp.Value.ListAccountEntries.Sum(x => x.AmountEnd).ToString("c"));
				row.Tag=kvp.Value.ListAccountEntries;
				gridImbalances.Rows.Add(row);
			}
			gridImbalances.EndUpdate();
		}

		///<summary>Performs an income transfer but doesn't save the results.  Instead, returns true if any PaySplits are suggested for transfer
		///or if there is something wrong with the transfer in general so that the user can click the Transfer button and get the error message.
		///Otherwise returns false.  This method does not check permissions on purpose (so the user can get permission specific messages).</summary>
		private bool IsIncomeTransferNeeded() {
			//Make a deep copy of the current charges because the income transfer can manipulate the values within the objects.
			List<AccountEntry> listAccountEntries=_constructResults.ListAccountCharges.Select(x => x.Copy()).ToList();
			if(!PaymentEdit.TryCreateIncomeTransfer(listAccountEntries,DateTimeOD.Today,out PaymentEdit.IncomeTransferData results)) {
				return true;//Something is wrong and a transfer cannot be performed.  Let the user click the Transfer button in order to get this error.
			}
			if(results.HasInvalidSplits) {
				return true;//Let the user click the Transfer button in order to get this error.
			}
			else if(results.HasInvalidProcWithPayPlan) {
				return true;//Let the user click the Transfer button in order to get this error.
			}
			if(results.ListSplitsCur.Count>0) {
				return true;//A transfer needs to be made for this account.
			}
			//The income transfer code successfully ran and had no suggested splits to transfer therefore a transfer is not necessary.
			return false;
		}

		private void gridTransfers_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormPayment FormPayment2=new FormPayment(_patCur,_famCur,(Payment)gridTransfers.Rows[e.Row].Tag,false);
			FormPayment2.IsNew=false;
			FormPayment2.ShowDialog();
			RefreshWindow();//The user could have done anything, refresh the UI just to be safe.
		}

		///<summary>Creates paysplits for selected charges if there is enough payment left.</summary>
		private void butTransfer_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.PaymentCreate,DateTime.Today)) {
				return;
			}
			if(!PaymentEdit.TryCreateIncomeTransfer(_constructResults.ListAccountCharges,DateTimeOD.Today,out PaymentEdit.IncomeTransferData results)) {
				MsgBoxCopyPaste msgBoxCopyPaste=new MsgBoxCopyPaste(results.SummaryText);
				msgBoxCopyPaste.Show();
				return;
			}
			if(results.HasInvalidSplits) {
				MessageBox.Show("Due to Rigorous Accounting, one or more invalid transactions have been cancelled.  Please fix those manually.");
			}
			else if(results.HasInvalidProcWithPayPlan) {
				MessageBox.Show("One or more over allocated paysplit was not able to be reversed.");
			}
			if(results.ListSplitsCur.IsNullOrEmpty()) {
				return;
			}
			Payment paymentCur=new Payment();
			paymentCur.PayDate=DateTimeOD.Today;
			paymentCur.PatNum=_patCur.PatNum;
			//Explicitly set ClinicNum=0, since a pat's ClinicNum will remain set if the user enabled clinics, assigned patients to clinics, and then
			//disabled clinics because we use the ClinicNum to determine which PayConnect or XCharge/XWeb credentials to use for payments.
			paymentCur.ClinicNum=0;
			if(PrefC.HasClinicsEnabled) {//if clinics aren't enabled default to 0
				paymentCur.ClinicNum=Clinics.Active.Id;
				if((PayClinicSetting)PrefC.GetInt(PreferenceName.PaymentClinicSetting)==PayClinicSetting.PatientDefaultClinic) {
					paymentCur.ClinicNum=_patCur.ClinicNum;
				}
				else if((PayClinicSetting)PrefC.GetInt(PreferenceName.PaymentClinicSetting)==PayClinicSetting.SelectedExceptHQ) {
					paymentCur.ClinicNum=(Clinics.ClinicId==null ? _patCur.ClinicNum : Clinics.ClinicId.Value);
				}
			}
			paymentCur.DateEntry=DateTimeOD.Today;//So that it will show properly in the new window.
			paymentCur.PaymentSource=CreditCardSource.None;
			paymentCur.ProcessStatus=ProcessStat.OfficeProcessed;
			paymentCur.PayAmt=0;
			paymentCur.PayType=0;
			Payments.Insert(paymentCur);
			PaySplits.InsertManyWithAssociated(paymentCur.PayNum,results.ListSplitsCur,results.ListSplitsAssociated);
			string logText=Payments.GetSecuritylogEntryText(paymentCur,paymentCur,isNew:true)+", "+"from Income Transfer Manager.";
			SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,paymentCur.PatNum,logText);
			string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,results.ListSplitsCur);
			if(!string.IsNullOrEmpty(strErrorMsg)) {
				MessageBox.Show(strErrorMsg);
			}
			RefreshWindow();
			//Check to see if any providers have PaySplit entries associated to unallocated or unearned that have positive value.
			//Negative unallocated or unearned should never be a thing after an income transfer has been made.
			if(PaymentEdit.IsUnallocatedOrUnearnedNegative(_patCur.PatNum,_famCur,out string warningMessage,_constructResults.ListAccountCharges)) {
				string displayText="The following pre-existing payment splits cannot be processed by the income transfer tool:"
					+"\r\n"+warningMessage;
				MsgBoxCopyPaste msgBoxCopyPaste=new MsgBoxCopyPaste(displayText);
				msgBoxCopyPaste.Show();
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
			this.Close();
		}
	}
}