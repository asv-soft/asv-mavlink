using System.Collections.Generic;

namespace Asv.Mavlink.Payload
{
    public class Pv2CfgDescriptionEmptyStore : IPv2CfgDescriptionStore
    {
        public static IPv2CfgDescriptionStore Default { get; } = new Pv2CfgDescriptionEmptyStore();

        public bool TryGetFromCache(uint hash, uint count, out List<Pv2ParamType> paramsList)
        {
            paramsList = null;
            return false;
        }

        public void Save(uint hash, List<Pv2ParamType> paramsList)
        {
        }
    }
}
