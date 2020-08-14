using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEmailSetupEHR:ODForm {
		public FormEmailSetupEHR() {
			InitializeComponent();
			
		}

		private void FormEmailSetupEHR_Load(object sender,EventArgs e) {
			textPOPserver.Text=Prefs.GetString(PrefName.EHREmailPOPserver);
			textUsername.Text=Prefs.GetString(PrefName.EHREmailFromAddress);
			textPassword.Text=Prefs.GetString(PrefName.EHREmailPassword);
			textPort.Text=Prefs.GetString(PrefName.EHREmailPort);
		}

		private void butOK_Click(object sender,EventArgs e) {
			Prefs.Set(PrefName.EHREmailPOPserver,textPOPserver.Text);
			Prefs.Set(PrefName.EHREmailFromAddress,textUsername.Text);
			Prefs.Set(PrefName.EHREmailPassword,textPassword.Text);
			try{
				Prefs.Set(PrefName.EHREmailPort,PIn.Long(textPort.Text));
			}
			catch{
				MessageBox.Show("invalid port number.");
			}
			DataValid.SetInvalid(InvalidType.Prefs);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}