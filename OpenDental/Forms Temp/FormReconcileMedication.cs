using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using Imedisoft.Data.Models;
using Imedisoft.Data;
using System.Linq;
using Imedisoft.Forms;

namespace OpenDental {
	public partial class FormReconcileMedication:ODForm {
		public List<MedicationPat> ListMedicationPatNew;
		private List<MedicationPat> _listMedicationPatReconcile;
		private List<MedicationPat> _listMedicationPatCur;
		private List<Medication> _listMedicationCur;
		private Patient _patCur;

		///<summary>Patient must be valid.  Do not pass null.</summary>
		public FormReconcileMedication(Patient patCur) {
			InitializeComponent();
			
			_patCur=patCur;
		}

		private void FormReconcileMedication_Load(object sender,EventArgs e) {
			for(int index=0;index<ListMedicationPatNew.Count;index++) {
				ListMedicationPatNew[index].PatNum=_patCur.PatNum;
			}
			FillExistingGrid();
			_listMedicationPatReconcile=new List<MedicationPat>(_listMedicationPatCur);
			#region Delete after testing
			//ListMedicationPatNew=new List<MedicationPat>();
			//MedicationPat medP=new MedicationPat();
			//medP.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(5));
			//medP.MedDescript="Valpax";
			//medP.PatNote="Two a day";
			//medP.RxCui=542687;
			//medP.IsNew=true;
			//medP.PatNum=PatCur.PatNum;
			//ListMedicationPatNew.Add(medP);
			//medP=new MedicationPat();
			//medP.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(2));
			//medP.MedDescript="Usept";
			//medP.PatNote="Three a day";
			//medP.RxCui=405384;
			//medP.IsNew=true;
			//medP.PatNum=PatCur.PatNum;
			//ListMedicationPatNew.Add(medP);
			//medP=new MedicationPat();
			//medP.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(1));
			//medP.MedDescript="SmileGuard";
			//medP.PatNote="Two a day";
			//medP.RxCui=1038751;
			//medP.IsNew=true;
			//medP.PatNum=PatCur.PatNum;
			//ListMedicationPatNew.Add(medP);
			//medP=new MedicationPat();
			//medP.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(4));
			//medP.MedDescript="Slozem";
			//medP.PatNote="One a day";
			//medP.RxCui=151154;
			//medP.IsNew=true;
			//medP.PatNum=PatCur.PatNum;
			//ListMedicationPatNew.Add(medP);
			//medP=new MedicationPat();
			//medP.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(6));
			//medP.MedDescript="Prax";
			//medP.PatNote="Four a day";
			//medP.RxCui=219336;
			//medP.IsNew=true;
			//medP.PatNum=PatCur.PatNum;
			//ListMedicationPatNew.Add(medP);
			//medP=new MedicationPat();
			//medP.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(5));
			//medP.MedDescript="PrameGel";
			//medP.PatNote="Two a day";
			//medP.RxCui=93822;
			//medP.IsNew=true;
			//medP.PatNum=PatCur.PatNum;
			//ListMedicationPatNew.Add(medP);
			//medP=new MedicationPat();
			//medP.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(7));
			//medP.MedDescript="Pramotic";
			//medP.PatNote="Five a day";
			//medP.RxCui=405268;
			//medP.IsNew=true;
			//medP.PatNum=PatCur.PatNum;
			//ListMedicationPatNew.Add(medP);
			//medP=new MedicationPat();
			//medP.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(3));
			//medP.MedDescript="Medetomidine";
			//medP.PatNote="Three a day";
			//medP.RxCui=52016;
			//medP.IsNew=true;
			//medP.PatNum=PatCur.PatNum;
			//ListMedicationPatNew.Add(medP);
			//medP=new MedicationPat();
			//medP.DateStart=DateTime.Now.Subtract(TimeSpan.FromDays(4));
			//medP.MedDescript="Medcodin";
			//medP.PatNote="One a day";
			//medP.RxCui=218274;
			//medP.IsNew=true;
			//medP.PatNum=PatCur.PatNum;
			//ListMedicationPatNew.Add(medP);
			#endregion
			//Automation to initially fill reconcile grid with "recommended" rows.
			bool isValid;
			for(int i=0;i<ListMedicationPatNew.Count;i++) {
				isValid=true;
				for(int j=0;j<_listMedicationPatReconcile.Count;j++) {
					if(_listMedicationPatReconcile[j].RxCui==ListMedicationPatNew[i].RxCui) {
						isValid=false;
						break;
					}
				}
				if(isValid) {
					_listMedicationPatReconcile.Add(ListMedicationPatNew[i]);
				}
			}
			FillImportGrid();
			FillReconcileGrid();
		}

