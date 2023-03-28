using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Payload.Digits;
using NLog;

namespace Asv.Mavlink.Payload
{
    public interface IPv2ServerRttInterface
    {
        object this[Pv2RttFieldDesc field] { get; set; }
        bool IsRecordStarted { get; }
        IServerRxParam<uint> RecordTime { get; }
        IServerRxParam<bool> SendingEnabled { get; }
        IServerRxParam<uint> SendingTime { get; }
        SessionMetadata StartSession(SessionSettings sessionSetting);
        void StopSession();
    }


    public class Pv2ServerRttInterface : Pv2ServerInterfaceBase, IPv2ServerRttInterface
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<uint, Pv2RttFieldDesc> _fieldsDictionary = new();

        private readonly IPv2ServerBaseInterface _baseIfc;
        private readonly IPv2ServerParamsInterface _paramsSvc;
        private readonly HashSet<ushort> _recordEnabledGroups = new();

        private readonly Dictionary<ushort, (Pv2RttRecordDesc, SortedDictionary<byte, Pv2RttFieldDesc>)>
            _rttDescriptions = new();

        private readonly ConcurrentDictionary<uint, object> _rttValues = new();
        private readonly HashSet<ushort> _sendingEnabledGroups = new();
        private readonly TimeSpan _sendUpdateDelay = TimeSpan.FromMilliseconds(1);
        private readonly IChunkStore _store;
        private Pv2RttFieldDesc[] _fieldsArray;
        private DateTime _lastSendTime = DateTime.Now;
        private uint _localHash;
        private Pv2RttRecordDesc[] _recordArray;
        private double _recordTickCompleteFlag;
        private Pv2ServerRxParam<uint, Pv2UIntParamType, Pv2UIntParamValue> _recordTimeParam;
        private Pv2ServerRxParam<bool, Pv2BoolParamType, Pv2BoolParamValue> _sendingEnabledParam;
        private Pv2ServerRxParam<uint, Pv2UIntParamType, Pv2UIntParamValue> _sendTickTimeParam;
        private int _skippedRecordTicks;
        private long _tick;
        private IDisposable _timerSubscribe;


        public Pv2ServerRttInterface(IPayloadV2Server server, IPv2ServerBaseInterface baseIfc, IPv2ServerParamsInterface paramsSvc, IChunkStore store,
            IEnumerable<Pv2RttRecordDesc> rttRecords,
            IEnumerable<Pv2RttFieldDesc> rttFields) : base(server, Pv2RttInterface.InterfaceName)
        {
            _baseIfc = baseIfc;
            _paramsSvc = paramsSvc;
            _store = store;
            InitParams();
            InitFieldsAndRecords(rttRecords, rttFields);


            Server.Register(Pv2RttInterface.Status, InternalReadStatus);
            Server.Register(Pv2RttInterface.ReadRecordDesc, InternalReadRecord);
            Server.Register(Pv2RttInterface.ReadFieldDesc, InternalReadField);
            Server.Register(Pv2RttInterface.StartSession, StartSession);
            Server.Register(Pv2RttInterface.StopSession, StopSession);

            Server.Register(Pv2RttInterface.GetSessionStoreInfo, GetSessionStoreInfo);
            Server.Register(Pv2RttInterface.GetSessionInfo, GetSessionInfo);
            Server.Register(Pv2RttInterface.GetSessionInfoByGuid, GetSessionInfoByGuid);
            Server.Register(Pv2RttInterface.GetSessionFieldInfo, GetSessionFieldInfo);
            Server.Register(Pv2RttInterface.GetSessionFieldData, GetSessionRecordData);
            Server.Register(Pv2RttInterface.DeleteSessionByGuid, DeleteSessionByGuid);


            foreach (var records in rttRecords)
            {
                _recordEnabledGroups.Add(records.Id);
                _sendingEnabledGroups.Add(records.Id);
            }

            _recordTimeParam.Subscribe(GlobalTickChanged).DisposeItWith(Disposable);
        }

        

        public object this[Pv2RttFieldDesc field]
        {
            get => _rttValues[field.FullId];
            set
            {
                _rttDescriptions[field.GroupId].Item2[field.Id].ValidateValue(value);
                _rttValues[field.FullId] = value;
            }
        }

