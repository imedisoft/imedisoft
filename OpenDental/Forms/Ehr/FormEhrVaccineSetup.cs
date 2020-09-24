using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrVaccineSetup : FormBase
	{
		public FormEhrVaccineSetup()
		{
			InitializeComponent();
		}

		private void FormEhrVaccineSetup_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			EhrVaccines.RefreshCache();

			foreach (var vaccine in EhrVaccines.GetAll())
			{
				vaccinesListBox.Items.Add(vaccine);
			}
		}

		private void VaccinesListBox_DoubleClick(object sender, EventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void VaccinesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = vaccinesListBox.SelectedItem != null;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var vaccine = new EhrVaccine();

			using var formEhrVaccineEdit = new FormEhrVaccineEdit(vaccine);
			if (formEhrVaccineEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (!(vaccinesListBox.SelectedItem is EhrVaccine vaccine))
            {
                return;
            }

            using var formEhrVaccineEdit = new FormEhrVaccineEdit(vaccine);
			if (formEhrVaccineEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (!(vaccinesListBox.SelectedItem is EhrVaccine vaccine))
            {
                return;
            }

            if (!Confirm(Translation.Common.ConfirmDeleteSelectedItems))
            {
				return;
            }

			EhrVaccines.Delete(vaccine);

			FillGrid();
		}
    }
}
