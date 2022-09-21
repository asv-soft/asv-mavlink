using System.Collections.Generic;

namespace Asv.Mavlink.Payload
{
    public interface IPv2BaseDescriptionStore
    {
        bool TryGetFromCache(uint hash, uint count, out IList<(Pv2WorkModeInfo, IList<Pv2WorkModeStatusInfo>)> list);
        void Save(uint hash, IList<(Pv2WorkModeInfo, IList<Pv2WorkModeStatusInfo>)> paramsList);
    }
}
