using System;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public class MavParamArdupilotValueConverter : IMavParamValueConverter
    {
        public float ConvertToMavlinkUnionToParamValue(decimal value, MavParamType type)
        {
            switch (type)
            {
                case MavParamType.MavParamTypeUint8:
                    return (float) value;
                case MavParamType.MavParamTypeInt8:
                    return (float) value;
                case MavParamType.MavParamTypeUint16:
                    return (float) value;
                case MavParamType.MavParamTypeInt16:
                    return (float) value;
                case MavParamType.MavParamTypeUint32:
                    return (float) value;
                case MavParamType.MavParamTypeInt32:
                    return (float) value;
                case MavParamType.MavParamTypeUint64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeInt64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeReal32:
                    return (float) value;
                case MavParamType.MavParamTypeReal64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public decimal ConvertFromMavlinkUnionToParamValue(float value, MavParamType type)
        {
            switch (type)
            {
                case MavParamType.MavParamTypeUint8:
                case MavParamType.MavParamTypeInt8:
                case MavParamType.MavParamTypeUint16:
                case MavParamType.MavParamTypeInt16:
                case MavParamType.MavParamTypeUint32:
                case MavParamType.MavParamTypeInt32:
                    return (decimal)value;
                case MavParamType.MavParamTypeUint64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeInt64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeReal32:
                    return (decimal)value;
                case MavParamType.MavParamTypeReal64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}