using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Client
{
    public interface IMavParamValueConverter
    {
        float ConvertToMavlinkUnionToParamValue(MavParam param);
        void ConvertFromMavlinkUnionToParamValue(float payloadParamValue, MavParamType payloadParamType, out float? floatVal, out long? longVal);
    }
}