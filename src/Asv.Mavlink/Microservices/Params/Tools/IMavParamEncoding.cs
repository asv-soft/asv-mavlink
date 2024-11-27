using System;
using System.Buffers;
using Asv.Mavlink.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    /// <summary>
    /// Interface for encoding and decoding MAVLink parameter values.
    /// </summary>
    public interface IMavParamEncoding
    {
        /// <summary>
        /// Converts a MavParamValue to a float using the Mavlink union.
        /// </summary>
        /// <param name="value">The MavParamValue to convert.</param>
        /// <returns>The converted float value.</returns>
        float ConvertToMavlinkUnion(MavParamValue value);

        /// <summary>
        /// Converts a value of type float to a MavParamValue based on the specified MavParamType.
        /// </summary>
        /// <param name="value">The float value to convert.</param>
        /// <param name="type">The MavParamType indicating the type of the value.</param>
        /// <returns>A MavParamValue representing the converted value.</returns>
        /// <remarks>
        /// This method is used to convert a float value to a MavParamValue, which is a container for different types of data
        /// used in the MAVLink protocol. The MavParamType parameter is used to determine the appropriate conversion logic.
        /// </remarks>
        MavParamValue ConvertFromMavlinkUnion(float value, MavParamType type);
    }

    /// <summary>
    /// Class for encoding and decoding Mavlink parameter values in C-style union format.
    /// </summary>
    public class MavParamCStyleEncoding : IMavParamEncoding
    {
        /// Converts the given MavParamValue to a float value based on its type.
        /// @param value The MavParamValue to convert.
        /// @return The float value converted from the MavParamValue.
        /// @throws ArgumentOutOfRangeException If the MavParamValue type is not valid.
        /// @throws MavlinkException If the MavParamValue type requires more bytes.
        /// /
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

        /// <summary>
        /// Converts a value from the Mavlink union format to a MavParamValue object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="type">The MavParamType of the value.</param>
        /// <returns>A new MavParamValue object representing the converted value.</returns>
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

    /// <summary>
    /// Represents a class that performs byte-wise encoding and decoding of MAVLink parameter values.
    /// </summary>
    public class MavParamByteWiseEncoding : IMavParamEncoding
    {
        /// <summary>
        /// Converts the given MavParamValue to a Mavlink union.
        /// </summary>
        /// <param name="value">The MavParamValue to convert.</param>
        /// <returns>The converted Mavlink union as a float value.</returns>
        public float ConvertToMavlinkUnion(MavParamValue value)
        {
            var arr = ArrayPool<byte>.Shared.Rent(4);
            var span = new Span<byte>(arr, 0, 4);
            var readSpan = new ReadOnlySpan<byte>(arr, 0, 4);
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
                        if (BitConverter.TryWriteBytes(span, (ushort)value) == false)
                            throw new MavlinkException("BitConverter.TryWriteBytes(span, (ushort)value) == false");
                        span[2] = 0;
                        span[3] = 0;
                        break;
                    case MavParamType.MavParamTypeInt16:
                        if (BitConverter.TryWriteBytes(span, (short)value) == false)
                            throw new MavlinkException("BitConverter.TryWriteBytes(span, (short)value) == false");
                        span[2] = 0;
                        span[3] = 0;
                        break;
                    case MavParamType.MavParamTypeUint32:
                        if (BitConverter.TryWriteBytes(span, (uint)value) == false)
                            throw new MavlinkException("BitConverter.TryWriteBytes(span, (uint)value) == false"); 
                        break;
                    case MavParamType.MavParamTypeInt32:
                        if (BitConverter.TryWriteBytes(span, (int)value) == false)
                            throw new MavlinkException("BitConverter.TryWriteBytes(span, (int)value) == false");
                        break;
                    case MavParamType.MavParamTypeReal32:
                        if (BitConverter.TryWriteBytes(span, (float)value) == false)
                            throw new MavlinkException("BitConverter.TryWriteBytes(span, (float)value) == false");
                        break;
                    case MavParamType.MavParamTypeInt64:
                    case MavParamType.MavParamTypeUint64:
                    case MavParamType.MavParamTypeReal64:
                        throw new MavlinkException(RS.Vehicle_ConvertToMavlinkUnionToParamValue_NeedMoreByte);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value.Type), value.Type, null);
                }
                return BitConverter.ToSingle(readSpan);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arr);
            }
            
        }

        /// <summary>
        /// Converts a specified value and type from a MAVLink union to a <see cref="MavParamValue"/> object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="type">The type of the value.</param>
        /// <returns>A new <see cref="MavParamValue"/> object representing the converted value.</returns>
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