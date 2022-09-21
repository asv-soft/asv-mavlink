using System.Collections.Generic;
using Asv.Mavlink.Payload.Digits;

namespace Asv.Mavlink.Payload
{
    public static class Pv2DeviceParams
    {
        public const string GroupName = "Mavlink";

        public static readonly Pv2UIntParamType SystemId =
            new(nameof(SystemId), "Device mavlink SystemId", GroupName, "{0:D3}", string.Empty, 0, 255, 14,
                Pv2ParamFlags.RebootRequired);

        public static readonly Pv2UIntParamType ComponentId =
            new(nameof(ComponentId), "Device mavlink ComponentId", GroupName, "{0:D3}", string.Empty, 0, 255, 14,
                Pv2ParamFlags.RebootRequired);

        public static readonly Pv2UIntParamType NetworkId =
            new(nameof(NetworkId), "Device mavlink NetworkId", GroupName, "{0:D3}", string.Empty, 0, 255, 0,
                Pv2ParamFlags.RebootRequired);

        public static IEnumerable<Pv2ParamType> Params
        {
            get
            {
                yield return SystemId;
                yield return ComponentId;
                yield return NetworkId;
            }
        }
    }
}
