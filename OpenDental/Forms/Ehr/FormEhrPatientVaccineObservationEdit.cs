using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using Imedisoft.Data.Models.CodeLists.HL7;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrPatientVaccineObservationEdit : FormBase
	{
		private readonly EhrPatientVaccineObservation patientVaccineObs;

		public FormEhrPatientVaccineObservationEdit(EhrPatientVaccineObservation patientVaccineObs)
		{
			InitializeComponent();

			this.patientVaccineObs = patientVaccineObs;
		}

		private void FormEhrPatientVaccineObservationEdit_Load(object sender, EventArgs e)
		{
			foreach (var item in NIP003.GetDataItems())
            {
				questionComboBox.Items.Add(item);
				if (item.Value == patientVaccineObs.IdentifyingCode)
                {
					questionComboBox.SelectedItem = item;
                }
            }

			foreach (var item in HL70125.GetDataItems())
            {
				valueTypeComboBox.Items.Add(item);
				if (item.Value == patientVaccineObs.ValueType)
                {
					valueTypeComboBox.SelectedItem = item;
                }
            }

			codeSystemComboBox.Text = patientVaccineObs.CodeSystem;

			valueTextBox.Text = patientVaccineObs.Value;
			valueUnitsComboBox.Items.Add(Translation.Common.None);
			valueUnitsComboBox.SelectedIndex = 0;

			foreach (var ucum in Ucums.GetAll())
            {
				valueUnitsComboBox.Items.Add(ucum);
				if (ucum.Code.Equals(patientVaccineObs.UcumCode))
                {
					valueUnitsComboBox.SelectedItem = ucum;
                }
            }

			dateObservedTextBox.Text = patientVaccineObs.Date?.ToShortDateString();
			methodCodeTextBox.Text = patientVaccineObs.MethodCode;

			ComboBox_SelectionChangeCommitted(this, EventArgs.Empty);
		}

		private void ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			var valueType = valueTypeComboBox.SelectedItem as DataItem<string>;
			var observationIdentifier = questionComboBox.SelectedItem as DataItem<string>;

			codeSystemLabel.Enabled = codeSystemComboBox.Enabled
				= valueType.Value == HL70125.Coded;

			valueUnitsLabel.Enabled = valueUnitsComboBox.Enabled
				= valueType.Value == HL70125.Numeric;

			methodCodeLabel.Enabled = methodCodeTextBox.Enabled
				= observationIdentifier.Value == NIP003.FundPgmEligCat;
		}

		private void DateObservedButton_Click(object sender, EventArgs e)
		{
			dateObservedTextBox.Text = DateTimeOD.Today.ToShortDateString();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
            if (!(questionComboBox.SelectedItem is DataItem<string> observationIdentifier))
            {
                ShowError(Translation.Ehr.PleaseSelectQuestionThisObservationAnswers);

                return;
            }

            var value = valueTextBox.Text.Trim();
			if (value.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterValue);

				return;
			}

			if (!(valueTypeComboBox.SelectedItem is DataItem<string> valueType))
            {
				ShowError(Translation.Common.PleaseSelectValueType);

                return;
            }

			var codeSystem = codeSystemComboBox.Text.Trim();

            switch (valueType.Value)
            {
				case HL70125.Text:
					break;

				case HL70125.Coded:
					if (string.IsNullOrEmpty(codeSystem))
                    {
						ShowError(Translation.Ehr.PleaseEnterCodeSystem);

						return;
                    }
					break;

				case HL70125.DateAndTime:
					if (!DateTime.TryParse(value, out var _))
					{
						ShowError(Translation.Ehr.SpecifiedValueIsNotValidDateTime);

						return;
					}
					break;

				case HL70125.Dated:
					if (!DateTime.TryParse(value, out var _))
                    {
						ShowError(Translation.Ehr.SpecifiedValueIsNotValidDate);

						return;
                    }
					break;

				case HL70125.Numeric:
					if (!double.TryParse(value, out var _))
                    {
						ShowError(Translation.Ehr.SpecifiedValueIsNotValidNumber);

						return;
                    }
					break;
			}

			var ucum = valueUnitsComboBox.SelectedItem as Ucum;
			if (valueType.Value == HL70125.Numeric && ucum == null)
			{
				ShowError(Translation.Ehr.PleaseSelectValueUnits);

				return;
			}

			DateTime? date = null;
			if (!string.IsNullOrEmpty(dateObservedTextBox.Text))
            {
				if (!DateTime.TryParse(dateObservedTextBox.Text, out var result))
                {
					ShowError(Translation.Common.PleaseEnterDate);

					return;
                }

				date = result;
            }

			var methodCode = methodCodeTextBox.Text.Trim();
			if (string.IsNullOrEmpty(methodCode) && observationIdentifier.Value == NIP003.FundPgmEligCat)
            {
				ShowError(Translation.Ehr.PleaseEnterMethodCode);

				return;
            }

			patientVaccineObs.IdentifyingCode = observationIdentifier.Value;
			patientVaccineObs.ValueType = valueType.Value;
			patientVaccineObs.CodeSystem = codeSystem;
			patientVaccineObs.Value = valueTextBox.Text;
			patientVaccineObs.UcumCode = ucum?.Code;
			patientVaccineObs.Date = date;
			patientVaccineObs.MethodCode = methodCode;

			DialogResult = DialogResult.OK;
		}
	}
}
