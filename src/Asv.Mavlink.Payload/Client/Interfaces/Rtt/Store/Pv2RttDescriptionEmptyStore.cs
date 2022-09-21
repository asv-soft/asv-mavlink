using System.Collections.Generic;

namespace Asv.Mavlink.Payload
{
    public class Pv2RttDescriptionEmptyStore : IPv2RttDescriptionStore
    {
        public bool TyGetFromCache(uint hash, uint recordsCount, uint fieldsCount, out List<Pv2RttRecordDesc> records,
            out List<Pv2RttFieldDesc> fields)
        {
            records = null;
            fields = null;
            return false;
        }

        public void Save(uint hash, List<Pv2RttRecordDesc> records, List<Pv2RttFieldDesc> fields)
        {
        }
    }
}