		private void FillImportGrid() {
			gridMedImport.BeginUpdate();
			gridMedImport.Columns.Clear();
			GridColumn col=new GridColumn("Last Modified",100,HorizontalAlignment.Center);
			gridMedImport.Columns.Add(col);
			col=new GridColumn("Date Start",100,HorizontalAlignment.Center);
			gridMedImport.Columns.Add(col);
			col=new GridColumn("Date Stop",100,HorizontalAlignment.Center);
			gridMedImport.Columns.Add(col);
			col=new GridColumn("Description",220);
			gridMedImport.Columns.Add(col);
			gridMedImport.Rows.Clear();
			GridRow row;
			for(int i=0;i<ListMedicationPatNew.Count;i++) {
				row=new GridRow();
				row.Cells.Add(DateTime.Now.ToShortDateString());
				if(ListMedicationPatNew[i].DateStart.Year<1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(ListMedicationPatNew[i].DateStart.ToShortDateString());
				}
				if(ListMedicationPatNew[i].DateStop.Year<1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(ListMedicationPatNew[i].DateStop.ToShortDateString());
				}
				if(ListMedicationPatNew[i].MedDescript==null) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(ListMedicationPatNew[i].MedDescript);
				}
				gridMedImport.Rows.Add(row);
			}
			gridMedImport.EndUpdate();
		}

