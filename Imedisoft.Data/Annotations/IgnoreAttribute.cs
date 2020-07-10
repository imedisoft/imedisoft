using System;

namespace Imedisoft.Data.Annotations
{
    /// <summary>
    /// Used to identify properties that should be ignored by the CRUD generator.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class IgnoreAttribute : Attribute
    {
    }
}
