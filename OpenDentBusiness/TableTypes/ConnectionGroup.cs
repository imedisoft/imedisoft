using System;

namespace OpenDentBusiness
{
	/// <summary>
	/// Used in the Central Enterprise Management Tool for creating a group of connections.
	/// </summary>
	[CrudTable(IsSynchable = true)]
	public class ConnectionGroup : TableBase
	{
		[CrudColumn(IsPriKey = true)]
		public long ConnectionGroupNum;

		/// <summary>
		/// Description of the connection group
		/// </summary>
		public string Description;

		public override string ToString() => Description;
    }
}