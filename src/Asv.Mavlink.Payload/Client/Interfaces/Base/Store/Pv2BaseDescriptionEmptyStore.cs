using System.Collections.Generic;

namespace Asv.Mavlink.Payload
{
    public class Pv2BaseDescriptionEmptyStore : IPv2BaseDescriptionStore
    {
        public bool TryGetFromCache(uint hash, uint count, out IList<(Pv2WorkModeInfo, IList<Pv2WorkModeStatusInfo>)> list)
        {
            list = null;
            return false;
        }

        public void Save(uint hash, IList<(Pv2WorkModeInfo, IList<Pv2WorkModeStatusInfo>)> paramsList)
        {
            
        }
    }
}
