using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    /// <summary>
    ///		<para>
    ///			We do not import synonyms, only "Fully Specified Name records". 
    ///		</para>
    ///		<para>
    ///			SNOMED CT maintained, owned and copyright International Health Terminology Standards Development Organisation (IHTSDO).
    ///		</para>
    /// </summary>
    [Table("snomed")]
	public class Snomed
	{
		/// <summary>
		/// Also called the Concept ID.
		/// </summary>
		[PrimaryKey]
		public string Code;

		/// <summary>
		/// Also called "Term", "Name", or "Fully Specified Name". Not editable and doesn't change.
		/// </summary>
		public string Description;
	}
}
