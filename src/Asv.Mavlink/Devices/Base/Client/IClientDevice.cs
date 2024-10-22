using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink;

/// <summary>
/// Represents a client device that communicates over a Mavlink connection.
/// </summary>
public interface IClientDevice
{
    /// <summary>
    /// Gets the full ID of the property.
    /// </summary>
    /// <remarks>
    /// The full ID is a unique identifier that represents the property.
    /// </remarks>
    /// <returns>
    /// The full ID of the property as a <see cref="ushort"/> value.
    /// </returns>
    ushort FullId { get; }

    /// <summary>
    /// Gets the identity of the Mavlink client.
    /// </summary>
    /// <returns>The identity of the Mavlink client.</returns>
    MavlinkClientIdentity Identity { get; }

    ICoreServices Core { get; }
    /// <summary>
    /// Gets the device class.
    /// </summary>
    /// <value>
    /// The device class.
    /// </value>
    DeviceClass Class { get; }

    /// <summary>
    /// Gets the value of the Name property.
    /// </summary>
    /// <returns>The value of the Name property.</returns>
    IRxValue<string> Name { get; }

    /// <summary>
    /// Gets the interface for the heartbeat client.
    /// </summary>
    /// <value>
    /// The heartbeat client.
    /// </value>
    IHeartbeatClient Heartbeat { get; }

    /// <summary>
    /// Gets the client for retrieving the status text.
    /// </summary>
    /// <value>
    /// The client for retrieving the status text.
    /// </value>
    IStatusTextClient StatusText { get; }

    /// <summary>
    /// Gets the initial value of the property.
    /// </summary>
    /// <value>
    /// An instance of <see cref="IRxValue{T}"/> representing the initial value
    /// of the property.
    /// </value>
    IRxValue<InitState> OnInit { get; }
}

/// <summary>
/// Provides helper methods for working with client devices.
/// </summary>
public static class ClientDeviceHelper
{
    /// <summary>
    /// Waits until the client device is connected.
    /// </summary>
    /// <param name="client">The client device.</param>
    /// <param name="timeoutMs">Timeout</param>
    public static void WaitUntilConnect(this IClientDevice client, int timeoutMs = 3000)
    {
        var tcs = new TaskCompletionSource();
        using var subscribe = client.Heartbeat.Link.Where(s => s == LinkState.Connected).FirstAsync().Subscribe(_ =>
        {
            tcs.TrySetResult();
        });
        tcs.Task.Wait(timeoutMs);
    }

    /// <summary>
    /// Waits until the client is connected and initialized.
    /// </summary>
    /// <param name="client">The IVehicleClient object.</param>
    public static void WaitUntilConnectAndInit(this IVehicleClient client, int timeoutMs = 3000)
    {
        client.WaitUntilConnect(timeoutMs);
        var tcs = new TaskCompletionSource();
        using var subscribe = client.OnInit.Where(s => s == InitState.Complete).FirstAsync().Subscribe(_ =>
        {
            tcs.TrySetResult();
        });
        tcs.Task.Wait(timeoutMs);
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