using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormTaskListEdit : FormBase
	{
		private readonly TaskList taskList;

		public FormTaskListEdit(TaskList taskList)
		{
			InitializeComponent();

			this.taskList = taskList;
		}

		private void FormTaskListEdit_Load(object sender, EventArgs e)
		{
			descriptionTextBox.Text = taskList.Description;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var description = descriptionTextBox.Text.Trim();
			if (description.Length == 0)
            {
				ShowError("Please enter a description.");

				return;
            }

			taskList.Description = description;

			try
			{
				if (taskList.Id == 0)
				{
					TaskLists.Insert(taskList);

					SecurityLogs.MakeLogEntry(Permissions.TaskListCreate, 0, taskList.Description + " added");
				}
				else
				{
					TaskLists.Update(taskList);
				}
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			DialogResult = DialogResult.OK;
		}
	}
}
