using System;

namespace Imedisoft.Data.Cache
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CacheGroupAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the cache group.
        /// </summary>
        public string GroupName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheGroupAttribute"/> class.
        /// </summary>
        /// <param name="categoryName">The name of the category.</param>
        public CacheGroupAttribute(string categoryName)
        {
            GroupName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
        }
    }
}
