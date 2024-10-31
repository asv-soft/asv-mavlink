using Asv.Common;
using R3;

namespace Asv.Mavlink;

/// <summary>
/// Represents an extended TelemetryClient interface.
/// </summary>
public interface ITelemetryClientEx:IMavlinkMicroserviceClient
{
    /// <summary>
    /// Property that returns an instance of ITelemetryClient.
    /// </summary>
    /// <remarks>
    /// The ITelemetryClient interface represents a telemetry client that can be used to track telemetry data in an application.
    /// </remarks>
    /// <returns>
    /// An instance of ITelemetryClient that can be used to track telemetry data.
    /// </returns>
    ITelemetryClient Base { get; }


    /// <summary>
    /// Gets the current battery charge level.
    /// </summary>
    /// <value>
    /// The value of the battery charge level.
    /// </value>
    ReadOnlyReactiveProperty<double> BatteryCharge { get; }

    /// <summary>
    /// Gets the current value of the battery current.
    /// </summary>
    /// <returns>
    /// The current value of the battery current.
    /// </returns>
    ReadOnlyReactiveProperty<double> BatteryCurrent { get; }

    /// <summary>
    /// Gets the value of the battery voltage.
    /// </summary>
    /// <value>
    /// The battery voltage.
    /// </value>
    ReadOnlyReactiveProperty<double> BatteryVoltage { get; }

    /// <summary>
    /// Gets the current CPU load as a reactive value.
    /// </summary>
    /// <remarks>
    /// The CPU load represents the current utilization of the central processing unit (CPU) in the system.
    /// The value returned is a reactive value of type double that represents the current CPU load as a percentage.
    /// The value ranges from 0 to 100, where 0 indicates no utilization and 100 indicates full utilization.
    /// The value will change periodically based on the activity of the CPU.
    /// </remarks>
    /// <returns>
    /// The CPU load as a reactive value of type double.
    /// </returns>
    ReadOnlyReactiveProperty<double> CpuLoad { get; }

    /// <summary>
    /// Gets the rate at which communication with the server is dropping.
    /// </summary>
    /// <returns>
    /// An <see cref="IRxValue{T}"/> object representing the drop rate of communication.
    /// </returns>
    ReadOnlyReactiveProperty<double> DropRateCommunication { get; }
}