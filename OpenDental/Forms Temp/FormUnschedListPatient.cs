﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Imedisoft.Data;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormUnschedListPatient:ODForm {
		private List<Appointment> _listUnschedApptsForPat;
		private Patient _patCur;
		///<summary>Holds the selected appointment from the grid. Only one selection is allowed.</summary>
		public Appointment SelectedAppt;

		public FormUnschedListPatient(Patient pat) {
			InitializeComponent();
			_patCur=pat;
		}

		private void FormPatientUnschedList_Load(object sender,EventArgs e) {
			this.Text=" "+_patCur.GetNameLF();
			_listUnschedApptsForPat=Appointments.GetUnschedApptsForPat(_patCur.PatNum);
			FillGrid();
		}

		private void FillGrid() {
			this.Cursor=Cursors.WaitCursor;
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col=new GridColumn("Date",65,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new GridColumn("AptStatus",90);
			gridMain.Columns.Add(col);
			col=new GridColumn("UnschedStatus",110);
			gridMain.Columns.Add(col);
			col=new GridColumn("Prov",80);
			gridMain.Columns.Add(col);
			col=new GridColumn("Procedures",150);
			gridMain.Columns.Add(col);
			col=new GridColumn("Notes",200);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			foreach(Appointment apt in _listUnschedApptsForPat) {
				row=new GridRow();
				row.Cells.Add(apt.AptDateTime.ToShortDateString());
				row.Cells.Add(apt.AptStatus.ToString());
				row.Cells.Add(apt.UnschedStatus.ToString());
				row.Cells.Add(Providers.GetAbbr(apt.ProvNum));
				row.Cells.Add(apt.ProcDescript);
				row.Cells.Add(apt.Note);
				row.Tag=apt;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			this.Cursor=Cursors.Default;
		}

		///<summary>Sets SelectedAppt to the appointment that is currently selected in the grid.  Shows an error message to the user if no appointment is selected.
		///Otherwise; Sets the appointment and then sets DialogResult to OK.</summary>
		private void SetSelectedAppt() {
			SelectedAppt=gridMain.SelectedTag<Appointment>();
			if(SelectedAppt==null) {//No row was selected.
				MessageBox.Show("Please select an unscheduled appointment to use.");
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			SetSelectedAppt();
		}

		private void butOK_Click(object sender,EventArgs e) {
			SetSelectedAppt();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
