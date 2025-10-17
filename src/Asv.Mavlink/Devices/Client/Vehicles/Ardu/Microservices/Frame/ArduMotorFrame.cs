using System.Collections.Generic;

namespace Asv.Mavlink;

public sealed class ArduMotorFrame : IMotorFrame
{
    public string Id { get; }
    public ArduCopterFrameClass MotorFrameClass { get; }
    public ArduCopterFrameType? MotorFrameType { get; }
    public IReadOnlyDictionary<string, string>? Meta { get; }
    
    internal ArduMotorFrame(
        ArduCopterFrameClass motorFrameClass, 
        ArduCopterFrameType? motorFrameType, 
        IReadOnlyDictionary<string, string>? meta = null)
    {
        MotorFrameClass = motorFrameClass;
        MotorFrameType = motorFrameType;
        Id = GenerateId(motorFrameClass, motorFrameType);
        Meta = meta;
    }

    public static string GenerateId(
        ArduCopterFrameClass motorFrameClass,
        ArduCopterFrameType? motorFrameType)
    {
        return motorFrameType is null
            ? $"{motorFrameClass} ({(int)motorFrameClass})"
            : $"{motorFrameClass} ({(int)motorFrameClass}) / {motorFrameType} ({(int)motorFrameType})";
    }
}