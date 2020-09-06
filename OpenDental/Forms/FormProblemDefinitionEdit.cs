using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormProblemDefinitionEdit : FormBase
	{
		private readonly ProblemDefinition problemDefinition;
		private string codeIcd9 = null;
		private string codeIcd10 = null;
		private string codeSnomed = null;

		public FormProblemDefinitionEdit(ProblemDefinition problemDefinition)
		{
			InitializeComponent();

			this.problemDefinition = problemDefinition;
		}

		private void FormProblemDefinitionEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = problemDefinition.Description;

			codeIcd9 = problemDefinition.CodeIcd9;
			codeIcd10 = problemDefinition.CodeIcd10;
			codeSnomed = problemDefinition.CodeSnomed;

			var description = Icd9s.GetCodeAndDescription(codeIcd9);
			if (string.IsNullOrEmpty(description))
			{
				icd9TextBox.Text = codeIcd9;
			}
			else
			{
				icd9TextBox.Text = description;
			}

			var icd10 = Icd10s.GetByCode(codeIcd10);
			if (icd10 == null)
			{
				icd10TextBox.Text = codeIcd10;
			}
			else
			{
				icd10TextBox.Text = icd10.Code + "-" + icd10.Description;
			}

			description = Snomeds.GetCodeAndDescription(codeSnomed);
			if (string.IsNullOrEmpty(description))
			{
				snomedTextBox.Text = codeSnomed;
			}
			else
			{
				snomedTextBox.Text = description;
			}

			hiddenCheckBox.Checked = problemDefinition.IsHidden;
		}

		private void Icd9Button_Click(object sender, EventArgs e)
		{
			using var formIcd9s = new FormIcd9s
			{
				IsSelectionMode = true
			};

			if (formIcd9s.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			if (ProblemDefinitions.ContainsIcd9(formIcd9s.SelectedIcd9.Code, problemDefinition.Id))
			{
				ShowError("ICD-9 code already exists in the problems list.");

				return;
			}

			codeIcd9 = formIcd9s.SelectedIcd9.Code;

			var description = Icd9s.GetCodeAndDescription(codeIcd9);
			if (string.IsNullOrEmpty(description))
			{
				icd9TextBox.Text = codeIcd9;
			}
			else
			{
				icd9TextBox.Text = description;
			}
		}

		private void Icd10Button_Click(object sender, EventArgs e)
		{
			using var formIcd10s = new FormIcd10s
			{
				IsSelectionMode = true
			};

			if (formIcd10s.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			if (ProblemDefinitions.ContainsIcd10(formIcd10s.SelectedIcd10.Code, problemDefinition.Id))
			{
				ShowError("ICD-10 code already exists in the problems list.");

				return;
			}

			codeIcd10 = formIcd10s.SelectedIcd10.Code;

			icd10TextBox.Text = formIcd10s.SelectedIcd10.Code + "-" + formIcd10s.SelectedIcd10.Description;
		}

		private void SnomedButton_Click(object sender, EventArgs e)
		{
            using var formSnomeds = new FormSnomeds
            {
                IsSelectionMode = true
            };

            if (formSnomeds.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			if (ProblemDefinitions.ContainsSnomed(formSnomeds.SelectedSnomed.Code, problemDefinition.Id))
			{
				ShowError("Snomed code already exists in the problems list.");

				return;
			}

			codeSnomed = formSnomeds.SelectedSnomed.Code;

			var description = Snomeds.GetCodeAndDescription(codeSnomed);
			if (string.IsNullOrEmpty(description))
			{
				snomedTextBox.Text = codeSnomed;
			}
			else
			{
				snomedTextBox.Text = description;
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

			problemDefinition.Description = descriptionTextBox.Text;
			problemDefinition.CodeIcd9 = codeIcd9;
			problemDefinition.CodeIcd10 = codeIcd10;
			problemDefinition.CodeSnomed = codeSnomed;
			problemDefinition.IsHidden = hiddenCheckBox.Checked;

			DialogResult = DialogResult.OK;
		}
	}
}
