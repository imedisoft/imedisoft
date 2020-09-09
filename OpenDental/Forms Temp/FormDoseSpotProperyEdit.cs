using System;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Linq;
using System.Collections.Generic;
using Imedisoft.Forms;

namespace OpenDental {
	public partial class FormDoseSpotPropertyEdit:ODForm {
		
		public string ClinicIdVal;
		public string ClinicKeyVal;
		public List<ProgramProperty> ListProperties;
		private Clinic _clinicCur;

		public FormDoseSpotPropertyEdit(Clinic clinicCur,string ppClinicIdVal,string ppClinicKeyVal,List<ProgramProperty> listProperties) {
			InitializeComponent();
			
			_clinicCur=clinicCur;
			ClinicIdVal=ppClinicIdVal;
			ClinicKeyVal=ppClinicKeyVal;
			ListProperties=listProperties;
		}

		private void FormDoseSpotPropertyEdit_Load(object sender,EventArgs e) {
			textClinicAbbr.Text=_clinicCur.Abbr;
			textClinicID.Text=ClinicIdVal;
			textClinicKey.Text=ClinicKeyVal;
			if(ClinicIdVal.Trim()!="" && ClinicKeyVal.Trim()!="") {//The clinic has values for the clinicId/clinicKey, so they are effectively registered.
				butRegisterClinic.Enabled=false;
			}
			if(_clinicCur.Id==0) {//Clinics disabled or is HQ.
				menuItemSetup.Enabled=false;//There is no clinic record to edit.
			}
			Program programErx=Programs.GetCur(ProgramName.eRx);
			ProgramProperty ppClinicID=ListProperties
					.FirstOrDefault(x => x.ClinicId!=_clinicCur.Id && x.Description==Erx.PropertyDescs.ClinicID && x.Value!="");
			ProgramProperty ppClinicKey=null;
			if(ppClinicID!=null) {
				ppClinicKey=ListProperties
					.FirstOrDefault(x => x.ClinicId==ppClinicID.ClinicId && x.Description==Erx.PropertyDescs.ClinicKey && x.Value!="");
			}
			if(ppClinicID==null || string.IsNullOrWhiteSpace(ppClinicID.Value)
				|| ppClinicKey==null || string.IsNullOrWhiteSpace(ppClinicKey.Value))
			{
				//No clinicID/clinicKey found.  This would be the first clinic to register
				butRegisterClinic.Enabled=false;
				butClear.Enabled=false;
			}
		}

		private void menuItemSetup_Click(object sender,EventArgs e) {
			FormClinicEdit form=new FormClinicEdit(_clinicCur);
			form.ShowDialog();
			if(form.DialogResult==DialogResult.OK) {
				Clinics.Update(_clinicCur);
				DataValid.SetInvalid(InvalidType.Providers);
			}
		}

		private void butClear_Click(object sender,EventArgs e) {
			textClinicID.Text="";
			textClinicKey.Text="";
		}

		private void butRegisterClinic_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			try {
				Program programErx=Programs.GetCur(ProgramName.eRx);
				ProgramProperty ppClinicID=ListProperties
					.FirstOrDefault(x => x.ClinicId!=_clinicCur.Id && x.Description==Erx.PropertyDescs.ClinicID && x.Value!="");
				ProgramProperty ppClinicKey=null;
				if(ppClinicID!=null) {
					ppClinicKey=ListProperties
						.FirstOrDefault(x => x.ClinicId==ppClinicID.ClinicId && x.Description==Erx.PropertyDescs.ClinicKey && x.Value!="");
				}
				if(ppClinicID==null || string.IsNullOrWhiteSpace(ppClinicID.Value)
					|| ppClinicKey==null || string.IsNullOrWhiteSpace(ppClinicKey.Value))
				{
					//Should never happen since we disable this button if we can't find a valid clinicID/clinicKey combo ahead of time
					throw new ODException("No registered clinics found.  "
						+"There must be at least one registered clinic before adding additional clinics.");
				}
				string clinicID="";
				string clinicKey="";
				DoseSpot.RegisterClinic(_clinicCur.Id,ppClinicID.Value,ppClinicKey.Value
					,DoseSpot.GetUserID(Security.CurrentUser,_clinicCur.Id),out clinicID,out clinicKey);
				textClinicID.Text=clinicID;
				textClinicKey.Text=clinicKey;
			}
			catch(ODException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			catch(Exception ex) {
				MessageBox.Show("Error: "+ex.Message);
				return;
			}
			finally {
				Cursor=Cursors.Default;
			}
			MessageBox.Show("This clinic has successfully been registered with DoseSpot.\r\n"
				+"If patients in this clinic can be shared with other clinics, contact DoseSpot to link this clinic before using.");
		}

		private void butOK_Click(object sender,EventArgs e) {
			ClinicIdVal=textClinicID.Text.Trim();
			ClinicKeyVal=textClinicKey.Text.Trim();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}