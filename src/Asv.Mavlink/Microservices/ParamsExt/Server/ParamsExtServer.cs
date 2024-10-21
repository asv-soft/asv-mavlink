using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class ParamsExtServer : MavlinkMicroserviceServer, IParamsExtServer
{
    
    public ParamsExtServer(MavlinkIdentity identity,ICoreServices core)
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

    

    public Task SendParamExtAck(Action<ParamExtAckPayload> changeCallback, CancellationToken cancel = default) =>
        InternalSend<ParamExtAckPacket>(packet => changeCallback(packet.Payload), cancel);
    
    public Task SendParamExtValue(Action<ParamExtValuePayload> changeCallback, CancellationToken cancel = default) =>
        InternalSend<ParamExtValuePacket>(packet => changeCallback(packet.Payload), cancel);

    public IObservable<ParamExtSetPacket> OnParamExtSet { get; }
    public IObservable<ParamExtRequestListPacket> OnParamExtRequestList { get; }
    public IObservable<ParamExtRequestReadPacket> OnParamExtRequestRead { get; }
}