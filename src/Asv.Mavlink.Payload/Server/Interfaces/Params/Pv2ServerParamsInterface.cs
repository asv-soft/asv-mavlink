using System;
using System.Collections.Generic;
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
    public class Pv2ServerParamsInterface : Pv2ServerInterfaceBase, IPv2ServerParamsInterface
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration _config;

        private readonly string _configSuffix;
        private readonly KeyValuePair<string, object> _diagTagId;
        private readonly RxValue<bool> _isSendingUpdateSubject = new();
        private readonly Subject<uint> _needSendUpdates = new();
        private readonly Subject<Pv2ParamValueAndTypePair> _onRemoteUpdated = new();
        private readonly Pv2ParamsCollection _params;
        private readonly TimeSpan _sendUpdateDelay = TimeSpan.FromMilliseconds(100);
        private readonly IPayloadV2Server _server;
        private readonly ConcurrentHashSet<uint> _valuesToUpdate = new();
        private double _isUpdateInProgress;

        public Pv2ServerParamsInterface(IPayloadV2Server server, IConfiguration config, IEnumerable<Pv2ParamType> items,
            string configSuffix = "PARAMS")
            : base(server, Pv2ParamInterface.InterfaceName)
        {
            _diagTagId = new KeyValuePair<string, object>("ID", server.Server.Identity);
            _configSuffix = configSuffix;
            _server = server;
            _config = config;
            var payloadV2ParamInfos = items as Pv2ParamType[] ?? items.ToArray();
            foreach (var item in payloadV2ParamInfos.GroupBy(_ => _.FullName))
                if (item.Count() > 1)
                    throw new Exception($"Params name '{item.Key}' not unique");

            _params = new Pv2ParamsCollection(payloadV2ParamInfos.Length);
            for (uint i = 0; i < _params.Count; i++)
            {
                var type = payloadV2ParamInfos[i];
                type.ValidateSize();
                var value = type.ReadFromConfig(config, configSuffix);
                _params.Add(new Pv2ParamValueAndTypePair(type, value, i));
                value.Index = i;
                value.ValidateSize();
                Logger.Info($"{InterfaceName}: Load param '{type}' = {type.ConvertToString(value)}");
            }

            Disposable.Add(System.Reactive.Disposables.Disposable.Create(() =>
            {
                _onRemoteUpdated.OnCompleted();
                _onRemoteUpdated.Dispose();
            }));

            ParamsInfoHash = Pv2ParamType.CalculateHash(_params.CopyToList());
            server.Register(Pv2ParamInterface.Status, RemoteReadStatus);
            server.Register(Pv2ParamInterface.ReadType, RemoteReadDescription);
            server.Register(Pv2ParamInterface.ReadValue, RemoteReadValue);
            server.Register(Pv2ParamInterface.Write, RemoteWrite);

            _needSendUpdates.Where(_ => IsSendUpdateEnabled).Buffer(TimeSpan.FromMilliseconds(500))
                .Subscribe(SendUpdate).DisposeItWith(Disposable);
        }

        public IObservable<Pv2ParamValueAndTypePair> OnRemoteUpdated => _onRemoteUpdated;

        public IObservable<Pv2ParamValueAndTypePair> CurrentAndThenRemoteUpdated =>
            _params.CopyAllToList().ToObservable().Concat(_onRemoteUpdated);


        public uint ParamsInfoHash { get; }

        public bool IsSendUpdateEnabled { get; set; } = true;

        public IRxValue<bool> IsSendingUpdate => _isSendingUpdateSubject;

        public void Write(Pv2ParamType param, Action<Pv2ParamType, Pv2ParamValue> valueWriteCallback)
        {
            if (param == null) throw new ArgumentNullException(nameof(param));
            if (valueWriteCallback == null) throw new ArgumentNullException(nameof(valueWriteCallback));
            var item = _params[param.FullName];

            var newValue = Pv2ParamInterface.CreateValue(item.Type.TypeEnum);
            valueWriteCallback(item.Type, newValue);
            item.Type.ValidateValue(newValue);
            item.Value.CopyFrom(newValue);
            if (item.Type.Flags.HasFlag(Pv2ParamFlags.ReadOnly) == false)
                item.Type.WriteToConfig(_config, _configSuffix, item.Value);
            _needSendUpdates.OnNext(item.Index);
        }

        public Pv2ParamValue Read(Pv2ParamType param)
        {
            if (param == null) throw new ArgumentNullException(nameof(param));
            var item = _params[param.FullName];
            return item.Value;
        }

        private async void SendUpdate(IList<uint> itemsToUpdate)
        {
            if (IsDisposed) return;
            if (itemsToUpdate.Count > 0) _valuesToUpdate.AddRange(itemsToUpdate);
            if (_valuesToUpdate.Count == 0) return;
            if (Interlocked.CompareExchange(ref _isUpdateInProgress, 1, 0) != 0) return;
            _isSendingUpdateSubject.OnNext(true);
            var pool = _valuesToUpdate.ToArray();

            try
            {
                var size = 0;
                var itemsToSend = new List<Pv2ParamValueItem>();
                for (var i = 0; i < pool.Length; i++)
                {
                    var paramIndex = pool[i];
                    var value = new Pv2ParamValueItem(_params[paramIndex].Value);
                    var itemSize = value.GetByteSize();

                    if (size + itemSize <= PayloadV2Helper.MaxMessageSize)
                    {
                        size += itemSize;
                        itemsToSend.Add(value);
                        _valuesToUpdate.Remove(paramIndex);
                        // if not last index, continue add values to list
                        if (i != pool.Length - 1) continue;
                        // if last index, just send and break
                        Logger.Trace($"SERVER.PARAMS ==> {_valuesToUpdate.Count}");
                        await _server.SendResult(DeviceIdentity.Broadcast, Pv2ParamInterface.UpdateEvent,
                                new Pv2ParamValueList(itemsToSend), cancel: DisposeCancel)
                            .ConfigureAwait(false);
                        await Task.Delay(_sendUpdateDelay, DisposeCancel).ConfigureAwait(false);
                        break;
                    }

                    // send part and continue
                    await _server.SendResult(DeviceIdentity.Broadcast,
                            Pv2ParamInterface.UpdateEvent, new Pv2ParamValueList(itemsToSend), cancel: DisposeCancel)
                        .ConfigureAwait(false);
                    Logger.Trace($"SERVER.PARAMS ==> {_valuesToUpdate.Count}");
                    await Task.Delay(_sendUpdateDelay, DisposeCancel).ConfigureAwait(false);
                    itemsToSend.Clear();
                    // we ignore last, couse oversize
                    --i;
                    size = 0;
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Error to send updates:{e.Message}");
            }
            finally
            {
                Interlocked.Exchange(ref _isUpdateInProgress, 0);
                _isSendingUpdateSubject.OnNext(false);
            }
        }

        private Task<(Pv2ParamValueAndTypePair, DeviceIdentity)> RemoteReadDescription(DeviceIdentity devid,
            SpanPacketUnsignedIntegerType index, CancellationToken cancel)
        {
            if (_params.Count <= index.Value)
                throw new Exception($"Wrong index {index}. Size {_params.Count}");
            var param = _params[index.Value];
            return Task.FromResult((new Pv2ParamValueAndTypePair(param.Type, param.Value, index.Value),
                DeviceIdentity.Broadcast));
        }


        public Task<(Pv2ParamValueItem, DeviceIdentity)> RemoteWrite(DeviceIdentity devid, Pv2ParamValueItem data,
            CancellationToken cancel)
        {
            if (_params.Count <= data.Value.Index)
                throw new Exception($"Wrong index {data.Value.Index}. Size {_params.Count}");
            var item = _params[data.Value.Index];
            if (item.Type.Flags.HasFlag(Pv2ParamFlags.ReadOnly))
                throw new Exception($"{item.Type.FullName} is read only.");
            var oldValue = item.Type.ConvertToString(item.Value);
            item.Type.ValidateValue(data.Value);
            item.Value.CopyFrom(data.Value);
            item.Type.WriteToConfig(_config, _configSuffix, item.Value);
            _onRemoteUpdated.OnNext(item);
            LogInfo($"{item.Type.FullName}:{oldValue}=>{item.Type.ConvertToString(item.Value)}");
            return Task.FromResult((new Pv2ParamValueItem(item.Value), DeviceIdentity.Broadcast));
        }

        private Task<(Pv2ParamValueItem, DeviceIdentity)> RemoteReadValue(DeviceIdentity devid,
            SpanPacketUnsignedIntegerType index, CancellationToken cancel)
        {
            if (_params.Count <= index.Value)
                throw new Exception($"Wrong index {index}. Size {_params.Count}");
            var item = _params[index.Value];
            return Task.FromResult((new Pv2ParamValueItem(item.Value), DeviceIdentity.Broadcast));
        }

        private Task<(Pv2ParamsStatus, DeviceIdentity)> RemoteReadStatus(DeviceIdentity devid, SpanVoidType data,
            CancellationToken cancel)
        {
            return Task.FromResult((new Pv2ParamsStatus
            {
                DescriptionHash = ParamsInfoHash,
                Count = (uint)_params.Count
            }, DeviceIdentity.Broadcast));
        }
    }
}
