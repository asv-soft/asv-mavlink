using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class GnssClient : MavlinkMicroserviceClient, IGnssClient
{
    private readonly RxValue<GpsRawIntPayload> _gnss1;
    private readonly RxValue<Gps2RawPayload> _gnss2;

    public GnssClient(
        IMavlinkV2Connection connection, 
        MavlinkClientIdentity identity, 
        IPacketSequenceCalculator seq,
        IScheduler? scheduler = null,
        ILogger? logger = null) : base("RTT:GNSS", connection, identity, seq,scheduler,logger)
    {
        _gnss1 = new RxValue<GpsRawIntPayload>().DisposeItWith(Disposable);
        _gnss2 = new RxValue<Gps2RawPayload>().DisposeItWith(Disposable);
        InternalFilter<GpsRawIntPacket>().Select(p => p.Payload)
            .Subscribe(_gnss1).DisposeItWith(Disposable);
        InternalFilter<Gps2RawPacket>().Select(p => p.Payload)
            .Subscribe(_gnss2).DisposeItWith(Disposable);
    }
    public IRxValue<GpsRawIntPayload> Main => _gnss1;
    public IRxValue<Gps2RawPayload> Additional => _gnss2;
}