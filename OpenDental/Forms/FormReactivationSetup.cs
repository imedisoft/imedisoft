using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using CodeBase;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;

namespace OpenDental {
///<summary></summary>
	public partial class FormReactivationSetup : ODForm {

		///<summary></summary>
		public FormReactivationSetup(){
			InitializeComponent();
			Lan.F(this);
		}

		public void FormReactivationSetup_Load(object sender, System.EventArgs e) {
			checkGroupFamilies.Checked = PrefC.GetBool(PrefName.ReactivationGroupByFamily);
			textPostcardsPerSheet.Text=PrefC.GetLong(PrefName.ReactivationPostcardsPerSheet).ToString();
			textDaysPast.Text=PrefC.GetLongHideNegOne(PrefName.ReactivationDaysPast);
			List<Def> listRecallUnschedStatusDefs=Defs.GetDefsForCategory(DefCat.RecallUnschedStatus,true);
			comboStatusMailedReactivation.Items.AddDefs(listRecallUnschedStatusDefs);
			comboStatusMailedReactivation.SetSelectedDefNum(PrefC.GetLong(PrefName.ReactivationStatusMailed));
			comboStatusEmailedReactivation.Items.AddDefs(listRecallUnschedStatusDefs);
			comboStatusEmailedReactivation.SetSelectedDefNum(PrefC.GetLong(PrefName.ReactivationStatusEmailed));
			comboStatusTextedReactivation.Items.AddDefs(listRecallUnschedStatusDefs);
			comboStatusTextedReactivation.SetSelectedDefNum(PrefC.GetLong(PrefName.ReactivationStatusTexted));
			comboStatusEmailTextReactivation.Items.AddDefs(listRecallUnschedStatusDefs);
			comboStatusEmailTextReactivation.SetSelectedDefNum(PrefC.GetLong(PrefName.ReactivationStatusEmailedTexted));
			textDaysContactInterval.Text=PrefC.GetLongHideNegOne(PrefName.ReactivationContactInterval,useZero:true);
			textMaxReminders.Text=PrefC.GetLongHideNegOne(PrefName.ReactivationCountContactMax);
			FillGrid();
		}

		private void FillGrid(){
			string availableFields="[NameFL], [NameF], [ClinicName], [ClinicPhone], [PracticeName], [PracticePhone], [OfficePhone]";
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableReactivationMsgs","Mode"),61);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("",300);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableReactivationMsgs","Message"),500);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			#region 1st Reminder
			//ReactivationEmailSubject
			GridRow gridRow=new GridRow();
			gridRow.Cells.Add(Lan.g(this,"E-mail"));
			gridRow.Cells.Add(Lan.g(this,"Subject line"));
			gridRow.Cells.Add(PrefC.GetString(PrefName.ReactivationEmailSubject));
			gridRow.Tag=PrefName.ReactivationEmailSubject;
			gridMain.ListGridRows.Add(gridRow);
			//ReactivationEmailMessage
			gridRow=new GridRow();
			gridRow.Cells.Add(Lan.g(this,"E-mail"));
			gridRow.Cells.Add(Lan.g(this,"Available variables")+": "+availableFields);
			gridRow.Cells.Add(PrefC.GetString(PrefName.ReactivationEmailMessage));
			gridRow.Tag=PrefName.ReactivationEmailMessage;
			gridMain.ListGridRows.Add(gridRow);
			//ReactivationEmailFamMsg
			gridRow=new GridRow();
			gridRow.Cells.Add(Lan.g(this,"E-mail"));
			gridRow.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList] where the list of family members should show."));
			gridRow.Cells.Add(PrefC.GetString(PrefName.ReactivationEmailFamMsg));
			gridRow.Tag=PrefName.ReactivationEmailFamMsg;
			gridMain.ListGridRows.Add(gridRow);
			//ReactivationPostcardMessage
			gridRow=new GridRow();
			gridRow.Cells.Add(Lan.g(this,"Postcard"));
			gridRow.Cells.Add(Lan.g(this,"Available variables")+": "+availableFields);
			gridRow.Cells.Add(PrefC.GetString(PrefName.ReactivationPostcardMessage));
			gridRow.Tag=PrefName.ReactivationPostcardMessage;
			gridMain.ListGridRows.Add(gridRow);
			//ReactivationPostcardFamMsg
			gridRow=new GridRow();
			gridRow.Cells.Add(Lan.g(this,"Postcard"));
			gridRow.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList] where the list of family members should show."));
			gridRow.Cells.Add(PrefC.GetString(PrefName.ReactivationPostcardFamMsg));
			gridRow.Tag=PrefName.ReactivationPostcardFamMsg;
			gridMain.ListGridRows.Add(gridRow);
			#endregion
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PrefName prefName=gridMain.SelectedTag<PrefName>();
			FormRecallMessageEdit FormR = new FormRecallMessageEdit(prefName);
			FormR.MessageVal=PrefC.GetString(prefName);
			FormR.ShowDialog();
			if(FormR.DialogResult!=DialogResult.OK) {
				return;
			}
			if(Prefs.UpdateString(prefName,FormR.MessageVal)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			FillGrid();
		}		

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textDaysPast.errorProvider1.GetError(textDaysPast)!=""
				|| textMaxReminders.errorProvider1.GetError(textMaxReminders)!="") 
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textPostcardsPerSheet.Text!="1"
				&& textPostcardsPerSheet.Text!="3"
				&& textPostcardsPerSheet.Text!="4") 
			{
				MsgBox.Show(this,"The value in postcards per sheet must be 1, 3, or 4");
				return;
			}
			if(comboStatusMailedReactivation.SelectedIndex==-1
				|| comboStatusEmailedReactivation.SelectedIndex==-1
				|| comboStatusTextedReactivation.SelectedIndex==-1
				|| comboStatusEmailTextReactivation.SelectedIndex==-1) 
			{
				MsgBox.Show(this,"All status options on the left must be set.");
				return;
			}
			//End of Validation
			bool didChange=false;
			didChange |= Prefs.UpdateString(PrefName.ReactivationPostcardsPerSheet,textPostcardsPerSheet.Text);
			if(didChange) {
				if(textPostcardsPerSheet.Text=="1") {
					MsgBox.Show(this,"If using 1 postcard per sheet, you must adjust the position, and also the preview will not work");
				}
			}
			didChange |= Prefs.UpdateBool(PrefName.ReactivationGroupByFamily,checkGroupFamilies.Checked);
			didChange |= Prefs.UpdateLongAsNegOne(PrefName.ReactivationDaysPast,textDaysPast.Text);
			didChange |= Prefs.UpdateLongAsNegOne(PrefName.ReactivationContactInterval,textDaysContactInterval.Text);
			didChange |= Prefs.UpdateLongAsNegOne(PrefName.ReactivationCountContactMax,textMaxReminders.Text);
			//combo boxes These have already been checked for -1
			didChange |= Prefs.UpdateLong(PrefName.ReactivationStatusEmailed,comboStatusEmailedReactivation.GetSelected<Def>().DefNum);
			didChange |= Prefs.UpdateLong(PrefName.ReactivationStatusMailed,comboStatusMailedReactivation.GetSelected<Def>().DefNum);
			didChange |= Prefs.UpdateLong(PrefName.ReactivationStatusTexted,comboStatusTextedReactivation.GetSelected<Def>().DefNum);
			didChange |= Prefs.UpdateLong(PrefName.ReactivationStatusEmailedTexted,comboStatusEmailTextReactivation.GetSelected<Def>().DefNum);
			if(didChange) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}

}
