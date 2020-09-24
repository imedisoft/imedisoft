using Imedisoft.Data;
using Imedisoft.Data.Models;
using Imedisoft.Data.Models.CodeLists.HL7;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrPatientVaccineEdit : FormBase
	{
		private readonly EhrPatientVaccine patientVaccine;
		private readonly List<EhrPatientVaccineObservation> patientVaccineObservations = new List<EhrPatientVaccineObservation>();
		private readonly List<EhrPatientVaccineObservation> patientVaccineObservationsDeleted = new List<EhrPatientVaccineObservation>();

		public FormEhrPatientVaccineEdit(EhrPatientVaccine patientVaccine)
		{
			InitializeComponent();

			this.patientVaccine = patientVaccine;
		}

		private void FormEhrPatientVaccineEdit_Load(object sender, EventArgs e)
		{
			var patient = Patients.GetLim(patientVaccine.PatientId);
			if (patient.Age > 0 && patient.Age < 3)
			{
				documentLabel.Text = 
					"Document reason not given below. " +
					"Reason can include a contraindication due to a specific allergy, adverse effect, intollerance, or specific disease.";
			}

			vaccineComboBox.Items.Clear();
			foreach (var vaccine in EhrVaccines.GetAll())
			{
				vaccineComboBox.Items.Add(vaccine);

				if (vaccine.Id == patientVaccine.VaccineId)
				{
					vaccineComboBox.SelectedItem = vaccine;
				}
			}

			if (patientVaccine.VaccineId != null)
			{
				EhrVaccine vaccineDef = EhrVaccines.GetById(patientVaccine.VaccineId.Value);
				EhrDrugManufacturer manufacturer = EhrDrugManufacturers.GetById(vaccineDef.EhrDrugManufacturerId);
				manufacturerTextBox.Text = manufacturer.Code + " - " + manufacturer.Name;
			}

			if (patientVaccine.DateStart.HasValue)
			{
				dateStartTextBox.Text = patientVaccine.DateStart.ToString();
			}

			if (patientVaccine.DateEnd.HasValue)
			{
				dateStopTextBox.Text = patientVaccine.DateEnd.ToString();
			}

			if (patientVaccine.AdministeredAmount != 0)
			{
				amountTextBox.Text = patientVaccine.AdministeredAmount.ToString();
			}

			unitsComboBox.Items.Add(Translation.Common.None);
			unitsComboBox.SelectedIndex = 0;

			foreach (var drugUnit in EhrDrugUnits.GetAll())
			{
				unitsComboBox.Items.Add(drugUnit.Code);

				if (drugUnit.Code == patientVaccine.DrugUnitCode)
				{
					unitsComboBox.SelectedItem = drugUnit;
				}
			}

			lotNumberTextBox.Text = patientVaccine.LotNumber;
			if (patientVaccine.ExpirationDate.HasValue)
			{
				expirationDateTextBox.Text = patientVaccine.ExpirationDate.Value.ToShortDateString();
			}

			foreach (var item in SubstanceRefusalReason.GetDataItems())
            {
				refusalReasonComboBox.Items.Add(item);
				if (item.Value == patientVaccine.RefusalReason)
                {
					refusalReasonComboBox.SelectedItem = item;
                }
			}

			foreach (var item in TreatmentCompletionStatus.GetDataItems())
            {
				completionStatusComboBox.Items.Add(item);
				if (item.Value == patientVaccine.CompletionStatus)
                {
					completionStatusComboBox.SelectedItem = item;
				}
			}

			noteTextBox.Text = patientVaccine.Note;
			if (patientVaccine.Id == 0)
			{
				patientVaccine.UserId = Security.CurrentUser.Id;

				if (patient.ClinicNum == 0)
				{
					patientVaccine.FilledCity = Preferences.GetString(PreferenceName.PracticeCity);
					patientVaccine.FilledState = Preferences.GetString(PreferenceName.PracticeST);
				}
				else
				{
					Clinic clinic = Clinics.GetById(patient.ClinicNum);
					patientVaccine.FilledCity = clinic.City;
					patientVaccine.FilledState = clinic.State;
				}
			}

			filledCityTextBox.Text = patientVaccine.FilledCity;
			filledStateTextBox.Text = patientVaccine.FilledState;

			userTextBox.Text = Users.GetById(patientVaccine.UserId)?.UserName;

			orderedByComboBox.Items.Add(Translation.Common.None);
			orderedByComboBox.SelectedIndex = 0;

			administeredByComboBox.Items.Add(Translation.Common.None);
			administeredByComboBox.SelectedIndex = 0;

			var providers = Providers.GetAll(true).ToList();

			if (patientVaccine.OrderedBy.HasValue)
			{
				var providerOrderedBy = Providers.GetById(patientVaccine.OrderedBy.Value);
				if (providerOrderedBy != null && !providers.Any(p => p.Id == patientVaccine.OrderedBy))
				{
					providers.Add(providerOrderedBy);
				}
			}

			if (patientVaccine.AdministeredBy.HasValue)
			{
				var providerAdministeredBy = Providers.GetById(patientVaccine.AdministeredBy.Value);
				if (providerAdministeredBy != null && !providers.Any(p => p.Id == patientVaccine.AdministeredBy))
				{
					providers.Add(providerAdministeredBy);
				}
			}

			providers.Sort((x, y) => x.SortOrder.CompareTo(y.SortOrder));

			foreach (var provider in providers)
            {
				orderedByComboBox.Items.Add(provider);
				if (provider.Id == patientVaccine.OrderedBy)
                {
					orderedByComboBox.SelectedItem = provider;
				}

				administeredByComboBox.Items.Add(provider);
				if (provider.Id == patientVaccine.AdministeredBy)
                {
					administeredByComboBox.SelectedItem = provider;
				}
			}

			foreach (var item in RouteOfAdministration.GetDataItems())
            {
				administrationRouteComboBox.Items.Add(item);
				if (item.Value == patientVaccine.AdministrationRoute)
                {
					administrationRouteComboBox.SelectedItem = item;
				}
			}

			foreach (var item in AdministrativeSite.GetDataItems())
			{
				administrationSiteComboBox.Items.Add(item);
				if (item.Value == patientVaccine.AdministrationSite)
                {
					administrationSiteComboBox.SelectedItem = item;
                }
			}

			foreach (var item in NIP001.GetDataItems())
            {
				informationSourceComboBox.Items.Add(item);
				if (item.Value == patientVaccine.InformationSource)
                {
					informationSourceComboBox.SelectedItem = item;
				}
			}

			foreach (var item in HL70323.GetDataItems())
            {
				actionComboBox.Items.Add(item);
				if (item.Value == patientVaccine.ActionCode)
                {
					actionComboBox.SelectedItem = item;
				}
            }

			patientVaccineObservations.AddRange(EhrPatientVaccineObservations.GetByPatientVaccine(patientVaccine.Id));

			FillObservations();
		}

		private void FillObservations()
		{
			observationsGrid.BeginUpdate();
			observationsGrid.Columns.Clear();
			observationsGrid.Columns.Add(new GridColumn(Translation.Common.Question, 150));
			observationsGrid.Columns.Add(new GridColumn(Translation.Common.Value, 80) { IsWidthDynamic = true });
			observationsGrid.EndUpdate();
			observationsGrid.BeginUpdate();
			observationsGrid.Rows.Clear();

			foreach (var patientVaccineObs in patientVaccineObservations)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(new GridCell(patientVaccineObs.IdentifyingCode.ToString()));
				gridRow.Cells.Add(new GridCell(patientVaccineObs.Value));
				gridRow.Tag = patientVaccineObs;

				observationsGrid.Rows.Add(gridRow);
			}

			observationsGrid.EndUpdate();
		}

		private void VaccineComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(vaccineComboBox.SelectedItem is EhrVaccine vaccine))
			{
				return;
			}

			var drugManufacturer = EhrDrugManufacturers.GetById(vaccine.EhrDrugManufacturerId);

			manufacturerTextBox.Text = drugManufacturer.ToString();
		}

		private void OrderedByPickButton_Click(object sender, EventArgs e)
		{
			var selectedProvider = orderedByComboBox.SelectedItem as Provider;

            using var formProviderPick = new FormProviderPick
            {
                SelectedProviderId = selectedProvider?.Id ?? 0
			};

            if (formProviderPick.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			foreach (var provider in orderedByComboBox.Items.OfType<Provider>())
            {
				if (provider.Id == formProviderPick.SelectedProviderId)
                {
					orderedByComboBox.SelectedItem = provider;

					break;
                }
            }
		}

		private void OrderedByClearButton_Click(object sender, EventArgs e)
		{
			orderedByComboBox.SelectedIndex = 0;
		}

		private void AdministeredByPickButton_Click(object sender, EventArgs e)
		{
			var selectedProvider = administeredByComboBox.SelectedItem as Provider;

			using var formProviderPick = new FormProviderPick
			{
				SelectedProviderId = selectedProvider?.Id ?? 0
			};

			if (formProviderPick.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			foreach (var provider in administeredByComboBox.Items.OfType<Provider>())
			{
				if (provider.Id == formProviderPick.SelectedProviderId)
				{
					administeredByComboBox.SelectedItem = provider;

					return;
				}
			}

			administeredByComboBox.SelectedIndex = 0;
		}

		private void AdministeredByClearButton_Click(object sender, EventArgs e)
		{
			administeredByComboBox.SelectedIndex = -1;
		}

		private void ObservationsGrid_CellClick(object sender, ODGridClickEventArgs e)
		{
			if (observationsGrid.SelectedIndices.Length > 1)
			{
				return;
			}

			var patientVaccineObs = observationsGrid.SelectedTag<EhrPatientVaccineObservation>();
			if (patientVaccineObs == null || patientVaccineObs.Group == null)
            {
				return;
            }

			for (int i = 0; i < observationsGrid.Rows.Count; i++)
			{
                if (!(observationsGrid.Rows[i].Tag is EhrPatientVaccineObservation x) || 
					x == patientVaccineObs || x.Group != patientVaccineObs.Group)
                {
                    continue;
                }

                observationsGrid.SetSelected(i, true);
			}
		}

		private void ObservationsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var patientVaccineObs = new EhrPatientVaccineObservation();

			using var formPatientVaccineObsEdit = new FormEhrPatientVaccineObservationEdit(patientVaccineObs);
			if (formPatientVaccineObsEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			patientVaccineObservations.Add(patientVaccineObs);

			FillObservations();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var patientVaccineObs = observationsGrid.SelectedTag<EhrPatientVaccineObservation>();
			if (patientVaccineObs == null)
            {
				return;
            }

			using var formPatientVaccineObsEdit = new FormEhrPatientVaccineObservationEdit(patientVaccineObs);
			if (formPatientVaccineObsEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillObservations();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var observations = observationsGrid.SelectedTags<EhrPatientVaccineObservation>();
			if (observations.Count == 0)
			{
				return;
			}

			var message = observations.Count > 1 ?
				Translation.Common.ConfirmDeleteSelectedItems :
				Translation.Common.ConfirmDeleteSelectedItem;

			if (!Confirm(message))
            {
				return;
            }

			foreach (var observation in observations)
            {
				if (observation.Id > 0)
                {
					patientVaccineObservationsDeleted.Add(observation);
				}

				patientVaccineObservations.Remove(observation);
            }

			FixGroupNumbering();

			FillObservations();
		}

		private void FixGroupNumbering()
        {
			var groups = patientVaccineObservations
				.Where(
					pvo => pvo.Group.HasValue)
				.Select(
					pvo => pvo.Group.Value)
				.Distinct();

			var groupCount = groups.Count();
			if (groupCount == 0)
            {
				return;
            }

			var groupNumbers = new List<long>();
			for (int i = 0; i < groupCount; i++)
            {
				groupNumbers.Add(i + 1);
            }

			var groupNumbersInUse = new List<long>(groups);
			var groupNumbersOverlapped = groupNumbers.Where(group => groupNumbersInUse.Contains(group)).ToList();

			groupNumbers.RemoveAll(group => groupNumbersOverlapped.Contains(group));
			groupNumbersInUse.RemoveAll(group => groupNumbersOverlapped.Contains(group));
			if (groupNumbers.Count != groupNumbersInUse.Count)
            {
				return;
            }

			for (int i = 0; i < groupNumbers.Count; i++)
            {
				foreach (var patientVaccineObs in patientVaccineObservations.Where(pvo => pvo.Group == groupNumbersInUse[i]))
                {
					patientVaccineObs.Group = groupNumbers[i];
                }
            }
		}

		private void GroupButton_Click(object sender, EventArgs e)
		{
			if (observationsGrid.SelectedIndices.Length < 2)
			{
				ShowError(Translation.Ehr.PleaseSelectTwoOrMoreObservations);

				return;
			}

			var group = patientVaccineObservations.Where(x => x.Group.HasValue).Select(x => x.Group.Value).Max() + 1;

			foreach (var patientVaccineObs in observationsGrid.SelectedTags<EhrPatientVaccineObservation>())
			{
				patientVaccineObs.Group = group;
			}
		}

		private void UngroupButton_Click(object sender, EventArgs e)
		{
			if (observationsGrid.SelectedIndices.Length < 1)
			{
				ShowError(Translation.Ehr.PleaseSelectAtLeastOneObservation);

				return;
			}

			foreach (var patientVaccineObs in observationsGrid.SelectedTags<EhrPatientVaccineObservation>())
            {
				patientVaccineObs.Group = null;
            }

			observationsGrid.SetSelected(false);

			FixGroupNumbering();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			static string GetCode(ComboBox comboBox) => (comboBox.SelectedItem as DataItem<string>)?.Value;

			var completionStatus = GetCode(completionStatusComboBox) ?? TreatmentCompletionStatus.Complete;

			var vaccine = vaccineComboBox.SelectedItem as EhrVaccine;
			if (vaccine == null)
			{
				if (completionStatus != TreatmentCompletionStatus.NotAdministered)
				{
					ShowError(Translation.Ehr.PleaseSelectVaccine);

					return;
				}
			}

			DateTime? expirationDate = null;
			if (!string.IsNullOrEmpty(expirationDateTextBox.Text))
            {
				if (!DateTime.TryParse(expirationDateTextBox.Text, out var result))
                {
					ShowError(Translation.Ehr.PleaseEnterValidExpirationDate);

					return;
                }

				expirationDate = result;
            }

			var note = noteTextBox.Text.Trim();
			if (completionStatus == TreatmentCompletionStatus.NotAdministered)
			{
				if (note.Length == 0)
				{
					ShowError(Translation.Ehr.PleaseEnterDocumentationInNote);

					return;
				}
			}

			if (!DateTime.TryParse(dateStartTextBox.Text, out var dateStart) || !DateTime.TryParse(dateStopTextBox.Text, out var dateStop))
            {
				ShowError(Translation.Common.PleaseEnterValidStartAndStopDates);

				return;
            }

			var administeredAmount = 0f;
			if (!string.IsNullOrEmpty(amountTextBox.Text))
            {
				if (float.TryParse(amountTextBox.Text, out var result))
                {
					ShowError(Translation.Common.PleaseEnterValidAmount);
					
					return;
                }

				administeredAmount = result;
			}

			patientVaccine.VaccineId = vaccine.Id;
			patientVaccine.DateStart = dateStart;
			patientVaccine.DateEnd = dateStop;
			patientVaccine.AdministeredAmount = administeredAmount;
			patientVaccine.DrugUnitCode = (unitsComboBox.SelectedItem as EhrDrugUnit)?.Code;
			patientVaccine.LotNumber = lotNumberTextBox.Text.Trim();
			patientVaccine.ExpirationDate = expirationDate;
			patientVaccine.Note = note;
			patientVaccine.FilledCity = filledCityTextBox.Text;
			patientVaccine.FilledState = filledStateTextBox.Text;
			patientVaccine.CompletionStatus = completionStatus;
			patientVaccine.OrderedBy = (orderedByComboBox.SelectedItem as Provider)?.Id;
			patientVaccine.AdministeredBy = (administeredByComboBox.SelectedItem as Provider)?.Id;
			patientVaccine.RefusalReason = GetCode(refusalReasonComboBox);
			patientVaccine.AdministrationRoute = GetCode(administrationRouteComboBox);
			patientVaccine.AdministrationSite = GetCode(administrationSiteComboBox);
			patientVaccine.InformationSource = GetCode(informationSourceComboBox) ?? NIP001.NewRecord;
			patientVaccine.ActionCode = (actionComboBox.SelectedItem as DataItem<char>)?.Value ?? HL70323.Add;

			EhrPatientVaccines.Save(patientVaccine);

			foreach (var observation in patientVaccineObservations)
            {
				observation.EhrPatientVaccineId = patientVaccine.Id;

				EhrPatientVaccineObservations.Save(observation);
			}

			foreach (var observation in patientVaccineObservationsDeleted)
			{
				EhrPatientVaccineObservations.Delete(observation);
			}

			DialogResult = DialogResult.OK;
		}
    }
}
