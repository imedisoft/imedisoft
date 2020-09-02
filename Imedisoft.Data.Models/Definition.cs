using Imedisoft.Data.Annotations;
using System.Drawing;
using System.Text.Json;

namespace Imedisoft.Data.Models
{
	/// <summary>
	///		<para>
	///			Represents a definition. Every definition has a name and optionally a value, 
	///			taxonomy and/or a color. Definitions are used by many parts of the program.
	///		</para>
	/// </summary>
    [Table("definitions")]
	public class Definition
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The definition category.
		/// </summary>
		/// <seealso cref="DefinitionCategory"/>
		public string Category;

		/// <summary>
		/// The name of the item.
		/// </summary>
		public string Name;

		/// <summary>
		/// The (optional) value of the item. Can be used to store extra information.
		/// </summary>
		public string Value;

		/// <summary>
		/// Gets the value of the definition as a instance of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type to deserialize the value into.</typeparam>
		/// <returns>The value deserialized as the given type.</returns>
		/// <exception cref="JsonException"></exception>
		public T GetValueAs<T>() => JsonSerializer.Deserialize<T>(Value ?? "");

		/// <summary>
		/// Sets the value of the definition to the specified value.
		/// </summary>
		/// <param name="value">The new value of the definition.</param>
		public void SetValue(object value) => Value = JsonSerializer.Serialize(value);

		/// <summary>
		/// The (optional) taxonomy code of the item.
		/// </summary>
		public string Taxonomy;

		/// <summary>
		/// The (optional) color of the item.
		/// </summary>
		public Color Color;

		/// <summary>
		/// Order that each item shows on various lists. 0-indexed.
		/// </summary>
		public int SortOrder;

		/// <summary>
		/// A  value indicating whether the definition is hidden.
		/// </summary>
		public bool IsHidden;

		/// <summary>
		/// Returns a string representation of the definition.
		/// </summary>
		public override string ToString() => Name;
	}
}
