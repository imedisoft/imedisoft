using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormTaskInboxSetup : FormBase
	{
		private readonly List<User> users = new List<User>();
		private readonly Dictionary<long, long?> userInboxIds = new Dictionary<long, long?>();
		private readonly List<TaskList> taskLists = new List<TaskList>();

		public FormTaskInboxSetup()
		{
			InitializeComponent();
		}

		private void FormTaskInboxSetup_Load(object sender, EventArgs e)
		{
			users.AddRange(Users.GetAll(true));
			foreach (var user in users)
            {
				userInboxIds[user.Id] = user.InboxTaskListId;
            }

			taskLists.AddRange(TaskLists.GetTrunk().Where(x => x.Status == TaskListStatus.Active));

			trunkListBox.Items.Add(Translation.Common.None);
			foreach (var taskList in taskLists)
			{
				trunkListBox.Items.Add(taskList);
			}

			FillGrid();
		}

		private void FillGrid()
		{
			usersGrid.BeginUpdate();
			usersGrid.Columns.Clear();
			usersGrid.Columns.Add(new GridColumn(Translation.Common.User, 100));
			usersGrid.Columns.Add(new GridColumn(Translation.Common.Inbox, 100));
			usersGrid.Rows.Clear();

			foreach (var user in users)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(user.UserName);
				gridRow.Cells.Add(GetDescription(user.InboxTaskListId));
				gridRow.Tag = user;

				usersGrid.Rows.Add(gridRow);
			}

			usersGrid.EndUpdate();
		}

		private string GetDescription(long? taskListId)
		{
			if (!taskListId.HasValue) return "";

			return taskLists.FirstOrDefault(tl => tl.Id == taskListId)?.Description ?? "";
		}

		private void SetButton_Click(object sender, EventArgs e)
		{
			var user = usersGrid.SelectedTag<User>();
			if (user == null)
			{
				ShowError(Translation.Common.PleaseSelectUser);

				return;
			}

            if (!(trunkListBox.SelectedItem is TaskList taskList))
            {
                return;
            }

			var taskListId = taskList?.Id;
			if (taskListId == user.InboxTaskListId)
            {
				return;
            }

            user.InboxTaskListId = taskListId;

			FillGrid();

			trunkListBox.SelectedIndex = -1;
		}

		private void TrunkListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			setButton.Enabled = trunkListBox.SelectedItem != null;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			bool changed = false;

			var updateErrors = new Dictionary<string, List<User>>();

			foreach (var user in users)
			{
				if (!userInboxIds.TryGetValue(user.Id, out var inboxId) || user.InboxTaskListId != inboxId)
				{
					try
					{
						Users.Update(user);

						changed = true;
					}
					catch (Exception exception)
					{
						if (!updateErrors.TryGetValue(exception.Message, out var errorUsers))
                        {
							updateErrors[exception.Message] 
								= errorUsers = new List<User>();
                        }

						errorUsers.Add(user);
					}
				}
			}

			if (updateErrors.Count > 0)
			{
				var stringBuilder = new StringBuilder();

				foreach (var kvp in updateErrors)
				{
					foreach (var user in kvp.Value)
					{
						stringBuilder.AppendLine("  " + user.UserName + " - " + kvp.Key);
					}
				}

				ShowError(Translation.Common.TheFollowingUsersCouldNotBeUpdated + 
					"\r\n" + stringBuilder.ToString());
			}

			if (changed)
			{
				CacheManager.RefreshGlobal(nameof(InvalidType.Security));
			}

			DialogResult = DialogResult.OK;
		}
    }
}
