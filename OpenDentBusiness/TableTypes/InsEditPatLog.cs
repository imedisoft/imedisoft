using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness {

	///<summary></summary>
	[Serializable()]
	[CrudTable(HasBatchWriteMethods=true,IsLargeTable=true)]
	public class InsEditPatLog:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long InsEditPatLogNum;
		///<summary>Key to the foreign table.</summary>
		public long FKey;
		///<summary>Enum:InsEditPatLogType 0 - PatPlan, 1 - Subscriber, 2 - Adjustment.</summary>
		public InsEditPatLogType LogType;
		///<summary>The name of the column that was altered.</summary>
		public string FieldName;
		///<summary>The old value of this field.</summary>
		public string OldValue;
		///<summary>The new value of this field.</summary>
		public string NewValue;
		///<summary>FK to userod.UserNum. The user that made this change.</summary>
		public long UserNum;
		///<summary>Time that the row was inserted into the DB.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.TimeStamp)]
		public DateTime DateTStamp;
		///<summary>Stores the key to the parent table.
		///0 - PatPlan: patplan.PatNum
		///1 - Subscriber: inssub.Subscriber
		///2 - Adjustment: claimproc.InsSubNum</summary>
		public long ParentKey;
		///<summary>The string describing this entry. Displays different information depending on the LogType:
		///1 - Subscriber: Subscriber's Name
		///2 - Adjustment: Insurance Benefit</summary>
		public string Description;
	}

	public enum InsEditPatLogType {
		///<summary>0</summary>
		PatPlan,
		///<summary>1</summary>
		Subscriber,
		///<summary>2 - Adjustments to insurance benefits.</summary>
		Adjustment,
	}
}
