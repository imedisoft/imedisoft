using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using Imedisoft.Forms;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental {
	public partial class FormEncounterEdit:ODForm {
		private Encounter _encCur;
		private Patient _patCur;
		private long _provNumSelected;
		private List<Provider> _listProviders;

		public FormEncounterEdit(Encounter encCur) {
			InitializeComponent();
			_encCur=encCur;
		}

		public void FormEncounters_Load(object sender,EventArgs e) {
			_patCur=Patients.GetPat(_encCur.PatientId);
			this.Text+=" - "+_patCur.GetNameLF();
			textDateEnc.Text=_encCur.DateEncounter.ToShortDateString();
			_provNumSelected=_encCur.ProviderId;
			comboProv.Items.Clear();
			_listProviders=Providers.GetAll(true);
			for(int i=0;i<_listProviders.Count;i++) {
				comboProv.Items.Add(_listProviders[i].GetLongDesc());//Only visible provs added to combobox.
				if(_listProviders[i].Id==_encCur.ProviderId) {
					comboProv.SelectedIndex=i;//Sets combo text too.
				}
			}
			if(_provNumSelected==0) {//Is new
				comboProv.SelectedIndex=0;
				_provNumSelected=_listProviders[0].Id;
			}
			if(comboProv.SelectedIndex==-1) {//The provider exists but is hidden
				comboProv.Text=Providers.GetLongDesc(_provNumSelected);//Appends "(hidden)" to the end of the long description.
			}
			textNote.Text=_encCur.Note;
			textCodeValue.Text=_encCur.CodeValue;
			textCodeSystem.Text=_encCur.CodeSystem;
			//to get description, first determine which table the code is from.  Encounter is only allowed to be a CDT, CPT, HCPCS, and SNOMEDCT.  Will be null if new encounter.
			switch(_encCur.CodeSystem) {
				case "CDT":
					textCodeDescript.Text=ProcedureCodes.GetProcCode(_encCur.CodeValue).Description;
					break;
				case "CPT":
					Cpt cptCur=Cpts.GetByCode(_encCur.CodeValue);
					if(cptCur!=null) {
						textCodeDescript.Text=cptCur.Description;
					}
					break;
				case "HCPCS":
					Hcpcs hCur=Hcpcses.GetByCode(_encCur.CodeValue);
					if(hCur!=null) {
						textCodeDescript.Text=hCur.DescriptionShort;
					}
					break;
				case "SNOMEDCT":
					Snomed sCur=Snomeds.GetByCode(_encCur.CodeValue);
					if(sCur!=null) {
						textCodeDescript.Text=sCur.Description;
					}
					break;
				case null:
					textCodeDescript.Text="";
					break;
				default:
					MessageBox.Show("Error: Unknown code system");
					break;
			}
		}

		private void comboProv_SelectionChangeCommitted(object sender,EventArgs e) {
			_provNumSelected=_listProviders[comboProv.SelectedIndex].Id;
		}

		private void butPickProv_Click(object sender,EventArgs e) {
			FormProviderPick FormP=new FormProviderPick();
			if(comboProv.SelectedIndex>-1) {
				FormP.SelectedProviderId=_listProviders[comboProv.SelectedIndex].Id;
			}
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return;
			}
			comboProv.SelectedIndex=Providers.GetIndex(FormP.SelectedProviderId);
			_provNumSelected=FormP.SelectedProviderId;
		}

		private void butSnomed_Click(object sender,EventArgs e) {
			FormSnomeds formS=new FormSnomeds();
			formS.IsSelectionMode=true;
			if(formS.ShowDialog()==DialogResult.OK) {
				_encCur.CodeSystem="SNOMEDCT";
				_encCur.CodeValue=formS.SelectedSnomed.Code;
				textCodeSystem.Text="SNOMEDCT";
				textCodeValue.Text=formS.SelectedSnomed.Code;
				textCodeDescript.Text=formS.SelectedSnomed.Description;
			}
		}

		private void butHcpcs_Click(object sender,EventArgs e) {
			FormHcpcs formHcpcses=new FormHcpcs();
			formHcpcses.IsSelectionMode=true;
			if(formHcpcses.ShowDialog()==DialogResult.OK) {
				_encCur.CodeSystem="HCPCS";
				_encCur.CodeValue=formHcpcses.SelectedHcpcs.HcpcsCode;
				textCodeSystem.Text="HCPCS";
				textCodeValue.Text=formHcpcses.SelectedHcpcs.HcpcsCode;
				textCodeDescript.Text=formHcpcses.SelectedHcpcs.DescriptionShort;
			}
		}

		private void butCdt_Click(object sender,EventArgs e) {
			FormProcCodes formPCs=new FormProcCodes();
			formPCs.IsSelectionMode=true;
			if(formPCs.ShowDialog()==DialogResult.OK) {
				_encCur.CodeSystem="CDT";
				ProcedureCode procCode=ProcedureCodes.GetById(formPCs.SelectedCodeNum);
				_encCur.CodeValue=procCode.Code;
				textCodeSystem.Text="CDT";
				textCodeValue.Text=procCode.Code;
				textCodeDescript.Text=procCode.Description;
			}
		}

		private void butCpt_Click(object sender,EventArgs e) {
			FormCpts formCpts=new FormCpts();
			formCpts.IsSelectionMode=true;
			if(formCpts.ShowDialog()==DialogResult.OK) {
				_encCur.CodeSystem="CPT";
				_encCur.CodeValue=formCpts.SelectedCpt.CptCode;
				textCodeSystem.Text="CPT";
				textCodeValue.Text=formCpts.SelectedCpt.CptCode;
				textCodeDescript.Text=formCpts.SelectedCpt.Description;
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_encCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(MsgBox.Show(MsgBoxButtons.OKCancel,"Delete?")!=true) {
				return;
			}
			Encounters.Delete(_encCur.EncounterNum);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			//TODO: valid date box, validation here.
			if(textDateEnc.errorProvider1.GetError(textDateEnc)!="") {
				MessageBox.Show("You must enter a valid date");
				return;
			}
			_encCur.ProviderId=_provNumSelected;
			_encCur.Note=textNote.Text; //PIn.String(textNote.Text);
			_encCur.DateEncounter=PIn.Date(textDateEnc.Text);
			if(_encCur.CodeValue==null || _encCur.CodeSystem==null) {
				MessageBox.Show("You must select a code");
				return;
			}
			if(_encCur.ProviderId==0) { //Should never be hit, defaults to index 1
				MessageBox.Show("You must select a provider");
				return;
			}
			if(_encCur.IsNew){
				Encounters.Insert(_encCur);
			}
			else {
				Encounters.Update(_encCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}