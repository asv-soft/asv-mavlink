using System;
using System.Buffers;
using System.Text;
using Asv.IO;
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
        /// Converts a MavParamValue to a float using the Mavlink union.
        /// </summary>
        /// <param name="value">The MavParamExtValue to convert.</param>
        /// <returns>The converted char [] value.</returns>
        char[] ConvertToMavlinkUnion(MavParamExtValue value);

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

        /// <summary>
        /// Converts a value of type char [] to a MavParamExtValue based on the specified MavParamExtType.
        /// </summary>
        /// <param name="value">The MavParamExtValue indicating the type of the value.</param>
        /// <param name="type">The MavParamType indicating the type of the value.</param>
        /// <returns>A MavParamExtValue representing the converted value.</returns>
        MavParamExtValue ConvertFromMavlinkUnion(char[] value, MavParamExtType type); //TODO: переделать
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

        //TODO: fix
        /// <summary>
        /// Converts the given MavParamExtValue to a char array.
        /// </summary>
        /// <param name="value">The MavParamExtValue to convert.</param>
        /// <returns>A char array representing the value in a Mavlink-compatible format.</returns>
        public char[] ConvertToMavlinkUnion(MavParamExtValue value)
        {
            byte[] byteArray = value.Type switch
            {
                MavParamExtType.MavParamExtTypeUint8 => [(byte)value],
                MavParamExtType.MavParamExtTypeInt8 => [(byte)value],
                MavParamExtType.MavParamExtTypeUint16 => BitConverter.GetBytes((ushort)value),
                MavParamExtType.MavParamExtTypeInt16 => BitConverter.GetBytes((short)value),
                MavParamExtType.MavParamExtTypeUint32 => BitConverter.GetBytes((uint)value),
                MavParamExtType.MavParamExtTypeInt32 => BitConverter.GetBytes((int)value),
                MavParamExtType.MavParamExtTypeUint64 => BitConverter.GetBytes((ulong)value),
                MavParamExtType.MavParamExtTypeInt64 => BitConverter.GetBytes((long)value),
                MavParamExtType.MavParamExtTypeReal32 => BitConverter.GetBytes((float)value),
                MavParamExtType.MavParamExtTypeReal64 => BitConverter.GetBytes((double)value),
                MavParamExtType.MavParamExtTypeCustom => value,
                _ => throw new ArgumentOutOfRangeException(nameof(value.Type), value.Type, null)
            };
            return Encoding.ASCII.GetChars(byteArray);
        }

        /// <summary>
        /// Converts a value from a char array in Mavlink format to a MavParamExtValue object.
        /// </summary>
        /// <param name="value">The char array to convert.</param>
        /// <param name="type">The MavParamExtType of the value.</param>
        /// <returns>A new MavParamExtValue object representing the converted value.</returns>
        public MavParamExtValue ConvertFromMavlinkUnion(char[] value, MavParamExtType type)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(value);
            return type switch
            {
                MavParamExtType.MavParamExtTypeUint8 => new MavParamExtValue(byteArray[0]),
                MavParamExtType.MavParamExtTypeInt8 => new MavParamExtValue((sbyte)byteArray[0]),
                MavParamExtType.MavParamExtTypeUint16 => new MavParamExtValue(BitConverter.ToUInt16(byteArray, 0)),
                MavParamExtType.MavParamExtTypeInt16 => new MavParamExtValue(BitConverter.ToInt16(byteArray, 0)),
                MavParamExtType.MavParamExtTypeUint32 => new MavParamExtValue(BitConverter.ToUInt32(byteArray, 0)),
                MavParamExtType.MavParamExtTypeInt32 => new MavParamExtValue(BitConverter.ToInt32(byteArray, 0)),
                MavParamExtType.MavParamExtTypeUint64 => new MavParamExtValue(BitConverter.ToUInt64(byteArray, 0)),
                MavParamExtType.MavParamExtTypeInt64 => new MavParamExtValue(BitConverter.ToInt64(byteArray, 0)),
                MavParamExtType.MavParamExtTypeReal32 => new MavParamExtValue(BitConverter.ToSingle(byteArray, 0)),
                MavParamExtType.MavParamExtTypeReal64 => new MavParamExtValue(BitConverter.ToDouble(byteArray, 0)),
                MavParamExtType.MavParamExtTypeCustom => new MavParamExtValue(value),
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

        //TODO: не правильно работает конвертация из byte[] в char[]
        /// <summary>
        /// Converts the given MavParamValue to a Mavlink union.
        /// </summary>
        /// <param name="value">The MavParamExtValue to convert.</param>
        /// <returns>The converted Mavlink union as a char [] value.</returns>
        public char[] ConvertToMavlinkUnion(MavParamExtValue value)
        {
            var arr = ArrayPool<byte>.Shared.Rent(8);
            var span = new Span<byte>(arr, 0, 8);
            try
            {
                switch (value.Type)
                {
                    case MavParamExtType.MavParamExtTypeUint8:
                        span[0] = (byte)value;
                        span[1] = 0;
                        span[2] = 0;
                        span[3] = 0;
                        span[4] = 0;
                        span[5] = 0;
                        span[6] = 0;
                        span[7] = 0;
                        break;
                    case MavParamExtType.MavParamExtTypeInt8:
                        span[0] = (byte)value;
                        span[1] = 0;
                        span[2] = 0;
                        span[3] = 0;
                        span[4] = 0;
                        span[5] = 0;
                        span[6] = 0;
                        span[7] = 0;
                        break;
                    case MavParamExtType.MavParamExtTypeUint16:
                        if (BitConverter.TryWriteBytes(span, (ushort)value) == false)
                            throw new MavlinkException("Failed to write ushort value to span");
                        span[2] = 0;
                        span[3] = 0;
                        span[4] = 0;
                        span[5] = 0;
                        span[6] = 0;
                        span[7] = 0;
                        break;
                    case MavParamExtType.MavParamExtTypeInt16:
                        if (BitConverter.TryWriteBytes(span, (short)value) == false)
                            throw new MavlinkException("Failed to write short value to span");
                        span[2] = 0;
                        span[3] = 0;
                        span[4] = 0;
                        span[5] = 0;
                        span[6] = 0;
                        span[7] = 0;
                        break;
                    case MavParamExtType.MavParamExtTypeCustom:
                        if (BitConverter.TryWriteBytes(span, value) == false)
                            throw new MavlinkException("Failed to write char value to span");
                        break;
                    case MavParamExtType.MavParamExtTypeUint32:
                        if (BitConverter.TryWriteBytes(span, (uint)value) == false)
                            throw new MavlinkException("Failed to write uint value to span");
                        span[4] = 0;
                        span[5] = 0;
                        span[6] = 0;
                        span[7] = 0;
                        break;
                    case MavParamExtType.MavParamExtTypeInt32:
                        if (BitConverter.TryWriteBytes(span, (int)value) == false)
                            throw new MavlinkException("Failed to write int value to span");
                        span[4] = 0;
                        span[5] = 0;
                        span[6] = 0;
                        span[7] = 0;
                        break;
                    case MavParamExtType.MavParamExtTypeReal32:
                        if (BitConverter.TryWriteBytes(span, (float)value) == false)
                            throw new MavlinkException("Failed to write float value to span");
                        span[4] = 0;
                        span[5] = 0;
                        span[6] = 0;
                        span[7] = 0;
                        break;
                    case MavParamExtType.MavParamExtTypeUint64:
                        if (BitConverter.TryWriteBytes(span, (ulong)value) == false)
                            throw new MavlinkException("Failed to write ulong value to span");
                        break;
                    case MavParamExtType.MavParamExtTypeInt64:
                        if (BitConverter.TryWriteBytes(span, (long)value) == false)
                            throw new MavlinkException("Failed to write long value to span");
                        break;
                    case MavParamExtType.MavParamExtTypeReal64:
                        if (BitConverter.TryWriteBytes(span, (double)value) == false)
                            throw new MavlinkException("Failed to write double value to span");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value.Type), value.Type, "Unsupported type");
                }

                var charsNeeded = Encoding.ASCII.GetCharCount(span);
                var charArr = new char[charsNeeded];
                Encoding.ASCII.GetChars(span, charArr);
                return charArr;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arr);
            }
        }

        /// <summary>
        /// Converts a specified value and type from a MAVLink union to a <see cref="MavParamExtValue"/> object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="type">The type of the value.</param>
        /// <returns>A new <see cref="MavParamExtValue"/> object representing the converted value.</returns>
        public MavParamExtValue ConvertFromMavlinkUnion(char[] value, MavParamExtType type)
        {
            var bytesNeeded = Encoding.ASCII.GetByteCount(value);
            var arr = ArrayPool<byte>.Shared.Rent(bytesNeeded);

            try
            {
                var byteCount = Encoding.ASCII.GetBytes(value, 0, value.Length, arr, 0);
                var span = new ReadOnlySpan<byte>(arr, 0, byteCount);

                return type switch
                {
                    MavParamExtType.MavParamExtTypeUint8 => new MavParamExtValue(span[0]),
                    MavParamExtType.MavParamExtTypeInt8 => new MavParamExtValue((sbyte)span[0]),
                    MavParamExtType.MavParamExtTypeUint16 => new MavParamExtValue(BitConverter.ToUInt16(span)),
                    MavParamExtType.MavParamExtTypeInt16 => new MavParamExtValue(BitConverter.ToInt16(span)),
                    MavParamExtType.MavParamExtTypeUint32 => new MavParamExtValue(BitConverter.ToUInt32(span)),
                    MavParamExtType.MavParamExtTypeInt32 => new MavParamExtValue(BitConverter.ToInt32(span)),
                    MavParamExtType.MavParamExtTypeCustom => new MavParamExtValue(BitConverter.ToChar(span)),
                    MavParamExtType.MavParamExtTypeUint64 => new MavParamExtValue(BitConverter.ToUInt64(span)),
                    MavParamExtType.MavParamExtTypeInt64 => new MavParamExtValue(BitConverter.ToInt64(span)),
                    MavParamExtType.MavParamExtTypeReal32 => new MavParamExtValue(BitConverter.ToSingle(span)),
                    MavParamExtType.MavParamExtTypeReal64 => new MavParamExtValue(BitConverter.ToDouble(span)),
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