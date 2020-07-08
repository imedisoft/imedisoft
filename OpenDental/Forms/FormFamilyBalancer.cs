using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormFamilyBalancer:ODForm {
		///<summary>Set to the total number of guarantors in the database by the first balancer thread that runs.</summary>
		private int _guarantorTotal;
		///<summary>The number of new payments made by the balancer thread.</summary>
		private int _paymentCount;
		///<summary>The thread that executes an income transfer for every family in the database.</summary>
		private ODThread _threadBalancer;
		///<summary>A queue of PatNums for guarantors that need to be processed by the balancer thread.</summary>
		private ConcurrentQueue<long> _queueGuarantors;

		public FormFamilyBalancer() {
			InitializeComponent();
			labelProgress.Text="";
			labelPayments.Text="";
			datePicker.Value=DateTime.Now;
			datePicker.MaxDate=DateTime.Now;//Users can only pick dates in the past.
		}

		private void timerProgress_Tick(object sender,EventArgs e) {
			if(_guarantorTotal < 1 || _queueGuarantors==null || !butStartPause.Visible) {
				return;
			}
			progressBarTransfer.Maximum=_guarantorTotal;
			int progressBarValue=(_guarantorTotal-_queueGuarantors.Count);
			progressBarTransfer.Value=progressBarValue;
			//Use the StringFormat N0 so that commas show for larger numbers.
			labelProgress.Text=$"{progressBarValue:N0} / {_guarantorTotal:N0}";
			if(_queueGuarantors.Count==0) {
				labelProgress.Text="Done";
				butStartPause.Visible=false;
			}
			labelPayments.Text=$"{_paymentCount} New Payments";
		}

		private void InstantiateBalancerThread() {
			if(_threadBalancer!=null && !_threadBalancer.HasQuit) {
				return;
			}
			_threadBalancer=new ODThread(ThreadWorker);
			_threadBalancer.Tag=datePicker.Value;
			_threadBalancer.Name="FamilyBalancerThread";
			_threadBalancer.AddExceptionHandler(ex => ex.DoNothing());
		}

		private bool IsValid() {
			//FormIncomeTransferManage requires PaymentCreate to run.
			//This form requires SecurityAdmin and a password to open, and although rare, a SecuirtyAdmin doesn't have to have PaymentCreate permission.
			if(!Security.IsAuthorized(Permissions.PaymentCreate,datePicker.Value.Date)) {
				return false;
			}
			//Make sure the user wants to run the tool.
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"This process can take a long time and can only be manually reversed.\r\n\r\nContinue?")) {
				return false;
			}
			return true;
		}

		private void ThreadWorker(ODThread thread) {
			if(thread.Tag==null || thread.Tag.GetType()!=typeof(DateTime)) {
				return;
			}
			DateTime datePay=(DateTime)thread.Tag;
			if(_queueGuarantors==null) {
				//Fill the queue of guarantors that the thread will use as a way to determine if it has more work to do.
				_queueGuarantors=new ConcurrentQueue<long>(Patients.GetAllGuarantors());
				_guarantorTotal=_queueGuarantors.Count;
			}
			while(!thread.HasQuit && _queueGuarantors.TryDequeue(out long guarantor)) {
				Family fam=Patients.GetFamily(guarantor);
				Patient patCur=fam.GetPatient(guarantor);
				try {
					PaymentEdit.TransferClaimsPayAsTotal(patCur.PatNum,fam.GetPatNums(),"Automatic transfer of claims pay as total from family balancer.");
				}
				catch(Exception) {
					continue;
				}
				PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(fam.GetPatNums(),patCur.PatNum,
					new List<PaySplit>(),new Payment(),new List<AccountEntry>(),isIncomeTxfr:true);
				if(!PaymentEdit.TryCreateIncomeTransfer(constructResults.ListAccountCharges,datePay,out PaymentEdit.IncomeTransferData data)) {
					continue;
				}
				if(data.ListSplitsCur.IsNullOrEmpty()) {
					continue;
				}
				Payment paymentCur=new Payment();
				paymentCur.PayDate=datePay;
				paymentCur.PatNum=patCur.PatNum;
				//Explicitly set ClinicNum=0, since a pat's ClinicNum will remain set if the user enabled clinics, assigned patients to clinics, and then
				//disabled clinics because we use the ClinicNum to determine which PayConnect or XCharge/XWeb credentials to use for payments.
				paymentCur.ClinicNum=0;
				if(PrefC.HasClinicsEnabled) {
					paymentCur.ClinicNum=Clinics.ClinicNum;
					if((PayClinicSetting)PrefC.GetInt(PrefName.PaymentClinicSetting)==PayClinicSetting.PatientDefaultClinic) {
						paymentCur.ClinicNum=patCur.ClinicNum;
					}
					else if((PayClinicSetting)PrefC.GetInt(PrefName.PaymentClinicSetting)==PayClinicSetting.SelectedExceptHQ) {
						paymentCur.ClinicNum=(Clinics.ClinicNum==0 ? patCur.ClinicNum : Clinics.ClinicNum);
					}
				}
				paymentCur.DateEntry=DateTimeOD.Today;
				paymentCur.PaymentSource=CreditCardSource.None;
				paymentCur.ProcessStatus=ProcessStat.OfficeProcessed;
				paymentCur.PayAmt=0;
				paymentCur.PayType=0;
				Payments.Insert(paymentCur,data.ListSplitsCur);
				SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,patCur.PatNum,$"Income transfer created by the Family Balancer tool.");
				_paymentCount++;
				Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(patCur.PatNum,data.ListSplitsCur);
			}
		}

		private void butStartPause_Click(object sender,EventArgs e) {
			if(_threadBalancer==null) {
				if(!IsValid()) {
					return;
				}
				//Disable the date picker so that it cannot change for the rest of the life of this form.
				datePicker.Enabled=false;
				//Start the progress timer and let it run for the rest of the life of this form.
				timerProgress.Start();
				//Instantiate the thread so that we never get back into this if statement again. The thread will be told to start later.
				InstantiateBalancerThread();
			}
			if(butStartPause.Text=="Start") {
				//Since the user has the ability to start and stop this tool, check to see if the thread was manually stopped (HasQuit==true).
				if(_threadBalancer.HasQuit) {
					//Instantiate a new thread that will pick up where the last one left off. This is because we do not have access to flip HasQuit to false.
					InstantiateBalancerThread();
				}
				_threadBalancer.Start();
				butStartPause.Text="Pause";
			}
			else {
				_threadBalancer.QuitSync(5000);//Give the thread up to five seconds to quit before aborting.
				butStartPause.Text="Start";
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}

		private void FormFamilyBalancer_FormClosing(object sender,FormClosingEventArgs e) {
			if(_threadBalancer!=null && !_threadBalancer.HasQuit) {
				_threadBalancer.QuitSync(5000);//Give the thread up to five seconds to quit before aborting.
			}
		}
	}
}

