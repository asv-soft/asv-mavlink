using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public interface IModeServer<TWorkMode>
    where TWorkMode:IWorkMode  
{
    ReadOnlyReactiveProperty<bool> IsBusy { get; }
    IEnumerable<OpMode> AvailableModes { get; }
    ReadOnlyReactiveProperty<TWorkMode> CurrentMode { get; }
    Task SetMode(OpMode mode, CancellationToken cancel = default);
}

public abstract class ModeServer<TWorkMode> : MavlinkMicroserviceServer, IModeServer<TWorkMode>
    where TWorkMode:IWorkMode   
{
    private readonly IStatusTextServer _status;
    private readonly ILogger _logger;
    private int _setModeBusy;
    private readonly ReactiveProperty<TWorkMode> _currentMode;
    private readonly ReactiveProperty<bool> _isBusy = new(false);
    private readonly IDisposable _sub2;

    public ModeServer(MavlinkIdentity identity, IHeartbeatServer hb, ICommandServerEx<CommandLongPacket> command, IStatusTextServer status) 
        : base(ModeHelper.MicroserviceName, identity, hb.Core)
    {
        _status = status;
        _logger = hb.Core.LoggerFactory.CreateLogger<ModeServer<TWorkMode>>();
        
        command[MavCmd.MavCmdDoSetMode] = async (from, args, cancel) =>
        {
            var baseMode = (MavModeFlag)(uint)args.Payload.Param1;
            var customMode = (uint)args.Payload.Param2;
            var customSubMode = (uint)args.Payload.Param3;
            var mode = AvailableModes.FirstOrDefault(x=>x.EqualWithCommandLong(args));
            if (mode == null)
            {
                status.Error($"Mode ({(int)baseMode},{customMode},{customSubMode}) not found");
                _logger.ZLogInformation($"Mode ({(int)baseMode}, {customMode}, {customSubMode}) not found");
                return CommandResult.Unsupported;
            }
            await SetMode(mode, cancel).ConfigureAwait(false);
            return CommandResult.Accepted;
        };
        _sub2 = CurrentMode.Subscribe(x =>
        {
            hb.Set(hbPayload =>
            {
                hbPayload.BaseMode = x.Mode.Mode;
                hbPayload.CustomMode = x.Mode.CustomMode;
            });
        });
        _currentMode = new ReactiveProperty<TWorkMode>(IdleMode);
    }

    public ReadOnlyReactiveProperty<bool> IsBusy => _isBusy;
    public abstract IEnumerable<OpMode> AvailableModes { get; }
    protected abstract TWorkMode IdleMode { get; }
    public ReadOnlyReactiveProperty<TWorkMode> CurrentMode => _currentMode;
    public async Task SetMode(OpMode mode, CancellationToken cancel = default)
    {
        if (Interlocked.CompareExchange(ref _setModeBusy, 1, 0) != 0)
        {
            _logger.ZLogWarning($"Set mode {mode} is busy. Ignore...");
            return;
        }
        var current = CurrentMode.CurrentValue;
        _logger.ZLogTrace($"Begin change mode {current} => {mode}");
        try
        {
            
            if (current != null)
            {
                await current.Destroy(DisposeCancel).ConfigureAwait(false);
                current.Dispose();
            }
        }
        catch (Exception ex)
        {
            _logger.ZLogError($"Error to destroy or dispose mode {current}:{ex.Message}");
            // skip
        }
        
        try
        {
            using var linkCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
            var workMode = await InternalSafeSetMode(mode, linkCancel.Token).ConfigureAwait(false);
            await workMode.Init(linkCancel.Token).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _status.Error($"Fail set {mode.Name}:{e.Message}");
            _logger.ZLogError(e,$"Error to set mode {mode}:{e.Message}");
            _currentMode.OnNext(null);
        }
        finally
        {
            Interlocked.Exchange(ref _setModeBusy, 0);
        }
    }

    protected abstract ValueTask<IWorkMode> InternalSafeSetMode(OpMode mode, CancellationToken cancel);
}


public interface IWorkMode:IDisposable
{
    OpMode Mode { get; }
    Task Init(CancellationToken cancel);
    Task Destroy(CancellationToken cancel);
}
