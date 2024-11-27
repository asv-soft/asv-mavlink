using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvSdr;

using R3;

namespace Asv.Mavlink
{
    /// <summary>
    /// Interface for the ASV SDR server.
    /// </summary>
    public interface IAsvSdrServer:IMavlinkMicroserviceServer
    {
        /// Summary:
        /// Starts the execution of some process.
        /// Remarks:
        /// This method initiates the execution of a process. It does not accept any arguments
        /// and does not return any value. The process being executed may perform various tasks
        /// based on its implementation.
        /// Example:
        /// // Create an instance of the class
        /// var myProcess = new MyProcess();
        /// // Start the execution of the process
        /// myProcess.Start();
        /// /
        void Start();

        /// <summary>
        /// Sets the specified change callback function to be executed when the status payload is changed.
        /// </summary>
        /// <param name="changeCallback">The callback function to be executed when the status payload is changed.</param>
        /// <remarks>
        /// The change callback function will be passed an instance of <see cref="AsvSdrOutStatusPayload"/> which represents the updated status payload.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the Set method:
        /// <code>
        /// Set(payload =>
        /// {
        /// // Handle status payload change
        /// Console.WriteLine($"Status payload changed: {payload}");
        /// });
        /// </code>
        /// </example>
        void Set(Action<AsvSdrOutStatusPayload> changeCallback);

        /// <summary>
        /// Provides an observable sequence of AsvSdrRecordRequestPayload objects for recording requests.
        /// </summary>
        /// <value>
        /// An IObservable<AsvSdrRecordRequestPayload> representing the OnRecordRequest sequence.
        /// </value>
        Observable<AsvSdrRecordRequestPayload> OnRecordRequest { get; }

        /// <summary>
        /// Sends a record response.
        /// </summary>
        /// <param name="setValueCallback">The callback method to set the value of the record response payload.</param>
        /// <param name="cancel">Cancellation token (optional).</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <remarks>
        /// The setValueCallback parameter is a callback method that takes an instance of the AsvSdrRecordResponsePayload class as its parameter.
        /// This method allows you to set the value of the record response payload.
        /// The cancel parameter is an optional cancellation token. If provided, it allows you to cancel the asynchronous operation.
        /// The method returns a task that can be awaited to track the progress of the asynchronous operation. Once the operation is complete,
        /// the task will be completed.
        /// </remarks>
        ValueTask SendRecordResponse(Action<AsvSdrRecordResponsePayload> setValueCallback,
            CancellationToken cancel = default);

        /// <summary>
        /// Sends a record using the specified payload and provides a callback to set the value.
        /// </summary>
        /// <param name="setValueCallback">The callback action used to set the value of the record.</param>
        /// <param name="cancel">The cancellation token used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        ValueTask SendRecord(Action<AsvSdrRecordPayload> setValueCallback, CancellationToken cancel = default);

        /// <summary>
        /// Event that is triggered when a delete request is received for a record.
        /// </summary>
        /// <remarks>
        /// This event provides an <see cref="IObservable{T}"/> that emits instances of <see cref="AsvSdrRecordDeleteRequestPayload"/>.
        /// Subscribing to this observable allows the application to receive and handle delete requests for records.
        /// </remarks>
        Observable<AsvSdrRecordDeleteRequestPayload> OnRecordDeleteRequest { get; }

        /// <summary>
        /// Sends a record delete response via a callback method and optional cancellation token.
        /// </summary>
        /// <param name="setValueCallback">A callback method to set the value of the delete response payload.</param>
        /// <param name="cancel">Optional. A cancellation token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        ValueTask SendRecordDeleteResponse(Action<AsvSdrRecordDeleteResponsePayload> setValueCallback,
            CancellationToken cancel = default);

        /// <summary>
        /// Gets the observable sequence for handling the record tagging request.
        /// </summary>
        /// <value>
        /// The observable sequence of type <see cref="AsvSdrRecordTagRequestPayload"/> representing the record tag request payload.
        /// </value>
        /// <remarks>
        /// This property provides an observable sequence that emits a <see cref="AsvSdrRecordTagRequestPayload"/> payload when a record tagging request is received.
        /// The observer can subscribe to this sequence to handle the request.
        /// </remarks>
        Observable<AsvSdrRecordTagRequestPayload> OnRecordTagRequest { get; }

        /// <summary>
        /// Sends a record tag response payload to the specified value callback.
        /// </summary>
        /// <param name="setValueCallback">
        ///     The action that will be called with the <see cref="AsvSdrRecordTagResponsePayload"/> as its parameter.
        /// </param>
        /// <param name="cancel">
        ///     [Optional] A cancellation token that can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A Task representing the asynchronous operation. The task will complete when the response payload is sent.
        /// </returns>
        ValueTask SendRecordTagResponse(Action<AsvSdrRecordTagResponsePayload> setValueCallback,
            CancellationToken cancel = default);

