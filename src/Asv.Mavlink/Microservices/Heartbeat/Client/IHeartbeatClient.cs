using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface IHeartbeatClient : IDisposable
    {
        IRxValue<HeartbeatPayload> RawHeartbeat { get; }
        IRxValue<double> PacketRateHz { get; }
        IRxValue<double> LinkQuality { get; }
        IRxValue<LinkState> Link { get; }
    }

    
}