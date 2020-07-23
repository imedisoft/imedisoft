using Imedisoft.Data.Annotations;
using System;
using System.Collections;

namespace OpenDentBusiness
{
    /// <summary>
    /// In the accounting section, this automates entries into the database when user enters a payment into a patient account.
    /// This table presents the user with a picklist specific to that payment type.
    /// For example, a cash payment would create a picklist of cashboxes for user to put the cash into.
    /// </summary>
	[Table("accounting_auto_pays")]
	public class AccountingAutoPay : TableBase
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(Def), nameof(Def.DefNum))]
		public long PayType;

		/// <summary>
		/// A comma delimited list of <see cref="Account"/> ID's.
		/// </summary>
		public string PickList;
	}
}
