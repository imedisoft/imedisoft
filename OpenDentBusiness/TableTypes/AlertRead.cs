using Imedisoft.Data.Annotations;
using System;

namespace OpenDentBusiness
{
    [Table]
	[CrudTable(IsSynchable = true)]
	public class AlertRead : TableBase
	{
		[PrimaryKey]
		public long Id;

		[Column("AlertItemNum"), ForeignKey(typeof(AlertItem), nameof(AlertItem.Id))]
		public long AlertItemId;

		[Column("UserNum"), ForeignKey(typeof(Userod), nameof(Userod.Id))]
		public long UserId;

		public AlertRead()
		{
		}

		public AlertRead(long alertItemId, long userId)
		{
			AlertItemId = alertItemId;
			UserId = userId;
		}
	}
}
