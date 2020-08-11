using System;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormInsVerificationSetup:ODForm {
		private bool _hasChanged;

		public FormInsVerificationSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormInsVerificationSetup_Load(object sender,EventArgs e) {
			textInsBenefitEligibilityDays.Text=POut.Int(PrefC.GetInt(PrefName.InsVerifyBenefitEligibilityDays));
			textPatientEnrollmentDays.Text=POut.Int(PrefC.GetInt(PrefName.InsVerifyPatientEnrollmentDays));
			textScheduledAppointmentDays.Text=POut.Int(PrefC.GetInt(PrefName.InsVerifyAppointmentScheduledDays));
			textPastDueDays.Text=POut.Int(PrefC.GetInt(PrefName.InsVerifyDaysFromPastDueAppt));
			checkInsVerifyUseCurrentUser.Checked=Prefs.GetBool(PrefName.InsVerifyDefaultToCurrentUser);
			checkInsVerifyExcludePatVerify.Checked=Prefs.GetBool(PrefName.InsVerifyExcludePatVerify);
			checkFutureDateBenefitYear.Checked=Prefs.GetBool(PrefName.InsVerifyFutureDateBenefitYear);
			if(!Prefs.GetBool(PrefName.ShowFeaturePatientClone)) {
				checkExcludePatientClones.Visible=false;
			}
			else {
				checkExcludePatientClones.Checked=Prefs.GetBool(PrefName.InsVerifyExcludePatientClones);
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textInsBenefitEligibilityDays.errorProvider1.GetError(textInsBenefitEligibilityDays)!="") {
				MessageBox.Show("The number entered for insurance benefit eligibility was not a valid number.  Please enter a valid number to continue.");
				return;
			}
			if(textPatientEnrollmentDays.errorProvider1.GetError(textPatientEnrollmentDays)!="") {
				MessageBox.Show("The number entered for patient enrollment was not a valid number.  Please enter a valid number to continue.");
				return;
			}
			if(textScheduledAppointmentDays.errorProvider1.GetError(textScheduledAppointmentDays)!="") {
				MessageBox.Show("The number entered for scheduled appointments was not a valid number.  Please enter a valid number to continue.");
				return;
			}
			if(textPastDueDays.errorProvider1.GetError(textPastDueDays)!="") {
				MessageBox.Show("The number entered for appointment days past due was not a valid number.  Please enter a valid number to continue.");
				return;
			}
			int insBenefitEligibilityDays=PIn.Int(textInsBenefitEligibilityDays.Text);
			int patientEnrollmentDays=PIn.Int(textPatientEnrollmentDays.Text);
			int scheduledAppointmentDays=PIn.Int(textScheduledAppointmentDays.Text);
			int pastDueDays=PIn.Int(textPastDueDays.Text);
			if(Prefs.Set(PrefName.InsVerifyBenefitEligibilityDays,insBenefitEligibilityDays)
				| Prefs.Set(PrefName.InsVerifyPatientEnrollmentDays,patientEnrollmentDays)
				| Prefs.Set(PrefName.InsVerifyAppointmentScheduledDays,scheduledAppointmentDays)
				| Prefs.Set(PrefName.InsVerifyDaysFromPastDueAppt,pastDueDays)
				| Prefs.Set(PrefName.InsVerifyExcludePatVerify,checkInsVerifyExcludePatVerify.Checked)
				| Prefs.Set(PrefName.InsVerifyExcludePatientClones,checkExcludePatientClones.Checked)
				| Prefs.Set(PrefName.InsVerifyFutureDateBenefitYear,checkFutureDateBenefitYear.Checked)
				| Prefs.Set(PrefName.InsVerifyDefaultToCurrentUser,checkInsVerifyUseCurrentUser.Checked)) 
			{
				_hasChanged=true;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormInsVerificationSetup_FormClosing(object sender,FormClosingEventArgs e) {
			if(_hasChanged) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
	}
}