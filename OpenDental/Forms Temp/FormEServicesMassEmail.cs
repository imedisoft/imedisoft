using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using WebServiceSerializer;

namespace OpenDental
{

    public partial class FormEServicesMassEmail:ODForm {
		private bool _doSetInvalidClinicPrefs=false;
		private string _activateMessage;
		private WebServiceMainHQProxy.EServiceSetup.SignupOut _signupOut;

		//==================== Promotion Variables ====================
		private Clinic _promoClinicCur {	
			get {	
				return comboClinicMassEmail.GetSelected<Clinic>();	
			}	
		}

		private List<Clinic> _promoListClinics {	
			get {	
				return comboClinicMassEmail.Items.OfType<ODBoxItem<Clinic>>().Select(x => x.Tag).ToList(); 
			}
		}
		
		public FormEServicesMassEmail(WebServiceMainHQProxy.EServiceSetup.SignupOut signupOut) {
			InitializeComponent();
			
			_signupOut=signupOut;
		}

		private void FormEServicesMassEmail_Load(object sender,EventArgs e) {
			FillComboClinicsMassEmail();
			FillPromotionForClinic();
			bool allowEdit=Security.IsAuthorized(Permissions.EServicesSetup,true);
			_activateMessage="Mass Email is based on usage. By activating and enabling this feature you are agreeing to the charges. Continue? ";
			AuthorizeMassEmail(allowEdit);
			WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService massEmail=_signupOut.EServices
				.FirstOrDefault(x => x.EService==eServiceCode.EmailMassUsage);
			if(massEmail!=null) {
				webBrowser1.Navigate(massEmail.HostedUrl);
			}
			else {
				webBrowser1.Visible=false;
			}
		}

		private void AuthorizeMassEmail(bool allowEdit) {
			checkIsMassEmailEnabled.Enabled=allowEdit;
			butActivate.Enabled=allowEdit;
		}

		private void FillComboClinicsMassEmail() {
			comboClinicMassEmail.BeginUpdate();
			comboClinicMassEmail.Items.Clear();
			if(PrefC.HasClinicsEnabled) {
				ODBoxItem<Clinic> clinicItem=new ODBoxItem<Clinic>("Defaults",new Clinic() {Description="Defaults",Abbr="Defaults" });
				comboClinicMassEmail.Items.Add(clinicItem);
				foreach(Clinic item in Clinics.GetForUserod(Security.CurrentUser)) {
					clinicItem=new ODBoxItem<Clinic>(item.Abbr,item);
					comboClinicMassEmail.Items.Add(clinicItem);
				}
			}
			else {//NO CLINICS
				comboClinicMassEmail.Items.Add(new ODBoxItem<Clinic>("Practice",Clinics.GetPracticeAsClinicZero()));
			}
			comboClinicMassEmail.SelectedIndex=0;
			comboClinicMassEmail.EndUpdate();
		}

		///<summary>Fills all necessary data for the clinic.</summary>
		private void FillPromotionForClinic() {
			if(PrefC.HasClinicsEnabled && _promoClinicCur.ClinicNum==0) {
				//there is no default for these settings. They have to be saved by clinic. 
				checkIsMassEmailEnabled.Visible=false;
				butActivate.Visible=false;
			}
			else {
				//either no clinics, or has clinics but has a real clinic selected.
				MassEmailStatus massEmailStatus=PIn.Enum<MassEmailStatus>(ClinicPrefs.GetInt(_promoClinicCur.ClinicNum, PrefName.MassEmailStatus));
				butActivate.Visible=massEmailStatus==MassEmailStatus.NotActivated;
				checkIsMassEmailEnabled.Visible=!butActivate.Visible;
				checkIsMassEmailEnabled.Checked=massEmailStatus.HasFlag(MassEmailStatus.Enabled);
			}
		}

		private void comboClinicPromotion_SelectionChangeCommitted(object sender,EventArgs e) {
			FillPromotionForClinic();
		}

