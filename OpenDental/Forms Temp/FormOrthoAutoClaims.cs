using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using CodeBase;
using Imedisoft.Data;

namespace OpenDental {
	public partial class FormOrthoAutoClaims:ODForm {
		private DataTable _tableOutstandingAutoClaims;

		public FormOrthoAutoClaims() {
			InitializeComponent();
			
		}

		private void FormAutoOrtho_Load(object sender,EventArgs e) {
			_tableOutstandingAutoClaims=PatPlans.GetOutstandingOrtho();
			if(!Security.CurrentUser.ClinicIsRestricted) {
				comboClinics.IncludeAll=true;
			}
			if(comboClinics.IncludeAll && Clinics.ClinicId==null) {
				comboClinics.IsAllSelected=true;
			}
			else {
				comboClinics.SelectedClinicNum=Clinics.ClinicId??0;
			}
			FillGrid();
		}

		private void FillGrid() {
			long clinicNum = 0;
			if(!comboClinics.IsAllSelected) {
				clinicNum=comboClinics.SelectedClinicNum;
			}
			int clinicWidth=80;
			int patientWidth=180;
			int carrierWidth=220;
			if(PrefC.HasClinicsEnabled) {
				patientWidth=140;
				carrierWidth=200;
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col=new GridColumn("Patient",patientWidth);
			gridMain.Columns.Add(col);
			col=new GridColumn("Carrier",carrierWidth);
			gridMain.Columns.Add(col);
			col=new GridColumn("TxMonths",70);
			gridMain.Columns.Add(col);
			col=new GridColumn("Banding",80,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new GridColumn("MonthsRem",100);
			gridMain.Columns.Add(col);
			col=new GridColumn("#Sent",60);
			gridMain.Columns.Add(col);
			col=new GridColumn("LastSent",80,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new GridColumn("NextClaim",80,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) { //clinics is turned on
				col=new GridColumn("Clinic",clinicWidth,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			GridRow row;
			foreach(DataRow rowCur in _tableOutstandingAutoClaims.Rows) {
				//need a check for if clinics is on here
				if(PrefC.HasClinicsEnabled //Clinics are enabled
					&& (Security.CurrentUser.ClinicIsRestricted || !comboClinics.IsAllSelected) 
					&& clinicNum!=PIn.Long(rowCur["ClinicNum"].ToString()))   //currently selected clinic doesn't match the row's clinic
				{
					continue;
				}
				row=new GridRow();
				DateTime dateLastSeen=PIn.Date(rowCur["LastSent"].ToString());
				DateTime dateBanding=PIn.Date(rowCur["DateBanding"].ToString());
				DateTime dateNextClaim=PIn.Date(rowCur["OrthoAutoNextClaimDate"].ToString());
				DateSpan dateSpanMonthsRem=new DateSpan(PIn.Date(rowCur["DateBanding"].ToString()).AddMonths(PIn.Int(rowCur["MonthsTreat"].ToString())),DateTimeOD.Today);
				row.Cells.Add(PIn.String(rowCur["Patient"].ToString()));
				row.Cells.Add(PIn.String(rowCur["CarrierName"].ToString()));
				row.Cells.Add(PIn.String(rowCur["MonthsTreat"].ToString()));
				row.Cells.Add(dateBanding.Year < 1880 ? "" : dateBanding.ToShortDateString());//add blank if there is no banding
				if(dateBanding.Year < 1880) { //add blank if there is no banding
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(((dateSpanMonthsRem.YearsDiff * 12) + dateSpanMonthsRem.MonthsDiff)+" "+"months"
					+", "+dateSpanMonthsRem.DaysDiff +" "+"days");
				}
				row.Cells.Add(PIn.String(rowCur["NumSent"].ToString()));
				row.Cells.Add(dateLastSeen.Year < 1880 ? "" : dateLastSeen.ToShortDateString());
				row.Cells.Add(dateNextClaim.Year < 1880 ? "" : dateNextClaim.ToShortDateString());
				if(PrefC.HasClinicsEnabled) { //clinics is turned on
					//Use the long list of clinics so that hidden clinics can be shown for unrestricted users.
					row.Cells.Add(Clinics.GetAbbr(PIn.Long(rowCur["ClinicNum"].ToString())));
				}
				row.Tag=rowCur;
				gridMain.Rows.Add(row);

			}
			gridMain.EndUpdate();
		}

		private void butGenerateClaims_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Count() < 1) {
				MessageBox.Show("Please select the rows for which you would like to create procedures and claims.");
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Are you sure you want to generate claims and procedures for all patients and insurance plans?")) {
				return;
			}
			List<long> listPlanNums = new List<long>();
			List<long> listPatPlanNums = new List<long>();
			List<long> listInsSubNums = new List<long>();
			for(int i = 0;i < gridMain.SelectedIndices.Count();i++) {
				DataRow rowCur =(DataRow)gridMain.Rows[gridMain.SelectedIndices[i]].Tag;
				listPlanNums.Add(PIn.Long(rowCur["PlanNum"].ToString()));
				listPatPlanNums.Add(PIn.Long(rowCur["PatPlanNum"].ToString()));
				listInsSubNums.Add(PIn.Long(rowCur["InsSubNum"].ToString()));
			}
			List<InsurancePlan> listSelectedInsPlans=InsPlans.GetPlans(listPlanNums);
			List<PatPlan> listSelectedPatPlans=PatPlans.GetPatPlans(listPatPlanNums);
			List<InsSub> listSelectedInsSubs=InsSubs.GetMany(listInsSubNums);
			List<DataRow> rowsSucceeded=new List<DataRow>();
			int rowsFailed=0;
			List<Benefit> listBenefitsAll=Benefits.Refresh(listSelectedPatPlans,listSelectedInsSubs);
			for(int i = 0;i < gridMain.SelectedIndices.Count();i++) {
				try {
					DataRow rowCur =(DataRow)gridMain.Rows[gridMain.SelectedIndices[i]].Tag;
					long patNumCur = PIn.Long(rowCur["PatNum"].ToString());
					Patient patCur = Patients.GetPat(patNumCur);
					PatientNote patNoteCur = PatientNotes.Refresh(patNumCur,patCur.Guarantor);
					long codeNumCur = PIn.Long(rowCur["AutoCodeNum"].ToString());
					long provNumCur = PIn.Long(rowCur["ProvNum"].ToString());
					long clinicNumCur = PIn.Long(rowCur["ClinicNum"].ToString());
					long insPlanNumCur = PIn.Long(rowCur["PlanNum"].ToString());
					long patPlanNumCur = PIn.Long(rowCur["PatPlanNum"].ToString());
					long insSubNumCur = PIn.Long(rowCur["InsSubNum"].ToString());
					int monthsTreat = PIn.Int(rowCur["MonthsTreat"].ToString());
					DateTime dateDue =  PIn.Date(rowCur["OrthoAutoNextClaimDate"].ToString());
					//for each selected row
					//create a procedure
					//Procedures.CreateProcForPat(patNumCur,codeNumCur,"","",ProcStat.C,provNumCur);
					Procedure proc = Procedures.CreateOrthoAutoProcsForPat(patNumCur,codeNumCur,provNumCur,clinicNumCur,dateDue);
					InsurancePlan insPlanCur = InsPlans.GetPlan(insPlanNumCur,listSelectedInsPlans);
					PatPlan patPlanCur = listSelectedPatPlans.FirstOrDefault(x => x.PatPlanNum == patPlanNumCur);
					InsSub insSubCur = listSelectedInsSubs.FirstOrDefault(x => x.InsSubNum==insSubNumCur);
					List<Benefit> benefitList=listBenefitsAll.FindAll(x => x.PatPlanNum==patPlanCur.PatPlanNum || x.PlanNum==insSubCur.PlanNum);
					//create a claimproc
					List<ClaimProc> listClaimProcs=new List<ClaimProc>();
					Procedures.ComputeEstimates(proc,patNumCur,ref listClaimProcs,true,new List<InsurancePlan> { insPlanCur },
						new List<PatPlan> { patPlanCur },benefitList,null,null,true,patCur.Age,new List<InsSub> { insSubCur },isForOrtho:true);
					//make the feebilled == the insplan feebilled or patplan feebilled
					double feebilled = patPlanCur.OrthoAutoFeeBilledOverride == -1 ? insPlanCur.OrthoAutoFeeBilled : patPlanCur.OrthoAutoFeeBilledOverride;
					//create a claim with that claimproc
					string claimType="";
					switch(patPlanCur.Ordinal) {
						case 1:
							claimType="P";
							break;
						case 2:
							claimType="S";
							break;
					}
					DateSpan dateSpanMonthsRem=new DateSpan(PIn.Date(rowCur["DateBanding"].ToString()).AddMonths(PIn.Int(rowCur["MonthsTreat"].ToString())),DateTimeOD.Today);
					Claims.CreateClaimForOrthoProc(claimType,patPlanCur,insPlanCur,insSubCur,
						ClaimProcs.GetForProcWithOrdinal(proc.ProcNum,patPlanCur.Ordinal),proc,feebilled,PIn.Date(rowCur["DateBanding"].ToString()),
						PIn.Int(rowCur["MonthsTreat"].ToString()),((dateSpanMonthsRem.YearsDiff * 12) + dateSpanMonthsRem.MonthsDiff));
					PatPlans.IncrementOrthoNextClaimDates(patPlanCur,insPlanCur,monthsTreat,patNoteCur);
					rowsSucceeded.Add(rowCur);
					SecurityLogs.MakeLogEntry(Permissions.ProcComplCreate,patCur.PatNum
						,"Automatic ortho procedure and claim generated for"+" "+dateDue.ToShortDateString());
				}
				catch(Exception) {
					rowsFailed++;
				}
			}
			string message="Done."+" "+"There were"+" "+rowsSucceeded.Count+" "
					+"claim(s) generated and"+" "+rowsFailed+" "+"failures"+".";
				MessageBox.Show(message);
				foreach(DataRow row in rowsSucceeded) {
					_tableOutstandingAutoClaims.Rows.Remove(row);
				}
				FillGrid();

		}

		private void butSelectAll_Click(object sender,EventArgs e) {
			gridMain.SetSelected(true);
		}

		private void comboClinics_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}
	}
}