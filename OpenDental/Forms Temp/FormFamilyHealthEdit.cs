using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using Imedisoft.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormFamilyHealthEdit:ODForm {
		public FamilyHealth FamilyHealthCur;
		private ProblemDefinition DisDefCur;

		public FormFamilyHealthEdit() {
			InitializeComponent();
			
		}

		private void FormFamilyHealthEdit_Load(object sender,EventArgs e) {
			string[] familyRelationships=Enum.GetNames(typeof(FamilyRelationship));
			for(int i=0;i<familyRelationships.Length;i++) {
				listRelationship.Items.Add(familyRelationships[i]);
			}
			listRelationship.SelectedIndex=(int)FamilyHealthCur.Relationship;
			if(FamilyHealthCur.IsNew) {
				return; //Don't need to set any of the info below.  All null.
			}
			DisDefCur=ProblemDefinitions.GetItem(FamilyHealthCur.DiseaseDefNum);
			//Validation is done when deleting diseaseDefs to make sure they are not in use by FamilyHealths.
			textProblem.Text=DisDefCur.Description;
			textSnomed.Text=DisDefCur.CodeSnomed;
			textName.Text=FamilyHealthCur.PersonName;
		}

		private void butPick_Click(object sender,EventArgs e) {
			FormProblemDefinitions FormD=new FormProblemDefinitions();
			FormD.IsSelectionMode=true;
			FormD.ShowDialog();
			if(FormD.DialogResult!=DialogResult.OK) {
				return;
			}
			//the list should only ever contain one item.
			ProblemDefinition disDef=FormD.SelectedProblemDefinitions[0];
			if(disDef.CodeSnomed=="") {
				MessageBox.Show("Selection must have a SNOMED CT code associated");
				return;
			}
			textProblem.Text=disDef.Description;
			textSnomed.Text=disDef.CodeSnomed;
			DisDefCur=disDef;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(FamilyHealthCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Delete?")) {
				return;
			}
			FamilyHealths.Delete(FamilyHealthCur.FamilyHealthNum);
			SecurityLogs.MakeLogEntry(Permissions.PatFamilyHealthEdit,FamilyHealthCur.PatNum,FamilyHealthCur.PersonName+" "+FamilyHealthCur.Relationship+" deleted");
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(listRelationship.SelectedIndex<0) {
				MessageBox.Show("Relationship required.");
				return;
			}
			if(textName.Text.Trim()=="") {
				MessageBox.Show("Name required.");
				return;
			}
			if(DisDefCur==null) {
				MessageBox.Show("Problem required.");
				return;
			}
			FamilyHealthCur.DiseaseDefNum=DisDefCur.Id;
			FamilyHealthCur.Relationship=(FamilyRelationship)listRelationship.SelectedIndex;
			FamilyHealthCur.PersonName=textName.Text;
			if(FamilyHealthCur.IsNew) {
				SecurityLogs.MakeLogEntry(Permissions.PatFamilyHealthEdit,FamilyHealthCur.PatNum,FamilyHealthCur.PersonName+" "+FamilyHealthCur.Relationship+" added");
				FamilyHealths.Insert(FamilyHealthCur);
			}
			else {
				FamilyHealths.Update(FamilyHealthCur);
				SecurityLogs.MakeLogEntry(Permissions.PatFamilyHealthEdit,FamilyHealthCur.PatNum,FamilyHealthCur.PersonName+" "+FamilyHealthCur.Relationship+" edited");
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}