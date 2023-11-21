#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;


namespace Asv.Mavlink;

public delegate Task<MavResult> SetModeDelegate(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate,uint sendingThinningRatio, float referencePower, CancellationToken cancel);

public delegate Task<MavResult> StartRecordDelegate(string recordName, CancellationToken cancel);

public delegate Task<MavResult> StopRecordDelegate(CancellationToken cancel);

public delegate Task<MavResult> CurrentRecordSetTagDelegate(AsvSdrRecordTagType type, string name, byte[] value, CancellationToken cancel);

public delegate Task<MavResult> SystemControlActionDelegate(AsvSdrSystemControlAction action, CancellationToken cancel);

public delegate Task<MavResult> StartMissionDelegate(ushort missionIndex, CancellationToken cancel);
public delegate Task<MavResult> StopMissionDelegate(CancellationToken cancel);

public delegate Task<MavResult> StartCalibrationDelegate(CancellationToken cancel);
public delegate Task<MavResult> StopCalibrationDelegate(CancellationToken cancel);
public delegate bool TryReadCalibrationTableInfoDelegate(ushort tableIndex, out string? name, out ushort? size, out CalibrationTableMetadata? metadata);
public delegate bool TryReadCalibrationTableRowDelegate(ushort tableIndex, ushort rowIndex, out CalibrationTableRow? row);
public delegate void WriteCalibrationDelegate(ushort tableIndex, CalibrationTableRow[] items);


public interface IAsvSdrServerEx
{
    IAsvSdrServer Base { get; }
    IRxEditableValue<AsvSdrCustomMode> CustomMode { get; }
    SetModeDelegate SetMode { set; }
    StartRecordDelegate StartRecord { set; }
    StopRecordDelegate StopRecord { set; }
    CurrentRecordSetTagDelegate CurrentRecordSetTag { set; }
    SystemControlActionDelegate SystemControlAction { set; }
    StartMissionDelegate StartMission { set; }
    StopMissionDelegate StopMission { set; }
    
    StartCalibrationDelegate StartCalibration { set; }
    StopCalibrationDelegate StopCalibration { set; }
    TryReadCalibrationTableInfoDelegate? TryReadCalibrationTableInfo { set; }
    TryReadCalibrationTableRowDelegate? TryReadCalibrationTableRow { get; set; }
    WriteCalibrationDelegate? WriteCalibrationTable { get; set; }
    Task<bool> SendSignal(ulong unixTime, string name, ReadOnlyMemory<double> signal, AsvSdrSignalFormat format, CancellationToken cancel = default);
}