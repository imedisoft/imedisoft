using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Net.Mail;
using System.Linq;

namespace OpenDental {
	public partial class FormMassEmailSend:ODForm {
		private readonly EmailHostingTemplate _templateCur;
		private List<FormMassEmail.PatientInfo> _listPatientsSelected;
		///<summary>Patient users wants to view replaced data with. Can be null.</summary>
		private Patient _patSelected;

		public FormMassEmailSend(EmailHostingTemplate template,List<FormMassEmail.PatientInfo> listPatientSelected) {
			InitializeComponent();
			
			_templateCur=template;
			_listPatientsSelected=listPatientSelected;
		}

		private void FormMassEmailSend_Load(object sender,EventArgs e) {
			labelSendingPatients.Text=labelSendingPatients.Text.Replace("###",_listPatientsSelected.Count.ToString());
			textSubject.Text=_templateCur.Subject;
			userControlEmailTemplate1.RefreshView(_templateCur.BodyPlainText,_templateCur.BodyHTML,_templateCur.EmailTemplateType);
			labelReplacedData.ForeColor=Color.Firebrick;
			labelReplacedData.Text="Without replaced data";
		}

		private void checkDisplay_Click(object sender,EventArgs e) {
			if(_patSelected==null) {
				MsgBox.Show("Please select a patient to use as an example for replacement data.");
				checkDisplay.Checked=false;
				return;
			}
			if(checkDisplay.Checked) {
				labelReplacedData.ForeColor=Color.LimeGreen;
				labelReplacedData.Text="With replaced data";
				FormMassEmail.PatientInfo pat=_listPatientsSelected.First(x => x.PatNum==_patSelected.PatNum);
				Patient guarantor=Patients.GetPat(_patSelected.Guarantor);
				Appointment apt=Appointments.GetOneApt(pat.NextAptNum);
				Clinic clinicPat=Clinics.GetById(pat.ClinicNum);
				//Refresh view with the newly replaced data
				string replacedSubject;
				string replacedPlainText;
				string replacedHtmlText="";
				List<string> listSubjectReplacements=EmailHostingTemplates.GetListReplacements(_templateCur.Subject).Distinct().ToList();
				List<string> listBodyHtmlReplacements=EmailHostingTemplates.GetListReplacements(_templateCur.BodyHTML).Distinct().ToList();
				List<string> listBodyPlainReplacements=listBodyHtmlReplacements;
				SerializableDictionary<string,string> subjectReplacements=new SerializableDictionary<string,string>();
				foreach(string replacement in listSubjectReplacements) {
					subjectReplacements[replacement]=GetReplacementValue(replacement,pat,guarantor,apt,clinicPat);
				}
				SerializableDictionary<string,string> bodyHtmlReplacements=new SerializableDictionary<string,string>();
				foreach(string replacement in listBodyHtmlReplacements) {
					bodyHtmlReplacements[replacement]=GetReplacementValue(replacement,pat,guarantor,apt,clinicPat);
				}
				SerializableDictionary<string,string> bodyPlainReplacements=new SerializableDictionary<string,string>();
				foreach(string replacement in listBodyPlainReplacements) {
					bodyPlainReplacements[replacement]=GetReplacementValue(replacement,pat,guarantor,apt,clinicPat);
				}
				replacedSubject=PerformAllReplacements(_templateCur.Subject,_patSelected,guarantor,apt,clinicPat);
				replacedHtmlText=PerformAllReplacements(_templateCur.BodyHTML,_patSelected,guarantor,apt,clinicPat);
				replacedPlainText=PerformAllReplacements(_templateCur.BodyPlainText,_patSelected,guarantor,apt,clinicPat);
				userControlEmailTemplate1.RefreshView(replacedPlainText,replacedHtmlText,_templateCur.EmailTemplateType);
				textSubject.Text=replacedSubject;
			}
			else {
				labelReplacedData.ForeColor=Color.Firebrick;
				labelReplacedData.Text="Without replaced data";
				//Refresh view with the original un-replaced values. 
				userControlEmailTemplate1.RefreshView(_templateCur.BodyPlainText,_templateCur.BodyHTML,_templateCur.EmailTemplateType);
				textSubject.Text=_templateCur.Subject;
			}
		}

		private string GetReplacementValue(string replacementKey,FormMassEmail.PatientInfo pat,Patient guarantor,Appointment apt,Clinic clinicPat) {
			string bracketReplacement="["+replacementKey+"]";
			string result=Patients.ReplacePatient(bracketReplacement,_patSelected);
			if(result==bracketReplacement) {
				result=Patients.ReplaceGuarantor(bracketReplacement,guarantor);
			}
			if(result==bracketReplacement) {
				result=ReplaceTags.ReplaceMisc(bracketReplacement);
			}
			if(result==bracketReplacement) {
				result=ReplaceTags.ReplaceUser(bracketReplacement,Security.CurrentUser);
			}
			if(result==bracketReplacement) {
				result=Appointments.ReplaceAppointment(bracketReplacement,apt);
			}
			if(result==bracketReplacement) {
				result=Clinics.ReplaceOffice(bracketReplacement,clinicPat);
			}
			return result;
		}

