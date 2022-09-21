using System;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink.Client
{
    public class MavParamValueConverter : IMavParamValueConverter
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public float ConvertToMavlinkUnionToParamValue(MavParam param)
        {
            byte[] arr;
            switch (param.Type)
            {
                case MavParamType.MavParamTypeUint8:
                    if (!param.IntegerValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Integer_value_not_assigned_for_param, param.Name, param.Type));
                    arr = BitConverter.GetBytes((byte)param.IntegerValue);

                    break;
                case MavParamType.MavParamTypeInt8:
                    if (!param.IntegerValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Integer_value_not_assigned_for_param, param.Name, param.Type));
                    arr = BitConverter.GetBytes((sbyte)param.IntegerValue);
                    break;
                case MavParamType.MavParamTypeUint16:
                    if (!param.IntegerValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Integer_value_not_assigned_for_param, param.Name, param.Type));
                    arr = BitConverter.GetBytes((ushort)param.IntegerValue);
                    break;
                case MavParamType.MavParamTypeInt16:
                    if (!param.IntegerValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Integer_value_not_assigned_for_param, param.Name, param.Type));
                    arr = BitConverter.GetBytes((short)param.IntegerValue);
                    break;
                case MavParamType.MavParamTypeUint32:
                    if (!param.IntegerValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Integer_value_not_assigned_for_param, param.Name, param.Type));
                    arr = BitConverter.GetBytes((UInt32)param.IntegerValue);
                    break;
                case MavParamType.MavParamTypeInt32:
                    if (!param.IntegerValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Integer_value_not_assigned_for_param, param.Name, param.Type));
                    arr = BitConverter.GetBytes((Int32)param.IntegerValue);
                    break;
                
                case MavParamType.MavParamTypeInt64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeReal32:
                    if (!param.RealValue.HasValue)
                        throw new Exception(string.Format(RS.Vehicle_ConvertToMavlinkUnionToParamValue_Real_value_not_assigned_for_param, param.Name, param.Type));
                    arr = BitConverter.GetBytes((float)param.RealValue);
                    break;
                case MavParamType.MavParamTypeUint64:
                    _logger.Warn($"Error param type {param}");
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeReal64:
                    _logger.Warn($"Error param type {param}");
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                default:
                    _logger.Warn($"Unknown param type {param}");
                    return Single.NaN;
            }
            Array.Resize(ref arr, 4);
            return BitConverter.ToSingle(arr, 0);
        }

        public void ConvertFromMavlinkUnionToParamValue(float payloadParamValue, MavParamType payloadParamType, out float? floatVal, out long? longVal)
        {

            // MAVLink (v1.0, v2.0) supports these data types:
            // uint32_t - 32bit unsigned integer(use the ENUM value MAV_PARAM_TYPE_UINT32)
            // int32_t - 32bit signed integer(use the ENUM value MAV_PARAM_TYPE_INT32)
            // float - IEEE754 single precision floating point number(use the ENUM value MAV_PARAM_TYPE_FLOAT)
            // All parameters are send as the float value of mavlink_param_union_t, which means that a parameter 
            // should be byte-wise converted with this union to a byte-wise float (no type conversion). 
            // This is necessary in order to not limit the maximum precision for scaled integer params. 
            // E.g. GPS coordinates can only be expressed with single float precision up to a few meters, while GPS coordinates in 1E7 scaled integers 
            // provide very high accuracy.

            switch (payloadParamType)
            {
                case MavParamType.MavParamTypeUint8:
                    longVal = BitConverter.GetBytes(payloadParamValue)[0];
                    floatVal = null;
                    break;
                case MavParamType.MavParamTypeInt8:
                    longVal = (sbyte)BitConverter.GetBytes(payloadParamValue)[0];
                    floatVal = null;
                    break;
                case MavParamType.MavParamTypeUint16:
                    longVal = BitConverter.ToUInt16(BitConverter.GetBytes(payloadParamValue), 0);
                    floatVal = null;
                    break;
                case MavParamType.MavParamTypeInt16:
                    longVal = BitConverter.ToInt16(BitConverter.GetBytes(payloadParamValue), 0);
                    floatVal = null;
                    break;
                case MavParamType.MavParamTypeUint32:
                    longVal = BitConverter.ToUInt32(BitConverter.GetBytes(payloadParamValue), 0);
                    floatVal = null;
                    break;
                case MavParamType.MavParamTypeInt32:
                    longVal = BitConverter.ToInt32(BitConverter.GetBytes(payloadParamValue), 0);
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
                    _logger.Warn($"Unknown param type {payloadParamType} with value {payloadParamValue}");
                    floatVal = null;
                    longVal = null;
                    return;
            }
        }
    }
}