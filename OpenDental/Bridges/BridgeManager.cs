using System;
using System.Collections.Generic;

namespace Imedisoft.Bridges
{
    public static class BridgeManager
    {
        private static readonly Dictionary<string, IBridge> bridges = new Dictionary<string, IBridge>();

        /// <summary>
        /// Gets the bridge with the specified name.
        /// </summary>
        /// <param name="bridgeName">The name of the bridge.</param>
        /// <returns>The bridge with the specified name, or null if no bridge with the specified name exists.</returns>
        public static IBridge GetBridge(string bridgeName)
        {
            lock (bridges)
            {
                if (!bridges.TryGetValue(bridgeName, out var bridge))
                {
                    bridge = CreateInstance(bridgeName);

                    if (bridge != null)
                    {
                        bridges.Add(bridgeName, bridge);
                    }
                }

                return bridge;
            }
        }

        /// <summary>
        /// Creates a new instance of the specified bridge type.
        /// </summary>
        /// <param name="bridgeTypeName">The type name of the bridge.</param>
        /// <returns>The newly created instance.</returns>
        private static IBridge CreateInstance(string bridgeTypeName)
        {
            var type = Type.GetType(bridgeTypeName);
            if (type == null || type.IsInterface || type.IsAbstract || !typeof(IBridge).IsAssignableFrom(type))
            {
                return null;
            }

            try
            {
                return (IBridge)Activator.CreateInstance(type);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Forces all bridges to perform cleanup.
        /// </summary>
        public static void Cleanup()
        {
            var temp = new List<IBridge>();

            lock (bridges)
            {
                temp.AddRange(bridges.Values);
            }

            foreach (var bridge in temp) bridge.Cleanup();
        }
    }
}
