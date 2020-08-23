using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
    /// <summary>
    /// A vaccine definition. Should not be altered once linked to VaccinePat.
    /// </summary>
    [Table]
	public class VaccineDef : TableBase
	{
		[PrimaryKey]
		public long VaccineDefNum;

		/// <summary>
		/// RXA-5-1.
		/// </summary>
		public string CVXCode;

		/// <summary>
		/// Name of vaccine. RXA-5-2.
		/// </summary>
		public string VaccineName;

		[ForeignKey(typeof(DrugManufacturer), nameof(DrugManufacturer.DrugManufacturerNum))]
		public long DrugManufacturerNum;

		public VaccineDef Copy() 
			=> (VaccineDef)MemberwiseClone();
	}
}
