using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Minimal;

using R3;

namespace Asv.Mavlink;

public class ModeClient : MavlinkMicroserviceClient, IModeClient
{
    private readonly ICommandClient _command;
    private readonly ICustomMode _unknownMode;

    public ModeClient(IHeartbeatClient heartbeat, ICommandClient command, ICustomMode unknownMode,IEnumerable<ICustomMode> availableModes) 
        : base(ModeHelper.MicroserviceName, command?.Identity ?? throw new ArgumentNullException(), command?.Core ?? throw new ArgumentNullException())
    {
        ArgumentNullException.ThrowIfNull(heartbeat);
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(unknownMode);
        var builder = ImmutableArray.CreateBuilder<ICustomMode>();
        builder.Add(unknownMode);
        builder.AddRange(availableModes);
        AvailableModes = builder.ToImmutable();
        _command = command;
        _unknownMode = unknownMode;
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        CurrentMode = heartbeat.RawHeartbeat.Where(x=>x!= null)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            .DistinctUntilChangedBy(x => HashCode.Combine(x.BaseMode, x.CustomMode))
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            .Select(Convert)
            .ToReadOnlyReactiveProperty(unknownMode);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    private ICustomMode Convert(HeartbeatPayload? hb)
    {
        return AvailableModes.FirstOrDefault(x => x.IsCurrentMode(hb)) ?? _unknownMode;
    }
    public ImmutableArray<ICustomMode> AvailableModes { get; }
    public ReadOnlyReactiveProperty<ICustomMode> CurrentMode { get; }

    public virtual Task SetMode(ICustomMode mode, CancellationToken cancel = default)
    {
        ArgumentNullException.ThrowIfNull(mode);
        if (mode.InternalMode)
        {
            throw new NotSupportedException(RS.ModeClient_SetMode_Mode_is_internal_and_cannot_be_set_directly);
        }

        mode.GetCommandLongArgs(out var baseMode, out var customMode, out var customSubMode);
        return _command.DoSetMode(baseMode,customMode,customSubMode, cancel: cancel);
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
        CurrentMode.Dispose();
        await base.DisposeAsyncCore().ConfigureAwait(false);
    }

    #endregion
}