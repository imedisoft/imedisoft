using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using Imedisoft.Forms;
using Imedisoft.Data.Models;

namespace OpenDental {
	public partial class FormResellers:ODForm {
		private DataTable TableResellers;

		public FormResellers() {
			InitializeComponent();
			
			gridMain.ContextMenu=menuRightClick;
		}

		private void FormResellers_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			TableResellers=Resellers.GetResellerList();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col=new GridColumn("PatNum",60);
			gridMain.Columns.Add(col);
			col=new GridColumn("LName",150);
			gridMain.Columns.Add(col);
			col=new GridColumn("FName",130);
			gridMain.Columns.Add(col);
			col=new GridColumn("Email",200);
			gridMain.Columns.Add(col);
			col=new GridColumn("WkPhone",100);
			gridMain.Columns.Add(col);
			col=new GridColumn("PhoneNumberVal",100);
			gridMain.Columns.Add(col);
			col=new GridColumn("Address",180);
			gridMain.Columns.Add(col);
			col=new GridColumn("City",80);
			gridMain.Columns.Add(col);
			col=new GridColumn("State",40);
			gridMain.Columns.Add(col);
			col=new GridColumn("PatStatus",80);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			for(int i=0;i<TableResellers.Rows.Count;i++) {
				row=new GridRow();
				row.Cells.Add(TableResellers.Rows[i]["PatNum"].ToString());
				row.Cells.Add(TableResellers.Rows[i]["LName"].ToString());
				row.Cells.Add(TableResellers.Rows[i]["FName"].ToString());
				row.Cells.Add(TableResellers.Rows[i]["Email"].ToString());
				row.Cells.Add(TableResellers.Rows[i]["WkPhone"].ToString());
				row.Cells.Add(TableResellers.Rows[i]["PhoneNumberVal"].ToString());
				row.Cells.Add(TableResellers.Rows[i]["Address"].ToString());
				row.Cells.Add(TableResellers.Rows[i]["City"].ToString());
				row.Cells.Add(TableResellers.Rows[i]["State"].ToString());
				row.Cells.Add(((PatientStatus)PIn.Int(TableResellers.Rows[i]["PatStatus"].ToString())).ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Reseller reseller=Resellers.GetOne(PIn.Long(TableResellers.Rows[e.Row]["ResellerNum"].ToString()));
			FormResellerEdit FormRE=new FormResellerEdit(reseller);
			FormRE.ShowDialog();
			FillGrid();//Could have deleted the reseller.
		}

		private void menuItemAccount_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()<0) {
				MessageBox.Show("Please select a reseller first.");
				return;
			}
			GotoModule.GotoAccount(PIn.Long(TableResellers.Rows[gridMain.GetSelectedIndex()]["PatNum"].ToString()));
		}

		private void butAdd_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
				return;
			}
			FormPatientSelect FormPS=new FormPatientSelect();
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK) {
				return;
			}
			Patient patientSelected=Patients.GetPat(FormPS.SelectedPatientId);
			if(patientSelected.Guarantor!=FormPS.SelectedPatientId) {
				MessageBox.Show("Customer must be a guarantor before they can be added as a reseller.");
				return;
			}
			if(Resellers.IsResellerFamily(patientSelected)) {
				MessageBox.Show("Customer is already a reseller or part of a reseller family.");
				return;
			}
			Reseller reseller=new Reseller() {
				PatNum=FormPS.SelectedPatientId,
				BillingType=42,//Hardcoded to HQs "No Support: Developer/Reseller"
				VotesAllotted=0,
				Note="This is a customer of a reseller.  We do not directly support this customer.",
			};
			Resellers.Insert(reseller);
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}
	}
}