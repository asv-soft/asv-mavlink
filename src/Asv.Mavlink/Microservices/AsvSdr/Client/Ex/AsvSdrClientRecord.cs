using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using DynamicData;

namespace Asv.Mavlink;

public class AsvSdrClientRecord:DisposableOnceWithCancel, IAsvSdrClientRecord
{
    private readonly RxValue<DateTime> _created;
    private readonly RxValue<ushort> _tagsCount;
    private readonly RxValue<uint> _dataCount;
    private readonly RxValue<uint> _byteSize;
    private readonly RxValue<TimeSpan> _duration;
    private readonly SourceCache<AsvSdrClientRecordTag,TagId> _tags;
    private readonly TimeSpan _maxTimeToWaitForResponseForList;
    private readonly RxValue<AsvSdrCustomMode> _dataType;
    private readonly RxValue<ulong> _frequency;
    private readonly IAsvSdrClient _client;
    private readonly RxValue<string> _name;

    public AsvSdrClientRecord(Guid id, AsvSdrRecordPayload payload, IAsvSdrClient client, AsvSdrClientExConfig config )
    {
        if (payload == null) throw new ArgumentNullException(nameof(payload));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        Id = id;
        _name = new RxValue<string>(MavlinkTypesHelper.GetString(payload.RecordName))
            .DisposeItWith(Disposable);
        _dataType = new RxValue<AsvSdrCustomMode>(payload.DataType)
            .DisposeItWith(Disposable);
        _frequency = new RxValue<ulong>(payload.Frequency)
            .DisposeItWith(Disposable);
        _created = new RxValue<DateTime>(MavlinkTypesHelper.FromUnixTimeUs(payload.CreatedUnixUs))
            .DisposeItWith(Disposable);
        _duration = new RxValue<TimeSpan>(TimeSpan.FromSeconds(payload.DurationSec))
            .DisposeItWith(Disposable);
        _tagsCount = new RxValue<ushort>(payload.TagCount)
            .DisposeItWith(Disposable);
        _dataCount = new RxValue<uint>(payload.DataCount)
            .DisposeItWith(Disposable);
        _byteSize = new RxValue<uint>(payload.Size)
            .DisposeItWith(Disposable);
        _tags = new SourceCache<AsvSdrClientRecordTag, TagId>(x => x.Id)
            .DisposeItWith(Disposable);
        client.OnRecord
            .Where(_ => _.Item1 == Id)
            .Subscribe(InternalUpdateRecord)
            .DisposeItWith(Disposable);
        client.OnRecordTag
            .Where(_=>Id == _.Item1.RecordGuid)
            .Subscribe(_=> _tags.Edit(updater =>
            {
                var item = updater.Lookup(_.Item1);
                if (item.HasValue == false)
                {
                    _tags.AddOrUpdate(new AsvSdrClientRecordTag(_.Item1, _.Item2));    
                }
            })).DisposeItWith(Disposable);
        client.OnDeleteRecordTag
            .Where(_=>Id == _.Item1.RecordGuid && _.Item2.Result == AsvSdrRequestAck.AsvSdrRequestAckOk)
            .Subscribe(_=> _tags.Edit(updater =>
            {
                var item = updater.Lookup(_.Item1);
                if (item.HasValue == false) return;
                _tags.Remove(item.Value);
            })).DisposeItWith(Disposable);
        Tags = _tags.Connect().RefCount();
        _maxTimeToWaitForResponseForList = TimeSpan.FromMilliseconds(config.MaxTimeToWaitForResponseForListMs);
        
    }
    public Guid Id { get; }
    public IRxValue<AsvSdrCustomMode> DataType => _dataType;
    public IRxValue<string> Name => _name;
    public IRxValue<ulong> Frequency => _frequency;
    public IRxValue<DateTime> Created => _created;
    public IRxValue<ushort> TagsCount => _tagsCount;
    public IRxValue<uint> DataCount => _dataCount;
    public IRxValue<uint> ByteSize => _byteSize;
    public IRxValue<TimeSpan> Duration => _duration;
    
    public IObservable<IChangeSet<AsvSdrClientRecordTag,TagId>> Tags { get; }

    private void InternalUpdateRecord((Guid,AsvSdrRecordPayload) value)
    {
        _created.OnNext(MavlinkTypesHelper.FromUnixTimeUs(value.Item2.CreatedUnixUs));
        _duration.OnNext(TimeSpan.FromSeconds(value.Item2.DurationSec));
        _tagsCount.OnNext(value.Item2.TagCount);
        _dataCount.OnNext(value.Item2.DataCount);
        _byteSize.OnNext(value.Item2.Size);
    }
    
    public async Task<bool> UploadTagList(IProgress<double> progress, CancellationToken cancel)
    {
        var lastUpdate = DateTime.Now;
        _tags.Clear();
        using var request = _client.OnRecordTag
            .Where(_=>Id == _.Item1.RecordGuid)
            .Do(_=>lastUpdate = DateTime.Now)
            .Subscribe();
        var requestAck = await _client.GetRecordTagList(Id,0,ushort.MaxValue,cancel).ConfigureAwait(false);
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckFail) 
            throw new Exception("Request fail");

        while (DateTime.Now - lastUpdate > _maxTimeToWaitForResponseForList || _tags.Count < requestAck.ItemsCount)
        {
            await Task.Delay(1000, cancel).ConfigureAwait(false);
            progress?.Report((double)requestAck.ItemsCount/_tags.Count);
        }
        return _tags.Count == requestAck.ItemsCount;
    }
    public async Task DeleteTag(TagId id, CancellationToken cancel)
    {
        if (Id == id.RecordGuid)
            throw new Exception("Tag not belong to this record");
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var requestAck = await _client.DeleteRecordTag(id, cs.Token).ConfigureAwait(false);
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckFail) 
            throw new Exception("Request fail");
    }


}