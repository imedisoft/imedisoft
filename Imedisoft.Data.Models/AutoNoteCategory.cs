using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table("auto_note_categories")]
	public class AutoNoteCategory
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(AutoNoteCategory), nameof(AutoNoteCategory.Id))]
		public long? ParentId;

		public string Description;
	}
}
