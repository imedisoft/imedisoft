using OpenDentBusiness;
using System.Collections.Generic;

namespace Imedisoft.Claims
{
    /// <summary>
    /// Represents a bridge with a e-Claims service or program.
    /// </summary>
    public interface IClaimBridge
    {
        /// <summary>
        /// Gets the name of the bridge.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the last error message.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clearingHouse"></param>
        /// <param name="batchNumber"></param>
        /// <param name="queueItems"></param>
        /// <param name="medType"></param>
        /// <returns></returns>
        bool Send(Clearinghouse clearingHouse, long batchNumber, List<ClaimSendQueueItem> queueItems, EnumClaimMedType medType);

        /// <summary>
        /// Retrieves reports from the clearinghouse.
        /// </summary>
        /// <param name="clearingHouse">The clearinghouse.</param>
        bool Retrieve(Clearinghouse clearingHouse);
    }
}
