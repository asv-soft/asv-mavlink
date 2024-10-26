using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Minimal;
using R3;

namespace Asv.Mavlink;

public interface IModeClient: IMavlinkMicroserviceClient
{
    IEnumerable<OpMode> AvailableModes { get; }
    ReadOnlyReactiveProperty<OpMode> CurrentMode { get; }
    Task SetMode(OpMode mode, CancellationToken cancel = default);

}

public abstract class ModeClient : MavlinkMicroserviceClient, IModeClient
{
    private readonly ICommandClient _command;
    private readonly ReactiveProperty<OpMode> _currentMode;
    protected ModeClient(IHeartbeatClient heartbeat, ICommandClient command) : base("MODE", command.Identity, command.Core)
    {
        _command = command;
        _currentMode = new ReactiveProperty<OpMode>(OpMode.Unknown);
        heartbeat.RawHeartbeat
            .DistinctUntilChanged(x=>HashCode.Combine(x.BaseMode,x.CustomMode))
            .Select(Convert)
            .Subscribe(x => _currentMode.OnNext(x));
    }
    protected abstract OpMode Convert(HeartbeatPayload hb);
    public abstract IEnumerable<OpMode> AvailableModes { get; }
    public ReadOnlyReactiveProperty<OpMode> CurrentMode => _currentMode;

    public virtual Task SetMode(OpMode mode, CancellationToken cancel = default)
    {
        return _command.DoSetMode((uint)mode.Mode,mode.CustomMode,mode.CustomSubMode, cancel: cancel);
    }
}