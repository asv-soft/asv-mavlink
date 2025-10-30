using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Asv.Mavlink;

/// <summary>
///     Source: https://github.com/ArduPilot/ardupilot/blob/master/libraries/AP_Motors/AP_MotorsMatrix.cpp
/// </summary>
public static class ArduQuadPlaneFrameCompability
{
    public static readonly ReadOnlyDictionary<ArduFrameClass, IReadOnlyList<ArduFrameType>> Map = new(
        new Dictionary<ArduFrameClass, IReadOnlyList<ArduFrameType>>
    {
        [ArduFrameClass.Quad] =
        [
            ArduFrameType.Plus,
            ArduFrameType.X,
            ArduFrameType.V,
            ArduFrameType.H,
            ArduFrameType.VTail,
            ArduFrameType.ATail,
            ArduFrameType.PlusRev,
            ArduFrameType.Y4,
            ArduFrameType.NytPlus,
            ArduFrameType.NytX,
            ArduFrameType.BfX,
            ArduFrameType.BfXRev,
            ArduFrameType.DjiX,
            ArduFrameType.CwX,
        ],
        [ArduFrameClass.Hexa] = 
        [
            ArduFrameType.Plus,
            ArduFrameType.X,
            ArduFrameType.H,
            ArduFrameType.DjiX,
            ArduFrameType.CwX,
        ],
        [ArduFrameClass.Octa] = 
        [
            ArduFrameType.Plus,
            ArduFrameType.X,
            ArduFrameType.V,
            ArduFrameType.H,
            ArduFrameType.I,
            ArduFrameType.DjiX,
            ArduFrameType.CwX,
        ],
        [ArduFrameClass.OctaQuad] = 
        [
            ArduFrameType.Plus,
            ArduFrameType.X,
            ArduFrameType.V,
            ArduFrameType.H,
            ArduFrameType.CwX,
            ArduFrameType.BfX,
            ArduFrameType.BfXRev,
        ],
        [ArduFrameClass.Y6] = 
        [
            ArduFrameType.Y6B,
            ArduFrameType.Y6F,
        ],
        [ArduFrameClass.Tri] = [],
        [ArduFrameClass.Tailsitter] = [],
        [ArduFrameClass.DodecaHexa] = 
        [
            ArduFrameType.Plus,
            ArduFrameType.X,
        ],
        [ArduFrameClass.Deca] = 
        [
            ArduFrameType.Plus,
            ArduFrameType.X,
            ArduFrameType.CwX,
        ],
        [ArduFrameClass.ScriptingMatrix] = [],
        [ArduFrameClass.DynamicScriptingMatrix] = [],
        [ArduFrameClass.Undefined] = [],
    });
}