using Imedisoft.Data.Annotations;
using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Vaccines administered.
    /// </summary>
    [Table]
	public class Cvx
	{
		[PrimaryKey]
		public long Id;

		[Column(ReadOnly = true)]
		public string Code;

		public string Description;

		[Ignore, Obsolete]
		public string IsActive;
	}
}