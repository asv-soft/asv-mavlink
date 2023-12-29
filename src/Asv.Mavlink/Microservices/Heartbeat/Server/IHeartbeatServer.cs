using System;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink
{
    /// <summary>
    /// Defines the interface for a heartbeat server.
    /// </summary>
    public interface IHeartbeatServer:IDisposable
    {
        /// <summary>
        /// Starts the execution of the program.
        /// </summary>
        void Start();

        /// <summary>
        /// Sets the change callback for the heart beat payload.
        /// </summary>
        /// <param name="changeCallback">The action to be performed on the heart beat payload.</param>
        void Set(Action<HeartbeatPayload> changeCallback);
    }
}
