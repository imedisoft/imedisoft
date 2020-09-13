using Imedisoft.Data;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Forms
{
    public partial class FormTaskHistory : FormBase
	{
		private readonly long taskId;
		private List<TaskHistory> taskHistory;

		public FormTaskHistory(long taskId)
		{
			InitializeComponent();

			this.taskId = taskId;
		}

		private void FormTaskHistory_Load(object sender, EventArgs e)
		{
			taskHistory = TaskHists.GetArchivesForTask(taskId);

			FillGrid();
		}

		private void FillGrid()
		{
			taskHistoryGrid.BeginUpdate();
			taskHistoryGrid.Columns.Clear();
			taskHistoryGrid.Columns.Add(new GridColumn("Create Date", 140));
			taskHistoryGrid.Columns.Add(new GridColumn("Edit Date", 140));
			taskHistoryGrid.Columns.Add(new GridColumn("Editing User", 80));
			taskHistoryGrid.Columns.Add(new GridColumn("Changes", 100));
			taskHistoryGrid.Rows.Clear();

			for (int i = 1; i < taskHistory.Count; i++)
			{
				var taskHistoryCurrent = taskHistory[i - 1];
				var taskHistoryNext = taskHistory[i];

				var row = new GridRow();
				if (!taskHistoryCurrent.DateStart.HasValue)
				{
					row.Cells.Add(taskHistoryNext.DateStart.ToString());
				}
				else
				{
					row.Cells.Add(taskHistoryCurrent.DateStart.ToString());
				}

				row.Cells.Add(taskHistoryCurrent.HistoryDate.ToString());

				long userId = taskHistoryCurrent.HistoryUserId;
				if (userId == 0)
				{
					userId = taskHistoryCurrent.UserId;
				}

				row.Cells.Add(Users.GetById(userId).UserName);
				row.Cells.Add(TaskHists.GetChangesDescription(taskHistoryCurrent, taskHistoryNext));

				taskHistoryGrid.Rows.Add(row);
			}

			// Compare the current task with the last hist entry (Add the "current revision" of the task if necessary.)
			if (taskHistory.Count > 0)
			{
				Task task = Tasks.GetOne(taskId);

				if (task != null)
				{
					var taskHistoryCurrent = taskHistory[taskHistory.Count - 1];
					var taskHistoryNext = new TaskHistory(task);

					var row = new GridRow();
					if (taskHistoryCurrent.DateStart == DateTime.MinValue)
					{
						row.Cells.Add(taskHistoryNext.DateStart.ToString());
					}
					else
					{
						row.Cells.Add(taskHistoryCurrent.DateStart.ToString());
					}

					row.Cells.Add(taskHistoryCurrent.HistoryDate.ToString());

					long userId = taskHistoryCurrent.HistoryUserId;
					if (userId == 0)
					{
						userId = taskHistoryCurrent.UserId;
					}

					row.Cells.Add(Users.GetById(userId).UserName);
					row.Cells.Add(TaskHists.GetChangesDescription(taskHistoryCurrent, taskHistoryNext));
					taskHistoryGrid.Rows.Add(row);
				}
			}

			taskHistoryGrid.EndUpdate();
		}

		private void CloseButton_Click(object sender, EventArgs e) => Close();
	}
}
