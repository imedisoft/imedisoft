using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    /// <summary>
    /// Represents the manufacturer of a vaccine.
    /// </summary>
    [Table("drug_manufacturers")]
	public class EhrDrugManufacturer
	{
		[PrimaryKey]
		public long Id;

		public string Name;

		public string Code;

        /// <summary>
        /// Returns a string representation of the manufacturer.
        /// </summary>
        public override string ToString() => $"{Code} - {Name}";
    }
}
