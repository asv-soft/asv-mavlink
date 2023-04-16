using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface IAsvGbsServer
    {
        void Start();
        void Set(Action<AsvGbsOutStatusPayload> changeCallback);
        Task SendDgps(Action<GpsRtcmDataPacket> changeCallback, CancellationToken cancel = default);
    }
}