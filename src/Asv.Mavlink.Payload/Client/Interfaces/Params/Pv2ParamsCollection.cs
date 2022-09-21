using System;
using System.Collections.Generic;
using System.Linq;

namespace Asv.Mavlink.Payload
{
    public class Pv2ParamsCollection
    {
        private readonly Pv2ParamValueAndTypePair[] _list;
        private readonly object _sync = new();
        private readonly Dictionary<string, Pv2ParamValueAndTypePair> _dict;

        internal Pv2ParamsCollection(int count)
        {
            _list = new Pv2ParamValueAndTypePair[count];
            _dict = new Dictionary<string, Pv2ParamValueAndTypePair>(count);
        }

        public int Count => _list.Length;

        public Pv2ParamValueAndTypePair this[Pv2ParamType type] => this[type.FullName];

        public Pv2ParamValueAndTypePair this[string paramFullName]
        {
            get
            {
                lock (_sync)
                {
                    return _dict.TryGetValue(paramFullName, out var result) == false ? null : result;
                }
            }
        }

        public Pv2ParamValueAndTypePair this[uint index]
        {
            get
            {
                lock (_sync)
                {
                    if (index >= _list.Length) throw new Exception($"Wrong param index {index}. Size {_list.Length}");
                    return _list[index];
                }
            }
        }

        internal void Add(Pv2ParamValueAndTypePair value)
        {
            lock (_sync)
            {
                _list[value.Index] = value;
                _dict.Add(value.Type.FullName, value);
            }
        }

        public List<Pv2ParamType> CopyToList()
        {
            lock (_sync)
            {
                return _list.OrderBy(_ => _.Index).Select(_ => _.Type).ToList();
            }
        }

        public List<Pv2ParamValueAndTypePair> CopyAllToList()
        {
            lock (_sync)
            {
                return _list.OrderBy(_ => _.Index).ToList();
            }
        }
    }
}
