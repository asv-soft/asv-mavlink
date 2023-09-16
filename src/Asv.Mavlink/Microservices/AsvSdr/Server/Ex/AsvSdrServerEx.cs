using System;
using System.Globalization;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class AsvSdrServerEx : DisposableOnceWithCancel, IAsvSdrServerEx
{
    private double _signalSendingFlag;

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
            var nameArray = new byte[AsvSdrHelper.RecordNameMaxLength];
            BitConverter.GetBytes(args.Payload.Param1).CopyTo(nameArray,0);
            BitConverter.GetBytes(args.Payload.Param2).CopyTo(nameArray,4);
            BitConverter.GetBytes(args.Payload.Param3).CopyTo(nameArray,8);
            BitConverter.GetBytes(args.Payload.Param4).CopyTo(nameArray,12);
            BitConverter.GetBytes(args.Payload.Param5).CopyTo(nameArray,16);
            BitConverter.GetBytes(args.Payload.Param6).CopyTo(nameArray,20);
            BitConverter.GetBytes(args.Payload.Param7).CopyTo(nameArray,24);
            var name = MavlinkTypesHelper.GetString(nameArray);
            AsvSdrHelper.CheckRecordName(name);
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
            var nameArray = new byte[AsvSdrHelper.RecordTagNameMaxLength];
            BitConverter.GetBytes(args.Payload.Param2).CopyTo(nameArray,0);
            BitConverter.GetBytes(args.Payload.Param3).CopyTo(nameArray,4);
            BitConverter.GetBytes(args.Payload.Param4).CopyTo(nameArray,8);
            BitConverter.GetBytes(args.Payload.Param5).CopyTo(nameArray,12);
            var name = MavlinkTypesHelper.GetString(nameArray); 
            AsvSdrHelper.CheckTagName(name);
            var valueArray = new byte[AsvSdrHelper.RecordTagValueMaxLength];
            BitConverter.GetBytes(args.Payload.Param6).CopyTo(valueArray,0);
            BitConverter.GetBytes(args.Payload.Param7).CopyTo(valueArray,4);
            var result = await CurrentRecordSetTag((AsvSdrRecordTagType)tagType,name,valueArray, cs.Token).ConfigureAwait(false);
            return new CommandResult(result);
        };
        commands[(MavCmd)V2.AsvSdr.MavCmd.MavCmdAsvSdrSystemControlAction] = async (id, args, cancel) =>
        {
            if (SystemControlAction == null) return new CommandResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            var action = (AsvSdrSystemControlAction)BitConverter.ToUInt32(BitConverter.GetBytes(args.Payload.Param1));
            var result = await SystemControlAction(action, cs.Token).ConfigureAwait(false);
            return new CommandResult(result);
        };
    }

    public SetModeDelegate SetMode { get; set; }
    public StartRecordDelegate StartRecord { get; set; }
    public StopRecordDelegate StopRecord { get; set; }
    public CurrentRecordSetTagDelegate CurrentRecordSetTag { get; set; }
    public SystemControlActionDelegate SystemControlAction { get; set; }
   

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