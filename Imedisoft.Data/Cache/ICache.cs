using System;
using System.Collections.Generic;

namespace Imedisoft.Data.Cache
{
    /// <summary>
    /// Interface for data caches.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Gets a value indicating whether the cache is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Returns the number of cache entries.
        /// </summary>
        /// <returns>The number of cache entries.</returns>
        int Count();

        /// <summary>
        /// Refreshes the contents of the cache by reloaded data from the database and returns the new entries as a list.
        /// </summary>
        /// <returns>A list containing the refreshed entries.</returns>
        void Refresh();
    }

    /// <summary>
    /// Provides a cache for values of type <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the values stored in the cache.</typeparam>
    public interface ICache<TValue> : ICache
    {
        /// <summary>
        /// Finds all cache entries matching a given condition.
        /// </summary>
        /// <param name="predicate">A function to test each cache entry for a condition.</param>
        /// <returns>All cache entires matching the condition.</returns>
        List<TValue> Find(Predicate<TValue> predicate);

        /// <summary>
        /// Returns the number of cache entries matching a given condition.
        /// </summary>
        /// <param name="predicate">A function to test each cache entry for a condition.</param>
        /// <returns>The number of cache entries.</returns>
        int Count(Predicate<TValue> predicate);

        /// <summary>
        /// Gets a shallow copy of all cache entries.
        /// </summary>
        /// <returns>All cache entries.</returns>
        List<TValue> GetAll();

        /// <summary>
        /// Gets the first entry that matches the specified condition.
        /// </summary>
        /// <param name="predicate">A function to test each cache entry for a condition.</param>
        /// <returns>The first entry that matched the given condition.</returns>
        TValue FirstOrDefault(Predicate<TValue> predicate);
    }
}
