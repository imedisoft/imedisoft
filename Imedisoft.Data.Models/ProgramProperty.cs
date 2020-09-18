using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    /// <summary>
    /// Some program links (bridges), have properties that need to be set. 
    /// The property names are always hard coded.  User can change the value. 
    /// The property is usually retrieved based on its name.
    /// </summary>
    [Table]
	public class ProgramProperty
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The ID of the clinic the property applies to.
		/// </summary>
		//[Column("ClinicNum"), ForeignKey(typeof(Clinic), nameof(Clinic.Id))]
		public long ClinicId;

		//[Column("ProgramNum"), ForeignKey(typeof(Program), nameof(Program.Id))]
		public long ProgramId;

		/// <summary>
		/// The (internal) name of the property.
		/// </summary>
		public string Description;

		/// <summary>
		/// The property value
		/// </summary>
		public string Value;

		/// <summary>
		/// The name of the computer the property applies to.
		/// </summary>
		[Nullable]
		public string MachineName;

		public ProgramProperty Copy()
		{
			return (ProgramProperty)MemberwiseClone();
		}
	}
}
