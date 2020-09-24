using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrDrugUnitSetup : FormBase
	{
		public FormEhrDrugUnitSetup()
		{
			InitializeComponent();
		}

		private void FormEhrDrugUnitSetup_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			EhrDrugUnits.RefreshCache();

			drugUnitsListBox.Items.Clear();

			foreach (var drugUnit in EhrDrugUnits.GetAll())
            {
				drugUnitsListBox.Items.Add(drugUnit);
            }
		}

		private void DrugUnitsListBox_DoubleClick(object sender, EventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var drugUnit = new EhrDrugUnit();

			using var formEhrDrugUnitEdit = new FormEhrDrugUnitEdit(drugUnit);
			if (formEhrDrugUnitEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (!(drugUnitsListBox.SelectedItem is EhrDrugUnit drugUnit))
            {
                return;
            }

            using var formEhrDrugUnitEdit = new FormEhrDrugUnitEdit(drugUnit);
			if (formEhrDrugUnitEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (!(drugUnitsListBox.SelectedItem is EhrDrugUnit drugUnit))
            {
                return;
            }

            if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
			{
				return;
			}

			try
			{
				EhrDrugUnits.Delete(drugUnit);
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
