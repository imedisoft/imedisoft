namespace Imedisoft.Data
{
    /// <summary>
    ///     <para>
    ///         Represents a data element with a description of the element.
    ///     </para>
    /// </summary>
    /// <typeparam name="TValue">The data element type.</typeparam>
    public class DataItem<TValue>
    {
        /// <summary>
        /// The value of the data item.
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// A description of the data item.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataItem"/> class.
        /// </summary>
        /// <param name="value">The data item value.</param>
        /// <param name="description">A description of the data item.</param>
        public DataItem(TValue value, string description)
        {
            Value = value;
            Description = description ?? "";
        }

        /// <summary>
        /// Returns a string representation of the data item.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Description;
    }
}
