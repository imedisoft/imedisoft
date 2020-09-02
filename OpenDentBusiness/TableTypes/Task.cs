using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using System;
using System.Collections;
using System.Data;

namespace OpenDentBusiness
{
    /// <summary>
    /// A task is a single todo item.
    /// </summary>
    [Table("tasks")]
	[CrudTable(AuditPerms = CrudAuditPerm.TaskNoteEdit)]
	public class Task
	{
		[PrimaryKey]
		public long Id;

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

		[ForeignKey(typeof(Task), nameof(Id))]
		public long? RepeatTaskId;

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
		[ForeignKey(typeof(Userod), nameof(Userod.Id))]
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
		/// Only used when tracking unread status by user instead of by task.
		/// This gets set to true to indicate it has not yet been read.
		/// </summary>
		[Ignore]
		public bool IsUnread;

		/// <summary>
		/// Not a database column.
		/// Attached patient's name (NameLF) if there is an attached patient.
		/// </summary>
		[Ignore]
		public string PatientName;

		public Task Copy() => (Task)MemberwiseClone();

		public override bool Equals(object obj) 
			=> obj is Task other &&
				Id == other.Id &&
				TaskListId == other.TaskListId &&
				RepeatDate == other.RepeatDate &&
				Description == other.Description &&
				Status == other.Status &&
				Repeat == other.Repeat &&
				RepeatInterval == other.RepeatInterval &&
				RepeatTaskId == other.RepeatTaskId &&
				PatientId == other.PatientId &&
				AppointmentId == other.AppointmentId &&
				DateStart == other.DateStart &&
				UserId == other.UserId &&
				DateCompleted == other.DateCompleted &&
				PriorityId == other.PriorityId &&
				ReminderGroupId == other.ReminderGroupId &&
				ReminderType == other.ReminderType &&
				ReminderFrequency == other.ReminderFrequency;

		public override int GetHashCode() => base.GetHashCode();
	}

	/// <summary>
	/// Identifies the interval at which a <see cref="Task"/> should repeat.
	/// </summary>
	public enum TaskRepeatInterval
	{
		/// <summary>
		/// The task never repeats.
		/// </summary>
		Never,

		/// <summary>
		/// The task repeats once on a fixed date.
		/// </summary>
		Once,

		/// <summary>
		/// The task repeats daily.
		/// </summary>
		Daily,

		/// <summary>
		/// The task repeats weekly.
		/// </summary>
		Weekly,

		/// <summary>
		/// The task repeats monthly.
		/// </summary>
		Monthly,
	}


	[Flags]
	public enum TaskReminderType
	{
		None = 0,
		Daily = 1,
		Weekly = 2,
		Monthly = 4,
		Yearly = 8,

		/// <summary>Use in combination with Weekly.</summary>
		Monday = 16,

		/// <summary>Use in combination with Weekly.</summary>
		Tuesday = 32,

		/// <summary>Use in combination with Weekly.</summary>
		Wednesday = 64,

		/// <summary>Use in combination with Weekly.</summary>
		Thursday = 128,

		/// <summary>Use in combination with Weekly.</summary>
		Friday = 256,

		/// <summary>Use in combination with Weekly.</summary>
		Saturday = 512,

		/// <summary>Use in combination with Weekly.</summary>
		Sunday = 1024,

		/// <summary>Specific date for a reminder.</summary>
		Once = 2048,
	}

	public enum TaskStatus
	{
		New,
		Done
	}

	public enum TaskType
	{
		/// <summary>
		/// All task types.
		/// </summary>
		All = 0,

		/// <summary>
		/// Reminder tasks only.
		/// </summary>
		Reminder = 1,

		/// <summary>
		/// Regular tasks and repeating tasks.
		/// </summary>
		Normal = 2,
	}
}
