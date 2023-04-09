using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Server
{
    /// <summary>
    /// Message implementing parts of the V2 payload specs in V1 frames for transitional support.
    /// </summary>
    public interface IV2ExtensionServer : IDisposable
    {
        IRxValue<V2ExtensionPacket> OnData { get; }

        Task SendData(byte targetSystemId, byte targetComponentId, byte targetNetworkId, ushort messageType,
            byte[] data, CancellationToken cancel);
    }
}
