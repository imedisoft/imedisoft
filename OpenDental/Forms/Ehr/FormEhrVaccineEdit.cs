using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrVaccineEdit : FormBase
	{
		private readonly EhrVaccine vaccine;

		public FormEhrVaccineEdit(EhrVaccine vaccine)
		{
			InitializeComponent();

			this.vaccine = vaccine;
		}

		private void FormEhrVaccineEdit_Load(object sender, EventArgs e)
		{
			cvxTextBox.Text = vaccine.CvxCode;
			nameTextBox.Text = vaccine.Name;

			foreach (var drugManufacturer in EhrDrugManufacturers.GetAll())
            {
				manufacturerComboBox.Items.Add(drugManufacturer);
				if (drugManufacturer.Id == vaccine.EhrDrugManufacturerId)
                {
					manufacturerComboBox.SelectedItem = drugManufacturer;
				}
			}
		}

		private void CvxButton_Click(object sender, EventArgs e)
		{
            using var formCvxs = new FormCvxs
            {
                IsSelectionMode = true
            };

            if (formCvxs.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			cvxTextBox.Text = formCvxs.SelectedCvx.Code;
			nameTextBox.Text = formCvxs.SelectedCvx.Description;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var code = cvxTextBox.Text.Trim();
			if (code.Length == 0)
            {
				ShowError(Translation.Common.PleaseSelectCode);

				return;
            }

			var name = nameTextBox.Text.Trim();
			if (name.Length == 0)
            {
				ShowError(Translation.Common.PleaseEnterName);

				return;
            }

            if (!(manufacturerComboBox.SelectedItem is EhrDrugManufacturer manufacturer))
            {
                ShowError(Translation.Ehr.PleaseSelectManufacturer);

                return;
            }

            if (vaccine.Id == 0)
			{
				if (EhrVaccines.Any(x => x.CvxCode == cvxTextBox.Text && x.EhrDrugManufacturerId == vaccine.EhrDrugManufacturerId))
				{
					ShowError(Translation.Ehr.CvxCodeAlreadyExistsForTheSelectedManufacturer);

					return;
				}
			}

			vaccine.CvxCode = code;
			vaccine.Name = name;
			vaccine.EhrDrugManufacturerId = manufacturer.Id;

			EhrVaccines.Save(vaccine);

			DialogResult = DialogResult.OK;
		}
	}
}
