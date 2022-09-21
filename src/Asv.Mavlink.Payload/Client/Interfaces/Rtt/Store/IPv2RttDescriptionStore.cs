using System.Collections.Generic;

namespace Asv.Mavlink.Payload
{
    public interface IPv2RttDescriptionStore
    {
        bool TyGetFromCache(uint hash, uint recordsCount, uint fieldsCount, out List<Pv2RttRecordDesc> records,
            out List<Pv2RttFieldDesc> fields);

        void Save(uint hash, List<Pv2RttRecordDesc> records, List<Pv2RttFieldDesc> fields);
    }
}
