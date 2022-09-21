using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.IO;
using NLog;

namespace Asv.Mavlink.Payload
{
    public class Pv2ClientParamsInterfaceConfig
    {
        private int _parallelOperationToRead = 5;

        public int ParallelOperationToRead
        {
            get => _parallelOperationToRead;
            set
            {
                if (value <= 0) value = 1;
                if (value >= 100) value = 100;
                _parallelOperationToRead = value;
            }
        }
    }

    public class Pv2ClientParamsInterface : Pv2ClientInterfaceBase, IPv2ClientParamsInterface
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Pv2ClientParamsInterfaceConfig _config;
        private readonly Subject<Pv2ParamValueAndTypePair> _onRemoteUpdated = new();
        private readonly RxValue<Pv2ParamsCollection> _paramsValue = new();
        private readonly IPv2CfgDescriptionStore _store;
        private int _isRequestAllInProgress;
        private uint _localHash;

        public Pv2ClientParamsInterface(IPayloadV2Client client, IPv2CfgDescriptionStore store, IConfiguration cfg) :
            this(client, store, cfg.Get<Pv2ClientParamsInterfaceConfig>())
        {
        }

        public Pv2ClientParamsInterface(IPayloadV2Client client, IPv2CfgDescriptionStore store,
            Pv2ClientParamsInterfaceConfig cfg) : base(client, "PARAMS")
        {
            _store = store;
            _config = cfg;
            Disposable.Add(_paramsValue);
            Client.Subscribe(Pv2ParamInterface.UpdateEvent).Subscribe(OnParamListUpdate).DisposeItWith(Disposable);
            // this is for events, that
            Client.Subscribe(Pv2ParamInterface.ReadType).Where(_ => _.IsError == false && _.Value.Value != null)
                .Select(_ => _.Value).Subscribe(_onRemoteUpdated).DisposeItWith(Disposable);
            Client.Subscribe(Pv2ParamInterface.ReadValue).Subscribe(OnParamUpdate).DisposeItWith(Disposable);
            Client.Subscribe(Pv2ParamInterface.Write).Subscribe(OnParamUpdate).DisposeItWith(Disposable);
        }

        public async Task<Pv2ParamValue> Write(Pv2ParamType param,
            Action<Pv2ParamType, Pv2ParamValue> valueWriteCallback, CancellationToken cancel)
        {
            var item = InternalGetParam(param);
            var newValue = Pv2ParamInterface.CreateValue(item.Type.TypeEnum);
            newValue.Index = item.Index;
            valueWriteCallback(item.Type, newValue);
            item.Type.ValidateValue(newValue);
            var result = await Client.Call(Pv2ParamInterface.Write, new Pv2ParamValueItem(newValue), cancel)
                .ConfigureAwait(false);
            return result.Value;
        }

        public Pv2ParamValue Read(Pv2ParamType param)
        {
            return InternalGetParam(param).Value;
        }

        public async Task<Pv2ParamValue> Update(Pv2ParamType param, CancellationToken cancel)
        {
            if (param == null) throw new ArgumentNullException(nameof(param));
            var currentList = _paramsValue.Value;
            if (currentList is not { Count: > 0 })
                throw new Exception($"Error to update param {param}:param list is empty");
            var realParam = currentList[param.FullName];
            var result = await Client
                .Call(Pv2ParamInterface.ReadValue, new SpanPacketUnsignedIntegerType(realParam.Index), cancel)
                .ConfigureAwait(false);
            param.ValidateValue(result.Value);
            // we don't save result of previous operation, because they already updated in local list by ReadValue subscribe
            return Read(param);
        }

        public IObservable<Pv2ParamValueAndTypePair> CurrentAndThenOnUpdated => _paramsValue.Value == null
            ? _onRemoteUpdated
            : _paramsValue.Value.CopyAllToList().ToObservable().Concat(_onRemoteUpdated);

        public IObservable<Pv2ParamValueAndTypePair> OnUpdated => _onRemoteUpdated;

        public IRxValue<Pv2ParamsCollection> Params => _paramsValue;

