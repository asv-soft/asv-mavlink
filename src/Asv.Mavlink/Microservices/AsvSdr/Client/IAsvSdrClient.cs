using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvSdr;

using R3;

namespace Asv.Mavlink
{
   /// <summary>
   /// Provides the interface for a SDR payload client with Mavlink interface.
   /// </summary>
   public interface IAsvSdrClient:IMavlinkMicroserviceClient
   {
       /// <summary>
       /// Generates Request Index.
       /// </summary>
       ushort GenerateRequestIndex();

       /// <summary>
       /// Gets the status of the SDR out payload.
       /// </summary>
       ReadOnlyReactiveProperty<AsvSdrOutStatusPayload?> Status { get; }

       /// <summary>
       /// Observes the Signal Raw Payload.
       /// </summary>
       Observable<AsvSdrSignalRawPayload> OnSignal { get; }

       // Records
       /// <summary>
       /// Gets a list of records from specific start to end index.
       /// </summary>
       Task<AsvSdrRecordResponsePayload> GetRecordList(ushort startIndex, ushort stopIndex,
           CancellationToken cancel = default);

       /// <summary>
       /// Observes the SDR Record Payload.
       /// </summary>
       Observable<(Guid, AsvSdrRecordPayload)> OnRecord { get; }

       /// <summary>
       /// Deletes a record.
       /// </summary>
       Task<AsvSdrRecordDeleteResponsePayload> DeleteRecord(Guid recordId, CancellationToken cancel = default);

       /// <summary>
       /// Observes the deletion of a record.
       /// </summary>
       Observable<(Guid, AsvSdrRecordDeleteResponsePayload)> OnDeleteRecord { get; }

       // Tags
       /// <summary>
       /// Gets a list of record tags.
       /// </summary>
       Task<AsvSdrRecordTagResponsePayload> GetRecordTagList(Guid recordId, ushort skip, ushort count,
           CancellationToken cancel = default);

       /// <summary>
       /// Observes the Record Tag Payload.
       /// </summary>
       Observable<(TagId, AsvSdrRecordTagPayload)> OnRecordTag { get; }

       /// <summary>
       /// Deletes a Record's Tag.
       /// </summary>
       Task<AsvSdrRecordTagDeleteResponsePayload> DeleteRecordTag(TagId tag, CancellationToken cancel = default);

       /// <summary>
       /// Observes the deletion of a Record's Tag.
       /// </summary>
       Observable<(TagId, AsvSdrRecordTagDeleteResponsePayload)> OnDeleteRecordTag { get; }

       // Record data
       /// <summary>
       /// Gets a list of Record Data.
       /// </summary>
       Task<AsvSdrRecordDataResponsePayload> GetRecordDataList(Guid recordId, uint skip, uint count,
           CancellationToken cancel = default);

       /// <summary>
       /// Observes the Record's Data.
       /// </summary>
       Observable<MavlinkMessage> OnRecordData { get; }

       // Calibration
       /// <summary>
       /// Observes the Calibration Table Payload.
       /// </summary>
       Observable<AsvSdrCalibTablePayload> OnCalibrationTable { get; }

       /// <summary>
       /// Reads the Calibration Table.
       /// </summary>
       Task<AsvSdrCalibTablePayload> ReadCalibrationTable(ushort tableIndex, CancellationToken cancel = default);

       /// <summary>
       /// Reads the Calibration Table's Row.
       /// </summary>
       Task<AsvSdrCalibTableRowPayload> ReadCalibrationTableRow(ushort tableId, ushort rowIndex,
           CancellationToken cancel = default);

       /// <summary>
       /// Starts the upload process of a Calibration Table's Row.
       /// </summary>
       ValueTask SendCalibrationTableRowUploadStart(Action<AsvSdrCalibTableUploadStartPayload> argsFill,
           CancellationToken cancel = default);

       /// <summary>
       /// Observes the Calibration Table Row Upload Callback Payload.
       /// </summary>
       Observable<AsvSdrCalibTableUploadReadCallbackPayload> OnCalibrationTableRowUploadCallback { get; }

       /// <summary>
       /// Observes the Calibration Acceleration Payload.
       /// </summary>
       Observable<AsvSdrCalibAccPayload> OnCalibrationAcc { get; }

       /// <summary>
       /// Sends the upload item of a Calibration Table's Row.
       /// </summary>
       ValueTask SendCalibrationTableRowUploadItem(Action<AsvSdrCalibTableRowPayload> argsFill,
           CancellationToken cancel = default);
   }

   /// <summary>
   /// Extension methods for AsvSdrClient.
   /// </summary>
   public static class AsvSdrClientExtensions
   {

   }
}