		private void butActivate_Click(object sender,EventArgs e) {
			//if(!MsgBox.Show(MsgBoxButtons.YesNo,_activateMessage))	{
			//	return;
			//}
			//IWebServiceMainHQ instance=WebServiceMainHQProxy.GetWebServiceMainHQInstance();
			//string guid="";
			//string secret="";
			//try {
			//	ODProgress.ShowAction(() => {
			//		string result=instance.EmailHostingSignup(PayloadHelper.CreatePayload(new List<PayloadItem> {
			//			new PayloadItem(_promoClinicCur.ClinicNum,"ClinicNum"),
			//		},eServiceCode.Undefined));
			//		guid=WebSerializer.DeserializeTag<string>(result,"AccountGUID");
			//		ODException.SwallowAnyException(() => {
			//			//Only sent the first time EmailHostingSignup is called.
			//			secret=WebSerializer.DeserializeTag<string>(result,"AccountSecret");
			//		});
			//	},"Activating promotions...");
			//}
			//catch(Exception ex) {
			//	FriendlyException.Show("An error occurred while activating promotions.",ex);
			//	return;
			//}
			////If we made it this far, we have activated that account successfully.
			//ClinicPrefs.Upsert(PrefName.MassEmailStatus,_promoClinicCur.ClinicNum,((int)(MassEmailStatus.Activated | MassEmailStatus.Enabled)).ToString());
			//if(!string.IsNullOrWhiteSpace(guid)) {
			//	ClinicPrefs.Upsert(PrefName.MassEmailGuid,_promoClinicCur.ClinicNum,guid);
			//}
			//if(!string.IsNullOrWhiteSpace(secret)) {
			//	ClinicPrefs.Upsert(PrefName.MassEmailSecret,_promoClinicCur.ClinicNum,secret);
			//}
			//butActivate.Visible=false;
			//checkIsMassEmailEnabled.Visible=true;
			//checkIsMassEmailEnabled.Checked=true;
			//ClinicPrefs.RefreshCache();
			//_doSetInvalidClinicPrefs=true;
		}

		private void checkIsMassEmailEnabled_Click(object sender,EventArgs e) {
			//if(checkIsMassEmailEnabled.Checked && !MsgBox.Show(MsgBoxButtons.YesNo,_activateMessage))	{//box is being checked
			//	checkIsMassEmailEnabled.Checked=false;
			//	return;
			//}
			//IWebServiceMainHQ instance=WebServiceMainHQProxy.GetWebServiceMainHQInstance();
			//bool isEnabling=checkIsMassEmailEnabled.Checked;
			//try {
			//	ODProgress.ShowAction(() => { 			
			//		string result=instance.EmailHostingChangeClinicStatus(PayloadHelper.CreatePayload(new List<PayloadItem> {
			//			new PayloadItem(_promoClinicCur.ClinicNum,"ClinicNum"),
			//			new PayloadItem(isEnabling,"EnableClinic"),
			//		},eServiceCode.Undefined));
			//		//This will do the job of checking for an error.
			//		WebSerializer.DeserializeTag<string>(result,"ChangeClinicStatusResponse");
			//	},"Changing promotion status...");
			//}
			//catch(Exception ex) {
			//	//Failed. Reverse the state of what they were trying to do.
			//	checkIsMassEmailEnabled.Checked=!checkIsMassEmailEnabled.Checked;
			//	FriendlyException.Show("An error occurred while changing the promotion status.",ex);
			//	return;
			//}
			//MassEmailStatus status=MassEmailStatus.NotActivated;
			//ClinicPref clinicPref=ClinicPrefs.GetPref(PrefName.MassEmailStatus,_promoClinicCur.ClinicNum);
			//if(clinicPref!=null){
			//	status=PIn.Enum<MassEmailStatus>(clinicPref.ValueString,false);
			//}
			//status=isEnabling ? status.AddFlag(MassEmailStatus.Enabled) : status.RemoveFlag(MassEmailStatus.Enabled);
			//ClinicPrefs.Upsert(PrefName.MassEmailStatus,_promoClinicCur.ClinicNum,((int)status).ToString());
			//ClinicPrefs.RefreshCache();
			//_doSetInvalidClinicPrefs=true;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(_doSetInvalidClinicPrefs) {
				DataValid.SetInvalid(InvalidType.ClinicPrefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}