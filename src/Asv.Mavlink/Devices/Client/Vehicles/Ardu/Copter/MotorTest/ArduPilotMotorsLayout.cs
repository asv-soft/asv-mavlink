// This file is generated from https://github.com/ArduPilot/MissionPlanner/blob/master/APMotorLayout.json"

namespace Asv.Mavlink;

internal class Layout
{
    public int Class { get; init; }
    public required string ClassName { get; init; }
    public int Type { get; init; }
    public required string TypeName { get; init; }
    public Motor[] Motors { get; init; } = [];
}

internal class Motor
{
    public int Number { get; init; }
    public int TestOrder { get; init; }
    public required string Rotation { get; init; }
    public float Roll { get; init; }
    public float Pitch { get; init; }
}

internal static class ArduPilotMotorsLayout
{
    public const string Version = "AP_Motors library test ver 1.2";

    public static readonly Layout[] Layouts = new[]
    {
        new Layout
        {
            Class = 1,
            ClassName = "QUAD",
            Type = 0,
            TypeName = "PLUS",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 1,
            ClassName = "QUAD",
            Type = 1,
            TypeName = "X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 1,
            ClassName = "QUAD",
            Type = 2,
            TypeName = "V",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 1,
            ClassName = "QUAD",
            Type = 3,
            TypeName = "H",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 1,
            ClassName = "QUAD",
            Type = 4,
            TypeName = "VTAIL",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -0.5f,
                    Pitch = 0.266f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 0.5f,
                    Pitch = 0.266f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 1,
            ClassName = "QUAD",
            Type = 5,
            TypeName = "ATAIL",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -0.5f,
                    Pitch = 0.266f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = -0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 0.5f,
                    Pitch = 0.266f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 1,
            ClassName = "QUAD",
            Type = 6,
            TypeName = "PLUSREV",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 1,
            ClassName = "QUAD",
            Type = 12,
            TypeName = "BF_X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
            },
        },
        new Layout
        {
            Class = 1,
            ClassName = "QUAD",
            Type = 13,
            TypeName = "DJI_X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 1,
            ClassName = "QUAD",
            Type = 14,
            TypeName = "CW_X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
            },
        },
        new Layout
        {
            Class = 1,
            ClassName = "QUAD",
            Type = 16,
            TypeName = "NYT_PLUS",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = -0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -0f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 1,
            ClassName = "QUAD",
            Type = 17,
            TypeName = "NYT_X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 1,
            ClassName = "QUAD",
            Type = 18,
            TypeName = "X_REV",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
            },
        },
        new Layout
        {
            Class = 2,
            ClassName = "HEXA",
            Type = 0,
            TypeName = "PLUS",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.25f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.25f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.25f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.25f,
                },
            },
        },
        new Layout
        {
            Class = 2,
            ClassName = "HEXA",
            Type = 1,
            TypeName = "X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0.25f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = -0.25f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.25f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0.25f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 2,
            ClassName = "HEXA",
            Type = 3,
            TypeName = "H",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 2,
            ClassName = "HEXA",
            Type = 13,
            TypeName = "DJI_X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.25f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0.25f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0.25f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = -0.25f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0f,
                },
            },
        },
        new Layout
        {
            Class = 2,
            ClassName = "HEXA",
            Type = 14,
            TypeName = "CW_X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.25f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = -0.25f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0.25f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0.25f,
                    Pitch = 0.5f,
                },
            },
        },
        new Layout
        {
            Class = 3,
            ClassName = "OCTA",
            Type = 0,
            TypeName = "PLUS",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.3535f,
                    Pitch = 0.3535f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = -0.3535f,
                    Pitch = -0.3535f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 8,
                    Rotation = "CCW",
                    Roll = 0.3535f,
                    Pitch = 0.3535f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.3535f,
                    Pitch = -0.3535f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 7,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0f,
                },
            },
        },
        new Layout
        {
            Class = 3,
            ClassName = "OCTA",
            Type = 1,
            TypeName = "X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.2071f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.2071f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2071f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = -0.2071f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 8,
                    Rotation = "CCW",
                    Roll = 0.2071f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.2071f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 7,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2071f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.2071f,
                },
            },
        },
        new Layout
        {
            Class = 3,
            ClassName = "OCTA",
            Type = 2,
            TypeName = "V",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 7,
                    Rotation = "CW",
                    Roll = 0.415f,
                    Pitch = 0.17f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0.335f,
                    Pitch = -0.16f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.335f,
                    Pitch = -0.16f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = -0.25f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 8,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.415f,
                    Pitch = 0.17f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.25f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 3,
            ClassName = "OCTA",
            Type = 3,
            TypeName = "H",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.1665f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 8,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.1665f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 7,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.1665f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.1665f,
                },
            },
        },
        new Layout
        {
            Class = 3,
            ClassName = "OCTA",
            Type = 13,
            TypeName = "DJI_X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.2071f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 8,
                    Rotation = "CW",
                    Roll = 0.2071f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 7,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2071f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.2071f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = 0.2071f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = -0.2071f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0.2071f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2071f,
                },
            },
        },
        new Layout
        {
            Class = 3,
            ClassName = "OCTA",
            Type = 14,
            TypeName = "CW_X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.2071f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2071f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0.2071f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = -0.2071f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = 0.2071f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.2071f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 7,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2071f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 8,
                    Rotation = "CW",
                    Roll = 0.2071f,
                    Pitch = 0.5f,
                },
            },
        },
        new Layout
        {
            Class = 3,
            ClassName = "OCTA",
            Type = 15,
            TypeName = "I",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.1665f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.1665f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 8,
                    Rotation = "CCW",
                    Roll = 0.1665f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = -0.1665f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 7,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
            },
        },
        new Layout
        {
            Class = 4,
            ClassName = "OCTAQUAD",
            Type = 0,
            TypeName = "PLUS",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 7,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 8,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 4,
            ClassName = "OCTAQUAD",
            Type = 1,
            TypeName = "X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 7,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 8,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 4,
            ClassName = "OCTAQUAD",
            Type = 2,
            TypeName = "V",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 7,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 8,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 4,
            ClassName = "OCTAQUAD",
            Type = 3,
            TypeName = "H",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 7,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 8,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 4,
            ClassName = "OCTAQUAD",
            Type = 12,
            TypeName = "BF_X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 7,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 8,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
            },
        },
        new Layout
        {
            Class = 4,
            ClassName = "OCTAQUAD",
            Type = 14,
            TypeName = "CW_X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 7,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 8,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
            },
        },
        new Layout
        {
            Class = 4,
            ClassName = "OCTAQUAD",
            Type = 18,
            TypeName = "X_REV",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 7,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 8,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 0,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 1,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 2,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 3,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 4,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 5,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 6,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 7,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 8,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 9,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 10,
            TypeName = "Y6B",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.25f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.25f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.25f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.25f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 11,
            TypeName = "Y6F",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.25f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.25f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.25f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.25f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 12,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 13,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 14,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 15,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 16,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 17,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 5,
            ClassName = "Y6",
            Type = 18,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 2,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 5,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 6,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 1,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.2498f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 0,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 1,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 2,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 3,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 4,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 5,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 6,
            TypeName = "pitch-reversed",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 7,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 8,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 9,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 10,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 11,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 12,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 13,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 14,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 15,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 16,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 17,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 7,
            ClassName = "TRI",
            Type = 18,
            TypeName = "default",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "?",
                    Roll = -1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 4,
                    Rotation = "?",
                    Roll = 1f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 2,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = -1f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 3,
                    Rotation = "?",
                    Roll = 0f,
                    Pitch = 0f,
                },
            },
        },
        new Layout
        {
            Class = 12,
            ClassName = "DODECAHEXA",
            Type = 0,
            TypeName = "PLUS",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = 0.25f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.25f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0.25f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.25f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 7,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 8,
                    Rotation = "CCW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 9,
                    TestOrder = 9,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0.25f,
                },
                new Motor
                {
                    Number = 10,
                    TestOrder = 10,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.25f,
                },
                new Motor
                {
                    Number = 11,
                    TestOrder = 11,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = 0.25f,
                },
                new Motor
                {
                    Number = 12,
                    TestOrder = 12,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.25f,
                },
            },
        },
        new Layout
        {
            Class = 12,
            ClassName = "DODECAHEXA",
            Type = 1,
            TypeName = "X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.25f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.25f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 3,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = -0.25f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = -0.25f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 7,
                    Rotation = "CW",
                    Roll = 0.25f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 8,
                    Rotation = "CCW",
                    Roll = 0.25f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 9,
                    TestOrder = 9,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 10,
                    TestOrder = 10,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 11,
                    TestOrder = 11,
                    Rotation = "CW",
                    Roll = 0.25f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 12,
                    TestOrder = 12,
                    Rotation = "CCW",
                    Roll = 0.25f,
                    Pitch = 0.5f,
                },
            },
        },
        new Layout
        {
            Class = 14,
            ClassName = "DECA",
            Type = 0,
            TypeName = "PLUS",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.309f,
                    Pitch = 0.4045f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = 0.1545f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = -0.5f,
                    Pitch = -0.1545f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = -0.309f,
                    Pitch = -0.4045f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 7,
                    Rotation = "CCW",
                    Roll = 0.309f,
                    Pitch = -0.4045f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 8,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0.1545f,
                },
                new Motor
                {
                    Number = 9,
                    TestOrder = 9,
                    Rotation = "CCW",
                    Roll = 0.5f,
                    Pitch = 0.1545f,
                },
                new Motor
                {
                    Number = 10,
                    TestOrder = 10,
                    Rotation = "CW",
                    Roll = 0.309f,
                    Pitch = 0.4045f,
                },
            },
        },
        new Layout
        {
            Class = 14,
            ClassName = "DECA",
            Type = 1,
            TypeName = "X/CW_X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.1545f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.4045f,
                    Pitch = 0.309f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = -0.4045f,
                    Pitch = -0.309f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = -0.1545f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0.1545f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 7,
                    Rotation = "CCW",
                    Roll = 0.4045f,
                    Pitch = -0.309f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 8,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 9,
                    TestOrder = 9,
                    Rotation = "CCW",
                    Roll = 0.4045f,
                    Pitch = 0.309f,
                },
                new Motor
                {
                    Number = 10,
                    TestOrder = 10,
                    Rotation = "CW",
                    Roll = 0.1545f,
                    Pitch = 0.5f,
                },
            },
        },
        new Layout
        {
            Class = 14,
            ClassName = "DECA",
            Type = 14,
            TypeName = "X/CW_X",
            Motors = new[]
            {
                new Motor
                {
                    Number = 1,
                    TestOrder = 1,
                    Rotation = "CCW",
                    Roll = -0.1545f,
                    Pitch = 0.5f,
                },
                new Motor
                {
                    Number = 2,
                    TestOrder = 2,
                    Rotation = "CW",
                    Roll = -0.4045f,
                    Pitch = 0.309f,
                },
                new Motor
                {
                    Number = 3,
                    TestOrder = 3,
                    Rotation = "CCW",
                    Roll = -0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 4,
                    TestOrder = 4,
                    Rotation = "CW",
                    Roll = -0.4045f,
                    Pitch = -0.309f,
                },
                new Motor
                {
                    Number = 5,
                    TestOrder = 5,
                    Rotation = "CCW",
                    Roll = -0.1545f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 6,
                    TestOrder = 6,
                    Rotation = "CW",
                    Roll = 0.1545f,
                    Pitch = -0.5f,
                },
                new Motor
                {
                    Number = 7,
                    TestOrder = 7,
                    Rotation = "CCW",
                    Roll = 0.4045f,
                    Pitch = -0.309f,
                },
                new Motor
                {
                    Number = 8,
                    TestOrder = 8,
                    Rotation = "CW",
                    Roll = 0.5f,
                    Pitch = -0f,
                },
                new Motor
                {
                    Number = 9,
                    TestOrder = 9,
                    Rotation = "CCW",
                    Roll = 0.4045f,
                    Pitch = 0.309f,
                },
                new Motor
                {
                    Number = 10,
                    TestOrder = 10,
                    Rotation = "CW",
                    Roll = 0.1545f,
                    Pitch = 0.5f,
                },
            },
        },
    };
}