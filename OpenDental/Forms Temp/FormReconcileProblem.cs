using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using Imedisoft.Data.Models;
using Imedisoft.Data;
using System.Linq;

namespace OpenDental {
	public partial class FormReconcileProblem:ODForm {
		public List<ProblemDefinition> ListProblemDefNew;
		public List<Problem> ListProblemNew;
		private List<Problem> _listProblemReconcile;
		private List<ProblemDefinition> _listProblemDefCur;
		private List<Problem> _listProblemCur;
		private Patient _patCur;

		///<summary>Patient must be valid.  Do not pass null.</summary>
		public FormReconcileProblem(Patient patCur) {
			InitializeComponent();
			
			_patCur=patCur;
		}

		private void FormReconcileProblem_Load(object sender,EventArgs e) {
			for(int index=0;index<ListProblemNew.Count;index++) {
				ListProblemNew[index].PatientId=_patCur.PatNum;
			}
			FillExistingGrid();//Done first so that _listReconcileCur and _listReconcileDefCur are populated.
			_listProblemReconcile=new List<Problem>(_listProblemCur);
			#region Delete After Testing
			//-------------------------------Delete after testing
			//ListProblemNew=new List<Disease>();
			//Disease dis=new Disease();
			//dis.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(3));
			//dis.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(3));
			//dis.PatNum=PatCur.PatNum;
			//dis.ProbStatus=0;
			//dis.PatNote="Terrible";
			//dis.IsNew=true;
			//ListProblemNew.Add(dis);
			//dis=new Disease();
			//dis.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(5));
			//dis.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(5));
			//dis.PatNum=PatCur.PatNum;
			//dis.ProbStatus=0;
			//dis.PatNote="Deadly";
			//dis.IsNew=true;
			//ListProblemNew.Add(dis);
			//dis=new Disease();
			//dis.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(7));
			//dis.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(7));
			//dis.PatNum=PatCur.PatNum;
			//dis.ProbStatus=0;
			//dis.PatNote="Other";
			//dis.IsNew=true;
			//ListProblemNew.Add(dis);
			//dis=new Disease();
			//dis.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(11));
			//dis.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(11));
			//dis.PatNum=PatCur.PatNum;
			//dis.ProbStatus=0;
			//dis.PatNote="Can't Think";
			//dis.IsNew=true;
			//ListProblemNew.Add(dis);
			//dis=new Disease();
			//dis.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(4));
			//dis.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(4));
			//dis.PatNum=PatCur.PatNum;
			//dis.ProbStatus=0;
			//dis.PatNote="What is Next!";
			//dis.IsNew=true;
			//ListProblemNew.Add(dis);
			//dis=new Disease();
			//dis.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(2));
			//dis.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(2));
			//dis.PatNum=PatCur.PatNum;
			//dis.ProbStatus=0;
			//dis.PatNote="Hmmmm...";
			//dis.IsNew=true;
			//ListProblemNew.Add(dis);
			//dis=new Disease();
			//dis.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(1));
			//dis.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(1));
			//dis.PatNum=PatCur.PatNum;
			//dis.ProbStatus=0;
			//dis.PatNote="Otherthly";
			//dis.IsNew=true;
			//ListProblemNew.Add(dis);
			//dis=new Disease();
			//dis.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(6));
			//dis.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(6));
			//dis.PatNum=PatCur.PatNum;
			//dis.ProbStatus=0;
			//dis.PatNote="Dependant";
			//dis.IsNew=true;
			//ListProblemNew.Add(dis);
			//dis=new Disease();
			//dis.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(8));
			//dis.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(8));
			//dis.PatNum=PatCur.PatNum;
			//dis.ProbStatus=0;
			//dis.PatNote="Shifty";
			//dis.IsNew=true;
			//ListProblemNew.Add(dis);
			//ListProblemDefNew=new List<DiseaseDef>();
			//DiseaseDef disD=new DiseaseDef();
			//disD.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(3));
			//disD.DiseaseName="Totally Preggers";
			//disD.SnomedCode="54116654";
			//disD.IsHidden=false;
			//disD.IsNew=true;
			//ListProblemDefNew.Add(disD);
			//disD=new DiseaseDef();
			//disD.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(5));
			//disD.DiseaseName="Paraplegic";
			//disD.SnomedCode="4651561";
			//disD.IsHidden=false;
			//disD.IsNew=true;
			//ListProblemDefNew.Add(disD);
			//disD=new DiseaseDef();
			//disD.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(7));
			//disD.DiseaseName="HIV/AIDS";
			//disD.SnomedCode="2165";
			//disD.IsHidden=false;
			//disD.IsNew=true;
			//ListProblemDefNew.Add(disD);
			//disD=new DiseaseDef();
			//disD.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(11));
			//disD.DiseaseName="Milk Addict";
			//disD.SnomedCode="16544633";
			//disD.IsHidden=false;
			//disD.IsNew=true;
			//ListProblemDefNew.Add(disD);
			//disD=new DiseaseDef();
			//disD.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(4));
			//disD.DiseaseName="Munchies";
			//disD.SnomedCode="41842384";
			//disD.IsHidden=false;
			//disD.IsNew=true;
			//ListProblemDefNew.Add(disD);
			//disD=new DiseaseDef();
			//disD.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(2));
			//disD.DiseaseName="Gaddafid";
			//disD.SnomedCode="48416321";
			//disD.IsHidden=false;
			//disD.IsNew=true;
			//ListProblemDefNew.Add(disD);
			//disD=new DiseaseDef();
			//disD.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(1));
			//disD.DiseaseName="D-Tosh Disease";
			//disD.SnomedCode="1847913";
			//disD.IsHidden=false;
			//disD.IsNew=true;
			//ListProblemDefNew.Add(disD);
			//disD=new DiseaseDef();
			//disD.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(6));
			//disD.DiseaseName="Uncontrollable Hiccups";
			//disD.SnomedCode="486316";
			//disD.IsHidden=false;
			//disD.IsNew=true;
			//ListProblemDefNew.Add(disD);
			//disD=new DiseaseDef();
			//disD.DateTStamp=DateTime.Now.Subtract(TimeSpan.FromDays(8));
			//disD.DiseaseName="Explosive Diarrhea";
			//disD.SnomedCode="9874954165";
			//disD.IsHidden=false;
			//disD.IsNew=true;
			//ListProblemDefNew.Add(disD);
			//-------------------------------
			#endregion
			//Automation to initially fill reconcile grid with "recommended" rows.
			bool isValid;
			for(int i=0;i<ListProblemNew.Count;i++) {
				isValid=true;
				for(int j=0;j<_listProblemDefCur.Count;j++) {
					if(_listProblemDefCur[j].CodeSnomed==ListProblemDefNew[i].CodeSnomed) {
						isValid=false;
						break;
					}
				}
				if(isValid) {
					_listProblemReconcile.Add(ListProblemNew[i]);
				}
			}
			FillImportGrid();
			FillReconcileGrid();
		}

