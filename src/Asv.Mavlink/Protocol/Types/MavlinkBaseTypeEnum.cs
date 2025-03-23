namespace Asv.Mavlink.Types;

public enum MavlinkBaseTypeEnum
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

public readonly struct MavlinkFieldInfo
{
    public MavlinkFieldInfo(string name, string printFormat, string units, string display, string muliplier, string invalidValue, MavlinkBaseTypeEnum baseType, int arraySize, bool isExtended)
    {
        Name = name;
        PrintFormat = printFormat;
        Units = units;
        Display = display;
        Muliplier = muliplier;
        InvalidValue = invalidValue;
        BaseType = baseType;
        ArraySize = arraySize;
        IsExtended = isExtended;
    }
    public string Name { get; }
    public string PrintFormat { get; }
    public string Units { get; }
    public string Display { get; }
    public string Muliplier { get; }
    public string InvalidValue { get;  }
    public MavlinkBaseTypeEnum BaseType { get;  }
    public int ArraySize { get; }
    public bool IsExtended { get; }
}