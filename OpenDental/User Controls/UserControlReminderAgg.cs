﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using Imedisoft.Data;
using OpenDentBusiness;

namespace OpenDental {
	public partial class UserControlReminderAgg:UserControl {
		private string _templateEmailAggShared;

		public UserControlReminderAgg(ApptReminderRule apptReminderDefault) {
			InitializeComponent();
			Rule=apptReminderDefault;
			LoadControls();
		}

		public ApptReminderRule Rule { get; }

		///<summary>These tags are not allowed in confirmation auto reply.</summary>
		private List<string> ListTagsExludedFromAutoReply {
			get {
				return new List<string>() { "[Appts]","[ConfirmCode]","[ConfirmURL]" };
			}
		}

		///<summary>These tags are not allowed in Arrival Response or Come In sms messages.</summary>
		private List<string> ListTagsExludedFromArrivalResponseComeIn => new List<string> { OpenDentBusiness.AutoComm.ArrivalsTagReplacer.ARRIVED_TAG };

		private void LoadControls() {
			textSMSAggShared.Text=PIn.String(Rule.TemplateSMSAggShared);
			textSMSAggPerAppt.Text=PIn.String(Rule.TemplateSMSAggPerAppt);
			textEmailSubjAggShared.Text=PIn.String(Rule.TemplateEmailSubjAggShared);
			_templateEmailAggShared=PIn.String(Rule.TemplateEmailAggShared);
			RefreshEmail();
			textEmailAggPerAppt.Text=PIn.String(Rule.TemplateEmailAggPerAppt);
			labelTags.Text=GetTagsAvailable();
			textSingleAutoReply.Text=PIn.String(Rule.TemplateAutoReply);
			textAggregateAutoReply.Text=PIn.String(Rule.TemplateAutoReplyAgg);
			textArrivalResponse.Text=PIn.String(Rule.TemplateAutoReply);
			textComeIn.Text=PIn.String(Rule.TemplateComeInMessage);
			if(Rule.TypeCur==ApptReminderType.PatientPortalInvite) {
				textSMSAggShared.Enabled=false;	
				textSMSAggPerAppt.Enabled=false;
				labelEmailAggPerAppt.Text+="  "+"Replaces the [Credentials] tag.";
			}
			if(Rule.TypeCur!=ApptReminderType.ConfirmationFutureDay) {
				tabTemplates.TabPages.Remove(tabAutoReplyTemplate);
			}
			if(Rule.TypeCur==ApptReminderType.Arrival) {
				for(int i = tabTemplates.TabPages.Count-1;i>=0;i--) {
					if(!tabTemplates.TabPages[i].In(tabSMSTemplate,tabArrivalTemplate)) {
						tabTemplates.TabPages.RemoveAt(i);
					}
				}
			}
			else {
				tabTemplates.TabPages.Remove(tabArrivalTemplate);
			}
		}

		private string GetTagsAvailable() {
			List<string> listTagsAvailable=ApptReminderRules.GetAvailableAggTags(Rule.TypeCur);
			if(tabTemplates.SelectedTab==tabAutoReplyTemplate) {
				listTagsAvailable.RemoveAll(x => x.In(ListTagsExludedFromAutoReply));
			}
			else if(tabTemplates.SelectedTab==tabArrivalTemplate) {
				listTagsAvailable.RemoveAll(x => x.In(ListTagsExludedFromArrivalResponseComeIn));
			}
			return "Use the following replacement tags to customize messages: "
				 +string.Join(", ",listTagsAvailable);
		}

		private void RefreshEmail() {
			if(Rule.EmailTemplateType==EmailType.RawHtml) {
				browserEmailBody.DocumentText=_templateEmailAggShared;
				return;//text is already in HTML, it does not need to be translated. 
			}
			try {
				string text=MarkupEdit.TranslateToXhtml(_templateEmailAggShared,isPreviewOnly:true,hasWikiPageTitles:false,isEmail:true);
				browserEmailBody.DocumentText=text;
			}
			catch {
			}
		}

		private void butEditEmail_Click(object sender,EventArgs e) {
			FormEmailEdit formEE=new FormEmailEdit();
			formEE.IsRawAllowed=true;
			formEE.MarkupText=_templateEmailAggShared;
			formEE.IsRaw=Rule.AggEmailTemplateType==EmailType.RawHtml;
			formEE.ShowDialog();
			if(formEE.DialogResult!=DialogResult.OK) {
				return;
			}
			Rule.AggEmailTemplateType=formEE.IsRaw?EmailType.RawHtml:EmailType.Html;
			_templateEmailAggShared=formEE.MarkupText;
			RefreshEmail();
		}

		private void tabTemplates_SelectedIndexChanged(object sender,EventArgs e) {
			labelTags.Text=GetTagsAvailable();
		}

