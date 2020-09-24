using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table("ehr_vaccines")]
	public class EhrVaccine
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(EhrDrugManufacturer), nameof(EhrDrugManufacturer.Id))]
		public long EhrDrugManufacturerId;

		public string CvxCode;

		public string Name;

		/// <summary>
		/// Returns a string representation of the vaccine.
		/// </summary>
		public override string ToString() => $"{CvxCode} - {Name}";
    }
}
