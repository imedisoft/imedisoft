using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	///<summary>Was never finished/implemented.</summary>
	public partial class FormScreenSetup:ODForm {
		//List<SheetDef> listSheets;

		public FormScreenSetup() {
			InitializeComponent();
			
		}

		private void FormScreenSetup_Load(object sender,EventArgs e) {
			/*checkUsePat.Checked=Prefs.GetBool(PrefName.PublicHealthScreeningUsePat);
			listSheets=SheetDefs.GetCustomForType(SheetTypeEnum.ExamSheet);
			for(int i=0;i<listSheets.Count;i++) {
				comboExamSheets.Items.Add(listSheets[i].Description);
				if(Prefs.GetLong(PrefName.PublicHealthScreeningSheet)==listSheets[i].SheetDefNum) {
					comboExamSheets.SelectedIndex=i;
				}
			}*/
		}

		private void checkUsePat_CheckedChanged(object sender,EventArgs e) {
			/*if(checkUsePat.Checked) {
				comboExamSheets.Enabled=true;
			}
			else {
				comboExamSheets.Enabled=false;
			}*/
		}

		private void butOK_Click(object sender,EventArgs e) {
			/*Prefs.UpdateBool(PrefName.PublicHealthScreeningUsePat,checkUsePat.Checked);
			if(comboExamSheets.SelectedIndex!=-1) {
				Prefs.Set(PrefName.PublicHealthScreeningSheet,listSheets[comboExamSheets.SelectedIndex].SheetDefNum);
			}*/
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}