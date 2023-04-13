using Asv.Mavlink.Client;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface IMavParamValueConverter
    {
        float ConvertToMavlinkUnionToParamValue(decimal value, MavParamType type);
        decimal ConvertFromMavlinkUnionToParamValue(float value, MavParamType type);
    }
}