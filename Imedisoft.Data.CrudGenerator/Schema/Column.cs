using Imedisoft.Data.Annotations;
using System;
using System.Reflection;

namespace Imedisoft.Data.CrudGenerator.Schema
{
    public class Column
    {
        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the column is part of the primary key.
        /// </summary>
        public bool IsPrimaryKey { get; }

        /// <summary>
        /// Gets the column data type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Column"/> class.
        /// </summary>
        /// <param name="propertyInfo">The field that represents the column.</param>
        internal Column(FieldInfo fieldInfo)
        {
            var columnAttribute = fieldInfo.GetCustomAttribute<ColumnAttribute>();

            FieldName = fieldInfo.Name;
            Name = columnAttribute?.Name ?? FieldName;

            IsPrimaryKey = fieldInfo.GetCustomAttribute<PrimaryKeyAttribute>() != null;

            Type = fieldInfo.FieldType;
        }

        /// <summary>
        /// Returns a string representation of the column.
        /// </summary>
        public override string ToString() => $"{Name} ({Type.FullName})";
    }
}
