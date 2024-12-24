using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public abstract class CommandServerEx<TArgPacket> : MavlinkMicroserviceServer, ICommandServerEx<TArgPacket>, IDisposable, IAsyncDisposable
    where TArgPacket : MavlinkMessage
{
    private readonly Func<TArgPacket,ushort> _cmdGetter;
    private readonly Func<TArgPacket,byte> _confirmationGetter;
    private readonly ConcurrentDictionary<ushort, CommandDelegate<TArgPacket>> _registry = new();
    private readonly ILogger _logger;
    private int _isBusy;
    private int _lastCommand = -1;
    private readonly IDisposable _subscribe;
    private readonly CancellationTokenSource _disposeCancel;

    protected CommandServerEx(
        ICommandServer server,
        Observable<TArgPacket> commandsPipe, 
        Func<TArgPacket,ushort> cmdGetter, 
        Func<TArgPacket,byte> confirmationGetter ) 
        : base(CommandHelper.MicroserviceTypeName,server.Identity,server.Core)
    {
        Base = server;
        _disposeCancel = new CancellationTokenSource();
        _logger = server.Core.LoggerFactory.CreateLogger<CommandServerEx<TArgPacket>>();
        _cmdGetter = cmdGetter;
        _confirmationGetter = confirmationGetter;
        _subscribe = commandsPipe.SubscribeAwait(
            async (pkt, ct) => 
                await OnRequest(pkt, ct).ConfigureAwait(false)
        );
    }

    public ICommandServer Base { get; }

    public CommandDelegate<TArgPacket>? this[MavCmd cmd]
    {
        set
        {
            if (value == null)
            {
                _registry.TryRemove((ushort)cmd, out _);
                return;
            }
            _registry.AddOrUpdate((ushort)cmd, value, (_, _) => value);
        }
    }

    public IEnumerable<MavCmd> SupportedCommands =>_registry.Keys.Select(x=>(MavCmd)x);

    private async Task OnRequest(TArgPacket pkt, CancellationToken cancellationToken)
    {
        var requester = new DeviceIdentity() { ComponentId = pkt.ComponentId, SystemId = pkt.SystemId };
        var cmd = _cmdGetter(pkt);
        
        var confirmation = _confirmationGetter(pkt);
        try
        {
            // wait until prev been executed
            if (Interlocked.CompareExchange(ref _isBusy, 1, 0) !=0)
            {
                if (confirmation != 0 && _lastCommand == cmd)
                {
                    // do nothing, we already doing this task
                    
                    return;
                }
                _logger.ZLogWarning($"Reject command {pkt}): too busy now");
                await Base.SendCommandAck((MavCmd)cmd, requester,
                    CommandResult.FromResult(MavResult.MavResultTemporarilyRejected), DisposeCancel).ConfigureAwait(false);
                return;
            }
            _lastCommand = cmd;
            if (_registry.TryGetValue(cmd, out var callback) == false)
            {
                _logger.ZLogWarning($"Reject unknown command {pkt})");
                _lastCommand = -1;
                await Base.SendCommandAck((MavCmd)cmd, requester,
                    CommandResult.FromResult(MavResult.MavResultUnsupported), DisposeCancel).ConfigureAwait(false);
                return;
            }

            var result = await callback(requester,pkt, DisposeCancel).ConfigureAwait(false);
            await Base.SendCommandAck((MavCmd)cmd, requester, result, DisposeCancel).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"Error to execute command {pkt}:{e.Message}");
            _lastCommand = -1;
            await Base.SendCommandAck((MavCmd)cmd, requester,
                CommandResult.FromResult(MavResult.MavResultFailed), DisposeCancel).ConfigureAwait(false);
        }
        finally
        {
            Interlocked.Exchange(ref _isBusy, 0);
        }
    }


    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _subscribe.Dispose();
            _disposeCancel.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_subscribe).ConfigureAwait(false);
        await CastAndDispose(_disposeCancel).ConfigureAwait(false);

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