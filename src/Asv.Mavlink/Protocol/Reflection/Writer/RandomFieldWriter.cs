using System;
using DotNext;

namespace Asv.Mavlink;

public class RandomFieldWriter(Random random) : IMavlinkFieldReader
{
    public byte ReadByte(MavlinkFieldInfo field) => (byte)random.Next(byte.MinValue, byte.MaxValue);

    public sbyte ReadSByte(MavlinkFieldInfo field) => (sbyte)random.Next(0, sbyte.MaxValue); // TODO: this is bugged, should be sbyte.MinValue

    public short ReadShort(MavlinkFieldInfo field) => (short)random.Next(short.MinValue, short.MaxValue);

    public ushort ReadUShort(MavlinkFieldInfo field) => (ushort)random.Next(ushort.MinValue, ushort.MaxValue);

    public uint ReadUInt(MavlinkFieldInfo field) => random.Next<uint>();

    public int ReadInt(MavlinkFieldInfo field) => random.Next(int.MinValue, int.MaxValue);

    public long ReadLong(MavlinkFieldInfo field) => random.NextInt64(long.MinValue, long.MaxValue);

    public ulong ReadULong(MavlinkFieldInfo field) => random.Next<ulong>();

    public float ReadFloat(MavlinkFieldInfo field) => (float)random.NextDouble();

    public double ReadDouble(MavlinkFieldInfo field) => random.NextDouble();

    public void ReadByteArray(MavlinkFieldInfo field, Span<byte> value) => random.NextBytes(value);

    public void ReadSByteArray(MavlinkFieldInfo field, Span<sbyte> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = ReadSByte(field);
        }
    }

    public void ReadCharArray(MavlinkFieldInfo field, Span<char> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = (char)ReadChar(field);
        }
            
    }

    public void ReadUShortArray(MavlinkFieldInfo field, Span<ushort> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = ReadUShort(field);
        }
            
    }

    public void ReadShortArray(MavlinkFieldInfo field, Span<short> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = ReadShort(field);
        }
            
    }

    public void ReadFloatArray(MavlinkFieldInfo field, Span<float> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = ReadFloat(field);
        }
            
    }

    public void ReadDoubleArray(MavlinkFieldInfo field, Span<double> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = ReadDouble(field);
        }
            
    }

    public void ReadUIntArray(MavlinkFieldInfo field, Span<uint> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = ReadUInt(field);
        }
            
    }

    public void ReadIntArray(MavlinkFieldInfo field, Span<int> value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = ReadInt(field);
        }
            
    }

    public void ReadULongArray(MavlinkFieldInfo field, ulong[] value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = ReadULong(field);
        }
            
    }

    public void ReadLongArray(MavlinkFieldInfo field, long[] value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = ReadLong(field);
        }
            
    }

    public char ReadChar(MavlinkFieldInfo field)
    {
        const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return validChars[random.Next() % validChars.Length];
            
    }

    public object ReadEnum(MavlinkFieldInfo staticField)
    {
        throw new NotImplementedException();
    }

    public Array ReadEnumArray(MavlinkFieldInfo staticField)
    {
        throw new NotImplementedException();
    }
}