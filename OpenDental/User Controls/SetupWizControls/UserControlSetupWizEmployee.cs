﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using OpenDental.UI;
using OpenDental;
using OpenDentBusiness;
using Imedisoft.Data;

namespace OpenDental.User_Controls.SetupWizard {
	public partial class UserControlSetupWizEmployee:SetupWizControl {
		private int _blink;
		private bool isChanged;
		private List<Employee> _listEmployees;

		public UserControlSetupWizEmployee() {
			InitializeComponent();
			this.OnControlDone += ControlDone;
		}

		private void UserControlSetupWizEmployee_Load(object sender,EventArgs e) {
			FillGrid();
			if(_listEmployees.Where(x => x.FirstName.ToLower() != "default").ToList().Count==0) {
				MessageBox.Show("You have no valid employees. Please click the 'Add' button to add an employee.");
				timerBlink.Start();
			}
		}

		private void FillGrid() {
			_listEmployees=Employees.GetAll(true);
			Color colorNeedsAttn = OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col = new GridColumn("Last Name",135);
			gridMain.Columns.Add(col);
			col = new GridColumn("First Name",135);
			gridMain.Columns.Add(col);
			col = new GridColumn("MI",65);
			gridMain.Columns.Add(col);
			col = new GridColumn("Payroll ID",105);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			bool isAllComplete=true;
			if(_listEmployees.Where(x => x.FirstName.ToLower()!="default").ToList().Count==0) {
				isAllComplete=false;
			}
			foreach(Employee emp in _listEmployees) {
				row = new GridRow();
				row.Cells.Add(emp.LastName);
				if(string.IsNullOrEmpty(emp.LastName) || emp.LastName.ToLower() == "default") {
					row.Cells[row.Cells.Count-1].BackColor=colorNeedsAttn;
					isAllComplete=false;
				}
				row.Cells.Add(emp.FirstName);
				if(string.IsNullOrEmpty(emp.FirstName) || emp.FirstName.ToLower() == "default") {
					row.Cells[row.Cells.Count-1].BackColor=colorNeedsAttn;
					isAllComplete=false;
				}
				row.Cells.Add(emp.Initials);
				//middle initial is not a required column
				row.Cells.Add(emp.PayrollId);
				//Payroll ID is not a required column
				row.Tag=emp;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			if(isAllComplete) {
				IsDone=true;
			}
			else {
				IsDone=false;
			}
		}

		private void timerBlink_Tick(object sender,EventArgs e) {
			if(_blink > 5) {
				pictureAdd.Visible=true;
				foreach(GridRow rowCur in gridMain.Rows) {
					rowCur.BackColor=OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
				}
				gridMain.Invalidate();
				timerBlink.Stop();
				return;
			}
			pictureAdd.Visible=!pictureAdd.Visible;
			foreach(GridRow rowCur in gridMain.Rows) {
				rowCur.BackColor=rowCur.BackColor==Color.White?OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention):Color.White;
			}
			gridMain.Invalidate();
			_blink++;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Employee selectedEmployee=(Employee)gridMain.Rows[e.Row].Tag;
			FormEmployeeEdit FormEE=new FormEmployeeEdit();
			FormEE.EmployeeCur=selectedEmployee;
			FormEE.ShowDialog();
			if(FormEE.DialogResult==DialogResult.OK) {
				Employees.RefreshCache();
				FillGrid();
				isChanged=true;
			}
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormEmployeeEdit FormEE=new FormEmployeeEdit();
			FormEE.IsNew=true;
			FormEE.EmployeeCur=new Employee(); 
			FormEE.ShowDialog();
			if(FormEE.DialogResult==DialogResult.OK) {
				Employees.RefreshCache();
				FillGrid();
				isChanged=true;
			}
		}

		private void butAdvanced_Click(object sender,EventArgs e) {
			new FormEmployeeSelect().ShowDialog();
			FillGrid();
		}

		private void ControlDone(object sender, EventArgs e) {
			if(isChanged) {
				DataValid.SetInvalid(InvalidType.Employees);
			}
		}
	}
}
