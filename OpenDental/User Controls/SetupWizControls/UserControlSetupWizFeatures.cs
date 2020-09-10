using System;
using System.Linq;
using System.Windows.Forms;
using Imedisoft.Data;
using OpenDentBusiness;


namespace OpenDental.User_Controls.SetupWizard {
	public partial class UserControlSetupWizFeatures:SetupWizControl {
		public UserControlSetupWizFeatures() {
			InitializeComponent();
			this.OnControlDone += ControlDone;
		}

		private void UserControlSetupWizFeatures_Load(object sender,EventArgs e) {
			RefreshControls();
		}

		private void RefreshControls() {
			checkCapitation.Checked=!Preferences.GetBool(PreferenceName.EasyHideCapitation);
			checkMedicaid.Checked=!Preferences.GetBool(PreferenceName.EasyHideMedicaid);
			checkInsurance.Checked=!Preferences.GetBool(PreferenceName.EasyHideInsurance);
			checkClinical.Checked=!Preferences.GetBool(PreferenceName.EasyHideClinical);
			checkNoClinics.Checked=PrefC.HasClinicsEnabled;
			checkMedicalIns.Checked=Preferences.GetBool(PreferenceName.ShowFeatureMedicalInsurance);
			checkEhr.Checked=Preferences.GetBool(PreferenceName.ShowFeatureEhr);
			IsDone=true;
		}

		private void labelInfo_MouseClick(object sender,MouseEventArgs e) {
			panelInfo.Controls.OfType<Label>().ToList().ForEach(x => x.ImageIndex=0);
			//foreach(Control item in panelInfo.Controls) {
			//	if(item.GetType() == typeof(Label)) {
			//		((Label)item).ImageIndex = 0;
			//	}
			//}
			((Label)sender).ImageIndex = 1;
			labelExplanation.Text = (string)((Label)sender).Tag;
		}

		private void butAdvanced_Click(object sender,EventArgs e) {
			new FormShowFeatures().ShowDialog();
			RefreshControls();
		}

		private void ControlDone(object sender, EventArgs e) {
			if(
				Preferences.Set(PreferenceName.EasyHideCapitation,!checkCapitation.Checked)
				| Preferences.Set(PreferenceName.EasyHideMedicaid,!checkMedicaid.Checked)
				| Preferences.Set(PreferenceName.EasyHideInsurance,!checkInsurance.Checked)
				| Preferences.Set(PreferenceName.EasyHideClinical,!checkClinical.Checked)
				| Preferences.Set(PreferenceName.EasyNoClinics,!checkNoClinics.Checked)
				| Preferences.Set(PreferenceName.ShowFeatureMedicalInsurance,checkMedicalIns.Checked)
				| Preferences.Set(PreferenceName.ShowFeatureEhr,checkEhr.Checked)
			) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
	}
}
