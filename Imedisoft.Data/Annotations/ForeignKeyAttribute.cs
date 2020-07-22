using System;

namespace Imedisoft.Data.Annotations
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ForeignKeyAttribute : Attribute
    {
        /// <summary>
        /// Gets the type references by the foreign key.
        /// </summary>
        public Type ForeignType { get; }

        /// <summary>
        /// Gets the name of the foreign field references by the foreign key.
        /// </summary>
        public string ForeignFieldName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyAttribute"/> class.
        /// </summary>
        /// <param name="foreignType">The type references by the foreign key.</param>
        /// <param name="foreignFieldName">The name of the field in the reference type.</param>
        public ForeignKeyAttribute(Type foreignType, string foreignFieldName)
        {
            ForeignType = foreignType;
            ForeignFieldName = foreignFieldName;
        }
    }
}
