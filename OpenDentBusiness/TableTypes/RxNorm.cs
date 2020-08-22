using Imedisoft.Data.Annotations;
using System;

namespace OpenDentBusiness
{
    [Table("rx_norms")]
	public class RxNorm : TableBase
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// RxNorm Concept universal ID. Throughout the program, this is actually used as the Primary Key of this table rather than the RxNormNum.
		/// </summary>
		public string RxCui;

		/// <summary>
		/// Multum code. 
		/// Only used for crosscoding during import/export with electronic Rx program. 
		/// User cannot see multum codes. 
		/// Most of the rows in this table do not have an MmslCode and user searches ignore rows with an MmslCode.
		/// </summary>
		public string MmslCode;

		/// <summary>
		/// Only used for RxNorms, not Multums.
		/// </summary>
		public string Description;
	}
}
