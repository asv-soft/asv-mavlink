using System;
using System.Collections;
using System.Collections.Generic;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;


namespace Asv.Mavlink;

public static class RsgaHelper
{
    
    public const string MicroserviceExName = $"{MicroserviceName}EX";
    public const string MicroserviceName = "RSGA";
    
    public static void SetArgsForSetMode(CommandLongPayload item, AsvRsgaCustomMode mode, float param2 = float.NaN, float param3 = float.NaN, float param4 = float.NaN, float param5 = float.NaN, float param6 = float.NaN, float param7 = float.NaN)
    {
        item.Command = (Common.MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaSetMode;
        item.Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)mode));
        item.Param2 = float.NaN;
        item.Param3 = float.NaN;
        item.Param4 = float.NaN;
        item.Param5 = float.NaN;
        item.Param6 = float.NaN;
        item.Param7 = float.NaN;
    }

    public static void GetArgsForSetMode(CommandLongPayload item, out AsvRsgaCustomMode mode, out float param2, out float param3, out float param4, out float param5, out float param6, out float param7)
    {
        if (item.Command != (Common.MavCmd)AsvRsga.MavCmd.MavCmdAsvRsgaSetMode)
            throw new ArgumentException($"Command {item.Command:G} is not {AsvRsga.MavCmd.MavCmdAsvRsgaSetMode:G}");
        mode = (AsvRsgaCustomMode)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
       
        param2 = item.Param2;
        param3 = item.Param3;
        param4 = item.Param4;
        param5 = item.Param5;
        param6 = item.Param6;
        param7 = item.Param7;
        
    }

    public static void SetSupportedModes(AsvRsgaCompatibilityResponsePayload payload, IEnumerable<AsvRsgaCustomMode> modes)
    {
        var arr = new BitArray(payload.SupportedModes.Length * 8);
        foreach (var mode in modes)
        {
            var index = (int)mode;
            if (index < 0 || index >= arr.Length * sizeof(byte))
                throw new ArgumentOutOfRangeException(nameof(modes));
            arr[index] = true;
        }
        arr.CopyTo(payload.SupportedModes, 0);
    }
    public static IEnumerable<AsvRsgaCustomMode> GetSupportedModes(AsvRsgaCompatibilityResponsePayload payload)
    {
        var arr = new BitArray(payload.SupportedModes);
        for (var i = 0; i < arr.Length; i++)
        {
            if (arr[i])
                yield return (AsvRsgaCustomMode)i;
        }
    }

    #region Custom mode

    public static void SetCustomMode(HeartbeatPayload payload, AsvRsgaCustomMode mode)
    {
       payload.SetCustomMode(0,8,(byte)mode);
    }
    
    public static AsvRsgaCustomMode GetCustomMode(HeartbeatPayload? payload)
    {
        if (payload == null)
        {
            return AsvRsgaCustomMode.AsvRsgaCustomModeIdle;
        }

        return (AsvRsgaCustomMode)payload.GetCustomMode(0,8);
    }
    
    public static AsvRsgaCustomSubMode GetCustomSubMode(HeartbeatPayload? payload)
    {
        if (payload == null)
        {
            return 0;
        }

        return (AsvRsgaCustomSubMode)payload.GetCustomMode(8,8);
    }

    #endregion
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterRsga(this IServerDeviceBuilder builder)
    {
        builder.Register<IAsvRsgaServer>((identity, context,_) => new AsvRsgaServer(identity,context));
        return builder;
    }
    public static IServerDeviceBuilder RegisterRsgaEx(this IServerDeviceBuilder builder, AsvRadioCapabilities capabilities, IReadOnlySet<AsvAudioCodec> codecs)
    {
        builder
            .Register<IAsvRsgaServerEx, IAsvRsgaServer, ICommandServerEx<CommandLongPacket>,
                IStatusTextServer>((_, _, _, @base, cmd, status) =>
                new AsvRsgaServerEx(@base,cmd));
        return builder;
    }
   

    #endregion

    
}
