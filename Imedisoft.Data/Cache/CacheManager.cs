using System;
using System.Collections.Generic;
using System.Reflection;

namespace Imedisoft.Data.Cache
{
    public static class CacheManager
    {
        private static readonly Dictionary<string, List<ICache>> cachesByGroup = new Dictionary<string, List<ICache>>();
        private static readonly Dictionary<Type, List<string>> typeToGroupMap = new Dictionary<Type, List<string>>();

        /// <summary>
        /// Registers the specified cache with the <see cref="CacheManager"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of values stored in the cache.</typeparam>
        /// <param name="cache">The cache instance to register.</param>
        internal static void Register<TValue>(ICache<TValue> cache)
        {
            var groups = new List<string>();
            foreach (var attribute in cache.GetType().GetCustomAttributes<CacheGroupAttribute>())
            {
                var group = attribute.GroupName.ToLower();
                if (!groups.Contains(group))
                {
                    groups.Add(group);
                }
            }

            if (groups.Count == 0) groups.Add(typeof(TValue).FullName);

            foreach (var group in groups)
            {
                var caches = GetCachesForGroup(group);

                lock (caches)
                {
                    if (!caches.Contains(cache))
                    {
                        caches.Add(cache);
                    }
                }
            }

            lock (typeToGroupMap)
            {
                var type = typeof(TValue);

                if (!typeToGroupMap.TryGetValue(type, out var typeToGroups))
                {
                    typeToGroups = new List<string>();
                    typeToGroupMap.Add(type, typeToGroups);
                }

                foreach (var group in groups)
                {
                    if (!typeToGroups.Contains(group))
                    {
                        typeToGroups.Add(group);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a list of all caches registered for the specified <paramref name="group"/>.
        /// </summary>
        /// <param name="group">The cache group.</param>
        /// <returns>All caches registered for the specified group.</returns>
        private static IList<ICache> GetCachesForGroup(string group)
        {
            lock (cachesByGroup)
            {
                if (!cachesByGroup.TryGetValue(group, out var caches))
                {
                    caches = new List<ICache>();
                    cachesByGroup[group] = caches;
                }

                return caches;
            }
        }

        /// <summary>
        /// Refreshes all caches in the specified <paramref name="groups"/>.
        /// </summary>
        /// <param name="groups">The list of groups to refresh.</param>
        public static void Refresh(params string[] groups)
        {
            if (groups == null) return;

            var results = new List<ICache>();

            // Resolve all the (distinct) caches that need to be refreshes based on the given groups.

            lock (cachesByGroup)
            {
                foreach (var group in groups)
                {
                    if (!cachesByGroup.TryGetValue(group, out var caches))
                    {
                        continue;
                    }

                    foreach (var cache in caches)
                    {
                        if (!results.Contains(cache))
                        {
                            results.Add(cache);
                        }
                    }
                }
            }

            foreach (var cache in results) cache.Refresh();
        }

        /// <summary>
        /// Refreshes all caches in the specified <paramref name="group"/>.
        /// </summary>
        /// <param name="group">The name of the group to refresh.</param>
        public static void Refresh(string group)
        {
            if (string.IsNullOrEmpty(group)) return;

            var caches = GetCachesForGroup(group);

            lock (caches)
            {
                foreach (var cache in caches)
                {
                    cache.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets the groups that contain caches of the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>All groups that contain caches of the given type.</returns>
        private static string[] GetGroupsForType(Type type)
        {
            lock (typeToGroupMap)
            {
                if (typeToGroupMap.TryGetValue(type, out var groups))
                {
                    return groups.ToArray();
                }
            }

            return null;
        }

        /// <summary>
        /// Refreshes all caches for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type for which to refresh all caches.</param>
        public static void Refresh(Type type) => Refresh(GetGroupsForType(type));

        /// <summary>
        /// Refreshes all caches for the specified value.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        public static void Refresh<TValue>() => Refresh(typeof(TValue));

        /// <summary>
        /// Refreshes all caches.
        /// </summary>
        public static void RefreshAll()
        {
            var all = new List<ICache>();

            lock (cachesByGroup)
            {
                foreach (var caches in cachesByGroup.Values)
                {
                    foreach (var cache in caches)
                    {
                        if (!all.Contains(cache))
                        {
                            all.Add(cache);
                        }
                    }
                }
            }

            foreach (var cache in all) cache.Refresh();
        }
    }
}
