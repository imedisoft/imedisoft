using Imedisoft.Data.Annotations;
using System.Drawing;

namespace Imedisoft.Data.Models
{
    [Table("mount_defs")]
	public class MountDef
	{
		[PrimaryKey]
		public long Id;

		public string Description;

		/// <summary>
		/// The width of the mount, in pixels.
		/// </summary>
		public int Width;

		/// <summary>
		/// The height of the mount, in pixels.
		/// </summary>
		public int Height;

		/// <summary>
		/// Color of the mount background. Typically white for photos and black for radiographs.
		/// </summary>
		public Color BackColor;

		public int SortOrder;

		public MountDef Copy() => (MountDef)MemberwiseClone();
	}
}
