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
	[Table]
	public class AutoCodeCond : TableBase
	{
		[PrimaryKey]
		public long AutoCodeCondNum;

		/// <summary>
		/// FK to autocodeitem.AutoCodeItemNum.
		/// </summary>
		public long AutoCodeItemNum;

		/// <summary>
		/// Enum:AutoCondition
		/// </summary>
		public AutoCondition Cond;

		public AutoCodeCond Copy() => (AutoCodeCond)MemberwiseClone();
	}
}
