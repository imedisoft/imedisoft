using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table("ehr_drug_units")] // TODO: Drop this table and use UCUM instead?
	public class EhrDrugUnit
	{
		/// <summary>
		/// Example ml, capitalization not critical. Usually entered as lowercase except for L.
		/// </summary>
		[PrimaryKey]
		public string Code;

		/// <summary>
		/// Example milliliter.
		/// </summary>
		public string Description;

		/// <summary>
		/// Returns a string representation of the drug unit.
		/// </summary>
		public override string ToString() => $"{Code} - {Description}";
    }
}