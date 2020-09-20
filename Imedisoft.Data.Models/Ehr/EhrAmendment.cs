using Imedisoft.Data.Annotations;
using System;

namespace Imedisoft.Data.Models
{
	/// <summary>
	///		<para>
	///			Represents an verbal or written request to add information to the patient's record.
	///		</para>
	///		<para>
	///			Providers can either scan a written request or create a detailed description that 
	///			indicates what was verbally requested or where the document can be found.
	///		</para>
	///		<para>
	///			<b>Used in EHR only.</b>
	///		</para>
	/// </summary>
	[Table("ehr_amendments")]
	public class EhrAmendment
	{
		[PrimaryKey]
		public long Id;

		public long PatientId;

		/// <summary>
		///		<para>
		///			A value indicating whether the amendment has been accepted.
		///		</para>
		///		<para>
		///			Can be NULL when the amendment is not yet accepted or denied.
		///		</para>
		/// </summary>
		public bool? IsAccepted;

		/// <summary>
		/// Description or user-defined location of the amendment.
		/// </summary>
		public string Description;

		/// <summary>
		/// The source of the amendment.
		/// </summary>
		public EhrAmendmentSource Source;

		/// <summary>
		/// User-defined name of the amendment source. For example, a patient name or organization name.
		/// </summary>
		public string SourceName;

		/// <summary>
		/// The file is stored in the A-Z folder in 'EhrAmendments' folder. 
		/// This field stores the name of the file. 
		/// The files are named automatically based on Date/time along with EhrAmendmentNum for uniqueness.
		/// This meets the requirement of "appending" to the patient's record.
		/// </summary>
		public string FileName;

		/// <summary>
		/// The raw file data encoded as base64. Only used if there is no AtoZ folder. 
		/// This meets the requirement of "appending" to the patient's record.
		/// </summary>
		[Ignore, Obsolete]
		public string RawBase64;

		/// <summary>
		/// Date and time of the amendment request.
		/// </summary>
		public DateTime? RequestedOn;

		/// <summary>
		/// Date and time of the amendment acceptance or denial. 
		/// If there is a date here, then the IsAccepted will be set.
		/// </summary>
		public DateTime? AcceptedDeniedOn;

		/// <summary>
		/// Date and time of the file being appended to the amendment or a link provided.
		/// </summary>
		public DateTime? AppendedOn;
	}
}
