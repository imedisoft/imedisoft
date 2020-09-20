using Imedisoft.Data.Annotations;
using System;

namespace OpenDentBusiness
{
    [Table("ehr_quarterly_keys")]
	public class EhrQuarterlyKey
	{
		[PrimaryKey]
		public long Id;

		public int Year;

		public int Quarter;

		/// <summary>
		/// The customer must have this exact practice name entered in practice setup.
		/// </summary>
		public string PracticeName;

		/// <summary>
		/// The calculated key value, tied to year, quarter, and practice name.
		/// </summary>
		public string Key;

		[Ignore, Obsolete]
		public long PatNum;

		[Ignore, Obsolete]
		public string Notes;
	}
}
