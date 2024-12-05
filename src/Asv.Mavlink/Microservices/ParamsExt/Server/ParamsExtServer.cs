using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink;

public class ParamsExtServer : MavlinkMicroserviceServer, IParamsExtServer
{
    
    public ParamsExtServer(MavlinkIdentity identity,IMavlinkContext core)
        : base("PARAMS_EXT",identity, core)
    {
        OnParamExtSet = InternalFilter<ParamExtSetPacket>(
            packet => packet.Payload.TargetSystem,
            packet => packet.Payload.TargetComponent);
        OnParamExtRequestList = InternalFilter<ParamExtRequestListPacket>(
            packet => packet.Payload.TargetSystem,
            packet => packet.Payload.TargetComponent);
        OnParamExtRequestRead = InternalFilter<ParamExtRequestReadPacket>(
            packet => packet.Payload.TargetSystem,
            packet => packet.Payload.TargetComponent);
    }

    

    public ValueTask SendParamExtAck(Action<ParamExtAckPayload> changeCallback, CancellationToken cancel = default) =>
        InternalSend<ParamExtAckPacket>(packet => changeCallback(packet.Payload), cancel);
    
    public ValueTask SendParamExtValue(Action<ParamExtValuePayload> changeCallback, CancellationToken cancel = default) =>
        InternalSend<ParamExtValuePacket>(packet => changeCallback(packet.Payload), cancel);

    public Observable<ParamExtSetPacket> OnParamExtSet { get; }
    public Observable<ParamExtRequestListPacket> OnParamExtRequestList { get; }
    public Observable<ParamExtRequestReadPacket> OnParamExtRequestRead { get; }
}