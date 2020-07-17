using System;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data.Cache
{
    public abstract class CacheBase<T>
    {
		private readonly List<T> items = new List<T>();

		/// <summary>
		/// Gets a value indicating whether the cache is empty.
		/// </summary>
		public bool IsEmpty => items.Count == 0;

		/// <summary>
		/// Finds all cache entries matching a given condition.
		/// </summary>
		/// <param name="predicate">A function to test each cache entry for a condition.</param>
		/// <returns>All cache entires matching the condition.</returns>
		public IEnumerable<T> Find(Predicate<T> predicate)
        {
			if (predicate == null || IsEmpty) yield break;
			
			lock (items)
			{
				foreach (var item in items)
				{
					if (predicate(item))
					{
						yield return item;
					}
				}
			}
        }

		/// <summary>
		/// Returns the number of cache entries.
		/// </summary>
		/// <returns>The number of cache entries.</returns>
		public int Count() => items.Count;

		/// <summary>
		/// Returns the number of cache entries matching a given condition.
		/// </summary>
		/// <param name="predicate">A function to test each cache entry for a condition.</param>
		/// <returns>The number of cache entries.</returns>
		public int Count(Predicate<T> predicate)
        {
			if (predicate == null || IsEmpty) return 0;

			int matches = 0;

			lock (items)
			{
				foreach (var item in items)
				{
					if (predicate(item))
					{
						matches++;
					}
				}
			}

			return matches;
		}

		/// <summary>
		/// Refreshes the contents of the cache by reloaded data from the database and returns the new entries as a list.
		/// </summary>
		/// <returns>A list containing the refreshed entries.</returns>
		public List<T> Refresh()
        {
			var entries = Load().ToList();

			lock (items)
			{
				items.Clear();

				if (entries != null)
				{
					items.AddRange(entries);
				}
			}

			return entries;
        }

		/// <summary>
		/// Loads cache content from the database.
		/// </summary>
		protected abstract IEnumerable<T> Load();

		/// <summary>
		/// Enumerates all cache entries.
		/// </summary>
		public IEnumerable<T> All
		{
			get
			{
				lock (items)
				{
					foreach (var item in items) yield return item;
				}
			}
		}

		/// <summary>
		/// Gets the first entry that matches the specified condition.
		/// </summary>
		/// <param name="predicate">A function to test each cache entry for a condition.</param>
		/// <returns>The first entry that matched the given condition.</returns>
		public T FirstOrDefault(Predicate<T> predicate)
        {
			if (predicate == null) return default;

			lock (items)
            {
				foreach (var item in items)
                {
					if (predicate(item)) return item;
                }
            }

			return default;
        }
    }
}
