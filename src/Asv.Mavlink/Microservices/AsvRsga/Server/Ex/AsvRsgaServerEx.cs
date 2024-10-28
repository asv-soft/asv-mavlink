using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.AsvRsga;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class AsvRsgaServerEx : DisposableOnceWithCancel, IAsvRsgaServerEx
{
    public readonly ILogger _logger;
    public AsvRsgaServerEx(
        IAsvRsgaServer server, 
        IStatusTextServer status, 
        ICommandServerEx<CommandLongPacket> commands,
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null,
        ILoggerFactory? logFactory = null)
    {
        logFactory ??= NullLoggerFactory.Instance;
        _logger = logFactory.CreateLogger<AsvRsgaServerEx>();
        Base = server;
        server.OnCompatibilityRequest.Subscribe(OnCompatibilityRequest).DisposeItWith(Disposable);
        commands[(MavCmd)V2.AsvRsga.MavCmd.MavCmdAsvRsgaSetMode] = async (id,args, cancel) =>
        {
            if (SetMode == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            RsgaHelper.GetArgsForSetMode(args.Payload, out var mode, out var p2, out var p3, out var p4, out var p5, out var p6, out var p7);
            var result = await SetMode(mode, cs.Token).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };
    }
    
    public IAsvRsgaServer Base { get; }

    private async void OnCompatibilityRequest(AsvRsgaCompatibilityRequestPayload rx)
    {
        var modes = new HashSet<AsvRsgaCustomMode> { AsvRsgaCustomMode.AsvRsgaCustomModeIdle };
        try
        {
            if (GetCompatibility != null)
            {
                foreach (var mode in GetCompatibility())
                {
                    modes.Add(mode);
                }
            }
            await Base.SendCompatilityResponse(tx =>
            {
                tx.RequestId = rx.RequestId;
                tx.Result = AsvRsgaRequestAck.AsvRsgaRequestAckOk;
                RsgaHelper.SetSupportedModes(tx, modes);
            }).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,$"Error to get compatibility:{e.Message}");
            await Base.SendCompatilityResponse(tx =>
            {
                tx.RequestId = rx.RequestId;
                tx.Result = AsvRsgaRequestAck.AsvRsgaRequestAckFail;
            }).ConfigureAwait(false);
        }
    }

    public RsgaSetMode? SetMode { get; set; }
    public RsgaGetCompatibility? GetCompatibility { get; set; }
}