using System;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data.Cache
{
    public abstract class DictionaryCache<TKey, TValue> : ICache<TValue>
    {
        private readonly Dictionary<TKey, TValue> items = new Dictionary<TKey, TValue>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Dictionary{TKey, TValue}"/> class.
		/// </summary>
		public DictionaryCache() => CacheManager.Register(this);

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

			if (predicate != null && !IsEmpty)
			{
				lock (items)
				{
					result.AddRange(items.Values.Where(item => predicate(item)));
				}
			}

			return result;
		}

		/// <summary>
		/// Finds the cache entry with the specified <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key of the cache entry.</param>
		/// <returns>The cache entry with the specified key.</returns>
		public TValue Find(TKey key)
		{
			if (IsEmpty) return default;

			lock (items)
			{
				if (items.TryGetValue(key, out var value))
                {
					return value;
                }
			}

			return default;
		}

		/// <summary>
		/// Finds the cache entry with the specified <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key of the cache entry.</param>
		/// <returns>The cache entry with the specified key.</returns>
		public TValue this[TKey key] => Find(key);

		/// <summary>
		/// Determines whether the cache contains a entry with the specified <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key of the cache entry.</param>
		/// <returns>True if the cache contains a entry with the given key; otherwise, false.</returns>
		public bool Contains(TKey key)
        {
			lock (items)
            {
				return items.ContainsKey(key);
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

			int matches = 0;

			lock (items)
			{
				foreach (var item in items.Values)
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
			var entries = Load().ToList();

			lock (items)
			{
				items.Clear();

				if (entries != null)
				{
					foreach (var entry in entries)
                    {
						items[GetKey(entry)] = entry;
                    }
				}
			}
		}

		/// <summary>
		/// Loads cache content from the database.
		/// </summary>
		protected abstract IEnumerable<TValue> Load();

		/// <summary>
		/// Gets the key for the specified cache item.
		/// </summary>
		/// <param name="item">The cache item.</param>
		/// <returns>The cache key for the given item.</returns>
		protected abstract TKey GetKey(TValue item);

		/// <summary>
		/// Gets a shallow copy of all cache entries.
		/// </summary>
		/// <returns>All cache entries.</returns>
		public List<TValue> GetAll()
		{
			lock (items)
			{
				return new List<TValue>(items.Values);
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

			lock (items)
			{
				foreach (var item in items.Values)
				{
					if (predicate(item)) return item;
				}
			}

			return default;
		}
	}
}
