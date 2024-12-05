using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

namespace Asv.Mavlink
{
    public class ParamsServer: MavlinkMicroserviceServer, IParamsServer
    {
        public ParamsServer(MavlinkIdentity identity, IMavlinkContext core)
            : base("PARAM", identity, core)
        {
            OnParamRequestRead =
                InternalFilter<ParamRequestReadPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent);
            OnParamRequestList =
                InternalFilter<ParamRequestListPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent);
            OnParamSet =
                InternalFilter<ParamSetPacket>(p => p.Payload.TargetSystem, p => p.Payload.TargetComponent);

        }
        
        public ValueTask SendParamValue(Action<ParamValuePayload> changeCallback, CancellationToken cancel = default)
        {
            return InternalSend<ParamValuePacket>(p=>changeCallback(p.Payload), cancel);
        }

        public Observable<ParamRequestReadPacket> OnParamRequestRead { get; }
        public Observable<ParamRequestListPacket> OnParamRequestList { get; }
        public Observable<ParamSetPacket> OnParamSet { get; }
        
    }
}