        public bool IsRecordStarted => _store.IsStarted;
        public IServerRxParam<uint> RecordTime => _recordTimeParam;
        public IServerRxParam<uint> SendingTime => _sendTickTimeParam;
      

        public IServerRxParam<bool> SendingEnabled => _sendingEnabledParam;

        private void GlobalTickChanged(uint tickMs)
        {
            Logger.Info($"RTT change tick to {tickMs}");
            LogInfo($"Change RTT tick {tickMs} ms");
            if (_timerSubscribe != null)
            {
                _timerSubscribe.Dispose();
                Disposable.Remove(_timerSubscribe);
            }

            _timerSubscribe = Observable.Timer(TimeSpan.FromMilliseconds(_recordTimeParam.Value),
                    TimeSpan.FromMilliseconds(_recordTimeParam.Value))
                .Subscribe(RecordTick);
            Disposable.Add(_timerSubscribe);
        }

        private Task<(Pv2RttGetFieldsDataResult, DeviceIdentity)> GetSessionRecordData(DeviceIdentity devid,
            Pv2RttGetFieldsDataArgs data, CancellationToken cancel)
        {
            var index = data.StartIndex;
            if (_fieldsDictionary.TryGetValue(data.FieldId, out var field) == false)
                throw new Exception($"Unknown field {data.FieldId}");
            var maxSize = Pv2RttGetFieldsDataResult.MaxDataSize;
            var resultArray = ArrayPool<byte>.Shared.Rent(maxSize);
            var resultSpan = new Span<byte>(resultArray, 0, maxSize);
            try
            {
                var currentSize = 0;
                uint count = 0;
                int bitIndex = 0;
                for (var i = 0; i < data.Take; i++)
                {
                    if (currentSize + field.GetValueMaxByteSize() >= maxSize) break;

                    object value = null;
                    var result = _store.ReadRecord(data.SessionId, data.FieldId, (uint)(index + i),
                        (ref ReadOnlySpan<byte> span) =>
                        {
                            var tmp = 0;
                            value = field.DeserializeValue(span, ref tmp);
                        });
                    if (result == false)
                        // store is empty
                        break;
                    field.SerializeValue(resultSpan, value, ref bitIndex);
                    currentSize = bitIndex % 8 == 0 ? bitIndex / 8 : bitIndex / 8 + 1;
                    ++count;
                }

                return Task.FromResult((new Pv2RttGetFieldsDataResult(count, resultArray, (uint)currentSize), devid));
            }
            catch (Exception e)
            {
                return Task.FromException<(Pv2RttGetFieldsDataResult, DeviceIdentity)>(e);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(resultArray);
            }
        }

        private Task<(SessionFieldInfo, DeviceIdentity)> GetSessionFieldInfo(DeviceIdentity devid,
            Pv2RttGetFieldsArgs data, CancellationToken cancel)
        {
            try
            {
                var rec = _store.GetFieldsIds(data.SessionId).Skip((int)data.FieldIndex).First();
                var info = _store.GetFieldInfo(data.SessionId, rec);
                if (info == null)
                    throw new Exception($"Record not found {data.SessionId} with index [{data.FieldIndex}]");

                return Task.FromResult((info, devid));
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{LogSend} Error to {nameof(GetSessionStoreInfo)}: {e.Message}");
                return Task.FromException<(SessionFieldInfo, DeviceIdentity)>(e);
            }
        }



        private Task<(SessionInfo, DeviceIdentity)> GetSessionInfoByGuid(DeviceIdentity devid, SessionId sessionId,
            CancellationToken cancel)
        {
            try
            {
                var info = _store.GetSessionInfo(sessionId);
                if (info == null)
                    throw new Exception($"Session {sessionId} not found");

                return Task.FromResult((info, devid));
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{LogSend} Error to {nameof(GetSessionInfoByGuid)}: {e.Message}");
                return Task.FromException<(SessionInfo, DeviceIdentity)>(e);
            }
        }

        private Task<(SpanBoolType, DeviceIdentity)> DeleteSessionByGuid(DeviceIdentity devid, SessionId sessionId, CancellationToken cancel)
        {
            try
            {
                var info = _store.GetSessionInfo(sessionId);
                if (info == null)
                    throw new Exception($"Session {sessionId} not found");
                var result = _store.Delete(sessionId);
                return Task.FromResult((new SpanBoolType(result), devid));
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{LogSend} Error to {nameof(DeleteSessionByGuid)}: {e.Message}");
                return Task.FromException<(SpanBoolType, DeviceIdentity)>(e);
            }
        }

