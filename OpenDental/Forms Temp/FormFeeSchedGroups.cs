using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using OpenDental.UI;
using System.Linq;

namespace OpenDental {
	public partial class FormFeeSchedGroups:ODForm {
		///<summary>All clinics in the cache.</summary>
		private List<Clinic> _listAllClinics;
		///<summary>List of all clinics for the selected FeeSchedGroup in the grid.  Used to fill gridClinics.</summary>
		private List<Clinic> _listClinicsForGroup=new List<Clinic>();
		///<summary>List of all FeeSchedGroups in db.</summary>
		private List<FeeScheduleGroup> _listFeeSchedGroups;
		///<summary>List of all FeeSchedGroups that will populate the grid. This is a filtered version of the list retrieved from the cache.</summary>
		private List<FeeScheduleGroup> _listFeeSchedGroupsFiltered;

		public FormFeeSchedGroups() {
			InitializeComponent();
			
		}

		private void FormFeeSchedGroups_Load(object sender,EventArgs e) {
			SetFilterControlsAndAction(() => FilterFeeSchedGroups(),textFeeSched);
			//No restricting clinics because this window assumes that the user is an admin without restricted clinics
			_listAllClinics=Clinics.Where(x => x.Id > -1 && x.IsHidden==false).OrderBy(x => x.Abbr).ToList(); //Get all Clinics from cache that are not hidden
			_listFeeSchedGroups=FeeSchedGroups.GetAll().OrderBy(x => x.Description).ToList();
			_listFeeSchedGroupsFiltered=_listFeeSchedGroups.DeepCopyList<FeeScheduleGroup,FeeScheduleGroup>();
			FillClinicCombo();
			FilterFeeSchedGroups();
		}

		private void FillClinicCombo() {
			comboClinic.Items.Clear();
			comboClinic.Items.Add("All");
			foreach(Clinic clinic in _listAllClinics){
				comboClinic.Items.Add(clinic.Abbr);
			}
			comboClinic.SelectedIndex=0;
		}

		//Used by comboClinic to filter the list of FeeSchedGroups
		private void comboClinic_SelectionChanged(object sender,EventArgs e) {
			FilterFeeSchedGroups();
		}

		private void FilterFeeSchedGroups() {
			List<FeeSchedule> listFilteredFeeScheds=FeeScheds.GetWhere(x => x.Description.ToLower().Contains(textFeeSched.Text.ToLower()));
			//Clinic filter will be either a list of all clinics or a list containing only the selected clinic
			List<Clinic> listFilteredClinics=(comboClinic.SelectedIndex==0) ? _listAllClinics : _listAllClinics[comboClinic.SelectedIndex-1].SingleItemToList();
			//This filter should return everything if both filters are empty.
			_listFeeSchedGroupsFiltered=_listFeeSchedGroups
				.Where(x => x.FeeScheduleId.In(listFilteredFeeScheds.Select(y => y.Id)))
				.Where(x => x.ListClinicNumsAll.Any(y => y.In(listFilteredClinics.Select(z => z.Id))))
				.ToList();
			FillGridGroups();
			FillGridClinics();
		}

		private void FillGridGroups() {
			gridGroups.BeginUpdate();
			gridGroups.Columns.Clear();
			GridColumn col;
			col=new GridColumn("Group Name",200);
			gridGroups.Columns.Add(col);
			col=new GridColumn("Fee Schedule",75);
			gridGroups.Columns.Add(col);
			gridGroups.Rows.Clear();
			GridRow row;
			foreach(FeeScheduleGroup feeSchedGroupCur in _listFeeSchedGroupsFiltered) {
				row=new GridRow();
				row.Cells.Add(feeSchedGroupCur.Description);
				row.Cells.Add(FeeScheds.GetDescription(feeSchedGroupCur.FeeScheduleId));//Returns empty string if the FeeSched couldn't be found.
				row.Tag=feeSchedGroupCur;
				gridGroups.Rows.Add(row);
			} 
			gridGroups.EndUpdate();
		}

		private void FillGridClinics() {
			_listClinicsForGroup.Clear();
			if(gridGroups.GetSelectedIndex()>=0) {
				_listClinicsForGroup=Clinics.GetClinics(gridGroups.SelectedTag<FeeScheduleGroup>().ListClinicNumsAll).OrderBy(x => x.Abbr).ToList();
			}
			gridClinics.BeginUpdate();
			gridClinics.Columns.Clear();
			GridColumn col;
			col=new GridColumn("Abbr",100){ IsWidthDynamic=true };
			gridClinics.Columns.Add(col);
			col=new GridColumn("Description",100){ IsWidthDynamic=true,DynamicWeight=2 };
			gridClinics.Columns.Add(col);
			gridClinics.Rows.Clear();
			GridRow row;
			foreach(Clinic clinicCur in _listClinicsForGroup) {
				row=new GridRow();
				row.Cells.Add(clinicCur.Abbr);
				row.Cells.Add(clinicCur.Description+(clinicCur.IsHidden?" (Hidden)":""));
				row.Tag=clinicCur;
				gridClinics.Rows.Add(row);
			}
			gridClinics.EndUpdate();
		}

		private void gridGroups_CellClick(object sender,ODGridClickEventArgs e) {
			FillGridClinics();
		}

		private void gridGroups_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			FeeScheduleGroup feeSchedGroupCur=(FeeScheduleGroup)gridGroups.Rows[e.Row].Tag;
			FormFeeSchedGroupEdit formFG=new FormFeeSchedGroupEdit(feeSchedGroupCur);
			formFG.ShowDialog();
			if(formFG.DialogResult==DialogResult.OK) {
				FeeSchedGroups.Update(feeSchedGroupCur);
			}
			//Still need to refresh incase the user deleted the FeeSchedGroup, since it returns DialogResult.Cancel.
			FilterFeeSchedGroups();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FeeScheduleGroup feeSchedGroupNew=new FeeScheduleGroup(){ ListClinicNumsAll=new List<long>(), IsNew=true };
			FormFeeSchedGroupEdit formFG=new FormFeeSchedGroupEdit(feeSchedGroupNew);
			formFG.ShowDialog();
			if(formFG.DialogResult==DialogResult.OK) {
				FeeSchedGroups.Insert(feeSchedGroupNew);
				_listFeeSchedGroups.Add(feeSchedGroupNew);
				_listFeeSchedGroups=_listFeeSchedGroups.OrderBy(x => x.Description).ToList();
				FilterFeeSchedGroups();
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}