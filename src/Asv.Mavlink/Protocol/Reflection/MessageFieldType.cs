using System;
using DotNext;

namespace Asv.Mavlink;

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
    public static T Randomize<T>(this T src, Random random)
        where T : MavlinkMessage
    {
        /*src.WriteFields(new RandomFieldWriter(random));
        src.SystemId = random.Next<byte>();
        src.ComponentId = random.Next<byte>();
        src.Sequence = random.Next<byte>();
        return src;*/
        return src;
    }
    
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