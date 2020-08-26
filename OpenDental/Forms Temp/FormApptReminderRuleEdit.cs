using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;
using System.Globalization;

namespace OpenDental {
	///<summary>In-memory form. Changes are not saved to the DB from this form.</summary>
	public partial class FormApptReminderRuleEdit:ODForm {
		///<summary>The default appointment reminder rule</summary>
		public ApptReminderRule ApptReminderRuleCur;
		private List<CommType> _sendOrder;
		private List<ApptReminderRule> _listRulesClinic;
		/// <summary>Public so it can be passed back to the parent form. The list of new language rules that were added from this window. </summary>
		public List<ApptReminderRule> ListNonDefaultRulesAdded=new List<ApptReminderRule>();
		///<summary>A list to keep track of the loosely associated rules that get removed in this form so they can get sync'd properly.</summary>
		public List<ApptReminderRule> ListNonDefaultRulesRemoving=new List<ApptReminderRule>();
		///<summary>The control that handles the message templates for the default language.</summary>
		private UserControlReminderMessage _defaultControl;

		///<summary>True if any preferences were updated.</summary>
		public bool IsPrefsChanged {
			get;
			private set;
		}

		private string[] _arrLanguages {
			get {
				return Prefs.GetString(PrefName.LanguagesUsedByPatients).Split(',');
			}
		}

		///<summary>A list that holds associated language rules for this rule.</summary>
		public List<ApptReminderRule> ListLanguageRules {
			get {
				return _listRulesClinic.FindAll(x => x.TSPrior==ApptReminderRuleCur.TSPrior
					&& x.ClinicNum==ApptReminderRuleCur.ClinicNum
					&& x.TypeCur==ApptReminderRuleCur.TypeCur
					&& x.Language!="");
			}
		}

		private bool _doesDuplicateExist {
			get {
				//ApptReminderRuleCur will always be a duplicate with no language. If another rule exists with no language then we will be making a duplicate.
				int countDuplicates=_listRulesClinic.FindAll(x => x.TSPrior==_tsPriorFromUI 
					&& x.ClinicNum==ApptReminderRuleCur.ClinicNum
					&& x.TypeCur==ApptReminderRuleCur.TypeCur
					&& x.ApptReminderRuleNum!=ApptReminderRuleCur.ApptReminderRuleNum 
					&& x.Language=="").Count;
				if(countDuplicates>0) {
					return true;
				}
				return false;
			}
		}

		private TimeSpan _tsPriorFromUI {
			get {
				if(radioBeforeAppt.Checked && ApptReminderRuleCur.TypeCur!=ApptReminderType.ScheduleThankYou) {
					return new TimeSpan(PIn.Int(textDays.Text,false),PIn.Int(textHours.Text,false),0,0);
				}
				else {
					//ScheduleThankYous and "after appointment" PatientPortalInvites
					return new TimeSpan(-PIn.Int(textDays.Text,false),-PIn.Int(textHours.Text,false),0,0);
				}
			}
		}

		public FormApptReminderRuleEdit(ApptReminderRule apptReminderCur,List<ApptReminderRule> listRulesClinic=null) {
			InitializeComponent();
			
			ApptReminderRuleCur=apptReminderCur;
			_listRulesClinic=listRulesClinic??new List<ApptReminderRule>();
		}

