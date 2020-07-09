using System;
using System.Security.Cryptography;

namespace OpenDentBusiness
{
	/// <summary>
	/// A group of users.
	/// Security permissions are determined by the usergroup of a user.
	/// </summary>
	[Serializable]
	public class UserGroup : TableBase
	{
		[CrudColumn(IsPriKey = true)]
		public long UserGroupNum;

		public string Description;

		/// <summary>
		/// FK to usergroup.UserGroupNum.
		/// The user group num within the Central Manager database.
		/// Only editable via CEMT.  Can change when CEMT syncs.
		/// </summary>
		public long UserGroupNumCEMT;

		public UserGroup Copy()
		{
			UserGroup u = new UserGroup();
			u.UserGroupNum = UserGroupNum;
			u.Description = Description;
			u.UserGroupNumCEMT = UserGroupNumCEMT;
			return u;
		}

		public override string ToString() => Description ?? base.ToString();
    }
}
