using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2ParamValueList : ISizedSpanSerializable
    {
        public Pv2ParamValueList()
        {
        }

        public Pv2ParamValueList(IEnumerable<Pv2ParamValueItem> items)
        {
            foreach (var item in items) Items.Add(item);
        }

        public IList<Pv2ParamValueItem> Items { get; } = new List<Pv2ParamValueItem>();

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var count = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            for (var i = 0; i < count; i++)
            {
                var item = new Pv2ParamValueItem();
                item.Deserialize(ref buffer);
                Items.Add(item);
            }
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WritePackedUnsignedInteger(ref buffer, (uint)Items.Count);
            foreach (var item in Items)
            {
                Debug.Assert(item != null, nameof(item) + " != null");
                item.Serialize(ref buffer);
            }
        }

        public int GetByteSize()
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger((uint)Items.Count) +
                   Items.Sum(_ => _.GetByteSize());
        }
    }
}
