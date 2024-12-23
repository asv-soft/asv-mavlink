using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvGbs;
using Asv.Mavlink.Common;


namespace Asv.Mavlink
{
    /// <summary>
    /// Interface that defines the server for AsvGbs.
    /// </summary>
    public interface IAsvGbsServer: IMavlinkMicroserviceServer
    {
        
        /// <summary>
        /// Sets an action to be performed when Asv Gbs Out Status Payload gets changed.
        /// </summary>
        /// <param name="changeCallback">Action to be performed when Asv Gbs Out Status Payload changes.</param>
        void Set(Action<AsvGbsOutStatusPayload> changeCallback);

        /// <summary>
        /// Sends a DGPS packet with the specified data packet.
        /// </summary>
        /// <param name="changeCallback">Specifies the GPS RTCM data packet to be sent.</param>
        /// <param name="cancel">Optional cancellation token to allow halting this task.</param>
        /// <returns>A Task representing asynchronous send operation.</returns>
        ValueTask SendDgps(Action<GpsRtcmDataPacket> changeCallback, CancellationToken cancel = default);
    }
}