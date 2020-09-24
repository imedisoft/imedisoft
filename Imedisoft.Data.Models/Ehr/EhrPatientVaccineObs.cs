using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models.CodeLists.HL7;
using System;

namespace Imedisoft.Data.Models
{
    /// <summary>
    ///		<para>
    ///			Represents a vaccine observation.
    ///		</para>
    ///		<para>
    ///			There may be multiple vaccine observations for each vaccine.
    ///		</para>
    /// </summary>
    [Table("ehr_patient_vaccine_obs")]
	public class EhrPatientVaccineObservation
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// All vaccineobs records with matching GroupId are in the same group. 
		/// Set to 0 if this vaccine observation is not part of a group. 
		/// Used in HL7 OBX-4.
		/// </summary>
		public long? Group;

		[ForeignKey(typeof(EhrPatientVaccine), nameof(EhrPatientVaccine.Id))]
		public long EhrPatientVaccineId;

		/// <summary>
		///		<para>
		///			Identifies the observation question. 
		///		</para>
		///		<para>
		///			Used in <b>HL7 OBX-3</b>.
		///		</para>
		/// </summary>
		public string IdentifyingCode = NIP003.DatePublished;

		/// <summary>
		///		<para>
		///			Used in <b>HL7 OBX-2</b>.
		///		</para>
		/// </summary>
		public string ValueType = HL70125.Coded;

		/// <summary>
		///		<para>
		///			The observation value. The type of the value depends on the <see cref="ValueType"/>.
		///		</para>
		///		<para>
		///			Used in <b>HL7 OBX-5</b>.
		///		</para>
		/// </summary>
		public string Value;

		/// <summary>
		///		<para>
		///			The observation value code system when <see cref="ValueType"/> is 
		///			<see cref="HL70125.Coded"/>.
		///		</para>
		///		<para>
		///			Used in <b>HL7 OBX-5</b>.
		///		</para>
		/// </summary>
		public string CodeSystem;

		/// <summary>
		///		<para>
		///			Used in <b>HL7 OBX-6</b>.
		///		</para>
		/// </summary>
		public string UcumCode;

		/// <summary>
		///		<para>
		///			Date of observation.
		///		</para>
		///		<para>
		///			Used in <b>HL7 OBX-14</b>.
		///		</para>
		/// </summary>
		public DateTime? Date;

		/// <summary>
		///		<para>
		///			Code from code set CDCPHINVS (this code system is not yet fully defined, so 
		///			user has to enter manually). Only required when <see cref="IdentifyingCode"/> 
		///			is <see cref="NIP003.FundPgmEligCat"/>.
		///		</para>
		///		<para>
		///			Used in <b>HL7 OBX-17</b>.
		///		</para>
		/// </summary>
		public string MethodCode;
	}
}
