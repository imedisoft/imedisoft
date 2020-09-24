using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrCarePlanEdit : FormBase
	{
		private readonly EhrCarePlan ehrCarePlan;
		private Snomed snomed;

		public FormEhrCarePlanEdit(EhrCarePlan ehrCarePlan)
		{
			InitializeComponent();

			this.ehrCarePlan = ehrCarePlan;
		}

		private void FormEhrCarePlanEdit_Load(object sender, EventArgs e)
		{
			dateTextBox.Text = ehrCarePlan.DatePlanned?.ToShortDateString();

			snomed = null;
			if (!string.IsNullOrEmpty(ehrCarePlan.SnomedEducation))
			{
				snomed = Snomeds.GetByCode(ehrCarePlan.SnomedEducation);
				snomedTextBox.Text = snomed?.Description;
			}

			instructionsTextBox.Text = ehrCarePlan.Instructions;
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

			snomed = formSnomeds.SelectedSnomed;
			snomedTextBox.Text = snomed.Description;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!DateTime.TryParse(dateTextBox.Text, out var date))
            {
				ShowError(Translation.Common.PleaseEnterValidDate);

				return;
            }

			if (snomed == null)
			{
				ShowError(Translation.Ehr.PleaseSelectSnomedCtGoal);

				return;
			}

			ehrCarePlan.DatePlanned = date;
			ehrCarePlan.SnomedEducation = snomed.Code;
			ehrCarePlan.Instructions = instructionsTextBox.Text;

			EhrCarePlans.Save(ehrCarePlan);

			DialogResult = DialogResult.OK;
		}
	}
}
