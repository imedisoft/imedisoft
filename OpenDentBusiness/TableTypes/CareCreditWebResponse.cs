using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenDentBusiness {
	///<summary>This table will never delete records, only upsert. CareCreditResponseWeb rows are records of all CareCredit made.</summary>
	[Serializable]
	public class CareCreditWebResponse:TableBase {
		/// <summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long CareCreditWebResponseNum;
		///<summary>FK to patient.PatNum.</summary>
		public long PatNum;
		///<summary>FK to payment.PayNum.</summary>
		public long PayNum;
		///<summary>The RefNumber associated to this request.</summary>
		public string RefNumber;
		///<summary>The amount of the request. This can be purchases or refund amount.</summary>
		public double Amount;
		///<summary>The web token used for pullback request.</summary>
		public string WebToken;
		///<summary>Enum:CareCreditWebStatus Used to determine if the request is pending, needs action, or is completed.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public CareCreditWebStatus ProcessingStatus;
		///<summary>Timestamp automatically generated and user not allowed to change.  The actual datetime of entry.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateTEntry)]
		public DateTime DateTimeEntry;
		///<summary>DateTime that the payment went to the pending status.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimePending;
		///<summary>DateTime that the payment went to the completed status and is attached to a payment.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeCompleted;
		///<summary>DateTime that the payment opportunity time expired.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeExpired;
		///<summary>DateTime of the last time that the payment had an error.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeLastError;
		///<summary>Raw JSON response (or error) from CareCredit.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string LastResponseStr;
		///<summary>FK to clinic.ClinicNum.</summary>
		public long ClinicNum;
		///<summary>Enum:CareCreditServiceType Used to determine what service was requested for this web response.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public CareCreditServiceType ServiceType;
		///<summary>Enum:CareCreditTransType Used to determine the transaction type.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public CareCreditTransType TransType;

		///<summary>Returns the DateTime of the greatest value between DateTimeEntry, DateTimePending, and DateTimeLastError.
		///This does not consider DateTimeCompleted or DateTimeExpired, as those are DateTimes that make the CareCreditWebResponse done.</summary>
		public DateTime GetLastPendingUpdateDateTime() {
			return CodeBase.ODMathLib.Max(DateTimeEntry,CodeBase.ODMathLib.Max(DateTimePending,DateTimeLastError));
		}
	}

	public enum CareCreditWebStatus {
		///<summary>0.</summary>
		Created,
		///<summary>1.</summary>
		CreatedError,
		///<summary>2.</summary>
		Pending,
		///<summary>3.</summary>
		PendingError,
		///<summary>4.</summary>
		Expired,
		///<summary>5.</summary>
		Completed,
		///<summary>6.</summary>
		Cancelled,
		///<summary>7.</summary>
		Declined,
		///<summary>8.</summary>
		CallForAuth,
		///<summary>9.</summary>
		DupQS,
		///<summary>10.</summary>
		AccountFound,
		///<summary>11.</summary>
		Unknown,
		///<summary>12.</summary>
		BatchError,
		///<summary>13.</summary>
		UnknownError,
	}

	public enum CareCreditServiceType {
		///<summary>0.</summary>
		Batch,
		///<summary>1.</summary>
		Prefill,
	}

	public enum CareCreditTransType {
		///<summary>0.</summary>
		Purchase,
		///<summary>1.</summary>
		Refund,
	}
}