		private void FormApptReminderRuleEdit_Load(object sender,EventArgs e) {
			switch(ApptReminderRuleCur.TypeCur) {
				case ApptReminderType.Reminder:
					Text="Edit eReminder Rule";
					break;
				case ApptReminderType.ConfirmationFutureDay:
					Text="Edit eConfirmation Rule";
					break;
				case ApptReminderType.PatientPortalInvite:
					Text="Edit Patient Portal Invite Rule";
					break;
				case ApptReminderType.ScheduleThankYou:
					Text="Edit Automated Thank-You Rule";
					break;
				case ApptReminderType.Arrival:
					Text="Edit Arrival Rule";
					break;
				default:
					Text="Edit Rule";
					break;
			}
			checkEnabled.Checked=ApptReminderRuleCur.IsEnabled;
			checkEConfirmationAutoReplies.Checked=ApptReminderRuleCur.IsAutoReplyEnabled;
			labelRuleType.Text=ApptReminderRuleCur.TypeCur.GetDescription();
			labelTags.Text="Use the following replacement tags to customize messages : "
				+string.Join(", ",ApptReminderRules.GetAvailableTags(ApptReminderRuleCur.TypeCur));
			if(ApptReminderRuleCur.TypeCur.In(ApptReminderType.PatientPortalInvite,ApptReminderType.Arrival)) {
				checkSendAll.Visible=false;
			}
			if(!ApptReminderRuleCur.TypeCur.In(ApptReminderType.ConfirmationFutureDay,ApptReminderType.Arrival)) {
				checkEConfirmationAutoReplies.Visible=false;
			}
			if(_arrLanguages.Length==0 
				|| (_arrLanguages.Length==1 && _arrLanguages[0].ToLower().Trim()==Prefs.GetString(PrefName.LanguagesIndicateNone).ToLower().Trim())) 
			{
				butLanguage.Visible=false;
			}
			if(ListLanguageRules.Count==0) {
				butRemove.Visible=false;
			}
			_sendOrder=ApptReminderRuleCur.SendOrder.Split(',').Select(x => (CommType)PIn.Int(x)).ToList();
			FillGridPriority();
			FillTimeSpan();
			FillTabs();
			checkSendAll.Checked=ApptReminderRuleCur.IsSendAll;
			textHours.errorProvider1.SetIconAlignment(textHours,ErrorIconAlignment.MiddleLeft);
			textDays.errorProvider1.SetIconAlignment(textDays,ErrorIconAlignment.MiddleLeft);
		}
		
