using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental {
	public partial class FormProcBandingSelect:ODForm {
		///<summary>PatNum of currently selected patient.</summary>
		private long _patNum;
		///<summary>List of treatment planned banding procedures for the current patient.</summary>
		private List<Procedure> _listTpBandingProcs=new List<Procedure>();
		///<summary>The procedure selected from the grid.</summary>
		public Procedure SelectedProcedure;

		public FormProcBandingSelect(long patNum) {
			InitializeComponent();
			
			_patNum=patNum;
		}

		private void FormProcBandingSelect_Load(object sender,EventArgs e) {
			_listTpBandingProcs=Procedures.GetProcsForFormProcBandingSelect(_patNum);
			FillGrid();
		}

		private void FillGrid() {
			gridTpBandingProcs.BeginUpdate();
			gridTpBandingProcs.Columns.Clear();
			GridColumn col;
			col=new GridColumn("Code",70);
			gridTpBandingProcs.Columns.Add(col);
			col=new GridColumn("Description",140){ IsWidthDynamic=true };
			gridTpBandingProcs.Columns.Add(col);
			gridTpBandingProcs.Rows.Clear();
			GridRow row;
			foreach(Procedure proc in _listTpBandingProcs) {
				row=new GridRow();
				ProcedureCode procCode=ProcedureCodes.GetById(proc.CodeNum);
				row.Cells.Add(procCode.Code);
				row.Cells.Add(procCode.Description);
				row.Tag=proc;
				gridTpBandingProcs.Rows.Add(row);
			}
			gridTpBandingProcs.EndUpdate();
		}

		private void GridTpBandingProcs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			SelectProcedure((Procedure)gridTpBandingProcs.Rows[e.Row].Tag);
			DialogResult=DialogResult.OK;
		}

		private void SelectProcedure(Procedure selectedProc) {
			if(selectedProc.Discount!=0) {
				MessageBox.Show("Banding Procedures with discounts cannot be attached to an ortho case.");
				return;
			}
			SelectedProcedure=selectedProc;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridTpBandingProcs.GetSelectedIndex()==-1) {
				MessageBox.Show("Please select a procedure first.");
				return;
			}
			SelectProcedure((Procedure)gridTpBandingProcs.Rows[gridTpBandingProcs.GetSelectedIndex()].Tag);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}