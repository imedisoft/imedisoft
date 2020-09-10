using System;
using System.Windows.Forms;
using Imedisoft.Data;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormStateAbbrEdit:ODForm {
		private StateAbbr _stateAbbrCur;

		public FormStateAbbrEdit(StateAbbr stateAbbr) {
			_stateAbbrCur=stateAbbr;
			InitializeComponent();
			
		}

		private void FormStateAbbrEdit_Load(object sender,EventArgs e) {
			textDescription.Text=_stateAbbrCur.Description;
			textAbbr.Text=_stateAbbrCur.Abbr;
			if(Preferences.GetBool(PreferenceName.EnforceMedicaidIDLength)) {
				if(_stateAbbrCur.MedicaidIDLength!=0) {
					textMedIDLength.Text=_stateAbbrCur.MedicaidIDLength.ToString();
				}
			}
			else {
				labelMedIDLength.Visible=false;
				textMedIDLength.Visible=false;
				this.Height-=30;
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_stateAbbrCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Delete State Abbr?")) {
				return;
			}
			StateAbbrs.Delete(_stateAbbrCur.StateAbbrNum);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDescription.Text=="") {
				MessageBox.Show("Description cannot be blank.");
				return;
			}
			if(textAbbr.Text=="") {
				MessageBox.Show("Abbrevation cannot be blank.");
				return;
			}
			if(textMedIDLength.errorProvider1.GetError(textMedIDLength)!="") {
				MessageBox.Show("Medicaid ID length is invalid.");
				return;
			}
			_stateAbbrCur.Description=textDescription.Text;
			_stateAbbrCur.Abbr=textAbbr.Text;
			if(Preferences.GetBool(PreferenceName.EnforceMedicaidIDLength)) {
				_stateAbbrCur.MedicaidIDLength=0;
				if(textMedIDLength.Text!="") {
					_stateAbbrCur.MedicaidIDLength=PIn.Int(textMedIDLength.Text);
				}
			}
			if(_stateAbbrCur.IsNew) {
				StateAbbrs.Insert(_stateAbbrCur);
			}
			else {
				StateAbbrs.Update(_stateAbbrCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}