using Imedisoft.Data;
using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using System;
using System.Collections;

namespace OpenDentBusiness
{
	/// <summary>
	/// Many-to-many relationship connecting Rx with DiseaseDef, AllergyDef, or Medication. 
	/// Only one of those links may be specified in a single row; the other two will be NULL.
	/// </summary>
	[Table("rx_alerts")]
	public class RxAlert : TableBase
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// This alert is to be shown when user attempts to write an Rx for this RxDef.
		/// </summary>
		[ForeignKey(typeof(RxDef), nameof(RxDef.Id))]
		public long RxDefId;

		/// <summary>
		/// Only if DrugProblem interaction. 
		/// This is compared against disease.DiseaseDefNum using PatNum. 
		/// Drug-Problem (they call it Drug-Diagnosis) checking is also performed in NewCrop.
		/// </summary>
		[ForeignKey(typeof(ProblemDefinition), nameof(ProblemDefinition.Id))]
		public long DiseaseDefId;

		/// <summary>
		/// Only if DrugAllergy interaction.
		/// Compared against allergy.AllergyDefNum using PatNum.
		/// Drug-Allergy checking is also perfomed in NewCrop.
		/// </summary>
		[ForeignKey(typeof(AllergyDef), nameof(AllergyDef.Id))]
		public long AllergyDefId;

		/// <summary>
		/// Only if DrugDrug interaction.
		/// This will be compared against medicationpat.MedicationNum using PatNum.
		/// Drug-Drug checking is also performed in NewCrop.
		/// </summary>
		[ForeignKey(typeof(Medication), nameof(Medication.MedicationNum))]
		public long MedicationId;

		/// <summary>
		/// This is typically blank, so a default message will be displayed by OD.
		/// But if this contains a message, then this message will be used instead.
		/// </summary>
		public string NotificationMsg;

		/// <summary>
		/// False by default.
		/// Set to true to flag the drug-drug or drug-allergy intervention as high significance.
		/// </summary>
		public bool IsHighSignificance;

        public override string ToString()
        {
			if (DiseaseDefId > 0)
			{
				return ProblemDefinitions.GetName(DiseaseDefId);
			}

			if (AllergyDefId > 0)
			{
				var allergyDef = AllergyDefs.GetOne(AllergyDefId);

				if (allergyDef != null)
				{
					return allergyDef.Description;
				}
			}

			if (MedicationId > 0)
			{
				var medication = Medications.GetMedication(MedicationId);
				if (medication != null)
				{
					return medication.MedName;
				}
			}

			return base.ToString();
		}
    }
}
