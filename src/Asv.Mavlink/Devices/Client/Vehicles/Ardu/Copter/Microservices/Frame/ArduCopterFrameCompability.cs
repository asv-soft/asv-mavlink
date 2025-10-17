using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Asv.Mavlink;

/// <summary>
///     Source: https://github.com/ArduPilot/ardupilot/blob/master/libraries/AP_Motors/AP_MotorsMatrix.cpp
/// </summary>
public static class ArduCopterFrameCompability
{
    public static readonly ReadOnlyDictionary<ArduCopterFrameClass, IReadOnlyList<ArduCopterFrameType>> Map = new(
        new Dictionary<ArduCopterFrameClass, IReadOnlyList<ArduCopterFrameType>>
    {
        [ArduCopterFrameClass.Quad] =
        [
            ArduCopterFrameType.Plus,
            ArduCopterFrameType.X,
            ArduCopterFrameType.V,
            ArduCopterFrameType.H,
            ArduCopterFrameType.VTail,
            ArduCopterFrameType.ATail,
            ArduCopterFrameType.PlusRev,
            ArduCopterFrameType.Y4,
            ArduCopterFrameType.BfX,
            ArduCopterFrameType.BfXRev,
            ArduCopterFrameType.DjiX,
            ArduCopterFrameType.CwX,
        ],
        [ArduCopterFrameClass.Hexa] = 
        [
            ArduCopterFrameType.Plus,
            ArduCopterFrameType.X,
            ArduCopterFrameType.H,
            ArduCopterFrameType.DjiX,
            ArduCopterFrameType.CwX,
        ],
        [ArduCopterFrameClass.Octa] = 
        [
            ArduCopterFrameType.Plus,
            ArduCopterFrameType.X,
            ArduCopterFrameType.V,
            ArduCopterFrameType.H,
            ArduCopterFrameType.I,
            ArduCopterFrameType.DjiX,
            ArduCopterFrameType.CwX,
        ],
        [ArduCopterFrameClass.OctaQuad] = 
        [
            ArduCopterFrameType.Plus,
            ArduCopterFrameType.X,
            ArduCopterFrameType.V,
            ArduCopterFrameType.H,
            ArduCopterFrameType.CwX,
            ArduCopterFrameType.BfX,
            ArduCopterFrameType.BfXRev,
        ],
        [ArduCopterFrameClass.Y6] = 
        [
            ArduCopterFrameType.Y6B,
            ArduCopterFrameType.Y6F,
        ],
        [ArduCopterFrameClass.DodecaHexa] = 
        [
            ArduCopterFrameType.Plus,
            ArduCopterFrameType.X,
        ],
        [ArduCopterFrameClass.Deca] = 
        [
            ArduCopterFrameType.Plus,
            ArduCopterFrameType.X,
            ArduCopterFrameType.CwX,
        ],
        [ArduCopterFrameClass.Tri] = [],
        [ArduCopterFrameClass.Single] = [],
        [ArduCopterFrameClass.Coax] = [],
        [ArduCopterFrameClass.Heli] = [],
        [ArduCopterFrameClass.HeliDual] = [],
        [ArduCopterFrameClass.HeliQuad] = [],
        [ArduCopterFrameClass.Tailsitter] = [],
        [ArduCopterFrameClass._6DofScripting] = [],
        [ArduCopterFrameClass.ScriptingMatrix] = [],
        [ArduCopterFrameClass.DynamicScriptingMatrix] = [],
        [ArduCopterFrameClass.Undefined] = [],
    });
}