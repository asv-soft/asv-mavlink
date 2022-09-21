using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink.Payload
{
    public interface IPv2ClientParamsInterface
    {
        IObservable<Pv2ParamValueAndTypePair> CurrentAndThenOnUpdated { get; }
        IObservable<Pv2ParamValueAndTypePair> OnUpdated { get; }
        IRxValue<Pv2ParamsCollection> Params { get; }
        IPayloadV2Client Client { get; }
        Task<bool> RequestAll(CancellationToken cancel = default, IProgress<(double, string)> callback = null);

        Task<Pv2ParamValue> Write(Pv2ParamType param, Action<Pv2ParamType, Pv2ParamValue> valueWriteCallback,
            CancellationToken cancel = default);

        Pv2ParamValue Read(Pv2ParamType param);
        Task<Pv2ParamValue> Update(Pv2ParamType param, CancellationToken cancel);
    }

    public static class Pv2ClientParamsInterfaceHelper
    {
        public static IObservable<TValue> Filter<TParamType, TParamValue, TValue>(
            this IObservable<Pv2ParamValueAndTypePair> src, TParamType type)
            where TParamType : Pv2ParamType<TValue, TParamValue>
            where TParamValue : Pv2ParamValue

        {
            return src.Where(_ => _.Type == type).Select(_ => ((TParamType)_.Type).GetValue(_.Value));
        }
    }
}
