using System;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class AsvSdrServerEx : DisposableOnceWithCancel, IAsvSdrServerEx
{
    public AsvSdrServerEx(IAsvSdrServer server,IHeartbeatServer heartbeat, ICommandServerEx<CommandLongPacket> commands)
    {
        if (heartbeat == null) throw new ArgumentNullException(nameof(heartbeat));
        if (commands == null) throw new ArgumentNullException(nameof(commands));
        Base = server ?? throw new ArgumentNullException(nameof(server));

        #region Heartbeat

        heartbeat.Set(_ =>
        {
            _.Autopilot = MavAutopilot.MavAutopilotInvalid;
            _.Type = (Asv.Mavlink.V2.Common.MavType)V2.AsvSdr.MavType.MavTypeAsvSdrPayload;
            _.SystemStatus = MavState.MavStateActive;
            _.BaseMode = MavModeFlag.MavModeFlagCustomModeEnabled;
            _.MavlinkVersion = 3;
            _.CustomMode = (uint)AsvSdrCustomMode.AsvSdrCustomModeIdle;
        });
        CustomMode = new RxValue<AsvSdrCustomMode>().DisposeItWith(Disposable);
        CustomMode.DistinctUntilChanged().Subscribe(mode => heartbeat.Set(_ =>
        {
            _.CustomMode = (uint)mode;
        })).DisposeItWith(Disposable);

        #endregion
        
        commands[(MavCmd)V2.AsvSdr.MavCmd.MavCmdAsvSdrSetMode] = async (id,args, cancel) =>
        {
            if (SetMode == null) return new CommandResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            var mode = (AsvSdrCustomMode)BitConverter.ToUInt32(BitConverter.GetBytes(args.Payload.Param1));
            var freqArray = new byte[8];
            BitConverter.GetBytes(args.Payload.Param2).CopyTo(freqArray,0);
            BitConverter.GetBytes(args.Payload.Param3).CopyTo(freqArray,4);
            var freq = BitConverter.ToUInt64(freqArray,0);
            var rate = args.Payload.Param4;
            var sendingThinningRatio = BitConverter.ToInt32(BitConverter.GetBytes(args.Payload.Param5));
            var result = await SetMode(mode,freq, rate,sendingThinningRatio, cs.Token).ConfigureAwait(false);
            return new CommandResult(result);
        };
        commands[(MavCmd)V2.AsvSdr.MavCmd.MavCmdAsvSdrStartRecord] = async (id,args, cancel) =>
        {
            if (StartRecord == null) return new CommandResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            var nameArray = new byte[SdrWellKnown.RecordNameMaxLength];
            BitConverter.GetBytes(args.Payload.Param1).CopyTo(nameArray,0);
            BitConverter.GetBytes(args.Payload.Param2).CopyTo(nameArray,4);
            BitConverter.GetBytes(args.Payload.Param3).CopyTo(nameArray,8);
            BitConverter.GetBytes(args.Payload.Param4).CopyTo(nameArray,12);
            BitConverter.GetBytes(args.Payload.Param5).CopyTo(nameArray,16);
            BitConverter.GetBytes(args.Payload.Param6).CopyTo(nameArray,20);
            BitConverter.GetBytes(args.Payload.Param7).CopyTo(nameArray,24);
            var name = MavlinkTypesHelper.GetString(nameArray);
            SdrWellKnown.CheckRecordName(name);
            var result = await StartRecord(name, cs.Token).ConfigureAwait(false);
            return new CommandResult(result);
        };
        commands[(MavCmd)V2.AsvSdr.MavCmd.MavCmdAsvSdrStopRecord] = async (id,args, cancel) =>
        {
            if (StopRecord == null) return new CommandResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            var result = await StopRecord(cs.Token).ConfigureAwait(false);
            return new CommandResult(result);
        };
        commands[(MavCmd)V2.AsvSdr.MavCmd.MavCmdAsvSdrSetRecordTag] = async (id,args, cancel) =>
        {
            if (CurrentRecordSetTag == null) return new CommandResult(MavResult.MavResultUnsupported);
            var tagType = BitConverter.ToUInt32(BitConverter.GetBytes(args.Payload.Param1));
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            var nameArray = new byte[SdrWellKnown.RecordTagNameMaxLength];
            BitConverter.GetBytes(args.Payload.Param2).CopyTo(nameArray,0);
            BitConverter.GetBytes(args.Payload.Param3).CopyTo(nameArray,4);
            BitConverter.GetBytes(args.Payload.Param4).CopyTo(nameArray,8);
            BitConverter.GetBytes(args.Payload.Param5).CopyTo(nameArray,12);
            var name = MavlinkTypesHelper.GetString(nameArray); 
            SdrWellKnown.CheckTagName(name);
            var valueArray = new byte[SdrWellKnown.RecordTagValueMaxLength];
            BitConverter.GetBytes(args.Payload.Param6).CopyTo(valueArray,0);
            BitConverter.GetBytes(args.Payload.Param7).CopyTo(valueArray,4);
            var result = await CurrentRecordSetTag((AsvSdrRecordTagType)tagType,name,valueArray, cs.Token).ConfigureAwait(false);
            return new CommandResult(result);
        };
    }

    public SetModeDelegate SetMode { get; set; }
    public StartRecordDelegate StartRecord { get; set; }
    public StopRecordDelegate StopRecord { get; set; }
    public CurrentRecordSetTagDelegate CurrentRecordSetTag { get; set; }
    public IAsvSdrServer Base { get; }
    public IRxEditableValue<AsvSdrCustomMode> CustomMode { get; }
}