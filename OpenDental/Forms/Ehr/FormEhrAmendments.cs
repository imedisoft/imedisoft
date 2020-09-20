using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrAmendments : FormBase
	{
		private readonly Patient patient;

		public FormEhrAmendments(Patient patient)
		{
			InitializeComponent();

			this.patient = patient;
		}

		private void FormEhrAmendments_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			static string GetStatus(bool? status)
			{
				return status switch
				{
					true => Translation.Common.Accepted,
					false => Translation.Common.Denied,
					null => Translation.Common.Requested
				};
			}

			ehrAmendmentsGrid.BeginUpdate();
			ehrAmendmentsGrid.Columns.Clear();
			ehrAmendmentsGrid.Columns.Add(new GridColumn(Translation.Common.EntryDate, 70) { TextAlign = HorizontalAlignment.Center });
			ehrAmendmentsGrid.Columns.Add(new GridColumn(Translation.Common.Status, 70));
			ehrAmendmentsGrid.Columns.Add(new GridColumn(Translation.Common.Source, 80));
			ehrAmendmentsGrid.Columns.Add(new GridColumn(Translation.Common.Description, 170));
			ehrAmendmentsGrid.Columns.Add(new GridColumn(Translation.Common.Scanned, 25) { TextAlign = HorizontalAlignment.Center });
			ehrAmendmentsGrid.Rows.Clear();

			foreach (var ehrAmendment in EhrAmendments.Refresh(patient.PatNum))
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(ehrAmendment.RequestedOn?.ToShortDateString());
				gridRow.Cells.Add(GetStatus(ehrAmendment.IsAccepted));
				gridRow.Cells.Add(EhrAmendments.GetSourceDescription(ehrAmendment.Source));
				gridRow.Cells.Add(ehrAmendment.Description);
				gridRow.Cells.Add(string.IsNullOrEmpty(ehrAmendment.FileName) ? "" : "X");
				gridRow.Tag = ehrAmendment;

				ehrAmendmentsGrid.Rows.Add(gridRow);
			}
			ehrAmendmentsGrid.EndUpdate();
		}

		private void EhrAmendmentsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void EhrAmendmentsGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = ehrAmendmentsGrid.SelectedRows.Count > 0;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
            var ehrAmendment = new EhrAmendment
            {
                PatientId = patient.PatNum
            };

            EhrAmendments.Insert(ehrAmendment);

			using var formEhrAmendmentEdit = new FormEhrAmendmentEdit(ehrAmendment);
			if (formEhrAmendmentEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var ehrAmendment = ehrAmendmentsGrid.SelectedTag<EhrAmendment>();
			if (ehrAmendment == null)
            {
				return;
            }

			using var formEhrAmendmentEdit = new FormEhrAmendmentEdit(ehrAmendment);
			if (formEhrAmendmentEdit.ShowDialog(this) != DialogResult.OK)
            {
				return;
            }

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var ehrAmendment = ehrAmendmentsGrid.SelectedTag<EhrAmendment>();
			if (ehrAmendment == null)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
            {
				return;
            }

			EhrAmendments.Delete(ehrAmendment);

			FillGrid();
		}
    }
}
