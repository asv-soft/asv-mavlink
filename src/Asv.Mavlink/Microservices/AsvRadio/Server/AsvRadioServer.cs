using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRadio;
using Asv.Mavlink.AsvRadio;
using R3;

namespace Asv.Mavlink;

public class AsvRadioServerConfig
{
    public int StatusRateMs { get; set; } = 1000;
}

public class AsvRadioServer : MavlinkMicroserviceServer, IAsvRadioServer
{
    private readonly AsvRadioServerConfig _config;
    private readonly MavlinkPacketTransponder<AsvRadioStatusPacket> _transponder;

    public AsvRadioServer(MavlinkIdentity identity, AsvRadioServerConfig config, ICoreServices core)     
        : base(AsvRadioHelper.IfcName, identity,core)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _transponder =
            new MavlinkPacketTransponder<AsvRadioStatusPacket>(identity, core);

        OnCapabilitiesRequest =
            InternalFilter<AsvRadioCapabilitiesRequestPacket>(x => x.Payload.TargetSystem,
                    x => x.Payload.TargetComponent)
                .Select(x => x?.Payload).ToReadOnlyReactiveProperty();
        OnCodecCapabilitiesRequest = InternalFilter<AsvRadioCodecCapabilitiesRequestPacket>(x=>x.Payload.TargetSystem,x=>x.Payload.TargetComponent)
            .Select(x => x?.Payload)
            .ToReadOnlyReactiveProperty();
        
    }
    
    public void Start()
    {
        _transponder.Start(TimeSpan.FromMilliseconds(_config.StatusRateMs),TimeSpan.FromMilliseconds(_config.StatusRateMs));
    }

    public void Set(Action<AsvRadioStatusPayload> changeCallback)
    {
        if (changeCallback == null) throw new ArgumentNullException(nameof(changeCallback));
        _transponder.Set(x=>changeCallback(x.Payload));
    }
    
    public ReadOnlyReactiveProperty<AsvRadioCapabilitiesRequestPayload?> OnCapabilitiesRequest { get; }

    public ValueTask SendCapabilitiesResponse(Action<AsvRadioCapabilitiesResponsePayload> setValueCallback,
        CancellationToken cancel = default)
    {
        if (setValueCallback == null) throw new ArgumentNullException(nameof(setValueCallback));
        return InternalSend<AsvRadioCapabilitiesResponsePacket>(x => { setValueCallback(x.Payload); }, cancel);
    }
    
    public ReadOnlyReactiveProperty<AsvRadioCodecCapabilitiesRequestPayload?> OnCodecCapabilitiesRequest { get; }
    public ValueTask SendCodecCapabilitiesRequest(Action<AsvRadioCodecCapabilitiesResponsePayload> setValueCallback, CancellationToken cancel = default)
    {
        if (setValueCallback == null) throw new ArgumentNullException(nameof(setValueCallback));
        return InternalSend<AsvRadioCodecCapabilitiesResponsePacket>(x => { setValueCallback(x.Payload); }, cancel);
    }

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _transponder.Dispose();
            OnCapabilitiesRequest.Dispose();
            OnCodecCapabilitiesRequest.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_transponder).ConfigureAwait(false);
        await CastAndDispose(OnCapabilitiesRequest).ConfigureAwait(false);
        await CastAndDispose(OnCodecCapabilitiesRequest).ConfigureAwait(false);

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