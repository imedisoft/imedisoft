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
using Imedisoft.Data;

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
			
		}

		private void FormShowFeatures_Load(object sender, System.EventArgs e) {
			checkCapitation.Checked=!Preferences.GetBool(PreferenceName.EasyHideCapitation);
			checkMedicaid.Checked=!Preferences.GetBool(PreferenceName.EasyHideMedicaid);
			checkPublicHealth.Checked=!Preferences.GetBool(PreferenceName.EasyHidePublicHealth);
			checkDentalSchools.Checked=!Preferences.GetBool(PreferenceName.EasyHideDentalSchools);
			checkHospitals.Checked=!Preferences.GetBool(PreferenceName.EasyHideHospitals);
			checkInsurance.Checked=!Preferences.GetBool(PreferenceName.EasyHideInsurance);
			checkClinical.Checked=!Preferences.GetBool(PreferenceName.EasyHideClinical);
			checkBasicModules.Checked=Preferences.GetBool(PreferenceName.EasyBasicModules);
			_isClinicsEnabledInDb=!Preferences.GetBool(PreferenceName.EasyNoClinics);
			RestoreClinicCheckBox();
			checkRepeatCharges.Checked=!Preferences.GetBool(PreferenceName.EasyHideRepeatCharges);
			checkMedicalIns.Checked=Preferences.GetBool(PreferenceName.ShowFeatureMedicalInsurance);
			checkEhr.Checked=Preferences.GetBool(PreferenceName.ShowFeatureEhr);
			checkSuperFam.Checked=Preferences.GetBool(PreferenceName.ShowFeatureSuperfamilies);
			checkPatClone.Checked=Preferences.GetBool(PreferenceName.ShowFeaturePatientClone);
			checkShowEnterprise.Checked=Preferences.GetBool(PreferenceName.ShowFeatureEnterprise);
			checkShowReactivations.Checked=Preferences.GetBool(PreferenceName.ShowFeatureReactivations);
			checkEraShowControlId.Checked=Preferences.GetBool(PreferenceName.EraShowControlIdFilter);
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
				Preferences.Set(PreferenceName.EasyNoClinics,!checkEnableClinics.Checked);
				DataValid.SetInvalid(InvalidType.Prefs);
				//Create an alert for the user to know they may need to restart the eConnector if they are subscribed to eServices
				AlertItems.Insert(new AlertItem()
				{
					Description="Clinic Feature Changed, you may need to restart the eConnector if you are subscribed to eServices",
					Type=AlertType.ClinicsChanged,
					Severity=SeverityType.Low,
					Actions=ActionType.OpenForm | ActionType.MarkAsRead | ActionType.Delete,
					FormToOpen=FormType.FormEServicesEConnector,
					Details="Clinics turned "+(checkEnableClinics.Checked ? "On":"Off")
				});
				//Create an alert for the eConnector to perform the clinic conversion as needed.
				AlertItems.Insert(new AlertItem()
				{
					Description="Clinics Changed",
					Type=AlertType.ClinicsChangedInternal,
					Severity=SeverityType.Normal,
					Actions=ActionType.None,
					Details=checkEnableClinics.Checked ? "On":"Off"
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
			hasChanged |= Preferences.Set(PreferenceName.EasyHideCapitation,!checkCapitation.Checked);
			hasChanged |= Preferences.Set(PreferenceName.EasyHideMedicaid,!checkMedicaid.Checked);
			hasChanged |= Preferences.Set(PreferenceName.EasyHidePublicHealth,!checkPublicHealth.Checked);
			hasChanged |= Preferences.Set(PreferenceName.EasyHideDentalSchools,!checkDentalSchools.Checked);
			hasChanged |= Preferences.Set(PreferenceName.EasyHideHospitals,!checkHospitals.Checked);
			hasChanged |= Preferences.Set(PreferenceName.EasyHideInsurance,!checkInsurance.Checked);
			hasChanged |= Preferences.Set(PreferenceName.EasyHideClinical,!checkClinical.Checked);
			hasChanged |= Preferences.Set(PreferenceName.EasyBasicModules,checkBasicModules.Checked);
			hasChanged |= Preferences.Set(PreferenceName.EasyHideRepeatCharges,!checkRepeatCharges.Checked);
			hasChanged |= Preferences.Set(PreferenceName.ShowFeatureMedicalInsurance,checkMedicalIns.Checked);
			hasChanged |= Preferences.Set(PreferenceName.ShowFeatureEhr,checkEhr.Checked);
			hasChanged |= Preferences.Set(PreferenceName.ShowFeatureSuperfamilies,checkSuperFam.Checked);
			hasChanged |= Preferences.Set(PreferenceName.ShowFeaturePatientClone,checkPatClone.Checked);
			hasChanged |= Preferences.Set(PreferenceName.ShowFeatureEnterprise,checkShowEnterprise.Checked);
			hasChanged |= Preferences.Set(PreferenceName.ShowFeatureReactivations,checkShowReactivations.Checked);
			hasChanged |= Preferences.Set(PreferenceName.EraShowControlIdFilter,checkEraShowControlId.Checked);
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
