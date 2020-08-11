using System;
using System.Linq;
using System.Windows.Forms;
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
			checkCapitation.Checked=!Prefs.GetBool(PrefName.EasyHideCapitation);
			checkMedicaid.Checked=!Prefs.GetBool(PrefName.EasyHideMedicaid);
			checkInsurance.Checked=!Prefs.GetBool(PrefName.EasyHideInsurance);
			checkClinical.Checked=!Prefs.GetBool(PrefName.EasyHideClinical);
			checkNoClinics.Checked=PrefC.HasClinicsEnabled;
			checkMedicalIns.Checked=Prefs.GetBool(PrefName.ShowFeatureMedicalInsurance);
			checkEhr.Checked=Prefs.GetBool(PrefName.ShowFeatureEhr);
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
				Prefs.Set(PrefName.EasyHideCapitation,!checkCapitation.Checked)
				| Prefs.Set(PrefName.EasyHideMedicaid,!checkMedicaid.Checked)
				| Prefs.Set(PrefName.EasyHideInsurance,!checkInsurance.Checked)
				| Prefs.Set(PrefName.EasyHideClinical,!checkClinical.Checked)
				| Prefs.Set(PrefName.EasyNoClinics,!checkNoClinics.Checked)
				| Prefs.Set(PrefName.ShowFeatureMedicalInsurance,checkMedicalIns.Checked)
				| Prefs.Set(PrefName.ShowFeatureEhr,checkEhr.Checked)
			) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
	}
}
