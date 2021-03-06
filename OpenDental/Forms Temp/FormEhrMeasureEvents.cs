using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using Imedisoft.Forms;
using Imedisoft.Data.Models;
using Imedisoft.Data;
using System.Linq;

namespace OpenDental {
	public partial class FormEhrMeasureEvents:ODForm {
		private List<string> _typeNames;
		private List<EhrMeasureEvent> _listEhrMeasureEvents;

		public FormEhrMeasureEvents() {
			InitializeComponent();
			
		}

		private void FormEhrMeasureEvents_Load(object sender,System.EventArgs e) {
			comboType.Items.Add("All");
			comboType.SelectedIndex=0;
			_typeNames=new List<string>(Enum.GetNames(typeof(EhrMeasureEventType)));
			for(int i=0;i<_typeNames.Count;i++) {
				comboType.Items.Add(_typeNames[i]);
			}
			textDateStart.Text=new DateTime(DateTime.Today.Year,1,1).ToShortDateString();
			textDateEnd.Text=new DateTime(DateTime.Today.Year,12,31).ToShortDateString();
			FillGrid();
		}

		private void FillGrid() {
			if(comboType.SelectedIndex==0) {
				_listEhrMeasureEvents=EhrMeasureEvents.GetByDateRange(PIn.Date(textDateStart.Text),PIn.Date(textDateEnd.Text)).ToList();
			}
			else {
				_listEhrMeasureEvents=EhrMeasureEvents.GetByDateRange(PIn.Date(textDateStart.Text),PIn.Date(textDateEnd.Text),(EhrMeasureEventType)comboType.SelectedIndex-1).ToList();
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col=new GridColumn("Event Type",140);
			gridMain.Columns.Add(col);
			col=new GridColumn("Date",80);
			gridMain.Columns.Add(col);
			col=new GridColumn("PatNum",60);
			gridMain.Columns.Add(col);
			col=new GridColumn("More Info",160);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			for(int i=0;i<_listEhrMeasureEvents.Count;i++) {
				row=new GridRow();
				row.Cells.Add(_typeNames[(int)_listEhrMeasureEvents[i].Type]);
				row.Cells.Add(_listEhrMeasureEvents[i].Date.ToShortDateString());
				row.Cells.Add(_listEhrMeasureEvents[i].PatientId.ToString());
				row.Cells.Add(_listEhrMeasureEvents[i].MoreInfo);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormEhrMeasureEventEdit FormEMEE=new FormEhrMeasureEventEdit(_listEhrMeasureEvents[e.Row]);
			FormEMEE.ShowDialog();
			if(FormEMEE.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}

		private void butAuditTrail_Click(object sender,EventArgs e) {
			List<Permissions> listPermissions=new List<Permissions>();
			listPermissions.Add(Permissions.EhrMeasureEventEdit);
			FormAuditOneType FormAOT=new FormAuditOneType(0,listPermissions,"EHR Measure Event Edits",0);
			FormAOT.ShowDialog();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void comboType_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

	}
}