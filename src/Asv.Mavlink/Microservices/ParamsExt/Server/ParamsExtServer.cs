using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class ParamsExtServer : MavlinkMicroserviceServer, IParamsExtServer
{
    /// <summary>
    /// Initializes a new instance of the ParamsExtServer class.
    /// </summary>
    /// <param name="connection">The IMavlinkV2Connection object.</param>
    /// <param name="seq">The IPacketSequenceCalculator object.</param>
    /// <param name="identity">The MavlinkIdentity object.</param>
    /// <param name="scheduler">The IScheduler object.</param>
    public ParamsExtServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,
        MavlinkIdentity identity, IScheduler scheduler)
        : base("PARAMS_EXT", connection, identity, seq, scheduler)
    {
        if (seq == null) throw new ArgumentNullException(nameof(seq));
        if (connection == null) throw new ArgumentNullException(nameof(connection));
        if (scheduler == null) throw new ArgumentNullException(nameof(scheduler));

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