using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models.CodeLists.HL7;
using System;

namespace Imedisoft.Data.Models
{
	/// <summary>
	/// Represents a vaccine administered to a patient on a given date.
	/// </summary>
	/// <seealso href="http://hl7v2-iz-testing.nist.gov/mu-immunization/"/>
	[Table("ehr_patient_vaccines")]
	public class EhrPatientVaccine
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		///		<para>
		///			The ID of the vaccine that was administered.
		///		</para>
		///		<para>
		///			May be NULL if the vaccine has not yet been administered, in which case CVX code is assumed to be 998 (not administered) 
		///		</para>
		/// </summary>
		public long? VaccineId;

		/// <summary>
		/// The datetime that the vaccine was administered.
		/// </summary>
		public DateTime? DateStart;

		/// <summary>
		/// Typically set to the same as DateTimeStart. User can change.
		/// </summary>
		public DateTime? DateEnd;

		/// <summary>
		///		<para>
		///			Size of the dose of the vaccine. 0 indicates unknown.
		///		</para>
		/// </summary>
		public float AdministeredAmount;

		[Nullable, ForeignKey(typeof(EhrDrugUnit), nameof(EhrDrugUnit.Code))]
		public string DrugUnitCode;

		/// <summary>
		///		<para>
		///			Used in <b>HL7 RXA-9.1</b>.
		///		</para>
		/// </summary>
		public string LotNumber;

		public long PatientId;

		/// <summary>
		/// Documentation sometimes required.
		/// </summary>
		public string Note;

		/// <summary>
		///		<para>
		///			The city where the vaccine was filled. 
		///		</para>
		///		<para>
		///			This can be different than the practice office city for historical vaccine information. 
		///		</para>
		///		<para>
		///			Exported in <b>HL7 ORC-3</b>.
		///		</para>
		/// </summary>
		public string FilledCity;

		/// <summary>
		///		<para>
		///			The state where the vaccine was filled.
		///		</para>
		///		<para>
		///			This can be different than the practice office state for historical vaccine infromation.
		///		</para>
		///		<para>
		///			Exported in <b>HL7 ORC-3</b>.
		///		</para>
		/// </summary>
		public string FilledState;

		/// <summary>
		///		<para>
		///			Exported in <b>HL7 RXA-20</b>.
		///		</para>
		/// </summary>
		/// <see cref="TreatmentCompletionStatus"/>
		public string CompletionStatus = TreatmentCompletionStatus.Complete;

		/// <summary>
		///		<para>
		///			Exported in <b>HL7 RXA-9</b>.
		///		</para>
		/// </summary>
		///	<see cref="NIP001"/>
		public string InformationSource;

		/// <summary>
		///		<para>
		///			The ID of the user that entered the vaccine.
		///		</para>
		///		<para>
		///			Exported in <b>HL7 ORD-10</b>.
		///		</para>
		/// </summary>
		public long UserId;

		/// <summary>
		///		<para>
		///			The ID of the provider that ordered the vaccine.
		///		</para>
		///		<para>
		///			Exported in <b>HL7 ORD-12</b>.
		///		</para>
		/// </summary>
		public long? OrderedBy;

		/// <summary>
		///		<para>
		///			The ID of the provider that administered the vaccine.
		///		</para>
		///		<para>
		///			Exported in <b>HL7 RXA-10</b>.
		///		</para>
		/// </summary>
		public long? AdministeredBy;

		/// <summary>
		///		<para>
		///			The date on which the vaccine expires.
		///		</para>
		///		<para>Exported in <b>HL7 RXA-16</b>.</para>
		///	</summary>
		public DateTime? ExpirationDate;

		/// <summary>
		///		<para>
		///			Exported in <b>HL7 RXA-18</b>.
		///		</para>
		/// </summary>
		/// <see cref="SubstanceRefusalReason"/>
		[Nullable]
		public string RefusalReason;

		/// <summary>
		///		<para>
		///			Exported in <b>HL7 RXA-21</b>.
		///		</para>
		/// </summary>
		/// <see cref="HL70323"/>
		public char ActionCode = HL70323.Add;

		/// <summary>
		///		<para>
		///			Exported in <b>HL7 RXR-1</b>.
		///		</para>
		/// </summary>
		/// <see cref="RouteOfAdministration"/>
		[Nullable]
		public string AdministrationRoute;

		/// <summary>
		///		<para>
		///			Exported in <b>HL7 RXR-2</b>.
		///		</para>
		/// </summary>
		/// <see cref="AdministrativeSite"/>
		[Nullable]
		public string AdministrationSite;
	}
}
