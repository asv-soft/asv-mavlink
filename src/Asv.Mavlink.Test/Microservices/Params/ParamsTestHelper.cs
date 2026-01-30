using System.Collections.Generic;
using System.Linq;
using Asv.Mavlink.Common;

namespace Asv.Mavlink.Test;

public class ParamsTestHelper
{
    public static IMavParamEncoding Encoding => MavParamHelper.CStyleEncoding;

    public static List<MavParamTypeMetadata> ServerParamsMeta { get; } =
    [
        new("BARO_PRIMARY", MavParamType.MavParamTypeUint8)
        {
            DefaultValue = new MavParamValue((byte)0),
            MinValue = new MavParamValue((byte)0),
            MaxValue = new MavParamValue((byte)2),
            ShortDesc = "Selects the primary barometer when multiple are found",
        },

        new("SIM_CAN_TYPE1", MavParamType.MavParamTypeInt8)
        {
            DefaultValue = new MavParamValue((sbyte)0),
            MinValue = new MavParamValue((sbyte)0),
            MaxValue = new MavParamValue((sbyte)3),
            ShortDesc = "transport type for first CAN interface",
        },

        new("FENCE_ACTION", MavParamType.MavParamTypeUint16)
        {
            DefaultValue = new MavParamValue((ushort)1),
            MinValue = new MavParamValue((ushort)0),
            MaxValue = new MavParamValue((ushort)2),
        },

        new("RC7_DZ", MavParamType.MavParamTypeInt16)
        {
            DefaultValue = new MavParamValue((short)10),
            MinValue = new MavParamValue((short)0),
            MaxValue = new MavParamValue((short)200),
            ShortDesc = "Example parameter",
        },

        new("LOG_BITMASK", MavParamType.MavParamTypeUint32)
        {
            DefaultValue = new MavParamValue((uint)176126),
            MinValue = new MavParamValue((uint)0),
            MaxValue = new MavParamValue(4294967295),
            ShortDesc = "Mask for logging which messages to include",
        },

        new("WPNAV_SPEED", MavParamType.MavParamTypeInt32)
        {
            DefaultValue = new MavParamValue(500),
            MinValue = new MavParamValue(10),
            MaxValue = new MavParamValue(2000),
            ShortDesc = "Example parameter",
        },

        new("BARO_ALT_OFFSET", MavParamType.MavParamTypeReal32)
        {
            DefaultValue = new MavParamValue(0f),
            MinValue = new MavParamValue(-1000f),
            MaxValue = new MavParamValue(1000f),
            ShortDesc = "Maximum Bank Angle",
        }
    ];

    public static List<ParamDescription> ClientParamsDescriptions { get; } = ServerParamsMeta
        .Select(pm => new ParamDescription
        {
            Name = pm.Name,
            ParamType = pm.Type
        })
        .ToList();
}