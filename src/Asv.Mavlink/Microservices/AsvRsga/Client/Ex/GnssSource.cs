using System;
using Asv.Common;
using Asv.Mavlink.AsvRsga;
using R3;

namespace Asv.Mavlink;


public class GnssSource : DisposableOnce
{
    private readonly IDisposable _disposeIt;

    public GnssSource(
        ushort refId,
        Observable<AsvRsgaRttGnssPayload> rawStream,
        AsvRsgaRttGnssPayload initValue
    )
    {
        RefId = refId;
        NavId = $"gnss_{refId}";
        var builder = default(DisposableBuilder);
        Stream = rawStream
            .Where(refId, (x, id) => x.RefId == id)
            .ToReadOnlyReactiveProperty(initValue)
            .AddTo(ref builder);

        Location = Stream
            .Select(x => x.GetLocation())
            .ToReadOnlyReactiveProperty(new GeoPoint(0, 0, 0))
            .AddTo(ref builder);
        _disposeIt = builder.Build();
    }

    public string NavId { get; }
    public ushort RefId { get; }
    public ReadOnlyReactiveProperty<GeoPoint> Location { get; }
    public ReadOnlyReactiveProperty<AsvRsgaRttGnssPayload> Stream { get; }

    protected override void InternalDisposeOnce()
    {
        _disposeIt.Dispose();
    }
}