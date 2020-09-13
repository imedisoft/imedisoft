using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;

namespace OpenDentBusiness
{
    /// <summary>
    /// Allows multiple groups to be attached to a user. 
    /// Security permissions are determined by the usergroups of a user.
    /// </summary>
    [Table]
	public class UserGroupAttach : TableBase
	{
		[PrimaryKey]
		public long UserGroupAttachNum;

		[ForeignKey(typeof(User), nameof(User.Id))]
		public long UserId;

		[ForeignKey(typeof(UserGroup), nameof(UserGroup.Id))]
		public long UserGroupId;

		public UserGroupAttach Copy() 
			=> (UserGroupAttach)MemberwiseClone();
	}
}
