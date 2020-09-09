using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using System.Linq;
using Imedisoft.Forms;

namespace OpenDental {
	public partial class FormDoseSpotAssignClinicId:ODForm {
		private List<Clinic> _listClinicsInComboBox;
		private ClinicErx _clinicErxCur;
    List<ProgramProperty> _listClinicIDs;
    List<ProgramProperty> _listClinicKeys;

    public FormDoseSpotAssignClinicId(long clinicErxNum) {
			InitializeComponent();
			_clinicErxCur=ClinicErxs.GetFirstOrDefault(x => x.ClinicErxNum==clinicErxNum);
			
		}

		private void FormDoseSpotAssignUserId_Load(object sender,EventArgs e) {
      _listClinicsInComboBox=Clinics.GetByCurrentUser();
      List<ProgramProperty> listProgramProperties=ProgramProperties.GetForProgram(Programs.GetCur(ProgramName.eRx).Id);
      _listClinicIDs=listProgramProperties.FindAll(x => x.Description==Erx.PropertyDescs.ClinicID);
      _listClinicKeys=listProgramProperties.FindAll(x => x.Description==Erx.PropertyDescs.ClinicKey);
      _listClinicsInComboBox.RemoveAll(x =>//Remove all clinics that already have a DoseSpot Clinic ID OR Clinic Key entered
        _listClinicIDs.FindAll(y => !string.IsNullOrWhiteSpace(y.Value)).Select(y => y.ClinicId).Contains(x.Id) 
        || _listClinicKeys.FindAll(y => !string.IsNullOrWhiteSpace(y.Value)).Select(y => y.ClinicId).Contains(x.Id)
      );
      FillComboBox();
			textClinicId.Text=_clinicErxCur.ClinicId;//ClinicID passed from Alert
      textClinicKey.Text=_clinicErxCur.ClinicKey;//ClinicKey passed from Alert
      textClinicDesc.Text=_clinicErxCur.ClinicDesc;//ClinicDesc passed from Alert
    }

		private void FillComboBox(long selectedClinicNum=-1) {
			comboClinics.Items.Clear();//this is not a comboBoxClinicPicker because the list of clinics is filtered. Combo is still full of Clinics.
			foreach(Clinic clinicCur in _listClinicsInComboBox) {
				comboClinics.Items.Add(clinicCur.Description,clinicCur);
				if(clinicCur.Id==selectedClinicNum) {
					comboClinics.SelectedIndex=comboClinics.Items.Count-1;//Select The item that was just added if it is the selected num.
				}
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(comboClinics.SelectedIndex==-1) {
				MessageBox.Show("Please select a clinic.");
				return;
			}
      _clinicErxCur.ClinicNum=comboClinics.GetSelected<Clinic>().Id;
      Program progErx=Programs.GetCur(ProgramName.eRx);
      ProgramProperty ppClinicID=_listClinicIDs.FirstOrDefault(x => x.ClinicId==_clinicErxCur.ClinicNum);
      if(ppClinicID==null) {
        ppClinicID=new ProgramProperty();
        ppClinicID.ProgramId=progErx.Id;
        ppClinicID.ClinicId=_clinicErxCur.ClinicNum;
        ppClinicID.Description=Erx.PropertyDescs.ClinicID;
        ppClinicID.Value=_clinicErxCur.ClinicId;
        ProgramProperties.Save(ppClinicID);
      }
      else {
        ppClinicID.Value=_clinicErxCur.ClinicId;
        ProgramProperties.Save(ppClinicID);
      }
      ProgramProperty ppClinicKey=_listClinicKeys.FirstOrDefault(x => x.ClinicId==_clinicErxCur.ClinicNum);
      if(ppClinicKey==null) {
        ppClinicKey=new ProgramProperty();
        ppClinicKey.ProgramId=progErx.Id;
        ppClinicKey.ClinicId=_clinicErxCur.ClinicNum;
        ppClinicKey.Description=Erx.PropertyDescs.ClinicKey;
        ppClinicKey.Value=_clinicErxCur.ClinicKey;
        ProgramProperties.Save(ppClinicKey);
      }
      else {
        ppClinicKey.Value=_clinicErxCur.ClinicKey;
        ProgramProperties.Save(ppClinicKey);
      }
      DataValid.SetInvalid(InvalidType.Programs);
      DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butClinicPick_Click(object sender,EventArgs e) {
      FormClinics FormC=new FormClinics(_listClinicsInComboBox);
      FormC.IsSelectionMode=true;
      FormC.ShowDialog();
      if(FormC.DialogResult!=DialogResult.OK) {
        return;
      }
			FillComboBox(FormC.SelectedClinic?.Id ?? 0);
		}
	}
}