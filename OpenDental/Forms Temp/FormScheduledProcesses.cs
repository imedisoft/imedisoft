using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormScheduledProcesses:ODForm {

		public FormScheduledProcesses() {
			InitializeComponent();
			
		}
		private void FormScheduledProcesses_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			List<ScheduledProcess> listScheduledProcesses=ScheduledProcesses.Refresh();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col;
			col=new GridColumn("Scheduled Action",120);
			gridMain.Columns.Add(col);
			col=new GridColumn("Frequency to Run",150);
			gridMain.Columns.Add(col);
			col=new GridColumn("Time To Run",75);
			gridMain.Columns.Add(col);
			col=new GridColumn("Time of Last Run",155);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			foreach(ScheduledProcess schedProc in listScheduledProcesses) {
				row=new GridRow();
				row.Cells.Add(schedProc.ScheduledAction.GetDescription());
				row.Cells.Add(schedProc.FrequencyToRun.GetDescription());
				row.Cells.Add(schedProc.TimeToRun.ToShortTimeString());
				if(schedProc.LastRanDateTime.Year > 1880) {
					row.Cells.Add(schedProc.LastRanDateTime.ToString());
				}
				else {
					row.Cells.Add("");
				}
				row.Tag=schedProc;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			ScheduledProcess schedProc=new ScheduledProcess();
			schedProc.IsNew=true;
			FormScheduledProcessesEdit formScheduledProcessesEdit=new FormScheduledProcessesEdit(schedProc);
			if(formScheduledProcessesEdit.ShowDialog()!=DialogResult.OK) {
				return;
			}
			FillGrid();
		}

		private void GridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			ScheduledProcess selectedSchedProc=gridMain.SelectedTag<ScheduledProcess>();
			FormScheduledProcessesEdit formScheduledProcessesEdit=new FormScheduledProcessesEdit(selectedSchedProc);
			if(formScheduledProcessesEdit.ShowDialog()!=DialogResult.OK) {
				return;
			}
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}
}