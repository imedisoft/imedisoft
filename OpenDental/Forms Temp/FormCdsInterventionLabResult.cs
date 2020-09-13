using OpenDental;
using OpenDentBusiness;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormCdsInterventionLabResult : FormBase
	{
		///<summary>CDSI Trigger formatted text. This is the form result.</summary>
		public string ResultCDSITriggerText;

		public FormCdsInterventionLabResult()
		{
			InitializeComponent();
		}

		private void FormCdsInterventionLabResult_Load(object sender, EventArgs e)
		{
			FillComboBoxes();

			allResultsCheckBox.Checked = true;
		}

		private void FillComboBoxes()
		{
			comparatorComboBox.Items.Add("=");
			comparatorComboBox.Items.Add(">=");
			comparatorComboBox.Items.Add(">");
			comparatorComboBox.Items.Add("<");
			comparatorComboBox.Items.Add("<=");
			comparatorComboBox.SelectedIndex = 0;//not sure if this code works. Test it.

			var ucums = Ucums.GetAll().ToList();
			if (ucums.Count == 0)
			{
				ShowInfo("Units of measure have not been imported. Go to the code system importer window to import UCUM codes to continue.");

				DialogResult = DialogResult.Cancel;

				return;
			}

			foreach (var ucum in ucums)
            {
				unitsComboBox.Items.Add(ucum);
				if (ucum.Code == "mg/dL")
                {
					unitsComboBox.SelectedItem = ucum;
                }
            }
		}

		private void LoincButton_Click(object sender, EventArgs e)
		{
            using var formLoincs = new FormLoincs
            {
                IsSelectionMode = true
            };

			if (formLoincs.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			loincTextBox.Text = formLoincs.SelectedLoinc.Code;
			descriptionTextBox.Text = formLoincs.SelectedLoinc.LongCommonName;
			//if(FormL.SelectedLoinc.UnitsUCUM!="") {
			unitsComboBox.Text = formLoincs.SelectedLoinc.UnitsUCUM;//may be values that are not available otherwise. There are 270 units in the Loinc table that are not in the UCUM table.
															//}
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

			//Clear other options
			allResultsCheckBox.Checked = false;
			valueTextBox.Text = "";
			//Set Microbiology results
			snomedTextBox.Text = formSnomeds.SelectedSnomed.Code;
			snomedDescriptionTextBox.Text = formSnomeds.SelectedSnomed.Description;

		}

		private void ValueTextBox_TextChanged(object sender, EventArgs e)
		{
			if (valueTextBox.Text == "" && snomedTextBox.Text == "")
			{//cleared the text
				allResultsCheckBox.Checked = true;//user tried unchecking box but nothing else is selected.
				return;
			}

			allResultsCheckBox.Checked = false;
			snomedTextBox.Text = "";
			snomedDescriptionTextBox.Text = "";
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			ResultCDSITriggerText = loincTextBox.Text;/* +";";
			if(!checkAllResults.Checked && textObsValue.Text=="" && textSnomed.Text=="") {
				MessageBox.Show("Please select a valid lab result comparison.");
				return;//should never happen.  Somehow they have an invalid comparison set up.
			}
			else if(checkAllResults.Checked && textObsValue.Text=="" && textSnomed.Text=="") {
				ResultCDSITriggerText+=";";//loinc comparison only.
			}
			else if(!checkAllResults.Checked && textObsValue.Text!="" && textSnomed.Text=="") {
				ResultCDSITriggerText+=comboComparator.Text+textObsValue.Text+";"+comboUnits.Text;//Example:  >150;mg/dL
			}
			else if(!checkAllResults.Checked && textObsValue.Text=="" && textSnomed.Text!="") {
				ResultCDSITriggerText+=textSnomed.Text+";";//leave units blank to signify snomed.
			}
			else {
				MessageBox.Show("Please select a valid lab result comparison.");
				return;//should never happen.  Somehow they have an invalid comparison set up.
			}*/
			DialogResult = DialogResult.OK;
		}
	}
}
