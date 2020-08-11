using Imedisoft.Data.Annotations;
using System;
using System.Collections;

namespace OpenDentBusiness
{
    /// <summary>
    /// Represents one ancestor of one task.
    /// Each task will have at least one ancestor unless it is directly on a main trunk.
    /// An ancestor is defined as a tasklist that is higher in the heirarchy for the task, regardless of how many levels up it is.
    /// This allows us to mark task lists as having "new" tasks, and it allows us to quickly check for new tasks for a user on startup.
    /// </summary>
    [Table]
	public class TaskAncestor : TableBase
	{
		[PrimaryKey]
		public long TaskAncestorNum;

		[ForeignKey(typeof(Task), nameof(Task.Id))]
		public long TaskNum;

		[ForeignKey(typeof(TaskList), nameof(TaskList.Id))]
		public long TaskListNum;
	}
}
