using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table("user_groups")]
	public class UserGroup
	{
		[PrimaryKey]
		public long Id;

		public string Description;

		/// <summary>
		///		<para>
		///			The ID of the user group within the Central Manager database.
		///		</para>
		///		<para>
		///			Only editable via Central Enterprise Management Tool (CEMT).
		///		</para>
		/// </summary>
		public long? CentralUserGroupId;

		/// <summary>
		/// Returns a string representation of the user group.
		/// </summary>
		public override string ToString() => Description;
    }
}
