using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDentBusiness;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormComputers : FormBase
	{
		private bool hasChanges;

		public FormComputers()
		{
			InitializeComponent();
		}

		private void FormComputers_Load(object sender, EventArgs e)
		{
			FillList();
			
			var (name, comment, hostname, version) = Computers.GetServiceInfo();

			hostnameTextBox.Text = hostname;
			nameTextBox.Text = name;
			versionTextBox.Text = version;
			commentTextBox.Text = comment;
			workstationTextBox.Text = Environment.MachineName.ToUpper();

			if (!Security.IsAuthorized(Permissions.GraphicsEdit, true))
			{
				fixGraphicsButton.Enabled = false;
			}
		}

		private void FillList()
		{
			Computers.RefreshCache();

			var machineName = Environment.MachineName.ToUpper();

			computersListBox.Items.Clear();

			foreach (var computer in Computers.GetAll())
			{
				computersListBox.Items.Add(computer);
				if (computer.MachineName.Equals(machineName, StringComparison.InvariantCultureIgnoreCase))
                {
					computersListBox.SelectedItem = computer;
                }
			}
		}

		private void ComputersListBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (!(computersListBox.SelectedItem is Computer computer))
			{
				return;
			}

			if (!Security.IsAuthorized(Permissions.GraphicsEdit))
			{
				return;
			}

            using var formGraphics = new FormGraphics
            {
                ComputerPrefCur = ComputerPrefs.GetForComputer(computer.MachineName)
            };

            formGraphics.ShowDialog(this);
		}

		private void ComputersListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			deleteButton.Enabled = fixGraphicsButton.Enabled = computersListBox.SelectedItem != null;
		}

		private void FixGraphicsButton_Click(object sender, EventArgs e)
		{
            if (!(computersListBox.SelectedItem is Computer computer))
            {
                ShowError(Translation.Common.PleaseSelectItemFirst);

                return;
            }

            ComputerPrefs.SetToSimpleGraphics(computer.MachineName);

			ShowInfo(Translation.Common.OperationComplete);

			SecurityLogs.Write(Permissions.GraphicsEdit, 
				string.Format(Translation.Common.SetGraphicsForComputerToSimple, 
					computer.MachineName));
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
            if (!(computersListBox.SelectedItem is Computer computer))
            {
                return;
            }

            Computers.Delete(computer);

			hasChanges = true;

			computersListBox.Items.Remove(computer);
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void FormComputers_Closing(object sender, CancelEventArgs e)
		{
			if (hasChanges)
			{
				CacheManager.RefreshGlobal(nameof(InvalidType.Computers));
			}
		}
    }
}
