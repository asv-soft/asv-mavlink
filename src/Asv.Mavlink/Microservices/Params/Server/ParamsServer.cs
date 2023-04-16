using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public class ParamsServer: MavlinkMicroserviceServer, IParamsServer
    {
        public ParamsServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,
            MavlinkServerIdentity identity, IScheduler scheduler)
            : base("PARAM",connection, identity, seq, scheduler)
        {
            OnParamRequestRead =
                InternalFilter<ParamRequestReadPacket>(_ => _.Payload.TargetSystem, _ => _.Payload.TargetComponent);
            OnParamRequestList =
                InternalFilter<ParamRequestListPacket>(_ => _.Payload.TargetSystem, _ => _.Payload.TargetComponent);
            OnParamSet =
                InternalFilter<ParamSetPacket>(_ => _.Payload.TargetSystem, _ => _.Payload.TargetComponent);

        }
        
        public Task SendParamValue(Action<ParamValuePayload> changeCallback, CancellationToken cancel = default)
        {
            return InternalSend<ParamValuePacket>(_=>changeCallback(_.Payload), cancel);
        }

        public IObservable<ParamRequestReadPacket> OnParamRequestRead { get; }
        public IObservable<ParamRequestListPacket> OnParamRequestList { get; }
        public IObservable<ParamSetPacket> OnParamSet { get; }
    }
}
