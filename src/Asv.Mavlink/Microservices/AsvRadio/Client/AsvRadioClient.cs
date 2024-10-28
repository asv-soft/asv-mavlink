using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRadio;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class AsvRadioClient : MavlinkMicroserviceClient, IAsvRadioClient
{
    
    public AsvRadioClient(IMavlinkV2Connection connection,
        MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq,
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null,
        ILoggerFactory? logFactory = null) 
        : base(AsvRadioHelper.IfcName, connection, identity, seq,timeProvider,scheduler, logFactory)
    {
        Status = InternalFilter<AsvRadioStatusPacket>()
            .Select(p => p.Payload).Publish().RefCount();
    }

    public IObservable<AsvRadioStatusPayload> Status { get; }
    
    public Task<AsvRadioCapabilitiesResponsePayload> RequestCapabilities(CancellationToken cancel = default)
    {
        return InternalCall<AsvRadioCapabilitiesResponsePayload, AsvRadioCapabilitiesRequestPacket, AsvRadioCapabilitiesResponsePacket>(x =>
        {
            x.Payload.TargetComponent = Identity.TargetComponentId;
            x.Payload.TargetSystem = Identity.TargetSystemId;
        },_=> true, resultGetter:x=>x.Payload,cancel: cancel);
    }


    public Task<AsvRadioCodecCapabilitiesResponsePayload> RequestCodecCapabilities(ushort skip = 0, byte count = byte.MaxValue, CancellationToken cancel = default)
    {
        return InternalCall<AsvRadioCodecCapabilitiesResponsePayload, AsvRadioCodecCapabilitiesRequestPacket, AsvRadioCodecCapabilitiesResponsePacket>(x =>
        {
            x.Payload.TargetComponent = Identity.TargetComponentId;
            x.Payload.TargetSystem = Identity.TargetSystemId;
            x.Payload.Skip = (ushort)skip;
            x.Payload.Count = (byte)count;
        },x=> x.Payload.Skip == skip, resultGetter:x=>x.Payload,cancel: cancel);
    }
}