using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System.IO;
using System.Net.Mail;

namespace OpenDental {
	public partial class FormMassEmail:ODForm {
		private List<PatientInfo> _listPatients=new List<PatientInfo>();
		private List<PatientInfo> _listPatientsSelected=new List<PatientInfo>();
		private List<EmailHostingTemplate> _listTemplates;
		///<summary>Selected template. Viewable here only. User needs to make edits in a different form.</summary>
		private EmailHostingTemplate _templateCur;
		private bool _isLoading;

		///<summary>The lower bound of the analytics date range.</summary>
		private DateTime _dateTimeAnalyticsFrom {
			get { return dateRangeAnalytics.GetDateTimeFrom(); }
			set { dateRangeAnalytics.SetDateTimeFrom(value); }
		}

		///<summary>The upper bound of the analytics date range.</summary>
		private DateTime _dateTimeAnalyticsTo {
			get { return dateRangeAnalytics.GetDateTimeTo(); }
			set { dateRangeAnalytics.SetDateTimeTo(value); }
		}

		public FormMassEmail() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormMassEmail_Load(object sender,EventArgs e) {
			_isLoading=true;
			MassEmailStatus massEmailStatus=PIn.Enum<MassEmailStatus>(ClinicPrefs.GetInt(Clinics.ClinicNum, PrefName.MassEmailStatus));
			bool onHQClinic=PrefC.HasClinicsEnabled && Clinics.ClinicNum==0;
			if(
				//Pref has never been set.
				massEmailStatus==MassEmailStatus.NotActivated ||
				//Pref may have been set but HQ has ability to sync out the Activated flag.
				!massEmailStatus.HasFlag(MassEmailStatus.Activated) ||
				//This clinic has disabled the pref locally. They may be activated but they have shut it off for now.
				!massEmailStatus.HasFlag(MassEmailStatus.Enabled) ||
				//Never allow HQ clinic to send mass email.
				onHQClinic) 
			{
				DisableAllExcept(labelPleaseActivate,butClose);
				if(onHQClinic) {
					labelPleaseActivate.Text="Mass emails cannot be used with the HQ clinic. Please switch selected clinic.";
				}
				labelPleaseActivate.Visible=true;
				return;
			}
			FillFilters();
			RefreshPatientTab();
			FillGridTemplates();
			SelectAndLoadFirstTemplate();
			InitializeAnalyticsTab();
			FillGridAnalytics();
			_isLoading=false;
		}

		#region Tab Patients
		private void RefreshPatientTab() {
			RefreshPatients();
			FillGridAvailable();
			FillGridSelected();
			labelRefreshNeeded.Visible=false;
		}

