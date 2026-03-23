using System.Collections.Generic;
using System.Linq;
using Asv.Mavlink.Common;
using Xunit;

namespace Asv.Mavlink.Test;

public class ParamsExtTestHelper
{
    public static List<MavParamExtTypeMetadata> ServerParamsMeta { get; } =
    [
        new("WP_RADIUS", MavParamExtType.MavParamExtTypeUint8)
        {
            DefaultValue = new MavParamExtValue((byte)200),
            MinValue = new MavParamExtValue((byte)10),
            MaxValue = new MavParamExtValue((byte)255),
            ShortDesc = "Waypoint radius",
        },

        new("FS_GCS_ENABL", MavParamExtType.MavParamExtTypeInt8)
        {
            DefaultValue = new MavParamExtValue((sbyte)0),
            MinValue = new MavParamExtValue((sbyte)-128),
            MaxValue = new MavParamExtValue((sbyte)127),
            ShortDesc = "Enable failsafe for GCS",
        },

        new("RC_MAP_THROTTLE", MavParamExtType.MavParamExtTypeUint16)
        {
            DefaultValue = new MavParamExtValue((ushort)3),
            MinValue = new MavParamExtValue((ushort)0),
            MaxValue = new MavParamExtValue((ushort)16),
            ShortDesc = "RC throttle channel mapping",
        },

        new("RC1_TRIM", MavParamExtType.MavParamExtTypeInt16)
        {
            DefaultValue = new MavParamExtValue((short)1500),
            MinValue = new MavParamExtValue((short)-32768),
            MaxValue = new MavParamExtValue((short)32767),
            ShortDesc = "RC channel 1 trim",
        },

        new("ARMING_CHECK", MavParamExtType.MavParamExtTypeUint32)
        {
            DefaultValue = new MavParamExtValue((uint)1),
            MinValue = new MavParamExtValue((uint)0),
            MaxValue = new MavParamExtValue(4294967295),
            ShortDesc = "Arming checks flags",
        },

        new("SERVO1_MIN", MavParamExtType.MavParamExtTypeInt32)
        {
            DefaultValue = new MavParamExtValue(1000),
            MinValue = new MavParamExtValue(-2147483648),
            MaxValue = new MavParamExtValue(2147483647),
            ShortDesc = "Servo 1 minimum PWM value",
        },

        new("INS_FAST_SAMPLE", MavParamExtType.MavParamExtTypeUint64)
        {
            DefaultValue = new MavParamExtValue((ulong)0),
            MinValue = new MavParamExtValue((ulong)0),
            MaxValue = new MavParamExtValue(18446744073709551615),
            ShortDesc = "Fast IMU sampling rate",
        },

        new("SCHED_LOOP_RATE", MavParamExtType.MavParamExtTypeInt64)
        {
            DefaultValue = new MavParamExtValue((long)400),
            MinValue = new MavParamExtValue(-9223372036854775808),
            MaxValue = new MavParamExtValue(9223372036854775807),
            ShortDesc = "Loop rate for scheduler",
        },

        new("PITCH_MAX", MavParamExtType.MavParamExtTypeReal32)
        {
            DefaultValue = new MavParamExtValue(20f),
            MinValue = new MavParamExtValue(-90f),
            MaxValue = new MavParamExtValue(90f),
            ShortDesc = "Maximum pitch angle (degrees)",
        },

        new("GND_ABS_PRESS", MavParamExtType.MavParamExtTypeReal64)
        {
            DefaultValue = new MavParamExtValue((double)101325),
            MinValue = new MavParamExtValue((double)0),
            MaxValue = new MavParamExtValue((double)200000),
            ShortDesc = "Ground pressure in Pascals (absolute)",
        },

        new("SERIAL1_PROTOCOL", MavParamExtType.MavParamExtTypeCustom)
        {
            DefaultValue = new MavParamExtValue("test".ToCharArray()),
            ShortDesc = "Serial port 1 protocol",
        }

    ];

    public static List<ParamExtDescription> ClientParamsDescriptions { get; } = ServerParamsMeta
        .Select(pm => new ParamExtDescription
        {
            Name = pm.Name,
            ParamExtType = pm.Type
        })
        .ToList();

    public static void AssertParamsEqual(
        List<ParamExtValuePayload> expectedParams,
        List<ParamExtValuePayload> paramsToCheck)
    {
        for (var i = 0; i < ServerParamsMeta.Count; i++)
        {
            AssertParamsEqual(expectedParams[i], paramsToCheck[i]);
        }
    }
    
    public static void AssertParamsEqual(
        ParamExtValuePayload expectedParam,
        ParamExtValuePayload paramToCheck)
    {
        Assert.Equal(
            MavlinkTypesHelper.GetString(expectedParam.ParamId),
            MavlinkTypesHelper.GetString(paramToCheck.ParamId));
        
        Assert.Equal(
            expectedParam.ParamType, 
            paramToCheck.ParamType);
        
        Assert.Equivalent(
            MavlinkTypesHelper.GetBytes(expectedParam.ParamValue),
            MavlinkTypesHelper.GetBytes(paramToCheck.ParamValue), true);
        
        Assert.Equal(
            expectedParam.ParamIndex, 
            paramToCheck.ParamIndex);
    }
    
    public static void AssertParamsEqual(
        List<ParamExtValuePayload> expectedParams,
        List<ParamExtItem> paramItemsToCheck)
    {
        for (var i = 0; i < ServerParamsMeta.Count; i++)
        {
            Assert.Equal(
                MavlinkTypesHelper.GetString(expectedParams[i].ParamId), 
                paramItemsToCheck[i].Name);
            
            Assert.Equal(
                expectedParams[i].ParamType, 
                paramItemsToCheck[i].Type);
            
            Assert.Equivalent(
                MavlinkTypesHelper.GetBytes(expectedParams[i].ParamValue), 
                (byte[]) paramItemsToCheck[i].Value.Value);
            
            Assert.Equal(
                expectedParams[i].ParamIndex, 
                paramItemsToCheck[i].Index);
        }
    }
}