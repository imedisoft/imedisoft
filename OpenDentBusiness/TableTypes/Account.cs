using Imedisoft.Data.Annotations;
using System;
using System.Collections;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenDentBusiness
{
	[Table]
	public class Account : TableBase
	{
		[PrimaryKey]
		public long AccountNum;

		public string Description;

		/// <summary>
		/// Enum:AccountType Asset, Liability, Equity,Revenue, Expense
		/// </summary>
		public AccountType AcctType;

		/// <summary>
		/// For asset accounts, this would be the bank account number for deposit slips.
		/// </summary>
		public string BankNumber;

		/// <summary>
		/// Set to true to not normally view this account in the list.
		/// </summary>
		public bool Inactive;

		public Color AccountColor;

		public Account Clone() => (Account)MemberwiseClone();
	}
}
