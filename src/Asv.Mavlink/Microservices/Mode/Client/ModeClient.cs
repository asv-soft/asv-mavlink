using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.V2.Minimal;
using R3;

namespace Asv.Mavlink;

public abstract class ModeClient : MavlinkMicroserviceClient, IModeClient
{
    private readonly ICommandClient _command;

    protected ModeClient(IHeartbeatClient heartbeat, ICommandClient command) : base("MODE", command.Identity, command.Core)
    {
        _command = command;
        CurrentMode = heartbeat.RawHeartbeat
            .DistinctUntilChangedBy(x => HashCode.Combine(x.BaseMode, x.CustomMode))
            .Select(Convert).ToReadOnlyReactiveProperty();
    }
    protected abstract OpMode Convert(HeartbeatPayload? hb);
    public abstract IEnumerable<OpMode> AvailableModes { get; }
    public ReadOnlyReactiveProperty<OpMode> CurrentMode { get; }

    public virtual Task SetMode(OpMode mode, CancellationToken cancel = default)
    {
        return _command.DoSetMode((uint)mode.Mode,mode.CustomMode,mode.CustomSubMode, cancel: cancel);
    }

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            CurrentMode.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        if (CurrentMode is IAsyncDisposable currentModeAsyncDisposable)
            await currentModeAsyncDisposable.DisposeAsync().ConfigureAwait(false);
        else
            CurrentMode.Dispose();

        await base.DisposeAsyncCore().ConfigureAwait(false);
    }

    #endregion
}