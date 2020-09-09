using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEvaluationCriterionDefEdit : FormBase
	{
		private readonly EvaluationCriterionDef evaluationCriterionDef;
		private GradingScale gradingScale;
		private float maxPointsAllowed;

		public FormEvaluationCriterionDefEdit(EvaluationCriterionDef evaluationCriterionDef)
		{
			InitializeComponent();

			this.evaluationCriterionDef = evaluationCriterionDef;
		}

		private void FormEvaluationCriterionDefEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = evaluationCriterionDef.Description;

			gradingScale = GradingScales.GetById(evaluationCriterionDef.GradingScaleId);
			gradingScaleTextBox.Text = gradingScale.Description;
			isCategoryNameCheckBox.Checked = evaluationCriterionDef.IsCategory;

			maxPointsAllowed = evaluationCriterionDef.MaxPointsAllowed;
			if (gradingScale.Type == GradingScaleType.Weighted)
			{
				pointsTextBox.ReadOnly = false;
				pointsTextBox.Text = maxPointsAllowed.ToString();
			}
		}

		private void GradingScaleButton_Click(object sender, EventArgs e)
		{
            using var formGradingScales = new FormGradingScales
            {
                IsSelectionMode = true
            };

            if (formGradingScales.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			gradingScale = formGradingScales.SelectedGradingScale;
			gradingScaleTextBox.Text = formGradingScales.SelectedGradingScale.Description;

			maxPointsAllowed = evaluationCriterionDef.MaxPointsAllowed;
			if (formGradingScales.SelectedGradingScale.Type == GradingScaleType.Weighted)
			{
				pointsTextBox.ReadOnly = false;
				pointsTextBox.Text = maxPointsAllowed.ToString();
			}
			else
			{
				pointsTextBox.ReadOnly = true;
				pointsTextBox.Text = "";
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterDescription);

				return;
			}

            if (gradingScale.Type == GradingScaleType.Weighted)
            {
                if (!float.TryParse(pointsTextBox.Text, out float result))
                {
                    ShowError("The specified point value is not a valid number. Please input a valid number to save the criterion.");

                    return;
                }

                maxPointsAllowed = result;
            }

            evaluationCriterionDef.Description = description;
			evaluationCriterionDef.GradingScaleId = gradingScale.Id;
			evaluationCriterionDef.IsCategory = isCategoryNameCheckBox.Checked;
			evaluationCriterionDef.MaxPointsAllowed = maxPointsAllowed;

			DialogResult = DialogResult.OK;
		}
	}
}
