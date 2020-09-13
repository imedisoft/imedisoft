using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
    /// <summary>
    ///		<para>
    ///			Logical Observation Identifiers Names and Codes (LOINC)
    ///		</para>
    ///		<para>
    ///			Used to identify both lab panels and lab results. 
    ///		</para>
    /// </summary>
    /// <seealso href="https://loinc.org/"/>
    [Table("loinc")]
	public class Loinc
	{
		[Ignore]
		public long LoincNum;

		[PrimaryKey(AutoIncrement = false)]
		public string Code;

		/// <summary>
		/// The substance or entity being measured or observed.
		/// </summary>
		public string Component;

		/// <summary>
		/// The characteristic or attribute of the analyte.
		/// </summary>
		public string Property;

		/// <summary>
		/// The interval of time over which an observation was made.
		/// </summary>
		public string Time;

		/// <summary>
		/// The specimen or thing upon which the observation was made.
		/// </summary>
		public string System;

		/// <summary>
		/// How the observation value is quantified or expressed: quantitative, ordinal, nominal.
		/// </summary>
		public string Scale;

		/// <summary>
		/// (Optional) A high-level classification of how the observation was made. Only needed 
		/// when the technique affects the clinical interpretation of the results.
		/// </summary>
		public string Method;

		public string Class;

		/// <summary>
		/// 1=Laboratory class; 2=Clinical class; 3=Claims attachments; 4=Surveys. LOINC244 column 16.
		/// </summary>
		public string ClassType;

		/// <summary>
		/// This field contains the LOINC term in a more readable format than the fully specified name. 
		/// The long common names have been created via a table driven algorithmic process. 
		/// Most abbreviations and acronyms that are used in the LOINC database have been fully spelled out in English. Width 255. LOINC244 column 35.
		/// </summary>
		public string LongCommonName;

		/// <summary>
		/// Introduced in version 2.07, this field is a concatenation of the fully specified LOINC name. 
		/// The field width may change in a future release. Width 40. LOINC244 column 29.
		/// </summary>
		public string ShortName;

		public string ExternalCopyrightNotice;

		/// <summary>
		///		<para>
		///			The current status of the concept.
		///		</para>
		///		<list type="table">
		///			<item>
		///				<term>ACTIVE</term> 
		///				Concept is active. Use at will.
		///			</item>
		///			<item>
		///				<term>TRIAL</term>
		///				Concept is experimental in nature. Use with caution as the concept and 
		///				associated attributes may change.
		///			</item>
		///			<item>
		///				<term>DISCOURAGED</term>
		///				Concept is not recommended for current use. New mappings to this concept 
		///				are discouraged; although existing may mappings may continue to be valid in
		///				context. Wherever  possible, the superseding concept is indicated in the 
		///				MAP_TO field in the MAP_TO table (see Table 28b) and should be used 
		///				instead.
		///			</item>
		///			<item>
		///				<term>DEPRECATED</term>
		///				Concept is deprecated. Concept should not be used, but it is retained in 
		///				LOINC for historical purposes. Wherever possible, the superseding concept 
		///				is indicated in the MAP_TO field (see Table 28b) and should be used both 
		///				for new mappings and updating existing implementations.
		///			</item>
		///		</list>
		///</summary>
		public string Status;







		[Ignore]
		public bool UnitsRequired;

		[Ignore]
		public string OrderObs;

		[Ignore]
		public string UnitsUCUM;

		[Ignore]
		public string HL7FieldSubfieldID;

		[Ignore]
		public int RankCommonTests;

		[Ignore]
		public int RankCommonOrders;




		/// <summary>
		/// Gets a value indicating whether the concept is active.
		/// </summary>
		public bool IsActive => Status == "ACTIVE";

		/// <summary>
		/// Gets a value indicating whether the concept is a trial (expirimental) concept.
		/// </summary>
		public bool IsTrial => Status == "TRIAL";

		/// <summary>
		/// Gets a value indicating whether the use of the concept is discouraged.
		/// </summary>
		public bool IsDiscouraged => Status == "DISCOURAGED";

		/// <summary>
		/// Gets a value indicating whether the concept has been deprecated.
		/// </summary>
		public bool IsDeprecated => Status == "DEPRECATED";
	}
}
