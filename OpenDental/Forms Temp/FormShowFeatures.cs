using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Collections.Generic;
using System.Linq;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public partial class FormShowFeatures : ODForm {
		private bool _isClinicsEnabledInDb=false;
		private bool _hasClinicsEnabledChanged {
			get { return _isClinicsEnabledInDb!=checkEnableClinics.Checked; }
		}

		///<summary></summary>
		public FormShowFeatures()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		private void FormShowFeatures_Load(object sender, System.EventArgs e) {
			checkCapitation.Checked=!Prefs.GetBool(PrefName.EasyHideCapitation);
			checkMedicaid.Checked=!Prefs.GetBool(PrefName.EasyHideMedicaid);
			checkPublicHealth.Checked=!Prefs.GetBool(PrefName.EasyHidePublicHealth);
			checkDentalSchools.Checked=!Prefs.GetBool(PrefName.EasyHideDentalSchools);
			checkHospitals.Checked=!Prefs.GetBool(PrefName.EasyHideHospitals);
			checkInsurance.Checked=!Prefs.GetBool(PrefName.EasyHideInsurance);
			checkClinical.Checked=!Prefs.GetBool(PrefName.EasyHideClinical);
			checkBasicModules.Checked=Prefs.GetBool(PrefName.EasyBasicModules);
			_isClinicsEnabledInDb=!Prefs.GetBool(PrefName.EasyNoClinics);
			RestoreClinicCheckBox();
			checkRepeatCharges.Checked=!Prefs.GetBool(PrefName.EasyHideRepeatCharges);
			checkMedicalIns.Checked=Prefs.GetBool(PrefName.ShowFeatureMedicalInsurance);
			checkEhr.Checked=Prefs.GetBool(PrefName.ShowFeatureEhr);
			checkSuperFam.Checked=Prefs.GetBool(PrefName.ShowFeatureSuperfamilies);
			checkPatClone.Checked=Prefs.GetBool(PrefName.ShowFeaturePatientClone);
			checkShowEnterprise.Checked=Prefs.GetBool(PrefName.ShowFeatureEnterprise);
			checkShowReactivations.Checked=Prefs.GetBool(PrefName.ShowFeatureReactivations);
			checkEraShowControlId.Checked=Prefs.GetBool(PrefName.EraShowControlIdFilter);
		}

		private void checkEnableClinics_Click(object sender,EventArgs e) {
			string question="If you are subscribed to eServices, you may need to restart the eConnector when you turn clinics on or off. Continue?";
			if(!MsgBox.Show(MsgBoxButtons.YesNo,question)) {
				RestoreClinicCheckBox();
			}
		}

		private void checkEhr_Click(object sender,EventArgs e) {
			if(checkEhr.Checked && !File.Exists(ODFileUtils.CombinePaths(Application.StartupPath,"EHR.dll"))){
				checkEhr.Checked=false;
				MessageBox.Show("EHR.dll could not be found.");
				return;
			}
			MessageBox.Show("You will need to restart the program for the change to take effect.");
		}

		private void checkRestart_Click(object sender,EventArgs e) {
			MsgBox.Show("You will need to restart the program for the change to take effect.");
		}

		///<summary>Restores checkEnableClinics to original value when form was opened.</summary>
		private void RestoreClinicCheckBox() {
			checkEnableClinics.Checked=_isClinicsEnabledInDb;
		}

		///<summary>Validates that PrefName.EasyNoClinics is ok to be changed and changes it when necessary. Sends an alert to eConnector to perform the conversion.
		///If fails then restores checkEnableClinics to original value when form was opened.</summary>
		private bool IsClinicCheckBoxOk() {
			try {
				if(!_hasClinicsEnabledChanged) { //No change.
					return true;
				}
				//Turn clinics on/off locally and send the signal to other workstations. This must happen before we call HQ so we tell HQ the new value.
				Prefs.Set(PrefName.EasyNoClinics,!checkEnableClinics.Checked);
				DataValid.SetInvalid(InvalidType.Prefs);
				//Create an alert for the user to know they may need to restart the eConnector if they are subscribed to eServices
				AlertItems.Insert(new AlertItem()
				{
					Description=Lan.G(this,"Clinic Feature Changed, you may need to restart the eConnector if you are subscribed to eServices"),
					Type=AlertType.ClinicsChanged,
					Severity=SeverityType.Low,
					Actions=ActionType.OpenForm | ActionType.MarkAsRead | ActionType.Delete,
					FormToOpen=FormType.FormEServicesEConnector,
					ItemValue="Clinics turned "+(checkEnableClinics.Checked ? "On":"Off")
				});
				//Create an alert for the eConnector to perform the clinic conversion as needed.
				AlertItems.Insert(new AlertItem()
				{
					Description="Clinics Changed",
					Type=AlertType.ClinicsChangedInternal,
					Severity=SeverityType.Normal,
					Actions=ActionType.None,
					ItemValue=checkEnableClinics.Checked ? "On":"Off"
				});
				return true;
			}
			catch(Exception ex) {
				//Change it back to what the db has.
				RestoreClinicCheckBox();
				MessageBox.Show(ex.Message);
				return false;
			}	
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(!IsClinicCheckBoxOk()) {
				return;
			}
			bool hasChanged=false;
			hasChanged |= Prefs.Set(PrefName.EasyHideCapitation,!checkCapitation.Checked);
			hasChanged |= Prefs.Set(PrefName.EasyHideMedicaid,!checkMedicaid.Checked);
			hasChanged |= Prefs.Set(PrefName.EasyHidePublicHealth,!checkPublicHealth.Checked);
			hasChanged |= Prefs.Set(PrefName.EasyHideDentalSchools,!checkDentalSchools.Checked);
			hasChanged |= Prefs.Set(PrefName.EasyHideHospitals,!checkHospitals.Checked);
			hasChanged |= Prefs.Set(PrefName.EasyHideInsurance,!checkInsurance.Checked);
			hasChanged |= Prefs.Set(PrefName.EasyHideClinical,!checkClinical.Checked);
			hasChanged |= Prefs.Set(PrefName.EasyBasicModules,checkBasicModules.Checked);
			hasChanged |= Prefs.Set(PrefName.EasyHideRepeatCharges,!checkRepeatCharges.Checked);
			hasChanged |= Prefs.Set(PrefName.ShowFeatureMedicalInsurance,checkMedicalIns.Checked);
			hasChanged |= Prefs.Set(PrefName.ShowFeatureEhr,checkEhr.Checked);
			hasChanged |= Prefs.Set(PrefName.ShowFeatureSuperfamilies,checkSuperFam.Checked);
			hasChanged |= Prefs.Set(PrefName.ShowFeaturePatientClone,checkPatClone.Checked);
			hasChanged |= Prefs.Set(PrefName.ShowFeatureEnterprise,checkShowEnterprise.Checked);
			hasChanged |= Prefs.Set(PrefName.ShowFeatureReactivations,checkShowReactivations.Checked);
			hasChanged |= Prefs.Set(PrefName.EraShowControlIdFilter,checkEraShowControlId.Checked);
			if(hasChanged) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			if(_hasClinicsEnabledChanged) {
				MessageBox.Show("You will need to restart the program for the change to take effect.");
			}
			//We should use ToolBut invalidation to redraw toolbars that could've been just enabled and stop forcing customers restarting.
			//DataValid.SetInvalid(InvalidType.ToolBut);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
