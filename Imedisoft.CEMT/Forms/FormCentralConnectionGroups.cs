using Imedisoft.Data.Cemt;
using Imedisoft.Data.Models.Cemt;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Linq;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralConnectionGroups : FormBase
	{
		public FormCentralConnectionGroups() => InitializeComponent();

		private void FormCentralConnectionGroups_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			var connectionGroups = ConnectionGroups.GetAll().ToList();
			var connectionGroupConnectionCounts = ConnectionGroups.GetConnectionGroupConnectionCounts();

			long defaultConnectionGroupId = Prefs.GetLong(PrefName.ConnGroupCEMT);

			connectionGroupComboBox.Items.Clear();
			connectionGroupComboBox.Items.Add("All");
			connectionGroupComboBox.SelectedIndex = 0;
				
			foreach (var connectionGroup in connectionGroups)
			{
				connectionGroupComboBox.Items.Add(connectionGroup);
				if (connectionGroup.Id == defaultConnectionGroupId)
				{
					connectionGroupComboBox.SelectedItem = connectionGroup;
				}
			}

			connectionGroupsGrid.BeginUpdate();
			connectionGroupsGrid.ListGridColumns.Clear();
			connectionGroupsGrid.ListGridColumns.Add(new GridColumn("Group Name", 280));
			connectionGroupsGrid.ListGridColumns.Add(new GridColumn("Conns", 280) { IsWidthDynamic = true });
			connectionGroupsGrid.ListGridRows.Clear();

			foreach (var connectionGroup in connectionGroups)
			{
				if (!connectionGroupConnectionCounts.TryGetValue(connectionGroup.Id, out var connections))
                {
					connections = 0;
                }

				var gridRow = new GridRow();

				gridRow.Cells.Add(connectionGroup.Description);
				gridRow.Cells.Add(connections.ToString());
				gridRow.Tag = connectionGroup;

				connectionGroupsGrid.ListGridRows.Add(gridRow);
			}

			connectionGroupsGrid.EndUpdate();
		}

		private void ConnectionGroupsComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (connectionGroupComboBox.SelectedItem is ConnectionGroup connectionGroup)
            {
				Prefs.Set(PrefName.ConnGroupCEMT, connectionGroup.Id);
			}
            else
            {
				Prefs.Set(PrefName.ConnGroupCEMT, 0);
			}
		}

		private void ConnectionGroupsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (connectionGroupsGrid.SelectedGridRows[e.Row].Tag is ConnectionGroup connectionGroup)
			{
				using (var formCentralConnectionGroupEdit = new FormCentralConnectionGroupEdit(connectionGroup))
				{
					formCentralConnectionGroupEdit.ShowDialog();
				}

				FillGrid();
			}
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
            var connectionGroup = new ConnectionGroup
            {
                Description = "Group"
            };

			using (var formCentralConnectionGroupEdit = new FormCentralConnectionGroupEdit(connectionGroup))
			{
				formCentralConnectionGroupEdit.ShowDialog();
			}

			FillGrid();
		}
	}
}
