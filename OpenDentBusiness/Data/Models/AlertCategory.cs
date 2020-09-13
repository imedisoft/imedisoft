using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
    [Table("alert_categories")]
	public class AlertCategory
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// False by default, indicates that this is a row that can not be edited or deleted.
		/// </summary>
		public bool IsHqCategory;

		/// <summary>
		/// Name used by HQ to identify the type of alert category this started as, allows us to associate new alerts.
		/// </summary>
		public string InternalName;

		/// <summary>
		/// Name displayed to user when subscribing to alerts categories.
		/// </summary>
		public string Description;

		public AlertCategory Copy() => (AlertCategory)MemberwiseClone();
	}
}
