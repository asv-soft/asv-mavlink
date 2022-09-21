using System;

namespace Asv.Mavlink.Payload
{
    [Flags]
    public enum Pv2ParamFlags : uint
    {
        NoFlags = 0b0000_0000,
        ReadOnly = 0b0000_0001,
        RebootRequired = 0b0000_0010,
        Hidden = 0b0000_0100,
        ForAdvancedUsers = 0b0000_1000,
        Reserved5 = 0b0001_0000,
        Reserved6 = 0b0010_0000,
        Reserved7 = 0b0100_0000,
        Reserved8 = 0b1000_0000
    }
}
