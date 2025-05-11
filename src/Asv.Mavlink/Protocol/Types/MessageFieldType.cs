using System;
using Newtonsoft.Json;

namespace Asv.Mavlink;

public interface IMavlinkFieldWriter
{
    void Write(MavlinkFieldInfo field, object value);
}

public interface IMavlinkFieldReader
{
    byte ReadByte(MavlinkFieldInfo staticField);
    sbyte ReadSByte(MavlinkFieldInfo staticField);
    short ReadShort(MavlinkFieldInfo staticField);
    ushort ReadUShort(MavlinkFieldInfo field);
    object ReadEnum(MavlinkFieldInfo staticField);
    uint ReadUInt(MavlinkFieldInfo staticField);
    int ReadInt(MavlinkFieldInfo staticField);
    long ReadLong(MavlinkFieldInfo staticField);
    ulong ReadULong(MavlinkFieldInfo staticField);
    float ReadFloat(MavlinkFieldInfo staticField);
    double ReadDouble(MavlinkFieldInfo staticField);
    
    void ReadByteArray(MavlinkFieldInfo staticField,Span<byte> payloadArU8);
    void ReadSByteArray(MavlinkFieldInfo staticField,Span<sbyte> payloadArI8);
    void ReadCharArray(MavlinkFieldInfo staticField,Span<char> payloadArC);
    void ReadUShortArray(MavlinkFieldInfo staticField,Span<ushort> payloadArU16);
    void ReadShortArray(MavlinkFieldInfo staticField,Span<short> payloadArI16);
    void ReadFloatArray(MavlinkFieldInfo staticField,Span<float> payloadArF);
    void ReadDoubleArray(MavlinkFieldInfo staticField,Span<double> payloadArD);
    void ReadUIntArray(MavlinkFieldInfo staticField,Span<uint> payloadArU32);
    void ReadIntArray(MavlinkFieldInfo staticField,Span<int> payloadArI32);

    void ReadULongArray(MavlinkFieldInfo staticField, ulong[] payloadU64Array);
    void ReadLongArray(MavlinkFieldInfo staticField, long[] payloadS64Array);
    char ReadChar(MavlinkFieldInfo staticField);
    Array ReadEnumArray(MavlinkFieldInfo staticField);
}


public enum MessageFieldType
{
    Int8 = 0,
    Int16,
    Int32,
    Int64,
    Uint8,
    Uint16,
    Uint32,
    Float32,
    Uint64,
    Char,
    Double,
}

public static class MessageFieldTypeExt
{
    public static int GetFieldTypeByteSize(this MessageFieldType type)
    {
        return type switch
        {
            MessageFieldType.Int8 => 1,
            MessageFieldType.Int16 => 2,
            MessageFieldType.Int32 => 4,
            MessageFieldType.Int64 => 8,
            MessageFieldType.Uint8 => 1,
            MessageFieldType.Uint16 => 2,
            MessageFieldType.Uint32 => 4,
            MessageFieldType.Float32 => 4,
            MessageFieldType.Uint64 => 8,
            MessageFieldType.Char => 1,
            MessageFieldType.Double => 8,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
