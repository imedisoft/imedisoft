using System;

namespace Imedisoft.Data.Annotations
{
	/// <summary>
	/// Used to explicitly set the column name or type if they differ from the property.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public sealed class ColumnAttribute : Attribute
    {
		/// <summary>
		/// Gets or sets the column name.
		/// </summary>
		public string Name { get; set; }
	}
}
