using Imedisoft.Data;
using OpenDental.UI;
using System;

namespace Imedisoft.Forms
{
    public partial class FormPreviousVersions : FormBase
	{
		public FormPreviousVersions()
		{
			InitializeComponent();
		}

		private void FormPreviousVersions_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			updateHistoriesGrid.BeginUpdate();
			updateHistoriesGrid.Columns.Clear();
			updateHistoriesGrid.Columns.Add(new GridColumn(Translation.Common.Version, 117));
			updateHistoriesGrid.Columns.Add(new GridColumn(Translation.Common.Date, 117));
			updateHistoriesGrid.Rows.Clear();

			foreach (var updateHistory in UpdateHistories.GetAll())
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(updateHistory.Version);
				gridRow.Cells.Add(updateHistory.InstalledOn.ToString());

				updateHistoriesGrid.Rows.Add(gridRow);
			}

			updateHistoriesGrid.EndUpdate();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
