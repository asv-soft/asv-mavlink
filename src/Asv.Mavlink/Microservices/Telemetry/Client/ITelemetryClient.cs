using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using R3;

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
        ReadOnlyReactiveProperty<RadioStatusPayload?> Radio { get; }

        /// <summary>
        /// Represents the system status property.
        /// </summary>
        /// <remarks>
        /// The SystemStatus property is an IRxValue that provides the current system status information.
        /// </remarks>
        /// <seealso cref="IRxValue{T}"/>
        /// <seealso cref="SysStatusPayload"/>
        ReadOnlyReactiveProperty<SysStatusPayload?> SystemStatus { get; }

        /// <summary>
        /// Represents the extended state of the system.
        /// </summary>
        /// <returns>
        /// An observable value of type <see cref="ExtendedSysStatePayload"/> that represents the extended state of the system.
        /// </returns>
        ReadOnlyReactiveProperty<ExtendedSysStatePayload?> ExtendedSystemState { get; }

        /// <summary>
        /// Represents the battery status.
        /// </summary>
        /// <remarks>
        /// This property provides an instance of the <see cref="IRxValue{T}"/> interface which holds the battery status payload.
        /// </remarks>
        /// <value>
        /// An instance of the <see cref="IRxValue{T}"/> interface containing the battery status payload.
        /// </value>
        ReadOnlyReactiveProperty<BatteryStatusPayload?> Battery { get; }

        /// <summary>
        /// Request a data stream.
        /// DEPRECATED: Replaced by SET_MESSAGE_INTERVAL (2015-08).
        /// </summary>
        /// <param name="streamId">The ID of the data stream.</param>
        /// <param name="rateHz">The rate of the data stream in Hz.</param>
        /// <param name="startStop">Whether to start or stop the data stream.</param>
        /// <param name="cancel">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous request for the data stream.</returns>
        ValueTask RequestDataStream(byte streamId, ushort rateHz, bool startStop, CancellationToken cancel = default);
    }
}
