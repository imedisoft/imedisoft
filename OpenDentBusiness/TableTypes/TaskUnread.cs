using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;

namespace OpenDentBusiness
{
    /// <summary>
    /// When a task is created or a comment made, a series of these taskunread objects are created, one for each user who is subscribed to the tasklist.
    /// Duplicates are intelligently avoided.
    /// Rows are deleted once user reads the task.
    /// </summary>
    [Table]
	public class TaskUnread : TableBase
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(Task), nameof(Task.Id))]
		public long TaskId;

		[ForeignKey(typeof(User), nameof(User.Id))]
		public long UserId;
	}
}