		private void FillTabs() {
			_defaultControl=new UserControlReminderMessage(ApptReminderRuleCur);
			_defaultControl.Anchor=((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom);
			if(butLanguage.Visible==false && ListLanguageRules.Count==0) {
				//not using languages and no reminders have been added for this default. 
				tabControl.Visible=false;
				Controls.Add(_defaultControl);
				_defaultControl.Location=new Point(18,264);
				return;
			}
			//using languages
			_defaultControl.Dock=DockStyle.Fill;
			tabPageDefault.Tag=ApptReminderRuleCur;
			tabPageDefault.Controls.Add(_defaultControl);
			foreach(ApptReminderRule additionalRule in ListLanguageRules) {
				TabPage languageTab=new TabPage();
				CultureInfo culture=MiscUtils.GetCultureFromThreeLetter(additionalRule.Language);
				if(culture==null) {
					languageTab.Text=additionalRule.Language;
				}
				else {
					languageTab.Text=culture.DisplayName;
				}
				languageTab.Tag=additionalRule;
				UserControlReminderMessage languageControl=new UserControlReminderMessage(additionalRule);
				languageControl.Anchor=_defaultControl.Anchor;
				languageControl.Dock=DockStyle.Fill;
				languageTab.Controls.Add(languageControl);
				tabControl.TabPages.Add(languageTab);
			}
		}

		private void FillTimeSpan() {
			textHours.Text=Math.Abs(ApptReminderRuleCur.TSPrior.Hours).ToString();//Hours, not total hours.
			textDays.Text=Math.Abs(ApptReminderRuleCur.TSPrior.Days).ToString();//Days, not total Days.
			if(ApptReminderRuleCur.TSPrior>=TimeSpan.Zero) {
				radioBeforeAppt.Checked=true;
			}
			else {
				radioAfterAppt.Checked=true;
			}
			if(ApptReminderRuleCur.TypeCur!=ApptReminderType.PatientPortalInvite) {
				radioBeforeAppt.Visible=false;
				radioAfterAppt.Visible=false;
			}
			if(ApptReminderRuleCur.TypeCur==ApptReminderType.ScheduleThankYou) {
				groupSendTime.Text="Send Time - after appointment scheduled.";
			}
			if(ApptReminderRuleCur.DoNotSendWithin.Days > 0) {
				textDaysWithin.Text=ApptReminderRuleCur.DoNotSendWithin.Days.ToString();
			}
			if(ApptReminderRuleCur.DoNotSendWithin.Hours > 0) {
				textHoursWithin.Text=ApptReminderRuleCur.DoNotSendWithin.Hours.ToString();
			}
			UpdateDoNotSendWithinLabel();
		}

		private void FillGridPriority() {
			gridPriorities.BeginUpdate();
			gridPriorities.ListGridColumns.Clear();
			gridPriorities.ListGridColumns.Add(new GridColumn("",50){ IsWidthDynamic=true });
			gridPriorities.ListGridRows.Clear();
			for(int i = 0;i<_sendOrder.Count;i++) {
				CommType typeCur = _sendOrder[i];
				GridRow gridRow;
				if(typeCur==CommType.Preferred) {
					if(checkSendAll.Checked) {
						//"Preferred" is irrelevant when SendAll is checked.
						continue;
					}
					gridRow=new GridRow();
					gridRow.Cells.Add("Preferred Confirm Method");
					gridPriorities.ListGridRows.Add(gridRow);
					continue;
				}
				if(typeCur==CommType.Text && !SmsPhones.IsIntegratedTextingEnabled()) {
					gridRow=new GridRow();
					gridRow.Cells.Add(typeCur.ToString()+" ("+"Not Configured"+")");
					gridRow.BackColor=Color.LightGray;
					gridPriorities.ListGridRows.Add(gridRow);
				}
				else {
					gridRow=new GridRow();
					gridRow.Cells.Add(typeCur.ToString());
					gridPriorities.ListGridRows.Add(gridRow);
				}
			}
			gridPriorities.EndUpdate();
		}

		private void radioBeforeAfterAppt_CheckedChanged(object sender,EventArgs e) {
			bool enabled=radioBeforeAppt.Checked || ApptReminderRuleCur.TypeCur==ApptReminderType.ScheduleThankYou;
			labelDoNotSendWithin.Enabled=enabled;
			labelDaysWithin.Enabled=enabled;
			labelHoursWithin.Enabled=enabled;
			textDaysWithin.Enabled=enabled;
			textHoursWithin.Enabled=enabled;
		}

		private void textDoNotSendWithin_TextChanged(object sender,EventArgs e) {
			UpdateDoNotSendWithinLabel();
		}

		private void UpdateDoNotSendWithinLabel() {
			string daysHoursTxt="";
			int daysWithin=PIn.Int(textDaysWithin.Text,false);
			int hoursWithin=PIn.Int(textHoursWithin.Text,false);
			if(!textDaysWithin.IsValid || !textHoursWithin.IsValid
				|| (daysWithin==0 && hoursWithin==0)) 
			{
				daysHoursTxt="_____________";
			}
			else {
				if(daysWithin==1) {
					daysHoursTxt+=daysWithin+" "+"day";
				}
				else if(daysWithin > 1) {
					daysHoursTxt+=daysWithin+" "+"days";
				}
				if(daysWithin > 0 && hoursWithin > 0) {
					daysHoursTxt+=" ";
				}
				if(hoursWithin==1) {
					daysHoursTxt+=hoursWithin+" "+"hour";
				}
				else if(hoursWithin > 1) {
					daysHoursTxt+=hoursWithin+" "+"hours";
				}
			}
			labelDoNotSendWithin.Text="Do not send within"+" "+daysHoursTxt+" "+"of appointment";
		}

		private void butUp_Click(object sender,EventArgs e) {
			int idx = gridPriorities.GetSelectedIndex();
			if(idx<1) {
				//-1 if nothing selected. 0 if top item selected.
				return;
			}
			_sendOrder.Reverse(idx-1,2);
			FillGridPriority();
			gridPriorities.SetSelected(idx-1,true);
		}

		private void butDown_Click(object sender,EventArgs e) {
			int idx = gridPriorities.GetSelectedIndex();
			if(idx==-1 || idx==_sendOrder.Count-1) {
				//-1 nothing selected. Count-1 if last item selected.
				return;
			}
			_sendOrder.Reverse(idx,2);
			FillGridPriority();
			gridPriorities.SetSelected(idx+1,true);
		}

		private void butAdvanced_Click(object sender,EventArgs e) {
			List<ApptReminderRule> listRulesOldAndNew=new List<ApptReminderRule>();
			listRulesOldAndNew.AddRange(ListLanguageRules);
			listRulesOldAndNew.AddRange(ListNonDefaultRulesAdded);
			string selectedLanguage="";
			if(tabControl.TabPages.Count > 1) {
				selectedLanguage=((ApptReminderRule)tabControl.SelectedTab.Tag).Language;
			}
			FormApptReminderRuleAggEdit formAddEdit=new FormApptReminderRuleAggEdit(ApptReminderRuleCur,listRulesOldAndNew,selectedLanguage);
			formAddEdit.ShowDialog();
			if(formAddEdit.DialogResult==DialogResult.Cancel) {
				//since we don't make a deep copy of the ApptReminderRuleCur in the advanced tap, this is the simple (lazy) way to undo any changes we've made
				DialogResult=DialogResult.Cancel; 
			}
		}

		///<summary>Removes 'Do not send eConfirmations' from the confirmed status for 'eConfirm Sent' if multiple eConfirmations are set up.</summary>
		private void CheckMultipleEConfirms() {
			int countEConfirm=_listRulesClinic?.Count(x => x.TypeCur==ApptReminderType.ConfirmationFutureDay && x.Language=="")??0;
			string confStatusEConfirmSent=Defs.GetDef(DefCat.ApptConfirmed,Prefs.GetLong(PrefName.ApptEConfirmStatusSent)).ItemName;
			List<string> listExclude=Prefs.GetString(PrefName.ApptConfirmExcludeESend)
				.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries).ToList();
			if(ApptReminderRuleCur.TypeCur==ApptReminderType.ConfirmationFutureDay
				//And there is more than 1 eConfirmation rule.
				&& (countEConfirm > 1 || (countEConfirm==1 && ApptReminderRuleCur.ApptReminderRuleNum==0))
				//And the confirmed status for 'eConfirm Sent' is marked 'Do not send eConfirmations'
				&& listExclude.Contains(Prefs.GetString(PrefName.ApptEConfirmStatusSent))
				//Ask them to fix their exclude send statuses
				&& MessageBox.Show("Appointments will not receive multiple eConfirmations if the '"+confStatusEConfirmSent+"' "+
						"status is set as 'Don't Send'. Would you like to remove 'Don't Send' from that status?",
					"",MessageBoxButtons.YesNo)==DialogResult.Yes) 
			{
				listExclude.RemoveAll(x => x==Prefs.GetString(PrefName.ApptEConfirmStatusSent));
				IsPrefsChanged|=Prefs.Set(PrefName.ApptConfirmExcludeESend,string.Join(",",listExclude));
			}
		}