		private void FillImportGrid()
		{
			gridProbImport.BeginUpdate();
			gridProbImport.Columns.Clear();
			GridColumn col = new GridColumn("Last Modified", 100, HorizontalAlignment.Center);
			gridProbImport.Columns.Add(col);
			col = new GridColumn("Date Start", 100, HorizontalAlignment.Center);
			gridProbImport.Columns.Add(col);
			col = new GridColumn("Problem Name", 200);
			gridProbImport.Columns.Add(col);
			col = new GridColumn("Status", 80, HorizontalAlignment.Center);
			gridProbImport.Columns.Add(col);
			gridProbImport.Rows.Clear();
			GridRow row;
			for (int i = 0; i < ListProblemNew.Count; i++)
			{
				row = new GridRow();
				row.Cells.Add(DateTime.Now.ToShortDateString());
				row.Cells.Add(ListProblemNew[i].DateStart?.ToShortDateString() ?? "");
				
				if (ListProblemDefNew[i].Description == null)
				{
					row.Cells.Add("");
				}
				else
				{
					row.Cells.Add(ListProblemDefNew[i].Description);
				}
				if (ListProblemNew[i].Status == ProblemStatus.Active)
				{
					row.Cells.Add("Active");
				}
				else if (ListProblemNew[i].Status == ProblemStatus.Resolved)
				{
					row.Cells.Add("Resolved");
				}
				else
				{
					row.Cells.Add("Inactive");
				}
				gridProbImport.Rows.Add(row);
			}
			gridProbImport.EndUpdate();
		}

