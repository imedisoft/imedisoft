using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Source of Payment Typology. 
    /// Used by EHR. 
    /// Examples: Medicaid, MedicaidPPO, NoFee, etc.
    /// About 100 defined by govt. 
    /// Other tables generally use the SopCode as their foreign key.
    /// </summary>
	public class Sop : TableBase
	{
		[CrudColumn(IsPriKey = true)]
		public long SopNum;

		/// <summary>
		/// Sop code. Not allowed to edit this column once saved in the database.  Examples: 121, 3115, etc.
		/// </summary>
		public string SopCode;

		/// <summary>
		/// Description provided by Sop documentation.  Examples: Medicare FFS, TRICARE Reserve Select
		/// </summary>
		public string Description;

		public Sop Copy()
		{
			return (Sop)MemberwiseClone();
		}
	}
}
