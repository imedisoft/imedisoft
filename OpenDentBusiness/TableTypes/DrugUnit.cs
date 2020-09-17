using Imedisoft.Data.Annotations;
using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness
{
	/// <summary>
	/// And other kinds of units. We will only prefill this list with units needed for the tests. 
	/// Users would have to manually add any other units.
	/// </summary>
	[Table("drug_units")]
	public class DrugUnit : TableBase
	{
		[PrimaryKey]
		public long Id;

		///<summary>Example ml, capitalization not critical. Usually entered as lowercase except for L.</summary>
		public string UnitIdentifier;//VARCHAR(20)/VARCHAR2(20).

		///<summary>Example milliliter.</summary>
		public string UnitText;

		public DrugUnit Copy()
		{
			return (DrugUnit)MemberwiseClone();
		}
	}
}