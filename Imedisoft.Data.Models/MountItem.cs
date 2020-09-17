using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table("mount_items")]
	public class MountItem
	{
		[PrimaryKey]
		public long Id;

		public long MountId;
		public int X;
		public int Y;
		public int Width;
		public int Height;
		public int SortOrder;
	}
}
