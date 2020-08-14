using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDental.ReportingComplex;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	///<summary></summary>
	public partial class FormRpBrokenAppointments:ODForm {

		private List<Clinic> _listClinics;
		private List<Def> _listPosAdjTypes=new List<Def>();
		private List<BrokenApptProcedure> _listBrokenProcOptions=new List<BrokenApptProcedure>();
		private List<Provider> _listProviders;
		private bool _hasClinicsEnabled;

		///<summary></summary>
		public FormRpBrokenAppointments() {
			InitializeComponent();
			
		}

		private void FormRpBrokenAppointments_Load(object sender,EventArgs e) {
			if(PrefC.HasClinicsEnabled) {
				_hasClinicsEnabled=true;
			}
			else {
				_hasClinicsEnabled=false;
			}
			_listProviders=Providers.GetListReports();
			dateStart.SelectionStart=DateTime.Today;
			dateEnd.SelectionStart=DateTime.Today;
			for(int i=0;i<_listProviders.Count;i++) {
				listProvs.Items.Add(_listProviders[i].GetLongDesc());
			}
			if(!_hasClinicsEnabled) {
				listClinics.Visible=false;
				labelClinics.Visible=false;
				checkAllClinics.Visible=false;
			}
			else {
				_listClinics=Clinics.GetForUserod(Security.CurrentUser);
				if(!Security.CurrentUser.ClinicIsRestricted) {
					listClinics.Items.Add("Unassigned");
					listClinics.SetSelected(0,true);
				}
				for(int i=0;i<_listClinics.Count;i++) {
					int curIndex=listClinics.Items.Add(_listClinics[i].Abbr);
					if(Clinics.ClinicNum==0) {
						listClinics.SetSelected(curIndex,true);
						checkAllClinics.Checked=true;
					}
					if(_listClinics[i].ClinicNum==Clinics.ClinicNum) {
						listClinics.SelectedIndices.Clear();
						listClinics.SetSelected(curIndex,true);
					}
				}
			}
			int value=PrefC.GetInt(PrefName.BrokenApptProcedure);
			if(value==(int)BrokenApptProcedure.None) {//
				radioProcs.Visible=false;
			}
			if(value>0){
				radioProcs.Checked=true;
			}
			else if(Prefs.GetBool(PrefName.BrokenApptAdjustment)) {
				radioAdj.Checked=true;
			}
			else {
				radioAptStatus.Checked=true;
			}
		}

		private void checkAllProvs_Click(object sender,EventArgs e) {
			if(checkAllProvs.Checked) {
				listProvs.SelectedIndices.Clear();
			}
		}

		private void checkAllClinics_Click(object sender,EventArgs e) {
			if(checkAllClinics.Checked) {
				for(int i=0;i<listClinics.Items.Count;i++) {
					listClinics.SetSelected(i,true);
				}
			}
			else {
				listClinics.SelectedIndices.Clear();
			}
		}

		private void listProvs_Click(object sender,EventArgs e) {
			if(listProvs.SelectedIndices.Count>0) {
				checkAllProvs.Checked=false;
			}
		}

		private void listClinics_Click(object sender,EventArgs e) {
			if(listClinics.SelectedIndices.Count>0) {
				checkAllClinics.Checked=false;
			}
		}

		private void radioProcs_CheckedChanged(object sender,EventArgs e) {
			if(radioProcs.Checked) {
				listOptions.Items.Clear();
				listOptions.SelectionMode=SelectionMode.One;
				int index=0;
				_listBrokenProcOptions.Clear();
				BrokenApptProcedure brokenApptCodeDB=(BrokenApptProcedure)PrefC.GetInt(PrefName.BrokenApptProcedure);
				switch(brokenApptCodeDB) {
					case BrokenApptProcedure.None:
					case BrokenApptProcedure.Missed:
						_listBrokenProcOptions.Add(BrokenApptProcedure.Missed);
						index=listOptions.Items.Add(brokenApptCodeDB.ToString()+": (D9986)");
						labelDescr.Text="Broken appointments based on ADA code D9986";
					break;
					case BrokenApptProcedure.Cancelled:
						_listBrokenProcOptions.Add(BrokenApptProcedure.Cancelled);
						index=listOptions.Items.Add(brokenApptCodeDB.ToString()+": (D9987)");
						labelDescr.Text="Broken appointments based on ADA code D9987";
					break;
					case BrokenApptProcedure.Both:
						_listBrokenProcOptions.Add(BrokenApptProcedure.Missed);
						_listBrokenProcOptions.Add(BrokenApptProcedure.Cancelled);
						_listBrokenProcOptions.Add(BrokenApptProcedure.Both);
						listOptions.Items.Add(BrokenApptProcedure.Missed.ToString()+": (D9986)");
						listOptions.Items.Add(BrokenApptProcedure.Cancelled.ToString()+": (D9987)");
						index=listOptions.Items.Add(brokenApptCodeDB.ToString());
						labelDescr.Text="Broken appointments based on ADA code D9986 or D9987";
					break;
				}
				listOptions.SetSelected(index,true);
				listOptions.Visible=true;
			}
		}

		private void radioAdj_CheckedChanged(object sender,EventArgs e) {
			if(radioAdj.Checked) {
				labelDescr.Text="Broken appointments based on broken appointment adjustments";
				listOptions.Items.Clear();
				_listPosAdjTypes.Clear();
				listOptions.SelectionMode=SelectionMode.MultiSimple;
				_listPosAdjTypes=Defs.GetPositiveAdjTypes();
				long brokenApptAdjDefNum=Prefs.GetLong(PrefName.BrokenAppointmentAdjustmentType);
				for(int i=0; i<_listPosAdjTypes.Count;i++) {
					listOptions.Items.Add(_listPosAdjTypes[i].ItemName);
					if(_listPosAdjTypes[i].DefNum==brokenApptAdjDefNum) {
						listOptions.SelectedIndices.Add(i);
					}
				}
				listOptions.Visible=true;
			}
			else {
				listOptions.Visible=false;
			}
		}

		private void radioAptStatus_CheckedChanged(object sender,EventArgs e) {
			if(radioAptStatus.Checked) {
				labelDescr.Text="Broken appointments based on appointment status";
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!checkAllProvs.Checked && listProvs.SelectedIndices.Count==0) {
				MessageBox.Show("At least one provider must be selected.");
				return;
			}
			if(_hasClinicsEnabled) {
				if(!checkAllClinics.Checked && listClinics.SelectedIndices.Count==0) {
					MessageBox.Show("At least one clinic must be selected.");
					return;
				}
			}
			if(radioAdj.Checked && listOptions.SelectedIndices.Count==0) {
				MessageBox.Show("At least one adjustment type must be selected.");
				return;
			}
			if(radioProcs.Checked && listOptions.SelectedIndices.Count==0) {
				MessageBox.Show("At least one procedure code option must be selected.");
				return;
			}
			List<long> listClinicNums=new List<long>();
			for(int i=0;i<listClinics.SelectedIndices.Count;i++) {
				if(Security.CurrentUser.ClinicIsRestricted) {
						listClinicNums.Add(_listClinics[listClinics.SelectedIndices[i]].ClinicNum);//we know that the list is a 1:1 to _listClinics
					}
				else {
					if(listClinics.SelectedIndices[i]==0) {
						listClinicNums.Add(0);
					}
					else {
						listClinicNums.Add(_listClinics[listClinics.SelectedIndices[i]-1].ClinicNum);//Minus 1 from the selected index
					}
				}
			}
			List<long> listProvNums=new List<long>();
			if(checkAllProvs.Checked) {
				for(int i = 0;i<_listProviders.Count;i++) {
					listProvNums.Add(_listProviders[i].ProvNum);
				}
			}
			else {
				for(int i=0;i<listProvs.SelectedIndices.Count;i++) {
				listProvNums.Add(_listProviders[listProvs.SelectedIndices[i]].ProvNum);
				}
			}
			List<long> listAdjDefNums=new List<long>();
			if(radioAdj.Checked) {
				for(int i=0;i<listOptions.SelectedIndices.Count;i++) {
					listAdjDefNums.Add(_listPosAdjTypes[listOptions.SelectedIndices[i]].DefNum);
				}
			}
			BrokenApptProcedure brokenApptSelection=BrokenApptProcedure.None;
			if(radioProcs.Checked) {
				brokenApptSelection=_listBrokenProcOptions[listOptions.SelectedIndex];
			}
			ReportComplex reportComplex=new ReportComplex(true,false);
			DataTable table = new DataTable();
			table=RpBrokenAppointments.GetBrokenApptTable(dateStart.SelectionStart,dateEnd.SelectionStart,listProvNums,listClinicNums,listAdjDefNums,brokenApptSelection
				,checkAllClinics.Checked,radioProcs.Checked,radioAptStatus.Checked,radioAdj.Checked,_hasClinicsEnabled);
			string subtitleProvs="";
			string subtitleClinics="";
			if(checkAllProvs.Checked) {
				subtitleProvs="All Providers";
			}
			else {
				for(int i=0;i<listProvs.SelectedIndices.Count;i++) {
					if(i>0) {
						subtitleProvs+=", ";
					}
					subtitleProvs+=_listProviders[listProvs.SelectedIndices[i]].Abbr;
				}
			}
			if(_hasClinicsEnabled) {
				if(checkAllClinics.Checked) {
					subtitleClinics="All Clinics";
				}
				else {
					for(int i=0;i<listClinics.SelectedIndices.Count;i++) {
						if(i>0) {
							subtitleClinics+=", ";
						}
						if(Security.CurrentUser.ClinicIsRestricted) {
							subtitleClinics+=_listClinics[listClinics.SelectedIndices[i]].Abbr;
						}
						else {
							if(listClinics.SelectedIndices[i]==0) {
								subtitleClinics+="Unassigned";
							}
							else {
								subtitleClinics+=_listClinics[listClinics.SelectedIndices[i]-1].Abbr;//Minus 1 from the selected index
							}
						}
					}
				}
			}
			Font font=new Font("Tahoma",10);
			Font fontBold=new Font("Tahoma",10,FontStyle.Bold);
			Font fontTitle=new Font("Tahoma",17,FontStyle.Bold);
			Font fontSubTitle=new Font("Tahoma",11,FontStyle.Bold);
			reportComplex.ReportName="Broken Appointments";
			reportComplex.AddTitle("Title","Broken Appointments",fontTitle);
			if(radioProcs.Checked) {//Report looking at ADA procedure code D9986
				string codes="";
				switch(brokenApptSelection) {
					case BrokenApptProcedure.None:
					case BrokenApptProcedure.Missed:
						codes="D9986";
					break;
					case BrokenApptProcedure.Cancelled:
						codes="D9987";
					break;
					case BrokenApptProcedure.Both:
						codes="D9986 or D9987";
					break;
				}
				reportComplex.AddSubTitle("Report Description","By ADA Code "+codes,fontSubTitle);
			}
			else if(radioAdj.Checked) {//Report looking at broken appointment adjustments
				reportComplex.AddSubTitle("Report Description","By Broken Appointment Adjustment",fontSubTitle);
			}
			else {//Report looking at appointments with a status of 'Broken'
				reportComplex.AddSubTitle("Report Description","By Appointment Status",fontSubTitle);
			}
			reportComplex.AddSubTitle("Providers",subtitleProvs,fontSubTitle);
			reportComplex.AddSubTitle("Clinics",subtitleClinics,fontSubTitle);
			QueryObject queryObject;
			if(PrefC.HasClinicsEnabled) {//Split the query up by clinics.
				queryObject=reportComplex.AddQuery(table,"Date"+": "+DateTimeOD.Today.ToString("d"),"ClinicDesc",SplitByKind.Value,0,true);
			}
			else {
				queryObject=reportComplex.AddQuery(table,"Date"+": "+DateTimeOD.Today.ToString("d"),"",SplitByKind.None,0,true);
			}
			//Add columns to report
			if(radioProcs.Checked) {//Report looking at ADA procedure code D9986 or D9987
				queryObject.AddColumn("Date",85,FieldValueType.Date,font);
				queryObject.AddColumn("Provider",180,FieldValueType.String,font);
				if(brokenApptSelection==BrokenApptProcedure.Both) {
					queryObject.AddColumn("Code",75,FieldValueType.String,font);
				}
				queryObject.AddColumn("Patient",220,FieldValueType.String,font);
				queryObject.AddColumn("Fee",200,FieldValueType.Number,font);
				queryObject.AddGroupSummaryField("Total Broken Appointment Fees"+":","Fee","ProcFee",SummaryOperation.Sum,
					font:fontBold,offSetX:0,offSetY:10);
				queryObject.AddGroupSummaryField("Total Broken Appointments"+":","Fee","ProcFee",SummaryOperation.Count,
					font:fontBold,offSetX:0,offSetY:10);
			}
			else if(radioAdj.Checked) {//Report looking at broken appointment adjustments
				queryObject.AddColumn("Date",85,FieldValueType.Date,font);
				queryObject.AddColumn("Provider",100,FieldValueType.String,font);
				queryObject.AddColumn("Patient",220,FieldValueType.String,font);
				queryObject.AddColumn("Amount",80,FieldValueType.Number,font);
				queryObject.AddColumn("Note",300,FieldValueType.String,font);
				queryObject.AddGroupSummaryField("Total Broken Appointment Adjustment Amount"+":",
					"Amount","AdjAmt",SummaryOperation.Sum,font:fontBold,offSetX:0,offSetY:10);
				queryObject.AddGroupSummaryField("Total Broken Appointments"+":",
					"Amount","AdjAmt",SummaryOperation.Count,font:fontBold,offSetX:0,offSetY:10);
			}
			else {//Report looking at appointments with a status of 'Broken'
				queryObject.AddColumn("AptDate",85,FieldValueType.Date,font);
				queryObject.AddColumn("Patient",220,FieldValueType.String,font);
				queryObject.AddColumn("Doctor",165,FieldValueType.String,font);
				queryObject.AddColumn("Hygienist",165,FieldValueType.String,font);
				queryObject.AddColumn("IsHyg",50,FieldValueType.Boolean,font);
				queryObject.GetColumnDetail("IsHyg").ContentAlignment = ContentAlignment.MiddleCenter;
				queryObject.AddGroupSummaryField("Total Broken Appointments"+":","IsHyg","AptDateTime",SummaryOperation.Count,
					font:fontBold,offSetX:0,offSetY:10);
			}
			queryObject.ContentAlignment=ContentAlignment.MiddleRight;
			reportComplex.AddPageNum(font);
			//execute query
			if(!reportComplex.SubmitQueries()) {
				return;
			}
			//display report
			FormReportComplex formReportComplex=new FormReportComplex(reportComplex);
			//FormR.MyReport=report;
			formReportComplex.ShowDialog();
			font.Dispose();
			fontBold.Dispose();
			fontTitle.Dispose();
			fontSubTitle.Dispose();
			DialogResult=DialogResult.OK;
		}

	}
}