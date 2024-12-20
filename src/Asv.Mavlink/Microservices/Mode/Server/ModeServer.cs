using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public class ModeServer : MavlinkMicroserviceServer, IModeServer
{
    private readonly IStatusTextServer _status;
    private readonly Func<ICustomMode, IWorkModeHandler> _handlerFactory;
    private readonly ILogger _logger;
    private int _setModeBusy;
    private readonly ReactiveProperty<IWorkModeHandler> _currentMode;
    private readonly ReactiveProperty<bool> _isBusy = new(false);
    private readonly IDisposable _sub2;
    private readonly IDisposable _sub1;
    private readonly ICustomMode _idleMode;

    public ModeServer(
        MavlinkIdentity identity, 
        IHeartbeatServer hb, 
        ICommandServerEx<CommandLongPacket> command, 
        IStatusTextServer status, 
        ICustomMode idleMode,
        IEnumerable<ICustomMode> availableModes,
        Func<ICustomMode,IWorkModeHandler> handlerFactory) 
        : base(ModeHelper.MicroserviceName, identity, hb.Core)
    {
        ArgumentNullException.ThrowIfNull(identity);
        ArgumentNullException.ThrowIfNull(hb);
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(status);
        ArgumentNullException.ThrowIfNull(idleMode);
        ArgumentNullException.ThrowIfNull(availableModes);
        ArgumentNullException.ThrowIfNull(handlerFactory);

        _status = status;
        _idleMode = idleMode;
        _handlerFactory = handlerFactory;
        _logger = hb.Core.LoggerFactory.CreateLogger<ModeServer>();
        _sub1 = Disposable.Create(() => command[MavCmd.MavCmdDoSetMode] = null);
        var builder = ImmutableArray.CreateBuilder<ICustomMode>(); 
        builder.AddRange(availableModes);
        builder.Add(idleMode);
        AvailableModes = builder.ToImmutable();
        command[MavCmd.MavCmdDoSetMode] = async (from, args, cancel) =>
        {
            var baseMode = (MavModeFlag)(uint)args.Payload.Param1;
            var customMode = (uint)args.Payload.Param2;
            var customSubMode = (uint)args.Payload.Param3;
            
            var mode = AvailableModes.FirstOrDefault(x=>x.IsCurrentMode(args.Payload));
            if (mode == null)
            {
                status.Error($"Mode ({(int)baseMode},{customMode},{customSubMode}) not found");
                _logger.ZLogInformation($"Mode ({(int)baseMode}, {customMode}, {customSubMode}) not found");
                return CommandResult.Unsupported;
            }

            if (mode.InternalMode)
            {
                status.Error($"Mode ({(int)baseMode},{customMode},{customSubMode}) is internal");
                _logger.ZLogInformation($"Mode ({(int)baseMode}, {customMode}, {customSubMode}) is internal");
                return CommandResult.Unsupported;
            }
            await SetMode(mode, null, cancel).ConfigureAwait(false);
            return CommandResult.Accepted;
        };
        _sub2 = CurrentMode.Subscribe(x => hb.Set(x.Mode.Fill));
        _currentMode = new ReactiveProperty<IWorkModeHandler>(_handlerFactory(idleMode));
    }

    public ReadOnlyReactiveProperty<bool> IsBusy => _isBusy;
    public ImmutableArray<ICustomMode> AvailableModes { get; }
    public ReadOnlyReactiveProperty<IWorkModeHandler> CurrentMode => _currentMode;
    public async Task SetMode(ICustomMode mode, Action<IWorkModeHandler>? update = null, CancellationToken cancel = default)
    {
        if (Interlocked.CompareExchange(ref _setModeBusy, 1, 0) != 0)
        {
            _logger.ZLogWarning($"Set mode {mode} is busy. Ignore...");
            return;
        }
        _isBusy.OnNext(true);
        var current = CurrentMode.CurrentValue;
        if (mode == current.Mode)
        {
            _logger.ZLogWarning($"Set mode skipped. Mode is same. Ignore...");
            update?.Invoke(current);
            return;
        }
        
        _logger.ZLogTrace($"Begin change mode {current} => {mode}");
        try
        {
            await current.Destroy(DisposeCancel).ConfigureAwait(false);
            current.Dispose();
        }
        catch (Exception ex)
        {
            _logger.ZLogError($"Error to destroy or dispose mode {current}:{ex.Message}");
            // skip
        }

        IWorkModeHandler? workMode = null;
        try
        {
            using var linkCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
            workMode = _handlerFactory(mode);
            update?.Invoke(workMode);
            await workMode.Init(linkCancel.Token).ConfigureAwait(false);
            _status.Info($"Switched to '{mode.Name}'");
        }
        catch (Exception e)
        {
            _status.Error($"Fail set {mode.Name}:{e.Message}");
            _logger.ZLogError(e,$"Error to set mode {mode}:{e.Message}");
            _currentMode.OnNext(_handlerFactory(_idleMode));
            workMode?.Dispose();
        }
        finally
        {
            _isBusy.OnNext(false);
            Interlocked.Exchange(ref _setModeBusy, 0);
        }
    }


    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            var currentMode = _currentMode.CurrentValue;
            currentMode.Dispose();
            _currentMode.Dispose();
            _isBusy.Dispose();
            _sub2.Dispose();
            _sub1.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        var currentMode = _currentMode.CurrentValue;
        currentMode.Dispose();
        await CastAndDispose(_currentMode).ConfigureAwait(false);
        await CastAndDispose(_isBusy).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);

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