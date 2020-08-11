using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness
{
    public partial class Tasks
	{
		/// <summary>
		/// Returns true if task does not exist in the database.
		/// </summary>
		public static bool IsTaskDeleted(long taskId) 
			=> Database.ExecuteLong("SELECT COUNT(*) FROM `tasks` WHERE `id` = " + taskId) == 0;

		/// <summary>
		/// Returns true if task is a Reminder Task.
		/// </summary>
		public static bool IsReminderTask(Task task) 
			=> !string.IsNullOrEmpty(task.ReminderGroupId) && task.ReminderType != TaskReminderType.None;

		/// <summary>
		/// Gets one Task from database.
		/// </summary>
		public static Task GetOne(long taskId) 
			=> SelectOne(taskId);


		public class TaskSearchResult
        {
			public long Id;
			public string Description;
			public string List;
			public string Note;
			public long? PatientId;
			public string DateAdded;
			public string DateCompleted;
			public int Color;
        }

		/// <summary>
		/// Gets all tasks for the Task Search function, limited to 50 by default.
		/// </summary>
		public static IEnumerable<TaskSearchResult> GetDataSet(
			long? userId, 
			IEnumerable<long> taskListIds,
			IEnumerable<long> taskIds, 
			string taskDateCreatedFrom, 
			string taskDateCreatedTo,
			string taskDateCompletedFrom,
			string taskDateCompletedTo, 
			string taskDescription, 
			long? priorityId, 
			long? patientId,
			bool doIncludeCompleted,
			bool limit)
		{
			static DateTime? ParseDate(string dateStr)
            {
				if (!string.IsNullOrEmpty(dateStr) && DateTime.TryParse(dateStr, out var date))
                {
					return date;
                }

				return null;
            }

			var dateCreatedFrom = ParseDate(taskDateCreatedFrom);
			var dateCreatedTo = ParseDate(taskDateCreatedTo);
			var dateCompletedFrom = ParseDate(taskDateCompletedFrom);
			var dateCompletedTo = ParseDate(taskDateCompletedTo);

			var searchTaskIds =
				GetTaskIdsForSearch(userId, taskListIds, taskIds, 
					dateCreatedFrom, dateCreatedTo, 
					dateCompletedFrom, dateCompletedTo, 
					taskDescription, priorityId, patientId, doIncludeCompleted, limit);

			var tasks = GetMany(searchTaskIds.ToList())
				.OrderByDescending(x => x.DateAdded == DateTime.MinValue ? x.DateStart : x.DateAdded);

			List<Def> definitions = Defs.GetDefsForCategory(DefCat.ProgNoteColors, true);

			int colorText = Defs.GetColor(DefCat.ProgNoteColors, definitions[18].DefNum).ToArgb();//18="Patient Note Text"
			int colorTextCompleted = Defs.GetColor(DefCat.ProgNoteColors, definitions[20].DefNum).ToArgb();//20="Completed Pt Note Text"

			foreach (var task in tasks)
			{
				var searchResult = new TaskSearchResult
				{
					Id = task.Id,
					Description = task.Description,
					Note = TaskLists.GetFullPath(task.TaskListId),
					PatientId = task.PatientId,
					DateAdded = task.DateAdded.ToShortDateString(),
					DateCompleted = "",
					Color = colorText
				};

				if (task.Status == TaskStatus.Done)
					searchResult.Color = colorTextCompleted;

				if (task.DateCompleted.HasValue)
					searchResult.DateCompleted = task.DateCompleted.Value.ToShortDateString();

				yield return searchResult;
			}
		}

		/// <summary>
		///		<para>
		///			Gets the ID's of all the tasks that match the specified search criteria.
		///		</para>
		/// </summary>
		public static IEnumerable<long> GetTaskIdsForSearch(long? userId, IEnumerable<long> taskListIds, IEnumerable<long> taskIds, DateTime? createdFromDate, DateTime? createdToDate, DateTime? completedFromDate, DateTime? completedToDate, string description, long? priorityId, long? patientId, bool searchInCompleted, bool limit)
		{
			var criteria = new List<string>();
			var criteriaParams = new List<MySqlParameter>();

			// Filter: STATUS
			if (!searchInCompleted)
			{
				criteria.Add("`status` != " + (int)TaskStatus.Done);
			}

			// Filter: USER_ID
			if (userId.HasValue) criteria.Add("`user_id` = " + userId.Value);

			// Filter: TASK_LIST_ID
			if (taskListIds != null && taskListIds.Any())
			{
				criteria.Add("`task_list_id` IN (" + string.Join(", ", taskListIds) + ")");
			}

			// Filter: ID
			if (taskIds != null && taskIds.Any())
			{
				criteria.Add("`id` IN (" + string.Join(", ", taskIds) + ")");
			}

			// Filter: PRIORITY_ID
			if (priorityId.HasValue) criteria.Add("`priority_id` = " + priorityId.Value);

			// Filter: PATIENT_ID
			if (patientId.HasValue) criteria.Add("`patient_id` = " + patientId.Value);

			// Filter: DATE_ADDED
			if (createdFromDate.HasValue)
            {
				if (createdToDate.HasValue)
                {
					criteria.Add("DATE(`date_added`) BETWEEN ?date_added_min AND ?date_added_max");
					criteriaParams.Add(new MySqlParameter("?date_added_max", createdToDate.Value));

				}
                else
                {
					criteria.Add("DATE(`date_added`) >= ?date_added_min");
				}
				criteriaParams.Add(new MySqlParameter("?date_added_min", createdFromDate.Value));
			}
			else if (createdToDate.HasValue)
            {
				criteria.Add("DATE(`date_added`) <= ?date_added_max");
				criteriaParams.Add(new MySqlParameter("?date_added_max", createdToDate.Value));
			}

			// Filter: DATE_COMPLETED
			if (completedFromDate.HasValue)
			{
				if (completedToDate.HasValue)
				{
					criteria.Add("DATE(`date_completed`) BETWEEN ?date_completed_min AND ?date_completed_max");
					criteriaParams.Add(new MySqlParameter("?date_completed_max", completedToDate.Value));

				}
				else
				{
					criteria.Add("DATE(`date_completed`) >= ?date_completed_min");
				}
				criteriaParams.Add(new MySqlParameter("?date_completed_min", completedFromDate.Value));
			}
			else if (completedToDate.HasValue)
			{
				criteria.Add("DATE(`date_completed`) <= ?date_completed_max");
				criteriaParams.Add(new MySqlParameter("?date_completed_max", completedToDate.Value));
			}

			// Filter: DESCRIPTION
			if (!string.IsNullOrEmpty(description))
            {
				var words = description.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (var word in words)
                {
					criteria.Add("`description` LIKE '%" + MySqlHelper.EscapeString(word) + "%'");
                }
            }

			var where = "";
			if (criteria.Count > 0)
            {
				where += " WHERE " + string.Join(" AND ", criteria);
            }

			return Database.SelectMany(
				"SELECT `id` FROM `tasks`" + where + " ORDER BY `date_added` DESC" + (limit ? " LIMIT 50" : ""),
					Database.ToScalar<long>, criteriaParams.ToArray());
		}

		/// <summary>
		/// Gets all tasks with the given ID's.
		/// </summary>
		/// <param name="taskIds">The ID's of the tasks to get.</param>
		public static IEnumerable<Task> GetMany(List<long> taskIds)
		{
			if (taskIds == null || taskIds.Count == 0)
			{
				return new List<Task>();
			}

			return SelectMany("SELECT * FROM `tasks` WHERE `id` IN(" + string.Join(", ", taskIds) + ") ORDER BY `date_added");
		}

		/// <summary>
		/// Gets the count of reminder tasks on or after the specified dateTimeAsOf.
		/// </summary>
		public static long GetCountReminderTasks(string reminderGroupId, DateTime dateTimeAsOf)
		{
			return Database.ExecuteLong(
				"SELECT COUNT(*) FROM task " +
				"WHERE task.ReminderGroupId='" + POut.String(reminderGroupId) + "' " +
				"AND DateTimeEntry > " + POut.DateT(dateTimeAsOf));
		}

		/// <summary>
		/// After a refresh, this is used to determine whether the Current user has received any new tasks through subscription.
		/// Must supply the current usernum.  If the listTaskNums is null, then all subscribed tasks for the user will be returned.
		/// The signal list will include any task changes including status changes and deletions.
		/// </summary>
		public static IEnumerable<Task> GetNewTasksThisUser(long? userId, List<long> taskIds = null)
		{
			if (!userId.HasValue)
			{
				return new List<Task>();
			}

			if (taskIds != null && taskIds.Count == 0)
			{
				return new List<Task>();
			}

			string command =
				"SELECT `tasks`.*, " +
					"CASE " +
						"WHEN(tu.`task_id` IS NOT NULL) THEN 1 " +
						"ELSE 0 " +
					"END IsUnread " +
				"FROM taskancestor " +
				"INNER JOIN `tasks` t ON (t.`id` = taskancestor.`task_id` AND t.`status` != " + (int)TaskStatus.Done + "";

			if (taskIds != null)
			{
				command += " AND t.`id` IN (" + string.Join(",", taskIds) + ")";
			}
			command += ") ";

			command +=
				"INNER JOIN `task_lists` ON `task_lists`.`id` = taskancestor.TaskListNum " +
				"INNER JOIN `task_subscriptions` ts ON (ts.`task_list_id` = task_lists.`id` ND ts.`user_id` = " + userId.Value + ") " +
				"LEFT JOIN `tasks_unread` tu ON (tu.`task_id` = t.`id` AND tu.`user_id` = " + userId.Value + ")";

			return SelectMany(command);
		}

		/// <summary>
		/// Gets a string using the aptNum as the key,
		/// String consits of patient name and some appointment information.
		/// </summary>
		public static Dictionary<long, string> GetAppointmentDescriptions(List<long> appointmentIds)
		{
			if (appointmentIds.Count == 0)
			{
				return new Dictionary<long, string>();
			}

			var dataTable = Database.ExecuteDataTable(
				"SELECT " +
					"patient.LName, patient.FName, patient.Preferred, patient.MiddleI, " +
					"appointment.AptNum, appointment.AptDateTime, appointment.ProcDescript, appointment.Note " +
				"FROM appointment " +
				"INNER JOIN patient ON patient.PatNum=appointment.PatNum " +
				"WHERE appointment.AptNum IN (" + string.Join(",", appointmentIds) + ")");

			var dictTaskString = new Dictionary<long, string>();
			foreach (DataRow dataRow in dataTable.Rows)
			{
				string patientName = Patients.GetNameLF(dataRow["LName"].ToString(), dataRow["FName"].ToString(), dataRow["Preferred"].ToString(), dataRow["MiddleI"].ToString()); //no call to db
                
				var appointment = new Appointment
                {
                    AptNum = PIn.Long(dataRow["AptNum"].ToString()),
                    AptDateTime = PIn.Date(dataRow["AptDateTime"].ToString()),
                    ProcDescript = PIn.String(dataRow["ProcDescript"].ToString()),
                    Note = dataRow["Note"].ToString()
                };

                dictTaskString.Add(appointment.AptNum, patientName + " " + appointment.AptDateTime.ToString() + " " + appointment.ProcDescript + " " + appointment.Note + " - ");
			}

			return dictTaskString;
		}

		/// <summary>
		/// Sets the task.ReminderGroupId to a brand new and unique value.
		/// </summary>
		public static void SetReminderGroupId(Task task)
		{
			task.ReminderGroupId = Guid.NewGuid().ToString();
		}

		public static IEnumerable<Task> GetByPatient(long patientId) 
			=> SelectMany(
				"SELECT * FROM `tasks` WHERE `patient_id` = " + patientId + " ORDER BY `date_added`");

		public static IEnumerable<Task> GetByTaskList(long taskListId, bool showDone) 
			=> SelectMany(
				"SELECT * FROM `tasks` " +
				"WHERE `task_list_id` = " + taskListId + (showDone ? "" : " AND (`status` != " + (int)TaskStatus.Done + " OR `date_completed` IS NULL)") + " " +
				"ORDER BY `date_added`");

		/// <summary>
		/// Must supply the supposedly unaltered oldTask.
		/// Will throw an exception if oldTask does not exactly match the database state.
		/// Keeps users from overwriting each other's changes.
		/// </summary>
		public static void Update(Task task, Task oldTask)
		{
			Validate(task, oldTask);

			if (task.Status != oldTask.Status && 
				task.Status == TaskStatus.Done && !string.IsNullOrEmpty(task.ReminderGroupId))
			{
				// A reminder task status was changed to Done.
				CopyReminderToNextDueDate(task);
			}

			UpdateInternal(task, oldTask);
		}

		/// <summary>
		/// Creates a copy of reminderTask with DateTimeEntry set to the next date due in the future.
		/// Ensures new copy is marked new.
		/// Returns the newly created task, or null if the new task could not be created.
		/// </summary>
		public static Task CopyReminderToNextDueDate(Task reminderTask)
		{
			//Do not copy reminder task if a copy already exists in the future.
			if (reminderTask.ReminderType == TaskReminderType.Once || 
				reminderTask.ReminderType.HasFlag(TaskReminderType.Daily | TaskReminderType.Weekly | TaskReminderType.Monthly | TaskReminderType.Yearly)
				&& reminderTask.Id > 0//and is existing reminder task,
				&& GetCountReminderTasks(reminderTask.ReminderGroupId, reminderTask.DateStart.Value) > 0)//with copies in the future
			{
				return null;
			}
			DateTime dateMin = DateTime.Today;
			if (reminderTask.DateStart.Value.Date > DateTime.Today)
			{
				dateMin = reminderTask.DateStart.Value.Date;
			}
			Task taskNext = reminderTask.Copy();//This is where taskNext.DateTimeEntry is initially set.
			taskNext.Id = 0;//This causes a new PK to be created for the new task.
			taskNext.Status = TaskStatus.New;
			taskNext.DateCompleted = DateTime.MinValue;
			if (reminderTask.ReminderType.HasFlag(TaskReminderType.Daily))
			{
				//Find the first day in the schedule which is also in the future.
				while (taskNext.DateStart.Value.Date <= dateMin)
				{
					taskNext.DateStart = taskNext.DateStart.Value.AddDays(taskNext.ReminderFrequency);
				}
			}
			else if (reminderTask.ReminderType.HasFlag(TaskReminderType.Weekly))
			{
				//Find the first day in the schedule which is also in the future.
				while (taskNext.DateStart.Value.Date <= dateMin || !IsWeekDayFound(taskNext.DateStart.Value, taskNext.ReminderType))
				{
					if (taskNext.DateStart.Value.DayOfWeek == DayOfWeek.Sunday)
					{
						taskNext.DateStart = taskNext.DateStart.Value.AddDays(-6 + 7 * taskNext.ReminderFrequency);//Increment to monday of next week in schedule.
					}
					else
					{
						taskNext.DateStart = taskNext.DateStart.Value.AddDays(1);
					}
				}
			}
			else if (reminderTask.ReminderType.HasFlag(TaskReminderType.Monthly))
			{
				//Find the first day in the schedule which is also in the future.
				while (taskNext.DateStart.Value.Date <= dateMin)
				{
					DateTime dtMonthStart = new DateTime(taskNext.DateStart.Value.Year, taskNext.DateStart.Value.Month, 1);
					DateTime dtMonthNext = dtMonthStart.AddMonths(taskNext.ReminderFrequency);
					int dayNext = Math.Min(taskNext.DateStart.Value.Day, DateTime.DaysInMonth(dtMonthNext.Year, dtMonthNext.Month));
					taskNext.DateStart = dtMonthNext.AddDays(dayNext - 1).AddTicks(taskNext.DateStart.Value.TimeOfDay.Ticks);//-1 day since already on 1st.
				}
			}
			else if (reminderTask.ReminderType.HasFlag(TaskReminderType.Yearly))
			{
				//Find the first day in the schedule which is also in the future.
				while (taskNext.DateStart.Value.Date <= dateMin)
				{
					//We use the following algorithm to handle the edge case when the task was created on 02/29 of a leap year.
					//For this case, the task should be copied to 02/28 in a future year, unless that future year is also a leap year.
					DateTime dtYearMonthStart = new DateTime(taskNext.DateStart.Value.Year, taskNext.DateStart.Value.Month, 1);
					DateTime dtYearMonthNext = dtYearMonthStart.AddYears(taskNext.ReminderFrequency);
					int dayNext = Math.Min(taskNext.DateStart.Value.Day, DateTime.DaysInMonth(dtYearMonthNext.Year, dtYearMonthNext.Month));
					taskNext.DateStart = dtYearMonthNext.AddDays(dayNext - 1).AddTicks(taskNext.DateStart.Value.TimeOfDay.Ticks);//-1 day since already on 1st.
				}
			}
			long newTaskNum = Tasks.Insert(taskNext);
			//If we could we'd just call DataValid.SetInvalidTask(newTaskNum,true); but we're in ODBuisness so we'll do what we can to emulate it
			TaskUnreads.AddUnreads(taskNext, Security.CurrentUser.Id);  //We need the new copy to marked as unread for everyone for when it is "due"
																		//Here we do our best to follow the signal logic in OpenDental namespace.  This may be unneccessary because the copied task isn't due 
																		//for at least a day.  There will already be one signal for the old task being marked due, this is just for the copied task.
			Signalods.SetInvalid(InvalidType.TaskPopup, KeyType.Task, newTaskNum);
			return taskNext;
		}

		/// <summary>
		/// Returns true if the dateTimeEntry is on a day of the week specified by the day schedule inside reminderType.
		/// </summary>
		private static bool IsWeekDayFound(DateTime dateTimeEntry, TaskReminderType reminderType)
		{
			if (dateTimeEntry.Date.DayOfWeek == DayOfWeek.Monday && reminderType.HasFlag(TaskReminderType.Monday))
			{
				return true;
			}
			if (dateTimeEntry.Date.DayOfWeek == DayOfWeek.Tuesday && reminderType.HasFlag(TaskReminderType.Tuesday))
			{
				return true;
			}
			if (dateTimeEntry.Date.DayOfWeek == DayOfWeek.Wednesday && reminderType.HasFlag(TaskReminderType.Wednesday))
			{
				return true;
			}
			if (dateTimeEntry.Date.DayOfWeek == DayOfWeek.Thursday && reminderType.HasFlag(TaskReminderType.Thursday))
			{
				return true;
			}
			if (dateTimeEntry.Date.DayOfWeek == DayOfWeek.Friday && reminderType.HasFlag(TaskReminderType.Friday))
			{
				return true;
			}
			if (dateTimeEntry.Date.DayOfWeek == DayOfWeek.Saturday && reminderType.HasFlag(TaskReminderType.Saturday))
			{
				return true;
			}
			if (dateTimeEntry.Date.DayOfWeek == DayOfWeek.Sunday && reminderType.HasFlag(TaskReminderType.Sunday))
			{
				return true;
			}
			return false;
		}

		public static void Validate(Task task, Task oldTask)
		{
			if (task.Repeat && task.RepeatDate.HasValue)
				throw new Exception("Task cannot be tagged repeating and also have a date.");

			if (task.Repeat && task.Status != TaskStatus.New)
				throw new Exception("Tasks that are repeating must have a status of New.");

			if (task.Repeat && task.TaskListId != 0 && task.RepeatInterval != TaskRepeatInterval.Never)
				throw new Exception("In repeating tasks, only the main parents can have a task status.");

			if (WasTaskAltered(oldTask))
				throw new Exception("Not allowed to save changes because the task has been altered by someone else.");

			if (task.Id == 0)
			{
				TaskEditCreateLog("New task added.", task);
			}
			else
			{
				if (task.Status != oldTask.Status)
				{
					if (task.Status == TaskStatus.Done)
					{
						TaskEditCreateLog("Task marked as completed.", task);
					}
					if (task.Status == TaskStatus.New)
					{
						TaskEditCreateLog("Task marked as new.", task);
					}
				}

				if (task.Description != oldTask.Description)
				{
					TaskEditCreateLog("Task description edited.", task);
				}

				if (task.UserId != oldTask.UserId)
				{
					TaskEditCreateLog($"Changed user from {Userods.GetName(oldTask.UserId)}.", task);
				}

				static string GetPatientNameForLog(long patientId)
                {
					var patient = Patients.GetLim(patientId);
					if (patient != null)
					{
						return $"{patient.GetNameFL()} [{patientId}]";
					}

					return "(deleted)";
                }

				static string GetAppointmentDescriptionForLog(long appointmentId)
                {
					var appointment = Appointments.GetOneApt(appointmentId);
					if (appointment != null)
                    {
						return $"for {GetPatientNameForLog(appointment.PatNum)}";
                    }

					return "(deleted)";
                }

				if (task.PatientId != oldTask.PatientId)
                {
					if (task.PatientId.HasValue)
                    {
						if (oldTask.PatientId.HasValue)
                        {
							TaskEditCreateLog(
								$"Changed attached patient {GetPatientNameForLog(oldTask.PatientId.Value)} " +
								$"to {GetPatientNameForLog(task.PatientId.Value)}.", task);

						}
						else
						{
							TaskEditCreateLog(
								$"Attached patient {GetPatientNameForLog(task.PatientId.Value)} to task.", task);
						}
					}
                    else
                    {
						TaskEditCreateLog(
							$"Detached patient {GetPatientNameForLog(oldTask.PatientId.Value)} from task.", task);
                    }
                }

				if (task.AppointmentId != oldTask.AppointmentId)
                {
					if (task.AppointmentId.HasValue)
                    {
						if (oldTask.AppointmentId.HasValue)
                        {
							TaskEditCreateLog(
								$"Changed attached appointment {GetAppointmentDescriptionForLog(oldTask.AppointmentId.Value)} " +
								$"to appointment {GetAppointmentDescriptionForLog(task.AppointmentId.Value)}.", task);
						}
                        else
                        {
							TaskEditCreateLog(
								$"Attached appointment {GetAppointmentDescriptionForLog(task.AppointmentId.Value)} to task.", task);
						}
                    }
                    else
                    {
						TaskEditCreateLog(
							$"Detached appointment {GetAppointmentDescriptionForLog(oldTask.AppointmentId.Value)} from task.", task);
					}
                }

				if (task.TaskListId != oldTask.TaskListId)
				{
					var oldTaskList = TaskLists.GetOne(oldTask.TaskListId) ?? new TaskList()
					{
						Description = "(deleted)"
					};

					TaskEditCreateLog("Task moved from " + oldTaskList.Description, task);
				}
			}
		}

		public static long Insert(Task task)
		{
			if (task.Repeat && task.RepeatDate.HasValue)
				throw new Exception("Task cannot be tagged repeating and also have a date.");

			if (task.Repeat && task.Status != TaskStatus.New)
				throw new Exception("Tasks that are repeating must have a status of New.");

			if (task.Repeat && task.TaskListId != 0 && task.RepeatInterval != TaskRepeatInterval.Never)
				throw new Exception("In repeating tasks, only the main parents can have a task status.");

			InsertInternal(task);

			return task.Id;
		}

		public static bool WasTaskAltered(Task task)
		{
			var oldtask = SelectOne(task.Id);

			return 
				oldtask == null ||
				oldtask.RepeatDate != task.RepeatDate ||
				oldtask.RepeatInterval != task.RepeatInterval ||
				oldtask.Description != task.Description ||
				oldtask.RepeatTaskId != task.RepeatTaskId ||
				oldtask.Repeat != task.Repeat ||
				oldtask.PatientId != task.PatientId ||
				oldtask.AppointmentId != task.AppointmentId ||
				oldtask.TaskListId != task.TaskListId ||
				oldtask.Status != task.Status ||
				oldtask.UserId != task.UserId ||
				oldtask.DateStart != task.DateStart ||
				oldtask.DateCompleted != task.DateCompleted;
		}

		public static void TaskEditCreateLog(string logText, Task task)
		{
			TaskEditCreateLog(Permissions.TaskEdit, logText, task);
		}

		/// <summary>
		/// Makes audit trail entry for the task passed in.
		/// If this task has an object type set, the log will show up under the corresponding patient for the selected object type.
		/// Used for both TaskEdit and TaskNoteEdit permissions.
		/// </summary>
		public static void TaskEditCreateLog(Permissions perm, string logText, Task task)
		{
			if (task == null) return;

			long patientId = 0;
			if (task.PatientId.HasValue)
            {
				patientId = task.PatientId.Value;
            }
			else if (task.AppointmentId.HasValue)
            {
				var appointment = Appointments.GetOneApt(task.AppointmentId.Value);
				if (appointment != null)
                {
					patientId = appointment.PatNum;
                }
            }

			SecurityLogs.MakeLogEntry(perm, patientId, logText, task.Id, DateTime.MinValue);
		}
	}
}