        /// <summary>
        /// SendRecordTag sends a record tag payload to the server.
        /// </summary>
        /// <param name="setValueCallback">A callback action that sets the value of the record tag.</param>
        /// <param name="cancel">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is used to send a record tag payload to the server. You need to provide a callback
        /// action <paramref name="setValueCallback"/> that sets the value of the record tag. Optionally,
        /// you can provide a cancellation token <paramref name="cancel"/> to cancel the operation.
        /// </remarks>
        ValueTask SendRecordTag(Action<AsvSdrRecordTagPayload> setValueCallback, CancellationToken cancel = default);

        /// <summary>
        /// Gets an IObservable of AsvSdrRecordTagDeleteRequestPayload representing the OnRecordTagDeleteRequest property.
        /// </summary>
        /// <remarks>
        /// This property provides a way to subscribe to delete requests for a record tag.
        /// The delete request payload contains information about the record tag being deleted.
        /// </remarks>
        /// <returns>
        /// An IObservable of AsvSdrRecordTagDeleteRequestPayload representing the OnRecordTagDeleteRequest property.
        /// </returns>
        Observable<AsvSdrRecordTagDeleteRequestPayload> OnRecordTagDeleteRequest { get; }

        /// <summary>
        /// Sends a record tag delete request to the server and asynchronously waits for the response. </summary>
        /// <param name="setValueCallback">A callback function to handle the response payload of the record tag delete.</param>
        /// <param name="cancel">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation. The task completes when the response is received.</returns>
        /// /
        ValueTask SendRecordTagDeleteResponse(Action<AsvSdrRecordTagDeleteResponsePayload> setValueCallback,
            CancellationToken cancel = default);

        /// Summary:
        /// Gets an observable sequence of AsvSdrRecordDataRequestPayload objects representing
        /// record data requests.
        /// Remarks:
        /// The OnRecordDataRequest property provides a way to subscribe to receive record data
        /// requests. Each OnRecordDataRequest event will contain an instance of the
        /// AsvSdrRecordDataRequestPayload class that encapsulates the details of the request.
        /// By subscribing to this property, you can react to record data requests and handle them
        /// appropriately in your code.
        /// Returns:
        /// An IObservable<AsvSdrRecordDataRequestPayload> representing the observable sequence
        /// of record data requests.
        /// /
        Observable<AsvSdrRecordDataRequestPayload> OnRecordDataRequest { get; }

