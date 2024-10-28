using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using R3;

namespace Asv.Mavlink;

public class GnssClient : MavlinkMicroserviceClient, IGnssClient
{
    private readonly ReactiveProperty<GpsRawIntPayload?> _gnss1;
    private readonly ReactiveProperty<Gps2RawPayload?> _gnss2;
    private readonly IDisposable _disposeIt;

    public GnssClient(MavlinkClientIdentity identity,ICoreServices core) : base("GNSS", identity, core)
    {
        _gnss1 = new ReactiveProperty<GpsRawIntPayload?>(default);
        _gnss2 = new ReactiveProperty<Gps2RawPayload?>(default);
        var d1 = InternalFilter<GpsRawIntPacket>().Select(p => p.Payload)
            .Subscribe(_gnss1);
        var d2 = InternalFilter<Gps2RawPacket>().Select(p => p.Payload)
            .Subscribe(_gnss2);
        _disposeIt = Disposable.Combine(_gnss1, _gnss2, d1, d2);
    }
    public ReadOnlyReactiveProperty<GpsRawIntPayload?> Main => _gnss1;
    public ReadOnlyReactiveProperty<Gps2RawPayload?> Additional => _gnss2;

    public override void Dispose()
    {
        _disposeIt.Dispose();
        base.Dispose();
    }
}