using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRadio;
using Asv.Mavlink.V2.AsvRadio;
using R3;

namespace Asv.Mavlink;

public class AsvRadioClient : MavlinkMicroserviceClient, IAsvRadioClient
{
    
    public AsvRadioClient(MavlinkClientIdentity identity, ICoreServices core) 
        : base(AsvRadioHelper.IfcName, identity, core)
    {
        Status = InternalFilter<AsvRadioStatusPacket>()
            .Select(p => p?.Payload)
            .ToReadOnlyReactiveProperty();
    }

    public ReadOnlyReactiveProperty<AsvRadioStatusPayload?> Status { get; }
    
    public Task<AsvRadioCapabilitiesResponsePayload> RequestCapabilities(CancellationToken cancel = default)
    {
        return InternalCall<AsvRadioCapabilitiesResponsePayload, AsvRadioCapabilitiesRequestPacket, AsvRadioCapabilitiesResponsePacket>(x =>
        {
            x.Payload.TargetComponent = Identity.Target.ComponentId;
            x.Payload.TargetSystem = Identity.Target.SystemId;
        },_=> true, resultGetter:x=>x.Payload,cancel: cancel);
    }


    public Task<AsvRadioCodecCapabilitiesResponsePayload> RequestCodecCapabilities(ushort skip = 0, byte count = byte.MaxValue, CancellationToken cancel = default)
    {
        return InternalCall<AsvRadioCodecCapabilitiesResponsePayload, AsvRadioCodecCapabilitiesRequestPacket, AsvRadioCodecCapabilitiesResponsePacket>(x =>
        {
            x.Payload.TargetComponent = Identity.Target.ComponentId;
            x.Payload.TargetSystem = Identity.Target.SystemId;
            x.Payload.Skip = (ushort)skip;
            x.Payload.Count = (byte)count;
        },x=> x.Payload.Skip == skip, resultGetter:x=>x.Payload,cancel: cancel);
    }
}