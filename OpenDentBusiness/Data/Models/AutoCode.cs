using Imedisoft.Data.Annotations;
using System;
using System.Collections;

namespace OpenDentBusiness
{
    /// <summary>
    /// An autocode automates entering procedures.
    /// The user only has to pick composite, for instance, and the autocode figures out the code based on the number of surfaces, and posterior vs. anterior.
    /// Autocodes also enforce and suggest changes to a procedure code if the number of surfaces or other properties change.
    /// </summary>
    [Table("auto_codes")]
	public class AutoCode : TableBase
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// Displays meaningful decription, like "Amalgam".
		/// </summary>
		public string Description;

		/// <summary>
		/// A value indicating whether the autocode has been hidden.
		/// </summary>
		public bool IsHidden;

		/// <summary>
		/// This will be true if user no longer wants to see this autocode message when closing a procedure.
		/// This makes it less intrusive, but it can still be used in procedure buttons.
		/// </summary>
		public bool LessIntrusive;
	}
}
