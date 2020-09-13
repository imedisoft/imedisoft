using Imedisoft.Data.Annotations;
using System;

namespace Imedisoft.Data.Models
{
    [Table("ehr_provider_keys")]
	public class EhrProviderKey
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// Only used by HQ for generating keys for customers. Will always be 0 for non-HQ users.
		/// </summary>
		[Ignore, Obsolete]
		public long PatientId;

		public string LastName;

		public string FirstName;

		/// <summary>
		/// The key assigned to the provider.
		/// </summary>
		public string Key;

		/// <summary>
		/// Usually 1.  Can be less, like .5 or .25 to indicate possible discount is justified.
		/// </summary>
		[Ignore, Obsolete]
		public float FullTimeEquiv;

		/// <summary>
		/// Any notes that the tech wishes to include regarding this situation.
		/// </summary>
		public string Notes;

		/// <summary>
		/// Required when generating a new provider key. It is used to determine annual EHR eligibility. Format will always be YY.
		/// </summary>
		public int Year;
	}
}
