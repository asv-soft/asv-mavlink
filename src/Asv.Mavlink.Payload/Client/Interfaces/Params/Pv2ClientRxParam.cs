using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink.Payload
{
    public interface IClientRxParam<TValue> : IRxValue<TValue>
    {
        Task<TValue> Update(CancellationToken cancel = default);
        Task<TValue> Write(TValue value, CancellationToken cancel = default);
    }

    public class Pv2ClientRxParam<TValue, TParamType, TParamValue> : RxValue<TValue>, IClientRxParam<TValue>
        where TParamValue : Pv2ParamValue
        where TParamType : Pv2ParamType<TValue, TParamValue>
    {
        private readonly TParamType _param;
        private readonly IPv2ClientParamsInterface _paramSvc;
        private readonly IDisposable _subscribe;

        public Pv2ClientRxParam(IPv2ClientParamsInterface paramSvc, TParamType param)
        {
            _paramSvc = paramSvc;
            _param = param;
            _subscribe = _paramSvc.CurrentAndThenOnUpdated
                .Filter<TParamType, TParamValue, TValue>(param)
                .Subscribe(this);
        }

        public async Task<TValue> Update(CancellationToken cancel = default)
        {
            var value = await _paramSvc.Update(_param, cancel);
            return _param.GetValue(value);
        }

        public async Task<TValue> Write(TValue value, CancellationToken cancel = default)
        {
            var result = await _paramSvc.Write(_param, (type, paramValue) => { _param.SetValue(paramValue, value); },
                cancel);
            return _param.GetValue(result);
        }

        protected override void InternalDisposeOnce()
        {
            base.InternalDisposeOnce();
            _subscribe.Dispose();
        }
    }
}
