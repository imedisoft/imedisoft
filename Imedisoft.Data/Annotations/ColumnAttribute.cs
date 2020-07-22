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

		/// <summary>
		///		<para>
		///			Gets or sets a value indicating whether the column is read only.
		///		</para>
		///		<para>
		///			If <b>true</b> the column is excluded from all UPDATE statements.
		///		</para>
		/// </summary>
		public bool ReadOnly { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ColumnAttribute"/> class.
		/// </summary>
		public ColumnAttribute()
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ColumnAttribute"/> class.
		/// </summary>
		/// <param name="name">The column name.</param>
		public ColumnAttribute(string name)
        {
			Name = name;
        }
	}
}
