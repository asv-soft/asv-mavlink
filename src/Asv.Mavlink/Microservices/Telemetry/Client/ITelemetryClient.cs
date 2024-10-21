using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using R3;
using ZLogger;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a telemetry client that is responsible for retrieving and managing telemetry data.
    /// </summary>
    public interface ITelemetryClient: IMavlinkMicroserviceClient
    {
        /// <summary>
        /// Represents the radio property.
        /// </summary>
        /// <value>
        /// An implementation of the IRxValue interface that provides access to the RadioStatusPayload object.
        /// </value>
        IRxValue<RadioStatusPayload> Radio { get; }

        /// <summary>
        /// Represents the system status property.
        /// </summary>
        /// <remarks>
        /// The SystemStatus property is an IRxValue that provides the current system status information.
        /// </remarks>
        /// <seealso cref="IRxValue{T}"/>
        /// <seealso cref="SysStatusPayload"/>
        IRxValue<SysStatusPayload> SystemStatus { get; }

        /// <summary>
        /// Represents the extended state of the system.
        /// </summary>
        /// <returns>
        /// An observable value of type <see cref="ExtendedSysStatePayload"/> that represents the extended state of the system.
        /// </returns>
        IRxValue<ExtendedSysStatePayload> ExtendedSystemState { get; }

        /// <summary>
        /// Represents the battery status.
        /// </summary>
        /// <remarks>
        /// This property provides an instance of the <see cref="IRxValue{T}"/> interface which holds the battery status payload.
        /// </remarks>
        /// <value>
        /// An instance of the <see cref="IRxValue{T}"/> interface containing the battery status payload.
        /// </value>
        IRxValue<BatteryStatusPayload> Battery { get; }

        /// <summary>
        /// Request a data stream.
        /// DEPRECATED: Replaced by SET_MESSAGE_INTERVAL (2015-08).
        /// </summary>
        /// <param name="streamId">The ID of the data stream.</param>
        /// <param name="rateHz">The rate of the data stream in Hz.</param>
        /// <param name="startStop">Whether to start or stop the data stream.</param>
        /// <param name="cancel">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous request for the data stream.</returns>
        Task RequestDataStream(byte streamId, ushort rateHz, bool startStop, CancellationToken cancel = default);
    }

    /// <summary>
    /// Represents a telemetry client for communicating with a MAVLink microservice.
    /// </summary>
    public class TelemetryClient : MavlinkMicroserviceClient, ITelemetryClient
    {
        /// <summary>
        /// Provides logging functionality.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Represents a reactive value for holding the status payload of a radio.
        /// </summary>
        private readonly RxValue<RadioStatusPayload> _radio;

        /// <summary>
        /// Represents the current system status.
        /// </summary>
        private readonly RxValue<SysStatusPayload> _systemStatus;

        /// <summary>
        /// The extended system state variable.
        /// </summary>
        /// <remarks>
        /// This variable holds the extended system state information which includes additional payload data.
        /// </remarks>
        private readonly RxValue<ExtendedSysStatePayload> _extendedSystemState;

        /// <summary>
        /// Represents the battery status.
        /// </summary>
        private readonly RxValue<BatteryStatusPayload> _battery;

        private readonly IDisposable _disposeIt;

        /// TelemetryClient class is responsible for handling telemetry data received from an MAVLink connection.
        /// It extends the base class "RTT".
        /// Constructors:
        /// - TelemetryClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, IPacketSequenceCalculator seq)
        /// - Initializes a new instance of the TelemetryClient class.
        /// - Parameters:
        /// - connection: The IMavlinkV2Connection instance representing the MAVLink connection.
        /// - identity: The MavlinkClientIdentity instance representing the identity of the MAVLink client.
        /// - seq: The IPacketSequenceCalculator instance used for calculating packet sequence numbers.
        /// - Returns: A new instance of the TelemetryClient class.
        /// Properties:
        /// - _radio: An RxValue instance of type RadioStatusPayload used for storing radio status data.
        /// - _systemStatus: An RxValue instance of type SysStatusPayload used for storing system status data.
        /// - _extendedSystemState: An RxValue instance of type ExtendedSysStatePayload used for storing extended system state data.
        /// - _battery: An RxValue instance of type BatteryStatusPayload used for storing battery status data.
        /// /
        public TelemetryClient(MavlinkClientIdentity identity, ICoreServices core)
            : base("RTT", identity, core)
        {
            _logger = core.Log.CreateLogger<TelemetryClient>();
            _radio = new RxValue<RadioStatusPayload>();
            var d1 = InternalFilter<RadioStatusPacket>().Select(p=>p.Payload).Subscribe(_radio);
            _systemStatus = new RxValue<SysStatusPayload>();
            var d2 = InternalFilter<SysStatusPacket>().Select(p => p.Payload).Subscribe(_systemStatus);
            _extendedSystemState = new RxValue<ExtendedSysStatePayload>();
            var d3 = InternalFilter<ExtendedSysStatePacket>().Select(p => p.Payload).Subscribe(_extendedSystemState);
            _battery = new RxValue<BatteryStatusPayload>();
            var d4 = InternalFilter<BatteryStatusPacket>().Select(p => p.Payload).Subscribe(_battery);
            _disposeIt = Disposable.Combine(_radio, _systemStatus, _extendedSystemState, _battery, d1, d2, d3, d4);
        }

        /// <summary>
        /// Represents a radio property.
        /// </summary>
        /// <value>
        /// An instance of <see cref="IRxValue{T}"/> that holds the radio status payload.
        /// </value>
        public IRxValue<RadioStatusPayload> Radio => _radio;

        /// <summary>
        /// Gets the system status.
        /// </summary>
        /// <value>
        /// The system status.
        /// </value>
        public IRxValue<SysStatusPayload> SystemStatus => _systemStatus;

        /// <summary>
        /// Represents the extended system state.
        /// </summary>
        /// <value>
        /// The extended system state.
        /// </value>
        public IRxValue<ExtendedSysStatePayload> ExtendedSystemState => _extendedSystemState;

        /// <summary>
        /// Gets the battery status value.
        /// </summary>
        /// <remarks>
        /// This property represents the current battery status.
        /// </remarks>
        /// <value>
        /// The battery status value.
        /// </value>
        public IRxValue<BatteryStatusPayload> Battery => _battery;

        /// <summary>
        /// Requests a data stream.
        /// </summary>
        /// <param name="streamId">The ID of the data stream.</param>
        /// <param name="rateHz">The rate of the data stream in Hertz.</param>
        /// <param name="startStop">True to start the stream, false to stop it.</param>
        /// <param name="cancel">A cancellation token to cancel the operation (optional).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task RequestDataStream(byte streamId, ushort rateHz, bool startStop, CancellationToken cancel = default)
        {
            _logger.ZLogDebug($"{LogSend} {( startStop ? "Enable stream":"DisableStream")} with ID '{streamId}' and rate {rateHz} Hz");
            return InternalSend<RequestDataStreamPacket>(p =>
            {
                p.Payload.TargetSystem = Identity.Target.SystemId;
                p.Payload.TargetComponent = Identity.Target.ComponentId;
                p.Payload.ReqMessageRate = rateHz;
                p.Payload.StartStop = (byte)(startStop ? 1 : 0);
                p.Payload.ReqStreamId = streamId;
            }, cancel);
        }

        public override void Dispose()
        {
            _disposeIt.Dispose();
            base.Dispose();
        }
    }
}
