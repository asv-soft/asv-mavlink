using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;

public class AsvSdrClient : MavlinkMicroserviceClient, IAsvSdrClient
{
    private readonly RxValue<AsvSdrOutStatusPayload> _status;

    public AsvSdrClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq, IScheduler scheduler)
        : base(connection, identity, seq, "GBS", scheduler)
    {
        _status = new RxValue<AsvSdrOutStatusPayload>().DisposeItWith(Disposable);
        Filter<AsvSdrOutStatusPacket>().Select(_ => _.Payload).Subscribe(_status).DisposeItWith(Disposable);
    }


    public IRxValue<AsvSdrOutStatusPayload> Status => _status;
}