using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using Imedisoft.Data;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSubscriberMove:ODForm {

		private InsurancePlan _intoInsPlan;
		private InsurancePlan _fromInsPlan;

		public FormSubscriberMove() {
			InitializeComponent();
			
		}

		private void FormSubscriberMove_Load(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPlanChangeSubsc)) {
				DialogResult=DialogResult.Cancel;
				return;
			}
		}

		private void butChangePatientInto_Click(object sender,EventArgs e) {
			FormInsPlans formIP=new FormInsPlans();
			formIP.IsSelectMode=true;
			if(formIP.ShowDialog()==DialogResult.OK) {
				_intoInsPlan=formIP.SelectedPlan;
				textCarrierNameInto.Text=Carriers.GetName(_intoInsPlan.CarrierId);
			}
		}

		private void butChangePatientFrom_Click(object sender,EventArgs e) {
			FormInsPlans formIP=new FormInsPlans();
			formIP.IsSelectMode=true;
			if(formIP.ShowDialog()==DialogResult.OK) {
				_fromInsPlan=formIP.SelectedPlan;
				textCarrierNameFrom.Text=Carriers.GetName(_fromInsPlan.CarrierId);
			}
		}

		private void butViewInsPlanInto_Click(object sender,EventArgs e) {
			if(_intoInsPlan==null || InsPlans.GetPlan(_intoInsPlan.Id,new List<InsurancePlan>())==null) {
				MessageBox.Show("Valid insurance plan not selected.\r\nPlease select a valid insurance plan using the picker button.");
				return;
			}
			FormInsPlan formIP=new FormInsPlan(_intoInsPlan,null,null);
			formIP.ShowDialog();
		}

		private void butViewInsPlanFrom_Click(object sender,EventArgs e) {
			if(_fromInsPlan==null || InsPlans.GetPlan(_fromInsPlan.Id,new List<InsurancePlan>())==null) {
				MessageBox.Show("Valid insurance plan not selected.\r\nPlease select a valid insurance plan using the picker button.");
				return;
			}
			FormInsPlan formIP=new FormInsPlan(_fromInsPlan,null,null);
			formIP.ShowDialog();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(_fromInsPlan==null || InsPlans.GetPlan(_fromInsPlan.Id,new List<InsurancePlan>())==null) {
				MessageBox.Show("Please pick a valid plan to move subscribers from.");
				return;
			}
			if(_intoInsPlan==null || InsPlans.GetPlan(_intoInsPlan.Id,new List<InsurancePlan>())==null) {
				MessageBox.Show("Please pick a valid plan to move subscribers to.");
				return;
			}
			if(_fromInsPlan.Id==_intoInsPlan.Id) {
				MessageBox.Show("Can not move a plan into itself.");
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Moving subscribers is irreversible.  Always make a full backup before moving subscribers.  "
				+"Patient specific benefits, subscriber notes, benefit notes, and effective dates will not be copied to the other plan."
				+"\r\n\r\nRunning this tool can take several minutes to run.  We recommend running it after business hours or when network usage is low."
				+"\r\n\r\nClick OK to continue, or click Cancel to abort.")) 
			{
				return;
			}
			try {
				Cursor=Cursors.WaitCursor;
				long insSubModifiedCount=InsSubs.MoveSubscribers(_fromInsPlan.Id,_intoInsPlan.Id);
				Cursor=Cursors.Default;
				MessageBox.Show("Count of Subscribers Moved"+": "+insSubModifiedCount);
			}
			catch(ApplicationException ex) {//The tool was blocked due to validation failure.
				Cursor=Cursors.Default;
				MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(ex.Message);//No translaion here, because translation was done in the business layer.
				msgBox.ShowDialog();
				return;//Since this exception is due to validation failure, do not close the form.  Let the user manually click Cancel so they know what happened.
			}
			SecurityLogs.MakeLogEntry(Permissions.InsPlanChangeSubsc,0,"Subscribers Moved from"+" "+_fromInsPlan.Id+" "+"to"+" "+_intoInsPlan.Id);
			DialogResult=DialogResult.OK;//Closes the form.
		}

		private void butCancel_Click(object sender,EventArgs e) {
			//probably don't need this log entry, but here to maintain old behavior
			SecurityLogs.MakeLogEntry(Permissions.InsPlanChangeSubsc,0,"Subscriber Move Cancel");
			DialogResult=DialogResult.Cancel;//Closes the form.
		}

	}
}