using System;
using Asv.Mavlink.AsvRsga;

namespace Asv.Mavlink;

public interface IMavlinkFieldReader
{
    byte ReadByte(MavlinkFieldInfo field);
    sbyte ReadSByte(MavlinkFieldInfo field);
    short ReadShort(MavlinkFieldInfo field);
    ushort ReadUShort(MavlinkFieldInfo field);
    uint ReadUInt(MavlinkFieldInfo field);
    int ReadInt(MavlinkFieldInfo field);
    long ReadLong(MavlinkFieldInfo field);
    ulong ReadULong(MavlinkFieldInfo field);
    float ReadFloat(MavlinkFieldInfo field);
    double ReadDouble(MavlinkFieldInfo field);
    
    void ReadByteArray(MavlinkFieldInfo field,Span<byte> value);
    void ReadSByteArray(MavlinkFieldInfo field,Span<sbyte> value);
    void ReadCharArray(MavlinkFieldInfo field,Span<char> value);
    void ReadUShortArray(MavlinkFieldInfo field,Span<ushort> value);
    void ReadShortArray(MavlinkFieldInfo field,Span<short> value);
    void ReadFloatArray(MavlinkFieldInfo field,Span<float> value);
    void ReadDoubleArray(MavlinkFieldInfo field,Span<double> value);
    void ReadUIntArray(MavlinkFieldInfo field,Span<uint> value);
    void ReadIntArray(MavlinkFieldInfo field,Span<int> value);

    void ReadULongArray(MavlinkFieldInfo field, ulong[] value);
    void ReadLongArray(MavlinkFieldInfo field, long[] value);
    char ReadChar(MavlinkFieldInfo field);
    object ReadEnum(MavlinkFieldInfo staticField);
    Array ReadEnumArray(MavlinkFieldInfo staticField);
}