using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Imedisoft.Forms
{
    public partial class FormEvaluationReport : FormBase
	{
		private bool changedStudentSelectionFromGrid;
		private FormQuery formQuery;

		public FormEvaluationReport()
		{
			InitializeComponent();
		}

		private void FormEvaluationReport_Load(object sender, EventArgs e)
		{
			dateStartTextBox.Text = DateTime.Today.AddMonths(-4).ToShortDateString();
			dateEndTextBox.Text = DateTime.Today.ToShortDateString();

			FillCourses();
			FillInstructors();
		}

		private void FillCourses()
		{
			schoolCoursesGrid.BeginUpdate();
			schoolCoursesGrid.Columns.Clear();
			schoolCoursesGrid.Columns.Add(new GridColumn(Translation.DentalSchools.Course, 60));
			schoolCoursesGrid.Columns.Add(new GridColumn(Translation.Common.Description, 90));
			schoolCoursesGrid.Rows.Clear();
	
			foreach (var schoolCourse in SchoolCourses.GetAll())
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(schoolCourse.CourseID);
				gridRow.Cells.Add(schoolCourse.Description);
				gridRow.Tag = schoolCourse;

				schoolCoursesGrid.Rows.Add(gridRow);
			}

			schoolCoursesGrid.EndUpdate();
		}

		private void FillInstructors()
		{
			instructorsGrid.BeginUpdate();
			instructorsGrid.Columns.Clear();
			instructorsGrid.Columns.Add(new GridColumn(Translation.Common.ProviderIdAbbr, 50));
			instructorsGrid.Columns.Add(new GridColumn(Translation.Common.LastName, 80));
			instructorsGrid.Columns.Add(new GridColumn(Translation.Common.FirstName, 80));
			instructorsGrid.Rows.Clear();

			foreach (var provider in Providers.GetInstructors())
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(provider.Id.ToString());
				gridRow.Cells.Add(provider.LastName);
				gridRow.Cells.Add(provider.FirstName);
				gridRow.Tag = provider;

				instructorsGrid.Rows.Add(gridRow);
			}

			instructorsGrid.EndUpdate();
		}

		private void FillStudents()
		{
			var schoolCourseIds = 
				schoolCoursesGrid.SelectedTags<SchoolCourse>().Select(sc => sc.Id);

			var instructorIds = 
				instructorsGrid.SelectedTags<Provider>().Select(prov => prov.Id);

			studentsGrid.BeginUpdate();
			studentsGrid.Columns.Clear();
			studentsGrid.Columns.Add(new GridColumn(Translation.Common.ProviderIdAbbr, 60));
			studentsGrid.Columns.Add(new GridColumn(Translation.Common.LastName, 80));
			studentsGrid.Columns.Add(new GridColumn(Translation.Common.FirstName, 80));
			studentsGrid.Rows.Clear();

			foreach (var studentInfo in Evaluations.GetStudents(schoolCourseIds, instructorIds))
			{
				var row = new GridRow();
				row.Cells.Add(studentInfo.ProviderId.ToString());
				row.Cells.Add(studentInfo.LastName);
				row.Cells.Add(studentInfo.FirstName);
				row.Tag = studentInfo;

				studentsGrid.Rows.Add(row);
			}

			studentsGrid.EndUpdate();
			studentsGrid.SetSelected(allStudentsCheckBox.Checked);
		}

		private void AllInstructorsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			instructorsGrid.SetSelected(allInstructorsCheckBox.Checked);
			instructorsGrid.Visible = !allInstructorsCheckBox.Checked;

			FillStudents();
		}

		private void AllCoursesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			schoolCoursesGrid.SetSelected(allCoursesCheckBox.Checked);
			schoolCoursesGrid.Visible = !allCoursesCheckBox.Checked;

			FillStudents();
		}

		private void AllStudentsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (changedStudentSelectionFromGrid) return;

			schoolCoursesGrid.SetSelected(allStudentsCheckBox.Checked);
		}

		private void AllStudentsButton_Click(object sender, EventArgs e)
		{
			studentsGrid.SetSelected(true);
		}

		private void SchoolCoursesGrid_CellClick(object sender, ODGridClickEventArgs e)
		{
			FillStudents();
		}

		private void InstructorsGrid_CellClick(object sender, ODGridClickEventArgs e)
		{
			FillStudents();
		}

		private void StudentsGrid_SelectionCommitted(object sender, EventArgs e)
		{
			changedStudentSelectionFromGrid = true;

			allStudentsCheckBox.Checked = studentsGrid.SelectedRows.Count == studentsGrid.Rows.Count;

			changedStudentSelectionFromGrid = false;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(dateStartTextBox.Text) || string.IsNullOrEmpty(dateEndTextBox.Text))
            {
				ShowError(Translation.Common.PleaseEnterDate);

				return;
            }

			if (!DateTime.TryParse(dateStartTextBox.Text, out var dateStart) || 
				!DateTime.TryParse(dateEndTextBox.Text, out var dateEnd))
            {
				ShowError(Translation.Common.PleaseEnterValidDate);

				return;
            }

			if (schoolCoursesGrid.SelectedIndices.Length < 1)
			{
				ShowError("At least one course must be selected to run a report. Please select a row from the course grid.");
				return;
			}

			if (instructorsGrid.SelectedIndices.Length < 1)
			{
				ShowError("At least one instructor must be selected to run a report. Please select a row from the instructor grid.");

				return;
			}

			if (studentsGrid.SelectedIndices.Length < 1)
			{
				ShowError("At least one student must be selected to run a report. Please select a row from the student grid.");

				return;
			}

			var criteria = new List<string>();
			if (!allCoursesCheckBox.Checked)
            {
				var schoolCourseIds = schoolCoursesGrid.SelectedTags<SchoolCourse>().Select(sc => sc.Id).ToList();
				if (schoolCourseIds.Count > 0)
                {
					criteria.Add("evaluations.school_course_id IN (" + string.Join(", ", schoolCourseIds) + ")");
                }
                else
                {
					criteria.Add("evaluations.school_course_id IN (NULL)");
				}
            }

			if (!allInstructorsCheckBox.Checked)
			{
				var instructorIds = instructorsGrid.SelectedTags<Provider>().Select(prov => prov.Id).ToList();
				if (instructorIds.Count > 0)
                {
					criteria.Add("evaluations.instructor_id IN (" + string.Join(", ", instructorIds) + ")");
                }
                else
                {
					criteria.Add("evaluations.instructor_id IN (NULL)");
				}
			}

			var studentIds = studentsGrid.SelectedTags<Evaluations.StudentInfo>().Select(si => si.ProviderId).ToList();
			if (studentIds.Count > 0)
            {
				criteria.Add("evaluations.student_id IN (" + string.Join(", ", studentIds) + ")");
            }
            else
            {
				criteria.Add("evaluations.student_id IN (NULL)");
			}

			static string DbDate(DateTime dateTime) => '\'' + dateTime.ToString("yyyy-MM-dd") + '\'';

			var command = 
				"SELECT " +
					"CONCAT(student.last_name, ', ', student.first_name) AS student_name, " +
					"eval.evaluation_date, " +
					"course.course_id, " +
					"CONCAT(instructor.last_name, ', ', instructor.first_name) AS instructor_name, " +
					"eval.title, " +
					"scale.type AS grade_type, " +
					"eval.overall_grade_showing, " +
					"eval.overall_grade_number " +
				"FROM evaluations eval " +
				"INNER JOIN provider student ON eval.student_id = student.id " +
				"INNER JOIN provider instructor ON eval.instructor_id = instructor.id " +
				"INNER JOIN gradingscale scale ON eval.grading_scale_id = scale.id " +
				"INNER JOIN school_courses course ON eval.school_course_id = course.id " +
				"WHERE (eval.evaluation_date BETWEEN " + DbDate(dateStart) + " AND " + DbDate(dateEnd) + ") " +
				"AND " + string.Join(" AND ", criteria) + " " +
				"ORDER BY StudentName, eval.evaluation_date";

            var report = new ReportSimpleGrid
            {
                Query = command
            };

            formQuery = new FormQuery(report);
			formQuery.IsReport = true;

			var dataTable = report.GetTempTable();

			report.TableQ = new DataTable();
			for (int i = 0; i < 10; i++)
			{
				report.TableQ.Columns.Add(new DataColumn());
			}

			report.InitializeColumns();
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				var dataRow = report.TableQ.NewRow(); // create new row called 'row' based on structure of TableQ
				dataRow[0] = dataTable.Rows[i]["student_name"].ToString();
				dataRow[1] = PIn.Date(dataTable.Rows[i]["evaluation_date"].ToString()).ToShortDateString();
				dataRow[2] = dataTable.Rows[i]["course_id"].ToString();
				dataRow[3] = dataTable.Rows[i]["instructor_name"].ToString();
				dataRow[4] = dataTable.Rows[i]["title"].ToString();
				switch ((GradingScaleType)PIn.Int(dataTable.Rows[i]["grade_type"].ToString()))
				{
					case GradingScaleType.PickList:
						dataRow[5] = Enum.GetName(typeof(GradingScaleType), (int)GradingScaleType.PickList);
						break;

					case GradingScaleType.Percentage:
						dataRow[5] = Enum.GetName(typeof(GradingScaleType), (int)GradingScaleType.Percentage);
						break;

					case GradingScaleType.Weighted:
						dataRow[5] = Enum.GetName(typeof(GradingScaleType), (int)GradingScaleType.Weighted);
						break;
				}
				dataRow[6] = dataTable.Rows[i]["overall_grade_showing"].ToString();
				dataRow[7] = dataTable.Rows[i]["overall_grade_number"].ToString();
				report.TableQ.Rows.Add(dataRow);
			}

			formQuery.ResetGrid();

			report.Title = Translation.DentalSchools.CourseAverage;
			report.SubTitle.Add(dateStart.ToShortDateString() + " - " + dateEnd.ToShortDateString());

			if (allInstructorsCheckBox.Checked)
			{
				report.SubTitle.Add(Translation.DentalSchools.AllInstructors);
			}

			if (allCoursesCheckBox.Checked)
			{
				report.SubTitle.Add(Translation.DentalSchools.AllCourses);
			}

			report.SetColumn(this, 0, Translation.DentalSchools.Student, 120);
			report.SetColumn(this, 1, Translation.Common.Date, 80);
			report.SetColumn(this, 2, Translation.DentalSchools.Course, 100);
			report.SetColumn(this, 3, Translation.DentalSchools.Instructor, 120);
			report.SetColumn(this, 4, Translation.DentalSchools.Evaluation, 90);
			report.SetColumn(this, 5, Translation.DentalSchools.GradingScaleType, 90);
			report.SetColumn(this, 6, Translation.DentalSchools.GradeShowing, 100);
			report.SetColumn(this, 7, Translation.DentalSchools.GradeNumber, 100);

			formQuery.ShowDialog();
		}
    }
}
