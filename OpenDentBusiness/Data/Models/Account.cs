using Imedisoft.Data.Annotations;
using System;
using System.Collections;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenDentBusiness
{
	[Table("accounts")]
	public class Account : TableBase
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// A description of the account.
		/// </summary>
		public string Description;

		/// <summary>
		/// The account type (Asset, Liability, Equity, Revenue, Expense).
		/// </summary>
		public AccountType Type;

		/// <summary>
		/// For asset accounts, this would be the bank account number for deposit slips.
		/// </summary>
		public string BankNumber;

		/// <summary>
		/// Set to true to not normally view this account in the list.
		/// </summary>
		public bool Inactive;

		public Color Color;

		public Account Clone() => (Account)MemberwiseClone();
	}
}
