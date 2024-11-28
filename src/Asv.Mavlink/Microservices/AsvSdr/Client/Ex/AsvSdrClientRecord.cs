using System;

using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvSdr;
using ObservableCollections;
using R3;

namespace Asv.Mavlink;

public class AsvSdrClientRecord:IAsvSdrClientRecord, IDisposable,IAsyncDisposable
{
    private readonly ReactiveProperty<DateTime> _created;
    private readonly ReactiveProperty<ushort> _tagsCount;
    private readonly ReactiveProperty<uint> _dataCount;
    private readonly ReactiveProperty<uint> _byteSize;
    private readonly ReactiveProperty<TimeSpan> _duration;
    private readonly ObservableDictionary<TagId,AsvSdrClientRecordTag> _tags;
    private readonly TimeSpan _maxTimeToWaitForResponseForList;
    private readonly ReactiveProperty<AsvSdrCustomMode> _dataType;
    private readonly ReactiveProperty<ulong> _frequency;
    private readonly IAsvSdrClient _client;
    private readonly ReactiveProperty<string> _name;
    private readonly CancellationTokenSource _disposeCancel;
    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    private readonly IDisposable _sub3;

    public AsvSdrClientRecord(Guid id, AsvSdrRecordPayload payload, IAsvSdrClient client, AsvSdrClientExConfig config )
    {
        if (payload == null) throw new ArgumentNullException(nameof(payload));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        Id = id;
        _name = new ReactiveProperty<string>(MavlinkTypesHelper.GetString(payload.RecordName));
        _dataType = new ReactiveProperty<AsvSdrCustomMode>(payload.DataType);
        _frequency = new ReactiveProperty<ulong>(payload.Frequency);
        _created = new ReactiveProperty<DateTime>(MavlinkTypesHelper.FromUnixTimeUs(payload.CreatedUnixUs));
        _duration = new ReactiveProperty<TimeSpan>(TimeSpan.FromSeconds(payload.DurationSec));
        _tagsCount = new ReactiveProperty<ushort>(payload.TagCount);
        _dataCount = new ReactiveProperty<uint>(payload.DataCount);
        _byteSize = new ReactiveProperty<uint>(payload.Size);
        _tags = new ObservableDictionary<TagId, AsvSdrClientRecordTag>();
        _sub1 = client.OnRecord
            .Where(t => t.Item1 == Id)
            .Subscribe(InternalUpdateRecord);
        _sub2 = client.OnRecordTag
            .Where(t=>Id == t.Item1.RecordGuid)
            .Subscribe(t => _tags[t.Item1] = new AsvSdrClientRecordTag(t.Item1, t.Item2));
        _sub3 = client.OnDeleteRecordTag
            .Where(t=>Id == t.Item1.RecordGuid && t.Item2.Result == AsvSdrRequestAck.AsvSdrRequestAckOk)
            .Subscribe(t=>  _tags.Remove(t.Item1));
        _disposeCancel = new CancellationTokenSource();
        _maxTimeToWaitForResponseForList = TimeSpan.FromMilliseconds(config.MaxTimeToWaitForResponseForListMs);
        
    }
    private CancellationToken DisposeCancel => _disposeCancel.Token;
    public Guid Id { get; }
    public ReadOnlyReactiveProperty<AsvSdrCustomMode> DataType => _dataType;
    public ReadOnlyReactiveProperty<string> Name => _name;
    public ReadOnlyReactiveProperty<ulong> Frequency => _frequency;
    public ReadOnlyReactiveProperty<DateTime> Created => _created;
    public ReadOnlyReactiveProperty<ushort> TagsCount => _tagsCount;
    public ReadOnlyReactiveProperty<uint> DataCount => _dataCount;
    public ReadOnlyReactiveProperty<uint> ByteSize => _byteSize;
    public ReadOnlyReactiveProperty<TimeSpan> Duration => _duration;
    public IReadOnlyObservableDictionary<TagId, AsvSdrClientRecordTag> Tags => _tags;
    private void InternalUpdateRecord((Guid,AsvSdrRecordPayload) value)
    {
        _created.OnNext(MavlinkTypesHelper.FromUnixTimeUs(value.Item2.CreatedUnixUs));
        _duration.OnNext(TimeSpan.FromSeconds(value.Item2.DurationSec));
        _tagsCount.OnNext(value.Item2.TagCount);
        _dataCount.OnNext(value.Item2.DataCount);
        _byteSize.OnNext(value.Item2.Size);
    }
    public async Task<bool> DownloadTagList(IProgress<double>? progress, CancellationToken cancel)
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

    #region Dispose

    public void Dispose()
    {
        _created.Dispose();
        _tagsCount.Dispose();
        _dataCount.Dispose();
        _byteSize.Dispose();
        _duration.Dispose();
        _dataType.Dispose();
        _frequency.Dispose();
        _name.Dispose();
        _disposeCancel.Dispose();
        _sub1.Dispose();
        _sub2.Dispose();
        _sub3.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_created).ConfigureAwait(false);
        await CastAndDispose(_tagsCount).ConfigureAwait(false);
        await CastAndDispose(_dataCount).ConfigureAwait(false);
        await CastAndDispose(_byteSize).ConfigureAwait(false);
        await CastAndDispose(_duration).ConfigureAwait(false);
        await CastAndDispose(_dataType).ConfigureAwait(false);
        await CastAndDispose(_frequency).ConfigureAwait(false);
        await CastAndDispose(_name).ConfigureAwait(false);
        await CastAndDispose(_disposeCancel).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);
        await CastAndDispose(_sub3).ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    #endregion
}