using System;

namespace Imedisoft.Data.Annotations
{
    /// <summary>
    /// Used to explicitly identify properties that should be nullable by the CRUD generator.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class NullableAttribute : Attribute
    {
    }
}
