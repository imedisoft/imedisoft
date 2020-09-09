using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEvaluationEdit : FormBase
	{
		private readonly Evaluation evaluation;
		private readonly List<EvaluationCriterion> evaluationCriteria = new List<EvaluationCriterion>();
		private Dictionary<long, GradingScale> gradingScales;
		private GradingScale gradingScale;
		private List<GradingScaleItem> gradingScaleItems;
		private Dictionary<long, List<GradingScaleItem>> gradingScaleItemsDict;
		private SchoolCourse schoolCourse;
		private Provider student;
		private Provider instructor;
		private long selectedGradingScaleId;
		private float overallGradeValue;

		public FormEvaluationEdit(Evaluation evaluation)
		{
			InitializeComponent();

			this.evaluation = evaluation;
		}

		private void FormEvaluationEdit_Load(object sender, EventArgs e)
		{
			dateTextBox.Text = evaluation.EvaluationDate.ToShortDateString();
			titleTextBox.Text = evaluation.Title;

			gradingScales = GradingScales.GetAll().ToDictionary(gs => gs.Id);
			gradingScales.TryGetValue(evaluation.GradingScaleId, out gradingScale);
			gradingScaleTextBox.Text = gradingScale?.Description;
			gradingScaleItems = GradingScaleItems.GetByGradingScale(evaluation.GradingScaleId).ToList();
			gradingScaleItemsDict = gradingScaleItems.GroupBy(gsi => gsi.GradingScaleId).ToDictionary(g => g.Key, g => g.ToList());

			schoolCourse = SchoolCourses.GetById(evaluation.SchoolCourseId);
			schoolCourseTextBox.Text = SchoolCourses.GetDescription(schoolCourse);

			student = Providers.GetById(evaluation.StudentId);
			if (student != null)
			{
				studentTextBox.Text = student.GetLongDesc();
			}

			instructor = Providers.GetById(evaluation.InstructorId);
			instructorTextBox.Text = instructor.GetLongDesc();

			gradeNumberOverrideTextBox.Text = evaluation.OverallGradeNumber.ToString();
			gradeShowingOverrideTextBox.Text = evaluation.OverallGradeShowing;

			evaluationCriteria.AddRange(EvaluationCriterions.GetByEvaluation(evaluation.Id));

			FillCriterions();

			CalculateGrades();

			if (gradeNumberTextBox.Text == gradeNumberOverrideTextBox.Text)
			{
				gradeNumberOverrideTextBox.Text = "";
			}

			if (gradeShowingTextBox.Text == gradeShowingOverrideTextBox.Text)
			{
				gradeShowingOverrideTextBox.Text = "";
			}
		}

		private void FillCriterions()
		{
			evaluationCriteriaGrid.BeginUpdate();
			evaluationCriteriaGrid.ListGridColumns.Clear();
			evaluationCriteriaGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Description, 150));
			evaluationCriteriaGrid.ListGridColumns.Add(new GridColumn(Translation.DentalSchools.GradingScale, 90));
			evaluationCriteriaGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Showing, 60));
			evaluationCriteriaGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Number, 60) { IsEditable = true });
			evaluationCriteriaGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Note, 120) { IsEditable = true });
			evaluationCriteriaGrid.ListGridRows.Clear();
			
			foreach (var evaluationCriterion in evaluationCriteria)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(evaluationCriterion.Description);
				gridRow.Tag = evaluationCriterion;

				if (evaluationCriterion.IsCategory)
				{
					gridRow.Bold = true;
					gridRow.Cells.Add("");
					gridRow.Cells.Add("");
					gridRow.Cells.Add("");
					gridRow.Cells.Add("");
				}
				else
				{
					gridRow.Cells.Add(gradingScales[evaluationCriterion.GradingScaleId].Description);
					gridRow.Cells.Add(evaluationCriterion.GradeShowing);
					gridRow.Cells.Add(evaluationCriterion.GradeNumber.ToString());
					gridRow.Cells.Add(evaluationCriterion.Notes);
				}
				evaluationCriteriaGrid.ListGridRows.Add(gridRow);
			}

			evaluationCriteriaGrid.EndUpdate();
		}

		private void FillGradingScales(EvaluationCriterion evaluationCriterion)
		{
			gradingScalesGrid.BeginUpdate();
			gradingScalesGrid.ListGridColumns.Clear();
			gradingScalesGrid.ListGridRows.Clear();

			if (evaluationCriterion != null && gradingScales.TryGetValue(evaluationCriterion.GradingScaleId, out var gradingScale))
			{
				switch (gradingScale.Type)
                {
					case GradingScaleType.Weighted:
						gradingScalesGrid.ListGridColumns.Add(new GridColumn(Translation.DentalSchools.MaxPoints, 100, HorizontalAlignment.Center));
						break;

					case GradingScaleType.Percentage:
						gradingScalesGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Percentage, 100, HorizontalAlignment.Center));
						break;

					case GradingScaleType.PickList:
						gradingScalesGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Number, 60));
						gradingScalesGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Showing, 60));
						gradingScalesGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Description, 150));
						break;
                }

				if (!evaluationCriterion.IsCategory)
				{
					switch (gradingScale.Type)
					{
						case GradingScaleType.Weighted:
							gradingScalesGrid.ListGridRows.Add(
								new GridRow(evaluationCriterion.MaxPointsAllowed.ToString()));
							break;

						case GradingScaleType.Percentage:
							gradingScalesGrid.ListGridRows.Add(
								new GridRow(Translation.DentalSchools.ZeroToOneHundred));
							break;

						case GradingScaleType.PickList:
							if (gradingScaleItemsDict.TryGetValue(gradingScale.Id, out var gradingScaleItems))
							{
								foreach (var gradingScaleItem in gradingScaleItems)
								{
									var gridRow = new GridRow();
									gridRow.Cells.Add(gradingScaleItem.Value.ToString());
									gridRow.Cells.Add(gradingScaleItem.Text);
									gridRow.Cells.Add(gradingScaleItem.Description);
									gridRow.Tag = gradingScaleItem;

									gradingScalesGrid.ListGridRows.Add(gridRow);
								}
							}
							break;
					}
				}
			}

			gradingScalesGrid.EndUpdate();
		}

		private void StudentButton_Click(object sender, EventArgs e)
		{
			using var formProviderPick = new FormProviderPick
			{
				IsStudentPicker = true
			};

			if (formProviderPick.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			student = Providers.GetById(formProviderPick.SelectedProviderId);
			studentTextBox.Text = student?.GetLongDesc() ?? "";
		}

		

		private void EvaluationCriteriaGrid_CellEnter(object sender, ODGridClickEventArgs e)
		{
			var evaluationCriterion = evaluationCriteriaGrid.SelectedTag<EvaluationCriterion>();
			if (evaluationCriterion == null)
            {
				return;
            }

			if (selectedGradingScaleId != evaluationCriterion.GradingScaleId)
			{
				FillGradingScales(evaluationCriterion);

				selectedGradingScaleId = evaluationCriterion.GradingScaleId;
			}

			gradingScalesGrid.SetSelected(false);
		}

		private void EvaluationCriteriaGrid_CellClick(object sender, ODGridClickEventArgs e)
		{
			var evaluationCriterion = evaluationCriteriaGrid.SelectedTag<EvaluationCriterion>();
			if (evaluationCriterion == null)
			{
				return;
			}

			if (selectedGradingScaleId != evaluationCriterion.GradingScaleId)
			{
				FillGradingScales(evaluationCriterion);

				selectedGradingScaleId = evaluationCriterion.GradingScaleId;
			}

			gradingScalesGrid.SetSelected(false);
		}

		private void EvaluationCriteriaGrid_CellLeave(object sender, ODGridClickEventArgs e)
		{
			if (e.Row == -1 || e.Col == -1) return;

            if (!(evaluationCriteriaGrid.ListGridRows[e.Row].Tag is EvaluationCriterion evaluationCriterion))
            {
                return;
            }

			if (e.Col == 3) // Column 3 = Grade Number
            {
				if (!gradingScales.TryGetValue(evaluationCriterion.GradingScaleId, out var gradingScale))
				{
					return;
				}

				if (!float.TryParse(evaluationCriteriaGrid.ListGridRows[e.Row].Cells[3].Text, out float gradeNumber))
                {
                    evaluationCriteriaGrid.ListGridRows[e.Row].Cells[3].Text = evaluationCriterion.GradeNumber.ToString();

                    return;
                }

                switch (gradingScale.Type)
                {
					case GradingScaleType.PickList:
                        {
							bool valid = false;

							foreach (var gradingScaleItem in gradingScaleItems.Where(gsi => gsi.GradingScaleId == gradingScale.Id))
							{
								if (gradeNumber == gradingScaleItem.Value)
								{
									evaluationCriterion.GradeNumber = gradeNumber;
									evaluationCriterion.GradeShowing = gradingScaleItem.Text;

									valid = true;

									evaluationCriteriaGrid.ListGridRows[e.Row].Cells[2].Text = gradingScaleItem.Text;

									break;
								}
							}

							if (!valid)
							{
								evaluationCriteriaGrid.ListGridRows[e.Row].Cells[3].Text = evaluationCriterion.GradeNumber.ToString();
							}
						}
						break;

					default:
						evaluationCriterion.GradeNumber = gradeNumber;
						evaluationCriterion.GradeShowing = gradeNumber.ToString();
						evaluationCriteriaGrid.ListGridRows[e.Row].Cells[2].Text = gradeNumber.ToString();
						break;
                }

				evaluationCriteriaGrid.Invalidate();

				CalculateGrades();
			}
		}

		private void GradingScalesGrid_CellClick(object sender, ODGridClickEventArgs e)
		{
			var evaluationCriterion = evaluationCriteriaGrid.SelectedTag<EvaluationCriterion>();
			if (evaluationCriterion == null)
            {
				return;
            }

			var gradingScaleItem = gradingScalesGrid.SelectedTag<GradingScaleItem>();
			if (gradingScaleItem == null)
            {
				return;
            }

			evaluationCriterion.GradeNumber = gradingScaleItem.Value;
			evaluationCriterion.GradeShowing = gradingScaleItem.Text;
			
			var evalationCriterionCell = evaluationCriteriaGrid.SelectedCell;

			FillCriterions();

			evaluationCriteriaGrid.SetSelected(evalationCriterionCell);

			CalculateGrades();
		}

		private void CalculateGrades()
		{
			var grades = 0;
			var gradePoints = 0f;
			var gradePointsMax = 0f;
			var gradePointsForDisplay = 0f;

			var evaluationCriteria = evaluationCriteriaGrid.GetTags<EvaluationCriterion>();
			foreach (var evaluationCriterion in evaluationCriteria)
            {
				if (gradingScale.Id == evaluationCriterion.GradingScaleId)
                {
					grades++;
					gradePoints += evaluationCriterion.GradeNumber;
					gradePointsMax += evaluationCriterion.MaxPointsAllowed;
                }
            }

			if (gradingScale.Type == GradingScaleType.PickList || gradingScale.Type == GradingScaleType.Percentage)
			{
				if (grades > 0)
				{
					gradePointsForDisplay = gradePoints / grades;
				}
			}
			else if (gradingScale.Type == GradingScaleType.Weighted)
			{
				gradePointsForDisplay = gradePoints;
			}

			switch (gradingScale.Type)
            {
				case GradingScaleType.Percentage:
					gradeNumberTextBox.Text = gradeShowingTextBox.Text = gradePointsForDisplay.ToString();

					overallGradeValue = gradePointsForDisplay;
					break;

				case GradingScaleType.PickList:
                    {
						var diff = float.MaxValue;

						var gradeValue = 0f;
						var gradeText = "";

						foreach (var gradingScaleItem in gradingScaleItems.Where(gs => gs.GradingScaleId == gradingScale.Id))
						{
							var dist = Math.Abs(gradingScaleItem.Value - gradePointsForDisplay);
							if (dist < diff)
							{
								diff = dist;

								gradeValue = gradingScaleItem.Value;
								gradeText = gradingScaleItem.Text;
							}
						}

						// TODO: What should we do if there are 2 scale items with the same minimum 
						//       distance to the grade? Should we pick the one with the highest or lowest value? As is, 
						//       we pick the first match.

						gradeNumberTextBox.Text = gradeValue.ToString();
						gradeShowingTextBox.Text = gradeText;

						overallGradeValue = gradeValue;
					}
					break;

				case GradingScaleType.Weighted:
					gradeNumberTextBox.Text = gradePointsForDisplay.ToString();
					gradeShowingTextBox.Text = gradePointsForDisplay + "/" + gradePointsMax;

					overallGradeValue = gradePointsForDisplay;
					break;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (dateTextBox.Text.Trim() == "")
            {
				ShowError(Translation.Common.PleaseEnterDate);

				return;
            }

			if (!DateTime.TryParse(dateTextBox.Text, out var date))
            {
				ShowError(Translation.Common.PleaseEnterValidDate);

				return;
            }

			if (student == null)
			{
				ShowError(Translation.DentalSchools.PleaseAttachStudentToThisEvaluation);

				return;
			}

			var gradeNumber = overallGradeValue;
			if (!string.IsNullOrEmpty(gradeNumberOverrideTextBox.Text))
            {
				if (!float.TryParse(gradeNumberOverrideTextBox.Text, out gradeNumber))
                {
					ShowError(Translation.DentalSchools.OverrideForOverallGradeNumberIsNotValid);

					return;
                }
            }

			evaluation.EvaluationDate = date;
			evaluation.StudentId = student.Id;
			evaluation.OverallGradeShowing = gradeShowingTextBox.Text;
			evaluation.OverallGradeNumber = gradeNumber;

			if (!string.IsNullOrWhiteSpace(gradeShowingOverrideTextBox.Text))
			{
				evaluation.OverallGradeShowing = gradeShowingOverrideTextBox.Text;
			}

			Evaluations.Save(evaluation);

			foreach (var gridRow in evaluationCriteriaGrid.ListGridRows)
			{
				if (gridRow.Tag is EvaluationCriterion evaluationCriterion)
				{
					var notes = gridRow.Cells[4].Text;
					if (evaluationCriterion.EvaluationId > 0 && evaluationCriterion.Notes.Equals(notes))
                    {
						continue;
                    }

					evaluationCriterion.EvaluationId = evaluation.Id;
					evaluationCriterion.Notes = notes;

					EvaluationCriterions.Save(evaluationCriterion);
				}
			}

			DialogResult = DialogResult.OK;
		}
	}
}
