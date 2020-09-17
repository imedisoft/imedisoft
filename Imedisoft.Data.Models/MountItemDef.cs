using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table("mount_item_defs")]
	public class MountItemDef
	{
		[PrimaryKey]
		public long Id;

		public long MountDefId;
		public int X;
		public int Y;
		public int Width;
		public int Height;
		public int SortOrder;
	}
}
