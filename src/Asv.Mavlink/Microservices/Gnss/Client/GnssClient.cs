using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class GnssClient : MavlinkMicroserviceClient, IGnssClient
{
    private readonly RxValue<GpsRawIntPayload> _gnss1;
    private readonly RxValue<Gps2RawPayload> _gnss2;

    public GnssClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, IPacketSequenceCalculator seq) : base("RTT:GNSS", connection, identity, seq)
    {
        _gnss1 = new RxValue<GpsRawIntPayload>().DisposeItWith(Disposable);
        _gnss2 = new RxValue<Gps2RawPayload>().DisposeItWith(Disposable);
        InternalFilter<GpsRawIntPacket>().Select(_ => _.Payload)
            .Subscribe(_gnss1).DisposeItWith(Disposable);
        InternalFilter<Gps2RawPacket>().Select(_ => _.Payload)
            .Subscribe(_gnss2).DisposeItWith(Disposable);
    }
    public IRxValue<GpsRawIntPayload> Main => _gnss1;
    public IRxValue<Gps2RawPayload> Additional => _gnss2;
}