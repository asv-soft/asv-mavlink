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

    public TMicroservice? GetMicroservice<TMicroservice>() where TMicroservice :  IMavlinkMicroserviceClient
    {
        return (TMicroservice?)Microservices.FirstOrDefault(x=> x is TMicroservice);
    }
    MavlinkClientIdentity Identity => Heartbeat.Identity;
    public Observable<bool> IsInitComplete => InitState.Select(s => s == Mavlink.InitState.Complete);
    public async Task WaitUntilConnect(int timeoutMs = 3000, TimeProvider? timeProvider = null)
    {
        using var cancel = new CancellationTokenSource();
        if (timeProvider != null)
        {
            cancel.CancelAfter(timeoutMs, timeProvider);
        }
        else
        {
            cancel.CancelAfter(timeoutMs);
        }
        await Heartbeat.Link.Where(s => s == LinkState.Connected).FirstAsync();
    }
    
    public async void WaitUntilConnectAndInit(int timeoutMs = 3000)
    {
        await WaitUntilConnect(timeoutMs).ConfigureAwait(false);
        await InitState.FirstAsync(s => s == Mavlink.InitState.Complete).ConfigureAwait(false);
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
