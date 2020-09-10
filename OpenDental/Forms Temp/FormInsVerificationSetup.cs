using System;
using System.Windows.Forms;
using Imedisoft.Data;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormInsVerificationSetup:ODForm {
		private bool _hasChanged;

		public FormInsVerificationSetup() {
			InitializeComponent();
			
		}

		private void FormInsVerificationSetup_Load(object sender,EventArgs e) {
			textInsBenefitEligibilityDays.Text=POut.Int(PrefC.GetInt(PreferenceName.InsVerifyBenefitEligibilityDays));
			textPatientEnrollmentDays.Text=POut.Int(PrefC.GetInt(PreferenceName.InsVerifyPatientEnrollmentDays));
			textScheduledAppointmentDays.Text=POut.Int(PrefC.GetInt(PreferenceName.InsVerifyAppointmentScheduledDays));
			textPastDueDays.Text=POut.Int(PrefC.GetInt(PreferenceName.InsVerifyDaysFromPastDueAppt));
			checkInsVerifyUseCurrentUser.Checked=Preferences.GetBool(PreferenceName.InsVerifyDefaultToCurrentUser);
			checkInsVerifyExcludePatVerify.Checked=Preferences.GetBool(PreferenceName.InsVerifyExcludePatVerify);
			checkFutureDateBenefitYear.Checked=Preferences.GetBool(PreferenceName.InsVerifyFutureDateBenefitYear);
			if(!Preferences.GetBool(PreferenceName.ShowFeaturePatientClone)) {
				checkExcludePatientClones.Visible=false;
			}
			else {
				checkExcludePatientClones.Checked=Preferences.GetBool(PreferenceName.InsVerifyExcludePatientClones);
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
			if(Preferences.Set(PreferenceName.InsVerifyBenefitEligibilityDays,insBenefitEligibilityDays)
				| Preferences.Set(PreferenceName.InsVerifyPatientEnrollmentDays,patientEnrollmentDays)
				| Preferences.Set(PreferenceName.InsVerifyAppointmentScheduledDays,scheduledAppointmentDays)
				| Preferences.Set(PreferenceName.InsVerifyDaysFromPastDueAppt,pastDueDays)
				| Preferences.Set(PreferenceName.InsVerifyExcludePatVerify,checkInsVerifyExcludePatVerify.Checked)
				| Preferences.Set(PreferenceName.InsVerifyExcludePatientClones,checkExcludePatientClones.Checked)
				| Preferences.Set(PreferenceName.InsVerifyFutureDateBenefitYear,checkFutureDateBenefitYear.Checked)
				| Preferences.Set(PreferenceName.InsVerifyDefaultToCurrentUser,checkInsVerifyUseCurrentUser.Checked)) 
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