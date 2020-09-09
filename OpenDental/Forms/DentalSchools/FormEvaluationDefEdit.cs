using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEvaluationDefEdit : FormBase
	{
		private readonly EvaluationDef evaluationDef;
		private readonly List<EvaluationCriterionDef> evaluationCriterionDefs = new List<EvaluationCriterionDef>();
		private readonly List<EvaluationCriterionDef> evaluationCriterionDefsDeleted = new List<EvaluationCriterionDef>();
		private Dictionary<long, GradingScale> gradingScales;
		private GradingScale gradingScale;
		private SchoolCourse schoolCourse;
		private bool hasChanges;

		public FormEvaluationDefEdit(EvaluationDef evaluationDef)
		{
			InitializeComponent();

			this.evaluationDef = evaluationDef;
		}

		private void FormEvaluationDefEdit_Load(object sender, EventArgs e)
		{
			titleTextBox.Text = evaluationDef.Title;

			schoolCourse = SchoolCourses.GetById(evaluationDef.SchoolCourseId);
			if (schoolCourse != null)
            {
				schoolCourseTextBox.Text = SchoolCourses.GetDescription(schoolCourse);
            }

			gradingScales = GradingScales.GetAll().ToDictionary(gc => gc.Id);
			gradingScales.TryGetValue(evaluationDef.GradingScaleId, out gradingScale);
			if (gradingScale != null)
			{
				gradingScaleTextBox.Text = gradingScale.Description;

				if (gradingScale.Type != GradingScaleType.Weighted)
				{
					totalPointsLabel.Visible = false;
					totalPointsTextBox.Visible = false;
				}
			}

			evaluationCriterionDefs.AddRange(EvaluationCriterionDefs.GetAllForEvaluationDef(evaluationDef.Id));

			FillGrid();
		}

		private void FillGrid()
		{
			bool isWeighted = gradingScale != null && gradingScale.Type == GradingScaleType.Weighted;

			evaluationCriterionDefsGrid.BeginUpdate();
			evaluationCriterionDefsGrid.ListGridColumns.Clear();
			evaluationCriterionDefsGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Description, 140));
			evaluationCriterionDefsGrid.ListGridColumns.Add(new GridColumn(Translation.DentalSchools.GradingScale, 100));
			if (isWeighted)
			{
				evaluationCriterionDefsGrid.ListGridColumns.Add(new GridColumn(Translation.DentalSchools.MaxPoints, 80));
			}
			evaluationCriterionDefsGrid.ListGridRows.Clear();

			var totalPoints = 0f;
			foreach (var evaluationCriterionDef in evaluationCriterionDefs)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(evaluationCriterionDef.Description);
				gridRow.Tag = evaluationCriterionDef;

				if (evaluationCriterionDef.IsCategory)
				{
					gridRow.Bold = true;
					gridRow.Cells.Add("");
				}
				else
				{
					if (gradingScales.TryGetValue(evaluationCriterionDef.GradingScaleId, out var gradingScale))
					{
						gridRow.Cells.Add(gradingScales[evaluationCriterionDef.GradingScaleId].Description);
					}
                    else
                    {
						gridRow.Cells.Add(Translation.Common.TagNull);
                    }
				}

				if (isWeighted)
				{
					totalPoints += evaluationCriterionDef.MaxPointsAllowed;
					if (evaluationCriterionDef.IsCategory)
					{
						gridRow.Cells.Add("");
					}
					else
					{
						gridRow.Cells.Add(evaluationCriterionDef.MaxPointsAllowed.ToString());
					}
				}

				evaluationCriterionDefsGrid.ListGridRows.Add(gridRow);
			}

			evaluationCriterionDefsGrid.EndUpdate();

			totalPointsTextBox.Text = isWeighted ? totalPoints.ToString() : Translation.Common.NotApplicableAbbr;
		}

		private void GradingScaleButton_Click(object sender, EventArgs e)
		{
			using var formGradingScales = new FormGradingScales
			{
				IsSelectionMode = true
			};

			if (formGradingScales.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			gradingScale = formGradingScales.SelectedGradingScale;
			gradingScaleTextBox.Text = gradingScale.Description;
			gradingScales[gradingScale.Id] = gradingScale;

			FillGrid();
		}

		private void SchoolCourseButton_Click(object sender, EventArgs e)
		{
			using var formSchoolCourses = new FormSchoolCourses
			{
				IsSelectionMode = true
			};

			if (formSchoolCourses.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			schoolCourse = formSchoolCourses.SelectedSchoolCourse;
			schoolCourseTextBox.Text = schoolCourse.CourseID;
		}

		private void EvaluationCriterionDefsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void EvaluationCriterionDefsGrid_SelectionCommitted(object sender, EventArgs e)
		{
			var selectedIndices = evaluationCriterionDefsGrid.SelectedIndices;

			editButton.Enabled = deleteButton.Enabled =
				selectedIndices.Length > 0;

			upButton.Enabled = 
				selectedIndices.Any(
					index => index > 0);

			downButton.Enabled = 
				selectedIndices.Any(
					index => index < evaluationCriterionDefsGrid.ListGridRows.Count - 1);
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			if (gradingScale == null)
			{
				ShowError(Translation.DentalSchools.PleaseSelectGradingScaleBeforeAddingCriterion);

				return;
			}

            var evaluationCriterionDef = new EvaluationCriterionDef
            {
                EvaluationDefId = evaluationDef.Id,
                GradingScaleId = gradingScale.Id,
				SortOrder = evaluationCriterionDefs.Count + 1
            };

            using var formEvaluationCriterionDefEdit = new FormEvaluationCriterionDefEdit(evaluationCriterionDef);
			if (formEvaluationCriterionDefEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			evaluationCriterionDefs.Add(evaluationCriterionDef);

			hasChanges = true;

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var evaluationCriterionDef = evaluationCriterionDefsGrid.SelectedTag<EvaluationCriterionDef>();
			if (evaluationCriterionDef == null)
			{
				return;
			}

			using var formEvaluationCriterionDefEdit = new FormEvaluationCriterionDefEdit(evaluationCriterionDef);
			if (formEvaluationCriterionDefEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var evaluationCriterionDef = evaluationCriterionDefsGrid.SelectedTag<EvaluationCriterionDef>();
			if (evaluationCriterionDef == null)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
			{
				return;
			}

			evaluationCriterionDefs.Remove(evaluationCriterionDef);

			if (evaluationCriterionDef.Id > 0)
            {
				evaluationCriterionDefsDeleted.Add(evaluationCriterionDef);
			}
		}

		private void UpButton_Click(object sender, EventArgs e)
		{
			if (evaluationCriterionDefsGrid.SelectedIndices.Length == 0)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			var selectedIndices = new List<int>();

			foreach (var index in evaluationCriterionDefsGrid.SelectedIndices)
            {
				if (index < 1) continue;

				var item1 = evaluationCriterionDefsGrid.ListGridRows[index].Tag as EvaluationCriterionDef;
				var item2 = evaluationCriterionDefsGrid.ListGridRows[index - 1].Tag as EvaluationCriterionDef;

				(item1.SortOrder, item2.SortOrder) = (item2.SortOrder, item1.SortOrder);

				selectedIndices.Add(index - 1);

				hasChanges = true;
			}

			FillGrid();

			foreach (var index in selectedIndices)
			{
				evaluationCriterionDefsGrid.SetSelected(index, true);
			}
		}

		private void DownButton_Click(object sender, EventArgs e)
		{
			if (evaluationCriterionDefsGrid.SelectedIndices.Length == 0)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			var selectedIndices = new List<int>();

			var lastIndex = evaluationCriterionDefsGrid.ListGridRows.Count - 1;
			foreach (var index in evaluationCriterionDefsGrid.SelectedIndices)
			{
				if (index >= lastIndex) continue;

				var item1 = evaluationCriterionDefsGrid.ListGridRows[index].Tag as EvaluationCriterionDef;
				var item2 = evaluationCriterionDefsGrid.ListGridRows[index + 1].Tag as EvaluationCriterionDef;

				(item1.SortOrder, item2.SortOrder) = (item2.SortOrder, item1.SortOrder);

				selectedIndices.Add(index + 1);

				hasChanges = true;
			}

			FillGrid();

			foreach (var index in selectedIndices)
            {
				evaluationCriterionDefsGrid.SetSelected(index, true);
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var title = titleTextBox.Text.Trim();
			if (title.Length == 0)
            {
				ShowError(Translation.Common.PleaseEnterTitle);

				return;
            }

			if (schoolCourse == null)
			{
				ShowError(Translation.DentalSchools.CourseMustBeSelectedForEvaluationDef);

				return;
			}

			if (gradingScale == null)
			{
				ShowError(Translation.DentalSchools.GradingScaleMustBeSelectedForEvaluationDef);

				return;
			}

			if (!string.IsNullOrWhiteSpace(title) && evaluationDef.Title != title)
			{
				if (!Confirm(Translation.DentalSchools.ConfirmChangeEvaluationDefTitle))
				{
					return;
				}
			}

			evaluationDef.SchoolCourseId = schoolCourse.Id;
			evaluationDef.GradingScaleId = gradingScale.Id;
			evaluationDef.Title = title;

			EvaluationDefs.Save(evaluationDef);

			if (hasChanges)
			{
				foreach (var evaluationCriterionDef in evaluationCriterionDefs)
				{
					evaluationCriterionDef.EvaluationDefId = evaluationDef.Id;

					EvaluationCriterionDefs.Save(evaluationCriterionDef);
				}
			}

			foreach (var deletedEvaluationCriterionDef in evaluationCriterionDefsDeleted)
            {
				EvaluationCriterionDefs.Delete(deletedEvaluationCriterionDef);
			}

			DialogResult = DialogResult.OK;
		}
    }
}