		private void butLanguage_Click(object sender,EventArgs e) {
			List<string> listLanguagesDisplay=new List<string>();//contains the user friendly name ex: "Spanish"
			List<string> listLanguagesData=new List<string>();//contains the db friendly string ex: "SPA"
			List<ApptReminderRule> listCurrentControls=new List<ApptReminderRule>();
			foreach(TabPage page in tabControl.TabPages) {
				listCurrentControls.Add((ApptReminderRule)page.Tag);
			}
			foreach(string language in _arrLanguages) {
				if(language.IsNullOrEmpty() || language.ToLower().Trim()==Prefs.GetString(PrefName.LanguagesIndicateNone).ToLower().Trim()) {
					continue;
				}
				if(language.In(listCurrentControls.Select(x => x.Language).ToList()))	{
					continue;//we already have a tab for this langauge. 
				}
				CultureInfo culture=MiscUtils.GetCultureFromThreeLetter(language);
				if(culture==null) {
					listLanguagesDisplay.Add(language);//custom language - display what the user entered
				}
				else {
					listLanguagesDisplay.Add(culture.DisplayName);//display full name of the abbreviation
				}
				listLanguagesData.Add(language);
			}
			if(listLanguagesDisplay.Count==0) {
				MessageBox.Show("No additional languages available.");
				return;
			}
			InputBox languageSelect=new InputBox("Select language for template: ",listLanguagesDisplay,0);
			languageSelect.ShowDialog();
			if(languageSelect.DialogResult!=DialogResult.OK) {
				return;
			}
			ApptReminderRule rule=ApptReminderRuleCur.DeepCopy<ApptReminderRule,ApptReminderRule>();
			rule.ApptReminderRuleNum=0;
			rule.Language=listLanguagesData[languageSelect.SelectedIndex];
			TabPage tabLang=new TabPage();
			tabLang.Tag=rule;
			tabLang.Text=listLanguagesDisplay[languageSelect.SelectedIndex];
			UserControlReminderMessage ruleControl=new UserControlReminderMessage(rule);
			ruleControl.Anchor=tabPageDefault.Controls[0].Anchor;
			ruleControl.Dock=DockStyle.Fill;
			tabLang.Controls.Add(ruleControl);
			tabControl.TabPages.Add(tabLang);
			tabControl.SelectedTab=tabLang;
			ListNonDefaultRulesAdded.Add(rule);
			butRemove.Visible=true;
		}