		private string PerformAllReplacements(string message,Patient pat,Patient guarantor,Appointment apt,Clinic clinicPat) {
			message=message.Replace("[{[{ ","[").Replace(" }]}]","]")
				.Replace("[{[{","[").Replace("}]}]","]");
			message=Patients.ReplacePatient(message,pat);
			message=Patients.ReplaceGuarantor(message,guarantor);
			message=ReplaceTags.ReplaceMisc(message);
			message=ReplaceTags.ReplaceUser(message,Security.CurrentUser);
			message=Appointments.ReplaceAppointment(message,apt);
			message=Clinics.ReplaceOffice(message,clinicPat);
			return message;
		}

		private void butPatientSelect_Click(object sender,EventArgs e) {
			//Build our grid to display for the form we are about to show the user. 
			List<UI.GridColumn> listColumns=new List<UI.GridColumn>();
			listColumns.Add(new UI.GridColumn("Patient",0));
			List<UI.GridRow> listRows=new List<UI.GridRow>();
			foreach(FormMassEmail.PatientInfo patient in _listPatientsSelected) {
				UI.GridRow row=new UI.GridRow();
				row.Cells.Add(patient.Name);
				row.Tag=patient;
				listRows.Add(row);
			}
			FormGridSelection formGridSelection=new FormGridSelection(listColumns,listRows,"Select Patient","Select Patient");
			if(formGridSelection.ShowDialog()!=DialogResult.OK) {
				return;
			}
			FormMassEmail.PatientInfo selectedPat=(FormMassEmail.PatientInfo)formGridSelection.ListSelectedTags.FirstOrDefault();
			_patSelected=Patients.GetPat(selectedPat.PatNum);
			if(_patSelected!=null) {
				textPatient.Text=_patSelected.LName+", "+_patSelected.FName;
			}
		}

		private void butSendEmails_Click(object sender,EventArgs e) {
			string message="Are you sure you want to send this email to "+_listPatientsSelected.Count+" patients?";
			if(MessageBox.Show(message,"",MessageBoxButtons.YesNo)==DialogResult.No) {
				return;
			}
			EmailAddress emailAddress=null;
			long emailAddressNum=PrefC.HasClinicsEnabled
				? Clinics.GetById(Clinics.ClinicId)?.EmailAddressId??0
				: Prefs.GetLong(PrefName.EmailDefaultAddressNum);
			if(emailAddressNum > 0) {
				emailAddress=EmailAddresses.GetOne(emailAddressNum);
			}
			if(emailAddress==null) {
				FormEmailAddresses formEmailAddresses=new FormEmailAddresses();
				formEmailAddresses.IsSelectionMode=true;
				DialogResult result=formEmailAddresses.ShowDialog();
				if(result!=DialogResult.OK || formEmailAddresses.EmailAddressNum==0) {
					return;
				}
				emailAddress=EmailAddresses.GetOne(formEmailAddresses.EmailAddressNum);
			}
			//This object will parse out the sender name and email address.
			MailAddress toAddress=new MailAddress(string.IsNullOrWhiteSpace(emailAddress.SenderAddress) ? emailAddress.EmailUsername : emailAddress.SenderAddress);
			string promotionName="";
			InputBox box=new InputBox("What would you like to name this mass email group? This will help identify the emails when looking at analytics.");
			box.textResult.Text=_templateCur.TemplateName;
			if(box.ShowDialog()!=DialogResult.OK) {
				return;
			}
			promotionName=box.textResult.Text;
			if(string.IsNullOrWhiteSpace(promotionName)) {
				return;
			}
			string error=null;
			ODProgress.ShowAction(() => {
				error=Promotions.SendEmails(_templateCur,_listPatientsSelected.Select(x => new MassEmailDestination { PatNum=x.PatNum,AptNum=x.NextAptNum }).ToList(),
					toAddress.DisplayName,toAddress.Address,promotionName,PromotionType.Manual);
			},"Sending emails...");
			if(!string.IsNullOrWhiteSpace(error)) {
				MsgBox.Show(error);
				return;
			}
			long numberSent=_listPatientsSelected.Count;
			string templateName=_templateCur.TemplateName;
			MessageBox.Show($"Sent to {numberSent} patients with template: \"{templateName}\" for group: \"{promotionName}\".");
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}