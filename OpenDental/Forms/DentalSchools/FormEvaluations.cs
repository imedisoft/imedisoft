using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
	public partial class FormEvaluations : FormBase
	{
		public FormEvaluations()
		{
			InitializeComponent();
		}

		private void FormEvaluations_Load(object sender, EventArgs e)
		{
			var isEvaluationAdministrator = Security.IsAuthorized(Permissions.AdminDentalEvaluations, true);

			var userProvider = Providers.GetById(Security.CurrentUser.ProviderId);
			if ((userProvider == null || !userProvider.IsInstructor) && !isEvaluationAdministrator)
			{
				ShowError(Translation.DentalSchools.OnlyInstructorsMayViewOrEditEvaluations);

				Close();

				return;
			}

			dateStartTextBox.Text = DateTime.Today.ToShortDateString();
			dateEndTextBox.Text = DateTime.Today.ToShortDateString();

			schoolCourseComboBox.Items.Add(Translation.Common.All);
			schoolCourseComboBox.SelectedIndex = 0;

			foreach (var schoolCourse in SchoolCourses.GetAll())
            {
				schoolCourseComboBox.Items.Add(schoolCourse);
			}

			instructorComboBox.Items.Add(Translation.Common.All);
			instructorComboBox.SelectedIndex = 0;

			foreach (var provider in Providers.GetInstructors())
			{
				instructorComboBox.Items.Add(provider);
				if (userProvider != null && userProvider.Id == provider.Id)
				{
					instructorComboBox.SelectedItem = provider;
				}
			}

			instructorComboBox.Enabled = !isEvaluationAdministrator;

			if (userProvider == null)
            {
				addButton.Enabled = false;
            }

			FillGrid();
		}

		private void FillGrid()
		{
			var schoolCourse = schoolCourseComboBox.SelectedItem as SchoolCourse;
			var instructor = instructorComboBox.SelectedItem as Provider;

			long? providerId = null;
			if (!string.IsNullOrEmpty(providerIdTextBox.Text))
            {
				if (long.TryParse(providerIdTextBox.Text, out var result))
                {
					providerId = result;
                }
            }

			var evaluations = 
				Evaluations.GetSummaries(
					DateTime.Parse(dateStartTextBox.Text), 
					DateTime.Parse(dateEndTextBox.Text), 
					lastNameTextBox.Text, firstNameTextBox.Text,
					providerId, schoolCourse?.Id, instructor?.Id);
			
			evaluationsGrid.BeginUpdate();
			evaluationsGrid.Columns.Clear();
			evaluationsGrid.Columns.Add(new GridColumn(Translation.Common.Date, 70, HorizontalAlignment.Center));
			evaluationsGrid.Columns.Add(new GridColumn(Translation.Common.Title, 90));
			evaluationsGrid.Columns.Add(new GridColumn(Translation.DentalSchools.Instructor, 90));
			evaluationsGrid.Columns.Add(new GridColumn(Translation.Common.ProviderIdAbbr, 60));
			evaluationsGrid.Columns.Add(new GridColumn(Translation.Common.LastName, 90));
			evaluationsGrid.Columns.Add(new GridColumn(Translation.Common.FirstName, 80));
			evaluationsGrid.Columns.Add(new GridColumn(Translation.DentalSchools.Course, 90));
			evaluationsGrid.Columns.Add(new GridColumn(Translation.DentalSchools.Grade, 60));
			evaluationsGrid.Columns.Add(new GridColumn(Translation.DentalSchools.GradingScale, 90));
			evaluationsGrid.Rows.Clear();

			foreach (var summary in evaluations)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(summary.EvaluationDate.ToShortDateString());
				gridRow.Cells.Add(summary.EvaluationTitle);
				gridRow.Cells.Add(summary.InstructorId.ToString());
				gridRow.Cells.Add(summary.StudentId.ToString());
				gridRow.Cells.Add(summary.LastName);
				gridRow.Cells.Add(summary.FirstName);
				gridRow.Cells.Add(summary.CourseID);
				gridRow.Cells.Add(summary.Grade);
				gridRow.Cells.Add(summary.GradingScale);
				gridRow.Tag = summary;

				evaluationsGrid.Rows.Add(gridRow);
			}

			evaluationsGrid.EndUpdate();
		}

		private void EvaluationsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void EvaluationsGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = evaluationsGrid.SelectedRows.Count > 0;
		}

		private void DateTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (sender is TextBox textBox)
			{
				if (!DateTime.TryParse(textBox.Text, out var result))
				{
					e.Cancel = true;
				}
				else
				{
					textBox.Text = result.ToShortDateString();
				}
			}
		}

		private void SchoolCourseComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void ProviderIdTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void RefreshButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(dateStartTextBox.Text) || string.IsNullOrEmpty(dateEndTextBox.Text))
            {
				ShowError(Translation.Common.PleaseEnterDate);

				return;
            }

			if (!DateTime.TryParse(dateStartTextBox.Text, out _) || !DateTime.TryParse(dateEndTextBox.Text, out _))
            {
				ShowError(Translation.Common.PleaseEnterValidDate);

				return;
            }

			FillGrid();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var schoolCourse = schoolCourseComboBox.SelectedItem as SchoolCourse;

            using var formEvaluationDefs = new FormEvaluationDefs(schoolCourse?.Id)
            {
                IsSelectionMode = true,
            };

            if (formEvaluationDefs.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			CreateNewEvaluation(formEvaluationDefs.SelectedEvaluationDef);
		}

		private void CreateNewEvaluation(EvaluationDef evaluationDef)
		{
			if (!Security.CurrentUser.ProviderId.HasValue)
            {
				return;
            }

			var evaluation = new Evaluation
			{
				EvaluationDate = DateTime.Today,
				Title = evaluationDef.Title,
				GradingScaleId = evaluationDef.GradingScaleId,
				InstructorId = Security.CurrentUser.ProviderId.Value,
				SchoolCourseId = evaluationDef.SchoolCourseId
			};

			Evaluations.Save(evaluation);

			foreach (var evaluationCriterionDef in EvaluationCriterionDefs.GetAllForEvaluationDef(evaluationDef.Id))
			{
				var evaluationCriterion = new EvaluationCriterion
				{
					Description = evaluationCriterionDef.Description,
					EvaluationId = evaluation.Id,
					GradingScaleId = evaluationCriterionDef.GradingScaleId,
					IsCategory = evaluationCriterionDef.IsCategory,
					SortOrder = evaluationCriterionDef.SortOrder,
					MaxPointsAllowed = evaluationCriterionDef.MaxPointsAllowed
				};

				EvaluationCriterions.Save(evaluationCriterion);
			}

			using var formEvaluationEdit = new FormEvaluationEdit(evaluation);
			if (formEvaluationEdit.ShowDialog(this) != DialogResult.OK)
            {
				Evaluations.Delete(evaluation);

				return;
            }

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var summary = evaluationsGrid.SelectedTag<Evaluations.Summary>();
			if (summary == null)
			{
				return;
			}

			var evaluation = Evaluations.GetById(summary.EvaluationId);
			if (evaluation == null)
			{
				return;
			}

			using var formEvaluationEdit = new FormEvaluationEdit(evaluation);
			if (formEvaluationEdit.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var summary = evaluationsGrid.SelectedTag<Evaluations.Summary>();
			if (summary == null)
			{
				return;
			}

			var evaluation = Evaluations.GetById(summary.EvaluationId);
			if (evaluation == null)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
            {
				return;
            }

			Evaluations.Delete(evaluation);

			FillGrid();
		}

		private void ReportsButton_Click(object sender, EventArgs e)
		{
			using var formEvaluationReport = new FormEvaluationReport();

			formEvaluationReport.ShowDialog(this);
		}
    }
}
