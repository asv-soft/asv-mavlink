using System.Collections.Generic;

namespace Asv.Mavlink;

public sealed class ArduMotorFrame : IMotorFrame
{
    public string Id { get; }
    public ArduFrameClass MotorFrameClass { get; }
    public ArduFrameType? MotorFrameType { get; }
    public IReadOnlyDictionary<string, string>? Meta { get; }
    
    internal ArduMotorFrame(
        ArduFrameClass motorFrameClass, 
        ArduFrameType? motorFrameType, 
        IReadOnlyDictionary<string, string>? meta = null)
    {
        MotorFrameClass = motorFrameClass;
        MotorFrameType = motorFrameType;
        Id = GenerateId(motorFrameClass, motorFrameType);
        Meta = meta;
    }

    public static string GenerateId(
        ArduFrameClass motorFrameClass,
        ArduFrameType? motorFrameType)
    {
        return motorFrameType is null
            ? $"{motorFrameClass} ({(int)motorFrameClass})"
            : $"{motorFrameClass} ({(int)motorFrameClass}) / {motorFrameType} ({(int)motorFrameType})";
    }
}