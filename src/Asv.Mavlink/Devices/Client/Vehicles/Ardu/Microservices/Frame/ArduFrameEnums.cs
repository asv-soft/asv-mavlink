namespace Asv.Mavlink;

/// <summary>
///     Source: https://github.com/ArduPilot/ardupilot/blob/master/libraries/AP_Motors/AP_Motors_Class.h#L54
/// </summary>
public enum ArduFrameClass
{
    Undefined = 0,
    Quad = 1,
    Hexa = 2,
    Octa = 3,
    OctaQuad = 4, // X8
    Y6 = 5,
    Heli = 6,
    Tri = 7,
    Single = 8, // singlecopter
    Coax = 9, // coaxcopter
    Tailsitter = 10,
    HeliDual = 11,
    DodecaHexa = 12, // 12-motor
    HeliQuad = 13,
    Deca = 14,
    ScriptingMatrix = 15,
    _6DofScripting = 16,
    DynamicScriptingMatrix = 17,
}

/// <summary>
///     Source: https://github.com/ArduPilot/ardupilot/blob/master/libraries/AP_Motors/AP_Motors_Class.h#L78
/// </summary>
public enum ArduFrameType
{
    Plus = 0,
    X = 1,
    V = 2,
    H = 3,
    VTail = 4,
    ATail = 5,
    PlusRev = 6, // plus with reversed motor direction
    Y6B = 10,
    Y6F = 11, // for FireFlyY6
    BfX = 12, // X frame, betaflight ordering
    DjiX = 13, // X frame, DJI ordering
    CwX = 14, // X frame, clockwise ordering
    I = 15, // (sideways H) octo only
    NytPlus = 16, // plus frame, no differential torque for yaw
    NytX = 17, // X frame, no differential torque for yaw
    BfXRev = 18, // X frame, betaflight ordering, reversed motors
    Y4 = 19, //Y4 Quadrotor frame
}
