using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormMedicationMerge:ODForm {
		private Medication _medFrom;
		private Medication _medInto;

		public FormMedicationMerge() {
			InitializeComponent();
			
		}

		private void CheckUIState() {
			butMerge.Enabled=(textMedNumFrom.Text.Trim()!="" && textMedNumInto.Text.Trim()!="");
		}
		
		private void butChangeMedInto_Click(object sender,EventArgs e) {
			FormMedications FormM=new FormMedications();
			FormM.IsSelectionMode=true;
			FormM.ShowDialog();
			if(FormM.DialogResult==DialogResult.OK) {
				_medInto=Medications.GetById(FormM.SelectedMedicationNum);
				textGenNumInto.Text=_medInto.GenericId?.ToString();
				textMedNameInto.Text=_medInto.Name;
				textMedNumInto.Text=POut.Long(_medInto.Id);
				textRxInto.Text=_medInto.RxCui;
			}
			CheckUIState();
		}

		private void butChangeMedFrom_Click(object sender,EventArgs e) {
			FormMedications FormM=new FormMedications();
			FormM.IsSelectionMode=true;
			FormM.ShowDialog();
			if(FormM.DialogResult==DialogResult.OK) {
				_medFrom=Medications.GetById(FormM.SelectedMedicationNum);
				textGenNumFrom.Text=_medFrom.GenericId?.ToString();
				textMedNameFrom.Text=_medFrom.Name;
				textMedNumFrom.Text=POut.Long(_medFrom.Id);
				textRxFrom.Text=_medFrom.RxCui;
			}
			CheckUIState();
		}

		private void butMerge_Click(object sender,EventArgs e) {
			string differentFields="";
			string msgText="";
			if(textMedNumInto.Text==textMedNumFrom.Text) {
				//do not attempt a merge if the same medication was selected twice, or if one of the fields is blank.
				MessageBox.Show("You must select two different medications to merge.");
				return;
			}
			if(_medFrom.Id==_medFrom.GenericId && _medInto.Id!=_medInto.GenericId) {
				msgText="You may not merge a generic medication into a brand"+".  "+
					"Select the generic version of the medication to merge into instead"+".";
				MessageBox.Show(msgText);
				return;
			}
			if(textMedNameFrom.Text!=textMedNameInto.Text) {
				differentFields+="\r\n"+"Medication Name";
			}
			if(textGenNumFrom.Text!=textGenNumInto.Text) {
				differentFields+="\r\n"+"GenericNum";
			}
			if(textRxFrom.Text!=textRxInto.Text) {
				differentFields+="\r\n"+"RxCui";
			}
			long numPats=Medications.CountPats(_medFrom.Id);
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Are you sure?  The results are permanent and cannot be undone.")) {
				return;
			}
			msgText="";
			if(differentFields!="") {
				msgText="The following medication fields do not match"+": "+differentFields+"\r\n";
			}
			msgText+="This change is irreversible"+".  "+"This medication is assigned to"+" "+numPats+" "
				+"patients"+".  "+"Continue anyways?";
			if(MessageBox.Show(msgText,"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				return;
			}
			long rowsChanged=Medications.Merge(_medFrom.Id,_medInto.Id);
			string logText="Medications merged"+": "+_medFrom.Name+" "+"merged into"+" "+_medInto.Name+".\r\n"
			+"Rows changed"+": "+POut.Long(rowsChanged);
			SecurityLogs.MakeLogEntry(Permissions.MedicationMerge,0,logText);
			textRxFrom.Clear();
			textMedNumFrom.Clear();
			textMedNameFrom.Clear();
			textGenNumFrom.Clear();
			MessageBox.Show("Done.");
			DataValid.SetInvalid(InvalidType.Medications);
			CheckUIState();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}
}