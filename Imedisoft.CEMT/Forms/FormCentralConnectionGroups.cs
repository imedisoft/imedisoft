using OpenDental.UI;
using OpenDentBusiness;
using System;

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
			var connectionGroups = ConnectionGroups.GetAll();
			var connectionGroupAttached = ConnGroupAttaches.GetAll();

			long defaultConnectionGroupId = PrefC.GetLong(PrefName.ConnGroupCEMT);

			connectionGroupComboBox.Items.Clear();
			connectionGroupComboBox.Items.Add("All");
			connectionGroupComboBox.SelectedIndex = 0;
				
			foreach (var connectionGroup in connectionGroups)
			{
				connectionGroupComboBox.Items.Add(connectionGroup);
				if (connectionGroup.ConnectionGroupNum == defaultConnectionGroupId)
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
				var gridRow = new GridRow();

				gridRow.Cells.Add(connectionGroup.Description);
				gridRow.Cells.Add(connectionGroupAttached.FindAll(x => x.ConnectionGroupNum == connectionGroup.ConnectionGroupNum).Count.ToString());
				gridRow.Tag = connectionGroup;

				connectionGroupsGrid.ListGridRows.Add(gridRow);
			}

			connectionGroupsGrid.EndUpdate();
		}

		private void ConnectionGroupsComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (connectionGroupComboBox.SelectedItem is ConnectionGroup connectionGroup)
            {
				Prefs.UpdateLong(PrefName.ConnGroupCEMT, connectionGroup.ConnectionGroupNum);
			}
            else
            {
				Prefs.UpdateLong(PrefName.ConnGroupCEMT, 0);
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

            connectionGroup.ConnectionGroupNum = ConnectionGroups.Insert(connectionGroup);
			connectionGroup.IsNew = true;

			using (var formCentralConnectionGroupEdit = new FormCentralConnectionGroupEdit(connectionGroup))
			{
				formCentralConnectionGroupEdit.ShowDialog();
			}

			FillGrid();
		}
	}
}
