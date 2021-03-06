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

namespace OpenDental
{
	public partial class FormDispensary : ODForm
	{
		private List<SchoolClass> _listSchoolClasses;

		public FormDispensary()
		{
			InitializeComponent();

		}

		private void FormDispensary_Load(object sender, EventArgs e)
		{
			comboClass.Items.Add("All");
			comboClass.SelectedIndex = 0;
			_listSchoolClasses = SchoolClasses.GetAll();
			for (int i = 0; i < _listSchoolClasses.Count; i++)
			{
				comboClass.Items.Add(SchoolClasses.GetDescription(_listSchoolClasses[i]));
			}
			FillStudents();
		}

		private void FillStudents()
		{
			long selectedProvNum = 0;
			long schoolClass = 0;
			if (comboClass.SelectedIndex > 0)
			{
				schoolClass = _listSchoolClasses[comboClass.SelectedIndex - 1].Id;
			}
			long.TryParse(textProvNum.Text, out selectedProvNum);
			DataTable table = Providers.RefreshForDentalSchool(schoolClass, textLName.Text, textFName.Text, textProvNum.Text, false, false);
			gridStudents.BeginUpdate();
			gridStudents.Columns.Clear();
			GridColumn col;
			col = new GridColumn("ProvNum", 60);
			gridStudents.Columns.Add(col);
			col = new GridColumn("Last Name", 90);
			gridStudents.Columns.Add(col);
			col = new GridColumn("First Name", 90);
			gridStudents.Columns.Add(col);
			col = new GridColumn("Class", 100);
			gridStudents.Columns.Add(col);
			gridStudents.Rows.Clear();
			GridRow row;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				row = new GridRow();
				row.Cells.Add(table.Rows[i]["ProvNum"].ToString());
				row.Cells.Add(table.Rows[i]["LName"].ToString());
				row.Cells.Add(table.Rows[i]["FName"].ToString());
				if (table.Rows[i]["GradYear"].ToString() != "")
				{
					row.Cells.Add(table.Rows[i]["GradYear"].ToString() + "-" + table.Rows[i]["Descript"].ToString());
				}
				else
				{
					row.Cells.Add("");
				}
				row.Tag = table.Rows[i]["ProvNum"];
				gridStudents.Rows.Add(row);
			}
			gridStudents.EndUpdate();
			for (int i = 0; i < table.Rows.Count; i++)
			{
				if (table.Rows[i]["ProvNum"].ToString() == selectedProvNum.ToString())
				{
					gridStudents.SetSelected(i, true);
					break;
				}
			}
		}

		private void FillDispSupply()
		{
			long selectedProvNum = 0;
			long.TryParse(gridStudents.Rows[gridStudents.GetSelectedIndex()].Tag.ToString(), out selectedProvNum);
			DataTable table = DispSupplies.RefreshDispensary(PIn.Long(textProvNum.Text));
			gridDispSupply.BeginUpdate();
			gridDispSupply.Columns.Clear();
			GridColumn col;
			col = new GridColumn("DateDispensed", 100);
			gridDispSupply.Columns.Add(col);
			col = new GridColumn("Description", 90);
			gridDispSupply.Columns.Add(col);
			col = new GridColumn("Qty", 40);
			gridDispSupply.Columns.Add(col);
			col = new GridColumn("Note", 100);
			gridDispSupply.Columns.Add(col);
			gridDispSupply.Rows.Clear();
			GridRow row;
			for (int i = 0; i < table.Rows.Count; i++)
			{
				row = new GridRow();
				row.Cells.Add(table.Rows[i]["DateDispensed"].ToString());
				row.Cells.Add(table.Rows[i]["Descript"].ToString());
				row.Cells.Add(table.Rows[i]["DispQuantity"].ToString());
				row.Cells.Add(table.Rows[i]["Note"].ToString());
				gridDispSupply.Rows.Add(row);
			}
			gridDispSupply.EndUpdate();
		}

		private void gridStudents_CellClick(object sender, ODGridClickEventArgs e)
		{
			FillDispSupply();
		}

		private void menuItemClose_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}


		private void butOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
