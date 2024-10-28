using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using R3;

namespace Asv.Mavlink;

public class StatusTextClient : MavlinkMicroserviceClient, IStatusTextClient
{
    private readonly RxValueBehaviour<StatusMessage?> _onMessage;
    private readonly RxValueBehaviour<string> _name;
    private readonly IDisposable _disposeIt;

    public StatusTextClient(MavlinkClientIdentity identity,ICoreServices core) 
        : base("STATUS",identity,core)
    {
        _name = new RxValueBehaviour<string>($"[{identity.Target.SystemId},{identity.Target.ComponentId}]");
        _onMessage = new RxValueBehaviour<StatusMessage?>(default);
        var d1 = InternalFilter<StatustextPacket>()
            .Select(p => new StatusMessage
                { Sender = DeviceName.Value, Text = MavlinkTypesHelper.GetString(p.Payload.Text), Type = p.Payload.Severity })
            .Subscribe(_onMessage);
        _disposeIt = Disposable.Combine(_name,_onMessage, d1);
    }

    public IRxEditableValue<string> DeviceName => _name;

    public IRxValue<StatusMessage?> OnMessage => _onMessage;
    public override void Dispose()
    {
        _disposeIt.Dispose();
        base.Dispose();
    }
}