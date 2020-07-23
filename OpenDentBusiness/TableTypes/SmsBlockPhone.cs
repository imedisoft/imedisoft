using Imedisoft.Data.Annotations;
using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness
{
	/// <summary>
	/// If a number is entered in this table, then any incoming text message will not be entered into the database.
	/// </summary>
	[Table]
	public class SmsBlockPhone : TableBase
	{
		[PrimaryKey]
		public long SmsBlockPhoneNum;

		/// <summary>
		/// The phone number to be blocked.
		/// </summary>
		public string BlockWirelessNumber;

		public SmsBlockPhone Copy() => (SmsBlockPhone)MemberwiseClone();
	}
}
