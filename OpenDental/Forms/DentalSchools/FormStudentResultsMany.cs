using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using System;

namespace Imedisoft.Forms
{
    public partial class FormStudentResultsMany : FormBase
	{
		public FormStudentResultsMany()
		{
			InitializeComponent();
		}

		private void FormStudentResultsMany_Load(object sender, EventArgs e)
		{
			foreach (var schoolClass in SchoolClasses.GetAll())
			{
				classComboBox.Items.Add(schoolClass);
			}

			foreach (var schoolCourse in SchoolCourses.GetAll())
			{
				courseComboBox.Items.Add(schoolCourse);
			}

			if (classComboBox.Items.Count > 0) 
				classComboBox.SelectedIndex = 0;

			if (courseComboBox.Items.Count > 0)
				courseComboBox.SelectedIndex = 0;
			
			FillGrid();
		}

		private void FillGrid()
		{
            if (!(classComboBox.SelectedItem is SchoolClass schoolClass) || 
				!(courseComboBox.SelectedItem is SchoolCourse schoolCourse))
            {
                return;
            }

            studentsGrid.BeginUpdate();
			studentsGrid.ListGridColumns.Clear();
			studentsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.LastName, 100));
			studentsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.FirstName, 100));
			studentsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Done, 50));
			studentsGrid.ListGridRows.Clear();

			foreach (var summary in StudentResults.GetSummaryForStudents(schoolClass.Id, schoolCourse.Id))
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(summary.LastName);
				gridRow.Cells.Add(summary.FirstName);
				gridRow.Cells.Add($"{summary.ReqCompleted}/{summary.ReqTotal}");
				gridRow.Tag = summary;

				studentsGrid.ListGridRows.Add(gridRow);
			}

			studentsGrid.EndUpdate();
		}

		private void StudentsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var summary = studentsGrid.SelectedTag<StudentResults.StudentSummary>();
			if (summary == null)
			{
				return;
			}

			using var formStudentResultsOne = new FormStudentResultsOne(summary.StudentProviderId);

			formStudentResultsOne.ShowDialog(this);

			FillGrid();
		}

		private void ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
