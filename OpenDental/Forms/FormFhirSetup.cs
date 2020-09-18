using Imedisoft.Bridges.Fhir;
using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormFhirSetup : FormBase
	{
		private Program program;

		public FormFhirSetup()
		{
			InitializeComponent();
		}

		private void FormFhirSetup_Load(object sender, EventArgs e)
		{
			program = Programs.GetCur(ProgramName.FHIR);

			enabledCheckBox.Checked = program.Enabled;

			intervalTextBox.Text = ProgramProperties.GetPropVal(program.Id, "SubscriptionProcessingFrequency");

			var paymentTypeId = Preferences.GetLong(PreferenceName.ApiPaymentType);

			foreach (var paymentType in Definitions.GetDefsForCategory(DefinitionCategory.PaymentTypes, true))
            {
				paymentTypeComboBox.Items.Add(paymentType);
				if (paymentTypeId == paymentType.Id)
                {
					paymentTypeComboBox.SelectedItem = paymentType;
                }
            }

			FillGrid();
		}

		private async void FillGrid()
		{
			refreshButton.Enabled = false;

			apiKeysGrid.BeginUpdate();
			apiKeysGrid.Columns.Clear();
			apiKeysGrid.Columns.Add(new GridColumn(Translation.Common.Developer, 140));
			apiKeysGrid.Columns.Add(new GridColumn(Translation.Common.Key, 250));
			apiKeysGrid.Columns.Add(new GridColumn(Translation.Common.Enabled, 40, HorizontalAlignment.Center));
			apiKeysGrid.Rows.Clear();
			apiKeysGrid.EndUpdate();

			List<FhirApiKey> apiKeys;

			Cursor = Cursors.WaitCursor;

			try
			{
				apiKeys = await FhirClient.GetApiKeysAsync();

				Cursor = Cursors.Default;
			}
			catch (Exception exception)
            {
				apiKeysGrid.EndUpdate();

				if (Visible)
				{
					Cursor = Cursors.Default;

					ShowError(exception.Message);
				}

				return;
            }
            finally
            {
				refreshButton.Enabled = true;
			}

			apiKeysGrid.BeginUpdate();

			foreach (var apiKey in apiKeys)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(apiKey.DeveloperName);
				gridRow.Cells.Add(apiKey.Key);
				gridRow.Cells.Add(apiKey.Enabled ? "X" : "");
				gridRow.Tag = apiKey;

				apiKeysGrid.Rows.Add(gridRow);
			}

			apiKeysGrid.EndUpdate();
		}

		private void ApiKeysGrid_CellClick(object sender, ODGridClickEventArgs e)
		{
			permissionsListBox.Items.Clear();

			var fhirApiKey = apiKeysGrid.SelectedTag<FhirApiKey>();
			if (fhirApiKey == null || fhirApiKey.Permissions == null)
			{
				return;
			}

			foreach (var permission in fhirApiKey.Permissions)
			{
				permissionsListBox.Items.Add(FhirClient.GetDescription(permission));
			}
		}

		private void ApiKeysGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var fhirApiKey = apiKeysGrid.SelectedTag<FhirApiKey>();
			if (fhirApiKey == null)
			{
				return;
			}

			using var formFhirApiKeyEdit = new FormFhirApiKeyEdit(fhirApiKey);
			if (formFhirApiKeyEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			using var formFhirAssignKey = new FormFhirApiKeyAssign();
			if (formFhirAssignKey.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void RefreshButton_Click(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			string freq = "";

			if (!string.IsNullOrEmpty(intervalTextBox.Text))
            {
				if (!int.TryParse(intervalTextBox.Text, out var result))
                {
					ShowError(Translation.Common.PleaseEnterValidInterval);

					return;
                }

				freq = result.ToString();
            }

            if (!(paymentTypeComboBox.SelectedItem is Definition paymentType))
            {
                ShowError(Translation.Common.PleaseSelectPaymentType);

                return;
            }

            program.Enabled = enabledCheckBox.Checked;

			Programs.Update(program);

			var programProperty = 
				ProgramProperties.GetPropByDesc("SubscriptionProcessingFrequency", 
					ProgramProperties.GetForProgram(program.Id));

			ProgramProperties.UpdateProgramPropertyWithValue(programProperty, freq);

			CacheManager.RefreshGlobal(nameof(InvalidType.Programs));

			if (Preferences.Set(PreferenceName.ApiPaymentType, paymentType.Id))
			{
				CacheManager.RefreshGlobal(nameof(InvalidType.Prefs));
			}

			DialogResult = DialogResult.OK;

			Close();
		}
    }
}
