using System;
using System.Threading;
using System.Threading.Tasks;
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
    private readonly Subject<AsvRadioCapabilitiesRequestPayload?> _onCapabilitiesRequest = new();
    private readonly Subject<AsvRadioCodecCapabilitiesRequestPayload?> _onCodecCapabilitiesRequest = new();

    public AsvRadioServer(MavlinkIdentity identity, AsvRadioServerConfig config, ICoreServices core)     
        : base(AsvRadioHelper.IfcName, identity,core)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _transponder =
            new MavlinkPacketTransponder<AsvRadioStatusPacket>(identity, core);
        
        // we create Subject and subscribe to it because we need to dispose subscription in Dispose method
        InternalFilter<AsvRadioCapabilitiesRequestPacket>(x => x.Payload.TargetSystem,
                    x => x.Payload.TargetComponent)
                .Select(x => x?.Payload).Subscribe(_onCapabilitiesRequest.AsObserver());
        
        InternalFilter<AsvRadioCodecCapabilitiesRequestPacket>(x=>x.Payload.TargetSystem,x=>x.Payload.TargetComponent)
            .Select(x => x?.Payload).Subscribe(_onCodecCapabilitiesRequest.AsObserver());
        
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

    public Observable<AsvRadioCapabilitiesRequestPayload?> OnCapabilitiesRequest => _onCapabilitiesRequest;

    public ValueTask SendCapabilitiesResponse(Action<AsvRadioCapabilitiesResponsePayload> setValueCallback,
        CancellationToken cancel = default)
    {
        if (setValueCallback == null) throw new ArgumentNullException(nameof(setValueCallback));
        return InternalSend<AsvRadioCapabilitiesResponsePacket>(x => { setValueCallback(x.Payload); }, cancel);
    }

    public Observable<AsvRadioCodecCapabilitiesRequestPayload?> OnCodecCapabilitiesRequest => _onCodecCapabilitiesRequest;

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
            _onCapabilitiesRequest.Dispose();
            _onCodecCapabilitiesRequest.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_transponder).ConfigureAwait(false);
        await CastAndDispose(_onCapabilitiesRequest).ConfigureAwait(false);
        await CastAndDispose(_onCodecCapabilitiesRequest).ConfigureAwait(false);

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