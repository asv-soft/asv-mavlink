using System;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;

public static class RfsaHelper
{
    public static void SetArgsForEnableCommand(CommandLongPayload item, ulong frequencyHz, uint spanHz)
    {
        var freqArray = BitConverter.GetBytes(frequencyHz);
        item.Command = (Common.MavCmd)AsvRfsa.MavCmd.MavCmdAsvRfsaOn;
        item.Param1 = BitConverter.ToSingle(freqArray, 0);
        item.Param2 = BitConverter.ToSingle(freqArray, 4);
        item.Param3 = BitConverter.ToSingle(BitConverter.GetBytes(spanHz));
        item.Param4 = Single.NaN;
        item.Param5 = Single.NaN;
        item.Param6 = Single.NaN;
        item.Param7 = Single.NaN;
    }
    public static void GetArgsForEnableCommand(CommandLongPayload item, out ulong frequencyHz,
        out uint span)
    {
        if (item.Command != (Common.MavCmd)AsvRfsa.MavCmd.MavCmdAsvRfsaOn)
            throw new ArgumentException($"Command {item.Command:G} is not {AsvRfsa.MavCmd.MavCmdAsvRfsaOn:G}");
        var freqArray = new byte[8];
        BitConverter.GetBytes(item.Param1).CopyTo(freqArray,0);
        BitConverter.GetBytes(item.Param2).CopyTo(freqArray,4);
        frequencyHz = BitConverter.ToUInt64(freqArray,0);
        span = BitConverter.ToUInt32(BitConverter.GetBytes(item.Param3));
    }
    public static void SetArgsForDisableCommand(CommandLongPayload item)
    {
        item.Command = (Common.MavCmd)AsvRfsa.MavCmd.MavCmdAsvRfsaOff;
        item.Param1 = Single.NaN;
        item.Param2 = Single.NaN;
        item.Param3 = Single.NaN;
        item.Param4 = Single.NaN;
        item.Param5 = Single.NaN;
        item.Param6 = Single.NaN;
        item.Param7 = Single.NaN;
    }

    public static void GetArgsForDisableCommand(CommandLongPayload item)
    {
        if (item.Command != (Common.MavCmd)AsvRfsa.MavCmd.MavCmdAsvRfsaOff)
            throw new ArgumentException($"Command {item.Command:G} is not {AsvRfsa.MavCmd.MavCmdAsvRfsaOff:G}");
    }
}