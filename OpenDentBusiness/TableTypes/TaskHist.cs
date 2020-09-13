using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness
{
    /// <summary>
    ///		<para>
    ///			Represents a history record of a task.
    ///		</para>
    ///		<para>
    ///			Everytime a task is modified, a history record is created that represents the state 
    ///			of the task before the modifications are applied. This enables us to show a full
    ///			and acurate history of any given task.
    ///		</para>
    ///		<para>
    ///			At present, this is only used for auditing purposes. In the future we may use this 
    ///			to extend task functionality with a rollback feature so that tasks can be reverted 
	///			to any previous state.
    ///		</para>
    /// </summary>
    [Table("task_history")]
	public class TaskHistory
	{
		[PrimaryKey]
		public long Id;
		
		/// <summary>
		/// The ID of the user that modified the task from this state.
		/// </summary>
		[ForeignKey(typeof(User), nameof(User.Id))]
		public long HistoryUserId;
		
		/// <summary>
		/// The date and time that this task was edited and added to the Hist table. This value will not be updated by MySQL whenever the row changes.
		/// </summary>
		public DateTime HistoryDate;

		/// <summary>
		/// The ID of the task this history record is attached to.
		/// </summary>
		[ForeignKey(typeof(Task), nameof(Task.Id))]
		public long TaskId;

		/// <summary>
		/// The ID of the task list the task is attached to.
		/// </summary>
		[ForeignKey(typeof(TaskList), nameof(TaskList.Id))]
		public long TaskListId;

		/// <summary>
		/// The description of the task.
		/// </summary>
		public string Description = "";

		/// <summary>
		/// A value indicating whether the task should repeat.
		/// </summary>
		public bool Repeat;

		/// <summary>
		/// The interval after which the task should repeat.
		/// </summary>
		public TaskRepeatInterval RepeatInterval = TaskRepeatInterval.Never;

		/// <summary>
		/// The date on which the task repeats.
		/// </summary>
		public DateTime? RepeatDate;

		/// <summary>
		/// The ID of the repeating task from which this task was derived.
		/// </summary>
		[ForeignKey(typeof(Task), nameof(Id))]
		public long? RepeatSourceTaskId;

		/// <summary>
		/// The (optional) ID of the patient that is attached to the task.
		/// </summary>
		public long? PatientId;

		/// <summary>
		/// The (optional) ID of the appointment that is attached to the task.
		/// </summary>
		public long? AppointmentId;

		/// <summary>
		/// The date and time that this task was added.  User editable.
		/// For reminder tasks, this field is used to indicate the date and time the reminder will take effect.
		/// </summary>
		public DateTime? DateStart;

		/// <summary>
		/// The ID of the user that createed the task.
		/// </summary>
		[ForeignKey(typeof(User), nameof(User.Id))]
		public long UserId;

		/// <summary>
		/// The ID of the task priority definition.
		/// </summary>
		[ForeignKey(typeof(Definition), nameof(Definition.Id))]
		public long PriorityId;

		public string ReminderGroupId;
		public TaskReminderType ReminderType;
		public int ReminderFrequency;

		/// <summary>
		/// The date and time on which the task was completed (e.g. marked as 'Done').
		/// </summary>
		public DateTime? DateCompleted;

		/// <summary>
		/// The date and time on which the task was created.
		/// </summary>
		public DateTime DateAdded;

		/// <summary>
		/// The date and time on which the task was created or last modified.
		/// </summary>
		public DateTime DateModified;

		/// <summary>
		/// The status of the task.
		/// </summary>
		public TaskStatus Status;

		/// <summary>
		/// Pass in the old task that needs to be recorded.
		/// </summary>
		public TaskHistory(Task task)
		{
			HistoryDate = DateTime.UtcNow;
			HistoryUserId = Security.CurrentUser.Id;
			TaskId = task.Id;
			TaskListId = task.TaskListId;
			Description = task.Description;
			Repeat = task.Repeat;
			RepeatInterval = task.RepeatInterval;
			RepeatDate = task.RepeatDate;
			RepeatSourceTaskId = task.RepeatTaskId;
			PatientId = task.PatientId;
			AppointmentId = task.AppointmentId;
			UserId = task.UserId;
			PriorityId = task.PriorityId;
			ReminderGroupId = task.ReminderGroupId;
			ReminderType = task.ReminderType;
			ReminderFrequency = task.ReminderFrequency;
			DateAdded = task.DateAdded;
			DateStart = task.DateStart;
			DateCompleted = task.DateCompleted;
			Status = task.Status;
		}

		public TaskHistory()
		{
		}
	}
}