		private void butRemove_Click(object sender,EventArgs e) {
			//Don't delete the default.
			if(((ApptReminderRule)tabControl.SelectedTab.Tag).Language=="") {
				MessageBox.Show("Cannot remove the default template.");
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Delete the currently selected language?")) {
				return;
			}
			ApptReminderRule ruleRemoving=(ApptReminderRule)tabControl.SelectedTab.Tag;
			if(!ListNonDefaultRulesAdded.Contains(ruleRemoving)) {
				ListNonDefaultRulesRemoving.Add(ruleRemoving);
			}
			ListNonDefaultRulesAdded.Remove(ruleRemoving);
			tabControl.TabPages.Remove(tabControl.SelectedTab);
		}

		private void butOk_Click(object sender,EventArgs e) {
			if(!textHours.IsValid	|| !textDays.IsValid || !textHoursWithin.IsValid || !textDaysWithin.IsValid) {
				MessageBox.Show("Fix data entry errors first.");
				return;
			}
			if(!ValidateRule()) {
				return;
			}
			if(_listRulesClinic.Any(x => x.TypeCur!=ApptReminderRuleCur.TypeCur && x.TSPrior==_tsPriorFromUI && x.IsEnabled)
				&& !MsgBox.Show(MsgBoxButtons.OKCancel,"There are multiple rules for sending at this send time. Are you sure you want to send multiple "
				+"messages at the same time?")) 
			{
				return;
			}
			if(_doesDuplicateExist) {
				MessageBox.Show("Not allowed to create a duplicate rule for the same send time.");
				return;
			}
			CheckMultipleEConfirms();
			ApptReminderRuleCur.SendOrder=string.Join(",",_sendOrder.Select(x => ((int)x).ToString()).ToArray());
			ApptReminderRuleCur.IsSendAll=checkSendAll.Checked;
			ApptReminderRuleCur.TSPrior=_tsPriorFromUI;
			if(radioBeforeAppt.Checked || ApptReminderRuleCur.TypeCur==ApptReminderType.ScheduleThankYou) {
				ApptReminderRuleCur.DoNotSendWithin=new TimeSpan(PIn.Int(textDaysWithin.Text,false),PIn.Int(textHoursWithin.Text,false),0,0);
			}
			ApptReminderRuleCur.IsEnabled=checkEnabled.Checked;
			ApptReminderRuleCur.IsAutoReplyEnabled=checkEConfirmationAutoReplies.Checked;
			_defaultControl.SaveControlTemplates();
			if(tabControl.TabPages.Count>1) {//handle additional language rules
				foreach(TabPage page in tabControl.TabPages) {
					UserControlReminderMessage reminderControl=(UserControlReminderMessage)page.Controls[0];//update the additional languages to match
					if(reminderControl.Rule.Language=="") {
						continue;//this is the default, which we already took care of.
					}
					reminderControl.Rule.TSPrior=ApptReminderRuleCur.TSPrior;
					reminderControl.Rule.IsAutoReplyEnabled=ApptReminderRuleCur.IsAutoReplyEnabled;
					reminderControl.Rule.IsEnabled=ApptReminderRuleCur.IsEnabled;
					reminderControl.Rule.SendOrder=ApptReminderRuleCur.SendOrder;
					reminderControl.Rule.IsSendAll=ApptReminderRuleCur.IsSendAll;
					reminderControl.Rule.DoNotSendWithin=ApptReminderRuleCur.DoNotSendWithin;
					reminderControl.SaveControlTemplates();
				}
			}
			DialogResult=DialogResult.OK;
		}

