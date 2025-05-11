using System;

namespace Asv.Mavlink;



public readonly struct MavlinkFieldInfo(
    int index,
    string name,
    string description,
    string printFormat,
    string units,
    string display,
    string invalidValue,
    MessageFieldType type,
    int arraySize,
    bool isExtended)
{
    public int Index => index;
    public string Name => name;
    public string Description => description;
    public string PrintFormat => printFormat;
    public string Units => units;
    public string Display => display;
    public string InvalidValue => invalidValue;
    public MessageFieldType BaseType => type;
    public int ArraySize => arraySize;
    public bool IsExtended => isExtended;
}