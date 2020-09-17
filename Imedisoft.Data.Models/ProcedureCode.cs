using Imedisoft.Data.Annotations;
using System;
using System.Drawing;
using System.Xml.Serialization;

namespace Imedisoft.Data.Models
{
    /// <summary>
    /// A list setup ahead of time with all the procedure codes used by the office. 
    /// Every procedurelog entry which is attached to a patient is also linked to this table.
    /// </summary>
    [Table("procedure_codes")]
	public class ProcedureCode
	{
		[PrimaryKey]
		public long Id;

		[Column(ReadOnly = true)]
		public string Code;

		/// <summary>
		/// A description of the procedure code.
		/// </summary>
		public string Description;

		/// <summary>
		/// A abbreviated description.
		/// </summary>
		public string ShortDescription;

		/// <summary>
		/// X's and /'s describe Dr's time and assistant's time in the same increments as the user has set.
		/// </summary>
		public string Time;
		
		[ForeignKey(typeof(Definition), nameof(Definition.Id))]
		public long ProcedureCategory;

		/// <summary>
		/// A value indicating the area the procedure applies to.
		/// </summary>
		public ProcedureTreatmentArea TreatmentArea;

		/// <summary>
		/// If true, do not usually bill this procedure to insurance.
		/// </summary>
		public bool NoInsuranceBill;

		/// <summary>
		///		<para>
		///			A value indicating whether the code represents a prosthesis (i.e. Crown, Bridge, Denture or RPD).
		///		</para>
		///		<para>
		///			When true, users are forced to specify 'Initial' or 'Replacement' and the placement date.
		///		</para>
		/// </summary>
		public bool IsProsthesis;

		/// <summary>
		///		<para>
		///			A value indicating whether this is a radiology procedure.
		///		</para>
		///		<para>
		///			An EHR core measure uses this flag to help determine the denominator for rad orders.
		///		</para>
		/// </summary>
		public bool IsRadiology;

		/// <summary>
		/// A value indicating whether the procedure represents a hygiene procedure.
		/// </summary>
		public bool IsHygiene;

		/// <summary>
		///		<para>
		///			A value indicating whether the procedure code is only used as an adjunct to 
		///			track the lab fee.
		///		</para>
		///		<para>
		///			<b>Only used in Canada.</b>
		///		</para>
		/// </summary>
		public bool IsCanadianLab;

		/// <summary>
		///		<para>
		///			A value indicating whether sales tax should be added for this procedure.
		///		</para>
		///		<para>
		///			Used by some offices even though no user interface built yet. 
		///		</para>	
		///		<para>
		///			The <b>SalesTaxPercentage</b> preference has been added to store the amount 
		///			of sales tax to apply as an adjustment attached to a procedurelog entry.
		///		</para>
		/// </summary>
		public bool IsTaxed;

		/// <summary>
		/// A value indicating whether the procedure takes multiple visits (i.o. multiple 
		/// appointments) to complete.
		/// </summary>
		public bool IsMultiVisit;

		/// <summary>
		/// For Medicaid. There may be more later.
		/// </summary>
		public string AlternateCode1;

		/// <summary>
		///		<para>
		///			The medical code related to this procedure code. Anytime a procedure is added, 
		///			this medical code will also be added to that procedure.
		///		</para>
		///		<para>
		///			There is no foreign key constraint defined for this field, that means that the 
		///			code this field refers to might not exist in the database.
		///		</para>
		/// </summary>
		[ForeignKey(typeof(ProcedureCode), nameof(Code)), Nullable]
		public string MedicalCode;


		public ToothPaintingType PaintType;

		/// <summary>
		/// If set to anything but 0, then this will override the graphic color for all procedures of this code, regardless of the status.
		/// </summary>
		public Color GraphicColor;

		/// <summary>
		/// When creating treatment plans, this description will be used instead of the technical description.
		/// </summary>
		public string LaymanTerm;

		/// <summary>
		/// Support for Base Units for a Code (like anesthesia).  Should normally be zero.
		/// </summary>
		public int BaseUnits;

