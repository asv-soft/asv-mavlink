using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
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