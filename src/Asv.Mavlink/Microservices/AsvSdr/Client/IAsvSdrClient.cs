using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink;
/// <summary>
/// Implementation of SDR payload client mavlink interface ()
/// </summary>
public interface IAsvSdrClient
{
    MavlinkClientIdentity Identity { get; }
    ushort GenerateRequestIndex();
    
    IRxValue<AsvSdrOutStatusPayload> Status { get; }
    
    IObservable<AsvSdrSignalRawPayload> OnSignal { get; }
    
    #region Records
    Task<AsvSdrRecordResponsePayload> GetRecordList(ushort startIndex, ushort stopIndex, CancellationToken cancel=default);
    IObservable<(Guid,AsvSdrRecordPayload)> OnRecord { get; }
    Task<AsvSdrRecordDeleteResponsePayload> DeleteRecord(Guid recordId, CancellationToken cancel = default);
    IObservable<(Guid,AsvSdrRecordDeleteResponsePayload)> OnDeleteRecord { get; }
    #endregion
    
    #region Tags
    Task<AsvSdrRecordTagResponsePayload> GetRecordTagList(Guid recordId, ushort skip, ushort count, CancellationToken cancel = default);
    IObservable<(TagId,AsvSdrRecordTagPayload)> OnRecordTag { get; }
    Task<AsvSdrRecordTagDeleteResponsePayload> DeleteRecordTag(TagId tag, CancellationToken cancel = default);
    IObservable<(TagId, AsvSdrRecordTagDeleteResponsePayload)> OnDeleteRecordTag { get; }
    #endregion
    
    #region Record data
    Task<AsvSdrRecordDataResponsePayload> GetRecordDataList(Guid recordId, uint skip, uint count, CancellationToken cancel = default);
    IObservable<IPacketV2<IPayload>> OnRecordData { get; }
    #endregion
    

    #region Calibration

    IObservable<AsvSdrCalibTablePayload> OnCalibrationTable { get; }
    Task<AsvSdrCalibTablePayload> ReadCalibrationTable(ushort tableIndex, CancellationToken cancel=default);
    Task<AsvSdrCalibTableRowPayload> ReadCalibrationTableRow(ushort tableId, ushort rowIndex, CancellationToken cancel = default);
    Task SendCalibrationTableRowUploadStart(Action<AsvSdrCalibTableUploadStartPayload> argsFill, CancellationToken cancel = default);
    IObservable<AsvSdrCalibTableUploadReadCallbackPayload> OnCalibrationTableRowUploadCallback { get; }
    IObservable<AsvSdrCalibAccPayload> OnCalibrationAcc { get; }
    Task SendCalibrationTableRowUploadItem(Action<AsvSdrCalibTableRowPayload> argsFill, CancellationToken cancel = default);

    #endregion
    
}

public static class AsvSdrClientExtensions
{
    
}



