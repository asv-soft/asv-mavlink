using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvRadio;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class AsvRadioServerConfig
{
    public int StatusRateMs { get; set; } = 1000;
}

public class AsvRadioServer : MavlinkMicroserviceServer, IAsvRadioServer
{
    private readonly AsvRadioServerConfig _config;
    private readonly MavlinkPacketTransponder<AsvRadioStatusPacket,AsvRadioStatusPayload> _transponder;

    public AsvRadioServer(
        IMavlinkV2Connection connection, 
        MavlinkIdentity identity,
        AsvRadioServerConfig config, 
        IPacketSequenceCalculator seq, 
        TimeProvider? timeProvider = null,
        IScheduler? rxScheduler = null,
        ILoggerFactory? logFactory = null) 
        : base(AsvRadioHelper.IfcName, connection, identity, seq, timeProvider, rxScheduler, logFactory)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _transponder =
            new MavlinkPacketTransponder<AsvRadioStatusPacket, AsvRadioStatusPayload>(connection, identity, seq,timeProvider,logFactory)
                .DisposeItWith(Disposable);
        
        OnCapabilitiesRequest = InternalFilter<AsvRadioCapabilitiesRequestPacket>(x=>x.Payload.TargetSystem,x=>x.Payload.TargetComponent)
            .Select(x => x.Payload).Publish().RefCount();
        OnCodecCapabilitiesRequest = InternalFilter<AsvRadioCodecCapabilitiesRequestPacket>(x=>x.Payload.TargetSystem,x=>x.Payload.TargetComponent)
            .Select(x => x.Payload).Publish().RefCount();
        
    }
    
    public void Start()
    {
        _transponder.Start(TimeSpan.FromMilliseconds(800),TimeSpan.FromMilliseconds(_config.StatusRateMs));
    }

    public void Set(Action<AsvRadioStatusPayload> changeCallback)
    {
        if (changeCallback == null) throw new ArgumentNullException(nameof(changeCallback));
        _transponder.Set(changeCallback);
    }
    
    public IObservable<AsvRadioCapabilitiesRequestPayload> OnCapabilitiesRequest { get; }

    public Task SendCapabilitiesResponse(Action<AsvRadioCapabilitiesResponsePayload> setValueCallback,
        CancellationToken cancel = default)
    {
        if (setValueCallback == null) throw new ArgumentNullException(nameof(setValueCallback));
        return InternalSend<AsvRadioCapabilitiesResponsePacket>(x => { setValueCallback(x.Payload); }, cancel);
    }
    
    public IObservable<AsvRadioCodecCapabilitiesRequestPayload> OnCodecCapabilitiesRequest { get; }
    public Task SendCodecCapabilitiesRequest(Action<AsvRadioCodecCapabilitiesResponsePayload> setValueCallback, CancellationToken cancel = default)
    {
        if (setValueCallback == null) throw new ArgumentNullException(nameof(setValueCallback));
        return InternalSend<AsvRadioCodecCapabilitiesResponsePacket>(x => { setValueCallback(x.Payload); }, cancel);
    }
}