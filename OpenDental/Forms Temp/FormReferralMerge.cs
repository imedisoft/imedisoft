using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormReferralMerge:ODForm {
		private long _referralNumInto;
		private long _referralNumFrom;

		public FormReferralMerge() {
			InitializeComponent();
			Lan.F(this);
		}

		private void butChangeReferralInto_Click(object sender,EventArgs e) {
			FormReferralSelect FormRS=new FormReferralSelect();
			FormRS.IsSelectionMode=true;
			if(FormRS.ShowDialog()==DialogResult.OK) {
				Referral selectedReferral=FormRS.SelectedReferral;
				_referralNumInto=selectedReferral.ReferralNum;
				textReferralNameInto.Text=selectedReferral.LName+", "+selectedReferral.FName;
				textTitleInto.Text=selectedReferral.Title;
				checkIsPersonInto.Checked=!selectedReferral.NotPerson;
				checkIsDoctorInto.Checked=selectedReferral.IsDoctor;
				CheckUIState();
			}
		}

		private void butChangeReferralFrom_Click(object sender,EventArgs e) {
			FormReferralSelect FormRS=new FormReferralSelect();
			FormRS.IsSelectionMode=true;
			if(FormRS.ShowDialog()==DialogResult.OK) {
				Referral selectedReferral=FormRS.SelectedReferral;
				_referralNumFrom=selectedReferral.ReferralNum;
				textReferralNameFrom.Text=selectedReferral.LName+", "+selectedReferral.FName;
				textTitleFrom.Text=selectedReferral.Title;
				checkIsPersonFrom.Checked=!selectedReferral.NotPerson;
				checkIsDoctorFrom.Checked=selectedReferral.IsDoctor;
				CheckUIState();
			}
		}

		private void CheckUIState() {
			butMerge.Enabled=(textReferralNameInto.Text.Trim()!="" && textReferralNameFrom.Text.Trim()!="");
		}

		private void butMerge_Click(object sender,EventArgs e) {
			if(_referralNumInto==_referralNumFrom) {
				MessageBox.Show("Cannot merge the same referral.");
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Are you sure?  The results are permanent and cannot be undone.")) {
				return;
			}
			string differentFields="";
			if(textReferralNameInto.Text.Trim()!=textReferralNameFrom.Text.Trim()) {
				differentFields+=Lan.G(this,"Referral Name")+"\r\n";
			}
			if(textTitleInto.Text.Trim()!=textTitleFrom.Text.Trim()) {
				differentFields+=Lan.G(this,"Title")+"\r\n";
			}
			if(checkIsPersonInto.Checked!=checkIsPersonFrom.Checked) {
				differentFields+=Lan.G(this,"Is Person")+"\r\n";
			}
			if(checkIsDoctorInto.Checked!=checkIsDoctorFrom.Checked) {
				differentFields+=Lan.G(this,"Is Doctor")+"\r\n";
			}
			string warningMsg="";
			if(differentFields!="") {
				warningMsg+=Lan.G(this,"The following referral fields do not match")+": \r\n"+differentFields;
			}
			int patAttachCount=Referrals.CountReferralAttach(_referralNumFrom);
			warningMsg+=Lan.G(this,"The selected referrals may be different")+".  "+Lan.G(this,"This change is irreversible! The referral is attached to")+" "
				+patAttachCount+" "+Lan.G(this,"patients")+".  "+Lan.G(this,"Continue anyways?");
			if(MessageBox.Show(warningMsg,"",MessageBoxButtons.YesNo)==DialogResult.No) { 
				return;
			}
			if(!Referrals.MergeReferrals(_referralNumInto,_referralNumFrom)) {
				MessageBox.Show("Referrals failed to merge.");
				return;
			}
			MessageBox.Show("Referrals merged successfully.");
			string logText=Lan.G(this,"Referral Merge from")
				+" "+Referrals.GetNameLF(_referralNumFrom)+" "+Lan.G(this,"to")+" "+Referrals.GetNameLF(_referralNumInto)+"\r\n"
				+Lan.G(this,"Patients attached to this referral")+": "+patAttachCount.ToString();
			//Make log entry here not in parent form because we can merge multiple referrals at a time.
			SecurityLogs.MakeLogEntry(Permissions.ReferralMerge,0,logText);
			textReferralNameFrom.Text="";
			textTitleFrom.Text="";
			checkIsPersonFrom.Checked=false;
			checkIsDoctorFrom.Checked=false;
			CheckUIState();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}


	}
}