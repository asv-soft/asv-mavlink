using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class StatusTextClient : MavlinkMicroserviceClient, IStatusTextClient
{
    private readonly RxValue<StatusMessage> _onMessage;

    public StatusTextClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq, IScheduler scheduler) : base("STATUS", connection, identity, seq, scheduler)
    {
        Name = new RxValue<string>($"[{identity.TargetSystemId},{identity.TargetComponentId}]").DisposeItWith(Disposable);
        _onMessage = new RxValue<StatusMessage>().DisposeItWith(Disposable);
        InternalFilter<StatustextPacket>()
            .Select(_ => new StatusMessage { Sender = Name.Value, Text = MavlinkTypesHelper.GetString(_.Payload.Text), Type = _.Payload.Severity  })
            .Subscribe(_onMessage)
            .DisposeItWith(Disposable);
    }

    public IRxEditableValue<string> Name { get; }

    public IRxValue<StatusMessage> OnMessage => _onMessage;
}