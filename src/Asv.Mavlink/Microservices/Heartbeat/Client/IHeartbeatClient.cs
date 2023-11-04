using System;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;

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