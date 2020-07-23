using Imedisoft.Data.Annotations;
using System;
using System.Collections;

namespace OpenDentBusiness
{
	/// <summary>
	/// AutoCode condition. 
	/// Always attached to an AutoCodeItem, which is then, in turn, attached to an autocode.
	/// There is usually only one or two conditions for a given AutoCodeItem.
	/// </summary>
	[Table("auto_code_conditions")]
	public class AutoCodeCond : TableBase
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(AutoCodeItem), nameof(AutoCodeItem.Id))]
		public long AutoCodeItemId;

		/// <summary>
		/// Enum:AutoCondition
		/// </summary>
		public AutoCondition Cond;
	}
}
