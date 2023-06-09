using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public interface IMavParamValueConverter
    {
        float ConvertToMavlinkUnion(decimal value, MavParamType type);
        decimal ConvertFromMavlinkUnion(float value, MavParamType type);
    }
}