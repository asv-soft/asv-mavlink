using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

/// <summary>
/// Represents an extended TelemetryClient interface.
/// </summary>
public interface ITelemetryClientEx
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
    IRxValue<double> BatteryCharge { get; }

    /// <summary>
    /// Gets the current value of the battery current.
    /// </summary>
    /// <returns>
    /// The current value of the battery current.
    /// </returns>
    IRxValue<double> BatteryCurrent { get; }

    /// <summary>
    /// Gets the value of the battery voltage.
    /// </summary>
    /// <value>
    /// The battery voltage.
    /// </value>
    IRxValue<double> BatteryVoltage { get; }

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
    IRxValue<double> CpuLoad { get; }

    /// <summary>
    /// Gets the rate at which communication with the server is dropping.
    /// </summary>
    /// <returns>
    /// An <see cref="IRxValue{T}"/> object representing the drop rate of communication.
    /// </returns>
    IRxValue<double> DropRateCommunication { get; }
}

/// <summary>
/// Represents an extended telemetry client that provides additional telemetry data.
/// </summary>
public class TelemetryClientEx : DisposableOnceWithCancel, ITelemetryClientEx
{
    /// <summary>
    /// Stores the current value of the battery charge.
    /// </summary>
    private readonly RxValue<double> _batteryCharge;

    /// <summary>
    /// This is a private readonly variable that represents the current value of the battery.
    /// </summary>
    private readonly RxValue<double> _batteryCurrent;

    /// <summary>
    /// Represents the voltage level of a battery.
    /// </summary>
    private readonly RxValue<double> _batteryVoltage;

    /// <summary>
    /// Represents the current CPU load.
    /// </summary>
    private readonly RxValue<double> _cpuLoad;

    /// Private readonly field for storing the drop rate for communication. </summary> <remarks>
    /// This variable represents the drop rate for communication. </remarks> <seealso cref="RxValue{T}"/>
    /// /
    private readonly RxValue<double> _dropRateComm;

    private readonly ILogger _logger;
    private readonly IScheduler _scheduler;

    /// initialize various RxValue properties based on the data retrieved from the `SystemStatus` property of the `client` object.
    public TelemetryClientEx(ITelemetryClient client, IScheduler? scheduler = null, ILogger? logger = null)
    {
        _scheduler = scheduler ?? Scheduler.Default;
        _logger = logger ?? NullLogger.Instance;
        Base = client;
        
        _batteryCharge = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(p=>p.BatteryRemaining < 0 ? Double.NaN : p.BatteryRemaining / 100.0d).Subscribe(_batteryCharge).DisposeItWith(Disposable);
        _batteryCurrent = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(p=>p.CurrentBattery < 0 ? Double.NaN : p.CurrentBattery / 100.0d).Subscribe(_batteryCurrent).DisposeItWith(Disposable);
        _batteryVoltage = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(p=>p.VoltageBattery / 1000.0d).Subscribe(_batteryVoltage).DisposeItWith(Disposable);
        
        _cpuLoad = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(p=>p.Load/1000D).Subscribe(_cpuLoad).DisposeItWith(Disposable);
        _dropRateComm = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(p => p.DropRateComm / 1000D).Subscribe(_dropRateComm).DisposeItWith(Disposable);
    }

    /// <summary>
    /// Gets the base telemetry client.
    /// </summary>
    /// <value>
    /// The base telemetry client.
    /// </value>
    public ITelemetryClient Base { get; }


    /// <summary>
    /// The current battery charge level.
    /// </summary>
    /// <remarks>
    /// This property represents the current charge level of the battery.
    /// It is an IRxValue<double>, which allows for subscribing to changes in the battery charge level.
    /// </remarks>
    public IRxValue<double> BatteryCharge => _batteryCharge;

    /// <summary>
    /// The current value of the battery.
    /// </summary>
    /// <remarks>
    /// This property represents the current flowing through the battery.
    /// </remarks>
    public IRxValue<double> BatteryCurrent => _batteryCurrent;

    /// <summary>
    /// The battery voltage property.
    /// </summary>
    /// <value>
    /// Provides access to the battery voltage value.
    /// </value>
    public IRxValue<double> BatteryVoltage => _batteryVoltage;

    /// <summary>
    /// Represents the CPU load as a reactive value.
    /// </summary>
    public IRxValue<double> CpuLoad => _cpuLoad;

    /// <summary>
    /// Gets the drop rate of communication.
    /// </summary>
    /// <value>
    /// An object that implements the <see cref="IRxValue{Double}"/> interface, representing the drop rate of communication.
    /// </value>
    public IRxValue<double> DropRateCommunication => _dropRateComm;
}