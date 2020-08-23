using OpenDental;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormRxDefEdit : FormBase
	{
		private readonly RxDef rxDef;
		private readonly List<RxAlert> rxAlertsDeleted = new List<RxAlert>();

		public FormRxDefEdit(RxDef rxDef)
		{
			InitializeComponent();

			this.rxDef = rxDef;
		}

		private void FormRxDefEdit_Load(object sender, EventArgs e)
		{
			drugTextBox.Text = rxDef.Drug;
			sigTextBox.Text = rxDef.Sig;
			dispenseTextBox.Text = rxDef.Disp;
			refillsTextBox.Text = rxDef.Refills;
			notesTextBox.Text = rxDef.Notes;
			patInstructionsTextBox.Text = rxDef.PatientInstruction;
			controlledCheckBox.Checked = rxDef.IsControlled;

			if (Prefs.GetBool(PrefName.RxHasProc))
			{
				procRequiredCheckBox.Enabled = true;
				procRequiredCheckBox.Checked = rxDef.IsProcRequired;
			}

			FillAlerts();
			FillRxCui();
		}

		private void FillRxCui()
		{
			// Hide the RX CUI fields outside the US.
			if (!CultureInfo.CurrentCulture.Name.EndsWith("US"))
			{
				rxCuiLabel.Visible = false;
				rxCuiTextBox.Visible = false;
				rxCuiButton.Visible = false;

				return;
			}

			if (rxDef.RxCui == 0)
			{
				rxCuiTextBox.Text = "";
			}
			else
			{
				rxCuiTextBox.Text = rxDef.RxCui.ToString() + " - " + RxNorms.GetDescByRxCui(rxDef.RxCui.ToString());
			}
		}

		private void FillAlerts()
		{
			Medications.RefreshCache();

			alertsListBox.Items.Clear();
			foreach (var rxAlert in RxAlerts.Refresh(rxDef.Id))
            {
				alertsListBox.Items.Add(rxAlert);
            }
		}

		private void RxCuiButton_Click(object sender, EventArgs e)
		{
			using var formRxNorms = new FormRxNorms
			{
				IsSelectionMode = true,
				InitSearchCodeOrDescript = drugTextBox.Text
			};

			if (formRxNorms.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			long.TryParse(formRxNorms.SelectedRxNorm.RxCui, out var rxCui);

			rxDef.RxCui = rxCui;

			FillRxCui();
		}

		private void AlertsListBox_DoubleClick(object sender, EventArgs e)
		{
			if (alertsListBox.SelectedItem is RxAlert rxAlert)
            {
				using var formRxAlertEdit = new FormRxAlertEdit(rxAlert, rxDef);

				if (formRxAlertEdit.ShowDialog(this) == DialogResult.Cancel)
                {
					return;
                }

				alertsListBox.Invalidate(
					alertsListBox.GetItemRectangle(
						alertsListBox.SelectedIndex));
			}
            else
            {
				ShowError("Select at least one alert.");
			}
		}

		private void AlertsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			deleteButton.Enabled = alertsListBox.SelectedItem != null;
		}

		private void AddProblemButton_Click(object sender, EventArgs e)
		{
            using var formDiseaseDefs = new FormDiseaseDefs
            {
                IsSelectionMode = true,
                IsMultiSelect = true
            };

            if (formDiseaseDefs.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			foreach (var diseaseDef in formDiseaseDefs.ListSelectedDiseaseDefs)
			{
				alertsListBox.Items.Add(new RxAlert
				{
					DiseaseDefId = diseaseDef.DiseaseDefNum
				});
			}
		}

		private void AddMedicationButton_Click(object sender, EventArgs e)
		{
            using var formMedications = new FormMedications
            {
                IsSelectionMode = true
            };

            if (formMedications.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			var rxAlert = new RxAlert
			{
				MedicationId = formMedications.SelectedMedicationNum
			};

			alertsListBox.Items.Add(rxAlert);
			alertsListBox.SelectedItem = rxAlert;
		}

		private void AddAllergyButton_Click(object sender, EventArgs e)
		{
            using var formAllergySetup = new FormAllergySetup
            {
                IsSelectionMode = true
            };

            if (formAllergySetup.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			var rxAlert = new RxAlert
			{
				AllergyDefId = formAllergySetup.SelectedAllergyDefNum
			};

			alertsListBox.Items.Add(rxAlert);
			alertsListBox.SelectedItem = rxAlert;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (alertsListBox.SelectedItem is RxAlert rxAlert)
			{
				if (rxAlert.Id > 0)
                {
					rxAlertsDeleted.Add(rxAlert);
                }

				alertsListBox.Items.Remove(rxAlert);
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			// In the US if there is no RX CUI entered, prompt the user for confirmation...
			if (CultureInfo.CurrentCulture.Name.EndsWith("US") && rxDef.RxCui == 0)
			{
				if (Prompt(Translation.Rx.WarningRxNormNotPicked, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
				{
					return;
				}
			}

			rxDef.Drug = drugTextBox.Text;
			rxDef.Sig = sigTextBox.Text;
			rxDef.Disp = dispenseTextBox.Text;
			rxDef.Refills = refillsTextBox.Text;
			rxDef.Notes = notesTextBox.Text;
			rxDef.IsControlled = controlledCheckBox.Checked;
			rxDef.IsProcRequired = procRequiredCheckBox.Checked;
			rxDef.PatientInstruction = patInstructionsTextBox.Text;

			if (rxDef.Id == 0) rxDef.Id = RxDefs.Insert(rxDef);
            else
            {
				RxDefs.Update(rxDef);
            }

			foreach (var rxAlert in alertsListBox.Items.OfType<RxAlert>())
            {
				rxAlert.RxDefId = rxDef.Id;

				if (rxAlert.Id == 0) RxAlerts.Insert(rxAlert);
                else
                {
					RxAlerts.Update(rxAlert);
                }
            }

			foreach (var rxAlert in rxAlertsDeleted)
            {
				RxAlerts.Delete(rxAlert);
            }

			DialogResult = DialogResult.OK;
		}
    }
}
