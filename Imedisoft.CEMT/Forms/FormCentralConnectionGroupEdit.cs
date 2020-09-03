using Imedisoft.Data.Cemt;
using Imedisoft.Data.Models.Cemt;
using OpenDental.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralConnectionGroupEdit : FormBase
	{
		public ConnectionGroup connectionGroup;
		private List<Connection> connections = new List<Connection>();
		private List<Connection> connectionsAvailable = new List<Connection>();

		public FormCentralConnectionGroupEdit(ConnectionGroup connectionGroup)
		{
			InitializeComponent();

			this.connectionGroup = connectionGroup;
		}

		private void FormCentralConnectionGroupEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = connectionGroup.Description;

			connections = Connections.GetAllInGroup(connectionGroup.Id).ToList();
			connectionsAvailable = Connections.GetAllNotInGroup(connectionGroup.Id).ToList();

			FillGrid();
			FillGridAvailable();
		}

		private void FillGrid()
		{
			mainGrid.BeginUpdate();
			mainGrid.ListGridColumns.Clear();
			mainGrid.ListGridColumns.Add(new GridColumn("Database", 320));
			mainGrid.ListGridColumns.Add(new GridColumn("Note", 300));
			mainGrid.ListGridRows.Clear();

			foreach (var connection in connections)
			{
				var row = new GridRow();

				row.Cells.Add(connection.Description);
				row.Cells.Add(connection.Note);
				row.Tag = connection;

				mainGrid.ListGridRows.Add(row);
			}

			mainGrid.EndUpdate();
		}

		private void FillGridAvailable()
		{
			availableGrid.BeginUpdate();
			availableGrid.ListGridColumns.Clear();
			availableGrid.ListGridColumns.Add(new GridColumn("Database", 320));
			availableGrid.ListGridColumns.Add(new GridColumn("Note", 300));
			availableGrid.ListGridRows.Clear();

			foreach (var connection in connectionsAvailable)
			{
				var row = new GridRow();

				row.Cells.Add(connection.Description);
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
				if (gridRow.Tag is Connection connection)
				{
					connections.Add(connection);
					connectionsAvailable.Remove(connection);
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
				if (gridRow.Tag is Connection connection)
				{
					connections.Remove(connection);
					connectionsAvailable.Add(connection);
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

			ConnectionGroups.Delete(connectionGroup.Id);

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

			ConnectionGroups.Save(connectionGroup);

			ConnectionGroups.AddConnectionsToGroup(connectionGroup, 
				connections.Select(connection => connection.Id));

			ConnectionGroups.RemoveConnectionsFromGroup(connectionGroup, 
				connectionsAvailable.Select(connection => connection.Id));

			DialogResult = DialogResult.OK;
		}
    }
}
