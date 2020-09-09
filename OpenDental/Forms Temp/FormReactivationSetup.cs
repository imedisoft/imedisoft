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
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental {
///<summary></summary>
	public partial class FormReactivationSetup : ODForm {

		///<summary></summary>
		public FormReactivationSetup(){
			InitializeComponent();
			
		}

		public void FormReactivationSetup_Load(object sender, System.EventArgs e) {
			checkGroupFamilies.Checked = Prefs.GetBool(PrefName.ReactivationGroupByFamily);
			textPostcardsPerSheet.Text=Prefs.GetLong(PrefName.ReactivationPostcardsPerSheet).ToString();
			textDaysPast.Text=Prefs.GetLongHideNegOne(PrefName.ReactivationDaysPast);
			List<Definition> listRecallUnschedStatusDefs=Definitions.GetDefsForCategory(DefinitionCategory.RecallUnschedStatus,true);
			comboStatusMailedReactivation.Items.AddDefs(listRecallUnschedStatusDefs);
			comboStatusMailedReactivation.SetSelectedDefNum(Prefs.GetLong(PrefName.ReactivationStatusMailed));
			comboStatusEmailedReactivation.Items.AddDefs(listRecallUnschedStatusDefs);
			comboStatusEmailedReactivation.SetSelectedDefNum(Prefs.GetLong(PrefName.ReactivationStatusEmailed));
			comboStatusTextedReactivation.Items.AddDefs(listRecallUnschedStatusDefs);
			comboStatusTextedReactivation.SetSelectedDefNum(Prefs.GetLong(PrefName.ReactivationStatusTexted));
			comboStatusEmailTextReactivation.Items.AddDefs(listRecallUnschedStatusDefs);
			comboStatusEmailTextReactivation.SetSelectedDefNum(Prefs.GetLong(PrefName.ReactivationStatusEmailedTexted));
			textDaysContactInterval.Text=Prefs.GetLongHideNegOne(PrefName.ReactivationContactInterval);
			textMaxReminders.Text=Prefs.GetLongHideNegOne(PrefName.ReactivationCountContactMax);
			FillGrid();
		}

		private void FillGrid(){
			string availableFields="[NameFL], [NameF], [ClinicName], [ClinicPhone], [PracticeName], [PracticePhone], [OfficePhone]";
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col=new GridColumn("Mode",61);
			gridMain.Columns.Add(col);
			col=new GridColumn("",300);
			gridMain.Columns.Add(col);
			col=new GridColumn("Message",500);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			#region 1st Reminder
			//ReactivationEmailSubject
			GridRow gridRow=new GridRow();
			gridRow.Cells.Add("E-mail");
			gridRow.Cells.Add("Subject line");
			gridRow.Cells.Add(Prefs.GetString(PrefName.ReactivationEmailSubject));
			gridRow.Tag=PrefName.ReactivationEmailSubject;
			gridMain.Rows.Add(gridRow);
			//ReactivationEmailMessage
			gridRow=new GridRow();
			gridRow.Cells.Add("E-mail");
			gridRow.Cells.Add("Available variables"+": "+availableFields);
			gridRow.Cells.Add(Prefs.GetString(PrefName.ReactivationEmailMessage));
			gridRow.Tag=PrefName.ReactivationEmailMessage;
			gridMain.Rows.Add(gridRow);
			//ReactivationEmailFamMsg
			gridRow=new GridRow();
			gridRow.Cells.Add("E-mail");
			gridRow.Cells.Add("For multiple patients in one family.  Use [FamilyList] where the list of family members should show.");
			gridRow.Cells.Add(Prefs.GetString(PrefName.ReactivationEmailFamMsg));
			gridRow.Tag=PrefName.ReactivationEmailFamMsg;
			gridMain.Rows.Add(gridRow);
			//ReactivationPostcardMessage
			gridRow=new GridRow();
			gridRow.Cells.Add("Postcard");
			gridRow.Cells.Add("Available variables"+": "+availableFields);
			gridRow.Cells.Add(Prefs.GetString(PrefName.ReactivationPostcardMessage));
			gridRow.Tag=PrefName.ReactivationPostcardMessage;
			gridMain.Rows.Add(gridRow);
			//ReactivationPostcardFamMsg
			gridRow=new GridRow();
			gridRow.Cells.Add("Postcard");
			gridRow.Cells.Add("For multiple patients in one family.  Use [FamilyList] where the list of family members should show.");
			gridRow.Cells.Add(Prefs.GetString(PrefName.ReactivationPostcardFamMsg));
			gridRow.Tag=PrefName.ReactivationPostcardFamMsg;
			gridMain.Rows.Add(gridRow);
			#endregion
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			string prefName=gridMain.SelectedTag<string>();
			FormRecallMessageEdit FormR = new FormRecallMessageEdit(prefName);
			FormR.MessageVal=Prefs.GetString(prefName);
			FormR.ShowDialog();
			if(FormR.DialogResult!=DialogResult.OK) {
				return;
			}
			if(Prefs.Set(prefName,FormR.MessageVal)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			FillGrid();
		}		

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textDaysPast.errorProvider1.GetError(textDaysPast)!=""
				|| textMaxReminders.errorProvider1.GetError(textMaxReminders)!="") 
			{
				MessageBox.Show("Please fix data entry errors first.");
				return;
			}
			if(textPostcardsPerSheet.Text!="1"
				&& textPostcardsPerSheet.Text!="3"
				&& textPostcardsPerSheet.Text!="4") 
			{
				MessageBox.Show("The value in postcards per sheet must be 1, 3, or 4");
				return;
			}
			if(comboStatusMailedReactivation.SelectedIndex==-1
				|| comboStatusEmailedReactivation.SelectedIndex==-1
				|| comboStatusTextedReactivation.SelectedIndex==-1
				|| comboStatusEmailTextReactivation.SelectedIndex==-1) 
			{
				MessageBox.Show("All status options on the left must be set.");
				return;
			}
			//End of Validation
			bool didChange=false;
			didChange |= Prefs.Set(PrefName.ReactivationPostcardsPerSheet,textPostcardsPerSheet.Text);
			if(didChange) {
				if(textPostcardsPerSheet.Text=="1") {
					MessageBox.Show("If using 1 postcard per sheet, you must adjust the position, and also the preview will not work");
				}
			}
			didChange |= Prefs.Set(PrefName.ReactivationGroupByFamily,checkGroupFamilies.Checked);
			didChange |= Prefs.Set(PrefName.ReactivationDaysPast,textDaysPast.Text);
			didChange |= Prefs.Set(PrefName.ReactivationContactInterval,textDaysContactInterval.Text);
			didChange |= Prefs.Set(PrefName.ReactivationCountContactMax,textMaxReminders.Text);
			//combo boxes These have already been checked for -1
			didChange |= Prefs.Set(PrefName.ReactivationStatusEmailed,comboStatusEmailedReactivation.GetSelected<Definition>().Id);
			didChange |= Prefs.Set(PrefName.ReactivationStatusMailed,comboStatusMailedReactivation.GetSelected<Definition>().Id);
			didChange |= Prefs.Set(PrefName.ReactivationStatusTexted,comboStatusTextedReactivation.GetSelected<Definition>().Id);
			didChange |= Prefs.Set(PrefName.ReactivationStatusEmailedTexted,comboStatusEmailTextReactivation.GetSelected<Definition>().Id);
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
