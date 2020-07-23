using Imedisoft.Data.Annotations;
using System;
using System.Security.Cryptography;

namespace OpenDentBusiness
{
    /// <summary>
    /// A group of users.
    /// Security permissions are determined by the usergroup of a user.
    /// </summary>
    [Table("user_groups")]
	public class UserGroup : TableBase
	{
		[PrimaryKey]
		public long Id;

		public string Description;

		/// <summary>
		/// The user group num within the Central Manager database.
		/// Only editable via CEMT. Can change when CEMT syncs.
		/// </summary>
		public long CentralUserGroupId;

		public UserGroup Copy() 
			=> (UserGroup)MemberwiseClone();

		public override string ToString() 
			=> Description ?? base.ToString();
    }
}
