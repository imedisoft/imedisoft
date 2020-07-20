using System;

namespace Imedisoft.Data.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class TableAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether records in this table may be modified.
        /// </summary>
        public bool AllowUpdate { get; set; } = true;

        /// <summary>
        /// Gets or set a value indicating whether records may be deleted from this table.
        /// </summary>
        public bool AllowDelete { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableAttribute"/> class.
        /// </summary>
        public TableAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableAttribute"/> class.
        /// </summary>
        /// <param name="name">The table name.</param>
        public TableAttribute(string name)
        {
            Name = name;
        }
    }
}
