using System;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface IHeartbeatClient : IDisposable
    {
        ushort FullId { get; }
        IRxValue<HeartbeatPayload> RawHeartbeat { get; }
        IRxValue<double> PacketRateHz { get; }
        IRxValue<double> LinkQuality { get; }
        IRxValue<LinkState> Link { get; }
    }

    
}