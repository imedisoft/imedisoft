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
	public partial class FormProvStudentBulkEdit:ODForm {
		private List<SchoolClass> _listSchoolClasses;

		public FormProvStudentBulkEdit() {
			InitializeComponent();
			
		}

		private void FormProvStudentBulkEdit_Load(object sender,EventArgs e) {
			SetFilterControlsAndAction(() => FillGrid(),
				(int)TimeSpan.FromSeconds(0.5).TotalMilliseconds,
				textProvNum);
			comboClass.Items.Add("All");
			comboClass.SelectedIndex=0;
			_listSchoolClasses=SchoolClasses.GetAll();
			for(int i=0;i<_listSchoolClasses.Count;i++) {
				comboClass.Items.Add(SchoolClasses.GetDescription(_listSchoolClasses[i]));
			}
			FillGrid();
		}

		private void FillGrid() {
			long selectedProvNum=0;
			long schoolClass=0;
			if(comboClass.SelectedIndex>0) {
				schoolClass=_listSchoolClasses[comboClass.SelectedIndex-1].Id;
			}
			DataTable table=Providers.RefreshForDentalSchool(schoolClass,"","",textProvNum.Text,false,false);
			gridStudents.BeginUpdate();
			gridStudents.Columns.Clear();
			GridColumn col;
			col=new GridColumn("ProvNum",60);
			gridStudents.Columns.Add(col);
			col=new GridColumn("Last Name",90);
			gridStudents.Columns.Add(col);
			col=new GridColumn("First Name",90);
			gridStudents.Columns.Add(col);
			col=new GridColumn("Class",100);
			gridStudents.Columns.Add(col);
			gridStudents.Rows.Clear();
			GridRow row;
			for(int i=0;i<table.Rows.Count;i++) {
				row=new GridRow();
				if(!Preferences.GetBool(PreferenceName.EasyHideDentalSchools)) {
					row.Cells.Add(table.Rows[i]["ProvNum"].ToString());
				}
				row.Cells.Add(table.Rows[i]["LName"].ToString());
				row.Cells.Add(table.Rows[i]["FName"].ToString());
				if(table.Rows[i]["GradYear"].ToString()!="") {
						row.Cells.Add(table.Rows[i]["GradYear"].ToString()+"-"+table.Rows[i]["Descript"].ToString());
				}
				else {
						row.Cells.Add("");
				}

				gridStudents.Rows.Add(row);
			}
			gridStudents.EndUpdate();
			for(int i=0;i<table.Rows.Count;i++) {
				if(table.Rows[i]["ProvNum"].ToString()==selectedProvNum.ToString()) {
					gridStudents.SetSelected(i,true);
					break;
				}
			}
		}

		private void comboClass_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void butColor_Click(object sender,System.EventArgs e) {
			colorDialog1.Color=butColor.BackColor;
			colorDialog1.ShowDialog();
			butColor.BackColor=colorDialog1.Color;
		}

		private void butOutlineColor_Click(object sender,System.EventArgs e) {
			colorDialog1.Color=butOutlineColor.BackColor;
			colorDialog1.ShowDialog();
			butOutlineColor.BackColor=colorDialog1.Color;
		}

		private void butBulkEdit_Click(object sender,EventArgs e) {
			for(int i=0;i<gridStudents.SelectedIndices.Length;i++) {
				Provider studSelected=Providers.GetById(PIn.Long(gridStudents.Rows[i].Cells[0].Text));
				studSelected.Color=butColor.BackColor;
				studSelected.ColorOutline=butOutlineColor.BackColor;
				Providers.Update(studSelected);
			}
			MessageBox.Show("Selected students have been updated.");
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}



	}
}