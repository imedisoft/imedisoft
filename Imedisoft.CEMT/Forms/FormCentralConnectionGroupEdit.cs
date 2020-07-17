using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralConnectionGroupEdit : FormBase
	{
		public ConnectionGroup connectionGroup;

		public FormCentralConnectionGroupEdit(ConnectionGroup connectionGroup)
		{
			InitializeComponent();

			this.connectionGroup = connectionGroup;
		}

		private void FormCentralConnectionGroupEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = connectionGroup.Description;

			FillGrid();
			FillGridAvailable();
		}

		private void FillGrid()
		{
			var connections = CentralConnections.GetForGroup(connectionGroup.ConnectionGroupNum);

			mainGrid.BeginUpdate();
			mainGrid.ListGridColumns.Clear();
			mainGrid.ListGridColumns.Add(new GridColumn("Database", 320));
			mainGrid.ListGridColumns.Add(new GridColumn("Note", 300));
			mainGrid.ListGridRows.Clear();

			foreach (var connection in connections)
			{
				var row = new GridRow();

				row.Cells.Add(connection.ServerName + ", " + connection.DatabaseName);
				row.Cells.Add(connection.Note);
				row.Tag = connection;

				mainGrid.ListGridRows.Add(row);
			}

			mainGrid.EndUpdate();
		}

		private void FillGridAvailable()
		{
			var connections = CentralConnections.GetNotForGroup(connectionGroup.ConnectionGroupNum);

			availableGrid.BeginUpdate();
			availableGrid.ListGridColumns.Clear();
			availableGrid.ListGridColumns.Add(new GridColumn("Database", 320));
			availableGrid.ListGridColumns.Add(new GridColumn("Note", 300));
			availableGrid.ListGridRows.Clear();

			foreach (var connection in connections)
			{
				var row = new GridRow();

				row.Cells.Add(connection.ServerName + ", " + connection.DatabaseName);
				row.Cells.Add(connection.Note);
				row.Tag = connection;

				availableGrid.ListGridRows.Add(row);
			}

			availableGrid.EndUpdate();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			if (availableGrid.SelectedIndices.Length == 0)
			{
				ShowInfo("Please select connections first.");
				return;
			}

			foreach (var gridRow in availableGrid.SelectedGridRows)
			{
				if (gridRow.Tag is CentralConnection centralConnection)
				{
					ConnGroupAttaches.Attach(
						centralConnection.CentralConnectionNum, 
						connectionGroup.ConnectionGroupNum);
				}
			}

			FillGrid();
			FillGridAvailable();
		}

		private void RemoveButton_Click(object sender, EventArgs e)
		{
			if (mainGrid.SelectedIndices.Length == 0)
			{
				ShowInfo("Please select a connection first.");
				return;
			}


			foreach (var gridRow in mainGrid.SelectedGridRows)
			{
				if (gridRow.Tag is CentralConnection centralConnection)
				{
					ConnGroupAttaches.Detach(
						centralConnection.CentralConnectionNum,
						connectionGroup.ConnectionGroupNum);
				}
			}

			FillGrid();
			FillGridAvailable();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (Confirm("Delete this entire connection group?") == DialogResult.No)
			{
				return;
			}

			ConnGroupAttaches.DeleteForGroup(connectionGroup.ConnectionGroupNum);
			ConnectionGroups.Delete(connectionGroup.ConnectionGroupNum);

			DialogResult = DialogResult.OK;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
			{
				ShowError("Please enter a description.");

				return;
			}

			connectionGroup.Description = description;

			ConnectionGroups.Update(connectionGroup);

			DialogResult = DialogResult.OK;
		}

		private void FormCentralConnectionGroupEdit_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.OK) return;

			if (connectionGroup.IsNew)
			{
				ConnGroupAttaches.DeleteForGroup(connectionGroup.ConnectionGroupNum);
				ConnectionGroups.Delete(connectionGroup.ConnectionGroupNum);
			}
		}
	}
}
