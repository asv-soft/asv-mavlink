using System;
using System.Collections.Generic;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.AsvRsga;
using Asv.Mavlink.V2.Common;
using NLog;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class AsvRsgaServerEx : DisposableOnceWithCancel, IAsvRsgaServerEx
{
    public static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    public AsvRsgaServerEx(IAsvRsgaServer server, IStatusTextServer status, ICommandServerEx<CommandLongPacket> commands)
    {
        Base = server;
        server.OnCompatibilityRequest.Subscribe(OnCompatibilityRequest).DisposeItWith(Disposable);
        commands[(MavCmd)V2.AsvRsga.MavCmd.MavCmdAsvRsgaSetMode] = async (id,args, cancel) =>
        {
            if (SetMode == null) return new CommandResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            RsgaHelper.GetArgsForSetMode(args.Payload, out var mode);
            var result = await SetMode(mode, cs.Token).ConfigureAwait(false);
            return new CommandResult(result);
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
            Logger.Error(e,$"Error to get compatibility:{e.Message}");
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