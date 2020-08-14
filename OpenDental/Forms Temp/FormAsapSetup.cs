using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormAsapSetup:ODForm {
		public FormAsapSetup() {
			InitializeComponent();
			
		}

		private void FormAsapSetup_Load(object sender,EventArgs e) {
			comboClinic.SelectedClinicNum=Clinics.ClinicNum;
			FillPrefs();
		}

		private void FillPrefs() {
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col;
			col=new GridColumn("Type",100);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("",250);//Random tidbits regarding the template
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("Template",500);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			checkUseDefaults.Checked=true;
			string baseVars="Available variables:"+" [NameF], [Date], [Time], [OfficeName], [OfficePhone]";
			GridRow row;
			row=BuildRowForTemplate(PrefName.ASAPTextTemplate,"Text manual",baseVars);
			gridMain.ListGridRows.Add(row);
			row=BuildRowForTemplate(PrefName.WebSchedAsapTextTemplate,"Web Sched Text",baseVars+", [AsapURL]");
			gridMain.ListGridRows.Add(row);
			row=BuildRowForTemplate(PrefName.WebSchedAsapEmailTemplate,"Web Sched Email Body",baseVars+", [AsapURL]");
			gridMain.ListGridRows.Add(row);
			row=BuildRowForTemplate(PrefName.WebSchedAsapEmailSubj,"Web Sched Email Subject",baseVars);
			gridMain.ListGridRows.Add(row);
			gridMain.EndUpdate();
			if(comboClinic.SelectedClinicNum==0) {
				textWebSchedPerDay.Text=Prefs.GetString(PrefName.WebSchedAsapTextLimit);
				checkUseDefaults.Checked=false;
			}
			else {
				var clinicPref=ClinicPrefs.GetString(comboClinic.SelectedClinicNum,PrefName.WebSchedAsapTextLimit);
				if(string.IsNullOrEmpty(clinicPref)) {
					textWebSchedPerDay.Text=Prefs.GetString(PrefName.WebSchedAsapTextLimit);
				}
				else {
					textWebSchedPerDay.Text=clinicPref;
					checkUseDefaults.Checked=false;
				}
			}
		}

		///<summary>Creates a row for the passed in template type. Unchecks checkUseDefaults if a clinic-level template is in use.</summary>
		private GridRow BuildRowForTemplate(string prefName,string templateName,string availableVars) {
			string templateText;
			bool doShowDefault=false;
			if(comboClinic.SelectedClinicNum==0) {
				templateText=Prefs.GetString(prefName);
				checkUseDefaults.Checked=false;
			}
			else {
				var clinicPref=ClinicPrefs.GetString(comboClinic.SelectedClinicNum, prefName);
				if(string.IsNullOrEmpty(clinicPref)) {
					templateText=Prefs.GetString(prefName);
					doShowDefault=true;
				}
				else {
					templateText=clinicPref;
					checkUseDefaults.Checked=false;
				}
			}
			GridRow row=new GridRow();
			row.Cells.Add(templateName+(doShowDefault ? " "+"(Default)" : ""));
			row.Cells.Add(availableVars);
			row.Cells.Add(templateText);
			row.Tag=prefName;
			return row;
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboClinic.SelectedClinicNum==0) {
				checkUseDefaults.Visible=false;
			}
			else {
				checkUseDefaults.Visible=true;
			}
			FillPrefs();
		}

		private void checkUseDefaults_Click(object sender,EventArgs e) {
			//List<string> listPrefs=new List<string> {
			//	PrefName.ASAPTextTemplate,
			//	PrefName.WebSchedAsapTextTemplate,
			//	PrefName.WebSchedAsapEmailTemplate,
			//	PrefName.WebSchedAsapEmailSubj,
			//	PrefName.WebSchedAsapTextLimit,
			//};
			//if(checkUseDefaults.Checked) {
			//	if(MsgBox.Show(MsgBoxButtons.YesNo,"Delete custom templates for this clinic and switch to using defaults? This cannot be undone.")) {
			//		ClinicPrefs.DeletePrefs(comboClinic.SelectedClinicNum,listPrefs);
			//		DataValid.SetInvalid(InvalidType.ClinicPrefs);
			//	}
			//	else {
			//		checkUseDefaults.Checked=false;
			//	}
			//}
			//else {//Was checked, now user is unchecking it.
			//	bool wasChanged=false;
			//	foreach(string pref in listPrefs) {
			//		if(ClinicPrefs.Upsert(pref,comboClinic.SelectedClinicNum,Prefs.GetString(pref))) {
			//			wasChanged=true;
			//		}
			//	}
			//	if(wasChanged) {
			//		DataValid.SetInvalid(InvalidType.ClinicPrefs);
			//	}
			//}
			//FillPrefs();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//string prefName =(string)gridMain.ListGridRows[e.Row].Tag;
			//string curPrefValue=GetClinicPrefValue(prefName);
			//EmailType emailType=PIn.Enum<EmailType>(GetClinicPrefValue(PrefName.WebSchedAsapEmailTemplateType));
			//string newPrefValue;
			//bool isHtmlTemplate=prefName==PrefName.WebSchedAsapEmailTemplate;
			//if(isHtmlTemplate) {
			//	FormEmailEdit formEmailEdit=new FormEmailEdit {
			//		MarkupText=curPrefValue,
			//		DoCheckForDisclaimer=true,
			//		IsRawAllowed=true,
			//		IsRaw=emailType==EmailType.RawHtml,
			//	};
			//	formEmailEdit.ShowDialog();
			//	if(formEmailEdit.DialogResult!=DialogResult.OK) {
			//		return;
			//	}
			//	emailType=formEmailEdit.IsRaw?EmailType.RawHtml:EmailType.Html;
			//	newPrefValue=formEmailEdit.MarkupText;
			//}
			//else {
			//	FormRecallMessageEdit FormR=new FormRecallMessageEdit(prefName);
			//	FormR.MessageVal=curPrefValue;
			//	FormR.ShowDialog();
			//	if(FormR.DialogResult!=DialogResult.OK) {
			//		return;
			//	}
			//	newPrefValue=FormR.MessageVal;
			//}
			//if(comboClinic.SelectedClinicNum==0) {
			//	if(Prefs.Set(prefName,newPrefValue) | Prefs.Set(PrefName.WebSchedAsapEmailTemplateType,(int)emailType)) {
			//		DataValid.SetInvalid(InvalidType.Prefs);
			//	}
			//}
			//else {
			//	if(ClinicPrefs.Upsert(prefName,comboClinic.SelectedClinicNum,newPrefValue) 
			//		| ClinicPrefs.Upsert(PrefName.WebSchedAsapEmailTemplate,comboClinic.SelectedClinicNum,((int)emailType).ToString())) 
			//	{
			//		DataValid.SetInvalid(InvalidType.ClinicPrefs);
			//	}
			//}
			//FillPrefs();
		}

		private string GetClinicPrefValue(string prefName) {
			if(comboClinic.SelectedClinicNum==0) {
				return Prefs.GetString(prefName);
			}
			//ClinicPref clinicPref=ClinicPrefs.GetPref(prefName,comboClinic.SelectedClinicNum);
			//if(clinicPref==null || string.IsNullOrEmpty(clinicPref.ValueString)) {
			//	return Prefs.GetString(prefName);
			//}
			//return clinicPref.ValueString;
			return "";
		}

		private void textWebSchedPerDay_Leave(object sender,EventArgs e) {
			//if(!textWebSchedPerDay.IsValid) {
			//	return;
			//}
			//if(comboClinic.SelectedClinicNum==0) {
			//	if(Prefs.Set(PrefName.WebSchedAsapTextLimit,textWebSchedPerDay.Text)) {
			//		DataValid.SetInvalid(InvalidType.Prefs);
			//	}
			//}
			//else {
			//	if(ClinicPrefs.Upsert(PrefName.WebSchedAsapTextLimit,comboClinic.SelectedClinicNum,textWebSchedPerDay.Text)) {
			//		DataValid.SetInvalid(InvalidType.ClinicPrefs);
			//	}
			//}
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private class ComboClinicItem {
			public string DisplayName;
			public long ClinicNum;
			public ComboClinicItem(string displayName,long clinicNum) {
				DisplayName=displayName;
				ClinicNum=clinicNum;
			}
			public override string ToString() {
				return DisplayName;
			}
		}

	}
}