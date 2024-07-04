using System;
using System.Collections;
using System.Collections.Generic;
using Asv.Mavlink.V2.AsvRsga;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class RsgaHelper
{
    public static void SetArgsForSetMode(CommandLongPayload item, AsvRsgaCustomMode mode)
    {
        
        item.Command = (V2.Common.MavCmd)V2.AsvRsga.MavCmd.MavCmdAsvRsgaSetMode;
        item.Param1 = BitConverter.ToSingle(BitConverter.GetBytes((uint)mode));
        item.Param2 = float.NaN;
        item.Param3 = float.NaN;
        item.Param4 = float.NaN;
        item.Param5 = float.NaN;
        item.Param6 = float.NaN;
        item.Param7 = float.NaN;
    }

    public static void GetArgsForSetMode(CommandLongPayload item, out AsvRsgaCustomMode mode)
    {
        if (item.Command != (V2.Common.MavCmd)V2.AsvRsga.MavCmd.MavCmdAsvRsgaSetMode)
            throw new ArgumentException($"Command {item.Command:G} is not {V2.AsvRsga.MavCmd.MavCmdAsvRsgaSetMode:G}");
        mode = (AsvRsgaCustomMode)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
       
    }

    public static void SetSupportedModes(AsvRsgaCompatibilityResponsePayload payload, IEnumerable<AsvRsgaCustomMode> modes)
    {
        var arr = new BitArray(payload.SupportedModes.Length * sizeof(byte));
        foreach (var mode in modes)
        {
            var index = (int)mode;
            if (index < 0 || index >= arr.Length)
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
}

public class SupportedWorkMode
{
    
    
    public AsvRsgaCustomMode Mode { get; set; }
    
}