        private Task<(SessionInfo, DeviceIdentity)> GetSessionInfo(DeviceIdentity devid,
            SpanPacketUnsignedIntegerType data, CancellationToken cancel)
        {
            try
            {
                var guid = _store.GetSessions().Skip((int)data.Value).FirstOrDefault();
                if (guid == default)
                    throw new Exception($"Wrong index {data.Value}. Size {_store.GetSessions().Count()}");
                var info = _store.GetSessionInfo(guid);
                if (info == null)
                    throw new Exception($"Session [{data.Value}] {guid} not found");

                return Task.FromResult((info, devid));
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{LogSend} Error to {nameof(GetSessionStoreInfo)}: {e.Message}");
                return Task.FromException<(SessionInfo, DeviceIdentity)>(e);
            }
        }

        private Task<(Pv2RttSessionStoreInfo, DeviceIdentity)> GetSessionStoreInfo(DeviceIdentity devid,
            SpanVoidType data, CancellationToken cancel)
        {
            try
            {
                return Task.FromResult((new Pv2RttSessionStoreInfo
                {
                    SessionCount = (uint)_store.GetSessions().Count()
                }, devid));
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{LogSend} Error to {nameof(GetSessionStoreInfo)}: {e.Message}");
                return Task.FromException<(Pv2RttSessionStoreInfo, DeviceIdentity)>(e);
            }
        }

        

        #region Interface implementation

        public void StopSession()
        {
            if (!_store.IsStarted) return;
            _store.Stop();
            _baseIfc.UpdateDeviceInfo(_ => _.RttRecordEnabled = false);
        }

        private Task<(SpanBoolType, DeviceIdentity)> StopSession(DeviceIdentity devid, SpanVoidType data,
            CancellationToken cancel)
        {
            try
            {
                StopSession();
                return Task.FromResult((new SpanBoolType(true), devid));
            }
            catch (Exception e)
            {
                return Task.FromException<(SpanBoolType, DeviceIdentity)>(e);
            }
        }

        public SessionMetadata StartSession(SessionSettings sessionSetting)
        {
            SessionMetadata info;
            if (IsRecordStarted == false)
            {
                var recSettings = _recordEnabledGroups.SelectMany(_ => _rttDescriptions[_].Item2).Select(_ =>
                    new SessionFieldSettings(_.Value.FullId, _.Value.Name,
                        (ushort)_.Value.GetValueMaxByteSize()));
                info = _store.Start(sessionSetting, recSettings);

                _baseIfc.UpdateDeviceInfo(_ => _.RttRecordEnabled = true);
            }
            else
            {
                info = _store.Current;
            }
            return info;
        }

        

        private Task<(Pv2RttSessionMetadataType, DeviceIdentity)> StartSession(DeviceIdentity devid,
            SessionSettings data, CancellationToken cancel)
        {
            try
            {
                var info = StartSession(data);
                var meta = new Pv2RttSessionMetadataType
                {
                    IsEnabled = true,
                    Session = info
                };
                return Task.FromResult((meta, devid));
            }
            catch (Exception e)
            {
                return Task.FromException<(Pv2RttSessionMetadataType, DeviceIdentity)>(e);
            }
        }

        private Task<(Pv2RttStatus, DeviceIdentity)> InternalReadStatus(DeviceIdentity devid, SpanVoidType data,
            CancellationToken cancel)
        {
            return Task.FromResult((
                new Pv2RttStatus
                {
                    DescriptionHash = _localHash,
                    FieldsCount = (uint)_fieldsArray.Length,
                    RecordsCount = (uint)_recordArray.Length
                }, devid));
        }

        private Task<(Pv2RttRecordDesc, DeviceIdentity)> InternalReadRecord(DeviceIdentity devid,
            SpanPacketUnsignedIntegerType data, CancellationToken cancel)
        {
            if (data.Value >= _recordArray.Length)
                Task.FromException(new Exception($"Worng index {data.Value}. Max {_fieldsArray.Length}"));
            return Task.FromResult((_recordArray[data.Value], devid));
        }

