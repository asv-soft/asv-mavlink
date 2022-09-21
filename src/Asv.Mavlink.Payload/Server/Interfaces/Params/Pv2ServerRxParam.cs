using System;
using Asv.Common;

namespace Asv.Mavlink.Payload
{
    public class Pv2ServerRxParam<TValue, TParamType, TParamValue> : RxValue<TValue>, IServerRxParam<TValue>
        where TParamValue : Pv2ParamValue
        where TParamType : Pv2ParamType<TValue, TParamValue>
    {
        private readonly TParamType _param;
        private readonly IPv2ServerParamsInterface _paramSvc;
        private readonly IDisposable _subscribe;

        public Pv2ServerRxParam(IPv2ServerParamsInterface paramSvc, TParamType param)
        {
            _paramSvc = paramSvc;
            _param = param;
            _subscribe = _paramSvc.OnRemoteUpdated
                .Filter<TParamType, TParamValue, TValue>(param)
                .Subscribe(this);
            OnNext(_param.GetValue(_paramSvc.Read(_param)));
        }

        public void Write(TValue value)
        {
            _paramSvc.Write(_param, (type, paramValue) => _param.SetValue(paramValue, value));
        }


        protected override void InternalDisposeOnce()
        {
            base.InternalDisposeOnce();
            _subscribe.Dispose();
        }
    }
}
