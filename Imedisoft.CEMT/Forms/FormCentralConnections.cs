using Imedisoft.Data.Cemt;
using Imedisoft.Data.Models.Cemt;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralConnections : FormBase
	{
		private List<Connection> allConnections;
		private List<Connection> shownConnections;
		private List<ConnectionGroup> connectionGroups;

		/// <summary>
		/// Gets the selected connections.
		/// </summary>
		public List<Connection> SelectedConnections { get; } = new List<Connection>();

		/// <summary>
		/// Used when selecting a connection for patient transfer and when pushing security settings to a db.
		/// </summary>
		public bool IsSelectionMode { get; set; }

		public FormCentralConnections()
		{
			InitializeComponent();
		}

		private void FormCentralConnections_Load(object sender, EventArgs e)
		{
			connectionGroups = ConnectionGroups.GetAll().ToList();
			connectionGroupComboBox.Items.Add("All");
			connectionGroupComboBox.Items.AddRange(connectionGroups.Select(x => x.Description).ToArray());
			connectionGroupComboBox.SelectedIndex = 0;

			if (IsSelectionMode)
			{
				addButton.Visible = false;
			}
			else
			{
				acceptButton.Visible = false;
				cancelButton.Text = "Close";
			}

			allConnections = Connections.GetAll().ToList();

			bool reordered = false;

			for (int i = 0; i < allConnections.Count; i++)
			{
				if (allConnections[i].ItemOrder != i)
				{
					reordered = true;

					allConnections[i].ItemOrder = i;

					Connections.Update(allConnections[i]);
				}
			}

			if (reordered) allConnections = Connections.GetAll().ToList();

			FillGrid();
		}

		public IEnumerable<Connection> FilterConnections(IEnumerable<Connection> connections, string filterText)
		{
			if (connectionGroupComboBox.SelectedItem is ConnectionGroup connectionGroup)
			{
				var connectionIds = Connections.GetAllInGroup(connectionGroup.Id).Select(connection => connection.Id).ToList();

				connections = connections.Where(
					connection => connectionIds.Contains(connection.Id));
			}

			return connections.Where(x => x.ToString().ToLower().Contains(filterText.ToLower()));
		}

		private void FillGrid()
		{
			shownConnections = FilterConnections(allConnections, searchTextBox.Text).ToList();

			connectionsGrid.BeginUpdate();
			connectionsGrid.ListGridColumns.Clear();
			connectionsGrid.ListGridColumns.Add(new GridColumn("#", 40));
			connectionsGrid.ListGridColumns.Add(new GridColumn("Database", 300));
			connectionsGrid.ListGridColumns.Add(new GridColumn("Note", 260));
			connectionsGrid.ListGridRows.Clear();

			var selectedIndices = connectionsGrid.SelectedIndices;
			for (int i = 0; i < shownConnections.Count; i++)
			{
				var row = new GridRow();

				row.Cells.Add(shownConnections[i].ItemOrder.ToString());
				row.Cells.Add(shownConnections[i].Description);
				row.Cells.Add(shownConnections[i].Note);

				connectionsGrid.ListGridRows.Add(row);
			}

			connectionsGrid.EndUpdate();
			connectionsGrid.SetSelected(selectedIndices, true);
		}

		private void ConnectionGroupComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			SetToolsEnabled();
			FillGrid();
		}

		private void SearchTextBox_TextChanged(object sender, EventArgs e)
		{
			SetToolsEnabled();
			FillGrid();
		}

		private void SetToolsEnabled()
		{
			if (connectionGroupComboBox.SelectedIndex == 0 && searchTextBox.Text == "")
			{
				upButton.Enabled = true;
				downButton.Enabled = true;
				alphabetizeButton.Enabled = true;
			}
			else
			{
				upButton.Enabled = false;
				downButton.Enabled = false;
				alphabetizeButton.Enabled = false;
			}
		}

		private void ConnectionsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (IsSelectionMode)
			{
				SelectedConnections.Clear();
				SelectedConnections.Add(shownConnections[e.Row]);

                DialogResult = DialogResult.OK;

				return;
			}

			using (var formCentralConnectionEdit = new FormCentralConnectionEdit(shownConnections[e.Row]))
			{
				formCentralConnectionEdit.ShowDialog();
			}

			allConnections = Connections.GetAll().ToList();

			FillGrid();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var connection = new Connection();

            using (var formCentralConnectionEdit = new FormCentralConnectionEdit(connection))
			{
				formCentralConnectionEdit.LastItemOrder = 0;
				if (allConnections.Count > 0)
				{
					formCentralConnectionEdit.LastItemOrder = allConnections[allConnections.Count - 1].ItemOrder;
				}

				formCentralConnectionEdit.ShowDialog(this);
			}

			allConnections = Connections.GetAll().ToList();

			FillGrid();
		}

		private void UpButton_Click(object sender, EventArgs e)
		{
			if (connectionsGrid.SelectedIndices.Length == 0)
			{
				ShowInfo("Please select a connection, first.");
				return;
			}

			if (connectionsGrid.SelectedIndices.Length > 1)
			{
				ShowInfo("Please only select one connection.");
				return;
			}

			int index = connectionsGrid.SelectedIndices[0];
			if (shownConnections.Count < 2 || index == 0)
			{
				return;
			}

			shownConnections[index].ItemOrder--;
			Connections.Update(shownConnections[index]);

			shownConnections[index - 1].ItemOrder++;
			Connections.Update(shownConnections[index - 1]);

			allConnections = Connections.GetAll().ToList();

			FillGrid();

			connectionsGrid.SetSelected(index - 1, true);
		}

		private void DownButton_Click(object sender, EventArgs e)
		{
			if (connectionsGrid.SelectedIndices.Length == 0)
			{
				ShowInfo("Please select a connection, first.");
				return;
			}

			if (connectionsGrid.SelectedIndices.Length > 1)
			{
				ShowInfo("Please only select one connection.");
				return;
			}

			int index = connectionsGrid.SelectedIndices[0];
			if (shownConnections.Count < 2 || index == shownConnections.Count - 1)
			{
				return;
			}

			shownConnections[index].ItemOrder++;
			Connections.Update(shownConnections[index]);

			shownConnections[index + 1].ItemOrder--;
			Connections.Update(shownConnections[index + 1]);

			allConnections = Connections.GetAll().ToList();

			FillGrid();

			connectionsGrid.SetSelected(index + 1, true);
		}

		private void AlphabetizeButton_Click(object sender, EventArgs e)
		{
			int SortByNote(Connection x, Connection y)
            {
				return x.Note.CompareTo(y.Note);
			}

			shownConnections.Sort(SortByNote);

			for (int i = 0; i < shownConnections.Count; i++)
			{
				shownConnections[i].ItemOrder = i;

				Connections.Update(shownConnections[i]);
			}

			allConnections = Connections.GetAll().ToList();

			FillGrid();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (connectionsGrid.SelectedIndices.Length == 0)
			{
				ShowInfo("Please select connection(s) first.");
				return;
			}

			SelectedConnections.Clear();
			for (int i = 0; i < connectionsGrid.SelectedIndices.Length; i++)
			{
				SelectedConnections.Add(shownConnections[connectionsGrid.SelectedIndices[i]]);
			}

			DialogResult = DialogResult.OK;
		}
	}
}
