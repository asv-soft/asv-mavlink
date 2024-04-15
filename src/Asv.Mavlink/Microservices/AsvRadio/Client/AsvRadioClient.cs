using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvAudio;
using Asv.Mavlink.V2.AsvRadio;

namespace Asv.Mavlink;

public class AsvRadioClient : MavlinkMicroserviceClient, IAsvRadioClient
{
    
    public AsvRadioClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, IPacketSequenceCalculator seq) 
        : base(AsvRadioHelper.IfcName, connection, identity, seq)
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

    public Task<AsvRadioCodecCfgResponsePayload> RequestCodecOptions(AsvAudioCodec codec, CancellationToken cancel = default)
    {
        return InternalCall<AsvRadioCodecCfgResponsePayload, AsvRadioCodecCfgRequestPacket, AsvRadioCodecCfgResponsePacket>(x =>
        {
            x.Payload.TargetComponent = Identity.TargetComponentId;
            x.Payload.TargetSystem = Identity.TargetSystemId;
            x.Payload.TargetCodec = codec;
        },x=> x.Payload.TargetCodec == codec, resultGetter:x=>x.Payload,cancel: cancel);
    }
}