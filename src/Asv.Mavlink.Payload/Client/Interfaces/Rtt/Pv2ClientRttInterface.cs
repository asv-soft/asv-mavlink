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
using Asv.Mavlink.Payload.Digits;
using NLog;

namespace Asv.Mavlink.Payload
{
    public interface IPv2ClientRttInterface
    {
        IObservable<RttUpdate> OnUpdate { get; }
        IClientRxParam<uint> RecordTime { get; }
        IRxValue<Pv2RttCollection> Records { get; }
        IRxValue<Pv2RttStreamMetadata> SessionInfo { get; }
        Task<bool> RequestAll(CancellationToken cancel = default, IProgress<(double, string)> callback = null);
        Task<Pv2RttSessionMetadataType> StartRecord(SessionSettings settings, CancellationToken cancel = default);
        Task<bool> StopRecord(CancellationToken cancel = default);
        Task<Pv2RttSessionStoreInfo> GetStoreInfo(CancellationToken cancel = default);
        Task<SessionInfo> GetSessionInfo(SessionId sessionId, CancellationToken cancel = default);
        Task<SessionInfo> GetSessionInfo(uint sessionIndex, CancellationToken cancel = default);

        Task<SessionFieldInfo> GetSessionFieldInfo(SessionId sessionId, uint recordIndex,
            CancellationToken cancel = default);

        Task<bool> DeleteSession(SessionId id, CancellationToken cancel = default);
        Task<(Pv2RttFieldDesc desc, (uint index, object value)[] data)> GetSessionFieldData(SessionId sessionId,
            uint fieldFullId, uint start, byte count, CancellationToken cancel = default);
    }

    public delegate bool TryGetDataDelegate<T>(int index, out T value);

    public delegate void DataDelegate<in T>(int index, T value, bool isNew);

    public static class Pv2ClientRttInterfaceHelper
    {
        public static IObservable<RttUpdate> OnUpdate(this IPv2ClientRttInterface src, Pv2RttFieldDesc field)
        {
            return src.OnUpdate.Where(_ => _.Field.FullId == field.FullId);
        }

        public static IObservable<(RttUpdate, RttUpdate)> OnUpdate(this IPv2ClientRttInterface src, Pv2RttFieldDesc field1,Pv2RttFieldDesc field2)
        {
            return src.OnUpdate(field1).Zip(src.OnUpdate(field2), (a, b) => (a, b));
        }

        public static async Task<SessionFieldInfo[]> GetSessionFields(this IPv2ClientRttInterface src, SessionId id,
            Action<SessionFieldInfo> onData = null, Action<double> progress = null, CancellationToken cancel = default)
        {
            var result = await src.GetSessionInfo(id, cancel);
            return await GetSessionFields(src, result, onData, progress, cancel);
        }

        public static async Task<SessionFieldInfo[]> GetSessionFields(this IPv2ClientRttInterface src, SessionInfo info,
            Action<SessionFieldInfo> onData = null, Action<double> progress = null, CancellationToken cancel = default)
        {
            progress?.Invoke(0.0);
            var array = new SessionFieldInfo[info.FieldsCount];
            for (uint i = 0; i < info.FieldsCount; i++)
            {
                var result = await src.GetSessionFieldInfo(info.Metadata.SessionId, i, cancel);
                progress?.Invoke((double)(i + 1) / info.FieldsCount);
                onData?.Invoke(result);
            }

            return array;
        }

        public static async Task<SessionInfo[]> GetSessionList(this IPv2ClientRttInterface src,
            Action<SessionInfo> onData = null, Action<double> progress = null, CancellationToken cancel = default)
        {
            var result = await src.GetStoreInfo(cancel);
            var array = new SessionInfo[result.SessionCount];
            progress?.Invoke(0.0);
            for (var i = 0; i < array.Length; i++)
            {
                var info = await src.GetSessionInfo((uint)i, cancel);
                array[i] = info;
                onData?.Invoke(info);
                progress?.Invoke((double)(i + 1) / array.Length);
            }

            return array;
        }

        public static async Task DownloadData<T>(this IPv2ClientRttInterface src, SessionId session,
            SessionFieldInfo field, TryGetDataDelegate<T> tryGetCallback, DataDelegate<T> onData,
            Action<double> progress = null, CancellationToken cancel = default)
        {
            var lastUnknownIndex = 0;
            var total = 0;
            var state = 0;
            var count = 0;
            var maxPacketSize = Pv2RttGetFieldsDataResult.MaxDataSize / field.Metadata.Settings.Offset;
            progress ??= _ => { };

            for (var i = 0; i < field.Count; i++)
            {
                if (cancel.IsCancellationRequested) break;
                switch (state)
                {
                    // local store contain item
                    case 0:
                        if (tryGetCallback(i, out var value1))
                        {
                            onData(i, value1, false);
                            total++;
                            progress((double)total / field.Count);
                        }
                        else
                        {
                            lastUnknownIndex = i;
                            count = 1;
                            state = 1;
                        }

                        break;
                    // add items to sync
                    case 1:
                        if (tryGetCallback(i, out var value2))
                        {
                            onData(i, value2, false);
                            total++;
                            progress((double)total / field.Count);
                            goto case 3;
                        }

                        count++;
                        if (count >= maxPacketSize) goto case 3;

                        if (i + 1 >= field.Count) goto case 3;
                        break;
                    // upload items from remote store
                    case 3:
                        var uploadedData = 0;
                        state = 0;
                        while (true)
                            try
                            {
                                var result = await src.GetSessionFieldData(session, field.Metadata.Settings.Id,
                                    (uint)(lastUnknownIndex + uploadedData), (byte)(count - uploadedData), cancel);
                                uploadedData += result.data.Length;

                                foreach (var tuple in result.data) onData((int)tuple.index, (T)tuple.value, true);
                                total += result.data.Length;
                                progress((double)total / field.Count);
                                if (uploadedData >= count) break;
                            }
                            catch (Exception)
                            {
                                if (cancel.IsCancellationRequested) break;
                            }

                        break;
                }
            }
        }
    }


