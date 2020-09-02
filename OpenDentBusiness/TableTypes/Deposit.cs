using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using System;
using System.Collections;

namespace OpenDentBusiness
{
    /// <summary>
    /// A deposit slip. Contains multiple insurance and patient checks.
    /// </summary>
    [Table("deposits")]
	public class Deposit : TableBase
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The date of the deposit.
		/// </summary>
		public DateTime DateDeposit;

		/// <summary>
		/// User editable. 
		/// Usually includes name on the account and account number. 
		/// Possibly the bank name as well.
		/// </summary>
		public string BankAccountInfo;

		/// <summary>
		/// Total amount of the deposit. User not allowed to directly edit.
		/// </summary>
		public double Amount;

		/// <summary>
		/// Short description to help identify the deposit.
		/// </summary>
		public string Memo;

		/// <summary>
		/// Holds the batch number for the deposit. Does not have a default value. 25 character limit.
		/// </summary>
		public string Batch;

		/// <summary>
		/// Links this deposit to a definition of type AutoDeposit.
		/// When set to a valid value, it indicates that this deposit is an "auto deposit".
		/// </summary>
		[ForeignKey(typeof(Definition), nameof(Definition.Id))]
		public long DepositAccountId;

		/// <summary>
		/// Not in the database table. 
		/// Identifies the clinic(s) that the deposit is associated to.
		/// '(None)', specific clinic abbr or '(Multiple)'.
		/// </summary>
		[Ignore]
		public string ClinicAbbr;

		public Deposit Copy()
		{
			return (Deposit)MemberwiseClone();
		}
	}
}
