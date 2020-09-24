using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrDrugManufacturerSetup : FormBase
	{
		public FormEhrDrugManufacturerSetup()
		{
			InitializeComponent();
		}

		private void FormEhrDrugManufacturerSetup_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			EhrDrugManufacturers.RefreshCache();

			drugManufacturersListBox.Items.Clear();

			foreach (var drugManufacturer in EhrDrugManufacturers.GetAll())
			{
				drugManufacturersListBox.Items.Add(drugManufacturer);
			}
		}

		private void DrugManufacturersListBox_DoubleClick(object sender, EventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var drugManufacturer = new EhrDrugManufacturer();

			using var formEhrDrugManufacturerEdit = new FormEhrDrugManufacturerEdit(drugManufacturer);
			if (formEhrDrugManufacturerEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (!(drugManufacturersListBox.SelectedItem is EhrDrugManufacturer drugManufacturer))
            {
                return;
            }

            using var formEhrDrugManufacturerEdit = new FormEhrDrugManufacturerEdit(drugManufacturer);
			if (formEhrDrugManufacturerEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (!(drugManufacturersListBox.SelectedItem is EhrDrugManufacturer drugManufacturer))
            {
                return;
            }

            if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
			{
				return;
			}

			try
			{
				EhrDrugManufacturers.Delete(drugManufacturer);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			FillGrid();
		}
    }
}
