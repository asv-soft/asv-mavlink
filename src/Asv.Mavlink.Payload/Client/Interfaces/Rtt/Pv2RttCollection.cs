using System;
using System.Collections.Generic;
using System.Linq;

namespace Asv.Mavlink.Payload
{
    public class Pv2RttCollection
    {
        private readonly Dictionary<uint, Pv2RttFieldDesc> _fieldDescDictionary = new();
        private readonly Dictionary<ushort, (Pv2RttRecordDesc, List<Pv2RttFieldDesc>)> _rttDescriptions = new();
        private readonly object _sync = new();

        internal Pv2RttCollection(IList<Pv2RttRecordDesc> records, IList<Pv2RttFieldDesc> fields)
        {
            Records = records;
            var desc = records.ToDictionary(_ => _.Id, _ => _);
            foreach (var group in fields.GroupBy(_ => _.GroupId))
            {
                var dict = new List<Pv2RttFieldDesc>();
                if (desc.TryGetValue(group.Key, out var record) == false)
                    throw new Exception($"Couldn't find record description with id={group.Key}");
                _rttDescriptions.Add(group.Key, (record, dict));
                foreach (var item in group)
                {
                    _fieldDescDictionary.Add(item.FullId, item);
                    dict.Add(item);
                }
            }
        }

        public IList<Pv2RttRecordDesc> Records { get; }


        public bool TryGetFieldsForRecordWithId(ushort recordId, out Pv2RttRecordDesc record,
            out IList<Pv2RttFieldDesc> fields)
        {
            if (_rttDescriptions.TryGetValue(recordId, out var rec) == false)
            {
                record = null;
                fields = null;
                return false;
            }

            record = rec.Item1;
            fields = rec.Item2;
            return true;
        }

        public bool TryGetFieldsWithId(uint fieldFullId, out Pv2RttFieldDesc fieldDesc)
        {
            if (_fieldDescDictionary.TryGetValue(fieldFullId, out var rec) == false)
            {
                fieldDesc = null;
                return false;
            }

            fieldDesc = rec;
            return true;
        }

        public bool TryGetRecordForFieldWithId(uint fieldId, out Pv2RttFieldDesc field, out Pv2RttRecordDesc rec)
        {
            if (_fieldDescDictionary.TryGetValue(fieldId, out field))
                if (_rttDescriptions.TryGetValue(field.GroupId, out var record))
                {
                    rec = record.Item1;
                    return true;
                }

            rec = null;
            return false;
        }
    }
}
