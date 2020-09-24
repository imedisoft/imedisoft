using System;

namespace OpenDentBusiness
{
    ///<summary>When doing a lettermerge, a data file is created with certain fields.  This is a list of those fields for each lettermerge.</summary>
    [Serializable]
	public class LetterMergeField : TableBase
	{
		[CrudColumn(IsPriKey = true)]
		public long FieldNum;

		///<summary>FK to lettermerge.LetterMergeNum.</summary>
		public long LetterMergeNum;

		///<summary>One of the preset available field names.</summary>
		public string FieldName;

		public LetterMergeField Copy()
		{
			return (LetterMergeField)MemberwiseClone();
		}
	}
}
