using System;
using System.Collections;

namespace OpenDentBusiness
{
	///<summary>Ehr</summary>
	[Serializable]
	public class ReminderRule : TableBase
	{
		[CrudColumn(IsPriKey = true)]
		public long ReminderRuleNum;

		///<summary>Enum:EhrCriterion Problem,Medication,Allergy,Age,Gender,LabResult.</summary>
		public EhrCriterion ReminderCriterion;

		///<summary>Foreign key to disease.DiseaseDefNum, medicationpat.MedicationNum, or allergy.AllergyDefNum. Will be 0 if Age, Gender, or LabResult are the trigger.</summary>
		public long CriterionFK;

		///<summary>Only used if Age, Gender, or LabResult are the trigger. Examples: "&lt;25"(must include &lt; or &gt;), "Male"/"Female", "INR" (the simple description of the lab test)</summary>
		public string CriterionValue;

		///<summary>Text that will show as the reminder.</summary>
		public string Message;
	}

	public enum EhrCriterion
	{
		///<summary>Shows as 'problem' because it needs to be human readable.</summary>
		Problem,
		Medication,
		Allergy,
		Age,
		Gender,
		LabResult,
	}
}