		private void FillExistingGrid() {
			gridProbExisting.BeginUpdate();
			gridProbExisting.Columns.Clear();
			GridColumn col=new GridColumn("Last Modified",100,HorizontalAlignment.Center);
			gridProbExisting.Columns.Add(col);
			col=new GridColumn("Date Start",100,HorizontalAlignment.Center);
			gridProbExisting.Columns.Add(col);
			col=new GridColumn("Problem Name",200);
			gridProbExisting.Columns.Add(col);
			col=new GridColumn("Status",80,HorizontalAlignment.Center);
			gridProbExisting.Columns.Add(col);
			gridProbExisting.Rows.Clear();
			_listProblemCur=Problems.GetByPatient(_patCur.PatNum,true).ToList();
			List<long> problemDefNums=new List<long>();
			for(int h=0;h<_listProblemCur.Count;h++) {
				if(_listProblemCur[h].ProblemDefId > 0) {
					problemDefNums.Add(_listProblemCur[h].ProblemDefId);
				}
			}
			_listProblemDefCur=ProblemDefinitions.GetMultDiseaseDefs(problemDefNums);
			GridRow row;
			ProblemDefinition disD;
			for(int i=0;i<_listProblemCur.Count;i++) {
				row=new GridRow();
				disD=new ProblemDefinition();
				disD=ProblemDefinitions.GetItem(_listProblemCur[i].ProblemDefId);
				row.Cells.Add(_listProblemCur[i].LastModifiedDate.ToShortDateString());
				row.Cells.Add(_listProblemCur[i].DateStart?.ToShortDateString() ?? "");
				
				if(disD.Description==null) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(disD.Description);
				}
				if(_listProblemCur[i].Status==ProblemStatus.Active) {
					row.Cells.Add("Active");
				}
				else if(_listProblemCur[i].Status==ProblemStatus.Resolved) {
					row.Cells.Add("Resolved");
				}
				else {
					row.Cells.Add("Inactive");
				}
				gridProbExisting.Rows.Add(row);
			}
			gridProbExisting.EndUpdate();
		}

