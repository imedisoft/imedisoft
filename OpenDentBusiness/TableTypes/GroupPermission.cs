using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using System;

namespace OpenDentBusiness
{

    /// <summary>
    /// Every user group has certain permissions. This defines a permission for a group. 
    /// The absense of permission would cause that row to be deleted from this table.
    /// </summary>
    [Table("group_permissions")]
	public class GroupPermission : TableBase
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The user group for which this permission is granted. If not authorized, then this groupPermission will have been deleted.
		/// </summary>
		[ForeignKey(typeof(UserGroup), nameof(UserGroup.Id))]
		public long UserGroupId;

		/// <summary>
		///		<para>
		///			Indicates the date from which this permission takes effect. If null the permission is always granted.
		///		</para>
		/// </summary>
		public DateTime? NewerDate;

		/// <summary>
		///		<para>
		///			The number of days for which the permission is active.
		///		</para>
		/// </summary>
		public int NewerDays;

		/// <summary>
		/// The permission.
		/// </summary>
		public Permissions Permission;

		/// <summary>
		/// The ID of the object this permission grants authorization for.
		/// </summary>
		public long? ObjectId;

		public GroupPermission Copy() => (GroupPermission)MemberwiseClone();
	}
}
