using Imedisoft.Data.Annotations;
using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// A tasklist is like a folder system, where it can have child tasklists as well as tasks.
    /// </summary>
    [Table]
	public class TaskList
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// A description of the task list.
		/// </summary>
		public string Description;

		/// <summary>
		/// FK to tasklist.TaskListNum  The parent task list to which this task list is assigned.
		/// If zero, then this task list is on the main trunk of one of the sections.
		/// </summary>
		[ForeignKey(typeof(TaskList), nameof(Id))]
		public long? ParentId;

		/// <summary>
		/// The date and time on which the list was added.
		/// </summary>
		public DateTime DateAdded;
		
		/// <summary>
		/// The description of the parent list.
		/// </summary>
		[Ignore]
		public string ParentDesc;

		/// <summary>
		/// The number of new tasks found in the list.
		/// </summary>
		[Ignore]
		public int NewTaskCount;

		/// <summary>
		///		<para>
		///			The status of the list.
		///		</para>
		///		<para>
		///			Lists with status <see cref="TaskListStatus.Archived"/> are hidden by default.
		///		</para>
		/// </summary>
		public TaskListStatus Status;

		public TaskList Copy() => (TaskList)MemberwiseClone();

		/// <summary>
		/// Returns a string representation of the task list.
		/// </summary>
		public override string ToString() => Description;
    }

	/// <summary>
	/// Identifies the status of a <see cref="TaskList"/>.
	/// </summary>
	public enum TaskListStatus
	{
		Active = 0,
		Archived,
	}
}
