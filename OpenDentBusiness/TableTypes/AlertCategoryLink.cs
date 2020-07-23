using Imedisoft.Data.Annotations;
using System;

namespace OpenDentBusiness
{
    [Table]
	public class AlertCategoryLink : TableBase
	{
		[PrimaryKey]
		public long Id;

		[Column("AlertCategoryNum"), ForeignKey(typeof(AlertCategory), nameof(AlertCategory.Id))]
		public long AlertCategoryId;

		/// <summary>
		/// Identifies what types of alert this row is associated with.
		/// </summary>
		public AlertType AlertType;

		public AlertCategoryLink()
		{
		}

		public AlertCategoryLink(long alertCategoryNum, AlertType alertType)
		{
			AlertCategoryId = alertCategoryNum;
			AlertType = alertType;
		}

		public AlertCategoryLink Copy() => (AlertCategoryLink)MemberwiseClone();
	}
}
