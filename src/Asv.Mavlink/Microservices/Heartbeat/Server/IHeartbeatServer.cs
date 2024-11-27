using System;
using Asv.Common;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink
{
    /// <summary>
    /// Defines the interface for a heartbeat server.
    /// </summary>
    public interface IHeartbeatServer:IMavlinkMicroserviceServer
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

        void SetCustomMode(Action<UintBitArray> changeCallback)
        {
            Set(x =>
            {
                var bitArray = new UintBitArray(x.CustomMode, 32);
                changeCallback(bitArray);
                x.CustomMode = bitArray.Value;
            });
        }
    }
}
