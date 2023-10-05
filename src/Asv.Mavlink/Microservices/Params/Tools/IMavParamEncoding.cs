using System;
using System.Buffers;
using System.Diagnostics;
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
            return value.Type switch
            {
                MavParamType.MavParamTypeUint8 => (byte)value,
                MavParamType.MavParamTypeInt8 => (sbyte)value,
                MavParamType.MavParamTypeUint16 => (ushort)value,
                MavParamType.MavParamTypeInt16 => (short)value,
                MavParamType.MavParamTypeUint32 => (uint)value,
                MavParamType.MavParamTypeInt32 => (int)value,
                MavParamType.MavParamTypeUint64 => throw new MavlinkException(
                    RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte),
                MavParamType.MavParamTypeInt64 => throw new MavlinkException(
                    RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte),
                MavParamType.MavParamTypeReal32 => (float)value,
                MavParamType.MavParamTypeReal64 => throw new MavlinkException(
                    RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte),
                _ => throw new ArgumentOutOfRangeException(nameof(value.Type), value.Type, null)
            };
        }

        public MavParamValue ConvertFromMavlinkUnion(float value, MavParamType type)
        {
            return type switch
            {
                MavParamType.MavParamTypeUint8 => new MavParamValue((byte)value),
                MavParamType.MavParamTypeInt8 => new MavParamValue((sbyte)value),
                MavParamType.MavParamTypeUint16 => new MavParamValue((ushort)value),
                MavParamType.MavParamTypeInt16 => new MavParamValue((short)value),
                MavParamType.MavParamTypeUint32 => new MavParamValue((uint)value),
                MavParamType.MavParamTypeInt32 => new MavParamValue((int)value),
                MavParamType.MavParamTypeUint64 => throw new MavlinkException(
                    RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte),
                MavParamType.MavParamTypeInt64 => throw new MavlinkException(
                    RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte),
                MavParamType.MavParamTypeReal32 => new MavParamValue((float)value),
                MavParamType.MavParamTypeReal64 => throw new MavlinkException(
                    RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
    
    public class MavParamByteWiseEncoding : IMavParamEncoding
    {
        public float ConvertToMavlinkUnion(MavParamValue value)
        {
            var arr = ArrayPool<byte>.Shared.Rent(4);
            var span = new Span<byte>(arr, 0, 4);
            try
            {
                switch (value.Type)
                {
                    case MavParamType.MavParamTypeUint8:
                        span[0] = (byte)value;
                        span[1] = 0;
                        span[2] = 0;
                        span[3] = 0;
                        break;
                    case MavParamType.MavParamTypeInt8:
                        span[0] = (byte)value;
                        span[1] = 0;
                        span[2] = 0;
                        span[3] = 0;
                        break;
                    case MavParamType.MavParamTypeUint16:
                        Debug.Assert(BitConverter.TryWriteBytes(span, (ushort)value),
                            "BitConverter.TryWriteBytes(span, (ushort)value) == false");
                        span[2] = 0;
                        span[3] = 0;
                        break;
                    case MavParamType.MavParamTypeInt16:
                        Debug.Assert(BitConverter.TryWriteBytes(span, (short)value),
                            "BitConverter.TryWriteBytes(span, (short)value) == false");
                        span[2] = 0;
                        span[3] = 0;
                        break;
                    case MavParamType.MavParamTypeUint32:
                        Debug.Assert(BitConverter.TryWriteBytes(span, (uint)value),
                            "BitConverter.TryWriteBytes(span, (uint)value) == false");
                        break;
                    case MavParamType.MavParamTypeInt32:
                        Debug.Assert(BitConverter.TryWriteBytes(span, (int)value),
                            "BitConverter.TryWriteBytes(span, (int)value) == false");
                        break;
                    case MavParamType.MavParamTypeReal32:
                        Debug.Assert(BitConverter.TryWriteBytes(span, (float)value),
                            "BitConverter.TryWriteBytes(span, (float)value) == false");
                        break;
                    case MavParamType.MavParamTypeInt64:
                    case MavParamType.MavParamTypeUint64:
                    case MavParamType.MavParamTypeReal64:
                        throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value.Type), value.Type, null);
                }
                return BitConverter.ToSingle(span);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arr);
            }
            
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

            var arr = ArrayPool<byte>.Shared.Rent(4);
            
            
            try
            {
                var writeSpan = new Span<byte>(arr, 0, 4);
                var span = new ReadOnlySpan<byte>(arr, 0, 4);
                BitConverter.TryWriteBytes(writeSpan, value);
                return type switch
                {
                    MavParamType.MavParamTypeUint8 => new MavParamValue(span[0]),
                    MavParamType.MavParamTypeInt8 => new MavParamValue((sbyte)span[0]),
                    MavParamType.MavParamTypeUint16 => new MavParamValue(BitConverter.ToUInt16(span)),
                    MavParamType.MavParamTypeInt16 => new MavParamValue(BitConverter.ToInt16(span)),
                    MavParamType.MavParamTypeUint32 => new MavParamValue(BitConverter.ToUInt32(span)),
                    MavParamType.MavParamTypeInt32 => new MavParamValue(BitConverter.ToInt32(span)),
                    MavParamType.MavParamTypeReal32 => new MavParamValue((float)value),
                    MavParamType.MavParamTypeUint64 => throw new MavlinkException(
                        RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte),
                    MavParamType.MavParamTypeInt64 => throw new MavlinkException(
                        RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte),
                    MavParamType.MavParamTypeReal64 => throw new MavlinkException(
                        RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte),
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arr);
            }
            
        }
    }
}