		private void FillExistingGrid() {
			gridMedExisting.BeginUpdate();
			gridMedExisting.Columns.Clear();
			GridColumn col=new GridColumn("Last Modified",100,HorizontalAlignment.Center);
			gridMedExisting.Columns.Add(col);
			col=new GridColumn("Date Start",100,HorizontalAlignment.Center);
			gridMedExisting.Columns.Add(col);
			col=new GridColumn("Date Stop",100,HorizontalAlignment.Center);
			gridMedExisting.Columns.Add(col);
			col=new GridColumn("Description",320);
			gridMedExisting.Columns.Add(col);
			gridMedExisting.Rows.Clear();
			_listMedicationPatCur=MedicationPats.GetMedPatsForReconcile(_patCur.PatNum);
			List<long> medicationNums=new List<long>();
			for(int h=0;h<_listMedicationPatCur.Count;h++) {
				if(_listMedicationPatCur[h].MedicationNum > 0) {
					medicationNums.Add(_listMedicationPatCur[h].MedicationNum);
				}
			}
			_listMedicationCur=Medications.GetMultMedications(medicationNums).ToList();
			GridRow row;
			Medication med;
			for(int i=0;i<_listMedicationPatCur.Count;i++) {
				row=new GridRow();
				med=Medications.GetById(_listMedicationPatCur[i].MedicationNum);//Possibly change if we decided to postpone caching medications
				row.Cells.Add(_listMedicationPatCur[i].DateTStamp.ToShortDateString());
				if(_listMedicationPatCur[i].DateStart.Year<1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(_listMedicationPatCur[i].DateStart.ToShortDateString());
				}
				if(_listMedicationPatCur[i].DateStop.Year<1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(_listMedicationPatCur[i].DateStop.ToShortDateString());
				}
				if(med.Name==null) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(med.Name);
				}
				gridMedExisting.Rows.Add(row);
			}
			gridMedExisting.EndUpdate();
		}

		private void FillReconcileGrid() {
			gridMedReconcile.BeginUpdate();
			gridMedReconcile.Columns.Clear();
			GridColumn col=new GridColumn("Last Modified",130,HorizontalAlignment.Center);
			gridMedReconcile.Columns.Add(col);
			col=new GridColumn("Date Start",100,HorizontalAlignment.Center);
			gridMedReconcile.Columns.Add(col);
			col=new GridColumn("Date Stop",100,HorizontalAlignment.Center);
			gridMedReconcile.Columns.Add(col);
			col=new GridColumn("Description",350);
			gridMedReconcile.Columns.Add(col);
			col=new GridColumn("Notes",150);
			gridMedReconcile.Columns.Add(col);
			col=new GridColumn("Is Incoming",50,HorizontalAlignment.Center);
			gridMedReconcile.Columns.Add(col);
			gridMedReconcile.Rows.Clear();
			GridRow row;
			for(int i=0;i<_listMedicationPatReconcile.Count;i++) {
				row=new GridRow();
				row.Cells.Add(DateTime.Now.ToShortDateString());
				if(_listMedicationPatReconcile[i].DateStart.Year<1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(_listMedicationPatReconcile[i].DateStart.ToShortDateString());
				}
				if(_listMedicationPatReconcile[i].DateStop.Year<1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(_listMedicationPatReconcile[i].DateStop.ToShortDateString());
				}
				if(_listMedicationPatReconcile[i].IsNew) {
					if(_listMedicationPatReconcile[i].MedDescript==null) {
						row.Cells.Add("");
					}
					else {
						row.Cells.Add(_listMedicationPatReconcile[i].MedDescript);
					}
				}
				else {
					Medication med=Medications.GetById(_listMedicationPatReconcile[i].MedicationNum);
					if(med.Name==null) {
						row.Cells.Add("");
					}
					else {
						row.Cells.Add(med.Name);
					}
				}
				if(_listMedicationPatReconcile[i].PatNote==null) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(_listMedicationPatReconcile[i].PatNote);
				}
				row.Cells.Add(_listMedicationPatReconcile[i].IsNew?"X":"");
				gridMedReconcile.Rows.Add(row);
			}
			gridMedReconcile.EndUpdate();
		}

		private void butAddNew_Click(object sender,EventArgs e) {
			if(gridMedImport.SelectedIndices.Length==0) {
				MessageBox.Show("A row must be selected to add");
				return;
			}
			MedicationPat medP;
			int skipCount=0;
			bool isValid;
			for(int i=0;i<gridMedImport.SelectedIndices.Length;i++) {
				isValid=true;
				//Since gridMedImport and ListMedicationPatNew are a 1:1 list we can use the selected index position to get our medP
				medP=ListMedicationPatNew[gridMedImport.SelectedIndices[i]];
				for(int j=0;j<gridMedReconcile.Rows.Count;j++) {
					if(medP.RxCui > 0 && medP.RxCui==_listMedicationPatReconcile[j].RxCui) {
						isValid=false;
						skipCount++;
						break;
					}
				}
				if(isValid) {
					_listMedicationPatReconcile.Add(medP);
				}
			}
			if(skipCount>0) {
				MessageBox.Show(" Row(s) skipped because medication already present in the reconcile list"+": "+skipCount);
			}
			FillReconcileGrid();
		}

		private void butAddExist_Click(object sender,EventArgs e) {
			if(gridMedExisting.SelectedIndices.Length==0) {
				MessageBox.Show("A row must be selected to add");
				return;
			}
			MedicationPat medP;
			int skipCount=0;
			bool isValid;
			for(int i=0;i<gridMedExisting.SelectedIndices.Length;i++) {
				isValid=true;
				//Since gridMedImport and ListMedicationPatNew are a 1:1 list we can use the selected index position to get our medP
				medP=_listMedicationPatCur[gridMedExisting.SelectedIndices[i]];
				for(int j=0;j<gridMedReconcile.Rows.Count;j++) {
					if(medP.RxCui > 0 && medP.RxCui==_listMedicationPatReconcile[j].RxCui) {
						isValid=false;
						skipCount++;
						break;
					}
					if(medP.MedicationNum==_listMedicationPatReconcile[j].MedicationNum) {
						isValid=false;
						skipCount++;
						break;
					}
				}
				if(isValid) {
					_listMedicationPatReconcile.Add(medP);
				}
			}
			if(skipCount>0) {
				MessageBox.Show(" Row(s) skipped because medication already present in the reconcile list"+": "+skipCount);
			}
			FillReconcileGrid();
		}

		private void butRemoveRec_Click(object sender,EventArgs e) {
			if(gridMedReconcile.SelectedIndices.Length==0) {
				MessageBox.Show("A row must be selected to remove");
				return;
			}
			MedicationPat medP;
			for(int i=gridMedReconcile.SelectedIndices.Length-1;i>-1;i--) {//Loop backwards so that we can remove from the list as we go
				medP=_listMedicationPatReconcile[gridMedReconcile.SelectedIndices[i]];
				_listMedicationPatReconcile.Remove(medP);
			}
			FillReconcileGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(_listMedicationPatReconcile.Count==0) {
				if(!MsgBox.Show(MsgBoxButtons.YesNo,"The reconcile list is empty which will cause all existing medications to be removed.  Continue?")) {
					return;
				}
			}
			MedicationPat medP;
			bool isActive;
			//Discontinue any current medications that are not present in the reconcile list.
			for(int i=0;i<_listMedicationPatCur.Count;i++) {//Start looping through all current medications
				isActive=false;
				medP=_listMedicationPatCur[i];
				for(int j=0;j<_listMedicationPatReconcile.Count;j++) {//Compare each reconcile medication to the current medication
					if(medP.RxCui > 0 && medP.RxCui==_listMedicationPatReconcile[j].RxCui && _listMedicationPatReconcile[j].MedicationNum==_listMedicationPatCur[i].MedicationNum) {//Has an RxNorm code and they are equal
						isActive=true;
						break;
					}
				}
				if(!isActive) {//Update current medications.
					_listMedicationPatCur[i].DateStop=DateTime.Now;//Set the current DateStop to today (to set the medication as discontinued)
					MedicationPats.Update(_listMedicationPatCur[i]);
				}
			}
			//Always update every current medication for the patient so that DateTStamp reflects the last reconcile date.
			if(_listMedicationPatCur.Count>0) {
				MedicationPats.ResetTimeStamps(_patCur.PatNum,true);
			}
			Medication med;
			int index;
			for(int j=0;j<_listMedicationPatReconcile.Count;j++) {
				index=ListMedicationPatNew.IndexOf(_listMedicationPatReconcile[j]);
				if(index<0) {
					continue;
				}
				if(_listMedicationPatReconcile[j]==ListMedicationPatNew[index]) {
					med=Medications.GetByRxCuiNoCache(_listMedicationPatReconcile[j].RxCui.ToString());
					if(med==null) {
						med=new Medication();
						med.Name=ListMedicationPatNew[index].MedDescript;
						med.RxCui=ListMedicationPatNew[index].RxCui.ToString();
						ListMedicationPatNew[index].MedicationNum=Medications.Insert(med);
						med.GenericId=med.Id;
						Medications.Update(med);
					}
					else {
						ListMedicationPatNew[index].MedicationNum=med.Id;
					}
					ListMedicationPatNew[index].ProvNum=0;//Since imported, set provnum to 0 so it does not affect CPOE.
					MedicationPats.Insert(ListMedicationPatNew[index]);
				}
			}
			EhrMeasureEvent newMeasureEvent=new EhrMeasureEvent();
			newMeasureEvent.Date=DateTime.Now;
			newMeasureEvent.Type=EhrMeasureEventType.MedicationReconcile;
			newMeasureEvent.PatientId=_patCur.PatNum;
			newMeasureEvent.MoreInfo="";
			EhrMeasureEvents.Save(newMeasureEvent);
			for(int inter=0;inter<_listMedicationPatReconcile.Count;inter++) {
				if(CdsPermissions.GetByUser(Security.CurrentUser.Id).ShowCDS && CdsPermissions.GetByUser(Security.CurrentUser.Id).MedicationCDS) {
					Medication medInter=Medications.GetByRxCuiNoCache(_listMedicationPatReconcile[inter].RxCui.ToString());
					FormCdsIntervention.ShowIfRequired(EhrTriggers.TriggerMatch(medInter, _patCur), false);
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}