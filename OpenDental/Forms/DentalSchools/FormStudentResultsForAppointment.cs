using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormStudentResultsForAppointment : FormBase
	{
		private readonly long patientId;
		private readonly long apptId;

		public FormStudentResultsForAppointment(long patientId, long apptId)
		{
			InitializeComponent();

			this.patientId = patientId;
			this.apptId = apptId;
		}

		private void FormStudentResultsForAppointment_Load(object sender, EventArgs e)
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

			instructorComboBox.Items.Add(Translation.Common.None);
			instructorComboBox.SelectedIndex = 0;

			var providerId = StudentResults.GetByAppt(apptId).FirstOrDefault()?.ProviderId;

			foreach (var provider in Providers.GetDeepCopy(true))
			{
				instructorComboBox.Items.Add(provider);
				if (provider.Id == providerId)
                {
					instructorComboBox.SelectedItem = provider;
                }
			}

			FillStudents();
			FillRequirements();
			FillStudentResults();
		}

		private void FillStudents()
		{
            if (!(classComboBox.SelectedItem is SchoolClass schoolClass))
            {
                return;
            }

            studentsGrid.BeginUpdate();
			studentsGrid.ListGridColumns.Clear();
			studentsGrid.ListGridColumns.Add(new GridColumn("", 100));
			studentsGrid.ListGridRows.Clear();

			foreach (var student in StudentResults.GetStudents(schoolClass.Id))
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(student.LastName + ", " + student.FirstName);
				gridRow.Tag = student;

				studentsGrid.ListGridRows.Add(gridRow);
			}

			studentsGrid.EndUpdate();
		}

		private void FillRequirements()
		{
			var student = studentsGrid.SelectedTag<Provider>();

            requirementsGrid.BeginUpdate();
            requirementsGrid.ListGridColumns.Clear();
			requirementsGrid.ListGridColumns.Add(new GridColumn("", 100));
			requirementsGrid.ListGridRows.Clear();

			if (student == null ||
				!(classComboBox.SelectedItem is SchoolClass schoolClass) || 
				!(courseComboBox.SelectedItem is SchoolCourse schoolCourse))
            {
				requirementsGrid.EndUpdate();

				return;
            }

			foreach (var requirement in SchoolCourseRequirements.GetByCourseAndClass(schoolClass.Id, schoolCourse.Id))
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(requirement.Description);
				gridRow.Tag = requirement;

				requirementsGrid.ListGridRows.Add(gridRow);
			}

			requirementsGrid.EndUpdate();
		}

		private void FillStudentResults()
		{
			resultsGrid.BeginUpdate();
			resultsGrid.ListGridColumns.Clear();
			resultsGrid.ListGridColumns.Add(new GridColumn(Translation.DentalSchools.Student, 130));
			resultsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Description, 150));
			resultsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Completed, 40));
			resultsGrid.ListGridRows.Clear();

			foreach (var studentResult in StudentResults.GetByAppt(apptId))
            {
				var gridRow = new GridRow();
				gridRow.Cells.Add(Providers.GetAbbr(studentResult.ProviderId));
				gridRow.Cells.Add(studentResult.Description);
				gridRow.Cells.Add(studentResult.CompletionDate.HasValue ? "X" : "");
				gridRow.Tag = studentResult;

				resultsGrid.ListGridRows.Add(gridRow);
			}


			resultsGrid.EndUpdate();
		}

		private void StudentsGrid_CellClick(object sender, ODGridClickEventArgs e)
		{
			FillRequirements();
		}

		private void RequirementsGrid_SelectionCommitted(object sender, EventArgs e)
		{
			addButton.Enabled = requirementsGrid.SelectedTag<SchoolCourseRequirements>() != null;
		}

		private void ResultsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var studentResult = resultsGrid.SelectedTag<StudentResult>();
			if (studentResult == null)
            {
				return;
            }

			using var formStudentResultEdit = new FormStudentResultEdit(studentResult);
			if (formStudentResultEdit.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			FillStudentResults();
		}

		private void ResultsGrid_SelectionCommitted(object sender, EventArgs e)
		{
			removeButton.Enabled = resultsGrid.SelectedTag<StudentResult>() != null;
		}

		private void ClassComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillStudents();
			FillRequirements();
		}

		private void CourseComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillRequirements();
		}

		private void InstructorComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			var instructor = instructorComboBox.SelectedItem as Provider;
			var instructorId = instructor?.Id;

			foreach (var studentResult in resultsGrid.GetTags<StudentResult>())
			{
				if (studentResult.CompletionDate.HasValue) continue;

				if (studentResult.InstructorId != instructorId)
				{
					studentResult.InstructorId = instructorId;

					StudentResults.Save(studentResult);
				}
			}
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var student = studentsGrid.SelectedTag<Provider>();
			var instructor = instructorComboBox.SelectedItem as Provider;

			if (student == null || !(courseComboBox.SelectedItem is SchoolCourse schoolCourse))
            {
                return;
            }

            if (requirementsGrid.SelectedTag<SchoolCourseRequirement>() == null)
			{
				ShowError(Translation.DentalSchools.PleaseSelectAtLeastOneRequirementFromListLeft);

				return;
			}

			var results = resultsGrid.GetTags<StudentResult>();
			foreach (var requirement in requirementsGrid.SelectedTags<SchoolCourseRequirement>())
			{
				var result = results.FirstOrDefault(r => r.SchoolCourseRequirementId == requirement.Id);
                if (result == null)
                {
					result = new StudentResult
					{
						ApptId = apptId,
						Description = requirement.Description,
						InstructorId = instructor?.Id,
						PatientId = patientId,
						ProviderId = student.Id,
						SchoolCourseRequirementId = requirement.Id,
						SchoolCourseId = schoolCourse.Id
					};
				}
                else
                {
					result.Description = requirement.Description;
					result.InstructorId = instructor?.Id;
					result.SchoolCourseId = schoolCourse.Id;
				}

				StudentResults.Save(result);
			}

			FillStudentResults();
		}

		private void RemoveButton_Click(object sender, EventArgs e)
		{
			if (resultsGrid.SelectedTag<StudentResult>() == null)
			{
				ShowError(Translation.DentalSchools.PleaseSelectAtLeastOneResultFromListBelow);

				return;
			}

			foreach (var studentResult in resultsGrid.SelectedTags<StudentResult>())
            {
				studentResult.ApptId = null;

				StudentResults.Save(studentResult);
            }

			FillStudentResults();
		}

        private void CancelButton_Click(object sender, EventArgs e)
        {
			Close();
        }
    }
}
