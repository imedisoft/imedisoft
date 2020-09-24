using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
    [Table("codesystems")]
	public class CodeSystem : TableBase
	{
		[PrimaryKey]
		public long Id;

		public string Name;

		/// <summary>
		/// Only used for display, not actually interpreted. Updated by Code System importer. Examples: 2013 or 1
		/// </summary>
		public string VersionCur;

		/// <summary>
		/// Only used for display, not actually interpreted. Updated by Convert DB script.
		/// </summary>
		public string VersionAvail;

		/// <summary>
		/// Example: 2.16.840.1.113883.6.13
		/// </summary>
		public string HL7OID;

		/// <summary>
		/// Notes to display to user. Examples: "CDT codes distributed via program updates.", "CPT codes require purchase and download from www.ama.com"
		/// </summary>
		public string Note;
	}
}
