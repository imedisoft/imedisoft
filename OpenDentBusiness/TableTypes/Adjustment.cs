using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using System;
using System.Collections;

namespace OpenDentBusiness
{
    /// <summary>
    /// An adjustment in the patient account.
    /// Usually, adjustments are very simple, just being assigned to one patient and provider.
    /// But they can also be attached to a procedure to represent a discount on that procedure.
    /// Attaching adjustments to procedures is not automated, so it is not very common.
    /// </summary>
	[Table("adjustments")]
	public class Adjustment
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(Patient), nameof(Patient.PatNum))]
		public long PatientId;

		/// <summary>
		/// The date that the adjustment shows in the patient account.
		/// </summary>
		public DateTime AdjustDate;

		/// <summary>
		/// Amount of adjustment. Can be positive or negative.
		/// </summary>
		public double AdjustAmount;

		/// <summary>
		/// The ID of a definition in the <see cref="DefinitionCategory.AdjTypes"/> category.
		/// </summary>
		[ForeignKey(typeof(Definition), nameof(Definition.Id))]
		public long Type;

		[ForeignKey(typeof(Provider), nameof(Provider.ProvNum))]
		public long ProviderId;

		/// <summary>
		/// Note for this adjustment.
		/// </summary>
		public string Note;

		/// <summary>
		///  Only used if attached to a procedure.  Otherwise, null.
		/// </summary>
		public long? ProcedureId;

		/// <summary>
		/// Procedure date. Not when the adjustment was entered.
		/// </summary>
		public DateTime ProcedureDate;

		[ForeignKey(typeof(Clinic), nameof(Clinic.Id))]
		public long ClinicId;

		/// <summary>
		/// Only used when the statement is an invoice.
		/// </summary>
		[ForeignKey(typeof(Statement), nameof(Statement.StatementNum))]
		public long StatementId;

		/// <summary>
		/// The date on which the adjustment was created.
		/// </summary>
		[Column(ReadOnly = true)]
		public DateTime AddedDate;

		///<summary>The ID of the user that created the adjustment.</summary>
		[Column(ReadOnly = true), ForeignKey(typeof(Userod), nameof(Userod.Id))]
		public long AddedByUserId;

		/// <summary>
		/// The date on which the adjustment was last modified.
		/// </summary>
		public DateTime LastModifiedDate;

		public Adjustment Clone() => (Adjustment)MemberwiseClone();
	}
}
