using Imedisoft.Data.Annotations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Imedisoft.Data.CrudGenerator.Schema
{
    public class Table
    {
        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type on which this table is based.
        /// </summary>
        public Type Type {get;}

        /// <summary>
        /// Gets the table columns.
        /// </summary>
        public List<Column> Columns { get; } = new List<Column>();

        /// <summary>
        /// Gets the primary key of the column.
        /// </summary>
        public Column PrimaryKey { get; }

        /// <summary>
        /// Gets or sets a value indicating whether records in this table may be modified.
        /// </summary>
        public TableOperations AllowedOperations { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="type">The type that represents the table.</param>
        /// <exception cref="ArgumentException">
        ///     If the specified type does not have a <see cref="TableAttribute"/> attribute.
        /// </exception>
        public Table(Type type)
        {
            Type = type;

            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            if (tableAttribute == null)
            {
                throw new ArgumentException(
                    $"The type '{type.FullName}' does is missing the '{typeof(TableAttribute).FullName}' attribute.", nameof(type));
            }

            Name = (tableAttribute.Name ?? type.Name).ToLower();
            AllowedOperations = tableAttribute.AllowedOperations;

            var primaryKeys = new List<Column>();

            var properties = type.GetFields();

            foreach (var property in properties)
            {
                var ignore = property.GetCustomAttribute<IgnoreAttribute>();
                if (ignore != null)
                {
                    continue;
                }

                var column = new Column(property);
                if (column.IsPrimaryKey)
                {
                    primaryKeys.Add(column);
                }

                Columns.Add(column);
            }

            if (primaryKeys.Count > 0)
            {
                if (primaryKeys.Count > 1) 
                    throw new Exception(
                        $"The type '{type.FullName}' has multiple primary keys. " +
                        "Composite primary keys are not supported.");

                PrimaryKey = primaryKeys[0];
            }
        }
    }
}
