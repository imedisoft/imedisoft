using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Imedisoft.Data;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormAllergyDefEdit:ODForm {
		public AllergyDef AllergyDefCur;

		public FormAllergyDefEdit() {
			InitializeComponent();
			
		}

		private void FormAllergyEdit_Load(object sender,EventArgs e) {
			textDescription.Text=AllergyDefCur?.Description??"";//set description if available. New allergies can be added with descriptions. 
			if(!AllergyDefCur.IsNew) { 
				checkHidden.Checked=AllergyDefCur.IsHidden;
			}
			for(int i=0;i<Enum.GetNames(typeof(SnomedAllergy)).Length;i++) {
				comboSnomedAllergyType.Items.Add(Enum.GetNames(typeof(SnomedAllergy))[i]);
			}
			comboSnomedAllergyType.SelectedIndex=(int)AllergyDefCur.SnomedType;
			textMedication.Text=Medications.GetDescription(AllergyDefCur.MedicationId);
			textUnii.Text=AllergyDefCur.UniiCode;
		}

		private void butUniiToSelect_Click(object sender,EventArgs e) {
			//FormSnomeds formS=new FormSnomeds();
			//formS.IsSelectionMode=true;
			//if(formS.ShowDialog()==DialogResult.OK) {
			//	snomedAllergicTo=formS.SelectedSnomed;
			//	//textSnomedAllergicTo.Text=snomedAllergicTo.Description;
			//}
			//TODO: Implement similar code for Unii
		}

		private void butMedicationSelect_Click(object sender,EventArgs e) {
			FormMedications FormM=new FormMedications();
			FormM.IsSelectionMode=true;
			FormM.ShowDialog();
			if(FormM.DialogResult!=DialogResult.OK){
				return;
			}
			AllergyDefCur.MedicationId=FormM.SelectedMedicationNum;
			textMedication.Text=Medications.GetDescription(AllergyDefCur.MedicationId);
		}

		private void butNoneUniiTo_Click(object sender,EventArgs e) {
			//TODO: Implement this
		}

		private void butNone_Click(object sender,EventArgs e) {
			AllergyDefCur.MedicationId=0;
			textMedication.Text="";
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDescription.Text.Trim()=="") {
				MessageBox.Show("Description cannot be blank.");
				return;
			}
			if(textUnii.Text!="" && textMedication.Text!="") {
				MessageBox.Show("Only one code is allowed per allergy def.");
				return;
			}
			string validChars="ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			StringBuilder notAllowed=new StringBuilder();
			for(int i=0;i<textUnii.Text.Length;i++) {
				if(validChars.IndexOf(textUnii.Text[i])==-1) {//Not found.
					notAllowed.Append(textUnii.Text[i]);
				}
			}
			if(notAllowed.ToString()!="") {
				MessageBox.Show("UNII code has invalid characters: "+notAllowed);
				return;
			}
			if(textUnii.Text!="" && textUnii.Text.Length!=10) {
				MessageBox.Show("UNII code must be 10 characters in length.");
				return;
			}
			AllergyDefCur.Description=textDescription.Text;
			AllergyDefCur.IsHidden=checkHidden.Checked;
			AllergyDefCur.SnomedType=(SnomedAllergy)comboSnomedAllergyType.SelectedIndex;
			AllergyDefCur.UniiCode=textUnii.Text;
			//if(snomedAllergicTo!=null) { //TODO: Do UNII check once the table is added
			//	AllergyDefCur.SnomedAllergyTo=snomedAllergicTo.SnomedCode;
			//}
			if(AllergyDefCur.IsNew) {
				AllergyDefs.Insert(AllergyDefCur);
			}
			else {
				AllergyDefs.Update(AllergyDefCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!AllergyDefCur.IsNew) {
				if(!AllergyDefs.DefIsInUse(AllergyDefCur.Id)) {
					AllergyDefs.Delete(AllergyDefCur.Id);
				}
				else {
					MessageBox.Show("Cannot delete allergies in use.");
					return;
				}
			}
			DialogResult=DialogResult.Cancel;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}