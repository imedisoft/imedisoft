using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrCarePlans : FormBase
	{
		private readonly Patient patient;

		public FormEhrCarePlans(Patient patient)
		{
			InitializeComponent();

			this.patient = patient;
		}

		private void FormEhrCarePlans_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FormEhrCarePlans_Resize(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			int colDatePixCount = 66;
			int variablePixCount = carePlansGrid.Width - 10 - colDatePixCount;
			int colGoalPixCount = variablePixCount / 2;
			int colInstructionsPixCount = variablePixCount - colGoalPixCount;

			carePlansGrid.BeginUpdate();
			carePlansGrid.Columns.Clear();
			carePlansGrid.Columns.Add(new GridColumn(Translation.Common.Date, colDatePixCount));
			carePlansGrid.Columns.Add(new GridColumn(Translation.Common.Goal, colGoalPixCount));
			carePlansGrid.Columns.Add(new GridColumn(Translation.Common.Instructions, colInstructionsPixCount));
			carePlansGrid.EndUpdate();
			carePlansGrid.BeginUpdate();
			carePlansGrid.Rows.Clear();

			foreach (var carePlan in EhrCarePlans.Refresh(patient.PatNum))
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(carePlan.DatePlanned?.ToShortDateString());
				gridRow.Cells.Add(Snomeds.GetByCode(carePlan.SnomedEducation)?.Description);
				gridRow.Cells.Add(carePlan.Instructions);
				gridRow.Tag = carePlan;

				carePlansGrid.Rows.Add(gridRow);
			}

			carePlansGrid.EndUpdate();
		}

		private void CarePlansGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void CarePlansGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = carePlansGrid.SelectedRows.Count > 0;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
            var ehrCarePlan = new EhrCarePlan
            {
                PatientId = patient.PatNum,
                DatePlanned = DateTime.Today
            };

            using var formEhrCarePlanEdit = new FormEhrCarePlanEdit(ehrCarePlan);
			if (formEhrCarePlanEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

        private void EditButton_Click(object sender, EventArgs e)
        {
			var carePlan = carePlansGrid.SelectedTag<EhrCarePlan>();
			if (carePlan == null)
			{
				return;
			}

			using var formEhrCarePlanEdit = new FormEhrCarePlanEdit(carePlan);
			if (formEhrCarePlanEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

        private void DeleteButton_Click(object sender, EventArgs e)
        {
			var carePlans = carePlansGrid.SelectedTags<EhrCarePlan>();
			if (carePlans.Count == 0)
            {
				return;
            }

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItems))
            {
				return;
            }

			foreach (var carePlan in carePlans)
            {
				EhrCarePlans.Delete(carePlan);
            }

			FillGrid();
        }
    }
}
