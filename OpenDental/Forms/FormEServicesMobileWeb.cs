using OpenDentBusiness;
using System;
using System.Linq;

namespace OpenDental
{

    public partial class FormEServicesMobileWeb:ODForm {
		WebServiceMainHQProxy.EServiceSetup.SignupOut _signupOut;
		
		public FormEServicesMobileWeb(WebServiceMainHQProxy.EServiceSetup.SignupOut signupOut=null) {
			InitializeComponent();
			Lan.F(this);
			_signupOut=signupOut;
		}

		private void FormEServicesMobileWeb_Load(object sender,EventArgs e) {
			if(_signupOut==null){
				_signupOut=FormEServicesSetup.GetSignupOut();
			}
			string urlFromHQ=(
				WebServiceMainHQProxy.GetSignups<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>(_signupOut,eServiceCode.MobileWeb).FirstOrDefault()??
				new WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService() { HostedUrl="" }
			).HostedUrl;
			textHostedUrlMobileWeb.Text=urlFromHQ;
		}
	
		private void butSetupMobileWebUsers_Click(object sender,EventArgs e) {
			FormOpenDental.S_MenuItemSecurity_Click(sender,e);
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}
	}
}