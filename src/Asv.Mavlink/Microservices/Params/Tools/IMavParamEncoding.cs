using System;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface IMavParamEncoding
    {
        float ConvertToMavlinkUnion(MavParamValue value);
        MavParamValue ConvertFromMavlinkUnion(float value, MavParamType type);
    }
    
    public class MavParamCStyleEncoding : IMavParamEncoding
    {
        public float ConvertToMavlinkUnion(MavParamValue value)
        {
            switch (value.Type)
            {
                case MavParamType.MavParamTypeUint8:
                    return (byte)value;
                case MavParamType.MavParamTypeInt8:
                    return (sbyte) value;
                case MavParamType.MavParamTypeUint16:
                    return (ushort) value;
                case MavParamType.MavParamTypeInt16:
                    return (short) value;
                case MavParamType.MavParamTypeUint32:
                    return (uint) value;
                case MavParamType.MavParamTypeInt32:
                    return (int) value;
                case MavParamType.MavParamTypeUint64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeInt64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeReal32:
                    return (float)value;
                case MavParamType.MavParamTypeReal64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                default:
                    throw new ArgumentOutOfRangeException(nameof(value.Type), value.Type, null);
            }
        }

        public MavParamValue ConvertFromMavlinkUnion(float value, MavParamType type)
        {
            switch (type)
            {
                case MavParamType.MavParamTypeUint8:
                    return new MavParamValue((byte)value);
                case MavParamType.MavParamTypeInt8:
                    return new MavParamValue((sbyte) value);
                case MavParamType.MavParamTypeUint16:
                    return new MavParamValue((ushort) value);
                case MavParamType.MavParamTypeInt16:
                    return new MavParamValue((short) value);
                case MavParamType.MavParamTypeUint32:
                    return new MavParamValue((uint) value);
                case MavParamType.MavParamTypeInt32:
                    return new MavParamValue((int) value);
                case MavParamType.MavParamTypeUint64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeInt64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeReal32:
                    return new MavParamValue((float)value);
                case MavParamType.MavParamTypeReal64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
    
    public class MavParamByteWiseEncoding : IMavParamEncoding
    {
        public float ConvertToMavlinkUnion(MavParamValue value)
        {
            byte[] arr;
            switch (value.Type)
            {
                case MavParamType.MavParamTypeUint8:
                    arr = new []{(byte)value};
                    break;
                case MavParamType.MavParamTypeInt8:
                    arr = new []{(byte)value};
                    break;
                case MavParamType.MavParamTypeUint16:
                    arr = BitConverter.GetBytes((ushort)value);
                    break;
                case MavParamType.MavParamTypeInt16:
                    arr = BitConverter.GetBytes((short)value);
                    break;
                case MavParamType.MavParamTypeUint32:
                    arr = BitConverter.GetBytes((uint)value);
                    break;
                case MavParamType.MavParamTypeInt32:
                    arr = BitConverter.GetBytes((int)value);
                    break;
                case MavParamType.MavParamTypeInt64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeReal32:
                    arr = BitConverter.GetBytes((float)value);
                    break;
                case MavParamType.MavParamTypeUint64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeReal64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                default:
                    throw new ArgumentOutOfRangeException(nameof(value.Type), value.Type, null);
            }
            return BitConverter.ToSingle(arr, 0);
        }

        public MavParamValue ConvertFromMavlinkUnion(float value, MavParamType type)
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

            switch (type)
            {
                case MavParamType.MavParamTypeUint8:
                    return BitConverter.GetBytes(value)[0];
                case MavParamType.MavParamTypeInt8:
                    return (sbyte)BitConverter.GetBytes(value)[0];
                case MavParamType.MavParamTypeUint16:
                    return BitConverter.ToUInt16(BitConverter.GetBytes(value), 0);
                case MavParamType.MavParamTypeInt16:
                    return BitConverter.ToInt16(BitConverter.GetBytes(value), 0);
                case MavParamType.MavParamTypeUint32:
                    return BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
                case MavParamType.MavParamTypeInt32:
                    return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
                case MavParamType.MavParamTypeUint64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeInt64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                case MavParamType.MavParamTypeReal32:
                    return value;
                case MavParamType.MavParamTypeReal64:
                    throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}