		public List<string> ValidateTemplates() {
			List<string> errors=new List<string>();
			if(Rule.TypeCur==ApptReminderType.Arrival) {
				if(string.IsNullOrWhiteSpace(textSMSAggShared.Text)) {
					errors.Add(groupBoxSMSAggShared.Text+" cannot be blank.");
				}
				if(ListTagsExludedFromArrivalResponseComeIn.Any(x => x.ToLower().Trim().In(textArrivalResponse.Text.ToLower()))
					|| ListTagsExludedFromArrivalResponseComeIn.Any(x => x.ToLower().Trim().In(textComeIn.Text.ToLower()))) 
				{
					//Not allowed to use [Arrived] in Arrival Response or ComeIn messages.
					errors.Add(groupArrivedReply.Text+" and "+groupComeIn.Text
						+" cannot contain "+string.Join(",",ListTagsExludedFromArrivalResponseComeIn));
				}
				if(!textSMSAggShared.Text.ToLower().Contains(OpenDentBusiness.AutoComm.ArrivalsTagReplacer.ARRIVED_TAG.ToLower())) {
					errors.Add(groupBoxSMSAggShared.Text+$" must contain the \"{OpenDentBusiness.AutoComm.ArrivalsTagReplacer.ARRIVED_TAG}\" tag.");
				}
				return errors;//Arrival Response and ComeIn templates are allowed to be blank, so we can just return here.
			}
			if(Rule.TypeCur!=ApptReminderType.PatientPortalInvite) {
				if(string.IsNullOrWhiteSpace(textSMSAggShared.Text)) {
					errors.Add("Text message cannot be blank.");
				}
				if(!textSMSAggShared.Text.ToLower().Contains("[appts]")) {
					errors.Add("Text message must contain the \"[Appts]\" tag.");
				}
				if(!_templateEmailAggShared.ToLower().Contains("[appts]")) {
					errors.Add("Email message must contain the \"[Appts]\" tag.");
				}
			}
			if(string.IsNullOrWhiteSpace(textEmailSubjAggShared.Text)) {
				errors.Add("Email subject cannot be blank.");
			}
			if(string.IsNullOrWhiteSpace(_templateEmailAggShared)) {
				errors.Add("Email message cannot be blank.");
			}	
			if(Rule.TypeCur==ApptReminderType.ConfirmationFutureDay) {
				if(_templateEmailAggShared.ToLower().Contains("[confirmcode]")) {
					errors.Add("Confirmation emails should not contain the \"[ConfirmCode]\" tag.");
				}
				if(!_templateEmailAggShared.ToLower().Contains("[confirmurl]")) {
					errors.Add("Confirmation emails must contain the \"[ConfirmURL]\" tag.");
				}
				if(string.IsNullOrWhiteSpace(textSingleAutoReply.Text)) {
					errors.Add("Single auto reply text cannot be blank.");
				}
				if(string.IsNullOrWhiteSpace(textAggregateAutoReply.Text)) {
					errors.Add("Aggregate auto reply text cannot be blank.");
				}
				List<string> listInvalidTags=ListTagsExludedFromAutoReply.FindAll(x => textSingleAutoReply.Text.ToLower().Contains(x.ToLower()));
				if(listInvalidTags.Count>0) {
					errors.Add("Single auto reply text contains invalid tags:\r\n"+$"  {string.Join(", ",listInvalidTags)}");
				}
				listInvalidTags=ListTagsExludedFromAutoReply.FindAll(x => textAggregateAutoReply.Text.ToLower().Contains(x.ToLower()));
				if(listInvalidTags.Count>0) {
					errors.Add("Aggregate auto reply text contains invalid tags:\r\n"+$"  {string.Join(", ",listInvalidTags)}");
				}
			}
			if(Rule.TypeCur==ApptReminderType.ScheduleThankYou) {
				//ThankYou templates can only use the [AddToCalendar] tag when Confirmations are enabled.
				string addToCalTag=ApptThankYouSents.ADD_TO_CALENDAR.ToLower();
				if(!Preferences.GetBool(PreferenceName.ApptConfirmAutoSignedUp)) {
					if(textSMSAggPerAppt.Text.ToLower().Contains(addToCalTag)) {
						errors.Add("Automated Thank-You texts cannot contain "+ApptThankYouSents.ADD_TO_CALENDAR
							+" when not signed up for eConfirmations.");
					}
					if(textEmailAggPerAppt.Text.ToLower().Contains(addToCalTag)) {
						errors.Add("Automated Thank-You emails cannot contain "+ApptThankYouSents.ADD_TO_CALENDAR
							+" when not signed up for eConfirmations.");
					}
				}
				//Shared templates cannot use [AddToCalendar] tag.
				if(textSMSAggShared.Text.ToLower().Contains(addToCalTag)) {
					errors.Add("Automated Thank-You Aggregated SMS Template cannot contain "+ApptThankYouSents.ADD_TO_CALENDAR
						+". Use Per Appointment instead.");
				}
				if(_templateEmailAggShared.ToLower().Contains(addToCalTag)) {
					errors.Add("Automated Thank-You Aggregated E-mail Template cannot contain "+ApptThankYouSents.ADD_TO_CALENDAR
						+". Use Per Appointment instead.");
				}
			}
			if(Preferences.GetBool(PreferenceName.EmailDisclaimerIsOn) && !_templateEmailAggShared.ToLower().Contains("[emaildisclaimer]")) {
				errors.Add("Email must contain the \"[EmailDisclaimer]\" tag.");
			}
			return errors;

		}

		public void SaveControlTemplates() {
			Rule.TemplateSMSAggShared=textSMSAggShared.Text.Replace("[ConfirmURL].","[ConfirmURL] .");
			Rule.TemplateSMSAggPerAppt=textSMSAggPerAppt.Text;
			Rule.TemplateEmailSubjAggShared=textEmailSubjAggShared.Text;
			Rule.TemplateEmailAggShared=_templateEmailAggShared;
			Rule.TemplateEmailAggPerAppt=textEmailAggPerAppt.Text;
			Rule.TemplateAutoReply=(Rule.TypeCur==ApptReminderType.Arrival) ? textArrivalResponse.Text : textSingleAutoReply.Text;
			Rule.TemplateAutoReplyAgg=textAggregateAutoReply.Text;
			Rule.TemplateComeInMessage=textComeIn.Text;
		}

	}
}
