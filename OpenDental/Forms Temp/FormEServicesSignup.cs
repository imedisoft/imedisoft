using CodeBase;
using OpenDentBusiness;
using System;

namespace OpenDental
{

    public partial class FormEServicesSignup:ODForm {
		WebServiceMainHQProxy.EServiceSetup.SignupOut _signupOut;
		
		public FormEServicesSignup(WebServiceMainHQProxy.EServiceSetup.SignupOut signupOut=null) {
			InitializeComponent();
			Lan.F(this);
			_signupOut=signupOut;
		}

		private void FormEServicesSignup_Load(object sender,EventArgs e) {
			if(_signupOut==null){
				_signupOut=FormEServicesSetup.GetSignupOut();
			}
			ODException.SwallowAnyException(() => {
#if DEBUG
				_signupOut.SignupPortalUrl=_signupOut.SignupPortalUrl.Replace("https://www.patientviewer.com/SignupPortal/GWT/SignupPortal/SignupPortal.html","http://127.0.0.1:8888/SignupPortal.html");
#endif
				webBrowserSignup.Navigate(_signupOut.SignupPortalUrl);
			});
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}
	}
}