    public class Pv2ClientRttInterfaceConfig
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

    public class Pv2ClientRttInterface : Pv2ClientInterfaceBase, IPv2ClientRttInterface
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Pv2ClientRttInterfaceConfig _config;
        private readonly Subject<RttUpdate> _onRttUpdate = new();
        private readonly RxValue<Pv2RttCollection> _recordsCollection = new();
        private readonly Pv2ClientRxParam<uint, Pv2UIntParamType, Pv2UIntParamValue> _recordTime;
        private readonly IPv2RttDescriptionStore _rttDescriptionStore;
        private readonly RxValue<Pv2RttStreamMetadata> _sessionInfoSubject = new();
        private int _isRequestAllInProgress;
        private uint _localHash;

        public Pv2ClientRttInterface(IPayloadV2Client client, IPv2ClientParamsInterface paramSvc,
            IPv2RttDescriptionStore rttDescriptionStore, Pv2ClientRttInterfaceConfig config) : base(client,
            Pv2RttInterface.InterfaceName)
        {
            _config = config;
            _rttDescriptionStore = rttDescriptionStore;
            Disposable.Add(_recordsCollection);
            Disposable.Add(_onRttUpdate);


            _recordTime =
                new Pv2ClientRxParam<uint, Pv2UIntParamType, Pv2UIntParamValue>(paramSvc,
                    Pv2RttInterface.RecordTickTime);
            Disposable.Add(_recordTime);
            _onRttUpdate.Select(_ => _.Metadata).Subscribe(_sessionInfoSubject).DisposeItWith(Disposable);
            Disposable.Add(_sessionInfoSubject);
            Client.Subscribe(Pv2RttInterface.OnStream, DeserializeStream).Subscribe().DisposeItWith(Disposable);
        }

        public Pv2ClientRttInterface(IPayloadV2Client client, IPv2ClientParamsInterface paramSvc,
            IPv2RttDescriptionStore rttDescriptionStore, IConfiguration config)
            : this(client, paramSvc, rttDescriptionStore, config.Get<Pv2ClientRttInterfaceConfig>())
        {
        }

        public async Task<bool> RequestAll(CancellationToken cancel = default,
            IProgress<(double, string)> callback = null)
        {
            if (Interlocked.CompareExchange(ref _isRequestAllInProgress, 0, 1) != 0) return false;
            try
            {
                var currentList = _recordsCollection.Value;
                _logger.Trace($"{LogSend} Request RTT description info...");
                var status = await Client.Call(Pv2RttInterface.Status, SpanVoidType.Default, cancel)
                    .ConfigureAwait(false);
                _logger.Trace(
                    $"{LogRecv} Records={status.RecordsCount}, Fields={status.FieldsCount}. Hash={status.DescriptionHash}");
                if (_localHash == status.DescriptionHash && currentList != null)
                {
                    _logger.Trace("Local RTT description valid");
                    return true;
                }

                // try restore from store by hash
                if (!_rttDescriptionStore.TyGetFromCache(status.DescriptionHash, status.RecordsCount,
                        status.FieldsCount,
                        out var records, out var fields))
                {
                    records = new List<Pv2RttRecordDesc>((int)status.RecordsCount);
                    fields = new List<Pv2RttFieldDesc>((int)status.FieldsCount);
                    _logger.Trace($"RTT description not found in local store {_rttDescriptionStore}");
                    _logger.Trace(
                        $"{LogSend} Read {status.RecordsCount} records by {_config.ParallelOperationToRead} items");
                    // not found in local cache => read all :(
                    var readRecordsTasks = Enumerable.Range(0, (int)status.RecordsCount).Select(_ =>
                        Client.Call(Pv2RttInterface.ReadRecordDesc, new SpanPacketUnsignedIntegerType((uint)_),
                            cancel));
                    foreach (var tasks in readRecordsTasks.Chunked(_config.ParallelOperationToRead))
                        records.AddRange(await Task.WhenAll(tasks).ConfigureAwait(false));

                    _logger.Trace(
                        $"{LogSend} Read {status.FieldsCount} fields by {_config.ParallelOperationToRead} items");
                    var readFieldsTasks = Enumerable.Range(0, (int)status.FieldsCount).Select(_ =>
                        Client.Call(Pv2RttInterface.ReadFieldDesc, new SpanPacketUnsignedIntegerType((uint)_), cancel));

                    foreach (var tasks in readFieldsTasks.Chunked(_config.ParallelOperationToRead))
                    {
                        var items = await Task.WhenAll(tasks).ConfigureAwait(false);
                        fields.AddRange(items.Select(_ => _.Desc));
                    }
                }

                _localHash = status.DescriptionHash;
                var list = new Pv2RttCollection(records, fields);
                _recordsCollection.OnNext(list);
                _rttDescriptionStore.Save(_localHash, records, fields);
            }
            finally
            {
                Interlocked.Exchange(ref _isRequestAllInProgress, 0);
            }

            return true;
        }

