using System;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public static class RfsaHelper
{
    public static void SetArgsForEnableCommand(CommandLongPayload item, ulong frequencyHz, uint spanHz)
    {
        var freqArray = BitConverter.GetBytes(frequencyHz);
        item.Command = (V2.Common.MavCmd)V2.AsvRfsa.MavCmd.MavCmdAsvRfsaOn;
        item.Param1 = BitConverter.ToSingle(freqArray, 0);
        item.Param2 = BitConverter.ToSingle(freqArray, 4);
        item.Param3 = BitConverter.ToSingle(BitConverter.GetBytes(spanHz));
        item.Param4 = Single.NaN;
        item.Param5 = Single.NaN;
        item.Param6 = Single.NaN;
        item.Param7 = Single.NaN;
    }

    public static void SetArgsForDisableCommand(CommandLongPayload item)
    {
        item.Command = (V2.Common.MavCmd)V2.AsvRfsa.MavCmd.MavCmdAsvRfsaOff;
        item.Param1 = Single.NaN;
        item.Param2 = Single.NaN;
        item.Param3 = Single.NaN;
        item.Param4 = Single.NaN;
        item.Param5 = Single.NaN;
        item.Param6 = Single.NaN;
        item.Param7 = Single.NaN;
    }
}