using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using R3;

namespace Asv.Mavlink;

/// <summary>
/// Represents an extended telemetry client that provides additional telemetry data.
/// </summary>
public sealed class TelemetryClientEx : ITelemetryClientEx,IDisposable, IAsyncDisposable
{
    private readonly ReactiveProperty<double> _batteryCharge;
    private readonly ReactiveProperty<double> _batteryCurrent;
    private readonly ReactiveProperty<double> _batteryVoltage;
    private readonly ReactiveProperty<double> _cpuLoad;
    private readonly ReactiveProperty<double> _dropRateComm;
    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    private readonly IDisposable _sub3;
    private readonly IDisposable _sub4;
    private readonly IDisposable _sub5;

    public TelemetryClientEx(ITelemetryClient client)
    {
        Base = client;

        _batteryCharge = new ReactiveProperty<double>(double.NaN);
        _sub1 = client.SystemStatus.Select(p=>p?.BatteryRemaining < 0 ? double.NaN : (p?.BatteryRemaining ?? 0) / 100.0d).Subscribe(_batteryCharge.AsObserver());
        _batteryCurrent = new ReactiveProperty<double>(double.NaN);
        _sub2 = client.SystemStatus.Select(p=>p?.CurrentBattery < 0 ? double.NaN : (p?.CurrentBattery ?? 0) / 100.0d).Subscribe(_batteryCurrent.AsObserver());
        _batteryVoltage = new ReactiveProperty<double>(double.NaN);
        _sub3 = client.SystemStatus.Select(p=>(p?.VoltageBattery ?? 0) / 1000.0d).Subscribe(_batteryVoltage.AsObserver());
        
        _cpuLoad = new ReactiveProperty<double>(double.NaN);
        _sub4 = client.SystemStatus.Select(p=>(p?.Load ?? 0)/1000D).Subscribe(_cpuLoad.AsObserver());
        _dropRateComm = new ReactiveProperty<double>(double.NaN);
        _sub5 = client.SystemStatus.Select(p => (p?.DropRateComm ?? 0) / 1000D).Subscribe(_dropRateComm.AsObserver());
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
    /// It is an ReadOnlyReactiveProperty<double>, which allows for subscribing to changes in the battery charge level.
    /// </remarks>
    public ReadOnlyReactiveProperty<double> BatteryCharge => _batteryCharge;

    /// <summary>
    /// The current value of the battery.
    /// </summary>
    /// <remarks>
    /// This property represents the current flowing through the battery.
    /// </remarks>
    public ReadOnlyReactiveProperty<double> BatteryCurrent => _batteryCurrent;

    /// <summary>
    /// The battery voltage property.
    /// </summary>
    /// <value>
    /// Provides access to the battery voltage value.
    /// </value>
    public ReadOnlyReactiveProperty<double> BatteryVoltage => _batteryVoltage;

    /// <summary>
    /// Represents the CPU load as a reactive value.
    /// </summary>
    public ReadOnlyReactiveProperty<double> CpuLoad => _cpuLoad;

    /// <summary>
    /// Gets the drop rate of communication.
    /// </summary>
    /// <value>
    /// An object that implements the <see cref="IRxValue{Double}"/> interface, representing the drop rate of communication.
    /// </value>
    public ReadOnlyReactiveProperty<double> DropRateCommunication => _dropRateComm;

    public string Name => $"{Base.Name}Ex";
    public MavlinkClientIdentity Identity => Base.Identity;
    public ICoreServices Core => Base.Core;
    public Task Init(CancellationToken cancel = default)
    {
        return Task.CompletedTask;
    }

    #region Dispose

    public void Dispose()
    {
        _batteryCharge.Dispose();
        _batteryCurrent.Dispose();
        _batteryVoltage.Dispose();
        _cpuLoad.Dispose();
        _dropRateComm.Dispose();
        _sub1.Dispose();
        _sub2.Dispose();
        _sub3.Dispose();
        _sub4.Dispose();
        _sub5.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_batteryCharge).ConfigureAwait(false);
        await CastAndDispose(_batteryCurrent).ConfigureAwait(false);
        await CastAndDispose(_batteryVoltage).ConfigureAwait(false);
        await CastAndDispose(_cpuLoad).ConfigureAwait(false);
        await CastAndDispose(_dropRateComm).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);
        await CastAndDispose(_sub3).ConfigureAwait(false);
        await CastAndDispose(_sub4).ConfigureAwait(false);
        await CastAndDispose(_sub5).ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    #endregion
}