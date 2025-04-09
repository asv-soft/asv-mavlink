using System;

namespace Asv.Mavlink;



public readonly struct MavlinkFieldInfo
{
    public MavlinkFieldInfo(string name, string printFormat, string units, string display, string muliplier, string invalidValue, MessageFieldType baseType, int arraySize, bool isExtended)
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
    public MessageFieldType BaseType { get;  }
    public int ArraySize { get; }
    public bool IsExtended { get; }
}