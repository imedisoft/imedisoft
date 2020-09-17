using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table("alert_category_links")]
	public class AlertCategoryLink
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(AlertCategory), nameof(AlertCategory.Id))]
		public long AlertCategoryId;

		public string Type;

		public AlertCategoryLink()
		{
		}

		public AlertCategoryLink(long alertCategoryNum, string alertType)
		{
			AlertCategoryId = alertCategoryNum;
			Type = alertType;
		}

		public AlertCategoryLink Copy() => (AlertCategoryLink)MemberwiseClone();
	}
}
