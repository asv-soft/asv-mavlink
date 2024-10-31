using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using R3;

namespace Asv.Mavlink;

public class AsvSdrClientRecord:DisposableOnceWithCancel, IAsvSdrClientRecord
{
    private readonly ReactiveProperty<DateTime> _created;
    private readonly ReactiveProperty<ushort> _tagsCount;
    private readonly ReactiveProperty<uint> _dataCount;
    private readonly ReactiveProperty<uint> _byteSize;
    private readonly ReactiveProperty<TimeSpan> _duration;
    private readonly SourceCache<AsvSdrClientRecordTag,TagId> _tags;
    private readonly TimeSpan _maxTimeToWaitForResponseForList;
    private readonly ReactiveProperty<AsvSdrCustomMode> _dataType;
    private readonly ReactiveProperty<ulong> _frequency;
    private readonly IAsvSdrClient _client;
    private readonly ReactiveProperty<string> _name;

    public AsvSdrClientRecord(Guid id, AsvSdrRecordPayload payload, IAsvSdrClient client, AsvSdrClientExConfig config )
    {
        if (payload == null) throw new ArgumentNullException(nameof(payload));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        Id = id;
        _name = new ReactiveProperty<string>(MavlinkTypesHelper.GetString(payload.RecordName))
            .DisposeItWith(Disposable);
        _dataType = new ReactiveProperty<AsvSdrCustomMode>(payload.DataType)
            .DisposeItWith(Disposable);
        _frequency = new ReactiveProperty<ulong>(payload.Frequency)
            .DisposeItWith(Disposable);
        _created = new ReactiveProperty<DateTime>(MavlinkTypesHelper.FromUnixTimeUs(payload.CreatedUnixUs))
            .DisposeItWith(Disposable);
        _duration = new ReactiveProperty<TimeSpan>(TimeSpan.FromSeconds(payload.DurationSec))
            .DisposeItWith(Disposable);
        _tagsCount = new ReactiveProperty<ushort>(payload.TagCount)
            .DisposeItWith(Disposable);
        _dataCount = new ReactiveProperty<uint>(payload.DataCount)
            .DisposeItWith(Disposable);
        _byteSize = new ReactiveProperty<uint>(payload.Size)
            .DisposeItWith(Disposable);
        _tags = new SourceCache<AsvSdrClientRecordTag, TagId>(x => x.Id)
            .DisposeItWith(Disposable);
        client.OnRecord
            .Where(t => t.Item1 == Id)
            .Subscribe(InternalUpdateRecord)
            .DisposeItWith(Disposable);
        client.OnRecordTag
            .Where(t=>Id == t.Item1.RecordGuid)
            .Subscribe(t=> _tags.Edit(updater =>
            {
                var item = updater.Lookup(t.Item1);
                if (item.HasValue == false)
                {
                    _tags.AddOrUpdate(new AsvSdrClientRecordTag(t.Item1, t.Item2));    
                }
            })).DisposeItWith(Disposable);
        client.OnDeleteRecordTag
            .Where(t=>Id == t.Item1.RecordGuid && t.Item2.Result == AsvSdrRequestAck.AsvSdrRequestAckOk)
            .Subscribe(t=> _tags.Edit(updater =>
            {
                var item = updater.Lookup(t.Item1);
                if (item.HasValue == false) return;
                _tags.Remove(item.Value);
            })).DisposeItWith(Disposable);
        Tags = _tags.Connect().RefCount();
        _maxTimeToWaitForResponseForList = TimeSpan.FromMilliseconds(config.MaxTimeToWaitForResponseForListMs);
        
    }
    public Guid Id { get; }
    public ReadOnlyReactiveProperty<AsvSdrCustomMode> DataType => _dataType;
    public ReadOnlyReactiveProperty<string> Name => _name;
    public ReadOnlyReactiveProperty<ulong> Frequency => _frequency;
    public ReadOnlyReactiveProperty<DateTime> Created => _created;
    public ReadOnlyReactiveProperty<ushort> TagsCount => _tagsCount;
    public ReadOnlyReactiveProperty<uint> DataCount => _dataCount;
    public ReadOnlyReactiveProperty<uint> ByteSize => _byteSize;
    public ReadOnlyReactiveProperty<TimeSpan> Duration => _duration;
    
    public IObservable<IChangeSet<AsvSdrClientRecordTag,TagId>> Tags { get; }

    private void InternalUpdateRecord((Guid,AsvSdrRecordPayload) value)
    {
        _created.OnNext(MavlinkTypesHelper.FromUnixTimeUs(value.Item2.CreatedUnixUs));
        _duration.OnNext(TimeSpan.FromSeconds(value.Item2.DurationSec));
        _tagsCount.OnNext(value.Item2.TagCount);
        _dataCount.OnNext(value.Item2.DataCount);
        _byteSize.OnNext(value.Item2.Size);
    }
    
    public async Task<bool> DownloadTagList(IProgress<double> progress, CancellationToken cancel)
    {
        var lastUpdate = DateTime.Now;
        _tags.Clear();
        using var request = _client.OnRecordTag
            .Where(t=>Id == t.Item1.RecordGuid)
            .Do(_=>lastUpdate = DateTime.Now)
            .Subscribe();
        var requestAck = await _client.GetRecordTagList(Id,0,ushort.MaxValue,cancel).ConfigureAwait(false);
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckFail) 
            throw new Exception("Request fail");

        while (DateTime.Now - lastUpdate < _maxTimeToWaitForResponseForList && _tags.Count < requestAck.ItemsCount)
        {
            await Task.Delay(1000, cancel).ConfigureAwait(false);
            progress?.Report((double)requestAck.ItemsCount/_tags.Count);
        }
        return _tags.Count == requestAck.ItemsCount;
    }
    public async Task DeleteTag(TagId id, CancellationToken cancel)
    {
        if (Id != id.RecordGuid)
            throw new Exception("Tag not belong to this record");
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var requestAck = await _client.DeleteRecordTag(id, cs.Token).ConfigureAwait(false);
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckFail) 
            throw new Exception("Request fail");
    }


}