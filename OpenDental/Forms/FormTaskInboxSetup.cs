using Imedisoft.Data.Cache;
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
		private List<Userod> users;
		private readonly Dictionary<long, long?> userInboxIds = new Dictionary<long, long?>();
		private List<TaskList> taskLists;

		public FormTaskInboxSetup()
		{
			InitializeComponent();
		}

		private void FormTaskInboxSetup_Load(object sender, EventArgs e)
		{
			users = Userods.GetAll(true);
			foreach (var user in users)
            {
				userInboxIds[user.Id] = user.InboxTaskListId;
            }

			taskLists = TaskLists.GetTrunk().ToList().FindAll(x => x.Status == TaskListStatus.Active);

			trunkListBox.Items.Add("none");
			foreach (var taskList in taskLists)
			{
				trunkListBox.Items.Add(taskList);
			}

			FillGrid();
		}

		private void FillGrid()
		{
			usersGrid.BeginUpdate();
			usersGrid.ListGridColumns.Clear();
			usersGrid.ListGridColumns.Add(new GridColumn("User", 100));
			usersGrid.ListGridColumns.Add(new GridColumn("Inbox", 100));
			usersGrid.ListGridRows.Clear();

			foreach (var user in users)
			{
				var row = new GridRow();

				row.Cells.Add(user.UserName);
				row.Cells.Add(GetDescription(user.InboxTaskListId));

				usersGrid.ListGridRows.Add(row);
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
			if (usersGrid.GetSelectedIndex() == -1)
			{
				ShowError("Please select a user first.");
				return;
			}

			if (trunkListBox.SelectedIndex == -1)
			{
				ShowError("Please select an item from the list first.");
				return;
			}

			if (trunkListBox.SelectedItem is TaskList taskList)
            {
				users[usersGrid.GetSelectedIndex()].InboxTaskListId = taskList.Id;
            }
            else
            {
				users[usersGrid.GetSelectedIndex()].InboxTaskListId = null;
			}

			FillGrid();

			trunkListBox.SelectedIndex = -1;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			bool changed = false;

			var updateErrors = new Dictionary<string, List<Userod>>();

			foreach (var user in users)
			{
				if (!userInboxIds.TryGetValue(user.Id, out var inboxId) || user.InboxTaskListId != inboxId)
				{
					try
					{
						Userods.Update(user);

						changed = true;
					}
					catch (Exception exception)
					{
						if (!updateErrors.TryGetValue(exception.Message, out var errorUsers))
                        {
							updateErrors[exception.Message] 
								= errorUsers = new List<Userod>();
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

				ShowError(
					"The following users could not be updated:\r\n" + stringBuilder.ToString());
			}

			if (changed)
			{
				CacheManager.Refresh(nameof(InvalidType.Security));
			}

			DialogResult = DialogResult.OK;
		}
	}
}
