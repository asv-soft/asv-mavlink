using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using DynamicData;

namespace Asv.Mavlink.Sdr;

public class SdrClientDeviceRecord:DisposableOnceWithCancel, ISdrClientDeviceRecord
{
    
    private readonly IMavlinkClient _client;
    private readonly RxValue<AsvSdrRecordStateFlag> _state;
    private readonly RxValue<DateTime> _created;
    private readonly RxValue<ushort> _tagsCount;
    private readonly RxValue<uint> _dataCount;
    private readonly RxValue<uint> _byteSize;
    private readonly RxValue<TimeSpan> _duration;
    private readonly SourceCache<SdrClientDeviceRecordTag,ushort> _tags;
    private readonly TimeSpan _maxTimeToWaitForResponseForList;
    private readonly RxValue<AsvSdrCustomMode> _recordMode;
    private readonly RxValue<ulong> _frequency;

    public SdrClientDeviceRecord(AsvSdrRecordPayload payload, IMavlinkClient client,
        SdrClientDeviceConfig config)
    {
        if (payload == null) throw new ArgumentNullException(nameof(payload));
        if (config == null) throw new ArgumentNullException(nameof(config));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        Index = payload.Index;
        Name = MavlinkTypesHelper.GetString(payload.Name);
        DataMessageId = payload.DataMessageId;

        _recordMode = new RxValue<AsvSdrCustomMode>(payload.RecordMode).DisposeItWith(Disposable);
        _frequency = new RxValue<ulong>(payload.Frequency).DisposeItWith(Disposable);
        _state = new RxValue<AsvSdrRecordStateFlag>(payload.State).DisposeItWith(Disposable);
        _created = new RxValue<DateTime>(MavlinkTypesHelper.FromUnixTimeUs(payload.CreatedUnixUs)).DisposeItWith(Disposable);
        _duration = new RxValue<TimeSpan>(TimeSpan.FromSeconds(payload.DurationSec)).DisposeItWith(Disposable);
        _tagsCount = new RxValue<ushort>(payload.TagCount).DisposeItWith(Disposable);
        _dataCount = new RxValue<uint>(payload.DataCount).DisposeItWith(Disposable);
        _byteSize = new RxValue<uint>(payload.Size).DisposeItWith(Disposable);
        _tags = new SourceCache<SdrClientDeviceRecordTag, ushort>(x => x.TagIndex).DisposeItWith(Disposable);
        
        client.Sdr.OnRecordTag.Subscribe(_=>_tags.AddOrUpdate(new SdrClientDeviceRecordTag(_))).DisposeItWith(Disposable);
        Tags = _tags.Connect().DisposeMany().RefCount();
        _maxTimeToWaitForResponseForList = TimeSpan.FromMilliseconds(config.MaxTimeToWaitForResponseForListMs);
        
    }
    public ushort DataMessageId { get; }
    public ushort Index { get; }
    public string Name { get; }
    public IRxValue<AsvSdrCustomMode> RecordMode => _recordMode;
    public IRxValue<ulong> Frequency => _frequency;
    public IRxValue<AsvSdrRecordStateFlag> State => _state;
    public IRxValue<DateTime> Created => _created;
    public IRxValue<ushort> TagsCount => _tagsCount;
    public IRxValue<uint> DataCount => _dataCount;
    public IRxValue<uint> ByteSize => _byteSize;
    public IRxValue<TimeSpan> Duration => _duration;
    
    public IObservable<IChangeSet<SdrClientDeviceRecordTag,ushort>> Tags { get; }
    internal void UpdateRecord(AsvSdrRecordPayload payload)
    {
        if (payload == null) throw new ArgumentNullException(nameof(payload));
        if (payload.Index != Index) throw new ArgumentException("Record index not equal");
        if (MavlinkTypesHelper.GetString(payload.Name) != Name) throw new ArgumentException($"Record {nameof(Name)} not equal");
        if (payload.DataMessageId != DataMessageId) throw new ArgumentException($"Record {nameof(DataMessageId)} not equal");
        
        _state.OnNext(payload.State);
        _created.OnNext(MavlinkTypesHelper.FromUnixTimeUs(payload.CreatedUnixUs));
        _duration.OnNext(TimeSpan.FromSeconds(payload.DurationSec));
        _tagsCount.OnNext(payload.TagCount);
        _dataCount.OnNext(payload.DataCount);
        _byteSize.OnNext(payload.Size);
    }
    
    public async Task<bool> UploadTagList(IProgress<double>? progress, CancellationToken cancel)
    {
        var lastUpdate = DateTime.Now;
        _tags.Clear();
        using var request = _client.Sdr.OnRecordTag.Where(_=>_.RecordIndex == Index)
            .Do(_=>lastUpdate = DateTime.Now)
            .Subscribe();
        var requestAck = await _client.Sdr.GetRecordTagList(Index,cancel);
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckInProgress)
            throw new Exception("Request already in progress");
        if (requestAck.Result == AsvSdrRequestAck.AsvSdrRequestAckFail) 
            throw new Exception("Request fail");

        while (DateTime.Now - lastUpdate > _maxTimeToWaitForResponseForList || _tags.Count < requestAck.ItemsCount)
        {
            await Task.Delay(1000, cancel);
            progress?.Report((double)requestAck.ItemsCount/_tags.Count);
        }
        return _tags.Count == requestAck.ItemsCount;
    }

    
}