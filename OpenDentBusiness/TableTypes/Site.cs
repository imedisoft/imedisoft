using Imedisoft.Data.Annotations;
using System;
using System.Collections;

namespace OpenDentBusiness
{
	/// <summary>
	/// Generally used by mobile clinics to track the temporary locations where treatment is performed, such as schools, nursing homes, and community centers.
	/// Replaces the old school table.
	/// </summary>
	[Table]
	public class Site : TableBase
	{
		[PrimaryKey]
		public long SiteNum;

		public string Description;

		/// <summary>
		/// Notes could include phone, contacts, etc.
		/// </summary>
		public string Note;

		public string Address;

		/// <summary>
		/// Optional second address line.
		/// </summary>
		public string Address2;

		public string City;

		/// <summary>
		/// 2 Char in USA. Used to store province for Canadian users.
		/// </summary>
		public string State;

		/// <summary>
		/// Postal code.
		/// </summary>
		public string Zip;

		/// <summary>
		/// FK to provider.ProvNum.  Default provider for the site.
		/// </summary>
		public long ProvNum;

		/// <summary>
		/// Enum:PlaceOfService Describes where the site is located.
		/// </summary>
		public string PlaceService;

		public Site Copy() => (Site)MemberwiseClone();

		public override string ToString() => Description;
    }
}
