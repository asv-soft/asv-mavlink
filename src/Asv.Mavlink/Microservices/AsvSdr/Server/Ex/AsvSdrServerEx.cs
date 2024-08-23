#nullable enable
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using NLog;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;
using MavType = Asv.Mavlink.V2.Minimal.MavType;

namespace Asv.Mavlink;

public class AsvSdrServerEx : DisposableOnceWithCancel, IAsvSdrServerEx
{
    private readonly IStatusTextServer _status;
    private double _signalSendingFlag;
    private int _calibrationTableUploadFlag;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public AsvSdrServerEx(IAsvSdrServer server, IStatusTextServer status, IHeartbeatServer heartbeat, ICommandServerEx<CommandLongPacket> commands)
    {
        if (heartbeat == null) throw new ArgumentNullException(nameof(heartbeat));
        if (commands == null) throw new ArgumentNullException(nameof(commands));
        _status = status ?? throw new ArgumentNullException(nameof(status));
        Base = server ?? throw new ArgumentNullException(nameof(server));

        #region Heartbeat

        heartbeat.Set(_ =>
        {
            _.Autopilot = MavAutopilot.MavAutopilotInvalid;
            _.Type = (MavType)V2.AsvSdr.MavType.MavTypeAsvSdrPayload;
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
            if (SetMode == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            AsvSdrHelper.GetArgsForSdrSetMode(args.Payload, out var mode, out var freq, out var rate, out var sendingThinningRatio, out var referencePower);
            var result = await SetMode(mode,freq, rate,sendingThinningRatio, referencePower, cs.Token).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };
        commands[(MavCmd)V2.AsvSdr.MavCmd.MavCmdAsvSdrStartRecord] = async (id,args, cancel) =>
        {
            if (StartRecord == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            AsvSdrHelper.GetArgsForSdrStartRecord(args.Payload, out var name);
            var result = await StartRecord(name, cs.Token).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };
        commands[(MavCmd)V2.AsvSdr.MavCmd.MavCmdAsvSdrStopRecord] = async (id,args, cancel) =>
        {
            if (StopRecord == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            AsvSdrHelper.GetArgsForSdrStopRecord(args.Payload);
            var result = await StopRecord(cs.Token).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };
        commands[(MavCmd)V2.AsvSdr.MavCmd.MavCmdAsvSdrSetRecordTag] = async (id,args, cancel) =>
        {
            if (CurrentRecordSetTag == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            AsvSdrHelper.GetArgsForSdrCurrentRecordSetTag(args.Payload, out var name, out var tagType, out var valueArray);
            var result = await CurrentRecordSetTag(tagType,name,valueArray, cs.Token).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };
        commands[(MavCmd)V2.AsvSdr.MavCmd.MavCmdAsvSdrSystemControlAction] = async (id, args, cancel) =>
        {
            if (SystemControlAction == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            AsvSdrHelper.GetArgsForSdrSystemControlAction(args.Payload, out var action);
            var result = await SystemControlAction(action, cs.Token).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };
        commands[(MavCmd)V2.AsvSdr.MavCmd.MavCmdAsvSdrStartMission] = async (id, args, cancel) =>
        {
            if (StartMission == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            AsvSdrHelper.GetArgsForSdrStartMission(args.Payload, out var missionIndex);
            var result = await StartMission(missionIndex, cs.Token).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };
        commands[(MavCmd)V2.AsvSdr.MavCmd.MavCmdAsvSdrStopMission] = async (id, args, cancel) =>
        {
            if (StopMission == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            AsvSdrHelper.GetArgsForSdrStopMission(args.Payload);
            var result = await StopMission(cs.Token).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };
        
        commands[(MavCmd)V2.AsvSdr.MavCmd.MavCmdAsvSdrStartCalibration] = async (id, args, cancel) =>
        {
            if (StartCalibration == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            AsvSdrHelper.SetArgsForSdrStartCalibration(args.Payload);
            var result = await StartCalibration(cs.Token).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };
        commands[(MavCmd)V2.AsvSdr.MavCmd.MavCmdAsvSdrStopCalibration] = async (id, args, cancel) =>
        {
            if (StopCalibration == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            AsvSdrHelper.SetArgsForSdrStopCalibration(args.Payload);
            var result = await StopCalibration(cs.Token).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };

        Base.OnCalibrationTableReadRequest.Subscribe(OnCalibrationReadTable).DisposeItWith(Disposable);
        Base.OnCalibrationTableRowReadRequest.Subscribe(OnCalibrationReadTableRow).DisposeItWith(Disposable);
        Base.OnCalibrationTableUploadStart.Subscribe(OnCalibrationTableUploadStart).DisposeItWith(Disposable);
    }

    private async void OnCalibrationTableUploadStart(AsvSdrCalibTableUploadStartPacket args)
    {
        if (WriteCalibrationTable == null)
        {
            await Base.SendCalibrationAcc(args.Payload.RequestId, AsvSdrRequestAck.AsvSdrRequestAckNotSupported).ConfigureAwait(false);
            return;
        }
        if (Interlocked.CompareExchange(ref _calibrationTableUploadFlag, 1, 0) != 0)
        {
            await Base.SendCalibrationAcc(args.Payload.RequestId, AsvSdrRequestAck.AsvSdrRequestAckInProgress).ConfigureAwait(false);
            Logger.Warn($"Calibration table upload already in progress");
            return;
        }
        try
        {
            _status.Info($"Upload calibration [{args.Payload.TableIndex}] started");
            var rows = new CalibrationTableRow[args.Payload.RowCount];
            for (ushort i = 0; i < args.Payload.RowCount; i++)
            {
                var row = await Base.CallCalibrationTableUploadReadCallback(args.SystemId,args.ComponentId,args.Payload.RequestId,args.Payload.TableIndex,i,DisposeCancel).ConfigureAwait(false);
                rows[i] = row;
            }
            WriteCalibrationTable(args.Payload.TableIndex, new CalibrationTableMetadata(args), rows);
            _status.Info($"Upload calibration [{args.Payload.TableIndex}] completed");
            await Base.SendCalibrationAcc(args.Payload.RequestId, AsvSdrRequestAck.AsvSdrRequestAckOk).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _status.Info($"Upload calibration [{args.Payload.TableIndex}] error");
            Logger.Error(e,$"Upload calibration [{args.Payload.TableIndex}] error:{e.Message}");
            await Base.SendCalibrationAcc(args.Payload.RequestId, AsvSdrRequestAck.AsvSdrRequestAckFail).ConfigureAwait(false);
        }
        finally
        {
            Interlocked.Exchange(ref _calibrationTableUploadFlag, 0);
        }
        
    }
    private async void OnCalibrationReadTableRow(AsvSdrCalibTableRowReadPayload args)
    {
        if (TryReadCalibrationTableRow == null)
        {
            await Base.SendCalibrationAcc(args.RequestId, AsvSdrRequestAck.AsvSdrRequestAckNotSupported).ConfigureAwait(false);
            return;
        }
        try
        {
            if (TryReadCalibrationTableRow(args.TableIndex, args.RowIndex, out var info) == true)
            {
                await Base.SendCalibrationTableRowReadResponse(res =>
                {
                    res.RowIndex = args.RowIndex;
                    res.TableIndex = args.TableIndex;
                    res.TargetComponent = 0;
                    res.TargetSystem = 0;
                    info?.Fill(res);
                }).ConfigureAwait(false);
            }
            else
            {
                await Base.SendCalibrationAcc(args.RequestId, AsvSdrRequestAck.AsvSdrRequestAckFail).ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            await Base.SendCalibrationAcc(args.RequestId, AsvSdrRequestAck.AsvSdrRequestAckFail).ConfigureAwait(false);
        }
    }

    

    private void OnCalibrationReadTable(AsvSdrCalibTableReadPayload args)
    {
        if (TryReadCalibrationTableInfo == null)
        {
            Base.SendCalibrationAcc(args.RequestId, AsvSdrRequestAck.AsvSdrRequestAckNotSupported);
            return;
        }
        try
        {
            if (TryReadCalibrationTableInfo(args.TableIndex, out var name, out var size, out var metadata))
            {
                Base.SendCalibrationTableReadResponse(res =>
                {
                    res.TableIndex = args.TableIndex;
                    if (size != null) res.RowCount = size.Value;
                    MavlinkTypesHelper.SetString(res.TableName, name);
                    metadata?.Fill(res);
                });
            }
            else
            {
                Base.SendCalibrationAcc(args.RequestId, AsvSdrRequestAck.AsvSdrRequestAckFail);
            }
        }
        catch (Exception e)
        {
            Base.SendCalibrationAcc(args.RequestId, AsvSdrRequestAck.AsvSdrRequestAckFail);
        }
    }

    public SetModeDelegate SetMode { get; set; }
    public StartRecordDelegate StartRecord { get; set; }
    public StopRecordDelegate StopRecord { get; set; }
    public CurrentRecordSetTagDelegate CurrentRecordSetTag { get; set; }
    public SystemControlActionDelegate SystemControlAction { get; set; }
    public StartMissionDelegate StartMission { get; set; }
    public StopMissionDelegate StopMission { get; set; }
    public StartCalibrationDelegate StartCalibration { get; set; }
    public StopCalibrationDelegate StopCalibration { get; set; }
    public TryReadCalibrationTableInfoDelegate? TryReadCalibrationTableInfo { get; set; }
    public TryReadCalibrationTableRowDelegate? TryReadCalibrationTableRow { get; set; }
    public WriteCalibrationDelegate? WriteCalibrationTable { get; set; }

    public async Task<bool> SendSignal(ulong unixTime, string name, ReadOnlyMemory<double> signal,
        AsvSdrSignalFormat format, CancellationToken cancel = default)
    {
        if (Interlocked.CompareExchange(ref _signalSendingFlag, 1, 0) != 0) return false;
        try
        {
            ushort index = 0;
            while (signal.IsEmpty == false)
            {
                switch (format)
                {
                    case AsvSdrSignalFormat.AsvSdrSignalFormatRangeFloat8bit:
                    {
                        await Base.SendSignal(x =>
                        {
                            var size = sizeof(byte);

                            
                            // ReSharper disable once UselessBinaryOperation
                            var maxSendPerPacket = x.Payload.Data.Length / size;
                            var count = (byte)(signal.Length - index);
                            if (count > maxSendPerPacket)
                            {
                                count = (byte)maxSendPerPacket;
                            }

                            x.Payload.Start = index;
                            x.Payload.Count = count;
                            x.Payload.Format = format;
                            x.Payload.Total = (ushort)signal.Length;
                            MavlinkTypesHelper.SetString(x.Payload.SignalName, name);
                            x.Payload.TimeUnixUsec = unixTime;
                            double min = float.MinValue;
                            double max = float.MaxValue;
                            for (var i = 0; i < count; i++)
                            {
                                var val = signal.Span[index + i];
                                if (val < min)
                                {
                                    min = val;
                                }

                                if (val > max)
                                {
                                    max = val;
                                }
                            }

                            x.Payload.Min = (float)min;
                            x.Payload.Max = (float)max;
                            WriteRange8(signal, x, count, min, max, index);

                            signal = signal.Slice(count);
                            index += count;
                        }, cancel).ConfigureAwait(false);
                    }
                        break;
                    case AsvSdrSignalFormat.AsvSdrSignalFormatRangeFloat16bit:

                      {
                        await Base.SendSignal(x =>
                        {
                            var size = sizeof(ushort);

                           
                            // ReSharper disable once UselessBinaryOperation
                            var maxSendPerPacket = x.Payload.Data.Length / size;
                            var count = (byte)(signal.Length - index);
                            if (count > maxSendPerPacket)
                            {
                                count = (byte)maxSendPerPacket;
                            }

                            x.Payload.Start = index;
                            x.Payload.Count = count;
                            x.Payload.Format = format;
                            x.Payload.Total = (ushort)signal.Length;
                            MavlinkTypesHelper.SetString(x.Payload.SignalName, name);
                            x.Payload.TimeUnixUsec = unixTime;
                            double min = float.MinValue;
                            double max = float.MaxValue;
                            for (var i = 0; i < count; i++)
                            {
                                var val = signal.Span[index + i];
                                if (val < min)
                                {
                                    min = val;
                                }

                                if (val > max)
                                {
                                    max = val;
                                }
                            }

                            x.Payload.Min = (float)min;
                            x.Payload.Max = (float)max;
                            WriteRange16(signal, x, count, min, max, index);
                            signal = signal.Slice(count);
                            index += count;
                        }, cancel).ConfigureAwait(false);
                    }
                        break;
                    case AsvSdrSignalFormat.AsvSdrSignalFormatFloat:
                        
                      {
                        await Base.SendSignal(x =>
                        {
                            var size = sizeof(float);

                            
                            // ReSharper disable once UselessBinaryOperation
                            var maxSendPerPacket = x.Payload.Data.Length / size;
                            var count = (byte)(signal.Length - index);
                            if (count > maxSendPerPacket)
                            {
                                count = (byte)maxSendPerPacket;
                            }

                            x.Payload.Start = index;
                            x.Payload.Count = count;
                            x.Payload.Format = format;
                            x.Payload.Total = (ushort)signal.Length;
                            MavlinkTypesHelper.SetString(x.Payload.SignalName, name);
                            x.Payload.TimeUnixUsec = unixTime;
                            double min = float.MinValue;
                            double max = float.MaxValue;
                            for (var i = 0; i < count; i++)
                            {
                                var val = signal.Span[index + i];
                                if (val < min)
                                {
                                    min = val;
                                }

                                if (val > max)
                                {
                                    max = val;
                                }
                            }

                            x.Payload.Min = (float)min;
                            x.Payload.Max = (float)max;
                            
                            WriteFloat(signal, x, count, index);

                            signal = signal.Slice(count);
                            index += count;
                        }, cancel).ConfigureAwait(false);
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            Interlocked.Exchange(ref _signalSendingFlag, 0);
        }

    }

    private static void WriteRange8(ReadOnlyMemory<double> signal, AsvSdrSignalRawPacket x, byte count, double min, double max,
        ushort index)
    {
        var span = new Span<byte>(x.Payload.Data);
        for (var i = 0; i < count; i++)
        {
            BinSerialize.Write8BitRange(ref span, (float)min, (float)max, (float)signal.Span[index + i]);
        }
    }

    private static void WriteRange16(ReadOnlyMemory<double> signal, AsvSdrSignalRawPacket x, byte count, double min, double max,
        ushort index)
    {
        var span = new Span<byte>(x.Payload.Data);
        for (var i = 0; i < count; i++)
        {
            BinSerialize.Write16BitRange(ref span, (float)min, (float)max, (float)signal.Span[index + i]);
        }
    }

    private static void WriteFloat(ReadOnlyMemory<double> signal, AsvSdrSignalRawPacket x, byte count, ushort index)
    {
        var span = new Span<byte>(x.Payload.Data);
        for (var i = 0; i < count; i++)
        {
            BinSerialize.WriteFloat(ref span, (float)signal.Span[index + i]);
        }
    }

    public IAsvSdrServer Base { get; }
    public IRxEditableValue<AsvSdrCustomMode> CustomMode { get; }
}