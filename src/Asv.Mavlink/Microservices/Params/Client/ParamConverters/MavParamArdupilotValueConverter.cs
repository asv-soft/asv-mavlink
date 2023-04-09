using System;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Client
{
    public class MavParamArdupilotValueConverter : IMavParamValueConverter
    {
        public float ConvertToMavlinkUnionToParamValue(MavParam param)
        {
            switch (param.Type)
            {
                case MavParamType.MavParamTypeUint8:
                    if (!param.IntegerValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Integer_value_not_assigned_for_param, param.Name, param.Type));
                    return (float) param.IntegerValue;
                case MavParamType.MavParamTypeInt8:
                    if (!param.IntegerValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Integer_value_not_assigned_for_param, param.Name, param.Type));
                    return (float)param.IntegerValue;
                case MavParamType.MavParamTypeUint16:
                    if (!param.IntegerValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Integer_value_not_assigned_for_param, param.Name, param.Type));
                    return (float)param.IntegerValue;
                case MavParamType.MavParamTypeInt16:
                    if (!param.IntegerValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Integer_value_not_assigned_for_param, param.Name, param.Type));
                    return (float)param.IntegerValue;
                case MavParamType.MavParamTypeUint32:
                    if (!param.IntegerValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Integer_value_not_assigned_for_param, param.Name, param.Type));
                    return (float)param.IntegerValue;
                case MavParamType.MavParamTypeInt32:
                    if (!param.IntegerValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Integer_value_not_assigned_for_param, param.Name, param.Type));
                    return (float)param.IntegerValue;
                case MavParamType.MavParamTypeUint64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeInt64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeReal32:
                    if (!param.RealValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Real_value_not_assigned_for_param, param.Name, param.Type));
                    return (float)param.RealValue;
                case MavParamType.MavParamTypeReal64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                default:
                    throw new ArgumentOutOfRangeException(nameof(param.Type), param.Type, null);
            }
        }

        public void ConvertFromMavlinkUnionToParamValue(float payloadParamValue, MavParamType payloadParamType, out float? floatVal,
            out long? longVal)
        {
            switch (payloadParamType)
            {
                case MavParamType.MavParamTypeUint8:
                case MavParamType.MavParamTypeInt8:
                case MavParamType.MavParamTypeUint16:
                case MavParamType.MavParamTypeInt16:
                case MavParamType.MavParamTypeUint32:
                case MavParamType.MavParamTypeInt32:
                    longVal = (long?)payloadParamValue;
                    floatVal = null;
                    break;
                case MavParamType.MavParamTypeUint64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeInt64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeReal32:
                    floatVal = payloadParamValue;
                    longVal = null;
                    break;
                case MavParamType.MavParamTypeReal64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                default:
                    throw new ArgumentOutOfRangeException(nameof(payloadParamType), payloadParamType, null);
            }
        }
    }
}