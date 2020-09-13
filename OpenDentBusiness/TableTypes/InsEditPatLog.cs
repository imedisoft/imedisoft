using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness
{
    [Table]
	public class InsEditPatLog : TableBase
	{
		[PrimaryKey]
		public long InsEditPatLogNum;

		/// <summary>
		/// Key to the foreign table.
		/// </summary>
		public long FKey;

		/// <summary>
		/// Enum:InsEditPatLogType 0 - PatPlan, 1 - Subscriber, 2 - Adjustment.
		/// </summary>
		public InsEditPatLogType LogType;

		/// <summary>
		/// The name of the column that was altered.
		/// </summary>
		public string FieldName;

		/// <summary>
		/// The old value of this field.
		/// </summary>
		public string OldValue;

		/// <summary>
		/// The new value of this field.
		/// </summary>
		public string NewValue;

		/// <summary>
		/// The user that made this change.
		/// </summary>
		[ForeignKey(typeof(User), nameof(User.Id))]
		public long UserNum;

		/// <summary>
		/// Time that the row was inserted into the DB.
		/// </summary>
		public DateTime DateTStamp;

		/// <summary>
		/// Stores the key to the parent table.
		/// 0 - PatPlan: patplan.PatNum
		/// 1 - Subscriber: inssub.Subscriber
		/// 2 - Adjustment: claimproc.InsSubNum
		/// </summary>
		public long ParentKey;

		/// <summary>
		/// The string describing this entry. Displays different information depending on the LogType:
		/// 1 - Subscriber: Subscriber's Name
		/// 2 - Adjustment: Insurance Benefit
		/// </summary>
		public string Description;
	}

	public enum InsEditPatLogType
	{
		PatPlan,

		Subscriber,

		/// <summary>
		/// Adjustments to insurance benefits.
		/// </summary>
		Adjustment,
	}
}
