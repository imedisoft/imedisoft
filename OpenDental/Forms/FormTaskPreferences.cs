using Imedisoft.Data;
using Imedisoft.Data.Cache;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormTaskPreferences : FormBase
	{
		public FormTaskPreferences() => InitializeComponent();

		private void FormTaskPreferences_Load(object sender, EventArgs e)
		{
			alwaysShowTaskListCheckBox.Checked = Preferences.GetBool(PreferenceName.TaskListAlwaysShowsAtBottom);
			localGroupBox.Enabled = alwaysShowTaskListCheckBox.Checked;
			showOpenTicketsCheckBox.Checked = Preferences.GetBool(PreferenceName.TasksShowOpenTickets);
			keepTaskListHiddenCheckBox.Checked = ComputerPrefs.LocalComputer.TaskKeepListHidden;

			if (ComputerPrefs.LocalComputer.TaskDock == 0)
			{
				dockBottomRadioButton.Checked = true;
			}
			else
			{
				dockRightRadioButton.Checked = true;
			}

			xDefaultTextBox.Text = ComputerPrefs.LocalComputer.TaskX.ToString();
			yDefaultTextBox.Text = ComputerPrefs.LocalComputer.TaskY.ToString();
			sortApptDateTimeCheckBox.Checked = Preferences.GetBool(PreferenceName.TaskSortApptDateTime);
		}

		private void InboxSetupButton_Click(object sender, EventArgs e)
		{
            using var formTaskInboxSetup = new FormTaskInboxSetup();

            formTaskInboxSetup.ShowDialog(this);
        }

		private void AlwaysShowTaskListCheckBox_CheckedChanged(object sender, EventArgs e) 
			=> localGroupBox.Enabled = alwaysShowTaskListCheckBox.Checked;

		private void KeepTaskListHiddenCheckBox_CheckedChanged(object sender, EventArgs e) 
			=> dockBottomRadioButton.Enabled =
				dockRightRadioButton.Enabled =
				xDefaultLabel.Enabled =
				yDefaultLabel.Enabled =
				xDefaultTextBox.Enabled =
				yDefaultTextBox.Enabled = !keepTaskListHiddenCheckBox.Checked;

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			int.TryParse(xDefaultTextBox.Text, out var xDefault);
			int.TryParse(yDefaultTextBox.Text, out var yDefault);

			if (FormOpenDental.IsDashboardVisible && alwaysShowTaskListCheckBox.Checked && !keepTaskListHiddenCheckBox.Checked && dockRightRadioButton.Checked)
			{
				ShowError("Tasks cannot be docked to the right when Dashboards are in use.");

				return;
			}

			bool changed =
				Preferences.Set(PreferenceName.TaskListAlwaysShowsAtBottom, alwaysShowTaskListCheckBox.Checked) |
				Preferences.Set(PreferenceName.TasksShowOpenTickets, showOpenTicketsCheckBox.Checked) |
				Preferences.Set(PreferenceName.TaskSortApptDateTime, sortApptDateTimeCheckBox.Checked);

			if (ComputerPrefs.LocalComputer.TaskKeepListHidden != keepTaskListHiddenCheckBox.Checked)
			{
				ComputerPrefs.LocalComputer.TaskKeepListHidden = keepTaskListHiddenCheckBox.Checked;

				changed = true; // needed to trigger screen refresh
			}

			var dock = dockBottomRadioButton.Checked ? 0 : 1;
			if (ComputerPrefs.LocalComputer.TaskDock != dock)
			{
				ComputerPrefs.LocalComputer.TaskDock = dock;

				changed = true;
			}

			if (xDefault != ComputerPrefs.LocalComputer.TaskX)
			{
				ComputerPrefs.LocalComputer.TaskX = xDefault;

				changed = true;
			}

			if (yDefault != ComputerPrefs.LocalComputer.TaskY)
			{
				ComputerPrefs.LocalComputer.TaskY = yDefault;

				changed = true;
			}

			if (changed)
			{
				CacheManager.Refresh(nameof(InvalidType.Computers));
				CacheManager.Refresh(nameof(InvalidType.Prefs));

				ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			}

			DialogResult = DialogResult.OK;
		}

        private void DefaultTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
				e.Handled = true;
            }
        }

        private void DefaultTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
			if (sender is TextBox textBox)
            {
				if (int.TryParse(textBox.Text, out var value))
                {
					textBox.Text = value.ToString();
                }
                else
                {
					textBox.Text = 0.ToString();
                }
            }
        }
    }
}
