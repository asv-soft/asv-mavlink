using System;
using System.Collections.Generic;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    [Flags]
    public enum PowerCycleFlags : uint
    {
        NoFlags = 0b0000_0000,
        CanReboot = 0b0000_0001,
        CanPowerOff = 0b0000_0010,
        IsRebootRequired = 0b0000_0100,
        Reserved4 = 0b0000_1000,
        Reserved5 = 0b0001_0000,
        Reserved6 = 0b0010_0000,
        Reserved7 = 0b0100_0000,
        Reserved8 = 0b1000_0000
    }

    public static class Pv2PowerInterface
    {
        public const string InterfaceName = "Power";
        public const int InterfaceId = 2;

        public static MethodInfo<SpanVoidType, SpanVoidType> PowerOff = new(nameof(PowerOff), 0, InterfaceName,
            InterfaceId);

        public static MethodInfo<SpanVoidType, SpanVoidType>
            Reboot = new(nameof(Reboot), 1, InterfaceName, InterfaceId);

        public static readonly Pv2FlagsParamType PowerCycleCompatibility =
            new(nameof(PowerCycleCompatibility), "To apply config you must reboot system", InterfaceName, "Yes", "No",
                Pv2ParamFlags.ReadOnly | Pv2ParamFlags.Hidden,
                ("IsRebootRequired", false),
                ("CanReboot", false),
                ("CanPowerOff", false));

        public static IEnumerable<Pv2ParamType> Params
        {
            get { yield return PowerCycleCompatibility; }
        }

        public static PowerCycleFlags ConvertPowerCycleCompatibility(UintBitArray value)
        {
            return (PowerCycleFlags)value.Value;
        }
    }
}
