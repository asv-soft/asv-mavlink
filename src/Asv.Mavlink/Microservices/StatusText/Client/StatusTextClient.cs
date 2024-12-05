using System;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink;

public sealed class StatusTextClient : MavlinkMicroserviceClient, IStatusTextClient
{
    private readonly Subject<StatusMessage> _onMessage;
    private readonly ReactiveProperty<string> _name;

    public StatusTextClient(MavlinkClientIdentity identity,IMavlinkContext core) 
        : base("STATUS",identity,core)
    {
        _name = new ReactiveProperty<string>($"[{identity.Target.SystemId},{identity.Target.ComponentId}]");
        _onMessage = new Subject<StatusMessage>();
        _sub = InternalFilter<StatustextPacket>()
            .Select(Convert)
            .Subscribe(_onMessage.AsObserver());
    }

    private StatusMessage Convert(StatustextPacket p)
    {
        return new StatusMessage
        {
            Sender = DeviceName.Value, Text = MavlinkTypesHelper.GetString(p.Payload.Text), Type = p.Payload.Severity
        };
    }

    public ReactiveProperty<string> DeviceName => _name;

    public Observable<StatusMessage> OnMessage => _onMessage;

    #region Dispose

    private readonly IDisposable _sub;
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _onMessage.Dispose();
            _name.Dispose();
            _sub.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_onMessage).ConfigureAwait(false);
        await CastAndDispose(_name).ConfigureAwait(false);
        await CastAndDispose(_sub).ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    #endregion
}