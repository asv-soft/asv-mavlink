using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;


namespace Asv.Mavlink;

public delegate Task<MavResult> SetModeDelegate(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate,int sendingThinningRatio, CancellationToken cancel);

public delegate Task<MavResult> StartRecordDelegate(string recordName, CancellationToken cancel);

public delegate Task<MavResult> StopRecordDelegate(CancellationToken cancel);

public delegate Task<MavResult> CurrentRecordSetTagDelegate(AsvSdrRecordTagType type, string name, byte[] value, CancellationToken cancel);

public interface IAsvSdrServerEx
{
    IAsvSdrServer Base { get; }
    IRxEditableValue<AsvSdrCustomMode> CustomMode { get; }
    SetModeDelegate SetMode { set; }
    StartRecordDelegate StartRecord { set; }
    StopRecordDelegate StopRecord { set; }
    CurrentRecordSetTagDelegate CurrentRecordSetTag { set; }
    
}