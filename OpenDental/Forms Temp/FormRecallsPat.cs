using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormRecallsPat:ODForm {
		public long PatNum;
		///<summary>This is just the list for the current patient.</summary>
		private List<Recall> RecallList;
		private bool IsPerio;

		public FormRecallsPat() {
			InitializeComponent();
			
		}

		private void FormRecallsPat_Load(object sender,EventArgs e) {
			/*
			//patient may or may not have existing recalls.
			Recall recallCur=null;
			for(int i=0;i<RecallList.Count;i++){
				if(RecallList[i].PatNum==PatCur.PatNum){
					recallCur=RecallList[i];
				}
			}*/
			//for testing purposes and because synchronization might have bugs, always synch here:
			//This might add a recall.
			//Recalls.Synch(PatNum);			
			FillGrid();
		}

		private void FillGrid(){
			Recalls.Synch(PatNum);
			Recalls.SynchScheduledApptFull(PatNum);
			RecallList=Recalls.GetList(PatNum);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col=new GridColumn("Type",90);
			gridMain.Columns.Add(col);
			//col=new ODGridColumn("Disabled",60,HorizontalAlignment.Center);
			//gridMain.Columns.Add(col);
			col=new GridColumn("PreviousDate",80);
			gridMain.Columns.Add(col);
			col=new GridColumn("Due Date",80);
			gridMain.Columns.Add(col);
			col=new GridColumn("Sched Date",80);
			gridMain.Columns.Add(col);
			col=new GridColumn("Interval",70);
			gridMain.Columns.Add(col);
			col=new GridColumn("Status",80);
			gridMain.Columns.Add(col);
			col=new GridColumn("Note",100);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			GridCell cell;
			IsPerio=false;
			butPerio.Text="Set Perio";
			string cellStr;
			for(int i=0;i<RecallList.Count;i++){
				if(Prefs.GetLong(PrefName.RecallTypeSpecialPerio)==RecallList[i].RecallTypeNum){
					IsPerio=true;
					butPerio.Text="Set Prophy";
				}
				row=new GridRow();
				row.Cells.Add(RecallTypes.GetDescription(RecallList[i].RecallTypeNum));
				//if(RecallList[i].IsDisabled){
				//	row.Cells.Add("X");
				//}
				//else{
				//	row.Cells.Add("");
				//}
				if(RecallList[i].DatePrevious.Year<1880){
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(RecallList[i].DatePrevious.ToShortDateString());
				}
				if(RecallList[i].DateDue.Year<1880){
					row.Cells.Add("");
				}
				else{
					cell=new GridCell(RecallList[i].DateDue.ToShortDateString());
					if(RecallList[i].DateDue<DateTime.Today){
						cell.Bold= true;
						cell.ForeColor=Color.Firebrick;
					}
					row.Cells.Add(cell);
				}
				if(RecallList[i].DateScheduled.Year<1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(RecallList[i].DateScheduled.ToShortDateString());
				}
				row.Cells.Add(RecallList[i].RecallInterval.ToString());
				row.Cells.Add(Definitions.GetValue(DefinitionCategory.RecallUnschedStatus,RecallList[i].RecallStatus));
				cellStr="";
				if(RecallList[i].IsDisabled) {
					cellStr+="Disabled";
				}
				if(RecallList[i].DisableUntilDate.Year>1880) {
					if(cellStr!="") {
						cellStr+=", ";
					}
					cellStr+="Disabled until "+RecallList[i].DisableUntilDate.ToShortDateString();
				}
				if(RecallList[i].DisableUntilBalance>0) {
					if(cellStr!="") {
						cellStr+=", ";
					}
					cellStr+="Disabled until balance "+RecallList[i].DisableUntilBalance.ToString("c");
				}
				if(RecallList[i].Note!="") {
					if(cellStr!="") {
						cellStr+=", ";
					}
					cellStr+=RecallList[i].Note;
				}
				row.Cells.Add(cellStr);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormRecallEdit FormR=new FormRecallEdit();
			FormR.RecallCur=RecallList[e.Row].Copy();
			FormR.ShowDialog();
			FillGrid();
		}

		private void butPerio_Click(object sender,EventArgs e) {
			//make sure we have both special types properly setup.
			if(!RecallTypes.PerioAndProphyBothHaveTriggers()){
				MessageBox.Show("Prophy and Perio special recall types are not setup properly.  They must both exist, and they must both have a trigger.");
				return;
			}
			if(IsPerio){
				//change the perio types to prophy
				for(int i=0;i<RecallList.Count;i++){
					if(Prefs.GetLong(PrefName.RecallTypeSpecialPerio)==RecallList[i].RecallTypeNum){
						RecallList[i].RecallTypeNum=Prefs.GetLong(PrefName.RecallTypeSpecialProphy);
						RecallList[i].RecallInterval=RecallTypes.GetInterval(Prefs.GetLong(PrefName.RecallTypeSpecialProphy));
						//previous date will be reset below in synch, but probably won't change since similar triggers.
						Recalls.Update(RecallList[i]);
						SecurityLogs.MakeLogEntry(Permissions.RecallEdit,RecallList[i].PatNum,"Recall changed to Prophy from the Recalls for Patient window.");
						break;
					}
				}
			}
			else{
				bool found=false;
				//change any prophy types to perio
				for(int i=0;i<RecallList.Count;i++){
					if(Prefs.GetLong(PrefName.RecallTypeSpecialProphy)==RecallList[i].RecallTypeNum){
						RecallList[i].RecallTypeNum=Prefs.GetLong(PrefName.RecallTypeSpecialPerio);
						RecallList[i].RecallInterval=RecallTypes.GetInterval(Prefs.GetLong(PrefName.RecallTypeSpecialPerio));
						//previous date will be reset below in synch, but probably won't change since similar triggers.
						Recalls.Update(RecallList[i]);
						SecurityLogs.MakeLogEntry(Permissions.RecallEdit,RecallList[i].PatNum,"Recall changed to Perio from the Recalls for Patient window.");
						found=true;
						break;
					}
				}
				//if none found, then add a perio
				if(!found){
					Recall recall=new Recall();
					recall.PatNum=PatNum;
					recall.RecallInterval=RecallTypes.GetInterval(Prefs.GetLong(PrefName.RecallTypeSpecialPerio));
					recall.RecallTypeNum=Prefs.GetLong(PrefName.RecallTypeSpecialPerio);
					Recalls.Insert(recall);
					SecurityLogs.MakeLogEntry(Permissions.RecallEdit,recall.PatNum,"Perio recall added from the Recalls for Patient window.");
				}
			}
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			Recall recall=new Recall();
			recall.RecallTypeNum=0;//user will have to pick
			recall.PatNum=PatNum;
			recall.RecallInterval=new Interval(0,0,6,0);
			FormRecallEdit FormRE=new FormRecallEdit();
			FormRE.IsNew=true;
			FormRE.RecallCur=recall;
			FormRE.ShowDialog();
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {	
			Close();
		}

		private void FormRecallsPat_FormClosing(object sender,FormClosingEventArgs e) {
			//check for duplicates that might cause a malfunction.
			int prophyCount=0;
			for(int i=0;i<RecallList.Count;i++) {
				if(RecallTypes.ProphyType==RecallList[i].RecallTypeNum) {
					prophyCount++;
				}
				if(RecallTypes.PerioType==RecallList[i].RecallTypeNum) {
					prophyCount++;
				}
			}
			if(prophyCount>1) {
				if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Multiple prophy and/or perio recalls detected.  A patient should have only one prophy or perio recall, and the calculations will not work correctly otherwise.  Continue anyway?")){
					e.Cancel=true;
				}
			}
		}

		

		

		

		
		

		
	}
}