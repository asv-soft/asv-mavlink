using System.Collections.Generic;

namespace Asv.Mavlink;

public sealed class ArduDroneFrame : IDroneFrame
{
    public string Id { get; }
    public ArduFrameClass FrameClass { get; }
    public ArduFrameType? FrameType { get; }
    public IReadOnlyDictionary<string, string>? Meta { get; }
    
    internal ArduDroneFrame(
        ArduFrameClass frameClass, 
        ArduFrameType? frameType, 
        IReadOnlyDictionary<string, string>? meta = null)
    {
        FrameClass = frameClass;
        FrameType = frameType;
        Id = GenerateId(frameClass, frameType);
        Meta = meta;
    }

    public static string GenerateId(ArduFrameClass frameClass, ArduFrameType? frameType)
    {
        return frameType is null
            ? $"{frameClass} ({(int)frameClass})"
            : $"{frameClass} ({(int)frameClass}) / {frameType} ({(int)frameType})";
    }
}