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
using Imedisoft.Forms;

namespace OpenDental.User_Controls.SetupWizard {
	public partial class UserControlSetupWizClinic:SetupWizControl {
		private int _blink;

		public UserControlSetupWizClinic() {
			InitializeComponent();
		}

		private void UserControlSetupWizClinic_Load(object sender,EventArgs e) {
			FillGrid();
			if(Clinics.Count(false)==0) {
				MsgBox.Show("You have no valid clinics. Please click the Add button to add a clinic.");
				timer1.Start();
			}
		}

		private void FillGrid() {
			Color needsAttnCol = OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col;
			col = new GridColumn("Clinic",110);
			gridMain.Columns.Add(col);
			col = new GridColumn("Abbrev",70);
			gridMain.Columns.Add(col);
			col = new GridColumn("Phone",100);
			gridMain.Columns.Add(col);
			col = new GridColumn("Address",120);
			gridMain.Columns.Add(col);
			col = new GridColumn("City",90);
			gridMain.Columns.Add(col);
			col = new GridColumn("State",50);
			gridMain.Columns.Add(col);
			col = new GridColumn("ZIP",80);
			gridMain.Columns.Add(col);
			col = new GridColumn("Default Prov",75);
			gridMain.Columns.Add(col);
			col = new GridColumn("IsHidden",55,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			bool IsAllComplete = true;
			List<Clinic> listClins = Clinics.GetAll(true);
			if(listClins.Count == 0) {
				IsAllComplete=false;
			}
			foreach(Clinic clinCur in listClins) {
				row = new GridRow();
				row.Cells.Add(clinCur.Description);
				if(!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.Description)) {
					row.Cells[row.Cells.Count-1].BackColor=needsAttnCol;
					IsAllComplete=false;
				}
				row.Cells.Add(clinCur.Abbr);
				if(!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.Abbr)) {
					row.Cells[row.Cells.Count-1].BackColor=needsAttnCol;
					IsAllComplete=false;
				}
				row.Cells.Add(TelephoneNumbers.FormatNumbersExactTen(clinCur.Phone));
				if(!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.Phone)) {
					row.Cells[row.Cells.Count-1].BackColor=needsAttnCol;
					IsAllComplete=false;
				}
				row.Cells.Add(clinCur.AddressLine1);
				if(!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.AddressLine1)) {
					row.Cells[row.Cells.Count-1].BackColor=needsAttnCol;
					IsAllComplete=false;
				}
				row.Cells.Add(clinCur.City);
				if(!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.City)) {
					row.Cells[row.Cells.Count-1].BackColor=needsAttnCol;
					IsAllComplete=false;
				}
				row.Cells.Add(clinCur.State);
				if(!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.State)) {
					row.Cells[row.Cells.Count-1].BackColor=needsAttnCol;
					IsAllComplete=false;
				}
				row.Cells.Add(clinCur.Zip);
				if(!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.Zip)) {
					row.Cells[row.Cells.Count-1].BackColor=needsAttnCol;
					IsAllComplete=false;
				}
				row.Cells.Add(clinCur.DefaultProviderId.HasValue ? Providers.GetAbbr(clinCur.DefaultProviderId.Value) : "");
				row.Cells.Add(clinCur.IsHidden?"X":"");
				row.Tag=clinCur;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			if(IsAllComplete) {
				IsDone=true;
			}
			else {
				IsDone=false;
			}
		}

		private void timer1_Tick(object sender,EventArgs e) {
			if(_blink > 5) {
				pictureAdd.Visible=true;
				timer1.Stop();
				return;
			}
			pictureAdd.Visible=!pictureAdd.Visible;
			_blink++;
		}

		private void butAdd_Click(object sender,EventArgs e) {
			var clinic = new Clinic();

			FormClinicEdit FormCE = new FormClinicEdit(clinic);
			FormCE.ShowDialog();
			if(FormCE.DialogResult==DialogResult.OK) {
				Clinics.Insert(clinic);
				DataValid.SetInvalid(InvalidType.Providers);
				FillGrid();
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Clinic clinCur = (Clinic)gridMain.Rows[e.Row].Tag;
			FormClinicEdit FormCE = new FormClinicEdit(clinCur);
			FormCE.ShowDialog();
			if(FormCE.DialogResult==DialogResult.OK) {
				Clinics.Update(clinCur);
				DataValid.SetInvalid(InvalidType.Providers);
				FillGrid();
			}
		}

		private void butAdvanced_Click(object sender,EventArgs e) {
			new FormClinics().ShowDialog();
			FillGrid();
		}
	}
}
