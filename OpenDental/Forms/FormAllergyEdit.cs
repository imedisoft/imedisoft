using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAllergyEdit : FormBase
	{
		private readonly Allergy allergy;
		private Snomed reactionSnomed;

		public FormAllergyEdit(Allergy allergy)
		{
			InitializeComponent();

			this.allergy = allergy;
		}

		private void FormAllergyEdit_Load(object sender, EventArgs e)
		{
			var allergyDefs = AllergyDefs.GetAll(false).ToList();
			if (allergyDefs.Count < 1)
			{
				ShowError(Translation.Common.NeedToSetupAtLeastOneAllergy);

				DialogResult = DialogResult.Cancel;

				return;
			}

			foreach (var allergyDef in allergyDefs)
            {
				allergyDefComboBox.Items.Add(allergyDef);
				if (allergyDef.Id == allergy.AllergyDefId)
                {
					allergyDefComboBox.SelectedItem = allergyDef;
                }
            }

			reactionSnomed = Snomeds.GetByCode(allergy.ReactionSnomedCode);
			if (reactionSnomed != null)
			{
				reactionSnomedCodeTextBox.Text = reactionSnomed.Description;
			}

			reactionTextBox.Text = allergy.Reaction;
			
			if (allergy.AdverseReactionDate.HasValue)
            {
				adverseReactionDateTextBox.Text = allergy.AdverseReactionDate.Value.ToShortDateString();
            }

			isActiveCheckBox.Checked = allergy.IsActive;
		}

		private void ReactionSnomedCodeSelectButton_Click(object sender, EventArgs e)
		{
            using var formSnomeds = new FormSnomeds
            {
                IsSelectionMode = true
            };

            if (formSnomeds.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			reactionSnomed = formSnomeds.SelectedSnomed;
			reactionSnomedCodeTextBox.Text = reactionSnomed.Description;
		}

		private void ReactionSnomedCodeNoneButton_Click(object sender, EventArgs e)
		{
			reactionSnomed = null;
			reactionSnomedCodeTextBox.Text = "";
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var allegyDef = allergyDefComboBox.SelectedItem as AllergyDef;
			if (allegyDef == null)
			{
				ShowError(Translation.Common.PleaseSelectAllergy);

				return;
			}

			DateTime? adverseReactionDate = null;
			if (!string.IsNullOrWhiteSpace(adverseReactionDateTextBox.Text))
			{
				if (!DateTime.TryParse(adverseReactionDateTextBox.Text, out var result))
				{
					ShowError(Translation.Common.PleaseEnterValidDate);

					return;
				}

				adverseReactionDate = result;

			}

			allergy.AllergyDefId = allegyDef.Id;
			allergy.Reaction = reactionTextBox.Text.Trim();
			allergy.ReactionSnomedCode = reactionSnomed?.Code;
			allergy.AdverseReactionDate = adverseReactionDate;
			allergy.IsActive = isActiveCheckBox.Checked;

			Allergies.Save(allergy);

			DialogResult = DialogResult.OK;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (allergy.Id == 0)
			{
				DialogResult = DialogResult.Cancel;
				return;
			}

			if (!Confirm("Delete?"))
			{
				return;
			}

			Allergies.Delete(allergy);

			DialogResult = DialogResult.OK;
		}
	}
}