        public IClientRxParam<uint> RecordTime => _recordTime;
        public IRxValue<Pv2RttCollection> Records => _recordsCollection;
        public IRxValue<Pv2RttStreamMetadata> SessionInfo => _sessionInfoSubject;

        public Task<Pv2RttSessionMetadataType> StartRecord(SessionSettings settings, CancellationToken cancel = default)
        {
            return Client.Call(Pv2RttInterface.StartSession, settings, cancel);
        }

        public async Task<bool> StopRecord(CancellationToken cancel = default)
        {
            var result = await Client.Call(Pv2RttInterface.StopSession, SpanVoidType.Default, cancel);
            return result.Value;
        }

        public Task<Pv2RttSessionStoreInfo> GetStoreInfo(CancellationToken cancel = default)
        {
            return Client.Call(Pv2RttInterface.GetSessionStoreInfo, SpanVoidType.Default, cancel);
        }

        public Task<SessionInfo> GetSessionInfo(SessionId sessionId, CancellationToken cancel = default)
        {
            return Client.Call(Pv2RttInterface.GetSessionInfoByGuid, sessionId, cancel);
        }

        public Task<SessionInfo> GetSessionInfo(uint sessionIndex, CancellationToken cancel = default)
        {
            return Client.Call(Pv2RttInterface.GetSessionInfo, new SpanPacketUnsignedIntegerType(sessionIndex), cancel);
        }

        public Task<SessionFieldInfo> GetSessionFieldInfo(SessionId sessionId, uint recordIndex,
            CancellationToken cancel = default)
        {
            return Client.Call(Pv2RttInterface.GetSessionFieldInfo, new Pv2RttGetFieldsArgs(sessionId, recordIndex),
                cancel);
        }

        public async Task<bool> DeleteSession(SessionId id, CancellationToken cancel = default)
        {
            _logger.Info($"{LogSend} Remove session with id {id}");
            var result = await Client.Call(Pv2RttInterface.DeleteSessionByGuid, id, cancel: cancel);
            return result;
        }

        public async Task<(Pv2RttFieldDesc desc, (uint index, object value)[] data)> GetSessionFieldData(
            SessionId sessionId, uint fieldFullId, uint start, byte count, CancellationToken cancel = default)
        {
            if (_recordsCollection.Value == null)
                throw new Exception($"RTT description is empty. Call {nameof(RequestAll)} first");
            if (_recordsCollection.Value.TryGetFieldsWithId(fieldFullId, out var fieldDesc) == false)
                throw new Exception($"RTT field field with id {fieldFullId} not found");
            var result = await Client.Call(Pv2RttInterface.GetSessionFieldData,
                new Pv2RttGetFieldsDataArgs(sessionId, fieldFullId, start, count), cancel);
            var bitIndex = 0;
            var items = new (uint index, object value)[result.Count];
            await Task.Factory.StartNew(() =>
            {
                var span = new ReadOnlySpan<byte>(result.Data);
                for (var i = 0; i < result.Count; i++)
                    items[i] = ((uint)(start + i), fieldDesc.DeserializeValue(span, ref bitIndex));
            }, cancel);
            return (fieldDesc, items);
        }

        public IObservable<RttUpdate> OnUpdate => _onRttUpdate;

        private Pv2RttStreamData DeserializeStream(ref ReadOnlySpan<byte> data)
        {
            var collection = _recordsCollection.Value;
            if (collection == null) return new Pv2RttStreamData("[]");
            var metadata = new Pv2RttStreamMetadata();
            var size = data.Length;
            metadata.Deserialize(ref data);
            var bitIndex = 0;
            foreach (var item in metadata.Groups)
            {
                if (collection.TryGetFieldsForRecordWithId(item, out var record, out var fields) == false)
                {
                    _logger.Warn($"Receive unknown RTT group with id {item}. Ignore it.");
                    continue;
                }

                foreach (var field in fields)
                {
                    var value = field.DeserializeValue(data, ref bitIndex);
                    _onRttUpdate.OnNext(new RttUpdate(metadata, record, field, value));
                }
            }

            size = size - data.Length;
            return new Pv2RttStreamData($"{size} bytes, groups:{string.Join(",", metadata.Groups)}");
            ;
        }
    }
}