        /// <summary>
        /// Sends a record data response and sets the value callback for received payload.
        /// This method is asynchronous.
        /// </summary>
        /// <param name="setValueCallback">The action to be performed when the payload is received.</param>
        /// <param name="cancel">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        ValueTask SendRecordDataResponse(Action<AsvSdrRecordDataResponsePayload> setValueCallback,
            CancellationToken cancel = default);

        /// <summary>
        /// Sends record data with the specified mode and callback function.
        /// </summary>
        /// <param name="mode">The AsvSdrCustomMode representing the mode in which the record data should be sent.</param>
        /// <param name="setValueCallback">The callback function that will be executed for each payload received during the data sending process.</param>
        /// <param name="cancel">The CancellationToken used to cancel the data sending process (optional).</param>
        /// <returns>A Task representing the asynchronous sending of the record data.</returns>
        ValueTask SendRecordData(AsvSdrCustomMode mode, Action<IPayload> setValueCallback, CancellationToken cancel = default);

        /// <summary>
        /// Creates a record data packet based on the specified mode.
        /// </summary>
        /// <param name="mode">The custom mode for creating the record data.</param>
        /// <returns>An instance of the IPacketV2 interface containing the record data.</returns>
        /// <seealso cref="IPacketV2{IPayload}"/>
        MavlinkMessage? CreateRecordData(AsvSdrCustomMode mode);

        /// <summary>
        /// Sends a signal.
        /// </summary>
        /// <param name="setValueCallback">The callback action for setting the raw packet signal.</param>
        /// <param name="cancel">The cancellation token (default value is CancellationToken.None)</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// The SendSignal method sends a signal by invoking the specified callback action
        /// with a raw packet signal. It can be used to send signals to a SDR device.
        /// The method runs asynchronously and it can be cancelled using the cancellation token.
        /// </remarks>
        ValueTask SendSignal(Action<AsvSdrSignalRawPacket> setValueCallback, CancellationToken cancel = default);
        
        #region Calibration

        /// <summary>
        /// Sends a calibration acceleration request with the specified request ID and result code.
        /// </summary>
        /// <param name="reqId">The ID of the calibration acceleration request.</param>
        /// <param name="resultCode">The result code of the request.</param>
        /// <param name="cancel">The cancellation token to cancel the request (optional).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// The SendCalibrationAcc method sends a calibration acceleration request to the server
        /// with the specified request ID and result code. If a cancellation token is provided,
        /// the operation can be cancelled by calling the Cancel method on the token.
        /// </remarks>
        ValueTask SendCalibrationAcc(ushort reqId, AsvSdrRequestAck resultCode, CancellationToken cancel = default);

        /// <summary>
        /// Gets an <see cref="IObservable{T}"/> representing the event triggered when a calibration table read request is received.
        /// </summary>
        /// <remarks>
        /// The event is raised when a request is made to read the calibration table. Subscribers can listen to this event to be notified
        /// when a calibration table read request is received. The event handler should be implemented to handle the request and provide the
        /// necessary response.
        /// </remarks>
        /// <value>
        /// An <see cref="IObservable{T}"/> of type <see cref="AsvSdrCalibTableReadPayload"/> representing the event triggered when a
        /// calibration table read request is received.
        /// </value>
        Observable<AsvSdrCalibTableReadPayload> OnCalibrationTableReadRequest { get; }

        /// <summary>
        /// Sends a calibration table read response.
        /// </summary>
        /// <param name="setValueCallback">The callback function to set the value of the calibration table payload.</param>
        /// <param name="cancel">Optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        ValueTask SendCalibrationTableReadResponse(Action<AsvSdrCalibTablePayload> setValueCallback,
            CancellationToken cancel = default);

        /// <summary>
        /// Event raised when a request is made to read the calibration table row.
        /// </summary>
        /// <value>
        /// An observable sequence of <see cref="AsvSdrCalibTableRowReadPayload"/> objects.
        /// </value>
        Observable<AsvSdrCalibTableRowReadPayload> OnCalibrationTableRowReadRequest { get; }

        /// <summary>
        /// Sends the calibration table row read response.
        /// </summary>
        /// <param name="setValueCallback">The callback function to set the value of the calibration table row payload.</param>
        /// <param name="cancel">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is used to send the calibration table row read response.
        /// The <paramref name="setValueCallback"/> parameter is used to set the value of the calibration table row payload.
        /// The <paramref name="cancel"/> parameter is optional and can be used to cancel the operation.
        /// </remarks>
        ValueTask SendCalibrationTableRowReadResponse(Action<AsvSdrCalibTableRowPayload> setValueCallback, CancellationToken cancel = default);

        /// <summary>
        /// Gets an observable sequence that signals the start of a calibration table upload.
        /// </summary>
        /// <remarks>
        /// This property provides a stream of <see cref="AsvSdrCalibTableUploadStartPacket"/>
        /// representing the start of a calibration table upload process.
        /// </remarks>
        /// <returns>
        /// An <see cref="IObservable{T}"/> of type <see cref="AsvSdrCalibTableUploadStartPacket"/>
        /// that emits the start packets when a calibration table upload is initiated.
        /// </returns>
        Observable<AsvSdrCalibTableUploadStartPacket> OnCalibrationTableUploadStart { get; }

        /// <summary>
        /// A method that sends a request to read a calibration table row from a target system. </summary> <param name="targetSysId">The target system ID.</param> <param name="targetCompId">The target component ID.</param> <param name="reqId">The request ID.</param> <param name="tableIndex">The index of the calibration table.</param> <param name="rowIndex">The index of the row within the calibration table.</param> <param name="cancel">A cancellation token to cancel the operation.</param> <returns>A task representing the asynchronous operation. The task result contains the calibration table row.</returns>
        /// /
        Task<CalibrationTableRow> CallCalibrationTableUploadReadCallback(byte targetSysId, byte targetCompId, ushort reqId, ushort tableIndex, ushort rowIndex , CancellationToken cancel = default);
        
        #endregion

        /// <summary>
        /// Sends a record response with a failure result code.
        /// </summary>
        /// <param name="request">The record request payload.</param>
        /// <param name="resultCode">The result code indicating the failure.</param>
        /// <returns>The task representing the operation.</returns>
        public ValueTask SendRecordResponseFail( AsvSdrRecordRequestPayload request,
            AsvSdrRequestAck resultCode, CancellationToken cancel = default)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return SendRecordResponse(x =>
            {
                x.ItemsCount = 0;
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, cancel);
        }

        /// <summary>
        /// Sends a successful record response. </summary>
        /// <param name="request">The record request payload.</param>
        /// <param name="recordsCount">The number of records.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// /
        public ValueTask SendRecordResponseSuccess( AsvSdrRecordRequestPayload request,
            ushort recordsCount, CancellationToken cancel = default)
        {
            return SendRecordResponse(x =>
            {
                x.ItemsCount = recordsCount;
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, cancel);
        }

        /// <summary>
        /// Sends a record tag response with a fail result code.
        /// </summary>
        /// <param name="request">The payload of the record tag request.</param>
        /// <param name="resultCode">The result code of the record tag request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public ValueTask SendRecordTagResponseFail( AsvSdrRecordTagRequestPayload request,
            AsvSdrRequestAck resultCode, CancellationToken cancel = default)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return SendRecordTagResponse(x =>
            {
                x.ItemsCount = 0;
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, cancel);
        }

        /// <summary>
        /// Sends a success response for a record tag request.
        /// </summary>
        /// <param name="request">The payload of the record tag request</param>
        /// <param name="recordsCount">The count of records in the request</param>
        /// <returns>Returns a task that represents the asynchronous operation</returns>
        public ValueTask SendRecordTagResponseSuccess( AsvSdrRecordTagRequestPayload request,
            ushort recordsCount, CancellationToken cancel = default)
        {
            return SendRecordTagResponse(x =>
            {
                x.ItemsCount = recordsCount;
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, cancel);
        }

        /// <summary>
        /// Sends a record delete response with a failure result code.
        /// </summary>
        /// <param name="request">The record delete request payload.</param>
        /// <param name="resultCode">The result code to be included in the response.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public ValueTask SendRecordDeleteResponseFail( AsvSdrRecordDeleteRequestPayload request,
            AsvSdrRequestAck resultCode, CancellationToken cancel = default)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return SendRecordDeleteResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, cancel);
        }

        /// <summary>
        /// Sends a success response for record deletion.
        /// </summary>
        /// <param name="request">The request payload containing the record GUID and request ID.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public ValueTask SendRecordDeleteResponseSuccess( AsvSdrRecordDeleteRequestPayload request, CancellationToken cancel = default)
        {
            return SendRecordDeleteResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, cancel);
        }

        /// <summary>
        /// Sends a failure response for a record tag deletion request.
        /// </summary>
        /// <param name="request">The deletion request payload.</param>
        /// <param name="resultCode">The result code of the deletion request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown if the result code is AsvSdrRequestAckOk.</exception>
        public ValueTask SendRecordTagDeleteResponseFail( AsvSdrRecordTagDeleteRequestPayload request,
            AsvSdrRequestAck resultCode, CancellationToken cancel = default)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return SendRecordTagDeleteResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                request.TagGuid.CopyTo(x.TagGuid, 0);
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, cancel);
        }

        /// <summary>
        /// Sends a success response for a record tag delete request.
        /// </summary>
        /// <param name="request">The payload of the delete request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public ValueTask SendRecordTagDeleteResponseSuccess( AsvSdrRecordTagDeleteRequestPayload request, CancellationToken cancel = default)
        {
           
            return SendRecordTagDeleteResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                request.TagGuid.CopyTo(x.TagGuid, 0);
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, cancel);
        }

        /// <summary>
        /// Sends a fail response for record data.
        /// </summary>
        /// <param name="request">The record data request payload.</param>
        /// <param name="resultCode">The result code for the response.</param>
        /// <returns>A task representing the asynchronous operation of sending the response.</returns>
        public ValueTask SendRecordDataResponseFail( AsvSdrRecordDataRequestPayload request,
            AsvSdrRequestAck resultCode, CancellationToken cancel = default)
        {
            if (resultCode == AsvSdrRequestAck.AsvSdrRequestAckOk)
                throw new ArgumentException("Result code must be not success");
            return SendRecordDataResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                x.ItemsCount = 0;
                x.RequestId = request.RequestId;
                x.Result = resultCode;
            }, cancel);
        }

        /// <param name="request">The request payload containing the record data.</param>
        /// <param name="count">The count of items in the record data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public ValueTask SendRecordDataResponseSuccess( AsvSdrRecordDataRequestPayload request,
            uint count, CancellationToken cancel = default)
        {
            return SendRecordDataResponse(x =>
            {
                request.RecordGuid.CopyTo(x.RecordGuid, 0);
                x.ItemsCount = count;
                x.RequestId = request.RequestId;
                x.Result = AsvSdrRequestAck.AsvSdrRequestAckOk;
            }, cancel);
        }
    }

}