		private void FillReconcileGrid() {
			gridProbReconcile.BeginUpdate();
			gridProbReconcile.Columns.Clear();
			GridColumn col=new GridColumn("Last Modified",130,HorizontalAlignment.Center);
			gridProbReconcile.Columns.Add(col);
			col=new GridColumn("Date Start",100,HorizontalAlignment.Center);
			gridProbReconcile.Columns.Add(col);
			col=new GridColumn("Problem Name",260);
			gridProbReconcile.Columns.Add(col);
			col=new GridColumn("Notes",300);
			gridProbReconcile.Columns.Add(col);
			col=new GridColumn("Status",80,HorizontalAlignment.Center);
			gridProbReconcile.Columns.Add(col);
			col=new GridColumn("Is Incoming",50,HorizontalAlignment.Center);
			gridProbReconcile.Columns.Add(col);
			gridProbReconcile.Rows.Clear();
			GridRow row;
			ProblemDefinition disD;
			for(int i=0;i<_listProblemReconcile.Count;i++) {
				row=new GridRow();
				disD=new ProblemDefinition();
				if(_listProblemReconcile[i].Id == 0) {
					//To find the disease def for new disease, get the index of the matching problem in ListProblemNew, and use that index in ListProblemDefNew because they are 1 to 1 lists.
					disD=ListProblemDefNew[ListProblemNew.IndexOf(_listProblemReconcile[i])];
				}
				for(int j=0;j<_listProblemDefCur.Count;j++) {
					if(_listProblemReconcile[i].ProblemDefId > 0 && _listProblemReconcile[i].ProblemDefId==_listProblemDefCur[j].Id) {
						disD=_listProblemDefCur[j];//Gets the diseasedef matching the disease so we can use it to populate the grid
						break;
					}
				}
				row.Cells.Add(DateTime.Now.ToShortDateString());
				row.Cells.Add(_listProblemReconcile[i].DateStart?.ToShortDateString() ?? "");
				
				if(disD.Description==null) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(disD.Description);
				}
				if(_listProblemReconcile[i]==null) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(_listProblemReconcile[i].PatientNote);
				}
				if(_listProblemReconcile[i].Status==ProblemStatus.Active) {
					row.Cells.Add("Active");
				}
				else if(_listProblemReconcile[i].Status==ProblemStatus.Resolved) {
					row.Cells.Add("Resolved");
				}
				else {
					row.Cells.Add("Inactive");
				}
				row.Cells.Add(_listProblemReconcile[i].Id == 0?"X":"");
				gridProbReconcile.Rows.Add(row);
			}
			gridProbReconcile.EndUpdate();
		}

		private void butAddNew_Click(object sender,EventArgs e) {
			if(gridProbImport.SelectedIndices.Length==0) {
				MessageBox.Show("A row must be selected to add");
				return;
			}
			Problem dis;
			ProblemDefinition disD;
			ProblemDefinition disDR=null;
			int skipCount=0;
			bool isValid;
			for(int i=0;i<gridProbImport.SelectedIndices.Length;i++) {
				isValid=true;
				//Since gridProbImport and ListProblemPatNew are a 1:1 list we can use the selected index position to get our disease
				dis=ListProblemNew[gridProbImport.SelectedIndices[i]];
				disD=ListProblemDefNew[gridProbImport.SelectedIndices[i]];
				for(int j=0;j<_listProblemReconcile.Count;j++) {
					if(_listProblemReconcile[j].Id == 0) {
						disDR=ListProblemDefNew[ListProblemNew.IndexOf(_listProblemReconcile[j])];
					}
					else {
						disDR=ProblemDefinitions.GetItem(_listProblemReconcile[j].ProblemDefId);
					}
					if(disDR==null) {
						continue;
					}
					if(disDR.CodeSnomed!="" && disDR.CodeSnomed==disD.CodeSnomed) {
						isValid=false;
						skipCount++;
						break;
					}
				}
				if(isValid) {
					_listProblemReconcile.Add(dis);
				}
			}
			if(skipCount>0) {
				MessageBox.Show(" Row(s) skipped because problem already present in the reconcile list"+": "+skipCount);
			}
			FillReconcileGrid();
		}

		private void butAddExist_Click(object sender,EventArgs e) {
			if(gridProbExisting.SelectedIndices.Length==0) {
				MessageBox.Show("A row must be selected to add");
				return;
			}
			Problem dis;
			ProblemDefinition disD;
			int skipCount=0;
			bool isValid;
			for(int i=0;i<gridProbExisting.SelectedIndices.Length;i++) {
				isValid=true;
				//Since gridProbImport and ListProblemPatNew are a 1:1 list we can use the selected index position to get our dis
				dis=_listProblemCur[gridProbExisting.SelectedIndices[i]];
				disD=ProblemDefinitions.GetItem(dis.ProblemDefId);
				for(int j=0;j<_listProblemReconcile.Count;j++) {
					if(_listProblemCur[j].Id == 0) {
						for(int k=0;k<ListProblemDefNew.Count;k++) {
							if(disD.CodeSnomed!="" && disD.CodeSnomed==ListProblemDefNew[k].CodeSnomed) {
								isValid=false;
								skipCount++;
								break;
							}
						}
					}
					if(!isValid) {
						break;
					}
					if(dis.ProblemDefId==_listProblemReconcile[j].ProblemDefId) {
						isValid=false;
						skipCount++;
						break;
					}
				}
				if(isValid) {
					_listProblemReconcile.Add(dis);
				}
			}
			if(skipCount>0) {
				MessageBox.Show(" Row(s) skipped because problem already present in the reconcile list"+": "+skipCount);
			}
			FillReconcileGrid();
		}

		private void butRemoveRec_Click(object sender,EventArgs e) {
			if(gridProbReconcile.SelectedIndices.Length==0) {
				MessageBox.Show("A row must be selected to remove");
				return;
			}
			Problem dis;
			for(int i=gridProbReconcile.SelectedIndices.Length-1;i>-1;i--) {//Loop backwards so that we can remove from the list as we go
				dis=_listProblemReconcile[gridProbReconcile.SelectedIndices[i]];
				_listProblemReconcile.Remove(dis);
			}
			FillReconcileGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(_listProblemReconcile.Count==0) {
				if(!MsgBox.Show(MsgBoxButtons.YesNo,"The reconcile list is empty which will cause all existing problems to be removed.  Continue?")) {
					return;
				}
			}
			Problem dis;
			ProblemDefinition disD;
			bool isActive;
			//Discontinue any current medications that are not present in the reconcile list.
			for(int i=0;i<_listProblemCur.Count;i++) {//Start looping through all current problems
				isActive=false;
				dis=_listProblemCur[i];
				disD=ProblemDefinitions.GetItem(dis.ProblemDefId);
				for(int j=0;j<_listProblemReconcile.Count;j++) {//Compare each reconcile problem to the current problem
					ProblemDefinition disDR=ProblemDefinitions.GetItem(_listProblemReconcile[j].ProblemDefId);
					if(_listProblemReconcile[j].ProblemDefId==_listProblemCur[i].ProblemDefId) {//Has identical DiseaseDefNums
						isActive=true;
						break;
					}
					if(disDR==null) {
						continue;
					}
					if(disDR.CodeSnomed!="" && disDR.CodeSnomed==disD.CodeSnomed) {//Has a Snomed code and they are equal
						isActive=true;
						break;
					}
				}
				if(!isActive) {//Update current problems.
					dis.Status=ProblemStatus.Inactive;
					Problems.Update(_listProblemCur[i]);
				}
			}
			//Always update every current problem for the patient so that DateTStamp reflects the last reconcile date.
			if(_listProblemCur.Count>0) {
				Problems.ResetTimeStamps(_patCur.PatNum,ProblemStatus.Active);
			}
			ProblemDefinition disDU=null;
			int index;
			for(int j=0;j<_listProblemReconcile.Count;j++) {
				index=ListProblemNew.IndexOf(_listProblemReconcile[j]);
				if(index<0) {
					continue;
				}
				if(_listProblemReconcile[j]==ListProblemNew[index]) {
					var problemId = ProblemDefinitions.GetNumFromCode(ListProblemDefNew[index].CodeSnomed);

					disDU =ProblemDefinitions.GetItem(problemId??0);
					if(disDU==null) {
						ListProblemNew[index].ProblemDefId=ProblemDefinitions.Insert(ListProblemDefNew[index]);
					}
					else {
						ListProblemNew[index].ProblemDefId=disDU.Id;
					}
					Problems.Insert(ListProblemNew[index]);
				}
			}
			DataValid.SetInvalid(InvalidType.Diseases);
			//EhrMeasureEvent newMeasureEvent = new EhrMeasureEvent();
			//newMeasureEvent.DateTEvent=DateTime.Now;
			//newMeasureEvent.EventType=EhrMeasureEventType.ProblemReconcile;
			//newMeasureEvent.PatNum=PatCur.PatNum;
			//newMeasureEvent.MoreInfo="";
			//EhrMeasureEvents.Insert(newMeasureEvent);
			for(int inter=0;inter<_listProblemReconcile.Count;inter++) {
				if(CDSPermissions.GetForUser(Security.CurrentUser.Id).ShowCDS && CDSPermissions.GetForUser(Security.CurrentUser.Id).ProblemCDS) {
					ProblemDefinition disDInter=ProblemDefinitions.GetItem(_listProblemReconcile[inter].ProblemDefId);
					FormCDSIntervention FormCDSI=new FormCDSIntervention();
					FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(disDInter,_patCur);
					FormCDSI.ShowIfRequired(false);
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}