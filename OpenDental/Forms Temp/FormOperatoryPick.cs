﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Imedisoft.Data;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormOperatoryPick:ODForm {

		///<summary>After this window closes, this will be the OperatoryNum of the selected operatory.</summary>
		public long SelectedOperatoryNum;
		///<summary>Passed in list of operatories shown to user.</summary>
		private List<Operatory> _listOps;

		public FormOperatoryPick(List<Operatory> listOps) {
			InitializeComponent();
			
			_listOps=listOps.Select(x=>x.Copy()).ToList();
		}
		
		private void FormOperatoryPick_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!SelectOperatory()) {
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			int opNameWidth=180;
			int clinicWidth=85;
			if(!PrefC.HasClinicsEnabled) {
				//Clinics are hidden so add the width of the clinic column to the Op Name column because the clinic column will not show.
				opNameWidth+=clinicWidth;
			}
			GridColumn col=new GridColumn("Op Name",opNameWidth);
			gridMain.Columns.Add(col);
			col=new GridColumn("Abbrev",70);
			gridMain.Columns.Add(col);
			col=new GridColumn("IsHidden",64,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new GridColumn("Clinic",clinicWidth);
				gridMain.Columns.Add(col);
			}
			col=new GridColumn("Provider",70);
			gridMain.Columns.Add(col);
			col=new GridColumn("Hygienist",70);
			gridMain.Columns.Add(col);
			col=new GridColumn("IsHygiene",64,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new GridColumn("IsWebSched",74,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new GridColumn("IsNewPat",50,HorizontalAlignment.Center){ IsWidthDynamic=true };
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			List<long> listWSNPAOperatoryNums=Operatories.GetOpsForWebSchedNewPatAppts().Select(x => x.Id).ToList();
			foreach(Operatory opCur in _listOps) {
				row=new GridRow();
				row.Cells.Add(opCur.OpName);
				row.Cells.Add(opCur.Abbrev);
				if(opCur.IsHidden){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetAbbr(opCur.ClinicNum));
				}
				row.Cells.Add(Providers.GetAbbr(opCur.ProvDentist));
				row.Cells.Add(Providers.GetAbbr(opCur.ProvHygienist));
				if(opCur.IsHygiene){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				row.Cells.Add(opCur.IsWebSched?"X":"");
				row.Cells.Add(listWSNPAOperatoryNums.Contains(opCur.Id) ? "X" : "");
				row.Tag=opCur;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		///<summary>Returns true if there was an operatory selected.</summary>
		private bool SelectOperatory() {
			if(gridMain.GetSelectedIndex()==-1){
				MessageBox.Show("Please select an item first.");
				return false;
			}
			SelectedOperatoryNum=((Operatory)gridMain.Rows[gridMain.GetSelectedIndex()].Tag).Id;
			return true;
		}
		
		private void butOK_Click(object sender,EventArgs e) {
			if(!SelectOperatory()) {
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
