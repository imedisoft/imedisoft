using CodeBase;
using Imedisoft.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public partial class TaskLists
	{
		/// <summary>
		/// Gets all task lists for the trunk of the user tab. 
		/// </summary>
		public static IEnumerable<TaskList> RefreshUserTrunk(long userId) 
			=> SelectMany(
				"SELECT tasklist.*," +
					"COALESCE(unreadtasks.Count,0) 'NewTaskCount',t2.Descript 'ParentDesc1',t3.Descript 'ParentDesc2' " +
				"FROM tasklist " +
				"LEFT JOIN tasksubscription " +
					"ON tasksubscription.TaskListNum=tasklist.TaskListNum " +
				"LEFT JOIN tasklist t2 " +
					"ON t2.TaskListNum=tasklist.Parent " +
				"LEFT JOIN tasklist t3 " +
					"ON t3.TaskListNum=t2.Parent " +
				"LEFT JOIN (" +
					"SELECT taskancestor.TaskListNum,COUNT(*) 'Count' " +
					"FROM taskancestor " +
					"INNER JOIN task " +
						"ON task.TaskNum=taskancestor.TaskNum " +
						"AND NOT(COALESCE(task.ReminderGroupId,'') != '' " +
						"AND task.DateTimeEntry > NOW()" +
				") " +
				"INNER JOIN taskunread " +
					"ON taskunread.TaskNum=task.TaskNum " +
				"WHERE taskunread.UserNum = " + userId + " " +
				"AND task.TaskStatus!=" + (int)TaskStatus.Done + " " +
				"GROUP BY taskancestor.TaskListNum) unreadtasks " +
					"ON unreadtasks.TaskListNum = tasklist.TaskListNum " +
					"WHERE tasksubscription.UserNum=" + userId + " " +
				"AND tasksubscription.TaskListNum!=0 " +
				"ORDER BY tasklist.Descript,tasklist.DateTimeEntry");

		public static IEnumerable<TaskList> GetTrunk() 
			=> SelectMany(
				"SELECT * FROM `task_lists` " +
				"WHERE `parent_id` IS NULL " +
				"ORDER BY `description`, `date_added`");

		public static IEnumerable<TaskList> GetChildren(long taskListId)
			=> SelectMany(
				"SELECT * FROM `task_lists` " +
				"WHERE `parent_id` = " + taskListId + " " +
				"ORDER BY `description`, `date_added`");

		public static TaskList GetOne(long taskListId)
			=> SelectOne(taskListId);

		public static IEnumerable<TaskList> GetAll() 
			=> SelectMany("SELECT * FROM `task_lists`");

		public static IEnumerable<long> GetIdsByDescription(string description) 
			=> Database.SelectMany("SELECT `id` FROM `task_lists` WHERE `description` LIKE '%" + MySqlHelper.EscapeString(description) + "%'", Database.ToScalar<long>);

		/// <summary>
		///		<para>
		///			Attempts to delete the specified task list from the database.
		///		</para>
		///		<para>
		///			If there is a condition that prevents the list from being deleted a exception 
		///			will thrown. The exception message will be translated and can be shown directly
		///			to the user.
		///		</para>
		/// </summary>
		public static void Delete(TaskList taskList)
		{
			var count = Database.ExecuteLong("SELECT COUNT(*) FROM `task_lists` WHERE `parent_id` = " + taskList.Id);
			if (count > 0)
				throw new Exception("Not allowed to delete task list because it still has child lists attached.");

			count = Database.ExecuteLong("SELECT COUNT(*) FROM `tasks` WHERE `task_list_id` = " + taskList.Id);
			if (count > 0)
				throw new Exception("Not allowed to delete task list because it still has child tasks attached.");

			count = Database.ExecuteLong("SELECT COUNT(*) FROM `userod` WHERE `inbox_task_list_id` = " + taskList.Id);
			if (count > 0)
				throw new Exception("Not allowed to delete task list because it is attached to a user inbox.");

			DeleteInternal(taskList.Id);
		}

		/// <summary>
		/// Determines whether the task list identified by <paramref name="childTaskListId"/> is a child
		/// of the task list identified by <paramref name="taskListId"/>.
		/// </summary>
		public static bool IsAncestor(long taskListId, long childTaskListId)
		{
			long parentId = childTaskListId;

			while (true)
			{
				parentId = Database.ExecuteLong("SELECT `parent_id` FROM `task_lists` WHERE `id` = " + parentId);
				if (parentId == 0)
				{
					return false;
				}

				if (parentId == taskListId)
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Will return 0 if not anyone's inbox.
		/// </summary>
		public static long GetMailboxUserNum(long taskListId) 
			=> Database.ExecuteLong("SELECT UserNum FROM userod WHERE TaskListInBox=" + taskListId);

		/// <summary>
		/// Build the full path to the passed in task list.
		/// Returns the string in the standard Windows path format.
		/// </summary>
		public static string GetFullPath(long taskListId)
		{
			var taskList = GetOne(taskListId);
			
			if (taskList == null) return "";

			var stringBuilder = new StringBuilder();

			stringBuilder.Append(taskList.Description);

			while (taskList.ParentId.HasValue)
			{
				taskList = GetOne(taskList.ParentId.Value);

				if (taskList == null) break;

				stringBuilder.Insert(0, taskList.Description + "/");
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// DateType and ObjectType to None, the TaskListStatus to 1 - Archived, and set all Task List Inboxes that reference this Task List to 0.
		/// </summary>
		public static void Archive(TaskList taskList)
		{
			if (taskList.Status != TaskListStatus.Active)
			{
				return;
			}

			TaskList taskListOld = taskList.Copy();
			taskList.Status = TaskListStatus.Archived;

			Update(taskList, taskListOld);

			Userods.DisassociateTaskListInBox(taskList.Id);

			Signalods.SetInvalid(InvalidType.Security); // Send a signal in case any userod was associated to the task list.
		}

		/// <summary>
		/// Set the TaskListStatus to 0 - Active.
		/// </summary>
		public static void Unarchive(TaskList taskList)
		{
			if (taskList.Status != TaskListStatus.Archived)
			{
				return;
			}

			TaskList taskListOld = taskList.Copy();
			taskList.Status = TaskListStatus.Active;
			Update(taskList, taskListOld);
		}

		/// <summary>
		/// False if taskList has no parent, all of taskList's ancestors are not archived, or taskList ancestor can't be found.
		/// </summary>
		public static bool IsAncestorTaskListArchived(ref Dictionary<long, TaskList> allTaskLists, TaskList taskList, bool isDictionaryRefreshed = false)
		{
			if (!taskList.ParentId.HasValue) return false;

			if (allTaskLists.TryGetValue(taskList.ParentId.Value, out var list))
			{ 
				if (list.Status == TaskListStatus.Archived)
                {
					return true;
                }

				return IsAncestorTaskListArchived(ref allTaskLists, list, isDictionaryRefreshed);
            }

			if (!isDictionaryRefreshed)
			{
				allTaskLists = GetAll().ToDictionary(x => x.Id);

				return IsAncestorTaskListArchived(ref allTaskLists, taskList, true);
			}

			return false;
		}
	}
}