		private void FillGridAvailable() {
			gridAvailablePatients.BeginUpdate();
			gridAvailablePatients.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lans.g(gridAvailablePatients.TranslationName,"Name"),140,GridSortingStrategy.StringCompare);
			gridAvailablePatients.ListGridColumns.Add(col);
			col=new GridColumn(Lans.g(gridAvailablePatients.TranslationName,"Birthdate"),80,HorizontalAlignment.Center,GridSortingStrategy.DateParse);
			gridAvailablePatients.ListGridColumns.Add(col);
			col=new GridColumn(Lans.g(gridAvailablePatients.TranslationName,"Email"),140,GridSortingStrategy.StringCompare);
			gridAvailablePatients.ListGridColumns.Add(col);
			col=new GridColumn(Lans.g(gridAvailablePatients.TranslationName,"Contact Method"),60,GridSortingStrategy.StringCompare);
			gridAvailablePatients.ListGridColumns.Add(col);
			col=new GridColumn(Lans.g(gridAvailablePatients.TranslationName,"Status"),70,GridSortingStrategy.StringCompare);
			gridAvailablePatients.ListGridColumns.Add(col);
			col=new GridColumn(Lans.g(gridAvailablePatients.TranslationName,"Last Appointment"),85,HorizontalAlignment.Center,
				GridSortingStrategy.DateParse);
			gridAvailablePatients.ListGridColumns.Add(col);
			col=new GridColumn(Lans.g(gridAvailablePatients.TranslationName,"Next Appointment"),80,HorizontalAlignment.Center,
				GridSortingStrategy.DateParse);
			gridAvailablePatients.ListGridColumns.Add(col);
			gridAvailablePatients.ListGridRows.Clear();
			GridRow row;
			foreach(PatientInfo patient in _listPatients) {
				row=new GridRow();
				row.Cells.Add(patient.Name);
				row.Cells.Add(patient.Birthdate.ToShortDateString());
				row.Cells.Add(patient.Email);
				row.Cells.Add(patient.ContactMethod.GetDescription());
				row.Cells.Add(patient.Status.GetDescription());
				if(patient.DateTimeLastAppt==DateTime.MinValue) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(patient.DateTimeLastAppt.ToShortDateString());
				}
				if(patient.DateTimeNextAppt==DateTime.MinValue) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(patient.DateTimeNextAppt.ToShortDateString());
				}
				row.Tag=patient;
				gridAvailablePatients.ListGridRows.Add(row);
			}
			gridAvailablePatients.EndUpdate();
		}

		private void FillGridSelected() {
			gridSelectedPatients.BeginUpdate();
			gridSelectedPatients.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lans.g(gridSelectedPatients.TranslationName,"Name"),150,GridSortingStrategy.StringCompare);
			gridSelectedPatients.ListGridColumns.Add(col);
			col=new GridColumn(Lans.g(gridSelectedPatients.TranslationName,"Birthdate"),80,HorizontalAlignment.Center,GridSortingStrategy.DateParse);
			gridSelectedPatients.ListGridColumns.Add(col);
			col=new GridColumn(Lans.g(gridSelectedPatients.TranslationName,"Email"),80,GridSortingStrategy.StringCompare);
			gridSelectedPatients.ListGridColumns.Add(col);
			gridSelectedPatients.ListGridRows.Clear();
			GridRow row;
			foreach(PatientInfo patient in _listPatientsSelected) {
				row=new GridRow();
				row.Cells.Add(patient.Name);
				row.Cells.Add(patient.Birthdate.ToShortDateString());
				row.Cells.Add(patient.Email);
				row.Tag=patient;
				gridSelectedPatients.ListGridRows.Add(row);
			}
			gridSelectedPatients.EndUpdate();
			labelNumberPats.Text=gridSelectedPatients.ListGridRows.Count.ToString();
			labelNumberPats.ForeColor=_listPatientsSelected.Count>0 ? Color.LimeGreen : Color.Firebrick;
		}

		private void FillFilters() {
			//patient status list box
			listBoxPatStatus.Items.Clear();
			foreach(PatientStatus status in Enum.GetValues(typeof(PatientStatus))) {
				if(status==PatientStatus.Deceased || status==PatientStatus.Deleted) {
					continue;
				}
				listBoxPatStatus.Items.Add(new ODBoxItem<PatientStatus>(status.GetDescription(),status));
			}
			listBoxPatStatus.SelectedIndex=0;
			//preferred contact method list box
			listBoxContactMethod.Items.Clear();
			listBoxContactMethod.Items.Add(new ODBoxItem<int>(Lans.g(this,"Any"),-1));
			listBoxContactMethod.Items.Add(new ODBoxItem<int>(Lans.g(this,ContactMethod.Email.GetDescription()),(int)ContactMethod.Email));
			listBoxContactMethod.Items.Add(new ODBoxItem<int>(Lans.g(this,ContactMethod.None.GetDescription()),(int)ContactMethod.None));
			listBoxContactMethod.SelectedIndex=0;
			//Age Range
			textAgeFrom.Text="1";
			textAgeTo.Text="110";
			//patient billing type list box
			listBoxPatBillingType.Items.Clear();
			listBoxPatBillingType.Items.Add(new ODBoxItem<Def>(Lan.G(this,"All"),new Def()));
			foreach(Def billingType in Defs.GetDefsForCategory(DefCat.BillingTypes)) {
				listBoxPatBillingType.Items.Add(new ODBoxItem<Def>(billingType.ItemName,billingType));
			}
			listBoxPatBillingType.SelectedIndex=0;
			//NotSeenSince and SeenSince datePicker and checkBox 
			datePickerNotSeenSince.SetDateTime(DateTime.Now.AddYears(-3));
			checkHideSeenSince.Checked=false;//Should be defaulted to unchecked on load.
			datePickerSeenSince.SetDateTime(DateTime.Now.AddYears(-3));
			checkHideNotSeenSince.Checked=true;
		}

		///<summary>Refreshed the patient data for the available patient's grid only. Uses filters to limit the data. If a patient already exists 
		///in the selected patients grid then they will not additionally be added here again so the user can't add them twice.</summary>
		private void RefreshPatients() {
			int ageFrom=PIn.Int(textAgeFrom.Text);
			int ageTo=PIn.Int(textAgeTo.Text);
			if(ageFrom > ageTo) {
				MessageBox.Show( "The 'From age' cannot be greater than the 'To age'.");
				return;
			}
			_listPatients=new List<PatientInfo>();
			List<PatientStatus> listSelectedPatStatus=listBoxPatStatus.GetListSelected<PatientStatus>();
			//int so "Any" can be selected. First() because only one item can be selected in this list box. 
			int contactMethod=listBoxContactMethod.GetListSelected<int>().First();
			int daysExcluding=checkExcludeWithin.Checked?PIn.Int(textNumDays.Text):-1;
			DataTable table=Patients.GetPatientsWithFirstLastAppointments(listSelectedPatStatus,checkHiddenFutureAppt.Checked
				,comboClinicPatient.SelectedClinicNum,ageFrom,ageTo,getSeenSinceDateTime(),getNotSeenSinceDateTime(),getPatBillingType(),contactMethod,daysExcluding);
			_listPatients=PatientInfo.GetListPatientInfos(table)
				.FindAll(x => !x.PatNum.In(_listPatientsSelected.Select(y => y.PatNum)
				.ToList()));
		}

		public DateTime getNotSeenSinceDateTime() {
			if(!checkHideNotSeenSince.Checked) {
				return DateTime.MinValue;
			}
			return datePickerNotSeenSince.GetDateTime().Date;
		}

		public DateTime getSeenSinceDateTime() {
			if(!checkHideSeenSince.Checked) {
				return DateTime.MinValue;
			}
			return datePickerSeenSince.GetDateTime().Date;
		}

		public List<Def> getPatBillingType() {
			//First load of the UI/Form, nothing can be selected, null allows us to ignore it for the query
			//In that case, treat it the same as "All" and return the full list
			if(listBoxPatBillingType.SelectedItem==null || listBoxPatBillingType.SelectedItem.ToString()=="All"){
				List<Def> listAllBillinTpes=new List<Def>();
				foreach(Def def in Defs.GetDefsForCategory(DefCat.BillingTypes)) {
					listAllBillinTpes.Add(def);
				}
				return listAllBillinTpes;
			}
			return listBoxPatBillingType.GetListSelected<Def>();
		}


		private void butRefreshPatientFilters_Click(object sender,EventArgs e) {
			RefreshPatientTab();
		}

		private void listBoxPatStatus_SelectedIndexChanged(object sender,EventArgs e) {
			labelRefreshNeeded.Visible=true;
		}

		private void listBoxContactMethod_SelectedIndexChanged(object sender,EventArgs e) {
			labelRefreshNeeded.Visible=true;
		}

		private void checkHiddenFutureAppt_Click(object sender,EventArgs e) {
			labelRefreshNeeded.Visible=true;
		}

		private void checkExcludeWithin_Click(object sender,EventArgs e) {
			labelRefreshNeeded.Visible=true;
		}

		private void checkHideNotSeenSince_Click(object sender,EventArgs e) {
			labelRefreshNeeded.Visible=true;
		}
		
		private void checkBoxHideSeenSince_Click(object sender,EventArgs e) {
			labelRefreshNeeded.Visible=true;
		}

		private void listBoxPatBillingType_SelectedIndexChanged(object sender,EventArgs e) {
			labelRefreshNeeded.Visible=true;
		}

		private void textNumDays_TextChanged(object sender,EventArgs e) {
			labelRefreshNeeded.Visible=true;
		}

		private void comboClinicPatient_SelectionChangeCommitted(object sender,EventArgs e) {
			labelRefreshNeeded.Visible=true;
		}

		private void textAgeFrom_TextChanged(object sender,EventArgs e) {
			if(!_isLoading) {
				labelRefreshNeeded.Visible=true;
			}
		}

		private void textAgeTo_TextChanged(object sender,EventArgs e) {
			if(!_isLoading) {
				labelRefreshNeeded.Visible=true;
			}
		}

		private void butMoveToSelected_Click(object sender,EventArgs e) {
			List<PatientInfo> listMoving=gridAvailablePatients.SelectedGridRows.Select(x => (PatientInfo)x.Tag).ToList();
			_listPatientsSelected.AddRange(listMoving);
			_listPatients.RemoveAll(x => x.PatNum.In(listMoving.Select(y => y.PatNum).ToList()));
			FillGridSelected();
			FillGridAvailable();
		}

		private void butMoveToAvailable_Click(object sender,EventArgs e) {
			List<PatientInfo> listMoving=gridSelectedPatients.SelectedGridRows.Select(x => (PatientInfo)x.Tag).ToList();
			_listPatients.AddRange(listMoving);
			_listPatientsSelected.RemoveAll(x => x.PatNum.In(listMoving.Select(y => y.PatNum).ToList()));
			FillGridSelected();
			FillGridAvailable();
		}

		private void butSelectAllAvailable_Click(object sender,EventArgs e) {
			gridAvailablePatients.SetSelected(true);
		}

		private void butSelectAllSelected_Click(object sender,EventArgs e) {
			gridSelectedPatients.SetSelected(true);
		}
		#endregion

		#region Tab Templates
		///<summary>Also refreshes data for the list of email hosting templates.</summary>
		private void FillGridTemplates() {
			gridTemplates.BeginUpdate();
			gridTemplates.ListGridColumns.Clear();
			_listTemplates=EmailHostingTemplates.Refresh();
			GridColumn col=new GridColumn(Lans.g(gridTemplates.TranslationName,"Saved Templates"),240);
			gridTemplates.ListGridColumns.Add(col);
			col=new GridColumn(Lans.g(gridTemplates.TranslationName,"Email Type"),35);
			gridTemplates.ListGridColumns.Add(col);
			gridTemplates.ListGridRows.Clear();
			GridRow row;
			foreach(EmailHostingTemplate template in _listTemplates) {
				row=new GridRow();
				row.Cells.Add(template.TemplateName);
				row.Cells.Add(Lans.g(this,template.EmailTemplateType.GetDescription()));
				row.Tag=template;
				gridTemplates.ListGridRows.Add(row);
			}
			gridTemplates.EndUpdate();
		}

		private void gridTemplates_CellClick(object sender,ODGridClickEventArgs e) {
			_templateCur=gridTemplates.SelectedTag<EmailHostingTemplate>();
			userControlEmailTemplate1.RefreshView(_templateCur.BodyPlainText,_templateCur.BodyHTML,_templateCur.EmailTemplateType);
		}

		private void gridTemplates_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormMassEmailTemplate formMassEmailTemplate=new FormMassEmailTemplate(_templateCur);
			if(formMassEmailTemplate.ShowDialog()!=DialogResult.OK) {
				return;
			}
			FillGridTemplates();
			userControlEmailTemplate1.RefreshView(_templateCur.BodyPlainText,_templateCur.BodyHTML,_templateCur.EmailTemplateType);
		}

		private void butDeleteTemplate_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Are you sure you want to delete the selected template? This cannot be undone.")) {
				return;
			}
			//Create an API instance with the clinic num for this template.
			IAccountApi api=EmailHostingTemplates.GetAccountApi(_templateCur.ClinicNum);
			try {
				api.DeleteTemplate(new DeleteTemplateRequest {
					TemplateNum=_templateCur.TemplateId,
				});
			}
			catch(Exception ex) {
				FriendlyException.Show("Failed to delete template. Please try again.",ex);
				return;
			}
			EmailHostingTemplates.Delete(_templateCur.EmailHostingTemplateNum);
			FillGridTemplates();
			SelectAndLoadFirstTemplate();
		}

		private void butCopy_Click(object sender,EventArgs e) {
			if(_templateCur==null) {
				MessageBox.Show("No valid template selected to copy from.");
				return;
			}
			EmailHostingTemplate copyTemplate=_templateCur.DeepCopy<EmailHostingTemplate,EmailHostingTemplate>();
			copyTemplate.TemplateName.Append('1');
			_templateCur=copyTemplate;
			_templateCur.IsNew=true;
			_templateCur.ClinicNum=Clinics.ClinicNum;
			FormMassEmailTemplate formMassEmailTemplate=new FormMassEmailTemplate(_templateCur);
			if(formMassEmailTemplate.ShowDialog()!=DialogResult.OK) {
				SelectAndLoadFirstTemplate();
				return;
			}
			FillGridTemplates();
			_templateCur=_listTemplates.FirstOrDefault(x => x.EmailHostingTemplateNum==formMassEmailTemplate.NewTemplateCurPriKey);
			if(_templateCur!=null) {
				gridTemplates.SetSelected(_listTemplates.IndexOf(_templateCur),true);
				userControlEmailTemplate1.RefreshView(_templateCur.BodyPlainText,_templateCur.BodyHTML,_templateCur.EmailTemplateType);
			}
			else {
				SelectAndLoadFirstTemplate();
			}
		}

		private void butNewTemplate_Click(object sender,EventArgs e) {
			_templateCur=new EmailHostingTemplate();
			_templateCur.IsNew=true;
			_templateCur.ClinicNum=Clinics.ClinicNum;
			FormMassEmailTemplate formMassEmailTemplate=new FormMassEmailTemplate(_templateCur);
			if(formMassEmailTemplate.ShowDialog()!=DialogResult.OK) {
				SelectAndLoadFirstTemplate();
				return;
			}
			FillGridTemplates();
			_templateCur=_listTemplates.FirstOrDefault(x => x.EmailHostingTemplateNum==formMassEmailTemplate.NewTemplateCurPriKey);
			if(_templateCur!=null) {
				gridTemplates.SetSelected(_listTemplates.IndexOf(_templateCur),true);
				userControlEmailTemplate1.RefreshView(_templateCur.BodyPlainText,_templateCur.BodyHTML,_templateCur.EmailTemplateType);
			}
			else {
				SelectAndLoadFirstTemplate();
			}
		}

		private void butEditTemplate_Click(object sender,EventArgs e) {
			FormMassEmailTemplate formMassEmailTemplate=new FormMassEmailTemplate(_templateCur);
			if(formMassEmailTemplate.ShowDialog()!=DialogResult.OK) {
				return;
			}
			FillGridTemplates();
			userControlEmailTemplate1.RefreshView(_templateCur.BodyPlainText,_templateCur.BodyHTML,_templateCur.EmailTemplateType);
		}

		private void SelectAndLoadFirstTemplate() {
			//usually called when the user cancels out of editing without saving changes for a new template.
			gridTemplates.SetSelected(0,true);
			_templateCur=gridTemplates.SelectedTag<EmailHostingTemplate>();
			userControlEmailTemplate1.RefreshView(_templateCur.BodyPlainText,_templateCur.BodyHTML,_templateCur.EmailTemplateType);
		}

		private void butImport_Click(object sender,EventArgs e) {
			#region Get Imported HTML 
			OpenFileDialog openFileDialog=new OpenFileDialog();
			openFileDialog.Multiselect=false;
			if(Directory.Exists(Prefs.GetString(PrefName.ExportPath))) {
				openFileDialog.InitialDirectory=Prefs.GetString(PrefName.ExportPath);
			}
			else if(Directory.Exists("C:\\")) {
				openFileDialog.InitialDirectory="C:\\";
			}
			if(openFileDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			string fileName=Path.GetFileName(openFileDialog.FileName);
			string path=openFileDialog.FileName;
			if(!File.Exists(path)) {
				MessageBox.Show("File does not exist or cannot be read.");
				return;
			}
			if(Path.GetExtension(fileName)!=".html") {
				MsgBox.Show("Not a valid selection. Only HTML files can be imported.");
				return;
			}
			string documentText="";
			try {
				documentText=File.ReadAllText(path);
			}
			catch(Exception ex) {
				FriendlyException.Show(ex.Message,ex);
			}
			#endregion
			#region Show Email Edit window for user to make any desired adjustments to HTML
			//note, users could uncheck 'raw' and turn this into a regular html email in this window, so we need to do some extra checking.
			_templateCur=new EmailHostingTemplate();
			_templateCur.IsNew=true;
			FormEmailEdit formEmailEdit=new FormEmailEdit();
			formEmailEdit.MarkupText=documentText;
			formEmailEdit.IsRawAllowed=true;
			formEmailEdit.IsRaw=true;
			formEmailEdit.AreReplacementsAllowed=true;
			if(formEmailEdit.ShowDialog()!=DialogResult.OK) {
				SelectAndLoadFirstTemplate();
				return;
			}
			_templateCur.BodyHTML=formEmailEdit.MarkupText;
			if(string.IsNullOrEmpty(_templateCur.BodyHTML)) {//user decided against making an html email. We may even want to break out here. 
				_templateCur.EmailTemplateType=EmailType.Regular;
			}
			else if(formEmailEdit.IsRaw) {
				_templateCur.EmailTemplateType=EmailType.RawHtml;
			}
			else {//user decided they wanted to use the master template.
				_templateCur.EmailTemplateType=EmailType.Html;//Our special wiki replcement that will turn into html.
			}
			#endregion
			#region User sets plain text
			//ALL emails need to have plain text. Will will attempt to convert, but user needs to verify. 
			FormMassEmailTemplate formMassEmailTemplate=new FormMassEmailTemplate(_templateCur);
			if(formMassEmailTemplate.ShowDialog()!=DialogResult.OK) {
				//user chose to not set plain text, this is no longer a valid template. 
				SelectAndLoadFirstTemplate();
				return;
			}
			FillGridTemplates();
			_templateCur=_listTemplates.FirstOrDefault(x => x.EmailHostingTemplateNum==formMassEmailTemplate.NewTemplateCurPriKey);
			if(_templateCur!=null) {
				gridTemplates.SetSelected(_listTemplates.IndexOf(_templateCur),true);
				userControlEmailTemplate1.RefreshView(_templateCur.BodyPlainText,_templateCur.BodyHTML,_templateCur.EmailTemplateType);
			}
			else {
				SelectAndLoadFirstTemplate();
			}
			#endregion
		}
		#endregion

		#region Tab Analytics

		///<summary>Called once. Initializes the analytics tab.</summary>
		private void InitializeAnalyticsTab() {
			//The default date time's do not get set until after Form.OnLoad. However, the first time we fill the analytics grid
			//is on load and we look at the date range's properties so we set both.
			dateRangeAnalytics.DefaultDateTimeFrom=DateTime.Now.AddMonths(-1).Date;
			dateRangeAnalytics.DefaultDateTimeTo=DateTime.Now.Date;
			_dateTimeAnalyticsFrom=dateRangeAnalytics.DefaultDateTimeFrom;
			_dateTimeAnalyticsTo=dateRangeAnalytics.DefaultDateTimeTo;
		}

		///<summary>Fills the main grid on the analytics tab. Hits the database everytime this method is called.</summary>
		private void FillGridAnalytics() {
			gridAnalytics.BeginUpdate();
			if(gridAnalytics.ListGridColumns.Count==0) {		
				GridColumn col=new GridColumn(Lans.g(gridAnalytics.TranslationName,"Name"),100,GridSortingStrategy.StringCompare);
				gridAnalytics.ListGridColumns.Add(col);
				col=new GridColumn(Lans.g(gridAnalytics.TranslationName,"Type"),100,GridSortingStrategy.StringCompare);
				gridAnalytics.ListGridColumns.Add(col);
				if(PrefC.HasClinicsEnabled) {
					col=new GridColumn(Lans.g(gridAnalytics.TranslationName,"Clinic"),100,GridSortingStrategy.StringCompare);
					gridAnalytics.ListGridColumns.Add(col);
				}
				col=new GridColumn(Lans.g(gridAnalytics.TranslationName,"Date Created"),100,GridSortingStrategy.DateParse);
				gridAnalytics.ListGridColumns.Add(col);
				col=new GridColumn(Lans.g(gridAnalytics.TranslationName,"Total"),85,HorizontalAlignment.Center);
				gridAnalytics.ListGridColumns.Add(col);
				col=new GridColumn(Lans.g(gridAnalytics.TranslationName,"Pending"),85,HorizontalAlignment.Center);
				gridAnalytics.ListGridColumns.Add(col);
				col=new GridColumn(Lans.g(gridAnalytics.TranslationName,"Unopened"),85,HorizontalAlignment.Center);
				gridAnalytics.ListGridColumns.Add(col);
				col=new GridColumn(Lans.g(gridAnalytics.TranslationName,"Opened"),85,HorizontalAlignment.Center);
				gridAnalytics.ListGridColumns.Add(col);
				col=new GridColumn(Lans.g(gridAnalytics.TranslationName,"Bounced"),85,HorizontalAlignment.Center);
				gridAnalytics.ListGridColumns.Add(col);
				col=new GridColumn(Lans.g(gridAnalytics.TranslationName,"Complaint"),85,HorizontalAlignment.Center);
				gridAnalytics.ListGridColumns.Add(col);
				col=new GridColumn(Lans.g(gridAnalytics.TranslationName,"Failed"),85,HorizontalAlignment.Center);
				gridAnalytics.ListGridColumns.Add(col);
				col=new GridColumn(Lans.g(gridAnalytics.TranslationName,"Unsubscribed"),85,HorizontalAlignment.Center);
				gridAnalytics.ListGridColumns.Add(col);
			}
			gridAnalytics.ListGridRows.Clear();
			long clinicNum=PrefC.HasClinicsEnabled ? comboClinicAnalytics.SelectedClinicNum : -1;
			List<PromotionAnalytic> listAnalytics=Promotions.GetAnalytics(_dateTimeAnalyticsFrom,_dateTimeAnalyticsTo,clinicNum)
				.OrderByDescending(x => x.Promotion.DateTimeCreated)
				.ThenByDescending(x => x.Promotion.PromotionNum)
				.ToList();
			string GetCountString(PromotionAnalytic analytic,int total,params PromotionLogStatus[] statuses) {
				int count=0;
				foreach(PromotionLogStatus status in statuses) {
					//If this is not in the dictionary, count will be 0.
					analytic.DictionaryCounts.TryGetValue(status,out int countStatus);
					count+=countStatus;
				}
				return $"{count} \r\n({((float)count/total*100).ToString("N")})%";
			}
			foreach(PromotionAnalytic analytic in listAnalytics) {
				GridRow row=new GridRow();
				row.Cells.Add(analytic.Promotion.PromotionName);
				row.Cells.Add(analytic.Promotion.TypePromotion.GetDescription());
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetDesc(analytic.Promotion.ClinicNum));
				}
				row.Cells.Add(analytic.Promotion.DateTimeCreated.ToShortDateString());
				//Combine all statuses together to get the total.
				int total=analytic.DictionaryCounts.Sum(x => x.Value);
				row.Cells.Add(total.ToString());
				row.Cells.Add(GetCountString(analytic,total,PromotionLogStatus.Pending));
				row.Cells.Add(GetCountString(analytic,total,PromotionLogStatus.Delivered));
				row.Cells.Add(GetCountString(analytic,total,PromotionLogStatus.Opened));
				row.Cells.Add(GetCountString(analytic,total,PromotionLogStatus.Bounced));
				row.Cells.Add(GetCountString(analytic,total,PromotionLogStatus.Complaint));
				row.Cells.Add(GetCountString(analytic,total,PromotionLogStatus.Failed));
				row.Cells.Add(GetCountString(analytic,total,PromotionLogStatus.Unsubscribed));
				gridAnalytics.ListGridRows.Add(row);
			}
			gridAnalytics.EndUpdate();
		}

		private void butRefreshAnalytics_Click(object sender,EventArgs e) {
			FillGridAnalytics();
		}

		#endregion

		private void butSendEmails_Click(object sender,EventArgs e) {
			//send the email with the selected template to each person that is in the selected patients grid.
			if(_listPatientsSelected.Count<=0) {
				MessageBox.Show("Patients must be selected before email can be sent.");
				return;
			}
			if(_templateCur.TemplateId==0) {
				string xhtml;//api templates must have the full html text, even if only partial html. Database templates will store partial as plain text. 
				xhtml=_templateCur.BodyHTML;
				if(_templateCur.EmailTemplateType==EmailType.Html && !string.IsNullOrEmpty(_templateCur.BodyHTML)) {
					//This might not work for images, we should consider blocking them or warning them about sending if we detect images
					try {
						xhtml=MarkupEdit.TranslateToXhtml(_templateCur.BodyHTML,true,false,true);
					}
					catch {
						if(!MsgBox.Show(MsgBoxButtons.YesNo,"There was an issue rendering your email.  If you use this template, you may send malformed emails to " +
							"every selected patient. Do you want to continue saving?")) 
						{
							return;
						}
					}
				}
				//most likely case for this scenario, someone is sending one of the templates that came in the convert, without modifying it.
				//Create an API instance with the clinic num for this template.
				IAccountApi api=EmailHostingTemplates.GetAccountApi(_templateCur.ClinicNum);
				try {
					CreateTemplateResponse response=api.CreateTemplate(new CreateTemplateRequest { 
						Template=new Template { 
							TemplateName=_templateCur.TemplateName,
							TemplateBodyHtml=xhtml,
							TemplateBodyPlainText=_templateCur.BodyPlainText,
							TemplateSubject=_templateCur.Subject,
						},
					});
					//This is how we can update the template later
					_templateCur.TemplateId=response.TemplateNum;
					if(_templateCur.EmailHostingTemplateNum==0) {//New (not expected, should only execute when it's an existing that hasn't been added to backend.)
						EmailHostingTemplates.Insert(_templateCur);
					}
					else {
						EmailHostingTemplates.Update(_templateCur);
					}
				}
				catch(Exception ex) {
					FriendlyException.Show("Failed to create email from template. Please try again.",ex);
					return;
				}
			}
			FormMassEmailSend formMassEmailSend=new FormMassEmailSend(_templateCur,_listPatientsSelected);
			if(formMassEmailSend.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_listPatientsSelected=new List<PatientInfo>();
			FillGridSelected();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		///<summary>Helper class to store data for the main object used in this form.</summary>
		public class PatientInfo {
			public long PatNum;
			public string Name;
			public DateTime Birthdate;
			public string Email;
			public ContactMethod ContactMethod;
			public PatientStatus Status;
			public long ClinicNum;
			//DateTime of patient's most recent appointment
			public DateTime DateTimeLastAppt;
			public long NextAptNum;
			//DateTime of patient's next scheduled appointment
			public DateTime DateTimeNextAppt;

			public static List<PatientInfo> GetListPatientInfos(DataTable table) {
				List<PatientInfo> listPatientInfos=new List<PatientInfo>();
				foreach(DataRow row in table.Rows) {
					PatientInfo patInfo=new PatientInfo();
					patInfo.PatNum=PIn.Long(row["PatNum"].ToString());
					patInfo.Name=row["LName"].ToString()+", "+row["FName"].ToString();
					patInfo.Birthdate=PIn.Date(row["Birthdate"].ToString());
					patInfo.Email=row["Email"].ToString();
					patInfo.Status=PIn.Enum<PatientStatus>(row["PatStatus"].ToString());
					patInfo.ContactMethod=PIn.Enum<ContactMethod>(row["PreferContactMethod"].ToString());
					patInfo.DateTimeLastAppt=PIn.Date(row["DateTimeLastApt"].ToString());
					patInfo.DateTimeNextAppt=PIn.Date(row["DateTimeNextApt"].ToString());
					patInfo.NextAptNum=PIn.Long(row["NextAptNum"].ToString(),false);
					patInfo.ClinicNum=PIn.Long(row["ClinicNum"].ToString(),false);
					listPatientInfos.Add(patInfo);
				}
				return listPatientInfos;
			}
			
		}
	}
}