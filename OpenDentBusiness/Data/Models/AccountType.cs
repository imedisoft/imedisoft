using System.Collections.Generic;

namespace Imedisoft.Data.Models
{
    /// <summary>
    ///		<para>
    ///			Identifies the type of a <see cref="Account"/>.
    ///		</para>
    ///		<para>
    ///			Used in accounting for chart of accounts.
    ///		</para>
    /// </summary>
    public static class AccountType
	{
		public const char Asset = 'A';
		public const char Liability = 'L';
		public const char Equity = 'Q';
		public const char Income = 'I';
		public const char Expense = 'E';

		/// <summary>
		/// Enumerates all the available account types.
		/// </summary>
		public static IEnumerable<(char code, string description)> All
        {
            get
            {
				yield return (Asset, Translation.Accounting.Asset);
				yield return (Liability, Translation.Accounting.Liability);
				yield return (Equity, Translation.Accounting.Equity);
				yield return (Income, Translation.Accounting.Income);
				yield return (Expense, Translation.Accounting.Expense);
			}
        }

		/// <summary>
		/// Gets a short (translated) description of the specified account type.
		/// </summary>
		/// <param name="accountType">The account type.</param>
		/// <returns>A description of the account type.</returns>
		public static string GetDescription(char accountType)
			=> accountType switch
			{
				Asset => Translation.Accounting.Asset,
				Liability => Translation.Accounting.Liability,
				Equity => Translation.Accounting.Equity,
				Income => Translation.Accounting.Income,
				Expense => Translation.Accounting.Expense,
				_ => Translation.Common.Unknown
			};
	}
}
