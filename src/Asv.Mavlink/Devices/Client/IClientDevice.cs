using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using DynamicData.Binding;
using R3;

namespace Asv.Mavlink;

/// <summary>
/// Represents a mavlink client device that communicates over a Mavlink connection.
/// </summary>
public interface IClientDevice
{
    DeviceClass Class { get; }
    ReadOnlyReactiveProperty<InitState> InitState { get; }
    ReadOnlyReactiveProperty<string> Name { get; }
    IHeartbeatClient Heartbeat { get; }
    IEnumerable<IMavlinkMicroserviceClient> Microservices { get; }
    MavlinkClientIdentity Identity => Heartbeat.Identity;
    public Observable<bool> IsInitComplete => InitState.Select(s => s == Mavlink.InitState.Complete);
    
}

public static class ClientDeviceHelper
{
    public static TMicroservice? GetMicroservice<TMicroservice>(this IClientDevice src) where TMicroservice :  IMavlinkMicroserviceClient
    {
        return (TMicroservice?)src.Microservices.FirstOrDefault(x=> x is TMicroservice);
    }
    
    public static async Task WaitUntilConnect(this IClientDevice src, int timeoutMs = 3000)
    {
        using var cancel = new CancellationTokenSource(TimeSpan.FromMilliseconds(timeoutMs),src.Heartbeat.Core.TimeProvider);
        var tcs = new TaskCompletionSource();
        cancel.Token.Register(() => tcs.TrySetCanceled());
        using var c = src.Heartbeat.Link.Where(s => s == LinkState.Connected)
            .Subscribe(x => tcs.TrySetResult());
        await tcs.Task.ConfigureAwait(false);
    }
    
    public static async void WaitUntilConnectAndInit(this IClientDevice src, int timeoutMs = 3000)
    {
        await src.WaitUntilConnect(timeoutMs).ConfigureAwait(false);
        using var cancel = new CancellationTokenSource(TimeSpan.FromMilliseconds(timeoutMs),src.Heartbeat.Core.TimeProvider);
        var tcs = new TaskCompletionSource();
        cancel.Token.Register(() => tcs.TrySetCanceled());
        using var c = src.InitState.Where(s => s == Mavlink.InitState.Complete)
            .Subscribe(x => tcs.TrySetResult());
        await tcs.Task.ConfigureAwait(false);
    }
}

/// <summary>
/// Represents the initialization state of a process.
/// </summary>
public enum InitState
{
    /// <summary>
    /// Represents the initialization state of a process, when waiting for a connection.
    /// </summary>
    WaitConnection,

    /// <summary>
    /// Represents the state of an initialization process where the initialization has failed.
    /// </summary>
    Failed,

    /// <summary>
    /// Represents the current initialization state as being in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// Represents the initialization state of a process.
    /// </summary>
    Complete
}

/// Represents the class of a device.
/// /
public enum DeviceClass
{
    Unknown,
    /// <summary>
    /// Represents a device class category of Plane.
    /// </summary>
    Plane,
    /// <summary>
    /// Represents the Copter device class.
    /// </summary>
    Copter,
    /// <summary>
    /// Represents a device of GBS RTK class.
    /// </summary>
    GbsRtk,
    /// <summary>
    /// Represents the class of a device.
    /// </summary>
    SdrPayload,
    /// <summary>
    /// Radio transmitter device class.
    /// </summary>
    Radio,
    /// <summary>
    /// Represents the device class for ADS-B devices.
    /// </summary>
    Adsb,
    /// <summary>
    /// Represents the device class for RF signal analyzer devices.
    /// </summary>
    Rfsa,
    /// <summary>
    /// Represents the device class for Radio Signal Generator and Analyzer devices.
    /// </summary>
    Rsga,
}
