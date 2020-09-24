using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
    /// <summary>
    ///		<para>
    ///			Represents patient information needed for EHR.
    ///		</para>
    /// </summary>
    [Table("ehr_patients")]
	public class EhrPatient
	{
		[PrimaryKey]
		public long PatientId;

		/// <summary>
		///		<para>
		///			Mother's maiden first name.
		///		</para>
		///		<para>
		///			Exported in HL7 PID-6 for immunization messages.
		///		</para>
		/// </summary>
		public string MotherMaidenFirstName;

		/// <summary>
		///		<para>
		///			Mother's maiden last name.
		///		</para>
		///		<para>
		///			Exported in HL7 PID-6 for immunization messages.
		///		</para>
		/// </summary>
		public string MotherMaidenLastName;

		/// <summary>
		///		<para>
		///			Indicates whether or not the patient wants to share their vaccination information with other EHRs. 
		///		</para>
		/// </summary>
		public bool? AllowShareVaccines;

		/// <summary>
		///		<para>
		///			The abbreviation for the state for the patient's MedicaidID.
		///		</para>
		///		<para>
		///			Displayed in patient information window, used to validate the length of the 
		///			MedicaidID.
		///		</para>
		/// </summary>
		public string MedicaidState;

		/// <summary>
		///		<para>
		///			Snomed CT code that identifies the patient's sexual orientation.
		///		</para>
		///		<para>
		///			If the <see cref="SexualOrientation"/> is set to 'OTH' (other), the sexual 
		///			orientation should be described in <see cref="SexualOrientationOther"/>.
		///		</para>
		/// </summary>
		/// <see cref="Imedisoft.Data.Models.CodeLists.HL7.SexualOrientation"/>
		[Nullable]
		public string SexualOrientation;

		/// <summary>
		/// Describes a additional sexual orientation. Blank unless <see cref="SexualOrientation"/> is 'OTH'.
		/// </summary>
		public string SexualOrientationOther;

		/// <summary>
		///		<para>
		///			Snomed CT code that identifies the gender identity of the patient.
		///		</para>
		///		<para>
		///			If the <see cref="GenderIdentity"/> is set to 'OTH' (other), the gender 
		///			identity should be described in <see cref="GenderIdentityOther"/>.
		///		</para>
		/// </summary>
		/// <see cref="Imedisoft.Data.Models.CodeLists.HL7.GenderIdentity"/>
		[Nullable]
		public string GenderIdentity;

		/// <summary>
		/// Describes a additional gender identity. Blank unless <see cref="GenderIdentity"/> is 'OTH'.
		/// </summary>
		public string GenderIdentityOther;
	}
}