		/// <summary>
		/// FK to procedurecode.ProcCode.
		/// Used for posterior composites because insurance substitutes the amalgam code when figuring the coverage.
		/// </summary>
		[ForeignKey(typeof(ProcedureCode), nameof(Code)), Nullable]
		public string SubstitutionCode;

		/// <summary>
		/// Enum:SubstitutionCondition Used so that posterior composites only substitute if tooth is molar.
		/// Ins usually pays for premolar composites.
		/// </summary>
		public SubstitutionCondition SubstitutionCondition;

		/// <summary>
		/// 11 digits or blank, enforced. For 837I
		/// </summary>
		[Column("drug_ndc")]
		public string DrugNDC;

		/// <summary>
		/// Gets copied to procedure.RevCode.  For 837I
		/// </summary>
		public string RevenueCodeDefault;

		/// <summary>
		///		<para>
		///			The ID of the provider to use for this code. If null the normal provider is used instead.
		///		</para>
		/// </summary>
		[ForeignKey(typeof(Provider), nameof(Provider.Id))]
		public long? DefaultProviderId;

		/// <summary>
		///		<para>
		///			For Canadian users; tracks scaling insurance and periodontal scaling units for
		///			patients depending on coverage.
		///		</para>
		///		<para>
		///			<b>Only used in Canada.</b>
		///		</para>
		/// </summary>
		public double CanadaTimeUnits;

		/// <summary>
		/// The default procedure note to copy when marking complete.
		/// </summary>
		public string DefaultNote;

		/// <summary>
		/// The note to attach to a claim when creating a claim that includes this procedure.
		/// </summary>
		[Column("default_note_claim")]
		public string DefaultNoteForClaim;

		/// <summary>
		/// The note to attach to a treatment plan when adding this procedure to a treatment plan.
		/// </summary>
		[Column("default_note_tp")]
		public string DefaultNoteForTreatmentPlan;

		[Column(AutoGenerated = true)]
		public DateTime LastModifiedDate;


		///<summary>Enum:BypassLockStatus Specifies whether a proceduce with this code can be created before the global lock date. The only values that
		///should be used for this field are NeverBypass and BypassIfZero.</summary>
		public BypassLockStatus BypassGlobalLock;

		public ProcedureCode()
		{
			Time = "/X/";

			GraphicColor = Color.Black;
		}

		//[Ignore]
		//private string procCatDescript;

		//[XmlIgnore]
		//public string ProcCatDescript
		//{
		//	get
		//	{
		//		if (ProcedureCategory == 0)
		//		{
		//			return procCatDescript;
		//		}
		//		return Definitions.GetName(DefinitionCategory.ProcCodeCats, ProcedureCategory);
		//	}
		//	set
		//	{
		//		procCatDescript = value;
		//	}
		//}

		public ProcedureCode Copy() => (ProcedureCode)MemberwiseClone();
	}

	public enum ToothPaintingType
	{
		None,
		Extraction,
		Implant,
		RCT,
		PostBU,
		FillingDark,
		FillingLight,
		CrownDark,
		CrownLight,
		BridgeDark,
		BridgeLight,
		DentureDark,
		DentureLight,
		Sealant,
		Veneer,
		Watch
	}

	/// <summary>
	///		<para>
	///			Identifies the treatment area of a procedure.
	///		</para>
	/// </summary>
	public enum ProcedureTreatmentArea
	{
		None,
		Surface,
		Tooth,
		Mouth,
		Quad,
		Sextant,
		Arch,
		ToothRange
	}


	///<summary>The conditions when the global lock date can be bypassed.</summary>
	public enum BypassLockStatus
	{
		///<summary>0 - Never bypass the lock date.</summary>
		NeverBypass,

		///<summary>1 - Bypass the lock date if the fee is zero.</summary>
		BypassIfZero,

		///<summary>2 - Always bypass the global lock date.</summary>
		BypassAlways
	}

	/// <Summary>
	/// Used for insurance substitutions conditions of procedurecodes. 
	/// Mostly for posterior composites.
	/// </Summary>
	public enum SubstitutionCondition
	{
		Always,
		Molar,
		SecondMolar,
		Never,
		Posterior
	}
}
