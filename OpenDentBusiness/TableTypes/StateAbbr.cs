using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
	[Table("states")]
	public class StateAbbr : TableBase
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// Full state name.
		/// </summary>
		public string Description;

		/// <summary>
		/// Short state abbreviation (usually 2 characters).
		/// </summary>
		public string Abbr;

		/// <summary>
		/// The length that the Medicaid ID should be for this state. If 0, then the Medicaid length is not enforced for this state.
		/// </summary>
		[Column("medicaid_id_length")]
		public int MedicaidIDLength;

		public StateAbbr Clone() => (StateAbbr)MemberwiseClone();
	}
}
