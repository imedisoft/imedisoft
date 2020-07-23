using Imedisoft.Data.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// Corresponds to the autocodeitem table in the database.
    /// There are multiple AutoCodeItems for a given AutoCode.
    /// Each Item has one ADA code.
    /// </summary>
    [Table("auto_code_items")]
	public class AutoCodeItem : TableBase
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(AutoCode), nameof(AutoCode.Id))]
		public long AutoCodeId;

		/// <summary>
		/// Do not use
		/// </summary>
		[Obsolete, Ignore]
		public string OldCode;

		[ForeignKey(typeof(ProcedureCode), nameof(ProcedureCode.CodeNum))]
		public long ProcedureCodeId;

		/// <summary>
		/// Only used in the validation section when closing FormAutoCodeEdit. 
		/// Will normally be empty.
		/// </summary>
		[Ignore]
		public List<AutoCodeCond> ListConditions;
	}
}
