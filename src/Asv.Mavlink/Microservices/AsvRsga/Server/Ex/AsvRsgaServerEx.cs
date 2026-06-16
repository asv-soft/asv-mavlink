using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.Common;
using Microsoft.Extensions.Logging;
using ObservableCollections;
using R3;
using ZLogger;
using MavCmd = Asv.Mavlink.Common.MavCmd;

namespace Asv.Mavlink;

public class AsvRsgaServerEx : MavlinkMicroserviceServer, IAsvRsgaServerEx
{
    private readonly ILogger _logger;
    

    public AsvRsgaServerEx(
        IAsvRsgaServer server, 
        ICommandServerEx<CommandLongPacket> commands ) : base(RsgaHelper.MicroserviceExName,server.Identity,server.Core)
    {
        _logger = server.Core.LoggerFactory.CreateLogger<AsvRsgaServerEx>();
        Base = server;
        _sub1 = server.OnCompatibilityRequest.SubscribeAwait(OnCompatibilityRequest, AwaitOperation.Parallel);
        commands[(MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaSetMode] = async (id,args, cancel) =>
        {
            if (SetMode == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            RsgaHelper.GetArgsForSetMode(args.Payload, out var mode, out var p2, out var p3, out var p4, out var p5, out var p6, out var p7);
            var result = await SetMode(mode, cancel).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };
        commands[(MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaStartRecord] = async (id,args, cancel) =>
        {
            if (StartRecord == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            RsgaHelper.GetArgsForStartRecord(args.Payload, out var name);
            var result = await StartRecord(name, cancel).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };
        commands[(MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaStopRecord] = async (id,args, cancel) =>
        {
            if (StopRecord == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            RsgaHelper.GetArgsForStopRecord(args.Payload);
            var result = await StopRecord(cancel).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };
       
    }

    

    public IAsvRsgaServer Base { get; }

    private async ValueTask OnCompatibilityRequest(AsvRsgaCompatibilityRequestPayload rx, CancellationToken cancel)
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
            await Base.SendCompatibilityResponse(tx =>
            {
                tx.RequestId = rx.RequestId;
                tx.Result = AsvRsgaRequestAck.AsvRsgaRequestAckOk;
                RsgaHelper.SetSupportedModes(tx, modes);
            }, cancel).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,$"Error to get compatibility:{e.Message}");
            await Base.SendCompatibilityResponse(tx =>
            {
                tx.RequestId = rx.RequestId;
                tx.Result = AsvRsgaRequestAck.AsvRsgaRequestAckFail;
            }, cancel).ConfigureAwait(false);
        }
    }

    public RsgaSetMode? SetMode { get; set; }
    public RsgaStartRecord? StartRecord { get; set; }
    public RsgaStopRecord? StopRecord { get; set; }
    public RsgaGetCompatibility? GetCompatibility { get; set; }

    public ValueTask SendMeasure(MavlinkV2Message message, CancellationToken cancel)
    {
        FillMessageBeforeSent(message);
        return InternalSend(message, cancel);   
    }

    public ValueTask SendChart(ReadOnlyMemory<double> values, RsgaChartSendOptions? options = null,
        CancellationToken cancel = default)
    {
        options ??= new RsgaChartSendOptions();
        if (options.Timestamp == null)
        {
            options = options with { Timestamp = Core.TimeProvider.GetUtcNow().UtcDateTime };
        }

        return InternalSend<AsvRsgaRttChartPacket>(
            packet => RsgaChartHelper.WriteChartData(packet.Payload, values.Span, options),
            cancel
        );
    }

    
    

    #region Dispose
    private readonly IDisposable _sub1;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            SetMode = null;
            StartRecord = null;
            StopRecord = null;
            GetCompatibility = null;
            
            _sub1.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        SetMode = null;
        StartRecord = null;
        StopRecord = null;
        GetCompatibility = null;
        
        if (_sub1 is IAsyncDisposable sub1AsyncDisposable)
            await sub1AsyncDisposable.DisposeAsync().ConfigureAwait(false);
        else
            _sub1.Dispose();
        
        
        await base.DisposeAsyncCore().ConfigureAwait(false);
    }

    #endregion
}
