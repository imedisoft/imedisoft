using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;

namespace OpenDentBusiness
{
    public class DefLink
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(Definition), nameof(Definition.Id))]
		public long DefinitionId;

		/// <summary>
		/// A foreign key to a table associated with the DefLinkType. 
		/// Uses include:  ClinicNum with DefLinkType Clinic, PatNum with DefLinkType Patient.
		/// </summary>
		public long FKey;

		/// <summary>
		/// The type of link.
		/// </summary>
		public DefLinkType LinkType;
	}

	///<summary>The manner in which a definition is linked.</summary>
	public enum DefLinkType
	{
		///<summary>0. The definition is linked to a clinic.</summary>
		Clinic,
		///<summary>1. The definition is linked to a patient.</summary>
		Patient,
		///<summary>2. The definition is linked to an appointment type.</summary>
		AppointmentType,
		///<summary>3. The definition is linked to an operatory.</summary>
		Operatory,
		///<summary>4. The definition is linked to another definition that is in the BlockoutType category.</summary>
		BlockoutType,
	}
}