        private Task<(Pv2RttFieldResult, DeviceIdentity)> InternalReadField(DeviceIdentity devid,
            SpanPacketUnsignedIntegerType data, CancellationToken cancel)
        {
            try
            {
                if (data.Value >= _fieldsArray.Length)
                    Task.FromException(new Exception($"Worng index {data.Value}. Max {_fieldsArray.Length}"));
                return Task.FromResult((new Pv2RttFieldResult(_fieldsArray[data.Value]), devid));
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Init

        private void InitFieldsAndRecords(IEnumerable<Pv2RttRecordDesc> rttRecords,
            IEnumerable<Pv2RttFieldDesc> rttFields)
        {
            _fieldsArray = rttFields.OrderBy(_ => _.FullId).ToArray();
            _recordArray = rttRecords.OrderBy(_ => _.Id).ToArray();

            foreach (var field in _fieldsArray)
                if (_recordArray.FirstOrDefault(_ => _.Id == field.GroupId) == null)
                    throw new ArgumentException(
                        $"Record id {field.GroupId} for RTT field {field} not found");

            var duplicateFullName = _fieldsArray.GroupBy(_ => _.FullName).Where(_ => _.Count() > 1);
            foreach (var field in duplicateFullName)
                throw new ArgumentException(
                    $"Found duplicate RTT field FullName='{field.Key}': {string.Join(", ", field)} ");

            foreach (var rec in _recordArray)
                if (_fieldsArray.Any(_ => _.GroupId == rec.Id) == false)
                    throw new ArgumentException(
                        $"Record {rec} has not fields. It can not be empty.");

            var duplicatesRecords = _recordArray.GroupBy(_ => _.Id).Where(_ => _.Count() > 1);
            foreach (var record in duplicatesRecords)
                throw new ArgumentException(
                    $"Found duplicate RTT records with same id='{record.Key}': {string.Join(", ", record)} ");

            var duplicatesFields = _fieldsArray.GroupBy(_ => _.FullId).Where(_ => _.Count() > 1);
            foreach (var field in duplicatesFields)
                throw new ArgumentException(
                    $"Found duplicate RTT fields with same FullId='{field.Key}': {string.Join(", ", field)} ");


            _localHash = Pv2RttInterface.CalculateHash(_fieldsArray, _recordArray);
            var desc = _recordArray.ToDictionary(_ => _.Id, _ => _);
            foreach (var group in _fieldsArray.GroupBy(_ => _.GroupId))
            {
                var dict = new SortedDictionary<byte, Pv2RttFieldDesc>();
                if (desc.TryGetValue(group.Key, out var record) == false)
                    throw new Exception($"Couldn't find record description with id={group.Key}");
                _rttDescriptions.Add(group.Key, (record, dict));
                foreach (var item in group)
                {
                    dict.Add(item.Id, item);
                    _rttValues.AddOrUpdate(item.FullId, item.DefaultValue, (k, v) => item.DefaultValue);
                    _fieldsDictionary.Add(item.FullId, item);
                }

                var sizeValidate = dict.Values.Sum(_ => _.GetValueMaxByteSize()) +
                                   Pv2RttStreamMetadata.MaxMetadataSizeWithOneGroup;
                if (sizeValidate >= PayloadV2Helper.MaxMessageSize)
                    throw new Exception(
                        $"Size of one record data in group must be less then {Pv2RttInterface.MaxOnStreamDataSize} bytes. Now {sizeValidate} bytes. Group id {group.Key}");
            }
        }

        private void InitParams()
        {
            _recordTimeParam =
                new Pv2ServerRxParam<uint, Pv2UIntParamType, Pv2UIntParamValue>(_paramsSvc,
                    Pv2RttInterface.RecordTickTime);
            Disposable.Add(_recordTimeParam);

            _sendTickTimeParam =
                new Pv2ServerRxParam<uint, Pv2UIntParamType, Pv2UIntParamValue>(_paramsSvc,
                    Pv2RttInterface.SendTickTime);
            Disposable.Add(_sendTickTimeParam);

            _sendingEnabledParam =
                new Pv2ServerRxParam<bool, Pv2BoolParamType, Pv2BoolParamValue>(_paramsSvc,
                    Pv2RttInterface.SendEnabled);
            Disposable.Add(_sendingEnabledParam);
        }

        #endregion

        #region Ticks

        private async void RecordTick(long increment)
        {
            if (Interlocked.CompareExchange(ref _recordTickCompleteFlag, 1, 0) != 0)
            {
                _logger.Error($"Record tick skiped {++_skippedRecordTicks}");
                return;
            }

            var counter = Interlocked.Increment(ref _tick);
            if (counter >= uint.MaxValue) Interlocked.Exchange(ref _tick, 0);

            try
            {
                var fileIndex = RecordToFile((uint)counter);
                if (_sendingEnabledParam.Value &&
                    (DateTime.Now - _lastSendTime).TotalMilliseconds > _sendTickTimeParam.Value)
                {
                    _lastSendTime = DateTime.Now;
                    await SendToLink(fileIndex);
                }
            }

            catch (Exception e)
            {
                _logger.Error(e, $"Record tick error:{e.Message}");
            }
            finally
            {
                Interlocked.Exchange(ref _recordTickCompleteFlag, 0);
            }
        }

        private async Task SendToLink(uint counter)
        {
            try
            {
                var itemsToSend = new List<ushort>();
                var summ = 0;
                foreach (var recId in _sendingEnabledGroups)
                {
                    if (_rttDescriptions.TryGetValue(recId, out var record) == false) continue;
                    var bitSize = record.Item2.Sum(_ => _.Value.GetValueBitSize(_rttValues[_.Value.FullId]));
                    var byteSize = bitSize % 8 == 0 ? bitSize / 8 : bitSize / 8 + 1;
                    var sizeAdded = byteSize + BinSerialize.GetSizeForPackedUnsignedInteger(recId);

                    if (summ + sizeAdded + Pv2RttStreamMetadata.MetadataSizeWithoutGroupIds <
                        Pv2RttInterface.MaxOnStreamDataSize)
                    {
                        summ += sizeAdded;
                        itemsToSend.Add(recId);
                        continue;
                    }

                    // time to send data
                    await SendUpdates(itemsToSend, summ, counter);
                    itemsToSend.Clear();
                    summ = 0;
                }

                if (itemsToSend.Count > 0) await SendUpdates(itemsToSend, summ, counter);
            }
            catch (Exception)
            {
                Debug.Assert(false, "Error to send data");
            }
        }

        private async Task SendUpdates(List<ushort> itemsToSend, int byteSize, uint counter)
        {
            var metadata = new Pv2RttStreamMetadata
            {
                SessionId = new SessionId(_store.Current?.SessionId.Guid ?? Guid.Empty),
                Counter = counter,
                Flags = _store.IsStarted ? Pv2StreamFlags.RecordEnabled : Pv2StreamFlags.NoFlags,
                Groups = itemsToSend
            };
            // bytesize - size with
            var allPacketSize = byteSize + metadata.GetByteSize();
            var dataArr = ArrayPool<byte>.Shared.Rent(allPacketSize);
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    var buffer = new Span<byte>(dataArr, 0, allPacketSize);
                    metadata.Serialize(ref buffer);
                    var bitIndex = 0;
                    foreach (var recId in itemsToSend)
                    foreach (var item in _rttDescriptions[recId].Item2)
                        item.Value.SerializeValue(buffer, _rttValues[item.Value.FullId], ref bitIndex);
                });
                await Server.SendResult(DeviceIdentity.Broadcast, Pv2RttInterface.OnStream,
                    new Pv2RttStreamData(dataArr), (ref Span<byte> data, Pv2RttStreamData value) =>
                    {
                        var from = new ReadOnlySpan<byte>(value.Data, 0, allPacketSize);
                        from.CopyTo(data);
                    }, _ => allPacketSize);
                await Task.Delay(_sendUpdateDelay, DisposeCancel).ConfigureAwait(false);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(dataArr);
            }
        }

        private uint RecordToFile(uint counter)
        {
            if (_store.IsStarted == false) return counter;
            try
            {
                var indexes = new HashSet<uint>();
                foreach (var recId in _recordEnabledGroups)
                foreach (var item in _rttDescriptions[recId].Item2)
                    indexes.Add(_store.Append(item.Value.FullId, (ref Span<byte> data) =>
                    {
                        var bitIndex = 0;
                        item.Value.SerializeValue(data, _rttValues[item.Value.FullId], ref bitIndex);
                        data = data.Slice(item.Value.GetValueMaxByteSize());
                    }));
                Debug.Assert(indexes.Count == 1);
                return indexes.First();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error to record data");
                Debug.Assert(false, e.Message);
                throw;
            }
        }

        #endregion
    }
}
