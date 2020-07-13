using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormImageCatMerge:ODForm {
		private long _defNumInto;
		private long _defNumFrom;

		public FormImageCatMerge() {
			InitializeComponent();
			Lan.F(this);
		}

		private void buttonChangeInto_Click(object sender,EventArgs e) {
			FormDefinitionPicker FormDefinitionPicker=new FormDefinitionPicker(DefCat.ImageCats);
			if(FormDefinitionPicker.ShowDialog()==DialogResult.OK && FormDefinitionPicker.ListSelectedDefs.Count>0) {
				textBoxInto.Text=FormDefinitionPicker.ListSelectedDefs.FirstOrDefault().ItemName;
				_defNumInto=FormDefinitionPicker.ListSelectedDefs.FirstOrDefault().DefNum;
				CheckUIState();
			}
		}

		private void buttonChangeFrom_Click(object sender,EventArgs e) {
			FormDefinitionPicker FormDefinitionPicker=new FormDefinitionPicker(DefCat.ImageCats);
			if(FormDefinitionPicker.ShowDialog()==DialogResult.OK && FormDefinitionPicker.ListSelectedDefs.Count>0) {
				textBoxFrom.Text=FormDefinitionPicker.ListSelectedDefs.FirstOrDefault().ItemName;
				_defNumFrom=FormDefinitionPicker.ListSelectedDefs.FirstOrDefault().DefNum;
				CheckUIState();
			}
		}
	
		//Double check the state and status of the form.
		private void CheckUIState() {
			butMerge.Enabled=(textBoxFrom.Text.Trim()!="" && textBoxInto.Text.Trim()!="");
		}

		private void butMerge_Click(object sender,EventArgs e) {
			if(_defNumInto==_defNumFrom) { 
				MessageBox.Show("Cannot merge the same Image Category. Please update either the merge Into field or the merge From field.");
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Are you sure? The results are permanent and cannot be undone.")) {
				return;
			}
			try {
				Defs.MergeImageCatDefNums(_defNumFrom,_defNumInto);
			}
			catch(Exception ex) {
				FriendlyException.Show(Lans.g(this,"Image Categories failed to merge."),ex);
				return;
			}
			Defs.HideDef(Defs.GetDef(DefCat.ImageCats,_defNumFrom));
			DataValid.SetInvalid(InvalidType.Defs);
			MessageBox.Show("Image Categories merged successfully.");
			string logText=Lan.G(this,"Image Category Merge from")
				+" "+Defs.GetName(DefCat.ImageCats,_defNumFrom)+" "+Lan.G(this,"to")+" "+Defs.GetName(DefCat.ImageCats,_defNumInto);
			//Make log entry here.
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,logText);
			textBoxFrom.Clear();
			textBoxInto.Clear();
			CheckUIState();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}
	}
}