using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using System;
using System.Text;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAllergyDefEdit : FormBase
	{
		private readonly AllergyDef allergyDef;
		private long? medicationId;

		public FormAllergyDefEdit(AllergyDef allergyDef)
		{
			InitializeComponent();

			this.allergyDef = allergyDef;
		}

		private void FormAllergyEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = allergyDef.Description;

			snomedCodeComboBox.Items.Add(Translation.Common.None);
			snomedCodeComboBox.SelectedIndex = 0;

			foreach (var dataItem in SnomedAllergyCode.EnumerateDataItems())
			{
				snomedCodeComboBox.Items.Add(dataItem);
				if (dataItem.Value == allergyDef.SnomedCode)
				{
					snomedCodeComboBox.SelectedItem = dataItem;
				}
			}

			uniiTextBox.Text = allergyDef.UniiCode;

			medicationId = allergyDef.MedicationId;
			if (medicationId.HasValue)
			{
				medicationTextBox.Text = Medications.GetDescription(medicationId.Value);
			}

			isHiddenCheckBox.Checked = allergyDef.IsHidden;
		}

		private void UniiSelectButton_Click(object sender, EventArgs e)
		{
			// TODO: Implement this for UNII.

			throw new NotImplementedException();
		}

		private void UniiNoneButton_Click(object sender, EventArgs e)
		{
			// TODO: Implement this for UNII.

			throw new NotImplementedException();
		}

		private void MedicationSelectButton_Click(object sender, EventArgs e)
		{
            using var formMedications = new FormMedications
            {
                IsSelectionMode = true
            };

            if (formMedications.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			medicationId = formMedications.SelectedMedicationNum;
			if (medicationId.HasValue)
            {
				medicationTextBox.Text = Medications.GetDescription(medicationId.Value);
            }
            else
            {
				medicationTextBox.Text = "";
            }
		}

		private void MedicationNoneButton_Click(object sender, EventArgs e)
		{
			medicationId = null;
			medicationTextBox.Text = "";
		}

		private bool ValidateUNII(string uniiCode)
        {
			const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

			if (string.IsNullOrEmpty(uniiCode) || uniiCode.Length != 10)
			{
				ShowError("UNII code must be 10 characters in length.");

				return false;
			}

			var invalid = new StringBuilder();

			foreach (var c in uniiCode)
            {
				if (valid.IndexOf(c) == -1)
                {
					invalid.Append(c);
				}
            }

			if (invalid.Length > 0)
            {
				ShowError("UNII code has invalid characters: " + invalid.ToString());

				return false;
			}

			return true;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterDescription);

				return;
			}

			var uniiCode = uniiTextBox.Text.Trim();
			if (!string.IsNullOrEmpty(uniiCode) && medicationId.HasValue)
			{
				ShowError("Only one allergen code is allowed per allergy definition.");

				return;
			}

			string snomedCode = null;
			if (snomedCodeComboBox.SelectedItem is DataItem<string> snomedDataItem)
			{
				snomedCode = snomedDataItem.Value;
			}

			if (!ValidateUNII(uniiCode))
			{
				return;
			}

			allergyDef.Description = description;
			allergyDef.SnomedCode = snomedCode;
			allergyDef.MedicationId = medicationId;
			allergyDef.UniiCode = uniiTextBox.Text;
			allergyDef.IsHidden = isHiddenCheckBox.Checked;

			AllergyDefs.Save(allergyDef);

			DialogResult = DialogResult.OK;
		}
	}
}
