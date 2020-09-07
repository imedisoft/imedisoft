using Imedisoft.Data.Annotations;
using System;
using System.Collections;

namespace OpenDentBusiness
{
	/// <summary>
	/// Spell check custom dictionary, shared by the whole office.
	/// </summary>
	[Table]
	public class DictCustom : TableBase
	{
		[PrimaryKey]
		public long DictCustomNum;

		/// <summary>No space or punctuation allowed.</summary>
		public string WordText;

		public DictCustom Copy()
		{
			return (DictCustom)MemberwiseClone();
		}
	}
}
