using System.Collections.Generic;

namespace Asv.Mavlink.Payload
{
    public interface IPv2CfgDescriptionStore
    {
        bool TryGetFromCache(uint hash, uint count, out List<Pv2ParamType> paramsList);
        void Save(uint hash, List<Pv2ParamType> paramsList);
    }
}
