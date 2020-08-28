using Imedisoft.Data.Annotations;
using System.Drawing;

namespace Imedisoft.Data.Models
{
	[Table("accounts")]
	public class Account
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// A description of the account.
		/// </summary>
		public string Description;

		/// <summary>
		/// The account type.
		/// </summary>
		public char Type;

		/// <summary>
		/// For asset accounts, this would be the bank account number for deposit slips.
		/// </summary>
		public string BankNumber;

		/// <summary>
		/// Set to true to not normally view this account in the list.
		/// </summary>
		public bool Inactive;

		/// <summary>
		/// The color used when displaying the account or related journal entries.
		/// </summary>
		public Color Color;
	}
}
