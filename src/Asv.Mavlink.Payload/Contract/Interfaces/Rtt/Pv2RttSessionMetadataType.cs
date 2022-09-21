using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2RttSessionMetadataType : ISizedSpanSerializable
    {
        public bool IsEnabled { get; set; }
        public SessionMetadata Session { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            IsEnabled = BinSerialize.ReadBool(ref buffer);
            if (!IsEnabled) return;
            Session = new SessionMetadata();
            Session.Deserialize(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteBool(ref buffer, IsEnabled);
            if (IsEnabled && Session != null) Session.Serialize(ref buffer);
        }

        public int GetByteSize()
        {
            return 1 + (IsEnabled && Session != null ? Session.GetByteSize() : 0);
        }

        public override string ToString()
        {
            return $"enabled:{IsEnabled}, {Session}";
        }
    }
}
