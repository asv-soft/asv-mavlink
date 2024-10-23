using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using DynamicData.Binding;
using R3;

namespace Asv.Mavlink;

/// <summary>
/// Represents a client device that communicates over a Mavlink connection.
/// </summary>
public interface IClientDevice
{
    ushort FullId { get; }
    MavlinkClientIdentity Identity { get; }
    ICoreServices Core { get; }
    DeviceClass Class { get; }
    ReadOnlyReactiveProperty<InitState> OnInit { get; }
    ReadOnlyReactiveProperty<string> Name { get; }
    
    IHeartbeatClient Heartbeat { get; }
    IStatusTextClient StatusText { get; }
    
    
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
        await OnInit.FirstAsync(s => s == InitState.Complete).ConfigureAwait(false);
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

