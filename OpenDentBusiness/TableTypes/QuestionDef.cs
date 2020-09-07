using System;
using System.Collections;

namespace OpenDentBusiness
{
	/// <summary>
	/// Each row represents one question on the medical history questionnaire. 
	/// Later, other questionnaires will be allowed, but for now, all questions are on one questionnaire for the patient. 
	/// This table has no dependencies, since the question is copied when added to a patient record. 
	/// Any row can be freely deleted or altered without any problems.
	/// </summary>
	[Serializable]
	public class QuestionDef : TableBase
	{
		[CrudColumn(IsPriKey = true)]
		public long QuestionDefNum;

		///<summary>The question as presented to the patient.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.TextIsClob)]
		public string Description;

		///<summary>The order that the Questions will show.</summary>
		public int ItemOrder;
		///<summary>Enum:Question
		///Type</summary>
		public QuestionType QuestType;
	}
}
