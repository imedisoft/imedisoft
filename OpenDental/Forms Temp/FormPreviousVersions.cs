using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormPreviousVersions:ODForm {

		public FormPreviousVersions() {
			InitializeComponent();
			
		}

		private void FormPreviousVersions_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn("Version",117);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("Date",117);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row=null;
			foreach(UpdateHistory updateHistory in UpdateHistories.GetAll()) {
				row=new GridRow();
				row.Cells.Add(updateHistory.Version);
				row.Cells.Add(updateHistory.InstalledOn.ToString());
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}
	}
}