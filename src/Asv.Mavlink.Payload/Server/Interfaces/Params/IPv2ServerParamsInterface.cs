using System;
using Asv.Common;

namespace Asv.Mavlink.Payload
{
    public interface IPv2ServerParamsInterface : IDisposable
    {
        uint ParamsInfoHash { get; }
        bool IsSendUpdateEnabled { get; set; }
        IRxValue<bool> IsSendingUpdate { get; }
        IObservable<Pv2ParamValueAndTypePair> OnRemoteUpdated { get; }
        IObservable<Pv2ParamValueAndTypePair> CurrentAndThenRemoteUpdated { get; }
        IPayloadV2Server Server { get; }
        void Write(Pv2ParamType param, Action<Pv2ParamType, Pv2ParamValue> valueWriteCallback);
        Pv2ParamValue Read(Pv2ParamType param);
    }
}
