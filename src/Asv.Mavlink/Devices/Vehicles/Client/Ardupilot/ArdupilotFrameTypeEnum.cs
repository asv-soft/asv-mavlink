namespace Asv.Mavlink
{
    public enum ArdupilotFrameTypeEnum
    {
        Plus = 0,
        X = 1,
        V = 2,
        H = 3,
        VTail = 4,
        ATail = 5,
        Y6B = 10,
        Y6F = 11,
        BetaFlightX = 12,
        DJIX = 13,
        ClockwiseX = 14,
        I = 15,
        BetaFlightXReversed = 18,
        Y4 = 19,
        Unknown,
    }

    public enum ArdupilotFrameClassEnum
    {
        Undefined = 0,
        Quad = 1,
        Hexa = 2,
        Octa = 3,
        OctaQuad = 4,
        Y6 = 5,
        Heli = 6,
        Tri = 7,
        SingleCopter = 8,
        CoaxCopter = 9,
        BiCopter = 10,
        Heli_Dual = 11,
        DodecaHexa = 12,
        HeliQuad = 13,
        Deca = 14,
        ScriptingMatrix = 15,
        DoF6Scripting = 16,
        DynamicScriptingMatrix = 17,
    }


    public static class ArdupilotFrameTypeHelper
    {
        public static ArdupilotFrameTypeEnum ParseFrameType(long? frameType)
        {
            if (frameType == null) return ArdupilotFrameTypeEnum.Unknown;
            return (ArdupilotFrameTypeEnum)frameType.Value;
        }

        public static ArdupilotFrameClassEnum ParseFrameClass(long? frameType)
        {
            if (frameType == null) return ArdupilotFrameClassEnum.Undefined;
            return (ArdupilotFrameClassEnum)frameType.Value;
        }

        public static string GenerateName(ArdupilotFrameClassEnum frameClass, ArdupilotFrameTypeEnum frameType, int serialNumber)
        {
            return $"{frameClass:G} {frameType:G} [{serialNumber:D5}]";
        }
    }
}
