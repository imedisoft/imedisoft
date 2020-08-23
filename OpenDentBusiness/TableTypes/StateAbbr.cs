using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
	/// <summary>
	/// State abbreviations are always copied to patient records rather than linked.  
	/// Items in this list can be freely altered or deleted without harming patient data.
	/// </summary>
	[Table]
	public class StateAbbr : TableBase
	{
		[PrimaryKey]
		public long StateAbbrNum;

		/// <summary>
		/// Full state name.
		/// </summary>
		public string Description;

		/// <summary>
		/// Short state abbreviation (usually 2 digit)
		/// </summary>
		public string Abbr;

		/// <summary>
		/// The length that the Medicaid ID should be for this state. If 0, then the Medicaid length is not enforced for this state.
		/// </summary>
		public int MedicaidIDLength;

		public StateAbbr Clone() => (StateAbbr)MemberwiseClone();
	}
}
