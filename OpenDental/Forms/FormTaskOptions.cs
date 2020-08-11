using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormTaskOptions : FormBase
	{
		public bool IsShowFinishedTasks { get; set; }

		public bool IsShowArchivedTaskLists { get; set; }

		public DateTime DateTimeStartShowFinished { get; set; }

		public bool IsSortApptDateTime { get; set; }

		public FormTaskOptions(bool isShowFinishedTasks, DateTime dateTimeStartShowFinished, bool isAptDateTimeSort, bool isShowArchivedTaskLists)
		{
			InitializeComponent();

			showFinishedCheckBox.Checked = isShowFinishedTasks;
			showArchivedTaskListsCheckBox.Checked = isShowArchivedTaskLists;
			startDateTextBox.Text = dateTimeStartShowFinished.ToShortDateString();
			sortApptDateTimeCheckBox.Checked = isAptDateTimeSort;

			defaultCollapsedCheckBox.Checked = UserPreference.GetBool(UserPreferenceName.TaskCollapse);

			if (!isShowFinishedTasks)
			{
				startDateLabel.Enabled = false;
				startDateTextBox.Enabled = false;
			}
		}

		private void checkShowFinished_Click(object sender, EventArgs e)
		{
			if (showFinishedCheckBox.Checked)
			{
				startDateLabel.Enabled = true;
				startDateTextBox.Enabled = true;
			}
			else
			{
				startDateLabel.Enabled = false;
				startDateTextBox.Enabled = false;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!(startDateTextBox.errorProvider1.GetError(startDateTextBox) == ""))
			{
				if (showFinishedCheckBox.Checked)
				{
					MessageBox.Show("Invalid finished task start date");
					return;
				}
				else
				{
					// We are not going to be using the textStartDate so not reason to warn the user, just reset it back to the default value.
					startDateTextBox.Text = DateTime.Today.AddDays(-7).ToShortDateString();
				}
			}

			UserPreference.Set(UserPreferenceName.TaskCollapse, defaultCollapsedCheckBox.Checked);

			IsShowFinishedTasks = showFinishedCheckBox.Checked;
			IsShowArchivedTaskLists = showArchivedTaskListsCheckBox.Checked;
			DateTimeStartShowFinished = PIn.Date(startDateTextBox.Text);
			IsSortApptDateTime = sortApptDateTimeCheckBox.Checked;

			DialogResult = DialogResult.OK;
		}
	}
}
