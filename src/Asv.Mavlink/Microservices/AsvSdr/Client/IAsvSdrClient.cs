﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;

namespace Asv.Mavlink
{
   /// <summary>
   /// Provides the interface for a SDR payload client with Mavlink interface.
   /// </summary>
   public interface IAsvSdrClient
   {
       /// <summary>
       /// An Identity of the Mavlink client.
       /// </summary>
       MavlinkClientIdentity Identity { get; }

       /// <summary>
       /// Generates Request Index.
       /// </summary>
       ushort GenerateRequestIndex();

       /// <summary>
       /// Gets the status of the SDR out payload.
       /// </summary>
       IRxValue<AsvSdrOutStatusPayload> Status { get; }

       /// <summary>
       /// Observes the Signal Raw Payload.
       /// </summary>
       IObservable<AsvSdrSignalRawPayload> OnSignal { get; }

       // Records
       /// <summary>
       /// Gets a list of records from specific start to end index.
       /// </summary>
       Task<AsvSdrRecordResponsePayload> GetRecordList(ushort startIndex, ushort stopIndex,
           CancellationToken cancel = default);

       /// <summary>
       /// Observes the SDR Record Payload.
       /// </summary>
       IObservable<(Guid, AsvSdrRecordPayload)> OnRecord { get; }

       /// <summary>
       /// Deletes a record.
       /// </summary>
       Task<AsvSdrRecordDeleteResponsePayload> DeleteRecord(Guid recordId, CancellationToken cancel = default);

       /// <summary>
       /// Observes the deletion of a record.
       /// </summary>
       IObservable<(Guid, AsvSdrRecordDeleteResponsePayload)> OnDeleteRecord { get; }

       // Tags
       /// <summary>
       /// Gets a list of record tags.
       /// </summary>
       Task<AsvSdrRecordTagResponsePayload> GetRecordTagList(Guid recordId, ushort skip, ushort count,
           CancellationToken cancel = default);

       /// <summary>
       /// Observes the Record Tag Payload.
       /// </summary>
       IObservable<(TagId, AsvSdrRecordTagPayload)> OnRecordTag { get; }

       /// <summary>
       /// Deletes a Record's Tag.
       /// </summary>
       Task<AsvSdrRecordTagDeleteResponsePayload> DeleteRecordTag(TagId tag, CancellationToken cancel = default);

       /// <summary>
       /// Observes the deletion of a Record's Tag.
       /// </summary>
       IObservable<(TagId, AsvSdrRecordTagDeleteResponsePayload)> OnDeleteRecordTag { get; }

       // Record data
       /// <summary>
       /// Gets a list of Record Data.
       /// </summary>
       Task<AsvSdrRecordDataResponsePayload> GetRecordDataList(Guid recordId, uint skip, uint count,
           CancellationToken cancel = default);

       /// <summary>
       /// Observes the Record's Data.
       /// </summary>
       IObservable<IPacketV2<IPayload>> OnRecordData { get; }

       // Calibration
       /// <summary>
       /// Observes the Calibration Table Payload.
       /// </summary>
       IObservable<AsvSdrCalibTablePayload> OnCalibrationTable { get; }

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
       Task SendCalibrationTableRowUploadStart(Action<AsvSdrCalibTableUploadStartPayload> argsFill,
           CancellationToken cancel = default);

       /// <summary>
       /// Observes the Calibration Table Row Upload Callback Payload.
       /// </summary>
       IObservable<AsvSdrCalibTableUploadReadCallbackPayload> OnCalibrationTableRowUploadCallback { get; }

       /// <summary>
       /// Observes the Calibration Acceleration Payload.
       /// </summary>
       IObservable<AsvSdrCalibAccPayload> OnCalibrationAcc { get; }

       /// <summary>
       /// Sends the upload item of a Calibration Table's Row.
       /// </summary>
       Task SendCalibrationTableRowUploadItem(Action<AsvSdrCalibTableRowPayload> argsFill,
           CancellationToken cancel = default);
   }

   /// <summary>
   /// Extension methods for AsvSdrClient.
   /// </summary>
   public static class AsvSdrClientExtensions
   {

   }
}



