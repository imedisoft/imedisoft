using CodeBase;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Imedisoft.Claims
{
    public static class ClaimBridgeManager
    {
        private static readonly List<IClaimBridge> bridges = new List<IClaimBridge>();

        /// <summary>
        /// Initializes a instance of every concrete class that implements the <see cref="IClaimBridge"/> interface.
        /// </summary>
        static ClaimBridgeManager()
        {
            var bridgeType = typeof(IClaimBridge);

            Logger.LogTrace("Initialize e-Claims bridges...");

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsClass && type.IsPublic && !type.IsAbstract && bridgeType.IsAssignableFrom(type))
                {
                    try
                    {
                        var bridgeInstance = (IClaimBridge)Activator.CreateInstance(bridgeType);
                        if (null != bridgeInstance)
                        {
                            bridges.Add(bridgeInstance);
                        }

                        Logger.LogDebug($"OK {bridgeType.FullName}");
                    }
                    catch (Exception exception)
                    {
                        Logger.LogError($"ERROR {bridgeType.FullName} - {exception.Message}");
                    }
                }
            }

            Logger.LogTrace($"Loaded {bridges.Count} e-Claims bridge(s).");
        }

        /// <summary>
        /// Enumerates all avaialble e-Claims bridges.
        /// </summary>
        public static IEnumerable<IClaimBridge> EnumerateBridges() => bridges;

        /// <summary>
        /// Gets the e-Claims bridge of the specified type.
        /// </summary>
        /// <param name="typeName">The full type name of the e-Claims bridge.</param>
        /// <returns>The e-Claims Bridge instance; or null no bridge with the given type name exists.</returns>
        public static IClaimBridge GetBridgeByType(string typeName)
        {
            foreach (var bridge in bridges)
            {
                if (bridge.GetType().FullName.Equals(typeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return bridge;
                }
            }

            return null;
        }
    }
}
