using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Claims
{
    public abstract class ClaimBridge : IClaimBridge
    {
        /// <summary>
        /// Gets the name of the bridge.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the last error message.
        /// </summary>
        public string ErrorMessage { get; protected set; } = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimBridge"/> class.
        /// </summary>
        /// <param name="name">The name of the bridge.</param>
        public ClaimBridge(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public bool Send(Clearinghouse clearingHouse, long batchNumber, List<ClaimSendQueueItem> queueItems, EnumClaimMedType medType)
            => OnSend(clearingHouse, batchNumber, queueItems, medType);

        protected virtual bool OnSend(Clearinghouse clearingHouse, long batchNumber, List<ClaimSendQueueItem> queueItems, EnumClaimMedType medType)
        {
            Logger.LogDebug($"{GetType().FullName} has not implemented {nameof(OnSend)}.");
            return false;
        }

        /// <summary>
        /// Retrieves reports from the clearinghouse.
        /// </summary>
        /// <param name="clearingHouse">The clearinghouse.</param>
        public bool Retrieve(Clearinghouse clearingHouse)
            => OnRetrieve(clearingHouse);

        protected virtual bool OnRetrieve(Clearinghouse clearingHouse) => true;
    }
}
