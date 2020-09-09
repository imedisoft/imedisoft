using System;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data.Cache
{
    public abstract class ListCache<TValue> : ICache<TValue>
    {
		private readonly List<TValue> items = new List<TValue>();
		private bool requiresRefresh = true;
		private bool isInitialized;

		/// <summary>
		/// Initializes a new instance of the <see cref="ListCache{TValue}"/> class.
		/// </summary>
		public ListCache() => CacheManager.Register(this);

		/// <summary>
		/// Gets a value indicating whether the cache is empty.
		/// </summary>
		public bool IsEmpty => items.Count == 0;

		/// <summary>
		/// Finds all cache entries matching a given condition.
		/// </summary>
		/// <param name="predicate">A function to test each cache entry for a condition.</param>
		/// <returns>All cache entires matching the condition.</returns>
		public List<TValue> Find(Predicate<TValue> predicate)
        {
			var result = new List<TValue>();

			if (requiresRefresh) Refresh();

			if (predicate != null && !IsEmpty)
			{
				lock (items)
				{
					result.AddRange(items.Where(item => predicate(item)));
				}
			}

			return result;
		}

		/// <summary>
		/// Removes the specified item from the cache.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <returns>True if the item was removed; otherwise, false.</returns>
		public bool Remove(TValue item)
        {
            lock (items)
            {
				return items.Remove(item);
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
		public int Count(Predicate<TValue> predicate)
        {
			if (predicate == null || IsEmpty) return 0;

			if (requiresRefresh) Refresh();

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
		public void Refresh()
        {
			if (!isInitialized)
            {
				Initialize();

				isInitialized = true;
			}

			var entries = Load().ToList();

			lock (items)
			{
				items.Clear();

				if (entries != null)
				{
					items.AddRange(entries);
				}
			}

			requiresRefresh = false;
		}

		/// <summary>
		/// Loads cache content from the database.
		/// </summary>
		protected abstract IEnumerable<TValue> Load();

		/// <summary>
		/// Gets a shallow copy of all cache entries.
		/// </summary>
		/// <returns>All cache entries.</returns>
		public List<TValue> GetAll()
		{
			if (requiresRefresh) Refresh();

			lock (items)
			{
				return new List<TValue>(items);
			}
		}

		/// <summary>
		/// Gets the first entry in the cache, or a default value if the cache is empty.
		/// </summary>
		/// <returns>The first entry in the cache; or the default value.</returns>
		public TValue FirstOrDefault()
        {
			lock (items)
            {
				return items.FirstOrDefault();
            }
        }

		/// <summary>
		/// Gets the first entry that matches the specified condition.
		/// </summary>
		/// <param name="predicate">A function to test each cache entry for a condition.</param>
		/// <returns>The first entry that matched the given condition.</returns>
		public TValue FirstOrDefault(Predicate<TValue> predicate)
        {
			if (predicate == null) return default;

			if (requiresRefresh) Refresh();

			lock (items)
            {
				foreach (var item in items)
                {
					if (predicate(item)) return item;
                }
            }

			return default;
        }

		/// <summary>
		/// Gets the first entry that matches the specified condition.
		/// </summary>
		/// <param name="predicate">A function to test each cache entry for a condition.</param>
		/// <returns>The first entry that matched the given condition.</returns>
		public TValue LastOrDefault(Predicate<TValue> predicate)
		{
			if (predicate == null) return default;

			if (requiresRefresh) Refresh();

			lock (items)
			{
				return items.LastOrDefault(item => predicate(item));
			}
		}

		/// <summary>
		/// Determines whether the cache contains any element that satisfies the given condition.
		/// </summary>
		/// <param name="predicate">A function to test each cache entry for a condition.</param>
		/// <returns>True if any element matches the givencondition; otherwise, false.</returns>
		public bool Any(Predicate<TValue> predicate)
        {
			if (predicate == null) return false;

			if (requiresRefresh) Refresh();

			lock (items)
			{
				foreach (var item in items)
				{
					if (predicate(item)) return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Called the first time the cache is refreshed (i.o. on the first load).
		/// </summary>
		protected virtual void Initialize()
        {
        }
    }
}
