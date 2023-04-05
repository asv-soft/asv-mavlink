using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using DynamicData;
using DynamicData.Kernel;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class SdrClientDevice : DisposableOnceWithCancel, ISdrClientDevice
{
    private readonly IMavlinkClient _client;
    private readonly RxValue<AsvSdrCustomMode> _customMode;
    private readonly SourceCache<AsvSdrRecord,ushort> _records;
    private const int DefaultAttemptCount = 3;

    public SdrClientDevice(IMavlinkClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _customMode = new RxValue<AsvSdrCustomMode>();
        _client.Heartbeat.RawHeartbeat.Select(_ => (AsvSdrCustomMode)_.CustomMode).Subscribe(_customMode)
            .DisposeItWith(Disposable);
        
        _records = new SourceCache<AsvSdrRecord, ushort>(x => x.Index);
        _client.Sdr.OnRecord.Subscribe(_=>_records.Edit(updater =>
            {
                var value = updater.Lookup(_.Index);
                if (value.HasValue)
                {
                    value.Value.Update(_);    
                }
                else
                {
                    updater.AddOrUpdate(new AsvSdrRecord(_,_client));
                }
            })).DisposeItWith(Disposable);
    }

    public IRxValue<AsvSdrCustomMode> CustomMode => _customMode;

   
}

public class AsvSdrRecord:DisposableOnceWithCancel
{
    public AsvSdrRecord(AsvSdrRecordPayload asvSdrRecordPayload, IMavlinkClient mavlinkClient)
    {
        Index = asvSdrRecordPayload.Index;
        Update(asvSdrRecordPayload);
    }
    public ushort Index { get; }

    internal void Update(AsvSdrRecordPayload payload)
    {
        if (payload.Index != Index) throw new ArgumentException("Index not equal");
        
    }
}

// var result = new ConcurrentDictionary<ushort, AsvSdrRecordPayload>();
// var lastUpdate = DateTime.Now;
// using var request = OnRecord
//     .Do(_=>lastUpdate = DateTime.Now)
//     .Subscribe(_=>result.AddOrUpdate(_.Index, (k)=>_, (k,v)=>_));
// var requestAck = await InternalCall<AsvSdrRecordResponseListPayload,AsvSdrRecordRequestListPacket,AsvSdrRecordResponseListPacket>(_=>
// {
//     _.Payload.TargetSystem = _identity.TargetSystemId;
//     _.Payload.TargetComponent = _identity.TargetComponentId;
// }, _=> true, resultGetter:_=>_.Payload,cancel: cancel);
//         
// if (requestAck.Result == AsvSdrRequestListAck.AsvSdrRequestAckInProgress)
//     throw new Exception("Request in progress");
// if (requestAck.Result == AsvSdrRequestListAck.AsvSdrRequestAckFail) 
//     throw new Exception("Request fail");
//
// while (DateTime.Now - lastUpdate > MaxTimeToWaitForResponseForList)
// {
//     await Task.Delay(100, cancel);
// }
// return result.Values.OrderBy(_ => _.Index).ToArray();