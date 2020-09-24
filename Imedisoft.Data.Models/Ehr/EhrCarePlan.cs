using Imedisoft.Data.Annotations;
using System;

namespace Imedisoft.Data.Models
{
    [Table("ehr_care_plans")]
	public class EhrCarePlan
	{
		[PrimaryKey]
		public long Id;

		public long PatientId;

		/// <summary>
		/// Snomed code describing the type of educational instruction provided.
		/// Limited to terms descending from the Snomed 409073007 (Education Hierarchy).
		/// </summary>
		public string SnomedEducation;

		/// <summary>
		/// Instructions provided to the patient.
		/// </summary>
		public string Instructions;

		/// <summary>
		/// This field does not help much with care plan instructions, but will be more helpful for
		/// other types of care plans if we expand in the future (for example, planned procedures). 
		/// We also saw examples where this date was included in the human readable part of a CCD, 
		/// but not in the machine readable part.
		/// </summary>
		public DateTime? DatePlanned;
	}
}
