using System;
using System.Threading;
using System.Threading.Tasks;
using R3;

namespace Asv.Mavlink;

/// <summary>
/// Represents an extended telemetry client that provides additional telemetry data.
/// </summary>
public sealed class TelemetryClientEx : ITelemetryClientEx, IDisposable, IAsyncDisposable
{
    public TelemetryClientEx(ITelemetryClient client)
    {
        Base = client;
        
        BatteryCharge = client.SystemStatus.Select(p=>p?.BatteryRemaining < 0 ? double.NaN : (p?.BatteryRemaining ?? 0) / 100.0d).ToReadOnlyReactiveProperty();;
        BatteryCurrent = client.SystemStatus.Select(p=>p?.CurrentBattery < 0 ? double.NaN : (p?.CurrentBattery ?? 0) / 100.0d).ToReadOnlyReactiveProperty();
        BatteryVoltage = client.SystemStatus.Select(p=>(p?.VoltageBattery ?? 0) / 1000.0d).ToReadOnlyReactiveProperty();
        CpuLoad = client.SystemStatus.Select(p=>(p?.Load ?? 0)/1000D).ToReadOnlyReactiveProperty();
        DropRateCommunication = client.SystemStatus.Select(p => (p?.DropRateComm ?? 0) / 1000D).ToReadOnlyReactiveProperty();
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
    /// It is an ReadOnlyReactiveProperty, which allows for subscribing to changes in the battery charge level.
    /// </remarks>
    public ReadOnlyReactiveProperty<double> BatteryCharge { get; }

    /// <summary>
    /// The current value of the battery.
    /// </summary>
    /// <remarks>
    /// This property represents the current flowing through the battery.
    /// </remarks>
    public ReadOnlyReactiveProperty<double> BatteryCurrent { get; }

    /// <summary>
    /// The battery voltage property.
    /// </summary>
    /// <value>
    /// Provides access to the battery voltage value.
    /// </value>
    public ReadOnlyReactiveProperty<double> BatteryVoltage { get; }

    /// <summary>
    /// Represents the CPU load as a reactive value.
    /// </summary>
    public ReadOnlyReactiveProperty<double> CpuLoad { get; }

    /// <summary>
    /// Gets the drop rate of communication.
    /// </summary>
    /// <value>
    /// The property representing the drop rate of communication.
    /// </value>
    public ReadOnlyReactiveProperty<double> DropRateCommunication { get; }

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
        BatteryCharge.Dispose();
        BatteryCurrent.Dispose();
        BatteryVoltage.Dispose();
        CpuLoad.Dispose();
        DropRateCommunication.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(BatteryCharge).ConfigureAwait(false);
        await CastAndDispose(BatteryCurrent).ConfigureAwait(false);
        await CastAndDispose(BatteryVoltage).ConfigureAwait(false);
        await CastAndDispose(CpuLoad).ConfigureAwait(false);
        await CastAndDispose(DropRateCommunication).ConfigureAwait(false);

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