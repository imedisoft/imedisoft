using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using System;

namespace OpenDentBusiness
{
	/// <summary>
	/// Subscribes a user and optional clinic to specifc alert types.
	/// Users will not get alerts unless they have an entry in this table.
	/// </summary>
	[Table("alert_subs")]
	public class AlertSub : TableBase
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(User), nameof(User.Id))]
		public long UserId;

		[ForeignKey(typeof(Clinic), nameof(Clinic.Id))]
		public long? ClinicId;

		[ForeignKey(typeof(AlertCategory), nameof(AlertCategory.Id))]
		public long AlertCategoryId;

		public AlertSub()
		{
		}

		public AlertSub(long userId, long? clinicId, long alertCategoryId)
		{
			UserId = userId;
			ClinicId = clinicId;
			AlertCategoryId = alertCategoryId;
		}

		public AlertSub Copy() => (AlertSub)MemberwiseClone();
	}
}