		private bool ValidateRule() {
			if(!checkEnabled.Checked) {
				return true;
			}
			List<string> errors = new List<string>();
			if(butLanguage.Visible==false) {
				errors.AddRange(Controls.OfType<UserControlReminderMessage>().First().ValidateTemplates());
			}
			else{ 
				foreach(TabPage page in tabControl.TabPages) {
					errors.AddRange(((UserControlReminderMessage)page.Controls[0]).ValidateTemplates());
				}
			}
			if(PIn.Int(textDays.Text,false)>366) {
				errors.Add("Lead time must 365 days or less.");
			}
			if(checkEnabled.Checked && PIn.Int(textHours.Text,false)==0 
				&& PIn.Int(textDays.Text,false)==0 
				&& ApptReminderRuleCur.TypeCur!=ApptReminderType.ScheduleThankYou) //ScheduleThankYou can be 0, meaning send immediately.
			{
				errors.Add("Lead time must be greater than 0 hours.");
			}
			if(ApptReminderRuleCur.TypeCur==ApptReminderType.ConfirmationFutureDay) {
				if(PIn.Int(textDays.Text,false)==0) {
					errors.Add("Lead time must 1 day or more for confirmations.");
				}
			}
			if(radioBeforeAppt.Checked) {
				TimeSpan tsPrior=new TimeSpan(PIn.Int(textDays.Text,false),PIn.Int(textHours.Text,false),0,0);
				TimeSpan doNotSendWithin=new TimeSpan(PIn.Int(textDaysWithin.Text,false),PIn.Int(textHoursWithin.Text,false),0,0);
				if(doNotSendWithin >= tsPrior && ApptReminderRuleCur.TypeCur!=ApptReminderType.ScheduleThankYou) {
					errors.Add("'Send Time' must be greater than 'Do Not Send Within' time.");
				}
			}
			if(errors.Count>0) {
				MessageBox.Show("You must fix the following errors before continuing."+"\r\n\r\n-"+string.Join("\r\n-",errors));
				return false;
			}
			return true;
		}

		private void checkSendAll_CheckedChanged(object sender,EventArgs e) {
			butUp.Enabled=!checkSendAll.Checked;
			butDown.Enabled=!checkSendAll.Checked;
			gridPriorities.Enabled=!checkSendAll.Checked;
			gridPriorities.SetSelected(false);
			FillGridPriority();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(ApptReminderRuleCur.IsAutoReplyEnabled 
				&& ConfirmationRequests.GetPendingForRule(ApptReminderRuleCur.ApptReminderRuleNum).Count > 0
				&& !MsgBox.Show(MsgBoxButtons.OKCancel,"Outstanding confirmation text messages associated to this appointment rule were found.  " +
					"Auto reply text messages will no longer be sent.  Continue?")) 
			{
				return;
			}
			foreach(ApptReminderRule dbRule in ListLanguageRules) {
				if(!ListNonDefaultRulesRemoving.Contains(dbRule) && !ListNonDefaultRulesAdded.Contains(dbRule)) {
					ListNonDefaultRulesRemoving.Add(dbRule);
				}
			}
			ListNonDefaultRulesAdded.Clear();
			ApptReminderRuleCur=null;
			DialogResult=DialogResult.OK;
		}

	}
}
