using Imedisoft.Data.Annotations;
using System;

namespace OpenDentBusiness
{
    [Table]
	public class AlertCategoryLink : TableBase
	{
		[PrimaryKey]
		public long AlertCategoryLinkNum;

		/// <summary>
		/// FK to AlertCategory.AlertCategoryNum.
		/// </summary>
		public long AlertCategoryNum;

		/// <summary>
		/// Enum:AlertType Identifies what types of alert this row is assocaited to.
		/// </summary>
		public AlertType AlertType;

		public AlertCategoryLink()
		{
		}

		public AlertCategoryLink(long alertCategoryNum, AlertType alertType)
		{
			AlertCategoryNum = alertCategoryNum;
			AlertType = alertType;
		}

		public AlertCategoryLink Copy() => (AlertCategoryLink)MemberwiseClone();
	}
}
