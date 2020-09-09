using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormGradingScaleItemEdit : FormBase
	{
		private readonly GradingScaleItem gradingScaleItem;

		public FormGradingScaleItemEdit(GradingScaleItem gradingScaleItem)
		{
			InitializeComponent();

			this.gradingScaleItem = gradingScaleItem;
		}

		private void FormGradingScaleItemEdit_Load(object sender, EventArgs e)
		{
			textTextBox.Text = gradingScaleItem.Text;
			valueTextBox.Text = gradingScaleItem.Value.ToString();
			descriptionTextBox.Text = gradingScaleItem.Description;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var value = valueTextBox.Text.Trim();
			if (value.Length == 0)
			{
				ShowError(Translation.DentalSchools.PleaseEnterGradeNumber);

				return;
			}

			if (!float.TryParse(value, out var gradeNumber))
			{
				ShowError(Translation.DentalSchools.PleaseEnterGradeNumberInValidFormat);

				return;
			}

			gradingScaleItem.Value = gradeNumber;
			gradingScaleItem.Text = textTextBox.Text.Trim();
			gradingScaleItem.Description = descriptionTextBox.Text.Trim();
			
			DialogResult = DialogResult.OK;
		}
	}
}
