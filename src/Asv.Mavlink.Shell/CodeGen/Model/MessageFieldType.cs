using System;

namespace Asv.Mavlink.Shell
{
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
            switch (type)
            {
                case MessageFieldType.Int8:
                    return 1;
                case MessageFieldType.Int16:
                    return 2;
                case MessageFieldType.Int32:
                    return 4;
                case MessageFieldType.Int64:
                    return 8;
                case MessageFieldType.Uint8:
                    return 1;
                case MessageFieldType.Uint16:
                    return 2;
                case MessageFieldType.Uint32:
                    return 4;
                case MessageFieldType.Float32:
                    return 4;
                case MessageFieldType.Uint64:
                    return 8;
                case MessageFieldType.Char:
                    return 1;
                case MessageFieldType.Double:
                    return 8;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
