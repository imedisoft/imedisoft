using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
	/// <summary>
	/// Used in dental schools. e.g. "Dental 2009" or "Hygiene 2007".
	/// </summary>
	[Table("school_classes")]
	public class SchoolClass
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The year the class will graduate.
		/// </summary>
		public int Year;

		/// <summary>
		/// A description of the class. e.g. "Dental" or "Hygiene".
		/// </summary>
		public string Description;

		/// <summary>
		/// Returns a string representation of the class.
		/// </summary>
		public override string ToString() 
			=> $"{Year} {Description}";
	}
}
