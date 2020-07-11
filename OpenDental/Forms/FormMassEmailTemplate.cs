using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormMassEmailTemplate:ODForm {
		///<summary>When a new template gets inserted in this form, this will be set to the primary key that the template cur receives.</summary>
		public long NewTemplateCurPriKey;
		///<summary>The template passed into this form. Shallow copy so anything that happens in this form will persist.</summary>
		private EmailHostingTemplate _templateCur;
		///<summary>Enum to keep track of the changes that happened while editing in the html email editor.</summary>
		private EmailType _emailType;
		///<summary>To save the changes that were made to the html body while in the email editor.</summary>
		private string _htmlText;


		///<summary>True when a new template is being created off of a pre-existing file (import).</summary>
		private bool _isImportOrCopy {
			get {
				return _templateCur.IsNew && !string.IsNullOrEmpty(_templateCur.BodyHTML);
			}
		}

		public FormMassEmailTemplate(EmailHostingTemplate template) {
			InitializeComponent();
			Lan.F(this);
			_templateCur=template;
		}

		private void FormMassEmailTemplate_Load(object sender,EventArgs e) {
			if(_isImportOrCopy && string.IsNullOrEmpty(_templateCur.BodyPlainText)) {
				//copy's will already have plain text set. We only want to override if importing html.
				_templateCur.BodyPlainText=MarkupEdit.ConvertToPlainText(_templateCur.BodyHTML);
			}
			_htmlText=_templateCur.BodyHTML??"";
			_emailType=_templateCur.EmailTemplateType;
			textTemplateName.Text=_templateCur.TemplateName;
			textSubject.Text=_templateCur.Subject;
			textboxPlainText.Text=_templateCur.BodyPlainText;
		}

		private void butBodyFieldsPlainText_Click(object sender,EventArgs e) {
			FormMessageReplacements FormMR=new FormMessageReplacements(
				MessageReplaceType.Appointment | MessageReplaceType.Office | MessageReplaceType.Patient | MessageReplaceType.User | MessageReplaceType.Misc);
			FormMR.IsSelectionMode=true;
			FormMR.ShowDialog();
			if(FormMR.DialogResult==DialogResult.OK) {
				textboxPlainText.SelectedText=ReplaceReplacementTag(FormMR.Replacement);
			}
		}

		private void butEditTemplate_Click(object sender,EventArgs e) {
			FormEmailEdit formEmailEdit=new FormEmailEdit();
			if(string.IsNullOrEmpty(_htmlText)) {
				_htmlText=textboxPlainText.Text;
			}
			formEmailEdit.MarkupText=_htmlText;
			formEmailEdit.IsRawAllowed=true;
			formEmailEdit.IsRaw=_emailType==EmailType.RawHtml;
			formEmailEdit.AreReplacementsAllowed=true;
			if(formEmailEdit.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_htmlText=formEmailEdit.MarkupText;
			if(string.IsNullOrEmpty(_htmlText)) {
				_emailType=EmailType.Regular;
			}
			else if(formEmailEdit.IsRaw) {
				_emailType=EmailType.RawHtml;
			}
			else {
				_emailType=EmailType.Html;//Our special wiki replcement that will turn into html.
			}
		}

		private void butSubjectFields_Click(object sender,EventArgs e) {
			FormMessageReplacements FormMR=new FormMessageReplacements(
				MessageReplaceType.Appointment | MessageReplaceType.Office | MessageReplaceType.Patient | MessageReplaceType.User | MessageReplaceType.Misc);
			FormMR.IsSelectionMode=true;
			FormMR.ShowDialog();
			if(FormMR.DialogResult==DialogResult.OK) {
				textSubject.SelectedText=ReplaceReplacementTag(FormMR.Replacement);
				textSubject.Invalidate();
			}
		}

		private string ReplaceReplacementTag(string replacementStr) {
			return replacementStr
					.Replace("[","[{[{ ")
					.Replace("]"," }]}]");
		}

		private bool IsTemplateValid() {
			if(string.IsNullOrEmpty(textTemplateName.Text)) {
				MessageBox.Show("Template must have a name.");
				return false;
			}
			if(string.IsNullOrEmpty(textSubject.Text)) {
				MessageBox.Show("Template must have a subject.");
				return false;
			}
			if(string.IsNullOrEmpty(textboxPlainText.Text)) {
				MessageBox.Show("Template must have plain text body.");
				return false;
			}
			return true;
		}

		private void butSave_Click(object sender,EventArgs e) {
			if(Save()) {
				DialogResult=DialogResult.OK;
			}
		}

		private bool Save() {
			if(!IsTemplateValid()) {
				return false;
			}
			if(string.IsNullOrEmpty(_htmlText)) {
				_htmlText=textboxPlainText.Text;
			}
			string xhtml;//api templates must have the full html text, even if only partial html. Database templates will store partial as plain text. 
			xhtml=_htmlText;
			if(_emailType==EmailType.Html && !string.IsNullOrEmpty(_htmlText)) {
				//This might not work for images, we should consider blocking them or warning them about sending if we detect images
				try {
					xhtml=MarkupEdit.TranslateToXhtml(_htmlText,true,false,true);
				}
				catch(Exception ex) {
					ex.DoNothing();
					if(!MsgBox.Show(MsgBoxButtons.YesNo,"There was an issue rendering your email.  If you use this template, you may send malformed emails to " +
						"every selected patient. Do you want to continue saving?")) 
					{
						return false;
					}
				}
			}
			List<string> listBodyReplacementsHTML=EmailHostingTemplates.GetListReplacements(xhtml).Distinct().ToList();
			List<string> listBodyReplacementsPT=EmailHostingTemplates.GetListReplacements(textboxPlainText.Text).Distinct().ToList();
			//If the template has HTML, we must ensure that the replacements in both HTML and Plain Text exactly match.
			if(!string.IsNullOrEmpty(_htmlText) && !listBodyReplacementsHTML.All(x => listBodyReplacementsPT.Contains(x))) {
				MsgBox.Show("The replacements in the HTML template did not exactly match the replacements in the Plain Text template.");
				return false;
			}
			//Create an API instance with the clinic num for this template.
			IAccountApi api=EmailHostingTemplates.GetAccountApi(_templateCur.ClinicNum);
			if(_templateCur.IsNew || _templateCur.TemplateId==0) {//templates inserted during convert didn't have their Id's set, we need to create them.
				try {
					CreateTemplateResponse response=api.CreateTemplate(new CreateTemplateRequest { 
						Template=new Template { 
							TemplateName=textTemplateName.Text,
							TemplateBodyHtml=xhtml,
							TemplateBodyPlainText=textboxPlainText.Text,
							TemplateSubject=textSubject.Text,
						},
					});
					//This is how we can update the template later
					_templateCur.TemplateId=response.TemplateNum;
				}
				catch(Exception e) {
					FriendlyException.Show("Failed to create template. Please try again.",e);
					return false;
				}
				_templateCur.Subject=textSubject.Text;
				_templateCur.TemplateName=textTemplateName.Text;
				_templateCur.BodyPlainText=textboxPlainText.Text;
				//_htmlText might be wiki html, which is intended to be saved (vs raw html) in case the user wants to modify their template later.
				_templateCur.BodyHTML=_htmlText;
				_templateCur.EmailTemplateType=_emailType;
				if(_templateCur.IsNew) {
					NewTemplateCurPriKey=EmailHostingTemplates.Insert(_templateCur);
				}
				else {
					EmailHostingTemplates.Update(_templateCur);
				}
			}
			else {
				//We must update the template with the api before we can update our template.
				try {
					api.UpdateTemplate(new UpdateTemplateRequest {//is back end expecting that template name can be changed now?
						TemplateNum=_templateCur.TemplateId,
						Template=new Template {
							TemplateName=textTemplateName.Text,
							TemplateBodyHtml=xhtml,
							TemplateBodyPlainText=textboxPlainText.Text,
							TemplateSubject=textSubject.Text,
						},
					});
				}
				catch(Exception e) {
					FriendlyException.Show("Failed to update template. Please try again.",e);
					return false;
				}
				_templateCur.Subject=textSubject.Text;
				_templateCur.TemplateName=textTemplateName.Text;
				_templateCur.BodyPlainText=textboxPlainText.Text;
				_templateCur.BodyHTML=_htmlText;
				_templateCur.EmailTemplateType=_emailType;
				EmailHostingTemplates.Update(_templateCur);
			}
			return true;//save successful
		}

		private void butCancel_Click(object sender,EventArgs e) {
			if(_isImportOrCopy  &&
				!MsgBox.Show(MsgBoxButtons.YesNo,"Canceling will cause a loss of all work. Do you want to continue?")) 
			{
				return;
			}
			DialogResult=DialogResult.Cancel;
		}
	}
}