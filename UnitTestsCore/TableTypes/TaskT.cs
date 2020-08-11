using Imedisoft.Data;
using OpenDentBusiness;
using System;

namespace UnitTestsCore
{
	public class TaskT
	{
		/// <summary>
		/// Creates a TaskSubscription, dateTimeEntry will be DateTime.Now if not specified.
		/// </summary>
		public static Task CreateTask(long taskListNum = 0, long keyNum = 0, string descript = "",
			TaskStatus taskStatus = TaskStatus.New, bool isRepeating = false, TaskRepeatInterval dateType = TaskRepeatInterval.Never, long fromNum = 0,
			DateTime dateTimeEntry = new DateTime(), long userNum = 0, bool isUnread = false, string parentDesc = "", string patientName = "",
			long priorityDefNum = 0, string reminderGroupId = "", TaskReminderType reminderType = TaskReminderType.None, int reminderFrequency = 0)
		{
			if (dateTimeEntry == DateTime.MinValue)
			{
				dateTimeEntry = DateTime.Now;
			}

			Task task = new Task
			{
				TaskListId = taskListNum,
				RepeatDate = DateTime.MinValue,
				Description = descript,
				Status = taskStatus,
				Repeat = isRepeating,
				RepeatInterval = dateType,
				RepeatTaskId = fromNum,
				DateStart = dateTimeEntry,
				UserId = userNum,
				DateCompleted = DateTime.MinValue,
				IsUnread = isUnread,
				PatientName = patientName,
				PriorityId = priorityDefNum,
				ReminderGroupId = reminderGroupId,
				ReminderType = reminderType,
				ReminderFrequency = reminderFrequency,
				DateAdded = DateTime.Now
			};

			Tasks.Insert(task);

			task = Tasks.GetOne(task.Id);//Make sure task matches Db. Avoids problems with DateTime columns.

			return task;
		}

		/// <summary>
		/// Deletes everything from the TaskSubscription table. 
		/// Does not truncate the table so that PKs are not reused on accident.
		/// </summary>
		public static void ClearTaskTable()
		{
			string command = "DELETE FROM task";
			Database.ExecuteNonQuery(command);
		}
	}
}
