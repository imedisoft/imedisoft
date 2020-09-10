using Imedisoft.Data.Annotations;
using System;

namespace Imedisoft.Data.Models
{
    /// <summary>
    /// An allergy definition.
    /// Gets linked to an allergy and patient.
    /// Allergies will not show in CCD messages unless they have a valid Medication (that has an RxNorm) or UniiCode.
    /// </summary>
    [Table("allergy_defs")]
	public class AllergyDef
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// Name of the drug. User can change this.
		/// If an RxCui is present, the RxNorm string can be pulled from the in-memory table for UI display in addition to the Description.
		/// </summary>
		public string Description;

		/// <summary>
		/// A value indicating whether the row has been hidden.
		/// </summary>
		public bool IsHidden;

		/// <summary>
		/// The date on which the row was last modified. Not user editable.
		/// </summary>
		public DateTime LastModifiedDate;

		[Nullable]
		public string SnomedCode;

		/// <summary>
		/// Optional, only used with CCD messages.
		/// </summary>
		[ForeignKey(typeof(Medication), nameof(Medication.Id))]
		public long? MedicationId;

		/// <summary>
		/// The Unii code for the Allergen. Optional, but there must be either a <see cref="MedicationId"/> or a UniiCode.  
		/// Used to create CCD in FormSummaryOfCare, or set during CCD allergy reconcile.
		/// </summary>
		public string UniiCode;

		/// <summary>
		/// Returns a string representation of the allergy definition.
		/// </summary>
		public override string ToString() => Description;
    }
}
