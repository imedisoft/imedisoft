using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	/// <summary>These are always attached to a mount. Like a mount, they cannot be edited.  Documents are attached to each MountItem using Document.MountItemNum field.  Image will always be cropped to make it look smaller or bigger if it doesn't exactly match the mount item rectangle ratio.</summary>
	[Serializable()]
	public class MountItem : TableBase {
		/// <summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long MountItemNum ;
		/// <summary>FK to mount.MountNum.</summary>
		public long MountNum;
		/// <summary>The x position, in pixels, of the item on the mount.</summary>
		public int Xpos;
		/// <summary>The y position, in pixels, of the item on the mount.</summary>
		public int Ypos;
		/// <summary>The order of the item on the mount.</summary>
		public int ItemOrder;
		/// <summary>The width, in pixels, of the mount item rectangle.</summary>
		public int Width;
		/// <summary>The height, in pixels, of the mount item rectangle.</summary>
		public int Height;

		///<summary></summary>
		public MountItem Copy() {
			return (MountItem)this.MemberwiseClone();
		}

		public override string ToString(){
			return "ItemOrder:"+ItemOrder.ToString();
		}

	}
}
