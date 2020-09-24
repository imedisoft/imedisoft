using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using System;

namespace OpenDentBusiness
{
    [Table("task_notes")]
	public class TaskNote
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The ID of the task the note is attached to.
		/// </summary>
		[ForeignKey(typeof(Task), nameof(Task.Id))]
		public long TaskId;

		/// <summary>
		/// The ID of the user that created the note.
		/// </summary>
		[ForeignKey(typeof(User), nameof(User.Id))]
		public long UserId;

		/// <summary>
		/// The note.
		/// </summary>
		public string Note = "";

		/// <summary>
		/// The date and time on which the note was created or last modified.
		/// </summary>
		public DateTime DateModified;
	}
}
