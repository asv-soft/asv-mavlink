using System;
using System.Collections.Generic;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.AsvRadio;
using Asv.Mavlink.Common;

using Asv.Mavlink.AsvRadio;

using MavCmd = Asv.Mavlink.AsvRadio.MavCmd;

namespace Asv.Mavlink;

public class AsvRadioHelper
{
    public const string IfcName = "ASV_RADIO";

    public static void SetArgsForRadioOn(CommandLongPayload item, uint frequencyHz, AsvRadioModulation modulation, float referenceRxPowerDbm, float txPowerDbm, AsvAudioCodec codec)
    {
        item.Command = (Common.MavCmd)AsvRadio.MavCmd.MavCmdAsvRadioOn;
        item.Param1 = BitConverter.ToSingle(BitConverter.GetBytes(frequencyHz));
        item.Param2 = BitConverter.ToSingle(BitConverter.GetBytes((uint)modulation));
        item.Param3 = referenceRxPowerDbm;
        item.Param4 = txPowerDbm;
        item.Param5 = BitConverter.ToSingle(BitConverter.GetBytes((uint)codec));
        item.Param6 = 0;
        item.Param7 = 0;
    }
    public static void GetArgsForRadioOn(CommandLongPayload item, out uint frequencyHz,out AsvRadioModulation modulation,out float referenceRxPowerDbm,out float txPowerDbm,out AsvAudioCodec codec)
    {
        if (item.Command != (Common.MavCmd)AsvRadio.MavCmd.MavCmdAsvRadioOn)
            throw new ArgumentException($"Command {item.Command} is not {AsvRadio.MavCmd.MavCmdAsvRadioOn:G}");
        frequencyHz = BitConverter.ToUInt32(BitConverter.GetBytes(item.Param1));
        modulation = (AsvRadioModulation)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param2));
        referenceRxPowerDbm = item.Param3;
        txPowerDbm = item.Param4;
        codec = (AsvAudioCodec)BitConverter.ToUInt32(BitConverter.GetBytes(item.Param5));
    }

    public static void SetArgsForRadioOff(CommandLongPayload item)
    {
        item.Command = (Common.MavCmd)AsvRadio.MavCmd.MavCmdAsvRadioOff;
        item.Param1 = 0;
        item.Param2 = 0;
        item.Param3 = 0;
        item.Param4 = 0;
        item.Param5 = 0;
        item.Param6 = 0;
        item.Param7 = 0;
    }
    public static void GetArgsForRadioOff(CommandLongPayload item)
    {
        if (item.Command != (Common.MavCmd)AsvRadio.MavCmd.MavCmdAsvRadioOff)
            throw new ArgumentException($"Command {item.Command} is not {AsvRadio.MavCmd.MavCmdAsvRadioOff:G}");
    }

    public static void SetModulation(AsvRadioCapabilitiesResponsePayload payload, IEnumerable<AsvRadioModulation> modulations)
    {
        foreach (var modulation in modulations)
        {
            var modValue = (int)modulation; // Get the numeric value of the modulation enum
            var byteIndex = modValue / 8; // Find the corresponding byte index
            var bitIndex = modValue % 8; // Find the specific bit within that byte

            if (byteIndex >= AsvRadioCapabilitiesResponsePayload.SupportedModulationMaxItemsCount) 
                throw new ArgumentOutOfRangeException(nameof(modulations),byteIndex,$"{modulation:G}={modulation:D} has value, that out of range {AsvRadioCapabilitiesResponsePayload.SupportedModulationMaxItemsCount * byte.MaxValue}"); // Ensure the byte index is within the supported range
            payload.SupportedModulation[byteIndex] |= (byte)(1 << bitIndex); // Set the specific bit
        }
    }
    public static IEnumerable<AsvRadioModulation> GetModulation(AsvRadioCapabilitiesResponsePayload payload)
    {
        for (var byteIndex = 0; byteIndex < AsvRadioCapabilitiesResponsePayload.SupportedModulationMaxItemsCount; byteIndex++)
        {
            var byteValue = payload.SupportedModulation[byteIndex];
            for (var bitIndex = 0; bitIndex < 8; bitIndex++)
            {
                if ((byteValue & (1 << bitIndex)) != 0)
                {
                    var modValue = byteIndex * 8 + bitIndex;
                    yield return (AsvRadioModulation)modValue;
                }
            }
        }
    }


}