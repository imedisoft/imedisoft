using System;

namespace Imedisoft.Data.Annotations
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class PrimaryKeyAttribute : Attribute
    {
    }
}
