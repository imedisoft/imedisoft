using Imedisoft.Data.Annotations;
using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// One to one relation with the patient table representing each customer as a reference.
    /// </summary>
    [Table]
	public class CustReference : TableBase
	{
		[PrimaryKey]
		public long CustReferenceNum;

		[ForeignKey(typeof(Patient), nameof(Patient.PatNum))]
		public long PatNum;

		/// <summary>
		/// Most recent date the reference was used, loosely kept updated.
		/// </summary>
		public DateTime DateMostRecent;

		/// <summary>
		/// Notes specific to this customer as a reference.
		/// </summary>
		public string Note;

		/// <summary>
		/// Set to true if this customer was a bad reference.
		/// </summary>
		public bool IsBadRef;
	}
}
