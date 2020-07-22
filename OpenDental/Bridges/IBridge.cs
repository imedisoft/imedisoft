using OpenDentBusiness;

namespace Imedisoft.Bridges
{
    /// <summary>
    ///     <para>
    ///         Represents a bridge with an external application. 
    ///     </para>
    ///     <para>
    ///         Bridges facilitate the transfer of patient information to external programs.
    ///     </para>
    /// </summary>
    public interface IBridge
    {
        /// <summary>
        /// Sends the details of the specified patient to the external program.
        /// </summary>
        /// <param name="program">The program configuration.</param>
        /// <param name="patient">The patient details.</param>
        void Send(Program program, Patient patient);

        /// <summary>
        ///     <para>
        ///         Performs any cleanup. Takes care of deleting any files generated, etc...
        ///     </para>
        ///     <para>
        ///         This is typically called by <see cref="BridgeManager.Cleanup"/> when the application is shutting down.
        ///     </para>
        /// </summary>
        void Cleanup();
    }
}
