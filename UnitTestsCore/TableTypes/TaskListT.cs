using Imedisoft.Data;
using OpenDentBusiness;
using System;

namespace UnitTestsCore
{
    public class TaskListT
	{
		/// <summary>
		/// Creates a TaskList.  If parent is not 0, then a parentDesc must also be provided.
		/// </summary>
		public static TaskList CreateTaskList(string description = "", long? parentId = null, string parentDescription = "")
		{
			var taskList = new TaskList
			{
				Description = description,
				ParentId = parentId,
				DateAdded = DateTime.Now,
				ParentDesc = parentDescription,
				NewTaskCount = 0
			};

			TaskLists.Insert(taskList);

			return taskList;
		}

		/// <summary>
		/// Deletes everything from the TaskSubscription table.
		/// Does not truncate the table so that PKs are not reused on accident.
		/// </summary>
		public static void ClearTaskListTable()
		{
			Database.ExecuteNonQuery("DELETE FROM `task_lists`");
		}
	}
}
