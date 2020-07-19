using Imedisoft.Data.Annotations;
using System;

namespace OpenDentBusiness
{
	/// <summary>
	/// Subscribes a user and optional clinic to specifc alert types.
	/// Users will not get alerts unless they have an entry in this table.
	/// </summary>
	[Table]
	[CrudTable(IsSynchable = true)]
	public class AlertSub : TableBase
	{
		[PrimaryKey]
		public long AlertSubNum;

		/// <summary>
		/// FK to userod.UserNum.
		/// </summary>
		public long UserNum;

		/// <summary>
		/// FK to clinic.ClinicNum. Can be 0.
		/// </summary>
		public long ClinicNum;

		/// <summary>
		/// Deprecated.
		/// </summary>
		public AlertType Type;

		/// <summary>
		/// FK to alertcategory.AlertCategoryNum.
		/// </summary>
		public long AlertCategoryNum;

		public AlertSub()
		{
		}

		public AlertSub(long userNum, long clinicNum, long alertCatNum)
		{
			UserNum = userNum;
			ClinicNum = clinicNum;
			AlertCategoryNum = alertCatNum;
		}

		public AlertSub Copy() => (AlertSub)MemberwiseClone();
	}
}
