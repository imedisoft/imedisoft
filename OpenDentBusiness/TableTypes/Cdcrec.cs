using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
    /// <summary>
    /// CDC Race and Ethnicity.
    /// </summary>
    [Table("cdcrecs")]
	public class Cdcrec
	{
		[PrimaryKey]
		public long Id;

		[Column(ReadOnly = true)]
		public string Code;

		/// <summary>
		///		Hierarchical Code. Example:
		///		<list type="table">
		///			<item>
		///				<term>R1</term> "American Indian or alaska Native"
		///			</item>
		///			<item>
		///				<term>R1.01</term> "American Indian"
		///			</item>
		///			<item>
		///				<term>R1.01.001</term> "Abenaki"
		///			</item>
		///		</list>
		/// </summary>
		[Column(ReadOnly = true)]
		public string HierarchicalCode;

		public string Description;
	}
}
