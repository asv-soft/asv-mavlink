namespace Asv.Mavlink.Shell;

public class MessageFieldModel: MavlinkModelBase
{
    public MessageFieldType Type { get; set; }
    public int FieldTypeByteSize { get; set; }
    public int FieldByteSize => IsArray ? FieldTypeByteSize * ArrayLength : FieldTypeByteSize;
    public string? Name { get; set; }
    public string? Units { get; set; }
    public byte ArrayLength { get; set; }
    public bool IsArray { get; set; }
    public bool IsExtended { get; set; }
    public string? TypeName { get; set; }
    public string? Enum { get; set; }
    public bool IsTheLargestArrayInMessage { get; set; }
    public string? Display { get; set; }
    public string? PrintFormat { get; set; }
    public string? Invalid { get; set; }
}