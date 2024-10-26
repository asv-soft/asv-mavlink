using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
public class TelemetryClientEx : ITelemetryClientEx,IDisposable
{
    private readonly RxValueBehaviour<double> _batteryCharge;
    private readonly RxValueBehaviour<double> _batteryCurrent;
    private readonly RxValueBehaviour<double> _batteryVoltage;
    private readonly RxValueBehaviour<double> _cpuLoad;
    private readonly RxValueBehaviour<double> _dropRateComm;
    private readonly IDisposable _disposeIt;

    public TelemetryClientEx(ITelemetryClient client)
    {
        Base = client;

        _batteryCharge = new RxValueBehaviour<double>(double.NaN);
        var d1 = client.SystemStatus.Select(p=>p.BatteryRemaining < 0 ? Double.NaN : p.BatteryRemaining / 100.0d).Subscribe(_batteryCharge);
        _batteryCurrent = new RxValueBehaviour<double>(double.NaN);
        var d2 = client.SystemStatus.Select(p=>p.CurrentBattery < 0 ? Double.NaN : p.CurrentBattery / 100.0d).Subscribe(_batteryCurrent);
        _batteryVoltage = new RxValueBehaviour<double>(double.NaN);
        var d3 = client.SystemStatus.Select(p=>p.VoltageBattery / 1000.0d).Subscribe(_batteryVoltage);
        
        _cpuLoad = new RxValueBehaviour<double>(double.NaN);
        var d4 = client.SystemStatus.Select(p=>p.Load/1000D).Subscribe(_cpuLoad);
        _dropRateComm = new RxValueBehaviour<double>(double.NaN);
        var d5 = client.SystemStatus.Select(p => p.DropRateComm / 1000D).Subscribe(_dropRateComm);
        _disposeIt = Disposable.Combine(_batteryCharge,_batteryCurrent,_batteryVoltage,_cpuLoad,_dropRateComm, d1, d2, d3, d4, d5);
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

    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;
    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }
    public void Dispose()
    {
        _disposeIt.Dispose();
    }
}