using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEvaluationDefs : FormBase
	{
		private readonly long? schoolCourseId;

		/// <summary>
		/// Gets or sets a value indicating whether the form is in selection mode.
		/// </summary>
		public bool IsSelectionMode { get; set; }

		/// <summary>
		/// Gets the selected evaluation definition.
		/// </summary>
		public EvaluationDef SelectedEvaluationDef => evaluationDefsGrid.SelectedTag<EvaluationDef>();

		public FormEvaluationDefs(long? schoolCourseId = null)
		{
			InitializeComponent();

			this.schoolCourseId = schoolCourseId;
		}

		private void FormEvaluationDefs_Load(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				duplicateButton.Visible = false;

				addButton.Visible = false;
			}

			schoolCourseComboBox.Items.Add(Translation.Common.All);
			foreach (var schoolCourse in SchoolCourses.GetAll())
			{
				schoolCourseComboBox.Items.Add(schoolCourse);
				if (schoolCourse.Id == schoolCourseId)
                {
					schoolCourseComboBox.SelectedItem = schoolCourse;
                }
			}

			if (schoolCourseComboBox.SelectedItem == null || 
				schoolCourseComboBox.Items.Count > 0)
				schoolCourseComboBox.SelectedIndex = 0;
	
			FillGrid();
		}

		private void FillGrid()
		{
			var schoolCourse = schoolCourseComboBox.SelectedItem as SchoolCourse;
			var schoolCourseId = schoolCourse?.Id;

			evaluationDefsGrid.BeginUpdate();
			evaluationDefsGrid.ListGridColumns.Clear();
			evaluationDefsGrid.ListGridColumns.Add(new GridColumn(Translation.DentalSchools.Course, 100));
			evaluationDefsGrid.ListGridColumns.Add(new GridColumn(Translation.DentalSchools.Evaluation, 180));
			evaluationDefsGrid.ListGridRows.Clear();

			foreach (var summary in EvaluationDefs.GetSummaryForCourse(schoolCourseId))
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(summary.CourseID);
				gridRow.Cells.Add(summary.EvaluationTitle);
				gridRow.Tag = summary;

				evaluationDefsGrid.ListGridRows.Add(gridRow);
			}

			evaluationDefsGrid.EndUpdate();
		}

		private void EvaluationDefsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (IsSelectionMode)
			{
				if (SelectedEvaluationDef == null)
                {
					ShowError(Translation.Common.PleaseSelectItemFirst);

					return;
                }

				DialogResult = DialogResult.OK;

				return;
			}

			EditButton_Click(this, EventArgs.Empty);
		}

		private void EvaluationDefsGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = evaluationDefsGrid.SelectedGridRows.Count > 0;
		}

		private void SchoolCourseComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var evaluationDef = new EvaluationDef();

			using var formEvaluationDefEdit = new FormEvaluationDefEdit(evaluationDef);
			if (formEvaluationDefEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var summary = evaluationDefsGrid.SelectedTag<EvaluationDefs.Summary>();
			if (summary == null)
			{
				return;
			}

			var evaluationDef = EvaluationDefs.GetById(summary.EvaluationDefId);
			if (evaluationDef == null)
			{
				return;
			}

			var formEvaluationDefEdit = new FormEvaluationDefEdit(evaluationDef);
			if (formEvaluationDefEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var summary = evaluationDefsGrid.SelectedTag<EvaluationDefs.Summary>();
			if (summary == null)
			{
				return;
			}

			var evaluationDef = EvaluationDefs.GetById(summary.EvaluationDefId);
			if (evaluationDef == null)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
			{
				return;
			}

			EvaluationDefs.Delete(evaluationDef);

			FillGrid();
		}

		private void DuplicateButton_Click(object sender, EventArgs e)
		{
			if (evaluationDefsGrid.GetSelectedIndex() == -1)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			var summary = evaluationDefsGrid.SelectedTag<EvaluationDefs.Summary>();
			if (summary == null)
			{
				return;
			}

			var evaluationDef = EvaluationDefs.GetById(summary.EvaluationDefId);
			if (evaluationDef == null)
			{
				return;
			}

            var newEvaluationDef = new EvaluationDef
            {
                Id = evaluationDef.Id,
                SchoolCourseId = evaluationDef.SchoolCourseId,
                GradingScaleId = evaluationDef.GradingScaleId,
                Title = evaluationDef.Title + " " + Translation.Common.TagCopy
            };

			EvaluationDefs.Save(newEvaluationDef);

			foreach (var evaluationCriterionDef in EvaluationCriterionDefs.GetAllForEvaluationDef(evaluationDef.Id))
            {
                var newEvaluationCriterionDef = new EvaluationCriterionDef
                {
                    EvaluationDefId = newEvaluationDef.Id,
                    GradingScaleId = evaluationCriterionDef.GradingScaleId,
                    Description = evaluationCriterionDef.Description,
                    IsCategory = evaluationCriterionDef.IsCategory,
                    SortOrder = evaluationCriterionDef.SortOrder,
                    MaxPointsAllowed = evaluationCriterionDef.MaxPointsAllowed
                };

                EvaluationCriterionDefs.Save(newEvaluationCriterionDef);
			}

			FillGrid();
		}
	}
}
