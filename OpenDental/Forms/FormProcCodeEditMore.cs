using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormProcCodeEditMore : FormBase
	{
		private readonly ProcedureCode procedureCode;
		private bool hasChanges = false;

		public FormProcCodeEditMore(ProcedureCode procedureCode)
		{
			InitializeComponent();

			this.procedureCode = procedureCode;
		}

		private void FormProcCodeEditMore_Load(object sender, EventArgs e) 
			=> FillGrid();

		private void FillGrid()
		{
			var clinicIds = Clinics.GetByCurrentUser().Select(x => x.Id).ToList();
			var fees = Fees.GetFeesForCode(procedureCode.Id, clinicIds);

			feesGrid.BeginUpdate();
			feesGrid.Columns.Clear();
			feesGrid.Columns.Add(new GridColumn("Schedule", 130));
			feesGrid.Columns.Add(new GridColumn("Clinic", 130));
			feesGrid.Columns.Add(new GridColumn("Provider", 75));
			feesGrid.Columns.Add(new GridColumn("Amount", 100, HorizontalAlignment.Right));
			feesGrid.Rows.Clear();

			long lastFeeScheduleId = 0;
			foreach (var fee in fees)
			{
				var gridRow = new GridRow();
				if (fee.FeeScheduleId != lastFeeScheduleId)
				{
					lastFeeScheduleId = fee.FeeScheduleId;

					gridRow.Cells.Add(FeeScheds.GetDescription(fee.FeeScheduleId));
					gridRow.Bold = true;
					gridRow.BackColor = Color.LightBlue;

					if (fee.ClinicNum != 0 || fee.ProvNum != 0) // FeeSched change, but not with a default fee. Insert placeholder row.
					{
						gridRow.Cells.Add("");
						gridRow.Cells.Add("");
						gridRow.Cells.Add("");
						gridRow.Tag = new Fee
						{
							FeeScheduleId = fee.FeeScheduleId
						};

						feesGrid.Rows.Add(gridRow);

						// Now that we have a placeholder for the default fee (none was found), go about adding the next row (non-default fee).
						gridRow = new GridRow();
						gridRow.Cells.Add("");
					}
				}
				else
				{
					gridRow.Cells.Add("");
				}

				gridRow.Cells.Add(Clinics.GetAbbr(fee.ClinicNum)); 
				gridRow.Cells.Add(Providers.GetAbbr(fee.ProvNum));
				gridRow.Cells.Add(fee.Amount.ToString("n"));
				gridRow.Tag = fee;

				feesGrid.Rows.Add(gridRow);
			}
			feesGrid.EndUpdate();
		}

		private void FeesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var fee = feesGrid.SelectedTag<Fee>();
			if (fee.FeeNum == 0)
			{
				fee.CodeNum = procedureCode.Id;
			}

			using var formFeeEdit = new FormFeeEdit(fee);
			if (formFeeEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();

			hasChanges = true;
		}

		private void CloseButton_Click(object sender, EventArgs e) 
			=> DialogResult = hasChanges ?
				DialogResult.OK : DialogResult.Cancel;
	}
}
