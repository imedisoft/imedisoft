using System;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEmailMasterTemplate:ODForm {

		public FormEmailMasterTemplate() {
			InitializeComponent();
			
		}

		private void FormEmailSetupMasterTemplate_Load(object sender,EventArgs e) {
			textMaster.Text=Prefs.GetString(PrefName.EmailMasterTemplate);
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(Prefs.Set(PrefName.EmailMasterTemplate,textMaster.Text)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
