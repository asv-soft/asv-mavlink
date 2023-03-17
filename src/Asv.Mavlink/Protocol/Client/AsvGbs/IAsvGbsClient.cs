using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface IAsvGbsClient:IDisposable
    {
        IRxValue<AsvGbsState> State { get; }
        IRxValue<GeoPoint> Position { get; }
        Task<MavResult> StartAutoMode(float duration, float accuracy, CancellationToken cancel);
        Task<MavResult> StartFixedMode(GeoPoint geoPoint, CancellationToken cancel);
        Task<MavResult> StartIdleMode(CancellationToken cancel);
    }
}