using Imedisoft.Data;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
	public partial class FormStudentResultsOne : FormBase
	{
		private readonly long studentProviderId;
		private Provider studentProvider;

		public FormStudentResultsOne(long studentProviderId)
		{
			InitializeComponent();

			this.studentProviderId = studentProviderId;
		}

		private void FormStudentResultsOne_Load(object sender, EventArgs e)
		{
			studentProvider = Providers.GetById(studentProviderId);
			if (studentProvider == null)
			{
				return;
			}

			Text = Text + " - " + studentProvider.ToString();

			FillGrid();
		}

		private void FillGrid()
		{
			requirementSummaryGrid.BeginUpdate();
			requirementSummaryGrid.Columns.Clear();
			requirementSummaryGrid.Columns.Add(new GridColumn(Translation.DentalSchools.Course, 100));
			requirementSummaryGrid.Columns.Add(new GridColumn(Translation.DentalSchools.Requirement, 200));
			requirementSummaryGrid.Columns.Add(new GridColumn(Translation.Common.Done, 40));
			requirementSummaryGrid.Columns.Add(new GridColumn(Translation.Common.Patient, 140));
			requirementSummaryGrid.Columns.Add(new GridColumn(Translation.Common.Appointment, 190));
			requirementSummaryGrid.Rows.Clear();

			foreach (var summary in StudentResults.GetSummaryForStudentResults(studentProviderId))
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(summary.Course);
				gridRow.Cells.Add(summary.Requirement);
				gridRow.Cells.Add(summary.Completed ? "X" : "");
				gridRow.Cells.Add(summary.Patient);
				gridRow.Cells.Add(summary.Appointment);
				gridRow.Tag = summary;

				requirementSummaryGrid.Rows.Add(gridRow);
			}

			requirementSummaryGrid.EndUpdate();
		}

		private void RequirementSummaryGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var summary = requirementSummaryGrid.SelectedTag<StudentResults.StudentResultSummary>();
			if (summary == null)
			{
				return;
			}

			var studentResult = StudentResults.GetById(summary.StudentResultId);
			if (studentResult == null)
			{
				return;
			}

			using var formReqStudentEdit = new FormStudentResultEdit(studentResult);
			if (formReqStudentEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
