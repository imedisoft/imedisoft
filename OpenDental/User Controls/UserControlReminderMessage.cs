using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CodeBase;
using Imedisoft.Data;
using OpenDentBusiness;

namespace OpenDental
{
	public partial class UserControlReminderMessage : UserControl
	{
		private string _templateEmail;

		public UserControlReminderMessage(ApptReminderRule apptReminder)
		{
			InitializeComponent();
			Rule = apptReminder;
			LoadControl();
		}

		public ApptReminderRule Rule { get; }

		private void butEditEmail_Click(object sender, EventArgs e)
		{
			FormEmailEdit formEE = new FormEmailEdit();
			formEE.MarkupText = _templateEmail;
			formEE.IsRawAllowed = true;
			formEE.IsRaw = Rule.EmailTemplateType == EmailType.RawHtml;
			formEE.ShowDialog();
			if (formEE.DialogResult != DialogResult.OK)
			{
				return;
			}
			Rule.EmailTemplateType = formEE.IsRaw ? EmailType.RawHtml : EmailType.Html;
			_templateEmail = formEE.MarkupText;
			RefreshEmail();
		}

		private void LoadControl()
		{
			textTemplateSms.Text = Rule.TemplateSMS;
			textTemplateSubject.Text = Rule.TemplateEmailSubject;
			_templateEmail = Rule.TemplateEmail;
			if (Rule.TypeCur == ApptReminderType.PatientPortalInvite)
			{
				textTemplateSms.Enabled = false;
			}
			if (Rule.TypeCur == ApptReminderType.Arrival)
			{
				groupEmail.Visible = false;
			}
			RefreshEmail();
		}

		private void RefreshEmail()
		{
			if (Rule.EmailTemplateType == EmailType.RawHtml)
			{
				browserEmailBody.DocumentText = _templateEmail;
				return;//text is already in HTML, it does not need to be translated. 
			}
			try
			{
				string text = MarkupEdit.TranslateToXhtml(_templateEmail, isPreviewOnly: true, hasWikiPageTitles: false, isEmail: true);
				browserEmailBody.DocumentText = text;
			}
			catch
			{
			}
		}

		public List<string> ValidateTemplates()
		{
			List<string> listErrors = new List<string>();
			if (Rule.TypeCur == ApptReminderType.Arrival)
			{
				if (!textTemplateSms.Text.ToLower().Contains(OpenDentBusiness.AutoComm.ArrivalsTagReplacer.ARRIVED_TAG.ToLower()))
				{
					listErrors.Add($"Arrival texts must contain the \"{OpenDentBusiness.AutoComm.ArrivalsTagReplacer.ARRIVED_TAG}\" tag.");
				}
			}
			else
			{
				if (Rule.TypeCur != ApptReminderType.PatientPortalInvite)
				{
					if (string.IsNullOrWhiteSpace(textTemplateSms.Text))
					{
						listErrors.Add("Text message cannot be blank.");
					}
				}
				if (string.IsNullOrWhiteSpace(textTemplateSubject.Text))
				{
					listErrors.Add("Email subject cannot be blank.");
				}
				if (string.IsNullOrWhiteSpace(_templateEmail))
				{
					listErrors.Add("Email message cannot be blank.");
				}
				if (Preferences.GetBool(PreferenceName.EmailDisclaimerIsOn) && !_templateEmail.ToLower().Contains("[emaildisclaimer]"))
				{
					listErrors.Add("Email must contain the \"[EmailDisclaimer]\" tag.");
				}
			}
			if (Rule.TypeCur == ApptReminderType.ConfirmationFutureDay)
			{
				if (!textTemplateSms.Text.ToLower().Contains("[confirmcode]"))
				{
					listErrors.Add("Confirmation texts must contain the \"[ConfirmCode]\" tag.");
				}
				if (_templateEmail.ToLower().Contains("[confirmcode]"))
				{
					listErrors.Add("Confirmation emails should not contain the \"[ConfirmCode]\" tag.");
				}
				if (!_templateEmail.ToLower().Contains("[confirmurl]"))
				{
					listErrors.Add("Confirmation emails must contain the \"[ConfirmURL]\" tag.");
				}
			}
			if (Rule.TypeCur == ApptReminderType.ScheduleThankYou)
			{
				//ThankYou templates can only use the [AddToCalendar] tag when Confirmations are enabled.
				if (!Preferences.GetBool(PreferenceName.ApptConfirmAutoSignedUp))
				{
					string addToCalTag = ApptThankYouSents.ADD_TO_CALENDAR.ToLower();
					string errorText(string mode)
					{
						return
							$"Automated Thank-You {mode} cannot contain " + ApptThankYouSents.ADD_TO_CALENDAR + " when not signed up for eConfirmations.";
					}
					if (textTemplateSms.Text.ToLower().Contains(addToCalTag))
					{
						listErrors.Add(errorText("texts"));
					}
					if (_templateEmail.ToLower().Contains(addToCalTag))
					{
						listErrors.Add(errorText("emails"));
					}
				}
			}
			if (Rule.Language != "" && Rule.ApptReminderRuleNum == 0)
			{//new rule, user may have not remembered to setup the aggregates
				string err =
					"Aggregated templates have not been set up for all additional languages. " +
					"Click on the 'Advanced' button to set up aggregated templates.";
				if (Rule.TypeCur == ApptReminderType.Arrival)
				{
					if (Rule.TemplateSMSAggShared == "" || Rule.TemplateSMSAggPerAppt == "")
					{ //Arrivals don't include email
						listErrors.Add(err);
					}
				}
				else
				{
					if (Rule.TemplateSMSAggShared == "" || Rule.TemplateSMSAggPerAppt == "" || Rule.TemplateEmailAggShared == "" || Rule.TemplateEmailAggPerAppt == "")
					{
						listErrors.Add(err);
					}
				}
			}
			return listErrors;
		}

		public void SaveControlTemplates()
		{
			Rule.TemplateSMS = textTemplateSms.Text.Replace("[ConfirmURL].", "[ConfirmURL] .");//Clicking a link with a period will not get recognized. 
			Rule.TemplateEmailSubject = textTemplateSubject.Text;
			Rule.TemplateEmail = _templateEmail;
		}

	}
}
