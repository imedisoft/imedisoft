using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table("auto_notes")]
	public class AutoNote
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(AutoNoteCategory), nameof(AutoNoteCategory.Id))]
		public long? AutoNoteCategoryId;

		public string Name;

		public string Content;
	}
}