        public async Task<bool> RequestAll(CancellationToken cancel = default,
            IProgress<(double, string)> callback = null)
        {
            if (Interlocked.CompareExchange(ref _isRequestAllInProgress, 0, 1) != 0) return false;
            var currentList = _paramsValue.Value;
            try
            {
                _logger.Trace($"{LogSend} Request params description info...");
                var status = await Client.Call(Pv2ParamInterface.Status, SpanVoidType.Default, cancel)
                    .ConfigureAwait(false);
                callback?.Report((0.1, $"{status.Count} params"));
                _logger.Trace($"{LogRecv} Params info:{status.Count} items with hash summ {status.DescriptionHash}");
                if (_localHash == status.DescriptionHash && currentList != null)
                {
                    // all description is valid => just read all values
                    var lazyTasks = Enumerable.Range(0, currentList.Count)
                        .Select(_ => Update(currentList[(uint)_].Type, cancel));
                    foreach (var tasks in lazyTasks.Chunked(_config.ParallelOperationToRead))
                        await Task.WhenAll(tasks).ConfigureAwait(false);
                }
                else
                {
                    var list = new Pv2ParamsCollection((int)status.Count);
                    // need sync descriptions
                    if (_store.TryGetFromCache(status.DescriptionHash, status.Count, out var desc) == false)
                    {
                        // not found in local cache => read all :(

                        var lazyTasks = Enumerable.Range(0, (int)status.Count).Select(_ =>
                            Client.Call(Pv2ParamInterface.ReadType, new SpanPacketUnsignedIntegerType((uint)_),
                                cancel));
                        foreach (var tasks in lazyTasks.Chunked(_config.ParallelOperationToRead))
                        {
                            var resultArr = await Task.WhenAll(tasks).ConfigureAwait(false);
                            foreach (var result in resultArr)
                                if (result.Value == null)
                                {
                                    // need read value, because it's not present in result
                                    var value = await Update(result.Type, cancel).ConfigureAwait(false);
                                    Debug.Assert(value.Index == result.Index);
                                    list.Add(result);
                                }
                                else
                                {
                                    // value present in result
                                    list.Add(result);
                                }
                        }
                    }
                    else
                    {
                        // found in local cache => need read values only
                        Debug.Assert(desc.Count == status.Count);

                        var lazyTasks = Enumerable.Range(0, desc.Count).Select(_ =>
                            Client.Call(Pv2ParamInterface.ReadValue, new SpanPacketUnsignedIntegerType((uint)_),
                                cancel));
                        foreach (var tasks in lazyTasks.Chunked(_config.ParallelOperationToRead))
                        {
                            var resultArr = await Task.WhenAll(tasks).ConfigureAwait(false);
                            foreach (var value in resultArr)
                                list.Add(new Pv2ParamValueAndTypePair(desc[(int)value.Value.Index], value.Value,
                                    value.Value.Index));
                        }
                    }

                    _localHash = status.DescriptionHash;
                    _paramsValue.OnNext(list);
                    _store.Save(_localHash, list.CopyToList());
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.Error($"Error to sync params description:{e.Message}");
                throw;
            }
            finally
            {
                Interlocked.Exchange(ref _isRequestAllInProgress, 0);
            }
        }

        private void OnParamUpdate(StreamResult<Pv2ParamValueItem> result)
        {
            if (result.IsError)
            {
                _logger.Error("Recv error param update");
                return;
            }

            InternalOnValueUpdate(result.Value);
        }

        private void InternalOnValueUpdate(Pv2ParamValueItem resultValue)
        {
            var currentList = _paramsValue.Value;
            if (currentList == null) return;
            if (currentList.Count == 0) return;
            try
            {
                if (currentList.Count <= resultValue.Value.Index)
                {
                    _logger.Warn(
                        $"Recv param with unknown index. Want <{currentList.Count}. Got {resultValue.Value.Index}");
                    return;
                }

                var item = currentList[resultValue.Value.Index];
                item.Type.ValidateValue(resultValue.Value);
                item.Value.CopyFrom(resultValue.Value);
                _onRemoteUpdated.OnNext(item);
            }
            catch (Exception)
            {
                _logger.Error($"Error to publish param [{resultValue.Value.Index}] update");

                Debug.Assert(false, $"Error to publish param [{resultValue.Value.Index}] update");
            }
        }

        private void OnParamListUpdate(StreamResult<Pv2ParamValueList> result)
        {
            if (result.IsError) return;
            foreach (var item in result.Value.Items)
                try
                {
                    InternalOnValueUpdate(item);
                }
                catch (Exception)
                {
                    Debug.Assert(false, $"Error to publish param [{item.Value.Index}] update");
                }
        }

        private Pv2ParamValueAndTypePair InternalGetParam(Pv2ParamType param)
        {
            if (param == null) throw new ArgumentNullException(nameof(param));
            var currentList = _paramsValue.Value;
            if (currentList is not { Count: > 0 })
                throw new Exception($"Error to write param {param}:param list is empty");
            var realParam = currentList[param.FullName];
            if (realParam == null)
                throw new Exception($"Wrong paramName {param}");
            return realParam;
        }
    }
}
